<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DatePicker.ascx.cs"
    Inherits="AMDES_WEB.CustomControls.DatePicker" %>
<asp:TextBox ID="txtDate" runat="server" Width="170px"></asp:TextBox>
<asp:RequiredFieldValidator ID="reqValidator" runat="server" ControlToValidate="txtDate"
    Display="Static" Visible="true">*</asp:RequiredFieldValidator>
<asp:Button ID="btnChangeDate" runat="server" Text="..." ValidationGroup="date" Width="30px"
    OnClick="btnChangeDate_Click" />
&nbsp; (Format dd/mm/yyyy, e.g. 31/12/1990)
<asp:RegularExpressionValidator ID="RegularExpressionValidator1" ControlToValidate="txtDate" ValidationExpression="(^(((0[1-9]|[12][0-8])[\/](0[1-9]|1[012]))|((29|30|31)[\/](0[13578]|1[02]))|((29|30)[\/](0[4,6,9]|11)))[\/](19|[2-9][0-9])\d\d$)|(^29[\/]02[\/](19|[2-9][0-9])(00|04|08|12|16|20|24|28|32|36|40|44|48|52|56|60|64|68|72|76|80|84|88|92|96)$)"
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
