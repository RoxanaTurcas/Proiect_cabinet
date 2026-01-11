using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VetCare.Migrations
{
    /// <inheritdoc />
    public partial class RenameTableVaccinuri : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Vaccinari_Animale_AnimalPetId",
                table: "Vaccinari");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Vaccinari",
                table: "Vaccinari");

            migrationBuilder.DropIndex(
                name: "IX_Vaccinari_AnimalPetId",
                table: "Vaccinari");

            migrationBuilder.DropColumn(
                name: "AnimalPetId",
                table: "Vaccinari");

            migrationBuilder.RenameTable(
                name: "Vaccinari",
                newName: "Vaccinuri");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Vaccinuri",
                table: "Vaccinuri",
                column: "VaccinId");

            migrationBuilder.CreateIndex(
                name: "IX_Vaccinuri_PetId",
                table: "Vaccinuri",
                column: "PetId");

            migrationBuilder.AddForeignKey(
                name: "FK_Vaccinuri_Animale_PetId",
                table: "Vaccinuri",
                column: "PetId",
                principalTable: "Animale",
                principalColumn: "PetId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Vaccinuri_Animale_PetId",
                table: "Vaccinuri");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Vaccinuri",
                table: "Vaccinuri");

            migrationBuilder.DropIndex(
                name: "IX_Vaccinuri_PetId",
                table: "Vaccinuri");

            migrationBuilder.RenameTable(
                name: "Vaccinuri",
                newName: "Vaccinari");

            migrationBuilder.AddColumn<int>(
                name: "AnimalPetId",
                table: "Vaccinari",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Vaccinari",
                table: "Vaccinari",
                column: "VaccinId");

            migrationBuilder.CreateIndex(
                name: "IX_Vaccinari_AnimalPetId",
                table: "Vaccinari",
                column: "AnimalPetId");

            migrationBuilder.AddForeignKey(
                name: "FK_Vaccinari_Animale_AnimalPetId",
                table: "Vaccinari",
                column: "AnimalPetId",
                principalTable: "Animale",
                principalColumn: "PetId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
