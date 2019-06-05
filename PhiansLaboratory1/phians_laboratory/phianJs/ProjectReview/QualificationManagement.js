var line_test = 0;
var line_per = 0;
$(function () {
    //授权类型
    $('#NodeType').combobox({
        value: '0',
        data: [
            { 'value': '2', 'text': '编制' },
            { 'value': '0', 'text': '审核' },
            { 'value': '1', 'text': '签发' }
        ],
        onSelect: function() {
            userlist_search();
            userlist_authorize();
        }
    });
    //用户搜索
    $('#search').combobox({
        data: [
            { 'value': 'UserName', 'text': "用户名" },
            { 'value': 'UserCount', 'text': "用户账号" }
            
        ]
    });
    //参数模板加载初始化
    Model_load_init();
    //用户列表
    userlist_Init();
    //授权
    userlist_authorize_Init();

});

/*
*functionName:Model_load_init
*function:模板列表初始化
*Param: 
*author:张慧敏
*date:2018-05-10
*/
function Model_load_init() {
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
        url: '/ProjectReview/LoadReportList',
        type: 'POST',
        queryParams: {
            //search: $("#Parameter_datagrid_search1").combobox("getValue"),
            //key: $("#Parameter_datagrid_search").textbox("getText")
        },
        dataType: "json",
        columns: [[
            { field: 'FileNum', title: '文件编号', width: 100 },
            { field: 'FileName', title: '文件名称', width: 150 },
            {
                field: 'AddDate', title: '添加时间', width: 150, sortable: true
            }
        ]],
        onLoadSuccess: function (data) {
            $('#Parameter_model_datagrid').datagrid('selectRow', 0);

        },
        onSelect: function () {
            //加载已授权
            userlist_authorize();
        },
        sortName: 'AddDate',
        sortOrder: 'asc',

        toolbar: Parameter_model_datagrid_toolbar
    });

}

/*
*functionName:userlist_Init
*function:userlist_Init
*Param: 
*author:张慧敏
*date:2018-05-10
*/
function userlist_Init() {
    $('#userlist_datagrid').datagrid({
        nowrap: false,
        striped: true,
        //rownumbers: true,
        singleSelect: true,
        //border: false,
        fitColumns: true,
        fit: true,
        title: '用户列表',
        pagination: true,
        pageSize: 30,
        pageNumber: 1,
        url: '/ProjectReview/GetAllUserList',
        type: 'POST',
        queryParams: {
            AuthorizationType: $("#NodeType").combobox("getValue"),
            search: $("#search").combobox("getValue"),
            key: $("#search1").textbox("getText")
        },
        dataType: "json",
        columns: [[
            { field: 'id', title: 'id', width: 40},
            { field: 'UserName', title: '用户名', width: 100, sortable: true },
            { field: 'UserCount', title: '用户账号', width: 150 }
        ]],
        onLoadSuccess: function (data) {
            $('#userlist_datagrid').datagrid('selectRow', 0);

        },
        sortName: 'UserName',
        sortOrder: 'asc',
        toolbar: ConditionTemplate_toolbar
    });
    $('#search_info').unbind("click").bind("click", function () {
        userlist_search();
    });
}
/*
*functionName:userlist_search
*function:用户搜索
*Param: 
*author:张慧敏
*date:2018-05-10
*/
function userlist_search() {
    $('#userlist_datagrid').datagrid({
        fitColumns: true,
        //fit: true,
        queryParams: {
            AuthorizationType: $("#NodeType").combobox("getValue"),
            search: $("#search").combobox("getValue"),
            key: $("#search1").textbox("getText")
        },
        sortName: 'UserName',
        sortOrder: 'asc',
        url: '/ProjectReview/GetAllUserList'
    });
}

/*
*functionName:userlist_authorize_Init
*function:用户授权初始化
*Param: 
*author:张慧敏
*date:2018-05-10
*/
function userlist_authorize_Init() {
    //授权参数
    $('#userlist_authorize').datagrid({
        nowrap: false,
        striped: true,
        //rownumbers: true,
        singleSelect: true,
        autoRowHeight: true,
       // border: false,
        fitColumns: true,
        fit: true,
        title: '已授权',
        pagination: true,
        pageSize: 30,
        pageNumber: 1,
        columns: [[
            { field: 'UserName', title: '用户名', width: 100, sortable: true },
            { field: 'UserCount', title: '用户账号', width: 150 }

        ]],
        sortName: 'UserName',
        sortOrder: 'asc',
        onLoadSuccess: function (data) {
            $('#userlist_authorize').datagrid('selectRow', 0);

        },
        type: 'POST',
        dataType: "json"
    });

    var json = {
        "rows": [],
        "total": 0,
        "success": true
    };
    $('#userlist_authorize').datagrid("loadData", json);


    //添加授性能参数
    $('#addUser').unbind("click").bind("click", function () {
        var selectRow = $("#userlist_datagrid").datagrid("getSelected");//获取选中行
        var TemplateID = $("#Parameter_model_datagrid").datagrid('getSelected')
        var AuthorizationType = $("#NodeType").combobox('getValue');
        var UserId = selectRow.UserId
        if (!TemplateID) {
            $.messager.alert('提示', '请选择操作模板！');
            return;
        }
        if (selectRow) {
            $.ajax({
                url: '/ProjectReview/AddQualificationPerson',
                type: 'POST',
                dataType: 'json',
                data: {
                    id: selectRow.ID,
                    TemplateID: TemplateID.ID,
                    UserId:UserId,
                    AuthorizationType: AuthorizationType
                },
                success: function(data) {
                    if (data) {
                        if (data.Success == true) {
                            $.messager.alert('提示', data.Message);
                            $('#userlist_authorize').datagrid('reload'); //重新加载照片列表
                        } else {
                            $.messager.alert('提示', data.Message);
                        }
                    }

                }
            });
        } else {
            $.messager.alert('提示', "请选择操作行！");
        }
       
    });

    //删除测试条件参数
    $('#removeUser').unbind("click").bind("click", function () {
        var selectRow = $("#userlist_authorize").datagrid("getSelected");//获取选中行
        var TemplateID = $("#Parameter_model_datagrid").datagrid('getSelected');
        if (selectRow) {
            $.ajax({
                url: '/ProjectReview/DelQualificationPerson',
                type: 'POST',
                dataType: 'json',
                data: {
                    ID: selectRow.ID,
                    TemplateID: TemplateID.ID,
                    UserId: selectRow.UserId
                },
                success: function(data) {
                    if (data) {
                        if (data.Success == true) {
                            $.messager.alert('提示', data.Message);
                            $('#userlist_authorize').datagrid('reload'); //重新加载照片列表
                        } else {
                            $.messager.alert('提示', data.Message);
                        }
                    }

                }
            });
        } else {
            $.messager.alert('提示', "请选择操作行！");
        }
    });
}

/*
*functionName:userlist_authorize
*function:用户授权初始化
*Param: 
*author:张慧敏
*date:2018-05-10
*/
function userlist_authorize() {
    var select = $('#Parameter_model_datagrid').datagrid("getSelected");
    $('#userlist_authorize').datagrid({
        fitColumns: true,
        //fit: true,
        queryParams: {
            TemplateID:select.ID,
            AuthorizationType: $("#NodeType").combobox("getValue"),
            search: $("#search").combobox("getValue"),
            key: $("#search1").textbox("getText")
        },
        sortName: 'UserName',
        sortOrder: 'asc',
        url: '/ProjectReview/GetQualificationUserList'
    });
}