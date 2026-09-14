using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace School_Managment_system.Migrations
{
    /// <inheritdoc />
    public partial class Addprofile : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "profile",
                table: "StudentData",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "profile",
                table: "StudentData");
        }
    }
}
