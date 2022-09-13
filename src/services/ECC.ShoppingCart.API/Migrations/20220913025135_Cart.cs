using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ECC.ShoppingCart.API.Migrations
{
    public partial class Cart : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ClientCarts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClientId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TotalPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientCarts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ItemCarts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProductId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "varchar(100)", nullable: true),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Image = table.Column<string>(type: "varchar(100)", nullable: true),
                    CartId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemCarts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ItemCarts_ClientCarts_CartId",
                        column: x => x.CartId,
                        principalTable: "ClientCarts",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IDX_Client",
                table: "ClientCarts",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemCarts_CartId",
                table: "ItemCarts",
                column: "CartId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ItemCarts");

            migrationBuilder.DropTable(
                name: "ClientCarts");
        }
    }
}
