using LoanManagementSystem.Services.Admins.Contracts;

namespace LoanManagementSystem.Persistence.Ef.Admins;

public class EFAdminQuery(EfDataContext context) : AdminQuery
{
}