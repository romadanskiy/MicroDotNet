using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class Initialcreateentitiesforutilization : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Garbage",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    Picture = table.Column<string>(type: "text", nullable: false),
                    Barcode = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Garbage", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GarbageReceptionPoint",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    Address = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GarbageReceptionPoint", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GarbageType",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GarbageType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GarbageGarbageType",
                columns: table => new
                {
                    GarbageTypesId = table.Column<long>(type: "bigint", nullable: false),
                    GarbagesId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GarbageGarbageType", x => new { x.GarbageTypesId, x.GarbagesId });
                    table.ForeignKey(
                        name: "FK_GarbageGarbageType_Garbage_GarbagesId",
                        column: x => x.GarbagesId,
                        principalTable: "Garbage",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GarbageGarbageType_GarbageType_GarbageTypesId",
                        column: x => x.GarbageTypesId,
                        principalTable: "GarbageType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GarbageReceptionPointGarbageType",
                columns: table => new
                {
                    GarbageReceptionPointsId = table.Column<long>(type: "bigint", nullable: false),
                    GarbageTypesId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GarbageReceptionPointGarbageType", x => new { x.GarbageReceptionPointsId, x.GarbageTypesId });
                    table.ForeignKey(
                        name: "FK_GarbageReceptionPointGarbageType_GarbageReceptionPoint_Garb~",
                        column: x => x.GarbageReceptionPointsId,
                        principalTable: "GarbageReceptionPoint",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GarbageReceptionPointGarbageType_GarbageType_GarbageTypesId",
                        column: x => x.GarbageTypesId,
                        principalTable: "GarbageType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GarbageGarbageType_GarbagesId",
                table: "GarbageGarbageType",
                column: "GarbagesId");

            migrationBuilder.CreateIndex(
                name: "IX_GarbageReceptionPointGarbageType_GarbageTypesId",
                table: "GarbageReceptionPointGarbageType",
                column: "GarbageTypesId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GarbageGarbageType");

            migrationBuilder.DropTable(
                name: "GarbageReceptionPointGarbageType");

            migrationBuilder.DropTable(
                name: "Garbage");

            migrationBuilder.DropTable(
                name: "GarbageReceptionPoint");

            migrationBuilder.DropTable(
                name: "GarbageType");
        }
    }
}
