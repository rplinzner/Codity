using Microsoft.EntityFrameworkCore.Migrations;

namespace Twitter.Data.Migrations
{
    public partial class CreateFullTextIndex : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("CREATE FULLTEXT CATALOG fullTextCatalog AS DEFAULT", true);
            migrationBuilder.Sql("CREATE FULLTEXT INDEX ON dbo.Users(FirstName, LastName, AboutMe) KEY INDEX PK_Users WITH STOPLIST = SYSTEM", true);
            migrationBuilder.Sql(
                @"CREATE FUNCTION [dbo].[SearchUsers]
                      (@SearchParameter nvarchar(4000),
                      @PageNumber int,
                      @PageSize int)
                  RETURNS TABLE  
                  AS
                  RETURN
                    (
                      SELECT * FROM [Users] AS [u]
                      INNER JOIN FREETEXTTABLE([Users], ([FirstName], [LastName], [AboutMe]), @SearchParameter) [ft]
                            ON [u].Id = [ft].[Key]
                      ORDER BY [ft].[RANK] DESC
                      OFFSET(@PageNumber - 1) * @PageSize ROWS FETCH NEXT @PageSize ROWS only
                    )");    
            migrationBuilder.Sql(
                @"CREATE FUNCTION [dbo].[SearchUsersCount]
                      (@SearchParameter nvarchar(4000))
                  RETURNS TABLE  
                  AS
                  RETURN
                    (
                      SELECT * FROM [Users] AS [u]
                      WHERE FREETEXT(([FirstName], [LastName], [AboutMe]), @SearchParameter)
                    )");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
