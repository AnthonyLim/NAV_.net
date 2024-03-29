USE [NavIntegrationDB]
GO
/****** Object:  StoredProcedure [dbo].[SWITCHHistory_HeaderGet]    Script Date: 02/13/2012 17:17:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SWITCHHistory_HeaderGet] 

	@param_strPortfolioID	nvarchar(50)
	,@param_intSwitchID		int	

AS
BEGIN

	SET NOCOUNT ON;

	SELECT	[HistoryID]
			,[PortfolioID]
			,[SwitchID]
			,[Action_Date]
			,[Status]
	FROM	[NavIntegrationDB].[dbo].[SwitchHistoryHeader]
	WHERE	[PortfolioID] = @param_strPortfolioID
			AND [SwitchID] = @param_intSwitchID
	ORDER BY [HistoryID] desc

END
GO
