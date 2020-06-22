<%@ Page Language="C#" validateRequest="false" AutoEventWireup="true" CodeFile="new_task.aspx.cs" Inherits="v2_newtask" MasterPageFile="layout/MasterPage.master"%>

<%@ Register Src="layout/ContentHeader.ascx" TagPrefix="uc1" TagName="ContentHeader" %>
<%--<%@Import Namespace="System.Data.SqlClient" %>
<%@Import Namespace ="System.Data" %>--%>
<asp:Content ContentPlaceHolderId="AdditionalCSS" runat="server">
   
    <link href="plugins/summernote/summernote-bs4.min.css" rel="stylesheet" />
</asp:Content>

<asp:Content ContentPlaceHolderId="Header" runat="server">

    <uc1:ContentHeader runat="server" ID="ContentHeader" GTitle="New Task" />

</asp:Content>
<asp:Content ContentPlaceHolderId="AdditionalJS" runat="server">
  
 
  
    <script src="plugins/summernote/summernote-bs4.min.js"></script>
       <%--<script src="adminLTE/js/demo.js"></script>--%>
    <script>
  $(function () {
    //Add text editor
      $('#ctl00_Content_TaskNote').summernote()
  })
</script>
</asp:Content>
<asp:Content ContentPlaceHolderId="Content" runat="server">

  <!-- Main content -->
         <section class="content">
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
                <h3 class="card-title">Compose New Message</h3>
              </div>
              <!-- /.card-header -->
              <div class="card-body">
                <div class="form-group row">
                  
                <label for="Content_TextBoxURL" class="col-sm-2 col-form-label">Related URL</label>
                    <div class="col-sm-10"><asp:TextBox ID="TextBoxURL" runat="server" class="form-control"  ></asp:TextBox></div>
                    
                </div>
                <div class="form-group row">
                     <label for="Content_TextBoxSubject" class="col-sm-2 col-form-label">Subject</label>
                    <div class="col-sm-10"><asp:TextBox class="form-control" ID="TextBoxSubject"  runat="server"></asp:TextBox></div>   
                  
                </div>
                <div class="form-group">
                    <textarea id="TaskNote" ClientID="TaskNote" class="form-control" style="height: 300px" runat="server" >
                      <h1><u>Heading Of Message</u></h1>
                      <h4>Subheading</h4>
                      <p>But I must explain to you how all this mistaken idea of denouncing pleasure and praising pain
                        was born and I will give you a complete account of the system, and expound the actual teachings
                        of the great explorer of the truth, the master-builder of human happiness. No one rejects,
                        dislikes, or avoids pleasure itself, because it is pleasure, but because those who do not know
                        how to pursue pleasure rationally encounter consequences that are extremely painful. Nor again
                        is there anyone who loves or pursues or desires to obtain pain of itself, because it is pain,
                        but because occasionally circumstances occur in which toil and pain can procure him some great
                        pleasure. To take a trivial example, which of us ever undertakes laborious physical exercise,
                        except to obtain some advantage from it? But who has any right to find fault with a man who
                        chooses to enjoy a pleasure that has no annoying consequences, or one who avoids a pain that
                        produces no resultant pleasure? On the other hand, we denounce with righteous indignation and
                        dislike men who are so beguiled and demoralized by the charms of pleasure of the moment, so
                        blinded by desire, that they cannot foresee</p>
                      <ul>
                        <li>List item one</li>
                        <li>List item two</li>
                        <li>List item three</li>
                        <li>List item four</li>
                      </ul>
                      <p>Thank you,</p>
                      <p>John Doe</p>
                    </textarea>
                </div>
           
              <!-- /.card-body -->
              <div class="card-footer">
                <div class="float-right">
                  <%--<button type="button" class="btn btn-default"><i class="fas fa-pencil-alt"></i> Draft</button>--%>
                    <asp:TextBox ID="TextBox2" runat="server"></asp:TextBox>
                    <asp:Button ID="Button1" runat="server" Text="Save" class="btn btn-primary" OnClick="Button1_Click" />
               
                </div>
                <button type="reset" class="btn btn-default"><i class="fas fa-times"></i> Discard</button>
                 
              </div>
              <!-- /.card-footer -->
            </div>
            <!-- /.card -->
          </div>
          <!-- /.col -->
        </div>
        <!-- /.row -->
      </div><!-- /.container-fluid -->
    </section>
 <!-- /.content -->
</asp:Content>

