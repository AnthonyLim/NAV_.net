using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;

namespace NAV
{
    public class clsSwitchDetails
    {
        private SqlConnection con = new clsSystem_DBConnection(clsSystem_DBConnection.strConnectionString.NavIntegrationDB).propConnection;

        #region properties

        private int intSwitchDetailsID;
        public int propSwitchDetailsID {get { return intSwitchDetailsID; }set { intSwitchDetailsID = value; }}

        private int intSwitchID;
        public int propSwitchID {get { return intSwitchID; } set { intSwitchID = value; }}

        private int intFundID;
        public int propFundID { get { return intFundID; } set { intFundID = value; } }

        private float fAllocation;
        public float propAllocation { get { return fAllocation; } set { fAllocation = value; }}

	    private DateTime dtDate_Created;
        public DateTime propDate_Created { get { return dtDate_Created; } set { dtDate_Created = value; }}

        private string strCreated_By;
        public string propCreated_By { get { return strCreated_By; } set { strCreated_By = value; } }

        private DateTime dtDate_LastUpdate;
        public DateTime propDate_LastUpdate { get { return dtDate_LastUpdate; } set { dtDate_LastUpdate = value; }}

	    private string strUpdated_By;
        public string propUpdated_By { get { return strUpdated_By; } set { strUpdated_By = value; } }

        private clsFund Fund;
        public clsFund propFund { get {return Fund; } set { Fund = value; }}

        private decimal dUnits;
        public decimal propUnits {get { return dUnits; } set { dUnits = value; }}

        private float fValue;
        public float propValue { get { return fValue; } set { fValue = value; }}

        private float fTotalValue;
        public float propTotalValue { get { return fTotalValue; } set { fTotalValue = value; } }

        private float fTotalAllocation;
        public float propTotalAllocation { get { return fTotalAllocation; } set { fTotalAllocation = value; } }

        private bool isDeletable;
        public bool propIsDeletable { get { return isDeletable; } set { isDeletable = value; }}

        private float fCurrencyMultiplier;
        public float propCurrencyMultiplier {get { return fCurrencyMultiplier; } set { fCurrencyMultiplier = value; }}


        #endregion

        public clsSwitchDetails() {}

        public static List<clsSwitchDetails> replicatePortfolioDetails(clsPortfolio _clsPortfolio)
        {
            List<clsSwitchDetails> listSwitchDetails = new List<clsSwitchDetails>();
            float fTotalAllocation = 0;
            double dPrice = 0;

            foreach (clsPortfolioDetails PortfolioDetails in _clsPortfolio.propPortfolioDetails)
            {
                clsSwitchDetails _clsSwitchDetails = new clsSwitchDetails();
                _clsSwitchDetails.propFund = new clsFund(PortfolioDetails.propFundNameID);
                _clsSwitchDetails.propAllocation = float.Parse(Math.Round(double.Parse(PortfolioDetails.propAllocationPercent.ToString()), 2).ToString());

                if (PortfolioDetails.propClientCurrency != PortfolioDetails.propFundCurrency)
                {
                    double dConvertedValue = clsCurrency.convertToClientCurrency(_clsPortfolio.propClientID, PortfolioDetails.propPrice, PortfolioDetails.propFundCurrency);
                    int intMarker = dConvertedValue.ToString().IndexOf('.');
                    string strIntegerPart = dConvertedValue.ToString().Substring(0, intMarker);
                    string strDecimalPart = dConvertedValue.ToString().Substring(intMarker, 4);
                    dPrice = Convert.ToDouble(strIntegerPart + strDecimalPart);
                }
                else
                {
                    dPrice = Math.Round(PortfolioDetails.propPrice, 4);
                }

                _clsSwitchDetails.propCurrencyMultiplier = clsCurrency.getCurrencyMultiplier(_clsPortfolio.propClientID, PortfolioDetails.propFundCurrency);
                _clsSwitchDetails.propTotalValue = float.Parse(Math.Round(double.Parse(PortfolioDetails.propTotalCurrentValueClient.ToString()), 0).ToString());

                fTotalAllocation = fTotalAllocation + _clsSwitchDetails.propAllocation;
                _clsSwitchDetails.propTotalAllocation = fTotalAllocation;
                _clsSwitchDetails.propIsDeletable = false;

                _clsSwitchDetails.propValue = float.Parse(((Math.Round(_clsSwitchDetails.propAllocation, 2) / 100) * int.Parse(Math.Round(_clsPortfolio.propTotalValue, 0).ToString())).ToString());
                _clsSwitchDetails.propUnits = Convert.ToDecimal((((Math.Round(_clsSwitchDetails.propAllocation, 2) / 100) * int.Parse(Math.Round(_clsPortfolio.propTotalValue, 0).ToString())) / dPrice));

                listSwitchDetails.Add(_clsSwitchDetails);
            }

            return listSwitchDetails;
          }

