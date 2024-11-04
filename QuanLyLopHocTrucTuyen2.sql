CREATE DATABASE QuanLyLopHocTrucTuyen2;
GO

USE QuanLyLopHocTrucTuyen2;
GO

-- Bảng Users (Tài khoản)
CREATE TABLE tblUsers (
    UserID INT PRIMARY KEY IDENTITY(1,1),
    Username NVARCHAR(50) UNIQUE ,
	Email NVARCHAR(100) UNIQUE,
    Password NVARCHAR(255) ,    
    Role NVARCHAR(10) CHECK (Role IN ('Admin', 'Teacher', 'Student'))
);
GO

------------------------------------------------------ Nhập dữ liệu ------------------------------------------------------
-- Thêm dữ liệu vào bảng tblUsers
INSERT INTO tblUsers (Username, Password, Email, Role)
VALUES 
(N'Nguyễn Quản Trị', 'matkhau123', 'admin@gmail.com', 'Admin'),
(N'Mai Thị Thuý Hà', 'matkhau123', 'ha@gmail.com',  'Teacher'),
( N'Nguyễn Văn Chải', 'matkhau123', 'chai@gmail.com', 'Teacher'),
(N'Trần Minh Anh', 'matkhau123', 'anh@gmail.com',  'Student'),
( N'Nguyễn Văn Bình', 'matkhau123', 'binh@gmail.com', 'Student');
GO

select * from tblUsers