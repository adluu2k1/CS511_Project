USE [master];
GO

CREATE DATABASE [CS511_ChatApp];
GO

USE [CS511_ChatApp];
GO

CREATE TABLE Clients(
    Username varchar NOT NULL PRIMARY KEY,
    Pass varchar,
    DisplayName nvarchar
);

CREATE TABLE Groups(
    ID int NOT NULL PRIMARY KEY,
    DisplayName nvarchar
);

CREATE TABLE GroupClientLinks(
    ClientUsername varchar NOT NULL FOREIGN KEY REFERENCES Clients(Username),
    GroupID int NOT NULL FOREIGN KEY REFERENCES Groups(ID)
);

CREATE TABLE Messages(
    ID int NOT NULL PRIMARY KEY,
    Content ntext NOT NULL,
    GroupID int NOT NULL FOREIGN KEY REFERENCES Groups(ID),
    ClientUsername varchar NOT NULL FOREIGN KEY REFERENCES Clients(Username),
    TimeSent datetime NOT NULL
);

GO
