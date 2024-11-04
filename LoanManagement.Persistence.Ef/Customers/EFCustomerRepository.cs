using LoanManagementSystem.Entities.Customers;
using LoanManagementSystem.Entities.Loans;
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

   

    public void UpdateCustomerFinancialInformation(Customer customer)
    {
        context.Set<CustomerFinancialInformation>()
            .Update(customer.FinancialInformation);
    }

    public CustomerScoreInformationDto? FindScoreInformationById(int customerId)
    {
        var customer = context.Set<Customer>()
            .Where(c => c.Id == customerId)
            .Select(c => new
            {
                JobType = c.FinancialInformation != null
                    ? c.FinancialInformation.JobType
                    : JobType.Unemployed,
                MonthlyIncome = c.FinancialInformation != null
                    ? c.FinancialInformation.MonthlyIncome
                    : 0,
                TotalAssetsValue = c.FinancialInformation != null
                    ? c.FinancialInformation.TotalAssetsValue
                    : 0,
                LateRepaidInstallments = c.Loans
                    .SelectMany(l => l.Installments)
                    .Count(i =>
                        i.PaidDate.HasValue && i.PaidDate > i.ShouldPayDate),
                HasLoanAndRepaidInTime = c.Loans.Any(l =>
                    l.LoanStatus == LoanStatus.Closed &&
                    !l.Installments.Any(i =>
                        i.PaidDate.HasValue && i.PaidDate > i.ShouldPayDate))
            })
            .FirstOrDefault();

        if (customer == null) return null;

        // نگاشت نتیجه میانی به DTO نهایی
        return new CustomerScoreInformationDto
        {
            JobType = customer.JobType,
            MonthlyIncome = customer.MonthlyIncome,
            TotalAssetsValue = customer.TotalAssetsValue,
            LateRepaidInstallmentsCount = customer.LateRepaidInstallments,
            HasLoanAndRepaidInTime = customer.HasLoanAndRepaidInTime
        };
    }
}