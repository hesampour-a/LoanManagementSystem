using FluentAssertions;
using LoanManagementSystem.Entities.Admins;
using LoanManagementSystem.Persistence.Ef.Admins;
using LoanManagementSystem.Persistence.Ef.UnitOfWorks;
using LoanManagementSystem.Services.Admins;
using LoanManagementSystem.Services.Admins.Contracts;
using LoanManagementSystem.TestTools.Infrastructure.DataBaseConfig.Integration;
using Xunit;

namespace LoanManagementSystem.Service.Unit.Tests.Admins;

public class AdminServiceTests : BusinessIntegrationTest
{
    private readonly AdminService _sut;

    public AdminServiceTests()
    {
        var adminRepository = new EFAdminRepository(SetupContext);
        var unitOfWork = new EfUnitOfWork(SetupContext);
        _sut = new AdminAppService(adminRepository, unitOfWork);
    }

    [Fact]
    public void Add_add_admin_properly()
    {
        var dto = new AddAdminDto
        {
            Name = "Admin",
        };

        var actual = _sut.Add(dto);

        var expected = ReadContext.Set<Admin>().ToList();
        expected.Should().HaveCount(1);
        expected.Should().ContainEquivalentOf(new Admin
        {
            Id = actual,
            Name = dto.Name,
        });
    }
}