using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ASP.NET_DnD_App.Migrations
{
    public partial class MakeCharacterOwnerOptional : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FullCharacterSheet_AspNetUsers_CharacterOwnerId",
                table: "FullCharacterSheet");

            migrationBuilder.AlterColumn<string>(
                name: "CharacterOwnerId",
                table: "FullCharacterSheet",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddForeignKey(
                name: "FK_FullCharacterSheet_AspNetUsers_CharacterOwnerId",
                table: "FullCharacterSheet",
                column: "CharacterOwnerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FullCharacterSheet_AspNetUsers_CharacterOwnerId",
                table: "FullCharacterSheet");

            migrationBuilder.AlterColumn<string>(
                name: "CharacterOwnerId",
                table: "FullCharacterSheet",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_FullCharacterSheet_AspNetUsers_CharacterOwnerId",
                table: "FullCharacterSheet",
                column: "CharacterOwnerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
