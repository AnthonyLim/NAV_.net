USE [NavIntegrationDB]
GO
/****** Object:  Table [dbo].[SwitchSchemeHeader]    Script Date: 02/13/2012 17:15:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SwitchSchemeHeader](
	[SchemeID] [nvarchar](50) COLLATE Latin1_General_BIN NOT NULL,
	[ClientID] [nvarchar](50) COLLATE Latin1_General_BIN NOT NULL,
	[SwitchID] [int] IDENTITY(1,1) NOT NULL,
	[Status] [smallint] NOT NULL,
	[Date_Created] [datetime] NOT NULL,
	[Created_By] [nvarchar](50) COLLATE Latin1_General_BIN NOT NULL,
	[SecurityCodeAttempt] [int] NOT NULL,
	[Description] [nvarchar](max) COLLATE Latin1_General_BIN NULL,
	[Amend_Status] [smallint] NULL,
	[Amend_Description] [nvarchar](max) COLLATE Latin1_General_BIN NULL,
 CONSTRAINT [PK_SwitchSchemeHeader] PRIMARY KEY CLUSTERED 
(
	[SwitchID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
