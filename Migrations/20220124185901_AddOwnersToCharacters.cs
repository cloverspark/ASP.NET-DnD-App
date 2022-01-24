using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ASP.NET_DnD_App.Migrations
{
    public partial class AddOwnersToCharacters : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CharacterOwnerId",
                table: "FullCharacterSheet",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_FullCharacterSheet_CharacterOwnerId",
                table: "FullCharacterSheet",
                column: "CharacterOwnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_FullCharacterSheet_AspNetUsers_CharacterOwnerId",
                table: "FullCharacterSheet",
                column: "CharacterOwnerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FullCharacterSheet_AspNetUsers_CharacterOwnerId",
                table: "FullCharacterSheet");

            migrationBuilder.DropIndex(
                name: "IX_FullCharacterSheet_CharacterOwnerId",
                table: "FullCharacterSheet");

            migrationBuilder.DropColumn(
                name: "CharacterOwnerId",
                table: "FullCharacterSheet");
        }
    }
}
