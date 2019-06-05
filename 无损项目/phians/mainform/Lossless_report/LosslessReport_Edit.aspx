<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LosslessReport_Edit.aspx.cs" Inherits="phians.mainform.Lossless_report.LosslessReport_Edit" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <%=WebExtensions.CombresLink("public_Css") %>
</head>
<body>
    <div id="tt" class="easyui-panel" data-options="fit:true,border:false" style="margin-left: 2px; overflow: scroll; overflow-x: hidden; overflow-y: hidden">
        <div id="report_edit_task">
            <!--<%-- 检测报告编制-报告信息-工具栏 --%>-->
            <div id="reports_toolbar">
                <span style="display: inline-block; float: left;">
                    <label>管理选择：</label><a href="#" id="management_all" style="width: 100px" class="easyui-combobox"></a>
                </span>
                <%--  <span class="history_link_button" style="display: inline-block; float: left;">
                    <a href="#" id="read_Report" class="easyui-linkbutton" data-options="iconCls:'icon-Document',plain:true">阅读报告</a>
                    <span class="datagrid-btn-separator" style="vertical-align: middle; height: 22px; display: inline-block; float: none"></span>
                </span>--%>
                <%-- <span class="link_button" style="display: inline-block; float: left;">
                    <a href="#" id="read_return_info" class="easyui-linkbutton" data-options="iconCls:'icon-Document',plain:true">查看退回信息</a>
                    <span class="datagrid-btn-separator" style="vertical-align: middle; height: 22px; display: inline-block; float: none"></span>
                </span>--%>
                <span class="link_button" style="display: inline-block; float: left;">
                    <a href="#" id="read_report_info" class="easyui-linkbutton" data-options="iconCls:'icon-Document',plain:true">查看附件</a>
                    <span class="datagrid-btn-separator" style="vertical-align: middle; height: 22px; display: inline-block; float: none"></span>
                </span>
                <span class="link_button" style="display: inline-block; float: left;">
                   <a href="#" id="download_info" class="easyui-linkbutton" data-options="iconCls:'icon-upload',plain:true">下载报告</a>
                    <span class="datagrid-btn-separator" style="vertical-align: middle; height: 22px; display: inline-block; float: none"></span>
                </span>
                <span class="link_button" style="display: inline-block; float: left;">
                    <a href="#" id="edit_info" class="easyui-linkbutton" data-options="iconCls:'icon-add',plain:true">添加报告</a>
                    <span class="datagrid-btn-separator" style="vertical-align: middle; height: 22px; display: inline-block; float: none"></span>
                </span>
                <span class="link_button" style="display: inline-block; float: left;">
                    <a href="#" id="edit_report_info" class="easyui-linkbutton" data-options="iconCls:'icon-edit',plain:true">修改报告信息</a>
                    <span class="datagrid-btn-separator" style="vertical-align: middle; height: 22px; display: inline-block; float: none"></span>
                </span>
                <span class="link_button" style="display: inline-block; float: left;">
                    <a href="#" id="DataDel" class="easyui-linkbutton" data-options="iconCls:'icon-remove',plain:true">删除信息</a>
                    <span class="datagrid-btn-separator" style="vertical-align: middle; height: 22px; display: inline-block; float: none"></span>
                </span>
                <span class="link_button" style="display: inline-block; float: left;">
                    <a href="#" id="WriteTestData" class="easyui-linkbutton" data-options="iconCls:'icon-add',plain:true">写入检测数据</a>
                    <span class="datagrid-btn-separator" style="vertical-align: middle; height: 22px; display: inline-block; float: none"></span>
                </span>
                <span class="link_button" style="display: inline-block; float: left;">
                    <a href="#" id="load_Report" class="easyui-linkbutton" data-options="iconCls:'icon-ok',plain:true">载入报告</a>
                    <span class="datagrid-btn-separator" style="vertical-align: middle; height: 22px; display: inline-block; float: none"></span>
                </span>
                <span class="link_button" style="display: inline-block; float: left;">
                    <a href="#" id="Edit_online_report" class="easyui-linkbutton" data-options="iconCls:'icon-edit',plain:true">检测报告在线编辑</a>
                    <span class="datagrid-btn-separator" style="vertical-align: middle; height: 22px; display: inline-block; float: none"></span>
                </span>
                <%-- <span class="link_button" style="display: inline-block; float: left;">
                    <a href="#" id="Delete_certificate_file" class="easyui-linkbutton" data-options="iconCls:'icon-remove',plain:true">删除报告</a>
                    <span class="datagrid-btn-separator" style="vertical-align: middle; height: 22px; display: inline-block; float: none"></span>
                </span>--%>
                <span class="link_button" style="display: inline-block; float: left;">
                    <a href="#" id="submit_review_Report" class="easyui-linkbutton" data-options="iconCls:'icon-ok',plain:true">提交报告审核</a>
                    <span class="datagrid-btn-separator" style="vertical-align: middle; height: 22px; display: inline-block; float: none"></span>
                </span>
                <%--                <span class="link_button" style="display: inline-block; float: left;">
                    <a href="#" id="AddAccessory" class="easyui-linkbutton" data-options="iconCls:'icon-ok',plain:true">附件添加到报告</a>
                    <span class="datagrid-btn-separator" style="vertical-align: middle; height: 22px; display: inline-block; float: none"></span>
                </span>--%>
                <span class="history_link_button" style="display: inline-block; float: left;">
                    <a href="#" id="Preview_Report" class="easyui-linkbutton" data-options="iconCls:'icon-Document',plain:true">预览报告</a>
                    <span class="datagrid-btn-separator" style="vertical-align: middle; height: 22px; display: inline-block; float: none"></span>
                </span>
                <a href="#" id="return_reason" class="easyui-linkbutton" data-options="iconCls:'icon-Document',plain:true">查看退回原因</a>
                <span class="datagrid-btn-separator" style="vertical-align: middle; height: 22px; display: inline-block; float: none"></span>
                <a href="#" id="read_document" class="easyui-linkbutton" data-options="iconCls:'icon-Document',plain:true">查看修改记录</a>
                <span class="datagrid-btn-separator" style="vertical-align: middle; height: 22px; display: inline-block; float: none"></span>
                <input id="search" style="width: 120px" class="easyui-combobox" />
                <input id="search1" style="width: 120px" class="easyui-textbox" />
                <a href="#" id="search_info" class="easyui-linkbutton" data-options="iconCls:'icon-search',plain:true">搜索</a>
            </div>

            <%--            <span class="link_button" style="display: inline-block; float: left;">
                <a href="#" id="AddRecord" class="easyui-linkbutton" data-options="iconCls:'icon-ok',plain:true">产生已提交的记录文档</a>
                <span class="datagrid-btn-separator" style="vertical-align: middle; height: 22px; display: inline-block; float: none"></span>
            </span>--%>
        </div>
    </div>
    <div style="display: none">
        <%--查看退回信息--%>
        <form id="Back_report_dialog" style="overflow-x: hidden;">
            <div id="return_accessory">
                <div id="return_accessory_toolbar">
                    <a href="#" id="open_Picture" class="easyui-linkbutton" data-options="iconCls:'icon-add',plain:true">查看图片</a>
                </div>
            </div>
        </form>
        <%--复制报告--%>
        <form id="copy_report_dialog" style="overflow-x: hidden;">
            <div id="copy_report_list">
            </div>
            <div id="copy_report_list_toolbar">
            <input id="Copy_search1" style="width: 110px" class="easyui-combobox" />
            <span id="key_span1" style="display:inline-block"><input id="Copy_key1" style="width: 110px" class="easyui-textbox" /></span>
            <input id="Copy_search" style="width: 110px" class="easyui-combobox" />
            <span id="key_span" style="display:inline-block"><input id="Copy_key" style="width: 110px" class="easyui-textbox" /></span>
                <a href="#" id="_search2" class="easyui-linkbutton" data-options="iconCls:'icon-search',plain:true">搜索</a>
            </div>
        </form>
        <%--查看图片--%>
        <form id="Picture_form">
            <img id="Picture_img" src="#" width="200" height="120" />
        </form>
        <!-- 查看附件 -->
        <form id="read_dialog" style="overflow-x: hidden;">
            <!--<%--任务datagrid--%>-->
            <div id="read_table"></div>
            <div id="read_toolbar">
                <a href="#" id="add_read" class="easyui-linkbutton" data-options="iconCls:'icon-add',plain:true">添加附件</a>
                <span class="datagrid-btn-separator" style="vertical-align: middle; height: 22px; display: inline-block; float: none"></span>
                <a href="#" id="del_read" class="easyui-linkbutton" data-options="iconCls:'icon-remove',plain:true">删除附件</a>
                 <span class="datagrid-btn-separator" style="vertical-align: middle; height: 22px; display: inline-block; float: none"></span>
                <a href="#" id="read_read" class="easyui-linkbutton" data-options="iconCls:'icon-Document',plain:true">查看附件</a>
                 <span class="datagrid-btn-separator" style="vertical-align: middle; height: 22px; display: inline-block; float: none"></span>
                <a href="#" id="download_read" class="easyui-linkbutton" data-options="iconCls:'icon-upload',plain:true">下载附件</a>
                  <span class="datagrid-btn-separator" style="vertical-align: middle; height: 22px; display: inline-block; float: none"></span>
                <a href="#" id="downloads_read" class="easyui-linkbutton" data-options="iconCls:'icon-upload',plain:true">批量下载附件</a>
            </div>
        </form>
        <!-- 查看信息 -->
        <form id="ReadDatagrid_dialog" style="overflow-x: hidden;">
            <div id="ReadDatagrid_info"></div>
            <div id="ReadDatagrid_toolbar">
                <input id="searchs" style="width: 150px" class="easyui-combobox" />
                <label>为 </label>
                <input id="key" style="width: 150px" class="easyui-textbox" />
                <a href="#" id="_search" class="easyui-linkbutton" data-options="iconCls:'icon-search',plain:true">搜索</a>
            </div>
        </form>
        <!-- 添加附件 -->
        <form id="add_read_dialog" style="padding-top: 30px;" method="post" enctype="multipart/form-data">
            <div class="h_dline" style="height: 23px; margin-top: 20px; margin-left: 4px; float: left">
                <p class="h_d1" style="width: 80px; text-align: right;">附件名称：</p>
                <input id="" name="accessory_name" class="easyui-textbox" style="width: 300px; height: 22px; margin-left: 5px" />
            </div>
            <div class="h_dline" style="height: 23px; margin-top: 20px; margin-left: 4px; float: left">
                <p class="h_d1" style="width: 80px; text-align: right;">上传文件：</p>
                <input id="file_add" name="Filedata" class="easyui-filebox" required="required" style="width: 300px; height: 22px; margin-left: 5px" />
            </div>
            <div class="h_dline" style="height: 23px; margin-top: 10px; margin-left: 4px; float: left">
                <p class="h_d1" style="width: 80px; text-align: right;">附注:</p>
                <input id="remarks1" name="remarks" class="easyui-textbox" data-options="multiline:true" style="width: 300px; height: 66px; margin-left: 5px" />
            </div>
        </form>
        <!-- 提交报告审核 -->
        <form id="submit_dialog" style="padding-top: 20px;">
            <div class="h_dline" style="height: 20px; margin-top: 10px; margin-left: 10px; float: left">
                <p class="h_d1" style="width: 60px; text-align: right;">组:</p>
                <input id="group" name="" required="required" style="width: 180px; height: 22px; margin-left: 5px" />
            </div>
            <div class="h_dline" style="height: 20px; margin-top: 10px; float: left; margin-left: 10px">
                <p class="h_d1" style="width: 60px; text-align: right;">检验级别:</p>
                <input id="level_Inspection"  class="easyui-combobox" style="width: 180px; height: 22px; margin-left: 5px" />
            </div>
              <div class="h_dline" style="height: 20px; margin-top: 10px; float: left; margin-left: 10px">
                <p class="h_d1" style="width: 60px; text-align: right;">时间:</p>
                 <input id="level_date" name="level_date" class="easyui-datetimebox" style="width: 180px; height: 22px; margin-left: 5px" data-options="disabled: true,required: true"/>
            </div>
        </form>
        <!-- 写入报告信息 -->
        <form id="add_reports_info" style="overflow-x: hidden;">
            <input type="hidden" name="" value="" id="copy_id" />
            <div class="fitem test">
                <div class="h_dline play_1" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                    <p class="h_d1" style="width: 100px; text-align: right;">报告编号:</p>
                    <input type="text" id="report_num" name="report_num" required="required" class="easyui-textbox" style="width: 180px; height: 22px; margin-left: 5px" />
                </div>
            </div>
            <div class="h_dline play_2" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px;">
                <p class="h_d1" style="width: 100px; text-align: right;">报告模板:</p>
                <input id="TemplateChoose1" name="TemplateChoose1" required="required" class="easyui-combobox" style="width: 180px; height: 22px; margin-left: 5px" />
            </div>
          
            <div class="h_dline play_3" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px; display: block;">
                <p class="h_d1" style="width: 100px; text-align: right;">用户:</p>
                <input id="clientele_department" name="clientele_department" class="easyui-textbox" style="width: 180px; height: 22px; margin-left: 5px" />
            </div>
              <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px; display: block;">
                <p class="h_d1" style="width: 100px; text-align: right;">工号:</p>
                <input id="Job_num" name="Job_num" class="easyui-textbox" style="width: 180px; height: 22px; margin-left: 5px" />
            </div>
            <div class="h_dline play_4" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px; display: none;">
                <p class="h_d1" style="width: 100px; text-align: right;">委托人:</p>
                <input id="clientele" name="clientele" class="easyui-textbox" style="width: 180px; height: 22px; margin-left: 5px" />
            </div>

            <div class="h_dline " style="height: 23px; margin-top: 10px; float: left; margin-left: 10px; display: block;">
                <p class="h_d1" style="width: 100px; text-align: right; color: green;">流转卡号/ST:</p>
                <input id="circulation_NO" name="circulation_NO" class="easyui-textbox" style="width: 180px; height: 22px; margin-left: 5px" />
            </div>
            <div class="h_dline " style="height: 23px; margin-top: 10px; float: left; margin-left: 10px; display: block;">
                <p class="h_d1" style="width: 100px; text-align: right; color: green;">工序号:</p>
                <input id="procedure_NO" name="procedure_NO" class="easyui-textbox" style="width: 150px; height: 22px; margin-left: 5px" />
                <%--<a href="#" id="process" class="easyui-linkbutton" data-options="iconCls:'icon-add',plain:false">点击</a>--%>
                <a href="#" id="ReadDatagrid" class="easyui-linkbutton" data-options="iconCls:'icon-add',plain:false">MES信息</a>
            </div>
            <div class="h_dline " style="height: 23px; margin-top: 10px; float: left; margin-left: 10px; display: block;">
                <p class="h_d1" style="width: 100px; text-align: right; color: #f00;">订单号:</p>
                <input id="application_num" name="application_num" class="easyui-textbox" style="width: 180px; height: 22px; margin-left: 5px" />
            </div>
            <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px; display: block;">
                <p class="h_d1" style="width: 100px; text-align: right; color: #f00;">部件名称:</p>
                <input id="Subassembly_name" name="Subassembly_name" class="easyui-textbox" style="width: 180px; height: 22px; margin-left: 5px" />
            </div>
            <div class="h_dline " style="height: 23px; margin-top: 10px; float: left; margin-left: 10px; display: block;">
                <p class="h_d1" style="width: 100px; text-align: right; color: #f00;">检验内容:</p>
                <input id="Inspection_context" name="Inspection_context" class="easyui-textbox" style="width: 180px; height: 22px; margin-left: 5px" />
            </div>

            <div class="h_dline play_6" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px; display: block;">
                <p class="h_d1" style="width: 100px; text-align: right;">项目名称:</p>
                <input id="Project_name" name="Project_name" class="easyui-textbox" style="width: 180px; height: 22px; margin-left: 5px" />
            </div>
            <div class="h_dline play_9" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px; display: block;">
                <p class="h_d1" style="width: 100px; text-align: right;">材质:</p>
                <input id="Material" name="Material" class="easyui-textbox" style="width: 180px; height: 22px; margin-left: 5px" />
            </div>
            <div class="h_dline play_10" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px; display: block;">
                <p class="h_d1" style="width: 100px; text-align: right;">规格/型号:</p>
                <input id="Type_" name="Type_" class="easyui-textbox" style="width: 180px; height: 22px; margin-left: 5px" />
            </div>
            <div class="h_dline play_11" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px; display: block;">
                <p class="h_d1" style="width: 100px; text-align: right;">坡口型式:</p>
                <input id="Chamfer_type" name="Chamfer_type" class="easyui-textbox" style="width: 180px; height: 22px; margin-left: 5px" />
            </div>
            <div class="h_dline play_12" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px; display: block;">
                <p class="h_d1" style="width: 100px; text-align: right;">图号:</p>
                <input id="Drawing_num" name="Drawing_num" class="easyui-textbox" style="width: 180px; height: 22px; margin-left: 5px" />
            </div>
            <div class="h_dline play_13" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px; display: block;">
                <p class="h_d1" style="width: 100px; text-align: right;">检验规程:</p>
                <input id="Procedure_" name="Procedure_" class="easyui-textbox" style="width: 180px; height: 22px; margin-left: 5px" />
            </div>

            <div class="h_dline play_15" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px; display: block;">
                <p class="h_d1" style="width: 100px; text-align: right;">检验时机:</p>
                <input id="Inspection_opportunity" name="Inspection_opportunity" class="easyui-textbox" style="width: 180px; height: 22px; margin-left: 5px" />
            </div>

            <div class="h_dline play_17" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px; display: block;">
                <p class="h_d1" style="width: 100px; text-align: right;">表面状态:</p>
                <input id="apparent_condition" name="apparent_condition" class="easyui-textbox" style="width: 180px; height: 22px; margin-left: 5px" />
            </div>
            <div class="h_dline play_18" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px; display: none;">
                <p class="h_d1" style="width: 100px; text-align: right;">制造工艺:</p>
                <input id="manufacturing_process" name="manufacturing_process" class="easyui-textbox" style="width: 180px; height: 22px; margin-left: 5px" />
            </div>
            <div class="h_dline play_19" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px; display: none;">
                <p class="h_d1" style="width: 100px; text-align: right;">批次号:</p>
                <input id="Batch_Num" name="Batch_Num" class="easyui-textbox" style="width: 180px; height: 22px; margin-left: 5px" />
            </div>
            <div class="h_dline play_20" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px; display: none;">
                <p class="h_d1" style="width: 100px; text-align: right;">附图:</p>
                <input id="figure" name="figure" class="easyui-combobox" style="width: 180px; height: 22px; margin-left: 5px" />
            </div>
            <div class="h_dline play_21" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px; display: none;">
                <p class="h_d1" style="width: 100px; text-align: right;">不符合项报告号:</p>
                <input id="disable_report_num" name="disable_report_num" class="easyui-textbox" style="width: 180px; height: 22px; margin-left: 5px" />
            </div>
            <div class="h_dline play_22" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px; display: none;">
                <p class="h_d1" style="width: 100px; text-align: right;">管子数量:</p>
                <input id="Tubes_num" name="Tubes_num" class="easyui-textbox" style="width: 180px; height: 22px; margin-left: 5px" />
            </div>
            <div class="h_dline play_23" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px; display: none;">
                <p class="h_d1" style="width: 100px; text-align: right;">管子规格:</p>
                <input id="Tubes_Size" name="Tubes_Size" class="easyui-textbox" style="width: 180px; height: 22px; margin-left: 5px" />
            </div>
            <div class="h_dline play_24" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px; display: none;">
                <p class="h_d1" style="width: 100px; text-align: right;">焊接方法:</p>
                <input id="welding_method" name="welding_method" class="easyui-textbox" style="width: 180px; height: 22px; margin-left: 5px" />
            </div>
            <div class="h_dline play_25" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px;">
                <p class="h_d1" style="width: 100px; text-align: right;">检验结果:</p>
                <input id="Inspection_result" name="Inspection_result" class="easyui-combobox" style="width: 180px; height: 22px; margin-left: 5px" />
            </div>
            <div class="h_dline play_26" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px; display: none;">
                <p class="h_d1" style="width: 100px; text-align: right;">检验编号:</p>
                <input id="Inspection_NO" name="Inspection_NO" class="easyui-textbox" style="width: 180px; height: 22px; margin-left: 5px" />
            </div>
            <div class="h_dline play_27" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px;">
                <p class="h_d1" style="width: 100px; text-align: right;">检验日期:</p>
                <input id="Inspection_date" name="Inspection_date" required="required" class="easyui-datebox" style="width: 180px; height: 22px; margin-left: 5px" />
            </div>
            <div class="h_dline play_28" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px; display: none;">
                <p class="h_d1" style="width: 100px; text-align: right;">作业指导书:</p>
                <input id="Work_instruction" name="Work_instruction" class="easyui-textbox" style="width: 180px; height: 22px; margin-left: 5px" />
            </div>
            <div class="h_dline play_29" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px; display: none;">
                <p class="h_d1" style="width: 100px; text-align: right;">热处理设备:</p>
                <input id="heat_treatment" name="heat_treatment" class="easyui-textbox" style="width: 180px; height: 22px; margin-left: 5px" />
            </div>

            <%--            <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 50px;width:100%;">
                <span class="link_button" style="display: inline-block; float: left;">
                    <a href="#" id="view_word_temp" class="easyui-linkbutton" data-options="iconCls:'icon-Document',plain:true">预览报告模板</a>
                    <span class="datagrid-btn-separator" style="vertical-align: middle; height: 22px; display: inline-block; float: none"></span>
                </span>
            </div>--%>
            <div class="h_dline" id="copy_add" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px; display: none;">
                <a href="#" id="copy_info" class="easyui-linkbutton" data-options="iconCls:'icon-add',plain:true">选择复制报告</a>
            </div>
            <div class="" style="float: left; margin-top: 10px; margin-left: 10px">
                <input id="copy_flag1" type="checkbox" value="1" /><label style="color: #f00;">复制</label>
                <input id="copy_flag2" type="checkbox" value="0" checked="checked" /><label style="color: #f00;">不复制</label>
                <input type="hidden" name="flag" value="0" id="copy_flag" />
            </div>
            <div class="h_dline" style="height: 23px; margin-top: 10px; margin-left: 10px; float: left">
                <p class="h_d1" style="width: 100px; text-align: right;">附注:</p>
                <input id="remarks" name="remarks" class="easyui-textbox" data-options="multiline:true" style="width: 400px; height: 66px; margin-left: 5px" />
            </div>
        </form>
        <%-- 探头dialog --%>
        <form id="Probe_info">
            <!--<%-- 试验显示 --%>-->
            <div class="easyui-panel" style="width: 880px; height: 410px; border: none; margin-top: 10px">
                <div class="h_d1" style="width: 470px; margin-left: 6px;">
                    <!--<%-- 已选试验--%>-->
                    <div style="width: 470px; height: 390px;" class="easyui-panel" title="探头">
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
                    <div class="easyui-panel" style="width: 270px; height: 390px;" title="已选择探头 ">
                        <div id="Standard_verification"></div>
                    </div>
                </div>
            </div>
        </form>
        <%--报告退回dialog --%>
        <form id="Standard_equipment_info">
            <!--<%-- 已选择报告原因显示 --%>-->
            <div id="Standard_verification1"></div>
            <div class="h_dline" style="height: 23px; margin-top: 10px; margin-left: 4px; float: left">
                <p class="h_d1" style="width: 80px; text-align: right;">退回原因:</p>
                <input id="return_info1" name="return_info" class="easyui-textbox" data-options="multiline:true" style="width: 600px; height: 44px; margin-left: 5px" />
            </div>
        </form>
        <%--记录文档dialog --%>
        <form id="ReadRecord_info">
            <div id="ReadRecord"></div>
            <div id="ReadRecord_toolbar">
                <a href="#" id="ReadRecord_Preview_Report" class="easyui-linkbutton" data-options="iconCls:'icon-Document',plain:true">预览报告</a>
                <span class="datagrid-btn-separator" style="vertical-align: middle; height: 22px; display: inline-block; float: none"></span>
            </div>
        </form>
        <%-- 4 ECT-涡流检验报告模板RPV --%>
        <form id="form_4">
            <fieldset style="border: 1px solid #c3ddf4; margin-top: 3px;">
                <legend style="font-weight: 600">检验条件</legend>
                <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                    <input type="hidden" value="数据采集系统,扫查器,运动控制器" name="equipment_name_R" />
                    <p class="h_d1">数据采集系统:</p>
                    <input class="easyui-combobox Daq_System Device_test" style="width: 180px; height: 22px; margin-left: 5px" />
                </div>
                <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                    <p class="h_d1">扫查器:</p>
                    <input class="easyui-combobox Scanner Device_test" style="width: 180px; height: 22px; margin-left: 5px" />
                </div>
                <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                    <p class="h_d1">运动控制器:</p>
                    <input class="easyui-combobox Motion_Controller Device_test" style="width: 180px; height: 22px; margin-left: 5px" />
                </div>
                <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                    <a href="#" class="easyui-linkbutton Probe_test" data-options="iconCls:'icon-add',plain:true">添加探头</a>
                </div>
            </fieldset>
            <fieldset style="border: 1px solid #c3ddf4; margin-top: 3px;">
                <legend style="font-weight: 600">制造工艺</legend>
                <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                    <p class="h_d1">
                        <input name="Data1" type="checkbox" value="1" />锻
                          <input name="Data1" type="hidden" value="flag" />
                    </p>

                </div>
                <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                    <p class="h_d1">
                        <input name="Data2" type="checkbox" value="1" />轧
                          <input name="Data2" type="hidden" value="flag" />
                    </p>
                </div>
                <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                    <p class="h_d1">
                        <input name="Data3" type="checkbox" value="1" />铸
                          <input name="Data3" type="hidden" value="flag" />
                    </p>
                </div>
                <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                    <p class="h_d1">
                        <input name="Data4" type="checkbox" value="1" />焊
                          <input name="Data4" type="hidden" value="flag" />
                    </p>
                </div>
                <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                    <p class="h_d1">其他</p>
                    <input name="Data5" class="easyui-textbox" style="width: 180px; height: 22px; margin-left: 5px" />
                </div>
                <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                    <p class="h_d1">焊接方法</p>
                    <input name="Data6" class="easyui-textbox" style="width: 180px; height: 22px; margin-left: 5px" />
                </div>
                <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                    <p class="h_d1">耦合剂类型</p>
                    <input name="Data7" class="easyui-textbox" style="width: 180px; height: 22px; margin-left: 5px" />
                </div>
            </fieldset>
        </form>
        <%-- 5 ECT-涡流检验报告模板SG --%>
        <form id="form_5">
            <fieldset style="border: 1px solid #c3ddf4; margin-top: 3px;">
                <legend style="font-weight: 600">检验条件</legend>
                <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                    <input type="hidden" value="涡流仪,标定样管" name="equipment_name_R" />
                    <p class="h_d1">涡流仪:</p>
                    <input class="easyui-combobox Daq_System Device_test" style="width: 180px; height: 22px; margin-left: 5px" />
                </div>
                <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                    <p class="h_d1">标定样管:</p>
                    <input class="easyui-combobox Scanner Device_test" style="width: 180px; height: 22px; margin-left: 5px" />
                </div>
                <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                    <a href="#" class="easyui-linkbutton Probe_test" data-options="iconCls:'icon-add',plain:true">添加探头</a>
                </div>
            </fieldset>
            <fieldset style="border: 1px solid #c3ddf4; margin-top: 3px;">
                <legend style="font-weight: 600">检验条件</legend>
                <fieldset style="border: 1px solid #c3ddf4; margin-top: 3px;">
                    <legend style="font-weight: 600">探头移动方式</legend>
                    <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                        <p class="h_d1">
                            <input name="Data1" type="checkbox" value="1" />手动
                          <input name="Data1" type="hidden" value="flag" />
                        </p>
                    </div>
                    <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                        <p class="h_d1">
                            <input name="Data2" type="checkbox" value="1" />自动
                          <input name="Data2" type="hidden" value="flag" />
                        </p>
                    </div>
                </fieldset>
                <fieldset style="border: 1px solid #c3ddf4; margin-top: 3px;">
                    <legend style="font-weight: 600">记录方向</legend>
                    <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                        <p class="h_d1">
                            <input name="Data3" type="checkbox" value="1" />前进
                          <input name="Data3" type="hidden" value="flag" />
                        </p>
                    </div>
                    <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                        <p class="h_d1">
                            <input name="Data4" type="checkbox" value="1" />后退
                          <input name="Data4" type="hidden" value="flag" />
                        </p>
                    </div>
                </fieldset>
                <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                    <p class="h_d1">检测速率:</p>
                    <input name="Data5" class="easyui-textbox" style="width: 180px; height: 22px; margin-left: 5px" />
                </div>
                <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                    <p class="h_d1">采样率:</p>
                    <input name="Data6" class="easyui-textbox" style="width: 180px; height: 22px; margin-left: 5px" />
                </div>
                <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                    <p class="h_d1">激励电压:</p>
                    <input name="Data7" class="easyui-textbox" style="width: 180px; height: 22px; margin-left: 5px" />
                </div>
            </fieldset>
            <fieldset style="border: 1px solid #c3ddf4; margin-top: 3px;">
                <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                    <p class="h_d1">
                        <input name="Data8" type="checkbox" value="1" />热端
                          <input name="Data8" type="hidden" value="flag" />
                    </p>
                </div>
                <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                    <p class="h_d1">
                        <input name="Data9" type="checkbox" value="1" />冷端
                          <input name="Data9" type="hidden" value="flag" />
                    </p>
                </div>
                <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                    <p class="h_d1">数据记录:</p>
                    <input name="Data10" class="easyui-textbox" style="width: 180px; height: 22px; margin-left: 5px" />
                </div>
                <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                    <p class="h_d1">周期校验:</p>
                    <input name="Data11" class="easyui-textbox" style="width: 180px; height: 22px; margin-left: 5px" />
                </div>
            </fieldset>
            <fieldset style="border: 1px solid #c3ddf4; margin-top: 3px;">
                <legend style="font-weight: 600">检验设备</legend>
                <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                    <p class="h_d1">标定样管:</p>
                    <input name="Data12" class="easyui-textbox" style="width: 180px; height: 22px; margin-left: 5px" />
                </div>
                <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                    <p class="h_d1">样管编号:</p>
                    <input name="Data13" class="easyui-textbox" style="width: 180px; height: 22px; margin-left: 5px" />
                </div>
                <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                    <p class="h_d1">软件及版本:</p>
                    <input name="Data14" class="easyui-textbox" style="width: 180px; height: 22px; margin-left: 5px" />
                </div>
            </fieldset>
        </form>
        <%-- 6 LT-氦检漏报告模板 4版123 --%>
        <form id="form_6" style="overflow-x: hidden;">
            <fieldset style="border: 1px solid #c3ddf4; margin-top: 3px;">
                <legend style="font-weight: 600">检验条件</legend>
                <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                    <input type="hidden" value="检测仪器,真空计,氦浓度仪,湿度计,温度计,压力表" name="equipment_name_R" />
                    <p class="h_d1">检测仪器:</p>
                    <input class="easyui-combobox Daq_System Device_test" style="width: 180px; height: 22px; margin-left: 5px" />
                </div>
                <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                    <p class="h_d1">真空计:</p>
                    <input class="easyui-combobox Scanner Device_test" style="width: 180px; height: 22px; margin-left: 5px" />
                </div>
                <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                    <p class="h_d1">氦浓度仪:</p>
                    <input class="easyui-combobox Scanner Device_test" style="width: 180px; height: 22px; margin-left: 5px" />
                </div>
                <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                    <p class="h_d1">湿度计:</p>
                    <input class="easyui-combobox Scanner Device_test" style="width: 180px; height: 22px; margin-left: 5px" />
                </div>
                <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                    <p class="h_d1">温度计:</p>
                    <input class="easyui-combobox Scanner Device_test" style="width: 180px; height: 22px; margin-left: 5px" />
                </div>
                <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                    <p class="h_d1">压力表:</p>
                    <input class="easyui-combobox Scanner Device_test" style="width: 180px; height: 22px; margin-left: 5px" />
                </div>
                <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                    <a href="#" class="easyui-linkbutton Probe_test" data-options="iconCls:'icon-add',plain:true">添加探头</a>
                </div>
            </fieldset>
            <fieldset style="border: 1px solid #c3ddf4; margin-top: 3px;">
                <legend style="font-weight: 600">表面准备</legend>
                <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                    <p class="h_d1">
                        <input name="Data1" type="checkbox" value="1" />焊态
                          <input name="Data1" type="hidden" value="flag" />
                    </p>
                </div>
                <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                    <p class="h_d1">
                        <input name="Data2" type="checkbox" value="1" />机加
                          <input name="Data2" type="hidden" value="flag" />
                    </p>
                </div>
                <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                    <p class="h_d1">其它</p>
                    <input name="Data3" class="easyui-textbox" style="width: 180px; height: 22px; margin-left: 5px" />
                </div>
                <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                    <p class="h_d1">检验方法:</p>
                    <input name="Data4" class="easyui-textbox" style="width: 180px; height: 22px; margin-left: 5px" />
                </div>
                <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                    <p class="h_d1">清洁方法:</p>
                    <input name="Data42" class="easyui-textbox" style="width: 180px; height: 22px; margin-left: 5px" />
                </div>
            </fieldset>
            <fieldset style="border: 1px solid #c3ddf4; margin-top: 3px;">
                <legend style="font-weight: 600">检验方法</legend>
                <fieldset style="border: 1px solid #c3ddf4; margin-top: 3px; width: 345px; float: left;">
                    <legend style="font-weight: 600">气泡法</legend>
                    <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                        <p class="h_d1">
                            <input name="Data29" type="checkbox" value="1" />气泡法
                          <input name="Data29" type="hidden" value="flag" />
                        </p>
                    </div>
                    <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                        <p class="h_d1">
                            <input name="Data30" type="checkbox" value="1" />有压
                          <input name="Data30" type="hidden" value="flag" />
                        </p>
                    </div>
                    <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                        <p class="h_d1">
                            <input name="Data31" type="checkbox" value="1" />真空
                          <input name="Data31" type="hidden" value="flag" />
                        </p>
                    </div>
                </fieldset>
                <fieldset style="border: 1px solid #c3ddf4; margin-top: 3px; width: 345px; float: left;">
                    <legend style="font-weight: 600">压力变化</legend>
                    <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                        <p class="h_d1">
                            <input name="Data32" type="checkbox" value="1" />压力变化
                          <input name="Data32" type="hidden" value="flag" />
                        </p>
                    </div>
                    <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                        <p class="h_d1">
                            <input name="Data33" type="checkbox" value="1" />有压
                          <input name="Data33" type="hidden" value="flag" />
                        </p>
                    </div>
                    <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                        <p class="h_d1">
                            <input name="Data34" type="checkbox" value="1" />真空
                          <input name="Data34" type="hidden" value="flag" />
                        </p>
                    </div>
                </fieldset>
                <fieldset style="border: 1px solid #c3ddf4; margin-top: 3px; width: 345px; float: left;">
                    <legend style="font-weight: 600">卤素</legend>
                    <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                        <p class="h_d1">
                            <input name="Data35" type="checkbox" value="1" />卤素
                          <input name="Data35" type="hidden" value="flag" />
                        </p>
                    </div>

                    <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                        <p class="h_d1">
                            <input name="Data36" type="checkbox" value="1" />护罩检测
                          <input name="Data36" type="hidden" value="flag" />
                        </p>
                    </div>
                    <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                        <p class="h_d1">
                            <input name="Data37" type="checkbox" value="1" />嗅探器
                          <input name="Data37" type="hidden" value="flag" />
                        </p>
                    </div>
                </fieldset>
                <fieldset style="border: 1px solid #c3ddf4; margin-top: 3px; width: 345px; float: left;">
                    <legend style="font-weight: 600">氨</legend>
                    <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                        <p class="h_d1">
                            <input name="Data38" type="checkbox" value="1" />氨
                          <input name="Data38" type="hidden" value="flag" />
                        </p>
                    </div>
                    <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                        <p class="h_d1">
                            <input name="Data39" type="checkbox" value="1" />氨-有压
                          <input name="Data39" type="hidden" value="flag" />
                        </p>
                    </div>
                    <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                        <p class="h_d1">
                            <input name="Data40" type="checkbox" value="1" />氨-密封设备
                          <input name="Data40" type="hidden" value="flag" />
                        </p>
                    </div>
                </fieldset>
                <fieldset style="border: 1px solid #c3ddf4; margin-top: 3px;">
                    <legend style="font-weight: 600">氦</legend>
                    <div class="h_dline" style="height: 23px; float: left; margin-left: 10px">
                        <p class="h_d1">
                            <input name="Data41" type="checkbox" value="1" />氦
                          <input name="Data41" type="hidden" value="flag" />
                        </p>
                    </div>
                    <div class="h_dline" style="height: 23px; float: left; margin-left: 10px">
                        <p class="h_d1">
                            <input name="Data5" type="checkbox" value="1" />全真空
                          <input name="Data5" type="hidden" value="flag" />
                        </p>
                    </div>
                    <div class="h_dline" style="height: 23px; float: left; margin-left: 10px">
                        <p class="h_d1">
                            <input name="Data6" type="checkbox" value="1" />部分真空
                          <input name="Data6" type="hidden" value="flag" />
                        </p>
                    </div>
                    <div class="h_dline" style="height: 23px; float: left; margin-left: 10px">
                        <p class="h_d1">
                            <input name="Data7" type="checkbox" value="1" />真空下喷射
                          <input name="Data7" type="hidden" value="flag" />
                        </p>
                    </div>
                    <div class="h_dline" style="height: 23px; float: left; margin-left: 10px">
                        <p class="h_d1">
                            <input name="Data8" type="checkbox" value="1" />真空氦罩法
                          <input name="Data8" type="hidden" value="flag" />
                        </p>
                    </div>
                    <div class="h_dline" style="height: 23px; float: left; margin-left: 10px">
                        <p class="h_d1">
                            <input name="Data9" type="checkbox" value="1" />累积量
                          <input name="Data9" type="hidden" value="flag" />
                        </p>
                    </div>
                    <div class="h_dline" style="height: 23px; float: left; margin-left: 10px">
                        <p class="h_d1">
                            <input name="Data10" type="checkbox" value="1" />气体渗透
                          <input name="Data10" type="hidden" value="flag" />
                        </p>
                    </div>
                    <div class="h_dline" style="height: 23px; float: left; margin-left: 10px">
                        <p class="h_d1">
                            <input name="Data11" type="checkbox" value="1" />嗅探器
                          <input name="Data11" type="hidden" value="flag" />
                        </p>
                    </div>
                    <div class="h_dline" style="height: 23px; float: left; margin-left: 10px">
                        <p class="h_d1">
                            <input name="Data12" type="checkbox" value="1" />真空吸盘法
                          <input name="Data12" type="hidden" value="flag" />
                        </p>
                    </div>
                </fieldset>
            </fieldset>
            <fieldset style="border: 1px solid #c3ddf4; margin-top: 3px;">
                <legend style="font-weight: 600">检验设备</legend>
                <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                    <p class="h_d1" style="width: 120px; text-align: right;">示踪气体:</p>
                    <input name="Data13" class="easyui-textbox" style="width: 160px; height: 22px; margin-left: 5px" />
                </div>
                <fieldset style="border: 1px solid #c3ddf4; margin-top: 3px; width: 100%; overflow-x: hidden;">
                    <legend style="font-weight: 600">标准泄漏试样</legend>
                    <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                        <p class="h_d1" style="width: 120px; text-align: right;">编号1:</p>
                        <input name="Data14" class="easyui-textbox" style="width: 160px; height: 22px; margin-left: 5px" />
                    </div>
                    <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                        <p class="h_d1" style="width: 120px; text-align: right;">校正的泄漏率:</p>
                        <input name="Data15" class="easyui-textbox" style="width: 160px; height: 22px; margin-left: 5px" />
                    </div>
                    <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                        <p class="h_d1" style="width: 120px; text-align: right;">编号2:</p>
                        <input name="Data16" class="easyui-textbox" style="width: 160px; height: 22px; margin-left: 5px" />
                    </div>
                    <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                        <p class="h_d1" style="width: 120px; text-align: right;">校正的泄漏率:</p>
                        <input name="Data17" class="easyui-textbox" style="width: 160px; height: 22px; margin-left: 5px" />
                    </div>
                </fieldset>
            </fieldset>
            <fieldset style="border: 1px solid #c3ddf4; margin-top: 3px;">
                <legend style="font-weight: 600">检验条件</legend>
                <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                    <p class="h_d1">注入示踪气体前的真空度:</p>
                    <input name="Data18" class="easyui-textbox" style="width: 160px; height: 22px; margin-left: 5px" />
                </div>
                <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                    <p class="h_d1" style="width: 100px; text-align: right;">持续时间:</p>
                    <input name="Data19" class="easyui-textbox" style="width: 160px; height: 22px; margin-left: 5px" />
                </div>
                <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                    <p class="h_d1" style="width: 120px; text-align: right;">温度:</p>
                    <input name="Data20" class="easyui-textbox" style="width: 160px; height: 22px; margin-left: 5px" />
                </div>
                <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                    <p class="h_d1" style="width: 120px; text-align: right;">湿度:</p>
                    <input name="Data21" class="easyui-textbox" style="width: 160px; height: 22px; margin-left: 5px" />
                </div>
                <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                    <p class="h_d1" style="width: 120px; text-align: right;">灵敏度:</p>
                    <input name="Data22" class="easyui-textbox" style="width: 160px; height: 22px; margin-left: 5px" />
                </div>
                <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                    <p class="h_d1" style="width: 120px; text-align: right;">浓度:</p>
                    <input name="Data23" class="easyui-textbox" style="width: 160px; height: 22px; margin-left: 5px" />
                </div>
                <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                    <p class="h_d1" style="width: 120px; text-align: right;">测试周期:</p>
                    <input name="Data24" class="easyui-textbox" style="width: 160px; height: 22px; margin-left: 5px" />
                </div>
                <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                    <p class="h_d1" style="width: 120px; text-align: right;">真空度:</p>
                    <input name="Data25" class="easyui-textbox" style="width: 160px; height: 22px; margin-left: 5px" />
                </div>
                <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                    <p class="h_d1" style="width: 120px; text-align: right;">检测压力:</p>
                    <input name="Data26" class="easyui-textbox" style="width: 160px; height: 22px; margin-left: 5px" />
                </div>
                <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                    <p class="h_d1" style="width: 120px; text-align: right;">测试周期:</p>
                    <input name="Data27" class="easyui-textbox" style="width: 160px; height: 22px; margin-left: 5px" />
                </div>
                <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                    <p class="h_d1" style="width: 120px; text-align: right;">检测时最小保持时间:</p>
                    <input name="Data28" class="easyui-textbox" style="width: 160px; height: 22px; margin-left: 5px" />
                </div>
            </fieldset>
        </form>
        <%-- 7 MT-磁轭法和触头法磁粉检验报告3版123 --%>
        <form id="form_7">
            <fieldset style="border: 1px solid #c3ddf4; margin-top: 3px;">
                <legend style="font-weight: 600">检验条件</legend>
                <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                    <input type="hidden" value="使用设备,测温仪,照度计" name="equipment_name_R" />
                    <p class="h_d1">使用设备:</p>
                    <input class="easyui-combobox Daq_System Device_test" style="width: 180px; height: 22px; margin-left: 5px" />
                </div>
                <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                    <p class="h_d1">测温仪:</p>
                    <input class="easyui-combobox Daq_System Device_test" style="width: 180px; height: 22px; margin-left: 5px" />
                </div>
                <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                    <p class="h_d1">照度计:</p>
                    <input class="easyui-combobox Daq_System Device_test" style="width: 180px; height: 22px; margin-left: 5px" />
                </div>
                <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                    <a href="#" class="easyui-linkbutton Probe_test" data-options="iconCls:'icon-add',plain:true">添加探头</a>
                </div>
            </fieldset>
            <fieldset style="border: 1px solid #c3ddf4; margin-top: 3px;">
                <legend style="font-weight: 600">制造工艺</legend>
                <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                    <p class="h_d1">
                        <input name="Data1" type="checkbox" value="1" />锻
                          <input name="Data1" type="hidden" value="flag" />
                    </p>
                </div>
                <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                    <p class="h_d1">
                        <input name="Data2" type="checkbox" value="1" />轧
                          <input name="Data2" type="hidden" value="flag" />
                    </p>
                </div>
                <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                    <p class="h_d1">
                        <input name="Data3" type="checkbox" value="1" />铸
                          <input name="Data3" type="hidden" value="flag" />
                    </p>
                </div>
                <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                    <p class="h_d1">
                        <input name="Data4" type="checkbox" value="1" />焊
                          <input name="Data4" type="hidden" value="flag" />
                    </p>
                </div>
                <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                    <p class="h_d1">其他:</p>
                    <input name="Data5" class="easyui-textbox" style="width: 180px; height: 22px; margin-left: 5px" />
                </div>
            </fieldset>
            <fieldset style="border: 1px solid #c3ddf4; margin-top: 3px;">
                <legend style="font-weight: 600">设备</legend>
                <fieldset style="border: 1px solid #c3ddf4; margin-top: 3px;">
                    <legend style="font-weight: 600">检验介质</legend>
                    <fieldset style="border: 1px solid #c3ddf4; margin-top: 3px; width: 335px; float: left;">
                        <legend style="font-weight: 600">磁粉</legend>
                        <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                            <p class="h_d1">
                                <input name="Data6" type="checkbox" value="1" />干
                          <input name="Data6" type="hidden" value="flag" />
                            </p>
                        </div>
                        <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                            <p class="h_d1">
                                <input name="Data7" type="checkbox" value="1" />湿
                          <input name="Data7" type="hidden" value="flag" />
                            </p>
                        </div>
                        <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                            <p class="h_d1">
                                <input name="Data8" type="checkbox" value="1" />荧光
                          <input name="Data8" type="hidden" value="flag" />
                            </p>
                        </div>
                    </fieldset>
                    <fieldset style="border: 1px solid #c3ddf4; margin-top: 3px; width: 335px; float: left;">
                        <legend style="font-weight: 600">色泽</legend>
                        <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                            <p class="h_d1">
                                <input name="Data9" type="checkbox" value="1" />黑
                              <input name="Data9" type="hidden" value="flag" />
                            </p>
                        </div>
                        <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                            <p class="h_d1">
                                <input name="Data10" type="checkbox" value="1" />红棕色
                              <input name="Data10" type="hidden" value="flag" />
                            </p>
                        </div>
                        <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                            <p class="h_d1">
                                <input name="Data11" type="checkbox" value="1" />灰色
                              <input name="Data11" type="hidden" value="flag" />
                            </p>
                        </div>
                    </fieldset>
                    <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                        <p class="h_d1" style="width: 80px; text-align: right;">商标:</p>
                        <input name="Data12" class="easyui-textbox" style="width: 180px; height: 22px; margin-left: 5px" />
                    </div>
                    <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                        <p class="h_d1" style="width: 80px; text-align: right;">牌号:</p>
                        <input name="Data13" class="easyui-textbox" style="width: 180px; height: 22px; margin-left: 5px" />
                    </div>
                    <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                        <p class="h_d1" style="width: 80px; text-align: right;">批号:</p>
                        <input name="Data14" class="easyui-textbox" style="width: 180px; height: 22px; margin-left: 5px" />
                    </div>

                    <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                        <p class="h_d1" style="width: 80px; text-align: right;">磁悬液浓度:</p>
                        <input name="Data15" class="easyui-textbox" style="width: 180px; height: 22px; margin-left: 5px" />
                    </div>
                    <fieldset style="border: 1px solid #c3ddf4; margin-top: 3px; width: 100%;">
                        <legend style="font-weight: 600">载液</legend>
                        <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                            <p class="h_d1">
                                <input name="Data16" type="checkbox" value="1" />水
                          <input name="Data16" type="hidden" value="flag" />
                            </p>
                        </div>
                        <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                            <p class="h_d1">
                                <input name="Data17" type="checkbox" value="1" />油
                          <input name="Data17" type="hidden" value="flag" />
                            </p>
                        </div>
                    </fieldset>
                    <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                        <p class="h_d1">
                            <input name="Data18" type="checkbox" value="1" />反差增强剂
                          <input name="Data18" type="hidden" value="flag" />
                        </p>
                    </div>
                    <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                        <p class="h_d1">商标:</p>
                        <input name="Data19" class="easyui-textbox" style="width: 180px; height: 22px; margin-left: 5px" />
                    </div>
                    <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                        <p class="h_d1">牌号:</p>
                        <input name="Data20" class="easyui-textbox" style="width: 180px; height: 22px; margin-left: 5px" />
                    </div>

                </fieldset>
                <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                    <p class="h_d1">
                        <input name="Data21" type="checkbox" value="1" />磁场指示器
                          <input name="Data21" type="hidden" value="flag" />
                    </p>
                </div>
                <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                    <p class="h_d1">
                        <input name="Data22" type="checkbox" value="1" />人工伤试片
                          <input name="Data22" type="hidden" value="flag" />
                    </p>
                </div>
            </fieldset>
            <fieldset style="border: 1px solid #c3ddf4; margin-top: 3px;">
                <legend style="font-weight: 600">检验条件</legend>
                <fieldset style="border: 1px solid #c3ddf4; margin-top: 3px;">
                    <legend style="font-weight: 600">表面状态</legend>
                    <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                        <p class="h_d1">
                            <input name="Data23" type="checkbox" value="1" />焊态
                          <input name="Data23" type="hidden" value="flag" />
                        </p>
                    </div>
                    <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                        <p class="h_d1">
                            <input name="Data24" type="checkbox" value="1" />刚刷
                          <input name="Data24" type="hidden" value="flag" />
                        </p>
                    </div>
                    <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                        <p class="h_d1">
                            <input name="Data25" type="checkbox" value="1" />打磨
                          <input name="Data25" type="hidden" value="flag" />
                        </p>
                    </div>
                    <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                        <p class="h_d1">
                            <input name="Data26" type="checkbox" value="1" />机加
                          <input name="Data26" type="hidden" value="flag" />
                        </p>
                    </div>
                    <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                        <p class="h_d1">其它</p>
                        <input name="Data27" class="easyui-textbox" style="width: 180px; height: 22px; margin-left: 5px" />
                    </div>
                    <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                        <p class="h_d1">工件温度:</p>
                        <input name="Data28" class="easyui-textbox" style="width: 180px; height: 22px; margin-left: 5px" />
                    </div>
                    <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                        <p class="h_d1">粗糙度:</p>
                        <input name="Data29" class="easyui-textbox" style="width: 180px; height: 22px; margin-left: 5px" />
                    </div>
                </fieldset>
                <fieldset style="border: 1px solid #c3ddf4; margin-top: 3px;">
                    <legend style="font-weight: 600">预清洗</legend>
                    <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                        <p class="h_d1">
                            <input name="Data30" type="checkbox" value="1" />已做
                          <input name="Data30" type="hidden" value="flag" />
                        </p>
                    </div>
                    <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                        <p class="h_d1">
                            <input name="Data31" type="checkbox" value="1" />未做
                          <input name="Data31" type="hidden" value="flag" />
                        </p>
                    </div>
                    <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                        <p class="h_d1">清洗剂:</p>
                        <input name="Data32" class="easyui-textbox" style="width: 180px; height: 22px; margin-left: 5px" />
                    </div>
                </fieldset>
                <fieldset style="border: 1px solid #c3ddf4; margin-top: 3px;">
                    <legend style="font-weight: 600">照明</legend>
                    <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                        <p class="h_d1">
                            <input name="Data33" type="checkbox" value="1" />自然光
                          <input name="Data33" type="hidden" value="flag" />
                        </p>
                    </div>
                    <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                        <p class="h_d1">
                            <input name="Data34" type="checkbox" value="1" />行灯
                          <input name="Data34" type="hidden" value="flag" />
                        </p>
                    </div>
                    <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                        <p class="h_d1">
                            <input name="Data35" type="checkbox" value="1" />手电筒
                          <input name="Data35" type="hidden" value="flag" />
                        </p>
                    </div>
                    <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                        <p class="h_d1">
                            <input name="Data36" type="checkbox" value="1" />黑光
                          <input name="Data36" type="hidden" value="flag" />
                        </p>
                    </div>
                    <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                        <p class="h_d1">光照强度:</p>
                        <input name="Data37" class="easyui-textbox" style="width: 180px; height: 22px; margin-left: 5px" />
                    </div>
                </fieldset>
                <fieldset style="border: 1px solid #c3ddf4; margin-top: 3px;">
                    <legend style="font-weight: 600">方法</legend>
                    <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                        <p class="h_d1">
                            <input name="Data38" type="checkbox" value="1" />触头法
                          <input name="Data38" type="hidden" value="flag" />
                        </p>
                    </div>
                    <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                        <p class="h_d1">
                            <input name="Data39" type="checkbox" value="1" />电磁轭法
                          <input name="Data39" type="hidden" value="flag" />
                        </p>
                    </div>
                </fieldset>
                <fieldset style="border: 1px solid #c3ddf4; margin-top: 3px;">
                    <legend style="font-weight: 600">极间距</legend>
                    <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                        <p class="h_d1">触头间距:</p>
                        <input name="Data40" class="easyui-textbox" style="width: 180px; height: 22px; margin-left: 5px" />
                    </div>
                    <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                        <p class="h_d1">磁极间距:</p>
                        <input name="Data41" class="easyui-textbox" style="width: 180px; height: 22px; margin-left: 5px" />
                    </div>
                </fieldset>
                <fieldset style="border: 1px solid #c3ddf4; margin-top: 3px;">
                    <legend style="font-weight: 600">电流类型</legend>
                    <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                        <p class="h_d1">
                            <input name="Data42" type="checkbox" value="1" />交流
                          <input name="Data42" type="hidden" value="flag" />
                        </p>
                    </div>
                    <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                        <p class="h_d1">
                            <input name="Data43" type="checkbox" value="1" />整流电流
                          <input name="Data43" type="hidden" value="flag" />
                        </p>
                    </div>
                    <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                        <p class="h_d1">
                            <input name="Data44" type="checkbox" value="1" />直流
                          <input name="Data44" type="hidden" value="flag" />
                        </p>
                    </div>
                </fieldset>
                <fieldset style="border: 1px solid #c3ddf4; margin-top: 3px;">
                    <legend style="font-weight: 600">磁化条件</legend>
                    <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                        <p class="h_d1">安培:</p>
                        <input name="Data45" class="easyui-textbox" style="width: 180px; height: 22px; margin-left: 5px" />
                    </div>
                    <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                        <p class="h_d1">提升力:</p>
                        <input name="Data46" class="easyui-textbox" style="width: 180px; height: 22px; margin-left: 5px" />
                    </div>
                    <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                        <p class="h_d1">磁化时间:</p>
                        <input name="Data47" class="easyui-textbox" style="width: 180px; height: 22px; margin-left: 5px" />
                    </div>
                </fieldset>
                <fieldset style="border: 1px solid #c3ddf4; margin-top: 3px;">
                    <legend style="font-weight: 600">退磁</legend>
                    <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                        <p class="h_d1">
                            <input name="Data48" type="checkbox" value="1" />已做
                          <input name="Data48" type="hidden" value="flag" />
                        </p>
                    </div>

                    <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                        <p class="h_d1">
                            <input name="Data49" type="checkbox" value="1" />未做
                          <input name="Data49" type="hidden" value="flag" />
                        </p>
                    </div>
                </fieldset>
                <fieldset style="border: 1px solid #c3ddf4; margin-top: 3px;">
                    <legend style="font-weight: 600">后清洗</legend>
                    <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                        <p class="h_d1">
                            <input name="Data50" type="checkbox" value="1" />已做
                          <input name="Data50" type="hidden" value="flag" />
                        </p>
                    </div>
                    <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                        <p class="h_d1">
                            <input name="Data51" type="checkbox" value="1" />未做
                          <input name="Data51" type="hidden" value="flag" />
                        </p>
                    </div>
                    <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                        <p class="h_d1">清洗剂:</p>
                        <input name="Data52" class="easyui-textbox" style="width: 180px; height: 22px; margin-left: 5px" />
                    </div>
                </fieldset>
                <fieldset style="border: 1px solid #c3ddf4; margin-top: 3px;">
                    <legend style="font-weight: 600">检验顺序</legend>
                    <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                        <p class="h_d1">
                            <input name="Data53" type="checkbox" value="1" />单独实现
                          <input name="Data53" type="hidden" value="flag" />
                        </p>
                    </div>
                    <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                        <p class="h_d1">
                            <input name="Data54" type="checkbox" value="1" />组合实现
                          <input name="Data54" type="hidden" value="flag" />
                        </p>
                    </div>
                </fieldset>
            </fieldset>
        </form>
        <%-- 8 MT-磁粉检验报告床式 --%>
        <form id="form_8">
            <fieldset style="border: 1px solid #c3ddf4; margin-top: 3px;">
                <legend style="font-weight: 600">检验条件</legend>
                <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                    <input type="hidden" value="使用设备,测温仪,黑光强度计" name="equipment_name_R" />
                    <p class="h_d1">使用设备:</p>
                    <input class="easyui-combobox Daq_System Device_test" style="width: 180px; height: 22px; margin-left: 5px" />
                </div>
                <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                    <p class="h_d1">测温仪:</p>
                    <input class="easyui-combobox Daq_System Device_test" style="width: 180px; height: 22px; margin-left: 5px" />
                </div>
                <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                    <p class="h_d1">黑光强度计:</p>
                    <input class="easyui-combobox Daq_System Device_test" style="width: 180px; height: 22px; margin-left: 5px" />
                </div>
                <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                    <a href="#" class="easyui-linkbutton Probe_test" data-options="iconCls:'icon-add',plain:true">添加探头</a>
                </div>
            </fieldset>
            <fieldset style="border: 1px solid #c3ddf4; margin-top: 3px;">
                <legend style="font-weight: 600">制造工艺</legend>
                <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                    <p class="h_d1">
                        <input name="Data1" type="checkbox" value="1" />锻
                          <input name="Data1" type="hidden" value="flag" />
                    </p>
                </div>
                <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                    <p class="h_d1">
                        <input name="Data2" type="checkbox" value="1" />轧
                          <input name="Data2" type="hidden" value="flag" />
                    </p>
                </div>
                <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                    <p class="h_d1">
                        <input name="Data3" type="checkbox" value="1" />铸
                          <input name="Data3" type="hidden" value="flag" />
                    </p>
                </div>
                <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                    <p class="h_d1">
                        <input name="Data4" type="checkbox" value="1" />焊
                          <input name="Data4" type="hidden" value="flag" />
                    </p>
                </div>
                <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                    <p class="h_d1">其他:</p>
                    <input name="Data5" class="easyui-textbox" style="width: 180px; height: 22px; margin-left: 5px" />
                </div>
            </fieldset>
            <fieldset style="border: 1px solid #c3ddf4; margin-top: 3px;">
                <legend style="font-weight: 600">设备</legend>
                <fieldset style="border: 1px solid #c3ddf4; margin-top: 3px;">
                    <legend style="font-weight: 600">检验介质</legend>
                    <fieldset style="border: 1px solid #c3ddf4; margin-top: 3px; width: 335px; float: left;">
                        <legend style="font-weight: 600">磁粉</legend>
                        <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                            <p class="h_d1">
                                <input name="Data6" type="checkbox" value="1" />干
                          <input name="Data6" type="hidden" value="flag" />
                            </p>
                        </div>
                        <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                            <p class="h_d1">
                                <input name="Data7" type="checkbox" value="1" />湿
                          <input name="Data7" type="hidden" value="flag" />
                            </p>
                        </div>
                        <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                            <p class="h_d1">
                                <input name="Data8" type="checkbox" value="1" />荧光
                          <input name="Data8" type="hidden" value="flag" />
                            </p>
                        </div>
                    </fieldset>
                    <fieldset style="border: 1px solid #c3ddf4; margin-top: 3px; width: 335px; float: left;">
                        <legend style="font-weight: 600">色泽</legend>
                        <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                            <p class="h_d1">
                                <input name="Data9" type="checkbox" value="1" />黑
                          <input name="Data9" type="hidden" value="flag" />
                            </p>
                        </div>
                        <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                            <p class="h_d1">
                                <input name="Data10" type="checkbox" value="1" />红棕色
                          <input name="Data10" type="hidden" value="flag" />
                            </p>
                        </div>
                        <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                            <p class="h_d1">
                                <input name="Data11" type="checkbox" value="1" />灰色
                          <input name="Data11" type="hidden" value="flag" />
                            </p>
                        </div>
                    </fieldset>
                    <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                        <p class="h_d1">商标:</p>
                        <input name="Data12" class="easyui-textbox" style="width: 180px; height: 22px; margin-left: 5px" />
                    </div>
                    <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                        <p class="h_d1">牌号:</p>
                        <input name="Data13" class="easyui-textbox" style="width: 180px; height: 22px; margin-left: 5px" />
                    </div>
                    <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                        <p class="h_d1">批号:</p>
                        <input name="Data14" class="easyui-textbox" style="width: 180px; height: 22px; margin-left: 5px" />
                    </div>
                    <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                        <p class="h_d1">磁悬液浓度:</p>
                        <input name="Data15" class="easyui-textbox" style="width: 180px; height: 22px; margin-left: 5px" />
                    </div>
                    <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                        <p class="h_d1">
                            <input name="Data16" type="checkbox" value="1" />污物检查
                          <input name="Data16" type="hidden" value="flag" />
                        </p>
                    </div>
                    <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                        <p class="h_d1">磁场强度:</p>
                        <input name="Data17" class="easyui-textbox" style="width: 180px; height: 22px; margin-left: 5px" />
                    </div>
                    <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                        <p class="h_d1">
                            <input name="Data18" type="checkbox" value="1" />反差增强剂
                          <input name="Data18" type="hidden" value="flag" />
                        </p>
                    </div>
                    <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                        <p class="h_d1">商标:</p>
                        <input name="Data19" class="easyui-textbox" style="width: 180px; height: 22px; margin-left: 5px" />
                    </div>
                    <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                        <p class="h_d1">牌号:</p>
                        <input name="Data20" class="easyui-textbox" style="width: 180px; height: 22px; margin-left: 5px" />
                    </div>
                </fieldset>
                <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                    <p class="h_d1">
                        <input name="Data21" type="checkbox" value="1" />磁场指示器
                          <input name="Data21" type="hidden" value="flag" />
                    </p>
                </div>
                <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                    <p class="h_d1">
                        <input name="Data22" type="checkbox" value="1" />人工伤试片
                          <input name="Data22" type="hidden" value="flag" />
                    </p>
                </div>
            </fieldset>
            <fieldset style="border: 1px solid #c3ddf4; margin-top: 3px;">
                <legend style="font-weight: 600">检验条件</legend>
                <fieldset style="border: 1px solid #c3ddf4; margin-top: 3px;">
                    <legend style="font-weight: 600">表面状态</legend>
                    <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                        <p class="h_d1">
                            <input name="Data23" type="checkbox" value="1" />焊态
                          <input name="Data23" type="hidden" value="flag" />
                        </p>
                    </div>
                    <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                        <p class="h_d1">
                            <input name="Data24" type="checkbox" value="1" />喷丸
                          <input name="Data24" type="hidden" value="flag" />
                        </p>
                    </div>
                    <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                        <p class="h_d1">
                            <input name="Data25" type="checkbox" value="1" />打磨
                          <input name="Data25" type="hidden" value="flag" />
                        </p>
                    </div>
                    <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                        <p class="h_d1">
                            <input name="Data26" type="checkbox" value="1" />机加
                          <input name="Data26" type="hidden" value="flag" />
                        </p>
                    </div>
                    <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                        <p class="h_d1">
                            <input name="Data27" type="checkbox" value="1" />抛光
                          <input name="Data27" type="checkbox" value="1" />
                        </p>
                    </div>
                    <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                        <p class="h_d1">
                            <input name="Data54" type="checkbox" value="1" />刷
                          <input name="Data54" type="checkbox" value="1" />
                        </p>
                    </div>
                    <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                        <p class="h_d1">工件温度:</p>
                        <input name="Data28" class="easyui-textbox" style="width: 180px; height: 22px; margin-left: 5px" />
                    </div>
                    <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                        <p class="h_d1">粗糙度:</p>
                        <input name="Data29" class="easyui-textbox" style="width: 180px; height: 22px; margin-left: 5px" />
                    </div>
                </fieldset>
                <fieldset style="border: 1px solid #c3ddf4; margin-top: 3px;">
                    <legend style="font-weight: 600">预清洗</legend>
                    <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                        <p class="h_d1">
                            <input name="Data30" type="checkbox" value="1" />已做
                          <input name="Data30" type="hidden" value="flag" />
                        </p>
                    </div>
                    <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                        <p class="h_d1">
                            <input name="Data31" type="checkbox" value="1" />未做
                          <input name="Data31" type="hidden" value="flag" />
                        </p>
                    </div>
                    <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                        <p class="h_d1">清洗剂:</p>
                        <input name="Data32" class="easyui-textbox" style="width: 180px; height: 22px; margin-left: 5px" />
                    </div>
                </fieldset>
                <fieldset style="border: 1px solid #c3ddf4; margin-top: 3px;">
                    <legend style="font-weight: 600">照明</legend>
                    <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                        <p class="h_d1">
                            <input name="Data33" type="checkbox" value="1" />自然光
                          <input name="Data33" type="hidden" value="flag" />
                        </p>
                    </div>
                    <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                        <p class="h_d1">
                            <input name="Data34" type="checkbox" value="1" />行灯
                          <input name="Data34" type="hidden" value="flag" />
                        </p>
                    </div>
                    <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                        <p class="h_d1">
                            <input name="Data35" type="checkbox" value="1" />手电筒
                          <input name="Data35" type="hidden" value="flag" />
                        </p>
                    </div>
                    <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                        <p class="h_d1">
                            <input name="Data36" type="checkbox" value="1" />黑光
                          <input name="Data36" type="hidden" value="flag" />
                        </p>
                    </div>
                    <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                        <p class="h_d1">光照强度:</p>
                        <input name="Data37" class="easyui-textbox" style="width: 180px; height: 22px; margin-left: 5px" />
                    </div>
                </fieldset>
                <fieldset style="border: 1px solid #c3ddf4; margin-top: 3px;">
                    <legend style="font-weight: 600">方法</legend>
                    <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                        <p class="h_d1">
                            <input name="Data38" type="checkbox" value="1" />纵向纵化法
                          <input name="Data38" type="hidden" value="flag" />
                        </p>
                    </div>
                    <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                        <p class="h_d1">
                            <input name="Data39" type="checkbox" value="1" />周向磁化法
                          <input name="Data39" type="hidden" value="flag" />
                        </p>
                    </div>
                </fieldset>
                <fieldset style="border: 1px solid #c3ddf4; margin-top: 3px;">
                    <legend style="font-weight: 600">电流类型</legend>
                    <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                        <p class="h_d1">
                            <input name="Data40" type="checkbox" value="1" />交流1
                          <input name="Data40" type="hidden" value="flag" />
                        </p>
                    </div>
                    <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                        <p class="h_d1">
                            <input name="Data41" type="checkbox" value="1" />整流1
                          <input name="Data41" type="hidden" value="flag" />
                        </p>
                    </div>
                    <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                        <p class="h_d1">
                            <input name="Data42" type="checkbox" value="1" />交流2
                          <input name="Data42" type="hidden" value="flag" />
                        </p>
                    </div>
                    <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                        <p class="h_d1">
                            <input name="Data43" type="checkbox" value="1" />整流2
                          <input name="Data43" type="hidden" value="flag" />
                        </p>
                    </div>
                </fieldset>
                <fieldset style="border: 1px solid #c3ddf4; margin-top: 3px;">
                    <legend style="font-weight: 600">磁化条件</legend>
                    <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                        <p class="h_d1">安培1:</p>
                        <input name="Data44" class="easyui-textbox" style="width: 180px; height: 22px; margin-left: 5px" />
                    </div>
                    <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                        <p class="h_d1">磁化时间1:</p>
                        <input name="Data45" class="easyui-textbox" style="width: 180px; height: 22px; margin-left: 5px" />
                    </div>
                    <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                        <p class="h_d1">安培2:</p>
                        <input name="Data46" class="easyui-textbox" style="width: 180px; height: 22px; margin-left: 5px" />
                    </div>
                    <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                        <p class="h_d1">磁化时间2:</p>
                        <input name="Data47" class="easyui-textbox" style="width: 180px; height: 22px; margin-left: 5px" />
                    </div>
                </fieldset>
                <fieldset style="border: 1px solid #c3ddf4; margin-top: 3px;">
                    <legend style="font-weight: 600">检验顺序</legend>
                    <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                        <p class="h_d1">
                            <input name="Data48" type="checkbox" value="1" />组合实现
                          <input name="Data48" type="hidden" value="flag" />
                        </p>
                    </div>
                </fieldset>
                <fieldset style="border: 1px solid #c3ddf4; margin-top: 3px;">
                    <legend style="font-weight: 600">磁场强度</legend>
                    <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                        <p class="h_d1">磁场强度:</p>
                        <input name="Data55" class="easyui-textbox" style="width: 180px; height: 22px; margin-left: 5px" />
                    </div>
                </fieldset>
                <fieldset style="border: 1px solid #c3ddf4; margin-top: 3px;">
                    <legend style="font-weight: 600">退磁</legend>
                    <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                        <p class="h_d1">
                            <input name="Data49" type="checkbox" value="1" />已做
                          <input name="Data49" type="hidden" value="flag" />
                        </p>
                    </div>
                    <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                        <p class="h_d1">
                            <input name="Data50" type="checkbox" value="1" />未做
                          <input name="Data50" type="hidden" value="flag" />
                        </p>
                    </div>
                </fieldset>
                <fieldset style="border: 1px solid #c3ddf4; margin-top: 3px;">
                    <legend style="font-weight: 600">后清洗</legend>
                    <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                        <p class="h_d1">
                            <input name="Data51" type="checkbox" value="1" />已做
                          <input name="Data51" type="hidden" value="flag" />
                        </p>
                    </div>
                    <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                        <p class="h_d1">
                            <input name="Data52" type="checkbox" value="1" />未做
                          <input name="Data52" type="hidden" value="flag" />
                        </p>
                    </div>

                    <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                        <p class="h_d1">清洗剂:</p>
                        <input name="Data53" class="easyui-textbox" style="width: 180px; height: 22px; margin-left: 5px" />
                    </div>
                </fieldset>
            </fieldset>
        </form>
        <%--10 UT-超声波测厚报告 --%>
        <form id="form_10">
            <fieldset style="border: 1px solid #c3ddf4; margin-top: 3px;">
                <legend style="font-weight: 600">检验条件</legend>
                <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                    <input type="hidden" value="仪器" name="equipment_name_R" />
                    <p class="h_d1">仪器:</p>
                    <input class="easyui-combobox Daq_System Device_test" style="width: 180px; height: 22px; margin-left: 5px" />
                </div>

                <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                    <a href="#" class="easyui-linkbutton Probe_test" data-options="iconCls:'icon-add',plain:true">添加探头</a>
                </div>
            </fieldset>
            <fieldset style="border: 1px solid #c3ddf4; margin-top: 3px;">
                <legend style="font-weight: 600">制造工艺</legend>
                <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                    <p class="h_d1">
                        <input name="Data1" type="checkbox" value="1" />锻
                        <input name="Data1" type="hidden" value="flag" />
                    </p>
                </div>
                <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                    <p class="h_d1">
                        <input name="Data2" type="checkbox" value="1" />轧
                        <input name="Data2" type="hidden" value="flag" />
                    </p>
                </div>
                <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                    <p class="h_d1">
                        <input name="Data3" type="checkbox" value="1" />铸
                         <input name="Data3" type="hidden" value="flag" />
                    </p>
                </div>
                <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                    <p class="h_d1">
                        <input name="Data4" type="checkbox" value="1" />焊
                         <input name="Data4" type="hidden" value="flag" />
                    </p>
                </div>
                <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                    <p class="h_d1">其他:</p>
                    <input name="Data5" class="easyui-textbox" style="width: 180px; height: 22px; margin-left: 5px" />
                </div>
                <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                    <p class="h_d1">焊接方法:</p>
                    <input name="Data6" class="easyui-textbox" style="width: 180px; height: 22px; margin-left: 5px" />
                </div>
            </fieldset>
            <fieldset style="border: 1px solid #c3ddf4; margin-top: 3px;">
                <legend style="font-weight: 600">设备</legend>
                <fieldset style="border: 1px solid #c3ddf4; margin-top: 3px;">
                    <legend style="font-weight: 600">耦合剂</legend>
                    <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                        <p class="h_d1">
                            <input name="Data7" type="checkbox" value="1" />纤维素胶
                          <input name="Data7" type="hidden" value="flag" />
                        </p>
                    </div>
                    <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                        <p class="h_d1">
                            <input name="Data8" type="checkbox" value="1" />浆糊
                          <input name="Data8" type="hidden" value="flag" />
                        </p>
                    </div>
                    <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                        <p class="h_d1">
                            <input name="Data9" type="checkbox" value="1" />机油
                          <input name="Data9" type="hidden" value="flag" />
                        </p>
                    </div>
                    <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                        <p class="h_d1">
                            <input name="Data10" type="checkbox" value="1" />水
                          <input name="Data10" type="hidden" value="flag" />
                        </p>
                    </div>
                </fieldset>
                <fieldset style="border: 1px solid #c3ddf4; margin-top: 3px;">
                    <legend style="font-weight: 600">商标</legend>
                    <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                        <p class="h_d1">
                            <input name="Data11" type="checkbox" value="1" />GE ZG-F
                         <input name="Data11" type="hidden" value="flag" />
                        </p>
                    </div>
                    <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                        <p class="h_d1">
                            <input name="Data12" type="checkbox" value="1" />新美达CG-10
                         <input name="Data12" type="hidden" value="flag" />
                        </p>
                    </div>
                    <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                        <p class="h_d1">
                            <input name="Data13" type="checkbox" value="1" />新美达CG-88 
                         <input name="Data13" type="hidden" value="flag" />
                        </p>
                    </div>
                    <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                        <p class="h_d1">
                            <input name="Data14" type="checkbox" value="1" />SOFRANEL
                         <input name="Data14" type="hidden" value="flag" />
                        </p>
                    </div>
                </fieldset>
            </fieldset>
            <fieldset style="border: 1px solid #c3ddf4; margin-top: 3px;">
                <legend style="font-weight: 600">表面准备</legend>
                <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                    <p class="h_d1">
                        <input name="Data15" type="checkbox" value="1" />未加工
                            <input name="Data15" type="hidden" value="flag" />
                    </p>
                </div>
                <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                    <p class="h_d1">
                        <input name="Data16" type="checkbox" value="1" />打磨
                         <input name="Data16" type="hidden" value="flag" />
                    </p>
                </div>
                <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                    <p class="h_d1">
                        <input name="Data17" type="checkbox" value="1" />钢刷:
                         <input name="Data17" type="hidden" value="flag" />
                    </p>
                </div>
                <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                    <p class="h_d1">
                        <input name="Data18" type="checkbox" value="1" />焊态
                         <input name="Data18" type="hidden" value="flag" />
                    </p>
                </div>
                <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                    <p class="h_d1">
                        <input name="Data19" type="checkbox" value="1" />机加:
                         <input name="Data19" type="hidden" value="flag" />
                    </p>
                </div>
                <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                    <p class="h_d1">粗糙度:</p>
                    <input name="Data20" class="easyui-textbox" style="width: 180px; height: 22px; margin-left: 5px" />
                </div>
                <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                    <p class="h_d1">
                        <input name="Data21" type="checkbox" value="1" />试块与被检表面温度差
                          <input name="Data21" type="hidden" value="flag" />
                    </p>
                </div>
            </fieldset>
            <fieldset style="border: 1px solid #c3ddf4; margin-top: 3px;">
                <legend style="font-weight: 600">检验条件</legend>
                <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                    <p class="h_d1">周期校验:</p>
                    <input name="Data22" class="easyui-textbox" style="width: 180px; height: 22px; margin-left: 5px" />
                </div>
                <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                    <p class="h_d1">开始: </p>
                    <input name="Data23" class="easyui-textbox" style="width: 180px; height: 22px; margin-left: 5px" />
                </div>
                <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                    <p class="h_d1">中间: </p>
                    <input name="Data27" class="easyui-textbox" style="width: 180px; height: 22px; margin-left: 5px" />
                </div>
                <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                    <p class="h_d1">结束:</p>
                    <input name="Data24" class="easyui-textbox" style="width: 180px; height: 22px; margin-left: 5px" />
                </div>
                <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                    <p class="h_d1">
                        <input name="Data25" type="checkbox" value="1" />后清理
                          <input name="Data25" type="hidden" value="flag" />
                    </p>
                </div>
                <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                    <p class="h_d1">清理材料:</p>
                    <input name="Data26" class="easyui-textbox" style="width: 180px; height: 22px; margin-left: 5px" />
                </div>
            </fieldset>
        </form>
        <%--11-UT-超声波检验报告 --%>
        <form id="form_11" style="overflow-x: hidden">
            <fieldset style="border: 1px solid #c3ddf4; margin-top: 3px;">
                <legend style="font-weight: 600">检验条件</legend>
                <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                    <input type="hidden" value="仪器" name="equipment_name_R" />
                    <p class="h_d1">仪器:</p>
                    <input class="easyui-combobox Daq_System Device_test" style="width: 180px; height: 22px; margin-left: 5px" />
                </div>

                <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                    <a href="#" class="easyui-linkbutton Probe_test" data-options="iconCls:'icon-add',plain:true">添加探头</a>
                </div>
            </fieldset>
            <fieldset style="border: 1px solid #c3ddf4; margin-top: 3px;">
                <legend style="font-weight: 600">制造工艺</legend>
                <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                    <p class="h_d1">
                        <input name="Data1" type="checkbox" value="1" />锻
                        <input name="Data1" type="hidden" value="flag" />
                    </p>
                </div>
                <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                    <p class="h_d1">
                        <input name="Data2" type="checkbox" value="1" />轧
                        <input name="Data2" type="hidden" value="flag" />
                    </p>
                </div>
                <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                    <p class="h_d1">
                        <input name="Data3" type="checkbox" value="1" />铸
                         <input name="Data3" type="hidden" value="flag" />
                    </p>
                </div>
                <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                    <p class="h_d1">
                        <input name="Data4" type="checkbox" value="1" />焊
                         <input name="Data4" type="hidden" value="flag" />
                    </p>
                </div>
                <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                    <p class="h_d1">其他:</p>
                    <input name="Data5" class="easyui-textbox" style="width: 180px; height: 22px; margin-left: 5px" />
                </div>
                <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                    <p class="h_d1">焊接方法:</p>
                    <input name="Data6" class="easyui-textbox" style="width: 180px; height: 22px; margin-left: 5px" />
                </div>
            </fieldset>
            <fieldset style="border: 1px solid #c3ddf4; margin-top: 3px;">
                <legend style="font-weight: 600">设备</legend>
                <fieldset style="border: 1px solid #c3ddf4; margin-top: 3px;">
                    <legend style="font-weight: 600">耦合剂</legend>
                    <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                        <p class="h_d1">
                            <input name="Data7" type="checkbox" value="1" />纤维素胶
                          <input name="Data7" type="hidden" value="flag" />
                        </p>
                    </div>
                    <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                        <p class="h_d1">
                            <input name="Data8" type="checkbox" value="1" />浆糊
                          <input name="Data8" type="hidden" value="flag" />
                        </p>
                    </div>
                    <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                        <p class="h_d1">
                            <input name="Data9" type="checkbox" value="1" />机油
                          <input name="Data9" type="hidden" value="flag" />
                        </p>
                    </div>
                    <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                        <p class="h_d1">
                            <input name="Data10" type="checkbox" value="1" />水
                          <input name="Data10" type="hidden" value="flag" />
                        </p>
                    </div>
                </fieldset>
                <fieldset style="border: 1px solid #c3ddf4; margin-top: 3px;">
                    <legend style="font-weight: 600">商标</legend>
                    <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                        <p class="h_d1">
                            <input name="Data11" type="checkbox" value="1" />GE ZG-F
                         <input name="Data11" type="hidden" value="flag" />
                        </p>
                    </div>
                    <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                        <p class="h_d1">
                            <input name="Data12" type="checkbox" value="1" />新美达CG-10
                         <input name="Data12" type="hidden" value="flag" />
                        </p>
                    </div>
                    <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                        <p class="h_d1">
                            <input name="Data13" type="checkbox" value="1" />新美达CG-88 
                         <input name="Data13" type="hidden" value="flag" />
                        </p>
                    </div>
                    <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                        <p class="h_d1">
                            <input name="Data14" type="checkbox" value="1" />SOFRANEL
                         <input name="Data14" type="hidden" value="flag" />
                        </p>
                    </div>
                </fieldset>
            </fieldset>
            <fieldset style="border: 1px solid #c3ddf4; margin-top: 3px;">
                <legend style="font-weight: 600">表面准备</legend>
                <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                    <p class="h_d1">
                        <input name="Data15" type="checkbox" value="1" />未加工
                            <input name="Data15" type="hidden" value="flag" />
                    </p>
                </div>
                <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                    <p class="h_d1">
                        <input name="Data16" type="checkbox" value="1" />打磨
                         <input name="Data16" type="hidden" value="flag" />
                    </p>
                </div>
                <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                    <p class="h_d1">
                        <input name="Data17" type="checkbox" value="1" />钢刷:
                         <input name="Data17" type="hidden" value="flag" />
                    </p>
                </div>
                <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                    <p class="h_d1">
                        <input name="Data18" type="checkbox" value="1" />焊态
                         <input name="Data18" type="hidden" value="flag" />
                    </p>
                </div>
                <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                    <p class="h_d1">
                        <input name="Data19" type="checkbox" value="1" />机加
                         <input name="Data19" type="hidden" value="flag" />
                    </p>
                </div>
                <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                    <p class="h_d1">粗糙度:</p>
                    <input name="Data20" class="easyui-textbox" style="width: 180px; height: 22px; margin-left: 5px" />
                </div>
                <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                    <p class="h_d1">
                        <input name="Data21" type="checkbox" value="1" />试块与被检表面温度差
                          <input name="Data21" type="hidden" value="flag" />
                    </p>
                </div>
            </fieldset>
            <fieldset style="border: 1px solid #c3ddf4; margin-top: 3px;">
                <legend style="font-weight: 600">检验条件</legend>
                <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                    <p class="h_d1">周期校验:</p>
                    <input name="Data22" class="easyui-textbox" style="width: 180px; height: 22px; margin-left: 5px" />
                </div>
                <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                    <p class="h_d1">开始: </p>
                    <input name="Data23" class="easyui-textbox" style="width: 180px; height: 22px; margin-left: 5px" />
                </div>
                <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                    <p class="h_d1">中间: </p>
                    <input name="Data31" class="easyui-textbox" style="width: 180px; height: 22px; margin-left: 5px" />
                </div>
                <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                    <p class="h_d1">结束:</p>
                    <input name="Data24" class="easyui-textbox" style="width: 180px; height: 22px; margin-left: 5px" />
                </div>
                <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                    <p class="h_d1">
                        <input name="Data25" type="checkbox" value="1" />仪器抑制
                          <input name="Data25" type="hidden" value="flag" />
                    </p>
                </div>
                <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                    <p class="h_d1">扫查灵敏度:</p>
                    <input name="Data26" class="easyui-textbox" style="width: 180px; height: 22px; margin-left: 5px" />
                </div>
                <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                    <p class="h_d1">
                        <input name="Data27" type="checkbox" value="1" />后清理
                          <input name="Data27" type="hidden" value="flag" />
                    </p>
                </div>
                <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                    <p class="h_d1">清理材料:</p>
                    <input name="Data28" class="easyui-textbox" style="width: 180px; height: 22px; margin-left: 5px" />
                </div>
                <fieldset style="border: 1px solid #c3ddf4; margin-top: 3px; width: 100%;">
                    <legend style="font-weight: 600">盲区</legend>
                    <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                        <p class="h_d1">
                            <input name="Data29" type="checkbox" value="1" />有
                          <input name="Data29" type="hidden" value="flag" />
                        </p>
                    </div>
                    <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                        <p class="h_d1">
                            <input name="Data30" type="checkbox" value="1" />无
                          <input name="Data30" type="hidden" value="flag" />
                        </p>
                    </div>
                </fieldset>
            </fieldset>
        </form>
        <%--13 自动超声波检验报告 --%>
        <form id="form_13">
            <fieldset style="border: 1px solid #c3ddf4; margin-top: 3px;">
                <legend style="font-weight: 600">检验条件</legend>
                <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                    <input type="hidden" value="超声数据采集系统,扫查器,运动控制器" name="equipment_name_R" />
                    <p class="h_d1">超声数据采集系统:</p>
                    <input class="easyui-combobox Daq_System Device_test" style="width: 180px; height: 22px; margin-left: 5px" />
                </div>
                <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                    <p class="h_d1">扫查器:</p>
                    <input class="easyui-combobox Scanner Device_test" style="width: 180px; height: 22px; margin-left: 5px" />
                </div>
                <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                    <p class="h_d1">运动控制器:</p>
                    <input class="easyui-combobox Motion_Controller Device_test" style="width: 180px; height: 22px; margin-left: 5px" />
                </div>
                <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                    <a href="#" class="easyui-linkbutton Probe_test" data-options="iconCls:'icon-add',plain:true">探头</a>
                </div>
            </fieldset>
            <fieldset style="border: 1px solid #c3ddf4; margin-top: 3px;">
                <legend style="font-weight: 600">制造工艺</legend>
                <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                    <p class="h_d1">
                        <input name="Data1" type="checkbox" value="1" />锻
                        <input name="Data1" type="hidden" value="flag" />
                    </p>
                </div>
                <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                    <p class="h_d1">
                        <input name="Data2" type="checkbox" value="1" />轧
                        <input name="Data2" type="hidden" value="flag" />
                    </p>
                </div>
                <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                    <p class="h_d1">
                        <input name="Data3" type="checkbox" value="1" />铸
                         <input name="Data3" type="hidden" value="flag" />
                    </p>
                </div>
                <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                    <p class="h_d1">
                        <input name="Data4" type="checkbox" value="1" />焊
                         <input name="Data4" type="hidden" value="flag" />
                    </p>
                </div>
                <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                    <p class="h_d1">其他:</p>
                    <input name="Data5" class="easyui-textbox" style="width: 180px; height: 22px; margin-left: 5px" />
                </div>
                <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                    <p class="h_d1">焊接方法:</p>
                    <input name="Data6" class="easyui-textbox" style="width: 180px; height: 22px; margin-left: 5px" />
                </div>
                <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                    <p class="h_d1">耦合剂类型:</p>
                    <input name="Data7" class="easyui-textbox" style="width: 180px; height: 22px; margin-left: 5px" />
                </div>
            </fieldset>
        </form>
        <%-- 26 PT-液体渗透检验报告 --%>
        <form id="form_26">
            <fieldset style="border: 1px solid #c3ddf4; margin-top: 3px;">
                <legend style="font-weight: 600">检验条件</legend>
                <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                    <input type="hidden" value="仪器,照度计/黑光强度计" name="equipment_name_R" />
                    <p class="h_d1">仪器:</p>
                    <input class="easyui-combobox Daq_System Device_test" style="width: 180px; height: 22px; margin-left: 5px" />
                </div>
                <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                    <p class="h_d1">照度计/黑光强度计:</p>
                    <input class="easyui-combobox Scanner Device_test" style="width: 180px; height: 22px; margin-left: 5px" />
                </div>
                <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                    <a href="#" class="easyui-linkbutton Probe_test" data-options="iconCls:'icon-add',plain:true">添加探头</a>
                </div>
            </fieldset>
            <fieldset style="border: 1px solid #c3ddf4; margin-top: 3px;">
                <legend style="font-weight: 600">制造工艺</legend>
                <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                    <p class="h_d1">
                        <input name="Data1" type="checkbox" value="1" />锻
                           <input name="Data1" type="hidden" value="flag" />
                    </p>
                </div>
                <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                    <p class="h_d1">
                        <input name="Data2" type="checkbox" value="1" />轧
                          <input name="Data2" type="hidden" value="flag" />
                    </p>
                </div>
                <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                    <p class="h_d1">
                        <input name="Data3" type="checkbox" value="1" />铸
                          <input name="Data3" type="hidden" value="flag" />
                    </p>
                </div>
                <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                    <p class="h_d1">
                        <input name="Data4" type="checkbox" value="1" />焊
                          <input name="Data4" type="hidden" value="flag" />
                    </p>
                </div>
                <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                    <p class="h_d1">其他:</p>
                    <input name="Data5" class="easyui-textbox" style="width: 180px; height: 22px; margin-left: 5px" />
                </div>
            </fieldset>
            <fieldset style="border: 1px solid #c3ddf4; margin-top: 3px;">
                <legend style="font-weight: 600">试剂</legend>
                <fieldset style="border: 1px solid #c3ddf4; margin-top: 3px;">
                    <legend style="font-weight: 600">渗透剂</legend>
                    <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                        <p class="h_d1">
                            <input name="Data6" type="checkbox" value="1" />着色
                          <input name="Data6" type="hidden" value="flag" />
                        </p>
                    </div>
                    <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                        <p class="h_d1">
                            <input name="Data7" type="checkbox" value="1" />荧光
                          <input name="Data7" type="hidden" value="flag" />
                        </p>
                    </div>

                    <div class="h_dline" style="height: 23px; margin-top: 0px; float: left; margin-left: 10px">
                        <p class="h_d1">商标:</p>
                        <input name="Data8" class="easyui-textbox" style="width: 180px; height: 22px; margin-left: 5px" />
                    </div>
                    <div class="h_dline" style="height: 23px; margin-top: 0px; float: left; margin-left: 10px">
                        <p class="h_d1">型号:</p>
                        <input name="Data9" class="easyui-textbox" style="width: 180px; height: 22px; margin-left: 5px" />
                    </div>
                    <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                        <p class="h_d1">批号:</p>
                        <input name="Data10" class="easyui-textbox" style="width: 180px; height: 22px; margin-left: 5px" />
                    </div>
                </fieldset>
                <fieldset style="border: 1px solid #c3ddf4; margin-top: 3px;">
                    <legend style="font-weight: 600">清洗剂</legend>
                    <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                        <p class="h_d1">
                            <input name="Data11" type="checkbox" value="1" />水洗
                          <input name="Data11" type="hidden" value="flag" />
                        </p>
                    </div>
                    <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                        <p class="h_d1">
                            <input name="Data12" type="checkbox" value="1" />溶剂
                          <input name="Data12" type="hidden" value="flag" />
                        </p>
                    </div>
                    <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                        <p class="h_d1">商标:</p>
                        <input name="Data13" class="easyui-textbox" style="width: 180px; height: 22px; margin-left: 5px" />
                    </div>
                    <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                        <p class="h_d1">型号:</p>
                        <input name="Data14" class="easyui-textbox" style="width: 180px; height: 22px; margin-left: 5px" />
                    </div>
                    <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                        <p class="h_d1">批号:</p>
                        <input name="Data15" class="easyui-textbox" style="width: 180px; height: 22px; margin-left: 5px" />
                    </div>
                </fieldset>
                <fieldset style="border: 1px solid #c3ddf4; margin-top: 3px;">
                    <legend style="font-weight: 600">显像剂</legend>
                    <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                        <p class="h_d1">
                            <input name="Data16" type="checkbox" value="1" />湿粉
                          <input name="Data16" type="hidden" value="flag" />
                        </p>
                    </div>
                    <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                        <p class="h_d1">
                            <input name="Data17" type="checkbox" value="1" />干粉
                          <input name="Data17" type="hidden" value="flag" />
                        </p>
                    </div>
                    <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                        <p class="h_d1">商标:</p>
                        <input name="Data18" class="easyui-textbox" style="width: 180px; height: 22px; margin-left: 5px" />
                    </div>
                    <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                        <p class="h_d1">型号:</p>
                        <input name="Data19" class="easyui-textbox" style="width: 180px; height: 22px; margin-left: 5px" />
                    </div>
                    <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                        <p class="h_d1">批号:</p>
                        <input name="Data20" class="easyui-textbox" style="width: 180px; height: 22px; margin-left: 5px" />
                    </div>
                </fieldset>
            </fieldset>
            <fieldset style="border: 1px solid #c3ddf4; margin-top: 3px;">
                <legend style="font-weight: 600">表面状态</legend>
                <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                    <p class="h_d1">
                        <input name="Data21" type="checkbox" value="1" />焊态
                          <input name="Data21" type="hidden" value="flag" />
                    </p>
                </div>
                <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                    <p class="h_d1">
                        <input name="Data22" type="checkbox" value="1" />打磨
                         <input name="Data22" type="hidden" value="flag" />
                    </p>
                </div>
                <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                    <p class="h_d1">
                        <input name="Data23" type="checkbox" value="1" />钢刷
                         <input name="Data23" type="hidden" value="flag" />
                    </p>
                </div>
                <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                    <p class="h_d1">
                        <input name="Data24" type="checkbox" value="1" />抛光
                         <input name="Data24" type="hidden" value="flag" />
                    </p>
                </div>
                <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                    <p class="h_d1">
                        <input name="Data25" type="checkbox" value="1" />机加
                         <input name="Data25" type="hidden" value="flag" />
                    </p>
                </div>
                <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                    <p class="h_d1">其它:</p>
                    <input name="Data26" class="easyui-textbox" style="width: 180px; height: 22px; margin-left: 5px" />
                </div>

                <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                    <p class="h_d1">粗糙度:</p>
                    <input name="Data27" class="easyui-textbox" style="width: 180px; height: 22px; margin-left: 5px" />
                </div>
                <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                    <p class="h_d1">受检件温度:</p>
                    <input name="Data28" class="easyui-textbox" style="width: 180px; height: 22px; margin-left: 5px" />
                </div>
            </fieldset>
            <fieldset style="border: 1px solid #c3ddf4; margin-top: 3px;">
                <legend style="font-weight: 600">检验条件</legend>
                <fieldset style="border: 1px solid #c3ddf4; margin-top: 3px;">
                    <legend style="font-weight: 600">预清洗</legend>
                    <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                        <p class="h_d1">
                            <input name="Data29" type="checkbox" value="1" />已做
                         <input name="Data29" type="hidden" value="flag" />
                        </p>
                    </div>
                    <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                        <p class="h_d1">
                            <input name="Data30" type="checkbox" value="1" />未做
                         <input name="Data30" type="hidden" value="flag" />
                        </p>
                    </div>
                </fieldset>
                <fieldset style="border: 1px solid #c3ddf4; margin-top: 3px;">
                    <legend style="font-weight: 600">清洗剂</legend>
                    <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                        <p class="h_d1">
                            <input name="Data31" type="checkbox" value="1" />丙酮
                         <input name="Data31" type="hidden" value="flag" />
                        </p>
                    </div>
                    <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                        <p class="h_d1">
                            <input name="Data32" type="checkbox" value="1" />威第尔
                         <input name="Data32" type="hidden" value="flag" />
                        </p>
                    </div>
                    <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                        <p class="h_d1">其它:</p>
                        <input name="Data33" class="easyui-textbox" style="width: 180px; height: 22px; margin-left: 5px" />
                    </div>
                </fieldset>
                <fieldset style="border: 1px solid #c3ddf4; margin-top: 3px;">
                    <legend style="font-weight: 600">干燥方法</legend>
                    <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                        <p class="h_d1">
                            <input name="Data34" type="checkbox" value="1" />自然蒸发
                         <input name="Data34" type="hidden" value="flag" />
                        </p>
                    </div>
                    <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                        <p class="h_d1">
                            <input name="Data35" type="checkbox" value="1" />强制热或冷空气蒸发
                         <input name="Data35" type="hidden" value="flag" />
                        </p>
                    </div>
                    <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                        <p class="h_d1">干燥时间:</p>
                        <input name="Data36" class="easyui-textbox" style="width: 180px; height: 22px; margin-left: 5px" />
                    </div>
                </fieldset>
                <fieldset style="border: 1px solid #c3ddf4; margin-top: 3px;">
                    <legend style="font-weight: 600">渗透剂施加</legend>
                    <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                        <p class="h_d1">
                            <input name="Data37" type="checkbox" value="1" />刷涂
                         <input name="Data37" type="hidden" value="flag" />
                        </p>
                    </div>
                    <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                        <p class="h_d1">
                            <input name="Data38" type="checkbox" value="1" />喷涂
                         <input name="Data38" type="hidden" value="flag" />
                        </p>
                    </div>
                    <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                        <p class="h_d1">渗透时间:</p>
                        <input name="Data39" class="easyui-textbox" style="width: 180px; height: 22px; margin-left: 5px" />
                    </div>
                </fieldset>
                <fieldset style="border: 1px solid #c3ddf4; margin-top: 3px;">
                    <legend style="font-weight: 600">多余渗透剂的去除</legend>
                    <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                        <p class="h_d1">
                            <input name="Data40" type="checkbox" value="1" />不起毛的布
                         <input name="Data40" type="hidden" value="flag" />
                        </p>
                    </div>
                    <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                        <p class="h_d1">
                            <input name="Data41" type="checkbox" value="1" />吸水纸
                         <input name="Data41" type="hidden" value="flag" />
                        </p>
                    </div>
                    <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                        <p class="h_d1">
                            <input name="Data42" type="checkbox" value="1" />水
                         <input name="Data42" type="hidden" value="flag" />
                        </p>
                    </div>
                    <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                        <p class="h_d1">
                            <input name="Data43" type="checkbox" value="1" />溶剂
                         <input name="Data43" type="hidden" value="flag" />
                        </p>
                    </div>
                </fieldset>
                <fieldset style="border: 1px solid #c3ddf4; margin-top: 3px;">
                    <legend style="font-weight: 600">干燥方法</legend>
                    <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                        <p class="h_d1">
                            <input name="Data44" type="checkbox" value="1" />干净的材料吸干
                         <input name="Data44" type="hidden" value="flag" />
                        </p>
                    </div>
                    <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                        <p class="h_d1">
                            <input name="Data45" type="checkbox" value="1" />自然蒸发
                         <input name="Data45" type="hidden" value="flag" />
                        </p>
                    </div>
                    <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                        <p class="h_d1">干燥时间:</p>
                        <input name="Data46" class="easyui-textbox" style="width: 180px; height: 22px; margin-left: 5px" />
                    </div>
                </fieldset>
                <fieldset style="border: 1px solid #c3ddf4; margin-top: 3px;">
                    <legend style="font-weight: 600">显像剂施加方法</legend>
                    <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                        <p class="h_d1">
                            <input name="Data47" type="checkbox" value="1" />喷涂
                         <input name="Data47" type="hidden" value="flag" />
                        </p>
                    </div>
                </fieldset>
                <fieldset style="border: 1px solid #c3ddf4; margin-top: 3px;">
                    <legend style="font-weight: 600">显像剂施加方法</legend>
                    <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                        <p class="h_d1">
                            <input name="Data48" type="checkbox" value="1" />自然蒸发
                         <input name="Data48" type="hidden" value="flag" />
                        </p>
                    </div>
                </fieldset>
                <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                    <p class="h_d1">显像剂干燥时间:</p>
                    <input name="Data49" class="easyui-textbox" style="width: 180px; height: 22px; margin-left: 5px" />
                </div>
                <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                    <p class="h_d1">最终评定时间:</p>
                    <input name="Data50" class="easyui-textbox" style="width: 180px; height: 22px; margin-left: 5px" />
                </div>
                <fieldset style="border: 1px solid #c3ddf4; margin-top: 3px;">
                    <legend style="font-weight: 600">照明</legend>
                    <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                        <p class="h_d1">
                            <input name="Data51" type="checkbox" value="1" />自然光
                         <input name="Data51" type="hidden" value="flag" />
                        </p>
                    </div>
                    <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                        <p class="h_d1">
                            <input name="Data52" type="checkbox" value="1" />行灯
                         <input name="Data52" type="hidden" value="flag" />
                        </p>
                    </div>
                    <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                        <p class="h_d1">
                            <input name="Data53" type="checkbox" value="1" />手电筒
                         <input name="Data53" type="hidden" value="flag" />
                        </p>
                    </div>
                    <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                        <p class="h_d1">
                            <input name="Data54" type="checkbox" value="1" />黑光
                         <input name="Data54" type="hidden" value="flag" />
                        </p>
                    </div>
                    <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                        <p class="h_d1">光照强度:</p>
                        <input name="Data55" class="easyui-textbox" style="width: 180px; height: 22px; margin-left: 5px" />
                    </div>
                </fieldset>
                <fieldset style="border: 1px solid #c3ddf4; margin-top: 3px;">
                    <legend style="font-weight: 600">后清洗</legend>
                    <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                        <p class="h_d1">
                            <input name="Data56" type="checkbox" value="1" />已做
                         <input name="Data56" type="hidden" value="flag" />
                        </p>
                    </div>
                </fieldset>
                <fieldset style="border: 1px solid #c3ddf4; margin-top: 3px;">
                    <legend style="font-weight: 600">清洗剂</legend>
                    <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                        <p class="h_d1">
                            <input name="Data57" type="checkbox" value="1" />水
                         <input name="Data57" type="hidden" value="flag" />
                        </p>
                    </div>
                    <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                        <p class="h_d1">
                            <input name="Data58" type="checkbox" value="1" />丙酮
                         <input name="Data58" type="hidden" value="flag" />
                        </p>
                    </div>
                    <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                        <p class="h_d1">
                            <input name="Data59" type="checkbox" value="1" />威第尔
                         <input name="Data59" type="hidden" value="flag" />
                        </p>
                    </div>
                    <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                        <p class="h_d1">其他:</p>
                        <input name="Data60" class="easyui-textbox" style="width: 180px; height: 22px; margin-left: 5px" />
                    </div>
                </fieldset>
            </fieldset>
        </form>
        <%-- 27 RT-射线检验报告1 --%>
        <form id="form_27">
            <%-- <fieldset style="border: 1px solid #c3ddf4; margin-top: 3px;">
                <legend style="font-weight: 600">检验条件</legend>
                <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                    <input type="hidden" value="仪器,照度计/黑光强度计" name="equipment_name_R" />
                    <p class="h_d1">仪器:</p>
                    <input class="easyui-combobox Daq_System Device_test" style="width: 180px; height: 22px; margin-left: 5px" />
                </div>
                <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                    <p class="h_d1">照度计/黑光强度计:</p>
                    <input class="easyui-combobox Scanner Device_test" style="width: 180px; height: 22px; margin-left: 5px" />
                </div>
                <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                    <a href="#" class="easyui-linkbutton Probe_test" data-options="iconCls:'icon-add',plain:true">探头</a>
                </div>
            </fieldset>--%>
            <fieldset style="border: 1px solid #c3ddf4; margin-top: 3px;">
                <legend style="font-weight: 600">象质计</legend>
                <fieldset style="border: 1px solid #c3ddf4; margin-top: 3px;">
                    <legend style="font-weight: 600">类型</legend>
                    <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                        <p class="h_d1">
                            <input name="Data1" type="checkbox" value="1" />孔型
                         <input name="Data1" type="hidden" value="flag" />
                        </p>
                    </div>
                    <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                        <p class="h_d1">
                            <input name="Data2" type="checkbox" value="1" />线型
                         <input name="Data2" type="hidden" value="flag" />
                        </p>
                    </div>
                    <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                        <p class="h_d1">编号:</p>
                        <input name="Data4" class="easyui-textbox" style="width: 180px; height: 22px; margin-left: 5px" />
                    </div>
                </fieldset>
                <fieldset style="border: 1px solid #c3ddf4; margin-top: 3px;">
                    <legend style="font-weight: 600">位置</legend>
                    <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                        <p class="h_d1">
                            <input name="Data5" type="checkbox" value="1" />源侧
                         <input name="Data5" type="hidden" value="flag" />
                        </p>
                    </div>
                    <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                        <p class="h_d1">
                            <input name="Data6" type="checkbox" value="1" />片侧
                         <input name="Data6" type="hidden" value="flag" />
                        </p>
                    </div>

                    <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                        <p class="h_d1">须见孔(钢丝)直径: </p>
                        <input name="Data7" class="easyui-textbox" style="width: 180px; height: 22px; margin-left: 5px" />
                    </div>
                </fieldset>
            </fieldset>
            <fieldset style="border: 1px solid #c3ddf4; margin-top: 3px;">
                <legend style="font-weight: 600">胶片</legend>
                <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                    <p class="h_d1">胶片类型和名称:</p>
                    <input name="Data8" class="easyui-textbox" style="width: 180px; height: 22px; margin-left: 5px" />
                </div>
                <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                    <p class="h_d1">胶片尺寸:</p>
                    <input name="Data9" class="easyui-textbox" style="width: 180px; height: 22px; margin-left: 5px" />
                </div>
                <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                    <p class="h_d1">一个暗盒的数量:</p>
                    <input name="Data10" class="easyui-textbox" style="width: 180px; height: 22px; margin-left: 5px" />
                </div>
            </fieldset>
            <fieldset style="border: 1px solid #c3ddf4; margin-top: 3px;">
                <legend style="font-weight: 600">增感屏</legend>
                <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                    <p class="h_d1" style="width: 80px; text-align: right;">
                        增感屏材料:
                    </p>
                    <input name="Data11" class="easyui-textbox" style="width: 180px; height: 22px; margin-left: 5px" />
                </div>
                <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                    <p class="h_d1" style="width: 80px; text-align: right;">
                        前屏:
                    </p>
                    <input name="Data12" class="easyui-textbox" style="width: 180px; height: 22px; margin-left: 5px" />
                </div>
                <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                    <p class="h_d1" style="width: 80px; text-align: right;">
                        中屏:
                    </p>
                    <input name="Data13" class="easyui-textbox" style="width: 180px; height: 22px; margin-left: 5px" />
                </div>
                <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                    <p class="h_d1" style="width: 80px; text-align: right;">后屏:</p>
                    <input name="Data14" class="easyui-textbox" style="width: 180px; height: 22px; margin-left: 5px" />
                </div>
                <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                    <p class="h_d1" style="width: 80px; text-align: right;">滤光片材料:</p>
                    <input name="Data15" class="easyui-textbox" style="width: 180px; height: 22px; margin-left: 5px" />
                </div>
                <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                    <p class="h_d1" style="width: 80px; text-align: right;">前滤光板厚度:</p>
                    <input name="Data16" class="easyui-textbox" style="width: 180px; height: 22px; margin-left: 5px" />
                </div>

                <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                    <p class="h_d1" style="width: 80px; text-align: right;">后挡板材料: </p>
                    <input name="Data17" class="easyui-textbox" style="width: 180px; height: 22px; margin-left: 5px" />
                </div>
                <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                    <p class="h_d1" style="width: 80px; text-align: right;">后挡板厚度: </p>
                    <input name="Data18" class="easyui-textbox" style="width: 180px; height: 22px; margin-left: 5px" />
                </div>
            </fieldset>
            <fieldset style="border: 1px solid #c3ddf4; margin-top: 3px;">
                <legend style="font-weight: 600">透照方式</legend>
                <fieldset style="border: 1px solid #c3ddf4; margin-top: 3px;">
                    <legend style="font-weight: 600">X射线</legend>
                    <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                        <p class="h_d1" style="width: 80px; text-align: right;">设备型号:</p>
                        <input name="Data20" class="easyui-textbox" style="width: 180px; height: 22px; margin-left: 5px" />
                    </div>
                    <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                        <p class="h_d1" style="width: 80px; text-align: right;">管电压:</p>
                        <input name="Data21" class="easyui-textbox" style="width: 180px; height: 22px; margin-left: 5px" />
                    </div>
                    <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                        <p class="h_d1" style="width: 80px; text-align: right;">管电流:</p>
                        <input name="Data22" class="easyui-textbox" style="width: 180px; height: 22px; margin-left: 5px" />
                    </div>

                    <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                        <p class="h_d1" style="width: 80px; text-align: right;">焦点:</p>
                        <input name="Data23" class="easyui-textbox" style="width: 180px; height: 22px; margin-left: 5px" />
                    </div>
                    <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                        <p class="h_d1" style="width: 80px; text-align: right;">焦物距:</p>
                        <input name="Data24" class="easyui-textbox" style="width: 180px; height: 22px; margin-left: 5px" />
                    </div>
                    <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                        <p class="h_d1" style="width: 80px; text-align: right;">源测工件: </p>
                        <input name="Data25" class="easyui-textbox" style="width: 180px; height: 22px; margin-left: 5px" />
                    </div>
                    <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                        <p class="h_d1" style="width: 80px; text-align: right;">曝光时间:</p>
                        <input name="Data26" class="easyui-textbox" style="width: 180px; height: 22px; margin-left: 5px" />
                    </div>
                </fieldset>
                <fieldset style="border: 1px solid #c3ddf4; margin-top: 3px;">
                    <legend style="font-weight: 600">直线加速器</legend>
                    <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                        <p class="h_d1" style="width: 80px; text-align: right;">设备型号:</p>
                        <input name="Data47" class="easyui-textbox" style="width: 180px; height: 22px; margin-left: 5px" />
                    </div>
                    <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                        <p class="h_d1" style="width: 80px; text-align: right;">能量:</p>
                        <input name="Data48" class="easyui-textbox" style="width: 180px; height: 22px; margin-left: 5px" />
                    </div>
                    <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                        <p class="h_d1" style="width: 80px; text-align: right;">曝光量:</p>
                        <input name="Data60" class="easyui-textbox" style="width: 180px; height: 22px; margin-left: 5px" />
                    </div>
                    <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                        <p class="h_d1" style="width: 80px; text-align: right;">焦点:</p>
                        <input name="Data49" class="easyui-textbox" style="width: 180px; height: 22px; margin-left: 5px" />
                    </div>
                    <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                        <p class="h_d1" style="width: 80px; text-align: right;">焦物距:</p>
                        <input name="Data50" class="easyui-textbox" style="width: 180px; height: 22px; margin-left: 5px" />
                    </div>
                    <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                        <p class="h_d1" style="width: 80px; text-align: right;">源测工件至胶片距离d:</p>
                        <input name="Data51" class="easyui-textbox" style="width: 180px; height: 22px; margin-left: 5px" />
                    </div>
                    <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                        <p class="h_d1" style="width: 80px; text-align: right;">曝光时间: </p>
                        <input name="Data52" class="easyui-textbox" style="width: 180px; height: 22px; margin-left: 5px" />
                    </div>

                </fieldset>
                <fieldset style="border: 1px solid #c3ddf4; margin-top: 3px;">
                    <legend style="font-weight: 600">γ-射线源</legend>
                    <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                        <p class="h_d1">
                            <input name="Data53" type="checkbox" value="1" />Co60
                         <input name="Data53" type="hidden" value="flag" />
                        </p>
                    </div>
                    <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                        <p class="h_d1">
                            <input name="Data54" type="checkbox" value="1" />Ir192
                             <input name="Data54" type="hidden" value="flag" />
                        </p>
                    </div>
                    <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                        <p class="h_d1">活度:</p>
                        <input name="Data55" class="easyui-textbox" style="width: 180px; height: 22px; margin-left: 5px" />
                    </div>
                    <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                        <p class="h_d1">焦点:</p>
                        <input name="Data56" class="easyui-textbox" style="width: 180px; height: 22px; margin-left: 5px" />
                    </div>

                    <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                        <p class="h_d1">焦物距:</p>
                        <input name="Data57" class="easyui-textbox" style="width: 180px; height: 22px; margin-left: 5px" />
                    </div>
                    <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                        <p class="h_d1">源测工件至胶片距离d: </p>
                        <input name="Data58" class="easyui-textbox" style="width: 180px; height: 22px; margin-left: 5px" />
                    </div>
                    <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                        <p class="h_d1">曝光时间:</p>
                        <input name="Data59" class="easyui-textbox" style="width: 180px; height: 22px; margin-left: 5px" />
                    </div>
                </fieldset>
                <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                    <p class="h_d1">
                        <input name="Data27" type="checkbox" value="1" />单壁
                         <input name="Data27" type="hidden" value="flag" />
                    </p>
                </div>
                <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                    <p class="h_d1">
                        <input name="Data28" type="checkbox" value="1" />双壁
                         <input name="Data28" type="hidden" value="flag" />
                    </p>
                </div>
                <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                    <p class="h_d1">
                        <input name="Data29" type="checkbox" value="1" />分段
                         <input name="Data29" type="hidden" value="flag" />
                    </p>
                </div>
                <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                    <p class="h_d1">
                        <input name="Data30" type="checkbox" value="1" />周向
                         <input name="Data30" type="hidden" value="flag" />
                    </p>
                </div>
                <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                    <p class="h_d1">
                        <input name="Data31" type="checkbox" value="1" />内照
                         <input name="Data31" type="hidden" value="flag" />
                    </p>
                </div>
                <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                    <p class="h_d1">
                        <input name="Data32" type="checkbox" value="1" />外照
                         <input name="Data32" type="hidden" value="flag" />
                    </p>
                </div>
                <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                    <p class="h_d1">透照次数:</p>
                    <input name="Data33" class="easyui-textbox" style="width: 180px; height: 22px; margin-left: 5px" />
                </div>
                <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                    <p class="h_d1">透照张数:</p>
                    <input name="Data34" class="easyui-textbox" style="width: 180px; height: 22px; margin-left: 5px" />
                </div>
            </fieldset>
            <fieldset style="border: 1px solid #c3ddf4; margin-top: 3px;">
                <legend style="font-weight: 600">手工胶片处理</legend>
                <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                    <p class="h_d1">显影时间:</p>
                    <input name="Data35" class="easyui-textbox" style="width: 180px; height: 22px; margin-left: 5px" />
                </div>
                <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                    <p class="h_d1">定影时间</p>
                    <input name="Data36" class="easyui-textbox" style="width: 180px; height: 22px; margin-left: 5px" />
                </div>
                <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                    <p class="h_d1">水洗时间:</p>
                    <input name="Data37" class="easyui-textbox" style="width: 180px; height: 22px; margin-left: 5px" />
                </div>
                <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                    <p class="h_d1">显影温度:</p>
                    <input name="Data38" class="easyui-textbox" style="width: 180px; height: 22px; margin-left: 5px" />
                </div>
                <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                    <p class="h_d1">定影温度:</p>
                    <input name="Data39" class="easyui-textbox" style="width: 180px; height: 22px; margin-left: 5px" />
                </div>
                <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                    <p class="h_d1">水洗温度:</p>
                    <input name="Data40" class="easyui-textbox" style="width: 180px; height: 22px; margin-left: 5px" />
                </div>
            </fieldset>
            <fieldset style="border: 1px solid #c3ddf4; margin-top: 3px;">
                <legend style="font-weight: 600">自动胶片处理</legend>
                <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                    <p class="h_d1" style="width: 80px; text-align: right;">设备制造商:</p>
                    <input name="Data41" class="easyui-textbox" style="width: 180px; height: 22px; margin-left: 5px" />
                </div>
                <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                    <p class="h_d1" style="width: 80px; text-align: right;">显影液牌号: </p>
                    <input name="Data42" class="easyui-textbox" style="width: 180px; height: 22px; margin-left: 5px" />
                </div>
                <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                    <p class="h_d1" style="width: 80px; text-align: right;">冲洗速度:</p>
                    <input name="Data43" class="easyui-textbox" style="width: 180px; height: 22px; margin-left: 5px" />
                </div>
                <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                    <p class="h_d1" style="width: 80px; text-align: right;">设备型号:</p>
                    <input name="Data44" class="easyui-textbox" style="width: 180px; height: 22px; margin-left: 5px" />
                </div>
                <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                    <p class="h_d1" style="width: 80px; text-align: right;">定影液牌号:</p>
                    <input name="Data45" class="easyui-textbox" style="width: 180px; height: 22px; margin-left: 5px" />
                </div>
                <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                    <p class="h_d1" style="width: 80px; text-align: right;">显、定温度:</p>
                    <input name="Data46" class="easyui-textbox" style="width: 180px; height: 22px; margin-left: 5px" />
                </div>
            </fieldset>
        </form>

        <a href="<%=PageOffice.PageOfficeLink.OpenWindow("/mainform/read_contract.aspx?","width=1000px ;height=740px;")%>" id="read_doc" class="easyui-linkbutton" data-options="iconCls:'icon-Document',plain:true">阅读文件</a>

        <a href="<%=PageOffice.PageOfficeLink.OpenWindow("/mainform/Revisionsshow.aspx?save_type=Lossless_report_1&ReturnNode=EditUpdate&","width=1000px;height=740px;")%>" id="read_doc1" class="easyui-linkbutton" data-options="iconCls:'icon-read',plain:true"><span id="read_doc_">阅读</span>></a>
        <a href="<%=PageOffice.PageOfficeLink.OpenWindow("/mainform/Lossless_report_word_edit.aspx?report_state=edit&save_type=Lossless_report_&","width=1000px ;height=700px;")%>" id="reports_edit_1" class="easyui-linkbutton" data-options="iconCls:'icon-edit',plain:true">编制这份证书1</a>
        <a href="<%=PageOffice.PageOfficeLink.OpenWindow("/mainform/read_contract.aspx?","width=1000px ;height=740px;")%>" id="read_records" class="easyui-linkbutton" data-options="iconCls:'icon-Document'" style="margin-left: 40px">查看检测记录</a>

    </div>

    <%-- 隐藏 --%>
    <div style="display: none">
        <form id="S_dialog" runat="server">
            <%--//消息类型--%>
            <asp:HiddenField ID="message_type" runat="server" />
            <%-- 隐藏的消息提示按钮 --%>
            <div style="display: none"><a id="test_message" href="#" class="easyui-linkbutton" data-options="iconCls:'icon-edit'">测试消息</a>    </div>
            <%-- 登录账户 --%>
            <asp:HiddenField ID="user_count" runat="server" />
            <%-- 1代表存在；0代表不存在 --%>
            <asp:HiddenField ID="contract_sate" runat="server" Value="0" />
            <asp:HiddenField ID="contract_url" runat="server" Value="0" />
            <%-- 预览证书：为“预览证书”时禁用一切写入操作；为“0”时恢复 --%>
            <asp:HiddenField ID="view_word_sate" runat="server" Value="0" />
        </form>
    </div>

</body>
<%=WebExtensions.CombresLink("LosslessReport_Edit_Js") %>
</html>
