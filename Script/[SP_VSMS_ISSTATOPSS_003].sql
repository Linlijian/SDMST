--WARNING! ERRORS ENCOUNTERED DURING SQL PARSING!
USE [SDDB]
GO

/****** Object:  StoredProcedure [bond].[SP_VSMS_ISSTATOPSS_003]    Script Date: 5/19/2019 1:12:33 PM ******/
DROP PROCEDURE

IF EXISTS [bond].[SP_VSMS_ISSTATOPSS_003]
	/****** Object:  StoredProcedure [bond].[SP_VSMS_ISSTATOPSS_003]    Script Date: 5/19/2019 1:12:33 PM ******/
	SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

/*
		------------------------------------------------------------------------------------
		|																					|
		|    MISS01P003															|
		|																					|
		------------------------------------------------------------------------------------
		| history			         										                |
		------------------------------------------------------------------------------------
		| modify by | version   |   date    | action comment						        |
		------------------------------------------------------------------------------------
          A		   1.0.0	 2019-3-20   Create procedure  FOR Cancel or Areed
		  Chang    1.0.1	 2019-3-20   add logic
		  A	       1.1.0	 2019-3-26   condition
		------------------------------------------------------------------------------------

*/
CREATE PROCEDURE [bond].[SP_VSMS_ISSTATOPSS_003] @error_code VARCHAR(200) OUTPUT
	,@ISE_KEY CHAR(18)
	,@COM_CODE CHAR(6)
	,@FALG CHAR(1)
	,@NO DECIMAL(18, 0)
	,@CRET_BY CHAR(16)
	,@CRET_DATE DATETIME
