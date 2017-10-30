Imports System.Web.Script.Serialization
Imports System.DirectoryServices.AccountManagement
Imports System.Web.Configuration

Namespace core

    Partial Class APP_Menu_APR_SiteEntry
        Inherits core.APRWebApp

        Public CID As String
        Public CtxCID As String
        Public LocationObject As String
        Public UserID As String
        Public NavPerms As String = "[0]"
        Public Authicated As Boolean = False
        Public CookieUserData As String = ""
        Public MenuUrl As String = ""
        Public CtxUrl As String = ""
        Dim UDAO As New userDAO
        Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
            Dim UserParse As Object = Request.UserAgent
            Dim jser As New JavaScriptSerializer

            CID = Request.QueryString("CID")
            Try
                If Not CID Is Nothing Then
                    CtxCID = GetLocationObjectInfoString(CID)
                End If

                Dim authCookie = Request.Cookies(FormsAuthentication.FormsCookieName)
                Dim queryquth As String = Request.QueryString("zrt")
                If IsNothing(authCookie) = False Then
                    Dim ticket As FormsAuthenticationTicket = FormsAuthentication.Decrypt(authCookie.Value)
                    If ticket.UserData = "APRSTCAUTHCOOKIE" Then
                        Authenticated_hidden.Value = "True"
                        SetCookie("APR_UserActivityLog", "PrimaryKey", UDAO.LogUserActivity(ticket.Name, "WebApp_MENU", GetUserCID()))
                    End If
                    CookieUserData = ticket.UserData
                End If
                If IsNothing(queryquth) = False And cypher.HashQueryString("aprtrue") = queryquth Then
                    Authenticated_hidden.Value = "True"
                End If
                If DecryptAprKey() Then
                    Dim IsAuth = True
                End If

                MenuUrl = GetMenuUrl()
            Catch ex As Exception
                Response.Redirect("~/Errorpage.aspx?UC=" + ex.Message)
            End Try
        End Sub

        Private Function GetMenuUrl() As String
            Return "http://aprmenu.standardtextile.com"
        End Function

        Private Function GetUserCID() As String
            Dim CIDCookie As HttpCookie = Request.Cookies("APRKeepMeIn")
            Dim CID As String = "000000"
            If IsNothing(CIDCookie) = False Then
                If CIDCookie.HasKeys = True Then
                    Try
                        CID = CIDCookie("CID_Print")
                    Catch ex As Exception
                        Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
                    End Try
                End If
            End If
            Return CID
        End Function
        Sub OnAuthenticate(ByVal sender As Object, ByVal e As AuthenticateEventArgs)
            Dim Authenticated As Boolean
            Using pc As New PrincipalContext(ContextType.Domain, "standardtextile.com", Nothing, ContextOptions.SimpleBind)
                Dim username = Login1.UserName
                Dim usersplit = username.Trim().Split("\")
                Dim usernamefrm As String
                Dim testobj As Object
                If usersplit.Length < 2 Then
                    usernamefrm = "textile\" + username
                Else
                    usernamefrm = username
                End If

                If pc.ValidateCredentials(usernamefrm, Login1.Password, ContextOptions.SimpleBind) Then
                    e.Authenticated = Authenticated

                    Dim user As UserPrincipal = UserPrincipal.FindByIdentity(pc, username)

                    If IsNothing(user) = False Then

                        Try

                        Catch ex As Exception

                        End Try
                        
                    End If

                    SetCookie("APR_UserActivityLog", "PrimaryKey", UDAO.LogUserActivity(usernamefrm, "WebApp_STC_ActiveDirectory"))
                    SetCookie("APR_Username", "Username", usernamefrm)
                    AddAuthCookie(username)
                    '  AddUserCookie(usernamefrm)
                ElseIf IsNothing(Me.Session("CID_Info")) = False Then
                    Dim CIDobj As List(Of core.CID) = Me.Session("CID_Info")

                    If UDAO.AutenticateUser(username, Login1.Password.Trim(), CIDobj.ToArray()(0).CID) = True Then
                        SetCookie("APR_UserActivityLog", "PrimaryKey", UDAO.LogUserActivity(username, "WebApp_CTX", CIDobj.ToArray()(0).CID))
                        SetCookie("APR_Username", "Username", usernamefrm)
                        AddAuthCookie(username)
                        'AddUserCookie(username)
                    End If
                End If

            End Using


        End Sub

        Private Sub AddUserCookie(UserName As String)
            Dim cyp As New cypher
            Dim expires As DateTime = DateTime.Now.AddDays(14)
            Dim userCookie As HttpCookie = New HttpCookie(
                                           "UID",
                                           cypher.MD5Encrypt(UserName, WebConfigurationManager.AppSettings("Key64")))

            userCookie.Expires = expires

            Response.Cookies.Add(userCookie)

        End Sub

        Private Sub AddAuthCookie(ByVal username As String)
            FormsAuthentication.Initialize()
            Dim expires As DateTime = DateTime.Now.AddDays(365)
            Dim ticket As FormsAuthenticationTicket = New FormsAuthenticationTicket(1,
                            username,
                            DateTime.Now,
                            expires,
                            True,
                            "APRSTCAUTHCOOKIE",
                            FormsAuthentication.FormsCookiePath)
            Dim encryptedTicket = FormsAuthentication.Encrypt(ticket)

            Dim authCookie As HttpCookie = New HttpCookie(
                                           FormsAuthentication.FormsCookieName,
                                           encryptedTicket)
            authCookie.Expires = expires

            Response.Cookies.Add(authCookie)

            Authenticated_hidden.Value = "True"
        End Sub

        Private Function DecryptAprKey() As Boolean
            Dim AprCookie = Request.Cookies("APRKeepMeIn")
            Dim IsAuth As Boolean = False
            If Not AprCookie Is Nothing Then
                If Not AprCookie("MachineKey") Is Nothing Then
                    Dim Cyper As New cypher
                    Dim encryptKey = System.Web.Configuration.WebConfigurationManager.AppSettings("key64")
                    Dim aprkey = System.Web.Configuration.WebConfigurationManager.AppSettings("AprKey")
                    Dim key = Cyper.MD5Decrypt(AprCookie("MachineKey"), encryptKey)
                    If key = aprkey Then
                        IsAuth = True
                    End If
                End If
            End If
            Return IsAuth
        End Function

        Private Function GetLocationObjectInfoString(ByVal CID As String) As String
            Dim jser As New JavaScriptSerializer
            Dim CID_ As String = ""
            If CID.Trim().Length = 0 Then
                Return ""
            End If

            CID_ = getCachedLocationObject(CID)

            If CID_.Length = 6 Then
                Return CID_
            End If

            Using mangContext As New AprManager_Entities
                Dim LocationObject_ As LocationMaster = (From x In mangContext.LocationMasters Where x.CID = CID.Trim Select x).FirstOrDefault()

                If Not LocationObject_ Is Nothing Then
                    If Not LocationObject_.CtxCID Is Nothing Then
                        If LocationObject_.CtxCID.Trim().Length = 6 Then
                            CID_ = LocationObject_.CtxCID.Trim()

                            HttpContext.Current.Cache.Insert("CtxCID:" + CID.Trim(), CID_, Nothing, DateTime.Now.AddDays(21), Cache.NoSlidingExpiration)
                        End If
                    End If
                Else
                    Throw New Exception("reference location Id was not found.  Please Use an enabled valid location id.")
                End If
            End Using

            Return CID_
        End Function

        Private Function getCachedLocationObject(ByVal CID As String) As String
            If CID.Trim().Length = 0 Then
                Return ""
            End If

            Dim cachedval = HttpContext.Current.Cache("CtxCID:" + CID.Trim())

            If Not cachedval Is Nothing Then
                If cachedval.ToString().Trim().Length = 6 Then
                    Return cachedval.ToString().Trim()
                End If
            End If

            Return ""
        End Function
    End Class

End Namespace
