using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace e_commerce.Data.Migrations
{
    /// <inheritdoc />
    public partial class Remove_SEO_Image : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MetaPictureUrl",
                table: "ProductSEOs");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MetaPictureUrl",
                table: "ProductSEOs",
                type: "varchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");
        }
    }
}
