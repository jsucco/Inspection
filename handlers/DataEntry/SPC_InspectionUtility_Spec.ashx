<%@ WebHandler Language="VB" Class="core.SPC_InspectionUtility_Spec" %>

Imports System
Imports System.Web
Imports System.Web.Script.Serialization

Namespace core
   
    Public Class SPC_InspectionUtility_Spec : Implements IHttpHandler, IRequiresSessionState
        Private IU As New InspectionUtilityDAO
        Dim util As New Utilities
        
        Public Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest
            Dim RequestParams As NameValueCollection = context.Request.Params
            Dim resultobj As New Object
            If IsNothing(RequestParams.GetValues("oper")) = False Then
                Dim _oper = RequestParams.GetValues("oper")(0).ToString()                
                Dim SpecObj As New SPCInspection.ProductSpecs
                Dim bmapps As New BMappers(Of SPCInspection.ProductSpecs)
                Dim sqlstr As String = ""
                If _oper <> "del" Then

                    SpecObj.SpecId = util.ConvertType(RequestParams.GetValues("SpecId")(0), "Integer")
                    SpecObj.TabTemplateId = 0
                    SpecObj.DataNo = util.ConvertType(RequestParams.GetValues("DataNo")(0), "String")
                    SpecObj.ProductType = util.ConvertType(RequestParams.GetValues("ProductType")(0), "String")
                    SpecObj.Spec_Description = util.ConvertType(RequestParams.GetValues("Spec_Description")(0), "String")
                    SpecObj.HowTo = util.ConvertType(RequestParams.GetValues("HowTo")(0), "String")
                    SpecObj.value = util.ConvertType(RequestParams.GetValues("value")(0), "Decimal")
                    SpecObj.Upper_Spec_Value = util.ConvertType(RequestParams.GetValues("Upper_Spec_Value")(0), "Decimal")
                    SpecObj.Lower_Spec_Value = util.ConvertType(RequestParams.GetValues("Lower_Spec_Value")(0), "Decimal")
                    SpecObj.GlobalSpec = util.ConvertType(RequestParams.GetValues("GlobalSpec")(0), "Boolean")
                    SpecObj.SpecSource = util.ConvertType(RequestParams.GetValues("SpecSource")(0), "String")
                    If SpecObj.Lower_Spec_Value > SpecObj.Upper_Spec_Value Or SpecObj.Lower_Spec_Value = SpecObj.Upper_Spec_Value Then
                        resultobj = "Upper Spec Value must be greater than Lower Spec Value"
                        GoTo 101
                    End If
                Else
                    If IsNothing(RequestParams.GetValues("id")) = False Then
                        Dim _id As Integer = CType(RequestParams.GetValues("id")(0), Integer)
                        SpecObj.SpecId = util.ConvertType(RequestParams.GetValues("SpecId")(0), "Integer")
                    End If
                End If
                Select Case _oper
                    Case "add"
                        sqlstr = "INSERT INTO ProductSpecification (TabTemplateId, DataNo, ProductType, Spec_Description, HowTo, value, Upper_Spec_Value, Lower_Spec_Value, GlobalSpec, SpecSource)" & vbCrLf &
                                    "VALUES (@TabTemplateId,@DataNo,@ProductType,@Spec_Description,@HowTo,@value,@Upper_Spec_Value,@Lower_Spec_Value, @GlobalSpec, @SpecSource)"
                        If SpecObj.value > 0 And SpecObj.DataNo.Length > 2 And SpecObj.ProductType.Length > 2 Then
                            resultobj = bmapps.InsertSpcObject(sqlstr, SpecObj)
                            If resultobj = True Then
                                resultobj = "Product Spec successfully Added"
                            Else
                                resultobj = "Product Spec not added.  error"
                            End If
                        Else
                            resultobj = "Input Values not Properly filled Out"
                        End If
                    Case "edit"
                        sqlstr = "UPDATE ProductSpecification SET  TabTemplateId = @TabTemplateId, DataNo = @DataNo, ProductType = @ProductType, Spec_Description = @Spec_Description, HowTo = @HowTo, value = @value, Upper_Spec_Value = @Upper_Spec_Value, GlobalSpec = @GlobalSpec, " & vbCrLf &
                                    "Lower_Spec_Value = @Lower_Spec_Value WHERE (SpecId = @SpecId)"
                        If SpecObj.SpecId > 0 And SpecObj.value > 0 Then
                            resultobj = bmapps.InsertSpcObject(sqlstr, SpecObj)
                            If resultobj = True Then
                                resultobj = "Product Spec successfully Updated"
                            Else
                                resultobj = "Product Spec not added.  error"
                            End If
                        Else
                            resultobj = "SpecId or value cannot be zero"
                        End If
                    Case "del"
                        sqlstr = "DELETE FROM ProductSpecification WHERE (SpecId = @SpecId)"
                        If SpecObj.SpecId > 0 Then
                            resultobj = bmapps.InsertSpcObject(sqlstr, SpecObj)
                            If resultobj = True Then
                                resultobj = "Product Spec successfully Deleted"
                            Else
                                resultobj = "Product Spec not added.  error"
                            End If
                        Else
                            resultobj = "SpecId cannot be zero"
                        End If
                End Select
                    
            End If
                Dim jser As New JavaScriptSerializer
101:
                context.Response.Write(resultobj)
        End Sub
 
        Public ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
            Get
                Return False
            End Get
        End Property

    End Class
    
    
End Namespace
