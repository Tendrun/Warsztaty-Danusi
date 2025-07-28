using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PackageTracker.Core.Migrations
{
    /// <inheritdoc />
    public partial class ChangeIdsToGuid : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_packages_PackageStatusHistories_PackageStatusHistoryId",
                table: "packages");

            migrationBuilder.DropForeignKey(
                name: "FK_packages_Users_UserId",
                table: "packages");

            migrationBuilder.DropForeignKey(
                name: "FK_packages_carriers_CarrierId",
                table: "packages");

            migrationBuilder.DropTable(
                name: "PackagePackageStatus");

            migrationBuilder.DropTable(
                name: "PackageStatusPackageStatusHistory");

            migrationBuilder.DropIndex(
                name: "IX_packages_PackageStatusHistoryId",
                table: "packages");

            migrationBuilder.DropColumn(
                name: "PackageStatusHistoryId",
                table: "packages");

            migrationBuilder.RenameColumn(
                name: "isActive",
                table: "carriers",
                newName: "IsActive");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "Users",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<string>(
                name: "Notes",
                table: "PackageStatusHistories",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "PackageStatusHistories",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<Guid>(
                name: "PackageId",
                table: "PackageStatusHistories",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "StatusId",
                table: "PackageStatusHistories",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "PackageStatuses",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "packages",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CarrierId",
                table: "packages",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "packages",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<int>(
                name: "ServiceTypeId",
                table: "packages",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "ServiceTypeId1",
                table: "packages",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "StatusId",
                table: "packages",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "carrierServices",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "carriers",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.CreateIndex(
                name: "IX_PackageStatusHistories_PackageId",
                table: "PackageStatusHistories",
                column: "PackageId");

            migrationBuilder.CreateIndex(
                name: "IX_PackageStatusHistories_StatusId",
                table: "PackageStatusHistories",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_packages_ServiceTypeId1",
                table: "packages",
                column: "ServiceTypeId1");

            migrationBuilder.CreateIndex(
                name: "IX_packages_StatusId",
                table: "packages",
                column: "StatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_packages_PackageStatuses_StatusId",
                table: "packages",
                column: "StatusId",
                principalTable: "PackageStatuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_packages_Users_UserId",
                table: "packages",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_packages_carrierServices_ServiceTypeId1",
                table: "packages",
                column: "ServiceTypeId1",
                principalTable: "carrierServices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_packages_carriers_CarrierId",
                table: "packages",
                column: "CarrierId",
                principalTable: "carriers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PackageStatusHistories_PackageStatuses_StatusId",
                table: "PackageStatusHistories",
                column: "StatusId",
                principalTable: "PackageStatuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PackageStatusHistories_packages_PackageId",
                table: "PackageStatusHistories",
                column: "PackageId",
                principalTable: "packages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_packages_PackageStatuses_StatusId",
                table: "packages");

            migrationBuilder.DropForeignKey(
                name: "FK_packages_Users_UserId",
                table: "packages");

            migrationBuilder.DropForeignKey(
                name: "FK_packages_carrierServices_ServiceTypeId1",
                table: "packages");

            migrationBuilder.DropForeignKey(
                name: "FK_packages_carriers_CarrierId",
                table: "packages");

            migrationBuilder.DropForeignKey(
                name: "FK_PackageStatusHistories_PackageStatuses_StatusId",
                table: "PackageStatusHistories");

            migrationBuilder.DropForeignKey(
                name: "FK_PackageStatusHistories_packages_PackageId",
                table: "PackageStatusHistories");

            migrationBuilder.DropIndex(
                name: "IX_PackageStatusHistories_PackageId",
                table: "PackageStatusHistories");

            migrationBuilder.DropIndex(
                name: "IX_PackageStatusHistories_StatusId",
                table: "PackageStatusHistories");

            migrationBuilder.DropIndex(
                name: "IX_packages_ServiceTypeId1",
                table: "packages");

            migrationBuilder.DropIndex(
                name: "IX_packages_StatusId",
                table: "packages");

            migrationBuilder.DropColumn(
                name: "PackageId",
                table: "PackageStatusHistories");

            migrationBuilder.DropColumn(
                name: "StatusId",
                table: "PackageStatusHistories");

            migrationBuilder.DropColumn(
                name: "ServiceTypeId",
                table: "packages");

            migrationBuilder.DropColumn(
                name: "ServiceTypeId1",
                table: "packages");

            migrationBuilder.DropColumn(
                name: "StatusId",
                table: "packages");

            migrationBuilder.RenameColumn(
                name: "IsActive",
                table: "carriers",
                newName: "isActive");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Users",
                type: "int",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier")
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<string>(
                name: "Notes",
                table: "PackageStatusHistories",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "PackageStatusHistories",
                type: "int",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier")
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "PackageStatuses",
                type: "int",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier")
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "packages",
                type: "int",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<int>(
                name: "CarrierId",
                table: "packages",
                type: "int",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "packages",
                type: "int",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier")
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<int>(
                name: "PackageStatusHistoryId",
                table: "packages",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "carrierServices",
                type: "int",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier")
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "carriers",
                type: "int",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier")
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.CreateTable(
                name: "PackagePackageStatus",
                columns: table => new
                {
                    PackagesId = table.Column<int>(type: "int", nullable: false),
                    packageStatusId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PackagePackageStatus", x => new { x.PackagesId, x.packageStatusId });
                    table.ForeignKey(
                        name: "FK_PackagePackageStatus_PackageStatuses_packageStatusId",
                        column: x => x.packageStatusId,
                        principalTable: "PackageStatuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PackagePackageStatus_packages_PackagesId",
                        column: x => x.PackagesId,
                        principalTable: "packages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PackageStatusPackageStatusHistory",
                columns: table => new
                {
                    PackageStatusHistoriesId = table.Column<int>(type: "int", nullable: false),
                    packageStatusesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PackageStatusPackageStatusHistory", x => new { x.PackageStatusHistoriesId, x.packageStatusesId });
                    table.ForeignKey(
                        name: "FK_PackageStatusPackageStatusHistory_PackageStatusHistories_PackageStatusHistoriesId",
                        column: x => x.PackageStatusHistoriesId,
                        principalTable: "PackageStatusHistories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PackageStatusPackageStatusHistory_PackageStatuses_packageStatusesId",
                        column: x => x.packageStatusesId,
                        principalTable: "PackageStatuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_packages_PackageStatusHistoryId",
                table: "packages",
                column: "PackageStatusHistoryId");

            migrationBuilder.CreateIndex(
                name: "IX_PackagePackageStatus_packageStatusId",
                table: "PackagePackageStatus",
                column: "packageStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_PackageStatusPackageStatusHistory_packageStatusesId",
                table: "PackageStatusPackageStatusHistory",
                column: "packageStatusesId");

            migrationBuilder.AddForeignKey(
                name: "FK_packages_PackageStatusHistories_PackageStatusHistoryId",
                table: "packages",
                column: "PackageStatusHistoryId",
                principalTable: "PackageStatusHistories",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_packages_Users_UserId",
                table: "packages",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_packages_carriers_CarrierId",
                table: "packages",
                column: "CarrierId",
                principalTable: "carriers",
                principalColumn: "Id");
        }
    }
}
