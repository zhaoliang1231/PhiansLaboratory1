var WordUrlSpit = new Array();//new一个新的数组
var word_link;//设置一个全局变量
var word_edit;//设置一个在线编辑全局变量
//******************************************************方法******************************************************************//

//加载信息
$(function () {
    word_link = $("#WordRead").attr("href");//获取office控件链接 URL
    WordUrlSpit = word_link.split("?");//以问号对获取的数组进行分割

    word_edit = $("#reports_edit_1").attr("href");//获取office控件链接 URL
    WordUrlSpit_edit = word_edit.split("?");//以问号对获取的数组进行分割
    //搜索
    $('#search').combobox({
        value:'report_num',
        data: [
                { 'value': 'report_num', 'text': '报告编号' }
        ]
    });

});

//检测报告编制-报告信息
$(function () {
    var tabs_width = screen.width - 300 - 182;
    var other_height = document.body.clientHeight;
    //显示分页条数
    var _height1 = window.screen.height - 400;
    var num = parseInt(_height1 / 25);
    //页面选择下拉框
    page_all();
    //管理处理/已经处理的
    management_all()
    //加载列表
    load_list();

    //搜索
    $("#search_info").unbind("click").bind("click", function () {
        search();
    });

    //查看附件
    $('#read_report_info').unbind('click').bind('click', function () {
        read_report_info();
    });
    //添加附件信息
    $('#add_read').unbind('click').bind('click', function () {
        add_read();
    });
    //删除附件信息
    $('#del_read').unbind('click').bind('click', function () {
        del_read();
    });
    //查看附件信息
    $('#read_read').unbind('click').bind('click', function () {
        //判断是否是图片
        var getSelected = $('#read_table').datagrid('getSelected');
        if (getSelected) {
            if (getSelected.accessory_format == ".jpg" || getSelected.accessory_format == ".jpeg" || getSelected.accessory_format == ".GPEG" || getSelected.accessory_format == ".PNG" || getSelected.accessory_format == ".png") {
                read_pic();
            } else if (getSelected.accessory_format == ".pdf") {
                view_pdf();
            } else if (getSelected.accessory_format == ".zip") {
                $.messager.alert('提示', 'zip压缩文件只能下载不能查看');
            } else {
                $.messager.alert('提示', '不是png,jpg或是pdf格式的不能打开');
            }
        } else {
            $.messager.alert('提示', '请选择你要操作得行');
        }

        //判断是否是文件
    });
    //下载附件信息
    $('#download_read').unbind('click').bind('click', function () {
        download_read();
    });

    //提交审核报告
    $('#Submit_report').unbind('click').bind('click', function () {
        submit_report();
    });
    
    //退回编制
    $('#returnCompilation').unbind('click').bind('click', function () {
        returnCompilation();
    });
    //通过审核
    $('#throughAuditing').unbind('click').bind('click', function () {
        throughAuditing()
    });

    //检测报告在线编辑
    $('#Edit_online_report').unbind('click').bind('click', function () {
        edit_online();
    });
    //查看申请原因
    $('#view_return_info').unbind('click').bind('click', function () {
        Back_report_dialog();
    });
    //预览报告
    $('#Preview_Report').unbind('click').bind('click', function () {
        //判断是待审核还是历史
        //var flag = $('#management_all').combobox("getValue");
        //if (flag == "0") {
        //    view_report();

        //} else {
        //    view_report1();
        //}

        view_report();

    });
});

