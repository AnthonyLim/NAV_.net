USE [NavIntegrationDB]
GO
/****** Object:  Default [DF_SwitchFee_Switch_Access]    Script Date: 02/13/2012 17:15:04 ******/
ALTER TABLE [dbo].[SwitchFee] ADD  CONSTRAINT [DF_SwitchFee_Switch_Access]  DEFAULT ((0)) FOR [Access_Denied]
GO
