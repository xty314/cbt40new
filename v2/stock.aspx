<%@ Page Language="C#" AutoEventWireup="true" CodeFile="stock.aspx.cs" Inherits="v2_stock"   MasterPageFile="~/MasterPage/MasterPage.master" %>

<%--注意加入MasterPageFile--%>

<%@ Register Src="~/MasterPage/layout/ContentHeader.ascx" TagPrefix="uc1" TagName="ContentHeader" %>
<%--<%@Import Namespace="System.Data.SqlClient" %>
<%@Import Namespace ="System.Data" %>--%>
<asp:Content ContentPlaceHolderId="AdditionalCSS" runat="server">


</asp:Content>

<asp:Content ContentPlaceHolderId="Header" runat="server">

    <%--<uc1:ContentHeader runat="server" ID="ContentHeader" GTitle="Gpos" />--%>

</asp:Content>
<asp:Content ContentPlaceHolderId="Content" runat="server">

  <!-- Main content -->
    <section class="content">
      <div class="container-fluid">
  
        <!-- /.row -->
        <div class="row">
          <div class="col-12">
            <div class="card">
  
              <!-- /.card-header -->
              <div class="card-body table-responsive p-0">
            <%PrintPage(); %>
              </div>
              <!-- /.card-body -->
            </div>
            <!-- /.card -->
          </div>
        </div>
        <!-- /.row -->
      
        <!-- /.row -->
      </div><!-- /.container-fluid -->
    </section>

 <!-- /.content -->
</asp:Content>
<asp:Content ContentPlaceHolderId="AdditionalJS" runat="server">


</asp:Content>