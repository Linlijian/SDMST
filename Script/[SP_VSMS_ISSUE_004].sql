--WARNING! ERRORS ENCOUNTERED DURING SQL PARSING!
USE [SDDB]
GO

/****** Object:  StoredProcedure [bond].[SP_VSMS_ISSUE_004]    Script Date: 5/21/2019 1:05:34 PM ******/
DROP PROCEDURE

IF EXISTS [bond].[SP_VSMS_ISSUE_004]
	/****** Object:  StoredProcedure [bond].[SP_VSMS_ISSUE_004]    Script Date: 5/21/2019 1:05:34 PM ******/
	SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

/*
		------------------------------------------------------------------------------------
		|																					|
		|    [VSMS_ISSUE]																	|
		|																					|
		------------------------------------------------------------------------------------
		| history			         										                |
		------------------------------------------------------------------------------------
		| modify by | version   |   date    | action comment						        |
		------------------------------------------------------------------------------------
          A	       1.0.0	 2019-2-6   Create procedure  FOR UPDATE 
		  A	       1.0.1	 2019-3-9   change locig
		  A	       1.2.0	 2019-3-23  Fix insert vsms_module
		  A	       1.3.0	 2019-4-9   add cal close date, deploy qa + pd 
		  A	       1.4.0	 2019-4-10  ADD LOG, INSERT VSMS_ISSTATOPSS
		  A	       1.4.1	 2019-4-21  add year MA
		  A	       1.4.2	 2019-4-27  fix com code table vsms_pit_data
		  A	       1.5.0	 2019-4-29  del VSMS_PRGASYS
		  A	       1.5.1	 2019-5-21  fix cal
		------------------------------------------------------------------------------------

*/
CREATE PROCEDURE [bond].[SP_VSMS_ISSUE_004] @error_code VARCHAR(200) OUTPUT
	,@COM_CODE CHAR(6)
	,@NO DECIMAL(18, 0)
	,@ISSUE_DATE DATETIME
	,@ISSUE_DATE_PERIOD VARCHAR(50)
	,@MODULE VARCHAR(600)
	,@DETAIL VARCHAR(2048)
	,@ROOT_CAUSE VARCHAR(2048)
	,@SOLUTION VARCHAR(2048)
	,@EFFECTS VARCHAR(2048)
	,@ISSUE_BY VARCHAR(50)
	,@RESPONSE_BY VARCHAR(50)
	,@RESPONSE_DATE DATETIME
	,@RESPONSE_TARGET VARCHAR(50)
	,@RESOLUTION_TARGET VARCHAR(50)
	,@ESSR_NO VARCHAR(400)
	,@ISSUE_TYPE VARCHAR(50)
	,@DEFECT VARCHAR(50)
	,@PRIORITY VARCHAR(50)
	,@REMARK VARCHAR(2048)
	,@MAN_PLM_SA DECIMAL(8, 1)
	,@MAN_PLM_QA DECIMAL(8, 1)
	,@MAN_PLM_PRG DECIMAL(8, 1)
	,@MAN_PLM_PL DECIMAL(8, 1)
	,@MAN_PLM_DBA VARCHAR(10)
	,@CRET_BY CHAR(16)
	,@CRET_DATE DATETIME
	,@REF_NO DECIMAL(18, 0)
	,@CLOSE_DATE DATETIME
	,@DEPLOY_QA DATETIME
	,@DEPLOY_PD DATETIME
	,@ISS_TYPE VARCHAR(1) -- add  1.4.1
	,@ISS_YEAR VARCHAR(4) -- add  1.4.1
	,@TARGET_DATE DATETIME
