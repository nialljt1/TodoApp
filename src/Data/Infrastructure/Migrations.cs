using System.Data.Entity.Infrastructure;
using System.Data.Entity.Migrations;
using System.Linq;
using Core.AppMigrations;
using Enable.Infrastructure;

namespace Data.Infrastructure
{
    public class Migrations
    {
        private readonly DbMigrationsConfiguration _config;

        private Migrations(DbMigrationsConfiguration config)
        {
            Argument.IsNotNull(config, "config");

            _config = config;
        }

        public static Migrations App(string connectionString = null)
        {
            var config = new AppConfiguration();

            if (connectionString != null)
            {
                config.TargetDatabase = new DbConnectionInfo(connectionString, "System.Data.SqlClient");
            }

            return new Migrations(config);
        }

        public string[] GetMigrations()
        {
            return GetMigrator().GetLocalMigrations().ToArray();
        }

        public void Migrate(string targetMigration = null, bool seedDatabase = true)
        {
            var migrator = GetMigrator();
            var configuration = migrator.Configuration as IDbMigrationsConfiguration;

            if (configuration != null)
            {
                configuration.SeedDataEnabled = seedDatabase;
            }

            if (!string.IsNullOrWhiteSpace(targetMigration))
            {
                migrator.Update(targetMigration);
            }
            else
            {
                migrator.Update();
            }
        }

        private DbMigrator GetMigrator()
        {
            return new DbMigrator(_config);
        }
    }
}
