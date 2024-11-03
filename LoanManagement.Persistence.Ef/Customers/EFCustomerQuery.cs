using LoanManagementSystem.Entities.Customers;
using LoanManagementSystem.Services.Customers.Contracts;

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
}