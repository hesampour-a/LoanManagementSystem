using System.ComponentModel.DataAnnotations;

namespace LoanManagementSystem.Entities.Customers;

public class CustomerFinancialInformation
{
    public int Id { get; set; }

    [Range(0, (double)decimal.MaxValue)]
    public decimal MonthlyIncome { get; set; }

    [Range(0, (double)decimal.MaxValue)]
    public decimal TotalAssetsValue { get; set; }

    public JobType JobType { get; set; }
    public int CustomerId { get; set; }
    public Customer Customer { get; set; } = default!;
}