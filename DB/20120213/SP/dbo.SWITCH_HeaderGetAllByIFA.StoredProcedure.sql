USE [NavIntegrationDB]
GO
/****** Object:  StoredProcedure [dbo].[SWITCH_HeaderGetAllByIFA]    Script Date: 02/13/2012 17:17:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SWITCH_HeaderGetAllByIFA]

@param_IFA_ID		NVARCHAR(50),
@param_ClientName	NVARCHAR(50),
@param_Company		NVARCHAR(50),
@param_Status		INT,
@param_StartDate	NVARCHAR(10),
@param_EndDate		NVARCHAR(10)

AS
BEGIN
	SELECT 
		 SH.SwitchID
		,SH.PortfolioID
		,SH.ClientID 
		,SH.Status 
		,SH.Date_Created
		,SH.Created_By
		,SH.SecurityCodeAttempt
		,SH.Description
	FROM NavIntegrationDB.dbo.SwitchHeader SH 
	INNER JOIN NavGlobalDBwwwGUID.dbo.Portfolio P on P.ClientID = SH.ClientID AND P.PortfolioID = SH.PortfolioID
	INNER JOIN NavGlobalDBwwwGUID.dbo.Client C ON C.ClientID = SH.ClientID
	INNER JOIN NavGlobalDBwwwGUID.dbo.ClientWebDetails CWD ON CWD.ClientID = SH.ClientID
	INNER JOIN NavGlobalDBwwwGUID.dbo.Company CO ON CO.CompanyID = P.Company
	
	WHERE C.IFA_ID = @param_IFA_ID
	AND (UPPER(CWD.Forenames) + ' ' +UPPER(CWD.Surname) LIKE '%' + UPPER(@param_ClientName) + '%' OR @param_ClientName = '')
	AND (UPPER(CO.Company) LIKE '%' + UPPER(@param_Company) + '%' OR @param_Company = '')
	AND (SH.Status = @param_Status OR @param_Status = '0')
	AND (CONVERT(VARCHAR(10),SH.Date_Created,105) >= @param_StartDate OR @param_StartDate = '')
	AND (CONVERT(VARCHAR(10),SH.Date_Created,105) <= @param_EndDate OR @param_EndDate = '' )
	AND SH.Status > 0
	ORDER BY SH.Date_Created DESC

END
GO
