<%@ Page Title="" Language="C#" MasterPageFile="~/NAV.Master" AutoEventWireup="true" CodeBehind="SwitchFeeMaintenance.aspx.cs" Inherits="NAV.Admin.Fee_List" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"> </asp:ToolkitScriptManager>
<br />&nbsp;
<br />&nbsp;
<table border="0" align="left" cellpadding="0" cellspacing="0" width="100%">
    <tr><td><h2>Switch Fee Configuration</h2></td></tr>
    <tr>
        <td>
            <div id="table1_container">
            <asp:GridView ID="gvSwitchFee_List" runat="server" AutoGenerateColumns="false" DataKeyNames="" Width="100%" ShowFooter="true" BorderStyle="None" CellPadding="0" CellSpacing="0" UseAccessibleHeader="true" CssClass="table1" EmptyDataText="No record found">
                <Columns>
                    <asp:TemplateField>
                        <HeaderStyle CssClass="t1_column_color1" BorderStyle="None" VerticalAlign="Middle" HorizontalAlign="Center"/>
                        <ItemStyle CssClass="t1_column_color1" BorderStyle="None" HorizontalAlign="Left"  />
                        <FooterStyle CssClass="t1_column_color1" BorderStyle="None" />
                        <ItemTemplate><asp:LinkButton ID="lbtnRemoveIFASwitchFee" runat="server" Text="Delete" CommandArgument='<%#Eval("propIFA_ID")%>' OnCommand="lbtnRemoveIFASwitchFee_Click" OnClientClick="return confirm('You are about to delete this fee. Are you sure you want to proceed?');"></asp:LinkButton> || <asp:LinkButton ID="lbtnEditIFASwitchFee" runat="server" Text="Edit" CommandArgument='<%#Eval("propIFA_ID")%>' OnCommand="lbtnEditIFASwitchFee_Click"></asp:LinkButton></ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="propIFA_ID" HeaderText="IFA ID" 
                                    HeaderStyle-BorderStyle="None" HeaderStyle-VerticalAlign="Middle" HeaderStyle-CssClass="t1_column_color2" 
                                    ItemStyle-BorderStyle="None" ItemStyle-HorizontalAlign="Left" ItemStyle-CssClass="t1_column_color2" 
                                    FooterStyle-BorderStyle="None" FooterStyle-CssClass="t1_column_color2"/>
                    <asp:BoundField DataField="propIFA_Name" HeaderText="IFA Name" 
                                    HeaderStyle-BorderStyle="None" HeaderStyle-VerticalAlign="Middle" HeaderStyle-CssClass="t1_column_color1" 
                                    ItemStyle-BorderStyle="None" ItemStyle-HorizontalAlign="Left" ItemStyle-CssClass="t1_column_color1"
                                    FooterStyle-BorderStyle="None" FooterStyle-CssClass="t1_column_color1"/>
                    <asp:BoundField DataField="propAnnual_Fee" HeaderText="Annual Fee" 
                                    HeaderStyle-BorderStyle="None" HeaderStyle-VerticalAlign="Middle" HeaderStyle-CssClass="t1_column_color2" 
                                    ItemStyle-BorderStyle="None" ItemStyle-HorizontalAlign="Right" ItemStyle-CssClass="t1_column_color2" 
                                    FooterStyle-BorderStyle="None" FooterStyle-CssClass="t1_column_color2"/>
                    <asp:BoundField DataField="propPer_Switch_Fee" HeaderText="Per Switch Fee" 
                                    HeaderStyle-BorderStyle="None" HeaderStyle-VerticalAlign="Middle" HeaderStyle-CssClass="t1_column_color1" 
                                    ItemStyle-BorderStyle="None" ItemStyle-HorizontalAlign="Right" ItemStyle-CssClass="t1_column_color1"
                                    FooterStyle-BorderStyle="None" FooterStyle-CssClass="t1_column_color1"/>
                    <asp:TemplateField HeaderText="Deny Access">
                        <HeaderStyle CssClass="t1_column_color2" BorderStyle="None" VerticalAlign="Middle" HorizontalAlign="Center"/>
                        <ItemStyle CssClass="t1_column_color2" BorderStyle="None" HorizontalAlign="Center"  />
                        <FooterStyle CssClass="t1_column_color2" BorderStyle="None" />
                        <ItemTemplate>
                            <asp:CheckBox ID="cboxAccessDenied" runat="server" Checked='<%#Eval("propAccess_Denied")%>' Enabled="false" />
                        </ItemTemplate>
                </asp:TemplateField>                           
                </Columns>
            </asp:GridView>
            </div>
        </td>
    </tr>
    <tr>
        <td align="left">
            <asp:Button ID="btnAdd" runat="server" OnClick="btnAdd_Click" Text="   Add   " />
        </td>
