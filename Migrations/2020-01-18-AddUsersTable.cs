using FluentMigrator;

namespace MoneyManager.Migrations
{
    [Migration(1)]
    public class AddUsersTable : Migration
    {
        public override void Up()
        {
            Create.Table("User")
                .WithIdColumn()
                .WithTimeStamps()
                .WithColumn("Email").AsString(255).NotNullable()
                .WithColumn("PasswordHash").AsBinary().NotNullable()
                .WithColumn("PasswordSalt").AsBinary().NotNullable();
        }

        public override void Down()
        {
            Delete.Table("User");
        }

    }
}