        public static List<clsSwitchDetails> replicatePortfolioDetails(List<clsPortfolioDetails> _clsPortfolioDetails, int intSwitchID)
        {
            List<clsSwitchDetails> listSwitchDetails = new List<clsSwitchDetails>();
            float fTotalAllocation = 0;
            double dPrice = 0;

            foreach (clsPortfolioDetails PortfolioDetails in _clsPortfolioDetails)
            {
                clsSwitchDetails _clsSwitchDetails = new clsSwitchDetails();
                _clsSwitchDetails.propSwitchID = intSwitchID;
                _clsSwitchDetails.propFund = new clsFund(PortfolioDetails.propFundNameID);
                _clsSwitchDetails.propAllocation = float.Parse(Math.Round(double.Parse(PortfolioDetails.propAllocationPercent.ToString()), 2).ToString());

                if (PortfolioDetails.propClientCurrency != PortfolioDetails.propFundCurrency)
                {
                    //_clsSwitchDetails.propUnits = computeUnits(PortfolioDetails.propAllocationPercent,
                    //                                            PortfolioDetails.propTotalCurrentValueClient,//float.Parse(PortfolioDetails.propTotalCurrentValueClient.ToString()), //float.Parse(Math.Round(double.Parse(PortfolioDetails.propTotalCurrentValueClient.ToString()), 0).ToString()),
                    //                                           clsCurrency.convertToClientCurrency(_clsPortfolioDetails[0].propClientID, PortfolioDetails.propPrice, PortfolioDetails.propFundCurrency));
                    double dConvertedValue = clsCurrency.convertToClientCurrency(_clsPortfolioDetails[0].propClientID, PortfolioDetails.propPrice, PortfolioDetails.propFundCurrency);
                    int intMarker = dConvertedValue.ToString().IndexOf('.');
                    string strIntegerPart = dConvertedValue.ToString().Substring(0, intMarker);
                    string strDecimalPart = dConvertedValue.ToString().Substring(intMarker, 4);
                    dPrice = Convert.ToDouble(strIntegerPart + strDecimalPart);
                }
                else
                {
                    //_clsSwitchDetails.propUnits = computeUnits(PortfolioDetails.propAllocationPercent,
                    //                                           PortfolioDetails.propTotalCurrentValueClient, //float.Parse(PortfolioDetails.propTotalCurrentValueClient.ToString()),//float.Parse(Math.Round(double.Parse(PortfolioDetails.propTotalCurrentValueClient.ToString()), 0).ToString()),
                    //                                           PortfolioDetails.propPrice);
                    dPrice = Math.Round(PortfolioDetails.propPrice, 4);
                }

                _clsSwitchDetails.propCurrencyMultiplier = clsCurrency.getCurrencyMultiplier(_clsPortfolioDetails[0].propClientID, PortfolioDetails.propFundCurrency);
                //_clsSwitchDetails.propValue = computeValue(PortfolioDetails.propAllocationPercent, PortfolioDetails.propTotalCurrentValueClient);
                _clsSwitchDetails.propTotalValue = float.Parse(Math.Round(double.Parse(PortfolioDetails.propTotalCurrentValueClient.ToString()), 0).ToString());
                //_clsSwitchDetails.propValue = computeValue(PortfolioDetails.propAllocationPercent, _clsSwitchDetails.propTotalValue);

                fTotalAllocation = fTotalAllocation + _clsSwitchDetails.propAllocation;
                _clsSwitchDetails.propTotalAllocation = fTotalAllocation;
                _clsSwitchDetails.propIsDeletable = false;

                _clsSwitchDetails.propValue = float.Parse(((Math.Round(_clsSwitchDetails.propAllocation, 2) / 100) * int.Parse(PortfolioDetails.propTotalCurrentValueClient.ToString())).ToString());
                _clsSwitchDetails.propUnits = Convert.ToDecimal((((Math.Round(_clsSwitchDetails.propAllocation, 2) / 100) * int.Parse(PortfolioDetails.propTotalCurrentValueClient.ToString())) / dPrice));

                listSwitchDetails.Add(_clsSwitchDetails);
            }

            return listSwitchDetails;
        }

        public static decimal computeUnits(float fPercentAllocation, float fTotalValue, float fPrice)
        {
            decimal dblUnits;
            string[] strPrice = fPrice.ToString().Split('.');
            string[] strTotalValue = fTotalValue.ToString().Split('.');

            fTotalValue = float.Parse(strTotalValue[0]);

            if (strPrice.Length > 1)
            {
                if (strPrice[1].Length < 3)
                {
                    strPrice[1] = strPrice[1] + "000";
                }

                string strPriceDecimal = strPrice[1].Substring(0, 3);
                string strNewPrice = strPrice[0] + "." + strPrice[1].Substring(0, 3);
                fPrice = float.Parse(strNewPrice);
            }
            if (decimal.Parse(fPrice.ToString()) == 0)
            {
                dblUnits = 0;
            }
            else {
                dblUnits = ((decimal.Parse(fPercentAllocation.ToString()) / 100) * decimal.Parse(fTotalValue.ToString())) / decimal.Parse(fPrice.ToString());
            }
            return dblUnits;
        }

