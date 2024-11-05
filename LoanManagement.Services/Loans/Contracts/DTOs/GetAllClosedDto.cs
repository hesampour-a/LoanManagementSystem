namespace LoanManagementSystem.Services.Loans.Contracts.DTOs;

public class GetAllClosedDto
{
    public int Id { get; set; }
    public int CustomerId { get; set; }
    public int LoanFormatId { get; set; }
    public int ValidationScore { get; set; }
    public decimal LoanAmount { get; set; }
    public int InstallmentsCount { get; set; }
    public decimal TotalPenaltyAmount { get; set; }
}