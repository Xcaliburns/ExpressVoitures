using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DotnetProjet5.Data.Migrations
{
    /// <inheritdoc />
    public partial class simplifyDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Repairs_Vehicle_CodeVin",
                table: "Repairs");

            migrationBuilder.DropTable(
                name: "Purchases");

            migrationBuilder.DropTable(
                name: "Sales");

            migrationBuilder.DropIndex(
                name: "IX_Repairs_CodeVin",
                table: "Repairs");

            migrationBuilder.RenameColumn(
                name: "Image",
                table: "Vehicle",
                newName: "ImageUrl");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Vehicle",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "PurchaseDate",
                table: "Vehicle",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<float>(
                name: "PurchasePrice",
                table: "Vehicle",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<bool>(
                name: "selled",
                table: "Vehicle",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<string>(
                name: "CodeVin",
                table: "Repairs",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Vehicle");

            migrationBuilder.DropColumn(
                name: "PurchaseDate",
                table: "Vehicle");

            migrationBuilder.DropColumn(
                name: "PurchasePrice",
                table: "Vehicle");

            migrationBuilder.DropColumn(
                name: "selled",
                table: "Vehicle");

            migrationBuilder.RenameColumn(
                name: "ImageUrl",
                table: "Vehicle",
                newName: "Image");

            migrationBuilder.AlterColumn<string>(
                name: "CodeVin",
                table: "Repairs",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateTable(
                name: "Purchases",
                columns: table => new
                {
                    PurchaseId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CodeVIN = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PurchaseDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PurchasePrice = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Purchases", x => x.PurchaseId);
                    table.ForeignKey(
                        name: "FK_Purchases_Vehicle_CodeVIN",
                        column: x => x.CodeVIN,
                        principalTable: "Vehicle",
                        principalColumn: "CodeVin",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sales",
                columns: table => new
                {
                    SaleId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    codeVin = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SaleDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sales", x => x.SaleId);
                    table.ForeignKey(
                        name: "FK_Sales_Vehicle_codeVin",
                        column: x => x.codeVin,
                        principalTable: "Vehicle",
                        principalColumn: "CodeVin",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Repairs_CodeVin",
                table: "Repairs",
                column: "CodeVin");

            migrationBuilder.CreateIndex(
                name: "IX_Purchases_CodeVIN",
                table: "Purchases",
                column: "CodeVIN");

            migrationBuilder.CreateIndex(
                name: "IX_Sales_codeVin",
                table: "Sales",
                column: "codeVin");

            migrationBuilder.AddForeignKey(
                name: "FK_Repairs_Vehicle_CodeVin",
                table: "Repairs",
                column: "CodeVin",
                principalTable: "Vehicle",
                principalColumn: "CodeVin",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
