using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace School_Managment_system.Migrations
{
    /// <inheritdoc />
    public partial class ChangeOverallRateToDouble : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "OverallRate",
                table: "StudentData",
                type: "float",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "OverallRate",
                table: "StudentData",
                type: "int",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");
        }
    }
}
