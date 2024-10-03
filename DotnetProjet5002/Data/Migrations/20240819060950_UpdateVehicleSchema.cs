using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DotnetProjet5.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateVehicleSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "selled",
                table: "Vehicle",
                newName: "Selled");

            migrationBuilder.AddColumn<float>(
                name: "SellPrice",
                table: "Vehicle",
                type: "real",
                nullable: false,
                defaultValue: 0f);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SellPrice",
                table: "Vehicle");

            migrationBuilder.RenameColumn(
                name: "Selled",
                table: "Vehicle",
                newName: "selled");
        }
    }
}
