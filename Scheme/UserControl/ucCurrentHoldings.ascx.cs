using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NAV.Scheme.UserControl
{
    public partial class ucCurrentHoldings : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {

                String strClientID = Session[clsSystem_Session.strSession.clientID.ToString()].ToString();
                String strSchemeID = Session[clsSystem_Session.strSession.tempschemeid.ToString()].ToString();

                populate(strClientID, strSchemeID);
            }
        }

        private void populate(String strClientID, String strSchemeID)
        {
            clsScheme Scheme = new clsScheme(strClientID, strSchemeID);

            this.gvCurrentHoldings.DataSource = Scheme.propDetails;
            this.gvCurrentHoldings.DataBind();

            if (Scheme.propDetails.Count != 0)
            {
                Label lblgvFooterValueFundCur = (Label)this.gvCurrentHoldings.FooterRow.Cells[3].FindControl("gvFooterValueFundCur");
                lblgvFooterValueFundCur.Text = Scheme.propSC_TotalValue.ToString("n0");

                Label lblgvFooterCurrentValueClient = (Label)this.gvCurrentHoldings.FooterRow.Cells[4].FindControl("gvFooterCurrentValueClient");
                lblgvFooterCurrentValueClient.Text = Scheme.propCC_TotalValue.ToString("n0");

                Label lblgvHeaderClientCurrency = (Label)this.gvCurrentHoldings.HeaderRow.Cells[4].FindControl("gvHeaderClientCurrency");
                lblgvHeaderClientCurrency.Text = Scheme.propClient.propCurrency.ToString();
            }
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
    }
}