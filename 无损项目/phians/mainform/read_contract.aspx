<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="read_contract.aspx.cs" Inherits="phians.mainform.read_contract" %>
<%@ Register Assembly="PageOffice, Version=3.0.0.1, Culture=neutral, PublicKeyToken=1d75ee5788809228" Namespace="PageOffice" TagPrefix="po" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>查看文档</title>
    <%= WebExtensions.CombresLink("read_contract_Js") %>
   
     <script type="text/javascript">
         function AfterDocumentOpened() {
             document.getElementById("PageOfficeCtrl1").SetEnableFileCommand(3, false); // 禁止保存
             document.getElementById("PageOfficeCtrl1").SetEnableFileCommand(4, false); // 禁止另存
             document.getElementById("PageOfficeCtrl1").SetEnableFileCommand(5, false); //禁止打印
             document.getElementById("PageOfficeCtrl1").SetEnableFileCommand(6, false); // 禁止页面设置
         }
         //全屏
         function SetFullScreen() {
             document.getElementById("PageOfficeCtrl1").FullScreen = !document.getElementById("PageOfficeCtrl1").FullScreen;
         }
         //默认全屏
         $(document).ready(function () {
             SetFullScreen();
         });
         //弹窗关闭函数
         function Close() {
             window.external.close();
         }
    </script>
        
</head>
<body style="text-align:center">
    <form id="form1" runat="server" >
    <div id="pageoffice" style="width:900px;height:680px;margin-left:auto;margin-right:auto">
        <po:PageOfficeCtrl ID="PageOfficeCtrl1" runat="server"></po:PageOfficeCtrl>
    </div>
    </form>
</body>
</html>