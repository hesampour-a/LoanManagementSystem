using LoanManagementSystem.Services.Customers.Contracts.DTOs;

namespace LoanManagementSystem.Services.Customers.Contracts;

public interface CustomerQuery
{
    List<GetAllCustomersWaitingForVerificationDto>
        GetAllCustomersWaitingForVerification();
}