<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="QuestionsUC.ascx.cs"
    Inherits="AMDES_WEB.CustomControls.QuestionsUC" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="act" %>
<tr>
    <td style="vertical-align: top; width: 5%;">
        <div>
            <h4>
                Q<asp:Label ID="lblQnID" runat="server" Text=""></asp:Label>.&nbsp;</h4>
        </div>
    </td>
    <td style="vertical-align: top; width: 80%;">
        <h4>
            <asp:Label ID="lblQn" runat="server" Text=""></asp:Label></h4>
        <br />
        <asp:Image ID="imgQn" runat="server" Visible="false" />
    </td>
    <td style="min-width: 10%; max-width: 60px">
        <asp:CheckBox ID="chkAns" runat="server" AutoPostBack="True" 
    oncheckedchanged="chkAns_CheckedChanged" />
        <act:ToggleButtonExtender ID="ToggleEx" runat="server" TargetControlID="chkAns" ImageWidth="50"
            ImageHeight="25" CheckedImageAlternateText="Yes" UncheckedImageAlternateText="No"
            UncheckedImageUrl="~/images/ToggleButton_Unchecked.png" CheckedImageUrl="~/images/ToggleButton_Checked.png" />
    </td>
</tr>
