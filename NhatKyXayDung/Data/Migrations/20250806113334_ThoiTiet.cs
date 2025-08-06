using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NhatKyXayDung.Data.Migrations
{
    /// <inheritdoc />
    public partial class ThoiTiet : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ThoiTietChieu",
                table: "NhatKy",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ThoiTietSang",
                table: "NhatKy",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ThoiTietChieu",
                table: "NhatKy");

            migrationBuilder.DropColumn(
                name: "ThoiTietSang",
                table: "NhatKy");
        }
    }
}
