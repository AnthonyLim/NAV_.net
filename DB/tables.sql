USE [NavIntegrationDB]
GO
/****** Object:  User [BUILTIN\Users]    Script Date: 03/01/2012 14:29:46 ******/
CREATE USER [BUILTIN\Users] FOR LOGIN [BUILTIN\Users]
GO
/****** Object:  Table [dbo].[ModelPortfolioDetails]    Script Date: 03/01/2012 14:29:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ModelPortfolioDetails](
	[IFA_ID] [int] NOT NULL,
	[ModelID] [bigint] NOT NULL,
	[ModelGroupID] [nvarchar](50) COLLATE Latin1_General_BIN NOT NULL,
	[ModelPortfolioID] [nvarchar](50) COLLATE Latin1_General_BIN NOT NULL,
	[FundID] [int] NOT NULL,
	[Allocation] [float] NOT NULL,
	[isDeletable] [smallint] NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ModelPortfolio]    Script Date: 03/01/2012 14:29:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ModelPortfolio](
	[ModelID] [bigint] IDENTITY(1,1) NOT NULL,
	[ModelGroupID] [nvarchar](50) COLLATE Latin1_General_BIN NOT NULL,
	[ModelPortfolioID] [nvarchar](50) COLLATE Latin1_General_BIN NOT NULL,
	[ModelPortfolioName] [nvarchar](50) COLLATE Latin1_General_BIN NOT NULL,
	[ModelPortfolioDesc] [nvarchar](max) COLLATE Latin1_General_BIN NULL,
	[IsConsumed] [bit] NOT NULL,
 CONSTRAINT [PK_ModelPortfolio_1] PRIMARY KEY CLUSTERED 
(
	[ModelID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ModelGroup]    Script Date: 03/01/2012 14:29:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ModelGroup](
	[ModelGroupID] [nvarchar](50) COLLATE Latin1_General_BIN NOT NULL,
	[ModelGroupName] [nvarchar](50) COLLATE Latin1_General_BIN NOT NULL,
	[IFA_ID] [int] NOT NULL,
	[ModelGroupCode] [int] NOT NULL,
	[Date_Created] [datetime] NULL,
 CONSTRAINT [PK_ModelGroup] PRIMARY KEY CLUSTERED 
(
	[ModelGroupID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Switch_SMSTemplate]    Script Date: 03/01/2012 14:29:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Switch_SMSTemplate](
	[SMSTemplateID] [smallint] IDENTITY(1,1) NOT NULL,
	[TemplateName] [nvarchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[TemplateFor] [nchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[Message] [nvarchar](160) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
 CONSTRAINT [PK_Switch_SMSTemplate] PRIMARY KEY CLUSTERED 
(
	[SMSTemplateID] ASC,
	[TemplateName] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Switch_SMSTemplate] ON
INSERT [dbo].[Switch_SMSTemplate] ([SMSTemplateID], [TemplateName], [TemplateFor], [Message]) VALUES (1, N'ProposeSwitch', N'IFA proposing Switch to client                    ', N'Your IFA has proposed a change to the holdings in your {%PortfolioName%} portfolio. Please log into the NAV website to see the proposed change.')
INSERT [dbo].[Switch_SMSTemplate] ([SMSTemplateID], [TemplateName], [TemplateFor], [Message]) VALUES (2, N'SecurityCode', N'Sending security code to client                   ', N'Thank you for approving the change to your portfolio. Your security code is {%SecurityCode%}.')
INSERT [dbo].[Switch_SMSTemplate] ([SMSTemplateID], [TemplateName], [TemplateFor], [Message]) VALUES (3, N'Reset', N'Resetting the switch attempt                      ', N'The security code for your {%PortfolioName%} portfolio has been reset. Please log into the NAV website to confirm the proposed change.')
INSERT [dbo].[Switch_SMSTemplate] ([SMSTemplateID], [TemplateName], [TemplateFor], [Message]) VALUES (8, N'ProposeSchemeSwitch', N'*Official do not Delete!                          ', N'Your IFA has proposed a change to the holdings in your {%param_SchemeName%} Regular Savings. Please log into the NAV website to see the proposed change.')
INSERT [dbo].[Switch_SMSTemplate] ([SMSTemplateID], [TemplateName], [TemplateFor], [Message]) VALUES (9, N'SecurityCodeScheme', N'Sending scurity code to client (scheme switch)    ', N'Thank you for approving the change to your portfolio. Your security code is {%SecurityCode%}')
SET IDENTITY_INSERT [dbo].[Switch_SMSTemplate] OFF
/****** Object:  Table [dbo].[Switch_Output]    Script Date: 03/01/2012 14:29:46 ******/
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
/****** Object:  Table [dbo].[Switch_EmailTemplate]    Script Date: 03/01/2012 14:29:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Switch_EmailTemplate](
	[EmailTemplateID] [int] IDENTITY(1,1) NOT NULL,
	[TemplateName] [nvarchar](100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[Description] [nvarchar](100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[Body] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
 CONSTRAINT [PK_Switch_EmailTemplate] PRIMARY KEY CLUSTERED 
(
	[EmailTemplateID] ASC,
	[TemplateName] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Switch_EmailTemplate] ON
INSERT [dbo].[Switch_EmailTemplate] ([EmailTemplateID], [TemplateName], [Description], [Body]) VALUES (1, N'ClientRequestContact', N'Template use by client requesting contact to IFA for further Switch instructions', N'<table> <tr><td align=''left''>Dear {%param_IFAName%} </td></tr> <tr><td align=''left''>The following client has requested contact from you about the proposed switch to their portfolio.</td></tr> <tr><td align=''left''><b>Client:</b> {%param_ClientName%} </td></tr> <tr><td align=''left''><b>Portfolio:</b> {%param_PortfolioName%} </td></tr> <tr><td align=''left''><b>Telephone Number:</b> {%param_ContactNo%} </td></tr> <tr><td align=''left''><b>Message:</b> {%param_Comment%} </td></tr> <tr><td align=''left''>Thank you.</td></tr> </table>')
INSERT [dbo].[Switch_EmailTemplate] ([EmailTemplateID], [TemplateName], [Description], [Body]) VALUES (2, N'ClientAmendmentNotification', N'Email IFA that a client amended the proposed Switch', N'Your client ({%param_ClientName%}) had made some amendments to the proposed switch and is requesting your approval.')
INSERT [dbo].[Switch_EmailTemplate] ([EmailTemplateID], [TemplateName], [Description], [Body]) VALUES (3, N'ClientDeclineNotification', N'Notify IFA that client declined the proposed Switch', N'<table> <tr><td align=''left''>Dear {%param_IFAName%} </td></tr> <tr><td align=''left''>Your client declined this switch proposal.</td></tr> <tr><td align=''left''><b>Client:</b> {%param_ClientName%} </td></tr> <tr><td align=''left''><b>Portfolio:</b> {%param_PortfolioName%} </td></tr> <tr><td align=''left''><b>Reason:</b> {%param_Comment%} </td></tr> <tr><td align=''left''>Thank you.</td></tr> </table>')
INSERT [dbo].[Switch_EmailTemplate] ([EmailTemplateID], [TemplateName], [Description], [Body]) VALUES (4, N'SchemeClientAmendmentNotification', N'Email IFA that a client amended the proposed Scheme Switch', N'Your client ({%param_ClientName%}) had made some amendments to the proposed scheme switch and is requesting your approval.')
INSERT [dbo].[Switch_EmailTemplate] ([EmailTemplateID], [TemplateName], [Description], [Body]) VALUES (5, N'NotifyApprovedEmail', N'Email Template for Sending Switch Instructions that does not requires Client approval.', N'<p>Please find attached the switch instructions for switch number {%SwitchID%}. These instructions should be sent to {%Company%} for processing.</p><p>Thanks</p><p style=''font-weight:bolder;''>NAV</p>')
INSERT [dbo].[Switch_EmailTemplate] ([EmailTemplateID], [TemplateName], [Description], [Body]) VALUES (6, N'NotifyApprovedEmailReqSign', N'Email Template for Sending Switch Instructions that requires', N'<p>Please find attached the switch instructions for switch number {%SwitchID%}.</p><p>Please note that you will need to receive a signed copy of these instructions from your client which will then need to be sent to {%Company%} for processing.</p><p>Thanks</p><p style=''font-weight:bolder;''>NAV</p>')
INSERT [dbo].[Switch_EmailTemplate] ([EmailTemplateID], [TemplateName], [Description], [Body]) VALUES (7, N'NotifyClientApprovedEmail', N'Email Template for Sending Switch Instructions to Client that does not requires Client approval.', N'<p>Please find attached details of the switch instructions for your {%param_PortfolioName%} portfolio.</p> 
<p>These instructions will be sent to {%Company%} for processing by your IFA.</p>
<p>Thanks</p>
<p>NAV</p>')
INSERT [dbo].[Switch_EmailTemplate] ([EmailTemplateID], [TemplateName], [Description], [Body]) VALUES (8, N'NotifyClientApprovedEmailReqSign', N'Email Template for Sending Switch Instructions to Client that requires Client approval.', N'<p>Please find attached details of the switch instructions for your {%param_PortfolioName%} portfolio.</p> 
<p>Please print and sign a copy of these instructions and return this to your IFA so that the confirmed instructions can be sent to {%Company%} for processing.</p>
<p>Thanks</p>
<p>NAV</p>')
SET IDENTITY_INSERT [dbo].[Switch_EmailTemplate] OFF
/****** Object:  Table [dbo].[SwitchSchemeHistoryMessages]    Script Date: 03/01/2012 14:29:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SwitchSchemeHistoryMessages](
	[MessageID] [int] IDENTITY(1,1) NOT NULL,
	[HistoryID] [int] NOT NULL,
	[Message] [nvarchar](max) COLLATE Latin1_General_BIN NULL,
 CONSTRAINT [PK_SwitchSchemeHistoryMessages] PRIMARY KEY CLUSTERED 
(
	[MessageID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SwitchSchemeHistoryHeader]    Script Date: 03/01/2012 14:29:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SwitchSchemeHistoryHeader](
	[HistoryID] [int] IDENTITY(1,1) NOT NULL,
	[SchemeID] [nvarchar](50) COLLATE Latin1_General_BIN NOT NULL,
	[SwitchID] [int] NOT NULL,
	[Action_Date] [datetime] NOT NULL,
	[Status] [smallint] NOT NULL,
 CONSTRAINT [PK_SwitchSchemeHistoryHeader] PRIMARY KEY CLUSTERED 
(
	[HistoryID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SwitchSchemeHistoryDetails]    Script Date: 03/01/2012 14:29:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SwitchSchemeHistoryDetails](
	[HistoryDetailsID] [int] IDENTITY(1,1) NOT NULL,
	[HistoryID] [int] NOT NULL,
	[SwitchDetailsID] [int] NOT NULL,
	[FundID] [int] NOT NULL,
	[Allocation] [float] NOT NULL,
	[Date_Created] [datetime] NOT NULL,
	[Created_By] [nvarchar](50) COLLATE Latin1_General_BIN NOT NULL,
	[isContribution] [smallint] NOT NULL,
 CONSTRAINT [PK_SwitchSchemeHistoryDetails] PRIMARY KEY CLUSTERED 
(
	[HistoryDetailsID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SwitchSchemeHeader]    Script Date: 03/01/2012 14:29:46 ******/
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
/****** Object:  Table [dbo].[SwitchSchemeDetails_Client]    Script Date: 03/01/2012 14:29:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SwitchSchemeDetails_Client](
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
 CONSTRAINT [PK_SwitchSchemeDetails_Client] PRIMARY KEY CLUSTERED 
(
	[SwitchDetailsID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SwitchSchemeDetails]    Script Date: 03/01/2012 14:29:46 ******/
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
/****** Object:  Table [dbo].[SwitchSchemeClientSecurityCode]    Script Date: 03/01/2012 14:29:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SwitchSchemeClientSecurityCode](
	[CodeID] [int] IDENTITY(1,1) NOT NULL,
	[SwitchID] [int] NOT NULL,
	[Code] [nvarchar](20) COLLATE Latin1_General_BIN NOT NULL,
	[ClientID] [nvarchar](10) COLLATE Latin1_General_BIN NOT NULL,
	[SchemeID] [nvarchar](50) COLLATE Latin1_General_BIN NOT NULL,
	[IsConsumed] [nchar](1) COLLATE Latin1_General_BIN NOT NULL,
 CONSTRAINT [PK_SwitchSchemeClientSecurityCode] PRIMARY KEY CLUSTERED 
(
	[CodeID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SwitchSMSMessage]    Script Date: 03/01/2012 14:29:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SwitchSMSMessage](
	[SMS_ID] [int] NOT NULL,
	[SMS_Type] [nvarchar](20) COLLATE Latin1_General_BIN NULL,
	[SMS_Message] [nvarchar](max) COLLATE Latin1_General_BIN NULL,
 CONSTRAINT [PK_SwitchSMSMessage] PRIMARY KEY CLUSTERED 
(
	[SMS_ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
INSERT [dbo].[SwitchSMSMessage] ([SMS_ID], [SMS_Type], [SMS_Message]) VALUES (1, N'Notification', N'Your IFA has proposed a change to the holdings in your {0} portfolio. Please log into the NAV website to see the proposed change.')
INSERT [dbo].[SwitchSMSMessage] ([SMS_ID], [SMS_Type], [SMS_Message]) VALUES (2, N'SecurityCode', N'Thank you for approving the change to your portfolio. Your security code is {0}.')
INSERT [dbo].[SwitchSMSMessage] ([SMS_ID], [SMS_Type], [SMS_Message]) VALUES (3, N'CorrectCode', N'Thank you, the proposed changes will be made to your portfolio.')
INSERT [dbo].[SwitchSMSMessage] ([SMS_ID], [SMS_Type], [SMS_Message]) VALUES (4, N'IncorrectCode', N'Sorry, the security code you have entered is incorrect, please re-enter the security code. You have {0} more attempts.')
INSERT [dbo].[SwitchSMSMessage] ([SMS_ID], [SMS_Type], [SMS_Message]) VALUES (5, N'Locked', N'Sorry, you have entered the security code incorrectly three times. Please contact your IFA to have the security code reset.')
INSERT [dbo].[SwitchSMSMessage] ([SMS_ID], [SMS_Type], [SMS_Message]) VALUES (6, N'Reset', N'The security code for your {0} portfolio has been reset. Please log into the NAV website to confirm the proposed change.')
/****** Object:  Table [dbo].[SwitchHistoryMessages]    Script Date: 03/01/2012 14:29:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SwitchHistoryMessages](
	[MessageID] [int] IDENTITY(1,1) NOT NULL,
	[HistoryID] [int] NOT NULL,
	[Message] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
 CONSTRAINT [PK_SwitchHistoryMessages_1] PRIMARY KEY CLUSTERED 
(
	[MessageID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SwitchHistoryHeader]    Script Date: 03/01/2012 14:29:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SwitchHistoryHeader](
	[HistoryID] [int] IDENTITY(1,1) NOT NULL,
	[PortfolioID] [nvarchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[SwitchID] [int] NOT NULL,
	[Action_Date] [datetime] NOT NULL,
	[Status] [smallint] NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SwitchHistoryDetails]    Script Date: 03/01/2012 14:29:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SwitchHistoryDetails](
	[HistoryDetailsID] [int] IDENTITY(1,1) NOT NULL,
	[HistoryID] [int] NOT NULL,
	[SwitchDetailsID] [int] NOT NULL,
	[FundID] [int] NOT NULL,
	[Allocation] [float] NOT NULL,
	[Date_Created] [datetime] NOT NULL,
	[Created_By] [nvarchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
 CONSTRAINT [PK_SwitchHistoryDetails] PRIMARY KEY CLUSTERED 
(
	[HistoryDetailsID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SwitchHeaderTemp]    Script Date: 03/01/2012 14:29:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SwitchHeaderTemp](
	[IFA_ID] [int] NOT NULL,
	[ModelID] [int] NOT NULL,
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
/****** Object:  Table [dbo].[SwitchHeader]    Script Date: 03/01/2012 14:29:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SwitchHeader](
	[PortfolioID] [nvarchar](50) COLLATE Latin1_General_BIN NOT NULL,
	[ClientID] [nvarchar](50) COLLATE Latin1_General_BIN NOT NULL,
	[SwitchID] [int] IDENTITY(1,1) NOT NULL,
	[Status] [smallint] NOT NULL,
	[Date_Created] [datetime] NOT NULL,
	[Created_By] [nvarchar](50) COLLATE Latin1_General_BIN NOT NULL,
	[Date_Updated] [datetime] NULL,
	[Updated_By] [nvarchar](50) COLLATE Latin1_General_BIN NULL,
	[SecurityCodeAttempt] [int] NOT NULL,
	[Description] [nvarchar](max) COLLATE Latin1_General_BIN NULL,
	[Amend_Status] [smallint] NULL,
	[Amend_Description] [nvarchar](max) COLLATE Latin1_General_BIN NULL,
	[ModelID] [int] NULL,
	[ModelGroupID] [nvarchar](50) COLLATE Latin1_General_BIN NULL,
	[ModelPortfolioID] [nvarchar](50) COLLATE Latin1_General_BIN NULL,
	[IsModelCustomized] [bit] NOT NULL,
 CONSTRAINT [PK_SwitchHeader] PRIMARY KEY CLUSTERED 
(
	[SwitchID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SwitchGenerateCode]    Script Date: 03/01/2012 14:29:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SwitchGenerateCode](
	[Code] [nvarchar](16) COLLATE Latin1_General_BIN NOT NULL,
	[DateCreated] [datetime] NOT NULL,
	[DateValidUntil]  AS (dateadd(day,(30),[DateCreated])),
 CONSTRAINT [PK_SwitchGenerateCode_1] PRIMARY KEY CLUSTERED 
(
	[Code] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SwitchFeeHistory]    Script Date: 03/01/2012 14:29:46 ******/
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
 CONSTRAINT [PK_SwitchFeeHistory] PRIMARY KEY CLUSTERED 
