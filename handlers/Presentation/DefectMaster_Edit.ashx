<%@ WebHandler Language="VB" Class="core.JqGrid_Edit" %>

Imports System
Imports System.Web

Namespace core
    

    Public Class JqGrid_Edit : Implements IHttpHandler
        Private IU As New InspectionUtilityDAO
        
        Public Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest
            Dim RequestParams As NameValueCollection = context.Request.Params
            Dim TemplateId As Integer
            Dim Status As Boolean
            Dim DefectId As Object
            Dim ErrorFlag As Integer = 0
            For i = 0 To RequestParams.Count - 1
                If RequestParams.GetKey(i) = "DefectId" Then
                    DefectId = RequestParams.GetValues(i)(0)
                    If IsNumeric(DefectId) = True Then
                        Dim returnint As Integer
                        returnint = IU.GetTemplateTable()
                        If TemplateTable.Count > 0 Then
                            TemplateArray = TemplateTable.ToArray()
                            TemplateId = CType(TemplateArray(rowid - 1).TemplateId, Integer)
                            Status = CType(TemplateArray(rowid - 1).Status, Boolean)
                            
                            Dim resultint As Integer = IU.ToggleStatusTemplateById(TemplateId, Status)
                            If resultint > 0 Then
                                GoTo 101
                                
                            ElseIf resultint < 0 Then
                                ErrorFlag = 2
                                GoTo 101
                            End If
                            
                        End If
                    End If
                    
                End If
                
            Next
            ErrorFlag = 1
101:
            context.Response.ContentType = "text/plain"
            Select Case ErrorFlag
                Case 0
                    context.Response.Write("success")
                Case 1
                    context.Response.Write("notfound")
                Case 2
                    context.Response.Write("servererror")
                Case Else
                    context.Response.Write("error")
            End Select
            
        End Sub
 
        Public ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
            Get
                Return False
            End Get
        End Property

    End Class
    
End Namespace


