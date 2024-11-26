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

-- Bảng Courses (Khóa học)
CREATE TABLE tblCourses (
    CourseID INT PRIMARY KEY IDENTITY(1,1),
    Title NVARCHAR(255) NOT NULL,
    Description NVARCHAR(MAX),
    Price float NOT NULL,
    TeacherID INT,   
    FOREIGN KEY (TeacherID) REFERENCES tblUsers(UserID)
);
GO

-- Bảng Enrollments (Đăng ký khóa học)
CREATE TABLE tblEnrollments (
    EnrollmentID INT PRIMARY KEY IDENTITY(1,1),
    StudentID INT,
    CourseID INT,
    EnrollmentDate DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (StudentID) REFERENCES tblUsers(UserID),
    FOREIGN KEY (CourseID) REFERENCES tblCourses(CourseID)
);
GO


-- Bảng LearningMaterials (Thông tin tài liệu học tập)
CREATE TABLE tblLearningMaterials (
    MaterialID INT PRIMARY KEY IDENTITY(1,1),
    Title NVARCHAR(255) NOT NULL,                  -- Tên tài liệu
    Description NVARCHAR(MAX),                     -- Mô tả tài liệu
    CourseID INT NOT NULL,                         -- Khóa học liên quan
    TeacherID INT NOT NULL,                        -- Giáo viên đăng tài liệu
    CreatedDate DATETIME DEFAULT GETDATE(),        -- Ngày tạo tài liệu
    FOREIGN KEY (CourseID) REFERENCES tblCourses(CourseID),
    FOREIGN KEY (TeacherID) REFERENCES tblUsers(UserID)
);
GO








------------------------- Nhập dữ liệu ------------------------------------------------------
-- Thêm dữ liệu vào bảng tblUsers
INSERT INTO tblUsers (Username, Password, Email, Role)
VALUES 
(N'Nguyễn Quản Trị', 'matkhau123', 'admin@gmail.com', 'Admin'),
(N'Mai Thị Thuý Hà', 'matkhau123', 'ha@gmail.com',  'Teacher'),
( N'Nguyễn Văn Chải', 'matkhau123', 'chai@gmail.com', 'Teacher'),
(N'Trần Minh Anh', 'matkhau123', 'anh@gmail.com',  'Student'),
( N'Nguyễn Văn Bình', 'matkhau123', 'binh@gmail.com', 'Student');
GO
-- Thêm dữ liệu mẫu vào bảng Courses
INSERT INTO tblCourses (Title, Description, Price, TeacherID)
VALUES 
(N'Lập trình C# cơ bản', N'Khóa học lập trình C# cho người mới bắt đầu', 1500000, 2),
(N'Lập trình Java', N'Khóa học lập trình Java từ cơ bản đến nâng cao', 2000000, 3),
(N'Lập trình Web với PHP', N'Xây dựng website với PHP & MySQL', 1800000, 2);
GO
INSERT INTO tblEnrollments (StudentID, CourseID, EnrollmentDate)
VALUES 
(4, 1, '2024-11-25 09:00:00'),  -- Học viên với UserID = 4 đăng ký khóa học C# cơ bản
(5, 2, '2024-11-25 10:00:00'),  -- Học viên với UserID = 5 đăng ký khóa học Java
(4, 3, '2024-11-25 11:00:00'),  -- Học viên với UserID = 4 đăng ký khóa học PHP
(5, 1, '2024-11-25 12:00:00');  -- Học viên với UserID = 5 đăng ký lại khóa học C# cơ bản
go
-- Thêm dữ liệu mẫu vào bảng LearningMaterials
INSERT INTO tblLearningMaterials (Title, Description, CourseID, TeacherID)
VALUES 
(N'Bài giảng về lập trình C#', N'Tài liệu giới thiệu về cú pháp cơ bản trong C#', 1, 2),
(N'Java OOP Concepts', N'Tài liệu về lập trình hướng đối tượng trong Java', 2, 3),
(N'Hướng dẫn sử dụng MySQL', N'Hướng dẫn các câu lệnh cơ bản trong MySQL',3, 2);
GO

-- Truy vấn kiểm tra dữ liệu
SELECT * FROM tblUsers;
SELECT * FROM tblCourses;
SELECT * FROM tblEnrollments;
SELECT * FROM tblLearningMaterials;

-- Truy vấn join để xem chi tiết khóa học và học viên
SELECT 
    c.Title AS 'Tên khóa học',
    u1.Username AS 'Giảng viên',
    u2.Username AS 'Học viên',
    e.EnrollmentDate AS 'Ngày đăng ký'
FROM tblCourses c
JOIN tblUsers u1 ON c.TeacherID = u1.UserID
JOIN tblEnrollments e ON c.CourseID = e.CourseID
JOIN tblUsers u2 ON e.StudentID = u2.UserID;

-- Truy vấn join để xem tài liệu học tập theo khóa học và giáo viên
SELECT 
    lm.Title AS 'Tên tài liệu',
    lm.Description AS 'Mô tả',
    c.Title AS 'Tên khóa học',
    u.Username AS 'Giáo viên',
    lm.CreatedDate AS 'Ngày tạo'
FROM tblLearningMaterials lm
JOIN tblCourses c ON lm.CourseID = c.CourseID
JOIN tblUsers u ON lm.TeacherID = u.UserID;
GO
-----------------------------



--------------------------------------------------- tạo rằng buộc -----------------------------------------
-- Tạo trigger để kiểm tra TeacherID phải là UserID có vai trò là 'Teacher'
CREATE TRIGGER trg_CheckTeacherRole
ON tblCourses
FOR INSERT, UPDATE
AS
BEGIN
    -- Kiểm tra nếu có TeacherID không phải là UserID có vai trò là 'Teacher'
    IF EXISTS (
        SELECT 1
        FROM inserted i
        JOIN tblUsers u ON i.TeacherID = u.UserID
        WHERE u.Role != 'Teacher'
    )
    BEGIN
        -- Nếu phát hiện TeacherID không đúng vai trò, đưa ra lỗi
        RAISERROR('TeacherID must be a UserID with the role of ''Teacher''.', 16, 1);
        ROLLBACK TRANSACTION;
    END
END;
GO

CREATE TRIGGER trg_CheckStudentRole
ON tblEnrollments
FOR INSERT, UPDATE
AS
BEGIN
    -- Kiểm tra nếu có StudentID không phải là UserID có vai trò là 'Student'
    IF EXISTS (
        SELECT 1
        FROM inserted i
        JOIN tblUsers u ON i.StudentID = u.UserID
        WHERE u.Role != 'Student'
    )
    BEGIN
        -- Nếu phát hiện StudentID không đúng vai trò, đưa ra lỗi
        RAISERROR('StudentID must be a UserID with the role of ''Student''.', 16, 1);
        ROLLBACK TRANSACTION;
    END
END;
GO
