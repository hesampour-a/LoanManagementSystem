namespace LoanManagementSystem.Entities.Customers;

public class CustomerFinancialInformation
{
    public int Id { get; set; }
    public decimal MonthlyIncome { get; set; }
    public decimal TotalAssetsValue { get; set; }
    public JobType JobType { get; set; }
    public int CustomerId { get; set; }
    public Customer Customer { get; set; } = default!;
}

public enum JobType
{
    Government = 1,
    Free = 2,
    Unemployed = 3
}