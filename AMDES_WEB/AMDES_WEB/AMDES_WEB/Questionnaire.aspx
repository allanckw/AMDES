<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="Questionnaire.aspx.cs" Inherits="AMDES_WEB.Questionnaire" %>

<%@ Register Src="~/CustomControls/Section.ascx" TagPrefix="uc" TagName="Section" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphCENTER" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <uc:Section runat="server" ID="ucSection" Enabled="true" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
