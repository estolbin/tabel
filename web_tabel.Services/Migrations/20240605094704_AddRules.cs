using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace web_tabel.Services.Migrations
{
    /// <inheritdoc />
    public partial class AddRules : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TypeOfWorkingTimeRules",
                columns: table => new
                {
                    SourceName = table.Column<string>(type: "TEXT", maxLength: 3, nullable: false),
                    TargetName = table.Column<string>(type: "TEXT", maxLength: 3, nullable: false)
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

            migrationBuilder.CreateIndex(
                name: "IX_TypeOfWorkingTimeRules_TargetName",
                table: "TypeOfWorkingTimeRules",
                column: "TargetName");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TypeOfWorkingTimeRules");
        }
    }
}
