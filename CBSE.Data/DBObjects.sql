-- Create School Table
CREATE TABLE Schools (
    SchoolId INT PRIMARY KEY IDENTITY(1,1),  
    SchoolName NVARCHAR(255) NOT NULL        
);

-- Create Student Table
CREATE TABLE Students (
    StudentId INT PRIMARY KEY IDENTITY(1,1),  
    Name NVARCHAR(255) NOT NULL,              
    Age INT NOT NULL,                         
    SchoolId INT NOT NULL,                    
    FOREIGN KEY (SchoolId) REFERENCES Schools(SchoolId)  
);

-- Create Marks Table
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

-- Create User Table
CREATE TABLE Users (
    Id INT PRIMARY KEY IDENTITY(1,1),         
    Username NVARCHAR(255) NOT NULL,          
    Password NVARCHAR(255) NOT NULL,          
    SchoolId INT NOT NULL,                    
    FOREIGN KEY (SchoolId) REFERENCES Schools(SchoolId)  
);
