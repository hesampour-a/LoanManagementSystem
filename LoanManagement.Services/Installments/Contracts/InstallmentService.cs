namespace LoanManagementSystem.Services.Installments.Contracts;

public interface InstallmentService
{
    void Repayment(int adminId, int installmentId);
}