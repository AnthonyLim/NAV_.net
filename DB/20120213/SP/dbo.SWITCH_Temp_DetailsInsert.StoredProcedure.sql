USE [NavIntegrationDB]
GO
/****** Object:  StoredProcedure [dbo].[SWITCH_Temp_DetailsInsert]    Script Date: 02/13/2012 17:17:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SWITCH_Temp_DetailsInsert]

	 @param_strClientID			NVARCHAR(50)
	,@param_strPortfolioID		NVARCHAR(50)
	,@param_strUser				NVARCHAR(50)
    ,@param_intFundID			INT
    ,@param_fAllocation			FLOAT
    ,@param_sintIsDeletable		SMALLINT
     
AS
BEGIN
	SET NOCOUNT ON;
IF EXISTS (SELECT [ClientID], [PortfolioID]  FROM [NavIntegrationDB].[dbo].[SwitchDetailsTemp] WHERE [ClientID] = @param_strClientID AND [PortfolioID] = @param_strPortfolioID AND [FundID] = @param_intFundID)
	BEGIN
		UPDATE NavIntegrationDB.dbo.SwitchDetailsTemp
			SET	[Allocation]		= @param_fAllocation
			   ,[Date_Updated]		= CURRENT_TIMESTAMP
			   ,[Updated_By]		= @param_strUser
			   ,[isDeletable]		= @param_sintIsDeletable
		WHERE	[ClientID]			= @param_strClientID 
		AND		[PortfolioID]		= @param_strPortfolioID
		AND		[FundID]			= @param_intFundID
	END
ELSE
	BEGIN
		INSERT INTO [NavIntegrationDB].[dbo].[SwitchDetailsTemp]
			   ([ClientID] 
			   ,[PortfolioID] 
			   ,[FundID]
			   ,[Allocation]
			   ,[Date_Created]
			   ,[Created_By]
			   ,[Date_Updated]
			   ,[Updated_By]
			   ,[isDeletable])
		 VALUES
			   (@param_strClientID
			   ,@param_strPortfolioID
			   ,@param_intFundID
			   ,@param_fAllocation
			   ,CURRENT_TIMESTAMP
			   ,@param_strUser
			   ,CURRENT_TIMESTAMP
			   ,@param_strUser
			   ,@param_sintIsDeletable)
	END
END
GO
