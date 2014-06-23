<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="QuestionsUC.ascx.cs"
    Inherits="AMDES_WEB.CustomControls.QuestionsUC" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="act" %>
<tr>
    <td style="width: 10%; height: 30px;" align="right">
        <h4>
            Q<asp:Label ID="lblQnID" runat="server" Text=""></asp:Label>. &nbsp;
        </h4>
    </td>
    <td style="width: 80%">
        <h4>
            <asp:Label ID="lblQn" runat="server" Text=""></asp:Label></h4>
    </td>
    <td style="width: 5%">
        <asp:CheckBox ID="chkAns" runat="server" AutoPostBack="True" 
    oncheckedchanged="chkAns_CheckedChanged" />
        <act:ToggleButtonExtender ID="ToggleEx" runat="server" TargetControlID="chkAns" ImageWidth="50"
            ImageHeight="25" CheckedImageAlternateText="Yes" UncheckedImageAlternateText="No"
            UncheckedImageUrl="~/images/ToggleButton_Unchecked.png" CheckedImageUrl="~/images/ToggleButton_Checked.png" />
    </td>
</tr>
