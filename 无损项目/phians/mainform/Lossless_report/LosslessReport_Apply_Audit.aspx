<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LosslessReport_Apply_Audit.aspx.cs" Inherits="phians.mainform.Lossless_report.LosslessReport_Apply_Audit" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <%=WebExtensions.CombresLink("public_Css") %>
</head>
<body>
    <div id="tt" class="easyui-panel" data-options="fit:true,border:false" style="margin-left: 2px; overflow: scroll; overflow-x: hidden; overflow-y: hidden">
        <div id="report_review_task"></div>
    </div>

    <div style="display: none">
        <!--<%-- 检测报告编制-报告信息-工具栏 --%>-->
        <div id="reports_toolbar">
            <span style="display: inline-block; float: left;">
                <label>管理选择：</label><a href="#" id="management_all" style="width: 100px" class="easyui-combobox"></a></span>
            <span class="" style="display: inline-block; float: left;">
                <a href="#" id="Preview_Report" class="easyui-linkbutton" data-options="iconCls:'icon-Document',plain:true">预览报告</a>
                <span class="datagrid-btn-separator" style="vertical-align: middle; height: 22px; display: inline-block; float: none"></span>
            </span>
            <span class="link_button" style="display: inline-block; float: left;">
                <a href="#" id="view_return_info" class="easyui-linkbutton" data-options="iconCls:'icon-read',plain:true">查看申请原因</a>
                <span class="datagrid-btn-separator" style="vertical-align: middle; height: 22px; display: inline-block; float: none"></span>
            </span>
            <span class="link_button" style="display: inline-block; float: left;">
                <a href="#" id="Back_report" class="easyui-linkbutton" data-options="iconCls:'icon-invalid',plain:true">拒绝</a>
                <span class="datagrid-btn-separator" style="vertical-align: middle; height: 22px; display: inline-block; float: none"></span>
            </span>
            <span class="link_button" style="display: inline-block; float: left;">
                <a href="#" id="Submit_report" class="easyui-linkbutton" data-options="iconCls:'icon-ok',plain:true">通过</a>
                <span class="datagrid-btn-separator" style="vertical-align: middle; height: 22px; display: inline-block; float: none"></span>
            </span>
            <input id="search" style="width: 150px" class="easyui-combobox" />
            <input id="search1" style="width: 150px" class="easyui-textbox" />
            <a href="#" id="search_info" class="easyui-linkbutton" data-options="iconCls:'icon-search',plain:true">搜索</a>
        </div>
        <form id="Back_report_dialog">
            <div style="margin-top: 5px; text-align: right; margin-right: 20px">
                <label style="display: inline-block; width: 80px">退回原因：</label><input id="return_info" name="return_info" data-options="multiline:true" class="easyui-textbox" style="width: 300px; height: 100px;" />
            </div>
            <div style="margin-top: 5px; text-align: right; margin-right: 20px">
                <label style="display: inline-block; width: 80px">说明：</label><input id="remarks" name="return_info" data-options="multiline:true" class="easyui-textbox" style="width: 300px; height: 100px;" />
            </div>
        </form>
    </div>

    <a href="<%=PageOffice.PageOfficeLink.OpenWindow("/mainform/read_contract.aspx?","width=1000px ;height=740px;")%>" id="read_doc" class="easyui-linkbutton" data-options="iconCls:'icon-read',plain:true">阅读文件</a>
    <a href="<%=PageOffice.PageOfficeLink.OpenWindow("/mainform/read_contract.aspx?","width=1000px ;height=740px;")%>" id="read_records" class="easyui-linkbutton" data-options="iconCls:'icon-read'" style="margin-left: 40px">查看检测记录</a>

</body>

<%=WebExtensions.CombresLink("LosslessReport_Apply_Audit_Js") %>
</html>

