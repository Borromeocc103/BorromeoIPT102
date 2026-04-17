CREATE PROCEDURE [dbo].[ReadTshirtById]
    @TshirtId INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT TshirtId, TshirtName AS Name, Quantity, Price, Brand
    FROM [dbo].[Tshirt]
    WHERE TshirtId = @TshirtId;
END
