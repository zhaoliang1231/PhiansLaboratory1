
var firstload = 0;
$(function () {
    //iframe可用高度
    var _height = screen.availHeight;
    $("#cc").css("height", _height);
    $("#Laboratory_tree_toolbar").css("display", "block");
    //初始化测试项目树
    load_tree();
    //条件列表
    contation_datagrid_load();
    //测试参数加载初始化
    Test_parameters_load();
    ////性能参数加载初始化
    Performance_parameter_load();
    ////员工加载初始化
    employee_load();
    ////易耗品加载初始化
    Consumables_load();
    ////夹具加载初始化
    Fixture_load();
    ////设备加载初始化
    Equipment_load();
    ////控制器加载初始化
    Control_load();
    ////报告加载初始化
    Report_load();
    // 查看温度湿度
    $('#View_temprature_Btn').unbind('click').bind('click', function () {

        View_tempratureInit();

    });




    //是否是父项目1
    $('#Isparent1').unbind('click').bind('click', function () {
        $("#Isparent1").prop("checked", "checked");
        $("#Isparent2").prop("checked", false);
        $("#Isparent").val("true");
    });
    //是否显示 0
    $('#Isparent2').unbind('click').bind('click', function () {
        $("#Isparent2").prop("checked", "checked");
        $("#Isparent1").prop("checked", false);
        $("#Isparent").val("false");
    });
    //AutoFlag1
    $('#AutoFlag1').unbind('click').bind('click', function () {
        $("#AutoFlag1").prop("checked", "checked");
        $("#AutoFlag2").prop("checked", false);
        $("#AutoFlag").val("true");
    });
    //AutoFlag 0
    $('#AutoFlag2').unbind('click').bind('click', function () {
        $("#AutoFlag2").prop("checked", "checked");
        $("#AutoFlag1").prop("checked", false);
        $("#AutoFlag").val("false");
    });
    // 性能参数 Commonflag 共有参数
    $('#Commonflag1').unbind('click').bind('click', function () {
        $("#Commonflag1").prop("checked", "checked");
        $("#Commonflag2").prop("checked", false);
        $("#Commonflag").val("true");
    });
    //Commonflag 0
    $('#Commonflag2').unbind('click').bind('click', function () {
        $("#Commonflag2").prop("checked", "checked");
        $("#Commonflag1").prop("checked", false);
        $("#Commonflag").val("false");
    });
    // 测试参数 Commonflag 共有参数
    $('#Commonflag1_').unbind('click').bind('click', function () {
        $("#Commonflag1_").prop("checked", "checked");
        $("#Commonflag2_").prop("checked", false);
        $("#Commonflag_").val("true");
    });
    //Commonflag 0
    $('#Commonflag2_').unbind('click').bind('click', function () {
        $("#Commonflag2_").prop("checked", "checked");
        $("#Commonflag1_").prop("checked", false);
        $("#Commonflag_").val("false");
    });
    /*
     *functionName:
     *function:点击commonflag 是否显示公共参数 测试
     *Param: 
     *author:程媛
     *date:2018-05-12
     */
    $('#Commonflag_yes').unbind('click').bind('click', function () {
        reflesh_test();
    });
    $('#Commonflag_no').unbind('click').bind('click', function () {
        reflesh_test();
    });
    /*
   *functionName:
   *function:点击commonflag 是否显示公共参数 性能
   *Param: 
   *author:程媛
   *date:2018-05-12
   */
    $('#Commonflag_true').unbind('click').bind('click', function () {
        reflesh_Parameter();
    });
    $('#Commonflag_false').unbind('click').bind('click', function () {
        reflesh_Parameter();
    });
    //RangeFlag
    $("#RangeFlag").combobox({
        data: [
                { 'value': 'A＜X', 'text': 'A＜X' },
                { 'value': 'A≦X', 'text': 'A≦X' },
                { 'value': 'X＜B', 'text': 'X＜B' },
                { 'value': 'X≦B', 'text': 'X≦B' },
                { 'value': 'A＜X＜B', 'text': 'A＜X＜B' },
                { 'value': 'A≦X＜B', 'text': 'A≦X＜B' },
                { 'value': 'A＜X≤B', 'text': 'A＜X≤B' },
                 { 'value': 'A≤X≤B', 'text': 'A≤X≤B' }
        ], onSelect: function () {
            var range_text = $("#RangeFlag").combobox('getText')
            if (range_text == "A＜X" || range_text == "A≦X") {
                //要求值2隐藏
                $(".hide_div").css("display", "none");
            }
            else {
                $(".hide_div").css("display", "block");
            };
            if (range_text == "X＜B" || range_text == "X≦B") {
                //要求值1隐藏
                $(".hide_div1").css("display", "none");
            }
            else {
                $(".hide_div1").css("display", "block");
            };
        }
    });
    //RangeFlag1
    $("#CompareRangeFlag").combobox({
        data: [
                { 'value': 'A＜X', 'text': 'A＜X' },
                { 'value': 'A≦X', 'text': 'A≦X' },
                { 'value': 'X＜B', 'text': 'X＜B' },
                { 'value': 'X≦B', 'text': 'X≦B' },
                { 'value': 'A＜X＜B', 'text': 'A＜X＜B' },
                { 'value': 'A≦X＜B', 'text': 'A≦X＜B' },
                { 'value': 'A＜X≤B', 'text': 'A＜X≤B' },
                 { 'value': 'A≤X≤B', 'text': 'A≤X≤B' }
        ], onSelect: function () {
            var range_text = $("#CompareRangeFlag").combobox('getText')
            if (range_text == "A＜X" || range_text == "A≦X") {
                //要求值2隐藏
                $(".hide_div_").css("display", "none");
            }
            else {
                $(".hide_div_").css("display", "block");
            };
            if (range_text == "X＜B" || range_text == "X≦B") {
                //要求值1隐藏
                $(".hide_div1_").css("display", "none");
            }
            else {
                $(".hide_div1_").css("display", "block");
            };
        }
    });

});
//编辑后默认选中
var line = 0;
var line_ = 0;
var line1_ = 0;
//获取url中的参数
function getUrlParam(name) {
    var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)"); //构造一个含有目标参数的正则表达式对象
    var r = window.location.search.substr(1).match(reg); //匹配目标参数
    if (r != null) return unescape(r[2]); return null; //返回参数值
}
//获取url链接传参
var MTRNO = unescape(getUrlParam('MTRNO'));
//初始化测试项目树
function load_tree() {
    $('#Laboratory_tree').tree({
        url: "/ScheduleManagement/LoadDVPProjectTree",
        method: 'post',
        required: true,
        queryParams: {
            MTRNO: MTRNO
        },
        onContextMenu: function (e, node) {
            e.preventDefault();
            $('#Laboratory_tree').tree('select', node.target);
            $('#keyMenu').menu('show', {
                left: e.pageX,
                top: e.pageY
                //onClick: function (item) {
                //    alert(item.name);
                //    remove();
                //}
            });
        },
        //onBeforeExpand: function (node, param) {
        //    $('#Laboratory_tree').tree('options').url = "/ScheduleManagement/LoadDVPProjectTree?ParentId=" + node.id;
        //},
        onSelect: function () {
            ////如果选中项目,项目人员回显
            employee_authorize();
            ////如果选中项目,易耗品回显
            Consumables_authorize();
            ////如果选中项目,夹具回显
            Fixture_authorize();
            ////如果选中项目,设备回显
            Equipment_authorize();
            ////如果选中项目,控制器回显
            Control_authorize();
            ////如果选中项目,性能回显
            contation_datagrid_load_authorize();
            ////保存检验条件回显
            save_contation_View();
            Test_Parameter_authorize();
            ////如果选中项目,性能回显
            Performance_Parameter_authorize();
            //加载输出报告参数
            Report_authorize();




        }
    });
    //添加下级项目
    $('#tree_add_next').unbind('click').bind('click', function () {
        var node = $('#Laboratory_tree').tree('getSelected');
        //获取父节点的id
        var parent = $('#Laboratory_tree').tree('getParent', node.target);
        switch (node.Isparent) {
            case "True":
                tree_add_next(); break;
            case "False":
                $.messager.alert('Tips', 'Cannot add next node！'); break;
                defalut: break;
        }
    });
    $('#tree_add_next1').unbind('click').bind('click', function () {
        var node = $('#Laboratory_tree').tree('getSelected');
        //获取父节点的id
        var parent = $('#Laboratory_tree').tree('getParent', node.target);
        switch (node.Isparent) {
            case "True":
                tree_add_next(); break;
            case "False":
                $.messager.alert('Tips', 'Cannot add next node！'); break;
                defalut: break;
        }
    });
    //编辑项目
    $('#tree_edit').unbind("click").bind("click", function () {
        var node = $('#Laboratory_tree').tree('getSelected');
        if (node) {
            //node.text = '修改';  //-->txt-->DB
            editInfo(node);
        }
    });
    //编辑项目
    $('#Item_add1').unbind("click").bind("click", function () {
        var node = $('#Laboratory_tree').tree('getSelected');
        if (node) {
            //node.text = '修改';  //-->txt-->DB
            editInfo(node);
        }
    });
    //删除项目
    $('#tree_del').unbind("click").bind("click", function () {
        deleteInfo()
    });

    $('#tree_del1').unbind("click").bind("click", function () {
        deleteInfo()
    });
    //选择模板
    $('#Choose_Template').unbind("click").bind("click", function () {
        chooseTemplate()
    });
    //查看项目详情
    $('#read_detail').unbind("click").bind("click", function () {
        Item_datagrid()
    });

    //查看项目详情
    $('#View_Sample').unbind("click").bind("click", function () {
        View_Sample()
    });
    //保存检验条件
    //保存检验条件
    $("#submit_contation").unbind("click").bind("click", function () {
        var node = $('#Laboratory_tree').tree('getSelected');
        if (node) {
            //获取父节点的id
            var parent = $('#Laboratory_tree').tree('getParent', node.target);
            switch (node.Isparent) {
                case "True":
                    saveHtml(node.id, node.text); break;
                case "False":
                    saveHtml(parent.id, parent.text); break;
                    defalut: break;
            }
        } else {
            $.messager.alert('Tips', 'Please select the tree to be operated！');
        }

    });
    // 选择对比项目
    $('#Choose_Compare_Btn').unbind('click').bind('click', function () {
        Choose_Compare_Tree_init();
    });

    //保存检验标准
    //保存检验标准
    $("#submit_standard").unbind("click").bind("click", function () {
        var node = $('#Laboratory_tree').tree('getSelected');

        if (node) {
            //获取父节点的id
            var parent = $('#Laboratory_tree').tree('getParent', node.target);
            switch (node.Isparent) {
                case "True":
                    saveHtml1(node.id, node.text); break;
                case "False":
                    saveHtml1(parent.id, parent.text); break;
                    defalut: break;
            }
        } else {
            $.messager.alert('Tips', 'Please select the tree to be operated！');
        }

    });
}
/*
 *functionName:Choose_Compare_Tree_init
 *function:选择对比项目
 *Param 参数
 *author:张慧敏
 *date:2018/6/08
*/
function Choose_Compare_Tree_init() {
    var node = $('#Laboratory_tree').tree('getSelected');
    $('#Compare_Item_tree').tree({
        url: "/ScheduleManagement/LoadDVPProjectTree",
        method: 'post',
        required: true,
        queryParams: {
            MTRNO: MTRNO
        },
        onContextMenu: function (e, node) {
            e.preventDefault();
            $('#Compare_Item_tree').tree('select', node.target);
            $('#keyMenu').menu('show', {
                left: e.pageX,
                top: e.pageY
                //onClick: function (item) {
                //    alert(item.name);
                //    remove();
                //}
            });
        }
    });
    saveCompare();//确认对比
};
//确认对比
function saveCompare() {
    var node = $('#Laboratory_tree').tree('getSelected');
    if (node) {
        $('#Compare_dialog').dialog({
            title: 'Compare Item',
            width: 800,
            height: 500,
            buttons: [
                {
                    text: 'Save',
                    iconCls: 'icon-ok',
                    handler: function () {
                        var Compare_Item_tree = $('#Compare_Item_tree').tree('getSelected');
                        if (Compare_Item_tree) {
                            $.ajax({
                                url: "/ScheduleManagement/ConfirmCompareTask",
                                type: 'POST',
                                data: {
                                    TaskId: node.id,
                                    CompareTestId: Compare_Item_tree.id
                                },
                                success: function (data) {
                                    if (data) {
                                        var result = $.parseJSON(data);
                                        if (result.Success == true) {
                                            $.messager.alert('tips', result.Message);
                                            $('#Compare_dialog').dialog('close');
                                        } else {
                                            $.messager.alert('tips', result.Message);
                                        }
                                    }

                                }
                            });
                        } else {
                            $.messager.alert('Tips', 'Please select the tree to be operated！');
                        }
                    }
                }
                , {
                    text: 'close',
                    iconCls: 'icon-cancel',
                    handler: function () {
                        $('#Compare_dialog').dialog('close');
                    }
                }]
        });

    }
}
/*
 *functionName:chooseTemplate
 *function:选择模板
 *Param 参数
 *author:张慧敏
 *date:2018/6/09
*/
function chooseTemplate() {

    var node = $('#Laboratory_tree').tree('getSelected');
    if (node) {
        //TemplateCombobox combobox
        $('#TemplateCombobox').combobox({
            url: "/ScheduleManagement/GetAccreditTempList ",//接收一般处理程序返回来的json数据      
            valueField: 'Value',
            textField: 'Text',
            queryParams: {
                ProjectId: node.ProjectId
            },
            required: true,
            //editable: false,
            //本地联系人数据模糊索引
            filter: function (q, row) {
                var opts = $(this).combobox('options');
                //  return row[opts.textField].indexOf(q) >= 0;
                return row[opts.textField].toLowerCase().indexOf(q.toLowerCase()) != -1;
            }
        });
        $('#TemplateCombobox_dialog').dialog({
            title: 'Template',
            width: 600,
            height: 400,
            // fit: true,
            buttons: [{
                text: 'Save',
                iconCls: 'icon-ok',
                handler: function () {
                    $.ajax({
                        url: "/ScheduleManagement/EditDVPTemplateID",
                        type: 'POST',
                        data: {
                            TaskId: node.id,
                            TemplateID: $('#TemplateCombobox').combobox("getValue"),
                            ProjectName: node.text
                        },
                        success: function (data) {
                            if (data) {
                                var result = $.parseJSON(data);
                                if (result.Success == true) {
                                    $.messager.alert('tips', result.Message);
                                    $('#TemplateCombobox_dialog').dialog('close');
                                } else {
                                    $.messager.alert('tips', result.Message);
                                }
                            }

                        }
                    });
                }
            }
                , {
                    text: 'close',
                    iconCls: 'icon-cancel',
                    handler: function () {
                        $('#TemplateCombobox_dialog').dialog('close');
                    }
                }]
        });
    } else {
        $.messager.alert('Tips', 'Please select the tree to be operated！');
    }
}
/*
 *functionName:View_Sample
 *function:查看样品
 *Param 参数
 *author:程媛
 *date:2018/4/28
*/
function View_Sample() {
    //var selectRow = $("#MTR_information_datagrid").datagrid("getSelected");//获取选中行
    //console.log(selectRow)
    var node = $('#Laboratory_tree').tree('getSelected');
    if (node) {
        $('#sample_dialog').dialog({
            title: 'View Sample process',
            width: 800,
            height: 500,
            // fit: true,
            buttons: [{
                text: 'Close',
                iconCls: 'icon-cancel',
                handler: function () {
                    $('#sample_dialog').dialog('close');
                }
            }]
        });
    }

    //MotoNum_Range 点击编号范围显示
    if (node.SampleNo == "") {
        $("#MotoNum_Range").html('(无范围值)');
    } else {
        $("#MotoNum_Range").html('(' + node.SampleNo + ')');
    }
    $('#SampleNo1').textbox({//设置占位符
        prompt: '1#~2#'
    })
    $('#sample_diagrid').datagrid({
        nowrap: false,
        striped: true,
        rownumbers: true,
        border: true,
        fitColumns: true,
        pagination: true,
        ctrlSelect: true,
        fit: true,
        pageSize: 15,
        pageList: [15, 30, 45, 60],
        pageNumber: 1,
        type: 'POST',
        dataType: 'json',
        queryParams: {
            TaskId: node.id,
            SampleNo: node.SampleNo
        },
        //   onClickCell: onClickCell,
        url: "/ScheduleManagement/GetDVPMotorList",
        columns: [[
            { field: 'MotoNum', title: 'SampleNo', width: 100, sortable: 'true' },
            { field: 'Sortnum', title: 'Sortnum', width: 100, sortable: 'true' }
        ]],
        onLoadSuccess: function (data) {
            $('#sample_diagrid').datagrid('selectRow', 0);
        },
        sortName: 'Sortnum',
        sortOrder: 'asc',

        //toolbar: "#sample_information_toolbar"
    });
    //添加点击编号排序
    $("#sample_save").unbind("click").bind('click', function () {
        sample_save(node);
    });
};
/*
 *functionName:View_Sample
 *function:查看样品
 *Param TaskId,MotorList,SortNum,MTRNO,MotorMax
 *author:程媛
 *date:2018/4/28
*/
function sample_save(node) {
    if ($("#SampleNo1").textbox("getValue") == "") {
        $.messager.alert('tips', "The SampleNo can not be empty!");
        return;
    }
    if ($("#Sort").textbox("getValue") == "") {
        $.messager.alert('tips', "The Sort can not be empty!");
        return;
    }
    $.ajax({
        url: "/ScheduleManagement/EditDVPMotorSortnum",
        type: 'POST',
        dataType: 'json',
        data: {
            TaskId: node.id,
            MotorList: $("#SampleNo1").textbox("getText"),
            SortNum: $("#Sort").textbox("getText"),
            MotorMax: node.SampleNo,
            MTRNO: MTRNO
        },
        success: function (data) {
            //  var result = $.parseJSON(data);
            if (data.Success == true) {
                $.messager.alert('tips', data.Message);
                $('#sample_diagrid').datagrid("reload");
                $("#SampleNo1").textbox("setText", "");
                $("#Sort").textbox("setText", "");
            } else {
                $.messager.alert('tips', data.Message);
            }
        }
    });
};
/*
 * 
 *functionName:save_contation_View()
 *function://////保存检验条件回显
 *Param 参数
 *author:张慧敏
 *date:2018/4/17
*/
function save_contation_View() {
    var node = $('#Laboratory_tree').tree('getSelected');
    //获取文本值
    // var Method = document.getElementById("editor1").value;
    if (node) {
        var node = $('#Laboratory_tree').tree('getSelected');
        //获取文本值
        // var Method = document.getElementById("editor1").value;
        if (node) {
            //获取父节点的id
            var parent = $('#Laboratory_tree').tree('getParent', node.target);
            switch (node.Isparent) {
                case "True":
                    document.getElementById("EditIframe").src = "/ScheduleManagement/KindeditorEdit?TaskId=" + node.id;
                    //  getHtml(node.id);
                    break;
                case "False":
                    document.getElementById("EditIframe").src = "/ScheduleManagement/KindeditorEdit?TaskId=" + parent.id;
                    // getHtml(parent.id);
                    break;
                    defalut: break;
            }
            //传父id还是子id
            //function getHtml(id) {
            //    $.ajax({
            //        url: "/ScheduleManagement/GetHtmlInfo",
            //        type: 'POST',
            //        // dataType: 'html',
            //        data: {
            //            TaskId: id,
            //            //   ProjectName: node.text,
            //            flag: "0",
            //            //   Method: Method

            //        },
            //        success: function (data) {
            //            if (data) {
            //                var result = $.parseJSON(data);

            //                KindEditor.html('#editor', result.MainDescription);
            //                KindEditor.html('#editor1', result.Method);
            //            }

            //        }
            //    });
            //}
        } else {
            $.messager.alert('Tips', 'Please select the tree to be operated！');
        }

    }
}
/*
 * 
 *functionName:saveHtml()
 *function://保存html文本 检验条件
 *Param 参数id  节点id,text 节点名
 *author:张慧敏
 *date:2018/4/17
*/

function saveHtml(id, text) {
    //获取文本值encodeHtml = HtmlUtil.htmlEncode(html);
    var MainDescription = encode(document.getElementById("editor").value);
    $.ajax({
        url: "/ScheduleManagement/EditHtmlInfo",
        type: 'POST',
        dataType: 'html',
        data: {
            TaskId: id,
            ProjectName: text,
            flag: "0",
            MainDescription: MainDescription
        },
        success: function (data) {
            if (data) {
                var result = $.parseJSON(data);
                if (result.Success == true) {
                    $.messager.alert('tips', result.Message);
                } else {
                    $.messager.alert('tips', result.Message);
                }
            }

        }
    });
}
/*
 * 
 *functionName:saveHtml()
 *function://保存html文本 检验标准
 *Param 参数id  节点id,text 节点名
 *author:张慧敏
 *date:2018/4/17
*/

