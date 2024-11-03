using FluentAssertions;
using LoanManagementSystem.Entities.LoanFormats;
using LoanManagementSystem.Services.Admins.Exceptions;
using LoanManagementSystem.Services.LoanFormats.Contracts;
using LoanManagementSystem.TestTools.Admins;
using LoanManagementSystem.TestTools.Infrastructure.DataBaseConfig.Integration;
using LoanManagementSystem.TestTools.LoanFormats;
using Xunit;

namespace LoanManagementSystem.Service.Unit.Tests.LoanFormats;

public class LoanFormatServiceTests : BusinessIntegrationTest
{
    private readonly LoanFormatService _sut;

    public LoanFormatServiceTests()
    {
        _sut = LoanFormatServiceFactory.Generate(SetupContext);
    }

    [Theory]
    [InlineData(100000000, 6)]
    [InlineData(100000000, 12)]
    [InlineData(100000000, 24)]
    public void Add_add_loan_format_properly(decimal loanAmount,
        int installmentsCount)
    {
        var admin = AdminFactory.Generate();
        Save(admin);
        var dto1 = new AddLoanFormatDto
        {
            Amount = loanAmount,
            InstallmentsCount = installmentsCount
        };

        _sut.Add(admin.Id, dto1);

        var expected = ReadContext.Set<LoanFormat>().Single();

        expected.Should().BeEquivalentTo(
            LoanFormatFactory.Generate(dto1.Amount, dto1.InstallmentsCount),
            o => o.Excluding(l => l.Id));
    }

    [Theory]
    [InlineData(-1)]
    public void Add_throw_exception_if_admin_does_not_exist(int dummyAdminId)
    {
        var actual = () => _sut.Add(dummyAdminId, new AddLoanFormatDto());

        actual.Should().ThrowExactly<AdminNotFoundException>();
        ReadContext.Set<LoanFormat>().Should().HaveCount(0);
    }
}