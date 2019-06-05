<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Dictionary_management.aspx.cs" Inherits="phians.mainform.System_settings.WebForm2" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>

       <%= WebExtensions.CombresLink("public_Css") %>
</head>
<body>
        <div id="department_layout" class="easyui-layout" data-options="fit:true,border:false">
        <!--<%-- 部门树 --%>-->
        <div  data-options="region:'west',border:false">
           <div id="project_info"  title="数据字典管理" >
                </div>
                    <div class="easyui-menu" id="keyMenu" style="display: none;">
                        <div id="tree_edit" data-options="iconCls:'icon-edit'">修改节点</div>
                        <div id="tree_add" data-options="iconCls:'icon-add'">
                            添加同级节点
                        </div>
                        <div id="tree_add_next" data-options="iconCls:'icon-add'">
                            添加下级节点
                        </div>
                       <%-- <div id="tree_del" data-options="iconCls:'icon-remove'">
                            删除节点
                        </div>--%>
                    </div>
        </div>
        <div data-options="region:'east', border:false">
           
                 
                  <div id="standards_info" style="margin-top: 0px">
                                   
                </div>
        </div>
    </div>
 
   

    <div style ="display:none">
      <div id="tree_add_dialog">
        <div class="h_d" style="height: 50px; margin-top: 8px; margin-left: 15px">
            <p class="h_d1" style="width: 80px">添加项目名称:</p>
            <input id="tree_add_dialog_input" class="h_d1" name="" style="width: 200px;" />
        </div>
     </div>
    
        <div id="standards_info_toolbar">
            <a href="#" id="click_edit" class="easyui-linkbutton" data-options="iconCls:'icon-edit',plain:true">修改</a>
                <span class="datagrid-btn-separator" style="vertical-align: middle; height: 22px; display: inline-block; float: none"></span>
            <a href="#" id="click_add" class="easyui-linkbutton" data-options="iconCls:'icon-add',plain:true">添加</a>
                <span class="datagrid-btn-separator" style="vertical-align: middle; height: 22px; display: inline-block; float: none"></span>
            <a href="#" id="click_del" class="easyui-linkbutton" data-options="iconCls:'icon-remove',plain:true">删除</a>
        </div>
         <form id="dictionary_dialog">
            <!--<%-- 添加修改字典内容 --%>-->
                 <div class="h_dline" style="height: 23px; margin-top: 10px; margin-left: 42px; float: left">
                    <p class="h_d1" style="width: 60px;text-align:right;">项目内容:</p>&nbsp;
                    <input id="Project_name" name="Project_name" class="easyui-textbox" required="required" style="width: 150px; height: 22px; margin-left: 5px" />
                </div> 
                <br />
                 <div class="h_dline" style="height: 23px; margin-top: 10px; margin-left: 42px; float: left">
                    <p class="h_d1" style="width: 60px;text-align:right;">项目值:</p>&nbsp;
                    <input id="Project_value" name="Project_value" class="easyui-textbox" required="required" style="width: 150px; height: 22px; margin-left: 5px" />
                </div>  
                 <br />
                 <div class="h_dline" style="height: 23px; margin-top: 10px; margin-left: 42px; float: left">
                    <p class="h_d1" style="width: 60px; text-align:right;">排序:</p>&nbsp;
                    <input id="Sort_num" name="Sort_num" class="easyui-textbox" style="width: 150px; height: 22px; margin-left: 5px" />
                </div>    
                 <div class="h_dline" style="height: 23px; margin-top:20px; margin-left:42px; float: left">
                    <p class="h_d1" style="width: 60px;text-align:right;">说明:</p>&nbsp;
                    <input id="remarks" name="remarks" class="easyui-textbox" data-options="multiline:true" style="width: 150px; height: 66px; margin-left: 5px" />
                </div>     
        </form>
    </div>
</body>


     <%= WebExtensions.CombresLink("Dictionary_management_Js") %>
</html>

