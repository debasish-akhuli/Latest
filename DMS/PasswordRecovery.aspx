<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PasswordRecovery.aspx.cs" Inherits="DMS.PasswordRecovery" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>myDOCbase</title>
    <link  href="style.css" rel="stylesheet" type="text/css" media="all" />
    <link  href="editable.css" rel="stylesheet" type="text/css" media="all" />
    <link  href="common.css" rel="stylesheet" type="text/css" media="all" />
</head>
<body>
    <form id="form1" runat="server">
        <div class="header">
            <div class="main">
            <div class="logo_bg"> <%--<a href="#"><img src="images/logo.jpg" width="286" height="88" alt="" border="0" /></a>--%></div>
            <h1><a href="userhome.aspx" style="border:none; text-decoration:none; outline:none;"><img src="images/product_logo.png" border="0" alt="" /></a></h1>
            </div>
        </div>
        <div style=" position:relative;" class="main_body">
            <div style="height:350px; padding-top:90px;">
                <div style=" width:400px; height:200px; margin:0 auto; padding:20px; background:#fff;">
                    <div style="font-weight:normal; text-align:center; font-size:24px; color:#010439; font-family:Arial,verdana,Times New Roman;">
                        Password Recovery
                    </div>
                    <div style=" margin:0 auto; width:330px; margin-top:50px;">
                        <div style=" width:100%; float:left; margin-bottom:10px;">
                            <div style=" float:left; width:120px; font-weight:bold;">Enter Your Mail ID</div>
                            <div style=" float:left;">
                                <asp:TextBox ID="txtMail" CssClass="input_field" runat="server" Text=""></asp:TextBox>
                                <span style="padding-left:5px; float:left; font-weight:bold; font-size:24px; color:Red;">*</span>
                            </div>
                        </div>
                        <div style=" float:left; margin-bottom:10px; margin-left:120px;">
                            <asp:Button CssClass="login_btn"  ID="cmdSubmit" runat="server" Text="Submit" onclick="cmdSubmit_Click" />
                            <a class="ForgotPwd" href="Default.aspx"><div style="float:left; width:100%; margin-left:150px; font-size:12px;">Login Now !!</div></a>
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
