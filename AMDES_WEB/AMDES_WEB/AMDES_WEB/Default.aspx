<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="Default.aspx.cs" Inherits="AMDES_WEB._Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphCENTER" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <h2>
                Aid for Dementia Diagnosis (ADD)</h2>
            <h3>
                This is an expert system designed to facilitate the diagnosis of Dementia.
                <br />
                <br />
                As ADD serves primarily as a diagnostic aid, the physician should always exercise
                clinical judgment with respect to the conclusions and recommendations offered by
                the system. By using the application or site, you agree that the developers of ADD
                shall not be liable for any damages arising from the results given by the application.
                <br />
                <br />
                We recommend you start with the AD8 with partial MSME test, before proceeding to
                complete the full ADD questionaire as the former is easier and takes a shorter time
                to administer. The system will advise you if the patient will require the more comprehensive
                ADD
                <br />
                <br />
                <i>Begin by hovering over Start Assessment on the top of the page to select the appropriate
                    test to administer. </i>
                <br />
                <br />
            </h3>
            <h5>
                Your feedback will be greatly appreciated, you may send them to amdes_nus_soc[at]googlegroups.com.
                <br />
                <br />
                Credits: Geriatric Centre and Department of Medical Informatics from Khoo Teck Puat
                Hospital and National University of Singapore, School of Computing.
                <br />
                <br />
                <br />
            </h5>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