//加载列表
function load_list() {
    var tabs_width = screen.width - 300 - 182;
    var other_height = document.body.clientHeight;
    //显示分页条数
    var _height1 = window.screen.height - 400;
    var num = parseInt(_height1 / 25);
    var search = $("#search").combobox('getValue');
    var key = $("#search1").textbox('getText');
    var history_flag = $('#management_all').combobox('getValue');
    var PageType = $("#page_all").combobox("getValue")

    $('#report_edit_task').datagrid(
       {
           nowrap: false,
           striped: true,
           rownumbers: true,
           singleSelect: true,
           //autoRowHeight: true,
           border: false,
           fitColumns: true,
           fit: true,
           pagination: true,
           pageSize: num,
           pageList: [num, num + 10, num + 20, num + 30],
           pageNumber: 1,
           type: 'POST',
           dataType: "json",
           url: "/ReportManagement/LoadUnusualCertificateEditList",//接收一般处理程序返回来的json数据        
           columns: [[
              { field: 'report_num', title: '报告编号', sortable: 'true' ,width:100},
              { field: 'report_name', title: '报告名称', width: 100 },
              { field: 'clientele', title: '委托人', width: 100 },
              { field: 'clientele_department', title: '用户(委托部门)', width: 100 },
              { field: 'File_format', title: '证书url', hidden: 'true', width: 100 },

              { field: 'error_remark', title: '申请信息', width: 100 },
              { field: 'accept_personnel_n', title: '申请人', width: 100 },
              {
                  field: 'accept_date', title: '申请时间', formatter: function (value, row, index) {
                      if (value) {
                          if (value.length >= 10) {
                              value = value.substr(0, 10)
                              return value;
                          }
                      }
                  }, width: 100
              },
               { field: 'review_personnel_n', title: '评审人' },
              {
                  field: 'review_date', title: '评审时间', formatter: function (value, row, index) {
                      if (value) {
                          if (value.length >= 10) {
                              value = value.substr(0, 10)
                              return value;
                          }
                      }
                  }, width: 100
              },
             { field: 'review_remarks', title: '评审说明', width: 100 },
             { field: 'review_remarks_word', title: '退回评审说明', width: 100 },
            { field: 'constitute_personnel', title: '报告编制人', width: 100 },
            {
                field: 'constitute_date', title: '编制时间', formatter: function (value, row, index) {
                    if (value) {
                        if (value.length >= 10) {
                            value = value.substr(0, 10)
                            return value;
                        }
                    }
                }, width: 100
            },
             { field: 'review_personnel_word', title: '报告评审人', width: 100 },
              {
                  field: 'review_personnel_word_date', title: '报告评审时间', formatter: function (value, row, index) {
                      if (value) {
                          if (value.length >= 10) {
                              value = value.substr(0, 10)
                              return value;
                          }
                      }
                  }, width: 100
              },
            {
                field: 'accept_state', width: 100, title: '处理状态', formatter: function (value, row, index) {
                    var return_value;
                    switch (value) {
                        case 1:
                            return_value = "异常申请审核";
                            break;
                        case 2:
                            return_value = "异常申请退回";
                            break;
                        case 3:
                            return_value = "异常编制";
                            break;
                        case 4:
                            return_value = "异常编制审核";
                            break;
                        case 5:
                            return_value = "异常编制审核退回";
                            break;
                        case 6:
                            return_value = "异常编制完成";
                            break;
                    }

                    return return_value;

                }
            },
            { field: 'other_remarks', title: '附注', width: 100 }
           ]],
           queryParams: {
               search: search,
               key: key,
               history_flag: history_flag,
               PageType: PageType
           },
           onLoadSuccess: function (data) {
               //默认选择行
               $('#report_edit_task').datagrid('selectRow', 0);
           },
           onSelect: function () {
               //提示
               //  $('#report_edit_task').datagrid('doCellTip', { cls: { 'background-color': 'red' }, delay: 1000 });
               //view_taskinfo1();
           },
           rowStyler: function (index, row) {
           },
           sortOrder: 'asc',
           toolbar: reports_toolbar
       });
}

//搜索

function search() {
    var search = $("#search").combobox('getValue');
    var key = $("#search1").textbox('getText');
    var history_flag = $('#management_all').combobox('getValue');
    var PageType = $('#page_all').combobox('getValue');
    $('#report_edit_task').datagrid(
            {
                url: "/ReportManagement/LoadUnusualCertificateEditList",//接收一般处理程序返回来的json数据    
                queryParams: {
                    PageType:PageType,
                    search: search,
                    key: key,
                    history_flag: history_flag

                },
                onLoadSuccess: function (data) {
                    $('#report_edit_task').datagrid('selectRow', 0);
                }
            }).datagrid('resize');
}



