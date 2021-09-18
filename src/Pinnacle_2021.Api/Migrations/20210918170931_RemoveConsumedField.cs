using Microsoft.EntityFrameworkCore.Migrations;

namespace Pinnacle_2021.Api.Migrations
{
	public partial class RemoveConsumedField : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropColumn(
				name: "Consumed",
				table: "InventoryItem");
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.AddColumn<bool>(
				name: "Consumed",
				table: "InventoryItem",
				type: "bit",
				nullable: false,
				defaultValue: false);
		}
	}
}
