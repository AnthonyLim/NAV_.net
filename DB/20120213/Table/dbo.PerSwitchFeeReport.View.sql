USE [NavIntegrationDB]
GO
/****** Object:  View [dbo].[PerSwitchFeeReport]    Script Date: 02/13/2012 17:15:05 ******/
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
