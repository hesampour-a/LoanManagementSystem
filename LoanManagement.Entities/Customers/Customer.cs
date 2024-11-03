namespace LoanManagementSystem.Entities.Customers;

public class Customer
{
    public int Id { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
}