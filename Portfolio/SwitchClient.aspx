<%@ Page Title="" Language="C#" MasterPageFile="~/NAV.Master" AutoEventWireup="true" CodeBehind="SwitchClient.aspx.cs" Inherits="NAV.Portfolio.SwitchClient" MaintainScrollPositionOnPostback="true" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"></asp:ToolkitScriptManager>

 <script type="text/javascript" src="https://www.google.com/jsapi?key=ABQIAAAAJIc7LVNGdtica6PhZe_sFRQqgDN7nsaPpAk-csjZoyzusnQgwBSfA71pi0A-I91r0MmWO8okG_Zq6Q"></script>
    <script type="text/javascript">
        google.load("jquery", "1.7.1");
        // google.load("jqueryui", "1.8.1");
    </script>
    <!--
    <script src="../js/domready.js"></script>
    <script src="../js/jsSelector.js"></script>
    -->
    <script src="../js/switchJS.js"></script>
    
    <script>
        $(function() {
            $('#<%= txtPopupFund.ClientID %>').removeAttr('onkeypress').removeAttr('onchange');
        });

        $(function() {
            $('#<%= gvFunds.ClientID %> a').attr('onclick', "getFlickerSolved('<%=PopupFundSearch.ClientID%>');");
        });        
    </script>

    <style type="text/css">
        .modalBox
        {
            background-color : #f5f5f5;
            border-width: 3px;
            border-style: solid;
            border-color: Blue;
            padding: 3px;
        }
        .modalBackground
        {
            background-color: Gray;
            filter: alpha(opacity=50);
            opacity: 0.5;
        }
        .multilineTextbox
        { 
            resize: none;
        }
        .style1
        {
            width: 35%;
        }
        .style2
        {
            width: 65%;
        }
    </style>
<script language="javascript" type="text/javascript">
    function hidePopup(oModalPopup) {
        var modalPopup = $find(oModalPopup);
        var hasIFAPermit = '<%=Session["IFAPermit"]%>'
        if (Boolean.parse(hasIFAPermit) == true) {
            $find('bhvrThirdSwitchPopup', null).show();
            modalPopup.hide();
        }
        else {
            $find('bhvrSecondSwitchPopup', null).show();
            modalPopup.hide();
        }
    } 

    function checkMobile() {
        if (document.getElementById('ctl00_ContentPlaceHolder1_txtSMSMobileNo').value == '') {
            alert('Please enter mobile number.');
            document.getElementById('ctl00_ContentPlaceHolder1_txtSMSMobileNo').focus()
            return false;
        }
    }
    function checkSecurityCode() {
        if (document.getElementById('ctl00_ContentPlaceHolder1_txtSecurityCode').value == '') {
            alert('Please enter your security code.');
            document.getElementById('ctl00_ContentPlaceHolder1_txtSecurityCode').focus();
            return false;
        }
    }
    function showSecurityCodePanel() {
        var divSecurityCode = document.getElementById('divSecurityCodePanel');
        var divApprove = document.getElementById('divApprovePanel');
        divSecurityCode.style.display = "inline";
//        divApprove.style.display = "none";
    }
    function showApproveSwitchPanel() {
        var divSecurityCode = document.getElementById('divSecurityCodePanel');
        var divApprove = document.getElementById('divApprovePanel');
        divSecurityCode.style.display = "none";
        divApprove.style.display = "inline";
    }
    function halt() {
        return false;
    }
</script>
<table border="0" align="left" cellpadding="0" cellspacing="0" width="100%" class="sms_template">
    <tr>
        <td>
            <asp:LinkButton runat="server" ID="lbtnHistory" Text="History"  Font-Size="X-Small" OnClick="lbtnHistory_Click"></asp:LinkButton>
        </td>
    </tr>
