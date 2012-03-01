using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;

namespace SmsNav
{
    public class Sms
    {

       public struct responseStruct
        {
            public string strMessage;
            public string strError;
        }
         
        public responseStruct sendSms(string strUsername, string strPassword, string strApiId, string strTo, string strText, string strUrl)
        {
            string concatValue = computeMessageToConcat(strText);
            //todo: validate complete parameters
            responseStruct results = new responseStruct();

            WebClient client = new WebClient();
            // Add a user agent header in case the requested URI contains a query.
            client.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)");
            client.QueryString.Add("user", strUsername);
            client.QueryString.Add("password", strPassword);
            client.QueryString.Add("api_id", strApiId);
            client.QueryString.Add("to", strTo);
            client.QueryString.Add("text", strText);

            client.QueryString.Add("concat", concatValue);
            string baseurl = strUrl;
            //handle http errors
            try
            {
                Stream data = client.OpenRead(baseurl);
                StreamReader reader = new StreamReader(data);
                results.strMessage = reader.ReadToEnd();
                data.Close();
                reader.Close();
            }
            catch (Exception e)
            {
                results.strError = e.Message;
            }


            return results;

        }
        private string computeMessageToConcat(string messageToCompute)
        {
            int result = 0;

            int divResult = messageToCompute.Length / 160;
            int modResult = messageToCompute.Length % 160;
            if (modResult > 0)
            {
                result = divResult + 1;
            }
            return result.ToString();
        }
    }
}
