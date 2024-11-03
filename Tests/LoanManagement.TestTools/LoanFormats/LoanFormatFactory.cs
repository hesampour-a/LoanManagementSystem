using LoanManagementSystem.Entities.LoanFormats;
using LoanManagementSystem.Services.Calculators;

namespace LoanManagementSystem.TestTools.LoanFormats;

public static class LoanFormatFactory
{
    public static LoanFormat Generate(decimal loanAmount = 100000000,
        int installmentsCount = 12)
    {
        return new LoanFormat
        {
            Amount = loanAmount,
            InstallmentsCount = installmentsCount,
            InterestRate = Calculator.InterestRate(installmentsCount),
            MonthlyInterestAmount =
                Calculator.MonthlyInterestAmount(loanAmount, installmentsCount),
            MonthlyPenaltyAmount =
                Calculator.MonthlyPenaltyAmount(loanAmount, installmentsCount),
            MonthlyRepayAmount =
                Calculator.MonthlyRepayAmount(loanAmount, installmentsCount),
        };
    }
}