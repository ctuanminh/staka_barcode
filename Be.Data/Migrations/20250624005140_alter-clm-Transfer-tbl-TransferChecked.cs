using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Be.Data.Migrations
{
    /// <inheritdoc />
    public partial class alterclmTransfertblTransferChecked : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Transfer",
                schema: "Catalog",
                table: "TransferCheckeds",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Transfer",
                schema: "Catalog",
                table: "TransferCheckeds");
        }
    }
}
