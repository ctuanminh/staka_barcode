using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Be.Data.Migrations
{
    /// <inheritdoc />
    public partial class alteruserNametbltransferCheckeds : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                schema: "Catalog",
                table: "TransferCheckeds");

            migrationBuilder.AddColumn<string>(
                name: "UserName",
                schema: "Catalog",
                table: "TransferCheckeds",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserName",
                schema: "Catalog",
                table: "TransferCheckeds");

            migrationBuilder.AddColumn<long>(
                name: "UserId",
                schema: "Catalog",
                table: "TransferCheckeds",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }
    }
}
