using LoanManagementSystem.Entities.Installments;

namespace LoanManagementSystem.TestTools.Installments;

public static class InstallmentFactory
{
    public static Installment Generate(
        int loanId,
        DateOnly? shouldPayDate = null,
        DateOnly? paidDate = null)
    {
        shouldPayDate ??= DateOnly.FromDateTime(DateTime.Today.AddMonths(1));
        return new Installment
        {
            LoanId = loanId,
            ShouldPayDate = shouldPayDate.Value,
            PaidDate = paidDate != null ? paidDate.Value : null,
        };
    }
}