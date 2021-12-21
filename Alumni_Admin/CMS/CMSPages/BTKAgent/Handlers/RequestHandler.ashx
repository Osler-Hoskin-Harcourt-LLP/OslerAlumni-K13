<%@ WebHandler Language="C#" Class="RequestHandler" %>

using System.Web;

public class RequestHandler : IHttpHandler
{
    public void ProcessRequest(HttpContext context)
    {
        BizStreamToolkit.Agent.RequestHandler.ProcessRequest(context);
    }

    public bool IsReusable
    {
        get { return false; }
    }
}