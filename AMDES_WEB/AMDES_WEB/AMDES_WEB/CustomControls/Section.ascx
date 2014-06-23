<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Section.ascx.cs" Inherits="AMDES_WEB.CustomControls.Section" %>
<h2>
    <asp:Label ID="lblSection" runat="server" Text=""></asp:Label></h2>
<h3>
    <asp:Label ID="lblHeader" runat="server" Text=""></asp:Label></h3>
<table>
    <asp:PlaceHolder ID="phRegister" runat="server"></asp:PlaceHolder>
    <tr>
        <td colspan="3" align="right" style="width: 100%;">
            <h4>
                <asp:Label ID="lbl1" runat="server" Text="">Score: </asp:Label>
                <asp:Label ID="lblScore" runat="server" Text=""></asp:Label>
                <asp:Label ID="lblMax" runat="server" Text=""></asp:Label>
            </h4>
        </td>
    </tr>
    <tr>
        <td colspan="3" align="right" style="width: 100%;">
            <asp:Button ID="btnPrevious" runat="server" Text="Previous" Width="70" 
                onclick="btnPrevious_Click" />
            <asp:Button ID="btnNext" runat="server" Text="Next" Width="70" 
                onclick="btnNext_Click" />
        </td>
    </tr>
</table>
<br />
