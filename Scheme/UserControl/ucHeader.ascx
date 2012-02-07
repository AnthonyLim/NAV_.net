<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucHeader.ascx.cs" Inherits="NAV.Scheme.UserControl.ucHeader" %>
<table class='main_info' border="0" cellpadding="0" cellspacing="0" width="100%">
    <thead align="left">
        <tr>
            <td colspan="2">
                <h2><asp:Label runat="server" ID="lblValue_ClientName"/></h2>
            </td>
        </tr>        
    </thead>
    <tbody class='member_info'>
    <tr> 
        <td style="width:1%" align="left">
            <asp:Label runat="server" ID="lblTitle_Company" Text="Company" />
        </td>
        <td style="width:1%" align="left">
            <asp:Label runat="server" ID="lblValue_Company"/>
        </td>
    </tr>    
    <tr> 
        <td style="width:1%" nowrap="nowrap" align="left">
            <asp:Label runat="server" ID="lblTitle_SchemeType" Text="Scheme Type" />
        </td>
        <td style="width:1%" nowrap="nowrap" align="left">
            <asp:Label runat="server" ID="lblValue_PortfolioType"/>
        </td>
    </tr>    
    <tr> 
        <td style="width:1%" nowrap="nowrap" align="left">
            <asp:Label runat="server" ID="lblTitle_Currency" Text="Currency" />
        </td>
        <td style="width:1%" nowrap="nowrap" align="left">
            <asp:Label runat="server" ID="lblValue_Currency"/>
        </td>
        <td style="width:1%" nowrap="nowrap" align="left">
            <asp:Label runat="server" ID="lblTitle_SchemeCurrency" Text="Scheme Currency" />
        </td>
        <td style="width:1%" nowrap="nowrap" align="left">
            <asp:Label runat="server" ID="lblValue_SchemeCurrency"/>
        </td>        
    </tr>
    <tr>
        <td style="width:1%" nowrap="nowrap" align="left">
            <asp:Label runat="server" ID="lblTitle_AccountNumber" Text="AccountNumber" />
        </td>
        <td style="width:1%" nowrap="nowrap" align="left">
            <asp:Label runat="server" ID="lblValue_AccountNumber"/>
        </td>
        <td style="width:1%" nowrap="nowrap" align="left">
            <asp:Label runat="server" ID="lblTitle_TotalContribution" Text="Total Contribution" />
        </td>
        <td style="width:1%" nowrap="nowrap" align="left">
            <asp:Label runat="server" ID="lblValue_TotalContribution"/>
        </td>
    </tr>    
    <tr>
        <td style="width:1%" nowrap="nowrap" align="left">
            <asp:Label runat="server" ID="lblTitle_PlanStatus" Text="Plan Status" />
        </td>
        <td style="width:1%" nowrap="nowrap" align="left">
            <asp:Label runat="server" ID="lblValue_PlanStatus"/>
        </td>
        <td style="width:1%" nowrap="nowrap" align="left">
            <asp:Label runat="server" ID="lblTitle_GainLossValue" Text="Gain/Loss Value" />
        </td>
        <td style="width:1%" nowrap="nowrap" align="left">
            <asp:Label runat="server" ID="lblValue_GainLossValue"/>
        </td>
    </tr>
    <tr>
        <td style="width:1%" nowrap="nowrap" align="left">
            <asp:Label runat="server" ID="lblTitle_StartDate" Text="Start Date" />
        </td>
        <td style="width:1%" nowrap="nowrap" align="left">
            <asp:Label runat="server" ID="lblValue_StartDate"/>
        </td>
        <td style="width:1%" nowrap="nowrap" align="left">
            <asp:Label runat="server" ID="lblTitle_GainLossPercent" Text="Gain/Loss %" />
        </td>
        <td style="width:1%" nowrap="nowrap" align="left">
            <asp:Label runat="server" ID="lblValue_GainLossPercent"/>
        </td>        
    </tr>
    <tr>
        <td style="width:1%" nowrap="nowrap" align="left">
            <asp:Label runat="server" ID="lblTitle_MaturityDate" Text="Maturity Date" />
        </td>
        <td style="width:1%" nowrap="nowrap" align="left">
            <asp:Label runat="server" ID="lblValue_MaturityDate"/>
        </td>
    </tr>    
    <tr>
        <td style="width:1%" nowrap="nowrap" align="left">
            <asp:Label runat="server" ID="lblTitle_PolicyCategory" Text="Policy Category" />
        </td>
        <td style="width:1%" nowrap="nowrap" align="left">
            <asp:Label runat="server" ID="lblValue_PolicyCategory"/>
        </td>
    </tr>
    <tr>
        <td style="width:1%" nowrap="nowrap" align="left">
            <asp:Label runat="server" ID="lblTitle_Profile" Text="Profile" />
        </td>
        <td style="width:1%" nowrap="nowrap" align="left">
            <asp:Label runat="server" ID="lblValue_Profile"/>
        </td>
    </tr>    
    <tr>
        <td style="width:1%" nowrap="nowrap" align="left">
            <asp:Label runat="server" ID="lblTitle_SpecialistInformation" Text="Specialist Information" />
        </td>
        <td style="width:1%" nowrap="nowrap" align="left">
            <asp:Label runat="server" ID="lblValue_SpecialistInformation"/>
        </td>
    </tr>    
    <tr>
        <td style="width:1%" nowrap="nowrap" align="left">
            <asp:Label runat="server" ID="lblTitle_Discretionary" Text="Discretionary" />
        </td>
        <td style="width:1%" nowrap="nowrap" align="left">
            <asp:Label runat="server" ID="lblValue_Discretionary"/>
        </td>
    </tr>
    </tbody>
</table>