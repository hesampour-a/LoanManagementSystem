using LoanManagementSystem.Services.Loans.Contracts.DTOs;

namespace LoanManagementSystem.Services.Loans.Contracts;

public interface LoanService
{
    void Add(int customerId, AddLoanDto dto);
    void Confirm(int adminId, int loanId);
    void Reject(int adminId, int loan2Id);
    void Pay(int adminId, int loan2Id);
    void UpdateDeferreds();
}