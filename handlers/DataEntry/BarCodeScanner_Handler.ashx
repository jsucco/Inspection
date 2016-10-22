<%@ WebHandler Language="VB" Class="core.BarCodeScanner_Handler" %>

Imports System
Imports System.Web
Imports System.IO
Imports Atalasoft.Barcoding.Reading
Imports System.Drawing
Imports Atalasoft.Imaging
Imports System.Web.HttpServerUtility

Namespace core
    

    Public Class BarCodeScanner_Handler : Implements IHttpHandler
        Public server As HttpServerUtility
        Private ir As New ImageReader
        Public Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest
            context.Response.ContentType = "text/plain"
            Dim file As HttpPostedFile
            file = context.Request.Files("flDoc")
            
            Dim BarList As String
            BarList = ir.GetBarCode(file)
            
            context.Response.Write(BarList)
            
        End Sub
 
        Public ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
            Get
                Return False
            End Get
        End Property

    End Class
    
    
End Namespace