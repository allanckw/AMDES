<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PatAttributeUC.ascx.cs"
    Inherits="AMDES_WEB.CustomControls.PatAttributeUC" %>
<tr>
    <td align="right">
        <asp:Label ID="lblFieldName" Width="150px" runat="server" Text="Label"></asp:Label> &nbsp; &nbsp;
    </td>
    <td>
        <asp:TextBox ID="txtFieldResult" runat="server"></asp:TextBox>
        <asp:DropDownList ID="ddlSelections" runat="server">
        </asp:DropDownList>
        <asp:RequiredFieldValidator ID="reqValidator" runat="server" ControlToValidate="txtFieldResult"
            Display="Static" Visible="true">*</asp:RequiredFieldValidator>
        <asp:RangeValidator ID="regRange" runat="server" ErrorMessage="" ControlToValidate="txtFieldResult"
            Display="Dynamic" Type="Double"></asp:RangeValidator>&nbsp;
        <asp:RegularExpressionValidator ID="regEmail" ControlToValidate="txtFieldResult"
            Enabled="false" ValidationExpression="^[a-zA-Z][\w\.-]*[a-zA-Z0-9]@[a-zA-Z0-9][\w\.-]*[a-zA-Z0-9]\.[a-zA-Z][a-zA-Z\.]*[a-zA-Z]$"
            Display="Dynamic" Visible="true" runat="server" ForeColor="Red"> Invalid E-mail Format </asp:RegularExpressionValidator>
    </td>
</tr>
