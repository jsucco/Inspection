<%@ WebHandler Language="VB" Class="core.MainGrid_SubgridLoad" %>


Imports System.Web.Script.Serialization


Namespace core

    Public Class MainGridSubgrid
        Public Property Facility As String
        Public Property Time_Period As String
        Public Property From_date As String
        Public Property To_date As String
        Public Property DataNo As String
        Public Property WorkOrder As String
        Public Property AuditType As String
        Public Property SessionId As String
    End Class


    Public Class MainGrid_SubgridLoad : Implements IHttpHandler, IRequiresSessionState
        Dim jser As New JavaScriptSerializer
        Dim jsonData As New SubgridjsonData
        Dim IU As New InspectionUtilityDAO
        Dim objdm As New MainGridSubgrid
        Public Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest

            Dim listdm As New List(Of SPCInspection.MainGridSubgrid)
            Dim RequestParams As NameValueCollection = context.Request.Params

            If RequestParams.Count > 0 Then
                Dim bmapsl As New BMappers(Of MainGridSubgrid)

                objdm = bmapsl.GetReqParamAsObject(RequestParams)
                If objdm.Facility <> Nothing Then
                    listdm = IU.GetMainGridSubGrid(objdm.Facility, objdm.Time_Period, objdm.From_date, objdm.To_date, objdm.DataNo, objdm.WorkOrder, objdm.AuditType)
                    If listdm.Count > 0 Then
                        HttpRuntime.Cache.Insert("MainGridSubGrid_Active." + objdm.SessionId, listdm, Nothing, Date.Now.AddMinutes(10), System.Web.Caching.Cache.NoSlidingExpiration)
                    End If
                End If

            End If

            jsonData.rows = listdm
            jsonData.repeatitems = "false"
            jsonData.cell = ""
            jsonData.id = "WorkRoom"
            context.Response.Write(jser.Serialize(jsonData))
        End Sub

        Public ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
            Get
                Return False
            End Get
        End Property

    End Class

End Namespace