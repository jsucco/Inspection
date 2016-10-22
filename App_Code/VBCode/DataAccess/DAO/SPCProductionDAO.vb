Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Data


Namespace core
    Public Class SPCProductionDAO
        Private DL As New dlayer

        Dim dr As SqlDataReader
        Dim record As IDataRecord

        Public Function GetJobSummaryObjects(ByVal cmd As SqlCommand, ByVal con As SqlConnection) As List(Of SPCInspection.JobSummary_old)

            Dim readerlist As New List(Of SPCInspection.JobSummary_old)

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
                readerlist.Add(New SPCInspection.JobSummary_old With {.idJobSummary = Convert.ToInt64(record(0)), .WorkOrder = Convert.ToString(record(1)), .DataNo = Convert.ToString(record(2)), .Description = Convert.ToString(record(3)), .Machine = Convert.ToString(record(4)), .TotalSewn = Convert.ToInt64(record(5)), .TotalDefects = Convert.ToInt64(record(6)), .TotalDefectPercentage = Convert.ToDecimal(record(7)), .TotalWeaveDefects = Convert.ToInt32(record(8)), .TotalFinishingDefects = Convert.ToInt32(record(9)), .TotalSewDefects = Convert.ToInt32(record(10)), .DefectsPerHundredYards = Convert.ToDecimal(record(11)), .TotalYards = Convert.ToDecimal(record(12)), .WEAVE_SEAMS = Convert.ToInt32(record(13)), .SELVEDGE_STRINGS = Convert.ToInt32(record(14)), .INSIDE_TAILS = Convert.ToInt32(record(15)), .BROKEN_PICKS = Convert.ToInt32(record(16)), .THIN_PLACES = Convert.ToInt32(record(17)), .OIL_SPOTS = Convert.ToInt32(record(18)), .RED_DYE_SPOTSSTAINS = Convert.ToInt32(record(19)), .BLUE_DYE_SPOTSSTAINS = Convert.ToInt32(record(20)), .GRAY_SPOTSSTAINS = Convert.ToInt32(record(21)), .BLACK_SPOTSSTAINS = Convert.ToInt32(record(22)), .BROWN_SPOTSSTAINS = Convert.ToInt32(record(23)), .FINISH_DIRTY_HANDLING = Convert.ToInt32(record(24)), .SHADED_FABRIC_HANDLING = Convert.ToInt32(record(25)), .NARROW_FABRIC = Convert.ToInt32(record(26)), .CLIP_OUT = Convert.ToInt32(record(27)), .TORN_SELVAGE = Convert.ToInt32(record(28)), .FINISH_SEAMS = Convert.ToInt32(record(29)), .HOLES = Convert.ToInt32(record(30)), .PLEATED_FABRIC = Convert.ToInt32(record(31)), .TEARS = Convert.ToInt32(record(32)), .RAW_HEMS = Convert.ToInt32(record(33)), .LIGHT_OIL = Convert.ToInt32(record(34)), .SEW_SEAMS = Convert.ToInt32(record(35)), .SEW_DIRTY_HANDLING = Convert.ToInt32(record(36)), .COLORED_FLY = Convert.ToInt32(record(37)), .FinishTime = Convert.ToDateTime(record(38)), .RunTime = Convert.ToDecimal(record(39)), .DownTime = Convert.ToDecimal(record(40)), .CutlengthOverage = Convert.ToDecimal(record(41)), .RunTimeEfficiency = Convert.ToDecimal(record(42)), .AvgSheetsPerHour = Convert.ToDecimal(record(43)), .Updated = Convert.ToDateTime(record(44)), .Roll_OperatorList = Convert.ToString(record(45))})

            End While

            dr.Close()
            cmd.Dispose()
            'con.Close()

            Return readerlist

        End Function

        Public Function GetDefectsSum(ByVal cmd As SqlCommand, ByVal con As SqlConnection) As List(Of SPCInspection.DefectGroup)

            Dim readerlist As New List(Of SPCInspection.DefectGroup)

            'Dim con As New SqlConnection(dl.APRConnectionString(3))
            Try
                'con.Open()
                dr = cmd.ExecuteReader()
            Catch ex As Exception
                Return readerlist
            End Try


            While dr.Read
                record = CType(dr, IDataRecord)
                If readerlist.Count > 0 Then
                    readerlist.Clear()
                End If

                Dim length As Integer = record.FieldCount
                For i = 0 To length - 1
                    readerlist.Add(New SPCInspection.DefectGroup With {.DefectType = dr.GetName(i), .Count = Convert.ToInt64(record(i))})
                Next

            End While

            dr.Close()
            cmd.Dispose()
            'con.Close()

            Return readerlist

        End Function

        Public Function GetDefectsPercentage(ByVal cmd As SqlCommand, ByVal con As SqlConnection) As List(Of SPCInspection.DefectsPercentage)

            Dim readerlist As New List(Of SPCInspection.DefectsPercentage)

            'Dim con As New SqlConnection(dl.APRConnectionString(3))
            Try
                'con.Open()
                dr = cmd.ExecuteReader()
            Catch ex As Exception
                Return readerlist
            End Try


            While dr.Read
                record = CType(dr, IDataRecord)
                If readerlist.Count > 0 Then
                    readerlist.Clear()
                End If

                Dim length As Integer = record.FieldCount
                For i = 0 To length - 1
                    readerlist.Add(New SPCInspection.DefectsPercentage With {.pvalue = Convert.ToDecimal(record(0)), .Month = Convert.ToInt32(record(1)), .Day = Convert.ToInt32(record(2)), .year = Convert.ToInt32(record(3))})
                Next

            End While

            dr.Close()
            cmd.Dispose()
            'con.Close()

            Return readerlist

        End Function


        Public Function GetDefectsGroup(ByVal WorkOrder As String, ByVal GroupBy As String) As Dictionary(Of Integer, SPCInspection.DefectGroup)
            Dim con As New SqlConnection(DL.InspectConnectionString())
            Dim cmd As SqlCommand = con.CreateCommand()
            Try
                Using con
                    con.Open()
                    Using cmd
                        cmd = New SqlCommand("SP_SPC_GetDefectGroups", con)
                        cmd.CommandType = CommandType.StoredProcedure
                        cmd.Parameters.Add("@workorder", SqlDbType.VarChar).Direction = ParameterDirection.Input
                        cmd.Parameters.Add("@GroupBy", SqlDbType.VarChar).Direction = ParameterDirection.Input
                        cmd.Parameters("@workorder").Value = WorkOrder
                        cmd.Parameters("@GroupBy").Value = GroupBy
                        cmd.CommandTimeout = 5000

                        Dim GroupDictionary As Dictionary(Of Integer, SPCInspection.DefectGroup)
                    End Using
                End Using
            Catch ex As Exception
                Throw New System.Exception(ex.Message)
            End Try

            Return Nothing
        End Function

        Public Function GetInjectDefectsQueryString(ByVal sheetnum As Integer, ByVal fromdate As String, ByVal todate As String) As String
            Dim querystring As String
            If IsDate(fromdate) = True And IsDate(todate) = True Then
                querystring = "SELECT SUM(`BROKEN PICKS`), SUM(`COLORED FLY`),SUM(`BROWN SPOTS/STAINS`)" & vbCrLf &
                                        ",SUM(`WEAVE SEAMS`),SUM(`BLACK SPOTS/STAINS`),SUM(`TORN SELVAGE`), SUM(`FINISH SEAMS`)" & vbCrLf &
                                        ",SUM(`OIL SPOTS`),SUM(`SELVEDGE STRINGS`),SUM(`HOLES`),SUM(`THIN PLACES`),SUM(`FINISH DIRTY HANDLING`)" & vbCrLf &
                                        ",SUM(`NARROW FABRIC`),SUM(`LIGHT OIL`),SUM(`CLIP OUT`),SUM(`BLUE DYE SPOTS/STAINS`),SUM(`GRAY SPOTS/STAINS`)" & vbCrLf &
                                        ",SUM(`SEW DIRTY HANDLING`),SUM(`RED DYE SPOTS/STAINS`),SUM(`SEW SEAMS`),SUM(`SHADED FABRIC`),SUM(`INSIDE TAILS`)" & vbCrLf &
                                        ",SUM(`TEARS`),SUM(`RAW HEMS`),SUM(`PLEATED FABRIC`) FROM jobsummary WHERE FinishTime > '" & fromdate & "' and FinishTime < '" & todate & "'"
                Select Case sheetnum
                    Case 4
                        querystring = querystring + " and (Machine = 'STT_TEXPA1' OR Machine = 'STT_TEXPA2')"
                    Case 5
                        querystring = querystring + " and Machine = 'STT_TEXPA3'"
                    Case 6
                        querystring = querystring + " and (Machine = 'STT_PILLOW1' OR Machine = 'STT_PILLOW2')"
                End Select

                Return querystring
            Else
                Return Nothing
            End If

        End Function

        Public Function GetDefectPercentageQueryString(ByVal sheetnum As Integer, ByVal fromdate As String, ByVal todate As String, ByVal year As String) As String
            Dim querystring As String

            If IsDate(fromdate) = True And IsDate(todate) = True Then
                querystring = "select sum(TotalDefects)/sum(TotalSewn) * 100,MONTH(FinishTime), dayofmonth(FinishTime)" & vbCrLf &
                                        ",YEAR(FinishTime), dayofyear(FinishTime) from JobSummary where FinishTime > '" & year & "-01-01 00:00:00' " & vbCrLf &
                                        "and FinishTime < '" & todate & "'"
                Select Case sheetnum
                    Case 4
                        querystring = querystring + " and (Machine = 'STT_TEXPA1' OR Machine = 'STT_TEXPA2') GROUP BY dayofyear(FinishTime)"
                    Case 5
                        querystring = querystring + " and Machine = 'STT_TEXPA3' GROUP BY dayofyear(FinishTime)"
                    Case 6
                        querystring = querystring + " and (Machine = 'STT_PILLOW1' OR Machine = 'STT_PILLOW2') GROUP BY dayofyear(FinishTime)"
                    Case Else
                        querystring = querystring + " GROUP BY dayofyear(FinishTime)"
                End Select
                Return querystring
            Else
                Return Nothing
            End If
        End Function
    End Class
End Namespace

