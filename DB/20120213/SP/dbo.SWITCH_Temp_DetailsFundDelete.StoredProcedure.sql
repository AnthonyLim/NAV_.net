USE [NavIntegrationDB]
GO
/****** Object:  StoredProcedure [dbo].[SWITCH_Temp_DetailsFundDelete]    Script Date: 02/13/2012 17:17:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SWITCH_Temp_DetailsFundDelete]

	 @param_strClientID			NVARCHAR(50)
	,@param_strPortfolioID		NVARCHAR(50)
    ,@param_intFundID			INT
     
AS
BEGIN
	
SET NOCOUNT ON;

DELETE 
FROM	[NavIntegrationDB].[dbo].[SwitchDetailsTemp]
WHERE	[ClientID]		= @param_strClientID
AND		[PortfolioID]	= @param_strPortfolioID
AND		[FundID]		= @param_intFundID
	
END
GO
