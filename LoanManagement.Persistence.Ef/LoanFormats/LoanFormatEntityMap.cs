using LoanManagementSystem.Entities.LoanFormats;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LoanManagementSystem.Persistence.Ef.LoanFormats;

public class LoanFormatEntityMap : IEntityTypeConfiguration<LoanFormat>
{
    public void Configure(EntityTypeBuilder<LoanFormat> builder)
    {
        builder.ToTable("LoanFormats");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd();
    }
}