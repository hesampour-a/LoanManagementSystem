using LoanManagementSystem.Entities.Customers;

namespace LoanManagementSystem.TestTools.Customers;

public class CustomerBuilder
{
    private Customer _customer = new Customer
    {
        FirstName = "FakeName",
        LastName = "FakeLastName",
        Email = "Fake@email.com",
        NationalCode = "5555555555",
        PhoneNumber = "09999999999",
        IsVerified = false,
        IdentityDocument = null
    };

    public CustomerBuilder WithIsVerified(bool isVerified)
    {
        _customer.IsVerified = isVerified;
        return this;
    }

    public CustomerBuilder WithIdentityDocument(string identityDocument)
    {
        _customer.IdentityDocument = identityDocument;
        return this;
    }

    public CustomerBuilder WithId(int id)
    {
        _customer.Id = id;
        return this;
    }

    public CustomerBuilder WithFirstName(string firstName)
    {
        _customer.FirstName = firstName;
        return this;
    }

    public CustomerBuilder WithLastName(string lastName)
    {
        _customer.LastName = lastName;
        return this;
    }

    public CustomerBuilder WithNationalCode(string nationalCode)
    {
        _customer.NationalCode = nationalCode;
        return this;
    }

    public CustomerBuilder WithEmail(string email)
    {
        _customer.Email = email;
        return this;
    }

    public CustomerBuilder WithPhoneNumber(string phoneNumber)
    {
        _customer.PhoneNumber = phoneNumber;
        return this;
    }

    public Customer Build()
    {
        return _customer;
    }
}