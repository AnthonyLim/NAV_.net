USE [NavIntegrationDB]
GO
/****** Object:  Table [dbo].[SwitchClientSecurityCode]    Script Date: 02/13/2012 17:15:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SwitchClientSecurityCode](
	[CodeID] [int] IDENTITY(1,1) NOT NULL,
	[SwitchID] [int] NOT NULL,
	[Code] [nvarchar](20) COLLATE Latin1_General_BIN NOT NULL,
	[ClientID] [nvarchar](10) COLLATE Latin1_General_BIN NOT NULL,
	[PortfolioID] [nvarchar](10) COLLATE Latin1_General_BIN NOT NULL,
	[Attempt] [int] NOT NULL,
	[IsConsumed] [nchar](1) COLLATE Latin1_General_BIN NOT NULL,
 CONSTRAINT [PK_SwitchClientSecurityCode] PRIMARY KEY CLUSTERED 
(
	[CodeID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
