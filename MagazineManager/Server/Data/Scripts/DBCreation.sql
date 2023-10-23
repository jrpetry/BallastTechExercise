DECLARE @dbname nvarchar(128)
SET @dbname = N'BallastExercise'

IF (NOT EXISTS (SELECT name FROM master.sys.databases WHERE ('[' + name + ']' = @dbname OR name = @dbname)))
BEGIN
    CREATE DATABASE BallastExercise ON PRIMARY 
    (
        NAME = BallastExercise_Data, 
        FILENAME = 'C:\ExerciseDB\MyDatabaseData.mdf', 
        SIZE = 2MB, 
        MAXSIZE = 10MB, 
        FILEGROWTH = 10%
    )
    LOG ON 
    (
        NAME = BallastExercise_Log,  
        FILENAME = 'C:\ExerciseDB\MyDatabaseLog.ldf', 
        SIZE = 1MB, 
        MAXSIZE = 5MB, 
        FILEGROWTH = 10%
    );
END;
GO

Use BallastExercise;

if not exists(select * from sysobjects where type = 'U' and name = 'APPLICATION_USERS') 
begin 
     
    CREATE TABLE [APPLICATION_USERS]
    (
	    [Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY CLUSTERED,
        [UserName] VARCHAR(30) NOT NULL,
        [Pwd] VARBINARY(max)  NOT NULL, 
        [Role] VARCHAR(20) NOT NULL
    )
end
GO

if not exists(select 1 from sysobjects where type = 'U' and name = 'MAGAZINES') 
begin 
     
    CREATE TABLE [MAGAZINES]
    (
	    [Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY CLUSTERED,
        [Name] VARCHAR(50) NOT NULL, 
        [ReleaseDate] DATETIME NOT NULL, 
        [ApplicationUserId] INT NOT NULL,
        FOREIGN KEY (ApplicationUserId) REFERENCES APPLICATION_USERS(Id)
    )
end
GO