USE [SDDB]
GO

DECLARE	@return_value int,
		@error_code varchar(200)

EXEC	@return_value = [bond].[SP_VSMS_ISSUE_004]
		@error_code = @error_code OUTPUT,
		@COM_CODE = N'CMAH',
		@NO = 1,
		@ISSUE_DATE = N'2019-01-01',
		@ISSUE_DATE_PERIOD = N'2019-01',
		@MODULE = N'a',
		@DETAIL = N'a',
		@ROOT_CAUSE = N'a',
		@SOLUTION = N'a',
		@EFFECTS = N'a',
		@ISSUE_BY = N'A',
		@RESPONSE_BY = N'a',
		@RESPONSE_DATE = N'2019-01-01',
		@RESPONSE_TARGET = N'2 Business Hrs.',
		@RESOLUTION_TARGET = N'4 Business Hrs.',
		@ESSR_NO = N'a',
		@ISSUE_TYPE = N'S9999',
		@DEFECT = N'User Friendly',
		@PRIORITY = N'Critical',
		@REMARK = N'a',
		@MAN_PLM_SA = 9,
		@MAN_PLM_QA = 2,
		@MAN_PLM_PRG = 2,
		@MAN_PLM_PL = 2,
		@MAN_PLM_DBA = N'2',
		@CRET_BY = N'Meiio',
		@CRET_DATE = N'2019-01-01',
		@REF_NO = NULL,
		@CLOSE_DATE = N'2019-01-01',
		@DEPLOY_QA = N'2019-01-01',
		@DEPLOY_PD = N'2019-01-01',
		@ISS_TYPE = N'S',
		@ISS_YEAR = N'9999',
		@TARGET_DATE = N'2019-01-01'

SELECT	@error_code as N'@error_code'

SELECT	'Return Value' = @return_value

GO
