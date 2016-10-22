<%@ Application Language="VB" %>
<%@ Import Namespace="System.Web.Optimization" %>
<%@ Import Namespace="System.Web.Routing" %>


<script runat="server">

    Sub Application_Start(ByVal sender As Object, ByVal e As EventArgs)

        core.BundleConfig.RegisterBundles(BundleTable.Bundles)

        'Dim config As HttpConfiguration = GlobalConfiguration.Configuration

        'config.Formatters.JsonFormatter.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore

        BundleTable.Bundles.Add(New ScriptBundle("~/bundles/SPCInspectReporter").Include(
"~/Scripts/jquery.layout.js",
"~/Scripts/grid.locale-en.js",
"~/Scripts/jquery.jqGrid.min.js",
"~/Scripts/jquery.jqGrid.js",
"~/Scripts/wijmo/jquery.wijmo-open.all.3.20141.34.min.js",
"~/Scripts/D3/d3.v3.min.js",
"~/Scripts/D3/d3.v3.js",
"~/Scripts/wijmo/jquery.wijmo-pro.all.3.20141.34.min.js",
"~/Scripts/wijmo/wijmo.data.ajax.3.20141.34.js",
"~/Scripts/Gcharts/jsapi.js"))
        BundleTable.Bundles.Add(New ScriptBundle("~/bundles/InspectionVisualizer").Include(
                    "~/Scripts/jquery-1.11.1.js",
                    "~/Scripts/jquery-ui.min.js",
                    "~/Scripts/jquery.layout.js",
                    "~/Scripts/grid.locale-en.js",
                    "~/Scripts/jquery.jqGrid.min.js",
                    "~/Scripts/jquery.jqGrid.js",
                    "~/Scripts/Gcharts/jsapi.js",
                    "~/Scripts/owl/owl.carousel.js",
                    "~/Scripts/owl/owl.carousel.min.js"))

        BundleTable.Bundles.Add(New ScriptBundle("~/bundles/FlagBrdMaintenance").Include(
                       "~/Scripts/jquery-1.11.1.js",
                       "~/Scripts/jquery-ui.min.js",
                       "~/Scripts/wijmo/jquery.wijmo-open.all.3.20141.34.FlgBrd.min.cache",
                       "~/Scripts/wijmo/jquery.wijmo-pro.all.3.20141.34.FlgBrd.min.cache",
                       "~/Scripts/wijmo/wijmo.data.ajax.3.20141.34.FlgBrd.cache",
                       "~/Scripts/jquery.jqGrid.js",
                       "~/Scripts/jquery.layout.js",
                       "~/Scripts/grid.locale-en.js",
                       "~/Scripts/select2.js"
                       ))

    End Sub

    '    "~/Scripts/wijmo/jquery.wijmo-open.all.3.20141.34.min.js",
    '"~/Scripts/wijmo/jquery.wijmo-pro.all.3.20141.34.min.js",
    '"~/Scripts/wijmo/wijmo.data.ajax.3.20141.34.js",

    Sub Application_EndRequest(ByVal sender As Object, ByVal e As EventArgs)



        '        Dim threadPrincipal As System.Security.Principal.IPrincipal = System.Threading.Thread.CurrentPrincipal
        '        Dim IsAutho As String = threadPrincipal.Identity.IsAuthenticated
        '        'Dim UserName As String = Convert.ToString(System.Web.HttpContext.Current.User.Identity)   ' Context.User.Identity.Name
        '        Dim UO As New core.UserObjects


        '        If IsAutho = True Or Session("user") = "John" Then
        '            GoTo 1010
        '        Else
        '            Response.Redirect("~/Login.aspx")
        '        End If

        '1010:





    End Sub

    Public Overrides Sub Init()
        'MyBase.Init()

        'System.Diagnostics.Debugger.Break()

    End Sub





    Sub Application_End(ByVal sender As Object, ByVal e As EventArgs)
        ' Code that runs on application shutdown
    End Sub


    Sub Application_Error(ByVal sender As Object, ByVal e As EventArgs)
        ' Code that runs when an unhandled error occurs
    End Sub

    Sub Session_Start(ByVal sender As Object, ByVal e As EventArgs)
        ' Code that runs when a new session is started
    End Sub

    Sub Session_End(ByVal sender As Object, ByVal e As EventArgs)
        ' Code that runs when a session ends. 
        ' Note: The Session_End event is raised only when the sessionstate mode
        ' is set to InProc in the Web.config file. If session mode is set to StateServer 
        ' or SQLServer, the event is not raised.
    End Sub

</script>