CREATE PROCEDURE [dbo].[SearchTShirt]
    @Keyword NVARCHAR(100)
AS
BEGIN
    SELECT * FROM TShirt
    WHERE Brand  LIKE '%' + @Keyword + '%'
       OR Type   LIKE '%' + @Keyword + '%'
       OR Design LIKE '%' + @Keyword + '%'
    ORDER BY Id DESC;
END
