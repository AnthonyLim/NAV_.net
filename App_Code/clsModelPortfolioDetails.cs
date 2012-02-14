using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;

namespace NAV
{
    public class clsModelPortfolioDetails
    {
        SqlConnection con = new clsSystem_DBConnection(clsSystem_DBConnection.strConnectionString.NavIntegrationDB).propConnection;

        #region Properties

        private string strModelGroupID;
        private string strModelPortfolioID;
        private int intFundID;
        private clsFund Fund;
        private decimal dUnits;
        private float fValue;
        private float fTotalValue;
        private float fAllocation;
        private float fTotalAllocation;
        private bool bIsDeletable;
        private float fCurrencyMultiplier;

        public string propModelGroupID { get { return strModelGroupID; } set { strModelGroupID = value; } }
        public string propModelPortfolioID { get { return strModelPortfolioID; } set { strModelPortfolioID = value; } }
        public int propFundID { get { return intFundID; } set { intFundID = value; } }
        public clsFund propFund { get { return Fund; } set { Fund = value; } }
        public decimal propUnits { get { return dUnits; } set { dUnits = value; } }
        public float propValue { get { return fValue; } set { fValue = value; } }
        public float propTotalValue { get { return fTotalValue; } set { fTotalValue = value; } }
        public float propAllocation { get { return fAllocation; } set { fAllocation = value; } }
        public float propTotalAllocation { get { return fTotalAllocation; } set { fTotalAllocation = value; } }
        public bool propIsDeletable { get { return bIsDeletable; } set { bIsDeletable = value; } }
        public float propCurrencyMultiplier { get { return fCurrencyMultiplier; } set { fCurrencyMultiplier = value; } }

        #endregion

        public clsModelPortfolioDetails() { }

        //public clsModelPortfolioDetails(clsPortfolio Portfolio, string strModelGroupID, string strModolPortfolioID) 
        //{
        //    getModelPortfolioDetails(Portfolio, strModelGroupID, strModolPortfolioID);
        //}

        public static List<clsModelPortfolioDetails> getModelPortfolioDetails(clsPortfolio Portfolio, string strModelGroupID, string strModolPortfolioID)
        {

            SqlConnection con = new clsSystem_DBConnection(clsSystem_DBConnection.strConnectionString.NavIntegrationDB).propConnection;
            List<clsModelPortfolioDetails> listModelPortfolioDetails = new List<clsModelPortfolioDetails>();

            SqlCommand cmd = new SqlCommand();
            SqlDataReader dr1;

            //con1.Open();
            con.Open();
            cmd.Connection = con;//con1;
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "[SWITCH_ModelPortfolioDetailsGet]";

            cmd.Parameters.Add("@param_ModelGroupID", System.Data.SqlDbType.NVarChar).Value = strModelGroupID;
            cmd.Parameters.Add("@param_ModelPortfolioID", System.Data.SqlDbType.NVarChar).Value = strModolPortfolioID;

            dr1 = cmd.ExecuteReader();

            float fTotalAllocation = 0;

            while (dr1.Read())
            {

                clsModelPortfolioDetails ModelPortfolioDetails = new clsModelPortfolioDetails();

                ModelPortfolioDetails.propAllocation = float.Parse(Math.Round(double.Parse(dr1["Allocation"].ToString()), 2).ToString());
                ModelPortfolioDetails.propFund = new clsFund(int.Parse(dr1["FundID"].ToString()));
                ModelPortfolioDetails.propFundID = int.Parse(dr1["FundID"].ToString());
                ModelPortfolioDetails.propIsDeletable = dr1["isDeletable"].ToString().Equals("1") ? true : false;

                if (Portfolio.propPortfolioDetails[0].propClientCurrency != ModelPortfolioDetails.propFund.propCurrency)
                {
                    ModelPortfolioDetails.propUnits = clsSwitchDetails.computeUnits(ModelPortfolioDetails.propAllocation,
                                                                                  float.Parse(Math.Round(double.Parse(Portfolio.propPortfolioDetails[0].propTotalCurrentValueClient.ToString()), 0).ToString()),
                                                                                  clsCurrency.convertToClientCurrency(strModelGroupID, ModelPortfolioDetails.propFund.propPrice, ModelPortfolioDetails.propFund.propCurrency));
                }
                else
                {
                    ModelPortfolioDetails.propUnits = clsSwitchDetails.computeUnits(ModelPortfolioDetails.propAllocation,
                                                                                  float.Parse(Math.Round(double.Parse(Portfolio.propPortfolioDetails[0].propTotalCurrentValueClient.ToString()), 0).ToString()),
                                                                                  ModelPortfolioDetails.propFund.propPrice);
                }


                ModelPortfolioDetails.propCurrencyMultiplier = clsCurrency.getCurrencyMultiplier(strModelGroupID, ModelPortfolioDetails.propFund.propCurrency);
                ModelPortfolioDetails.propTotalValue = float.Parse(Math.Round(double.Parse(Portfolio.propPortfolioDetails[0].propTotalCurrentValueClient.ToString()), 0).ToString());
                ModelPortfolioDetails.propValue = clsSwitchDetails.computeValue(ModelPortfolioDetails.propAllocation, ModelPortfolioDetails.propTotalValue);


                fTotalAllocation = fTotalAllocation + ModelPortfolioDetails.propAllocation;
                ModelPortfolioDetails.propTotalAllocation = fTotalAllocation;

                listModelPortfolioDetails.Add(ModelPortfolioDetails);

            }
            //con1.Close();
            con.Close();
            cmd.Dispose();
            //con1.Dispose();
            con.Dispose();

            return listModelPortfolioDetails;
        }

