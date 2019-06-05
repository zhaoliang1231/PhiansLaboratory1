var line = 0;
var treeid = 0;

$(function () {

    var tabs_width = screen.width - 182;
    //iframe可用高度
    var _height = $(".tab-content").height() - 100;
    //设置左边树的样式大小
    $('#_layout').layout('panel', 'west').panel('resize', {
        width: 250,
        height: _height
    });
    //设置右边列表的样式大小
    $('#_layout').layout('panel', 'east').panel('resize', {
        width: tabs_width - 300,
        //height: _height
    });
    $('#_layout').layout('resize');//页面重置，初始化

    treeDatagrid();
    //加载MTR项目树
    initProjectTree();
})
/*
*functionName:initProjectTree
*function:加载MTR项目树
*Param: 
*author:黄小文
*date:2018-09-14
*/

//获取url中的参数
function getUrlParam(name) {
    var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)"); //构造一个含有目标参数的正则表达式对象
    var r = window.location.search.substr(1).match(reg); //匹配目标参数
    if (r != null) return unescape(r[2]); return null; //返回参数值
}
function initProjectTree() {
    //获取url链接传参
    var MTRNO = unescape(getUrlParam('MTRNO'));
    $('#ProjectTree').tree({
        url: '/ScheduleManagement/LoadProjectScheduleTree',
        method: 'post',
        required: true,
        top: 0,
        fit: true,
        queryParams: {
            MTRNO: MTRNO
        },

        onSelect: function () {
            var node = $('#ProjectTree').tree('getSelected');//获取选中树节点的信息
            if (node) {
                $('#ProjectName1').textbox('setText', node.text);
                $('#PlanStartDate1').textbox('setValue', node.PlanStartDate);
                $('#PlanEndDate1').textbox('setValue', node.PlanEndDate);

                treeid = node.id;
                //渲染右边列表
                loadDatagrid();
            }

        },
        onBeforeExpand: function (node, param) {//树节点展开
            $('#ProjectTree').tree('options').url = "/ScheduleManagement/LoadProjectScheduleTree?ParentID=" + node.id;
        },
        onLoadSuccess: function (node, data) {
            //树菜单展开
            var t = $(this);
            if (data) {
                $(data).each(function (index, d) {
                    if (this.state == 'closed') {
                        t.tree('expandAll');
                    }
                });
            };


            //默认选中第一个节点
            if (data.length > 0) {
                if (treeid != 0) {
                    var node2 = $('#ProjectTree').tree('find', treeid);
                    $('#ProjectTree').tree('select', node2.target);
                } else {

                    //找到第一2元素
                    var n = $('#ProjectTree').tree('find', data[0].children[0].id);
                    //调用选中事件
                    $('#ProjectTree').tree('select', n.target);
                }

            }
        }
    });


}

