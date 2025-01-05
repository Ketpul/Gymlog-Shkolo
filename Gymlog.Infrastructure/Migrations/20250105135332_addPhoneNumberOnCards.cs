using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gymlog.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addPhoneNumberOnCards : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "phoneNumber",
                table: "Cards",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "df7c92db-9dec-4483-9b0c-39836de8f44a",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "d044d56a-b3b8-46e2-9c14-85f8b2e24d53", "AQAAAAIAAYagAAAAECE8061/z6QtuG021JUXt5bQwdgcC4/HwHQGFkXlQViB4WQ1hpgT60pHRedLerSqOA==", "14d0f4bb-7a4a-4f44-bea4-df9b1aee90b5" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "phoneNumber",
                table: "Cards");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "df7c92db-9dec-4483-9b0c-39836de8f44a",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "2da850ba-6edd-4d57-9a78-216953958cf6", "AQAAAAIAAYagAAAAEDXU6+0mV+0hequDHORf+2ytSuvl5fdln8iePV4xnpgkEwbw61Wgk+n/SvAIeRtmIQ==", "b0d4ee12-0d99-4f05-9ba3-43ef82d60887" });
        }
    }
}
