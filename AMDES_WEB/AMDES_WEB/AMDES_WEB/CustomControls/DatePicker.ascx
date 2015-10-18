<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DatePicker.ascx.cs"
    Inherits="AMDES_WEB.CustomControls.DatePicker" %>
<asp:TextBox ID="txtDate" runat="server" Width="170px"></asp:TextBox>
<asp:RequiredFieldValidator ID="reqValidator" runat="server" ControlToValidate="txtDate"
    Display="Static" Visible="true">*</asp:RequiredFieldValidator>
<asp:Button ID="btnChangeDate" runat="server" Text="..." ValidationGroup="date" Width="30px"
    OnClick="btnChangeDate_Click" />
&nbsp; (Format dd/mm/yyyy, e.g. 31/12/1990)
<asp:RegularExpressionValidator ID="RegularExpressionValidator1" ControlToValidate="txtDate" 
            ValidationExpression="^(?:(?:31(\/|-|\.)(?:0?[13578]|1[02]))\1|(?:(?:29|30)(\/|-|\.)(?:0?[1,3-9]|1[0-2])\2))(?:(?:1[6-9]|[2-9]\d)?\d{2})$|^(?:29(\/|-|\.)0?2\3(?:(?:(?:1[6-9]|[2-9]\d)?(?:0[48]|[2468][048]|[13579][26])|(?:(?:16|[2468][048]|[3579][26])00))))$|^(?:0?[1-9]|1\d|2[0-8])(\/|-|\.)(?:(?:0?[1-9])|(?:1[0-2]))\4(?:(?:1[6-9]|[2-9]\d)?\d{2})$"
    runat="server" ErrorMessage="Invalid Date"></asp:RegularExpressionValidator>
<table cellspacing="0" cellpadding="0" width="20%" border="0" runat="server" id="dTable">
    <tbody>
        <tr>
            <td align="left" bgcolor="#cccccc">
                <asp:DropDownList ID="drpCalMonth" runat="Server" AutoPostBack="True" CssClass="calTitle"
                    OnSelectedIndexChanged="drpCalMonth_SelectedIndexChanged" Width="100px">
                </asp:DropDownList>
            </td>
            <td align="right" bgcolor="#cccccc">
                <asp:DropDownList ID="drpCalYear" runat="Server" AutoPostBack="True" CssClass="calTitle"
                    OnSelectedIndexChanged="drpCalYear_SelectedIndexChanged" Width="100px">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:Calendar OtherMonthDayStyle-BackColor="White" DayStyle-BackColor="LightYellow"
                    ID="myCalendar" runat="Server" CssClass="calBody" DayHeaderStyle-BackColor="#eeeeee"
                    Width="100%" FirstDayOfWeek="Monday" OnSelectionChanged="myCalendar_SelectionChanged">
                </asp:Calendar>
            </td>
        </tr>
    </tbody>
</table>
