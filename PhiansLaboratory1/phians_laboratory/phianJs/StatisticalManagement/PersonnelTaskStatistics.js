var WordUrlSpit = new Array();//new一个新的数组
var word_link;//设置一个全局变量
$(function () {
    word_link = $("#WordRead").attr("href");//获取office控件链接 URL

    WordUrlSpit = word_link.split("?");//以问号对获取的数组进行分割
});

$(function () {
    var _height = window.screen.height - 430;
    report_management();
    //报告搜索
    $('#report_search').unbind('click').bind('click', function () {
        report_search();
    });

    //下载报告
    $('#download_info').unbind('click').bind('click', function () {
        var rows = $("#report_management").datagrid("getSelections");
        download_info(rows);
    });
    //预览打印报告pdf
    $('#download_report_pdf').unbind("click").bind("click", function () {
        download_report_pdf();
    });

    $('#room').combobox({
        url: "/Common/LoadRoomCombobox",
        valueField: 'Value',
        textField: 'Text',
        onSelect: function () {
            var id = $('#room').combobox('getValue');
            $('#Group').combobox({
                url: "/Common/LoadGroupCombobox",
                valueField: 'Value',
                textField: 'Text',
                queryParams: {
                    "GroupId": id
                },
                onSelect: function () {
                    var id = $('#Group').combobox('getValue');
                    $('#Personnel').combobox({
                        url: "/Common/LoadPersonnelCombobox",
                        valueField: 'Value',
                        textField: 'Text',
                        queryParams: {
                            "id": id
                        }
                    });
                },
                //本地联系人数据模糊索引
                filter: function (q, row) {
                    var opts = $(this).combobox('options');
                    return row[opts.textField].indexOf(q) >= 0;
                }
            });
        },
        //本地联系人数据模糊索引
        filter: function (q, row) {
            var opts = $(this).combobox('options');
            return row[opts.textField].indexOf(q) >= 0;
        }
    });



    //搜索
    $('#operateType').combobox({
        data: [
            { 'value': 'Inspection_personnel', 'text': '报告编制' },
            { 'value': 'Audit_personnel', 'text': '报告审核' },
            { 'value': 'issue_personnel', 'text': '报告签发' }
        ]
    });




})
//页面加载
function report_management() {
    var tabs_width = screen.width - 182;
    var other_height = window.outerHeight - document.body.clientHeight;
    //iframe可用高度
    var _height = screen.availHeight - 107 - 35 - other_height;
    //显示分页条数
    var _height1 = screen.availHeight - (window.outerHeight - window.top.document.body.clientHeight) - 300;
    var num = parseInt(_height1 / 25);
    if (tabs_width < 1110) {
        tabs_width = 1100;

    }
    if (_height < 650) {
        _height = 650;
    }

    $('#report_management').datagrid(
        {
            striped: true,
            rownumbers: true,
            //autoRowHeight: true,
            ctrlSelect: true,
            border: false,
            fitColumns: true,
            fit: true,
            height: _height,
            pagination: true,
            pageSize: 25,
            pageList: [25, 40, 50, 60],
            pageNumber: 1,
            type: 'POST',
            dataType: "json",
            url: "/StatisticalManagement/LoadPersonnelTaskStatistics",//接收一般处理程序返回来的json数据        
            columns: [[
                { field: 'report_num', title: '总报告编号', sortable: false, width: 150 },
                { field: 'report_name', title: '报告名称', sortable: false, width: 150 },
                                {
                                    field: 'Inspection_date', title: '检验日期', sortable: false, formatter: function (value, row, index) {
                                        if (value) {
                                            if (value.length >= 10) {
                                                value = value.substr(0, 10)
                                                return value;
                                            }
                                        }
                                    }, width: 100
                                },
                { field: 'Inspection_personnel_n', title: '检验人', hidden: false, sortable: false, width: 100 },
                {
                    field: 'Inspection_personnel_date', title: '检验人签字时间', hidden: false, sortable: false, formatter: function (value, row, index) {
                        if (value) {
                            if (value.length >= 10) {
                                value = value.substr(0, 10)
                                return value;
                            }
                        }
                    }, width: 100
                },


                { field: 'Audit_personnel_n', title: '审核人员', hidden: false, sortable: false, width: 100 },
                {
                    field: 'Audit_date', title: '审核时间', hidden: false, sortable: false, formatter: function (value, row, index) {
                        if (value) {
                            if (value.length >= 10) {
                                value = value.substr(0, 10)
                                return value;
                            }
                        }
                    }, width: 100
                },
                //{ field: 'level_Audit', title: '审核级别', hidden: false, sortable: false, width: 100 },
                { field: 'issue_personnel_n', title: '签发人员', hidden: false, sortable: false, width: 100 },
                {
                    field: 'issue_date', title: '签发时间', hidden: false, sortable: false, formatter: function (value, row, index) {
                        if (value) {
                            if (value.length >= 10) {
                                value = value.substr(0, 10)
                                return value;
                            }
                        }
                    }, width: 100
                },

                //{ field: 'clientele', title: '委托人', hidden: false, sortable: false, width: 100 },
                { field: 'application_num', title: '订单号', sortable: false, sortable: true, width: 100 },


                //{ field: 'report_format', title: '报告格式', hidden: false, sortable: false, width: 100 },

                { field: 'Inspection_result', title: '检验结果', hidden: false, sortable: false, width: 100 },
                { field: 'remarks', title: '备注', sortable: false, width: 100 },
                {
                    field: 'state_', title: '状态', sortable: false,
                    formatter: function (value, row, index) {
                        if (value == "0") { return "异常申请"; } if (value == "1") { return "报告编辑"; } if (value == "2") { return "报告审核"; } if (value == "3") { return "报告签发"; } if (value == "4") { return "完成"; } if (value == "5") { return "异常申请"; } if (value == "6") { return "报废申请"; } if (value == "7") { return "异常完成"; } if (value == "8") { return "报废完成"; }
                    }, width: 100
                }

            ]],

            onLoadSuccess: function (data) {
                //默认选择行
                $('#report_management').datagrid('selectRow', 0);

            },

            sortName: 'id',
            sortOrder: 'desc',
            toolbar: "#report_management_toolbar"
        });
}
//加载页面
function report_search() {
    $('#report_management').datagrid(
        {
            url: "/StatisticalManagement/LoadPersonnelTaskStatistics",//接收一般处理程序返回来的json数据     
            queryParams: {
                OperationField: $("#operateType").combobox('getValue'),
                PersonCount: $("#Personnel").combobox('getValue'),
                StartTime: $("#StartTime").datebox('getText'),
                EndTime: $("#EndTime").datebox('getText')

            },
            onLoadSuccess: function (data) {
                $('#report_management').datagrid('selectRow', 0);
            }
        }).datagrid('resize');
}
function download_info(rows) {
    var id = []
    var report_url = [];
    for (var i = 0; i < rows.length; i++) {
        id.push(rows[i].id);
        report_url.push(rows[i].report_url);

    }
    var ids = id.join(",");
    var report_urls = report_url.join(",");
    $.ajax({
        type: "post",
        async: true,            //异步请求（同步请求将会锁住浏览器，用户其他操作必须等待请求完成才可以执行）
        //url: "LosslessReport_Management.ashx?&cmd=download",
        data: {
            report_urls: report_urls,
            ids: ids,
            DownloadCheck: '0',
        },
        success: function (data) { // 接口调用成功回调函数
            // data 为服务器返回的数据
            if (data == "F") {
                $.messager.alert('提示', '下载失败！');
            } else {

                window.location = data;
            }
        }
    })
}