//报告在线编辑
var new_url = $("#reports_edit_1").prop("href");
function edit_online() {
    var selected_report = $("#report_edit_task").datagrid("getSelected");
    if (selected_report) {
        
        let cookie_val = getCookie("UserCount");

        $("#reports_edit_1").prop("href", WordUrlSpit_edit[0] + "?id=" + selected_report.id + '&save_type=Lossless_report_Error&OperateType=ErrorReport&UserCount_=' + cookie_val + WordUrlSpit_edit[1]);
        document.getElementById('reports_edit_1').click();
    } else {
        $.messager.alert('提示', '请选择报告要操作的报告');

    }
}

//获取cookie值
function getCookie(cname) {
    var name = cname + "=";
    var ca = document.cookie.split(';');
    for (var i = 0; i < ca.length; i++) {
        var c = ca[i].trim();
        if (c.indexOf(name) == 0) return c.substring(name.length, c.length);
    }
    return "";
}


//预览报告
var new_url1 = $("#read_doc").prop("href");
function view_report() {
    var selected_report = $("#report_edit_task").datagrid("getSelected");
    if (selected_report) {
        //$.ajax({
        //    url: "/ReportManagement/get_report_url",
        //    dataType: "text",
        //    type: 'POST',
        //    data: {
        //        report_id: selected_report.report_id,
        //    },
        //    success: function (data) {
        //        if (result.Success == true) {
        //            $.messager.alert('提示',result.Message);
        //        } else {

        //            $("#read_doc").prop("href", new_url1 + "___hxw&url=" + data);
        //            document.getElementById('read_doc').click();
        //        }
        //    }
        //});
        let cookie_val = getCookie("UserCount");

        $("#WordRead").prop("href", WordUrlSpit[0] + '?id=' + selected_report.id + "&OperateType=ErrorReport&UserCount_=" + cookie_val + WordUrlSpit[1]);
        document.getElementById('WordRead').click();
    } else {
        $.messager.alert('提示', '请选择报告要操作的报告');

    }
}
//预览报告pdf
//function view_report1() {
//    var selected_report = $("#report_edit_task").datagrid("getSelected");
//    if (selected_report) {
//        $.ajax({
//            url: "/ReportManagement/Preview_Report",
//            type: 'POST',
//            data: {
//                id: selected_report.report_id
//            },
//            success: function (data) {
//                var result = $.parseJSON(data);
//                if (result.Success == true) {
//                    $.messager.alert('提示',result.Message);
//                } else {

//                    var url_ = "/pdf_read/web/viewer.html?___hxw&url=" + data;
//                    window.parent.addTab(selected_report.report_num + "报告文件查看", url_);


//                }

//            }
//        });
//    } else {
//        $.messager.alert('提示', '请选择报告要操作的报告');
//    }
//}
//查看申请原因
function Back_report_dialog() {
    var selected_report = $("#report_edit_task").datagrid("getSelected");
    if (selected_report) {
        $('#Back_report_dialog').dialog({
            width: 420,
            height: 300,
            modal: true,
            title: '申请原因',
            draggable: true,
            buttons: [{
                text: '关闭',
                iconCls: 'icon-cancel',
                handler: function () {
                    $('#Back_report_dialog').dialog('close');
                }
            }]
        });
        $('#return_info').textbox('setText', selected_report.error_remark);
        $('#remarks').textbox('setText', selected_report.other_remarks);
    }

}
//页面选择
function page_all() {
    
    $('#page_all').combobox({
        value: "Edit",
        data: [
              { 'value': 'Edit', 'text': '异常报告编辑' },
              { 'value': 'Audit', 'text': '异常报告审核' }
        ],
        onSelect: function () {
            var page_allText = $("#page_all").combobox('getText');
            if (page_allText == "异常报告编辑") {
                //为编辑时  工具栏显示
                $(".auditingSelect").css('display', 'inline-block');
                $(".editSelect").css('display', 'none');

                $("#management_all").combobox({
                    value: 0,
                    data: [
                          { 'value': '0', 'text': '待编制' },
                          { 'value': '1', 'text': '历史编制' }
                    ],
                    onSelect: function () {
                        //历史编制  1    未提交 0
                        load_list();
                    }
                });
            } else if (page_allText == "异常报告审核") {
                //为审核时  工具栏显示
                $(".auditingSelect").css('display', 'none');
                $(".editSelect").css('display', 'inline-block');
                $("#management_all").combobox({
                    value: 0,
                    data: [
                          { 'value': '0', 'text': '待审核' },
                          { 'value': '1', 'text': '历史审核' }
                    ],
                    onSelect: function () {
                        //历史编制  1    未提交 0
                        load_list();
                    }
                });

            }


            load_list();
        }
    });

}


