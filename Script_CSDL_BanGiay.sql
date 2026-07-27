CREATE DATABASE QL_BanGiay;
GO

USE QL_BanGiay;
GO

-- 1. Bảng Danh mục sản phẩm (Giày Nam, Giày Nữ, Giày Thể Thao, ...)
CREATE TABLE DanhMuc (
    MaDanhMuc INT IDENTITY(1,1) PRIMARY KEY,
    TenDanhMuc NVARCHAR(100) NOT NULL
);

-- 2. Bảng Sản phẩm Giày
CREATE TABLE SanPham (
    MaGiay INT IDENTITY(1,1) PRIMARY KEY,
    TenGiay NVARCHAR(200) NOT NULL,
    GiaGoc DECIMAL(18,2) NOT NULL,
    GiaKhuyenMai DECIMAL(18,2) NULL,
    HinhAnh NVARCHAR(500) NULL,
    MoTa NVARCHAR(MAX) NULL,
    MaDanhMuc INT FOREIGN KEY REFERENCES DanhMuc(MaDanhMuc) ON DELETE CASCADE
);

-- 3. Bảng Size Giày (38, 39, 40, 41, 42, 43...)
CREATE TABLE SizeGiay (
    MaSize INT IDENTITY(1,1) PRIMARY KEY,
    TenSize NVARCHAR(10) NOT NULL
);

-- 4. Bảng Chi tiết Giày (Quản lý số lượng tồn kho theo Size)
CREATE TABLE ChiTietGiay (
    MaChiTiet INT IDENTITY(1,1) PRIMARY KEY,
    MaGiay INT FOREIGN KEY REFERENCES SanPham(MaGiay) ON DELETE CASCADE,
    MaSize INT FOREIGN KEY REFERENCES SizeGiay(MaSize) ON DELETE CASCADE,
    SoLuongTon INT DEFAULT 0
);

-- 5. Bảng Tài khoản / Người dùng
CREATE TABLE TaiKhoan (
    MaTaiKhoan INT IDENTITY(1,1) PRIMARY KEY,
    TenDangNhap VARCHAR(50) UNIQUE NOT NULL,
    MatKhau VARCHAR(255) NOT NULL,
    HoTen NVARCHAR(100) NOT NULL,
    Email VARCHAR(100) NULL,
    SoDienThoai VARCHAR(15) NULL,
    VaiTro NVARCHAR(20) DEFAULT N'KhachHang'
);

-- 6. Bảng Đơn hàng
CREATE TABLE DonHang (
    MaDonHang INT IDENTITY(1,1) PRIMARY KEY,
    MaTaiKhoan INT FOREIGN KEY REFERENCES TaiKhoan(MaTaiKhoan),
    NgayDat DATETIME DEFAULT GETDATE(),
    TongTien DECIMAL(18,2) NOT NULL,
    TrangThai NVARCHAR(50) DEFAULT N'Đang xử lý',
    DiaChiGiaoHang NVARCHAR(255) NOT NULL,
    SoDienThoaiNhan VARCHAR(15) NOT NULL
);

-- 7. Bảng Chi tiết Đơn hàng
CREATE TABLE ChiTietDonHang (
    MaChiTietDH INT IDENTITY(1,1) PRIMARY KEY,
    MaDonHang INT FOREIGN KEY REFERENCES DonHang(MaDonHang) ON DELETE CASCADE,
    MaGiay INT FOREIGN KEY REFERENCES SanPham(MaGiay),
    TenSize NVARCHAR(10) NOT NULL,
    SoLuong INT NOT NULL,
    DonGia DECIMAL(18,2) NOT NULL
);

-- CHÈN DỮ LIỆU MẪU BAN ĐẦU
INSERT INTO DanhMuc (TenDanhMuc) VALUES 
(N'Giày Sneaker'), (N'Giày Chạy Bộ'), (N'Giày Tây / Công Sở'), (N'Sandal & Dép');

INSERT INTO SizeGiay (TenSize) VALUES ('38'), ('39'), ('40'), ('41'), ('42'), ('43');

INSERT INTO TaiKhoan (TenDangNhap, MatKhau, HoTen, Email, VaiTro) VALUES 
('admin', '123456', N'Quản Trị Viên', 'admin@bangiay.com', 'Admin'),
('khachhang', '123456', N'Trương Thị Cả Em', 'emttc110386@tvu-onschool.edu.vn', 'KhachHang');