AS
BEGIN
	DECLARE @MSG CHAR(200)
	DECLARE @CONS CHAR(50)
	DECLARE @MI DECIMAL(6, 0) --minute
		,@WK DECIMAL(6, 0) --week
		,@DD DECIMAL(6, 0) --day
		,@HH DECIMAL(6, 0) --hour
		,@H_ISS DECIMAL(6, 0) --hour ise
		,@MI_ISS DECIMAL(6, 0) --minute ise
		,@H_RES DECIMAL(6, 0) --hour res ise
		,@MI_RES DECIMAL(6, 0) --minute res ise
	DECLARE @HOLIDAY DECIMAL(6, 0)

	IF @FALG = 'A'
	BEGIN
		BEGIN TRY
			UPDATE VSMS_ISSTATOPSS
			SET ASSIGN_STATUS = 'D'
				,ISE_DATE_ONPROCESS = GETDATE()
				,ISE_DATE_DOING = GETDATE()
				,ISE_STATUS = 'P'
				,MNT_BY = @CRET_BY
				,MNT_DATE = GETDATE()
			WHERE ISE_KEY = @ISE_KEY
				AND COM_CODE = @COM_CODE

			UPDATE VSMS_ISSUE
			SET STATUS = 'P'
			WHERE no = @NO
				AND COM_CODE = @COM_CODE

			SET @error_code = 0
		END TRY

		BEGIN CATCH
			SET @error_code = ERROR_MESSAGE()
		END CATCH

		--ALEART USER
		SET @MSG = CONVERT(NVARCHAR, @CRET_BY) + ' Agree Fix Issue No.' + CONVERT(NVARCHAR, @NO)

		BEGIN TRY
			INSERT INTO VSMS_NOTIFICATION (
				[COM_CODE]
				,[NO]
				--,[NTF_KEY]
				,[USER_ID]
				,[DETAIL]
				,[FLAG]
				,[ACTIVE]
				,[CRET_DATE]
				,[MNT_DATE]
				)
			VALUES (
				@COM_CODE
				,@NO
				,(
					SELECT CRET_BY
					FROM VSMS_ISSUE
					WHERE no = @NO
						AND COM_CODE = @COM_CODE
					)
				,@MSG
				,'T'
				,'F'
				,GETDATE()
				,GETDATE()
				)

			SET @error_code = 0
		END TRY

		BEGIN CATCH
			SET @error_code = ERROR_MESSAGE()

			RETURN
		END CATCH
			--END
	END
	ELSE IF @FALG = 'C'
	BEGIN
		BEGIN TRY
			DELETE
			FROM VSMS_ISSTATOPSS
			WHERE ISE_KEY = @ISE_KEY
				AND COM_CODE = @COM_CODE

			SET @error_code = 0
		END TRY

		BEGIN CATCH
			SET @error_code = ERROR_MESSAGE()
		END CATCH

		--ALEART USER
		SET @MSG = CONVERT(NVARCHAR, @CRET_BY) + ' Cancel Fix Issue No.' + CONVERT(NVARCHAR, @NO)

		BEGIN TRY
			INSERT INTO VSMS_NOTIFICATION (
				[COM_CODE]
				,[NO]
				--,[NTF_KEY]
				,[USER_ID]
				,[DETAIL]
				,[FLAG]
				,[ACTIVE]
				,[CRET_DATE]
				,[MNT_DATE]
				)
			VALUES (
				@COM_CODE
				,@NO
				,(
					SELECT CRET_BY
					FROM VSMS_ISSUE
					WHERE no = @NO
						AND COM_CODE = @COM_CODE
					)
				,@MSG
				,'T'
				,'F'
				,GETDATE()
				,GETDATE()
				)

			SET @error_code = 0
		END TRY

		BEGIN CATCH
			SET @error_code = ERROR_MESSAGE()

			RETURN
		END CATCH
			--END
	END
	ELSE IF @FALG = 'D'
	BEGIN
		IF ISNULL((
					SELECT SOLUTION
					FROM VSMS_ISSUE
					WHERE COM_CODE = @COM_CODE
						AND NO = @NO
					), '') <> ''
		BEGIN
			BEGIN TRY
				UPDATE VSMS_ISSTATOPSS
				SET ASSIGN_STATUS = 'T' --testing
					,ISE_DATE_TESTING = GETDATE()
					,MNT_BY = @CRET_BY
					,MNT_DATE = GETDATE()
				WHERE ISE_KEY = @ISE_KEY
					AND COM_CODE = @COM_CODE

				SET @error_code = 0
			END TRY

			BEGIN CATCH
				SET @error_code = ERROR_MESSAGE()
			END CATCH
		END
		ELSE
		BEGIN
			SET @error_code = 'Fix Done InComplete, Please add Solution for Fix'
		END
	END
	ELSE IF @FALG = 'T'
	BEGIN
		BEGIN TRY
			UPDATE VSMS_ISSTATOPSS
			SET ASSIGN_STATUS = 'E'
				,ISE_DATE_DONE = GETDATE()
				,MNT_BY = @CRET_BY
				,MNT_DATE = GETDATE()
			WHERE ISE_KEY = @ISE_KEY
				AND COM_CODE = @COM_CODE

			SET @error_code = 0
		END TRY

		BEGIN CATCH
			SET @error_code = ERROR_MESSAGE()
		END CATCH

		--ALEART USER
		SET @MSG = CONVERT(NVARCHAR, @CRET_BY) + ' Comfirm Test Issue No.' + CONVERT(NVARCHAR, @NO) + ' is Complete!'

		BEGIN TRY
			INSERT INTO VSMS_NOTIFICATION (
				[COM_CODE]
				,[NO]
				--,[NTF_KEY]
				,[USER_ID]
				,[DETAIL]
				,[FLAG]
				,[ACTIVE]
				,[CRET_DATE]
				,[MNT_DATE]
				)
			VALUES (
				@COM_CODE
				,@NO
				,(
					SELECT ASSIGN_USER
					FROM VSMS_ISSUE
					WHERE no = @NO
						AND COM_CODE = @COM_CODE
					)
				,@MSG
				,'T'
				,'F'
				,GETDATE()
				,GETDATE()
				)

			SET @error_code = 0
		END TRY

		BEGIN CATCH
			SET @error_code = ERROR_MESSAGE()

			RETURN
		END CATCH
			--END
	END
	ELSE IF @FALG = 'F'
	BEGIN
		IF ISNULL((
					SELECT DEFECT
					FROM VSMS_ISSUE
					WHERE COM_CODE = @COM_CODE
						AND NO = @NO
					), '') <> ''
		BEGIN
			BEGIN TRY
				UPDATE VSMS_ISSTATOPSS
				SET ISE_DATE_FOLLOWUP = GETDATE()
					,ISE_STATUS = 'F'
					,MNT_BY = @CRET_BY
					,MNT_DATE = GETDATE()
				WHERE ISE_KEY = @ISE_KEY
					AND COM_CODE = @COM_CODE

				SET @error_code = 0
			END TRY

			BEGIN CATCH
				SET @error_code = ERROR_MESSAGE()

				RETURN
			END CATCH

			BEGIN TRY
				UPDATE VSMS_ISSUE
				SET STATUS = 'F'
					,MNT_BY = @CRET_BY
					,MNT_DATE = GETDATE()
				WHERE NO = @NO
					AND COM_CODE = @COM_CODE

				SET @error_code = 0
			END TRY

			BEGIN CATCH
				SET @error_code = ERROR_MESSAGE()

				RETURN
			END CATCH
		END
		ELSE
		BEGIN
			SET @error_code = 'Follow up InComplete, Defect Not Found'
		END
	END
	ELSE IF @FALG = 'G'
	BEGIN
		SET @CONS = (
				SELECT DEFECT
				FROM VSMS_ISSUE
				WHERE COM_CODE = @COM_CODE
					AND NO = @NO
				)

		IF EXISTS (
				SELECT *
				FROM VSMS_PIT_DATA
				WHERE COM_CODE = 'VSM'
					AND KEY_ID = 'I'
					AND IS_CONS = 'Y'
					AND ISSUE_TYPE = @CONS
				)
		BEGIN
			BEGIN TRY
				UPDATE VSMS_ISSTATOPSS
				SET ISE_STATUS = 'S'
					,ISE_DATE_CLOSE = GETDATE()
					,MNT_BY = @CRET_BY
					,MNT_DATE = GETDATE()
				WHERE COM_CODE = @COM_CODE
					AND ISE_KEY = @ISE_KEY

				SET @error_code = 0
			END TRY

			BEGIN CATCH
				SET @error_code = ERROR_MESSAGE()

				RETURN
			END CATCH

			BEGIN TRY
				UPDATE VSMS_ISSUE
				SET STATUS = 'S'
					,CLOSE_DATE = GETDATE()
					,MNT_BY = @CRET_BY
					,MNT_DATE = GETDATE()
				WHERE COM_CODE = @COM_CODE
					AND NO = @NO
			END TRY

			BEGIN CATCH
				SET @error_code = ERROR_MESSAGE()

				RETURN
			END CATCH

			--cal ACTUAL_RESTIME_CL
			--CLOSE_DATE = DEPLOY_QA = RESPONSE_DATE
			--ISSUE_DATE = RESPONSE_DATE = ISSUE_DATE
			SELECT @DD = DATEDIFF(DD, ISSUE_DATE, CLOSE_DATE)
				,@WK = DATEDIFF(WK, ISSUE_DATE, CLOSE_DATE)
				,@H_ISS = CASE DATEPART(HH, ISSUE_DATE)
					WHEN 12
						THEN 0
					WHEN 13
						THEN 1
					WHEN 14
						THEN 2
					WHEN 15
						THEN 3
					WHEN 16
						THEN 4
					WHEN 17
						THEN 5
					WHEN 18
						THEN 6
					WHEN 19
						THEN 7
					WHEN 20
						THEN 8
					WHEN 21
						THEN 9
					WHEN 22
						THEN 10
					WHEN 23
						THEN 11
					WHEN 24
						THEN 0
					ELSE DATEPART(HH, ISSUE_DATE)
					END
				,@MI_ISS = DATEPART(MI, ISSUE_DATE)
				,@H_RES = CASE DATEPART(HH, CLOSE_DATE)
					WHEN 12
						THEN 0
					WHEN 13
						THEN 1
					WHEN 14
						THEN 2
					WHEN 15
						THEN 3
					WHEN 16
						THEN 4
					WHEN 17
						THEN 5
					WHEN 18
						THEN 6
					WHEN 19
						THEN 7
					WHEN 20
						THEN 8
					WHEN 21
						THEN 9
					WHEN 22
						THEN 10
					WHEN 23
						THEN 11
					WHEN 24
						THEN 0
					ELSE DATEPART(HH, CLOSE_DATE)
					END
				,@MI_RES = DATEPART(MI, CLOSE_DATE)
			FROM VSMS_ISSUE
			WHERE COM_CODE = @COM_CODE
				AND NO = @NO

			IF @MI_RES < @MI_ISS
			BEGIN
				SET @MI_RES += 60
			END

			IF @H_RES < @H_ISS
			BEGIN
				SET @H_RES += 12
			END

			SET @WK *= 2

			SELECT @HOLIDAY = COUNT(*)
			FROM VSMS_DEPLOY
			WHERE MONTH >= (
					SELECT DATEPART(MM, (
								SELECT ISSUE_DATE
								FROM VSMS_ISSUE
								WHERE NO = @NO
									AND COM_CODE = 'VSM'
								))
					)
				AND MONTH <= (
					SELECT DATEPART(MM, GETDATE())
					)
				AND YEAR = (
					SELECT DATEPART(YYYY, (
								SELECT ISSUE_DATE
								FROM VSMS_ISSUE
								WHERE NO = @NO
									AND COM_CODE = 'VSM'
								))
					)
				AND TYPE_DAY = 'H'
				AND COM_CODE = 'VSM'

			SET @DD -= @WK - @HOLIDAY -- DD DAY รวมกับวันหยุดประจำปี
			SET @MI = @MI_RES - @MI_ISS -- MI MINS
			SET @HH = @H_RES - @H_ISS -- HH HRS

			BEGIN TRY
				UPDATE VSMS_ISSUE
				SET ACTUAL_RESTIME_CL = CONCAT (
						@DD
						,' day '
						,@HH
						,' hrs '
						,@MI
						,' mins'
						)
				WHERE COM_CODE = @COM_CODE
					AND NO = @NO

				SET @error_code = 0
			END TRY

			BEGIN CATCH
				SET @error_code = ERROR_MESSAGE()

				RETURN
			END CATCH
				--end
		END
		ELSE
		BEGIN
			IF ISNULL((
						SELECT FILE_ID
						FROM VSMS_ISSUE
						WHERE COM_CODE = @COM_CODE
							AND NO = @NO
						), ' ') <> ' '
			BEGIN
				IF ISNULL((
							SELECT DEPLOY_QA
							FROM VSMS_ISSUE
							WHERE COM_CODE = @COM_CODE
								AND NO = @NO
							), ' ') <> ' '
				BEGIN
					IF ISNULL((
								SELECT DEPLOY_PD
								FROM VSMS_ISSUE
								WHERE COM_CODE = @COM_CODE
									AND NO = @NO
								), ' ') <> ' '
					BEGIN
						BEGIN TRY
							UPDATE VSMS_ISSUE
							SET STATUS = 'G'
								,MNT_BY = @CRET_BY
								,MNT_DATE = GETDATE()
							WHERE COM_CODE = @COM_CODE
								AND NO = @NO

							SET @error_code = 0
						END TRY

						BEGIN CATCH
							SET @error_code = ERROR_MESSAGE()

							RETURN
						END CATCH

						BEGIN TRY
							UPDATE VSMS_ISSTATOPSS
							SET ISE_STATUS = 'G'
								,ISE_DATE_GOLIVE = GETDATE()
								,MNT_BY = @CRET_BY
								,MNT_DATE = GETDATE()
							WHERE COM_CODE = @COM_CODE
								AND ISE_KEY = @ISE_KEY

							SET @error_code = 0
						END TRY

						BEGIN CATCH
							SET @error_code = ERROR_MESSAGE()

							RETURN
						END CATCH
					END
					ELSE
					BEGIN
						SET @error_code = 'Time Stemp Deploy PD Date :('
					END
				END
				ELSE
				BEGIN
					SET @error_code = 'Time Stemp Deploy QA Date :('
				END
			END
			ELSE
			BEGIN
				SET @error_code = 'Add File Packet First!'
			END
		END
	END
	ELSE IF @FALG = 'S'
	BEGIN
		BEGIN TRY
			UPDATE VSMS_ISSTATOPSS
			SET ISE_STATUS = 'S'
				,ISE_DATE_CLOSE = GETDATE()
				,MNT_BY = @CRET_BY
				,MNT_DATE = GETDATE()
			WHERE COM_CODE = @COM_CODE
				AND ISE_KEY = @ISE_KEY

			SET @error_code = 0
		END TRY

		BEGIN CATCH
			SET @error_code = ERROR_MESSAGE()

			RETURN
		END CATCH

		BEGIN TRY
			UPDATE VSMS_ISSUE
			SET STATUS = 'S'
				,CLOSE_DATE = GETDATE()
				,MNT_BY = @CRET_BY
				,MNT_DATE = GETDATE()
			WHERE COM_CODE = @COM_CODE
				AND NO = @NO
		END TRY

		BEGIN CATCH
			SET @error_code = ERROR_MESSAGE()

			RETURN
		END CATCH

		--cal ACTUAL_RESTIME_CL
		--CLOSE_DATE = DEPLOY_QA = RESPONSE_DATE
		--ISSUE_DATE = RESPONSE_DATE = ISSUE_DATE
		SELECT @DD = DATEDIFF(DD, ISSUE_DATE, CLOSE_DATE)
			,@WK = DATEDIFF(WK, ISSUE_DATE, CLOSE_DATE)
			,@H_ISS = CASE DATEPART(HH, ISSUE_DATE)
				WHEN 12
					THEN 0
				WHEN 13
					THEN 1
				WHEN 14
					THEN 2
				WHEN 15
					THEN 3
				WHEN 16
					THEN 4
				WHEN 17
					THEN 5
				WHEN 18
					THEN 6
				WHEN 19
					THEN 7
				WHEN 20
					THEN 8
				WHEN 21
					THEN 9
				WHEN 22
					THEN 10
				WHEN 23
					THEN 11
				WHEN 24
					THEN 0
				ELSE DATEPART(HH, ISSUE_DATE)
				END
			,@MI_ISS = DATEPART(MI, ISSUE_DATE)
			,@H_RES = CASE DATEPART(HH, CLOSE_DATE)
				WHEN 12
					THEN 0
				WHEN 13
					THEN 1
				WHEN 14
					THEN 2
				WHEN 15
					THEN 3
				WHEN 16
					THEN 4
				WHEN 17
					THEN 5
				WHEN 18
					THEN 6
				WHEN 19
					THEN 7
				WHEN 20
					THEN 8
				WHEN 21
					THEN 9
				WHEN 22
					THEN 10
				WHEN 23
					THEN 11
				WHEN 24
					THEN 0
				ELSE DATEPART(HH, CLOSE_DATE)
				END
			,@MI_RES = DATEPART(MI, CLOSE_DATE)
		FROM VSMS_ISSUE
		WHERE COM_CODE = @COM_CODE
			AND NO = @NO

		IF @MI_RES < @MI_ISS
		BEGIN
			SET @MI_RES += 60
		END

		IF @H_RES < @H_ISS
		BEGIN
			SET @H_RES += 12
		END

		SET @WK *= 2

		SELECT @HOLIDAY = COUNT(*)
		FROM VSMS_DEPLOY
		WHERE MONTH >= (
				SELECT DATEPART(MM, (
							SELECT ISSUE_DATE
							FROM VSMS_ISSUE
							WHERE NO = @NO
								AND COM_CODE = 'VSM'
							))
				)
			AND MONTH <= (
				SELECT DATEPART(MM, GETDATE())
				)
			AND YEAR = (
				SELECT DATEPART(YYYY, (
							SELECT ISSUE_DATE
							FROM VSMS_ISSUE
							WHERE NO = @NO
								AND COM_CODE = 'VSM'
							))
				)
			AND TYPE_DAY = 'H'
			AND COM_CODE = 'VSM'

		SET @DD -= @WK - @HOLIDAY -- DD DAY รวมกับวันหยุดประจำปี
		SET @MI = @MI_RES - @MI_ISS -- MI MINS
		SET @HH = @H_RES - @H_ISS -- HH HRS

		BEGIN TRY
			UPDATE VSMS_ISSUE
			SET ACTUAL_RESTIME_CL = CONCAT (
					@DD
					,' day '
					,@HH
					,' hrs '
					,@MI
					,' mins'
					)
			WHERE COM_CODE = @COM_CODE
				AND NO = @NO

			SET @error_code = 0
		END TRY

		BEGIN CATCH
			SET @error_code = ERROR_MESSAGE()

			RETURN
		END CATCH
			--end
	END
END
GO


