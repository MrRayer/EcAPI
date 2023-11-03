using Dapper;
using System.Data.SqlClient;

Console.WriteLine("Enter Connection String: ");
string? connString = Console.ReadLine();
string? connStringUsers = connString + ";initial catalog=ECUserDB";
string? connStringProducts = connString + ";initial catalog=ECProductsDB";
string? connStringPurchases = connString + ";initial catalog=ECPurchasesDB";
if (connString is not null)
{
    Console.WriteLine("Creating DataBases...");
    using (SqlConnection conn = new SqlConnection(connString))
    {
        conn.Query("IF NOT EXISTS(SELECT * FROM sys.databases WHERE name = 'ECUserDB') BEGIN CREATE DATABASE ECUserDB END");
        Console.WriteLine("ECUserDB Created.");
        conn.Query("IF NOT EXISTS(SELECT * FROM sys.databases WHERE name = 'ECProductsDB') BEGIN CREATE DATABASE ECProductsDB END");
        Console.WriteLine("ECProductsDB Created.");
        conn.Query("IF NOT EXISTS(SELECT * FROM sys.databases WHERE name = 'ECPurchasesDB') BEGIN CREATE DATABASE ECPurchasesDB END");
        Console.WriteLine("ECPurchasesDB Created.");
    }
    Console.WriteLine("Creating Table Users...");
    using (SqlConnection conn = new SqlConnection(connStringUsers))
    {
        conn.Query("IF OBJECT_ID (N'dbo.Users', N'U') IS NULL BEGIN CREATE TABLE Users(Id int PRIMARY KEY IDENTITY(1,1), Username varchar(50) UNIQUE NOT NULL, Password varchar(255) NOT NULL, Email varchar(100) NOT NULL, Address varchar(100) NOT NULL) END");
        Console.WriteLine("Users table created.");
        Console.WriteLine("Creating Stored Procedures...");
        conn.Query("IF OBJECT_ID ('dbo.addUser') IS NULL BEGIN EXEC('CREATE PROCEDURE addUser @Username varchar(50), @Password varchar(255), @Email varchar(100), @Address varchar(100) AS BEGIN INSERT INTO Users (Username,Password,Email,Address) VALUES (@Username,@Password,@Email,@Address) SELECT \"true\" END') END");
        Console.WriteLine("addUser SP created.");
        conn.Query("IF OBJECT_ID ('dbo.DeleteUser') IS NULL\r\nBEGIN\r\nEXEC('CREATE PROCEDURE DeleteUser @Username varchar(50), @Password varchar(255)\r\nAS\r\nBEGIN\r\n\tIF @Password = (SELECT Password FROM Users WHERE Username = @Username)\r\n\t\tBEGIN\r\n\t\tDELETE FROM Users WHERE Username = @Username\r\n\t\tSELECT \"true\"\r\n\t\tEND\r\n\tELSE\r\n\t\tSELECT \"false\"\r\nEND')\r\nEND");
        Console.WriteLine("DeleteUser SP created.");
        conn.Query("IF OBJECT_ID ('dbo.GetUser') IS NULL\r\nBEGIN\r\nEXEC('CREATE PROCEDURE GetUser(\r\n\t@Id int,\r\n\t@Username varchar(50))\r\nAS\r\nBEGIN\r\n\tIF @Id IS NOT NULL\r\n\t\tSELECT * FROM Users WHERE Id = @Id\r\n\tELSE IF @Username IS NOT NULL\r\n\t\tSELECT * FROM Users WHERE Username = @Username\r\nEND')\r\nEND");
        Console.WriteLine("GetUser created.");
        conn.Query("IF OBJECT_ID ('dbo.updateUser') IS NULL\r\nBEGIN\r\nEXEC('CREATE PROCEDURE updateUser(\r\n\t@Username varchar(50),\r\n\t@Password varchar(255),\r\n\t@UpdatedUsername varchar(50),\r\n\t@UpdatedPassword varchar(255),\r\n\t@UpdatedEmail varchar(100),\r\n\t@UpdatedAddress varchar(100))\r\nAS\r\nBEGIN\r\n\tDECLARE @Id int = -1\r\n\tIF @Password = (SELECT Password FROM Users WHERE Username = @Username)\r\n\t\tSELECT @Id = (SELECT Id FROM Users WHERE Username = @Username)\r\n\tIF (@Id <> -1)\r\n\t\tBEGIN\r\n\t\tIF (@UpdatedUsername <> '')\r\n\t\t\tUPDATE Users SET Username = @UpdatedUsername WHERE Id = @Id\r\n\t\tIF (@UpdatedPassword <> '')\r\n\t\t\tUPDATE Users SET Password = @UpdatedPassword WHERE Id = @Id\r\n\t\tIF (@UpdatedEmail <> '')\r\n\t\t\tUPDATE Users SET Email = @UpdatedEmail WHERE Id = @Id\r\n\t\tIF (@UpdatedAddress <> '')\r\n\t\t\tUPDATE Users SET Address = @UpdatedAddress WHERE Id = @Id\r\n\t\tSELECT \"true\"\r\n\t\tEND\r\n\tELSE\r\n\t\tSELECT \"false\"\r\nEND')\r\nEND");
        Console.WriteLine("updateUser created.");
        conn.Query("IF OBJECT_ID ('dbo.validateUser') IS NULL\r\nBEGIN\r\nEXEC('CREATE PROCEDURE validateUser\r\n\t@Username varchar(50),\r\n\t@Password varchar (255)\r\nAS\r\nBEGIN\r\n\tIF @Password = (SELECT Password FROM Users WHERE Username = @Username)\r\n\t\tSELECT \"true\"\r\n\tELSE SELECT \"false\"\r\nEND')\r\nEND");
        Console.WriteLine("validateUser created.");
        Console.WriteLine("UsersSP creation done.");
    }
    Console.WriteLine("Creating Table Products...");
    using (SqlConnection conn = new SqlConnection(connStringProducts))
    {
        conn.Query("IF OBJECT_ID (N'dbo.Products', N'U') IS NULL\r\nBEGIN\r\nEXEC('CREATE TABLE Products(\r\n\tId int IDENTITY(1,1) PRIMARY KEY,\r\n\tName varchar(255) NOT NULL,\r\n\tDescription varchar(255) NULL,\r\n\tPrice smallmoney NOT NULL)')\r\nEND");
        Console.WriteLine("Products table created.");
        Console.WriteLine("Creating Stored Procedures...");
        conn.Query("IF OBJECT_ID ('dbo.addProduct') IS NULL\r\nBEGIN\r\nEXEC('CREATE PROCEDURE [dbo].[addProduct] \r\n\t@Name varchar(255),\r\n\t@Description varchar(255),\r\n\t@Price smallmoney\r\nAS\r\nBEGIN\r\n\tINSERT INTO Products (Name,Description,Price)\r\n\tVALUES (@Name,@Description,@Price)\r\n\tSELECT \"true\"\r\nEND')\r\nEND");
        Console.WriteLine("addProduct created.");
        conn.Query("IF OBJECT_ID ('dbo.DeleteProduct') IS NULL\r\nBEGIN\r\nEXEC('CREATE PROCEDURE [dbo].[DeleteProduct]\r\n\t@Id int\r\nAS\r\nBEGIN\r\n\tDELETE FROM Products WHERE Id = @Id\r\n\tSELECT \"true\"\r\nEND')\r\nEND");
        Console.WriteLine("DeleteProduct created.");
        conn.Query("IF OBJECT_ID ('dbo.GetProducts') IS NULL\r\nBEGIN\r\nEXEC('CREATE PROCEDURE [dbo].[GetProducts]\r\nAS\r\nBEGIN\r\n\tSELECT * FROM Products\r\nEND')\r\nEND");
        Console.WriteLine("GetProducts created.");
        conn.Query("IF OBJECT_ID ('dbo.UpdateProduct') IS NULL\r\nBEGIN\r\nEXEC('CREATE PROCEDURE [dbo].[UpdateProduct]\r\n\t@Id int,\r\n\t@Name varchar(255),\r\n\t@Description varchar(255),\r\n\t@Price smallmoney\r\nAS\r\nBEGIN\r\n\tIF @Id IS NOT NULL\r\n\t\tBEGIN\r\n\t\t\tIF @Name <> '' UPDATE Products SET Name = @Name\r\n\t\t\tIF @Description <> '' UPDATE Products SET Description = @Description\r\n\t\t\tIF @Price <> '' UPDATE Products SET Price = @Price\r\n\t\t\tSELECT \"true\"\r\n\t\tEND\r\n\tELSE IF @Name IS NOT NULL\r\n\t\tBEGIN\r\n\t\t\tIF @Description <> '' UPDATE Products SET Description = @Description\r\n\t\t\tIF @Price <> '' UPDATE Products SET Price = @Price\r\n\t\t\tSELECT \"true\"\r\n\t\tEND\r\n\tELSE SELECT \"false\"\r\nEND')\r\nEND");
        Console.WriteLine("UpdateProduct created.");
        Console.WriteLine("ProductsSP creation done.");
    }
    Console.WriteLine("Creating Table Purchases...");
    using (SqlConnection conn = new SqlConnection(connStringPurchases))
    {
        conn.Query("IF OBJECT_ID (N'dbo.Purchase', N'U') IS NULL\r\nBEGIN\r\nEXEC('CREATE TABLE Purchase(\r\n\tId int IDENTITY(1,1) PRIMARY KEY,\r\n\tUserId int NOT NULL,\r\n\tPaypalToken varchar(100) NULL,\r\n\tPaymentStatus bit NOT NULL DEFAULT 0,\r\n\tStatus bit NOT NULL DEFAULT 0)')\r\nEND");
        Console.WriteLine("Purchases table created");
        conn.Query("IF OBJECT_ID (N'dbo.Deliveries', N'U') IS NULL\r\nBEGIN\r\nEXEC('CREATE TABLE Deliveries(\r\n\tId int IDENTITY(1,1) PRIMARY KEY,\r\n\tPurchaseId int NOT NULL,\r\n\tUserId int NOT NULL,\r\n\tProductId int NOT NULL,\r\n\tStatus bit NOT NULL DEFAULT 0)')\r\nEND");
        Console.WriteLine("Deliveries table created.");
        Console.WriteLine("Creating Stored Procedures...");
        conn.Query("IF OBJECT_ID ('AddDelivery') IS NULL\r\nBEGIN\r\n\tEXEC('PROCEDURE [dbo].[AddDelivery] @PurchaseId int, @UserId int, @ProductId int\r\nAS\r\nBEGIN\r\n\tINSERT INTO Deliveries(PurchaseId,UserId,ProductId)VALUES(@PurchaseId,@UserId,@ProductId)\r\n\tSELECT \"true\"\r\nEND')\r\nEND");
        Console.WriteLine("AddDelivery created.");
        conn.Query("IF OBJECT_ID ('AddPurchase') IS NULL\r\nBEGIN\r\n\tEXEC('CREATE PROCEDURE AddPurchase @UserId int, @PaypalToken varchar(100)\r\nAS\r\nBEGIN\r\n\tINSERT INTO Purchase(UserId,PaypalToken) OUTPUT INSERTED.Id VALUES(@UserId,@PaypalToken)\r\n\tSELECT \"true\"\r\nEND')\r\nEND");
        Console.WriteLine("AddPurchase created.");
        conn.Query("IF OBJECT_ID ('GetAllPurchases') IS NULL\r\nBEGIN\r\n\tEXEC('CREATE PROCEDURE GetAllPurchases\r\n\tAS\r\n\tBEGIN\t\r\n\t\tSELECT * FROM Purchase\r\n\tEND')\r\nEND");
        Console.WriteLine("GetAllPurchases created.");
        conn.Query("IF OBJECT_ID ('GetUserPurchases') IS NULL\r\nBEGIN\r\n\tEXEC('CREATE PROCEDURE GetUserPurchases @UserId int\r\nAS\r\nBEGIN\t\r\n\tSELECT * FROM Purchase WHERE UserId = @UserId\r\nEND')\r\nEND");
        Console.WriteLine("GetUserPurchases created.");
        conn.Query("IF OBJECT_ID ('SetDelivery') IS NULL\r\nBEGIN\r\n\tEXEC('CREATE PROCEDURE SetDelivery @DeliveryId int, @Status bit\r\nAS\r\nBEGIN\r\n\tUPDATE Deliveries SET Status = @Status WHERE Id = @DeliveryId\r\n\tSELECT \"true\"\r\nEND')\r\nEND");
        Console.WriteLine("SetDelivery created.");
        conn.Query("IF OBJECT_ID ('SetPayment') IS NULL\r\nBEGIN\r\n\tEXEC('CREATE PROCEDURE SetPayment @PurchaseId int, @PaymentStatus bit\r\nAS\r\nBEGIN\r\n\tUPDATE Purchase SET PaymentStatus = @PaymentStatus WHERE Id = @PurchaseId\r\n\tSELECT \"true\"\r\nEND')\r\nEND");
        Console.WriteLine("SetPayment created.");
        Console.WriteLine("PurchaseSP creation done.");
    }
    Console.WriteLine("DBs ready to use, press enter to exit");
    Console.ReadLine();
}