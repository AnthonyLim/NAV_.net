USE [NavIntegrationDB]
GO
/****** Object:  StoredProcedure [dbo].[SWITCH_EmailTemplateGet]    Script Date: 02/13/2012 17:17:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SWITCH_EmailTemplateGet] 

@param_intEmailTemplateID	int = null,
@param_strTemplateName		nvarchar(100) = null

AS
BEGIN

	SET NOCOUNT ON;

IF EXISTS (SELECT EmailTemplateID FROM dbo.Switch_EmailTemplate WHERE EmailTemplateID = @param_intEmailTemplateID AND TemplateName = @param_strTemplateName)
	BEGIN
		SELECT	EmailTemplateID, TemplateName, Description, Body
		FROM	dbo.Switch_EmailTemplate
		WHERE	EmailTemplateID = @param_intEmailTemplateID 
				AND TemplateName = @param_strTemplateName
	END
ELSE IF EXISTS (SELECT EmailTemplateID FROM dbo.Switch_EmailTemplate WHERE TemplateName = @param_strTemplateName)
	BEGIN
		SELECT	EmailTemplateID, TemplateName, Description, Body
		FROM	dbo.Switch_EmailTemplate 
		WHERE	TemplateName = @param_strTemplateName
	END
ELSE
	BEGIN
		SELECT	EmailTemplateID, TemplateName, Description, Body
		FROM	dbo.Switch_EmailTemplate 
		WHERE	EmailTemplateID = @param_intEmailTemplateID
	END
END
GO
