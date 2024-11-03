using LoanManagementSystem.Entities.Customers;
using LoanManagementSystem.Services.Customers.Contracts;

namespace LoanManagementSystem.Persistence.Ef.Customers;

public class EFCustomerRepository(EfDataContext context) : CustomerRepository
{
    public void Add(Customer customer)
    {
        context.Set<Customer>().Add(customer);
    }

    public bool IsDuplicated(string nationalCode)
    {
        return context.Set<Customer>().Any(c => c.NationalCode == nationalCode);
    }

    public Customer? FindById(int customerId)
    {
        return context.Set<Customer>().FirstOrDefault(c => c.Id == customerId);
    }

    public void Update(Customer customer)
    {
        context.Set<Customer>()
            .Update(customer);
    }

    public void AddFinancialInformation(
        CustomerFinancialInformation financialInformation)
    {
        context.Set<CustomerFinancialInformation>().Add(financialInformation);
    }

    public void CustomerFinancialInformation(Customer customer)
    {
        context.Set<CustomerFinancialInformation>()
            .Update(customer.FinancialInformation);
    }
}