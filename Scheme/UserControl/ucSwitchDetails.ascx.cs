using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

namespace NAV.Scheme.UserControl
{
    public partial class ucSwitchDetails : System.Web.UI.UserControl
    {
        #region "properties and enums"

        private String strBackPageURL;
        public String propBackPageURL { get { return strBackPageURL; } set { strBackPageURL = value; } }

        private enum enumButton
        {
            Approve,
            Resubmit
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
                
                Session["listSwitchDetails"] = new clsSwitchScheme(Scheme, strUserID).propSwitchDetails;

                Session["listSwitchContributionDetails"] = new clsSwitchScheme(Scheme, strUserID).propSwitchDetailsContribution;
                Session["HasDiscretionary"] = Scheme.propMFPercent;

                populate((List<clsSwitchScheme.clsSwitchSchemeDetails>)Session["listSwitchDetails"], false);
                populate((List<clsSwitchScheme.clsSwitchSchemeDetails>)Session["listSwitchContributionDetails"], true);

                pageStatus(new clsSwitchScheme(Scheme).propStatus);
            }
            else
            {
                if (Request["__EVENTARGUMENT"] == "SaveSwitchDetails") { btnSave_Click(); }
                else if (Request["__EVENTARGUMENT"] == "ChangeFund") { replaceFund(int.Parse(this.hfFundIDOrig.Value), int.Parse(this.hfFundIDNew.Value), Boolean.Parse(this.hfIsContribution.Value)); }
                else if (Request["__EVENTARGUMENT"] == "AddMoreFund") { addNewFund(int.Parse(this.hfFundIDNew.Value), Boolean.Parse(this.hfIsContribution.Value)); }
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            String strSchemeID = Session[clsSystem_Session.strSession.tempschemeid.ToString()].ToString();
            String strUserID = Session[clsSystem_Session.strSession.User.ToString()].ToString();
            String strClientID = Session[clsSystem_Session.strSession.clientID.ToString()].ToString();

            List<clsSwitchScheme.clsSwitchSchemeDetails> ListSwitchDetails = (List<clsSwitchScheme.clsSwitchSchemeDetails>)Session["listSwitchDetails"];
            clsSwitchScheme SchemeSwitch = new clsSwitchScheme(new clsScheme(strClientID, strSchemeID), strUserID);

            if ((SchemeSwitch.propStatus != (int)clsSwitch.enumSwitchStatus.Draft) && (SchemeSwitch.propStatus != (int)clsSwitch.enumSwitchStatus.Saved))
            {
                int intHistoryID = clsHistory.clsHistoryScheme.insertHeader(strSchemeID, SchemeSwitch.propSwitchID, (Int16)clsSwitch.enumSwitchStatus.Cancelled);
            }

            if (ListSwitchDetails[0].propSwitchScheme != null)
            {
                clsSwitchScheme.deleteSwitch(ListSwitchDetails[0].propSwitchScheme.propSwitchID);
            }

            Page.ClientScript.RegisterStartupScript(this.GetType(), "alertCancelledSwitch", "alert('This Switch has been cancelled'); window.location='" + this.propBackPageURL + "';", true);
        }

        private void btnSave_Click()
        {
            List<clsSwitchScheme.clsSwitchSchemeDetails> ListSwitchDetails = (List<clsSwitchScheme.clsSwitchSchemeDetails>)Session["listSwitchDetails"];

            String strSchemeID = Session[clsSystem_Session.strSession.tempschemeid.ToString()].ToString();
            String strUserID = Session[clsSystem_Session.strSession.User.ToString()].ToString();
            String strClientID = Session[clsSystem_Session.strSession.clientID.ToString()].ToString();

            captureSwitchGridviewDetails();

            clsScheme Scheme = new clsScheme(strClientID, strSchemeID);

            if (ListSwitchDetails[0].propSwitchScheme != null)
            {
                int intSwitchID = ListSwitchDetails[0].propSwitchScheme.propSwitchID;
                saveSwitch(clsSwitch.enumSwitchStatus.Saved, intSwitchID, string.Empty, strUserID, Scheme);
            }
            else
            {
                saveSwitch(clsSwitch.enumSwitchStatus.Saved, null, string.Empty, strUserID, Scheme);
            }

            populate(ListSwitchDetails,false);
            populate(ListSwitchDetails, true);

            Page.ClientScript.RegisterStartupScript(this.GetType(), "alertSaveSuccess", "alert('This Switch has been saved.'); window.location='" + this.propBackPageURL + "';", true);
        }

