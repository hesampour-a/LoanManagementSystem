using LoanManagementSystem.Entities.Customers;
using LoanManagementSystem.Services.Customers.Contracts;
using LoanManagementSystem.Services.Customers.Contracts.DTOs;

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

    public static int CustomerLoanScore(CustomerScoreInformationDto dto,
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