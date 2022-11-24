using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Practica.TiendasAPI.Migrations
{
    public partial class AddFirstUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "8e445865-a24d-4543-a6c6-9443d048cdb9", 0, "37800987-bb7e-456d-832a-f28a413c953e", null, false, false, null, null, "FERNANDO", "AQAAAAEAACcQAAAAENSRwWudUsdK4cenM4bLI1b4wIlEoZ13OTjU+eAS4Zxe4VsBM1/Walre6nvUuljyww==", null, false, "cb5d1fb0-668a-4cc4-a763-1209a22c4912", false, "Fernando" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "8e445865-a24d-4543-a6c6-9443d048cdb9");
        }
    }
}
