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
            //ִ�еķ��� 
            $('#login').click();
        }
    }
    function show_info() {

        $.messager.alert('��ʾ', '�û��������������');

    }
</script>
</head>

    <body style="margin-top: 0px; overflow: scroll; overflow-x: hidden; overflow-y: hidden">
    <div class="page-container" style=" background-color:rgba(0,0,0,0.5);padding:40px;border-radius:10px;">
        <div style="vertical-align: middle;">
            <h1>�����������ֻ��������ϵͳ</h1>
            <h1>��¼(Login)</h1>

            <input type="text" id="phians_name" style="width: 300px" class="username" placeholder="�����������û�����" />
            <input type="password" id="phians_pwd" style="width: 300px" class="password" placeholder="�����������û����룡" />
            <%--<input type="Captcha" class="Captcha" name="Captcha" placeholder="��������֤�룡"/>--%>
            <button style="width: 330px" id="login">��¼</button>
            <div class="error"></div>

        </div>

    </div>

    <div class="footer" style="position:absolute;bottom:10px;right:20px;">
           <p style="font-size:12px;line-height:20px;text-align:left;"> ��Ӧ�̣��㶫���ƶ���ҵ���ݼ������޹�˾</p>
         <p style="font-size:12px;line-height:20px;text-align:left;"> ��ϵ�绰��0755-86218118</p>     
        <p style="font-size:12px;line-height:20px;text-align:left;"> ��˾��ַ����������ɽ����ɽ·��ȴ�ѧ�Ǵ�ҵ԰��¥��ɣ̩���ã�</p>
    </div>
</body>
      <%= WebExtensions.CombresLink("siteJs") %>
      <script  charset="gb2312"   src="/phians_js/login_new.js?version=20160722"></script>
</html>
