<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LosslessReport_Management.aspx.cs" Inherits="phians.mainform.Lossless_report.LosslessReport_Management1" %>

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
            <a href="#" id="download_report_pdf" class="easyui-linkbutton print_report" data-options="iconCls:'icon-print',plain:true">预览打印报告</a>
            <a href="#" id="report_print" class="easyui-linkbutton print_report" data-options="iconCls:'icon-print',plain:true">打印报告</a>
            <a href="#" id="Download_word" class="easyui-linkbutton" data-options="iconCls:'icon-downloads',plain:true">下载报告（Word版）</a>
            <span class="datagrid-btn-separator" style="vertical-align: middle; height: 22px; display: inline-block; float: none"></span>
            <a href="#" id="read_report_info" class="easyui-linkbutton" data-options="iconCls:'icon-Document',plain:true">查看附件</a>
            <%--    <span class="datagrid-btn-separator" style="vertical-align: middle; height: 22px; display: inline-block; float: none"></span>
            <a href="#" id="view_report" class="easyui-linkbutton view_report" data-options="iconCls:'icon-Document',plain:true">预览报告</a>
            <span class="datagrid-btn-separator" style="vertical-align: middle; height: 22px; display: inline-block; float: none"></span>--%>
            <span class="datagrid-btn-separator" style="vertical-align: middle; height: 22px; display: inline-block; float: none"></span>
            <a href="#" id="edit_apply" class="easyui-linkbutton" data-options="iconCls:'icon-edit',plain:true">修改报告申请</a>
            <span class="datagrid-btn-separator" style="vertical-align: middle; height: 22px; display: inline-block; float: none"></span>
            <a href="#" id="ScrapApply" class="easyui-linkbutton" data-options="iconCls:'icon-edit',plain:true">报废报告申请</a>


            <label>检索条件：</label>
            <input id="search3" style="width: 110px" class="easyui-combobox" />

            <span id="key_span3" style="display: inline-block">
                <input id="key3" style="width: 110px" class="easyui-textbox" /></span>
            <input id="search2" style="width: 110px" class="easyui-combobox" />
            <span id="key_span2" style="display: inline-block">
                <input id="key2" style="width: 110px" class="easyui-textbox" /></span>
            <input id="search1" style="width: 110px" class="easyui-combobox" />
            <span id="key_span1" style="display: inline-block">
                <input id="key1" style="width: 110px" class="easyui-textbox" /></span>
            <input id="search" style="width: 110px" class="easyui-combobox" />
            <span id="key_span" style="display: inline-block">
                <input id="key" style="width: 110px" class="easyui-textbox" /></span>
            <a href="#" id="report_search" class="easyui-linkbutton" data-options="iconCls:'icon-search',plain:true">搜索</a>
            <span class="datagrid-btn-separator" style="vertical-align: middle; height: 22px; display: inline-block; float: none"></span>
            <a href="#" id="report_arrange" class="easyui-linkbutton" data-options="iconCls:'icon-reload',plain:true">报告统计</a>
            <span class="datagrid-btn-separator" style="vertical-align: middle; height: 22px; display: inline-block; float: none"></span>
            <input id="condition1" type="checkbox" value="1" /><label class="required_report">搜索下载</label>
            <input id="condition2" type="checkbox" checked="checked" value="0" /><label class="no_report">选择下载</label>
            <input id="condition" name="condition" type="hidden" value="0" />
            <a href="#" id="BatchDownload" class="easyui-linkbutton" data-options="iconCls:'icon-downloads',plain:true">批量下载（PDF版）</a>
            <span class="datagrid-btn-separator" style="vertical-align: middle; height: 22px; display: inline-block; float: none"></span>
        </div>
    </div>
    <div style="display: none">
        <%-- 打印这份证书 --%>
        <a href="<%=PageOffice.PageOfficeLink.OpenWindow("/mainform/read_contract.aspx?","width=1000px ;height=740px;")%>" id="contract_read" class="easyui-linkbutton" data-options="iconCls:'icon-search'" style="margin-left: 40px;">查看合同</a>
        <a href="<%=PageOffice.PageOfficeLink.OpenWindow("/mainform/read_contract.aspx?","width=1000px ;height=740px;")%>" id="read_doc" class="easyui-linkbutton" data-options="iconCls:'icon-read',plain:true">阅读文件</a>
        <a href="<%=PageOffice.PageOfficeLink.OpenWindow("/mainform/Certificate_error_word_print.aspx?","width=1000px ;height=740px;")%>" id="Certificate_print" class="easyui-linkbutton" data-options="iconCls:'icon-read'" style="margin-left: 40px">打印证书</a>
        <%--修改申请--%>
        <form id="importFileForm1">
            <div class="h_dline" style="height: 23px; margin-top: 10px; margin-left: 0px; float: left">
                <p class="h_d1" style="width: 60px">组:</p>
                <input id="group" name="" class="easyui-combobox" style="width: 150px; height: 22px; margin-left: 5px" />
            </div>
            <div class="h_dline" style="height: 23px; margin-top: 10px; margin-left: 0px; float: left">
                <p class="h_d1" style="width: 60px">评审人:</p>
                <input id="review_personnel" name="review_personnel" class="easyui-combobox" style="width: 150px; height: 22px; margin-left: 5px" />
            </div>
            <div class="h_dline" style="height: 23px; margin-top: 8px; float: left">
                <p class="h_d1" style="width: 60px">申请原因:</p>
                <input id="error_remark1" name="error_remark" class="easyui-textbox" data-options="multiline:true" required="required" style="width: 400px; height: 44px; margin-left: 5px" />
            </div>

            <div class="h_dline" style="height: 23px; margin-top: 30px; float: left">
                <p class="h_d1" style="width: 60px">说明:</p>
                <input id="other_remarks1" name="other_remarks" class="easyui-textbox" data-options="multiline:true" style="width: 400px; height: 44px; margin-left: 5px" />
            </div>
        </form>
        <!-- 查看附件 -->
        <form id="read_dialog" style="overflow-x: hidden;">
            <!--<%--任务datagrid--%>-->
            <div id="read_table"></div>
            <div id="read_toolbar">
                <input id="read_search" style="width: 120px" class="easyui-combobox" />
                <input id="read_search1" style="width: 120px" class="easyui-textbox" />
                <a href="#" id="read_search_info" class="easyui-linkbutton" data-options="iconCls:'icon-search',plain:true">搜索</a>
                <span class="datagrid-btn-separator" style="vertical-align: middle; height: 22px; display: inline-block; float: none"></span>
                <a href="#" id="read_read" class="easyui-linkbutton" data-options="iconCls:'icon-Document',plain:true">查看附件</a>
                <span class="datagrid-btn-separator" style="vertical-align: middle; height: 22px; display: inline-block; float: none"></span>
                <a href="#" id="download_read" class="easyui-linkbutton" data-options="iconCls:'icon-upload',plain:true">下载附件</a>
                <span class="datagrid-btn-separator" style="vertical-align: middle; height: 22px; display: inline-block; float: none"></span>
                <a href="#" id="downloads_read" class="easyui-linkbutton" data-options="iconCls:'icon-upload',plain:true">批量下载附件</a>
            </div>
        </form>
    </div>
    <form id="Picture_form">
        <img id="Picture_img" src="#" width="200" height="120" />
    </form>
    <div id="report_arrangeMain" style="overflow-y: hidden;">
        <div style="position: absolute; z-index: 99999; left: 50px;">
            <div class="sTestSearch fl ml-10" style="height: 23px; margin-top: 10px; margin-left: 15px; float: left">
                <label class="all1" style="margin-left: 3px;">统计类型：</label>
                <input id="report_arrange_group" name="" class="easyui-combobox" style="width: 100px; height: 22px; margin-left: 5px;" />
                <span class="statistics" style="vertical-align: middle; height: 22px; display: inline-block; float: none"></span>
                <span id="group_personal" style="display: none;">
                    <label class="all1">班组：</label>
                    <input id="comboxgroup" name="review_personnel" style="width: 100px; height: 22px; margin-left: 5px;" />
                </span>
                <span id="personal" style="display: none;">
                    <label class="all1" style="margin-left: 3px;">人员：</label>
                    <input id="comboxperson" name="review_personnel" class="easyui-combobox" style="width: 100px; height: 22px; margin-left: 5px;" />
                </span>
                <span id="statistics_type" style="display: none;">
                    <span class="statistics" style="vertical-align: middle; height: 22px; display: inline-block; float: none"></span>
                    <input id="statisticsChk1" class="statistics" type="radio" value="0" checked="checked" name="statistics" /><label class="statistics">统计所有组</label>
                    <input id="statisticsChk2" class="statistics" type="radio" value="1" name="statistics" /><label class="statistics">统计所有人</label>
                </span>
                <span id="allTime" style="display: none;">
                    <label class="all1" style="margin-left: 3px;">耗时：</label>
                    <input id="allTimeH" name="allTime" class="easyui-numberbox" style="width: 60px; height: 22px; margin-left: 5px;" />天
                </span>
                <span class="statistics" style="vertical-align: middle; height: 22px; display: inline-block; float: none"></span>
            </div>
            <div class="sTestSearch fl ml-10" style="height: 23px; margin-top: 10px; margin-left: 36px; float: left">
                <label class="all1">检验日期：</label>
                <input id="dateStart" class="easyui-datebox" style="width: 102px" /><label style="margin-left: 12px;"> 到 </label>
                <p class="h_d1" style="width: 6px"></p>
                <input id="dateEnd" class="easyui-datebox" style="width: 102px; margin-left: 10px;" />
                <a id="date_select_" style="display: inline-block; width: 40px; height: 20px; margin-left: 5px; line-height: 20px; border-radius: 4px; text-align: center; background: #3398DB; text-align: center; color: #fff; cursor: pointer;">查询</a>
            </div>

        </div>


        <!-- 柱状图 -->
        <div id="container" style="width: 100%; height: 85%; top: 60px; margin-top: 15px;"></div>
        <!-- 饼状图 -->
        <div id="container1" style="width: 100%; height: 400px; top: 60px; display: none;"></div>
    </div>
</body>

<%=WebExtensions.CombresLink("LosslessReport_Management_Js") %>
</html>
