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
}