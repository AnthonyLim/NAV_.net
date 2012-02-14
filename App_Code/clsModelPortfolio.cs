using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;

namespace NAV
{
    public class clsModelPortfolio
    {
        SqlConnection con = new clsSystem_DBConnection(clsSystem_DBConnection.strConnectionString.NavIntegrationDB).propConnection;

        #region Property

        private string strModelPortfolioID;
        public string propModelPortfolioID { get { return strModelPortfolioID; } set { strModelPortfolioID = value; } }

        private string strModelGroupID;
        public string propModelGroupID { get { return strModelGroupID; } set { strModelGroupID = value; } }

        private string strModelPortfolioName;
        public string propModelPortfolioName { get { return strModelPortfolioName; } set { strModelPortfolioName = value; } }

        private string strModelPortfolioDesc;
        public string propModelPortfolioDesc { get { return strModelPortfolioDesc; } set { strModelPortfolioDesc = value; } }

        private List<clsModelPortfolioDetails> ModelPortfolioDetails;
        public List<clsModelPortfolioDetails> propModelPortfolioDetails { get { return ModelPortfolioDetails; } set { ModelPortfolioDetails = value; } }
        #endregion

        public clsModelPortfolio(clsPortfolio Portfolio, string strModelGroupID, string strModelPortfolioID)
        {
            getModelPortfolioInfo(Portfolio, strModelGroupID, strModelPortfolioID);
        }
        private void getModelPortfolioInfo(clsPortfolio Portfolio, string strModelGroupID, string strModelPortfolioID)
        {
            SqlCommand cmd = new SqlCommand();
            SqlDataReader dr;

            con.Open();
            cmd.Connection = con;
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "SWITCH_ModelPortfolioGet";

            cmd.Parameters.Add("@param_ModelGroupID", System.Data.SqlDbType.NVarChar).Value = strModelGroupID;
            cmd.Parameters.Add("@param_ModelPortfolioID", System.Data.SqlDbType.NVarChar).Value = strModelPortfolioID;

            dr = cmd.ExecuteReader();

            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    this.strModelPortfolioID = dr["ModelPortfolioID"].ToString();
                    this.strModelGroupID = dr["ModelGroupID"].ToString();
                    this.strModelPortfolioName = dr["ModelPortfolioName"].ToString();
                    this.strModelPortfolioDesc = dr["ModelPortfolioDesc"].ToString();
                    this.ModelPortfolioDetails = clsModelPortfolioDetails.getModelPortfolioDetails(Portfolio, strModelGroupID, strModelPortfolioID);
                }
            }
            else
            {
                this.strModelGroupID = Portfolio.propClientID;
                this.strModelPortfolioID = Portfolio.propPortfolioID;
                this.strModelPortfolioName = Portfolio.propAccountNumber;
                this.ModelPortfolioDetails = clsModelPortfolioDetails.replicatePortfolioDetails(Portfolio.propPortfolioDetails);
            }
            dr.Dispose();
            cmd.Connection.Close();
            cmd.Dispose();
            con.Close();
            //con.Dispose();
        }
        
        public int saveModelPortfolioSwitch()
        {
            int result;
            SqlCommand cmd = new SqlCommand();

            con.Open();
            cmd.Connection = con;
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "SWITCH_ModelPortfolioSave";
            cmd.Parameters.Add("@param_ModelPortfolioID", System.Data.SqlDbType.NVarChar).Value = this.propModelPortfolioID;
            cmd.Parameters.Add("@param_ModelGroupID", System.Data.SqlDbType.NVarChar).Value = this.strModelGroupID;
            cmd.Parameters.Add("@param_ModelPortfolioName", System.Data.SqlDbType.NVarChar).Value = this.propModelPortfolioName;
            cmd.Parameters.Add("@param_ModelPortfolioDesc", System.Data.SqlDbType.NVarChar).Value = this.propModelPortfolioDesc;

            result = (int)cmd.ExecuteNonQuery();

            cmd.Connection.Close();
            cmd.Dispose();
            con.Close();
            //con.Dispose();

            
            return result;
        }
        public void deleteModelPortfolioSwitch()
        {
            SqlCommand cmd = new SqlCommand();
            con.Open();
            cmd.Connection = con;
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "SWITCH_ModelPortfolioDelete";
            cmd.Parameters.Add("@param_ModelGroupID", System.Data.SqlDbType.NVarChar).Value = this.strModelGroupID;
            cmd.Parameters.Add("@param_ModelPortfolioID", System.Data.SqlDbType.NVarChar).Value = this.strModelPortfolioID;
            cmd.ExecuteNonQuery();
            cmd.Connection.Close();
            cmd.Dispose();
            con.Close();
        }
        public void updateModelPortfolioHeader()
        {
            SqlCommand cmd = new SqlCommand();
            con.Open();
            cmd.Connection = con;
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "SWITCH_ModelPortfolioUpdate";
            cmd.Parameters.Add("@param_ModelGroupID", System.Data.SqlDbType.NVarChar).Value = this.strModelGroupID;
            cmd.Parameters.Add("@param_ModelPortfolioID", System.Data.SqlDbType.NVarChar).Value = this.strModelPortfolioID;
            cmd.Parameters.Add("@param_ModelPortfolioDesc", System.Data.SqlDbType.NVarChar).Value = this.strModelPortfolioDesc;
            
            cmd.ExecuteNonQuery();
            
            cmd.Connection.Close();
            cmd.Dispose();
            con.Close();
        }
    }
}
