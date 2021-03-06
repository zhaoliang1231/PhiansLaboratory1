﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PageShowSetting.aspx.cs" Inherits="phians.mainform.System_settings.PageShowSetting1" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
   <%= WebExtensions.CombresLink("public_Css") %>

</head>
<body>
    <div id="PageSettingDatagrid"></div>
    <div style="display: none">
        <div id="message_show_toolbar">
            选择页面：
            <input id="Page" style="width: 150px" class="easyui-combobox" />
            <a href="#" id="PageSettingAdd" class="easyui-linkbutton" data-options="iconCls:'icon-edit',plain:true">修改</a>
            <span class="datagrid-btn-separator" style="vertical-align: middle; height: 22px; display: inline-block; float: none"></span>
<%--            <input id="search2" style="width: 150px" class="easyui-combobox" />--%>
<%--            <input id="search3" style="width: 150px" class="easyui-textbox" />--%>
<%--            <a href="#" id="report_search" class="easyui-linkbutton" data-options="iconCls:'icon-search',plain:true">搜索</a>--%>
        </div>
        <!--<%-- 查看 --%>-->
        <form id="S1_dialog">
            <div title="修改">
                <div class="h_dline" style="height: 23px; margin-top: 20px; margin-left: 10px">
                    <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px;">
                        <p class="h_d1" style="width: 100px; text-align: right;">字段名字:</p>
                        <input id="Title" name="Title" class="easyui-textbox" style="width: 400px; height: 22px; margin-left: 5px" />
                    </div>
                <%--    <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px; ">
                        <p class="h_d1" style="width: 100px; text-align: right;">字段显示顺序:</p>
                        <input id="FieldSort" name="FieldSort" class="easyui-textbox" style="width: 180px; height: 22px; margin-left: 5px" />
                    </div>--%>
                    <div class="" style="float: left; margin-top: 10px; margin-left: 10px">
                        <label class="h_d1" style="display:inline-block;width: 100px;text-align:right;">字段是否排序:</label>
                        <input id="Sortable1" type="checkbox" value="1" /><label for="Sortable1" style="color: #f00;">是</label>
                        <input id="Sortable2" type="checkbox" value="0" checked="checked" /><label for="Sortable2" style="color: #f00;">否</label>
                        <input type="hidden" name="Sortable" value="0" id="Sortable" />
                    </div>
                    <div class="" style="float: left; margin-top: 10px; margin-left: 10px">
                        <label class="h_d1" style="display:inline-block;width: 80px;text-align:right;">是否显示:</label>
                        <input id="hidden1" type="checkbox" value="1" /><label for="hidden1" style="color: #f00;">是</label>
                        <input id="hidden2" type="checkbox" value="0" checked="checked" /><label for="hidden2" style="color: #f00;">否</label>
                        <input type="hidden" name="hidden" value="0" id="hidden" />
                    </div>
                    <div class="h_dline" style="height: 23px; margin-top: 10px; margin-left: 10px; float: left">
                        <p class="h_d1" style="width: 100px; text-align: right;">附注:</p>
                        <input id="remarks" name="Remark" class="easyui-textbox" data-options="multiline:true" style="width: 400px; height: 66px; margin-left: 5px" />
                    </div>
                </div>
            </div>
        </form>
    </div>

   
</body>
    
       <%= WebExtensions.CombresLink("PageShowSetting_Js") %>

</html>

