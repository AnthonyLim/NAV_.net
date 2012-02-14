using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NAV.Scheme.UserControl
{
    public partial class ucSwitchDetailsClient : System.Web.UI.UserControl
    {
        #region "properties and enums"

        private String strBackPageURL;
        public String propBackPageURL { get { return strBackPageURL; } set { strBackPageURL = value; } }

        private int intSwitchID = 0;
        public int propSwitchID { get { return intSwitchID; } set { intSwitchID = value; } }

        private enum enumPageState
        {
            AmmendingSwitch,
            hideAll,
            view
        }

        #endregion

        #region "events"

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {

                String strSchemeID = Session[clsSystem_Session.strSession.tempschemeid.ToString()].ToString();
                String strUserID = Session[clsSystem_Session.strSession.User.ToString()].ToString();
                String strClientID = Session[clsSystem_Session.strSession.clientID.ToString()].ToString();

                clsScheme Scheme = new clsScheme(strClientID, strSchemeID);

                clsSwitchScheme SwitchScheme = new clsSwitchScheme(Scheme);

                Session["IFAPermit"] = Scheme.propSwitchIFAPermit;

                enableButtons((clsSwitch.enumSwitchStatus)SwitchScheme.propStatus);
                showhideButtons(enumPageState.view);

                if (SwitchScheme.propStatus == (int)clsSwitch.enumSwitchStatus.Amended)
                {
                    showhideAmendArea(false);
                }
            }
            else
            {
                if (Request["__EVENTARGUMENT"] == "SaveSwitchDetails") { btnSave_Click(); }
                else if (Request["__EVENTARGUMENT"] == "ChangeFund") { replaceFund(int.Parse(this.hfFundIDOrig.Value), int.Parse(this.hfFundIDNew.Value), Boolean.Parse(this.hfIsContribution.Value)); }
                else if (Request["__EVENTARGUMENT"] == "AddMoreFund") { addNewFund(int.Parse(this.hfFundIDNew.Value), Boolean.Parse(this.hfIsContribution.Value)); }
            }

        }

        private void btnSave_Click()
        {
            captureSwitchGridviewDetails();

            List<clsSwitchScheme_Client.clsSwitchSchemeDetails_Client> ListSwitchDetails = (List<clsSwitchScheme_Client.clsSwitchSchemeDetails_Client>)Session["SwitchSchemeDetailsPortfolio_Client"];
            List<clsSwitchScheme_Client.clsSwitchSchemeDetails_Client> ListSwitchDetailsContribution = (List<clsSwitchScheme_Client.clsSwitchSchemeDetails_Client>)Session["SwitchSchemeDetailsContribution_Client"];

            saveclientSwitch(clsSwitch.enumSwitchStatus.Saved, propSwitchID, null);

            Page.ClientScript.RegisterStartupScript(this.GetType(), "alertSaveSuccess", "alert('Amendments saved.'); window.location='" + propBackPageURL + "';", true);

        }

        protected void btnAmendSwitch_Click(object sender, EventArgs e)
        {
            showhideAmendArea(true);
            showhideButtons(enumPageState.AmmendingSwitch);
        }

        protected void btnSwitchNo_Click(object sender, EventArgs e)
        {
            btnSave_Click();
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            clsSwitch_Client.deleteSwitch(this.propSwitchID);

            Page.ClientScript.RegisterStartupScript(this.GetType(), "alertCancelledSwitch", "alert('All amendments havs been discarded'); window.location='" + this.propBackPageURL + "';", true);

        }

        protected void btnAmendSave_Click(object sender, EventArgs e)
        {
            captureSwitchGridviewDetails();

            String strClientID = Session[clsSystem_Session.strSession.clientID.ToString()].ToString();
            String strSchemeID = Session[clsSystem_Session.strSession.tempschemeid.ToString()].ToString();
            String strUserID = Session[clsSystem_Session.strSession.User.ToString()].ToString();            

            saveclientSwitch(clsSwitch.enumSwitchStatus.Saved, propSwitchID, this.txtAmendDesc.Text.Trim());

            bool hasIFAPermit = (bool)Session["IFAPermit"];
            int intSwitchID = propSwitchID;

            if (hasIFAPermit)
            {
                sendValidationCode(this.txtSMSMobileNoApproval.Text.Trim());
            }
            else
            {
                clsIFA IFA = new clsIFA(int.Parse(Session["ifaid"].ToString()));
                clsClient Client = new clsClient(strClientID);
                String strEmailMessage = "";

                try
                {
                    String strRecipient = IFA.propIFAEmail;

                    try
                    {
                        strEmailMessage = clsEmail.generateEmailBody("SchemeClientAmendmentNotification", null, Client.propForename + " " + Client.propSurname, null, null, null, null, null);
                        strEmailMessage = Server.HtmlEncode(strEmailMessage);
                    }
                    catch
                    {
                        Page.ClientScript.RegisterStartupScript(this.GetType(), "AlertNoEmailTemplate", "alert('System error! No email template found for this purpose. Please report this error to your systems administrator');", true);
                        return;
                    }

                    clsSwitchScheme_Client.updateSwitchHeader(clsSwitch.enumSwitchStatus.Proposed, intSwitchID, this.txtAmendDesc.Text.Trim());
                    clsSwitchScheme.updateSwitchHeader(intSwitchID, clsSwitch.enumSwitchStatus.Amended, new clsSwitchScheme(intSwitchID).propDescription);

                    clsEmail.Send(strRecipient, "NAVSwitch@NoReply.com", "NAV-Switch client amended proposed scheme switch", strEmailMessage, propSwitchID, strClientID, clsEmail.enumEmailPurpose.ClientAmendmentsNotification);

                    int intHistoryID = clsHistory.clsHistoryScheme.insertHeader(strSchemeID, intSwitchID, (Int16)clsSwitch.enumSwitchStatus.Amended);
                    clsHistory.clsHistoryScheme.insertDetailsClient(intHistoryID, new clsSwitchScheme_Client(intSwitchID).propSwitchDetailsPortfolio, false);
                    clsHistory.clsHistoryScheme.insertDetailsClient(intHistoryID, new clsSwitchScheme_Client(intSwitchID).propSwitchDetailsContribution, true);
                    clsHistory.clsHistoryScheme.insertMessage(intHistoryID, this.txtAmendDesc.Text.Trim());
                    
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "EmailAmendNotification", "alert('An email was sent to notify your IFA regarding the amendments in the proposed scheme switch.'); window.location='" + propBackPageURL + "';", true);
                }
                catch (Exception ex)
                {
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "EmailFailedAlert", "alert('" + ex + "');", true);                    
                    return;
                }

            }
        }

        protected void SendValidationCode_Click(object sender, EventArgs e)
        {
            sendValidationCode(this.txtSMSMobileNo.Text.Trim());
        }

        protected void btnCheckSecurityCode_Click(object sender, EventArgs e)
        {
            String strUserID = Session[clsSystem_Session.strSession.User.ToString()].ToString();
            String strSchemeID = Session[clsSystem_Session.strSession.tempschemeid.ToString()].ToString();
            String strClientID = Session[clsSystem_Session.strSession.clientID.ToString()].ToString();

            int intSwitchID = propSwitchID;

            List<clsSwitchScheme_Client.clsSwitchSchemeDetails_Client> listSwitchDetailsPortfolio = (List<clsSwitchScheme_Client.clsSwitchSchemeDetails_Client>)Session["SwitchSchemeDetailsPortfolio_Client"];
            List<clsSwitchScheme_Client.clsSwitchSchemeDetails_Client> listSwitchDetailsContribution = (List<clsSwitchScheme_Client.clsSwitchSchemeDetails_Client>)Session["SwitchSchemeDetailsContribution_Client"];

            if (!txtSecurityCode.Text.Equals(string.Empty))
            {                

                if (listSwitchDetailsPortfolio == null) 
                { 
                    clsSwitchScheme_Client SwitchScheme_Client = new clsSwitchScheme_Client(intSwitchID);

                    listSwitchDetailsPortfolio = SwitchScheme_Client.propSwitchDetailsPortfolio;
                    listSwitchDetailsContribution = SwitchScheme_Client.propSwitchDetailsContribution;
                }

                clsSecurityCode oSecurityCode = new clsSecurityCode(this.txtSecurityCode.Text.Trim());
                string strSecurityCodeMessage = oSecurityCode.ValidateSecurityCodeScheme(intSwitchID, strClientID, strSchemeID);

                if (strSecurityCodeMessage.Contains("Sorry, the security code you have entered is incorrect, please re-enter the security code."))
                {
                    this.mpeSecurityCodePanel.Show();
                }
                else if (strSecurityCodeMessage.Contains("Thank you, the proposed changes will be made to your portfolio."))
                {
                    if (listSwitchDetailsPortfolio != null)
                    {
                        clsSwitchScheme.clsSwitchSchemeDetails.transferClientSwitchToIFA(listSwitchDetailsPortfolio, strUserID, false, true);
                        clsSwitchScheme.clsSwitchSchemeDetails.transferClientSwitchToIFA(listSwitchDetailsContribution, strUserID, true, false);
                    }

                    NotifyApprovedSwtich();

                    int intHistoryID = clsHistory.clsHistoryScheme.insertHeader(strSchemeID, intSwitchID, (Int16)clsSwitch.enumSwitchStatus.Approved);
                    clsHistory.clsHistoryScheme.insertDetailsClient(intHistoryID, listSwitchDetailsPortfolio, false);
                    clsHistory.clsHistoryScheme.insertDetailsClient(intHistoryID, listSwitchDetailsContribution, true);
                    clsHistory.clsHistoryScheme.insertMessage(intHistoryID, new clsSwitch_Client(intSwitchID).propDescription);

                    Page.ClientScript.RegisterStartupScript(this.GetType(), "alertCancelledSwitch", "alert('This Switch has been approved!'); window.location='" + this.propBackPageURL + "';", true);

                }
                else if (strSecurityCodeMessage.Contains("Sorry, you have entered the security code incorrectly three times"))
                {
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "alertMessage", string.Format("alert('{0}'); window.location='" + this.propBackPageURL + "'; ", strSecurityCodeMessage), true);
                }

                Page.ClientScript.RegisterStartupScript(this.GetType(), "alertMessage", string.Format("alert('{0}'); ", strSecurityCodeMessage), true);
            }            

            populate(listSwitchDetailsPortfolio, false);
            populate(listSwitchDetailsContribution, true);

        }

        protected void btnContactMeSend_Click(object sender, EventArgs e)
        {

            String strSchemeID = Session[clsSystem_Session.strSession.tempschemeid.ToString()].ToString();
            String strUserID = Session[clsSystem_Session.strSession.User.ToString()].ToString();
            String strClientID = Session[clsSystem_Session.strSession.clientID.ToString()].ToString();

            clsIFA IFA = new clsIFA(int.Parse(Session["ifaid"].ToString()));
            clsClient Client = new clsClient(strClientID);
            clsScheme Scheme = new clsScheme(strClientID, strSchemeID);

            int intSwitchID = propSwitchID;

            String strEmailMessage = "";

            try
            {
                String strRecipient = IFA.propIFAEmail;

                try
                {
                    strEmailMessage = clsEmail.generateEmailBody("ClientRequestContact", IFA.propIFA_Name, Client.propForename + " " + Client.propSurname,
                                                                     Scheme.propCompany.propCompany, this.txtContactMeByTel.Text.Trim(), this.txtContactMeComments.Text.Trim(), null, null);
                    strEmailMessage = Server.HtmlDecode(strEmailMessage);
                }
                catch
                {
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "AlertNoEmailTemplate", "alert('System error! No email template found for this purpose. Please report this error to your systems administrator');", true);
                    return;
                }


                clsEmail.Send(strRecipient, "NAVSwitch@NoReply.com", "NAV-Switch client request", strEmailMessage, propSwitchID, strClientID, clsEmail.enumEmailPurpose.ClientRequestingContactScheme);

                clsSwitchScheme.updateSwitchHeader(propSwitchID, clsSwitch.enumSwitchStatus.Request_ForDiscussion);

                int intHistoryID = clsHistory.clsHistoryScheme.insertHeader(strSchemeID, intSwitchID, (Int16)clsSwitch.enumSwitchStatus.Request_ForDiscussion);
                clsHistory.clsHistoryScheme.insertMessage(intHistoryID, strEmailMessage);

                Page.ClientScript.RegisterStartupScript(this.GetType(), "EmailRequestContactSent", "alert('Request for contact to your IFA has been sent successfully'); window.location='" + this.propBackPageURL + "';", true);

            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "EmailFailedAlert", "alert('Sending failed. Error:  " + ex.Message.Replace("'", " ") + "');", true);
                return;
            }

        }

        protected void btnDeclineSwitch_Click(object sender, EventArgs e)
        {
            String strSchemeID = Session[clsSystem_Session.strSession.tempschemeid.ToString()].ToString();
            String strUserID = Session[clsSystem_Session.strSession.User.ToString()].ToString();
            String strClientID = Session[clsSystem_Session.strSession.clientID.ToString()].ToString();

            clsScheme Scheme = new clsScheme(strClientID, strSchemeID);

            int intSwitchID = propSwitchID;            

            try
            {
                clsIFA IFA = new clsIFA(int.Parse(Session["ifaid"].ToString()));
                clsClient Client = new clsClient(strClientID);
                String strEmailMessage = "";

                String strRecipient = IFA.propIFAEmail;

                try
                {
                    strEmailMessage = clsEmail.generateEmailBody("ClientDeclineNotification", IFA.propIFA_Name, Client.propForename + " " + Client.propSurname, Scheme.propCompany.propCompany, null, this.txtDeclineDescription.Text.Trim(), null, null);
                }
                catch
                {
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "AlertNoEmailTemplate", "alert('System error! No email template found for declining email. Please report this error to your systems administrator');", true);
                    return;
                }

                clsEmail.Send(strRecipient, "NAVSwitch@NoReply.com", "NAV-Switch client declined proposed switch", strEmailMessage, intSwitchID, strClientID, clsEmail.enumEmailPurpose.ClientDeclinedSwitchScheme);

                clsSwitchScheme.updateSwitchHeader(intSwitchID, clsSwitch.enumSwitchStatus.Declined_Client, new clsSwitch(intSwitchID).propDescription);

                int intHistoryID = clsHistory.clsHistoryScheme.insertHeader(strSchemeID, intSwitchID, (Int16)clsSwitch.enumSwitchStatus.Declined_Client);
                clsHistory.clsHistoryScheme.insertMessage(intHistoryID, strEmailMessage);

                clsSwitchScheme_Client.declineSwitch(intSwitchID);
                
                Page.ClientScript.RegisterStartupScript(this.GetType(), "alertDeclinedSwitch", "alert('This switch has been declined.'); window.location='" + this.propBackPageURL + "';", true);

            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "alertDeclinedSwitcherror", "alert('Error declining switch! " + ex.ToString().Replace("'", " ") + "'); window.location='" + this.propBackPageURL + "';", true);                
            }
        }

        #endregion

        #region "Session SwitchDetails"

        private void populate(List<clsSwitchScheme_Client.clsSwitchSchemeDetails_Client> listSwitchDetails, Boolean isContribution)
        {
            GridView gridview = isContribution ? this.gvContributionSwitch : gridview = this.gvSchemeSwitch;

            gridview.DataSource = listSwitchDetails;
            gridview.DataBind();

            if (listSwitchDetails != null && listSwitchDetails.Count != 0)
            {
                Label gvSwitchFooterLblTotalValue = (Label)gridview.FooterRow.Cells[3].FindControl("gvSwitchFooterLblTotalValue");
                gvSwitchFooterLblTotalValue.Text = listSwitchDetails[0].propTotalValue.ToString("n0");

                Label gvSwitchFooterLblTotalAllocationOrig = (Label)gridview.FooterRow.Cells[4].FindControl("gvSwitchFooterLblTotalAllocationOrig");
                gvSwitchFooterLblTotalAllocationOrig.Text = listSwitchDetails[listSwitchDetails.Count - 1].propTotalAllocation.ToString("n2");

                Label gvSwitchFooterLblTotalAllocation = (Label)gridview.FooterRow.Cells[4].FindControl("gvSwitchFooterLblTotalAllocation");
                gvSwitchFooterLblTotalAllocation.Text = listSwitchDetails[listSwitchDetails.Count - 1].propTotalAllocation.ToString("n2");

                gvSwitchFooterHfTotalAllocation.Value = listSwitchDetails[listSwitchDetails.Count - 1].propTotalAllocation.ToString("n2");

                if (float.Parse(gvSwitchFooterLblTotalAllocationOrig.Text) != 100)
                {
                    gvSwitchFooterLblTotalAllocation.Style.Add("color", "#FF0000");
                    gvSwitchFooterLblTotalAllocationOrig.Style.Add("color", "#FF0000");
                }
                else
                {
                    gvSwitchFooterLblTotalAllocation.Style.Add("color", "#000000");
                    gvSwitchFooterLblTotalAllocationOrig.Style.Add("color", "#000000");

                }

                if (!isContribution)
                {
                    this.hfCurrentValueClientTotal.Value = listSwitchDetails[0].propTotalValue.ToString("n0");
                    this.hfFundCount.Value = listSwitchDetails.Count.ToString();
                    this.lblSwitchStatusValue.Text = listSwitchDetails[0].propSwitchScheme.propStatusString;

                    if (float.Parse(gvSwitchFooterHfTotalAllocation.Value) != 100)
                    {                        
                        this.btnSave.Disabled = true;
                        this.btnAmend.Disabled = true;
                    }
                }
                else
                {
                    if (float.Parse(gvSwitchFooterHfTotalAllocation.Value) > 100)
                    {
                        this.btnSave.Disabled = true;
                        this.btnAmend.Disabled = true;
                    }

                    this.hfCurrentValueClientTotal_Contribution.Value = listSwitchDetails[0].propTotalValue.ToString("n0");
                    this.hfFundCount_Contribution.Value = listSwitchDetails.Count.ToString();

                }
                
            }
        }

        public void captureSwitchGridviewDetails()
        {                            
            String strSchemeID = Session[clsSystem_Session.strSession.tempschemeid.ToString()].ToString();
            String strClientID = Session[clsSystem_Session.strSession.clientID.ToString()].ToString();

            List<clsSwitchScheme_Client.clsSwitchSchemeDetails_Client> ListSwitchDetails = (List<clsSwitchScheme_Client.clsSwitchSchemeDetails_Client>)Session["SwitchSchemeDetailsPortfolio_Client"];
            float dblTotalAllocation = 0;

            foreach (GridViewRow row in this.gvSchemeSwitch.Rows)
            {
                HiddenField hfSwitchDetailsID = (HiddenField)row.FindControl("hfSwitchDetailsID");
                HiddenField hfSelectedFundID = (HiddenField)row.FindControl("hfSelectedFundID");
                TextBox txtAllocation = (TextBox)row.FindControl("gvSwitchTxtTargetAllocation");
                HiddenField gvhfSwitchHeaderValue = (HiddenField)row.FindControl("gvhfSwitchHeaderValue");
                HiddenField gvhfSwitchDetailsLblUnits = (HiddenField)row.FindControl("gvhfSwitchDetailsLblUnits");

                foreach (clsSwitchScheme_Client.clsSwitchSchemeDetails_Client SwitchDetails in ListSwitchDetails)
                {
                    if (hfSelectedFundID.Value.Trim() == SwitchDetails.propFund.propFundID.ToString())
                    {
                        SwitchDetails.propSwitchScheme = new clsSwitchScheme(propSwitchID);
                        SwitchDetails.propAllocation = txtAllocation.Text.ToString() == "" ? 0f : float.Parse(txtAllocation.Text.ToString());
                        SwitchDetails.propValue = float.Parse(gvhfSwitchHeaderValue.Value.ToString());
                        SwitchDetails.propUnits = decimal.Parse(gvhfSwitchDetailsLblUnits.Value.ToString());

                        dblTotalAllocation = dblTotalAllocation + SwitchDetails.propAllocation;
                        SwitchDetails.propTotalAllocation = dblTotalAllocation;
                    }

                    Session["SwitchSchemeDetailsPortfolio_Client"] = ListSwitchDetails;
                }
            }

            List<clsSwitchScheme_Client.clsSwitchSchemeDetails_Client> ListSwitchDetailsContribution = (List<clsSwitchScheme_Client.clsSwitchSchemeDetails_Client>)Session["SwitchSchemeDetailsContribution_Client"];
            dblTotalAllocation = 0;

            foreach (GridViewRow row in this.gvContributionSwitch.Rows)
            {
                HiddenField hfSwitchDetailsID = (HiddenField)row.FindControl("hfSwitchDetailsID");
                HiddenField hfSelectedFundID = (HiddenField)row.FindControl("hfSelectedFundID");
                TextBox txtAllocation = (TextBox)row.FindControl("gvSwitchTxtTargetAllocation");
                HiddenField gvhfSwitchHeaderValue = (HiddenField)row.FindControl("gvhfSwitchHeaderValue");
                HiddenField gvhfSwitchDetailsLblUnits = (HiddenField)row.FindControl("gvhfSwitchDetailsLblUnits");

                foreach (clsSwitchScheme_Client.clsSwitchSchemeDetails_Client SwitchDetails in ListSwitchDetailsContribution)
                {
                    if (hfSelectedFundID.Value.Trim() == SwitchDetails.propFund.propFundID.ToString())
                    {
                        SwitchDetails.propAllocation = txtAllocation.Text.ToString() == "" ? 0f : float.Parse(txtAllocation.Text.ToString());
                        SwitchDetails.propValue = float.Parse(gvhfSwitchHeaderValue.Value.ToString());
                        SwitchDetails.propUnits = decimal.Parse(gvhfSwitchDetailsLblUnits.Value.ToString());

                        dblTotalAllocation = dblTotalAllocation + SwitchDetails.propAllocation;
                        SwitchDetails.propTotalAllocation = dblTotalAllocation;
                    }

                    Session["SwitchSchemeDetailsContribution_Client"] = ListSwitchDetailsContribution;
                }
            }
        }

        private void saveclientSwitch(clsSwitch.enumSwitchStatus SwitchStatus, int intSwitchID, string strDesc)
        {
            String strUserID = Session[clsSystem_Session.strSession.User.ToString()].ToString();

            clsSwitchScheme_Client.clsSwitchSchemeDetails_Client.deleteAllDetails(intSwitchID);

            List<clsSwitchScheme_Client.clsSwitchSchemeDetails_Client> ListSwitchDetails = (List<clsSwitchScheme_Client.clsSwitchSchemeDetails_Client>)Session["SwitchSchemeDetailsPortfolio_Client"];
            List<clsSwitchScheme_Client.clsSwitchSchemeDetails_Client> ListSwitchDetailsContribution = (List<clsSwitchScheme_Client.clsSwitchSchemeDetails_Client>)Session["SwitchSchemeDetailsContribution_Client"];

            clsSwitchScheme_Client.updateSwitchHeader(SwitchStatus, intSwitchID, strDesc);
            clsSwitchScheme_Client.clsSwitchSchemeDetails_Client.insertSwitchDetails(ListSwitchDetails, strUserID, propSwitchID, false);
            clsSwitchScheme_Client.clsSwitchSchemeDetails_Client.insertSwitchDetails(ListSwitchDetailsContribution, strUserID, propSwitchID, true);

        }

        private void addNewFund(int intNewFundID, Boolean isContribution)
        {

            captureSwitchGridviewDetails();

            string strSessionDetailsSet = isContribution ? "SwitchSchemeDetailsContribution_Client" : "SwitchSchemeDetailsPortfolio_Client";

            List<clsSwitchScheme_Client.clsSwitchSchemeDetails_Client> currentListSwitchDetails = (List<clsSwitchScheme_Client.clsSwitchSchemeDetails_Client>)Session[strSessionDetailsSet];
            String strUserID = Session[clsSystem_Session.strSession.User.ToString()].ToString();
            clsClient Client = new clsClient(Session[clsSystem_Session.strSession.clientID.ToString()].ToString());

            List<clsSwitchScheme_Client.clsSwitchSchemeDetails_Client> newListSwitchDetails = new List<clsSwitchScheme_Client.clsSwitchSchemeDetails_Client>();

            try
            {
                newListSwitchDetails = clsSwitchScheme_Client.clsSwitchSchemeDetails_Client.addFund(intNewFundID, currentListSwitchDetails, Client.propClientID, Client.propCurrency, strUserID, false);

                Session[strSessionDetailsSet] = newListSwitchDetails;

                populate((List<clsSwitchScheme_Client.clsSwitchSchemeDetails_Client>)Session[strSessionDetailsSet], isContribution);

            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "errDuplicateDund", "alert('" + ex.Message + "');", true);
                populate((List<clsSwitchScheme_Client.clsSwitchSchemeDetails_Client>)Session[strSessionDetailsSet], isContribution);
                return;
            }
        }

        private void replaceFund(int intOldFundID, int intNewFundID, Boolean isContribution)
        {

            captureSwitchGridviewDetails();

            string strSessionDetailsSet = isContribution ? "SwitchSchemeDetailsContribution_Client" : "SwitchSchemeDetailsPortfolio_Client";

            List<clsSwitchScheme_Client.clsSwitchSchemeDetails_Client> PreviousListSwitchDetails = (List<clsSwitchScheme_Client.clsSwitchSchemeDetails_Client>)Session[strSessionDetailsSet];

            String strUserID = Session[clsSystem_Session.strSession.User.ToString()].ToString();
            String strClientID = Session[clsSystem_Session.strSession.clientID.ToString()].ToString();
            String strSchemeID = Session[clsSystem_Session.strSession.tempschemeid.ToString()].ToString();

            clsScheme Scheme = new clsScheme(strClientID, strSchemeID);

            List<clsSwitchScheme_Client.clsSwitchSchemeDetails_Client> newListSwitchDetails = new List<clsSwitchScheme_Client.clsSwitchSchemeDetails_Client>();

            try
            {
                newListSwitchDetails = clsSwitchScheme_Client.clsSwitchSchemeDetails_Client.FundChange(intOldFundID, intNewFundID, PreviousListSwitchDetails, strClientID, Scheme.propClient.propCurrency, false);

                Session[strSessionDetailsSet] = newListSwitchDetails;

                populate((List<clsSwitchScheme_Client.clsSwitchSchemeDetails_Client>)Session[strSessionDetailsSet], isContribution);
            }
            catch (Exception ex)
            {
                populate((List<clsSwitchScheme_Client.clsSwitchSchemeDetails_Client>)Session[strSessionDetailsSet], isContribution);
                Page.ClientScript.RegisterStartupScript(this.GetType(), "errDuplicateFund", "alert('" + ex.Message + "');", true);
                return;
            }
        }

        protected void lbtnRemoveFund_Click(object sender, CommandEventArgs e)
        {
            captureSwitchGridviewDetails();

            Boolean isContribution = Boolean.Parse(this.hfIsContribution.Value);
            string strSessionDetailsSet = isContribution ? "SwitchSchemeDetailsContribution_Client" : "SwitchSchemeDetailsPortfolio_Client";

            List<clsSwitchScheme_Client.clsSwitchSchemeDetails_Client> PreviousListSwitchDetails = (List<clsSwitchScheme_Client.clsSwitchSchemeDetails_Client>)Session[strSessionDetailsSet];

            Session[strSessionDetailsSet] = clsSwitchScheme_Client.clsSwitchSchemeDetails_Client.removeSwitchDetails(int.Parse(e.CommandArgument.ToString()), PreviousListSwitchDetails);
            populate((List<clsSwitchScheme_Client.clsSwitchSchemeDetails_Client>)Session[strSessionDetailsSet], isContribution);

        }

        protected void btnMirrorFundAllocation_Click(object sender, EventArgs e)
        {
            captureSwitchGridviewDetails();

            List<clsSwitchScheme_Client.clsSwitchSchemeDetails_Client> listSwitchDetails = (List<clsSwitchScheme_Client.clsSwitchSchemeDetails_Client>)Session["SwitchSchemeDetailsPortfolio_Client"];
            List<clsSwitchScheme_Client.clsSwitchSchemeDetails_Client> newListSwitchDetails = new List<clsSwitchScheme_Client.clsSwitchSchemeDetails_Client>();

            foreach (clsSwitchScheme_Client.clsSwitchSchemeDetails_Client SwitchDetails in listSwitchDetails)
            {
                clsSwitchScheme_Client.clsSwitchSchemeDetails_Client newSwitchDetailsContribution = new clsSwitchScheme_Client.clsSwitchSchemeDetails_Client();

                newSwitchDetailsContribution.propAllocation = SwitchDetails.propAllocation;
                newSwitchDetailsContribution.propCreated_By = Session[clsSystem_Session.strSession.User.ToString()].ToString();
                newSwitchDetailsContribution.propCurrencyMultiplier = SwitchDetails.propCurrencyMultiplier;
                newSwitchDetailsContribution.propDate_Created = DateTime.Today;
                newSwitchDetailsContribution.propDate_LastUpdate = DateTime.Today;
                newSwitchDetailsContribution.propFund = SwitchDetails.propFund;
                newSwitchDetailsContribution.propIsContribution = true;
                newSwitchDetailsContribution.propIsDeletable = SwitchDetails.propIsDeletable;
                newSwitchDetailsContribution.propSwitchDetailsID = SwitchDetails.propSwitchDetailsID;
                newSwitchDetailsContribution.propSwitchScheme = SwitchDetails.propSwitchScheme;
                newSwitchDetailsContribution.propTotalAllocation = SwitchDetails.propTotalAllocation;
                newSwitchDetailsContribution.propTotalValue = SwitchDetails.propTotalValue;
                newSwitchDetailsContribution.propUnits = SwitchDetails.propUnits;
                newSwitchDetailsContribution.propUpdated_By = SwitchDetails.propUpdated_By;
                newSwitchDetailsContribution.propValue = SwitchDetails.propValue;

                newListSwitchDetails.Add(newSwitchDetailsContribution);
            }

            populate(listSwitchDetails, false);
            populate(newListSwitchDetails, true);

            Session["SwitchSchemeDetailsContribution_Client"] = newListSwitchDetails;

        }

        #endregion

        #region "visibility"

        public string HideClickableFundLinks(object isDeletetable)
        {
            if (!(bool)isDeletetable) { return "display:none"; }
            else { return ""; }
        }

        public string ShowClickableFundLinks(object isDeletetable)
        {
            if ((bool)isDeletetable) { return "display:none"; }
            else { return ""; }
        }

        private void enableButtons(clsSwitch.enumSwitchStatus SwitchStatus)
        {
            switch (SwitchStatus)
            {
                case clsSwitch.enumSwitchStatus.Proposed:
                    this.btnApproveSwitch.Disabled = false;
                    this.btnAmendSwitch.Enabled = true;
                    this.btnDeclineSwitch.Enabled = true;
                    this.btnContactMe.Enabled = true;
                    break;

                case clsSwitch.enumSwitchStatus.Request_ForDiscussion:
                    this.btnApproveSwitch.Disabled = true;
                    this.btnAmendSwitch.Enabled = false;
                    this.btnDeclineSwitch.Enabled = false;
                    this.btnContactMe.Enabled = false;
                    break;
                default:
                    this.btnApproveSwitch.Disabled = true;
                    this.btnAmendSwitch.Enabled = false;
                    this.btnDeclineSwitch.Enabled = false;
                    this.btnContactMe.Enabled = false;
                    break;
            }
        }

        private void showhideButtons(enumPageState pageState)
        {
            switch (pageState)
            {
                case enumPageState.AmmendingSwitch:
                    this.btnApproveSwitch.Visible = false;
                    this.btnAmendSwitch.Visible = false;
                    this.btnDeclineSwitch.Visible = false;
                    this.btnContactMe.Visible = false;
                    this.divAmendButtonSet.Visible = true;
                    break;
                case enumPageState.hideAll:
                    this.btnApproveSwitch.Visible = false;
                    this.btnAmendSwitch.Visible = false;
                    this.btnDeclineSwitch.Visible = false;
                    this.btnContactMe.Visible = false;
                    this.divAmendButtonSet.Visible = false;
                    break;
                case enumPageState.view:
                    this.btnApproveSwitch.Visible = true;
                    this.btnAmendSwitch.Visible = true;
                    this.btnDeclineSwitch.Visible = true;
                    this.btnContactMe.Visible = true;
                    this.divAmendButtonSet.Visible = false;
                    break;
                default:
                    break;
            }
        }

        private void showhideAmendArea(bool isShow)
        {

            if (isShow)
            {
                String strSchemeID = Session[clsSystem_Session.strSession.tempschemeid.ToString()].ToString();
                String strUserID = Session[clsSystem_Session.strSession.User.ToString()].ToString();
                String strClientID = Session[clsSystem_Session.strSession.clientID.ToString()].ToString();         

                List<clsSwitchScheme_Client.clsSwitchSchemeDetails_Client> listSwitchDetailsPortfolio = clsSwitchScheme_Client.clsSwitchSchemeDetails_Client.getSwitchDetails(propSwitchID, false);
                List<clsSwitchScheme_Client.clsSwitchSchemeDetails_Client> listSwitchDetailsContribution = clsSwitchScheme_Client.clsSwitchSchemeDetails_Client.getSwitchDetails(propSwitchID, true);

                Session["SwitchSchemeDetailsPortfolio_Client"] = listSwitchDetailsPortfolio;
                Session["SwitchSchemeDetailsContribution_Client"] = listSwitchDetailsContribution;

                this.populate(listSwitchDetailsPortfolio, false);
                this.populate(listSwitchDetailsContribution, true);

                this.lblTitle.Visible = true;                
                this.lblSwitchStatusTitle.Visible = true;
                this.lblSwitchStatusValue.Visible = true;
                this.lblTitle_ContributionAllocation.Visible = true;
                this.btnMirrorFundAllocation.Visible = true;
            }
            else
            {
                this.populate(null, false);

                this.lblTitle.Visible = false;
                this.lblSwitchStatusTitle.Visible = false;
                this.lblSwitchStatusValue.Visible = false;
                this.lblTitle_ContributionAllocation.Visible = false;
                this.btnMirrorFundAllocation.Visible = false;
            }

        }

        #endregion

        #region "misc"

        public void FixAjaxValidationSummaryErrorLabel_PreRender(object sender, EventArgs e)
        {

            string objId = ((Label)sender).ID;

            ScriptManager.RegisterStartupScript(this, this.GetType(), objId, ";", true);

        }

        #endregion

        #region "funds"

        private void populateFundGrid(String strFundName)
        {
            this.gvFunds.DataSource = clsFund.getAllFunds(strFundName);
            this.gvFunds.DataBind();
            this.gvFunds.Visible = true;
        }

        protected void btnPopupFund_Click(object sender, EventArgs e)
        {
            if (!this.txtPopupFund.Text.Trim().Equals(""))
            {
                populateFundGrid(this.txtPopupFund.Text.Trim());
                this.mpeFundSearch.Show();
            }
        }

        protected void btnCloseFundSearch_Click(object sender, EventArgs e)
        {
            clearPopupFundFields();
            this.mpeFundSearch.Hide();
            captureSwitchGridviewDetails();

            List<clsSwitchScheme_Client.clsSwitchSchemeDetails_Client> listSwitchDetailsPortfolio = (List<clsSwitchScheme_Client.clsSwitchSchemeDetails_Client>) Session["SwitchSchemeDetailsPortfolio_Client"];
            List<clsSwitchScheme_Client.clsSwitchSchemeDetails_Client> listSwitchDetailsContribution = (List<clsSwitchScheme_Client.clsSwitchSchemeDetails_Client>)Session["SwitchSchemeDetailsContribution_Client"];

            populate(listSwitchDetailsPortfolio, false);
            populate(listSwitchDetailsContribution, true);
        }

        protected void txtPopupFund_TextChanged(object sender, EventArgs e)
        {
            btnPopupFund_Click(sender, e);
        }

        protected void gvFunds_lbtnFundName_Click(object sender, EventArgs e)
        {
            this.hfFundIDNew.Value = ((LinkButton)sender).CommandArgument.ToString();

            switch (this.hfPopUpFundAction.Value)
            {
                case "add":
                    clearPopupFundFields();
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "doPostChangeFund", "__doPostBack('hfSelectedFundID', 'AddMoreFund');", true);
                    break;
                case "edit":
                    clearPopupFundFields();
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "doPostChangeFund", "__doPostBack('hfSelectedFundID', 'ChangeFund');", true);
                    break;
                default:
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('error on funds popUps  : '" + this.hfPopUpFundAction.Value + ");", true);
                    break;
            }

        }

        protected void clearPopupFundFields()
        {
            this.gvFunds.DataSource = null;
            this.gvFunds.DataBind();
            this.gvFunds.Visible = false;
            this.txtPopupFund.Text = "";

        }

        #endregion

        #region "SMS"

        private void sendValidationCode(string strSMSMobileNo)
        {
            String strSchemeID = Session[clsSystem_Session.strSession.tempschemeid.ToString()].ToString();
            String strUserID = Session[clsSystem_Session.strSession.User.ToString()].ToString();
            String strClientID = Session[clsSystem_Session.strSession.clientID.ToString()].ToString();

            int intSwitchID = propSwitchID;
            clsScheme Scheme = new clsScheme(strClientID, strSchemeID);

            string strSecurityCode = GenerateSecurityCode(intSwitchID, strClientID, strSchemeID);

            string strMessage = clsSMS.subclsSMSTemplate.getSMSTemplate(clsSMS.subclsSMSTemplate.enumSMSTemplateID.SecurityCodeScheme.ToString());
            strMessage = clsSMS.subclsSMSTemplate.convertSMSMessage(strMessage, null, null, null, strSecurityCode, Scheme);

            if (!strSMSMobileNo.Trim().Equals(string.Empty))
            {
                sendSMS(strUserID, strMessage, strSMSMobileNo);
            }
            Page.ClientScript.RegisterStartupScript(this.GetType(), "showSecurityCode", string.Format("showSecurityCodePanel();"), true);
        }

        private void sendSMS(string strUserID, string strSMSMessage, string strSMSMobileNo)
        {            
            clsSMS SMS = new clsSMS(strUserID);
            try
            {
                SMS.sendMessage(strSMSMobileNo.Trim(), strSMSMessage);
            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "alrterror", "alert('error!! " + ex.Message.ToString().Replace("'", " ") + "');", true);
            }


            if (!SMS.propErrorMsg.Equals(""))
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "alertMsgFailed", "alert('Sending Failed  " + SMS.propErrorMsg.Replace("'", " ") + "');", true);
                this.mdlPopupSms.Show();
            }

            if (!SMS.propReturnID.Equals(""))
            {
                string strMessageOuput = "You will recieve a text message to your registered mobile number. Enter the code that it contains in the Approval Code field that will appear below the portfolio that you have approved.";
                Page.ClientScript.RegisterStartupScript(this.GetType(), "alertMsgSent", string.Format("alert('{0}');", strMessageOuput), true);
                this.mpeSecurityCodePanel.Show();
            }
        }

        private string GenerateSecurityCode(int intSwitchID, string strClientID, string strSchemeID)
        {
            string strSecurityCode = string.Empty;
            int intSecurityCode = 0;
            while (intSecurityCode == 0)
            {
                clsSecurityCode oSecurityCode = new clsSecurityCode(4);
                intSecurityCode = oSecurityCode.insertEncryptedCode_Scheme(intSwitchID, strClientID, strSchemeID);
                strSecurityCode = oSecurityCode.propSecurityCode;
            }
            return strSecurityCode;
        }

        #endregion

        #region "Client-side Handlers"

        public ucSwitchDetailsClient()
            : base()
        {
            this.Init += this.Initialize;
        }

        public void Initialize(object sender, EventArgs e)
        {
            /* Added on January 19, 2012 */
            this.AddClientHandlers();
            /* End of addition January 19, 2012 */
        }

        protected void AddClientHandlers()
        {
            string jsFunction = "getFlickerSolved";
            string jsConditionFortxtPopup = string.Format("PopupValidation('{0}','{1}');", txtPopupFund.ClientID, PopupFundSearch.ClientID);

            this.txtPopupFund.Attributes["autocomplete"] = "off";
            this.PopupFundSearch.Attributes["oncancelscript"] += jsFunction + "('" + PopupFundSearch.ClientID + "');";
            this.btnPopupFund.OnClientClick += jsConditionFortxtPopup;
            this.btnCloseFundSearch.OnClientClick += jsFunction + "('" + PopupFundSearch.ClientID + "');";
            this.txtPopupFund.Attributes.Remove("onchange");
            this.txtPopupFund.Attributes.Remove("onkeypress");

            btnSwitchProceed.Attributes["onclick"] += jsFunction + "('" + FirstSwitchPopup.ClientID + "');";
            btnSwitchCancel.Attributes["onclick"] += jsFunction + "('" + FirstSwitchPopup.ClientID + "');";
            FirstSwitchPopup.Attributes["onCancelScript"] += jsFunction + "('" + FirstSwitchPopup.ClientID + "');";

            btnAmendYes.Attributes["onclick"] += jsFunction + "('" + SecondSwitchPopup.ClientID + "');";
            btnAmendNo.Attributes["onclick"] += jsFunction + "('" + SecondSwitchPopup.ClientID + "');";
            SecondSwitchPopup.Attributes["onCancelScript"] += jsFunction + "('" + SecondSwitchPopup.ClientID + "');";

            btnAmendSaveSend.Attributes["onclick"] += jsFunction + "('" + ThirdSwitchPopup.ClientID + "');";
            btnAmendCancelSend.Attributes["onclick"] += jsFunction + "('" + ThirdSwitchPopup.ClientID + "');";
            ThirdSwitchPopup.Attributes["OnCancelScript"] += jsFunction + "('" + ThirdSwitchPopup.ClientID + "');";

            btnValidationCodeCancel.OnClientClick += jsFunction + "('" + panelPopupSms.ClientID + "');";
            SendValidationCode.OnClientClick += jsFunction + "('" + panelPopupSms.ClientID + "');";
            panelPopupSms.Attributes["OnCancelScript"] += jsFunction + "('" + panelPopupSms.ClientID + "');";
        }

        #endregion

        protected void NotifyApprovedSwtich()
        {
            //try
            //{
                String strSchemeID = Session[clsSystem_Session.strSession.tempschemeid.ToString()].ToString();
                String strClientID = Session[clsSystem_Session.strSession.clientID.ToString()].ToString();

                clsScheme Scheme = new clsScheme(strClientID, strSchemeID);
                clsSwitchScheme switchScheme = new clsSwitchScheme(Scheme);
                string htmlTemplate = clsOutput.generateApprovedSwitchScheme(Scheme);
                StyleSheet style = clsOutput.getStyleSheet_ApprovedSwitch();
                string strFilename = clsOutput.generateOutputFile(clsOutput.enumOutputType.PDF, htmlTemplate, style, switchScheme.propSwitchID, clsOutput.enumSwitchType.Scheme);

                int intIFAID = int.Parse(Session["ifaid"].ToString());
                string strRecepient = (new clsIFA(intIFAID)).propIFAEmail;
                string strSender = "NAVSwitch@NoReply.com";
                string strSubject = "Switch Instruction";
                string strBody = clsEmail.generateEmailBody((Scheme.propConfirmationRequired ? "NotifyApprovedEmailReqSign" : "NotifyApprovedEmail"), null, null, null, null, null, Scheme.propCompany.propCompany, switchScheme.propSwitchID.ToString());
                clsEmail.SendWithAttachment(strRecepient, strSender, strSubject, strBody, switchScheme.propSwitchID, switchScheme.propClient.propClientID, clsEmail.enumEmailPurpose.ApproveSwitchNotification, strFilename);

                clsClient client = new clsClient(Scheme.propClient.propClientID);
                string ClientName = client.propForename + " " + client.propSurname;
                if (!String.IsNullOrEmpty(client.propEmailWork))
                {
                    strRecepient = client.propEmailWork;
                    strBody = clsEmail.generateEmailBody((Scheme.propConfirmationRequired ? "NotifyClientApprovedEmailReqSign" : "NotifyClientApprovedEmail"), null, null, ClientName, null, null, Scheme.propCompany.propCompany, null);
                    clsEmail.SendWithAttachment(strRecepient, strSender, strSubject, strBody, switchScheme.propSwitchID, Scheme.propClient.propClientID, clsEmail.enumEmailPurpose.ApproveSwitchNotification, strFilename);
                }
                if (!String.IsNullOrEmpty(client.propEmailPersonal))
                {
                    strRecepient = client.propEmailPersonal;
                    strBody = clsEmail.generateEmailBody((Scheme.propConfirmationRequired ? "NotifyClientApprovedEmailReqSign" : "NotifyClientApprovedEmail"), null, null, ClientName, null, null, Scheme.propCompany.propCompany, null);
                    clsEmail.SendWithAttachment(strRecepient, strSender, strSubject, strBody, switchScheme.propSwitchID, Scheme.propClient.propClientID, clsEmail.enumEmailPurpose.ApproveSwitchNotification, strFilename);
                }
            //}
            //catch (Exception e)
            //{
            //    Response.Write("<p>" + e.ToString() + "</p>");
            //}
        }

    }
}
