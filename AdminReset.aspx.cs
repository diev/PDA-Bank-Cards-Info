using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;

public partial class AdminReset : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        MembershipUser user = Membership.GetUser(Bank.sadmin);
        if (user.LastLockoutDate.Year > 2000)
            user.UnlockUser();

        string newPass = Membership.Provider.ResetPassword(Bank.sadmin, "");
        NewPass.Text = newPass;
        //if (Membership.Provider.ChangePassword(Bank.sadmin, newPass, ""))

    }
}
