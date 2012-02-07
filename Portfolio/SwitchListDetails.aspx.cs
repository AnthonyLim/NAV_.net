using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NAV
{
    public partial class SwitchResetCodeDetails : System.Web.UI.Page
    {
        //string sourcePage = "SwitchResetCode.aspx"; //--->temporary!

        int intSwitchID;
        string strUserID;
        string strClientID;
        string strPortfolioID;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                ((NAV)this.Page.Master).FindControl("btnBack_Classic").Visible=false;
                strUserID = Session[clsSystem_Session.strSession.User.ToString()].ToString();
                ViewState["SwitchID"] = int.Parse(Request.QueryString["SwitchID"].ToString());
                ViewState["ClientID"] = Request.QueryString["ClientID"].ToString();
                ViewState["PortfolioID"] = Request.QueryString["PortfolioID"].ToString();

                strClientID = ViewState["ClientID"].ToString();
                strPortfolioID = ViewState["PortfolioID"].ToString();
                intSwitchID = (int)ViewState["SwitchID"];

                string strPortfolioForename = Request.QueryString["PortfolioForename"].ToString();
                string strPortfolioSurname = Request.QueryString["PortfolioSurname"].ToString();

                string strPortfolioName = string.Format("{0} {1}",strPortfolioForename, strPortfolioSurname);

                clsPortfolio oPortfolio = new clsPortfolio(strClientID, strPortfolioID, strUserID);

                ViewState["Company"] = oPortfolio.propCompany;
                
                populateHeader(oPortfolio, strPortfolioName);
                populateSwitchDetails(oPortfolio.propSwitch);
            }
        }
        protected void btnPrevious_Click(object sender, EventArgs e)
        {
            //Response.Redirect("https://" + Request.ServerVariables["SERVER_NAME"] + ":" + Request.ServerVariables["SERVER_PORT"] + "/report/" + sourcePage);
            Response.Redirect("SwitchList.aspx");
        }
        protected void btnSMSResetSend_Click(object sender, EventArgs e)
        {
            strUserID = Session[clsSystem_Session.strSession.User.ToString()].ToString();
            strClientID = ViewState["ClientID"].ToString();
            strPortfolioID = ViewState["PortfolioID"].ToString();
            int intSwitchID = (int)ViewState["SwitchID"];
            string strPortfolioName = ViewState["Company"].ToString();
            string strPopupMessage = "The selected portfolio has been reset.";
            string strSMSMobileNo = this.txtMobileNoResetCode.Text.Trim();
            doSwitch(intSwitchID, strPortfolioName, clsSMS.subclsSMSTemplate.enumSMSTemplateID.Reset, strPopupMessage, strSMSMobileNo);

            clsPortfolio oPortfolio = new clsPortfolio(strClientID, strPortfolioID, strUserID);
            populateSwitchDetails(oPortfolio.propSwitch);
        }
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
            //this.lblValue_Discretionary.Text = _clsPortfolio.propMFPercent == 0 ? "no" : "yes";
            this.lblValue_Discretionary.Text = _clsPortfolio.propPortfolioDetails[0].propMFPercent == 0 ? "no" : "yes";
        }
        private void populateSwitchDetails(clsSwitch oSwitch)
        {
            List<clsSwitchDetails> oSwitchDetails = oSwitch.propSwitchDetails;
            this.gvSwitchDetails.DataSource = oSwitchDetails;
            this.gvSwitchDetails.DataBind();

            this.lblSwitchStatusValue.Text = oSwitch.propStatusString;
            int intStatus = oSwitch.propStatus;

            if (intStatus != (int)clsSwitch.enumSwitchStatus.Locked)
            {
                btnResetSecurityCode.Visible = false;
            }
            Label gvSwitchDetailsFooterLabelTotalValue = (Label)this.gvSwitchDetails.FooterRow.Cells[3].FindControl("gvSwitchDetailsFooterLabelTotalValue");
            gvSwitchDetailsFooterLabelTotalValue.Text = oSwitchDetails[0].propTotalValue.ToString("n0");

            Label gvSwitchDetailsFooterLabelTotalTargetAllocation = (Label)this.gvSwitchDetails.FooterRow.Cells[4].FindControl("gvSwitchDetailsFooterLabelTotalTargetAllocation");
            gvSwitchDetailsFooterLabelTotalTargetAllocation.Text = oSwitchDetails[oSwitchDetails.Count - 1].propTotalAllocation.ToString("n2");
        }
        private void doSwitch(int intSwitchID, string strPortfolioName, clsSMS.subclsSMSTemplate.enumSMSTemplateID intSmsID, string strPopupMessage, string strSMSMobileNo)
        {
            clsSMS.subclsSMSTemplate osubclsSMSTemplate = new clsSMS.subclsSMSTemplate(intSmsID);
            string strReplacerVariable = clsSMS.subclsSMSTemplate.strPortfolioNameVariable;
            string strMessage = osubclsSMSTemplate.propMessage.Replace(strReplacerVariable, strPortfolioName);

            if (strSMSMobileNo.Trim().Length != 0)
            {
                sendSMS(intSwitchID, strUserID, strMessage, strPopupMessage, strSMSMobileNo);
                //ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + strMessage + "');", true);
            }
        }
        private string UpdateStatus(int intSwitchID)
        {
            clsSwitch.updateSwitchHeader(intSwitchID, clsSwitch.enumSwitchStatus.Proposed, string.Empty);
            return clsSwitch.enumSwitchStatus.Proposed.ToString();
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
                string strStatus = UpdateStatus(intSwitchID);
                ClientScript.RegisterStartupScript(this.GetType(), "alertMsgSent", "alert('" + strPopupMessage + "');", true);
            }
        }
    }
}