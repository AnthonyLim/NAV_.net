USE [NavIntegrationDB]
GO
/****** Object:  Table [dbo].[SwitchSMSMessage]    Script Date: 02/13/2012 17:15:04 ******/
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
