using LoanManagementSystem.Entities.Admins;
using LoanManagementSystem.Services.Admins.Contracts;

namespace LoanManagementSystem.Persistence.Ef.Admins;

public class EFAdminRepository(EfDataContext context) : AdminRepository
{
    public void Add(Admin admin)
    {
        context.Set<Admin>().Add(admin);
    }

    public Admin? FindById(int adminId)
    {
        return context.Set<Admin>().FirstOrDefault(x => x.Id == adminId);
    }
}