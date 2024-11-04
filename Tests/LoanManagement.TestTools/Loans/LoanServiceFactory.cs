using LoanManagementSystem.Persistence.Ef;
using LoanManagementSystem.Persistence.Ef.Admins;
using LoanManagementSystem.Persistence.Ef.Customers;
using LoanManagementSystem.Persistence.Ef.LoanFormats;
using LoanManagementSystem.Persistence.Ef.Loans;
using LoanManagementSystem.Persistence.Ef.UnitOfWorks;
using LoanManagementSystem.Services.Loans;

namespace LoanManagementSystem.TestTools.Loans;

public static class LoanServiceFactory
{
    public static LoanAppService Generate(EfDataContext context)
    {
        var loanRepository = new EFLoanRepository(context);
        var adminRepository = new EFAdminRepository(context);
        var customerRepository = new EFCustomerRepository(context);
        var loanFormatRepository = new EFLoanFormatRepository(context);
        var unitOfWork = new EfUnitOfWork(context);
        return new LoanAppService(
            loanRepository,
            adminRepository,
            customerRepository,
            loanFormatRepository,
            unitOfWork);
    }
}