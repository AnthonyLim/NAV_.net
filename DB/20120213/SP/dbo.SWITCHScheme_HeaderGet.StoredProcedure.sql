USE [NavIntegrationDB]
GO
/****** Object:  StoredProcedure [dbo].[SWITCHScheme_HeaderGet]    Script Date: 02/13/2012 17:17:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SWITCHScheme_HeaderGet] 

@param_strSchemeID	nvarchar(50) = NULL,
@param_strClientID		nvarchar(50) = NULL,
@param_intSwitchID		INT = NULL

AS
BEGIN

	SET NOCOUNT ON;

IF EXISTS (SELECT SwitchID FROM [NavIntegrationDB].[dbo].[SwitchSchemeHeader] WHERE SwitchID = @param_intSwitchID)	
	BEGIN

	SELECT	[SchemeID]
			,[ClientID]
			,[SwitchID]
			,[Status]
			,[Date_Created]
			,[Created_By]
			,[SecurityCodeAttempt]
			,[Description]
			,[Amend_Status]
			,[Amend_Description]
	FROM	[NavIntegrationDB].[dbo].[SwitchSchemeHeader]
	WHERE	[SwitchID] = @param_intSwitchID

	END
ELSE
	BEGIN

	SELECT	[SchemeID]
			,[ClientID]
			,[SwitchID]
			,[Status]
			,[Date_Created]
			,[Created_By]
			,[SecurityCodeAttempt]
			,[Description]
			,[Amend_Status]
			,[Amend_Description]
	FROM	[NavIntegrationDB].[dbo].[SwitchSchemeHeader]
	WHERE	SchemeID = @param_strSchemeID
			AND ClientID = @param_strClientID
			--and status is not complete
	END
END
GO
