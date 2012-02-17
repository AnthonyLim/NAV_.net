using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;

namespace NAV
{
    public class clsSwitchTemp
    {
        private SqlConnection con = new clsSystem_DBConnection(clsSystem_DBConnection.strConnectionString.NavIntegrationDB).propConnection;

        #region properties

        private int intSwitchID;
        public int propSwitchID { get { return intSwitchID; } set { intSwitchID = value; } }

        private int intModelID;
        public int propModelID { get { return intModelID; } set { intModelID = value; } }

        private string strModelGroupID;
        public string propModelGroupID { get { return strModelGroupID; } set { strModelGroupID = value; } }

        private string strModelPortfolioID;
        public string propModelPortfolioID { get { return strModelPortfolioID; } set { strModelPortfolioID = value; } }

        private string strPortfolioID;
        public string propPortfolioID { get { return strPortfolioID; } set { strPortfolioID = value; } }

        private string strClientID;
        public string propClientID { get { return strClientID; } set { strClientID = value; } }

        private Int16 sintStatus;
        public Int16 propStatus { get { return sintStatus; } set { sintStatus = value; } }

        private string strStatus = "Draft";
        public string propStatusString { get { return strStatus; } set { strStatus = value; } }

        private DateTime dtDate_Created;
        public DateTime propDate_Created { get { return dtDate_Created; } set { dtDate_Created = value; } }

        private string strCreated_By;
        public string propCreated_By { get { return strCreated_By; } set { strCreated_By = value; } }

        private string strDescription = string.Empty;
        public string propDescription { get { return strDescription; } set { strDescription = value; } }

        private clsClient oClient;
        public clsClient propSwitchClient { get { return oClient; } set { oClient = value; } }

        private clsPortfolio oPortfolio;
        public clsPortfolio propPortfolio { get { return oPortfolio; } set { oPortfolio = value; } }

        private List<clsSwitchDetails> listSwitchDetails;
        public List<clsSwitchDetails> propSwitchDetails { get { return listSwitchDetails; } set { listSwitchDetails = value; } }

        #endregion


        public clsSwitchTemp(clsPortfolio Portfolio, string strUserID, int intIFA_ID, int intModelID, string strModelGroupID, string strModelPortfolioID)
        {
            getSwitchInfo(Portfolio, strUserID, intIFA_ID, intModelID, strModelGroupID, strModelPortfolioID);
        }

        //public clsSwitchTemp(int intSwitchID)
        //{
        //    getHeaderInfo(intSwitchID);
        //}

        public clsSwitchTemp() { }

        //private void getHeaderInfo(int intSwitchID)
        //{
        //    SqlCommand cmd = new SqlCommand();
        //    SqlDataReader dr;
        //    con.Open();
        //    cmd.Connection = con;
        //    cmd.CommandType = System.Data.CommandType.StoredProcedure;
        //    cmd.CommandText = "[SWITCH_Temp_HeaderGet]";
           
        //    cmd.Parameters.Add("@param_intSwitchID", System.Data.SqlDbType.Int).Value = intSwitchID;

        //    dr = cmd.ExecuteReader();

        //    if (dr.HasRows)
        //    {

        //        while (dr.Read())
        //        {

        //            this.propClientID = dr["ClientID"].ToString();
        //            this.propDate_Created = DateTime.Parse(dr["Date_Created"].ToString());
        //            this.propPortfolioID = dr["PortfolioID"].ToString();
        //            this.propStatus = short.Parse(dr["Status"].ToString());
        //            //this.propStatusString = getSwitchStringStatus(this.propStatus);
        //            this.propSwitchID = int.Parse(dr["SwitchID"].ToString());
        //            this.propCreated_By = dr["Created_By"].ToString();
        //            this.propDescription = dr["Description"].ToString();
        //        }
        //    }

        //    dr.Close();
        //    con.Close();
        //    cmd.Dispose();
        //    con.Dispose();
        //}

