USE [NavIntegrationDB]
GO
/****** Object:  StoredProcedure [dbo].[SWITCH_EmailTemplateInsert]    Script Date: 02/13/2012 17:17:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SWITCH_EmailTemplateInsert] 
	@param_strTemplateName nvarchar(100),
	@param_strDescription	nvarchar(100),
	@param_strBody		nvarchar(max)

AS
BEGIN

	SET NOCOUNT ON;
	IF NOT EXISTS (SELECT EmailTemplateID, TemplateName, Description, Body FROM dbo.Switch_EmailTemplate WHERE TemplateName = @param_strTemplateName)
		BEGIN
			INSERT INTO dbo.Switch_EmailTemplate
			(TemplateName, Description, Body)
			VALUES
			(@param_strTemplateName, @param_strDescription, @param_strBody)
		END	
	ELSE
		BEGIN
			RAISERROR('Template name already exists.', 16, 1)		
		END
END
GO
