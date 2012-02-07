using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NAV.Scheme
{
    public partial class Scheme : System.Web.UI.Page
    {
        protected void Page_Init(object sender, EventArgs e)
        {
            if (Session[clsSystem_Session.strSession.User.ToString()] == null)
            {
                Response.Redirect("https://" + Request.ServerVariables["SERVER_NAME"] + ":" + Request.ServerVariables["SERVER_PORT"] + "/report/schemedetails.asp");
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            Session["SourcePage"] = "/report/schemedetails.asp";
            this.ucSwitchDetails1.propBackPageURL = "https://" + Request.ServerVariables["SERVER_NAME"] + ":" + Request.ServerVariables["SERVER_PORT"] + "/report/schemedetails.asp";

            String strSchemeID = Session[clsSystem_Session.strSession.tempschemeid.ToString()].ToString();
            String strUserID = Session[clsSystem_Session.strSession.User.ToString()].ToString();
            String strClientID = Session[clsSystem_Session.strSession.clientID.ToString()].ToString();

            clsScheme Scheme = new clsScheme(strClientID, strSchemeID);
            clsSwitchScheme SwitchScheme = new clsSwitchScheme(Scheme);

            switch (SwitchScheme.propStatus)
            {
                case (short) clsSwitch.enumSwitchStatus.Amended:
                    this.ucProposalDisplay1.Visible = true;
                    this.ucProposalDisplay1.propSwitchID = SwitchScheme.propSwitchID;
                    this.ucProposalDisplay1.propUserLevel = global::NAV.Scheme.UserControl.ucProposalDisplay.enumUserLevel.Client;

                    this.ucProposalDisplay2.Visible = true;
                    this.ucProposalDisplay2.propSwitchID = SwitchScheme.propSwitchID;
                    this.ucProposalDisplay2.propUserLevel = global::NAV.Scheme.UserControl.ucProposalDisplay.enumUserLevel.IFA;
                    break;
                case (short)clsSwitch.enumSwitchStatus.Proposed:
                    this.ucProposalDisplay2.Visible = true;
                    this.ucProposalDisplay2.propSwitchID = SwitchScheme.propSwitchID;
                    this.ucProposalDisplay2.propUserLevel = global::NAV.Scheme.UserControl.ucProposalDisplay.enumUserLevel.Client;
                    break;
                case (short)clsSwitch.enumSwitchStatus.Request_ForDiscussion:
                    this.ucProposalDisplay1.Visible = true;
                    this.ucProposalDisplay1.propSwitchID = SwitchScheme.propSwitchID;
                    this.ucProposalDisplay1.propUserLevel = global::NAV.Scheme.UserControl.ucProposalDisplay.enumUserLevel.Client;
                    break;
                case (short)clsSwitch.enumSwitchStatus.Declined_IFA:
                    this.ucProposalDisplay1.Visible = true;
                    this.ucProposalDisplay1.propSwitchID = SwitchScheme.propSwitchID;
                    this.ucProposalDisplay1.propUserLevel = global::NAV.Scheme.UserControl.ucProposalDisplay.enumUserLevel.Client;

                    this.ucProposalDisplay2.Visible = true;
                    this.ucProposalDisplay2.propSwitchID = SwitchScheme.propSwitchID;
                    this.ucProposalDisplay2.propUserLevel = global::NAV.Scheme.UserControl.ucProposalDisplay.enumUserLevel.IFA;
                    break;
                case (short)clsSwitch.enumSwitchStatus.Approved:
                    this.ucProposalDisplay1.Visible = true;
                    this.ucProposalDisplay1.propSwitchID = SwitchScheme.propSwitchID;
                    this.ucProposalDisplay1.propUserLevel = global::NAV.Scheme.UserControl.ucProposalDisplay.enumUserLevel.Client;
                    break;
                case (short)clsSwitch.enumSwitchStatus.Completed:
                    this.ucProposalDisplay1.Visible = true;
                    this.ucProposalDisplay1.propSwitchID = SwitchScheme.propSwitchID;
                    this.ucProposalDisplay1.propUserLevel = global::NAV.Scheme.UserControl.ucProposalDisplay.enumUserLevel.Client;
                    break;
                default:
                    break;
            }
        }

        protected void lbtnHistory_Click(object sender, EventArgs e)
        {
            String strSchemeID = Session[clsSystem_Session.strSession.tempschemeid.ToString()].ToString();
            String strUserID = Session[clsSystem_Session.strSession.User.ToString()].ToString();
            String strClientID = Session[clsSystem_Session.strSession.clientID.ToString()].ToString();

            clsScheme Scheme = new clsScheme(strClientID, strSchemeID);
            clsSwitchScheme SwitchScheme = new clsSwitchScheme(Scheme);

            string strSwitchID = SwitchScheme.propSwitchID.ToString();

            String strHistoryURL = "SchemeHistory.aspx?SID=" + strSwitchID + "&SchID=" + strSchemeID + "&CID=" + strClientID;
            Session["SourcePage"] = "/ASPX/Scheme/Scheme.aspx";            
            Response.Redirect(strHistoryURL);

        }
    }    
}
