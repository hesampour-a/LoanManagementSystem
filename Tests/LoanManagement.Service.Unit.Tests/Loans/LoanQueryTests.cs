using FluentAssertions;
using LoanManagementSystem.Entities.LoanFormats;
using LoanManagementSystem.Entities.Loans;
using LoanManagementSystem.Persistence.Ef.Loans;
using LoanManagementSystem.Services.Loans.Contracts;
using LoanManagementSystem.Services.Loans.Contracts.DTOs;
using LoanManagementSystem.TestTools.Customers;
using LoanManagementSystem.TestTools.Infrastructure.DataBaseConfig.Integration;
using LoanManagementSystem.TestTools.Installments;
using LoanManagementSystem.TestTools.LoanFormats;
using LoanManagementSystem.TestTools.Loans;
using Xunit;

namespace LoanManagementSystem.Service.Unit.Tests.Loans;

public class LoanQueryTests : BusinessIntegrationTest
{
    private readonly LoanQuery _sut;

    public LoanQueryTests()
    {
        _sut = new EFLoanQuey(SetupContext);
    }

    [Fact]
    public void GetAllPendingLoans_gets_all_pending_loans_properly()
    {
        var customer = new CustomerBuilder().Build();
        Save(customer);
        var loanFormat = LoanFormatFactory.Generate();
        Save(loanFormat);
        var loan1 = new LoanBuilder(customer.Id, loanFormat.Id)
            .WithLoanStatus(LoanStatus.Pending)
            .Build();
        Save(loan1);
        var loan2 = new LoanBuilder(customer.Id, loanFormat.Id)
            .WithLoanStatus(LoanStatus.Pending)
            .Build();
        Save(loan2);
        var loan3 = new LoanBuilder(customer.Id, loanFormat.Id)
            .WithLoanStatus(LoanStatus.Confirmed)
            .Build();
        Save(loan3);

        var actual = _sut.GetAllPendings();

        actual.Should().HaveCount(2);
        actual.Should().ContainEquivalentOf(new GetAllPendingLoansDto
        {
            Id = loan1.Id,
            LoanFormatId = loan1.LoanFormatId,
            CustomerId = loan1.CustomerId,
            ValidationScore = loan1.ValidationScore,
        });
        actual.Should().ContainEquivalentOf(new GetAllPendingLoansDto
        {
            Id = loan2.Id,
            LoanFormatId = loan2.LoanFormatId,
            CustomerId = loan2.CustomerId,
            ValidationScore = loan2.ValidationScore,
        });
    }

