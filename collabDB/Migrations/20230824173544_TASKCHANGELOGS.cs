using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace collabDB.Migrations
{
    /// <inheritdoc />
    public partial class TASKCHANGELOGS : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TaskChangeLogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Comment = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TaskId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GetUtcDate()"),
                    IsCompleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    isUatCompleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    userid = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskChangeLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TaskChangeLogs_User_userid",
                        column: x => x.userid,
                        principalTable: "User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TaskChangeLogs_UsersTask_TaskId",
                        column: x => x.TaskId,
                        principalTable: "UsersTask",
                        principalColumn: "TaskId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TaskChangeLogs_TaskId",
                table: "TaskChangeLogs",
                column: "TaskId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskChangeLogs_userid",
                table: "TaskChangeLogs",
                column: "userid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TaskChangeLogs");
        }
    }
}
