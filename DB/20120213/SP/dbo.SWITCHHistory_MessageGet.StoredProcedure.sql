USE [NavIntegrationDB]
GO
/****** Object:  StoredProcedure [dbo].[SWITCHHistory_MessageGet]    Script Date: 02/13/2012 17:17:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SWITCHHistory_MessageGet] 

	@param_intHistoryID		int	

AS
BEGIN

	SET NOCOUNT ON;

	SELECT [MessageID]
		  ,[HistoryID]
		  ,[Message]
	FROM [NavIntegrationDB].[dbo].[SwitchHistoryMessages]
	Where [HistoryID] = @param_intHistoryID

END
GO
