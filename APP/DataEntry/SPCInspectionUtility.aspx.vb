Imports System.Web.Script.Serialization
Imports System.Data

Namespace core

    Partial Class APP_DataEntry_SPCInspectionUtility
        Inherits core.APRWebApp

        Private Util As New Utilities
        Public IU As New InspectionUtilityDAO
        Public II As New InspectionInputDAO
        Public PostedTemplateId As Integer = 0
        Public PostedName As String = "TitleError"
        Public DefectCodeList As String = "[0]"
        Public HasAuthorization As Boolean = False
        Public ProductSpecifications As String
        Dim ProductSpecscache As New List(Of SPCInspection.ProductSpecs)
        Dim jser As New JavaScriptSerializer
        Public InspectionTypesArray As String

        Public Sub page_load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            Dim jser As New JavaScriptSerializer
            jser.MaxJsonLength = Int32.MaxValue

            If Page.IsPostBack = False Then
                If IsNothing(Request.QueryString("prTid")) = False And IsNumeric(Request.QueryString("prTid")) = True Then
                    PostedTemplateId = Convert.ToInt32(Request.QueryString("prTid"))
                    Dim TempData As New DataSet
                    Dim sql As String = "SELECT Name" & vbCrLf &
                    "FROM TemplateName" & vbCrLf &
                    "WHERE (TemplateId = " + PostedTemplateId.ToString() + ")"

                    If Util.FillSPCDataSet(TempData, "TempData", sql) Then
                        If TempData.Tables(0).Rows.Count > 0 Then
                            PostedName = TempData.Tables(0).Rows(0)("Name")
                        End If
                    End If
                End If
            End If

            InspectionTypesArray = jser.Serialize(II.GetInspectionTypes())
            Dim authCookie = Request.Cookies(FormsAuthentication.FormsCookieName)
            If IsNothing(authCookie) = False Then
                Dim ticket As FormsAuthenticationTicket = FormsAuthentication.Decrypt(authCookie.Value)
                If ticket.Name <> "" Then
                    Dim bmap_so As New BMappers(Of core.SingleObject)
                    Dim list_so As New List(Of core.SingleObject)
                    Dim sql As String = "SELECT Address AS Object1 FROM EmailMaster WHERE ADMIN = 1"
                    list_so = bmap_so.GetAprMangObject(sql)
                    If list_so.Count > 0 Then
                        Dim soarray = list_so.ToArray()
                        Dim TicketUserName = FormatUserName(ticket.Name)
                        For Each item In soarray
                            Dim splitar = item.Object1.ToString().Split("@")
                            If splitar.Count > 1 Then
                                If TicketUserName = splitar(0) Then
                                    HasAuthorization = True
                                End If
                            End If
                        Next
                    End If
                End If
            End If

            Dim buttonlib As List(Of SPCInspection.ButtonLibrarygrid) = IU.GetLibraryGrid()
            If buttonlib.Count > 0 Then
                DefectCodeList = jser.Serialize(buttonlib)
            End If

            If HasAuthorization = False Then
                Response.Redirect("~/APP/APR_SiteEntry.aspx")
            End If
        End Sub

        Private Function FormatUserName(ByVal username As String) As String

            Dim userFormed As String = ""
            Dim userSplit As String()
            If IsNothing(username) = False Then
                userSplit = username.Split("\").ToArray()
                If userSplit.Count = 1 Then
                    userFormed = username
                ElseIf userSplit.Count > 1 Then
                    userFormed = userSplit(1)
                End If
            End If
            Return userFormed
        End Function

        Protected Sub TemplateSubmit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles TemplateSubmit.Click
            If Page.IsPostBack = True Then

                Dim TabArrayString As String = TabArray_Hidden.Value
                Dim ButtonArrayStirng As String = ButtonArray_Hidden.Value
                Dim TemplateId As Integer = Convert.ToInt64(TemplateId_Hidden.Value.Trim())
                Dim ColumnCount As Integer = Convert.ToInt32(ColumnCount_Hidden.Value)
                Dim LineType As String = LineType_Hidden.Value
                Dim jser As New JavaScriptSerializer
                Dim returnmessage As String = "Nothing"

                If TabArrayString <> "0" And TemplateId <> 0 Then

                    Dim TabArrayDe = jser.Deserialize(Of List(Of SPCInspection.tabarray))(TabArrayString)
                    Dim ButtonArrayDe = jser.Deserialize(Of List(Of SPCInspection.buttonarray))(ButtonArrayStirng)
                    Dim TabArray As Array = TabArrayDe.ToArray()
                    Dim ButtonArray As Array = ButtonArrayDe.ToArray()

                    Dim TempIdSet As DataSet = New DataSet

                    Dim sqlstring As String = "SELECT COUNT(TemplateId) AS Count FROM TabTemplate WHERE (TemplateId = " & TemplateId.ToString() & ")"

                    If Util.FillSPCDataSet(TempIdSet, "TempIdSet", sqlstring) Then
                        If TempIdSet.Tables(0).Rows(0)("Count") = 0 Then
                            Dim result As Boolean = IU.TemplateInsert(TemplateId, TabArrayDe, ButtonArrayDe)

                            If result = True Then
                                IU.UpdateTemplateCollectionCache(TemplateId)
                                returnmessage = "Template Inserted"
                            Else
                                returnmessage = "Failed"
                            End If
                        Else
                            Dim result As Boolean = IU.TemplateUpdate(TemplateId, TabArrayDe, ButtonArrayDe)
                            If result = True Then
                                IU.UpdateTemplateCollectionCache(TemplateId)
                                returnmessage = "Template Updated"
                            Else
                                returnmessage = "Failed"
                            End If
                        End If
                        IU.SetLineType(TemplateId, LineType, ColumnCount)
                    Else
                        returnmessage = "Failed"
                    End If

                End If

                If returnmessage <> "Nothing" Then
                    Dim cs As ClientScriptManager = Page.ClientScript
                    Dim cstype As Type = Me.GetType()
                    Dim cstext1 As String = "alert('" + returnmessage + "');"
                    cs.RegisterStartupScript(cstype, "PopUpScript", cstext1, True)
                End If

            End If

        End Sub

        Private Sub LoadProductSpecs()
            Dim sqlstr As String = "SELECT SpecId, TabTemplateId, DataNo, ProductType, Spec_Description, HowTo, value, Upper_Spec_Value, Lower_Spec_Value" & vbCrLf &
                            "FROM  ProductSpecification"
            Dim bmapps As New BMappers(Of SPCInspection.ProductSpecs)
            ProductSpecscache = bmapps.GetInspectObject(sqlstr)
            Dim expired As DateTime = Now.AddMinutes(60)

            If ProductSpecscache.Count > 0 Then
                Context.Cache.Insert("ProductSpecs", ProductSpecscache, Nothing, expired, System.Web.Caching.Cache.NoSlidingExpiration)
                InspectionUtilityDAO.ProductSpecsLastCachedDT = Date.Now
            End If




        End Sub
    End Class

End Namespace

