using System.Data.Entity.Infrastructure;

namespace Data
{
    #region snippet_IDbContextFactory
    public class SchoolContextFactory : IDbContextFactory<AppContext>
    {
        public AppContext Create()
        {
            return new Data.AppContext("Server=DESKTOP-82VF481\\SQLEXPRESS;Database=GroupBookings;Trusted_Connection=True;MultipleActiveResultSets=true");
        }
    }
    #endregion
}
