<%@ Page Title="" Language="C#" MasterPageFile="~/NAV.Master" AutoEventWireup="true" CodeBehind="SwitchBulkClientList.aspx.cs" Inherits="NAV.Portfolio.SwitchBulkClientList" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"></ajaxToolkit:ToolkitScriptManager>

<table border="0" align="left" cellpadding="0" cellspacing="0" width="100%">
<br />
<br />
<tr>&nbsp;</tr>
<tr>&nbsp;</tr>
<tr><td>
    <table  border="0" align="left" cellpadding="0" cellspacing="0" width="100%">
        <thead align="left">
            <tr>
                <td colspan="2">
                    <h2><asp:Label runat="server" ID="lblTitle_BulkSwitchList" Text="List of Clients in this model"/></h2>
                </td>
            </tr>        
        </thead>
    </table>
    </td></tr>
    <tr>
        <td>
            <div id="Div2">
            <asp:GridView ID="gvBulkSwitchListDiscretionaryYes" runat="server" AutoGenerateColumns="false" DataKeyNames="propClientID, propPortfolioID" Width="100%" ShowFooter="true" BorderStyle="None" CellPadding="0" CellSpacing="0" UseAccessibleHeader="true" CssClass="table1" EmptyDataText="No record found">
                <Columns>
                    <asp:TemplateField HeaderText="Switch ID" HeaderStyle-HorizontalAlign="Right" HeaderStyle-BorderStyle="None" FooterStyle-BorderStyle="None" HeaderStyle-VerticalAlign="Top">
                        <HeaderStyle CssClass="t1_column_color1" HorizontalAlign="Center" VerticalAlign="Middle" />
                        <ItemStyle CssClass="t1_column_color1" HorizontalAlign="Center" BorderStyle="None" />                        
                        <FooterStyle CssClass="t1_column_color1" BorderStyle="None" />
                        <ItemTemplate>
                            <asp:Label ID="lblSwitchID" runat="server" Text='<%#Eval("propSwitch.propSwitchID")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Client Name" HeaderStyle-HorizontalAlign="Right" HeaderStyle-BorderStyle="None" FooterStyle-BorderStyle="None" HeaderStyle-VerticalAlign="Top">
                        <HeaderStyle CssClass="t1_column_color2" HorizontalAlign="Center" VerticalAlign="Middle" />
                        <ItemStyle CssClass="t1_column_color2" HorizontalAlign="Left"  BorderStyle="None" />
                        <FooterStyle CssClass="t1_column_color2" BorderStyle="None" />
                        <ItemTemplate>
                            <asp:Label ID="gvBulkSwitchListLabelForename" runat="server" Text='<%#Eval("propClient.propForename")%>' ></asp:Label>&nbsp;
                            <asp:Label ID="gvBulkSwitchListLabelSurname" runat="server" Text='<%#Eval("propClient.propSurname")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Portfolio Name" HeaderStyle-HorizontalAlign="Right" HeaderStyle-BorderStyle="None" FooterStyle-BorderStyle="None" HeaderStyle-VerticalAlign="Top">
                        <HeaderStyle CssClass="t1_column_color1" HorizontalAlign="Center" VerticalAlign="Middle" />
                        <ItemStyle CssClass="t1_column_color1"  HorizontalAlign="Left" BorderStyle="None" />                        
                        <FooterStyle CssClass="t1_column_color1" BorderStyle="None" />
                        <ItemTemplate>
                                <asp:Label ID="gvBulkSwitchListLabelPortfolioName" runat="server" Text='<%#Eval("propCompany")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--<asp:TemplateField HeaderText="Portfolio ID" HeaderStyle-HorizontalAlign="Right" HeaderStyle-BorderStyle="None" FooterStyle-BorderStyle="None" HeaderStyle-VerticalAlign="Top">
                        <HeaderStyle CssClass="t1_column_color2" HorizontalAlign="Center" VerticalAlign="Middle" />
                        <ItemStyle CssClass="t1_column_color2" HorizontalAlign="Right"  BorderStyle="None" />
                        <FooterStyle CssClass="t1_column_color2" BorderStyle="None" />
                        <ItemTemplate>
                            <asp:Label runat="server" ID="gvBulkSwitchListLabelPortfolioID" Text='<%#Eval("propPortfolioID")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>--%>
                    <%--<asp:TemplateField HeaderText="Switch Status" HeaderStyle-HorizontalAlign="Right" HeaderStyle-BorderStyle="None" FooterStyle-BorderStyle="None" HeaderStyle-VerticalAlign="Top">
                        <HeaderStyle CssClass="t1_column_color1" VerticalAlign="Middle" HorizontalAlign="Center"/>
                        <ItemStyle HorizontalAlign="Center" BorderStyle="None" CssClass="t1_column_color1" />                        
                        <FooterStyle BorderStyle="None" CssClass="t1_column_color1"/>
                        <ItemTemplate>
                            <asp:Label runat="server" ID="gvSwitchListLabelStatus" Text='<%#Eval("propStatusString")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderStyle-HorizontalAlign="Right" HeaderStyle-BorderStyle="None" FooterStyle-BorderStyle="None" HeaderStyle-VerticalAlign="Top">
                        <HeaderStyle CssClass="t1_column_color2" VerticalAlign="Middle" HorizontalAlign="Center"/>
                        <ItemStyle HorizontalAlign="Center" BorderStyle="None" CssClass="t1_column_color2" />  
                        <FooterStyle BorderStyle="None" CssClass="t1_column_color2"/>
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkbtnGVHistory" runat="server" Text="History" PostBackUrl='<%# String.Format("~/Portfolio/SwitchHistory.aspx?SID={0}&CID={1}&PID={2}", Eval("propSwitchID"), Eval("propClientID"), Eval("propPortfolioID"))%>' ></asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>--%>
                    <asp:TemplateField HeaderText="Discretionary" HeaderStyle-HorizontalAlign="Right" HeaderStyle-BorderStyle="None" FooterStyle-BorderStyle="None" HeaderStyle-VerticalAlign="Top">
                        <HeaderStyle CssClass="t1_column_color2" VerticalAlign="Middle" HorizontalAlign="Center" />
                        <ItemStyle CssClass="t1_column_color2" HorizontalAlign="Right"  BorderStyle="None" />
                        <FooterStyle CssClass="t1_column_color2" BorderStyle="None" />
                        <ItemTemplate><asp:Label ID="lblDiscretionary" runat="server" Text='<%#int.Parse((Eval("propMFPercent")).ToString()) > 0 ? "Yes": "No" %>' /></ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Customized" HeaderStyle-HorizontalAlign="Right" HeaderStyle-BorderStyle="None" FooterStyle-BorderStyle="None" HeaderStyle-VerticalAlign="Top">
                        <HeaderStyle CssClass="t1_column_color1" HorizontalAlign="Center" VerticalAlign="Middle" />
                        <ItemStyle CssClass="t1_column_color1" HorizontalAlign="Center" BorderStyle="None" />                        
                        <FooterStyle CssClass="t1_column_color1" BorderStyle="None" />
                        <ItemTemplate>
                            <asp:Label runat="server" ID="lblCustomized" Text='<%#((int)Eval("propSwitchTemp.propModelID")) > 0 ? "Yes": "No" %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderStyle-HorizontalAlign="Right" HeaderStyle-BorderStyle="None" FooterStyle-BorderStyle="None" HeaderStyle-VerticalAlign="Top">
                        <HeaderStyle CssClass="t1_column_color2" VerticalAlign="Middle" HorizontalAlign="Center"/>
                        <ItemStyle CssClass="t1_column_color2" HorizontalAlign="Center" BorderStyle="None" />  
                        <FooterStyle CssClass="t1_column_color2" BorderStyle="None" />
                        <ItemTemplate>
                            <asp:LinkButton ID="lbtnModify" runat="server" Text="Modify" PostBackUrl='<%# String.Format("~/Portfolio/SwitchBulkCustomized.aspx?MID={0}&MGID={1}&MPID={2}&CID={3}&PID={4}", Eval("propModelID"), Eval("propModelGroupID"), Eval("propModelPortfolioID"), Eval("propClientID"), Eval("propPortfolioID"))%>' ></asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Selected" HeaderStyle-HorizontalAlign="Right" HeaderStyle-BorderStyle="None" FooterStyle-BorderStyle="None" HeaderStyle-VerticalAlign="Top">
                        <HeaderStyle CssClass="t1_column_color1" VerticalAlign="Middle" HorizontalAlign="Center" />
                        <ItemStyle CssClass="t1_column_color1" HorizontalAlign="Right"  BorderStyle="None" />
                        <FooterStyle CssClass="t1_column_color1" BorderStyle="None" />
                        <ItemTemplate><asp:CheckBox ID="cboxSelected" runat="server" /></ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="propClientID" InsertVisible="true" Visible="false"/>
                    <asp:BoundField DataField="propPortfolioID" InsertVisible="true" Visible="false" />
                </Columns>
            </asp:GridView>
            </div>
        </td>
    </tr>
    <tr><td>&nbsp;</td></tr>
    <tr><td><div><center>
                        <asp:Button ID="btnPreviousPage" runat="server" Text="Back" OnClick="btnPreviousPage_Click" />
                        &nbsp;
                        <asp:Button ID="btnProceedDiscretionaryYes" runat="server" Text="Bulk Switch" OnClick="btnProceedBulkSwitch_Click" OnClientClick="return check_sel('ctl00_ContentPlaceHolder1_gvBulkSwitchListDiscretionaryYes')" />
                        &nbsp;
                        <%--<asp:Button ID="btnBulkSwitchCancel" runat="server" Text="Cancel" />--%>
    </center></div></td></tr>
