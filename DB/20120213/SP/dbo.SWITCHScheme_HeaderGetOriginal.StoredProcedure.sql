USE [NavIntegrationDB]
GO
/****** Object:  StoredProcedure [dbo].[SWITCHScheme_HeaderGetOriginal]    Script Date: 02/13/2012 17:17:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SWITCHScheme_HeaderGetOriginal]

@param_strClientID nvarchar(50),
@param_strSchemeID nvarchar(50)


AS
BEGIN

	SET NOCOUNT ON;

SELECT [AccountNumber]
      ,[CompanyID]
      ,[ClientGenerated]
      ,[Company]
      ,[PortfolioType]
      ,[PortfolioTypeID]
      ,[SchemeID]
      ,[ClientID]
      ,[SchemeCurrency]
      ,[ContributionTotal]
      ,[ExcludeFromReports]
      ,[StartDate]
      ,[MaturityDate]
      ,[SumAssured]
      ,[PlanStatus]
      ,[Liquidity]
      ,[RiskProfile]
      ,[RetentionTerm]
      ,[MFPercent]
	  ,(select sum(CurrentValueScheme) as SC_TotalValue from [NavGlobalDBwwwGUID].[dbo].[SchemeDetails3] where	ClientID = @param_strClientID and SchemeID = @param_strSchemeID) as SC_TotalValue
	  ,(select WithdrawalTotal from [NavGlobalDBwwwGUID].[dbo].[vwSchemeTotalWithdrawalsToDate] where SchemeID = @param_strSchemeID) as WithdrawalTotal
	  ,(select sum(CurrentValueClient) as SumOfSumOfCurrentValueClient from [NavGlobalDBwwwGUID].[dbo].SchemeDetails3 where ClientID = @param_strClientID and SchemeID = @param_strSchemeID ) as CC_TotalValue
	  ,(select SwitchIFAPermit from NavGlobalDBwwwGUID.dbo.[Scheme] where ClientID = @param_strClientID and SchemeID = @param_strSchemeID and OLDeleted = 0) as SwitchIFAPermit
	  ,(select SwitchConfirmationRequired from NavGlobalDBwwwGUID.dbo.[Scheme] where ClientID = @param_strClientID and SchemeID = @param_strSchemeID and OLDeleted = 0) as SwitchConfirmationRequired


FROM [NavGlobalDBwwwGUID].[dbo].[SchemeGeneralDetailsTest]

where	ClientID = @param_strClientID
		and SchemeID = @param_strSchemeID

END

--select ((select sum(CurrentValueScheme) as SC_TotalValue from [NavGlobalDBwwwGUID].[dbo].[SchemeDetails3] where	ClientID = '409003-1' and SchemeID = '106151-1') - 
--(select WithdrawalTotal from [NavGlobalDBwwwGUID].[dbo].[vwSchemeTotalWithdrawalsToDate] where SchemeID = '106151-1'))
--
--SWITCHScheme_HeaderGet '409003-1', '92051-1' 106151-1
GO
