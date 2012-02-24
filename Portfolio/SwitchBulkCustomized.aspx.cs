using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NAV.Portfolio
{
    public partial class SwitchBulkCustomized : System.Web.UI.Page
    {
        #region Factors on Form
        string ClientID()
        {
            if (ViewState["CID"] != null) { return ViewState["CID"].ToString(); }
            else return string.Empty;
        }
        string PortfolioID()
        {
            if (ViewState["PID"] != null) { return ViewState["PID"].ToString(); }
            else return string.Empty;
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
        int IFA_ID()
        {
            if (Session[clsSystem_Session.strSession.ifaid.ToString()] != null)
            {
                return int.Parse(Session[clsSystem_Session.strSession.ifaid.ToString()].ToString());
            }
            else
            {
                return 0;
            }
        }
        clsPortfolio Portfolio()
        {
            return new clsPortfolio(ClientID(), PortfolioID(), UserID());
        }

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                ((NAV)this.Page.Master).FindControl("btnBack_Classic").Visible = false;
                ViewState["MID"] = Request.QueryString["MID"];
                ViewState["MGID"] = Request.QueryString["MGID"];
                ViewState["MPID"] = Request.QueryString["MPID"];
                ViewState["CID"] = Request.QueryString["CID"];
                ViewState["PID"] = Request.QueryString["PID"];
                
                clsPortfolio _clsPortfolio = new clsPortfolio(ClientID(), PortfolioID(), UserID());
                _clsPortfolio.propModelGroupID = ViewState["MGID"].ToString();
                _clsPortfolio.propModelPortfolioID = ViewState["MPID"].ToString();

                clsModelPortfolio _clsModelPortfolio = new clsModelPortfolio(_clsPortfolio, ViewState["MGID"].ToString(), ViewState["MPID"].ToString());
                clsSwitchTemp _clsSwitchTemp = new clsSwitchTemp();
                _clsSwitchTemp.propModelGroupID = _clsModelPortfolio.propModelGroupID;
                _clsSwitchTemp.propModelPortfolioID = _clsModelPortfolio.propModelPortfolioID;
                _clsPortfolio.propSwitchTemp = new clsSwitchTemp(_clsPortfolio, UserID(), IFA_ID(), _clsModelPortfolio.propModelID, ViewState["MGID"].ToString(), ViewState["MPID"].ToString());
                
                //Load the data
                populateHeader(_clsPortfolio);
                populateDetails(_clsPortfolio.propPortfolioDetails);
                populateSwitchDetails(_clsPortfolio.propSwitchTemp.propSwitchDetails, Portfolio());
            }
            else
            {
                if (Request["__EVENTARGUMENT"] == "SaveSwitchDetails") { btnSave_Click(); }
                else if (Request["__EVENTARGUMENT"] == "ChangeFund") { replaceFund(int.Parse(this.hfFundIDOrig.Value), int.Parse(this.hfFundIDNew.Value)); }
                else if (Request["__EVENTARGUMENT"] == "AddMoreFund") { addNewFund(int.Parse(this.hfFundIDNew.Value)); }
            }
        }
        protected void lbtnRemoveFund_Click(object sender, CommandEventArgs e)
        {
            captureSwitchGridviewDetails();

            List<clsSwitchDetails> PreviousListSwitchDetails = (List<clsSwitchDetails>)Session["SwitchDetails"];
            List<clsSwitchDetails> newListSwitchDetails = clsSwitchDetails.removeSwitchDetails(int.Parse(e.CommandArgument.ToString()), PreviousListSwitchDetails);
            populateSwitchDetails(newListSwitchDetails, Portfolio());
        }
        protected void btnPreviousPage_Click(object sender, EventArgs e)
        {
            Response.Redirect("SwitchBulkClientList.aspx");
        }
        private void btnSave_Click()
        {
            captureSwitchGridviewDetails();

            List<clsSwitchDetails> ListSwitchDetails = (List<clsSwitchDetails>)Session["SwitchDetails"];
            saveSwitch();
            Response.Redirect("SwitchBulkClientList.aspx");
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            clsSwitchTemp.deleteSwitchTemp(ClientID(), PortfolioID());
            Response.Redirect("SwitchBulkClientList.aspx");
        }

        private void saveSwitch()
        {
            List<clsSwitchDetails> newListSwitchDetails = (List<clsSwitchDetails>)Session["SwitchDetails"];
            clsSwitchTemp.insertSwitchHeaderTemp(IFA_ID(), int.Parse(ViewState["MID"].ToString()), ViewState["MGID"].ToString(), ViewState["MPID"].ToString(), ClientID(), PortfolioID(), UserID());
            clsSwitchTemp.deleteSwitchDetailsTemp(ClientID(), PortfolioID());
            clsSwitchTemp.insertSwitchDetailsTemp(int.Parse(ViewState["MID"].ToString()), newListSwitchDetails, ClientID(), PortfolioID(), UserID());
        }
        #region Populate Data

        private void populateHeader(clsPortfolio _clsPortfolio)
        {
            this.lblValue_PortfolioName.Text = _clsPortfolio.propClient.propForename + " " + _clsPortfolio.propClient.propSurname;
            this.lblValue_Company.Text = _clsPortfolio.propCompany;
            this.lblValue_PortfolioType.Text = _clsPortfolio.propPortfolioType;
            this.lblValue_Currency.Text = _clsPortfolio.propPortfolioCurrency;
            this.lblValue_AccountNumber.Text = _clsPortfolio.propAccountNumber;
            this.lblValue_PlanStatus.Text = _clsPortfolio.propPlanStatus;
            this.lblValue_StartDate.Text = _clsPortfolio.propPortfolioStartDate.ToString("dd/MM/yyyy");
            this.lblValue_PolicyCategory.Text = _clsPortfolio.propLiquidity;
            this.lblValue_Profile.Text = _clsPortfolio.propRiskProfile;
            this.lblValue_SpecialistInformation.Text = _clsPortfolio.propRetentionTerm;
            //this.lblValue_Discretionary.Text = _clsPortfolio.propMFPercent == 0 ? "no" : "yes";
            this.lblValue_Discretionary.Text = _clsPortfolio.propPortfolioDetails[0].propMFPercent == 0 ? "no" : "yes";
        }
        private void populateDetails(List<clsPortfolioDetails> listPortfolioDetails)
        {
            this.gvModelPortfolioDetails.DataSource = listPortfolioDetails;
            this.gvModelPortfolioDetails.DataBind();

            Label lblgvFooterCurrentValueClient = (Label)this.gvModelPortfolioDetails.FooterRow.Cells[6].FindControl("gvFooterCurrentValueClient");
            lblgvFooterCurrentValueClient.Text = listPortfolioDetails[0].propTotalCurrentValueClient.ToString("n0");

            Label lblgvHeaderPurchaseCostFundPortfolioCurrency = (Label)this.gvModelPortfolioDetails.HeaderRow.Cells[5].FindControl("gvHeaderPurchaseCostFundPortfolioCurrency");
            lblgvHeaderPurchaseCostFundPortfolioCurrency.Text = listPortfolioDetails[0].propPortfolioCurrency.ToString();

            Label lblgvHeaderValueClientCurCurrency = (Label)this.gvModelPortfolioDetails.HeaderRow.Cells[6].FindControl("gvHeaderValueClientCurCurrency");
            lblgvHeaderValueClientCurCurrency.Text = listPortfolioDetails[0].propClientCurrency.ToString();

            //Label lblgvHeaderGainLossCurrency = (Label)this.gvModelPortfolioDetails.HeaderRow.Cells[5].FindControl("gvHeaderGainLossCurrency");
            //lblgvHeaderGainLossCurrency.Text = listPortfolioDetails[0].propPortfolioCurrency.ToString();
        }
        private void populateSwitchDetails(List<clsSwitchDetails> lstSwitchDetailsTemp, clsPortfolio _clsPortfolio)
        {
            this.gvSwitchDetails.DataSource = lstSwitchDetailsTemp;
            this.gvSwitchDetails.DataBind();

            //this.lblSwitchStatusValue.Text = new clsSwitch(listSwitchDetails[0].propSwitchID).propStatusString;
            //int intStatus = new clsSwitch(listSwitchDetails[0].propSwitchID).propStatus;

            Label gvSwitchFooterLblTotalValue = (Label)this.gvSwitchDetails.FooterRow.Cells[3].FindControl("gvSwitchFooterLblTotalValue");
            gvSwitchFooterLblTotalValue.Text = _clsPortfolio.propTotalValue.ToString("n0");//lstSwitchDetailsTemp[0].propTotalValue.ToString("n0");

            Label gvSwitchFooterLblTotalAllocationOrig = (Label)this.gvSwitchDetails.FooterRow.Cells[4].FindControl("gvSwitchFooterLblTotalAllocationOrig");
            gvSwitchFooterLblTotalAllocationOrig.Text = lstSwitchDetailsTemp[lstSwitchDetailsTemp.Count - 1].propTotalAllocation.ToString("n2");

            Label gvSwitchFooterLblTotalAllocation = (Label)this.gvSwitchDetails.FooterRow.Cells[4].FindControl("gvSwitchFooterLblTotalAllocation");
            gvSwitchFooterLblTotalAllocation.Text = lstSwitchDetailsTemp[lstSwitchDetailsTemp.Count - 1].propTotalAllocation.ToString("n2");

            gvSwitchFooterHfTotalAllocation.Value = lstSwitchDetailsTemp[lstSwitchDetailsTemp.Count - 1].propTotalAllocation.ToString("n2");

            this.hfCurrentValueClientTotal.Value = _clsPortfolio.propTotalValue.ToString("n0");//lstSwitchDetailsTemp[0].propTotalValue.ToString("n0");
            this.hfFundCount.Value = lstSwitchDetailsTemp.Count.ToString();

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
            Session["SwitchDetails"] = lstSwitchDetailsTemp;
        }

        #endregion

        #region Functions Use in HTML

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
            populateSwitchDetails(ListSwitchDetails, Portfolio());

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
            //Session["SwitchDetailsClient"] = newListSwitchDetails;
            this.populateSwitchDetails(newListSwitchDetails, Portfolio());

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
        protected string HideClickableFundLinks(object isDeletetable)
        {
            if (!(bool)isDeletetable) { return "display:none"; }
            else { return ""; }
        }
        protected string ShowClickableFundLinks(object isDeletetable)
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
    }
}
