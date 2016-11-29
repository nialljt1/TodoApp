namespace Data.Infrastructure
{
    public interface IDbMigrationsConfiguration
    {
        bool SeedDataEnabled { get; set; }
    }
}
