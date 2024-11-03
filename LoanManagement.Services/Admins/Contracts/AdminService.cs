namespace LoanManagementSystem.Services.Admins.Contracts;

public interface AdminService
{
    int Add(AddAdminDto dto);
}

public class AddAdminDto
{
    public required string Name { get; set; }
}