CREATE PROCEDURE [dbo].[UpdateTshirt]
    @TshirtId INT,
    @Name     NVARCHAR(100),
    @Quantity INT,
    @Price    DECIMAL(10,2),
    @Brand    NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE [dbo].[Tshirt]
    SET TshirtName = @Name,
        Quantity   = @Quantity,
        Price      = @Price,
        Brand      = @Brand
    WHERE TshirtId = @TshirtId;
END
