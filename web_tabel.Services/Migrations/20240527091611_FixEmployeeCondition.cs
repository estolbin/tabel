using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace web_tabel.Services.Migrations
{
    /// <inheritdoc />
    public partial class FixEmployeeCondition : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeCondition_TypeEmployments_TypeOfWorkingTimeName",
                table: "EmployeeCondition");

            migrationBuilder.AlterColumn<string>(
                name: "TypeOfWorkingTimeName",
                table: "EmployeeCondition",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeCondition_TypeEmployments_TypeOfWorkingTimeName",
                table: "EmployeeCondition",
                column: "TypeOfWorkingTimeName",
                principalTable: "TypeEmployments",
                principalColumn: "Name");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeCondition_TypeEmployments_TypeOfWorkingTimeName",
                table: "EmployeeCondition");

            migrationBuilder.AlterColumn<string>(
                name: "TypeOfWorkingTimeName",
                table: "EmployeeCondition",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeCondition_TypeEmployments_TypeOfWorkingTimeName",
                table: "EmployeeCondition",
                column: "TypeOfWorkingTimeName",
                principalTable: "TypeEmployments",
                principalColumn: "Name",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
