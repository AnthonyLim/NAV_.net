USE [NavIntegrationDB]
GO
/****** Object:  Table [dbo].[SwitchFeeHistory]    Script Date: 02/13/2012 17:15:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SwitchFeeHistory](
	[HistoryID] [int] IDENTITY(1,1) NOT NULL,
	[IFA_ID] [int] NOT NULL,
	[IFA_Username] [nvarchar](50) COLLATE Latin1_General_BIN NOT NULL,
	[Annual_Fee] [decimal](18, 2) NOT NULL,
	[Per_Switch_Fee] [decimal](18, 2) NOT NULL,
	[Date_Created] [datetime] NOT NULL,
	[Date_Effectivity] [datetime] NOT NULL,
	[History_Created_By] [nvarchar](50) COLLATE Latin1_General_BIN NOT NULL,
 CONSTRAINT [PK_SwitchFeeHistory] PRIMARY KEY CLUSTERED 
(
	[HistoryID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
