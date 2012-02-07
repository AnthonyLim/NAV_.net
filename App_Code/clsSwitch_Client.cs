using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;

namespace NAV
{
    public class clsSwitch_Client
    {
        private SqlConnection con = new clsSystem_DBConnection(clsSystem_DBConnection.strConnectionString.NavIntegrationDB).propConnection;

        #region properties

        private int intSwitchID;
        public int propSwitchID { get { return intSwitchID; } set { intSwitchID = value; } }

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

        private List<clsSwitchDetails_Client> listSwitchDetails;
        public List<clsSwitchDetails_Client> propSwitchDetails { get { return listSwitchDetails; } set { listSwitchDetails = value; } }

        #endregion

        public clsSwitch_Client(int intSwitchID)
        {
            getSwitchInfo(intSwitchID);
        }

        private void getSwitchInfo(int intSwitchID)
        { 
            SqlCommand cmd = new SqlCommand();
            SqlDataReader dr;
            con.Open();
            cmd.Connection = con;
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "[SWITCH_HeaderGet]";

            cmd.Parameters.Add("@param_intSwitchID", System.Data.SqlDbType.Int).Value = intSwitchID;            
                                   
            dr = cmd.ExecuteReader();

            while (dr.Read())
            {

                this.propClientID = dr["ClientID"].ToString();
                this.propDate_Created = DateTime.Parse(dr["Date_Created"].ToString());
                this.propPortfolioID = dr["PortfolioID"].ToString();
                this.propStatus = dr["Amend_Status"] != System.DBNull.Value ? short.Parse(dr["Amend_Status"].ToString()) : (short) 0;
                this.propStatusString = clsSwitch.getSwitchStringStatus(this.propStatus);
                this.propSwitchID = int.Parse(dr["SwitchID"].ToString());
                this.propSwitchDetails = clsSwitchDetails_Client.getSwitchDetails(intSwitchID);
                this.propCreated_By = dr["Created_By"].ToString();
                this.propDescription = dr["Amend_Description"].ToString();
            }

            dr.Close();
            con.Close();
            cmd.Dispose();
            //con.Dispose();
        }

        public static void updateSwitchHeader(clsSwitch.enumSwitchStatus SwitchStatus, int intSwitchID, string strDescription)
        {

            SqlConnection con = new clsSystem_DBConnection(clsSystem_DBConnection.strConnectionString.NavIntegrationDB).propConnection;
            SqlCommand cmd = new SqlCommand();

            con.Open();
            cmd.Connection = con;
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "[SWITCHclient_HeaderUpdate]";
           
            cmd.Parameters.Add("@param_intSwitchID", System.Data.SqlDbType.Int).Value = intSwitchID;            
            cmd.Parameters.Add("@param_intStatus", System.Data.SqlDbType.SmallInt).Value = SwitchStatus;
            cmd.Parameters.Add("@param_strDescription", System.Data.SqlDbType.NVarChar).Value = strDescription;

            cmd.ExecuteNonQuery();
            
        }

        public static void deleteSwitch(int intSwitchID)
        {

            SqlConnection con = new clsSystem_DBConnection(clsSystem_DBConnection.strConnectionString.NavIntegrationDB).propConnection;
            SqlCommand cmd = new SqlCommand();

            con.Open();
            cmd.Connection = con;
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "[SWITCHclient_Delete]";

            cmd.Parameters.Add("@param_SwitchID", System.Data.SqlDbType.Int).Value = intSwitchID;

            cmd.ExecuteNonQuery();

        }

        public static void declineSwitch(int intSwitchID) 
        {
            SqlConnection con = new clsSystem_DBConnection(clsSystem_DBConnection.strConnectionString.NavIntegrationDB).propConnection;
            SqlCommand cmd = new SqlCommand();

            con.Open();
            cmd.Connection = con;
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "[SWITCHclient_Decline]";

            cmd.Parameters.Add("@param_SwitchID", System.Data.SqlDbType.Int).Value = intSwitchID;

            cmd.ExecuteNonQuery();
        }

    }
}
