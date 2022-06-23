using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RuOverflow.Questions.Migrations
{
    public partial class AddedQuestionSeed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Questions",
                columns: new[] { "Id", "Body", "Created", "Modified", "Rating", "Title", "UserId" },
                values: new object[,]
                {
                    { new Guid("63645098-b77b-49ad-b924-bf1d6dfadef8"), "Тело2?", new DateTime(2022, 4, 19, 18, 31, 47, 88, DateTimeKind.Utc).AddTicks(3022), null, 0, "Контент2", new Guid("84b9f2ce-d9d3-4cfd-b90d-0d62eb50abf9") },
                    { new Guid("63692500-6acf-4a74-9b6a-b99b092b0841"), "Тело1?", new DateTime(2022, 4, 19, 18, 31, 47, 88, DateTimeKind.Utc).AddTicks(1620), null, 0, "Контент1", new Guid("358bdac5-6ac6-4776-87df-d94b6b7db0ee") },
                    { new Guid("98a9c0e6-9661-4a06-9796-c969f009f875"), "Тело5?", new DateTime(2022, 4, 19, 18, 31, 47, 88, DateTimeKind.Utc).AddTicks(3029), null, 0, "Контент5", new Guid("26d1db82-fd40-476f-9bd1-7367fa7793bb") },
                    { new Guid("c122f210-041d-4871-b173-00fc945f58d1"), "Тело4?", new DateTime(2022, 4, 19, 18, 31, 47, 88, DateTimeKind.Utc).AddTicks(3028), null, 0, "Контент4", new Guid("8b364fad-a5a4-4ecf-a990-bdb941191cc2") },
                    { new Guid("e12506fe-b6ae-43dc-9bea-927bd1d82788"), "Тело3?", new DateTime(2022, 4, 19, 18, 31, 47, 88, DateTimeKind.Utc).AddTicks(3025), null, 0, "Контент3", new Guid("86118127-0de2-43e0-89e0-0150a2196e59") }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("63645098-b77b-49ad-b924-bf1d6dfadef8"));

            migrationBuilder.DeleteData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("63692500-6acf-4a74-9b6a-b99b092b0841"));

            migrationBuilder.DeleteData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("98a9c0e6-9661-4a06-9796-c969f009f875"));

            migrationBuilder.DeleteData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("c122f210-041d-4871-b173-00fc945f58d1"));

            migrationBuilder.DeleteData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("e12506fe-b6ae-43dc-9bea-927bd1d82788"));

            migrationBuilder.DeleteData(
                table: "Tags",
                keyColumn: "Id",
                keyValue: new Guid("329cebd4-0a0e-4ec4-96ee-b9c05d195374"));

            migrationBuilder.DeleteData(
                table: "Tags",
                keyColumn: "Id",
                keyValue: new Guid("4b4953d1-8611-4d5c-be7f-5dc61de1fc0a"));

            migrationBuilder.DeleteData(
                table: "Tags",
                keyColumn: "Id",
                keyValue: new Guid("86220849-24b0-4608-b3c3-4fbd5237a46c"));

            migrationBuilder.DeleteData(
                table: "Tags",
                keyColumn: "Id",
                keyValue: new Guid("8cc1b13b-10b9-4fd5-808e-c71b6194e658"));

            migrationBuilder.InsertData(
                table: "Questions",
                columns: new[] { "Id", "Body", "Created", "Modified", "Rating", "Title", "UserId" },
                values: new object[,]
                {
                    { new Guid("226a2e4f-8a5f-4349-9eab-5445df367919"), "Тело1?", new DateTime(2022, 4, 10, 19, 57, 25, 113, DateTimeKind.Utc).AddTicks(142), null, 0, "Контент1", new Guid("40939913-cab2-440b-b88b-42333d986df7") },
                    { new Guid("28e6813d-b10c-4cd0-aa15-c9072d1cb90d"), "Тело5?", new DateTime(2022, 4, 10, 19, 57, 25, 113, DateTimeKind.Utc).AddTicks(491), null, 0, "Контент5", new Guid("26bde74b-55f6-402f-8adc-7f0f3de81c1c") },
                    { new Guid("3e1b5868-eec4-4b2a-8ac7-a5a4c8ba0256"), "Тело3?", new DateTime(2022, 4, 10, 19, 57, 25, 113, DateTimeKind.Utc).AddTicks(487), null, 0, "Контент3", new Guid("fa0f5103-f6ba-4f62-a38d-ae4ab3e9e4db") },
                    { new Guid("7696b1c3-8ba1-445c-b1ae-e0a1da337117"), "Тело4?", new DateTime(2022, 4, 10, 19, 57, 25, 113, DateTimeKind.Utc).AddTicks(489), null, 0, "Контент4", new Guid("1ba72385-867a-43af-8174-93275fd4b048") },
                    { new Guid("caa4900f-8da5-40e3-8e1a-c39cdc17e5e9"), "Тело2?", new DateTime(2022, 4, 10, 19, 57, 25, 113, DateTimeKind.Utc).AddTicks(485), null, 0, "Контент2", new Guid("e949dfd6-fc5e-43fe-b82b-154847778a43") }
                });

            migrationBuilder.InsertData(
                table: "Tags",
                columns: new[] { "Id", "Created", "Description", "Modified", "Name" },
                values: new object[,]
                {
                    { new Guid("28334830-0097-4be9-abcb-b5ccfa50b164"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "кiт", null, "Docker" },
                    { new Guid("7d885607-57f3-45c0-bf2c-93b9b870f08b"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Лучший язык программироваия в мире!(после C#)", null, "Kotlin" },
                    { new Guid("c9fce06c-80be-432b-9580-b1a8ded5762e"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Лучший брокер(после тинькоффа)", null, "Kafka" },
                    { new Guid("fd246836-0278-475f-9fa7-fb2eba19c000"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Лучший язык программирования в мире!", null, "C#" }
                });
        }
    }
}
