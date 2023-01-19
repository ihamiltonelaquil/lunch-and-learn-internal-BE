using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LunchnLearnAPI.Migrations
{
    /// <inheritdoc />
    public partial class addedusers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LinkToSlides",
                table: "Meetings");

            migrationBuilder.DropColumn(
                name: "TeamsLink",
                table: "Meetings");

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    AuthID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.AuthID);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.AddColumn<string>(
                name: "LinkToSlides",
                table: "Meetings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TeamsLink",
                table: "Meetings",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
