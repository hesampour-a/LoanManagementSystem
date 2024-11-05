using LoanManagementSystem.Entities.Customers;
using LoanManagementSystem.Services.Customers.Contracts;
using LoanManagementSystem.Services.Customers.Contracts.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LoanManagementSystem.RestApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CusomerController(
    CustomerService customerService,
    CustomerQuery customerQuery) : ControllerBase
{
    [HttpPost("Add")]
    public int Add([FromBody] AddCustomerDto dto)
    {
        return customerService.Add(dto);
    }

    [HttpGet("GetAllWaitingForVerification")]
    public List<GetAllCustomersWaitingForVerificationDto>
        GetAllWaitingForVerification()
    {
        return customerQuery.GetAllCustomersWaitingForVerification();
    }

    [HttpGet("GetAllHighRisks")]
    public List<GetAllHighRisksDto> GetAllHighRisks()
    {
        return customerQuery.GetAllHighRisks();
    }

    [HttpPatch("AddIdentityDocument/{customerId}")]
    public void AddIdentityDocument(int customerId,
        [FromBody] AddIdentityDocumentDto dto)
    {
        customerService.AddIdentityDocument(customerId, dto);
    }

    [HttpPatch("VerifyCustomer/{adminId}")]
    public void VerifyCustomer([FromRoute] int adminId,
        [FromBody] VerifyCustomerDto dto)
    {
        customerService.VerifyCustomer(adminId, dto);
    }

    [HttpPatch("RejectIdentityDocument/{adminId}")]
    public void RejectIdentityDocument([FromRoute] int adminId,
        [FromBody] RejectIdentityDocumentDto dto)
    {
        customerService.RejectIdentityDocument(adminId, dto);
    }

    [HttpPost("AddCustomerFinancialInformation/{customerId}")]
    public void AddCustomerFinancialInformation([FromRoute] int customerId,
        AddCustomerFinancialInformationDto dto)
    {
        customerService.AddCustomerFinancialInformation(customerId, dto);
    }

    [HttpPut("Update/{customerId}")]
    public void Update([FromRoute] int customerId, UpdateCustomerDto dto)
    {
        customerService.Update(customerId, dto);
    }

    [HttpPut("UpdateCustomerFinancialInformation/{customerId}")]
    public void UpdateCustomerFinancialInformation([FromRoute] int customerId,
        UpdateCustomerFinancialInformationDto dto)
    {
        customerService.UpdateCustomerFinancialInformation(customerId, dto);
    }
}