</table>    
<br />&nbsp;
<table class='main_info' border="0" cellpadding="0" cellspacing="0" width="100%">
    <thead align="left">
        <tr>
            <td colspan="2">
                <h2><asp:Label runat="server" ID="lblValue_PortfolioName"/></h2>
            </td>
        </tr>        
    </thead>
    <tbody class='member_info'>
    <tr> 
        <td style="width:21%" align="left">
            <asp:Label runat="server" ID="lblTitle_Company" Text="Company" />
        </td>
        <td style="width:79%" align="left">
            <asp:Label runat="server" ID="lblValue_Company"/>
        </td>
    </tr>    
    <tr> 
        <td style="width:1%" nowrap="nowrap" align="left">
            <asp:Label runat="server" ID="lblTitle_PortfolioType" Text="Portfolio Type" />
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
    </tr>
    <tr>
        <td style="width:1%" nowrap="nowrap" align="left">
            <asp:Label runat="server" ID="lblTitle_AccountNumber" Text="AccountNumber" />
        </td>
        <td style="width:1%" nowrap="nowrap" align="left">
            <asp:Label runat="server" ID="lblValue_AccountNumber"/>
        </td>
    </tr>    
    <tr>
        <td style="width:1%" nowrap="nowrap" align="left">
            <asp:Label runat="server" ID="lblTitle_PlanStatus" Text="Plan Status" />
        </td>
        <td style="width:1%" nowrap="nowrap" align="left">
            <asp:Label runat="server" ID="lblValue_PlanStatus"/>
        </td>
    </tr>        
    <tr>
        <td style="width:1%" nowrap="nowrap" align="left">
            <asp:Label runat="server" ID="lblTitle_StartDate" Text="Start Date" />
        </td>
        <td style="width:1%" nowrap="nowrap" align="left">
            <asp:Label runat="server" ID="lblValue_StartDate"/>
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
<table  border="0" align="left" cellpadding="0" cellspacing="0" width="100%">
    <tr>
        <td  class="table1_header" align="left">           
            <b><asp:Label runat="server" ID="lblTitle_CurrentHoldings" Text="Current Holdings"/></b>
        </td>
    </tr>
    <tr>
        <td>
            <div id="table1_container">
            <asp:GridView runat="server" ID="gvPortfolioDetails" AutoGenerateColumns="false" Width="100%" ShowFooter="true" BorderStyle="None" CellPadding="0" CellSpacing="0" UseAccessibleHeader="true" CssClass="table1">
                <Columns>
                    <asp:TemplateField HeaderText="Acq Date" HeaderStyle-HorizontalAlign="Right" HeaderStyle-BorderStyle="None" FooterStyle-BorderStyle="None" HeaderStyle-VerticalAlign="Top">
                        <HeaderStyle CssClass="t1_column_color1" VerticalAlign="Middle" HorizontalAlign="Center"/>
                        <ItemStyle HorizontalAlign="Center" BorderStyle="None" CssClass="t1_column_color1" />                        
                        <FooterStyle BorderStyle="None" CssClass="t1_column_color1"/>
                        <ItemTemplate>
                            <asp:Label ID="lblDataDatePortfolio" runat="server"  htmlencode="false" text='<%# CheckNull(Eval("propDataDate", "{0:dd/MM/yyyy}"))%>'  />
                        </ItemTemplate>                        
                    </asp:TemplateField>                    
                    <asp:BoundField DataField="propNameOfFund" 
                                    HeaderText="Fund Name" HeaderStyle-BorderStyle="None" HeaderStyle-VerticalAlign="Middle" HeaderStyle-CssClass="t1_column_color2"
                                    ItemStyle-HorizontalAlign="Left" ItemStyle-BorderStyle="None" ItemStyle-CssClass="t1_column_color2"
                                    FooterStyle-BorderStyle="None" FooterStyle-CssClass="t1_column_color2"/>
                    <asp:BoundField DataField="propNumberOfUnits" DataFormatString="{0:n4}" HtmlEncode="false"
                                    HeaderText="Units" HeaderStyle-HorizontalAlign="Center" HeaderStyle-BorderStyle="None" HeaderStyle-VerticalAlign="Middle" HeaderStyle-CssClass="t1_column_color1"
                                    ItemStyle-HorizontalAlign="Right" ItemStyle-BorderStyle="None" ItemStyle-CssClass="t1_column_color1"
                                    FooterStyle-BorderStyle="None" FooterStyle-CssClass="t1_column_color1"/>
                    <asp:TemplateField HeaderText="Price" HeaderStyle-HorizontalAlign="Right" HeaderStyle-BorderStyle="None" FooterStyle-BorderStyle="None" HeaderStyle-VerticalAlign="Top">
                        <ItemStyle HorizontalAlign="Right"  BorderStyle="None" CssClass="t1_column_color2" />
                        <HeaderStyle CssClass="t1_column_color2" VerticalAlign="Middle" HorizontalAlign="Center" />
                        <FooterStyle BorderStyle="None" CssClass="t1_column_color2"/>
                        <ItemTemplate>
                            <asp:Label runat="server" ID="lblFundCurrencyPortfolio" Text='<% #Eval("propFundCurrency") %>'/>&nbsp;&nbsp;<asp:Label runat="server" ID="lblPrice" Text='<% #Eval("propPrice", "{0:0.0000}") %>'/>
                            <br />
                            <asp:Label ID="Label1" runat="server"  htmlencode="false" text='<%# Eval("propDatePriceUpdated", "({0:dd/MM/yyyy})") %>'  />
                        </ItemTemplate>                        
                    </asp:TemplateField>
                    <asp:TemplateField FooterStyle-BorderStyle="None">
                        <HeaderStyle BorderStyle="None" CssClass="t1_column_color1" VerticalAlign="Middle" HorizontalAlign="Center" />
                        <ItemStyle HorizontalAlign="Right"  BorderStyle="None" CssClass="t1_column_color1"/>
                        <FooterStyle BorderStyle="None" CssClass="t1_column_color1"/>
                        <HeaderTemplate>
                            <asp:Label runat="server" ID="gvHeaderPurchaseCostFund" Text="Purchase Cost" />
                        </HeaderTemplate>                        
                        <ItemTemplate>
                            <asp:Label runat="server" ID="gvDetailsPurchaseCostFund" text='<%# Eval("propPurchaseCostFund", "{0:n2}") %>'/>
                        </ItemTemplate>                        
                    </asp:TemplateField>
                    <asp:TemplateField FooterStyle-BorderStyle="None">
                        <HeaderStyle BorderStyle="None" CssClass="t1_column_color2" VerticalAlign="Middle" HorizontalAlign="Center"/>
                        <ItemStyle HorizontalAlign="Right" BorderStyle="None" CssClass="t1_column_color2" />
                        <FooterStyle BorderStyle="None" CssClass="t1_column_color2"/>
                        <HeaderTemplate>
                            <asp:Label runat="server" ID="gvHeaderPurchaseCostPortfolio" Text="Purchase Cost" />
                            <br />
                            <asp:Label runat="server" ID="gvHeaderPurchaseCostFundPortfolioCurrency"/>                            
                        </HeaderTemplate>                        
                        <ItemTemplate>
                            <asp:Label runat="server" ID="gvDetailsPurchaseCostPortfolio" text='<%# Eval("propPurchaseCostPortfolio", "{0:n2}") %>'/>
                        </ItemTemplate>                        
                    </asp:TemplateField>
                    <asp:TemplateField ItemStyle-HorizontalAlign="Right" FooterStyle-BorderStyle="None">
                        <HeaderStyle BorderStyle="None" CssClass="t1_column_color1" VerticalAlign="Middle" HorizontalAlign="Center" />
                        <ItemStyle HorizontalAlign="Right" BorderStyle="None" CssClass="t1_column_color1" />
                        <FooterStyle BorderStyle="None" CssClass="t1_column_color1"/>
                        <HeaderTemplate>
                            <asp:Label runat="server" ID="gvHeaderGainLoss" Text="Gain/Loss" />
                            <br />
                            <asp:Label runat="server" ID="gvHeaderGainLossCurrency"/>
                        </HeaderTemplate>                                            
                        <ItemTemplate>
                            <asp:Label runat="server" ID="GainLostPortfolio" Text='<% #Eval("propGainOrLossPortfolio", "{0:n0}") %>'/>
                            <br />
                            <asp:Label runat="server" ID="GainLostPercent" Text='<% #Eval("propGainOrLossPercent", "{0:0.00}%") %>'/>
                        </ItemTemplate>                        
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderStyle BorderStyle="None" CssClass="t1_column_color2" VerticalAlign="Middle" HorizontalAlign="Center"/>
                        <ItemStyle HorizontalAlign="Right" BorderStyle="None" CssClass="t1_column_color2" />
                        <FooterStyle HorizontalAlign="Right" BorderStyle="None" CssClass="t1_column_color2"/>
                        <HeaderTemplate>
                            <asp:Label runat="server" ID="gvHeaderValueClientCur" Text="Value Client Cur" />
                            <br />
                            <asp:Label runat="server" ID="gvHeaderValueClientCurCurrency"/>
                        </HeaderTemplate>                                            
                        <ItemTemplate>
                            <asp:Label runat="server" ID="gvDetailsCurrentValueClient" Text='<% #Eval("propCurrentValueClient", "{0:n0}") %>'/>     
                        </ItemTemplate>
                        <FooterTemplate>
                            Total:&nbsp;<asp:Label runat="server" ID="gvFooterCurrentValueClient" Font-Bold="true"/>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="propAllocationPercent" HeaderText="Allocation %" DataFormatString="{0:0.00}" 
                                    HeaderStyle-HorizontalAlign="Center" HeaderStyle-BorderStyle="None" HeaderStyle-VerticalAlign="Top" HeaderStyle-CssClass="t1_column_color1"
                                    ItemStyle-HorizontalAlign="Right" ItemStyle-BorderStyle="None"  ItemStyle-CssClass="t1_column_color1"
                                    FooterStyle-BorderStyle="None" FooterStyle-CssClass="t1_column_color1"/>
                </Columns>
            </asp:GridView>
            </div>
        </td>
    </tr>