<tr><td>&nbsp;</td></tr>
<asp:Panel ID="panel1" runat="server" Visible="false">
<tbody class='member_info'>
        <tr> 
        <td style="width:21%" align="left">
            <asp:Label runat="server" ID="lblTitle_DiscretionaryNo" Text="Without Discretionary" />
        </td>
    </tr>  
    </tbody>
    <tr>
        <td>
            <div id="Div1">
            <asp:GridView ID="gvBulkSwitchListDiscretionaryNo" runat="server" AutoGenerateColumns="false" DataKeyNames="propClientID, propPortfolioID" Width="100%" ShowFooter="true" BorderStyle="None" CellPadding="0" CellSpacing="0" UseAccessibleHeader="true" CssClass="table1" EmptyDataText="No record found">
                <Columns>
                    <asp:TemplateField HeaderText="Client ID" HeaderStyle-HorizontalAlign="Right" HeaderStyle-BorderStyle="None" FooterStyle-BorderStyle="None" HeaderStyle-VerticalAlign="Top">
                        <HeaderStyle CssClass="t1_column_color1" VerticalAlign="Middle" HorizontalAlign="Center"/>
                        <ItemStyle HorizontalAlign="Center" BorderStyle="None" CssClass="t1_column_color1" />                        
                        <FooterStyle BorderStyle="None" CssClass="t1_column_color1"/>
                        <ItemTemplate>
                            <asp:Label ID="gvBulkSwitchListLabelClientID" runat="server" Text='<%#Eval("propClientID") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Client Name" HeaderStyle-HorizontalAlign="Right" HeaderStyle-BorderStyle="None" FooterStyle-BorderStyle="None" HeaderStyle-VerticalAlign="Top">
                        <HeaderStyle CssClass="t1_column_color2" VerticalAlign="Middle" HorizontalAlign="Center" />
                        <ItemStyle HorizontalAlign="Left"  BorderStyle="None" CssClass="t1_column_color2" />
                        <FooterStyle BorderStyle="None" CssClass="t1_column_color2"/>
                        <ItemTemplate>
                            <asp:Label ID="gvBulkSwitchListLabelForename" runat="server" Text='<%#Eval("propClient.propForename")%>' ></asp:Label>&nbsp;
                            <asp:Label ID="gvBulkSwitchListLabelSurname" runat="server" Text='<%#Eval("propClient.propSurname")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Portfolio Name" HeaderStyle-HorizontalAlign="Right" HeaderStyle-BorderStyle="None" FooterStyle-BorderStyle="None" HeaderStyle-VerticalAlign="Top">
                        <HeaderStyle CssClass="t1_column_color1" VerticalAlign="Middle" HorizontalAlign="Center"/>
                        <ItemStyle HorizontalAlign="Left" BorderStyle="None" CssClass="t1_column_color1" />                        
                        <FooterStyle BorderStyle="None" CssClass="t1_column_color1"/>
                        <ItemTemplate>
                                <asp:Label ID="lblPortfolioName" runat="server" Text='<%#Eval("propCompany")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Portfolio ID" HeaderStyle-HorizontalAlign="Right" HeaderStyle-BorderStyle="None" FooterStyle-BorderStyle="None" HeaderStyle-VerticalAlign="Top">
                        <HeaderStyle CssClass="t1_column_color2" VerticalAlign="Middle" HorizontalAlign="Center" />
                        <ItemStyle HorizontalAlign="Right"  BorderStyle="None" CssClass="t1_column_color2" />
                        <FooterStyle BorderStyle="None" CssClass="t1_column_color2"/>
                        <ItemTemplate>
                            <asp:Label runat="server" ID="gvBulkSwitchListLabelPortfolioID" Text='<%#Eval("propPortfolioID")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--<asp:TemplateField HeaderText="Switch Status" HeaderStyle-HorizontalAlign="Right" HeaderStyle-BorderStyle="None" FooterStyle-BorderStyle="None" HeaderStyle-VerticalAlign="Top">
                        <HeaderStyle CssClass="t1_column_color1" VerticalAlign="Middle" HorizontalAlign="Center"/>
                        <ItemStyle HorizontalAlign="Center" BorderStyle="None" CssClass="t1_column_color1" />                        
                        <FooterStyle BorderStyle="None" CssClass="t1_column_color1"/>
                        <ItemTemplate>
                            <asp:Label runat="server" ID="gvSwitchListLabelStatus" Text='<%#Eval("propStatusString")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderStyle-HorizontalAlign="Right" HeaderStyle-BorderStyle="None" FooterStyle-BorderStyle="None" HeaderStyle-VerticalAlign="Top">
                        <HeaderStyle CssClass="t1_column_color2" VerticalAlign="Middle" HorizontalAlign="Center"/>
                        <ItemStyle HorizontalAlign="Center" BorderStyle="None" CssClass="t1_column_color2" />  
                        <FooterStyle BorderStyle="None" CssClass="t1_column_color2"/>
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkbtnGVHistory" runat="server" Text="History" PostBackUrl='<%# String.Format("~/Portfolio/SwitchHistory.aspx?SID={0}&CID={1}&PID={2}", Eval("propSwitchID"), Eval("propClientID"), Eval("propPortfolioID"))%>' ></asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>--%>
                    <asp:TemplateField HeaderText="Selected" HeaderStyle-HorizontalAlign="Right" HeaderStyle-BorderStyle="None" FooterStyle-BorderStyle="None" HeaderStyle-VerticalAlign="Top">
                        <HeaderStyle CssClass="t1_column_color1" VerticalAlign="Middle" HorizontalAlign="Center" />
                        <ItemStyle HorizontalAlign="Right"  BorderStyle="None" CssClass="t1_column_color1" />
                        <FooterStyle BorderStyle="None" CssClass="t1_column_color1"/>
                        <ItemTemplate>
                            <asp:CheckBox ID="cboxSelected" runat="server" /><!--Checked='<%#((int)Eval("propSwitch.propSwitchID")) > 0 ? true: false %>'-->
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="propClientID" InsertVisible="true" Visible="false"/>
                    <asp:BoundField DataField="propPortfolioID" InsertVisible="true" Visible="false" />
                </Columns>
            </asp:GridView>
            </div>
        </td>
    </tr>
    <tr><td><div><center>
                        <asp:Button ID="btnProceedDiscretionaryNo" runat="server" Text="Bulk Switch" OnClick="btnProceedBulkSwitch_Click" OnClientClick="return check_sel('ctl00_ContentPlaceHolder1_gvBulkSwitchListDiscretionaryNo')" />
                        &nbsp;
                        <asp:Button ID="Button2" runat="server" Text="Cancel" />
    </center></div></td></tr>
