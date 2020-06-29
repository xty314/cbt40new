<%@ Page Language="C#" AutoEventWireup="true" CodeFile="especial.aspx.cs" Inherits="v2_especial" MasterPageFile="~/MasterPage/MasterPage.master"%>




<%--注意加入MasterPageFile--%>

<%@ Register Src="~/MasterPage/layout/ContentHeader.ascx" TagPrefix="uc1" TagName="ContentHeader" %>
<%--<%@Import Namespace="System.Data.SqlClient" %>
<%@Import Namespace ="System.Data" %>--%>
<asp:Content ContentPlaceHolderId="AdditionalCSS" runat="server">


</asp:Content>

<asp:Content ContentPlaceHolderId="Header" runat="server">

    <uc1:ContentHeader runat="server" ID="ContentHeader" GTitle="Gpos" />

</asp:Content>
<asp:Content ContentPlaceHolderId="Content" runat="server">

  <!-- Main content -->
     <%PrintPage(); %>
 <!-- /.content -->
</asp:Content>
<asp:Content ContentPlaceHolderId="AdditionalJS" runat="server">


</asp:Content>