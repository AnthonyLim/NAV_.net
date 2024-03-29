USE [NavIntegrationDB]
GO
/****** Object:  StoredProcedure [dbo].[SWITCHHistory_DetailsInsert]    Script Date: 02/13/2012 17:17:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SWITCHHistory_DetailsInsert]

      @param_HistoryID			int
      ,@param_SwitchDetailsID	int
      ,@param_FundID			int
      ,@param_Allocation		float
      ,@param_Created_By		nvarchar(50)
     
AS
BEGIN
	
	SET NOCOUNT ON;
INSERT INTO [NavIntegrationDB].[dbo].[SwitchHistoryDetails]
           ([HistoryID]
           ,[SwitchDetailsID]
           ,[FundID]
           ,[Allocation]
           ,[Date_Created]
           ,[Created_By])
     VALUES
           (@param_HistoryID
           ,@param_SwitchDetailsID
           ,@param_FundID
           ,@param_Allocation
           ,Current_TimeStamp
           ,@param_Created_By)
END
GO
