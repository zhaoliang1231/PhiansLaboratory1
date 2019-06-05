<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LosslessReport_EditApply_Edit.aspx.cs" Inherits="phians.mainform.Lossless_report.WebForm1" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <%=WebExtensions.CombresLink("public_Css") %>
</head>
<body>
    <div id="tt" class="easyui-panel" data-options="fit:true,border:false" style=" margin-left: 2px; overflow: scroll; overflow-x: hidden; overflow-y: hidden">
        <div id="report_edit_task">
            <!--<%-- 检测报告编制-报告信息-工具栏 --%>-->
            <div id="reports_toolbar">

                <span style="display: inline-block; float: left;">
                    <label>管理选择：</label><a href="#" id="management_all" style="width: 100px" class="easyui-combobox"></a></span>
                <span class="datagrid-btn-separator" style="vertical-align: middle; height: 22px; display: inline-block; float: none"></span>
                 <a href="#" id="Preview_Report" class="easyui-linkbutton" data-options="iconCls:'icon-Document',plain:true">预览报告</a>
                <span class="datagrid-btn-separator" style="vertical-align: middle; height: 22px; display: inline-block; float: none"></span>
                <a href="#" id="Edit_online_report" class="easyui-linkbutton" data-options="iconCls:'icon-edit',plain:true">编制这份报告</a>
                <span class="datagrid-btn-separator" style="vertical-align: middle; height: 22px; display: inline-block; float: none"></span>
                <a href="#" id="view_return_info" class="easyui-linkbutton" data-options="iconCls:'icon-read',plain:true">查看申请原因</a>
                <span class="datagrid-btn-separator" style="vertical-align: middle; height: 22px; display: inline-block; float: none"></span>
                <a href="#" id="edit_time" class="easyui-linkbutton" data-options="iconCls:'icon-edit',plain:true">修改时间</a>
                <span class="datagrid-btn-separator" style="vertical-align: middle; height: 22px; display: inline-block; float: none"></span>
                <a href="#" id="Submit_report" class="easyui-linkbutton" data-options="iconCls:'icon-ok',plain:true">提交审核</a>
                <span class="datagrid-btn-separator" style="vertical-align: middle; height: 22px; display: inline-block; float: none"></span>
                <input id="search" style="width: 150px" class="easyui-combobox" />
                <input id="search1" style="width: 150px" class="easyui-textbox" />
                <a href="#" id="search_info" class="easyui-linkbutton" data-options="iconCls:'icon-search',plain:true">搜索</a>
            </div>
        </div>
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
        <form id="Back_report_dialog">
            <div style="margin-top: 5px; text-align: right; margin-right: 20px">
                <label style="display: inline-block; width: 80px">退回原因：</label><input id="return_info" name="return_info" data-options="multiline:true" class="easyui-textbox" style="width: 300px; height: 100px;" />
            </div>
            <div style="margin-top: 5px; text-align: right; margin-right: 20px">
                <label style="display: inline-block; width: 80px">说明：</label><input id="remarks" name="return_info" data-options="multiline:true" class="easyui-textbox" style="width: 300px; height: 100px;" />
            </div>
        </form>

          <form id="importFileForm1">
            <div class="h_dline" style="height: 23px; margin-top: 10px; margin-left: 0px; float: left">
                <p class="h_d1" style="width: 60px">组:</p>
                <input id="group" name="" class="easyui-combobox" style="width: 150px; height: 22px; margin-left: 5px" />
            </div>
             <div class="h_dline" style="height: 23px; margin-top: 10px; margin-left: 0px; float: left">
                <p class="h_d1" style="width: 60px">评审人:</p>
                <input id="review_personnel" name="review_personnel" class="easyui-combobox" style="width: 150px; height: 22px; margin-left: 5px" />
            </div>
         
        </form>

        <a href="<%=PageOffice.PageOfficeLink.OpenWindow("/mainform/read_contract.aspx?","width=1000px ;height=740px;")%>" id="read_doc" class="easyui-linkbutton" data-options="iconCls:'icon-read',plain:true">阅读文件</a>
        <a href="<%=PageOffice.PageOfficeLink.OpenWindow("/mainform/word_view.aspx?report_state=edit&save_type=Lossless_report_&","width=1000px ;height=740px;")%>" id="reports_edit_1" class="easyui-linkbutton" data-options="iconCls:'icon-edit',plain:true">编制这份证书1</a>
        <a href="<%=PageOffice.PageOfficeLink.OpenWindow("/mainform/read_contract.aspx?","width=1000px ;height=740px;")%>" id="read_records" class="easyui-linkbutton" data-options="iconCls:'icon-read'" style="margin-left: 40px">查看检测记录</a>
    </div>

</body>

<%=WebExtensions.CombresLink("LosslessReport_EditApply_Edit_Js") %>
</html>

