using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace web_tabel.Services.Migrations
{
    /// <inheritdoc />
    public partial class UserAndFilter : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CompositeFilterId",
                table: "Organizations",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OrganizationFilterId",
                table: "Organizations",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CompositeFilterId",
                table: "Departments",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DepartmentFilterId",
                table: "Departments",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Filters",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FilterType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    OrganizationIds = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompositeFilter_DepartmentIds = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DepartmentIds = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OrganizationFilter_OrganizationIds = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Filters", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    IsApproving = table.Column<bool>(type: "bit", nullable: false),
                    HasRestriction = table.Column<bool>(type: "bit", nullable: false),
                    DaysCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Name);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    RoleName = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    FilterId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_Filters_FilterId",
                        column: x => x.FilterId,
                        principalTable: "Filters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Users_Roles_RoleName",
                        column: x => x.RoleName,
                        principalTable: "Roles",
                        principalColumn: "Name",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Organizations_CompositeFilterId",
                table: "Organizations",
                column: "CompositeFilterId");

            migrationBuilder.CreateIndex(
                name: "IX_Organizations_OrganizationFilterId",
                table: "Organizations",
                column: "OrganizationFilterId");

            migrationBuilder.CreateIndex(
                name: "IX_Departments_CompositeFilterId",
                table: "Departments",
                column: "CompositeFilterId");

            migrationBuilder.CreateIndex(
                name: "IX_Departments_DepartmentFilterId",
                table: "Departments",
                column: "DepartmentFilterId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_FilterId",
                table: "Users",
                column: "FilterId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_RoleName",
                table: "Users",
                column: "RoleName");

            migrationBuilder.AddForeignKey(
                name: "FK_Departments_Filters_CompositeFilterId",
                table: "Departments",
                column: "CompositeFilterId",
                principalTable: "Filters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Departments_Filters_DepartmentFilterId",
                table: "Departments",
                column: "DepartmentFilterId",
                principalTable: "Filters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Organizations_Filters_CompositeFilterId",
                table: "Organizations",
                column: "CompositeFilterId",
                principalTable: "Filters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Organizations_Filters_OrganizationFilterId",
                table: "Organizations",
                column: "OrganizationFilterId",
                principalTable: "Filters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Departments_Filters_CompositeFilterId",
                table: "Departments");

            migrationBuilder.DropForeignKey(
                name: "FK_Departments_Filters_DepartmentFilterId",
                table: "Departments");

            migrationBuilder.DropForeignKey(
                name: "FK_Organizations_Filters_CompositeFilterId",
                table: "Organizations");

            migrationBuilder.DropForeignKey(
                name: "FK_Organizations_Filters_OrganizationFilterId",
                table: "Organizations");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Filters");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropIndex(
                name: "IX_Organizations_CompositeFilterId",
                table: "Organizations");

            migrationBuilder.DropIndex(
                name: "IX_Organizations_OrganizationFilterId",
                table: "Organizations");

            migrationBuilder.DropIndex(
                name: "IX_Departments_CompositeFilterId",
                table: "Departments");

            migrationBuilder.DropIndex(
                name: "IX_Departments_DepartmentFilterId",
                table: "Departments");

            migrationBuilder.DropColumn(
                name: "CompositeFilterId",
                table: "Organizations");

            migrationBuilder.DropColumn(
                name: "OrganizationFilterId",
                table: "Organizations");

            migrationBuilder.DropColumn(
                name: "CompositeFilterId",
                table: "Departments");

            migrationBuilder.DropColumn(
                name: "DepartmentFilterId",
                table: "Departments");
        }
    }
}
