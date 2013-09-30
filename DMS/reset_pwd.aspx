<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="reset_pwd.aspx.cs" Inherits="DMS.reset_pwd" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>myDOCbase</title>
    <link  href="common.css" rel="stylesheet" type="text/css" media="all" />
    <link href="editable.css" rel="stylesheet" type="text/css" media="all" />
    <script type="text/javascript" src="js/jquary_min.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $(".drop_nav .drop").addClass('hide');
            $(".drop_nav").mouseover(function () {
                $(this).children(".drop").addClass('show');
                $(this).children(".drop").removeClass('hide');
            });
            $(".drop_nav").mouseout(function () {
                $(".drop_nav .drop").addClass('hide');
                $(this).children(".drop").removeClass('show');
            });
            $(".drop_nav1").mouseover(function () {
                $(this).children(".drop1").addClass('show');
                $(this).children(".drop1").removeClass('hide');
            });
            $(".drop_nav1").mouseout(function () {
                $(this).children(".drop1").addClass('hide');
                $(this).children(".drop1").removeClass('show');
            });
        });
    </script>
    <script type="text/javascript">
        function showLoading() {
            document.getElementById("dvLoading1").style.display = 'block';
            document.getElementById("dvLoading2").style.display = 'block';
        }
        function hideLoading() {
            document.getElementById("dvLoading1").style.display = 'none';
            document.getElementById("dvLoading2").style.display = 'none';
            return true;
        }
        window.onload = hideLoading;
    </script>
    <script type="text/javascript">
        $(document).ready(function () {
            $("a.close").hide();
            $("#red").hide();

            $("td.show_close").mouseover(function () {
                $(this).children('a.close').show();
            });

            $("td.show_close").mouseout(function () {
                $(this).children('a.close').hide();
            });

            $(".dropmain li").mouseover(function () {
                $(this).children('ul').addClass('g_show');
                $(this).children('ul').removeClass('g_hide');
            });

            $(".dropmain li").mouseout(function () {
                $(this).children('ul').removeClass('g_show')
                $(this).children('ul').addClass('g_hide')
            });

            $(".dropmain2 li").mouseover(function () {
                $(this).children('ul.dropmain3').addClass('g_show');
                $(this).children('ul.dropmain3').removeClass('g_hide');
            });

            $(".dropmain2 li").mouseout(function () {
                $(this).children('ul.dropmain3').addClass('g_hide');
                $(this).children('ul.dropmain3').removeClass('g_show');
            });

            $(".RedirectHL").click(function () {
                $('#def').hide();
                $('#red').show();
                $(this).hide();

                return false;

            });
            $("#close2").click(function () {
                $('#def').show();
                $('#red').hide();
                $('#dvLoading1').hide();
                $('#dvLoading2').hide();
                $('.RedirectHL').show();

            });
        });
           
    </script>

    <style type="text/css" >
    *{ padding:0px; margin:0px;}
    </style>
    <script language="javascript" type="text/javascript">
        var alertTimerIdS = 0;
        var alertTimerIdE = 0;

        var c = 0;
        var t;
        var timer_is_on = 0;

        function BrowserWidth() {
            var theWidth;
            if (window.innerWidth) {
                theWidth = window.innerWidth;
            }
            else if (document.documentElement && document.documentElement.clientWidth) {
                theWidth = document.documentElement.clientWidth;
            }
            else if (document.body) {
                theWidth = document.body.clientWidth;
            }
            return theWidth;
        }

        function BrowserHeight() {
            var theHeight;
            if (window.innerHeight) {
                theHeight = window.innerHeight;
            }
            else if (document.documentElement && document.documentElement.clientHeight) {
                theHeight = document.documentElement.clientHeight;
            }
            else if (document.body) {
                theHeight = document.body.clientHeight;
            }
            return theHeight;
        }

        function DisplayExtendSessionLifeBox() {

            var boxElement = document.getElementById("<%= ExtendSessionLifeBox.ClientID %>");
            if (boxElement) {

                boxElement.style.display = "block";
                var bw = BrowserWidth();
                var bh = BrowserHeight();
                var boxElementWidth = (boxElement.clientWidth) ? boxElement.clientWidth : boxElement.offsetWidth;
                var boxElementHeight = (boxElement.clientHeight) ? boxElement.clientHeight : boxElement.offsetHeight;

                var boxElementTop = (bh / 2) - (boxElementHeight / 2);
                var boxElementLeft = (bw / 2) - (boxElementWidth / 2);
                boxElement.style.top = boxElementTop;
                boxElement.style.left = boxElementLeft;
                boxElement.style.position = "absolute";
            }

            var boxElement2 = document.getElementById("ExtendSessionLifeBox2");
            if (boxElement2) {

                boxElement2.style.display = "block";
                var bw = BrowserWidth();
                var bh = BrowserHeight();
                var boxElement2Width = (boxElement2.clientWidth) ? boxElement2.clientWidth : boxElement2.offsetWidth;
                var boxElement2Height = (boxElement2.clientHeight) ? boxElement2.clientHeight : boxElement2.offsetHeight;

                //var boxElement2Top = (bh / 2) - (boxElement2Height / 2);
                //var boxElement2Left = (bw / 2) - (boxElement2Width / 2);
                boxElement2.style.top = "0px";
                boxElement2.style.left = "0px";
                boxElement2.style.position = "fixed";
            }
            clearTimeout(alertTimerIdS);
            c = 20; //set count down time
            doTimer();
        }

        function CloseExtendSessionLifeBox() {
            var boxElement = document.getElementById("<%= ExtendSessionLifeBox.ClientID %>");
            var boxElement2 = document.getElementById("ExtendSessionLifeBox2");
            //checking if the element exists before trying to use it
            if (boxElement) {
                boxElement.style.display = "none";
            }
            if (boxElement2) {
                boxElement2.style.display = "none";
            }
            clearTimeout(alertTimerIdE);
        }
        function startTimer() {
            CloseExtendSessionLifeBox();
            alertTimerIdS = setTimeout(DisplayExtendSessionLifeBox, 2880000);
            alertTimerIdE = setTimeout(CloseExtendSessionLifeBox, 3480000);
        }
        window.onload = startTimer;

        function timedCount() {
            document.getElementById('countDown').innerHTML = c;
            c = c - 1;
            t = setTimeout("timedCount()", 1000);
        }

        function doTimer() {
            if (!timer_is_on) {
                timer_is_on = 1;
                timedCount();
            }
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div class="header">
<div class="main">
  <div class="logo_bg"> <%--<a href="#"><img src="images/logo.jpg" width="286" height="88" alt="" border="0" /></a>--%></div>
  <h1><a href="userhome.aspx" style="border:none; text-decoration:none; outline:none;"><img src="images/product_logo.png" border="0" alt="" /></a></h1>
</div>
</div>
    <div>

        <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <%--For Session Expired Start--%>
    <div id="ExtendSessionLifeBox2" style=" width:100%; z-index:10000; height:100%; opacity:.2; filter:alpha(opacity=20); background:#660066; display:none;">
       </div>
    <asp:Panel ID="ExtendSessionLifeBox" runat="server" style="height:100px; z-index:10001; width:250px; background-color:#660066; padding:20px; color:#fff; border:solid 1px #666; display:none; position:absolute;">
   <div style=" width:100%; float:left">
  <asp:Label ID="ExtendSessionLifePrompt" runat="server" Text="Your session is going to expire in 10 minutes. Would you like to extend your Session for another 60 minutes?"></asp:Label>
  </div>
  <div style=" width:100%; float:left">
  <div id="countDown"></div>
  </div>
  <div style=" width:100%; float:left; padding-top:20px;">
  <div style=" float:left; padding-right:20px;">
  <asp:UpdatePanel ID="up1" runat="server" >
  <ContentTemplate>
  <asp:Button ID="ExtendSessionLife" runat="server" Text="Yes" OnClientClick="startTimer(); return true;"/>
  </ContentTemplate>
  </asp:UpdatePanel>
  </div>
  <div style=" float:left">
  <input type="button" id="CancelExtendSessionLife" value="No" onclick="CloseExtendSessionLifeBox(); return false;" />  
  </div>
  </div>
  
</asp:Panel>
<%--For Session Expired End--%>
    <div class="main_body">
        <div class="main">
            <div class="normal_common">
                <div class="wf_mas_Heading"><span>Welcome, </span><asp:Label ID="lblUser" runat="server" Text="User Name"></asp:Label>
                    <div class="dropdown" id="divMenuSuperAdmin" runat="server">
                        <div class="menu_bg_n">
                            <ul>
                                <li class="drop_nav"><a href="#">Filing</a>
                                    <ul class="drop hide">
                                        <li><a href="home.aspx">e-Filing system</a></li>
                                        <li><a href="cabinet_mast.aspx">Cabinet</a></li>
                                        <li><a href="drawer_mast.aspx">Drawer</a></li>
                                        <li><a href="folder_mast.aspx">Folder</a></li>
                                        <li><a href="doc_type_mast.aspx">Document Type</a></li>
                                        <li><a href="BlankTempUpload.aspx">New Template Upload</a></li>
                                        <li><a class="nob" href="doc_mast.aspx">New Document Upload</a></li>
                                        <%--<li><a href="grp_mast.aspx">Groups</a></li>
                                        <li><a class="nob" href="grp_doc.aspx">Group-wise Document</a></li>--%>
                                    </ul>
                                </li>
                                <li class="drop_nav"><a href="#">System</a>
                                    <ul class="drop hide">
                                        <li class="drop_nav1"><a href="#">User</a>
                                            <ul class="drop1 hide">
                                                <li><a href="dept_mast.aspx">Department</a></li>
                                                <li><a href="role_mast.aspx">Role</a></li>
                                                <li><a href="user_mast.aspx">New User</a></li>
                                                <li><a href="user_rights.aspx">User Rights</a></li>
                                                <li><a href="user_role.aspx" class="nob">User-wise Role Mapping</a></li>
                                            </ul>
                                        </li>
                                        <li><a href="workflow_mast.aspx">Workflow Definition</a></li>
                                        <li><a href="reset_pwd.aspx">Reset Password</a></li>
                                        <li><a href="mail_setup.aspx">Mail Setup</a></li>
                                        <li><a href="CompWiseStatistics.aspx">Statistics</a></li>
                                        <li><a href="ClientManagement.aspx">Client Management</a></li>
                                        <li><a class="nob" href="ServerConfig.aspx">Server Config</a></li>
                                    </ul>
                                </li>
                                <li class="no"><a href="logout.aspx">Logout</a></li>
                            </ul>
                        </div>
                    </div>
                    <div class="dropdown" id="divMenuAdmin" runat="server">
                        <div class="menu_bg_n">
                            <ul>
                                <li class="drop_nav"><a href="#">Filing</a>
                                    <ul class="drop hide">
                                        <li><a href="home.aspx">e-Filing system</a></li>
                                        <li><a href="cabinet_mast.aspx">Cabinet</a></li>
                                        <li><a href="drawer_mast.aspx">Drawer</a></li>
                                        <li><a href="folder_mast.aspx">Folder</a></li>
                                        <li><a href="doc_type_mast.aspx">Document Type</a></li>
                                        <li><a href="BlankTempUpload.aspx">New Template Upload</a></li>
                                        <li><a class="nob" href="doc_mast.aspx">New Document Upload</a></li>
                                        <%--<li><a href="grp_mast.aspx">Groups</a></li>
                                        <li><a class="nob" href="grp_doc.aspx">Group-wise Document</a></li>--%>
                                    </ul>
                                </li>
                                <li class="drop_nav"><a href="#">System</a>
                                    <ul class="drop hide">
                                        <li class="drop_nav1"><a href="#">User</a>
                                            <ul class="drop1 hide">
                                                <li><a href="dept_mast.aspx">Department</a></li>
                                                <li><a href="role_mast.aspx">Role</a></li>
                                                <li><a href="user_mast.aspx">New User</a></li>
                                                <li><a href="user_rights.aspx">User Rights</a></li>
                                                <li><a href="user_role.aspx" class="nob">User-wise Role Mapping</a></li>
                                            </ul>
                                        </li>
                                        <li><a href="workflow_mast.aspx">Workflow Definition</a></li>
                                        <li><a href="reset_pwd.aspx">Reset Password</a></li>
                                        <li><a class="nob" href="Billing.aspx">Statistics</a></li>
                                    </ul>
                                </li>
                                <li class="no"><a href="logout.aspx">Logout</a></li>
                            </ul>
                        </div>
                    </div>
                    <div class="dropdown" id="divMenuNormal" runat="server">
                        <div class="menu_bg_n">
                            <ul>
                                <li id="menuGenHome" runat="server"><a href="userhome.aspx">Home</a></li>
                                <li class="drop_nav"><a href="#">Filing</a>
                                <ul class="drop hide">
                                    <li><a href="home.aspx">e-Filing system</a></li>
                                    <li><a href="cabinet_mast.aspx">Cabinet</a></li>
                                    <li><a href="drawer_mast.aspx">Drawer</a></li>
                                    <li><a href="folder_mast.aspx">Folder</a></li>
                                    <li><a class="nob" href="doc_mast.aspx">New Document Upload</a></li>
                                </ul>
                                </li>
                                <li id="menuGenSystem" runat="server" class="drop_nav"><a href="#">System</a>
                                    <ul class="drop hide">
                                        <li><a class="nob" href="reset_pwd.aspx">Reset Password</a></li>
                                    </ul>
                                </li>
                                <li class="no"><a href="logout.aspx">Logout</a></li>
                            </ul>
                        </div>
                    </div>
                </div>
            </div>
            <div style="padding-bottom:0px;" class="normal_common">
                <div class="wf_mas_Heading2">
                    Reset Password
                </div>
            </div>
            <div class="normal_common">
                <div class="wf_mas_doc_holder" style="min-height:300px;">
                    <%--Top Panel--%>
                    <div class="wf_mas_doc_holder_padd">
                        <div class="normal_common">
                            <span class="wf_mas_cap">Email ID</span>
                            <asp:TextBox ID="txtUserID" MaxLength="20" ReadOnly="true" runat="server" CssClass="wf_mas_field"></asp:TextBox>
                        </div>
                        <div class="normal_common">
                            <span class="wf_mas_cap">Old Password</span>
                            <asp:TextBox ID="txtOPwd" runat="server" TextMode="Password" CssClass="wf_mas_field"></asp:TextBox>
                            <span style="padding-left:5px; float:left; font-weight:bold; font-size:24px; color:Red;">*</span>
                        </div>
                        <div class="normal_common">
                            <span class="wf_mas_cap">New Password</span>
                            <asp:TextBox ID="txtNPwd" runat="server" TextMode="Password" CssClass="wf_mas_field"></asp:TextBox>
                            <span style="padding-left:5px; float:left; font-weight:bold; font-size:24px; color:Red;">*</span>
                        </div>
                        <div class="normal_common">
                            <span class="wf_mas_cap">Confirm Password</span>
                            <asp:TextBox ID="txtCPwd" runat="server" TextMode="Password" CssClass="wf_mas_field"></asp:TextBox>
                            <span style="padding-left:5px; float:left; font-weight:bold; font-size:24px; color:Red;">*</span>
                            <span class="wf_det_add_holder">
                                <asp:Button ID="cmdReset" runat="server" CssClass="wf_mas_add" Text="Change" onclick="Reset_Click" />
                                <span style="padding-left:20px; float:left;"><asp:Button ID="cmdSkipNow" runat="server" CssClass="wf_mas_add" Text="Skip Now" onclick="cmdSkipNow_Click" /></span>
                            </span>
                        </div>
                    </div>
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
