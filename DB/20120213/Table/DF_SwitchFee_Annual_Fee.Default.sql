USE [NavIntegrationDB]
GO
/****** Object:  Default [DF_SwitchFee_Annual_Fee]    Script Date: 02/13/2012 17:15:04 ******/
ALTER TABLE [dbo].[SwitchFee] ADD  CONSTRAINT [DF_SwitchFee_Annual_Fee]  DEFAULT ((0)) FOR [Annual_Fee]
GO
