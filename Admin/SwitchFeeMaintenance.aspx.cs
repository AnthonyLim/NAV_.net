using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NAV.Admin
{
    public partial class Fee_List : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                Session["SourcePage"] = "/report/ifa/searchindex.asp";
                List<clsSwitchFee> oSwitchFeeList = clsSwitchFee.getSwitchFeeList();
                populateSwitchFeeList(oSwitchFeeList);

                List<clsIFA> oIFAList = clsIFA.getIFAList();
                populateIFAList(oIFAList);
            }
        }
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            btnEditSwitchFeeConfig.Visible = false;
            btnAddSwitchFeeConfig.Visible = true;
            clearModalPopupFields();
            mpeSwitchFeeConfig.Show();
        }
        protected void lbtnEditIFASwitchFee_Click(object sender, CommandEventArgs e)
        {
            btnAddSwitchFeeConfig.Visible = false;
            btnEditSwitchFeeConfig.Visible = true;
            int intIFA_ID = int.Parse(e.CommandArgument.ToString());
            populateModalPopupFields(intIFA_ID);
            mpeSwitchFeeConfig.Show();
        }
        protected void lbtnRemoveIFASwitchFee_Click(object sender, CommandEventArgs e)
        {
            int intIFA_ID = int.Parse(e.CommandArgument.ToString());
            clsSwitchFee.deleteSwitchFee(intIFA_ID);
            List<clsSwitchFee> oSwitchFeeList = clsSwitchFee.getSwitchFeeList();
            populateSwitchFeeList(oSwitchFeeList);
        }
        protected void btnAddSwitchFeeConfig_Click(object sender, EventArgs e)
        {
            int intIFA_ID = int.Parse(ddlIFAList.SelectedValue.ToString());
            foreach (GridViewRow row in gvSwitchFee_List.Rows)
            {
                if (intIFA_ID == int.Parse(row.Cells[1].Text.Trim()))
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "alertErrDuplicateIFA", "alert('Duplicate IFA. Please choose another.');", true);
                    return;
                }
            }
            saveSwitchFee();
        }
        protected void btnEditSwitchFeeConfig_Click(object sender, EventArgs e)
        {
            saveSwitchFee();
        }

        private void saveSwitchFee()
        {
            int intIFA_ID = int.Parse(ddlIFAList.SelectedValue.ToString());
            string strIFA_Name = ddlIFAList.SelectedItem.Text;
            decimal dAnnual_Fee = txtAnnualFeeAmount.Text.Trim() == string.Empty ? 0 : decimal.Parse(txtAnnualFeeAmount.Text.Trim());
            decimal dPerSwitch_Fee = txtPerSwitchFeeAmount.Text.Trim() == string.Empty ? 0 : decimal.Parse(txtPerSwitchFeeAmount.Text.Trim());
            string strUser = Session[clsSystem_Session.strSession.User.ToString()].ToString();
            bool bAccessDenied = cboxAccessDenied.Checked;

            clsSwitchFee.saveSwitchFee(intIFA_ID, strIFA_Name, dAnnual_Fee, dPerSwitch_Fee, strUser, bAccessDenied);
            List<clsSwitchFee> oSwitchFeeList = clsSwitchFee.getSwitchFeeList();
            populateSwitchFeeList(oSwitchFeeList);
        }
        private void populateSwitchFeeList(List<clsSwitchFee> oSwitchFeeList)
        {
            gvSwitchFee_List.DataSource = oSwitchFeeList;
            gvSwitchFee_List.DataBind();
        }
        private void populateIFAList(List<clsIFA> oIFAList)
        {
            ddlIFAList.DataSource = oIFAList;
            ddlIFAList.DataValueField = "propIFA_ID";
            ddlIFAList.DataTextField = "propIFA_Name";
            ddlIFAList.DataBind();
        }
        private void populateModalPopupFields(int intIFA_ID)
        {
            clsSwitchFee oSwitchFee = new clsSwitchFee(intIFA_ID);
            
            ddlIFAList.SelectedValue = oSwitchFee.propIFA_ID.ToString();
            ddlIFAList.Enabled = false;
            txtAnnualFeeAmount.Text = oSwitchFee.propAnnual_Fee.ToString();
            txtPerSwitchFeeAmount.Text = oSwitchFee.propPer_Switch_Fee.ToString();
            cboxAccessDenied.Checked = oSwitchFee.propAccess_Denied;
        }
        private void clearModalPopupFields()
        {
            ddlIFAList.Enabled = true;
            ddlIFAList.SelectedIndex = 0;
            txtAnnualFeeAmount.Text = "0.00";
            txtPerSwitchFeeAmount.Text = "0.00";
            cboxAccessDenied.Checked = false;
        }
    }
}
