using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Practica.TiendasAPI.Migrations
{
    public partial class AddTestUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "8e445865-a24d-4543-a6c6-9443d048cdb9",
                columns: new[] { "ConcurrencyStamp", "Email", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "SecurityStamp", "UserName" },
                values: new object[] { "44142cc2-e6ea-467e-a760-988e3fb0e342", "correo_prueba@hotmail.com", "CORREO_PRUEBA@HOTMAIL.COM", "USUARIO", "AQAAAAEAACcQAAAAEHN/pPoEFSWCLQKCxvnkJpsE6ISllW/oLl6r2jVCrsRIeX8AEDAbHnHzPIe6YAjXcw==", "56925c1c-90e5-4085-852d-2f53088679e2", "Usuario" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "8e445865-a24d-4543-a6c6-9443d048cdb9",
                columns: new[] { "ConcurrencyStamp", "Email", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "SecurityStamp", "UserName" },
                values: new object[] { "37800987-bb7e-456d-832a-f28a413c953e", null, null, "FERNANDO", "AQAAAAEAACcQAAAAENSRwWudUsdK4cenM4bLI1b4wIlEoZ13OTjU+eAS4Zxe4VsBM1/Walre6nvUuljyww==", "cb5d1fb0-668a-4cc4-a763-1209a22c4912", "Fernando" });
        }
    }
}
