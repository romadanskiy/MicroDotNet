using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Core.Migrations
{
    public partial class UpdatePosts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Posts_Projects_ProjectId",
                table: "Posts");

            migrationBuilder.DropIndex(
                name: "IX_Posts_ProjectId",
                table: "Posts");

            migrationBuilder.RenameColumn(
                name: "Text",
                table: "Posts",
                newName: "Content");

            migrationBuilder.RenameColumn(
                name: "ProjectId",
                table: "Posts",
                newName: "OwnerId");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateTime",
                table: "Posts",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "EntityType",
                table: "Posts",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateTime",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "EntityType",
                table: "Posts");

            migrationBuilder.RenameColumn(
                name: "OwnerId",
                table: "Posts",
                newName: "ProjectId");

            migrationBuilder.RenameColumn(
                name: "Content",
                table: "Posts",
                newName: "Text");

            migrationBuilder.CreateIndex(
                name: "IX_Posts_ProjectId",
                table: "Posts",
                column: "ProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_Projects_ProjectId",
                table: "Posts",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
