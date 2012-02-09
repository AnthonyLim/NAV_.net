using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NAV.Portfolio
{
    public partial class SwitchClient : System.Web.UI.Page
    {

        #region "fields"

        private string strUserID;
        private string strClientID;
        private string strPortfolioID;

        #endregion

        #region "events"

        protected void Page_Init(object sender, EventArgs e)
        {
            if (Session[clsSystem_Session.strSession.User.ToString()] == null)
            {
                Response.Redirect("https://" + Request.ServerVariables["SERVER_NAME"] + ":" + Request.ServerVariables["SERVER_PORT"] + "/report/" + "portfoliodetails.asp");
            }
            
            this.AddClientHandlers();

        }

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!Page.IsPostBack)
            {
                //Session["SourcePage"] = "portfoliodetails.asp"; //local
                Session["SourcePage"] = "/report/portfoliodetails.asp"; //deploy

                strClientID = Session[clsSystem_Session.strSession.clientID.ToString()].ToString();
                strPortfolioID = Session[clsSystem_Session.strSession.tempportfolioid.ToString()].ToString();
                strUserID = Session[clsSystem_Session.strSession.User.ToString()].ToString();

                clsPortfolio objPortfolio = new clsPortfolio(strClientID, strPortfolioID, strUserID);

                Session["IFAPermit"] = objPortfolio.propPortfolioDetails[0].propSwitchIFAPermit;

                enableButtons((clsSwitch.enumSwitchStatus)objPortfolio.propSwitch.propStatus);
                showhideButtons(enumPageState.hideAll);
                    
                populateHeader(objPortfolio);                
                populateOriginalPortfolio(objPortfolio.propPortfolioDetails);                
                populateProposedSwitch(objPortfolio.propSwitch.propSwitchDetails);

                showhideButtons(enumPageState.view);
                ViewState["intSwitchID"] = objPortfolio.propSwitch.propSwitchID;

                clsSwitch_Client objSwitch_Client = new clsSwitch_Client((int)ViewState["intSwitchID"]);
                Session["SwitchDetails_Client"] = objSwitch_Client.propSwitchDetails;

                if (objPortfolio.propSwitch.propStatus == (int)clsSwitch.enumSwitchStatus.Amended)
                {
                    showhideAmendArea(true);
                }
            }
            else 
            {
                if (Request["__EVENTARGUMENT"] == "SaveSwitchDetails") { btnSave_Click(); }
                else if (Request["__EVENTARGUMENT"] == "ChangeFund") { replaceFund(int.Parse(this.hfFundIDOrig.Value), int.Parse(this.hfFundIDNew.Value)); }
                else if (Request["__EVENTARGUMENT"] == "AddMoreFund") { addNewFund(int.Parse(this.hfFundIDNew.Value)); }
            }
        }

        protected void lbtnHistory_Click(object sender, EventArgs e)
        {
            List<clsSwitchDetails_Client> SwitchDetails = (List<clsSwitchDetails_Client>)Session["SwitchDetails_Client"];
            String strSwitchID = ViewState["intSwitchID"].ToString();
            string ClientID = Session[clsSystem_Session.strSession.clientID.ToString()].ToString();
            string PortfolioID = Session[clsSystem_Session.strSession.tempportfolioid.ToString()].ToString();

            String strHistoryURL = "SwitchHistory.aspx?SID=" + strSwitchID + "&PID=" + PortfolioID + "&CID=" + ClientID;

            Session["SourcePage"] = "/ASPX/Portfolio/SwitchClient.aspx"; //Depl..            
            //Session["SourcePage"] = "../ASPX/Portfolio/SwitchClient.aspx"; //Devt..

            Response.Redirect(strHistoryURL);
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
            List<clsSwitchDetails_Client> ListSwitchDetails = (List<clsSwitchDetails_Client>)Session["SwitchDetails_Client"];

            clsSwitch_Client.deleteSwitch(ListSwitchDetails[0].propSwitchID);

            string backPageURL = "https://" + Request.ServerVariables["SERVER_NAME"] + ":" + Request.ServerVariables["SERVER_PORT"] + "/report/" + "portfoliodetails.asp";
            ClientScript.RegisterStartupScript(this.GetType(), "alertCancelledSwitch", "alert('Amendments cancelled'); window.location='" + backPageURL + "';", true);

        }

        protected void lbtnRemoveFund_Click(object sender, CommandEventArgs e)
        {            
            captureSwitchGridviewDetails();

            List<clsSwitchDetails_Client> PreviousListSwitchDetails = (List<clsSwitchDetails_Client>)Session["SwitchDetails_Client"];

            strClientID = Session[clsSystem_Session.strSession.clientID.ToString()].ToString();
            strPortfolioID = Session[clsSystem_Session.strSession.tempportfolioid.ToString()].ToString();
            strUserID = Session[clsSystem_Session.strSession.User.ToString()].ToString();
                       
            Session["SwitchDetails_Client"] = clsSwitchDetails_Client.removeSwitchDetails(int.Parse(e.CommandArgument.ToString()), PreviousListSwitchDetails);
            populateAmendSwitch((List<clsSwitchDetails_Client>)Session["SwitchDetails_Client"]);
        }

        protected void btnAmendSave_Click(object sender, EventArgs e)
        {
            captureSwitchGridviewDetails();

            strUserID = Session[clsSystem_Session.strSession.User.ToString()].ToString();
            strClientID = Session[clsSystem_Session.strSession.clientID.ToString()].ToString();
            strPortfolioID = Session[clsSystem_Session.strSession.tempportfolioid.ToString()].ToString();

            List<clsSwitchDetails_Client> ListSwitchDetails = (List<clsSwitchDetails_Client>)Session["SwitchDetails_Client"];
            saveclientSwitch(clsSwitch.enumSwitchStatus.Saved, ListSwitchDetails[0].propSwitchID, this.txtAmendDesc.Text.Trim());

            clsPortfolio oPortfolio = new clsPortfolio(strClientID, strPortfolioID, strUserID);

            bool hasIFAPermit = oPortfolio.propPortfolioDetails[0].propSwitchIFAPermit;
            int intSwitchID = ListSwitchDetails[0].propSwitchID;

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
                        strEmailMessage = clsEmail.generateEmailBody("ClientAmendmentNotification", null, Client.propForename + " " + Client.propSurname, null, null, null, null, null);
                    }
                    catch 
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "AlertNoEmailTemplate", "alert('System error! No email template found for this purpose. Please report this error to your systems administrator');", true);
                        return;
                    }
                    clsEmail.Send(strRecipient, "NAVSwitch@NoReply.com", "NAV-Switch client amended proposed switch", strEmailMessage, oPortfolio.propSwitch.propSwitchID, strClientID, clsEmail.enumEmailPurpose.ClientRequestingContact);
                    clsSwitch_Client.updateSwitchHeader(clsSwitch.enumSwitchStatus.Proposed, intSwitchID, this.txtAmendDesc.Text.Trim());
                    clsSwitch.updateSwitchHeader(intSwitchID, clsSwitch.enumSwitchStatus.Amended, new clsSwitch(intSwitchID).propDescription);

                    int intHistoryID = clsHistory.insertHeader(oPortfolio.propPortfolioID, intSwitchID, (Int16)clsSwitch.enumSwitchStatus.Amended);
                    clsHistory.insertDetailsClient(intHistoryID, ListSwitchDetails);
                    clsHistory.insertMessage(intHistoryID, this.txtAmendDesc.Text.Trim());

                    string backPageURL = "https://" + Request.ServerVariables["SERVER_NAME"] + ":" + Request.ServerVariables["SERVER_PORT"] + "/report/" + "portfoliodetails.asp";
                    ClientScript.RegisterStartupScript(this.GetType(), "EmailAmendNotification", "alert('An email was sent to notify your IFA regarding the amendments in the proposed switch.'); window.location='" + backPageURL + "';", true);                    
                }
                catch (Exception ex)
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "EmailFailedAlert", "alert('Sending failed. Error:  " + ex.Message.Replace("'", " ") + "');", true);
                    return;
                }

            }

            Session["SwitchDetails_Client"] = oPortfolio.propSwitchClient.propSwitchDetails;
            populateAmendSwitch((List<clsSwitchDetails_Client>)Session["SwitchDetails_Client"]);
        }

        protected void SendValidationCode_Click(object sender, EventArgs e)
        {
            sendValidationCode(this.txtSMSMobileNo.Text.Trim());
        }

        protected void btnCheckSecurityCode_Click(object sender, EventArgs e)
        {
            strUserID = Session[clsSystem_Session.strSession.User.ToString()].ToString();

            //try 
            //{
                if (!txtSecurityCode.Text.Equals(string.Empty))
                {
                    strClientID = Session[clsSystem_Session.strSession.clientID.ToString()].ToString();
                    strPortfolioID = Session[clsSystem_Session.strSession.tempportfolioid.ToString()].ToString();
                    int intSwitchID = (int)ViewState["intSwitchID"];

                    clsSecurityCode oSecurityCode = new clsSecurityCode(this.txtSecurityCode.Text.Trim());
                    string strSecurityCodeMessage = oSecurityCode.ValidateSecurityCode(intSwitchID, strClientID, strPortfolioID);

                    if (strSecurityCodeMessage.Contains("Sorry, the security code you have entered is incorrect, please re-enter the security code."))
                    {
                        this.mpeSecurityCodePanel.Show();
                    }
                    else if (strSecurityCodeMessage.Contains("Thank you, the proposed changes will be made to your portfolio."))
                    {
                        clsSwitchDetails.transferClientSwitchToIFA((List<clsSwitchDetails_Client>)Session["SwitchDetails_Client"], strUserID);

                        int intHistoryID = clsHistory.insertHeader(strPortfolioID, intSwitchID, (Int16)clsSwitch.enumSwitchStatus.Approved);
                        clsHistory.insertDetailsClient(intHistoryID, (List<clsSwitchDetails_Client>)Session["SwitchDetails_Client"]);
                        clsHistory.insertMessage(intHistoryID, new clsSwitch_Client(intSwitchID).propDescription);

                        NotifyApprovedSwtich(new clsPortfolio(strClientID, strPortfolioID, strUserID));

                        ClientScript.RegisterStartupScript(this.GetType(), "showApproveSwitch", string.Format("showApproveSwitchPanel();"), true);

                        showhideAmendArea(false);
                        showhideButtons(enumPageState.view);

                    }
                    ClientScript.RegisterStartupScript(this.GetType(), "alertMessage", string.Format("alert('{0}');", strSecurityCodeMessage), true);
                }

                clsPortfolio oPortfolio = new clsPortfolio(strClientID, strPortfolioID, strUserID);
                populateHeader(oPortfolio);
                populateProposedSwitch(oPortfolio.propSwitch.propSwitchDetails);
            //}
            //catch (Exception ex)
            //{
            //    string backPageURL = "https://" + Request.ServerVariables["SERVER_NAME"] + ":" + Request.ServerVariables["SERVER_PORT"] + "/report/" + "portfoliodetails.asp";
            //    ClientScript.RegisterStartupScript(this.GetType(), "alertApproveError", "alert('Error! " + ex.ToString().Replace("'", "") + "'); window.location='" + backPageURL + "';", true);
            //}
        }

        protected void btnContactMeSend_Click(object sender, EventArgs e) {

            strClientID = Session[clsSystem_Session.strSession.clientID.ToString()].ToString();
            strPortfolioID = Session[clsSystem_Session.strSession.tempportfolioid.ToString()].ToString();
            strUserID = Session[clsSystem_Session.strSession.User.ToString()].ToString();

            clsIFA IFA = new clsIFA(int.Parse(Session["ifaid"].ToString()));
            clsClient Client = new clsClient(strClientID);
            clsPortfolio Portfolio = new clsPortfolio(strClientID, strPortfolioID, strUserID);

            String strEmailMessage = "";

            try
            {
                String strRecipient = IFA.propIFAEmail;

                try 
                { 
                    strEmailMessage = clsEmail.generateEmailBody("ClientRequestContact", IFA.propIFA_Name, Client.propForename + " " + Client.propSurname,
                                                                     Portfolio.propCompany, this.txtContactMeByTel.Text.Trim(), this.txtContactMeComments.Text.Trim(), null, null);
                }
                catch
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "AlertNoEmailTemplate", "alert('System error! No email template found for this purpose. Please report this error to your systems administrator');", true);
                    return;
                }
                

                clsEmail.Send(strRecipient, "NAVSwitch@NoReply.com", "NAV-Switch client request", strEmailMessage, Portfolio.propSwitch.propSwitchID, strClientID, clsEmail.enumEmailPurpose.ClientRequestingContact);                

                clsSwitch.updateSwitchHeader(Portfolio.propSwitch.propSwitchID, clsSwitch.enumSwitchStatus.Request_ForDiscussion);

                int intHistoryID = clsHistory.insertHeader(Portfolio.propPortfolioID, Portfolio.propSwitch.propSwitchID, (Int16)clsSwitch.enumSwitchStatus.Request_ForDiscussion);
                clsHistory.insertMessage(intHistoryID, strEmailMessage);

                ClientScript.RegisterStartupScript(this.GetType(), "EmailRequestContactSent", "alert('Request for contact to your IFA has been sent successfully');", true);

                populateProposedSwitch(Portfolio.propSwitch.propSwitchDetails);

                enableButtons(clsSwitch.enumSwitchStatus.Request_ForDiscussion);

            }
            catch (Exception ex)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "EmailFailedAlert", "alert('Sending failed. Error:  " + ex.Message.Replace("'", " ") + "');", true);
                return;
            }

        }

        protected void btnDeclineSwitch_Click(object sender, EventArgs e)
        {
            strUserID = Session[clsSystem_Session.strSession.User.ToString()].ToString();
            strClientID = Session[clsSystem_Session.strSession.clientID.ToString()].ToString();
            strPortfolioID = Session[clsSystem_Session.strSession.tempportfolioid.ToString()].ToString();

            clsPortfolio oPortfolio = new clsPortfolio(strClientID, strPortfolioID, strUserID);

            int intSwitchID = (int)ViewState["intSwitchID"];

            string backPageURL = "https://" + Request.ServerVariables["SERVER_NAME"] + ":" + Request.ServerVariables["SERVER_PORT"] + "/report/" + "portfoliodetails.asp";

            try
            {
                clsIFA IFA = new clsIFA(int.Parse(Session["ifaid"].ToString()));
                clsClient Client = new clsClient(strClientID);
                String strEmailMessage = "";

                String strRecipient = IFA.propIFAEmail;

                try
                {
                    strEmailMessage = clsEmail.generateEmailBody("ClientDeclineNotification", IFA.propIFA_Name, Client.propForename + " " + Client.propSurname, oPortfolio.propCompany, null, this.txtDeclineDescription.Text.Trim(), null, null);
                }
                catch
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "AlertNoEmailTemplate", "alert('System error! No email template found for declining email. Please report this error to your systems administrator');", true);
                    return;
                }

                clsEmail.Send(strRecipient, "NAVSwitch@NoReply.com", "NAV-Switch client declined proposed switch", strEmailMessage, intSwitchID, strClientID, clsEmail.enumEmailPurpose.ClientDeclinedSwitchPortfolio);
                
                clsSwitch.updateSwitchHeader(intSwitchID, clsSwitch.enumSwitchStatus.Declined_Client, new clsSwitch(intSwitchID).propDescription);

                int intHistoryID = clsHistory.insertHeader(oPortfolio.propPortfolioID, intSwitchID, (Int16)clsSwitch.enumSwitchStatus.Declined_Client);                
                clsHistory.insertMessage(intHistoryID, strEmailMessage);

                clsSwitch_Client.declineSwitch(intSwitchID);
                
                ClientScript.RegisterStartupScript(this.GetType(), "alertDeclinedSwitch", "alert('This switch has been declined.'); window.location='" + backPageURL + "';", true);

            }
            catch (Exception ex)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "alertDeclinedSwitcherror", "alert('Error declining switch! " + ex.ToString().Replace("'", " ") + "'); window.location='" + backPageURL + "';", true);
            }
        }

        #endregion

        #region "procedures"

        #region "populate"

        private void populateHeader(clsPortfolio _clsPortfolio)
        {
            //this.lblValue_ PortfolioName.Text = strPortfolioName;
            this.lblValue_Company.Text = _clsPortfolio.propCompany;
            this.lblValue_PortfolioType.Text = _clsPortfolio.propPortfolioType;
            this.lblValue_Currency.Text = _clsPortfolio.propPortfolioCurrency;
            this.lblValue_AccountNumber.Text = _clsPortfolio.propAccountNumber;
            this.lblValue_PlanStatus.Text = _clsPortfolio.propPlanStatus;
            this.lblValue_StartDate.Text = _clsPortfolio.propPortfolioStartDate.ToString("dd/MM/yyyy");
            this.lblValue_PolicyCategory.Text = _clsPortfolio.propLiquidity;
            this.lblValue_Profile.Text = _clsPortfolio.propRiskProfile;
            this.lblValue_SpecialistInformation.Text = _clsPortfolio.propRetentionTerm;
            this.lblValue_Discretionary.Text = _clsPortfolio.propMFPercent == 0 ? "no" : "yes";

            Session["Company"] = _clsPortfolio.propCompany;
        }

        private void populateOriginalPortfolio(List<clsPortfolioDetails> listOriginalPortfolio)
        {
            this.gvPortfolioDetails.DataSource = listOriginalPortfolio;
            this.gvPortfolioDetails.DataBind();

            Label lblgvFooterCurrentValueClient = (Label)this.gvPortfolioDetails.FooterRow.Cells[7].FindControl("gvFooterCurrentValueClient");
            lblgvFooterCurrentValueClient.Text = listOriginalPortfolio[0].propTotalCurrentValueClient.ToString("n0");

            Label lblgvHeaderPurchaseCostFundPortfolioCurrency = (Label)this.gvPortfolioDetails.HeaderRow.Cells[5].FindControl("gvHeaderPurchaseCostFundPortfolioCurrency");
            lblgvHeaderPurchaseCostFundPortfolioCurrency.Text = listOriginalPortfolio[0].propPortfolioCurrency.ToString();

            Label lblgvHeaderValueClientCurCurrency = (Label)this.gvPortfolioDetails.HeaderRow.Cells[7].FindControl("gvHeaderValueClientCurCurrency");
            lblgvHeaderValueClientCurCurrency.Text = listOriginalPortfolio[0].propClientCurrency.ToString();

            Label lblgvHeaderGainLossCurrency = (Label)this.gvPortfolioDetails.HeaderRow.Cells[6].FindControl("gvHeaderGainLossCurrency");
            lblgvHeaderGainLossCurrency.Text = listOriginalPortfolio[0].propPortfolioCurrency.ToString();

        }

        private void populateProposedSwitch(List<clsSwitchDetails> listProposedSwitch)
        {

            this.gvSwitchDetails_IFA.DataSource = listProposedSwitch;
            this.gvSwitchDetails_IFA.DataBind();

            Label gvSwitchFooterLblTotalValue = (Label)this.gvSwitchDetails_IFA.FooterRow.Cells[3].FindControl("gvSwitchFooterLblTotalValue");
            gvSwitchFooterLblTotalValue.Text = listProposedSwitch[0].propTotalValue.ToString("n0");

            Label gvSwitchFooterLblTotalAllocation = (Label)this.gvSwitchDetails_IFA.FooterRow.Cells[4].FindControl("gvSwitchFooterLblTotalAllocation");
            gvSwitchFooterLblTotalAllocation.Text = listProposedSwitch[listProposedSwitch.Count - 1].propTotalAllocation.ToString("n2");

            this.lblSwitchDetails_IFAStatusValue.Text = new clsSwitch(listProposedSwitch[0].propSwitchID).propStatusString;
            this.txtProposedSwitchDesc.Text = new clsSwitch(listProposedSwitch[0].propSwitchID).propDescription.ToString();

            int intStatus = new clsSwitch(listProposedSwitch[0].propSwitchID).propStatus;

            if ((intStatus == (int)clsSwitch.enumSwitchStatus.Approved) || (intStatus == (int)clsSwitch.enumSwitchStatus.Locked))
            {
                this.btnApproveSwitch.Disabled = true;
                this.btnAmendSwitch.Enabled = false;
                this.btnDeclineSwitch.Enabled = false;
                this.btnContactMe.Enabled = false;
            }
        }

        private void populateAmendSwitch(List<clsSwitchDetails_Client> listAmendSwitch)
        {
            this.gvAmendSwitch.DataSource = listAmendSwitch;
            this.gvAmendSwitch.DataBind();

            if (listAmendSwitch != null) 
            {
                this.lblSwitchStatusValue.Text = new clsSwitch_Client(listAmendSwitch[0].propSwitchID).propStatusString;
                int intStatus = new clsSwitch(listAmendSwitch[0].propSwitchID).propStatus;

                Label gvSwitchFooterLblTotalValue = (Label)this.gvAmendSwitch.FooterRow.Cells[3].FindControl("gvSwitchFooterLblTotalValue");
                gvSwitchFooterLblTotalValue.Text = listAmendSwitch[0].propTotalValue.ToString("n0");

                Label gvSwitchFooterLblTotalAllocationOrig = (Label)this.gvAmendSwitch.FooterRow.Cells[4].FindControl("gvSwitchFooterLblTotalAllocationOrig");
                gvSwitchFooterLblTotalAllocationOrig.Text = listAmendSwitch[listAmendSwitch.Count - 1].propTotalAllocation.ToString("n2");


                Label gvSwitchFooterLblTotalAllocation = (Label)this.gvAmendSwitch.FooterRow.Cells[4].FindControl("gvSwitchFooterLblTotalAllocation");
                gvSwitchFooterLblTotalAllocation.Text = listAmendSwitch[listAmendSwitch.Count - 1].propTotalAllocation.ToString("n2");

                gvSwitchFooterHfTotalAllocation.Value = listAmendSwitch[listAmendSwitch.Count - 1].propTotalAllocation.ToString("n2");

                this.hfCurrentValueClientTotal.Value = listAmendSwitch[0].propTotalValue.ToString("n0");
                this.hfFundCount.Value = listAmendSwitch.Count.ToString();

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
            }


            //if (new clsSwitch(listAmendSwitch[0].propSwitchID).propStatusString == clsSwitch.enumSwitchStatus.Saved.ToString())
            //{
            //    this.btnSave.Disabled = true;
            //}
            //else
            //{
            //    this.btnSave.Disabled = false;
            //}
            //if (intStatus == (int)clsSwitch.enumSwitchStatus.Proposed)
            //{
            //    btnSave.Disabled = true;
            //    btnSwitch.Disabled = true;
            //}
            //else if (intStatus == (int)clsSwitch.enumSwitchStatus.Approved)
            //{
            //    btnSave.Disabled = true;
            //    btnSwitch.Disabled = true;
            //    btnCancel.Enabled = false;
            //}
        }

        protected string CheckNull(object objGrid)
        {
            if (object.ReferenceEquals(objGrid, DBNull.Value) || objGrid.ToString().Equals(""))
            {
                return "no data";
            }
            else
            {
                if (objGrid.ToString().Equals("01/01/1800") || objGrid.ToString().Equals(""))
                {
                    return "no data";
                }
                else
                {
                    return objGrid.ToString();
                }
            }
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
                strClientID = Session[clsSystem_Session.strSession.clientID.ToString()].ToString();
                strPortfolioID = Session[clsSystem_Session.strSession.tempportfolioid.ToString()].ToString();
                strUserID = Session[clsSystem_Session.strSession.User.ToString()].ToString();
                //clsPortfolio objPortfolio = new clsPortfolio(strClientID, strPortfolioID, strUserID);

                clsSwitch_Client objSwitch_Client = new clsSwitch_Client((int)ViewState["intSwitchID"]);

                this.populateAmendSwitch(objSwitch_Client.propSwitchDetails);
                Session["SwitchDetails_Client"] = objSwitch_Client.propSwitchDetails;

                this.lblTitle_AmendSwitch.Visible = true;
                this.lblSwitchStatusTitle.Visible = true;
                this.lblSwitchStatusValue.Visible = true;
            }
            else
            {
                this.populateAmendSwitch(null);

                this.lblTitle_AmendSwitch.Visible = false;
                this.lblSwitchStatusTitle.Visible = false;
                this.lblSwitchStatusValue.Visible = false;
            }

        }

        #endregion
       
        #region "data"

        private void saveclientSwitch(clsSwitch.enumSwitchStatus SwitchStatus, int intSwitchID, string strDesc)
        {
            strUserID = Session[clsSystem_Session.strSession.User.ToString()].ToString();
            strClientID = Session[clsSystem_Session.strSession.clientID.ToString()].ToString();
            strPortfolioID = Session[clsSystem_Session.strSession.tempportfolioid.ToString()].ToString();

            List<clsSwitchDetails_Client> newListSwitchDetails = (List<clsSwitchDetails_Client>)Session["SwitchDetails_Client"];

            clsSwitch_Client.updateSwitchHeader(SwitchStatus, intSwitchID, strDesc);
            clsSwitchDetails_Client.insertSwitchDetails(newListSwitchDetails, strUserID, intSwitchID);

        }

        public void captureSwitchGridviewDetails()
        {

            List<clsSwitchDetails_Client> ListSwitchDetails = (List<clsSwitchDetails_Client>)Session["SwitchDetails_Client"];

            foreach (GridViewRow row in this.gvAmendSwitch.Rows)
            {
                HiddenField hfSwitchDetailsID = (HiddenField)row.FindControl("hfSwitchDetailsID");
                HiddenField hfSelectedFundID = (HiddenField)row.FindControl("hfSelectedFundID");
                TextBox txtAllocation = (TextBox)row.FindControl("gvSwitchTxtTargetAllocation");
                HiddenField gvhfSwitchHeaderValue = (HiddenField)row.FindControl("gvhfSwitchHeaderValue");
                HiddenField gvhfSwitchDetailsLblUnits = (HiddenField)row.FindControl("gvhfSwitchDetailsLblUnits");

                foreach (clsSwitchDetails_Client SwitchDetails in ListSwitchDetails)
                {
                    if (hfSelectedFundID.Value.Trim() == SwitchDetails.propFund.propFundID.ToString())
                    {
                        SwitchDetails.propAllocation = txtAllocation.Text.ToString() == "" ? 0f : float.Parse(txtAllocation.Text.ToString());
                        SwitchDetails.propValue = float.Parse(gvhfSwitchHeaderValue.Value.ToString());
                        SwitchDetails.propUnits = decimal.Parse(gvhfSwitchDetailsLblUnits.Value.ToString());
                    }

                    Session["SwitchDetails_Client"] = ListSwitchDetails;
                }                
            }
        }

        private void addNewFund(int intNewFundID)
        {
            captureSwitchGridviewDetails();

            List<clsSwitchDetails_Client> currentListSwitchDetails = (List<clsSwitchDetails_Client>)Session["SwitchDetails_Client"];
            String strUserID = Session[clsSystem_Session.strSession.User.ToString()].ToString();
            clsClient Client = new clsClient(Session[clsSystem_Session.strSession.clientID.ToString()].ToString());

            List<clsSwitchDetails_Client> newListSwitchDetails = new List<clsSwitchDetails_Client>();

            try
            {
                newListSwitchDetails = clsSwitchDetails_Client.addFund(intNewFundID, currentListSwitchDetails, Client.propClientID, Client.propCurrency, strUserID);
            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "errDuplicateDund", "alert('" + ex.Message + "');", true);
                return;
            }

            Session["SwitchDetails_Client"] = newListSwitchDetails;

            populateAmendSwitch(newListSwitchDetails);

        }

        private void replaceFund(int intOldFundID, int intNewFundID)
        {
            captureSwitchGridviewDetails();

            List<clsSwitchDetails_Client> PreviousListSwitchDetails = (List<clsSwitchDetails_Client>)Session["SwitchDetails_Client"];

            strUserID = Session[clsSystem_Session.strSession.User.ToString()].ToString();
            strClientID = Session[clsSystem_Session.strSession.clientID.ToString()].ToString();
            strPortfolioID = Session[clsSystem_Session.strSession.tempportfolioid.ToString()].ToString();

            clsPortfolio Portfolio = new clsPortfolio(strClientID, strPortfolioID, strUserID);

            List<clsSwitchDetails_Client> newListSwitchDetails = new List<clsSwitchDetails_Client>();

            try
            {
                newListSwitchDetails = clsSwitchDetails_Client.FundChange(intOldFundID, intNewFundID, PreviousListSwitchDetails, strClientID, Portfolio.propPortfolioDetails[0].propClientCurrency);
            }
            catch (Exception ex)
            {
                populateAmendSwitch((List<clsSwitchDetails_Client>)Session["SwitchDetails_Client"]);
                ClientScript.RegisterStartupScript(this.GetType(), "errDuplicateDund", "alert('" + ex.Message + "');", true);
                return;
            }

            Session["SwitchDetails_Client"] = newListSwitchDetails;

            this.populateAmendSwitch(newListSwitchDetails);

        }

        private void btnSave_Click()
        {
            captureSwitchGridviewDetails();

            strUserID = Session[clsSystem_Session.strSession.User.ToString()].ToString();
            strClientID = Session[clsSystem_Session.strSession.clientID.ToString()].ToString();
            strPortfolioID = Session[clsSystem_Session.strSession.tempportfolioid.ToString()].ToString();

            List<clsSwitchDetails_Client> ListSwitchDetails = (List<clsSwitchDetails_Client>)Session["SwitchDetails_Client"];
            saveclientSwitch(clsSwitch.enumSwitchStatus.Saved, ListSwitchDetails[0].propSwitchID, null);

            clsPortfolio Portfolio = new clsPortfolio(strClientID, strPortfolioID, strUserID);

            Session["SwitchDetails_Client"] = Portfolio.propSwitchClient.propSwitchDetails;
            populateAmendSwitch((List<clsSwitchDetails_Client>)Session["SwitchDetails_Client"]);

            string backPageURL = "https://" + Request.ServerVariables["SERVER_NAME"] + ":" + Request.ServerVariables["SERVER_PORT"] + "/report/" + "portfoliodetails.asp";
            ClientScript.RegisterStartupScript(this.GetType(), "alertSaveSuccess", "alert('Amendments saved.'); window.location='" + backPageURL + "';", true);

        }

        #endregion

        #region "SMS"

        private void sendValidationCode(string strSMSMobileNo)
        {
            strClientID = Session[clsSystem_Session.strSession.clientID.ToString()].ToString();
            strPortfolioID = Session[clsSystem_Session.strSession.tempportfolioid.ToString()].ToString();
            strUserID = Session[clsSystem_Session.strSession.User.ToString()].ToString();
            int intSwitchID = (int)ViewState["intSwitchID"];

            string strSecurityCode = GenerateSecurityCode(intSwitchID, strClientID, strPortfolioID);

            clsSMS.subclsSMSTemplate osubclsSMSTemplate = new clsSMS.subclsSMSTemplate(2);
            string strReplacerVariable = clsSMS.subclsSMSTemplate.strSecurityCodeVariable;
            string strMessage = osubclsSMSTemplate.propMessage.Replace(strReplacerVariable, strSecurityCode);

            if (!strSMSMobileNo.Trim().Equals(string.Empty))
            {
                sendSMS(strUserID, strMessage, strSMSMobileNo);
                //ClientScript.RegisterStartupScript(this.GetType(), "testingSecurityCode", string.Format("alert('{0}-{1}-{2}');", strMessage, strReplacerVariable, strSecurityCode), true);
            }
            ClientScript.RegisterStartupScript(this.GetType(), "showSecurityCode", string.Format("showSecurityCodePanel();"), true);
        }

        private void sendSMS(string strUserID, string strSMSMessage, string strSMSMobileNo)
        {
            clsSMS SMS = new clsSMS(strUserID);
            try {
                SMS.sendMessage(strSMSMobileNo.Trim(), strSMSMessage);
            }
            catch (Exception ex)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "alrterror", "alert('error!! " + ex.Message.ToString().Replace("'", " ") + "');", true);
            }
            

            if (!SMS.propErrorMsg.Equals(""))
            {
                ClientScript.RegisterStartupScript(this.GetType(), "alertMsgFailed", "alert('Sending Failed  " + SMS.propErrorMsg.Replace("'", " ") + "');", true);
                this.mdlThirdSwitchPopup.Show();
            }

            if (!SMS.propReturnID.Equals(""))
            {
                string strMessageOuput = "You will recieve a text message to your registered mobile number. Enter the code that it contains in the Approval Code field that will appear below the portfolio that you have approved.";
                ClientScript.RegisterStartupScript(this.GetType(), "alertMsgSent", string.Format("alert('{0}');", strMessageOuput), true);
                this.mpeSecurityCodePanel.Show();
            }
        }

        private string GenerateSecurityCode(int intSwitchID, string strClientID, string strPortfolioID)
        {
            string strSecurityCode = string.Empty;
            int intSecurityCode = 0;
            while (intSecurityCode == 0)
            {
                clsSecurityCode oSecurityCode = new clsSecurityCode(4);
                intSecurityCode = oSecurityCode.insertEncryptedCode(intSwitchID, strClientID, strPortfolioID);
                strSecurityCode = oSecurityCode.propSecurityCode;
            }
            return strSecurityCode;
        }

        #endregion

        #endregion

        #region "Popup - Funds"

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
                Label gvSwitchFooterLblTotalValue = (Label)this.gvAmendSwitch.FooterRow.Cells[3].FindControl("gvSwitchFooterLblTotalValue");

                this.mpeFundSearch.Show();
            }
        }

        protected void btnCloseFundSearch_Click(object sender, EventArgs e)
        {
            clearPopupFundFields();
            this.mpeFundSearch.Hide();
            captureSwitchGridviewDetails();
            populateAmendSwitch((List<clsSwitchDetails_Client>)Session["SwitchDetails_Client"]);
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
                    ClientScript.RegisterStartupScript(this.GetType(), "doPostChangeFund", "__doPostBack('hfSelectedFundID', 'AddMoreFund');", true);
                    break;
                case "edit":
                    clearPopupFundFields();
                    ClientScript.RegisterStartupScript(this.GetType(), "doPostChangeFund", "__doPostBack('hfSelectedFundID', 'ChangeFund');", true);
                    break;
                default:
                    ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('error on funds popUps  : '" + this.hfPopUpFundAction.Value + ");", true);
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

        public void FixAjaxValidationSummaryErrorLabel_PreRender(object sender, EventArgs e)
        {

            string objId = ((Label)sender).ID;

            ScriptManager.RegisterStartupScript(this, this.GetType(), objId, ";", true);

        }

        #endregion

        private enum enumPageState
        {
            AmmendingSwitch,
            hideAll,
            view
        }

        #region "Client-side handlers"

        private void AddClientHandlers()
        {
            string jsFunction = "getFlickerSolved";
            string jsConditionFortxtPopup = string.Format("PopupValidation('{0}','{1}');", txtPopupFund.ClientID, PopupFundSearch.ClientID);

            mpeFundSearch.OnCancelScript += jsFunction + "('" + PopupFundSearch.ClientID + "');";
            btnCloseFundSearch.OnClientClick += jsFunction + "('" + PopupFundSearch.ClientID + "');";
            btnPopupFund.OnClientClick += jsConditionFortxtPopup;
            txtPopupFund.Attributes["autocomplete"] = "off";
            txtPopupFund.Attributes["onkeypress"] = string.Empty;
            txtPopupFund.Attributes["onchange"] = string.Empty;
            txtPopupFund.Attributes.Remove("onkeypress");
            txtPopupFund.Attributes.Remove("onchange");

            btnSwitchProceed.Attributes["onClick"] += jsFunction + "('" + FirstSwitchPopup.ClientID + "');";
            btnSwitchCancel.Attributes["onClick"] += jsFunction + "('" + FirstSwitchPopup.ClientID + "');";
            FirstSwitchPopup.Attributes["onCancelScript"] += jsFunction + "('" + FirstSwitchPopup.ClientID + "');";

            btnAmendYes.Attributes["onclick"] += jsFunction + "('" + SecondSwitchPopup.ClientID + "');";
            btnAmendNo.Attributes["onclick"] += jsFunction + "('" + SecondSwitchPopup.ClientID + "');";
            SecondSwitchPopup.Attributes["onCancelScript"] += jsFunction + "('" + SecondSwitchPopup.ClientID + "');";

            btnAmendSaveSend.Attributes["onclick"] += jsFunction + "('" + ThirdSwitchPopup.ClientID + "')";
            btnAmendCancelSend.Attributes["onclick"] += jsFunction + "('" + ThirdSwitchPopup.ClientID + "');";
            ThirdSwitchPopup.Attributes["onCancelScript"] += jsFunction + "('" + ThirdSwitchPopup.ClientID + "');";

            btnContactMeSend.Attributes["onclick"] += jsFunction + "('" + PopupContactMe.ClientID + "');";
            btnContactMeCancel.Attributes["conclick"] += jsFunction + "('" + PopupContactMe.ClientID + "');";
            PopupContactMe.Attributes["onCancelScript"] += jsFunction + "('" + PopupContactMe.ClientID + "');";
        }

        #endregion

        protected void NotifyApprovedSwtich(clsPortfolio Portfolio)
        {
            //try
            //{
                int intIFAID = int.Parse(Session["ifaid"].ToString());
                clsIFA IFA = new clsIFA(intIFAID);

                string htmlTemplate = clsOutput.generateApprovedSwitch(Portfolio, IFA.propIFA_ID);
                StyleSheet style = clsOutput.getStyleSheet_ApprovedSwitch();
                string strFilename = clsOutput.generateOutputFile(clsOutput.enumOutputType.PDF, htmlTemplate, style, Portfolio.propSwitch.propSwitchID, clsOutput.enumSwitchType.Portfolio);

                string strRecepient = IFA.propIFAEmail;
                string strSender = "NAVSwitch@NoReply.com";
                string strSubject = "Switch Instruction";
                string strBody = clsEmail.generateEmailBody(((new clsCompany(Portfolio.propCompanyID)).propSignedConfirmation ? "NotifyApprovedEmailReqSign" : "NotifyApprovedEmail"), null, null, null, null, null, Portfolio.propCompany, Portfolio.propSwitch.propSwitchID.ToString());
                clsEmail.SendWithAttachment(strRecepient, strSender, strSubject, strBody, Portfolio.propSwitch.propSwitchID, Portfolio.propClientID, clsEmail.enumEmailPurpose.ApproveSwitchNotification, strFilename);

                clsClient client = new clsClient(Portfolio.propClientID);
                string ClientName = client.propForename + " " + client.propSurname;
                if (!String.IsNullOrEmpty(client.propEmailWork))
                {
                    strRecepient = client.propEmailWork;
                    strBody = clsEmail.generateEmailBody(((new clsCompany(Portfolio.propCompanyID)).propSignedConfirmation ? "NotifyClientApprovedEmailReqSign" : "NotifyClientApprovedEmail"), null, null, ClientName, null, null, Portfolio.propCompany, null);
                    clsEmail.SendWithAttachment(strRecepient, strSender, strSubject, strBody, Portfolio.propSwitch.propSwitchID, Portfolio.propClientID, clsEmail.enumEmailPurpose.ApproveSwitchNotification, strFilename);
                }
                if (!String.IsNullOrEmpty(client.propEmailPersonal))
                {
                    strRecepient = client.propEmailPersonal;
                    strBody = clsEmail.generateEmailBody(((new clsCompany(Portfolio.propCompanyID)).propSignedConfirmation ? "NotifyClientApprovedEmailReqSign" : "NotifyClientApprovedEmail"), null, null, ClientName, null, null, Portfolio.propCompany, null);
                    clsEmail.SendWithAttachment(strRecepient, strSender, strSubject, strBody, Portfolio.propSwitch.propSwitchID, Portfolio.propClientID, clsEmail.enumEmailPurpose.ApproveSwitchNotification, strFilename);
                }

                //:SwitchType_IFA Name_Client Name_yyyy-mm-dd.pdf
            //}
            //catch (Exception ex)
            //{
            //    Response.Write("Exception Occured<br/>");
            //    Response.Write(String.Format("<p>{0}</p><br/>", ex.ToString()));
            //}
        }


    }
}