<%@ WebHandler Language="VB" Class="core.SPC_InspectionInput_SpecLoad" %>

Imports System
Imports System.Web
Imports System.Web.Script.Serialization

Namespace core
    Public Class pSpecLoad
        Public Property DataNo As String
        Public Property WorkOrder As String
        Public Property oper As Object
        Public Property SessionID As String
        Public Property CID As String
    End Class
    Public Class SPC_InspectionInput_SpecLoad : Implements IHttpHandler, IRequiresSessionState

        Dim ProductSpecscache As New List(Of SPCInspection.InspectProductSpec)
        Dim II As New InspectionInputDAO
        Dim bmapps As New BMappers(Of SPCInspection.ProductSpecs)
        Dim listpps As New List(Of SPCInspection.PDMProductSpecs)
        Dim objsl As New pSpecLoad
        'Dim SessionID As String = ""
        'Dim DataNo As String = ""
        'Dim WorkOrder As String
        Public Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest
            Dim RequestParams As NameValueCollection = context.Request.Params
            Dim jser As New JavaScriptSerializer


            If RequestParams.Count > 0 Then
                Dim bmapsl As New BMappers(Of pSpecLoad)
                objsl = bmapsl.GetReqParamAsObject(RequestParams)
                If IsNothing(objsl.DataNo) = False And IsNothing(objsl.oper) = True And IsNothing(objsl.CID) = False Then
                    Select Case objsl.CID
                        Case "000111"
                            If IsNothing(objsl.WorkOrder) = False Then
                                If objsl.WorkOrder.Length > 1 And objsl.DataNo.Length > 1 Then
                                    ProductSpecscache = II.GetInteriorSpecs(objsl.WorkOrder, objsl.DataNo)
                                    GetSPCSpecsList()
                                    UpdateInteriorsSpecList(ProductSpecscache)
                                End If
                            End If
                        Case "000112"
                            If IsNothing(objsl.WorkOrder) = False Then
                                If objsl.WorkOrder.Length > 1 And objsl.DataNo.Length > 1 Then
                                    ProductSpecscache = II.GetInteriorSpecs(objsl.WorkOrder, objsl.DataNo)
                                    GetSPCSpecsList()
                                    UpdateInteriorsSpecList(ProductSpecscache)
                                End If
                            End If
                        Case "000113"
                            If IsNothing(objsl.WorkOrder) = False Then
                                If objsl.WorkOrder.Length > 1 And objsl.DataNo.Length > 1 Then
                                    ProductSpecscache = II.GetInteriorSpecs(objsl.WorkOrder, objsl.DataNo)
                                    GetSPCSpecsList()
                                    UpdateInteriorsSpecList(ProductSpecscache)
                                End If
                            End If
                        Case Else
                            If objsl.DataNo.Length > 0 Then
                                'listpps = II.GetPDMProductSpecs(objsl.DataNo)
                                'UpdateSPCSpecsList(listpps)
                                GetSPCSpecsList()
                            End If

                    End Select
                    ProductSpecscache = AddWeightSpec(ProductSpecscache)
                End If
            End If

            'If IsNothing(RequestParams.GetValues("oper")) = True And IsNothing(RequestParams.GetValues("DataNo")) = False Then
            '    'DataNo = RequestParams.GetValues("DataNo")(0).ToString()
            '    'Dim CID As String = ""

            '    'If IsNothing(RequestParams.GetValues("SessionID")) = False Then
            '    '    SessionID = RequestParams.GetValues("SessionID")(0).ToString()
            '    'End If

            '    If Len(DataNo) > 1 Then
            '        listpps = II.GetPDMProductSpecs(DataNo)
            '        UpdateSPCSpecsList(listpps)
            '        GetSPCSpecsList()
            '        ProductSpecscache = AddWeightSpec(ProductSpecscache)
            '    End If
            'End If

            If ProductSpecscache.Count > 0 Then
                Dim productspecret As New List(Of SPCInspection.InspectProductSpec)

                context.Response.Write(jser.Serialize(ProductSpecscache))
            End If
        End Sub

        Public ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
            Get
                Return False
            End Get
        End Property

        Private Function AddWeightSpec(ByRef speclist As List(Of SPCInspection.InspectProductSpec)) As List(Of SPCInspection.InspectProductSpec)
            Dim listps As List(Of SPCInspection.ProductSpecs)

            listps = bmapps.GetInspectObject("SELECT  SpecId, POM_Row, TabTemplateId, DataNo, ProductType, Spec_Description, HowTo, value, Upper_Spec_Value, Lower_Spec_Value FROM  ProductSpecification WHERE  (DataNo = 'ALL') AND (ProductType = N'ALL')")

            If listps.Count > 0 Then
                speclist.Add(New SPCInspection.InspectProductSpec With {.SpecId = listps.ToArray()(0).SpecId, .POM_Row = listps.ToArray()(0).POM_Row, .DataNo = listps.ToArray()(0).DataNo, .RefCode = 1, .Spec_Description = listps.ToArray()(0).Spec_Description, .Lower_Spec_Value = 0, .Upper_Spec_Value = 0, .value = 0})

            End If

            Return speclist
        End Function

        Private Sub GetSPCSpecsList()
            Dim prodlist As New List(Of SPCInspection.InspectProductSpec)
            Dim bmappl As New BMappers(Of SPCInspection.InspectProductSpec)
            Dim sqlstr As String = "SELECT        SpecId, POM_Row, DataNo, ProductType, Spec_Description, value, Upper_Spec_Value, Lower_Spec_Value FROM ProductSpecification" & vbCrLf &
                                    "WHERE        (SpecSource = 'user' AND GlobalSpec = 1 AND DataNo = '" & objsl.DataNo & "') OR (SpecSource = 'user' AND ME_SessionID = '" & objsl.SessionID & "' AND DataNo = '" & objsl.DataNo & "')"


            prodlist = bmappl.GetInspectObject(sqlstr)

            If prodlist.Count > 0 Then
                For Each item As SPCInspection.InspectProductSpec In prodlist
                    ProductSpecscache.Add(New SPCInspection.InspectProductSpec With {.SpecId = item.SpecId, .POM_Row = item.POM_Row, .RefCode = 0, .Spec_Description = item.Spec_Description, .Upper_Spec_Value = item.Upper_Spec_Value, .Lower_Spec_Value = item.Lower_Spec_Value, .Measured_Value = 0, .value = item.value})
                Next
            End If

        End Sub
        Private Sub UpdateInteriorsSpecList(ByVal listps As List(Of SPCInspection.InspectProductSpec))
            If listps.Count > 0 Then
                Dim listret As New List(Of SPCInspection.InspectProductSpec)

                For Each item In listps
                    Dim psobj As New SPCInspection.ProductSpecs
                    Dim listso As New List(Of SingleObject)
                    Dim bmapso As New BMappers(Of SingleObject)
                    Dim updatedps As Boolean = False
                    Dim sql As String
                    Dim updateSpecId As Integer = 0

                    psobj.POM_Row = item.POM_Row
                    psobj.DataNo = item.DataNo
                    psobj.Spec_Description = item.Spec_Description
                    psobj.value = item.value
                    psobj.ProductType = item.DataNo
                    psobj.HowTo = ""
                    Try
                        listso = bmapso.GetInspectObject("SELECT TOP(1) Upper_Spec_Value as Object1, Lower_Spec_Value as Object3 FROM ProductSpecification WHERE DataNo = '" & item.DataNo & "' AND Spec_Description = '" & item.Spec_Description & "' AND SpecSource = 'Interiors'")
                        If listso.Count > 0 Then
                            psobj.Upper_Spec_Value = listso.ToArray()(0).Object1
                            psobj.Lower_Spec_Value = listso.ToArray()(0).Object3

                            sql = "UPDATE  ProductSpecification SET  ProductType = @ProductType, Spec_Description = '" & psobj.Spec_Description & "', HowTo = @HowTo, value = @value, Upper_Spec_Value = @Upper_Spec_Value, " & vbCrLf &
                                    "Lower_Spec_Value = @Lower_Spec_Value OUTPUT INSERTED.SpecId WHERE  (DataNo = '" & item.DataNo & "' ) AND (Spec_Description = '" & item.Spec_Description & "' ) AND (SpecSource = 'Interiors' )"
                            updatedps = bmapps.InsertSpcObject_RetNum(sql, psobj, True)
                            If updatedps = True Then
                                updateSpecId = bmapps.RowReturnIdentity
                            End If
                        Else
                            psobj.Upper_Spec_Value = item.Upper_Spec_Value
                            psobj.Lower_Spec_Value = item.Lower_Spec_Value

                            sql = "INSERT INTO ProductSpecification (POM_Row, ProductType, Spec_Description, HowTo, value, Upper_Spec_Value, Lower_Spec_Value, DataNo, SpecSource )" & vbCrLf &
                                "VALUES (@POM_Row,@ProductType,@Spec_Description,@HowTo,@value,@Upper_Spec_Value,@Lower_Spec_Value,@DataNo, 'Interiors' )"
                            updatedps = bmapps.InsertSpcObject_RetNum(sql, psobj, True)
                            If updatedps = True Then
                                updateSpecId = bmapps.RowReturnIdentity
                            End If
                        End If

                        listret.Add(New SPCInspection.InspectProductSpec With {.SpecId = updateSpecId, .POM_Row = psobj.POM_Row, .RefCode = 0, .Spec_Description = psobj.Spec_Description, .Upper_Spec_Value = psobj.Upper_Spec_Value, .Lower_Spec_Value = psobj.Lower_Spec_Value, .Measured_Value = item.value, .value = item.value})

                    Catch ex As Exception

                    End Try
                Next
                ProductSpecscache.Clear()
                ProductSpecscache = listret
            End If
        End Sub
        Private Sub UpdateSPCSpecsList(ByVal listpps As List(Of SPCInspection.PDMProductSpecs))
            Dim matchcnt As Integer = 1
            If listpps.Count > 0 Then
                For Each item In listpps.ToArray()
                    Dim listspcps As New List(Of SPCInspection.ProductSpecs)
                    Dim updatedps As Boolean = False
                    Dim updateSpecId As Integer = 0
                    Dim psobj As New SPCInspection.ProductSpecs
                    Dim util As New Utilities
                    Dim sql As String

                    psobj.POM_Row = item.POM_Row
                    psobj.DataNo = item.Style
                    psobj.HowTo = item.HowTo
                    psobj.Lower_Spec_Value = util.ConvertStrFractionToDecimal(item.TolMinus)
                    psobj.ProductType = item.ProductType
                    psobj.Spec_Description = item.Description
                    psobj.Upper_Spec_Value = util.ConvertStrFractionToDecimal(item.TolPlus)
                    psobj.value = util.ConvertStrFractionToDecimal(item.Grade)

                    listspcps = bmapps.GetInspectObject("SELECT SpecId, value, Upper_Spec_Value, Lower_Spec_Value FROM ProductSpecification WHERE DataNo = '" & item.Style & "' and POM_Row = " & item.POM_Row & " and value = " & item.Grade & "")

                    If listspcps.Count > 0 Then
                        For Each item1 In listspcps.ToArray()
                            sql = "UPDATE  ProductSpecification SET  ProductType = @ProductType, Spec_Description = @Spec_Description, HowTo = @HowTo, value = @value, Upper_Spec_Value = @Upper_Spec_Value, " & vbCrLf &
                                    "Lower_Spec_Value = @Lower_Spec_Value OUTPUT INSERTED.SpecId WHERE  (DataNo = @DataNo ) AND (POM_Row = @POM_Row )"
                            updatedps = bmapps.InsertSpcObject_RetNum(sql, psobj, True)
                            If updatedps = True Then
                                updateSpecId = bmapps.RowReturnIdentity
                            End If

                        Next
                    ElseIf listspcps.Count = 0 Then
                        sql = "INSERT INTO ProductSpecification (POM_Row, ProductType, Spec_Description, HowTo, value, Upper_Spec_Value, Lower_Spec_Value, DataNo )" & vbCrLf &
                                "VALUES (@POM_Row,@ProductType,@Spec_Description,@HowTo,@value,@Upper_Spec_Value,@Lower_Spec_Value,@DataNo )"
                        updatedps = bmapps.InsertSpcObject_RetNum(sql, psobj, True)
                        If updatedps = True Then
                            updateSpecId = bmapps.RowReturnIdentity
                        End If

                    End If

                    If updateSpecId > 0 Then
                        Dim RefCodeCount As Integer = (From x In ProductSpecscache Where x.RefCode = item.RefCode And x.value = psobj.value And x.Upper_Spec_Value = psobj.Upper_Spec_Value And x.Lower_Spec_Value = psobj.Lower_Spec_Value Select x.SpecId, x.POM_Row, x.RefCode).Count()
                        If RefCodeCount = 0 Then
                            Dim RefCodeCount2 As Integer = (From x In ProductSpecscache Where x.RefCode = item.RefCode And x.Spec_Description = psobj.Spec_Description Select x.SpecId, x.POM_Row, x.RefCode).Count()
                            If RefCodeCount2 > 0 Then
                                matchcnt += 1
                                psobj.Spec_Description = psobj.Spec_Description + " *" + matchcnt.ToString()
                            End If
                            ProductSpecscache.Add(New SPCInspection.InspectProductSpec With {.SpecId = updateSpecId, .POM_Row = psobj.POM_Row, .RefCode = item.RefCode, .Spec_Description = psobj.Spec_Description, .Upper_Spec_Value = psobj.Upper_Spec_Value, .Lower_Spec_Value = psobj.Lower_Spec_Value, .Measured_Value = 0, .value = psobj.value})
                        End If
                    End If
                Next

            End If

        End Sub
    End Class


End Namespace