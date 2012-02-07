using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Text;
using System.Net;
using System.IO;
using System.Security.Cryptography.X509Certificates;

using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html;
using iTextSharp.text.html.simpleparser;

namespace NAV
{
    public class clsOutput
    {
        public const string ROWDETAILS = "<tr>{^row$}</tr>";
        public const string CELLDETAILS = @"<td>{^FundCompany$}</td><td>{^FundName$}</td><td>{^InsurComp$}</td><td>{^SEDOL$}</td><td align='right'>{^%Portfolio$}</td>";
        public const string SIGNATURE = @"<br />
            <p>I hereby confirm that I wish to proceed with the switch as detailed above.</p>
            <br /><br /><br /><br />
            <table style='width:100%; border-top-width:medium; border-top-color:Black; border-top-style:solid;'>
                <tr>
                    <td style='text-align:left;'>{^SignatureClientName$}</td>
                    <td style='text-align:right;'>Date</td>
                    <td>&nbsp;</td>
                </tr>
            </table>";
        protected clsPortfolio _Portfolio = null;
        protected string ClientName = string.Empty;

        public enum enumSwitchType
        {
            Portfolio,
            Scheme
        }

        public enum enumOutputType
        {
            PDF,
            Spreadsheet,
            WordDocument
        }

        public clsOutput()
        {
            this.ClientName = HttpContext.Current.Session["Forenames"] + " " + HttpContext.Current.Session["surname"];
            //this.Portfolio_Signature += "<table class=\"signatory\" style=\"width:100%\"><tr><td align=left>{^SignatureClientName$}</td><td align=right>{^DateNow$}</td></tr></table>";
        }

        public clsOutput(clsPortfolio Portfolio)
            : this()
        {
            this._Portfolio = Portfolio;
        }

        //protected static string PortfolioOutput_GetHTML()
        //{
        //    return clsPDF.ToHTMLString("https://" + HttpContext.Current.Request["HTTP_HOST"] + HttpContext.Current.Request.ApplicationPath + "/Output/Templates/SWITCH_Portfolio.aspx");
        //}
        //protected static string SchemeOutput_GetHTML()
        //{
        //    return clsPDF.ToHTMLString("https://" + HttpContext.Current.Request["HTTP_HOST"] + HttpContext.Current.Request.ApplicationPath + "/Output/Templates/SWITCH_Scheme.aspx");
        //}

        protected static string PortfolioOutput_GetHTML()
        {
            return File.ReadAllText(HttpContext.Current.Server.MapPath("~/Output/Templates/SWITCH_Portfolio.txt"));
            //return clsPDF.ToHTMLString("https://" + HttpContext.Current.Request["HTTP_HOST"] + HttpContext.Current.Request.ApplicationPath + "/Output/Templates/SWITCH_Portfolio.aspx");
        }
        protected static string SchemeOutput_GetHTML()
        {
            return File.ReadAllText(HttpContext.Current.Server.MapPath("~/Output/Templates/SWITCH_Scheme.txt"));
            //return clsPDF.ToHTMLString("https://" + HttpContext.Current.Request["HTTP_HOST"] + HttpContext.Current.Request.ApplicationPath + "/Output/Templates/SWITCH_Scheme.aspx");
        }

        public static string generateOutputFile(enumOutputType outputType,string strTemplate, StyleSheet Style,int SwitchID,enumSwitchType SwitchType)
        {
            try
            {
                clsClient client = new clsClient(HttpContext.Current.Session["requestclientid"].ToString());
                switch (outputType)
                {
                    case enumOutputType.PDF:
                        string Path = HttpContext.Current.Server.MapPath("~/Output/PDF/");
                        string PDFPath = string.Format("{0}{1}_{2}_{3}_{4:yyyy-MM-dd}.pdf",
                            Path,
                            SwitchType.ToString(),
                            (new clsIFA(int.Parse(HttpContext.Current.Session["ifaid"].ToString()))).propIFA_Name,
                            client.propForename +" "+ client.propSurname,
                            DateTime.Now);
                        
                        using (clsPDF O = new clsPDF(){StyleSheet = Style})
                        {
                            O.PageSetup(PageSize.LETTER, 40, 40, 30, 15);
                            O.FromHTMLString(strTemplate);
                            O.StartCreate(PDFPath);
                            LogOutput(enumSwitchType.Portfolio,SwitchID,PDFPath,enumOutputType.PDF);
                        }
                        return PDFPath;
                    default:
                        return "";
                }
            }
            catch (System.Net.Mail.SmtpException SE)
            {
                throw new clsOutputException(SE.Message, SE);
            }
            catch (Exception ex) { throw new clsOutputException(ex.Message, ex); }
        }

