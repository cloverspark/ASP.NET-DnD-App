using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ASP.NET_DnD_App.Migrations
{
    public partial class AddCampaignInvitesCampaignsCampaignPlayersTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CampaignInvites",
                columns: table => new
                {
                    InviteId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DungeonMasterId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    InviteCode = table.Column<int>(type: "int", nullable: false),
                    InvitedPlayerUserName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CampaignInvites", x => x.InviteId);
                    table.ForeignKey(
                        name: "FK_CampaignInvites_AspNetUsers_DungeonMasterId",
                        column: x => x.DungeonMasterId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Campaigns",
                columns: table => new
                {
                    CampaignId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DungeonMasterId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Campaigns", x => x.CampaignId);
                    table.ForeignKey(
                        name: "FK_Campaigns_AspNetUsers_DungeonMasterId",
                        column: x => x.DungeonMasterId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CampaignPlayers",
                columns: table => new
                {
                    PlayerId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CampaignsIdCampaignId = table.Column<int>(type: "int", nullable: false),
                    BasicPlayerId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CampaignPlayers", x => x.PlayerId);
                    table.ForeignKey(
                        name: "FK_CampaignPlayers_AspNetUsers_BasicPlayerId",
                        column: x => x.BasicPlayerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CampaignPlayers_Campaigns_CampaignsIdCampaignId",
                        column: x => x.CampaignsIdCampaignId,
                        principalTable: "Campaigns",
                        principalColumn: "CampaignId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CampaignInvites_DungeonMasterId",
                table: "CampaignInvites",
                column: "DungeonMasterId");

            migrationBuilder.CreateIndex(
                name: "IX_CampaignPlayers_BasicPlayerId",
                table: "CampaignPlayers",
                column: "BasicPlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_CampaignPlayers_CampaignsIdCampaignId",
                table: "CampaignPlayers",
                column: "CampaignsIdCampaignId");

            migrationBuilder.CreateIndex(
                name: "IX_Campaigns_DungeonMasterId",
                table: "Campaigns",
                column: "DungeonMasterId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CampaignInvites");

            migrationBuilder.DropTable(
                name: "CampaignPlayers");

            migrationBuilder.DropTable(
                name: "Campaigns");
        }
    }
}
