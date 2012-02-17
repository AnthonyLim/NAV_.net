using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Web.SessionState;

namespace NAV.Portfolio
{
    public partial class Switch : System.Web.UI.Page
    {
        #region Factor on Form

        string ClientID()
        {
            if (Session[clsSystem_Session.strSession.clientID.ToString()] != null)
            {
                return Session[clsSystem_Session.strSession.clientID.ToString()].ToString();
            }
            else
            {
                return string.Empty;
            }
        }
        string PortfolioID()
        {
            if (Session[clsSystem_Session.strSession.tempportfolioid.ToString()] != null)
            {
                return Session[clsSystem_Session.strSession.tempportfolioid.ToString()].ToString();
            }
            else
            {
                return string.Empty;
            }
        }
        string UserID()
        {
            if (Session[clsSystem_Session.strSession.User.ToString()] != null)
            {
                return Session[clsSystem_Session.strSession.User.ToString()].ToString();
            }
            else
            {
                return string.Empty;
            }
        }

        clsPortfolio Portfolio()
        {
            return new clsPortfolio(ClientID(), PortfolioID(), UserID());
        }

        #endregion

        private string strPortfolioName;

        #region Switch Events

        protected void Page_Init(object sender, EventArgs e)
        {
            if (Session[clsSystem_Session.strSession.User.ToString()] == null)
            {
                Response.Redirect("https://" + Request.ServerVariables["SERVER_NAME"] + ":" + Request.ServerVariables["SERVER_PORT"] + "/report/" + "portfoliodetails.asp");
            }
            AddClientHandlers();
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Page.IsPostBack)
            {
                if (Request["__EVENTARGUMENT"] == "SaveSwitchDetails") { btnSave_Click(); }
                else if (Request["__EVENTARGUMENT"] == "ChangeFund") { replaceFund(int.Parse(this.hfFundIDOrig.Value), int.Parse(this.hfFundIDNew.Value)); }
                else if (Request["__EVENTARGUMENT"] == "AddMoreFund") { addNewFund(int.Parse(this.hfFundIDNew.Value)); }
            }
            else
            {
                Session["SourcePage"] = "/report/portfoliodetails.asp";
                string strSurname = Session[clsSystem_Session.strSession.surname.ToString()].ToString();
                string strForename = Session[clsSystem_Session.strSession.Forenames.ToString()].ToString();
                string strCode = Session[clsSystem_Session.strSession.code.ToString()].ToString();

                if (Session[clsSystem_Session.strSession.showname.ToString()].ToString() == "yes")
                {
                    if (Session[clsSystem_Session.strSession.surname.ToString()] == null)
                    {
                        strPortfolioName = "Portfolio Details";
                    }
                    else
                    {
                        if (Session[clsSystem_Session.strSession.lang.ToString()].ToString() == "2")
                        {
                            strPortfolioName = string.Format("{0} {1}({2})", strSurname, strForename, strCode);
                        }
                        else
                        {
                            strPortfolioName = string.Format("{1} {0}({2})", strSurname, strForename, strCode);
                        }
                    }
                }
                else
                {
                    strPortfolioName = Session[clsSystem_Session.strSession.code.ToString()].ToString();
                }
                setSessionCodepage();

                populateHeader(Portfolio(), strPortfolioName);
                this.populateDetails(Portfolio().propPortfolioDetails);
                if (Portfolio().propSwitch.propStatus == (int)clsSwitch.enumSwitchStatus.Amended)
                {
                    clsSwitch_Client oClientSwitch = new clsSwitch_Client(Portfolio().propSwitch.propSwitchID);
                    populateSwitchDetailsClient(oClientSwitch);
                }

                if (Portfolio().propPortfolioDetails.Count > 0)
                {
                    clsSwitch oSwitch = new clsSwitch(Portfolio(), UserID());
                    populateSwitchDetails(oSwitch.propSwitchDetails);
                    pageStatus(Portfolio().propSwitch.propStatus);
                }
            }
        }
        private void btnSave_Click()
        {
            captureSwitchGridviewDetails();

            List<clsSwitchDetails> ListSwitchDetails = (List<clsSwitchDetails>)Session["SwitchDetails"];
            saveSwitch(clsSwitch.enumSwitchStatus.Saved, ListSwitchDetails[0].propSwitchID, string.Empty);
            clsSwitch oSwitch = new clsSwitch(Portfolio(), UserID());
            populateSwitchDetails(oSwitch.propSwitchDetails);

            string backPageURL = "https://" + Request.ServerVariables["SERVER_NAME"] + ":" + Request.ServerVariables["SERVER_PORT"] + "/report/" + "portfoliodetails.asp";
            ClientScript.RegisterStartupScript(this.GetType(), "alertSaveSuccess", "alert('This Switch has been saved.'); window.location='" + backPageURL + "';", true);
        }
        protected void btnSwitchNo_Click(object sender, EventArgs e)
        {
            btnSave_Click();
        }
        protected void btnSwitchSendSave_Click(object sender, EventArgs e)//
        {
            string strButtonClick = string.Empty;
            if (ViewState["Button"] != null)
            {
                strButtonClick = ViewState["Button"].ToString();
            }
            doSwitchSendSave(strButtonClick);
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            List<clsSwitchDetails> ListSwitchDetails = (List<clsSwitchDetails>)Session["SwitchDetails"];
            clsSwitch oSwitch = new clsSwitch(Portfolio(), UserID());
            if ((oSwitch.propStatus != (int)clsSwitch.enumSwitchStatus.Draft) && (oSwitch.propStatus != (int)clsSwitch.enumSwitchStatus.Saved))
            {
                int intHistoryID = clsHistory.insertHeader(PortfolioID(), oSwitch.propSwitchID, (Int16)clsSwitch.enumSwitchStatus.Cancelled);
            }
            clsSwitch.deleteSwitch(ListSwitchDetails[0].propSwitchID);
            string backPageURL = "https://" + Request.ServerVariables["SERVER_NAME"] + ":" + Request.ServerVariables["SERVER_PORT"] + "/report/" + "portfoliodetails.asp";
            ClientScript.RegisterStartupScript(this.GetType(), "alertCancelledSwitch", "alert('This Switch has been cancelled'); window.location='" + backPageURL + "';", true);
        }
        protected void btnComplete_Click(object sender, EventArgs e)
        {
            List<clsSwitchDetails> ListSwitchDetails = (List<clsSwitchDetails>)Session["SwitchDetails"];

            clsSwitch.updateSwitchHeader(ListSwitchDetails[0].propSwitchID, clsSwitch.enumSwitchStatus.Completed);
            clsHistory.insertHeader(Portfolio().propPortfolioID, ListSwitchDetails[0].propSwitchID, (short)clsSwitch.enumSwitchStatus.Completed);

            string backPageURL = "https://" + Request.ServerVariables["SERVER_NAME"] + ":" + Request.ServerVariables["SERVER_PORT"] + "/report/" + "portfoliodetails.asp";
            ClientScript.RegisterStartupScript(this.GetType(), "alertCompletedSwitch", "alert('This Switch has been finally completed.'); window.location='" + backPageURL + "';", true);
        }
        protected void lbtnRemoveFund_Click(object sender, CommandEventArgs e)
        {
            captureSwitchGridviewDetails();

            List<clsSwitchDetails> PreviousListSwitchDetails = (List<clsSwitchDetails>)Session["SwitchDetails"];
            List<clsSwitchDetails> oSwitchDetailsList = clsSwitchDetails.removeSwitchDetails(int.Parse(e.CommandArgument.ToString()), PreviousListSwitchDetails);
            populateSwitchDetails(oSwitchDetailsList);
        }
        protected void lbtnSMSTemplateList_Click(object sender, EventArgs e)
        { //<<---temporary!!!!
            Response.Redirect("../Admin/SMSTemplate_List.aspx");
        }
        protected void lbtnEmailTemplateList_Click(object sender, EventArgs e)
        {
            Response.Redirect("../Admin/EmailTemplate_List.aspx");
        }
        protected void lbtnHistory_Click(object sender, EventArgs e)
        {
            List<clsSwitchDetails> SwitchDetails = (List<clsSwitchDetails>)Session["SwitchDetails"];
            String strSwitchID = SwitchDetails[0].propSwitchID.ToString();
            String strHistoryURL = "SwitchHistory.aspx?SID=" + strSwitchID + "&PID=" + PortfolioID() + "&CID=" + ClientID();
            Session["SourcePage"] = "/ASPX/Portfolio/Switch.aspx";
            Response.Redirect(strHistoryURL);
        }
        protected void lbtnSignedConfirmation1(object sender, EventArgs e)
        {
            Session["SourcePage"] = "/ASPX/Portfolio/Switch.aspx";
            Response.Redirect("../Admin/EmailSignedConfirmation_List.aspx");
        }

