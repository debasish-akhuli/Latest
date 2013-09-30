<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="DMS._Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>myDOCbase</title>
    <link href="style.css" rel="stylesheet" type="text/css" media="all" />
    <link href="editable.css" rel="stylesheet" type="text/css" media="all" />
    <link href="common.css" rel="stylesheet" type="text/css" media="all" />
</head>
<body>
    <form id="form1" runat="server">
        <div class="header">
            <div class="main">
            <div class="logo_bg"><%--<a href="#"><img src="images/logo.jpg" width="286" height="88" alt="logo" border="0" /></a>--%></div>
            <h1><img src="images/product_logo.png" alt="" /></h1>
            </div>
        </div>
        <div style="position:relative;" class="main_body">
            <div style="height:350px; padding-top:90px;">
                <div style="width:400px; height:200px; margin:0 auto; padding:20px; background:#fff;">
                    <div style="font-weight:normal; text-align:center; font-size:24px; color:#010439; font-family:Arial,verdana,Times New Roman;">
                        Login
                    </div>
                    <div style="margin:0 auto; width:280px; margin-top:50px;">
                        <div style="width:100%; float:left; margin-bottom:10px;">
                            <div style="float:left; width:70px; font-weight:bold;">Email ID</div>
                            <div style="float:left;">
                                <asp:TextBox ID="txtEmailID" CssClass="input_field" runat="server" Text=""></asp:TextBox>
                            </div>
                        </div>
                        <div style="width:100%; float:left; margin-bottom:10px;">
                            <div style="float:left; width:70px; font-weight:bold;">Password</div>
                            <div style="float:left;">
                                <asp:TextBox ID="txtPwd" CssClass="input_field" runat="server" TextMode="Password" Text=""></asp:TextBox>
                            </div>
                        </div>
                        <div style="float:left; margin-bottom:10px; margin-left:70px;">
                            <asp:Button CssClass="login_btn" ID="cmdLogin" runat="server" Text="Login" onclick="cmdLogin_Click" />
                            <a class="ForgotPwd" href="PasswordRecovery.aspx"><div style="float:left; width:100%; margin-left:150px; font-size:12px;">Forgot Password?</div></a>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="footer_main">
            <div class="footer_text">Powered by ADA modeling technology</div>
        </div>
    </form>
</body>
</html>