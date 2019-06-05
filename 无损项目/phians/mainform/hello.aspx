<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="hello.aspx.cs" Inherits="phians.mainform.hello" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <%= WebExtensions.CombresLink("hello_Js") %>
    <%= WebExtensions.CombresLink("mainformCss") %> 
</head>
<body >
    <form id="form1" runat="server">
        <asp:Button ID="Button1" runat="server" Text="Button" OnClick="Button1_Click" />
    <div id="testtree">
    </div>
    </form>
</body>
</html>
