﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Device_Library.aspx.cs" Inherits="phians.mainform.Lossless_report.Device_Library" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
       <%=WebExtensions.CombresLink("public_Css") %>

</head>
<body>

    <!--<%--设备库数据表格--%>-->
    <div id="Device_Library_datagrid"></div>

    <div style="display: none">

        <!-- 台账资料维护工具栏 -->
        <div id="Device_Library_datagrid_toolbar">
            <a href="#" id="_add" class="easyui-linkbutton" data-options="iconCls:'icon-add',plain:true">增加信息</a>
            <span class="datagrid-btn-separator" style="vertical-align: middle; height: 22px; display: inline-block; float: none"></span>
            <a href="#" id="_edit" class="easyui-linkbutton" data-options="iconCls:'icon-edit',plain:true">修改信息</a>
            <span class="datagrid-btn-separator" style="vertical-align: middle; height: 22px; display: inline-block; float: none"></span>
            <a href="#" id="_delete" class="easyui-linkbutton" data-options="iconCls:'icon-remove',plain:true">删除信息</a>
            <span class="datagrid-btn-separator" style="vertical-align: middle; height: 22px; display: inline-block; float: none"></span>
            <input id="search" style="width: 150px" class="easyui-combobox" />
            <label>为 </label>
            <input id="key" style="width: 150px" class="easyui-textbox" />
            <a href="#" id="maintenance_search" class="easyui-linkbutton" data-options="iconCls:'icon-search',plain:true">搜索</a>
        </div>
        <!--<%--dialog修改 设备库 资料--%>-->
        <form id="AddEidt_info">
            <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                <p class="h_d1" style="width: 100px; text-align: right;">设备名称:</p>
                <input id="equipment_nem" name="equipment_nem" required="required" class="easyui-textbox" style="width: 180px; height: 22px; margin-left: 5px" />
            </div>
            <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                <p class="h_d1" style="width: 100px; text-align: right;">设备型号:</p>
                <input id="equipment_Type" name="equipment_Type" required="required" class="easyui-textbox" style="width: 180px; height: 22px; margin-left: 5px" />
            </div>
            <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                <p class="h_d1" style="width: 100px; text-align: right;">设备编号:</p>
                <input id="equipment_num" name="equipment_num" class="easyui-textbox" style="width: 180px; height: 22px; margin-left: 5px" />
            </div>
            <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                <p class="h_d1" style="width: 100px; text-align: right;">制造商:</p>
                <input id="Manufacture" name="Manufacture" class="easyui-textbox" style="width: 180px; height: 22px; margin-left: 5px" />
            </div>
            <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                <p class="h_d1" style="width: 100px; text-align: right;">测量范围:</p>
                <input id="range_" name="range_" class="easyui-textbox" style="width: 180px; height: 22px; margin-left: 5px" />
            </div>
            <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                <p class="h_d1" style="width: 100px; text-align: right;">有效期:</p>
                <input id="effective" name="effective" class="easyui-datebox" style="width: 180px; height: 22px; margin-left: 5px" />
            </div>
            <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                <p class="h_d1" style="width: 100px; text-align: right;">状态:</p>
                <input id="E_state" name="E_state" class="easyui-combobox" style="width: 180px; height: 22px; margin-left: 5px" />
            </div>
            <div class="h_dline" style="height: 23px; margin-top: 10px; margin-left: 10px; float: left">
                <p class="h_d1" style="width: 100px; text-align: right;">附注:</p>
                <input id="remarks" name="remarks" class="easyui-textbox" data-options="multiline:true" style="width: 400px; height: 66px; margin-left: 5px" />
            </div>
        </form>

    </div>

</body>
        <%=WebExtensions.CombresLink("Device_Library_Js") %>

</html>
