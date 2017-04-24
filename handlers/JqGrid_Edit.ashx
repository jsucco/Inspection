<%@ WebHandler Language="VB" Class="core.JqGrid_Edit" %>

Imports System
Imports System.Web

Namespace core
    

    Public Class JqGrid_Edit : Implements IHttpHandler, IRequiresSessionState
        Private IU As New InspectionUtilityDAO
        
        Public Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest
            Dim RequestParams As NameValueCollection = context.Request.Params
            Dim TemplateId As Integer
            Dim Status As Boolean
            Dim ErrorFlag As Integer = 0
            
            'If IsNothing(RequestParams.GetValues("CID")) = False Then
            'Dim CID_val = RequestParams.GetValues("CID")(0).ToString().Split("&")(0).Split("=")(1)
            Dim TemplateId_val = CType(RequestParams.GetValues("id")(0), Integer)
            Dim oper As String = RequestParams.GetValues("oper")(0)
            'context.Session("CID") = CID_val

            If IsNumeric(TemplateId_val) = True Then
                Dim TemplateTable As New List(Of SPCInspection.TemplateTable)
                Dim TemplateArray As Array
                Dim bmaptt As New BMappers(Of SPCInspection.TemplateTable)
                
                
                TemplateTable = IU.GetTemplateTable()
               
                TemplateArray = TemplateTable.ToArray()
                TemplateId = CType(TemplateArray(TemplateId_val - 1).TemplateId, Integer)
                If oper = "del" Then
                    If TemplateTable.Count > 0 Then
                        
                        Status = CType(TemplateArray(TemplateId_val - 1).Active, Boolean)
                            
                        Dim resultint As Integer = IU.ToggleStatusTemplateById(TemplateId, Status)
                        If resultint > 0 Then
                            GoTo 101
                                
                        ElseIf resultint < 0 Then
                            ErrorFlag = 2
                            GoTo 101
                        End If
                            
                    End If
                ElseIf oper = "edit" Then
                    Dim Loc_list As New Dictionary(Of String, Boolean)
                    Dim bmap_col As New BMappers(Of InfoSchema)
                    Dim listcol As List(Of InfoSchema) = bmap_col.GetInspectObject("select COLUMN_NAME from INFORMATION_SCHEMA.COLUMNS where TABLE_NAME = N'TemplateName'")
                    If listcol.Count > 0 Then
                        Dim arraycol = listcol.ToArray()
                        For Each item In arraycol
                            Try
                                If item.COLUMN_NAME.Contains("Loc") Then
                                    Dim pageobj As Object = RequestParams.GetValues(item.COLUMN_NAME)(0)
                                    If IsNothing(pageobj) = False Then
                                        Dim boolstrg As String = "0"
                                        If pageobj = "Yes" Then
                                            boolstrg = "1"
                                        ElseIf pageobj = "No" Then
                                            boolstrg = "0"
                                        End If
                                        Dim sql As String = "UPDATE TemplateName SET " & item.COLUMN_NAME.ToString() & " = " & boolstrg & " WHERE (TemplateId = " & TemplateId.ToString() & ")"
                                
                                        bmap_col.InsertSpcObject(sql)
                                    End If
                                End If
                            Catch ex As Exception
                                ErrorFlag = 1
                            End Try
                            
                        Next
                        
                    End If
                    
                End If
            End If
                    
                
101:
                
                'Else
                '    ErrorFlag = 2
                'End If
            
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


