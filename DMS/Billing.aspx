<%@ Page Title="myDOCbase" Language="C#" MasterPageFile="adm_mast_entry.Master" AutoEventWireup="true" CodeBehind="Billing.aspx.cs" Inherits="DMS.Billing" %>
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
                    Statistics
                </div>
            </div>
            <div class="normal_common">
                <div class="wf_mas_doc_holder" style="min-height:300px;">
                    <div class="wf_mas_doc_holder_padd">
                        <%--<div class="normal_common">
                            <span class="wf_mas_cap">Month & Year</span>
                            <asp:DropDownList ID="ddMonth" CssClass="wf_det_field2" runat="server" AutoPostBack="true" onselectedindexchanged="ddMonth_SelectedIndexChanged">
                                <asp:ListItem Value="01" Text="January"></asp:ListItem>
                                <asp:ListItem Value="02" Text="February"></asp:ListItem>
                                <asp:ListItem Value="03" Text="March"></asp:ListItem>
                                <asp:ListItem Value="04" Text="April"></asp:ListItem>
                                <asp:ListItem Value="05" Text="May"></asp:ListItem>
                                <asp:ListItem Value="06" Text="June"></asp:ListItem>
                                <asp:ListItem Value="07" Text="July"></asp:ListItem>
                                <asp:ListItem Value="08" Text="August"></asp:ListItem>
                                <asp:ListItem Value="09" Text="September"></asp:ListItem>
                                <asp:ListItem Value="10" Text="October"></asp:ListItem>
                                <asp:ListItem Value="11" Text="November"></asp:ListItem>
                                <asp:ListItem Value="12" Text="December"></asp:ListItem>
                            </asp:DropDownList>
                            <asp:DropDownList ID="ddYear" CssClass="wf_det_field2" runat="server" AutoPostBack="true" onselectedindexchanged="ddYear_SelectedIndexChanged">
                                <asp:ListItem Value="2012" Text="2012"></asp:ListItem>
                                <asp:ListItem Value="2013" Text="2013"></asp:ListItem>
                                <asp:ListItem Value="2014" Text="2014"></asp:ListItem>
                                <asp:ListItem Value="2015" Text="2015"></asp:ListItem>
                                <asp:ListItem Value="2016" Text="2016"></asp:ListItem>
                                <asp:ListItem Value="2017" Text="2017"></asp:ListItem>
                                <asp:ListItem Value="2018" Text="2018"></asp:ListItem>
                                <asp:ListItem Value="2019" Text="2019"></asp:ListItem>
                                <asp:ListItem Value="2020" Text="2020"></asp:ListItem>
                            </asp:DropDownList>
                        </div>--%>
                        <div class="normal_common">
                            <table border="1" cellpadding="0" cellspacing="0" width="50%" style="margin:0 auto;">
                                <tr>
                                    <td style="padding:2px 5px 2px 5px; font-weight:bold;">
                                        Total Space
                                    </td>
                                    <td style="padding:2px 5px 2px 5px; font-weight:bold;">
                                        <asp:Label ID="lblTotalSpace" runat="server" Text=""></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="padding:2px 5px 2px 5px; font-weight:bold;">
                                        Used Space
                                    </td>
                                    <td style="padding:2px 5px 2px 5px; font-weight:bold;">
                                        <asp:Label ID="lblUsedSpace" runat="server" Text=""></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="padding:2px 5px 2px 5px; font-weight:bold;">
                                        Available Space
                                    </td>
                                    <td style="padding:2px 5px 2px 5px; font-weight:bold;">
                                        <asp:Label ID="lblAvailableSpace" runat="server" Text=""></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="padding:2px 5px 2px 5px; font-weight:bold;">
                                        Max # of Users
                                    </td>
                                    <td style="padding:2px 5px 2px 5px; font-weight:bold;">
                                        <asp:Label ID="lblMaxUsers" runat="server" Text=""></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="padding:2px 5px 2px 5px; font-weight:bold;">
                                        Existing Users
                                    </td>
                                    <td style="padding:2px 5px 2px 5px; font-weight:bold;">
                                        <asp:Label ID="lblExistingUsers" runat="server" Text=""></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="padding:2px 5px 2px 5px; font-weight:bold;">
                                        Space Rate
                                    </td>
                                    <td style="padding:2px 5px 2px 5px; font-weight:bold;">
                                        <asp:Label ID="lblSpaceRate" runat="server" Text=""></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="padding:2px 5px 2px 5px; font-weight:bold;">
                                        User Rate
                                    </td>
                                    <td style="padding:2px 5px 2px 5px; font-weight:bold;">
                                        <asp:Label ID="lblUserRate" runat="server" Text=""></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="padding:2px 5px 2px 5px; font-weight:bold;">
                                        Total Rate
                                    </td>
                                    <td style="padding:2px 5px 2px 5px; font-weight:bold;">
                                        <asp:Label ID="lblTotalRate" runat="server" Text=""></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </div>

                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
