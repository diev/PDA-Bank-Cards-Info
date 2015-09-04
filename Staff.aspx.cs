using System;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using System.Web.Security;
//using System.Data;
//using System.Data.SqlClient;

public partial class Default : System.Web.UI.Page
{
    protected void Page_PreInit(object sender, EventArgs e)
    {
        //Page.Theme = (string)Profile.Theme;
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        //Security by default
        bool IsAdmin = false;
        bool IsOperator = false;

        if (!User.Identity.IsAuthenticated)
            FormsAuthentication.RedirectToLoginPage();
        if (User.Identity.Name.Equals(Bank.sadmin)) //exception for admin's external access
            IsAdmin = true;
        else if (Request.UserHostAddress.StartsWith(Bank.AppSetting("LAN")) ||
            Request.IsLocal)
        {
            IsAdmin = Roles.IsUserInRole(Bank.admins);
            IsOperator = Roles.IsUserInRole(Bank.operators);
        }
        if (!IsAdmin && !IsOperator)
            Response.Redirect("~", true);

        UserListDataBind();

        if (!IsPostBack)
        {
            AdminUserList.Visible = true;
            NextUserButton.Visible = true;
            if (IsAdmin)
            {
                AdminActionList.Visible = true;
                ActionButton.Visible = true;
            }
            AdminUserList.Text = User.Identity.Name;
        }
    }
    private void UserListDataBind()
    {
        string saved = AdminUserList.SelectedValue;
        AdminUserList.DataSource = Membership.GetAllUsers();
        AdminUserList.DataBind();
        string s = "";
        for (int i = 0; i < AdminUserList.Items.Count; i++)
        {
            string userName = AdminUserList.Items[i].Value;
            MembershipUser user = Membership.GetUser(userName);
            if (!user.IsApproved)
                AdminUserList.Items[i].Text = userName + " [-]";
            else if (user.IsLockedOut)
            {
                if (user.UserName.Equals(Bank.sadmin))
                {
                    user.UnlockUser();
                    Message("Админ {0} был заблокирован!?", user.UserName);
                }
                else
                {
                    s += ", " + userName;
                    AdminUserList.Items[i].Text = userName + " [b]";
                }
            }
            else if (Roles.IsUserInRole(userName, Bank.admins) || Roles.IsUserInRole(userName, Bank.sadmin))
                AdminUserList.Items[i].Text = userName + " (A)";
            else if (Roles.IsUserInRole(userName, Bank.operators))
                AdminUserList.Items[i].Text = userName + " (o)";
        }
        if (s.Length > 0)
            Message("Есть заблокированные: {0}", s.Remove(0, 2));
        if (!string.IsNullOrEmpty(saved))
            AdminUserList.Text = saved;
    }
    protected void AdminButton_Command(object sender, CommandEventArgs e)
    {
        string userName = AdminUserList.SelectedValue;
        MembershipUser user = Membership.GetUser(userName);
        switch (e.CommandName)
        {
            case "NextUser":
                int index = AdminUserList.SelectedIndex + 1;
                AdminUserList.SelectedIndex = index == AdminUserList.Items.Count ? 0 : index;
                UserNameChanged();
                break;
            case "Action":
                string action = AdminActionList.SelectedValue;
                switch (action)
                {
                    case "AddUser":
                        LockControls();
                        AddUserPanel.Visible = true;
                        break;
                    case "Comment":
                        LockControls();
                        BigTextPanel.Visible = true;
                        BigTextBox.Text = user.Comment;
                        break;
                    case "Unlock":
                        if (user.LastLockoutDate.Year > 2000)
                            ConfirmAction("Пользователь {0} был заблокирован {1}. Разблокировать?", userName, user.LastLockoutDate);
                        else
                            Message("Пользователь {0} не блокирован.", userName);
                        break;
                    case "Password":
                        LockControls();
                        AddUserPanel.Visible = true;
                        AddUserName.Text = userName;
                        AddUserName.Enabled = false;
                        //AddUserPass.Text = Membership.GeneratePassword(
                        //    Membership.MinRequiredPasswordLength,
                        //    Membership.MinRequiredPasswordLength);//////////////////////////
                        AddUserPass.Text = "******";
                        AddUserEmail.Text = user.Email;
                        AddUserEmail.Enabled = false;
                        break;
                    case "ChEmail":
                        LockControls();
                        AddUserPanel.Visible = true;
                        AddUserName.Text = userName;
                        AddUserName.Enabled = false;
                        AddUserPass.Text = "******";
                        AddUserPass.Enabled = false;
                        AddUserEmail.Text = user.Email;
                        break;
                    case "Disable":
                        if (userName.Equals(Bank.sadmin))
                            Message("*Запрещено лишать доступа этого Администратора!");
                        else if (userName.Equals(User.Identity.Name))
                            Message("*Запрещено лишать доступа самого себя!");
                        else
                            ConfirmAction("Запретить доступ пользователю {0}?", userName);
                        break;
                    case "Enable":
                        ConfirmAction("Pазрешить доступ пользователю {0}?", userName);
                        break;
                    case "Email": //////////////////////////
                        Message("Почта пользователя {0}: <a href=\"mailto:{1}\">{1}</a>", userName, user.Email);
                        break;
                    case "User":
                        if (userName.Equals(Bank.sadmin))
                            Message("*Запрещено понижать этого Администратора!");
                        else if (userName.Equals(User.Identity.Name))
                            Message("*Запрещено понижать самого себя!");
                        else
                            ConfirmAction("Понизить пользователя {0} до прав простого клиента?", userName);
                        break;
                    case "Operator":
                        if (userName.Equals(Bank.sadmin))
                            Message("*Запрещены изменения этого Администратора!");
                        else if (userName.Equals(User.Identity.Name))
                            Message("*Запрещены изменения самого себя!");
                        else
                            ConfirmAction("Дать пользователю {0} права оператора в Банке?", userName);
                        break;
                    case "Admin":
                        ConfirmAction("Дать пользователю {0} права Администратора?!", userName);
                        break;
                    case "Delete":
                        if (userName.Equals(Bank.sadmin))
                            Message("*Запрещено удалять этого Администратора!");
                        else if (userName.Equals(User.Identity.Name))
                            Message("*Запрещено удалять самого себя!");
                        else
                            ConfirmAction("Удалить пользователя {0}?", userName);
                        break;
                }
                //UserListDataBind();
                break;
            case "AddUserOK":
                if (AddUserPass.Text.Length < Membership.MinRequiredPasswordLength)
                    Message("*Длина пароля должна быть не менее {0}.", Membership.MinRequiredPasswordLength);
                else if (AddUserPass.Text.Equals(userName, StringComparison.OrdinalIgnoreCase))
                    Message("*Пароль не должен совпадать с именем {0}.", userName);
                else if (AddUserName.Enabled)//AddUser
                {
                    string result = "";
                    if (AddUserPass.Text.Contains("*"))
                        Message("*Пароль не должен содержать звездочек.");
                    else if (Bank.CreateUser(
                        AddUserName.Text, AddUserPass.Text, AddUserEmail.Text,
                        out result))
                    {
                        UserListDataBind();
                        AdminUserList.Text = AddUserName.Text;
                        Page.DataBind();
                        Message("Новый пользователь {0} добавлен.", AddUserName.Text);
                        UnlockControls();
                    }
                    else
                    {
                        Message("*" + result);
                        //return;
                    }
                }
                else if (AddUserPass.Enabled)//Password
                {
                    if (AddUserPass.Text.Contains("*"))
                        Message("*Пароль не должен содержать звездочек.");
                    else
                    {
                        string newPass = Membership.Provider.ResetPassword(userName, "");
                        if (Membership.Provider.ChangePassword(userName, newPass, AddUserPass.Text))
                            Message("Пароль пользователя {0} успешно сменен.", userName);
                        else
                            Message("*В смене пароля пользователю {0} отказано.", userName);
                        UnlockControls();
                    }
                }
                else if (AddUserEmail.Enabled)//ChEmail
                {
                    user.Email = AddUserEmail.Text;
                    Membership.UpdateUser(user);
                    Message("Email пользователя {0} сменен на <a href=\"mailto:{1}\">{1}</a>.", userName, user.Email);
                    UnlockControls();
                }
                break;
            case "BigTextOK":
                user.Comment = BigTextBox.Text;
                Membership.UpdateUser(user);
                UnlockControls();
                break;
            case "ConfirmOK":
                string arg1 = ConfirmButton1.CommandArgument;
                string arg2 = ConfirmButton2.CommandArgument;
                switch (arg1)
                {
                    case "Unlock":
                        if (user.UnlockUser())
                            Message("Пользователь {0} разблокирован.", arg2);
                        else
                            Message("*Пользователя {0} разблокировать не удалось.", arg2);
                        break;
                    case "Disable":
                        user.IsApproved = false;
                        Membership.UpdateUser(user);
                        UserListDataBind();
                        Message("Пользователю {0} доступ запрещен.", arg2);
                        break;
                    case "Enable":
                        user.IsApproved = true;
                        Membership.UpdateUser(user);
                        UserListDataBind();
                        Message("Пользователю {0} доступ разрешен.", arg2);
                        break;
                    case "User":
                        if (Roles.IsUserInRole(userName, Bank.admins))
                            Roles.RemoveUserFromRole(userName, Bank.admins);
                        if (Roles.IsUserInRole(userName, Bank.operators))
                            Roles.RemoveUserFromRole(userName, Bank.operators);
                        UserListDataBind();
                        Message("Пользователь {0} понижен до прав простого клиента.", arg2);
                        break;
                    case "Operator":
                        if (Roles.IsUserInRole(userName, Bank.admins))
                            Roles.RemoveUserFromRole(userName, Bank.admins);
                        if (!Roles.IsUserInRole(userName, Bank.operators))
                            Roles.AddUserToRole(userName, Bank.operators);
                        UserListDataBind();
                        Message("Пользователю {0} даны права оператора в Банке.", arg2);
                        break;
                    case "Admin":
                        if (Roles.IsUserInRole(userName, Bank.operators))
                            Roles.RemoveUserFromRole(userName, Bank.operators);
                        if (!Roles.IsUserInRole(userName, Bank.admins))
                            Roles.AddUserToRole(userName, Bank.admins);
                        UserListDataBind();
                        Message("Пользователю {0} даны права Администратора!", arg2);
                        break;
                    case "Delete":
                        if (Membership.DeleteUser(arg2, true))
                        {
                            int i = AdminUserList.SelectedIndex + 1;
                            AdminUserList.SelectedIndex = i == AdminUserList.Items.Count ? 0 : i;
                            UserListDataBind(); 
                            Message("Пользователь {0} удален.", arg2);
                        }
                        else
                            Message("*Пользователя {0} удалить не удалось.", arg2);
                        break;
                }
                UnlockControls();
                break;
            case "AdminCancel":
                UnlockControls();
                break;
            case "MessageOK":
                MsgPanel.Visible = false;
                break;
        }
    }
    public void Message(string text, params Object[] args)
    {
        string s = string.Format(text, args);
        MsgPanel.Visible = text.Length > 0;
        if (s.StartsWith("*"))
        {
            MsgPanel.BackColor = Color.LightYellow;
            Msg.ForeColor = Color.Red;
            Msg.Text = "Ошибка: " + s.Remove(0, 1);
        }
        else
        {
            MsgPanel.BackColor = Color.LightGreen;
            Msg.ForeColor = Color.Blue;
            Msg.Text = s;
        }
    }
    protected void ConfirmAction(string text, params Object[] args)
    {
        LockControls();
        ConfirmPanel.Visible = true;
        ConfirmLabel.Text = string.Format(text, args);
        ConfirmButton1.CommandArgument = AdminActionList.SelectedValue;
        ConfirmButton2.CommandArgument = AdminUserList.SelectedValue;
    }
    public void LockControls()
    {
        AdminUserList.Enabled = false;
        NextUserButton.Enabled = false;
        AdminActionList.Enabled = false;
        ActionButton.Enabled = false;
    }
    public void UnlockControls()
    {
        AdminUserList.Enabled = true;
        NextUserButton.Enabled = true;
        AdminActionList.Enabled = true;
        ActionButton.Enabled = true;
        AddUserPanel.Visible = false;
        BigTextPanel.Visible = false;
        ConfirmPanel.Visible = false;
        AddUserName.Enabled = true;
        AddUserPass.Enabled = true;
        AddUserEmail.Enabled = true;
    }
    protected void GridCards_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            object dataItem = e.Row.DataItem;
            int cardId = (int)DataBinder.Eval(dataItem, "card_id");
            string cardNo = ((string)DataBinder.Eval(dataItem, "card_number")).Trim();
            string cardType = (string)DataBinder.Eval(dataItem, "card_type_name");
            string cur = (string)DataBinder.Eval(dataItem, "card_currency");
            decimal amount = (decimal)DataBinder.Eval(dataItem, "amount_available");
            decimal hold = (decimal)DataBinder.Eval(dataItem, "amount_on_hold");
            decimal debt = (decimal)DataBinder.Eval(dataItem, "amount_debt");
            DateTime exp = (DateTime)DataBinder.Eval(dataItem, "exp_date");
            string cardStatus = (string)DataBinder.Eval(dataItem, "card_status");

