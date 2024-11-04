using LoanManagementSystem.Entities.Installments;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LoanManagementSystem.Persistence.Ef.Installments;

public class InstallmentEntityMap : IEntityTypeConfiguration<Installment>
{
    public void Configure(EntityTypeBuilder<Installment> builder)
    {
        builder.ToTable("Installments");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd();
        builder
            .HasOne(i => i.Loan)
            .WithMany(i => i.Installments)
            .HasForeignKey(i => i.LoanId);
    }
}