using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LunchnLearnAPI.Migrations
{
    /// <inheritdoc />
    public partial class addedauthID : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AuthID",
                table: "Meetings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AuthID",
                table: "Meetings");
        }
    }
}
