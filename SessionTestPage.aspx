<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SessionTestPage.aspx.cs" Inherits="NAV.SessionTestPage" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Session Test Page</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:Button ID="Button1" runat="server" Text="Back to Classic" 
            onclick="Button1_Click" />
    </div>    
        <iframe src="https://<%= Request.ServerVariables["SERVER_NAME"] + ":" + Request.ServerVariables["SERVER_PORT"]%>/integration/RefreshPage.asp" frameborder="0" ></iframe>
    </form>
</body>
</html>
