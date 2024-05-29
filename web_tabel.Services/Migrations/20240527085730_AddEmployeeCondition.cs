using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace web_tabel.Services.Migrations
{
    /// <inheritdoc />
    public partial class AddEmployeeCondition : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EmployeeCondition",
                columns: table => new
                {
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    TypeOfWorkingTimeName = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeCondition", x => x.Name);
                    table.ForeignKey(
                        name: "FK_EmployeeCondition_TypeEmployments_TypeOfWorkingTimeName",
                        column: x => x.TypeOfWorkingTimeName,
                        principalTable: "TypeEmployments",
                        principalColumn: "Name",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeCondition_TypeOfWorkingTimeName",
                table: "EmployeeCondition",
                column: "TypeOfWorkingTimeName");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmployeeCondition");
        }
    }
}
