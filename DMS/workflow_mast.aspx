<%@ Page Title="myDOCbase" Language="C#" MasterPageFile="adm_mast_entry.Master" AutoEventWireup="true" CodeBehind="workflow_mast.aspx.cs" Inherits="DMS.workflow_mast" %>
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
        var ActToMail = new String();

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
        });

        function showLoading() {            
            document.getElementById("dvLoading1").style.display = 'block';
            document.getElementById("dvLoading2").style.display = 'block';
        }
        function hideLoading() {
            document.getElementById("dvLoading1").style.display = 'none';
            document.getElementById("dvLoading2").style.display = 'none';
        }

        function showLoading1() {
            document.getElementById("Div1").style.display = 'block';
            document.getElementById("Div2").style.display = 'block';
        }
        function SaveCopyPopup() {
            if (document.getElementById('<%=ddCabinet2.ClientID%>').selectedIndex == -1) {
                alert("Please select a Cabinet !!");
            }
            else if (document.getElementById('<%=ddDrawer2.ClientID%>').selectedIndex == -1) {
                alert("Please select a Drawer !!");
            }
            else if (document.getElementById('<%=ddFolder2.ClientID%>').selectedIndex == -1) {
                alert("Please select a Folder !!");
            }
            else {
                document.getElementById("Div1").style.display = 'none';
                document.getElementById("Div2").style.display = 'none';
            }
        }
        function CancelCopyPopup() {
            document.getElementById("Div1").style.display = 'none';
            document.getElementById("Div2").style.display = 'none';
        }

        function showLoadingAmbleMail() {
            document.getElementById("divAmbleMail1").style.display = 'block';
            document.getElementById("divAmbleMail2").style.display = 'block';
        }
        function hideLoadingAmbleMail() {
            document.getElementById("divAmbleMail1").style.display = 'none';
            document.getElementById("divAmbleMail2").style.display = 'none';
        }
        function hideAmbleMail(emailcntl, seperator) {
            var value1 = emailcntl.value;
            value1 = document.getElementById("<%= txtToMail.ClientID %>").value;
            var Flag = "2";
            if (document.getElementById("<%=txtToMail.ClientID%>").value == "") {
                alert("Please Enter Email ID !!");
            }
            else if (document.getElementById("<%=txtAmbleSub.ClientID%>").value == "") {
                alert("Please Enter Email Subject !!");
            }
            else if (document.getElementById("<%=txtAmbleMsg.ClientID%>").value == "") {
                alert("Please Enter Email Message !!");
            }
            else {
                if (value1 != '') {
                    var result1 = value1.split(seperator);
                    for (var i = 0; i < result1.length; i++) {
                        if (result1[i] != '') {
                            if (!validateEmail(result1[i])) {
                                Flag = "0";
                                break;
                            }
                            else {
                                Flag = "1";
                            }
                        }
                    }
                }
                if (Flag == "0") {
                    alert('Please check, email addresses not valid!');
                }
                else if (Flag == "1") {
                    document.getElementById("divAmbleMail1").style.display = 'none';
                    document.getElementById("divAmbleMail2").style.display = 'none';
                }
            }
        }
        function validateEmail(field) {
            var regex = /^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,5}$/;
            return (regex.test(field)) ? true : false;
        }

        function showLoadingCondMail() {
            document.getElementById("divCondMail1").style.display = 'block';
            document.getElementById("divCondMail2").style.display = 'block';
        }
        function hideLoadingCondMail() {
            document.getElementById("divCondMail1").style.display = 'none';
            document.getElementById("divCondMail2").style.display = 'none';
        }

        function showLoadingSignDate() {
            document.getElementById("divSignDate1").style.display = 'block';
            document.getElementById("divSignDate2").style.display = 'block';
        }
        function hideLoadingSignDate() {
            document.getElementById("divSignDate1").style.display = 'none';
            document.getElementById("divSignDate2").style.display = 'none';
        }
        function showLoadingAppendDoc() {
            document.getElementById("divAppendDoc1").style.display = 'block';
            document.getElementById("divAppendDoc2").style.display = 'block';
        }
        function hideLoadingAppendDoc() {
            if (document.getElementById('<%=ddCabinet3.ClientID%>').selectedIndex == -1) {
                alert("Please select a Cabinet !!");
            }
            else if (document.getElementById('<%=ddDrawer3.ClientID%>').selectedIndex == -1) {
                alert("Please select a Drawer !!");
            }
            else if (document.getElementById('<%=ddFolder3.ClientID%>').selectedIndex == -1) {
                alert("Please select a Folder !!");
            }
            else if (document.getElementById('<%=ddDocument3.ClientID%>').selectedIndex == -1) {
                alert("Please select a Document !!");
            }
            else {
                document.getElementById("divAppendDoc1").style.display = 'none';
                document.getElementById("divAppendDoc2").style.display = 'none';
            }
        }
        function CancelAppendDoc() {
            document.getElementById("divAppendDoc1").style.display = 'none';
            document.getElementById("divAppendDoc2").style.display = 'none';
        }

        function AddMail() {
            var TextBoxVal = document.getElementById("<%= txtToMail.ClientID %>");
            var Flag = true;
            var e = document.getElementById("ctl00_ContentPlaceHolder1_ddUser");            
            var strMail = e.options[e.selectedIndex].value;            
            var Arr = new Array();
            Arr = ActToMail.split(',');
            for (var i = 0; i < Arr.length; i++) {
                if (Arr[i] == strMail) {
                    Flag = false;
                    break;
                }
            }
            if (Flag) {
                ActToMail += ActToMail == "" ? strMail : "," + strMail;
            }
            TextBoxVal.value = ActToMail.toString();
        }
        function AddMail1() {
            var TextBoxVal = document.getElementById("<%= txtToMail1.ClientID %>");
            var Flag = true;
            var e = document.getElementById("ctl00_ContentPlaceHolder1_ddUser1");
            var strMail = e.options[e.selectedIndex].value;
            var Arr = new Array();
            Arr = ActToMail.split(',');
            for (var i = 0; i < Arr.length; i++) {
                if (Arr[i] == strMail) {
                    Flag = false;
                    break;
                }
            }
            if (Flag) {
                ActToMail += ActToMail == "" ? strMail : "," + strMail;
            }
            TextBoxVal.value = ActToMail.toString();
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <asp:UpdateProgress ID="UpdateProgress1" runat="server">
    <ProgressTemplate>
      <div style="position: fixed; width:1019px; height:100%; background:#999; opacity:.3; filter:alpha(opacity=30); z-index:103;"></div>
        <div style=" width:100px; height:100px; position:absolute; top:50%; left:47%; z-index:104;"><asp:Image runat="server" ID="imgBusy" ImageUrl="images/busy.gif" /></div>
    </ProgressTemplate>
    </asp:UpdateProgress>
    <asp:HiddenField ID="hdCounter" runat="server" />
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
                    Workflow Definition
                </div>
            </div>
            <div class="normal_common">
                <div class="wf_mas_doc_holder" style="min-height:300px;">
                    <div class="wf_mas_doc_holder_padd">
                        <div class="normal_common" id="divCompany" runat="server">
                            <span class="wf_mas_cap">Company</span>
                            <asp:DropDownList ID="ddCompany" CssClass="wf_det_field2_big" runat="server" AutoPostBack="true" onselectedindexchanged="ddCompany_SelectedIndexChanged"></asp:DropDownList>
                        </div>
                        <div class="normal_common">
                            <span class="wf_mas_cap">Workflow Name</span>
                            <asp:TextBox ID="txtWFName" runat="server" CssClass="wf_mas_field"></asp:TextBox>
                            <span style="padding-left:5px; float:left; font-weight:bold; font-size:24px; color:Red;">*</span>
                        </div>
                        <div class="normal_common">
                            <span class="wf_mas_cap">Department</span>
                            <asp:DropDownList ID="ddDept" CssClass="wf_det_field2_big" runat="server"></asp:DropDownList>
                            <span style="padding-left:5px; float:left; font-weight:bold; font-size:24px; color:Red;">*</span>
                        </div>
                        <div class="normal_common">
                            <span class="wf_mas_cap">Document Type</span>
                            <asp:DropDownList ID="ddDocType" CssClass="wf_det_field2_big" runat="server"></asp:DropDownList>
                            <span style="padding-left:5px; float:left; font-weight:bold; font-size:24px; color:Red;">*</span>
                        </div>
                        <div class="normal_common">
                            <span class="wf_mas_cap">Cabinet</span>
                            <asp:UpdatePanel runat="server" id="UpdatePanel13">
                                <ContentTemplate>
                                    <asp:DropDownList ID="ddCabinet" CssClass="wf_det_field2_big" runat="server" AutoPostBack="true" onselectedindexchanged="ddCabinet_SelectedIndexChanged"></asp:DropDownList>
                                    <span style="padding-left:5px; float:left; font-weight:bold; font-size:24px; color:Red;">*</span>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                        <div class="normal_common">
                            <span class="wf_mas_cap">Drawer</span>
                            <asp:UpdatePanel runat="server" id="UpdatePanel14">
                                <ContentTemplate>
                                    <asp:DropDownList ID="ddDrawer" CssClass="wf_det_field2_big" runat="server" AutoPostBack="true" onselectedindexchanged="ddDrawer_SelectedIndexChanged"></asp:DropDownList>
                                    <span style="padding-left:5px; float:left; font-weight:bold; font-size:24px; color:Red;">*</span>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                        <div class="normal_common">
                            <span class="wf_mas_cap">Folder</span>
                            <asp:UpdatePanel runat="server" id="UpdatePanel15">
                                <ContentTemplate>
                                    <asp:DropDownList ID="ddFolder" CssClass="wf_det_field2_big" runat="server"></asp:DropDownList>
                                    <span style="padding-left:5px; float:left; font-weight:bold; font-size:24px; color:Red;">*</span>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                            <span class="wf_det_add_holder"> 
                            <asp:Button ID="cmdAddMaster" runat="server" CssClass="wf_mas_add" Text="Add" onclick="cmdAddMaster_Click" />
                        </span>
                        </div>
                        <div class="normal_common" style="text-align:center; font-weight:bold; font-size:14px; color:#a09c9c;">
                            <asp:Label ID="lblHeading" runat="server" Text=""></asp:Label>
                        </div>
                        <div class="normal_common">
                            <table class="wf_det_tab" cellpadding="0" cellspacing="0" width="100%"  >
                                <tr>
                                    <td class="wf_det_tab_heading" width="40">Stage</td>
                                    <td class="wf_det_tab_heading" width="120">Role</td>
                                    <td class="wf_det_tab_heading" width="70">Time (Hrs)</td>
                                    <td class="wf_det_tab_heading" width="160">Task</td>
                                    <td style="display:none;" class="wf_det_tab_heading" width="80">Action Type</td>
                                    <td class="wf_det_tab_heading" width="50">Actions</td>
                                    <td class="wf_det_tab_heading" width="30">&nbsp;</td>
                                    <td class="wf_det_tab_heading">Assigned Tasks</td>
                                    <td class="wf_det_tab_heading" width="115">Add to List</td>
                                </tr>
                                <tr>
                                    <td valign="top" class="wf_det_tab_bold">
                                        <asp:TextBox ID="txtStage1" CssClass="wf_det_field" runat="server" Text="1" ReadOnly="true"></asp:TextBox>
                                    </td>
                                    <td valign="top" class="wf_det_tab_bold">
                                        <asp:UpdatePanel runat="server" id="UpdatePanel11">
                                            <ContentTemplate>
                                                <asp:DropDownList ID="ddRole1" CssClass="wf_det_field2" runat="server" AutoPostBack="true"
                                            onselectedindexchanged="ddRole1_SelectedIndexChanged"></asp:DropDownList>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                    <td valign="top" class="wf_det_tab_bold">
                                        <asp:UpdatePanel runat="server" id="UpdatePanel12">
                                            <ContentTemplate>
                                                <asp:TextBox ID="txtTime" CssClass="wf_det_field" runat="server" Text="168" Width="60px"></asp:TextBox>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                    <td valign="top" class="wf_det_tab_bold">  
                                        <asp:UpdatePanel runat="server" id="UpdatePanel10">
                                            <ContentTemplate>
                                                <asp:DropDownList ID="ddTask1" CssClass="wf_det_field3" runat="server" AutoPostBack="true" 
                                            onselectedindexchanged="ddTask1_SelectedIndexChanged"></asp:DropDownList>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                    <td valign="top" class="wf_det_tab_bold" style="display:none;">  
                                        <asp:UpdatePanel runat="server" id="UpdatePanel16">
                                            <ContentTemplate>
                                            <asp:DropDownList ID="ddActType1" CssClass="wf_det_field_m" runat="server" Enabled="false">
                                                <asp:ListItem Value="Interactive" Text="Interactive"></asp:ListItem>
                                                <asp:ListItem Value="Preamble" Text="Preamble"></asp:ListItem>
                                                <asp:ListItem Value="Postamble" Text="Postamble"></asp:ListItem>
                                            </asp:DropDownList>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                    <td class="wf_det_tab_bold">
                                        <div class="mul_btn">
                                            <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Always">
                                                <ContentTemplate>
                                                    <asp:HiddenField ID="hfMsg" runat="server" />
                                                    <asp:ImageButton CssClass="mul_img_btn" ID="cmdAddCopyLoc" runat="server" ToolTip="Set Copy Location" ImageUrl="images/copy_location.png" OnClientClick="javascript: return showLoading1()"/>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                            <asp:UpdatePanel ID="UpdatePanel6" runat="server" UpdateMode="Always">
                                                <ContentTemplate>
                                                    <asp:ImageButton CssClass="mul_img_btn" ID="cmdAddAmbleMail" runat="server" ToolTip="Set Mail ID" ImageUrl="images/sendemail.png" OnClientClick="javascript: return showLoadingAmbleMail()"/>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                            <asp:UpdatePanel ID="UpdatePanel7" runat="server" UpdateMode="Always">
                                                <ContentTemplate>
                                                    <asp:ImageButton CssClass="mul_img_btn" ID="cmdAddCondMail" runat="server" ToolTip="Set Conditions" ImageUrl="images/conditions.png" OnClientClick="javascript: return showLoadingCondMail()"/>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                            <asp:UpdatePanel ID="UpdatePanel8" runat="server" UpdateMode="Always">
                                                <ContentTemplate>
                                                    <asp:ImageButton CssClass="mul_img_btn" ID="cmdAppendDoc" runat="server" ToolTip="Set Appended Documents" ImageUrl="images/append.png" OnClientClick="javascript: return showLoadingAppendDoc()"/>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                            <asp:UpdatePanel ID="UpdatePanel20" runat="server" UpdateMode="Always">
                                                <ContentTemplate>
                                                    <asp:ImageButton CssClass="mul_img_btn" ID="cmdSign" runat="server" ToolTip="Set Signature Fields" ImageUrl="images/sign.png" OnClientClick="javascript: return showLoadingSignDate()"/>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </div>
                                    </td>
                                    <td class="wf_det_tab_bold">
                                        <asp:ImageButton ID="cmdAddList1" runat="server" ToolTip="Add Task" ImageUrl="images/add_arrow.png" onclick="cmdAddList1_Click"/>
                                    </td>
                                    <td class="show_close">
                                        <asp:Label ID="lblTaskList1" runat="server" Text=""></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Button ID="cmdAddDtl" runat="server" Text="Add to Detail" CssClass="wf_mas_add" onclick="cmdAddDtl_Click" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div class="normal_common" style="text-align:center; font-weight:bold; font-size:14px; color:#a09c9c;">
                            <asp:Label ID="lblWFDtls" runat="server" Text=""></asp:Label>
                        </div>
                        <div style="padding:0px;" class="normal_common">
                            <asp:GridView ID="gvDtl" runat="server" style="border:1px solid #000;" Width="100%" BackColor="White" CellPadding="4" 
                               ForeColor="Black" GridLines="Vertical" AutoGenerateColumns="False" AutoGenerateEditButton="false"
                               AutoGenerateDeleteButton="false" OnRowDataBound="gvDispRec_RowDataBound" onrowediting="gvDtl_RowEditing" 
                               onrowcancelingedit="gvDtl_RowCancelingEdit" onrowupdating="gvDtl_RowUpdating" onrowdeleting="gvDtl_RowDeleting" CaptionAlign="Top">
                               <RowStyle BackColor="#F7F7DE" HorizontalAlign="Center"/>
                               <FooterStyle BackColor="#CCCC99"/>
                               <PagerStyle BackColor="#F7F7DE" ForeColor="Black"/>
                               <SelectedRowStyle BackColor="#dae2ea" Font-Bold="True" ForeColor="Blue" HorizontalAlign="Center"/>
                               <HeaderStyle BackColor="#cccccc" Font-Bold="True" HorizontalAlign="Center"  />
                               <AlternatingRowStyle BackColor="White"/>
                             <Columns>
                             <asp:CommandField HeaderText="Action" ShowDeleteButton="true" HeaderStyle-Width="60" ItemStyle-Width="60"/>
                                <asp:TemplateField HeaderText="Stage No." HeaderStyle-Width="60" ItemStyle-Width="60">  
                                    <ItemTemplate ><asp:Label ID="lbl1" runat="server"></asp:Label></ItemTemplate>                                   
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Backend Stage No." HeaderStyle-Width="60" ItemStyle-Width="60" Visible="false">  
                                    <ItemTemplate ><%#Eval("step_no")%></ItemTemplate>                                   
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Workflow Name" HeaderStyle-Width="150" ItemStyle-Width="150">  
                                    <ItemTemplate ><%#Eval("wf_name")%></ItemTemplate>                                   
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Role Name" HeaderStyle-Width="150" ItemStyle-Width="150">  
                                    <ItemTemplate ><%#Eval("role_name")%></ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Time (Hrs)" HeaderStyle-Width="100" ItemStyle-Width="100">  
                                    <ItemTemplate ><%#Eval("duration")%></ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Task" HeaderStyle-Width="300" ItemStyle-Width="300">  
                                    <ItemTemplate ><%#Eval("Task")%></ItemTemplate>   
                                </asp:TemplateField>
                               
                             </Columns>
                           </asp:GridView>
                        </div>
                        <div class="normal_common">
                            <label>
                                <asp:ImageButton ID="cmdFinalAdd" runat="server" ImageUrl="images/add_btn.png" onclick="cmdFinalAdd_Click" />
                            </label>
                        </div>
                        <div style="margin-top:20px;" class="normal_common">
                            <div style="overflow:auto; height:300px;">
                                <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Always">                                
                                <ContentTemplate>
                                    <asp:GridView ID="gvDispRec" runat="server" style="border:1px solid #000;" Width="100%" BackColor="White" CellPadding="4" 
                                    ForeColor="Black" GridLines="Vertical" AutoGenerateColumns="False"  OnPageIndexChanging="gvDispRec_PageIndexChanging"
                                    onrowediting="gvDispRec_RowEditing" onrowcancelingedit="gvDispRec_RowCancelingEdit" OnRowDataBound="gvDispRec_RowDataBound" onrowupdating="gvDispRec_RowUpdating" 
                                    onselectedindexchanged="gvDispRec_SelectedIndexChanged" onrowdeleting="gvDispRec_RowDeleting" CaptionAlign="Top" DataKeyNames="wf_id" 
                                    EnableModelValidation="True" AllowPaging="True">
                                    <RowStyle BackColor="#F7F7DE" HorizontalAlign="Center" />
                                    <FooterStyle BackColor="#CCCC99" />
                                    <PagerSettings Position="Top" />
                                    <PagerStyle BackColor="#F7F7DE" ForeColor="Black" />
                                    <SelectedRowStyle BackColor="#dae2ea" Font-Bold="True" ForeColor="Blue" HorizontalAlign="Center" />
                                    <HeaderStyle BackColor="#8b8b8b" Font-Bold="True" HorizontalAlign="Center" ForeColor="White"  />
                                    <AlternatingRowStyle BackColor="White" />
                                    <Columns>
                                        <asp:TemplateField Visible="False">  
                                            <ItemTemplate >    
                                                <asp:Label ID="lbWfid" runat="server" Text='<%#Eval("wf_id")%>' ></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Workflow" HeaderStyle-Width="200" ItemStyle-Width="200">  
                                            <ItemTemplate ><%#Eval("wf_name")%></ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox ID="txtEditWFName" runat="server" Text='<%#Eval("wf_name")%>' Width="100%"></asp:TextBox>
                                            </EditItemTemplate>
                                            <HeaderStyle Width="200px" HorizontalAlign="Left" CssClass="GVRec"></HeaderStyle>
                                            <ItemStyle Width="200px" HorizontalAlign="Left" CssClass="GVRec"></ItemStyle>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Department" HeaderStyle-Width="200" ItemStyle-Width="200" >  
                                            <ItemTemplate > <%#Eval("dept_name")%></ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:DropDownList ID="ddlEditDept" CssClass="wf_det_field2" runat="server" Width="100%"></asp:DropDownList>
                                                <asp:HiddenField ID="hdDept" runat="server" Value='<%#Eval("dept_name")%>' />
                                            </EditItemTemplate>
                                            <HeaderStyle Width="200px" HorizontalAlign="Left" CssClass="GVRec"></HeaderStyle>
                                            <ItemStyle Width="200px" HorizontalAlign="Left" CssClass="GVRec"></ItemStyle>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Document Type" HeaderStyle-Width="500" ItemStyle-Width="500" >  
                                            <ItemTemplate > <%#Eval("doc_type_name")%></ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:DropDownList ID="ddlEditDocType" CssClass="wf_det_field2" runat="server" Width="100%"></asp:DropDownList>
                                                <asp:HiddenField ID="hdDocType" runat="server" Value='<%#Eval("doc_type_name")%>' />
                                            </EditItemTemplate>
                                            <HeaderStyle Width="500px" HorizontalAlign="Left" CssClass="GVRec"></HeaderStyle>
                                            <ItemStyle Width="500px" HorizontalAlign="Left" CssClass="GVRec"></ItemStyle>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="View" HeaderStyle-Width="200" ItemStyle-Width="200" >  
                                            <ItemTemplate >                             
                                                <asp:LinkButton ID="LinkButton1" runat="server" CommandName="Select" Text="View Details" OnClientClick="javascript: return showLoading();"  tooltip ='show details'></asp:LinkButton>
                                            </ItemTemplate> 
                                            <HeaderStyle Width="200px" />
                                            <ItemStyle Width="200px" />
                                        </asp:TemplateField>
                                        <asp:CommandField HeaderText="Action" ShowDeleteButton="True" ShowEditButton="True" />
                                    </Columns> 
                                    </asp:GridView>
                                </ContentTemplate>
                                </asp:UpdatePanel>
                           </div>
                        </div>

                        <%--Workflow Details View Start--%>
                        <div id="dvLoading1"></div>
                        <div style="top:-100px;" id="dvLoading2">
                        <div class="lightbox_1">
                        
                        <div class="lightbox_2">
                        <asp:UpdatePanel ID="UpdatePanel22" runat="server" UpdateMode="Conditional">
                        <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="gvDispRec" EventName="selectedindexchanged"></asp:AsyncPostBackTrigger>
                        </Triggers>
                        <ContentTemplate>
                        <div class="lightbox_3">
                        <div style="float:left; font-weight:bold; border-bottom:1px solid #666; font-size:16px; width:100%;">
                            Workflow Details
                        </div>
                        <div class="normal_feature">
                        <asp:Label CssClass="lightbox_label" ID="Lblwf" runat="server" Text=""></asp:Label><asp:Label CssClass="lightbox_label2" ID="lblWfname" runat="server" Text=""></asp:Label></div>
                        <div class="normal_feature"><asp:Label CssClass="lightbox_label" ID="Lbldept" runat="server"  Text=""></asp:Label><asp:Label CssClass="lightbox_label2" ID="lblWfdept" runat="server" Text=""></asp:Label></div>
                        <div class="normal_feature"><asp:Label CssClass="lightbox_label" ID="Lbldoc" runat="server" Text=""></asp:Label><asp:Label CssClass="lightbox_label2" ID="lblWfDoctype" runat="server" Text=""></asp:Label></div>
                        <div class="normal_feature"><asp:Label CssClass="lightbox_label" ID="LblFld" runat="server" Text=""></asp:Label><asp:Label CssClass="lightbox_label2" ID="lblWfFld" runat="server" Text=""></asp:Label></div>
                        
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
                    </div>
                </div>
            </div>
        </div>
    </div>

    

    <%--Copy to Location Popup Start--%>
    <div class="dvpopup1" id="Div1"></div>
    <div class="dvpopup2" style="top:-100px;" id="Div2">
    <div class="lightbox_1">
    <div class="lightbox_2">    
        <div class="lightbox_3">
            <div style="width:100%; float:left; padding-top:10px;">
                <asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="always">
                <ContentTemplate>
                <div style="float:left; font-weight:bold; border-bottom:1px solid #666; font-size:16px; width:100%;">
                    Select Location
                </div>
                <div class="normal_feature">
                    <div style=" width:100%; float:left; padding-top:20px;">
                        <div style="float:left; width:100%;">
                            <span class="wf_mas_cap">Cabinet</span>
                            <asp:DropDownList ID="ddCabinet2" CssClass="wf_det_field2_big" runat="server" AutoPostBack="true" onselectedindexchanged="ddCabinet2_SelectedIndexChanged"></asp:DropDownList>
                            <span style="padding-left:5px; float:left; font-weight:bold; font-size:24px; color:Red;">*</span>
                        </div>
                        <div style="float:left; width:100%;">
                            <span class="wf_mas_cap">Drawer</span>
                            <asp:DropDownList ID="ddDrawer2" CssClass="wf_det_field2_big" runat="server" AutoPostBack="true" onselectedindexchanged="ddDrawer2_SelectedIndexChanged"></asp:DropDownList>
                            <span style="padding-left:5px; float:left; font-weight:bold; font-size:24px; color:Red;">*</span>
                        </div>
                        <div style="float:left; width:100%;">
                            <span class="wf_mas_cap">Folder</span>
                            <asp:DropDownList ID="ddFolder2" CssClass="wf_det_field2_big" runat="server"></asp:DropDownList>
                            <span style="padding-left:5px; float:left; font-weight:bold; font-size:24px; color:Red;">*</span>
                        </div>
                    </div>
                </div>
                <div class="normal_feature">
                    <div style="color:#FF0000; text-align:center; font-weight:bold; font-size:16px; border-bottom:1px solid #666; padding:10px;">
                    </div>
                </div>
                <div class="normal_feature" style="text-align:right;">
                    <div style="float:left;"><asp:Button ID="cmdLocSave" runat="server" CssClass="wf_mas_add" Text="Save" OnClientClick="javascript: return SaveCopyPopup();" OnClick="cmdLocClose_Click" /></div>
                    <div style="float:right;"><asp:Button ID="cmdLocClose" runat="server" CssClass="wf_mas_add" Text="Cancel" OnClientClick="javascript: return CancelCopyPopup();" OnClick="cmdLocClose_Click" /></div>
                </div>
                </ContentTemplate>
                </asp:UpdatePanel> 
            </div>
        </div>
    </div>
    </div>
    </div>
    <%--Copy to Loaction Popup End--%>

    <%--Amble Mail Popup Start--%>
    <div class="dvpopup1" id="divAmbleMail1"></div>
    <div class="dvpopup2" style="top:-100px;" id="divAmbleMail2">
    <div class="lightbox_1">
    <div class="lightbox_2">        
        <div class="lightbox_3">
            <div style="width:100%; float:left; padding-top:10px;">
                <asp:UpdatePanel ID="UpdatePanel5" runat="server" UpdateMode="always">
                <ContentTemplate>
                <div style="float:left; font-weight:bold; border-bottom:1px solid #666; font-size:16px; width:100%;">
                    Set Email ID & Message
                </div>
                <div class="normal_feature">
                    <div style=" width:100%; float:left; padding-top:20px;">
                        <div style="float:left; width:100%; margin-bottom:5px;">
                            <span class="wf_mas_cap">Select User</span>
                            <asp:DropDownList ID="ddUser" CssClass="wf_det_field3_big" runat="server"></asp:DropDownList>
                            <asp:Button CssClass="Btn" ID="cmdAdd" runat="server" Text="Add" OnClientClick="javascript: AddMail()" />
                        </div>
                        <div style="float:left; width:100%; margin-bottom:5px;">
                            <span class="wf_mas_cap">To</span>
                            <asp:TextBox ID="txtToMail" runat="server" CssClass="wf_mas_field4"></asp:TextBox>
                            <span style="padding-left:5px; float:left; font-weight:bold; font-size:24px; color:Red;">*</span>
                        </div>
                        <div style="float:left; width:100%; margin-bottom:5px;">
                            <span class="wf_mas_cap">Subject</span>
                            <asp:TextBox ID="txtAmbleSub" runat="server" CssClass="wf_mas_field4"></asp:TextBox>
                            <span style="padding-left:5px; float:left; font-weight:bold; font-size:24px; color:Red;">*</span>
                        </div>
                        <div style="float:left; width:100%; margin-bottom:5px;">
                            <span class="wf_mas_cap">Message</span>
                            <asp:TextBox ID="txtAmbleMsg" runat="server" CssClass="wf_mas_field_m2" TextMode="MultiLine"></asp:TextBox>
                            <span style="padding-left:5px; float:left; font-weight:bold; font-size:24px; color:Red;">*</span>
                        </div>
                        <div style="float:left; width:100%;">
                            <span class="wf_mas_cap">Attachment</span>
                            <asp:DropDownList ID="ddAttachMail" CssClass="wf_det_field2" runat="server">
                                <asp:ListItem Value="Yes" Text="Yes"></asp:ListItem>
                                <asp:ListItem Value="No" Text="No" Selected="True"></asp:ListItem>
                            </asp:DropDownList>
                            <span style="padding-left:5px; float:left; font-weight:bold; font-size:24px; color:Red;">*</span>
                        </div>
                        <div style="float:left; width:100%;">
                            <span class="wf_mas_cap">URL</span>
                            <asp:DropDownList ID="ddAmbleURL" CssClass="wf_det_field2" runat="server">
                                <asp:ListItem Value="Yes" Text="Yes" Selected="True"></asp:ListItem>
                                <asp:ListItem Value="No" Text="No"></asp:ListItem>
                            </asp:DropDownList>
                            <span style="padding-left:5px; float:left; font-weight:bold; font-size:24px; color:Red;">*</span>
                        </div>
                    </div>
                </div>
                <div class="normal_feature" style="margin-bottom:5px;">
                    <div style="color:#FF0000; text-align:center; font-weight:bold; font-size:16px;  border-bottom:1px solid #666; padding:10px;">
                    </div>
                </div>
                <div class="normal_feature" style="text-align:right;">
                    <div style="float:left;"><asp:Button ID="cmdAmbMailSave" runat="server" CssClass="wf_mas_add" Text="Save" OnClientClick="javascript: return hideAmbleMail(this,',');" OnClick="cmdAmbMailSave_Click" /></div>
                    <div style="float:right;"><asp:Button ID="cmdAmbMailCancel" runat="server" CssClass="wf_mas_add" Text="Cancel" OnClientClick="javascript: return hideLoadingAmbleMail();" OnClick="cmdAmbMailCancel_Click" /></div>
                </div>
                </ContentTemplate>
                </asp:UpdatePanel> 
            </div>
        </div>
    </div>
    </div>
    </div>
    <%--Amble Mail Popup End--%>
    
    <%--Conditional Mail Popup Start--%>
    <div class="dvpopup1" id="divCondMail1"></div>
    <div class="dvpopup2" style="top:-100px;" id="divCondMail2">
    <div class="lightbox_1">
    <div class="lightbox_2">    
        <div class="lightbox_3">
            <div style="width:100%; float:left; padding-top:10px;">
                <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="always">
                <ContentTemplate>
                <div style="float:left; font-weight:bold; border-bottom:1px solid #666; font-size:16px; width:100%;">
                    Set Conditions, Email ID & Message
                </div>
                <div class="normal_feature">
                    <div style=" width:100%; float:left; padding-top:20px;">
                        <div style="float:left; width:100%; margin-bottom:5px;">
                            <span class="wf_mas_cap">Field No</span>
                            <asp:TextBox ID="txtFormFieldNo" runat="server" CssClass="wf_det_field_sm" Text="0"></asp:TextBox>
                            <asp:DropDownList ID="ddCondOp" CssClass="wf_det_field_sm" runat="server">
                                <asp:ListItem Value="<" Text="<"></asp:ListItem>
                                <asp:ListItem Value=">" Text=">"></asp:ListItem>
                                <asp:ListItem Value="=" Text="="></asp:ListItem>
                                <asp:ListItem Value="!=" Text="!="></asp:ListItem>
                            </asp:DropDownList>
                            <asp:TextBox ID="txtCondVal" runat="server" CssClass="wf_det_field_sm"></asp:TextBox>
                            <asp:Button CssClass="Btn" ID="cmdAddCond" runat="server" Text="Add Condition" OnClick="cmdAddCond_Click" />
                        </div>
                        <div style="float:left; width:100%; margin-bottom:5px;">
                            <span class="wf_mas_cap">Select User</span>
                            <asp:DropDownList ID="ddUser1" CssClass="wf_det_field3_big" runat="server"></asp:DropDownList>
                            <asp:Button CssClass="Btn" ID="cmdAdd1" runat="server" Text="Add" OnClientClick="javascript: AddMail1()" />
                        </div>
                        <div style="float:left; width:100%; margin-bottom:5px;">
                            <span class="wf_mas_cap">To</span>
                            <asp:TextBox ID="txtToMail1" runat="server" CssClass="wf_mas_field4"></asp:TextBox>
                        </div>
                        <div style="float:left; width:100%; margin-bottom:5px;">
                            <span class="wf_mas_cap">Subject</span>
                            <asp:TextBox ID="txtCondSub" runat="server" CssClass="wf_mas_field4"></asp:TextBox>
                        </div>
                        <div style="float:left; width:100%; margin-bottom:5px;">
                            <span class="wf_mas_cap">Message</span>
                            <asp:TextBox ID="txtCondMsg" runat="server" CssClass="wf_mas_field_m2" TextMode="MultiLine"></asp:TextBox>
                        </div>
                        <div style="float:left; width:100%;">
                            <span class="wf_mas_cap">Attachment</span>
                            <asp:DropDownList ID="ddAttachCondMail" CssClass="wf_det_field2" runat="server">
                                <asp:ListItem Value="Yes" Text="Yes"></asp:ListItem>
                                <asp:ListItem Value="No" Text="No" Selected="True"></asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div style="float:left; width:100%;">
                            <span class="wf_mas_cap">URL</span>
                            <asp:DropDownList ID="ddCondURL" CssClass="wf_det_field2" runat="server">
                                <asp:ListItem Value="Yes" Text="Yes" Selected="True"></asp:ListItem>
                                <asp:ListItem Value="No" Text="No"></asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div style="float:left; width:100%;">
                        <div style="overflow:auto; max-height:160px;">
                            <asp:GridView style="border:1px solid #000;" Width="100%" onrowdeleting="gvCond_RowDeleting" BackColor="White" CellPadding="4" ForeColor="Black" ID="gvCond" runat="server">
                            <RowStyle BackColor="#F7F7DE" HorizontalAlign="Center"/>
                            <FooterStyle BackColor="#CCCC99"/>
                            <PagerStyle BackColor="#F7F7DE" ForeColor="Black"/>
                            <SelectedRowStyle BackColor="#CE5D5A" Font-Bold="True" ForeColor="White" HorizontalAlign="Center"/>
                            <HeaderStyle BackColor="#cccccc" Font-Bold="True" HorizontalAlign="Center"/>
                            <AlternatingRowStyle BackColor="White"/>
                            <Columns>
                            <asp:TemplateField Visible="False">
                                <ItemTemplate>
                                    <asp:Label ID="lbAutoID1" runat="server" Text='<%#Eval("wf_id")%>' ></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField Visible="False">
                                <ItemTemplate>
                                    <asp:Label ID="lbAutoID2" runat="server" Text='<%#Eval("step_no")%>' ></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField Visible="False">
                                <ItemTemplate>
                                    <asp:Label ID="lbAutoID3" runat="server" Text='<%#Eval("task_id")%>' ></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField Visible="False">
                                <ItemTemplate>
                                    <asp:Label ID="lbAutoID4" runat="server" Text='<%#Eval("form_field_no")%>' ></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField Visible="False">
                                <ItemTemplate>
                                    <asp:Label ID="lbAutoID5" runat="server" Text='<%#Eval("cond_op")%>' ></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField Visible="False">
                                <ItemTemplate>
                                    <asp:Label ID="lbAutoID6" runat="server" Text='<%#Eval("cond_val")%>' ></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField Visible="False">
                                <ItemTemplate>
                                    <asp:Label ID="lbAutoID7" runat="server" Text='<%#Eval("amble_mails")%>' ></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField Visible="False">
                                <ItemTemplate>
                                    <asp:Label ID="lbAutoID8" runat="server" Text='<%#Eval("amble_msg")%>' ></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Department" HeaderStyle-Width="200" ItemStyle-Width="200" Visible="false" >  
                                <ItemTemplate><%#Eval("wf_id")%></ItemTemplate>
                                <HeaderStyle Width="200px" HorizontalAlign="Left" CssClass="GVRec"></HeaderStyle>
                                <ItemStyle Width="200px" HorizontalAlign="Left" CssClass="GVRec"></ItemStyle>
                            </asp:TemplateField>
                            <asp:CommandField HeaderText="Action" ShowDeleteButton="false" ShowEditButton="false" />
                            </Columns>
                            </asp:GridView>
                            </div>
                        </div>
                    </div>
                </div>
                    <div class="normal_feature" style="margin-bottom:5px;">
                        <div style="color:#FF0000; text-align:center; font-weight:bold; font-size:16px; border-bottom:1px solid #666; padding:10px;">
                        </div>
                    </div>
                    <div class="normal_feature" style="text-align:right;">
                        <div style="float:left;"><asp:Button ID="cmdCondMailSave" runat="server" CssClass="wf_mas_add" Text="Save" OnClientClick="javascript: return hideLoadingCondMail();" OnClick="cmdCondMailSave_Click" /></div>
                        <div style="float:right;"><asp:Button ID="cmdCondMailCancel" runat="server" CssClass="wf_mas_add" Text="Cancel" OnClientClick="javascript: return hideLoadingCondMail();" OnClick="cmdCondMailCancel_Click" /></div>
                    </div>
                </div>
                </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
    </div>
    </div>
    </div>
    <%--Conditional Mail Popup End--%>

    <%--Sign & Date Fields Popup Start--%>
    <div class="dvpopup1" id="divSignDate1"></div>
    <div class="dvpopup2" style="top:-100px;" id="divSignDate2">
    <div class="lightbox_1">
    <div class="lightbox_2">    
        <div class="lightbox_3">
            <div style="width:100%; float:left; padding-top:10px;">
                <asp:UpdatePanel ID="UpdatePanel32" runat="server" UpdateMode="always">
                <ContentTemplate>
                <div style="float:left; font-weight:bold; border-bottom:1px solid #666; font-size:16px; width:100%;">
                    Set Signature & Date Fields
                </div>
                <div class="normal_feature">
                    <div style="width:100%; float:left; padding-top:20px;">
                        <div style="float:left; width:20%; margin-bottom:5px;">
                            <span style="font-weight:bold;">Sl. No.</span>
                        </div>
                        <div style="float:left; width:38%; margin-bottom:5px;">
                            <span style="font-weight:bold;">Signature Fields No.</span>
                        </div>
                        <div style="float:left; width:38%; margin-bottom:5px;">
                            <span style="font-weight:bold;">Date Fields No.</span>
                        </div>
                    </div>
                    <div style="width:100%; float:left; padding-top:5px;">
                        <div style="float:left; width:20%; margin-bottom:5px;">
                            <span style="font-weight:bold;">#1</span>
                        </div>
                        <div style="float:left; width:38%; margin-bottom:5px;">
                            <asp:TextBox ID="txtSign1" runat="server" CssClass="wf_det_field_sm" Text="0"></asp:TextBox>
                        </div>
                        <div style="float:left; width:38%; margin-bottom:5px;">
                            <asp:TextBox ID="txtDate1" runat="server" CssClass="wf_det_field_sm" Text="0"></asp:TextBox>
                        </div>
                    </div>
                    <div style="width:100%; float:left;">
                        <div style="float:left; width:20%; margin-bottom:5px;">
                            <span style="font-weight:bold;">#2</span>
                        </div>
                        <div style="float:left; width:38%; margin-bottom:5px;">
                            <asp:TextBox ID="txtSign2" runat="server" CssClass="wf_det_field_sm" Text="0"></asp:TextBox>
                        </div>
                        <div style="float:left; width:38%; margin-bottom:5px;">
                            <asp:TextBox ID="txtDate2" runat="server" CssClass="wf_det_field_sm" Text="0"></asp:TextBox>
                        </div>
                    </div>
                    <div style="width:100%; float:left;">
                        <div style="float:left; width:20%; margin-bottom:5px;">
                            <span style="font-weight:bold;">#3</span>
                        </div>
                        <div style="float:left; width:38%; margin-bottom:5px;">
                            <asp:TextBox ID="txtSign3" runat="server" CssClass="wf_det_field_sm" Text="0"></asp:TextBox>
                        </div>
                        <div style="float:left; width:38%; margin-bottom:5px;">
                            <asp:TextBox ID="txtDate3" runat="server" CssClass="wf_det_field_sm" Text="0"></asp:TextBox>
                        </div>
                    </div>
                    <div style="width:100%; float:left;">
                        <div style="float:left; width:20%; margin-bottom:5px;">
                            <span style="font-weight:bold;">#4</span>
                        </div>
                        <div style="float:left; width:38%; margin-bottom:5px;">
                            <asp:TextBox ID="txtSign4" runat="server" CssClass="wf_det_field_sm" Text="0"></asp:TextBox>
                        </div>
                        <div style="float:left; width:38%; margin-bottom:5px;">
                            <asp:TextBox ID="txtDate4" runat="server" CssClass="wf_det_field_sm" Text="0"></asp:TextBox>
                        </div>
                    </div>
                    <div style="width:100%; float:left;">
                        <div style="float:left; width:20%; margin-bottom:5px;">
                            <span style="font-weight:bold;">#5</span>
                        </div>
                        <div style="float:left; width:38%; margin-bottom:5px;">
                            <asp:TextBox ID="txtSign5" runat="server" CssClass="wf_det_field_sm" Text="0"></asp:TextBox>
                        </div>
                        <div style="float:left; width:38%; margin-bottom:5px;">
                            <asp:TextBox ID="txtDate5" runat="server" CssClass="wf_det_field_sm" Text="0"></asp:TextBox>
                        </div>
                    </div>
                    <div style="width:100%; float:left;">
                        <div style="float:left; width:20%; margin-bottom:5px;">
                            <span style="font-weight:bold;">#6</span>
                        </div>
                        <div style="float:left; width:38%; margin-bottom:5px;">
                            <asp:TextBox ID="txtSign6" runat="server" CssClass="wf_det_field_sm" Text="0"></asp:TextBox>
                        </div>
                        <div style="float:left; width:38%; margin-bottom:5px;">
                            <asp:TextBox ID="txtDate6" runat="server" CssClass="wf_det_field_sm" Text="0"></asp:TextBox>
                        </div>
                    </div>
                    <div style="width:100%; float:left;">
                        <div style="float:left; width:20%; margin-bottom:5px;">
                            <span style="font-weight:bold;">#7</span>
                        </div>
                        <div style="float:left; width:38%; margin-bottom:5px;">
                            <asp:TextBox ID="txtSign7" runat="server" CssClass="wf_det_field_sm" Text="0"></asp:TextBox>
                        </div>
                        <div style="float:left; width:38%; margin-bottom:5px;">
                            <asp:TextBox ID="txtDate7" runat="server" CssClass="wf_det_field_sm" Text="0"></asp:TextBox>
                        </div>
                    </div>
                    <div style="width:100%; float:left;">
                        <div style="float:left; width:20%; margin-bottom:5px;">
                            <span style="font-weight:bold;">#8</span>
                        </div>
                        <div style="float:left; width:38%; margin-bottom:5px;">
                            <asp:TextBox ID="txtSign8" runat="server" CssClass="wf_det_field_sm" Text="0"></asp:TextBox>
                        </div>
                        <div style="float:left; width:38%; margin-bottom:5px;">
                            <asp:TextBox ID="txtDate8" runat="server" CssClass="wf_det_field_sm" Text="0"></asp:TextBox>
                        </div>
                    </div>
                    <div style="width:100%; float:left;">
                        <div style="float:left; width:20%; margin-bottom:5px;">
                            <span style="font-weight:bold;">#9</span>
                        </div>
                        <div style="float:left; width:38%; margin-bottom:5px;">
                            <asp:TextBox ID="txtSign9" runat="server" CssClass="wf_det_field_sm" Text="0"></asp:TextBox>
                        </div>
                        <div style="float:left; width:38%; margin-bottom:5px;">
                            <asp:TextBox ID="txtDate9" runat="server" CssClass="wf_det_field_sm" Text="0"></asp:TextBox>
                        </div>
                    </div>
                    <div style="width:100%; float:left;">
                        <div style="float:left; width:20%; margin-bottom:5px;">
                            <span style="font-weight:bold;">#10</span>
                        </div>
                        <div style="float:left; width:38%; margin-bottom:5px;">
                            <asp:TextBox ID="txtSign10" runat="server" CssClass="wf_det_field_sm" Text="0"></asp:TextBox>
                        </div>
                        <div style="float:left; width:38%; margin-bottom:5px;">
                            <asp:TextBox ID="txtDate10" runat="server" CssClass="wf_det_field_sm" Text="0"></asp:TextBox>
                        </div>
                    </div>
                </div>
                    <div class="normal_feature" style="margin-bottom:5px;">
                        <div style="color:#FF0000; text-align:center; font-weight:bold; font-size:16px; border-bottom:1px solid #666; padding:10px;">
                        </div>
                    </div>
                    <div class="normal_feature" style="text-align:right;">
                        <div style="float:left;"><asp:Button ID="cmdSignDateSave" runat="server" CssClass="wf_mas_add" Text="Save" OnClientClick="javascript: return hideLoadingSignDate();" OnClick="cmdSignDateSave_Click" /></div>
                        <div style="float:right;"><asp:Button ID="cmdSignDateCancel" runat="server" CssClass="wf_mas_add" Text="Cancel" OnClientClick="javascript: return hideLoadingSignDate();" OnClick="cmdSignDateCancel_Click" /></div>
                    </div>
                </div>
                </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
    </div>
    </div>
    </div>
    <%--Sign & Date Fields Popup End--%>

    <%--Appended Doc Popup Start--%>
    <div class="dvpopup1" id="divAppendDoc1"></div>
    <div class="dvpopup2" style="top:-100px;" id="divAppendDoc2">
    <div class="lightbox_1">
    <div class="lightbox_2">    
        <div class="lightbox_3">
            <div style="width:100%; float:left; padding-top:10px;">
                <asp:UpdatePanel ID="UpdatePanel9" runat="server" UpdateMode="always">
                <ContentTemplate>
                <div style="float:left; font-weight:bold; border-bottom:1px solid #666; font-size:16px; width:100%;">
                    Select the Appended Document
                </div>
                <div class="normal_feature">
                    <div style=" width:100%; float:left; padding-top:20px;">
                        <div style="float:left; width:100%;">
                            <span class="wf_mas_cap">Cabinet</span>
                            <asp:DropDownList ID="ddCabinet3" CssClass="wf_det_field2_big" runat="server" AutoPostBack="true" onselectedindexchanged="ddCabinet3_SelectedIndexChanged"></asp:DropDownList>
                            <span style="padding-left:5px; float:left; font-weight:bold; font-size:24px; color:Red;">*</span>
                        </div>
                        <div style="float:left; width:100%;">
                            <span class="wf_mas_cap">Drawer</span>
                            <asp:DropDownList ID="ddDrawer3" CssClass="wf_det_field2_big" runat="server" AutoPostBack="true" onselectedindexchanged="ddDrawer3_SelectedIndexChanged"></asp:DropDownList>
                            <span style="padding-left:5px; float:left; font-weight:bold; font-size:24px; color:Red;">*</span>
                        </div>
                        <div style="float:left; width:100%;">
                            <span class="wf_mas_cap">Folder</span>
                            <asp:DropDownList ID="ddFolder3" CssClass="wf_det_field2_big" runat="server" AutoPostBack="true" onselectedindexchanged="ddFolder3_SelectedIndexChanged"></asp:DropDownList>
                            <span style="padding-left:5px; float:left; font-weight:bold; font-size:24px; color:Red;">*</span>
                        </div>
                        <div style="float:left; width:100%;">
                            <span class="wf_mas_cap">Document</span>
                            <asp:DropDownList ID="ddDocument3" CssClass="wf_det_field2_big" runat="server"></asp:DropDownList>
                            <span style="padding-left:5px; float:left; font-weight:bold; font-size:24px; color:Red;">*</span>
                        </div>
                    </div>
                </div>
                <div class="normal_feature">
                    <div style="color:#FF0000; text-align:center; font-weight:bold; font-size:16px;  border-bottom:1px solid #666; padding:10px;">
                    </div>
                </div>
                <div class="normal_feature" style="text-align:right;">
                    <div style="float:left;"><asp:Button ID="cmdAppendDocSave" runat="server" CssClass="wf_mas_add" Text="Save" OnClientClick="javascript: return hideLoadingAppendDoc();" OnClick="cmdAppendDocSave_Click" /></div>
                    <div style="float:right;"><asp:Button ID="cmdAppendDocCancel" runat="server" CssClass="wf_mas_add" Text="Cancel" OnClientClick="javascript: return CancelAppendDoc();" OnClick="cmdAppendDocCancel_Click" /></div>
                </div>
                </ContentTemplate>
                </asp:UpdatePanel> 
            </div>
        </div>
    </div>
    </div>
    </div>
    <%--Appended Doc Popup End--%>
</asp:Content>