        #endregion

        #region Populate Data
        private void populateHeader(clsPortfolio _clsPortfolio, string strPortfolioName)
        {
            this.lblValue_PortfolioName.Text = strPortfolioName;
            this.lblValue_Company.Text = _clsPortfolio.propCompany;
            this.lblValue_PortfolioType.Text = _clsPortfolio.propPortfolioType;
            this.lblValue_Currency.Text = _clsPortfolio.propPortfolioCurrency;
            this.lblValue_AccountNumber.Text = _clsPortfolio.propAccountNumber;
            this.lblValue_PlanStatus.Text = _clsPortfolio.propPlanStatus;
            this.lblValue_StartDate.Text = _clsPortfolio.propPortfolioStartDate.ToString("dd/MM/yyyy");
            this.lblValue_PolicyCategory.Text = _clsPortfolio.propLiquidity;
            this.lblValue_Profile.Text = _clsPortfolio.propRiskProfile;
            this.lblValue_SpecialistInformation.Text = _clsPortfolio.propRetentionTerm;
            if (_clsPortfolio.propPortfolioDetails.Count > 0)
            {
                this.lblValue_Discretionary.Text = _clsPortfolio.propPortfolioDetails[0].propMFPercent == 0 ? "no" : "yes";
            }
            else
            {
                btnSave.Disabled = true;
                btnSwitch.Disabled = true;
                btnCancel.Enabled = false;
                lblSwitchStatusTitle.Visible = false;
            }
        }
        private void populateDetails(List<clsPortfolioDetails> listPortfolioDetails)
        {
            this.gvPortfolioDetails.DataSource = listPortfolioDetails;
            this.gvPortfolioDetails.DataBind();
            if (listPortfolioDetails.Count > 0)
            {
                Label lblgvFooterCurrentValueClient = (Label)this.gvPortfolioDetails.FooterRow.Cells[7].FindControl("gvFooterCurrentValueClient");
                lblgvFooterCurrentValueClient.Text = listPortfolioDetails[0].propTotalCurrentValueClient.ToString("n0");

                Label lblgvHeaderPurchaseCostFundPortfolioCurrency = (Label)this.gvPortfolioDetails.HeaderRow.Cells[5].FindControl("gvHeaderPurchaseCostFundPortfolioCurrency");
                lblgvHeaderPurchaseCostFundPortfolioCurrency.Text = listPortfolioDetails[0].propPortfolioCurrency.ToString();

                Label lblgvHeaderValueClientCurCurrency = (Label)this.gvPortfolioDetails.HeaderRow.Cells[7].FindControl("gvHeaderValueClientCurCurrency");
                lblgvHeaderValueClientCurCurrency.Text = listPortfolioDetails[0].propClientCurrency.ToString();

                Label lblgvHeaderGainLossCurrency = (Label)this.gvPortfolioDetails.HeaderRow.Cells[6].FindControl("gvHeaderGainLossCurrency");
                lblgvHeaderGainLossCurrency.Text = listPortfolioDetails[0].propPortfolioCurrency.ToString();

                Session["SwitchDetails"] = null;
                Session["HasDiscretionary"] = Portfolio().propPortfolioDetails[0].propMFPercent;
            }
        }
        private void populateSwitchDetailsClient(clsSwitch_Client oSwitchClient)
        {
            this.lblSwitchClientDescription.Text = oSwitchClient.propDescription;
            gvSwitchDetailsViewLabelStatusValue.Text = oSwitchClient.propStatusString;
            gvSwitchDetailsClient.DataSource = oSwitchClient.propSwitchDetails;
            gvSwitchDetailsClient.DataBind();

            Label gvSwitchDetailsClientFooterLabelTotalValue = (Label)gvSwitchDetailsClient.FooterRow.Cells[3].FindControl("gvSwitchDetailsClientFooterLabelTotalValue");
            gvSwitchDetailsClientFooterLabelTotalValue.Text = oSwitchClient.propSwitchDetails[oSwitchClient.propSwitchDetails.Count - 1].propTotalValue.ToString("n0");
            Label gvSwitchDetailsClientFooterLabelTotalTargetAllocation = (Label)gvSwitchDetailsClient.FooterRow.Cells[4].FindControl("gvSwitchDetailsClientFooterLabelTotalTargetAllocation");
            gvSwitchDetailsClientFooterLabelTotalTargetAllocation.Text = oSwitchClient.propSwitchDetails[oSwitchClient.propSwitchDetails.Count - 1].propTotalAllocation.ToString("n2");

            Session["SwitchDetailsClient"] = oSwitchClient.propSwitchDetails;
        }
        private void populateSwitchDetails(List<clsSwitchDetails> listSwitchDetails)
        {
            this.gvSwitchDetails.DataSource = listSwitchDetails;
            this.gvSwitchDetails.DataBind();

            if (listSwitchDetails.Count > 0)
            {
                this.lblSwitchStatusValue.Text = new clsSwitch(listSwitchDetails[0].propSwitchID).propStatusString;
                int intStatus = new clsSwitch(listSwitchDetails[0].propSwitchID).propStatus;

                Label gvSwitchFooterLblTotalValue = (Label)this.gvSwitchDetails.FooterRow.Cells[3].FindControl("gvSwitchFooterLblTotalValue");
                gvSwitchFooterLblTotalValue.Text = listSwitchDetails[0].propTotalValue.ToString("n0");

                Label gvSwitchFooterLblTotalAllocationOrig = (Label)this.gvSwitchDetails.FooterRow.Cells[4].FindControl("gvSwitchFooterLblTotalAllocationOrig");
                gvSwitchFooterLblTotalAllocationOrig.Text = listSwitchDetails[listSwitchDetails.Count - 1].propTotalAllocation.ToString("n2");

                Label gvSwitchFooterLblTotalAllocation = (Label)this.gvSwitchDetails.FooterRow.Cells[4].FindControl("gvSwitchFooterLblTotalAllocation");
                gvSwitchFooterLblTotalAllocation.Text = listSwitchDetails[listSwitchDetails.Count - 1].propTotalAllocation.ToString("n2");

                gvSwitchFooterHfTotalAllocation.Value = listSwitchDetails[listSwitchDetails.Count - 1].propTotalAllocation.ToString("n2");

                this.hfCurrentValueClientTotal.Value = listSwitchDetails[0].propTotalValue.ToString("n0");
                this.hfFundCount.Value = listSwitchDetails.Count.ToString();

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

                if (new clsSwitch(listSwitchDetails[0].propSwitchID).propStatusString == clsSwitch.enumSwitchStatus.Saved.ToString())
                {
                    this.btnSave.Disabled = true;
                }
                else
                {
                    this.btnSave.Disabled = false;
                }
                Session["SwitchDetails"] = listSwitchDetails;
            }
        }
        #endregion