function saveHtml1(id, text) {
    //获取文本值
    var Method = encode(document.getElementById("editor1").value);
    $.ajax({
        url: "/ScheduleManagement/EditHtmlInfo",
        type: 'POST',
        dataType: 'html',
        data: {
            TaskId: id,
            ProjectName: text,
            flag: "1",
            Method: Method
        },
        success: function (data) {
            if (data) {
                var result = $.parseJSON(data);
                if (result.Success == true) {
                    $.messager.alert('tips', result.Message);
                } else {
                    $.messager.alert('tips', result.Message);
                }
            }

        }
    });
}
//查看项目详情
function Item_datagrid() {
    var node = $('#Laboratory_tree').tree('getSelected');
    if (node) {
        $('#Test_item_dialog').dialog({
            title: 'Viem information',
            width: 400,
            height: 300,
            left: 50,
            top: 100,
            buttons: [{
                text: 'Close',
                iconCls: 'icon-cancel',
                handler: function () {
                    $('#Test_item_dialog').dialog('close');
                }
            }]
        });
        // form数据回显
        //$.ajax({
        //  //  url: "/ScheduleManagement/LoadProjectInfo",
        //    type: 'POST',
        //    data: {
        //        rows: "10",
        //        page: "1",
        //        id: node.id,
        //    },
        //    success: function (data) {
        //        if (data) {
        //            var obj = $.parseJSON(data);
        //            // form数据回显
        //            $('#Test_item_dialog').form('load', obj.rows[0]);
        //            if (obj.rows[0].Flag == true) {
        //                $("#Flag1").prop("checked", "checked");
        //                $("#Flag2").prop("checked", false);
        //                $("#Flag").val("true");
        //            } else {
        //                $("#Flag2").prop("checked", "checked");
        //                $("#Flag1").prop("checked", false);
        //                $("#Flag").val("false");
        //            }
        //        }

        //    }
        //});
    } else {
        $.messager.alert('Tips', 'Please select the tree to be operated！');
    }

}
//*************************添加下级测试项目
function tree_add_next() {
    var node_add = $('#Laboratory_tree').tree('getSelected');

    if (!node_add) {
        $.messager.alert('Tips', 'Please select the tree to be operated！');
        return;
    }
    //默认传  var Isparent = "False"; 
    var Isparent = "False";
    //判断项目是主项目还是子项目,显示添加项不同
    if (node_add.Isparent == "True" && node_add.id == "a447d88b-c84a-4746-9d78-2842011def77") {
        //$("#Sub_num").css("display", "none");
        Isparent = "True";
        //$("#Isparent_").css("display", "inline-block");
        $("#Project").css("display", "none");

    } else {
        // $("#Sub_num").css("display", "inline-block");
        // $("#Isparent_").css("display", "none");
        Isparent = "False";
        $("#Project").css("display", "inline-block");
    }
    $("#ProjectName").textbox({
        readonly: false
    });
    //加载左边树
    TestTitle_tree();
    $('#Item_dialog1').form('reset');
    // $("#SampleNo").textbox("setText", node_add.SampleNo);
    // $("#SampleNo").textbox().focus();
    $('#Item_dialog1').dialog({
        title: 'Add Chirdren Item',
        width: 800,
        height: 600,
        buttons: [{
            text: 'Save',
            iconCls: 'icon-save',
            handler: function () {
                var node_tree = $('#TestTitle_tree').tree('getSelected');
                if (node_tree) {
                    if (node_tree.state == "closed") {
                        $.messager.alert('tips', "this tree has children");
                        return;
                    }
                    $("#ProjectId_").val(node_tree.id);//赋值到一个隐藏的input
                    $('#Item_dialog1').form('submit', {
                        url: "/ScheduleManagement/AddProject",//接收一般处理程序返回来的json数据     
                        onSubmit: function (param) {
                            param.ParentID = node_add.id;
                            param.MTRNO = MTRNO;
                            param.SampleQty = node_add.SampleQty;
                            param.Isparent = Isparent;
                            // param.ProjectId = node_tree.id;
                            return $(this).form('enableValidation').form('validate');
                        },
                        success: function (data) {
                            if (data) {
                                var result = $.parseJSON(data);
                                if (result.Success == true) {
                                    //更新当前选中的节点
                                    var node1 = $('#Laboratory_tree').tree('getSelected');
                                    var node2 = $('#TestTitle_tree').tree('getSelected');
                                    if (node1) {
                                        $('#Laboratory_tree').tree('append', {
                                            parent: node1.target,
                                            data: [{
                                                Isparent: Isparent,
                                                id: result.Data.TaskId,
                                                ProjectId: node2.id,
                                                text: $("#ProjectName").textbox("getText")
                                            }]
                                        });
                                    }

                                    $('#Item_dialog1').dialog('close');

                                    $.messager.alert('tips', result.Message);
                                } else {

                                    $.messager.alert('tips', result.Message);
                                }
                            }

                        }
                    });
                } else {
                    $.messager.alert('tips', "choose this tree");
                }


            }

        }, {
            text: 'close',
            iconCls: 'icon-cancel',
            handler: function () {
                $('#Item_dialog1').dialog('close');
            }
        }]

    });

    ////将MTR单回显
    $("#MTRNO").textbox("setText", MTRNO);
    ////样品编号回显
    $("#SampleNo").textbox("setText", node_add.SampleNo);
    $("#MTRNO").textbox('textbox').focus();


}
//加载项目树给与选择
function TestTitle_tree() {

    $('#TestTitle_tree').tree({
        url: "/ScheduleManagement/LoadProjectTree",
        method: 'post',
        required: true,
        onContextMenu: function (e, node) {

        },
        onBeforeExpand: function (node, param) {
            $('#TestTitle_tree').tree('options').url = "/ScheduleManagement/LoadProjectTree?ParentId=" + node.id;
        },
        onSelect: function () {
            var node1 = $('#TestTitle_tree').tree('getSelected');
            $('#ProjectId').textbox("setText", node1.text);
        }
    });

}
//**************************编辑节点
function editInfo(node) {
    TestTitle_tree();
    //项目名称不可编辑
    if (node.Isparent == "True") {
        $("#Project").css("display", "none");
        $("#ProjectName").textbox({
            readonly: true
        });
    } else {
        $("#Project").css("display", "inline-block");
        $("#ProjectName").textbox({
            readonly: false
        });
    }
    //判断是否是主项目，主项目就显示样品编号框,子项目就隐藏
    if (node.Isparent == "True") {
        $("#SampleText").css("display", "block");
    } else {
        $("#SampleText").css("display", "none");
    }
    var node1 = $('#TestTitle_tree').tree('getSelected');
    $('#Item_dialog1').dialog({
        title: 'Edit Item',
        width: 800,
        height: 600,
        beforeSubmit: function (formData, jqForm, options) {//提交前的回调方法
            //return $("#Item_dialog1").form('validate');
        },
        onOpen: function () {

        },
        buttons: [{
            text: 'Save',
            iconCls: 'icon-save',
            handler: function () {
                $('#Item_dialog1').form('submit', {
                    url: "/ScheduleManagement/EditProject",//接收一般处理程序返回来的json数据     
                    onSubmit: function (param) {
                        param.TaskId = node.id;
                        param.MTRNO = MTRNO;
                        param.SampleNo = node.SampleNo;
                        return $(this).form('enableValidation').form('validate');
                        //  param.ProjectId = ;
                    },
                    success: function (data) {
                        var result = $.parseJSON(data);
                        if (result.Success == true) {
                            var node1 = $('#Laboratory_tree').tree('getSelected');
                            if (node1) {
                                $('#Laboratory_tree').tree('update', {
                                    SampleNo: $('#SampleNo').textbox('getText'),
                                    target: node.target,
                                    text: $('#ProjectName').textbox('getText')
                                });
                            }
                            $('#Item_dialog1').dialog('close');

                            $.messager.alert('tips', result.Message);
                        } else {

                            $.messager.alert('tips', result.Message);
                        }
                    }
                });
                ////重置表单
                //$('#Item_dialog1').form("reset");
            }

        }, {
            text: 'close',
            iconCls: 'icon-cancel',
            handler: function () {
                $('#Item_dialog1').dialog('close');
            }
        }]
    });
    // form数据回显
    $.ajax({
        url: "/ScheduleManagement/LoadProjectShow",
        type: 'POST',
        data: {
            rows: "10",
            page: "1",
            TaskId: node.id,
        },
        success: function (data) {
            if (data) {
                var obj = $.parseJSON(data);
                if (obj.rows[0]) {
                    // form数据回显
                    $('#Item_dialog1').form('load', obj.rows[0]);
                    //  $("#ProjectId").combobox("setValue", obj.rows[0].ProjectId);
                    $("#MTRNO").textbox("setText", MTRNO);
                    $("#SampleNo").textbox("setText", obj.rows[0].SampleNo);
                    $("#SampleNo").textbox().focus();
                    if (obj.rows[0].Isparent == true) {
                        $("#Isparent_").css("display", "inline-block");
                        $("#Isparent1").prop("checked", true);
                        $("#Isparent2").prop("checked", false);
                        $("#Isparent").val("true");
                    } else {
                        $("#Isparent_").css("display", "none");
                        $("#Isparent2").prop("checked", true);
                        $("#Isparent1").prop("checked", false);
                        $("#Isparent").val("false");
                    }
                }

            }

        }
    });

}
//shanc    //删除一个节点
function deleteInfo() {
    var nodes = $('#Laboratory_tree').tree('getSelected');
    if (nodes) {//id小于29的组都为必要组
        $.messager.confirm('confirm', 'confirm delete？', function (r) {
            if (!r) {
                return false;
            } else {
                $.ajax({
                    url: '/ScheduleManagement/DelProject',
                    type: 'POST',
                    data: {
                        TaskId: nodes.id,
                        ProjectName: nodes.text
                    },
                    success: function (data) {
                        if (data) {
                            var result = $.parseJSON(data);
                            if (result.Success == true) {
                                //选择当前添加节点
                                $('#Laboratory_tree').tree('remove', nodes.target);

                                $.messager.alert('tips', result.Message);
                            } else {

                                $.messager.alert('tips', result.Message);
                            }
                        }

                    }
                })
            }
        });


    }
};
/*
*functionName:contation_datagrid_load
*function:性能参数条件记录
*Param 
*author:张慧敏
*date:2018-05-02
*/
function contation_datagrid_load() {
    $('#contation_datagrid').datagrid({
        nowrap: false,
        striped: true,
        //rownumbers: true,
        singleSelect: true,
        //autoRowHeight: true,
        //border: false,
        // fitColumns: true,
        fit: true,
        // title: 'Condition',
        pagination: true,
        pageSize: 40,
        queryParams: {
            //id:node.id
        },
        //    height: 390,
        //  onClickCell: onClickCell1,
        pageNumber: 1,
        type: 'POST',
        //   url: '/ScheduleManagement/GetOutputConditionList',
        dataType: "json",
        columns: [[
            { field: 'ConditionName', title: 'ConditionName', width: 150 },
            {
                field: 'Remark', title: 'remarks', width: 150, formatter: function (value, row, index) {
                    if (value) {
                        if (value.length > 30) {
                            var result = value.replace(" ", "");//去空
                            var value1 = result.substr(0, 30);
                            return '<span  title=' + value + '>' + value1 + "......" + '</span>';
                        } else {
                            return '<span  title=' + value + '>' + value + '</span>';
                        }
                    }
                }
            }
        ]],
        onLoadSuccess: function (data) {
            $('#contation_datagrid').datagrid('selectRow', 0);
        },
        onSelect: function (data) {
            var sele = $('#contation_datagrid').datagrid('getSelected');
            if (sele) {
                ////如果选中项目测试回显
                Test_Parameter_authorize();
                ////如果选中项目,性能回显
                Performance_Parameter_authorize();
            }


        },
        toolbar: contation_datagrid_toolbar
    });
    //定义pagination加载内容
    //var p = $('#contation_datagrid').datagrid('getPager');
    //(p).pagination({
    //    layout: ['first', 'prev', 'last', 'next']

    //});
    var json = {
        "rows": [],
        "total": 0,
        "success": true
    };
    $('#contation_datagrid').datagrid("loadData", json);
    //添加
    $("#contation_datagrid_add").unbind("click").bind("click", function () {
        contation_datagrid_add();
    });
    //删除参数
    $("#contation_datagrid_delete").unbind("click").bind("click", function () {
        contation_datagrid_delete(); //参数

    });
    //编辑参数参数
    $("#contation_datagrid_edit").unbind("click").bind("click", function () {
        contation_datagrid_edit(); //编辑参数
    });
    //添加模板
    $("#template_add").unbind("click").bind("click", function () {
        template_add(); //

    });
}
//条件搜索初始化回显
function contation_datagrid_load_authorize() {
    //如果选中项目,项目人员回显
    var node_on = $('#Laboratory_tree').tree('getSelected');
    if (node_on) {
        $('#contation_datagrid').datagrid({
            //fit: true,
            queryParams: {
                TaskId: node_on.id
            },
            url: '/ScheduleManagement/GetOutputConditionList',

        });

        ////定义pagination加载内容
        //var p1_employee_authorize = $('#contation_datagrid').datagrid('getPager');
        //(p1_employee_authorize).pagination({
        //    layout: ['first', 'prev', 'last', 'next']
        //});
    }
}
//添加条件参数dialog
function contation_datagrid_add() {
    var node = $('#Laboratory_tree').tree('getSelected');
    if (node) {
        $('#contation_datagrid_dialog').dialog({
            title: 'Contation Add',
            width: 600,
            height: 350,
            buttons: [
                {
                    text: 'Save',
                    iconCls: 'icon-ok',
                    handler: function () {
                        $('#contation_datagrid_dialog').form('submit', {
                            url: "/ScheduleManagement/AddOutputCondition",//接收一般处理程序返回来的json数据     
                            onSubmit: function (param) {
                                param.TaskId = node.id,
                                param.ProjectName = node.text
                                return $(this).form('enableValidation').form('validate');//验证表单
                            },
                            success: function (data) {
                                if (data) {
                                    var result = $.parseJSON(data);
                                    if (result.Success == true) {
                                        $('#contation_datagrid_dialog').dialog('close');//关闭编辑部门管理树节点弹窗
                                        $('#contation_datagrid').datagrid('reload');
                                        $.messager.alert('Tips', result.Message);
                                    } else {
                                        $.messager.alert('Tips', result.Message);
                                    }
                                }
                            }
                        });
                    }
                },
                {
                    text: 'close',
                    iconCls: 'icon-cancel',
                    handler: function () {
                        $('#contation_datagrid_dialog').dialog('close');
                    }
                }]
        });
    } else {
        $.messager.alert('Tips', 'Please select the line！');
    }
    $('#contation_datagrid_dialog').form("reset");
}
//条件查询授权加载添加
function contation_datagrid_edit() {
    var node_on = $('#Laboratory_tree').tree('getSelected');
    var selectdatagrid = $('#contation_datagrid').datagrid('getSelected');
    if (selectdatagrid) {
        $('#contation_datagrid_dialog').dialog({
            title: 'Contation Edit',
            width: 600,
            height: 350,
            buttons: [
                {
                    text: 'Save',
                    iconCls: 'icon-ok',
                    handler: function () {
                        $('#contation_datagrid_dialog').form('submit', {
                            url: "/ScheduleManagement/EditOutputCondition",//接收一般处理程序返回来的json数据     
                            onSubmit: function (param) {
                                param.TaskId = node_on.id,
                                param.ID = selectdatagrid.ID,
                                 param.ProjectName = node_on.text
                                return $(this).form('enableValidation').form('validate');//验证表单
                            },
                            success: function (data) {
                                var result = $.parseJSON(data);
                                if (result.Success == true) {
                                    $('#contation_datagrid_dialog').dialog('close');//关闭编辑部门管理树节点弹窗
                                    $('#contation_datagrid').datagrid('reload');
                                    $.messager.alert('Tips', result.Message);
                                } else {
                                    $.messager.alert('Tips', result.Message);
                                }
                            }
                        });
                    }
                },
                {
                    text: 'close',
                    iconCls: 'icon-cancel',
                    handler: function () {
                        $('#contation_datagrid_dialog').dialog('close');
                    }
                }]
        });
    } else {
        $.messager.alert('Tips', 'Please select the tree to be operated！');
    }
    var selectRow = $("#contation_datagrid").datagrid("getSelected");
    $('#contation_datagrid_dialog').form('load', selectRow);

}
//删除添加条件参数
function contation_datagrid_delete() {
    var node_on1 = $('#Laboratory_tree').tree('getSelected');
    var selectRow_contation_datagrid = $('#contation_datagrid').datagrid('getSelected');
    if (node_on1) {
        if (selectRow_contation_datagrid) {
            $.messager.confirm('confirm', 'confirm delete?', function (r) {
                if (r) {
                    $.ajax({
                        url: "/ScheduleManagement/DelOutputCondition",
                        type: 'POST',
                        data: {
                            TaskId: node_on1.id,
                            Parameter: selectRow_contation_datagrid.Parameter,
                            ID: selectRow_contation_datagrid.ID,
                            ProjectName: node_on1.text
                        },
                        success: function (data) {
                            var result = $.parseJSON(data);
                            if (result.Success == true) {
                                $("#contation_datagrid").datagrid("reload");
                                $("#Test_parameters_info_datagrid").datagrid("reload");
                                $("#Performance_parameters_info_datagrid").datagrid("reload");
                                $.messager.alert('tips', result.Message);
                            } else {

                                $.messager.alert('tips', result.Message);
                            }
                        }
                    });
                }
            });
        }
        else {
            $.messager.alert('Tips', 'Please select the tree to be operated！');
        }
    }
}
//性能参数初始化化加载//性能参数初始化化加载**************************************//性能参数初始化化加载*****************************************************
function Performance_parameter_load() {
    ////性能参数
    $('#Performance_parameters_info_datagrid').datagrid({
        nowrap: false,
        striped: true,
        //rownumbers: true,
        singleSelect: true,
        //autoRowHeight: true,
        //border: false,
        // fitColumns: true,
        fit: true,
        // title: 'Performance_parameter',
        pagination: true,
        pageSize: 40,
        height: 390,
        // onClickCell: onClickCell,
        pageNumber: 1,
        type: 'POST',
        //  url: '/ScheduleManagement/GetAddedParameterValueList',
        dataType: "json",
        columns: [[
           { field: 'Parameter', title: 'Parameter', width: 100 },
            { field: 'ParameterType_n', title: 'Type', width: 100, hidden: true },
            {
                field: 'Flag', title: 'Auto'
            },
              //{
              //    field: 'RangeFlag', title: 'Range', width: 100
              //},
              { field: 'ParameterUnit_n', title: 'Unit', width: 100 },
            {
                field: 'SpecificationsLower', title: 'lower limit'
            },
             {
                 field: 'SpecificationsTOP', title: 'upper limit', width: 100
             },
              {
                  field: 'CompareLower', title: 'CompareLower'
              },
             {
                 field: 'CompareTOP', title: 'CompareTOP', width: 100
             },
             {
                 field: 'Commonflag', title: 'Common', width: 100, formatter: function (value, row, index) {
                     if (value == false) {
                         return 'No'
                     }
                     if (value == true) {
                         return 'Yes'
                     }
                 }
             }
            //{
            //    field: 'remarks', title: 'remarks', formatter: function (value, row, index) {
            //        if (value) {
            //            if (value.length > 30) {
            //                var result = value.replace(" ", "");//去空
            //                var value1 = result.substr(0, 30);
            //                return '<span  title=' + value + '>' + value1 + "......" + '</span>';
            //            } else {
            //                return '<span  title=' + value + '>' + value + '</span>';
            //            }
            //        }
            //    }
            //}
        ]],
        onLoadSuccess: function (data) {
            $('#Performance_parameters_info_datagrid').datagrid('selectRow', 0);

        },
        onSelect: function () {
            //选中后回显
            var select = $('#Performance_parameters_info_datagrid').datagrid('getSelected');

            if (select) {
                //ParameterType combobox
                $('#ParameterUnit1').combobox({
                    url: "/ScheduleManagement/LoadParameterUnitInfo ",//接收一般处理程序返回来的json数据      
                    valueField: 'Value',
                    textField: 'Text',
                    required: true,
                    //editable: false,
                    //本地联系人数据模糊索引
                    filter: function (q, row) {
                        var opts = $(this).combobox('options');
                        //  return row[opts.textField].indexOf(q) >= 0;
                        return row[opts.textField].toLowerCase().indexOf(q.toLowerCase()) != -1;
                    }
                });
                $("#add_Performance_detail").form("load", select);
                $("#RangeFlag").combobox("setValue", select.RangeFlag);
                $("#CompareRangeFlag").combobox("setValue", select.CompareRangeFlag);
                ////复选框回显
                if (select.Flag == true) {
                    $("#AutoFlag1").prop("checked", "checked");
                    $("#AutoFlag2").prop("checked", false);
                    $("#AutoFlag").val(true);
                } else {
                    $("#AutoFlag2").prop("checked", "checked");
                    $("#AutoFlag1").prop("checked", false);
                    $("#AutoFlag").val(false);
                }
                ////共有参数复选框回显
                if (select.Commonflag == true) {
                    $("#Commonflag1").prop("checked", "checked");
                    $("#Commonflag2").prop("checked", false);
                    $("#Commonflag").val(true);
                } else {
                    $("#Commonflag2").prop("checked", "checked");
                    $("#Commonflag1").prop("checked", false);
                    $("#Commonflag").val(false);
                }
                $("#RequiredValue1").textbox("setText", select.SpecificationsLower);
                $("#RequiredValue2").textbox("setText", select.SpecificationsTOP);
                $("#min_limit").textbox("setText", select.CompareLower);
                $("#max_limit").textbox("setText", select.CompareTOP);
            }
            //数据范围回显


        },
        toolbar: Performance_parameters_info_toolbar
    });
    //定义pagination加载内容
    //var p = $('#Performance_parameters_info_datagrid').datagrid('getPager');
    //(p).pagination({
    //    layout: ['first', 'prev', 'last', 'next']

    //});
    var json = {
        "rows": [],
        "total": 0,
        "success": true
    };
    $('#Performance_parameters_info_datagrid').datagrid("loadData", json);
    //添加
    $("#Performance_parameters_info_add").unbind("click").bind("click", function () {
        var selectRow_contation_datagrid = $('#contation_datagrid').datagrid('getSelected');
        if (selectRow_contation_datagrid) {
            Performance_parameters_info_add();
        } else {
            $.messager.alert('Tips', 'Please select the Condition to be operated！');
        }

    });
    //删除参数
    $("#Performance_parameters_info_delete").unbind("click").bind("click", function () {
        var selectRow_contation_datagrid = $('#contation_datagrid').datagrid('getSelected');
        if (selectRow_contation_datagrid) {
            Performance_parameters_info_delete(); //添加参数
        } else {
            $.messager.alert('Tips', 'Please select the Condition to be operated！');
        }

    });
    //编辑参数参数
    $("#Performance_parameters_info_edit").unbind("click").bind("click", function () {
        var selectRow_contation_datagrid = $('#contation_datagrid').datagrid('getSelected');
        if (selectRow_contation_datagrid) {
            Performance_parameters_info_edit(); //编辑参数
        } else {
            $.messager.alert('Tips', 'Please select the Condition to be operated！');
        }

    });

}
//************************************************添加性能参数dialog
function Performance_parameters_info_add() {
    var node = $('#Laboratory_tree').tree('getSelected');
    if (node) {
        $('#parameter_dialog').dialog({
            title: 'Add',
            width: 900,
            height: 550,
            buttons: [{
                text: 'close',
                iconCls: 'icon-cancel',
                handler: function () {
                    $('#parameter_dialog').dialog('close');
                }
            }]
        });

        //性能参数授权加载添加
        Performance_parameter_authorize_Add();
    } else {
        $.messager.alert('Tips', 'Please select the tree to be operated！');
    }

}
//性能参数授权加载添加
function Performance_parameter_authorize_Add() {
    var node = $('#Laboratory_tree').tree('getSelected');
    //search
    $("#Parameter_datagrid_search1").combobox({
        data: [
                 { 'value': 'Parameter', 'text': 'Parameter' },
                 { 'value': 'ParameterType', 'text': 'ParameterType' },
                 { 'value': 'Attribut', 'text': 'Attribut' }
        ]
    });
    ////性能参数
    $('#Performance_parameter_Unauthorize').datagrid({
        nowrap: false,
        striped: true,
        //rownumbers: true,
        singleSelect: true,
        //autoRowHeight: true,
        //border: false,
        //fitColumns: true,
        //fit: true,
        title: 'parameter',
        queryParams: {
            ProjectId: node.ProjectId
        },
        url: '/ScheduleManagement/GetAuthPerforParamList',
        pagination: true,
        pageSize: 40,
        pageNumber: 1,
        type: 'POST',
        dataType: "json",
        columns: [[
         { field: 'Parameter', title: 'Parameter', width: 100 },
           { field: 'ParameterType_n', title: 'Type', width: 150, hidden: true }
        ]],
        onDblClickRow: function (index, row) {
            $('#add_Performance_parameter').click();
        },
        onLoadSuccess: function (data) {
            $('#Performance_parameter_Unauthorize').datagrid('selectRow', 0);

        },
        toolbar: Parameter_authorize_toobar
    });
    //定义pagination加载内容
    var p3_2 = $('#Performance_parameter_Unauthorize').datagrid('getPager');
    (p3_2).pagination({
        layout: ['first', 'prev', 'last', 'next']
    });
    //授权参数
    $('#Performance_parameter_authorize').datagrid({
        nowrap: false,
        striped: true,
        //rownumbers: true,
        singleSelect: true,
        //autoRowHeight: true,
        //border: false,
        //fitColumns: true,
        //fit: true,
        title: 'Authorization parameters',
        pagination: true,
        pageSize: 20,
        pageNumber: 1,
        columns: [[
           { field: 'Parameter', title: 'Parameter', width: 100 },
           { field: 'ParameterType_n', title: 'Type', width: 150, hidden: true }

        ]],
        onDblClickRow: function (index, row) {
            $('#remove_Performance_parameter1').click();
        },
        onLoadSuccess: function (data) {
            $('#Performance_parameter_authorize').datagrid('selectRow', 0);

        },
        type: 'POST',
        dataType: "json"
    });
    //定义pagination加载内容
    var p2_3 = $('#Performance_parameter_authorize').datagrid('getPager');
    (p2_3).pagination({
        layout: ['first', 'prev', 'last', 'next', 'efresh']

    });
    //性能参数搜索
    $('#Parameter_datagrid_find').unbind("click").bind("click", function () {
        Performance_parameters_search();
    });
    //添加授性能参数
    $('#add_Performance_parameter').unbind("click").bind("click", function () {
        add_Performance_Parameter();
    })
    //删除授权性能参数
    $('#remove_Performance_parameter1').unbind("click").bind("click", function () {
        remove_Performance_Parameter();
    })
    //测试实验室授权性能参数添加
    function add_Performance_Parameter() {
        var node_on1 = $('#Laboratory_tree').tree('getSelected');
        var selectRow_Performance_parameter_datagrid = $('#Performance_parameter_Unauthorize').datagrid('getSelected');
        if (node_on1) {
            if (selectRow_Performance_parameter_datagrid) {
                //ParameterType combobox
                $('#ParameterUnit1').combobox({
                    url: "/ScheduleManagement/LoadParameterUnitInfo ",//接收一般处理程序返回来的json数据      
                    valueField: 'Value',
                    textField: 'Text',
                    required: true,
                    //editable: false,
                    //本地联系人数据模糊索引
                    filter: function (q, row) {
                        var opts = $(this).combobox('options');
                        //  return row[opts.textField].indexOf(q) >= 0;
                        return row[opts.textField].toLowerCase().indexOf(q.toLowerCase()) != -1;
                    }
                });
                $('#add_Performance_parameter_add').dialog({
                    title: 'Add',
                    width: 700,
                    height: 500,
                    buttons: [{
                        text: 'ok',
                        iconCls: 'icon-ok',
                        handler: function () {
                            //性能参数添加判断+提交 
                            Jsrule_Submit(node_on1, selectRow_Performance_parameter_datagrid);
                        }
                    },
                        {
                            text: 'close',
                            iconCls: 'icon-cancel',
                            handler: function () {
                                $('#add_Performance_parameter_add').dialog('close');
                            }
                        }]
                });

            }
            else {
                $.messager.alert('Tips', 'Please select the row to be operated！');
            }

        } else {
            $.messager.alert('Tips', 'Please select the tree to be operated！');
        }
    }
    //性能参数添加判断+提交 node_on1  selectRow_Performance_parameter_datagrid传参
    function Jsrule_Submit(node_on1, selectRow_Performance_parameter_datagrid) {
        //RangeFlag的值
        var RangeFlag_ = $("#RangeFlag").combobox("getValue");
        //CompareRangeFlag的值
        var CompareRangeFlag_ = $("#CompareRangeFlag").combobox("getValue");
        //判断AutoFlag是否自动
        var AutoFlag_ = $("#AutoFlag").val();
        //获取参照A 和B的值
        var RequiredValue1_ = $("#RequiredValue1").textbox("getText");
        var RequiredValue2_ = $("#RequiredValue2").textbox("getText");
        //获取cpmpare参照A 和B的值
        var CompareValue1_ = $("#min_limit").textbox("getText");
        var CompareValue2_ = $("#max_limit").textbox("getText");
        // 如果是 对下面的数据进行判断
        if (AutoFlag_ == "true") {
            switch (RangeFlag_) {
                case "A＜X":
                    if (!RequiredValue1_) {
                        $.messager.alert('tips', "RequiredValue1 is null");
                        return;
                    }
                    try {
                        parseInt(RequiredValue1_)
                    } catch (e) {
                        $.messager.alert('tips', "RequiredValue1 is char");
                        return;
                    }
                    RequiredValue2_ = "";
                    break;
                case "A≦X":
                    if (!RequiredValue1_) {
                        $.messager.alert('tips', "RequiredValue1 is null");
                        return;
                    }
                    try {
                        parseInt(RequiredValue1_)
                    } catch (e) {
                        $.messager.alert('tips', "RequiredValue1 is char");
                        return;
                    }
                    RequiredValue2_ = "";
                    break;
                case "X＞B":
                    if (!RequiredValue2_) {
                        $.messager.alert('tips', "RequiredValue2 is null");
                        return;
                    }
                    try {
                        parseInt(RequiredValue2_)
                    } catch (e) {
                        $.messager.alert('tips', "RequiredValue2 is char");
                        return;
                    }
                    RequiredValue1_ = "";
                    break;
                case "X≧B":
                    if (!RequiredValue2_) {
                        $.messager.alert('tips', "RequiredValue2 is null");
                        return;
                    }
                    try {
                        parseInt(RequiredValue2_)
                    } catch (e) {
                        $.messager.alert('tips', "RequiredValue2 is char");
                        return;
                    }
                    RequiredValue1_ = "";
                    break;

                case "A＜X＜B": if (!RequiredValue2_ && !RequiredValue1_) {

                    $.messager.alert('tips', "RequiredValue1 or RequiredValue2 is null");
                    return;

                }

                    try {
                        parseInt(RequiredValue1_);
                        parseInt(RequiredValue2_);
                        if (parseInt(RequiredValue1_) >= parseInt(RequiredValue2_)) {
                            $.messager.alert('tips', "RequiredValue1 >= RequiredValue2");
                            return;
                        }
                    } catch (e) {
                        $.messager.alert('tips', "RequiredValue1 or RequiredValue2 is char");
                        return;
                    }
                    break;

                case "A≦X＜B": if (!RequiredValue2_ && !RequiredValue1_) {

                    $.messager.alert('tips', "RequiredValue1 or RequiredValue2 is null");
                    return;

                }

                    try {
                        parseInt(RequiredValue1_);
                        parseInt(RequiredValue2_);
                        if (parseInt(RequiredValue1_) >= parseInt(RequiredValue2_)) {
                            $.messager.alert('tips', "RequiredValue1 >RequiredValue2");
                            return;
                        }
                    } catch (e) {
                        $.messager.alert('tips', "RequiredValue1 or RequiredValue2 is char");
                        return;
                    }
                    break;
                case "A＜X≤B": if (!RequiredValue2_ && !RequiredValue1_) {

                    $.messager.alert('tips', "RequiredValue1 or RequiredValue2 is null");
                    return;

                }

                    try {
                        parseInt(RequiredValue1_);
                        parseInt(RequiredValue2_);
                        if (parseInt(RequiredValue1_) >= parseInt(RequiredValue2_)) {
                            $.messager.alert('tips', "RequiredValue1 >RequiredValue2");
                            return;
                        }
                    } catch (e) {
                        $.messager.alert('tips', "RequiredValue1 or RequiredValue2 is char");
                        return;
                    }
                    break;
                case "A≤X≤B": if (!RequiredValue2_ && !RequiredValue1_) {

                    $.messager.alert('tips', "RequiredValue1 or RequiredValue2 is null");
                    return;

                }

                    try {
                        parseInt(RequiredValue1_);
                        parseInt(RequiredValue2_);
                        if (parseInt(RequiredValue1_) >= parseInt(RequiredValue2_)) {
                            $.messager.alert('tips', "RequiredValue1 >RequiredValue2");
                            return;
                        }
                    } catch (e) {
                        $.messager.alert('tips', "RequiredValue1 or RequiredValue2 is char");
                        return;
                    }
                    break;
                default:

            }
        }
        //compare判断
        switch (CompareRangeFlag_) {
            case "A＜X":
                if (!CompareValue1_) {
                    $.messager.alert('tips', "CompareValue1 is null");
                    return;
                }
                try {
                    parseInt(CompareValue1_)
                } catch (e) {
                    $.messager.alert('tips', "CompareValue1 is char");
                    return;
                }
                CompareValue2_ = "";
                break;
            case "A≦X":
                if (!CompareValue1_) {
                    $.messager.alert('tips', "CompareValue1 is null");
                    return;
                }
                try {
                    parseInt(CompareValue1_)
                } catch (e) {
                    $.messager.alert('tips', "CompareValue1 is char");
                    return;
                }
                CompareValue2_ = "";
                break;
            case "X＞B":
                if (!CompareValue1_) {
                    $.messager.alert('tips', "CompareValue2 is null");
                    return;
                }
                try {
                    parseInt(CompareValue2_)
                } catch (e) {
                    $.messager.alert('tips', "CompareValue2 is char");
                    return;
                }
                CompareValue1_ = "";
                break;
            case "X≧B":
                if (!CompareValue2_) {
                    $.messager.alert('tips', "CompareValue2 is null");
                    return;
                }
                try {
                    parseInt(CompareValue2_)
                } catch (e) {
                    $.messager.alert('tips', "CompareValue2 is char");
                    return;
                }
                CompareValue1_ = "";
                break;

            case "A＜X＜B": if (!CompareValue2_ && !CompareValue1_) {

                $.messager.alert('tips', "CompareValue1 or CompareValue2 is null");
                return;

            }

                try {
                    parseInt(CompareValue1_);
                    parseInt(CompareValue2_);
                    if (parseInt(CompareValue1_) >= parseInt(CompareValue2_)) {
                        $.messager.alert('tips', "CompareValue1_ >= CompareValue2_");
                        return;
                    }
                } catch (e) {
                    $.messager.alert('tips', "CompareValue1 or RequiredValue2 is char");
                    return;
                }
                break;

            case "A≦X＜B": if (!CompareValue2_ && !CompareValue1_) {

                $.messager.alert('tips', "CompareValue1 or CompareValue2 is null");
                return;

            }

                try {
                    parseInt(CompareValue1_);
                    parseInt(CompareValue2_);
                    if (parseInt(CompareValue1_) >= parseInt(CompareValue2_)) {
                        $.messager.alert('tips', "CompareValue1 >CompareValue2");
                        return;
                    }
                } catch (e) {
                    $.messager.alert('tips', "CompareValue1 or CompareValue2 is char");
                    return;
                }
                break;
            case "A＜X≤B": if (!CompareValue2_ && !CompareValue1_) {

                $.messager.alert('tips', "CompareValue1 or CompareValue2 is null");
                return;

            }

                try {
                    parseInt(CompareValue1_);
                    parseInt(CompareValue2_);
                    if (parseInt(CompareValue1_) >= parseInt(CompareValue2_)) {
                        $.messager.alert('tips', "CompareValue1 >CompareValue2");
                        return;
                    }
                } catch (e) {
                    $.messager.alert('tips', "CompareValue1 or CompareValue2 is char");
                    return;
                }
                break;
            case "A≤X≤B": if (!CompareValue2_ && !CompareValue1_) {

                $.messager.alert('tips', "CompareValue1 or CompareValue2 is null");
                return;

            }

                try {
                    parseInt(CompareValue1_);
                    parseInt(CompareValue2_);
                    if (parseInt(CompareValue1_) >= parseInt(CompareValue2_)) {
                        $.messager.alert('tips', "CompareValue1 >CompareValue2");
                        return;
                    }
                } catch (e) {
                    $.messager.alert('tips', "CompareValue1 or CompareValue2 is char");
                    return;
                }
                break;
            default:

        }
        var selectRow_contation_datagrid = $('#contation_datagrid').datagrid('getSelected');//获取条件ID

        $('#add_Performance_detail').form('submit', {
            url: "/ScheduleManagement/AddProjParameterValue",//接收一般处理程序返回来的json数据     
            onSubmit: function (param) {
                param.GroupID = selectRow_contation_datagrid.ID
                param.TaskId = node_on1.id;
                param.ParameterId = selectRow_Performance_parameter_datagrid.id;
                param.ParameterType = selectRow_Performance_parameter_datagrid.ParameterType;
                param.Parameter = selectRow_Performance_parameter_datagrid.Parameter;
                param.ProjectName = node_on1.text;
                param.SpecificationsTOP = RequiredValue2_;
                param.SpecificationsLower = RequiredValue1_;
                param.RangeFlag = RangeFlag_;
                param.CompareRangeFlag = CompareRangeFlag_;
                param.CompareLower = CompareValue1_;
                param.CompareTOP = CompareValue2_;
            },
            success: function (data) {
                var result = $.parseJSON(data);
                if (result.Success == true) {
                    //刷新权限
                    Performance_Parameter_authorize();

                    $.messager.alert('tips', result.Message);
                    //  $('#add_Performance_parameter_add').dialog('close');

                }
                else {
                    $.messager.alert('tips', result.Message);
                }
            }
        });
        $("#add_Performance_detail").form("reset");
    }
    //测试实验室授权性能参数删除
    function remove_Performance_Parameter() {
        var node_on1 = $('#Laboratory_tree').tree('getSelected');
        var selectRow_Performance_parameter_authorize = $('#Performance_parameter_authorize').datagrid('getSelected');
        if (node_on1) {
            if (selectRow_Performance_parameter_authorize) {
                $.messager.confirm('confirm', 'confirm delete', function (r) {
                    if (r) {
                        $.ajax({
                            url: "/ScheduleManagement/DelProjParameterValue",
                            type: 'POST',
                            data: {
                                Parameter: selectRow_Performance_parameter_authorize.Parameter,
                                ID: selectRow_Performance_parameter_authorize.ID,
                                ProjectName: node_on1.text,
                                TaskId: node_on1.id
                            },
                            success: function (data) {
                                var result = $.parseJSON(data);
                                if (result.Success == true) {
                                    //刷新权限
                                    Performance_Parameter_authorize();

                                    $.messager.alert('tips', result.Message);
                                } else {

                                    $.messager.alert('tips', result.Message);
                                }
                            }
                        });
                    }
                });
            }
            else {
                $.messager.alert('Tips', 'Please select the row to be operated！');
            }
        }
    }
}
//***********************************************编辑性能参数dialog
function Performance_parameters_info_edit() {
    var node_on1 = $('#Laboratory_tree').tree('getSelected');
    var selectRow_Performance_parameter_datagrid = $('#Performance_parameters_info_datagrid').datagrid('getSelected');
    if (selectRow_Performance_parameter_datagrid) {
        line1_ = $('#Test_parameters_info_datagrid').datagrid("getRowIndex", selectRow_Performance_parameter_datagrid);
        //ParameterType combobox
        $('#ParameterUnit1').combobox({
            url: "/ScheduleManagement/LoadParameterUnitInfo ",//接收一般处理程序返回来的json数据      
            valueField: 'Value',
            textField: 'Text',
            required: true,
            //editable: false,
            //本地联系人数据模糊索引
            filter: function (q, row) {
                var opts = $(this).combobox('options');
                //  return row[opts.textField].indexOf(q) >= 0;
                return row[opts.textField].toLowerCase().indexOf(q.toLowerCase()) != -1;
            }
        });
        $('#add_Performance_parameter_add').dialog({
            title: 'edit parameter',
            width: 700,
            height: 500,
            buttons: [{
                text: 'save',
                iconCls: 'icon-ok',
                handler: function () {
                    //性能参数添加判断+提交 node_on1  selectRow_Performance_parameter_datagrid传参
                    Jsrule_Edit_Submit(node_on1, selectRow_Performance_parameter_datagrid);

                }
            }, {
                text: 'close',
                iconCls: 'icon-cancel',
                handler: function () {
                    $('#add_Performance_parameter_add').dialog('close');
                }
            }]
        });
        $("#add_Performance_detail").form("load", selectRow_Performance_parameter_datagrid);
        //数据范围回显
        $("#RangeFlag").combobox("setValue", selectRow_Performance_parameter_datagrid.RangeFlag);
        //复选框回显
        if (selectRow_Performance_parameter_datagrid.Flag == true) {
            $("#AutoFlag1").prop("checked", "checked");
            $("#AutoFlag2").prop("checked", false);
            $("#AutoFlag").val(true);
        } else {
            $("#AutoFlag2").prop("checked", "checked");
            $("#AutoFlag1").prop("checked", false);
            $("#AutoFlag").val(false);
        }
        //共有参数复选框回显
        if (selectRow_Performance_parameter_datagrid.Commonflag == true) {
            $("#Commonflag1").prop("checked", "checked");
            $("#Commonflag2").prop("checked", false);
            $("#Commonflag").val(true);
        } else {
            $("#Commonflag2").prop("checked", "checked");
            $("#Commonflag1").prop("checked", false);
            $("#Commonflag").val(false);
        }
        $("#RequiredValue1").textbox("setText", selectRow_Performance_parameter_datagrid.SpecificationsLower);
        $("#RequiredValue2").textbox("setText", selectRow_Performance_parameter_datagrid.SpecificationsTOP);
        //性能参数授权加载添加
    } else {
        $.messager.alert('tips', 'Please select the test project！');
    }

}
//性能参数 修改判断+提交 node_on1  selectRow_Performance_parameter_datagrid传参
function Jsrule_Edit_Submit(node_on1) {
    var selectRow_Performance_parameter_datagrid = $('#Performance_parameters_info_datagrid').datagrid('getSelected');
    //RangeFlag的值
    var RangeFlag_ = $("#RangeFlag").combobox("getValue");
    //判断AutoFlag是否自动
    var AutoFlag_ = $("#AutoFlag").val();
    //获取参照A 和B的值
    var RequiredValue1_ = $("#RequiredValue1").textbox("getText");
    var RequiredValue2_ = $("#RequiredValue2").textbox("getText");
    //CompareRangeFlag的值
    var CompareRangeFlag_ = $("#CompareRangeFlag").combobox("getValue");
    //获取cpmpare参照A 和B的值
    var CompareValue1_ = $("#min_limit").textbox("getText");
    var CompareValue2_ = $("#max_limit").textbox("getText");
    // 如果是 对下面的数据进行判断
    if (AutoFlag_ == "true") {
        switch (RangeFlag_) {

            case "A＜X":
                if (!RequiredValue1_) {
                    $.messager.alert('tips', "RequiredValue1 is null");
                    return;
                }
                try {
                    parseInt(RequiredValue1_)
                } catch (e) {
                    $.messager.alert('tips', "RequiredValue1 is char");
                    return;
                }
                RequiredValue2_ = "";
                break;
            case "A≦X":
                if (!RequiredValue1_) {
                    $.messager.alert('tips', "RequiredValue1 is null");
                    return;
                }
                try {
                    parseInt(RequiredValue1_)
                } catch (e) {
                    $.messager.alert('tips', "RequiredValue1 is char");
                    return;
                }
                RequiredValue2_ = "";
                break;
            case "X＜B":
                if (!RequiredValue2_) {
                    $.messager.alert('tips', "RequiredValue2 is null");
                    return;
                }
                try {
                    parseInt(RequiredValue2_)
                } catch (e) {
                    $.messager.alert('tips', "RequiredValue2 is char");
                    return;
                }
                RequiredValue1_ = "";
                break;
            case "X≦B":
                if (!RequiredValue2_) {
                    $.messager.alert('tips', "RequiredValue2 is null");
                    return;
                }
                try {
                    parseInt(RequiredValue2_)
                } catch (e) {
                    $.messager.alert('tips', "RequiredValue2 is char");
                    return;
                }
                RequiredValue1_ = "";
                break;

            case "A＜X＜B":
                if (!RequiredValue2_ && !RequiredValue1_) {

                    $.messager.alert('tips', "RequiredValue1 or RequiredValue2 is null");
                    return;

                }
                try {
                    parseInt(RequiredValue1_);
                    parseInt(RequiredValue2_);
                    if (parseInt(RequiredValue1_) >= parseInt(RequiredValue2_)) {
                        $.messager.alert('tips', "RequiredValue1 >= RequiredValue2");
                        return;
                    }
                } catch (e) {
                    $.messager.alert('tips', "RequiredValue1 or RequiredValue2 is char");
                    return;
                }
                break;
            case "A≦X＜B": if (!RequiredValue2_ && !RequiredValue1_) {

                $.messager.alert('tips', "RequiredValue1 or RequiredValue2 is null");
                return;

            }

                try {
                    parseInt(RequiredValue1_);
                    parseInt(RequiredValue2_);
                    if (parseInt(RequiredValue1_) >= parseInt(RequiredValue2_)) {
                        $.messager.alert('tips', "RequiredValue1 >RequiredValue2");
                        return;
                    }
                } catch (e) {
                    $.messager.alert('tips', "RequiredValue1 or RequiredValue2 is char");
                    return;
                }
                break;
            case "A＜X≤B": if (!RequiredValue2_ && !RequiredValue1_) {

                $.messager.alert('tips', "RequiredValue1 or RequiredValue2 is null");
                return;

            }

                try {
                    parseInt(RequiredValue1_);
                    parseInt(RequiredValue2_);
                    if (parseInt(RequiredValue1_) >= parseInt(RequiredValue2_)) {
                        $.messager.alert('tips', "RequiredValue1 >RequiredValue2");
                        return;
                    }
                } catch (e) {
                    $.messager.alert('tips', "RequiredValue1 or RequiredValue2 is char");
                    return;
                }
                break;
            default:

        }
    }
    //compare判断
    switch (CompareRangeFlag_) {
        case "A＜X":
            if (!CompareValue1_) {
                $.messager.alert('tips', "CompareValue1 is null");
                return;
            }
            try {
                parseInt(CompareValue1_)
            } catch (e) {
                $.messager.alert('tips', "CompareValue1 is char");
                return;
            }
            CompareValue2_ = "";
            break;
        case "A≦X":
            if (!CompareValue1_) {
                $.messager.alert('tips', "CompareValue1 is null");
                return;
            }
            try {
                parseInt(CompareValue1_)
            } catch (e) {
                $.messager.alert('tips', "CompareValue1 is char");
                return;
            }
            CompareValue2_ = "";
            break;
        case "X＜B":
            if (!CompareValue1_) {
                $.messager.alert('tips', "CompareValue2 is null");
                return;
            }
            try {
                parseInt(CompareValue2_)
            } catch (e) {
                $.messager.alert('tips', "CompareValue2 is char");
                return;
            }
            CompareValue1_ = "";
            break;
        case "X≦B":
            if (!CompareValue2_) {
                $.messager.alert('tips', "CompareValue2 is null");
                return;
            }
            try {
                parseInt(CompareValue2_)
            } catch (e) {
                $.messager.alert('tips', "CompareValue2 is char");
                return;
            }
            CompareValue1_ = "";
            break;

        case "A＜X＜B": if (!CompareValue2_ && !CompareValue1_) {

            $.messager.alert('tips', "CompareValue1 or CompareValue2 is null");
            return;

        }

            try {
                parseInt(CompareValue1_);
                parseInt(CompareValue2_);
                if (parseInt(CompareValue1_) >= parseInt(CompareValue2_)) {
                    $.messager.alert('tips', "CompareValue1_ >= CompareValue2_");
                    return;
                }
            } catch (e) {
                $.messager.alert('tips', "CompareValue1 or RequiredValue2 is char");
                return;
            }
            break;

        case "A≦X＜B": if (!CompareValue2_ && !CompareValue1_) {

            $.messager.alert('tips', "CompareValue1 or CompareValue2 is null");
            return;

        }

            try {
                parseInt(CompareValue1_);
                parseInt(CompareValue2_);
                if (parseInt(CompareValue1_) >= parseInt(CompareValue2_)) {
                    $.messager.alert('tips', "CompareValue1 >CompareValue2");
                    return;
                }
            } catch (e) {
                $.messager.alert('tips', "CompareValue1 or CompareValue2 is char");
                return;
            }
            break;
        case "A＜X≤B": if (!CompareValue2_ && !CompareValue1_) {

            $.messager.alert('tips', "CompareValue1 or CompareValue2 is null");
            return;

        }

            try {
                parseInt(CompareValue1_);
                parseInt(CompareValue2_);
                if (parseInt(CompareValue1_) >= parseInt(CompareValue2_)) {
                    $.messager.alert('tips', "CompareValue1 >CompareValue2");
                    return;
                }
            } catch (e) {
                $.messager.alert('tips', "CompareValue1 or CompareValue2 is char");
                return;
            }
            break;
        case "A≤X≤B": if (!CompareValue2_ && !CompareValue1_) {

            $.messager.alert('tips', "CompareValue1 or CompareValue2 is null");
            return;

        }

            try {
                parseInt(CompareValue1_);
                parseInt(CompareValue2_);
                if (parseInt(CompareValue1_) >= parseInt(CompareValue2_)) {
                    $.messager.alert('tips', "CompareValue1 >CompareValue2");
                    return;
                }
            } catch (e) {
                $.messager.alert('tips', "CompareValue1 or CompareValue2 is char");
                return;
            }
            break;
        default:

    }
    var selectRow_contation_datagrid = $('#contation_datagrid').datagrid('getSelected');//获取条件ID
    $('#add_Performance_detail').form('submit', {
        url: "/ScheduleManagement/EditProjParameterValue",//接收一般处理程序返回来的json数据     
        onSubmit: function (param) {
            param.GroupID = selectRow_contation_datagrid.ID;
            param.ID = selectRow_Performance_parameter_datagrid.ID;
            param.TaskId = node_on1.id;

            param.Parameter = selectRow_Performance_parameter_datagrid.Parameter;
            param.ProjectName = node_on1.text;
            param.SpecificationsTOP = RequiredValue2_;
            param.SpecificationsLower = RequiredValue1_;
            param.RangeFlag = RangeFlag_;
            param.CompareRangeFlag = CompareRangeFlag_;
            param.CompareLower = CompareValue1_;
            param.CompareTOP = CompareValue2_;
        },
        success: function (data) {
            var result = $.parseJSON(data);
            if (result.Success == true) {
                //刷新权限
                Performance_Parameter_authorize();

                $.messager.alert('tips', result.Message);
                // $('#add_Performance_parameter_add').dialog('close');
            }
            else {
                $.messager.alert('tips', result.Message);
            }
        }
    });
}
//***************************************************删除性能参数dialog
function Performance_parameters_info_delete() {
    var node_on1 = $('#Laboratory_tree').tree('getSelected');
    var selectRow_Performance_parameter_authorize = $('#Performance_parameters_info_datagrid').datagrid('getSelected');
    if (node_on1) {
        if (selectRow_Performance_parameter_authorize) {
            $.messager.confirm('confirm', 'confirm delete?', function (r) {
                if (r) {
                    $.ajax({
                        url: "/ScheduleManagement/DelProjParameterValue",
                        type: 'POST',
                        data: {
                            Parameter: selectRow_Performance_parameter_authorize.Parameter,
                            ID: selectRow_Performance_parameter_authorize.ID,
                            ProjectName: node_on1.text,
                            TaskId: node_on1.id
                        },
                        success: function (data) {
                            var result = $.parseJSON(data);
                            if (result.Success == true) {
                                //刷新权限
                                Performance_Parameter_authorize();
                                $.messager.alert('tips', result.Message);
                            } else {
                                $.messager.alert('tips', result.Message);
                            }
                        }
                    });
                }
            });
        }
        else {
            $.messager.alert('Tips', 'Please select the row to be operated！');
        }
    }
}
//*********************************************性能初始化回显
function Performance_Parameter_authorize() {
    var selectRow = $('#contation_datagrid').datagrid('getSelected');//获取条件ID
    //如果选中项目,
    var node_on = $('#Laboratory_tree').tree('getSelected');
    if (node_on) {
        if (selectRow) {
            $('#Performance_parameters_info_datagrid').datagrid({
                //fit: true,
                queryParams: {
                    GroupID: selectRow.ID,
                    TaskId: node_on.id,
                    flag: $("input[name='Commonflag_true']:checked").val()
                },
                url: '/ScheduleManagement/GetAddedParameterValueList',

            });
            $('#Performance_parameter_authorize').datagrid({
                fitColumns: true,
                //fit: true,
                queryParams: {
                    GroupID: selectRow.ID,
                    TaskId: node_on.id,
                    flag: $("input[name='Commonflag_true']:checked").val()
                },
                url: '/ScheduleManagement/GetAddedParameterValueList',
            });
            //定义pagination加载内容
            //var p1_employee_authorize1 = $('#Performance_parameter_authorize').datagrid('getPager');
            //(p1_employee_authorize1).pagination({
            //    layout: ['first', 'prev', 'last', 'next']
            //});

        } else {
            var json = {
                "rows": [],
                "total": 0,
                "success": true
            };
            $('#Performance_parameters_info_datagrid').datagrid("loadData", json);
        }

    } else {
        $.messager.alert('Tips', 'Please select the tree to be operated！');
    }
}
//性能参数搜索
function Performance_parameters_search() {
    var node = $('#Laboratory_tree').tree('getSelected');
    $('#Performance_parameter_Unauthorize').datagrid({
        queryParams: {
            search: $("#Parameter_datagrid_search1").combobox("getValue"),
            key: $("#Parameter_datagrid_search").textbox("getText"),
            ProjectId: node.ProjectId
        },
        url: '/ScheduleManagement/GetAuthPerforParamList'
    });
    //定义pagination加载内容
    var p3 = $('#Performance_parameter_Unauthorize').datagrid('getPager');
    (p3).pagination({
        layout: ['first', 'prev', 'last', 'next', 'efresh']

    });
}

