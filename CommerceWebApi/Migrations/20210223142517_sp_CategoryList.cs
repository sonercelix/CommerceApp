using Microsoft.EntityFrameworkCore.Migrations;

namespace CommerceWebApi.Migrations
{
    public partial class sp_CategoryList : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            string procedure = @"CREATE PROCEDURE sp_CategoryList
                                AS
                                BEGIN
                                SELECT * FROM 
	                            (
		                            SELECT t1.Id as Id, t2.Id as ParentId, t1.Name, t2.Name as SubCategoryName 
		                            FROM [dbo].[Categories] t1 JOIN Categories t2 on t1.Id=t2.ParentId 
		                            UNION ALL
		                            SELECT Id,ParentId,Name,'' as SubCategoryName  FROM Categories WHERE ParentId is null
	                            ) as catTable 
	                            ORDER BY catTable.Id
                                END";
            migrationBuilder.Sql(procedure);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            string procedure = @"DROP PROCEDURE sp_CategoryList";
            migrationBuilder.Sql(procedure);
        }
    }
}
