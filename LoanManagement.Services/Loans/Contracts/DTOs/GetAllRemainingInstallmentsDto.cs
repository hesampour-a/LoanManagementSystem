namespace LoanManagementSystem.Services.Loans.Contracts.DTOs;

public class GetAllRemainingInstallmentsDto
{
    public int Id { get; set; }
    public DateOnly ShouldPayDate { get; set; }
}