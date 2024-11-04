using LoanManagementSystem.Entities.Loans;

namespace LoanManagementSystem.TestTools.Loans;

public class LoanBuilder(int customerId, int loanFormatId)
{
    private Loan _loan = new Loan
    {
        LoanFormatId = loanFormatId,
        CustomerId = customerId,
        ValidationScore = 70,
        LoanStatus = LoanStatus.Pending,
    };

    public LoanBuilder WithId(int loanId)
    {
        _loan.Id = loanId;
        return this;
    }

    public LoanBuilder WithValidationScore(int validationScore)
    {
        _loan.ValidationScore = validationScore;
        return this;
    }

    public LoanBuilder WithLoanStatus(LoanStatus loanStatus)
    {
        _loan.LoanStatus = loanStatus;
        return this;
    }

    public Loan Build()
    {
        return _loan;
    }
}