<%@ WebHandler Language="VB" Class="core.SPC_InspectionInput_SpecSave" %>

Imports System
Imports System.Web
Imports System.Reflection

Namespace core
    Public Class PSpecSave
        Public Property SpecId As Integer
        Public Property InspectionSummaryId As Integer
        Public Property InspectionId As Integer
        Public Property TemplateId As Integer
        Public Property WorkOrder As String
        Public Property WOQuantity As Integer
        Public Property AQL As String
        Public Property Inspector As String
        Public Property Measured_Value As Decimal
        Public Property Location As String
        Public Property DataNo As String
        Public Property SampleSize As Integer
        Public Property InspectionState As String
        Public Property DefectDesc As String
        Public Property RejectLimiter As Integer
        Public Property CPNumber As Integer
        Public Property SpecItemCount As Integer
    End Class

    Public Class SPC_InspectionInput_SpecSave : Implements IHttpHandler, IRequiresSessionState
        Dim IU As New InspectionUtilityDAO, II As New InspectionInputDAO
        Dim ErrorMessage As String
        Dim pssObj As PSpecSave
        Dim util As New Utilities
        
        Public Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest
            Dim RequestParams As NameValueCollection = context.Request.Params
            Dim DefectId As Object = 0
            Dim RequiredParameters As Boolean = True
            Dim SpecDelta As Decimal = 0
            context.Response.ContentType = "text/plain"
            If RequestParams.Count > 0 Then
                Dim bmappss As New BMappers(Of PSpecSave)
                pssObj = bmappss.GetReqParamAsObject(RequestParams)
               
                If IsNothing(pssObj) = False Then
                    Dim listps As New List(Of SPCInspection.ProductSpecs)
                    Dim bmapps As New BMappers(Of SPCInspection.ProductSpecs)
                    Dim OutSpecFlag As Boolean = False
                    Dim SpecDefect As New SPCInspection.DefectMaster
                    Dim MajorDefectFlag As Boolean = False
                    Dim DefectList As New List(Of SPCInspection.DefectMaster)
                    
                    listps = bmapps.GetInspectObject("SELECT top(1) SpecId, DataNo, Spec_Description, value, Upper_Spec_Value, Lower_Spec_Value  FROM  ProductSpecification  WHERE    (SpecId = " & pssObj.SpecId.ToString() & ")")
                    
                    If listps.Count > 0 Then
                        SpecDelta = pssObj.Measured_Value - listps.ToArray()(0).value

                        If SpecDelta > listps.ToArray()(0).Upper_Spec_Value Then
                            MajorDefectFlag = True
                            pssObj.DefectDesc = "MEASUREMENT ABOVE SPEC"
                        ElseIf SpecDelta < listps.ToArray()(0).Lower_Spec_Value Then
                            MajorDefectFlag = True
                            pssObj.DefectDesc = "MEASUREMENT BELOW SPEC"
                        End If
                        
                        If MajorDefectFlag = True Then
                            DefectList.Add(New SPCInspection.DefectMaster With {.WorkOrder = pssObj.WorkOrder, .TemplateId = pssObj.TemplateId, .TotalLotPieces = pssObj.WOQuantity, .AQL = pssObj.AQL, .Location = pssObj.Location, .DataNo = pssObj.DataNo, .Inspector = "", .InspectionJobSummaryId = pssObj.InspectionSummaryId, .InspectionId = pssObj.InspectionId, .SampleSize = pssObj.SampleSize, .InspectionState = pssObj.InspectionState, .DefectClass = "MAJOR", .Tablet = "Browser", .DefectTime = Date.Now, .DataType = "Spec", .DefectDesc = pssObj.DefectDesc, .Product = "", .RejectLimiter = pssObj.RejectLimiter, .POnumber = pssObj.CPNumber, .EmployeeNo = pssObj.Inspector, .LotNo = "", .Comment = "", .ItemNumber = "", .WorkRoom = "", .ThisPieceNo = "", .Dimensions = "", .RollNumber = ""})
                            DefectId = II.InsertDefects(DefectList)
                        End If
                    End If
                    Dim returnval As Boolean
                    Try
                        returnval = IU.InsertSpecMeasurement(pssObj.SpecId, DefectId, pssObj.InspectionId, pssObj.InspectionSummaryId, pssObj.Measured_Value, pssObj.SpecItemCount, SpecDelta)
                    Catch ex As Exception
                        context.Response.Write(ex.Message)
                    End Try
                    
                    If returnval = True Then
                        context.Response.Write("success")
                    Else
                        context.Response.Write("failure")
                    End If
                    
                Else
                    context.Response.Write("All Required Parameters not Sent")
                End If
            End If

        
        End Sub
        
        Public ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
            Get
                Return False
            End Get
        End Property

    End Class
    

End Namespace