CREATE DATABASE IF NOT EXISTS blazorcrm;
USE blazorcrm;

CREATE TABLE CustomerTypes (
    Id INT PRIMARY KEY AUTO_INCREMENT,
    Name VARCHAR(100) NOT NULL,
    Description TEXT
);

CREATE TABLE Customers (
    Id INT PRIMARY KEY AUTO_INCREMENT,
    Name VARCHAR(100) NOT NULL,
    Email VARCHAR(100),
    Phone VARCHAR(20),
    CustomerTypeId INT,
    FOREIGN KEY (CustomerTypeId) REFERENCES CustomerTypes(Id)
);

CREATE TABLE ProductGroups (
    Id INT PRIMARY KEY AUTO_INCREMENT,
    Name VARCHAR(100) NOT NULL,
    Description TEXT
);

CREATE TABLE Products (
    Id INT PRIMARY KEY AUTO_INCREMENT,
    Name VARCHAR(100) NOT NULL,
    Description TEXT,
    ProductGroupId INT,
    Cost DECIMAL(10,2) NOT NULL,
    SalePrice DECIMAL(10,2) NOT NULL,
    FOREIGN KEY (ProductGroupId) REFERENCES ProductGroups(Id)
);

CREATE TABLE Orders (
    Id INT PRIMARY KEY AUTO_INCREMENT,
    CustomerId INT,
    OrderDate DATETIME NOT NULL,
    TotalAmount DECIMAL(10,2) NOT NULL,
    FOREIGN KEY (CustomerId) REFERENCES Customers(Id)
);

CREATE TABLE OrderItems (
    Id INT PRIMARY KEY AUTO_INCREMENT,
    OrderId INT,
    ProductId INT,
    Quantity INT NOT NULL,
    UnitPrice DECIMAL(10,2) NOT NULL,
    TotalPrice DECIMAL(10,2) NOT NULL,
    FOREIGN KEY (OrderId) REFERENCES Orders(Id),
    FOREIGN KEY (ProductId) REFERENCES Products(Id)
);