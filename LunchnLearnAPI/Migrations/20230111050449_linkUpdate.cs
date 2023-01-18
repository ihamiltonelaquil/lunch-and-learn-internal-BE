using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LunchnLearnAPI.Migrations
{
    /// <inheritdoc />
    public partial class linkUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Links",
                columns: table => new
                {
                    linkId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    linkName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    linkUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MeetingID = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Links", x => x.linkId);
                    table.ForeignKey(
                        name: "FK_Links_Meetings_MeetingID",
                        column: x => x.MeetingID,
                        principalTable: "Meetings",
                        principalColumn: "MeetingID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Links_MeetingID",
                table: "Links",
                column: "MeetingID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Links");
        }
    }
}
