SET NOCOUNT ON;
GO


IF OBJECT_ID(N'dbo.Users', N'U') IS NULL
CREATE TABLE dbo.Users (
    UserId     INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    Username   NVARCHAR(50)  NOT NULL UNIQUE,
    [Password] NVARCHAR(100) NOT NULL,
    [Role]     NVARCHAR(10)  NOT NULL CHECK ([Role] IN (N'Admin', N'Staff')),
    CreatedAt  DATETIME2     NOT NULL DEFAULT SYSDATETIME()
);
GO

IF OBJECT_ID(N'dbo.UserArchive', N'U') IS NULL
CREATE TABLE dbo.UserArchive (
    UserArchiveId INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    UserId        INT           NOT NULL,     
    Username      NVARCHAR(50)  NOT NULL,
    [Password]    NVARCHAR(100) NOT NULL,
    [Role]        NVARCHAR(10)  NOT NULL,
    ArchivedAt    DATETIME2     NOT NULL DEFAULT SYSDATETIME()
);
GO


IF OBJECT_ID(N'dbo.Products', N'U') IS NULL
CREATE TABLE dbo.Products (
    ProductId     INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    ProductName   NVARCHAR(100) NOT NULL,
    Category      NVARCHAR(20)  NOT NULL CHECK (Category IN (N'Books', N'Merchandise')),
    Price         DECIMAL(10,2) NOT NULL CHECK (Price >= 0),
    Variant       NVARCHAR(30)  NULL,            
    StockQuantity INT           NOT NULL DEFAULT 0 CHECK (StockQuantity >= 0),
    Sku           NVARCHAR(20)  NOT NULL UNIQUE,
    IsActive      BIT           NOT NULL DEFAULT 1
);
GO


IF OBJECT_ID(N'dbo.Archive', N'U') IS NULL
CREATE TABLE dbo.Archive (
    ArchiveId     INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    ProductId     INT           NOT NULL,      
    ProductName   NVARCHAR(100) NOT NULL,
    Category      NVARCHAR(20)  NOT NULL,
    Price         DECIMAL(10,2) NOT NULL,
    Variant       NVARCHAR(30)  NULL,
    StockQuantity INT           NOT NULL,
    Sku           NVARCHAR(20)  NOT NULL,
    IsActive      BIT           NOT NULL,
    ArchivedAt    DATETIME2     NOT NULL DEFAULT SYSDATETIME()
);
GO



IF OBJECT_ID(N'dbo.Orders', N'U') IS NULL
CREATE TABLE dbo.Orders (
    OrderId         NVARCHAR(30)  NOT NULL PRIMARY KEY,   
    CreatedAt       DATETIME2     NOT NULL DEFAULT SYSDATETIME(),
    CashierUsername NVARCHAR(50)  NULL,
    PurchaserName   NVARCHAR(100) NULL,
    YearLevel       NVARCHAR(20)  NULL,
    College         NVARCHAR(100) NULL,
    Subtotal        DECIMAL(10,2) NOT NULL,
    DiscountName    NVARCHAR(50)  NOT NULL DEFAULT N'None',
    DiscountAmount  DECIMAL(10,2) NOT NULL DEFAULT 0,
    Total           DECIMAL(10,2) NOT NULL,
    PaymentMethod   NVARCHAR(10)  NOT NULL CHECK (PaymentMethod IN (N'Cash', N'GCash')),
    CashReceived    DECIMAL(10,2) NOT NULL DEFAULT 0,
    ChangeAmount    DECIMAL(10,2) NOT NULL DEFAULT 0
);
GO

IF OBJECT_ID(N'dbo.OrderItems', N'U') IS NULL
CREATE TABLE dbo.OrderItems (
    OrderItemId INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    OrderId     NVARCHAR(30)  NOT NULL REFERENCES dbo.Orders(OrderId) ON DELETE CASCADE,
    ProductId   INT           NULL,
    ProductName NVARCHAR(100) NOT NULL,
    Sku         NVARCHAR(20)  NOT NULL,
    UnitPrice   DECIMAL(10,2) NOT NULL,
    Quantity    INT           NOT NULL CHECK (Quantity > 0),
    LineTotal   AS (UnitPrice * Quantity) PERSISTED
);
GO


CREATE OR ALTER TRIGGER dbo.trg_Products_ArchiveOnDelete
ON dbo.Products
AFTER DELETE
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO dbo.Archive
        (ProductId, ProductName, Category, Price, Variant, StockQuantity, Sku, IsActive)
    SELECT
        ProductId, ProductName, Category, Price, Variant, StockQuantity, Sku, IsActive
    FROM deleted;
END
GO


