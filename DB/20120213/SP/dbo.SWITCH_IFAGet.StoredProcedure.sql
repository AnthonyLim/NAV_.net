USE [NavIntegrationDB]
GO
/****** Object:  StoredProcedure [dbo].[SWITCH_IFAGet]    Script Date: 02/13/2012 17:17:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SWITCH_IFAGet] 

@param_intIFA_ID int

AS
BEGIN

	SET NOCOUNT ON;
	
SELECT [IFA_ID]
      ,[IFA_Code]
      ,[IFA_Name]
      ,[IFA_Currency]
      ,[IFAUpdatedDate]
      ,[IFAAdd1]
      ,[IFAAdd2]
      ,[IFAAdd3]
      ,[IFACountry]
      ,[IFAPrincipal]
      ,[IFATel]
      ,[IFAFax]
      ,[IFAEmail]
      ,[IFAWebsiteContact]
      ,[IFATechContact]
      ,[IFADataEntryEmail]
      ,[IFAShowOnWeb]
      ,[SuperIFAID]
      ,[IncludeInBilling]
      ,[IFAODEEmail]
      ,[IFAPaymentReceived]
      ,[ContributionsOnWeb]
      ,[ClientNamesOnWeb]
      ,[ClientDetailsOnWeb]

FROM [NavGlobalDBwwwGUID].[dbo].[IFA]
WHERE ([IFA_ID] = @param_intIFA_ID OR @param_intIFA_ID = '')
	

END
GO