            ((Label)e.Row.FindControl("CardNo")).Text = cardNo;
            ((Label)e.Row.FindControl("CardType")).Text = cardType;
            ((Label)e.Row.FindControl("CardExp")).Text = exp.ToString("MM'/'yy");
            ((Label)e.Row.FindControl("CardStatus")).Text = "("+cardStatus+")";

            ((Label)e.Row.FindControl("CardAmount")).Text = "<b>" + amount.ToString("F2") + "</b> " + cur;

            if (hold > 0)
            {
                //GridCards.Columns[3].Visible = true;
                ((Label)e.Row.FindControl("CardHold")).Text = "<b>" + hold.ToString("F2") + "</b> " + cur;
                ((LinkButton)e.Row.FindControl("CardHoldQ")).Visible = true;
            }

            if (debt > 0)
            {
                //GridCards.Columns[4].Visible = true;
                ((Label)e.Row.FindControl("CardDebt")).Text = "<b>" + debt.ToString("F2") + "</b> " + cur;
                ((LinkButton)e.Row.FindControl("CardDebtQ")).Visible = true;
            }
        }
    }
    protected void GridTrans_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            object dataItem = e.Row.DataItem;
            DateTime trDate = (DateTime)DataBinder.Eval(dataItem, "transaction_date");
            decimal amount = (decimal)DataBinder.Eval(dataItem, "amount");
            string cur = (string)DataBinder.Eval(dataItem, "operation_currency_iso");
            string comment = (string)DataBinder.Eval(dataItem, "comment");

            ((Label)e.Row.FindControl("TranDate")).Text = trDate.ToString(Bank.dateFormat);
            ((Label)e.Row.FindControl("TranComment")).Text = comment;

            Label labelAmount = (Label)e.Row.FindControl("TranAmount");
            if (amount != 0)
                labelAmount.Text = amount.ToString("F2");
            if (amount > 0)
                labelAmount.ForeColor = Color.Green;
            ((Label)e.Row.FindControl("TranCur")).Text = cur;
        }
    }
    protected void GridHolds_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            object dataItem = e.Row.DataItem;
            DateTime opDate = (DateTime)DataBinder.Eval(dataItem, "operation_date");
            decimal amount = (decimal)DataBinder.Eval(dataItem, "amount");
            string cur = (string)DataBinder.Eval(dataItem, "operation_currency_iso");
            string comment = (string)DataBinder.Eval(dataItem, "comment");

            ((Label)e.Row.FindControl("HoldDate")).Text = opDate.ToString(Bank.dateFormat);
            ((Label)e.Row.FindControl("HoldAmount")).Text = "<b>" + amount.ToString("F2") + "</b> " + cur;
            ((Label)e.Row.FindControl("HoldComment")).Text = comment;
        }
    }
    protected void AdminUserList_SelectedIndexChanged(object sender, EventArgs e)
    {
        UserNameChanged();
    }
    protected void UserNameChanged()
    {
        GridCards.SelectedIndex = -1;
        GridTrans.Visible = false;
        GridHolds.Visible = false;
        DetailsDebts.Visible = false;
    }
    protected void GridCards_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        switch ((string)e.CommandArgument)
        {
            case "Trans":
                GridTrans.Visible = true;
                GridTrans.DataBind();
                Excel.Visible = true;
                GridHolds.Visible = false;
                DetailsDebts.Visible = false;
                break;
            case "Holds":
                GridTrans.Visible = false;
                Excel.Visible = false;
                GridHolds.Visible = true;
                GridHolds.DataBind();
                DetailsDebts.Visible = false;
                break;
            case "Debts":
                GridTrans.Visible = false;
                Excel.Visible = false;
                GridHolds.Visible = false;
                DetailsDebts.Visible = true;
                DetailsDebts.DataBind();
                break;
        }
    }
    protected void Excel_Click(object sender, EventArgs e)
    {
        GridTrans.UseAccessibleHeader = true;
        GridTrans.AllowPaging = false;
        GridTrans.AllowSorting = false;
        GridTrans.Caption = "Экспорт в Microsoft Excel";
        //GridTrans.PageSize = 0;
        GridTrans.DataBind();
       
        //http://geekswithblogs.net/azamsharp/archive/2005/12/21/63843.aspx
        //http://www.c-sharpcorner.com/UploadFile/DipalChoksi/exportxl_asp2_dc11032006003657AM/exportxl_asp2_dc.aspx?ArticleID=000c64fb-8a22-414a-8247-984335aaa0eb

        string attachment = "attachment; filename=CardTrans.xls";
        Response.Clear();
        Response.ClearContent();
        Response.Buffer = true;
        Response.Charset = "";
        this.EnableViewState = false;
        Response.AddHeader("content-disposition", attachment);
        Response.ContentType = "application/ms-excel";
        StringWriter sw = new StringWriter();
        HtmlTextWriter htw = new HtmlTextWriter(sw);
        this.ClearControls(GridTrans);
        GridTrans.RenderControl(htw);
        Response.Write(sw.ToString());
        Response.End();
    }
    public override void VerifyRenderingInServerForm(Control control)
    {
    }
    private void ClearControls(Control control)
    {
        for (int i=control.Controls.Count -1; i>=0; i--)
        {
            ClearControls(control.Controls[i]);
        }
        if (!(control is TableCell))
        {
            if (control.GetType().GetProperty("SelectedItem") != null)
            {
                LiteralControl literal = new LiteralControl();
                control.Parent.Controls.Add(literal);
                try
                {
                    literal.Text = (string)control.GetType().GetProperty("SelectedItem").GetValue(control,null);
                }
                catch
                {
                }
                control.Parent.Controls.Remove(control);
            }
            else
            if (control.GetType().GetProperty("Text") != null)
            {
                LiteralControl literal = new LiteralControl();
                control.Parent.Controls.Add(literal);
                literal.Text = (string)control.GetType().GetProperty("Text").GetValue(control,null);
                control.Parent.Controls.Remove(control);
            }
        }
        return;
    }
}