CREATE OR ALTER PROCEDURE dbo.usp_ArchiveStaff
    @UserId INT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;   -- any error rolls the whole thing back

    IF NOT EXISTS (SELECT 1 FROM dbo.Users WHERE UserId = @UserId AND [Role] = N'Staff')
        THROW 50001, N'Staff account not found. Admin accounts cannot be archived.', 1;

    BEGIN TRANSACTION;

        INSERT INTO dbo.UserArchive (UserId, Username, [Password], [Role])
        SELECT UserId, Username, [Password], [Role]
        FROM dbo.Users
        WHERE UserId = @UserId;

        DELETE FROM dbo.Users WHERE UserId = @UserId;

    COMMIT TRANSACTION;
END
GO
--Archive
CREATE OR ALTER PROCEDURE dbo.usp_DeleteArchivedUser
    @UserArchiveId INT
AS
BEGIN
    SET NOCOUNT ON;

    DELETE FROM dbo.UserArchive WHERE UserArchiveId = @UserArchiveId;

    IF @@ROWCOUNT = 0
        THROW 50002, N'Archived account not found.', 1;
END
GO

--Users
IF NOT EXISTS (SELECT 1 FROM dbo.Users WHERE Username = N'admin')
    INSERT INTO dbo.Users (Username, [Password], [Role]) VALUES (N'admin', N'admin123', N'Admin');

IF NOT EXISTS (SELECT 1 FROM dbo.Users WHERE Username = N'staff1')
    INSERT INTO dbo.Users (Username, [Password], [Role]) VALUES (N'staff1', N'staff123', N'Staff');

IF NOT EXISTS (SELECT 1 FROM dbo.Users WHERE Username = N'staff2')
    INSERT INTO dbo.Users (Username, [Password], [Role]) VALUES (N'staff2', N'staff123', N'Staff');
GO

--Merch
INSERT INTO dbo.Products (ProductName, Category, Price, Variant, StockQuantity, Sku)
SELECT v.ProductName, v.Category, v.Price, v.Variant, v.StockQuantity, v.Sku
FROM (VALUES
    (N'PE Uniform',                N'Merchandise', 350.00, N'Size M', 50, N'UM-001'),
    (N'ID Holder',                 N'Merchandise',  45.00, NULL,      80, N'UM-002'),
    (N'UMak Polo',                 N'Merchandise', 450.00, N'Size L', 40, N'UM-003'),
    (N'UMak Lanyard',              N'Merchandise',  60.00, NULL,      80, N'UM-004'),
    (N'PE Jogging Pants',          N'Merchandise', 400.00, N'Size M', 40, N'UM-005'),
    (N'College Manual',            N'Books',       280.00, NULL,      30, N'UM-006'),
    (N'UMak Spiral Notebook',      N'Merchandise',  35.00, NULL,     100, N'UM-007'),
    (N'Ballpen Set (3pcs)',        N'Merchandise',  65.00, NULL,      60, N'UM-008'),
    (N'University Pin',            N'Merchandise',  50.00, NULL,      60, N'UM-009'),
    (N'Computer Programming Book', N'Books',       550.00, NULL,      20, N'UM-010')
) AS v (ProductName, Category, Price, Variant, StockQuantity, Sku)
WHERE NOT EXISTS (SELECT 1 FROM dbo.Products p WHERE p.Sku = v.Sku);
GO


SELECT * FROM dbo.Users;
SELECT * FROM dbo.Products;
SELECT * FROM dbo.Archive;
SELECT * FROM dbo.UserArchive;
GO



