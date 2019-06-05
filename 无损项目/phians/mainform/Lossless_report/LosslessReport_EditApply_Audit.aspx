﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LosslessReport_EditApply_Audit.aspx.cs" Inherits="phians.mainform.Lossless_report.WebForm2" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <%=WebExtensions.CombresLink("public_Css") %>
</head>
<body>
    <div id="tt" class="easyui-panel" data-options="fit:true,border:false" style=" margin-left: 2px; overflow: scroll; overflow-x: hidden; overflow-y: hidden">
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
                <a href="#" id="Back_report" class="easyui-linkbutton" data-options="iconCls:'icon-undo',plain:true">退回编制</a>
                <span class="datagrid-btn-separator" style="vertical-align: middle; height: 22px; display: inline-block; float: none"></span>
            </span>
            <span class="link_button" style="display: inline-block; float: left;">
                <a href="#" id="Submit_report" class="easyui-linkbutton" data-options="iconCls:'icon-ok',plain:true">通过审核</a>
                <span class="datagrid-btn-separator" style="vertical-align: middle; height: 22px; display: inline-block; float: none"></span>
            </span>
            <input id="search" style="width: 150px" class="easyui-combobox" />
            <input id="search1" style="width: 150px" class="easyui-textbox" />
            <a href="#" id="search_info" class="easyui-linkbutton" data-options="iconCls:'icon-search',plain:true">搜索</a>
        </div>
        <%-- 报告审核页面 --%>
        <form id="reports_review_dialog">
            <%-- 检测记录信息 --%>
            <%--  <fieldset style="border: 1px solid #c3ddf4; height: 185px; margin-top: 3px">
                <legend style="font-weight: 600">检测记录信息</legend>--%>
            <%-- 记录数据 --%>
            <%--  <div id="record_ed"></div>
                <a href="#" id="open_data" class="easyui-linkbutton" data-options="iconCls:'icon-read',plain:true">打开检测记录</a>
            </fieldset>--%>
            <%-- 操作 --%>
            <fieldset style="border: 1px solid #c3ddf4; height: 115px; width: 600px; margin-top: 3px">
                <legend style="font-weight: 600">操作</legend>

                <%-- <div class="h_d" style="margin-top: 3px">
                    <label>选择组：</label><input id="group2" name="" class="easyui-combobox" style="width: 120px;" />
                    <label>报告审核人：</label><input id="search_man2" name="" class="easyui-combobox" style="width: 120px;" />
                    <a href="#" id="Transfer_review_task" class="easyui-linkbutton" data-options="iconCls:'icon-add',plain:true">转移审核任务</a>
                </div>
                <div id="Level_three_approval" style="display: none;float:left">
                    <label>选择组：</label><input id="group" name="" class="easyui-combobox" style="width: 120px;" />
                    <label>报告审核人：</label><input id="search_man" name="" class="easyui-combobox" style="width: 120px;" />
                    <a href="#" id="submit_review_Report" class="easyui-linkbutton" data-options="iconCls:'icon-ok',plain:true">提交报告签发</a>
                </div>
                <div id="Level_two_approval" style="display: none;float:left">
                    <a href="#" id="submit_Report" class="easyui-linkbutton" data-options="iconCls:'icon-ok',plain:true">报告完成</a>
                </div>--%>
            </fieldset>
        </form>
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
<%= WebExtensions.CombresLink("LosslessReport_EditApply_Audit_Js") %>
</html>
