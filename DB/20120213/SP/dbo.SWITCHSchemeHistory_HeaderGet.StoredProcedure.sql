USE [NavIntegrationDB]
GO
/****** Object:  StoredProcedure [dbo].[SWITCHSchemeHistory_HeaderGet]    Script Date: 02/13/2012 17:17:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SWITCHSchemeHistory_HeaderGet] 

	@param_strSchemeID		nvarchar(50)
	,@param_intSwitchID		int	

AS
BEGIN

	SET NOCOUNT ON;

	SELECT	[HistoryID]
			,[SchemeID]
			,[SwitchID]
			,[Action_Date]
			,[Status]
	FROM	[NavIntegrationDB].[dbo].[SwitchSchemeHistoryHeader]
	WHERE	[SchemeID] = @param_strSchemeID
			AND [SwitchID] = @param_intSwitchID
	ORDER BY [HistoryID] desc

END
GO
