<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebForm2.aspx.cs" Inherits="phians.mainform.WebForm2" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
     <%= WebExtensions.CombresLink("WebForm2_Js") %>
     <%= WebExtensions.CombresLink("WebForm2_Css") %>
</head>
<body>
    <div class="login-box" id="login-id">
        <p>
            <label style="width: 100px">帐  号</label>
            <input name="username" value="name" type="text" class="normal" />
        </p>
        <p>
            <label style="width: 100px">密码</label>
            <input name="username" value="name" type="text" class="normal" />
        </p>
    </div>
</body>
</html>
