<%@ Page Title="myDOCbase" Language="C#" MasterPageFile="adm_mast_entry.Master" AutoEventWireup="true"
    CodeBehind="user_rights.aspx.cs" Inherits="DMS.user_rights" %>

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
                <div class="wf_mas_Heading">
                    <span>Welcome, </span>
                    <asp:Label ID="lblUser" runat="server" Text="User Name"></asp:Label>
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
            <div style="padding-bottom: 0px;" class="normal_common">
                <div class="wf_mas_Heading2">
                    User Rights Module
                </div>
            </div>
            <div class="normal_common">
                <div class="wf_mas_doc_holder" style="min-height: 300px;">
                    <div class="wf_mas_doc_holder_padd">
                        <div class="normal_common" id="divCompany" runat="server">
                            <span class="wf_mas_cap">Company</span>
                            <asp:DropDownList ID="ddCompany" CssClass="wf_det_field2_big" runat="server" AutoPostBack="true" onselectedindexchanged="ddCompany_SelectedIndexChanged"></asp:DropDownList>
                        </div>
                        <div class="normal_common">
                            <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Always">
                                <ContentTemplate>
                                    <%--<span class="wf_mas_cap">Role</span>--%>
                                   
                                    <%--Role listing start--%>
                                    <div style="float: left; width: 100%; height: 300px; overflow: auto; background-color: Gray;
                                        padding-left: 3px; padding-right: 3px; font-size: 12px; font-weight: bold; margin-right: 10px;">
                                        <div style="padding-left:2px; padding-right:2px">
                                        <div style="background-color: Black; margin-top: 5px; height: 20px;
                                            line-height: 20px; margin-bottom: 5px; padding:3px; color: White">
                                            Role</div>
                                            </div>
                                        <div style=" background-color: Black; width: 100%; margin-bottom: 3px;">
                                            <asp:GridView ID="gvRole" runat="server" Style="border: 1px solid #000;" Width="100%"
                                                BackColor="White" CellPadding="4" ForeColor="Black" GridLines="Vertical" AutoGenerateColumns="False"
                                                EnableModelValidation="True" DataKeyNames="role_id">
                                                <RowStyle BackColor="#F7F7DE" HorizontalAlign="Center" />
                                                <FooterStyle BackColor="#CCCC99" />
                                                <SelectedRowStyle BackColor="#CE5D5A" Font-Bold="True" ForeColor="White" HorizontalAlign="Center" />
                                                <HeaderStyle BackColor="#cccccc" Font-Bold="True" HorizontalAlign="Center" />
                                                <AlternatingRowStyle BackColor="White" />
                                                <Columns>
                                                    <asp:TemplateField HeaderStyle-Width="10%" ItemStyle-Width="10%">
                                                        <HeaderTemplate>
                                                            <asp:CheckBox ID="CheckBox1" runat="server" Text="Select All" AutoPostBack="true" OnCheckedChanged="ChkAll_Role" />
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="CheckBox2" runat="server" AutoPostBack="true" OnCheckedChanged="chkRole"  />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField Visible="False">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbAutoIDRole" runat="server" Text='<%#Eval("role_id")%>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Role" HeaderStyle-Width="100%" ItemStyle-Width="100%">
                                                        <ItemTemplate>
                                                            <%#Eval("role_name")%></ItemTemplate>
                                                        <HeaderStyle Width="100%" HorizontalAlign="Left" CssClass="GVRec"></HeaderStyle>
                                                        <ItemStyle Width="100%" HorizontalAlign="Left" CssClass="GVRec"></ItemStyle>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                    </div>
                                    <%--<asp:CheckBox ID="chkAllRole" runat="server" Text=" Select All" />--%>
                                    <%--Role listing end--%>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>












                          <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Always">
                                <ContentTemplate>
                     
                                <div class="normal_common">
                                    <div style="float: left; width: 220px; height: 300px; overflow: auto; background-color: Gray;
                                        padding-left: 3px; padding-right: 3px; font-size: 12px; font-weight: bold; margin-right: 10px;">
                                        <div style="float: left; background-color: Black; width: 97%; margin-top: 5px; height: 20px;
                                            padding-left: 5px; line-height: 20px; margin-bottom: 5px; color: White">
                                            Cabinet</div>
                                        <div style="float: left; background-color: Black; width: 100%; margin-bottom: 3px;">
                                      
                                            <asp:GridView ID="gvCab" runat="server" Style="border: 1px solid #000;" Width="100%"
                                                BackColor="White" CellPadding="4" ForeColor="Black" GridLines="Vertical" AutoGenerateColumns="False"
                                                EnableModelValidation="True" DataKeyNames="cab_uuid">
                                                <RowStyle BackColor="#F7F7DE" HorizontalAlign="Center" />
                                                <FooterStyle BackColor="#CCCC99" />
                                                <SelectedRowStyle BackColor="#CE5D5A" Font-Bold="True" ForeColor="White" HorizontalAlign="Center" />
                                                <HeaderStyle BackColor="#cccccc" Font-Bold="True" HorizontalAlign="Center" />
                                                <AlternatingRowStyle BackColor="White" />
                                                <Columns>
                                                    <asp:TemplateField HeaderStyle-Width="10%" ItemStyle-Width="10%">
                                                        <HeaderTemplate>
                                                           <asp:CheckBox ID="chkSelectAll" runat="server" Text="SelectAll" AutoPostBack="true" OnCheckedChanged="chkSelectAllCab_CheckedChanged" />

                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="cabRows" runat="server" CssClass="chkItem1" AutoPostBack="True"
                                                                OnCheckedChanged="Check_Clicked_Cab" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField Visible="False">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbAutoIDcab" runat="server" Text='<%#Eval("cab_uuid")%>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Cabinet" HeaderStyle-Width="100%" ItemStyle-Width="100%">
                                                        <ItemTemplate>
                                                            <%#Eval("cab_name")%></ItemTemplate>
                                                        <HeaderStyle Width="100%" HorizontalAlign="Left" CssClass="GVRec"></HeaderStyle>
                                                        <ItemStyle Width="100%" HorizontalAlign="Left" CssClass="GVRec"></ItemStyle>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                            
                                        </div>
                                    </div>
                                    <div style="float: left; width: 220px; height: 300px; overflow: auto; background-color: Gray;
                                        padding-left: 3px; padding-right: 3px; font-size: 12px; font-weight: bold; margin-right: 10px;">
                                        <div style="float: left; background-color: Black; width: 97%; margin-top: 5px; height: 20px;
                                            padding-left: 5px; line-height: 20px; margin-bottom: 5px; color: White">
                                            Drawer</div>
                                        <div style="float: left; background-color: Black; width: 100%; margin-bottom: 3px;">
                                        
                                            <asp:GridView ID="gvDrawer" runat="server" Style="border: 1px solid #000;" Width="100%"
                                                BackColor="White" CellPadding="4" ForeColor="Black" GridLines="Vertical" AutoGenerateColumns="False"
                                                EnableModelValidation="True" DataKeyNames="drw_uuid">
                                                <RowStyle BackColor="#F7F7DE" HorizontalAlign="Center" />
                                                <FooterStyle BackColor="#CCCC99" />
                                                <SelectedRowStyle BackColor="#CE5D5A" Font-Bold="True" ForeColor="White" HorizontalAlign="Center" />
                                                <HeaderStyle BackColor="#cccccc" Font-Bold="True" HorizontalAlign="Center" />
                                                <AlternatingRowStyle BackColor="White" />
                                                <Columns>
                                                    <asp:TemplateField HeaderStyle-Width="10%" ItemStyle-Width="10%">
                                                        <HeaderTemplate>
                                                           <asp:CheckBox ID="chkSelectAll1" runat="server" Text="SelectAll" AutoPostBack="true" OnCheckedChanged="chkSelectAllDrw_CheckedChanged" />
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="drwRows" runat="server" CssClass="chkItem2" AutoPostBack="True"
                                                                OnCheckedChanged="Check_Clicked_Drw" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField Visible="False">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbAutoIDdrw" runat="server" Text='<%#Eval("drw_uuid")%>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Drawer" HeaderStyle-Width="100%" ItemStyle-Width="100%">
                                                        <ItemTemplate>
                                                            <%#Eval("drw_name")%></ItemTemplate>
                                                        <HeaderStyle Width="100%" HorizontalAlign="Left" CssClass="GVRec"></HeaderStyle>
                                                        <ItemStyle Width="100%" HorizontalAlign="Left" CssClass="GVRec"></ItemStyle>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                            
                                        </div>
                                    </div>
                                    <div style="float: left; width: 220px; height: 300px; overflow: auto; background-color: Gray;
                                        padding-left: 3px; padding-right: 3px; font-size: 12px; font-weight: bold; margin-right: 10px;">
                                        <div style="float: left; background-color: Black; width: 97%; margin-top: 5px; height: 20px;
                                            padding-left: 5px; line-height: 20px; margin-bottom: 5px; color: White">
                                            Folder</div>
                                        <div style="float: left; background-color: Black; width: 100%; margin-bottom: 3px;">
                                            <asp:GridView ID="gvFolder" runat="server" Style="border: 1px solid #000;" Width="100%"
                                                BackColor="White" CellPadding="4" ForeColor="Black" GridLines="Vertical" AutoGenerateColumns="False"
                                                EnableModelValidation="True" DataKeyNames="fld_uuid">
                                                <RowStyle BackColor="#F7F7DE" HorizontalAlign="Center" />
                                                <FooterStyle BackColor="#CCCC99" />
                                                <SelectedRowStyle BackColor="#CE5D5A" Font-Bold="True" ForeColor="White" HorizontalAlign="Center" />
                                                <HeaderStyle BackColor="#cccccc" Font-Bold="True" HorizontalAlign="Center" />
                                                <AlternatingRowStyle BackColor="White" />
                                                <Columns>
                                                    <asp:TemplateField HeaderStyle-Width="10%" ItemStyle-Width="10%">
                                                        <HeaderTemplate>
                                                       <asp:CheckBox ID="chkSelectAll2" runat="server" Text="SelectAll" AutoPostBack="true" OnCheckedChanged="chkSelectAllFld_CheckedChanged" />
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="fldRows" runat="server" CssClass="chkItem3" AutoPostBack="True"
                                                                OnCheckedChanged="Check_Clicked_Fld" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField Visible="False">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbAutoIDfld" runat="server" Text='<%#Eval("fld_uuid")%>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Folder" HeaderStyle-Width="100%" ItemStyle-Width="100%">
                                                        <ItemTemplate>
                                                            <%#Eval("fld_name")%></ItemTemplate>
                                                        <HeaderStyle Width="100%" HorizontalAlign="Left" CssClass="GVRec"></HeaderStyle>
                                                        <ItemStyle Width="100%" HorizontalAlign="Left" CssClass="GVRec"></ItemStyle>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                    </div>
                                    <div style="float: left; width: 220px; height: 300px; overflow: auto; background-color: Gray;
                                        padding-left: 3px; padding-right: 3px; font-size: 12px; font-weight: bold;">
                                        <div style="float: left; background-color: Black; width: 97%; margin-top: 5px; height: 20px;
                                            padding-left: 5px; line-height: 20px; margin-bottom: 5px; color: White">
                                            Document</div>
                                        <div style="float: left; background-color: Black; width: 100%; margin-bottom: 3px;">
                                            <asp:GridView ID="gvDoc" runat="server" Style="border: 1px solid #000;" Width="100%"
                                                BackColor="White" CellPadding="4" ForeColor="Black" GridLines="Vertical" AutoGenerateColumns="False"
                                                EnableModelValidation="True" DataKeyNames="uuid">
                                                <RowStyle BackColor="#F7F7DE" HorizontalAlign="Center" />
                                                <FooterStyle BackColor="#CCCC99" />
                                                <SelectedRowStyle BackColor="#CE5D5A" Font-Bold="True" ForeColor="White" HorizontalAlign="Center" />
                                                <HeaderStyle BackColor="#cccccc" Font-Bold="True" HorizontalAlign="Center" />
                                                <AlternatingRowStyle BackColor="White" />
                                                <Columns>
                                                    <asp:TemplateField HeaderStyle-Width="10%" ItemStyle-Width="10%">
                                                        <HeaderTemplate>
                                                           <asp:CheckBox ID="chkSelectAll3" runat="server" Text="SelectAll" AutoPostBack="true" OnCheckedChanged="chkSelectAllDoc_CheckedChanged" />
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="docRows" runat="server" CssClass="chkItem4" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField Visible="False">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbAutoIDdoc" runat="server" Text='<%#Eval("uuid")%>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Document" HeaderStyle-Width="100%" ItemStyle-Width="100%">
                                                        <ItemTemplate>
                                                            <%#Eval("doc_name")%></ItemTemplate>
                                                        <HeaderStyle Width="100%" HorizontalAlign="Left" CssClass="GVRec"></HeaderStyle>
                                                        <ItemStyle Width="100%" HorizontalAlign="Left" CssClass="GVRec"></ItemStyle>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                    </div>
                                </div>
                            </ContentTemplate>
                            </asp:UpdatePanel>
                        <div class="normal_common">
                            <span class="wf_det_add_holder">
                                <div style="width: 860px; float: left; line-height: 24px;">
                                    <div style="-moz-border-radius: 5px; -webkit-border-radius: 5px; -khtml-border-radius: 5px;
                                        border-radius: 5px; border: 1px solid #000; padding: 5px; width: 340px;">
                                        <div class="OptBtn">
                                            <asp:RadioButton ID="OptNo" Text=" No Permission" runat="server" Checked="true" GroupName="OptPermission" /></div>
                                        <div class="OptBtn">
                                            <asp:RadioButton ID="OptView" Text=" View (Only View Option will be Allowed)" runat="server"
                                                GroupName="OptPermission" /></div>
                                        <div class="OptBtn">
                                            <asp:RadioButton ID="OptModify" Text=" Modify (View and Modify Options will be Allowed)"
                                                runat="server" GroupName="OptPermission" /></div>
                                        <div class="OptBtn">
                                            <asp:RadioButton ID="OptDelete" Text=" Delete (View, Modify and Delete Options will be Allowed)"
                                                runat="server" GroupName="OptPermission" /></div>
                                    </div>
                                </div>
                                <asp:Button ID="cmdAllow" runat="server" CssClass="wf_mas_add" Text="Allow" OnClick="cmdAllow_Click" />
                            </span>
                        </div>
                        <div style="margin-top: 20px; display:block;" class="normal_common">
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                              <%--  <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="gvRole" EventName="SelectedIndexChanged" />
                                </Triggers>--%>
                                <ContentTemplate>
                                    <div style="float:left; width:220px; height:300px; overflow:auto; background-color:Gray;
                                        padding-left:3px; padding-right:3px; font-size:12px; font-weight:bold; margin-right:10px;">
                                        <div style="float:left; background-color:Black; width:97%; margin-top:5px; height:20px;
                                            padding-left:5px; line-height:20px; margin-bottom:5px; color:White">
                                            Cabinet</div>
                                        <div style="float:left; background-color:Black; width:100%; margin-bottom:3px;">
                                            <asp:GridView ID="gvDispCab" runat="server" Style="border:1px solid #000;" Width="100%"
                                                BackColor="White" CellPadding="4" ForeColor="Black" GridLines="Vertical" AutoGenerateColumns="False"
                                                CaptionAlign="Top" EnableModelValidation="True">
                                                <RowStyle BackColor="#F7F7DE" HorizontalAlign="Center" />
                                                <FooterStyle BackColor="#CCCC99" />
                                                <SelectedRowStyle BackColor="#CE5D5A" Font-Bold="True" ForeColor="White" HorizontalAlign="Center" />
                                                <HeaderStyle BackColor="#cccccc" Font-Bold="True" HorizontalAlign="Center" />
                                                <AlternatingRowStyle BackColor="White" />
                                                <Columns>
                                                    <asp:TemplateField Visible="False">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbCabID" runat="server" Text='<%#Eval("NodeUUID")%>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Cabinet">
                                                        <ItemTemplate>
                                                            <%#Eval("cab_name")%></ItemTemplate>
                                                        <HeaderStyle HorizontalAlign="Left" CssClass="GVRec"></HeaderStyle>
                                                        <ItemStyle HorizontalAlign="Left" CssClass="GVRec"></ItemStyle>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Permission">
                                                        <ItemTemplate>
                                                            <%#Eval("Permission")%></ItemTemplate>
                                                        <HeaderStyle HorizontalAlign="Left" CssClass="GVRec"></HeaderStyle>
                                                        <ItemStyle HorizontalAlign="Left" CssClass="GVRec"></ItemStyle>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                    </div>
                                    <div style="float:left; width:220px; height:300px; overflow:auto; background-color:Gray;
                                        padding-left:3px; padding-right:3px; font-size:12px; font-weight:bold; margin-right:10px;">
                                        <div style="float:left; background-color:Black; width:97%; margin-top:5px; height:20px;
                                            padding-left:5px; line-height:20px; margin-bottom:5px; color:White">
                                            Drawer</div>
                                        <div style="float:left; background-color:Black; width:100%; margin-bottom:3px;">
                                            <asp:GridView ID="gvDispDrw" runat="server" Style="border:1px solid #000;" Width="100%"
                                                BackColor="White" CellPadding="4" ForeColor="Black" GridLines="Vertical" AutoGenerateColumns="False"
                                                CaptionAlign="Top" EnableModelValidation="True">
                                                <RowStyle BackColor="#F7F7DE" HorizontalAlign="Center" />
                                                <FooterStyle BackColor="#CCCC99" />
                                                <SelectedRowStyle BackColor="#CE5D5A" Font-Bold="True" ForeColor="White" HorizontalAlign="Center" />
                                                <HeaderStyle BackColor="#cccccc" Font-Bold="True" HorizontalAlign="Center" />
                                                <AlternatingRowStyle BackColor="White" />
                                                <Columns>
                                                    <asp:TemplateField Visible="False">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbDrwID" runat="server" Text='<%#Eval("NodeUUID")%>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Drawer">
                                                        <ItemTemplate>
                                                            <%#Eval("drw_name")%></ItemTemplate>
                                                        <HeaderStyle HorizontalAlign="Left" CssClass="GVRec"></HeaderStyle>
                                                        <ItemStyle HorizontalAlign="Left" CssClass="GVRec"></ItemStyle>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Permission">
                                                        <ItemTemplate>
                                                            <%#Eval("Permission")%></ItemTemplate>
                                                        <HeaderStyle HorizontalAlign="Left" CssClass="GVRec"></HeaderStyle>
                                                        <ItemStyle HorizontalAlign="Left" CssClass="GVRec"></ItemStyle>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                    </div>
                                    <div style="float:left; width:220px; height:300px; overflow:auto; background-color:Gray;
                                        padding-left:3px; padding-right:3px; font-size:12px; font-weight:bold; margin-right:10px;">
                                        <div style="float:left; background-color:Black; width:97%; margin-top:5px; height:20px;
                                            padding-left:5px; line-height:20px; margin-bottom:5px; color:White">
                                            Folder</div>
                                        <div style="float:left; background-color:Black; width:100%; margin-bottom:3px;">
                                            <asp:GridView ID="gvDispFld" runat="server" Style="border:1px solid #000;" Width="100%"
                                                BackColor="White" CellPadding="4" ForeColor="Black" GridLines="Vertical" AutoGenerateColumns="False"
                                                CaptionAlign="Top" EnableModelValidation="True">
                                                <RowStyle BackColor="#F7F7DE" HorizontalAlign="Center" />
                                                <FooterStyle BackColor="#CCCC99" />
                                                <SelectedRowStyle BackColor="#CE5D5A" Font-Bold="True" ForeColor="White" HorizontalAlign="Center" />
                                                <HeaderStyle BackColor="#cccccc" Font-Bold="True" HorizontalAlign="Center" />
                                                <AlternatingRowStyle BackColor="White" />
                                                <Columns>
                                                    <asp:TemplateField Visible="False">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbFldID" runat="server" Text='<%#Eval("NodeUUID")%>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Folder">
                                                        <ItemTemplate>
                                                            <%#Eval("fld_name")%></ItemTemplate>
                                                        <HeaderStyle HorizontalAlign="Left" CssClass="GVRec"></HeaderStyle>
                                                        <ItemStyle HorizontalAlign="Left" CssClass="GVRec"></ItemStyle>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Permission">
                                                        <ItemTemplate>
                                                            <%#Eval("Permission")%></ItemTemplate>
                                                        <HeaderStyle HorizontalAlign="Left" CssClass="GVRec"></HeaderStyle>
                                                        <ItemStyle HorizontalAlign="Left" CssClass="GVRec"></ItemStyle>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                    </div>
                                    <div style="float:left; width:220px; height:300px; overflow:auto; background-color:Gray;
                                        padding-left:3px; padding-right:3px; font-size:12px; font-weight:bold;">
                                        <div style="float:left; background-color:Black; width:97%; margin-top:5px; height:20px;
                                            padding-left:5px; line-height:20px; margin-bottom:5px; color:White">
                                            Document</div>
                                        <div style="float:left; background-color:Black; width:100%; margin-bottom:3px;">
                                            <asp:GridView ID="gvDispDoc" runat="server" Style="border:1px solid #000;" Width="100%"
                                                BackColor="White" CellPadding="4" ForeColor="Black" GridLines="Vertical" AutoGenerateColumns="False"
                                                CaptionAlign="Top" EnableModelValidation="True">
                                                <RowStyle BackColor="#F7F7DE" HorizontalAlign="Center" />
                                                <FooterStyle BackColor="#CCCC99" />
                                                <SelectedRowStyle BackColor="#CE5D5A" Font-Bold="True" ForeColor="White" HorizontalAlign="Center" />
                                                <HeaderStyle BackColor="#cccccc" Font-Bold="True" HorizontalAlign="Center" />
                                                <AlternatingRowStyle BackColor="White" />
                                                <Columns>
                                                    <asp:TemplateField Visible="False">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbDocID" runat="server" Text='<%#Eval("NodeUUID")%>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Document">
                                                        <ItemTemplate>
                                                            <%#Eval("doc_name")%></ItemTemplate>
                                                        <HeaderStyle HorizontalAlign="Left" CssClass="GVRec"></HeaderStyle>
                                                        <ItemStyle HorizontalAlign="Left" CssClass="GVRec"></ItemStyle>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Permission">
                                                        <ItemTemplate>
                                                            <%#Eval("Permission")%></ItemTemplate>
                                                        <HeaderStyle HorizontalAlign="Left" CssClass="GVRec"></HeaderStyle>
                                                        <ItemStyle HorizontalAlign="Left" CssClass="GVRec"></ItemStyle>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
