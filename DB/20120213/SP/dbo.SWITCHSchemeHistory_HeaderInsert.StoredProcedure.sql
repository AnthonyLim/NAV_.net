USE [NavIntegrationDB]
GO
/****** Object:  StoredProcedure [dbo].[SWITCHSchemeHistory_HeaderInsert]    Script Date: 02/13/2012 17:17:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SWITCHSchemeHistory_HeaderInsert]

	@param_SwitchID		int
	,@param_SchemeID	nvarchar(50)
	,@param_Status		smallint
	
AS
BEGIN

SET NOCOUNT ON;

INSERT INTO [NavIntegrationDB].[dbo].[SwitchSchemeHistoryHeader]
           ([SchemeID],[SwitchID],[Action_Date],[Status])
     VALUES
           (@param_SchemeID, @param_SwitchID, CURRENT_TIMESTAMP, @param_Status);

SELECT @@IDENTITY
END
GO
