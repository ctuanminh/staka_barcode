using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Be.Data.Migrations
{
    /// <inheritdoc />
    public partial class updatetblproduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                schema: "Catalog",
                table: "Products");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                schema: "Catalog",
                table: "Products",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                schema: "Catalog",
                table: "Products");

            migrationBuilder.AddColumn<int>(
                name: "Status",
                schema: "Catalog",
                table: "Products",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
