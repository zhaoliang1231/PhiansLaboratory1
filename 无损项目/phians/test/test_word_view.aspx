<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="test_word_view.aspx.cs" Inherits="phians.test.test_word_view" %>

<%@ Register Assembly="PageOffice, Version=3.0.0.1, Culture=neutral, PublicKeyToken=1d75ee5788809228" Namespace="PageOffice" TagPrefix="po" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
     <script type="text/javascript">
         function AfterDocumentOpened() {
             document.getElementById("PageOfficeCtrl1").SetEnableFileCommand(4, false);  //禁止另存
             //document.getElementById("PageOfficeCtrl1").SetEnableFileCommand(5, false);  //禁止打印
             //document.getElementById("PageOfficeCtrl1").SetEnableFileCommand(6, false);  //禁止页面设置
             //document.getElementById("PageOfficeCtrl1").SetEnableFileCommand(8, false);  //禁止打印预览
         }
    </script>
     <script>

         function GetRequest() {
             var url = location.search; //获取url中"?"符后的字串
             var theRequest = new Object();
             if (url.indexOf("?") != -1) {
                 var str = url.substr(1);
                 strs = str.split("&");
                 for (var i = 0; i < strs.length; i++) {
                     theRequest[strs[i].split("=")[0]] = unescape(strs[i].split("=")[1]);
                 }
             }
             return theRequest;
         }
         var Request = new Object();
         Request = GetRequest();
         alert(Request['id']);
         alert(Request['id2']);
         alert(Request['urlx']);
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div style="height:780px;width:1200px">
        <po:PageOfficeCtrl ID="PageOfficeCtrl1" runat="server"></po:PageOfficeCtrl>
    </div>
    </form>
</body>
</html>
