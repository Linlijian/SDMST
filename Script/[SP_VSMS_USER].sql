--WARNING! ERRORS ENCOUNTERED DURING SQL PARSING!
USE [SDDB]
GO

/****** Object:  StoredProcedure [bond].[SP_VSMS_USER]    Script Date: 5/18/2019 12:59:40 AM ******/
DROP PROCEDURE

IF EXISTS [bond].[SP_VSMS_USER]
	/****** Object:  StoredProcedure [bond].[SP_VSMS_USER]    Script Date: 5/18/2019 12:59:40 AM ******/
	SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

/*
		------------------------------------------------------------------------------------
		|																					|
		|    [VSMS_USER]																	|
		|																					|
		------------------------------------------------------------------------------------
		| history			         										                |
		------------------------------------------------------------------------------------
		| modify by | version   |   date    | action comment						        |
		------------------------------------------------------------------------------------
          A	       1.0.0	 2019-05-18   Create procedure  FOR LOGIN
		------------------------------------------------------------------------------------

*/
CREATE PROCEDURE [bond].[SP_VSMS_USER] @error_code VARCHAR(200) OUTPUT
	,@USER_PWD CHAR(16)
	,@USER_ID CHAR(16)
AS
BEGIN TRY
	--SELECT *
	--FROM VSMS_USER --insert uer pw
	--WHERE USER_ID = @USER_ID
	--	AND USER_PWD = CONVERT(VARCHAR(35), HashBytes('MD5', @USER_PWD), 2)
	SELECT *
FROM VSMS_USER
WHERE USER_ID = LTRIM(RTRIM(@USER_ID))
	AND USER_PWD = CONVERT(VARCHAR(35), HashBytes('MD5', LTRIM(RTRIM(@USER_PWD))), 2)
END TRY

BEGIN CATCH
	SET @error_code = ERROR_MESSAGE()
END CATCH
GO


