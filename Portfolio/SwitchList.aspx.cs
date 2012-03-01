using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;

namespace NAV
{
    public partial class SwitchList : System.Web.UI.Page
    {
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

        string sourcePage = "/report/ifa/searchindex.asp"; //--->temporary!
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                Session["SourcePage"] = "/report/ifa/searchindex.asp";
                //int intIFA_ID = int.Parse(Session[clsSystem_Session.strSession.ifaid.ToString()].ToString());

                List<clsSwitch> oSwitchList = clsSwitch.getSwitchList(IFA_ID(), string.Empty, string.Empty, 0, string.Empty, string.Empty);
                populateSwitchList(oSwitchList);
                populateStatusDropdown(ddlSearchStatus);
                populateYearDropdown(ddlStartDateYear);
                populateYearDropdown(ddlEndDateYear);
                populateMonthDropdown(ddlStartDateMonth);
                populateMonthDropdown(ddlEndDateMonth);
                populateDayDropdown(ddlStartDateDay);
                populateDayDropdown(ddlEndDateDay);
            }
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            searchSwitchList();
        }
        private void searchSwitchList()
        {
            string strStartDate = string.Empty;
            string strEndDate = string.Empty;

            if ((ddlStartDateDay.SelectedValue.Trim() + ddlStartDateMonth.SelectedValue.Trim() + ddlStartDateYear.SelectedValue.Trim()).Length > 0)
            {
                strStartDate = string.Format("{0}-{1}-{2}", ddlStartDateDay.SelectedValue.Trim(), ddlStartDateMonth.SelectedValue.Trim(), ddlStartDateYear.SelectedValue.Trim());
            }
            if ((ddlEndDateDay.SelectedValue.Trim() + ddlEndDateMonth.SelectedValue.Trim() + ddlEndDateYear.SelectedValue.Trim()).Length > 0)
            {
                strEndDate = string.Format("{0}-{1}-{2}", ddlEndDateDay.SelectedValue.Trim(), ddlEndDateMonth.SelectedValue.Trim(), ddlEndDateYear.SelectedValue.Trim());
            }
            //Response.Write("X:" + strStartDate + "<br> X:" + strEndDate);
            Response.Write(this.txtSearchClient.Text.Trim());
            List<clsSwitch> oSwitchList = clsSwitch.getSwitchList(IFA_ID(), this.txtSearchClient.Text.Trim(), this.txtSearchInsuranceCompany.Text.Trim(), int.Parse(this.ddlSearchStatus.SelectedValue.Trim()), strStartDate, strEndDate);
            populateSwitchList(oSwitchList);
        }
        protected void btnSwitchCancel_Click(object sender, EventArgs e)
        {
            foreach (GridViewRow row in gvSwitchList.Rows)
            {
                CheckBox cbox = (CheckBox)row.FindControl("gvSwitchListCheckbox");
                LinkButton lnk = (LinkButton)row.FindControl("lnkbtnGVCodeResetSwitchID");
                Label lbl = (Label)row.FindControl("gvSwitchListLabelStatus");
                string strPortfolioID = Convert.ToString(gvSwitchList.DataKeys[row.RowIndex].Values[0]);
                if (cbox.Checked)
                {
                    if ((lbl.Text.Trim() != clsSwitch.enumSwitchStatus.Saved.ToString()) && (lbl.Text.Trim() != clsSwitch.enumSwitchStatus.Draft.ToString()))
                    {
                        int intHistoryID = clsHistory.insertHeader(strPortfolioID, int.Parse(lnk.Text), (Int16)clsSwitch.enumSwitchStatus.Cancelled);
                    }
                    clsSwitch.deleteSwitch(int.Parse(lnk.Text));
                }
            }
            searchSwitchList();
        }
        protected void btnPrevious_Click(object sender, EventArgs e)
        {
            Response.Redirect("https://" + Request.ServerVariables["SERVER_NAME"] + ":" + Request.ServerVariables["SERVER_PORT"] + "/report/" + sourcePage);
        }

        private void populateSwitchList(List<clsSwitch> oSwitchList)
        {
            this.gvSwitchList.DataSource = oSwitchList;
            this.gvSwitchList.DataBind();
        }
        private void populateStatusDropdown(DropDownList ddl)
        {
            ListItem item;
            foreach (clsSwitch.enumSwitchStatus enumItem in Enum.GetValues(typeof(clsSwitch.enumSwitchStatus)))
            {
                if (enumItem != 0)
                {
                    item = new ListItem(enumItem.ToString(), ((int)enumItem).ToString());
                    ddl.Items.Add(item);
                }
            }
            item = new ListItem("", "0");
            ddl.Items.Insert(0, item);
        }
        private void populateYearDropdown(DropDownList ddl)
        {
            ArrayList arr = new ArrayList();

            for (int i = 0; i < 10; i++)
            {
                arr.Add(DateTime.Now.AddYears(-i).Year.ToString());
            }

            ListItem item;

            foreach (string str in arr)
            {
                item = new ListItem(str, str);
                ddl.Items.Add(item);
            }
            //ddl.SelectedValue = DateTime.Now.Year.ToString();
            item = new ListItem("", "");
            ddl.Items.Insert(0, item);
        }
        private void populateMonthDropdown(DropDownList ddl)
        {
            Dictionary<string, string> arr = new Dictionary<string, string>();
            string idxValue = string.Empty;
            string strValue = string.Empty;
            for (int i = 1; i <= 12; i++)
            {
                if (i.ToString().Length == 1)
                {
                    idxValue = "0" + i.ToString();
                }
                else
                {
                    idxValue = i.ToString();
                }
                strValue = System.Globalization.DateTimeFormatInfo.CurrentInfo.GetMonthName(i);
                arr.Add(idxValue, strValue);
            }

            ListItem item;

            foreach (var str in arr)
            {
                item = new ListItem(str.Value, str.Key);
                ddl.Items.Add(item);
            }
            //string selMonth = DateTime.Now.Month.ToString();
            //if (selMonth.Length == 1)
            //{
            //    selMonth = "0" + selMonth;
            //}
            //ddl.SelectedValue = selMonth;
            item = new ListItem("", "");
            ddl.Items.Insert(0, item);
        }
        private void populateDayDropdown(DropDownList ddl)
        {
            ArrayList arr = new ArrayList();
            for (int i = 1; i <= 31; i++)
            {
                if (i.ToString().Length == 1)
                {
                    arr.Add("0" + i.ToString());
                }
                else
                {
                    arr.Add(i.ToString());
                }
            }
            ListItem item;

            foreach (string str in arr)
            {
                item = new ListItem(str, str);
                ddl.Items.Add(item);
            }
            //string selDay = DateTime.Now.Day.ToString();
            //if (selDay.Length == 1)
            //{
            //    selDay = "0" + selDay;
            //}
            //ddl.SelectedValue = selDay;
            item = new ListItem("", "");
            ddl.Items.Insert(0, item);
        }

        public bool isCancelCheckBoxVisible(int iStatus)
        {
            if (iStatus == (int)clsSwitch.enumSwitchStatus.Proposed)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
