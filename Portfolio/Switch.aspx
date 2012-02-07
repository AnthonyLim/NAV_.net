<%@ Page Title="" Language="C#" MasterPageFile="~/NAV.Master" AutoEventWireup="true" CodeBehind="Switch.aspx.cs" Inherits="NAV.Portfolio.Switch" MaintainScrollPositionOnPostback="true" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"></ajaxToolkit:ToolkitScriptManager>
  <script type="text/javascript" src="https://www.google.com/jsapi?key=ABQIAAAAJIc7LVNGdtica6PhZe_sFRQqgDN7nsaPpAk-csjZoyzusnQgwBSfA71pi0A-I91r0MmWO8okG_Zq6Q"></script>
    <script type="text/javascript">
        google.load("jquery", "1.7.1");
        // google.load("jqueryui", "1.8.1");
    </script>
    <script src="../js/switchJS.js"></script>
    
    <script type="text/javascript">
        $(function() {
            $('#<%= txtPopupFund.ClientID %>').removeAttr('onchange').removeAttr('onkeypress');
            $('#<%= gvFunds.ClientID %> a').attr('onclick', "getFlickerSolved('<%=PopupFundSearch.ClientID%>');");
        });
        function hidePopup(oModalPopup) {
            var modalPopup = $find(oModalPopup);
            var hasDiscretionary = '<%=Session["HasDiscretionary"]%>'
            if ((hasDiscretionary == 1) || (hasDiscretionary == -1)) {
                $find('bhvrThirdSwitchPopup', null).show();
                modalPopup.hide();
            }
            else {
                $find('bhvrSecondSwitchPopup', null).show();
                modalPopup.hide();
            }
        }

        function validateTextbox() {
            if (document.getElementById('ctl00_ContentPlaceHolder1_txtSMSMobileNo').value == '') {
                alert('Please enter mobile number!');
                document.getElementById('ctl00_ContentPlaceHolder1_txtSMSMobileNo').focus()
                return false;
            }
        }
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
        function confirmApprove() {
            var result = confirm("Do you want to approve this portfolio?");
            if (result) {
                $find('bhvrThirdSwitchPopup', null).show();
            }
            else {
                $find('bhvrThirdSwitchPopup', null).hide();
            }
        }
    </script>  
    
<table border="0" align="left" cellpadding="0" cellspacing="0" width="100%" class="sms_template">
    <tr>
        <td align="left">
            <asp:LinkButton runat="server" ID="lbtnSMSTemplateList" Text="Admin - SMS Templates"  Font-Size="X-Small" OnClick="lbtnSMSTemplateList_Click"></asp:LinkButton>
        </td>
        <td>
            <asp:LinkButton runat="server" ID="lbtnEmailTemplateList" Text="Admin - Email Templates"  Font-Size="X-Small" OnClick="lbtnEmailTemplateList_Click"></asp:LinkButton>
        </td>
        <td>
            <asp:LinkButton runat="server" ID="lbtnHistory" Text="History"  Font-Size="X-Small" OnClick="lbtnHistory_Click"></asp:LinkButton>
        </td>                 
        <td align="left">
            <asp:LinkButton runat="server" ID="lbtnSignedConfirmation" Text="Email Signed Confirmation"  Font-Size="X-Small" OnClick="lbtnSignedConfirmation1"></asp:LinkButton>
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
<table width="100%">
    <tr>
        <td>
            <asp:Button runat="server" ID="btnComplete" Text="Complete Switch" OnClientClick="return confirm('Proceed changing the status of this Switch to Complete?')" OnClick="btnComplete_Click" Visible="false"  />
        </td>
    </tr>
</table>
<table  border="0" align="left" cellpadding="0" cellspacing="0" width="100%">
    <tr>
        <td  class="table1_header" align="left">           
            <b><asp:Label runat="server" ID="lblTitle_Holdings" Text="Holdings"/></b>
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
                            <asp:Label ID="lblDataDatePortfolio" runat="server" htmlencode="false" text='<%# CheckNull(Eval("propDataDate", "{0:dd/MM/yyyy}"))%>'  />
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
                            Total:&nbsp;<asp:Label runat="server" ID="gvFooterCurrentValueClient"/>
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
    <tr>
        <td  class="table1_header" align="left">           
            <b><asp:Label runat="server" ID="lblTitle_ProposedSwitch" Text="Client Switch" Visible="false"/></b>
        </td>
    </tr>
