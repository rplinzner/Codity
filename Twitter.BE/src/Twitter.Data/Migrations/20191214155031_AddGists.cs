using Microsoft.EntityFrameworkCore.Migrations;

namespace Twitter.Data.Migrations
{
    public partial class AddGists : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "GithubToken",
                table: "Settings",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GistURL",
                table: "CodeSnippets",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GithubToken",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "GistURL",
                table: "CodeSnippets");
        }
    }
}
