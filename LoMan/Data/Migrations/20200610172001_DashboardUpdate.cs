using Microsoft.EntityFrameworkCore.Migrations;

namespace LoMan.Data.Migrations
{
    public partial class DashboardUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Iincrement",
                table: "Dashboard");

            migrationBuilder.DropColumn(
                name: "MonthlyGrowth",
                table: "Dashboard");

            migrationBuilder.DropColumn(
                name: "Pincrement",
                table: "Dashboard");

            migrationBuilder.AddColumn<int>(
                name: "PreviousLoans",
                table: "Dashboard",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Printerest",
                table: "Dashboard",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Prprinciple",
                table: "Dashboard",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PreviousLoans",
                table: "Dashboard");

            migrationBuilder.DropColumn(
                name: "Printerest",
                table: "Dashboard");

            migrationBuilder.DropColumn(
                name: "Prprinciple",
                table: "Dashboard");

            migrationBuilder.AddColumn<int>(
                name: "Iincrement",
                table: "Dashboard",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MonthlyGrowth",
                table: "Dashboard",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Pincrement",
                table: "Dashboard",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