        public static float computeValue(float fPercentAllocation, float fTotalValue) {
            float fValue;

            fValue = (fPercentAllocation / 100) * fTotalValue;

            return fValue;
        }

        public static List<clsSwitchDetails> AllocationChange(int intFundID, float fAllocation, List<clsSwitchDetails> listSwitchDetails, string strClientID, string strClientCurrency )
        {
            List<clsSwitchDetails> newListSwitchDetails = new List<clsSwitchDetails>();
            float fTotalAllocation = 0;
            foreach (clsSwitchDetails SwitchDetail in listSwitchDetails)
            {
                clsFund FundInfo = new clsFund(intFundID);                

                if (SwitchDetail.propFund.propFundID == intFundID) {
                    SwitchDetail.propAllocation = fAllocation;

                    if (strClientCurrency != FundInfo.propCurrency)
                    {                        
                        SwitchDetail.propUnits = computeUnits(fAllocation, SwitchDetail.propTotalValue, clsCurrency.convertToClientCurrency(strClientID, FundInfo.propPrice, FundInfo.propCurrency));
                    }
                    else {
                        SwitchDetail.propUnits = computeUnits(fAllocation, SwitchDetail.propTotalValue, FundInfo.propPrice);
                    }

                    SwitchDetail.propValue = computeValue(fAllocation, SwitchDetail.propTotalValue);                    
                }

                fTotalAllocation = fTotalAllocation + SwitchDetail.propAllocation;

                SwitchDetail.propTotalAllocation = fTotalAllocation;

                SwitchDetail.propCurrencyMultiplier = clsCurrency.getCurrencyMultiplier(strClientID, FundInfo.propCurrency);

                newListSwitchDetails.Add(SwitchDetail);
            }

            return newListSwitchDetails;
        }

        public static List<clsSwitchDetails> FundChange(int intFundID_old, int intFundID_new, List<clsSwitchDetails> listSwitchDetails, string strClientID, string strClientCurrency)
        {
            List<clsSwitchDetails> newListSwitchDetails = new List<clsSwitchDetails>();
            float fTotalAllocation = 0;
            float fTotalValue = listSwitchDetails[0].propTotalValue;

            foreach (clsSwitchDetails SwitchDetail in listSwitchDetails)
            {
                if (SwitchDetail.propFund.propFundID == intFundID_new) {
                    throw new Exception("Fund already exists.");
                }

                if (SwitchDetail.propFund.propFundID == intFundID_old)
                {

                    clsFund FundInfo = new clsFund(intFundID_new);

                    SwitchDetail.propFund = FundInfo;

                    if (strClientCurrency != FundInfo.propCurrency)
                    {
                        SwitchDetail.propUnits = computeUnits(SwitchDetail.propAllocation, fTotalValue, clsCurrency.convertToClientCurrency(strClientID, FundInfo.propPrice, FundInfo.propCurrency));
                    }
                    else
                    {
                        SwitchDetail.propUnits = computeUnits(SwitchDetail.propAllocation, fTotalValue, FundInfo.propPrice);
                    }

                    SwitchDetail.propValue = computeValue(SwitchDetail.propAllocation, fTotalValue);

                    SwitchDetail.propCurrencyMultiplier = clsCurrency.getCurrencyMultiplier(strClientID, FundInfo.propCurrency);
                }

                fTotalAllocation = fTotalAllocation + SwitchDetail.propAllocation;
                
                SwitchDetail.propTotalAllocation = fTotalAllocation;

                newListSwitchDetails.Add(SwitchDetail);
            }

            return newListSwitchDetails;
        }
        public static void insertSwitchDetails(int intSwitchID, string strClientID, string strPortfolioID)
        {
            SqlConnection con = new clsSystem_DBConnection(clsSystem_DBConnection.strConnectionString.NavIntegrationDB).propConnection;
            con.Open();


            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;

            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "[SWITCH_Customized_DetailsInsert]";

            cmd.Parameters.Add("@param_intSwitchID", System.Data.SqlDbType.Int).Value = intSwitchID;
            //cmd.Parameters.Add("@param_intFundID", System.Data.SqlDbType.Int).Value = SwitchDetail.propFund.propFundID;
            //cmd.Parameters.Add("@param_fAllocation", System.Data.SqlDbType.Float).Value = SwitchDetail.propAllocation;
            cmd.Parameters.Add("@param_strPortfolioID", System.Data.SqlDbType.NVarChar).Value = strPortfolioID;
            cmd.Parameters.Add("@param_strClientID", System.Data.SqlDbType.NVarChar).Value = strClientID;
            //cmd.Parameters.Add("@param_intSwitchDetailsID", System.Data.SqlDbType.Int).Value = SwitchDetail.intSwitchDetailsID;
            //cmd.Parameters.Add("@param_sintIsDeletable", System.Data.SqlDbType.SmallInt).Value = SwitchDetail.propIsDeletable == true ? 1 : 0;

            cmd.ExecuteNonQuery();
        }
        public static void insertSwitchDetails(List<clsSwitchDetails> listSwitchDetails, string strUserID, int intSwitchID)
        {
            SqlConnection con = new clsSystem_DBConnection(clsSystem_DBConnection.strConnectionString.NavIntegrationDB).propConnection;
            con.Open();

            foreach (clsSwitchDetails SwitchDetail in listSwitchDetails)
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;

                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "[SWITCH_DetailsInsert]";

                cmd.Parameters.Add("@param_intSwitchID", System.Data.SqlDbType.Int).Value = intSwitchID;
                cmd.Parameters.Add("@param_intFundID", System.Data.SqlDbType.Int).Value = SwitchDetail.propFund.propFundID;
                cmd.Parameters.Add("@param_fAllocation", System.Data.SqlDbType.Float).Value = SwitchDetail.propAllocation;
                cmd.Parameters.Add("@param_strCreated_By", System.Data.SqlDbType.NVarChar).Value = strUserID;
                cmd.Parameters.Add("@param_strUpdated_By", System.Data.SqlDbType.NVarChar).Value = strUserID;
                cmd.Parameters.Add("@param_intSwitchDetailsID", System.Data.SqlDbType.Int).Value = SwitchDetail.intSwitchDetailsID;
                cmd.Parameters.Add("@param_sintIsDeletable", System.Data.SqlDbType.SmallInt).Value = SwitchDetail.propIsDeletable == true ? 1: 0;

                cmd.ExecuteNonQuery();
            }
        }

