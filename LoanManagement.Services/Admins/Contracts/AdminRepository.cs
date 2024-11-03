using LoanManagementSystem.Entities.Admins;

namespace LoanManagementSystem.Services.Admins.Contracts;

public interface AdminRepository
{
    void Add(Admin admin);
    Admin? FindById(int adminId);
}