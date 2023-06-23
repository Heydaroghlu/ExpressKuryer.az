using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExpressKuryer.Persistence.Migrations
{
    public partial class Courier : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_express_deliveries_AspNetUsers_AppUserId",
                table: "express_deliveries");

            migrationBuilder.RenameColumn(
                name: "AppUserId",
                table: "express_deliveries",
                newName: "MemberUserId");

            migrationBuilder.RenameIndex(
                name: "IX_express_deliveries_AppUserId",
                table: "express_deliveries",
                newName: "IX_express_deliveries_MemberUserId");

            migrationBuilder.AddColumn<string>(
                name: "UserType",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Couriers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Gain = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CourierPersonId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    WhoIsModified = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Couriers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Couriers_AspNetUsers_CourierPersonId",
                        column: x => x.CourierPersonId,
                        principalTable: "AspNetUsers",
                        principalColumn: "AppUserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Couriers_CourierPersonId",
                table: "Couriers",
                column: "CourierPersonId");

            migrationBuilder.AddForeignKey(
                name: "FK_express_deliveries_AspNetUsers_MemberUserId",
                table: "express_deliveries",
                column: "MemberUserId",
                principalTable: "AspNetUsers",
                principalColumn: "AppUserId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_express_deliveries_AspNetUsers_MemberUserId",
                table: "express_deliveries");

            migrationBuilder.DropTable(
                name: "Couriers");

            migrationBuilder.DropColumn(
                name: "UserType",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "MemberUserId",
                table: "express_deliveries",
                newName: "AppUserId");

            migrationBuilder.RenameIndex(
                name: "IX_express_deliveries_MemberUserId",
                table: "express_deliveries",
                newName: "IX_express_deliveries_AppUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_express_deliveries_AspNetUsers_AppUserId",
                table: "express_deliveries",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "AppUserId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
