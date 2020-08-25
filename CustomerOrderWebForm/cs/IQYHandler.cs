using System;
using System.Web;

namespace CustomerOrderWebForm
{
  public class IQYHandler : IHttpHandler
  {
    private String _FileName;
    public String FileName
    {
      get { return _FileName; }
      set
      {
        _FileName = value ?? "";
        _FileName = _FileName.Substring(_FileName.LastIndexOf(@"/") + 1);
      }
    }

    public void ProcessRequest(HttpContext context)
    {
      HttpResponse Response = context.Response;
      HttpRequest Request = context.Request;
      
      FileName = Request.FilePath;
      String sURL = Request.Url.ToString().Replace(".iqy", ".aspx");

      Response.ContentType = "text/x-ms-iqy";
      Response.AddHeader("Content-Disposition", "attachment; filename=" + FileName);

      Response.Output.WriteLine("WEB");
      Response.Output.WriteLine("1");
      Response.Output.WriteLine(sURL);
      Response.Output.WriteLine("");
      Response.Output.WriteLine("Selection=tblReportBody");
      Response.Output.WriteLine("Formatting=All");
      Response.Output.WriteLine("PreFormattedTextToColumns=True");
      Response.Output.WriteLine("ConsecutiveDelimitersAsOne=True");
      Response.Output.WriteLine("SingleBlockTextImport=False");
      Response.Output.WriteLine("DisableDateRecognition=False");
      Response.Output.WriteLine("DisableRedirections=False");
    }

    public bool IsReusable
    {
      get
      {
        return false;
      }
    }
  }
}
