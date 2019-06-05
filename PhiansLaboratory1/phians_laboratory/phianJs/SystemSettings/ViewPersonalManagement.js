var line = 0;//定义列表的行

$(function () {
    var tabs_width = screen.width - 182;
    //iframe可用高度
    var _height = $(".tab-content").height();
    //设置左边树的样式大小
    $('#department_layout').layout('panel', 'west').panel('resize', {
        width: 150,
        height: _height
    });
    //设置右边列表的样式大小
    $('#department_layout').layout('panel', 'east').panel('resize', {
        width: tabs_width - 300,
        //height: _height
    });
    $('#department_layout').layout('resize');//页面重置，初始化

    department_tree_init();//******************************************************组织架构管理树初始化  
 
    $('#search').combobox({
        data: [
            { 'value': 'UserCount', 'text': 'UserCount' },
            { 'value': 'UserName', 'text': 'UserName' }
        ]
    });
    $('#search1').textbox({
        value: ''
    });
});

//********************************************************************组织架构管理树初始化********************************************************************
//********************************************************************组织架构管理树
function department_tree_init() {
    $('#department_info').tree({
        url: '/SystemSettings/GetDepartmentOne',
        method: 'post',
        required: true,
        top: 0,
        fit: true,
        onBeforeExpand: function (node, param) {//树节点展开
            $('#department_info').tree('options').url = "/SystemSettings/GetDepartmentOne?ParentId=" + node.id;
        },
        onSelect: function () {
            //渲染右边列表
            treeDatagrid();
        },
        onLoadSuccess: function (node, data) {
            //树菜单展开
            var t = $(this);
            if (data) {
                $(data).each(function (index, d) {
                    if (this.state == 'closed') {
                        t.tree('expandAll');
                    }
                })
            };
            //默认选中第一个节点
            if (data.length > 0) {
                //找到第一个元素
                var n = $('#department_info').tree('find', data[0].id);
                //调用选中事件
                $('#department_info').tree('select', n.target);
            };
        }
    });
};


//********************************************************************人员详细信息列表初始化********************************************************************
//********************************************************************人员详细信息列表
function treeDatagrid() {
    var node = $('#department_info').tree('getSelected');//获取选中树节点的信息
    $('#department_people').datagrid(
    {
        striped: true,
        rownumbers: true,
        ctrlSelect: true,
        border: true,
        fit: true,
        pagination: true,
        pageSize: 20,
        pageList: [20, 40, 60, 80],
        pageNumber: 1,
        type: 'POST',
        dataType: "json",
        url: "/SystemSettings/GetDepUserList",//接收一般处理程序返回来的json数据  
        queryParams: {
            NodeId: node.id//获取树节点的id传给后台
        },
        columns: [[
           { field: 'UserCount', title: 'UserCount', width: 100, sortable: true },
           { field: 'UserName', title: 'UserName', width: 100 },
           { field: 'JobNum', title: 'JobNum', width: 100 },
           {
               field: 'UserNsex', title: 'UserNsex', width: 100, formatter: function (value, row, index) {
                   if (value == true) {
                       return "man"
                   }
                   if (value == false) {
                       return "women"
                   }
               }
           },
           { field: 'Tel', title: 'Tel', width: 100 },
           { field: 'Phone', title: 'Phone', width: 100 },
           { field: 'Fax', title: 'Fax', width: 50 },
           { field: 'Email', title: 'Email', width: 100 },
           { field: 'Postcode', title: 'Postcode', width: 100 },
           { field: 'QQ', title: 'QQ', width: 100 },
           { field: 'Address', title: 'Address', width: 100 },
           { field: 'CreatePersonnel_n', title: 'Create Personnel', width: 120 },
           {
               field: 'CreateDate', title: 'CreateDate', width: 100, formatter: function (value, row, index) {
                   if (value) {
                       if (value.length > 10) {
                           value = value.substr(0, 10)
                           return value;
                       }
                   }
               }
           },
           {
               field: 'CountState', title: 'CountState', width: 100, formatter: function (value, row, index) {
                   if (value == 1) {
                       return "在用"
                   } else if (value == 0) {
                       return "停用"
                   }
               }
           },
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
        onLoadSuccess: function (data) {
            $('#department_people').datagrid('selectRow', line);
        },
        SortName: 'UserCount',
        SortOrder: 'asc',
        toolbar: department_people_toolbar
    });
    //搜索部门人员信息
    $("#department_people_search").unbind("click").bind('click', function () {
        department_people_search();
    });
};
//*******************************************************************部门人员搜索
function department_people_search() {
    var nodes = $('#department_info').tree('getSelected');//获取选中树节点的信息
    var search = $('#search').combobox('getValue');
    var search1 = $('#search1').textbox('getText');
    $('#department_people').datagrid({
        type: 'POST',
        dataType: "json",
        url: "/SystemSettings/GetDepUserList",//接收一般处理程序返回来的json数据                
        queryParams: {
            NodeId:nodes.id,
            search: search,
            key: search1
        }
    });
}



