<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="WebApplication1.testDatabase._default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:Button ID="btnInsert"  runat="server" Text="Insert" />

        <asp:Button ID="btnUpdate" runat="server" Text="Update" />

        <asp:Button ID="btnGet" runat="server" Text="Get" />

         <asp:Button ID="btndel" runat="server" Text="Delete" />
    </div>
    </form>
</body>
</html>
