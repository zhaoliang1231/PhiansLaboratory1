<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LosslessPerson_Management.aspx.cs" Inherits="phians.phians_js.Lossless_report.LosslessPerson_Management" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <%=WebExtensions.CombresLink("public_Css") %>
  <%--  <style>
        #test_layout .layout-button-left ,#test_layout .layout-button-down{
    background:0;
}
    </style>--%>
</head>
<body>
    <div id="test_layout" class="easyui-layout" data-options="fit:true">
        <div data-options="region:'south',title:'页面访问权限设置',border:false,split:false">
            <form id="Permissions_1" runat="server" style="float: left; margin-left: 10px">

                <!--<%-- 权限设置 --%>-->
                <div style="height: 2px"></div>


                <div style="float: left;">
                    <!--<%-- 个人中心 --%>-->
                    <div id="Personal_center">
                        <fieldset style="border: 1px solid #c3ddf4; margin-top: 3px; width: 700px">
                            <legend style="font-weight: 600">个人中心</legend>
                            <input id="page0" class="tog_click" name="page0" type="checkbox" style="width: auto" value="0" /><label for="page0">个人资料</label>
                            <input id="page1" class="tog_click" name="message_management" type="checkbox" style="width: auto" value="0" /><label for="page1">工作消息</label>
                            <a href="#" id="Personal_choose" style="float: right;" class="easyui-linkbutton" data-options="iconCls:'icon-edit',plain:true">全选</a>
                        </fieldset>
                    </div>
                    <!--<%-- 系统设置 --%>-->
                    <div id="System_settings">
                        <fieldset style="border: 1px solid #c3ddf4; margin-top: 3px; width: 700px">
                            <legend style="font-weight: 600">系统设置</legend>
                            <input id="page2" class="tog_click" name="System_log" type="checkbox" style="width: auto" value="0" /><label for="page2">系统日志</label>
                            <input id="page3" class="tog_click" name="Dictionary_management" type="checkbox" style="width: auto; display: none" value="0" /><label for="page3" style="display: none">字典管理</label>
                            <input id="page4" class="tog_click" name="Personnel_management" type="checkbox" style="width: auto; display: none" value="0" /><label for="page4" style="display: none">人员管理</label>
                            <input id="page5" class="tog_click" name="Authority_management" type="checkbox" style="width: auto; display: none" value="0" /><label for="page5" style="display: none">权限管理</label>
                        </fieldset>
                    </div>
                    <!--<%-- 质量管理 --%>-->
                    <div id="Quality_management" style="display: none">
                        <fieldset style="border: 1px solid #c3ddf4; margin-top: 3px; width: 700px">
                            <legend style="font-weight: 600">质量管理</legend>
                            <input id="page6" class="tog_click" name="system_file" type="checkbox" style="width: auto" value="0" /><label for="page6">体系文件</label>
                            <input id="page7" class="tog_click" name="Internal_audit" type="checkbox" style="width: auto" value="0" /><label for="page7">内部审核</label>
                            <input id="page8" class="tog_click" name="Management_review" type="checkbox" style="width: auto" value="0" /><label for="page8">管理评审</label>
                            <input id="page9" class="tog_click" name="Environmental_management" type="checkbox" style="width: auto" value="0" /><label for="page9">环境管理</label>
                            <a href="#" id="Quality_management_choose" style="float: right;" class="easyui-linkbutton" data-options="iconCls:'icon-edit',plain:true">全选</a>
                        </fieldset>
                    </div>
                    <!--<%-- 技术管理 --%>-->
                    <div id="Technical_management" style="display: none">
                        <fieldset style="border: 1px solid #c3ddf4; margin-top: 3px; width: 700px">
                            <legend style="font-weight: 600">技术管理</legend>
                            <input id="page10" class="tog_click" name="Test_Standard_Library" type="checkbox" style="width: auto" value="0" /><label for="page10">试验标准库</label>
                            <input id="page11" class="tog_click" name="Test_Standard_constitute" type="checkbox" style="width: auto" value="0" /><label for="page11">试验标准编制</label>
                            <input id="page12" class="tog_click" name="Test_standard_review" type="checkbox" style="width: auto" value="0" /><label for="page12">试验标准审核</label>
                            <%--                                    <input id="Test_Item_config" name="Test_Item_config" type="checkbox" style="width: auto" value="0" /><label for="Test_Item_config">试验项目配置</label>--%>
                            <input id="page13" class="tog_click" name="commissioned_Information_config" type="checkbox" style="width: auto" value="0" /><label for="page13">委托信息配置</label>
                            <input id="page14" class="tog_click" name="Laboratory_Qualification_info" type="checkbox" style="width: auto" value="0" /><label for="page14">技术要求</label>
                            <%--<input id="technicians" name="technicians" type="checkbox" style="width: auto" value="0" /><label for="technicians">人员资质管理</label>--%>
                            <input id="page15" class="tog_click" name="Method_management" type="checkbox" style="width: auto" value="0" /><label for="page15">规程文件</label>
                            <input id="page16" class="tog_click" name="Template_file" type="checkbox" style="width: auto" value="0" /><label for="page16">模板文件</label>
                            <a href="#" id="Technology_choose" style="float: right;" class="easyui-linkbutton" data-options="iconCls:'icon-edit',plain:true">全选</a>
                        </fieldset>
                    </div>
                    <!--<%-- 委托管理 --%>-->
                    <div id="Commissioned_managements" style="display: none">
                        <fieldset style="border: 1px solid #c3ddf4; margin-top: 3px; width: 700px">
                            <legend style="font-weight: 600">试验管理</legend>
                            <input id="page17" class="tog_click" name="Warehousing_commissioned" type="checkbox" style="width: auto" value="0" /><label for="page17">入库委托</label>
                            <input id="page18" class="tog_click" name="Test_commissioned_application" type="checkbox" style="width: auto" value="0" /><label for="page18">试验委托申请</label>
                            <input id="page19" class="tog_click" name="Test_commissioned_review" type="checkbox" style="width: auto" value="0" /><label for="page19">试验委托评审</label>
                            <input id="page20" class="tog_click" name="Sample_machining" type="checkbox" style="width: auto" value="0" /><label for="page20">试样机加</label>
                            <input id="page21" class="tog_click" name="Task_appoint_machining" type="checkbox" style="width: auto" value="0" /><label for="page21">机加任务指派</label>
                            <input id="page22" class="tog_click" name="Task_pool" type="checkbox" style="width: auto" value="0" /><label for="page22">任务池</label>
                            <input id="page23" class="tog_click" name="Test_record" type="checkbox" style="width: auto" value="0" /><label for="page23">试验记录</label>
                            <input id="page24" class="tog_click" name="Test_commissioned_management" type="checkbox" style="width: auto" value="0" /><label for="page24">试验委托管理</label>
                            <input id="page25" class="tog_click" name="Test_return_application" type="checkbox" style="width: auto" value="0" /><label for="page25">任务退回审核</label>
                            <input id="page26" class="tog_click" name="Sample_invalid" type="checkbox" style="width: auto" value="0" /><label for="page26">样品失效</label>
                            <input id="page27" class="tog_click" name="Sample_invalid_machining" type="checkbox" style="width: auto" value="0" /><label for="page27">样品失效机加</label>
                            <a href="#" id="Commissioned_managements_choose" style="float: right;" class="easyui-linkbutton" data-options="iconCls:'icon-edit',plain:true">全选</a>
                        </fieldset>
                    </div>
                    <!--<%-- 报告管理 --%>-->
                    <div id="Report_managements" style="display: none">
                        <fieldset style="border: 1px solid #c3ddf4; margin-top: 3px; width: 700px">
                            <legend style="font-weight: 600">报告管理</legend>
                            <input id="page28" class="tog_click" name="Report_Edit" type="checkbox" style="width: auto" value="0" /><label for="page28">报告编制</label>
                            <input id="page29" class="tog_click" name="Report_Review" type="checkbox" style="width: auto" value="0" /><label for="page29">报告审核</label>
                            <input id="page30" class="tog_click" name="Report_Issue" type="checkbox" style="width: auto" value="0" /><label for="page30">报告批准</label>
                            <input id="page31" class="tog_click" name="Report_management" type="checkbox" style="width: auto" value="0" /><label for="page31">报告管理</label>
                            <input id="page32" class="tog_click" name="Report_view" type="checkbox" style="width: auto" value="0" /><label for="page32">报告查看</label>
                            <input id="page33" class="tog_click" name="Error_Report_Apply_Audit" type="checkbox" style="width: auto" value="0" /><label for="page33">异常报告申请审核</label>
                            <input id="page34" class="tog_click" name="Error_Report_EditApply_Edit" type="checkbox" style="width: auto" value="0" /><label for="page34">异常报告编辑申请编辑</label>
                            <input id="page35" class="tog_click" name="Error_Report_EditApply_Audit" type="checkbox" style="width: auto" value="0" /><label for="page35">异常报告编辑申请审核</label>
                            <input id="page36" class="tog_click" name="Error_Report_Apply_Management" type="checkbox" style="width: auto" value="0" /><label for="page36">异常报告申请管理</label>
                            <input id="page37" class="tog_click" name="Report_distribution" type="checkbox" style="width: auto" value="0" /><label for="page37">报告分配</label>
                            <input id="page38" class="tog_click" name="Report_receive" type="checkbox" style="width: auto" value="0" /><label for="page38">报告接收</label>
                            <input id="page54" class="tog_click" name="report_arrange" type="checkbox" style="width: auto" value="0" /><label for="page54">报告整理</label>
                            <a href="#" id="Report_managements_choose" style="float: right;" class="easyui-linkbutton" data-options="iconCls:'icon-edit',plain:true">全选</a>
                        </fieldset>
                    </div>
                    <!--<%-- 样品管理 --%>-->
                    <div id="Sample_managements" style="display: none">
                        <fieldset style="border: 1px solid #c3ddf4; margin-top: 3px; width: 700px">
                            <legend style="font-weight: 600">样品管理</legend>
                            <input id="page39" class="tog_click" name="Sample_accept" type="checkbox" style="width: auto" value="0" /><label for="page39">样品接收</label>
                            <input id="page40" class="tog_click" name="Sample_receive" type="checkbox" style="width: auto" value="0" /><label for="page40">样品领取</label>
                            <input id="page41" class="tog_click" name="Sample_management" type="checkbox" style="width: auto" value="0" /><label for="page41">样品管理</label>
                            <input id="page42" class="tog_click" name="Stay_management" type="checkbox" style="width: auto" value="0" /><label for="page42">留样管理</label>
                            <a href="#" id="Sample_choose" style="float: right;" class="easyui-linkbutton" data-options="iconCls:'icon-edit',plain:true">全选</a>
                        </fieldset>
                    </div>
                    <!--<%-- 台账管理 --%>-->
                    <div id="Equipment_managements" style="display: none">
                        <fieldset style="border: 1px solid #c3ddf4; margin-top: 3px; width: 700px">
                            <legend style="font-weight: 600">台账管理</legend>
                            <input id="page43" class="tog_click" name="Metering_equipment" type="checkbox" style="width: auto" value="0" /><label for="page43">试验设备</label>
                            <input id="page44" class="tog_click" name="Metering_standard" type="checkbox" style="width: auto" value="0" /><label for="page44">标准物质</label>
                            <a href="#" id="Equipment_choose" style="float: right;" class="easyui-linkbutton" data-options="iconCls:'icon-edit',plain:true">全选</a>
                        </fieldset>
                    </div>
                    <!--<%-- 业务监控 --%>-->
                    <div id="Business_monitoring" style="display: none">
                        <fieldset style="border: 1px solid #c3ddf4; margin-top: 3px; width: 700px">
                            <legend style="font-weight: 600">业务监控</legend>
                            <input id="page45" class="tog_click" name="task_monitoring" type="checkbox" style="width: auto" value="0" /><label for="page45">任务监控</label>
                            <%--                                    <input id="Overdue_monitoring" name="Overdue_monitoring" type="checkbox" style="width: auto" value="0" /><label for="Overdue_monitoring">预期监控</label>--%>
                            <a href="#" id="Monitoring_choose" style="float: right;" class="easyui-linkbutton" data-options="iconCls:'icon-edit',plain:true">全选</a>
                        </fieldset>
                    </div>
                    <!--<%-- 审批管理 --%>-->
                    <%--                        <div id="Approval_managements">
                                <fieldset style="border: 1px solid #c3ddf4; margin-top: 3px; width: 700px">
                                    <legend style="font-weight: 600">审批管理</legend>
                                    <input id="Application_review" name="Application_review" type="checkbox" style="width: auto" value="0" /><label for="Application_approval">申请审核</label>
                                    <input id="Application_approval" name="Application_approval" type="checkbox" style="width: auto" value="0" /><label for="Application_approval">申请批准</label>
                                    <input id="Approval_management" name="Approval_management" type="checkbox" style="width: auto" value="0" /><label for="Approval_management">审批管理</label>
                                    <a href="#" id="Approval_managements_choose" style="float: right;" class="easyui-linkbutton" data-options="iconCls:'icon-edit',plain:true">全选</a>
                                </fieldset>
                            </div>--%>
                    <!--<%-- 统计管理 --%>-->
                    <div id="Statistical_management" style="display: none">
                        <fieldset style="border: 1px solid #c3ddf4; margin-top: 3px; width: 700px">
                            <legend style="font-weight: 600">统计管理</legend>
                            <input id="page46" class="tog_click" name="Commissioned_statistical" type="checkbox" style="width: auto" value="0" /><label for="page46">委托统计</label>
                            <input id="page47" class="tog_click" name="Equipment_statistical" type="checkbox" style="width: auto" value="0" /><label for="page47">台账统计</label>
                            <a href="#" id="Statistical_choose" style="float: right;" class="easyui-linkbutton" data-options="iconCls:'icon-edit',plain:true">全选</a>
                        </fieldset>
                    </div>
                    <!--<%-- 无损报告管理 --%>-->
                    <div id="Lossless_report_management">
                        <fieldset style="border: 1px solid #c3ddf4; margin-top: 3px; width: 700px">
                            <legend style="font-weight: 600">无损报告管理</legend>
                            <input id="page48" class="tog_click" name="LosslessReport_Edit" type="checkbox" style="width: auto" value="0" /><label for="page48">无损报告编制</label>
                            <input id="page49" class="tog_click" name="LosslessReport_Review" type="checkbox" style="width: auto" value="0" /><label for="page49">无损报告审核</label>
                            <input id="page50" class="tog_click" name="LosslessReport_Issue" type="checkbox" style="width: auto" value="0" /><label for="page50">无损报告签发</label>
                            <input id="page51" class="tog_click" name="LosslessReport_management" type="checkbox" style="width: auto" value="0" /><label for="page51">无损报告管理</label>
                            <input id="page55" class="tog_click" name="LosslessReport_Apply_Audit" type="checkbox" style="width: auto" value="0" /><label for="page55">无损异常报告审核</label>
                            <input id="page56" class="tog_click" name="LosslessReport_EditApply_Edit" type="checkbox" style="width: auto" value="0" /><label for="page56">无损报告异常编制</label>
                            <input id="page57" class="tog_click" name="LosslessReport_EditApply_Audit" type="checkbox" style="width: auto" value="0" /><label for="page57">无损报告编制审核</label>
                            <input id="page58" class="tog_click" name="LosslessReport_Apply_Management" type="checkbox" style="width: auto" value="0" /><label for="page58">报告异常管理</label>
                            <input id="page52" class="tog_click" name="Probe_Library" type="checkbox" style="width: auto" value="0" /><label for="page52">探头库</label>
                            <input id="page53" class="tog_click" name="Device_Library" type="checkbox" style="width: auto" value="0" /><label for="page53">设备库</label>
                            <input id="page59" class="tog_click" name="LosslessPerson_Management" type="checkbox" style="width: auto" value="0" /><label for="page59">无损人员管理</label>
                            <a href="#" id="Lossless_report_choose" style="float: right;" class="easyui-linkbutton" data-options="iconCls:'icon-edit',plain:true">全选</a>
                        </fieldset>
                    </div>
                </div>
                <div style="float: left; margin-left: 10px; margin-top: 20px">
                    <a href="#" id="Permissions_submit" style="float: right; margin-top: 10px" class="easyui-linkbutton" data-options="iconCls:'icon-ok'">确定修改</a>
                </div>



            </form>
        </div>

        <div data-options="region:'center',border:false,split:false">
            <div id="department_people" data-options="fit:true">
            </div>

        </div>
        <div data-options="region:'west',title:'部门资料',split:false" style="width: 100px;">

            <div id="department_info">
            </div>

        </div>



    </div>
    <div class="" style="display: none;">
        <div id="department_people_toolbar">

            <a href="#" id="read_info" class="easyui-linkbutton" data-options="iconCls:'icon-edit',plain:true">修改信息</a>
            <span class="datagrid-btn-separator" style="vertical-align: middle; height: 22px; display: inline-block; float: none"></span>
            <%--<a href="#" id="read_Signature" class="easyui-linkbutton" data-options="iconCls:'icon-Document',plain:true">查看签名</a>
            <span class="datagrid-btn-separator" style="vertical-align: middle; height: 22px; display: inline-block; float: none"></span>--%>
            <%-- <a href="#" id="no_department_personnel" class="easyui-linkbutton" data-options="iconCls:'icon-Document',plain:true">查看未分配人员</a>
            <span class="datagrid-btn-separator" style="vertical-align: middle; height: 22px; display: inline-block; float: none"></span>--%>
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
            <a id="add_dictionary" href="#" class="easyui-linkbutton" data-options="iconCls:'icon-add',plain:true">添加字典</a>
            <span class="datagrid-btn-separator" style="vertical-align: middle; height: 22px; display: inline-block; float: none"></span>
            <input id="search" style="width: 120px" />
            <input id="search1" style="width: 120px; font-size: 12px;" />
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
                <div class="h_d1" style="height: 25px;">
                    <p class="h_d1" style="width: 60px">部门:</p>
                    <input id="department_" class="easyui-textbox" style="width: 150px;" />

                </div>
                <div class="h_d" style="height: 25px; margin-left: 0px;">
                    <p class="h_d1" style="width: 60px; text-align: center;">附注:</p>
                    <input id="Remarks" class="easyui-textbox" data-options="multiline:true" name="Remarks" style="width: 400px; height: 44px" />
                </div>

            </div>



        </form>
        <form id="Signature_form">
            <!--<%--用来作为文件队列区域--%>-->
            <div id="upload_view" class="h_d1" style="width: 150px; height: 150px; display: none; margin-left: 50px; text-align: left; margin-top: 20px">
                <img style="display: none" id="upload_org_code_img" src="#" width="200" height="120" />
                <div id="fileQueue" style="display: none"></div>
                <input style="display: none" type="file" name="uploadify" id="uploadify" />
            </div>

        </form>

        <!--<%-- 添加项目对话框 --%>-->
        <div id="tree_add_dialog">
            <div style="margin-top: 30px;">
                <p class="h_d1" style="width: 80px">添加组名称:</p>
                <input id="tree_add_dialog_input" class="h_d1" name="" style="width: 200px;" />
            </div>


        </div>
        <%--添加字典--%>
        <form id="dictionary_list_dialog">
            <div id="standards_info" style="margin-top: 0px">
            </div>
        </form>
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
                <p class="h_d1" style="width: 60px; text-align: right;">项目内容:</p>
                &nbsp;
                    <input id="Project_name" name="Project_name" class="easyui-textbox" required="required" style="width: 150px; height: 22px; margin-left: 5px" />
            </div>
            <br />
            <div class="h_dline" style="height: 23px; margin-top: 10px; margin-left: 42px; float: left">
                <p class="h_d1" style="width: 60px; text-align: right;">项目值:</p>
                &nbsp;
                    <input id="Project_value" name="Project_value" class="easyui-textbox" required="required" style="width: 150px; height: 22px; margin-left: 5px" />
            </div>
            <br />
            <div class="h_dline" style="height: 23px; margin-top: 10px; margin-left: 42px; float: left">
                <p class="h_d1" style="width: 60px; text-align: right;">排序:</p>
                &nbsp;
                    <input id="Sort_num" name="Sort_num" class="easyui-textbox" style="width: 150px; height: 22px; margin-left: 5px" />
            </div>
            <div class="h_dline" style="height: 23px; margin-top: 20px; margin-left: 42px; float: left">
                <p class="h_d1" style="width: 60px; text-align: right;">说明:</p>
                &nbsp;
                    <input id="remarks" name="remarks" class="easyui-textbox" data-options="multiline:true" style="width: 150px; height: 66px; margin-left: 5px" />
            </div>
        </form>
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
                <div class="h_d" style="height: 25px; margin-left: 15px; margin-top: 5px">
                    <p class="h_d1" style="width: 60px">部门:</p>
                    <input id="department" style="width: 120px;" required="required" />

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
    <!--<%-- 详细权限 --%>-->
    <form id="dialog_Permissions">
        <div style="float: left">
            <div>
                <!--<%-- 部门树 --%>-->
                <div id="page_info" class="easyui-panel" title="页面信息" style="width: 230px; height: 425px;">
                </div>
            </div>
        </div>
        <div style="float: left; margin-top: 30px">
            <div id="supplier_info_dialog">
                <fieldset style="border: 1px solid #c3ddf4; margin-top: 10px; margin-left: 10px; width: 300px; height: 300px">
                    <legend style="font-weight: 600">操作权限</legend>
                    <div style="margin-top: 5px"></div>
                    <input id="read_content" class="h_d1" name="read_content" type="checkbox" style="width: auto" value="0" /><label for="read_content">阅读</label>
                    <input id="create_content" class="h_d1" name="create_content" type="checkbox" style="width: auto; margin-left: 15px;" value="0" /><label for="create_content">创建内容</label>
                    <input id="edit_content" class="h_d1" name="edit_content" type="checkbox" style="width: auto; margin-left: 15px;" value="0" /><label for="edit_content">编辑</label>
                    <input id="delete_content" class="h_d1" name="delete_content" type="checkbox" style="width: auto; margin-left: 15px;" value="0" /><label for="delete_content">删除</label><br />
                    <div style="margin-top: 5px"></div>
                    <input id="export_content" class="h_d1" name="export_content" type="checkbox" style="width: auto;" value="0" /><label for="export_content">导出</label>
                    <input id="print_content" class="h_d1" name="print_content" type="checkbox" style="width: auto; margin-left: 15px;" value="0" /><label for="print_content">打印</label>
                    <input id="collate_content" class="h_d1" name="collate_content" type="checkbox" style="width: auto; margin-left: 15px;" value="0" /><label for="collate_content">校对</label>
                    <input id="review_content" class="h_d1" name="review_content" type="checkbox" style="width: auto; margin-left: 15px;" value="0" /><label for="review_content">审核</label><br />
                    <div style="margin-top: 5px"></div>
                    <input id="issue_content" class="h_d1" name="issue_content" type="checkbox" style="width: auto;" value="0" /><label for="issue_content">签发</label>
                    <input id="create_plan" class="h_d1" name="create_plan" type="checkbox" style="width: auto; margin-left: 15px;" value="0" /><label for="create_plan">创建计划</label>
                    <input id="cancel_plan" class="h_d1" name="cancel_plan" type="checkbox" style="width: auto; margin-left: 15px;" value="0" /><label for="cancel_plan">取消创建计划</label><br />
                    <div style="margin-top: 5px"></div>
                    <input id="Equipment_operation" class="h_d1" name="Equipment_operation" type="checkbox" style="width: auto;" value="0" /><label for="Equipment_operation">设备操作（启用、停止作废、、、）</label>
                    <div style="margin-top: 5px"></div>
                </fieldset>
            </div>
            <div id="button_view">
                <a href="#" id="all_choose" style="float: right; margin-top: 10px" class="easyui-linkbutton" data-options="iconCls:'icon-edit',plain:true">全部选择</a>
                <a href="#" id="all_cancel" style="float: right; margin-top: 10px" class="easyui-linkbutton" data-options="iconCls:'icon-edit',plain:true">全部取消</a>
            </div>

        </div>

    </form>
</body>

<%=WebExtensions.CombresLink("LosslessPerson_Management_Js") %>
</html>
