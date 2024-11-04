using LoanManagementSystem.Entities.Loans;
using LoanManagementSystem.Services.Admins.Contracts;
using LoanManagementSystem.Services.Admins.Exceptions;
using LoanManagementSystem.Services.Installments.Contracts;
using LoanManagementSystem.Services.Installments.Exceptions;
using LoanManagementSystem.Services.Loans.Contracts;
using LoanManagementSystem.Services.UnitOfWorks;

namespace LoanManagementSystem.Services.Installments;

public class InstallmentAppService(
    InstallmentRepository installmentRepository,
    LoanRepository loanRepository,
    AdminRepository adminRepository,
    UnitOfWork unitOfWork) : InstallmentService
{
    public void Repayment(int adminId, int installmentId)
    {
        var admin = adminRepository.FindById(adminId)
                    ?? throw new AdminNotFoundException();

        var loan =
            loanRepository.FindByInstallmentIdIncludeInstallments(
                installmentId)
            ?? throw new InstallmentNotFoundException();

        var installment = loan.Installments.First(i => i.Id == installmentId);
        if (installment.PaidDate.HasValue)
        {
            throw new InstallmentAlreadyPaiedException();
        }

        installment.PaidDate = DateOnly.FromDateTime(DateTime.Today);


        if (!loan.Installments.Any(i => i.PaidDate == null))
        {
            loan.LoanStatus = LoanStatus.Closed;
        }
        else if (loan.Installments.Any(i =>
                     i.ShouldPayDate < DateOnly.FromDateTime(DateTime.Today)))
        {
            loan.LoanStatus = LoanStatus.Deferred;
        }
        else
        {
            loan.LoanStatus = LoanStatus.Repaymenting;
        }

        loanRepository.Update(loan);
        installmentRepository.Update(installment);
        unitOfWork.Save();
    }
}