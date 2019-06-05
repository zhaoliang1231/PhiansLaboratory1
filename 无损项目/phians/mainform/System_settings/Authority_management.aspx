<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Authority_management.aspx.cs" Inherits="phians.mainform.Permissions" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <%=WebExtensions.CombresLink("public_Css") %>
</head>
<body>
    <div id="test_layout" class="easyui-layout" data-options="fit:true">
        <div data-options="region:'west',border:false" style="margin-top: 0px; overflow: scroll; overflow-x: hidden; overflow-y: hidden">
            <div style="margin-top: 5px; margin-left: 5px">
                <!--<%-- 部门树 --%>-->
                <div id="department_info_1" class="easyui-panel" title="部门资料" style="width: 300px;">
                    <div id="department_info"></div>
                </div>
                <div>
                    <!--<%-- 员工表 --%>-->
                    <div id="department_people" style="width: 300px;"></div>
                    <div id="department_people_toolbar">
                        <div>
                            <input id="search" style="width: 150px" />
                        </div>
                        <input id="search1" style="width: 150px" />
                        <a href="#" id="search_people" class="easyui-linkbutton" data-options="iconCls:'icon-search',plain:true">搜索</a>
                    </div>
                </div>

            </div>
        </div>
        <div data-options="region:'east',border:false">
            <form id="Permissions_1" runat="server" style="float: left; margin-left: 10px">
                <div data-options="region:'west',border:false">
                    <!--<%-- 权限设置 --%>-->
                    <div style="height: 2px"></div>
                    <fieldset style="border: 1px solid #c3ddf4; width: 820px">
                        <legend style="font-weight: 600">页面访问权限设置</legend>
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
                                    <input id="page3" class="tog_click" name="Dictionary_management" type="checkbox" style="width: auto" value="0" /><label for="page3">字典管理</label>
                                    <input id="page4" class="tog_click" name="Personnel_management" type="checkbox" style="width: auto" value="0" /><label for="page4">人员管理</label>
                                    <input id="page5" class="tog_click" name="Authority_management" type="checkbox" style="width: auto" value="0" /><label for="page5">权限管理</label>
                                    <input id="page17" class="tog_click" name="Template_file" type="checkbox" style="width: auto" value="0" /><label for="page17">模板管理</label>
                                    <input id="page18" class="tog_click" name="PageShowSetting" type="checkbox" style="width: auto" value="0" /><label for="page18">页面显示配置</label>
                                    <a href="#" id="System_settings_choose" style="float: right;" class="easyui-linkbutton" data-options="iconCls:'icon-edit',plain:true">全选</a>
                                </fieldset>
                            </div>
                           
                           
                            <!--<%-- 无损报告管理 --%>-->
                            <div id="Lossless_report_management">
                                <fieldset style="border: 1px solid #c3ddf4; margin-top: 3px; width: 700px">
                                    <legend style="font-weight: 600">无损报告管理</legend>
                                    <input id="page6" class="tog_click" name="LosslessReport_Edit" type="checkbox" style="width: auto" value="0" /><label for="page6">无损报告编制</label>
                                    <input id="page7" class="tog_click" name="LosslessReport_Review" type="checkbox" style="width: auto" value="0" /><label for="page7">无损报告审核</label>
                                    <input id="page8" class="tog_click" name="LosslessReport_Issue" type="checkbox" style="width: auto" value="0" /><label for="page8">无损报告签发</label>
                                    <input id="page9" class="tog_click" name="LosslessReport_management" type="checkbox" style="width: auto" value="0" /><label for="page9">无损报告管理</label>
                                    <input id="page10" class="tog_click" name="LosslessReport_Apply_Audit" type="checkbox" style="width: auto" value="0" /><label for="page10">无损异常报告申请审核</label>
                                    <input id="page11" class="tog_click" name="LosslessReport_EditApply_Edit" type="checkbox" style="width: auto" value="0" /><label for="page11">无损异常报告编辑</label>
                                    <input id="page12" class="tog_click" name="LosslessReport_EditApply_Audit" type="checkbox" style="width: auto" value="0" /><label for="page12">无损异常报告审核</label>
                                    <input id="page13" class="tog_click" name="LosslessReport_Apply_Management" type="checkbox" style="width: auto" value="0" /><label for="page13">无损异常报告管理</label>
                                    <input id="page14" class="tog_click" name="Probe_Library" type="checkbox" style="width: auto" value="0" /><label for="page14">探头库</label>
                                    <input id="page15" class="tog_click" name="Device_Library" type="checkbox" style="width: auto" value="0" /><label for="page15">设备库</label>
                                    <input id="page16" class="tog_click" name="LosslessReport_Monitor" type="checkbox" style="width: auto" value="0" /><label for="page16">无损监控管理</label>
                                    
                                    <a href="#" id="Lossless_report_choose" style="float: right;" class="easyui-linkbutton" data-options="iconCls:'icon-edit',plain:true">全选</a>
                                </fieldset>
                            </div>
                        </div>
                        <div style="float: left; margin-left: 10px; margin-top: 20px">
                            <a href="#" id="Permissions_submit" style="float: right; margin-top: 10px" class="easyui-linkbutton" data-options="iconCls:'icon-ok'">确定修改</a>
                        </div>
                    </fieldset>
                </div>

            </form>
        </div>
    </div>
</body>

<%=WebExtensions.CombresLink("Authority_management_Js") %>
</html>
