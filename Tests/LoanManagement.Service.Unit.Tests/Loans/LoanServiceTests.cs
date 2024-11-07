using FluentAssertions;
using LoanManagementSystem.Entities.Customers;
using LoanManagementSystem.Entities.Installments;
using LoanManagementSystem.Entities.Loans;
using LoanManagementSystem.Services.Admins.Exceptions;
using LoanManagementSystem.Services.Customers.Exceptions;
using LoanManagementSystem.Services.LoanFormats.Exceptions;
using LoanManagementSystem.Services.Loans.Contracts;
using LoanManagementSystem.Services.Loans.Contracts.DTOs;
using LoanManagementSystem.Services.Loans.Exceptions;
using LoanManagementSystem.TestTools.Admins;
using LoanManagementSystem.TestTools.Customers;
using LoanManagementSystem.TestTools.Infrastructure.DataBaseConfig.Integration;
using LoanManagementSystem.TestTools.Installments;
using LoanManagementSystem.TestTools.LoanFormats;
using LoanManagementSystem.TestTools.Loans;
using Xunit;

namespace LoanManagementSystem.Service.Unit.Tests.Loans;

public class LoanServiceTests : BusinessIntegrationTest
{
    private readonly LoanService _sut;

    public LoanServiceTests()
    {
        _sut = LoanServiceFactory.Generate(SetupContext);
    }

    [Fact]
    public void Add_add_a_loan_properly()
    {
        var customer1 = new CustomerBuilder()
            .WithIsVerified(true)
            .WithIdentityDocument("dummyUrl")
            .Build();
        customer1.CustomerFinancialInformation =
            new CustomerFinancialInformation
            {
                CustomerId = customer1.Id,
                JobType = JobType.Government,
                MonthlyIncome = 10000000,
                TotalAssetsValue = 100000000
            };
        Save(customer1);
        var customer2 = new CustomerBuilder().WithIsVerified(true)
            .WithIdentityDocument("dummyUrl")
            .Build();
        customer2.CustomerFinancialInformation =
            new CustomerFinancialInformation
            {
                CustomerId = customer2.Id,
                JobType = JobType.Free,
                MonthlyIncome = 0,
                TotalAssetsValue = 0
            };
        Save(customer2);
        var customer3 = new CustomerBuilder()
            .WithIsVerified(true)
            .WithIdentityDocument("dummyUrl")
            .Build();
        customer3.CustomerFinancialInformation =
            new CustomerFinancialInformation
            {
                CustomerId = customer3.Id,
                JobType = JobType.Unemployed,
                MonthlyIncome = 15000000,
                TotalAssetsValue = 10000000000
            };
        Save(customer3);
        var loanFormat = LoanFormatFactory.Generate();
        Save(loanFormat);
        var dto = new AddLoanDto
        {
            LoanFormatId = loanFormat.Id,
        };
        var today = DateOnly.FromDateTime(DateTime.Today);
        var loan1 = new LoanBuilder(customer1.Id, loanFormat.Id)
            .WithLoanStatus(LoanStatus.Deferred)
            .Build();
        Save(loan1);
        var installment1 =
            InstallmentFactory.Generate(
                loan1.Id,
                today.AddMonths(-4),
                today.AddMonths(-3));
        Save(installment1);
        var installment2 =
            InstallmentFactory.Generate(
                loan1.Id,
                today.AddMonths(-2),
                today.AddMonths(-2));
        Save(installment2);
        var installment3 =
            InstallmentFactory.Generate(
                loan1.Id,
                today.AddMonths(-1));
        Save(installment3);
        var loan2 = new LoanBuilder(customer2.Id, loanFormat.Id)
            .WithLoanStatus(LoanStatus.Closed)
            .Build();
        Save(loan2);
        var installment4 =
            InstallmentFactory.Generate(
                loan2.Id,
                today.AddMonths(1),
                today.AddMonths(1));
        Save(installment4);
        var installment5 =
            InstallmentFactory.Generate(
                loan2.Id,
                today.AddMonths(2),
                today.AddMonths(2));
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
                today.AddMonths(1));
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

        _sut.Add(customer1.Id, dto);
        _sut.Add(customer2.Id, dto);
        _sut.Add(customer3.Id, dto);

