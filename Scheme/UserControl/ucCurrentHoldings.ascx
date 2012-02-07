<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucCurrentHoldings.ascx.cs" Inherits="NAV.Scheme.UserControl.ucCurrentHoldings" %>
<table  border="0" align="left" cellpadding="0" cellspacing="0" width="100%">
    <tr>
        <td  class="table1_header" align="left">           
            <b><asp:Label runat="server" ID="lblTitle_CurrentHoldings" Text="Current Holdings"/></b>
        </td>
    </tr>
    <tr>
        <td>
            <div id="table1_container">
            <asp:GridView runat="server" ID="gvCurrentHoldings" AutoGenerateColumns="false" Width="100%" ShowFooter="true" BorderStyle="None" CellPadding="0" CellSpacing="0" UseAccessibleHeader="true" CssClass="table1">
                <Columns>                   
                    <asp:TemplateField HeaderText="Fund Name" HeaderStyle-HorizontalAlign="Center" HeaderStyle-BorderStyle="None" FooterStyle-BorderStyle="None" HeaderStyle-VerticalAlign="Top">
                        <ItemStyle HorizontalAlign="Left"  BorderStyle="None" CssClass="t1_column_color2" />
                        <HeaderStyle CssClass="t1_column_color2" VerticalAlign="Middle" HorizontalAlign="Center" />
                        <FooterStyle BorderStyle="None" CssClass="t1_column_color2"/>
                        <ItemTemplate>
                            <asp:Label runat="server" ID="lblFundCurrencyPortfolio" Text='<%#DataBinder.Eval(Container.DataItem, "propFund.propFundName")%>'/>
                        </ItemTemplate>                        
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Price" HeaderStyle-HorizontalAlign="Right" HeaderStyle-BorderStyle="None" FooterStyle-BorderStyle="None" HeaderStyle-VerticalAlign="Top">
                        <ItemStyle HorizontalAlign="Right"  BorderStyle="None" CssClass="t1_column_color1" />
                        <HeaderStyle CssClass="t1_column_color1" VerticalAlign="Middle" HorizontalAlign="Center" />
                        <FooterStyle BorderStyle="None" CssClass="t1_column_color1"/>
                        <ItemTemplate>
                            <asp:Label runat="server" ID="lblFundCurrency" Text='<%#DataBinder.Eval(Container.DataItem, "propFund.propCurrency")%>'/>&nbsp;&nbsp;<asp:Label runat="server" ID="lblPrice" Text='<%#DataBinder.Eval(Container.DataItem, "propFund.propPrice", "{0:0.0000}")%>'/>
                            <br />
                            <asp:Label ID="lblDatePriceUpdated" runat="server"  htmlencode="false" text='<%#DataBinder.Eval(Container.DataItem, "propFund.propDatePriceUpdated", "({0:dd/MM/yyyy})")%>'  />                            
                        </ItemTemplate>                        
                    </asp:TemplateField>
                    <asp:BoundField DataField="propUnits" DataFormatString="{0:n4}" HtmlEncode="false"
                                    HeaderText="Units" HeaderStyle-HorizontalAlign="Center" HeaderStyle-BorderStyle="None" HeaderStyle-VerticalAlign="Middle" HeaderStyle-CssClass="t1_column_color2"
                                    ItemStyle-HorizontalAlign="Right" ItemStyle-BorderStyle="None" ItemStyle-CssClass="t1_column_color2"
                                    FooterStyle-BorderStyle="None" FooterStyle-CssClass="t1_column_color2"/>                    
                    <asp:TemplateField FooterStyle-BorderStyle="None">
                        <HeaderStyle BorderStyle="None" CssClass="t1_column_color1" VerticalAlign="Middle" HorizontalAlign="Center" />
                        <ItemStyle HorizontalAlign="Right"  BorderStyle="None" CssClass="t1_column_color1"/>
                        <FooterStyle HorizontalAlign="Right" BorderStyle="None" CssClass="t1_column_color1"/>
                        <HeaderTemplate>
                            <asp:Label runat="server" ID="gvHeaderValueFundCur" Text="Value Fund Cur" />
                        </HeaderTemplate>                        
                        <ItemTemplate>
                            <asp:Label runat="server" ID="gvDetailsValueFundCur" text='<%# Eval("propValue", "{0:n0}") %>'/>
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:Label runat="server" ID="gvFooterValueFundCur" Font-Bold="true"/>
                        </FooterTemplate>                        
                    </asp:TemplateField>
                    <asp:TemplateField FooterStyle-BorderStyle="None">
                        <HeaderStyle BorderStyle="None" CssClass="t1_column_color2" VerticalAlign="Middle" HorizontalAlign="Center"/>
                        <ItemStyle HorizontalAlign="Right" BorderStyle="None" CssClass="t1_column_color2" />
                        <FooterStyle HorizontalAlign="Right" BorderStyle="None" CssClass="t1_column_color2"/>
                        <HeaderTemplate>
                            <asp:Label runat="server" ID="gvHeaderValueClientCur" Text="Value Client Cur" />
                            <br />
                            <asp:Label runat="server" ID="gvHeaderClientCurrency"/>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label runat="server" ID="gvDetailsValueClientCur" Text='<%# Eval("propCurrentValueClient", "{0:n0}") %>'/>
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:Label runat="server" ID="gvFooterCurrentValueClient" Font-Bold="true"/>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField ItemStyle-HorizontalAlign="Right" FooterStyle-BorderStyle="None">
                        <HeaderStyle BorderStyle="None" CssClass="t1_column_color1" VerticalAlign="Middle" HorizontalAlign="Center" />
                        <ItemStyle HorizontalAlign="Right" BorderStyle="None" CssClass="t1_column_color1" />
                        <FooterStyle BorderStyle="None" CssClass="t1_column_color1"/>
                        <HeaderTemplate>
                            <asp:Label runat="server" ID="gvHeaderAllocation" Text="Allocation %" />
                        </HeaderTemplate>                                            
                        <ItemTemplate>
                            <asp:Label runat="server" ID="gvDetailsAllocation" Text='<% #Eval("propAllocation", "{0:n2}") %>'/>
                        </ItemTemplate>                        
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            </div>
        </td>
    </tr>
</table>