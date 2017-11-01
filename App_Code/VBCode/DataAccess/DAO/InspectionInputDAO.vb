Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Data
Imports System.Drawing
Imports System.Web.Script.Serialization

Namespace core
    
    Public Class InspectionInputDAO
        Public Shared InspectionCache As New List(Of SPCInspection.InspectionCacheVar)
        Public Shared SpecCache As New List(Of SPCInspection.InspectionCacheVar)
        Public Property _DAOFactory As New DAOFactory
        Public Property dlayer As New dlayer
        Private Property Util As New Utilities

        Public Function InsertDefects(ByVal input As List(Of SPCInspection.DefectMaster)) As Integer
            Dim SQL As String
            Dim Internalcmd As SqlCommand
            Dim inputarray() As SPCInspection.DefectMaster
            Dim returnint As Int32 = 0

            If Not (input Is Nothing) Then
                inputarray = input.ToArray()
            Else
                Return False
                Exit Function
            End If
            SQL = "INSERT INTO DefectMaster (DefectTime, DefectDesc, POnumber, DataNo, EmployeeNo, ThisPieceNo," & vbCrLf &
                    "SampleSize, RejectLimiter, TotalLotPieces, Product, DefectClass, MergeDate, Tablet, WorkOrder, LotNo, " & vbCrLf &
                    "Location, DataType, Dimensions, Comment, LoomNo, DefectPoints, GriegeNo, RollNo, Operation, TemplateId, InspectionId, ButtonTemplateId, Inspector, ItemNumber, InspectionState, WorkRoom, InspectionJobSummaryId, WeaverShiftId)" & vbCrLf &
                    "VALUES (@DefectTime, @DefectDesc, @CPNumber, @DataNo,@EmployeeNo, @ThisPieceNo,@SampleSize, @RejectLimiter, @TotalLotPieces, " & vbCrLf &
                    " @Product, @DefectClass, @MergeDate, @Tablet,@WorkOrder,@LotNo,@Location,@DataType,@Dimensions,@Comment,@LoomNo,@DefectPoints,@GreigeNo,@RollNo," & vbCrLf &
                    "@Operation,@TemplateId, @InspectionId, @ButtonTemplateId, @Inspector, @ItemNumber, @InspectionState, @WorkRoom, @InspectionJobSummaryId, @WeaverShiftId);" & vbCrLf &
                    "SELECT SCOPE_IDENTITY();"

            Using connection As New SqlConnection(dlayer.InspectConnectionString())

                Internalcmd = _DAOFactory.GetCommand(SQL, connection)

                Internalcmd.Parameters.Add(_DAOFactory.Getparameter("@DefectTime", DbType.DateTime))
                Internalcmd.Parameters.Add(_DAOFactory.Getparameter("@DefectDesc", DbType.String))
                Internalcmd.Parameters.Add(_DAOFactory.Getparameter("@CPNumber", DbType.String))
                Internalcmd.Parameters.Add(_DAOFactory.Getparameter("@DataNo", DbType.String))
                Internalcmd.Parameters.Add(_DAOFactory.Getparameter("@EmployeeNo", DbType.String))
                Internalcmd.Parameters.Add(_DAOFactory.Getparameter("@ThisPieceNo", DbType.String))
                Internalcmd.Parameters.Add(_DAOFactory.Getparameter("@SampleSize", DbType.String))
                Internalcmd.Parameters.Add(_DAOFactory.Getparameter("@RejectLimiter", DbType.String))
                Internalcmd.Parameters.Add(_DAOFactory.Getparameter("@TotalLotPieces", DbType.String))
                Internalcmd.Parameters.Add(_DAOFactory.Getparameter("@Product", DbType.String))
                Internalcmd.Parameters.Add(_DAOFactory.Getparameter("@DefectClass", DbType.String))
                Internalcmd.Parameters.Add(_DAOFactory.Getparameter("@MergeDate", DbType.DateTime))
                Internalcmd.Parameters.Add(_DAOFactory.Getparameter("@Tablet", DbType.String))
                Internalcmd.Parameters.Add(_DAOFactory.Getparameter("@WorkOrder", DbType.String))
                Internalcmd.Parameters.Add(_DAOFactory.Getparameter("@LotNo", DbType.String))
                Internalcmd.Parameters.Add(_DAOFactory.Getparameter("@Location", DbType.String))
                Internalcmd.Parameters.Add(_DAOFactory.Getparameter("@DataType", DbType.String))
                Internalcmd.Parameters.Add(_DAOFactory.Getparameter("@Dimensions", DbType.String))
                Internalcmd.Parameters.Add(_DAOFactory.Getparameter("@Comment", DbType.String))
                Internalcmd.Parameters.Add(_DAOFactory.Getparameter("@LoomNo", DbType.String))
                Internalcmd.Parameters.Add(_DAOFactory.Getparameter("@DefectPoints", DbType.String))
                Internalcmd.Parameters.Add(_DAOFactory.Getparameter("@GreigeNo", DbType.String))
                Internalcmd.Parameters.Add(_DAOFactory.Getparameter("@RollNo", DbType.String))
                Internalcmd.Parameters.Add(_DAOFactory.Getparameter("@Operation", DbType.String))
                Internalcmd.Parameters.Add(_DAOFactory.Getparameter("@TemplateId", DbType.Int32))
                Internalcmd.Parameters.Add(_DAOFactory.Getparameter("@InspectionId", DbType.Int32))
                Internalcmd.Parameters.Add(_DAOFactory.Getparameter("@ButtonTemplateId", DbType.Int32))
                Internalcmd.Parameters.Add(_DAOFactory.Getparameter("@Inspector", DbType.String))
                Internalcmd.Parameters.Add(_DAOFactory.Getparameter("@ItemNumber", DbType.String))
                Internalcmd.Parameters.Add(_DAOFactory.Getparameter("@InspectionState", DbType.String))
                Internalcmd.Parameters.Add(_DAOFactory.Getparameter("@WorkRoom", DbType.String))
                Internalcmd.Parameters.Add(_DAOFactory.Getparameter("@InspectionJobSummaryId", DbType.Int32))
                Internalcmd.Parameters.Add(_DAOFactory.Getparameter("@WeaverShiftId", DbType.Int32))
                Internalcmd.Parameters("@DefectTime").Value = inputarray(0).DefectTime
                Internalcmd.Parameters("@DefectDesc").Value = inputarray(0).DefectDesc
                Internalcmd.Parameters("@CPNumber").Value = inputarray(0).POnumber
                Internalcmd.Parameters("@DataNo").Value = inputarray(0).DataNo
                Internalcmd.Parameters("@EmployeeNo").Value = inputarray(0).EmployeeNo
                If inputarray(0).LotSize <> Nothing Then
                    Internalcmd.Parameters("@ThisPieceNo").Value = inputarray(0).LotSize
                Else
                    Internalcmd.Parameters("@ThisPieceNo").Value = System.DBNull.Value
                End If
                Internalcmd.Parameters("@SampleSize").Value = inputarray(0).SampleSize
                Internalcmd.Parameters("@RejectLimiter").Value = inputarray(0).RejectLimiter
                Internalcmd.Parameters("@TotalLotPieces").Value = inputarray(0).TotalLotPieces
                If inputarray(0).Product <> Nothing Then
                    Internalcmd.Parameters("@Product").Value = inputarray(0).Product
                Else
                    Internalcmd.Parameters("@Product").Value = System.DBNull.Value
                End If

                Internalcmd.Parameters("@DefectClass").Value = inputarray(0).DefectClass
                If inputarray(0).MergeDate <> Nothing Then
                    Internalcmd.Parameters("@MergeDate").Value = inputarray(0).MergeDate
                Else
                    Internalcmd.Parameters("@MergeDate").Value = System.DBNull.Value
                End If

                Internalcmd.Parameters("@Tablet").Value = inputarray(0).Tablet
                Internalcmd.Parameters("@WorkOrder").Value = inputarray(0).WorkOrder
                Internalcmd.Parameters("@LotNo").Value = inputarray(0).LotNo
                If inputarray(0).DataType.Substring(0, 3) = "STT" Then
                    Internalcmd.Parameters("@Location").Value = "578"
                Else
                    Internalcmd.Parameters("@Location").Value = inputarray(0).Location
                End If

                Internalcmd.Parameters("@DataType").Value = inputarray(0).DataType
                Internalcmd.Parameters("@Dimensions").Value = inputarray(0).Dimensions
                If inputarray(0).Comment <> Nothing Then
                    Internalcmd.Parameters("@Comment").Value = inputarray(0).Comment
                Else
                    Internalcmd.Parameters("@Comment").Value = System.DBNull.Value
                End If
                Internalcmd.Parameters("@LoomNo").Value = inputarray(0).LoomNumber
                Internalcmd.Parameters("@DefectPoints").Value = System.DBNull.Value
                Internalcmd.Parameters("@GreigeNo").Value = System.DBNull.Value
                Internalcmd.Parameters("@RollNo").Value = inputarray(0).RollNumber
                Internalcmd.Parameters("@Operation").Value = System.DBNull.Value
                Internalcmd.Parameters("@InspectionId").Value = inputarray(0).InspectionId
                Internalcmd.Parameters("@ButtonTemplateId").Value = inputarray(0).ButtonTemplateId
                Internalcmd.Parameters("@Inspector").Value = inputarray(0).Inspector
                Internalcmd.Parameters("@ItemNumber").Value = inputarray(0).ItemNumber
                Internalcmd.Parameters("@InspectionState").Value = inputarray(0).InspectionState
                Internalcmd.Parameters("@WorkRoom").Value = inputarray(0).WorkRoom
                Internalcmd.Parameters("@InspectionJobSummaryId").Value = inputarray(0).InspectionJobSummaryId
                Internalcmd.Parameters("@WeaverShiftId").Value = inputarray(0).WeaverShiftId

                If inputarray(0).TemplateId <> Nothing Then
                    Internalcmd.Parameters("@TemplateId").Value = inputarray(0).TemplateId
                Else
                    Return 0
                    Exit Function
                End If


                Try
                    Internalcmd.Connection.Open()
                    returnint = Convert.ToInt32(Internalcmd.ExecuteScalar())
                    RegisterInspectionCache(returnint, inputarray(0).Location)
                    Return returnint
                Catch ex As Exception

                    Return -1
                    Exit Function
                End Try

            End Using

        End Function

        Public Sub InsertDefectAsync(ByVal input As List(Of SPCInspection.DefectMaster))
            Try
                Dim t As System.Threading.Tasks.Task = System.Threading.Tasks.Task.Run(Sub()
                                                                                           InsertDefects(input)
                                                                                       End Sub)
            Catch ex As Exception
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex)
            End Try
        End Sub

        Private Sub RegisterInspectionCache(ByVal DefectID As Integer, ByVal CID As String)

            Dim count = (From v In InspectionCache Where v.CID = CID).Count()
            If count = 0 Then
                InspectionCache.Add(New SPCInspection.InspectionCacheVar With {.CID = CID, .LastDefectID = DefectID, .LastDefectIDTimeStamp = DateTime.Now})
            Else
                For Each item In InspectionCache
                    If item.CID = CID Then
                        item.LastDefectID = DefectID
                        item.LastDefectIDTimeStamp = DateTime.Now
                    End If
                Next
            End If

        End Sub
        Public Shared Sub RegisterSpecCache(ByVal SpecID As Integer, ByVal CID As String)

            Dim count = (From v In InspectionCache Where v.CID = CID).Count()
            If count = 0 Then
                SpecCache.Add(New SPCInspection.InspectionCacheVar With {.CID = CID, .LastDefectID = SpecID, .LastDefectIDTimeStamp = DateTime.Now})
            Else
                For Each item In SpecCache
                    If item.CID = CID Then
                        item.LastDefectID = SpecID
                        item.LastDefectIDTimeStamp = DateTime.Now
                    End If
                Next
            End If

        End Sub
        Public Function CalculateDHU(ByVal TargetState As String, ByVal TargetNumber As String, ByVal InspectionJobSummaryId As Integer, ByVal TotalInspected As Integer) As Decimal
            Dim DHU As Decimal = 0
            Try
                Dim sqlis As String = ""
                Dim retupdate As Boolean = False
                Dim objis As New SPCInspection.InspectionJobSummary
                Dim AllTypesDefectCount As Integer = 0
                Dim listso As New List(Of SingleObject)
                Dim bmapso As New BMappers(Of SingleObject)

                sqlis = "SELECT COUNT(DefectID) AS Object1 FROM DefectMaster  INNER JOIN ButtonTemplate ON DefectMaster.ButtonTemplateId = ButtonTemplate.id WHERE DefectMaster." & TargetState & " = '" & TargetNumber & "' AND InspectionJobSummaryId = " & InspectionJobSummaryId.ToString() & " and DefectMaster.DefectClass <> 'TIME' AND DefectMaster.DefectClass <> 'FIX' AND DefectMaster.DefectClass <> 'UPGRADE'"
                listso = bmapso.GetInspectObject(sqlis)
                If listso.Count > 0 And TargetState = "WorkOrder" Then
                    AllTypesDefectCount = listso.ToArray()(0).Object1

                    If TotalInspected > 0 Then
                        DHU = (AllTypesDefectCount * 100) / TotalInspected
                    End If
                End If


            Catch ex As Exception

            End Try

            Return DHU

        End Function

        Public Function GetInspectionJobSummaryIdCount(ByVal TargetNumber As String, ByVal TemplateId As Integer) As Integer
            Dim returnid As Integer = 0
            Dim sql As String
            Dim bmap_so As New BMappers(Of SingleObject)
            Dim listso As New List(Of SingleObject)

            sql = "SELECT COUNT(*) AS Object1 FROM InspectionJobSummary WHERE (JobNumber = '" & TargetNumber & "') AND (Technical_PassFail IS NULL) AND (TemplateId = " & TemplateId.ToString() & ")"

            listso = bmap_so.GetInspectObject(sql)

            If listso.Count > 0 Then
                returnid = listso.ToArray()(0).Object1
            End If

            Return returnid
        End Function

        Public Function GetOpenInspectionJobSummaryId(ByVal TargetNumber As String, ByVal TemplateId As Integer) As Integer
            Dim returnid As Integer = 0
            Dim sql As String
            Dim bmap_so As New BMappers(Of SingleObject)
            Dim listso As New List(Of SingleObject)

            sql = "SELECT TOP(1) id AS Object1 FROM InspectionJobSummary WHERE (JobNumber = '" & TargetNumber & "') AND (Technical_PassFail IS NULL) AND (TemplateId = " & TemplateId.ToString() & ")"

            listso = bmap_so.GetInspectObject(sql)

            If listso.Count > 0 Then
                returnid = listso.ToArray()(0).Object1
            End If

            Return returnid
        End Function

        Public Function GetOpenInspectionJobSummaryId2(ByVal TargetNumber As String, ByVal TemplateId As Integer) As List(Of SPCInspection.ExInspectionReturn)

            Dim sql As String
            Dim bmap_so As New BMappers(Of SPCInspection.ExInspectionReturn)
            Dim listso As New List(Of SPCInspection.ExInspectionReturn)

            sql = "SELECT TOP(1) ijs.id, CAST(ijs.AQL_Level AS VARCHAR(10)) AS AQL_Level, ijs.Standard, ijs.RejectLimiter, ijs.SampleSize, ijs.ItemFailCount, ijs.WOQuantity, tn.LineType  FROM InspectionJobSummary ijs inner join TemplateName tn ON ijs.TemplateId = tn.TemplateId WHERE (JobNumber = '" & TargetNumber & "') AND (Technical_PassFail IS NULL) AND (TemplateId = " & TemplateId.ToString() & ")"

            listso = bmap_so.GetInspectObject(sql)

            Return listso
        End Function

        Public Function GetNewInspectionJobSummaryId(ByVal TargetNumber As String) As Integer
            Dim returnid As Integer = 0
            Dim sql As String
            Dim bmap_so As New BMappers(Of SingleObject)
            Dim listso As New List(Of SingleObject)

            sql = "SELECT TOP(1) id AS Object1 FROM InspectionJobSummary ORDER BY id desc"

            listso = bmap_so.GetInspectObject(sql)

            If listso.Count > 0 Then
                returnid = CType(listso.ToArray()(0).Object1, Integer) + 1
            End If

            Return returnid
        End Function

        Public Function InsertJobSummaryRecord(ByVal jslist As SPCInspection.InspectionJobSummary) As Int64
            Dim returnid As Int64 = 0
            Dim bmapjs As New BMappers(Of SPCInspection.InspectionJobSummary)
            If IsNothing(jslist) = False Then

                ' returnid = bmapjs.InsertSpcObject()
                Dim sql As String = "INSERT INTO InspectionJobSummary (JobType, JobNumber, Standard, CID, TemplateId, ItemPassCount, ItemFailCount, WOQuantity, WorkOrderPieces, AQL_Level, SampleSize, RejectLimiter, Inspection_Started, DataNo, UnitCost, UnitDesc, PRP_Code, EmployeeNo, CasePack, WorkRoom, TotalInspectedItems )" & vbCrLf &
                                        "VALUES (@JobType,@JobNumber, @Standard, @CID, @TemplateId, @ItemPassCount,@ItemFailCount,@WOQuantity, @WorkOrderPieces, @AQL_Level,@SampleSize,@RejectLimiter, @Inspection_Started, @DataNo, @UnitCost, @UnitDesc, @PRP_Code, @EmployeeNo, @CasePack, @WorkRoom, @TotalInspectedItems )"
                returnid = bmapjs.InsertSpcObject_RetNum(sql, jslist, True)

            End If

            Return bmapjs.RowReturnIdentity
        End Function

        Public Function GetInspectionTypes() As List(Of SPCInspection.InspectionTypes)

            Dim sql As String
            Dim bmap_so As New BMappers(Of SPCInspection.InspectionTypes)
            Dim listso As New List(Of SPCInspection.InspectionTypes)

            sql = "SELECT  InspectionTypes.* FROM InspectionTypes"

            listso = bmap_so.GetInspectObject(sql)

            Return listso
        End Function

        Public Function GetTemplateId() As Integer
            Dim sql As String
            Dim InspectNum = New DataSet

            sql = "SELECT TOP 1 InspectionId" & vbCrLf &
            "FROM DefectMaster" & vbCrLf &
            "ORDER BY InspectionId DESC"

            If Not Util.FillSPCDataSet(InspectNum, "InspectNum", sql) Then
                Return 0
            End If

            If InspectNum.Tables(0).Rows.Count > 0 Then
                If IsNumeric(InspectNum.Tables(0).Rows(0)("InspectionId")) Then
                    Return Convert.ToInt64(InspectNum.Tables(0).Rows(0)("InspectionId")) + 1
                End If
                Return 0
            Else
                Return 0
            End If
        End Function

        Public Function InsertDefectImageById(ByVal file As System.Web.HttpPostedFile, ByVal DefectID As Integer) As Boolean
            Dim result As Boolean
            Dim returnint As Integer
            If Not file Is Nothing Then
                Dim image As Image = image.FromStream(file.InputStream())
                Dim filename As String = file.FileName
                Dim converter As New ImageConverter
                Dim bytes As Byte() = converter.ConvertTo(image, GetType(Byte()))
                Dim SQL As String
                Dim Internalcmd As SqlCommand

                SQL = "UPDATE DefectMaster" & vbCrLf &
                        "SET DefectImage = @DefectImage, DefectImage_Filename = @DefectImage_Filename" & vbCrLf &
                        "WHERE (DefectID = @DefectID)"
                Using connection As New SqlConnection(dlayer.InspectConnectionString())
                    Internalcmd = _DAOFactory.GetCommand(SQL, connection)

                    Internalcmd.Parameters.Add(_DAOFactory.Getparameter("@DefectImage", DbType.Binary))
                    Internalcmd.Parameters.Add(_DAOFactory.Getparameter("@DefectID", DbType.Int32))
                    Internalcmd.Parameters.Add(_DAOFactory.Getparameter("@DefectImage_Filename", DbType.String))
                    Internalcmd.Parameters("@DefectImage").Value = bytes
                    Internalcmd.Parameters("@DefectID").Value = DefectID
                    Internalcmd.Parameters("@DefectImage_Filename").Value = filename
                    Try
                        Internalcmd.Connection.Open()
                        returnint = Convert.ToInt32(Internalcmd.ExecuteNonQuery())
                        If returnint > 0 Then
                            Return True
                        End If
                    Catch ex As Exception

                        Return -1
                        Exit Function
                    End Try
                End Using


            End If

            Return result

        End Function
        Public Function SetReducedSamplesSize(ByVal lotsizenumber As Integer, ByVal AQLevel As Decimal) As List(Of SPCInspection.InspectionVaribles)
            Dim AQstring As String = AQLevel
            Dim Lotsizestring As Integer = lotsizenumber
            Dim SampleSize As Integer = 0
            Dim Acceptance As Integer = 0
            Dim RE As String = "0"
            Dim AC As String = "0"
            Dim LotSize As String = "0"

            If AQLevel = 1 Then
                If (lotsizenumber >= 1) And (lotsizenumber <= 90) Then
                    SampleSize = 2
                    If SampleSize > lotsizenumber Then
                        SampleSize = lotsizenumber
                    End If
                    Acceptance = 0
                ElseIf (lotsizenumber >= 91) And (lotsizenumber <= 150) Then
                    SampleSize = 3
                    Acceptance = 0
                ElseIf (lotsizenumber >= 151) And (lotsizenumber <= 280) Then
                    SampleSize = 5
                    Acceptance = 0
                ElseIf (lotsizenumber >= 281) And (lotsizenumber <= 500) Then
                    SampleSize = 8
                    Acceptance = 0
                ElseIf (lotsizenumber >= 501) And (lotsizenumber <= 1200) Then
                    SampleSize = 13
                    Acceptance = 1
                ElseIf (lotsizenumber >= 1201) And (lotsizenumber <= 3200) Then
                    SampleSize = 20
                    Acceptance = 1
                ElseIf (lotsizenumber >= 3201) And (lotsizenumber <= 35000) Then
                    SampleSize = 32
                    Acceptance = 2
                ElseIf (lotsizenumber >= 35001) And (lotsizenumber <= 150000) Then
                    SampleSize = 80
                    Acceptance = 4
                ElseIf (lotsizenumber >= 150001) And (lotsizenumber <= 500000) Then
                    SampleSize = 125
                    Acceptance = 5
                ElseIf (lotsizenumber >= 500001) Then
                    SampleSize = 200
                    Acceptance = 7
                End If
            End If
            If AQLevel = 1.5 Then
                If (lotsizenumber >= 1) And (lotsizenumber <= 90) Then
                    SampleSize = 2
                    If SampleSize > lotsizenumber Then
                        SampleSize = lotsizenumber
                    End If
                    Acceptance = 0
                ElseIf (lotsizenumber >= 91) And (lotsizenumber <= 150) Then
                    SampleSize = 3
                    Acceptance = 0
                ElseIf (lotsizenumber >= 151) And (lotsizenumber <= 280) Then
                    SampleSize = 5
                    Acceptance = 0
                ElseIf (lotsizenumber >= 281) And (lotsizenumber <= 500) Then
                    SampleSize = 8
                    Acceptance = 0
                ElseIf (lotsizenumber >= 501) And (lotsizenumber <= 1200) Then
                    SampleSize = 13
                    Acceptance = 1
                ElseIf (lotsizenumber >= 1201) And (lotsizenumber <= 3200) Then
                    SampleSize = 20
                    Acceptance = 2
                ElseIf (lotsizenumber >= 3201) And (lotsizenumber <= 35000) Then
                    SampleSize = 32
                    Acceptance = 3
                ElseIf (lotsizenumber >= 35001) And (lotsizenumber <= 150000) Then
                    SampleSize = 80
                    Acceptance = 5
                ElseIf (lotsizenumber >= 150001) And (lotsizenumber <= 500000) Then
                    SampleSize = 125
                    Acceptance = 7
                ElseIf (lotsizenumber >= 500001) Then
                    SampleSize = 200
                    Acceptance = 9
                End If
            End If
            If AQLevel = 2.5 Then
                If (lotsizenumber >= 1) And (lotsizenumber <= 90) Then
                    SampleSize = 2
                    If SampleSize > lotsizenumber Then
                        SampleSize = lotsizenumber
                    End If
                    Acceptance = 0
                ElseIf (lotsizenumber >= 91) And (lotsizenumber <= 150) Then
                    SampleSize = 3
                    Acceptance = 0
                ElseIf (lotsizenumber >= 151) And (lotsizenumber <= 280) Then
                    SampleSize = 5
                    Acceptance = 0
                ElseIf (lotsizenumber >= 281) And (lotsizenumber <= 500) Then
                    SampleSize = 8
                    Acceptance = 0
                ElseIf (lotsizenumber >= 501) And (lotsizenumber <= 1200) Then
                    SampleSize = 13
                    Acceptance = 2
                ElseIf (lotsizenumber >= 1201) And (lotsizenumber <= 3200) Then
                    SampleSize = 20
                    Acceptance = 3
                ElseIf (lotsizenumber >= 3201) And (lotsizenumber <= 35000) Then
                    SampleSize = 32
                    Acceptance = 4
                ElseIf (lotsizenumber >= 35001) And (lotsizenumber <= 150000) Then
                    SampleSize = 80
                    Acceptance = 7
                ElseIf (lotsizenumber >= 150001) And (lotsizenumber <= 500000) Then
                    SampleSize = 125
                    Acceptance = 9
                ElseIf (lotsizenumber >= 500001) Then
                    SampleSize = 200
                    Acceptance = 12
                End If

            End If
            If AQLevel = 4 Then
                If (lotsizenumber >= 1) And (lotsizenumber <= 90) Then
                    SampleSize = 2
                    If SampleSize > lotsizenumber Then
                        SampleSize = lotsizenumber
                    End If
                    Acceptance = 0
                ElseIf (lotsizenumber >= 91) And (lotsizenumber <= 150) Then
                    SampleSize = 3
                    Acceptance = 0
                ElseIf (lotsizenumber >= 151) And (lotsizenumber <= 280) Then
                    SampleSize = 5
                    Acceptance = 1
                ElseIf (lotsizenumber >= 281) And (lotsizenumber <= 500) Then
                    SampleSize = 8
                    Acceptance = 2
                ElseIf (lotsizenumber >= 501) And (lotsizenumber <= 1200) Then
                    SampleSize = 13
                    Acceptance = 3
                ElseIf (lotsizenumber >= 1201) And (lotsizenumber <= 3200) Then
                    SampleSize = 20
                    Acceptance = 4
                ElseIf (lotsizenumber >= 3201) And (lotsizenumber <= 35000) Then
                    SampleSize = 32
                    Acceptance = 5
                ElseIf (lotsizenumber >= 35001) And (lotsizenumber <= 150000) Then
                    SampleSize = 80
                    Acceptance = 9
                ElseIf (lotsizenumber >= 150001) And (lotsizenumber <= 500000) Then
                    SampleSize = 125
                    Acceptance = 12
                ElseIf (lotsizenumber >= 500001) Then
                    SampleSize = 200
                    Acceptance = 12
                End If

            End If
            If AQLevel = 100 Then

                SampleSize = lotsizenumber

                Acceptance = lotsizenumber

            End If
            AC = Acceptance.ToString()
            RE = Convert.ToString(CType(AC, Integer) + 1)
            LotSize = lotsizenumber.ToString()

            Dim returnlist As New List(Of SPCInspection.InspectionVaribles)
            Dim jser As New JavaScriptSerializer

            returnlist.Add(New SPCInspection.InspectionVaribles With {.AC = AC, .RE = RE, .LotSize = LotSize, .Acceptance = Acceptance, .SampleSize = SampleSize})
            Return returnlist

        End Function

        Public Function SetTightSamplesSize(ByVal lotsizenumber As Integer, ByVal AQLevel As Decimal) As List(Of SPCInspection.InspectionVaribles)
            Dim AQstring As String = AQLevel
            Dim Lotsizestring As Integer = lotsizenumber
            Dim SampleSize As Integer = 0
            Dim Acceptance As Integer = 0
            Dim RE As String = "0"
            Dim AC As String = "0"
            Dim LotSize As String = "0"

            If AQLevel = 1 Then
                If (lotsizenumber >= 1) And (lotsizenumber <= 8) Then
                    SampleSize = 3
                    If SampleSize > lotsizenumber Then
                        SampleSize = lotsizenumber
                    End If
                    Acceptance = 0
                ElseIf (lotsizenumber >= 9) And (lotsizenumber <= 15) Then
                    SampleSize = 5
                    Acceptance = 0
                ElseIf (lotsizenumber >= 16) And (lotsizenumber <= 25) Then
                    SampleSize = 8
                    Acceptance = 0
                ElseIf (lotsizenumber >= 26) And (lotsizenumber <= 50) Then
                    SampleSize = 13
                    Acceptance = 0
                ElseIf (lotsizenumber >= 51) And (lotsizenumber <= 90) Then
                    SampleSize = 20
                    Acceptance = 0
                ElseIf (lotsizenumber >= 91) And (lotsizenumber <= 150) Then
                    SampleSize = 32
                    Acceptance = 0
                ElseIf (lotsizenumber >= 151) And (lotsizenumber <= 280) Then
                    SampleSize = 50
                    Acceptance = 0
                ElseIf (lotsizenumber >= 281) And (lotsizenumber <= 500) Then
                    SampleSize = 80
                    Acceptance = 1
                ElseIf (lotsizenumber >= 501) And (lotsizenumber <= 1200) Then
                    SampleSize = 125
                    Acceptance = 2
                ElseIf (lotsizenumber >= 1201) And (lotsizenumber <= 3200) Then
                    SampleSize = 200
                    Acceptance = 3
                ElseIf (lotsizenumber >= 3201) And (lotsizenumber <= 10000) Then
                    SampleSize = 315
                    Acceptance = 5
                ElseIf (lotsizenumber >= 10001) And (lotsizenumber <= 35000) Then
                    SampleSize = 500
                    Acceptance = 8
                ElseIf (lotsizenumber >= 35001) And (lotsizenumber <= 150000) Then
                    SampleSize = 800
                    Acceptance = 13
                ElseIf (lotsizenumber >= 150001) And (lotsizenumber <= 500000) Then
                    SampleSize = 1250
                    Acceptance = 18
                ElseIf (lotsizenumber >= 500001) Then
                    SampleSize = 2000
                    Acceptance = 18
                End If
            End If
            If AQLevel = 1.5 Then
                If (lotsizenumber >= 1) And (lotsizenumber <= 8) Then
                    SampleSize = 3
                    If SampleSize > lotsizenumber Then
                        SampleSize = lotsizenumber
                    End If
                    Acceptance = 0
                ElseIf (lotsizenumber >= 9) And (lotsizenumber <= 15) Then
                    SampleSize = 5
                    Acceptance = 0
                ElseIf (lotsizenumber >= 16) And (lotsizenumber <= 25) Then
                    SampleSize = 8
                    Acceptance = 0
                ElseIf (lotsizenumber >= 26) And (lotsizenumber <= 50) Then
                    SampleSize = 13
                    Acceptance = 0
                ElseIf (lotsizenumber >= 51) And (lotsizenumber <= 90) Then
                    SampleSize = 20
                    Acceptance = 0
                ElseIf (lotsizenumber >= 91) And (lotsizenumber <= 150) Then
                    SampleSize = 32
                    Acceptance = 0
                ElseIf (lotsizenumber >= 151) And (lotsizenumber <= 280) Then
                    SampleSize = 50
                    Acceptance = 1
                ElseIf (lotsizenumber >= 281) And (lotsizenumber <= 500) Then
                    SampleSize = 80
                    Acceptance = 2
                ElseIf (lotsizenumber >= 501) And (lotsizenumber <= 1200) Then
                    SampleSize = 125
                    Acceptance = 3
                ElseIf (lotsizenumber >= 1201) And (lotsizenumber <= 3200) Then
                    SampleSize = 200
                    Acceptance = 5
                ElseIf (lotsizenumber >= 3201) And (lotsizenumber <= 10000) Then
                    SampleSize = 315
                    Acceptance = 8
                ElseIf (lotsizenumber >= 10001) And (lotsizenumber <= 35000) Then
                    SampleSize = 500
                    Acceptance = 12
                ElseIf (lotsizenumber >= 35001) And (lotsizenumber <= 150000) Then
                    SampleSize = 800
                    Acceptance = 13
                ElseIf (lotsizenumber >= 150001) And (lotsizenumber <= 500000) Then
                    SampleSize = 1250
                    Acceptance = 18
                ElseIf (lotsizenumber >= 500001) Then
                    SampleSize = 2000
                    Acceptance = 18
                End If

            End If
            If AQLevel = 2.5 Then
                If (lotsizenumber >= 1) And (lotsizenumber <= 8) Then
                    SampleSize = 3
                    If SampleSize > lotsizenumber Then
                        SampleSize = lotsizenumber
                    End If
                    Acceptance = 0
                ElseIf (lotsizenumber >= 9) And (lotsizenumber <= 15) Then
                    SampleSize = 5
                    Acceptance = 0
                ElseIf (lotsizenumber >= 16) And (lotsizenumber <= 25) Then
                    SampleSize = 8
                    Acceptance = 0
                ElseIf (lotsizenumber >= 26) And (lotsizenumber <= 50) Then
                    SampleSize = 13
                    Acceptance = 0
                ElseIf (lotsizenumber >= 51) And (lotsizenumber <= 90) Then
                    SampleSize = 20
                    Acceptance = 0
                ElseIf (lotsizenumber >= 91) And (lotsizenumber <= 150) Then
                    SampleSize = 32
                    Acceptance = 1
                ElseIf (lotsizenumber >= 151) And (lotsizenumber <= 280) Then
                    SampleSize = 50
                    Acceptance = 2
                ElseIf (lotsizenumber >= 281) And (lotsizenumber <= 500) Then
                    SampleSize = 80
                    Acceptance = 3
                ElseIf (lotsizenumber >= 501) And (lotsizenumber <= 1200) Then
                    SampleSize = 125
                    Acceptance = 5
                ElseIf (lotsizenumber >= 1201) And (lotsizenumber <= 3200) Then
                    SampleSize = 200
                    Acceptance = 8
                ElseIf (lotsizenumber >= 3201) And (lotsizenumber <= 10000) Then
                    SampleSize = 315
                    Acceptance = 12
                ElseIf (lotsizenumber >= 10001) And (lotsizenumber <= 35000) Then
                    SampleSize = 500
                    Acceptance = 18
                ElseIf (lotsizenumber >= 35001) And (lotsizenumber <= 150000) Then
                    SampleSize = 800
                    Acceptance = 18
                ElseIf (lotsizenumber >= 150001) And (lotsizenumber <= 500000) Then
                    SampleSize = 1250
                    Acceptance = 18
                ElseIf (lotsizenumber >= 500001) Then
                    SampleSize = 2000
                    Acceptance = 18
                End If
            End If
            If AQLevel = 4 Then
                If (lotsizenumber >= 1) And (lotsizenumber <= 8) Then
                    SampleSize = 3
                    If SampleSize > lotsizenumber Then
                        SampleSize = lotsizenumber
                    End If
                    Acceptance = 0
                ElseIf (lotsizenumber >= 9) And (lotsizenumber <= 15) Then
                    SampleSize = 5
                    Acceptance = 0
                ElseIf (lotsizenumber >= 16) And (lotsizenumber <= 25) Then
                    SampleSize = 8
                    Acceptance = 0
                ElseIf (lotsizenumber >= 26) And (lotsizenumber <= 50) Then
                    SampleSize = 13
                    Acceptance = 0
                ElseIf (lotsizenumber >= 51) And (lotsizenumber <= 90) Then
                    SampleSize = 20
                    Acceptance = 1
                ElseIf (lotsizenumber >= 91) And (lotsizenumber <= 150) Then
                    SampleSize = 32
                    Acceptance = 2
                ElseIf (lotsizenumber >= 151) And (lotsizenumber <= 280) Then
                    SampleSize = 50
                    Acceptance = 3
                ElseIf (lotsizenumber >= 281) And (lotsizenumber <= 500) Then
                    SampleSize = 80
                    Acceptance = 5
                ElseIf (lotsizenumber >= 501) And (lotsizenumber <= 1200) Then
                    SampleSize = 125
                    Acceptance = 8
                ElseIf (lotsizenumber >= 1201) And (lotsizenumber <= 3200) Then
                    SampleSize = 200
                    Acceptance = 12
                ElseIf (lotsizenumber >= 3201) And (lotsizenumber <= 10000) Then
                    SampleSize = 315
                    Acceptance = 18
                ElseIf (lotsizenumber >= 10001) And (lotsizenumber <= 35000) Then
                    SampleSize = 500
                    Acceptance = 18
                ElseIf (lotsizenumber >= 35001) And (lotsizenumber <= 150000) Then
                    SampleSize = 800
                    Acceptance = 18
                ElseIf (lotsizenumber >= 150001) And (lotsizenumber <= 500000) Then
                    SampleSize = 1250
                    Acceptance = 18
                ElseIf (lotsizenumber >= 500001) Then
                    SampleSize = 2000
                    Acceptance = 18
                End If

            End If
            If AQLevel = 100 Then

                SampleSize = lotsizenumber

                Acceptance = lotsizenumber

            End If
            AC = Acceptance.ToString()
            RE = Convert.ToString(CType(AC, Integer) + 1)

            LotSize = lotsizenumber.ToString()

            Dim returnlist As New List(Of SPCInspection.InspectionVaribles)
            Dim jser As New JavaScriptSerializer

            returnlist.Add(New SPCInspection.InspectionVaribles With {.AC = AC, .RE = RE, .LotSize = LotSize, .Acceptance = Acceptance, .SampleSize = SampleSize})
            Return returnlist
        End Function

        Public Function SetSampleSize(ByVal lotsizenumber As Integer, ByVal AQLevel As Decimal) As List(Of SPCInspection.InspectionVaribles)
            Dim AQstring As String = AQLevel
            Dim Lotsizestring As Integer = lotsizenumber
            Dim SampleSize As Integer = 0
            Dim Acceptance As Integer = 0
            Dim RE As String = "0"
            Dim AC As String = "0"
            Dim LotSize As String = "0"

            If AQLevel = 1 Then
                If (lotsizenumber >= 1) And (lotsizenumber <= 150) Then
                    SampleSize = 13
                    If SampleSize > lotsizenumber Then
                        SampleSize = lotsizenumber
                    End If
                    Acceptance = 0
                ElseIf (lotsizenumber >= 151) And (lotsizenumber <= 500) Then
                    SampleSize = 50
                    Acceptance = 1
                ElseIf (lotsizenumber >= 501) And (lotsizenumber <= 1200) Then
                    SampleSize = 80
                    Acceptance = 2
                ElseIf (lotsizenumber >= 1201) And (lotsizenumber <= 3200) Then
                    SampleSize = 125
                    Acceptance = 3
                ElseIf (lotsizenumber >= 3201) And (lotsizenumber <= 10000) Then
                    SampleSize = 200
                    Acceptance = 5
                ElseIf (lotsizenumber >= 10001) And (lotsizenumber <= 35000) Then
                    SampleSize = 315
                    Acceptance = 7
                ElseIf (lotsizenumber >= 35001) And (lotsizenumber <= 150000) Then
                    SampleSize = 500
                    Acceptance = 10
                ElseIf (lotsizenumber >= 150001) And (lotsizenumber <= 500000) Then
                    SampleSize = 800
                    Acceptance = 14
                ElseIf (lotsizenumber >= 500001) Then
                    SampleSize = 1250
                    Acceptance = 21
                End If
            End If
            If AQLevel = 1.5 Then
                If (lotsizenumber >= 1) And (lotsizenumber <= 90) Then
                    SampleSize = 8
                    If SampleSize > lotsizenumber Then
                        SampleSize = lotsizenumber
                    End If
                    Acceptance = 0
                ElseIf (lotsizenumber >= 91) And (lotsizenumber <= 280) Then
                    SampleSize = 32
                    Acceptance = 1
                ElseIf (lotsizenumber >= 281) And (lotsizenumber <= 500) Then
                    SampleSize = 50
                    Acceptance = 2
                ElseIf (lotsizenumber >= 501) And (lotsizenumber <= 1200) Then
                    SampleSize = 80
                    Acceptance = 3
                ElseIf (lotsizenumber >= 1201) And (lotsizenumber <= 3200) Then
                    SampleSize = 125
                    Acceptance = 5
                ElseIf (lotsizenumber >= 3201) And (lotsizenumber <= 10000) Then
                    SampleSize = 200
                    Acceptance = 7
                ElseIf (lotsizenumber >= 10001) And (lotsizenumber <= 35000) Then
                    SampleSize = 315
                    Acceptance = 10
                ElseIf (lotsizenumber >= 35001) And (lotsizenumber <= 150000) Then
                    SampleSize = 500
                    Acceptance = 14
                ElseIf (lotsizenumber >= 150001) Then
                    SampleSize = 800
                    Acceptance = 21
                End If
            End If
            If AQLevel = 2.5 Then
                If (lotsizenumber >= 1) And (lotsizenumber <= 50) Then
                    SampleSize = 5
                    If SampleSize > lotsizenumber Then
                        SampleSize = lotsizenumber
                    End If
                    Acceptance = 0
                ElseIf (lotsizenumber >= 51) And (lotsizenumber <= 150) Then
                    SampleSize = 20
                    Acceptance = 1
                ElseIf (lotsizenumber >= 151) And (lotsizenumber <= 280) Then
                    SampleSize = 32
                    Acceptance = 2
                ElseIf (lotsizenumber >= 281) And (lotsizenumber <= 500) Then
                    SampleSize = 50
                    Acceptance = 3
                ElseIf (lotsizenumber >= 501) And (lotsizenumber <= 1200) Then
                    SampleSize = 80
                    Acceptance = 5
                ElseIf (lotsizenumber >= 1201) And (lotsizenumber <= 3200) Then
                    SampleSize = 125
                    Acceptance = 7
                ElseIf (lotsizenumber >= 3201) And (lotsizenumber <= 10000) Then
                    SampleSize = 200
                    Acceptance = 10
                ElseIf (lotsizenumber >= 10001) And (lotsizenumber <= 35000) Then
                    SampleSize = 315
                    Acceptance = 14
                ElseIf (lotsizenumber >= 35001) Then
                    SampleSize = 500
                    Acceptance = 21
                End If
            End If
            If AQLevel = 4 Then
                If (lotsizenumber >= 1) And (lotsizenumber <= 50) Then
                    SampleSize = 5
                    If SampleSize > lotsizenumber Then
                        SampleSize = lotsizenumber
                    End If
                    Acceptance = 0
                ElseIf (lotsizenumber >= 51) And (lotsizenumber <= 90) Then
                    SampleSize = 13
                    Acceptance = 1
                ElseIf (lotsizenumber >= 91) And (lotsizenumber <= 150) Then
                    SampleSize = 20
                    Acceptance = 2
                ElseIf (lotsizenumber >= 151) And (lotsizenumber <= 280) Then
                    SampleSize = 32
                    Acceptance = 3
                ElseIf (lotsizenumber >= 281) And (lotsizenumber <= 500) Then
                    SampleSize = 50
                    Acceptance = 5
                ElseIf (lotsizenumber >= 501) And (lotsizenumber <= 1200) Then
                    SampleSize = 80
                    Acceptance = 7
                ElseIf (lotsizenumber >= 1201) And (lotsizenumber <= 3200) Then
                    SampleSize = 125
                    Acceptance = 10
                ElseIf (lotsizenumber >= 3201) And (lotsizenumber <= 10000) Then
                    SampleSize = 200
                    Acceptance = 14
                ElseIf (lotsizenumber >= 10001) Then
                    SampleSize = 315
                    Acceptance = 21
                End If
            End If
            If AQLevel = 100 Then

                SampleSize = lotsizenumber

                Acceptance = lotsizenumber

            End If
            AC = Acceptance.ToString()
            RE = Convert.ToString(CType(AC, Integer) + 1)
            LotSize = lotsizenumber.ToString()

            Dim returnlist As New List(Of SPCInspection.InspectionVaribles)
            Dim jser As New JavaScriptSerializer

            returnlist.Add(New SPCInspection.InspectionVaribles With {.AC = AC, .RE = RE, .LotSize = LotSize, .Acceptance = Acceptance, .SampleSize = SampleSize})
            Return returnlist
        End Function
        Public Function SetSampleSize_old(ByVal _lotsize As String, ByVal _AQLevel As String) As List(Of SPCInspection.InspectionVaribles)
            Dim AQstring As String = _AQLevel
            Dim Lotsizestring As String = _lotsize
            Dim SampleSize As Integer = 0
            Dim Acceptance As Integer = 0
            Dim RE As String = "0"
            Dim AC As String = "0"
            Dim LotSize As String = "0"

            If IsNumeric(AQstring) = True And IsNumeric(Lotsizestring) = True Then

                Dim AQLevel As Decimal = CType(AQstring, Decimal)
                Dim lotsizenumber As Integer = CType(Lotsizestring, Integer)
                If lotsizenumber < 600000 Then
                    If (lotsizenumber >= 2) And (lotsizenumber <= 8) Then
                        SampleSize = 2
                        If AQLevel = 4 Then
                            Acceptance = 0
                        End If
                        If AQLevel = 1 Then
                            SampleSize = lotsizenumber.ToString()
                            Acceptance = lotsizenumber
                        End If
                        If AQLevel = 1.5 Then
                            Acceptance = 0
                        End If
                        If AQLevel = 2.5 Then
                            Acceptance = 0
                        End If
                    End If
                    If (lotsizenumber >= 9) And (lotsizenumber <= 15) Then
                        SampleSize = 3
                        If AQLevel = 4 Then
                            Acceptance = 0
                        End If
                        If AQLevel = 1 Then
                            SampleSize = lotsizenumber.ToString()
                            Acceptance = lotsizenumber
                        End If
                        If AQLevel = 1.5 Then
                            Acceptance = 0
                        End If
                        If AQLevel = 2.5 Then
                            Acceptance = 0
                        End If
                    End If
                    If (lotsizenumber >= 16) And (lotsizenumber <= 25) Then
                        SampleSize = 5
                        If AQLevel = 4 Then
                            Acceptance = 0
                        End If
                        If AQLevel = 1 Then
                            SampleSize = lotsizenumber.ToString()
                            Acceptance = lotsizenumber
                        End If
                        If AQLevel = 1.5 Then
                            Acceptance = 0
                        End If
                        If AQLevel = 2.5 Then
                            Acceptance = 0
                        End If
                    End If
                    If (lotsizenumber >= 26) And (lotsizenumber <= 50) Then
                        SampleSize = 8
                        If AQLevel = 4 Then
                            Acceptance = 1
                        End If
                        If AQLevel = 1 Then
                            SampleSize = lotsizenumber.ToString()
                            Acceptance = lotsizenumber
                        End If
                        If AQLevel = 1.5 Then
                            Acceptance = 0
                        End If
                        If AQLevel = 2.5 Then
                            Acceptance = 1
                        End If
                    End If
                    If (lotsizenumber >= 51) And (lotsizenumber <= 90) Then
                        SampleSize = 13
                        If AQLevel = 4 Then
                            Acceptance = 1
                        End If
                        If AQLevel = 1 Then
                            SampleSize = lotsizenumber.ToString()
                            Acceptance = lotsizenumber
                        End If
                        If AQLevel = 1.5 Then
                            Acceptance = 0
                        End If
                        If AQLevel = 2.5 Then
                            Acceptance = 1
                        End If
                    End If
                    If (lotsizenumber >= 91) And (lotsizenumber <= 150) Then
                        SampleSize = 20
                        If AQLevel = 4 Then
                            Acceptance = 2
                        End If
                        If AQLevel = 1 Then
                            SampleSize = lotsizenumber.ToString()
                            Acceptance = lotsizenumber
                        End If
                        If AQLevel = 1.5 Then
                            Acceptance = 0
                        End If
                        If AQLevel = 2.5 Then
                            Acceptance = 1
                        End If
                    End If
                    If (lotsizenumber >= 151) And (lotsizenumber <= 280) Then
                        SampleSize = 32
                        If AQLevel = 4 Then
                            Acceptance = 3
                        End If
                        If AQLevel = 1 Then
                            SampleSize = lotsizenumber.ToString()
                            Acceptance = lotsizenumber
                        End If
                        If AQLevel = 1.5 Then
                            Acceptance = 1
                        End If
                        If AQLevel = 2.5 Then
                            Acceptance = 2
                        End If
                    End If
                    If (lotsizenumber >= 281) And (lotsizenumber <= 500) Then
                        SampleSize = 50
                        If AQLevel = 4 Then
                            Acceptance = 4
                        End If
                        If AQLevel = 1 Then
                            SampleSize = lotsizenumber.ToString()
                            Acceptance = lotsizenumber
                        End If
                        If AQLevel = 1.5 Then
                            Acceptance = 2
                        End If
                        If AQLevel = 2.5 Then
                            Acceptance = 3
                        End If
                    End If
                    If (lotsizenumber >= 501) And (lotsizenumber <= 1200) Then
                        SampleSize = 80
                        If AQLevel = 4 Then
                            Acceptance = 7
                        End If
                        If AQLevel = 1 Then
                            SampleSize = lotsizenumber.ToString()
                            Acceptance = lotsizenumber
                        End If
                        If AQLevel = 1.5 Then
                            Acceptance = 3
                        End If
                        If AQLevel = 2.5 Then
                            Acceptance = 5
                        End If
                    End If
                    If (lotsizenumber >= 1201) And (lotsizenumber <= 3200) Then
                        SampleSize = 125
                        If AQLevel = 4 Then
                            Acceptance = 10
                        End If
                        If AQLevel = 1 Then
                            SampleSize = lotsizenumber.ToString()
                            Acceptance = lotsizenumber
                        End If
                        If AQLevel = 1.5 Then
                            Acceptance = 5
                        End If
                        If AQLevel = 2.5 Then
                            Acceptance = 7
                        End If
                    End If
                    If (lotsizenumber >= 3201) And (lotsizenumber <= 10000) Then
                        SampleSize = 200
                        If AQLevel = 4 Then
                            Acceptance = 14
                        End If
                        If AQLevel = 1 Then
                            SampleSize = lotsizenumber.ToString()
                            Acceptance = lotsizenumber
                        End If
                        If AQLevel = 1.5 Then
                            Acceptance = 7
                        End If
                        If AQLevel = 2.5 Then
                            Acceptance = 10
                        End If
                    End If
                    If (lotsizenumber >= 10001) And (lotsizenumber <= 35000) Then
                        SampleSize = 315
                        If AQLevel = 4 Then
                            Acceptance = 21
                        End If
                        If AQLevel = 1 Then
                            SampleSize = lotsizenumber.ToString()
                            Acceptance = lotsizenumber
                        End If
                        If AQLevel = 1.5 Then
                            Acceptance = 10
                        End If
                        If AQLevel = 2.5 Then
                            Acceptance = 14
                        End If
                    End If
                    If (lotsizenumber >= 35001) And (lotsizenumber <= 150000) Then
                        SampleSize = 500
                        If AQLevel = 4 Then
                            Acceptance = 21
                        End If
                        If AQLevel = 1 Then
                            SampleSize = lotsizenumber.ToString()
                            Acceptance = lotsizenumber
                        End If
                        If AQLevel = 1.5 Then
                            Acceptance = 14
                        End If
                        If AQLevel = 2.5 Then
                            Acceptance = 21
                        End If
                    End If
                    If (lotsizenumber >= 150001) And (lotsizenumber <= 500000) Then
                        SampleSize = 800
                        If AQLevel = 4 Then
                            Acceptance = 21
                        End If
                        If AQLevel = 1 Then
                            SampleSize = lotsizenumber.ToString()
                            Acceptance = lotsizenumber
                        End If
                        If AQLevel = 1.5 Then
                            Acceptance = 21
                        End If
                        If AQLevel = 2.5 Then
                            Acceptance = 21
                        End If
                    End If
                    If (lotsizenumber >= 500001) Then
                        SampleSize = 1250
                        If AQLevel = 4 Then
                            Acceptance = 21
                        End If
                        If AQLevel = 1 Then
                            SampleSize = lotsizenumber.ToString()
                            Acceptance = lotsizenumber
                        End If
                        If AQLevel = 1.5 Then
                            Acceptance = 21
                        End If
                        If AQLevel = 2.5 Then
                            Acceptance = 21
                        End If
                    End If

                    AC = Acceptance.ToString()
                    RE = Convert.ToString(CType(AC, Integer) + 1)
                    LotSize = lotsizenumber.ToString()

                Else

                End If

            End If


            Dim returnlist As New List(Of SPCInspection.InspectionVaribles)
            Dim jser As New JavaScriptSerializer

            returnlist.Add(New SPCInspection.InspectionVaribles With {.AC = AC, .RE = RE, .LotSize = LotSize, .Acceptance = Acceptance, .SampleSize = SampleSize})
            Return returnlist

        End Function

        Public Function GetPDMProductSpecs(ByVal DataNo As String) As List(Of SPCInspection.PDMProductSpecs)
            Dim con As New SqlConnection(dlayer.PDMConnectionString())
            Dim cmd As SqlCommand = con.CreateCommand()
            Dim bmappps As New BMappers(Of SPCInspection.PDMProductSpecs)
            Dim listpps As New List(Of SPCInspection.PDMProductSpecs)
            Try
                Using con
                    con.Open()
                    Using cmd
                        cmd = New SqlCommand("GetSTCProductSpecs2", con)
                        cmd.CommandType = CommandType.StoredProcedure
                        cmd.Parameters.Add("@DATANO", SqlDbType.VarChar).Direction = ParameterDirection.Input
                        cmd.Parameters("@DATANO").Value = DataNo

                        cmd.CommandTimeout = 5000

                        listpps = bmappps.GetSpcSP(cmd)
                    End Using
                End Using
            Catch ex As Exception

            End Try

            Return listpps

        End Function
        Public Function LoadDefectSelection(ByVal InspectionId As String) As String

            Dim bmapds As New BMappers(Of SPCInspection.DefectSelection)
            Dim listds As New List(Of SPCInspection.DefectSelection)
            Dim listret As New List(Of SPCInspection.DefectSelection)
            Dim jser As New JavaScriptSerializer()
            Dim sql As String = "SELECT DefectID, convert(varchar(25) ,DefectTime , 100) as DefectTime, DefectDesc, InspectionJobSummaryId FROM  DefectMaster WHERE (InspectionJobSummaryId = " & InspectionId & ")"

            listds = bmapds.GetInspectObject(sql)

            If listds.Count > 0 Then
                Dim listar = listds.ToArray()
                listds.Add(New SPCInspection.DefectSelection With {.DefectId = -1, .DefectTime = "", .DefectDesc = "SELECT OPTION"})
                listret = (From x In listds Select x Order By x.DefectId Descending).ToList()
            Else
                listret.Add(New SPCInspection.DefectSelection With {.DefectId = -1, .DefectTime = "", .DefectDesc = "NO DEFECTS FOUND"})
            End If

            Return jser.Serialize(listret)
        End Function
        Public Function LoadWorkOrderSelection(ByVal PassCID As String) As String
            Dim bmapis As New BMappers(Of SPCInspection.InspectionJobSummary)
            Dim listis As New List(Of SPCInspection.InspectionJobSummary)
            Dim listret As New List(Of SPCInspection.InspectionJobSummary)
            Dim jser As New JavaScriptSerializer()
            Dim DefaultId As Integer = 1
            Dim sql As String = "SELECT ijs.id, ijs.JobNumber, ijs.JobType, ijs.ItemFailCount, ijs.TemplateId, tn.Name, ijs.AQL_Level, ijs.Standard, ijs.SampleSize, ijs.RejectLimiter, convert(varchar(25), ijs.Inspection_Started, 100) as Inspection_StartedString FROM InspectionJobSummary ijs LEFT OUTER JOIN TemplateName tn ON ijs.TemplateId = tn.TemplateId " & vbCrLf &
                                    "WHERE (Inspection_Started >= GETDATE() - 2) AND (LEN(JobNumber) > 3) AND (CID = '" & PassCID & "')"
            listis = bmapis.GetInspectObject(sql)

            If listis.Count > 0 Then
                Dim listar = listis.ToArray()
                DefaultId = listar(listar.Length - 1).id
                listis.Add(New SPCInspection.InspectionJobSummary With {.id = -1, .JobNumber = "SELECT OPTION"})
                listret = (From x In listis Select x Order By x.id Descending).ToList()
            End If

            Return jser.Serialize(listret)

        End Function

        Public Function GetItemInfo(ByVal DataNo As String) As List(Of SPCInspection.InspectionItemInfo)
            Dim con As New SqlConnection(dlayer.InspectConnectionString())
            Dim cmd As SqlCommand = con.CreateCommand()
            Dim bmappps As New BMappers(Of SPCInspection.InspectionItemInfo)
            Dim listpps As New List(Of SPCInspection.InspectionItemInfo)
            Try
                Using con
                    con.Open()
                    Using cmd
                        cmd = New SqlCommand("SP_AS400_GetItemInfo", con)
                        cmd.CommandType = CommandType.StoredProcedure
                        cmd.Parameters.Add("@DATANUMBER", SqlDbType.VarChar).Direction = ParameterDirection.Input
                        cmd.Parameters("@DATANUMBER").Value = DataNo

                        cmd.CommandTimeout = 5000

                        listpps = bmappps.GetSpcSP(cmd)
                    End Using
                End Using
            Catch ex As Exception
                Dim UTIL As New Utilities
                UTIL.newlogobj.application_name = "SP_AS400_GetItemInfo"
                UTIL.newlogobj.date_added = Date.Now
                UTIL.newlogobj.Target = "USER-Manual Spec Entry"
                UTIL.newlogobj.type = "Stored Procedure Access-GetItemInfo"
                UTIL.newlogobj.Message = ex.Message
            End Try

            Return listpps

        End Function

        Public Function GetInteriorSpecs(ByVal WorkOrder As String, ByVal DataNo As String) As List(Of SPCInspection.InspectProductSpec)
            Dim con As New SqlConnection(dlayer.InspectConnectionString())
            Dim cmd As SqlCommand = con.CreateCommand()
            Dim bmappps As New BMappers(Of SPCInspection.InteriorSpecs)
            Dim ProductSpecscache As New List(Of SPCInspection.InspectProductSpec)
            Dim listpps As New List(Of SPCInspection.InteriorSpecs)
            Dim listspecname As New List(Of String)(New String() {"Finished Length", "Return", "Return Snap/Panel", "Factory Snap/Panel", "Master Snap/Panel", "Board Width", "Return Left", "Return Right", "FW + BB Overall", "FW + BB Per Panel", "Ordered Width", "Width", "Length", "Finished Drop", "Bolster Width", "Diameter", "Finished Width", "Side Drop", "Foot Drop"})

            Try
                Using con
                    con.Open()
                    Using cmd
                        cmd = New SqlCommand("SP_AS400_GetInteriorSpecs", con)
                        cmd.CommandType = CommandType.StoredProcedure
                        cmd.Parameters.Add("@WORKORDER", SqlDbType.VarChar).Direction = ParameterDirection.Input
                        cmd.Parameters("@WORKORDER").Value = WorkOrder

                        cmd.CommandTimeout = 5000

                        listpps = bmappps.GetSpcSP(cmd)
                    End Using
                End Using
            Catch ex As Exception
                Dim UTIL As New Utilities
                UTIL.newlogobj.application_name = "SP_AS400_GetInteriorSpecs"
                UTIL.newlogobj.date_added = Date.Now
                UTIL.newlogobj.Target = "USER-as400 spec retrieval"
                UTIL.newlogobj.type = "Stored Procedure Access-GetInteriorSpecs"
                UTIL.newlogobj.Message = ex.Message
                UTIL.Log()
            End Try
            Dim cnt As Integer = 0
            For Each item In listpps
                Dim splitarr = item.Description.Split(":")
                If splitarr.Count > 1 Then
                    Dim fieldname As String = splitarr(0).Trim.ToUpper()
                    Dim specValue As Decimal
                    If IsNumeric(splitarr(1).Trim) = True And splitarr(1).IndexOf(",") = -1 Then
                        specValue = Convert.ToDecimal(splitarr(1).Trim)

                        Dim matchcnt = (From v In listspecname Where v.ToString.Trim.ToUpper() = fieldname Select v).Count()
                        If matchcnt > 0 Then
                            cnt += 1
                            ProductSpecscache.Add(New SPCInspection.InspectProductSpec With {.DataNo = DataNo, .GlobalSpec = True, .Lower_Spec_Value = -2, .Upper_Spec_Value = 2, .Measured_Value = specValue, .POM_Row = cnt, .Spec_Description = fieldname, .value = specValue})
                        End If
                    End If
                End If
            Next

            Return ProductSpecscache

        End Function

        Public Function Getas400UnitCost(ByVal DataNo As String) As List(Of SPCInspection.UnitCost)
            Dim con As New SqlConnection(dlayer.InspectConnectionString())
            Dim cmd As SqlCommand = con.CreateCommand()
            Dim listso As New List(Of SPCInspection.UnitCost)
            Dim bmap As New BMappers(Of SPCInspection.UnitCost)
            Dim retobj As Double = 0
            Try
                Using con
                    con.Open()
                    Using cmd
                        cmd = New SqlCommand("SP_AS400_GetUnitCost_3", con)
                        cmd.CommandType = CommandType.StoredProcedure
                        cmd.Parameters.Add("@DATANUMBER", SqlDbType.VarChar, 50).Direction = ParameterDirection.Input
                        cmd.Parameters("@DATANUMBER").Value = DataNo

                        cmd.CommandTimeout = 4000

                        listso = bmap.GetSpcSP(cmd)
                    End Using
                End Using
            Catch ex As Exception

            End Try

            Return listso

        End Function

        Public Function GetLocalUnitCost(ByVal DataNo As String, ByVal CID As String) As Double
            Dim listso As New List(Of SingleObject)
            Dim bmapso As New BMappers(Of SingleObject)
            Dim retobj As Double

            listso = bmapso.GetInspectObject("SELECT TOP(1) UnitCost as Object1 FROM InspectionJobSummary WHERE DataNo = '" & DataNo & "' AND CID = '" & CID & "' AND UnitCost > 0")

            If listso.Count > 0 Then
                retobj = listso.ToArray()(0).Object1
            End If

            Return retobj
        End Function

        Public Function GetDefectCountByType(ByVal JobSummaryId As String, ByVal DefectClass As String) As Integer
            Dim listso1 As New List(Of SingleObject)
            Dim bmapso As New BMappers(Of SingleObject)
            Dim returnint As Integer = 0
            Dim countstring As String = "SELECT COUNT(DefectID) AS Object1 FROM DefectMaster WHERE (DefectMaster.InspectionJobSummaryId = " & JobSummaryId & ") AND (DefectMaster.DefectClass = '" & DefectClass & "')"
            listso1 = bmapso.GetInspectObject(countstring)
            If listso1.Count > 0 Then
                returnint = CType(listso1.ToArray()(0).Object1, Integer)
            End If

            Return returnint

        End Function

    End Class


End Namespace