        public static void LogOutput(enumSwitchType switchType, int SwitchID, string Filename, enumOutputType outputType)
        {
            using (System.Data.SqlClient.SqlConnection con = new clsSystem_DBConnection(clsSystem_DBConnection.strConnectionString.NavIntegrationDB).propConnection )
            {
                con.Open();
                using (System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand())
                {
                    cmd.Connection = con;
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "[SWITCH_Output_Log]";
                    cmd.Parameters.Add("@SwitchType", System.Data.SqlDbType.VarChar).Value = switchType.ToString();
                    cmd.Parameters.Add("@SwitchID", System.Data.SqlDbType.Int).Value = SwitchID;
                    cmd.Parameters.Add("@Filename", System.Data.SqlDbType.VarChar).Value = Filename;
                    cmd.Parameters.Add("@OutputType", System.Data.SqlDbType.VarChar).Value = outputType.ToString();
                    cmd.ExecuteNonQuery();
                }
            }
        }
        /// pending: 

        public static StyleSheet getStyleSheet_ApprovedSwitch()
        {
            StyleSheet Style = new StyleSheet();
            Style.LoadTagStyle(HtmlTags.BODY, HtmlTags.FONTSIZE, "5");
            Style.LoadTagStyle(HtmlTags.TH, HtmlTags.TEXTALIGN, "center");
            Style.LoadTagStyle(HtmlTags.TH, HtmlTags.FONTWEIGHT, "bolder");
            Style.LoadStyle("title", HtmlTags.TEXTALIGN, "center");
            //Style.LoadStyle("title", HtmlTags.FONTSIZE, "1em");
            Style.LoadStyle("title", HtmlTags.FONTWEIGHT, "bolder");
            Style.LoadStyle("left", HtmlTags.WIDTH, "40%");
            Style.LoadStyle("left", HtmlTags.FONTWEIGHT, "bold");
            Style.LoadStyle("left", "padding", "0px");
            Style.LoadStyle("right", "padding", "0px");
            Style.LoadStyle("right", HtmlTags.WIDTH, "60%");
            Style.LoadStyle("right", HtmlTags.PADDINGLEFT, "0.5em");
            return Style;
        }

        public static string generateApprovedSwitch(clsPortfolio Portfolio, int IFAID)
        {
            string resultingRow = string.Empty;
            foreach (clsPortfolioDetails SwitchDetails in Portfolio.propPortfolioDetails)
            {
                string myRow = ROWDETAILS.Replace("{^row$}", clsOutput.CELLDETAILS);
                myRow = myRow.Replace("{^FundCompany$}", (new clsFund(SwitchDetails.propFundNameID)).propCompanyID.ToString());
                myRow = myRow.Replace("{^FundName$}", SwitchDetails.propNameOfFund);
                myRow = myRow.Replace("{^InsurComp$}", "None"); 
                myRow = myRow.Replace("{^SEDOL$}", (new clsFund(SwitchDetails.propFundNameID)).propSEDOL);
                myRow = myRow.Replace("{^%Portfolio$}", SwitchDetails.propAllocationPercent.ToString("N2") + "%");
                resultingRow += myRow;
            }
            clsClient client = new clsClient(Portfolio.propClientID);
            string html = clsOutput.PortfolioOutput_GetHTML();
            clsIFA IFA = new clsIFA(IFAID);
            html = html.Replace("{^IFAName$}", IFA.propIFA_Name ?? string.Empty);
            html = html.Replace("{^ClientName$}",(client.propForename ?? String.Empty) + " " + (client.propSurname ?? string.Empty)  );
            html = html.Replace("{^Company$}", Portfolio.propCompany);
            html = html.Replace("{^PType$}", Portfolio.propPortfolioType);
            html = html.Replace("{^DateTransmit$}", "?"); // DateTime.Now.ToString("MM-dd-yyyy")); //-->Temporary
            html = html.Replace("{^Curr$}", Portfolio.propPortfolioCurrency);
            html = html.Replace("{^AccNum$}", Portfolio.propAccountNumber);
            html = html.Replace("{^DateApprv$}", DateTime.Now.ToString("MM-dd-yyyy") ?? String.Empty);// Portfolio.propSwitch.propDate_Created.ToString("MM-dd-yyyy") ?? string.Empty);
            clsCompany company = new clsCompany(Portfolio.propCompanyID);
            //throw new Exception(company.propSignedConfirmation.ToString());
            if (company.propSignedConfirmation)
            {
                html = html.Replace("{^Sign$}", clsOutput.SIGNATURE)
                    .Replace("{^SignatureClientName$}", client.propForename + " " + client.propSurname );
            }
            else
            {
                html = html.Replace("{^Sign$}", string.Empty);
            }
            html = html.Replace("{^SwitchDetails$}", resultingRow);
            return html;
        }

