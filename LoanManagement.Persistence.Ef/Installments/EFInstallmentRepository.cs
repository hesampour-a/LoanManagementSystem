using LoanManagementSystem.Services.Installments.Contracts;

namespace LoanManagementSystem.Persistence.Ef.Installments;

public class EFInstallmentRepository(EfDataContext context)
    : InstallmentRepository
{
}