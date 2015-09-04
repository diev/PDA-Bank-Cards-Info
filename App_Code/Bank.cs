using System;
using System.Web;
using System.Web.Security;
using System.Configuration;
using System.Web.Configuration;
using System.IO;
using System.Collections.Specialized;

/// <summary>
/// Summary description for Bank
/// </summary>
public class Bank
{
    public Bank()
    {
            //
            // TODO: Add constructor logic here
            //
    }
    /*
    private static string CurString(decimal value)
    {
        return "<b>" + value.ToString("F2") + "</b>";
    }
    public static string CurByCode(decimal value, string code)
    {
        return CurString(value) + " " + code;
    }
    public static string CurByCode(object value, object code)
    {
        return CurString((decimal)value) + " " + (string)code;
    }
     */

    public const string sadmin = "admin"; //"********";
    public const string admins = "admins";
    public const string operators = "operators";

    public const string dateFormat = "dd.MM.yy";

    public static string ConnectionString()
    {
        //Configuration config = WebConfigurationManager.OpenWebConfiguration("~");
        //SectionInformation asInfo = config.AppSettings.SectionInformation;
        //SectionInformation csInfo = config.ConnectionStrings.SectionInformation;

        //txt = ConfigurationManager.AppSettings["key"];
        //txt = ConfigurationManager.ConnectionStrings["key"].ConnectionString;
        return ConfigurationManager.ConnectionStrings["ConnectionDataMart"].ConnectionString;
    }

    public static string AppSetting(string key)
    {
        Configuration rootWebConfig =
            WebConfigurationManager.OpenWebConfiguration("~");
        if (rootWebConfig.AppSettings.Settings.Count > 0)
        {
            KeyValueConfigurationElement customSetting =
                rootWebConfig.AppSettings.Settings[key];
            if (customSetting != null)
                return customSetting.Value;
        }
        return "";
    }

    public static bool CreateUser(string userName, string userPass, string userEmail,
        out string errMessage)
    {
        //MembershipCreateStatus result;
        errMessage = "";
        try
        {
            MembershipUser newUser = Membership.CreateUser(userName, userPass, userEmail);
            return true;
        }
        catch (MembershipCreateUserException e)
        {
            errMessage = GetErrorMessage(e.StatusCode);
            return false;
        }
        catch (HttpException e)
        {
            errMessage = e.Message;
            return false;
        }
    }
    public static string GetErrorMessage(MembershipCreateStatus status)
    {
        switch (status)
        {
            case MembershipCreateStatus.DuplicateUserName:
                return "Username already exists. Please enter a different user name.";

            case MembershipCreateStatus.DuplicateEmail:
                return "A username for that e-mail address already exists. Please enter a different e-mail address.";

            case MembershipCreateStatus.InvalidPassword:
                return "The password provided is invalid. Please enter a valid password value.";

            case MembershipCreateStatus.InvalidEmail:
                return "The e-mail address provided is invalid. Please check the value and try again.";

            case MembershipCreateStatus.InvalidAnswer:
                return "The password retrieval answer provided is invalid. Please check the value and try again.";

            case MembershipCreateStatus.InvalidQuestion:
                return "The password retrieval question provided is invalid. Please check the value and try again.";

            case MembershipCreateStatus.InvalidUserName:
                return "The user name provided is invalid. Please check the value and try again.";

            case MembershipCreateStatus.ProviderError:
                return "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

            case MembershipCreateStatus.UserRejected:
                return "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

            default:
                return "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
        }
    }
    public static StringCollection GetAvailableThemes()
    {
        string path = HttpContext.Current.Request.PhysicalApplicationPath + @"App_Themes";
        DirectoryInfo dir = new DirectoryInfo(path);
        StringCollection themes = new StringCollection();
        foreach (DirectoryInfo di in dir.GetDirectories())
            themes.Add(di.Name);
        return themes;
    }
    public static string GetNextTheme(string theme)
    {
        StringCollection themes = GetAvailableThemes();
        int i = themes.IndexOf(theme) + 1;
        if (i == themes.Count)
            i = 0;
        return themes[i];
    }
    public static void Log(string file, string text, params Object[] args)
    {
        string path = HttpContext.Current.Request.PhysicalApplicationPath + @"App_Data\\" + file;
        File.AppendAllText(path, String.Format(text, args));
    }
    public static void LoginLog(string user)
    {
        //Bank.Log("{0} {1} {2} \"{3}\"\n", DateTime.Now, user, Request.UserHostAddress, Request.UserAgent);
        HttpContext ctx = HttpContext.Current;
        string file = String.Format("login.{0:yyyy'.'MM}.log", DateTime.Now);
        Log(file, "{0} {1} {2} \"{3}\"\n", DateTime.Now,
            user, //ctx.Request.User.Identity.Name,
            ctx.Request.UserHostAddress,
            ctx.Request.UserAgent);
    }
    public static void HackLog(string user, string pass)
    {
        //Bank.Log("{0} {1} {2} \"{3}\"\n", DateTime.Now, user, Request.UserHostAddress, Request.UserAgent);
        HttpContext ctx = HttpContext.Current;
        string file = String.Format("hack.{0:yyyy'.'MM}.log", DateTime.Now);
        Log(file, "{0} {1}/{2} {3} \"{4}\"\n", DateTime.Now,
            user, //ctx.Request.User.Identity.Name,
            pass,
            ctx.Request.UserHostAddress,
            ctx.Request.UserAgent);
    }
}
