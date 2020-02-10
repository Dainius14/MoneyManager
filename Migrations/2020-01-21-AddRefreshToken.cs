using FluentMigrator;

namespace MoneyManager.Migrations
{
    [Migration(1)]
    public class AddRefreshToken : Migration
    {
        public override void Up()
        {
            Create.Table("RefreshToken")
                .WithIdColumn()
                .WithColumn("UserId").AsInt32().ForeignKey("User", "Id").NotNullable()
                .WithColumn("Token").AsString(64).NotNullable()
                .WithColumn("IsValid").AsBoolean().NotNullable()
                .WithColumn("IssuedAt").AsDateTime().NotNullable()
                .WithColumn("ExpiresAt").AsDateTime().NotNullable();
        }

        public override void Down()
        {
            Delete.Table("RefreshToken");
        }

    }
}
