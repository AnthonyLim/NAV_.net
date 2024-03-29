USE [NavIntegrationDB]
GO
/****** Object:  StoredProcedure [dbo].[SWITCH_CompanyGet]    Script Date: 02/13/2012 17:17:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SWITCH_CompanyGet] 

 @param_intCompanyID int

AS
BEGIN

 SET NOCOUNT ON;

 SELECT [CompanyID]
    ,[Company]
    ,[CompanyAdd1]
    ,[CompanyAdd2]
    ,[CompanyAdd3]
    ,[CompanyCountry]
    ,[CompanyTel]
    ,[CompanyFax]
    ,[CompanyEmail]
    ,[CompanyWebSite]
    ,[CompanyType]
    ,[FeedListID]
    
 FROM [NavGlobalDBwwwGUID].[dbo].[Company]

 WHERE CompanyID = @param_intCompanyID 

END
GO
