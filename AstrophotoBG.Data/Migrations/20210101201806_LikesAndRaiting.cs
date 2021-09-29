using Microsoft.EntityFrameworkCore.Migrations;

namespace AstrophotoBG.Data.Migrations
{
    public partial class LikesAndRaiting : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Likes",
                table: "Pictures",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Raiting",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Likes",
                table: "Pictures");

            migrationBuilder.DropColumn(
                name: "Raiting",
                table: "AspNetUsers");
        }
    }
}
