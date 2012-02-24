using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;

namespace NAV
{
    public class clsSwitchFee
    {
        private SqlConnection con = new clsSystem_DBConnection(clsSystem_DBConnection.strConnectionString.NavIntegrationDB).propConnection;

        #region Properties

        private int intIFA_ID;
        public int propIFA_ID { get { return intIFA_ID; } set { intIFA_ID = value; } }

        private string strIFA_Name;
        public string propIFA_Name { get { return strIFA_Name; } set { strIFA_Name = value; } }

        private decimal dAnnual_Fee;
        public decimal propAnnual_Fee { get { return dAnnual_Fee; } set { dAnnual_Fee = value; } }

        private decimal dPer_Switch_Fee;
        public decimal propPer_Switch_Fee { get { return dPer_Switch_Fee; } set { dPer_Switch_Fee = value; } }

        private bool bAccess_Denied;
        public bool propAccess_Denied { get { return bAccess_Denied; } set { bAccess_Denied = value; } }

        private int intQuantity;
        public int propQuantity { get { return intQuantity; } set { intQuantity = value; } }

        private DateTime dtStartDate;
        public DateTime propStartDate { get { return dtStartDate; } set { dtStartDate = value; } }

        private DateTime dtEndDate;
        public DateTime propEndDate { get { return dtEndDate; } set { dtEndDate = value; } }

        //private decimal dFees_Due;
        public decimal propFees_Due { get { return intQuantity * dPer_Switch_Fee; } }

        private string strSwitches;
        public string propSwitches { get { return strSwitches; } set { strSwitches = value; } }

        private decimal dTotal_Fee;
        public decimal propTotal_Fee { get { return dTotal_Fee; } set { dTotal_Fee = value; } }

        #endregion

        #region Constructors

        public clsSwitchFee(int intIFA_ID)
        {
            getSwitchFee(intIFA_ID);
        }

        public clsSwitchFee() {}

        #endregion

        #region Methods

        private void getSwitchFee(int intIFA_ID)
        {
            SqlCommand cmd = new SqlCommand();
            SqlDataReader dr;

            con.Open();
            cmd.Connection = con;

            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "SWITCH_FeeGet";
            cmd.Parameters.Add("@param_intIFA_ID", System.Data.SqlDbType.Int).Value = intIFA_ID;

            dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                this.intIFA_ID = dr["IFA_ID"] == System.DBNull.Value ? 0 : int.Parse(dr["IFA_ID"].ToString());
                this.strIFA_Name = dr["IFA_Username"] == System.DBNull.Value ? string.Empty : dr["IFA_Username"].ToString();
                this.dAnnual_Fee = dr["Annual_Fee"] == System.DBNull.Value ? 0 : decimal.Parse(dr["Annual_Fee"].ToString());
                this.dPer_Switch_Fee = dr["Per_Switch_Fee"] == System.DBNull.Value ? 0 : decimal.Parse(dr["Per_Switch_Fee"].ToString());
                this.bAccess_Denied = dr["Access_Denied"] == System.DBNull.Value ? false : bool.Parse(dr["Access_Denied"].ToString());
            }
            dr.Close();
            cmd.Dispose();
            con.Close();
            con.Dispose();
        }

        #endregion

        public static List<clsSwitchFee> getSwitchFeeList()
        {
            List<clsSwitchFee> oSwitchFeeList = new List<clsSwitchFee>();
            SqlConnection con = new clsSystem_DBConnection(clsSystem_DBConnection.strConnectionString.NavIntegrationDB).propConnection;

            SqlCommand cmd = new SqlCommand();
            SqlDataReader dr;

            con.Open();
            cmd.Connection = con;

            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "SWITCH_FeeGet";
            cmd.Parameters.Add("@param_intIFA_ID", System.Data.SqlDbType.Int).Value = 0;

            dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                clsSwitchFee oSwitchFee = new clsSwitchFee();

                oSwitchFee.propIFA_ID = dr["IFA_ID"] == System.DBNull.Value ? 0 : int.Parse(dr["IFA_ID"].ToString());
                oSwitchFee.propIFA_Name = dr["IFA_Username"] == System.DBNull.Value ? string.Empty : dr["IFA_Username"].ToString();
                oSwitchFee.propAnnual_Fee = dr["Annual_Fee"] == System.DBNull.Value ? 0 : decimal.Parse(dr["Annual_Fee"].ToString());
                oSwitchFee.propPer_Switch_Fee = dr["Per_Switch_Fee"] == System.DBNull.Value ? 0 : decimal.Parse(dr["Per_Switch_Fee"].ToString());
                oSwitchFee.propAccess_Denied = dr["Access_Denied"] == System.DBNull.Value ? false : bool.Parse(dr["Access_Denied"].ToString());

                oSwitchFeeList.Add(oSwitchFee);
            }
            dr.Close();
            cmd.Dispose();
            con.Close();
            con.Dispose();

