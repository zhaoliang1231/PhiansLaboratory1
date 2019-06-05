//页面权限
$(function () {
    var _height = screen.availHeight - (window.outerHeight - window.top.document.body.clientHeight) - 125;

    var tabs_width = screen.width - 182;
    $('#test_layout').layout('panel', 'west').panel('resize', {
        width: 310,
        height: _height
    });
    //$('#test_layout').layout('panel', 'east').panel('resize', {
    //    width: tabs_width - 310,
    //    height: _height
    //});
    $('#department_info_1').panel('resize', {

        height: _height / 2 - 15
    });
    $('#department_').combobox({
        url: "LosslessPerson_Management.aspx?cmd=show_department",
        valueField: 'Project_value',
        textField: 'Project_name',
        required: true,

        //本地联系人数据模糊索引
        filter: function (q, row) {
            var opts = $(this).combobox('options');
            return row[opts.textField].indexOf(q) >= 0;
        }

    });
    //默认添加多个用户名逗号隔开
    $("#Text12").textbox({
        prompt: '多个用户名逗号隔开'
    });
    $("#Text13").textbox({
        prompt: '多个姓名逗号隔开'
    });
    //$('#test_layout').layout('panel', 'south').panel('resize', { height: 30 });
    $('#test_layout').layout('resize');
    //部门信息
    $('#department_info').tree({
        url: "LosslessPerson_Management.aspx?&cmd=loadtree",
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
            url: "LosslessPerson_Management.aspx?&cmd=load_userlist",
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
             { field: 'User_name', title: '姓名' },
             { field: 'User_sex', title: '性别' },
             { field: 'User_job', title: '职务' },
             { field: 'Phone', title: '手机' },
             { field: 'QQ', title: 'QQ' },
             { field: 'User_count_state', title: '用户状态' }
        ]],
        toolbar: department_people_toolbar,
        onClickRow: function (index, row) {
            //$('#department_people_info').form('reset');
            var selectRow_c2 = $("#department_people").datagrid("getSelected");
            if (selectRow_c2) {
                $.ajax({
                    type: 'POST',
                    dataType: "json",
                    url: "LosslessPerson_Management.aspx?&cmd=load_data",
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
        prompt: "请输入搜索内容"
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
                url: "LosslessPerson_Management.aspx?&cmd=search_people",//接收一般处理程序返回来的json数据                
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

        var fid_num1 = new Array(51, 53, 11, 12, 13, 48, 31, 32, 33, 34,//0-9
                            16, 85, 81, 93, 80, 18, 36, 14, 15, 20,//10-19
                            94, 97, 21, 22, 63, 86, 95, 96, 76, 77,//20-29
                            78, 79, 82, 87, 88, 89, 90, 98, 99, 46,//30-39
                            47, 49, 83, 37, 38, 26, 29, 30, 101, 102,//40-49
                            103, 104, 105, 106, 107, 108, 109, 110, 111, 112);//fid数组

        for (var i = 0; i < obj.total; i++) {
            for (var y = 0; y < fid_num1.length; y++) {
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
            var fid_num = new Array(
                        51, 53, 11, 12, 13, 48, 31, 32, 33, 34,
                        16, 85, 81, 93, 80, 18, 36, 14, 15, 20,
                        94, 97, 21, 22, 63, 86, 95, 96, 76, 77,
                        78, 79, 82, 87, 88, 89, 90, 98, 99, 46,
                        47, 49, 83, 37, 38, 26, 29, 30,
                        101, 102, 103, 104, 105, 106, 107, 108, 109, 110, 111, 112);//fid数组·

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
            if ($("#page2").val() != 1 && $("#page3").val() != 1 && $("#page4").val() != 1 && $("#page5").val() != 1) {

                Web_value[2] = 1;
                Web_value[3] = 0;

            } else {

                Web_value[2] = 1;
                Web_value[3] = 1;
            }
            //判断质量管理的title
            if ($("#page6").val() != 1 && $("#page7").val() != 1 && $("#page8").val() != 1 && $("#page9").val() != 1) {

                Web_value[4] = 6;
                Web_value[5] = 0;

            } else {

                Web_value[4] = 6;
                Web_value[5] = 1;
            }
            //判断技术管理的title
            if ($("#page10").val() != 1 && $("#page11").val() != 1 && $("#page12").val() != 1 && $("#page13").val() != 1 && $("#page14").val() != 1 && $("#page15").val() != 1 && $("#page16").val()) {

                Web_value[6] = 7;
                Web_value[7] = 0;

            } else {

                Web_value[6] = 7;
                Web_value[7] = 1;
            }
            //判断委托管理的title
            if ($("#page17").val() != 1 && $("#page18").val() != 1 && $("#page19").val() != 1 && $("#page20").val() != 1 && $("#page21").val() != 1 && $("#page22").val() != 1 && $("#page23").val() != 1 && $("#page24").val() != 1 && $("#page25").val() != 1 && $("#page26").val() != 1 && $("#page27").val() != 1) {

                Web_value[8] = 2;
                Web_value[9] = 0;

            } else {

                Web_value[8] = 2;
                Web_value[9] = 1;
            }
            //判断报告管理的title
            if ($("#page54").val() != 1 && $("#page28").val() != 1 && $("#page29").val() != 1 && $("#page30").val() != 1 && $("#page31").val() != 1 && $("#page32").val() != 1 && $("#page33").val() != 1 && $("#page34").val() != 1 && $("#page35").val() != 1 && $("#page36").val() != 1 && $("#page37").val() != 1 && $("#page38").val() != 1) {

                Web_value[10] = 75;
                Web_value[11] = 0;

            } else {

                Web_value[10] = 75;
                Web_value[11] = 1;
            }
            //判断样品管理的title
            if ($("#page39").val() != 1 && $("#page40").val() != 1 && $("#page41").val() != 1 && $("#page42").val() != 1) {

                Web_value[12] = 9;
                Web_value[13] = 0;

            } else {

                Web_value[12] = 9;
                Web_value[13] = 1;
            }

            //判断台账管理的title
            if ($("#page43").val() != 1 && $("#page44").val() != 1) {

                Web_value[14] = 8;
                Web_value[15] = 0;

            } else {

                Web_value[14] = 8;
                Web_value[15] = 1;
            }
            //判断业务监控的title
            if ($("#page45").val() != 1) {

                Web_value[16] = 4;
                Web_value[17] = 0;

            } else {

                Web_value[16] = 4;
                Web_value[17] = 1;
            }

            //判断统计管理的page
            if ($("#page46").val() != 1 && $("#page47").val() != 1) {

                Web_value[18] = 5;
                Web_value[19] = 0;

            } else {

                Web_value[18] = 5;
                Web_value[19] = 1;
            }
            //判断无损报告管理的page
            if ($("#page48").val() != 1 && $("#page49").val() != 1 && $("#page50").val() != 1 && $("#page51").val() != 1 && $("#page52").val() != 1 && $("#page53").val() != 1 && $("#page55").val() != 1 && $("#page56").val() != 1 && $("#page57").val() != 1 && $("#page58").val() != 1 && $("#page59").val() != 1) {

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
                url: "LosslessPerson_Management.aspx?&cmd=Permissions",
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
            flag_System = "false"
        } else {
            $("#page2").prop("checked", false); $("#page2").val("0");
            $("#page3").prop("checked", false); $("#page3").val("0");
            $("#page4").prop("checked", false); $("#page4").val("0");
            $("#page5").prop("checked", false); $("#page5").val("0");
            flag_System = "true"
        }
        //for (var a = 2; a < 6; a++) {
        //    $("#page" + i).prop("checked", "checked"); $("#page" + i).val("1");
        //}


    })

    //质量管理
    var flag_Quality = "true";
    $('#Quality_management_choose').unbind('click').bind('click', function () {
        if (flag_Quality == "true") {
            $("#page6").prop("checked", "checked"); $("#page6").val("1");
            $("#page7").prop("checked", "checked"); $("#page7").val("1");
            $("#page8").prop("checked", "checked"); $("#page8").val("1");
            $("#page9").prop("checked", "checked"); $("#page9").val("1");
            flag_Quality = "false"
        } else {
            $("#page6").prop("checked", false); $("#page6").val("0");
            $("#page7").prop("checked", false); $("#page7").val("0");
            $("#page8").prop("checked", false); $("#page8").val("0");
            $("#page9").prop("checked", false); $("#page9").val("0");
            flag_Quality = "true"
        }
        //for (var a = 6; a < 10; a++) {
        //    $("#page" + i).prop("checked", "checked"); $("#page" + i).val("1");
        //}

    })
    //技术管理
    var flag_Technology = "true";
    $('#Technology_choose').unbind('click').bind('click', function () {
        if (flag_Technology == "true") {
            $("#page10").prop("checked", "checked"); $("#page10").val("1");
            $("#page11").prop("checked", "checked"); $("#page11").val("1");
            $("#page12").prop("checked", "checked"); $("#page12").val("1");
            $("#page13").prop("checked", "checked"); $("#page13").val("1");
            $("#page14").prop("checked", "checked"); $("#page14").val("1");
            $("#page15").prop("checked", "checked"); $("#page15").val("1");
            $("#page16").prop("checked", "checked"); $("#page16").val("1");
            flag_Technology = "false"
        } else {
            $("#page10").prop("checked", false); $("#page10").val("0");
            $("#page11").prop("checked", false); $("#page11").val("0");
            $("#page12").prop("checked", false); $("#page12").val("0");
            $("#page13").prop("checked", false); $("#page13").val("0");
            $("#page14").prop("checked", false); $("#page14").val("0");
            $("#page15").prop("checked", false); $("#page15").val("0");
            $("#page16").prop("checked", false); $("#page16").val("0");
            flag_Technology = "true"
        }
        //for (var a = 10; a < 17; a++) {
        //    $("#page" + i).prop("checked", "checked"); $("#page" + i).val("1");
        //}

    })
    //委托管理
    var flag_Commissioned = "true";
    $('#Commissioned_managements_choose').unbind('click').bind('click', function () {
        if (flag_Commissioned == "true") {
            $("#page17").prop("checked", "checked"); $("#page17").val("1");
            $("#page18").prop("checked", "checked"); $("#page18").val("1");
            $("#page19").prop("checked", "checked"); $("#page19").val("1");
            $("#page20").prop("checked", "checked"); $("#page20").val("1");
            $("#page21").prop("checked", "checked"); $("#page21").val("1");
            $("#page22").prop("checked", "checked"); $("#page22").val("1");
            $("#page23").prop("checked", "checked"); $("#page23").val("1");
            $("#page24").prop("checked", "checked"); $("#page24").val("1");
            $("#page25").prop("checked", "checked"); $("#page25").val("1");
            $("#page26").prop("checked", "checked"); $("#page26").val("1");
            $("#page27").prop("checked", "checked"); $("#page27").val("1");
            flag_Commissioned = "false"
        } else {
            $("#page17").prop("checked", false); $("#page17").val("0");
            $("#page18").prop("checked", false); $("#page18").val("0");
            $("#page19").prop("checked", false); $("#page19").val("0");
            $("#page20").prop("checked", false); $("#page20").val("0");
            $("#page21").prop("checked", false); $("#page21").val("0");
            $("#page22").prop("checked", false); $("#page22").val("0");
            $("#page23").prop("checked", false); $("#page23").val("0");
            $("#page24").prop("checked", false); $("#page24").val("0");
            $("#page25").prop("checked", false); $("#page25").val("0");
            $("#page26").prop("checked", false); $("#page26").val("0");
            $("#page27").prop("checked", false); $("#page27").val("0");
            flag_Commissioned = "true"
        }
        //for (var a = 17; a < 28; a++) {
        //    $("#page" + i).prop("checked", "checked"); $("#page" + i).val("1");
        //}

    })
    //个人中心
    var flagReport = "true";
    //报告管理
    $('#Report_managements_choose').unbind('click').bind('click', function () {
        if (flagReport == "true") {
            $("#page28").prop("checked", "checked"); $("#page28").val("1");
            $("#page29").prop("checked", "checked"); $("#page29").val("1");
            $("#page30").prop("checked", "checked"); $("#page30").val("1");
            $("#page31").prop("checked", "checked"); $("#page31").val("1");
            $("#page32").prop("checked", "checked"); $("#page32").val("1");
            $("#page33").prop("checked", "checked"); $("#page33").val("1");
            $("#page34").prop("checked", "checked"); $("#page34").val("1");
            $("#page35").prop("checked", "checked"); $("#page35").val("1");
            $("#page36").prop("checked", "checked"); $("#page36").val("1");
            $("#page37").prop("checked", "checked"); $("#page37").val("1");
            $("#page38").prop("checked", "checked"); $("#page38").val("1");
            $("#page54").prop("checked", "checked"); $("#page54").val("1");
            flagReport = "false"
        } else {
            $("#page28").prop("checked", false); $("#page28").val("0");
            $("#page29").prop("checked", false); $("#page29").val("0");
            $("#page30").prop("checked", false); $("#page30").val("0");
            $("#page31").prop("checked", false); $("#page31").val("0");
            $("#page32").prop("checked", false); $("#page32").val("0");
            $("#page33").prop("checked", false); $("#page33").val("0");
            $("#page34").prop("checked", false); $("#page34").val("0");
            $("#page35").prop("checked", false); $("#page35").val("0");
            $("#page36").prop("checked", false); $("#page36").val("0");
            $("#page37").prop("checked", false); $("#page37").val("0");
            $("#page38").prop("checked", false); $("#page38").val("0");
            $("#page54").prop("checked", false); $("#page54").val("0");
            flagReport = "true"
        }
        //for (var a = 28; a < 39; a++) {
        //    $("#page" + i).prop("checked", "checked"); $("#page" + i).val("1");
        //}

    })
    //样品管理
    var flag_Sample = "true";
    $('#Sample_choose').unbind('click').bind('click', function () {
        if (flag_Sample == "true") {
            $("#page39").prop("checked", "checked"); $("#page39").val("1");
            $("#page40").prop("checked", "checked"); $("#page40").val("1");
            $("#page41").prop("checked", "checked"); $("#page41").val("1");
            $("#page42").prop("checked", "checked"); $("#page42").val("1");
            flag_Sample = "false"
        } else {
            $("#page39").prop("checked", false); $("#page39").val("0");
            $("#page40").prop("checked", false); $("#page40").val("0");
            $("#page41").prop("checked", false); $("#page41").val("0");
            $("#page42").prop("checked", false); $("#page42").val("0");
            flag_Sample = "true"
        }
        //for (var a = 39; a < 43; a++) {
        //    $("#page" + i).prop("checked", "checked"); $("#page" + i).val("1");
        //}

    })
    //台账管理
    var flag_Equipment = "true";
    $('#Equipment_choose').unbind('click').bind('click', function () {
        if (flag_Equipment == "true") {
            $("#page43").prop("checked", "checked"); $("#page43").val("1");
            $("#page44").prop("checked", "checked"); $("#page44").val("1");
            flag_Equipment = "false"
        } else {
            $("#page43").prop("checked", false); $("#page43").val("0");
            $("#page44").prop("checked", false); $("#page44").val("0");
            flag_Equipment = "true"
        }
        //for (var a = 43; a < 45; a++) {
        //    $("#page" + i).prop("checked", "checked"); $("#page" + i).val("1");
        //}

    })
    var flag_Monitoring = "true";
    //业务监控
    $('#Monitoring_choose').unbind('click').bind('click', function () {
        if (flag_Monitoring == "true") {
            $("#page45").prop("checked", "checked"); $("#page45").val("1");
            flag_Monitoring = "false"
        } else {
            $("#page45").prop("checked", false); $("#page45").val("0");
            flag_Monitoring = "true"
        }
        //for (var a = 45; a < 46; a++) {
        //    $("#page" + i).prop("checked", "checked"); $("#page" + i).val("1");
        //}


    })
    //统计管理
    var flag_Statistical = "true";
    $('#Statistical_choose').unbind('click').bind('click', function () {
        if (flag_Statistical == "true") {
            $("#page46").prop("checked", "checked"); $("#page46").val("1");
            $("#page47").prop("checked", "checked"); $("#page47").val("1");
            flag_Statistical = "false"
        } else {
            $("#page46").prop("checked", false); $("#page46").val("0");
            $("#page47").prop("checked", false); $("#page47").val("0");
            flag_Statistical = "true"
        }
        //for (var a = 46; a < 48; a++) {
        //    $("#page" + i).prop("checked", "checked"); $("#page" + i).val("1");
        //}


    })
    var flag_Lossless = "true";
    //无损报告管理
    $('#Lossless_report_choose').unbind('click').bind('click', function () {
        if (flag_Lossless == "true") {
            $("#page48").prop("checked", "checked"); $("#page48").val("1");
            $("#page49").prop("checked", "checked"); $("#page49").val("1");
            $("#page50").prop("checked", "checked"); $("#page50").val("1");
            $("#page51").prop("checked", "checked"); $("#page51").val("1");
            $("#page52").prop("checked", "checked"); $("#page52").val("1");
            $("#page53").prop("checked", "checked"); $("#page53").val("1");

            $("#page55").prop("checked", "checked"); $("#page55").val("1");
            $("#page56").prop("checked", "checked"); $("#page56").val("1");
            $("#page57").prop("checked", "checked"); $("#page57").val("1");
            $("#page58").prop("checked", "checked"); $("#page58").val("1");
            $("#page59").prop("checked", "checked"); $("#page59").val("1");
            flag_Lossless = "false"
        } else {
            $("#page48").prop("checked", false); $("#page48").val("0");
            $("#page49").prop("checked", false); $("#page49").val("0");
            $("#page50").prop("checked", false); $("#page50").val("0");
            $("#page51").prop("checked", false); $("#page51").val("0");
            $("#page52").prop("checked", false); $("#page52").val("0");
            $("#page53").prop("checked", false); $("#page53").val("0");

            $("#page55").prop("checked", false); $("#page55").val("0");
            $("#page56").prop("checked", false); $("#page56").val("0");
            $("#page57").prop("checked", false); $("#page57").val("0");
            $("#page58").prop("checked", false); $("#page58").val("0");
            $("#page59").prop("checked", false); $("#page59").val("0");
            flag_Lossless = "true"
        }


    })
});
//按钮详细权限
$(function () {
    //详细权限
    $('#more_Permissions').unbind('click').bind('click', function () {
        var selectRow_c2 = $("#department_people").datagrid("getSelected");
        if (selectRow_c2) {
            $('#dialog_Permissions').form('reset');//重新加载一下form
            $('#dialog_Permissions').dialog({
                width: 600,
                height: 500,
                modal: true,
                title: '修改权限',
                draggable: true,
                buttons: [{
                    text: '确定修改',
                    iconCls: 'icon-ok',
                    id: 'submit'
                }, {
                    text: '取消',
                    iconCls: 'icon-cancel',
                    handler: function () {
                        $('#dialog_Permissions').dialog('close');
                    }
                }]
            }).dialog('close');
            //打开对话框
            $('#dialog_Permissions').dialog('open');

            //加载页面信息树
            load_page_tree();
            //提交信息
            $('#submit').unbind('click').bind('click', function () {
                //提交新权限
                submit_more_Permissions();
            });
        } else {
            $.messager.alert('提示', '请选择要操作的用户！');
        }

    });
    //加载页面信息树
    function load_page_tree() {
        //页面信息
        $('#page_info').tree({
            url: "LosslessPerson_Management.aspx?&cmd=load_page_tree",
            method: 'post',
            required: true,
            title: '页面',
            top: 0,
            onSelect: function () {
                //加载已经分配权限
                load_Permissions();
            }
        });
    }
    //加载已经分配权限
    function load_Permissions() {
        //$('#department_people_info').form('reset');
        var selectRow_c2 = $("#department_people").datagrid("getSelected");
        if (selectRow_c2) {
            $.ajax({
                type: 'POST',
                dataType: "json",
                url: "LosslessPerson_Management.aspx?&cmd=load_operate_Permissions_data",
                data: {
                    User_count: selectRow_c2.User_count,
                },
                success: function (data) {
                    //显示已有详细权限
                    view_operate_Permissions(data);
                }
            });
        }
        //显示已有详细权限
        function view_operate_Permissions(data) {
            var node_on = $('#page_info').tree('getSelected');
            if (node_on) {
                //转成json对象
                var obj = eval(data);
                //读取json对象数据
                $('#dialog_Permissions').form('reset');//重新加载一下form
                //恢复为0
                $("#read_content").val("0");
                $("#create_content").val("0");
                $("#edit_content").val("0");
                $("#delete_content").val("0");
                $("#export_content").val("0");
                $("#print_content").val("0");
                $("#collate_content").val("0");
                $("#review_content").val("0");
                $("#issue_content").val("0");
                $("#create_plan").val("0");
                $("#cancel_plan").val("0");
                $("#Equipment_operation").val("0");

                //alert($("#supplier_info").val());
                for (var i = 0; i < obj.total; i++) {
                    if (obj.rows[i].fid == node_on.id) {
                        //阅读
                        if (obj.rows[i].read_content == "True") {
                            $("#read_content").prop("checked", "checked"); $("#read_content").val("1");
                        } else {
                            $("#read_content").prop("checked", false); $("#read_content").val("0");
                        }
                        //创建内容
                        if (obj.rows[i].create_content == "True") {
                            $("#create_content").prop("checked", "checked"); $("#create_content").val("1");
                        } else {
                            $("#create_content").prop("checked", false); $("#create_content").val("0");
                        }
                        //编辑
                        if (obj.rows[i].edit_content == "True") {
                            $("#edit_content").prop("checked", "checked"); $("#edit_content").val("1");
                        } else {
                            $("#edit_content").prop("checked", false); $("#edit_content").val("0");
                        }
                        //删除
                        if (obj.rows[i].delete_content == "True") {
                            $("#delete_content").prop("checked", "checked"); $("#delete_content").val("1");
                        } else {
                            $("#delete_content").prop("checked", false); $("#delete_content").val("0");
                        }
                        //导出
                        if (obj.rows[i].export_content == "True") {
                            $("#export_content").prop("checked", "checked"); $("#export_content").val("1");
                        } else {
                            $("#export_content").prop("checked", false); $("#export_content").val("0");
                        }
                        //打印
                        if (obj.rows[i].print_content == "True") {
                            $("#print_content").prop("checked", "checked"); $("#print_content").val("1");
                        } else {
                            $("#print_content").prop("checked", false); $("#print_content").val("0");
                        }
                        //校对
                        if (obj.rows[i].collate_content == "True") {
                            $("#collate_content").prop("checked", "checked"); $("#collate_content").val("1");
                        } else {
                            $("#collate_content").prop("checked", false); $("#collate_content").val("0");
                        }
                        //审核
                        if (obj.rows[i].review_content == "True") {
                            $("#review_content").prop("checked", "checked"); $("#review_content").val("1");
                        } else {
                            $("#review_content").prop("checked", false); $("#review_content").val("0");
                        }
                        //签发
                        if (obj.rows[i].issue_content == "True") {
                            $("#issue_content").prop("checked", "checked"); $("#issue_content").val("1");
                        } else {
                            $("#issue_content").prop("checked", false); $("#issue_content").val("0");
                        }
                        //创建计划
                        if (obj.rows[i].create_plan == "True") {
                            $("#create_plan").prop("checked", "checked"); $("#create_plan").val("1");
                        } else {
                            $("#create_plan").prop("checked", false); $("#create_plan").val("0");
                        }
                        //取消创建计划
                        if (obj.rows[i].cancel_plan == "True") {
                            $("#cancel_plan").prop("checked", "checked"); $("#cancel_plan").val("1");
                        } else {
                            $("#cancel_plan").prop("checked", false); $("#cancel_plan").val("0");
                        }
                        //设备操作（启用、停用、作废）
                        if (obj.rows[i].Equipment_operation == "True") {
                            $("#Equipment_operation").prop("checked", "checked"); $("#Equipment_operation").val("1");
                        } else {
                            $("#Equipment_operation").prop("checked", false); $("#Equipment_operation").val("0");
                        }
                    }
                }
            }

        }
    }
    //提交新权限
    function submit_more_Permissions() {
        var node_on = $('#page_info').tree('getSelected');
        var selectRow_c2 = $("#department_people").datagrid("getSelected");
        //******************  十二种权限
        var read_content = $("#read_content").val();
        var create_content = $("#create_content").val();
        var edit_content = $("#edit_content").val();
        var delete_content = $("#delete_content").val();
        var export_content = $("#export_content").val();
        var print_content = $("#print_content").val();
        var collate_content = $("#collate_content").val();
        var review_content = $("#review_content").val();
        var issue_content = $("#issue_content").val();
        var create_plan = $("#create_plan").val();
        var cancel_plan = $("#cancel_plan").val();
        var Equipment_operation = $("#Equipment_operation").val();

        $.ajax({
            type: 'POST',
            dataType: "text",
            url: "LosslessPerson_Management.aspx?&cmd=Detailed_Permissions",
            data: {
                User_count: selectRow_c2.User_count,
                fid: node_on.id,
                read_content: read_content,
                create_content: create_content,
                edit_content: edit_content,
                delete_content: delete_content,
                export_content: export_content,
                print_content: print_content,
                collate_content: collate_content,
                review_content: review_content,
                issue_content: issue_content,
                create_plan: create_plan,
                cancel_plan: cancel_plan,
                Equipment_operation: Equipment_operation
            },
            success: function (data) {
                //alert(data);
                if (data = "T") {
                    $.messager.alert('提示', '设置权限成功');
                }

            }
        });
    }
    //子模块全部选择check
    $('#all_choose').unbind('click').bind('click', function () {
        $("#read_content").prop("checked", "checked"); $("#read_content").val("1");
        $("#create_content").prop("checked", "checked"); $("#create_content").val("1");
        $("#edit_content").prop("checked", "checked"); $("#edit_content").val("1");
        $("#delete_content").prop("checked", "checked"); $("#delete_content").val("1");
        $("#export_content").prop("checked", "checked"); $("#export_content").val("1");
        $("#print_content").prop("checked", "checked"); $("#print_content").val("1");
        $("#collate_content").prop("checked", "checked"); $("#collate_content").val("1");
        $("#review_content").prop("checked", "checked"); $("#review_content").val("1");
        $("#issue_content").prop("checked", "checked"); $("#issue_content").val("1");
        $("#create_plan").prop("checked", "checked"); $("#create_plan").val("1");
        $("#cancel_plan").prop("checked", "checked"); $("#cancel_plan").val("1");
        $("#Equipment_operation").prop("checked", "checked"); $("#Equipment_operation").val("1");
    })
    //子模块全部取消check
    $('#all_cancel').unbind('click').bind('click', function () {
        $("#read_content").prop("checked", false); $("#read_content").val("0");
        $("#create_content").prop("checked", false); $("#create_content").val("0");
        $("#edit_content").prop("checked", false); $("#edit_content").val("0");
        $("#delete_content").prop("checked", false); $("#delete_content").val("0");
        $("#export_content").prop("checked", false); $("#export_content").val("0");
        $("#print_content").prop("checked", false); $("#print_content").val("0");
        $("#collate_content").prop("checked", false); $("#collate_content").val("0");
        $("#review_content").prop("checked", false); $("#review_content").val("0");
        $("#issue_content").prop("checked", false); $("#issue_content").val("0");
        $("#create_plan").prop("checked", false); $("#create_plan").val("0");
        $("#cancel_plan").prop("checked", false); $("#cancel_plan").val("0");
        $("#Equipment_operation").prop("checked", false); $("#Equipment_operation").val("0");
    })
})
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



    //for (var z = 0; z < 48; z++) {
    //    console.log(z);
    //    $("#page" + z).click(function () {

    //        if ($("#page" + z).is(":checked")) {
    //            $("#page" + z).val("1");
    //        } else {
    //            $("#page" + z).val("0");
    //        }
    //    })
    //}


    //************************************************************************************
    //******************************      详细权限    ************************************
    //************************************************************************************

    //阅读
    $('#read_content').unbind('click').bind('click', function () {
        if ($("#read_content").is(":checked")) {
            $("#read_content").val("1");
        } else {
            $("#read_content").val("0");
        }
    });
    //创建内容
    $('#create_content').unbind('click').bind('click', function () {
        if ($("#create_content").is(":checked")) {
            $("#create_content").val("1");
        } else {
            $("#create_content").val("0");
        }
    });
    //编辑
    $('#edit_content').unbind('click').bind('click', function () {
        if ($("#edit_content").is(":checked")) {
            $("#edit_content").val("1");
        } else {
            $("#edit_content").val("0");
        }
    });
    //删除
    $('#delete_content').unbind('click').bind('click', function () {
        if ($("#delete_content").is(":checked")) {
            $("#delete_content").val("1");
        } else {
            $("#delete_content").val("0");
        }
    });
    //导出
    $('#export_content').unbind('click').bind('click', function () {
        if ($("#export_content").is(":checked")) {
            $("#export_content").val("1");
        } else {
            $("#export_content").val("0");
        }
    });
    //打印
    $('#print_content').unbind('click').bind('click', function () {
        if ($("#print_content").is(":checked")) {
            $("#print_content").val("1");
        } else {
            $("#print_content").val("0");
        }
    });
    //校对
    $('#collate_content').unbind('click').bind('click', function () {
        if ($("#collate_content").is(":checked")) {
            $("#collate_content").val("1");
        } else {
            $("#collate_content").val("0");
        }
    });
    //审核
    $('#review_content').unbind('click').bind('click', function () {
        if ($("#review_content").is(":checked")) {
            $("#review_content").val("1");
        } else {
            $("#review_content").val("0");
        }
    });
    //签发
    $('#issue_content').unbind('click').bind('click', function () {
        if ($("#issue_content").is(":checked")) {
            $("#issue_content").val("1");
        } else {
            $("#issue_content").val("0");
        }
    });
    //创建计划
    $('#create_plan').unbind('click').bind('click', function () {
        if ($("#create_plan").is(":checked")) {
            $("#create_plan").val("1");
        } else {
            $("#create_plan").val("0");
        }
    });
    //取消创建计划
    $('#cancel_plan').unbind('click').bind('click', function () {
        if ($("#cancel_plan").is(":checked")) {
            $("#cancel_plan").val("1");
        } else {
            $("#cancel_plan").val("0");
        }
    });
    //设备操作（启用、停止、作废）
    $('#Equipment_operation').unbind('click').bind('click', function () {
        if ($("#Equipment_operation").is(":checked")) {
            $("#Equipment_operation").val("1");
        } else {
            $("#Equipment_operation").val("0");
        }
    });
})
//搜索
$('#department_people_search').unbind('click').bind('click', function () {
    var search = $('#search').combobox('getText');
    var search1 = $('#search1').combobox('getText');
    switch (search) {
        case "用户名": search = "User_count"; break;
        case "姓名": search = "User_name"; break;
        default: search = "";
    }

    $('#department_people').datagrid({
        type: 'POST',
        dataType: "json",
        url: "LosslessPerson_Management.aspx?&cmd=department_people_search",//接收一般处理程序返回来的json数据                
        queryParams: {
            search: search,
            search1: search1
        }

    });

});
//查看未分配组人员
$('#no_department_personnel').unbind('click').bind('click', function () {
    $('#department_people').datagrid({
        type: 'POST',
        dataType: "json",
        url: "LosslessPerson_Management.aspx?&cmd=no_department_personnel",//接收一般处理程序返回来的json数据                
    });

});
//启用
$('#enable_personnel').unbind('click').bind('click', function () {
    var personnel = $("#department_people").datagrid("getSelected");
    if (personnel) {
        $.ajax({
            url: "LosslessPerson_Management.aspx?&cmd=enable_personnel",
            type: 'POST',
            data: {
                id: personnel.id,
                User_count: personnel.User_count
            },
            success: function (data) {
                if (data == 'T') {
                    $('#department_people').datagrid('reload');
                    $.messager.alert('提示', '启用成功！');
                }
            }

        });
    }
    else {

        $.messager.alert('提示', '请选择要操作的员工！');
    }

});
//停用
$('#disable_personnel').unbind('click').bind('click', function () {
    var personnel = $("#department_people").datagrid("getSelected");
    if (personnel) {
        $.messager.confirm('确认', '您确认想要停用这个员工吗？', function (r) {
            if (r) {
                $.ajax({
                    url: "LosslessPerson_Management.aspx?&cmd=disable_personnel",
                    type: 'POST',
                    data: {
                        id: personnel.id,
                        User_count: personnel.User_count
                    },
                    success: function (data) {
                        if (data == 'T') {
                            $('#department_people').datagrid('reload');
                            $.messager.alert('提示', '停用成功！');
                        }
                    }

                });
            }

        });
    }
    else {

        $.messager.alert('提示', '请选择要操作的员工！');
    }

});
//重置密码
$('#Reset_password').unbind('click').bind('click', function () {
    var personnel = $("#department_people").datagrid("getSelected");
    if (personnel) {
        $.messager.confirm('确认', '您确认要重置这个员工的密码吗？', function (r) {
            if (r) {
                $.ajax({
                    url: "LosslessPerson_Management.aspx?&cmd=Reset_password",
                    type: 'POST',
                    data: {
                        id: personnel.id,
                        User_count: personnel.User_count
                    },
                    success: function (data) {
                        if (data == 'T') {
                            $('#department_people').datagrid('reload');
                            $.messager.alert('重置成功', '已重置密码为：123456');
                        }
                    }
                });
            }

        });
    }
    else {

        $.messager.alert('提示', '请选择要操作的员工！');
    }

});
//删除员工
$('#department_people_del').unbind('click').bind('click', function () {
    var node = $('#department_info').tree('getSelected');
    var selectRow = $("#department_people").datagrid("getSelected");
    if (selectRow) {
        $.messager.confirm('确认', '您确认想要删除这个员工吗？', function (r) {
            if (r) {
                var cmd = "del";
                $.ajax({
                    url: "LosslessPerson_Management.aspx?&cmd=technicians_del",
                    type: 'POST',
                    data: {
                        User_count: selectRow.User_count,
                        User_department: node.id
                    },
                    success: function (data) {
                        if (data == 'T') {
                            $('#department_people').datagrid('reload');
                        }
                    }
                });
            }

        });

    }

    else {

        $.messager.alert('提示', '请选择要操作的员工！');
    }

});

//添加员工
$('#department_people_add').unbind('click').bind('click', function () {
    var selectRow = $("#department_people").datagrid("getSelected");
    var node = $('#department_info').tree('getSelected');


    if (node) {
        $('#department_people_info_dialog').dialog({
            width: 500,
            height: 400,
            top: 40,
            modal: true,
            title: '添加员工',
            draggable: true,
            buttons: [{
                text: '确定',
                iconCls: 'icon-ok',
                id: 'department_people_saveadd'
            }, {
                text: '取消',
                iconCls: 'icon-cancel',
                handler: function () {
                    $('#department_people_info_dialog').dialog('close');
                }
            }]
        });
        $('#department').combobox({
            url: "LosslessPerson_Management.aspx?cmd=show_department",
            valueField: 'Project_value',
            textField: 'Project_name',
            required: true,

            //本地联系人数据模糊索引
            filter: function (q, row) {
                var opts = $(this).combobox('options');
                return row[opts.textField].indexOf(q) >= 0;
            }

        });

        $('#department_people_saveadd').unbind('click').bind('click', function () {
            var selecttree = $("#department_info").tree("getSelected");
            if ($("#Text12").textbox('getText') != '') {
                if ($("#Text13").textbox('getText') != '') {
                    if ($("#department").combobox('getValue') != '') {
                        $('#department_people_info_dialog').form('submit', {
                            url: "LosslessPerson_Management.aspx",
                            ajax: true,
                            //额外提交参数
                            onSubmit: function (param) {
                                param.cmd = 'technicians_saveadd';
                                param.User_department = selecttree.id;
                                param.Department_name = selecttree.text;
                                param.department_code = $('#department').combobox('getValue');
                                param.department = $('#department').combobox('getText');

                            },
                            success: function (data) {
                                if (data == 'T') {
                                    $.messager.alert('提示', '添加用户成功！');
                                    $('#department_people').datagrid('reload');
                                    $('#department_people_info_dialog').dialog('close');
                                } else if (data == '请保持用户账号个用户名数量一致') {
                                    $.messager.alert('提示', '请保持用户账号个用户名数量一致！');
                                } else if (data == 'F') {
                                    $.messager.alert('提示', '添加用户失败！');
                                }
                            }

                        });
                    } else {
                        $.messager.alert('提示', '部门不能为空！');
                    }
                } else {
                    $.messager.alert('提示', '姓名不能为空！');
                }
            } else {
                $.messager.alert('提示', '用户名不能为空！');
            }
        });

    } else {
        $.messager.alert('提示', '请选择要操作的部门！');

    }

});
//将员工指派到其它部门
$('#add_others_department').unbind('click').bind('click', function () {
    var selectRow = $("#department_people").datagrid("getSelected");
    if (selectRow) {
        //部门信息显示
        $('#department_info_add').tree({
            url: "LosslessPerson_Management.aspx?&cmd=loadtree",
            method: 'post',
            required: true,
            title: '部门',
            top: 0
        });
        //清空树
        $('#project_room').tree('loadData', []);
        //已加部门加载
        $('#project_room').tree({
            url: "LosslessPerson_Management.aspx?&cmd=load_department",
            method: 'post',
            required: true,
            title: '部门',
            queryParams: {
                User_count: selectRow.User_count
            },
            top: 0
        });
        $('#add_others_department_dialog').dialog({
            width: 550,
            height: 300,
            top: 40,
            modal: true,
            title: '将员工指派到其它部门',
            draggable: true,
            buttons: [{
                text: '确定',
                iconCls: 'icon-ok',
                handler: function () {
                    $('#add_others_department_dialog').dialog('close');
                }
            }]
        });

    } else {
        $.messager.alert('提示', '请选择要操作的员工！');

    }

});

//添加入新部门
$('#tree_add_project').unbind('click').bind('click', function () {
    var selectRow = $("#department_people").datagrid("getSelected");
    var node = $('#department_info_add').tree('getSelected');
    if (selectRow) {
        $.ajax({
            url: "LosslessPerson_Management.aspx?&cmd=tree_add_project",
            type: 'POST',
            data: {
                User_count: selectRow.User_count,
                User_department: node.id,
                Department_name: node.text
            },
            success: function (data) {
                if (data = "T") {
                    $('#project_room').tree('reload');
                    //部门信息
                    //$('#project_room').tree({
                    //    url: "/mainform/technicians_info_management.ashx?&cmd=load_department",
                    //    method: 'post',
                    //    required: true,
                    //    title: '部门',
                    //    queryParams: {
                    //        User_count: selectRow.User_count
                    //    },
                    //    top: 0
                    //});
                }
            }
        });
    } else {
        $.messager.alert('提示', '请选择要加入组！');
    }
})
// 查找已经项目，避免重复添加
function searchTree(parentNode, searchCon) {
    var children;
    var content1 = '';
    var state2 = 0;
    for (var i = 0; i < parentNode.length; i++) { //循环顶级 node 
        if (parentNode[i].text == searchCon) {
            state2 = 1;
            break;
        }
        //content1 = content1 + parentNode[i].text;
    }
    //alert(content1)
    return state2;

}
//移除一个检测项目
$('#tree_remove_project').unbind('click').bind('click', function () {
    var selectRow = $("#department_people").datagrid("getSelected");
    var remove_node = $('#project_room').tree('getSelected');
    if (selectRow) {
        $.ajax({
            url: "LosslessPerson_Management.aspx?&cmd=tree_remove_project",
            type: 'POST',
            data: {
                User_count: selectRow.User_count,
                User_department: remove_node.id
            },
            success: function (data) {
                if (data == "T") {
                    $('#project_room').tree('reload');
                    //部门信息
                    //$('#project_room').tree({
                    //    url: "/mainform/technicians_info_management.ashx?&cmd=load_department",
                    //    method: 'post',
                    //    required: true,
                    //    title: '部门',
                    //    queryParams: {
                    //        User_count: selectRow.User_count
                    //    },
                    //    top: 0
                    //});
                } else if (data == "人员至少存在一个组中") {
                    $.messager.alert('提示', '人员至少存在一个组中！');
                } else if (data == 'F') {
                    $.messager.alert('提示', '移除失败！')
                }
            }
        });
    } else {
        $.messager.alert('提示', '请选择要移除得组！');
    }

});
//部门信息
//$('#department_info_add').tree({
//    url: "../mainform/technicians_info_management.ashx?&cmd=loadtree",
//    method: 'post',
//    required: true,
//    title: '部门',
//    top: 0
//});

//查看人员信息
$('#read_info').unbind('click').bind('click', function () {
    var selectRow = $("#department_people").datagrid("getSelected");
    var rowss = $('#department_people').datagrid('getSelections');
    if (selectRow) {
        $('#department_people_info').form('reset');
        //$('#rg_sites').combobox("setText", rowss[0].rg_sites);
        $('#department_people_info').form('load', rowss[0]);
        $('#department_').combobox('select', rowss[0].department);

        $('#department_people_info').dialog({
            width: 600,
            height: 350,
            top: 40,
            modal: true,
            title: '人员信息',
            draggable: true,
            buttons: [{
                text: '确认修改',
                iconCls: 'icon-ok',
                handler: function () {
                    //更新人员信息
                    updateinfo();
                }
            }, {
                text: '关闭',
                iconCls: 'icon-cancel',
                handler: function () {
                    $('#department_people_info').dialog('close');
                }
            }]
        });

    } else {
        $.messager.alert('提示', '请选择要操作的员工！');

    }

});
////查看签名
$('#read_Signature').unbind('click').bind('click', function () {
    var selectRow = $("#department_people").datagrid("getSelected");
    if (selectRow) {

        var rowss = $('#department_people').datagrid('getSelections');
        //$('#rg_sites').combobox("setText", rowss[0].rg_sites);                   
        $('#upload_view').show();
        $('#upload_org_code_img').show();
        $('#uploadify').show();
        $('#fileQueue').show();
        $("#upload_org_code_img").attr("src", rowss[0].Signature);
        $("#uploadify").uploadify({
            //指定swf文件
            swf: '/uploadify/uploadify.swf',
            //后台处理的页面
            uploader: 'LosslessPerson_Management.aspx?&cmd=technicians_autograph',
            //按钮显示的文字
            buttonText: '修改签名',
            //'cancelImg': '../uploadify/uploadify-cancel.png',
            folder: '/upload_Folder',
            //fileTypeDesc: 'xx',  //过滤掉除了*.jpg,*.gif,*.png的文件
            'fileTypeDesc': 'All Files',
            'fileTypeExts': '*.jpg;*.png;*.gif',
            queueID: 'fileQueue',
            sizeLimit: '2048000',                         //最大允许的文件大小为2M
            auto: true,                                  //需要手动的提交申请
            multi: false,                                //一次只允许上传一张图片
            onUploadStart: function (file) {
                var ids = {}
                var selectRow_c2 = $("#department_people").datagrid("getSelected");
                ids.ids = selectRow_c2.id;
                $("#uploadify").uploadify('settings', 'formData', ids);
            },
            onUploadSuccess: function (file, data, response) {

                $("#upload_org_code_img").attr("src", "/upload_Folder/" + data);

            }
            //,
            //onFallback: function () { //Flash无法加载错误
            //    alert('提示', "您未安装FLASH控件，无法上传！请安装FLASH控件后再试。");
            //},
            //onSelectError: function (file, errorCode) {  //选择文件出错
            //    alert('提示', errorCode);
            //},
            //onUploadError: function (file, errorCode, errorMsg) { //上传失败
            //    alert('提示', file.name + "上传失败，</br>错误信息：" + errorMsg);

            //}
        });
        $('#Signature_form').dialog({
            width: 400,
            height: 350,
            top: 40,
            modal: true,
            title: '签名图片',
            draggable: true,
            buttons: [{
                text: '关闭',
                iconCls: 'icon-cancel',
                handler: function () {
                    $('#Signature_form').dialog('close');
                }
            }]
        });

    } else {
        $.messager.alert('提示', '请选择要操作的员工！');

    }

});
//更新员工
function updateinfo() {
    var selectRow = $("#department_people").datagrid("getSelected");
    if (selectRow) {
        $.messager.confirm('更新信息提示', '您确认想要更新这个员工信息吗？', function (r) {
            if (r) {
                var selectRow = $("#department_people").datagrid("getSelected");
                $('#department_people_info').form('submit', {
                    url: "LosslessPerson_Management.aspx",
                    ajax: true,
                    //额外提交参数
                    onSubmit: function (param) {
                        param.cmd = 'technicians_save';
                        param.ids = selectRow.id;
                        param.department_code = $('#department_').combobox('getValue');
                        param.department = $('#department_').combobox('getText');
                        param.ids = selectRow.id;
                    },
                    //error:function (XMLHttpRequest, textStatus, errorThrown) {
                    //    // 通常情况下textStatus和errorThown只有其中一个有值 
                    //    this; // the options for this ajax request
                    //},
                    success: function (data) {
                        if (data == 'T') {

                            $.messager.alert('提示', '更新信息成功');
                            $('#department_people_info').dialog('close');
                            $('#department_people').datagrid('reload');
                        }
                    }

                });

            }

        });
    } else {
        $.messager.alert('提示', '请选择要操作的员工');
    }
}
//字典列表
$("#add_dictionary").unbind("click").bind("click", function () {
    standards_info();
});
//添加字典
$("#click_add").unbind("click").bind("click", function () {
    insert_form();
});
//修改字典
$("#click_edit").unbind("click").bind("click", function () {
    edit_form();
});
//删除字典
$("#click_del").unbind("click").bind("click", function () {
    delete_task()
});
function standards_info() {

    $('#dictionary_list_dialog').dialog({
        width: 600,
        height: 500,
        modal: true,
        title: '字典内容',
        draggable: true,
        buttons: [{
            text: '取消',
            iconCls: 'icon-cancel',
            handler: function () {
                $('#dictionary_list_dialog').dialog('close');
            }
        }]
    });
    $('#standards_info').datagrid({
        nowrap: false,
        striped: true,
        //rownumbers: true,
        singleSelect: true,
        //autoRowHeight: true,
        //border: false,
        fitColumns: true,
        fit: true,
        pagination: true,
        pageSize: 5,
        pageList: [5, 10, 20, 30],
        pageNumber: 1,
        type: 'POST',
        dataType: "json",

        url: "LosslessPerson_Management.aspx?&cmd=load_info",//接收一般处理程序返回来的json数据      
        columns: [[
           { title: '项目内容', field: 'Project_name' },
           { title: '项目值', field: 'Project_value' },
           { title: '排序', field: 'Sort_num' },
           { title: '说明', field: 'remarks' }
        ]],
        onLoadSuccess: function (data) {
            //默认选择行
            $('#standards_info').datagrid('selectRow', 0);

        },

        rowStyler: function (index, row) {
            if (row.expedited == "是") {
                //return 'background-color:pink;color:blue;font-weight:bold;';
                return 'color:red;';
            }
        },
        sortOrder: 'asc',
        toolbar: "#standards_info_toolbar"
    });


}
///////////*******************添加字典end//////////////////////////

//添加字典内容
function insert_form() {
    $('#dictionary_dialog').dialog({
        width: 380,
        height: 280,
        modal: true,
        title: '添加字典内容',
        draggable: true,
        buttons: [{
            text: '确定',
            iconCls: 'icon-ok',
            id: 'save'
        }, {
            text: '取消',
            iconCls: 'icon-cancel',
            handler: function () {
                $('#dictionary_dialog').dialog('close');
            }
        }]
    });

    $('#dictionary_dialog').form('reset');
    $("#save").unbind('click').bind('click', function () {
        //form表单提交
        if ($("#Project_name").textbox("getText") != '') {
            if ($("#Project_value").textbox("getText") != '') {
                $('#dictionary_dialog').form('submit', {
                    url: "LosslessPerson_Management.aspx",//接收一般处理程序返回来的json数据     
                    onSubmit: function (param) {
                        param.cmd = 'insert_context';
                    },
                    success: function (data) {
                        if (data == 'T') {
                            $('#dictionary_dialog').dialog('close');
                            $('#standards_info').datagrid('reload');
                            $.messager.alert('提示', '添加信息成功');

                        } if (data == "无权操作！") {
                            $.messager.alert('提示', '您没有权限添加资料！');

                        }
                    }
                });
            } else {
                $.messager.alert('提示', '项目值不能为空！');
            }
        } else {
            $.messager.alert('提示', '项目内容不能为空！');
        }

    })
}
function edit_form() {
    //修改

    var selectRow = $("#standards_info").datagrid("getSelected");
    if (selectRow) {
        line = $('#standards_info').datagrid("getRowIndex", selectRow);
        $('#dictionary_dialog').dialog({
            width: 380,
            height: 280,
            modal: true,
            title: '修改字典内容',
            draggable: true,
            buttons: [{
                text: '确定',
                iconCls: 'icon-ok',
                id: 'save1'
            }, {
                text: '取消',
                iconCls: 'icon-cancel',
                handler: function () {
                    $('#dictionary_dialog').dialog('close');
                }
            }]
        });
        //获取选中
        var rowss = $('#standards_info').datagrid('getSelections'); //获取选中数据   
        $('#dictionary_dialog').form('load', rowss[0]);
        $("#save1").unbind('click').bind('click', function () {
            //form表单提交
            if ($("#Project_name").textbox("getText") != '') {
                if ($("#Project_value").textbox("getText") != '') {
                    $('#dictionary_dialog').form('submit', {
                        url: "LosslessPerson_Management.aspx",//接收一般处理程序返回来的json数据     
                        onSubmit: function (param) {
                            param.cmd = 'edit_context';
                            param.id = selectRow.id;
                        },
                        success: function (data) {
                            if (data == 'T') {
                                $('#dictionary_dialog').dialog('close');
                                $.messager.alert('提示', '修改信息成功');
                                $('#standards_info').datagrid('reload');
                            } else if (data == "无权操作！") {
                                $.messager.alert('提示', '您没有权限编辑资料！');
                            }
                            else {
                                $.messager.alert('提示', '修改信息失败');
                            }
                        }
                    });
                } else {
                    $.messager.alert('提示', '项目值不能为空！');
                }
            } else {
                $.messager.alert('提示', '项目内容不能为空！');
            }
        })
    } else {
        $.messager.alert('提示', '请选择要操作的任务！');
    }
}
//删除内容
function delete_task() {
    var selectRow = $("#standards_info").datagrid("getSelected");
    var rowss = $('#standards_info').datagrid('getSelections'); //获取选中数据 
    var ids1 = [];
    for (var i = 0; i < rowss.length; i++) {
        ids1.push(rowss[i].id);
    }
    var ids = ids1.join(';');
    if (selectRow) {
        $.messager.confirm('删除提示', '您确认要删除选中内容吗', function (r) {
            if (!r) {
                return false;
            } else {
                $.ajax({
                    url: "LosslessPerson_Management.aspx?&cmd=del_context",
                    data: { id: ids },
                    type: "POST",
                    success: function (data) {
                        $('#standards_info').datagrid('reload');
                    }
                });
            }
        });
    } else {
        $.messager.alert('提示', '请选择要操作的内容！');
    }
}