using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PackageTracker.Core.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "carriers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_carriers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "carrierServices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_carrierServices", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PackageStatuses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PackageStatuses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PackageStatusHistories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Timestamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PackageStatusHistories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
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

            migrationBuilder.CreateTable(
                name: "packages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TrackingNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CarrierId = table.Column<int>(type: "int", nullable: true),
                    PackageStatusHistoryId = table.Column<int>(type: "int", nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_packages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_packages_PackageStatusHistories_PackageStatusHistoryId",
                        column: x => x.PackageStatusHistoryId,
                        principalTable: "PackageStatusHistories",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_packages_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_packages_carriers_CarrierId",
                        column: x => x.CarrierId,
                        principalTable: "carriers",
                        principalColumn: "Id");
                });

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

            migrationBuilder.CreateIndex(
                name: "IX_PackagePackageStatus_packageStatusId",
                table: "PackagePackageStatus",
                column: "packageStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_packages_CarrierId",
                table: "packages",
                column: "CarrierId");

            migrationBuilder.CreateIndex(
                name: "IX_packages_PackageStatusHistoryId",
                table: "packages",
                column: "PackageStatusHistoryId");

            migrationBuilder.CreateIndex(
                name: "IX_packages_UserId",
                table: "packages",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_PackageStatusPackageStatusHistory_packageStatusesId",
                table: "PackageStatusPackageStatusHistory",
                column: "packageStatusesId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "carrierServices");

            migrationBuilder.DropTable(
                name: "PackagePackageStatus");

            migrationBuilder.DropTable(
                name: "PackageStatusPackageStatusHistory");

            migrationBuilder.DropTable(
                name: "packages");

            migrationBuilder.DropTable(
                name: "PackageStatuses");

            migrationBuilder.DropTable(
                name: "PackageStatusHistories");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "carriers");
        }
    }
}
