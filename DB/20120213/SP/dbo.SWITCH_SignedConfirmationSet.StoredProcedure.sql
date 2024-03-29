USE [NavIntegrationDB]
GO
/****** Object:  StoredProcedure [dbo].[SWITCH_SignedConfirmationSet]    Script Date: 02/13/2012 17:17:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SWITCH_SignedConfirmationSet]

 @CompanyID int,
 @status bit
AS
BEGIN
 SET NOCOUNT ON;

    if @status = 1
  begin
   if not exists(select * from Switch_EmailSignedConfirmation where CompanyID = @CompanyID)
   begin
    insert into Switch_EmailSignedConfirmation(CompanyID) values(@CompanyID)
   end
  end
    else
  begin
   delete from Switch_EmailSignedConfirmation where CompanyID = @CompanyID
  end
 
END
GO
