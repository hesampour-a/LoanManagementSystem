using LoanManagementSystem.Persistence.Ef;
using LoanManagementSystem.Persistence.Ef.Admins;
using LoanManagementSystem.Persistence.Ef.LoanFormats;
using LoanManagementSystem.Persistence.Ef.UnitOfWorks;
using LoanManagementSystem.Services.LoanFormats;
using LoanManagementSystem.Services.LoanFormats.Contracts;

namespace LoanManagementSystem.TestTools.LoanFormats;

public static class LoanFormatServiceFactory
{
    public static LoanFormatAppService Generate(EfDataContext context)
    {
        var loanFormatRepository = new EFLoanFormatRepository(context);
        var adminRepository = new EFAdminRepository(context);
        var unitOfWork = new EfUnitOfWork(context);
        return new LoanFormatAppService(loanFormatRepository, adminRepository,
            unitOfWork);
    }
}