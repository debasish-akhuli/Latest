<%@ Page Title="myDOCbase" Language="C#" MasterPageFile="adm_mast_entry.Master" AutoEventWireup="true" CodeBehind="BlankTempUpload.aspx.cs" Inherits="DMS.BlankTempUpload" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
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
        function SetDocName(DocName) {
            DocName = DocName.substring(DocName.lastIndexOf("\\") + 1, DocName.length);
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
           
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
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
                    New Template Upload
                </div>
            </div>
            <div class="normal_common">
                <div class="wf_mas_doc_holder" style="min-height:300px;">
                    <div class="wf_mas_doc_holder_padd">
                        <div style="float:left; width:50%;">
                            <div class="normal_common" id="divCompany" runat="server">
                                <span class="wf_mas_cap">Company</span>
                                <asp:DropDownList ID="ddCompany" CssClass="wf_det_field2_big" runat="server" AutoPostBack="true" onselectedindexchanged="ddCompany_SelectedIndexChanged"></asp:DropDownList>
                            </div>
                            <div class="normal_common">
                                <span class="wf_mas_cap">Document Type</span>
                                <asp:DropDownList ID="ddDocType" CssClass="wf_det_field2_big" runat="server"></asp:DropDownList>
                                <span style="padding-left:5px; float:left; font-weight:bold; font-size:24px; color:Red;">*</span>
                            </div>
                            <%--<div class="normal_common">
                                <span class="wf_mas_cap">Department</span>
                                <asp:DropDownList ID="ddDept" CssClass="wf_det_field2_big" runat="server"></asp:DropDownList>
                            </div>--%>
                            <div style="float:left; width:100%; display:block; margin-bottom:10px; margin-top:10px;">
                                <span class="wf_mas_cap">Cabinet</span>
                                <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Always">                                
                                <ContentTemplate>
                                    <asp:DropDownList ID="ddCabinet1" CssClass="wf_det_field2_big" runat="server" AutoPostBack="true" onselectedindexchanged="ddCabinet1_SelectedIndexChanged"></asp:DropDownList>
                                    <span style="padding-left:5px; float:left; font-weight:bold; font-size:24px; color:Red;">*</span>
                                </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                            <div style="float:left; width:100%; display:block; margin-bottom:10px;">
                                <span class="wf_mas_cap">Drawer</span>
                                <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Always">                                
                                <ContentTemplate>
                                    <asp:DropDownList ID="ddDrawer1" CssClass="wf_det_field2_big" runat="server" AutoPostBack="true" onselectedindexchanged="ddDrawer1_SelectedIndexChanged"></asp:DropDownList>
                                    <span style="padding-left:5px; float:left; font-weight:bold; font-size:24px; color:Red;">*</span>
                                </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                            <div style="float:left; width:100%; display:block; margin-bottom:10px;">
                                <span class="wf_mas_cap">Folder</span>
                                <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Always">                                
                                <ContentTemplate>
                                    <asp:DropDownList ID="ddFolder1" CssClass="wf_det_field2_big" runat="server"></asp:DropDownList>
                                    <span style="padding-left:5px; float:left; font-weight:bold; font-size:24px; color:Red;">*</span>
                                </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                            <div class="normal_common" id="divBrowse" runat="server">
                                <span class="wf_mas_cap">Select Document</span>
                                <asp:FileUpload ID="btnBrows" runat="server" onchange="javascript: SetDocName(this.value);" CssClass="fieldBrowse" />
                                <span style="padding-left:5px; float:left; font-weight:bold; font-size:24px; color:Red;">*</span>
                                <span style="float:left;">(.pdf only)</span>
                                <asp:HiddenField ID="hfSelActualDocName" runat="server" />
                                <asp:HiddenField ID="hfUUID" runat="server" />
                            </div>
                            <div class="normal_common">
                                <span class="wf_mas_cap">Document Name</span>
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
                                    <asp:ListItem Value="M" Text="Modify (View and Modify Options will be Allowed)"></asp:ListItem>
                                    <asp:ListItem Value="D" Text="Delete (View, Modify and Delete Options will be Allowed)"></asp:ListItem>
                                </asp:DropDownList>
                            </div>--%>
                            <div class="normal_common">
                                <asp:Button ID="cmdAddMaster" runat="server" CssClass="wf_mas_add" Text="Upload" onclick="cmdAddMaster_Click" />
                            </div>
                        </div>
                        <div style="float:right; width:50%; display:none;">
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
</asp:Content>