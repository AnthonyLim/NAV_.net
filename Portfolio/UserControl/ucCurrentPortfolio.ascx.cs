using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NAV.Portfolio.UserControl
{
    public partial class ucCurrentPortfolio : System.Web.UI.UserControl
    {
        #region "properties"

        private String strClientID;
        public String propClientID { get { return strClientID; } set { strClientID = value; } }

        private String strPortfolioID;
        public String propPortfolioID { get { return strPortfolioID; } set { strPortfolioID = value; } }

        private String strUserID;
        public String propUserID { get { return strUserID; } set { strUserID = value; } }

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack) {
                clsPortfolio Portfolio = new clsPortfolio(this.propClientID, this.propPortfolioID, this.propUserID);
                populate(Portfolio.propPortfolioDetails);             
            }
        }

        private void populate(List<clsPortfolioDetails> listOriginalPortfolio)
        {
            this.gvPortfolioDetails.DataSource = listOriginalPortfolio;
            this.gvPortfolioDetails.DataBind();

            if (listOriginalPortfolio.Count != 0)
            { 
                Label lblgvFooterCurrentValueClient = (Label)this.gvPortfolioDetails.FooterRow.Cells[7].FindControl("gvFooterCurrentValueClient");
                lblgvFooterCurrentValueClient.Text = listOriginalPortfolio[0].propTotalCurrentValueClient.ToString("n0");

                Label lblgvHeaderPurchaseCostFundPortfolioCurrency = (Label)this.gvPortfolioDetails.HeaderRow.Cells[5].FindControl("gvHeaderPurchaseCostFundPortfolioCurrency");
                lblgvHeaderPurchaseCostFundPortfolioCurrency.Text = listOriginalPortfolio[0].propPortfolioCurrency.ToString();

                Label lblgvHeaderValueClientCurCurrency = (Label)this.gvPortfolioDetails.HeaderRow.Cells[7].FindControl("gvHeaderValueClientCurCurrency");
                lblgvHeaderValueClientCurCurrency.Text = listOriginalPortfolio[0].propClientCurrency.ToString();

                Label lblgvHeaderGainLossCurrency = (Label)this.gvPortfolioDetails.HeaderRow.Cells[6].FindControl("gvHeaderGainLossCurrency");
                lblgvHeaderGainLossCurrency.Text = listOriginalPortfolio[0].propPortfolioCurrency.ToString();
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