</table>
<table  border="0" align="left" cellpadding="0" cellspacing="0" width="100%">
    <tr>
        <td  class="table1_header" align="left">           
            <b><asp:Label runat="server" ID="lblTitle_ProposedSwitch" Text="IFA Proposed Switch"/></b>
        </td>
    </tr>
    <tr>
        <td align="left">
            <asp:Label runat="server" ID="lblSwitchDetails_IFAStatusTitle" Text="Status: "/>&nbsp;<asp:Label runat="server" ID="lblSwitchDetails_IFAStatusValue"/>
        </td>
    </tr>    
    <tr>
        <td>
            <div id="table1_container">
            <asp:GridView runat="server" ID="gvSwitchDetails_IFA" AutoGenerateColumns="false" Width="100%" ShowFooter="true" BorderStyle="None" CellPadding="0" CellSpacing="0" UseAccessibleHeader="true" CssClass="table1" >
                <Columns>
                    <asp:TemplateField ItemStyle-HorizontalAlign="Right" >                        
                        <HeaderStyle BorderStyle="None" CssClass="t1_column_color1" VerticalAlign="Middle" HorizontalAlign="Center"/>
                        <ItemStyle HorizontalAlign="Left" BorderStyle="None" CssClass="t1_column_color1" />                        
                        <FooterStyle BorderStyle="None" CssClass="t1_column_color1"/>                                                                                               
                        <HeaderTemplate>
                            <asp:Label runat="server" ID="gvSwitchHeaderFundName" Text="Fund Name"  />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <%# DataBinder.Eval(Container.DataItem, "propFund.propFundName")%>
                        </ItemTemplate>                        
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Price">
                        <HeaderStyle BorderStyle="None" CssClass="t1_column_color2" VerticalAlign="Middle" HorizontalAlign="Center"/>
                        <ItemStyle HorizontalAlign="Right" BorderStyle="None" CssClass="t1_column_color2" />
                        <FooterStyle BorderStyle="None" CssClass="t1_column_color2"/>
                        <ItemTemplate>
                            <asp:Label runat="server" ID="lblFundCurrencySwitch" Text='<%# DataBinder.Eval(Container.DataItem, "propFund.propCurrency")%>'/>&nbsp;&nbsp;<asp:Label ID='lblPrice' runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "propFund.propPrice", "{0:n4}")%>' />
                        </ItemTemplate>
                    </asp:TemplateField>                    
                    <asp:TemplateField>
                        <HeaderStyle BorderStyle="None" CssClass="t1_column_color1" VerticalAlign="Middle" HorizontalAlign="Center"/>
                        <ItemStyle HorizontalAlign="Center" BorderStyle="None" CssClass="t1_column_color1" />                        
                        <FooterStyle BorderStyle="None" CssClass="t1_column_color1"/>                                        
                        <HeaderTemplate>
                            <asp:Label runat="server" ID="gvSwitchHeaderUnits" Text="Unit" />
                        </HeaderTemplate>                    
                        <ItemStyle HorizontalAlign="Right" BorderStyle="None" />
                        <ItemTemplate>
                            <asp:Label runat="server" ID="gvSwitchDetailsLblUnits" Text='<% #Eval("propUnits", "{0:n4}") %>'/>                                 
                        </ItemTemplate>
                    </asp:TemplateField>                    
                    <asp:TemplateField>
                        <HeaderStyle BorderStyle="None" CssClass="t1_column_color2" VerticalAlign="Middle" HorizontalAlign="Center"/>
                        <ItemStyle HorizontalAlign="Right" BorderStyle="None" CssClass="t1_column_color2" />
                        <FooterStyle HorizontalAlign="Right" BorderStyle="None" CssClass="t1_column_color2"/>                                                                   
                        <HeaderTemplate>
                            <asp:Label runat="server" ID="gvSwitchHeaderValue" Text="Value" />
                        </HeaderTemplate>                    
                        <ItemTemplate>
                            <asp:Label runat="server" ID="gvSwitchDetailsLblValue" Text='<% #Eval("propValue", "{0:n0}") %>'/>                                 
                        </ItemTemplate>
                        <FooterTemplate>
                            Total:&nbsp;<asp:Label runat="server" ID="gvSwitchFooterLblTotalValue" Font-Bold="true" />
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Allocation %">
                        <HeaderStyle BorderStyle="None" CssClass="t1_column_color1" VerticalAlign="Middle" HorizontalAlign="Center"/>
                        <ItemStyle HorizontalAlign="Right" BorderStyle="None" CssClass="t1_column_color1" />                        
                        <FooterStyle HorizontalAlign="Right" BorderStyle="None" CssClass="t1_column_color1"/>
                        <ItemTemplate>
                            <asp:Label runat="server" ID="gvSwitchHeaderValue" Text='<% #Eval("propAllocation", "{0:0.00}") %>' />
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:Label runat="server" ID="gvSwitchFooterLblTotalAllocation" Font-Bold="true" />
                        </FooterTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            </div>
        </td>
    </tr>
    <tr>
        <td align="left" valign="top">
            <div><asp:Label runat="server" ID="lblIFASwitchDescTitle" Text="Description: "/></div>
            <%--<div><asp:TextBox runat="server" ID="txtProposedSwitchDesc" TextMode="MultiLine" Rows="5" Columns="80" Enabled="false" BackColor="White" BorderStyle="None"></asp:TextBox></div>--%>
            <div><asp:Label ID="txtProposedSwitchDesc" runat="server" style="width:350px;" Font-Size="Smaller"/></div>
        </td>
    </tr>
