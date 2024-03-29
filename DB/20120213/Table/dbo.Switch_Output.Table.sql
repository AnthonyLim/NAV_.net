USE [NavIntegrationDB]
GO
/****** Object:  Table [dbo].[Switch_Output]    Script Date: 02/13/2012 17:15:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Switch_Output](
	[OutputID] [int] IDENTITY(1,1) NOT NULL,
	[SwitchType] [varchar](50) COLLATE Latin1_General_BIN NOT NULL,
	[SwitchID] [int] NOT NULL,
	[DateCreated] [datetime] NOT NULL,
	[FileName] [varchar](255) COLLATE Latin1_General_BIN NOT NULL,
	[OutputType] [varchar](50) COLLATE Latin1_General_BIN NOT NULL,
 CONSTRAINT [PK_SWITCH_Output] PRIMARY KEY CLUSTERED 
(
	[OutputID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'File name of the output document' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Switch_Output', @level2type=N'COLUMN',@level2name=N'FileName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Type of document e.g.:Excel, PDF, XML, and the like' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Switch_Output', @level2type=N'COLUMN',@level2name=N'OutputType'
GO
