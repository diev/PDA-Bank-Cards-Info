using System;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Cards : System.Web.UI.Page
{
    protected static DateTime RepDate;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            string userName = User.Identity.Name;
            SqlCards.SelectParameters["client_login"].DefaultValue = userName;
            SqlTrans.SelectParameters["client_login"].DefaultValue = userName;
            SqlHolds.SelectParameters["client_login"].DefaultValue = userName;
            SqlDebts.SelectParameters["client_login"].DefaultValue = userName;

            RepDate = DateTime.Today.AddDays(-DateTime.Today.Day + 1);
            MonthlyReport();

            /*
                <FilterParameters>
                    <asp:Parameter DefaultValue="2008-01-01" Name="StartDate" />
                </FilterParameters>
             */

            //SqlTrans.FilterParameters[0].DefaultValue = DateTime.Today.AddDays(-5).ToString("yyyy-MM-dd");
        }
    }
    protected void CardsList_SelectedIndexChanged(object sender, EventArgs e)
    {
        BackButton.Visible = true;
        CardsList.Visible = false;
    }
    protected void BackButton_Click(object sender, EventArgs e)
    {
        BackButton.Visible = false;
        CardsList.Visible = true;
        CardsList.SelectedIndex = -1;
    }
    protected void FilterButton_Command(object sender, CommandEventArgs e)
    {
        //DateTime StartDate;
        //int n = int.Parse((string)e.CommandArgument);
        switch (e.CommandName)
        {
            case "prev":
                //StartDate = DateTime.Today.AddDays(-n);
                RepDate = RepDate.AddMonths(-1);
                break;
            case "next":
                //StartDate = DateTime.Today.AddMonths(-n);
                RepDate = RepDate.AddMonths(1);
                break;
            case "today":
                //StartDate = DateTime.Today.AddYears(-n);
                RepDate = DateTime.Today.AddDays(-DateTime.Today.Day + 1);
                break;
            default:
                //StartDate = DateTime.Today.AddDays(-10);
                break;
        }
        //SqlTrans.FilterParameters[0].DefaultValue = StartDate.ToString("yyyy-MM-dd");
        MonthlyReport();
    }
    /*
    protected void TotalButton_Command(object sender, CommandEventArgs e)
    {
        decimal sum = 0;
        int num = 0;
        Control ctl;

        Button btn = (Button)sender;

        btn.Text = "?";
        switch (e.CommandName)
        {
            default:
                for (int i = 0; i < TransList.Items.Count; i++)
                {
                    ctl = TransList.Items[i].FindControl("Selected");
                    if (((CheckBox)ctl).Checked)
                    {
                        ctl = TransList.Items[i].FindControl("Sum");
                        sum += Convert.ToDecimal(((Label)ctl).Text);
                        ((Button)sender).Text = String.Format("{0} в {1}", sum, ++num);
                    }
                }
                ctl = btn.Parent.FindControl("Warning");
                ((Label)ctl).Visible = !btn.Text.Equals("?");
                break;
        }
    }
    */
    protected void CardsList_PreRender(object sender, EventArgs e)
    {
        CardsList.ShowFooter = CardsList.Items.Count == 0;
    }
    protected void HoldsList_PreRender(object sender, EventArgs e)
    {
        HoldsList.ShowHeader = HoldsList.Items.Count > 0;
    }
    protected void MonthlyReport()
    {
        //TransList.Caption = "Операции за " + RepDate.ToString("MMMM yyyy");

        SqlTrans.FilterParameters.Clear();
        SqlTrans.FilterParameters.Add("From", RepDate.ToString());
        SqlTrans.FilterParameters.Add("Before", RepDate.AddMonths(1).ToString());
        SqlTrans.DataBind();
    }
}
