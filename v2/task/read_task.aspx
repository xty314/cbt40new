<%@ Page Language="C#" AutoEventWireup="true" CodeFile="read_task.aspx.cs" Inherits="v2_read_task" MasterPageFile="~/MasterPage/MasterPage.master" %>


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
         <section class="content">
             <form runat="server">
      <div class="container-fluid">
        <div class="row">
          <div class="col-md-3">
            <a href="mailbox.html" class="btn btn-primary btn-block mb-3">Back to Inbox</a>

        <div class="card card-info">
              <div class="card-header">
                <h3 class="card-title">Horizontal Form</h3>
              </div>
              <!-- /.card-header -->
              <!-- form start -->
              
                <div class="card-body">
                  <div class="form-group row">
                    <label for="Content_TextBoxCustomer" class="col-sm-6 col-form-label">Customer</label>
                    <div class="col-sm-6">
                      
                        <asp:TextBox ID="TextBoxCustomer" class="form-control"  runat="server"></asp:TextBox>
                    </div>
                  </div>
            <div class="form-group row">
                    <label for="Content_TextBoxTVID" class="col-sm-6 col-form-label">TV ID</label>
                    <div class="col-sm-6">
                      
                        <asp:TextBox ID="TextBoxTVID" class="form-control"  runat="server"></asp:TextBox>
                    </div>
                  </div>
                       <div class="form-group row">
                    <label for="Content_TextBoxTaskType" class="col-sm-6 col-form-label">Task Type</label>
                    <div class="col-sm-6">
                      <asp:TextBox ID="TextBoxTaskType" class="form-control"  runat="server"></asp:TextBox>
                    </div>
                  </div>
                       <div class="form-group row">
                    <label for="Content_TextBoxTaskLevel" class="col-sm-6 col-form-label">Task Level</label>
                    <div class="col-sm-6">
                        <asp:TextBox ID="TextBoxTaskLevel" class="form-control"  runat="server"></asp:TextBox>
                    
                    </div>
                  </div>
                       <div class="form-group row">
                    <label for="Content_TextBoxFDR" class="col-sm-6 col-form-label">Finish Date Required</label>
                    <div class="col-sm-6">
                     
                         <asp:TextBox ID="TextBoxFDR" class="form-control"  runat="server"></asp:TextBox>

                    </div>
                  </div>
                       <div class="form-group row">
                    <label for="Content_TextBoxAssignTo" class="col-sm-6 col-form-label">Assign To</label>
                    <div class="col-sm-6">
                        <asp:TextBox ID="TextBoxAssignTo" class="form-control"  runat="server"></asp:TextBox>
                     
                    </div>
                  </div>
                                <div class="form-group row">
                    <label for="Content_TextBoxPriority" class="col-sm-6 col-form-label">Priority</label>
                    <div class="col-sm-6">
                      
                        <asp:TextBox ID="TextBoxPriority" class="form-control"  runat="server"></asp:TextBox>
                    </div>
                  </div> 
                  <div class="form-group row">
                    <div class=" col-sm-10">
                      <div class="form-check">
                        <input type="checkbox" class="form-check-input" id="exampleCheck2">
                        <label class="form-check-label" for="exampleCheck2">Remember me</label>
                      </div>
                    </div>
                  </div>
                </div>
                <!-- /.card-body -->
                <div class="card-footer">
                  <button type="submit" class="btn btn-info">Sign in</button>
                  <button type="submit" class="btn btn-default float-right">Cancel</button>
                </div>
                <!-- /.card-footer -->
         
            </div>
          </div>
          <!-- /.col -->
            <div class="col-md-9">
          <div class="card card-primary card-outline">
            <div class="card-header">
              <h3 class="card-title">Read Mail</h3>

              <div class="card-tools">
                <a href="#" class="btn btn-tool" data-toggle="tooltip" title="Previous"><i class="fas fa-chevron-left"></i></a>
                <a href="#" class="btn btn-tool" data-toggle="tooltip" title="Next"><i class="fas fa-chevron-right"></i></a>
              </div>
            </div>
            <!-- /.card-header -->
            <div class="card-body p-0">
              <div class="mailbox-read-info">
                <h5>Message Subject Is Placed Here</h5>
                <h6>From: support@adminlte.io
                  <span class="mailbox-read-time float-right">15 Feb. 2015 11:03 PM</span></h6>
              </div>
              <!-- /.mailbox-read-info -->
              <div class="mailbox-controls with-border text-center">
                <div class="btn-group">
                  <button type="button" class="btn btn-default btn-sm" data-toggle="tooltip" data-container="body" title="Delete">
                    <i class="far fa-trash-alt"></i></button>
                  <button type="button" class="btn btn-default btn-sm" data-toggle="tooltip" data-container="body" title="Reply">
                    <i class="fas fa-reply"></i></button>
                  <button type="button" class="btn btn-default btn-sm" data-toggle="tooltip" data-container="body" title="Forward">
                    <i class="fas fa-share"></i></button>
                </div>
                <!-- /.btn-group -->
                <button type="button" class="btn btn-default btn-sm" data-toggle="tooltip" title="Print">
                  <i class="fas fa-print"></i></button>
              </div>
              <!-- /.mailbox-controls -->
              <div class="mailbox-read-message" runat="server" id="MessageBoard">
         
              </div>
              <!-- /.mailbox-read-message -->
            </div>
            <!-- /.card-body -->
            <div class="card-footer bg-white">
              <ul class="mailbox-attachments d-flex align-items-stretch clearfix">
                <li>
                  <span class="mailbox-attachment-icon"><i class="far fa-file-pdf"></i></span>

                  <div class="mailbox-attachment-info">
                    <a href="#" class="mailbox-attachment-name"><i class="fas fa-paperclip"></i> Sep2014-report.pdf</a>
                        <span class="mailbox-attachment-size clearfix mt-1">
                          <span>1,245 KB</span>
                          <a href="#" class="btn btn-default btn-sm float-right"><i class="fas fa-cloud-download-alt"></i></a>
                        </span>
                  </div>
                </li>
                <li>
                  <span class="mailbox-attachment-icon"><i class="far fa-file-word"></i></span>

                  <div class="mailbox-attachment-info">
                    <a href="#" class="mailbox-attachment-name"><i class="fas fa-paperclip"></i> App Description.docx</a>
                        <span class="mailbox-attachment-size clearfix mt-1">
                          <span>1,245 KB</span>
                          <a href="#" class="btn btn-default btn-sm float-right"><i class="fas fa-cloud-download-alt"></i></a>
                        </span>
                  </div>
                </li>
                <li>
                  <span class="mailbox-attachment-icon has-img"><img src="../../dist/img/photo1.png" alt="Attachment"></span>

                  <div class="mailbox-attachment-info">
                    <a href="#" class="mailbox-attachment-name"><i class="fas fa-camera"></i> photo1.png</a>
                        <span class="mailbox-attachment-size clearfix mt-1">
                          <span>2.67 MB</span>
                          <a href="#" class="btn btn-default btn-sm float-right"><i class="fas fa-cloud-download-alt"></i></a>
                        </span>
                  </div>
                </li>
                <li>
                  <span class="mailbox-attachment-icon has-img"><img src="../../dist/img/photo2.png" alt="Attachment"></span>

                  <div class="mailbox-attachment-info">
                    <a href="#" class="mailbox-attachment-name"><i class="fas fa-camera"></i> photo2.png</a>
                        <span class="mailbox-attachment-size clearfix mt-1">
                          <span>1.9 MB</span>
                          <a href="#" class="btn btn-default btn-sm float-right"><i class="fas fa-cloud-download-alt"></i></a>
                        </span>
                  </div>
                </li>
              </ul>
            </div>
            <!-- /.card-footer -->
            <div class="card-footer">
              <div class="float-right">
                <button type="button" class="btn btn-default"><i class="fas fa-reply"></i> Reply</button>
                <button type="button" class="btn btn-default"><i class="fas fa-share"></i> Forward</button>
              </div>
              <button type="button" class="btn btn-default"><i class="far fa-trash-alt"></i> Delete</button>
              <button type="button" class="btn btn-default"><i class="fas fa-print"></i> Print</button>
            </div>
            <!-- /.card-footer -->
          </div>
          <!-- /.card -->
        </div>
      <!-- /.row -->
      </div><!-- /.container-fluid -->
          </form>
    </section>
 <!-- /.content -->
</asp:Content>
<asp:Content ContentPlaceHolderId="AdditionalJS" runat="server">


</asp:Content>
