<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="personal_information.aspx.cs" Inherits="phians.mainform.personal_information" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
  
    <%= WebExtensions.CombresLink("personal_information_Css") %>
    <link rel="stylesheet" type="text/css" href="/phians_css/myStyle.css" />
    <style>
        #uploadify{
            margin-left:30px !important;
        }
    </style>
</head>
<body>
    <div id="department_layout" class="easyui-layout" data-options="fit:true,border:false">
        <%-- 资质证书 --%>
        <div data-options="region:'west'">
            <div id="qualification_certificate" class="easyui-panel">
            </div>
        </div>

        <%-- 个人信息 --%>
        <div data-options="region:'center'" style="margin-left: 10px">
            <div class="h_d" style="margin-left: 150px;">
                <%--用来作为文件队列区域--%>
                <div>
                    <img style="margin-top: 10px" id="upload_org_code_img" src="#" width="200" height="100" />
                    <form id="info_form" method="post" enctype="multipart/form-data">
                        <div class="h_d" style="margin-top: 10px; margin-left: 15px">
                            <div class="h_d1">照片上传：</div>
                            <input class="easyui-filebox" name="file_" style="width: 150px" />
                        </div>
                    </form>
                    <a href="#" id="save_image" style="margin-left: 80px" class="easyui-linkbutton" data-options="iconCls:'icon-ok',plain:true">保存签名</a>

                </div>
