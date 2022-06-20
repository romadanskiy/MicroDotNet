using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Core.Migrations
{
    public partial class AddSeedData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "SubscriptionLevels",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Free" },
                    { 2, "Basic" },
                    { 3, "Improved" },
                    { 4, "Pro" }
                });

            migrationBuilder.InsertData(
                table: "Tariffs",
                columns: new[] { "Id", "PricePerMonth", "SubscriptionLevelId", "SubscriptionType" },
                values: new object[,]
                {
                    { 1, 100, 1, 1 },
                    { 2, 100, 2, 1 },
                    { 3, 100, 3, 1 },
                    { 4, 100, 4, 1 },
                    { 5, 100, 1, 0 },
                    { 6, 100, 2, 0 },
                    { 7, 100, 3, 0 },
                    { 8, 100, 4, 0 },
                    { 9, 100, 1, 2 },
                    { 10, 100, 2, 2 },
                    { 11, 100, 3, 2 },
                    { 12, 100, 4, 2 }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Tariffs",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Tariffs",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Tariffs",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Tariffs",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Tariffs",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Tariffs",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Tariffs",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Tariffs",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Tariffs",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Tariffs",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Tariffs",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Tariffs",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "SubscriptionLevels",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "SubscriptionLevels",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "SubscriptionLevels",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "SubscriptionLevels",
                keyColumn: "Id",
                keyValue: 4);
        }
    }
}
