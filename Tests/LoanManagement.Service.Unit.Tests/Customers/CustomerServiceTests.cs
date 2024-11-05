using FluentAssertions;
using LoanManagementSystem.Entities.Customers;
using LoanManagementSystem.Persistence.Ef.Customers;
using LoanManagementSystem.Persistence.Ef.UnitOfWorks;
using LoanManagementSystem.Services.Admins.Exceptions;
using LoanManagementSystem.Services.Customers;
using LoanManagementSystem.Services.Customers.Contracts;
using LoanManagementSystem.Services.Customers.Contracts.DTOs;
using LoanManagementSystem.Services.Customers.Exceptions;
using LoanManagementSystem.TestTools.Admins;
using LoanManagementSystem.TestTools.Customers;
using LoanManagementSystem.TestTools.Infrastructure.DataBaseConfig.Integration;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace LoanManagementSystem.Service.Unit.Tests.Customers;

public class CustomerServiceTests : BusinessIntegrationTest
{
    private readonly CustomerService _sut;

    public CustomerServiceTests()
    {
        _sut = CustomerServiceFactory.Generate(SetupContext);
    }

    [Fact]
    public void Add_add_a_customer_properly()
    {
        var dto = new AddCustomerDto
        {
            FirstName = "Test",
            LastName = "Testmanesh",
            Email = "Test@test.com",
            NationalCode = "5555555555",
            PhoneNumber = "09999999999"
        };

        var actual = _sut.Add(dto);

        var expected = ReadContext.Set<Customer>().Single();
        expected.Should().BeEquivalentTo(new Customer
        {
            Id = actual,
            Email = dto.Email,
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            NationalCode = dto.NationalCode,
            PhoneNumber = dto.PhoneNumber,
            IsVerified = false,
            IdentityDocument = null
        }, options => options.Excluding(x => x.CustomerFinancialInformation));
    }

    [Theory]
    [InlineData("5555555555")]
    public void Add_throws_exception_if_national_code_already_exists(
        string nationalCode)
    {
        var customer = new CustomerBuilder().WithNationalCode(nationalCode)
            .Build();
        Save(customer);
        var dto = new AddCustomerDto
        {
            FirstName = "Test",
            LastName = "Testmanesh",
            Email = "Test@test.com",
            NationalCode = nationalCode,
            PhoneNumber = "09999999999"
        };

        var actual = () => _sut.Add(dto);
        actual.Should().ThrowExactly<NationalCodeAlreadyExistsException>();
        ReadContext.Set<Customer>().Single().Should().BeEquivalentTo(
            new CustomerBuilder().WithId(customer.Id)
                .WithNationalCode(nationalCode).Build());
    }

    [Fact]
    public void AddIdentityDocument_add_a_customer_identity_document_properly()
    {
        var customer1 = new CustomerBuilder().Build();
        Save(customer1);
        var customer2 = new CustomerBuilder().Build();
        Save(customer2);
        var dto = new AddIdentityDocumentDto
        {
            IdentityDocument = "Dummy Url"
        };

        _sut.AddIdentityDocument(customer2.Id, dto);

        var expected = ReadContext.Set<Customer>().ToList();
        expected.Should().HaveCount(2);
        expected.Should()
            .ContainEquivalentOf(
                new CustomerBuilder()
                    .WithId(customer1.Id)
                    .Build());
        expected.Should()
            .ContainEquivalentOf(
                new CustomerBuilder()
                    .WithId(customer2.Id)
                    .WithIdentityDocument(dto.IdentityDocument)
                    .Build());
    }

    [Theory]
    [InlineData(-1)]
    public void AddIdentityDocument_throws_exception_if_customer_dose_not_exist(
        int dummyId)
    {
        var actual = () =>
            _sut.AddIdentityDocument(dummyId, new AddIdentityDocumentDto());

        actual.Should().ThrowExactly<CustomerNotFoundException>();
    }

    [Fact]
    public void
        AddIdentityDocument_throws_exception_if_customer_already_has_identity_document()
    {
        var customer1 = new CustomerBuilder()
            .WithIdentityDocument("dummyUrl")
            .Build();
        Save(customer1);
        var dto = new AddIdentityDocumentDto
        {
            IdentityDocument = "AdddummyUrl"
        };

        var actual = () => _sut.AddIdentityDocument(customer1.Id, dto);

        actual.Should().ThrowExactly<IdentityDocumentAlreadyExistsException>();
        var expected = ReadContext.Set<Customer>().Single();
        expected.Should().BeEquivalentTo(new CustomerBuilder()
            .WithIdentityDocument("dummyUrl")
            .WithId(customer1.Id)
            .Build());
    }

