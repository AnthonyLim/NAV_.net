<%@ Page Title="" Language="C#" MasterPageFile="~/NAV.Master" AutoEventWireup="true" CodeBehind="SwitchHistory.aspx.cs" Inherits="NAV.Portfolio.SwitchHistory" %>

<%@ Register src="UserControl/ucSwitchDetails.ascx" tagname="ucSwitchDetails" tagprefix="uc1" %>
<%@ Register src="UserControl/ucHeader.ascx" tagname="ucHeader" tagprefix="uc2" %>
<%@ Register src="UserControl/ucCurrentPortfolio.ascx" tagname="ucCurrentPortfolio" tagprefix="uc3" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<br />
<br />
<table width="100%">    
    <tr>
        <td>
            <uc2:ucHeader ID="ucHeader1" runat="server" />
        </td>
    </tr>
    <tr>
        <td>
            <uc3:ucCurrentPortfolio ID="ucCurrentPortfolio" runat="server" />
        </td>
    </tr>
    <tr>
        <td>
            <div id="divHistoryHolder" runat="server"></div>
        </td>
    </tr>
</table>



</asp:Content>