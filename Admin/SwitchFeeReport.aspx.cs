using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;

namespace NAV.Admin
{
    public partial class SwitchFeeReport : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                Session["SourcePage"] = "/report/ifa/searchindex.asp";

                List<clsIFA> oIFAList = clsIFA.getIFAList();
                populateIFAList(oIFAList);

                populateYearDropdown(ddlStartDateYear);
                populateYearDropdown(ddlEndDateYear);
                populateMonthDropdown(ddlStartDateMonth);
                populateMonthDropdown(ddlEndDateMonth);
                populateDayDropdown(ddlStartDateDay);
                populateDayDropdown(ddlEndDateDay);

                if (Request.QueryString["IFA"] != null)
                {
                    int intIFA = int.Parse(Request.QueryString["IFA"].ToString());

                    string[] arrStartDate = Request.QueryString["SDate"].ToString().Split('-');
                    string[] arrEndDate = Request.QueryString["EDate"].ToString().Split('-');
                    if ((arrStartDate.Length == 3) && (arrEndDate.Length == 3))
                    {
                        ddlStartDateYear.SelectedValue = arrStartDate[2].ToString();
                        ddlEndDateYear.SelectedValue = arrEndDate[2].ToString();
                        ddlStartDateMonth.SelectedValue = arrStartDate[1].ToString();
                        ddlEndDateMonth.SelectedValue = arrEndDate[1].ToString();
                        ddlStartDateDay.SelectedValue = arrStartDate[0].ToString();
                        ddlEndDateDay.SelectedValue = arrStartDate[0].ToString();
                    }
                    ddlIFAList.SelectedValue = intIFA.ToString();

                    List<clsSwitchFee> oSwitchFeeList = clsSwitchFee.computePerSwitchFeeReport(intIFA, Request.QueryString["SDate"].ToString(), Request.QueryString["EDate"].ToString());
                    populateSwitchFeeReport(oSwitchFeeList);
                }
            }
        }
        protected void btnViewReport_Click(object sender, EventArgs e)
        {
            string strStartDate = getStartDate();
            string strEndDate = getEndDate();

            List<clsSwitchFee> oSwitchFeeList = clsSwitchFee.computePerSwitchFeeReport(int.Parse(ddlIFAList.SelectedValue), strStartDate, strEndDate);
            populateSwitchFeeReport(oSwitchFeeList);
        }
        protected void lbtnSwitchDetails_Click(object sender, CommandEventArgs e)
        {
            string strStartDate = getStartDate();
            string strEndDate = getEndDate();
            string strSwitches = e.CommandArgument.ToString();
            Response.Redirect(string.Format("SwitchFeeReportList.aspx?SID={0}&IFA={1}&SDate={2}&EDate={3}", strSwitches, ddlIFAList.SelectedValue, strStartDate, strEndDate));
        }
        private void populateSwitchFeeReport(List<clsSwitchFee> oSwitchFeeList)
        {
            if (oSwitchFeeList.Count > 0)
            {
                this.lblIFA_Name_Value.Text = oSwitchFeeList[0].propIFA_Name;
            }
            this.panelSwitchFeeReport.Visible = true;
            this.gvSwitchFeeReport.DataSource = oSwitchFeeList;
            this.gvSwitchFeeReport.DataBind();
        }
        private void populateIFAList(List<clsIFA> oIFAList)
        {
            ddlIFAList.DataSource = oIFAList;
            ddlIFAList.DataValueField = "propIFA_ID";
            ddlIFAList.DataTextField = "propIFA_Name";
            ddlIFAList.DataBind();
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
            item = new ListItem("", "");
            ddl.Items.Insert(0, item);
        }
        private string getStartDate()
        {
            string strStartDate = string.Empty;
            if ((ddlStartDateDay.SelectedValue.Trim() + ddlStartDateMonth.SelectedValue.Trim() + ddlStartDateYear.SelectedValue.Trim()).Length > 0)
            {
                strStartDate = string.Format("{0}-{1}-{2}", ddlStartDateDay.SelectedValue.Trim(), ddlStartDateMonth.SelectedValue.Trim(), ddlStartDateYear.SelectedValue.Trim());
            }
            return strStartDate;
        }
        private string getEndDate()
        {
            string strEndDate = string.Empty;
            if ((ddlEndDateDay.SelectedValue.Trim() + ddlEndDateMonth.SelectedValue.Trim() + ddlEndDateYear.SelectedValue.Trim()).Length > 0)
            {
                strEndDate = string.Format("{0}-{1}-{2}", ddlEndDateDay.SelectedValue.Trim(), ddlEndDateMonth.SelectedValue.Trim(), ddlEndDateYear.SelectedValue.Trim());
            }
            return strEndDate;
        }
    }
}
