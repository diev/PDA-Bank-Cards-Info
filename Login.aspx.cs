using System;
using System.Web.UI.WebControls;
using System.Drawing;
using System.Web.Security;

public partial class Login : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (User.Identity.IsAuthenticated)
                FormsAuthentication.SignOut();
            PDACheck.Checked = (bool)Session["PDA"];
        }

        foreach (string key in Request.QueryString.AllKeys)
            switch (key.ToLower())
            {
                case "openpassword":
                case "changepassword":
                    string value = Request.QueryString[key];
                    if (value.Equals("-1"))
                        Session[key] = !(bool)Session[key];
                    else
                        Session[key] = value.Equals("1");
                    break;
            }

        if ((bool)Session["ChangePassword"])
        {
            ChangePanel.Visible = true;
            LoginOK.CommandName = "change";
            LoginCancel.Visible = true;
            LinkChange.Visible = false;
            LinkChangeBreak.Visible = false;
        }

        if ((bool)Session["OpenPassword"])
        {
            LoginPassword.TextMode = TextBoxMode.SingleLine;
            ChangePassword1.TextMode = TextBoxMode.SingleLine;
            ChangePassword2.TextMode = TextBoxMode.SingleLine;
            LinkOpen.Text = "Скрыть пароль";
        }
    }
    protected void Login_Command(object sender, CommandEventArgs e)
    {
        if (e.CommandName == "cancel")
            Response.Redirect("?changepassword=0", true);

        string user = LoginUserName.Text.ToLower();
        string pass = LoginPassword.Text;
        string pass1 = ChangePassword1.Text;
        string pass2 = ChangePassword2.Text;
        bool save = SaveCheck.Checked;

        if (user.Length == 0)
            Message("*Не указан логин!");
        else if (pass.Length == 0)
            Message("*Не указан пароль!");
        else if (pass.Length < Membership.MinRequiredPasswordLength)
            Message("*Пароль требуется не менее {0} знаков!", Membership.MinRequiredPasswordLength);
        else
        {
            MembershipUser u = Membership.GetUser(user);
            if (u == null)
            {
                Bank.HackLog(user + "[?]", pass);
                Message("*У нас таких нет!");
            }
            else if (u.IsLockedOut)
            {
                Bank.HackLog(user + "[b]", pass);
                Message("*Вы заблокированы из-за подбора пароля! Звоните в Банк!");
            }
            else if (!u.IsApproved)
            {
                Bank.HackLog(user + "[x]", pass);
                Message("*Вам запрещен доступ к системе!");
            }
            else if (!Membership.ValidateUser(user, pass))
            {
                Bank.HackLog(user, pass + "[?]");
                Message("*Пароль неверен! Вас могут заблокировать!");
            }
            else if (e.CommandName == "login")
            {
                Session["PDA"] = PDACheck.Checked;
                Bank.LoginLog(user);
                FormsAuthentication.RedirectFromLoginPage(user, save);
            }
            else if (e.CommandName == "change")
            {
                if (pass1.Length < Membership.MinRequiredPasswordLength)
                    Message("*Пароль требуется не менее {0} знаков!", Membership.MinRequiredPasswordLength);
                else if (pass1 != pass2)
                    Message("*Новые пароли не совпадают!");
                else if (!Membership.Provider.ChangePassword(user, pass, pass1))//////////////exception!
                {
                    Bank.HackLog(user, pass + "[-]");
                    Message("*В смене пароля отказано!");
                }
                else
                {
                    Session["PDA"] = PDACheck.Checked;
                    Bank.LoginLog(user);
                    FormsAuthentication.RedirectFromLoginPage(user, save);
                }
            }
        }
    }
    public void Message(string text, params Object[] args)
    {
        MessagePanel.Visible = true;
        string s = String.Format(text, args);
        switch (s.Substring(0, 1))
        {
            case "*":
                MessagePanel.BackColor = Color.LightYellow;
                MessageLabel.ForeColor = Color.Red;
                MessageLabel.Text = "Ошибка: " + s.Remove(0, 1);
                break;
            case "?":
                MessageCancel.Visible = true;
                MessagePanel.BackColor = Color.LightYellow;
                MessageLabel.ForeColor = Color.Red;
                MessageLabel.Text = "Вопрос: " + s.Remove(0, 1);
                break;
            default:
                MessagePanel.BackColor = Color.LightGreen;
                MessageLabel.ForeColor = Color.Blue;
                MessageLabel.Text = s;
                break;
        }
    }
    protected void Message_Command(object sender, CommandEventArgs e)
    {
        MessagePanel.Visible = false;
    }
}
