using Microsoft.EntityFrameworkCore.Migrations;

namespace LoMan.Data.Migrations
{
    public partial class AddDashboardToDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Dashboard",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TotalLoans = table.Column<int>(nullable: false),
                    Tprinciple = table.Column<int>(nullable: false),
                    Tinterest = table.Column<int>(nullable: false),
                    MonthlyLoans = table.Column<int>(nullable: false),
                    Mprinciple = table.Column<int>(nullable: false),
                    Minterest = table.Column<int>(nullable: false),
                    Pincrement = table.Column<int>(nullable: false),
                    Iincrement = table.Column<int>(nullable: false),
                    MonthlyGrowth = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dashboard", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Dashboard");
        }
    }
}
