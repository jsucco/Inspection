<%@ WebHandler Language="VB" Class="core.DefectMasterSubgrid_Load" %>

Imports System
Imports System.Web
Imports System.Web.Script.Serialization


Namespace core

    Public Class DefectMasterSubgrid
        Public Property ijsid As Integer
        Public Property SessionId As String
    End Class


    Public Class DefectMasterSubgrid_Load : Implements IHttpHandler, IRequiresSessionState
        Dim jser As New JavaScriptSerializer
        Dim jsonData As New SubgridjsonData
        Dim IU As New InspectionUtilityDAO
        Dim objdm As New DefectMasterSubgrid
        Public Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest

            Dim listdm As New List(Of SPCInspection.DefectMasterSubgrid)
            Dim RequestParams As NameValueCollection = context.Request.Params

            If RequestParams.Count > 0 Then
                Dim bmapsl As New BMappers(Of DefectMasterSubgrid)

                objdm = bmapsl.GetReqParamAsObject(RequestParams)
                If objdm.ijsid > 0 Then
                    listdm = IU.GetDefectMasterById(objdm.ijsid)
                    If listdm.Count > 0 Then
                        HttpRuntime.Cache.Insert("ijsSubGrid_Active." + objdm.SessionId, listdm, Nothing, Date.Now.AddMinutes(10), System.Web.Caching.Cache.NoSlidingExpiration)
                    End If
                End If

            End If

            jsonData.rows = listdm
            jsonData.repeatitems = "false"
            jsonData.cell = ""
            jsonData.id = "DefectID"
            context.Response.Write(jser.Serialize(jsonData))
        End Sub

        Public ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
            Get
                Return False
            End Get
        End Property

    End Class

End Namespace