<!--Start Client GridView-->    
    <tr>
        <td align="left">
            <asp:Label runat="server" ID="gvSwitchDetailsClientLabelStatusTitle" Text="Status: " Visible="false" />&nbsp;<asp:Label runat="server" ID="gvSwitchDetailsViewLabelStatusValue" Visible="false" />
        </td>
    </tr>
    <tr>
        <td>
            <div id="table1_container">
                <asp:GridView runat="server" ID="gvSwitchDetailsClient" Visible="false" AutoGenerateColumns="false" Width="100%" ShowFooter="true" BorderStyle="None" CellPadding="0" CellSpacing="0" UseAccessibleHeader="true" CssClass="table1" >
                    <Columns>
                        <asp:TemplateField HeaderText="Fund Name" HeaderStyle-HorizontalAlign="Center" HeaderStyle-BorderStyle="None" FooterStyle-BorderStyle="None" HeaderStyle-VerticalAlign="Top">
                            <HeaderStyle CssClass="t1_column_color1" VerticalAlign="Middle" />
                            <ItemStyle HorizontalAlign="Left" BorderStyle="None" CssClass="t1_column_color1" />                        
                            <FooterStyle BorderStyle="None" CssClass="t1_column_color1"/>
                            <ItemTemplate>
                                <asp:Label runat="server" ID="gvSwitchDetailsClientItemLabelFundName" Text='<%#DataBinder.Eval(Container.DataItem, "propFund.propFundName")%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Unit" HeaderStyle-HorizontalAlign="Right" HeaderStyle-BorderStyle="None" FooterStyle-BorderStyle="None" HeaderStyle-VerticalAlign="Top">
                            <HeaderStyle CssClass="t1_column_color2" VerticalAlign="Middle" HorizontalAlign="Center" />
                            <ItemStyle HorizontalAlign="Right"  BorderStyle="None" CssClass="t1_column_color2" />
                            <FooterStyle BorderStyle="None" CssClass="t1_column_color2"/>
                            <ItemTemplate>
                                <asp:Label runat="server" ID="gvSwitchDetailsClientItemLabelUnit" Text='<%#Eval("propUnits", "{0:#,##0.0000}")%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                       <%-- <asp:TemplateField>
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lblsample" Text='<% %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>--%>
                        <asp:TemplateField HeaderText="Price" HeaderStyle-HorizontalAlign="Right" HeaderStyle-BorderStyle="None" FooterStyle-BorderStyle="None" HeaderStyle-VerticalAlign="Top">
                            <HeaderStyle HorizontalAlign="Center"  VerticalAlign="Middle" CssClass="t1_column_color1"/>
                            <ItemStyle HorizontalAlign="Right"  BorderStyle="None" CssClass="t1_column_color1"/>
                            <FooterStyle BorderStyle="None" CssClass="t1_column_color1"/>
                            <ItemTemplate>
                                <asp:Label runat="server" ID="gvSwitchDetailsClientItemLabelCurrency" Text='<%#Eval("propFund.propCurrency")%>'></asp:Label>
                                &nbsp;
                                <asp:Label runat="server" ID="gvSwitchDetailsClientItemLabelPrice" Text='<%#Eval("propFund.propPrice", "{0:0.0000}")%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Value" HeaderStyle-HorizontalAlign="Right" HeaderStyle-BorderStyle="None" FooterStyle-BorderStyle="None" HeaderStyle-VerticalAlign="Top">
                            <HeaderStyle BorderStyle="None" CssClass="t1_column_color2" VerticalAlign="Middle" HorizontalAlign="Center" />
                            <ItemStyle HorizontalAlign="Right" BorderStyle="None" CssClass="t1_column_color2" />
                            <FooterStyle HorizontalAlign="Right" BorderStyle="None" CssClass="t1_column_color2"/>
                            <ItemTemplate>
                                <asp:Label runat="server" ID="gvSwitchDetailsClientItemLabelValue" Text='<% #Eval("propValue", "{0:n0}") %>'/>
                            </ItemTemplate>
                            <FooterTemplate>
                                Total:&nbsp;<asp:Label runat="server" ID="gvSwitchDetailsClientFooterLabelTotalValue" />
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Allocation %" HeaderStyle-HorizontalAlign="Right" HeaderStyle-BorderStyle="None" FooterStyle-BorderStyle="None" HeaderStyle-VerticalAlign="Top">
                            <HeaderStyle HorizontalAlign="Center"  VerticalAlign="Top" CssClass="t1_column_color1" />
                            <ItemStyle HorizontalAlign="Right"  BorderStyle="None" CssClass="t1_column_color1" />
                            <FooterStyle HorizontalAlign="Right" BorderStyle="None" CssClass="t1_column_color1" />
                            <ItemTemplate>
                                <asp:Label runat="server" ID="gvSwitchDetailsClientItemLabelTargetAllocation" Text='<%#Eval("propAllocation")%>'></asp:Label><!--, "{0:0.00}"-->
                            </ItemTemplate>
                            <FooterTemplate>
                                <asp:Label runat="server" ID="gvSwitchDetailsClientFooterLabelTotalTargetAllocation" />
                            </FooterTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
        </td>
    </tr>
    <tr>
        <td>
