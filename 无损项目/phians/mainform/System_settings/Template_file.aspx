<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Template_file.aspx.cs" Inherits="phians.mainform.System_settings.Template_file" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <%--<link rel="stylesheet" type="text/css" href="/easyui/themes/default/easyui.css" />
    <link rel="stylesheet" type="text/css" href="/easyui/themes/icon.css" />
    <link rel="stylesheet" type="text/css" href="/phians_css/mainform.css" />
    <link rel="stylesheet" type="text/css" href="/phians_css/system_file.css" />
    <link rel="stylesheet" type="text/css" href="/phians_css/myStyle.css" />
    <link rel="stylesheet" type="text/css" href="/uploadify/uploadify.css" />--%>
    <%= WebExtensions.CombresLink("Environmental_management_Css") %>
</head>
<body>
    <div class="easyui-panel" data-options="fit:true,border:false">

        <div id="report">
        </div>
    </div>
    <%--工具栏--%>
    <div style="display: none">
        <div id="report_toolbar">
<%--            <a href="#" id="report_add_file" class="easyui-linkbutton" data-options="iconCls:'icon-add',plain:true">增加模板</a>
            <span class="datagrid-btn-separator" style="vertical-align: middle; height: 22px; display: inline-block; float: none"></span>
            <a href="#" id="report_alter_file" class="easyui-linkbutton" data-options="iconCls:'icon-edit',plain:true">修改模板</a>
            <span class="datagrid-btn-separator" style="vertical-align: middle; height: 22px; display: inline-block; float: none"></span>--%>
            <%--        <a href="#" id="report_remove" class="easyui-linkbutton" data-options="iconCls:'icon-remove',plain:true">删除模板</a>
            <span class="datagrid-btn-separator" style="vertical-align: middle; height: 22px; display: inline-block; float: none"></span>--%>

            <a href="#" id="report_read_2" class="easyui-linkbutton" data-options="iconCls:'icon-edit',plain:true">编辑模板</a>
            <span class="datagrid-btn-separator" style="vertical-align: middle; height: 22px; display: inline-block; float: none"></span>
            <a href="#" id="DownloadReport" class="easyui-linkbutton" data-options="iconCls:'icon-downloads',plain:true">下载模板</a>
            <span class="datagrid-btn-separator" style="vertical-align: middle; height: 22px; display: inline-block; float: none"></span>
            <a href="#" id="report_read_1" class="easyui-linkbutton" data-options="iconCls:'icon-Document',plain:true">阅读文件</a>

            <%--<span class="datagrid-btn-separator" style="vertical-align: middle; height: 22px; display: inline-block; float: none"></span>
            <label>导出范围：</label>
            <input id="all2" type="checkbox" checked="checked" value="1" /><label class="all2">全部导出</label>
            <input id="choose2" type="checkbox" value="0" /><label class="choose2">选择导出　</label>
            <a href="#" id="report_export" class="easyui-linkbutton" data-options="iconCls:'icon-export',plain:true">导出表格</a>
            <span class="datagrid-btn-separator" style="vertical-align: middle; height: 22px; display: inline-block; float: none"></span>
            <input id="search2" style="width: 150px" />
            <input id="search3" style="width: 150px" />
            <a href="#" id="report_search" class="easyui-linkbutton" data-options="iconCls:'icon-search',plain:true">搜索</a>--%>
        </div>

    </div>

    <%-- 阅读文件隐藏按钮doc --%>
    <a href="<%=PageOffice.PageOfficeLink.OpenWindow("/mainform/read_contract.aspx?","width=1000px ;height=740px;")%>" id="read_doc" class="easyui-linkbutton" data-options="iconCls:'icon-read',plain:true">阅读文件</a>
    <a href="<%=PageOffice.PageOfficeLink.OpenWindow("/mainform/word_view.aspx?report_state=edit&save_type=certificate_Template&","width=1000px ;height=740px;")%>" id="edit_word" class="easyui-linkbutton" data-options="iconCls:'icon-read',plain:true">阅读文件</a>
    <%-- 阅读文件隐藏按钮pdf --%>
    <a href="<%=PageOffice.PageOfficeLink.OpenWindow("/mainform/read_pdf.aspx?","width=1000px ;height=740px;")%>" id="read_pdf" class="easyui-linkbutton" data-options="iconCls:'icon-read',plain:true">阅读文件</a>
</body>

<%= WebExtensions.CombresLink("Template_file_Js") %>
</html>
