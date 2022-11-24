using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Practica.TiendasAPI.Migrations
{
    public partial class firstmigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Tiendas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 300, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tiendas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Productos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 300, nullable: true),
                    Price = table.Column<double>(type: "REAL", nullable: false),
                    ShopId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Productos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Productos_Tiendas_ShopId",
                        column: x => x.ShopId,
                        principalTable: "Tiendas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Tiendas",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[] { 1, "Pan recién hecho.", "Panadería Paqui" });

            migrationBuilder.InsertData(
                table: "Tiendas",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[] { 2, "Todo a 1€.", "Bazar Todo a 1" });

            migrationBuilder.InsertData(
                table: "Tiendas",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[] { 3, null, "Supermercados MAS" });

            migrationBuilder.InsertData(
                table: "Tiendas",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[] { 4, null, "Verdulería La Fresca" });

            migrationBuilder.InsertData(
                table: "Productos",
                columns: new[] { "Id", "Description", "Name", "Price", "ShopId" },
                values: new object[] { 1, null, "Pan integral", 0.29999999999999999, 1 });

            migrationBuilder.InsertData(
                table: "Productos",
                columns: new[] { "Id", "Description", "Name", "Price", "ShopId" },
                values: new object[] { 2, "Porción de bizcocho casero.", "Bizcocho", 0.29999999999999999, 1 });

            migrationBuilder.InsertData(
                table: "Productos",
                columns: new[] { "Id", "Description", "Name", "Price", "ShopId" },
                values: new object[] { 3, "Botella de 1 litro de cerveza.", "Cruzcampo 1L", 1.0, 2 });

            migrationBuilder.InsertData(
                table: "Productos",
                columns: new[] { "Id", "Description", "Name", "Price", "ShopId" },
                values: new object[] { 4, "Botella de 1 litro de agua.", "Agua 2L", 1.0, 2 });

            migrationBuilder.InsertData(
                table: "Productos",
                columns: new[] { "Id", "Description", "Name", "Price", "ShopId" },
                values: new object[] { 5, "Botella de 1 litro de agua.", "Agua 2L", 1.2, 3 });

            migrationBuilder.InsertData(
                table: "Productos",
                columns: new[] { "Id", "Description", "Name", "Price", "ShopId" },
                values: new object[] { 6, "1 Kilogramo de tomates.", "Tomate", 1.99, 4 });

            migrationBuilder.InsertData(
                table: "Productos",
                columns: new[] { "Id", "Description", "Name", "Price", "ShopId" },
                values: new object[] { 7, "1 Kilogramo de calabacín.", "Calabacín", 1.99, 4 });

            migrationBuilder.CreateIndex(
                name: "IX_Productos_ShopId",
                table: "Productos",
                column: "ShopId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Productos");

            migrationBuilder.DropTable(
                name: "Tiendas");
        }
    }
}