//测试参数初始化化加载***********************************************************测试参数初始化化加载********************************************************
function Test_parameters_load() {

    ////测试条件
    $('#Test_parameters_info_datagrid').datagrid({
        nowrap: false,
        striped: true,
        //rownumbers: true,
        singleSelect: true,
        //autoRowHeight: true,
        //border: false,
        //fitColumns: true,
        fit: true,
        // title: 'Test Condition',
        pagination: true,
        pageSize: 40,
        queryParams: {
            // id:node.id
        },
        height: 390,
        //  onClickCell: onClickCell1,
        pageNumber: 1,
        type: 'POST',
        //  url: '/ScheduleManagement/GetAuthTestParamList',
        dataType: "json",
        columns: [[
            { field: 'Parameter', title: 'Parameter', width: 100 },
            { field: 'ParameterType_n', title: 'Type', width: 100 },
            { field: 'ParameterUnit_n', title: 'Unit', width: 100 },
            { field: 'ParameterValue', title: 'Value', width: 100 },
            {
                field: 'Commonflag', title: 'Common', width: 100, formatter: function (value, row, index) {
                    if (value == false) {
                        return 'No'
                    }
                    if (value == true) {
                        return 'Yes'
                    }
                }
            }
               //{
               //    field: 'remarks', title: 'remarks', width: 100, formatter: function (value, row, index) {
               //        if (value) {
               //            if (value.length > 30) {
               //                var result = value.replace(" ", "");//去空
               //                var value1 = result.substr(0, 30);
               //                return '<span  title=' + value + '>' + value1 + "......" + '</span>';
               //            } else {
               //                return '<span  title=' + value + '>' + value + '</span>';
               //            }
               //        }
               //    }
               //}
        ]],
        onLoadSuccess: function (data) {
            $('#Test_parameters_info_datagrid').datagrid('selectRow', line_);
        },
        onSelect: function () {
            //选中后回显
            var select = $('#Test_parameters_info_datagrid').datagrid('getSelected');

            $('#add_Test_detail').form('load', select);
        },
        toolbar: Test_parameters_info_toolbar
    });
    //定义pagination加载内容
    //var p = $('#Test_parameters_info_datagrid').datagrid('getPager');
    //(p).pagination({
    //    layout: ['first', 'prev', 'last', 'next']

    //});
    var json = {
        "rows": [],
        "total": 0,
        "success": true
    };
    $('#Test_parameters_info_datagrid').datagrid("loadData", json);
    //var node = $('#Laboratory_tree').tree('getSelected');
    //if (!node) {
    //    $.messager.alert('提示', '请选择测试项目');
    //    return;
    //}
    //添加
    $("#Test_parameters_info_add").unbind("click").bind("click", function () {
        var selectRow_contation_datagrid = $('#contation_datagrid').datagrid('getSelected');
        if (selectRow_contation_datagrid) {
            Test_parameters_info_add();
        } else {
            $.messager.alert('Tips', 'Please select the Condition to be operated！');
        }

    });
    //删除参数
    $("#Test_parameters_info_delete").unbind("click").bind("click", function () {
        var selectRow_contation_datagrid = $('#contation_datagrid').datagrid('getSelected');
        if (selectRow_contation_datagrid) {
            Test_parameters_info_delete(); //添加参数
        } else {
            $.messager.alert('Tips', 'Please select the Condition to be operated！');
        }



    });
    //编辑参数参数
    $("#Test_parameters_info_edit").unbind("click").bind("click", function () {
        var selectRow_contation_datagrid = $('#contation_datagrid').datagrid('getSelected');
        if (selectRow_contation_datagrid) {
            Test_parameters_info_edit(); //编辑参数
        } else {
            $.messager.alert('Tips', 'Please select the Condition to be operated！');
        }


    });

}
//添加性能参数dialog
function Test_parameters_info_add() {
    var node = $('#Laboratory_tree').tree('getSelected');
    if (node) {
        $('#parameter_dialog1').dialog({
            title: 'Add',
            width: 800,
            height: 550,
            buttons: [{
                text: 'close',
                iconCls: 'icon-cancel',
                handler: function () {
                    $('#parameter_dialog1').dialog('close');
                }
            }]
        });

        //性能参数授权加载添加
        Test_parameter_authorize_Add();
    } else {
        $.messager.alert('Tips', 'Please select the tree to be operated！');
    }

}
//测试参数授权加载添加
function Test_parameter_authorize_Add() {
    var node_on = $('#Laboratory_tree').tree('getSelected');
    //search
    $("#Test_datagrid_search1").combobox({
        data: [
                 { 'value': 'Parameter', 'text': 'Parameter' },
                 { 'value': 'ParameterType', 'text': 'ParameterType' },
                 { 'value': 'Attribut', 'text': 'Attribut' }
        ]
    });
    ////测试参数
    $('#Test_parameter_Unauthorize').datagrid({
        nowrap: false,
        striped: true,
        //rownumbers: true,
        singleSelect: true,
        //autoRowHeight: true,
        //border: false,
        fitColumns: true,
        //fit: true,
        title: 'parameter',
        url: '/ScheduleManagement/GetAuthTestParamList',
        pagination: true,
        pageSize: 40,
        pageNumber: 1,
        type: 'POST',
        //fit: true,
        queryParams: {
            ProjectId: node_on.ProjectId
        },
        dataType: "json",
        columns: [[
            { field: 'Parameter', title: 'Parameter', width: 100 },
           { field: 'ParameterType_n', title: 'Type', width: 150 },
           { field: 'Attribut_n', title: 'Attribut', width: 100 },
           {
               field: 'Remarks', title: 'Remarks', width: 150, formatter: function (value, row, index) {
                   if (value) {
                       if (value.length > 30) {
                           var result = value.replace(" ", "");//去空
                           var value1 = result.substr(0, 30);
                           return '<span  title=' + value + '>' + value1 + "......" + '</span>';
                       } else {
                           return '<span  title=' + value + '>' + value + '</span>';
                       }
                   }
               }
           }
        ]],
        onDblClickRow: function (index, row) {
            $('#add_Test_parameter').click();
        },
        onLoadSuccess: function (data) {
            $('#Test_parameter_Unauthorize').datagrid('selectRow', 0);

        },
        toolbar: Test_authorize_toobar
    });
    //定义pagination加载内容
    var p3_2 = $('#Test_parameter_Unauthorize').datagrid('getPager');
    (p3_2).pagination({
        layout: ['first', 'prev', 'last', 'next', 'efresh']
    });
    var selectRow_contation_datagrid = $('#contation_datagrid').datagrid('getSelected');
    //授权参数
    $('#Test_parameter_authorize').datagrid({
        nowrap: false,
        striped: true,
        //rownumbers: true,
        singleSelect: true,
        //autoRowHeight: true,
        //border: false,
        fitColumns: true,
        //fit: true,
        title: 'Authorization parameters',
        pagination: true,
        pageSize: 20,
        pageNumber: 1,
        queryParams: {
            GroupID: selectRow_contation_datagrid.ID,
            TaskId: node_on.id,
            flag: $("input[name='Commonflag_yes']:checked").val()
        },
        url: '/ScheduleManagement/GetAddedCondiitonValueList',
        columns: [[
           { field: 'Parameter', title: 'Parameter', width: 100 },
           { field: 'ParameterType_n', title: 'Type', width: 150 }

        ]],
        onDblClickRow: function (index, row) {
            $('#remove_Test_parameter').click();
        },
        onLoadSuccess: function (data) {
            $('#Test_parameter_authorize').datagrid('selectRow', 0);

        },
        type: 'POST',
        dataType: "json"
    });
    //定义pagination加载内容
    var p2_3 = $('#Test_parameter_authorize').datagrid('getPager');
    (p2_3).pagination({
        layout: ['first', 'prev', 'last', 'next', 'efresh']

    });
    //测试参数搜索
    $('#Test_datagrid_find').unbind("click").bind("click", function () {
        Test_parameters_search();
    });
    //添加授测试参数
    $('#add_Test_parameter').unbind("click").bind("click", function () {
        add_Test_parameter();
    })
    //删除授权测试参数
    $('#remove_Test_parameter').unbind("click").bind("click", function () {
        remove_Test_parameter();
    })
    //测试实验室授权测试参数添加
    function add_Test_parameter() {
        var node_on1 = $('#Laboratory_tree').tree('getSelected');
        var selectRow_Performance_parameter_datagrid = $('#Test_parameter_Unauthorize').datagrid('getSelected');
        var selectRow_contation_datagrid = $('#contation_datagrid').datagrid('getSelected');
        if (node_on1) {
            if (selectRow_Performance_parameter_datagrid) {
                $('#ParameterUnit').combobox({
                    url: "/ScheduleManagement/LoadParameterUnitInfo ",//接收一般处理程序返回来的json数据      
                    valueField: 'Value',
                    textField: 'Text',
                    required: true,
                    //editable: false,
                    //本地联系人数据模糊索引
                    filter: function (q, row) {
                        var opts = $(this).combobox('options');
                        //  return row[opts.textField].indexOf(q) >= 0;
                        return row[opts.textField].toLowerCase().indexOf(q.toLowerCase()) != -1;
                    }
                });
                $('#add_Test_parameter_add').dialog({
                    title: 'Add',
                    width: 500,
                    height: 400,
                    buttons: [{
                        text: 'ok',
                        iconCls: 'icon-ok',
                        handler: function () {
                            $('#add_Test_detail').form('submit', {
                                url: "/ScheduleManagement/AddProjCondiitonValue",//接收一般处理程序返回来的json数据     
                                onSubmit: function (param) {
                                    param.GroupID = selectRow_contation_datagrid.ID;
                                    param.TaskId = node_on1.id;
                                    param.ParameterId = selectRow_Performance_parameter_datagrid.id;
                                    param.ParameterType = selectRow_Performance_parameter_datagrid.ParameterType;
                                    param.Parameter = selectRow_Performance_parameter_datagrid.Parameter;
                                    param.ProjectName = node_on1.text;
                                },
                                success: function (data) {
                                    var result = $.parseJSON(data);
                                    if (result.Success == true) {
                                        //刷新权限
                                        Test_Parameter_authorize();

                                        $.messager.alert('tips', result.Message);
                                        $('#add_Test_parameter_add').dialog('close');
                                    }
                                    else {

                                        $.messager.alert('tips', result.Message);
                                    }
                                }
                            });
                        }
                    },
                        {
                            text: 'close',
                            iconCls: 'icon-cancel',
                            handler: function () {
                                $('#add_Test_parameter_add').dialog('close');
                            }
                        }]
                });
                $("#add_Test_detail").form("reset");
            }
            else {
                $.messager.alert('tips', 'Please select the authorization performance parameters to add！');
            }

        } else {
            $.messager.alert('Tips', 'Please select the tree to be operated！');
        }
    }
    //测试实验室授权测试参数删除
    function remove_Test_parameter() {
        var node_on1 = $('#Laboratory_tree').tree('getSelected');
        var selectRow_Test_parameter_authorize = $('#Test_parameter_authorize').datagrid('getSelected');
        if (node_on1) {
            if (selectRow_Test_parameter_authorize) {
                $.messager.confirm('confirm', 'confirm delete?', function (r) {
                    if (r) {
                        $.ajax({
                            url: "/ScheduleManagement/DelProjCondiitonValue",
                            type: 'POST',
                            data: {
                                TaskId: node_on1.id,
                                Parameter: selectRow_Test_parameter_authorize.Parameter,
                                ID: selectRow_Test_parameter_authorize.ID,
                                ProjectName: node_on1.text
                            },
                            success: function (data) {
                                var result = $.parseJSON(data);
                                if (result.Success == true) {
                                    //刷新权限
                                    Test_Parameter_authorize();

                                    $.messager.alert('tips', result.Message);
                                } else {

                                    $.messager.alert('tips', result.Message);
                                }
                            }
                        });
                    }
                });
            }
            else {
                $.messager.alert('Tips', 'Please select the tree to be operated！');
            }
        }
    }
}
//删除参数
function Test_parameters_info_delete() {
    var node_on1 = $('#Laboratory_tree').tree('getSelected');
    var selectRow_Test_parameter_authorize = $('#Test_parameters_info_datagrid').datagrid('getSelected');
    if (node_on1) {
        if (selectRow_Test_parameter_authorize) {
            $.messager.confirm('confirm', 'confirm delete?', function (r) {
                if (r) {
                    $.ajax({
                        url: "/ScheduleManagement/DelProjCondiitonValue",
                        type: 'POST',
                        data: {
                            TaskId: node_on1.id,
                            Parameter: selectRow_Test_parameter_authorize.Parameter,
                            ID: selectRow_Test_parameter_authorize.ID,
                            ProjectName: node_on1.text
                        },
                        success: function (data) {
                            var result = $.parseJSON(data);
                            if (result.Success == true) {
                                //刷新权限
                                Test_Parameter_authorize();

                                $.messager.alert('tips', result.Message);
                            } else {

                                $.messager.alert('tips', result.Message);
                            }
                        }
                    });
                }
            });
        }
        else {
            $.messager.alert('Tips', 'Please select the tree to be operated！');
        }
    }
}
//测试实验室授权测试参数编辑dialog0
function Test_parameters_info_edit() {
    var node_on1 = $('#Laboratory_tree').tree('getSelected');
    var selectRow_Performance_parameter_datagrid1 = $('#Test_parameters_info_datagrid').datagrid('getSelected');
    var selectRow_contation_datagrid = $('#contation_datagrid').datagrid('getSelected');
    if (selectRow_Performance_parameter_datagrid1) {
    
        //ParameterType combobox
        $('#ParameterUnit').combobox({
            url: "/ScheduleManagement/LoadParameterUnitInfo ",//接收一般处理程序返回来的json数据      
            valueField: 'Value',
            textField: 'Text',
            required: true,
            //editable: false,
            //本地联系人数据模糊索引
            filter: function (q, row) {
                var opts = $(this).combobox('options');
                //  return row[opts.textField].indexOf(q) >= 0;
                return row[opts.textField].toLowerCase().indexOf(q.toLowerCase()) != -1;
            }
        });
        $('#add_Test_parameter_add').dialog({
            title: 'edit parameter',
            width: 500,
            height: 400,
            buttons: [{
                text: 'save',
                iconCls: 'icon-ok',
                handler: function () {
                    var selectRow_Performance_parameter_datagrid = $('#Test_parameters_info_datagrid').datagrid('getSelected');
                    line_ = $('#Test_parameters_info_datagrid').datagrid("getRowIndex", selectRow_Performance_parameter_datagrid);
                    $('#add_Test_detail').form('submit', {
                        url: "/ScheduleManagement/EditProjCondiitonValue",//接收一般处理程序返回来的json数据     
                        onSubmit: function (param) {
                            // param.ID = selectRow_contation_datagrid.ID;
                            param.TaskId = node_on1.id;
                            param.ID = selectRow_Performance_parameter_datagrid.ID;
                            param.Parameter = selectRow_Performance_parameter_datagrid.Parameter;
                            param.ProjectName = node_on1.text;
                        },
                        success: function (data) {
                            var result = $.parseJSON(data);
                            if (result.Success == true) {
                                //刷新权限
                                Test_Parameter_authorize();
                                $.messager.alert('tips', result.Message);
                                // $('#add_Test_parameter_add').dialog('close');
                            }
                            else {
                                $.messager.alert('tips', result.Message);
                            }
                        }
                    });
                }
            }, {
                text: 'close',
                iconCls: 'icon-cancel',
                handler: function () {
                    $('#add_Test_parameter_add').dialog('close');
                }
            }]
        });
        $("#add_Test_detail").form("load", selectRow_Performance_parameter_datagrid);
        //数据范围回显
        $("#RangeFlag").combobox("setValue", selectRow_Performance_parameter_datagrid.RangeFlag);
        //复选框回显
        if (selectRow_Performance_parameter_datagrid.Commonflag == true) {
            $("#Commonflag1_").prop("checked", "checked");
            $("#Commonflag2_").prop("checked", false);
            $("#Commonflag_").val("true");
        } else {
            $("#Commonflag2_").prop("checked", "checked");
            $("#Commonflag1_").prop("checked", false);
            $("#Commonflag_").val("false");
        }
        //性能参数授权加载添加
    } else {
        $.messager.alert('Tips', 'Please select the tree to be operated！');
    }

}
//测试初始化回显
function Test_Parameter_authorize() {
    var selectRow = $('#contation_datagrid').datagrid('getSelected');//获取条件ID
    var sele = $('#contation_datagrid').datagrid('getSelected');

    //如果选中项目,项目人员回显
    var node_on = $('#Laboratory_tree').tree('getSelected');
    if (node_on) {
        if (sele) {
            $('#Test_parameters_info_datagrid').datagrid({
                //fit: true,
                queryParams: {
                    GroupID: selectRow.ID,
                    TaskId: node_on.id,
                    flag: $("input[name='Commonflag_yes']:checked").val()
                },
                url: '/ScheduleManagement/GetAddedCondiitonValueList',

            });

            //定义pagination加载内容
            //var p1_employee_authorize = $('#Test_parameters_info_datagrid').datagrid('getPager');
            //(p1_employee_authorize).pagination({
            //    layout: ['first', 'prev', 'last', 'next']
            //});
            $('#Test_parameter_authorize').datagrid({
                //fit: true,
                queryParams: {
                    GroupID: selectRow.ID,
                    TaskId: node_on.id,
                    flag: $("input[name='Commonflag_yes']:checked").val()
                },
                url: '/ScheduleManagement/GetAddedCondiitonValueList',
            });
            //定义pagination加载内容
            //var p1_employee_authorize1 = $('#Test_parameter_authorize').datagrid('getPager');
            //(p1_employee_authorize1).pagination({
            //    layout: ['first', 'prev', 'last', 'next']
            //});
        } else {
            var json = {
                "rows": [],
                "total": 0,
                "success": true
            };
            $('#Test_parameters_info_datagrid').datagrid("loadData", json);
        }
    }
};
/*
*functionName:reflesh_test
*function:授权性能刷新
*Param: 
*author:程媛
*date:2018-05-12
*/
function reflesh_test() {
    var selectRow = $('#contation_datagrid').datagrid('getSelected');//获取条件ID
    var node_on = $('#Laboratory_tree').tree('getSelected');
    if (selectRow) {
        //授权参数
        $('#Test_parameters_info_datagrid').datagrid({
            queryParams: {
                GroupID: selectRow.ID,
                TaskId: node_on.id,
                flag: $("input[name='Commonflag_yes']:checked").val()
            },
            url: '/ScheduleManagement/GetAddedCondiitonValueList',
        });
    } else {
        $.messager.alert('Tips', 'Please select the row to be operated！');
    }
};
/*
*functionName:reflesh_Parameter
*function:授权性能刷新
*Param: 
*author:程媛
*date:2018-05-12
*/
function reflesh_Parameter() {
    var selectRow = $('#contation_datagrid').datagrid('getSelected');//获取条件ID
    var node_on = $('#Laboratory_tree').tree('getSelected');
    if (selectRow) {
        $('#Performance_parameters_info_datagrid').datagrid({
            // fitColumns: true,
            //fit: true,
            queryParams: {
                GroupID: selectRow.ID,
                TaskId: node_on.id,
                flag: $("input[name='Commonflag_true']:checked").val()
            },
            url: '/ScheduleManagement/GetAddedParameterValueList',
        });
    } else {
        $.messager.alert('Tips', 'Please select the row to be operated！');
    }
}
//测试参数搜索
function Test_parameters_search() {
    var node_on = $('#Laboratory_tree').tree('getSelected');
    $('#Test_parameter_Unauthorize').datagrid({
        queryParams: {
            search: $("#Test_datagrid_search1").combobox("getValue"),
            key: $("#Test_datagrid_search").textbox("getText"),
            ProjectId: node_on.ProjectId
        },
        url: '/ScheduleManagement/GetAuthTestParamList'
    });
    ////定义pagination加载内容
    //var p3 = $('#Test_parameter_Unauthorize').datagrid('getPager');
    //(p3).pagination({
    //    layout: ['first', 'prev', 'last', 'next']

    //});
}
//员工初始化化加载******************************************************************员工初始化化加载************************************************************
function employee_load() {
    //人员
    $('#employee_list').datagrid({
        title: "Employee",
        nowrap: false,
        striped: true,
        //rownumbers: true,
        singleSelect: true,
        //autoRowHeight: true,
        //border: false,
        fitColumns: true,
        //fit: true,
        pagination: true,
        pageSize: 40,
        pageNumber: 1,
        type: 'POST',
        dataType: "json",
        height: 300,
        columns: [[
         { field: 'UserCount', title: 'UserCount', width: 100 },
           { field: 'UserName', title: 'UserName', width: 100, hidden: true },
            { field: 'TaskNum', title: 'TaskNum', hidden: true },
            { field: 'JobNum', title: 'JobNum', width: 100 }
        ]],
        onLoadSuccess: function (data) {
            $('#employee_list').datagrid('selectRow', 0);

        },
        toolbar: person_toolbar
    });
    //定义pagination加载内容
    //var p = $('#employee_list').datagrid('getPager');
    //(p).pagination({
    //    layout: ['first', 'prev', 'last', 'next']

    //});
    var json = {
        "rows": [],
        "total": 0,
        "success": true
    };
    $('#employee_list').datagrid("loadData", json);
    //添加
    $("#person_add").unbind("click").bind("click", function () {
        add_verification_person();
    });
    //查看详情 已授权
    $("#person_read").unbind("click").bind("click", function () {
        employee_read();
    });
    //删除人员 已授权
    $("#person_delete").unbind("click").bind("click", function () {
        person_delete();
    });
    function person_delete() {
        var node_on1 = $('#Laboratory_tree').tree('getSelected');
        var selectRow_employee_authorize = $('#employee_list').datagrid('getSelected');
        if (node_on1) {
            if (selectRow_employee_authorize) {

                $.messager.confirm('confirm', 'confirm delete', function (r) {
                    if (r) {
                        $.ajax({
                            url: "/ScheduleManagement/DelProjPerson",
                            type: 'POST',
                            data: {
                                UserName: selectRow_employee_authorize.UserName,
                                id: selectRow_employee_authorize.id,
                                TaskId: node_on1.id,
                                ProjectName: node_on1.text
                            },
                            success: function (data) {
                                var result = $.parseJSON(data);
                                if (result.Success == true) {
                                    //刷新权限
                                    employee_authorize();
                                } else {

                                    $.messager.alert('tips', result.Message);
                                }
                            }
                        });
                    }
                });
            }
            else {
                $.messager.alert('Tips', 'Please select the row to be operated！');
            }
        } else {
            $.messager.alert('Tips', 'Please select the tree to be operated！');
        }
    }
}
//授权人员dialog
function add_verification_person() {
    var node_on1 = $('#Laboratory_tree').tree('getSelected');
    if (node_on1) {
        $('#person_dialog').dialog({
            title: 'Add',
            width: 800,
            height: 500,
            buttons: [{
                text: 'Close',
                iconCls: 'icon-cancel',
                handler: function () {
                    $('#person_dialog').dialog('close');
                }
            }]
        });
        //授权人员dialog
        employee_authorize_Add();
    } else {
        $.messager.alert('Tips', 'Please select the tree to be operated！');
    }

}
//人员初始化回显
function employee_authorize() {
    //如果选中项目,
    var node_on = $('#Laboratory_tree').tree('getSelected');
    if (node_on) {
        $('#employee_list').datagrid({
            fitColumns: true,
            //fit: true,
            queryParams: {
                TaskNum: node_on.id
            },
            url: '/ScheduleManagement/GetAddedPersonList',
        });
        ////定义pagination加载内容
        //var p1_employee_authorize = $('#employee_list').datagrid('getPager');
        //(p1_employee_authorize).pagination({
        //    layout: ['first', 'prev', 'last', 'next']
        //});
    } else {
        $.messager.alert('Tips', 'Please select the tree to be operated！');
    }
}
//人员初始化添加的时候的回显
function employee_authorize1() {
    //如果选中项目,
    var node_on = $('#Laboratory_tree').tree('getSelected');
    if (node_on) {
        $('#employee_authorize').datagrid({
            fitColumns: true,
            //fit: true,
            queryParams: {
                TaskNum: node_on.id
            },
            url: '/ScheduleManagement/GetAddedPersonList',
        });
        //定义pagination加载内容
        var p1_employee_authorize1 = $('#employee_authorize').datagrid('getPager');
        (p1_employee_authorize1).pagination({
            layout: ['first', 'prev', 'last', 'next']
        });
    } else {
        $.messager.alert('Tips', 'Please select the tree to be operated！');
    }
}
//授权人员权限添加
function employee_authorize_Add() {
    //search
    $("#person_search1").combobox({
        data: [
                 { 'value': 'UserCount', 'text': 'UserCount' },
                 { 'value': 'UserName', 'text': 'UserName' }
        ]
    });
    var node_on = $('#Laboratory_tree').tree('getSelected');
    if (!node_on) {
        $.messager.alert("提示", "请选中测试项目");
        return;
    }

    ///////////*******************实验室员工加载处理end //授权检定员 //授权检定员 //授权检定员 //授权检定员 //授权检定员 //授权检定员 //授权检定员 //授权检定员 //授权检定员 //授权检定员 //授权检定员 //授权检定员//////////////////////////
    //实验室员工
    $('#employee_datagrid').datagrid({
        nowrap: false,
        striped: true,
        //rownumbers: true,
        singleSelect: true,
        //autoRowHeight: true,
        //border: false,
        fitColumns: true,
        //fit: true,
        title: '实验室员工',
        pagination: true,
        pageSize: 40,
        pageNumber: 1,
        type: 'POST',
        dataType: "json",
        queryParams: {
            ProjectId: node_on.ProjectId
        },
        url: '/ScheduleManagement/GetAuthPersonnelList',
        columns: [[
           { field: 'UserCount', title: 'UserCount', width: 100 },
           { field: 'UserName', title: 'UserName', width: 100 }
        ]],
        onLoadSuccess: function (data) {
            $('#employee_datagrid').datagrid('selectRow', 0);

        },
        onDblClickRow: function (index, row) {
            $('#add_employee_person').click();
        },
        toolbar: person_authorize_toolbar
    });
    //定义pagination加载内容
    var p = $('#employee_datagrid').datagrid('getPager');
    (p).pagination({
        layout: ['first', 'prev', 'last', 'next']

    });
    //授权检定员初始化
    $('#employee_authorize').datagrid({
        nowrap: false,
        striped: true,
        //rownumbers: true,
        singleSelect: true,
        //autoRowHeight: true,
        //border: false,
        fitColumns: true,
        //fit: true,
        title: '授权试验员',
        pagination: true,
        pageSize: 20,
        pageNumber: 1,
        queryParams: {
            TaskNum: node_on.id
        },
        url: '/ScheduleManagement/GetAddedPersonList',
        columns: [[
            { field: 'UserCount', title: 'UserCount', width: 100 },
           { field: 'JobNum', title: 'JobNum', width: 100 }

        ]],
        onLoadSuccess: function (data) {
            $('#employee_authorize').datagrid('selectRow', 0);

        },
        onDblClickRow: function (index, row) {
            $('#remove_employee_person').click();
        },
        type: 'POST',
        dataType: "json"
    });

    //定义pagination加载内容
    var p1_employee_authorize = $('#employee_authorize').datagrid('getPager');
    (p1_employee_authorize).pagination({
        layout: ['first', 'prev', 'last', 'next']

    });
    //人员搜索
    $('#person_find').unbind("click").bind("click", function () {
        employee_search();
    });
    //添加授权检定员
    $('#add_employee_person').unbind("click").bind("click", function () {
        add_employee();
    })
    //删除授权检定员
    $('#remove_employee_person').unbind("click").bind("click", function () {
        remove_employee();
    })
    //测试实验室授权检定员添加
    function add_employee() {
        var node_on1 = $('#Laboratory_tree').tree('getSelected');
        var selectRow_A = $('#employee_datagrid').datagrid('getSelected');
        if (node_on1) {
            if (selectRow_A) {
                $.ajax({
                    url: "/ScheduleManagement/AddProjPerson",
                    type: 'POST',
                    data: {
                        TaskNum: node_on1.id,
                        UserId: selectRow_A.UserId,
                        UserName: selectRow_A.UserName,
                        UserCount: selectRow_A.UserCount,
                        JobNum: selectRow_A.JobNum,
                        ProjectName: node_on1.text
                    },
                    success: function (data) {
                        var result = $.parseJSON(data);
                        if (result.Success == true) {
                            //刷新权限
                            employee_authorize();
                            employee_authorize1();
                        }
                        else {
                            $.messager.alert('Tips', result.Message);
                        }
                    }
                });
            }
            else {
                $.messager.alert('Tips', 'Please select the row to be operated！');
            }
        } else {
            $.messager.alert('Tips', 'Please select the tree to be operated！');
        }
    }
    //测试实验室授权检定员删除
    function remove_employee() {
        var node_on1 = $('#Laboratory_tree').tree('getSelected');
        var selectRow_employee_authorize = $('#employee_authorize').datagrid('getSelected');
        if (node_on1) {
            if (selectRow_employee_authorize) {
                $.messager.confirm('confirm', 'confirm delete?', function (r) {
                    if (r) {
                        $.ajax({
                            url: "/ScheduleManagement/DelProjPerson",
                            type: 'POST',
                            data: {
                                UserName: selectRow_employee_authorize.UserName,
                                id: selectRow_employee_authorize.id,
                                TaskId: node_on1.id,
                                ProjectName: node_on1.text
                            },
                            success: function (data) {
                                var result = $.parseJSON(data);
                                if (result.Success == true) {
                                    //刷新权限
                                    employee_authorize();
                                    employee_authorize1();
                                } else {

                                    $.messager.alert('tips', result.Message);
                                }
                            }
                        });
                    }
                });
            }
            else {
                $.messager.alert('Tips', 'Please select the row to be operated！');
            }
        } else {
            $.messager.alert('Tips', 'Please select the tree to be operated！');
        }

    }

}
//人员搜索
function employee_search() {
    var node_on1 = $('#Laboratory_tree').tree('getSelected');
    $('#employee_datagrid').datagrid({
        queryParams: {
            search: $("#person_search1").combobox("getValue"),
            key: $("#person_search").textbox("getText"),
            ProjectId: node_on1.ProjectId
        },
        url: '/ScheduleManagement/GetAuthPersonnelList'
    });
    //定义pagination加载内容
    var p3 = $('#employee_datagrid').datagrid('getPager');
    (p3).pagination({
        layout: ['first', 'prev', 'last', 'next']

    });
}
//人员详细信息
function employee_read() {
    var selectRow = $('#employee_list').datagrid('getSelected');
    var rowss = $('#employee_list').datagrid('getSelections');
    if (selectRow) {
        $('#employee_detail_dialog').dialog({
            title: '夹具详细信息',
            width: 500,
            height: 300,
            top: 200,
            buttons: [{
                text: 'close',
                iconCls: 'icon-cancel',
                handler: function () {
                    $('#employee_detail_dialog').dialog('close');
                }
            }]
        });
        $('#employee_datagrid_detail').form("load", rowss[0]);
    } else {
        $.messager.alert('Tips', 'Please select the row to be operated！');
    }
}
//易耗品  初始化化加载***************************************************************易耗品初始化化加载*************************************************
function Consumables_load() {
    //易耗品
    $('#Consumables_list').datagrid({
        title: "Consumables",
        nowrap: false,
        striped: true,
        //rownumbers: true,
        singleSelect: true,
        //autoRowHeight: true,
        //border: false,
        fitColumns: true,
        //fit: true,
        pagination: true,
        pageSize: 40,
        pageNumber: 1,
        type: 'POST',
        dataType: "json",
        height: 300,
        columns: [[
            { field: 'PONum', title: 'PONum', sortable: 'true', width: 100 },//
            { field: 'ConsumablesName', title: 'ConsumablesName', width: 150 },//
            { field: 'TaskNum', title: 'TaskNum', hidden: true },
            { field: 'ConsumablesType', title: 'ConsumablesType', hidden: true },
            { field: 'Qty', title: 'Qty', hidden: true }

        ]],
        onLoadSuccess: function (data) {
            $('#Consumables_list').datagrid('selectRow', 0);

        },
        toolbar: Consumables_toolbar
    });
    //定义pagination加载内容
    //var p = $('#Consumables_list').datagrid('getPager');
    //(p).pagination({
    //    layout: ['first', 'prev', 'last', 'next']

    //});
    var json = {
        "rows": [],
        "total": 0,
        "success": true
    };
    $('#Consumables_list').datagrid("loadData", json);
    //添加
    $("#Consumables_add").unbind("click").bind("click", function () {
        Consumables_dialog_add();
    });
    //查看详情 已授权
    $("#Consumables_read").unbind("click").bind("click", function () {
        Consumables_read();
    });
    //删除人员 已授权
    $("#Consumables_delete").unbind("click").bind("click", function () {
        Consumables_delete();
    });
    function Consumables_delete() {
        var node_on1 = $('#Laboratory_tree').tree('getSelected');
        var selectRow_Consumables_authorize = $('#Consumables_list').datagrid('getSelected');
        if (selectRow_Consumables_authorize) {
            $.messager.confirm('confirm', 'confirm delete?', function (r) {
                if (r) {
                    $.ajax({
                        url: "/ScheduleManagement/DelProjConsum",
                        type: 'POST',
                        data: {
                            id: selectRow_Consumables_authorize.id,
                            ConsumablesName: selectRow_Consumables_authorize.ConsumablesName,
                            PONum: selectRow_Consumables_authorize.PONum,
                            ProjectName: node_on1.text,
                            TaskId: node_on1.id
                        },
                        success: function (data) {
                            var result = $.parseJSON(data);
                            if (result.Success == true) {
                                //刷新权限
                                Consumables_authorize();
                            } else {
                                $.messager.alert('Tips', result.Message);
                            }
                        }
                    });
                }
            });
        }
        else {
            $.messager.alert('Tips', 'Please select the row to be operated！');
        }
    }
}
//易耗品初始化回显
function Consumables_authorize() {
    //如果选中项目,
    var node_on = $('#Laboratory_tree').tree('getSelected');
    if (node_on) {
        $('#Consumables_list').datagrid({
            fitColumns: true,
            //fit: true,
            queryParams: {
                TaskNum: node_on.id
            },
            url: '/ScheduleManagement/GetAddedConsumList',
        });
        ////定义pagination加载内容
        //var p1_employee_authorize = $('#Consumables_list').datagrid('getPager');
        //(p1_employee_authorize).pagination({
        //    layout: ['first', 'prev', 'last', 'next']
        //});
    } else {
        $.messager.alert('Tips', 'Please select the tree to be operated！');
    }
}
//易耗品初始化添加的时候的回显回显
function Consumables_authorize1() {
    //如果选中项目,
    var node_on = $('#Laboratory_tree').tree('getSelected');
    if (node_on) {
        $('#Consumables_authorize').datagrid({
            fitColumns: true,
            //fit: true,
            queryParams: {
                TaskNum: node_on.id
            },
            url: '/ScheduleManagement/GetAddedConsumList',
        });
        //定义pagination加载内容
        var p1_employee_authorize1 = $('#Consumables_authorize').datagrid('getPager');
        (p1_employee_authorize1).pagination({
            layout: ['first', 'prev', 'last', 'next']
        });
    } else {
        $.messager.alert('Tips', 'Please select the tree to be operated！');
    }
}
//易耗品添加dialog
function Consumables_dialog_add() {
    //如果选中项目,
    var node_on = $('#Laboratory_tree').tree('getSelected');
    if (node_on) {
        $('#Consumables_dialog').dialog({
            title: 'Add',
            width: 850,
            height: 500,
            buttons: [{
                text: 'Close',
                iconCls: 'icon-cancel',
                handler: function () {
                    $('#Consumables_dialog').dialog('close');
                }
            }]

        });
        //易耗品授权加载添加
        Consumables_authorize_Add();
    } else {
        $.messager.alert('Tips', 'Please select the tree to be operated！');
    }
}
//易耗品授权加载添加
function Consumables_authorize_Add() {
    var node_on = $('#Laboratory_tree').tree('getSelected');
    //search
    $("#Consumables_search1").combobox({
        data: [
                 { 'value': 'PONum', 'text': 'PONum' },
                 { 'value': 'ConsumablesName', 'text': 'ConsumablesName' }
        ]
    });
    $('#Consumables_datagrid').datagrid({
        border: true,
        nowrap: false,
        striped: true,
        ///rownumbers: true,
        title: "Consumables",
        //singleSelect: true,
        //autoRowHeight: true,
        ctrlSelect: true,
        // fit: true,
        fitColumns: true,
        pagination: true,
        pageSize: 15,
        pageList: [15, 30, 45, 60],
        pageNumber: 1,
        type: 'POST',
        queryParams: {
            ProjectId: node_on.ProjectId
        },
        url: '/ScheduleManagement/GetAuthConsumList',
        dataType: "json",
        columns: [[
                 { field: 'PONum', title: 'PONum.', sortable: 'true', width: 100 },//
        { field: 'ConsumablesName', title: 'ConsumablesName', width: 150 },//\

        ]],
        onDblClickRow: function (index, row) {
            $('#add_Consumables_datagrid').click();
        },
        onLoadSuccess: function (data) {
            $('#Consumables_datagrid').datagrid('selectRow', 0);

        },
        toolbar: Consumables_authorize_toolbar
    });
    //定义pagination加载内容
    var p_111 = $('#Consumables_datagrid').datagrid('getPager');
    (p_111).pagination({
        layout: ['first', 'prev', 'last', 'next']

    });
    //
    $('#Consumables_authorize').datagrid({
        border: true,
        nowrap: false,
        striped: true,
        // rownumbers: true,
        title: "Authorize Consumables",
        //singleSelect: true,
        //autoRowHeight: true,
        ctrlSelect: true,
        // fit: true,
        fitColumns: true,
        pagination: true,
        pageSize: 15,
        pageList: [15, 30, 45, 60],
        pageNumber: 1,
        type: 'POST',
        dataType: "json",
        queryParams: {
            TaskNum: node_on.id
        },
        url: '/ScheduleManagement/GetAddedConsumList',
        columns: [[
              { field: 'PONum', title: 'PONum', sortable: 'true', width: 100 },//
              { field: 'ConsumablesName', title: 'ConsumablesName', width: 150 },//


        ]],
        onLoadSuccess: function (data) {
            $('#Consumables_authorize').datagrid('selectRow', 0);

        },
        onDblClickRow: function (index, row) {
            $('#remove_Consumables_datagrid').click();
        }
        // toolbar: maintenance_plan_toolbar
    });
    //定义pagination加载内容
    var p_211 = $('#Consumables_authorize').datagrid('getPager');
    (p_211).pagination({
        layout: ['first', 'prev', 'last', 'next']

    });
    //易耗品搜索
    $('#Consumable_find').unbind("click").bind("click", function () {
        Consumables_search();
    });
    //添加已授权易耗品
    $('#add_Consumables_datagrid').unbind("click").bind("click", function () {
        add_Consumables_datagrid();
    })
    //删除已授权易耗品
    $('#remove_Consumables_datagrid').unbind("click").bind("click", function () {
        remove_Consumables_datagrid();
    })
    //添加已授权易耗品
    function add_Consumables_datagrid() {
        var node_on1 = $('#Laboratory_tree').tree('getSelected');
        var selectRow_Consumables_datagrid = $('#Consumables_datagrid').datagrid('getSelected');
        if (node_on1) {
            if (selectRow_Consumables_datagrid) {
                $.ajax({
                    url: "/ScheduleManagement/AddProjConsum",
                    type: 'POST',
                    data: {
                        TaskNum: node_on1.id,
                        PONum: selectRow_Consumables_datagrid.PONum,
                        ConsumablesName: selectRow_Consumables_datagrid.ConsumablesName,
                        ConsumablesType: selectRow_Consumables_datagrid.ConsumablesType,
                        Qty: selectRow_Consumables_datagrid.Qty,
                        ProjectName: node_on1.text
                    },
                    success: function (data) {
                        var result = $.parseJSON(data);
                        if (result.Success == true) {
                            //刷新权限
                            Consumables_authorize();
                            Consumables_authorize1();
                        }
                        else {

                            $.messager.alert('tips', result.Message);
                        }
                    }
                });
            }
            else {
                $.messager.alert('Tips', 'Please select the row to be operated！');
            }
        } else {
            $.messager.alert('Tips', 'Please select the tree to be operated！');
        }
    }
    //删除已授权易耗品
    function remove_Consumables_datagrid() {
        var node_on1 = $('#Laboratory_tree').tree('getSelected');
        var selectRow_Consumables_authorize = $('#Consumables_authorize').datagrid('getSelected');
        if (selectRow_Consumables_authorize) {
            $.messager.confirm('confirm', 'confirm delete?', function (r) {
                if (r) {
                    $.ajax({
                        url: "/ScheduleManagement/DelProjConsum",
                        type: 'POST',
                        data: {
                            id: selectRow_Consumables_authorize.id,
                            ConsumablesName: selectRow_Consumables_authorize.ConsumablesName,
                            PONum: selectRow_Consumables_authorize.PONum,
                            ProjectName: node_on1.text,
                            TaskId: node_on1.id
                        },
                        success: function (data) {
                            var result = $.parseJSON(data);
                            if (result.Success == true) {
                                //刷新权限
                                Consumables_authorize();
                                Consumables_authorize1();
                            } else {

                                $.messager.alert('tips', result.Message);
                            }
                        }
                    });
                }
            });
        }
        else {
            $.messager.alert('Tips', 'Please select the row to be operated！');
        }
    }
}
//易耗品搜索
function Consumables_search() {
    var node_on = $('#Laboratory_tree').tree('getSelected');
    $('#Consumables_datagrid').datagrid({
        queryParams: {
            ProjectId: node_on.ProjectId,
            search: $("#Consumables_search1").combobox("getValue"),
            key: $("#Consumables_search").textbox("getText")
        },
        url: '/ScheduleManagement/GetAuthConsumList'
    });
    var p_111 = $('#Consumables_datagrid').datagrid('getPager');
    (p_111).pagination({
        layout: ['first', 'prev', 'last', 'next']

    });
}
//易耗品详细信息
function Consumables_read() {
    var selectRow = $('#Consumables_list').datagrid('getSelected');
    var rowss = $('#Consumables_list').datagrid('getSelections');
    if (selectRow) {
        $('#Consumables_detail_dialog').dialog({
            title: '夹具详细信息',
            width: 500,
            height: 300,
            top: 200,
            buttons: [{
                text: 'close',
                iconCls: 'icon-cancel',
                handler: function () {
                    $('#Consumables_detail_dialog').dialog('close');
                }
            }]
        });
        $('#Consumables_datagrid_detail').form("load", rowss[0]);
    } else {
        $.messager.alert('Tips', 'Please select the row to be operated！');
    }
}
//夹具  初始化化加载***************************************************************夹具初始化化加载*************************************************
function Fixture_load() {
    //夹具
    $('#Fixture_list').datagrid({
        title: "Fixture",
        nowrap: false,
        striped: true,
        //rownumbers: true,
        singleSelect: true,
        //autoRowHeight: true,
        //border: false,
        fitColumns: true,
        //fit: true,
        pagination: true,
        pagePosition: 'bottom',
        pageSize: 40,
        pageNumber: 1,
        type: 'POST',
        dataType: "json",
        height: 300,

        columns: [[
                  { field: 'ScheduleType', title: 'ScheduleType', sortable: 'true', width: 100 },//
                 //{ field: 'FixtureName', title: 'FixtureName', width: 150 },//
                 //{ field: 'TaskNum', title: 'TaskNum', hidden: true },//
                 //{ field: 'FixtureType', title: 'FixtureType', hidden: true }

        ]],
        onLoadSuccess: function (data) {
            $('#Fixture_list').datagrid('selectRow', 0);

        },
        toolbar: clamp_toolbar
    });
    //定义pagination加载内容
    //var p = $('#Fixture_list').datagrid('getPager');
    //(p).pagination({
    //    layout: ['first', 'prev', 'last', 'next']

    //});
    var json = {
        "rows": [],
        "total": 0,
        "success": true
    };
    $('#Fixture_list').datagrid("loadData", json);
    //添加 已授权
    $("#clamp_add").unbind("click").bind("click", function () {
        clamp_add();
    });
    //查看详情 已授权
    $("#clamp_read").unbind("click").bind("click", function () {
        Fixture_read();
    });
    //删除 已授权
    $("#clamp_delete").unbind("click").bind("click", function () {
        clamp_delete();
    });
    function clamp_delete() {
        var node_on1 = $('#Laboratory_tree').tree('getSelected');
        var selectRow_Fixture_authorize = $('#Fixture_list').datagrid('getSelected');
        if (selectRow_Fixture_authorize) {
            $.messager.confirm('Confirm', 'confirm delete?', function (r) {
                if (r) {
                    $.ajax({
                        url: "/ScheduleManagement/DelTaskAuth",
                        type: 'POST',
                        data: {
                            Assortment: 2,
                            //TaskId: node_on1.id,
                            ScheduleType: selectRow_Fixture_authorize.ScheduleType,//添加的类型的名称
                            ID: selectRow_Fixture_authorize.ID,//已授权表的id
                            ProjectName: node_on1.text//项目名称
                        },
                        success: function (data) {
                            var result = $.parseJSON(data);
                            if (result.Success == true) {
                                //刷新权限
                                Fixture_authorize();
                            } else {

                                $.messager.alert('tips', result.Message);
                            }
                        }
                    });
                }
            });
        }
        else {
            $.messager.alert('Tips', 'Please select the row to be operated！');
        }
    }
}
//夹具初始化回显
function Fixture_authorize() {
    //如果选中项目,
    var node_on = $('#Laboratory_tree').tree('getSelected');
    if (node_on) {
        $('#Fixture_list').datagrid({
            fitColumns: true,
            //fit: true,
            queryParams: {
                TaskNum: node_on.id,
                Assortment: 2
            },
            url: '/ScheduleManagement/GetAuthList',
        });
        ////定义pagination加载内容
        //var p1_employee_authorize = $('#Fixture_list').datagrid('getPager');
        //(p1_employee_authorize).pagination({
        //    layout: ['first', 'prev', 'last', 'next']
        //});
    } else {
        $.messager.alert('Tips', 'Please select the tree to be operated！');
    }
}
//夹具初始化添加的回显
function Fixture_authorize1() {
    //如果选中项目,
    var node_on = $('#Laboratory_tree').tree('getSelected');
    if (node_on) {
        $('#Fixture_authorize').datagrid({
            fitColumns: true,
            //fit: true,
            queryParams: {
                TaskNum: node_on.id,
                Assortment: 2
            },
            url: '/ScheduleManagement/GetAuthList',
        });
        //定义pagination加载内容
        var p1_employee_authorize1 = $('#Fixture_authorize').datagrid('getPager');
        (p1_employee_authorize1).pagination({
            layout: ['first', 'prev', 'last', 'next']
        });

    } else {
        $.messager.alert('Tips', 'Please select the tree to be operated！');
    }
}
//添加 已授权
function clamp_add() {
    //如果选中项目,
    var node_on = $('#Laboratory_tree').tree('getSelected');
    if (node_on) {
        $('#Fixture_list_dialog').dialog({
            title: 'Add',
            width: 800,
            height: 500,
            buttons: [{
                text: 'close',
                iconCls: 'icon-cancel',
                handler: function () {
                    $('#Fixture_list_dialog').dialog('close');
                }
            }]
        });
        //夹具  授权加载添加
        Fixture_authorize_Add();
    } else {
        $.messager.alert('Tips', 'Please select the tree to be operated！');
    }
}
//夹具  授权加载添加
function Fixture_authorize_Add() {
    var node_on = $('#Laboratory_tree').tree('getSelected');
    //search
    $("#clamp_search1").combobox({
        data: [
                 { 'value': 'Project_name', 'text': 'AuthType' }
                 //{ 'value': 'FixtureName', 'text': 'FixtureName' }
        ]
    });
    //夹具
    $('#Fixture_datagrid').datagrid({
        border: true,
        nowrap: false,
        striped: true,
        //  rownumbers: true,
        title: "Fixture",
        //singleSelect: true,
        //autoRowHeight: true,
        ctrlSelect: true,
        // fit: true,
        fitColumns: true,
        pagination: true,
        pageSize: 15,
        pageList: [15, 30, 45, 60],
        pageNumber: 1,
        type: 'POST',
        dataType: "json",
        queryParams: {
            search: $("#clamp_search1").combobox("getValue"),
            key: $("#clamp_search").textbox("getText"),
            ProjectNum: node_on.ProjectId,
            AuthorizationType: 2
        },
        url: "/ScheduleManagement/GetUnAuthList",
        columns: [[
              { field: 'AuthType', title: 'AuthType', sortable: 'true', width: 100 },//
              //{ field: 'FixtureName', title: 'FixtureName', width: 150 },//
        ]],
        onLoadSuccess: function (data) {
            $('#Fixture_datagrid').datagrid('selectRow', 0);

        },
        onDblClickRow: function (index, row) {
            $('#add_Fixture_datagrid').click();
        },
        toolbar: clamp_authorize_toolbar
    });
    //定义pagination加载内容
    var p_111 = $('#Fixture_datagrid').datagrid('getPager');
    (p_111).pagination({
        layout: ['first', 'prev', 'last', 'next']

    });
    //
    $('#Fixture_authorize').datagrid({
        border: true,
        nowrap: false,
        striped: true,
        //rownumbers: true,
        title: "Authorize Fixture",
        ctrlSelect: true,
        // fit: true,
        fitColumns: true,
        pagination: true,
        pageSize: 15,
        pageList: [15, 30, 45, 60],
        pageNumber: 1,
        type: 'POST',
        dataType: "json",
        queryParams: {
            TaskNum: node_on.id,
            Assortment: 2
        },
        url: '/ScheduleManagement/GetAuthList',
        columns: [[
              { field: 'ScheduleType', title: 'ScheduleType', sortable: 'true', width: 100 },//
              //{ field: 'FixtureName', title: 'FixtureName', width: 150 },//
        ]],
        onLoadSuccess: function (data) {
            $('#Fixture_authorize').datagrid('selectRow', 0);

        },
        onDblClickRow: function (index, row) {
            $('#remove_Fixture_datagrid').click();
        },
        // toolbar: maintenance_plan_toolbar
    });
    //定义pagination加载内容
    var p_21 = $('#Fixture_authorize').datagrid('getPager');
    (p_21).pagination({
        layout: ['first', 'prev', 'last', 'next']

    });
    //夹具搜索
    $('#clam_find').unbind("click").bind("click", function () {
        Fixture_search();
    });
    //添加夹具
    $('#add_Fixture_datagrid').unbind("click").bind("click", function () {
        add_Fixture_datagrid();
    })
    //删除已授权夹具
    $('#remove_Fixture_datagrid').unbind("click").bind("click", function () {
        remove_Fixture_datagrid();
    })
    //添加已授权夹具
    function add_Fixture_datagrid() {
        var node_on1 = $('#Laboratory_tree').tree('getSelected');
        var selectRow_Fixture_datagrid = $('#Fixture_datagrid').datagrid('getSelected');
        if (node_on1) {
            if (selectRow_Fixture_datagrid) {
                $.ajax({
                    url: "/ScheduleManagement/AddTaskAuth",
                    type: 'POST',
                    data: {
                        id: node_on1.id,//任务id
                        Assortment: 2,//列表类型
                        ScheduleType: selectRow_Fixture_datagrid.AuthType,//类型名称
                        //FixtureName: selectRow_Fixture_datagrid.FixtureName,
                        //FixtureType: selectRow_Fixture_datagrid.FixtureType,
                        ProjectName: node_on1.text
                    },
                    success: function (data) {
                        var result = $.parseJSON(data);
                        if (result.Success == true) {
                            //刷新权限
                            Fixture_authorize();
                            Fixture_authorize1();
                        }
                        else {

                            $.messager.alert('tips', result.Message);
                        }
                    }
                });
            }
            else {
                $.messager.alert('Tips', 'Please select the row to be operated！');
            }
        } else {
            $.messager.alert('Tips', 'Please select the tree to be operated！');
        }
    }
    //删除已授权夹具
    function remove_Fixture_datagrid() {
        var node_on1 = $('#Laboratory_tree').tree('getSelected');
        var selectRow_Fixture_authorize = $('#Fixture_authorize').datagrid('getSelected');
        if (selectRow_Fixture_authorize) {
            $.messager.confirm('Confirm', 'Confirm delete?', function (r) {
                if (r) {
                    $.ajax({
                        url: "/ScheduleManagement/DelTaskAuth",
                        type: 'POST',
                        data: {
                            Assortment: 2,//类型
                            ID: selectRow_Fixture_authorize.ID,//已授权表的id
                            ScheduleType: selectRow_Fixture_authorize.ScheduleType,//被删除的类型的名称
                            ProjectName: node_on1.text//项目名称
                            //TaskId: node_on1.id,
                            // EquipmentName: selectRow_Fixture_authorize.EquipmentName,//添加的类型的名称
                        },
                        success: function (data) {
                            var result = $.parseJSON(data);
                            if (result.Success == true) {
                                //刷新权限
                                Fixture_authorize();
                                Fixture_authorize1();
                            } else {
                                $.messager.alert('tips', result.Message);
                            }
                        }
                    });
                }
            });
        }
        else {
            $.messager.alert('Tips', 'Please select the row to be operated！');
        }
    }
}
//夹具搜索
function Fixture_search() {
    var node_on = $('#Laboratory_tree').tree('getSelected');
    $('#Fixture_datagrid').datagrid({
        url: '/ScheduleManagement/GetUnAuthList',
        queryParams: {
            search: $("#clamp_search1").combobox("getValue"),
            key: $("#clamp_search").textbox("getText"),
            ProjectNum: node_on.ProjectId,
            AuthorizationType: 2
        },
    });
    //定义pagination加载内容
    var p_111 = $('#Fixture_datagrid').datagrid('getPager');
    (p_111).pagination({
        layout: ['first', 'prev', 'last', 'next']

    });
}
//夹具详细信息
function Fixture_read() {
    var selectRow = $('#Fixture_list').datagrid('getSelected');
    if (selectRow) {
        $('#clamp_datagrid_dia').dialog({
            title: 'Fixture Detail',
            width: 700,
            height: 400,
            //fit: true,
            buttons: [{
                text: 'close',
                iconCls: 'icon-cancel',
                handler: function () {
                    $('#clamp_datagrid_dia').dialog('close');
                }
            }]
        });
    } else {
        $.messager.alert('Tips', 'Please select the row to be operated！');
    }
    $('#ViewClampList').datagrid({
        nowrap: false,
        striped: true,
        //rownumbers: true,
        singleSelect: true,
        pagePosition: 'bottom',
        //autoRowHeight: true,
        //border: false,
        fitColumns: true,
        fit: true,
        pagination: true,
        pageSize: 40,
        pageNumber: 1,
        type: 'POST',
        dataType: "json",
        queryParams: {
            FixtureType: selectRow.ScheduleType
        },
        url: '/ProjectReview/GetTypeFixtureList',
        columns: [[
             { field: 'FixtureCode', title: 'FixtureCode', sortable: 'true', width: 100 },//
             { field: 'FixtureName', title: 'FixtureName', width: 100 },//
             { field: 'FDJR', title: 'FDJR', width: 100 },//
             { field: 'Location', title: 'Location', width: 100 },//
             { field: 'Purpose', title: 'Purpose', width: 100 },//
             { field: 'Qty', title: 'Qty', width: 100 },//
             {
                 field: 'Status', title: 'Status', width: 100, formatter: function (value, row, index) {//状态
                     if (value == 2) { value = '维修'; }
                     else if (value == 3) { value = '报废'; }
                     else if (value == 0) { value = '在用'; }
                     return value;
                 }
             },//
             {
                 field: 'AddDate', title: 'AddDate', width: 100, formatter: function (value, row, index) {
                     if (value) {//格式化时间
                         if (value.length > 10) {
                             value = value.substr(0, 10)
                             return value;
                         }
                     }
                 }
             },//
                {
                    field: 'ValidationDate', title: 'ValidationDate', width: 100, formatter: function (value, row, index) {
                        if (value) {//格式化时间
                            if (value.length > 10) {
                                value = value.substr(0, 10)
                                return value;
                            }
                        }
                    }
                },//
             {
                 field: 'Remark', title: 'Remark', width: 150, formatter: function (value, row, index) {
                     if (value) {
                         if (value.length > 30) {
                             var result = value.replace(" ", "");//去空
                             var value1 = result.substr(0, 30);
                             return '<span  title=' + value + '>' + value1 + "......" + '</span>';
                         } else {
                             return '<span  title=' + value + '>' + value + '</span>';
                         }
                     }
                 }
             },//
        ]],
        onLoadSuccess: function (data) {
            $('#ViewClampList').datagrid('selectRow', 0);

        },
        //   toolbar: equipment_toolbar
    });
}
//设备  初始化化加载***************************************************************设备初始化化加载*************************************************
function Equipment_load() {
    $('#Equipment_list').datagrid({
        title: "Equipment",
        nowrap: false,
        striped: true,
        singleSelect: true,
        fitColumns: true,
        //fit: true,
        pagination: true,
        pageSize: 40,
        pageNumber: 1,
        type: 'POST',
        dataType: "json",
        height: 300,
        columns: [[
            { field: 'ScheduleType', title: 'ScheduleType', sortable: 'true', width: 100 },
            //{ title: 'equipment_num', field: 'equipment_num', width: 100 },
            //{ title: 'equipment_name', field: 'equipment_name', width: 100 }
        ]],
        onLoadSuccess: function (data) {
            $('#Equipment_list').datagrid('selectRow', 0);
        },
        toolbar: equipment_toolbar
    });
    //定义pagination加载内容
    //var p = $('#Equipment_list').datagrid('getPager');
    //(p).pagination({
    //    layout: ['first', 'prev', 'last', 'next']

    //});
    var json = {
        "rows": [],
        "total": 0,
        "success": true
    };
    $('#Equipment_list').datagrid("loadData", json);
    //添加
    $("#equipment_add1").unbind("click").bind("click", function () {
        equipmentAdd();
    });
    //查看已授权设备
    $("#equipment_read").unbind("click").bind("click", function () {
        equipment_read();
    });
    //删除已授权设备
    $("#equipment_delete").unbind("click").bind("click", function () {
        equipment_delete();
    });
};
//设备初始化回显
function Equipment_authorize() {
    //如果选中项目,
    var node_on = $('#Laboratory_tree').tree('getSelected');
    if (node_on) {
        $('#Equipment_list').datagrid({
            fitColumns: true,
            //fit: true,
            queryParams: {
                TaskNum: node_on.id,
                Assortment: 1
            },
            url: '/ScheduleManagement/GetAuthList',
        });
        ////定义pagination加载内容
        //var p1_employee_authorize = $('#Equipment_list').datagrid('getPager');
        //(p1_employee_authorize).pagination({
        //    layout: ['first', 'prev', 'last', 'next']
        //});
    }
};
//详细信息
function equipment_read() {
    var selectRow = $('#Equipment_list').datagrid('getSelected');
    if (selectRow) {
        $('#equipment_datagrid_dia').dialog({
            title: 'Equipment',
            width: 700,
            height: 400,
            //fit: true,
            buttons: [{
                text: 'close',
                iconCls: 'icon-cancel',
                handler: function () {
                    $('#equipment_datagrid_dia').dialog('close');
                }
            }]
        });
    } else {
        $.messager.alert('Tips', 'Please select the row to be operated！');
    }
    $('#ViewEquipmentList').datagrid({
        nowrap: false,
        striped: true,
        //rownumbers: true,
        singleSelect: true,
        pagePosition: 'bottom',
        //autoRowHeight: true,
        //border: false,
        fitColumns: true,
        fit: true,
        pagination: true,
        pageSize: 40,
        pageNumber: 1,
        type: 'POST',
        dataType: "json",
        queryParams: {
            EquipmentType: selectRow.ScheduleType
        },
        url: '/ProjectReview/GetTypeEquipmentList',
        columns: [[
            { field: 'ScheduleEquipmentNum', title: 'ScheduleEquipmentNum', sortable: 'true', width: 100 },//
            { field: 'ScheduleEquipmentName', title: 'ScheduleEquipmentName', width: 150 },//
            { field: 'ScheduleEquipmentFlag', title: 'ScheduleEquipmentFlag', width: 150 },//
            { field: 'Assortment', title: 'Assortment', width: 150 },//
            { field: 'Capacity', title: 'Capacity', width: 150 },//
            { field: 'ScheduleLocation', title: 'ScheduleLocation', width: 150 },//
            { field: 'ScheduleState', title: 'ScheduleState', width: 150 },//
            { field: 'TestType', title: 'TestType', width: 150 },//
            { field: 'Testrange', title: 'Testrange', width: 150 },//
            {
                field: 'Remark', title: 'Remark', width: 150, formatter: function (value, row, index) {
                    if (value) {
                        if (value.length > 30) {
                            var result = value.replace(" ", "");//去空
                            var value1 = result.substr(0, 30);
                            return '<span  title=' + value + '>' + value1 + "......" + '</span>';
                        } else {
                            return '<span  title=' + value + '>' + value + '</span>';
                        }
                    }
                }
            },//
        ]],
        onLoadSuccess: function (data) {
            $('#ViewEquipmentList').datagrid('selectRow', 0);

        },
        //   toolbar: equipment_toolbar
    });
}
function Equipment_authorize1() {
    //如果选中项目,
    var node_on = $('#Laboratory_tree').tree('getSelected');
    if (node_on) {
        $('#Equipment_info_authorize').datagrid({
            fitColumns: true,
            //fit: true,
            queryParams: {
                TaskNum: node_on.id,
                Assortment: 1
            },
            url: '/ScheduleManagement/GetAuthList',
        });
        //定义pagination加载内容
        var p1_employee_authorize = $('#Equipment_info_authorize').datagrid('getPager');
        (p1_employee_authorize).pagination({
            layout: ['first', 'prev', 'last', 'next']
        });
    } else {
        $.messager.alert('Tips', 'Please select the tree to be operated！');
    }
}
//添加设备
function equipmentAdd() {
    //如果选中项目,
    var node_on = $('#Laboratory_tree').tree('getSelected');
    if (node_on) {
        $('#equipment_info_dialog').dialog({
            title: 'Add',
            width: 800,
            height: 500,
            buttons: [{
                text: 'close',
                iconCls: 'icon-cancel',
                handler: function () {
                    $('#equipment_info_dialog').dialog('close');
                }
            }]
        });
        //设备 授权加载添加
        Equipment_authorize_Add();
    } else {
        $.messager.alert('Tips', 'Please select the tree to be operated！');
    }
}
//设备 授权加载添加
function Equipment_authorize_Add() {
    var node_on = $('#Laboratory_tree').tree('getSelected');
    //search
    $("#equipment_search1").combobox({
        data: [
            { 'value': 'Project_name', 'text': 'AuthType' }
        ]
    });
    //备选试验设备
    $('#Equipment_info_datagrid').datagrid({
        nowrap: false,
        striped: true,
        //rownumbers: true,
        singleSelect: true,
        pagePosition: 'bottom',
        //autoRowHeight: true,
        //border: false,
        fitColumns: true,
        //fit: true,
        title: 'Equipment',
        pagination: true,
        pageSize: 40,
        pageNumber: 1,
        type: 'POST',
        dataType: "json",
        queryParams: {
            search: $("#equipment_search1").combobox("getValue"),
            key: $("#equipment_search").textbox("getText"),
            ProjectNum: node_on.ProjectId,
            AuthorizationType: 1,
        },
        url: '/ScheduleManagement/GetUnAuthList',
        columns: [[
           { field: 'AuthType', title: 'AuthType', sortable: 'true', width: 100 },
            //{ title: 'equipment_num', field: 'equipment_num', width: 100 },
            //{ title: 'equipment_name', field: 'equipment_name', width: 100 }
        ]],
        onLoadSuccess: function (data) {
            $('#Equipment_info_datagrid').datagrid('selectRow', 0);
        },
        onDblClickRow: function (index, row) {
            $('#add_equipment').click();
        },
        toolbar: equipment_authorize_toolbar
    });
    //定义pagination加载内容
    var p4 = $('#Equipment_info_datagrid').datagrid('getPager');
    (p4).pagination({
        layout: ['first', 'prev', 'last', 'next']
    });

    ///////////*******************备选设备加载处理end//////////////////////////

    //已选设备
    $('#Equipment_info_authorize').datagrid({
        nowrap: false,
        striped: true,
        //rownumbers: true,
        singleSelect: true,
        pagePosition: 'bottom',
        //autoRowHeight: true,
        //border: false,
        fitColumns: true,
        //fit: true,
        title: 'Authorize Equipment',
        pagination: true,
        pageSize: 20,
        pageNumber: 1,
        type: 'POST',
        dataType: "json",
        queryParams: {
            TaskNum: node_on.id,
            Assortment: 1
        },
        url: '/ScheduleManagement/GetAuthList',
        columns: [[
            { field: 'ScheduleType', title: 'ScheduleType', sortable: true, width: 100 },
        ]],
        onDblClickRow: function (index, row) {
            $('#remove_equipment').click();
        },
        onLoadSuccess: function (data) {
            $('#Equipment_info_authorize').datagrid('selectRow', 0);

        }
    });
    //定义pagination加载内容
    var p5 = $('#Equipment_info_authorize').datagrid('getPager');
    (p5).pagination({
        layout: ['first', 'prev', 'last', 'next']
    });
    //设备搜索
    $('#equipment_find').unbind("click").bind("click", function () {
        Equipment_search();
    });
    //添加设备
    $('#add_equipment').unbind("click").bind("click", function () {
        add_equipment();
    })

    //删除设备
    $('#remove_equipment').unbind("click").bind("click", function () {
        remove_equipment();
    })

    //添加设备
    function add_equipment() {
        var node_on1 = $('#Laboratory_tree').tree('getSelected');
        var selectRow_Equipment_datagrid = $('#Equipment_info_datagrid').datagrid('getSelected');
        if (node_on1) {
            if (selectRow_Equipment_datagrid) {
                $.ajax({
                    url: "/ScheduleManagement/AddTaskAuth",
                    type: 'POST',
                    data: {
                        id: node_on1.id,//任务id
                        Assortment: 1,//列表类型
                        ScheduleType: selectRow_Equipment_datagrid.AuthType,//类型名称
                        ProjectName: node_on1.text
                    },
                    success: function (data) {
                        var result = $.parseJSON(data);
                        if (result.Success == true) {
                            //刷新权限
                            Equipment_authorize();
                            Equipment_authorize1();
                        }
                        else {
                            $.messager.alert('tips', result.Message);
                        }
                    }
                });
            }
            else {
                $.messager.alert('Tips', 'Please select the row to be operated！');
            }
        } else {
            $.messager.alert('Tips', 'Please select the tree to be operated！');
        }
    }
};

