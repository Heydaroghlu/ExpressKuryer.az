using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExpressKuryer.Persistence.Migrations
{
    public partial class PartnerAddedToPartnerProduct : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PartnerId",
                table: "express_PartnersProducts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_express_PartnersProducts_PartnerId",
                table: "express_PartnersProducts",
                column: "PartnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_express_PartnersProducts_express_partners_PartnerId",
                table: "express_PartnersProducts",
                column: "PartnerId",
                principalTable: "express_partners",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_express_PartnersProducts_express_partners_PartnerId",
                table: "express_PartnersProducts");

            migrationBuilder.DropIndex(
                name: "IX_express_PartnersProducts_PartnerId",
                table: "express_PartnersProducts");

            migrationBuilder.DropColumn(
                name: "PartnerId",
                table: "express_PartnersProducts");
        }
    }
}
