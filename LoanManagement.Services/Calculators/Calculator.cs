namespace LoanManagementSystem.Services.Calculators;

public static class Calculator
{
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