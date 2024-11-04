using LoanManagementSystem.Entities.Loans;

namespace LoanManagementSystem.Services.Loans.Contracts;

public interface LoanRepository
{
    void Add(Loan loan);
    Loan? FindById(int loanId);
    void Update(Loan loan);
    Loan? FindByIdWithLoanFormat(int loanId);
    void UpdateRangeLoanInstallments(Loan loan);
}