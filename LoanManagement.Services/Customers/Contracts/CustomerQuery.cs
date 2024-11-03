namespace LoanManagementSystem.Services.Customers.Contracts;

public interface CustomerQuery
{
    List<GetAllCustomersWaitingForVerificationDto>
        GetAllCustomersWaitingForVerification();
}