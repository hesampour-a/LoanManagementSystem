using LoanManagementSystem.Entities.Admins;
using LoanManagementSystem.Services.Admins.Contracts;
using LoanManagementSystem.Services.UnitOfWorks;

namespace LoanManagementSystem.Services.Admins;

public class AdminAppService(
    AdminRepository adminRepository,
    UnitOfWork unitOfWork) : AdminService
{
    public int Add(AddAdminDto dto)
    {
        var admin = new Admin
        {
            Name = dto.Name,
        };

        adminRepository.Add(admin);
        unitOfWork.Save();
        return admin.Id;
    }
}