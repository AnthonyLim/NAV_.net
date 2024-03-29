USE [NavIntegrationDB]
GO
/****** Object:  StoredProcedure [dbo].[SWITCH_EmailLogInsert]    Script Date: 02/13/2012 17:17:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SWITCH_EmailLogInsert]

	@param_intSwitchID		int
	,@param_strRecipient	nvarchar(255)
	,@param_strClientID		nvarchar(50)
	,@param_strMessage		nvarchar(max)
	,@param_strPurpose		nvarchar(100)

AS
BEGIN

SET NOCOUNT ON;


INSERT INTO [NavIntegrationDB].[dbo].[SwitchEmail_Logs]
           ([SwitchID]
           ,[Recipient]
           ,[ClientID]
           ,[Message]
           ,[DateSent]
		   ,Purpose)
     VALUES
           (@param_intSwitchID
           ,@param_strRecipient
           ,@param_strClientID
           ,@param_strMessage
           ,CURRENT_TIMESTAMP
		   ,@param_strPurpose)
END
GO
