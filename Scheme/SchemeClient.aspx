<%@ Page Title="" Language="C#" MasterPageFile="~/NAV.Master" AutoEventWireup="true" CodeBehind="SchemeClient.aspx.cs" Inherits="NAV.Scheme.SchemeClient" MaintainScrollPositionOnPostback="true" %>

<%@ Register src="UserControl/ucHeader.ascx" tagname="ucHeader" tagprefix="uc1" %>

<%@ Register src="UserControl/ucCurrentHoldings.ascx" tagname="ucCurrentHoldings" tagprefix="uc2" %>

<%@ Register src="UserControl/ucSwitchDetails.ascx" tagname="ucSwitchDetails" tagprefix="uc3" %>

<%@ Register src="UserControl/ucContributions.ascx" tagname="ucContributions" tagprefix="uc4" %>

<%@ Register src="UserControl/ucProposalDisplay.ascx" tagname="ucProposalDisplay" tagprefix="uc5" %>

<%@ Register src="UserControl/ucSwitchDetailsClient.ascx" tagname="ucSwitchDetailsClient" tagprefix="uc6" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<table border="0" align="left" cellpadding="0" cellspacing="0" width="100%" class="sms_template">
    <tr>
        <td align="left">
            <asp:LinkButton runat="server" ID="lbtnHistory" Text="History"  Font-Size="X-Small" OnClick="lbtnHistory_Click"></asp:LinkButton>
        </td>                 
    </tr>    
</table>
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
            <uc4:ucContributions ID="ucContributions1" runat="server" />
        </td>
    </tr>    
    <tr>
        <td>
            <uc5:ucProposalDisplay ID="ucProposalDisplay1" runat="server" />
        </td>
    </tr>
    <tr>
        <td>
            <uc5:ucProposalDisplay ID="ucProposalDisplay2" runat="server" Visible="false" />
        </td>
    </tr>    
    <tr>
        <td>
            <uc6:ucSwitchDetailsClient ID="ucSwitchDetailsClient1" runat="server" />
        </td>
    </tr>
</table>
</asp:Content>