//下载报告
//function download_info() {
//    var selected_report = $("#report_management").datagrid("getSelected");

//    if (selected_report && selected_report.state_ != '6') {
//        //if (selected_report.accessory_format != "") {
//        console.log(selected_report.report_url);
//        // window.location.href = ;
//        //为添加downloam名字
//        var str = selected_report.report_url.split("/");
//        var str1 = str[3];
//        var str2 = str1.split('.');
//        //console.log(selected_report.report_url);
//        $("#download_info").attr("href", selected_report.report_url);
//        //console.log(selected_report.report_url);
//        console.log(str2[0]);
//        $("#download_info").attr("download", str2[0]);
//        //console.log(str2[0]);
//        //} else {
//        //    $.messager.alert('提示', '没有上传附件文件！');
//        //}
//    } else {
//        $.messager.alert('提示', '请选择操作行！');
//    }
//}

//预览报告文件
function download_report_pdf() {


    var selected_report = $("#report_management").datagrid("getSelected");

    if (selected_report) {

        let cookie_val = getCookie("UserCount");
        $("#WordRead").prop("href", WordUrlSpit[0] + '?id=' + selected_report.id + "&OperateType=Report&UserCount_=" + cookie_val + WordUrlSpit[1]);
        document.getElementById('WordRead').click();

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
