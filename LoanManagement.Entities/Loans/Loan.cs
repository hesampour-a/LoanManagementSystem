using LoanManagementSystem.Entities.Customers;
using LoanManagementSystem.Entities.Installments;
using LoanManagementSystem.Entities.LoanFormats;

namespace LoanManagementSystem.Entities.Loans;

public class Loan
{
    public int Id { get; set; }
    public int CustomerId { get; set; }
    public int LoanFormatId { get; set; }
    public LoanStatus LoanStatus { get; set; }
    public int ValidationScore { get; set; }
    public Customer Customer { get; set; } = default!;
    public LoanFormat LoanFormat { get; set; } = default!;
    public List<Installment> Installments { get; set; } = [];
}

public enum LoanStatus
{
    Pending = 1,
    Confirmed = 2,
    Rejected = 3,
    Refunding = 4,
    Overdue = 5,
    Closed = 6,
}