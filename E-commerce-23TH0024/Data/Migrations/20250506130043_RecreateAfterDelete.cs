using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace E_commerce_23TH0024.Data.Migrations
{
    /// <inheritdoc />
    public partial class RecreateAfterDelete : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DiscountRules_LoaiSanPhams_LoaiSanPhamId",
                table: "DiscountRules");

            migrationBuilder.DropForeignKey(
                name: "FK_Menus_LoaiSanPhams_LoaiSanPhamId",
                table: "Menus");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductVariantAttributes_AttributeValue_AttributeValueID",
                table: "ProductVariantAttributes");

            migrationBuilder.DropForeignKey(
                name: "FK_SanPham_LoaiSanPhams_MaLSP",
                table: "SanPham");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LoaiSanPhams",
                table: "LoaiSanPhams");

            migrationBuilder.RenameTable(
                name: "LoaiSanPhams",
                newName: "LoaiSanPham");

            migrationBuilder.RenameColumn(
                name: "MaLSP",
                table: "SanPham",
                newName: "IdLoaiSanPham");

            migrationBuilder.RenameIndex(
                name: "IX_SanPham_MaLSP",
                table: "SanPham",
                newName: "IX_SanPham_IdLoaiSanPham");

            migrationBuilder.RenameColumn(
                name: "AttributeValueID",
                table: "ProductVariantAttributes",
                newName: "AttributeValueId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductVariantAttributes_AttributeValueID",
                table: "ProductVariantAttributes",
                newName: "IX_ProductVariantAttributes_AttributeValueId");

            migrationBuilder.AddColumn<int>(
                name: "IdAttributeValue",
                table: "ProductVariantAttributes",
                type: "int",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_LoaiSanPham",
                table: "LoaiSanPham",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DiscountRules_LoaiSanPham_LoaiSanPhamId",
                table: "DiscountRules",
                column: "LoaiSanPhamId",
                principalTable: "LoaiSanPham",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Menus_LoaiSanPham_LoaiSanPhamId",
                table: "Menus",
                column: "LoaiSanPhamId",
                principalTable: "LoaiSanPham",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductVariantAttributes_AttributeValue_AttributeValueId",
                table: "ProductVariantAttributes",
                column: "AttributeValueId",
                principalTable: "AttributeValue",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SanPham_LoaiSanPham_IdLoaiSanPham",
                table: "SanPham",
                column: "IdLoaiSanPham",
                principalTable: "LoaiSanPham",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DiscountRules_LoaiSanPham_LoaiSanPhamId",
                table: "DiscountRules");

            migrationBuilder.DropForeignKey(
                name: "FK_Menus_LoaiSanPham_LoaiSanPhamId",
                table: "Menus");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductVariantAttributes_AttributeValue_AttributeValueId",
                table: "ProductVariantAttributes");

            migrationBuilder.DropForeignKey(
                name: "FK_SanPham_LoaiSanPham_IdLoaiSanPham",
                table: "SanPham");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LoaiSanPham",
                table: "LoaiSanPham");

            migrationBuilder.DropColumn(
                name: "IdAttributeValue",
                table: "ProductVariantAttributes");

            migrationBuilder.RenameTable(
                name: "LoaiSanPham",
                newName: "LoaiSanPhams");

            migrationBuilder.RenameColumn(
                name: "IdLoaiSanPham",
                table: "SanPham",
                newName: "MaLSP");

            migrationBuilder.RenameIndex(
                name: "IX_SanPham_IdLoaiSanPham",
                table: "SanPham",
                newName: "IX_SanPham_MaLSP");

            migrationBuilder.RenameColumn(
                name: "AttributeValueId",
                table: "ProductVariantAttributes",
                newName: "AttributeValueID");

            migrationBuilder.RenameIndex(
                name: "IX_ProductVariantAttributes_AttributeValueId",
                table: "ProductVariantAttributes",
                newName: "IX_ProductVariantAttributes_AttributeValueID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LoaiSanPhams",
                table: "LoaiSanPhams",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DiscountRules_LoaiSanPhams_LoaiSanPhamId",
                table: "DiscountRules",
                column: "LoaiSanPhamId",
                principalTable: "LoaiSanPhams",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Menus_LoaiSanPhams_LoaiSanPhamId",
                table: "Menus",
                column: "LoaiSanPhamId",
                principalTable: "LoaiSanPhams",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductVariantAttributes_AttributeValue_AttributeValueID",
                table: "ProductVariantAttributes",
                column: "AttributeValueID",
                principalTable: "AttributeValue",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SanPham_LoaiSanPhams_MaLSP",
                table: "SanPham",
                column: "MaLSP",
                principalTable: "LoaiSanPhams",
                principalColumn: "Id");
        }
    }
}
