using LoanManagementSystem.Entities.Installments;

namespace LoanManagementSystem.TestTools.Installments;

public static class InstallmentFactory
{
    public static Installment Generate(
        int loanId,
        DateOnly? loanDate = null,
        DateOnly? paidDate = null)
    {
        loanDate ??= DateOnly.FromDateTime(DateTime.Today.AddMonths(1));
        return new Installment
        {
            LoanId = loanId,
            ShouldPayDate = loanDate.Value,
            PaidDate = paidDate != null ? paidDate.Value : null,
        };
    }
}