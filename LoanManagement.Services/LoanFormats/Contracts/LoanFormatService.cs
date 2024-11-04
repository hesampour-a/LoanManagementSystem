using LoanManagementSystem.Services.LoanFormats.Contracts.DTOs;

namespace LoanManagementSystem.Services.LoanFormats.Contracts;

public interface LoanFormatService
{
    void Add(int adminId, AddLoanFormatDto dto);
}