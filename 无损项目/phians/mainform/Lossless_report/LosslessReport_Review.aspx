<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LosslessReport_Review.aspx.cs" Inherits="phians.mainform.Lossless_report.LosslessReport_Review" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <%=WebExtensions.CombresLink("public_Css") %>
</head>
<body>
    <div id="tt" class="easyui-panel" data-options="fit:true,border:false" style="overflow: scroll; overflow-x: hidden; overflow-y: hidden">
        <div id="report_review_task"></div>
    </div>

    <div style="display: none">
        <!-- 提交报告签发 -->
        <form id="submit_dialog" style="padding-top: 30px;">
            <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                <p class="h_d1" style="width: 60px; text-align: right;">审核级别:</p>
                <input id="level_Audit" class="easyui-combobox" style="width: 180px; height: 22px; margin-left: 5px" />
            </div>
            <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                <p class="h_d1" style="width: 60px; text-align: right;">时间:</p>
                <input id="level_date" name="level_date" class="easyui-datetimebox" style="width: 180px; height: 22px; margin-left: 5px" />
            </div>
            <%--            <div class="h_dline" style="height: 23px; margin-top: 20px; margin-left: 4px; float: left">
                <p class="h_d1" style="width: 60px">组:</p>
                <input id="group" name="" required="required" style="width: 150px; height: 22px; margin-left: 5px" />
            </div>--%>
            <%--            <div class="h_dline" style="height: 23px; margin-top: 20px; margin-left: 4px; float: left">
                <p class="h_d1" style="width: 80px">签发人员:</p>
                <input id="Issue_personnel" name="" required="required" class="easyui-combobox" style="width: 150px; height: 22px; margin-left: 5px" />
            </div>--%>
        </form>
        <!--<%-- 检测报告编制-报告信息-工具栏 --%>-->
        <div id="reports_toolbar">
            <span style="display: inline-block; float: left;">
                <label>管理选择：</label><a href="#" id="management_all" style="width: 100px" class="easyui-combobox"></a></span>
            <span class="link_button" style="display: inline-block; float: left;">
                <a href="#" id="Preview_Report" class="easyui-linkbutton" data-options="iconCls:'icon-Document',plain:true">预览报告</a>
                <span class="datagrid-btn-separator" style="vertical-align: middle; height: 22px; display: inline-block; float: none"></span>
            </span>
            <span class="link_button" style="display: inline-block; float: left;">
                <a href="#" id="read_report_info" class="easyui-linkbutton" data-options="iconCls:'icon-Document',plain:true">查看附件</a>
                <span class="datagrid-btn-separator" style="vertical-align: middle; height: 22px; display: inline-block; float: none"></span>
            </span>
            <span class="temp" style="display: inline-block; float: left;">
                <a href="#" id="read_Report" class="easyui-linkbutton" data-options="iconCls:'icon-Document',plain:true">阅读报告</a>
                <span class="datagrid-btn-separator" style="vertical-align: middle; height: 22px; display: inline-block; float: none"></span>
            </span>
            <span class="link_button" style="display: inline-block; float: left;">
                <a href="#" id="Back_report" class="easyui-linkbutton" data-options="iconCls:'icon-undo',plain:true">退回报告编制</a>
                <span class="datagrid-btn-separator" style="vertical-align: middle; height: 22px; display: inline-block; float: none"></span>
            </span>
            <span class="link_button" style="display: inline-block; float: left;">
            <a href="#" id="return_reason" class="easyui-linkbutton" data-options="iconCls:'icon-Document',plain:true">查看退回原因</a>
                <span class="datagrid-btn-separator" style="vertical-align: middle; height: 22px; display: inline-block; float: none"></span>
            </span>
            <span class="link_button" style="display: inline-block; float: left;">
                <a href="#" id="Submit_report" class="easyui-linkbutton" data-options="iconCls:'icon-ok',plain:true">审核确认</a>
                <span class="datagrid-btn-separator" style="vertical-align: middle; height: 22px; display: inline-block; float: none"></span>
            </span>
            <input id="search" style="width: 150px" class="easyui-combobox" />
            <input id="search1" style="width: 150px" class="easyui-textbox" />
            <a href="#" id="search_info" class="easyui-linkbutton" data-options="iconCls:'icon-search',plain:true">搜索</a>
        </div>
        <form id="Back_report_dialog" style="overflow-x: hidden;">
            <%--            <div class="h_dline" style="height: 23px; margin-top: 20px; margin-left: 10px; float: left;">
                <p class="h_d1" style="width: 80px">退回原因:</p>
                <input id="return_info" name="return_info" data-options="multiline:true" class="easyui-textbox" style="width: 240px; height: 66px;" />
            </div>--%>
            <div id="return_accessory">
                <div id="return_accessory_toolbar">
                    <a href="#" id="add_Picture" class="easyui-linkbutton" data-options="iconCls:'icon-remove',plain:true">添加文件</a>
                    <span class="datagrid-btn-separator" style="vertical-align: middle; height: 22px; display: inline-block; float: none"></span>
                    <a href="#" id="open_Picture" class="easyui-linkbutton" data-options="iconCls:'icon-add',plain:true">查看文件</a>
                    <span class="datagrid-btn-separator" style="vertical-align: middle; height: 22px; display: inline-block; float: none"></span>
                    <a href="#" id="del_Picture" class="easyui-linkbutton" data-options="iconCls:'icon-remove',plain:true">删除文件</a>
                </div>
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
                <%--报告退回dialog --%>
        <form id="Standard_equipment_info1">
            <!--<%-- 已选择报告原因显示 --%>-->
            <div id="Standard_verification1"></div>
            <div class="h_dline" style="height: 23px; margin-top: 10px; margin-left: 4px; float: left">
                <p class="h_d1" style="width: 80px; text-align: right;">退回原因:</p>
                <input id="return_info2" name="return_info" class="easyui-textbox" data-options="multiline:true" style="width: 600px; height: 44px; margin-left: 5px" />
            </div>
        </form>
        <!-- 添加文件 -->
        <form id="add_Picture_dialog" style="padding-top: 30px;" method="post" enctype="multipart/form-data">
            <div class="h_dline" style="height: 23px; margin-top: 20px; margin-left: 4px; float: left">
                <p class="h_d1" style="width: 80px; text-align: right;">上传文件：</p>
                <input id="file_add" name="Filedata" class="easyui-filebox" required="required" style="width: 300px; height: 22px; margin-left: 5px" />
            </div>
            <div class="h_dline" style="height: 23px; margin-top: 10px; margin-left: 4px; float: left">
                <p class="h_d1" style="width: 80px; text-align: right;">退回原因:</p>
                <input id="return_info" name="return_info" class="easyui-textbox" style="width: 300px; height: 22px; margin-left: 5px" />
            </div>
            <div class="h_dline" style="height: 23px; margin-top: 10px; margin-left: 4px; float: left">
                <p class="h_d1" style="width: 80px; text-align: right;">附注:</p>
                <input id="remarks" name="remarks" class="easyui-textbox" data-options="multiline:true" style="width: 300px; height: 66px; margin-left: 5px" />
            </div>
        </form>
        <form id="Picture_form">
            <img id="Picture_img" src="#" width="200" height="120" />
        </form>
        <%--报告退回dialog --%>
        <form id="Standard_equipment_info">
            <!--<%-- 已有报告原因显示 --%>-->
            <div class="easyui-panel" style="width: 710px; height: 410px; border: none; margin-top: 10px">
                <div class="h_d1" style="width: 310px; margin-left: 6px;">
                    <!--<%-- 已有报告原因--%>-->
                    <div style="width: 308px; height: 390px;" class="easyui-panel" title="待选退回原因 ">
                        <div id="Standard_Authorized"></div>
                    </div>
                </div>
                <div class="h_d1" style="margin-left: 15px;">
                    <div class="easyui-panel" style="width: 80px; height: 390px;">
                        <a style="width: 60px; margin-left: 10px; margin-top: 150px" id="Standard_add" href="#" class="easyui-linkbutton">添加</a>
                        <a style="width: 60px; margin-left: 10px; margin-top: 10px" id="Standard_remove" href="#" class="easyui-linkbutton ">移除</a>
                    </div>
                </div>
                <div class="h_d1" style="width: 270px; margin-left: 10px;">
                    <div class="easyui-panel" style="width: 270px; height: 390px;" title="已选择退回原因 ">
                        <div id="Standard_verification"></div>
                    </div>
                </div>
            </div>
            <div class="h_dline" style="height: 23px; margin-top: 10px; margin-left: 4px; float: left">
                <p class="h_d1" style="width: 80px; text-align: right;">退回原因:</p>
                <input id="return_info1" name="return_info" class="easyui-textbox" data-options="multiline:true" style="width: 600px; height: 44px; margin-left: 5px" />
            </div>
        </form>
    </div>
    <a href="<%=PageOffice.PageOfficeLink.OpenWindow("/mainform/Revisionsshow.aspx?save_type=Lossless_report_1&ReturnNode=ReviewUpdate&","width=1000px;height=740px;")%>" id="read_doc" class="easyui-linkbutton" data-options="iconCls:'icon-read',plain:true"><span id="read_doc_">阅读</span>></a>
    <a href="<%=PageOffice.PageOfficeLink.OpenWindow("/mainform/read_contract.aspx?","width=1000px ;height=740px;")%>" id="read_records" class="easyui-linkbutton" data-options="iconCls:'icon-read'" style="margin-left: 40px">查看检测记录</a>

</body>

<%=WebExtensions.CombresLink("LosslessReport_Review_Js") %>
</html>
