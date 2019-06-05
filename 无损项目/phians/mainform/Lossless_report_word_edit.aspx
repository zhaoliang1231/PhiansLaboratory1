<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Lossless_report_word_edit.aspx.cs" Inherits="phians.mainform.Lossless_report_word_edit" %>

<%@ Register Assembly="PageOffice, Version=3.0.0.1, Culture=neutral, PublicKeyToken=1d75ee5788809228" Namespace="PageOffice" TagPrefix="po" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
      <title>Phians</title>
        <%=WebExtensions.CombresLink("word_view_Js") %>

    <script type="text/javascript">
        //function setTitleText() {
        //    document.getElementById("PageOfficeCtrl1").DataRegionList.GetDataRegionByName("PO_sample_name").Value = '001';
        //}
        function AfterDocumentOpened() {
            document.getElementById("PageOfficeCtrl1").SetEnableFileCommand(3, false); // 禁止保存
            document.getElementById("PageOfficeCtrl1").SetEnableFileCommand(4, false); // 禁止另存
            document.getElementById("PageOfficeCtrl1").SetEnableFileCommand(5, false); //禁止打印
             document.getElementById("PageOfficeCtrl1").SetEnableFileCommand(6, false); // 禁止页面设置
        }
        function view_office_toolbar()
        {
            document.getElementById("PageOfficeCtrl1").OfficeToolbars = !document.getElementById("PageOfficeCtrl1").OfficeToolbars;
        }
        function Save() {
            document.getElementById("PageOfficeCtrl1").WebSave();

        }
        function SetFullScreen() {
            document.getElementById("PageOfficeCtrl1").FullScreen = !document.getElementById("PageOfficeCtrl1").FullScreen;
        }
        function OnProgressComplete() {
            //window.parent.myFunc(); //调用父页面(Default.aspx)的js函数
        }

        $(document).ready(function () {
            SetFullScreen();
        });
        //弹窗关闭函数
        function Close() {
            window.external.close();
        }
    </script>
</head>

<body>
    
    <form id="form1" runat="server" style="width:900px;height:800px">   
       
      <po:PageOfficeCtrl ID="PageOfficeCtrl1" runat="server"></po:PageOfficeCtrl>
      
    </form>
</body>
</html>
