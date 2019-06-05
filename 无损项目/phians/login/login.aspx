<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="login.aspx.cs" Inherits="phians.login2" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
  
<head runat="server">
     
<meta http-equiv="Content-Type" content="text/html; charset=gb2312"/>
    <title></title>  
   
      <%= WebExtensions.CombresLink("siteCss") %>
  

    <script type="text/javascript">
    if (top.location !== self.location) { 
        top.location = self.location;
    }

    document.onkeydown = function (event) {
        e = event ? event : (window.event ? window.event : null);
        if (e.keyCode == 13) {
            //执行的方法 
            $('#login').click();
        }
    }
    function show_info() {

        $.messager.alert('提示', '用户名或者密码错误');

    }
</script>
</head>

    <body style="margin-top: 0px; overflow: scroll; overflow-x: hidden; overflow-y: hidden">
    <div class="page-container" style=" background-color:rgba(0,0,0,0.5);padding:40px;border-radius:10px;">
        <div style="vertical-align: middle;">
            <h1>东方电气数字化质量检测系统</h1>
            <h1>登录(Login)</h1>

            <input type="text" id="phians_name" style="width: 300px" class="username" placeholder="请输入您的用户名！" />
            <input type="password" id="phians_pwd" style="width: 300px" class="password" placeholder="请输入您的用户密码！" />
            <%--<input type="Captcha" class="Captcha" name="Captcha" placeholder="请输入验证码！"/>--%>
            <button style="width: 330px" id="login">登录</button>
            <div class="error"></div>

        </div>

    </div>

    <div class="footer" style="position:absolute;bottom:10px;right:20px;">
           <p style="font-size:12px;line-height:20px;text-align:left;"> 供应商：广东清大菲恩工业数据技术有限公司</p>
         <p style="font-size:12px;line-height:20px;text-align:left;"> 联系电话：0755-86218118</p>     
        <p style="font-size:12px;line-height:20px;text-align:left;"> 公司地址：深圳市南山区丽山路硅谷大学城创业园三楼（桑泰大厦）</p>
    </div>
</body>
      <%= WebExtensions.CombresLink("siteJs") %>
      <script  charset="gb2312"   src="/phians_js/login_new.js?version=20160722"></script>
</html>
