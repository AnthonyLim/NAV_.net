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

        private void populateModelClientList(List<clsPortfolio> lstPortfolioWithModel)
        {
            this.gvClientListInModel.DataSource = lstPortfolioWithModel;
            this.gvClientListInModel.DataBind();
        }
        private void populateClientAndMobileNumber(List<clsClient> lstClientAndMobileNumber)
        {
            this.gvClientAndMobileNumberList.DataSource = lstClientAndMobileNumber;
            this.gvClientAndMobileNumberList.DataBind();
        }
        private void processBulkSwitch(string _strClientID, string strMobileNumber, clsModelPortfolio _clsModelPortfolio)
        {
            int intSwitchID = 0;
            clsSwitch.enumSwitchStatus enumSwitchStatus;

            foreach (GridViewRow row in gvClientListInModel.Rows)
            {
                Label lblSwitchID = (Label)row.FindControl("lblSwitchID");
                Label lblClientName = (Label)row.FindControl("gvBulkSwitchListLabelClientName");
                Label lblDiscretionary = (Label)row.FindControl("lblDiscretionary");
                Label lblCustomized = (Label)row.FindControl("lblCustomized");
                string strClientID = Convert.ToString(gvClientListInModel.DataKeys[row.RowIndex].Values[0]);
                string strPortfolioID = Convert.ToString(gvClientListInModel.DataKeys[row.RowIndex].Values[1]);

                if (_strClientID.Trim().Equals(strClientID.Trim()))
                {
                    enumSwitchStatus = determineDiscretionaryStatus(lblDiscretionary.Text.Trim());

                    string strMessage = getSMSMessage(strClientID, strPortfolioID, UserID());
                    if (sendSMS(UserID(), strMessage, lblClientName.Text.Trim(), strMobileNumber))
                    {
                        clsPortfolio _clsPortfolio = new clsPortfolio(strClientID, strPortfolioID, UserID());
                        if (lblCustomized.Text.Trim() == enumYesNO.Yes.ToString())
                        {
                            intSwitchID = saveCustomizedSwitch(IFA_ID(), strClientID, strPortfolioID, UserID(), enumSwitchStatus, int.Parse(lblSwitchID.Text.Trim()), ModelID(), ModelPortfolioID());
                        }
                        else
                        {
                            _clsPortfolio.propModelGroupID = ModelID();
                            _clsPortfolio.propModelPortfolioID = ModelPortfolioID();
                            _clsPortfolio.propSwitchTemp = new clsSwitchTemp(_clsPortfolio, UserID(), IFA_ID(), _clsModelPortfolio.propModelID, ModelID(), ModelPortfolioID());
                            intSwitchID = saveSwitch(strClientID, strPortfolioID, UserID(), enumSwitchStatus, int.Parse(lblSwitchID.Text.Trim()), _clsPortfolio.propSwitchTemp.propSwitchDetails, _clsModelPortfolio.propModelPortfolioDesc, _clsModelPortfolio.propModelID, ModelID(), ModelPortfolioID());
                        }
                        saveSwitchHistory(_clsPortfolio, intSwitchID, strPortfolioID, UserID(), enumSwitchStatus, _clsModelPortfolio.propModelPortfolioDesc);
                    }//end if
                }//end if
            }//end foreach
        }
        private void saveSwitchHistory(clsPortfolio _clsPortfolio, int intSwitchID, string strPortfolioID, string strUserID, clsSwitch.enumSwitchStatus enumSwitchStatus, string strDescription)
                {
                    clsSwitch oSwitch = new clsSwitch(_clsPortfolio, strUserID);
                    int intHistoryID = clsHistory.insertHeader(strPortfolioID, intSwitchID, Convert.ToInt16(enumSwitchStatus));
                    clsHistory.insertDetailsIFA(intHistoryID, oSwitch.propSwitchDetails);
                    clsHistory.insertMessage(intHistoryID, strDescription);
                }
        private int saveSwitch(string strClientID, string strPortfolioID, string strUserID, clsSwitch.enumSwitchStatus enumSwitchStatus, int intSwitchID, List<clsSwitchDetails> clsSwitchDetailsList, string strDescription,int intModelID, string strModelGroupID, string strModelPortfolioID)
        {
            int intNewSwitchID = clsSwitch.insertSwitchHeaderWithModel(strPortfolioID, strClientID, strUserID, enumSwitchStatus, intSwitchID, strDescription, intModelID, ModelID(), ModelPortfolioID());
            clsSwitch.deleteSwitchDetails(intNewSwitchID);
            clsSwitchDetails.insertSwitchDetails(clsSwitchDetailsList, strUserID, intNewSwitchID);

            return intNewSwitchID;
        }
        private int saveCustomizedSwitch(int intIFA_ID, string strClientID, string strPortfolioID, string strUserID, clsSwitch.enumSwitchStatus enumSwitchStatus, Nullable<int> intSwitchID, string strModelGroupID, string strModelPortfolioID)
        {
            int intNewSwitchID = clsSwitch.insertCustomizedSwitchHeaderWithModel(intIFA_ID, strClientID, strPortfolioID, strUserID, enumSwitchStatus, intSwitchID, strModelGroupID, strModelPortfolioID);
            
            clsSwitch.deleteSwitchDetails(intNewSwitchID);
            clsSwitchDetails.insertSwitchDetails(intNewSwitchID, strClientID, strPortfolioID);

            return intNewSwitchID;
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
        private bool sendSMS(string strUserID, string strSMSMessage, string strClientName, string strSMSMobileNo)
        {
            System.Text.StringBuilder sbErrorMessage;
            bool result = false;
            clsSMS SMS = new clsSMS(strUserID);

            if (ViewState["SMSErrorMessage"] != null)
            {
                sbErrorMessage = (System.Text.StringBuilder)ViewState["SMSErrorMessage"];
            }
            else
            {
                sbErrorMessage = new System.Text.StringBuilder();
            }
            
            SMS.sendMessage(strSMSMobileNo.Trim(), strSMSMessage);

            if (!SMS.propErrorMsg.Equals(""))
            {
                string strError = string.Empty;

                if (SMS.propErrorMsg.Contains("114") || SMS.propErrorMsg.Contains("105"))
                {
                    strError = string.Format("Invalid mobile number {0} for {1}.\\n", strSMSMobileNo, strClientName);
                }
                else
                {
                    strError = string.Format("{0} for {1} with Mobile No. {2} \\n", SMS.propErrorMsg, strClientName, strSMSMobileNo);
                }
                sbErrorMessage.Append(strError);
                ViewState["SMSErrorMessage"] = sbErrorMessage;
                result = false;
            }
            if (!SMS.propReturnID.Equals(""))
            {
                result = true;
            }
            return result;
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

                List<clsPortfolio> clientListWithModel = clsPortfolio.getClientListWithModel(IFA_ID(), _clsModelPortfolio.propModelID, ModelID(), ModelPortfolioID(), string.Empty);
                populateModelClientList(clientListWithModel);
            }
        }
        protected void btnPreviousPage_Click(object sender, EventArgs e)
        {
            Response.Redirect("SwitchBulk.aspx");
        }
        protected void btnBulkSwitch_Click(object sender, EventArgs e)
        {
            if (!this.txtDescription.Text.Trim().Equals(string.Empty))
            {
                clsModelPortfolio _clsModelPortfolio = new clsModelPortfolio(Portfolio(), ModelID(), ModelPortfolioID());
                _clsModelPortfolio.propModelPortfolioDesc = txtDescription.Text.Trim();
                _clsModelPortfolio.updateModelPortfolioHeader();
            }
            List<clsClient> listOfClient = new List<clsClient>();
            foreach (GridViewRow row in gvClientListInModel.Rows)
            {
                CheckBox cbox = (CheckBox)row.FindControl("cboxSelected");
                string strClientID = Convert.ToString(gvClientListInModel.DataKeys[row.RowIndex].Values[0]);
                if (cbox.Checked)
                {
                    clsClient oClient = new clsClient(strClientID);
                    listOfClient.Add(oClient);
                }
            }
            populateClientAndMobileNumber(listOfClient);
            mpeClientPopup.Show();
        }
        protected void btnProceedBulkSwitch_Click(object sender, EventArgs e)
        {
            clsModelGroup _clsModelGroup = new clsModelGroup(Portfolio(), ModelID(), ModelPortfolioID(), IFA_ID());
            clsModelPortfolio _clsModelPortfolio = _clsModelGroup.propModelPortfolio;

            foreach (GridViewRow row in gvClientAndMobileNumberList.Rows)
            {
                TextBox txtMobileNumber = (TextBox)row.FindControl("txtMobileNumber");
                Label lblForename = (Label)row.FindControl("gvClientAndMobileNumberListLabelForename");
                Label lblSurname = (Label)row.FindControl("gvClientAndMobileNumberListLabelSurname");
                string strClientID = Convert.ToString(gvClientAndMobileNumberList.DataKeys[row.RowIndex].Values[0]);

                if (!txtMobileNumber.Text.Trim().Equals(string.Empty))
                {
                    processBulkSwitch(strClientID, txtMobileNumber.Text.Trim(), _clsModelPortfolio);
                }
            }
            _clsModelPortfolio.propIsConsumed = true;
            _clsModelPortfolio.updateModelPortfolioHeader();
            clsSwitchTemp.deleteSwitchTempByModel(_clsModelPortfolio.propModelID);


            string strPopupMessage = string.Empty;
            if (ViewState["SMSErrorMessage"] != null)
            {
                System.Text.StringBuilder sbErrorMessage = (System.Text.StringBuilder)ViewState["SMSErrorMessage"];
                strPopupMessage = sbErrorMessage.ToString();
            }
            else
            {
                strPopupMessage = "Bulk Switch processed successfully";
            }
            string backPageURL = string.Format("https://{0}:{1}/MP/details.asp?MID={2}&MPID={3}", Request.ServerVariables["SERVER_NAME"], Request.ServerVariables["SERVER_PORT"], ModelID(), ModelPortfolioID());            
            ClientScript.RegisterStartupScript(this.GetType(), "alertPopupMessage", string.Format("alert('{0}'); window.location='{1}';", strPopupMessage, backPageURL), true);
        }
        protected void SortClientList(object sender, EventArgs e)
        {
            LinkButton oClick = (LinkButton)sender;
            clsModelGroup _clsModelGroup = new clsModelGroup(Portfolio(), ModelID(), ModelPortfolioID(), IFA_ID());
            clsModelPortfolio _clsModelPortfolio = _clsModelGroup.propModelPortfolio;
            List<clsPortfolio> clientListWithModel = clsPortfolio.getClientListWithModel(IFA_ID(), _clsModelPortfolio.propModelID, ModelID(), ModelPortfolioID(), oClick.CommandName);
            populateModelClientList(clientListWithModel);
        }

        #endregion

        #region Private Enums

        private clsSwitch.enumSwitchStatus determineDiscretionaryStatus(string strDiscretionary)
        {
            if (strDiscretionary == enumYesNO.Yes.ToString())
            {
                return clsSwitch.enumSwitchStatus.Approved;
            }
            else
            {
                return clsSwitch.enumSwitchStatus.Proposed;
            }
        }
        private enum enumYesNO
        {
            Yes,
            No
        }

        #endregion

        #region Unused

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
            //ClientScript.RegisterStartupScript(this.GetType(), "popupSwitch", string.Format("showhidePopup({0}, {1});", this.mpeSwitchPopup2.BehaviorID, true), true);
        }
        //original function to Process Bulk Switch
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
            gvClientList = this.gvClientListInModel;

            clsSwitch.enumSwitchStatus enumSwitchStatus;// = (clsSwitch.enumSwitchStatus)Enum.ToObject(typeof(clsSwitch.enumSwitchStatus), intSwitchStatus);

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
                    //if (sendSMS(UserID(), strMessage, strPopupMessage, strMobileNum))
                    //{
                    if (lblCustomized.Text.Trim() == "Yes")
                    {
                        intSwitchID = saveCustomizedSwitch(IFA_ID(), strClientID, strPortfolioID, UserID(), enumSwitchStatus, int.Parse(lblSwitchID.Text.Trim()), ModelID(), ModelPortfolioID());
                        clsPortfolio _clsPortfolio = new clsPortfolio(strClientID, strPortfolioID, UserID());
                        saveSwitchHistory(_clsPortfolio, intSwitchID, strPortfolioID, UserID(), enumSwitchStatus, _clsModelPortfolio.propModelPortfolioDesc);
                    }
                    else
                    {
                        clsPortfolio _clsPortfolio = new clsPortfolio(strClientID, strPortfolioID, UserID());
                        _clsPortfolio.propModelGroupID = ModelID();
                        _clsPortfolio.propModelPortfolioID = ModelPortfolioID();
                        //_clsPortfolio.propSwitchTemp = new clsSwitchTemp(_clsPortfolio, UserID(), 1, IFA_ID(), ModelID(), ModelPortfolioID());


                        //clsModelPortfolio _clsModelPortfolio = new clsModelPortfolio(_clsPortfolio, ViewState["MGID"].ToString(), ViewState["MPID"].ToString());
                        clsSwitchTemp _clsSwitchTemp = new clsSwitchTemp();
                        //_clsSwitchTemp.propModelID = _clsModelPortfolio.propModelID;
                        _clsSwitchTemp.propModelGroupID = _clsModelPortfolio.propModelGroupID;
                        _clsSwitchTemp.propModelPortfolioID = _clsModelPortfolio.propModelPortfolioID;
                        _clsPortfolio.propSwitchTemp = new clsSwitchTemp(_clsPortfolio, UserID(), IFA_ID(), _clsModelPortfolio.propModelID, ModelID(), ModelPortfolioID());

                        intSwitchID = saveSwitch(strClientID, strPortfolioID, UserID(), enumSwitchStatus, int.Parse(lblSwitchID.Text.Trim()), _clsPortfolio.propSwitchTemp.propSwitchDetails, _clsModelPortfolio.propModelPortfolioDesc, _clsModelPortfolio.propModelID, ModelID(), ModelPortfolioID());

                        saveSwitchHistory(_clsPortfolio, intSwitchID, strPortfolioID, UserID(), enumSwitchStatus, _clsModelPortfolio.propModelPortfolioDesc);
                    }
                    //}
                }
            }
            _clsModelPortfolio.propIsConsumed = true;
            _clsModelPortfolio.updateModelPortfolioHeader();
            clsSwitchTemp.deleteSwitchTempByModel(_clsModelPortfolio.propModelID);

            List<clsPortfolio> clientListWithModel = clsPortfolio.getClientListWithModel(IFA_ID(), _clsModelPortfolio.propModelID, ModelID(), ModelPortfolioID(), "");
            populateModelClientList(clientListWithModel);
        }

        #endregion
    }
}