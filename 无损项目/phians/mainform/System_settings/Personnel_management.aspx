<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Personnel_management.aspx.cs" Inherits="phians.mainform.technicians_info_" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <%= WebExtensions.CombresLink("public_Css") %>
    <link rel="stylesheet" type="text/css" href="/uploadify/uploadify.css" />

</head>
<body>
    <div id="department_layout" class="easyui-layout" data-options="fit:true,border:false">
        <!--<%-- 部门树 --%>-->
        <div data-options="region:'west', border:false">
            <div id="department_info">
            </div>
            <div class="easyui-menu" id="keyMenu" style="display: none;">
                <div id="tree_add" data-options="iconCls:'icon-add'">
                    添加同级项目
                </div>
                <div id="tree_add_next" data-options="iconCls:'icon-add'">
                    添加下级项目
                </div>
                <%--<div id="tree_del" data-options="iconCls:'icon-remove'">
                    删除项目
                </div>--%>
            </div>
        </div>
        <div data-options="region:'east', border:false">
            <!--<%-- 员工列表 --%>-->

            <div id="department_people">
            </div>



        </div>
    </div>
    <div style="display: none">
        <%--工具栏--%>
        <div id="department_people_toolbar">

            <a href="#" id="read_info" class="easyui-linkbutton" data-options="iconCls:'icon-edit',plain:true">修改信息</a>
            <span class="datagrid-btn-separator" style="vertical-align: middle; height: 22px; display: inline-block; float: none"></span>
            <a href="#" id="read_Signature" class="easyui-linkbutton" data-options="iconCls:'icon-Document',plain:true">查看签名</a>
 <%--           <span class="datagrid-btn-separator" style="vertical-align: middle; height: 22px; display: inline-block; float: none"></span>
            <a href="#" id="no_department_personnel" class="easyui-linkbutton" data-options="iconCls:'icon-Document',plain:true">查看未分配人员</a>--%>
            <span class="datagrid-btn-separator" style="vertical-align: middle; height: 22px; display: inline-block; float: none"></span>
            <a href="#" id="enable_personnel" class="easyui-linkbutton" data-options="iconCls:'icon-strat',plain:true">启用</a>
            <span class="datagrid-btn-separator" style="vertical-align: middle; height: 22px; display: inline-block; float: none"></span>
            <a href="#" id="disable_personnel" class="easyui-linkbutton" data-options="iconCls:'icon-shop',plain:true">停用</a>
            <span class="datagrid-btn-separator" style="vertical-align: middle; height: 22px; display: inline-block; float: none"></span>
            <a href="#" id="Reset_password" class="easyui-linkbutton" data-options="iconCls:'icon-Set_up',plain:true">重置密码</a>
            <span class="datagrid-btn-separator" style="vertical-align: middle; height: 22px; display: inline-block; float: none"></span>
            <a id="department_people_add" href="#" class="easyui-linkbutton" data-options="iconCls:'icon-add',plain:true">添加员工</a>
            <span class="datagrid-btn-separator" style="vertical-align: middle; height: 22px; display: inline-block; float: none"></span>
            <%-- <a id="department_people_del" href="#" class="easyui-linkbutton" data-options="iconCls:'icon-remove',plain:true" >从组移除员工</a>
                        <span class="datagrid-btn-separator" style="vertical-align: middle; height: 22px; display: inline-block; float: none"></span>--%>
            <a id="add_others_department" href="#" class="easyui-linkbutton" data-options="iconCls:'icon-edit',plain:true">人员管理</a>
            <span class="datagrid-btn-separator" style="vertical-align: middle; height: 22px; display: inline-block; float: none"></span>
            <input id="search" style="width: 150px" />
            <input id="search1" style="width: 150px" />
            <span class="datagrid-btn-separator" style="vertical-align: middle; height: 22px; display: inline-block; float: none"></span>
            <a href="#" id="department_people_search" class="easyui-linkbutton" data-options="iconCls:'icon-search',plain:true">搜索</a>
        </div>
        <form class="h_d" id="department_people_info">

            <div class="h_d1" style="margin-top: 10px">
                <div class="h_d" style="height: 25px; margin-left: 15px">
                    <p class="h_d1" style="width: 60px">职务:</p>
                    <input id="User_job" class="easyui-textbox" name="User_job" style="width: 150px;" />
                    <p class="h_d1" style="width: 60px; margin-left: 50px">职责:</p>
                    <input id="User_duties" class="easyui-textbox" name="User_duties" style="width: 150px;" />
                </div>
                <div class="h_d" style="height: 25px; margin-left: 15px">
                    <p class="h_d1" style="width: 60px">座机:</p>
                    <input id="Tel" class="easyui-textbox" name="Tel" style="width: 150px;" />
                    <p class="h_d1" style="width: 60px; margin-left: 50px">手机:</p>
                    <input id="Phone" class="easyui-textbox" name="Phone" style="width: 150px;" />
                </div>
                <div class="h_d" style="height: 25px; margin-left: 15px">
                    <p class="h_d1" style="width: 60px">传真:</p>
                    <input id="Fax" class="easyui-textbox" name="Fax" style="width: 150px;" />
                    <p class="h_d1" style="width: 60px; margin-left: 50px">邮箱:</p>
                    <input id="Email" class="easyui-textbox" name="Email" style="width: 150px;" />
                </div>
                <div class="h_d" style="height: 25px; margin-left: 15px">
                    <p class="h_d1" style="width: 60px">QQ:</p>
                    <input id="QQ" class="easyui-textbox" name="QQ" style="width: 150px;" />
                    <p class="h_d1" style="width: 60px; margin-left: 50px">MSN:</p>
                    <input id="MSN" class="easyui-textbox" name="MSN" style="width: 150px;" />
                </div>
                <div class="h_d" style="height: 25px; margin-left: 15px">
                    <p class="h_d1" style="width: 60px">地址:</p>
                    <input id="Address" class="easyui-textbox" name="Address" style="width: 150px;" />
                    <p class="h_d1" style="width: 60px; margin-left: 50px">邮编:</p>
                    <input id="Postcode" class="easyui-textbox" name="Postcode" style="width: 150px;" />
                </div>
                <div class="h_d" style="height: 25px; margin-left: 0px;">
                    <p class="h_d1" style="width: 60px; text-align: center;">附注:</p>
                    <input id="Remarks" class="easyui-textbox" data-options="multiline:true" name="Remarks" style="width: 400px; height: 44px" />
                </div>

            </div>



        </form>
        <form id="Signature_form" method="post" enctype="multipart/form-data">
            <div style="margin-left:50px;" >
            <img style="margin-top: 10px;margin-left:30px;" id="upload_org_code_img" src="#" width="200" height="100" />
            <div class="h_d" style="margin-top: 10px; margin-left: 15px">
                <div class="h_d1">照片上传：</div>
                <input class="easyui-filebox" name="Filedata" style="width: 150px" />
            </div>
        <a href="#" id="save_image" style="margin-left: 80px" class="easyui-linkbutton" data-options="iconCls:'icon-ok',plain:true">保存签名</a>
            </div>
        </form>

        <!--<%-- 添加项目对话框 --%>-->
        <div id="tree_add_dialog">
            <div style="margin-top: 30px;">
                <p class="h_d1" style="width: 80px">添加组名称:</p>
                <input id="tree_add_dialog_input" class="h_d1" name="" style="width: 200px;" />
            </div>


        </div>

        <!--<%-- 添加员工对话框 --%>-->
        <form id="department_people_info_dialog">
            <div style="float: left; margin-top: 15px">
                <div class="h_d" style="height: 23px; margin-left: 15px">
                    <p class="h_d1" style="width: 60px; text-align: right;">用户名:</p>
                    <input id="Text12" class="h_d1 easyui-textbox" name="User_count" required="required" style="width: 120px;" />
                    <p class="h_d1" style="width: 60px; text-align: right;">姓名:</p>
                    <input id="Text13" class="h_d1 easyui-textbox" name="User_name" required="required" style="width: 120px;" />
                </div>
                <div class="h_d" style="height: 23px; margin-left: 15px; margin-top: 5px">
                    <p class="h_d1" style="width: 60px; text-align: right;">职务:</p>
                    <input id="Text1" class="h_d1 easyui-textbox" name="User_job" style="width: 120px;" />
                    <p class="h_d1" style="width: 60px; text-align: right;">职责:</p>
                    <input id="Text2" class="h_d1 easyui-textbox" name="User_duties" style="width: 120px;" />

                </div>
                <div class="h_d" style="height: 23px; margin-left: 15px; margin-top: 5px">
                    <p class="h_d1 " style="width: 60px; text-align: right;">座机:</p>
                    <input id="Text3" class="h_d1 easyui-textbox" name="Tel" style="width: 120px;" />
                    <p class="h_d1" style="width: 60px; text-align: right;">手机:</p>
                    <input id="Text4" class="h_d1 easyui-textbox" name="Phone" style="width: 120px;" />
                </div>
                <div class="h_d" style="height: 23px; margin-left: 15px; margin-top: 5px">
                    <p class="h_d1" style="width: 60px; text-align: right;">传真:</p>
                    <input id="Text5" class="h_d1 easyui-textbox" name="Fax" style="width: 120px;" />
                    <p class="h_d1" style="width: 60px; text-align: right;">邮箱:</p>
                    <input id="Text6" class="h_d1 easyui-textbox" name="Email" style="width: 120px;" />
                </div>
                <div class="h_d" style="height: 23px; margin-left: 15px; margin-top: 5px">
                    <p class="h_d1" style="width: 60px; text-align: right;">QQ:</p>
                    <input id="Text7" class="h_d1 easyui-textbox" name="QQ" style="width: 120px;" />
                    <p class="h_d1" style="width: 60px; text-align: right;">MSN :</p>
                    <input id="Text8" class="h_d1 easyui-textbox" name="MSN" style="width: 120px;" />

                </div>
                <div class="h_d" style="height: 23px; margin-left: 15px; margin-top: 5px">
                    <p class="h_d1" style="width: 60px; text-align: right;">地址:</p>
                    <input id="Text9" class="h_d1 easyui-textbox" name="Address" style="width: 120px;" />
                    <p class="h_d1" style="width: 60px; text-align: right;">邮编:</p>
                    <input id="Text10" class="h_d1 easyui-textbox" name="Postcode" style="width: 120px;" />
                </div>
                <div class="h_d" style="height: 23px; margin-left: 15px; margin-top: 5px">
                    <p class="h_d1" style="width: 60px; text-align: right;">附注:</p>
                    <input id="Text11" class="h_d1 easyui-textbox" name="Remarks" data-options="multiline:true" style="width: 320px; height: 66px;" />
                </div>
                <br />
                <br />
            </div>
        </form>

        <!--<%-- 将员工指派到其它部门 --%>-->
        <form id="add_others_department_dialog">
            <div style="margin-top: 10px; float: left">
                <div class="h_d" style="margin-left: 15px">
                    <div class="h_d1" style="width: 200px; height: 170px; margin-left: 6px; margin-top: 6px">
                        <!--<%-- 检定项目--%>-->
                        <div id="department_info_add" style="width: 198px; height: 170px;" class="easyui-panel" title="组信息 "></div>
                    </div>
                    <div class="h_d1" style="margin-left: 5px;">
                        <div class="easyui-panel" style="width: 80px; height: 170px;">
                            <a style="width: 60px; margin-left: 10px; margin-top: 50px" id="tree_add_project" href="#" class="easyui-linkbutton">加入组</a>
                            <a style="width: 60px; margin-left: 10px; margin-top: 10px" id="tree_remove_project" href="#" class="easyui-linkbutton ">移除组</a>
                        </div>
                    </div>
                    <div class="h_d1" style="width: 20px; height: 170px; margin-left: 5px;">
                        <div class="easyui-panel" style="width: 200px; height: 170px;" title="加入组 ">
                            <div id="project_room" class="easyui-tree" style="width: 160px; height: 140px;"></div>
                        </div>

                    </div>
                </div>

            </div>

        </form>
    </div>
</body>
<%= WebExtensions.CombresLink("Personnel_management_Js") %>
</html>
