USE [NavIntegrationDB]
GO
/****** Object:  Table [dbo].[Switch_EmailTemplate]    Script Date: 02/13/2012 17:15:04 ******/
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
