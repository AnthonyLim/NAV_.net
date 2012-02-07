using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NAV.Portfolio.UserControl
{
    public partial class WebUserControl1 : System.Web.UI.UserControl
    {

        private List<clsSwitchDetails> listSwitchDetails;
        public List<clsSwitchDetails> propSwitchDetails {get { return listSwitchDetails; }set { listSwitchDetails = value; }}

        private Int16 intStatus;
        public Int16 propStatus {get { return intStatus; }set { intStatus = value; }}

        private DateTime dtDateAction;
        public DateTime propDateAction {get { return dtDateAction; }set { dtDateAction = value; }}

        private String strMessage;
        public String propMessage {get { return strMessage; }set { strMessage = value; }}

        private String strTitle;
        public String propTitle { get { return strTitle; } set { strTitle = value; } }

        protected void Page_Load(object sender, EventArgs e)
        {
            populateGrid(this.propSwitchDetails);

            this.txtProposedSwitchDesc.Text = this.propMessage;
            this.lblTitle_ProposedSwitch.Text = this.propTitle;

            this.lblDateAction.Text = " (" + this.propDateAction + ") ";
        }

        private void populateGrid(List<clsSwitchDetails> SwitchDetails) {
            this.gvSwitchDetails.DataSource = SwitchDetails;
            this.gvSwitchDetails.DataBind();

            if (SwitchDetails != null)
            {

                Label gvSwitchFooterLblTotalValue = (Label)this.gvSwitchDetails.FooterRow.Cells[3].FindControl("gvSwitchFooterLblTotalValue");
                gvSwitchFooterLblTotalValue.Text = SwitchDetails[0].propTotalValue.ToString("n0");

                Label gvSwitchFooterLblTotalAllocation = (Label)this.gvSwitchDetails.FooterRow.Cells[4].FindControl("gvSwitchFooterLblTotalAllocation");
                gvSwitchFooterLblTotalAllocation.Text = SwitchDetails[SwitchDetails.Count - 1].propTotalAllocation.ToString("n2");

            }
            else { this.table1_container.Visible = false; }

        }
    }
}