USE [NavIntegrationDB]
GO
/****** Object:  Default [DF_SwitchHeader_Date_Created]    Script Date: 02/13/2012 17:15:04 ******/
ALTER TABLE [dbo].[SwitchHeader] ADD  CONSTRAINT [DF_SwitchHeader_Date_Created]  DEFAULT (getdate()) FOR [Date_Created]
GO
