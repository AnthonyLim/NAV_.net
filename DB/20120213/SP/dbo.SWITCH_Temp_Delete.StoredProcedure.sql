USE [NavIntegrationDB]
GO
/****** Object:  StoredProcedure [dbo].[SWITCH_Temp_Delete]    Script Date: 02/13/2012 17:17:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SWITCH_Temp_Delete] 

@param_strClientID		nvarchar(50) = NULL,
@param_strPortfolioID	nvarchar(50) = NULL

AS
BEGIN

	DELETE 
	FROM	[NavIntegrationDB].[dbo].[SwitchDetailsTemp] 
	WHERE	ClientID = @param_strClientID
	AND		PortfolioID = @param_strPortfolioID
	
	DELETE
	FROM	[NavIntegrationDB].[dbo].[SwitchHeaderTemp]  
	WHERE	ClientID = @param_strClientID
	AND		PortfolioID = @param_strPortfolioID

END
GO
