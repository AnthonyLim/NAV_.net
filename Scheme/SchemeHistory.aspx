<%@ Page Title="" Language="C#" MasterPageFile="~/NAV.Master" AutoEventWireup="true" CodeBehind="SchemeHistory.aspx.cs" Inherits="NAV.Scheme.SchemeHistory" %>

<%@ Register src="UserControl/ucHeader.ascx" tagname="ucHeader" tagprefix="uc1" %>

<%@ Register src="UserControl/ucCurrentHoldings.ascx" tagname="ucCurrentHoldings" tagprefix="uc2" %>

<%@ Register src="UserControl/ucContributions.ascx" tagname="ucContributions" tagprefix="uc3" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<br />
<br />
<table width="100%">    
    <tr>
        <td>
            <uc1:ucHeader ID="ucHeader1" runat="server" />
        </td>
    </tr>
    <tr>
        <td>
            <uc2:ucCurrentHoldings ID="ucCurrentHoldings1" runat="server" />
        </td>
    </tr>
    <tr>
        <td>
            <uc3:ucContributions ID="ucContributions1" runat="server" />
        </td>
    </tr>    
    <tr>
        <td>
            <div id="divHistoryHolder" runat="server"></div>
        </td>
    </tr>
</table>
</asp:Content>
