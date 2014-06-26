<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ResultsPrintFriendly.aspx.cs"
    Inherits="AMDES_WEB.WebForm1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Aid for Dementia Diagnosis (ADD) - Patient Report</title>
    <link rel="stylesheet" href="css/style3.css" type="text/css" />
</head>
<body style="background-color: White; color: Black; font: Times New Roman, serif;">
    <form id="form1" runat="server">
    <div style="background-color: White; color: Black;">
        <h2 style="color: Blue">
            Aid for Dementia Diagnosis (ADD) - Patient Report</h2>
        <br />
        <br />
        <h3>
            Date:
            <asp:Label ID="lblDate" runat="server" Text="Label"></asp:Label>
            <br />
            Patient's Age:
            <asp:Label ID="lblAge" runat="server" Text="Label"></asp:Label>
        </h3>
        <br />
        <br />
        <h3>
            <asp:Label ID="lblHeader" runat="server" Text="Label"></asp:Label>
        </h3>
        <h5>
            <asp:PlaceHolder ID="phFindings" runat="server"></asp:PlaceHolder>
        </h5>
        <br />
        <br />
        <asp:PlaceHolder ID="phRecommendations" runat="server"></asp:PlaceHolder>
        <br />
        <br />
        <h6>
            Disclaimer:
            <asp:Label ID="lblApp" runat="server" Text="Label"></asp:Label>
            serves primarily as a diagnostic aid, the physician should always exercise clinical
            judgment with respect to the conclusions and recommendations offered by the system.
            The developers of ADD shall not be liable for any damages arising from using the
            application.</h6>
    </div>
    </form>
</body>
</html>
