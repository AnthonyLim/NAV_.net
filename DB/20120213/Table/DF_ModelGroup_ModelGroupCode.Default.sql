USE [NavIntegrationDB]
GO
/****** Object:  Default [DF_ModelGroup_ModelGroupCode]    Script Date: 02/13/2012 17:15:04 ******/
ALTER TABLE [dbo].[ModelGroup] ADD  CONSTRAINT [DF_ModelGroup_ModelGroupCode]  DEFAULT ((210)) FOR [ModelGroupCode]
GO
