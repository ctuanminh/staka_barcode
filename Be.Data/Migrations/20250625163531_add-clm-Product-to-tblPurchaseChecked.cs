using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Be.Data.Migrations
{
    /// <inheritdoc />
    public partial class addclmProducttotblPurchaseChecked : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ProductCode",
                schema: "Catalog",
                table: "PurchaseCheckeds",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProductCode",
                schema: "Catalog",
                table: "PurchaseCheckeds");
        }
    }
}