        public static string generateApprovedSwitchScheme(clsScheme Scheme)
        {
            string HoldingsRows = string.Empty;
            clsSwitchScheme switchScheme = new clsSwitchScheme(Scheme);
            foreach(clsSwitchScheme.clsSwitchSchemeDetails details in switchScheme.propSwitchDetails)
            {
                string myRow = clsOutput.ROWDETAILS.Replace("{^row$}", clsOutput.CELLDETAILS);
                myRow = myRow.Replace("{^FundCompany$}", details.propFund.propCompanyID.ToString() /* (new clsFund(details.propFund.propFundID)).propCompanyID.ToString()*/);
                myRow = myRow.Replace("{^FundName$}", details.propFund.propFundName);
                myRow = myRow.Replace("{^InsurComp$}", "None");
                myRow = myRow.Replace("{^SEDOL$}", details.propFund.propSEDOL);
                myRow = myRow.Replace("{^%Portfolio$}", details.propAllocation.ToString("N2") + "%");
                HoldingsRows += myRow;
            }
            string Contribution = string.Empty;
            foreach(clsSwitchScheme.clsSwitchSchemeDetails details in switchScheme.propSwitchDetailsContribution)
            {
                string myRow = clsOutput.ROWDETAILS.Replace("{^row$}", clsOutput.CELLDETAILS);
                myRow = myRow.Replace("{^FundCompany$}", details.propFund.propCompanyID.ToString() /* (new clsFund(details.propFund.propFundID)).propCompanyID.ToString()*/);
                myRow = myRow.Replace("{^FundName$}", details.propFund.propFundName);
                myRow = myRow.Replace("{^InsurComp$}", "None");
                myRow = myRow.Replace("{^SEDOL$}", details.propFund.propSEDOL);
                myRow = myRow.Replace("{^%Portfolio$}", details.propAllocation.ToString("N2") + "%");
                Contribution += myRow;
            }
            string html = SchemeOutput_GetHTML();
            html = html.Replace("{^PortfolioHoldings$}", HoldingsRows);
            html = html.Replace("{^Contribution$}", Contribution);

            clsClient client = new clsClient(Scheme.propClient.propClientID);
            clsIFA IFA = new clsIFA(int.Parse(HttpContext.Current.Session["ifaid"].ToString()));
            html = html.Replace("{^IFAName$}", IFA.propIFA_Name);
            html = html.Replace("{^ClientName$}", client.propForename + " " + client.propSurname);
            html = html.Replace("{^Company$}", Scheme.propCompany.propCompany);
            html = html.Replace("{^PType$}", Scheme.propPortfolioType);
            html = html.Replace("{^DateTransmit$}", "?"); //-->Temporary
            html = html.Replace("{^Curr$}", Scheme.propSchemeCurrency);
            html = html.Replace("{^AccNum$}", Scheme.propAccountNumber);
            html = html.Replace("{^DateApprv$}", DateTime.Now.ToString("MM-dd-yyyy") ?? String.Empty);
            if (Scheme.propCompany.propSignedConfirmation)
            {
                html = html.Replace("{^Sign$}", clsOutput.SIGNATURE);
                html = html.Replace("{^SignatureClientName$}", client.propForename + " " + client.propSurname);
            }
            else
            {
                html = html.Replace("{^Sign$}", string.Empty);
            }
            return html;
        }

