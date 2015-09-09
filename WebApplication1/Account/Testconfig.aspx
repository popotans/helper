<%@ Page Language="C#" AutoEventWireup="true" ValidateRequest="false" CodeBehind="Testconfig.aspx.cs" Inherits="WebApplication1.Account.Testconfig" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:Button ID="ButtonRead" runat="server" Text="Read" />
        <asp:Button ID="ButtonCreate" runat="server" Text="Create" />
    </div>

        <asp:TextBox ID="rs" TextMode="MultiLine" style=" width:600px; height:600px" runat="server"></asp:TextBox>

    </form>
</body>
</html>