    [Fact]
    public void
        VerifyIdentityDocument_verify_a_customer_identity_document_properly()
    {
        var admin = AdminFactory.Generate();
        Save(admin);
        var customer1 =
            new CustomerBuilder()
                .WithIdentityDocument("dummyUrl")
                .WithIsVerified(false)
                .Build();
        Save(customer1);
        var customer2 =
            new CustomerBuilder()
                .WithIdentityDocument("dummyUrl")
                .WithIsVerified(false)
                .Build();
        Save(customer2);
        var dto = new VerifyCustomerDto
        {
            customerId = customer2.Id
        };

        _sut.VerifyCustomer(admin.Id, dto);

        var expected = ReadContext.Set<Customer>().ToList();
        expected.Should().HaveCount(2);
        expected.Should().ContainEquivalentOf(
            new CustomerBuilder()
                .WithId(customer1.Id)
                .WithIdentityDocument("dummyUrl")
                .WithIsVerified(false)
                .Build(),
            o => o.Excluding(c => c.CustomerFinancialInformation));
        expected.Should().ContainEquivalentOf(
            new CustomerBuilder()
                .WithId(customer2.Id)
                .WithIdentityDocument("dummyUrl")
                .WithIsVerified(true)
                .Build(),
            o => o.Excluding(c => c.CustomerFinancialInformation));
    }

    [Theory]
    [InlineData(-1)]
    public void
        VerifyIdentityDocument_throws_exception_if_admin_dose_not_exists(
            int dummyId)
    {
        var actual = () =>
            _sut.VerifyCustomer(dummyId, new VerifyCustomerDto());

        actual.Should().ThrowExactly<AdminNotFoundException>();
    }

    [Theory]
    [InlineData(-1)]
    public void
        VerifyIdentityDocument_throws_exception_if_customer_dose_not_exists(
            int dummyId)
    {
        var admin = AdminFactory.Generate();
        Save(admin);
        var dto = new VerifyCustomerDto
        {
            customerId = dummyId
        };

        var actual = () => _sut.VerifyCustomer(admin.Id, dto);

        actual.Should().ThrowExactly<CustomerNotFoundException>();
    }

    [Fact]
    public void
        VerifyIdentityDocument_throws_exception_if_customer_dose_not_have_identity_document()
    {
        var admin = AdminFactory.Generate();
        Save(admin);
        var customer = new CustomerBuilder().Build();
        Save(customer);
        var dto = new VerifyCustomerDto
        {
            customerId = customer.Id
        };

        var actual = () => _sut.VerifyCustomer(admin.Id, dto);

        actual.Should()
            .ThrowExactly<CustomerDoseNotHaveIdentityDocumentException>();
        var expected = ReadContext.Set<Customer>().Single();
        expected.IsVerified.Should().BeFalse();
    }

    [Fact]
    public void RejectIdentityDocument_reject_a_identity_document_properly()
    {
        var admin = AdminFactory.Generate();
        Save(admin);
        var customer = new CustomerBuilder()
            .WithIdentityDocument("dummyUrl")
            .WithIsVerified(false)
            .Build();
        Save(customer);
        var dto = new RejectIdentityDocumentDto
        {
            CustomerId = customer.Id
        };

        _sut.RejectIdentityDocument(admin.Id, dto);

        var expected = ReadContext.Set<Customer>().Single();
        expected.IsVerified.Should().BeFalse();
        expected.IdentityDocument.Should().Be(null);
    }

    [Theory]
    [InlineData(-1)]
    public void
        RejectIdentityDocument_throws_exception_if_admin_dose_not_exists(
            int dummyId)
    {
        var actual = () =>
            _sut.RejectIdentityDocument(dummyId,
                new RejectIdentityDocumentDto());

        actual.Should().ThrowExactly<AdminNotFoundException>();
    }

    [Theory]
    [InlineData(-1)]
    public void
        RejectIdentityDocument_throws_exception_if_customer_dose_not_exists(
            int dummyId)
    {
        var admin = AdminFactory.Generate();
        Save(admin);
        var dto = new RejectIdentityDocumentDto
        {
            CustomerId = dummyId
        };
        var actual = () => _sut.RejectIdentityDocument(admin.Id, dto);

        actual.Should().ThrowExactly<CustomerNotFoundException>();
    }


    [Fact]
    public void
        AddCustomerFinancialInformation_add_a_customers_financial_information_properly()
    {
        var customer1 = new CustomerBuilder()
            .Build();
        Save(customer1);
        var dto = new AddCustomerFinancialInformationDto
        {
            JobType = JobType.Government,
            MonthlyIncome = 10000000,
            TotalAssetsValue = 10000000000
        };

        _sut.AddCustomerFinancialInformation(customer1.Id, dto);

        var expected =
            ReadContext.Set<Customer>()
                .Include(c => c.CustomerFinancialInformation)
                .Single();
        expected.Should().BeEquivalentTo(new Customer
        {
            Id = customer1.Id,
            Email = customer1.Email,
            FirstName = customer1.FirstName,
            LastName = customer1.LastName,
            NationalCode = customer1.NationalCode,
            PhoneNumber = customer1.PhoneNumber,
            IsVerified = customer1.IsVerified,
            IdentityDocument = customer1.IdentityDocument,
        }, o => o.Excluding(c => c.CustomerFinancialInformation));

        expected.CustomerFinancialInformation.JobType.Should().Be(dto.JobType);
        expected.CustomerFinancialInformation.MonthlyIncome.Should()
            .Be(dto.MonthlyIncome);
        expected.CustomerFinancialInformation.TotalAssetsValue.Should()
            .Be(dto.TotalAssetsValue);
        expected.CustomerFinancialInformation.CustomerId.Should()
            .Be(customer1.Id);
    }