SET NOCOUNT ON;
GO
-- Books
INSERT INTO dbo.Products (ProductName, Category, Price, Variant, StockQuantity, Sku)
SELECT v.ProductName, v.Category, v.Price, v.Variant, v.StockQuantity, v.Sku
FROM (VALUES
    (N'Art Appreciation 2nd Edition',                               N'Books', 425.00, NULL, 20, N'UM-011'),
    (N'Business and Transfer Taxation',                             N'Books', 500.00, NULL, 20, N'UM-012'),
    (N'Business Communication',                                     N'Books', 365.00, NULL, 20, N'UM-013'),
    (N'Computer Programming 1 Learn Java the Easy Way',             N'Books', 375.00, NULL, 20, N'UM-014'),
    (N'Computer Programming 2 Java Concepts',                       N'Books', 395.00, NULL, 20, N'UM-015'),
    (N'Dalumat ng sa Filipino',                                     N'Books', 500.00, NULL, 20, N'UM-016'),
    (N'Entrepreneurship in Tourism and Hospitality',                N'Books', 375.00, NULL, 20, N'UM-017'),
    (N'Essentials of Marketing',                                    N'Books', 350.00, NULL, 20, N'UM-018'),
    (N'Essentials of Organizational Communication',                 N'Books', 475.00, NULL, 20, N'UM-019'),
    (N'Essentials of Purposive Communication',                      N'Books', 450.00, NULL, 20, N'UM-020'),
    (N'Essentials of Workplace-Readiness Skills',                   N'Books', 450.00, NULL, 20, N'UM-021'),
    (N'Events Management',                                          N'Books', 350.00, NULL, 20, N'UM-022'),
    (N'Field Study 4',                                              N'Books', 300.00, NULL, 20, N'UM-023'),
    (N'Field Study 5',                                              N'Books', 300.00, NULL, 20, N'UM-024'),
    (N'Field Study 6',                                              N'Books', 300.00, NULL, 20, N'UM-025'),
    (N'Foundations of Operations Management',                       N'Books', 475.00, NULL, 20, N'UM-026'),
    (N'Fundamentals of Management',                                 N'Books', 345.00, NULL, 20, N'UM-027'),
    (N'General Zoology',                                            N'Books', 425.00, NULL, 20, N'UM-028'),
    (N'Housekeeping Operation',                                     N'Books', 450.00, NULL, 20, N'UM-029'),
    (N'Introduction to Computing',                                  N'Books', 395.00, NULL, 20, N'UM-030'),
    (N'Kontekstwalisadong Filipino sa Malayuning Komunikasyon',     N'Books', 325.00, NULL, 20, N'UM-031'),
    (N'Kontektwalisadong Kom. sa Filipino',                         N'Books', 450.00, NULL, 20, N'UM-032'),
    (N'Laboratory Manual in Chemistry Part 1',                      N'Books', 300.00, NULL, 20, N'UM-033'),
    (N'Laboratory Manual in Physics',                               N'Books', 425.00, NULL, 20, N'UM-034'),
    (N'Makati Heritage (Herons)',                                   N'Books', 250.00, NULL, 20, N'UM-035'),
    (N'Marketing Management',                                       N'Books', 425.00, NULL, 20, N'UM-036'),
    (N'Mastering Pricing and Costing',                              N'Books', 375.00, NULL, 20, N'UM-037'),
    (N'Mathematics in the Modern World',                            N'Books', 500.00, NULL, 20, N'UM-038'),
    (N'Mechanics of Deformable Bodies',                             N'Books', 600.00, NULL, 20, N'UM-039'),
    (N'Modern Physics',                                             N'Books', 350.00, NULL, 20, N'UM-040'),
    (N'Multicultural Diversity in Workplace',                       N'Books', 450.00, NULL, 20, N'UM-041'),
    (N'Nursing Passport',                                           N'Books', 330.00, NULL, 20, N'UM-042'),
    (N'Nursing Skills Laboratory Workbook',                         N'Books', 550.00, NULL, 20, N'UM-043'),
    (N'Panitikang Panlipunan',                                      N'Books', 290.00, NULL, 20, N'UM-044'),
    (N'People, Earth, and Ecosystem',                               N'Books', 450.00, NULL, 20, N'UM-045'),
    (N'Popular Culture',                                            N'Books', 510.00, NULL, 20, N'UM-046'),
    (N'Professional Development',                                   N'Books', 360.00, NULL, 20, N'UM-047'),
    (N'Research in Tourism and Hospitality',                        N'Books', 425.00, NULL, 20, N'UM-048'),
    (N'Risk Management',                                            N'Books', 375.00, NULL, 20, N'UM-049'),
    (N'Science, Technology and Society',                            N'Books', 375.00, NULL, 20, N'UM-050'),
    (N'Simplified Acctg for Sole Proprietorship V 1 2018 Edition',  N'Books', 450.00, NULL, 20, N'UM-051'),
    (N'Simplified Acctg for Sole Proprietorship Volume 1 2018 Edition', N'Books', 450.00, NULL, 20, N'UM-052'),
    (N'Simplified Acctg for Sole Proprietorship Volume 1 2023 Edition', N'Books', 450.00, NULL, 20, N'UM-053'),
    (N'Sosyedad at Literatura',                                     N'Books', 490.00, NULL, 20, N'UM-054'),
    (N'Strategic Management in Tourism & Hospitality Industry',     N'Books', 400.00, NULL, 20, N'UM-055'),
    (N'Sustainable Tourism',                                        N'Books', 400.00, NULL, 20, N'UM-056'),
    (N'The Contemporary World',                                     N'Books', 410.00, NULL, 20, N'UM-057'),
    (N'Tourism and Hospitality Marketing - Mindshapers',            N'Books', 445.00, NULL, 20, N'UM-058'),
    (N'Tourism and Hospitality Marketing - Rex Bookstore',          N'Books', 325.00, NULL, 20, N'UM-059'),
    (N'Understanding the Self',                                     N'Books', 530.00, NULL, 20, N'UM-060')
) AS v (ProductName, Category, Price, Variant, StockQuantity, Sku)
WHERE NOT EXISTS (SELECT 1 FROM dbo.Products p WHERE p.Sku = v.Sku);
GO