AS
BEGIN
	/********************************* cal ma **********************************/
	DECLARE @MA DECIMAL(8, 1)
		,@SUM DECIMAL(8, 1)
		,@IS_FREE VARCHAR(1)
		,@X DECIMAL(8, 1)
		,@Y DECIMAL(8, 1)
		,@A DECIMAL(8, 1)
		,@B DECIMAL(8, 1)
		,@C DECIMAL(8, 1)

	/*A = MANDAY PRG OLD, B = MANDAY QA OLD, C = MANDAY SA OLD
	  X = MANDAY DBA OLD, Y = MANDAY PL OLD*/
	CREATE TABLE #Dataset (
		COM_CODE CHAR(6)
		,YEAR VARCHAR(4)
		,MANDAY_VAL DECIMAL(8, 1)
		,IS_USE CHAR(1)
		,CRET_BY CHAR(16)
		,CRET_DATE DATETIME
		,MNT_BY CHAR(16)
		,MNT_DATE DATETIME
		,LOG_TYPE CHAR(1)
		,LOG_BY CHAR(16)
		,LOG_DATE DATETIME
		)

	SELECT @A = MAN_PLM_PRG
		,@B = MAN_PLM_QA
		,@C = MAN_PLM_SA
		,@X = MAN_PLM_DBA
		,@Y = MAN_PLM_PL
	FROM VSMS_ISSUE
	WHERE COM_CODE = @COM_CODE
		AND NO = @NO

	SELECT @IS_FREE = IS_FREE
	FROM VSMS_PIT_DATA
	WHERE COM_CODE = 'VSM'
		AND ISSUE_TYPE = @DEFECT

	--SELECT @MA = MANDAY_VAL
	--FROM VSMS_MANDAY_T
	--WHERE COM_CODE = @COM_CODE
	--	AND YEAR = @ISS_YEAR --1.4.1
	DECLARE @IS_EMPTY_MA CHAR(1) = (
			SELECT COUNT(MANDAY_VAL)
			FROM VSMS_MANDAY_T
			WHERE COM_CODE = @COM_CODE
				AND YEAR = @ISS_YEAR --1.4.1
			)

	PRINT (@IS_FREE)

	IF @IS_EMPTY_MA = 0
		AND @ISS_YEAR <> '9999'
	BEGIN
		SET @error_code = 'Man/day MA is empty this Application Code: ' + @COM_CODE + 'Please re-check MA Waranty'

		RETURN
	END
	ELSE
	BEGIN
		IF @ISS_TYPE <> 'S'
		BEGIN
			IF @IS_FREE = 'N'
			BEGIN
				SELECT @MA = MANDAY_VAL
				FROM VSMS_MANDAY_T
				WHERE COM_CODE = @COM_CODE
					AND YEAR = @ISS_YEAR --1.5.1

				SET @SUM = (@MA + (@A + @B + @C + @X + @Y)) - (@MAN_PLM_PRG + @MAN_PLM_QA + @MAN_PLM_SA + @MAN_PLM_DBA + @MAN_PLM_PL)

				IF @SUM < 0
				BEGIN
					SET @error_code = 'Not enough Man/day MA this Application Code: ' + @COM_CODE + 'Please re-check MA Waranty'

					RETURN
				END
				ELSE
				BEGIN
					--UPDATE MA
					IF EXISTS (
							SELECT *
							FROM VSMS_MANDAY_T
							WHERE IS_USE = 'T'
								AND COM_CODE = @COM_CODE
								AND YEAR = @ISS_YEAR --1.4.1
							)
					BEGIN
						BEGIN TRY
							UPDATE VSMS_MANDAY_T
							SET MANDAY_VAL = @SUM
							WHERE COM_CODE = @COM_CODE
								AND YEAR = @ISS_YEAR --1.4.1

							SET @error_code = 0
						END TRY

						BEGIN CATCH
							SET @error_code = ERROR_MESSAGE()

							RETURN
						END CATCH
					END
					ELSE
					BEGIN
						BEGIN TRY
							UPDATE VSMS_MANDAY_T
							SET MANDAY_VAL = @SUM
								,IS_USE = 'T'
							WHERE COM_CODE = @COM_CODE
								AND YEAR = @ISS_YEAR --1.4.1

							--LOG
							DELETE
							FROM #Dataset

							INSERT INTO #Dataset (
								COM_CODE
								,YEAR
								,MANDAY_VAL
								,IS_USE
								,CRET_BY
								,CRET_DATE
								,MNT_BY
								,MNT_DATE
								,LOG_TYPE
								,LOG_BY
								,LOG_DATE
								)
							VALUES (
								@COM_CODE
								,(
									SELECT YEAR
									FROM VSMS_MANDAY_T
									WHERE COM_CODE = @COM_CODE
										AND YEAR = @ISS_YEAR --1.4.1
									)
								,(
									SELECT MANDAY_VAL
									FROM VSMS_MANDAY_T
									WHERE COM_CODE = @COM_CODE
										AND YEAR = @ISS_YEAR --1.4.1
									)
								,'T'
								,(
									SELECT CRET_BY
									FROM VSMS_MANDAY_T
									WHERE COM_CODE = @COM_CODE
										AND YEAR = @ISS_YEAR --1.4.1
									)
								,(
									SELECT CRET_DATE
									FROM VSMS_MANDAY_T
									WHERE COM_CODE = @COM_CODE
										AND YEAR = @ISS_YEAR --1.4.1
									)
								,(
									SELECT MNT_BY
									FROM VSMS_MANDAY_T
									WHERE COM_CODE = @COM_CODE
										AND YEAR = @ISS_YEAR --1.4.1
									)
								,(
									SELECT MNT_DATE
									FROM VSMS_MANDAY_T
									WHERE COM_CODE = @COM_CODE
										AND YEAR = @ISS_YEAR --1.4.1
									)
								,'B'
								,--BASELINE
								@CRET_BY
								,GETDATE()
								)

							INSERT INTO VSMS_MANDAY_T_LOG
							SELECT *
							FROM #Dataset

							SET @error_code = 0
						END TRY

						BEGIN CATCH
							SET @error_code = ERROR_MESSAGE()

							RETURN
						END CATCH
					END
				END
			END
			ELSE
			BEGIN
				BEGIN TRY
					UPDATE VSMS_MANDAY_T
					SET IS_USE = 'T'
					WHERE COM_CODE = @COM_CODE
						AND YEAR = @ISS_YEAR --1.4.1

					--LOG
					DELETE
					FROM #Dataset

					INSERT INTO #Dataset (
						COM_CODE
						,YEAR
						,MANDAY_VAL
						,IS_USE
						,CRET_BY
						,CRET_DATE
						,MNT_BY
						,MNT_DATE
						,LOG_TYPE
						,LOG_BY
						,LOG_DATE
						)
					VALUES (
						@COM_CODE
						,(
							SELECT YEAR
							FROM VSMS_MANDAY_T
							WHERE COM_CODE = @COM_CODE
								AND YEAR = @ISS_YEAR --1.4.1
							)
						,(
							SELECT MANDAY_VAL
							FROM VSMS_MANDAY_T
							WHERE COM_CODE = @COM_CODE
								AND YEAR = @ISS_YEAR --1.4.1
							)
						,'T'
						,(
							SELECT CRET_BY
							FROM VSMS_MANDAY_T
							WHERE COM_CODE = @COM_CODE
								AND YEAR = @ISS_YEAR --1.4.1
							)
						,(
							SELECT CRET_DATE
							FROM VSMS_MANDAY_T
							WHERE COM_CODE = @COM_CODE
								AND YEAR = @ISS_YEAR --1.4.1
							)
						,(
							SELECT MNT_BY
							FROM VSMS_MANDAY_T
							WHERE COM_CODE = @COM_CODE
								AND YEAR = @ISS_YEAR --1.4.1
							)
						,(
							SELECT MNT_DATE
							FROM VSMS_MANDAY_T
							WHERE COM_CODE = @COM_CODE
								AND YEAR = @ISS_YEAR --1.4.1
							)
						,'B'
						,--BASELINE
						@CRET_BY
						,GETDATE()
						)

					INSERT INTO VSMS_MANDAY_T_LOG
					SELECT *
					FROM #Dataset

					SET @error_code = 0
				END TRY

				BEGIN CATCH
					SET @error_code = ERROR_MESSAGE()

					RETURN
				END CATCH
			END
		END
		ELSE
		BEGIN
			SELECT @MA = MANDAY_VAL
			FROM VSMS_MANDAY_T
			WHERE COM_CODE = @COM_CODE
				AND YEAR = (
					SELECT SUBSTRING(issue_type, 2, 5)
					FROM vsms_issue
					WHERE COM_CODE = @COM_CODE
						AND no = @NO
					)

			SET @SUM = (@MA + (@A + @B + @C + @X + @Y))

			BEGIN TRY
				UPDATE VSMS_MANDAY_T
				SET IS_USE = 'T'
					,MANDAY_VAL = @SUM --1.5.1
				WHERE COM_CODE = @COM_CODE
					AND YEAR = (
						SELECT SUBSTRING(issue_type, 2, 5)
						FROM vsms_issue
						WHERE COM_CODE = @COM_CODE
							AND no = @NO
						)

				SET @error_code = 0
			END TRY

			BEGIN CATCH
				SET @error_code = ERROR_MESSAGE()

				RETURN
			END CATCH
		END
	END

	/*********************************** end ***********************************/
	/********************* insert VSMS_ISSTATOPSS follow up*********************/
	BEGIN TRY
		DECLARE @CONS VARCHAR(1)

		SELECT @CONS = IS_CONS
		FROM VSMS_PIT_DATA
		WHERE ISSUE_TYPE = @DEFECT
			AND COM_CODE = 'VSM'

		IF @CONS = 'Y'
		BEGIN
			IF EXISTS (
					SELECT *
					FROM VSMS_ISSTATOPSS
					WHERE COM_CODE = @COM_CODE
						AND ISE_NO = @NO
					)
			BEGIN
				UPDATE VSMS_ISSTATOPSS
				SET ISE_STATUS = 'F'
					,ASSIGN_STATUS = 'E'
					,ISE_DATE_FOLLOWUP = GETDATE()
					,ISE_DATE_DONE = GETDATE()
					,MNT_BY = @CRET_BY
					,MNT_DATE = GETDATE()
				WHERE COM_CODE = @COM_CODE
					AND ISE_NO = @NO

				UPDATE VSMS_ISSUE
				SET STATUS = 'F'
					,MNT_BY = @CRET_BY
					,MNT_DATE = GETDATE()
				WHERE COM_CODE = @COM_CODE
					AND NO = @NO
			END
			ELSE
			BEGIN
				INSERT INTO VSMS_ISSTATOPSS (
					[COM_CODE]
					,[ISE_NO]
					,[ISE_STATUS]
					,[USER_ID]
					,[ASSIGN_STATUS]
					,[ISE_DATE_OPENING]
					,[ISE_DATE_ONPROCESS]
					,[ISE_DATE_FOLLOWUP]
					,[ISE_DATE_GOLIVE]
					,[ISE_DATE_CLOSE]
					,[ISE_DATE_CANCEL]
					,[ISE_DATE_WAITING]
					,[ISE_DATE_DOING]
					,[ISE_DATE_DONE]
					,[ISE_DATE_TESTING]
					,[CRET_BY]
					,[CRET_DATE]
					,[MNT_BY]
					,[MNT_DATE]
					)
				VALUES (
					@COM_CODE
					,@NO
					,(
						SELECT FIXOPT_D_CODE
						FROM [bond].[DDL_DATA]
						WHERE FIXOPT_H_ID = 115
							AND FIXOPT_D_CODE = 'F'
						)
					,@CRET_BY
					,(
						SELECT FIXOPT_D_CODE
						FROM [bond].[DDL_DATA]
						WHERE FIXOPT_H_ID = 117
							AND FIXOPT_D_CODE = 'E'
						)
					,getdate()
					,NULL
					,getdate()
					,NULL
					,NULL
					,NULL
					,getdate()
					,NULL
					,NULL
					,NULL
					,@CRET_BY
					,@CRET_DATE
					,@CRET_BY
					,@CRET_DATE
					)
			END

			SET @error_code = 0
		END
	END TRY

	BEGIN CATCH
		SET @error_code = ERROR_MESSAGE()
	END CATCH

	/*********************************** end ***********************************/
	/******************************* update issue ******************************/
	BEGIN TRY
		UPDATE VSMS_ISSUE
		SET [ISSUE_DATE] = @ISSUE_DATE
			,[REF_NO] = @REF_NO
			,[ISSUE_DATE_PERIOD] = @ISSUE_DATE_PERIOD
			,[MODULE] = @MODULE
			,[DETAIL] = @DETAIL
			,[ROOT_CAUSE] = @ROOT_CAUSE
			,[SOLUTION] = @SOLUTION
			,[EFFECTS] = @EFFECTS
			,[ISSUE_BY] = @ISSUE_BY
			,[RESPONSE_BY] = @RESPONSE_BY
			,[RESPONSE_DATE] = @RESPONSE_DATE
			,[RESPONSE_TARGET] = @RESPONSE_TARGET
			,[RESOLUTION_TARGET] = @RESOLUTION_TARGET
			,[DEPLOY_QA] = @DEPLOY_QA
			,[DEPLOY_PD] = @DEPLOY_PD
			,[CLOSE_DATE] = @CLOSE_DATE
			,[ESSR_NO] = @ESSR_NO
			,[ISSUE_TYPE] = @ISSUE_TYPE
			,[DEFECT] = @DEFECT
			,[PRIORITY] = @PRIORITY
			,[REMARK] = @REMARK
			,[MAN_PLM_SA] = @MAN_PLM_SA
			,[MAN_PLM_QA] = @MAN_PLM_QA
			,[MAN_PLM_PRG] = @MAN_PLM_PRG
			,[MAN_PLM_PL] = @MAN_PLM_PL
			,[MAN_PLM_DBA] = @MAN_PLM_DBA
			,[MNT_BY] = @CRET_BY
			,[MNT_DATE] = @CRET_DATE
			,[TARGET_DATE] = @TARGET_DATE
		WHERE COM_CODE = @COM_CODE
			AND NO = @NO

		SET @error_code = 0
	END TRY

	BEGIN CATCH
		SET @error_code = ERROR_MESSAGE()

		RETURN
	END CATCH

	/*********************************** end *******************************/
	/************************ INSERT VSMS_ISSTATOPSS ***********************/
	--DECLARE @COUNT CHAR(1) = (
	--		SELECT count(*)
	--		FROM VSMS_ISSTATOPSS
	--		WHERE com_code = @COM_CODE
	--			AND USER_ID = @RESPONSE_BY
	--			AND ISE_NO = @no
	--		)
	--IF @COUNT = 0
	--BEGIN
	--	BEGIN TRY
	--		UPDATE VSMS_ISSTATOPSS
	--		SET [ASSIGN_STATUS] = (
	--				SELECT FIXOPT_D_CODE
	--				FROM [bond].[DDL_DATA]
	--				WHERE FIXOPT_H_ID = 115
	--					AND FIXOPT_D_CODE = 'c'
	--				)
	--			,[ISE_DATE_CANCEL] = getdate()
	--			,[MNT_BY] = @CRET_BY
	--			,[MNT_DATE] = @CRET_DATE
	--		WHERE com_code = @COM_CODE
	--			AND USER_ID = @RESPONSE_BY
	--			AND ISE_NO = @no
	--		SET @error_code = 0
	--	END TRY
	--	BEGIN CATCH
	--		SET @error_code = ERROR_MESSAGE()
	--		RETURN
	--	END CATCH
	--	BEGIN TRY
	--		INSERT INTO VSMS_ISSTATOPSS (
	--			[COM_CODE]
	--			,[ISE_NO]
	--			,[ISE_STATUS]
	--			,[USER_ID]
	--			,[ASSIGN_STATUS]
	--			,[ISE_DATE_OPENING]
	--			,[ISE_DATE_ONPROCESS]
	--			,[ISE_DATE_FOLLOWUP]
	--			,[ISE_DATE_GOLIVE]
	--			,[ISE_DATE_CLOSE]
	--			,[ISE_DATE_CANCEL]
	--			,[ISE_DATE_WAITING]
	--			,[ISE_DATE_DOING]
	--			,[ISE_DATE_DONE]
	--			,[ISE_DATE_TESTING]
	--			,[CRET_BY]
	--			,[CRET_DATE]
	--			,[MNT_BY]
	--			,[MNT_DATE]
	--			)
	--		VALUES (
	--			@COM_CODE
	--			,@NO
	--			,(
	--				SELECT FIXOPT_D_CODE
	--				FROM [bond].[DDL_DATA]
	--				WHERE FIXOPT_H_ID = 115
	--					AND FIXOPT_D_CODE = 'o'
	--				)
	--			,@RESPONSE_BY
	--			,(
	--				SELECT FIXOPT_D_CODE
	--				FROM [bond].[DDL_DATA]
	--				WHERE FIXOPT_H_ID = 117
	--					AND FIXOPT_D_CODE = 'W'
	--				)
	--			,getdate()
	--			,NULL
	--			,NULL
	--			,NULL
	--			,NULL
	--			,NULL
	--			,getdate()
	--			,NULL
	--			,NULL
	--			,NULL
	--			,@CRET_BY
	--			,@CRET_DATE
	--			,@CRET_BY
	--			,@CRET_DATE
	--			)
	--		SET @error_code = 0
	--	END TRY
	--	BEGIN CATCH
	--		SET @error_code = ERROR_MESSAGE()
	--		RETURN
	--	END CATCH
	--END
	--ELSE
	--BEGIN
	--	UPDATE VSMS_ISSTATOPSS
	--	SET [ASSIGN_STATUS] = (
	--			SELECT FIXOPT_D_CODE
	--			FROM [bond].[DDL_DATA]
	--			WHERE FIXOPT_H_ID = 117
	--				AND FIXOPT_D_CODE = 'W'
	--			)
	--		,[ISE_DATE_CANCEL] = getdate()
	--		,[MNT_BY] = @CRET_BY
	--		,[MNT_DATE] = @CRET_DATE
	--	WHERE com_code = @COM_CODE
	--		AND USER_ID = @RESPONSE_BY
	--		AND ISE_NO = @no
	--	UPDATE VSMS_ISSTATOPSS
	--	SET [ASSIGN_STATUS] = (
	--			SELECT FIXOPT_D_CODE
	--			FROM [bond].[DDL_DATA]
	--			WHERE FIXOPT_H_ID = 115
	--				AND FIXOPT_D_CODE = 'C'
	--			)
	--		,[ISE_DATE_CANCEL] = getdate()
	--		,[MNT_BY] = @CRET_BY
	--		,[MNT_DATE] = @CRET_DATE
	--	WHERE com_code = @COM_CODE
	--		AND USER_ID != @RESPONSE_BY
	--		AND ISE_NO = @no
	--END
	/*********************************** end *******************************/
	/******************************** cal 1.3.0 ****************************/
	DECLARE @MI DECIMAL(6, 0)
		,@WK DECIMAL(6, 0)
		,@DD DECIMAL(6, 0)
		,@HH DECIMAL(6, 0)
		,@H_ISS DECIMAL(6, 0)
		,@MI_ISS DECIMAL(6, 0)
		,@H_RES DECIMAL(6, 0)
		,@MI_RES DECIMAL(6, 0)
	DECLARE @HOLIDAY DECIMAL(6, 0)

	IF ISNULL(@RESPONSE_DATE, '') <> ''
	BEGIN
		IF ISNULL(@DEPLOY_QA, '') <> ''
		BEGIN
			SELECT @DD = DATEDIFF(DD, RESPONSE_DATE, DEPLOY_QA)
				,@WK = DATEDIFF(WK, RESPONSE_DATE, DEPLOY_QA)
				,@H_ISS = CASE DATEPART(HH, RESPONSE_DATE)
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
					ELSE DATEPART(HH, RESPONSE_DATE)
					END
				,@MI_ISS = DATEPART(MI, RESPONSE_DATE)
				,@H_RES = CASE DATEPART(HH, DEPLOY_QA)
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
					ELSE DATEPART(HH, DEPLOY_QA)
					END
				,@MI_RES = DATEPART(MI, DEPLOY_QA)
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
				SET ACTUAL_RESTIME_QA = CONCAT (
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
		END

		IF ISNULL(@DEPLOY_PD, '') <> ''
		BEGIN
			SELECT @DD = DATEDIFF(DD, RESPONSE_DATE, DEPLOY_PD)
				,@WK = DATEDIFF(WK, RESPONSE_DATE, DEPLOY_PD)
				,@H_ISS = CASE DATEPART(HH, RESPONSE_DATE)
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
					ELSE DATEPART(HH, RESPONSE_DATE)
					END
				,@MI_ISS = DATEPART(MI, RESPONSE_DATE)
				,@H_RES = CASE DATEPART(HH, DEPLOY_PD)
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
					ELSE DATEPART(HH, DEPLOY_PD)
					END
				,@MI_RES = DATEPART(MI, DEPLOY_PD)
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
				SET ACTUAL_RESTIME_PD = CONCAT (
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
		END
	END

	IF ISNULL(@CLOSE_DATE, '') <> ''
	BEGIN
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
	END

	IF ISNULL(@RESPONSE_DATE, '') <> ''
	BEGIN
		SELECT @DD = DATEDIFF(DD, ISSUE_DATE, RESPONSE_DATE)
			,@WK = DATEDIFF(WK, ISSUE_DATE, RESPONSE_DATE)
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
			,@H_RES = CASE DATEPART(HH, RESPONSE_DATE)
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
				ELSE DATEPART(HH, RESPONSE_DATE)
				END
			,@MI_RES = DATEPART(MI, RESPONSE_DATE)
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
			SET ACTUAL_RESTIME = CONCAT (
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
	END
			/*********************************** end *******************************/
END
GO


