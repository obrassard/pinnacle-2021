using Microsoft.EntityFrameworkCore.Migrations;

namespace Pinnacle_2021.Api.Migrations
{
    public partial class TTL : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TTL",
                table: "Item",
                type: "int",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TTL",
                table: "Item");
        }
    }
}
