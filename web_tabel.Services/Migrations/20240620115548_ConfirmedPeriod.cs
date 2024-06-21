using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace web_tabel.Services.Migrations
{
    /// <inheritdoc />
    public partial class ConfirmedPeriod : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employees_Organizations_OrganizationId",
                table: "Employees");

            migrationBuilder.DropForeignKey(
                name: "FK_Employees_StaffSchedules_StaffScheduleId",
                table: "Employees");

            migrationBuilder.DropForeignKey(
                name: "FK_StaffSchedules_Organizations_OrganizationId",
                table: "StaffSchedules");

            migrationBuilder.DropForeignKey(
                name: "FK_TimeShifts_WorkSchedules_WorkScheduleId",
                table: "TimeShifts");

            migrationBuilder.DropForeignKey(
                name: "FK_TypeOfWorkingTimeRules_TypeEmployments_SourceName",
                table: "TypeOfWorkingTimeRules");

            migrationBuilder.DropForeignKey(
                name: "FK_TypeOfWorkingTimeRules_TypeEmployments_TargetName",
                table: "TypeOfWorkingTimeRules");

            migrationBuilder.CreateTable(
                name: "ConfirmedPeriods",
                columns: table => new
                {
                    PeriodId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DepartmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FirstHalfIsConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    SecondHalfIsConfirmed = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConfirmedPeriods", x => new { x.PeriodId, x.DepartmentId });
                    table.ForeignKey(
                        name: "FK_ConfirmedPeriods_Departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Departments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ConfirmedPeriods_TimeShiftPeriods_PeriodId",
                        column: x => x.PeriodId,
                        principalTable: "TimeShiftPeriods",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ConfirmedPeriods_DepartmentId",
                table: "ConfirmedPeriods",
                column: "DepartmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_Organizations_OrganizationId",
                table: "Employees",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_StaffSchedules_StaffScheduleId",
                table: "Employees",
                column: "StaffScheduleId",
                principalTable: "StaffSchedules",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_StaffSchedules_Organizations_OrganizationId",
                table: "StaffSchedules",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TimeShifts_WorkSchedules_WorkScheduleId",
                table: "TimeShifts",
                column: "WorkScheduleId",
                principalTable: "WorkSchedules",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TypeOfWorkingTimeRules_TypeEmployments_SourceName",
                table: "TypeOfWorkingTimeRules",
                column: "SourceName",
                principalTable: "TypeEmployments",
                principalColumn: "Name",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TypeOfWorkingTimeRules_TypeEmployments_TargetName",
                table: "TypeOfWorkingTimeRules",
                column: "TargetName",
                principalTable: "TypeEmployments",
                principalColumn: "Name",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employees_Organizations_OrganizationId",
                table: "Employees");

            migrationBuilder.DropForeignKey(
                name: "FK_Employees_StaffSchedules_StaffScheduleId",
                table: "Employees");

            migrationBuilder.DropForeignKey(
                name: "FK_StaffSchedules_Organizations_OrganizationId",
                table: "StaffSchedules");

            migrationBuilder.DropForeignKey(
                name: "FK_TimeShifts_WorkSchedules_WorkScheduleId",
                table: "TimeShifts");

            migrationBuilder.DropForeignKey(
                name: "FK_TypeOfWorkingTimeRules_TypeEmployments_SourceName",
                table: "TypeOfWorkingTimeRules");

            migrationBuilder.DropForeignKey(
                name: "FK_TypeOfWorkingTimeRules_TypeEmployments_TargetName",
                table: "TypeOfWorkingTimeRules");

            migrationBuilder.DropTable(
                name: "ConfirmedPeriods");

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_Organizations_OrganizationId",
                table: "Employees",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_StaffSchedules_StaffScheduleId",
                table: "Employees",
                column: "StaffScheduleId",
                principalTable: "StaffSchedules",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_StaffSchedules_Organizations_OrganizationId",
                table: "StaffSchedules",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TimeShifts_WorkSchedules_WorkScheduleId",
                table: "TimeShifts",
                column: "WorkScheduleId",
                principalTable: "WorkSchedules",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TypeOfWorkingTimeRules_TypeEmployments_SourceName",
                table: "TypeOfWorkingTimeRules",
                column: "SourceName",
                principalTable: "TypeEmployments",
                principalColumn: "Name");

            migrationBuilder.AddForeignKey(
                name: "FK_TypeOfWorkingTimeRules_TypeEmployments_TargetName",
                table: "TypeOfWorkingTimeRules",
                column: "TargetName",
                principalTable: "TypeEmployments",
                principalColumn: "Name");
        }
    }
}
