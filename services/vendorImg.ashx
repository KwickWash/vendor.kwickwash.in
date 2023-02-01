<%@ WebHandler Language="C#" Class="vendorImg" %>

using System;
using System.Web;

public class vendorImg : IHttpHandler {

    public void ProcessRequest(HttpContext context)
    {
        for (int i = 0; i < context.Request.Files.Count; i++)
        {
            var file = context.Request.Files[i];
            // Do something with this file, for example:
            string[] files = file.FileName.Split('.');
            string ext = files[files.Length - 1].ToString();
            string path = HttpContext.Current.Server.MapPath("~/upload/vendorImg/");
            string Filename = Function.MilliTimeStamp().ToString() + "." + ext;
            path = path + Filename;
            file.SaveAs(path.ToLower());
            context.Response.Write(Filename.ToLower());
        }
    }

    public bool IsReusable {
        get {
            return false;
        }
    }

}