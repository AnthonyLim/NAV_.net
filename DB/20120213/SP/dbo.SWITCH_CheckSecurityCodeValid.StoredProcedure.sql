USE [NavIntegrationDB]
GO
/****** Object:  StoredProcedure [dbo].[SWITCH_CheckSecurityCodeValid]    Script Date: 02/13/2012 17:17:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SWITCH_CheckSecurityCodeValid]

	 @param_Code			NVARCHAR(16)
	,@param_ClientID		NVARCHAR(10)
	,@param_PortfolioID		NVARCHAR(10)
	,@param_SwitchID		INT

AS
	DECLARE 
		 @return_Message	NVARCHAR(MAX)
		,@Attempt			int = 0
BEGIN
	IF EXISTS (SELECT * FROM NavIntegrationDB.dbo.SwitchClientSecurityCode WHERE Code = @param_Code AND SwitchID = @param_SwitchID)
		BEGIN
			IF ((SELECT IsConsumed FROM NavIntegrationDB.dbo.SwitchClientSecurityCode WHERE Code = @param_Code AND SwitchID = @param_SwitchID) = 0)
				BEGIN
					UPDATE NavIntegrationDB.dbo.SwitchClientSecurityCode SET IsConsumed = 1 WHERE Code = @param_Code AND SwitchID = @param_SwitchID AND IsConsumed = 0
					UPDATE NavIntegrationDB.dbo.SwitchHeader  SET Status = 6 WHERE SwitchID = @param_SwitchID
					SET @return_Message = 'Thank you, the proposed changes will be made to your portfolio.'
				END
			ELSE
				BEGIN
					SET @return_Message = 'The security code you have entered is already used'
				END
		END
	ELSE
		BEGIN
			SET @Attempt = (SELECT SecurityCodeAttempt FROM NavIntegrationDB.dbo.SwitchHeader WHERE SwitchID = @param_SwitchID)
			IF (@Attempt < 3)
				BEGIN
					SET @Attempt = @Attempt + 1
					UPDATE NavIntegrationDB.dbo.SwitchHeader SET SecurityCodeAttempt =  @Attempt WHERE SwitchID = @param_SwitchID
					IF (@Attempt > 2)
						BEGIN
							--SET @return_Message = CONVERT(CHAR(1), @Attempt)
							SET @return_Message = 'Sorry, you have entered the security code incorrectly three times. Please contact your IFA to have the security code reset.'
							UPDATE NavIntegrationDB.dbo.SwitchHeader SET Status = 7 WHERE SwitchID = @param_SwitchID
						END
					ELSE
						BEGIN
							SET @return_Message = 'Sorry, the security code you have entered is incorrect, please re-enter the security code. You have ' + CONVERT(CHAR(1),3 - @Attempt) + ' more attempts.'
						END
				END
			ELSE
				BEGIN
					UPDATE NavIntegrationDB.dbo.SwitchHeader SET Status = 7 WHERE SwitchID = @param_SwitchID
					SET @return_Message = 'Sorry, you have entered the security code incorrectly three times. Please contact your IFA to have the security code reset.'
				END
		END
END

SELECT ISNULL(@return_Message,'')
GO
