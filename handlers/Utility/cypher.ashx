<%@ WebHandler Language="VB" Class="core.cypherashx" %>

Imports System
Imports System.Web
Imports App.Utilities.Web.Handlers
Imports System.Web.Script.Serialization

Namespace core

    Public Class cypherashx : Implements IHttpHandler
        
        Public Property cypherclass As New cypher
    
        Public Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest
            Dim ClientBaseurl As String = context.Request.Url.AbsoluteUri.Split("/")(2)
            Dim BaseUrl As String = HttpContext.Current.Request.Url.AbsoluteUri.Split("/")(2)
            Dim RequestParams As NameValueCollection = context.Request.Params
            Dim querystring As String = RequestParams("querystring")
            
            context.Response.ContentType = "text/plain"
            If ClientBaseurl = BaseUrl Then
                Dim queryhash As String = cypherclass.HashQueryString(querystring)
                context.Response.Write(queryhash)
            Else
                context.Response.Write("NOHASH")
            End If
            
        End Sub
 
        Public ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
            Get
                Return False
            End Get
        End Property

    End Class
    
End Namespace
