<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Certificate_error_word_print.aspx.cs" Inherits="phians.mainform.Certificate_error_word_print" %>

<%@ Register Assembly="PageOffice, Version=3.0.0.1, Culture=neutral, PublicKeyToken=1d75ee5788809228" Namespace="PageOffice" TagPrefix="po" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <%= WebExtensions.CombresLink("Back_suggestion_dialogJs") %>
</head>
<body style="text-align:center">
    <form id="Certificate_error_word_print" runat="server" >
    <div id="pageoffice" style="width:900px;height:780px;margin-left:auto;margin-right:auto">
        <%--<a href="#"id="print_reports" class="easyui-linkbutton" data-options="iconCls:'icon-add',plain:true">打印这份报告</a>--%>
        <po:PageOfficeCtrl ID="PageOfficeCtrl1" runat="server"></po:PageOfficeCtrl>
    </div>
    </form>
</body>
</html>
