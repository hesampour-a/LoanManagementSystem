namespace LoanManagementSystem.Services.Loans.Contracts.DTOs;

public class GetMonthlyIncomeDto
{
    public decimal TotalInterestAmount { get; set; }
    public decimal TotalPenaltyAmount { get; set; }
}