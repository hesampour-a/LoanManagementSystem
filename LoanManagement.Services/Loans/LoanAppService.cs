using LoanManagementSystem.Entities.Customers;
using LoanManagementSystem.Entities.Installments;
using LoanManagementSystem.Entities.Loans;
using LoanManagementSystem.Services.Admins.Contracts;
using LoanManagementSystem.Services.Admins.Exceptions;

using LoanManagementSystem.Services.Customers.Contracts;
using LoanManagementSystem.Services.Customers.Contracts.DTOs;
using LoanManagementSystem.Services.Customers.Exceptions;
using LoanManagementSystem.Services.LoanFormats.Contracts;
using LoanManagementSystem.Services.LoanFormats.Exceptions;
using LoanManagementSystem.Services.Loans.Contracts;
using LoanManagementSystem.Services.Loans.Contracts.DTOs;
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
            ClaculateCustomerLoanScore(customerInformation,
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

        loan.LoanStatus = LoanStatus.Repaymenting;

        loanRepository.UpdateRangeLoanInstallments(loan);
        loanRepository.Update(loan);
        unitOfWork.Save();
    }

    public void UpdateDeferreds()
    {
        var deferreds = loanRepository.GetAllDeferreds();
        deferreds.ForEach(l => { l.LoanStatus = LoanStatus.Deferred; });
        loanRepository.UpdateRange(deferreds);
        unitOfWork.Save();
    }
    
    private int ClaculateCustomerLoanScore(CustomerScoreInformationDto dto,
        decimal loanAmount)
    {
        int score = 0;
        if (dto.HasLoanAndRepaidInTime)
        {
            score += 30;
        }

        score -= (dto.LateRepaidInstallmentsCount * 5);

        if (dto.MonthlyIncome > 10000000)
        {
            score += 10;
        }
        else if (5000000 <= dto.MonthlyIncome)
        {
            score += 5;
        }

        if (dto.JobType == JobType.Government)
        {
            score += 20;
        }
        else if (dto.JobType == JobType.Free)
        {
            score += 10;
        }

        decimal loanAmountAssetsValueRatio =
            Math.Round(loanAmount / dto.TotalAssetsValue, 2);

        if (loanAmountAssetsValueRatio < 0.5m)
        {
            score += 20;
        }
        else if (loanAmountAssetsValueRatio <= 0.7m)
        {
            score += 10;
        }

        return score;
    }
}