using LoanManagementSystem.Entities.Loans;

namespace LoanManagementSystem.Entities.Customers;

public class Customer
{
    public int Id { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string PhoneNumber { get; set; }
    public required string NationalCode { get; set; }
    public required string Email { get; set; }
    public string? IdentityDocument { get; set; }
    public bool IsVerified { get; set; }

    public CustomerFinancialInformation? FinancialInformation { get; set; } =
        default!;

    public List<Loan> Loans { get; set; } = [];
}