    [Theory]
    [InlineData(-1)]
    public void
        AddCustomerFinancialInformation_throw_excption_if_customer_dose_not_exists(
            int dummyId)
    {
        var actual = () => _sut.AddCustomerFinancialInformation(dummyId,
            new AddCustomerFinancialInformationDto());

        actual.Should().ThrowExactly<CustomerNotFoundException>();
    }

    [Fact]
    public void
        UpdateCustomerFinancialInformation_update_a_customers_financial_information_properly()
    {
        var customer1 = new CustomerBuilder()
            .Build();
        customer1.CustomerFinancialInformation =
            new CustomerFinancialInformation
            {
                CustomerId = customer1.Id,
                MonthlyIncome = 15000000,
                TotalAssetsValue = 140000000,
                JobType = JobType.Free
            };
        Save(customer1);
        var dto = new UpdateCustomerFinancialInformationDto
        {
            JobType = JobType.Government,
            MonthlyIncome = 10000000,
            TotalAssetsValue = 10000000000
        };

        _sut.UpdateCustomerFinancialInformation(customer1.Id, dto);

        var expected =
            ReadContext.Set<Customer>()
                .Include(c => c.CustomerFinancialInformation)
                .Single();
        expected.Should().BeEquivalentTo(new Customer
        {
            Id = customer1.Id,
            Email = customer1.Email,
            FirstName = customer1.FirstName,
            LastName = customer1.LastName,
            NationalCode = customer1.NationalCode,
            PhoneNumber = customer1.PhoneNumber,
            IsVerified = customer1.IsVerified,
            IdentityDocument = customer1.IdentityDocument,
        }, o => o.Excluding(c => c.CustomerFinancialInformation));

        expected.CustomerFinancialInformation.JobType.Should().Be(dto.JobType);
        expected.CustomerFinancialInformation.MonthlyIncome.Should()
            .Be(dto.MonthlyIncome);
        expected.CustomerFinancialInformation.TotalAssetsValue.Should()
            .Be(dto.TotalAssetsValue);
        expected.CustomerFinancialInformation.CustomerId.Should()
            .Be(customer1.Id);
    }

    [Theory]
    [InlineData(-1)]
    public void
        UpdateCustomerFinancialInformation_throw_excption_if_customer_dose_not_exists(
            int dummyId)
    {
        var actual = () => _sut.UpdateCustomerFinancialInformation(dummyId,
            new UpdateCustomerFinancialInformationDto());

        actual.Should().ThrowExactly<CustomerNotFoundException>();
    }

    [Fact]
    public void Update_update_a_customers_properly()
    {
        var customer1 = new CustomerBuilder().Build();
        Save(customer1);
        var customer2 = new CustomerBuilder().Build();
        Save(customer2);
        var dto = new UpdateCustomerDto
        {
            Email = "test@test.com",
            FirstName = "edit",
            LastName = "test",
            NationalCode = "test",
            PhoneNumber = "test",
        };

        _sut.Update(customer2.Id, dto);

        var expected = ReadContext.Set<Customer>().ToList();
        expected.Should().HaveCount(2);
        expected.Should().ContainEquivalentOf(customer1,
            o => o.Excluding(c => c.CustomerFinancialInformation));
        expected.Should().ContainEquivalentOf(new Customer
        {
            Id = customer2.Id,
            Email = dto.Email,
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            NationalCode = dto.NationalCode,
            PhoneNumber = dto.PhoneNumber,
            IsVerified = false,
            IdentityDocument = null,
        }, o => o.Excluding(c => c.CustomerFinancialInformation));
    }

    [Theory]
    [InlineData(-1)]
    public void Update_throw_exception_if_customer_dose_not_exists(int dummyId)
    {
        var dto = new UpdateCustomerDto()
        {
            Email = "test@test.com",
            FirstName = "edit",
            LastName = "test",
            NationalCode = "test",
            PhoneNumber = "mamad"
        };
        var actual = () => _sut.Update(dummyId, dto);

        actual.Should().ThrowExactly<CustomerNotFoundException>();
    }

    [Theory]
    [InlineData("5555555555")]
    public void Update_throw_exception_if_national_code_already_exists(
        string nationalCode)
    {
        var customer1 = new CustomerBuilder().WithNationalCode(nationalCode)
            .Build();
        Save(customer1);
        var customer2 = new CustomerBuilder().WithNationalCode("3333333333")
            .Build();
        Save(customer2);
        var dto = new UpdateCustomerDto()
        {
            Email = "test@test.com",
            FirstName = "edit",
            LastName = "test",
            NationalCode = nationalCode,
            PhoneNumber = "mamad"
        };

        var actual = () => _sut.Update(customer2.Id, dto);

        actual.Should().ThrowExactly<NationalCodeAlreadyExistsException>();
        ReadContext.Set<Customer>().First(c => c.Id == customer2.Id)
            .NationalCode.Should().Be("3333333333");
    }
}