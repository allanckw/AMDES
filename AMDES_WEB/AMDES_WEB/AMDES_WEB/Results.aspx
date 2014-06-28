<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="Results.aspx.cs" Inherits="AMDES_WEB.Results" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphCENTER" runat="server">
    <h2>
        Results</h2>
    <h6>
        Disclaimer:
        <asp:Label ID="lblApp" runat="server" Text="Label"></asp:Label>
        serves primarily as a diagnostic aid, the physician should always exercise clinical
        judgment with respect to the conclusions and recommendations offered by the system.
        The developers of ADD shall not be liable for any damages arising from using the
        application.</h6>
    <table style="width: 100%;">
        <tr>
            <td>
                <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="~/History.aspx">View Selections to Result </asp:HyperLink>
                &nbsp;&nbsp;
                <asp:HyperLink ID="HyperLink2" NavigateUrl="~/PatientStart.aspx?appID=ADD" runat="server">Try Again </asp:HyperLink>
                &nbsp;&nbsp;
            </td>
            <td style="text-align: right;">
                <asp:HyperLink ID="HyperLink3" NavigateUrl="~/ResultsPrintFriendly.aspx" Target="_blank"
                    runat="server">Printer Friendly Report <img src="https://cdn1.iconfinder.com/data/icons/nuvola2/128x128/devices/print_printer.png" width="16" height="16" /></asp:HyperLink>
            </td>
        </tr>
    </table>
    <h4>
        Patient's Age:
        <asp:Label ID="lblAge" runat="server" Text=""></asp:Label>
        &nbsp; &nbsp; &nbsp;<br />
    </h4>
    <ajaxToolkit:TabContainer ID="TabContainer1" runat="server" ActiveTabIndex="0" OnDemand="false"
        AutoPostBack="false" TabStripPlacement="Top" CssClass="fancy fancy-green" ScrollBars="Auto">
        <ajaxToolkit:TabPanel ID="findings" runat="server" HeaderText="Findings" ScrollBars="Vertical"
            OnDemandMode="None">
            <ContentTemplate>
                <h3>
                    <asp:Label ID="lblHeader" runat="server" Text="Label"></asp:Label>
                </h3>
                <h5>
                    <asp:PlaceHolder ID="phFindings" runat="server"></asp:PlaceHolder>
                </h5>
            </ContentTemplate>
        </ajaxToolkit:TabPanel>
        <ajaxToolkit:TabPanel ID="Recommendations" runat="server" HeaderText="Recommendations"
            ScrollBars="Vertical" OnDemandMode="None">
            <ContentTemplate>
                <asp:PlaceHolder ID="phRecommendations" runat="server"></asp:PlaceHolder>
            </ContentTemplate>
        </ajaxToolkit:TabPanel>
        <ajaxToolkit:TabPanel ID="Resources" runat="server" HeaderText="Resources" ScrollBars="Vertical"
            OnDemandMode="None">
            <ContentTemplate>
                <asp:PlaceHolder ID="phResources" runat="server"></asp:PlaceHolder>
            </ContentTemplate>
        </ajaxToolkit:TabPanel>
    </ajaxToolkit:TabContainer>
    
</asp:Content>
