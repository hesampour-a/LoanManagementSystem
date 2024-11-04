using LoanManagementSystem.Entities.Installments;
using LoanManagementSystem.Entities.Loans;
using LoanManagementSystem.Services.Admins.Contracts;
using LoanManagementSystem.Services.Admins.Exceptions;
using LoanManagementSystem.Services.Calculators;
using LoanManagementSystem.Services.Customers.Contracts;
using LoanManagementSystem.Services.Customers.Exceptions;
using LoanManagementSystem.Services.LoanFormats.Contracts;
using LoanManagementSystem.Services.LoanFormats.Exceptions;
using LoanManagementSystem.Services.Loans.Contracts;
using LoanManagementSystem.Services.Loans.Exceptions;
using LoanManagementSystem.Services.UnitOfWorks;

namespace LoanManagementSystem.Services.Loans;

public class LoanAppService(
    LoanRepository loanRepository,
    AdminRepository adminRepository,
    CustomerRepository customerRepository,
    LoanFormatRepository loanFormatRepository,
    UnitOfWork unitOfWork) : LoanService
{
    public void Add(int customerId, AddLoanDto dto)
    {
        var customerInformation =
            customerRepository.FindScoreInformationById(customerId)
            ?? throw new CustomerNotFoundException();
        var loanFormat = loanFormatRepository.FindById(dto.LoanFormatId)
                         ?? throw new LoanFormatNotFoundException();

        int loanScore =
            Calculator.CustomerLoanScore(customerInformation,
                loanFormat.Amount);

        var loan = new Loan
        {
            LoanFormatId = loanFormat.Id,
            CustomerId = customerId,
            ValidationScore = loanScore,
            LoanStatus = loanScore < 60
                ? LoanStatus.Rejected
                : LoanStatus.Pending,
        };

        loanRepository.Add(loan);
        unitOfWork.Save();
    }

    public void Confirm(int adminId, int loanId)
    {
        var admin = adminRepository.FindById(adminId)
                    ?? throw new AdminNotFoundException();
        var loan = loanRepository.FindById(loanId)
                   ?? throw new LoanNotFoundException();

        if (loan.LoanStatus != LoanStatus.Pending)
        {
            throw new LoanIsNotInPendingStatusException();
        }

        if (loan.ValidationScore < 60)
        {
            throw new LoanValidationScoreIsNotEnoughException();
        }

        loan.LoanStatus = LoanStatus.Confirmed;
        loanRepository.Update(loan);
        unitOfWork.Save();
    }

    public void Reject(int adminId, int loanId)
    {
        var admin = adminRepository.FindById(adminId)
                    ?? throw new AdminNotFoundException();
        var loan = loanRepository.FindById(loanId)
                   ?? throw new LoanNotFoundException();
        if (loan.LoanStatus != LoanStatus.Pending)
        {
            throw new LoanIsNotInPendingStatusException();
        }

        loan.LoanStatus = LoanStatus.Rejected;
        loanRepository.Update(loan);
        unitOfWork.Save();
    }

    public void Pay(int adminId, int loanId)
    {
        var admin = adminRepository.FindById(adminId)
                    ?? throw new AdminNotFoundException();
        var loan = loanRepository.FindByIdWithLoanFormat(loanId)
                   ?? throw new LoanNotFoundException();
        if (loan.LoanStatus != LoanStatus.Confirmed)
        {
            throw new LoanIsNotInConfirmedStatusException();
        }

        for (int i = 1; i <= loan.LoanFormat.InstallmentsCount; i++)
        {
            loan.Installments.Add(new Installment
            {
                ShouldPayDate =
                    DateOnly.FromDateTime(DateTime.Now.AddMonths(i)),
            });
        }

        loan.LoanStatus = LoanStatus.Refunding;

        loanRepository.UpdateRangeLoanInstallments(loan);
        loanRepository.Update(loan);
        unitOfWork.Save();
    }
}