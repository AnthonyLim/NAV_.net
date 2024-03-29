USE [NavIntegrationDB]
GO
/****** Object:  StoredProcedure [dbo].[SWITCHHistory_DetailsGet]    Script Date: 02/13/2012 17:17:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SWITCHHistory_DetailsGet] 

	@param_intHistoryID	int

AS
BEGIN

	SET NOCOUNT ON;

	SELECT	[HistoryDetailsID]
			,[HistoryID]
			,[SwitchDetailsID]
			,[FundID]
			,[Allocation]
			,[Date_Created]
			,[Created_By]
	FROM	[NavIntegrationDB].[dbo].[SwitchHistoryDetails]
	WHERE	[HistoryID] = @param_intHistoryID

END
GO
