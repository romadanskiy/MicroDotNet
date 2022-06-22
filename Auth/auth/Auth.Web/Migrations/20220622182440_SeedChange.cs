using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuthorizationServer.Web.Migrations
{
    public partial class SeedChange : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("5f34130c-2ed9-4c83-a600-e474e8f48bac"),
                columns: new[] { "CreatedAt", "FirstName", "LastName", "NormalizedUserName", "PhoneNumber", "UserName" },
                values: new object[] { new DateOnly(2022, 6, 22), "John1", "Doe2", "ADMIN@MAIL.RU", "89274868648", "Admin@mail.ru" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("5f34130c-2ed9-4c83-a600-e474e8f48bac"),
                columns: new[] { "CreatedAt", "FirstName", "LastName", "NormalizedUserName", "PhoneNumber", "UserName" },
                values: new object[] { new DateOnly(2022, 4, 3), "John", "Doe", "ADMIN", null, "Admin" });
        }
    }
}
