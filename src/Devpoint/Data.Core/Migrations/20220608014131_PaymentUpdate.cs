using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Core.Migrations
{
    public partial class PaymentUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DateTime",
                table: "Withdrawals",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DateTime",
                table: "Replenishments",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.InsertData(
                table: "SubscriptionLevels",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 5, "Team member" },
                    { 6, "Owner" }
                });

            migrationBuilder.UpdateData(
                table: "Tariffs",
                keyColumn: "Id",
                keyValue: 7,
                column: "SubscriptionLevelId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Tariffs",
                keyColumn: "Id",
                keyValue: 8,
                column: "SubscriptionLevelId",
                value: 2);

            migrationBuilder.UpdateData(
                table: "Tariffs",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "SubscriptionLevelId", "SubscriptionType" },
                values: new object[] { 3, 0 });

            migrationBuilder.UpdateData(
                table: "Tariffs",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "SubscriptionLevelId", "SubscriptionType" },
                values: new object[] { 4, 0 });

            migrationBuilder.InsertData(
                table: "Tariffs",
                columns: new[] { "Id", "PricePerMonth", "SubscriptionLevelId", "SubscriptionType" },
                values: new object[,]
                {
                    { 13, 100, 1, 2 },
                    { 14, 100, 2, 2 },
                    { 15, 100, 3, 2 },
                    { 16, 100, 4, 2 }
                });

            migrationBuilder.UpdateData(
                table: "Tariffs",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "PricePerMonth", "SubscriptionLevelId", "SubscriptionType" },
                values: new object[] { 0, 5, 1 });

            migrationBuilder.UpdateData(
                table: "Tariffs",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "PricePerMonth", "SubscriptionLevelId", "SubscriptionType" },
                values: new object[] { 0, 6, 1 });

            migrationBuilder.UpdateData(
                table: "Tariffs",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "PricePerMonth", "SubscriptionLevelId", "SubscriptionType" },
                values: new object[] { 0, 5, 0 });

            migrationBuilder.UpdateData(
                table: "Tariffs",
                keyColumn: "Id",
                keyValue: 12,
                columns: new[] { "PricePerMonth", "SubscriptionLevelId", "SubscriptionType" },
                values: new object[] { 0, 6, 0 });

            migrationBuilder.InsertData(
                table: "Tariffs",
                columns: new[] { "Id", "PricePerMonth", "SubscriptionLevelId", "SubscriptionType" },
                values: new object[,]
                {
                    { 17, 0, 5, 2 },
                    { 18, 0, 6, 2 }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Tariffs",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Tariffs",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Tariffs",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "Tariffs",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "Tariffs",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "Tariffs",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "SubscriptionLevels",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "SubscriptionLevels",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DropColumn(
                name: "DateTime",
                table: "Withdrawals");

            migrationBuilder.DropColumn(
                name: "DateTime",
                table: "Replenishments");

            migrationBuilder.UpdateData(
                table: "Tariffs",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "PricePerMonth", "SubscriptionLevelId", "SubscriptionType" },
                values: new object[] { 100, 1, 0 });

            migrationBuilder.UpdateData(
                table: "Tariffs",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "PricePerMonth", "SubscriptionLevelId", "SubscriptionType" },
                values: new object[] { 100, 2, 0 });

            migrationBuilder.UpdateData(
                table: "Tariffs",
                keyColumn: "Id",
                keyValue: 7,
                column: "SubscriptionLevelId",
                value: 3);

            migrationBuilder.UpdateData(
                table: "Tariffs",
                keyColumn: "Id",
                keyValue: 8,
                column: "SubscriptionLevelId",
                value: 4);

            migrationBuilder.UpdateData(
                table: "Tariffs",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "SubscriptionLevelId", "SubscriptionType" },
                values: new object[] { 1, 2 });

            migrationBuilder.UpdateData(
                table: "Tariffs",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "SubscriptionLevelId", "SubscriptionType" },
                values: new object[] { 2, 2 });

            migrationBuilder.UpdateData(
                table: "Tariffs",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "PricePerMonth", "SubscriptionLevelId", "SubscriptionType" },
                values: new object[] { 100, 3, 2 });

            migrationBuilder.UpdateData(
                table: "Tariffs",
                keyColumn: "Id",
                keyValue: 12,
                columns: new[] { "PricePerMonth", "SubscriptionLevelId", "SubscriptionType" },
                values: new object[] { 100, 4, 2 });
        }
    }
}
