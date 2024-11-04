using LoanManagementSystem.Entities.Loans;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LoanManagementSystem.Persistence.Ef.Loans;

public class LoanEntityMap : IEntityTypeConfiguration<Loan>
{
    public void Configure(EntityTypeBuilder<Loan> builder)
    {
        builder.ToTable("Loans");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd();
        builder.HasOne(l => l.Customer)
            .WithMany(l => l.Loans)
            .HasForeignKey(l => l.CustomerId)
            .OnDelete(DeleteBehavior.NoAction);
        builder.HasOne(l => l.LoanFormat)
            .WithMany(l => l.Loans)
            .HasForeignKey(l => l.LoanFormatId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}