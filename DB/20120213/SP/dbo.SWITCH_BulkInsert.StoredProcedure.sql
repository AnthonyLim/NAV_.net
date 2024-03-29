USE [NavIntegrationDB]
GO
/****** Object:  StoredProcedure [dbo].[SWITCH_BulkInsert]    Script Date: 02/13/2012 17:17:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SWITCH_BulkInsert]

AS

BEGIN

	DECLARE @ClientID NVARCHAR(50)
	DECLARE @PortfolioID NVARCHAR(50)
	DECLARE @SwitchID INT

	DECLARE db_cursor CURSOR FAST_FORWARD FOR SELECT ClientID, PortfolioID FROM [NavIntegrationDB].[dbo].[SwitchHeaderTemp]

	OPEN db_cursor

	FETCH NEXT FROM db_cursor INTO @ClientID, @PortfolioID 

	WHILE @@FETCH_STATUS = 0
	BEGIN
		INSERT INTO [NavIntegrationDB].[dbo].[SwitchHeader]
			   ([PortfolioID]
			   ,[ClientID]
			   ,[Status]
			   ,[Date_Created]
			   ,[Created_By]
			   ,[Description] 
			   ,[ModelGroupID]
			   ,[ModelPortfolioID] 
			   ,[IsModelCustomized] 
			   )

			SELECT 
				 PortfolioID
				,ClientID
				,0
				,Date_Created
				,Created_By
				,Description
				,ModelGroupID
				,ModelPortfolioID
				,[IsModelCustomized] 
			FROM [NavIntegrationDB].[dbo].[SwitchHeaderTemp] WHERE ClientID = @ClientID AND PortfolioID = @PortfolioID

		SET @SwitchID = @@IDENTITY 


		INSERT INTO [NavIntegrationDB].[dbo].[SwitchDetails]
				   ([SwitchID]
				   ,[FundID]
				   ,[Allocation]
				   ,[Created_By]
				   ,[Date_Created]
				   ,[Date_LastUpdate]
				   ,[Updated_By]
				   ,[isDeletable])
			SELECT  
					 @SwitchID
					,[FundID] 
					,[Allocation]
					,[Created_By]
					,[Date_Created]
					,[Date_Updated] 
					,[Updated_By] 
					,[isDeletable] 
			
			FROM [NavIntegrationDB].[dbo].[SwitchDetailsTemp] WHERE ClientID = @ClientID AND PortfolioID = @PortfolioID

		FETCH NEXT FROM db_cursor INTO @ClientID, @PortfolioID 
	END

	CLOSE db_cursor 
	DEALLOCATE db_cursor

END
GO
