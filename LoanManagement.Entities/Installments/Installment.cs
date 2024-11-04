using LoanManagementSystem.Entities.Loans;

namespace LoanManagementSystem.Entities.Installments;

public class Installment
{
    public int Id { get; set; }
    public int LoanId { get; set; }
    public DateOnly ShouldPayDate { get; set; }
    public DateOnly? PaidDate { get; set; }
    public Loan Loan { get; set; } = default!;
}