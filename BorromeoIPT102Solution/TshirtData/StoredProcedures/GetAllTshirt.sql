CREATE PROCEDURE [dbo].[GetAllTshirt]
AS
BEGIN
    SET NOCOUNT ON;
    SELECT TshirtId, TshirtName AS Name, Quantity, Price, Brand
    FROM [dbo].[Tshirt];
END
