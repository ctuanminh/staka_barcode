using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Be.Data.Migrations
{
    /// <inheritdoc />
    public partial class rmclmproductIdtblorderchecked : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProductId",
                schema: "Catalog",
                table: "OrderChecked");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "ProductId",
                schema: "Catalog",
                table: "OrderChecked",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }
    }
}
