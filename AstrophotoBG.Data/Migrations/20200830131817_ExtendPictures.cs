using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AstrophotoBG.Data.Migrations
{
    public partial class ExtendPictures : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "Date",
                table: "Pictures",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Pictures",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Date",
                table: "Pictures");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Pictures");
        }
    }
}
