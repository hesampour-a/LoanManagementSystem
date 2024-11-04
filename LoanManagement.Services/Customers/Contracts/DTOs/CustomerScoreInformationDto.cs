using LoanManagementSystem.Entities.Customers;

namespace LoanManagementSystem.Services.Customers.Contracts.DTOs;

public class CustomerScoreInformationDto
{
    public decimal MonthlyIncome { get; set; }
    public decimal TotalAssetsValue { get; set; }
    public JobType JobType { get; set; }
    public bool HasLoanAndRepaidInTime { get; set; }
    public int LateRepaidInstallmentsCount { get; set; }
}