using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HmctsDts.Server.Migrations
{
    /// <inheritdoc />
    public partial class StaffIdUniqueConstraint : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Users_StaffId",
                table: "Users",
                column: "StaffId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Users_StaffId",
                table: "Users");
        }
    }
}
