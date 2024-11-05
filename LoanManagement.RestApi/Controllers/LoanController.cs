using LoanManagementSystem.Entities.Loans;
using LoanManagementSystem.Services.Loans.Contracts;
using LoanManagementSystem.Services.Loans.Contracts.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LoanManagementSystem.RestApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class LoanController(LoanService loanService, LoanQuery loanQuery)
    : ControllerBase
{
    [HttpGet("GetAllPendings")]
    public List<GetAllPendingLoansDto> GetAllPendings()
    {
        return loanQuery.GetAllPendings();
    }

    [HttpGet("GetAllRepaymentingAndDeferred")]
    public List<GetAllRepaymentingAndDeferredDto>
        GetAllRepaymentingAndDeferred()
    {
        return loanQuery.GetAllRepaymentingAndDeferred();
    }

    [HttpGet("GetMonthlyIncome")]
    public GetMonthlyIncomeDto GetMonthlyIncome(DateOnly date)
    {
        return loanQuery.GetMonthlyIncome(date);
    }

    [HttpGet("GetAllClosed")]
    public List<GetAllClosedDto> GetAllClosed()
    {
        return loanQuery.GetAllClosed();
    }

    [HttpPost("Add/{customerId}")]
    public void Add([FromRoute] int customerId, [FromBody] AddLoanDto dto)
    {
        loanService.Add(customerId, dto);
    }

    [HttpPatch("Confirm/{adminId}/{loanId}")]
    public void Confirm([FromRoute] int adminId, [FromRoute] int loanId)
    {
        loanService.Confirm(adminId, loanId);
    }

    [HttpPatch("Reject/{adminId}/{loanId}")]
    public void Reject([FromRoute] int adminId, [FromRoute] int loanId)
    {
        loanService.Reject(adminId, loanId);
    }

    [HttpPatch("Pay/{adminId}/{loanId}")]
    public void Pay([FromRoute] int adminId, [FromRoute] int loanId)
    {
        loanService.Pay(adminId, loanId);
    }

    [HttpPut("UpdateDeferreds")]
    public void UpdateDeferreds()
    {
        loanService.UpdateDeferreds();
    }
}