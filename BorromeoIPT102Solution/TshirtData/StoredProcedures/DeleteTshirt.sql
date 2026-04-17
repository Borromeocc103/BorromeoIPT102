CREATE PROCEDURE [dbo].[DeleteTshirt]
    @TshirtId INT
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM [dbo].[Tshirt] WHERE TshirtId = @TshirtId;
END
