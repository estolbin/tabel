﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace web_tabel.Services.Migrations
{
    /// <inheritdoc />
    public partial class FixCondition : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Organizations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Organizations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TimeShiftPeriods",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Start = table.Column<DateTime>(type: "datetime2", nullable: false),
                    End = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Closed = table.Column<bool>(type: "bit", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TimeShiftPeriods", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TypeEmployments",
                columns: table => new
                {
                    Name = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    ColorText = table.Column<string>(type: "nvarchar(7)", maxLength: 7, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TypeEmployments", x => x.Name);
                });

            migrationBuilder.CreateTable(
                name: "TypeOfEmployments",
                columns: table => new
                {
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TypeOfEmployments", x => x.Name);
                });

            migrationBuilder.CreateTable(
                name: "WorkingCalendar",
                columns: table => new
                {
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Year = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkingCalendar", x => x.Date);
                });

            migrationBuilder.CreateTable(
                name: "WorkSchedules",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReferenceDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsWeekly = table.Column<bool>(type: "bit", nullable: false),
                    HoursInWeek = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkSchedules", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Departments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OrganizationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Departments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Departments_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EmployeeCondition",
                columns: table => new
                {
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TypeOfWorkingTimeName = table.Column<string>(type: "nvarchar(3)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeCondition", x => x.Name);
                    table.ForeignKey(
                        name: "FK_EmployeeCondition_TypeEmployments_TypeOfWorkingTimeName",
                        column: x => x.TypeOfWorkingTimeName,
                        principalTable: "TypeEmployments",
                        principalColumn: "Name");
                });

            migrationBuilder.CreateTable(
                name: "TypeOfWorkingTimeRules",
                columns: table => new
                {
                    SourceName = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    TargetName = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TypeOfWorkingTimeRules", x => new { x.SourceName, x.TargetName });
                    table.ForeignKey(
                        name: "FK_TypeOfWorkingTimeRules_TypeEmployments_SourceName",
                        column: x => x.SourceName,
                        principalTable: "TypeEmployments",
                        principalColumn: "Name");
                    table.ForeignKey(
                        name: "FK_TypeOfWorkingTimeRules_TypeEmployments_TargetName",
                        column: x => x.TargetName,
                        principalTable: "TypeEmployments",
                        principalColumn: "Name");
                });

            migrationBuilder.CreateTable(
                name: "WorkSchedulleHours",
                columns: table => new
                {
                    DayNumber = table.Column<int>(type: "int", nullable: false),
                    WorkScheduleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TypeOfWorkingTimeName = table.Column<string>(type: "nvarchar(3)", nullable: false),
                    Hours = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkSchedulleHours", x => new { x.WorkScheduleId, x.DayNumber, x.TypeOfWorkingTimeName });
                    table.ForeignKey(
                        name: "FK_WorkSchedulleHours_TypeEmployments_TypeOfWorkingTimeName",
                        column: x => x.TypeOfWorkingTimeName,
                        principalTable: "TypeEmployments",
                        principalColumn: "Name",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WorkSchedulleHours_WorkSchedules_WorkScheduleId",
                        column: x => x.WorkScheduleId,
                        principalTable: "WorkSchedules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StaffSchedules",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrganizationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DepartmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ParentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NumberOfPositions = table.Column<float>(type: "real", nullable: false),
                    WorkScheduleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StaffSchedules", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StaffSchedules_Departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Departments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StaffSchedules_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_StaffSchedules_WorkSchedules_WorkScheduleId",
                        column: x => x.WorkScheduleId,
                        principalTable: "WorkSchedules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrganizationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DepartmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StaffScheduleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TypeOfEmploymentName = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    WorkScheduleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Employees_Departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Departments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Employees_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Employees_StaffSchedules_StaffScheduleId",
                        column: x => x.StaffScheduleId,
                        principalTable: "StaffSchedules",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Employees_TypeOfEmployments_TypeOfEmploymentName",
                        column: x => x.TypeOfEmploymentName,
                        principalTable: "TypeOfEmployments",
                        principalColumn: "Name");
                    table.ForeignKey(
                        name: "FK_Employees_WorkSchedules_WorkScheduleId",
                        column: x => x.WorkScheduleId,
                        principalTable: "WorkSchedules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EmployeeNames",
                columns: table => new
                {
                    EmployeeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MiddleName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeNames", x => x.EmployeeId);
                    table.ForeignKey(
                        name: "FK_EmployeeNames_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EmployeeStates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ConditionName = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false)
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

            migrationBuilder.CreateTable(
                name: "TimeShifts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    HoursPlanned = table.Column<float>(type: "real", nullable: false),
                    HoursWorked = table.Column<float>(type: "real", nullable: false),
                    TimeShiftPeriodId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EmployeeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WorkScheduleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WorkDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TypeEmploymentPlannedName = table.Column<string>(type: "nvarchar(3)", nullable: true),
                    TypeEmploymentWorkedName = table.Column<string>(type: "nvarchar(3)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TimeShifts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TimeShifts_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TimeShifts_TimeShiftPeriods_TimeShiftPeriodId",
                        column: x => x.TimeShiftPeriodId,
                        principalTable: "TimeShiftPeriods",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TimeShifts_TypeEmployments_TypeEmploymentPlannedName",
                        column: x => x.TypeEmploymentPlannedName,
                        principalTable: "TypeEmployments",
                        principalColumn: "Name");
                    table.ForeignKey(
                        name: "FK_TimeShifts_TypeEmployments_TypeEmploymentWorkedName",
                        column: x => x.TypeEmploymentWorkedName,
                        principalTable: "TypeEmployments",
                        principalColumn: "Name");
                    table.ForeignKey(
                        name: "FK_TimeShifts_WorkSchedules_WorkScheduleId",
                        column: x => x.WorkScheduleId,
                        principalTable: "WorkSchedules",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Departments_OrganizationId",
                table: "Departments",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeCondition_TypeOfWorkingTimeName",
                table: "EmployeeCondition",
                column: "TypeOfWorkingTimeName");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_DepartmentId",
                table: "Employees",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_OrganizationId",
                table: "Employees",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_StaffScheduleId",
                table: "Employees",
                column: "StaffScheduleId");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_TypeOfEmploymentName",
                table: "Employees",
                column: "TypeOfEmploymentName");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_WorkScheduleId",
                table: "Employees",
                column: "WorkScheduleId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeStates_ConditionName",
                table: "EmployeeStates",
                column: "ConditionName");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeStates_EmployeeId",
                table: "EmployeeStates",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_StaffSchedules_DepartmentId",
                table: "StaffSchedules",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_StaffSchedules_OrganizationId",
                table: "StaffSchedules",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_StaffSchedules_WorkScheduleId",
                table: "StaffSchedules",
                column: "WorkScheduleId");

            migrationBuilder.CreateIndex(
                name: "IX_TimeShifts_EmployeeId",
                table: "TimeShifts",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_TimeShifts_TimeShiftPeriodId",
                table: "TimeShifts",
                column: "TimeShiftPeriodId");

            migrationBuilder.CreateIndex(
                name: "IX_TimeShifts_TypeEmploymentPlannedName",
                table: "TimeShifts",
                column: "TypeEmploymentPlannedName");

            migrationBuilder.CreateIndex(
                name: "IX_TimeShifts_TypeEmploymentWorkedName",
                table: "TimeShifts",
                column: "TypeEmploymentWorkedName");

            migrationBuilder.CreateIndex(
                name: "IX_TimeShifts_WorkScheduleId",
                table: "TimeShifts",
                column: "WorkScheduleId");

            migrationBuilder.CreateIndex(
                name: "IX_TypeOfWorkingTimeRules_TargetName",
                table: "TypeOfWorkingTimeRules",
                column: "TargetName");

            migrationBuilder.CreateIndex(
                name: "IX_WorkingCalendar_Year",
                table: "WorkingCalendar",
                column: "Year");

            migrationBuilder.CreateIndex(
                name: "IX_WorkSchedulleHours_TypeOfWorkingTimeName",
                table: "WorkSchedulleHours",
                column: "TypeOfWorkingTimeName");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmployeeNames");

            migrationBuilder.DropTable(
                name: "EmployeeStates");

            migrationBuilder.DropTable(
                name: "TimeShifts");

            migrationBuilder.DropTable(
                name: "TypeOfWorkingTimeRules");

            migrationBuilder.DropTable(
                name: "WorkingCalendar");

            migrationBuilder.DropTable(
                name: "WorkSchedulleHours");

            migrationBuilder.DropTable(
                name: "EmployeeCondition");

            migrationBuilder.DropTable(
                name: "Employees");

            migrationBuilder.DropTable(
                name: "TimeShiftPeriods");

            migrationBuilder.DropTable(
                name: "TypeEmployments");

            migrationBuilder.DropTable(
                name: "StaffSchedules");

            migrationBuilder.DropTable(
                name: "TypeOfEmployments");

            migrationBuilder.DropTable(
                name: "Departments");

            migrationBuilder.DropTable(
                name: "WorkSchedules");

            migrationBuilder.DropTable(
                name: "Organizations");
        }
    }
}
