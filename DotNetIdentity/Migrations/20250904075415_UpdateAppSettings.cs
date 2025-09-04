using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DotNetIdentity.Migrations
{
    /// <inheritdoc />
    public partial class UpdateAppSettings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "IdentityRole<string>",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IdentityRole<string>", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "AppSettings",
                keyColumns: new[] { "Name", "Type" },
                keyValues: new object[] { "ShowMfaEnableBanner", "GlobalSettings" },
                column: "Value",
                value: "True");

            migrationBuilder.UpdateData(
                table: "AppSettings",
                keyColumns: new[] { "Name", "Type" },
                keyValues: new object[] { "SmtpPort", "MailSettings" },
                column: "Value",
                value: "25");

            migrationBuilder.InsertData(
                table: "AppSettings",
                columns: new[] { "Name", "Type", "Value" },
                values: new object[,]
                {
                    { "ApplicationLogo", "BrandSettings", null },
                    { "LoginBackground", "BrandSettings", null }
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "6fbfb682-568c-4f5b-a298-85937ca4f7f3",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "e1201a54-ea68-4c99-99c8-e6ec8bc9f140", "AQAAAAIAAYagAAAAEFXCtdRX0VN3TMQopdnsyGQD0GXk5SpgD9n1zdtrqt3ZQ7jZf0JbJkHLzPeyf7KTmw==", "6fea72a0-a034-4799-abea-47573504bc1d" });

            migrationBuilder.InsertData(
                table: "IdentityRole<string>",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "dffc6dd5-b145-41e9-a861-c87ff673e9ca", "1", "Admin", "ADMIN" },
                    { "f8a527ac-d7f6-4d9d-aca6-46b2261b042b", "8e93b798-9018-49d5-adbe-ab33d959cb22", "User", "USER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "IdentityRole<string>");

            migrationBuilder.DeleteData(
                table: "AppSettings",
                keyColumns: new[] { "Name", "Type" },
                keyValues: new object[] { "ApplicationLogo", "BrandSettings" });

            migrationBuilder.DeleteData(
                table: "AppSettings",
                keyColumns: new[] { "Name", "Type" },
                keyValues: new object[] { "LoginBackground", "BrandSettings" });

            migrationBuilder.UpdateData(
                table: "AppSettings",
                keyColumns: new[] { "Name", "Type" },
                keyValues: new object[] { "ShowMfaEnableBanner", "GlobalSettings" },
                column: "Value",
                value: "true");

            migrationBuilder.UpdateData(
                table: "AppSettings",
                keyColumns: new[] { "Name", "Type" },
                keyValues: new object[] { "SmtpPort", "MailSettings" },
                column: "Value",
                value: "587");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "6fbfb682-568c-4f5b-a298-85937ca4f7f3",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "13e1cffd-49a0-49bc-a380-4443838a6742", "AQAAAAIAAYagAAAAEAX3jRxxi8hIaibNJfl8SWVNLfubJN6838pIySruBBHoSIVcAo7Uytci3OULTfYf+A==", "7c3b2ca6-898e-4c2e-a0fe-261d43de5ca5" });
        }
    }
}
