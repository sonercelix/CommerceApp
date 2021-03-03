using Microsoft.EntityFrameworkCore.Migrations;

namespace CommerceWebApi.Migrations
{
    public partial class sp_ProductList : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            string procedure = @"CREATE PROCEDURE sp_ProductList
                                AS
                                BEGIN
                                    SELECT p.Id,p.Name,p.CategoryId,c.Name as CategoryName,ImageUrl,Price,Description
	                                FROM [dbo].[Products] p 
	                                JOIN Categories c on p.CategoryId = c.Id
                                END";
            migrationBuilder.Sql(procedure);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            string procedure = @"DROP PROCEDURE sp_ProductList";
            migrationBuilder.Sql(procedure);
        }
    }
}
