using System;
using System.Web.UI.WebControls;

public partial class Excel : System.Web.UI.Page
{
    protected static DateTime RepDate;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            string userName = User.Identity.Name;
            SqlCards.SelectParameters["client_login"].DefaultValue = userName;
            SqlTrans.SelectParameters["client_login"].DefaultValue = userName;

            RepDate = DateTime.Today.AddDays(-DateTime.Today.Day + 1);
            MonthlyReport();

            if (CardsGrid.Rows.Count > 0)
                CardsGrid.SelectedIndex = 0;
        }
    }
    protected void MonthlyReport()
    {
        //TransList.Caption = "Операции за " + RepDate.ToString("MMMM yyyy");

        SqlTrans.FilterParameters.Clear();
        SqlTrans.FilterParameters.Add("From", RepDate.ToString());
        SqlTrans.FilterParameters.Add("Before", RepDate.AddMonths(1).ToString());
        SqlTrans.DataBind();
    }
    protected void CardsGrid_SelectedIndexChanged(object sender, EventArgs e)
    {
        MonthlyReport();
    }
}
