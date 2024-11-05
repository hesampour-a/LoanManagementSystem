using LoanManagementSystem.Entities.Customers;
using LoanManagementSystem.Services.Customers.Contracts;
using LoanManagementSystem.Services.Customers.Contracts.DTOs;

namespace LoanManagementSystem.Persistence.Ef.Customers;

public class EFCustomerQuery(EfDataContext context) : CustomerQuery
{
    public List<GetAllCustomersWaitingForVerificationDto>
        GetAllCustomersWaitingForVerification()
    {
        return context.Set<Customer>()
            .Where(c => c.IsVerified == false && c.IdentityDocument != null)
            .Select(c => new GetAllCustomersWaitingForVerificationDto
            {
                Email = c.Email,
                FirstName = c.FirstName,
                LastName = c.LastName,
                NationalCode = c.NationalCode,
                PhoneNumber = c.PhoneNumber,
                IdentityDocument = c.IdentityDocument,
                Id = c.Id,
            }).ToList();
    }

    public List<GetAllHighRisksDto> GetAllHighRisks()
    {
        return context.Set<Customer>().Where(c =>
                c.Loans.SelectMany(l => l.Installments)
                    .Count(i =>
                        i.PaidDate > i.ShouldPayDate ||
                        (i.PaidDate == null &&
                         i.ShouldPayDate <
                         DateOnly.FromDateTime(DateTime.Today))) >= 2)
            .Select(c =>
                new GetAllHighRisksDto
                {
                    CustomerId = c.Id,
                    LateInstallmentsCount = c.Loans
                        .SelectMany(c => c.Installments)
                        .Count(i =>
                            i.PaidDate > i.ShouldPayDate ||
                            (i.PaidDate == null &&
                             i.ShouldPayDate <
                             DateOnly.FromDateTime(DateTime.Today))),
                }).ToList();
    }
}