<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="PdfAreaCalc_PdfAreaCalc" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>PdfAreaCalc</title>
    <meta name="viewport" content="width=device-width, initial-scale=1.0"/>
</head>
<body>
    <form id="PdfAreaCalc" runat="server">
        <%--<div style="float: none; margin-left: 0px;">--%>
        <div>
            <h2>Pdf Area kalkylator</h2>
            <b>Katalog för pdf-filer</b> &nbsp 
            <asp:TextBox ID="DirectoryTextBox" ToolTip="Katalog för pdf-filer" runat="server" Width="200"></asp:TextBox>
            <br /><br />
            <b>Areatyp</b> &nbsp
            <asp:DropDownList ID="AreaTypeDropDownList" runat="server" Height="20" Width="100">
                <asp:ListItem Text="BleedBox" Value="0"></asp:ListItem>
                <asp:ListItem Text="CropBox" Value="1"></asp:ListItem>
                <asp:ListItem Text="MediaBox" Value="2" Selected="True"></asp:ListItem>
                <asp:ListItem Text="TrimBox" Value="3"></asp:ListItem>
            </asp:DropDownList>
            <br /><br />
            <asp:Button ID="InvokeButton" runat="server" Text="Hämta Pdf-Areor" Width="150" OnClick="InvokeButton_Click" />
            <br /><br />
            <asp:TextBox ID="ResultTextBox" TextMode="MultiLine" Width="700" Height="400" runat="server"></asp:TextBox>
            <footer> <br />Footer Text
            </footer>
        </div>
    </form>
</body>
</html>
