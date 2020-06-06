using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace LoMan.Data.Migrations
{
    public partial class TablesToDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Analytics",
                columns: table => new
                {
                    Month = table.Column<int>(nullable: false),
                    Year = table.Column<int>(nullable: false),
                    Principle = table.Column<float>(nullable: false),
                    Interest = table.Column<float>(nullable: false),
                    Ppercent = table.Column<float>(nullable: false),
                    Ipercent = table.Column<float>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Analytics", x => new { x.Month, x.Year });
                });

            migrationBuilder.CreateTable(
                name: "Loans",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Address = table.Column<string>(nullable: true),
                    Phone = table.Column<string>(maxLength: 10, nullable: true),
                    Asset = table.Column<string>(nullable: true),
                    Principle = table.Column<float>(nullable: false),
                    Interest = table.Column<float>(nullable: false),
                    Rate = table.Column<float>(nullable: false),
                    Amount = table.Column<float>(nullable: false),
                    Idate = table.Column<DateTime>(nullable: false),
                    Rdate = table.Column<DateTime>(nullable: false),
                    Penalty = table.Column<float>(nullable: false),
                    Times = table.Column<int>(nullable: false),
                    Period = table.Column<int>(nullable: false),
                    Status = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Loans", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Recoveries",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Date = table.Column<DateTime>(nullable: false),
                    Principle = table.Column<float>(nullable: false),
                    Interest = table.Column<float>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Recoveries", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Analytics");

            migrationBuilder.DropTable(
                name: "Loans");

            migrationBuilder.DropTable(
                name: "Recoveries");
        }
    }
}
