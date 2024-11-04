using LoanManagementSystem.Entities.Customers;

namespace LoanManagementSystem.Services.Customers.Contracts;

public interface CustomerRepository
{
    void Add(Customer customer);
    bool IsDuplicated(string nationalCode);
    Customer? FindById(int customerId);
    void Update(Customer customer);
    void UpdateCustomerFinancialInformation(Customer customer);
    CustomerScoreInformationDto? FindScoreInformationById(int customerId);
}

public class CustomerScoreInformationDto
{
    public decimal MonthlyIncome { get; set; }
    public decimal TotalAssetsValue { get; set; }
    public JobType JobType { get; set; }
    public bool HasLoanAndRepaidInTime { get; set; }
    public int LateRepaidInstallmentsCount { get; set; }
}