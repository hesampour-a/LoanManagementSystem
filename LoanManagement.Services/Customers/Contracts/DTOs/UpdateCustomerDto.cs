namespace LoanManagementSystem.Services.Customers.Contracts.DTOs;

public class UpdateCustomerDto
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string PhoneNumber { get; set; }
    public required string NationalCode { get; set; }
    public required string Email { get; set; }
}