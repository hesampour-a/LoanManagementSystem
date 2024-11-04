using LoanManagementSystem.Services.Customers.Contracts.DTOs;

namespace LoanManagementSystem.Services.Customers.Contracts;

public interface CustomerService
{
    int Add(AddCustomerDto dto);

    List<GetAllCustomersWaitingForVerificationDto>
        GetAllCustomersWaitingForVerification(int adminId);

    void AddIdentityDocument(int customerId, AddIdentityDocumentDto dto);
    void VerifyCustomer(int adminId, VerifyCustomerDto dto);

    void AddCustomerFinancialInformation(int customerId,
        AddCustomerFinancialInformationDto dto);


    void Update(int customerId, UpdateCustomerDto dto);
    void RejectIdentityDocument(int adminId, RejectIdentityDocumentDto dto);
    void UpdateCustomerFinancialInformation(int customerId, UpdateCustomerFinancialInformationDto dto);
}