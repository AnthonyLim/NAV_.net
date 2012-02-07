using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;

namespace NAV
{
    public class clsScheme
    {
        private SqlConnection con = new clsSystem_DBConnection(clsSystem_DBConnection.strConnectionString.NavIntegrationDB).propConnection;

        #region "properties"

        private string strSchemeID;
        public string propSchemeID {get {return strSchemeID;} set {strSchemeID = value;}}

        private clsClient Client;
        public clsClient propClient {get {return Client;} set {Client = value;}}

        private clsCompany Company;
        public clsCompany propCompany {get { return Company;} set {Company = value; }}

        private string strAccountNumber;
        public string propAccountNumber {get { return strAccountNumber;} set { strAccountNumber = value;}}

        private Boolean isClientGenerated;
        public Boolean propIsClientGenerated {get {return isClientGenerated;} set {isClientGenerated = value;}}

        private string strPortfolioType;
        public string propPortfolioType {get { return strPortfolioType;} set { strPortfolioType = value;}}

        private string strSchemeCurrency;
        public string propSchemeCurrency {get { return strSchemeCurrency;} set { strSchemeCurrency = value;}}

        private float fContributionTotal;
        public float propContributionTotal {get { return fContributionTotal;} set { fContributionTotal = value;}}

        private Boolean isExcludeFromReports;
        public Boolean propIsExcludeFromReports {get { return isExcludeFromReports;} set { isExcludeFromReports = value;}}

        private DateTime dtStartDate;
        public DateTime propStartDate {get {return dtStartDate;} set {dtStartDate = value;}}

        private DateTime dtMaturityDate;
        public DateTime propMaturityDate {get {return dtMaturityDate;} set {dtMaturityDate= value;}}

        private float fSumAssured;
        public float propSumAssured {get {return fSumAssured;} set {fSumAssured = value;}}

        private string strPlanStatus;
        public string propPlanStatus {get { return strPlanStatus;} set { strPlanStatus = value;}}

        private string strLiquidity;
        public string propLiquidity {get { return strLiquidity;} set { strLiquidity = value;}}

        private string strRiskProfile;
        public string propRiskProfile {get {return strRiskProfile;} set {strRiskProfile = value;}}

        private string strRetentionTerm;
        public string propRetentionTerm {get {return strRetentionTerm;} set {strRetentionTerm = value;}}

        private float fMFPercent;
        public float propMFPercent {get {return fMFPercent;}set {fMFPercent = value;} }

        private float fSC_TotalValue;
        public float propSC_TotalValue {get { return fSC_TotalValue; } set { fSC_TotalValue = value; }}

        private float fCC_TotalValue;
        public float propCC_TotalValue { get { return fCC_TotalValue; } set { fCC_TotalValue = value; } }

        private float fWithdrawalTotal;
        public float propWithdrawalTotal {get { return fWithdrawalTotal; }set { fWithdrawalTotal = value; }}
        
        public float propGainLoss { get { return (propSC_TotalValue + propWithdrawalTotal) - propContributionTotal; }}

        public float propGainLossPercent {get { return (propGainLoss/propContributionTotal) * 100;}}

        private List<clsDetails> listDetails;
        public List<clsDetails> propDetails {get { return listDetails; } set { listDetails = value; }}

        private List<clsContribution> listContribution;
        public List<clsContribution> propContributions { get { return listContribution; } set { listContribution = value; } }

        private bool boolSwitchIFAPermit;
        public bool propSwitchIFAPermit { get { return boolSwitchIFAPermit; } set { boolSwitchIFAPermit = value; } }

        #endregion

        #region "constructors"

        public clsScheme() { }

        public clsScheme(string strClientID, string strSchemeID)
        {
            getHeader(strClientID, strSchemeID);
        }

        #endregion

