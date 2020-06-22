
<%@ Page  Language="C#" AutoEventWireup="true" CodeFile="index.aspx.cs" Inherits="v2_index" MasterPageFile="layout/MasterPage.master" %>

<%@ Register Src="./layout/ContentHeader.ascx" TagPrefix="uc1" TagName="ContentHeader" %>
<%@Import Namespace="System.Data.SqlClient" %>
<%@Import Namespace ="System.Data" %>
<asp:Content ContentPlaceHolderId="Header" runat="server">
    <uc1:ContentHeader runat="server" ID="ContentHeader" GTitle="Gpos" />
</asp:Content>
<asp:Content ContentPlaceHolderId="Content" runat="server">
  <!-- Main content -->
     <section class="content">
      <div class="container-fluid">
    
        <!-- Main row -->
        <div class="row">
            <!-- left column -->
          <div class="col-md-6">
            <!-- general form elements -->
            <div class="card card-primary">
              <div class="card-header">
                <h3 class="card-title">Quick Example</h3>
              </div>
              <!-- /.card-header -->
              <!-- form start -->
              <form role="form">
                <div class="card-body">
                  <div class="form-group">
                    <label for="exampleInputEmail1">Email address</label>
                    <input type="email" class="form-control" id="exampleInputEmail1" placeholder="Enter email">
                  </div>
                  <div class="form-group">
                    <label for="exampleInputPassword1">Password</label>
                    <%--<input type="password" class="form-control" id="exampleInputPassword1" placeholder="Password">--%>
                      <asp:TextBox ID="TextBox1"    class="form-control" runat="server" placeholder="Password" OnTextChanged="TextBox1_TextChanged"></asp:TextBox>
                  </div>
                  <div class="form-group">
                    <label for="exampleInputFile">File input</label>
                    <div class="input-group">
                      <div class="custom-file">
                        <input type="file" class="custom-file-input" id="exampleInputFile">
                        <label class="custom-file-label" for="exampleInputFile">Choose file</label>
                      </div>
                      <div class="input-group-append">
                          
                        <span class="input-group-text" id="">Upload</span>
                      </div>
                    </div>
                  </div>
                  <div class="form-check">
                    <input type="checkbox" class="form-check-input" id="exampleCheck1">
                      <asp:TextBox ID="TextBox2" runat="server"></asp:TextBox>
                      <input type="text" value="" />
                    <label class="form-check-label" for="exampleCheck1">Check me out</label>
                  </div>
                </div>
                <!-- /.card-body -->

                <div class="card-footer">
                <asp:Label ID="Label1" runat="server" Text="Label"></asp:Label>
                  <button type="submit" class="btn btn-primary">Submit</button>
                    <asp:Button ID="Button3" runat="server" Text="Button" />
                    <asp:Button ID="Button2" runat="server" Text="Button"  class="btn btn-primary"  OnClick="Button2_Click" />
                    <asp:Button ID="Button1" runat="server"  class="btn btn-primary" 
                        Text="Button" OnClick="Button1_Click" />
                </div>
              </form>
            </div>
            <!-- /.card -->

      

          </div>
          <!--/.col (left) -->
      
        </div>
        <!-- /.row (main row) -->
      </div><!-- /.container-fluid -->
    </section>
    <script>
        $(function () {
            //Initialize Select2 Elements
            $('.select2').select2()
        })
    </script>
  

 <!-- /.content -->
</asp:Content>