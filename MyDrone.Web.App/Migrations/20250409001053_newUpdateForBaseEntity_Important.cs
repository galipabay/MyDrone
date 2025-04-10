using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyDrone.Web.App.Migrations
{
    /// <inheritdoc />
    public partial class newUpdateForBaseEntity_Important : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DeleteReason",
                table: "User",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "User",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeleteReason",
                table: "Favorite",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "Favorite",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeleteReason",
                table: "Device",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "Device",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeleteReason",
                table: "User");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "User");

            migrationBuilder.DropColumn(
                name: "DeleteReason",
                table: "Favorite");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "Favorite");

            migrationBuilder.DropColumn(
                name: "DeleteReason",
                table: "Device");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "Device");
        }
    }
}
