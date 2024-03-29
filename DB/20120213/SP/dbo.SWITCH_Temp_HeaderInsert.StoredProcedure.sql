USE [NavIntegrationDB]
GO
/****** Object:  StoredProcedure [dbo].[SWITCH_Temp_HeaderInsert]    Script Date: 02/13/2012 17:17:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SWITCH_Temp_HeaderInsert]

	 @param_intIFA_ID			INT
	,@param_strModelGroupID		NVARCHAR(50)
	,@param_strModelPortfolioID	NVARCHAR(50)
	,@param_strClientID			NVARCHAR(50)
	,@param_strPortfolioID		NVARCHAR(50)
	,@param_strUser				NVARCHAR(50)
	
AS
BEGIN

SET NOCOUNT ON;

IF NOT EXISTS (SELECT ClientID, PortfolioID FROM [NavIntegrationDB].[dbo].[SwitchHeaderTemp] WHERE [ClientID] = @param_strClientID AND [PortfolioID] = @param_strPortfolioID)
	BEGIN
		INSERT INTO [NavIntegrationDB].[dbo].[SwitchHeaderTemp]
			   ([IFA_ID]
			   ,[ModelGroupID] 
			   ,[ModelPortfolioID] 
			   ,[ClientID]
			   ,[PortfolioID]
			   ,[Date_Created]
			   ,[Created_By]
			   ,[Date_Updated]
			   ,[Updated_By] 
			   ,[IsModelCustomized] 
			   )
		 VALUES
			   (@param_intIFA_ID
			   ,@param_strModelGroupID
			   ,@param_strModelPortfolioID
			   ,@param_strClientID
			   ,@param_strPortfolioID
			   ,CURRENT_TIMESTAMP
			   ,@param_strUser
			   ,CURRENT_TIMESTAMP
			   ,@param_strUser
			   ,1
			   );
	END
ELSE
	BEGIN
		UPDATE [NavIntegrationDB].[dbo].[SwitchHeaderTemp]
		SET		 [IFA_ID] = @param_intIFA_ID
				,[ModelGroupID] = @param_strModelGroupID
				,[ModelPortfolioID] = @param_strModelPortfolioID
				,[Date_Updated] = CURRENT_TIMESTAMP
				,[Updated_By] = @param_strUser
		WHERE	[ClientID] = @param_strClientID 
		AND		[PortfolioID] = @param_strPortfolioID
	END
END
GO