//管理待处理/已处理
function management_all() {

    $('#management_all').combobox({
        value: 0,
        data: [
              { 'value': '0', 'text': '待编制' },
              { 'value': '1', 'text': '历史编制' }
        ],
        onSelect: function () {

            //历史编制  1    未提交 0

            load_list();
        }
    });

}

//查看附件
function read_report_info() {
    var selectRow = $("#report_edit_task").datagrid("getSelected");
    if (selectRow) {
        $('#read_dialog').dialog({
            width: 800,
            height: 475,
            modal: true,
            title: '附件信息',
            draggable: true,
            buttons: [{
                text: '取消',
                iconCls: 'icon-cancel',
                handler: function () {
                    $('#read_dialog').dialog('close');
                }
            }]
        });
    } else {
        $.messager.alert('提示', '请选择要操作的行！');
    }

    //<%--附件信息datagrid--%>
    $('#read_table').datagrid({
        nowrap: false,
        striped: true,
        rownumbers: true,
        ctrlSelect: true,
        autoRowHeight: true,
        fitColumns: true,
        fit: true,
        border: false,
        fit: true,
        pagination: true,
        pageSize: 10,
        pageList: [5, 10, 15, 20],
        pageNumber: 1,
        type: 'POST',
        queryParams: {
            report_id: selectRow.report_id
        },
        dataType: "json",
        url: "/ReportManagement/load_accessory",//接收一般处理程序返回来的json数据        
        columns: [[
       { field: 'report_id', title: '报告id', sortable: 'true', width: 100 },
       { field: 'accessory_name', title: '附件名称', width: 100 },
       { field: 'accessory_format', title: '文件格式', width: 100 },
       { field: 'add_personnel', title: '添加人', width: 100 },
       { field: 'add_date', title: '添加时间', width: 100 },
       { field: 'remarks  ', title: '说明', width: 100 }
        ]],
        onLoadSuccess: function (data) {
            $('#read_table').datagrid('selectRow', 0);

        },
        onSelect: function () {
            //提示
            //$('#test_records_Sample_table').datagrid('doCellTip', { cls: { 'background-color': 'red' }, delay: 1000 });
        },
        rowStyler: function (index, row) {

        },
        toolbar: "#read_toolbar"
    });


}

//添加附件信息
function add_read() {
    var selectRow = $("#report_edit_task").datagrid("getSelected");
    $('#add_read_dialog').dialog({
        title: '添加附件信息',
        width: 500,
        height: 300,
        top: 100,
        buttons: [{
            text: '提交',
            iconCls: 'icon-ok',
            handler: function () {
                save(selectRow);
            }

        }, {
            text: '取消',
            iconCls: 'icon-cancel',
            handler: function () {
                $('#add_read_dialog').dialog('close');
            }
        }]

    });
}
function save(node_add) {
    $('#add_read_dialog').form('submit', {
        url: "/ReportManagement/upload_accessory",//接收一般处理程序返回来的json数据     
        onSubmit: function (param) {
            param.report_id = node_add.report_id;
            param.report_num = node_add.report_num;
        },

        success: function (data) {
            var result = $.parseJSON(data);
            if (result.Success == true) {
                $('#add_read_dialog').dialog('close');
                $('#read_table').datagrid('reload');
                $.messager.alert('Tips', result.Message);

            }
            else {
                $.messager.alert('Tips', result.Message);
            }
        }
    });

}

