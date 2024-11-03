using LoanManagementSystem.Entities.LoanFormats;
using LoanManagementSystem.Services.Admins.Contracts;
using LoanManagementSystem.Services.Admins.Exceptions;
using LoanManagementSystem.Services.Calculators;
using LoanManagementSystem.Services.LoanFormats.Contracts;
using LoanManagementSystem.Services.UnitOfWorks;

namespace LoanManagementSystem.Services.LoanFormats;

public class LoanFormatAppService(
    LoanFormatRepository loanFormatRepository,
    AdminRepository adminRepository,
    UnitOfWork unitOfWork) : LoanFormatService
{
    public void Add(int adminId, AddLoanFormatDto dto)
    {
        var admin = adminRepository.FindById(adminId)
                    ?? throw new AdminNotFoundException();

        var loanFormat = new LoanFormat
        {
            Amount = dto.Amount,
            InstallmentsCount = dto.InstallmentsCount,
            InterestRate = Calculator.InterestRate(dto.InstallmentsCount),
            MonthlyInterestAmount =
                Calculator.MonthlyInterestAmount(dto.Amount,
                    dto.InstallmentsCount),
            MonthlyRepayAmount =
                Calculator.MonthlyRepayAmount(dto.Amount,
                    dto.InstallmentsCount),
            MonthlyPenaltyAmount =
                Calculator.MonthlyPenaltyAmount(dto.Amount,
                    dto.InstallmentsCount),
        };

        loanFormatRepository.Add(loanFormat);
        unitOfWork.Save();
    }
}