using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace collabDB.Migrations
{
    /// <inheritdoc />
    public partial class TASK_SUPPORT : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TaskSize",
                columns: table => new
                {
                    TaskSizeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TaskDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Weigth = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskSize", x => x.TaskSizeId);
                });

            migrationBuilder.CreateTable(
                name: "TaskState",
                columns: table => new
                {
                    TaskStateId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskState", x => x.TaskStateId);
                });

            migrationBuilder.CreateTable(
                name: "UsersTask",
                columns: table => new
                {
                    TaskId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GetUtcDate()"),
                    From = table.Column<DateTime>(type: "datetime2", nullable: false),
                    To = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserTest = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    TaskSizeId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TaskStateId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsersTask", x => x.TaskId);
                    table.ForeignKey(
                        name: "FK_UsersTask_TaskSize_TaskSizeId",
                        column: x => x.TaskSizeId,
                        principalTable: "TaskSize",
                        principalColumn: "TaskSizeId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UsersTask_TaskState_TaskStateId",
                        column: x => x.TaskStateId,
                        principalTable: "TaskState",
                        principalColumn: "TaskStateId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UsersTask_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "TaskSize",
                columns: new[] { "TaskSizeId", "TaskDescription", "Weigth" },
                values: new object[,]
                {
                    { 1, "XS", 5 },
                    { 2, "S", 12 },
                    { 3, "M", 25 },
                    { 4, "L", 50 },
                    { 5, "XL", 100 }
                });

            migrationBuilder.InsertData(
                table: "TaskState",
                columns: new[] { "TaskStateId", "Description" },
                values: new object[,]
                {
                    { 1, "Pending" },
                    { 2, "InProgress" },
                    { 3, "Completed" },
                    { 4, "Delayed" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_UsersTask_TaskSizeId",
                table: "UsersTask",
                column: "TaskSizeId");

            migrationBuilder.CreateIndex(
                name: "IX_UsersTask_TaskStateId",
                table: "UsersTask",
                column: "TaskStateId");

            migrationBuilder.CreateIndex(
                name: "IX_UsersTask_UserId",
                table: "UsersTask",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UsersTask");

            migrationBuilder.DropTable(
                name: "TaskSize");

            migrationBuilder.DropTable(
                name: "TaskState");
        }
    }
}
