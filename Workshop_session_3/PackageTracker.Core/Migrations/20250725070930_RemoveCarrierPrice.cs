using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PackageTracker.Core.Migrations
{
    /// <inheritdoc />
    public partial class RemoveCarrierPrice : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Price",
                table: "carriers");

            migrationBuilder.AddColumn<decimal>(
                name: "Height",
                table: "packages",
                type: "decimal(10,2)",
                precision: 10,
                scale: 2,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Length",
                table: "packages",
                type: "decimal(10,2)",
                precision: 10,
                scale: 2,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Weight",
                table: "packages",
                type: "decimal(10,2)",
                precision: 10,
                scale: 2,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Width",
                table: "packages",
                type: "decimal(10,2)",
                precision: 10,
                scale: 2,
                nullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "carrierServices",
                type: "decimal(10,2)",
                precision: 10,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Height",
                table: "packages");

            migrationBuilder.DropColumn(
                name: "Length",
                table: "packages");

            migrationBuilder.DropColumn(
                name: "Weight",
                table: "packages");

            migrationBuilder.DropColumn(
                name: "Width",
                table: "packages");

            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "carrierServices",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(10,2)",
                oldPrecision: 10,
                oldScale: 2);

            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "carriers",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
