using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using NAV.Scheme.UserControl;

namespace NAV.Scheme
{
    public partial class SchemeHistory : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
                int intSwitchID = int.Parse(Request.QueryString["SID"].ToString());
                string strSchemeID = Request.QueryString["SchID"].ToString();
                string strClientID = Request.QueryString["CID"].ToString();

                clsScheme Scheme = new clsScheme(strClientID, strSchemeID);

                foreach (clsHistory.clsHistoryScheme History in clsHistory.clsHistoryScheme.getListHistory(strSchemeID, intSwitchID))
                {
                    switch (History.propStatus)
                    {
                        case 0: //draft
                            break;
                        case 1: //save
                            break;
                        case 2: //proposed

                            ucProposalDisplay ucProposed = (ucProposalDisplay)LoadControl("UserControl/ucProposalDisplay.ascx");
                            ucProposed.propUserLevel = ucProposalDisplay.enumUserLevel.Client;
                            ucProposed.propIsDetailsAsGivenParameter = true;
                            ucProposed.propSwitchDetailsList = ucProposalDisplay.clsSwitchDetailsList.convertSwitchDetails(clsHistory.clsHistoryScheme.getSwitchDetails(Scheme, intSwitchID, History.propHistoryID, false));
                            ucProposed.propSwitchDetailsListContribution = ucProposalDisplay.clsSwitchDetailsList.convertSwitchDetails(clsHistory.clsHistoryScheme.getSwitchDetails(Scheme, intSwitchID, History.propHistoryID, true));
                            ucProposed.propStatus = clsSwitch.getSwitchStringStatus(History.propStatus) + " (" + History.propAction_Date + ")";
                            ucProposed.propDescription = clsHistory.clsHistoryScheme.getMessage(History.propHistoryID);
                            this.divHistoryHolder.Controls.Add(ucProposed);
                            break;
                        case 3: //amended
                                                       
                            ucProposalDisplay ucAmended = (ucProposalDisplay)LoadControl("UserControl/ucProposalDisplay.ascx");                                                        
                            ucAmended.propUserLevel = ucProposalDisplay.enumUserLevel.IFA;
                            ucAmended.propIsDetailsAsGivenParameter = true;
                            ucAmended.propSwitchDetailsList = ucProposalDisplay.clsSwitchDetailsList.convertSwitchDetails(clsHistory.clsHistoryScheme.getSwitchDetails(Scheme, intSwitchID, History.propHistoryID, false));
                            ucAmended.propSwitchDetailsListContribution = ucProposalDisplay.clsSwitchDetailsList.convertSwitchDetails(clsHistory.clsHistoryScheme.getSwitchDetails(Scheme, intSwitchID, History.propHistoryID, true));
                            ucAmended.propStatus = clsSwitch.getSwitchStringStatus(History.propStatus) + " (" + History.propAction_Date + ")";
                            ucAmended.propDescription = clsHistory.clsHistoryScheme.getMessage(History.propHistoryID);
                            this.divHistoryHolder.Controls.Add(ucAmended);
                            break;
                        case 4: //decline IFA
                            break;
                        case 5: //decline Client
                            ucProposalDisplay ucDeclinedClient = (ucProposalDisplay)LoadControl("UserControl/ucProposalDisplay.ascx");
                            ucDeclinedClient.propUserLevel = ucProposalDisplay.enumUserLevel.Client;
                            ucDeclinedClient.propIsDetailsAsGivenParameter = true;
                            ucDeclinedClient.propStatus = clsSwitch.getSwitchStringStatus(History.propStatus) + " (" + History.propAction_Date + ")";
                            ucDeclinedClient.propDescription = clsHistory.clsHistoryScheme.getMessage(History.propHistoryID);
                            this.divHistoryHolder.Controls.Add(ucDeclinedClient);
                            break;
                        case 6: //Approved
                            ucProposalDisplay ucApproved = (ucProposalDisplay)LoadControl("UserControl/ucProposalDisplay.ascx");
                            ucApproved.propUserLevel = ucProposalDisplay.enumUserLevel.Client;
                            ucApproved.propIsDetailsAsGivenParameter = true;
                            ucApproved.propSwitchDetailsList = ucProposalDisplay.clsSwitchDetailsList.convertSwitchDetails(clsHistory.clsHistoryScheme.getSwitchDetails(Scheme, intSwitchID, History.propHistoryID, false));
                            ucApproved.propSwitchDetailsListContribution = ucProposalDisplay.clsSwitchDetailsList.convertSwitchDetails(clsHistory.clsHistoryScheme.getSwitchDetails(Scheme, intSwitchID, History.propHistoryID, true));
                            ucApproved.propStatus = clsSwitch.getSwitchStringStatus(History.propStatus) + " (" + History.propAction_Date + ")";
                            ucApproved.propDescription = clsHistory.clsHistoryScheme.getMessage(History.propHistoryID);
                            this.divHistoryHolder.Controls.Add(ucApproved);
                            break;
                        case 7: //lock
                            break;
                        case 8: //request for discussion
                            ucProposalDisplay ucRequest = (ucProposalDisplay)LoadControl("UserControl/ucProposalDisplay.ascx");
                            ucRequest.propUserLevel = ucProposalDisplay.enumUserLevel.Client;
                            ucRequest.propIsDetailsAsGivenParameter = true;
                            ucRequest.propStatus = clsSwitch.getSwitchStringStatus(History.propStatus) + " (" + History.propAction_Date + ")";
                            ucRequest.propDescription = clsHistory.clsHistoryScheme.getMessage(History.propHistoryID);
                            this.divHistoryHolder.Controls.Add(ucRequest);
                            break;
                        case 9: //Cancelled
                            break;
                        case 10: //Completed
                            ucProposalDisplay ucCompleted = (ucProposalDisplay)LoadControl("UserControl/ucProposalDisplay.ascx");
                            ucCompleted.propUserLevel = ucProposalDisplay.enumUserLevel.Client;
                            ucCompleted.propIsDetailsAsGivenParameter = true;
                            ucCompleted.propStatus = clsSwitch.getSwitchStringStatus(History.propStatus) + " (" + History.propAction_Date + ")";
                            ucCompleted.propDescription = clsHistory.clsHistoryScheme.getMessage(History.propHistoryID);
                            this.divHistoryHolder.Controls.Add(ucCompleted);
                            break;
                    }
                }
        }
    }
}