//删除附件信息
function del_read() {
    var selectRow = $("#report_edit_task").datagrid("getSelected");
    var selected_report = $("#read_table").datagrid("getSelected");
    if (selected_report) {
        $.messager.confirm('删除任务提示', '您确认要删除选中信息吗', function (r) {
            if (r) {
                $.ajax({
                    url: "/ReportManagement/DelAccessory",
                    type: 'POST',
                    data: {
                        report_num: selectRow.report_num,
                        report_id: selectRow.id,
                        id: selected_report.id
                    },
                    success: function (data) {
                        var result = $.parseJSON(data);
                        if (result.Success == true) {
                            $('#read_table').datagrid('reload');
                            $.messager.alert('Tips', result.Message);
                        }
                        else {
                            $.messager.alert('Tips', result.Message);
                        }
                    }
                });
            }
        })

    } else {
        $.messager.alert('提示', '请选择报告');
    }

}
/*
*functionName:
*function:下载图片、文件
*Param: 文件路径：fileurl_ 
*Param: 文件名称：filename_ 
*Param: 文件格式：fileFormat_ 
*author:黄小文
*date:2018-11-09
*/
function DownloadFile(fileurl_, filename_, fileFormat_) {


    if (fileFormat_ == "" || fileFormat_ == null) {
        $.messager.alert('提示', '非文件无法下载');

        return;
    }

    var DataFormat = fileFormat_.toLowerCase();
    //ie 下载
    if (!!window.ActiveXObject || "ActiveXObject" in window) {//判断是否为IE
        // alert(1);
        //图片的保存方法
        if (DataFormat == ".png" || DataFormat == ".jpg" || DataFormat == ".jpeg") {
            if (window.navigator.msSaveBlob) {//IE10+方法           
                var canvas_ = document.createElement('canvas');
                var img_ = document.createElement('img');
                img_.onload = function (e) {
                    canvas_.width = img_.width;
                    canvas_.height = img_.height;
                    var context = canvas_.getContext('2d');
                    context.drawImage(img_, 0, 0, img_.width, img_.height);
                    window.navigator.msSaveBlob(canvas_.msToBlob(), filename_ + fileFormat_);///保存图片
                }
                img_.src = fileurl_;
            }
        }
        else {

            window.location = fileurl_;
        }
    }
        //谷歌等浏览器下载方法
    else {

        var $a = $("<a></a>").attr("href", fileurl_).attr("download", filename_ + fileFormat_);
        $a[0].click();


    }


}

//查看图片
function read_pic() {
    var getSelected = $('#read_table').datagrid('getSelected');
    if (getSelected) {
        var rowss = $('#read_table').datagrid('getSelections');
        $("#Picture_img").attr("src", "../.." + rowss[0].accessory_url);
        $('#Picture_form').dialog({
            width: 600,
            height: 600,
            top: 40,
            modal: true,
            title: '图片',
            draggable: true,
            buttons: [{
                text: '关闭',
                iconCls: 'icon-cancel',
                handler: function () {
                    $('#Picture_form').dialog('close');
                }
            }]
        });
    } else {
        $.messager.alert('提示', '请选择要操作的行！');
    }

}
//预览附件pdf
function view_pdf() {
    var selected_report = $("#read_table").datagrid("getSelected");
    if (selected_report) {
        var url_ = "/pdf_read/web/viewer.html?___hxw&url=" + selected_report.accessory_url;
        window.parent.addTab(selected_report.report_num + "报告文件查看", url_);
    } else {
        $.messager.alert('提示', '请选择报告要操作的报告');
    }
}
//下载附件
function download_read() {
    var selected_report = $("#read_table").datagrid("getSelected");
    if (selected_report) {
        DownloadFile(selected_report.accessory_url, selected_report.accessory_name, selected_report.accessory_format);
    } else {
        $.messager.alert('提示', '请选择操作行！');
    }
}

