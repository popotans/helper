<%@ Page Title="关于我们" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="About.aspx.cs" Inherits="WebApplication1.About" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <h2>
        关于
    </h2>
    <p>
        将内容放置在此处。
    </p>
    <script runat="server">
        void aaa() {
            Response.Write(DateTime.MinValue.ToString());
            Response.Write(DateTime.MinValue.Year==1);
        }

    
    
    </script>
    <%aaa(); %>
</asp:Content>
