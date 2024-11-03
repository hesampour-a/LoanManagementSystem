using LoanManagementSystem.Persistence.Ef.Customers;
using LoanManagementSystem.Services.Customers.Contracts;
using LoanManagementSystem.TestTools.Admins;
using LoanManagementSystem.TestTools.Customers;
using LoanManagementSystem.TestTools.Infrastructure.DataBaseConfig.Integration;
using Xunit;

namespace LoanManagementSystem.Service.Unit.Tests.Customers;

public class CustomerQueryTest : BusinessIntegrationTest
{
    private readonly CustomerQuery _sut;

    public CustomerQueryTest()
    {
        _sut = CustomerQueryFactory.Generate(SetupContext);
    }

    
}