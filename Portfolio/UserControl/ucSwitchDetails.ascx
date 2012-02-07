<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucSwitchDetails.ascx.cs" Inherits="NAV.Portfolio.UserControl.WebUserControl1" %>
<hr />
<table  border="0" align="left" cellpadding="0" cellspacing="0" width="100%">
    <tr>
        <td  class="table1_header" align="left">           
            <b><asp:Label runat="server" ID="lblTitle_ProposedSwitch"/></b>
        </td>
    </tr>
    <tr>
        <td align="left">
            <asp:Label runat="server" ID="lblSwitchDetails_ActionDate" Text="Action Date:" Font-Bold="true" />&nbsp;<asp:Label runat="server" ID="lblDateAction" Font-Size="Smaller"/>
        </td>
    </tr>
    <tr>
        <td align="left" valign="top">
            <div><asp:Label runat="server" ID="lblIFASwitchDescTitle" Text="Note: " Font-Bold="true"/><asp:Label ID="txtProposedSwitchDesc" runat="server" style="width:300px;" Font-Size="Smaller" Font-Italic="true"/></div>
        </td>
    </tr>        
    <tr>
        <td>
            <div runat="server" id="table1_container">
            <asp:GridView runat="server" ID="gvSwitchDetails" AutoGenerateColumns="false" Width="100%" ShowFooter="true" BorderStyle="None" CellPadding="0" CellSpacing="0" UseAccessibleHeader="true" CssClass="table1" >
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
</table>
<br />&nbsp;