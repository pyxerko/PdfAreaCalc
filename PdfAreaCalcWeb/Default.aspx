<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="PdfAreaCalcPage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>PdfAreaCalc</title>
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
</head>
<body>
    <form id="PdfAreaCalc" runat="server">
        <div>
            <h2>Pdf Area kalkylator</h2>
            <b>Katalog för pdf-filer på server</b> &nbsp 
            <asp:TextBox ID="DirectoryTextBox" ToolTip="Katalog för pdf-filer på server" runat="server" Width="200"></asp:TextBox>
            <br />
            <br />
            <b>Areatyp</b> &nbsp
            <asp:DropDownList ID="AreaTypeDropDownList" runat="server" Height="20" Width="100">
                <asp:ListItem Text="BleedBox" Value="0"></asp:ListItem>
                <asp:ListItem Text="CropBox" Value="1"></asp:ListItem>
                <asp:ListItem Text="MediaBox" Value="2" Selected="True"></asp:ListItem>
                <asp:ListItem Text="TrimBox" Value="3"></asp:ListItem>
            </asp:DropDownList>
            &nbsp &nbsp<b>FilMask</b>&nbsp
            <asp:TextBox ID="FileMaskTextBox" runat="server" Width="150" Text="*.pdf"></asp:TextBox>
            <br />
            <br />
            <asp:Button ID="InvokeButton" runat="server" Text="Hämta Pdf-Areor" Width="150" OnClick="InvokeButton_Click" />
            &nbsp &nbsp &nbsp &nbsp<asp:CheckBox ID="UseGridCheckBox" runat="server" Text="Use grid"/>
            <br />
            <br />
            <asp:Table ID="ResultTable" runat="server" BorderStyle="Solid"
                GridLines="Both" Height="50">
            </asp:Table>
             <br />
            <br />
            <asp:TextBox ID="ResultTextBox" TextMode="MultiLine" Width="700" Height="400" runat="server" Visible="false"></asp:TextBox>
            <footer>
                <br />
                <div style="float: right;">
                    <asp:Label ID="CurrentDtLabel" runat="server" Text="DateTime"></asp:Label>
                </div>
                Footer Text 
            </footer>
        </div>
    </form>
</body>
</html>
