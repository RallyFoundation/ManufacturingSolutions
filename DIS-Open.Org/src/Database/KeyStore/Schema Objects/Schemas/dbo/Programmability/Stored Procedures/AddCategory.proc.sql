CREATE PROCEDURE [dbo].[AddCategory]
	-- Add the parameters for the function here
	@categoryName nvarchar(64),
	@logID int
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @CatID INT
	SELECT @CatID = CategoryID FROM Category WHERE CategoryName = @categoryName
	IF @CatID IS NULL
	BEGIN
		INSERT INTO Category (CategoryName) VALUES(@categoryName)
		SELECT @CatID = @@IDENTITY
	END

	EXEC InsertCategoryLog @CatID, @logID 

	RETURN @CatID
END