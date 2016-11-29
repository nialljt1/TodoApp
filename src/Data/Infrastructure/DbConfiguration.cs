using System;

namespace Data.Infrastructure
{
    public class DbConfiguration : System.Data.Entity.DbConfiguration
    {
        public DbConfiguration()
        {
#if !DEBUG
            // Remove the default database initializer in RELEASE mode so that applying migrations
            // during deployments does not cause a model / schema mismatch to be detected.
            SetDatabaseInitializer<Core.AppModel.AppContext>(null);
#endif
        }
    }
}
