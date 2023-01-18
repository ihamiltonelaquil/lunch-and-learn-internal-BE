using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LunchnLearnAPI.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Meetings",
                columns: table => new
                {
                    MeetingID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatorName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MeetingStart = table.Column<DateTime>(type: "datetime2", nullable: true),
                    MeetingEnd = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Topic = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LinkToSlides = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TeamsLink = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Meetings", x => x.MeetingID);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Meetings");
        }
    }
}
