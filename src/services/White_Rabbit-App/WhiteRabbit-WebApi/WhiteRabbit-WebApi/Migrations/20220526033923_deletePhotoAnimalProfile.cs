using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WhiteRabbit_WebApi.Migrations
{
    public partial class deletePhotoAnimalProfile : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Photo",
                table: "AnimalProfiles");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "Photo",
                table: "AnimalProfiles",
                type: "bytea",
                nullable: true);
        }
    }
}