<%--            <asp:TextBox runat="server" ID="txtSwitchClientDescription" Visible="false" TextMode="MultiLine" Rows="10" Width="100%" Enabled="false" BackColor="White" CssClass="multilineTextbox"></asp:TextBox>--%>
            <asp:Label runat="server" ID="lblSwitchClientDescription" Visible="false" Width="100%" BackColor="White"></asp:Label>
        </td>
    </tr>
<!--End Client GridView-->    
    <tr><td>&nbsp;</td></tr>
    <tr>
        <td align="left">
            <asp:Label runat="server" ID="lblSwitchStatusTitle" Text="Status: "/>&nbsp;<asp:Label runat="server" ID="lblSwitchStatusValue"/>
        </td>
    </tr>             
    <tr>
        <td>
            <div class="table2">
            <asp:GridView runat="server" ID="gvSwitchDetails" AutoGenerateColumns="false" Width="100%" ShowFooter="true" BorderStyle="None" CellPadding="0" CellSpacing="0" UseAccessibleHeader="true" CssClass="table2" >
                <Columns>
                    <asp:TemplateField>
                        <HeaderStyle CssClass="t2_header" />
                        <ItemStyle HorizontalAlign="Center" BorderStyle="None"/>
                        <FooterStyle HorizontalAlign="Center"  CssClass="t2_results"/>
                        <ItemTemplate>
                            <asp:ImageButton runat="server" id="ibtnEditFund" ImageUrl="~/Images/remove_btn.png" OnClientClick="return confirm('Proceed removing this fund?')" OnCommand="lbtnRemoveFund_Click" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "propFund.propFundID")%>' Visible='<%#Eval("propIsDeletable")%>' />
                        </ItemTemplate>
                        <FooterTemplate>                                                        
                            <img runat="server" id="imgAddFund" src="../Images/add_btn.png" onclick='javascript:popUp_AddFunds();' onmouseover="javascript:this.style.cursor='pointer'"/>
                        </FooterTemplate>                        
                    </asp:TemplateField>
                    <asp:TemplateField ItemStyle-HorizontalAlign="Right" >
                        <HeaderStyle HorizontalAlign="Left"  VerticalAlign="Top" CssClass="t2_header"/>
                        <ItemStyle HorizontalAlign="Left"  BorderStyle="None"/>
                        <FooterStyle HorizontalAlign="Left" CssClass="t2_results"/>
                        <FooterTemplate>
                            <a runat="server" id="lbtnAddFund" href='javascript:popUp_AddFunds();'>Add more funds</a>
                        </FooterTemplate>
                        <HeaderTemplate>
                            <asp:Label runat="server" ID="gvSwitchHeaderFundName" Text="Fund Name"  />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <div class="added_fund"  style='<%# HideClickableFundLinks(Eval("propIsDeletable"))%>'>                                                            
                                <a runat="server" href='<%#String.Format("javascript:popUp_EditFunds({0})",Eval("propFund.propFundID"))%>' id="lbtnFundName"><%# DataBinder.Eval(Container.DataItem, "propFund.propFundName") %></a>
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
                            <asp:HiddenField runat="server" ID="hfUnits" Value='<% #Eval("propUnits", "{0:n4}") %>' />
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
                            <asp:HiddenField runat="server" ID="hfValue" Value='<% #Eval("propValue", "{0:n0}") %>' />
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
                        <HeaderStyle HorizontalAlign="Center"  VerticalAlign="Top" CssClass="t2_header" />
                        <ItemStyle HorizontalAlign="Right"  BorderStyle="None" />
                        <FooterStyle HorizontalAlign="Center" CssClass="t2_results" />
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
<div runat="server" id="divSwitch">
    <center>
        <button id="btnSave" onclick="javascript:SwitchSave();" type="button" causesvalidation="true" runat="server">Save</button>&nbsp;        
        <input type="button" id="btnSwitch" runat="server" onclick="return validateSwitchAllocation();" value="Switch"/>
        <asp:Button runat="server" ID="btnCancel"  Text="Cancel" UseSubmitBehavior="false" CausesValidation="false" OnClientClick='if (!confirm("Cancel this switch?")) { return; }' OnClick="btnCancel_Click" />
        
    </center>
