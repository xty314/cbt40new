<%@ Page Language="C#" AutoEventWireup="true" CodeFile="login.aspx.cs" Inherits="mobile_login" %>

<%@ Register Src="./layout/RequiredScript.ascx" TagPrefix="uc1" TagName="RequiredScript" %>
<%@ Register Src="./layout/Head.ascx" TagPrefix="uc1" TagName="Head" %>
<%--<%@ Assembly Src="~/common/Myclass.cs" %> --%>

<!DOCTYPE html>
<html>
<head>
 <uc1:Head runat="server" ID="Head" />
</head>
<body class="hold-transition login-page">
   

<div class="login-box">
  <div class="login-logo">
   <b>CBT</b>40
  </div>
  <!-- /.login-logo -->
  

  <div class="card">
    <div class="card-body login-card-body">
      <p class="login-box-msg">Sign in to start your session</p>
        <form runat="server"> 
        <div class="input-group mb-3">     
            <asp:TextBox ID="EmailTextBox" runat="server" type="email" class="form-control" placeholder="Email">
            </asp:TextBox>
          <div class="input-group-append">
            <div class="input-group-text">
              <span class="fas fa-envelope"></span>
            </div>
          </div>
        </div>
        <div class="input-group mb-3">
          <%--<input type="password" class="form-control" placeholder="Password">--%>
         <asp:TextBox ID="PasswordTextBox" runat="server"  type="password" class="form-control" placeholder="Password"></asp:TextBox>
            <div class="input-group-append">
            <div class="input-group-text">
              <span class="fas fa-lock"></span>
            </div>
          </div>
        </div>
        <div class="row">
          <div class="col-8">
            <div class="icheck-primary">
                <asp:CheckBox ID="CheckBox1" runat="server" />
              <%--<input type="checkbox" id="remember">--%>
              <label for=<%=CheckBox1.ClientID %>>
                Remember Me
              </label>
            </div>
          </div>
          <!-- /.col -->
          <div class="col-4">          
          <asp:Button  type="submit" class="btn btn-primary btn-block" ID="SubmitBtn" runat="server" Text="Sign In" OnClick="SubmitBtn_Click"  />
          </div>
          <!-- /.col -->
        </div>
        </form>
    </div>
    <!-- /.login-card-body -->
  </div>
</div>
    <%if (IsPostBack){%>
   
      <div class="alert alert-danger alert-dismissible">
                  <button type="button" class="close" data-dismiss="alert" aria-hidden="true">×</button>
                  <h5><i class="icon fas fa-ban"></i> Alert!</h5>
                <%=errorMessage %>
                </div>
    <%}%>
<!-- /.login-box -->

<uc1:RequiredScript runat="server" ID="RequiredScript" />

</body>
</html>

