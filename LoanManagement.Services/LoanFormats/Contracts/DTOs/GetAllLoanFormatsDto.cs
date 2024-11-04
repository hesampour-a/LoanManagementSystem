namespace LoanManagementSystem.Services.LoanFormats.Contracts.DTOs;

public class GetAllLoanFormatsDto
{
    public int Id { get; set; }
    public decimal Amount { get; set; }
    public int InstallmentsCount { get; set; }
    public decimal InterestRate { get; set; }
    public decimal MonthlyInterestAmount { get; set; }
    public decimal MonthlyRepayAmount { get; set; }
    public decimal MonthlyPenaltyAmount { get; set; }
}