using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MYCV.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateUserPersonalDetail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                table: "UserPersonalDetails");

            migrationBuilder.RenameColumn(
                name: "Address",
                table: "UserPersonalDetails",
                newName: "PresentAddress");

            migrationBuilder.AddColumn<string>(
                name: "Nationality",
                table: "UserPersonalDetails",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PermanentAddress",
                table: "UserPersonalDetails",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Religion",
                table: "UserPersonalDetails",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Nationality",
                table: "UserPersonalDetails");

            migrationBuilder.DropColumn(
                name: "PermanentAddress",
                table: "UserPersonalDetails");

            migrationBuilder.DropColumn(
                name: "Religion",
                table: "UserPersonalDetails");

            migrationBuilder.RenameColumn(
                name: "PresentAddress",
                table: "UserPersonalDetails",
                newName: "Address");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "UserPersonalDetails",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: false,
                defaultValue: "");
        }
    }
}
