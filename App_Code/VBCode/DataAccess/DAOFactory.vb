Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Data
Imports System.Reflection


Namespace core


    Public Class DAOFactory


        'Public Shared corestring As String
        'Public Shared coreReader As SqlDataReader
        'Public Shared corecmdBuilder As SqlCommandBuilder
        'Public Shared corecmd As SqlCommand
        'Public Shared coreConnection As SqlConnection
        'Public Shared coreAdapter As SqlDataAdapter
        'Public Shared coreDatatable As DataTable
        'Public Shared coreNewRow As DataRow

        Public Property corestring As String
        Public Property coreReader As SqlDataReader
        Public Property corecmdBuilder As SqlCommandBuilder
        Public Property corecmd As SqlCommand
        Public Property coreConnection As SqlConnection
        Public coreAdapter As SqlDataAdapter
        Public coreDatatable As DataTable
        Public coreNewRow As DataRow


        Public Property dl As New dlayer

        Dim record As IDataRecord
        Dim MaintFlag As New MaintFlagMS

        Public Function GetTRNReader(ByVal sqlstring As String, ByVal database As Integer) As Boolean


            Try
                coreConnection = New SqlConnection(dl.APRConnectionString(database))
                coreConnection.Open()
                corecmd = New SqlCommand(sqlstring, coreConnection)
                coreReader = corecmd.ExecuteReader(CommandBehavior.CloseConnection)

                Return True
            Catch ex As Exception

                Return False
                Exit Function
            End Try



        End Function

        'Public Function GetUtilityData(ByVal sqlstring As String) As Boolean


        '    Try
        '        coreConnection = New SqlConnection(dl.CrConnectionString)
        '        coreConnection.Open()
        '        corecmd = New SqlCommand(sqlstring, coreConnection)
        '        coreReader = corecmd.ExecuteReader(CommandBehavior.CloseConnection)

        '        Return True
        '    Catch ex As Exception

        '        Return False
        '        Exit Function
        '    End Try



        'End Function

        Public Function GetAdapter(ByVal Command As IDbCommand) As IDbDataAdapter

            Return New SqlDataAdapter(Command)

        End Function

        Private Function ProcessRecordSet(ByVal record As IDataRecord, ByVal FieldInfo As List(Of ClassFieldInfo)) As Dictionary(Of String, Object)
            Dim returndict As New Dictionary(Of String, Object)

            If record.FieldCount > 0 Then
                For i = 0 To record.FieldCount - 1
                    If IsDBNull(record(i)) = True Then
                        Dim fieldname As String = record.GetName(i)
                        Dim Fieldtype = (From v In FieldInfo Where v.Name = fieldname Select v).Take(1)
                        For Each result In Fieldtype
                            Select Case result.Type
                                Case "System.String"
                                    returndict.Add(record.GetName(i).ToString(), "")
                                Case "System.Int32"
                                    returndict.Add(record.GetName(i).ToString(), "0")
                                Case "System.Int64"
                                    returndict.Add(record.GetName(i).ToString(), "0")
                                Case "System.Boolean"
                                    returndict.Add(record.GetName(i).ToString(), "True")
                                Case Else
                                    returndict.Add(record.GetName(i).ToString(), "")
                            End Select
                        Next
                    Else
                        returndict.Add(record.GetName(i).ToString(), record(i).ToString())
                    End If
                Next
            Else
                Return Nothing
            End If

            Return returndict
        End Function

        Private Function GetClassFieldTypes(ByVal ClassName As String) As List(Of ClassFieldInfo)
            Dim returnlist As New List(Of ClassFieldInfo)
            If IsNothing(Type.GetType(ClassName)) = False Then
                Dim classInstance As Object = Activator.CreateInstance(Type.GetType(ClassName))
                If IsNothing(classInstance) = False Then
                    Dim MyFields = classInstance.GetType().GetFields(BindingFlags.Public Or BindingFlags.Instance)

                    For Each mfield As FieldInfo In MyFields
                        returnlist.Add(New ClassFieldInfo With {.Name = mfield.Name.ToString(), .Type = mfield.FieldType.ToString()})

                    Next
                End If
            End If
            Return returnlist
        End Function
        Private Function GetClassInstanceTypes(ByVal ClassName As String) As List(Of ClassFieldInfo)
            Dim returnlist As New List(Of ClassFieldInfo)
            If IsNothing(Type.GetType(ClassName)) = False Then
                Dim classInstance As Object = Activator.CreateInstance(Type.GetType(ClassName))
                Dim Properties As PropertyInfo() = classInstance.GetType().GetProperties()
                For Each info As PropertyInfo In Properties
                    returnlist.Add(New ClassFieldInfo With {.Name = info.Name.ToString(), .Type = info.PropertyType.ToString()})
                Next

            End If

            Return returnlist
        End Function
        Public Function GetCommand(ByVal sqlQuery As String, ByVal _mgrConnection As IDbConnection) As IDbCommand

            Dim NewSqlCommand As SqlCommand = New SqlCommand(sqlQuery, _mgrConnection)

            NewSqlCommand.CommandTimeout = 1200
            Return NewSqlCommand


        End Function

        Public Function AdapterFill(ByRef da As IDbDataAdapter, ByRef DataSet As DataSet, ByVal TableName As String) As Boolean
            AdapterFill = False

            DirectCast(da, System.Data.SqlClient.SqlDataAdapter).Fill(DataSet, TableName)

            Return True
        End Function

        Public Function getSelector2_Ctxmang(ByVal sqlstring As String) As List(Of selector2array)
            Dim readerlist As New List(Of selector2array)()

            Using con As New SqlConnection(dl.ctxmangConnectionString)
                con.Open()
                Dim cmd As New SqlCommand(sqlstring, con)
                coreReader = cmd.ExecuteReader(CommandBehavior.CloseConnection)

                If coreReader.FieldCount > 0 Then
                    While coreReader.Read
                        record = CType(coreReader, IDataRecord)

                        readerlist.Add(New selector2array With {.text = Convert.ToString(record(1)), .id = Convert.ToInt16(record(0))})

                    End While

                    Return readerlist
                Else
                    Return Nothing
                End If

            End Using

        End Function

        Public Function GetRefparameter(ByVal dbParameterName As String, ByVal systype As String) As IDbDataParameter

            Select Case systype.ToString()
                Case "System.String"
                    Return New SqlParameter(dbParameterName, SqlDbType.NVarChar)
                Case "System.Int16"
                    Return New SqlParameter(dbParameterName, SqlDbType.SmallInt)
                Case "System.Int32"
                    Return New SqlParameter(dbParameterName, SqlDbType.Int)
                Case "System.Boolean"
                    Return New SqlParameter(dbParameterName, SqlDbType.Bit)
                Case "System.DateTime"
                    Return New SqlParameter(dbParameterName, SqlDbType.DateTime)
                Case "System.Single"                                                      'bms 11/4/04
                    Return New SqlParameter(dbParameterName, SqlDbType.Real)            'bms 11/4/04
                Case "System.Currency"                                                    'bms 11/17/04
                    Return New SqlParameter(dbParameterName, SqlDbType.Money)           'bms 11/17/04
                Case "System.Binary"                                              'SER 12/9/2008
                    Return New SqlParameter(dbParameterName, SqlDbType.Binary)  'SER 12/9/2008
                Case "System.Decimal"                                                 ' DJP 8/30/12"
                    Return New SqlParameter(dbParameterName, SqlDbType.Decimal)     ' DJP 8/30/12
                Case "System.Double"
                    Return New SqlParameter(dbParameterName, SqlDbType.Float)
                Case Else
                    Return Nothing
            End Select

        End Function

        Public Function Getparameter(ByVal dbParameterName As String, ByVal dbParameterValue As DbType) As IDbDataParameter

            Select Case dbParameterValue
                Case DbType.String
                    Return New SqlParameter(dbParameterName, SqlDbType.NVarChar)
                Case DbType.Int16
                    Return New SqlParameter(dbParameterName, SqlDbType.SmallInt)
                Case DbType.Int32
                    Return New SqlParameter(dbParameterName, SqlDbType.Int)
                Case DbType.Int64
                    Return New SqlParameter(dbParameterName, SqlDbType.Int)
                Case DbType.Boolean
                    Return New SqlParameter(dbParameterName, SqlDbType.Bit)
                Case DbType.DateTime
                    Return New SqlParameter(dbParameterName, SqlDbType.DateTime)
                Case DbType.Single                                                      'bms 11/4/04
                    Return New SqlParameter(dbParameterName, SqlDbType.Real)            'bms 11/4/04
                Case DbType.Currency                                                    'bms 11/17/04
                    Return New SqlParameter(dbParameterName, SqlDbType.Money)           'bms 11/17/04
                Case DbType.Binary                                              'SER 12/9/2008
                    Return New SqlParameter(dbParameterName, SqlDbType.Binary)  'SER 12/9/2008
                Case DbType.Decimal                                                 ' DJP 8/30/12
                    Return New SqlParameter(dbParameterName, SqlDbType.Decimal)     ' DJP 8/30/12
                Case Else
                    Return Nothing
            End Select

        End Function

        Public Function getUtilityData(ByVal sqlstring As String) As List(Of Chartdatavalues)
            Dim readerlist As New List(Of Chartdatavalues)()

            Using con As New SqlConnection(dl.CrConnectionString)
                con.Open()
                Dim cmd As New SqlCommand(sqlstring, con)
                coreReader = cmd.ExecuteReader(CommandBehavior.CloseConnection)

                If coreReader.FieldCount > 0 Then
                    While coreReader.Read
                        record = CType(coreReader, IDataRecord)
                        Try
                            readerlist.Add(New Chartdatavalues With {.Timestamp = Convert.ToString(record(0)), .value1 = Convert.ToDecimal(record(1)), .value2 = Convert.ToDecimal(record(2))})
                        Catch ex As Exception

                        End Try


                    End While

                    Return readerlist
                Else
                    Return Nothing
                End If

            End Using

        End Function

        'trnx server 
        Public Function getbuttonlibrary(ByVal sqlstring As String, ByVal database As Integer) As List(Of SPCInspection.buttonlibrary)
            Dim readerlist As New List(Of SPCInspection.buttonlibrary)()

            Using con As New SqlConnection(dl.InspectConnectionString())
                con.Open()
                Dim cmd As New SqlCommand(sqlstring, con)
                coreReader = cmd.ExecuteReader(CommandBehavior.CloseConnection)

                If coreReader.FieldCount > 0 Then
                    While coreReader.Read
                        record = CType(coreReader, IDataRecord)
                        Try
                            readerlist.Add(New SPCInspection.buttonlibrary With {.id = Convert.ToInt32(record(0)), .label = Convert.ToString(record(1)), .value = record(5)})
                        Catch ex As Exception

                        End Try


                    End While

                    Return readerlist
                Else
                    Return Nothing
                End If

            End Using
        End Function

        Public Function getSelector2(ByVal sqlstring As String, ByVal database As Integer) As List(Of selector2array)
            Dim readerlist As New List(Of selector2array)()

            Using con As New SqlConnection(dl.InspectConnectionString())
                con.Open()
                Dim cmd As New SqlCommand(sqlstring, con)
                coreReader = cmd.ExecuteReader(CommandBehavior.CloseConnection)

                If coreReader.FieldCount > 0 Then
                    While coreReader.Read
                        record = CType(coreReader, IDataRecord)

                        readerlist.Add(New selector2array With {.text = Convert.ToString(record(1)), .id = Convert.ToInt16(record(0))})

                    End While

                    Return readerlist
                Else
                    Return Nothing
                End If

            End Using

        End Function

        Public Function getTemplateTable(ByVal sqlstring As String, ByVal database As Integer) As List(Of SPCInspection.TemplateTable)
            Dim readerlist As New List(Of SPCInspection.TemplateTable)()

            Using con As New SqlConnection(dl.InspectConnectionString())
                con.Open()
                Dim cmd As New SqlCommand(sqlstring, con)
                coreReader = cmd.ExecuteReader(CommandBehavior.CloseConnection)

                If coreReader.FieldCount > 0 Then
                    While coreReader.Read
                        record = CType(coreReader, IDataRecord)

                        readerlist.Add(New SPCInspection.TemplateTable With {.TemplateId = Convert.ToString(record(0)), .Name = Convert.ToString(record(1)), .Owner = Convert.ToString(record(2)), .DateCreated = Convert.ToString(record(3)), .Status = Convert.ToString(record(4)), .Loc_STT = Convert.ToBoolean(record(7)), .Loc_CAR = Convert.ToBoolean(record(8)), .Loc_STJ = Convert.ToBoolean(record(9)), .Loc_SPA = Convert.ToBoolean(record(10)), .Loc_CDC = Convert.ToBoolean(record(11)), .Loc_LINYI = Convert.ToBoolean(record(12)), .Loc_PCE = Convert.ToBoolean(record(13)), .Loc_FSK = Convert.ToBoolean(record(14)), .Loc_FNL = Convert.ToBoolean(record(15)), .Loc_FPC = Convert.ToBoolean(record(16))})

                    End While

                    Return readerlist
                Else
                    Return Nothing
                End If

            End Using

        End Function


        Public Function getTemplateCollection(ByVal sqlstring As String) As List(Of SPCInspection.TemplateCollection)
            Dim readerlist As New List(Of SPCInspection.TemplateCollection)()
            Dim dictionary As New Dictionary(Of String, Object)
            Dim fieldList As List(Of ClassFieldInfo)

            fieldList = GetClassFieldTypes("core.SPCInspection.TemplateCollection")

            Using con As New SqlConnection(dl.InspectConnectionString())
                con.Open()
                Dim cmd As New SqlCommand(sqlstring, con)
                coreReader = cmd.ExecuteReader(CommandBehavior.CloseConnection)


                If coreReader.FieldCount > 0 Then
                    While coreReader.Read
                        record = CType(coreReader, IDataRecord)
                        Dim DefectTypeVal As String = "Default"

                        'If IsDBNull(record(7)) = True Then
                        '    DefectTypeVal = "Default"
                        'Else
                        '    DefectTypeVal = Convert.ToString(record(7))
                        'End If
                        'If IsDBNull(record(1)) = True Or IsDBNull(record(5)) = True Or IsDBNull(record(8)) Then
                        '    readerlist.Add(New SPCInspection.TemplateCollection With {.TabTemplateId = Convert.ToInt32(record(0)), .ButtonId = 0, .Name = Convert.ToString(record(2)), .TabNumber = Convert.ToInt32(record(3)), .TemplateId = Convert.ToInt32(record(4)), .ButtonName = "NaN", .ProductSpecs = Convert.ToBoolean(record(6)), .DefectType = DefectTypeVal, .ButtonTemplateId = Convert.ToInt32(record(8))})
                        'Else
                        '    readerlist.Add(New SPCInspection.TemplateCollection With {.TabTemplateId = Convert.ToInt32(record(0)), .ButtonId = Convert.ToInt32(record(1)), .Name = Convert.ToString(record(2)), .TabNumber = Convert.ToInt32(record(3)), .TemplateId = Convert.ToInt32(record(4)), .ButtonName = Convert.ToString(record(5)), .ProductSpecs = Convert.ToBoolean(record(6)), .DefectType = DefectTypeVal, .ButtonTemplateId = Convert.ToInt32(record(8))})
                        'End If
                        dictionary = ProcessRecordSet(record, fieldList)
                        If dictionary.Count > 0 Then
                            readerlist.Add(New SPCInspection.TemplateCollection With {.TabTemplateId = Convert.ToInt32(dictionary.Item("TabTemplateId")), .ButtonId = Convert.ToInt32(dictionary.Item("ButtonId")), .Name = Convert.ToString(dictionary.Item("Name")), .TabNumber = Convert.ToInt32(dictionary.Item("TabNumber")), .TemplateId = Convert.ToInt32(dictionary.Item("TemplateId")), .ButtonName = Convert.ToString(dictionary.Item("ButtonName")), .ProductSpecs = Convert.ToBoolean(dictionary.Item("ProductSpecs")), .DefectType = Convert.ToBoolean(dictionary.Item("DefectType")), .id = Convert.ToInt32(dictionary.Item("id")), .DefectCode = dictionary.Item("DefectCode"), .Hide = Convert.ToBoolean(dictionary.Item("Hide"))})
                        Else
                            readerlist.Add(New SPCInspection.TemplateCollection With {.TabTemplateId = Convert.ToInt32(record(0)), .ButtonId = Convert.ToInt32(record(1)), .Name = Convert.ToString(record(2)), .TabNumber = Convert.ToInt32(record(3)), .TemplateId = Convert.ToInt32(record(4)), .ButtonName = Convert.ToString(record(5)), .ProductSpecs = Convert.ToBoolean(record(6)), .DefectType = Convert.ToBoolean(record(7)), .id = Convert.ToInt32(record(8)), .DefectCode = record(9), .Hide = Convert.ToBoolean(record(10))})
                        End If

                    End While

                    Return readerlist
                Else
                    Return Nothing
                End If

            End Using

        End Function

        Public Function getProductSpecCollection(ByVal sqlstring As String) As List(Of SPCInspection.ProductSpecCollection)
            Dim readerlist As New List(Of SPCInspection.ProductSpecCollection)()

            Using con As New SqlConnection(dl.InspectConnectionString())
                con.Open()
                Dim cmd As New SqlCommand(sqlstring, con)
                coreReader = cmd.ExecuteReader(CommandBehavior.CloseConnection)


                If coreReader.FieldCount > 0 Then
                    While coreReader.Read
                        record = CType(coreReader, IDataRecord)
                        readerlist.Add(New SPCInspection.ProductSpecCollection With {.TabTemplateId = Convert.ToInt32(record(0)), .TabNumber = Convert.ToInt32(record(1)), .Spec_Description = Convert.ToString(record(2)), .value = Convert.ToDecimal(record(3)), .Spec_Value_Upper = Convert.ToDecimal(record(4)), .Spec_Value_Lower = Convert.ToDecimal(record(5)), .SpecId = Convert.ToInt32(record(6)), .TabName = Convert.ToString(record(7))})

                    End While

                    Return readerlist
                Else
                    Return Nothing
                End If

            End Using

        End Function

        Public Function getRollInspectionSummaryHeaders(ByVal sqlstring As String) As List(Of SPCInspection.RollInspectionSummaryHeaders)
            Dim readerlist As New List(Of SPCInspection.RollInspectionSummaryHeaders)()

            Using con As New SqlConnection(dl.InspectConnectionString())
                con.Open()
                Dim cmd As New SqlCommand(sqlstring, con)
                coreReader = cmd.ExecuteReader(CommandBehavior.CloseConnection)


                If coreReader.FieldCount > 0 And coreReader.HasRows = True Then
                    While coreReader.Read
                        record = CType(coreReader, IDataRecord)
                        readerlist.Add(New SPCInspection.RollInspectionSummaryHeaders With {.LoomNo = Convert.ToInt32(record(1)), .RollNumber = Convert.ToString(record(2)), .Style = Convert.ToString(record(3)), .Yards_Inspected = Convert.ToDecimal(record(4)), .DefectYardsf = Convert.ToInt64(record(5)), .DHY = Convert.ToDecimal(record(6)), .RollStartTimestamp = Convert.ToDateTime(record(0))})

                    End While

                    Return readerlist
                Else
                    Return readerlist
                End If

            End Using

        End Function

        'Public Function getProductDisplaySpecCollection(ByVal sqlstring As String) As List(Of SPCInspection.ProductDisplaySpecCollection)
        '    Dim readerlist As New List(Of SPCInspection.ProductDisplaySpecCollection)()

        '    Using con As New SqlConnection(dl.InspectConnectionString())
        '        con.Open()
        '        Dim cmd As New SqlCommand(sqlstring, con)
        '        coreReader = cmd.ExecuteReader(CommandBehavior.CloseConnection)

        '        If coreReader.FieldCount > 0 Then
        '            While coreReader.Read
        '                record = CType(coreReader, IDataRecord)

        '                If IsNothing(record(7)) = True Or IsDBNull(record(7)) = True Then
        '                    readerlist.Add(New SPCInspection.ProductDisplaySpecCollection With {.SpecId = Convert.ToInt32(record(0)), .Spec_Value_Upper = Convert.ToDecimal(record(1)), .Spec_Value_Lower = Convert.ToDecimal(record(2)), .TabName = Convert.ToString(record(3)), .MeasureValue = Convert.ToDecimal(record(4)), .Timestamp = Convert.ToDateTime(record(5)), .TabTemplateId = Convert.ToInt32(record(6)), .InspectionId = 0, .Spec_Description = Convert.ToString(record(8)), .value = Convert.ToDecimal(record(9)), .SpecDelta = Convert.ToDecimal(record(10))})
        '                Else
        '                    readerlist.Add(New SPCInspection.ProductDisplaySpecCollection With {.SpecId = Convert.ToInt32(record(0)), .Spec_Value_Upper = Convert.ToDecimal(record(1)), .Spec_Value_Lower = Convert.ToDecimal(record(2)), .TabName = Convert.ToString(record(3)), .MeasureValue = Convert.ToDecimal(record(4)), .Timestamp = Convert.ToDateTime(record(5)), .TabTemplateId = Convert.ToInt32(record(6)), .InspectionId = Convert.ToInt32(record(7)), .Spec_Description = Convert.ToString(record(8)), .value = Convert.ToDecimal(record(9)), .SpecDelta = Convert.ToDecimal(record(10))})
        '                End If

        '            End While

        '            Return readerlist
        '        Else
        '            Return Nothing
        '        End If

        '    End Using

        'End Function

        Public Function getDefectMasterData(ByVal sqlstring As String) As List(Of SPCInspection.DefectMasterDisplay)
            Dim readerlist As New List(Of SPCInspection.DefectMasterDisplay)()
            Dim fieldList As List(Of ClassFieldInfo)
            Dim dictionary As New Dictionary(Of String, Object)
            fieldList = GetClassInstanceTypes("core.SPCInspection.DefectMasterDisplay")

            Using con As New SqlConnection(dl.InspectConnectionString())
                con.Open()
                Dim cmd As New SqlCommand(sqlstring, con)
                coreReader = cmd.ExecuteReader(CommandBehavior.CloseConnection)

                If coreReader.FieldCount > 0 And fieldList.Count > 0 Then
                    While coreReader.Read
                        record = CType(coreReader, IDataRecord)

                        dictionary = ProcessRecordSet(record, fieldList)
                        readerlist.Add(New SPCInspection.DefectMasterDisplay With {.DefectID = Convert.ToInt32(dictionary.Item("DefectID")), .DefectTime = Convert.ToString(dictionary.Item("DefectTime")), .DefectDesc = Convert.ToString(dictionary.Item("DefectDesc")), .DataNo = Convert.ToString(dictionary.Item("DataNo")), .EmployeeNo = Convert.ToString(dictionary.Item("EmployeeNo")), .InspectionId = Convert.ToInt32(dictionary.Item("InspectionId")), .TotalLotPieces = Convert.ToString(dictionary.Item("TotalLotPieces")), .Product = Convert.ToString(dictionary.Item("Product")), .WorkOrder = Convert.ToString(dictionary.Item("WorkOrder")), .RollNo = Convert.ToString(dictionary.Item("RollNo")), .LoomNo = Convert.ToString(dictionary.Item("LoomNo")), .DataType = Convert.ToString(dictionary.Item("DataType")), .DefectImage_Filename = Convert.ToString(dictionary.Item("DefectImage_Filename")), .Inspector = Convert.ToString(dictionary.Item("Inspector")), .ItemNumber = Convert.ToString(dictionary.Item("ItemNumber")), .InspectionState = Convert.ToString(dictionary.Item("InspectionState"))})
                        'readerlist.Add(New SPCInspection.DefectMasterDisplay With {.DefectId = Convert.ToInt32(record(0)), .DefectTime = Convert.ToString(record(1)), .DefectDesc = Convert.ToString(record(2)), .POnumber = Convert.ToString(record(3)), .DataNo = Convert.ToString(record(4)), .EmployeeNo = Convert.ToString(record(5)), .AQL = Convert.ToString(record(6)), .SampleSize = Convert.ToString(record(7)), .TotalLotPieces = Convert.ToString(record(8))})
                    End While

                    Return readerlist
                Else
                    Return Nothing
                End If

            End Using

        End Function

        Public Function GetTemplateId(ByVal sqlstring As String, ByVal database As Integer) As Integer

            Using con As New SqlConnection(dl.InspectConnectionString())
                con.Open()
                Dim cmd As New SqlCommand(sqlstring, con)
                coreReader = cmd.ExecuteReader(CommandBehavior.CloseConnection)

                If coreReader.FieldCount > 0 Then
                    Dim TemplateId As Integer
                    While coreReader.Read
                        record = CType(coreReader, IDataRecord)
                        TemplateId = Convert.ToInt32(record(0))
                    End While

                    Return TemplateId
                Else
                    Return Nothing
                End If

            End Using

        End Function

        Public Function getLoomStats(ByVal sqlstring As String, ByVal database As Integer) As List(Of LoomStopStats)
            Dim readerlist As New List(Of LoomStopStats)()

            Using con As New SqlConnection(dl.APRConnectionString(database))
                con.Open()
                Dim cmd As New SqlCommand(sqlstring, con)
                coreReader = cmd.ExecuteReader(CommandBehavior.CloseConnection)

                If coreReader.FieldCount > 0 Then
                    While coreReader.Read
                        record = CType(coreReader, IDataRecord)
                        Try
                            readerlist.Add(New LoomStopStats With {.Stops_PerShift = Convert.ToInt16(record(0)), .Disturbance_PerShift = Convert.ToDecimal(record(1)), .FillStop_PerShift = Convert.ToDecimal(record(2)), .WarpStop_PerShift = Convert.ToDecimal(record(3)), .PieceLength_PerShift = Convert.ToDecimal(record(4)), .Host_PerShift = Convert.ToDecimal(record(5))})
                        Catch ex As Exception

                        End Try


                    End While

                    Return readerlist
                Else
                    Return Nothing
                End If

            End Using
        End Function

        Public Function getLoomGridPicks(ByVal sqlstring As String, ByVal database As Integer) As List(Of LoomPickStats)
            Dim readerlist As New List(Of LoomPickStats)()

            Using con As New SqlConnection(dl.APRConnectionString(database))
                con.Open()
                Dim cmd As New SqlCommand(sqlstring, con)
                coreReader = cmd.ExecuteReader(CommandBehavior.CloseConnection)

                If coreReader.FieldCount > 0 Then
                    While coreReader.Read
                        record = CType(coreReader, IDataRecord)
                        Try
                            readerlist.Add(New LoomPickStats With {.PickCount_Curr = Convert.ToDecimal(record(0)), .PickCount_ShiftAvg = Convert.ToDecimal(record(1)), .PickCount_Max = Convert.ToDecimal(record(2))})
                        Catch ex As Exception

                        End Try


                    End While

                    Return readerlist
                Else
                    Return Nothing
                End If

            End Using
        End Function

        Public Function getCurrLoomPicks(ByVal sqlstring As String, ByVal database As Integer) As List(Of LoomPicksCurr)
            Dim readerlist As New List(Of LoomPicksCurr)()

            Using con As New SqlConnection(dl.APRConnectionString(database))
                con.Open()
                Dim cmd As New SqlCommand(sqlstring, con)
                coreReader = cmd.ExecuteReader(CommandBehavior.CloseConnection)

                If coreReader.FieldCount > 0 Then
                    While coreReader.Read
                        record = CType(coreReader, IDataRecord)
                        Try
                            readerlist.Add(New LoomPicksCurr With {.LoomNo = Convert.ToInt16(record(0)), .Picks = Convert.ToDecimal(record(1)), .updated = Convert.ToString(record(2))})
                        Catch ex As Exception

                        End Try

                    End While

                    Return readerlist
                Else
                    Return Nothing
                End If

            End Using
        End Function

        Public Function getDashSchedule(ByVal sqlstring As String, ByVal database As Integer) As List(Of DashBoardSchedule)
            Dim readerlist As New List(Of DashBoardSchedule)()

            Using con As New SqlConnection(dl.APRConnectionString(database))
                con.Open()
                Dim cmd As New SqlCommand(sqlstring, con)
                coreReader = cmd.ExecuteReader(CommandBehavior.CloseConnection)

                If coreReader.FieldCount > 0 Then
                    While coreReader.Read
                        record = CType(coreReader, IDataRecord)
                        Try
                            readerlist.Add(New DashBoardSchedule With {.url = Convert.ToString(record(0)), .ClipTop = Convert.ToInt16(record(1)), .ClipRight = Convert.ToInt16(record(2)), .ClipButton = Convert.ToInt16(record(3)), .ClipLeft = Convert.ToInt16(record(4)), .MainPlateWidth = Convert.ToInt16(record(5)), .MainPlateHeight = Convert.ToInt16(record(6)), .MainPlateScale = Convert.ToDecimal(record(7)), .InnerDivTop = Convert.ToInt16(record(8)), .InnerDivLeft = Convert.ToInt16(record(9)), .SlideOrder = Convert.ToInt16(record(10)), .Type = Convert.ToString(record(11)), .TransTime = Convert.ToInt16(record(12)), .TableSourceId = Convert.ToDecimal(record(13))})
                        Catch ex As Exception

                        End Try


                    End While

                    Return readerlist
                Else
                    Return Nothing
                End If

            End Using
        End Function

        'ctx manager

        Public Function getNavPermissions(ByVal sqlstring As String) As List(Of NavigationPermissions)
            Dim readerlist As New List(Of NavigationPermissions)()

            Using con As New SqlConnection(ConfigurationManager.ConnectionStrings("ctxmangconnectionstring").ConnectionString)
                con.Open()
                Dim cmd As New SqlCommand(sqlstring, con)
                coreReader = cmd.ExecuteReader(CommandBehavior.CloseConnection)

                If coreReader.FieldCount > 0 Then
                    While coreReader.Read
                        record = CType(coreReader, IDataRecord)
                        Try
                            readerlist.Add(New NavigationPermissions With {.APRPM_Enabled = Convert.ToBoolean(record(0)), .APRUtility_Enabled = Convert.ToBoolean(record(1)), .APRLoom_Enabled = Convert.ToBoolean(record(2)), .APRInspection_Enabled = Convert.ToBoolean(record(3)), .APRSPC_Enabled = Convert.ToBoolean(record(4))})
                        Catch ex As Exception

                        End Try


                    End While

                    Return readerlist
                Else
                    Return Nothing
                End If

            End Using
        End Function

        'other
        Public Function GetDefectMasterDailyObjects(ByVal cmd As SqlCommand, ByVal con As SqlConnection) As List(Of SPCInspection.DefectMasterCount)
            Dim record As IDataRecord
            Dim readerlist As New List(Of SPCInspection.DefectMasterCount)
            Dim dr As SqlDataReader
            'Dim con As New SqlConnection(dl.APRConnectionString(3))
            Try
                'con.Open()
                dr = cmd.ExecuteReader()
            Catch ex As Exception
                Return readerlist
            End Try


            While dr.Read
                record = CType(dr, IDataRecord)
                Dim length As Integer = record.FieldCount
                readerlist.Add(New SPCInspection.DefectMasterCount With {.Count = Convert.ToInt32(record(0)), .TemplateId = Convert.ToInt32(record(1)), .Year = Convert.ToInt32(record(2)), .Month = Convert.ToInt32(record(3))})

            End While

            dr.Close()
            cmd.Dispose()
            'con.Close()

            Return readerlist

        End Function

        Public Function GetDefectMasterDataTypeCount(ByVal cmd As SqlCommand, ByVal con As SqlConnection) As List(Of Integer)
            Dim record As IDataRecord
            Dim readerlist As New List(Of Integer)
            Dim dr As SqlDataReader
            'Dim con As New SqlConnection(dl.APRConnectionString(3))
            Try
                'con.Open()
                dr = cmd.ExecuteReader()
            Catch ex As Exception
                Return readerlist
            End Try


            While dr.Read
                record = CType(dr, IDataRecord)
                Dim length As Integer = record.FieldCount
                readerlist.Add(Convert.ToInt32(record(0)))

            End While

            dr.Close()
            cmd.Dispose()
            'con.Close()

            Return readerlist

        End Function

        Public Function GetDefectMasterHistogram(ByVal cmd As SqlCommand, ByVal con As SqlConnection) As List(Of SPCInspection.BarChart)
            Dim record As IDataRecord
            Dim readerlist As New List(Of SPCInspection.BarChart)
            Dim dr As SqlDataReader
            'Dim con As New SqlConnection(dl.APRConnectionString(3))
            Try
                'con.Open()
                dr = cmd.ExecuteReader()
            Catch ex As Exception
                Return readerlist
            End Try


            While dr.Read
                record = CType(dr, IDataRecord)
                Dim length As Integer = record.FieldCount

                For i = 0 To length - 1
                    readerlist.Add(New SPCInspection.BarChart With {.y = record(i), .x = dr.GetName(i)})
                Next

            End While

            dr.Close()
            cmd.Dispose()
            'con.Close()

            Return readerlist

        End Function

        Public Function GetPieChartData(ByVal cmd As SqlCommand, ByVal con As SqlConnection) As List(Of PieChartdata)
            Dim record As IDataRecord
            Dim readerlist As New List(Of PieChartdata)
            Dim dr As SqlDataReader
            'Dim con As New SqlConnection(dl.APRConnectionString(3))
            Try
                'con.Open()
                dr = cmd.ExecuteReader()
            Catch ex As Exception
                Return readerlist
            End Try


            While dr.Read
                record = CType(dr, IDataRecord)
                Dim length As Integer = record.FieldCount
                readerlist.Add(New PieChartdata With {.value = Convert.ToDecimal(record(0)), .id = Convert.ToInt32(record(1)), .desc = Convert.ToString(record(2))})

            End While

            dr.Close()
            cmd.Dispose()
            'con.Close()

            Return readerlist

        End Function

        Public Function getInspectionCompliance(ByVal cmd As SqlCommand) As List(Of SPCInspection.InspectionCompliance_Local)
            Dim record As IDataRecord
            Dim list As New List(Of SPCInspection.InspectionCompliance_Local)
            Dim dr As SqlDataReader

            Try
                dr = cmd.ExecuteReader(CommandBehavior.CloseConnection)

                While dr.Read
                    record = CType(dr, IDataRecord)
                    If record.FieldCount = 9 Then
                        list.Add(New SPCInspection.InspectionCompliance_Local With {.Id = Convert.ToInt64(record(0)), .WorkOrder = Convert.ToString(record(1)), .DataNo = Convert.ToString(record(2)), .WADL01 = Convert.ToString(record(3)), .Location = Convert.ToString(record(4)), .WADCTO = Convert.ToString(record(5)), .WASRST = Convert.ToString(record(6)), .ijsid = Convert.ToInt64(record(7)), .LineType = Convert.ToString(record(8))})
                    End If
                End While
            Catch ex As Exception

            End Try
            Return list
        End Function

        Private Function ConvertType(ByVal Obj As Object, ByVal Typename As String) As Object
            Dim returnobj As Object
            Dim typenamefm As String = Typename.ToUpper()
            Select Case typenamefm
                Case "STRING"
                    Try
                        returnobj = CType(Obj, String)
                    Catch ex As Exception
                        returnobj = ""
                    End Try

                Case "INT32", "INT64", "INTEGER"
                    If IsNumeric(Obj) = True And IsDBNull(Obj) = False Then
                        Try
                            returnobj = CType(Obj, Integer)
                        Catch ex As Exception
                            returnobj = 0
                        End Try
                    Else
                        returnobj = 0
                    End If
                Case "BOOLEAN"
                    If VarType(Obj) = vbBoolean And IsDBNull(Obj) = False Then
                        Try
                            returnobj = CType(Obj, Boolean)
                        Catch ex As Exception
                            returnobj = False
                        End Try

                    Else
                        returnobj = False
                    End If
                Case "DECIMAL"
                    If IsNumeric(Obj) = True And IsDBNull(Obj) = False Then
                        Try
                            returnobj = CType(Obj, Decimal)
                        Catch ex As Exception
                            returnobj = 0
                        End Try

                    Else
                        returnobj = 0
                    End If
                Case "DATETIME"
                    If IsDate(Obj) = True And IsDBNull(Obj) = False Then
                        Try
                            returnobj = CType(Obj, DateTime)
                        Catch ex As Exception
                            returnobj = New DateTime(1900, 1, 1)
                        End Try

                    Else
                        returnobj = New DateTime(1900, 1, 1)
                    End If
                Case Else
                    returnobj = Obj
            End Select

            Return returnobj
        End Function



    End Class

End Namespace
