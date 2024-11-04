namespace LoanManagementSystem.Services.LoanFormats.Contracts.DTOs;

public class AddLoanFormatDto
{
    public decimal Amount { get; set; }
    public int InstallmentsCount { get; set; }
}