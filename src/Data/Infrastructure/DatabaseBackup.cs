using System;
using System.Data.Entity;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using Enable.Infrastructure;

namespace Data.Infrastructure
{
    public class DatabaseBackup
    {
        private readonly DbContext _context;
        private readonly string _databaseName;

        private bool _compress;
        private bool _copyOnly;
        private bool _fallbackToDefaultPath;
        private string _filename;
        private string _name;
        private string _relativePath;

        public DatabaseBackup(DbContext context)
        {
            Argument.IsNotNull(context, "context");

            _context = context;

            var connectionStringBuilder = new SqlConnectionStringBuilder(context.Database.Connection.ConnectionString);
            _databaseName = connectionStringBuilder.InitialCatalog;

            _name = string.Concat(_databaseName, " - Application automatic backup");
        }

        public string DatabaseName
        {
            get
            {
                return _databaseName;
            }
        }

        public string Name
        {
            get
            {
                return _name;
            }
        }

        public void Backup()
        {
            try
            {
                ExecuteBackup();
            }
            catch (SqlException ex)
            {
                if (ex.Message.Contains("The system cannot find the path specified") && _fallbackToDefaultPath)
                {
                    ExecuteBackup(ignoreRelativePath: true);
                }
                else
                {
                    throw;
                }
            }
        }

        public DatabaseBackup WithCompression(bool compress = true)
        {
            _compress = compress;
            return this;
        }

        public DatabaseBackup WithCopyOnly(bool copyOnly = true)
        {
            _copyOnly = copyOnly;
            return this;
        }

        public DatabaseBackup WithFilename(string filename)
        {
            _filename = filename;
            return this;
        }

        public DatabaseBackup WithName(string name)
        {
            Argument.IsNotNullOrWhiteSpace(name, "name");

            _name = name;
            return this;
        }

        public DatabaseBackup WithRelativePath(string relativePath, bool fallbackToDefaultPath = false)
        {
            _relativePath = relativePath;
            _fallbackToDefaultPath = fallbackToDefaultPath;
            return this;
        }

        private string BuildCommand()
        {
            var commandBuilder = new StringBuilder();
            commandBuilder.Append("BACKUP DATABASE {0} TO DISK = {1} WITH NAME = {2}, NOINIT, SKIP, NOFORMAT, CHECKSUM");

            if (_compress)
            {
                commandBuilder.Append(", COMPRESSION");
            }

            if (_copyOnly)
            {
                commandBuilder.Append(", COPY_ONLY");
            }

            return commandBuilder.ToString();
        }

        private void ExecuteBackup(bool ignoreRelativePath = false)
        {
            var command = BuildCommand();
            var path = GetPath(ignoreRelativePath);

            _context.Database.ExecuteSqlCommand(TransactionalBehavior.DoNotEnsureTransaction, command, _databaseName, path, _name);
        }

        private string GetPath(bool ignoreRelativePath)
        {
            var path = _filename;

            if (string.IsNullOrWhiteSpace(path))
            {
                path = string.Format("{0}_{1:yyyyMMddHHmmss}.bak", _databaseName, DateTime.Now);
            }

            if (!ignoreRelativePath)
            {
                if (!string.IsNullOrWhiteSpace(_relativePath))
                {
                    path = Path.Combine(_relativePath, path);
                }
            }

            return path;
        }
    }
}
