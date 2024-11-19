/* 
Run This Script On: 
CBSE
*/
--HEADER, DO NOT MODIFY
SET NUMERIC_ROUNDABORT OFF;
GO

SET ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, ARITHABORT, QUOTED_IDENTIFIER, ANSI_NULLS ON;
GO

SET XACT_ABORT ON;
GO

SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;
GO

BEGIN TRANSACTION T1;
GO

-----------------------------------------------
SET ANSI_NULLS ON;
GO

SET QUOTED_IDENTIFIER ON;
GO
---------------------------------------------------------------
    
-- Create School Table if it doesn't exist
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Schools]') AND type in (N'U'))
BEGIN
    CREATE TABLE Schools (
        SchoolId INT PRIMARY KEY IDENTITY(1,1),  
        SchoolName NVARCHAR(255) NOT NULL        
    );
END

-- Create Student Table if it doesn't exist
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Students]') AND type in (N'U'))
BEGIN
    CREATE TABLE Students (
        StudentId INT PRIMARY KEY IDENTITY(1,1),  
        Name NVARCHAR(255) NOT NULL,              
        Age INT NOT NULL,                         
        SchoolId INT NOT NULL,                    
        RollNo VARCHAR(50) NOT NULL,
        FOREIGN KEY (SchoolId) REFERENCES Schools(SchoolId)  
    );
END

-- Create Marks Table if it doesn't exist
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Marks]') AND type in (N'U'))
BEGIN
    CREATE TABLE Marks (
        MarksId INT PRIMARY KEY IDENTITY(1,1),    
        StudentId INT NOT NULL,                    
        Math DECIMAL(5,2) NOT NULL,                
        Science DECIMAL(5,2) NOT NULL,             
        English DECIMAL(5,2) NOT NULL,             
        History DECIMAL(5,2) NOT NULL,             
        Geography DECIMAL(5,2) NOT NULL,           
        FOREIGN KEY (StudentId) REFERENCES Students(StudentId)  
    );
END

-- Create User Table if it doesn't exist
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Users]') AND type in (N'U'))
BEGIN
    CREATE TABLE Users (
        Id INT PRIMARY KEY IDENTITY(1,1),         
        Username NVARCHAR(255) NOT NULL,          
        Password NVARCHAR(255) NOT NULL,          
        SchoolId INT NOT NULL,                    
        FOREIGN KEY (SchoolId) REFERENCES Schools(SchoolId)  
    );
END





------------------------------------------------------------
--FOOTER, DO NOT MODIFY
IF @@ERROR <> 0
BEGIN
    PRINT 'The database update failed';
    PRINT 'ROLLING BACK CHANGES';

    ROLLBACK TRANSACTION T1;
END;
ELSE
BEGIN
    PRINT 'The database update succeeded';

    COMMIT TRANSACTION T1;

    PRINT 'EXECUTE VERSIONING SP';

------------------------------------------------------------
END;
GO