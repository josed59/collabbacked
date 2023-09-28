using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace collabDB.Migrations
{
    /// <inheritdoc />
    public partial class USERSTASKT_TITLE : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "UsersTask",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Title",
                table: "UsersTask");
        }
    }
}
