USE [NavIntegrationDB]
GO
/****** Object:  Default [DF_Table_1_Date_Created]    Script Date: 02/13/2012 17:15:04 ******/
ALTER TABLE [dbo].[SwitchFeeHistory] ADD  CONSTRAINT [DF_Table_1_Date_Created]  DEFAULT (getdate()) FOR [Date_Effectivity]
GO
