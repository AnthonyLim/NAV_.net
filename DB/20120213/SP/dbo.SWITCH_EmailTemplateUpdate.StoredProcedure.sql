USE [NavIntegrationDB]
GO
/****** Object:  StoredProcedure [dbo].[SWITCH_EmailTemplateUpdate]    Script Date: 02/13/2012 17:17:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SWITCH_EmailTemplateUpdate] 

	@param_intEmailTemplateID	int,
	@param_strTemplateName		nvarchar(100),
	@param_strDescription		nvarchar(100),
	@param_strBody				nvarchar(max)

AS
BEGIN

	SET NOCOUNT ON;

	IF NOT EXISTS (SELECT EmailTemplateID FROM dbo.Switch_EmailTemplate WHERE TemplateName = @param_strTemplateName AND EmailTemplateID <> @param_intEmailTemplateID)
	BEGIN
		UPDATE dbo.Switch_EmailTemplate
		SET Description = @param_strDescription,
			Body = @param_strBody
		WHERE EmailTemplateID = @param_intEmailTemplateID
	END
	ELSE
		BEGIN
			RAISERROR('Template name already exists.', 16, 1)		
		END

END
GO
