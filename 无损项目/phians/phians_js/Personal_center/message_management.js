////提示
//$(function () {
//    $.extend($.fn.datagrid.methods, {
//        /**
//         * 开打提示功能（基于1.3.3+版本）
//         * @param {} jq
//         * @param {} params 提示消息框的样式
//         * @return {}
//         */
//        doCellTip: function (jq, params) {
//            function showTip(showParams, td, e, dg) {
//                //无文本，不提示。
//                //if ($(td).text() == "") return;
//                params = params || {};
//                var options = dg.data('datagrid');
//                //var styler = 'style="';
//                //if (showParams.width) {
//                //    styler = styler + "width:" + showParams.width + ";"; 

//                //}
//                //if (showParams.maxWidth) {
//                //    styler = styler + "max-width:" + showParams.maxWidth + ";";
//                //}
//                //if (showParams.minWidth) {
//                //    styler = styler + "min-width:" + showParams.minWidth + ";";
//                //}
//                //styler = styler + '"';
//                showParams.content = '<div class="tipcontent" style="width:150px;">单击选中；双击打开页面；</div>';
//                $(td).tooltip({
//                    content: showParams.content,
//                    trackMouse: true,
//                    position: params.position,
//                    onHide: function () {
//                        $(this).tooltip('destroy');
//                    },
//                    onShow: function () {
//                        var tip = $(this).tooltip('tip');
//                        if (showParams.tipStyler) {
//                            tip.css(showParams.tipStyler);
//                        }
//                        if (showParams.contentStyler) {
//                            tip.find('div.tipcontent').css(showParams.contentStyler);
//                        }
//                    }
//                }).tooltip('show');
//            };
//            return jq.each(function () {
//                var grid = $(this);
//                var options = $(this).data('datagrid');
//                if (!options.tooltip) {
//                    var panel = grid.datagrid('getPanel').panel('panel');
//                    panel.find('.datagrid-body').each(function () {
//                        var delegateEle = $(this).find('> div.datagrid-body-inner').length ? $(this).find('> div.datagrid-body-inner')[0] : this;
//                        $(delegateEle).undelegate('td', 'mouseover').undelegate('td', 'mouseout').undelegate('td', 'mousemove').delegate('td[field]', {
//                            'mouseover': function (e) {
//                                //if($(this).attr('field')===undefined) return;
//                                var that = this;
//                                var setField = null;
//                                if (params.specialShowFields && params.specialShowFields.sort) {
//                                    for (var i = 0; i < params.specialShowFields.length; i++) {
//                                        if (params.specialShowFields[i].field == $(this).attr('field')) {
//                                            setField = params.specialShowFields[i];
//                                        }
//                                    }
//                                }
//                                if (setField == null) {
//                                    options.factContent = $(this).find('>div').clone().css({ 'margin-left': '-5000px', 'width': 'auto', 'display': 'inline', 'position': 'absolute' }).appendTo('body');
//                                    var factContentWidth = options.factContent.width();
//                                    params.content = $(this).text();
//                                    if (params.onlyShowInterrupt) {
//                                        if (factContentWidth > $(this).width()) {
//                                            showTip(params, this, e, grid);
//                                        }
//                                    } else {
//                                        showTip(params, this, e, grid);
//                                    }
//                                } else {
//                                    panel.find('.datagrid-body').each(function () {
//                                        var trs = $(this).find('tr[datagrid-row-index="' + $(that).parent().attr('datagrid-row-index') + '"]');
//                                        trs.each(function () {
//                                            var td = $(this).find('> td[field="' + setField.showField + '"]');
//                                            if (td.length) {
//                                                params.content = td.text();
//                                            }
//                                        });
//                                    });
//                                    showTip(params, this, e, grid);
//                                }
//                            },
//                            'mouseout': function (e) {
//                                if (options.factContent) {
//                                    options.factContent.remove();
//                                    options.factContent = null;
//                                }
//                            }
//                        });
//                    });
//                }
//            });
//        }
//        ///**
//        // * 关闭消息提示功能（基于1.3.3版本）
//        // * @param {} jq
//        // * @return {}
//        // */
//        //cancelCellTip: function (jq) {
//        //    return jq.each(function () {
//        //        var data = $(this).data('datagrid');
//        //        if (data.factContent) {
//        //            data.factContent.remove();
//        //            data.factContent = null;
//        //        }
//        //        var panel = $(this).datagrid('getPanel').panel('panel');
//        //        panel.find('.datagrid-body').undelegate('td', 'mouseover').undelegate('td', 'mouseout').undelegate('td', 'mousemove')
//        //    });
//        //}
//    });
//})

$(function () {
    $('#message_tabs').tabs({
        fit: true,
        border: false
    });
    //未读消息加载
    loadmessage();
   
  
    //已经读消息加载
    loadmessage_old();
    $(window).resize(function () {
        $('#message_tabs').tabs({
            fit: true,
            border: false
        });
       
        $('#message_show').datagrid('resize', {
            fit: true
        });
        $('#load_message_old').datagrid('resize', {
           fit:true
        });
        
    });
});

