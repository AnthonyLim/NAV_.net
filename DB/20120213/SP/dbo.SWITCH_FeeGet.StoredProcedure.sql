USE [NavIntegrationDB]
GO
/****** Object:  StoredProcedure [dbo].[SWITCH_FeeGet]    Script Date: 02/13/2012 17:17:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SWITCH_FeeGet]

@param_intIFA_ID		INT

AS
BEGIN
	SELECT [IFA_ID]
      ,[IFA_Username]
      ,[Annual_Fee]
      ,[Per_Switch_Fee]
      ,[Date_Created]
      ,[Created_By]
      ,[Access_Denied]
	FROM [NavIntegrationDB].[dbo].[SwitchFee]
	WHERE (IFA_ID = @param_intIFA_ID OR @param_intIFA_ID = 0)
END
GO
