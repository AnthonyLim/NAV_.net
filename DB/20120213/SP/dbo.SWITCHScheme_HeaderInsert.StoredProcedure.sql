USE [NavIntegrationDB]
GO
/****** Object:  StoredProcedure [dbo].[SWITCHScheme_HeaderInsert]    Script Date: 02/13/2012 17:17:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SWITCHScheme_HeaderInsert]

	 @param_strSchemeID		NVARCHAR(50)
	,@param_strClientID		NVARCHAR(50)
	,@param_intStatus		SMALLINT
	,@param_strCreated_By	NVARCHAR(50)
	,@param_intSwitchID		INT				= NULL
	,@param_strDescription	NVARCHAR(MAX)
	
AS
BEGIN

SET NOCOUNT ON;

IF NOT EXISTS (SELECT SchemeID FROM [NavIntegrationDB].[dbo].[SwitchSchemeHeader] WHERE [SwitchID] = @param_intSwitchID)
BEGIN
	
	INSERT INTO [NavIntegrationDB].[dbo].[SwitchSchemeHeader]
		   ([SchemeID]
		   ,[ClientID]
		   ,[Status]
		   ,[Date_Created]
		   ,[Created_By]
		   ,[Description] 
		   )
	 VALUES
		   (@param_strSchemeID
		   ,@param_strClientID
		   ,@param_intStatus
		   ,CURRENT_TIMESTAMP
		   ,@param_strCreated_By
		   ,@param_strDescription
		   );
		   
	SELECT @@IDENTITY 
END
ELSE
BEGIN
	IF(LEN(@param_strDescription) = 0)
		BEGIN
			UPDATE [NavIntegrationDB].[dbo].[SwitchSchemeHeader]
			SET [Status] = @param_intStatus, [SecurityCodeAttempt] = 0
			WHERE [SwitchID] = @param_intSwitchID;		
		END
	ELSE
		BEGIN
			UPDATE [NavIntegrationDB].[dbo].[SwitchSchemeHeader]
			SET [Status] = @param_intStatus, [SecurityCodeAttempt] = 0, [Description] = @param_strDescription
			WHERE [SwitchID] = @param_intSwitchID;
		END
	
	SELECT @param_intSwitchID

END
END
GO
