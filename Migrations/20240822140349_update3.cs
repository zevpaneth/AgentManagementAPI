using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AgentManagementAPI.Migrations
{
    /// <inheritdoc />
    public partial class update3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddCheckConstraint(
                name: "CK_Location_x_Range",
                table: "Location",
                sql: "[x] BETWEEN 0 AND 1000");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Location_y_Range",
                table: "Location",
                sql: "[y] BETWEEN 0 AND 1000");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_Location_x_Range",
                table: "Location");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Location_y_Range",
                table: "Location");
        }
    }
}
