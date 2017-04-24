Imports System.Data
Imports System.Web.Script.Serialization

Namespace core

    Public Class APP_Presentation_FlgBrd2_MaintenanceScheduleEditor
        Inherits core.APRWebApp

        Public CorpSelList As String = "[0]"
        Public FBSelList As String = "[0]"
        Public HasCID As Boolean = False
        Public HasFB As Boolean = False
        Public CID As String
        Public CIDnum As Integer
        Public BlankEMPID As Integer
        Public IsMobile As Boolean
        Public IsTablet As Integer
        Public SchId As String
        Public BoardData As String
        Public MFBID As Integer
        Public BoardType As String = "FB"
        Public BoardNumber As Integer = 1
        Public Property MSDAO As New MaintSchedDAO
        Public MFBNAME As String
        Private Util As New Utilities
        Dim corp As New corporate
        Dim listso As New List(Of SingleObject)
        Dim bmapso As New BMappers(Of SingleObject)
        Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
            Dim jser As New JavaScriptSerializer
            Dim fblist As New List(Of selector2array)
            'MFBID = Convert.ToInt32(Session("MFBID"))

            If Page.IsPostBack = False Then
                Dim corplist As New List(Of CID)
                CorpSelList = jser.Serialize(corp.GetFullCorporateList())
                Dim CIDCookie = GetCookie("APRKeepMeIn", "CID_Print")

                If CIDCookie.Count > 0 Then
                    'corplist = Me.Session("CID_Info")

                    'Dim corparray = corplist.ToArray()

                    CID = CIDCookie.Item("APRKeepMeIn")
                    CIDnum = CType(CID, Integer)
                    SetCookie("FlgBrdMobile", "SelectedCID", CIDnum)
                    HasCID = True
                    Dim bmap_fb As New BMappers(Of selector2array)

                    fblist = bmap_fb.GetCtxObject("SELECT MFB_Id AS id, UserID AS text FROM Maintenance_Flagboard", CIDnum)

                    FBSelList = jser.Serialize(fblist)

                ElseIf IsNothing(Request.QueryString("CID")) = False And IsNumeric(Request.QueryString("CID")) = True Then

                    CID = CType(Request.QueryString("CID"), Integer).ToString()
                    CIDnum = Convert.ToInt32(Request.QueryString("CID"))

                    HasCID = True
                    Dim bmap_fb As New BMappers(Of selector2array)

                    fblist = bmap_fb.GetCtxObject("SELECT MFB_Id AS id, UserID AS text FROM Maintenance_Flagboard", CType(CID, Integer))

                    FBSelList = jser.Serialize(fblist)

                Else

                    If IsNothing(Request.QueryString("CID_Info")) = False Then

                        If IsNumeric(Request.QueryString("CID_Info")) = True Then
                            Dim retbol As Boolean = corp.GetDBuser(Request.QueryString("CID_Info"))
                            Me.Session("CID_Info") = corp.cidclass
                            If corp.cidclass.Count > 0 Then
                                CID = corp.cidclass.ToArray()(0).CID.ToString()
                                CIDnum = corp.cidclass.ToArray()(0).CID
                                SetCookie("FlgBrdMobile", "SelectedCID", CIDnum)
                                HasCID = True
                                Dim bmap_fb As New BMappers(Of selector2array)

                                fblist = bmap_fb.GetCtxObject("SELECT MFB_Id AS id, UserID AS text FROM Maintenance_Flagboard", CType(CID, Integer))

                                GoTo 101
                            End If
                        End If

                        'Else
                        '    Dim CIDCookie As Dictionary(Of String, String) = GetCookie("FlgBrdMobile", "SelectedCID")
                        '    If CIDCookie.Count > 0 Then
                        '        CID = CIDCookie("FlgBrdMobile")
                        '        CIDnum = CType(CID, Integer)
                        '        HasCID = True
                        '        Dim bmap_fb As New BMappers(Of selector2array)
                        '        Dim fblist As New List(Of selector2array)

                        '        fblist = bmap_fb.GetCtxObject("SELECT MFB_Id AS id, UserID AS text FROM Maintenance_Flagboard", CType(CID, Integer))
                        '        FBSelList = jser.Serialize(fblist)
                        '    End If
                    End If

                    HasCID = False