</asp:Panel>
</table>

<div>
    <!-- Keep this control at the last to hide it, this will make validation of popup   -->
    <asp:Button ID="btnTest" runat="server"  Text="" style="display:none;"/>
    
    <ajaxToolkit:ModalPopupExtender ID="mpeSwitchPopup1" runat="server" BehaviorID="bhvrSwitchPopup1" TargetControlID="btnTest" PopupControlID="panelSwitchPopup1" CancelControlID="btnSwitchCancel" BackgroundCssClass="modalBackground" DropShadow="true" />
    
    <asp:Panel ID="panelSwitchPopup1" runat="server" style="display:none;" CssClass="modalBox"> 
        <div style="text-align:right;width:100%;margin-top:5px;">
            <asp:Label runat="server" ID="lblSwitchMessage" Text="You are about to perform this Bulk Switch. Are you sure you want to proceed?"></asp:Label><br /><br />
            <center>
            <input type="button" id="btnSwitchProceed" runat="server" onclick="showhidePopup('bhvrSwitchPopup1')" value="Proceed"/>&nbsp;&nbsp;<asp:Button runat="server" ID="btnSwitchCancel" Text="Cancel" />
            
            </center>
            <ajaxToolkit:ModalPopupExtender ID="mpeSwitchPopup2" runat="server" BehaviorID="bhvrSwitchPopup2" TargetControlID="btnTest" PopupControlID="panelSwitchPopup2" CancelControlID="" BackgroundCssClass="modalBackground" DropShadow="true" />
        </div>
    </asp:Panel>
    
    <asp:Panel ID="panelSwitchPopup2" runat="server" style="display:none;" CssClass="modalBox">
        <div style="text-align:right;width:100%;margin-top:5px;">
            <asp:Label runat="server" ID="lblMessage2" Text="Client Approval required. Request client approval for this switch"></asp:Label><br /><br />
            <center>
            <asp:Button ID="btnSwitchYes" Text="Yes" runat="server" OnClientClick="showhidePopup('bhvrSwitchPopup2')" />
            <asp:Button ID="btnSwitchNo" runat="server" Text="No" OnClick="btnSwitchNo_Click" />
            </center>
            <ajaxToolkit:ModalPopupExtender ID="mpeSwitchPopup3" runat="server" BehaviorID="bhvrSwitchPopup3" TargetControlID="btnSwitchYes" CancelControlID="btnSwitchSendCancel" PopupControlID="panelSwitchPopup3" BackgroundCssClass="modalBackground" DropShadow="true" />
        </div>
    </asp:Panel>
    
    <asp:Panel runat="server" ID="panelSwitchPopup3" style="display:none;" CssClass="modalBox" Width="350px">
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
<script type="text/javascript">
    function check_sel(gridId) {
        var grid = document.getElementById(gridId); // GridView Name
        var gridElements = grid.getElementsByTagName("input");
        count = 0;
        for (var i = 0; i < gridElements.length; i++) {
            var e = gridElements[i];
            if (e.type == "checkbox" && e.checked == true && e.id.substring(e.id.lastIndexOf('_') + 1, e.id.length) == 'cboxSelected') // Your Checkbox Control id which is within Gridview
            {
                count += 1;
            }
        }
        if (count == 0) {
            alert('Please select at least one to proceed Bulk Switch');
            return false;
        }
        else {
            return true; //$find('bhvrSwitchPopup1', null).show();
        }
    }
    function showhidePopup(mpe) {
        var popup = $find(mpe);
        var viewState = '<%=ViewState["HasDiscretionary"]%>'
        var hasDiscretionary = new Boolean(Number(viewState));
        if (hasDiscretionary == true) {
            $find('bhvrSwitchPopup3', null).show();
            popup.hide();
        }
        else {
            $find('bhvrSwitchPopup2', null).show();
            popup.hide();
        }
    }
</script>
</asp:Content>
