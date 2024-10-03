using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DotnetProjet5.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddVehicleNavigationToRepair : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CodeVin",
                table: "Repairs");

            migrationBuilder.AddColumn<int>(
                name: "VehicleId",
                table: "Repairs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Repairs_VehicleId",
                table: "Repairs",
                column: "VehicleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Repairs_Vehicle_VehicleId",
                table: "Repairs",
                column: "VehicleId",
                principalTable: "Vehicle",
                principalColumn: "VehicleId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Repairs_Vehicle_VehicleId",
                table: "Repairs");

            migrationBuilder.DropIndex(
                name: "IX_Repairs_VehicleId",
                table: "Repairs");

            migrationBuilder.DropColumn(
                name: "VehicleId",
                table: "Repairs");

            migrationBuilder.AddColumn<string>(
                name: "CodeVin",
                table: "Repairs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
