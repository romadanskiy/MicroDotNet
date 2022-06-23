using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WhiteRabbit_WebApi.Migrations
{
    public partial class addAvatarUrl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AvatarUrl",
                table: "AnimalProfiles",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AvatarUrl",
                table: "AnimalProfiles");
        }
    }
}
