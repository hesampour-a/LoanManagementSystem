using LoanManagementSystem.Entities.Loans;

namespace LoanManagementSystem.Services.Loans.Contracts.DTOs;

public class GetAllRepaymentingAndDeferredDto
{
    public int Id { get; set; }
    public int CustomerId { get; set; }
    public int LoanFormatId { get; set; }
    public LoanStatus LoanStatus { get; set; }
    public int ValidationScore { get; set; }
    public decimal TotalPaidUntilNow { get; set; }

    public List<GetAllRemainingInstallmentsDto> RemainingInstallments
    {
        get;
        set;
    } = [];
}