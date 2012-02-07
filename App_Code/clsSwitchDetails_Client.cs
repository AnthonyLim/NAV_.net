using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;

namespace NAV
{
    public class clsSwitchDetails_Client
    {
        private SqlConnection con = new clsSystem_DBConnection(clsSystem_DBConnection.strConnectionString.NavIntegrationDB).propConnection;

        #region properties

        private int intSwitchDetailsID;
        public int propSwitchDetailsID { get { return intSwitchDetailsID; } set { intSwitchDetailsID = value; } }

        private int intSwitchID;
        public int propSwitchID { get { return intSwitchID; } set { intSwitchID = value; } }

        private int intFundID;
        public int propFundID { get { return intFundID; } set { intFundID = value; } }

        private float fAllocation;
        public float propAllocation { get { return fAllocation; } set { fAllocation = value; } }

        private DateTime dtDate_Created;
        public DateTime propDate_Created { get { return dtDate_Created; } set { dtDate_Created = value; } }

        private string strCreated_By;
        public string propCreated_By { get { return strCreated_By; } set { strCreated_By = value; } }

        private DateTime dtDate_LastUpdate;
        public DateTime propDate_LastUpdate { get { return dtDate_LastUpdate; } set { dtDate_LastUpdate = value; } }

        private string strUpdated_By;
        public string propUpdated_By { get { return strUpdated_By; } set { strUpdated_By = value; } }

        private clsFund Fund;
        public clsFund propFund { get { return Fund; } set { Fund = value; } }

        private decimal dUnits;
        public decimal propUnits { get { return dUnits; } set { dUnits = value; } }

        private float fValue;
        public float propValue { get { return fValue; } set { fValue = value; } }

        private float fTotalValue;
        public float propTotalValue { get { return fTotalValue; } set { fTotalValue = value; } }

        private float fTotalAllocation;
        public float propTotalAllocation { get { return fTotalAllocation; } set { fTotalAllocation = value; } }

        private bool isDeletable;
        public bool propIsDeletable { get { return isDeletable; } set { isDeletable = value; } }

        private float fCurrencyMultiplier;
        public float propCurrencyMultiplier { get { return fCurrencyMultiplier; } set { fCurrencyMultiplier = value; } }


        #endregion

        public clsSwitchDetails_Client() { }        
        
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

            //throw new Exception(("(" + fPercentAllocation.ToString() + " /  100) * " + fTotalValue.ToString()) + " / " + decimal.Parse(fPrice.ToString()) + ")");
            dblUnits = ((decimal.Parse(fPercentAllocation.ToString()) / 100) * decimal.Parse(fTotalValue.ToString())) / decimal.Parse(fPrice.ToString());

            //if (fPercentAllocation == 11.01f)
            //{
            //    //throw new Exception(fPrice.ToString());
            //    throw new Exception(("(" + decimal.Parse(fPercentAllocation.ToString()) + " /  100) * " + decimal.Parse(fTotalValue.ToString())) + " / " + decimal.Parse(fPrice.ToString()) + " = " + dblUnits + ")");
            //}


            return dblUnits; //Math.Round(dblUnits, 4);
        }

        public static float computeValue(float fPercentAllocation, float fTotalValue) {
            float fValue;

            fValue = (fPercentAllocation / 100) * fTotalValue;

            return fValue;
        }