</table>
<br />&nbsp;
<table width="100%">
    <tr>
        <td class="table1_header" align="left">
            <b><asp:Label runat="server" ID="lblTitle_AmendSwitch" Text="Amend Switch" Visible="false"/></b>
        </td>
    </tr>
    <tr>
        <td align="left">
            <asp:Label runat="server" ID="lblSwitchStatusTitle" Text="Status: " Visible="false"/>&nbsp;<asp:Label runat="server" ID="lblSwitchStatusValue" Visible="false"/>
        </td>
    </tr>     
    <tr>
        <td>
            <div class="table2">
            <asp:GridView runat="server" ID="gvAmendSwitch" AutoGenerateColumns="false" Width="100%" ShowFooter="true" BorderStyle="None" CellPadding="0" CellSpacing="0" UseAccessibleHeader="true" CssClass="table2">
                <Columns>
                    <asp:TemplateField>
                        <HeaderStyle CssClass="t2_header" />
                        <ItemStyle HorizontalAlign="Center" BorderStyle="None"/>
                        <FooterStyle HorizontalAlign="Center"  CssClass="t2_results"/>
                        <ItemTemplate>
                            <asp:ImageButton runat="server" id="ibtnRemoveFund" ImageUrl="~/Images/remove_btn.png" OnClientClick="return confirm('Proceed removing this fund?')" OnCommand="lbtnRemoveFund_Click" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "propFund.propFundID")%>' Visible='<%#Eval("propIsDeletable")%>' />
                            <%--<asp:Image runat="server" id="ibtnEditFund" ImageUrl="~/Images/remove_btn.png" onclick="return confirm('Proceed removing this fund?');" OnCommand="lbtnRemoveFund_Click" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "propFund.propFundID")%>' Visible='<%#Eval("propIsDeletable")%>' onmouseover="javascript:this.style.cursor='pointer'"/>--%>
                            <%--<asp:Image runat="server" id="ibtnEditFund" ImageUrl="~/Images/remove_btn.png" onclick="removeFund(this);" OnCommand="lbtnRemoveFund_Click" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "propFund.propFundID")%>' Visible='<%#Eval("propIsDeletable")%>' onmouseover="javascript:this.style.cursor='pointer'"/>--%>
                        </ItemTemplate>
                        <FooterTemplate>                                                        
                            <img src="../Images/add_btn.png" onclick='javascript:popUp_AddFunds();' onmouseover="javascript:this.style.cursor='pointer'"/>
                        </FooterTemplate>                        
                    </asp:TemplateField>
                    <asp:TemplateField ItemStyle-HorizontalAlign="Right" >
                        <HeaderStyle HorizontalAlign="Left"  VerticalAlign="Top" CssClass="t2_header"/>
                        <ItemStyle HorizontalAlign="Left"  BorderStyle="None"/>
                        <FooterStyle HorizontalAlign="Left" CssClass="t2_results"/>
                        <FooterTemplate>
                            <a id="lbtnAddFund" href='javascript:popUp_AddFunds();'>Add more funds</a>
                        </FooterTemplate>
                        <HeaderTemplate>
                            <asp:Label runat="server" ID="gvSwitchHeaderFundName" Text="Fund Name"  />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <div class="added_fund"  style='<%# HideClickableFundLinks(Eval("propIsDeletable"))%>'>                                                            
                                <a href='javascript:popUp_EditFunds(<%# DataBinder.Eval(Container.DataItem, "propFund.propFundID")%>);' id="lbtnFundName"><%# DataBinder.Eval(Container.DataItem, "propFund.propFundName") %></a>
                            </div>
                            <div style='<%# ShowClickableFundLinks(Eval("propIsDeletable"))%>'  >
                                <%# DataBinder.Eval(Container.DataItem, "propFund.propFundName")%>
                            </div>                                
                            <asp:HiddenField runat="server" ID="hfSwitchDetailsID" Value='<%# Eval("propSwitchDetailsID")%>' />
                            <asp:HiddenField runat="server" ID="hfSelectedFundID" Value='<%# DataBinder.Eval(Container.DataItem, "propFund.propFundID")%>' />
                            <asp:HiddenField runat="server" ID="hfCurrencyMultiplier" Value='<%# Eval("propCurrencyMultiplier")%>' />                                                        
                        </ItemTemplate>                        
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Price">
                        <HeaderStyle HorizontalAlign="Right"  VerticalAlign="Top" CssClass="t2_header"/>
                        <ItemStyle HorizontalAlign="Right"  BorderStyle="None"/>
                        <FooterStyle CssClass="t2_results"/>
                        <ItemTemplate>
                            <asp:Label runat="server" ID="lblFundCurrencySwitch" Text='<%# DataBinder.Eval(Container.DataItem, "propFund.propCurrency")%>'/>&nbsp;&nbsp;<asp:Label ID='lblPrice' runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "propFund.propPrice", "{0:n4}")%>' />
                        </ItemTemplate>
                    </asp:TemplateField>                    
                    <asp:TemplateField>
                        <HeaderStyle HorizontalAlign="Right" VerticalAlign="Top" CssClass="t2_header" />
                        <HeaderTemplate>
                            <asp:Label runat="server" ID="gvSwitchHeaderUnits" Text="Unit" />
                        </HeaderTemplate>                    
                        <ItemStyle HorizontalAlign="Right" BorderStyle="None" />
                        <ItemTemplate>
                            <asp:Label runat="server" ID="gvSwitchDetailsLblUnits" Text='<% #Eval("propUnits", "{0:n4}") %>'/>
                            <asp:HiddenField runat="server" ID="gvhfSwitchDetailsLblUnits" Value='<% #Eval("propUnits", "{0:n4}") %>' />     
                        </ItemTemplate>
                        <FooterStyle HorizontalAlign="Right" CssClass="t2_results"/>
                    </asp:TemplateField>                    
                    <asp:TemplateField>
                        <HeaderStyle HorizontalAlign="Right" VerticalAlign="Top" CssClass="t2_header" />
                        <HeaderTemplate>
                            <asp:Label runat="server" ID="gvSwitchHeaderValue" Text="Value" />
                        </HeaderTemplate>                    
                        <ItemStyle HorizontalAlign="Right" BorderStyle="None" />
                        <ItemTemplate>
                            <asp:Label runat="server" ID="gvSwitchDetailsLblValue" Text='<% #Eval("propValue", "{0:n0}") %>'/>     
                            <asp:HiddenField runat="server" ID="gvhfSwitchHeaderValue" Value='<% #Eval("propValue", "{0:n0}") %>' />                            
                        </ItemTemplate>
                        <FooterStyle HorizontalAlign="Right" CssClass="t2_results"/>
                        <FooterTemplate>
                            Total:&nbsp;<asp:Label runat="server" ID="gvSwitchFooterLblTotalValue" />
                        </FooterTemplate>
                    </asp:TemplateField>                                        
                    <asp:TemplateField HeaderText="Allocation %">
                        <HeaderStyle HorizontalAlign="Right"  VerticalAlign="Top" CssClass="t2_header"/>
                        <ItemStyle HorizontalAlign="Right"  BorderStyle="None"/>
                        <FooterStyle HorizontalAlign="Right" CssClass="t2_results"/>
                        <ItemTemplate>
                            <asp:Label runat="server" ID="gvSwitchHeaderValue" Text='<% #Eval("propAllocation", "{0:0.00}") %>' />
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:Label runat="server" ID="gvSwitchFooterLblTotalAllocationOrig" />                            
                        </FooterTemplate>                        
                    </asp:TemplateField>                                                            
                    <asp:TemplateField HeaderText="Target Allocation %">
                        <HeaderStyle HorizontalAlign="Left"  VerticalAlign="Top" CssClass="t2_header" />
                        <ItemStyle HorizontalAlign="Right"  BorderStyle="None" />
                        <FooterStyle HorizontalAlign="Right" CssClass="t2_results" />
                        <ItemTemplate>
                            <asp:RequiredFieldValidator runat="server" ID="gvSwitchRfvTargetAllocation" ControlToValidate="gvSwitchTxtTargetAllocation" SetFocusOnError="true" Display="None" ValidationGroup="vgSwitchDetails" ></asp:RequiredFieldValidator>                            
                            <asp:CompareValidator runat="server" ID="gvSwitchCvTargetAllocation" ControlToValidate="gvSwitchTxtTargetAllocation" SetFocusOnError="true" Operator="DataTypeCheck" Type="Double"  Display="None" ValidationGroup="vgSwitchDetails"></asp:CompareValidator>
                            <asp:HiddenField runat="server" ID="hfCurrentValueClient"/>                                                        
                            <asp:RegularExpressionValidator ID="regexpName" runat="server" Display="Dynamic"
                                    ErrorMessage="(0.00 - 100.00)" 
                                    ControlToValidate="gvSwitchTxtTargetAllocation"      
                                    ValidationExpression="^\d*(|[.]\d{0,1}[0-9])?$" 
                                    Font-Size="X-Small"
                                    ValidationGroup="vgSwitchDetails"/>
                            <asp:RangeValidator runat="server" ID="gvSwitchRvTargetAllocation" ControlToValidate="gvSwitchTxtTargetAllocation" SetFocusOnError="true" MaximumValue="100" MinimumValue="0"  Type="Double" Display="Dynamic" ValidationGroup="vgSwitchDetails" ErrorMessage="(0.00 - 100.00)" Font-Size="X-Small"></asp:RangeValidator>
                            <asp:TextBox runat="server" ID="gvSwitchTxtTargetAllocation" Width="70" Text='<% #Eval("propAllocation", "{0:0.00}") %>'  MaxLength="6" CausesValidation="true" AutoPostBack="false" ValidationGroup="vgSwitchDetails" onblur='javascript:computeRows(this);' OnKeyUp='javascript:computeRows(this);' onkeypress="javascript:return onlyNumbers(this,event);"></asp:TextBox>
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:Label runat="server" ID="gvSwitchFooterLblTotalAllocation" />
                        </FooterTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            </div>
            <asp:HiddenField runat="server" ID="gvSwitchFooterHfTotalAllocation"/>
            <asp:HiddenField runat="server" ID="hfFundIDOrig"/>
            <asp:HiddenField runat="server" ID="hfFundIDNew"/>
            <asp:HiddenField runat="server" ID="hfCurrentValueClientTotal"/>
            <asp:HiddenField runat="server" ID="hfFundCount"/>
            <asp:HiddenField runat="server" ID="hfPopUpFundAction"/>
            <asp:ValidationSummary runat="server" ID="vsSwitchDetails" ShowSummary="false"  ShowMessageBox="true" ValidationGroup="vgSwitchDetails" EnableClientScript="true" HeaderText="Please input numeric values from 0.00 to 100.00" />
            <asp:Label ID="fixAjaxValidationSummaryErrorLabel" Text="" runat="server" OnPreRender="FixAjaxValidationSummaryErrorLabel_PreRender" />
        </td>
    </tr>    
