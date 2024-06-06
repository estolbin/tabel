using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace web_tabel.Services.Migrations
{
    /// <inheritdoc />
    public partial class AddEmployeeState : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EmployeeStates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    EmployeeId = table.Column<Guid>(type: "TEXT", nullable: false),
                    ConditionName = table.Column<string>(type: "TEXT", nullable: false),
                    StartDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    EndDate = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeStates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmployeeStates_EmployeeCondition_ConditionName",
                        column: x => x.ConditionName,
                        principalTable: "EmployeeCondition",
                        principalColumn: "Name",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EmployeeStates_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeStates_ConditionName",
                table: "EmployeeStates",
                column: "ConditionName");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeStates_EmployeeId",
                table: "EmployeeStates",
                column: "EmployeeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmployeeStates");
        }
    }
}
