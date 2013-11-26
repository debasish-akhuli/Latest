<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="search_list.aspx.cs" Inherits="DMS.search_list" %>

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
        function OpenDoc(DocID) {
            var jsVar = "OpenDoc";
            document.getElementById("<%= hfSelDocID.ClientID %>").value = DocID.toString();
            __doPostBack5('callPostBack5', jsVar);
        }
        function __doPostBack5(eventTarget2, eventArgument2) {
            document.form1.__EVENTTARGET1.value = "";
            document.form1.__EVENTARGUMENT1.value = "";
            document.form1.__EVENTTARGET2.value = eventTarget2;
            document.form1.__EVENTARGUMENT2.value = eventArgument2;
            document.form1.submit();
        }
        function showLoading() {
            document.getElementById("dvLoading1").style.display = 'block';
            document.getElementById("dvLoading2").style.display = 'block';
        }
        function hideLoading() {
            document.getElementById("dvLoading1").style.display = 'none';
            document.getElementById("dvLoading2").style.display = 'none';
        }

        $(document).ready(function () {
            $("a.close").hide();

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

        });

        window.onload = hideLoading;
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
    <div style=" float:left">
    <input type="button" id="CancelExtendSessionLife" value="No" onclick="CloseExtendSessionLifeBox(); return false;" />  
    </div>
    </div>
    </asp:Panel>
