<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="doc_mast.aspx.cs" Inherits="DMS.doc_mast" %>

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
        function FormValidation() {
            if (document.getElementById("<%=txtDocName.ClientID %>").value.trim() == "") {
                alert("Please Enter Document Name");
                document.getElementById("<%=txtDocName.ClientID %>").focus();
                return false;
            }
            else {
                return true;
            }
        }
    </script>
    <script language="JavaScript" type="text/javascript">
        function SetDocName(DocName) {
            DocName = DocName.substring(DocName.lastIndexOf("\\")+1, DocName.length);
            document.getElementById('<%# txtDocName.ClientID %>').value = DocName;
            document.getElementById('<%# hfSelActualDocName.ClientID %>').value = document.getElementById('<%# btnBrows.ClientID %>').value;
        }
        $(document).ready(function () {
            $("a.close").hide();

            $("td.show_close").mouseover(function () {
                $(this).children('a.close').show();
            });

            $("td.show_close").mouseout(function () {
                $(this).children('a.close').hide();
            });
        });
        function showLoading() {
            document.getElementById("dvLoading1").style.display = 'block';
            document.getElementById("dvLoading2").style.display = 'block';
        }
        function hideLoading() {
            document.getElementById("dvLoading1").style.display = 'none';
            document.getElementById("dvLoading2").style.display = 'none';
        }
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
                    New Document Upload
                </div>
            </div>
            <div class="normal_common">
                <div class="wf_mas_doc_holder" style="min-height:300px;">
                    <div class="wf_mas_doc_holder_padd">
                        <div style="float:left; width:48%;">
                            <div class="normal_common" id="divDocType" runat="server">
                                <span class="wf_mas_cap">Document Type</span>
                                <asp:DropDownList ID="ddDocType" CssClass="wf_det_field2_big" runat="server" AutoPostBack="true" onselectedindexchanged="ddDocType_SelectedIndexChanged"></asp:DropDownList>
                                <span style="padding-left:5px; float:left; font-weight:bold; font-size:24px; color:Red;">*</span>
                            </div>
                            <div class="normal_common" id="divDept" runat="server">
                                <span class="wf_mas_cap">Department</span>
                                <asp:DropDownList ID="ddDept" CssClass="wf_det_field2_big" runat="server" AutoPostBack="true" onselectedindexchanged="ddDept_SelectedIndexChanged"></asp:DropDownList>
                                <span style="padding-left:5px; float:left; font-weight:bold; font-size:24px; color:Red;">*</span>
                            </div>
                            <div class="normal_common" id="divUpldLoc" runat="server">
                                <span class="wf_mas_cap">Uploaded Location</span>
                                <asp:Label ID="lblLocation" runat="server" Font-Bold="true"></asp:Label>
                            </div>
                            <div class="normal_common" id="divAssignedWFL" runat="server">
                                <span class="wf_mas_cap">Workflow</span>
                                <asp:HiddenField ID="hfWFLID" runat="server" />
                                <asp:Label ID="lblWFL" runat="server" Font-Bold="true"></asp:Label>
                                <span style="float:right; padding-right:50px; font-weight:bold;"><asp:Button ID="cmdViewWFL" Text="View" CssClass="wf_mas_add" ToolTip="Show Details" OnClick="cmdViewWFL_Click" OnClientClick="javascript: return showLoading();" runat="server" /></span>
                            </div>
                            <div class="normal_common" id="divUpldCabinet" runat="server">
                                <span class="wf_mas_cap">Cabinet</span>
                                <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Always">                                
                                <ContentTemplate>
                                    <asp:DropDownList ID="ddCabinet" CssClass="wf_det_field2_big" runat="server" AutoPostBack="true" onselectedindexchanged="ddCabinet_SelectedIndexChanged"></asp:DropDownList>
                                    <span style="padding-left:5px; float:left; font-weight:bold; font-size:24px; color:Red;">*</span>
                                </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                            <div class="normal_common" id="divUpldDrawer" runat="server">
                                <span class="wf_mas_cap">Drawer</span>
                                <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Always">                                
                                <ContentTemplate>
                                    <asp:DropDownList ID="ddDrawer" CssClass="wf_det_field2_big" runat="server" AutoPostBack="true" onselectedindexchanged="ddDrawer_SelectedIndexChanged"></asp:DropDownList>
                                    <span style="padding-left:5px; float:left; font-weight:bold; font-size:24px; color:Red;">*</span>
                                </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                            <div class="normal_common" id="divUpldFolder" runat="server">
                                <span class="wf_mas_cap">Folder</span>
                                <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Always">                                
                                <ContentTemplate>
                                    <asp:DropDownList ID="ddFolder" CssClass="wf_det_field2_big" runat="server"></asp:DropDownList>
                                    <span style="padding-left:5px; float:left; font-weight:bold; font-size:24px; color:Red;">*</span>
                                </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                            <div class="normal_common" id="divBrowse" runat="server">
                                <span class="wf_mas_cap">Select Document</span>
                                <asp:FileUpload ID="btnBrows" runat="server" onchange="javascript: SetDocName(this.value);" CssClass="fieldBrowse" />
                                <span style="padding-left:5px; float:left; font-weight:bold; font-size:24px; color:Red;">*</span>
                                <asp:HiddenField ID="hfSelActualDocName" runat="server" />
                                <asp:HiddenField ID="hfUUID" runat="server" />
                            </div>
                            <div class="normal_common">
                                <span class="wf_mas_cap">Document Name</span>
                                <asp:Label ID="lblDocName" runat="server" Text="" Font-Bold="true"></asp:Label>
                                <asp:TextBox ID="txtDocName" runat="server" CssClass="wf_mas_field"></asp:TextBox>
                                <span style="padding-left:5px; float:left; font-weight:bold; font-size:24px; color:Red;">*</span>
                            </div>
                            <div class="normal_common">
                                <span class="wf_mas_cap">Document Description</span>
                                <asp:TextBox ID="txtDocDesc" runat="server" CssClass="wf_mas_field_m" TextMode="MultiLine"></asp:TextBox>
                            </div>
                            <%--<div class="normal_common">
                                <span class="wf_mas_cap">Permission for Others</span>
                                <asp:DropDownList ID="ddPermission" CssClass="wf_det_field2_big" runat="server">
                                    <asp:ListItem Value="X" Text="No Permission"></asp:ListItem>
                                    <asp:ListItem Value="V" Text="View (Only View Option will be Allowed)"></asp:ListItem>
                                    <asp:ListItem Value="M" Text="Modify (View and Modify Options will be Allowed)" Selected="True"></asp:ListItem>
                                    <asp:ListItem Value="D" Text="Delete (View, Modify and Delete Options will be Allowed)"></asp:ListItem>
                                </asp:DropDownList>
                            </div>--%>
                            <div class="normal_common">
                                <asp:Button ID="cmdAddMaster" runat="server" CssClass="wf_mas_add" Text="Upload" onclick="cmdAddMaster_Click" />
                            </div>
                            <div class="normal_common" style="display:none;">
                                <asp:Button ID="cmdFillMetaData" runat="server" CssClass="wf_mas_add" Text="Fill Metadata" onclick="cmdFillMetaData_Click" />
                            </div>
                        </div>
                        <div style="float:right; width:48%;">
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

    <%--Workflow Details View Start--%>
    <div id="dvLoading1" ></div>
    <div style="top:-100px;" id="dvLoading2" >
    <div class="lightbox_1">
    
    <div class="lightbox_2">
    <asp:UpdatePanel ID="UpdatePanel22" runat="server" UpdateMode="Conditional">
    <Triggers>
    <asp:AsyncPostBackTrigger ControlID="cmdViewWFL" EventName="click"></asp:AsyncPostBackTrigger>
    </Triggers>
    <ContentTemplate>
    <div class="lightbox_3">
    <div style="float:left; font-weight:bold; border-bottom:1px solid #666; font-size:16px; width:100%;">
        Workflow Details
        </div>
    <div class="normal_feature">
    <asp:Label CssClass="lightbox_label" ID="Lblwf" runat="server" Text=""></asp:Label><asp:Label CssClass="lightbox_label2" ID="lblWfname" runat="server" Text=""></asp:Label></div>
    <div class="normal_feature"><asp:Label  CssClass="lightbox_label" ID="Lbldept" runat="server"  Text=""></asp:Label><asp:Label CssClass="lightbox_label2" ID="lblWfdept" runat="server" Text=""></asp:Label></div>
    <div class="normal_feature"><asp:Label  CssClass="lightbox_label" ID="Lbldoc" runat="server" Text=""></asp:Label><asp:Label CssClass="lightbox_label2" ID="lblWfDoctype" runat="server" Text=""></asp:Label></div>
    <div class="normal_feature"><asp:Label  CssClass="lightbox_label" ID="LblFld" runat="server" Text=""></asp:Label><asp:Label CssClass="lightbox_label2" ID="lblWfFld" runat="server" Text=""></asp:Label></div>
                        
    <div style="width:100%; float:left; padding-top:20px;">
    <div style="overflow:auto; height:290px;">
        <asp:GridView style="border:1px solid #000;" Width="100%" BackColor="White" CellPadding="4" ForeColor="Black" ID="gv" runat="server">
        <RowStyle BackColor="#F7F7DE" HorizontalAlign="Center" />
        <FooterStyle BackColor="#CCCC99" />
        <PagerStyle BackColor="#F7F7DE" ForeColor="Black" />
        <SelectedRowStyle BackColor="#CE5D5A" Font-Bold="True" ForeColor="White" HorizontalAlign="Center" />
        <HeaderStyle BackColor="#cccccc" Font-Bold="True" HorizontalAlign="Center" />
        <AlternatingRowStyle BackColor="White" />
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
    <%--Workflow Details View End--%>
    </form>
</body>
</html>
