<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="System_log.aspx.cs" Inherits="phians.mainform.System_settings.System_log" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
   <%= WebExtensions.CombresLink("public_Css") %>

</head>
<body>
    <div id="System_log"></div>
    <div style="display: none">
        <div id="message_show_toolbar">
            <a href="#" id="click_ok" class="easyui-linkbutton" data-options="iconCls:'icon-Document',plain:true">查看</a>
            <span class="datagrid-btn-separator" style="vertical-align: middle; height: 22px; display: inline-block; float: none"></span>
            <input id="search2" style="width: 150px" class="easyui-combobox" />
            <input id="search3" style="width: 150px" class="easyui-textbox" />
            <a href="#" id="report_search" class="easyui-linkbutton" data-options="iconCls:'icon-search',plain:true">搜索</a>
        </div>
    </div>

    <!--<%-- 查看 --%>-->
    <form id="S1_dialog">
        <div title="查看">
            <div class="h_dline" style="height: 23px; margin-top: 20px; margin-left: 10px">
                <div class="h_dline" style="height: 23px; margin-top: 8px;">
                    <label class="h_d1" style="display:inline-block;width: 80px;text-align:right;">登录用户:</label>
                    <input id="edit_type" name="operation_user" class="easyui-textbox" style="width: 180px; height: 22px; margin-left: 5px" />
                </div>
                <div class="h_dline" style="height: 23px; margin-top: 8px;">
                    <label class="h_d1" style="display:inline-block;width: 80px;text-align:right;">登录用户名:</label>
                    <input id="Text10" name="operation_name" class="easyui-textbox" style="width: 180px; height: 22px; margin-left: 5px" />
                </div>
                <div class="h_dline" style="height: 23px; margin-top: 8px;">
                    <label class="h_d1" style="display:inline-block;width: 80px;text-align:right;">登录ip:</label>
                    <input id="Text12" name="operation_ip" class="easyui-textbox" style="width: 180px; height: 22px; margin-left: 5px" />
                </div>
                <div class="h_dline" style="height: 23px; margin-top: 8px;">
                    <label class="h_d1" style="display:inline-block;width: 80px;text-align:right;">操作时间:</label>
                    <input id="Text11" name="operation_date" class="easyui-textbox" style="width: 180px; height: 22px; margin-left: 5px" />
                </div>
                <div class="h_dline" style="height: 23px; margin-top: 8px;">
                    <label class="h_d1" style="display:inline-block;width: 80px;text-align:right;">操作类别:</label>
                    <input id="Text14" name="operation_type" class="easyui-textbox" style="width: 180px; height: 22px; margin-left: 5px" />
                </div>
                <div class="h_dline" style="height: 46px; margin-top: 8px;">
                    <label class="h_d1" style="display:inline-block;width: 80px;text-align:right;">操作内容:</label>
                    <input id="Text13" name="operation_info" class="easyui-textbox" data-options="multiline:true" style="width: 180px; height: 44px; margin-left: 5px" />
                </div>
                <div class="h_dline" style="height: 46px; margin-top: 8px;">
                    <label class="h_d1" style="display:inline-block;width: 80px;text-align:right;">附注:</label>
                    <input id="Text15" name="remarks" class="easyui-textbox" data-options="multiline:true" style="width: 180px; height: 44px; margin-left: 5px" />
                </div>

            </div>
        </div>
    </form>
</body>
    
       <%= WebExtensions.CombresLink("System_log_Js") %>

</html>

