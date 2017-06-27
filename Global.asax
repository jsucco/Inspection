<%@ Application Language="VB" %>
<%@ Import Namespace="System.Web.Optimization" %>
<%@ Import Namespace="System.Web.Routing" %>
<%@ Import Namespace="System.Web.Security" %>
<%@ Import Namespace="System.Security.Principal" %>

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
                "~/Scripts/wijmo/jquery.wijmo-open.all.3.20153.83.min.js",
                "~/Scripts/wijmo/jquery.wijmo-pro.all.3.20153.83.min.js",
                "~/Scripts/wijmo/interop/wijmo.data.ajax.3.20153.83.js",
                "~/Scripts/grid.locale-en.js",
                "~/Scripts/jquery.jqGrid.min.js",
                "~/Scripts/select2/select2.min.js"
))

        'BundleTable.Bundles.Add(New ScriptBundle("~/bundles/InspectionVisualizer").Include(
        '        "~/Scripts/wijmo/jquery.wijmo-open.all.3.20153.83.min.js",
        '        "~/Scripts/wijmo/jquery.wijmo-pro.all.3.20153.83.min.js",
        '        "~/Scripts/wijmo/interop/wijmo.data.ajax.3.20153.83.js",
        '        "~/Scripts/grid.locale-en.js",
        '        "~/Scripts/jquery.jqGrid.min.js",
        '         "~/Scripts/select2/select2.min.js",
        '        "~/Scripts/Gcharts/jsapi.js"
        '       ))

        BundleTable.Bundles.Add(New ScriptBundle("~/bundles/InspectionInput_groupA").Include(
"~/Scripts/jquery-1.11.1.js",
"~/Scripts/jquery-ui.min.js"
))

        BundleTable.Bundles.Add(New ScriptBundle("~/bundles/InspectionInput_groupB").Include(
            "~/Scripts/wijmo/jquery.wijmo-open.all.3.20141.34.min.js",
            "~/Scripts/wijmo/jquery.wijmo-pro.all.3.20141.34.min.js",
            "~/Scripts/wijmo/wijmo.data.ajax.3.20141.34.js",
            "~/Scripts/jquery.layout.js",
            "~/Scripts/grid.locale-en.js",
            "~/Scripts/jquery.jqGrid.js"
        ))

        BundleTable.Bundles.Add(New StyleBundle("~/bundles/InspectionInput_styles").Include(
                "~/Styles/wijmo/jquery-wijmo.css",
                "~/Styles/wijmo/jquery.wijmo-pro.all.3.20141.34.min.css",
                "~/Styles/ui.jqgrid.css",
                "~/Styles/jquery-ui.css",
                "~/Styles/defectEntry_main.css"
        ))

        '    BundleTable.Bundles.Add(New ScriptBundle("~/bundles/FlagBrdMaintenance").Include(
        '"~/Scripts/jquery-1.11.1.js",
        '"~/Scripts/jquery-ui.min.js",
        '"~/Scripts/wijmo/jquery.wijmo-open.all.3.20141.34.FlgBrd.min.cache",
        '"~/Scripts/wijmo/jquery.wijmo-pro.all.3.20141.34.FlgBrd.min.cache",
        '"~/Scripts/wijmo/wijmo.data.ajax.3.20141.34.FlgBrd.cache",
        '"~/Scripts/jquery.jqGrid.js",
        '"~/Scripts/jquery.layout.js",
        '"~/Scripts/grid.locale-en.js",
        '"~/Scripts/select2.js"
        '))

    End Sub

    '    "~/Scripts/wijmo/jquery.wijmo-open.all.3.20141.34.min.js",
    '"~/Scripts/wijmo/jquery.wijmo-pro.all.3.20141.34.min.js",
    '"~/Scripts/wijmo/wijmo.data.ajax.3.20141.34.js",

    Protected Sub Application_AuthenticateRequest(ByVal sender As Object, ByVal e As EventArgs)

        If Request.CurrentExecutionFilePath.StartsWith("/Login.aspx") Then
            Exit Sub
        End If
        Dim debugMode As String = System.Configuration.ConfigurationManager.AppSettings("debugMode")
        If debugMode = "true" Then
            Dim MyIdentity = New GenericIdentity("testUser")
            Dim groups As String() = {"Admin"}
            HttpContext.Current.User = New GenericPrincipal(MyIdentity, groups)
            Return
        End If

        Dim authCookie As HttpCookie = Request.Cookies(FormsAuthentication.FormsCookieName)

        If IsNothing(authCookie) = True Then
            UnAuthorized()
            Return
        End If

        Try
            Dim ticket As FormsAuthenticationTicket = FormsAuthentication.Decrypt(authCookie.Value)

            If IsNothing(ticket) = False Then
                Dim groups As String() = {""}
                HttpContext.Current.User = New GenericPrincipal(New FormsIdentity(ticket), groups)
            Else
                UnAuthorized()
            End If
        Catch ex As Exception

            UnAuthorized()
        End Try
    End Sub

    Protected Sub UnAuthorized()
        Try
            Dim newCookie As New HttpCookie(FormsAuthentication.FormsCookieName, "")
            newCookie.Expires = DateTime.Now.AddYears(-1)
            Response.Cookies.Add(newCookie)
        Catch ex As Exception

        End Try

        Response.Redirect("/Login.aspx?returnUrl=" + HttpContext.Current.Request.Url.AbsoluteUri)
    End Sub

</script>