using Microsoft.EntityFrameworkCore.Migrations;

namespace Belcukerkka.Repositories.Migrations
{
    public partial class Seed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "BoxPackages",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Картон" },
                    { 2, "Текстиль" },
                    { 3, "Дерево" },
                    { 4, "Туба" },
                    { 5, "Мягкая игрушка" }
                });

            migrationBuilder.InsertData(
                table: "WeightType",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Классический" },
                    { 2, "Премиум" },
                    { 3, "Эксклюзивный" },
                    { 4, "Белорусский" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "BoxPackages",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "BoxPackages",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "BoxPackages",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "BoxPackages",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "BoxPackages",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "WeightType",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "WeightType",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "WeightType",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "WeightType",
                keyColumn: "Id",
                keyValue: 4);
        }
    }
}
