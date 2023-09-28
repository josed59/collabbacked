using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace collabDB.Migrations
{
    /// <inheritdoc />
    public partial class FECHAS_TASK : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CloseDate",
                table: "UsersTask",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "QaDateFinished",
                table: "UsersTask",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CloseDate",
                table: "UsersTask");

            migrationBuilder.DropColumn(
                name: "QaDateFinished",
                table: "UsersTask");
        }
    }
}
