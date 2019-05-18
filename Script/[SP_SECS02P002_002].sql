--WARNING! ERRORS ENCOUNTERED DURING SQL PARSING!
USE [SDDB]
GO

/****** Object:  StoredProcedure [bond].[SP_SECS02P002_002]    Script Date: 5/18/2019 9:49:57 AM ******/
DROP PROCEDURE

IF EXISTS [bond].[SP_SECS02P002_002]
	/****** Object:  StoredProcedure [bond].[SP_SECS02P002_002]    Script Date: 5/18/2019 9:49:57 AM ******/
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
          A	       1.0.0	 2019-05-18   Create procedure  FOR insert 
		------------------------------------------------------------------------------------

*/
CREATE PROCEDURE [bond].[SP_SECS02P002_002] @error_code VARCHAR(200) OUTPUT
	,@COM_CODE CHAR(6)
	,@USER_ID CHAR(16)
	,@USER_FNAME_TH VARCHAR(255)
	,@USER_LNAME_TH VARCHAR(255)
	,@USER_FNAME_EN VARCHAR(255)
	,@USER_LNAME_EN VARCHAR(255)
	,@TITLE_ID DECIMAL(18, 0)
	,@USG_ID DECIMAL(18, 0)
	,@USER_PWD VARCHAR(35)
	,@USER_PWD_R VARCHAR(35)
	,@TELEPHONE VARCHAR(50)
	,@EMAIL VARCHAR(50)
	,@USER_STATUS CHAR(1)
	,@IS_DISABLED CHAR(1)
	,@LAST_LOGIN_DATE DATETIME
	,@CRET_BY CHAR(16)
	,@CRET_DATE DATETIME
	,@MNT_BY CHAR(16)
	,@MNT_DATE DATETIME
AS
BEGIN TRY
	IF @USER_PWD = @USER_PWD_R
	BEGIN
		INSERT INTO VSMS_USER (
			COM_CODE
			,USER_ID
			,USER_FNAME_TH
			,USER_LNAME_TH
			,USER_FNAME_EN
			,USER_LNAME_EN
			,TITLE_ID
			,USG_ID
			,USER_PWD
			,TELEPHONE
			,EMAIL
			,USER_STATUS
			,IS_DISABLED
			,LAST_LOGIN_DATE
			,CRET_BY
			,CRET_DATE
			,MNT_BY
			,MNT_DATE
			)
		VALUES (
			@COM_CODE
			,@USER_ID
			,@USER_FNAME_TH
			,@USER_LNAME_TH
			,@USER_FNAME_EN
			,@USER_LNAME_EN
			,@TITLE_ID
			,@USG_ID
			,CONVERT(VARCHAR(35), HashBytes('MD5', @USER_PWD), 2)
			,@TELEPHONE
			,@EMAIL
			,@USER_STATUS
			,@IS_DISABLED
			,@LAST_LOGIN_DATE
			,@CRET_BY
			,@CRET_DATE
			,@MNT_BY
			,@MNT_DATE
			)

		SET @error_code = 0
	END else begin
	SET @error_code = 'Password is Not Matching'
	end
END TRY

BEGIN CATCH
	SET @error_code = ERROR_MESSAGE()
END CATCH
GO


