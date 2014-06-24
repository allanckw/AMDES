<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="Questionnaire.aspx.cs" Inherits="AMDES_WEB.Questionnaire" %>

<%@ Register Src="~/CustomControls/Section.ascx" TagPrefix="uc" TagName="Section" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphCENTER" runat="server">
    <uc:Section runat="server" ID="ucSection" />
</asp:Content>
