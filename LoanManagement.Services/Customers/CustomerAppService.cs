using LoanManagementSystem.Entities.Customers;
using LoanManagementSystem.Services.Admins.Contracts;
using LoanManagementSystem.Services.Admins.Exceptions;
using LoanManagementSystem.Services.Customers.Contracts;
using LoanManagementSystem.Services.Customers.Contracts.DTOs;
using LoanManagementSystem.Services.Customers.Exceptions;
using LoanManagementSystem.Services.UnitOfWorks;

namespace LoanManagementSystem.Services.Customers;

public class CustomerAppService(
    CustomerRepository customerRepository,
    CustomerQuery customerQuery,
    AdminRepository adminRepository,
    UnitOfWork unitOfWork) : CustomerService
{
    public int Add(AddCustomerDto dto)
    {
        if (customerRepository.IsDuplicated(dto.NationalCode))
        {
            throw new NationalCodeAlreadyExistsException();
        }

        var customer = new Customer
        {
            Email = dto.Email,
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            NationalCode = dto.NationalCode,
            PhoneNumber = dto.PhoneNumber,
            IsVerified = false,
        };
        customerRepository.Add(customer);
        unitOfWork.Save();
        return customer.Id;
    }

    public List<GetAllCustomersWaitingForVerificationDto>
        GetAllCustomersWaitingForVerification(int adminId)
    {
        var admin = adminRepository.FindById(adminId)
                    ?? throw new AdminNotFoundException();
        return customerQuery.GetAllCustomersWaitingForVerification();
    }

    public void AddIdentityDocument(int customerId, AddIdentityDocumentDto dto)
    {
        var customer = customerRepository.FindById(customerId)
                       ?? throw new CustomerNotFoundException();

        if (customer.IdentityDocument != null)
        {
            throw new IdentityDocumentAlreadyExistsException();
        }

        customer.IdentityDocument = dto.IdentityDocument;
        customerRepository.Update(customer);
        unitOfWork.Save();
    }

    public void VerifyCustomer(int adminId, VerifyCustomerDto dto)
    {
        var admin = adminRepository.FindById(adminId)
                    ?? throw new AdminNotFoundException();
        var customer = customerRepository.FindById(dto.customerId)
                       ?? throw new CustomerNotFoundException();

        if (customer.IdentityDocument == null)
        {
            throw new CustomerDoseNotHaveIdentityDocumentException();
        }

        customer.IsVerified = true;
        customerRepository.Update(customer);
        unitOfWork.Save();
    }

    public void UpdateCustomerFinancialInformation(int customerId,
        UpdateCustomerFinancialInformationDto dto)
    {
        var customer = customerRepository.FindById(customerId)
                       ?? throw new CustomerNotFoundException();

        customer.FinancialInformation = new CustomerFinancialInformation
        {
            JobType = dto.JobType,
            MonthlyIncome = dto.MonthlyIncome,
            TotalAssetsValue = dto.TotalAssetsValue,
        };

        customerRepository.CustomerFinancialInformation(customer);
        unitOfWork.Save();
    }

    public void Update(int customerId, UpdateCustomerDto dto)
    {
        var customer = customerRepository.FindById(customerId)
                       ?? throw new CustomerNotFoundException();
        if (customer.NationalCode != dto.NationalCode)
        {
            if (customerRepository.IsDuplicated(dto.NationalCode))
            {
                throw new NationalCodeAlreadyExistsException();
            }
        }

        customer.Email = dto.Email;
        customer.FirstName = dto.FirstName;
        customer.LastName = dto.LastName;
        customer.NationalCode = dto.NationalCode;
        customer.PhoneNumber = dto.PhoneNumber;
        customer.IsVerified = false;
        customer.IdentityDocument = null;

        customerRepository.Update(customer);
        unitOfWork.Save();
    }

    public void RejectIdentityDocument(int adminId,
        RejectIdentityDocumentDto dto)
    {
        var admin = adminRepository.FindById(adminId)
                    ?? throw new AdminNotFoundException();
        var customer = customerRepository.FindById(dto.CustomerId)
                       ?? throw new CustomerNotFoundException();

        customer.IsVerified = false;
        customer.IdentityDocument = null;
        customerRepository.Update(customer);
        unitOfWork.Save();
    }
}