</div>
<div runat="server" id="divAmend" visible="false">
    <center>
        <asp:Button runat="server" ID="btnApprove" Text="Approve" OnClientClick="return confirm('Do you want to approve this portfolio?');" OnClick="btnApprove_Click" />
        &nbsp;
        <asp:Button runat="server" ID="btnReject" Text="Reject" OnClick="btnReject_Click" />
    </center>
</div>
<div runat="server" id="divRequestforDiscussion" visible="false">
    <center>
        <asp:Button runat="server" ID="btnAmend" Text="Amend" OnClick="btnAmend_Click" />
        &nbsp;
        <asp:Button runat="server" ID="btnResubmit" Text="Resubmit" OnClick="btnResubmit_Click" />
        &nbsp;
        <asp:Button runat="server" ID="btnCancel2" Text="Cancel" UseSubmitBehavior="false" CausesValidation="false" OnClientClick='if (!confirm("Cancel this switch?")) { return; }' OnClick="btnCancel_Click" />
    </center>
</div>
<div>
    <ajaxToolkit:ModalPopupExtender ID="mdlFirstSwitchPopup" runat="server" TargetControlID="Button2" PopupControlID="FirstSwitchPopup" CancelControlID="btnSwitchCancel" BackgroundCssClass="modalBackground" DropShadow="true" BehaviorID="bhvrFirstSwitchPopup" />
    
    <asp:Panel ID="FirstSwitchPopup" runat="server" style="display:none;" CssClass="modalBox"> 
        <div style="text-align:right;width:100%;margin-top:5px;">
            <asp:Label runat="server" ID="lblSwitchMessage" Text="You are about to perform this Switch. Are you sure you want to proceed?"></asp:Label><br /><br />
            <center>
            <input type="button" id="btnSwitchProceed" runat="server" onclick="hidePopup('bhvrFirstSwitchPopup');" value="Proceed"/>&nbsp;&nbsp;<asp:Button runat="server" ID="btnSwitchCancel" Text="Cancel" />
            </center>
            <ajaxToolkit:ModalPopupExtender ID="mdlSecondSwitchPopup" runat="server" TargetControlID="Button2" PopupControlID="SecondSwitchPopup" CancelControlID="" BackgroundCssClass="modalBackground" DropShadow="true" BehaviorID="bhvrSecondSwitchPopup" />
        </div>
    </asp:Panel>
    
    <asp:Panel ID="SecondSwitchPopup" runat="server" style="display:none;" CssClass="modalBox">
        <div style="text-align:right;width:100%;margin-top:5px;">
            <asp:Label runat="server" ID="lblMessage2" Text="Client Approval required. Request client approval for this switch"></asp:Label><br /><br />
            <center>
            <asp:Button ID="btnSwitchYes" Text="Yes" runat="server" OnClientClick="hidePopup('bhvrSecondSwitchPopup')" />
            <asp:Button ID="btnSwitchNo" runat="server" Text="No" OnClick="btnSwitchNo_Click" />
            </center>
            <ajaxToolkit:ModalPopupExtender ID="mdlThirdSwitchPopup" runat="server" TargetControlID="btnSwitchYes" CancelControlID="btnSwitchSendCancel" PopupControlID="ThirdSwitchPopup" BackgroundCssClass="modalBackground" DropShadow="true" BehaviorID="bhvrThirdSwitchPopup" />
        </div>
    </asp:Panel>
    
    <asp:Panel runat="server" ID="ThirdSwitchPopup" style="display:none;" CssClass="modalBox" Width="350px">
        <div style="text-align:left;width:100%;margin-top:1px;">
        <h4>Portfolio Description</h4>
            <table width="100%">
                <tr><td colspan="2"><asp:TextBox ID="txtDescription" runat="server" TextMode="MultiLine" CssClass="multilineTextbox" Rows="10" width="95%"></asp:TextBox></td></tr>
                <tr><td>&nbsp;</td></tr>
                <tr><td><asp:Label ID="lblSMSMobileNo" runat="server" Text="Mobile no.:" Width="35%"></asp:Label><asp:TextBox runat="server" ID="txtSMSMobileNo" width="60%" ></asp:TextBox></td></tr>
            </table>
        <center>
            <br />
            <asp:Button runat="server" ID="btnSwitchSendSave" Text="Save" OnClick="btnSwitchSendSave_Click" />&nbsp;&nbsp;
            <asp:Button runat="server" ID="btnSwitchSendCancel" Text="Cancel" />
            <br />
        </center>
        </div>
    </asp:Panel>
