using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExpressKuryer.Persistence.Migrations
{
    public partial class removePartnerProductRelation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_express_deliveries_express_PartnersProducts_PartnerProductId",
                table: "express_deliveries");

            migrationBuilder.DropIndex(
                name: "IX_express_deliveries_PartnerProductId",
                table: "express_deliveries");

            migrationBuilder.DropColumn(
                name: "PartnerProductId",
                table: "express_deliveries");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PartnerProductId",
                table: "express_deliveries",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_express_deliveries_PartnerProductId",
                table: "express_deliveries",
                column: "PartnerProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_express_deliveries_express_PartnersProducts_PartnerProductId",
                table: "express_deliveries",
                column: "PartnerProductId",
                principalTable: "express_PartnersProducts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
