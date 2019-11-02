using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Twitter.Data.Migrations
{
    public partial class AddNotifications : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Gender_GenderId",
                table: "Users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Gender",
                table: "Gender");

            migrationBuilder.DropColumn(
                name: "ProgrammingLanguage",
                table: "CodeSnippets");

            migrationBuilder.RenameTable(
                name: "Gender",
                newName: "Genders");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Genders",
                table: "Genders",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Notifications",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RedirectTo = table.Column<string>(nullable: true),
                    Text = table.Column<string>(nullable: true),
                    IsRead = table.Column<bool>(nullable: false),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    UserId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notifications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Notifications_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserNotifications",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(nullable: false),
                    NotificationId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserNotifications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserNotifications_Notifications_NotificationId",
                        column: x => x.NotificationId,
                        principalTable: "Notifications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserNotifications_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CodeSnippets_ProgrammingLanguageId",
                table: "CodeSnippets",
                column: "ProgrammingLanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_UserId",
                table: "Notifications",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserNotifications_NotificationId",
                table: "UserNotifications",
                column: "NotificationId");

            migrationBuilder.CreateIndex(
                name: "IX_UserNotifications_UserId",
                table: "UserNotifications",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_CodeSnippets_ProgrammingLanguages_ProgrammingLanguageId",
                table: "CodeSnippets",
                column: "ProgrammingLanguageId",
                principalTable: "ProgrammingLanguages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Genders_GenderId",
                table: "Users",
                column: "GenderId",
                principalTable: "Genders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CodeSnippets_ProgrammingLanguages_ProgrammingLanguageId",
                table: "CodeSnippets");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Genders_GenderId",
                table: "Users");

            migrationBuilder.DropTable(
                name: "UserNotifications");

            migrationBuilder.DropTable(
                name: "Notifications");

            migrationBuilder.DropIndex(
                name: "IX_CodeSnippets_ProgrammingLanguageId",
                table: "CodeSnippets");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Genders",
                table: "Genders");

            migrationBuilder.RenameTable(
                name: "Genders",
                newName: "Gender");

            migrationBuilder.AddColumn<string>(
                name: "ProgrammingLanguage",
                table: "CodeSnippets",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Gender",
                table: "Gender",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Gender_GenderId",
                table: "Users",
                column: "GenderId",
                principalTable: "Gender",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
