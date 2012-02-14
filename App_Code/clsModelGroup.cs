using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;

namespace NAV
{
    public class clsModelGroup
    {
        SqlConnection con = new clsSystem_DBConnection(clsSystem_DBConnection.strConnectionString.NavIntegrationDB).propConnection;

        #region Property

        private string strModelID;
        public string propModelID { get { return strModelID; } set { strModelID = value; } }
        private string strModelIFA;
        public string propModelIFA { get { return strModelIFA; } set { strModelIFA = value; } }
        private string strModelGroup;
        public string propModelGroup { get { return strModelGroup; } set { strModelGroup = value; } }
        private string strModelName;
        public string propModelName { get { return strModelName; } set { strModelName = value; } }

        private string strModelGroupID;
        public string propModelGroupID { get { return strModelGroupID; } set { strModelGroupID = value; } }

        private string strModelGroupName;
        public string propModelGroupName { get { return strModelGroupName; } set { strModelGroupName = value; } }

        private int intIFA_ID;
        public int propIFA_ID { get { return intIFA_ID; } set { intIFA_ID = value; } }

        private int intModelGroupCode;
        public int propModelGroupCode { get { return intModelGroupCode; } set { intModelGroupCode = value; } }

        private clsModelPortfolio _clsModelPortfolio;
        public clsModelPortfolio propModelPortfolio { get { return _clsModelPortfolio; } set { _clsModelPortfolio = value; } }

        #endregion

        public clsModelGroup() { }
        public clsModelGroup(clsPortfolio Portfolio, string strModelGroupID, string strModelPortfolioID, int intIFA_ID) 
        {
            getModelGroupInfo(Portfolio, strModelGroupID, strModelPortfolioID, intIFA_ID);
        }
        protected void getModelGroupInfo(clsPortfolio Portfolio, string strModelGroupID, string strModelPortfolioID, int intIFA_ID)
        {
            
            SqlCommand cmd = new SqlCommand();
            SqlDataReader dr;

            con.Open();
            cmd.Connection = con;
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "SWITCH_ModelGroupGet";

            cmd.Parameters.Add("@param_ModelGroupID", System.Data.SqlDbType.NVarChar).Value = strModelGroupID;
            cmd.Parameters.Add("@param_IFA_ID", System.Data.SqlDbType.Int).Value = intIFA_ID;

            dr = cmd.ExecuteReader();

            while(dr.Read())
            {
                this.strModelGroupID = dr["ClientID"].ToString();
                this.strModelGroupName = dr["Forenames"].ToString();
                this.intIFA_ID = int.Parse(dr["ClientNumber"].ToString());
                this.intModelGroupCode = int.Parse(dr["IFA_ID"].ToString());
                this._clsModelPortfolio = new clsModelPortfolio(Portfolio, strModelGroupID, strModelPortfolioID);
            }
            dr.Dispose();
            cmd.Connection.Close();
            cmd.Dispose();
            con.Close();
            //con.Dispose();
        }
        public int saveModelGroupSwitch()
        {
            int result;
            //SqlConnection con = new clsSystem_DBConnection(clsSystem_DBConnection.strConnectionString.NavIntegrationDB).propConnection;
            SqlCommand cmd = new SqlCommand();

            con.Open();
            cmd.Connection = con;
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "SWITCH_ModelGroupSave";

            cmd.Parameters.Add("@param_ModelGroupID", System.Data.SqlDbType.NVarChar).Value = this.strModelGroupID;
            cmd.Parameters.Add("@param_ModelGroupName", System.Data.SqlDbType.NVarChar).Value = this.strModelGroupName;
            cmd.Parameters.Add("@param_IFA_ID", System.Data.SqlDbType.Int).Value = this.intIFA_ID;
            cmd.Parameters.Add("@param_ModelGroupCode", System.Data.SqlDbType.Int).Value = this.intModelGroupCode;

            result = (int) cmd.ExecuteNonQuery();

            cmd.Connection.Close();
            cmd.Dispose();
            con.Close();
            //con.Dispose();

            return result;
        }
        public static clsModelGroup getModelPortfolioClientList(string strIFACode, string strModelGroup, string strModelName)
        {
            clsModelGroup _clsModelGroup = new clsModelGroup();
            List<clsPortfolio> listPortfolio = new List<clsPortfolio>();
            SqlConnection con = new clsSystem_DBConnection(clsSystem_DBConnection.strConnectionString.NavIntegrationDB).propConnection;
            SqlCommand cmd = new SqlCommand();
            SqlDataReader dr;
            string strClientID = string.Empty;
            string strPortfolioID = string.Empty;
            con.Open();
            cmd.Connection = con;
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "SWITCH_ModelPortfolioClientGet";

            cmd.Parameters.Add("", System.Data.SqlDbType.NVarChar).Value = strIFACode;
            cmd.Parameters.Add("", System.Data.SqlDbType.NVarChar).Value = strModelGroup;
            cmd.Parameters.Add("", System.Data.SqlDbType.NVarChar).Value = strModelName;

            dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                _clsModelGroup.propModelID = dr["ModelID"].ToString();
                _clsModelGroup.propModelIFA = dr["ModelIFA"].ToString();
                _clsModelGroup.propModelGroup = dr["ModelGroup"].ToString();
                _clsModelGroup.propModelName = dr["ModelName"].ToString();

                strClientID = dr["ClientID"].ToString();
                strPortfolioID = dr["ClientPortfolioID"].ToString();

                clsPortfolio _clsPortfolio = new clsPortfolio();
                //_clsPortfolio.(strClientID, strPortfolioID);
                _clsPortfolio.propClient = new clsClient(strClientID);
                listPortfolio.Add(_clsPortfolio);
            }
            return _clsModelGroup;
        }
    }
}