</tr>
</table>
<div>
    <asp:ModalPopupExtender ID="mpeSwitchFeeConfig" runat="server" TargetControlID="mpeButton" CancelControlID="btnCancelSwitchFeeConfig" PopupControlID="panelSwitchFeeConfig" BehaviorID="bhvrSwitchFeeConfig" BackgroundCssClass="modalBackground" DropShadow="true" />
    <asp:Panel ID="panelSwitchFeeConfig" runat="server" CssClass="modalBox" style="display:none;" >
    <div style="text-align:left;width:100%;margin-top:1px;">
        <table>
            <tr>
                <td><asp:Label ID="lblIFAList" runat="server" Text="IFA List"></asp:Label></td>
                <td><asp:DropDownList ID="ddlIFAList" runat="server"></asp:DropDownList></td>
            </tr>
            <tr>
                <td><asp:Label ID="lblAnnualFeeAmount" runat="server" Text="Annual Fee Amount"></asp:Label></td>
                <td><asp:TextBox ID="txtAnnualFeeAmount" runat="server" Text="0.00" onblur="extractNumber(this,2,false);" onkeyup="extractNumber(this,2,false);" onkeypress="return blockNonNumbers(this, event, true, false);"></asp:TextBox></td>
            </tr>
            <tr>
                <td><asp:Label ID="lblPerSwitchFeeAmount" runat="server" Text="Per Switch Fee Amount"></asp:Label></td>
                <td><asp:TextBox ID="txtPerSwitchFeeAmount" runat="server" Text="0.00" onblur="extractNumber(this,2,false);" onkeyup="extractNumber(this,2,false);" onkeypress="return blockNonNumbers(this, event, true, false);"></asp:TextBox></td>
            </tr>
            <tr>
                <td><asp:Label ID="lblAccessDenied" runat="server" Text="Deny Access"></asp:Label></td>
                <td><asp:CheckBox ID="cboxAccessDenied" runat="server" /></td>
            </tr>
            <tr><td>&nbsp;</td></tr>
            <tr>
                <td colspan="2">
                    <div>
                        <center>
                            <asp:Button ID="btnEditSwitchFeeConfig" runat="server" Text="Save" OnClick="btnEditSwitchFeeConfig_Click" />
                            <asp:Button ID="btnAddSwitchFeeConfig" runat="server" Text="Save" OnClick="btnAddSwitchFeeConfig_Click" /> &nbsp; 
                            <asp:Button ID="btnCancelSwitchFeeConfig" runat="server" Text="Cancel" />
                        </center>
                    </div>
                </td>
            </tr>
        </table>
    </div>
    </asp:Panel>
</div>

<asp:Button ID="mpeButton" runat="server"  Text="" style="display:none;"/>

<script language="javascript" type="text/javascript">
    function extractNumber(obj, decimalPlaces, allowNegative) {
        var temp = obj.value;

        // avoid changing things if already formatted correctly
        var reg0Str = '[0-9]*';
        if (decimalPlaces > 0) {
            reg0Str += '\\.?[0-9]{0,' + decimalPlaces + '}';
        } else if (decimalPlaces < 0) {
            reg0Str += '\\.?[0-9]*';
        }
        reg0Str = allowNegative ? '^-?' + reg0Str : '^' + reg0Str;
        reg0Str = reg0Str + '$';
        var reg0 = new RegExp(reg0Str);
        if (reg0.test(temp)) return true;

        // first replace all non numbers
        var reg1Str = '[^0-9' + (decimalPlaces != 0 ? '.' : '') + (allowNegative ? '-' : '') + ']';
        var reg1 = new RegExp(reg1Str, 'g');
        temp = temp.replace(reg1, '');

        if (allowNegative) {
            // replace extra negative
            var hasNegative = temp.length > 0 && temp.charAt(0) == '-';
            var reg2 = /-/g;
            temp = temp.replace(reg2, '');
            if (hasNegative) temp = '-' + temp;
        }

        if (decimalPlaces != 0) {
            var reg3 = /\./g;
            var reg3Array = reg3.exec(temp);
            if (reg3Array != null) {
                // keep only first occurrence of .
                //  and the number of places specified by decimalPlaces or the entire string if decimalPlaces < 0
                var reg3Right = temp.substring(reg3Array.index + reg3Array[0].length);
                reg3Right = reg3Right.replace(reg3, '');
                reg3Right = decimalPlaces > 0 ? reg3Right.substring(0, decimalPlaces) : reg3Right;
                temp = temp.substring(0, reg3Array.index) + '.' + reg3Right;
            }
        }

        obj.value = temp;
    }
    function blockNonNumbers(obj, e, allowDecimal, allowNegative) {
        var key;
        var isCtrl = false;
        var keychar;
        var reg;

        if (window.event) {
            key = e.keyCode;
            isCtrl = window.event.ctrlKey
        }
        else if (e.which) {
            key = e.which;
            isCtrl = e.ctrlKey;
        }

        if (isNaN(key)) return true;

        keychar = String.fromCharCode(key);

        // check for backspace or delete, or if Ctrl was pressed
        if (key == 8 || isCtrl) {
            return true;
        }

        reg = /\d/;
        var isFirstN = allowNegative ? keychar == '-' && obj.value.indexOf('-') == -1 : false;
        var isFirstD = allowDecimal ? keychar == '.' && obj.value.indexOf('.') == -1 : false;

        return isFirstN || isFirstD || reg.test(keychar);
    }
/*
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
        //limit the decimal into two places
        arrNumberValue = sender.value.split('.');
        if (arrNumberValue.length > 1) {
            if (arrNumberValue[1].length > 1) {
                return false;
            }
        }
    }
    function count(s1, letter) {
        try {
            return String(s1).match(new RegExp(letter, 'g')).length;
        } catch (err) { }
    }
*/
</script>
</asp:Content>