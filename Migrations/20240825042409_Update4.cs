using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AgentManagementAPI.Migrations
{
    /// <inheritdoc />
    public partial class Update4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PhotoUrl",
                table: "Target",
                newName: "photoUrl");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "photoUrl",
                table: "Target",
                newName: "PhotoUrl");
        }
    }
}
