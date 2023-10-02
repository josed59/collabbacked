using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace collabDB.Migrations
{
    /// <inheritdoc />
    public partial class DELETEDTaskState : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "TaskState",
                columns: new[] { "TaskStateId", "Description" },
                values: new object[] { 6, "Deleted" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "TaskState",
                keyColumn: "TaskStateId",
                keyValue: 6);
        }
    }
}
