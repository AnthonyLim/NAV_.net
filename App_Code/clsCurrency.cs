using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;

namespace NAV
{
    public class clsCurrency
    {
        
        public static float convertToClientCurrency(string strClientID, float fValueToConvert, string strCurrencyToConvert)
        {

            SqlConnection con = new clsSystem_DBConnection(clsSystem_DBConnection.strConnectionString.NavIntegrationDB).propConnection;
            float fConvertedValue = 0;

            SqlCommand cmd = new SqlCommand();
            SqlDataReader dr;

            con.Open();
            cmd.Connection = con;
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "[Switch_CurrencyConvert]";

            cmd.Parameters.Add("@param_strClientID", System.Data.SqlDbType.NVarChar).Value = strClientID;
            cmd.Parameters.Add("@param_fValueToConvert", System.Data.SqlDbType.Float).Value = fValueToConvert;
            cmd.Parameters.Add("@param_strFundCurrency", System.Data.SqlDbType.NVarChar).Value = strCurrencyToConvert;

            dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                fConvertedValue = dr["ConvertedValue"] != null ? float.Parse(dr["ConvertedValue"].ToString()) : 0;                
            }            

            dr.Close();
            con.Close();
            cmd.Dispose();
            con.Dispose();

            return fConvertedValue;

        }

        public static float getCurrencyMultiplier(string strClientID, string strCurrencyToConvert)
        {

            SqlConnection con = new clsSystem_DBConnection(clsSystem_DBConnection.strConnectionString.NavIntegrationDB).propConnection;
            float fConvertedValue = 0;

            SqlCommand cmd = new SqlCommand();
            SqlDataReader dr;

            con.Open();
            cmd.Connection = con;
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "[Switch_CurrencyMultiplier]";

            cmd.Parameters.Add("@param_strClientID", System.Data.SqlDbType.NVarChar).Value = strClientID;
            cmd.Parameters.Add("@param_strFundCurrency", System.Data.SqlDbType.NVarChar).Value = strCurrencyToConvert;

            dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                fConvertedValue = dr["CurrencyMultiplier"] != null ? float.Parse(dr["CurrencyMultiplier"].ToString()) : 0;
            }

            dr.Close();
            con.Close();
            cmd.Dispose();
            con.Dispose();

            return fConvertedValue;

        }
    }
}