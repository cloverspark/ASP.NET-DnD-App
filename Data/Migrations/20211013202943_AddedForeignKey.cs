using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ASP.NET_DnD_App.Data.Migrations
{
    public partial class AddedForeignKey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DescriptionId",
                table: "Characters");

            migrationBuilder.AlterColumn<string>(
                name: "ClassName",
                table: "Characters",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<int>(
                name: "CharacterDescriptionDescriptionId",
                table: "Characters",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Characters_CharacterDescriptionDescriptionId",
                table: "Characters",
                column: "CharacterDescriptionDescriptionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Characters_Description_CharacterDescriptionDescriptionId",
                table: "Characters",
                column: "CharacterDescriptionDescriptionId",
                principalTable: "Description",
                principalColumn: "DescriptionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Characters_Description_CharacterDescriptionDescriptionId",
                table: "Characters");

            migrationBuilder.DropIndex(
                name: "IX_Characters_CharacterDescriptionDescriptionId",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "CharacterDescriptionDescriptionId",
                table: "Characters");

            migrationBuilder.AlterColumn<string>(
                name: "ClassName",
                table: "Characters",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DescriptionId",
                table: "Characters",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
