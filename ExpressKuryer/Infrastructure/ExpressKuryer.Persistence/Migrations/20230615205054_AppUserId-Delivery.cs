using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExpressKuryer.Persistence.Migrations
{
    public partial class AppUserIdDelivery : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Id",
                table: "AspNetUsers",
                newName: "AppUserId");

            migrationBuilder.AlterColumn<string>(
                name: "OrderDeliveryStatus",
                table: "express_deliveries",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "DeliveryStatus",
                table: "express_deliveries",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "AppUserId",
                table: "express_deliveries",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_express_deliveries_AppUserId",
                table: "express_deliveries",
                column: "AppUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_express_deliveries_AspNetUsers_AppUserId",
                table: "express_deliveries",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "AppUserId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_express_deliveries_AspNetUsers_AppUserId",
                table: "express_deliveries");

            migrationBuilder.DropIndex(
                name: "IX_express_deliveries_AppUserId",
                table: "express_deliveries");

            migrationBuilder.DropColumn(
                name: "AppUserId",
                table: "express_deliveries");

            migrationBuilder.RenameColumn(
                name: "AppUserId",
                table: "AspNetUsers",
                newName: "Id");

            migrationBuilder.AlterColumn<int>(
                name: "OrderDeliveryStatus",
                table: "express_deliveries",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<int>(
                name: "DeliveryStatus",
                table: "express_deliveries",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }
    }
}
