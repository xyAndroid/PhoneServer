<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="SCMS.Web.Login" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManagerDefault" runat="server">
    </asp:ScriptManager>
    <div id="Div1" class="login_top" runat="server">
        <div class="login_expertsystem">
            <asp:Login ID="MyLogin" runat="server" OnLoggingIn="MyLogin_LoggingIn" FailureText="您输入的用户名或密码错误!"
                OnLoggedIn="MyLogin_LoggedIn">
                <LayoutTemplate>
                    <table cellpadding="3" border="0">
                        <tr>
                            <td>
                                <div id="divUser" class="div_username">
                                    <asp:TextBox ID="UserName" runat="server" CssClass="txtBoxUserName" Style="padding: 8px;"></asp:TextBox>
                                    <ajax:textboxwatermarkextender id="tbweUserName" runat="server" targetcontrolid="UserName"
                                        watermarktext="请输入用户名" watermarkcssclass="tbwe_username">
                                    </ajax:textboxwatermarkextender>
                                </div>
                            </td>
                            <td>
                                <span class="span_login_validator">
                                    <asp:RequiredFieldValidator ID="rfvUserName" runat="server" ErrorMessage="*" ForeColor="Red"
                                        ControlToValidate="UserName"></asp:RequiredFieldValidator>
                                </span>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <div id="divPassWord" class="div_username">
                                    <asp:TextBox ID="Password" runat="server" TextMode="Password" CssClass="txtBoxPassword"
                                        Style="padding: 8px; width: 128px; height: 12px;"></asp:TextBox>
                                    <ajax:textboxwatermarkextender id="tbwePassword" runat="server" targetcontrolid="Password"
                                        watermarktext="在此输入密码" watermarkcssclass="tbwe_password">
                                    </ajax:textboxwatermarkextender>
                                    <a id="forgetPwd" href="Account/FindPassword.aspx" class="a_forgetpwd">忘记密码</a>
                                </div>
                            </td>
                            <td>
                                <span class="span_login_validator">
                                    <asp:RequiredFieldValidator ID="rfvPasswprd" runat="server" ErrorMessage="*" ForeColor="Red"
                                        ControlToValidate="Password"></asp:RequiredFieldValidator>
                                </span>
                            </td>
                        </tr>
                        <tr>
                            <%--此处放验证码--%>
                            <table cellspacing="0">
                                <tr>
                                    <td>
                                        <div style="margin-left: 4px;">
                                            <asp:TextBox ID="VerificationCode" runat="server" CausesValidation="True" Style="padding: 8px;
                                                border: solid 1px #3197B5;" Height="14px" Width="88px">
                                            </asp:TextBox>
                                            <ajax:textboxwatermarkextender id="TextBoxWatermarkExtender1" runat="server" targetcontrolid="VerificationCode"
                                                watermarktext="请输入验证码" watermarkcssclass="tbwe_username">
                                            </ajax:textboxwatermarkextender>
                                        </div>
                                    </td>
                                    <td>
                                        <asp:RequiredFieldValidator ID="rqfvVerificationCode" runat="server" ErrorMessage="*"
                                            ControlToValidate="VerificationCode" ForeColor="Red"></asp:RequiredFieldValidator>
                                        <asp:Image ID="imgValidCode" runat="server" ImageUrl="~/ValidPage.aspx" Width="80px"
                                            Height="30px" />
                                    </td>
                                </tr>
                            </table>
                        </tr>
                        <tr>
                            <td colspan="2" style="padding-top: 7px;">
                                <div class="div_regist">
                                    <asp:LinkButton ID="lnkBtnRegister" runat="server" PostBackUrl="~/Account/ProjectRegister.aspx"
                                        CausesValidation="False" Text="注册" Font-Bold="true" Font-Size="9pt"></asp:LinkButton>
                                </div>
                                <div>
                                    &nbsp;<asp:ImageButton ID="imgBtnLogin" runat="server" ImageUrl="~/App_Themes/Blue/Images/loginImageButton.png"
                                        CommandName="Login" AccessKey="0" />
                                </div>
                            </td>
                        </tr>
                    </table>
                </LayoutTemplate>
            </asp:Login>
        </div>
    </div>
    </form>
</body>
</html>