var arr;
/*
*functionName:initProjectTree
*function:加载MTR子项目
*Param: 
*author:黄小文
*date:2018-09-14
*/
function treeDatagrid() {



    $('#ProjectSubList').datagrid(
    {
        border: true,
        nowrap: false,
        striped: true,
        ctrlSelect: true,
        fit: true,
        fitColumns: true,
        rownumbers: true,
        pagination: true,
        pageSize: 15,
        pageList: [15, 30, 45, 60],
        pageNumber: 1,
        type: 'POST',
        dataType: "json",
        title: 'Sub Project',

        columns: [[
           { field: 'ProjectName', title: 'Project name', width: 100, sortable: 'true' },
             {
                 field: 'PlanStartDate', title: 'Plan Start Date', width: 100, sortable: 'true', editor:
                 { type: 'datetimebox' }, formatter: function (value, row, index) {
                     //    if (value) {//格式化时间
                     //        return new Date(value).format("yyyy-MM-dd hh:mm:ss");

                     //    }
                     //    else { return ""}
                     //}
                     return value;
                 }
             },
           {
               field: 'PlanEndDate', title: 'Plan End Date', width: 100, sortable: 'true', editor:
                 { type: 'datetimebox' }, formatter: function (value, row, index) {
                     return value;
                 }

           },
             {
                 field: 'TaskRemarks', title: 'Remarks', width: 100, sortable: 'true', editor:
                   { type: 'textbox' }

             },

		   {
		       field: 'action', title: 'Action', width: 80, align: 'center',
		       formatter: function (value, row, index) {
		           if (row.editing) {
		               var s = '<a onclick="saverow(this)" style="text-align:center;border-radius:8px;width:80px;background:#2c7659;display:inline-block;height:26px;color:#fff;line-height:26px;list-style:none;text-decoration:none" class="easyui-linkbutton">Save</a> ';
		               var c = '<a onclick="cancelrow(this)" style="text-align:center;border-radius:8px;width:80px;background:#2c7659;display:inline-block;height:26px;color:#fff;line-height:26px;list-style:none;text-decoration:none" class="easyui-linkbutton">Cancel</a>';
		               return s + c;
		           } else {
		               var e = '<a onclick="editrow(this)" style="text-align:center;border-radius:8px;width:80px;background:#2c7659;display:inline-block;height:26px;color:#fff;line-height:26px;list-style:none;text-decoration:none" class="easyui-linkbutton">Edit</a> ';
		               //  var d = '<a href="#" onclick="deleterow(this)">Delete</a>';
		               return e;
		           }
		       }
		   }

        ]],
        onBeforeEdit: function (index, row) {
            row.editing = true;

            updateActions(index);
            line = index;
        },
        onAfterEdit: function (index, row) {



            row.editing = false;
            //  arr = $('#ProjectSubList').datagrid('getChanges');

            //console.log(arr);     
            // $('#ProjectSubList').datagrid('refreshRow', index);

            updateActions(index);
        },
        onEndEdit: function (index, row) {

            var ed = $('#ProjectSubList').datagrid('getEditor',
             { index: index, field: 'PlanStartDate' });

            console.log($(ed.target).datetimebox('getValue'));
            row.PlanStartDate = $(ed.target).datetimebox('getValue');

            var ed2 = $('#ProjectSubList').datagrid('getEditor',
          { index: index, field: 'PlanEndDate' });
            row.PlanEndDate = $(ed2.target).datetimebox('getValue');

            var ed3 = $('#ProjectSubList').datagrid('getEditor',
        { index: index, field: 'TaskRemarks' });
            row.TaskRemarks = $(ed3.target).textbox('getText');

            //保存数据
            Edit_save2(row);

        },
        onCancelEdit: function (index, row) {
            row.editing = false;
            updateActions(index);
        },
        //  sortName: 'UserCount',
        //  sortOrder: 'asc',
        onLoadSuccess: function (data) {
            $('#ProjectSubList').datagrid('selectRow', line);
        },
        rowStyler: function (index, row) {
            if (row.CountState == 0) {
                return 'color:#f00';
            }
        },
        toolbar: ProjectSubList_toolbar
    });
    //修改信息
    $('#edit').unbind('click').bind('click', function () {
        Edit_save();
    });
    ////编辑员工
    //$('#read_info').unbind('click').bind('click', function () {
    //    read_info();
    //});
    ////查看证书
    //$('#read_certificate').unbind('click').bind('click', function () {
    //    read_certificate();
    //});

};



/*
*functionName:initProjectTree
*function:加载MTR子项目
*Param: 
*author:黄小文
*date:2018-09-14
*/
function loadDatagrid() {
    var node = $('#ProjectTree').tree('getSelected');//获取选中树节点的信息
    $('#ProjectSubList').datagrid(
    {

        type: 'POST',
        dataType: "json",
        url: "/ScheduleManagement/GetProjectScheduleList",//接收一般处理程序返回来的json数据  
        queryParams: {
            TaskId: node.id//获取树节点的id传给后台
        },
        onLoadSuccess: function (data) {
            $('#ProjectSubList').datagrid('selectRow', line);
        },
    });
};


function EditInfo() {
    var selectRow = $("#ProjectSubList").datagrid("getSelected");//获取选中行
    if (selectRow) {
        line = $('#ProjectSubList').datagrid("getRowIndex", selectRow);
        $('#ProjectSubInfo').form('load', selectRow);//数据回显
        $('#ProjectSubInfo').dialog({
            title: 'Edit Info',
            width: 400,
            height: 280,
            buttons: [
                {
                    text: 'save',
                    iconCls: 'icon-ok',
                    handler: function () {
                        Edit_save();
                    }
                }, {
                    text: 'Close',
                    iconCls: 'icon-cancel',
                    handler: function () {
                        $('#ProjectSubInfo').dialog('close'); //关闭弹窗
                    }
                }
            ]
        });
    } else {
        $.messager.alert('Tips', 'Please select the row to be operated！');
    }
};

