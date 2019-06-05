<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Probe_Library.aspx.cs" Inherits="phians.mainform.Lossless_report.Probe_Library" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
           <%=WebExtensions.CombresLink("public_Css") %>

</head>
<body>

    <!--<%--设备库数据表格--%>-->
    <div id="Probe_Library_datagrid"></div>

    <div style="display: none">

        <!-- 台账资料维护工具栏 -->
        <div id="Probe_Library_datagrid_toolbar">
            <a href="#" id="_edit" class="easyui-linkbutton" data-options="iconCls:'icon-edit',plain:true">修改信息</a>
            <span class="datagrid-btn-separator" style="vertical-align: middle; height: 22px; display: inline-block; float: none"></span>
            <a href="#" id="_add" class="easyui-linkbutton" data-options="iconCls:'icon-add',plain:true">增加信息</a>
            <span class="datagrid-btn-separator" style="vertical-align: middle; height: 22px; display: inline-block; float: none"></span>
            <a href="#" id="_delete" class="easyui-linkbutton" data-options="iconCls:'icon-remove',plain:true">删除信息</a>
            <span class="datagrid-btn-separator" style="vertical-align: middle; height: 22px; display: inline-block; float: none"></span>
         
            <input id="search" style="width: 150px" class="easyui-combobox" />
            <label>为 </label>
            <input id="key" style="width: 150px" class="easyui-textbox" />
            <a href="#" id="maintenance_search" class="easyui-linkbutton" data-options="iconCls:'icon-search',plain:true">搜索</a>
        </div>
        <!--<%--dialog修改 设备库 资料--%>-->
        <form id="AddEidt_info">
            <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                <p class="h_d1" style="width: 110px; text-align: right;">仪器名称:</p>
                <input id="Probe_name" name="Probe_name" required="required" class="easyui-textbox" style="width: 180px; height: 22px; margin-left: 5px" />
            </div>
            <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                <p class="h_d1" style="width: 110px; text-align: right;">仪器编号:</p>
                <input id="Probe_num" name="Probe_num" required="required" class="easyui-textbox" style="width: 180px; height: 22px; margin-left: 5px" />
            </div>
            <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                <p class="h_d1" style="width: 110px; text-align: right;">仪器型号:</p>
                <input id="Probe_type" name="Probe_type" class="easyui-textbox" style="width: 180px; height: 22px; margin-left: 5px" />
            </div>
            <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                <p class="h_d1" style="width: 110px; text-align: right;">制造商:</p>
                <input id="Probe_Manufacture" name="Probe_Manufacture" class="easyui-textbox" style="width: 180px; height: 22px; margin-left: 5px" />
            </div>
            <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                <p class="h_d1" style="width: 110px; text-align: right;">探头尺寸:</p>
                <input id="Probe_size" name="Probe_size" class="easyui-textbox" style="width: 180px; height: 22px; margin-left: 5px" />
            </div>
            <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                <p class="h_d1" style="width: 110px; text-align: right;">频率:</p>
                <input id="Probe_frequency" name="Probe_frequency" class="easyui-textbox" style="width: 180px; height: 22px; margin-left: 5px" />
            </div>
            <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                <p class="h_d1" style="width: 110px; text-align: right;">线圈尺寸:</p>
                <input id="Coil_Size" name="Coil_Size" class="easyui-textbox" style="width: 180px; height: 22px; margin-left: 5px" />
            </div>
            <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                <p class="h_d1" style="width: 110px; text-align: right;">探头长度:</p>
                <input id="Probe_Length" name="Probe_Length" class="easyui-textbox" style="width: 180px; height: 22px; margin-left: 5px" />
            </div>
            <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                <p class="h_d1" style="width: 110px; text-align: right;">探头扩展线缆长度:</p>
                <input id="Cable_Length" name="Cable_Length" class="easyui-textbox" style="width: 180px; height: 22px; margin-left: 5px" />
            </div>
            <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                <p class="h_d1" style="width: 110px; text-align: right;">波型L:</p>
                <input id="Mode_L" name="Mode_L" class="easyui-textbox" style="width: 180px; height: 22px; margin-left: 5px" />
            </div>
            <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                <p class="h_d1" style="width: 110px; text-align: right;">波型T:</p>
                <input id="Mode_T" name="Mode_T" class="easyui-textbox" style="width: 180px; height: 22px; margin-left: 5px" />
            </div>
            <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                <p class="h_d1" style="width: 110px; text-align: right;">晶片尺寸:</p>
                <input id="Chip_size" name="Chip_size" class="easyui-textbox" style="width: 180px; height: 22px; margin-left: 5px" />
            </div>
            <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                <p class="h_d1" style="width: 110px; text-align: right;">角度:</p>
                <input id="Angle" name="Angle" class="easyui-textbox" style="width: 180px; height: 22px; margin-left: 5px" />
            </div>
            <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                <p class="h_d1" style="width: 110px; text-align: right;">标称角度:</p>
                <input id="Nom_Angle" name="Nom_Angle" class="easyui-textbox" style="width: 180px; height: 22px; margin-left: 5px" />
            </div>
            <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                <p class="h_d1" style="width: 110px; text-align: right;">楔块:</p>
                <input id="Shoe" name="Shoe" class="easyui-textbox" style="width: 180px; height: 22px; margin-left: 5px" />
            </div>
            <div class="h_dline" style="height: 23px; margin-top: 10px; float: left; margin-left: 10px">
                <p class="h_d1" style="width: 110px; text-align: right;">状态:</p>
                <input id="Probe_state" name="Probe_state" required="required" class="easyui-combobox" style="width: 180px; height: 22px; margin-left: 5px" />
            </div>
            <div class="h_dline" style="height: 23px; margin-top: 10px; margin-left: 10px; float: left">
                <p class="h_d1" style="width: 110px; text-align: right;">附注:</p>
                <input id="remarks" name="remarks" class="easyui-textbox" data-options="multiline:true" style="width: 400px; height: 66px; margin-left: 5px" />
            </div>
        </form>


    </div>

</body>
            <%=WebExtensions.CombresLink("Probe_Library_Js") %>

</html>
