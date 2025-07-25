CREATE TABLE Users (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Username NVARCHAR(50) NOT NULL,
    PasswordHash NVARCHAR(1000) NOT NULL,
    FirstName NVARCHAR(50) NOT NULL,
    LastName NVARCHAR(50) NOT NULL,
    Address NVARCHAR(255) NOT NULL,
    CreatedAt DATETIME NOT NULL DEFAULT GETDATE()
);

CREATE TABLE CarrierServices (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(100) NOT NULL,
    Description NVARCHAR(255),
    Price DECIMAL(10, 2) NOT NULL
);

CREATE TABLE Carriers (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(255) NOT NULL,
    Email NVARCHAR(100) NOT NULL,
    PhoneNumber NVARCHAR(20) NOT NULL,
    IsActive BIT NOT NULL DEFAULT 1
);

CREATE TABLE CarrierSupportedServices (
    CarrierId INT NOT NULL,
    ServiceId INT NOT NULL,
    PRIMARY KEY (CarrierId, ServiceId),
    FOREIGN KEY (CarrierId) REFERENCES Carriers(Id),
    FOREIGN KEY (ServiceId) REFERENCES CarrierServices(Id)
);

CREATE TABLE PackageStatuses (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(100) NOT NULL
);

CREATE TABLE Packages (
    Id INT PRIMARY KEY IDENTITY(1,1),
    TrackingNumber NVARCHAR(50) NOT NULL UNIQUE,
    StatusId INT NOT NULL,
    Weight DECIMAL(10, 2),
    Length DECIMAL(10, 2),
    Width DECIMAL(10, 2),
    Height DECIMAL(10, 2),
    RecipientAddress NVARCHAR(255) NOT NULL,
    SenderAddress NVARCHAR(255) NOT NULL,
    CarrierId INT NOT NULL,
    UserId INT NOT NULL,
    ServiceType INT NOT NULL,
    CreatedAt DATETIME NOT NULL DEFAULT GETDATE(),
    UpdatedAt DATETIME,

    FOREIGN KEY (ServiceType) REFERENCES CarrierServices(Id),
    FOREIGN KEY (CarrierId) REFERENCES Carriers(Id),
    FOREIGN KEY (UserId) REFERENCES Users(Id),
    FOREIGN KEY (StatusId) REFERENCES PackageStatuses(Id)
);

CREATE TABLE PackageStatusHistory (
    Id INT PRIMARY KEY IDENTITY(1,1),
    PackageId INT NOT NULL,
    StatusId INT NOT NULL,
    Timestamp DATETIME NOT NULL DEFAULT GETDATE(),
    Notes NVARCHAR(1000),

    FOREIGN KEY (PackageId) REFERENCES Packages(Id),
    FOREIGN KEY (StatusId) REFERENCES PackageStatuses(Id)
);
