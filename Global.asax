<%@ Application Language="C#" %>

<script runat="server">

    void Application_Start(object sender, EventArgs e) 
    {
        // Code that runs on application startup

    }
    
    void Application_End(object sender, EventArgs e) 
    {
        //  Code that runs on application shutdown

    }
        
    void Application_Error(object sender, EventArgs e) 
    { 
        // Code that runs when an unhandled error occurs

    }

    void Application_BeginRequest(object sender, EventArgs e)
    {
        /*
        HttpContext context = HttpContext.Current;
        foreach (string key in context.Request.QueryString.AllKeys)
        {
            switch (key.ToLower())
            {
                case "pda":
                case "openpassword":
                case "changepassword":
                    string value = context.Request.QueryString[key];
                    if (value.Equals("-1"))
                    {
                        try
                        {
                            object o = Session[key];
                            if (o == null)
                                Session[key] = false;
                            else
                                Session[key] = !(bool)Session[key];
                        }
                        catch (HttpException exp)
                        {
                            //?
                        }
                    }
                    else
                        Session[key] = value.Equals("1");
                    break;
            }
        }
        */
    }
    
    void Session_Start(object sender, EventArgs e) 
    {
        // Code that runs when a new session is started
        string ua = Request.UserAgent;
        Session["PDA"] = ua.Contains("240x") || ua.Contains("Mobile") || ua.Contains("Phone");
        Session["OpenPassword"] = false;
        Session["ChangePassword"] = false;
    }

    void Session_End(object sender, EventArgs e) 
    {
        // Code that runs when a session ends. 
        // Note: The Session_End event is raised only when the sessionstate mode
        // is set to InProc in the Web.config file. If session mode is set to StateServer 
        // or SQLServer, the event is not raised.

    }
    
</script>
