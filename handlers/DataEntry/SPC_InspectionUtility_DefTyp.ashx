<%@ WebHandler Language="VB" Class="core.SPC_InspectionUtility_DefTyp" %>

Imports System
Imports System.Web
Namespace core
    
    Public Class SPC_InspectionUtility_DefTyp : Implements IHttpHandler, IRequiresSessionState
        Dim IU As New InspectionUtilityDAO
        Dim ErrorMessage As String
        Private Property bmap As New BMappers(Of SPCInspection.ButtonLibrarygrid)
        Public Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest
            Dim RequestParams As NameValueCollection = context.Request.Params
            Dim ButLib As New SPCInspection.ButtonLibrarygrid
            Dim ReqCnt As Integer = 0
            Dim RequiredParameters As Boolean = True
            Dim result As Boolean
            Dim TemplateCache As Integer
            Dim bmap_so As New BMappers(Of SingleObject)
            context.Response.ContentType = "text/plain"
            If RequestParams.Count > 0 Then
                For i = 0 To RequestParams.Count - 1
                    Dim Reqkey = RequestParams.GetKey(i).ToString()
                    Dim Reqval As Object = RequestParams.GetValues(i)(0)
                    Select Case Reqkey
                        Case "ButtonId"
                            If Reqval = "" Then
                                ButLib.ButtonId = 0
                            Else
                                ButLib.ButtonId = Convert.ToInt64(Reqval)
                            End If
                            ReqCnt += 1
                        Case "DefectCode"
                            ButLib.DefectCode = Convert.ToString(Reqval)
                            ReqCnt += 1
                        Case "Name"
                            ButLib.Name = Convert.ToString(Reqval)
                            ReqCnt += 1
                        Case "Hide"
                            If Reqval = "false" Then
                                ButLib.Hide = False
                                ReqCnt += 1
                            Else
                                ButLib.Hide = True
                                ReqCnt += 1
                            End If
                        Case "TemplateCollCacheString"
                            If IsNumeric(Reqval) = True Then
                                If Convert.ToInt32(Reqval) > 0 Then
                                    TemplateCache = Convert.ToInt32(Reqval)
                                End If
                            End If
                    End Select
                Next
                If ReqCnt = 4 Then
                    If ButLib.ButtonId = 0 Then
                        
                        Dim countstirng As String = "select count(*) as Object1 from ButtonLibrary where Name = '" & ButLib.Name & "'"
                        
                        Dim countres As List(Of SingleObject) = bmap_so.GetInspectObject(countstirng)
                        If countres.Count > 0 Then
                            Dim namecount = Convert.ToInt16(countres.ToArray()(0).Object1)
                            If namecount = 0 Then
                        
                                Dim insertstring As String = "INSERT INTO ButtonLibrary" & vbCrLf &
                                                                "(Name, DefectCode, Hide)" & vbCrLf &
                                                                "VALUES (@Name,@DefectCode,@Hide)"
                        
                                result = bmap.InsertSpcObject(insertstring, ButLib)
                            End If
                        End If
                    Else
                        result = IU.UpdateButtonLibrary(ButLib)
                    End If
                    If result = True Then
                        context.Response.Write("success")
                        Dim templateid As String = HttpContext.Current.Session("TemplateCollCacheString")
                        If templateid <> "" Then
                            If IsNumeric(templateid) = True Then
                                IU.UpdateTemplateCollectionCache(Convert.ToInt32(templateid))
                            End If
                        End If
                    Else
                        context.Response.Write("failure")
                    End If
                Else
                    context.Response.Write("All Required Parameters not Sent")
                End If
                context.Response.Write("All Required Parameters not Sent")
            End If
        End Sub
 
        Public ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
            Get
                Return False
            End Get
        End Property

    End Class
End Namespace