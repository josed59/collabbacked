using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace collabDB.Migrations
{
    /// <inheritdoc />
    public partial class NEWSTATE : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "TaskState",
                columns: new[] { "TaskStateId", "Description" },
                values: new object[] { 5, "StandBy" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "TaskState",
                keyColumn: "TaskStateId",
                keyValue: 5);
        }
    }
}