--Uniform
INSERT INTO dbo.Products (ProductName, Category, Price, Variant, StockQuantity, Sku)
SELECT v.ProductName, v.Category, v.Price, v.Variant, v.StockQuantity, v.Sku
FROM (VALUES
    -- College uniform
    (N'College Uniform - Set with ID Lace',      N'Merchandise',  700.00, NULL,        20, N'UM-061'),
    (N'College Uniform - Blouse / Polo',         N'Merchandise',  375.00, NULL,        20, N'UM-062'),
    (N'College Uniform - Pants',                 N'Merchandise',  375.00, NULL,        20, N'UM-063'),
    -- PE uniform
    (N'PE Uniform with ID Lace',                 N'Merchandise',  750.00, N'XS-XL',    20, N'UM-064'),
    (N'PE Uniform with ID Lace',                 N'Merchandise',  800.00, N'2XL-4XL',  20, N'UM-065'),
    (N'PE Uniform with ID Lace',                 N'Merchandise',  850.00, N'5XL',      20, N'UM-066'),
    -- CET uniform
    (N'CET Uniform - College Shirt with ID Lace', N'Merchandise', 700.00, NULL,        20, N'UM-067'),
    (N'CET Uniform - College Shirt',             N'Merchandise',  600.00, NULL,        20, N'UM-068'),
    (N'CET Uniform - ID Lace',                   N'Merchandise',  120.00, NULL,        20, N'UM-069'),
    -- CTHM uniform
    (N'CTHM Uniform - Chef''s',                  N'Merchandise', 2150.00, NULL,        20, N'UM-070'),
    (N'CTHM Uniform - College Shirt',            N'Merchandise',  575.00, NULL,        20, N'UM-071'),
    (N'CTHM Uniform - Hotel Management',         N'Merchandise', 2650.00, NULL,        20, N'UM-072'),
    (N'CTHM Uniform - Tourism Management',       N'Merchandise', 1850.00, NULL,        20, N'UM-073'),
    -- IIHS uniform
    (N'IIHS Uniform - Scrub Suit',               N'Merchandise', 1500.00, NULL,        20, N'UM-074'),
    (N'IIHS Uniform - White Coat',               N'Merchandise', 1800.00, NULL,        20, N'UM-075'),
    (N'IIHS Uniform - Black and White Uniform',  N'Merchandise', 1650.00, NULL,        20, N'UM-076'),
    -- IOP uniform
    (N'IOP Uniform - Scrub Suit',                N'Merchandise', 1500.00, NULL,        20, N'UM-077'),
    (N'IOP Uniform - White Coat',                N'Merchandise', 1800.00, NULL,        20, N'UM-078'),
    (N'IOP Uniform - White Uniform',             N'Merchandise', 1650.00, NULL,        20, N'UM-079'),
    -- IOPSY uniform
    (N'IOPSY Uniform - Psych Uniform with ID Lace', N'Merchandise', 1000.00, NULL,     20, N'UM-080'),
    -- NSTP shirt
    (N'NSTP Shirt',                              N'Merchandise',  460.00, N'XS-XL',    20, N'UM-081'),
    (N'NSTP Shirt',                              N'Merchandise',  480.00, N'2XL',      20, N'UM-082'),
    (N'NSTP Shirt',                              N'Merchandise',  500.00, N'3XL',      20, N'UM-083'),
    (N'NSTP Shirt',                              N'Merchandise',  520.00, N'4XL',      20, N'UM-084'),
    (N'NSTP Shirt',                              N'Merchandise',  540.00, N'5XL',      20, N'UM-085'),
    -- ROTC shirt and belt
    (N'ROTC Shirt & Belt',                       N'Merchandise',  210.00, N'2XS-Large', 20, N'UM-086'),
    (N'ROTC Shirt & Belt',                       N'Merchandise',  215.00, N'XL-2XL',   20, N'UM-087'),
    (N'ROTC Shirt & Belt',                       N'Merchandise',  225.00, N'3XL',      20, N'UM-088'),
    (N'ROTC Shirt & Belt',                       N'Merchandise',  230.00, N'4XL-5XL',  20, N'UM-089'),
    (N'ROTC Naval Belt',                         N'Merchandise',  110.00, NULL,        20, N'UM-090')
) AS v (ProductName, Category, Price, Variant, StockQuantity, Sku)
WHERE NOT EXISTS (SELECT 1 FROM dbo.Products p WHERE p.Sku = v.Sku);
GO


SELECT Category, COUNT(*) AS Items FROM dbo.Products GROUP BY Category;
SELECT COUNT(*) AS TotalProducts FROM dbo.Products;
GO

