USE [NavIntegrationDB]
GO
/****** Object:  StoredProcedure [dbo].[SWITCHSchemeClient_DetailsGet]    Script Date: 02/13/2012 17:17:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SWITCHSchemeClient_DetailsGet] 

@param_intSwitchID int
,@param_isContribution	smallint

AS
BEGIN

	SET NOCOUNT ON;
	
SELECT [SwitchDetailsID]
      ,[SwitchID]
      ,[FundID]
      ,[Allocation]
      ,[Date_Created]
      ,[Created_By]
      ,[Date_LastUpdate]
      ,[Updated_By]
      ,[isDeletable]
      ,[isContribution]
  FROM [NavIntegrationDB].[dbo].[SwitchSchemeDetails_Client]
  WHERE SwitchID = @param_intSwitchID
		And [isContribution] = @param_isContribution

END
GO
