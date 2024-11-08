using System.ComponentModel.DataAnnotations;
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
    [Range(0, 100)]
    public int ValidationScore { get; set; }
    public Customer Customer { get; set; } = default!;
    public LoanFormat LoanFormat { get; set; } = default!;
    public List<Installment> Installments { get; set; } = [];
}