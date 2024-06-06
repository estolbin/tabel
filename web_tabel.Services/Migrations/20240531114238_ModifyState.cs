using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace web_tabel.Services.Migrations
{
    /// <inheritdoc />
    public partial class ModifyState : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeStates_EmployeeCondition_ConditionName",
                table: "EmployeeStates");

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeStates_EmployeeCondition_ConditionName",
                table: "EmployeeStates",
                column: "ConditionName",
                principalTable: "EmployeeCondition",
                principalColumn: "Name");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeStates_EmployeeCondition_ConditionName",
                table: "EmployeeStates");

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeStates_EmployeeCondition_ConditionName",
                table: "EmployeeStates",
                column: "ConditionName",
                principalTable: "EmployeeCondition",
                principalColumn: "Name",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