        protected void btnComplete_Click(object sender, EventArgs e)
        {
            String strSchemeID = Session[clsSystem_Session.strSession.tempschemeid.ToString()].ToString();
            String strUserID = Session[clsSystem_Session.strSession.User.ToString()].ToString();
            String strClientID = Session[clsSystem_Session.strSession.clientID.ToString()].ToString();

            clsScheme Scheme = new clsScheme(strClientID, strSchemeID);
            clsSwitchScheme SwitchScheme = new clsSwitchScheme(Scheme);

            int intSwitchID = SwitchScheme.propSwitchID;

            clsSwitchScheme.updateSwitchHeader(intSwitchID, clsSwitch.enumSwitchStatus.Completed);
            clsHistory.clsHistoryScheme.insertHeader(strSchemeID, intSwitchID, (short)clsSwitch.enumSwitchStatus.Completed);
            
            Page.ClientScript.RegisterStartupScript(this.GetType(), "alertCompletedSwitch", "alert('This Switch has been finally completed.'); window.location='" + this.propBackPageURL + "';", true);
        }


        #region Status - Request for Discussion

        protected void btnAmend_Click(object sender, EventArgs e)
        {
            this.divRequestforDiscussion.Visible = false;
            this.divSwitch.Visible = true;

            this.lblSwitchStatusTitle.Visible = true;
            this.lblSwitchStatusValue.Visible = true;
            isShowEditablePanel(true);
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
            String strSchemeID = Session[clsSystem_Session.strSession.tempschemeid.ToString()].ToString();
            String strUserID = Session[clsSystem_Session.strSession.User.ToString()].ToString();
            String strClientID = Session[clsSystem_Session.strSession.clientID.ToString()].ToString();

            clsScheme Scheme = new clsScheme(strClientID, strSchemeID);
            clsSwitchScheme SwitchScheme = new clsSwitchScheme(Scheme);

            int intSwitchID = SwitchScheme.propSwitchID;

            this.lblSwitchStatusTitle.Visible = true;
            this.lblSwitchStatusValue.Visible = true;
            //gvSwitchDetails.Visible = true;
            divSwitch.Visible = true;
            divAmend.Visible = false;

            Button ButtonClick = (Button)sender;
            ViewState["Button"] = ButtonClick.Text;
            
            clsSwitchScheme_Client.updateSwitchHeader(clsSwitch.enumSwitchStatus.Declined_IFA, intSwitchID, new clsSwitchScheme_Client(intSwitchID).propDescription);
            isShowEditablePanel(true);
        }

        protected void btnApprove_Click(object sender, EventArgs e)
        {
            Button ButtonClick = (Button)sender;
            ViewState["Button"] = ButtonClick.Text;
            mdlThirdSwitchPopup.Show();
        }

        #endregion

        #endregion

        #region "Session SwitchDetails"

