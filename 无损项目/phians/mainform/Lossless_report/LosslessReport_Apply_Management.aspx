<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LosslessReport_Apply_Management.aspx.cs" Inherits="phians.mainform.Lossless_report.LosslessReport_Apply_Management" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>

   <%=WebExtensions.CombresLink("public_Css") %>


</head>
<body>
    <div id="tt" class="easyui-panel" data-options="fit:true,border:false" style="margin-left: 2px; overflow: scroll; overflow-x: hidden; overflow-y: hidden">
        <div id="Report_management"></div>
    </div>

    <div style="display: none">
        <%-- 报告管理-工具栏 --%>
        <div id="reports_toolbar">
            <a href="#" id="read_report" class="easyui-linkbutton" data-options="iconCls:'icon-Document',plain:true">预览报告</a>
            <span class="datagrid-btn-separator" style="vertical-align: middle; height: 22px; display: inline-block; float: none"></span>
             <a href="#" id="view_return_info" class="easyui-linkbutton" data-options="iconCls:'icon-read',plain:true">查看申请原因</a> 
            <input id="search" style="width: 150px" class="easyui-combobox" />
            <input id="search1" style="width: 150px" class="easyui-textbox" />
            <a href="#" id="search_info" class="easyui-linkbutton" data-options="iconCls:'icon-search',plain:true">搜索</a>
        </div>
          <form id="Back_report_dialog" >
               <div style="margin-top:5px;text-align:right;margin-right:20px"><label style="display:inline-block;width:80px">退回原因：</label><input id="return_info" name="return_info" data-options="multiline:true" class="easyui-textbox" style="width: 300px; height: 100px;" />
           </div>
            <div style="margin-top:5px;text-align:right;margin-right:20px">
            <label  style="display:inline-block;width:80px">说明：</label><input id="remarks" name="return_info" data-options="multiline:true" class="easyui-textbox" style="width: 300px; height: 100px;" />
            </div>
        </form>
        <%-- 打印这份证书 --%>
        <a href="<%=PageOffice.PageOfficeLink.OpenWindow("/mainform/read_contract.aspx?","width=1000px ;height=740px;")%>" id="contract_read" class="easyui-linkbutton" data-options="iconCls:'icon-search'" style="margin-left: 40px;">查看合同</a>
        <a href="<%=PageOffice.PageOfficeLink.OpenWindow("/mainform/read_contract.aspx?","width=1000px ;height=740px;")%>" id="read_doc" class="easyui-linkbutton" data-options="iconCls:'icon-read',plain:true">阅读文件</a>
           <a href="<%=PageOffice.PageOfficeLink.OpenWindow("/mainform/Certificate_error_word_print.aspx?","width=1000px ;height=740px;")%>" id="Certificate_print" class="easyui-linkbutton" data-options="iconCls:'icon-read'" style="margin-left: 40px">打印证书</a>

    </div>
    
</body>
    <%= WebExtensions.CombresLink("LosslessReport_Apply_Management_Js") %>
</html>