function equipment_delete() {
    var node_on1 = $('#Laboratory_tree').tree('getSelected');
    var selectRow_Equipment_authorize = $('#Equipment_list').datagrid('getSelected');
    if (selectRow_Equipment_authorize) {
        $.messager.confirm('confirm', 'confirm delete', function (r) {
            if (r) {
                $.ajax({
                    url: "/ScheduleManagement/DelTaskAuth",
                    type: 'POST',
                    data: {
                        Assortment: 3,
                        ScheduleType: selectRow_Equipment_authorize.ScheduleType,//添加的类型的名称
                        ID: selectRow_Equipment_authorize.ID,//已授权表的id
                        ProjectName: node_on1.text//项目名称
                    },
                    success: function (data) {
                        var result = $.parseJSON(data);
                        if (result.Success == true) {
                            //刷新权限
                            Equipment_authorize();
                            Equipment_authorize1();
                        } else {
                            $.messager.alert('tips', result.Message);

                        }
                    }
                });
            }
        });
    }
    else {
        $.messager.alert('Tips', 'Please select the row to be operated！');
    }
}
//删除设备
function remove_equipment() {
    var node_on1 = $("#Laboratory_tree").tree("getSelected");
    var selectRow_Equipment_authorize = $('#Equipment_info_authorize').datagrid('getSelected');
    if (selectRow_Equipment_authorize) {
        $.messager.confirm('Confirm', 'Confirm delete?', function (r) {
            if (r) {
                $.ajax({
                    url: "/ScheduleManagement/DelTaskAuth",
                    type: 'POST',
                    data: {
                        Assortment: 1,//类型
                        ID: selectRow_Equipment_authorize.ID,//已授权表的id
                        ScheduleType: selectRow_Equipment_authorize.ScheduleType,//被删除的类型的名称
                        ProjectName: node_on1.text//项目名称
                    },
                    success: function (data) {
                        var result = $.parseJSON(data);
                        if (result.Success == true) {
                            //刷新权限
                            Equipment_authorize();
                            Equipment_authorize1();
                        } else {
                            $.messager.alert('tips', result.Message);
                        }
                    }
                });
            }
        });
    }
    else {
        $.messager.alert('Tips', 'Please select the row to be operated！');
    }
};
//设备搜索
function Equipment_search() {
    var node_on = $("#Laboratory_tree").tree("getSelected");
    $('#Equipment_info_datagrid').datagrid({
        queryParams: {
            search: $("#equipment_search1").combobox("getValue"),
            key: $("#equipment_search").textbox("getText"),
            ProjectNum: node_on.ProjectId,
            AuthorizationType: 1
        },
        url: '/ScheduleManagement/GetUnAuthList'
    });
    //定义pagination加载内容
    var p4 = $('#Equipment_info_datagrid').datagrid('getPager');
    (p4).pagination({
        layout: ['first', 'prev', 'last', 'next']
    });

}
//控制器  初始化化加载***************************************************************控制初始化化加载*************************************************
function Control_load() {
    //控制器
    $('#control').datagrid({
        border: true,
        nowrap: false,
        striped: true,
        rownumbers: true,
        title: "Controler",
        //singleSelect: true,
        //autoRowHeight: true,
        ctrlSelect: true,
        //  fit: true,
        height: 300,
        fitColumns: true,
        pagination: true,
        pageSize: 15,
        pageList: [15, 30, 45, 60],
        pageNumber: 1,
        type: 'POST',
        dataType: "json",
        columns: [[
              { field: 'ScheduleType', title: 'ScheduleType', sortable: 'true', width: 100 },//
              //{ field: 'ControllerNum', title: 'ControllerNum', sortable: 'true', width: 100 },//
              //{ field: 'ControllerName', title: 'ControllerName', width: 100 },//
              //{ field: 'TaskNum', title: 'TaskNum', hidden: true }
        ]],
        onLoadSuccess: function (data) {
            $('#control').datagrid('selectRow', 0);

        },
        toolbar: control_toolbar
    });
    //定义pagination加载内容
    //var l_1 = $('#control').datagrid('getPager');
    //(l_1).pagination({
    //    layout: ['first', 'prev', 'last', 'next']

    //});
    var json = {
        "rows": [],
        "total": 0,
        "success": true
    };
    $('#control').datagrid("loadData", json);
    //添加控制器
    $("#control_add").unbind("click").bind("click", function () {
        control_add();
    });
    //添加控制器程序
    $("#control_add_program").unbind("click").bind("click", function () {
        control_add_program();
    });
    //查看控制器
    $("#control_read").unbind("click").bind("click", function () {
        Control_read();
    });
    //删除 已授权
    $("#control_delete").unbind("click").bind("click", function () {
        control_delete();
    });
    function control_delete() {
        var node_on1 = $('#Laboratory_tree').tree('getSelected');
        var selectRow_Control_authorize = $('#control').datagrid('getSelected');
        if (selectRow_Control_authorize) {
            $.messager.confirm('confirm', 'confirm delete', function (r) {
                if (r) {
                    $.ajax({
                        url: "/ScheduleManagement/DelTaskAuth",
                        type: 'POST',
                        data: {
                            Assortment: 3,
                            ScheduleType: selectRow_Control_authorize.ScheduleType,//添加的类型的名称
                            ID: selectRow_Control_authorize.ID,//已授权表的id
                            ProjectName: node_on1.text//项目名称
                        },
                        success: function (data) {
                            var result = $.parseJSON(data);
                            if (result.Success == true) {
                                //刷新权限
                                Control_authorize();
                                Control_authorize1();
                            } else {
                                $.messager.alert('tips', result.Message);

                            }
                        }
                    });
                }
            });
        }
        else {
            $.messager.alert('Tips', 'Please select the row to be operated！');
        }
    }
}
//控制器初始化回显
function Control_authorize() {
    //如果选中项目,
    var node_on = $('#Laboratory_tree').tree('getSelected');
    if (node_on) {
        $('#control').datagrid({
            fitColumns: true,
            //fit: true,
            queryParams: {
                TaskNum: node_on.id,
                Assortment: 3
            },
            url: '/ScheduleManagement/GetAuthList',
        });
        //定义pagination加载内容
        var p1_employee_authorize = $('#control').datagrid('getPager');
        (p1_employee_authorize).pagination({
            layout: ['first', 'prev', 'last', 'next']
        });
    } else {
        $.messager.alert('Tips', 'Please select the tree to be operated！');
    }
}
function Control_authorize() {
    //如果选中项目,
    var node_on = $('#Laboratory_tree').tree('getSelected');
    if (node_on) {
        $('#control').datagrid({
            fitColumns: true,
            //fit: true,
            queryParams: {
                TaskNum: node_on.id,
                Assortment: 3
            },
            url: '/ScheduleManagement/GetAuthList',
        });
        ////定义pagination加载内容
        //var p1_employee_authorize = $('#control').datagrid('getPager');
        //(p1_employee_authorize).pagination({
        //    layout: ['first', 'prev', 'last', 'next']
        //});
    } else {
        $.messager.alert('Tips', 'Please select the tree to be operated！');
    }
}
function Control_authorize1() {
    //如果选中项目,
    var node_on = $('#Laboratory_tree').tree('getSelected');
    if (node_on) {
        $('#Control_authorize').datagrid({
            fitColumns: true,
            //fit: true,
            queryParams: {
                TaskNum: node_on.id,
                Assortment: 3
            },
            url: '/ScheduleManagement/GetAuthList',
        });
        //定义pagination加载内容
        var p1_employee_authorize = $('#Control_authorize').datagrid('getPager');
        (p1_employee_authorize).pagination({
            layout: ['first', 'prev', 'last', 'next']
        });
    } else {
        $.messager.alert('Tips', 'Please select the tree to be operated！');
    }
}
//添加控制器
function control_add() {
    //如果选中项目,
    var node_on = $('#Laboratory_tree').tree('getSelected');
    if (node_on) {
        $('#control_dialog').dialog({
            title: 'Add',
            width: 800,
            height: 500,
            buttons: [{
                text: 'close',
                iconCls: 'icon-cancel',
                handler: function () {
                    $('#control_dialog').dialog('close');
                }
            }]

        });
        //控制器 授权加载添加
        Control_authorize_Add();
    } else {
        $.messager.alert('Tips', 'Please select the tree to be operated！');
    }
};
//控制器 授权加载添加
function Control_authorize_Add() {
    var node_on = $('#Laboratory_tree').tree('getSelected');
    //search
    $("#control_search1").combobox({
        data: [
                 { 'value': 'Project_name', 'text': 'AuthType' },
                 //{ 'value': 'ControllerName', 'text': 'ControllerName' }
        ]
    });
    //控制器
    $('#Control_datagrid').datagrid({
        border: true,
        nowrap: false,
        striped: true,
        //rownumbers: true,
        title: "Control",
        //singleSelect: true,
        //autoRowHeight: true,
        ctrlSelect: true,
        // fit: true,
        fitColumns: true,
        pagination: true,
        pageSize: 15,
        pageList: [15, 30, 45, 60],
        pageNumber: 1,
        queryParams: {
            search: $("#control_search1").combobox("getValue"),
            key: $("#control_search").textbox("getText"),
            ProjectNum: node_on.ProjectId,
            AuthorizationType: 3,
        },
        url: '/ScheduleManagement/GetUnAuthList',
        type: 'POST',
        dataType: "json",
        columns: [[
               { field: 'AuthType', title: 'AuthType', sortable: 'true', width: 100 },//
               //{ field: 'ControllerNum', title: 'ControllerNum', sortable: 'true', width: 100 },//
               //{ field: 'ControllerName', title: 'ControllerName', width: 100 },//
        ]],
        onDblClickRow: function (index, row) {
            $('#add_Control_datagrid').click();
        },
        onLoadSuccess: function (data) {
            $('#Control_datagrid').datagrid('selectRow', 0);

        },
        toolbar: Control_authorize_toolbar
    });
    //定义pagination加载内容
    var control_1 = $('#Control_datagrid').datagrid('getPager');
    (control_1).pagination({
        layout: ['first', 'prev', 'last', 'next']

    });
    //
    $('#Control_authorize').datagrid({
        border: true,
        nowrap: false,
        striped: true,
        //rownumbers: true,
        title: "Authorize Control",
        //singleSelect: true,
        //autoRowHeight: true,
        ctrlSelect: true,
        // fit: true,
        fitColumns: true,
        pagination: true,
        pageSize: 15,
        pageList: [15, 30, 45, 60],
        pageNumber: 1,
        type: 'POST',
        dataType: "json",
        queryParams: {
            TaskNum: node_on.id,
            Assortment: 3
        },
        url: '/ScheduleManagement/GetAuthList',
        columns: [[
              { field: 'ScheduleType', title: 'ScheduleType', sortable: 'true', width: 100 },//
              //{ field: 'ControllerNum', title: 'ControllerNum', sortable: 'true', width: 100 },//
              //{ field: 'ControllerName', title: 'ControllerName', width: 100 },//
        ]],
        onDblClickRow: function (index, row) {
            $('#remove_Control_datagrid').click();
        },
        onLoadSuccess: function (data) {
            $('#Control_authorize').datagrid('selectRow', 0);

        }
        // toolbar: maintenance_plan_toolbar
    });
    //定义pagination加载内容
    var control_2 = $('#Control_authorize').datagrid('getPager');
    (control_2).pagination({
        layout: ['first', 'prev', 'last', 'next']

    });
    //控制器搜索
    $('#control_find').unbind("click").bind("click", function () {
        Control_search();
    });
    //添加已授权控制器
    $('#add_Control_datagrid').unbind("click").bind("click", function () {
        add_Control_datagrid();
    })
    //删除已授权    //添加已授权控制器

    $('#remove_Control_datagrid').unbind("click").bind("click", function () {
        remove_Control_datagrid();
    })
    //    //添加已授权控制器
    function add_Control_datagrid() {
        var node_on1 = $('#Laboratory_tree').tree('getSelected');
        var selectRow_Control_datagrid = $('#Control_datagrid').datagrid('getSelected');
        if (node_on1) {
            if (selectRow_Control_datagrid) {
                $.ajax({
                    url: "/ScheduleManagement/AddTaskAuth",
                    type: 'POST',
                    data: {
                        id: node_on1.id,//任务id
                        Assortment: 3,//列表类型
                        ScheduleType: selectRow_Control_datagrid.AuthType,//类型名称
                        ProjectName: node_on1.text
                    },
                    success: function (data) {
                        var result = $.parseJSON(data);
                        if (result.Success == true) {
                            //刷新权限
                            Control_authorize();
                            Control_authorize1();
                        }
                        else {
                            $.messager.alert('tips', result.Message);

                        }
                    }
                });
            }
            else {
                $.messager.alert('Tips', 'Please select the row to be operated！');
            }
        } else {
            $.messager.alert('Tips', 'Please select the tree to be operated！');
        }
    }
    //删除已授权控制器
    function remove_Control_datagrid() {
        var node_on1 = $('#Laboratory_tree').tree('getSelected');
        var selectRow_Control_authorize = $('#Control_authorize').datagrid('getSelected');
        if (selectRow_Control_authorize) {
            $.messager.confirm('confirm', 'confirm delete?', function (r) {
                if (r) {
                    $.ajax({
                        url: "/ScheduleManagement/DelTaskAuth",
                        type: 'POST',
                        data: {
                            Assortment: 3,
                            ScheduleType: selectRow_Control_authorize.ScheduleType,//添加的类型的名称
                            ID: selectRow_Control_authorize.ID,//已授权表的id
                            ProjectName: node_on1.text//项目名称
                        },
                        success: function (data) {
                            var result = $.parseJSON(data);
                            if (result.Success == true) {
                                //刷新权限
                                Control_authorize();
                                Control_authorize1();
                            } else {
                                $.messager.alert('tips', result.Message);

                            }
                        }
                    });
                }
            });
        }
        else {
            $.messager.alert('Tips', 'Please select the row to be operated！');
        }
    }
}
//控制器搜索
function Control_search() {
    var node_on = $('#Laboratory_tree').tree('getSelected');
    $('#Control_datagrid').datagrid({
        url: '/ScheduleManagement/GetUnAuthList',
        queryParams: {
            search: $("#control_search1").combobox("getValue"),
            key: $("#control_search").textbox("getText"),
            ProjectNum: node_on.ProjectId,
            AuthorizationType: 3
        },
    });
    //定义pagination加载内容
    var control_1 = $('#Control_datagrid').datagrid('getPager');
    (control_1).pagination({
        layout: ['first', 'prev', 'last', 'next']

    });
};
//添加控制器程序
function control_add_program() {
    var node_on = $('#Laboratory_tree').tree('getSelected');//获取选中树节点
    var selectRow = $('#control').datagrid('getSelected');
    if (node_on) {
        if (selectRow) {
            $('#Control_program_dialog').dialog({
                title: 'Add program',
                width: 800,
                height: 500,
                buttons: [{
                    text: 'close',
                    iconCls: 'icon-cancel',
                    handler: function () {
                        $('#Control_program_dialog').dialog('close');
                    }
                }]
            });
            //控制器 程序授权加载添加
            Control_program_authorize_Add();
        } else {
            $.messager.alert('Tips', 'Please select the row to be operated！');
        }
    } else {
        $.messager.alert('Tips', 'Please select the tree to be operated！');
    }
};
//控制器 授权加载添加
function Control_program_authorize_Add() {
    var node_on = $('#Laboratory_tree').tree('getSelected');//获取选中树节点
    var selectRow = $('#control').datagrid('getSelected');
    //search下拉框
    $("#program_search1").combobox({
        data: [
                 { 'value': 'ControllerNum', 'text': 'Controller Num' },
                 { 'value': 'ControllerName', 'text': 'Controller Name' }
        ]
    });
    //控制器程序列表
    $('#Control_program_datagrid').datagrid({
        border: true,
        nowrap: false,
        striped: true,
        //rownumbers: true,
        title: "Control program",
        //singleSelect: true,
        //autoRowHeight: true,
        ctrlSelect: true,
        // fit: true,
        fitColumns: true,
        pagination: true,
        pageSize: 15,
        pageList: [15, 30, 45, 60],
        pageNumber: 1,
        queryParams: {
            search: $("#program_search1").combobox("getValue"),
            key: $("#program_search").textbox("getText"),
            ProjectNum: node_on.ProjectId,
            AuthorizationType: 3,
            ControllerType: selectRow.ScheduleType
        },
        url: '/ScheduleManagement/GetUnAuthControllerProgramList',
        type: 'POST',
        dataType: "json",
        columns: [[
               { field: 'ControllerNum', title: 'Controller Num', width: 150, sortable: true },//控制器编号
               { field: 'ProgramName', title: 'Program Name', width: 150, sortable: true }//程序名称
        ]],
        onLoadSuccess: function (data) {
            $('#Control_program_datagrid').datagrid('selectRow', 0);
        },
        toolbar: Control_program_authorize_toolbar
    });
    //已授权控制器程序列表
    $('#Control_program_authorize').datagrid({
        border: true,
        nowrap: false,
        striped: true,
        //rownumbers: true,
        title: "Authorize Control Program",
        //singleSelect: true,
        //autoRowHeight: true,
        ctrlSelect: true,
        // fit: true,
        fitColumns: true,
        pagination: true,
        pageSize: 15,
        pageList: [15, 30, 45, 60],
        pageNumber: 1,
        type: 'POST',
        dataType: "json",
        queryParams: {
            TaskNum: node_on.id
        },
        url: '/ScheduleManagement/GetAuthControllerProgramList',
        columns: [[
              { field: 'ControllerNum', title: 'Controller Num', width: 150, sortable: true },//控制器编号
               { field: 'ProgramName', title: 'Program Name', width: 150, sortable: true }//程序名称
        ]],
        onDblClickRow: function (index, row) {
            $('#remove_program_datagrid').click();
        },
        onLoadSuccess: function (data) {
            $('#Control_program_authorize').datagrid('selectRow', 0);
        }
    });
    //控制器程序搜索
    $('#program_find').unbind("click").bind("click", function () {
        program_find();
    });
    //添加已授权控制器程序
    $('#add_program_datagrid').unbind("click").bind("click", function () {
        add_program_datagrid();
    })
    //删除已授权    //添加已授权控制器
    $('#remove_program_datagrid').unbind("click").bind("click", function () {
        remove_program_datagrid();
    })
    //添加已授权控制器程序
    function add_program_datagrid() {
        var node_on1 = $('#Laboratory_tree').tree('getSelected');
        var selectRow_Control_program_authorize = $('#Control_program_datagrid').datagrid('getSelected');
        if (node_on1) {
            if (selectRow_Control_program_authorize) {
                $.ajax({
                    url: "/ScheduleManagement/AddControllerProgramAuth",
                    type: 'POST',
                    data: {
                        id: node_on1.id,//任务id
                        ControllerProgramID: selectRow_Control_program_authorize.ID,//程序id
                        ProgramName: selectRow_Control_program_authorize.ProgramName,//控制器名称
                        ProjectName: node_on1.text
                    },
                    success: function (data) {
                        var result = $.parseJSON(data);
                        if (result.Success == true) {
                            //刷新权限
                            Control_program_authorize();//已授权控制器程序列表
                        }
                        else {
                            $.messager.alert('tips', result.Message);

                        }
                    }
                });
            }
            else {
                $.messager.alert('Tips', 'Please select the row to be operated！');
            }
        } else {
            $.messager.alert('Tips', 'Please select the tree to be operated！');
        }
    }
    //删除已授权控制器程序
    function remove_program_datagrid() {
        var node_on1 = $('#Laboratory_tree').tree('getSelected');
        var selectRow_Control_program_authorize = $('#Control_program_authorize').datagrid('getSelected');
        if (selectRow_Control_program_authorize) {
            $.messager.confirm('confirm', 'confirm delete?', function (r) {
                if (r) {
                    $.ajax({
                        url: "/ScheduleManagement/DelControllerProgramAuth",
                        type: 'POST',
                        data: {
                            ProgramName: selectRow_Control_program_authorize.ProgramName,//控制器名称
                            TB_CP_ID: selectRow_Control_program_authorize.TB_CP_ID,//已授权表的id
                            ProjectName: node_on1.text//项目名称
                        },
                        success: function (data) {
                            var result = $.parseJSON(data);
                            if (result.Success == true) {
                                //刷新权限
                                Control_program_authorize();//已授权控制器程序列表
                            } else {
                                $.messager.alert('tips', result.Message);

                            }
                        }
                    });
                }
            });
        }
        else {
            $.messager.alert('Tips', 'Please select the row to be operated！');
        }
    }
};
function Control_program_authorize() {
    //如果选中项目,
    var node_on = $('#Laboratory_tree').tree('getSelected');
    if (node_on) {
        $('#Control_program_authorize').datagrid({
            fitColumns: true,
            queryParams: {
                TaskNum: node_on.id
            },
            url: '/ScheduleManagement/GetAuthControllerProgramList',
        });
    } else {
        $.messager.alert('Tips', 'Please select the tree to be operated！');
    }
};
//控制器程序搜索
function program_find() {
    var node_on = $('#Laboratory_tree').tree('getSelected');
    var selectRow = $('#control').datagrid('getSelected');
    $('#Control_program_datagrid').datagrid({
        url: '/ScheduleManagement/GetUnAuthControllerProgramList',
        queryParams: {
            search: $("#program_search1").combobox("getValue"),
            key: $("#program_search").textbox("getText"),
            ProjectNum: node_on.ProjectId,
            ControllerType: selectRow.ScheduleType
        },
    });
};
//详细信息
function Control_read() {
    var selectRow = $('#control').datagrid('getSelected');
    var rowss = $('#control').datagrid('getSelections');
    if (selectRow) {
        $('#control_datagrid_dia').dialog({
            title: 'Control',
            width: 700,
            height: 400,
            //fit: true,
            buttons: [{
                text: 'close',
                iconCls: 'icon-cancel',
                handler: function () {
                    $('#control_datagrid_dia').dialog('close');
                }
            }]
        });
    } else {
        $.messager.alert('Tips', 'Please select the row to be operated！');
    }
    //控制器列表
    $('#ViewControlList').datagrid({
        nowrap: false,
        striped: true,
        //rownumbers: true,
        singleSelect: true,
        pagePosition: 'bottom',
        //autoRowHeight: true,
        //border: false,
        fitColumns: true,
        fit: true,
        pagination: true,
        pageSize: 40,
        pageNumber: 1,
        type: 'POST',
        dataType: "json",
        queryParams: {
            ControllerType: selectRow.ScheduleType
        },
        url: '/ProjectReview/GetTypeControList',
        columns: [[
           { field: 'ControllerNum', title: 'ControllerNum', sortable: 'true', width: 100 },//
           { field: 'ControllerName', title: 'ControllerName', width: 100 },//
           { field: 'Describe', title: 'Describe', width: 100 },//
           { field: 'AddDate', title: 'AddDate', width: 100 },//
           { field: 'url', title: 'url', width: 100 },//
           {
               field: 'Remark', title: 'Remark', width: 100, formatter: function (value, row, index) {
                   if (value) {
                       if (value.length > 30) {
                           var result = value.replace(" ", "");//去空
                           var value1 = result.substr(0, 30);
                           return '<span  title=' + value + '>' + value1 + "......" + '</span>';
                       } else {
                           return '<span  title=' + value + '>' + value + '</span>';
                       }
                   }
               }
           }//
        ]],
        onLoadSuccess: function (data) {
            $('#ViewControlList').datagrid('selectRow', 0);

        },
        //   toolbar: equipment_toolbar
    });
}
//报告参数初始化
function Report_load() {
    //报告
    $('#report_datagrid').datagrid({
        border: true,
        nowrap: false,
        striped: true,
        rownumbers: true,
        title: "Report",
        //singleSelect: true,
        //autoRowHeight: true,
        ctrlSelect: true,
        // fit: true,
        fitColumns: true,
        pagination: true,
        pageSize: 15,
        height: 400,
        pageList: [15, 30, 45, 60],
        pageNumber: 1,
        type: 'POST',
        dataType: "json",
        columns: [[

              { field: 'Parameter', title: 'Parameter', width: 100 },
           { field: 'Remark', title: 'Remark', width: 150 }
        ]],
        onLoadSuccess: function (data) {
            $('#report_datagrid').datagrid('selectRow', 0);

        },
        toolbar: Report_info_toolbar
    });
    var json = {
        "rows": [],
        "total": 0,
        "success": true
    };
    $('#report_datagrid').datagrid("loadData", json);

}
//报告选中加载
function Report_authorize() {
    //如果选中项目,
    var node_on = $('#Laboratory_tree').tree('getSelected');
    if (node_on) {
        $('#report_datagrid').datagrid({
            fitColumns: true,
            //fit: true,
            queryParams: {
                TaskId: node_on.id,
            },
            url: '/ScheduleManagement/GetAddedOuPutReportParameterList'
        });
        //添加
        $('#Report_info_add').unbind("click").bind("click", function () {
            add_report_datagrid();
        })
        //修改
        $('#Report_info_edit').unbind("click").bind("click", function () {
            Report_info_edit();
        })
        //删除
        $('#Report_info_delete').unbind("click").bind("click", function () {
            Report_info_delete();
        })
    } else {
        $.messager.alert('Tips', 'Please select the tree to be operated！');
    }

}
//报告授权选中加载
function Report_authorize1() {
    //如果选中项目,
    var node_on = $('#Laboratory_tree').tree('getSelected');
    if (node_on) {
        $('#report_add_authorize').datagrid({
            fitColumns: true,
            //fit: true,
            queryParams: {
                TaskId: node_on.id,
            },
            url: '/ScheduleManagement/GetAddedOuPutReportParameterList'
        });

    } else {
        $.messager.alert('Tips', 'Please select the tree to be operated！');
    }

}

