using FluentMigrator;

namespace MoneyManager.Migrations
{
    [Migration(2)]
    public class AddOpeningBalanceToAccount : Migration
    {
        public override void Up()
        {
            Alter.Table("Account")
                .AddColumn("OpeningBalance").AsDouble().Nullable()
                .AddColumn("OpeningBalanceCurrencyId").AsInt32().Nullable();
        }

        public override void Down()
        {
            Delete.Column("OpeningBalance").FromTable("Account");
            Delete.Column("OpeningBalanceCurrencyId").FromTable("Account");
        }

    }
}
