using FluentMigrator;

namespace LoanManagementSystem.Migration.Migrations;

[Migration(140308131548)]
public class _140308131548_AddLoanFormatsTable : FluentMigrator.Migration
{
    public override void Up()
    {
        Create.Table("LoanFormats")
            .WithColumn("Id").AsInt32().PrimaryKey().Identity()
            .WithColumn("Amount").AsDecimal(18, 2).NotNullable()
            .WithColumn("InstallmentsCount").AsInt32().NotNullable()
            .WithColumn("InterestRate").AsDecimal(5, 2).NotNullable()
            .WithColumn("MonthlyInterestAmount").AsDecimal(18, 2).NotNullable()
            .WithColumn("MonthlyRepayAmount").AsDecimal(18, 2).NotNullable()
            .WithColumn("MonthlyPenaltyAmount").AsDecimal(18, 2).NotNullable();
    }

    public override void Down()
    {
        Delete.Table("LoanFormats");
    }
}