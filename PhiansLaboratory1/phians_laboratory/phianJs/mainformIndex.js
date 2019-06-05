$(function () {
    /*//激活左边菜单*/
    $('#menu').sidebarMenu({
        data: [{
            id: '1',
            text: '系统设置',
            icon: 'menu-icon fa fa-asterisk',
            url: '',
            menus: [{
                id: '11',
                text: '编码管理',
                icon: 'ace-icon fa fa-glass',
                url: 'tables.html'
            }]
        }, {
            id: '2',
            text: '基础数据',
            icon: 'menu-icon fa  fa-asterisk',
            url: '',
            menus: [{
                id: '21',
                text: '基础特征',
                icon: 'ace-icon fa fa-glass',
                url: 'treeview.html'
            }, {
                id: '22',
                text: '特征管理',
                icon: 'ace-icon fa fa-glass',
                url: 'email.html'
            }, {
                id: '23',
                text: '物料维护',
                icon: 'ace-icon fa fa-glass',
                url: 'email.html'
            }, {
                id: '24',
                text: '站点管理',
                icon: 'ace-icon fa fa-glass',
                url: 'email.html'
            }]
        }, {
            id: '3',
            text: '权限管理',
            icon: 'menu-icon fa  fa-asterisk',
            url: '',
            menus: [{
                id: '31',
                text: '用户管理',
                icon: 'ace-icon fa fa-glass',
                url: 'email.html'
            }, {
                id: '32',
                text: '角色管理',
                icon: 'ace-icon fa fa-glass',
                url: 'email.html'
            }, {
                id: '33',
                text: '菜单管理',
                icon: 'ace-icon fa fa-glass',
                url: 'email.html'
            }, {
                id: '34',
                text: '部门管理',
                icon: 'ace-icon fa fa-glass',
                url: 'email.html'
            }]
        }, {
            id: '4',
            text: '订单管理',
            icon: 'menu-icon fa  fa-asterisk',
            url: '',
            menus: [{
                id: '41',
                text: '订单查询',
                icon: 'ace-icon fa fa-glass',
                url: '/Order/Query'
            }, {
                id: '42',
                text: '订单排产',
                icon: 'ace-icon fa fa-glass',
                url: 'tables.html'
            }, {
                id: '43',
                text: '订单撤排',
                icon: 'ace-icon fa fa-glass',
                url: 'email.html'
            }, {
                id: '44',
                text: '订单HOLD',
                icon: 'ace-icon fa fa-glass',
                url: 'email.html'
            }, {
                id: '45',
                text: '订单删除',
                icon: 'ace-icon fa fa-glass',
                url: 'email.html'
            }, {
                id: '47',
                text: '订单插单',
                icon: 'ace-icon fa fa-glass',
                url: 'email.html'
            }, {
                id: '48',
                text: '订单导入',
                icon: 'ace-icon fa fa-glass',
                url: 'email.html'
            }]
        }]
    });
});