<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="print.aspx.cs" Inherits="phians.mainform.print" %>
<%@ Register Assembly="PageOffice, Version=3.0.0.1, Culture=neutral, PublicKeyToken=1d75ee5788809228" Namespace="PageOffice" TagPrefix="po" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <%= WebExtensions.CombresLink("print_Js") %>
</head>
<body style="text-align:center">
        <script type="text/javascript">
            // 打印报告
            function print_reports() {
                document.getElementById("PageOfficeCtrl1").PrintPreview();
                //document.getElementById("PageOfficeCtrl1").PrintOut(true, PrinterName, Copies, FromPage, ToPage, OutputFile);
            }
            function Print() {
                document.getElementById("PageOfficeCtrl1").ShowDialog(4);
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
    <form id="form1" runat="server" >
    <div id="pageoffice" style="width:750px;height:780px;margin-left:auto;margin-right:auto">
        <%--<a href="#"id="print_reports" class="easyui-linkbutton" data-options="iconCls:'icon-add',plain:true">打印这份报告</a>--%>
        <po:PageOfficeCtrl ID="PageOfficeCtrl1" runat="server"></po:PageOfficeCtrl>
    </div>
    </form>
</body>
</html>
