<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="print_form1.aspx.cs" Inherits="phians.print_form1" %>

<%@ Register Assembly="PageOffice, Version=3.0.0.1, Culture=neutral, PublicKeyToken=1d75ee5788809228" Namespace="PageOffice" TagPrefix="po" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>phians打印</title>
</head>
        <script type="text/javascript" src="/easyui/jquery.min.js"></script>
   <%--<script type="text/javascript" src="/phians_js/print_form.js"></script>--%>
<body>
    <form id="Certificate_error_word_print" runat="server">
        <div id="pageoffice" style="width: 1150px; height: 640px; margin-left: auto; margin-right: auto">
            <po:PageOfficeCtrl ID="PageOfficeCtrl1" runat="server"></po:PageOfficeCtrl>
        </div>
    </form>
</body>
    <script type="text/javascript">

        $(function () {
           
            var Request = new Object();
            Request = GetRequest();
            //隐藏域--获取用户名
            // document.getElementById('HiddenField1').value = Request['user_count'];
            //alert(url);

            //$('#iframe1').prop('contentWindow').document.getElementById("PageOfficeCtrl1").Close();
            document.getElementById("PageOfficeCtrl1").WebOpen(Request['url'], "xlsNormalEdit", Request['user_name']);



        });
        // 打印预览
        function print_reports() {

            document.getElementById("PageOfficeCtrl1").PrintPreview();
            //document.getElementById("PageOfficeCtrl1").PrintOut(true, PrinterName, Copies, FromPage, ToPage, OutputFile);


        }
        function Print() {
            document.getElementById("PageOfficeCtrl1").ShowDialog(4);
        }
        //参数获取
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
        </script>

</html>
