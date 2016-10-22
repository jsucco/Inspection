<%@ WebHandler Language="VB" Class="core.TempMangSubgrid_Load" %>

Imports System
Imports System.Web
Imports System.Web.Script.Serialization

Namespace core

    Public Class TempMangSubgrid
        Public Property TemplateId As Integer
        Public Property oper As String
        Public Property TemplateId_Val As Integer
        Public Property Abr As String
        Public Property LocationMaster_id As Integer
        Public Property LiveStatus As Boolean
    End Class

    Public Class TempMangSubgrid_Load : Implements IHttpHandler
        Dim jser As New JavaScriptSerializer
        Dim jsonData As New SubgridjsonData
        Dim objtm As New TempMangSubgrid

        Public Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest
            Dim listdm As New List(Of SPCInspection.TemplateManger_LocSubgrid)
            Dim RequestParams As NameValueCollection = context.Request.Params
            Dim rowsAffected As Integer = 0
            If RequestParams.Count > 0 Then
                Dim bmapsl As New BMappers(Of TempMangSubgrid)

                objtm = bmapsl.GetReqParamAsObject(RequestParams)
                If objtm.TemplateId_Val > 0 Then

                    If objtm.oper = "edit" Then
                        Using _db As New Inspection_Entities
                            Dim tao = (From x In _db.TemplateActivators Where x.LocationMasterId = objtm.LocationMaster_id And x.TemplateId = objtm.TemplateId_Val Select x).FirstOrDefault()
                            tao.Status = objtm.LiveStatus
                            rowsAffected = _db.SaveChanges()
                        End Using
                    Else
                        Using _mang As New AprManager_Entities
                            Dim objsp = (From x In _mang.SP_SPC_GetTemplateLocationStatus(objtm.TemplateId_Val)).ToList()
                            listdm = (From v In objsp Select New SPCInspection.TemplateManger_LocSubgrid With {.LocationMaster_id = v.id, .Abr = v.Abr, .Name = v.Name, .LiveStatus = v.Status}).ToList()
                            rowsAffected = listdm.Count()
                        End Using
                    End If
                End If
            End If

            jsonData.rows = listdm
            jsonData.repeatitems = "false"
            jsonData.cell = ""
            jsonData.id = "LocationMaster_id"
            If rowsAffected > 0 Then
                If objtm.oper <> "edit" Then
                    context.Response.Write(jser.Serialize(jsonData))
                Else
                    context.Response.Write("success")
                End If

            Else
                context.Response.Write("failure")
            End If

        End Sub

        Public ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
            Get
                Return False
            End Get
        End Property

    End Class

End Namespace
