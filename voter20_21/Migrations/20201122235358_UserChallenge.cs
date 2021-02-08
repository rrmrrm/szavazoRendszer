using Microsoft.EntityFrameworkCore.Migrations;

namespace voter20_21.Migrations
{
    public partial class UserChallenge : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "userChallenge",
                table: "Users",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "userChallenge",
                table: "Users");
        }
    }
}
