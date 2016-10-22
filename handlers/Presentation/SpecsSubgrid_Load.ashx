<%@ WebHandler Language="VB" Class="core.SpecsSubgrid_Load" %>

Imports System
Imports System.Web
Imports System.Web.Script.Serialization


Namespace core
    
    Public Class SpecsSubgrid
        Public Property ijsid As Integer

    End Class
    

    Public Class SpecsSubgrid_Load : Implements IHttpHandler, IRequiresSessionState
        Dim jser As New JavaScriptSerializer
        Dim jsonData As New SubgridjsonData
        Dim IU As New InspectionUtilityDAO
        Dim objdm As New SpecsSubgrid
        Public Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest
            
            Dim listdm As New List(Of SPCInspection.SpecsSubgrid)
            Dim RequestParams As NameValueCollection = context.Request.Params
            
            If RequestParams.Count > 0 Then
                Dim bmapsl As New BMappers(Of SpecsSubgrid)
                
                objdm = bmapsl.GetReqParamAsObject(RequestParams)
                
                listdm = IU.GetSpecsById(objdm.ijsid)
            End If
            
            jsonData.rows = listdm
            jsonData.repeatitems = "false"
            jsonData.cell = ""
            jsonData.id = "SMid"
            context.Response.Write(jser.Serialize(jsonData))
        End Sub
 
        Public ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
            Get
                Return False
            End Get
        End Property

    End Class
    
End Namespace
