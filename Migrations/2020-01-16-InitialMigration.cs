using FluentMigrator;

namespace MoneyManager.Migrations
{
    [Migration(0)]
    public class InitialMigration : Migration
    {
        public override void Up()
        {
            Create.Table("Account")
                .WithIdColumn()
                .WithTimeStamps()
                .WithColumn("Name").AsString(255).NotNullable()
                .WithColumn("IsPersonal").AsBoolean().NotNullable();

            Create.Table("Category")
                .WithIdColumn()
                .WithTimeStamps()
                .WithColumn("Name").AsString(255).NotNullable()
                .WithColumn("ParentId").AsInt32().Nullable().ForeignKey("Category", "Id");

            Create.Table("Currency")
                .WithIdColumn()
                .WithColumn("Name").AsString(255).NotNullable()
                .WithColumn("IsoCode").AsString(7).NotNullable()
                .WithColumn("Symbol").AsString(3).NotNullable();

            Insert.IntoTable("Currency")
                .Row(new { Name = "Euro", IsoCode = "EUR", Symbol = "€" })
                .Row(new { Name = "United States dollar", IsoCode = "USD", Symbol = "$" });

            Create.Table("Transaction")
                .WithIdColumn()
                .WithTimeStamps()
                .WithColumn("Description").AsString(255).Nullable()
                .WithColumn("Date").AsDate().NotNullable();

            Create.Table("TransactionDetails")
                .WithIdColumn()
                .WithTimeStamps()
                .WithColumn("TransactionId").AsInt32().NotNullable().ForeignKey("Transaction", "Id")
                .WithColumn("Amount").AsDouble().NotNullable()
                .WithColumn("CurrencyId").AsInt32().NotNullable().ForeignKey("Currency", "Id")
                .WithColumn("FromAccountId").AsInt32().NotNullable().ForeignKey("Account", "Id")
                .WithColumn("ToAccountId").AsInt32().NotNullable().ForeignKey("Account", "Id")
                .WithColumn("CategoryId").AsInt32().NotNullable().ForeignKey("Category", "Id");
        }

        public override void Down()
        {
            Delete.Table("TransactionDetails");
            Delete.Table("Transaction");
            Delete.Table("Category");
            Delete.Table("Currency");
            Delete.Table("Account");
        }

    }
}