        public class clsPDF : IDisposable
        {
            protected Document _doc;
            public Document DocumentObject { get { return this._doc; } }

            protected string HTML;
            internal PageSetup page;
            protected string PDFDirectory = @"C:\PDFs";
            protected HttpContext Context { get { return HttpContext.Current; } }
            protected StyleSheet style = null;
            public StyleSheet StyleSheet { set { this.style = value; } }

            public clsPDF() { this.PDFDirectory = @"C:\PDFs"; }

            public void FromURL(string URL)
            {
                try
                {
                    this.HTML = clsPDF.ToHTMLString(URL);
                }
                catch (clsOutputException e)
                {
                    throw e;
                }
            }

            public void FromHTMLString(string HTML){ this.HTML = HTML; }

            public static string ToHTMLString(string URL)
            {
                try
                {
                    if (URL.Contains("https"))
                    {
                        // Trust all certificates
                        System.Net.ServicePointManager.ServerCertificateValidationCallback = ((sender, certificate, chain, sslPolicyErrors) => true);

                        //Trust sender
                        //System.Net.ServicePointManager.ServerCertificateValidationCallback = ((sender, cert, chain, sslPolicyErrors) => cert.Subject.Contains("localhost"));

                    }
                    HttpWebRequest wreq = (HttpWebRequest)HttpWebRequest.Create(URL);
                    wreq.MaximumAutomaticRedirections = 4;
                    wreq.MaximumResponseHeadersLength = 4;
                    wreq.UseDefaultCredentials = true;
                    StreamReader reader = new StreamReader(((HttpWebResponse)wreq.GetResponse()).GetResponseStream(), System.Text.Encoding.UTF8);

                    string html = reader.ReadToEnd();
                    reader.Close();
                    return html;

                    //this.HTML = reader.ReadToEnd();
                    //reader.Close();

                    /* Starting point for saving the HTMLContent into a file. (Just for debugging purpose) */
                    //string path = @"C:\logs\theHTML.html";
                    //using (StreamWriter sw = System.IO.File.CreateText(path))
                    //{
                    //    sw.Write(HTML);
                    //    sw.Close(); sw.Dispose();
                    //}
                    /* End point */

                }
                catch (SystemException e)
                {
                    throw new clsOutputException(e.Message, e);
                }
            }

            public void PageSetup(Rectangle Size, float marginLeft, float marginRight, float marginTop, float marginBottom)
            {
                this.page = new PageSetup(Size, marginLeft, marginRight, marginTop, marginBottom);
            }

            public void StartCreate()
            {
                try
                {
                    this.StartCreate(false, "Reports.pdf");
                }
                catch (Exception e)
                { throw new clsOutputException(e.Message, e); }
            }

            public void StartCreate(bool isDownloadable, string Filename)
            {
                try
                {
                    this._doc.AddAuthor("NAVIFA");
                    this._doc.AddCreator("NAV-ASPX");
                    this._doc.AddKeywords(Filename);
                    this._doc.AddKeywords("Nav Document");
                    this._doc.AddSubject("NAV Document");
                    this._doc.AddTitle("NAV Document");
                    this.Context.Response.ContentType = "application/pdf";
                    PdfWriter.GetInstance(this._doc, this.Context.Response.OutputStream);
                    this._doc.Open();
                    foreach (IElement elem in HTMLWorker.ParseToList(new StringReader(this.HTML), this.style)) { this._doc.Add(elem); }
                    this._doc.Close();
                    if (isDownloadable)
                    {
                        this.Context.Response.Buffer = true;
                        //this.Response.ClearContent();
                        //this.Response.ClearHeaders();
                        this.Context.Response.AddHeader("Content-disposition", "attachment; filename=" + Filename);
                        PdfReader pdrRead = new PdfReader(this.Context.Response.OutputStream);
                        PdfStamper pdfStamp = new PdfStamper(pdrRead, this.Context.Response.OutputStream);
                        pdfStamp.Close();
                    }
                }
                catch (Exception e)
                { throw new clsOutputException(e.Message, e); }
            }

