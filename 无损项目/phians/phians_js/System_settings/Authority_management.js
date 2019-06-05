//页面权限
$(function () {
    var _height = screen.availHeight - (window.outerHeight - window.top.document.body.clientHeight) - 125;

    var tabs_width = screen.width - 182;
    $('#test_layout').layout('panel', 'west').panel('resize', {
        width: 310,
        height: _height
    });
    $('#test_layout').layout('panel', 'east').panel('resize', {
        width: tabs_width - 310,
        height: _height
    });
    $('#department_info_1').panel('resize', {

        height: _height / 2 - 15
    });
    //$('#test_layout').layout('panel', 'south').panel('resize', { height: 30 });
    $('#test_layout').layout('resize');
    //部门信息
    $('#department_info').tree({
        url: "Authority_management.ashx?&cmd=loadtree",
        method: 'post',
        required: true,
        title: '部门',
        fit: true,
        top: 0,
        onSelect: function () {
            view_technicians_info();
        }
    });
    //选中树项目相应加载实验资质详细信息
    function view_technicians_info() {
        var node_on = $('#department_info').tree('getSelected');
        $('#department_people').datagrid({
            url: "Authority_management.ashx?&cmd=load_userlist",
            dataType: "json",
            type: 'POST',
            queryParams: {
                id: node_on.id
            }
        });
        $('#search').combobox({
            data: [
               { 'value': '0', 'text': '用户名' },
               { 'value': '1', 'text': '姓名' }
            ]
        });
        $('#search1').textbox({
            value: '请输入搜索内容 '
        });
    }
    //实验室员工
    $('#department_people').datagrid({
        nowrap: false,
        striped: true,
        //rownumbers: true,
        ctrlSelect: true,
        //singleSelect: true,
        //autoRowHeight: true,
        //border: false,
        fitColumns: true,
        //fit: true,
        title: '员工',
        pagination: true,
        pageSize: 50,
        pageNumber: 1,
        type: 'POST',
        dataType: "json",
        height: _height / 2,
        //url: "../mainform/technicians_info_management.ashx?&cmd=load" ,//接收一般处理程序返回来的json数据  
        columns: [[
           { field: 'User_count', title: '用户' },
           { field: 'User_name', title: '姓名' }
          
        ]],
        toolbar: department_people_toolbar,
        onClickRow: function (index, row) {
            //$('#department_people_info').form('reset');
            var selectRow_c2 = $("#department_people").datagrid("getSelected");
            if (selectRow_c2) {
                $.ajax({
                    type: 'POST',
                    dataType: "json",
                    url: "Authority_management.ashx?&cmd=load_data",
                    data: {
                        User_count: selectRow_c2.User_count,
                    },
                    success: function (data) {
                        //显示已有权限
                        view_Permissions(data);
                    }
                });
            }
        }
    });

    $('#search').combobox({
        data: [
             { 'value': '0', 'text': '用户名' },
             { 'value': '1', 'text': '姓名' }
        ]
    });
    $('#search1').textbox({
        value: '请输入搜索内容 '
    });

    //搜索
    $('#search_people').unbind('click').bind('click', function () {
        //var node_on = $('#department_info').tree('getSelected');
        var search = $('#search').combobox('getText');
        var search1 = $('#search1').combobox('getText');
        switch (search) {
            case "用户名": search = "User_count"; break;
            case "姓名": search = "User_name"; break;
            default: search = "";
        }

        $('#department_people').datagrid(
            {
                type: 'POST',
                dataType: "json",
                url: "Authority_management.ashx?&cmd=search_people",//接收一般处理程序返回来的json数据                
                queryParams: {
                    //id: node_on.id,
                    search: search,
                    search1: search1
                }
            });
    });

    //显示用户已有权限
    function view_Permissions(data) {
        //转成json对象
        var obj = eval(data);
        //读取json对象数据
        $('#Permissions_1').form('reset');//重新加载一下form
        //恢复为0
        for (var i = 0; i < obj.total; i++) {
            $("#page" + i).val("0");
        }

        var fid_num1 = new Array(51, 53, 11, 12, 13, 48, 101, 102,//40-49
                            103, 104, 108, 109, 110, 111, 105, 106, 113, 114, 115);//fid数组

        for (var i = 0; i < obj.total; i++) {
            for(var y = 0; y < fid_num1.length; y++){
                if (obj.rows[i].fid == fid_num1[y]) {
                    $("#page" + y).prop("checked", "checked"); $("#page" + y).val("1"); break;
                }
            }
        }
    }

    //确定修改148
    $('#Permissions_submit').unbind('click').bind('click', function () {
        var selectRow_c2 = $("#department_people").datagrid("getSelected");
        if (selectRow_c2) {

            var Web_value = new Array();//定义
            var fid_num = new Array(51, 53, 11, 12, 13, 48, 101, 102,//40-49
                            103, 104, 108, 109, 110, 111, 105, 106, 113, 114,115);//fid数组

            var p = 0;//"#page" + p 授权页面checkbox id
            for (var j = 22; j < (fid_num.length * 2 + 22) ; j = j + 2) {
                Web_value[j] = fid_num[p];//页面id（fid_num）
                Web_value[j + 1] = $("#page" + p).val();//页面checkbox id2
                p++;
            }
            //导航显示，子项没有，导航的根目录不显示
            if ($("#page0").val() != 1 && $("#page1").val() != 1) {
                
                Web_value[0] = 50;//导航的根目录 fid
                Web_value[1] = 0;//导航的根目录是否显示 0 不显示 1显示

            } else {

                Web_value[0] = 50;
                Web_value[1] = 1;
            }
            //判断系统设置的title
            if ($("#page2").val() != 1 && $("#page3").val() != 1 && $("#page4").val() != 1 && $("#page5").val() != 1 && $("#page17").val() != 1 && $("#page18").val() != 1) {

                Web_value[2] = 1;
                Web_value[3] = 0;

            } else {

                Web_value[2] = 1;
                Web_value[3] = 1;
            }
            //判断无损报告管理的page
            if ($("#page6").val() != 1 && $("#page7").val() != 1 && $("#page8").val() != 1 && $("#page9").val() != 1 && $("#page10").val() != 1 && $("#page11").val() != 1 && $("#page12").val() != 1 && $("#page13").val() != 1 && $("#page14").val() != 1 && $("#page15").val() != 1 && $("#page16").val() != 1) {

                Web_value[20] = 100;
                Web_value[21] = 0;

            } else {

                Web_value[20] = 100;
                Web_value[21] = 1;
            }


            var Web_values = "";
            for (var i = 0; i < Web_value.length; i++) {
                if (i == 0) {
                    Web_values = Web_value[i];
                }
                if (i > 0) {
                    Web_values = Web_values + ";" + Web_value[i];
                }
            }
            $.ajax({
                type: 'POST',
                dataType: "text",
                url: "Authority_management.ashx?&cmd=Permissions",
                data: {
                    Web_value: Web_values,
                    User_count: selectRow_c2.User_count,
                    User_name: selectRow_c2.User_name
                },
                success: function (data) {
                    //alert(data);
                    if (data = "T") {
                        $.messager.alert('提示', '设置权限成功');
                    }
                }
            });
        } else {
            $.messager.alert('提示', '请选择要操作的用户！');
        }

    });

    //*******************************************************************************************
    //******************************************全选
    //*******************************************************************************************

    //个人中心
    var flag = "true";
    $('#Personal_choose').unbind('click').bind('click', function () {
        if (flag == "true") {
            $("#page0").prop("checked", "checked"); $("#page0").val("1");
            $("#page1").prop("checked", "checked"); $("#page1").val("1");
            flag = "false"
        } else {
            $("#page0").prop("checked", false); $("#page0").val("0");
            $("#page1").prop("checked", false); $("#page1").val("0");
            flag = "true"
        }
        //for (var a = 0; a < 2; a++) {
        //    $("#page" + i).prop("checked", "checked"); $("#page" + i).val("1");
        //}
     
    })
    //系统设置
    var flag_System = "true";
    $('#System_settings_choose').unbind('click').bind('click', function () {
        if (flag_System == "true") {
            $("#page2").prop("checked", "checked"); $("#page2").val("1");
            $("#page3").prop("checked", "checked"); $("#page3").val("1");
            $("#page4").prop("checked", "checked"); $("#page4").val("1");
            $("#page5").prop("checked", "checked"); $("#page5").val("1");
            $("#page17").prop("checked", "checked"); $("#page17").val("1");
            $("#page18").prop("checked", "checked"); $("#page18").val("1");
            flag_System = "false"
        } else {
            $("#page2").prop("checked", false); $("#page2").val("0");
            $("#page3").prop("checked", false); $("#page3").val("0");
            $("#page4").prop("checked", false); $("#page4").val("0");
            $("#page5").prop("checked", false); $("#page5").val("0");
            $("#page17").prop("checked", false); $("#page17").val("0");
            $("#page18").prop("checked", false); $("#page18").val("0");
            flag_System = "true"
        }
        //for (var a = 2; a < 6; a++) {
        //    $("#page" + i).prop("checked", "checked"); $("#page" + i).val("1");
        //}
    

    })

    //无损管理
    var flag_Lossless = "true";
    $('#Lossless_report_choose').unbind('click').bind('click',function () {
        if (flag_Lossless == "true") {
            $("#page6").prop("checked", "checked"); $("#page6").val("1");
            $("#page7").prop("checked", "checked"); $("#page7").val("1");
            $("#page8").prop("checked", "checked"); $("#page8").val("1");
            $("#page9").prop("checked", "checked"); $("#page9").val("1");
            $("#page10").prop("checked", "checked"); $("#page10").val("1");
            $("#page11").prop("checked", "checked"); $("#page11").val("1");
            $("#page12").prop("checked", "checked"); $("#page12").val("1");
            $("#page13").prop("checked", "checked"); $("#page13").val("1");
            $("#page14").prop("checked", "checked"); $("#page14").val("1");
            $("#page15").prop("checked", "checked"); $("#page15").val("1");
            $("#page16").prop("checked", "checked"); $("#page16").val("1");

            flag_Lossless = "false"
        } else {
            $("#page6").prop("checked", false); $("#page6").val("0");
            $("#page7").prop("checked", false); $("#page7").val("0");
            $("#page8").prop("checked", false); $("#page8").val("0");
            $("#page9").prop("checked", false); $("#page9").val("0");
            $("#page10").prop("checked", false); $("#page10").val("0");
            $("#page11").prop("checked", false); $("#page11").val("0");
            $("#page12").prop("checked", false); $("#page12").val("0");
            $("#page13").prop("checked", false); $("#page13").val("0");
            $("#page14").prop("checked", false); $("#page14").val("0");
            $("#page15").prop("checked", false); $("#page15").val("0");
            $("#page16").prop("checked", false); $("#page16").val("0");
            flag_Lossless = "true"
        }
        //for (var a = 6; a < 10; a++) {
        //    $("#page" + i).prop("checked", "checked"); $("#page" + i).val("1");
        //}

    });
});
//checkbox定义
$(function () {

    //checkbox设置
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    //************************************************************************************
    //******************************      页面权限    ************************************
    //************************************************************************************
    var flag_ = "true";
    $(".tog_click").unbind('click').bind('click', function () {
        var index = $(".tog_click").index(this);
        var id = $(".tog_click").eq(index).attr("id");
        //alert($(".tog_click").eq(index).attr("id"));
            if (flag_ == "true") {
                $('#' + id).prop("checked", "checked");
                $('#' + id).val("1");
                flag_ = "false";
            } else {
                $('#' + id).prop("checked", false);
                $('#' + id).val("0");
                /// alert($("#type" + index).val());
                flag_ = "true";
            }
    });

})