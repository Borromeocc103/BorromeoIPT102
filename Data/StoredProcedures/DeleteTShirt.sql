CREATE PROCEDURE [dbo].[DeleteTShirt]
    @Id INT
AS
BEGIN
    DELETE FROM TShirt
    WHERE Id = @Id;
END
