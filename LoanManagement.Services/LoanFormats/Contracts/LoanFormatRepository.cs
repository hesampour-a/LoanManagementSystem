using LoanManagementSystem.Entities.LoanFormats;

namespace LoanManagementSystem.Services.LoanFormats.Contracts;

public interface LoanFormatRepository
{
    void Add(LoanFormat loanFormat);
    LoanFormat? FindById(int dtoLoanFormatId);
}