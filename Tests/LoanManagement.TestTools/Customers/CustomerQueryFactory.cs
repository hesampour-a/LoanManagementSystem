using LoanManagementSystem.Persistence.Ef;
using LoanManagementSystem.Persistence.Ef.Customers;

namespace LoanManagementSystem.TestTools.Customers;

public static class CustomerQueryFactory
{
    public static EFCustomerQuery Generate(EfDataContext context)
    {
        return new EFCustomerQuery(context);
    }
}