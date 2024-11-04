using FluentAssertions;
using LoanManagementSystem.Entities.Installments;
using LoanManagementSystem.Entities.Loans;
using LoanManagementSystem.Services.Admins.Exceptions;
using LoanManagementSystem.Services.Installments.Contracts;
using LoanManagementSystem.Services.Installments.Exceptions;
using LoanManagementSystem.TestTools.Admins;
using LoanManagementSystem.TestTools.Customers;
using LoanManagementSystem.TestTools.Infrastructure.DataBaseConfig.Integration;
using LoanManagementSystem.TestTools.Installments;
using LoanManagementSystem.TestTools.LoanFormats;
using LoanManagementSystem.TestTools.Loans;
using Xunit;

namespace LoanManagementSystem.Service.Unit.Tests.Installments;

public class InstallmentServiceTests : BusinessIntegrationTest
{
    private readonly InstallmentService _sut;

    public InstallmentServiceTests()
    {
        _sut = InstallmentServiceFactory.Generate(SetupContext);
    }

    [Fact]
    public void Repayment_repay_a_installment_properly()
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
        Save(loan1);
        var installment1 = InstallmentFactory.Generate(loan1.Id);
        Save(installment1);
        var installment2 = InstallmentFactory.Generate(loan1.Id,
            DateOnly.FromDateTime(DateTime.Today.AddMonths(2)));
        Save(installment2);


        _sut.Repayment(admin.Id, installment1.Id);

        var expected = ReadContext.Set<Loan>().ToList();
        expected.Should().HaveCount(1);
        expected.Should().ContainEquivalentOf(loan1,
            o =>
                o.Excluding(l => l.Customer)
                    .Excluding(l => l.LoanFormat)
                    .Excluding(l => l.Installments));
        var expected2 = ReadContext.Set<Installment>().ToList();
        expected2.Should().HaveCount(2);
        expected2.Should().ContainEquivalentOf(new Installment
        {
            Id = installment1.Id,
            LoanId = installment1.LoanId,
            ShouldPayDate = installment1.ShouldPayDate,
            PaidDate = DateOnly.FromDateTime(DateTime.Today)
        }, o => o.Excluding(i => i.Loan));
        expected2.Should().ContainEquivalentOf(new Installment
        {
            Id = installment2.Id,
            LoanId = installment2.LoanId,
            ShouldPayDate = installment2.ShouldPayDate,
            PaidDate = null
        }, o => o.Excluding(i => i.Loan));
    }

    [Theory]
    [InlineData(-1)]
    public void Repayment_throws_exception_if_admin_dose_not_exists(
        int dummyAdminId)
    {
        var customer = new CustomerBuilder().Build();
        Save(customer);
        var loanFormat = LoanFormatFactory.Generate(installmentsCount: 2);
        Save(loanFormat);
        var loan1 = new LoanBuilder(customer.Id, loanFormat.Id)
            .WithLoanStatus(LoanStatus.Repaymenting)
            .Build();
        Save(loan1);
        var installment1 = InstallmentFactory.Generate(loan1.Id);
        Save(installment1);

        var actual = () => _sut.Repayment(dummyAdminId, installment1.Id);

        actual.Should().ThrowExactly<AdminNotFoundException>();
        ReadContext.Set<Installment>().First(i => i.Id == installment1.Id)
            .PaidDate.Should().BeNull();
    }

    [Theory]
    [InlineData(-1)]
    public void Repayment_throws_exception_if_installment_dose_not_exists(
        int dummyinstallmentId)
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
        Save(loan1);
        var installment1 = InstallmentFactory.Generate(loan1.Id);
        Save(installment1);

        var actual = () => _sut.Repayment(admin.Id, dummyinstallmentId);

        actual.Should().ThrowExactly<InstallmentNotFoundException>();
        ReadContext.Set<Installment>().First(i => i.Id == installment1.Id)
            .PaidDate.Should().BeNull();
    }

    [Fact]
    public void Repayment_throws_exception_if_installment_already_paid()
    {
        var date = DateOnly.FromDateTime(DateTime.Today.AddMonths(-5));
        var admin = AdminFactory.Generate();
        Save(admin);
        var customer = new CustomerBuilder().Build();
        Save(customer);
        var loanFormat = LoanFormatFactory.Generate(installmentsCount: 2);
        Save(loanFormat);
        var loan1 = new LoanBuilder(customer.Id, loanFormat.Id)
            .WithLoanStatus(LoanStatus.Repaymenting)
            .Build();
        Save(loan1);
        var installment1 =
            InstallmentFactory.Generate(loan1.Id, paidDate: date);
        Save(installment1);

        var actual = () => _sut.Repayment(admin.Id, installment1.Id);

        actual.Should().ThrowExactly<InstallmentAlreadyPaiedException>();
        ReadContext.Set<Installment>().First(i => i.Id == installment1.Id)
            .PaidDate.Should().Be(installment1.PaidDate);
    }
}