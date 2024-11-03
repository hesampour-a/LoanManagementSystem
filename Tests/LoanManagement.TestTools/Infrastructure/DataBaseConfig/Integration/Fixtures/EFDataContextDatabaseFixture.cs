using LoanManagementSystem.Persistence.Ef;
using Xunit;

namespace LoanManagementSystem.TestTools.Infrastructure.DataBaseConfig.Integration.
    Fixtures;

[Collection(nameof(ConfigurationFixture))]
public class EFDataContextDatabaseFixture : DatabaseFixture
{
    public static EfDataContext CreateDataContext(string tenantId)
    {
        var connectionString =
            new ConfigurationFixture().Value.ConnectionString;
        

        return new EfDataContext(
            $"Server=DESKTOP-5LA4REF;database=LoanManagement_Db;Trusted_Connection=true;MultipleActiveResultSets=true;TrustServerCertificate=True");
    }
}