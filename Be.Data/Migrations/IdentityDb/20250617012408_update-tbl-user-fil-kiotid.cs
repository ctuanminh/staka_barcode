using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Be.Data.Migrations.IdentityDb
{
    /// <inheritdoc />
    public partial class updatetbluserfilkiotid : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "KiotId",
                schema: "Identity",
                table: "Users",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "KiotId",
                schema: "Identity",
                table: "Users");
        }
    }
}