101:
                End If

                If fblist.Count > 0 Then
                    fblist.Add(New selector2array With {.id = -1, .text = "*ALL"})
                End If
                FBSelList = jser.Serialize(fblist)
                Dim CookieDict As Dictionary(Of String, String) = GetFBCookie()
                'CookieDict.Clear()  'remove when publishing
                If IsNothing(Request.QueryString("MFBID")) = False Then

                    If IsNumeric(Request.QueryString("MFBID")) = True Then
                        MFBID = CType(Request.QueryString("MFBID"), Integer)
                        BoardNumber = MFBID

                        MaintSchedDAO.MFB_Id = MFBID
                        HasFB = True
                        RegisterFBCookie(MFBID, Request.QueryString("MFBNAME"))
                    Else
                        HasFB = False
                    End If
                ElseIf CookieDict.Count > 0 And HasCID = True Then
                    Dim MFB_IdCookie As Integer = Convert.ToInt32(CookieDict.Item("MFBID"))
                    If CookieDict.ContainsKey("Name") = True Then
                        Dim MFB_Name As String = CookieDict.Item("Name")
                        GetPrefix(MFB_Name)
                        HasFB = True
                    Else
                        HasFB = False
                    End If
                    
                    MaintSchedDAO.MFB_Id = MFB_IdCookie
                    MFBID = MFB_IdCookie
                Else
                    HasFB = False
                End If
                End If
                'If Not MFBID = 0 Then
                '    RegisterFBCookie(MFBID)
                'Else
                '    Dim CookieDict As Dictionary(Of String, String) = GetFBCookie()
                '    If CookieDict.Count > 0 Then
                '        Dim MFB_IdCookie As Integer = Convert.ToInt32(CookieDict.Item("MFBID"))
                '        Session("MFBID") = MFB_IdCookie
                '        MFBID = MFB_IdCookie
                '    Else
                '        Response.Redirect("~/Login.aspx")
                '    End If

                'End If
                IsTablet = 10

                Dim SchIdString As String = Me.Request.QueryString("SchId")

                If Not SchIdString Is Nothing Then
                    SchId = SchIdString
                Else
                    SchId = "2"
                End If
            If MFBID > 0 Then
                listso = bmapso.GetCtxObject("SELECT UserID as Object1 FROM Maintenance_FlagBoard WHERE MFB_Id = " & MFBID.ToString() & "", CType(CID, Integer))
                If listso.Count > 0 Then
                    MFBNAME = listso.ToArray()(0).Object1.ToString.ToUpper()
                    GetPrefix(MFBNAME)
                End If
            End If
                BoardData = MSDAO.GetMobileInputData(SchId, MFBID)
        End Sub

        Private Sub RegisterFBCookie(ByVal MFB_Id As Integer, ByVal Name As String)


            If Not Request.Cookies("MFBInfo_Mobile") Is Nothing Then
                Dim MFBIDCookie As String = Server.HtmlEncode(Request.Cookies("MFBInfo_Mobile")("MFBID")).ToString()

                If MFB_Id.ToString() <> MFBIDCookie Then
                    Response.Cookies("MFBInfo_Mobile")("MFBID") = MFB_Id.ToString()
                    Response.Cookies("MFBInfo")("Name") = Name
                    Response.Cookies("MFBInfo_Mobile")("lastVisit") = DateTime.Now.ToString()
                    Response.Cookies("MFBInfo_Mobile").Expires = DateTime.Now.AddDays(365)
                End If

            Else
                Response.Cookies("MFBInfo_Mobile")("MFBID") = MFB_Id.ToString()
                Response.Cookies("MFBInfo")("Name") = Name
                Response.Cookies("MFBInfo_Mobile")("lastVisit") = DateTime.Now.ToString()
                Response.Cookies("MFBInfo_Mobile").Expires = DateTime.Now.AddDays(365)
            End If




        End Sub

        Private Function GetFBCookie() As Dictionary(Of String, String)
            Dim dictionary As New Dictionary(Of String, String)

            If Not Request.Cookies("MFBInfo_Mobile") Is Nothing Then
                Dim MFBIDCookie As String = Server.HtmlEncode(Request.Cookies("MFBInfo_Mobile")("MFBID")).ToString()
                Dim lastVisitCookie As String = Server.HtmlEncode(Request.Cookies("MFBInfo_Mobile")("lastVisit")).ToString()
                dictionary.Add("MFBID", MFBIDCookie)
                dictionary.Add("lastVisit", lastVisitCookie)
            End If

            Return dictionary
        End Function

        Private Sub GetPrefix(ByVal SelectedName As String)

            If SelectedName.Length >= 3 Then
                BoardType = SelectedName.Substring(0, 2).ToUpper()
                Dim BoardNumString As String = SelectedName.Substring(2, 1)
                If IsNumeric(BoardNumString) = True Then
                    BoardNumber = CType(BoardNumString, Integer)
                End If
            End If

        End Sub

        Public Function GetBlankEMPID() As Integer
            Dim sqlstring As String
            Dim EMPNameSet As DataSet = New DataSet
            sqlstring = "select EMP_ID from dbo.Employees where EMP_Last_Name = '.' and EMP_First_Name = '.'"

            If Not Util.FillDataSet(EMPNameSet, "EMPNameSet", sqlstring) Then
                Return False
            End If

            Return EMPNameSet.Tables(0).Rows(0)("EMP_ID")

        End Function
    End Class


End Namespace
