<%@ Page Title="myDOCbase" Language="C#" MasterPageFile="adm_mast_entry.Master" AutoEventWireup="true" CodeBehind="CompWiseStatistics.aspx.cs" Inherits="DMS.CompWiseStatistics" %>
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
                    Company-wise Statistics
                </div>
            </div>
            <div class="normal_common">
                <div class="wf_mas_doc_holder" style="min-height:300px;">
                    <div class="wf_mas_doc_holder_padd">
                        <div style="margin-top:20px;" class="normal_common">
                            <div style="overflow:auto; height:250px;">
                                <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Always">                                
                                <ContentTemplate>
                                    <asp:HiddenField ID="HiddenField1" runat="server" />
                                    <asp:GridView ID="gvDispRecTot" runat="server" style="border:1px solid #000;" 
                                        Width="100%" BackColor="White" CellPadding="4" 
                                    ForeColor="Black" GridLines="Vertical" AutoGenerateColumns="False" OnPageIndexChanging="gvDispRecTot_PageIndexChanging"
                                     onselectedindexchanged="gvDispRecTot_SelectedIndexChanged" DataKeyNames="CompCode" CaptionAlign="Top" AllowPaging="True" PageSize="10">
                                    <RowStyle BackColor="#F7F7DE" HorizontalAlign="Center" />
                                    <FooterStyle BackColor="#CCCC99" />
                                    <PagerSettings Position="Top" />
                                    <PagerStyle BackColor="#F7F7DE" ForeColor="Black" />
                                    <SelectedRowStyle BackColor="#CE5D5A" Font-Bold="True" ForeColor="White" HorizontalAlign="Center" />
                                    <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                    <HeaderStyle BackColor="#cccccc" Font-Bold="True" HorizontalAlign="Center" />
                                    <AlternatingRowStyle BackColor="White" />
                                    <Columns>
                                    <asp:TemplateField Visible="False">
                                        <ItemTemplate >
                                        <asp:Label ID="lbAutoID" runat="server" Text='<%#Eval("CompCode")%>' ></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Company" HeaderStyle-Width="200" ItemStyle-Width="200">  
                                        <ItemTemplate><%#Eval("CompName")%></ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Left" CssClass="GVRec"></HeaderStyle>
                                        <ItemStyle HorizontalAlign="Left" CssClass="GVRec"></ItemStyle>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Total Space (KB)">  
                                        <ItemTemplate><%#Eval("TotalSpace")%></ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Right" CssClass="GVRec"></HeaderStyle>
                                        <ItemStyle HorizontalAlign="Right" CssClass="GVRec"></ItemStyle>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Used Space (KB)">  
                                        <ItemTemplate><%#Eval("UsedSpace")%></ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Right" CssClass="GVRec"></HeaderStyle>
                                        <ItemStyle HorizontalAlign="Right" CssClass="GVRec"></ItemStyle>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Available Space (KB)">  
                                        <ItemTemplate><%#Eval("AvailableSpace")%></ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Right" CssClass="GVRec"></HeaderStyle>
                                        <ItemStyle HorizontalAlign="Right" CssClass="GVRec"></ItemStyle>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Max # of Users">  
                                        <ItemTemplate><%#Eval("MaxNoOfUsers")%></ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Right" CssClass="GVRec"></HeaderStyle>
                                        <ItemStyle HorizontalAlign="Right" CssClass="GVRec"></ItemStyle>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Existing Users">  
                                        <ItemTemplate><%#Eval("ExistingUsers")%></ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Right" CssClass="GVRec"></HeaderStyle>
                                        <ItemStyle HorizontalAlign="Right" CssClass="GVRec"></ItemStyle>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Space Rate ($/Month)">  
                                        <ItemTemplate><%#Eval("SpaceRate")%></ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Right" CssClass="GVRec"></HeaderStyle>
                                        <ItemStyle HorizontalAlign="Right" CssClass="GVRec"></ItemStyle>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="User Rate ($/Month)">  
                                        <ItemTemplate><%#Eval("UserRate")%></ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Right" CssClass="GVRec"></HeaderStyle>
                                        <ItemStyle HorizontalAlign="Right" CssClass="GVRec"></ItemStyle>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Total Rate ($/Month)">  
                                        <ItemTemplate><%#Eval("TotalRate")%></ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Right" CssClass="GVRec"></HeaderStyle>
                                        <ItemStyle HorizontalAlign="Right" CssClass="GVRec"></ItemStyle>
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
        </div>
    </div>
</asp:Content>