        #region GridView Functions
        private void addNewFund(int intNewFundID)
        {
            captureSwitchGridviewDetails();

            List<clsSwitchDetails> ListSwitchDetails = (List<clsSwitchDetails>)Session["SwitchDetails"];
            foreach (clsSwitchDetails SwitchDetail in ListSwitchDetails)
            {
                if (SwitchDetail.propFund.propFundID == intNewFundID)
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "alertErrDuplicateFund", "alert('Duplicate funds. Please choose another.');", true);
                    return;
                }
            }
            clsSwitchDetails NewSwitchDetail = new clsSwitchDetails();
            NewSwitchDetail.propAllocation = 0f;
            NewSwitchDetail.propCreated_By = UserID();
            NewSwitchDetail.propDate_Created = DateTime.Now;
            NewSwitchDetail.propDate_LastUpdate = DateTime.Now;
            NewSwitchDetail.propFund = new clsFund(intNewFundID);
            NewSwitchDetail.propSwitchDetailsID = 0;
            NewSwitchDetail.propSwitchID = ListSwitchDetails[0].propSwitchID;
            NewSwitchDetail.propTotalAllocation = ListSwitchDetails[ListSwitchDetails.Count - 1].propTotalAllocation;
            NewSwitchDetail.propCurrencyMultiplier = clsCurrency.getCurrencyMultiplier(ClientID(), NewSwitchDetail.propFund.propCurrency);
            NewSwitchDetail.propIsDeletable = true;

            ListSwitchDetails.Add(NewSwitchDetail);
            populateSwitchDetails(ListSwitchDetails);
           
        }
        private void replaceFund(int intOldFundID, int intNewFundID)
        {
            captureSwitchGridviewDetails();
            List<clsSwitchDetails> PreviousListSwitchDetails = (List<clsSwitchDetails>)Session["SwitchDetails"];

            List<clsSwitchDetails> newListSwitchDetails = new List<clsSwitchDetails>();

            try
            {
                newListSwitchDetails = clsSwitchDetails.FundChange(intOldFundID, intNewFundID, PreviousListSwitchDetails, ClientID(), Portfolio().propPortfolioDetails[0].propClientCurrency);
            }
            catch (Exception ex)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "errDuplicateDund", "alert('" + ex.Message + "');", true);
                return;
            }
            this.populateSwitchDetails(newListSwitchDetails);
            
        }
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
        public void captureSwitchGridviewDetails()
        {
            List<clsSwitchDetails> ListSwitchDetails = (List<clsSwitchDetails>)Session["SwitchDetails"];
            float fTotalAllocation = 0f;
            foreach (GridViewRow row in this.gvSwitchDetails.Rows)
            {
                HiddenField hfSwitchDetailsID = (HiddenField)row.FindControl("hfSwitchDetailsID");
                HiddenField hfSelectedFundID = (HiddenField)row.FindControl("hfSelectedFundID");
                HiddenField hfCurrentUnit = (HiddenField)row.FindControl("hfUnits");
                HiddenField hfCurrentValue = (HiddenField)row.FindControl("hfValue");
                TextBox txtAllocation = (TextBox)row.FindControl("gvSwitchTxtTargetAllocation");

                foreach (clsSwitchDetails SwitchDetails in ListSwitchDetails)
                {
                    if (hfSelectedFundID.Value.Trim() == SwitchDetails.propFund.propFundID.ToString())
                    {
                        SwitchDetails.propAllocation = txtAllocation.Text.ToString() == string.Empty ? 0f : float.Parse(txtAllocation.Text.ToString());
                        SwitchDetails.propValue = hfCurrentValue.Value.ToString() == string.Empty ? 0f : float.Parse(hfCurrentValue.Value.ToString());
                        SwitchDetails.propUnits = hfCurrentUnit.Value.ToString() == string.Empty ? 0 : decimal.Parse(hfCurrentUnit.Value.ToString());
                        fTotalAllocation = fTotalAllocation + SwitchDetails.propAllocation;
                        SwitchDetails.propTotalAllocation = fTotalAllocation;
                    }
                    Session["SwitchDetails"] = ListSwitchDetails;
                }
            }
        }
        #endregion

        private void saveSwitch(clsSwitch.enumSwitchStatus SwitchStatus, Nullable<int> intSwitchID, string strDescription)
        {
            List<clsSwitchDetails> newListSwitchDetails = (List<clsSwitchDetails>)Session["SwitchDetails"];
            int intNewSwitchID = clsSwitch.insertSwitchHeader(PortfolioID(), ClientID(), UserID(), SwitchStatus, intSwitchID, strDescription);
            clsSwitchDetails.insertSwitchDetails(newListSwitchDetails, UserID(), intNewSwitchID);
        }

        private void setSessionCodepage()
        {
            if (Session[clsSystem_Session.strSession.lang.ToString()].ToString() == "2")
            {
                Session.CodePage = 950;
            }
            else if (Session[clsSystem_Session.strSession.lang.ToString()].ToString() == "3")
            {
                Session.CodePage = 932;
            }
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
        private void canEditGridView(bool CanEdit)
        {
            if (CanEdit)
            {
                foreach (GridViewRow row in gvSwitchDetails.Rows)
                {
                    TextBox gvSwitchTxtTargetAllocation = (TextBox)row.FindControl("gvSwitchTxtTargetAllocation");
                    gvSwitchTxtTargetAllocation.Enabled = true;
                    ImageButton ibtnEditFund = (ImageButton)row.FindControl("ibtnEditFund");
                    ibtnEditFund.Visible = Portfolio().propSwitch.propSwitchDetails[row.RowIndex].propIsDeletable;
                    HtmlAnchor lbtnFundName = (HtmlAnchor)row.FindControl("lbtnFundName");
                    lbtnFundName.Disabled = false;
                    lbtnFundName.Attributes.Add("href", String.Format("javascript:popUp_EditFunds({0})", Portfolio().propSwitch.propSwitchDetails[row.RowIndex].propFund.propFundID));
                }
                HtmlAnchor lbtnAddFund = (HtmlAnchor)this.gvSwitchDetails.FooterRow.Cells[1].FindControl("lbtnAddFund");
                lbtnAddFund.Visible = true;
                HtmlImage imgAddFund = (HtmlImage)this.gvSwitchDetails.FooterRow.Cells[1].FindControl("imgAddFund");
                imgAddFund.Visible = true;
            }
            else
            {
                foreach (GridViewRow row in gvSwitchDetails.Rows)
                {
                    TextBox gvSwitchTxtTargetAllocation = (TextBox)row.FindControl("gvSwitchTxtTargetAllocation");
                    gvSwitchTxtTargetAllocation.Enabled = false;
                    ImageButton ibtnEditFund = (ImageButton)row.FindControl("ibtnEditFund");
                    ibtnEditFund.Visible = false;
                    HtmlAnchor lbtnFundName = (HtmlAnchor)row.FindControl("lbtnFundName");
                    lbtnFundName.Disabled = true;
                    lbtnFundName.HRef = "";
                }
                HtmlAnchor lbtnAddFund = (HtmlAnchor)this.gvSwitchDetails.FooterRow.Cells[1].FindControl("lbtnAddFund");
                lbtnAddFund.Visible = false;
                HtmlImage imgAddFund = (HtmlImage)this.gvSwitchDetails.FooterRow.Cells[1].FindControl("imgAddFund");
                imgAddFund.Visible = false;
            }
        }
        private void pageStatus(int intStatus)
        {
            switch (intStatus)
            {
                case (int)clsSwitch.enumSwitchStatus.Proposed:
                    //disable Buttons[Save, Switch]
                    this.btnSave.Disabled = true;
                    this.btnSwitch.Disabled = true;
                    canEditGridView(false);
                    break;
                case (int)clsSwitch.enumSwitchStatus.Amended:
                    //hide Buttons[Save, Switch, Cancel] for Switch Function
                    this.divSwitch.Visible = false;

                    //hide Buttons[Amend, Resubmit, Cancel] for Request for Discussion Function
                    this.divRequestforDiscussion.Visible = false;
                    
                    //
                    //this.lblSwitchStatusTitle.Visible = false;
                    //this.lblSwitchStatusValue.Visible = false;
                    //show
                    this.divAmend.Visible = true;
                    this.lblTitle_ProposedSwitch.Visible = true;
                    this.lblSwitchClientDescription.Visible = true;
                    //this.table1_container.Visible = true;
                    this.gvSwitchDetailsClient.Visible = true;

                    clsSwitch_Client amendClientSwitch = new clsSwitch_Client(Portfolio().propSwitch.propSwitchID);
                    populateSwitchDetailsClient(amendClientSwitch);
                    canEditGridView(false);
                    break;
                case (int)clsSwitch.enumSwitchStatus.Request_ForDiscussion:
                    //
                    this.gvSwitchDetails.Visible = false;
                    //hide Buttons[Save, Switch, Cancel] for Switch Function
                    this.divSwitch.Visible = false;

                    //hide Buttons[Approve, Reject] for Amend Function
                    this.divAmend.Visible = false;
                    
                    //
                    this.lblSwitchStatusTitle.Visible = false;
                    this.lblSwitchStatusValue.Visible = false;

                    //show
                    this.lblTitle_ProposedSwitch.Visible = true;
                    this.divRequestforDiscussion.Visible = true;
                    this.lblSwitchClientDescription.Visible = true;
                    //this.table1_container.Visible = true;
                    this.gvSwitchDetailsClient.Visible = true;

                    clsSwitch_Client requestClientSwitch = new clsSwitch_Client(Portfolio().propSwitch.propSwitchID);
                    populateSwitchDetailsClient(requestClientSwitch);
                    canEditGridView(false);
                    break;
                case (int)clsSwitch.enumSwitchStatus.Approved:
                    this.btnSave.Disabled = true;
                    this.btnSwitch.Disabled = true;
                    this.btnCancel.Enabled = false;
                    canEditGridView(false);
                    this.btnComplete.Visible = true;
                    break;
                case (int)clsSwitch.enumSwitchStatus.Locked:
                    //disable Buttons[Save, Switch, Cancel]
                    this.btnSave.Disabled = true;
                    this.btnSwitch.Disabled = true;
                    this.btnCancel.Enabled = false;
                    canEditGridView(false);
                    break;
                case (int)clsSwitch.enumSwitchStatus.Declined_Client:
                    this.btnSave.Disabled = false;
                    this.btnSwitch.Disabled = false;
                    this.btnCancel.Enabled = true;
                    canEditGridView(true);
                    break;
                default:
                    break;
            }
        }
        private enum enumButton
        {
            Approve,
            Resubmit
        }

        #region SMS / SWITCH
        private void doSwitchSendSave(string strButtonClick)
        {
            captureSwitchGridviewDetails();
            
            string strPortfolioName = Portfolio().propCompany;
            int intDiscretionary = int.Parse(Session["HasDiscretionary"].ToString());

            clsSMS.subclsSMSTemplate osubclsSMSTemplate = new clsSMS.subclsSMSTemplate(clsSMS.subclsSMSTemplate.enumSMSTemplateID.ProposeSwitch);
            string strReplacerVariable = clsSMS.subclsSMSTemplate.strPortfolioNameVariable;
            string strMessage = osubclsSMSTemplate.propMessage.Replace(strReplacerVariable, strPortfolioName);
            string strPopupMessage = "Message sent.";
            string strSMSMobileNo = txtSMSMobileNo.Text.Trim();

            if (strButtonClick == enumButton.Approve.ToString())
            {
                List<clsSwitchDetails_Client> oSwitchDetailsClient = (List<clsSwitchDetails_Client>)Session["SwitchDetailsClient"];
                int intSwitchID = oSwitchDetailsClient[0].propSwitchID;

                clsSwitchDetails.transferClientSwitchToIFA(oSwitchDetailsClient, UserID());
                clsSwitch_Client.updateSwitchHeader(clsSwitch.enumSwitchStatus.Proposed, intSwitchID, string.Empty);
                clsSwitch.updateSwitchHeader(intSwitchID, clsSwitch.enumSwitchStatus.Proposed, txtDescription.Text);

                int intHistoryID = clsHistory.insertHeader(new clsSwitch(intSwitchID).propPortfolioID, intSwitchID, (Int16)clsSwitch.enumSwitchStatus.Proposed);
                clsSwitch oSwitch = new clsSwitch(Portfolio(), UserID());
                clsHistory.insertDetailsIFA(intHistoryID, oSwitch.propSwitchDetails);
                clsHistory.insertMessage(intHistoryID, txtDescription.Text);

                if (strSMSMobileNo.Length != 0)
                {
                    sendSMS(UserID(), strMessage, strPopupMessage, strSMSMobileNo);
                }
                string backPageURL = "https://" + Request.ServerVariables["SERVER_NAME"] + ":" + Request.ServerVariables["SERVER_PORT"] + "/report/" + "portfoliodetails.asp";
                ClientScript.RegisterStartupScript(this.GetType(), "alertSaveSuccess", "alert('This Amended Switch has been approved.'); window.location='" + backPageURL + "';", true);
            }
            else if (strButtonClick == enumButton.Resubmit.ToString())
            {
                int intSwitchID = Portfolio().propSwitch.propSwitchID;
                saveSwitch(clsSwitch.enumSwitchStatus.Proposed, intSwitchID, txtDescription.Text);
                clsSwitch_Client.updateSwitchHeader(clsSwitch.enumSwitchStatus.Proposed, intSwitchID, string.Empty);

                int intHistoryID = clsHistory.insertHeader(new clsSwitch(intSwitchID).propPortfolioID, intSwitchID, (Int16)clsSwitch.enumSwitchStatus.Proposed);
                clsSwitch oSwitch = new clsSwitch(Portfolio(), UserID());
                clsHistory.insertDetailsIFA(intHistoryID, oSwitch.propSwitchDetails);
                clsHistory.insertMessage(intHistoryID, txtDescription.Text);

                if (strSMSMobileNo.Length != 0)
                {
                    sendSMS(UserID(), strMessage, strPopupMessage, strSMSMobileNo);
                }
            }
            else
            {
                List<clsSwitchDetails> oSwitchDetails = (List<clsSwitchDetails>)Session["SwitchDetails"];
                int intSwitchID = oSwitchDetails[0].propSwitchID;
                if (intSwitchID == 0)
                {
                    if (intDiscretionary == 1 || intDiscretionary == -1)
                    {
                        saveSwitch(clsSwitch.enumSwitchStatus.Approved, null, txtDescription.Text.Trim());
                        clsSwitch oSwitch = new clsSwitch(Portfolio(), UserID());
                        int intHistoryID = clsHistory.insertHeader(PortfolioID(), intSwitchID, (Int16)clsSwitch.enumSwitchStatus.Approved);
                        NotifyApprovedSwtich(oSwitch);
                        clsHistory.insertDetailsIFA(intHistoryID, oSwitch.propSwitchDetails);
                        clsHistory.insertMessage(intHistoryID, txtDescription.Text.Trim());
                    }
                    else
                    {
                        saveSwitch(clsSwitch.enumSwitchStatus.Proposed, null, txtDescription.Text.Trim());
                        clsSwitch oSwitch = new clsSwitch(Portfolio(), UserID());
                        int intHistoryID = clsHistory.insertHeader(PortfolioID(), oSwitch.propSwitchID, (Int16)clsSwitch.enumSwitchStatus.Proposed);
                        clsHistory.insertDetailsIFA(intHistoryID, oSwitch.propSwitchDetails);
                        clsHistory.insertMessage(intHistoryID, txtDescription.Text.Trim());
                    }
                }
                else
                {
                    if (intDiscretionary == 1 || intDiscretionary == -1)
                    {
                        saveSwitch(clsSwitch.enumSwitchStatus.Approved, intSwitchID, txtDescription.Text.Trim());
                        clsSwitch oSwitch = new clsSwitch(Portfolio(), UserID());
                        int intHistoryID = clsHistory.insertHeader(PortfolioID(), intSwitchID, (Int16)clsSwitch.enumSwitchStatus.Approved);
                        NotifyApprovedSwtich(oSwitch);
                        clsHistory.insertDetailsIFA(intHistoryID, oSwitch.propSwitchDetails);
                        clsHistory.insertMessage(intHistoryID, txtDescription.Text);
                    }
                    else
                    {                        
                        saveSwitch(clsSwitch.enumSwitchStatus.Proposed, intSwitchID, txtDescription.Text.Trim());
                        clsSwitch oSwitch = new clsSwitch(Portfolio(), UserID());
                        int intHistoryID = clsHistory.insertHeader(PortfolioID(), intSwitchID, (Int16)clsSwitch.enumSwitchStatus.Proposed);
                        clsHistory.insertDetailsIFA(intHistoryID, oSwitch.propSwitchDetails);
                        clsHistory.insertMessage(intHistoryID, txtDescription.Text);
                    }
                }
                if (strSMSMobileNo.Length != 0)
                {
                    sendSMS(UserID(), strMessage, strPopupMessage, strSMSMobileNo);
                }
            }
            populateSwitchDetails(Portfolio().propSwitch.propSwitchDetails);
            pageStatus(Portfolio().propSwitch.propStatus);
        }
        //private string UpdateStatus(int intDiscretionary, int intSwitchID)
        //{
        //    if (intDiscretionary == 1 || intDiscretionary == -1)
        //    {
        //        clsSwitch.updateSwitchHeader(intSwitchID, clsSwitch.enumSwitchStatus.Approved, txtDescription.Text);
        //        return clsSwitch.enumSwitchStatus.Approved.ToString();
        //    }
        //    else
        //    {
        //        clsSwitch.updateSwitchHeader(intSwitchID, clsSwitch.enumSwitchStatus.Proposed, txtDescription.Text);
        //        return clsSwitch.enumSwitchStatus.Proposed.ToString();
        //    }
        //}
        private void sendSMS(string strUserID, string strSMSMessage, string strPopupMessage, string strSMSMobileNo)
        {
            clsSMS SMS = new clsSMS(strUserID);
            SMS.sendMessage(strSMSMobileNo.Trim(), strSMSMessage);

            if (!SMS.propErrorMsg.Equals(""))
            {
                ClientScript.RegisterStartupScript(this.GetType(), "alertMsgFailed", "alert('Sending Failed  " + SMS.propErrorMsg.Replace("'", " ") + "');", true);
            }

            if (!SMS.propReturnID.Equals(""))
            {
                ClientScript.RegisterStartupScript(this.GetType(), "alertMsgSent", "alert('" + strPopupMessage + "');", true);
            }
        }
        #endregion

        #region Status - Request for Discussion

        protected void btnAmend_Click(object sender, EventArgs e)
        {
            this.divRequestforDiscussion.Visible = false;
            this.divSwitch.Visible = true;

            this.lblSwitchStatusTitle.Visible = true;
            this.lblSwitchStatusValue.Visible = true;
            this.gvSwitchDetails.Visible = true;
            canEditGridView(true);
        }
        protected void btnResubmit_Click(object sender, EventArgs e)
        {
            Button ButtonClick = (Button)sender;
            ViewState["Button"] = ButtonClick.Text;
            mdlThirdSwitchPopup.Show();
        }
        #endregion

        #region Status - Amend

        protected void btnReject_Click(object sender, EventArgs e)
        {
            this.lblSwitchStatusTitle.Visible = true;
            this.lblSwitchStatusValue.Visible = true;
            gvSwitchDetails.Visible = true;
            divSwitch.Visible = true;
            divAmend.Visible = false;
            
            Button ButtonClick = (Button)sender;
            ViewState["Button"] = ButtonClick.Text;

            clsSwitch_Client.updateSwitchHeader(clsSwitch.enumSwitchStatus.Declined_IFA, Portfolio().propSwitch.propSwitchID, string.Empty);
            canEditGridView(true);
        }
        protected void btnApprove_Click(object sender, EventArgs e)
        {
            Button ButtonClick = (Button)sender;
            ViewState["Button"] = ButtonClick.Text;
            mdlThirdSwitchPopup.Show();
        }
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
                Label gvSwitchFooterLblTotalValue = (Label)this.gvSwitchDetails.FooterRow.Cells[3].FindControl("gvSwitchFooterLblTotalValue");

                this.mpeFundSearch.Show();
            }
        }

        protected void btnCloseFundSearch_Click(object sender, EventArgs e)
        {
            clearPopupFundFields();
            this.mpeFundSearch.Hide();
        }

        protected void txtPopupFund_TextChanged(object sender, EventArgs e)
        {
            btnPopupFund_Click(sender, e);
        }

        protected void gvFunds_lbtnFundName_Click(object sender, EventArgs e)
        {
            //Response.Write("RUEL---------");
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

        #region "Client-side handlers"

        private void AddClientHandlers()
        {
            string jsFunction = "getFlickerSolved";
            string jsConditionFortxtPopup = "PopupValidation('" + txtPopupFund.ClientID + "','" + PopupFundSearch.ClientID + "');";

            txtPopupFund.Attributes["autocomplete"] = "off";
            txtPopupFund.Attributes.Remove("onkeypress");
            txtPopupFund.Attributes.Remove("onchange");
            mpeFundSearch.OnCancelScript = jsFunction + "('" + PopupFundSearch.ClientID + "');";
            btnCloseFundSearch.OnClientClick += jsFunction + "('" + PopupFundSearch.ClientID + "');";
            btnPopupFund.OnClientClick += jsConditionFortxtPopup;

            btnSwitchProceed.Attributes["onClick"] += jsFunction + "('" + FirstSwitchPopup.ClientID + "');";
            btnSwitchCancel.Attributes["onClick"] += jsFunction + "('" + FirstSwitchPopup.ClientID + "');";
            FirstSwitchPopup.Attributes["onCancelScript"] += jsFunction + "('" + FirstSwitchPopup.ClientID + "');";

            btnSwitchNo.Attributes["onClick"] += jsFunction + "('" + SecondSwitchPopup.ClientID + "');";
            btnSwitchYes.Attributes["onClick"] += jsFunction + "('" + SecondSwitchPopup.ClientID + "');";
            SecondSwitchPopup.Attributes["onCancelScript"] += jsFunction + "('" + SecondSwitchPopup.ClientID + "');";

            btnSwitchSendSave.Attributes["onClick"] += jsFunction + "('" + ThirdSwitchPopup.ClientID + "');";
            btnSwitchSendCancel.Attributes["onClick"] += jsFunction + "('" + ThirdSwitchPopup.ClientID + "');";
            ThirdSwitchPopup.Attributes["onCancelScript"] += jsFunction + "('" + ThirdSwitchPopup.ClientID + "');";
        }

        #endregion

        protected void NotifyApprovedSwtich(clsSwitch Switch)
        {
            try
            {
                int intIFAID = int.Parse(Session["ifaid"].ToString());
                clsIFA IFA = new clsIFA(intIFAID);

                string htmlTemplate = clsOutput.generateApprovedSwitch(Switch, IFA.propIFA_ID);
                StyleSheet style = clsOutput.getStyleSheet_ApprovedSwitch();
                string strFilename = clsOutput.generateOutputFile(clsOutput.enumOutputType.PDF, htmlTemplate, style, Switch.propSwitchID, clsOutput.enumSwitchType.Portfolio);

                string strRecepient = IFA.propIFAEmail;
                string strSender = "NAVSwitch@NoReply.com";
                string strSubject = "Switch Instruction";
                string strBody = clsEmail.generateEmailBody((Switch.propPortfolio.propConfirmationRequired ? "NotifyApprovedEmailReqSign" : "NotifyApprovedEmail"), null, null, null, null, null, Switch.propPortfolio.propCompany, Switch.propSwitchID.ToString());
                clsEmail.SendWithAttachment(strRecepient, strSender, strSubject, strBody, Switch.propSwitchID, Switch.propClientID, clsEmail.enumEmailPurpose.ApproveSwitchNotification, strFilename);

                clsClient client = new clsClient(Switch.propClientID);
                string ClientName = client.propForename + " " + client.propSurname;
                if (!String.IsNullOrEmpty(client.propEmailWork))
                {
                    strRecepient = client.propEmailWork;
                    strBody = clsEmail.generateEmailBody((Switch.propPortfolio.propConfirmationRequired ? "NotifyClientApprovedEmailReqSign" : "NotifyClientApprovedEmail"), null, null, ClientName, null, null, Switch.propPortfolio.propCompany, null);
                    clsEmail.SendWithAttachment(strRecepient, strSender, strSubject, strBody, Switch.propSwitchID, Switch.propClientID, clsEmail.enumEmailPurpose.ApproveSwitchNotification, strFilename);
                }
                if (!String.IsNullOrEmpty(client.propEmailPersonal))
                {
                    strRecepient = client.propEmailPersonal;
                    strBody = clsEmail.generateEmailBody((Switch.propPortfolio.propConfirmationRequired ? "NotifyClientApprovedEmailReqSign" : "NotifyClientApprovedEmail"), null, null, ClientName, null, null, Switch.propPortfolio.propCompany, null);
                    clsEmail.SendWithAttachment(strRecepient, strSender, strSubject, strBody, Switch.propSwitchID, Switch.propClientID, clsEmail.enumEmailPurpose.ApproveSwitchNotification, strFilename);
                }

                //:SwitchType_IFA Name_Client Name_yyyy-mm-dd.pdf
            }
            catch (Exception ex)
            {
                Response.Write("Exception Occured<br/>");
                Response.Write(String.Format("<p>{0}</p><br/>", ex.ToString()));
            }
        }
    }

}

