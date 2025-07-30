using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PackageTracker.Core.Migrations
{
    /// <inheritdoc />
    public partial class CorrectMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_packages_carrierServices_ServiceTypeId1",
                table: "packages");

            migrationBuilder.DropIndex(
                name: "IX_packages_ServiceTypeId1",
                table: "packages");

            migrationBuilder.DropColumn(
                name: "ServiceTypeId1",
                table: "packages");

            migrationBuilder.AlterColumn<Guid>(
                name: "ServiceTypeId",
                table: "packages",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_packages_ServiceTypeId",
                table: "packages",
                column: "ServiceTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_packages_carrierServices_ServiceTypeId",
                table: "packages",
                column: "ServiceTypeId",
                principalTable: "carrierServices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_packages_carrierServices_ServiceTypeId",
                table: "packages");

            migrationBuilder.DropIndex(
                name: "IX_packages_ServiceTypeId",
                table: "packages");

            migrationBuilder.AlterColumn<int>(
                name: "ServiceTypeId",
                table: "packages",
                type: "int",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<Guid>(
                name: "ServiceTypeId1",
                table: "packages",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_packages_ServiceTypeId1",
                table: "packages",
                column: "ServiceTypeId1");

            migrationBuilder.AddForeignKey(
                name: "FK_packages_carrierServices_ServiceTypeId1",
                table: "packages",
                column: "ServiceTypeId1",
                principalTable: "carrierServices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
