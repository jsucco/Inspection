Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Web
Imports System.Configuration
Imports System.Data.SqlClient

Namespace core

    Public Class CorporateStrings


        'Public Shared Crconnection As String
        'Public Ctxconnection As String
        'Public Shared APRTRNconnection As String
    End Class

    Public Class dlayer
        Inherits core.APRWebApp

        Public Shared customerid As Integer
        Public Shared cidclass As New CID
        Private Default_CID As Integer = 597
        Public Ctxconnection As String
        Public Spcconnection As String
        Private APRTRNconnection As String
        Private Crconnection As String
        '  Private APRWeb As New APRWebApp
        Public CID As Integer

        Public Sub SetSessionVariable(ByVal Variablename As String, ByVal value As String)

            Me.Session("SPCInspection_Bad") = value
        End Sub

        Public Function GetSessionCID() As Integer
            Dim CID As Integer
            CID = Convert.ToInt32(Me.Session("CID"))
            If CID > 0 Then
                Return CID
            Else
                Dim CID_Print As Dictionary(Of String, String) = GetCookie("APRKeepMeIn", "CID_Print")

                If CID_Print.Count > 0 Then
                    If IsNumeric(CID_Print("APRKeepMeIn")) = True Then
                        CID = Convert.ToInt32(CID_Print("APRKeepMeIn"))
                        Dim corp As corporate = New corporate
                        If corp.GetDBuser(CID_Print("APRKeepMeIn")) = True Then
                            Session("CID_Info") = corp.cidclass
                            Me.Session("CID") = CID_Print("APRKeepMeIn")
                        Else
                            Response.Redirect("~/APP/Menu/CorporateSelection.aspx")
                        End If
                        Return CID
                    Else
                        Response.Redirect("~/APP/Menu/CorporateSelection.aspx")
                    End If
                Else
                    Response.Redirect("~/APP/Menu/CorporateSelection.aspx")
                End If
            End If
        End Function

        Public Function SetCtxConnectionString(ByVal CID As Integer) As String

            Ctxconnection = GetCtxConnectionString_("000" + CID.ToString())
            'Dim constrg As String = ConfigurationManager.ConnectionStrings(Ctxconnection).ConnectionString
            Dim sb As New SqlConnectionStringBuilder(Ctxconnection)

            If String.IsNullOrWhiteSpace(sb.ApplicationName) Then
                sb.ApplicationName = ApplicationName
            Else
                sb.ApplicationName = "APR"
            End If

            If ConnectionTimeout > 0 Then
                sb.ConnectTimeout = ConnectionTimeout
            Else
                sb.ConnectTimeout = ConnectionTimeout
            End If

            Return sb.ToString

        End Function
        Public Function InspectConnectionString() As String

            'GetCtxConnectionString()

            'Dim constrg As String = ConfigurationManager.ConnectionStrings(Spcconnection).ConnectionString
            Dim constrg As String = ConfigurationManager.ConnectionStrings("Inspectionconnectionstring").ConnectionString
            Dim sb As New SqlConnectionStringBuilder(constrg)

            If String.IsNullOrWhiteSpace(sb.ApplicationName) Then
                sb.ApplicationName = ApplicationName
            Else
                sb.ApplicationName = "SPC"
            End If

            sb.ConnectTimeout = 25

            Return sb.ToString

        End Function


        Public Function SPCConnectionString() As String

            'GetCtxConnectionString()

            'Dim constrg As String = ConfigurationManager.ConnectionStrings(Spcconnection).ConnectionString
            Dim constrg As String = ConfigurationManager.ConnectionStrings("SpcSTCMAINconnectionstring").ConnectionString
            Dim sb As New SqlConnectionStringBuilder(constrg)

            If String.IsNullOrWhiteSpace(sb.ApplicationName) Then
                sb.ApplicationName = ApplicationName
            Else
                sb.ApplicationName = "SPC"
            End If

            sb.ConnectTimeout = 25

            Return sb.ToString

        End Function

        Public Function PDMConnectionString() As String

            'GetCtxConnectionString()

            'Dim constrg As String = ConfigurationManager.ConnectionStrings(Spcconnection).ConnectionString
            Dim constrg As String = ConfigurationManager.ConnectionStrings("SpcPDMconnectionstring").ConnectionString
            Dim sb As New SqlConnectionStringBuilder(constrg)

            If String.IsNullOrWhiteSpace(sb.ApplicationName) Then
                sb.ApplicationName = ApplicationName
            Else
                sb.ApplicationName = "PDM"
            End If

            sb.ConnectTimeout = 25

            Return sb.ToString

        End Function

        Public Function AprManagerConnectionString() As String

            'GetCtxConnectionString()

            'Dim constrg As String = ConfigurationManager.ConnectionStrings(Spcconnection).ConnectionString
            Dim constrg As String = ConfigurationManager.ConnectionStrings("AprManager_TRNXstring").ConnectionString
            Dim sb As New SqlConnectionStringBuilder(constrg)

            If String.IsNullOrWhiteSpace(sb.ApplicationName) Then
                sb.ApplicationName = ApplicationName
            Else
                sb.ApplicationName = "SPC"
            End If

            sb.ConnectTimeout = 25

            Return sb.ToString

        End Function

        Public Function APRConnectionString(ByVal Database As Integer) As String

            GetTRN_APRSTTString(Database)

            Dim constrg As String = ConfigurationManager.ConnectionStrings(APRTRNconnection).ConnectionString
            Dim sb As New SqlConnectionStringBuilder(constrg)

            If String.IsNullOrWhiteSpace(sb.ApplicationName) Then
                sb.ApplicationName = ApplicationName
            Else
                sb.ApplicationName = "APR"
            End If

            If ConnectionTimeout > 0 Then
                sb.ConnectTimeout = ConnectionTimeout
            Else
                sb.ConnectTimeout = ConnectionTimeout
            End If

            Return sb.ToString


        End Function

        Public Function GetCtxConnectionString_(ByVal CID As String) As String
            If CID.Length <> 6 Then
                Throw New Exception("Invalid location variable.")
            End If
            Dim bmap As New BMappers(Of SingleObject)

            Dim sql As String = "SELECT DBName AS Object1 FROM Corporate WHERE CID = '" + CID + "'"
            Dim result = bmap.GetCtxMangObject(sql)

            If result.Count = 0 Then
                Return ""
            End If
            Return "Data Source=10.5.4.10;Initial Catalog=" + result.ToArray()(0).Object1.ToString() + ";Persist Security Info=True;User ID=sa;Password=textyler"
        End Function

        Public Sub GetTRN_APRSTTString(ByVal Database As Integer)

            Select Case Database
                Case Is = 1
                    APRTRNconnection = "AprSTT_TRNXstring"
                Case Is = 2
                    APRTRNconnection = "AprManager_TRNXstring"
                Case Is = 3
                    APRTRNconnection = "SpcInspection_TRNXstring"
                Case Else
                    APRTRNconnection = "AprSTT_TRNXstring"
            End Select


        End Sub


        Public Function CrConnectionString() As String


            GetCrConnectionString()

            Dim constrg As String = ConfigurationManager.ConnectionStrings(Crconnection).ConnectionString
            Dim sb As New SqlConnectionStringBuilder(constrg)

            If String.IsNullOrWhiteSpace(sb.ApplicationName) Then
                sb.ApplicationName = "CORE"
            Else
                sb.ApplicationName = "CORE"
            End If

            If ConnectionTimeout > 0 Then
                sb.ConnectTimeout = ConnectionTimeout
            Else
                sb.ConnectTimeout = ConnectionTimeout
            End If

            Return sb.ToString

        End Function

        Public Sub GetCrConnectionString()
            Dim CID As Integer
            Dim SessionCID As Integer = Convert.ToInt32(Me.Session("CID"))
            If SessionCID = 0 Then
                SessionCID = Default_CID
            Else
                CID = SessionCID
            End If

            Select Case CID
                Case Is = 577
                    Crconnection = "CrSTCconnectionstring"
                Case Is = 587
                    Crconnection = "AprCARconnectionstring"
                Case Is = 597
                    Crconnection = "CrSTCconnectionstring"
                Case Else
                    Crconnection = "CrSTCconnectionstring"
            End Select



        End Sub

        Public Function MachineConnectionString() As String

            Dim constrg As String = ConfigurationManager.ConnectionStrings("ctxdevconnectionstring").ConnectionString
            Dim sb As New SqlConnectionStringBuilder(constrg)

            If String.IsNullOrWhiteSpace(sb.ApplicationName) Then
                sb.ApplicationName = ApplicationName
            Else
                sb.ApplicationName = "CORE"
            End If

            If ConnectionTimeout > 0 Then
                sb.ConnectTimeout = ConnectionTimeout
            Else
                sb.ConnectTimeout = ConnectionTimeout
            End If

            Return sb.ToString

        End Function

        Public Property ConnectionTimeout As Int32

        '' summary
        '' Property Used to override the name of the application

        Public Property ApplicationName As String = "COREDemo"



        Public Function COREDemoConnectionString() As String

            Dim constrg As String = ConfigurationManager.ConnectionStrings("COREDemoConnectionString").ConnectionString
            Dim sb As New SqlConnectionStringBuilder(constrg)

            If String.IsNullOrWhiteSpace(sb.ApplicationName) Then
                sb.ApplicationName = ApplicationName
            Else
                sb.ApplicationName = "ChartMaster"
            End If

            If ConnectionTimeout > 0 Then
                sb.ConnectTimeout = ConnectionTimeout
            Else
                sb.ConnectTimeout = ConnectionTimeout
            End If

            Return sb.ToString

        End Function

        Public Function ctxmangConnectionString() As String

            Dim constrg As String = ConfigurationManager.ConnectionStrings("ctxmangconnectionstring").ConnectionString
            Dim sb As New SqlConnectionStringBuilder(constrg)

            If String.IsNullOrWhiteSpace(sb.ApplicationName) Then
                sb.ApplicationName = ApplicationName
            Else
                sb.ApplicationName = "CORELogin"
            End If

            If ConnectionTimeout > 0 Then
                sb.ConnectTimeout = ConnectionTimeout
            Else
                sb.ConnectTimeout = ConnectionTimeout
            End If

            Return sb.ToString

        End Function

        Public Function CtxSTCCOREConnectionString() As String

            Dim constrg As String = ConfigurationManager.ConnectionStrings("CtxSTCCOREconnectionstring").ConnectionString
            Dim sb As New SqlConnectionStringBuilder(constrg)

            If String.IsNullOrWhiteSpace(sb.ApplicationName) Then
                sb.ApplicationName = ApplicationName
            Else
                sb.ApplicationName = "CORELogin"
            End If

            If ConnectionTimeout > 0 Then
                sb.ConnectTimeout = ConnectionTimeout
            Else
                sb.ConnectTimeout = ConnectionTimeout
            End If

            Return sb.ToString

        End Function

        Public Function GetCIDInfo() As List(Of CID)
            If IsNothing(Me.Session("CID_Info")) = False Then
                Dim CIDList As List(Of CID)
                CIDList = Me.Session("CID_Info")
                If CIDList.Count > 0 Then
                    Return CIDList
                Else
                    Return Nothing
                End If
            Else
                Return Nothing
            End If

        End Function

        Public Sub InsertCacheObject(ByVal Cachestring As String, ByVal Cachobject As Object, ByVal DaysToExpire As Integer)
            If IsNothing(Cachobject) = False And Cachestring <> "" Then
                If DaysToExpire = 0 Then
                    DaysToExpire = 1
                End If
                HttpContext.Current.Cache.Insert(Cachestring, Cachobject, Nothing, Now.AddDays(DaysToExpire), System.Web.Caching.Cache.NoSlidingExpiration)
            End If
        End Sub



    End Class


End Namespace


