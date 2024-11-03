using LoanManagementSystem.Entities.Customers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LoanManagementSystem.Persistence.Ef.Customers;

public class
    CustomerFinancialInformationEntityMap : IEntityTypeConfiguration<
    CustomerFinancialInformation>
{
    public void Configure(
        EntityTypeBuilder<CustomerFinancialInformation> builder)
    {
        builder.ToTable("CustomerFinancialInformations");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd();
        builder.HasOne(_ => _.Customer).WithOne(_ => _.FinancialInformation)
            .HasForeignKey<CustomerFinancialInformation>(_ => _.CustomerId);
    }
}