<%--For Session Expired End--%>
    <asp:UpdateProgress ID="UpdateProgress1" runat="server">
        <ProgressTemplate>
            <div style="position:fixed; width:1019px; height:100%; background:#999; opacity:.3; filter:alpha(opacity=30); z-index:103;"></div>
            <div style="width:100px; height:100px; position:absolute; top:50%; left:47%; z-index:104;"><asp:Image runat="server" ID="imgBusy" ImageUrl="images/busy.gif" /></div>
        </ProgressTemplate>
    </asp:UpdateProgress>
    <div class="main_body">
        <div class="main">
            <asp:HiddenField ID="hfSelDocID" runat="server" />
            <input type="hidden" name="__EVENTTARGET1" value="" />
            <input type="hidden" name="__EVENTARGUMENT1" value="" />
            <input type="hidden" name="__EVENTTARGET2" value="" />
            <input type="hidden" name="__EVENTARGUMENT2" value="" />
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
                                        <li><a href="eFormCreation.aspx">New eForm Creation</a></li>
                                        <li><a class="nob" href="doc_mast.aspx">New Document Upload</a></li>
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
                                        <li><a href="eFormCreation.aspx">New eForm Creation</a></li>
                                        <li><a class="nob" href="doc_mast.aspx">New Document Upload</a></li>
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
                                    <li><a href="eFormCreation.aspx">New eForm Creation</a></li>
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
                            <asp:UpdatePanel runat="server" id="UpdatePanel1">
                            <ContentTemplate>
                            <div class="normal_common" id="divCompany" runat="server">
                                <span class="wf_mas_cap">Company</span>
                                <asp:DropDownList ID="ddCompany" CssClass="wf_det_field2_big" runat="server" AutoPostBack="true" onselectedindexchanged="ddCompany_SelectedIndexChanged"></asp:DropDownList>
                            </div>
                            <div class="normal_common">
                                <span class="wf_mas_cap">Document Type</span>
                                <asp:DropDownList ID="ddDocType" CssClass="wf_det_field2_big" runat="server" AutoPostBack="true" onselectedindexchanged="ddDocType_SelectedIndexChanged"></asp:DropDownList>
                            </div>
                            </ContentTemplate>
                            </asp:UpdatePanel>
                            <div class="normal_common">
                                <span class="wf_mas_cap">Document Name</span>
                                <asp:TextBox ID="txtDocName" runat="server" CssClass="wf_mas_field"></asp:TextBox>
                            </div>
                            <asp:UpdatePanel runat="server" id="UpdatePanel2">
                            <ContentTemplate>
                            <div class="normal_common" id="divTag1" runat="server">
                                <span class="wf_mas_cap"><asp:Label ID="lblTag1" Text="Tag1" runat="server"></asp:Label></span>
                                <asp:TextBox ID="txtTag1" runat="server" CssClass="wf_mas_field"></asp:TextBox>
                            </div>
                            <div class="normal_common" id="divTag2" runat="server">
                                <span class="wf_mas_cap"><asp:Label ID="lblTag2" Text="Tag2" runat="server"></asp:Label></span>
                                <asp:TextBox ID="txtTag2" runat="server" CssClass="wf_mas_field"></asp:TextBox>
                            </div>                            
                            <div class="normal_common" id="divTag3" runat="server">
                                <span class="wf_mas_cap"><asp:Label ID="lblTag3" Text="Tag3" runat="server"></asp:Label></span>
                                <asp:TextBox ID="txtTag3" runat="server" CssClass="wf_mas_field"></asp:TextBox>
                            </div>
                            <div class="normal_common" id="divTag4" runat="server">
                                <span class="wf_mas_cap"><asp:Label ID="lblTag4" Text="Tag4" runat="server"></asp:Label></span>
                                <asp:TextBox ID="txtTag4" runat="server" CssClass="wf_mas_field"></asp:TextBox>
                            </div>
                            <div class="normal_common" id="divTag5" runat="server">
                                <span class="wf_mas_cap"><asp:Label ID="lblTag5" Text="Tag5" runat="server"></asp:Label></span>
                                <asp:TextBox ID="txtTag5" runat="server" CssClass="wf_mas_field"></asp:TextBox>
                            </div>
                            <div class="normal_common" id="divTag6" runat="server">
                                <span class="wf_mas_cap"><asp:Label ID="lblTag6" Text="Tag6" runat="server"></asp:Label></span>
                                <asp:TextBox ID="txtTag6" runat="server" CssClass="wf_mas_field"></asp:TextBox>
                            </div>
                            <div class="normal_common" id="divTag7" runat="server">
                                <span class="wf_mas_cap"><asp:Label ID="lblTag7" Text="Tag7" runat="server"></asp:Label></span>
                                <asp:TextBox ID="txtTag7" runat="server" CssClass="wf_mas_field"></asp:TextBox>
                            </div>
                            <div class="normal_common" id="divTag8" runat="server">
                                <span class="wf_mas_cap"><asp:Label ID="lblTag8" Text="Tag8" runat="server"></asp:Label></span>
                                <asp:TextBox ID="txtTag8" runat="server" CssClass="wf_mas_field"></asp:TextBox>
                            </div>
                            <div class="normal_common" id="divTag9" runat="server">
                                <span class="wf_mas_cap"><asp:Label ID="lblTag9" Text="Tag9" runat="server"></asp:Label></span>
                                <asp:TextBox ID="txtTag9" runat="server" CssClass="wf_mas_field"></asp:TextBox>
                            </div>
                            <div class="normal_common" id="divTag10" runat="server">
                                <span class="wf_mas_cap"><asp:Label ID="lblTag10" Text="Tag10" runat="server"></asp:Label></span>
                                <asp:TextBox ID="txtTag10" runat="server" CssClass="wf_mas_field"></asp:TextBox>                            
                            </div>
                            </ContentTemplate>
                            </asp:UpdatePanel>
                            <div class="normal_common">
                                <span class="wf_det_add_holder">
                                    <asp:UpdatePanel runat="server" id="UpdatePanel3">
                                    <ContentTemplate>
                                    <asp:Button ID="cmdSearch" runat="server" CssClass="wf_mas_add" Text="Search" onclick="cmdSearch_Click" />
                                    </ContentTemplate>
                                    </asp:UpdatePanel>
                                </span>
                            </div>
                            <div class="normal_common">
                            
                                    <div style="border:1px solid #999; float:left; width:100%; -moz-border-radius:3px; -webkit-border-radius:3px; border-radius:3px;">
                                        <div style="border-bottom:1px solid #999; padding:0px;" class="normal_common">
                                            <div style="padding:5px; font-size:16px; color:#660066; font-weight:bold; background:#f0eded;">
                                                Searched List
                                            </div>
                                        </div>  
                                        <div class="normal_common">
                                            <%--Left GridView Holder Panel Start--%>
                                            <div style="padding:5px; overflow:auto; height:250px;">
                                                <asp:UpdatePanel ID="UpdatePanel11" runat="server" UpdateMode="Always">
                                                <ContentTemplate>                                                
                                                <asp:GridView ID="gvSearchedList" runat="server" style="border:1px solid #000;" Width="100%" BackColor="White" CellPadding="4" 
                                                ForeColor="Black" GridLines="Vertical" AutoGenerateColumns="False" CaptionAlign="Top" DataKeyNames=""
                                                EnableModelValidation="True" onselectedindexchanged="gvSearchedList_SelectedIndexChanged">
                                                <RowStyle BackColor="#F7F7DE" HorizontalAlign="Center" />
                                                <FooterStyle BackColor="#CCCC99" />
                                                <PagerStyle BackColor="#F7F7DE" ForeColor="Black" />
                                                <SelectedRowStyle BackColor="#dae2ea" Font-Bold="True" ForeColor="Blue" HorizontalAlign="Center" />
                                                <HeaderStyle BackColor="#cccccc" Font-Bold="True" HorizontalAlign="Center" />
                                                <AlternatingRowStyle BackColor="White" />
                                                <Columns>
                                                <asp:TemplateField Visible="False">
                                                <ItemTemplate >
                                                <asp:Label ID="lbDocID" runat="server" Text='<%#Eval("doc_id")%>' ></asp:Label>
                                                </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField Visible="False">
                                                <ItemTemplate >
                                                <asp:Label ID="lbDocName" runat="server" Text='<%#Eval("doc_name")%>' ></asp:Label>
                                                </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Document Name">  
                                                <ItemTemplate><%#Eval("doc_name")%></ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Left" CssClass="GVRec"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="Left" CssClass="GVRec"></ItemStyle>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Date">  
                                                <ItemTemplate><%#Eval("upld_dt")%></ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Left" CssClass="GVRec"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="Left" CssClass="GVRec"></ItemStyle>
                                                </asp:TemplateField>
                                                <asp:TemplateField Visible="False">
                                                <ItemTemplate >
                                                <asp:Label ID="lbDocTypeID" runat="server" Text='<%#Eval("doc_type_id")%>' ></asp:Label>
                                                </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Doc Type ID" Visible="false">  
                                                <ItemTemplate><%#Eval("doc_type_id")%></ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Left" CssClass="GVRec"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="Left" CssClass="GVRec"></ItemStyle>
                                                </asp:TemplateField>

                                                <asp:TemplateField Visible="false">
                                                <ItemTemplate >
                                                <asp:Label ID="lbcabName" runat="server" Text='<%#Eval("cab_name")%>' ></asp:Label>
                                                </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Cabinet">  
                                                <ItemTemplate><%#Eval("cab_name")%></ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Left" CssClass="GVRec"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="Left" CssClass="GVRec"></ItemStyle>
                                                </asp:TemplateField>
                                                <asp:TemplateField Visible="false">
                                                <ItemTemplate >
                                                <asp:Label ID="lbdrwName" runat="server" Text='<%#Eval("drw_name")%>' ></asp:Label>
                                                </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Drawer">  
                                                <ItemTemplate><%#Eval("drw_name")%></ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Left" CssClass="GVRec"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="Left" CssClass="GVRec"></ItemStyle>
                                                </asp:TemplateField>
                                                <asp:TemplateField Visible="False">
                                                <ItemTemplate >
                                                <asp:Label ID="lbfldName" runat="server" Text='<%#Eval("fld_name")%>' ></asp:Label>
                                                </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Folder">  
                                                <ItemTemplate><%#Eval("fld_name")%></ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Left" CssClass="GVRec"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="Left" CssClass="GVRec"></ItemStyle>
                                                </asp:TemplateField>
                                                <asp:TemplateField Visible="False">
                                                <ItemTemplate >
                                                <asp:Label ID="lbDownloadPath" runat="server" Text='<%#Eval("download_path")%>' ></asp:Label>
                                                </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Action" Visible="False" >
                                                <ItemTemplate>
                                                    <asp:HyperLink ID="cmdDownload" runat="server" CssClass="wf_mas_add" Text="View" NavigateUrl='<%#Eval("download_path")%>'></asp:HyperLink>
                                                </ItemTemplate>
                                                <HeaderStyle Width="70px" HorizontalAlign="Left" CssClass="GVRec"></HeaderStyle>
                                                <ItemStyle Width="70px" HorizontalAlign="Left" CssClass="GVRec"></ItemStyle>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="WFL Status" HeaderStyle-Width="80" ItemStyle-Width="80" >  
                                                    <ItemTemplate >
                                                    <asp:LinkButton ID="LinkButton1" runat="server" CommandName="Select" Text="View" OnClientClick="javascript: return showLoading();"  tooltip ='Show Comments'></asp:LinkButton>
                                                    </ItemTemplate> 
                                                    <HeaderStyle Width="80px" />
                                                    <ItemStyle Width="80px" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Document" HeaderStyle-Width="80" ItemStyle-Width="80" >  
                                                    <ItemTemplate >                             
                                                    <asp:LinkButton ID="LinkButton12" runat="server" CommandName="Select" Text="View" OnClientClick='<%#Eval("doc_id","javascript: OpenDoc({0});")%>' tooltip ='View the Doc'></asp:LinkButton>
                                                    </ItemTemplate> 
                                                    <HeaderStyle Width="80px" />
                                                    <ItemStyle Width="80px" />
                                                </asp:TemplateField>
                                                <asp:TemplateField Visible="False">
                                                <ItemTemplate >
                                                <asp:Label ID="lbDocTypeName" runat="server" Text='<%#Eval("doc_type_name")%>' ></asp:Label>
                                                </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Doc Type Name" >  
                                                <ItemTemplate><%#Eval("doc_type_name")%></ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Left" CssClass="GVRec"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="Left" CssClass="GVRec"></ItemStyle>
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
                        </div>

                    </div>
        </div>
    </div>
            </div>

                        

    <div class="footer_main">
  <div class="footer_text">Powered by ADA modeling technology</div>
