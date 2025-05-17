using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace E_commerce_23TH0024.Data.Migrations.SystemSetting
{
    /// <inheritdoc />
    public partial class CreateSystemSettingTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Configuration",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ConfigCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ConfigName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ConfigValue = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConfigType = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Configuration", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NhomMenu",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TenNhomMenu = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NhomMenu", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Menu",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdNhomMenu = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    LoaiMenu = table.Column<int>(type: "int", nullable: false),
                    MaLoaiMenu = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Menu", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Menu_NhomMenu_IdNhomMenu",
                        column: x => x.IdNhomMenu,
                        principalTable: "NhomMenu",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Menu_IdNhomMenu",
                table: "Menu",
                column: "IdNhomMenu");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Configuration");

            migrationBuilder.DropTable(
                name: "Menu");

            migrationBuilder.DropTable(
                name: "NhomMenu");
        }
    }
}
