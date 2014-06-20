<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TestPagedhelper.aspx.cs" Inherits="WebApplication1.TestPagedhelper" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <%
        Response.Write(Request["page"]);
        Response.Write("<br/>");

        int current = 1;
        int.TryParse(Request["page"], out current);
        if (current < 1) current = 1;

        Response.Write(Helper.Web.PagedHelper.RenderQueryPageNum(current, 120, 10, "TestPagedhelper.aspx?page={page}"));     
        
        
    %>
    </div>
    </form>
</body>
</html>
