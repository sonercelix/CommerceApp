using Microsoft.EntityFrameworkCore.Migrations;

namespace CommerceWebApi.Migrations
{
    public partial class spresult : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            string procedure = @"IF OBJECT_ID('sp_ProductList', 'P') IS NOT NULL
                                DROP PROC sp_ProductList
                                GO
                                CREATE PROCEDURE sp_ProductList
                                AS
                                BEGIN
                                    SELECT p.Id,p.Name,p.CategoryId,c.Name as CategoryName,ImageUrl,Price,Description,IsActive
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
