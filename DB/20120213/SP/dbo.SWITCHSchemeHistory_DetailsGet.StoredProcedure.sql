USE [NavIntegrationDB]
GO
/****** Object:  StoredProcedure [dbo].[SWITCHSchemeHistory_DetailsGet]    Script Date: 02/13/2012 17:17:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SWITCHSchemeHistory_DetailsGet] 

	@param_intHistoryID	int
	,@param_isContribution	smallint

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
			,[isContribution]
	FROM	[NavIntegrationDB].[dbo].[SwitchSchemeHistoryDetails]
	WHERE	[HistoryID] = @param_intHistoryID
			And [isContribution] = @param_isContribution

END
GO
