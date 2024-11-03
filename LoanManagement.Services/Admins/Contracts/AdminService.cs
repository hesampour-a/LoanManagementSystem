using LoanManagementSystem.Services.Admins.Contracts.DTOs;

namespace LoanManagementSystem.Services.Admins.Contracts;

public interface AdminService
{
    int Add(AddAdminDto dto);
}