//提交报告审核
function submit_report() {
    //加载室 组
    loadRoom();

    var selectRow = $("#report_edit_task").datagrid("getSelected");
    
    if (selectRow) {
        $('#importFileForm1').form("reset");
        $('#importFileForm1').dialog({
            width: 340,
            height: 180,
            modal: true,
            title: '提交报告审核',
            draggable: true,
            buttons: [{
                text: '确定',
                iconCls: 'icon-ok',
                handler: function () {
                    save_();
                }
            }, {
                text: '取消',
                iconCls: 'icon-cancel',
                handler: function () {
                    $('#importFileForm1').dialog('close');
                }
            }]
        });
        function save_() {
            $('#importFileForm1').form('submit', {
                url: "/ReportManagement/SubmitAbnormalReportReview",
                onSubmit: function (param) {
                    var PageType = $("#page_all").combobox("getValue"); //页面类型
                    //param.review_personnel = review_personnelId;
                    param.PageType = PageType;
                    param.id = selectRow.id;
                    param.report_id = selectRow.report_id;

                },
                success: function (data) {
                    var result = $.parseJSON(data);
                    if (result.Success == true) {
                        $('#report_edit_task').datagrid('reload');
                        $('#importFileForm1').dialog('close');
                        $.messager.alert('提示',result.Message);

                    }else {
                        $.messager.alert('提示', result.Message);
                    }
                }
            });

        }
    } else {
        $.messager.alert('提示', "请选择你要操作的报告");
    }
}



//退回编制
function returnCompilation() {
    var selected_report = $("#report_edit_task").datagrid("getSelected");
    
    if (selected_report) {
        $("#review_remarks_word").dialog({
            title: "报告退回说明",
            width: 350,
            height: 200,
            modal: true,
            buttons: [{
                text: '确认',
                iconCls:"icon-ok",
                handler: function () {
                    var remarksValue = $("#remarksValue").val();
                    $.ajax({
                        url: "/ReportManagement/BackAbnormalReviewReport",
                        type: "POST",
                        data: {
                            id: selected_report.id,
                            report_id: selected_report.report_id,
                            review_remarks_word: remarksValue
                        },
                        success: function (data) {
                            var result = $.parseJSON(data);
                            if (result.Success == true) {
                                $('#report_edit_task').datagrid('reload');
                                $.messager.alert('提示', result.Message);
                                $("#report_edit_task").datagrid("reload");
                                $("#review_remarks_word").dialog("close");

                            } else {
                                $.messager.alert('提示', result.Message);
                            }
                        }
                    });
                }
            }, {
                text: '关闭',
                iconCls: "icon-cancel",
                handler: function () {
                    $("#review_remarks_word").dialog("close");
                }
            }]
        });
       
    } else {
        $.messager.alert('提示', '请选择报告要操作的报告');
    }
}



//通过审核
function throughAuditing() {
    var selected_report = $("#report_edit_task").datagrid("getSelected");
    if (selected_report) {
        $.ajax({
            url: "/ReportManagement/SubmitAbnormalReportReview",
            type: 'POST',
            data: {
                PageType: "Audit",
                id: selected_report.id,
                report_id: selected_report.report_id
            },
            success: function (data) {
                var result = $.parseJSON(data);
                if (result.Success == true) {
                    $('#report_edit_task').datagrid('reload');
                    $.messager.alert('提示', result.Message);

                } else {
                    $.messager.alert('提示', result.Message);
                }
            }
        });

    } else {
        $.messager.alert('提示', '请选择报告要操作的报告');

    }
}

//加载室  组
function loadRoom() {

    $('#room').combobox({
        url: "/Common/LoadRoomCombobox",
        valueField: 'Value',
        textField: 'Text',
        required: true,
        //本地联系人数据模糊索引
        filter: function (q, row) {
            var opts = $(this).combobox('options');
            return row[opts.textField].indexOf(q) >= 0;
        },
        onSelect: function () {
            var roomId = $("#room").combobox("getValue")
            //详细位置---领取信息dialog
            $('#group').combobox({
                url: "/Common/LoadGroupCombobox",
                valueField: 'Value',
                textField: 'Text',
                required: true,
                //editable: false,
                queryParams: {
                    "GroupId": roomId
                },
                //本地联系人数据模糊索引
                filter: function (q, row) {
                    var opts = $(this).combobox('options');
                    return row[opts.textField].indexOf(q) >= 0;
                },
                onSelect: function () {
                    var GroupId = $("#group").combobox("getValue")
                    $('#review_personnel').combobox({
                        url: "/Common/LoadPersonnelCombobox",
                        valueField: 'Value',
                        textField: 'Text',
                        required: true,
                        //editable: false,
                        queryParams: {
                            "id": GroupId
                        },
                    });
                },
            
            })
        }
    });
}
