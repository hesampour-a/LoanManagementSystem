using LoanManagementSystem.Persistence.Ef;
using LoanManagementSystem.Persistence.Ef.LoanFormats;
using LoanManagementSystem.Services.LoanFormats.Contracts;

namespace LoanManagementSystem.TestTools.LoanFormats;

public static class LoanFormatQueryFactory
{
    public static EFLoanFormatQuery Generate(EfDataContext context)
    {
        return new EFLoanFormatQuery(context);
    }
}