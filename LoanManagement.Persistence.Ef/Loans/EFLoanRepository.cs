using System.Runtime.InteropServices.JavaScript;
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

    public List<Loan> GetAllDeferreds()
    {
        return context.Set<Loan>().Where(l =>
                l.LoanStatus == LoanStatus.Repaymenting && l.Installments.Any(
                    i =>
                        i.ShouldPayDate <
                        DateOnly.FromDateTime(DateTime.Today)))
            .ToList();
    }

    public void UpdateRange(List<Loan> deferreds)
    {
        context.Set<Loan>().UpdateRange(deferreds);
    }

    public Loan? FindByInstallmentIdIncludeInstallments(int installmentId)
    {
        return context.Set<Loan>()
            .Include(l => l.Installments)
            .FirstOrDefault(
                l => l.Installments.Any(i => i.Id == installmentId));
    }
}