<%@ Page Title="myDOCbase" Language="C#" MasterPageFile="adm_mast_entry.Master" AutoEventWireup="true" CodeBehind="ErrDisp.aspx.cs" Inherits="DMS.ErrDisp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" src="js/jquary_min.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="main_body">
        <div class="main">
            <div class="normal_common">
                <div class="wf_mas_Heading">
                    
                </div>
            </div>
            <div style="padding-bottom:0px;" class="normal_common">
                <div class="wf_mas_Heading2">
                    Warning !!
                </div>
            </div>
            <div class="normal_common">
                <div class="wf_mas_doc_holder" style="min-height:300px;">
                    <div class="wf_mas_doc_holder_padd">
                        <div class="normal_common" style="height:200px; margin:0 auto; text-align:center; padding-top:150px;">
                            <span style="text-align:center; font-size:20px; font-weight:bold; color:Red;">You can not proceed this Document as it is already travelling down in workflow.
                                <br /><br />
                                <asp:Button CssClass="TaskUpdtBtn1" ID="cmdBack" runat="server" Text="Go Back" onclick="cmdBack_Click" /> 
                            </span>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
