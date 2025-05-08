using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Quarter.Repostory.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EstateLocations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    City = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Area = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateUpdated = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EstateLocations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EstateTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateUpdated = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EstateTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Estates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EstateTypeId = table.Column<int>(type: "int", nullable: false),
                    EstateLocationId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Price = table.Column<int>(type: "int", nullable: false),
                    SquareMeters = table.Column<double>(type: "float", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    Images = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    NumOfBedrooms = table.Column<int>(type: "int", nullable: false),
                    NumOfBathrooms = table.Column<int>(type: "int", nullable: false),
                    NumOfFloor = table.Column<int>(type: "int", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateUpdated = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Estates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Estates_EstateLocations_EstateLocationId",
                        column: x => x.EstateLocationId,
                        principalTable: "EstateLocations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Estates_EstateTypes_EstateTypeId",
                        column: x => x.EstateTypeId,
                        principalTable: "EstateTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Estates_EstateLocationId",
                table: "Estates",
                column: "EstateLocationId");

            migrationBuilder.CreateIndex(
                name: "IX_Estates_EstateTypeId",
                table: "Estates",
                column: "EstateTypeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Estates");

            migrationBuilder.DropTable(
                name: "EstateLocations");

            migrationBuilder.DropTable(
                name: "EstateTypes");
        }
    }
}