        private void getHeader(string strClientID, string strSchemeID) {
            SqlCommand cmd = new SqlCommand();
            SqlDataReader dr;

            con.Open();
            cmd.Connection = con;
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "SWITCHScheme_HeaderGetOriginal";

            cmd.Parameters.Add("@param_strClientID", System.Data.SqlDbType.NVarChar).Value = strClientID;
            cmd.Parameters.Add("@param_strSchemeID", System.Data.SqlDbType.NVarChar).Value = strSchemeID;
            dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                this.propSchemeID = dr["SchemeID"] != System.DBNull.Value ? dr["SchemeID"].ToString() : "";
                this.propClient = new clsClient(strClientID);
                this.propCompany = dr["CompanyID"] != System.DBNull.Value ? new clsCompany(int.Parse(dr["CompanyID"].ToString())): null;
                this.propAccountNumber = dr["AccountNumber"] != System.DBNull.Value ? dr["AccountNumber"].ToString() : "";
                this.propIsClientGenerated = dr["ClientGenerated"] != System.DBNull.Value ? Boolean.Parse(dr["ClientGenerated"].ToString()) : false;
                this.propPortfolioType = dr["PortfolioType"] != System.DBNull.Value ? dr["PortfolioType"].ToString() : "";
                this.propSchemeCurrency = dr["SchemeCurrency"] != System.DBNull.Value ? dr["SchemeCurrency"].ToString() : "";
                this.propContributionTotal = dr["ContributionTotal"] != System.DBNull.Value ? float.Parse(dr["ContributionTotal"].ToString())  : 0;
                this.propIsExcludeFromReports = dr["ExcludeFromReports"] != System.DBNull.Value ? Boolean.Parse(dr["ExcludeFromReports"].ToString()) : false;
                this.propStartDate  = dr["StartDate"] != System.DBNull.Value ? DateTime.Parse(dr["StartDate"].ToString()) : DateTime.ParseExact("01/01/1800", "dd/MM/yyyy", null);
                this.propMaturityDate = dr["MaturityDate"] != System.DBNull.Value ? DateTime.Parse(dr["MaturityDate"].ToString()) : DateTime.ParseExact("01/01/1800", "dd/MM/yyyy", null);
                this.propSumAssured = dr["SumAssured"] != System.DBNull.Value ? float.Parse(dr["SumAssured"].ToString())  : 0;
                this.propPlanStatus = dr["PlanStatus"] != System.DBNull.Value ? dr["PlanStatus"].ToString() : "";
                this.propLiquidity = dr["Liquidity"] != System.DBNull.Value ? dr["Liquidity"].ToString() : "";
                this.propRiskProfile = dr["RiskProfile"] != System.DBNull.Value ? dr["RiskProfile"].ToString() : "";
                this.propRetentionTerm = dr["RetentionTerm"] != System.DBNull.Value ? dr["RetentionTerm"].ToString() : "";
                this.propMFPercent = dr["MFPercent"] != System.DBNull.Value ? float.Parse(dr["MFPercent"].ToString())  : 0;
                this.propSC_TotalValue = dr["SC_TotalValue"] != System.DBNull.Value ? float.Parse(dr["SC_TotalValue"].ToString()) : 0;
                this.propCC_TotalValue = dr["CC_TotalValue"] != System.DBNull.Value ? float.Parse(dr["CC_TotalValue"].ToString()) : 0;
                this.propWithdrawalTotal = dr["WithdrawalTotal"] != System.DBNull.Value ? float.Parse(dr["WithdrawalTotal"].ToString()) : 0;
                this.propDetails = clsDetails.getListOriginalDetails(this);
                this.propSwitchIFAPermit = dr["SwitchIFAPermit"] != System.DBNull.Value ? bool.Parse(dr["SwitchIFAPermit"].ToString()) : false; 
            }
            con.Close();
            cmd.Dispose();
            con.Dispose();
        }

        public class clsDetails
        {

            #region "constructors"

            public clsDetails() { }

            public clsDetails(clsScheme Scheme)
            {
                getListOriginalDetails(Scheme);
            }

            #endregion

            #region "properties"

            private clsScheme Scheme;
            public clsScheme propScheme {get { return Scheme; }set { Scheme = value; }}

            private clsClient Client;
            public clsClient propClient {get { return Client;} set { Client = value;}}

            private clsFund Fund;
            public clsFund propFund {get { return Fund;} set { Fund = value;}}

            private float fUnits;
            public float propUnits {get { return fUnits;}set { fUnits = value;}}

