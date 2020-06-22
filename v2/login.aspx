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
   <b>Gpos</b>RST40
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
              <input type="checkbox" id="remember">
              <label for="remember">
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

    
      <!-- /.social-auth-links -->
  <%--    <p class="mb-1">
        <a href="forgot-password.html">I forgot my password</a>
      </p>
      <p class="mb-0">
        <a href="register.html" class="text-center">Register a new membership</a>
      </p>--%>
    </div>
    <!-- /.login-card-body -->
  </div>
</div>
<!-- /.login-box -->

<uc1:RequiredScript runat="server" ID="RequiredScript" />

</body>
</html>

