using FluentAssertions;
using LoanManagementSystem.Entities.Loans;
using LoanManagementSystem.Persistence.Ef.Loans;
using LoanManagementSystem.Services.Loans.Contracts;
using LoanManagementSystem.Services.Loans.Contracts.DTOs;
using LoanManagementSystem.TestTools.Customers;
using LoanManagementSystem.TestTools.Infrastructure.DataBaseConfig.Integration;
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
}