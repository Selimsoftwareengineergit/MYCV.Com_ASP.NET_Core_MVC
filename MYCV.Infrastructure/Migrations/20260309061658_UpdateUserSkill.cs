using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MYCV.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateUserSkill : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CertificateFile",
                table: "UserSkills");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CertificateFile",
                table: "UserSkills",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: true);
        }
    }
}
