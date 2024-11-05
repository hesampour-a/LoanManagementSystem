using LoanManagementSystem.Entities.Installments;
using LoanManagementSystem.Entities.LoanFormats;
using LoanManagementSystem.Entities.Loans;
using LoanManagementSystem.Services.Loans.Contracts;
using LoanManagementSystem.Services.Loans.Contracts.DTOs;

namespace LoanManagementSystem.Persistence.Ef.Loans;

public class EFLoanQuey(EfDataContext context) : LoanQuery
{
    public List<GetAllPendingLoansDto> GetAllPendings()
    {
        return context.Set<Loan>()
            .Where(l => l.LoanStatus == LoanStatus.Pending).Select(l =>
                new GetAllPendingLoansDto
                {
                    Id = l.Id,
                    LoanFormatId = l.LoanFormatId,
                    CustomerId = l.CustomerId,
                    ValidationScore = l.ValidationScore,
                }).ToList();
    }

    public List<GetAllRepaymentingAndDeferredDto>
        GetAllRepaymentingAndDeferred()
    {
        return context.Set<Loan>()
            .Where(l =>
                l.LoanStatus == LoanStatus.Repaymenting ||
                l.LoanStatus == LoanStatus.Deferred)
            .Select(l => new
            {
                Loan = l,
                LoanInstallments = l.Installments,
                TotalPaidInstallments = l.Installments
                    .Where(i => i.PaidDate.HasValue)
                    .Select(i => new
                    {
                        MonthlyInterestAmount =
                            l.LoanFormat.MonthlyInterestAmount,
                        MonthlyRepayAmount = l.LoanFormat.MonthlyRepayAmount,
                        MonthlyPenaltyAmount = i.PaidDate > i.ShouldPayDate
                            ? l.LoanFormat.MonthlyPenaltyAmount
                            : 0
                    }).ToList()
            })
            .AsEnumerable() // انتقال محاسبه به حافظه
            .Select(x => new GetAllRepaymentingAndDeferredDto
            {
                Id = x.Loan.Id,
                LoanFormatId = x.Loan.LoanFormatId,
                CustomerId = x.Loan.CustomerId,
                ValidationScore = x.Loan.ValidationScore,
                LoanStatus = x.Loan.LoanStatus,
                TotalPaidUntilNow = x.TotalPaidInstallments.Sum(p =>
                    p.MonthlyInterestAmount +
                    p.MonthlyRepayAmount +
                    p.MonthlyPenaltyAmount),
                RemainingInstallments = x.LoanInstallments
                    .Where(i => !(i.PaidDate.HasValue))
                    .Select(i => new GetAllRemainingInstallmentsDto
                    {
                        Id = i.Id,
                        ShouldPayDate = i.ShouldPayDate,
                    })
                    .ToList()
            })
            .ToList();
    }

    public GetMonthlyIncomeDto GetMonthlyIncome(DateOnly date)
    {
        var temp = context.Set<Loan>().Where(l =>
                l.Installments.Any(i => i.PaidDate.Value.Month == date.Month))
            .Select(l => new
            {
                TotalPaid = l.Installments
                    .Where(i => i.PaidDate.Value.Month == date.Month).Select(
                        i => new
                        {
                            MontlyIntrestAmount =
                                l.LoanFormat.MonthlyInterestAmount,
                            MontlyPenaltyAmount = i.PaidDate > i.ShouldPayDate
                                ? l.LoanFormat.MonthlyPenaltyAmount
                                : 0,
                        }).ToList()
            }).AsEnumerable()
            .Select(l => new
            {
                TotalInterestAmount =
                    l.TotalPaid.Sum(t => t.MontlyIntrestAmount),
                TotalPenaltyAmount =
                    l.TotalPaid.Sum(t => t.MontlyPenaltyAmount),
            }).ToList();

        return new GetMonthlyIncomeDto
        {
            TotalInterestAmount = temp.Sum(t => t.TotalInterestAmount),
            TotalPenaltyAmount = temp.Sum(t => t.TotalPenaltyAmount),
        };
    }

    public List<GetAllClosedDto> GetAllClosed()
    {
        return context.Set<Loan>().Where(l => l.LoanStatus == LoanStatus.Closed)
            .Select(l => new GetAllClosedDto
            {
                Id = l.Id,
                LoanFormatId = l.LoanFormatId,
                CustomerId = l.CustomerId,
                ValidationScore = l.ValidationScore,
                LoanAmount = l.LoanFormat.Amount,
                InstallmentsCount = l.Installments.Count,
                TotalPenaltyAmount =
                    l.Installments.Count(i => i.PaidDate > i.ShouldPayDate) *
                    l.LoanFormat.MonthlyPenaltyAmount,
            }).ToList();
    }
}