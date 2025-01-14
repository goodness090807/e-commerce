using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace e_commerce.Data.Migrations
{
    /// <inheritdoc />
    public partial class Add_ValidColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<ulong>(
                name: "Valid",
                table: "Users",
                type: "BIT",
                nullable: false,
                defaultValue: 0ul);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Valid",
                table: "Users");
        }
    }
}
