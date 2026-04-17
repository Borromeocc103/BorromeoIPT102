CREATE TABLE [dbo].[Tshirt]
(
    [TshirtId]   INT           NOT NULL IDENTITY(1,1) PRIMARY KEY,
    [TshirtName] NVARCHAR(100) NOT NULL,
    [Quantity]   INT           NOT NULL,
    [Price]      DECIMAL(10,2) NOT NULL,
    [Brand]      NVARCHAR(100) NOT NULL
)
