using FluentMigrator;

namespace LoanManagementSystem.Migration.Migrations;
[Migration(140308140751)]
public class _140308140751_AddInstallmentsTable : FluentMigrator.Migration
{
    public override void Up()
    {
        Create.Table("Installments")
            .WithColumn("Id").AsInt32().PrimaryKey().Identity()
            .WithColumn("LoanId").AsInt32().NotNullable()
            .ForeignKey("FK_Installments_Loans", "Loans", "Id")
            .WithColumn("ShouldPayDate").AsDate().NotNullable()
            .WithColumn("PaidDate").AsDate().Nullable();
    }

    public override void Down()
    {
        Delete.Table("Installments");
    }
}