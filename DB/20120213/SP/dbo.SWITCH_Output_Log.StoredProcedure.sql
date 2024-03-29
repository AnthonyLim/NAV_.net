USE [NavIntegrationDB]
GO
/****** Object:  StoredProcedure [dbo].[SWITCH_Output_Log]    Script Date: 02/13/2012 17:17:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[SWITCH_Output_Log]
	-- Add the parameters for the stored procedure here
	@SwitchType varchar(50),
	@SwitchID int,
	@Filename varchar(255),
	@OutputType varchar(50)
AS
BEGIN
	insert into [NavIntegrationDB].[dbo].[Switch_Output](
			[SwitchType]
		  ,[SwitchID]
		  ,[DateCreated]
		  ,[FileName]
		  ,[OutputType])
	values(
		@SwitchType,
		@SwitchID,
		CURRENT_TIMESTAMP,
		@Filename,
		@OutputType
		)
END
GO
