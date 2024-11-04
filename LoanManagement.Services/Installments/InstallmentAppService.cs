using LoanManagementSystem.Services.Installments.Contracts;
using LoanManagementSystem.Services.UnitOfWorks;

namespace LoanManagementSystem.Services.Installments;

public class InstallmentAppService(
    InstallmentRepository installmentRepository,
    UnitOfWork unitOfWork) : InstallmentService
{
}