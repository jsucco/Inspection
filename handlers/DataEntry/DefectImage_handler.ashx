<%@ WebHandler Language="VB" Class="core.DefectImage_handler" %>

Imports System
Imports System.Web

Namespace core

    Public Class DefectImage_handler : Implements IHttpHandler, IRequiresSessionState
        
        Private Property InspectInput As New InspectionInputDAO
    
        Public Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest
            context.Response.ContentType = "text/plain"
            Dim file As HttpPostedFile
            Dim RequestParams As NameValueCollection = context.Request.Params
            Dim DefectId As Array
            
            If IsNumeric(RequestParams.GetValues("DefectID")(0)) = True Then
                file = context.Request.Files("flDoc")
                DefectId = RequestParams.GetValues("DefectID")
                If InspectInput.InsertDefectImageById(file, DefectId(0)) Then
                    context.Response.Write("Success")
                Else
                    context.Response.Write("Failure Inserting")
                End If
            Else
                context.Response.Write("Failure Loading Picture File")
            End If
                
        End Sub
 
        Public ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
            Get
                Return False
            End Get
        End Property

    End Class
    
End Namespace
