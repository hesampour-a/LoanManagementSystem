using LoanManagementSystem.Entities.Installments;
using LoanManagementSystem.Services.Installments.Contracts;

namespace LoanManagementSystem.Persistence.Ef.Installments;

public class EFInstallmentRepository(EfDataContext context)
    : InstallmentRepository
{
    public void Update(Installment installment)
    {
        context.Set<Installment>().Update(installment);
    }
}