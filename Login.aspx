<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Login.aspx.cs"
    Inherits="Login" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Вход в Банк</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:Label ID="Logo" runat="server" Text="<%$ AppSettings:Logo %>"
            SkinID="Logo" />
        <asp:Panel ID="MessagePanel" runat="server" Visible="false" SkinID="Message"
            DefaultButton="MessageOK">
            <table width="100%">
                <tr valign="top">
                    <td>
                        <asp:Label ID="MessageLabel" runat="server" />
                    </td>
                    <td align="right">
                        <asp:Button ID="MessageOK" runat="server" Text="OK" CommandName="ok"
                            OnCommand="Message_Command" />
                        <asp:Button ID="MessageCancel" runat="server" CommandName="cancel"
                            Text="Cancel" OnCommand="Message_Command" Visible="False" />
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <asp:Panel ID="LoginPanel" runat="server" DefaultButton="LoginOK">
            <asp:Label ID="LoginUserNameLabel" runat="server" Text="Логин:" />
            <br />
            <asp:TextBox ID="LoginUserName" runat="server" MaxLength="20"
                TabIndex="1" />
            <br />
            <asp:Label ID="LoginPasswordLabel" runat="server" Text="Пароль:" />
            <br />
            <asp:TextBox ID="LoginPassword" runat="server" TextMode="Password"
                MaxLength="20" TabIndex="2" />
            <br />
            <asp:Panel ID="ChangePanel" runat="server" 
                DefaultButton="LoginOK" Visible="False">
                <asp:Label ID="ChangePassword1Label" runat="server" Text="Новый пароль:" />
                <br />
                <asp:TextBox ID="ChangePassword1" runat="server" MaxLength="20"
                    TabIndex="3" TextMode="Password" />
                <br />
                <asp:Label ID="ChangePassword2Label" runat="server" Text="Новый пароль еще раз:" />
                <br />
                <asp:TextBox ID="ChangePassword2" runat="server" MaxLength="20"
                    TabIndex="4" TextMode="Password" />
                <br />
            </asp:Panel>
            <asp:CheckBox ID="SaveCheck" runat="server" Text="Запомнить меня"
                TabIndex="5" />
            <br />
            <asp:CheckBox ID="PDACheck" runat="server" Text="PDA версия"
                TabIndex="6" Visible="False" />
            <br />
            <asp:Button ID="LoginOK" runat="server" Text="Войти" CommandName="login"
                OnCommand="Login_Command" TabIndex="7" />
            <asp:Button ID="LoginCancel" runat="server" Text="Отмена" CommandName="cancel"
                OnCommand="Login_Command" TabIndex="8" 
                Visible="False" />
            <br />
            <asp:HyperLink ID="LinkChange" runat="server" TabIndex="9" 
                NavigateUrl="?ChangePassword=-1">Сменить пароль</asp:HyperLink>
            <asp:Literal ID="LinkChangeBreak" runat="server" Text="<br />" />
            <asp:HyperLink ID="LinkOpen" runat="server" TabIndex="10" 
                NavigateUrl="?OpenPassword=-1">Открыть пароль</asp:HyperLink>
        </asp:Panel>
    </div>
    </form>
</body>
</html>
