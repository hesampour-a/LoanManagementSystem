using LoanManagementSystem.Entities.LoanFormats;
using LoanManagementSystem.Services.LoanFormats.Contracts;

namespace LoanManagementSystem.Persistence.Ef.LoanFormats;

public class EFLoanFormatQuery(EfDataContext context) : LoanFormatQuery
{
    public List<GetAllLoanFormatsDto> GetAll()
    {
        return context.Set<LoanFormat>().Select(l => new GetAllLoanFormatsDto
        {
            Id = l.Id,
            MonthlyRepayAmount = l.MonthlyRepayAmount,
            Amount = l.Amount,
            InstallmentsCount = l.InstallmentsCount,
            InterestRate = l.InterestRate,
            MonthlyInterestAmount = l.MonthlyInterestAmount,
            MonthlyPenaltyAmount = l.MonthlyPenaltyAmount,
        }).ToList();
    }
}