            return oSwitchFeeList;
        }
        public static void saveSwitchFee(int intIFA_ID, string strIFA_Name, decimal dAnnual_Fee, decimal dPerSwitch_Fee, bool bAccessDenied)
        {
            SqlConnection con = new clsSystem_DBConnection(clsSystem_DBConnection.strConnectionString.NavIntegrationDB).propConnection;
            SqlCommand cmd = new SqlCommand();
            
            con.Open();
            cmd.Connection = con;
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "SWITCH_FeeSave";

            cmd.Parameters.Add("@param_intIFA_ID", System.Data.SqlDbType.Int).Value = intIFA_ID;
            cmd.Parameters.Add("@param_IFA_Username", System.Data.SqlDbType.NVarChar).Value = strIFA_Name;
            cmd.Parameters.Add("@param_Annual_Fee", System.Data.SqlDbType.Decimal).Value = dAnnual_Fee;
            cmd.Parameters.Add("@param_Per_Switch_Fee", System.Data.SqlDbType.Decimal).Value = dPerSwitch_Fee;
            cmd.Parameters.Add("param_Access_Denied", System.Data.SqlDbType.Bit).Value = bAccessDenied;

            cmd.ExecuteNonQuery();
            cmd.Dispose();
            con.Close();
            con.Dispose();
        }
        public static void deleteSwitchFee(int intIFA_ID)
        {
            SqlConnection con = new clsSystem_DBConnection(clsSystem_DBConnection.strConnectionString.NavIntegrationDB).propConnection;
            SqlCommand cmd = new SqlCommand();

            con.Open();
            cmd.Connection = con;
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "SWITCH_FeeDelete";

            cmd.Parameters.Add("@param_intIFA_ID", System.Data.SqlDbType.Int).Value = intIFA_ID;

            cmd.ExecuteNonQuery();
            cmd.Dispose();
            con.Close();
            con.Dispose();
        }
        public static List<clsSwitchFee> computePerSwitchFeeReport(int intIFA_ID, string strStartDate, string strEndDate)
        {
            List<clsSwitchFee> oSwitchFeeList = new List<clsSwitchFee>();
            decimal dTotalFee = 0;
            SqlConnection con = new clsSystem_DBConnection(clsSystem_DBConnection.strConnectionString.NavIntegrationDB).propConnection;
            SqlCommand cmd = new SqlCommand();
            SqlDataReader dr;

            con.Open();
            cmd.Connection = con;
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "SWITCH_FeeReportGet";

            cmd.Parameters.Add("@param_intIFA_ID", System.Data.SqlDbType.Int).Value = intIFA_ID;
            cmd.Parameters.Add("@param_StartDate", System.Data.SqlDbType.NVarChar).Value = strStartDate;
            cmd.Parameters.Add("@param_EndDate", System.Data.SqlDbType.NVarChar).Value = strEndDate;

            dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                clsSwitchFee oSwitchFee = new clsSwitchFee();
                //oSwitchFee.propSwitches = Array.ConvertAll(dr["Switches"].ToString().Split(','), s => int.Parse(s));
                oSwitchFee.propSwitches = dr["Switches"].ToString();
                oSwitchFee.propIFA_ID = (int)dr["IFA_ID"];
                oSwitchFee.propIFA_Name = dr["IFA_Name"].ToString();
                oSwitchFee.propPer_Switch_Fee = (decimal)dr["Per_Switch_Fee"];
                oSwitchFee.propQuantity = (int)dr["Quantity"];
                oSwitchFee.propStartDate = DateTime.Parse(dr["StartDate"].ToString());
                oSwitchFee.propEndDate = DateTime.Parse(dr["EndDate"].ToString());

                dTotalFee = dTotalFee + oSwitchFee.propFees_Due;
                oSwitchFee.propTotal_Fee = dTotalFee;
                oSwitchFeeList.Add(oSwitchFee);
            }
            return oSwitchFeeList;
        }
    }
}