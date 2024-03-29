USE [NavIntegrationDB]
GO
/****** Object:  StoredProcedure [dbo].[SWITCH_ModelPortfolioDetailsDelete]    Script Date: 02/13/2012 17:17:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SWITCH_ModelPortfolioDetailsDelete]

@param_IFA_ID				int,
@param_ModelGroupID			nvarchar(50),
@param_ModelPortfolioID		nvarchar(50)


AS
BEGIN
	DELETE 
	FROM	[NavIntegrationDB].[dbo].[ModelPortfolioDetails]
	WHERE	IFA_ID = @param_IFA_ID
	AND		ModelGroupID = @param_ModelGroupID
	AND		ModelPortfolioID = @param_ModelPortfolioID
END
GO
