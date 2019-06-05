<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="message_show2.aspx.cs" Inherits="phians.message_show2" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
     <script type="text/javascript" src="/easyui/jquery.min.js"></script>
    <script type="text/javascript" src="/easyui/jquery.easyui.min.js"></script>
    <script type="text/javascript" src="/easyui/locale/easyui-lang-zh_CN.js"></script>
    <script src="/Scripts/jquery.signalR-2.2.1.min.js"></script>
    <script src="/signalr/hubs"></script>
    <script src="/Scripts/jquery.cookie.js"></script>
    <link rel="stylesheet" type="text/css" href="/easyui/themes/default/easyui.css" />
    <link rel="stylesheet" type="text/css" href="/easyui/themes/icon.css" />
  <style type="text/css">
        .container {
            background-color: #99CCFF;
            border: thick solid #808080;
            padding: 20px;
            margin: 20px;
        }
    </style>
</head>
<body>
     <div class="container">
        <input type="text" id="user_name"/>
        <input type="text" id="message" />
        <input type="button" id="sendmessage" value="发送消息" />
        <input type="hidden" id="displayname" />
        <ul id="discussion"></ul>
       
    </div>
     <script type="text/javascript" >
         $(function () {
             //To enable logging for your hub's events in a browser, add the following command to your client application
             $.connection.hub.logging = true;
             // Declare a proxy to reference the hub.
             var chat = $.connection.message_Hub;

             chat.client.showMessage = function (message) {



                 $.messager.show({
                     title: "消息通知",
                     msg: "消息内容：" + message,
                     height: 300,
                     width: 300,
                     timeout: 8000,
                     showType: 'slide'
                 });


             };
            

             $('#message').focus();
             // Start the connection.
             $.connection.hub.start().done(function () {
                 $('#sendmessage').click(function () {
                     // Call the Send method on the hub.
                     chat.server.sendmessage2($('#user_name').val(), $('#message').val());
                     // chat.server.SendMessage($('#message').val());
                     // Clear text box and reset focus for next comment.
                     $('#message').val('').focus();
                 });
             });
         });
    </script>
</body>
</html>
