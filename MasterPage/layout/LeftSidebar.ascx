<%@ Control Language="C#" AutoEventWireup="true" CodeFile="LeftSidebar.ascx.cs" Inherits="mobile_layout_LeftSidebar" %>

<%@Import Namespace="System.Data" %>
<%@Import Namespace="System.Data.SqlClient" %>



<%-- <%@ Assembly Src="~/common/Myclass.cs" %> --%>
<aside class="main-sidebar sidebar-dark-primary elevation-4">
    <!-- Brand Logo -->
    <a href="index3.html" class="brand-link text-sm"">
       
      <img src="/asset/img/icon/company.png" alt="dist Logo" class="brand-image img-circle elevation-3"
           style="opacity: .8">
      <span class="brand-text font-weight-light">
          <%=Company.m_sCompanyName%>
      </span>
    </a>

    <!-- Sidebar -->
    <div class="sidebar">
      <!-- Sidebar user panel (optional) -->
      <div class="user-panel mt-3 pb-3 mb-3 d-flex">
        <div class="image">
           
          <img src="/asset/img/icon/user.png" class="img-circle elevation-2" alt="User Image">
        </div>
        <div class="info">
          <a href="#" class="d-block">
              <%=Session["name"] %>
              
          </a>
        </div>
      </div>

      <!-- Sidebar Menu -->
      <nav class="mt-2">
  
        <ul class="nav nav-pills nav-sidebar flex-column nav-compact nav-child-indent" data-widget="treeview" role="menu" data-accordion="false">
          <!-- Add icons to the links using the .nav-icon class
               with font-awesome or any other icon font library -->
  
        
              <% int i = 0;
                  foreach (DataRow dataRow in menuTable.Rows){
                      %>
                 <li class="nav-item has-treeview ">
            <a href="#" class="nav-link 
              <%--active--%>
                ">
              <i class="nav-icon fas <%=GetFontawesome(i) %>"></i>
              <p>
               <%=dataRow["name"]%>
                <i class="right fas fa-angle-left"></i>
              </p>
            </a>
             <ul class="nav nav-treeview">
                  <%   
                      DataTable menuSubTable = getSubTable(dataRow["id"].ToString());
                      foreach (DataRow subMenu in menuSubTable.Rows)
                      {
                         %>
                          <li class="nav-item">
                            <a href=<%= UseNewVersionMenu(subMenu["uri"].ToString())%> class="nav-link ">
                              <i class="far fa-circle nav-icon"></i>
                              <p><%=subMenu["name"]%></p>
                            </a>
                          </li>  
                 <%} %>
                    </ul>
        
          </li>
            <%  i = i + 1;}%>
       
<%--          <li class="nav-item">
            <a href="#" class="nav-link">
              <i class="nav-icon fas fa-th"></i>
              <p>
                Simple Link
                <span class="right badge badge-danger">
                </span>
              </p>
            </a>
          </li>--%>
        </ul>
      </nav>
      <!-- /.sidebar-menu -->
    </div>
    <!-- /.sidebar -->
  </aside>