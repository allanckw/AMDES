<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="Results.aspx.cs" Inherits="AMDES_WEB.Results" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphCENTER" runat="server">
    <h2>
        Results</h2>
    <br />
    <h4>
        Patient's Age:
        <asp:Label ID="lblAge" runat="server" Text="999"></asp:Label></h4>
    <ajaxToolkit:TabContainer ID="TabContainer1" runat="server" ActiveTabIndex="0" OnDemand="true"
        AutoPostBack="false" TabStripPlacement="Top" CssClass="fancy fancy-green" ScrollBars="Auto">
        <ajaxToolkit:TabPanel ID="findings" runat="server" HeaderText="Findings" ScrollBars="Auto"
            Height="60" OnDemandMode="None">
            <ContentTemplate>
            </ContentTemplate>
        </ajaxToolkit:TabPanel>
        <ajaxToolkit:TabPanel ID="Recommendations" runat="server" HeaderText="Recommendations"
            Height="60" ScrollBars="Auto" OnDemandMode="None">
            <ContentTemplate>
            </ContentTemplate>
        </ajaxToolkit:TabPanel>
        <ajaxToolkit:TabPanel ID="Resources" runat="server" HeaderText="Resources" ScrollBars="Auto"
            Height="60" OnDemandMode="None">
            <ContentTemplate>
            </ContentTemplate>
        </ajaxToolkit:TabPanel>
    </ajaxToolkit:TabContainer>
</asp:Content>