<%--                <div id="upload_view"  style="width: 300px; display: none; margin-left: 0px">
                    <img style="display: none; margin-top: 10px" id="upload_org_code_img" src="#" width="200" height="100" />
                    <div id="fileQueue" style="display: none;margin-left:100px;" ></div>
                    <input style="display: none" type="file" name="uploadify" id="uploadify" />
                </div>--%>
                <form id="department_people_info" class="h_d1" >
                    <div style="height: 25px; margin-left: 15px">
                        <label style="width: 40px; margin-top: 5px; float: left">部门:</label>
                        <input id="Department_name" class="easyui-textbox" name="Department_name" style="width: 150px;" readonly="true" /><br />
                        <div style="height: 5px"></div>
                        <label style="width: 40px; margin-top: 5px; float: left">职务:</label>
                        <input id="User_job" class="easyui-textbox" name="User_job" style="width: 150px;" readonly="true" /><br />
                        <div style="height: 5px"></div>
                        <label style="width: 40px; margin-top: 5px; float: left">职责:</label>
                        <input id="User_duties" class="easyui-textbox" name="User_duties" style="width: 150px;" readonly="true" /><br />
                        <div style="height: 5px"></div>
                        <label style="width: 40px; margin-top: 5px; float: left">座机:</label>
                        <input id="Tel" class="easyui-textbox" name="Tel" style="width: 150px;" /><br />
                        <div style="height: 5px"></div>
                        <label style="width: 40px; margin-top: 5px; float: left">手机:</label>
                        <input id="Phone" class="easyui-textbox" name="Phone" style="width: 150px;" /><br />
                        <div style="height: 5px"></div>
                        <label style="width: 40px; margin-top: 5px; float: left">传真:</label>
                        <input id="Fax" class="easyui-textbox" name="Fax" style="width: 150px;" /><br />
                        <div style="height: 5px"></div>
                        <label style="width: 40px; margin-top: 5px; float: left">邮箱:</label>
                        <input id="Email" class="easyui-textbox" name="Email" style="width: 150px;" /><br />
                        <div style="height: 5px"></div>
                        <label style="width: 40px; margin-top: 5px; float: left">QQ:</label>
                        <input id="QQ" class="easyui-textbox" name="QQ" style="width: 150px;" /><br />
                        <div style="height: 5px"></div>
                        <label style="width: 40px; margin-top: 5px; float: left">MSN:</label>
                        <input id="MSN" class="easyui-textbox" name="MSN" style="width: 150px;" /><br />
                        <div style="height: 5px"></div>
                        <label style="width: 40px; margin-top: 5px; float: left">地址:</label>
                        <input id="Address" class="easyui-textbox" name="Address" style="width: 150px;" /><br />
                        <div style="height: 5px"></div>
                        <label style="width: 40px; margin-top: 5px; float: left">邮编:</label>
                        <input id="Postcode" class="easyui-textbox" name="Postcode" style="width: 150px;" /><br />
                        <div style="height: 5px"></div>
                        <label style="width: 40px; margin-top: 5px; float: left">附注:</label>
                        <input id="Remarks" class="easyui-textbox" name="Remarks"data-options="multiline:true" style="width: 150px;height:45px;" /><br />

                    </div>

                </form>
                <a id="save_personnel_info" class="easyui-linkbutton" data-options="iconCls:'icon-ok'" style="margin-top: 100px; margin-left: -130px">确认修改</a>

            </div>

        </div>


    </div>
    <div style="display: none">

        <%-- 增加证书 --%>
        <form id="S_dialog" style="text-align:center;">
            <div title="增加证书">
                <div class="h_dline" style="height: 23px; margin-top: 20px; margin-left: 60px;">
                    <div class="h_dline" style="height: 23px; margin-top: 5px;">
                        <p class="h_d1" style="width: 60px;text-align:right;">证书名字:</p>
                        <input id="certificate_name" class="easyui-textbox" name="certificate_name" required="required" style="width: 160px; height: 23px" />
                    </div>
                    <div class="h_dline" style="height: 23px; margin-top: 10px;">
                        <p class="h_d1" style="width: 60px;text-align:right;">证书编号:</p>
                        <input id="certificate_num" class="easyui-textbox" name="certificate_num" required="required" style="width: 160px; height: 23px;" />
                    </div>
                    <div class="h_dline" style="height: 23px; margin-top: 10px;">
                        <p class="h_d1" style="width: 60px;text-align:right;">发证单位:</p>
                        <input id="Issuing_unit" class="easyui-textbox" name="Issuing_unit" required="required" style="width: 160px; height: 23px" />
                    </div>
                    <div class="h_dline" style="height: 23px; margin-top: 10px;">
                        <p class="h_d1" style="width: 60px;text-align:right;">有效日期:</p>
                        <input id="effective_date" type="text" class="easyui-datebox" required="required" name="effective_date" style="width: 160px; height: 23px;" />
                    </div>
                    <div class="h_dline" style="height: 23px; margin-top: 20px;">
                        <p class="h_d1" style="width: 60px;text-align:right;">附注:</p>
                        <input id="remarks" class="easyui-textbox" name="remarks" data-options="multiline:true" style="width: 160px; height: 45px" />
                    </div>
                </div>
            </div>
        </form>

        <div id="qualification_certificate_toolbar">
            <a href="#" id="add_certificate" class="easyui-linkbutton" data-options="iconCls:'icon-add',plain:true">增加证书</a>
            <span class="datagrid-btn-separator" style="vertical-align: middle; height: 22px; display: inline-block; float: none"></span>
            <a href="#" id="remove_certificate" class="easyui-linkbutton" data-options="iconCls:'icon-remove',plain:true">删除证书</a>
            <span class="datagrid-btn-separator" style="vertical-align: middle; height: 22px; display: inline-block; float: none"></span>
            <a href="#" id="due_warning" class="easyui-linkbutton" data-options="iconCls:'icon-tip',plain:true">到期预警(30天)</a>
            <span class="datagrid-btn-separator" style="vertical-align: middle; height: 22px; display: inline-block; float: none"></span>
            <a href="#" id="overdue_query" class="easyui-linkbutton" data-options="iconCls:'icon-search',plain:true">过期查询</a>
            <span class="datagrid-btn-separator" style="vertical-align: middle; height: 22px; display: inline-block; float: none"></span>
            <a href="#" id="show_all" class="easyui-linkbutton" data-options="iconCls:'icon-search',plain:true">显示全部</a>
        </div>
    </div>

</body>
      <%= WebExtensions.CombresLink("personal_information_Js") %>
</html>
