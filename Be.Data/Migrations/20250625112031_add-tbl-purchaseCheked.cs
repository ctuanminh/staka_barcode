using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Be.Data.Migrations
{
    /// <inheritdoc />
    public partial class addtblpurchaseCheked : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PurchaseCheckeds",
                schema: "Catalog",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PurchaseId = table.Column<long>(type: "bigint", nullable: false),
                    PurchaseCode = table.Column<string>(type: "text", nullable: true),
                    ProductBarCode = table.Column<string>(type: "text", nullable: true),
                    BranchId = table.Column<long>(type: "bigint", nullable: false),
                    UserName = table.Column<string>(type: "text", nullable: true),
                    Checked = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurchaseCheckeds", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PurchaseCheckeds",
                schema: "Catalog");
        }
    }
}
