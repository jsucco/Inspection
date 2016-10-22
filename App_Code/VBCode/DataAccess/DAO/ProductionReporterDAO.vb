Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Data


Namespace core


    Public Class ProductionReporterDAO

        Private dlayer As New dlayer
        Public LocationNames As List(Of Locationarray)
        Public LocationsSelectors As List(Of selector2array)
        Public NEWALLID As Integer

        Public Function GetSPCHourlyProduction(ByVal fromdate As DateTime, ByVal todate As DateTime, ByVal Location As String) As Object
            Dim Sql As String
            Dim returnobj As Object = 0


            If fromdate < todate Then

                Dim bmap_hpstt As New BMappers(Of Production.HourlyProductionSTT)
                Dim hpsttlist As New List(Of Production.HourlyProductionSTT)

                If Location = "ALL" Then
                    Sql = "SELECT HourID, Machine, HourBegin, ProductCount, HourlyYds, OverLengthInches, CutLengthSpec, RunTime, DownTime, WorkOrderID FROM HourlyProduction" & vbCrLf &
                                        "WHERE (HourBegin >= CONVERT(DATETIME, '" & fromdate.ToString("g") & "', 102)) AND (HourBegin <= CONVERT(DATETIME, '" & todate.ToString("g") & "', 102))" & vbCrLf &
                                        "ORDER BY HourBegin DESC"
                Else
                    Sql = "SELECT HourID, Machine, HourBegin, ProductCount, HourlyYds, OverLengthInches, CutLengthSpec, RunTime, DownTime, WorkOrderID FROM HourlyProduction" & vbCrLf &
                                        "WHERE (HourBegin >= CONVERT(DATETIME, '" & fromdate.ToString("g") & "', 102)) AND (HourBegin <= CONVERT(DATETIME, '" & todate.ToString("g") & "', 102))  AND (Location = '" & Location & "')" & vbCrLf &
                                        "ORDER BY HourBegin DESC"
                End If
                
                hpsttlist = bmap_hpstt.GetSpcObject(sql)
                returnobj = hpsttlist

            End If

                Return returnobj

        End Function

        Public Function GetProductionLocations() As List(Of Locationarray)
            Dim sqlstring As String
            Dim selectValues As New List(Of Locationarray)()
            Dim bmap As New BMappers(Of Locationarray)

            sqlstring = "SELECT id as id, Name as text, Abreviation as Abreviation FROM  LocationMaster WHERE  (ProductionResults = 1) ORDER BY text ASC"

            selectValues = bmap.GetAprMangObject(sqlstring)
            If selectValues.Count > 0 Then
                '    Try
                Dim object1 As Object = (From x In selectValues Order By x.id Descending Select x.id).ToArray()

                NEWALLID = object1(0) + 1

                selectValues.Add(New Locationarray With {.id = NEWALLID, .text = "ALL SITES", .Abreviation = "ALL"})

                LocationsSelectors = (From x In selectValues Order By x.id Descending Select New core.selector2array With {.id = x.id, .text = Trim(x.text)}).ToList()

                'NEWALLID = object1(0) + 1
                'selectValues.Add(New selector2array With {.id = NEWALLID, .text = "ALL SITES"})
                '     Catch ex As Exception

                '     End Try

            End If
            LocationNames = selectValues
            Return selectValues

        End Function

        Public Function GetJobSummary_1(ByVal Location As String, ByVal daysback As Integer, ByVal todate As DateTime, ByVal TemplateId As Integer) As List(Of Production.JobSummaryMAIN)
            Dim con As New SqlConnection(dlayer.SPCConnectionString)
            Dim cmd As SqlCommand = con.CreateCommand()
            Try
                Using con
                    con.Open()
                    Using cmd
                        cmd = New SqlCommand("SP_SPC_GetJobSummary_1", con)
                        cmd.CommandType = CommandType.StoredProcedure

                        cmd.Parameters.Add("@TEMPLATEID", SqlDbType.Int).Direction = ParameterDirection.Input
                        cmd.Parameters.Add("@DAYSBACK", SqlDbType.Int).Direction = ParameterDirection.Input
                        cmd.Parameters.Add("@TODATE", SqlDbType.DateTime).Direction = ParameterDirection.Input
                        cmd.Parameters.Add("@LOCATION", SqlDbType.VarChar).Direction = ParameterDirection.Input
                        cmd.Parameters("@TEMPLATEID").Value = TemplateId
                        cmd.Parameters("@DAYSBACK").Value = daysback
                        cmd.Parameters("@TODATE").Value = todate
                        cmd.Parameters("@LOCATION").Value = Location
                        cmd.CommandTimeout = 5000

                        Dim list As New List(Of Production.JobSummaryMAIN)
                        Dim bmapjs As New BMappers(Of Production.JobSummaryMAIN)

                        list = bmapjs.GetSpcSP(cmd)

                        If list.Count > 0 Then
                            Return list
                        Else
                            Return Nothing
                        End If

                    End Using
                    con.Close()
                End Using
            Catch ex As Exception
                Return Nothing
            End Try

        End Function

        Public Function GetSPCOperatorProduction(ByVal fromdate As DateTime, ByVal todate As DateTime, ByVal Location As String) As Object
            Dim sql As String
            Dim returnobj As Object = 0
            Dim wpsttlist As New List(Of Production.OperatorProduction)
            If fromdate < todate Then

                Dim bmap_wpstt As New BMappers(Of Production.OperatorProduction)

                If Location = "ALL" Then
                    sql = "SELECT  OperatorID, Machine, Operator AS OperatorNo, StartTime, EndTime, ScheduledTime, DownTime, RunTime, TotalYds, TotalSheets, Efficiency, AvgSheetsPerMin, AvgYdsPerMin, OverLengthInches FROM OperatorProduction WHERE (StartTime >= CONVERT(DATETIME, '" & fromdate.ToString("yyyy-MM-dd hh:mm:ss") & "', 120)) AND (EndTime <= CONVERT(DATETIME, '" & todate.ToString("yyyy-MM-dd hh:mm:ss") & "', 120)) ORDER BY StartTime DESC"
                Else
                    sql = "SELECT  OperatorID, Machine, Operator AS OperatorNo, StartTime, EndTime, ScheduledTime, DownTime, RunTime, TotalYds, TotalSheets, Efficiency, AvgSheetsPerMin, AvgYdsPerMin, OverLengthInches FROM OperatorProduction WHERE (StartTime >= CONVERT(DATETIME, '" & fromdate.ToString("yyyy-MM-dd hh:mm:ss") & "', 120)) AND (EndTime <= CONVERT(DATETIME, '" & todate.ToString("yyyy-MM-dd hh:mm:ss") & "', 120)) AND (SUBSTRING(Machine, 0 , 4) = '" & Location & "') ORDER BY StartTime DESC"
                End If

                wpsttlist = bmap_wpstt.GetSpcObject(sql)
                returnobj = wpsttlist

            End If

            Return wpsttlist

        End Function

        Public Function GetSPCWorkOrderProduction(ByVal fromdate As DateTime, ByVal todate As DateTime, ByVal Location As String) As Object
            Dim sql As String
            Dim returnobj As Object = 0

            If fromdate < todate Then

                Dim bmap_wpstt As New BMappers(Of Production.WorkOrderProductionSTT)
                Dim wpsttlist As New List(Of Production.WorkOrderProductionSTT)
                If Location = "ALL" Then
                    sql = "SELECT ID, Machine, WorkOrder, OperatorNo, StartTime, FinishTime, DataNo, GreigeNo, CutLengthSpec, JobYds, JobSheets, JobOverLengthInches, ScheduledTime, DownTime, RunTime, AvgSheetsPerHour, JDECOMP, JDESCRAP, JDETOTREC, DIFF_PERC FROM WorkOrderProduction" & vbCrLf &
                                        "WHERE (StartTime >= CONVERT(DATETIME, '" & fromdate.ToString("yyyy-MM-dd hh:mm:ss") & "', 120)) AND (FinishTime <= CONVERT(DATETIME, '" & todate.ToString("yyyy-MM-dd hh:mm:ss") & "', 120))" & vbCrLf &
                                        "ORDER BY FinishTime DESC"
                Else
                    sql = "SELECT ID, Machine, WorkOrder, OperatorNo, StartTime, FinishTime, DataNo, GreigeNo, CutLengthSpec, JobYds, JobSheets, JobOverLengthInches, ScheduledTime, DownTime, RunTime, AvgSheetsPerHour, JDECOMP, JDESCRAP, JDETOTREC, DIFF_PERC FROM WorkOrderProduction" & vbCrLf &
                                        "WHERE (StartTime >= CONVERT(DATETIME, '" & fromdate.ToString("yyyy-MM-dd hh:mm:ss") & "', 120)) AND (FinishTime <= CONVERT(DATETIME, '" & todate.ToString("yyyy-MM-dd hh:mm:ss") & "', 120)) AND (SUBSTRING(Machine, 0 , 4) = '" & Location & "')" & vbCrLf &
                                        "ORDER BY FinishTime DESC"
                End If
                
                wpsttlist = bmap_wpstt.GetSpcObject(sql)
                returnobj = wpsttlist

            End If

                Return returnobj

        End Function

        Public Function GetSPCRollProduction(ByVal fromdate As DateTime, ByVal todate As DateTime, ByVal Location As String) As Object
            Dim sql As String
            Dim returnobj As Object = 0

            If fromdate < todate Then

                Dim bmap_wpstt As New BMappers(Of Production.RollProductionSTT)
                Dim wpsttlist As New List(Of Production.RollProductionSTT)
                If Location = "ALL" Then
                    sql = "SELECT RollProductionID, Machine, OperatorNo, StartTime, EndTime, TotalYds, TotalSheets, TicketYds, TicketOverYds, RollNo, JobNo, DataNo, GreigeNo, TimeStamp_Trans FROM RollProduction WHERE (StartTime >= CONVERT(DATETIME, '" & fromdate.ToString("yyyy-MM-dd hh:mm:ss") & "', 120)) AND (EndTime <= CONVERT(DATETIME, '" & todate.ToString("yyyy-MM-dd hh:mm:ss") & "', 120)) ORDER BY StartTime DESC"
                Else
                    sql = "SELECT RollProductionID, Machine, OperatorNo, StartTime, EndTime, TotalYds, TotalSheets, TicketYds, TicketOverYds, RollNo, JobNo, DataNo, GreigeNo, TimeStamp_Trans FROM RollProduction WHERE (StartTime >= CONVERT(DATETIME, '" & fromdate.ToString("yyyy-MM-dd hh:mm:ss") & "', 120)) AND (EndTime <= CONVERT(DATETIME, '" & todate.ToString("yyyy-MM-dd hh:mm:ss") & "', 120)) AND (SUBSTRING(Machine, 0 , 4) = '" & Location & "') ORDER BY StartTime DESC"
                End If

                wpsttlist = bmap_wpstt.GetSpcObject(sql)
                returnobj = wpsttlist

            End If

                Return returnobj

        End Function

        Public Function GetHourlyProdRange(ByVal fromdate As DateTime, ByVal todate As DateTime, ByVal Location As String) As Object
            Dim con As New SqlConnection(dlayer.SPCConnectionString)
            Dim cmd As SqlCommand = con.CreateCommand()
            Dim cid As List(Of CID) = dlayer.GetCIDInfo()
            Dim returnobj As Object = 0

            If fromdate < todate Then
                Try
                    Using con
                        con.Open()
                        Using cmd
                            cmd = New SqlCommand("SP_SPC_HourProd", con)
                            cmd.CommandType = Data.CommandType.StoredProcedure
                            cmd.Parameters.Add("@FROMDATE", Data.SqlDbType.DateTime).Direction = Data.ParameterDirection.Input
                            cmd.Parameters.Add("@TODATE", Data.SqlDbType.DateTime).Direction = Data.ParameterDirection.Input
                            cmd.Parameters("@FROMDATE").Value = fromdate
                            cmd.Parameters("@TODATE").Value = todate

                            If Location = "STT" Then
                                Dim hplist As New List(Of Production.HP_ChartRangeSTT)
                                Dim bmap_hp As New BMappers(Of Production.HP_ChartRangeSTT)
                                hplist = bmap_hp.GetSpcSP(cmd)
                                returnobj = hplist
                            ElseIf Location = "CAR" Then
                                Dim hplist As New List(Of Production.HP_ChartRangeCAR)
                                Dim bmap_hp As New BMappers(Of Production.HP_ChartRangeCAR)
                                hplist = bmap_hp.GetSpcSP(cmd)
                                returnobj = hplist
                            End If
                        End Using
                        con.Close()
                    End Using

                Catch ex As Exception

                End Try

            End If

            Return returnobj
        End Function

        Public Function GetHourlyProdRange_Yards(ByVal fromdate As DateTime, ByVal todate As DateTime, ByVal Location As String) As Object
            Dim con As New SqlConnection(dlayer.SPCConnectionString)
            Dim cmd As SqlCommand = con.CreateCommand()
            Dim cid As List(Of CID) = dlayer.GetCIDInfo()
            Dim returnobj As Object = 0

            If fromdate < todate Then
                Try
                    Using con
                        con.Open()
                        Using cmd
                            cmd = New SqlCommand("SP_SPC_HourProd_Yards", con)
                            cmd.CommandType = Data.CommandType.StoredProcedure
                            cmd.Parameters.Add("@FROMDATE", Data.SqlDbType.DateTime).Direction = Data.ParameterDirection.Input
                            cmd.Parameters.Add("@TODATE", Data.SqlDbType.DateTime).Direction = Data.ParameterDirection.Input
                            cmd.Parameters("@FROMDATE").Value = fromdate
                            cmd.Parameters("@TODATE").Value = todate

                            If Location = "STT" Then
                                Dim hplist As New List(Of Production.HP_ChartRangeSTT)
                                Dim bmap_hp As New BMappers(Of Production.HP_ChartRangeSTT)
                                hplist = bmap_hp.GetSpcSP(cmd)
                                returnobj = hplist
                            ElseIf Location = "CAR" Then
                                Dim hplist As New List(Of Production.HP_ChartRangeCAR)
                                Dim bmap_hp As New BMappers(Of Production.HP_ChartRangeCAR)
                                hplist = bmap_hp.GetSpcSP(cmd)
                                returnobj = hplist
                            ElseIf Location = "ALL" Then
                                Dim hplist As New List(Of Production.HP_ChartRangeALL)
                                Dim bmap_hp As New BMappers(Of Production.HP_ChartRangeALL)
                                hplist = bmap_hp.GetSpcSP(cmd)
                                returnobj = hplist
                            End If
                        End Using
                        con.Close()
                    End Using

                Catch ex As Exception

                End Try

            End If

            Return returnobj
        End Function

    End Class

End Namespace