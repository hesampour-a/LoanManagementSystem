using FluentMigrator;

namespace LoanManagementSystem.Migration.Migrations;
[Migration(140308130720)]
public class _140308130720_AddCustomersTable : FluentMigrator.Migration
{
    public override void Up()
    {
        Create.Table("Customers")
            .WithColumn("Id").AsInt32().PrimaryKey().Identity()
            .WithColumn("FirstName").AsString().NotNullable()
            .WithColumn("LastName").AsString().NotNullable()
            .WithColumn("PhoneNumber").AsString().NotNullable()
            .WithColumn("NationalCode").AsString().NotNullable()
            .WithColumn("Email").AsString().NotNullable()
            .WithColumn("IsVerified").AsBoolean().NotNullable()
            .WithColumn("IdentityDocument").AsString().Nullable();

        Create.Table("CustomerFinancialInformations")
            .WithColumn("Id").AsInt32().PrimaryKey().Identity()
            .WithColumn("MonthlyIncome").AsDecimal().NotNullable()
            .WithColumn("TotalAssetsValue").AsDecimal().NotNullable()
            .WithColumn("JobType").AsInt32().NotNullable()
            .WithColumn("CustomerId").AsInt32().NotNullable()
            .ForeignKey("FK_CustomerFinancialInformations_Customers", "Customers", "Id");
    }

    public override void Down()
    {
        Delete.Table("CustomerFinancialInformations");
        Delete.Table("Customers");
    }
}