//测试实验室授权报告添加
function add_report_datagrid() {

    var node_on1 = $('#Laboratory_tree').tree('getSelected');
    var selectRow_report_datagrid = $('#report_datagrid').datagrid('getSelected');
    if (node_on1) {
        $('#report_add_dialog').dialog({
            title: 'Add',
            width: 800,
            height: 500,
            buttons: [
                {
                    text: 'close',
                    iconCls: 'icon-cancel',
                    handler: function () {
                        $('#report_datagrid').datagrid('reload');
                        $('#report_add_dialog').dialog('close');
                    }
                }]
        });

    } else {
        $.messager.alert('Tips', 'Please select the tree to be operated！');
    }
    //search
    $("#report_search1").combobox({
        data: [
            { 'value': 'Parameter', 'text': 'Parameter' }
        ]
    });

    //备选试验报告参数
    $('#report_add_datagrid').datagrid({
        nowrap: false,
        striped: true,
        //rownumbers: true,
        //  ctrlSelect: true,
        pagePosition: 'bottom',
        //autoRowHeight: true,
        //border: false,
        fitColumns: true,
        fit: true,
        //  title: 'Equipment',
        pagination: true,
        pageSize: 40,
        pageNumber: 1,
        type: 'POST',
        dataType: "json",
        queryParams: {
            search: $("#report_search1").combobox("getValue"),
            key: $("#report_search").textbox("getText"),
            TaskId: node_on1.id,
        },
        url: '/ScheduleManagement/GetOutOrInParameterList',
        columns: [[
             {
                 field: 'id', checkbox: 'true'
                 //formatter: function (value, row, index) {//说明
                 //    return '<input type="checkbox"  id="OPRP_1' + row.id + ' onclick="Commonflag1();" >';
                 //}
             },
              { field: 'Parameter', title: 'Parameter', width: 100 },
              { field: 'ParameterType_n', title: 'Type', width: 150 },
              {
                  field: 'OPRP_ID', title: 'ID', width: 150, hidden: true
              }
        ]],
        onBeforeLoad: function (param) {
            firstload = 0;
        },
        onCheck: function (index, row) {
            var node_on1 = $('#Laboratory_tree').tree('getSelected');
            if (node_on1) {
                if (firstload == 1) {
                    var selectRow_index = $('#report_add_datagrid').datagrid('getRowIndex');
                    $.ajax({
                        url: "/ScheduleManagement/AddOuPutReportParameter",
                        type: 'POST',
                        data: {
                            TaskId: node_on1.id,//任务id
                            ParameterId: row.id,//
                            Parameter: row.Parameter,//类型名称
                            ProjectName: node_on1.text
                        },
                        success: function (data) {
                            var result = $.parseJSON(data);
                            if (result.Success == true) {
                                $('#report_add_datagrid').datagrid('updateRow', {
                                    index: index,
                                    row: {
                                        OPRP_ID: result.Message
                                    }
                                });
                                //刷新权限
                                // Report_authorize();
                                //Report_authorize1();
                            }
                            else {
                                $.messager.alert('tips', result.Message);
                            }
                        }
                    });
                }
            } else {
                $.messager.alert('Tips', 'Please select the tree to be operated！');
            }
        },
        onUncheck: function (index, row) {
            var node_on1 = $('#Laboratory_tree').tree('getSelected');
            if (node_on1) {
                $.ajax({
                    url: "/ScheduleManagement/DelOuPutReportParameter",
                    type: 'POST',
                    data: {
                        Parameter: row.Parameter,//添加的类型的名称
                        ID: row.OPRP_ID,//已授权表的id
                        ProjectName: node_on1.text//项目名称
                    },
                    success: function (data) {
                        var result = $.parseJSON(data);
                        if (result.Success == true) {
                            //刷新权限
                            $('#report_add_datagrid').datagrid('updateRow', {
                                index: index,
                                row: {
                                    OPRP_ID: ""
                                }
                            });
                            // Report_authorize();
                            //  Report_authorize1();
                        } else {
                            $.messager.alert('tips', result.Message);

                        }
                    }
                });
            } else {
                $.messager.alert('Tips', 'Please select the tree to be operated！');
            }
        },
        onLoadSuccess: function (data) {
            //$("#OPRP_1e6a0d5bf-d1ec-496c-b0de-7e90bf651a27").prop("checked", "checked");
            //回显选中Commonflag_1
            $.each(data.rows, function (index, item) {
                if (item.OPRP_ID != "00000000-0000-0000-0000-000000000000") {
                    $("#report_add_datagrid").datagrid("checkRow", index);
                }

            });
            firstload = 1;
            //  $('#report_add_datagrid').datagrid('selectRow', 0);

        },
        //onDblClickRow: function (index, row) {
        //    $('#add_report_add_datagrid').click();
        //},
        toolbar: report_authorize_toolbar
    });

    /*
      *functionName:
      *function:// 回显选中的行
      *Param: 
      *author:张慧敏
      *date:2018-05-21
      */
    //回显选中
    //var selectRow_rows = $('#report_datagrid').datagrid('getRows');
    //if (selectRow_rows) {
    //    for (var i = 0; i < selectRow_rows.length; i++) {
    //        $("#Commonflag_1" + selectRow_rows[i].ID).prop("checked", "checked");
    //    }
    //}
    //$.ajax({
    //  //  url: '/ScheduleManagement/GetAddedOuPutReportParameterList',
    //    data: { TaskId: node_on1.id },
    //    type: "POST",
    //    success: function (data) {
    //        var result = $.parseJSON(data);
    //        if (result.Success == true) {
    //            for (var i = 0; i < selectRow_rows.length; i++) {
    //                $("#Commonflag_1" + selectRow_rows[i].ID).prop("checked", "checked");
    //            }
    //        } else {
    //            $.messager.alert('Tips', result.Message);

    //        }
    //    }
    //});
    ///////////*******************备选报告加载处理end//////////////////////////

    //已选报告
    //$('#report_add_authorize').datagrid({
    //    nowrap: false,
    //    striped: true,
    //    //rownumbers: true,
    //    singleSelect: true,
    //    pagePosition: 'bottom',
    //    //autoRowHeight: true,
    //    //border: false,
    //    fitColumns: true,
    //    //fit: true,
    //    title: 'Authorize Equipment',
    //    pagination: true,
    //    pageSize: 20,
    //    pageNumber: 1,
    //    type: 'POST',
    //    dataType: "json",
    //    queryParams: {
    //        TaskId: node_on1.id
    //    },
    //    url: '/ScheduleManagement/GetAddedOuPutReportParameterList',
    //    columns: [[
    //        { field: 'Parameter', title: 'Parameter', width: 100 },
    //       { field: 'ParameterType_n', title: 'Type', width: 150 }
    //    ]],
    //    onDblClickRow: function (index, row) {
    //        $('#remove_report_add_datagrid').click();
    //    },
    //    onLoadSuccess: function (data) {
    //        $('#report_add_authorize').datagrid('selectRow', 0);

    //    }
    //});
    ////定义pagination加载内容
    //var p5 = $('#report_add_authorize').datagrid('getPager');
    //(p5).pagination({
    //    layout: ['first', 'prev', 'last', 'next']
    //});
    ////报告搜索
    //$('#report_find').unbind("click").bind("click", function () {
    //    reoprt_search();
    //});
    ////添加报告
    //$('#add_report_add_datagrid').unbind("click").bind("click", function () {
    //    add_report_add_datagrid();
    //})

    ////删除报告
    //$('#remove_report_add_datagrid').unbind("click").bind("click", function () {
    //    remove_report_add_datagrid();
    //})

    /*
    *functionName:Commonflag1
    *function://Commonflag编辑行 回显
    *Param: 
    *author:张慧敏
    *date:2018-05-21
    */

    //添加报告
    function add_report_add_datagrid() {
        var node_on1 = $('#Laboratory_tree').tree('getSelected');
        var selectRow_report_add_datagrid = $('#report_add_datagrid').datagrid('getChecked');
        if (node_on1) {
            if (selectRow_report_add_datagrid) {
                var selectRow_index = $('#report_add_datagrid').datagrid('getRowIndex');
                $.ajax({
                    url: "/ScheduleManagement/AddOuPutReportParameter",
                    type: 'POST',
                    data: {
                        TaskId: node_on1.id,//任务id
                        ParameterId: selectRow_report_add_datagrid.id,//
                        Parameter: selectRow_report_add_datagrid.Parameter,//类型名称
                        ProjectName: node_on1.text
                    },
                    success: function (data) {
                        var result = $.parseJSON(data);
                        if (result.Success == true) {
                            $('#report_add_datagrid').datagrid('updateRow', {
                                index: selectRow_index,
                                row: {
                                    OPRP_ID: result.Data.OPRP_ID
                                }
                            });
                            //刷新权限
                            //Report_authorize();
                            //Report_authorize1();
                        }
                        else {
                            $.messager.alert('tips', result.Message);
                        }
                    }
                });
            }
            else {
                $.messager.alert('Tips', 'Please select the row to be operated！');
            }
        } else {
            $.messager.alert('Tips', 'Please select the tree to be operated！');
        }
    }
    ////报告授权删除
    function remove_report_add_datagrid() {
        var node_on1 = $('#Laboratory_tree').tree('getSelected');
        var selectRow_report_add_authorize = $('#report_add_authorize').datagrid('getSelected');
        var selectRow_index = $('#report_add_datagrid').datagrid('getRowIndex');
        if (selectRow_report_add_authorize) {
            $.messager.confirm('confirm', 'confirm delete', function (r) {
                if (r) {
                    $.ajax({
                        url: "/ScheduleManagement/DelOuPutReportParameter",
                        type: 'POST',
                        data: {
                            Parameter: selectRow_report_add_authorize.Parameter,//添加的类型的名称
                            ID: selectRow_report_add_authorize.OPRP_ID,//已授权表的id
                            ProjectName: node_on1.text//项目名称
                        },
                        success: function (data) {
                            var result = $.parseJSON(data);
                            if (result.Success == true) {
                                //刷新权限
                                $('#report_add_datagrid').datagrid('updateRow', {
                                    index: selectRow_index,
                                    row: {
                                        OPRP_ID: ""
                                    }
                                });
                                Report_authorize();
                                //  Report_authorize1();
                            } else {
                                $.messager.alert('tips', result.Message);

                            }
                        }
                    });
                }
            });
        }
        else {
            $.messager.alert('Tips', 'Please select the row to be operated！');
        }
    }
}
//报告搜索
function reoprt_search() {
    var node_on = $('#Laboratory_tree').tree('getSelected');
    $('#report_add_datagrid').datagrid({
        url: '/ScheduleManagement/GetOutOrInParameterList',
        queryParams: {
            search: $("#report_search1").combobox("getValue"),
            key: $("#report_search").textbox("getText"),
            TaskId: node_on.id
        },
    });
    //定义pagination加载内容
    var control_1 = $('#report_add_datagrid').datagrid('getPager');
    (control_1).pagination({
        layout: ['first', 'prev', 'last', 'next']

    });
}
//编辑报告
function Report_info_edit() {

    var node_on = $('#Laboratory_tree').tree('getSelected');
    var selectRow_report_datagrid = $('#report_datagrid').datagrid('getSelected');
    if (selectRow_report_datagrid) {
        $('#report_edit_dialog').dialog({
            title: 'edit',
            width: 500,
            height: 400,
            buttons: [{
                text: 'save',
                iconCls: 'icon-ok',
                handler: function () {
                    $('#report_edit_dialog').form('submit', {
                        url: "/ScheduleManagement/EditOuPutReportParameter",//接收一般处理程序返回来的json数据     
                        onSubmit: function (param) {
                            // param.ID = selectRow_contation_datagrid.ID;
                            param.TaskId = node_on.id;
                            param.ID = selectRow_report_datagrid.ID;
                            // param.Parameter = selectRow_report_datagrid.Parameter;
                            param.ProjectName = node_on.text;
                        },
                        success: function (data) {
                            var result = $.parseJSON(data);
                            if (result.Success == true) {
                                //刷新权限
                                //刷新权限
                                Report_authorize();
                                Report_authorize1();

                                $.messager.alert('tips', result.Message);
                                $('#report_edit_dialog').dialog('close');
                            }
                            else {
                                $.messager.alert('tips', result.Message);
                            }
                        }
                    });
                }
            }, {
                text: 'close',
                iconCls: 'icon-cancel',
                handler: function () {
                    $('#report_edit_dialog').dialog('close');
                }
            }]
        });
    } else {
        $.messager.alert('Tips', 'Please select the row to be operated！');
    }

    $("#report_edit_dialog").form("load", selectRow_report_datagrid);
}
function Report_info_delete() {
    var node_on1 = $('#Laboratory_tree').tree('getSelected');
    var selectRow_report_add_authorize = $('#report_datagrid').datagrid('getSelected');
    if (selectRow_report_add_authorize) {
        $.messager.confirm('confirm', 'confirm delete', function (r) {
            if (r) {
                $.ajax({
                    url: "/ScheduleManagement/DelOuPutReportParameter",
                    type: 'POST',
                    data: {
                        Parameter: selectRow_report_add_authorize.Parameter,//添加的类型的名称
                        ID: selectRow_report_add_authorize.ID,//已授权表的id
                        ProjectName: node_on1.text//项目名称
                    },
                    success: function (data) {
                        var result = $.parseJSON(data);
                        if (result.Success == true) {
                            //刷新权限
                            Report_authorize();
                            // Report_authorize1();
                        } else {
                            $.messager.alert('tips', result.Message);

                        }
                    }
                });
            }
        });
    }
    else {
        $.messager.alert('Tips', 'Please select the row to be operated！');
    }
}
$.extend($.fn.datagrid.methods, {
    editCell: function (jq, param) {
        return jq.each(function () {
            var opts = $(this).datagrid('options');
            var fields = $(this).datagrid('getColumnFields', true).concat($(this).datagrid('getColumnFields'));
            for (var i = 0; i < fields.length; i++) {
                var col = $(this).datagrid('getColumnOption', fields[i]);
                col.editor1 = col.editor;
                if (fields[i] != param.field) {
                    col.editor = null;
                }
            }
            $(this).datagrid('beginEdit', param.index);
            for (var i = 0; i < fields.length; i++) {
                var col = $(this).datagrid('getColumnOption', fields[i]);
                col.editor = col.editor1;
            }
        });
    }
});

