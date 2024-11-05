namespace LoanManagementSystem.Services.Customers.Contracts.DTOs;

public class GetAllHighRisksDto
{
    public int CustomerId { get; set; }
    public int LateInstallmentsCount { get; set; }
}