function Edit_save() {//主项目保存数据

    var node = $('#ProjectTree').tree('getSelected');//获取选中树节点的信息
    //if ($('#PlanEndDate1').textbox('getValue') == '') {
    //    $.messager.alert('Tips', 'Plan start date cannot be empty!');
    //} else if ($('#PlanStartDate1').textbox('getValue') == '') {
    //    $.messager.alert('Tips', 'Plan end date cannot be empty!');
    //} else {

        if (node) {


            if (node.id == ' a447d88b-c84a-4746-9d78-2842011def77') {

                return;
            }
            $.ajax({
                url: "/ScheduleManagement/EditScheduleMainProject", //接收一般处理程序返回来的json数据     
                data: {
                    TaskId: node.id,
                    //PlanEndDate: $('#PlanEndDate1').textbox('getValue'),
                    //PlanStartDate: $('#PlanStartDate1').textbox('getValue')


                },
                type: "POST",
                success: function (data) {
                    if (data) {
                        var result = $.parseJSON(data);
                        if (result.Success == true) {
                            // $('#ProjectSubInfo').dialog('close');
                            $.messager.alert('Tips', result.Message);
                            // $('#ProjectSubList').datagrid('reload');
                            //  $('#ProjectSubList').datagrid('selectRow', line);


                            $('#ProjectTree').tree('reload');


                        } else {
                            $.messager.alert('Tips', result.Message);
                        }
                    }
                }
            });
        }
    //}
};
// 行保存
function Edit_save2(row) {

    $.ajax({
        url: "/ScheduleManagement/EditScheduleProject",//接收一般处理程序返回来的json数据     
        data: {
            TaskId: row.TaskId,
            PlanEndDate: row.PlanEndDate,
            PlanStartDate: row.PlanStartDate,
            TaskRemarks: row.TaskRemarks
        },
        type: "POST",
        success: function (data) {
            if (data) {
                var result = $.parseJSON(data);
                if (result.Success == true) {
                    // $('#ProjectSubInfo').dialog('close');
                    $.messager.alert('Tips', result.Message);
                    $('#ProjectSubList').datagrid('reload');
                    $('#ProjectSubList').datagrid('selectRow', line);

                }
                else {
                    $.messager.alert('Tips', result.Message);
                }
            }
        }
    });
};



$.extend($.fn.datagrid.defaults.editors, {
    datetimebox: {// datetimebox就是你要自定义editor的名称
        init: function (container, options) {
            var input = $('<input class="easyuidatetimebox1">').appendTo(container);
            return input.datetimebox({
                formatter: function (date) {
                  
                        return new Date(date).format("yyyy-MM-dd hh:mm:ss");
                    
             
                }
            });
        },
        getValue: function (target) {
            return $(target).parent().find('input.combo-value').val();
        },
        setValue: function (target, value) {
            $(target).datetimebox("setValue", value);
        },
        resize: function (target, width) {
            var input = $(target);
            if ($.boxModel == true) {
                input.width(width - (input.outerWidth() - input.width()));
            } else {
                input.width(width);
            }
        }
    }
});
// 时间格式化
Date.prototype.format = function (format) {
    /*
    * eg:format="yyyy-MM-dd hh:mm:ss";
    */
    if (!format) {
        format = "yyyy-MM-dd hh:mm:ss";
    }

    var o = {
        "M+": this.getMonth() + 1, // month
        "d+": this.getDate(), // day
        "h+": this.getHours(), // hour
        "m+": this.getMinutes(), // minute
        "s+": this.getSeconds(), // second
        "q+": Math.floor((this.getMonth() + 3) / 3), // quarter
        "S": this.getMilliseconds()
        // millisecond
    };

    if (/(y+)/.test(format)) {
        format = format.replace(RegExp.$1, (this.getFullYear() + "").substr(4 - RegExp.$1.length));
    }

    for (var k in o) {
        if (new RegExp("(" + k + ")").test(format)) {
            format = format.replace(RegExp.$1, RegExp.$1.length == 1 ? o[k] : ("00" + o[k]).substr(("" + o[k]).length));
        }
    }
    return format;
};



function updateActions(index) {



    $('#ProjectSubList').datagrid('updateRow', {
        index: index,
        row: {}
    });
}
function getRowIndex(target) {
    var tr = $(target).closest('tr.datagrid-row');
    return parseInt(tr.attr('datagrid-row-index'));
}
function editrow(target) {
    $('#ProjectSubList').datagrid('beginEdit', getRowIndex(target));
}
function deleterow(target) {
    $.messager.confirm('Confirm', 'Are you sure?', function (r) {
        if (r) {
            $('#ProjectSubList').datagrid('deleteRow', getRowIndex(target));
        }
    });
}
function saverow(target) {
    $('#ProjectSubList').datagrid('endEdit', getRowIndex(target));


}
function cancelrow(target) {
    $('#ProjectSubList').datagrid('cancelEdit', getRowIndex(target));
}
function insert() {
    var row = $('#ProjectSubList').datagrid('getSelected');
    if (row) {
        var index = $('#ProjectSubList').datagrid('getRowIndex', row);
    } else {
        index = 0;
    }
    $('#ProjectSubList').datagrid('insertRow', {
        index: index,
        row: {
            status: 'P'
        }
    });
    $('#ProjectSubList').datagrid('selectRow', index);
    $('#ProjectSubList').datagrid('beginEdit', index);
}