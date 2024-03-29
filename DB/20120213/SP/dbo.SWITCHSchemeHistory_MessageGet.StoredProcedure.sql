USE [NavIntegrationDB]
GO
/****** Object:  StoredProcedure [dbo].[SWITCHSchemeHistory_MessageGet]    Script Date: 02/13/2012 17:17:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SWITCHSchemeHistory_MessageGet] 

	@param_intHistoryID		int	

AS
BEGIN

	SET NOCOUNT ON;

	SELECT [MessageID]
		  ,[HistoryID]
		  ,[Message]
	FROM [NavIntegrationDB].[dbo].[SwitchSchemeHistoryMessages]
	Where [HistoryID] = @param_intHistoryID

END
GO