        public static List<clsSwitchDetails_Client> AllocationChange(int intFundID, float fAllocation, List<clsSwitchDetails_Client> listSwitchDetails, string strClientID, string strClientCurrency )
        {
            List<clsSwitchDetails_Client> newListSwitchDetails = new List<clsSwitchDetails_Client>();
            float fTotalAllocation = 0;
            foreach (clsSwitchDetails_Client SwitchDetail in listSwitchDetails)
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

        public static List<clsSwitchDetails_Client> FundChange(int intFundID_old, int intFundID_new, List<clsSwitchDetails_Client> listSwitchDetails, string strClientID, string strClientCurrency)
        {
            List<clsSwitchDetails_Client> newListSwitchDetails = new List<clsSwitchDetails_Client>();
            float fTotalAllocation = 0;
            float fTotalValue = listSwitchDetails[0].propTotalValue;

            foreach (clsSwitchDetails_Client SwitchDetail in listSwitchDetails)
            {
                if (SwitchDetail.propFund.propFundID == intFundID_new) 
                {
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

        public static List<clsSwitchDetails_Client> addFund(int intNewFundID, List<clsSwitchDetails_Client> currentlistSwitchDetails, string strClientID, string strClientCurrency, string strUserID)
        {

            float fTotalAllocation = 0;
            float fTotalValue = currentlistSwitchDetails[0].propTotalValue;

            foreach (clsSwitchDetails_Client SwitchDetail in currentlistSwitchDetails)
            {
                fTotalAllocation = fTotalAllocation + SwitchDetail.propAllocation;

                if (SwitchDetail.propFund.propFundID == intNewFundID)
                {
                    throw new Exception("Fund already exists.");
                }
            }

            clsSwitchDetails_Client NewSwitchDetail = new clsSwitchDetails_Client();

            NewSwitchDetail.propCreated_By = strUserID;
            NewSwitchDetail.propFund = new clsFund(intNewFundID);
            NewSwitchDetail.propIsDeletable = true;
            NewSwitchDetail.propSwitchDetailsID = 0;
            //NewSwitchDetail.propSwitchScheme = NewSwitchDetail.propSwitchScheme;
            NewSwitchDetail.propTotalAllocation = fTotalAllocation;
            NewSwitchDetail.propCurrencyMultiplier = clsCurrency.getCurrencyMultiplier(strClientID, NewSwitchDetail.propFund.propCurrency);

            currentlistSwitchDetails.Add(NewSwitchDetail);

            return currentlistSwitchDetails;

        }

        public static List<clsSwitchDetails_Client> getSwitchDetails(int intSwitchID)
        {

            SqlConnection con1 = new clsSystem_DBConnection(clsSystem_DBConnection.strConnectionString.NavIntegrationDB).propConnection;
            List<clsSwitchDetails_Client> listSwitchDetails = new List<clsSwitchDetails_Client>();

            clsSwitch IFASwitch = new clsSwitch(intSwitchID);
            clsPortfolio Portfolio = new clsPortfolio(IFASwitch.propClientID.ToString(), IFASwitch.propPortfolioID);

            //throw new Exception(Portfolio.propPortfolioDetails.Count.ToString());
            //throw new Exception(intSwitchID.ToString() + " - " + IFASwitch.propPortfolioID);

            SqlCommand cmd = new SqlCommand();
            SqlDataReader dr1;

            con1.Open();
            cmd.Connection = con1;
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "[SWITCHclient_DetailsGet]";

            cmd.Parameters.Add("@param_intSwitchID", System.Data.SqlDbType.Int).Value = intSwitchID;

            dr1 = cmd.ExecuteReader();

            if (!dr1.HasRows) {                                
                SqlConnection con2 = new clsSystem_DBConnection(clsSystem_DBConnection.strConnectionString.NavIntegrationDB).propConnection;
                SqlCommand cmd2 = new SqlCommand();
                cmd2.Connection = con2;
                cmd2.CommandType = System.Data.CommandType.StoredProcedure;
                cmd2.CommandText = "[SWITCH_DetailsGet]";
                cmd2.Parameters.Add("@param_intSwitchID", System.Data.SqlDbType.Int).Value = intSwitchID;
                con2.Open();
                dr1 = cmd2.ExecuteReader();
            }

            float fTotalAllocation = 0;
            
            while (dr1.Read())
            {

                clsSwitchDetails_Client newClsSwitchDetails = new clsSwitchDetails_Client();

                newClsSwitchDetails.propAllocation = float.Parse(Math.Round(double.Parse(dr1["Allocation"].ToString()), 2).ToString());
                newClsSwitchDetails.propCreated_By = dr1["Created_By"].ToString();
                newClsSwitchDetails.propDate_Created = DateTime.Parse(dr1["Date_Created"].ToString());
                newClsSwitchDetails.propDate_LastUpdate = DateTime.Parse(dr1["Date_LastUpdate"].ToString());
                newClsSwitchDetails.propFund = new clsFund(int.Parse(dr1["FundID"].ToString()));
                newClsSwitchDetails.propFundID = int.Parse(dr1["FundID"].ToString());
                newClsSwitchDetails.propSwitchDetailsID = int.Parse(dr1["SwitchDetailsID"].ToString());
                newClsSwitchDetails.propSwitchID = int.Parse(dr1["SwitchID"].ToString());
                newClsSwitchDetails.propUpdated_By = dr1["Updated_By"].ToString();
                newClsSwitchDetails.propIsDeletable = dr1["isDeletable"].ToString().Equals("1") ? true : false;

                //if (Portfolio.propPortfolioDetails[0].propClientCurrency != newClsSwitchDetails.propFund.propCurrency)
                //{
                    newClsSwitchDetails.propUnits = clsSwitchDetails.computeUnits(newClsSwitchDetails.propAllocation,
                                                                                  float.Parse(Math.Round(double.Parse(Portfolio.propPortfolioDetails[0].propTotalCurrentValueClient.ToString()), 0).ToString()),
                                                                                  clsCurrency.convertToClientCurrency(IFASwitch.propClientID, newClsSwitchDetails.propFund.propPrice, newClsSwitchDetails.propFund.propCurrency));
                //}
                //else
                //{
                //    newClsSwitchDetails.propUnits = clsSwitchDetails.computeUnits(newClsSwitchDetails.propAllocation,
                //                                                                  float.Parse(Math.Round(double.Parse(Portfolio.propPortfolioDetails[0].propTotalCurrentValueClient.ToString()), 0).ToString()),
                //                                                                  newClsSwitchDetails.propFund.propPrice);
                //}


                newClsSwitchDetails.propCurrencyMultiplier = clsCurrency.getCurrencyMultiplier(IFASwitch.propClientID, newClsSwitchDetails.propFund.propCurrency);
                //newClsSwitchDetails.propValue = clsSwitchDetails.computeValue(newClsSwitchDetails.propAllocation, Portfolio.propPortfolioDetails[0].propTotalCurrentValueClient);
                newClsSwitchDetails.propTotalValue = float.Parse(Math.Round(double.Parse(Portfolio.propPortfolioDetails[0].propTotalCurrentValueClient.ToString()), 0).ToString());
                newClsSwitchDetails.propValue = clsSwitchDetails.computeValue(newClsSwitchDetails.propAllocation, newClsSwitchDetails.propTotalValue);


                fTotalAllocation = fTotalAllocation + newClsSwitchDetails.propAllocation;
                newClsSwitchDetails.propTotalAllocation = fTotalAllocation;

                listSwitchDetails.Add(newClsSwitchDetails);

            }
            con1.Close();
            cmd.Dispose();
            con1.Dispose();

            return listSwitchDetails;
        }

        public static void insertSwitchDetails(List<clsSwitchDetails_Client> listSwitchDetails, string strUserID, int intSwitchID)
        {
            SqlConnection con = new clsSystem_DBConnection(clsSystem_DBConnection.strConnectionString.NavIntegrationDB).propConnection;
            con.Open();

            foreach (clsSwitchDetails_Client SwitchDetail in listSwitchDetails)
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;

                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "[SWITCHclient_DetailsInsert]";

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
            cmd.CommandText = "[SWITCHclient_DetailsDelete]";

            cmd.Parameters.Add("@param_intSwitchDetailsID", System.Data.SqlDbType.Int).Value = intSwitchDetailsID;

            cmd.ExecuteNonQuery();

            con.Close();
        }

        public static List<clsSwitchDetails_Client> removeSwitchDetails(int intFundID, List<clsSwitchDetails_Client> listSwitchDetails)
        {
            List<clsSwitchDetails_Client> newListSwitchDetails = new List<clsSwitchDetails_Client>();

            foreach (clsSwitchDetails_Client SwitchDetail in listSwitchDetails)
            {                                 
                if (SwitchDetail.propFund.propFundID != intFundID)
                {
                    newListSwitchDetails.Add(SwitchDetail);
                }
            }

            return newListSwitchDetails;
        }        

    }
}
