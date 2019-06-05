$(function () {
    var cmd = "load";

    $('#customer_info').datagrid({
        //title: '供应商资料',
        // iconCls: 'icon-add',  
        border: false,
        nowrap: false,
        striped: true,
        rownumbers: true,
        singleSelect: true,
        //autoRowHeight: true,
        fit: true,
        fitColumns: true,
        pagination: true,
        pageSize: 15,
        pageList: [15, 30, 45, 60],
        pageNumber: 1,
        type: 'POST',
        dataType: "json",
        url: "customer_info.ashx?&cmd=" + cmd,//接收一般处理程序返回来的json数据         
        columns: [[
        { field: 'customer', title: '公司名称', sortable: 'true' },
        { field: 'customer_en', title: 'company' },
        { field: 'address', title: '公司地址' },
        { field: 'address_en', title: 'address' },
        { field: 'postcode', title: '邮政编码' },
        { field: 'phone', title: '公司电话' },
        { field: 'c_fax', title: '公司传真' },
        { field: 'web_site', title: '公司网址' },
        { field: 'rg_sites', title: '注册地点' },
        { field: 'Ownership', title: '单位权属' },
        { field: 'Investment_type', title: '投资方式' },
        { field: 'partnership', title: '合作等级' },
        { field: 'remarks', title: '附注' }
        ]],
        toolbar: customer_info_toolbar
    });

    $('#customer_info_add').click(function () {
        customer_info_add();

    });
    $('#customer_info_edit').click(function () {
        customer_info_edit();

    });
    $('#customer_info_del').click(function () {
        customer_info_del();

    });
    $('#customer_info_search').click(function () {
        customer_info_search();

    });
    //联系人管理显示
    $('#customer_info_contracts').click(function () {
        view_contacts();
    });

    $('#search').combobox({
        //url: 'combobox_data.json',
        //valueField: 'id',
        //textField: 'text'
        data: [
           { 'value': '0', 'text': '公司名称' },
           { 'value': '1', 'text': 'company' },
           { 'value': '2', 'text': '关键字' }
        ]

    });
    $('#search1').textbox({

        value: '请输入收索内容 '


    });
    $('#rg_sites').combobox({
        //url: 'combobox_data.json',
        //valueField: 'id',
        //textField: 'text'
        data: [
           { 'value': '1', 'text': '境 内' },
           { 'value': '2', 'text': '境 外' },
        ]
    });

    $('#partnership').combobox({
        //url: 'combobox_data.json',
        //valueField: 'id',
        //textField: 'text'
        data: [
           { 'value': '1', 'text': 'A' },
           { 'value': '2', 'text': 'B' },
           { 'value': '3', 'text': 'C' },
           { 'value': '4', 'text': 'D' },
           { 'value': '5', 'text': 'D' },
           { 'value': '6', 'text': 'E' },
           { 'value': '7', 'text': 'F' }

        ]
    });

    $('#Investment_type').combobox({
        //url: 'combobox_data.json',
        //valueField: 'id',
        //textField: 'text'
        data: [
           { 'value': '1', 'text': '国有' },
           { 'value': '2', 'text': '民营' },
           { 'value': '3', 'text': '外资' },
           { 'value': '4', 'text': '合资' },
           { 'value': '5', 'text': '其他' }

        ]
    });

    $('#sex').combobox({
        //url: 'combobox_data.json',
        //valueField: 'id',
        //textField: 'text'
        data: [
           { 'value': '0', 'text': '男' },
           { 'value': '1', 'text': '女' }

        ]

    });

    //搜索客户信息
    function customer_info_search() {
        var cmd = 'search';
        var search = $('#search').combobox('getText');
        var search1 = $('#search1').combobox('getText');

        switch (search) {
            case "公司名称": search = "customer"; break;
            case "company": search = "customer_en"; break;
            case "关键字": search = "keyword"; break;
            default: search = "";
        }

        $('#customer_info').datagrid({
            type: 'POST',
            dataType: "json",
            url: "customer_info.ashx?&cmd=" + cmd,//接收一般处理程序返回来的json数据 

            queryParams: {
                search: search,
                search1: search1
            }
        });

        $('#search').combobox({
            //url: 'combobox_data.json',
            //valueField: 'id',
            //textField: 'text'
            data: [
               { 'value': '0', 'text': '公司名称' },
               { 'value': '1', 'text': 'company' },
               { 'value': '2', 'text': '关键字' }
            ]

        });
        $('#search1').textbox({

            value: '请输入收索内容 '


        });
    }
    //添加客户信息
    function customer_info_add() {

        $('#C_customer_info').dialog({
            width: 500,
            height: 460,
            modal: true,
            title: '添加客户资料',
            draggable: true,
            buttons: [{
                text: '确定',
                iconCls: 'icon-ok',
                id: 'save'
            }, {
                text: '取消',
                iconCls: 'icon-cancel',
                id: 'customer_info_add_cancel'

            }]
        }
   );
        $('#customer_info_add_cancel').click(function () {

            $('#C_customer_info').dialog('close');

        });


        $('#C_customer_info').form('reset');

        $('#save').click(function () {
            //form表单提交

            $('#C_customer_info').form('submit', {
                url: "customer_info.ashx",
                ajax: true,
                onSubmit: function (param) {

                    param.cmd = 'add';
                    param.rg_sites = $('#rg_sites').combobox('getText');
                    param.Investment_type = $('#Investment_type').textbox('getText');
                    param.partnership = $('#partnership').textbox('getText');
                },
                success: function (data) {
                    if (data == 'T') {
                        $('#C_customer_info').dialog('close');
                        $.messager.alert('提示', '添加信息成功');
                        $('#customer_info').datagrid('reload');
                    } else if (data == "无权操作！") {
                        $.messager.alert('提示', '您没有权限进行此操作！');

                    }

                }
            });


        });

    }
    //更新客户信息
    function customer_info_edit() {


        $('#C_customer_info').dialog({
            width: 500,
            height: 460,
            modal: true,
            title: '添加客户资料',
            draggable: true,

            buttons: [{
                text: '确定',
                iconCls: 'icon-ok',
                id: 'save'
            }, {
                text: '取消',
                iconCls: 'icon-cancel',
                id: 'customer_info_edit_cancel'

            }]
        }
                ).dialog('close');

        $('#customer_info_edit_cancel').click(function () {

            $('#C_customer_info').dialog('close');

        });
        $('#C_customer_info').form('reset');
        var selectRow_c = $("#customer_info").datagrid("getSelected");
        if (selectRow_c) {
            var rowss = $('#customer_info').datagrid('getSelections');
            $('#rg_sites').combobox("setText", rowss[0].rg_sites);
            $('#partnership').textbox("setText", rowss[0].partnership);
            $('#Investment_type').textbox("setText", rowss[0].Investment_type);
            $('#C_customer_info').form('load', rowss[0]);
            $('#C_customer_info').dialog('open');

        }
        else {
            $.messager.alert('提示', '请选择要操作的行！');
        }



        $('#save').click(function () {
            //form表单提交
            var selectRow_c = $("#customer_info").datagrid("getSelected");
            $('#C_customer_info').form('submit', {
                url: "customer_info.ashx",
                ajax: true,
                onSubmit: function (param) {

                    param.cmd = 'edit';
                    param.rg_sites = $('#rg_sites').combobox('getText');
                    param.Investment_type = $('#Investment_type').textbox('getText');
                    param.partnership = $('#partnership').textbox('getText');
                    param.id = selectRow_c.id;
                },
                success: function (data) {
                    if (data == 'T') {
                        $('#C_customer_info').dialog('close');
                        $.messager.alert('提示', '更新信息成功');
                        $('#customer_info').datagrid('reload');
                    } if (data == "无权操作！") {
                        $.messager.alert('提示', '您没有权限编辑资料！');
                    }


                }
            });


        });


    }
    //删除客户信息
    function customer_info_del() {
        var selectRow_c = $("#customer_info").datagrid("getSelected");
        if (selectRow_c) {
            var cmd = "del";
            $.ajax({
                url: "customer_info.ashx?&cmd=del",
                type: 'POST',
                data: {
                    id: selectRow_c.id
                },
                success: function (data) {
                    if (data == 'T') {

                        $('#customer_info').datagrid('reload');
                    } if (data == "无权操作！") {
                        $.messager.alert('提示', '您没有权限删除资料！');

                    }

                }

            })
        }

        else {

            $.messager.alert('提示', '请选择要操作的行！');
        }



    }

    //************************联系人************************
    //联系人管理
    function view_contacts() {
        var selectRow = $("#customer_info").datagrid("getSelected");
        //var g_id_c = selectRow.id;
        if (selectRow) {
            var rowss = $('#customer_info').datagrid('getSelections');
            $('#container_title').form('load', rowss[0]);
            $('#S_dialog_contacts').dialog({
                width: 800,
                top: 0,
                modal: true,
                title: '客户资料_联系人',
                draggable: true
            }).dialog('close');

            //客户联系人查看管理
            $('#S_contacts_data').datagrid({
                title: '客户联系人',
                width: 800,
                height: 350,
                nowrap: false,
                striped: true,
                rownumbers: true,
                singleSelect: true,
                //autoRowHeight: true,
                border: false,
                //fitColumns: true,
                pagination: true,
                pageSize: 5,
                pageList: [5, 10, 15, 20],
                pageNumber: 1,
                type: 'POST',
                dataType: "json",
                url: "customer_info.ashx?&cmd=load_contacts",//接收一般处理程序返回来的json数据 
                queryParams: {
                    customer_id: selectRow.id
                },
                columns: [[
                { field: 'contacts_name', title: '姓名' },
                { field: 'contacts_sex', title: '姓别' },
                { field: 'contacts_department', title: '部门' },
                { field: 'contacts_job', title: '职务' },
                { field: 'tel', title: '座机' },
                { field: 'phone', title: '手机' },
                { field: 'FAX', title: '传真' },
                { field: 'Email', title: '邮箱' },
                { field: 'QQ', title: 'QQ' },
                { field: 'MSN', title: 'MSN' },
                { field: 'address', title: 'address' },
                { field: 'postcode', title: '邮编' },
                { field: 'remarks', title: '备注' }
                ]],
                toolbar: [{
                    text: '添加',
                    iconCls: 'icon-add',
                    id: 'h_add',
                    plain: true
                }, '-', {
                    text: '修改',
                    iconCls: 'icon-edit',
                    id: 'h_edit',
                    plain: true
                }, '-', {
                    text: '删除',
                    id: 'h_del',
                    iconCls: 'icon-remove',
                    plain: true
                }]
            });
            $('#S_dialog_contacts').dialog('open');
        }
        else {

            $.messager.alert('提示', '请选择要操作的行！');
        }

        $('#sex').combobox({
            //url: 'combobox_data.json',
            //valueField: 'id',
            //textField: 'text'
            data: [
               { 'value': '0', 'text': '男' },
               { 'value': '1', 'text': '女' }

            ]

        });
        $('#h_add').click(function () {
            Supplier_infoadd_t();
        });
        $('#h_edit').click(function () {

            Supplier_info_edit_t1();

        });
        $('#h_del').click(function () {

            Supplier_delete_t();
        });
    }
    //联系人添加
    function Supplier_infoadd_t() {
        var selectRow = $("#customer_info").datagrid("getSelected");
        $('#S_contacts_edit').dialog({
            width: 500,
            height: 500,
            top: 0,
            modal: true,
            title: '添加联系人资料',
            draggable: true,
            buttons: [{
                text: '确定',
                iconCls: 'icon-ok',
                id: 'save_c'
            }, {
                text: '取消',
                iconCls: 'icon-cancel',
                handler: function () {
                    $('#S_contacts_edit').dialog('close');
                }
            }]
        });
        var selectRow = $("#customer_info").datagrid("getSelected");
        //var g_id_c = selectRow.id;
        $('#S_contacts_edit').form('reset');
        $('#save_c').click(function () {
            $('#S_contacts_edit').form('submit', {
                url: "customer_info.ashx",
                ajax: true,
                onSubmit: function (param) {
                    param.cmd = 'contacts_add';
                    param.customer_id = selectRow.id;
                    param.customer = selectRow.customer;
                    param.sex = $('#sex').combobox('getText');
                },
                success: function (data) {
                    //alert("ssss");
                    if (data == 'T') {
                        $('#S_contacts_edit').dialog('close');
                        $.messager.alert('提示', '添加信息成功');
                        $('#S_contacts_data').datagrid('reload');
                    } if (data == "无权操作！") {
                        $.messager.alert('提示', '您没有权限添加资料！');
                    }
                }
            });
        });
    }
    //联系人编辑
    function Supplier_info_edit_t1() {
        var selectRow1 = $("#S_contacts_data").datagrid("getSelected");
        //alert(selectRow1.id);
        $('#S_contacts_edit').dialog({
            width: 500,
            height: 500,
            modal: true,
            title: '更新联系资料',
            draggable: true,
            buttons: [{
                text: '确定',
                iconCls: 'icon-ok',
                id: 'save_edit_c'
            }, {
                text: '取消',
                iconCls: 'icon-cancel',
                handler: function () {
                    $('#S_contacts_edit').dialog('close');
                }
            }]
        }
        ).dialog('close');

        if (selectRow1) {
            var rowss = $('#S_contacts_data').datagrid('getSelections');
            $('#sex').combobox("setText", rowss[0].sex);
            $('#S_contacts_edit').form('load', rowss[0]);
            $('#S_contacts_edit').dialog('open');

        }
        else {
            $.messager.alert('提示', '请选择要操作的行！');
        }

        //提交编辑信息
        $('#save_edit_c').click(function () {

            $('#S_contacts_edit').form('submit', {
                url: "customer_info.ashx",
                ajax: true,
                onSubmit: function (param) {
                    param.cmd = 'contacts_edit';
                    param.id = selectRow1.id;
                    param.sex = $('#sex').combobox('getText');
                },
                success: function (data) {
                    if (data == 'T') {
                        $('#S_contacts_edit').dialog('close');
                        $.messager.alert('提示', '编辑信息成功');
                        $('#S_contacts_data').datagrid('reload');
                    } if (data == "无权操作！") {
                        $.messager.alert('提示', '您没有权编辑加资料！');
                    }
                }
            });
        });
    }
    //联系人删除
    function Supplier_delete_t() {

        var selectRow2 = $("#S_contacts_data").datagrid("getSelected");
        if (selectRow2) {
            var cmd = "del";
            $.ajax({
                url: "customer_info.ashx?&cmd=Supplier_delete",
                type: 'POST',
                data: {
                    id: selectRow2.id
                },
                success: function (data) {
                    if (data == 'T') {
                        $('#S_contacts_data').datagrid('reload');
                    } if (data == "无权操作！") {
                        $.messager.alert('提示', '您没有权限删除资料！');
                    }
                }
            })
        }
        else {

            $.messager.alert('提示', '请选择要操作的行！');
        }
    }
})
