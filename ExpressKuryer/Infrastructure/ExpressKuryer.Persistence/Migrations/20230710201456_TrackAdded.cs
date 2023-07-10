using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExpressKuryer.Persistence.Migrations
{
    public partial class TrackAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TrackCode",
                table: "express_deliveries",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TrackCode",
                table: "express_deliveries");
        }
    }
}
