SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- The function is adapted from the code at http://stackoverflow.com/a/10914602
CREATE FUNCTION [dbo].[Func_SplitString] 
( 
	@stringToSplit VARCHAR(MAX),
	@delimiter VARCHAR(20)
)
RETURNS
	@returnList TABLE 
	(
		[Name] NVARCHAR(500)
	)
AS
BEGIN
	DECLARE @pos INT

	SET @pos  = CHARINDEX(@delimiter, @stringToSplit) 

	WHILE @pos > 0
	BEGIN 
		INSERT INTO @returnList 
		SELECT SUBSTRING(@stringToSplit, 1, @pos - 1)

		SET @pos = @pos + LEN(@delimiter)

		SET @stringToSplit = SUBSTRING(@stringToSplit, @pos, LEN(@stringToSplit) - @pos + 1)
		
		SET @pos  = CHARINDEX(@delimiter, @stringToSplit) 
	END

	INSERT INTO @returnList
	SELECT @stringToSplit

	RETURN
END