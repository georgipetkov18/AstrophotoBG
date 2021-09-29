using Microsoft.EntityFrameworkCore.Migrations;

namespace AstrophotoBG.Data.Migrations
{
    public partial class ChangeTechniquePlace : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Тechnique",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<string>(
                name: "Technique",
                table: "Pictures",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Technique",
                table: "Pictures");

            migrationBuilder.AddColumn<string>(
                name: "Тechnique",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
