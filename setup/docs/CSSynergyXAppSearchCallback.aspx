<%@ Page Language="C#" %>

<%@ Register TagPrefix="ex" Namespace="Exact.Web.UI.Controls" Assembly="Exact.Web.UI.Controls" %>
<%@ Import Namespace="System.Collections.Generic" %>
<%@ Import Namespace="Newtonsoft.Json" %>
<%@ Import Namespace="CSSynergyX.AppSearch.Core" %>

<html>
<script runat="server">

    private List<CSSynergyX.AppSearch.Core.Entities.Application> _applications;

    private void Page_Init()
    {
        AppLog = false;
        IgnoreCallStack = true;

        string cacheName = "CSSynergyXApplications";

        if (env.Cache[cacheName] != null)
        {
            _applications = (List<CSSynergyX.AppSearch.Core.Entities.Application>)env.Cache[cacheName];
        }
        else
        {
            _applications = CSSynergyX.AppSearch.Core.Tools.GetApplications();
            env.Cache.Add(cacheName, _applications, (int)TimeSpan.FromDays(7).TotalMinutes);
        }
    }

    private void Page_Load()
    {
        var action = (Actions)Convert.ToInt16(Action.Value);
        switch (action)
        {
            case Actions.GetFilteredApplication:
                var result = CSSynergyX.AppSearch.Core.Tools.GetFilteredApplication(Convert.ToString(InputSearch.Value), _applications);
                WriteResponse(result);
                break;
            default:
                break;
        }
    }


    private void WriteResponse(object obj, int statusCode = 200)
    {
        var settings = new Newtonsoft.Json.JsonSerializerSettings
        {
            ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver()
        };
        Response.Clear();
        Response.ContentType = "application/json; charset=utf-8";
        Response.StatusCode = statusCode;
        Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(obj, settings));

        HttpContext.Current.Response.Flush();
        HttpContext.Current.Response.SuppressContent = true;
        HttpContext.Current.ApplicationInstance.CompleteRequest();
    }

    private enum Actions
    {
        GetFilteredApplication = 1
    }

</script>
<body>
    <ex:InputField runat="server" id="Action" ValidationAs="Int" />
    <ex:InputField runat="server" id="InputSearch" ValidationAs="None" />
</body>
</html>