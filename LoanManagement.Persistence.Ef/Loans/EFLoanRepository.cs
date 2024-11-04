using LoanManagementSystem.Entities.Installments;
using LoanManagementSystem.Entities.Loans;
using LoanManagementSystem.Services.Loans.Contracts;
using Microsoft.EntityFrameworkCore;

namespace LoanManagementSystem.Persistence.Ef.Loans;

public class EFLoanRepository(EfDataContext context) : LoanRepository
{
    public void Add(Loan loan)
    {
        context.Set<Loan>().Add(loan);
    }

    public Loan? FindById(int loanId)
    {
        return context.Set<Loan>().FirstOrDefault(l => l.Id == loanId);
    }

    public void Update(Loan loan)
    {
        context.Set<Loan>().Update(loan);
    }

    public Loan? FindByIdWithLoanFormat(int loanId)
    {
        return context.Set<Loan>()
            .Include(l => l.LoanFormat)
            .FirstOrDefault(l => l.Id == loanId);
    }

    public void UpdateRangeLoanInstallments(Loan loan)
    {
        context.Set<Installment>().UpdateRange(loan.Installments);
    }
}