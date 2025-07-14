-- tạo dữ liệu cho bảng LoaiSanPham
INSERT INTO [dbo].[LoaiSanPham] ([TenLSP])
VALUES 
    (N'Thiệp cưới cao cấp'),
    (N'Thiệp cưới giá rẻ'),
    (N'Thiệp sinh nhật'),
    (N'Thiệp chúc mừng'),
    (N'Thiệp handmade');

-- tạo dữ liệu cho bảng SanPham
INSERT INTO [dbo].[SanPham] (
[IdLoaiSanPham],
[TenSP],
[MoTa],
[DonGia],
[DVT],
[Anh]
)
VALUES
(1, N'HL01', N'Mô tả sản phẩm HL01', 100000, N'Cái', N'HL01.jpg'),
(1, N'HL02', N'Mô tả sản phẩm HL02', 101000, N'Cái', N'HL02.jpg'),
(1, N'HL03', N'Mô tả sản phẩm HL03', 102000, N'Cái', N'HL03.jpg'),
(1, N'HL04', N'Mô tả sản phẩm HL04', 103000, N'Cái', N'HL04.jpg'),
(1, N'HL05', N'Mô tả sản phẩm HL05', 104000, N'Cái', N'HL05.jpg'),
(2, N'HL06', N'Mô tả sản phẩm HL06', 105000, N'Cái', N'HL06.jpg'),
(2, N'HL07', N'Mô tả sản phẩm HL07', 106000, N'Cái', N'HL07.jpg'),
(2, N'HL08', N'Mô tả sản phẩm HL08', 107000, N'Cái', N'HL08.jpg'),
(2, N'HL09', N'Mô tả sản phẩm HL09', 108000, N'Cái', N'HL09.jpg'),
(2, N'HL10', N'Mô tả sản phẩm HL10', 109000, N'Cái', N'HL10.jpg'),
(3, N'HL11', N'Mô tả sản phẩm HL11', 110000, N'Cái', N'HL11.jpg'),
(3, N'HL12', N'Mô tả sản phẩm HL12', 111000, N'Cái', N'HL12.jpg'),
(3, N'HL13', N'Mô tả sản phẩm HL13', 112000, N'Cái', N'HL13.jpg'),
(3, N'HL14', N'Mô tả sản phẩm HL14', 113000, N'Cái', N'HL14.jpg'),
(3, N'HL15', N'Mô tả sản phẩm HL15', 114000, N'Cái', N'HL15.jpg'),
(4, N'HL16', N'Mô tả sản phẩm HL16', 115000, N'Cái', N'HL16.jpg'),
(4, N'HL17', N'Mô tả sản phẩm HL17', 116000, N'Cái', N'HL17.jpg'),
(4, N'HL18', N'Mô tả sản phẩm HL18', 117000, N'Cái', N'HL18.jpg'),
(4, N'HL19', N'Mô tả sản phẩm HL19', 118000, N'Cái', N'HL19.jpg'),
(4, N'HL20', N'Mô tả sản phẩm HL20', 119000, N'Cái', N'HL20.jpg');
