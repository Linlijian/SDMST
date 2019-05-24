--WARNING! ERRORS ENCOUNTERED DURING SQL PARSING!
USE [SDDB]
GO

/****** Object:  StoredProcedure [bond].[SP_SECS02P002_005]    Script Date: 5/24/2019 12:18:09 PM ******/
DROP PROCEDURE

IF EXISTS [bond].[SP_SECS02P002_005]
	/****** Object:  StoredProcedure [bond].[SP_SECS02P002_005]    Script Date: 5/24/2019 12:18:09 PM ******/
	SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

/*
		------------------------------------------------------------------------------------
		|																					|
		|    [SECS02P002]																	|
		|																					|
		------------------------------------------------------------------------------------
		| history			         										                |
		------------------------------------------------------------------------------------
		| modify by | version   |   date    | action comment						        |
		------------------------------------------------------------------------------------
          A	       1.0.0	 2019-05-24   Create procedure  FOR ForGetPAssword
		------------------------------------------------------------------------------------

*/
CREATE PROCEDURE [bond].[SP_SECS02P002_005] @error_code VARCHAR(200) OUTPUT
	,@COM_CODE CHAR(6)
	,@USER_ID CHAR(16)
	,@CRET_BY CHAR(16)
AS
BEGIN TRY
	DECLARE @DEFUALT VARCHAR(16)

	SET @DEFUALT = 'P@ssw0rd'

	IF ISNULL(@USER_ID, '') <> ''
	BEGIN
		UPDATE VSMS_USER
		SET USER_PWD = CONVERT(VARCHAR(35), HashBytes('MD5', @DEFUALT), 2)
			,MNT_BY = @CRET_BY
			,MNT_DATE = GETDATE()
		WHERE USER_ID = @USER_ID
		SET @error_code = 0
	END
	ELSE
	BEGIN
		SET @error_code = 'USER_ID is null'
	END
END TRY

BEGIN CATCH
	SET @error_code = ERROR_MESSAGE()
END CATCH
GO


