using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace web_tabel.Services.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Organizations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Organizations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TimeShiftPeriods",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Start = table.Column<DateTime>(type: "TEXT", nullable: false),
                    End = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Closed = table.Column<bool>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TimeShiftPeriods", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TypeEmployments",
                columns: table => new
                {
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TypeEmployments", x => x.Name);
                });

            migrationBuilder.CreateTable(
                name: "TypeOfEmployments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TypeOfEmployments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WorkSchedules",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    ReferenceDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    HoursInWeek = table.Column<float>(type: "REAL", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkSchedules", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Departments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    OrganizationId = table.Column<Guid>(type: "TEXT", nullable: false)
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
                name: "WorkSchedulleHours",
                columns: table => new
                {
                    DayNumber = table.Column<int>(type: "INTEGER", nullable: false),
                    WorkScheduleId = table.Column<Guid>(type: "TEXT", nullable: false),
                    TypeOfWorkingTimeName = table.Column<string>(type: "TEXT", nullable: false),
                    Hours = table.Column<float>(type: "REAL", nullable: false)
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
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    OrganizationId = table.Column<Guid>(type: "TEXT", nullable: false),
                    DepartmentId = table.Column<Guid>(type: "TEXT", nullable: false),
                    ParentId = table.Column<Guid>(type: "TEXT", nullable: true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    NumberOfPositions = table.Column<float>(type: "REAL", nullable: false),
                    WorkScheduleId = table.Column<Guid>(type: "TEXT", nullable: false)
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
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    OrganizationId = table.Column<Guid>(type: "TEXT", nullable: false),
                    DepartmentId = table.Column<Guid>(type: "TEXT", nullable: false),
                    StaffScheduleId = table.Column<Guid>(type: "TEXT", nullable: false),
                    TypeOfEmploymentId = table.Column<Guid>(type: "TEXT", nullable: false),
                    WorkScheduleId = table.Column<Guid>(type: "TEXT", nullable: false)
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
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Employees_StaffSchedules_StaffScheduleId",
                        column: x => x.StaffScheduleId,
                        principalTable: "StaffSchedules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Employees_TypeOfEmployments_TypeOfEmploymentId",
                        column: x => x.TypeOfEmploymentId,
                        principalTable: "TypeOfEmployments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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
                    EmployeeId = table.Column<Guid>(type: "TEXT", nullable: false),
                    FirstName = table.Column<string>(type: "TEXT", nullable: false),
                    LastName = table.Column<string>(type: "TEXT", nullable: false),
                    MiddleName = table.Column<string>(type: "TEXT", nullable: false),
                    Id = table.Column<Guid>(type: "TEXT", nullable: false)
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
                name: "TimeShifts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    HoursPlanned = table.Column<float>(type: "REAL", nullable: false),
                    HoursWorked = table.Column<float>(type: "REAL", nullable: false),
                    TimeShiftPeriodId = table.Column<Guid>(type: "TEXT", nullable: false),
                    EmployeeId = table.Column<Guid>(type: "TEXT", nullable: false),
                    WorkScheduleId = table.Column<Guid>(type: "TEXT", nullable: false),
                    WorkDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    TypeEmploymentName = table.Column<string>(type: "TEXT", nullable: true)
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
                        name: "FK_TimeShifts_TypeEmployments_TypeEmploymentName",
                        column: x => x.TypeEmploymentName,
                        principalTable: "TypeEmployments",
                        principalColumn: "Name");
                    table.ForeignKey(
                        name: "FK_TimeShifts_WorkSchedules_WorkScheduleId",
                        column: x => x.WorkScheduleId,
                        principalTable: "WorkSchedules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Departments_OrganizationId",
                table: "Departments",
                column: "OrganizationId");

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
                name: "IX_Employees_TypeOfEmploymentId",
                table: "Employees",
                column: "TypeOfEmploymentId");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_WorkScheduleId",
                table: "Employees",
                column: "WorkScheduleId");

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
                name: "IX_TimeShifts_TypeEmploymentName",
                table: "TimeShifts",
                column: "TypeEmploymentName");

            migrationBuilder.CreateIndex(
                name: "IX_TimeShifts_WorkScheduleId",
                table: "TimeShifts",
                column: "WorkScheduleId");

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
                name: "TimeShifts");

            migrationBuilder.DropTable(
                name: "WorkSchedulleHours");

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
