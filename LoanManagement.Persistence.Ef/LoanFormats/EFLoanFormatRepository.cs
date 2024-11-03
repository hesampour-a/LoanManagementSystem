using LoanManagementSystem.Entities.LoanFormats;
using LoanManagementSystem.Services.LoanFormats.Contracts;

namespace LoanManagementSystem.Persistence.Ef.LoanFormats;

public class EFLoanFormatRepository(EfDataContext context)
    : LoanFormatRepository
{
    public void Add(LoanFormat loanFormat)
    {
        context.Set<LoanFormat>().Add(loanFormat);
    }
}