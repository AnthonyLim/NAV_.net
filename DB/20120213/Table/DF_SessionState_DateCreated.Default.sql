USE [NavIntegrationDB]
GO
/****** Object:  Default [DF_SessionState_DateCreated]    Script Date: 02/13/2012 17:15:04 ******/
ALTER TABLE [dbo].[SessionState] ADD  CONSTRAINT [DF_SessionState_DateCreated]  DEFAULT (getdate()) FOR [DateCreated]
GO