        public static List<clsModelPortfolioDetails> replicatePortfolioDetails(List<clsPortfolioDetails> _clsPortfolioDetails)
        {

            List<clsModelPortfolioDetails> listModelPortfolioDetails = new List<clsModelPortfolioDetails>();
            float fTotalAllocation = 0;
            double dPrice = 0;
            string strIntegerPart = string.Empty;
            double dConvertedValue = 0;
            int intMarker = 0;
            foreach (clsPortfolioDetails PortfolioDetails in _clsPortfolioDetails)
            {
                clsModelPortfolioDetails _clsModelPortfolioDetails = new clsModelPortfolioDetails();
                _clsModelPortfolioDetails.propFund = new clsFund(PortfolioDetails.propFundNameID);
                _clsModelPortfolioDetails.propAllocation = float.Parse(Math.Round(double.Parse(PortfolioDetails.propAllocationPercent.ToString()), 2).ToString());

                if (PortfolioDetails.propClientCurrency != PortfolioDetails.propFundCurrency)
                {
                    dConvertedValue = clsCurrency.convertToClientCurrency(_clsPortfolioDetails[0].propClientID, PortfolioDetails.propPrice, PortfolioDetails.propFundCurrency);
                    intMarker = dConvertedValue.ToString().IndexOf('.');
                    strIntegerPart = dConvertedValue.ToString().Substring(0, intMarker);

                    string strDecimalPart = dConvertedValue.ToString().Substring(intMarker, 4);
                    dPrice = Convert.ToDouble(strIntegerPart + strDecimalPart);
                }
                else
                {
                    dPrice = Math.Round(PortfolioDetails.propPrice, 4);
                }

                _clsModelPortfolioDetails.propCurrencyMultiplier = clsCurrency.getCurrencyMultiplier(_clsPortfolioDetails[0].propClientID, PortfolioDetails.propFundCurrency);
                _clsModelPortfolioDetails.propTotalValue = float.Parse(Math.Round(double.Parse(PortfolioDetails.propTotalCurrentValueClient.ToString()), 0).ToString());

                fTotalAllocation = fTotalAllocation + _clsModelPortfolioDetails.propAllocation;
                _clsModelPortfolioDetails.propTotalAllocation = fTotalAllocation;
                _clsModelPortfolioDetails.propIsDeletable = false;

                _clsModelPortfolioDetails.propValue = float.Parse(((Math.Round(_clsModelPortfolioDetails.propAllocation, 2) / 100) * int.Parse(PortfolioDetails.propTotalCurrentValueClient.ToString())).ToString());
                _clsModelPortfolioDetails.propUnits = Convert.ToDecimal((((Math.Round(_clsModelPortfolioDetails.propAllocation, 2) / 100) * int.Parse(PortfolioDetails.propTotalCurrentValueClient.ToString())) / dPrice));

                listModelPortfolioDetails.Add(_clsModelPortfolioDetails);
            }
            return listModelPortfolioDetails;
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
            else
            {
                dblUnits = ((decimal.Parse(fPercentAllocation.ToString()) / 100) * decimal.Parse(fTotalValue.ToString())) / decimal.Parse(fPrice.ToString());
            }
            return dblUnits;
        }

        public static float computeValue(float fPercentAllocation, float fTotalValue)
        {
            float fValue;

            fValue = (fPercentAllocation / 100) * fTotalValue;

            return fValue;
        }

