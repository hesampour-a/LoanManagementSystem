using LoanManagementSystem.Entities.Customers;
using LoanManagementSystem.Entities.Loans;
using LoanManagementSystem.Services.Customers.Contracts;
using LoanManagementSystem.Services.Customers.Contracts.DTOs;
using Microsoft.EntityFrameworkCore;

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


    public void AddCustomerFinancialInformation(Customer customer)
    {
        context.Set<CustomerFinancialInformation>()
            .Add(customer.CustomerFinancialInformation);
    }

    public CustomerScoreInformationDto? FindScoreInformationById(int customerId)
    {
        return context.Set<Customer>().Where(c => c.Id == customerId).Select(
            c =>
                new CustomerScoreInformationDto
                {
                    IsVerified = c.IsVerified,
                    JobType = c.CustomerFinancialInformation != null
                        ? c.CustomerFinancialInformation.JobType
                        : JobType.Unemployed,
                    MonthlyIncome = c.CustomerFinancialInformation != null
                        ? c.CustomerFinancialInformation.MonthlyIncome
                        : 0,
                    TotalAssetsValue = c.CustomerFinancialInformation != null
                        ? c.CustomerFinancialInformation.TotalAssetsValue
                        : 0,
                    LateRepaidInstallmentsCount = c.Loans
                        .SelectMany(l => l.Installments)
                        .Count(i =>
                            (i.PaidDate.HasValue &&
                             i.PaidDate > i.ShouldPayDate) ||
                            (i.PaidDate == null && i.ShouldPayDate <
                                DateOnly.FromDateTime(DateTime.Today))),
                    HasLoanAndRepaidInTime = c.Loans.Any(l =>
                        l.LoanStatus == LoanStatus.Closed &&
                        !l.Installments.Any(i =>
                            i.PaidDate.HasValue &&
                            i.PaidDate > i.ShouldPayDate))
                }).FirstOrDefault();
    }

    public Customer? FindByIdIncludeFinancialInformation(int customerId)
    {
        return context.Set<Customer>()
            .Include(c => c.CustomerFinancialInformation)
            .FirstOrDefault(c => c.Id == customerId);
    }

    public void UpdateCustomerFinancialInformation(
        CustomerFinancialInformation financialInformation)
    {
        context.Set<CustomerFinancialInformation>()
            .Update(financialInformation);
    }
}