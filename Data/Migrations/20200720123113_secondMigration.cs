using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class secondMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RoleID",
                table: "User",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UserID",
                table: "Role",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_User_RoleID",
                table: "User",
                column: "RoleID");

            migrationBuilder.CreateIndex(
                name: "IX_Role_UserID",
                table: "Role",
                column: "UserID");

            migrationBuilder.AddForeignKey(
                name: "FK_Role_User_UserID",
                table: "Role",
                column: "UserID",
                principalTable: "User",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_User_Role_RoleID",
                table: "User",
                column: "RoleID",
                principalTable: "Role",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Role_User_UserID",
                table: "Role");

            migrationBuilder.DropForeignKey(
                name: "FK_User_Role_RoleID",
                table: "User");

            migrationBuilder.DropIndex(
                name: "IX_User_RoleID",
                table: "User");

            migrationBuilder.DropIndex(
                name: "IX_Role_UserID",
                table: "Role");

            migrationBuilder.DropColumn(
                name: "RoleID",
                table: "User");

            migrationBuilder.DropColumn(
                name: "UserID",
                table: "Role");
        }
    }
}
