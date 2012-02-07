using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using NAV.Portfolio.UserControl;

namespace NAV.Portfolio
{
    public partial class SwitchHistory : System.Web.UI.Page
    {

        protected void Page_Init(object sender, EventArgs e)
        {
            if (Session[clsSystem_Session.strSession.User.ToString()] == null)
            {
                Response.Redirect("https://" + Request.ServerVariables["SERVER_NAME"] + ":" + Request.ServerVariables["SERVER_PORT"] + "/report/" + "portfoliodetails.asp");
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {            
            if (!Page.IsPostBack) {

                if (Request.QueryString["SourcePage"] != null)
                {
                    Session["SourcePage"] = Request.QueryString["SourcePage"];
                }

                int intSwitchID = int.Parse(Request.QueryString["SID"].ToString());
                string strPortfolioID = Request.QueryString["PID"].ToString();
                string strClientID = Request.QueryString["CID"].ToString();

                this.ucCurrentPortfolio.propClientID = strClientID;
                this.ucCurrentPortfolio.propPortfolioID = strPortfolioID;
                this.ucCurrentPortfolio.propUserID = strClientID;

                this.ucHeader1.propClientID = strClientID;
                this.ucHeader1.propPortfolioID = strPortfolioID;
                this.ucHeader1.propUserID = strClientID;

                clsPortfolio Portfolio = new clsPortfolio(strClientID, strPortfolioID);                
  
                foreach (clsHistory History in clsHistory.getListHistory(strPortfolioID, intSwitchID))
                {
                    switch (History.propStatus){
                        case 0: //draft
                            break;
                        case 1: //save
                            break;
                        case 2: //proposed
                            WebUserControl1 ucProposed = (WebUserControl1)LoadControl("UserControl/ucSwitchDetails.ascx");
                            ucProposed.propSwitchDetails = clsHistory.getSwitchDetailsIFA(Portfolio, intSwitchID, History.propHistoryID);
                            ucProposed.propStatus = History.propStatus;
                            ucProposed.propDateAction = History.propAction_Date;
                            ucProposed.propMessage = clsHistory.getMessage(History.propHistoryID);
                            ucProposed.propTitle = "IFA Proposed Switch";
                            this.divHistoryHolder.Controls.Add(ucProposed);
                            break;
                        case 3: //amended
                            WebUserControl1 ucAmended = (WebUserControl1)LoadControl("UserControl/ucSwitchDetails.ascx");
                            ucAmended.propSwitchDetails = clsHistory.getSwitchDetailsIFA(Portfolio, intSwitchID, History.propHistoryID);
                            ucAmended.propStatus = History.propStatus;
                            ucAmended.propDateAction = History.propAction_Date;
                            ucAmended.propMessage = clsHistory.getMessage(History.propHistoryID);
                            ucAmended.propTitle = "Client Proposed Switch";
                            this.divHistoryHolder.Controls.Add(ucAmended);
                            break;
                        case 4: //decline IFA
                            break;
                        case 5: //decline Client
                            WebUserControl1 ucDeclinedClient = (WebUserControl1)LoadControl("UserControl/ucSwitchDetails.ascx");                            
                            ucDeclinedClient.propStatus = History.propStatus;
                            ucDeclinedClient.propDateAction = History.propAction_Date;
                            ucDeclinedClient.propMessage = clsHistory.getMessage(History.propHistoryID);
                            ucDeclinedClient.propTitle = "Switch Declined";
                            this.divHistoryHolder.Controls.Add(ucDeclinedClient);
                            break;
                        case 6: //Approved
                            WebUserControl1 ucApproved = (WebUserControl1)LoadControl("UserControl/ucSwitchDetails.ascx");
                            ucApproved.propStatus = History.propStatus;
                            ucApproved.propDateAction= History.propAction_Date;
                            ucApproved.propTitle = "Switch Approved";
                            this.divHistoryHolder.Controls.Add(ucApproved);
                            break;
                        case 7: //lock
                            break;
                        case 8: //request for discussion
                            WebUserControl1 ucRequest = (WebUserControl1)LoadControl("UserControl/ucSwitchDetails.ascx");
                            ucRequest.propStatus = History.propStatus;
                            ucRequest.propDateAction = History.propAction_Date;
                            ucRequest.propMessage = clsHistory.getMessage(History.propHistoryID);
                            ucRequest.propTitle = "Client Requested Contact";
                            this.divHistoryHolder.Controls.Add(ucRequest);
                            break;
                        case 9: //Cancelled
                            break;
                        case 10: //Completed
                            WebUserControl1 ucCompleted = (WebUserControl1)LoadControl("UserControl/ucSwitchDetails.ascx");
                            ucCompleted.propStatus = History.propStatus;
                            ucCompleted.propDateAction = History.propAction_Date;
                            ucCompleted.propTitle = "Switch Completed";
                            this.divHistoryHolder.Controls.Add(ucCompleted);
                            break;
                    }
                }
            }
        }
    }
}