//未读消息加载
function loadmessage() {
    var _height = screen.availHeight - (window.outerHeight - window.top.document.body.clientHeight) - 150;
    var num = parseInt(_height / 25);
    
    $('#message_show').datagrid({
        border: false,
        nowrap: false,
        striped: true,
        rownumbers: true,
        //singleSelect: true,
        //singleSelect: true,
        ctrlSelect:true,
        //autoRowHeight: true,
        fit: true,
        fitColumns: true,
        pagination: true,
        
      //  pageList: [10, 20, 30, 40],
        pageNumber: 1,
        pageSize: num,
        pageList: [num, num + 10, num + 20, num+20],
        type: 'POST',
        dataType: "json",
        url: "/mainform/Personal_center/message_management.aspx?&cmd=load_message",//接收一般处理程序返回来的json数据         
        columns: [[
        { field: 'message_type', title: '消息类型', sortable: 'true' },
        { field: 'message', title: '消息内容' },
        { field: 'create_time', title: '消息时间' },
          {
              field: 'confirm_message_flag', title: '确认收到', formatter: function (value, row, index) {
                  if (value == "True") {
                      return "是";
                  }
                  if (value == "False") {
                      return "否";
                  }
              }
          }
        
        ]],
        //onSelect: function (index, row) {
        //    $('#message_show').datagrid('doCellTip', { cls: { 'background-color': 'red' }, delay: 1000 });
        //},
        onLoadSuccess: function (data) {
            //默认选择行
            $('#message_show').datagrid('selectRow', 0);
            $('#message_tabs').tabs('resize');
        },
        //onDblClickRow: function (index, row) {
        //    //打开页面判断
        //    open_web();
        //},
        toolbar: "#message_show_toolbar"
    });
   
  
    
   
   
    //确认收到消息
    fn_click_ok();
    //确认收到全部消息
    fn_click_all_ok();
}
function open_web() {
    var message_info = $("#message_show").datagrid("getSelected");
    var message_type = message_info.message_type;
    if (message_type == "待领取任务") {
        var url = "/mainform/Commissioned_management/Task_pool.aspx";
        //打开页面
        window.parent.addTab("任务池",url);
    } else if (message_type == "待检测任务") {
        var url = "/mainform/Commissioned_management/Detection_record.aspx";
        //打开页面
        window.parent.addTab("检测记录", url);
    } else if (message_type == "待审核退回任务") {
        var url = "/mainform/Commissioned_management/Commissioned_approval.aspx";
        //打开页面
        window.parent.addTab("委托审批", url);
    } else if (message_type == "报告编制") {
        var url = "/mainform/Report_management/Report_Edit.aspx";
        //打开页面
        window.parent.addTab("报告编制", url);
    } else if (message_type == "报告审核") {
        var url = "/mainform/Report_management/Report_Review.aspx";
        //打开页面
        window.parent.addTab("报告审核", url);
    } else if (message_type == "报告批准") {
        var url = "/mainform/Report_management/Report_Issue.aspx";
        //打开页面
        window.parent.addTab("报告批准", url);
    } 
}

//确认收到消息
function fn_click_ok() {

    $('#click_ok').unbind('click').bind('click', function () {
        var message_show_Row = $("#message_show").datagrid("getSelected");
        if (message_show_Row) {
            $.messager.confirm('提示', '确认已经阅读消息', function (r) {
                if (r) {
                    var selectRow = $("#message_show").datagrid('getSelections');
                    var sum = "";
                    for (var i = 0; i < selectRow.length; i++) {
                        if (i == 0) {
                            sum = selectRow[i].id;
                        }
                        if (i > 0) {
                            sum = sum + ";" + selectRow[i].id;
                        }
                    }
                    $.ajax({
                        url: "/mainform/Personal_center/message_management.aspx?&cmd=click_ok",
                        type: 'POST',
                        data: {
                            ids: sum
                        },
                        success: function (data) {
                            //alert(data)
                            if (data == "T") {
                                $("#message_show").datagrid("reload");
                            }
                        }
                    });
                }
            });
        } else {
            $.messager.alert("提示", "请选择信息")
        }
    });
}
//确认收到全部消息
function fn_click_all_ok() {

    $('#click_all_ok').unbind('click').bind('click', function () {
        $.messager.confirm('提示', '是否确认已经阅读全部消息？！', function (r) {
            if (r) {
                $.ajax({
                    url: "/mainform/Personal_center/message_management.aspx?&cmd=click_all_ok",
                    type: 'POST',
                    data: {
                        //ids: sum
                    },
                    success: function (data) {
                        //alert(data)
                        if (data == "T") {
                            $("#message_show").datagrid("reload");
                        }
                    }
                });
            }
        });
    });
}

//已经读消息加载
function loadmessage_old() {
    var _height = screen.availHeight - (window.outerHeight - window.top.document.body.clientHeight) - 150;
    var num = parseInt(_height / 25);
    $('#load_message_old').datagrid({
        border: false,
        nowrap: false,
        striped: true,
        rownumbers: true,
        singleSelect: true,
        //autoRowHeight: true,
        fit: true,
        fitColumns: true,
        pagination: true,
        pageSize: num,
        pageList: [num, num + 10, num + 20, num + 20],
        pageNumber: 1,
        type: 'POST',
        dataType: "json",
        url: "/mainform/Personal_center/message_management.aspx?&cmd=load_message_old",//接收一般处理程序返回来的json数据         
        columns: [[
        { field: 'message_type', title: '消息类型', sortable: 'true', width: '5%' },
        { field: 'message', title: '消息内容' },
        { field: 'create_time', title: '消息时间' },
        { field: 'confirm_time', title: '确认时间' ,width:'10%'},
        { field: 'confirm_message_flag', title: '确认收到', formatter: function (value, row, index) {
            if (value == "True") {
                return "是";
            }
            if (value == "False") {
                return "否";
            }
        }}
        ]],      
        onLoadSuccess: function (data) {
            //默认选择行
            $('#load_message_old').datagrid('selectRow', 0);
            $('#message_tabs').tabs('resize');
        }
    });
}