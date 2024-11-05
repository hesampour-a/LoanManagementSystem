using LoanManagementSystem.Services.Installments.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LoanManagementSystem.RestApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class InstallmentController(InstallmentService installmentService)
    : ControllerBase
{
    [HttpPatch("{adminId}/{installmentId}")]
    public void Repayment([FromRoute] int adminId,
        [FromRoute] int installmentId)
    {
        installmentService.Repayment(adminId, installmentId);
    }
}