using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NAV.Portfolio
{
    public partial class SwitchBulkClientList : System.Web.UI.Page
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

        #region Functions

        private void populateModelClientList(List<clsPortfolio> clsPortfolioListDiscretionaryYes, List<clsPortfolio> clsPortfolioListDiscretionaryNo)
        {
            this.gvBulkSwitchListDiscretionaryYes.DataSource = clsPortfolioListDiscretionaryYes;
            this.gvBulkSwitchListDiscretionaryYes.DataBind();

            this.gvBulkSwitchListDiscretionaryNo.DataSource = clsPortfolioListDiscretionaryNo;
            this.gvBulkSwitchListDiscretionaryNo.DataBind();
        }
        private int saveSwitch(string strClientID, string strPortfolioID, string strUserID, clsSwitch.enumSwitchStatus enumSwitchStatus, int intSwitchID, List<clsSwitchDetails> clsSwitchDetailsList, string strDescription, int intModelID, string strModelGroupID, string strModelPortfolioID)
        {
            int intNewSwitchID = clsSwitch.insertSwitchHeaderWithModel(strPortfolioID, strClientID, strUserID, enumSwitchStatus, intSwitchID, strDescription, intModelID, ModelID(), ModelPortfolioID());
            clsSwitch.deleteSwitchDetails(intNewSwitchID);
            clsSwitchDetails.insertSwitchDetails(clsSwitchDetailsList, strUserID, intNewSwitchID);

            return intNewSwitchID;
        }
        private int saveCustomizedSwitch(int intIFA_ID, string strClientID, string strPortfolioID, string strUserID, clsSwitch.enumSwitchStatus enumSwitchStatus, int intSwitchID, string strDescription, string strModelGroupID, string strModelPortfolioID)
        {
            int intNewSwitchID = clsSwitch.insertCustomizedSwitchHeaderWithModel(intIFA_ID, strClientID, strPortfolioID, strUserID, enumSwitchStatus, intSwitchID, strDescription, strModelGroupID, strModelPortfolioID);

            clsSwitch.deleteSwitchDetails(intNewSwitchID);
            clsSwitchDetails.insertSwitchDetails(intNewSwitchID, strClientID, strPortfolioID);

            return intNewSwitchID;
        }
        private void saveSwitchHistory(clsPortfolio _clsPortfolio, int intSwitchID, string strPortfolioID, string strUserID, clsSwitch.enumSwitchStatus enumSwitchStatus, string strDescription)
        {
            clsSwitch oSwitch = new clsSwitch(_clsPortfolio, strUserID);
            int intHistoryID = clsHistory.insertHeader(strPortfolioID, intSwitchID, Convert.ToInt16(enumSwitchStatus));
            clsHistory.insertDetailsIFA(intHistoryID, oSwitch.propSwitchDetails);
            clsHistory.insertMessage(intHistoryID, strDescription);
        }
        private string getSMSMessage(string strClientID, string strPortfolioID, string strUserID)
        {
            clsPortfolio _clsPortfolio = new clsPortfolio();
            _clsPortfolio.getPortfolioHeader(strClientID, strPortfolioID);
            string strPortfolioName = _clsPortfolio.propCompany;

            clsSMS.subclsSMSTemplate osubclsSMSTemplate = new clsSMS.subclsSMSTemplate(clsSMS.subclsSMSTemplate.enumSMSTemplateID.ProposeSwitch);
            string strReplacerVariable = clsSMS.subclsSMSTemplate.strPortfolioNameVariable;
            string strMessage = osubclsSMSTemplate.propMessage.Replace(strReplacerVariable, strPortfolioName);

            return strMessage;
        }
        private bool sendSMS(string strUserID, string strSMSMessage, string strPopupMessage, string strSMSMobileNo)
        {
            bool result = false;
            clsSMS SMS = new clsSMS(strUserID);
            SMS.sendMessage(strSMSMobileNo.Trim(), strSMSMessage);

            if (!SMS.propErrorMsg.Equals(""))
            {
                ClientScript.RegisterStartupScript(this.GetType(), "alertMsgFailed", "alert('Sending Failed  " + SMS.propErrorMsg.Replace("'", " ") + "');", true);
                result = false;
            }

            if (!SMS.propReturnID.Equals(""))
            {
                ClientScript.RegisterStartupScript(this.GetType(), "alertMsgSent", "alert('" + strPopupMessage + "');", true);
                result = true;
            }
            return result;
        }
        private void doBulkSwitch()
        {
            int intSwitchID = 0;
            string strPopupMessage = "Message sent.";
            GridView gvClientList = new GridView();
            List<clsSwitchDetails> clsSwitchDetailsList = new List<clsSwitchDetails>();
            clsModelGroup _clsModelGroup = new clsModelGroup(Portfolio(), ModelID(), ModelPortfolioID(), IFA_ID());
            clsModelPortfolio _clsModelPortfolio = _clsModelGroup.propModelPortfolio;
            
            foreach (clsModelPortfolioDetails item in _clsModelPortfolio.propModelPortfolioDetails)
            {
                clsSwitchDetails _clsSwitchDetails = new clsSwitchDetails();
                _clsSwitchDetails.propSwitchID = 0;
                _clsSwitchDetails.propFund = item.propFund;
                _clsSwitchDetails.propFundID = item.propFundID;
                _clsSwitchDetails.propAllocation = item.propAllocation;
                _clsSwitchDetails.propSwitchDetailsID = 0;
                _clsSwitchDetails.propIsDeletable = item.propIsDeletable;

                clsSwitchDetailsList.Add(_clsSwitchDetails);
            }
            gvClientList = this.gvBulkSwitchListDiscretionaryYes;
            clsSwitch.enumSwitchStatus enumSwitchStatus;
            
            foreach (GridViewRow row in gvClientList.Rows)
            {
                CheckBox cbox = (CheckBox)row.FindControl("cboxSelected");
                Label lblSwitchID = (Label)row.FindControl("lblSwitchID");
                Label lblDiscretionary = (Label)row.FindControl("lblDiscretionary");
                Label lblCustomized = (Label)row.FindControl("lblCustomized");
                if (lblDiscretionary.Text == "Yes")
                {
                    enumSwitchStatus = clsSwitch.enumSwitchStatus.Approved;
                }
                else
                {
                    enumSwitchStatus = clsSwitch.enumSwitchStatus.Proposed;
                }
                string strClientID = Convert.ToString(gvClientList.DataKeys[row.RowIndex].Values[0]);
                string strPortfolioID = Convert.ToString(gvClientList.DataKeys[row.RowIndex].Values[1]);

                if (cbox.Checked)
                {
                    string strMessage = getSMSMessage(strClientID, strPortfolioID, UserID());
                    string strMobileNum = clsSMS.getMobileNumber(strClientID);
                    if (strMobileNum.Trim().Equals(string.Empty))
                    {
                        strMobileNum = "9228829490";
                    }
                    if (sendSMS(UserID(), strMessage, strPopupMessage, strMobileNum))
                    {
                        if (lblCustomized.Text.Trim() == "Yes")
                        {
                            intSwitchID = saveCustomizedSwitch(IFA_ID(), strClientID, strPortfolioID, UserID(), enumSwitchStatus, int.Parse(lblSwitchID.Text.Trim()), _clsModelPortfolio.propModelPortfolioDesc, ModelID(), ModelPortfolioID());
                            clsPortfolio _clsPortfolio = new clsPortfolio(strClientID, strPortfolioID, UserID());
                            saveSwitchHistory(_clsPortfolio, intSwitchID, strPortfolioID, UserID(), enumSwitchStatus, _clsModelPortfolio.propModelPortfolioDesc);
                            _clsModelPortfolio.propIsConsumed = true;

                        }
                        else
                        {
                            clsPortfolio _clsPortfolio = new clsPortfolio(strClientID, strPortfolioID, UserID());
                            _clsPortfolio.propModelGroupID = ModelID();
                            _clsPortfolio.propModelPortfolioID = ModelPortfolioID();
                            clsSwitchTemp _clsSwitchTemp = new clsSwitchTemp();
                            _clsSwitchTemp.propModelGroupID = _clsModelPortfolio.propModelGroupID;
                            _clsSwitchTemp.propModelPortfolioID = _clsModelPortfolio.propModelPortfolioID;
                            _clsPortfolio.propSwitchTemp = new clsSwitchTemp(_clsPortfolio, UserID(), IFA_ID(), _clsModelPortfolio.propModelID, ModelID(), ModelPortfolioID());
                            intSwitchID = saveSwitch(strClientID, strPortfolioID, UserID(), enumSwitchStatus, int.Parse(lblSwitchID.Text.Trim()), _clsPortfolio.propSwitchTemp.propSwitchDetails, _clsModelPortfolio.propModelPortfolioDesc, _clsModelPortfolio.propModelID, ModelID(), ModelPortfolioID());
                            saveSwitchHistory(_clsPortfolio, intSwitchID, strPortfolioID, UserID(), enumSwitchStatus, _clsModelPortfolio.propModelPortfolioDesc);
                            _clsModelPortfolio.propIsConsumed = true;
                        }
                    }
                }
                _clsModelPortfolio.propIsConsumed = true;
                _clsModelPortfolio.updateModelPortfolioHeader();
                clsSwitchTemp.deleteSwitchTempByModel(_clsModelPortfolio.propModelID);
            }
            List<clsPortfolio> clsPortfolioListDiscretionaryYes = clsPortfolio.getPortfolioList(IFA_ID(), _clsModelPortfolio.propModelID, ModelID(), ModelPortfolioID(), true);
            List<clsPortfolio> clsPortfolioListDiscretionaryNo = clsPortfolio.getPortfolioList(IFA_ID(), _clsModelPortfolio.propModelID, ModelID(), ModelPortfolioID(), false);
            populateModelClientList(clsPortfolioListDiscretionaryYes, clsPortfolioListDiscretionaryNo);
        }

        #endregion

        #region Events
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                ((NAV)this.Page.Master).FindControl("btnBack_Classic").Visible = false;

                clsModelGroup _clsModelGroup = new clsModelGroup(Portfolio(), ModelID(), ModelPortfolioID(), IFA_ID());
                clsModelPortfolio _clsModelPortfolio = _clsModelGroup.propModelPortfolio;

                List<clsPortfolio> clsPortfolioListDiscretionaryYes = clsPortfolio.getPortfolioList(IFA_ID(), _clsModelPortfolio.propModelID, ModelID(), ModelPortfolioID(), true);
                List<clsPortfolio> clsPortfolioListDiscretionaryNo = clsPortfolio.getPortfolioList(IFA_ID(), _clsModelPortfolio.propModelID, ModelID(), ModelPortfolioID(), false);
                populateModelClientList(clsPortfolioListDiscretionaryYes, clsPortfolioListDiscretionaryNo);

            }
        }
        protected void btnPreviousPage_Click(object sender, EventArgs e)
        {
            Response.Redirect("SwitchBulk.aspx");
        }
        protected void btnProceedBulkSwitch_Click(object sender, EventArgs e)
        {
            if (!this.txtDescription.Text.Trim().Equals(string.Empty))
            {
                clsModelPortfolio _clsModelPortfolio = new clsModelPortfolio(Portfolio(), ModelID(), ModelPortfolioID());
                _clsModelPortfolio.propModelPortfolioDesc = txtDescription.Text.Trim();
                _clsModelPortfolio.updateModelPortfolioHeader();
            }
            doBulkSwitch();
            string backPageURL = string.Format("https://{0}:{1}/MP/details.asp?MID={2}&MPID={3}", Request.ServerVariables["SERVER_NAME"], Request.ServerVariables["SERVER_PORT"], ModelID(), ModelPortfolioID());
            ClientScript.RegisterStartupScript(this.GetType(), "alertSaveSuccess", "alert('Bulk Switch successfull'); window.location='" + backPageURL + "';", true);
        } 
        #endregion

        #region Popup Buttons(Unused)
        protected void btnSwitchSendSave_Click(object sender, EventArgs e)
        {
            if (!this.txtDescription.Text.Trim().Equals(string.Empty))
            {
                clsModelPortfolio _clsModelPortfolio = new clsModelPortfolio(Portfolio(), ModelID(), ModelPortfolioID());
                _clsModelPortfolio.propModelPortfolioDesc = txtDescription.Text.Trim();
                _clsModelPortfolio.updateModelPortfolioHeader();
            }
            doBulkSwitch();
        }
        protected void btnSwitchNo_Click(object sender, EventArgs e)
        {

        }
        public void btnSwitchProceed_Click()
        {
            ClientScript.RegisterStartupScript(this.GetType(), "popupSwitch", string.Format("showhidePopup({0}, {1});", this.mpeSwitchPopup2.BehaviorID, true), true);
        }
        protected void btnProceedDiscretionaryNo_Click(object sender, EventArgs e)
        {
            //doBulkSwitch(this.gvBulkSwitchListDiscretionaryNo);
        } 
        #endregion
    }
}