</table>&nbsp;
<br />&nbsp;
<div runat="server" id="divAmendButtonSet" visible="false">
    <center>
        <button id="btnSave" onclick="javascript:SwitchSave();" type="button" causesvalidation="true" runat="server">Save</button>&nbsp;        
        <input type="button" id="btnAmend" runat="server" onclick="return validateSwitchAllocation();" value="Amend Switch"/>
        <asp:Button runat="server" ID="btnCancel"  Text="Cancel" UseSubmitBehavior="false" CausesValidation="false" OnClientClick='if (!confirm("Cancel amending this switch?")) { return; }' OnClick="btnCancel_Click" />
    </center>
</div>
    
    
<table>    
<tr><td>
<div id="divApprovePanel" >
<center>
    <input type="button" id="btnApproveSwitch" runat="server" value="Approve"/>
    <asp:Button ID="btnAmendSwitch" runat="server" Text="Amend Switch" OnClick="btnAmendSwitch_Click" />
    <asp:Button ID="btnDeclineSwitch" runat="server" Text="Decline Switch" />
    <asp:Button ID="btnContactMe" runat="server" Text="Contact Me" OnClientClick="$find('bhvrPopupContactMe', null).show();" />
</center>
</div>
</td></tr>
<tr><td>
<asp:ModalPopupExtender ID="mpeSecurityCodePanel" runat="server" TargetControlID="Button2" PopupControlID="SecurityCodePanel" CancelControlID="btnSecurityCodeCancel" BackgroundCssClass="modalBackground" DropShadow="true" BehaviorID="bhvrSecurityCodePanel" />
<div id="divSecurityCodePanel" style="background-color:#f5f5f5">
<asp:Panel runat="server" ID="SecurityCodePanel" style="display:none" CssClass="modalBox" DefaultButton="btnCheckSecurityCode">
    <center>
        <table>
            <tr><td>&nbsp;</td></tr>
            <tr>
                <td>
                    Security Code
                </td>
                <td>
                    <asp:TextBox ID="txtSecurityCode" runat="server"/>
                </td>
            </tr>  
        </table>
        <br />
        <asp:Button runat="server" ID="btnCheckSecurityCode" Text="Approve Switch" onclick="btnCheckSecurityCode_Click" OnClientClick="return checkSecurityCode()" />&nbsp;&nbsp;<asp:Button runat="server" ID="btnSecurityCodeCancel"  Text="Cancel"/>
        <br />
    </center>
</asp:Panel>
</div>
</td></tr>
</table>
<div>
    <asp:ModalPopupExtender ID="mdlPopupSms" runat="server" TargetControlID="btnApproveSwitch" CancelControlID="btnValidationCodeCancel" PopupControlID="panelPopupSms" BackgroundCssClass="modalBackground" DropShadow="true"></asp:ModalPopupExtender>
    
    <asp:Panel runat="server" ID="panelPopupSms" style="display:none" CssClass="modalBox" DefaultButton="SendValidationCode">
    <div style="text-align:left;width:100%;margin-top:1px;">
        <center>
            <table >
            <tr><td>&nbsp;</td></tr>
                <tr>
                    <td align="right" class="style1">
                        <asp:Label runat="server" ID="lblSMSMobileNo" Text="Mobile no.:" Width="100%"></asp:Label>
                    </td>                
                    <td class="style2">
                        <asp:TextBox runat="server" ID="txtSMSMobileNo" ></asp:TextBox>
                    </td>
                </tr>
            </table>                
            <br />
            <asp:Button runat="server" ID="SendValidationCode" OnClick="SendValidationCode_Click" Text="Send Validation Code"/>&nbsp;&nbsp;<asp:Button runat="server" ID="btnValidationCodeCancel" Text="Cancel"/>
            <br />
        </center>
    </div>
    </asp:Panel>
</div>

    <asp:ModalPopupExtender ID="mdlFirstSwitchPopup" runat="server" TargetControlID="Button2" PopupControlID="FirstSwitchPopup" CancelControlID="btnSwitchCancel" BackgroundCssClass="modalBackground" DropShadow="true" BehaviorID="bhvrFirstSwitchPopup" />
    
    <asp:Panel ID="FirstSwitchPopup" runat="server" style="display:none;" CssClass="modalBox"> 
        <div style="text-align:right;width:100%;margin-top:5px;">
            <asp:Label runat="server" ID="lblSwitchMessage" Text="You are about to perform this Switch. Are you sure you want to proceed?"></asp:Label><br /><br />
            <center>
            <input type="button" id="btnSwitchProceed" runat="server" onclick="hidePopup('bhvrFirstSwitchPopup');" value="Proceed"/>&nbsp;&nbsp;<asp:Button runat="server" ID="btnSwitchCancel" Text="Cancel" />
            </center>
            <asp:ModalPopupExtender ID="mdlSecondSwitchPopup" runat="server" TargetControlID="Button2" PopupControlID="SecondSwitchPopup" BackgroundCssClass="modalBackground" DropShadow="true" BehaviorID="bhvrSecondSwitchPopup" />
        </div>
    </asp:Panel>
    
    <asp:Panel ID="SecondSwitchPopup" runat="server" style="display:none;" CssClass="modalBox">
        <div style="text-align:right;width:100%;margin-top:5px;">
            <asp:Label runat="server" ID="lblMessage2" Text="IFA approval required. Request IFA approval for this amendment?"></asp:Label><br /><br />
            <center>
                <asp:Button ID="btnAmendYes" Text="Yes" runat="server" OnClientClick="hidePopup('bhvrSecondSwitchPopup')" />
                <asp:Button ID="btnAmendNo" runat="server" Text="No" OnClick="btnSwitchNo_Click" />
            </center>
            <asp:ModalPopupExtender ID="mdlThirdSwitchPopup" runat="server" TargetControlID="btnAmendYes" CancelControlID="btnAmendCancelSend" PopupControlID="ThirdSwitchPopup" BackgroundCssClass="modalBackground" DropShadow="true" BehaviorID="bhvrThirdSwitchPopup" />
        </div>
    </asp:Panel>    
    
    <asp:Panel runat="server" ID="ThirdSwitchPopup" style="display:none;" CssClass="modalBox" Width="350px" DefaultButton="btnAmendSaveSend">
        <div style="text-align:left;width:100%;margin-top:1px;">
        <h4>Amendment Description</h4>
            <table width="100%">
                <tr><td><asp:TextBox ID="txtAmendDesc" runat="server" TextMode="MultiLine" CssClass="multilineTextbox" Rows="10" width="95%"></asp:TextBox></td></tr>
                    <%
                    if (Boolean.Parse(Session["IFAPermit"].ToString()))
                    {
                    %>
                        <tr><td><asp:Label ID="lblSMSMobileNoApproval" runat="server" Text="Your mobile no.:" Width="35%"></asp:Label><asp:TextBox runat="server" ID="txtSMSMobileNoApproval" width="60%" ></asp:TextBox></td></tr>                        
                    <%
                    }
                    %>
            </table>
        <center>
            <br />&nbsp;
            <asp:Button runat="server" ID="btnAmendSaveSend" Text="Proceed" OnClick="btnAmendSave_Click" />&nbsp;&nbsp;
            <asp:Button runat="server" ID="btnAmendCancelSend" Text="Cancel" />
            <br />&nbsp;
        </center>
        </div>
    </asp:Panel>    