        private void getSwitchInfo(clsPortfolio Portfolio, string strUserID, int intIFA_ID, int _intModelID, string strModelGroupID, string strModelPortfolioID)
        {
            SqlCommand cmd = new SqlCommand();
            SqlDataReader dr;
            con.Open();
            cmd.Connection = con;
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "[SWITCH_Temp_HeaderGet]";

            cmd.Parameters.Add("@param_intIFA_ID", System.Data.SqlDbType.Int).Value = intIFA_ID;
            cmd.Parameters.Add("@param_ModelGroupID", System.Data.SqlDbType.NVarChar).Value = strModelGroupID;
            cmd.Parameters.Add("@param_ModelPortfolioID", System.Data.SqlDbType.NVarChar).Value = strModelPortfolioID;
            cmd.Parameters.Add("@param_strPortfolioID", System.Data.SqlDbType.NVarChar).Value = Portfolio.propPortfolioID;
            cmd.Parameters.Add("@param_strClientID", System.Data.SqlDbType.NVarChar).Value = Portfolio.propClientID;

            dr = cmd.ExecuteReader();

            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    this.propClientID = dr["ClientID"].ToString();
                    this.propDate_Created = DateTime.Parse(dr["Date_Created"].ToString());
                    this.propPortfolioID = dr["PortfolioID"].ToString();
                    //this.propStatus = short.Parse(dr["Status"].ToString());
                    //this.propStatusString = clsSwitch.getSwitchStringStatus(this.propStatus);
                    this.propSwitchID = 77;//int.Parse(dr["SwitchID"].ToString());
                    //if (propStatus == (int)enumSwitchStatus.Declined_Client)
                    //{
                    //    this.propSwitchDetails = clsSwitchDetails.replicatePortfolioDetails(Portfolio);
                    //}
                    //else
                    //{
                    this.propSwitchDetails = this.getSwitchDetails(Portfolio, Portfolio.propClientID, Portfolio.propPortfolioID);
                    //}
                    this.propCreated_By = dr["Created_By"].ToString();
                    this.propDescription = dr["Description"].ToString();
                }
            }
            else
            {
                this.propSwitchID = 0;
                this.propPortfolioID = Portfolio.propPortfolioID;
                this.propClientID = Portfolio.propClientID;
                this.propStatus = (short)clsSwitch.enumSwitchStatus.Saved;
                this.propStatusString = clsSwitch.getSwitchStringStatus(this.propStatus);
                this.propCreated_By = strUserID;
                this.propSwitchDetails = this.combineModelAndPortfolioDetails(Portfolio, _intModelID, Portfolio.propModelGroupID, Portfolio.propModelPortfolioID);
            }