    [Fact]
    public void
        GetAllRepaymentingAndDeferred_gets_all_repaymenting_and_deferred_loans_properly()
    {
        var today = DateOnly.FromDateTime(DateTime.Today);
        var customer = new CustomerBuilder().Build();
        Save(customer);
        var loanFormat = LoanFormatFactory.Generate(installmentsCount: 2);
        Save(loanFormat);
        var loan1 = new LoanBuilder(customer.Id, loanFormat.Id)
            .WithLoanStatus(LoanStatus.Repaymenting)
            .Build();
        Save(loan1);
        var installment1 =
            InstallmentFactory.Generate(loan1.Id, today.AddMonths(1));
        Save(installment1);
        var installment2 =
            InstallmentFactory.Generate(loan1.Id, today.AddMonths(2));
        Save(installment2);
        var loan2 = new LoanBuilder(customer.Id, loanFormat.Id)
            .WithLoanStatus(LoanStatus.Deferred)
            .Build();
        Save(loan2);
        var installment3 =
            InstallmentFactory.Generate(
                loan2.Id,
                today.AddMonths(-2),
                today.AddMonths(-1));
        Save(installment3);
        var installment4 =
            InstallmentFactory.Generate(
                loan2.Id,
                today.AddMonths(-1),
                today.AddMonths(-1));
        Save(installment4);
        var loan3 = new LoanBuilder(customer.Id, loanFormat.Id)
            .WithLoanStatus(LoanStatus.Closed)
            .Build();
        Save(loan3);
        var installment5 =
            InstallmentFactory.Generate(
                loan3.Id,
                today.AddMonths(-2),
                today.AddMonths(-2));
        Save(installment5);
        var installment6 =
            InstallmentFactory.Generate(
                loan3.Id,
                today.AddMonths(-1),
                today.AddMonths(-2));
        Save(installment6);
        var loan4 = new LoanBuilder(customer.Id, loanFormat.Id)
            .WithLoanStatus(LoanStatus.Confirmed)
            .Build();
        Save(loan4);

        var actual = _sut.GetAllRepaymentingAndDeferred();

        actual.Should().HaveCount(2);
        actual.Should().ContainEquivalentOf(new GetAllRepaymentingAndDeferredDto
        {
            Id = loan1.Id,
            CustomerId = loan1.CustomerId,
            ValidationScore = loan1.ValidationScore,
            LoanFormatId = loan1.LoanFormatId,
            LoanStatus = loan1.LoanStatus,
            TotalPaidUntilNow = 0,
            RemainingInstallments =
            [
                new GetAllRemainingInstallmentsDto
                {
                    Id = installment1.Id,
                    ShouldPayDate = installment1.ShouldPayDate,
                },
                new GetAllRemainingInstallmentsDto
                {
                    Id = installment2.Id,
                    ShouldPayDate = installment2.ShouldPayDate,
                }
            ]
        });

        actual.Should().ContainEquivalentOf(new GetAllRepaymentingAndDeferredDto
        {
            Id = loan2.Id,
            CustomerId = loan2.CustomerId,
            ValidationScore = loan2.ValidationScore,
            LoanFormatId = loan2.LoanFormatId,
            LoanStatus = loan2.LoanStatus,
            TotalPaidUntilNow = (((loanFormat.MonthlyRepayAmount +
                                   loanFormat.MonthlyInterestAmount) * 2) +
                                 loanFormat.MonthlyPenaltyAmount),
            RemainingInstallments = []
        });
    }

    [Fact]
    public void GetMonthlyIncome_returns_all_monthly_income_properly()
    {
        var testDate = new DateOnly(1400, 05, 06);
        var today = DateOnly.FromDateTime(DateTime.Today);
        var customer1 = new CustomerBuilder().Build();
        Save(customer1);
        var customer2 = new CustomerBuilder().Build();
        Save(customer2);

        var loanFormat = LoanFormatFactory.Generate(installmentsCount: 3);
        Save(loanFormat);
        var loan1 = new LoanBuilder(customer1.Id, loanFormat.Id)
            .WithLoanStatus(LoanStatus.Repaymenting)
            .Build();
        Save(loan1);
        var installment1 =
            InstallmentFactory.Generate(
                loan1.Id,
                testDate,
                testDate);
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
                testDate.AddMonths(-1),
                testDate);
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


        var actual = _sut.GetMonthlyIncome(testDate);


        actual.Should().BeEquivalentTo(new GetMonthlyIncomeDto
        {
            TotalInterestAmount = loanFormat.MonthlyInterestAmount * 2,
            TotalPenaltyAmount = loanFormat.MonthlyPenaltyAmount * 1,
        });
    }

    [Fact]
    public void GetAllClosed_get_all_loans_with_status_closed()
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
                today.AddMonths(2),
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

        var actual = _sut.GetAllClosed();

        actual.Should().HaveCount(2);
        actual.Should().ContainEquivalentOf(new GetAllClosedDto
        {
            Id = loan2.Id,
            CustomerId = loan2.CustomerId,
            ValidationScore = loan2.ValidationScore,
            LoanFormatId = loan2.LoanFormatId,
            TotalPenaltyAmount = 2 * loanFormat.MonthlyPenaltyAmount,
            InstallmentsCount = loanFormat.InstallmentsCount,
            LoanAmount = loanFormat.Amount
        });
        actual.Should().ContainEquivalentOf(new GetAllClosedDto
        {
            Id = loan3.Id,
            CustomerId = loan3.CustomerId,
            ValidationScore = loan3.ValidationScore,
            LoanFormatId = loan3.LoanFormatId,
            TotalPenaltyAmount = 0,
            InstallmentsCount = loanFormat.InstallmentsCount,
            LoanAmount = loanFormat.Amount
        });
    }
}