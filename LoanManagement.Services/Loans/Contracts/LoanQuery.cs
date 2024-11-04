using LoanManagementSystem.Services.Loans.Contracts.DTOs;

namespace LoanManagementSystem.Services.Loans.Contracts;

public interface LoanQuery
{
    List<GetAllPendingLoansDto> GetAllPendings();
}