        public static List<clsModelPortfolioDetails> AllocationChange(int intFundID, float fAllocation, List<clsModelPortfolioDetails> listModelPortfolioDetails, string strClientID, string strClientCurrency)
        {
            List<clsModelPortfolioDetails> newListModelPortfolioDetails = new List<clsModelPortfolioDetails>();
            float fTotalAllocation = 0;
            foreach (clsModelPortfolioDetails ModelPortfolioDetails in listModelPortfolioDetails)
            {
                clsFund FundInfo = new clsFund(intFundID);

                if (ModelPortfolioDetails.propFund.propFundID == intFundID)
                {
                    ModelPortfolioDetails.propAllocation = fAllocation;

                    if (strClientCurrency != FundInfo.propCurrency)
                    {
                        ModelPortfolioDetails.propUnits = computeUnits(fAllocation, ModelPortfolioDetails.propTotalValue, clsCurrency.convertToClientCurrency(strClientID, FundInfo.propPrice, FundInfo.propCurrency));
                    }
                    else
                    {
                        ModelPortfolioDetails.propUnits = computeUnits(fAllocation, ModelPortfolioDetails.propTotalValue, FundInfo.propPrice);
                    }

                    ModelPortfolioDetails.propValue = computeValue(fAllocation, ModelPortfolioDetails.propTotalValue);
                }

                fTotalAllocation = fTotalAllocation + ModelPortfolioDetails.propAllocation;

                ModelPortfolioDetails.propTotalAllocation = fTotalAllocation;

                ModelPortfolioDetails.propCurrencyMultiplier = clsCurrency.getCurrencyMultiplier(strClientID, FundInfo.propCurrency);

                newListModelPortfolioDetails.Add(ModelPortfolioDetails);
            }

            return newListModelPortfolioDetails;
        }

        public static List<clsModelPortfolioDetails> FundChange(int intFundID_old, int intFundID_new, List<clsModelPortfolioDetails> listModelPortfolioDetails, string strClientID, string strClientCurrency)
        {
            List<clsModelPortfolioDetails> newListModelPortfolioDetails = new List<clsModelPortfolioDetails>();
            float fTotalAllocation = 0;
            float fTotalValue = listModelPortfolioDetails[0].propTotalValue;

            foreach (clsModelPortfolioDetails ModelPortfolioDetails in listModelPortfolioDetails)
            {
                if (ModelPortfolioDetails.propFund.propFundID == intFundID_new)
                {
                    throw new Exception("Fund already exists.");
                }

                if (ModelPortfolioDetails.propFund.propFundID == intFundID_old)
                {

                    clsFund FundInfo = new clsFund(intFundID_new);

                    ModelPortfolioDetails.propFund = FundInfo;

                    if (strClientCurrency != FundInfo.propCurrency)
                    {
                        ModelPortfolioDetails.propUnits = computeUnits(ModelPortfolioDetails.propAllocation, fTotalValue, clsCurrency.convertToClientCurrency(strClientID, FundInfo.propPrice, FundInfo.propCurrency));
                    }
                    else
                    {
                        ModelPortfolioDetails.propUnits = computeUnits(ModelPortfolioDetails.propAllocation, fTotalValue, FundInfo.propPrice);
                    }

                    ModelPortfolioDetails.propValue = computeValue(ModelPortfolioDetails.propAllocation, fTotalValue);

                    ModelPortfolioDetails.propCurrencyMultiplier = clsCurrency.getCurrencyMultiplier(strClientID, FundInfo.propCurrency);
                }

                fTotalAllocation = fTotalAllocation + ModelPortfolioDetails.propAllocation;

                ModelPortfolioDetails.propTotalAllocation = fTotalAllocation;

                newListModelPortfolioDetails.Add(ModelPortfolioDetails);
            }

            return newListModelPortfolioDetails;
        }
        public void saveModelPortfolioDetails(List<clsModelPortfolioDetails> listModelPortfolioDetails, int intIFA_ID, string strModelGroupID, string strModelPortfolioID)
        {
            con.Open();
            foreach (clsModelPortfolioDetails ModelPortfolioDetails in listModelPortfolioDetails)
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;

                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "[SWITCH_ModelPortfolioDetailsInsertUpdate]";

                cmd.Parameters.Add("@param_IFA_ID", System.Data.SqlDbType.Int).Value = intIFA_ID;
                cmd.Parameters.Add("@param_ModelPortfolioID", System.Data.SqlDbType.NVarChar).Value = strModelPortfolioID;
                cmd.Parameters.Add("@param_ModelGroupID", System.Data.SqlDbType.NVarChar).Value = strModelGroupID;
                cmd.Parameters.Add("@param_FundID", System.Data.SqlDbType.Int).Value = ModelPortfolioDetails.propFund.propFundID;
                cmd.Parameters.Add("@param_Allocation", System.Data.SqlDbType.Float).Value = ModelPortfolioDetails.propAllocation;
                cmd.Parameters.Add("@param_IsDeletable", System.Data.SqlDbType.SmallInt).Value = ModelPortfolioDetails.propIsDeletable;

                cmd.ExecuteNonQuery();
            }
        con.Close();
        }
        public void deleteFundInTable(List<clsModelPortfolioDetails> listModelPortfolioDetailsNew, List<clsModelPortfolioDetails> listModelPortfolioDetailsPrev)
        {
            
            foreach (clsModelPortfolioDetails item in listModelPortfolioDetailsPrev)
            {
                if (listModelPortfolioDetailsNew.Exists(p => p.propFundID != item.propFundID))
                {
                }
            }
        }
        //public static void insertSwitchDetails(List<clsSwitchDetails> listSwitchDetails, string strUserID, int intSwitchID)
        //{
        //    SqlConnection con = new clsSystem_DBConnection(clsSystem_DBConnection.strConnectionString.NavIntegrationDB).propConnection;
        //    con.Open();

