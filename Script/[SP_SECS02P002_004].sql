--WARNING! ERRORS ENCOUNTERED DURING SQL PARSING!
USE [SDDB]
GO

/****** Object:  StoredProcedure [bond].[SP_SECS02P002_004]    Script Date: 5/18/2019 12:10:05 PM ******/
DROP PROCEDURE

IF EXISTS [bond].[SP_SECS02P002_004]
	/****** Object:  StoredProcedure [bond].[SP_SECS02P002_004]    Script Date: 5/18/2019 12:10:05 PM ******/
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
          A	       1.0.0	 2019-05-18   Create procedure  FOR update
		------------------------------------------------------------------------------------

*/
CREATE PROCEDURE [bond].[SP_SECS02P002_004] @error_code VARCHAR(200) OUTPUT
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
	,@USER_PWD_OLD VARCHAR(35)
	,@TELEPHONE VARCHAR(50)
	,@EMAIL VARCHAR(50)
	,@USER_STATUS CHAR(1)
	,@IS_DISABLED CHAR(1)
	,@LAST_LOGIN_DATE DATETIME
	,@CRET_BY CHAR(16)
	,@CRET_DATE DATETIME
	,@MNT_BY CHAR(16)
	,@MNT_DATE DATETIME
	,@IDCPWD CHAR(10)
AS
BEGIN TRY
	IF ISNULL(@IDCPWD, '') <> ''
	BEGIN
		IF EXISTS (
				SELECT *
				FROM VSMS_USER
				WHERE user_id = @USER_ID
					AND USER_PWD = CONVERT(VARCHAR(35), HashBytes('MD5', @USER_PWD_OLD), 2)
				)
		BEGIN
			IF @USER_PWD = @USER_PWD_R
			BEGIN
				UPDATE VSMS_USER
				SET USER_FNAME_TH = @USER_FNAME_TH
					,USER_LNAME_TH = @USER_LNAME_TH
					,USER_FNAME_EN = @USER_FNAME_EN
					,USER_LNAME_EN = @USER_LNAME_EN
					,TITLE_ID = @TITLE_ID
					,USG_ID = @USG_ID
					,TELEPHONE = @TELEPHONE
					,EMAIL = @EMAIL
					,USER_STATUS = @USER_STATUS
					,IS_DISABLED = @IS_DISABLED
					,MNT_BY = @MNT_DATE
					,MNT_DATE = @MNT_DATE
					,COM_CODE = @COM_CODE
					,USER_PWD = CONVERT(VARCHAR(35), HashBytes('MD5', @USER_PWD), 2)
				WHERE COM_CODE = @COM_CODE
					AND USER_ID = @USER_ID

				SET @error_code = 0
			END
			ELSE
			BEGIN
				SET @error_code = 'New Password and Confirm Password in not mathing'
			END
		END
		ELSE
		BEGIN
			SET @error_code = 'Old Password in not mathing'
		END
	END
	ELSE
	BEGIN
		UPDATE VSMS_USER
		SET USER_FNAME_TH = @USER_FNAME_TH
			,USER_LNAME_TH = @USER_LNAME_TH
			,USER_FNAME_EN = @USER_FNAME_EN
			,USER_LNAME_EN = @USER_LNAME_EN
			,TITLE_ID = @TITLE_ID
			,USG_ID = @USG_ID
			,TELEPHONE = @TELEPHONE
			,EMAIL = @EMAIL
			,USER_STATUS = @USER_STATUS
			,IS_DISABLED = @IS_DISABLED
			,MNT_BY = @MNT_DATE
			,MNT_DATE = @MNT_DATE
			,COM_CODE = @COM_CODE
		WHERE COM_CODE = @COM_CODE
			AND USER_ID = @USER_ID

		SET @error_code = 0
	END
END TRY

BEGIN CATCH
END CATCH
GO