        private void populate(List<clsSwitchScheme.clsSwitchSchemeDetails> listSwitchDetails, Boolean isContribution)
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
                        this.btnSwitch.Disabled = true;
                    }
                }
                else
                {
                    if (float.Parse(gvSwitchFooterHfTotalAllocation.Value) > 100)
                    {
                        this.btnSave.Disabled = true;
                        this.btnSwitch.Disabled = true;
                    }

                    this.hfCurrentValueClientTotal_Contribution.Value = listSwitchDetails[0].propTotalValue.ToString("n0");
                    this.hfFundCount_Contribution.Value = listSwitchDetails.Count.ToString();

                }

            }
        }

        public void captureSwitchGridviewDetails()
        {

            List<clsSwitchScheme.clsSwitchSchemeDetails> ListSwitchDetails = (List<clsSwitchScheme.clsSwitchSchemeDetails>)Session["listSwitchDetails"];
            float dblTotalAllocation = 0;

            foreach (GridViewRow row in this.gvSchemeSwitch.Rows)
            {
                HiddenField hfSwitchDetailsID = (HiddenField)row.FindControl("hfSwitchDetailsID");
                HiddenField hfSelectedFundID = (HiddenField)row.FindControl("hfSelectedFundID");
                TextBox txtAllocation = (TextBox)row.FindControl("gvSwitchTxtTargetAllocation");
                HiddenField gvhfSwitchHeaderValue = (HiddenField)row.FindControl("gvhfSwitchHeaderValue");
                HiddenField gvhfSwitchDetailsLblUnits = (HiddenField)row.FindControl("gvhfSwitchDetailsLblUnits");

                foreach (clsSwitchScheme.clsSwitchSchemeDetails SwitchDetails in ListSwitchDetails)
                {
                    if (hfSelectedFundID.Value.Trim() == SwitchDetails.propFund.propFundID.ToString())
                    {                        
                        SwitchDetails.propAllocation = txtAllocation.Text.ToString() == "" ? 0f : float.Parse(txtAllocation.Text.ToString());
                        SwitchDetails.propValue = float.Parse(gvhfSwitchHeaderValue.Value.ToString());
                        SwitchDetails.propUnits = decimal.Parse(gvhfSwitchDetailsLblUnits.Value.ToString());
                        

                        dblTotalAllocation = dblTotalAllocation + SwitchDetails.propAllocation;
                        SwitchDetails.propTotalAllocation = dblTotalAllocation;
                    }

                    Session["listSwitchDetails"] = ListSwitchDetails;
                }
            }

            List<clsSwitchScheme.clsSwitchSchemeDetails> ListSwitchDetailsContribution = (List<clsSwitchScheme.clsSwitchSchemeDetails>)Session["listSwitchContributionDetails"];
            dblTotalAllocation = 0;

            foreach (GridViewRow row in this.gvContributionSwitch.Rows)
            {
                HiddenField hfSwitchDetailsID = (HiddenField)row.FindControl("hfSwitchDetailsID");
                HiddenField hfSelectedFundID = (HiddenField)row.FindControl("hfSelectedFundID");
                TextBox txtAllocation = (TextBox)row.FindControl("gvSwitchTxtTargetAllocation");
                HiddenField gvhfSwitchHeaderValue = (HiddenField)row.FindControl("gvhfSwitchHeaderValue");
                HiddenField gvhfSwitchDetailsLblUnits = (HiddenField)row.FindControl("gvhfSwitchDetailsLblUnits");

                foreach (clsSwitchScheme.clsSwitchSchemeDetails SwitchDetails in ListSwitchDetailsContribution)
                {
                    if (hfSelectedFundID.Value.Trim() == SwitchDetails.propFund.propFundID.ToString())
                    {
                        SwitchDetails.propAllocation = txtAllocation.Text.ToString() == "" ? 0f : float.Parse(txtAllocation.Text.ToString());
                        SwitchDetails.propValue = float.Parse(gvhfSwitchHeaderValue.Value.ToString());
                        SwitchDetails.propUnits = decimal.Parse(gvhfSwitchDetailsLblUnits.Value.ToString());
                        
                        dblTotalAllocation = dblTotalAllocation + SwitchDetails.propAllocation;
                        SwitchDetails.propTotalAllocation = dblTotalAllocation;
                    }

                    Session["listSwitchContributionDetails"] = ListSwitchDetailsContribution;
                }
            }
        }

        private void addNewFund(int intNewFundID, Boolean isContribution)
        {

            captureSwitchGridviewDetails();

            string strSessionDetailsSet = isContribution ? "listSwitchContributionDetails" : "listSwitchDetails";

            List<clsSwitchScheme.clsSwitchSchemeDetails> currentListSwitchDetails = (List<clsSwitchScheme.clsSwitchSchemeDetails>)Session[strSessionDetailsSet];
            String strUserID = Session[clsSystem_Session.strSession.User.ToString()].ToString();
            clsClient Client = new clsClient(Session[clsSystem_Session.strSession.clientID.ToString()].ToString());

            List<clsSwitchScheme.clsSwitchSchemeDetails> newListSwitchDetails = new List<clsSwitchScheme.clsSwitchSchemeDetails>();

            try
            {
                newListSwitchDetails = clsSwitchScheme.clsSwitchSchemeDetails.addFund(intNewFundID, currentListSwitchDetails, Client.propClientID, Client.propCurrency, strUserID, false);

                Session[strSessionDetailsSet] = newListSwitchDetails;

                populate(newListSwitchDetails, isContribution);

            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "errDuplicateDund", "alert('" + ex.Message + "');", true);
                populate((List<clsSwitchScheme.clsSwitchSchemeDetails>)Session[strSessionDetailsSet], isContribution);
                return;
            }        
        }

        private void replaceFund(int intOldFundID, int intNewFundID, Boolean isContribution)
        {

            captureSwitchGridviewDetails();

            string strSessionDetailsSet = isContribution ? "listSwitchContributionDetails" : "listSwitchDetails";

            List<clsSwitchScheme.clsSwitchSchemeDetails> PreviousListSwitchDetails = (List<clsSwitchScheme.clsSwitchSchemeDetails>)Session[strSessionDetailsSet];

            String strUserID = Session[clsSystem_Session.strSession.User.ToString()].ToString();
            String strClientID = Session[clsSystem_Session.strSession.clientID.ToString()].ToString();
            String strSchemeID = Session[clsSystem_Session.strSession.tempschemeid.ToString()].ToString();

            clsScheme Scheme = new clsScheme(strClientID, strSchemeID);

            List<clsSwitchScheme.clsSwitchSchemeDetails> newListSwitchDetails = new List<clsSwitchScheme.clsSwitchSchemeDetails>();

            try
            {
                newListSwitchDetails = clsSwitchScheme.clsSwitchSchemeDetails.FundChange(intOldFundID, intNewFundID, PreviousListSwitchDetails, strClientID, Scheme.propClient.propCurrency, false);

                Session[strSessionDetailsSet] = newListSwitchDetails;

                populate(newListSwitchDetails, isContribution);
            }
            catch (Exception ex)
            {
                populate((List<clsSwitchScheme.clsSwitchSchemeDetails>)Session["strSessionDetailsSet"], isContribution);
                Page.ClientScript.RegisterStartupScript(this.GetType(), "errDuplicateFund", "alert('" + ex.Message + "');", true);
                return;
            }
        }

        protected void lbtnRemoveFund_Click(object sender, CommandEventArgs e)
        {
            captureSwitchGridviewDetails();

            Boolean isContribution = Boolean.Parse(this.hfIsContribution.Value);
            string strSessionDetailsSet = isContribution ? "listSwitchContributionDetails" : "listSwitchDetails";            

            List<clsSwitchScheme.clsSwitchSchemeDetails> PreviousListSwitchDetails = (List<clsSwitchScheme.clsSwitchSchemeDetails>)Session[strSessionDetailsSet];

            String strClientID = Session[clsSystem_Session.strSession.clientID.ToString()].ToString();
            String strSchemeID = Session[clsSystem_Session.strSession.tempschemeid.ToString()].ToString();
            String strUserID = Session[clsSystem_Session.strSession.User.ToString()].ToString();

            Session[strSessionDetailsSet] = clsSwitchScheme.clsSwitchSchemeDetails.removeSwitchDetails(int.Parse(e.CommandArgument.ToString()), PreviousListSwitchDetails);
            populate((List<clsSwitchScheme.clsSwitchSchemeDetails>)Session[strSessionDetailsSet], isContribution);
        }

        protected void btnMirrorFundAllocation_Click(object sender, EventArgs e)
        {            
            captureSwitchGridviewDetails();

            List<clsSwitchScheme.clsSwitchSchemeDetails> listSwitchDetails = (List<clsSwitchScheme.clsSwitchSchemeDetails>)Session["listSwitchDetails"];
            List<clsSwitchScheme.clsSwitchSchemeDetails> newListSwitchDetails = new List<clsSwitchScheme.clsSwitchSchemeDetails>();

            foreach (clsSwitchScheme.clsSwitchSchemeDetails SwitchDetails in listSwitchDetails)
            {
                clsSwitchScheme.clsSwitchSchemeDetails newSwitchDetailsContribution = new clsSwitchScheme.clsSwitchSchemeDetails();

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

            Session["listSwitchContributionDetails"] = newListSwitchDetails;

        }

        #endregion

        #region "popUps"

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
            populate((List<clsSwitchScheme.clsSwitchSchemeDetails>)Session["listSwitchDetails"], false);
            populate((List<clsSwitchScheme.clsSwitchSchemeDetails>)Session["listSwitchContributionDetails"], true);            
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

        #region "Switch Series of popUps"

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

        #endregion

        #endregion

        #region "misc"

        public void FixAjaxValidationSummaryErrorLabel_PreRender(object sender, EventArgs e)
        {

            string objId = ((Label)sender).ID;

            ScriptManager.RegisterStartupScript(this, this.GetType(), objId, ";", true);

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

        private void pageStatus(int intStatus)
        {
            switch (intStatus)
            {
                case (int)clsSwitch.enumSwitchStatus.Proposed:

                    this.btnSave.Disabled = true;
                    this.btnSwitch.Disabled = true;

                    isShowEditablePanel(false);
                    break;
                case (int)clsSwitch.enumSwitchStatus.Amended:

                    this.divSwitch.Visible = false;
                    this.divRequestforDiscussion.Visible = false;
                    this.divAmend.Visible = true;

                    isShowEditablePanel(false);
                    break;
                case (int)clsSwitch.enumSwitchStatus.Request_ForDiscussion:

                    this.divSwitch.Visible = false;
                    this.divRequestforDiscussion.Visible = true;
                    this.divAmend.Visible = false;

                    isShowEditablePanel(false);
                    break;
                case (int)clsSwitch.enumSwitchStatus.Approved:
                    this.btnSave.Disabled = true;
                    this.btnSwitch.Disabled = true;
                    this.btnCancel.Enabled = false;

                    this.btnComplete.Visible = true;
                    isShowEditablePanel(false);
                    
                    break;
                case (int)clsSwitch.enumSwitchStatus.Declined_IFA:
                    this.btnSave.Disabled = false;
                    this.btnSwitch.Disabled = false;
                    this.btnCancel.Enabled = true;
                    isShowEditablePanel(true);
                    break;
                case (int)clsSwitch.enumSwitchStatus.Locked:
                    this.btnSave.Disabled = true;
                    this.btnSwitch.Disabled = true;
                    this.btnCancel.Enabled = false;
                    isShowEditablePanel(false);
                    break;
                case (int)clsSwitch.enumSwitchStatus.Completed:
                    this.btnSave.Disabled = true;
                    this.btnSwitch.Disabled = true;
                    this.btnCancel.Enabled = false;
                    isShowEditablePanel(false);
                    break;
                default:
                    break;
            }
        }

        private void isShowEditablePanel(Boolean isShow) 
        {
            this.lblSwitchStatusTitle.Visible = isShow;
            this.lblSwitchStatusValue.Visible = isShow;

            this.lblTitle_ContributionAllocation.Visible = isShow;
            this.lblTitle_ProposePortfolio.Visible = isShow;

            this.btnMirrorFundAllocation.Visible = isShow;

            this.gvContributionSwitch.Visible = isShow;
            this.gvSchemeSwitch.Visible = isShow;
        }

        #endregion

        #region SMS / SWITCH

        private void saveSwitch(clsSwitch.enumSwitchStatus SwitchStatus, Nullable<int> intSwitchID, string strDescription, string strUserID, clsScheme Scheme)
        {
            List<clsSwitchScheme.clsSwitchSchemeDetails> newListSwitchDetails = (List<clsSwitchScheme.clsSwitchSchemeDetails>)Session["listSwitchDetails"];
            List<clsSwitchScheme.clsSwitchSchemeDetails> newListSwitchDetailsContribution = (List<clsSwitchScheme.clsSwitchSchemeDetails>)Session["listSwitchContributionDetails"];

            clsSwitchScheme.clsSwitchSchemeDetails.deleteAllDetails(intSwitchID);

            int intNewSwitchID = clsSwitchScheme.insertSwitchHeader(Scheme, strUserID, SwitchStatus, intSwitchID, strDescription);
            clsSwitchScheme.clsSwitchSchemeDetails.insertSwitchDetails(newListSwitchDetails, strUserID, intNewSwitchID, false);
            clsSwitchScheme.clsSwitchSchemeDetails.insertSwitchDetails(newListSwitchDetailsContribution, strUserID, intNewSwitchID, true);
        }

        private void doSwitchSendSave(string strButtonClick)
        {
                
            String strSchemeID = Session[clsSystem_Session.strSession.tempschemeid.ToString()].ToString();                
            String strUserID = Session[clsSystem_Session.strSession.User.ToString()].ToString();                
            String strClientID = Session[clsSystem_Session.strSession.clientID.ToString()].ToString();

            captureSwitchGridviewDetails();
            List<clsSwitchScheme.clsSwitchSchemeDetails> SwitchDetails = (List<clsSwitchScheme.clsSwitchSchemeDetails>)Session["listSwitchDetails"];

            clsScheme Scheme = new clsScheme(strClientID, strSchemeID);           
            string strSchemeName = Scheme.propCompany.propCompany;
            int intDiscretionary = int.Parse(Session["HasDiscretionary"].ToString());
           
            int intSwitchID = SwitchDetails[0].propSwitchScheme.propSwitchID;

            if (strButtonClick == enumButton.Approve.ToString())
            {
                clsSwitchScheme.clsSwitchSchemeDetails.deleteAllDetails(intSwitchID);
                clsSwitchScheme.clsSwitchSchemeDetails.transferClientSwitchToIFA(new clsSwitchScheme_Client(intSwitchID).propSwitchDetailsPortfolio, strUserID, false);
                clsSwitchScheme.clsSwitchSchemeDetails.transferClientSwitchToIFA(new clsSwitchScheme_Client(intSwitchID).propSwitchDetailsContribution, strUserID, true);

                clsSwitchScheme_Client.updateSwitchHeader(clsSwitch.enumSwitchStatus.Approved, intSwitchID, string.Empty);
                clsSwitchScheme.updateSwitchHeader(intSwitchID, clsSwitch.enumSwitchStatus.Approved);

                int intHistoryID = clsHistory.clsHistoryScheme.insertHeader(strSchemeID, intSwitchID, (Int16)clsSwitch.enumSwitchStatus.Approved);
                clsSwitchScheme SwitchScheme = new clsSwitchScheme(intSwitchID);
                clsHistory.clsHistoryScheme.insertDetailsIFA(intHistoryID, SwitchScheme.propSwitchDetails, false);
                clsHistory.clsHistoryScheme.insertDetailsIFA(intHistoryID, SwitchScheme.propSwitchDetailsContribution, true);

                clsHistory.clsHistoryScheme.insertMessage(intHistoryID, txtDescription.Text);
            }
            else if (strButtonClick == enumButton.Resubmit.ToString())
            {
                saveSwitch(clsSwitch.enumSwitchStatus.Proposed, intSwitchID, txtDescription.Text, strUserID, Scheme);
                clsSwitchScheme_Client.updateSwitchHeader(clsSwitch.enumSwitchStatus.Proposed, intSwitchID, string.Empty);

                int intHistoryID = clsHistory.clsHistoryScheme.insertHeader(strSchemeID, intSwitchID, (Int16)clsSwitch.enumSwitchStatus.Proposed);
                clsSwitchScheme SwitchScheme = new clsSwitchScheme(intSwitchID);
                clsHistory.clsHistoryScheme.insertDetailsIFA(intHistoryID, SwitchScheme.propSwitchDetails, false);
                clsHistory.clsHistoryScheme.insertDetailsIFA(intHistoryID, SwitchScheme.propSwitchDetailsContribution, true);

                clsHistory.clsHistoryScheme.insertMessage(intHistoryID, txtDescription.Text);
            }
            else
            {
                                             
                clsSwitch.enumSwitchStatus status = (intDiscretionary == 1 || intDiscretionary == -1) ? clsSwitch.enumSwitchStatus.Approved : clsSwitch.enumSwitchStatus.Proposed;

                if (intSwitchID == 0)
                {
                    saveSwitch(status, null, txtDescription.Text.Trim(), strUserID, Scheme);
                }
                else
                {
                    saveSwitch(status, intSwitchID, txtDescription.Text.Trim(), strUserID, Scheme);
                }

                intSwitchID = new clsSwitchScheme(Scheme).propSwitchID;
                clsSwitchScheme SwitchScheme = new clsSwitchScheme(intSwitchID);

                int intHistoryID = clsHistory.clsHistoryScheme.insertHeader(strSchemeID, intSwitchID, (Int16)clsSwitch.enumSwitchStatus.Proposed);
                
                clsHistory.clsHistoryScheme.insertDetailsIFA(intHistoryID, SwitchScheme.propSwitchDetails, false);
                clsHistory.clsHistoryScheme.insertDetailsIFA(intHistoryID, SwitchScheme.propSwitchDetailsContribution, true);

                clsHistory.clsHistoryScheme.insertMessage(intHistoryID, txtDescription.Text);

            }            

            string strMessage = clsSMS.subclsSMSTemplate.getSMSTemplate(clsSMS.subclsSMSTemplate.enumSMSTemplateID.ProposeSchemeSwitch.ToString());
            strMessage = clsSMS.subclsSMSTemplate.convertSMSMessage(strMessage, null, null, null, null, Scheme);            

            string strPopupMessage = "Message sent.";
            string strSMSMobileNo = txtSMSMobileNo.Text.Trim();

            if (strSMSMobileNo.Length != 0)
            {
                sendSMS(strUserID, strMessage, strPopupMessage, strSMSMobileNo);
            }

            Page.ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + strPopupMessage + "'); window.location='" + this.propBackPageURL + "';", true);
        }

        private void sendSMS(string strUserID, string strSMSMessage, string strPopupMessage, string strSMSMobileNo)
        {
            clsSMS SMS = new clsSMS(strUserID);
            SMS.sendMessage(strSMSMobileNo.Trim(), strSMSMessage);

            if (!SMS.propErrorMsg.Equals(""))
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "alertMsgFailed", "alert('Sending Failed  " + SMS.propErrorMsg.Replace("'", " ") + "');", true);
            }

            if (!SMS.propReturnID.Equals(""))
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "alertMsgSent", "alert('" + strPopupMessage + "');", true);
            }
        }

        #endregion               

        #region "Client-side Handlers"

        public ucSwitchDetails()
            : base()
        {
            this.Init += this.Initialize;
        }

        public void Initialize(object sender, EventArgs e)
        {
            /* Added on January 11, 2012 */
            this.AddClientHandlers();
            /* End of addition January 11, 2012 */
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

    }

}