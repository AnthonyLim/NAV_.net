USE [NavIntegrationDB]
GO
/****** Object:  StoredProcedure [dbo].[SWITCHclient_DetailsInsert]    Script Date: 02/13/2012 17:17:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SWITCHclient_DetailsInsert]
	
	  @param_intSwitchID			INT
     ,@param_intFundID				INT
     ,@param_fAllocation			FLOAT
     ,@param_strCreated_By			NVARCHAR(50)
     ,@param_strUpdated_By			NVARCHAR(50)
     ,@param_intSwitchDetailsID		INT = NULL
	 ,@param_sintIsDeletable		SMALLINT
     
AS
BEGIN
	
	SET NOCOUNT ON;
IF EXISTS (SELECT SwitchDetailsID,SwitchID  FROM [NavIntegrationDB].[dbo].[SwitchDetails_Client] WHERE SwitchDetailsID = @param_intSwitchDetailsID AND SwitchID = @param_intSwitchID)
	BEGIN
		UPDATE NavIntegrationDB.dbo.SwitchDetails_Client
			SET	[FundID] = @param_intFundID
			   ,[Allocation] = @param_fAllocation
			   ,[Created_By] = @param_strCreated_By
			   ,[Date_LastUpdate] = CURRENT_TIMESTAMP
			   ,[Updated_By] = @param_strUpdated_By
			   ,[isDeletable] = @param_sintIsDeletable
		WHERE SwitchDetailsID = @param_intSwitchDetailsID AND SwitchID = @param_intSwitchID 
	END
ELSE
	BEGIN
		INSERT INTO [NavIntegrationDB].[dbo].[SwitchDetails_Client]
			   ([SwitchID]
			   ,[FundID]
			   ,[Allocation]
			   ,[Created_By]
			   ,[Date_Created]
			   ,[Date_LastUpdate]
			   ,[Updated_By]
			   ,[isDeletable])
		 VALUES
			   (@param_intSwitchID
			   ,@param_intFundID
			   ,@param_fAllocation
			   ,@param_strCreated_By
			   ,CURRENT_TIMESTAMP
			   ,CURRENT_TIMESTAMP
			   ,@param_strUpdated_By
			   ,@param_sintIsDeletable)
	END
END
GO
