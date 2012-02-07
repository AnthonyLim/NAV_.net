using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;

namespace NAV
{
    public class clsPortfolio
    {
        private SqlConnection con = new clsSystem_DBConnection(clsSystem_DBConnection.strConnectionString.NavIntegrationDB).propConnection;

        #region properties

        private String strAccountNumber;
        public String propAccountNumber { get { return strAccountNumber; } set { strAccountNumber = value; } }

        private int intCompanyID;
        public int propCompanyID { get { return intCompanyID; } set { intCompanyID = value; } }

        private String strCompany;
        public String propCompany { get { return strCompany; } set { strCompany = value; } }

        private String strPortfolioCurrency;
        public String propPortfolioCurrency { get { return strPortfolioCurrency; } set { strPortfolioCurrency = value; } }

        private String strPortfolioType;
        public String propPortfolioType { get { return strPortfolioType; } set { strPortfolioType = value; } }

        private String strPortfolioID;
        public String propPortfolioID { get { return strPortfolioID; } set { strPortfolioID = value; } }

        private String strClientID;
        public String propClientID { get { return strClientID; } set { strClientID = value; } }

        private String strPortfolioTypeID;
        public String propPortfolioTypeID { get { return strPortfolioTypeID; } set { strPortfolioTypeID = value; } }

        private DateTime dtPortfolioStartDate;
        public DateTime propPortfolioStartDate { get { return dtPortfolioStartDate; } set { dtPortfolioStartDate = value; } }

        private String strPlanStatus;
        public String propPlanStatus { get { return strPlanStatus; } set { strPlanStatus = value; } }

        private String strLiquidity;
        public String propLiquidity { get { return strLiquidity; } set { strLiquidity = value; } }

        private String strRiskProfile;
        public String propRiskProfile { get { return strRiskProfile; } set { strRiskProfile = value; } }

        private String strRetentionTerm;
        public String propRetentionTerm { get { return strRetentionTerm; } set { strRetentionTerm = value; } }

        private Double dblMFPercent;
        public Double propMFPercent { get { return dblMFPercent; } set { dblMFPercent = value; } }

        private Boolean boolExcludeFromReports;
        public Boolean propExcludeFromReports { get { return boolExcludeFromReports; } set { boolExcludeFromReports = value; } }

        private Boolean boolClientGenerated;
        public Boolean propClientGenerated { get { return boolClientGenerated; } set { boolClientGenerated = value; } }

        private List<clsPortfolioDetails> listPortfolioDetails;
        public List<clsPortfolioDetails> propPortfolioDetails { get { return listPortfolioDetails; } }

        private clsSwitch Switch;
        public clsSwitch propSwitch { get { return Switch; } set { Switch = value; } }

        private clsSwitch_Client SwitchClient;
        public clsSwitch_Client propSwitchClient { get { return SwitchClient; } set { SwitchClient = value; } }

        #endregion

        public clsPortfolio(string strClientID , string strPortfolioID, string strUserId) {
            getPortfolioHeader(strClientID, strPortfolioID);
            this.listPortfolioDetails = getPortfolioDetails(strClientID, strPortfolioID);
            this.Switch = new clsSwitch(this, strUserId);
            this.SwitchClient = new clsSwitch_Client(propSwitch.propSwitchID);
        }
        public clsPortfolio(string strClientID, string strPortfolioID)
        {
            getPortfolioHeader(strClientID, strPortfolioID);
            this.listPortfolioDetails = getPortfolioDetails(strClientID, strPortfolioID);
        }
        private void getPortfolioHeader(string strClientID, string strPortfolioID) {

            SqlCommand cmd = new SqlCommand();
            SqlDataReader dr;
            con.Open();
            cmd.Connection = con;
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "[SWITCH_PortfolioHeaderGet]";

            cmd.Parameters.Add("@param_strClientID", System.Data.SqlDbType.NVarChar).Value = strClientID;
            cmd.Parameters.Add("@param_strPortfolioID", System.Data.SqlDbType.NVarChar).Value = strPortfolioID;
                                   
            dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                this.strAccountNumber = dr["AccountNumber"].ToString();
                this.intCompanyID = int.Parse(dr["CompanyID"].ToString());
                this.strCompany = dr["Company"].ToString();
                this.strPortfolioCurrency = dr["PortfolioCurrency"].ToString();
                this.strPortfolioType = dr["PortfolioType"].ToString();
                this.strPortfolioID = dr["PortfolioID"].ToString();
                this.strClientID = dr["ClientID"].ToString();
                this.strPortfolioTypeID = dr["PortfolioTypeID"].ToString();
                this.dtPortfolioStartDate = DateTime.Parse(dr["PortfolioStartDate"].ToString());
                this.strPlanStatus = dr["PlanStatus"].ToString();
                this.strLiquidity = dr["Liquidity"].ToString();
                this.strRiskProfile = dr["RiskProfile"].ToString();
                this.strRetentionTerm = dr["RetentionTerm"].ToString();
                this.dblMFPercent = dr["MFPercent"] == null ? Double.Parse(dr["MFPercent"].ToString()) : 0.0;
                this.boolExcludeFromReports = Boolean.Parse(dr["ExcludeFromReports"].ToString());
                this.boolClientGenerated = Boolean.Parse(dr["ClientGenerated"].ToString());

            }

