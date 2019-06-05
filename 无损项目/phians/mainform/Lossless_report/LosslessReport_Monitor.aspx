<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LosslessReport_Monitor.aspx.cs" Inherits="phians.mainform.Lossless_report.LosslessReport_Monitor" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <%=WebExtensions.CombresLink("public_Css") %>
</head>
<body>
    <div id="tt" class="easyui-panel" data-options="fit:true,border:false" style="margin-left: 2px; overflow: scroll; overflow-x: hidden; overflow-y: hidden">
        <div id="report_management"></div>
    </div>
    <div style="display: none">
        <!-- 报告管理工具栏 -->
        <div id="report_management_toolbar">

            <span class="link_button" style="display: inline-block; float: left;">
                <a href="#" id="download_report_pdf" class="easyui-linkbutton print_report" data-options="iconCls:'icon-print',plain:true">预览打印报告</a>
                <span class="datagrid-btn-separator" style="vertical-align: middle; height: 22px; display: inline-block; float: none"></span>
            </span>

            <span class="link_button" style="display: inline-block; float: left;">
                <a href="#" id="download_info" class="easyui-linkbutton" data-options="iconCls:'icon-upload',plain:true">下载报告</a>
                <span class="datagrid-btn-separator" style="vertical-align: middle; height: 22px; display: inline-block; float: none"></span>
            </span>
            <label>检索条件：</label>
            <input id="search3" style="width: 110px" class="easyui-combobox" />
             <span id="key_span3" style="display:inline-block"><input id="key3" style="width: 110px" class="easyui-textbox" /></span>
            <input id="search2" style="width: 110px" class="easyui-combobox" />
           <span id="key_span2" style="display:inline-block"><input id="key2" style="width: 110px" class="easyui-textbox" /></span>
            <input id="search1" style="width: 110px" class="easyui-combobox" />
            <span id="key_span1" style="display:inline-block"><input id="key1" style="width: 110px" class="easyui-textbox" /></span>
            <input id="search" style="width: 110px" class="easyui-combobox" />
             <span id="key_span" style="display:inline-block"><input id="key" style="width: 110px" class="easyui-textbox" /></span>
            <a href="#" id="report_search" class="easyui-linkbutton" data-options="iconCls:'icon-search',plain:true">搜索</a>
            <span class="datagrid-btn-separator" style="vertical-align: middle; height: 22px; display: inline-block; float: none"></span>
        </div>
    </div>

    <form id="Picture_form">
        <img id="Picture_img" src="#" width="200" height="120" />
    </form>

</body>

<%=WebExtensions.CombresLink("LosslessReport_Monitor_Js") %>
</html>