        //    foreach (clsSwitchDetails SwitchDetail in listSwitchDetails)
        //    {
        //        SqlCommand cmd = new SqlCommand();
        //        cmd.Connection = con;

        //        cmd.CommandType = System.Data.CommandType.StoredProcedure;
        //        cmd.CommandText = "[SWITCH_DetailsInsert]";

        //        cmd.Parameters.Add("@param_intSwitchID", System.Data.SqlDbType.Int).Value = intSwitchID;
        //        cmd.Parameters.Add("@param_intFundID", System.Data.SqlDbType.Int).Value = SwitchDetail.propFund.propFundID;
        //        cmd.Parameters.Add("@param_fAllocation", System.Data.SqlDbType.Float).Value = SwitchDetail.propAllocation;
        //        cmd.Parameters.Add("@param_strCreated_By", System.Data.SqlDbType.NVarChar).Value = strUserID;
        //        cmd.Parameters.Add("@param_strUpdated_By", System.Data.SqlDbType.NVarChar).Value = strUserID;
        //        cmd.Parameters.Add("@param_intSwitchDetailsID", System.Data.SqlDbType.Int).Value = SwitchDetail.intSwitchDetailsID;
        //        cmd.Parameters.Add("@param_sintIsDeletable", System.Data.SqlDbType.SmallInt).Value = SwitchDetail.propIsDeletable == true ? 1 : 0;

        //        cmd.ExecuteNonQuery();
        //    }
        //}

        public static void deleteModelPortfolioDetails(int intIFA_ID, string strModelGroupID, string strModelPortfolioID)
        {
            SqlConnection con = new clsSystem_DBConnection(clsSystem_DBConnection.strConnectionString.NavIntegrationDB).propConnection;
            con.Open();

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;

            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "[SWITCH_ModelPortfolioDetailsDelete]";

            cmd.Parameters.Add("@param_IFA_ID", System.Data.SqlDbType.Int).Value = intIFA_ID;
            cmd.Parameters.Add("@param_ModelGroupID", System.Data.SqlDbType.NVarChar).Value = strModelGroupID;
            cmd.Parameters.Add("@param_ModelPortfolioID", System.Data.SqlDbType.NVarChar).Value = strModelPortfolioID;

            cmd.ExecuteNonQuery();

            con.Close();
        }

        public static List<clsModelPortfolioDetails> removeModelPortfolioDetails(int intFundID, List<clsModelPortfolioDetails> listModelPortfolioDetails)
        {
            List<clsModelPortfolioDetails> newListModelPortfolioDetails = new List<clsModelPortfolioDetails>();

            foreach (clsModelPortfolioDetails ModelPortfolioDetails in listModelPortfolioDetails)
            {
                if (ModelPortfolioDetails.propFund.propFundID != intFundID)
                {
                    newListModelPortfolioDetails.Add(ModelPortfolioDetails);
                }
            }

            return newListModelPortfolioDetails;
        }

        /*
        public clsModelPortfolioDetails() { }

        public clsModelPortfolioDetails(string strModelGroupID)
        {
            getModelPortfolioDetailsInfo();
        }
        private void getModelPortfolioDetailsInfo()
        {
            SqlCommand cmd = new SqlCommand();
            SqlDataReader dr;

            con.Open();
            cmd.Connection = con;
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "SWITCH_ModelPortfolioDetailsGet";

            cmd.Parameters.Add("@param_ModelGroupID", System.Data.SqlDbType.NVarChar).Value = "";
            cmd.Parameters.Add("@param_ModelPortfolioID", System.Data.SqlDbType.NVarChar).Value = "";

            dr = cmd.ExecuteReader();

            while (dr.Read())
            {

            }

            cmd.Connection.Close();
            cmd.Dispose();
            con.Close();
        }
        
         */
    }
}
