using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SunriseBlazor.Migrations
{
    public partial class first : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Cities",
                columns: table => new
                {
                    CityId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Latitude = table.Column<double>(type: "float", nullable: false),
                    Longitude = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cities", x => x.CityId);
                });

            migrationBuilder.CreateTable(
                name: "SunTime",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Sunrise = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Sunset = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OriginalSunrise = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OriginalSunset = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SummerWinter = table.Column<bool>(type: "bit", nullable: false),
                    CityId = table.Column<int>(type: "int", nullable: false),
                    CityName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CityLatitude = table.Column<double>(type: "float", nullable: false),
                    CityLongitude = table.Column<double>(type: "float", nullable: false),
                    Datum = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SunTime", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Cities");

            migrationBuilder.DropTable(
                name: "SunTime");
        }
    }
}