</div>
<%--Comments for the selected document Start--%>
<div id="dvLoading1" ></div>
<div id="dvLoading2" >
<div class="lightbox_1">
                        
<div class="lightbox_2">
<asp:UpdatePanel ID="UpdatePanel22" runat="server" UpdateMode="Conditional">
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvSearchedList" EventName="selectedindexchanged"></asp:AsyncPostBackTrigger>
</Triggers>
<ContentTemplate>
<div class="lightbox_3">
<div style="float:left; font-weight:bold; border-bottom:1px solid #666; font-size:16px; width:100%;">
    Comments Details
    </div>
<div class="normal_feature">
<div style=" width:100%; float:left; padding-top:20px;">
<div style=" overflow:auto; height:200px;">
<asp:GridView style="border:1px solid #000;" Width="100%" BackColor="White" CellPadding="4" ForeColor="Black" ID="gvComment" runat="server">
<RowStyle BackColor="#F7F7DE" HorizontalAlign="Center" />
<FooterStyle BackColor="#CCCC99" />
<PagerStyle BackColor="#F7F7DE" ForeColor="Black"  />
<SelectedRowStyle BackColor="#CE5D5A" Font-Bold="True" ForeColor="White" HorizontalAlign="Center" />
<HeaderStyle BackColor="#cccccc" Font-Bold="True" HorizontalAlign="Center"  />
<AlternatingRowStyle BackColor="White"  />
                            
</asp:GridView>
    </div>
</div>
    <div class="normal_feature">
<div style="color:#FF0000; text-align:center; font-weight:bold; font-size:16px;  border-bottom:1px solid #666; padding:10px;">
    <asp:Label ID="MsgNodet" runat="server"></asp:Label> 
</div>
</div>
<div  class="normal_feature" style="text-align:right;">                         
    <input type="button" id="ImageButton1" class="padd_marg1" onclick="javascript: return hideLoading();" />
</div>
</ContentTemplate>
</asp:UpdatePanel> 
</div>
                       
</div>
</div>
<%--Comments for the selected document End--%>
    </form>
</body>
</html>
