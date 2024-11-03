using LoanManagementSystem.Entities.Customers;

namespace LoanManagementSystem.Services.Customers.Contracts;

public interface CustomerRepository
{
    void Add(Customer customer);
    bool IsDuplicated(string nationalCode);
    Customer? FindById(int customerId);
    void Update(Customer customer);
    void AddFinancialInformation(CustomerFinancialInformation financialInformation);
    void CustomerFinancialInformation(Customer customer);
}