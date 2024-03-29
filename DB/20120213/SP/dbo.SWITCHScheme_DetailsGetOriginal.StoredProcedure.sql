USE [NavIntegrationDB]
GO
/****** Object:  StoredProcedure [dbo].[SWITCHScheme_DetailsGetOriginal]    Script Date: 02/13/2012 17:17:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SWITCHScheme_DetailsGetOriginal] 

@param_strClientID nvarchar(50),
@param_strSchemeID nvarchar(50)


AS
BEGIN

	SET NOCOUNT ON;
	
SELECT		[ClientID]
			,[SchemeID]
			,[FundName]
			,[NumberOfUnits]
			,[Price]
			,[Value]
			,[CurrentValueClient]
			,[CurrentValueScheme]
			,[ClientCurrency]
			,[FundExchangeRate]
			,[DatePriceUpdated]
			,[FundNameID]
			,[SEDOL]
			,[FundCurrency]
			,[SectorID]
			,[FundChoicePercentage]
			,[ContributionTotal]
			,[SchemeCurrency]
			,[ExchangeRate]
			,[SchemeStartDate]
			,[FundID]
			,[OLDeleted]
			,[TYPECODE]
			,[fundcode]
			,[Type]

FROM		[NavGlobalDBwwwGUID].[dbo].[SchemeDetails3]

WHERE		ClientID = @param_strClientID 
			and SchemeID = @param_strSchemeID 
			and OLDeleted = 0 

ORDER BY	FundName 

END
GO