var editIndex = undefined;
var line = 0;
//存取编辑行的field
var field_str;
//性能参数编辑 field编辑的那个字段
function endEditing() {
    var node_on1 = $('#Laboratory_tree').tree('getSelected');//项目树
    if (editIndex == undefined) { return true }
    if ($('#Performance_parameters_info_datagrid').datagrid('validateRow', editIndex)) {

        var row = $('#Performance_parameters_info_datagrid').datagrid("getSelections");
        var row1 = $("#Performance_parameters_info_datagrid").datagrid("getSelected");
        //获取旧数据
        //  var field_old_data = $('#Performance_parameters_info_datagrid').datagrid('getEditor', { index: editIndex, field: field });

        //var test_data1 = row1.test_data;
        //var conclusion_result1 = row1.conclusion;
        //是否自动判断
        var AutoFlag = row1.AutoFlag;

        //判断值得范围
        var RangeFlag_ = row1.RangeFlag;
        //获取参照A 和B的值
        var RequiredValue1_ = row1.RequiredValue1;
        var RequiredValue2_ = row1.RequiredValue2;
        // 如果是 对下面的数据进行判断
        //if (AutoFlag == "true") {
        //    if (RangeFlag_ == "A>=B") {
        //        if (parseInt(RequiredValue1_) < parseInt(RequiredValue2_)) {
        //            $.messager.alert('tips', "A must be >= B!");
        //            return;
        //        }
        //    }
        //    else if (RangeFlag_ == "A＜X＜B" || RangeFlag_ == "A≦X＜B" || RangeFlag_ == "A＜X≤B") {
        //        if (parseInt(RequiredValue1_) >= parseInt(RequiredValue2_)) {
        //            $.messager.alert('tips', "A must be < B!");
        //            return;
        //        }
        //    }
        //}

        switch (RangeFlag_) {
            default:

        }
        $.ajax({
            url: "/ScheduleManagement/EditProjParameterValue",
            type: 'POST',
            data: {
                // flag:field,//编辑的是哪个字段
                id: row1.id,
                //Value: value,
                TaskId: node_on1.id,//项目id
                Parameter: row1.Parameter,
                ProjectName: node_on1.text,//项目名称
                RangeFlag: row1.RangeFlag,
                RequiredValue1: row1.RequiredValue1,
                RequiredValue2: row1.RequiredValue2,
                AutoFlag: row1.AutoFlag,
                remarks: row1.remarks
            },
            success: function (data) {
                var result = $.parseJSON(data);
                if (result.Success == true) {
                    //刷新权限
                    $('#Performance_parameters_info_datagrid').datagrid('reload');
                } else {
                    $.messager.alert('tips', result.Message);

                }
            }
        });
        $('#Performance_parameters_info_datagrid').datagrid('endEdit', editIndex);
        editIndex = undefined;
        return true;
    } else {
        return false;
    }
}
//点击编辑行的时间 index行号  field字段
function onClickCell(index, field) {
    line2 = index;
    if (endEditing(field)) {
        $('#Performance_parameters_info_datagrid').datagrid('selectRow', index)
                .datagrid('editCell', { index: index, field: field });

        line = index;
        editIndex = index;
    }

}

