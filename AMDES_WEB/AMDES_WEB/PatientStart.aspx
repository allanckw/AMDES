<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="PatientStart.aspx.cs" Inherits="AMDES_WEB.patienttStart" %>

<%@ Register Assembly="MSCaptcha" Namespace="MSCaptcha" TagPrefix="cc1" %>
<%@ Register Src="~/CustomControls/DatePicker.ascx" TagName="DatePicker" TagPrefix="AOS" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphCENTER" runat="server">
    <h2>
        New Patient</h2>
    <h3>
        <label runat="server" id="lblEventName">
        </label>
    </h3>
    <asp:PlaceHolder ID="phRegister" runat="server"></asp:PlaceHolder>
    <br />
    <table>
        <tr>
            <td align="right">
                Date of birth
            </td>
            <td>
                <AOS:DatePicker ID="dpFrom" runat="server" DisplayFutureDate="false" Visible="true"
                    MinimumYear="1900" />
            </td>
        </tr>
        <tr>
            <td align="right">
                <asp:Label ID="lblFieldName" Width="150px" runat="server" Text="Captcha"></asp:Label>
                &nbsp; &nbsp;
            </td>
            <td>
                <cc1:CaptchaControl ID="ccJoin" runat="server" Height="50px" CaptchaBackgroundNoise="Low"
                    Width="180px" CaptchaLength="5" BackColor="White" EnableViewState="False" />
                <asp:TextBox ID="txtCaptcha" runat="server" MaxLength="5"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Verification Required"
                    ControlToValidate="txtCaptcha" Display="Static" Visible="true">* </asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;
            </td>
            <td>
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;
            </td>
            <td>
                <asp:Button ID="btnStart" runat="server" Text="Start Test" OnClick="btnStart_Click" />
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;
            </td>
            <td>
                <br />
                <br />
                <br />
                <br />
                <br />
                <h6>
                    <asp:Label ID="lblTest" runat="server" Text="Label"></asp:Label>
                    has benefitted
                    <asp:Label ID="lblCount" runat="server" Text="Label"></asp:Label>
                    patients
                </h6>
            </td>
        </tr>
    </table>
</asp:Content>
