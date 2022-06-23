using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Core.Migrations
{
    public partial class SubscriptionAndBillsUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bills_Subscription_SubscriptionId",
                table: "Bills");

            migrationBuilder.DropForeignKey(
                name: "FK_Subscription_Companies_CompanyId",
                table: "Subscription");

            migrationBuilder.DropForeignKey(
                name: "FK_Subscription_Developers_DeveloperId",
                table: "Subscription");

            migrationBuilder.DropForeignKey(
                name: "FK_Subscription_Developers_SubscriberId",
                table: "Subscription");

            migrationBuilder.DropForeignKey(
                name: "FK_Subscription_Projects_ProjectId",
                table: "Subscription");

            migrationBuilder.DropForeignKey(
                name: "FK_Subscription_Tariffs_TariffId",
                table: "Subscription");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Subscription",
                table: "Subscription");

            migrationBuilder.DropIndex(
                name: "IX_Subscription_CompanyId",
                table: "Subscription");

            migrationBuilder.DropIndex(
                name: "IX_Subscription_DeveloperId",
                table: "Subscription");

            migrationBuilder.DropIndex(
                name: "IX_Subscription_ProjectId",
                table: "Subscription");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "Subscription");

            migrationBuilder.DropColumn(
                name: "DeveloperId",
                table: "Subscription");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "Subscription");

            migrationBuilder.DropColumn(
                name: "ProjectId",
                table: "Subscription");

            migrationBuilder.RenameTable(
                name: "Subscription",
                newName: "Subscriptions");

            migrationBuilder.RenameColumn(
                name: "SubscriptionId",
                table: "Bills",
                newName: "TariffId");

            migrationBuilder.RenameIndex(
                name: "IX_Bills_SubscriptionId",
                table: "Bills",
                newName: "IX_Bills_TariffId");

            migrationBuilder.RenameIndex(
                name: "IX_Subscription_TariffId",
                table: "Subscriptions",
                newName: "IX_Subscriptions_TariffId");

            migrationBuilder.RenameIndex(
                name: "IX_Subscription_SubscriberId",
                table: "Subscriptions",
                newName: "IX_Subscriptions_SubscriberId");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateTime",
                table: "Bills",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Bills",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "EntityType",
                table: "Subscriptions",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "TargetId",
                table: "Subscriptions",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_Subscriptions",
                table: "Subscriptions",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Bills_Tariffs_TariffId",
                table: "Bills",
                column: "TariffId",
                principalTable: "Tariffs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Subscriptions_Developers_SubscriberId",
                table: "Subscriptions",
                column: "SubscriberId",
                principalTable: "Developers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Subscriptions_Tariffs_TariffId",
                table: "Subscriptions",
                column: "TariffId",
                principalTable: "Tariffs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bills_Tariffs_TariffId",
                table: "Bills");

            migrationBuilder.DropForeignKey(
                name: "FK_Subscriptions_Developers_SubscriberId",
                table: "Subscriptions");

            migrationBuilder.DropForeignKey(
                name: "FK_Subscriptions_Tariffs_TariffId",
                table: "Subscriptions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Subscriptions",
                table: "Subscriptions");

            migrationBuilder.DropColumn(
                name: "DateTime",
                table: "Bills");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Bills");

            migrationBuilder.DropColumn(
                name: "EntityType",
                table: "Subscriptions");

            migrationBuilder.DropColumn(
                name: "TargetId",
                table: "Subscriptions");

            migrationBuilder.RenameTable(
                name: "Subscriptions",
                newName: "Subscription");

            migrationBuilder.RenameColumn(
                name: "TariffId",
                table: "Bills",
                newName: "SubscriptionId");

            migrationBuilder.RenameIndex(
                name: "IX_Bills_TariffId",
                table: "Bills",
                newName: "IX_Bills_SubscriptionId");

            migrationBuilder.RenameIndex(
                name: "IX_Subscriptions_TariffId",
                table: "Subscription",
                newName: "IX_Subscription_TariffId");

            migrationBuilder.RenameIndex(
                name: "IX_Subscriptions_SubscriberId",
                table: "Subscription",
                newName: "IX_Subscription_SubscriberId");

            migrationBuilder.AddColumn<Guid>(
                name: "CompanyId",
                table: "Subscription",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DeveloperId",
                table: "Subscription",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "Subscription",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<Guid>(
                name: "ProjectId",
                table: "Subscription",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Subscription",
                table: "Subscription",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Subscription_CompanyId",
                table: "Subscription",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Subscription_DeveloperId",
                table: "Subscription",
                column: "DeveloperId");

            migrationBuilder.CreateIndex(
                name: "IX_Subscription_ProjectId",
                table: "Subscription",
                column: "ProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_Bills_Subscription_SubscriptionId",
                table: "Bills",
                column: "SubscriptionId",
                principalTable: "Subscription",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Subscription_Companies_CompanyId",
                table: "Subscription",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Subscription_Developers_DeveloperId",
                table: "Subscription",
                column: "DeveloperId",
                principalTable: "Developers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Subscription_Developers_SubscriberId",
                table: "Subscription",
                column: "SubscriberId",
                principalTable: "Developers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Subscription_Projects_ProjectId",
                table: "Subscription",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Subscription_Tariffs_TariffId",
                table: "Subscription",
                column: "TariffId",
                principalTable: "Tariffs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
