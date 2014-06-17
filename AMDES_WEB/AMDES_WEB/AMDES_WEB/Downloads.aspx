<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="Downloads.aspx.cs" Inherits="AMDES_WEB.Downloads" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphCENTER" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <h2>
                Aid for Dementia Diagnosis (ADD) Downloads</h2>
           
                <h3>
                    Instructions</h3>
                <ol>
                    <li>Download the zipped application by clicking
                        <asp:HyperLink ID="HyperLink1" NavigateUrl="http://bit.ly/AMDES_LIght" runat="server">Here</asp:HyperLink>
                    </li>
                    <li>Unzipped the zipped folder into a location of your choice.</li>
                    <li>The software requires the .NET framework 4.0 to run, if you do not have it you may
                        download it from Microsoft
                        <asp:HyperLink ID="HyperLink2" NavigateUrl="http://www.microsoft.com/en-sg/download/details.aspx?id=17718"
                            runat="server">Here</asp:HyperLink>.</li>
                    <li>Double Click on Add.exe to start running the program</li>
                </ol>
                <p>
                
           
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