            dr.Close();
            con.Close();
            cmd.Dispose();
            //con.Dispose();

        }

        private List<clsPortfolioDetails> getPortfolioDetails(string strClientID, string strPortfolioID)
        {
            List<clsPortfolioDetails> listPortfolioDetails = new List<clsPortfolioDetails>();

            SqlCommand cmd = new SqlCommand();
            SqlDataReader dr;
            con.Open();
            cmd.Connection = con;
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "[SWITCH_PortfolioDetailsGet]";

            cmd.Parameters.Add("@param_strClientID", System.Data.SqlDbType.NVarChar).Value = strClientID;
            cmd.Parameters.Add("@param_strPortfolioID", System.Data.SqlDbType.NVarChar).Value = strPortfolioID;

            dr = cmd.ExecuteReader();

            while (dr.Read())
            {

                clsPortfolioDetails newClsPortfolioDetails = new clsPortfolioDetails();

                newClsPortfolioDetails.propClientID = dr["ClientID"].ToString();
                newClsPortfolioDetails.propPortfolioID = dr["PortfolioID"].ToString();
                newClsPortfolioDetails.propPortfolioStartDate = DateTime.Parse(dr["PortfolioStartDate"].ToString());
                newClsPortfolioDetails.propCompany = dr["Company"].ToString();
                newClsPortfolioDetails.propFundManagerWeb = dr["FundManagerWeb"] != System.DBNull.Value ? dr["FundManagerWeb"].ToString() : "";
                newClsPortfolioDetails.propCompanyID = int.Parse(dr["CompanyID"].ToString());
                newClsPortfolioDetails.propNameOfFund = dr["NameOfFund"].ToString();
                newClsPortfolioDetails.propFundNameID = int.Parse(dr["FundNameID"].ToString());
                newClsPortfolioDetails.propSector = dr["Sector"].ToString();
                newClsPortfolioDetails.propSectorID = int.Parse(dr["SectorID"].ToString());
                newClsPortfolioDetails.propDataDate = dr["DataDate"] != System.DBNull.Value ? DateTime.Parse(dr["DataDate"].ToString()) : DateTime.ParseExact("01/01/1800", "dd/MM/yyyy", null);
                newClsPortfolioDetails.propClientCurrency = dr["ClientCurrency"].ToString();
                newClsPortfolioDetails.propPortfolioCurrency = dr["PortfolioCurrency"].ToString();
                newClsPortfolioDetails.propNumberOfUnits = float.Parse(dr["NumberOfUnits"].ToString());
                newClsPortfolioDetails.propPrice = float.Parse(dr["Price"].ToString());
                newClsPortfolioDetails.propSEDOL = dr["SEDOL"].ToString();
                newClsPortfolioDetails.propPurchaseCostFund = dr["PurchaseCostFund"] != System.DBNull.Value ? float.Parse(dr["PurchaseCostFund"].ToString()) : 0f;
                newClsPortfolioDetails.propFundCurrency = dr["FundCurrency"].ToString();
                newClsPortfolioDetails.propPurchaseCostPortfolio = dr["PurchaseCostPortfolio"] != System.DBNull.Value ? float.Parse(dr["PurchaseCostPortfolio"].ToString()) : 0f;
                newClsPortfolioDetails.propCurrentValuePortfolio = float.Parse(dr["CurrentValuePortfolio"].ToString());
                newClsPortfolioDetails.propCurrentValueClient = float.Parse(dr["CurrentValueClient"].ToString());
                newClsPortfolioDetails.propAllocationPercent = dr["AllocationPercent"] != System.DBNull.Value ? float.Parse(Math.Round(double.Parse(dr["AllocationPercent"].ToString()), 2).ToString()) : 0f;
                newClsPortfolioDetails.propGainOrLossPercent = float.Parse(dr["GainOrLossPercent"].ToString());
                newClsPortfolioDetails.propGainOrLossPortfolio = dr["GainOrLossPortfolio"] != System.DBNull.Value ? float.Parse(dr["GainOrLossPortfolio"].ToString()) : 0f;
                newClsPortfolioDetails.propPortfolioType = dr["PortfolioType"].ToString();
                newClsPortfolioDetails.propPortfolioTypeID = int.Parse(dr["PortfolioTypeID"].ToString());
                newClsPortfolioDetails.propAccountNumber = dr["AccountNumber"].ToString();
                newClsPortfolioDetails.propDatePriceUpdated = dr["DatePriceUpdated"] != System.DBNull.Value ? DateTime.Parse(dr["DatePriceUpdated"].ToString()) : DateTime.ParseExact("01/01/1800", "dd/MM/yyyy", null);
                newClsPortfolioDetails.propClientGenerated = Boolean.Parse(dr["ClientGenerated"].ToString());
                newClsPortfolioDetails.propFundID = dr["FundID"].ToString();
                newClsPortfolioDetails.propExcludeFromReports = Boolean.Parse(dr["ExcludeFromReports"].ToString());
                newClsPortfolioDetails.propOLDeleted = Boolean.Parse(dr["OLDeleted"].ToString());
                newClsPortfolioDetails.propPlanStatus = dr["PlanStatus"].ToString();
                newClsPortfolioDetails.propfundcode = dr["fundcode"].ToString();
                newClsPortfolioDetails.propType = dr["Type"].ToString();
                newClsPortfolioDetails.propTYPECODE = dr["TYPECODE"].ToString();
                newClsPortfolioDetails.propPortfolioDataCreatedDate = dr["PortfolioDataCreatedDate"] != System.DBNull.Value ? DateTime.Parse(dr["PortfolioDataCreatedDate"].ToString()) : DateTime.ParseExact("01/01/1800", "dd/MM/yyyy", null);
                //newClsPortfolioDetails.propModelWeightingPercentage = dr["ModelWeightingPercentage"] != null ? float.Parse(dr["ModelWeightingPercentage"].ToString()) : 0f;
                newClsPortfolioDetails.propLiquidity = dr["Liquidity"].ToString();
                newClsPortfolioDetails.propRiskProfile = dr["RiskProfile"].ToString();
                newClsPortfolioDetails.propRetentionTerm = dr["RetentionTerm"].ToString();
                newClsPortfolioDetails.propMFPercent = dr["MFPercent"] != System.DBNull.Value ? float.Parse(dr["MFPercent"].ToString()) : 0f;
                newClsPortfolioDetails.propTotalCurrentValueClient = float.Parse(Math.Round(double.Parse(dr["TotalCurrentValueClient"].ToString()), 0).ToString());
                newClsPortfolioDetails.propSwitchIFAPermit = dr["SwitchIFAPermit"] != System.DBNull.Value ? bool.Parse(dr["SwitchIFAPermit"].ToString()) : false; 


            listPortfolioDetails.Add(newClsPortfolioDetails);

            }
            dr.Close();
            con.Close();
            cmd.Dispose();
            con.Dispose();

            return listPortfolioDetails;
        }

    }
}    