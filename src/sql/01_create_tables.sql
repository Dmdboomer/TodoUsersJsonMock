-- Create Users table
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Users' AND xtype='U')
CREATE TABLE Users (
    Id INT PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL,
    Username NVARCHAR(50) NOT NULL,
    Email NVARCHAR(100) NOT NULL,
    Phone NVARCHAR(30),
    Website NVARCHAR(100),
    CompanyName NVARCHAR(100),
    City NVARCHAR(50)
);

-- Create Todos table
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Todos' AND xtype='U')
CREATE TABLE Todos (
    Id INT PRIMARY KEY,
    UserId INT NOT NULL,
    Title NVARCHAR(500) NOT NULL,
    Completed BIT NOT NULL,
    FOREIGN KEY (UserId) REFERENCES Users(Id)
);