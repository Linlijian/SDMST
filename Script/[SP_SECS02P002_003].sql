--WARNING! ERRORS ENCOUNTERED DURING SQL PARSING!
USE [SDDB]
GO

/****** Object:  StoredProcedure [bond].[SP_SECS02P002_003]    Script Date: 5/18/2019 10:51:57 AM ******/
DROP PROCEDURE

IF EXISTS [bond].[SP_SECS02P002_003]
	/****** Object:  StoredProcedure [bond].[SP_SECS02P002_003]    Script Date: 5/18/2019 10:51:57 AM ******/
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
          A	       1.0.0	 2019-05-18   Create procedure  FOR check old pwd
		------------------------------------------------------------------------------------

*/
CREATE PROCEDURE [bond].[SP_SECS02P002_003] @error_code VARCHAR(200) OUTPUT
	,@USER_PWD VARCHAR(35)
	,@USER_ID VARCHAR(16)
AS
BEGIN TRY
	IF EXISTS (
			SELECT *
			FROM VSMS_USER
			WHERE user_id = @USER_ID
				AND USER_PWD = CONVERT(VARCHAR(35), HashBytes('MD5', @USER_PWD), 2)
			)
	BEGIN
		SET @error_code = 0
	END
	ELSE
	BEGIN
		SET @error_code = 'Old Password is Not Matching'
	END
END TRY

BEGIN CATCH
	SET @error_code = ERROR_MESSAGE()
END CATCH
GO


