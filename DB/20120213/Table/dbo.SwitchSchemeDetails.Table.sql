USE [NavIntegrationDB]
GO
/****** Object:  Table [dbo].[SwitchSchemeDetails]    Script Date: 02/13/2012 17:15:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SwitchSchemeDetails](
	[SwitchDetailsID] [int] IDENTITY(1,1) NOT NULL,
	[SwitchID] [int] NOT NULL,
	[FundID] [int] NOT NULL,
	[Allocation] [float] NOT NULL,
	[Date_Created] [datetime] NOT NULL,
	[Created_By] [nvarchar](50) COLLATE Latin1_General_BIN NOT NULL,
	[Date_LastUpdate] [datetime] NULL,
	[Updated_By] [nvarchar](50) COLLATE Latin1_General_BIN NULL,
	[isDeletable] [smallint] NOT NULL,
	[isContribution] [smallint] NOT NULL,
 CONSTRAINT [PK_SwitchSchemeDetails] PRIMARY KEY CLUSTERED 
(
	[SwitchDetailsID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
