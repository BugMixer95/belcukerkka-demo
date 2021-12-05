using Microsoft.EntityFrameworkCore.Migrations;

namespace Belcukerkka.Repositories.Migrations
{
    public partial class AlterCatalogItemView : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            string view = @"ALTER VIEW bel_vw_CatalogItems
							AS
								SELECT
									bp.Id,
									bp.Name,
									bp.ImagePath,
									c1.Weight,
									(
										SELECT 
											b2.Id AS ChildId,
											wt.Name AS WeightTypeName,
											CAST(b2.Price AS NUMERIC(4,2)) AS Price,
											CAST(b2.FullPrice AS NUMERIC(4,2)) AS FullPrice
											FROM dbo.Boxes AS b2
											LEFT JOIN dbo.Compositions AS c2 
												ON b2.CompositionId = c2.Id
											LEFT JOIN dbo.WeightType AS wt 
												ON c2.WeightTypeId = wt.Id
											WHERE b2.BoxParentId = bp.Id
											AND c2.Weight = c1.Weight
											FOR JSON PATH
									) AS ChildBoxes
								FROM dbo.BoxParents AS bp
								LEFT JOIN dbo.Boxes AS b1 
									ON bp.Id = b1.BoxParentId
								LEFT JOIN dbo.Compositions AS c1 
									ON b1.CompositionId = c1.Id
								GROUP BY c1.Weight, bp.Id, bp.Name, bp.ImagePath";

			migrationBuilder.Sql(view);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
			string view = @"ALTER VIEW bel_vw_CatalogItems
							AS
								SELECT
									bp.Id,
									bp.Name,
									bp.ImagePath,
									c1.Weight,
									(
										SELECT 
											b2.Id AS ChildId,
											wt.Name AS WeightTypeName
										FROM dbo.Boxes AS b2
										LEFT JOIN dbo.Compositions AS c2 
											ON b2.CompositionId = c2.Id
										LEFT JOIN dbo.WeightType AS wt 
											ON c2.WeightTypeId = wt.Id
										WHERE b2.BoxParentId = bp.Id
										AND c2.Weight = c1.Weight
										FOR JSON PATH
									) AS ChildBoxes
								FROM dbo.BoxParents AS bp
								LEFT JOIN dbo.Boxes AS b1 
									ON bp.Id = b1.BoxParentId
								LEFT JOIN dbo.Compositions AS c1 
									ON b1.CompositionId = c1.Id
								GROUP BY c1.Weight, bp.Id, bp.Name, bp.ImagePath";

			migrationBuilder.Sql(view);
		}
    }
}