            private float fValue;
            public float propValue {get { return fValue;} set { fValue = value;}}

            private float fCurrentValueClient;
            public float propCurrentValueClient {get { return fCurrentValueClient;} set { fCurrentValueClient = value;}}

            private float fCurrentValueScheme;
            public float propCurrentValueScheme {get { return fCurrentValueScheme;} set { fCurrentValueScheme = value;}}

            private string strClientCurrency;
            public string propClientCurrency {get { return strClientCurrency;} set { strClientCurrency = value;}}

            private float fFundExchangeRate;
            public float propFundExchangeRate {get { return fFundExchangeRate;}set { fFundExchangeRate = value;}}

            private string strSEDOL;
            public string propSEDOL {get { return strSEDOL;} set { strSEDOL = value;}}

            private float fAllocation;
            public float propAllocation {get { return fAllocation;} set { fAllocation = value;}}

            private float fExchangeRate;
            public float propExchangeRate {get { return fExchangeRate;} set { fExchangeRate = value;}}

            private Boolean isOLDeleted;
            private Boolean propIsOLDeleted {get { return isOLDeleted;} set { isOLDeleted = value;}}

            #endregion            

            public static List<clsDetails> getListOriginalDetails(clsScheme Scheme)
            {

                string strClientID = Scheme.propClient.propClientID;
                string strSchemeID = Scheme.propSchemeID;

                SqlConnection con = new clsScheme().con;
                SqlCommand cmd = new SqlCommand();
                SqlDataReader dr;

                con.Open();
                cmd.Connection = con;
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "SWITCHScheme_DetailsGetOriginal";

                cmd.Parameters.Add("@param_strClientID", System.Data.SqlDbType.NVarChar).Value = strClientID;
                cmd.Parameters.Add("@param_strSchemeID", System.Data.SqlDbType.NVarChar).Value = strSchemeID;

                dr = cmd.ExecuteReader();

                List<clsDetails> newListDetails = new List<clsDetails>();

                while (dr.Read())
                {
                    clsDetails newDetails = new clsDetails();

                                                           
                    newDetails.propClient = dr["ClientID"] != System.DBNull.Value ? new clsClient(dr["ClientID"].ToString()) : null;
                    newDetails.propClientCurrency = newDetails.propClient.propCurrency;
                    newDetails.propCurrentValueClient = dr["CurrentValueClient"] != System.DBNull.Value ? float.Parse(dr["CurrentValueClient"].ToString()) : 0;
                    newDetails.propCurrentValueScheme = dr["CurrentValueScheme"] != System.DBNull.Value ? float.Parse(dr["CurrentValueScheme"].ToString()) : 0;
                    newDetails.propExchangeRate = dr["ExchangeRate"] != System.DBNull.Value ? float.Parse(dr["ExchangeRate"].ToString()) : 0;
                    newDetails.propFund = dr["FundNameID"] != System.DBNull.Value ? new clsFund(int.Parse(dr["FundNameID"].ToString())) : null;
                    newDetails.propFundExchangeRate = dr["FundExchangeRate"] != System.DBNull.Value ? float.Parse(dr["FundExchangeRate"].ToString()) : 0;
                    newDetails.propIsOLDeleted = dr["OLDeleted"] != System.DBNull.Value ? Boolean.Parse(dr["OLDeleted"].ToString()) : false;
                    newDetails.propScheme = Scheme;
                    newDetails.propSEDOL = dr["SEDOL"] != System.DBNull.Value ? dr["SEDOL"].ToString() : "";
                    newDetails.propUnits = dr["NumberOfUnits"] != System.DBNull.Value ? float.Parse(dr["NumberOfUnits"].ToString()) : 0;
                    newDetails.propValue = dr["Value"] != System.DBNull.Value ? float.Parse(dr["Value"].ToString()) : 0;
                    newDetails.propAllocation = (newDetails.propCurrentValueScheme / Scheme.propSC_TotalValue) * 100;                    
                    
                    newListDetails.Add(newDetails);
                }

                con.Close();
                cmd.Dispose();

                return newListDetails;
            }

        }

        public class clsContribution { 

            #region "properties"

            private clsClient Client;
            public clsClient propClient {get { return Client;} set { Client = value;}}

