using LoanManagementSystem.Persistence.Ef;
using LoanManagementSystem.Persistence.Ef.Admins;
using LoanManagementSystem.Persistence.Ef.Installments;
using LoanManagementSystem.Persistence.Ef.Loans;
using LoanManagementSystem.Persistence.Ef.UnitOfWorks;
using LoanManagementSystem.Services.Installments;

namespace LoanManagementSystem.TestTools.Installments;

public static class InstallmentServiceFactory
{
    public static InstallmentAppService Generate(EfDataContext context)
    {
        var installmentRepository = new EFInstallmentRepository(context);
        var loanRepository = new EFLoanRepository(context);
        var adminRepository = new EFAdminRepository(context);
        var unitOfWork = new EfUnitOfWork(context);
        return new InstallmentAppService(
            installmentRepository,
            loanRepository,
            adminRepository,
            unitOfWork);
    }
}