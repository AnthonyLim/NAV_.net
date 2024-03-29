USE [NavIntegrationDB]
GO
/****** Object:  Table [dbo].[SwitchDetails_Client]    Script Date: 02/13/2012 17:15:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SwitchDetails_Client](
	[SwitchDetailsID] [int] IDENTITY(1,1) NOT NULL,
	[SwitchID] [int] NOT NULL,
	[FundID] [int] NOT NULL,
	[Allocation] [float] NOT NULL,
	[Date_Created] [datetime] NOT NULL,
	[Created_By] [nvarchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[Date_LastUpdate] [datetime] NULL,
	[Updated_By] [nvarchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[isDeletable] [smallint] NOT NULL,
 CONSTRAINT [PK_SwitchDetails_Client] PRIMARY KEY CLUSTERED 
(
	[SwitchDetailsID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
