--use TaskAPI
---- ������� �������������
--CREATE TABLE Users (
--    Id INT IDENTITY(1,1) PRIMARY KEY,
--    Username NVARCHAR(100) NOT NULL UNIQUE,
--    Email NVARCHAR(255) NOT NULL UNIQUE,
--    PasswordHash NVARCHAR(255) NOT NULL,
--    CreatedAt DATETIME DEFAULT GETDATE()
--);

---- ������� ��������
--CREATE TABLE Projects (
--    Id INT IDENTITY(1,1) PRIMARY KEY,
--    Name NVARCHAR(200) NOT NULL,
--    Description NVARCHAR(MAX),
--    OwnerId INT NOT NULL FOREIGN KEY REFERENCES Users(Id) ON DELETE CASCADE,
--    CreatedAt DATETIME DEFAULT GETDATE()
--);

---- ������� �����
--CREATE TABLE Tasks (
--    Id INT IDENTITY(1,1) PRIMARY KEY,
--    Title NVARCHAR(200) NOT NULL,
--    Description NVARCHAR(MAX),
--    StatusId INT NOT NULL FOREIGN KEY REFERENCES TaskStatuses(Id),
--    ProjectId INT NOT NULL FOREIGN KEY REFERENCES Projects(Id) ON DELETE CASCADE,
--    AssignedToId INT FOREIGN KEY REFERENCES Users(Id) ON DELETE NO ACTION,
--    Deadline DATETIME,
--    CreatedAt DATETIME DEFAULT GETDATE(),
--    UpdatedAt DATETIME DEFAULT GETDATE()
--);

---- ������� �������� �����
--CREATE TABLE TaskStatuses (
--    Id INT IDENTITY(1,1) PRIMARY KEY,
--    Name NVARCHAR(50) NOT NULL UNIQUE
--);

---- �������������� ������� �������� �����
--INSERT INTO TaskStatuses (Name) VALUES
--('New'),
--('In Progress'),
--('Done'),
--('On Hold');
--select * from TaskStatuses
-- ������� ����� � �������
--CREATE LOGIN AppUser WITH PASSWORD = 'StrongPassword123!';

---- ������� ������������ � ���������� ���� ������
--USE TaskApi;
--CREATE USER AppUser FOR LOGIN AppUser;

---- ��������� ����� ������������
--ALTER ROLE db_datareader ADD MEMBER AppUser; -- ����� �� ������
--ALTER ROLE db_datawriter ADD MEMBER AppUser; -- ����� �� ������
--USE TaskAPI;
--SELECT name FROM sys.database_principals WHERE type = 'S';
--SELECT name, is_disabled FROM sys.server_principals WHERE name = 'AppUser';
--ALTER LOGIN AppUser ENABLE;




--SELECT name, type_desc 
--FROM sys.database_principals 
--WHERE name = 'AppUser';
--CREATE USER AppUser FOR LOGIN AppUser;


--USE TaskAPI;
--EXEC sp_helpuser;


--SELECT name, default_database_name 
--FROM sys.server_principals 
--WHERE name = 'AppUser';

--ALTER LOGIN AppUser WITH DEFAULT_DATABASE = TaskAPI;
--drop user AppUser;
--ALTER LOGIN [AppUser]
--WITH PASSWORD = '123';
--GO
--ALTER LOGIN [AppUser] ENABLE;
--GO
select * from dbo.Users
select * from dbo.Projects
select * from dbo.TaskStatuses
select * from dbo.Tasks