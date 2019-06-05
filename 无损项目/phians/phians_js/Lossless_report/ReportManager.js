
$(function () {
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

    //搜索
    $('#search').combobox({
        value: 'Subassembly_name',
        data: [
                { 'value': 'Subassembly_name', 'text': '部件名称' },
                { 'value': 'report_name', 'text': '报告名称' },
                { 'value': 'Job_num', 'text': '工号' },
                { 'value': 'circulation_NO', 'text': '流转卡号' },
                { 'value': 'Inspection_date', 'text': '检验时间' },//检验时间
                { 'value': 'Inspection_personnel', 'text': '报告人（用户名）' },//检验人
                { 'value': 'report_num', 'text': '报告编号' },
                { 'value': 'Audit_personnel', 'text': '审核人（用户名）' }
        ],
        onSelect: function () {
            var value = $('#search').combobox('getValue');
            if (value == "Inspection_date") {
                $("#key_span").html('<input id="key" style="width: 110px;" class="easyui-datebox"  />')
                //  $('#key1').attr('class', 'easyui-datebox');
                $.parser.parse($('#key_span'));

            } else {
                $("#key_span").html('<input id="key" style="width: 110px;" class="easyui-textbox"  />')
                $.parser.parse("#key_span");
            }
        }
    });
    $('#search1').combobox({
        value: 'Job_num',
        data: [
                { 'value': 'Subassembly_name', 'text': '部件名称' },
                { 'value': 'report_name', 'text': '报告类别' },
                { 'value': 'Job_num', 'text': '工号' },
                { 'value': 'circulation_NO', 'text': '流转卡号' },
                { 'value': 'Inspection_date', 'text': '检验时间' },//检验时间
                { 'value': 'Inspection_personnel', 'text': '报告人' },//检验人
                { 'value': 'report_num', 'text': '报告编号' },
                { 'value': 'Audit_personnel', 'text': '审核人' }
        ],
        onSelect: function () {
            var value = $('#search1').combobox('getValue');
            if (value == "Inspection_date") {
                $("#key_span1").html('<input id="key1" style="width: 110px;" class="easyui-datebox"  />')
                //  $('#key1').attr('class', 'easyui-datebox');
                $.parser.parse($('#key_span1'));

            } else {
                $("#key_span1").html('<input id="key1" style="width: 110px;" class="easyui-textbox"  />')
                $.parser.parse("#key_span1");
            }
        }
    });
    $('#search2').combobox({
        value: 'Job_num',
        data: [
                { 'value': 'Subassembly_name', 'text': '部件名称' },
                { 'value': 'report_name', 'text': '报告类别' },
                { 'value': 'Job_num', 'text': '工号' },
                { 'value': 'circulation_NO', 'text': '流转卡号' },
                { 'value': 'Inspection_date', 'text': '检验时间' },//检验时间
                { 'value': 'Inspection_personnel', 'text': '报告人' },//检验人
                { 'value': 'report_num', 'text': '报告编号' },
                { 'value': 'Audit_personnel', 'text': '审核人' }
        ],
        onSelect: function () {
            var value = $('#search2').combobox('getValue');
            if (value == "Inspection_date") {
                $("#key_span2").html('<input id="key2" style="width: 110px;" class="easyui-datebox"  />')
                //  $('#key1').attr('class', 'easyui-datebox');
                $.parser.parse($('#key_span2'));

            } else {
                $("#key_span2").html('<input id="key2" style="width: 110px;" class="easyui-textbox"  />')
                $.parser.parse("#key_span2");
            }
        }
    });
    $('#search3').combobox({
        value: 'report_num',
        data: [
                { 'value': 'Subassembly_name', 'text': '部件名称' },
                { 'value': 'report_name', 'text': '报告类别' },
                { 'value': 'Job_num', 'text': '工号' },
                { 'value': 'circulation_NO', 'text': '流转卡号' },
                { 'value': 'Inspection_date', 'text': '检验时间' },//检验时间
                { 'value': 'Inspection_personnel', 'text': '报告人' },//检验人
                { 'value': 'report_num', 'text': '报告编号' },
                { 'value': 'Audit_personnel', 'text': '审核人' }

        ],
        onSelect: function () {
            var value = $('#search3').combobox('getValue');
            if (value == "Inspection_date") {
                $("#key_span3").html('<input id="key3" style="width: 110px;" class="easyui-datebox"  />')
                //  $('#key1').attr('class', 'easyui-datebox');
                $.parser.parse($('#key_span3'));

            } else {
                $("#key_span3").html('<input id="key3" style="width: 110px;" class="easyui-textbox"  />')
                $.parser.parse("#key_span3");
            }
        }
    });
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
        var search = $("#search").combobox('getValue');
        var key = $("#key").textbox('getText');
        var search1 = $("#search1").combobox('getValue');
        var key1 = $("#key1").textbox('getText');
        var search2 = $("#search2").combobox('getValue');
        var key2 = $("#key2").textbox('getText');
        var search3 = $("#search3").combobox('getValue');
        var key3 = $("#key3").textbox('getText');
        $('#report_management').datagrid(
          {
              striped: true,
              rownumbers: true,
              //singleSelect: true,
              ctrlSelect: true,
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
              url: "LosslessReport_Monitor.ashx?&cmd=load_list",//接收一般处理程序返回来的json数据        
              columns: [[
               { field: 'report_num', title: '总报告编号', sortable: 'true' },
               { field: 'report_name', title: '报告名称' },
                { field: 'Inspection_personnel_n', title: '检验人员' },
               {
                   field: 'Inspection_date', title: '检验日期', sortable: 'true', formatter: function (value, row, index) {
                       if (value) {
                           if (value.length > 10) {
                               value = value.substr(0, 10)
                               return value;
                           }
                       }
                   }
               },
                 {
                     field: 'Audit_date', title: '二级归档日期', sortable: 'true', formatter: function (value, row, index) {
                         if (value) {
                             if (value.length > 10) {
                                 value = value.substr(0, 10)
                                 return value;
                             }
                         }
                     }
                 },
                  {
                      field: 'issue_date', title: '归档日期', sortable: 'true', formatter: function (value, row, index) {
                          if (value) {
                              if (value.length > 10) {
                                  value = value.substr(0, 10)
                                  return value;
                              }
                          }
                      }
                  },


               { field: 'Audit_personnel_n', title: '审核人员' },
               { field: 'issue_personnel_n', title: '签发人员' },
               { field: 'Job_num', title: '工号' },

               { field: 'Project_name', title: '项目名称' },
               //{ field: 'Type_', title: '规格' },
               //{ field: 'Chamfer_type', title: '坡口型式' },
                { field: 'application_num', title: '订单号', sortable: 'true' },
               { field: 'Subassembly_name', title: '部件名称' },
              { field: 'circulation_NO', title: '流转卡号' },
               {
                   field: 'state_', title: '状态', formatter: function (value, row, index) {

                       switch (value) {
                           case '1': return "编辑";
                           case '2': return "审核";
                           case '3': return "签发";
                           case '4': return "完成";
                           case '5': return "异常申请中";
                           case '6': return "报废";
                           default:

                       }

                   }
               },
               { field: 'remarks', title: '附注' }
              ]],
              queryParams: {
                  search: search,
                  key: key,
                  search1: search1,
                  key1: key1,
                  search2: search2,
                  key2: key2,
                  search3: search3,
                  key3: key3
              },
              onLoadSuccess: function (data) {
                  //默认选择行
                  $('#report_management').datagrid('selectRow', 0);
              },
              onSelect: function () {
                  //提示
                  //$('#report_management').datagrid('doCellTip', { cls: { 'background-color': 'red' }, delay: 1000 });
                  //view_taskinfo1();
              },
              sortOrder: 'asc',
              toolbar: "#report_management_toolbar"
          });
    }

    //加载页面
    function report_search() {
        $('#report_management').datagrid(
          {
              url: "LosslessReport_Monitor.ashx?&cmd=load_list",//接收一般处理程序返回来的json数据    
              queryParams: {
                  search: $("#search").combobox('getValue'),
                  key: $("#key").textbox('getText'),
                  search1: $("#search1").combobox('getValue'),
                  key1: $("#key1").textbox('getText'),
                  search2: $("#search2").combobox('getValue'),
                  key2: $("#key2").textbox('getText'),
                  search3: $("#search3").combobox('getValue'),
                  key3: $("#key3").textbox('getText')

              },
              onLoadSuccess: function (data) {
                  $('#report_management').datagrid('selectRow', 0);
              }
          }).datagrid('resize');
    }
})
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
        url: "LosslessReport_Management.ashx?&cmd=download",
        data: {
            report_urls: report_urls,
            ids: ids,
            DownloadCheck: '0',
            //    search: $("#search").combobox('getValue'),
            //    key: $("#key").textbox('getText'),
            //    search1: $("#search1").combobox('getValue'),
            //    key1: $("#key1").textbox('getText'),
            //    search2: $("#search2").combobox('getValue'),
            //    key2: $("#key2").textbox('getText'),
            //    search3: $("#search3").combobox('getValue'),
            //    key3: $("#key3").textbox('getText')
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

    if (selected_report && selected_report.state_ != '6') {
        $.messager.progress({
            text: '正在打开报告.....'
        });
        $.ajax({
            url: "LosslessReport_Monitor.ashx?&cmd=Preview_Report",
            dataType: "text",
            type: 'POST',
            data: {
                id: selected_report.id
            },
            success: function (data) {
                $.messager.progress('close');
                if (data == 'F') {
                    $.messager.alert('提示', '预览错误！');
                } else {
                    var url_ = "/pdf_read/web/viewer.html?___hxw&url=" + data;
                    window.parent.addTab(selected_report.report_num + "报告文件查看", url_);
                    $('#download_report_pdf').linkbutton('enable');
                    $('#download_report_pdf').bind("click")
                }
            },
            error: function (e) {
                $.messager.progress('close');
            },
        });

    } else {
        $.messager.alert('提示', '预览失败');
    }
}