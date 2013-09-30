<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="userhome.aspx.cs" Inherits="DMS.userhome" %>

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
        function showWFHistLoading() {
            document.getElementById("divWFHistLoading").style.display = 'block';
            document.getElementById("divWFHist").style.display = 'block';
        }
        function hideWFHistLoading() {
            document.getElementById("divWFHistLoading").style.display = 'none';
            document.getElementById("divWFHist").style.display = 'none';
        }
        function showRejectHist() {
            document.getElementById("divRejectHistLoading").style.display = 'block';
            document.getElementById("divRejectHist").style.display = 'block';
        }
        function hideRejectHist() {
            document.getElementById("divRejectHistLoading").style.display = 'none';
            document.getElementById("divRejectHist").style.display = 'none';
        }
        function showLoading() {
            document.getElementById("dvLoading1").style.display = 'block';
            document.getElementById("dvLoading2").style.display = 'block';
        }
        function hideLoading() {
            document.getElementById("dvLoading1").style.display = 'none';
            document.getElementById("dvLoading2").style.display = 'none';
            return true;
        }
    </script>
    <script type="text/javascript">
        $(document).ready(function () {
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
            function EndRequestHandler(sender, args) {
                if (document.getElementById("<%= hfMsg.ClientID %>").value != undefined && document.getElementById("<%= hfMsg.ClientID %>").value != "") {
                    alert(document.getElementById("<%= hfMsg.ClientID %>").value);
                    document.getElementById("<%= hfMsg.ClientID %>").value = "";
                }
            }

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
                $(this).children('ul').removeClass('g_show');
                $(this).children('ul').addClass('g_hide');
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
        function StartOnloading() {
            hideLoading();
            startTimer();
        }
        function RecentDocs(TempDocName) {
            document.getElementById("<%= hfTempDocName.ClientID %>").value =TempDocName;
            var jsVar = "RecentDocs";
            __doPostBack1('callPostBack2', jsVar);
        }
        function __doPostBack1(eventTarget2, eventArgument2) {
            document.form1.__EVENTTARGET1.value = "";
            document.form1.__EVENTARGUMENT1.value = "";
            document.form1.__EVENTTARGET2.value = eventTarget2;
            document.form1.__EVENTARGUMENT2.value = eventArgument2;
            document.form1.submit();
        }
        window.onload = StartOnloading;
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div class="header">
            <div class="main">
                <div class="logo_bg"><%--<a href="#"><img src="images/logo.jpg" width="286" height="88" alt="" border="0" /></a>--%></div>
                <h1><a href="userhome.aspx" style="border:none; text-decoration:none; outline:none;"><img src="images/product_logo.png" border="0" alt="" /></a></h1>
            </div>
        </div>
        <div>
            <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
            <%--For Session Expired Start--%>
            <div id="ExtendSessionLifeBox2" style="width:100%; z-index:10000; height:100%; opacity:.2; filter:alpha(opacity=20); background:#660066; display:none;">
            </div>
            <asp:Panel ID="ExtendSessionLifeBox" runat="server" style="height:100px; z-index:10001; width:250px; background-color:#660066; padding:20px; color:#fff; border:solid 1px #666; display:none; position:absolute;">
                <div style="width:100%; float:left">
                    <asp:Label ID="ExtendSessionLifePrompt" runat="server" Text="Your session is going to expire in 10 minutes. Would you like to extend your Session for another 60 minutes?"></asp:Label>
                </div>
                <div style="width:100%; float:left">
                    <div id="countDown"></div>
                </div>
                <div style="width:100%; float:left; padding-top:20px;">
                    <div style="float:left; padding-right:20px;">
                        <asp:UpdatePanel ID="up1" runat="server" >
                            <ContentTemplate>
                                <asp:Button ID="ExtendSessionLife" runat="server" Text="Yes" OnClientClick="startTimer(); return true;"/>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                    <div style="float:left">
                        <input type="button" id="CancelExtendSessionLife" value="No" onclick="CloseExtendSessionLifeBox(); return false;" />  
                    </div>
                </div>
            </asp:Panel>
            <%--For Session Expired End--%>
            <div class="main_body">
                <asp:HiddenField ID="hfTempDocName" runat="server" />
                <input type ="hidden" name ="__EVENTTARGET1" value ="" />
                <input type ="hidden" name ="__EVENTARGUMENT1" value ="" />
                <input type ="hidden" name ="__EVENTTARGET2" value ="" />
                <input type ="hidden" name ="__EVENTARGUMENT2" value ="" />
                <div class="main">
                    <div class="normal_common">
                        <div class="wf_mas_Heading"><span>Dashboard for </span><asp:Label ID="lblUser" runat="server" Text="User Name"></asp:Label>
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
                    <div class="normal_common">
                        <div class="wf_mas_doc_holder" style="min-height:300px;">
                            <%--Top Panel--%>
                            <div class="wf_mas_doc_holder_padd">
                                <%--To Do List Panel Start--%>
                                <div style="float:left; width:48%;">
                                    <div class="normal_common">
                                        <div style="border:1px solid #999; float:left; width:100%; -moz-border-radius:3px; -webkit-border-radius:3px; border-radius:3px;">
                                            <div style="border-bottom:1px solid #999; padding:0px;" class="normal_common">
                                                <div style="padding:5px; font-size:14px; color:#660066; font-weight:bold; background:#f0eded;">
                                                    Waiting for My Action
                                                </div>
                                            </div>
                                            <div class="normal_common">
                                                <%--Left GridView Holder Panel Start--%>
                                                <div style="padding:5px; overflow:auto; height:250px;">
                                                    <asp:UpdatePanel ID="UpdatePanel11" runat="server" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <asp:GridView ID="gvToDo" runat="server" style="border:1px solid #000;" Width="600px" BackColor="White" CellPadding="4" 
                                                        ForeColor="Black" Font-Size="11px" GridLines="Vertical" AutoGenerateColumns="False" CaptionAlign="Top" DataKeyNames="wf_log_id,wf_name,step_no,assign_dt,due_dt,started_by"
                                                        EnableModelValidation="True" onselectedindexchanged="gvToDo_SelectedIndexChanged">
                                                        <RowStyle BackColor="#F7F7DE" HorizontalAlign="Center" />
                                                        <FooterStyle BackColor="#CCCC99" />
                                                        <PagerStyle BackColor="#F7F7DE" ForeColor="Black" />
                                                        <SelectedRowStyle BackColor="#dae2ea" Font-Bold="True" ForeColor="Blue" HorizontalAlign="Center" />
                                                        <HeaderStyle BackColor="#cccccc" Font-Bold="True" HorizontalAlign="Center" />
                                                        <AlternatingRowStyle BackColor="White" />
                                                        <Columns>
                                                        <asp:TemplateField Visible="False">
                                                        <ItemTemplate >
                                                        <asp:Label ID="lbAutoID" runat="server" Text='<%#Eval("wf_log_id")%>'></asp:Label>
                                                        </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField Visible="False">
                                                        <ItemTemplate >
                                                        <asp:Label ID="lbWFName" runat="server" Text='<%#Eval("wf_name")%>'></asp:Label>
                                                        </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Document" HeaderStyle-Width="200" ItemStyle-Width="200">  
                                                        <ItemTemplate >
                                                            <%--<asp:LinkButton ID="lnkTask" CommandName="Select" CommandArgument="ShowTask" runat="server" Text='<%#Eval("wf_name")%>'></asp:LinkButton>--%>
                                                            <asp:LinkButton ID="lnkTask" CommandName="Select" CommandArgument="ShowTask" runat="server" Text='<%#Eval("doc_name")%>'></asp:LinkButton>
                                                        </ItemTemplate>
                                                        <HeaderStyle Width="200px" HorizontalAlign="Left" CssClass="GVRec"></HeaderStyle>
                                                        <ItemStyle Width="200px" HorizontalAlign="Left" CssClass="GVRec"></ItemStyle>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField Visible="False">
                                                        <ItemTemplate >
                                                        <asp:Label ID="lbStepNo" runat="server" Text='<%#Eval("step_no")%>'></asp:Label>
                                                        </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Step No" Visible="false">  
                                                        <ItemTemplate><%#Eval("step_no")%></ItemTemplate>
                                                        <HeaderStyle HorizontalAlign="Left" CssClass="GVRec"></HeaderStyle>
                                                        <ItemStyle HorizontalAlign="Left" CssClass="GVRec"></ItemStyle>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField Visible="False">
                                                        <ItemTemplate >
                                                        <asp:Label ID="lbDueDt" runat="server" Text='<%#Eval("due_dt")%>'></asp:Label>
                                                        </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Due Date" HeaderStyle-Width="175" ItemStyle-Width="175">  
                                                        <ItemTemplate ><%#Eval("due_dt")%></ItemTemplate>
                                                        <HeaderStyle Width="175px" HorizontalAlign="Left" CssClass="GVRec"></HeaderStyle>
                                                        <ItemStyle Width="175px" HorizontalAlign="Left" CssClass="GVRec"></ItemStyle>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField Visible="False">
                                                        <ItemTemplate >
                                                        <asp:Label ID="lbAssignedBy" runat="server" Text='<%#Eval("started_by")%>'></asp:Label>
                                                        </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Queued By" HeaderStyle-Width="100" ItemStyle-Width="100" >  
                                                        <ItemTemplate><%#Eval("started_by")%></ItemTemplate>
                                                        <HeaderStyle Width="100px" HorizontalAlign="Left" CssClass="GVRec"></HeaderStyle>
                                                        <ItemStyle Width="100px" HorizontalAlign="Left" CssClass="GVRec"></ItemStyle>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField Visible="False">
                                                        <ItemTemplate >
                                                        <asp:Label ID="lbDocID" runat="server" Text='<%#Eval("doc_id")%>'></asp:Label>
                                                        </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Doc ID" Visible="False" >  
                                                        <ItemTemplate><%#Eval("doc_id")%></ItemTemplate>
                                                        <HeaderStyle Width="100px" HorizontalAlign="Left" CssClass="GVRec"></HeaderStyle>
                                                        <ItemStyle Width="100px" HorizontalAlign="Left" CssClass="GVRec"></ItemStyle>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField Visible="False">
                                                        <ItemTemplate >
                                                        <asp:Label ID="lbDocName" runat="server" Text='<%#Eval("wf_name")%>'></asp:Label>
                                                        </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Doc Name" Visible="False" >  
                                                        <ItemTemplate><%#Eval("doc_name")%></ItemTemplate>
                                                        <HeaderStyle Width="100px" HorizontalAlign="Left" CssClass="GVRec"></HeaderStyle>
                                                        <ItemStyle Width="100px" HorizontalAlign="Left" CssClass="GVRec"></ItemStyle>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField Visible="False">
                                                        <ItemTemplate >
                                                        <asp:Label ID="lbDownloadPath" runat="server" Text='<%#Eval("download_path")%>'></asp:Label>
                                                        </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Download Path" Visible="False" >  
                                                        <ItemTemplate><%#Eval("download_path")%></ItemTemplate>
                                                        <HeaderStyle Width="100px" HorizontalAlign="Left" CssClass="GVRec"></HeaderStyle>
                                                        <ItemStyle Width="100px" HorizontalAlign="Left" CssClass="GVRec"></ItemStyle>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField Visible="False">
                                                        <ItemTemplate >
                                                        <asp:Label ID="lbAssignedDt" runat="server" Text='<%#Eval("assign_dt")%>'></asp:Label>
                                                        </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Queued Date" HeaderStyle-Width="175" ItemStyle-Width="175" >  
                                                        <ItemTemplate><%#Eval("assign_dt")%></ItemTemplate>
                                                        <HeaderStyle Width="175px" HorizontalAlign="Left" CssClass="GVRec"></HeaderStyle>
                                                        <ItemStyle Width="175px" HorizontalAlign="Left" CssClass="GVRec"></ItemStyle>
                                                        </asp:TemplateField>
                                                        </Columns>
                                                        </asp:GridView>
                                                    </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                                <%--Left GridView Holder Panel End--%>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <%--To Do List Panel End--%>
                        
                                <%--Recent Docs Panel Start--%>
                                <div style="float:right; width:48%;">
                                    <div class="normal_common">
                                        <div style="border:1px solid #999; float:left; width:100%; -moz-border-radius:3px; -webkit-border-radius:3px; border-radius:3px;">
                                            <div style="border-bottom:1px solid #999; padding:0px;" class="normal_common">
                                                <div style="padding:5px; font-size:14px; color:#660066; font-weight:bold; background:#f0eded;">
                                                    Incomplete Documents
                                                </div>
                                            </div>  
                                            <div class="normal_common">
                                                <div style="padding:5px; overflow:auto; height:250px;">
                                                    <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <asp:HiddenField ID="hfMsg" runat="server" />
                                                        <asp:GridView ID="gvRecentDocs" runat="server" style="border:1px solid #000;" Width="700px" BackColor="White" CellPadding="4" 
                                                        ForeColor="Black" Font-Size="11px" GridLines="Vertical" AutoGenerateColumns="False" CaptionAlign="Top" DataKeyNames="TempDocName"
                                                        EnableModelValidation="True" onrowdeleting="gvRecentDocs_RowDeleting" onselectedindexchanged="gvRecentDocs_SelectedIndexChanged" OnRowDataBound="gvRecentDocs_RowDataBound">
                                                        <RowStyle BackColor="#F7F7DE" HorizontalAlign="Center" />
                                                        <FooterStyle BackColor="#CCCC99" />
                                                        <PagerStyle BackColor="#F7F7DE" ForeColor="Black" />
                                                        <SelectedRowStyle BackColor="#dae2ea" Font-Bold="True" ForeColor="Blue" HorizontalAlign="Center" />
                                                        <HeaderStyle BackColor="#cccccc" Font-Bold="True" HorizontalAlign="Center" />
                                                        <AlternatingRowStyle BackColor="White" />
                                                        <Columns>
                                                        <asp:TemplateField Visible="false">
                                                        <ItemTemplate >
                                                        <asp:Label ID="lbTempDocName" runat="server" Text='<%#Eval("TempDocName")%>'></asp:Label>
                                                        </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Document Name">  
                                                        <ItemTemplate >
                                                            <asp:LinkButton ID="lnkTempDocName" CommandName="Select" CommandArgument="OpenRecentDoc" runat="server" Text='<%#Eval("TempDocName")%>'></asp:LinkButton>
                                                        </ItemTemplate>
                                                        <HeaderStyle HorizontalAlign="Left" CssClass="GVRec"></HeaderStyle>
                                                        <ItemStyle HorizontalAlign="Left" CssClass="GVRec"></ItemStyle>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Document Type">  
                                                        <ItemTemplate><%#Eval("doc_type_name")%></ItemTemplate>
                                                        <HeaderStyle HorizontalAlign="Left" CssClass="GVRec"></HeaderStyle>
                                                        <ItemStyle HorizontalAlign="Left" CssClass="GVRec"></ItemStyle>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Creation Date">  
                                                        <ItemTemplate><%#Eval("CreationDate")%></ItemTemplate>
                                                        <HeaderStyle HorizontalAlign="Left" CssClass="GVRec"></HeaderStyle>
                                                        <ItemStyle HorizontalAlign="Left" CssClass="GVRec"></ItemStyle>
                                                        </asp:TemplateField>
                                                        <asp:CommandField HeaderText="Action" ShowDeleteButton="True" ShowEditButton="false" />
                                                        </Columns>
                                                        </asp:GridView>
                                                    </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <%--Recent Docs Panel End--%>

                                <%--Workflow in Progress Panel Start--%>
                                <div style="float:left; width:48%;">
                                    <div class="normal_common">
                                        <div style="border:1px solid #999; float:left; width:100%; -moz-border-radius:3px; -webkit-border-radius:3px; border-radius:3px;">
                                            <div style="border-bottom:1px solid #999; padding:0px;" class="normal_common">
                                                <div style="padding:5px; font-size:14px; color:#660066; font-weight:bold; background:#f0eded;">
                                                    My Workflows in Progress
                                                </div>
                                            </div>
                                            <div class="normal_common">
                                                <div style="padding:5px; overflow:auto; height:250px;">
                                                    <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Always">
                                                    <ContentTemplate>
                                                        <asp:GridView ID="gvStartedWF" runat="server" style="border:1px solid #000;" Width="425px" BackColor="White" CellPadding="4" 
                                                        ForeColor="Black" Font-Size="11px" GridLines="Vertical" AutoGenerateColumns="False" CaptionAlign="Top" DataKeyNames="wf_log_id,wf_name"
                                                        EnableModelValidation="True" onselectedindexchanged="gvStartedWF_SelectedIndexChanged">
                                                        <RowStyle BackColor="#F7F7DE" HorizontalAlign="Center" />
                                                        <FooterStyle BackColor="#CCCC99" />
                                                        <PagerStyle BackColor="#F7F7DE" ForeColor="Black" />
                                                        <SelectedRowStyle BackColor="#dae2ea" Font-Bold="True" ForeColor="Blue" HorizontalAlign="Center" />
                                                        <HeaderStyle BackColor="#cccccc" Font-Bold="True" HorizontalAlign="Center" />
                                                        <AlternatingRowStyle BackColor="White" />
                                                        <Columns>
                                                        <asp:TemplateField Visible="false">
                                                        <ItemTemplate >
                                                        <asp:Label ID="lbWFLogID" runat="server" Text='<%#Eval("wf_log_id")%>'></asp:Label>
                                                        </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Workflow ID" Visible="false">  
                                                        <ItemTemplate><%#Eval("wf_log_id")%></ItemTemplate>
                                                        <HeaderStyle HorizontalAlign="Left" CssClass="GVRec"></HeaderStyle>
                                                        <ItemStyle HorizontalAlign="Left" CssClass="GVRec"></ItemStyle>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField Visible="False">
                                                        <ItemTemplate>
                                                        <asp:Label ID="lbWFID" runat="server" Text='<%#Eval("wf_id")%>'></asp:Label>
                                                        </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Workflow ID" Visible="false">  
                                                        <ItemTemplate><%#Eval("wf_id")%></ItemTemplate>
                                                        <HeaderStyle HorizontalAlign="Left" CssClass="GVRec"></HeaderStyle>
                                                        <ItemStyle HorizontalAlign="Left" CssClass="GVRec"></ItemStyle>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField Visible="False">
                                                        <ItemTemplate >
                                                        <asp:Label ID="lbWFName" runat="server" Text='<%#Eval("wf_name")%>'></asp:Label>
                                                        </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Document">  
                                                        <ItemTemplate >
                                                            <%--<asp:LinkButton ID="lnkTask" CommandName="Select" OnClientClick="javascript: return showWFHistLoading()" CommandArgument="ShowHist" runat="server" Text='<%#Eval("wf_name")%>'></asp:LinkButton>--%>
                                                            <asp:LinkButton ID="lnkTask" CommandName="Select" OnClientClick="javascript: return showWFHistLoading()" CommandArgument="ShowHist" runat="server" Text='<%#Eval("doc_name")%>'></asp:LinkButton>
                                                        </ItemTemplate>
                                                        <HeaderStyle HorizontalAlign="Left" CssClass="GVRec"></HeaderStyle>
                                                        <ItemStyle HorizontalAlign="Left" CssClass="GVRec"></ItemStyle>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField Visible="False">
                                                        <ItemTemplate >
                                                        <asp:Label ID="lbDocID" runat="server" Text='<%#Eval("doc_id")%>'></asp:Label>
                                                        </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Doc ID" Visible="false">  
                                                        <ItemTemplate><%#Eval("doc_id")%></ItemTemplate>
                                                        <HeaderStyle HorizontalAlign="Left" CssClass="GVRec"></HeaderStyle>
                                                        <ItemStyle HorizontalAlign="Left" CssClass="GVRec"></ItemStyle>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField Visible="False">
                                                        <ItemTemplate >
                                                        <asp:Label ID="lbAssignedDt" runat="server" Text='<%#Eval("doc_name")%>'></asp:Label>
                                                        </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Workflow Name" Visible="false" >  
                                                        <ItemTemplate><%#Eval("wf_name")%></ItemTemplate>
                                                        <HeaderStyle HorizontalAlign="Left" CssClass="GVRec"></HeaderStyle>
                                                        <ItemStyle HorizontalAlign="Left" CssClass="GVRec"></ItemStyle>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField Visible="False">
                                                        <ItemTemplate >
                                                        <asp:Label ID="lbStat" runat="server" Text='<%#Eval("wf_prog_stat")%>'></asp:Label>
                                                        </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Progress Status" >  
                                                        <ItemTemplate><%#Eval("wf_prog_stat")%></ItemTemplate>
                                                        <HeaderStyle HorizontalAlign="Left" CssClass="GVRec" Width="100"></HeaderStyle>
                                                        <ItemStyle HorizontalAlign="Left" CssClass="GVRec" Width="100"></ItemStyle>
                                                        </asp:TemplateField>
                                                        </Columns>
                                                        </asp:GridView>
                                                    </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <%--Workflow in Progress Panel End--%>
                        
                                <%--Completed Jobs / Workflows Panel Start--%>
                                <div style="float:right; width:48%;">
                                    <div class="normal_common">
                                        <div style="border:1px solid #999; float:left; width:100%; -moz-border-radius:3px; -webkit-border-radius:3px; border-radius:3px;">
                                            <div style="border-bottom:1px solid #999; padding:0px;" class="normal_common">
                                                <div style="padding:5px; font-size:14px; color:#660066; font-weight:bold; background:#f0eded;">
                                                    My Completed Workflows
                                                </div>
                                            </div>
                                            <div class="normal_common">
                                                <div style="padding:5px; overflow:auto; height:250px;">
                                                    <asp:UpdatePanel ID="UpdatePanel5" runat="server" UpdateMode="Always">
                                                        <ContentTemplate>
                                                        <asp:GridView ID="gvCompleted" runat="server" Width="600px" style="border:1px solid #000;" BackColor="White" CellPadding="4" 
                                                        ForeColor="Black" Font-Size="11px" GridLines="Vertical" AutoGenerateColumns="False" CaptionAlign="Top" 
                                                        EnableModelValidation="True" onselectedindexchanged="gvCompleted_SelectedIndexChanged">
                                                        <RowStyle BackColor="#F7F7DE" HorizontalAlign="Center" />
                                                        <FooterStyle BackColor="#CCCC99" />
                                                        <PagerStyle BackColor="#F7F7DE" ForeColor="Black" />
                                                        <SelectedRowStyle BackColor="#dae2ea" Font-Bold="True" ForeColor="Blue" HorizontalAlign="Center" />
                                                        <HeaderStyle BackColor="#cccccc" Font-Bold="True" HorizontalAlign="Center" />
                                                        <AlternatingRowStyle BackColor="White" />
                                                        <Columns>
                                                        <asp:TemplateField Visible="False">
                                                        <ItemTemplate >
                                                        <asp:Label ID="lbAutoID" runat="server" Text='<%#Eval("wf_log_id")%>'></asp:Label>
                                                        </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField Visible="false">
                                                        <ItemTemplate >
                                                        <asp:Label ID="lbWFLogID" runat="server" Text='<%#Eval("wf_log_id")%>'></asp:Label>
                                                        </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Document" HeaderStyle-Width="260" ItemStyle-Width="260" Visible="false">  
                                                        <ItemTemplate><%#Eval("doc_name")%></ItemTemplate>
                                                        <HeaderStyle Width="260px" HorizontalAlign="Left" CssClass="GVRec"></HeaderStyle>
                                                        <ItemStyle Width="260px" HorizontalAlign="Left" CssClass="GVRec"></ItemStyle>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Document">  
                                                            <ItemTemplate >
                                                                <asp:LinkButton ID="lnkTask1" CommandName="Select" OnClientClick="javascript: return showWFHistLoading()" CommandArgument="ShowHist" runat="server" Text='<%#Eval("doc_name")%>'></asp:LinkButton>
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Left" CssClass="GVRec"></HeaderStyle>
                                                            <ItemStyle HorizontalAlign="Left" CssClass="GVRec"></ItemStyle>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Queued Date" HeaderStyle-Width="175" ItemStyle-Width="175">  
                                                        <ItemTemplate><%#Eval("assign_dt")%></ItemTemplate>
                                                        <HeaderStyle Width="175px" HorizontalAlign="Left" CssClass="GVRec"></HeaderStyle>
                                                        <ItemStyle Width="175px" HorizontalAlign="Left" CssClass="GVRec"></ItemStyle>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Completion Date" HeaderStyle-Width="175" ItemStyle-Width="175">  
                                                        <ItemTemplate ><%#Eval("actual_completed_dt")%></ItemTemplate>
                                                        <HeaderStyle Width="175px" HorizontalAlign="Left" CssClass="GVRec"></HeaderStyle>
                                                        <ItemStyle Width="175px" HorizontalAlign="Left" CssClass="GVRec"></ItemStyle>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Queued By" HeaderStyle-Width="100" ItemStyle-Width="100">  
                                                        <ItemTemplate><%#Eval("started_by")%></ItemTemplate>
                                                        <HeaderStyle Width="100px" HorizontalAlign="Left" CssClass="GVRec"></HeaderStyle>
                                                        <ItemStyle Width="100px" HorizontalAlign="Left" CssClass="GVRec"></ItemStyle>
                                                        </asp:TemplateField>
                                                        </Columns>
                                                        </asp:GridView>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <%--Completed Jobs / Workflows Panel End--%>

                                <%--Rejected Jobs / Workflows Panel Start--%>
                                <div style="float:left; width:100%;">
                                    <div class="normal_common">
                                        <div style="border:1px solid #999; float:left; width:100%; -moz-border-radius:3px; -webkit-border-radius:3px; border-radius:3px;">
                                            <div style="border-bottom:1px solid #999; padding:0px;" class="normal_common">
                                                <div style="padding:5px; font-size:14px; color:#660066; font-weight:bold; background:#f0eded;">
                                                    Rejected Workflows
                                                </div>
                                            </div>
                                            <div class="normal_common">
                                                <div style="padding:5px; overflow:auto; height:200px;">
                                                    <asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Always">
                                                    <ContentTemplate>
                                                        <asp:GridView ID="gvRejectedWFL" runat="server" style="border:1px solid #000;" Width="915px" BackColor="White" CellPadding="4" 
                                                        ForeColor="Black" Font-Size="11px" GridLines="Vertical" AutoGenerateColumns="False" CaptionAlign="Top" DataKeyNames="wf_log_id,wf_name"
                                                        EnableModelValidation="True" onselectedindexchanged="gvRejectedWFL_SelectedIndexChanged">
                                                        <RowStyle BackColor="#F7F7DE" HorizontalAlign="Center" />
                                                        <FooterStyle BackColor="#CCCC99" />
                                                        <PagerStyle BackColor="#F7F7DE" ForeColor="Black" />
                                                        <SelectedRowStyle BackColor="#dae2ea" Font-Bold="True" ForeColor="Blue" HorizontalAlign="Center" />
                                                        <HeaderStyle BackColor="#cccccc" Font-Bold="True" HorizontalAlign="Center" />
                                                        <AlternatingRowStyle BackColor="White" />
                                                        <Columns>
                                                        <asp:TemplateField Visible="false">
                                                            <ItemTemplate >
                                                            <asp:Label ID="lbWFLogID" runat="server" Text='<%#Eval("wf_log_id")%>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Workflow ID" Visible="false">  
                                                            <ItemTemplate><%#Eval("wf_log_id")%></ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Left" CssClass="GVRec"></HeaderStyle>
                                                            <ItemStyle HorizontalAlign="Left" CssClass="GVRec"></ItemStyle>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField Visible="False">
                                                            <ItemTemplate>
                                                            <asp:Label ID="lbWFID1" runat="server" Text='<%#Eval("wf_id")%>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Workflow ID" Visible="false">  
                                                            <ItemTemplate><%#Eval("wf_id")%></ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Left" CssClass="GVRec"></HeaderStyle>
                                                            <ItemStyle HorizontalAlign="Left" CssClass="GVRec"></ItemStyle>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField Visible="False">
                                                            <ItemTemplate >
                                                            <asp:Label ID="lbWFName1" runat="server" Text='<%#Eval("wf_name")%>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <%--<asp:TemplateField HeaderText="Document">  
                                                            <ItemTemplate >
                                                                <asp:LinkButton ID="lnkReject" CommandName="Select" OnClientClick="javascript: return showRejectHist();" CommandArgument="ShowRejectHist" runat="server" Text='<%#Eval("doc_name")%>'></asp:LinkButton>
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Left" CssClass="GVRec"></HeaderStyle>
                                                            <ItemStyle HorizontalAlign="Left" CssClass="GVRec"></ItemStyle>
                                                        </asp:TemplateField>--%>
                                                        <asp:TemplateField HeaderText="Document">  
                                                            <ItemTemplate >
                                                                <asp:LinkButton ID="lnkTask2" CommandName="Select" OnClientClick="javascript: return showRejectHist()" CommandArgument="ShowHist" runat="server" Text='<%#Eval("doc_name")%>'></asp:LinkButton>
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Left" CssClass="GVRec"></HeaderStyle>
                                                            <ItemStyle HorizontalAlign="Left" CssClass="GVRec"></ItemStyle>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField Visible="False">
                                                            <ItemTemplate >
                                                            <asp:Label ID="lbDocID1" runat="server" Text='<%#Eval("doc_id")%>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Doc ID" Visible="false">  
                                                            <ItemTemplate><%#Eval("doc_id")%></ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Left" CssClass="GVRec"></HeaderStyle>
                                                            <ItemStyle HorizontalAlign="Left" CssClass="GVRec"></ItemStyle>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField Visible="False">
                                                            <ItemTemplate >
                                                            <asp:Label ID="lbAssignedDt1" runat="server" Text='<%#Eval("doc_name")%>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Workflow Name" Visible="false" >  
                                                            <ItemTemplate><%#Eval("wf_name")%></ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Left" CssClass="GVRec"></HeaderStyle>
                                                            <ItemStyle HorizontalAlign="Left" CssClass="GVRec"></ItemStyle>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField Visible="False">
                                                            <ItemTemplate >
                                                            <asp:Label ID="lbStat1" runat="server" Text='<%#Eval("wf_prog_stat")%>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Document" Visible="false">  
                                                            <ItemTemplate><%#Eval("doc_name")%></ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Left" CssClass="GVRec"></HeaderStyle>
                                                            <ItemStyle HorizontalAlign="Left" CssClass="GVRec"></ItemStyle>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Queued Date" >  
                                                            <ItemTemplate><%#Eval("start_dt")%></ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Left" CssClass="GVRec" Width="150"></HeaderStyle>
                                                            <ItemStyle HorizontalAlign="Left" CssClass="GVRec" Width="150"></ItemStyle>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Queued By" >  
                                                            <ItemTemplate><%#Eval("started_by")%></ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Left" CssClass="GVRec" Width="100"></HeaderStyle>
                                                            <ItemStyle HorizontalAlign="Left" CssClass="GVRec" Width="100"></ItemStyle>
                                                        </asp:TemplateField>
                                                        </Columns>
                                                        </asp:GridView>
                                                    </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <%--Rejected Jobs / Workflows Panel End--%>
                                
                                <%--Workflow History Start--%>
                                <div id="divWFHistLoading" style="display:none;"></div>
                                <div id="divWFHist" style="display:none;">
                                    <div class="lightbox_1">
                                        <div class="lightbox_2">
                                            <asp:UpdatePanel ID="UpdatePanel22" runat="server" UpdateMode="Always">
                                            <ContentTemplate>
                                            <div class="lightbox_3">
                                                <div class="normal_feature">
                                                    <div style="width:100%; float:left; padding-top:20px;">
                                                        <asp:GridView style="border:1px solid #000;" Width="100%" BackColor="White" CellPadding="4" ForeColor="Black" ID="gv" runat="server">
                                                        <RowStyle BackColor="#F7F7DE" HorizontalAlign="Center"/>
                                                        <FooterStyle BackColor="#CCCC99"/>
                                                        <PagerStyle BackColor="#F7F7DE" ForeColor="Black"/>
                                                        <SelectedRowStyle BackColor="#CE5D5A" Font-Bold="True" ForeColor="White" HorizontalAlign="Center"/>
                                                        <HeaderStyle BackColor="#cccccc" Font-Bold="True" HorizontalAlign="Center"/>
                                                        <AlternatingRowStyle BackColor="White"/>
                                                        </asp:GridView>
                                                    </div>
                                                    <div class="normal_feature">
                                                        <div style="color:#FF0000; text-align:center; font-weight:bold; font-size:16px; background:#000000; border:1px solid #999999; padding:10px;">
                                                            <asp:Label ID="MsgNodet" runat="server"></asp:Label> 
                                                        </div>
                                                    </div>
                                                    <div class="normal_feature" style="text-align:right;">                         
                                                        <input type="button" id="ImageButton1" class="padd_marg1" style="background:url('images/add_btn3.png'); width:153px; height:34px;" onclick="javascript: return hideWFHistLoading();" />
                                                    </div>
                                            </ContentTemplate>
                                            </asp:UpdatePanel> 
                                        </div>
                                    </div>
                                </div>
                                <%--Workflow History End--%>

                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <div class="footer_main">
            <div class="footer_text">Powered by ADA modeling technology</div>
        </div>

        <%--Reject History Start--%>
        <div id="divRejectHistLoading" style="display:none;"></div>
        <div id="divRejectHist" style="display:none;">
            <div class="lightbox_1">
                <div class="lightbox_2">
                    <asp:UpdatePanel ID="UpdatePanel41" runat="server" UpdateMode="Always">
                    <ContentTemplate>
                    <div class="lightbox_3">
                        <div class="normal_feature">
                            <div style="width:100%; float:left; padding-top:20px;">
                                <asp:GridView style="border:1px solid #000;" Width="100%" BackColor="White" CellPadding="4" ForeColor="Black" ID="gvRejectHist" runat="server">
                                <RowStyle BackColor="#F7F7DE" HorizontalAlign="Center"/>
                                <FooterStyle BackColor="#CCCC99"/>
                                <PagerStyle BackColor="#F7F7DE" ForeColor="Black"/>
                                <SelectedRowStyle BackColor="#CE5D5A" Font-Bold="True" ForeColor="White" HorizontalAlign="Center"/>
                                <HeaderStyle BackColor="#cccccc" Font-Bold="True" HorizontalAlign="Center"/>
                                <AlternatingRowStyle BackColor="White"/>
                                </asp:GridView>
                            </div>
                            <div class="normal_feature">
                                <div style="color:#FF0000; text-align:center; font-weight:bold; font-size:16px; background:#000000; border:1px solid #999999; padding:10px;">
                                    <asp:Label ID="MsgNodet1" runat="server"></asp:Label> 
                                </div>
                            </div>
                            <div class="normal_feature" style="text-align:right;">                         
                                <input type="button" id="Button1" class="padd_marg1" style="background:url('images/add_btn3.png'); width:153px; height:34px;" onclick="javascript: return hideRejectHist();" />
                            </div>
                        </div>
                    </div>
                    </ContentTemplate>
                    </asp:UpdatePanel> 
                </div>
            </div>
        </div>
        <%--Reject History End--%>
    </form>
</body>
</html>