<asp:Button ID="Button2" runat="server"  Text="" style="display:none;"/>
<asp:ModalPopupExtender ID="mpeFundSearch" runat="server" TargetControlID="Button2" PopupControlID="PopupFundSearch" BackgroundCssClass="modalBackground" DropShadow="true" BehaviorID="bhvrPopupFundSearch" />
<asp:Panel runat="server" ID="PopupFundSearch" CssClass="modalBox" ScrollBars="Both" style="display:none;overflow:auto;" Height="700px" Width="800px" DefaultButton="btnPopupFund" >    
    <div style="text-align:left;width:90%;margin-top:1px;">               
        <table width="100%">
            <tr>
                <td>
                    &nbsp;
                </td>
            </tr>                                    
            <tr>
                <td>
                    <asp:Label runat="server" ID="lblFundName" Text="Fund Name" Font-Bold="true"/>
                    <asp:ValidationSummary runat="server" ID="vsFundSearch" ShowSummary="false"  ShowMessageBox="true" ValidationGroup="vgFundSearch" EnableClientScript="true"/>
                    <asp:RequiredFieldValidator runat="server" ID="RfvFundSearch" ControlToValidate="txtPopupFund" SetFocusOnError="true" Display="None" ValidationGroup="vgFundSearch" ErrorMessage="Please input a search criteria" ></asp:RequiredFieldValidator>
                    <asp:TextBox runat="server" ID="txtPopupFund" AutoPostBack="true" OnTextChanged="txtPopupFund_TextChanged" CausesValidation="true" ValidationGroup="vgFundSearch"/>&nbsp;<asp:Button runat="server" ID="btnPopupFund" Text="Search" UseSubmitBehavior="false" OnClick="btnPopupFund_Click" CausesValidation="true" ValidationGroup="vgFundSearch"/>
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td>
                    <div id="Div1">
                    <asp:GridView runat="server" ID="gvFunds" AutoGenerateColumns="false" Width="100%" BorderStyle="Solid" BorderWidth="1" CssClass="table1">
                        <Columns>
                            <asp:BoundField DataField="propFundID" Visible="false" />
                            <asp:TemplateField ItemStyle-HorizontalAlign="Right" >
                                <HeaderStyle CssClass="t1_column_color1" BorderStyle="None" VerticalAlign="Middle" HorizontalAlign="Center"/>
                                <ItemStyle HorizontalAlign="Left" BorderStyle="None" CssClass="t1_column_color1" />                        
                                <FooterStyle BorderStyle="None" CssClass="t1_column_color1"/>
                                <HeaderTemplate>
                                    <asp:Label runat="server" ID="gvHeaderFundName" Text="Fund Name"  />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:LinkButton runat="server" ID="lbtnFundName" Text='<%# Eval("propFundName")%>' ForeColor="Black" OnCommand="gvFunds_lbtnFundName_Click" CommandArgument='<%# Eval("propFundID")%>' ></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="propDatePriceUpdated" HeaderText="Date Price Update" DataFormatString="{0:dd/MM/yyyy}"                                    
                                    HeaderStyle-HorizontalAlign="Center" HeaderStyle-BorderStyle="None" HeaderStyle-VerticalAlign="Middle" HeaderStyle-CssClass="t1_column_color2"
                                    ItemStyle-HorizontalAlign="Center" ItemStyle-BorderStyle="None" ItemStyle-CssClass="t1_column_color2"
                                    FooterStyle-BorderStyle="None" FooterStyle-CssClass="t1_column_color2"                                    />                            
                            <asp:BoundField DataField="propCurrency" HeaderText="Currency" 
                                    HeaderStyle-HorizontalAlign="Center" HeaderStyle-BorderStyle="None" HeaderStyle-VerticalAlign="Middle" HeaderStyle-CssClass="t1_column_color1"
                                    ItemStyle-HorizontalAlign="Center" ItemStyle-BorderStyle="None" ItemStyle-CssClass="t1_column_color1"
                                    FooterStyle-BorderStyle="None" FooterStyle-CssClass="t1_column_color1" />
                            <asp:BoundField DataField="propPrice" HeaderText="Price" DataFormatString="{0:n2}"
                                    HeaderStyle-HorizontalAlign="Center" HeaderStyle-BorderStyle="None" HeaderStyle-VerticalAlign="Middle" HeaderStyle-CssClass="t1_column_color2"
                                    ItemStyle-HorizontalAlign="Right" ItemStyle-BorderStyle="None" ItemStyle-CssClass="t1_column_color2"
                                    FooterStyle-BorderStyle="None" FooterStyle-CssClass="t1_column_color2"/>
                        </Columns>
                        <EmptyDataTemplate>
                            No data found.
                        </EmptyDataTemplate>
                        <EmptyDataRowStyle BorderStyle="None" Font-Bold="true" />
                        <AlternatingRowStyle BackColor="#d1d1ef" />
                    </asp:GridView>
                    </div>
                </td>
            </tr>
            <tr>
                <td align="left">
                    <asp:Button runat="server" ID="btnCloseFundSearch" Text="  Close  " OnClick="btnCloseFundSearch_Click" />
                </td>
            </tr>
        </table>                        
    </div>
</asp:Panel>

<asp:ModalPopupExtender ID="mpeContactMe" runat="server" TargetControlID="btnContactMe" PopupControlID="PopupContactMe" CancelControlID="btnContactMeCancel" BackgroundCssClass="modalBackground" DropShadow="true" BehaviorID="bhvrPopupContactMe" />
<asp:Panel runat="server" ID="PopupContactMe" CssClass="modalBox" ScrollBars="Both" style="display:none;overflow:auto;" Height="500px" Width="600px" >    
    <br />
    <br />    
    <div style="text-align:left;width:100%;margin-top:1px;">
        <table width="100%">
            <tr>
                <td align="left">                    
                    &nbsp;&nbsp;&nbsp;<asp:Label runat="server" ID="lblContactMeForwords" Text="I would like to discuss your recommendation. Please contact me." />
                </td>
            </tr>        
            <tr>
                <td>
                    &nbsp;
                </td>
            </tr>            
            <tr>
                <td align="center">
                    <table width="70%">
                        <tr>
                            <td align="left">
                                <asp:Label runat="server" ID="lblContactMeByTel" Text="* By telephone on"/>&nbsp;<asp:TextBox runat="server" ID="txtContactMeByTel" onkeypress="javascript:return onlyNumbers(this,event);" />
                            </td>
                        </tr>
                        <tr>
                            <td align="left">
                                <asp:Label runat="server" ID="lblContactMeByEmail" Text="* By Email"/>
                            </td>
                        </tr>                        
                        <tr>
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label runat="server" ID="lblContactMeComments"/>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:TextBox runat="server" ID="txtContactMeComments" TextMode="MultiLine" Rows="17" Columns="60" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Button runat="server" ID="btnContactMeSend"  Text="   Send   " OnClick="btnContactMeSend_Click"/>&nbsp;<asp:Button runat="server" ID="btnContactMeCancel"  Text="   Cancel   "/>
                            </td>
                        </tr>
                    </table>                    
                </td>
            </tr>
        </table>
    </div>
