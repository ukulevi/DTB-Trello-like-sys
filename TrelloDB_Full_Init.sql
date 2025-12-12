USE master;
GO

-- 1. RESET DATABASE
IF EXISTS (SELECT name FROM sys.databases WHERE name = N'TrelloSys')
BEGIN
    ALTER DATABASE TrelloSys SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE TrelloSys;
END
GO
CREATE DATABASE TrelloSys;
GO  
USE TrelloSys;
GO

-- 2. TẠO BẢNG (CORE TABLES)
CREATE TABLE [User] (
    UserID INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    FirstName NVARCHAR(50) NOT NULL, LastName NVARCHAR(50) NOT NULL,
    Email NVARCHAR(255) NOT NULL UNIQUE,
    UserType CHAR(5) NOT NULL
);

CREATE TABLE Normal_User (
    UserID INT NOT NULL PRIMARY KEY,
    PasswordHash VARBINARY(64) NOT NULL,
    EmailVerified BIT DEFAULT 1,
    FOREIGN KEY (UserID) REFERENCES [User](UserID)
);

CREATE TABLE Workspace (
    WorkspaceID INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL, Description NVARCHAR(500) NULL, 
    CreatedBy INT NOT NULL, Visibility NVARCHAR(20) DEFAULT 'public',
    CreatedDate DATETIME2(0) DEFAULT SYSUTCDATETIME() -- Đã thêm
);

CREATE TABLE Board (
    BoardID INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    WorkspaceID INT NOT NULL, Name NVARCHAR(100) NOT NULL, Description NVARCHAR(500) NULL,
    IsClosed BIT DEFAULT 0, CreatedBy INT NOT NULL, Visibility NVARCHAR(20) DEFAULT 'workspace',
    CreatedDate DATETIME2(0) DEFAULT SYSUTCDATETIME() -- Đã thêm
);

CREATE TABLE Board_Member (
    BoardID INT NOT NULL, UserID INT NOT NULL, Permission NVARCHAR(20) DEFAULT 'view', IsActive BIT DEFAULT 1,
    PRIMARY KEY (BoardID, UserID)
);

CREATE TABLE List (
    ListID INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    BoardID INT NOT NULL, Title NVARCHAR(100) NOT NULL, Position INT NOT NULL
);

CREATE TABLE Card (
    CardID INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    ListID INT NOT NULL, Title NVARCHAR(255) NOT NULL, 
    Description NVARCHAR(MAX) NULL, Position INT NOT NULL,
    Priority NVARCHAR(20) DEFAULT 'medium', 
    DueDate DATETIME2(0) NULL, IsCompleted BIT DEFAULT 0,
    CreatedBy INT NOT NULL, 
    CreatedDate DATETIME2(0) DEFAULT SYSUTCDATETIME(), -- [QUAN TRỌNG] Đã thêm cột này để fix lỗi
    LastModified DATETIME2(0) DEFAULT SYSUTCDATETIME()
);

CREATE TABLE Card_Member (
    CardID INT NOT NULL, UserID INT NOT NULL, IsActive BIT DEFAULT 1,
    PRIMARY KEY (CardID, UserID)
);

CREATE TABLE Checklist (ChecklistID INT IDENTITY(1,1) PRIMARY KEY, CardID INT NOT NULL, Title NVARCHAR(100));
CREATE TABLE Checklist_Item (ItemID INT IDENTITY(1,1) PRIMARY KEY, ChecklistID INT, IsCompleted BIT DEFAULT 0);

-- 3. NẠP DỮ LIỆU MẪU
PRINT 'Inserting Sample Data...';

-- 3.1 Users
INSERT INTO [User] (FirstName, LastName, Email, UserType) VALUES 
(N'Admin', N'System', 'admin@trello.com', 'NORM'),
(N'John', N'Doe', 'john.doe@example.com', 'NORM'),
(N'Jane', N'Smith', 'jane.smith@example.com', 'NORM');

INSERT INTO Normal_User (UserID, PasswordHash, EmailVerified) VALUES 
(1, 0x123456, 1), (2, 0x123456, 1), (3, 0x123456, 1);

-- 3.2 Workspace & Board
INSERT INTO Workspace (Name, Description, CreatedBy, Visibility) VALUES 
(N'Software Engineering Project', N'Assignment 2 Group Work', 1, 'public');

INSERT INTO Board (WorkspaceID, Name, Description, CreatedBy, Visibility) VALUES 
(1, N'Trello Clone Dev', N'Development tracking board', 1, 'workspace');

INSERT INTO Board_Member (BoardID, UserID, Permission) VALUES 
(1, 1, 'admin'), (1, 2, 'edit'), (1, 3, 'view');

-- 3.3 Lists
INSERT INTO List (BoardID, Title, Position) VALUES 
(1, N'To Do', 1), (1, N'In Progress', 2), (1, N'Done', 3);

-- 3.4 Cards
INSERT INTO Card (ListID, Title, Description, Position, Priority, CreatedBy, DueDate) 
VALUES (1, N'Design Database', N'Create ERD', 1, 'high', 1, DATEADD(day, 2, GETDATE()));

INSERT INTO Card (ListID, Title, Description, Position, Priority, CreatedBy, DueDate) 
VALUES (1, N'Setup Github', N'Repo init', 2, 'medium', 2, DATEADD(day, 1, GETDATE()));

