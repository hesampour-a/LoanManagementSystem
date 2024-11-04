using LoanManagementSystem.Entities.Customers;
using LoanManagementSystem.Services.Customers.Contracts.DTOs;

namespace LoanManagementSystem.Services.Customers.Contracts;

public interface CustomerRepository
{
    void Add(Customer customer);
    bool IsDuplicated(string nationalCode);
    Customer? FindById(int customerId);
    void Update(Customer customer);
    void AddCustomerFinancialInformation(Customer customer);
    CustomerScoreInformationDto? FindScoreInformationById(int customerId);
    Customer? FindByIdIncludeFinancialInformation(int customerId);
    CustomerFinancialInformation? FindFinancialInformationByCustomerId(int customerId);
    void UpdateCustomerFinancialInformation(CustomerFinancialInformation financialInformation);
}