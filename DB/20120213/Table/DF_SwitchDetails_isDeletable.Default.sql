USE [NavIntegrationDB]
GO
/****** Object:  Default [DF_SwitchDetails_isDeletable]    Script Date: 02/13/2012 17:15:04 ******/
ALTER TABLE [dbo].[SwitchDetails] ADD  CONSTRAINT [DF_SwitchDetails_isDeletable]  DEFAULT ((0)) FOR [isDeletable]
GO
