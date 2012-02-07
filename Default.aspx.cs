using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;

namespace NAV
{
	/// <summary>
	/// Summary description for SessionTransfer.
	/// </summary>
	public class SessionTransfer : System.Web.UI.Page
	{
        string conString = System.Configuration.ConfigurationManager.ConnectionStrings["NavIntegrationDB"].ToString();
        private string destinationPage;

        private void Page_Load(object sender, System.EventArgs e)
        {
            //string guidSave;
            if (Request.QueryString["guid"] != null)
            {
                //Retreive the session information, and redirect to the URL specified
                //  by the querystring.
                GetSessionFromDatabase(Request.QueryString["guid"]);
                ClearSessionFromDatabase(Request.QueryString["guid"]);

                if (Session[clsSystem_Session.strSession.level.ToString()].ToString() == "client")
                {
                    string[] strFolder = destinationPage.Split('/');

                    if (strFolder[0].Equals("Portfolio")) { Response.Redirect("Portfolio/SwitchClient.aspx"); }
                    if (strFolder[0].Equals("Scheme")) { Response.Redirect("Scheme/SchemeClient.aspx"); }

                }
                else
                {
                    Response.Redirect(destinationPage);
                }
            }
        }

		//This method adds the session information to the database and returns the GUID
		//  used to identify the data.
		private string AddSessionToDatabase(string strDestination)
		{
			SqlConnection con = new SqlConnection(conString);		

			SqlCommand cmd = new SqlCommand();
			con.Open();
			cmd.Connection = con;
			int i = 0;
			string strSql, guidTemp = GetGuid();
            System.Text.StringBuilder strSession = new System.Text.StringBuilder();
			while (i < Session.Contents.Count)
			{
                strSession.Append(Session.Contents.Keys[i] + "+++++" + Session.Contents[i].ToString() + "^^^^^");
				i++;
			}
            strSql = "INSERT INTO SessionState (GUID, Session, Destination) " + "VALUES ('" + guidTemp + "', '" + strSession.ToString() + "', '" + strDestination + "')";
            cmd.CommandText = strSql;
            cmd.ExecuteNonQuery();
			con.Close();
			cmd.Dispose();
			con.Dispose();

			return guidTemp;
		}


		//This method retrieves the session information identified by the parameter
		//  guidIn from the database.
		private void GetSessionFromDatabase(string guidIn)
		{
			//**************************************
			//Enter connection information here

			SqlConnection con = new SqlConnection(conString);
			
			//**************************************
			SqlCommand cmd = new SqlCommand();
			SqlDataReader dr;
			con.Open();
			cmd.Connection = con;

			string strSql, strSession = string.Empty, strDestination=string.Empty, guidTemp = GetGuid();
	
			//Get a DataReader that contains all the Session information
            strSql = "SELECT * FROM SessionState WHERE GUID = '" + guidIn + "'";
			cmd.CommandText = strSql;
			dr = cmd.ExecuteReader();
            //^^^^^
			//Iterate through the results and store them in the session object
			while (dr.Read())
			{
				//Session[dr["SessionKey"].ToString()] = dr["SessionValue"].ToString();
                strSession = dr["Session"].ToString();
                strDestination = dr["Destination"].ToString();
			}
            ConvertToSession(strSession);
            destinationPage = strDestination;
			//Clean up database objects
			dr.Close();
			con.Close();
			cmd.Dispose();
			con.Dispose();
		}


		//This method removes all session information from the database identified by the 
		//  the GUID passed in through the parameter guidIn.
		private void ClearSessionFromDatabase(string guidIn)
		{
			//**************************************
			//Enter connection information here

			SqlConnection con = new SqlConnection(conString);
			
			//**************************************
			SqlCommand cmd = new SqlCommand();
			con.Open();
			cmd.Connection = con;
			string strSql;

            strSql = "DELETE FROM SessionState WHERE GUID = '" + guidIn + "'";
			cmd.CommandText = strSql;
			cmd.ExecuteNonQuery();

			con.Close();
			cmd.Dispose();
			con.Dispose();
		}


		//This method returns a new GUID as a string.
		private string GetGuid()
		{
			return System.Guid.NewGuid().ToString();
		}

        private void ConvertToSession(string session)
        {
            string[] strSeparators = new string[] {"^^^^^"};
            if (session != string.Empty)
            {
                string[] strSession = session.Split(strSeparators, StringSplitOptions.RemoveEmptyEntries);

                foreach (string item in strSession)
                {
                    string[] separator = new string[] {"+++++"};
                    string[] strSessionValue = item.Split(separator, StringSplitOptions.RemoveEmptyEntries);

                    if (strSessionValue.Length == 2)
                    {
                        Session[strSessionValue[0].ToString()] = strSessionValue[1].ToString();
                    }
                }
            }
        }

		#region Web Form Designer generated code
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: This call is required by the ASP.NET Web Form Designer.
			//
			InitializeComponent();
			base.OnInit(e);
		}
		
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{    
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion
	}
}
