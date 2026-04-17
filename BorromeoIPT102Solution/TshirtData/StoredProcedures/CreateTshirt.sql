CREATE PROCEDURE [dbo].[CreateTshirt]
    @Name     NVARCHAR(100),
    @Quantity INT,
    @Price    DECIMAL(10,2),
    @Brand    NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO [dbo].[Tshirt] (TshirtName, Quantity, Price, Brand)
    VALUES (@Name, @Quantity, @Price, @Brand);
END
