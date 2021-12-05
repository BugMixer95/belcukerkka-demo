using Microsoft.EntityFrameworkCore.Migrations;

namespace Belcukerkka.Repositories.Migrations
{
    public partial class bel_sp_GetAllCatalogItems : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            string procedure = @"CREATE PROCEDURE bel_sp_GetAllCatalogItems
                                AS
                                BEGIN
	                                SELECT *
	                                FROM bel_vw_CatalogItems
                                END";

            migrationBuilder.Sql(procedure);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            string procedure = @"DROP PROCEDURE bel_sp_GetAllCatalogItems";

            migrationBuilder.Sql(procedure);
        }
    }
}
