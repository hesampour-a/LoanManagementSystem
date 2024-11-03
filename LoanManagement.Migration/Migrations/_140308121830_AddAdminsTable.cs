using FluentMigrator;

namespace LoanManagementSystem.Migration.Migrations;
[Migration(140308121830)]
public class _140308121830_AddAdminsTable : FluentMigrator.Migration
{
    public override void Up()
    {
        Create.Table("Admins")
            .WithColumn("Id").AsInt32().PrimaryKey().Identity()
            .WithColumn("Name").AsString().NotNullable();
    }

    public override void Down()
    {
       Delete.Table("Admins");
    }
}