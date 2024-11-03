using LoanManagementSystem.Persistence.Ef;
using LoanManagementSystem.Persistence.Ef.Admins;
using LoanManagementSystem.Persistence.Ef.Customers;
using LoanManagementSystem.Persistence.Ef.UnitOfWorks;
using LoanManagementSystem.Services.Customers;

namespace LoanManagementSystem.TestTools.Customers;

public static class CustomerServiceFactory
{
    public static CustomerAppService Generate(EfDataContext context)
    {
        var customerQuery = new EFCustomerQuery(context);
        var adminRepository = new EFAdminRepository(context);
        var customerRepository = new EFCustomerRepository(context);
        var usnitOfWork = new EfUnitOfWork(context);
        return new CustomerAppService(customerRepository,customerQuery,adminRepository, usnitOfWork);
    }
}