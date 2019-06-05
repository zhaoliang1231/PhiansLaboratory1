<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="mainform.aspx.cs" Inherits="phians.mainform.WebForm1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>东方电气-数字化理化系统</title>
    <%= WebExtensions.CombresLink("mainformCss") %>
    <%--<link rel="stylesheet" type="text/css" href="/artDialog/skins/blue.css" />--%>

</head>
  
<body>
    <form runat="server" id="mainbox">
        <div id="m_north" data-options="region:'north',title:'header',noheader:true" style="overflow: scroll; overflow-x: hidden; overflow-y: hidden; height: 95px">
            <div class="h_d">
                <div class="h_d1" style="margin-top:10px;text-align:left;margin-left:15px;"><img width="130" src="/image/东方重机logo.png" />
                    <p class="" style="font-size:16px;margin-top:2px;color: #27408B;letter-spacing:3px;">(东方电气数字化质量检测系统)</p>
                </div>
                <div class="h_d1" style="margin-left:120px;">
                    
                </div>

                <div class="h_d1 logo3">
                    <%--登录用户名和时间--%>

                    <div style="display: none">
                        <asp:Button ID="Button1" runat="server" Text="退出系统" OnClick="Clear" />
                    </div>
                    <%--操作按钮--%>
                    <div style="margin-top: 65px">
                        <div id="operate_click" style="color:#fff;font-size:16px;border:0px;">系统操作</div>
                    </div>


                    <div style="display: none"><a id="test_message" href="#" class="easyui-linkbutton" data-options="iconCls:'icon-edit'">测试消息</a>    </div>

                </div>

                <asp:HiddenField ID="HiddenField1" runat="server" />
                <asp:HiddenField ID="message_type" runat="server" />
            </div>
        </div>
        <div data-options="region:'south',noheader:true" style="height: 35px; line-height: 30px; text-align: center;">
             <asp:Label ID="bottom_text" runat="server" Text="Label"></asp:Label>
       
           
        </div>
        <div data-options="region:'west',title:'导航菜单',iconCls:'icon-home'" style="width: 180px;font-size:14px;">

            <div id="m_menu" class="easyui-accordion" data-options="fit:true,border:false">
                <!--  导航内容 -->
            </div>
        </div>
        <%--    style="overflow:hidden;"--%>
        <div id="mainPanle" data-options="region:'center'">
            <div id="tabs" class="easyui-tabs" data-options="fit:true,border:false">
                <div id="home" class="image_show" data-options="title:'起始页'" style="padding: 1px; display: block; width: 100%; height: 100%; background: url(/image/start_pic_2.jpg) top center no-repeat; background-size: cover; opacity: 0.8">
                    <%--style="padding: 1px; display: block; width: 100%; height: 100%; background: url(/image/start_pic.png) top center no-repeat; background-size: cover; opacity: 0.8"--%>

                    <%--<iframe src="/mainform/home.html" style="height:100%;width:100%;border:none"></iframe>--%>
                    <div class="h_d" style="float: right; margin-top: 10px;color:#fff;" >

                        <asp:Label ID="Label1" runat="server" Text="Label"></asp:Label>
                        <div class="h_d1" id="m_showtime" style="color:#fff;"></div>

                    </div>
                    <div class="h_d">
                        <h1 class="welcome_2">

                            东方电气数字化质量检测系统
                           </h1>
                    </div>
                </div>
            </div>
        </div>
    </form>
    <%--修改密码框--%>
    <div style="display: none">

        <form id="password_dialog">
            <div class="password" style="margin-top: 15px; margin-top: 15px">
                <div class="password1">

                    <a href="#" style="width: 60px">原密码： </a>
                    <input id="old_password" type="password" class="easyui-textbox " data-options="required:true" style="width: 250px" />
                </div>
                <div class="password1">
                    <a href="#" style="width: 60px">新密码： </a>
                    <input id="new_password" type="password" class="easyui-textbox" data-options="required:true" style="width: 250px" />
                </div>
                <div class="password1">
                    <a href="#" style="width: 60px">重复密码：</a><input id="R_new_password" type="password" class="easyui-textbox" data-options="required:true" style="width: 250px" />
                </div>
            </div>
        </form>
    </div>
    <%--操作菜单--%>
    <div id="menu_show" style="width: 100px;">
        <div id="Sign_out" data-options="iconCls:'icon-cancel'">退出系统</div>
        <div id="change_password" data-options="iconCls:'icon-lock'">修改密码</div>
        <div data-options="iconCls:'icon-import'"><a href="/mainform/Controls_download.html" target="_blank">下载控件</a></div>
    </div>
    <%--消息菜单--%>
    <div id="message_menu" runat="server"  style="width: 180px;">        
          
    </div>
    <form id="check_session"> </form>
 
</body>


    <%= WebExtensions.CombresLink("mainformJs") %>
     <script src="/signalr/hubs"></script>
    <script type="text/javascript">
        function display() {
            window.open("http://www.phians.cn/");
        }
</script>
</html>
