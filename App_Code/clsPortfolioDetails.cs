using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NAV
{
    public class clsPortfolioDetails
    {
        #region properties
        
        private string strClientID;
        public string propClientID { get { return strClientID; } set { strClientID = value; }}

        private string strPortfolioID;
        public string propPortfolioID { get { return strPortfolioID; } set { strPortfolioID = value; }}

        private DateTime dtPortfolioStartDate;
        public DateTime propPortfolioStartDate { get { return dtPortfolioStartDate; } set { dtPortfolioStartDate = value; }}

        private string strCompany;
        public string propCompany { get { return strCompany; } set { strCompany = value; }}
           
        private string strFundManagerWeb;
        public string propFundManagerWeb { get { return strFundManagerWeb; } set { strFundManagerWeb = value; }}

        private int intCompanyID;
        public int propCompanyID { get { return intCompanyID; } set { intCompanyID = value; }}

        private string strNameOfFund;
        public string propNameOfFund { get { return strNameOfFund; } set { strNameOfFund = value; } }

        private int intFundNameID;
        public int propFundNameID { get { return intFundNameID; } set { intFundNameID = value; }}

        private string strSector;
        public string propSector { get { return strSector; } set { strSector = value; } }

        private int intSectorID;
        public int propSectorID { get { return intSectorID; } set { intSectorID = value;}}

        private DateTime dtDataDate;
        public DateTime propDataDate { get { return dtDataDate; } set { dtDataDate = value; }}

        private string strClientCurrency;
        public string propClientCurrency {get { return strClientCurrency; } set { strClientCurrency = value; }}

        private string strPortfolioCurrency;
        public string propPortfolioCurrency { get { return strPortfolioCurrency; } set { strPortfolioCurrency = value; }}

        private float fNumberOfUnits;
        public float propNumberOfUnits {get { return fNumberOfUnits; } set { fNumberOfUnits = value; }}

        private float fPrice;
        public float propPrice { get { return fPrice; }set { fPrice = value; }}

        private string strSEDOL;
        public string propSEDOL {get { return strSEDOL; }set { strSEDOL = value; }}

        private float fPurchaseCostFund;
        public float propPurchaseCostFund {get { return fPurchaseCostFund; } set { fPurchaseCostFund = value; }}

        private string strFundCurrency;
        public string propFundCurrency {get { return strFundCurrency; } set { strFundCurrency = value; }}

        private float fPurchaseCostPortfolio;
        public float propPurchaseCostPortfolio {get { return fPurchaseCostPortfolio; } set { fPurchaseCostPortfolio = value; }}

        private float fCurrentValuePortfolio;
        public float propCurrentValuePortfolio {get { return fCurrentValuePortfolio; } set { fCurrentValuePortfolio = value; }}

        private float fCurrentValueClient;
        public float propCurrentValueClient { get { return fCurrentValueClient; } set { fCurrentValueClient = value; }}

        private float fAllocationPercent;
        public float propAllocationPercent {
            get {
                return (propCurrentValueClient / propTotalCurrentValueClient) * 100; 
            } 
            set { 
                fAllocationPercent = value; 
            }
        }

        private float fGainOrLossPercent;
        public float propGainOrLossPercent {get { return fGainOrLossPercent; } set { fGainOrLossPercent = value; }}

        private float fGainOrLossPortfolio;
        public float propGainOrLossPortfolio {get { return fGainOrLossPortfolio; }set { fGainOrLossPortfolio = value; }}

        private string strPortfolioType;
        public string propPortfolioType { get { return strPortfolioType; } set { strPortfolioType = value; }}

        private int intPortfolioTypeID;
        public int propPortfolioTypeID { get { return intPortfolioTypeID; } set { intPortfolioTypeID = value; }}

        private string strAccountNumber;
        public string propAccountNumber { get { return strAccountNumber; } set { strAccountNumber = value; }}

        private DateTime dtDatePriceUpdated;
        public DateTime propDatePriceUpdated { get { return dtDatePriceUpdated; } set { dtDatePriceUpdated = value; }}

        private Boolean boolClientGenerated;
        public Boolean propClientGenerated { get { return boolClientGenerated; } set { boolClientGenerated = value; } }

        private string strFundID;
        public string propFundID { get { return strFundID; } set { strFundID = value; }}

        private Boolean boolExcludeFromReports;
        public Boolean propExcludeFromReports { get { return boolExcludeFromReports; }set { boolExcludeFromReports = value; }}

        private Boolean boolOLDeleted;
        public Boolean propOLDeleted { get { return boolOLDeleted; }set { boolOLDeleted = value; }}

        private string strPlanStatus;
        public string propPlanStatus { get { return strPlanStatus; } set { strPlanStatus = value; }}

        private string strfundcode;
        public string propfundcode {get { return strfundcode; } set { strfundcode = value; }}

        private string strType;
        public string propType {get { return strType; } set { strType = value; }}

        private string strTYPECODE;
        public string propTYPECODE {get { return strTYPECODE; } set { strTYPECODE = value; }}

        private DateTime dtPortfolioDataCreatedDate;
        public DateTime propPortfolioDataCreatedDate { get { return dtPortfolioDataCreatedDate; } set { dtPortfolioDataCreatedDate = value; } }

        private float fModelWeightingPercentage;
        public float propModelWeightingPercentage { get { return fModelWeightingPercentage; } set { fModelWeightingPercentage = value; }}

        private string strLiquidity;
        public string propLiquidity {get { return strLiquidity; } set { strLiquidity = value; }}

        private string strRiskProfile;
        public string propRiskProfile {get { return strRiskProfile; } set { strRiskProfile = value; }}

        private string strRetentionTerm;
        public string propRetentionTerm {get { return strRetentionTerm; }set { strRetentionTerm = value; }}

        private float fMFPercent;
        public float propMFPercent {get { return fMFPercent; } set { fMFPercent = value; }}

        private float fTotalCurrentValueClient;
        public float propTotalCurrentValueClient { get { return fTotalCurrentValueClient; } set { fTotalCurrentValueClient = value; }}

        private bool boolSwitchIFAPermit;
        public bool propSwitchIFAPermit { get { return boolSwitchIFAPermit; } set { boolSwitchIFAPermit = value; } }

        #endregion

        public clsPortfolioDetails() { 

        }

    }
}