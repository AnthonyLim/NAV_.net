USE [NavIntegrationDB]
GO
/****** Object:  StoredProcedure [dbo].[SWITCHScheme_SchemeContributionsGet]    Script Date: 02/13/2012 17:17:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SWITCHScheme_SchemeContributionsGet] 

@param_SchemeID	nvarchar(50)

AS
BEGIN

	SET NOCOUNT ON;

SELECT [ClientID]
      ,[SchemeID]
      ,[ContributionID]
      ,[StartDate]
      ,[EndDate]
      ,[ContributionAmount]
      ,[ValuationFrequency]
      ,[SchemeContributionsUpdatedDate]
      ,[SchemeContributionsUpdatedBy]
      ,[OLDeleted]
      ,[IFAUpdatedDate]
      ,[IFAUpdatedBy]
FROM [NavGlobalDBwwwGUID].[dbo].[SchemeContributions]

WHERE SchemeID = @param_SchemeID and OLDeleted = 0 

ORDER BY ContributionID 

END
GO
