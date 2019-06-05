<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="message_management.aspx.cs" Inherits="phians.mainform.message_management" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <%= WebExtensions.CombresLink("mainformCss") %>
    <link rel="stylesheet" type="text/css" href="/phians_css/myStyle.css" />
</head>
<body>
    <div id="message_tabs" style="overflow: scroll; overflow-x: hidden; overflow-y: hidden;">
        <div title="未读消息">
            <div id="message_show">
            </div>
        </div>
        <div title="已读消息">
            <div id="load_message_old"></div>
        </div>
    </div>
    <div style="display: none">
        <div id="message_show_toolbar">
            <a href="#" id="click_ok" class="easyui-linkbutton" data-options="iconCls:'icon-ok',plain:true">确认消息</a>
            <span class="datagrid-btn-separator" style="vertical-align: middle; height: 22px; display: inline-block; float: none"></span>
            <a href="#" id="click_all_ok" class="easyui-linkbutton" data-options="iconCls:'icon-ok',plain:true">确认全部消息</a>
            <span class="datagrid-btn-separator" style="vertical-align: middle; height: 22px; display: inline-block; float: none"></span>
            <%--          <a href="#"id="test" class="easyui-linkbutton" data-options="iconCls:'icon-add',plain:true">test</a>   
        <span class="datagrid-btn-separator" style="vertical-align: middle; height: 22px;display:inline-block;float:none"></span>                       --%>
        </div>
    </div>
 
</body>
        <%= WebExtensions.CombresLink("message_management_Js") %>
   
</html>