</asp:Panel>

<asp:ModalPopupExtender ID="mpeDecline" runat="server" TargetControlID="btnDeclineSwitch" PopupControlID="PopupDecline" CancelControlID="btnCancelDecline" BackgroundCssClass="modalBackground" DropShadow="true" BehaviorID="bhvrPopupDecline" />
<asp:Panel runat="server" ID="PopupDecline" CssClass="modalBox" ScrollBars="Both" style="display:none;overflow:auto;" Height="450px" Width="600px" >    
    <br />
    <br />    
    <div style="text-align:left;width:100%;margin-top:1px;">
        <table width="100%">
            <tr>
                <td align="center">
                    <table width="70%">              
                        <tr>
                            <td align="left">                    
                                <asp:Label runat="server" ID="lblDeclineForwords" Text="Reason for declining this switch proposal: " />
                            </td>
                        </tr>        
                        <tr>
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:TextBox runat="server" ID="txtDeclineDescription" TextMode="MultiLine" Rows="17" Columns="60" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;
                            </td>
                        </tr>                        
                        <tr>
                            <td>
                                <asp:Button runat="server" ID="btnSendDecline"  Text="   Send   " OnClientClick="return confirm('You are about to decline this switch proposal, are you sure you want to continue?');"  OnClick="btnDeclineSwitch_Click" />&nbsp;<asp:Button runat="server" ID="btnCancelDecline"  Text="   Cancel   "/>
                            </td>
                        </tr>                        
                    </table>                    
                </td>
            </tr>
        </table>
    </div>
</asp:Panel>



<script type="text/javascript">

//    function removeFund(obj) {
//        confirmSave = confirm('Proceed removing this fund?');

