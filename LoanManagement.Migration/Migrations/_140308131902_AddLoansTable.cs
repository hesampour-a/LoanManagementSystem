using System.Data;
using FluentMigrator;

namespace LoanManagementSystem.Migration.Migrations;
[Migration(140308131902)]
public class _140308131902_AddLoansTable : FluentMigrator.Migration
{
    public override void Up()
    {
        Create.Table("Loans")
            .WithColumn("Id").AsInt32().PrimaryKey().Identity()
            .WithColumn("LoanStatus").AsInt32().NotNullable()
            .WithColumn("ValidationScore").AsInt32().NotNullable()
            .WithColumn("CustomerId").AsInt32().NotNullable()
            .WithColumn("LoanFormatId").AsInt32().NotNullable();
        
        Create
            .ForeignKey("FK_Loans_LoanFormats")
            .FromTable("Loans")
            .ForeignColumn("LoanFormatId")
            .ToTable("LoanFormats")
            .PrimaryColumn("Id")
            .OnDelete(Rule.None);
        
        Create
            .ForeignKey("FK_Loans_Customers")
            .FromTable("Loans")
            .ForeignColumn("CustomerId")
            .ToTable("Customers")
            .PrimaryColumn("Id")
            .OnDelete(Rule.None);
    }

    public override void Down()
    {
        Delete.ForeignKey("FK_Loans_Customers").OnTable("Loans");
        Delete.ForeignKey("FK_Loans_LoanFormats").OnTable("Loans");
        Delete.Table("Loans");
    }
}