            public void StartCreate(out string ResultFilename)
            {
                try
                {
                    ResultFilename = Path.Combine(this.PDFDirectory, string.Format("{0:MM-dd-yyyy_h_m_s.ffff}.pdf", DateTime.Now));
                    this.StartCreate(ResultFilename);
                }
                catch (clsOutputException ee)
                {
                    throw ee;
                }
            }

            public void StartCreate(string DesiredFilename)
            {
                try
                {
                    this._doc = new Document(page.Size, page.Left, page.Right, page.Top, page.Bottom);
                    this._doc.AddAuthor("NAVIFA");
                    this._doc.AddCreator("NAV-ASPX");
                    this._doc.AddKeywords(DesiredFilename);
                    this._doc.AddSubject("NAV Document");
                    this._doc.AddTitle("NAV Document");
                    if (System.Text.RegularExpressions.Regex.Match(DesiredFilename, @"^[C-Z]:").Success) { this.PDFDirectory = Path.GetDirectoryName(DesiredFilename); /*throw new Exception("Pattern Matches");*/ }
                    
                    if (!Directory.Exists(this.PDFDirectory)) { Directory.CreateDirectory(this.PDFDirectory); }
                    DesiredFilename = Path.Combine(this.PDFDirectory, DesiredFilename);
                    //this.CleanUp(); //---> commented just to keep the old generated files in place
                    using (FileStream fs = new FileStream(DesiredFilename, FileMode.Create))
                    {
                        PdfWriter.GetInstance(this._doc, fs);
                        this._doc.Open();
                        foreach (IElement elem in HTMLWorker.ParseToList(new StringReader(this.HTML), this.style))
                        {
                            //foreach (Chunk chunk in elem.Chunks)
                            //{
                            //    chunk.Font = new Font(Font.FontFamily.TIMES_ROMAN, 10f);
                            //    HttpContext.Current.Response.Write("<p>" + chunk.Type + "</p>");
                            //}
                            this._doc.Add(elem);
                        }
                        this._doc.Close();
                        fs.Close();
                    }
                }
                catch (Exception e)
                { throw new clsOutputException(e.Message, e); }
            }

            private void CleanUp()
            {
                if (!Directory.Exists(this.PDFDirectory)) { return; }
                foreach (string file in Directory.GetFiles(this.PDFDirectory))
                {
                    if (File.GetCreationTime(file).ToFileTimeUtc() <= DateTime.Now.AddDays(-1).ToFileTimeUtc())
                    {
                        try
                        { File.Delete(file); }
                        catch { }
                    }
                }
            }

            public void Dispose()
            {
                this._doc.Dispose();
                this.HTML = string.Empty;
                this.page = null;
                this.style = null;
                GC.SuppressFinalize(this);
            }
        }

    }

    public class StyleSheet : iTextSharp.text.html.simpleparser.StyleSheet { }

    public class PageSize : iTextSharp.text.PageSize { }

    class PageSetup
    {
        public float Left { get; set; }
        public float Right { get; set; }
        public float Top { get; set; }
        public float Bottom { get; set; }
        public Rectangle Size { get; set; }

        public PageSetup(Rectangle size, float left, float right, float top, float bottom)
        {
            this.Left = left;
            this.Right = right;
            this.Size = size;
            this.Top = top;
            this.Bottom = bottom;
        }
    }

    /// <summary>
    /// The Exception Handler for clsOutput and clsPDF Classes
    /// </summary>
    public class clsOutputException : Exception
    {
        #region Properties

        protected string _Title;
        /// <summary>
        /// Contains the title of the error message, useful for retrieving the title for the error.
        /// </summary>
        public string Title { get { return this._Title; } }

        #endregion

        #region Methods

        public clsOutputException() { }
        public clsOutputException(string message) : base(message) { }
        public clsOutputException(string message, Exception innerException) : base(message, innerException) { }
        public clsOutputException(string message, string title) : base(message) { this._Title = title; }

        #endregion
    }

}