//测试路径
function endEditing1() {
    if (editIndex == undefined) { return true }
    if ($('#experimenter_info_datagrid1').datagrid('validateRow', editIndex)) {
        var row = $('#experimenter_info_datagrid1').datagrid("getSelections");
        var row1 = $("#experimenter_info_datagrid1").datagrid("getSelected");
        //获取旧数据
        // var test_data = $('#test_chemistry_Sample_table').datagrid('getEditor', { index: editIndex, field: 'test_data' });
        //var test_data1 = row1.test_data;
        //var conclusion_result1 = row1.conclusion;

        $('#experimenter_info_datagrid1').datagrid('endEdit', editIndex);
        editIndex = undefined;
        return true;
    } else {
        return false;
    }
}
function onClickCell1(index, field) {
    line2 = index;
    if (endEditing1()) {
        $('#experimenter_info_datagrid1').datagrid('selectRow', index)
                .datagrid('editCell', { index: index, field: field });

        line = index;
        editIndex = index;
    }

}
// public method for encoding
function encode(input) {
    var _keyStr = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/=";

    var output = "";
    var chr1, chr2, chr3, enc1, enc2, enc3, enc4;
    var i = 0;
    input = _utf8_encode(input);
    while (i < input.length) {
        chr1 = input.charCodeAt(i++);
        chr2 = input.charCodeAt(i++);
        chr3 = input.charCodeAt(i++);
        enc1 = chr1 >> 2;
        enc2 = ((chr1 & 3) << 4) | (chr2 >> 4);
        enc3 = ((chr2 & 15) << 2) | (chr3 >> 6);
        enc4 = chr3 & 63;
        if (isNaN(chr2)) {
            enc3 = enc4 = 64;
        } else if (isNaN(chr3)) {
            enc4 = 64;
        }
        output = output +
        _keyStr.charAt(enc1) + _keyStr.charAt(enc2) +
        _keyStr.charAt(enc3) + _keyStr.charAt(enc4);
    }
    return output;
}
// public method for decoding
function decode(input) {
    var _keyStr = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/=";

    var output = "";
    var chr1, chr2, chr3;
    var enc1, enc2, enc3, enc4;
    var i = 0;
    input = input.replace(/[^A-Za-z0-9\+\/\=]/g, "");
    while (i < input.length) {
        enc1 = _keyStr.indexOf(input.charAt(i++));
        enc2 = _keyStr.indexOf(input.charAt(i++));
        enc3 = _keyStr.indexOf(input.charAt(i++));
        enc4 = _keyStr.indexOf(input.charAt(i++));
        chr1 = (enc1 << 2) | (enc2 >> 4);
        chr2 = ((enc2 & 15) << 4) | (enc3 >> 2);
        chr3 = ((enc3 & 3) << 6) | enc4;
        output = output + String.fromCharCode(chr1);
        if (enc3 != 64) {
            output = output + String.fromCharCode(chr2);
        }
        if (enc4 != 64) {
            output = output + String.fromCharCode(chr3);
        }
    }
    output = _utf8_decode(output);
    return output;
}

