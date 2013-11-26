<%@ Page Title="myDOCbase" Language="C#" MasterPageFile="adm_mast_entry.Master" AutoEventWireup="true" CodeBehind="user_mast.aspx.cs" Inherits="DMS.user_mast" %>
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
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
        function EndRequestHandler(sender, args) {
            if (document.getElementById("<%= hfMsg.ClientID %>").value != undefined && document.getElementById("<%= hfMsg.ClientID %>").value != "") {
                alert(document.getElementById('<%= hfMsg.ClientID %>').value);
                document.getElementById('<%= hfMsg.ClientID %>').value = "";
            }
        }

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
            <div style="padding-bottom:0px;" class="normal_common">
                <div class="wf_mas_Heading2">
                    User Master
                </div>
            </div>
            <div class="normal_common">
                <div class="wf_mas_doc_holder" style="min-height:300px;">
                    <div class="wf_mas_doc_holder_padd">
                        <div class="normal_common" id="divCompany" runat="server">
                            <span class="wf_mas_cap">Company</span>
                            <asp:DropDownList ID="ddCompany" CssClass="wf_det_field2_big" AutoPostBack="true" runat="server" onselectedindexchanged="ddCompany_SelectedIndexChanged"></asp:DropDownList>
                        </div>
                        <div class="normal_common" id="divUserType" runat="server">
                            <span class="wf_mas_cap">User Type</span>
                            <asp:DropDownList ID="ddUserType" CssClass="wf_det_field2" runat="server">
                                <asp:ListItem Value="A" Text="Admin"></asp:ListItem>
                                <asp:ListItem Value="N" Text="Normal"></asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div class="normal_common" style="display:none;">
                            <span class="wf_mas_cap">User ID</span>
                            <asp:TextBox ID="txtUserID" MaxLength="20" runat="server" CssClass="wf_mas_field"></asp:TextBox>
                            <span style="padding-left:5px; float:left; font-weight:bold; font-size:24px; color:Red;">*</span>
                        </div>
                        <div class="normal_common">
                            <span class="wf_mas_cap">First Name</span>
                            <asp:TextBox ID="txtFName" runat="server" CssClass="wf_mas_field"></asp:TextBox>
                            <span style="padding-left:5px; float:left; font-weight:bold; font-size:24px; color:Red;">*</span>
                        </div>
                        <div class="normal_common">
                            <span class="wf_mas_cap">Last Name</span>
                            <asp:TextBox ID="txtLName" runat="server" CssClass="wf_mas_field"></asp:TextBox>
                            <span style="padding-left:5px; float:left; font-weight:bold; font-size:24px; color:Red;">*</span>
                        </div>
                        <div class="normal_common">
                            <span class="wf_mas_cap">Email ID</span>
                            <asp:TextBox ID="txtMail" runat="server" CssClass="wf_mas_field"></asp:TextBox>
                            <span style="padding-left:5px; float:left; font-weight:bold; font-size:24px; color:Red;">*</span>
                        </div>
                        <div class="normal_common">
                            <span class="wf_mas_cap">Password</span>
                            <asp:TextBox ID="txtPwd" runat="server" TextMode="Password" CssClass="wf_mas_field"></asp:TextBox>
                            <span style="padding-left:5px; float:left; font-weight:bold; font-size:24px; color:Red;">*</span>
                        </div>
                        <div class="normal_common">
                            <span class="wf_mas_cap">Title</span>
                            <asp:TextBox ID="txtTitle" runat="server" CssClass="wf_mas_field"></asp:TextBox>
                            <span style="padding-left:5px; float:left; font-weight:bold; font-size:24px; color:Red;">*</span>
                        </div>
                        <div class="normal_common">
                            <span class="wf_mas_cap">Department</span>
                            <asp:DropDownList ID="ddDept" CssClass="wf_det_field2" runat="server"></asp:DropDownList>
                            <span style="padding-left:5px; float:left; font-weight:bold; font-size:24px; color:Red;">*</span>
                        </div>
                        <div class="normal_common">
                            <span class="wf_mas_cap">Status</span>
                            <asp:DropDownList ID="ddStat" CssClass="wf_det_field2" runat="server">
                                <asp:ListItem Value="A" Text="Active"></asp:ListItem>
                                <asp:ListItem Value="I" Text="Inactive"></asp:ListItem>
                            </asp:DropDownList>
                            <span style="padding-left:5px; float:left; font-weight:bold; font-size:24px; color:Red;">*</span>
                        </div>
                        <div class="normal_common">
                            <span class="wf_mas_cap">Can Change Password</span>
                            <asp:DropDownList ID="ddCanChangePwd" CssClass="wf_det_field2" runat="server">
                                <asp:ListItem Value="Y" Text="Yes" Selected="True"></asp:ListItem>
                                <asp:ListItem Value="N" Text="No"></asp:ListItem>
                            </asp:DropDownList>
                            <span style="padding-left:5px; float:left; font-weight:bold; font-size:24px; color:Red;">*</span>
                        </div>
                        <div class="normal_common">
                            <span class="wf_mas_cap">Default Permission (for e-Filing System)</span>
                            <asp:DropDownList ID="ddDefaultPermission" CssClass="wf_det_field2" runat="server">
                                <asp:ListItem Value="Y" Text="Yes" Selected="True"></asp:ListItem>
                                <asp:ListItem Value="N" Text="No"></asp:ListItem>
                            </asp:DropDownList>
                            <span style="padding-left:5px; float:left; font-weight:bold; font-size:24px; color:Red;">*</span>
                            <span class="wf_det_add_holder">
                                <asp:Button ID="cmdAddMaster" runat="server" CssClass="wf_mas_add" Text="Add" onclick="cmdAddMaster_Click" />
                            </span>
                        </div>
                        <div style="margin-top:20px;" class="normal_common">
                            <div style="overflow:auto; height:300px;">
                                <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Always">                                
                                <ContentTemplate>
                                    <asp:HiddenField ID="hfMsg" runat="server" />
                                    <asp:GridView ID="gvDispRec" runat="server" style="border:1px solid #000;" 
                                        Width="100%" BackColor="White" CellPadding="4" 
                                    ForeColor="Black" GridLines="Vertical" AutoGenerateColumns="False" OnPageIndexChanging="gvDispRec_PageIndexChanging"
                                    onrowediting="gvDispRec_RowEditing" OnRowDataBound="gvDispRec_RowDataBound" onrowcancelingedit="gvDispRec_RowCancelingEdit" 
                                    onrowupdating="gvDispRec_RowUpdating" 
                                    onrowdeleting="gvDispRec_RowDeleting" CaptionAlign="Top" 
                                    EnableModelValidation="True" AllowPaging="True">
                                    <RowStyle BackColor="#F7F7DE" HorizontalAlign="Center" />
                                    <FooterStyle BackColor="#CCCC99" />
                                        <PagerSettings Position="Top" />
                                    <PagerStyle BackColor="#F7F7DE" ForeColor="Black"  />
                                    <SelectedRowStyle BackColor="#CE5D5A" Font-Bold="True" ForeColor="White" HorizontalAlign="Center" />
                                    <HeaderStyle BackColor="#cccccc" Font-Bold="True" HorizontalAlign="Center"  />
                                    <AlternatingRowStyle BackColor="White"  />
                                    <Columns>
                                        <asp:TemplateField Visible="False">
                                            <ItemTemplate >
                                            <asp:Label ID="lbAutoID" runat="server" Text='<%#Eval("user_id")%>' ></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="User ID" HeaderStyle-Width="200" ItemStyle-Width="200">  
                                            <ItemTemplate ><%#Eval("user_id")%></ItemTemplate>
                                            <HeaderStyle Width="200px" HorizontalAlign="Left" CssClass="GVRec"></HeaderStyle>
                                            <ItemStyle Width="200px" HorizontalAlign="Left" CssClass="GVRec"></ItemStyle>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="First Name" HeaderStyle-Width="200" ItemStyle-Width="200">  
                                            <ItemTemplate ><%#Eval("f_name")%></ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox ID="txtEditFName" runat="server" Text='<%#Eval("f_name")%>' Width="100%"></asp:TextBox>
                                            </EditItemTemplate>
                                            <HeaderStyle Width="200px" HorizontalAlign="Left" CssClass="GVRec"></HeaderStyle>
                                            <ItemStyle Width="200px" HorizontalAlign="Left" CssClass="GVRec"></ItemStyle>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Last Name" HeaderStyle-Width="200" ItemStyle-Width="200">  
                                            <ItemTemplate ><%#Eval("l_name")%></ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox ID="txtEditLName" runat="server" Text='<%#Eval("l_name")%>' Width="100%"></asp:TextBox>
                                            </EditItemTemplate>
                                            <HeaderStyle Width="200px" HorizontalAlign="Left" CssClass="GVRec"></HeaderStyle>
                                            <ItemStyle Width="200px" HorizontalAlign="Left" CssClass="GVRec"></ItemStyle>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Email ID" HeaderStyle-Width="200" ItemStyle-Width="200">  
                                            <ItemTemplate ><%#Eval("email")%></ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox ID="txtEditEmailID" runat="server" Text='<%#Eval("email")%>' Width="100%"></asp:TextBox>
                                            </EditItemTemplate>
                                            <HeaderStyle Width="200px" HorizontalAlign="Left" CssClass="GVRec"></HeaderStyle>
                                            <ItemStyle Width="200px" HorizontalAlign="Left" CssClass="GVRec"></ItemStyle>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Title" HeaderStyle-Width="200" ItemStyle-Width="200">  
                                            <ItemTemplate ><%#Eval("user_title")%></ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox ID="txtEditUserTitle" runat="server" Text='<%#Eval("user_title")%>' Width="100%"></asp:TextBox>
                                            </EditItemTemplate>
                                            <HeaderStyle Width="200px" HorizontalAlign="Left" CssClass="GVRec"></HeaderStyle>
                                            <ItemStyle Width="200px" HorizontalAlign="Left" CssClass="GVRec"></ItemStyle>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Department" HeaderStyle-Width="200" ItemStyle-Width="200">  
                                            <ItemTemplate ><%#Eval("dept_name")%></ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:DropDownList ID="ddlEditDept" CssClass="wf_det_field2" runat="server" Width="100%"></asp:DropDownList>
                                                <asp:HiddenField ID="hdDept" runat="server" Value='<%#Eval("dept_name")%>' />
                                            </EditItemTemplate>
                                            <HeaderStyle Width="200px" HorizontalAlign="Left" CssClass="GVRec"></HeaderStyle>
                                            <ItemStyle Width="200px" HorizontalAlign="Left" CssClass="GVRec"></ItemStyle>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Status" HeaderStyle-Width="200" ItemStyle-Width="200">  
                                            <ItemTemplate ><%#Eval("user_stat").ToString()=="A"?"Active":"Inactive"%></ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:DropDownList ID="ddlEditStat" CssClass="wf_det_field2" runat="server">
                                                    <asp:ListItem Value="A" Text="Active"></asp:ListItem>
                                                    <asp:ListItem Value="I" Text="Inactive"></asp:ListItem>
                                                </asp:DropDownList>
                                                <asp:HiddenField ID="hdStat" runat="server" Value='<%#Eval("user_stat")%>' />
                                            </EditItemTemplate>
                                            <HeaderStyle Width="200px" HorizontalAlign="Left" CssClass="GVRec"></HeaderStyle>
                                            <ItemStyle Width="200px" HorizontalAlign="Left" CssClass="GVRec"></ItemStyle>
                                        </asp:TemplateField>
                                        <asp:CommandField HeaderText="Action" ShowEditButton="True" />
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
