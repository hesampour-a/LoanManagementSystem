using FluentAssertions;
using LoanManagementSystem.Services.LoanFormats.Contracts;
using LoanManagementSystem.TestTools.Infrastructure.DataBaseConfig.Integration;
using LoanManagementSystem.TestTools.LoanFormats;
using Xunit;

namespace LoanManagementSystem.Service.Unit.Tests.LoanFormats;

public class LoanFormatQueryTests : BusinessIntegrationTest
{
    private readonly LoanFormatQuery _sut;

    public LoanFormatQueryTests()
    {
        _sut = LoanFormatQueryFactory.Generate(SetupContext);
    }

    [Fact]
    public void GetAll_get_all_loan_formats_properly()
    {
        var loanFormat1 = LoanFormatFactory.Generate();
        Save(loanFormat1);
        var loanFormat2 = LoanFormatFactory.Generate();
        Save(loanFormat2);

        var actual = _sut.GetAll();

        actual.Should().HaveCount(2);
        actual.Should().ContainEquivalentOf(new GetAllLoanFormatsDto
        {
            Id = loanFormat1.Id,
            MonthlyPenaltyAmount = loanFormat1.MonthlyPenaltyAmount,
            Amount = loanFormat1.Amount,
            InstallmentsCount = loanFormat1.InstallmentsCount,
            InterestRate = loanFormat1.InterestRate,
            MonthlyInterestAmount = loanFormat1.MonthlyInterestAmount,
            MonthlyRepayAmount = loanFormat1.MonthlyRepayAmount,
        });
        actual.Should().ContainEquivalentOf(new GetAllLoanFormatsDto
        {
            Id = loanFormat2.Id,
            MonthlyPenaltyAmount = loanFormat2.MonthlyPenaltyAmount,
            Amount = loanFormat2.Amount,
            InstallmentsCount = loanFormat2.InstallmentsCount,
            InterestRate = loanFormat2.InterestRate,
            MonthlyInterestAmount = loanFormat2.MonthlyInterestAmount,
            MonthlyRepayAmount = loanFormat2.MonthlyRepayAmount,
        });
    }
}