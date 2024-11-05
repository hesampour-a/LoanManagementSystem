using FluentAssertions;
using LoanManagementSystem.Entities.Loans;
using LoanManagementSystem.Persistence.Ef.Customers;
using LoanManagementSystem.Services.Customers.Contracts;
using LoanManagementSystem.Services.Customers.Contracts.DTOs;
using LoanManagementSystem.TestTools.Admins;
using LoanManagementSystem.TestTools.Customers;
using LoanManagementSystem.TestTools.Infrastructure.DataBaseConfig.Integration;
using LoanManagementSystem.TestTools.Installments;
using LoanManagementSystem.TestTools.LoanFormats;
using LoanManagementSystem.TestTools.Loans;
using Xunit;

namespace LoanManagementSystem.Service.Unit.Tests.Customers;

public class CustomerQueryTest : BusinessIntegrationTest
{
    private readonly CustomerQuery _sut;

    public CustomerQueryTest()
    {
        _sut = CustomerQueryFactory.Generate(SetupContext);
    }

    [Fact]
    public void
        GetAllWaitingForVerifications_get_all_customers_waiting_for_verification()
    {
        var admin = AdminFactory.Generate();
        Save(admin);
        var customer1 = new CustomerBuilder().Build();
        Save(customer1);
        var customer2 =
            new CustomerBuilder()
                .WithIdentityDocument("test")
                .WithIsVerified(true)
                .Build();
        Save(customer2);
        var customer3 = new CustomerBuilder()
            .WithIdentityDocument("dummyDocTrl")
            .WithIsVerified(false)
            .Build();
        Save(customer3);

        var actual = _sut.GetAllCustomersWaitingForVerification();

        actual.Should().HaveCount(1);
        actual.Should().ContainEquivalentOf(
            new GetAllCustomersWaitingForVerificationDto
            {
                Id = customer3.Id,
                Email = customer3.Email,
                FirstName = customer3.FirstName,
                LastName = customer3.LastName,
                NationalCode = customer3.NationalCode,
                PhoneNumber = customer3.PhoneNumber,
                IdentityDocument = customer3.IdentityDocument,
            });
    }

    [Fact]
    public void
        GetAllHighRisks_gets_all_customers_with_two_or_more_late_repaid_installments()
    {
        var today = DateOnly.FromDateTime(DateTime.Today);
        var customer1 = new CustomerBuilder().Build();
        Save(customer1);
        var customer2 = new CustomerBuilder().Build();
        Save(customer2);
        var customer3 = new CustomerBuilder().Build();
        Save(customer3);
        var customer4 = new CustomerBuilder().Build();
        Save(customer4);
        var loanFormat = LoanFormatFactory.Generate(installmentsCount: 3);
        Save(loanFormat);
        var loan1 = new LoanBuilder(customer1.Id, loanFormat.Id)
            .WithLoanStatus(LoanStatus.Repaymenting)
            .Build();
        Save(loan1);
        var installment1 =
            InstallmentFactory.Generate(
                loan1.Id,
                today.AddMonths(1),
                today.AddMonths(1));
        Save(installment1);
        var installment2 =
            InstallmentFactory.Generate(
                loan1.Id,
                today.AddMonths(2),
                today.AddMonths(2));
        Save(installment2);
        var installment3 =
            InstallmentFactory.Generate(
                loan1.Id,
                today.AddMonths(3));
        Save(installment3);
        var loan2 = new LoanBuilder(customer2.Id, loanFormat.Id)
            .WithLoanStatus(LoanStatus.Closed)
            .Build();
        Save(loan2);
        var installment4 =
            InstallmentFactory.Generate(
                loan2.Id,
                today.AddMonths(1),
                today.AddMonths(2));
        Save(installment4);
        var installment5 =
            InstallmentFactory.Generate(
                loan2.Id,
                today.AddMonths(2),
                today.AddMonths(3));
        Save(installment5);
        var installment6 =
            InstallmentFactory.Generate(
                loan2.Id,
                today.AddMonths(3),
                today.AddMonths(3));
        Save(installment6);
        var loan3 = new LoanBuilder(customer3.Id, loanFormat.Id)
            .WithLoanStatus(LoanStatus.Closed)
            .Build();
        Save(loan3);
        var installment7 =
            InstallmentFactory.Generate(
                loan3.Id,
                today.AddMonths(1),
                today.AddMonths(2));
        Save(installment7);
        var installment8 =
            InstallmentFactory.Generate(
                loan3.Id,
                today.AddMonths(2),
                today.AddMonths(2));
        Save(installment8);
        var installment9 =
            InstallmentFactory.Generate(
                loan3.Id,
                today.AddMonths(3),
                today.AddMonths(3));
        Save(installment9);
        var loan4 = new LoanBuilder(customer4.Id, loanFormat.Id)
            .WithLoanStatus(LoanStatus.Deferred)
            .Build();
        Save(loan4);
        var installment10 =
            InstallmentFactory.Generate(
                loan4.Id,
                today.AddMonths(-1));
        Save(installment10);
        var installment11 =
            InstallmentFactory.Generate(
                loan4.Id,
                today.AddMonths(2));
        Save(installment11);
        var installment12 =
            InstallmentFactory.Generate(
                loan4.Id,
                today.AddMonths(3));
        Save(installment12);
        var loan5 = new LoanBuilder(customer4.Id, loanFormat.Id)
            .WithLoanStatus(LoanStatus.Closed)
            .Build();
        Save(loan5);
        var installment13 =
            InstallmentFactory.Generate(
                loan5.Id,
                today.AddMonths(-1),
                today.AddMonths(-1));
        Save(installment13);
        var installment14 =
            InstallmentFactory.Generate(
                loan5.Id,
                today.AddMonths(2),
                today.AddMonths(2));
        Save(installment14);
        var installment15 =
            InstallmentFactory.Generate(
                loan5.Id,
                today.AddMonths(3),
                today.AddMonths(4));
        Save(installment15);

        var actual = _sut.GetAllHighRisks();
        // should 2
        actual.Should().HaveCount(2);

        actual.Should().ContainEquivalentOf(new GetAllHighRisksDto
        {
            CustomerId = customer2.Id,
            LateInstallmentsCount = 2
        });
        actual.Should().ContainEquivalentOf(new GetAllHighRisksDto
        {
            CustomerId = customer4.Id,
            LateInstallmentsCount = 2
        });
    }
}