(
	[HistoryID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SwitchFee]    Script Date: 03/01/2012 14:29:46 ******/
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
	[Date_Updated] [datetime] NULL,
	[Access_Denied] [bit] NOT NULL,
 CONSTRAINT [PK_SwitchFee] PRIMARY KEY CLUSTERED 
(
	[IFA_ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
INSERT [dbo].[SwitchFee] ([IFA_ID], [IFA_Username], [Annual_Fee], [Per_Switch_Fee], [Date_Created], [Date_Updated], [Access_Denied]) VALUES (5, N'Demo', CAST(0.00 AS Decimal(18, 2)), CAST(2.00 AS Decimal(18, 2)), CAST(0x0000A00500B363E4 AS DateTime), CAST(0x0000A00100AD2277 AS DateTime), 0)
INSERT [dbo].[SwitchFee] ([IFA_ID], [IFA_Username], [Annual_Fee], [Per_Switch_Fee], [Date_Created], [Date_Updated], [Access_Denied]) VALUES (97, N'NAV Demo', CAST(0.00 AS Decimal(18, 2)), CAST(3.00 AS Decimal(18, 2)), CAST(0x0000A00500B363F6 AS DateTime), CAST(0x0000A00100AD2863 AS DateTime), 0)
/****** Object:  Table [dbo].[SwitchEmail_Logs]    Script Date: 03/01/2012 14:29:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SwitchEmail_Logs](
	[EmailID] [int] IDENTITY(1,1) NOT NULL,
	[SwitchID] [int] NOT NULL,
	[Recipient] [nvarchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[ClientID] [nvarchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[Message] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[DateSent] [datetime] NOT NULL,
	[Purpose] [nvarchar](100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
 CONSTRAINT [PK_SwitchEmail_Logs] PRIMARY KEY CLUSTERED 
(
	[EmailID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SwitchDetails_Client]    Script Date: 03/01/2012 14:29:46 ******/
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
/****** Object:  Table [dbo].[SwitchDetailsTemp]    Script Date: 03/01/2012 14:29:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SwitchDetailsTemp](
	[ModelID] [int] NOT NULL,
	[ClientID] [nvarchar](50) COLLATE Latin1_General_BIN NOT NULL,
	[PortfolioID] [nvarchar](50) COLLATE Latin1_General_BIN NOT NULL,
	[FundID] [int] NOT NULL,
	[Allocation] [float] NOT NULL,
	[Date_Created] [datetime] NULL,
	[Created_By] [nvarchar](50) COLLATE Latin1_General_BIN NULL,
	[Date_Updated] [datetime] NULL,
	[Updated_By] [nvarchar](50) COLLATE Latin1_General_BIN NULL,
	[isDeletable] [smallint] NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SwitchDetails]    Script Date: 03/01/2012 14:29:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SwitchDetails](
	[SwitchDetailsID] [int] IDENTITY(1,1) NOT NULL,
	[SwitchID] [int] NOT NULL,
	[FundID] [int] NOT NULL,
	[Allocation] [float] NOT NULL,
	[Date_Created] [datetime] NOT NULL,
	[Created_By] [nvarchar](50) COLLATE Latin1_General_BIN NOT NULL,
	[Date_LastUpdate] [datetime] NULL,
	[Updated_By] [nvarchar](50) COLLATE Latin1_General_BIN NULL,
	[isDeletable] [smallint] NOT NULL,
 CONSTRAINT [PK_SwitchDetails] PRIMARY KEY CLUSTERED 
(
	[SwitchDetailsID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SwitchClientSecurityCode]    Script Date: 03/01/2012 14:29:46 ******/
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
/****** Object:  Table [dbo].[SessionState]    Script Date: 03/01/2012 14:29:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[SessionState](
	[GUID] [char](38) COLLATE Latin1_General_BIN NOT NULL,
	[Session] [varchar](max) COLLATE Latin1_General_BIN NOT NULL,
	[Destination] [varchar](max) COLLATE Latin1_General_BIN NOT NULL,
	[DateCreated] [datetime] NOT NULL,
 CONSTRAINT [PK_SessionState] PRIMARY KEY CLUSTERED 
(
	[GUID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  View [dbo].[PerSwitchFeeReport]    Script Date: 03/01/2012 14:29:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[PerSwitchFeeReport]
AS
SELECT 
------------Create a column to hold all SwitchID belongs to Per Switch Fee------------------
stuff((SELECT ',' + CONVERT(VARCHAR(3),a.SwitchID) FROM NavIntegrationDB.dbo.SwitchHeader a 
	INNER JOIN (
	SELECT 
		a.IFA_ID,
		a.Annual_Fee,
		a.Per_Switch_Fee,
		CONVERT(VARCHAR(19),a.Date_Effectivity,121) AS Start_Date_Effective,
		CONVERT(VARCHAR(19),(DATEADD(SECOND,-1,
												(ISNULL((SELECT TOP 1 b.Date_Effectivity FROM
																						(SELECT IFA_ID, Annual_Fee, Per_Switch_Fee, Date_Updated AS Date_Effectivity FROM NavIntegrationDB.dbo.SwitchFee
																						UNION
																						SELECT IFA_ID, Annual_Fee, Per_Switch_Fee, Date_Effectivity FROM NavIntegrationDB.dbo.SwitchFeeHistory) AS b 
														WHERE b.IFA_ID = a.IFA_ID and b.Date_Effectivity > a.Date_Effectivity ORDER BY b.Date_Effectivity ASC), 
												GETDATE())))),
		121) AS End_Date_Effective
	FROM
		(
		SELECT IFA_ID, Annual_Fee, Per_Switch_Fee, Date_Updated AS Date_Effectivity FROM NavIntegrationDB.dbo.SwitchFee
		UNION
		SELECT IFA_ID, Annual_Fee, Per_Switch_Fee, Date_Effectivity FROM NavIntegrationDB.dbo.SwitchFeeHistory
		) AS a ) y ON a.Date_Created BETWEEN y.Start_Date_Effective AND y.End_Date_Effective AND y.IFA_ID=I.IFA_ID AND y.Per_Switch_Fee=TBLFEE.Per_Switch_Fee
								 ORDER BY SwitchID
								   FOR XML PATH('')
								),1,1,'')  as [Switches],
-----------------------------------------------------------------------------------------------
	 I.IFA_ID
	,I.IFA_Name
	,TBLFEE.Per_Switch_Fee
	,COUNT(SH.SwitchID) AS Quantity
	,(SELECT MIN(Date_Created) AS StartDate FROM NavIntegrationDB.dbo.SwitchHeader a INNER JOIN NavGlobalDBwwwGUID.dbo.Client b ON a.ClientID=b.ClientID WHERE b.IFA_ID = C.IFA_ID) AS StartDate
	,(SELECT MAX(Date_Created) AS StartDate FROM NavIntegrationDB.dbo.SwitchHeader a INNER JOIN NavGlobalDBwwwGUID.dbo.Client b ON a.ClientID=b.ClientID WHERE b.IFA_ID = C.IFA_ID) AS EndDate
	,SH.Status
FROM NavIntegrationDB.dbo.SwitchHeader SH
INNER JOIN NavGlobalDBwwwGUID.dbo.Client AS C ON C.ClientID=SH.ClientID
INNER JOIN NavGlobalDBwwwGUID.dbo.IFA AS I ON I.IFA_ID=C.IFA_ID
INNER JOIN (
SELECT 
	a.IFA_ID,
	a.Annual_Fee,
	a.Per_Switch_Fee,
    CONVERT(VARCHAR(19),a.Date_Effectivity,121) AS Start_Date_Effective,
    CONVERT(VARCHAR(19),(DATEADD(SECOND,-1,
											(ISNULL((SELECT TOP 1 b.Date_Effectivity FROM
																					(SELECT IFA_ID, Annual_Fee, Per_Switch_Fee, Date_Updated AS Date_Effectivity FROM NavIntegrationDB.dbo.SwitchFee
																					UNION
																					SELECT IFA_ID, Annual_Fee, Per_Switch_Fee, Date_Effectivity FROM NavIntegrationDB.dbo.SwitchFeeHistory) AS b 
													WHERE b.IFA_ID = a.IFA_ID and b.Date_Effectivity > a.Date_Effectivity ORDER BY b.Date_Effectivity ASC), 
											GETDATE())))),
	121) AS End_Date_Effective
FROM
	(
	SELECT IFA_ID, Annual_Fee, Per_Switch_Fee, Date_Updated AS Date_Effectivity FROM NavIntegrationDB.dbo.SwitchFee
	UNION
	SELECT IFA_ID, Annual_Fee, Per_Switch_Fee, Date_Effectivity FROM NavIntegrationDB.dbo.SwitchFeeHistory
	) AS a ) TBLFEE ON TBLFEE.IFA_ID = I.IFA_ID
WHERE 
	SH.Status = 6
AND 
	SH.Date_Created --temporary it will change when update date is applied
	BETWEEN TBLFEE.Start_Date_Effective AND TBLFEE.End_Date_Effective
GROUP BY  I.IFA_ID, I.IFA_Name, TBLFEE.Per_Switch_Fee, SH.Status, C.IFA_ID
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[29] 4[23] 2[29] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "SH"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 125
               Right = 246
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "C"
            Begin Extent = 
               Top = 6
               Left = 284
               Bottom = 125
               Right = 484
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "I"
            Begin Extent = 
               Top = 6
               Left = 522
               Bottom = 125
               Right = 730
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "TBLFEE"
            Begin Extent = 
               Top = 6
               Left = 768
               Bottom = 125
               Right = 975
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 9
         Width = 284
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 4155
         Width = 4740
         Width = 1500
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 12
         Column = 5430
         Alias = 1785
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'PerSwitchFeeReport'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'PerSwitchFeeReport'
GO
/****** Object:  Default [DF_ModelGroup_ModelGroupCode]    Script Date: 03/01/2012 14:29:46 ******/
ALTER TABLE [dbo].[ModelGroup] ADD  CONSTRAINT [DF_ModelGroup_ModelGroupCode]  DEFAULT ((210)) FOR [ModelGroupCode]
GO
/****** Object:  Default [DF_ModelGroup_Date_Created]    Script Date: 03/01/2012 14:29:46 ******/
ALTER TABLE [dbo].[ModelGroup] ADD  CONSTRAINT [DF_ModelGroup_Date_Created]  DEFAULT (getdate()) FOR [Date_Created]
GO
/****** Object:  Default [DF_ModelPortfolio_IsConsumed]    Script Date: 03/01/2012 14:29:46 ******/
ALTER TABLE [dbo].[ModelPortfolio] ADD  CONSTRAINT [DF_ModelPortfolio_IsConsumed]  DEFAULT ((0)) FOR [IsConsumed]
GO
/****** Object:  Default [DF_ModelPortfolioDetails_ModelID]    Script Date: 03/01/2012 14:29:46 ******/
ALTER TABLE [dbo].[ModelPortfolioDetails] ADD  CONSTRAINT [DF_ModelPortfolioDetails_ModelID]  DEFAULT ((0)) FOR [ModelID]
GO
/****** Object:  Default [DF_ModelPortfolioDetails_isDeletable]    Script Date: 03/01/2012 14:29:46 ******/
ALTER TABLE [dbo].[ModelPortfolioDetails] ADD  CONSTRAINT [DF_ModelPortfolioDetails_isDeletable]  DEFAULT ((0)) FOR [isDeletable]
GO
/****** Object:  Default [DF_SessionState_DateCreated]    Script Date: 03/01/2012 14:29:46 ******/
ALTER TABLE [dbo].[SessionState] ADD  CONSTRAINT [DF_SessionState_DateCreated]  DEFAULT (getdate()) FOR [DateCreated]
GO
/****** Object:  Default [DF_SwitchClientSecurityCode_Attempt]    Script Date: 03/01/2012 14:29:46 ******/
ALTER TABLE [dbo].[SwitchClientSecurityCode] ADD  CONSTRAINT [DF_SwitchClientSecurityCode_Attempt]  DEFAULT ((0)) FOR [Attempt]
GO
/****** Object:  Default [DF_SwitchClientCode_IsConsumed]    Script Date: 03/01/2012 14:29:46 ******/
ALTER TABLE [dbo].[SwitchClientSecurityCode] ADD  CONSTRAINT [DF_SwitchClientCode_IsConsumed]  DEFAULT ((0)) FOR [IsConsumed]
GO
/****** Object:  Default [DF_SwitchDetails_isDeletable]    Script Date: 03/01/2012 14:29:46 ******/
ALTER TABLE [dbo].[SwitchDetails] ADD  CONSTRAINT [DF_SwitchDetails_isDeletable]  DEFAULT ((0)) FOR [isDeletable]
GO
/****** Object:  Default [DF_SwitchDetailsTemp_isDeletable]    Script Date: 03/01/2012 14:29:46 ******/
ALTER TABLE [dbo].[SwitchDetailsTemp] ADD  CONSTRAINT [DF_SwitchDetailsTemp_isDeletable]  DEFAULT ((0)) FOR [isDeletable]
GO
/****** Object:  Default [DF_SwitchDetails_Client_isDeletable]    Script Date: 03/01/2012 14:29:46 ******/
ALTER TABLE [dbo].[SwitchDetails_Client] ADD  CONSTRAINT [DF_SwitchDetails_Client_isDeletable]  DEFAULT ((0)) FOR [isDeletable]
GO
/****** Object:  Default [DF_SwitchFee_Annual_Fee]    Script Date: 03/01/2012 14:29:46 ******/
ALTER TABLE [dbo].[SwitchFee] ADD  CONSTRAINT [DF_SwitchFee_Annual_Fee]  DEFAULT ((0)) FOR [Annual_Fee]
GO
/****** Object:  Default [DF_SwitchFee_Per_Switch_Fee]    Script Date: 03/01/2012 14:29:46 ******/
ALTER TABLE [dbo].[SwitchFee] ADD  CONSTRAINT [DF_SwitchFee_Per_Switch_Fee]  DEFAULT ((0)) FOR [Per_Switch_Fee]
GO
/****** Object:  Default [DF_SwitchFee_Date_Created]    Script Date: 03/01/2012 14:29:46 ******/
ALTER TABLE [dbo].[SwitchFee] ADD  CONSTRAINT [DF_SwitchFee_Date_Created]  DEFAULT (getdate()) FOR [Date_Created]
GO
/****** Object:  Default [DF_SwitchFee_Switch_Access]    Script Date: 03/01/2012 14:29:46 ******/
ALTER TABLE [dbo].[SwitchFee] ADD  CONSTRAINT [DF_SwitchFee_Switch_Access]  DEFAULT ((0)) FOR [Access_Denied]
GO
/****** Object:  Default [DF_SwitchFeeHistory_Annual_Fee]    Script Date: 03/01/2012 14:29:46 ******/
ALTER TABLE [dbo].[SwitchFeeHistory] ADD  CONSTRAINT [DF_SwitchFeeHistory_Annual_Fee]  DEFAULT ((0)) FOR [Annual_Fee]
GO
/****** Object:  Default [DF_SwitchFeeHistory_Per_Switch_Fee]    Script Date: 03/01/2012 14:29:46 ******/
ALTER TABLE [dbo].[SwitchFeeHistory] ADD  CONSTRAINT [DF_SwitchFeeHistory_Per_Switch_Fee]  DEFAULT ((0)) FOR [Per_Switch_Fee]
GO
/****** Object:  Default [DF_SwitchFeeHistory_Date_Created]    Script Date: 03/01/2012 14:29:46 ******/
ALTER TABLE [dbo].[SwitchFeeHistory] ADD  CONSTRAINT [DF_SwitchFeeHistory_Date_Created]  DEFAULT (getdate()) FOR [Date_Created]
GO
/****** Object:  Default [DF_Table_1_Date_Created]    Script Date: 03/01/2012 14:29:46 ******/
ALTER TABLE [dbo].[SwitchFeeHistory] ADD  CONSTRAINT [DF_Table_1_Date_Created]  DEFAULT (getdate()) FOR [Date_Effectivity]
GO
/****** Object:  Default [DF_SwitchGenerateCode_DateCreated]    Script Date: 03/01/2012 14:29:46 ******/
ALTER TABLE [dbo].[SwitchGenerateCode] ADD  CONSTRAINT [DF_SwitchGenerateCode_DateCreated]  DEFAULT (getdate()) FOR [DateCreated]
GO
/****** Object:  Default [DF_SwitchHeader_Date_Created]    Script Date: 03/01/2012 14:29:46 ******/
ALTER TABLE [dbo].[SwitchHeader] ADD  CONSTRAINT [DF_SwitchHeader_Date_Created]  DEFAULT (getdate()) FOR [Date_Created]
GO
/****** Object:  Default [DF_SwitchHeader_SecurityCodeAttempt]    Script Date: 03/01/2012 14:29:46 ******/
ALTER TABLE [dbo].[SwitchHeader] ADD  CONSTRAINT [DF_SwitchHeader_SecurityCodeAttempt]  DEFAULT ((0)) FOR [SecurityCodeAttempt]
GO
/****** Object:  Default [DF_SwitchHeader_IsModelCustomized]    Script Date: 03/01/2012 14:29:46 ******/
ALTER TABLE [dbo].[SwitchHeader] ADD  CONSTRAINT [DF_SwitchHeader_IsModelCustomized]  DEFAULT ((0)) FOR [IsModelCustomized]
GO
/****** Object:  Default [DF_SwitchSchemeClientCode_IsConsumed]    Script Date: 03/01/2012 14:29:46 ******/
ALTER TABLE [dbo].[SwitchSchemeClientSecurityCode] ADD  CONSTRAINT [DF_SwitchSchemeClientCode_IsConsumed]  DEFAULT ((0)) FOR [IsConsumed]
GO
/****** Object:  Default [DF_SwitchSchemeDetails_isDeletable]    Script Date: 03/01/2012 14:29:46 ******/
ALTER TABLE [dbo].[SwitchSchemeDetails] ADD  CONSTRAINT [DF_SwitchSchemeDetails_isDeletable]  DEFAULT ((0)) FOR [isDeletable]
GO
/****** Object:  Default [DF_SwitchSchemeDetails_isContribution]    Script Date: 03/01/2012 14:29:46 ******/
ALTER TABLE [dbo].[SwitchSchemeDetails] ADD  CONSTRAINT [DF_SwitchSchemeDetails_isContribution]  DEFAULT ((0)) FOR [isContribution]
GO
/****** Object:  Default [DF_SwitchSchemeDetails_Client_isDeletable]    Script Date: 03/01/2012 14:29:46 ******/
ALTER TABLE [dbo].[SwitchSchemeDetails_Client] ADD  CONSTRAINT [DF_SwitchSchemeDetails_Client_isDeletable]  DEFAULT ((0)) FOR [isDeletable]
GO
/****** Object:  Default [DF_SwitchSchemeDetails_Client_isContribution]    Script Date: 03/01/2012 14:29:46 ******/
ALTER TABLE [dbo].[SwitchSchemeDetails_Client] ADD  CONSTRAINT [DF_SwitchSchemeDetails_Client_isContribution]  DEFAULT ((0)) FOR [isContribution]
GO
/****** Object:  Default [DF_SwitchSchemeHeader_SecurityCodeAttempt]    Script Date: 03/01/2012 14:29:46 ******/
ALTER TABLE [dbo].[SwitchSchemeHeader] ADD  CONSTRAINT [DF_SwitchSchemeHeader_SecurityCodeAttempt]  DEFAULT ((0)) FOR [SecurityCodeAttempt]
GO
