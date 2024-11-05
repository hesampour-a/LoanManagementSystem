using LoanManagementSystem.Services.Loans.Contracts.DTOs;

namespace LoanManagementSystem.Services.Loans.Contracts;

public interface LoanQuery
{
    List<GetAllPendingLoansDto> GetAllPendings();
    List<GetAllRepaymentingAndDeferredDto> GetAllRepaymentingAndDeferred();
    GetMonthlyIncomeDto GetMonthlyIncome(DateOnly date);
    List<GetAllClosedDto> GetAllClosed();
}