INSERT INTO Card (ListID, Title, Description, Position, Priority, CreatedBy, DueDate) 
VALUES (2, N'Implement Login', N'Login UI', 1, 'high', 1, DATEADD(day, 3, GETDATE()));

PRINT 'Sample Data Inserted Successfully!';
GO

-- 4. PROCEDURES 1
GO
CREATE OR ALTER PROCEDURE sp_InsertCard
    @ListID INT, @Title NVARCHAR(255), @Description NVARCHAR(MAX)=NULL, 
    @Priority NVARCHAR(20), @CreatedBy INT, @DueDate DATETIME2(0)=NULL
AS BEGIN
    IF LEN(ISNULL(@Title,''))=0 THROW 51000, N'Title cannot be empty', 1;
    IF @DueDate < GETDATE() THROW 51000, N'DueDate must be future', 1;
    INSERT INTO Card (ListID, Title, Description, Priority, CreatedBy, DueDate, Position)
    VALUES (@ListID, @Title, @Description, @Priority, @CreatedBy, @DueDate, 1);
END;
GO

CREATE OR ALTER PROCEDURE sp_UpdateCard
    @CardID INT, @Title NVARCHAR(255), @Description NVARCHAR(MAX)=NULL,
    @Priority NVARCHAR(20), @DueDate DATETIME2(0)=NULL, @IsCompleted BIT
AS UPDATE Card SET Title=@Title, Description=@Description, Priority=@Priority, DueDate=@DueDate, IsCompleted=@IsCompleted WHERE CardID=@CardID;
GO

CREATE OR ALTER PROCEDURE sp_DeleteCard @CardID INT AS BEGIN
    IF EXISTS(SELECT 1 FROM Card WHERE CardID=@CardID AND IsCompleted=1)
        THROW 51000, N'Cannot delete completed card', 1;
    DELETE FROM Card WHERE CardID=@CardID;
END;
GO

-- 5. PROCEDURES 2
CREATE OR ALTER PROCEDURE sp_GetBoardData @BoardID INT, @SearchKeyword NVARCHAR(100)=NULL, @SortBy NVARCHAR(20)='Priority'
AS SELECT c.*, l.Title as ListName FROM Card c JOIN List l ON c.ListID=l.ListID 
WHERE l.BoardID=@BoardID AND (@SearchKeyword IS NULL OR c.Title LIKE '%'+@SearchKeyword+'%')
ORDER BY CASE WHEN @SortBy='Priority' THEN c.Priority END ASC, l.Position;
GO

-- 6. PROCEDURES 3
CREATE OR ALTER FUNCTION fn_GetOverdueDays(@DueDate DATETIME2(0)) RETURNS INT AS BEGIN
    IF @DueDate IS NULL RETURN 0;
    DECLARE @d INT = DATEDIFF(DAY, @DueDate, GETDATE());
    RETURN CASE WHEN @d < 0 THEN 0 ELSE @d END;
END;
GO

CREATE OR ALTER PROCEDURE sp_ProjectStatistics @BoardID INT AS BEGIN
    SELECT u.UserID,
        u.FirstName + ' ' + u.LastName as FullName,
        u.Email AS Email,
        COUNT(c.CardID) AS TotalCards,
        SUM(CASE WHEN c.IsCompleted = 1 THEN 1 ELSE 0 END) AS CompletedCards,
        SUM(CASE WHEN c.IsCompleted = 0 THEN 1 ELSE 0 END) AS PendingCards,
        SUM(CASE WHEN c.DueDate < GETDATE() AND c.IsCompleted = 0 THEN 1 ELSE 0 END) AS OverdueCards,
        ISNULL(AVG(DATEDIFF(DAY, c.CreatedDate, c.LastModified)), 0) AS AvgCompletionDays,
        MAX(c.CreatedDate) AS LastCardCreated,
        CASE WHEN COUNT(c.CardID) = 0 THEN 0 
        ELSE (CAST(SUM(CASE WHEN c.IsCompleted = 1 THEN 1 ELSE 0 END) AS FLOAT) / COUNT(c.CardID)) * 100 
        END AS CompletionRate
    FROM [User] u
    JOIN Board_Member bm ON u.UserID = bm.UserID
    LEFT JOIN Card_Member cm ON u.UserID = cm.UserID AND cm.IsActive = 1
    LEFT JOIN Card c ON cm.CardID = c.CardID AND c.ListID IN (SELECT ListID FROM List WHERE BoardID = @BoardID)
    WHERE bm.BoardID = @BoardID
    GROUP BY u.UserID, u.FirstName, u.LastName, u.Email
    ORDER BY TotalCards DESC;
END;
GO

CREATE OR ALTER PROCEDURE sp_GetOverdueCards @BoardID INT AS
SELECT c.CardID, c.Title, c.Description, c.DueDate, c.Priority, l.Title AS ListName, dbo.fn_GetOverdueDays(c.DueDate) as OverdueDays, 0 AS ProgressPercent, ISNULL(u.FirstName + ' ' + u.LastName, 'Unassigned') AS AssignedUsers
FROM Card c JOIN List l ON c.ListID=l.ListID 
WHERE l.BoardID=@BoardID AND c.IsCompleted=0 AND c.DueDate < GETDATE();
GO