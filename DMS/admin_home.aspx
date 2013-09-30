<%@ Page Title="myDOCbase" Language="C#" MasterPageFile="adm_mast_entry.Master" AutoEventWireup="true" CodeBehind="admin_home.aspx.cs" Inherits="DMS.admin_home" %>
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
        function showWFHistLoading() {
            document.getElementById("divWFHistLoading").style.display = 'block';
            document.getElementById("divWFHist").style.display = 'block';
        }
        function hideWFHistLoading() {
            document.getElementById("divWFHistLoading").style.display = 'none';
            document.getElementById("divWFHist").style.display = 'none';
        }
        window.onload = hideWFHistLoading;
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
                    
                    <div class="dropdown" id="divMenuAdmin" runat="server">
                        <div class="menu_bg_n">
                            <ul>
                                <li><a href="userhome.aspx">Home</a></li>
                                <li class="drop_nav"><a href="#">Filing</a>
                                <ul class="drop hide">
                                    <li><a href="home.aspx">e-Filing system</a></li>
                                    <li><a href="cabinet_mast.aspx">Cabinet</a></li>
                                    <li><a href="drawer_mast.aspx">Drawer</a></li>
                                    <li><a href="folder_mast.aspx">Folder</a></li>
                                    <li><a href="doc_type_mast.aspx">Document Type</a></li>
                                    <li><a href="BlankTempUpload.aspx">New Template Upload</a></li>
                                    <li><a href="doc_mast.aspx">New Document Upload</a></li>
                                    <li><a href="grp_mast.aspx">Groups</a></li>
                                    <li><a class="nob" href="grp_doc.aspx">Group-wise Document</a></li>
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

                </div>
            </div>
            <div style="padding-bottom:0px;" class="normal_common">
                <div class="wf_mas_Heading2">
                    Admin Home
                </div>
            </div>
            <div class="normal_common">
                <div class="wf_mas_doc_holder" style="min-height:300px;">
                    <div class="normal_common">
                            
                                    <div style="border:1px solid #999; float:left; width:100%; -moz-border-radius:3px; -webkit-border-radius:3px; border-radius:3px;">
                                        <div style="border-bottom:1px solid #999; padding:0px;" class="normal_common">
                                            <div style="padding:5px; font-size:14px; color:#660066; font-weight:bold; background:#f0eded;">
                                                Workflow Status
                                            </div>
                                        </div>  
                                        <div class="normal_common">
                                            <div style="padding:5px; overflow:auto; height:350px;">
                                                <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>                                                
                                                <asp:GridView ID="gvStartedWF" runat="server" style="border:1px solid #000;" 
                                                        Width="100%" BackColor="White" CellPadding="4" 
                                                ForeColor="Black" GridLines="Vertical" AutoGenerateColumns="False" OnPageIndexChanging="gvStartedWF_PageIndexChanging"
                                                        CaptionAlign="Top" DataKeyNames="wf_log_id,wf_name"
                                                EnableModelValidation="True" 
                                                        onselectedindexchanged="gvStartedWF_SelectedIndexChanged" AllowPaging="True">
                                                <RowStyle BackColor="#F7F7DE" HorizontalAlign="Center" />
                                                <FooterStyle BackColor="#CCCC99" />
                                                    <PagerSettings Position="Top" />
                                                <PagerStyle BackColor="#F7F7DE" ForeColor="Black" />
                                                <SelectedRowStyle BackColor="#dae2ea" Font-Bold="True" ForeColor="Blue" HorizontalAlign="Center" />
                                                <HeaderStyle BackColor="#cccccc" Font-Bold="True" HorizontalAlign="Center" />
                                                <AlternatingRowStyle BackColor="White" />
                                                <Columns>
                                                <asp:TemplateField Visible="false">
                                                <ItemTemplate >
                                                <asp:Label ID="lbWFLogID" runat="server" Text='<%#Eval("wf_log_id")%>' ></asp:Label>
                                                </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Workflow ID">  
                                                <ItemTemplate><%#Eval("wf_log_id")%></ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Left" CssClass="GVRec"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="Left" CssClass="GVRec"></ItemStyle>
                                                </asp:TemplateField>
                                                <asp:TemplateField Visible="False">
                                                <ItemTemplate>
                                                <asp:Label ID="lbWFID" runat="server" Text='<%#Eval("wf_id")%>' ></asp:Label>
                                                </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Workflow ID" Visible="false">  
                                                <ItemTemplate><%#Eval("wf_id")%></ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Left" CssClass="GVRec"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="Left" CssClass="GVRec"></ItemStyle>
                                                </asp:TemplateField>
                                                <asp:TemplateField Visible="False">
                                                <ItemTemplate >
                                                <asp:Label ID="lbWFName" runat="server" Text='<%#Eval("wf_name")%>' ></asp:Label>
                                                </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Workflow Name">  
                                                <ItemTemplate >
                                                    <asp:LinkButton ID="lnkTask" CommandName="Select" OnClientClick="javascript: return showWFHistLoading()" CommandArgument="ShowHist" runat="server" Text='<%#Eval("wf_name")%>'></asp:LinkButton>
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Left" CssClass="GVRec"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="Left" CssClass="GVRec"></ItemStyle>
                                                </asp:TemplateField>
                                                <asp:TemplateField Visible="False">
                                                <ItemTemplate >
                                                <asp:Label ID="lbDocID" runat="server" Text='<%#Eval("doc_id")%>' ></asp:Label>
                                                </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Doc ID" Visible="false">  
                                                <ItemTemplate><%#Eval("doc_id")%></ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Left" CssClass="GVRec"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="Left" CssClass="GVRec"></ItemStyle>
                                                </asp:TemplateField>
                                                <asp:TemplateField Visible="False">
                                                <ItemTemplate >
                                                <asp:Label ID="lbAssignedDt" runat="server" Text='<%#Eval("doc_name")%>' ></asp:Label>
                                                </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Document Name" >  
                                                <ItemTemplate><%#Eval("doc_name")%></ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Left" CssClass="GVRec"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="Left" CssClass="GVRec"></ItemStyle>
                                                </asp:TemplateField>
                                                <asp:TemplateField Visible="False">
                                                <ItemTemplate >
                                                <asp:Label ID="lbStat" runat="server" Text='<%#Eval("wf_prog_stat")%>' ></asp:Label>
                                                </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Progress Status" >  
                                                <ItemTemplate><%#Eval("wf_prog_stat")%></ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Left" CssClass="GVRec"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="Left" CssClass="GVRec"></ItemStyle>
                                                </asp:TemplateField>
                                                <asp:TemplateField Visible="False">
                                                <ItemTemplate >
                                                <asp:Label ID="lbStart" runat="server" Text='<%#Eval("started_by")%>' ></asp:Label>
                                                </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Queued By" >  
                                                <ItemTemplate><%#Eval("started_by")%></ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Left" CssClass="GVRec"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="Left" CssClass="GVRec"></ItemStyle>
                                                </asp:TemplateField>
                                                </Columns>
                                                </asp:GridView>
                                                </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </div>
                                        </div>


                                        <%--Workflow History Start--%>
                                        <div id="divWFHistLoading" style="display:none;" ></div>
                                        <div id="divWFHist" style="display:none;" >
                                        <div class="lightbox_1">
                                        <div class="lightbox_2">
                                        <asp:UpdatePanel ID="UpdatePanel22" runat="server" UpdateMode="Conditional">
                                        <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="gvStartedWF" EventName="selectedindexchanged"></asp:AsyncPostBackTrigger>
                                        </Triggers>
                                        <ContentTemplate>
                                        <div class="lightbox_3">
                        
                                        <div class="normal_feature">
                                        <div style=" width:100%; float:left; padding-top:20px;">
                                        <asp:GridView style="border:1px solid #000;" Width="100%" BackColor="White" CellPadding="4" ForeColor="Black" ID="gv" runat="server">
                                        <RowStyle BackColor="#F7F7DE" HorizontalAlign="Center" />
                                        <FooterStyle BackColor="#CCCC99" />
                                        <PagerStyle BackColor="#F7F7DE" ForeColor="Black"  />
                                        <SelectedRowStyle BackColor="#CE5D5A" Font-Bold="True" ForeColor="White" HorizontalAlign="Center" />
                                        <HeaderStyle BackColor="#cccccc" Font-Bold="True" HorizontalAlign="Center"  />
                                        <AlternatingRowStyle BackColor="White"  />
                            
                                        </asp:GridView>
                         
                                        </div>
                                         <div class="normal_feature">
                                        <div style="color:#FF0000; text-align:center; font-weight:bold; font-size:16px; background:#000000; border:1px solid #999999; padding:10px;">
                                            <asp:Label ID="MsgNodet" runat="server"></asp:Label> 
                                        </div>
                                        </div>
                                        <div  class="normal_feature" style="text-align:right;">                         
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
</asp:Content>