            private clsScheme Scheme;
            public clsScheme propScheme {get { return Scheme; }set { Scheme = value; }}

            private string strContributionID;
            public string propContributionID {get {return strContributionID;} set {strContributionID = value;}}

            private DateTime dtStartDate;
            public DateTime propStartDate {get {return dtStartDate;} set {dtStartDate = value;}}

            private DateTime dtEndDate;
            public DateTime propEndDate {get {return dtEndDate;} set {dtEndDate = value;}}

            private float fContributionAmount;
            public float propContributionAmount {get { return fContributionAmount;} set { fContributionAmount = value;}}
            
            private string strValuationFrequency;
            public string propValuationFrequency {get {return strValuationFrequency;} set {strValuationFrequency = value;}}

            private DateTime dtSchemeContributionsUpdatedDate;
            public DateTime propSchemeContributionsUpdatedDate {get {return dtSchemeContributionsUpdatedDate;} set {dtSchemeContributionsUpdatedDate = value;}}

            private string strSchemeContributionsUpdatedBy;
            public string propSchemeContributionsUpdatedBy {get {return strSchemeContributionsUpdatedBy;} set {strSchemeContributionsUpdatedBy = value;}}

            private DateTime dtIFAUpdatedDate;
            public DateTime propIFAUpdatedDate {get {return dtIFAUpdatedDate;}set {dtIFAUpdatedDate = value;}}

            private string strIFAUpdatedBy;
            public string propIFAUpdatedBy{get {return strIFAUpdatedBy;}set {strIFAUpdatedBy = value;}}
       
        #endregion

            public clsContribution() { }

            public static List<clsContribution> getContributions(clsScheme Scheme) {
                

                SqlConnection con1 = new clsSystem_DBConnection(clsSystem_DBConnection.strConnectionString.NavIntegrationDB).propConnection;
                List<clsContribution> listContribution = new List<clsContribution>();

                SqlCommand cmd = new SqlCommand();
                SqlDataReader dr;

                con1.Open();
                cmd.Connection = con1;
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.CommandText = "[SWITCHScheme_SchemeContributionsGet]";

                cmd.Parameters.Add("@param_SchemeID", System.Data.SqlDbType.NVarChar).Value = Scheme.propSchemeID;

                dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    clsContribution newContribution = new clsContribution();

                    newContribution.propClient = new clsClient(dr["ClientID"].ToString());
                    newContribution.propContributionAmount = float.Parse(dr["ContributionAmount"].ToString());
                    newContribution.propContributionID = dr["ContributionID"].ToString();
                    newContribution.propEndDate = dr["EndDate"] != System.DBNull.Value ? DateTime.Parse(dr["EndDate"].ToString()) : DateTime.ParseExact("01/01/1800", "dd/MM/yyyy", null);                    
                    newContribution.propIFAUpdatedBy = dr["IFAUpdatedBy"].ToString();
                    newContribution.propIFAUpdatedDate = dr["IFAUpdatedDate"] != System.DBNull.Value ? DateTime.Parse(dr["IFAUpdatedDate"].ToString()) : DateTime.ParseExact("01/01/1800", "dd/MM/yyyy", null);                    
                    newContribution.propScheme = Scheme;
                    newContribution.propSchemeContributionsUpdatedBy = dr["SchemeContributionsUpdatedBy"].ToString();
                    newContribution.propSchemeContributionsUpdatedDate = dr["SchemeContributionsUpdatedDate"] != System.DBNull.Value ? DateTime.Parse(dr["SchemeContributionsUpdatedDate"].ToString()) : DateTime.ParseExact("01/01/1800", "dd/MM/yyyy", null);
                    newContribution.propStartDate = dr["StartDate"] != System.DBNull.Value ? DateTime.Parse(dr["StartDate"].ToString()) : DateTime.ParseExact("01/01/1800", "dd/MM/yyyy", null);
                    newContribution.propValuationFrequency = dr["ValuationFrequency"].ToString();

                    listContribution.Add(newContribution);
                }
                con1.Close();
                cmd.Dispose();
                con1.Dispose();

                return listContribution;
            }
        }

    }
}