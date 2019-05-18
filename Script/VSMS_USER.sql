USE [SDDB]
GO

/****** Object:  StoredProcedure [dbo].[testEncodePW]    Script Date: 5/18/2019 1:08:46 AM ******/
DROP PROCEDURE IF EXISTS [dbo].[testEncodePW]
GO

/****** Object:  StoredProcedure [dbo].[testEncodePW]    Script Date: 5/18/2019 1:08:46 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO





CREATE PROCEDURE [dbo].[testEncodePW] @error_code VARCHAR(200) OUTPUT
,@USER_PWD CHAR(16)
,@USER_ID CHAR(16)
AS
begin
	--create table #data (
	--id			varchar(16)		null,
	--pw			varchar(35)		null
	--)

	--insert into #data (id,pw) values ('number',CONVERT(VARCHAR(35), HashBytes('MD5', '123456'), 2))
	--insert into #data (id,pw) values ('char',CONVERT(VARCHAR(35), HashBytes('MD5', 'asdfgh'), 2))

	--DECLARE @i_id char(20) = 'number',
		
	--@i_pw char(35) = 'asdfgh'

	--DECLARE @c_id char(20),
	--@c_pw char(35)

	--select * from #data
	--select  CONVERT(VARCHAR(35), HashBytes('MD5', 'asdfgh'), 2) number
	--select count(*) found from #data where pw = CONVERT(VARCHAR(35), HashBytes('MD5', 'asdfgh'), 2)


	select * from VSMS_USER --insert uer pw
	where USER_ID = LTRIM(RTRIM(@USER_ID)) and USER_PWD = CONVERT(VARCHAR(35), HashBytes('MD5', LTRIM(RTRIM(@USER_PWD))), 2)

end
GO


