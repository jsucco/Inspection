<%@ WebHandler Language="VB" Class="core.SPC_InspectionUtility_SpecLoad" %>

Imports System
Imports System.Web
Imports System.Web.Script.Serialization

Namespace core

    Public Class SPC_InspectionUtility_SpecLoad : Implements IHttpHandler, IRequiresSessionState
        
        Dim ProductSpecscache As List(Of SPCInspection.ProductSpecs)
        
        Public Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest
            Dim RequestParams As NameValueCollection = context.Request.Params
            Dim jser As New JavaScriptSerializer
            jser.MaxJsonLength = Int32.MaxValue
            If IsNothing(RequestParams.GetValues("oper")) = True Then
                LoadProductSpecs()
                If ProductSpecscache.Count > 0 Then
                    context.Response.Write(jser.Serialize(ProductSpecscache))
                End If
            End If
      
        End Sub
 
        Public ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
            Get
                Return False
            End Get
        End Property
    
        Private Sub LoadProductSpecs()
            Dim sqlstr As String = "SELECT top(5000) SpecId, TabTemplateId, DataNo, ProductType, Spec_Description, HowTo, value, Upper_Spec_Value, Lower_Spec_Value, GlobalSpec, SpecSource" & vbCrLf &
                            "FROM  ProductSpecification WHERE (SpecSource = 'user')"
            Dim bmapps As New BMappers(Of SPCInspection.ProductSpecs)
            ProductSpecscache = bmapps.GetInspectObject(sqlstr)

        End Sub

    End Class
    
        
End Namespace