using LoanManagementSystem.Services.Admins.Contracts;
using LoanManagementSystem.Services.Admins.Contracts.DTOs;
using LoanManagementSystem.Services.Customers.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace LoanManagementSystem.RestApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AdminController(
    AdminService adminService) : ControllerBase
{
    [HttpPost]
    public int Add([FromBody] AddAdminDto dto)
    {
        return adminService.Add(dto);
    }
}