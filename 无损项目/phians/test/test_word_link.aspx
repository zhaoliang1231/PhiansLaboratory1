<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="test_word_link.aspx.cs" Inherits="phians.test.test_word_link" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <script type="text/javascript" src="../easyui/jquery.min.js"></script>
    <script type="text/javascript" src="../easyui/jquery.easyui.min.js"></script>
    <script type="text/javascript" src="../easyui/locale/easyui-lang-zh_CN.js"></script> 
    <script type="text/javascript" src="../phians_js/test.js"></script>    
    <link rel="stylesheet" type="text/css" href="../easyui/themes/default/easyui.css" />
    <link rel="stylesheet" type="text/css" href="../easyui/themes/icon.css" />
</head>
    <script type="text/javascript" >

       
    </script>
<body>
    <form id="form1" runat="server">    
       <a href="#"class="easyui-linkbutton"data-options="iconCls:'icon-cancel'"id ="loginbtn_word"> Word模板</a>          
         <a href="<%=PageOffice.PageOfficeLink.OpenWindow("/test/test_word_view.aspx?","width=1200px;height=800px;")%>"class="easyui-linkbutton"data-options="iconCls:'icon-cancel'"id ="report_Auditing"> 打开Word模板</a>     
    </form>
</body>
   
</html>
