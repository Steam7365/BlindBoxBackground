using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlindBox.EF.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "box_connect",
                schema: "ro");

            migrationBuilder.DropTable(
                name: "describetype",
                schema: "ro");

            migrationBuilder.DropTable(
                name: "login",
                schema: "ro");

            migrationBuilder.DropTable(
                name: "orderinfo",
                schema: "ao");

            migrationBuilder.DropTable(
                name: "box_commodity",
                schema: "ro");

            migrationBuilder.DropTable(
                name: "staff",
                schema: "ro");

            migrationBuilder.DropTable(
                name: "addressinfo",
                schema: "ao");

            migrationBuilder.DropTable(
                name: "draw",
                schema: "ro");

            migrationBuilder.DropTable(
                name: "box_folder",
                schema: "ro");

            migrationBuilder.DropTable(
                name: "box_commodity_detail",
                schema: "ro");

            migrationBuilder.DropTable(
                name: "userinfo",
                schema: "ao");

            migrationBuilder.DropTable(
                name: "grade",
                schema: "ro");
        }
    }
}
