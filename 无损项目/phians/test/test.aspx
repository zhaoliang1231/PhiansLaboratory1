<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="test.aspx.cs" Inherits="phians.mainform.WebForm1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <script type="text/javascript" src="../easyui/jquery.min.js"></script>
<script type="text/javascript" src="../easyui/jquery.easyui.min.js"></script>
<script type="text/javascript" src="../easyui/locale/easyui-lang-zh_CN.js" ></script>
<script type="text/javascript" src="../phians_js/test.js"></script>
<link rel="stylesheet" type="text/css" href="../easyui/themes/default/easyui.css" />
<link rel="stylesheet" type="text/css" href="../easyui/themes/icon.css" />

</head>
<body > 
<form  runat="server" >
   
    <div id="loginbtn_word">           
             <a href="<%=PageOffice.PageOfficeLink.OpenWindow("/mainform/word_view.aspx?report_state=edit","width=screen.width ;height=screen.height;")%>"id="report_Auditing" class="easyui-linkbutton" data-options="iconCls:'icon-add',plain:true">审核这份报告</a>
        </div>
 </form>     
</body>
</html>
