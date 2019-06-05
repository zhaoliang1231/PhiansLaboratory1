<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="test_records_view_word.aspx.cs" Inherits="phians.mainform.test_records_view_word" %>

<%@ Register Assembly="PageOffice, Version=3.0.0.1, Culture=neutral, PublicKeyToken=1d75ee5788809228" Namespace="PageOffice" TagPrefix="po" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
      <script type="text/javascript" src="/easyui/jquery.min.js"></script>
    <script type="text/javascript" src="/phians_js/test_records_view_word.js"></script>
    <script type="text/javascript" src="../../easyui/jquery.easyui.min.js"></script>
<script type="text/javascript" src="../../easyui/locale/easyui-lang-zh_CN.js"></script>
</head>
     <script>
         function view_office_toolbar() {
             document.getElementById("view_test_records").OfficeToolbars = false;
         }
         function Save() {
             document.getElementById("view_test_records").WebSave();

         }
         function SetFullScreen() {
             document.getElementById("view_test_records").FullScreen = !document.getElementById("PageOfficeCtrl1").FullScreen;
         }
         function OnProgressComplete() {
             //window.parent.myFunc(); //调用父页面(Default.aspx)的js函数
         }


    </script>
<body>
    <div style="text-align:center">
      
    <form id="form1" runat="server" style="width:900px;height:780px">
   
        <po:PageOfficeCtrl ID="view_test_records" runat="server"></po:PageOfficeCtrl>
    
    </form>
          
    </div>
</body>
</html>
