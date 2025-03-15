using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyDrone.Web.App.Migrations
{
    /// <inheritdoc />
    public partial class DevicePropUpdate2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Price",
                table: "Device",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "Status",
                table: "Device",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Price",
                table: "Device");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Device");
        }
    }
}
