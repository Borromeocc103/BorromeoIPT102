CREATE PROCEDURE [dbo].[AddTShirt]
    @Brand  NVARCHAR(100),
    @Type   NVARCHAR(100),
    @Design NVARCHAR(100),
    @Price  DECIMAL(18,2)
AS
BEGIN
    INSERT INTO TShirt (Brand, Type, Design, Price)
    VALUES (@Brand, @Type, @Design, @Price);
END
