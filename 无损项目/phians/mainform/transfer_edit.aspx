<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="transfer_edit.aspx.cs" Inherits="phians.mainform.transfer_edit" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <%= WebExtensions.CombresLink("transfer_edit_Js") %>
    <%= WebExtensions.CombresLink("report_procedure_Css") %>
</head>
<script type="text/javascript">
    $(function () {
        //转交报告编制人员
        $('#man').combobox({
            url: "../mainform/report_procedure.ashx?&cmd=load_edit_man",
            valueField: 'id',
            textField: 'User_name',
            //required: true,
            //本地联系人数据模糊索引
            filter: function (q, row) {
                var opts = $(this).combobox('options');
                return row[opts.textField].indexOf(q) >= 0;
            }
        });
        //确定
        $('#OK').click(function () {
            var man = $('#man').combobox('getText');
            //alert(man);
            var id = art.dialog.data('report_edit_task_id');
            $.ajax({
                url: "report_procedure.ashx?&cmd=transfer_edit",
                dataType: "text",
                type: 'POST',
                data: {
                    id: id,
                    constitute_personnel : man
                },
                success: function (data) {
                    $.messager.confirm('任务提示', '转交成功！', function (r) {
                        art.dialog.close();
                    });
                    //var win = art.dialog.open.origin;
                    //win.location.reload(); // 注意：如果父页面重载或者关闭其子对话框全部会关闭
                    //return false;
                }
            });
        });
    });
</script>
<body>
      <div id="aa" style="margin-top:20px">
        <form id="R_dialog">
            <div title="请选择转交对象">
                <label>请选择转交对象：</label><input id="man" />
                 <a href="#"  id="OK" class="easyui-linkbutton" data-options="iconCls:'icon-edit',plain:true">  确定</a>   
            </div>
        </form>
    </div>
</body>
</html>
