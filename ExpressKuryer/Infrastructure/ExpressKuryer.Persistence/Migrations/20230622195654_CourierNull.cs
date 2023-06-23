using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExpressKuryer.Persistence.Migrations
{
    public partial class CourierNull : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_express_deliveries_Couriers_CourierId",
                table: "express_deliveries");

            migrationBuilder.AlterColumn<int>(
                name: "CourierId",
                table: "express_deliveries",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_express_deliveries_Couriers_CourierId",
                table: "express_deliveries",
                column: "CourierId",
                principalTable: "Couriers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_express_deliveries_Couriers_CourierId",
                table: "express_deliveries");

            migrationBuilder.AlterColumn<int>(
                name: "CourierId",
                table: "express_deliveries",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_express_deliveries_Couriers_CourierId",
                table: "express_deliveries",
                column: "CourierId",
                principalTable: "Couriers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
