using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExpressKuryer.Persistence.Migrations
{
    public partial class DeliveryCourier : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CourierId",
                table: "express_deliveries",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_express_deliveries_CourierId",
                table: "express_deliveries",
                column: "CourierId");

            migrationBuilder.AddForeignKey(
                name: "FK_express_deliveries_Couriers_CourierId",
                table: "express_deliveries",
                column: "CourierId",
                principalTable: "Couriers",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_express_deliveries_Couriers_CourierId",
                table: "express_deliveries");

            migrationBuilder.DropIndex(
                name: "IX_express_deliveries_CourierId",
                table: "express_deliveries");

            migrationBuilder.DropColumn(
                name: "CourierId",
                table: "express_deliveries");
        }
    }
}
