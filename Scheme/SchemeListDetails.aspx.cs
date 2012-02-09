using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NAV.Scheme
{
    public partial class SchemeListDetails : System.Web.UI.Page
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
            String strSchemeID = Session[clsSystem_Session.strSession.tempschemeid.ToString()].ToString();
            String strClientID = Session[clsSystem_Session.strSession.clientID.ToString()].ToString();

            clsScheme Scheme = new clsScheme(strClientID, strSchemeID);
            clsSwitchScheme SwitchScheme = new clsSwitchScheme(Scheme);

            switch (SwitchScheme.propStatus)
            {
                case (short)clsSwitch.enumSwitchStatus.Amended:
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
                case (short)clsSwitch.enumSwitchStatus.Locked:
                    this.ucProposalDisplay1.Visible = true;
                    this.ucProposalDisplay1.propSwitchID = SwitchScheme.propSwitchID;
                    this.ucProposalDisplay1.propUserLevel = global::NAV.Scheme.UserControl.ucProposalDisplay.enumUserLevel.Client;
                    break;
                default:
                    break;
            }

            if (SwitchScheme.propStatus != (int)clsSwitch.enumSwitchStatus.Locked)
            {                
                btnResetSecurityCode.Visible = false;
            }
        }

        protected void btnSMSResetSend_Click(object sender, EventArgs e)
        {

            String strSchemeID = Session[clsSystem_Session.strSession.tempschemeid.ToString()].ToString();
            String strClientID = Session[clsSystem_Session.strSession.clientID.ToString()].ToString();

            clsScheme Scheme = new clsScheme(strClientID, strSchemeID);
            clsSwitchScheme SwitchScheme = new clsSwitchScheme(Scheme);

            int intSwitchID = SwitchScheme.propSwitchID;
            string strSchemeName = SwitchScheme.propScheme.propCompany.propCompany;
            string strPopupMessage = "Switch is now unlocked";
            string strSMSMobileNo = this.txtMobileNoResetCode.Text.Trim();

            doSwitch(intSwitchID, strSchemeName, clsSMS.subclsSMSTemplate.enumSMSTemplateID.Reset, strPopupMessage, strSMSMobileNo);
        }

        private void doSwitch(int intSwitchID, string strSchemeName, clsSMS.subclsSMSTemplate.enumSMSTemplateID intSmsID, string strPopupMessage, string strSMSMobileNo)
        {
            String strUserID = Session[clsSystem_Session.strSession.User.ToString()].ToString();

            clsSMS.subclsSMSTemplate osubclsSMSTemplate = new clsSMS.subclsSMSTemplate(intSmsID);
            string strReplacerVariable = clsSMS.subclsSMSTemplate.strPortfolioNameVariable;
            string strMessage = osubclsSMSTemplate.propMessage.Replace(strReplacerVariable, strSchemeName);

            if (strSMSMobileNo.Trim().Length != 0)
            {
                sendSMS(intSwitchID, strUserID, strMessage, strPopupMessage, strSMSMobileNo);
                //ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + strMessage + "');", true);
            }
        }

        private void UpdateStatus(int intSwitchID)
        {
            clsSwitchScheme.updateSwitchHeader(intSwitchID, clsSwitch.enumSwitchStatus.Proposed, string.Empty);            
        }

        private void sendSMS(int intSwitchID, string strUserID, string strSMSMessage, string strPopupMessage, string strSMSMobileNo)
        {
            clsSMS SMS = new clsSMS(strUserID);
            SMS.sendMessage(strSMSMobileNo.Trim(), strSMSMessage);

            if (!SMS.propErrorMsg.Equals(""))
            {
                ClientScript.RegisterStartupScript(this.GetType(), "alertMsgFailed", "alert('Sending Failed  " + SMS.propErrorMsg.Replace("'", " ") + "');", true);
            }

            if (!SMS.propReturnID.Equals(""))
            {
                UpdateStatus(intSwitchID);
                ClientScript.RegisterStartupScript(this.GetType(), "alertMsgSent", "alert('" + strPopupMessage + "');window.location='SchemeListDetails.aspx';", true);
            }
        }

    }
}
