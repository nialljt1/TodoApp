using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Data.Model;
using Data.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Migrate
{
    internal class Program
    {
        private const string AutoUpdateArg = "-AUTOUPDATE";
        private const string BackupArg = "-BACKUP";
        private const string InitialDatabase = "$InitialDatabase";
        private const string LatestMigration = "*";
        private const string NoSeedArg = "-NOSEED";
        private const string NoUsersArg = "-NOUSERS";
        private static bool _autoUpdate = false;
        private static bool _createBackup = false;
        private static bool _pause = false;
        private static bool _skipSeedData = false;
        private static bool _skipUserConfiguration = false;
        private static bool _useLatestMigration = false;

        private static void CheckSqlAuthenticationMode()
        {
            using (var ctx = new Data.Model.AppContext())
            {
                var isIntegratedSecurityOnly = ctx.Database.SqlQuery<int>("select serverproperty('IsIntegratedSecurityOnly')").First();

                if (isIntegratedSecurityOnly == 1)
                {
                    Console.WriteLine(
                        "WARNING: SQL Server authentication is configured for Windows Authentication mode only."
                        + " Change this to SQL Server and Windows Authentication mode.");

                    _pause = true;
                }
            }
        }

        private static void CheckTransactionIsolationLevel()
        {
            using (var ctx = new Data.Model.AppContext())
            {
                var isReadCommittedSnapshot = ctx.Database.SqlQuery<bool>("select is_read_committed_snapshot_on from sys.databases where name = db_name();").First();

                if (isReadCommittedSnapshot == false)
                {
                    Console.WriteLine(
                        "WARNING: Read committed snapshot is not enabled for this database."
                        + " You must enable this feature.");

                    _pause = true;
                }
            }
        }

        private static void CreateBackup(DbContext ctx)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("Creating database backup ({0}).", ctx.Database.Connection.Database);
            Console.ResetColor();

            var originalTimeout = ctx.Database.CommandTimeout;

            try
            {
                ctx.Database.CommandTimeout = (int)TimeSpan.FromMinutes(10).TotalSeconds;

                new DatabaseBackup(ctx)
                    .WithRelativePath("Misc", fallbackToDefaultPath: true)
                    .WithCopyOnly()
                    .WithCompression()
                    .Backup();
            }
            finally
            {
                ctx.Database.CommandTimeout = originalTimeout;
            }

            Console.WriteLine();
        }

        private static string GetTargetMigration(IList<string> migrations)
        {
            Argument.IsNotNull(migrations, "migrations");

            var target = ReadTargetMigration();
            int targetMigrationOffset;

            if (target != null && int.TryParse(target, out targetMigrationOffset))
            {
                if (targetMigrationOffset < 0)
                {
                    if (migrations.Any())
                    {
                        var targetMigrationIndex = Math.Max(migrations.Count + targetMigrationOffset - 1, 0);
                        target = migrations[targetMigrationIndex];

                        Console.WriteLine("Using target migration: {0}", target);
                    }
                }
            }

            return target;
        }

        private static void Main(string[] args)
        {
            // Add framework services.
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            Database.SetInitializer<AppContext>(null);
            ProcessArguments(args);

            try
            {
                UpdateAppDatabase();
                CheckSqlAuthenticationMode();
                CheckTransactionIsolationLevel();
            }
            catch
            {
                _pause = true;
                throw;
            }

            if (_pause && !_autoUpdate)
            {
                Console.WriteLine();
                Console.WriteLine("Press any key to exit...");
                Console.ReadKey();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine();
                Console.WriteLine("DONE");
                Console.ResetColor();
            }
        }

        private static void ProcessArguments(string[] args)
        {
            if (args != null && args.Length > 0)
            {
                var normalisedArgs = args
                    .Where(o => !string.IsNullOrWhiteSpace(o))
                    .Select(o => o.ToUpperInvariant())
                    .ToArray();

                foreach (var arg in normalisedArgs)
                {
                    switch (arg)
                    {
                        case AutoUpdateArg:
                            _autoUpdate = true;
                            _useLatestMigration = true;
                            break;

                        case NoSeedArg:
                            _skipSeedData = true;
                            break;

                        case NoUsersArg:
                            _skipUserConfiguration = true;
                            break;

                        case BackupArg:
                            _createBackup = true;
                            break;

                        default:
                            Console.WriteLine(
                                "WARNING: Argument '{0}' is not recognised and will be ignored.", arg);
                            break;
                    }
                }
            }
        }

        private static string ReadTargetMigration()
        {
            if (_useLatestMigration)
            {
                Console.WriteLine(LatestMigration);

                return null;
            }

            var target = Console.ReadLine();

            if (target != null)
            {
                target = target.Trim();

                if (target.Length > 0)
                {
                    if (target == LatestMigration)
                    {
                        _useLatestMigration = true;
                        return null;
                    }

                    if (string.Equals(target, InitialDatabase, StringComparison.OrdinalIgnoreCase))
                    {
                        target = DbMigrator.InitialDatabase;
                    }

                    return target;
                }
            }

            return null;
        }

        private static void ScriptMigrationHistory(DbContext context)
        {
            var outputFileName = string.Format("{0}.MigrationHistory.sql", context.Database.Connection.Database);
            var outputPath = Path.Combine(Environment.CurrentDirectory, outputFileName);

            using (var writer = File.CreateText(outputPath))
            {
                writer.WriteLine(@"if not exists (select * from sys.objects where object_id = object_id(N'[dbo].[__MigrationHistory]') and type in (N'U')) begin
	create table [dbo].[__MigrationHistory] (
		[MigrationId] [nvarchar](150) not null,
		[ContextKey] [nvarchar](300) not null,
		[Model] [varbinary](max) not null,
		[ProductVersion] [nvarchar](32) not null,
		constraint [PK_dbo.__MigrationHistory] primary key clustered (
			[MigrationId] asc,
			[ContextKey] asc
		) with (pad_index = off, statistics_norecompute = off, ignore_dup_key = off, allow_row_locks = on, allow_page_locks = on) on [Primary]
	) on [Primary] textimage_on [Primary]
end
go");

                using (var command = context.Database.Connection.CreateCommand())
                {
                    try
                    {
                        command.CommandText = "select MigrationId, ContextKey, Model, ProductVersion from dbo.__MigrationHistory order by MigrationId";
                        command.Connection.Open();

                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var migrationId = reader.GetString(0);
                                var contextKey = reader.GetString(1);
                                var model = (byte[])reader.GetValue(2);
                                var modelHexString = "0x" + BitConverter.ToString(model).Replace("-", string.Empty);
                                var productVersion = reader.GetString(3);

                                writer.WriteLine("if not exists (select top 1 null from dbo.__MigrationHistory where MigrationId = N'{0}' and ContextKey = N'{1}') begin", migrationId, contextKey);
                                writer.WriteLine("    insert into dbo.__MigrationHistory (MigrationId, ContextKey, Model, ProductVersion) values (N'{0}', N'{1}', {2}, N'{3}');", migrationId, contextKey, modelHexString, productVersion);
                                writer.WriteLine("end");
                                writer.WriteLine("go");
                            }
                        }
                    }
                    finally
                    {
                        command.Connection.Close();
                    }
                }
            }
        }

        private static void UpdateAppDatabase()
        {
            using (var ctx = new AppContext())
            {
                if (_createBackup && ctx.Database.Exists())
                {
                    CreateBackup(ctx);
                }

                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Updating application database ({0}).", ctx.Database.Connection.Database);
                Console.ResetColor();
            }

            Console.Write("Enter target migration: ");

            var target = GetTargetMigration(Migrations.App().GetMigrations());

            using (var ctx = new AppContext())
            {
                if (!ctx.Database.Exists())
                {
                    Migrations.App().Migrate(DbMigrator.InitialDatabase, seedDatabase: !_skipSeedData);
                }

                if (!_skipUserConfiguration)
                {
                    try
                    {
                        ctx.Database.ExecuteSqlCommand("if not exists (select * from master.dbo.syslogins where name = N'AHDB-Potato-Data-Centre') create login [AHDB-Potato-Data-Centre] with password = 'gUpaTregus2ewreyevuSwameyawREc';");
                        ctx.Database.ExecuteSqlCommand("if not exists (select * from sys.sysusers where name = N'AHDB-Potato-Data-Centre') create user [AHDB-Potato-Data-Centre] for login [AHDB-Potato-Data-Centre];");
                        ctx.Database.ExecuteSqlCommand("if not exists (select * from sys.database_principals where type = 'R' and name = N'Application') create role Application;");

                        ctx.Database.ExecuteSqlCommand("exec sp_addrolemember db_datareader, [AHDB-Potato-Data-Centre];");
                        ctx.Database.ExecuteSqlCommand("exec sp_addrolemember db_datawriter, [AHDB-Potato-Data-Centre];");
                        ctx.Database.ExecuteSqlCommand("exec sp_addrolemember Application, [AHDB-Potato-Data-Centre];");
                        ctx.Database.ExecuteSqlCommand("grant create schema to [Application];");
                        ctx.Database.ExecuteSqlCommand("grant create table to [Application];");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Failed to configure database users and roles: {0}", ex.Message);
                        Console.WriteLine("Continuing with migration...");

                        _pause = true;
                    }
                }
            }

            Migrations.App().Migrate(target, seedDatabase: !_skipSeedData);

            if (target != DbMigrator.InitialDatabase
                    && !_autoUpdate)
            {
                using (var ctx = new AppContext())
                {
                    ScriptMigrationHistory(ctx);
                }
            }

            Console.WriteLine();
        }
    }
}
