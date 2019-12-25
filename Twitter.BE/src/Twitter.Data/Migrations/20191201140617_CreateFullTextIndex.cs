using Microsoft.EntityFrameworkCore.Migrations;

namespace Twitter.Data.Migrations
{
    public partial class CreateFullTextIndex : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("CREATE FULLTEXT CATALOG fullTextCatalog AS DEFAULT", true);
            migrationBuilder.Sql("CREATE FULLTEXT INDEX ON dbo.Users(FirstName, LastName) KEY INDEX PK_Users WITH STOPLIST = SYSTEM", true);
            migrationBuilder.Sql(
                @"CREATE FUNCTION [dbo].[SearchUsers]
                      (@SearchParameter nvarchar(4000),
                      @PageNumber int,
                      @PageSize int,
                      @ExceptId int)
                  RETURNS TABLE  
                  AS
                  RETURN
                    (
                      SELECT * FROM [Users] AS [u]
                      INNER JOIN (
					  Select [Key], SUM([Rank]) as [RANK] from 
					  (
						SELECT * FROM
    					    CONTAINSTABLE([Users], [FirstName], @SearchParameter) 
                        UNION ALL
                        SELECT * FROM
                            CONTAINSTABLE([Users], [LastName], @SearchParameter)
					  ) as fts
                        GROUP BY fts.[Key]
					  )[ft]
                            ON [u].Id = [ft].[Key]
                      WHERE NOT [u].Id = @ExceptId
                      ORDER BY [ft].[RANK] DESC
                      OFFSET(@PageNumber - 1) * @PageSize ROWS FETCH NEXT @PageSize ROWS only
                    )");    
            migrationBuilder.Sql(
                @"CREATE FUNCTION [dbo].[SearchUsersCount]
                      (@SearchParameter nvarchar(4000),
                      @ExceptId int)
                  RETURNS TABLE  
                  AS
                  RETURN
                    (
                      SELECT * FROM [Users] AS [u]
                       INNER JOIN (
					   Select [Key], SUM([Rank]) as [RANK] from 
					   (
						 SELECT * FROM
    					     CONTAINSTABLE([Users], [FirstName], @SearchParameter) 
                         UNION ALL
                         SELECT * FROM
                             CONTAINSTABLE([Users], [LastName], @SearchParameter)
					   ) as fts
                         GROUP BY fts.[Key]
					   )[ft]
                             ON [u].Id = [ft].[Key]
                       WHERE NOT [u].Id = @ExceptId
                    )");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
