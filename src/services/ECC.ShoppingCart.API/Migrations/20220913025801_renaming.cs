using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ECC.ShoppingCart.API.Migrations
{
    public partial class renaming : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ItemCarts");

            migrationBuilder.DropTable(
                name: "ClientCarts");

            migrationBuilder.CreateTable(
                name: "CartClients",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClientId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TotalPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CartClients", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CartItens",
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
                    table.PrimaryKey("PK_CartItens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CartItens_CartClients_CartId",
                        column: x => x.CartId,
                        principalTable: "CartClients",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IDX_Client",
                table: "CartClients",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_CartItens_CartId",
                table: "CartItens",
                column: "CartId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CartItens");

            migrationBuilder.DropTable(
                name: "CartClients");

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
                    CartId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Image = table.Column<string>(type: "varchar(100)", nullable: true),
                    Name = table.Column<string>(type: "varchar(100)", nullable: true),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ProductId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false)
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
    }
}