            dr.Close();
            con.Close();
            cmd.Dispose();
            //con.Dispose();
        }
        //SwitchHeaderTemp
        public List<clsSwitchDetails> getSwitchDetails(clsPortfolio _clsPortfolio, string strClientID, string strPortfolioID)
        {
            SqlConnection con1 = new clsSystem_DBConnection(clsSystem_DBConnection.strConnectionString.NavIntegrationDB).propConnection;
            List<clsSwitchDetails> listSwitchDetails = new List<clsSwitchDetails>();

            SqlCommand cmd = new SqlCommand();
            SqlDataReader dr1;

            con1.Open();
            cmd.Connection = con1;
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "[SWITCH_Temp_DetailsGet]";

            cmd.Parameters.Add("@param_strClientID", System.Data.SqlDbType.NVarChar).Value = strClientID;
            cmd.Parameters.Add("@param_strPortfolioID", System.Data.SqlDbType.NVarChar).Value = strPortfolioID;

            dr1 = cmd.ExecuteReader();

            float fTotalAllocation = 0;

            while (dr1.Read())
            {

                clsSwitchDetails newClsSwitchDetails = new clsSwitchDetails();

                newClsSwitchDetails.propAllocation = float.Parse(Math.Round(double.Parse(dr1["Allocation"].ToString()), 2).ToString());
                newClsSwitchDetails.propCreated_By = dr1["Created_By"].ToString();
                newClsSwitchDetails.propDate_Created = DateTime.Parse(dr1["Date_Created"].ToString());
                newClsSwitchDetails.propDate_LastUpdate = DateTime.Parse(dr1["Date_Updated"].ToString());
                newClsSwitchDetails.propFund = new clsFund(int.Parse(dr1["FundID"].ToString()));
                newClsSwitchDetails.propFundID = int.Parse(dr1["FundID"].ToString());
                //newClsSwitchDetails.propSwitchDetailsID = int.Parse(dr1["SwitchDetailsID"].ToString());
                //newClsSwitchDetails.propSwitchID = int.Parse(dr1["SwitchID"].ToString());
                newClsSwitchDetails.propUpdated_By = dr1["Updated_By"].ToString();
                newClsSwitchDetails.propIsDeletable = dr1["isDeletable"].ToString().Equals("1") ? true : false;

                //if (Portfolio.propPortfolioDetails[0].propClientCurrency != newClsSwitchDetails.propFund.propCurrency)
                if (_clsPortfolio.propClient.propCurrency != newClsSwitchDetails.propFund.propCurrency)
                {
                    newClsSwitchDetails.propUnits = clsSwitchDetails.computeUnits(newClsSwitchDetails.propAllocation,
                                                                                  float.Parse(Math.Round(double.Parse(_clsPortfolio.propPortfolioDetails[0].propTotalCurrentValueClient.ToString()), 0).ToString()),
                                                                                  clsCurrency.convertToClientCurrency(_clsPortfolio.propClientID, newClsSwitchDetails.propFund.propPrice, newClsSwitchDetails.propFund.propCurrency));
                }
                else
                {
                    newClsSwitchDetails.propUnits = clsSwitchDetails.computeUnits(newClsSwitchDetails.propAllocation,
                                                                                  float.Parse(Math.Round(double.Parse(_clsPortfolio.propPortfolioDetails[0].propTotalCurrentValueClient.ToString()), 0).ToString()),
                                                                                  newClsSwitchDetails.propFund.propPrice);
                }


                newClsSwitchDetails.propCurrencyMultiplier = clsCurrency.getCurrencyMultiplier(_clsPortfolio.propClientID, newClsSwitchDetails.propFund.propCurrency);
                //newClsSwitchDetails.propValue = clsSwitchDetails.computeValue(newClsSwitchDetails.propAllocation, Portfolio.propPortfolioDetails[0].propTotalCurrentValueClient);
                newClsSwitchDetails.propTotalValue = float.Parse(Math.Round(double.Parse(_clsPortfolio.propPortfolioDetails[0].propTotalCurrentValueClient.ToString()), 0).ToString());
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
        public List<clsSwitchDetails> combineModelAndPortfolioDetails(clsPortfolio _clsPortfolio, int intModelID, string _strModelGroupID, string _strModelPortfolioID)
        {
            List<clsSwitchDetails> listSwitchDetails = new List<clsSwitchDetails>();
            List<clsSwitchDetails> listPortfolioDetails = clsSwitchDetails.replicatePortfolioDetails(_clsPortfolio);
            List<clsSwitchDetails> listModelPortfolioDetails = replicateModelPortfolio(_clsPortfolio, intModelID, _strModelGroupID, _strModelPortfolioID);

            //List<clsSwitchDetails> listSwitchDetails = _clsPortfolio.propSwitch.propSwitchDetails;
            //List<clsSwitchDetails> listSwitchTempDetails = _clsPortfolio.propSwitchTemp.propSwitchDetails;
            //List<clsSwitchDetails> listSwitchFinalDetails = new List<clsSwitchDetails>();
            foreach (clsSwitchDetails origSwitchDetails in listPortfolioDetails)
            {
                foreach (clsSwitchDetails modelSwitchDetails in listModelPortfolioDetails)
                {
                    if (origSwitchDetails.propFundID != modelSwitchDetails.propFundID)
                    {
                        origSwitchDetails.propAllocation = 0;
                    }
                }
                listSwitchDetails.Add(origSwitchDetails);
            }
            foreach (clsSwitchDetails modelSwitchDetails in listModelPortfolioDetails)
            {
                clsSwitchDetails item = new clsSwitchDetails();
                foreach (clsSwitchDetails origSwitchDetails in listPortfolioDetails)
                {
                    if (modelSwitchDetails.propFundID != origSwitchDetails.propFundID)
                    {
                        item = modelSwitchDetails;
                    }
                }
                listSwitchDetails.Add(item);
            }

            return listSwitchDetails;
        }
        public List<clsSwitchDetails> replicateModelPortfolio(clsPortfolio _clsPortfolio, int _intModelID, string _strModelGroupID, string _strModelPortfolioID)
        {
            SqlConnection con1 = new clsSystem_DBConnection(clsSystem_DBConnection.strConnectionString.NavIntegrationDB).propConnection;
            List<clsSwitchDetails> listSwitchDetails = new List<clsSwitchDetails>();

            SqlCommand cmd = new SqlCommand();
            SqlDataReader dr1;

            con1.Open();
            cmd.Connection = con1;
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "[SWITCH_ModelPortfolioDetailsGet]";

            cmd.Parameters.Add("@param_ModelID", System.Data.SqlDbType.Int).Value = _intModelID;
            cmd.Parameters.Add("@param_ModelGroupID", System.Data.SqlDbType.NVarChar).Value = _strModelGroupID;
            cmd.Parameters.Add("@param_ModelPortfolioID", System.Data.SqlDbType.NVarChar).Value = _strModelPortfolioID;

            dr1 = cmd.ExecuteReader();

            float fTotalAllocation = 0;

            while (dr1.Read())
            {

                clsSwitchDetails newClsSwitchDetails = new clsSwitchDetails();

                newClsSwitchDetails.propAllocation = float.Parse(Math.Round(double.Parse(dr1["Allocation"].ToString()), 2).ToString());
                //newClsSwitchDetails.propCreated_By = //dr1["Created_By"].ToString();
                //newClsSwitchDetails.propDate_Created = DateTime.Parse(dr1["Date_Created"].ToString());
                //newClsSwitchDetails.propDate_LastUpdate = DateTime.Parse(dr1["Date_Updated"].ToString());
                newClsSwitchDetails.propFund = new clsFund(int.Parse(dr1["FundID"].ToString()));
                newClsSwitchDetails.propFundID = int.Parse(dr1["FundID"].ToString());
                //newClsSwitchDetails.propSwitchDetailsID = int.Parse(dr1["SwitchDetailsID"].ToString());
                //newClsSwitchDetails.propSwitchID = int.Parse(dr1["SwitchID"].ToString());
                //newClsSwitchDetails.propUpdated_By = dr1["Updated_By"].ToString();
                //newClsSwitchDetails.propIsDeletable = dr1["isDeletable"].ToString().Equals("1") ? true : false;

                //if (Portfolio.propPortfolioDetails[0].propClientCurrency != newClsSwitchDetails.propFund.propCurrency)
                if (_clsPortfolio.propClient.propCurrency != newClsSwitchDetails.propFund.propCurrency)
                {
                    newClsSwitchDetails.propUnits = clsSwitchDetails.computeUnits(newClsSwitchDetails.propAllocation,
                                                                                  float.Parse(Math.Round(double.Parse(_clsPortfolio.propPortfolioDetails[0].propTotalCurrentValueClient.ToString()), 0).ToString()),
                                                                                  clsCurrency.convertToClientCurrency(_clsPortfolio.propClientID, newClsSwitchDetails.propFund.propPrice, newClsSwitchDetails.propFund.propCurrency));
                }
                else
                {
                    newClsSwitchDetails.propUnits = clsSwitchDetails.computeUnits(newClsSwitchDetails.propAllocation,
                                                                                  float.Parse(Math.Round(double.Parse(_clsPortfolio.propPortfolioDetails[0].propTotalCurrentValueClient.ToString()), 0).ToString()),
                                                                                  newClsSwitchDetails.propFund.propPrice);
                }


                newClsSwitchDetails.propCurrencyMultiplier = clsCurrency.getCurrencyMultiplier(_clsPortfolio.propClientID, newClsSwitchDetails.propFund.propCurrency);
                //newClsSwitchDetails.propValue = clsSwitchDetails.computeValue(newClsSwitchDetails.propAllocation, Portfolio.propPortfolioDetails[0].propTotalCurrentValueClient);
                newClsSwitchDetails.propTotalValue = float.Parse(Math.Round(double.Parse(_clsPortfolio.propPortfolioDetails[0].propTotalCurrentValueClient.ToString()), 0).ToString());
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
        public static void insertSwitchHeaderTemp(int IFA_ID, int intModelID, string strModelGroupID, string strModelPortfolioID, string strClientID, string strPortfolioID, string strUserID)
        {

            SqlConnection con = new clsSystem_DBConnection(clsSystem_DBConnection.strConnectionString.NavIntegrationDB).propConnection;
            SqlCommand cmd = new SqlCommand();

            con.Open();
            cmd.Connection = con;
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "[SWITCH_Temp_HeaderInsert]";

            cmd.Parameters.Add("@param_intIFA_ID", System.Data.SqlDbType.Int).Value = IFA_ID;
            cmd.Parameters.Add("@param_ModelID", System.Data.SqlDbType.Int).Value = intModelID;
            cmd.Parameters.Add("@param_strModelGroupID", System.Data.SqlDbType.NVarChar).Value = strModelGroupID;
            cmd.Parameters.Add("@param_strModelPortfolioID", System.Data.SqlDbType.NVarChar).Value = strModelPortfolioID;
            cmd.Parameters.Add("@param_strClientID", System.Data.SqlDbType.NVarChar).Value = strClientID;
            cmd.Parameters.Add("@param_strPortfolioID", System.Data.SqlDbType.NVarChar).Value = strPortfolioID;
            cmd.Parameters.Add("@param_strUser", System.Data.SqlDbType.NVarChar).Value = strUserID;

            cmd.ExecuteNonQuery();

        }
        public static void insertSwitchDetailsTemp(int intModelID, List<clsSwitchDetails> listSwitchDetails, string strClientID, string strPortfolioID, string strUserID)
        {
            SqlConnection con = new clsSystem_DBConnection(clsSystem_DBConnection.strConnectionString.NavIntegrationDB).propConnection;
            con.Open();

            foreach (clsSwitchDetails SwitchDetail in listSwitchDetails)
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;

                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "[SWITCH_Temp_DetailsInsert]";

                cmd.Parameters.Add("@param_intModelID", System.Data.SqlDbType.Int).Value = intModelID;
                cmd.Parameters.Add("@param_strClientID", System.Data.SqlDbType.NVarChar).Value = strClientID;
                cmd.Parameters.Add("@param_strPortfolioID", System.Data.SqlDbType.NVarChar).Value = strPortfolioID;
                cmd.Parameters.Add("@param_intFundID", System.Data.SqlDbType.Int).Value = SwitchDetail.propFund.propFundID;
                cmd.Parameters.Add("@param_fAllocation", System.Data.SqlDbType.Float).Value = SwitchDetail.propAllocation;
                cmd.Parameters.Add("@param_strUser", System.Data.SqlDbType.NVarChar).Value = strUserID;
                cmd.Parameters.Add("@param_sintIsDeletable", System.Data.SqlDbType.SmallInt).Value = SwitchDetail.propIsDeletable == true ? 1 : 0;

                cmd.ExecuteNonQuery();
            }
        }
        public static void removeSwitchDetails(string strClientID, string strPortfolioID, int intFundID)
        {
            SqlConnection con = new clsSystem_DBConnection(clsSystem_DBConnection.strConnectionString.NavIntegrationDB).propConnection;
            con.Open();

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;

            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "[SWITCH_Temp_DetailsFundDelete]";

            cmd.Parameters.Add("@param_strClientID", System.Data.SqlDbType.NVarChar).Value = strClientID;
            cmd.Parameters.Add("@param_strPortfolioID", System.Data.SqlDbType.NVarChar).Value = strPortfolioID;
            cmd.Parameters.Add("@param_intFundID", System.Data.SqlDbType.Int).Value = intFundID;

            cmd.ExecuteNonQuery();

            con.Close();
        }
        public static void deleteSwitchTemp(string strClientID, string strPortfolioID)
        {

            SqlConnection con = new clsSystem_DBConnection(clsSystem_DBConnection.strConnectionString.NavIntegrationDB).propConnection;
            SqlCommand cmd = new SqlCommand();

            con.Open();
            cmd.Connection = con;
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "[SWITCH_Temp_Delete]";

            cmd.Parameters.Add("@param_strClientID", System.Data.SqlDbType.NVarChar).Value = strClientID;
            cmd.Parameters.Add("@param_strPortfolioID", System.Data.SqlDbType.NVarChar).Value = strPortfolioID;

            cmd.ExecuteNonQuery();

        }
        public static void deleteSwitchDetailsTemp(string strClientID, string strPortfolioID)
        {
            SqlConnection con = new clsSystem_DBConnection(clsSystem_DBConnection.strConnectionString.NavIntegrationDB).propConnection;
            con.Open();

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;

            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "[SWITCH_Temp_DetailsDelete]";

            cmd.Parameters.Add("@param_strClientID", System.Data.SqlDbType.NVarChar).Value = strClientID;
            cmd.Parameters.Add("@param_strPortfolioID", System.Data.SqlDbType.NVarChar).Value = strPortfolioID;

            cmd.ExecuteNonQuery();

            con.Close();
        }
        public static void doBulkSwitch()
        {
            SqlConnection con = new clsSystem_DBConnection(clsSystem_DBConnection.strConnectionString.NavIntegrationDB).propConnection;
            con.Open();

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;

            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "[SWITCH_BulkInsert]";
            
            cmd.ExecuteNonQuery();

            con.Close();
        }
        public static void deleteSwitchTempByModel(int intModelID)
        {
            SqlConnection con = new clsSystem_DBConnection(clsSystem_DBConnection.strConnectionString.NavIntegrationDB).propConnection;
            con.Open();

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;

            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "[SWITCH_Temp_DeleteByModel]";

            cmd.Parameters.Add("@param_ModelID", System.Data.SqlDbType.Int).Value = intModelID;
            cmd.ExecuteNonQuery();

            con.Close();
        }
    }
}
