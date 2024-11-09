using LoanManagementSystem.Entities.Loans;

namespace LoanManagementSystem.Services.Loans.Contracts;

public interface LoanRepository
{
    void Add(Loan loan);
    Loan? FindById(int loanId);
    void Update(Loan loan);
    Loan? FindByIdWithLoanFormat(int loanId);
    void AddRangeLoanInstallments(Loan loan);
    List<Loan> GetAllDeferreds();
    void UpdateRange(List<Loan> deferreds);
    Loan? FindByInstallmentIdIncludeInstallments(int installment1Id);
}