//        if (confirmSave) {
//            alert(obj.toString().clie);
//                 
//            //__doPostBack(obj.toString(), 'SaveSwitchDetails');
//        }
//        else { return false; } 
//        
//    }

    function SwitchSave() {
        confirmSave = confirm('Proceed saving this switch?')

        if (confirmSave) {

            var CurrentAllocation = window.document.getElementById("ctl00_ContentPlaceHolder1_gvSwitchFooterHfTotalAllocation").value

            if (CurrentAllocation != 100.00) {
                alert("Unable to proceed. Total allocation must be set equal to 100. Your total allocation is: " + CurrentAllocation);
            }
            else {
                __doPostBack('btnSave', 'SaveSwitchDetails');
            }
        }
        else { return false; }
    }

    function popupWindow(url) {
        if (window.showModalDialog) {

            hfBrowser = document.getElementById("hfBrowser");
            //alert(hfBrowser.value);

            var FundIDOrig = document.getElementById("ctl00_ContentPlaceHolder1_hfFundIDOrig");
            var FundIDNew = document.getElementById("ctl00_ContentPlaceHolder1_hfFundIDNew");

            retVal = window.showModalDialog(url, 'Popup window', 'dialogHeight:700px;dialogWidth:1200px;center:yes;status:no');

            if (retVal != null) {
                if (retVal.process == 'changeFund') {
                    FundIDNew.value = retVal.selectedFundID;
                    FundIDOrig.value = retVal.oldFundID;
                    __doPostBack('hfSelectedFundID', 'ChangeFund');
                } else if (retVal.process == 'newFund') {
                    FundIDNew.value = retVal.newFundID;
                    __doPostBack('hfSelectedFundID', 'AddMoreFund');
                }
            }

        }
    }

    function computeRows(sender) {

        ctlID = sender.id.substring(sender.id.lastIndexOf('ctl'), sender.id.length).split('_');
        ctlID = ctlID[0]

        fundCount = document.getElementById("ctl00_ContentPlaceHolder1_hfFundCount").value;
        totalRowCount = parseInt(fundCount) + 2;
        if (parseInt(totalRowCount) < 10) {
            totalRowCount = '0' + totalRowCount.toString();
        }

        currentValueClient = document.getElementById("ctl00_ContentPlaceHolder1_hfCurrentValueClientTotal").value;
        price = document.getElementById("ctl00_ContentPlaceHolder1_gvAmendSwitch_" + ctlID + "_lblPrice").innerHTML;
        allocationDisplay = document.getElementById("ctl00_ContentPlaceHolder1_gvAmendSwitch_" + ctlID + "_gvSwitchHeaderValue");
        value_ = document.getElementById("ctl00_ContentPlaceHolder1_gvAmendSwitch_" + ctlID + "_gvSwitchDetailsLblValue");
        units_ = document.getElementById("ctl00_ContentPlaceHolder1_gvAmendSwitch_" + ctlID + "_gvSwitchDetailsLblUnits");
        currencyMultiplier = document.getElementById("ctl00_ContentPlaceHolder1_gvAmendSwitch_" + ctlID + "_hfCurrencyMultiplier").value;
        totalAlloc1 = document.getElementById("ctl00_ContentPlaceHolder1_gvAmendSwitch_ctl" + totalRowCount + "_gvSwitchFooterLblTotalAllocationOrig");
        totalAlloc2 = document.getElementById("ctl00_ContentPlaceHolder1_gvAmendSwitch_ctl" + totalRowCount + "_gvSwitchFooterLblTotalAllocation");

        allocation = sender.value;

        if (allocation == '') { allocation = 0 };
        allocationDisplay.innerHTML = allocation;

        try {
            units_.innerHTML = CommaFormatted(parseFloat(computeUnits(allocation, currentValueClient, price, currencyMultiplier)).toFixed(4), true);
            document.getElementById("ctl00_ContentPlaceHolder1_gvAmendSwitch_" + ctlID + "_gvhfSwitchDetailsLblUnits").value = CommaFormatted(parseFloat(computeUnits(allocation, currentValueClient, price, currencyMultiplier)).toFixed(4), true);
        }
        catch (err) {
            units_.innerHTML = 0.00;
            document.getElementById("ctl00_ContentPlaceHolder1_gvAmendSwitch_" + ctlID + "_gvhfSwitchDetailsLblUnits").value = 0.00;
        }

        try {
            value_.innerHTML = CommaFormatted(parseFloat(computeValue(allocation, currentValueClient)).toFixed(0));
            document.getElementById("ctl00_ContentPlaceHolder1_gvAmendSwitch_" + ctlID + "_gvhfSwitchHeaderValue").value = CommaFormatted(parseFloat(computeValue(allocation, currentValueClient)).toFixed(0));
            //value_.innerHTML =  computeValue(allocation, currentValueClient);
        }
        catch (err) {
            value_.innerHTML = 0.00;
            document.getElementById("ctl00_ContentPlaceHolder1_gvAmendSwitch_" + ctlID + "_gvhfSwitchHeaderValue").value = 0.00;
        }

        _total = computeTotalAllocation(fundCount);
        _total = parseFloat(_total).toFixed(2);

        btnSwitch = document.getElementById("ctl00_ContentPlaceHolder1_btnAmend");
        btnSave = document.getElementById("ctl00_ContentPlaceHolder1_btnSave");

        if (isNaN(_total)) {
            totalAlloc1.innerHTML = "-error-"
            totalAlloc2.innerHTML = "-error-"
            totalAlloc1.style.color = "#FF0000";
            totalAlloc2.style.color = "#FF0000";
            btnSave.disabled = true;
            btnSwitch.disabled = true;

        } else {
            totalAlloc1.innerHTML = _total;
            totalAlloc2.innerHTML = totalAlloc1.innerHTML;
            window.document.getElementById("ctl00_ContentPlaceHolder1_gvSwitchFooterHfTotalAllocation").value = totalAlloc2.innerHTML

            if (parseFloat(_total) != 100) {
                btnSave.disabled = true;
                btnSwitch.disabled = true;
                totalAlloc1.style.color = "#FF0000";
                totalAlloc2.style.color = "#FF0000";
            }
            else {
                btnSave.disabled = false;
                btnSwitch.disabled = false;
                totalAlloc1.style.color = "#000000";
                totalAlloc2.style.color = "#000000";
            }
        }
    }

    function computeValue(fPercentAllocation, fTotalValue) {
        if (fPercentAllocation != 0) {
            fPercentAllocation = parseFloat(replaceAll(fPercentAllocation, ',', ''));
        }

        fTotalValue = parseFloat(replaceAll(fTotalValue, ',', ''));

        value = ((fPercentAllocation / 100) * fTotalValue);

        return parseFloat(value);
    }

    function computeUnits(fPercentAllocation, fTotalValue, fPrice, fCurrencyMultiplier) {

        if (fPercentAllocation != 0) {
            fPercentAllocation = parseFloat(replaceAll(fPercentAllocation, ',', ''));
        }

        fTotalValue = parseFloat(replaceAll(fTotalValue, ',', ''));
        fPrice = parseFloat(replaceAll(fPrice, ',', ''));
        fCurrencyMultiplier = parseFloat(replaceAll(fCurrencyMultiplier, ',', ''));

        fPrice = fPrice * fCurrencyMultiplier;

        strPrice = String(fPrice).split('.');

        if (strPrice.length > 1) {
            strPriceDecimal = strPrice[1].substring(0, 3);
            strNewPrice = strPrice[0] + "." + strPrice[1].substring(0, 3);
            //fPrice = parseFloat(strNewPrice);
            fPrice = parseFloat(strNewPrice);
        }

        //units = (((fPercentAllocation / 100) * fTotalValue) / (fPrice * fCurrencyMultiplier).toFixed(4));
        units = (((fPercentAllocation / 100) * fTotalValue) / (fPrice).toFixed(4));




        //alert('((' + fPercentAllocation + ' / 100) * ' + fTotalValue + ') / (' + (fPrice * fCurrencyMultiplier).toFixed(4) + ') = ' + units);

        return units;  //parseFloat(units, 10).toFixed(4);
    }

    function computeTotalAllocation(FundCount) {
        var i = 1;
        var total = 0;
        var ctlID;
        while (i <= FundCount) {
            i++;
            if (i < 10) {
                ctlID = '0' + i;
            } else { ctlID = i }

            rowFundAllocation = document.getElementById("ctl00_ContentPlaceHolder1_gvAmendSwitch_ctl" + ctlID + "_gvSwitchTxtTargetAllocation").value;

            if (rowFundAllocation == '' || isNaN(rowFundAllocation)) {
                rowFundAllocation = parseFloat(0);
            } else {
                rowFundAllocation = rowFundAllocation;
            }

            total = parseFloat(total) + parseFloat(rowFundAllocation);

            if (parseFloat(total) > 0) {
                total = total;
            }
        }
        return total;
    }

    function onlyNumbers(sender, e) {
        if (window.event)
            charCode = window.event.keyCode; // IE
        else
            charCode = e.which; // Firefox

        if (charCode == 8 || charCode == 0) {
            return true;
        }

        if ((!(charCode > 47 && charCode < 58)) && charCode != 46) { return false; }

        if (charCode == 46) {
            if ((count(sender.value, '\\.') == 1) || sender.value == '') {
                return false;
            }
        }

        //	    if (parseFloat(sender.value) > parseFloat(100)) {sender.value = sender.value.slice(0, -1);return false;}

        //	    try {
        //	        decimalctr = sender.value.split('.');
        //	        if (decimalctr[1] != undefined) {
        //	            if (parseInt(decimalctr[1].length) > 2) {
        //	                sender.value = sender.value.slice(0, -1);
        //	                return false;
        //	            }
        //	        }
        //	    }catch (err) { }


        return true;
    }

    function count(s1, letter) {

        try {
            return String(s1).match(new RegExp(letter, 'g')).length;
        } catch (err) { }
    }

    function CommaFormatted(Num, showDecimal) {
        //var Num = document.form.input.value;
        var newNum = "";
        var newNum2 = "";
        var count = 0;

        //check for decimal number
        if (Num.indexOf('.') != -1) {  //number ends with a decimal point
            if (Num.indexOf('.') == Num.length - 1) {
                Num += "00";
            }
            if (Num.indexOf('.') == Num.length - 2) { //number ends with a single digit
                Num += "0";
            }

            var a = Num.split(".");
            Num = a[0];   //the part we will commify
            var end = a[1] //the decimal place we will ignore and add back later
        }
        else { var end = "00"; }

        //this loop actually adds the commas   
        for (var k = Num.length - 1; k >= 0; k--) {
            var oneChar = Num.charAt(k);
            if (count == 3) {
                newNum += ",";
                newNum += oneChar;
                count = 1;
                continue;
            }
            else {
                newNum += oneChar;
                count++;
            }
        }  //but now the string is reversed!

        //re-reverse the string
        for (var k = newNum.length - 1; k >= 0; k--) {
            var oneChar = newNum.charAt(k);
            newNum2 += oneChar;
        }

        if (showDecimal == true) {
            newNum2 = newNum2 + "." + end;
        }

        return newNum2;
    }

    function replaceAll(txt, replace, with_this) { return txt.replace(new RegExp(replace, 'g'), with_this); }

    //Popup FUNDS///////////////////////////////////
    function ValidateSearchInput() {
        if (event.keyCode == 13) {

            var SearchTextBox = window.document.getElementById("txtPopupFund").value

            if (SearchTextBox == "") {
                alert("Please input a search criteria");
            }
        }
    }

    function popUp_AddFunds() {
        document.getElementById("ctl00_ContentPlaceHolder1_hfPopUpFundAction").value = 'add';
        $find("bhvrPopupFundSearch", null).show();
    }

    function popUp_EditFunds(FundIDOrig) {

        document.getElementById("ctl00_ContentPlaceHolder1_hfFundIDOrig").value = FundIDOrig;
        document.getElementById("ctl00_ContentPlaceHolder1_hfPopUpFundAction").value = 'edit';

        $find("bhvrPopupFundSearch", null).show();
    }


    //Popup FUNDS///////////////////////////////////


    function validateSwitchAllocation() {
        var CurrentAllocation = window.document.getElementById("ctl00_ContentPlaceHolder1_gvSwitchFooterHfTotalAllocation").value
        if (CurrentAllocation != 100.00) {
            alert("Unable to proceed. Total allocation must be set equal to 100. Your total allocation is:" + CurrentAllocation);
            $find('bhvrFirstSwitchPopup', null).hide();
            return false;
        }
        else {
            $find('bhvrFirstSwitchPopup', null).show();
        }
    }
    
//    //$(document).ready(function() {

//        window.onbeforeunload = promtUserB4Exit;

//        function promtUserB4Exit() {
//            return "You have unsaved items, do you want to proceed?";
//        }
//   // });

</script>
</asp:Content>