        public static void removeSwitchDetails(int intSwitchDetailsID)
        {
            SqlConnection con = new clsSystem_DBConnection(clsSystem_DBConnection.strConnectionString.NavIntegrationDB).propConnection;
            con.Open();

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;

            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "[SWITCH_DetailsDelete]";

            cmd.Parameters.Add("@param_intSwitchDetailsID", System.Data.SqlDbType.Int).Value = intSwitchDetailsID;

            cmd.ExecuteNonQuery();

            con.Close();
        }

        public static List<clsSwitchDetails> removeSwitchDetails(int intFundID, List<clsSwitchDetails> listSwitchDetails)
        {
            List<clsSwitchDetails> newListSwitchDetails = new List<clsSwitchDetails>();

            foreach (clsSwitchDetails SwitchDetail in listSwitchDetails)
            {
                if (SwitchDetail.propFund.propFundID != intFundID)
                {
                    newListSwitchDetails.Add(SwitchDetail);
                }
            }

            return newListSwitchDetails;
        }

        public static void transferClientSwitchToIFA(List<clsSwitchDetails_Client> listClientSwitchDetails, string strUserID)
        {

            clsSwitch SwitchIFA = new clsSwitch(listClientSwitchDetails[0].propSwitchID);

            string strClientID = SwitchIFA.propClientID;
            string strPortfolioID = SwitchIFA.propPortfolioID;

            List<clsSwitchDetails> listSwitchDetailsIFA = new clsPortfolio(strClientID, strPortfolioID, strUserID).propSwitch.propSwitchDetails;

            foreach (clsSwitchDetails SwitchDetailsIFA in listSwitchDetailsIFA)
            {
                clsSwitchDetails.removeSwitchDetails(SwitchDetailsIFA.propSwitchDetailsID);
            }

            List<clsSwitchDetails> newListSwitchDetails = new List<clsSwitchDetails>();

            foreach (clsSwitchDetails_Client SwitchDetails_Client in listClientSwitchDetails)
            {
                clsSwitchDetails newSwitchDetails = new clsSwitchDetails();
                newSwitchDetails.propSwitchID = SwitchIFA.propSwitchID;
                newSwitchDetails.propFund = new clsFund(SwitchDetails_Client.propFund.propFundID);
                newSwitchDetails.propAllocation = SwitchDetails_Client.propAllocation;
                newSwitchDetails.propIsDeletable = SwitchDetails_Client.propIsDeletable;

                newListSwitchDetails.Add(newSwitchDetails);

            }

            clsSwitchDetails.insertSwitchDetails(newListSwitchDetails, strUserID, SwitchIFA.propSwitchID);
        }
    }


}