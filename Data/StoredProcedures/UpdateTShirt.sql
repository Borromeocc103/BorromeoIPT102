CREATE PROCEDURE [dbo].[UpdateTShirt]
    @Id     INT,
    @Brand  NVARCHAR(100),
    @Type   NVARCHAR(100),
    @Design NVARCHAR(100),
    @Price  DECIMAL(18,2)
AS
BEGIN
    UPDATE TShirt
    SET Brand  = @Brand,
        Type   = @Type,
        Design = @Design,
        Price  = @Price
    WHERE Id = @Id;
END
