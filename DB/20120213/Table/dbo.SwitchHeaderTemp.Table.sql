USE [NavIntegrationDB]
GO
/****** Object:  Table [dbo].[SwitchHeaderTemp]    Script Date: 02/13/2012 17:15:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SwitchHeaderTemp](
	[IFA_ID] [int] NOT NULL,
	[ModelGroupID] [nvarchar](50) COLLATE Latin1_General_BIN NOT NULL,
	[ModelPortfolioID] [nvarchar](50) COLLATE Latin1_General_BIN NOT NULL,
	[ClientID] [nvarchar](50) COLLATE Latin1_General_BIN NOT NULL,
	[PortfolioID] [nvarchar](50) COLLATE Latin1_General_BIN NOT NULL,
	[Date_Created] [datetime] NOT NULL,
	[Created_By] [nvarchar](50) COLLATE Latin1_General_BIN NULL,
	[Date_Updated] [datetime] NULL,
	[Updated_By] [nvarchar](50) COLLATE Latin1_General_BIN NULL,
	[Description] [nvarchar](max) COLLATE Latin1_General_BIN NULL,
	[IsModelCustomized] [bit] NOT NULL
) ON [PRIMARY]
GO
