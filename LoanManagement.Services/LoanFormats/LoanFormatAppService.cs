using LoanManagementSystem.Entities.LoanFormats;
using LoanManagementSystem.Services.Admins.Contracts;
using LoanManagementSystem.Services.Admins.Exceptions;

using LoanManagementSystem.Services.LoanFormats.Contracts;
using LoanManagementSystem.Services.LoanFormats.Contracts.DTOs;
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
            InterestRate = ClaculateInterestRate(dto.InstallmentsCount),
            MonthlyInterestAmount =
                ClaculateMonthlyInterestAmount(dto.Amount,
                    dto.InstallmentsCount),
            MonthlyRepayAmount =
                ClaculateMonthlyRepayAmount(dto.Amount,
                    dto.InstallmentsCount),
            MonthlyPenaltyAmount =
                ClaculateMonthlyPenaltyAmount(dto.Amount,
                    dto.InstallmentsCount),
        };

        loanFormatRepository.Add(loanFormat);
        unitOfWork.Save();
    }

    private decimal ClaculateInterestRate(int installmentsCount)
    {
        return Math.Round(installmentsCount <= 12 ? 0.15m : 0.20m, 2);
    }

    private decimal ClaculateMonthlyInterestAmount(decimal amount,
        int installmentsCount)
    {
        return Math.Round(
            (ClaculateInterestRate(installmentsCount) / 12) * amount, 2);
    }

    private decimal ClaculateMonthlyRepayAmount(decimal amount,
        int installmentsCount)
    {
        return Math.Round(amount / installmentsCount, 2);
    }

    private decimal ClaculateMonthlyPenaltyAmount(decimal amount,
        int installmentsCount)
    {
        return Math.Round(0.02m *
                          ((ClaculateMonthlyInterestAmount(amount,
                               installmentsCount)) +
                           (ClaculateMonthlyRepayAmount(amount,
                               installmentsCount))), 2);
    }
}