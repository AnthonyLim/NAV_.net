USE [NavIntegrationDB]
GO
/****** Object:  StoredProcedure [dbo].[SWITCH_SMSTemplateUpdate]    Script Date: 02/13/2012 17:17:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SWITCH_SMSTemplateUpdate] 

@param_sintSMSTemplateID	smallint,
@param_strTemplateName		nvarchar(50),
@param_strTemplateFor		nvarchar(50),
@param_strMessage			nvarchar(160)

AS
BEGIN

	SET NOCOUNT ON;

	BEGIN
		UPDATE [NavIntegrationDB].[dbo].[Switch_SMSTemplate]
		SET TemplateName = @param_strTemplateName, 
			TemplateFor = @param_strTemplateFor,
			[Message] = @param_strMessage
		WHERE SMSTemplateID = @param_sintSMSTemplateID
	END

END
GO