// private method for UTF-8 encoding
function _utf8_encode(string) {
    var _keyStr = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/=";

    string = string.replace(/\r\n/g, "\n");
    var utftext = "";
    for (var n = 0; n < string.length; n++) {
        var c = string.charCodeAt(n);
        if (c < 128) {
            utftext += String.fromCharCode(c);
        } else if ((c > 127) && (c < 2048)) {
            utftext += String.fromCharCode((c >> 6) | 192);
            utftext += String.fromCharCode((c & 63) | 128);
        } else {
            utftext += String.fromCharCode((c >> 12) | 224);
            utftext += String.fromCharCode(((c >> 6) & 63) | 128);
            utftext += String.fromCharCode((c & 63) | 128);
        }

    }
    return utftext;
}

// private method for UTF-8 decoding
function _utf8_decode(utftext) {
    var _keyStr = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/=";

    var string = "";
    var i = 0;
    var c = c1 = c2 = 0;
    while (i < utftext.length) {
        c = utftext.charCodeAt(i);
        if (c < 128) {
            string += String.fromCharCode(c);
            i++;
        } else if ((c > 191) && (c < 224)) {
            c2 = utftext.charCodeAt(i + 1);
            string += String.fromCharCode(((c & 31) << 6) | (c2 & 63));
            i += 2;
        } else {
            c2 = utftext.charCodeAt(i + 1);
            c3 = utftext.charCodeAt(i + 2);
            string += String.fromCharCode(((c & 15) << 12) | ((c2 & 63) << 6) | (c3 & 63));
            i += 3;
        }
    }
    return string;
};

/*
*functionName:添加模板
*function:template_add
*Param: 
*author:程媛
*date:2018-05-11
*/
function template_add() {
    var node_on1 = $('#Laboratory_tree').tree('getSelected');
    var selectRow_contation_datagrid = $('#contation_datagrid').datagrid('getSelected');
    if (node_on1) {
        $('#template_add_dialog').dialog({
            title: 'Choose Templete',
            //fit:true,
            width: 1250,
            height: 820,
            buttons: [{
                text: 'Copy',
                iconCls: 'icon-ok',
                handler: function () {
                    copy_templete();
                }
            }, {
                text: 'Close',
                iconCls: 'icon-cancel',
                handler: function () {
                    $('#template_add_dialog').dialog('close');
                }
            }]
        });
    } else {
        $.messager.alert('Tips', 'Please select the tree to be operated！');
    };
    //模板加载初始化
    Model_parameters_load_init();
    //评审条件初始化
    ConditionTemplate_Init();
    //评审条件测试、性能初始化
    GetAuthParameterList_Init();
};
/*
*functionName:确认拷贝模板
*function:copy_templete
*Param: TaskId，TempletID
*author:程媛
*date:2018-05-12
*/
function copy_templete() {
    var node = $('#Laboratory_tree').tree('getSelected');
    var selectRow = $("#Parameter_model_datagrid").datagrid("getSelected");
    if (selectRow) {
        $.ajax({
            url: "/ScheduleManagement/CopyTempletParameterBLL",
            type: 'POST',
            data: {
                TempletID: selectRow.ID,//获取选中行的ID传给后台
                TaskId: node.id
            },
            success: function (data) {
                var result = $.parseJSON(data);
                if (result.Success == true) {
                    $.messager.alert('Tips', result.Message);
                    $("#template_add_dialog").dialog("close");
                    $('#contation_datagrid').datagrid('reload');
                    $('#Test_parameters_info_datagrid').datagrid('reload');
                    $('#Performance_parameters_info_datagrid').datagrid('reload');
                } else {
                    $.messager.alert('Tips', result.Message);
                }
            }
        });
    } else {
        $.messager.alert('Tips', 'Please select the Template to be operated！');
    }
};
/*
*functionName:Model_parameters_load_init
*function:加载模板列表初始化
*Param: 
*author:程媛
*date:2018-05-10
*/
function Model_parameters_load_init() {
    var nodes = $('#Laboratory_tree').tree('getSelected');//获取选中树节点
    $('#Parameter_model_datagrid').datagrid({
        nowrap: false,
        striped: true,
        //rownumbers: true,
        singleSelect: true,
        //autoRowHeight: true,
        //border: false,
        fitColumns: true,
        fit: true,
        // title: 'parameter',
        pagination: true,
        pageSize: 40,
        pageNumber: 1,
        type: 'POST',
        queryParams: {
            ProjectID: nodes.ProjectId
        },
        url: '/ProjectReview/GetParameterReviewTemplateList',
        dataType: "json",
        columns: [[
               { field: 'Name', title: 'Name', width: 100, sortable: true },
               { field: 'TemplateType_n', title: 'TemplateType', width: 150 },
               {
                   field: 'Remark', title: 'Remark', width: 150, formatter: function (value, row, index) {
                       if (value) {
                           if (value.length > 30) {
                               var result = value.replace(" ", "");//去空
                               var value1 = result.substr(0, 30);
                               return '<span  title=' + value + '>' + value1 + "......" + '</span>';
                           } else {
                               return '<span  title=' + value + '>' + value + '</span>';
                           }
                       }
                   }
               }
        ]],
        sortName: 'Name',
        sortOrder: 'asc',
        onLoadSuccess: function (data) {
            $('#Parameter_model_datagrid').datagrid('selectRow', 0);
        },
        onSelect: function () {
            var select = $('#Parameter_model_datagrid').datagrid('getSelected');
            if (select) {
                //评审条件加载
                ConditionTemplate_load();
            }
        },
    });
};
/*
*functionName:ConditionTemplate_Init
*function:条件参数列表初始化
*Param: 
*author:程媛
*date:2018-05-11
*/
function ConditionTemplate_Init() {
    var nodes = $('#Laboratory_tree').tree('getSelected');
    var selectRow = $("#Parameter_model_datagrid").datagrid("getSelected");//获取选中行
    $('#ConditionTemplate_datagrid').datagrid({
        nowrap: false,
        striped: true,
        //rownumbers: true,
        singleSelect: true,
        //autoRowHeight: true,
        //border: false,
        fitColumns: true,
        //fit: true,
        // title: 'parameter',
        pagination: true,
        pageSize: 40,
        pageNumber: 1,
        //url: '/ProjectReview/GetParameterReviewTemplateList',
        type: 'POST',
        //queryParams: {
        //    TemplateID: selectRow.ID
        //},
        dataType: "json",
        columns: [[
               { field: 'ConditionName', title: 'ConditionName', width: 100, sortable: true },
               {
                   field: 'Remark', title: 'Remark', width: 150, formatter: function (value, row, index) {
                       if (value) {
                           if (value.length > 30) {
                               var result = value.replace(" ", "");//去空
                               var value1 = result.substr(0, 30);
                               return '<span  title=' + value + '>' + value1 + "......" + '</span>';
                           } else {
                               return '<span  title=' + value + '>' + value + '</span>';
                           }
                       }
                   }
               }
        ]],
        sortName: 'ConditionName',
        sortOrder: 'asc',
        onLoadSuccess: function (data) {
            $('#ConditionTemplate_datagrid').datagrid('selectRow', 0);

        },
        onSelect: function () {
            var selectRow_ = $("#ConditionTemplate_datagrid").datagrid("getSelected");//获取选中行

            GetAuthParameterList_Load();//得到已授权的参数
        },
    });
};
/*
*functionName:ConditionTemplate_load
*function:评审输出条件模板加载
*Param: 
*author:程媛
*date:2018-05-11
*/
function ConditionTemplate_load() {
    var nodes = $('#Laboratory_tree').tree('getSelected');
    var selectRow = $('#Parameter_model_datagrid').datagrid('getSelected');
    if (selectRow) {
        $('#ConditionTemplate_datagrid').datagrid({
            fitColumns: true,
            queryParams: {
                TemplateID: selectRow.ID
            },
            url: '/ProjectReview/GetConditionTemplateList',
        });
    } else {
        $.messager.alert('Tips', 'Please select the ParameterReviewTemplate to be operated！');
    }
};

/*
*functionName:GetAuthParameterList_Load
*function:GetAuthParameterList_Load
*Param: 
*author:程媛
*date:2018-05-11
*/
function GetAuthParameterList_Load() {
    var selectRow_ConditionTemplate_datagrid = $("#ConditionTemplate_datagrid").datagrid("getSelected");//获取选中行
    if (selectRow_ConditionTemplate_datagrid) {
        //授权参数
        $('#Contation_Test_Parameter_authorize').datagrid({
            fitColumns: true,
            //fit: true,
            queryParams: {
                GroupID: selectRow_ConditionTemplate_datagrid.ID,
                ConditionFlag: "true",//测试条件
                TemplateID: selectRow_ConditionTemplate_datagrid.TemplateID,
            },
            url: '/ProjectReview/GetAuthedParameterList',
        });
        //授权参数
        $('#Contation_Performance_parameter_authorize').datagrid({
            fitColumns: true,
            //fit: true,
            queryParams: {
                GroupID: selectRow_ConditionTemplate_datagrid.ID,
                ConditionFlag: "false",//性能参数
                TemplateID: selectRow_ConditionTemplate_datagrid.TemplateID,
            },
            url: '/ProjectReview/GetAuthedParameterList',
        });
    } else {
        var json = {
            "rows": [],
            "total": 0,
            "success": true
        };
        $('#Contation_Test_Parameter_authorize').datagrid("loadData", json);
        $('#Contation_Performance_parameter_authorize').datagrid("loadData", json);
    }
};

/*
*functionName:GetAuthParameterList_Init
*function:GetAuthParameterList_Init
*Param: 
*author:张慧敏
*date:2018-05-10
*/
function GetAuthParameterList_Init() {
    //授权参数
    $('#Contation_Test_Parameter_authorize').datagrid({
        nowrap: false,
        striped: true,
        //rownumbers: true,
        singleSelect: true,
        //autoRowHeight: true,
        //border: false,
        fitColumns: true,
        //fit: true,
        title: 'Authorization Test parameters',
        pagination: true,
        pageSize: 20,
        pageNumber: 1,
        columns: [[
           { field: 'Parameter', title: 'Parameter', width: 100, sortable: true },
           //{ field: 'ParameterType_n', title: 'Type', width: 150 },
           { field: 'ParameterUnit_n', title: 'ParameterUnit', width: 100 },
           {
               field: 'Commonflag', title: 'Common', width: 150, formatter: function (value, row, index) {
                   if (value == false) {
                       return 'No'
                   }
                   if (value == true) {
                       return 'Yes'
                   }
               }
           },

        ]],

        sortName: 'Parameter',
        sortOrder: 'asc',
        onLoadSuccess: function (data) {
            $('#Contation_Test_Parameter_authorize').datagrid('selectRow', 0);

        },
        type: 'POST',
        dataType: "json"
    });
    var json = {
        "rows": [],
        "total": 0,
        "success": true
    };
    $('#Contation_Test_Parameter_authorize').datagrid("loadData", json);

    //授权参数
    $('#Contation_Performance_parameter_authorize').datagrid({
        nowrap: false,
        striped: true,
        //rownumbers: true,
        singleSelect: true,
        //autoRowHeight: true,
        //border: false,
        fitColumns: true,
        //fit: true,
        title: 'Authorization Parameter parameters',
        pagination: true,
        pageSize: 20,
        pageNumber: 1,
        columns: [[
           { field: 'Parameter', title: 'Parameter', width: 100, sortable: true },
           //{ field: 'ParameterType_n', title: 'Type', width: 150 },
           { field: 'ParameterUnit_n', title: 'ParameterUnit', width: 100 },
           {
               field: 'Commonflag', title: 'Common', width: 150, formatter: function (value, row, index) {
                   if (value == false) {
                       return 'No'
                   }
                   if (value == true) {
                       return 'Yes'
                   }
               }
           },
        ]],
        sortName: 'Parameter',
        sortOrder: 'asc',
        onLoadSuccess: function (data) {
            $('#Contation_Performance_parameter_authorize').datagrid('selectRow', 0);

        },
        type: 'POST',
        dataType: "json"
    });
    var json = {
        "rows": [],
        "total": 0,
        "success": true
    };
};
/*
*functionName:温度初始化
*function:View_tempratureInit
*Param: 
*author:黄小文
*date:2018-05-13
*/

function View_tempratureInit() {

    var node = $('#Laboratory_tree').tree('getSelected');
    if (node) {
        $('#temprature_dialog').dialog({
            title: 'View Temperature && Humidity',
            width: 900,
            height: 450,
            // fit: true,
            buttons: [{
                text: 'Close',
                iconCls: 'icon-cancel',
                handler: function () {
                    $('#temprature_dialog').dialog('close');
                }
            }]
        });

        //温度和湿度初始化
        $('#temprature_datagrid').datagrid({
            nowrap: false,
            striped: true,
            //rownumbers: true,
            singleSelect: true,

            fit: true,

            pagination: true,
            pageSize: 20,
            pageNumber: 1,
            type: 'POST',
            dataType: "json",
            url: "/ScheduleManagement/GetTaskTemperatureHumidityList",
            queryParams: {
                TaskId: node.id,

            },
            columns: [[
                   { field: 'SortNum', title: 'NO.', width: 150 },
               {
                   field: 'TemperatureStart', title: 'Temperature Start', width: 100
               },
               { field: 'TemperatureEnd', title: 'Temperature End', width: 150 },
               { field: 'TemperatureUnit', title: 'Unit', width: 150 },
               { field: 'HumidityStart', title: 'HumidityStart', width: 150 },
               {
                   field: 'HumidityEnd', title: 'HumidityEnd', width: 150
               },
               { field: 'HumidityUnit', title: 'Unit', width: 150 },
               { field: 'StartTime', title: 'StartTime', width: 150 },
               { field: 'EndTime', title: 'EndTime', width: 150 },
               { field: 'TotalTime', title: 'TotalTime', width: 150 }



            ]],
            sortName: 'Parameter',
            sortOrder: 'asc',
            onLoadSuccess: function (data) {
                $('#temprature_datagrid').datagrid('selectRow', 0);

            },
            toolbar: temprature_toolbar
        });


        // 添加温度湿度
        $('#temprature_add_Btn').unbind('click').bind('click', function () {

            addtempratureFormInit();

        });


        // 删除温度湿度
        $('#temprature_delete_Btn').unbind('click').bind('click', function () {

            var selectRowtemprature_datagrid = $("#temprature_datagrid").datagrid("getSelected");//获取选中行
            if (selectRowtemprature_datagrid) {
                $.ajax({
                    url: "/ScheduleManagement/DelTaskTemperatureHumidity",
                    type: 'POST',
                    dataType: 'json',
                    data: {
                        ID: selectRowtemprature_datagrid.ID

                    },
                    success: function (data) {
                        //  var result = $.parseJSON(data);
                        if (data.Success == true) {
                            $('#temprature_datagrid').datagrid("reload");
                            $.messager.alert('tips', data.Message);
                        } else {
                            $.messager.alert('tips', data.Message);
                        }
                    }
                });
            }


        });


    }
    //温度湿度添加框初始化
    function addtempratureFormInit() {

        $('#tempratureform').dialog({
            title: 'Temperature && Humidity add',
            width: 650,
            height: 400,
            // fit: true,
            buttons: [{
                text: 'Save',
                iconCls: 'icon-save',
                handler: function () {
                    Savetemprature();
                }
            }
                ,
                {
                    text: 'close',
                    iconCls: 'icon-cancel',
                    handler: function () {
                        $('#tempratureform').dialog('close');
                    }
                }]
        });


        //保存温度湿度内容
        function Savetemprature() {
            var node = $('#Laboratory_tree').tree('getSelected');
            $('#tempratureform').form('submit', {
                url: "/ScheduleManagement/AddTaskTemperatureHumidity",//接收一般处理程序返回来的json数据     
                onSubmit: function (param) {
                    param.TaskId = node.id;
                    //验证必填项目
                    return $(this).form('enableValidation').form('validate');
                },
                success: function (data) {
                    if (data) {
                        var result = $.parseJSON(data);
                        if (result.Success == true) {
                            $('#temprature_datagrid').datagrid("reload");
                            $.messager.alert('tips', result.Message);
                        } else {
                            $.messager.alert('tips', result.Message);
                        }
                    }
                }
            });


        }
    }



}