<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Datetime.aspx.cs" Inherits="WebApplication1.Date111time" %>

<script runat="server">
    public  string StripHtml(string html)
    {
        string StrNohtml = System.Text.RegularExpressions.Regex.Replace(html, "<[^>]+>", "");
        StrNohtml = System.Text.RegularExpressions.Regex.Replace(StrNohtml, "&[^;]+;", "");
        return StrNohtml;
    }
</script>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <% 
        
        
     %>
    </div>
    </form>
</body>
</html>
