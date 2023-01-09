using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LunchnLearnAPI.Migrations
{
    /// <inheritdoc />
    public partial class init3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Attachments_MeetingID",
                table: "Attachments",
                column: "MeetingID");

            migrationBuilder.AddForeignKey(
                name: "FK_Attachments_Meetings_MeetingID",
                table: "Attachments",
                column: "MeetingID",
                principalTable: "Meetings",
                principalColumn: "MeetingID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Attachments_Meetings_MeetingID",
                table: "Attachments");

            migrationBuilder.DropIndex(
                name: "IX_Attachments_MeetingID",
                table: "Attachments");
        }
    }
}
