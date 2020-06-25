<%@ Page Language="C#" AutoEventWireup="true" CodeFile="dev_task.aspx.cs" Inherits="v2_dev_task" MasterPageFile="~/MasterPage/MasterPage.master"%>

<%@ Register Src="./layout/ContentHeader.ascx" TagPrefix="uc1" TagName="ContentHeader" %>
<%@Import Namespace="System.Data.SqlClient" %>
<%@Import Namespace ="System.Data" %>
<asp:Content ContentPlaceHolderId="AdditionalCSS" runat="server">


</asp:Content>

<asp:Content ContentPlaceHolderId="Header" runat="server">

    <uc1:ContentHeader runat="server" ID="ContentHeader" GTitle="Gpos Task" />

</asp:Content>
<asp:Content ContentPlaceHolderId="Content" runat="server">
   <!-- Main content -->
    <section class="content">
      <div class="row">
        <div class="col-md-3">
          <a href="compose.html" class="btn btn-primary btn-block mb-3">New Task</a>

          <div class="card">
            <div class="card-header">
              <h3 class="card-title">Folders</h3>

              <div class="card-tools">
                <button type="button" class="btn btn-tool" data-card-widget="collapse"><i class="fas fa-minus"></i>
                </button>
              </div>
            </div>
            <div class="card-body p-0">
              <ul class="nav nav-pills flex-column">
                     <li class="nav-item active">
                  <a href="#" class="nav-link">
                    <i class="far fa-file-alt"></i>  Active
                  </a>
                </li>
                <li class="nav-item ">
                  <a href="?repeat=1" class="nav-link">
                    <i class="fas fa-inbox"></i>  Repeat Tasks
                    <span class="badge bg-primary float-right">12</span>
                  </a>
                </li>
                <li class="nav-item">
                  <a href="?owner=me" class="nav-link">
                    <i class="far fa-envelope"></i>  My Task 
                  </a>
                </li>
             
                <li class="nav-item">
                  <a href="?owner=<%=userId %>&status=finished&finished=1" class="nav-link">
                    <i class="fas fa-filter"></i>  Finished 
                    <span class="badge bg-warning float-right">65</span>
                  </a>
                </li>
                <li class="nav-item">
                  <a href="?status=deffered" class="nav-link">
                    <i class="far fa-trash-alt"></i>  Deffered
                  </a>
                </li>
                    <li class="nav-item">
                  <a href="?status=testing" class="nav-link">
                    <i class="far fa-trash-alt"></i>  Testing 
                  </a>
                </li>
                      <li class="nav-item">
                  <a href="?owner=<%=userId %>&status=completed" class="nav-link">
                    <i class="far fa-trash-alt"></i>  Completed
                  </a>
                </li>
                      <li class="nav-item">
                  <a href="?status=all" class="nav-link">
                    <i class="far fa-trash-alt"></i>  All
                  </a>
                </li>

              </ul>
            </div>
            <!-- /.card-body -->
          </div>

        </div>
        <!-- /.col -->
        <div class="col-md-9">
          <div class="card card-primary card-outline">
            <div class="card-header">
              <h3 class="card-title">Task List</h3>

              <div class="card-tools">
                <div class="input-group input-group-sm">
                  <input type="text" class="form-control" placeholder="Search Mail">
                  <div class="input-group-append">
                    <div class="btn btn-primary">
                      <i class="fas fa-search"></i>
                    </div>
                  </div>
                </div>
              </div>
              <!-- /.card-tools -->
            </div>
            <!-- /.card-header -->
            <div class="card-body p-0">
              <div class="mailbox-controls">
                <!-- Check all button -->
                <button type="button" class="btn btn-default btn-sm checkbox-toggle"><i class="far fa-square"></i>
                </button>
                <div class="btn-group">
                  <button type="button" class="btn btn-default btn-sm"><i class="far fa-trash-alt"></i></button>
                  <button type="button" class="btn btn-default btn-sm"><i class="fas fa-reply"></i></button>
                  <button type="button" class="btn btn-default btn-sm"><i class="fas fa-share"></i></button>
                </div>
                <!-- /.btn-group -->
                <button type="button" class="btn btn-default btn-sm"><i class="fas fa-sync-alt"></i></button>
                <div class="float-right">
                  1-50/200
                  <div class="btn-group">
                    <button type="button" class="btn btn-default btn-sm"><i class="fas fa-chevron-left"></i></button>
                    <button type="button" class="btn btn-default btn-sm"><i class="fas fa-chevron-right"></i></button>
                  </div>
                  <!-- /.btn-group -->
                </div>
                <!-- /.float-right -->
              </div>
              <div class="table-responsive mailbox-messages">
                <table class="table table-hover table-striped">
                  <tbody>

                <%foreach(DataRow dr in taskTable.Rows)
                  {
                %>

                          <tr>
                                    <td>
                                      <div class="icheck-primary">
                                        <input type="checkbox" value="" id="check1">
                                        <label for="check1"></label>
                                      </div>
                                    </td>
                                    <td class="mailbox-star"><a href="#"><i class="fas fa-star text-warning"></i></a></td>
                                    <td class="mailbox-name"><a href="read-mail.html">Alexander Pierce</a></td>
                                    <td class="mailbox-subject"><b>AdminLTE 3.0 Issue</b> <%=dr["subject"] %>
                                    </td>
                                    <td class="mailbox-attachment"></td>
                                    <td class="mailbox-date">5 mins ago</td>
                                  </tr>

                <%}%>
        

               
                
                  </tbody>
                </table>
                <!-- /.table -->
              </div>
              <!-- /.mail-box-messages -->
            </div>
            <!-- /.card-body -->
            <div class="card-footer p-0">
              <div class="mailbox-controls">
                <!-- Check all button -->
                <button type="button" class="btn btn-default btn-sm checkbox-toggle"><i class="far fa-square"></i>
                </button>
                <div class="btn-group">
                  <button type="button" class="btn btn-default btn-sm"><i class="far fa-trash-alt"></i></button>
                  <button type="button" class="btn btn-default btn-sm"><i class="fas fa-reply"></i></button>
                  <button type="button" class="btn btn-default btn-sm"><i class="fas fa-share"></i></button>
                </div>
                <!-- /.btn-group -->
                <button type="button" class="btn btn-default btn-sm"><i class="fas fa-sync-alt"></i></button>
                <div class="float-right">
                  1-50/200
                  <div class="btn-group">
                    <button type="button" class="btn btn-default btn-sm"><i class="fas fa-chevron-left"></i></button>
                    <button type="button" class="btn btn-default btn-sm"><i class="fas fa-chevron-right"></i></button>
                  </div>
                  <!-- /.btn-group -->
                </div>
                <!-- /.float-right -->
              </div>
            </div>
          </div>
          <!-- /.card -->
        </div>
        <!-- /.col -->
      </div>
      <!-- /.row -->
    </section>
    <!-- /.content -->
</asp:Content>
<asp:Content ContentPlaceHolderId="AdditionalJS" runat="server">


</asp:Content>