</div>
<ajaxToolkit:ModalPopupExtender ID="mpeFundSearch" runat="server" TargetControlID="Button2" PopupControlID="PopupFundSearch" BackgroundCssClass="modalBackground" DropShadow="true" BehaviorID="bhvrPopupFundSearch" />
<asp:Panel runat="server" ID="PopupFundSearch" CssClass="modalBox" ScrollBars="Both" style="display:none;overflow:auto;" Height="700px" Width="800px" DefaultButton="btnPopupFund">    
    <div style="text-align:left;width:100%;margin-top:1px;">               
        <table width="100%">
            <tr>
                <td>
                    &nbsp;
                </td>
            </tr>                                    
            <tr>
                <td>
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
                    <asp:GridView runat="server" ID="gvFunds" AutoGenerateColumns="false" Width="100%" BorderStyle="Solid" BorderWidth="1">
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

<!-- Keep this control at the last to hide it, this will make validation of popup   -->
    <asp:Button ID="Button2" runat="server"  Text="" style="display:none;"/>
<script type="text/javascript">
    
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
    function init() {
        //ctl00_ContentPlaceHolder1_gvSwitchDetails_ctl03_gvSwitchHeaderValue
        //        for (i = 2; i < 4; i++) {
        alert("s");
        computeRows("ctl00_ContentPlaceHolder1_gvSwitchDetails_ctl03_gvSwitchTxtTargetAllocation");
//        }
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
        price = document.getElementById("ctl00_ContentPlaceHolder1_gvSwitchDetails_" + ctlID + "_lblPrice").innerHTML;
        allocationDisplay = document.getElementById("ctl00_ContentPlaceHolder1_gvSwitchDetails_" + ctlID + "_gvSwitchHeaderValue");
        value_ = document.getElementById("ctl00_ContentPlaceHolder1_gvSwitchDetails_" + ctlID + "_gvSwitchDetailsLblValue");
        hfvalue_ = document.getElementById("ctl00_ContentPlaceHolder1_gvSwitchDetails_" + ctlID + "_hfValue");
        units_ = document.getElementById("ctl00_ContentPlaceHolder1_gvSwitchDetails_" + ctlID + "_gvSwitchDetailsLblUnits");
        hfunits_ = document.getElementById("ctl00_ContentPlaceHolder1_gvSwitchDetails_" + ctlID + "_hfUnits");
        currencyMultiplier = document.getElementById("ctl00_ContentPlaceHolder1_gvSwitchDetails_" + ctlID + "_hfCurrencyMultiplier").value;
        totalAlloc1 = document.getElementById("ctl00_ContentPlaceHolder1_gvSwitchDetails_ctl" + totalRowCount + "_gvSwitchFooterLblTotalAllocationOrig");
        totalAlloc2 = document.getElementById("ctl00_ContentPlaceHolder1_gvSwitchDetails_ctl" + totalRowCount + "_gvSwitchFooterLblTotalAllocation");

        allocation = sender.value;

        if (allocation == '') { allocation = 0 };
        allocationDisplay.innerHTML = allocation;
        try {
            units_.innerHTML = CommaFormatted(parseFloat(computeUnits(allocation, currentValueClient, price, currencyMultiplier)).toFixed(4), true);
            hfunits_.value = CommaFormatted(parseFloat(computeUnits(allocation, currentValueClient, price, currencyMultiplier)).toFixed(4), true);
        }
        catch (err) {
            units_.innerHTML = 0.00;
            hfunits_.value = 0.00;
        }
        try {
            value_.innerHTML = CommaFormatted(parseFloat(computeValue(allocation, currentValueClient)).toFixed(0));
            hfvalue_.value = CommaFormatted(parseFloat(computeValue(allocation, currentValueClient)).toFixed(0));
        }
        catch (err) {
            value_.innerHTML = 0.00;
            hfvalue_.value = 0.00;
        }
        _total = computeTotalAllocation(fundCount);
        _total = parseFloat(_total).toFixed(2);

        btnSwitch = document.getElementById("ctl00_ContentPlaceHolder1_btnSwitch");
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

            rowFundAllocation = document.getElementById("ctl00_ContentPlaceHolder1_gvSwitchDetails_ctl" + ctlID + "_gvSwitchTxtTargetAllocation").value;

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
        if (e.keyCode) {
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

</script>
 </asp:Content>