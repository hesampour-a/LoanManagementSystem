using LoanManagementSystem.Services.LoanFormats.Contracts;
using LoanManagementSystem.Services.LoanFormats.Contracts.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LoanManagementSystem.RestApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class LoanFormatController(
    LoanFormatService loanFormatService,
    LoanFormatQuery loanFormatQuery) : ControllerBase
{
    [HttpPost("{adminId}")]
    public void Add([FromRoute] int adminId, [FromBody] AddLoanFormatDto dto)
    {
        loanFormatService.Add(adminId, dto);
    }

    [HttpGet]
    public List<GetAllLoanFormatsDto> GetAll()
    {
        return loanFormatQuery.GetAll();
    }
}