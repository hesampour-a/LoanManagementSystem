using LoanManagementSystem.Entities.Customers;

namespace LoanManagementSystem.Services.Customers.Contracts;

public interface CustomerService
{
    int Add(AddCustomerDto dto);

    List<GetAllCustomersWaitingForVerificationDto>
        GetAllCustomersWaitingForVerification(int adminId);

    void AddIdentityDocument(int customerId, AddIdentityDocumentDto dto);
    void VerifyCustomer(int adminId, VerifyCustomerDto dto);

    void UpdateCustomerFinancialInformation(int customerId,
        UpdateCustomerFinancialInformationDto dto);


    void Update(int customerId, UpdateCustomerDto dto);
    void RejectIdentityDocument(int adminId, RejectIdentityDocumentDto dto);
}

public class RejectIdentityDocumentDto
{
    public int CustomerId { get; set; }
}

public class UpdateCustomerDto
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string PhoneNumber { get; set; }
    public required string NationalCode { get; set; }
    public required string Email { get; set; }
}

public class UpdateCustomerFinancialInformationDto
{
    public decimal MonthlyIncome { get; set; }
    public decimal TotalAssetsValue { get; set; }
    public JobType JobType { get; set; }
}

public class VerifyCustomerDto
{
    public int customerId { get; set; }
}

public class AddIdentityDocumentDto
{
    public string IdentityDocument { get; set; }
}

public class AddCustomerDto
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string PhoneNumber { get; set; }
    public required string NationalCode { get; set; }
    public required string Email { get; set; }
}