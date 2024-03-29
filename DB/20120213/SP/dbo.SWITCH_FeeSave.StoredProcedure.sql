USE [NavIntegrationDB]
GO
/****** Object:  StoredProcedure [dbo].[SWITCH_FeeSave]    Script Date: 02/13/2012 17:17:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SWITCH_FeeSave]

@param_intIFA_ID		INT,
@param_IFA_Username		NVARCHAR(50),
@param_Annual_Fee		DECIMAL(18,2),
@param_Per_Switch_Fee	DECIMAL(18,2),
@param_Created_By		NVARCHAR(50),
@param_Updated_By		NVARCHAR(50),
@param_Access_Denied	BIT

AS
BEGIN
	IF EXISTS(SELECT IFA_ID FROM [NavIntegrationDB].[dbo].[SwitchFee] WHERE IFA_ID = @param_intIFA_ID)
		BEGIN
			IF((SELECT [Per_Switch_Fee] FROM [NavIntegrationDB].[dbo].[SwitchFee] WHERE IFA_ID = @param_intIFA_ID) <> @param_Per_Switch_Fee)
				BEGIN
					INSERT INTO [NavIntegrationDB].[dbo].[SwitchFeeHistory]
						(IFA_ID
						,IFA_Username
						,Annual_Fee
						,Per_Switch_Fee
						,Date_Created
						,Date_Effectivity
						,History_Created_By)
					SELECT 
							 IFA_ID
							,IFA_Username
							,Annual_Fee
							,Per_Switch_Fee
							,GETDATE()
							,Date_Updated
							,Updated_By
					FROM [NavIntegrationDB].[dbo].[SwitchFee]
					WHERE IFA_ID = @param_intIFA_ID
				END
			UPDATE [NavIntegrationDB].[dbo].[SwitchFee]
				SET [Annual_Fee] = @param_Annual_Fee,
					[Per_Switch_Fee] = @param_Per_Switch_Fee,
					[Date_Updated] = GETDATE(),
					[Updated_By] = @param_Updated_By,
					[Access_Denied] = @param_Access_Denied 
				WHERE 
					IFA_ID = @param_intIFA_ID
			
			
		END
	ELSE
		BEGIN
			INSERT INTO [NavIntegrationDB].[dbo].[SwitchFee]
			   ([IFA_ID]
			   ,[IFA_Username]
			   ,[Annual_Fee]
			   ,[Per_Switch_Fee]
			   ,[Date_Created]
			   ,[Created_By]
			   ,[Access_Denied])
			VALUES
			   (@param_intIFA_ID
			   ,@param_IFA_Username
			   ,@param_Annual_Fee
			   ,@param_Per_Switch_Fee
			   ,GETDATE()
			   ,@param_Created_By
			   ,@param_Access_Denied)
		END
END
GO
