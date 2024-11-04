using LoanManagementSystem.Entities.Installments;

namespace LoanManagementSystem.Services.Installments.Contracts;

public interface InstallmentRepository
{
    void Update(Installment installment);
}