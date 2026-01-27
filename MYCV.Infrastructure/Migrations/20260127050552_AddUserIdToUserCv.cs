using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MYCV.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddUserIdToUserCv : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "UserCvs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_UserCvs_UserId",
                table: "UserCvs",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserCvs_Users_UserId",
                table: "UserCvs",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserCvs_Users_UserId",
                table: "UserCvs");

            migrationBuilder.DropIndex(
                name: "IX_UserCvs_UserId",
                table: "UserCvs");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "UserCvs");
        }
    }
}
