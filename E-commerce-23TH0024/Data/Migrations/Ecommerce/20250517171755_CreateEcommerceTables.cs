using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace E_commerce_23TH0024.Data.Migrations.Ecommerce
{
    /// <inheritdoc />
    public partial class CreateEcommerceTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CustomerTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerTypeName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DeliveryMethods",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MethodName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Cost = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    DeliveryTime = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ActiveStatus = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeliveryMethods", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LoaiSanPham",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenLSP = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoaiSanPham", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProductAttributes",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    AttributeName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductAttributes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "KhachHang",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HoTen = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IdAspNetUsers = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IdCustomerType = table.Column<int>(type: "int", nullable: true),
                    SoDienThoai = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Latitude = table.Column<double>(type: "float", nullable: true),
                    Longitude = table.Column<double>(type: "float", nullable: true),
                    IdCity = table.Column<int>(type: "int", nullable: true),
                    IdDistrict = table.Column<int>(type: "int", nullable: true),
                    IdWard = table.Column<int>(type: "int", nullable: true),
                    DiaChi = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KhachHang", x => x.Id);
                    table.ForeignKey(
                        name: "FK_KhachHang_CustomerTypes_IdCustomerType",
                        column: x => x.IdCustomerType,
                        principalTable: "CustomerTypes",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ShippingRates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdDeliveryMethod = table.Column<int>(type: "int", nullable: true),
                    FromDistance = table.Column<double>(type: "float", nullable: true),
                    ToDistance = table.Column<double>(type: "float", nullable: true),
                    FixedPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PricePerKm = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IdCity = table.Column<int>(type: "int", nullable: true),
                    WeightLitmit = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    CreateAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShippingRates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ShippingRates_DeliveryMethods_IdDeliveryMethod",
                        column: x => x.IdDeliveryMethod,
                        principalTable: "DeliveryMethods",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "DiscountRules",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Discount_Type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MinTotalPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DiscountAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DiscountPercent = table.Column<int>(type: "int", nullable: false),
                    IdLoaiSanPham = table.Column<int>(type: "int", nullable: true),
                    IdCustomerType = table.Column<int>(type: "int", nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Created_at = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiscountRules", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DiscountRules_CustomerTypes_IdCustomerType",
                        column: x => x.IdCustomerType,
                        principalTable: "CustomerTypes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DiscountRules_LoaiSanPham_IdLoaiSanPham",
                        column: x => x.IdLoaiSanPham,
                        principalTable: "LoaiSanPham",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "SanPham",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdLoaiSanPham = table.Column<int>(type: "int", nullable: true),
                    TenSP = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    MoTa = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DonGia = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    DVT = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Anh = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SanPham", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SanPham_LoaiSanPham_IdLoaiSanPham",
                        column: x => x.IdLoaiSanPham,
                        principalTable: "LoaiSanPham",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AttributeValues",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdProductAttribute = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Value = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AttributeValues", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AttributeValues_ProductAttributes_IdProductAttribute",
                        column: x => x.IdProductAttribute,
                        principalTable: "ProductAttributes",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "DonHang",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NgayDatHang = table.Column<DateTime>(type: "datetime2", nullable: true),
                    NgayGiaoHang = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IdKhachHang = table.Column<int>(type: "int", nullable: true),
                    IdNhanVienGiao = table.Column<int>(type: "int", nullable: true),
                    IdNhanVienDuyet = table.Column<int>(type: "int", nullable: true),
                    TinhTrang = table.Column<int>(type: "int", nullable: true),
                    VAT = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PaymentMethod = table.Column<int>(type: "int", nullable: true),
                    ShippingFee = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IdDeliveryMethod = table.Column<int>(type: "int", nullable: true),
                    TotalProductAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IdDiscountRule = table.Column<int>(type: "int", nullable: true),
                    DiscountRuleId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DonHang", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DonHang_DeliveryMethods_IdDeliveryMethod",
                        column: x => x.IdDeliveryMethod,
                        principalTable: "DeliveryMethods",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DonHang_DiscountRules_DiscountRuleId",
                        column: x => x.DiscountRuleId,
                        principalTable: "DiscountRules",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DonHang_KhachHang_IdKhachHang",
                        column: x => x.IdKhachHang,
                        principalTable: "KhachHang",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ProductVariants",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdSanPham = table.Column<int>(type: "int", nullable: true),
                    StockQuantity = table.Column<int>(type: "int", nullable: true),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductVariants", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductVariants_SanPham_IdSanPham",
                        column: x => x.IdSanPham,
                        principalTable: "SanPham",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ChiTietDonHang",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdDonHang = table.Column<int>(type: "int", nullable: false),
                    IdSanPham = table.Column<int>(type: "int", nullable: false),
                    SoLuong = table.Column<int>(type: "int", nullable: false),
                    DonGia = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DiscountApplied = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChiTietDonHang", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChiTietDonHang_DonHang_IdDonHang",
                        column: x => x.IdDonHang,
                        principalTable: "DonHang",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChiTietDonHang_SanPham_IdSanPham",
                        column: x => x.IdSanPham,
                        principalTable: "SanPham",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductVariantAttributes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdProductAttribute = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    IdProductVariant = table.Column<int>(type: "int", nullable: true),
                    IdAttributeValue = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductVariantAttributes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductVariantAttributes_AttributeValues_IdAttributeValue",
                        column: x => x.IdAttributeValue,
                        principalTable: "AttributeValues",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ProductVariantAttributes_ProductAttributes_IdProductAttribute",
                        column: x => x.IdProductAttribute,
                        principalTable: "ProductAttributes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ProductVariantAttributes_ProductVariants_IdProductVariant",
                        column: x => x.IdProductVariant,
                        principalTable: "ProductVariants",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_AttributeValues_IdProductAttribute",
                table: "AttributeValues",
                column: "IdProductAttribute");

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietDonHang_IdDonHang",
                table: "ChiTietDonHang",
                column: "IdDonHang");

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietDonHang_IdSanPham",
                table: "ChiTietDonHang",
                column: "IdSanPham");

            migrationBuilder.CreateIndex(
                name: "IX_DiscountRules_IdCustomerType",
                table: "DiscountRules",
                column: "IdCustomerType");

            migrationBuilder.CreateIndex(
                name: "IX_DiscountRules_IdLoaiSanPham",
                table: "DiscountRules",
                column: "IdLoaiSanPham");

            migrationBuilder.CreateIndex(
                name: "IX_DonHang_DiscountRuleId",
                table: "DonHang",
                column: "DiscountRuleId");

            migrationBuilder.CreateIndex(
                name: "IX_DonHang_IdDeliveryMethod",
                table: "DonHang",
                column: "IdDeliveryMethod");

            migrationBuilder.CreateIndex(
                name: "IX_DonHang_IdKhachHang",
                table: "DonHang",
                column: "IdKhachHang");

            migrationBuilder.CreateIndex(
                name: "IX_KhachHang_IdCustomerType",
                table: "KhachHang",
                column: "IdCustomerType");

            migrationBuilder.CreateIndex(
                name: "IX_ProductVariantAttributes_IdAttributeValue",
                table: "ProductVariantAttributes",
                column: "IdAttributeValue");

            migrationBuilder.CreateIndex(
                name: "IX_ProductVariantAttributes_IdProductAttribute",
                table: "ProductVariantAttributes",
                column: "IdProductAttribute");

            migrationBuilder.CreateIndex(
                name: "IX_ProductVariantAttributes_IdProductVariant",
                table: "ProductVariantAttributes",
                column: "IdProductVariant");

            migrationBuilder.CreateIndex(
                name: "IX_ProductVariants_IdSanPham",
                table: "ProductVariants",
                column: "IdSanPham");

            migrationBuilder.CreateIndex(
                name: "IX_SanPham_IdLoaiSanPham",
                table: "SanPham",
                column: "IdLoaiSanPham");

            migrationBuilder.CreateIndex(
                name: "IX_ShippingRates_IdDeliveryMethod",
                table: "ShippingRates",
                column: "IdDeliveryMethod");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChiTietDonHang");

            migrationBuilder.DropTable(
                name: "ProductVariantAttributes");

            migrationBuilder.DropTable(
                name: "ShippingRates");

            migrationBuilder.DropTable(
                name: "DonHang");

            migrationBuilder.DropTable(
                name: "AttributeValues");

            migrationBuilder.DropTable(
                name: "ProductVariants");

            migrationBuilder.DropTable(
                name: "DeliveryMethods");

            migrationBuilder.DropTable(
                name: "DiscountRules");

            migrationBuilder.DropTable(
                name: "KhachHang");

            migrationBuilder.DropTable(
                name: "ProductAttributes");

            migrationBuilder.DropTable(
                name: "SanPham");

            migrationBuilder.DropTable(
                name: "CustomerTypes");

            migrationBuilder.DropTable(
                name: "LoaiSanPham");
        }
    }
}
