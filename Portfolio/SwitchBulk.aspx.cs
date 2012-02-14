using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NAV.Portfolio
{
     public partial class SwitchBulk : System.Web.UI.Page
    {
        #region Factors on Form
         string ModelID()
         {
             if (Session["ModelID"] != null) { return Session["ModelID"].ToString(); }
             else return string.Empty;
         }
         string ModelPortfolioID()
         {
             if (Session["ModelPortfolioID"] != null) { return Session["ModelPortfolioID"].ToString(); }
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
             return new clsPortfolio(ModelID(), ModelPortfolioID(), UserID());
         }

         #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Page.IsPostBack)
            {
                if (Request["__EVENTARGUMENT"] == "SaveModelSwitchDetails") { btnSave_Click(); }
                else if (Request["__EVENTARGUMENT"] == "ChangeFund") { replaceFund(int.Parse(this.hfFundIDOrig.Value), int.Parse(this.hfFundIDNew.Value)); }
                else if (Request["__EVENTARGUMENT"] == "AddMoreFund") { addNewFund(int.Parse(this.hfFundIDNew.Value)); }
            }
            else
            {
                Session["SourcePage"] = string.Format("/MP/details.asp?MID={0}&MPID={1}", ModelID(), ModelPortfolioID());

                clsPortfolio oPortfolio = new clsPortfolio(ModelID(), ModelPortfolioID(), UserID());
                populateHeader(oPortfolio);
                populateDetails(oPortfolio.propPortfolioDetails);
                clsModelGroup _clsModelGroup = new clsModelGroup(Portfolio(), ModelID(), ModelPortfolioID(), IFA_ID());
                clsModelPortfolio _clsModelPortfolio = _clsModelGroup.propModelPortfolio;
                populateSwitchDetails(_clsModelPortfolio.propModelPortfolioDetails);
            }
        }
        private void btnSave_Click()
        {
            captureSwitchGridviewDetails();

            List<clsModelPortfolioDetails> ListSwitchDetails = (List<clsModelPortfolioDetails>)Session["ModelPortfolioDetails"];
            saveSwitch();
            //clsSwitch oSwitch = new clsSwitch(Portfolio(), UserID());
            //clsModelPortfolio _clsModelPortfolio = new clsModelPortfolio(Portfolio(), ModelID(), ModelPortfolioID());
            //populateSwitchDetails(_clsModelPortfolio.propModelPortfolioDetails);
            
            //string backPageURL = "https://" + Request.ServerVariables["SERVER_NAME"] + ":" + Request.ServerVariables["SERVER_PORT"] + "/report/" + "portfoliodetails.asp";
            //ClientScript.RegisterStartupScript(this.GetType(), "alertSaveSuccess", "alert('This Switch has been saved.'); window.location='" + backPageURL + "';", true);
        }
        private void saveSwitch()
        {
            captureSwitchGridviewDetails();
            List<clsModelPortfolioDetails> newListSwitchDetails = (List<clsModelPortfolioDetails>)Session["ModelPortfolioDetails"];
            
            clsModelGroup _clsModelGroup = new clsModelGroup(Portfolio(), ModelID(), ModelPortfolioID(), IFA_ID());
            int result = _clsModelGroup.saveModelGroupSwitch();
            int result2 = _clsModelGroup.propModelPortfolio.saveModelPortfolioSwitch();
            clsModelPortfolioDetails _clsModelPortfolioDetails = new clsModelPortfolioDetails();
            clsModelPortfolioDetails.deleteModelPortfolioDetails(IFA_ID(), ModelID(), ModelPortfolioID());
            _clsModelPortfolioDetails.saveModelPortfolioDetails(newListSwitchDetails, IFA_ID(), ModelID(), ModelPortfolioID());
        }
        protected void btnOkDescription_Click(object sender, EventArgs e)
        {
            saveSwitch();
            clsModelGroup _clsModelGroup = new clsModelGroup(Portfolio(), ModelID(), ModelPortfolioID(), IFA_ID());
            _clsModelGroup.propModelPortfolio.propModelPortfolioDesc = txtDescription.Text.Trim();
            _clsModelGroup.propModelPortfolio.updateModelPortfolioHeader();
            Response.Redirect("SwitchBulkClientList.aspx");
        }
        protected void lbtnRemoveFund_Click(object sender, CommandEventArgs e)
        {
            captureSwitchGridviewDetails();

            List<clsModelPortfolioDetails> PreviousListSwitchDetails = (List<clsModelPortfolioDetails>)Session["ModelPortfolioDetails"];
            List<clsModelPortfolioDetails> oSwitchDetailsList = clsModelPortfolioDetails.removeModelPortfolioDetails(int.Parse(e.CommandArgument.ToString()), PreviousListSwitchDetails);
            populateSwitchDetails(oSwitchDetailsList);
        }
        
        #region Populate Data

        private void populateHeader(clsPortfolio oPortfolio)
        {
            this.lblValue_PortfolioName.Text = oPortfolio.propAccountNumber;
            this.lblValue_Currency.Text = oPortfolio.propPortfolioCurrency;
            this.lblValue_StartDate.Text = oPortfolio.propPortfolioStartDate.ToString("dd/MM/yyyy");
            //this.lblValue_TotalInvestment.Text = oPortfolio.propTotalInvestments.ToString();
            //this.lblValue_CurrentValue.Text = oPortfolio.propCurrentValue.ToString();
            //this.lblValue_GainOrLoss.Text = oPortfolio.propGainLoss.ToString();
        }
        private void populateDetails(List<clsPortfolioDetails> listPortfolioDetails)
        {
            this.gvModelPortfolioDetails.DataSource = listPortfolioDetails;
            this.gvModelPortfolioDetails.DataBind();

            Label lblgvFooterCurrentValueClient = (Label)this.gvModelPortfolioDetails.FooterRow.Cells[6].FindControl("gvFooterCurrentValueClient");
            lblgvFooterCurrentValueClient.Text = listPortfolioDetails[0].propTotalCurrentValueClient.ToString("n0");

            //Label lblgvHeaderPurchaseCostFundPortfolioCurrency = (Label)this.gvModelPortfolioDetails.HeaderRow.Cells[5].FindControl("gvHeaderPurchaseCostFundPortfolioCurrency");
            //lblgvHeaderPurchaseCostFundPortfolioCurrency.Text = listPortfolioDetails[0].propPortfolioCurrency.ToString();

            //Label lblgvHeaderValueClientCurCurrency = (Label)this.gvModelPortfolioDetails.HeaderRow.Cells[6].FindControl("gvHeaderValueClientCurCurrency");
            //lblgvHeaderValueClientCurCurrency.Text = listPortfolioDetails[0].propClientCurrency.ToString();

            //Label lblgvHeaderGainLossCurrency = (Label)this.gvModelPortfolioDetails.HeaderRow.Cells[5].FindControl("gvHeaderGainLossCurrency");
            //lblgvHeaderGainLossCurrency.Text = listPortfolioDetails[0].propPortfolioCurrency.ToString();
        }
        private void populateSwitchDetails(List<clsModelPortfolioDetails> listModelPortfolioDetails)
        {
            this.gvModelPortfolioSwitchDetails.DataSource = listModelPortfolioDetails;
            this.gvModelPortfolioSwitchDetails.DataBind();

            //this.lblSwitchStatusValue.Text = new clsSwitch(listSwitchDetails[0].propSwitchID).propStatusString;
            //int intStatus = new clsSwitch(listSwitchDetails[0].propSwitchID).propStatus;

            Label gvSwitchFooterLblTotalValue = (Label)this.gvModelPortfolioSwitchDetails.FooterRow.Cells[3].FindControl("gvSwitchFooterLblTotalValue");
            gvSwitchFooterLblTotalValue.Text = listModelPortfolioDetails[0].propTotalValue.ToString("n0");

            Label gvSwitchFooterLblTotalAllocationOrig = (Label)this.gvModelPortfolioSwitchDetails.FooterRow.Cells[4].FindControl("gvSwitchFooterLblTotalAllocationOrig");
            gvSwitchFooterLblTotalAllocationOrig.Text = listModelPortfolioDetails[listModelPortfolioDetails.Count - 1].propTotalAllocation.ToString("n2");

            Label gvSwitchFooterLblTotalAllocation = (Label)this.gvModelPortfolioSwitchDetails.FooterRow.Cells[4].FindControl("gvSwitchFooterLblTotalAllocation");
            gvSwitchFooterLblTotalAllocation.Text = listModelPortfolioDetails[listModelPortfolioDetails.Count - 1].propTotalAllocation.ToString("n2");

            gvSwitchFooterHfTotalAllocation.Value = listModelPortfolioDetails[listModelPortfolioDetails.Count - 1].propTotalAllocation.ToString("n2");

            this.hfCurrentValueClientTotal.Value = listModelPortfolioDetails[0].propTotalValue.ToString("n0");
            this.hfFundCount.Value = listModelPortfolioDetails.Count.ToString();

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
            Session["ModelPortfolioDetails"] = listModelPortfolioDetails;
        }
        
        #endregion

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            List<clsModelPortfolioDetails> ListSwitchDetails = (List<clsModelPortfolioDetails>)Session["ModelPortfolioDetails"];
            clsModelPortfolio _clsModelPortfolio = new clsModelPortfolio(Portfolio(), ModelID(), ModelPortfolioID());
            _clsModelPortfolio.deleteModelPortfolioSwitch();
            clsModelPortfolio _clsModelPortfolioNew = new clsModelPortfolio(Portfolio(), ModelID(), ModelPortfolioID());
            string backPageURL = string.Format("https://{0}:{1}/MP/details.asp?MID={2}&MPID={3}", Request.ServerVariables["SERVER_NAME"], Request.ServerVariables["SERVER_PORT"], ModelID(), ModelPortfolioID());
            ClientScript.RegisterStartupScript(this.GetType(), "alertCancelledSwitch", "alert('This Switch has been cancelled'); window.location='" + backPageURL + "';", true);
            //populateSwitchDetails(_clsModelPortfolioNew.propModelPortfolioDetails);
            //clsSwitch oSwitch = new clsSwitch(Portfolio(), UserID());
            //if ((oSwitch.propStatus != (int)clsSwitch.enumSwitchStatus.Draft) && (oSwitch.propStatus != (int)clsSwitch.enumSwitchStatus.Saved))
            //{
            //    int intHistoryID = clsHistory.insertHeader(PortfolioID(), oSwitch.propSwitchID, (Int16)clsSwitch.enumSwitchStatus.Cancelled);
            //}
            //clsSwitch.deleteSwitch(ListSwitchDetails[0].propSwitchID);
            //string backPageURL = "https://" + Request.ServerVariables["SERVER_NAME"] + ":" + Request.ServerVariables["SERVER_PORT"] + "/report/" + "portfoliodetails.asp";
            //ClientScript.RegisterStartupScript(this.GetType(), "alertCancelledSwitch", "alert('This Switch has been cancelled'); window.location='" + backPageURL + "';", true);
        }

        #region Functions Use in HTML

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

        #endregion

        #region Functions Use in GridView
        private void addNewFund(int intNewFundID)
        {
            captureSwitchGridviewDetails();

            List<clsModelPortfolioDetails> ListSwitchDetails = (List<clsModelPortfolioDetails>)Session["ModelPortfolioDetails"];
            foreach (clsModelPortfolioDetails SwitchDetail in ListSwitchDetails)
            {
                Response.Write(SwitchDetail.propFundID + "<br>");
                if (SwitchDetail.propFund.propFundID == intNewFundID)
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "alertErrDuplicateFund", "alert('Duplicate funds. Please choose another.');", true);
                    return;
                }
            }
            clsModelPortfolioDetails NewSwitchDetail = new clsModelPortfolioDetails();
            NewSwitchDetail.propAllocation = 0f;
            //NewSwitchDetail.propCreated_By = UserID();
            //NewSwitchDetail.propDate_Created = DateTime.Now;
            //NewSwitchDetail.propDate_LastUpdate = DateTime.Now;
            NewSwitchDetail.propFund = new clsFund(intNewFundID);
            //NewSwitchDetail.propSwitchDetailsID = 0;
            //NewSwitchDetail.propSwitchID = ListSwitchDetails[0].propSwitchID;
            NewSwitchDetail.propTotalAllocation = ListSwitchDetails[ListSwitchDetails.Count - 1].propTotalAllocation;
            NewSwitchDetail.propCurrencyMultiplier = clsCurrency.getCurrencyMultiplier(ModelID(), NewSwitchDetail.propFund.propCurrency);
            NewSwitchDetail.propIsDeletable = true;

            ListSwitchDetails.Add(NewSwitchDetail);
            populateSwitchDetails(ListSwitchDetails);

        }
        private void replaceFund(int intOldFundID, int intNewFundID)
        {
            captureSwitchGridviewDetails();
            List<clsModelPortfolioDetails> PreviousListSwitchDetails = (List<clsModelPortfolioDetails>)Session["ModelPortfolioDetails"];

            List<clsModelPortfolioDetails> newListSwitchDetails = new List<clsModelPortfolioDetails>();

            try
            {
                newListSwitchDetails = clsModelPortfolioDetails.FundChange(intOldFundID, intNewFundID, PreviousListSwitchDetails, ModelID(), Portfolio().propPortfolioDetails[0].propClientCurrency);
            }
            catch (Exception ex)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "errDuplicateDund", "alert('" + ex.Message + "');", true);
                return;
            }
            this.populateSwitchDetails(newListSwitchDetails);
        }
        private void captureSwitchGridviewDetails()
        {
            List<clsModelPortfolioDetails> ListSwitchDetails = (List<clsModelPortfolioDetails>)Session["ModelPortfolioDetails"];
            float fTotalAllocation = 0f;
            foreach (GridViewRow row in this.gvModelPortfolioSwitchDetails.Rows)
            {
                HiddenField hfSwitchDetailsID = (HiddenField)row.FindControl("hfSwitchDetailsID");
                HiddenField hfSelectedFundID = (HiddenField)row.FindControl("hfSelectedFundID");
                HiddenField hfCurrentUnit = (HiddenField)row.FindControl("hfUnits");
                HiddenField hfCurrentValue = (HiddenField)row.FindControl("hfValue");
                TextBox txtAllocation = (TextBox)row.FindControl("gvSwitchTxtTargetAllocation");

                foreach (clsModelPortfolioDetails item in ListSwitchDetails)
                {
                    if (hfSelectedFundID.Value.Trim() == item.propFund.propFundID.ToString())
                    {
                        item.propAllocation = txtAllocation.Text.ToString() == string.Empty ? 0f : float.Parse(txtAllocation.Text.ToString());
                        item.propValue = hfCurrentValue.Value.ToString() == string.Empty ? 0f : float.Parse(hfCurrentValue.Value.ToString());
                        item.propUnits = hfCurrentUnit.Value.ToString() == string.Empty ? 0 : decimal.Parse(hfCurrentUnit.Value.ToString());
                        fTotalAllocation = fTotalAllocation + item.propAllocation;
                        item.propTotalAllocation = fTotalAllocation;
                    }
                    Session["ModelPortfolioDetails"] = ListSwitchDetails;
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
                Label gvSwitchFooterLblTotalValue = (Label)this.gvModelPortfolioDetails.FooterRow.Cells[3].FindControl("gvSwitchFooterLblTotalValue");

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
            Response.Write("YYY " + this.hfPopUpFundAction.Value);
        }
        protected void clearPopupFundFields()
        {
            this.gvFunds.DataSource = null;
            this.gvFunds.DataBind();
            this.gvFunds.Visible = false;
            this.txtPopupFund.Text = "";
        }
        //public void FixAjaxValidationSummaryErrorLabel_PreRender(object sender, EventArgs e)
        //{

        //    string objId = ((Label)sender).ID;

        //    ScriptManager.RegisterStartupScript(this, this.GetType(), objId, ";", true);

        //}

        #endregion
    }
}
