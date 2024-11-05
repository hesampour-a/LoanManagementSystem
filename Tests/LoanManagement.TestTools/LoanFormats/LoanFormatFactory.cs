using LoanManagementSystem.Entities.LoanFormats;


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
            InterestRate = InterestRate(installmentsCount),
            MonthlyInterestAmount =
                MonthlyInterestAmount(loanAmount, installmentsCount),
            MonthlyPenaltyAmount =
                MonthlyPenaltyAmount(loanAmount, installmentsCount),
            MonthlyRepayAmount =
                MonthlyRepayAmount(loanAmount, installmentsCount),
        };
    }

    public static decimal InterestRate(int installmentsCount)
    {
        return Math.Round(installmentsCount <= 12 ? 0.15m : 0.20m, 2);
    }

    public static decimal MonthlyInterestAmount(decimal amount,
        int installmentsCount)
    {
        return Math.Round((InterestRate(installmentsCount) / 12) * amount, 2);
    }

    public static decimal MonthlyRepayAmount(decimal amount,
        int installmentsCount)
    {
        return Math.Round(amount / installmentsCount, 2);
    }

    public static decimal MonthlyPenaltyAmount(decimal amount,
        int installmentsCount)
    {
        return Math.Round(0.02m *
                          ((MonthlyInterestAmount(amount, installmentsCount)) +
                           (MonthlyRepayAmount(amount, installmentsCount))), 2);
    }
}