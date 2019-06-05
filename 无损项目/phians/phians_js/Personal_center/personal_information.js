$(function () {

    var tabs_width = screen.width - 182;

    //iframe可用高度
    var _height = screen.availHeight - (window.outerHeight - window.top.document.body.clientHeight) - 125;
    //显示分页
     var _height1 = screen.availHeight - (window.outerHeight - window.top.document.body.clientHeight) - 300;
    // alert(_height);
    var num = parseInt(_height1/25);
    $('#department_layout').layout('panel', 'west').panel('resize', {
        //width: tabs_width - 300 - 15,
        width: tabs_width - 600-15,
        height: _height
    });
    $('#department_layout').layout('panel', 'center').panel('resize', {
        width:615,
        height: _height
    });
    //$('#department_layout').layout('panel', 'east').panel('resize', {
    //    width: 315,
    //    height: _height
    //});
    //$('#test_layout').layout('panel', 'south').panel('resize', { height: 30 });
    $('#department_layout').layout('resize');
   
    //个人资料
    $.ajax({
        url: "/mainform/Personal_center/personal_information.ashx?&cmd=load_personnel_info",
        type: 'POST',
        data: {
            //Project_name: node_on.Project_name,
            //User_count: node_on1.User_count,
            //project_num: node_on.id,
            //User_name: node_on1.User_name
        },
        success: function (result) {
            //var personnel_info = new Array(); //定义一数组 
            //personnel_info = data.split(","); //字符分割 
            var data = $.parseJSON(result)
            $('#User_job').textbox('setText', data.User_job);
            $('#User_duties').textbox('setText', data.User_duties);
            $('#Tel').textbox('setText', data.Tel);
            $('#Phone').textbox('setText', data.Phone);
            $('#Fax').textbox('setText', data.Fax);
            $('#Email').textbox('setText', data.Email);
            $('#QQ').textbox('setText', data.QQ);
            $('#MSN').textbox('setText', data.MSN);
            $('#Address').textbox('setText', data.Address);
            $('#Postcode').textbox('setText', data.Postcode);
            $('#Remarks').textbox('setText', data.Remarks);
            $('#Department_name').textbox('setText', data.Department_name);
            $("#upload_org_code_img").attr("src", data.Signature);
            //$('#department_people_info').form('reset');
            //$('#upload_view').show();
            //$('#upload_org_code_img').show();
            //$('#uploadify').show();
            //$('#fileQueue').show(); 
            //$("#upload_org_code_img").attr("src", personnel_info[11]);
            //$("#uploadify").uploadify({
            //    //指定swf文件
            //    swf: '/uploadify/uploadify.swf',
            //    //后台处理的页面
            //    uploader: '/mainform/Personal_center/personal_information.ashx?&cmd=technicians_autograph',
            //    //按钮显示的文字
            //    buttonText: '修改签名',
            //    //'cancelImg': '../uploadify/uploadify-cancel.png',
            //    //folder: '../upload_Folder',
            //    //fileTypeDesc: 'xx',  //过滤掉除了*.jpg,*.gif,*.png的文件
            //    'fileTypeDesc': 'All Files',
            //    'fileTypeExts': '*.jpg;*.png;*.gif',
            //    queueID: 'fileQueue',
            //    sizeLimit: '2048000',                         //最大允许的文件大小为2M
            //    auto: true,                                  //需要手动的提交申请
            //    multi: false,                                //一次只允许上传一张图片
            //    onUploadStart: function (file) {
            //        //var ids = {}
            //        //var selectRow_c2 = $("#department_people").datagrid("getSelected");
            //        //ids.ids = selectRow_c2.id;
            //        //$("#uploadify").uploadify('settings', 'formData', ids);
            //    },
            //    onUploadSuccess: function (file, data, response) {

            //        $("#upload_org_code_img").attr("src",  data);

            //    }
            //});
        }
    });

    $('#save_image').unbind('click').bind('click',function () {
        save_image();
    });
    function save_image() {

        var old_url = $('#upload_org_code_img').val();
        $('#info_form').form('submit', {
            url: "personal_information.ashx",//接收一般处理程序返回来的json数据     
            onSubmit: function (param) {
                param.cmd = 'technicians_autograph';
                param.old_url = old_url;

            },
            success: function (data) {
                if (data == '请上传图片') {
                    $.messager.alert('提示', '请上传图片格式签名！');

                } else if (data == '请选择签名') {
                    $.messager.alert('提示', '请选择签名上传！');
                } else if (data != 'F') {
                    $("#upload_org_code_img").attr("src", data);
                }
                else {

                    $.messager.alert('提示', '添加信息失败');
                }


            }
        });

    }

    //修改资料
    $('#save_personnel_info').unbind('click').bind('click',function () {
        //个人资料
        $.ajax({
            url: "/mainform/Personal_center/personal_information.ashx?&cmd=save_personnel_info",
            type: 'POST',
            data: {
                Phone1: $('#Phone').textbox('getText'),
                Department_name1: $('#Department_name').textbox('getText'),
                User_job1: $('#User_job').textbox('getText'),
                User_duties1: $('#User_duties').textbox('getText'),
                Tel1: $('#Tel').textbox('getText'),
                Fax1: $('#Fax').textbox('getText'),
                Email1: $('#Email').textbox('getText'),
                QQ1: $('#QQ').textbox('getText'),
                MSN1: $('#MSN').textbox('getText'),
                Address1: $('#Address').textbox('getText'),
                Postcode1: $('#Postcode').textbox('getText'),
                Remarks1: $('#Remarks').textbox('getText')
            },
            success: function (data) {
                if(data=='F'){
                    $.messager.alert('提示', '修改失败！');
                }
                else if (data == 'T') {
                    $.messager.alert('提示', '保存成功！');
                }
               
            }
        });
    })

    //证书信息
    $('#qualification_certificate').datagrid({
        title: "证书信息",
        //nowrap: false,
        striped: true,
        //rownumbers: true,
        //singleSelect: true,
        //autoRowHeight: true,
        ctrlSelect: true,//ctrlSelect
        border: false,
        fitColumns: true,
        width: tabs_width - 600-15,
        height: _height-50,
        pagination: true,
        pageSize: num,
        pageList: [num, num + 10, num + 20, num + 20],
        pageNumber: 1,
        type: 'POST',
        dataType: "json",
        url: "/mainform/Personal_center/personal_information.ashx?&cmd=load_certificate",
        columns: [[
           { field: 'id', title: '序号' },
           { field: 'User_count', title: '用户' },
           { field: 'User_name', title: '姓名' },
           { field: 'certificate_name', title: '证书名称' },
           { field: 'certificate_num', title: '证书编号' },
           { field: 'Issuing_unit', title: '发证单位' },
           { field: 'effective_date', title: '有效日期', formatter: function (value, row, index) {
               if (value) {
                   if (value.length > 10) {
                       value = value.substr(0, 10)
                       return value;
                   }
               }
               } },
           { field: 'remarks', title: '附注' }
        ]],
        sortOrder: 'asc',
        toolbar: "#qualification_certificate_toolbar"
    });

    //增加证书
    $('#add_certificate').unbind('click').bind('click', function () {
        $('#S_dialog').dialog({
            width: 400,
            height: 300,
            modal: true,
            title: '增加证书',
            draggable: true,
            buttons: [{
                text: '确定',
                iconCls: 'icon-ok',
                id: 'save1'
            }, {
                text: '取消',
                iconCls: 'icon-cancel',
                id: 'cancel'
            }]
        });
        $('#cancel').unbind('click').bind('click', function () {
            $('#S_dialog').dialog('close');
        });
        $('#S_dialog').form('reset');

        $('#save1').unbind('click').bind('click', function () {
            if ($('#certificate_name').textbox('getText') != "") {
                if ($('#certificate_num').textbox('getText') != "") {
                    if ($('#Issuing_unit').textbox('getText') != "") {
                        if ($('#effective_date').datebox('getValue') != "") {
                            $('#S_dialog').form('submit', {
                                url: "/mainform/Personal_center/personal_information.ashx",
                                ajax: true,
                                type: 'POST',
                                onSubmit: function (param) {
                                    param.cmd = 'add_certificate';
                                },
                                success: function (data) {
                                    if (data == 'T') {
                                        $('#S_dialog').dialog('close');
                                        $.messager.alert('提示', '增加信息成功');
                                        $('#qualification_certificate').datagrid('reload');
                                    }
                                }
                            });
                        } else {
                            $.messager.alert('提示', '请填写有效日期！');
                        }
                    } else {
                        $.messager.alert('提示', '请填写发证单位！');
                    }
                } else {
                    $.messager.alert('提示', '请填写证书编号！');
                }
            } else {
                $.messager.alert('提示', '请填写证书名字！');
            }

        });
    })
    //删除证书
    $('#remove_certificate').unbind('click').bind('click', function () {
        var certificate_info = $("#qualification_certificate").datagrid("getSelected");
        if (certificate_info) {
            $.messager.confirm('提示', '您确认要删除选中证书吗？', function (r) {
                if (r) {
                    var certificate_infos = $("#qualification_certificate").datagrid('getSelections');
                    var certificates = "";
                    for (var i = 0; i < certificate_infos.length; i++) {
                        if (i == 0) {
                            certificates = certificate_infos[i].id;
                        }
                        if (i > 0) {
                            certificates = certificates + ";" + certificate_infos[i].id;
                        }
                    }
                    $.ajax({
                        url: "personal_information.ashx?&cmd=del_certificate",
                        type: 'POST',
                        data: {
                            certificates: certificates
                        },
                        success: function (data) {
                            if (data == 'T') {
                                $('#qualification_certificate').datagrid('reload');
                            }
                        }
                    });
                }
            });
        }
        else {
            $.messager.alert('提示','请选择要操作的行');
        }


    })
    //到期预警
    $('#due_warning').unbind('click').bind('click', function () {
        $('#qualification_certificate').datagrid({
            url: "personal_information.ashx?&cmd=due_warning",
            dataType: "json",
            type: 'POST'
        });
    })
    //过期查询
    $('#overdue_query').unbind('click').bind('click', function () {
        $('#qualification_certificate').datagrid({
            url: "personal_information.ashx?&cmd=overdue_query",
            dataType: "json",
            type: 'POST'
        });
    })
    //显示全部
    $('#show_all').unbind('click').bind('click', function () {
        $('#qualification_certificate').datagrid({
            url: "personal_information.ashx?&cmd=load_certificate",
            dataType: "json",
            type: 'POST'
        });
    })
    
});