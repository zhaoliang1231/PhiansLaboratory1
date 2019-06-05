<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="functional_module.aspx.cs" Inherits="phians.mainform.functional_module" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <script type="text/javascript" src="../easyui/jquery.min.js"></script>
    <script type="text/javascript" src="../easyui/jquery.easyui.min.js"></script>
    <script type="text/javascript" src="../easyui/locale/easyui-lang-zh_CN.js"></script>
    <script type="text/javascript" src="../phians_js/functional_module.js"></script>
    <link rel="stylesheet" type="text/css" href="../easyui/themes/default/easyui.css" />
    <link rel="stylesheet" type="text/css" href="../easyui/themes/icon.css" />
</head>
<body>
    <%-- 详细权限 --%>
    <form id="dialog_Permissions">
        <div style="float: left">
            <div>
                <div id="page_info" class="easyui-panel" title="页面信息" style="width: 400px; height: 1000px;">
                </div>
                <div class="easyui-menu" id="keyMenu" style="display: none;">
                    <div id="tree_edit" data-options="iconCls:'icon-edit'">修改节点</div>
                    <div id="tree_add" data-options="iconCls:'icon-add'">
                        添加同级节点
                    </div>
                    <div id="tree_add_next" data-options="iconCls:'icon-add'">
                        添加下级节点
                    </div>
                    <div id="tree_del" data-options="iconCls:'icon-remove'">
                        删除节点
                    </div>
                </div>
            </div>
        </div>
    </form>

    <%-- 添加项目对话框 --%>
    <div style="display: none">
        <div id="tree_add_dialog">
            <div class="h_d" style="margin-top: 8px; margin-left: 15px">
                <p class="h_d1" style="width: 80px">页面名称:</p>
                <input id="m_name" class="easyui-textbox" name="m_name" style="width: 200px;" />
                <p class="h_d1" style="width: 80px">图标:</p>
                <input id="i_iconCls" class="easyui-textbox" name="i_iconCls" style="width: 200px;" />
                <p class="h_d1" style="width: 80px">链接:</p>
                <input id="u_url" class="easyui-textbox" name="u_url" style="width: 200px;" />
                <p class="h_d1" style="width: 80px">附注:</p>
                <input id="remarks" class="easyui-textbox" name="remarks" style="width: 200px;" />
            </div>
        </div>
    </div>
</body>
</html>
