using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Be.Data.Migrations
{
    /// <inheritdoc />
    public partial class addclmcounttblorderchecked : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Count",
                schema: "Catalog",
                table: "OrderChecked",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Count",
                schema: "Catalog",
                table: "OrderChecked");
        }
    }
}
