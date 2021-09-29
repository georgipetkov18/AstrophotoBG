using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AstrophotoBG.Data.Migrations
{
    public partial class AddPictureData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "PictureData",
                table: "Pictures",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PictureData",
                table: "Pictures");
        }
    }
}