        var expected = ReadContext.Set<Loan>().ToList();
        expected.Should().HaveCount(6);
        expected.Should().ContainEquivalentOf(new Loan
        {
            LoanFormatId = dto.LoanFormatId,
            CustomerId = customer1.Id,
            LoanStatus = LoanStatus.Rejected,
            ValidationScore = 15,
        }, o => o.Excluding(l => l.Id));
        expected.Should().ContainEquivalentOf(new Loan
        {
            LoanFormatId = dto.LoanFormatId,
            CustomerId = customer2.Id,
            LoanStatus = LoanStatus.Rejected,
            ValidationScore = 40,
        }, o => o.Excluding(l => l.Id));
        expected.Should().ContainEquivalentOf(new Loan
        {
            LoanFormatId = dto.LoanFormatId,
            CustomerId = customer3.Id,
            LoanStatus = LoanStatus.Pending,
            ValidationScore = 60,
        }, o => o.Excluding(l => l.Id));
    }

    [Theory]
    [InlineData(-1)]
    public void Add_throws_exception_if_customer_not_found(int dummyCustomerId)
    {
        var actual = () => _sut.Add(dummyCustomerId, new AddLoanDto());

        actual.Should().ThrowExactly<CustomerNotFoundException>();
        ReadContext.Set<Loan>().Should().HaveCount(0);
    }

    [Theory]
    [InlineData(-1)]
    public void Add_throws_exception_if_loan_format_not_found(
        int dummyLoanFormatId)
    {
        var customer = new CustomerBuilder().Build();
        Save(customer);
        var dto = new AddLoanDto
        {
            LoanFormatId = dummyLoanFormatId,
        };

        var actual = () => _sut.Add(customer.Id, dto);

        actual.Should().ThrowExactly<LoanFormatNotFoundException>();
        ReadContext.Set<Loan>().Should().HaveCount(0);
    }

    [Fact]
    public void Add_throw_exception_if_customer_is_not_verified()
    {
        var loanFormat = LoanFormatFactory.Generate();
        Save(loanFormat);
        var customer = new CustomerBuilder().Build();
        Save(customer);
        var dto = new AddLoanDto
        {
            LoanFormatId = loanFormat.Id,
        };

        var actual = () => _sut.Add(customer.Id, dto);

        actual.Should().ThrowExactly<CustomerIsNotVerifiedException>();
        ReadContext.Set<Loan>().Should().HaveCount(0);
    }

    [Fact]
    public void Confirm_confirm_a_loan_properly()
    {
        var admin = AdminFactory.Generate();
        Save(admin);
        var customer = new CustomerBuilder().Build();
        Save(customer);
        var loanFormat = LoanFormatFactory.Generate();
        Save(loanFormat);
        var loan1 = new LoanBuilder(customer.Id, loanFormat.Id).Build();
        Save(loan1);
        var loan2 = new LoanBuilder(customer.Id, loanFormat.Id).Build();
        Save(loan2);

        _sut.Confirm(admin.Id, loan2.Id);

        var expected = ReadContext.Set<Loan>().ToList();
        expected.Should().HaveCount(2);
        expected.Should().ContainEquivalentOf(loan1,
            o =>
                o.Excluding(l => l.Customer)
                    .Excluding(l => l.LoanFormat)
                    .Excluding(l => l.Installments));
        expected.Should().ContainEquivalentOf(
            new LoanBuilder(customer.Id, loanFormat.Id)
                .WithId(loan2.Id)
                .WithLoanStatus(LoanStatus.Confirmed)
                .Build(),
            o =>
                o.Excluding(l => l.Customer)
                    .Excluding(l => l.LoanFormat)
                    .Excluding(l => l.Installments));
    }

    [Theory]
    [InlineData(-1)]
    public void Confirm_throws_exception_if_admin_dose_not_exists(
        int dummyAdminId)
    {
        var customer = new CustomerBuilder().Build();
        Save(customer);
        var loanFormat = LoanFormatFactory.Generate();
        Save(loanFormat);
        var loan1 = new LoanBuilder(customer.Id, loanFormat.Id).Build();
        Save(loan1);

        var actual = () => _sut.Confirm(dummyAdminId, loan1.Id);

        actual.Should().ThrowExactly<AdminNotFoundException>();
        loan1.LoanStatus.Should().Be(LoanStatus.Pending);
    }

    [Theory]
    [InlineData(-1)]
    public void Confirm_throws_exception_if_loan_dose_not_exists(
        int dummyLoanId)
    {
        var admin = AdminFactory.Generate();
        Save(admin);
        var customer = new CustomerBuilder().Build();
        Save(customer);
        var loanFormat = LoanFormatFactory.Generate();
        Save(loanFormat);
        var loan1 = new LoanBuilder(customer.Id, loanFormat.Id).Build();
        Save(loan1);

        var actual = () => _sut.Confirm(admin.Id, dummyLoanId);

        actual.Should().ThrowExactly<LoanNotFoundException>();
        loan1.LoanStatus.Should().Be(LoanStatus.Pending);
    }

    [Theory]
    [InlineData(LoanStatus.Confirmed)]
    [InlineData(LoanStatus.Rejected)]
    [InlineData(LoanStatus.Closed)]
    [InlineData(LoanStatus.Deferred)]
    public void Confirm_throws_exception_if_loan_status_is_not_pending(
        LoanStatus loanStatus)
    {
        var admin = AdminFactory.Generate();
        Save(admin);
        var customer = new CustomerBuilder().Build();
        Save(customer);
        var loanFormat = LoanFormatFactory.Generate();
        Save(loanFormat);
        var loan1 = new LoanBuilder(customer.Id, loanFormat.Id)
            .WithLoanStatus(loanStatus)
            .Build();
        Save(loan1);

        var actual = () => _sut.Confirm(admin.Id, loan1.Id);

        actual.Should().ThrowExactly<LoanIsNotInPendingStatusException>();
        loan1.LoanStatus.Should().Be(loanStatus);
    }

    [Theory]
    [InlineData(59)]
    [InlineData(1)]
    public void Confirm_throws_exception_if_validation_score_is_not_enough(
        int validationScore)
    {
        var admin = AdminFactory.Generate();
        Save(admin);
        var customer = new CustomerBuilder().Build();
        Save(customer);
        var loanFormat = LoanFormatFactory.Generate();
        Save(loanFormat);
        var loan1 = new LoanBuilder(customer.Id, loanFormat.Id)
            .WithValidationScore(validationScore)
            .Build();
        Save(loan1);

        var actual = () => _sut.Confirm(admin.Id, loan1.Id);

        actual.Should().ThrowExactly<LoanValidationScoreIsNotEnoughException>();
        loan1.LoanStatus.Should().Be(LoanStatus.Pending);
    }

    [Fact]
    public void Reject_reject_a_loan_properly()
    {
        var admin = AdminFactory.Generate();
        Save(admin);
        var customer = new CustomerBuilder().Build();
        Save(customer);
        var loanFormat = LoanFormatFactory.Generate();
        Save(loanFormat);
        var loan1 = new LoanBuilder(customer.Id, loanFormat.Id).Build();
        Save(loan1);
        var loan2 = new LoanBuilder(customer.Id, loanFormat.Id).Build();
        Save(loan2);

        _sut.Reject(admin.Id, loan2.Id);

        var expected = ReadContext.Set<Loan>().ToList();
        expected.Should().HaveCount(2);
        expected.Should().ContainEquivalentOf(loan1,
            o =>
                o.Excluding(l => l.Customer)
                    .Excluding(l => l.LoanFormat)
                    .Excluding(l => l.Installments));
        expected.Should().ContainEquivalentOf(
            new LoanBuilder(customer.Id, loanFormat.Id)
                .WithId(loan2.Id)
                .WithLoanStatus(LoanStatus.Rejected)
                .Build(),
            o =>
                o.Excluding(l => l.Customer)
                    .Excluding(l => l.LoanFormat)
                    .Excluding(l => l.Installments));
    }

    [Theory]
    [InlineData(-1)]
    public void Reject_throws_exception_if_admin_dose_not_exists(
        int dummyAdminId)
    {
        var customer = new CustomerBuilder().Build();
        Save(customer);
        var loanFormat = LoanFormatFactory.Generate();
        Save(loanFormat);
        var loan1 = new LoanBuilder(customer.Id, loanFormat.Id)
            .Build();
        Save(loan1);

        var actual = () => _sut.Confirm(dummyAdminId, loan1.Id);

        actual.Should().ThrowExactly<AdminNotFoundException>();
        loan1.LoanStatus.Should().Be(LoanStatus.Pending);
    }

    [Theory]
    [InlineData(-1)]
    public void Reject_throws_exception_if_loan_dose_not_exists(
        int dummyLoanId)
    {
        var admin = AdminFactory.Generate();
        Save(admin);
        var customer = new CustomerBuilder().Build();
        Save(customer);
        var loanFormat = LoanFormatFactory.Generate();
        Save(loanFormat);
        var loan1 = new LoanBuilder(customer.Id, loanFormat.Id).Build();
        Save(loan1);

        var actual = () => _sut.Confirm(admin.Id, dummyLoanId);

        actual.Should().ThrowExactly<LoanNotFoundException>();
        loan1.LoanStatus.Should().Be(LoanStatus.Pending);
    }

    [Theory]
    [InlineData(LoanStatus.Confirmed)]
    [InlineData(LoanStatus.Rejected)]
    [InlineData(LoanStatus.Closed)]
    [InlineData(LoanStatus.Deferred)]
    public void Reject_throws_exception_if_loan_status_is_not_pending(
        LoanStatus loanStatus)
    {
        var admin = AdminFactory.Generate();
        Save(admin);
        var customer = new CustomerBuilder().Build();
        Save(customer);
        var loanFormat = LoanFormatFactory.Generate();
        Save(loanFormat);
        var loan1 = new LoanBuilder(customer.Id, loanFormat.Id)
            .WithLoanStatus(loanStatus)
            .Build();
        Save(loan1);

        var actual = () => _sut.Confirm(admin.Id, loan1.Id);

        actual.Should().ThrowExactly<LoanIsNotInPendingStatusException>();
        loan1.LoanStatus.Should().Be(loanStatus);
    }

    [Fact]
    public void Pay_pays_a_loan_properly()
    {
        var admin = AdminFactory.Generate();
        Save(admin);
        var customer = new CustomerBuilder().Build();
        Save(customer);
        var loanFormat = LoanFormatFactory.Generate();
        Save(loanFormat);
        var loan1 = new LoanBuilder(customer.Id, loanFormat.Id)
            .WithLoanStatus(LoanStatus.Confirmed)
            .Build();
        Save(loan1);
        var loan2 = new LoanBuilder(customer.Id, loanFormat.Id)
            .WithLoanStatus(LoanStatus.Confirmed)
            .Build();
        Save(loan2);

        _sut.Pay(admin.Id, loan2.Id);

        var expected = ReadContext.Set<Loan>().ToList();
        expected.Should().HaveCount(2);
        expected.Should().ContainEquivalentOf(loan1,
            o =>
                o.Excluding(l => l.Customer)
                    .Excluding(l => l.LoanFormat)
                    .Excluding(l => l.Installments));
        expected.Should().ContainEquivalentOf(
            new LoanBuilder(customer.Id, loanFormat.Id)
                .WithId(loan2.Id)
                .WithLoanStatus(LoanStatus.Repaymenting)
                .Build(),
            o =>
                o.Excluding(l => l.Customer)
                    .Excluding(l => l.LoanFormat)
                    .Excluding(l => l.Installments));
        var excpected2 = ReadContext.Set<Installment>().ToList();
        excpected2.Should().HaveCount(loanFormat.InstallmentsCount);
        for (var i = 1; i <= loanFormat.InstallmentsCount; i++)
        {
            excpected2.Should().ContainEquivalentOf(new Installment
            {
                LoanId = loan2.Id,
                ShouldPayDate =
                    DateOnly.FromDateTime(DateTime.Today.AddMonths(i))
            }, o => o.Excluding(i => i.Id).Excluding(i => i.Loan));
        }
    }

    [Fact]
    public void UpdateDeferred_sets_the_late_loan_status_to_deferred()
    {
        var admin = AdminFactory.Generate();
        Save(admin);
        var customer = new CustomerBuilder().Build();
        Save(customer);
        var loanFormat = LoanFormatFactory.Generate(installmentsCount: 2);
        Save(loanFormat);
        var loan1 = new LoanBuilder(customer.Id, loanFormat.Id)
            .WithLoanStatus(LoanStatus.Repaymenting)
            .Build();
        loan1.Installments.Add(new Installment
        {
            LoanId = loan1.Id,
            ShouldPayDate = DateOnly.FromDateTime(DateTime.Today.AddMonths(1))
        });
        loan1.Installments.Add(new Installment
        {
            LoanId = loan1.Id,
            ShouldPayDate = DateOnly.FromDateTime(DateTime.Today.AddMonths(2))
        });
        Save(loan1);
        var loan2 = new LoanBuilder(customer.Id, loanFormat.Id)
            .WithLoanStatus(LoanStatus.Repaymenting)
            .Build();
        loan2.Installments.Add(new Installment
        {
            LoanId = loan2.Id,
            ShouldPayDate = DateOnly.FromDateTime(DateTime.Today.AddMonths(-1))
        });
        loan2.Installments.Add(new Installment
        {
            LoanId = loan2.Id,
            ShouldPayDate = DateOnly.FromDateTime(DateTime.Today.AddMonths(-2))
        });
        Save(loan2);

        _sut.UpdateDeferreds();

        var expected = ReadContext.Set<Loan>().ToList();
        expected.Should().HaveCount(2);
        expected.Should().ContainEquivalentOf(loan1,
            o =>
                o.Excluding(l => l.Customer)
                    .Excluding(l => l.LoanFormat)
                    .Excluding(l => l.Installments));
        expected.Should().ContainEquivalentOf(
            new LoanBuilder(customer.Id, loanFormat.Id)
                .WithId(loan2.Id)
                .WithLoanStatus(LoanStatus.Deferred)
                .Build(),
            o =>
                o.Excluding(l => l.Customer)
                    .Excluding(l => l.LoanFormat)
                    .Excluding(l => l.Installments));
    }
}