using FluentMigrator;

namespace MoneyManager.Migrations
{
    [Migration(3)]
    public class AddUserIdToTables : Migration
    {
        public override void Up()
        {
            Alter.Table("Account")
                .AddColumn("UserId").AsInt32().ForeignKey("User", "Id").Nullable()
                .AlterColumn("UserId").AsInt32().NotNullable();  // Workaround for SQLite bug not allowing to add non nullable columns

            Alter.Table("Category")
                .AddColumn("UserId").AsInt32().ForeignKey("User", "Id").Nullable()
                .AlterColumn("UserId").AsInt32().NotNullable();

            Alter.Table("Transaction")
                .AddColumn("UserId").AsInt32().ForeignKey("User", "Id").Nullable()
                .AlterColumn("UserId").AsInt32().NotNullable();
        }

        public override void Down()
        {
            Delete.Column("UserId").FromTable("Account");
            Delete.Column("UserId").FromTable("Category");
            Delete.Column("UserId").FromTable("Transaction");
        }

    }
}
