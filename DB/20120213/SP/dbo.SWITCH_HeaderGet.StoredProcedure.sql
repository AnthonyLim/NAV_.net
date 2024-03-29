USE [NavIntegrationDB]
GO
/****** Object:  StoredProcedure [dbo].[SWITCH_HeaderGet]    Script Date: 02/13/2012 17:17:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SWITCH_HeaderGet] 

@param_strPortfolioID	nvarchar(50) = NULL,
@param_strClientID		nvarchar(50) = NULL,
@param_intSwitchID		INT = NULL

AS
BEGIN

	SET NOCOUNT ON;
IF EXISTS (SELECT SwitchID FROM [NavIntegrationDB].[dbo].[SwitchHeader] WHERE SwitchID = @param_intSwitchID)	
	BEGIN
		SELECT	PortfolioID
				,ClientID
				,SwitchID
				,[Status]
				,Date_Created
				,Created_By
				,SecurityCodeAttempt
				,[Description]
				,Amend_Status
				,Amend_Description				
		FROM [NavIntegrationDB].[dbo].[SwitchHeader]
		WHERE SwitchID = @param_intSwitchID
	END
ELSE
	BEGIN
		SELECT	PortfolioID
				,ClientID
				,SwitchID
				,[Status]
				,Date_Created
				,Created_By
				,SecurityCodeAttempt
				,[Description]
				,Amend_Status
				,Amend_Description				
		FROM [NavIntegrationDB].[dbo].[SwitchHeader]
		WHERE PortfolioID = @param_strPortfolioID
			AND ClientID = @param_strClientID
	END
END
GO
