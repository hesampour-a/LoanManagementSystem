using LoanManagementSystem.Services.LoanFormats.Contracts.DTOs;

namespace LoanManagementSystem.Services.LoanFormats.Contracts;

public interface LoanFormatQuery
{
    List<GetAllLoanFormatsDto> GetAll();
}