<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="Questionnaire.aspx.cs" Inherits="AMDES_WEB.Questionnaire" %>

<%@ Register Src="~/CustomControls/Section.ascx" TagPrefix="uc" TagName="Section" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphCENTER" runat="server">
    <div style="width: 80%">
        <uc:Section runat="server" ID="ucSection" />
    </div>
</asp:Content>
