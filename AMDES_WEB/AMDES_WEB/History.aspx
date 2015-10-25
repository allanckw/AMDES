<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="History.aspx.cs" Inherits="AMDES_WEB.History" %>

<%@ Register Src="~/CustomControls/Section.ascx" TagPrefix="uc" TagName="Section" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphCENTER" runat="server">
    <uc:Section runat="server" ID="ucSection" Enabled = "false" />
</asp:Content>
