namespace LoanManagementSystem.Services.Loans.Contracts.DTOs;

public class GetAllPendingLoansDto
{
    public int Id { get; set; }
    public int CustomerId { get; set; }
    public int LoanFormatId { get; set; }
    public int ValidationScore { get; set; }
}