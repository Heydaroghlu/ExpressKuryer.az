using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExpressKuryer.Persistence.Migrations
{
    public partial class SomeGainsAddedToDelivery : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CompanyGain",
                table: "express_deliveries",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CourierGain",
                table: "express_deliveries",
                type: "int",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CompanyGain",
                table: "express_deliveries");

            migrationBuilder.DropColumn(
                name: "CourierGain",
                table: "express_deliveries");
        }
    }
}
