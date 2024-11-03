namespace LoanManagementSystem.Services.LoanFormats.Contracts;

public interface LoanFormatService
{
    void Add(int adminId, AddLoanFormatDto dto);
}

public class AddLoanFormatDto
{
    public decimal Amount { get; set; }
    public int InstallmentsCount { get; set; }
}