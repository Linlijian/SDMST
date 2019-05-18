USE [SDDB]
GO

/****** Object:  StoredProcedure [bond].[SP_VSMS_ISSUE_R002]    Script Date: 5/17/2019 11:08:34 PM ******/
DROP PROCEDURE IF EXISTS [bond].[SP_VSMS_ISSUE_R002]
GO

/****** Object:  StoredProcedure [bond].[SP_VSMS_ISSUE_R002]    Script Date: 5/17/2019 11:08:34 PM ******/
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
          A	       1.0.0	 2019-05-02   Create procedure  FOR 5. A.Janaury19
		------------------------------------------------------------------------------------

*/
CREATE PROCEDURE [bond].[SP_VSMS_ISSUE_R002] @error_code VARCHAR(200) OUTPUT
	,@CRET_BY CHAR(16)
	,@ISSUE_DATE_PERIOD VARCHAR(50)
AS
BEGIN TRY
	/*************************** First setting data ****************************/
	CREATE TABLE #Dataset (
		PAGESHEET VARCHAR(50) COLLATE THAI_CI_AS NULL
		,COM_CODE CHAR(6) COLLATE THAI_CI_AS NULL
		,RL_DEFECT VARCHAR(50) COLLATE THAI_CI_AS NULL
		,RL_STATUS VARCHAR(50) COLLATE THAI_CI_AS NULL
		,RL_ISSTYPE VARCHAR(50) COLLATE THAI_CI_AS NULL
		)

	DECLARE @PAGESHEET VARCHAR(50)
		,@CUR_COM_CODE CHAR(6)
		,@CUR_STATUS CHAR(1)
		,@CUR_DEFECT VARCHAR(50)
		,@CUR_ISSUE_TYPE VARCHAR(50)
		,@RL_DEFECT VARCHAR(50)
		,@RL_STATUS VARCHAR(50)
		,@RL_ISSTYPE VARCHAR(50)

	--set page sheet 
	SET @PAGESHEET = 'A.' + (
			SELECT DATENAME(MONTH, CONVERT(DATE, CONCAT (
							@ISSUE_DATE_PERIOD
							,'-14'
							)))
			) + (
			SELECT substring(@ISSUE_DATE_PERIOD, 3, 2)
			)

	DECLARE CURSOR_REPORT CURSOR
	FOR
	SELECT COM_CODE
		,ISSUE_TYPE
		,DEFECT
		,STATUS
	FROM VSMS_ISSUE
	WHERE ISSUE_DATE_PERIOD >= (
			CONCAT (
				DATEPART(YYYY, CONVERT(DATE, CONCAT (
							@ISSUE_DATE_PERIOD
							,'-14'
							)))
				,'-01'
				)
			)
		AND ISSUE_DATE_PERIOD <= @ISSUE_DATE_PERIOD

	OPEN CURSOR_REPORT

	FETCH NEXT
	FROM CURSOR_REPORT
	INTO @CUR_COM_CODE
		,@CUR_ISSUE_TYPE
		,@CUR_DEFECT
		,@CUR_STATUS

	WHILE (@@FETCH_STATUS = 0)
	BEGIN
		--set defect
		IF (
				SELECT SUBSTRING(@CUR_ISSUE_TYPE, 1, 1)
				) = 'S'
		BEGIN
			SET @CUR_DEFECT = @CUR_DEFECT + ' (WT)'
		END

		--set status
		SET @RL_STATUS = (
				SELECT FIXOPT_D_NAME_EN
				FROM bond.DDL_DATA
				WHERE FIXOPT_H_ID = 115
					AND FIXOPT_D_CODE = @CUR_STATUS
				)
		--set isstype
		SET @RL_ISSTYPE = (
				SELECT FIXOPT_D_NAME_EN
				FROM bond.DDL_DATA
				WHERE FIXOPT_H_ID = 120
					AND FIXOPT_D_CODE = (
						SELECT SUBSTRING(@CUR_ISSUE_TYPE, 1, 1)
						)
				)

		INSERT INTO #Dataset (
			PAGESHEET
			,COM_CODE
			,RL_DEFECT
			,RL_STATUS
			,RL_ISSTYPE
			)
		VALUES (
			@PAGESHEET
			,ISNULL(@CUR_COM_CODE, ' ')
			,ISNULL(@CUR_DEFECT, '(blank)')
			,ISNULL(@RL_STATUS, ' ')
			,ISNULL(@RL_ISSTYPE, ' ')
			)

		FETCH NEXT
		FROM CURSOR_REPORT
		INTO @CUR_COM_CODE
			,@CUR_ISSUE_TYPE
			,@CUR_DEFECT
			,@CUR_STATUS
	END

	CLOSE CURSOR_REPORT

	DEALLOCATE CURSOR_REPORT

	/*********************************** end ***********************************/
	/***************************** setting report ******************************/
	CREATE TABLE #DRSet (
		PAGESHEET VARCHAR(50) COLLATE THAI_CI_AS NULL
		,COM_CODE CHAR(6) COLLATE THAI_CI_AS NULL
		,RL_DEFECT VARCHAR(50) COLLATE THAI_CI_AS NULL
		,RL_STATUS VARCHAR(50) COLLATE THAI_CI_AS NULL
		,RL_ISSTYPE VARCHAR(50) COLLATE THAI_CI_AS NULL
		,CL INT
		)

	--,CL_DEFECT INT
	--,CL_STATUS INT
	--,CL_ISSTYPE INT
	DECLARE @CL_DEFECT INT
		,@CL_STATUS INT
		,@CL_ISSTYPE INT

	DECLARE CURSOR_REPORT CURSOR
	FOR
	SELECT COM_CODE
		,RL_DEFECT
		,RL_ISSTYPE
		,RL_STATUS
	FROM #Dataset
	ORDER BY COM_CODE
		,RL_DEFECT

	OPEN CURSOR_REPORT

	FETCH NEXT
	FROM CURSOR_REPORT
	INTO @CUR_COM_CODE
		,@RL_DEFECT
		,@RL_ISSTYPE
		,@RL_STATUS

	WHILE (@@FETCH_STATUS = 0)
	BEGIN
		/**************************** com_code + defect ****************************/
		--cal RL_DEFECT
		IF EXISTS (
				SELECT *
				FROM #DRSet
				WHERE RL_DEFECT = @RL_DEFECT
					AND COM_CODE = @CUR_COM_CODE
					AND PAGESHEET = 'RL_DEFECT'
				)
		BEGIN
			SELECT @CL_DEFECT = CL
			FROM #DRSet
			WHERE COM_CODE = @CUR_COM_CODE
				AND RL_DEFECT = @RL_DEFECT
				AND PAGESHEET = 'RL_DEFECT'

			UPDATE #DRSet
			SET CL = @CL_DEFECT + 1
			WHERE COM_CODE = @CUR_COM_CODE
				AND RL_DEFECT = @RL_DEFECT
				AND PAGESHEET = 'RL_DEFECT'
		END
		ELSE
		BEGIN
			INSERT INTO #DRSet (
				PAGESHEET
				,COM_CODE
				,RL_DEFECT
				,CL
				)
			VALUES (
				'RL_DEFECT'
				,@CUR_COM_CODE
				,@RL_DEFECT
				,1
				)
		END

		/*********************************** end ***********************************/
		/*************************** com_code + isstype ****************************/
		--cal RL_ISSTYPE
		IF EXISTS (
				SELECT *
				FROM #DRSet
				WHERE RL_ISSTYPE = @RL_ISSTYPE
					AND COM_CODE = @CUR_COM_CODE
					AND RL_DEFECT = @RL_DEFECT
					AND RL_STATUS = @RL_STATUS
					AND PAGESHEET = 'RL_ISSTYPE'
				)
		BEGIN
			SELECT @CL_DEFECT = CL
			FROM #DRSet
			WHERE COM_CODE = @CUR_COM_CODE
				AND RL_DEFECT = @RL_DEFECT
				AND PAGESHEET = 'RL_ISSTYPE'

			UPDATE #DRSet
			SET CL = @CL_DEFECT + 1
			WHERE COM_CODE = @CUR_COM_CODE
				AND RL_ISSTYPE = @RL_ISSTYPE
				AND RL_DEFECT = @RL_DEFECT
				AND RL_STATUS = @RL_STATUS
				AND PAGESHEET = 'RL_ISSTYPE'
		END
		ELSE
		BEGIN
			INSERT INTO #DRSet (
				PAGESHEET
				,COM_CODE
				,RL_DEFECT
				,RL_ISSTYPE
				,RL_STATUS
				,CL
				)
			VALUES (
				'RL_ISSTYPE'
				,@CUR_COM_CODE
				,@RL_DEFECT
				,@RL_ISSTYPE
				,@RL_STATUS
				,1
				)
		END

		/*********************************** end ***********************************/
		/**************************** com_code + status ****************************/
		--cal RL_STATUS
		IF EXISTS (
				SELECT *
				FROM #DRSet
				WHERE RL_STATUS = @RL_STATUS
					AND COM_CODE = @CUR_COM_CODE
					AND RL_DEFECT = @RL_DEFECT
					AND PAGESHEET = 'RL_STATUS'
				)
		BEGIN
			SELECT @CL_DEFECT = CL
			FROM #DRSet
			WHERE COM_CODE = @CUR_COM_CODE
				AND RL_DEFECT = @RL_DEFECT
				AND PAGESHEET = 'RL_STATUS'

			UPDATE #DRSet
			SET CL = @CL_DEFECT + 1
			WHERE COM_CODE = @CUR_COM_CODE
				AND RL_STATUS = @RL_STATUS
				AND RL_DEFECT = @RL_DEFECT
				AND PAGESHEET = 'RL_STATUS'
		END
		ELSE
		BEGIN
			INSERT INTO #DRSet (
				PAGESHEET
				,COM_CODE
				,RL_STATUS
				,RL_DEFECT
				,RL_ISSTYPE
				,CL
				)
			VALUES (
				'RL_STATUS'
				,@CUR_COM_CODE
				,@RL_STATUS
				,@RL_DEFECT
				,@RL_ISSTYPE
				,1
				)
		END

		/*********************************** end ***********************************/
		FETCH NEXT
		FROM CURSOR_REPORT
		INTO @CUR_COM_CODE
			,@RL_DEFECT
			,@RL_ISSTYPE
			,@RL_STATUS
	END

	CLOSE CURSOR_REPORT

	DEALLOCATE CURSOR_REPORT

	--SELECT *
	--FROM #Dataset

	--SELECT *
	--FROM #Dataset
	--ORDER BY COM_CODE
	--	,RL_DEFECT

	DELETE
	FROM VSMS_ISSUER002T

	INSERT INTO VSMS_ISSUER002T (
		[COM_CODE]
		,[DEFECT]
		,STATUS
		,[ISSTYPE]
		,[CL]
		)
	SELECT COM_CODE
		,RL_DEFECT
		,ISNULL(RL_STATUS, ' ')
		,ISNULL(RL_ISSTYPE, '')
		,CL
	FROM #DRSet
	ORDER BY COM_CODE
		,RL_DEFECT

	--SELECT COM_CODE
	--	,RL_DEFECT
	--	,ISNULL(RL_STATUS, ' ')
	--	,ISNULL(RL_ISSTYPE, '')
	--	,CL
	--FROM #DRSet
	--ORDER BY COM_CODE
	--	,RL_DEFECT

	--if @CRET_BY = 'C' begin
	--SELECT COM_CODE
	--FROM #DRSet group by COM_CODE end else
	--if @CRET_BY = 'D' begin
	--SELECT COM_CODE
	--	,RL_DEFECT
	--	,CL
	--FROM #DRSet
	--WHERE PAGESHEET = 'rl_defect'

	----if @CRET_BY = 'S' begin
	--SELECT COM_CODE
	--	,RL_DEFECT
	--	,RL_STATUS
	--	,CL
	--FROM #DRSet
	--WHERE PAGESHEET = 'rl_status'

	----if @CRET_BY = 'I' begin
	--SELECT COM_CODE
	--	,RL_DEFECT
	--	,RL_STATUS
	--	,RL_ISSTYPE
	--	,CL
	--FROM #DRSet
	--WHERE PAGESHEET = 'rl_isstype'
		--end
		/*********************************** end ***********************************/
		/***************************** cal Grand Total *****************************/
		/*********************************** end ***********************************/
END TRY

BEGIN CATCH
	SET @error_code = ERROR_MESSAGE()
END CATCH
GO


