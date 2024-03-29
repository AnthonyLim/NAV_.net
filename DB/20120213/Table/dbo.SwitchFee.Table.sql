USE [NavIntegrationDB]
GO
/****** Object:  Table [dbo].[SwitchFee]    Script Date: 02/13/2012 17:15:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SwitchFee](
	[IFA_ID] [int] NOT NULL,
	[IFA_Username] [nvarchar](50) COLLATE Latin1_General_BIN NULL,
	[Annual_Fee] [decimal](18, 2) NOT NULL,
	[Per_Switch_Fee] [decimal](18, 2) NOT NULL,
	[Date_Created] [datetime] NOT NULL,
	[Created_By] [nvarchar](50) COLLATE Latin1_General_BIN NULL,
	[Date_Updated] [datetime] NULL,
	[Updated_By] [nvarchar](50) COLLATE Latin1_General_BIN NULL,
	[Access_Denied] [bit] NOT NULL,
 CONSTRAINT [PK_SwitchFee] PRIMARY KEY CLUSTERED 
(
	[IFA_ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
