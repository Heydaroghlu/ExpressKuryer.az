using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExpressKuryer.Persistence.Migrations
{
    public partial class VacancyJobSeekerRelationAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_express_vacancies",
                table: "express_vacancies");

            migrationBuilder.DropColumn(
                name: "Message",
                table: "express_vacancies");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "express_vacancies");

            migrationBuilder.DropColumn(
                name: "Surname",
                table: "express_vacancies");

            migrationBuilder.RenameTable(
                name: "express_vacancies",
                newName: "Vacancies");

            migrationBuilder.RenameColumn(
                name: "Cv",
                table: "Vacancies",
                newName: "Title");

            migrationBuilder.AddColumn<int>(
                name: "VacancyId",
                table: "JobSeekers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Vacancies",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Vacancies",
                table: "Vacancies",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_JobSeekers_VacancyId",
                table: "JobSeekers",
                column: "VacancyId");

            migrationBuilder.AddForeignKey(
                name: "FK_JobSeekers_Vacancies_VacancyId",
                table: "JobSeekers",
                column: "VacancyId",
                principalTable: "Vacancies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JobSeekers_Vacancies_VacancyId",
                table: "JobSeekers");

            migrationBuilder.DropIndex(
                name: "IX_JobSeekers_VacancyId",
                table: "JobSeekers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Vacancies",
                table: "Vacancies");

            migrationBuilder.DropColumn(
                name: "VacancyId",
                table: "JobSeekers");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Vacancies");

            migrationBuilder.RenameTable(
                name: "Vacancies",
                newName: "express_vacancies");

            migrationBuilder.RenameColumn(
                name: "Title",
                table: "express_vacancies",
                newName: "Cv");

            migrationBuilder.AddColumn<string>(
                name: "Message",
                table: "express_vacancies",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "express_vacancies",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Surname",
                table: "express_vacancies",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_express_vacancies",
                table: "express_vacancies",
                column: "Id");
        }
    }
}
