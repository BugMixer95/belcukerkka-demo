using Microsoft.EntityFrameworkCore.Migrations;

namespace Belcukerkka.Repositories.Migrations
{
    public partial class bel_sp_GetExactCatalogItem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            string procedure = @"CREATE PROCEDURE bel_sp_GetExactCatalogItem
                                @BoxParentId INT, @Weight INT
                                AS
                                BEGIN
	                                SELECT *
	                                FROM bel_vw_CatalogItems AS ci
	                                WHERE ci.Id = @BoxParentId
	                                AND ci.Weight = @Weight
                                END";

            migrationBuilder.Sql(procedure);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            string procedure = @"DROP PROCEDURE bel_sp_GetExactCatalogItem";

            migrationBuilder.Sql(procedure);
        }
    }
}
