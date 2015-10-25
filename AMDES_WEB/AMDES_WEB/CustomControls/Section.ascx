<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Section.ascx.cs" Inherits="AMDES_WEB.CustomControls.Section" %>
<h2>
    <asp:Label ID="lblSection" runat="server" Text=""></asp:Label></h2>
<h3>
    <asp:Label ID="lblHeader" runat="server" Text=""></asp:Label></h3>
<table style="table-layout: fixed; width: 100%; word-wrap: break-word;">
    <tr style="min-height: 0px; max-height: 0px; height: 0px;">
        <td style="vertical-align: top; width: 5%;">
        </td>
        <td style="vertical-align: top; width: 80%;">
        </td>
        <td style="min-width: 10%; max-width: 60px">
        </td>
    </tr>
    <tr>
        <td colspan="3" style="text-align: right; margin-right: 10%">
            <div style="margin-right: 10%">
                <h4>
                    <asp:Label ID="lbl1" runat="server" Text="" Visible="false">Score: </asp:Label>
                    <asp:Label ID="lblScore" runat="server" Text="" Visible="false"></asp:Label>
                    <asp:Label ID="lblMax" runat="server" Text="" Visible="false"></asp:Label>
                </h4>
            </div>
        </td>
    </tr>
    <asp:PlaceHolder ID="phRegister" runat="server"></asp:PlaceHolder>
    <tr>
        <td colspan="3" style="text-align: right;">
            <div style="margin-right: 10%">
                <br />
                <asp:Button ID="btnPrevious" runat="server" Text="Previous" Width="70" OnClick="btnPrevious_Click" />
                <asp:Button ID="btnNext" runat="server" Text="Next" Width="70" OnClick="btnNext_Click" /></div>
        </td>
    </tr>
</table>
<br />
