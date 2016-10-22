
Imports MySql.Data.MySqlClient
Imports System.Net
Imports System.IO
Imports System.ComponentModel
Imports C1.C1Excel
Imports OfficeOpenXml
Imports OfficeOpenXml.Drawing.Chart
Imports OfficeOpenXml.Drawing
Imports System.Data

Namespace core

    Public Class ChartData

        Public Shared sheetselect As String
        Public Shared ThirdPAvg As Decimal
        Public Shared Yearcurrent As Integer
        Public Shared TodatePublic As String

        Dim JobsummString1 As String
        Dim JobsummConnection As MySqlConnection
        Dim JobsummReader As MySqlDataReader
        Dim JobsummNewRow As DataRow
        Dim JobsummcmdBuilder As MySqlCommandBuilder
        Dim Jobsummcmd As MySqlCommand
        Public Shared DFPLength As Integer = 0

        Public Function injectdefects(ByRef xlbook As C1XLBook, ByVal sheetnum As Integer, ByVal fromdate As String, ByVal todate As String) As C1XLBook

            Dim record As IDataReader
            Dim sheet1 As XLSheet
            Dim datestyle As XLStyle = New XLStyle(xlbook)
            Dim i As Integer = 4
            Dim j As Integer = 0
            Dim reclength As Integer
            Dim firstyear As Integer = 2014
            Dim firstcol As Integer = 4
            Dim FromDateCur As DateTime = Convert.ToDateTime(fromdate)
            Dim ToDateCur As DateTime = Convert.ToDateTime(todate)
            Dim YearCur As Integer = ToDateCur.Year
            Dim ColCur As Integer
            Dim monthdiv As Decimal

            If IsDate(FromDateCur) = True And IsDate(ToDateCur) = True Then
                monthdiv = getmonthdiff(FromDateCur, ToDateCur)
                Yearcurrent = YearCur
                TodatePublic = todate
            Else
                monthdiv = 1
            End If


            If YearCur >= firstyear Then
                ColCur = firstcol + (YearCur - firstyear)
            Else
                ColCur = 4
            End If


            sheet1 = xlbook.Sheets(sheetselect)

            Try

                JobsummString1 = getquerystring(sheetnum, fromdate, todate)
                JobsummConnection = New MySqlConnection("server=STT-SERVER1-PC;port=3310;user id=akab1;password=hebron;database=Production")
                JobsummConnection.Open()
                Jobsummcmd = New MySqlCommand(JobsummString1, JobsummConnection)
                JobsummReader = Jobsummcmd.ExecuteReader(CommandBehavior.CloseConnection)

            Catch ex As Exception
                MsgBox(ex.Message)
            End Try

            While JobsummReader.Read
                record = CType(JobsummReader, IDataRecord)
                reclength = record.FieldCount

                Try
                    For j = 0 To reclength - 1
                        sheet1(j + 4, ColCur).Value = Convert.ToInt32(record(j)) / monthdiv
                    Next

                Catch ex As Exception
                    MsgBox(ex.Message)
                End Try

            End While

            JobsummConnection.Close()
            JobsummConnection.Dispose()


            Return xlbook

        End Function

        Public Function getquerystring(ByVal sheetnum As Integer, ByVal fromdate As String, ByVal todate As String) As String

            Dim querystring As String

            Dim value As Object = sheetnum
            If Not (value Is Nothing) Then

                Select Case sheetnum
                    Case 4
                        querystring = "SELECT SUM(`BROKEN PICKS`), SUM(`COLORED FLY`),SUM(`BROWN SPOTS/STAINS`)" & vbCrLf &
                                        ",SUM(`WEAVE SEAMS`),SUM(`BLACK SPOTS/STAINS`),SUM(`TORN SELVAGE`), SUM(`FINISH SEAMS`)" & vbCrLf &
                                        ",SUM(`OIL SPOTS`),SUM(`SELVEDGE STRINGS`),SUM(`HOLES`),SUM(`THIN PLACES`),SUM(`FINISH DIRTY HANDLING`)" & vbCrLf &
                                        ",SUM(`NARROW FABRIC`),SUM(`LIGHT OIL`),SUM(`CLIP OUT`),SUM(`BLUE DYE SPOTS/STAINS`),SUM(`GRAY SPOTS/STAINS`)" & vbCrLf &
                                        ",SUM(`SEW DIRTY HANDLING`),SUM(`RED DYE SPOTS/STAINS`),SUM(`SEW SEAMS`),SUM(`SHADED FABRIC`),SUM(`INSIDE TAILS`)" & vbCrLf &
                                        ",SUM(`TEARS`),SUM(`RAW HEMS`),SUM(`PLEATED FABRIC`) FROM jobsummary WHERE FinishTime > '" & fromdate & "' and FinishTime < '" & todate & "' and (Machine = 'STT_TEXPA1' OR Machine = 'STT_TEXPA2')"
                    Case 5
                        querystring = "SELECT SUM(`BROKEN PICKS`), SUM(`COLORED FLY`),SUM(`BROWN SPOTS/STAINS`)" & vbCrLf &
                                        ",SUM(`WEAVE SEAMS`),SUM(`BLACK SPOTS/STAINS`),SUM(`TORN SELVAGE`), SUM(`FINISH SEAMS`)" & vbCrLf &
                                        ",SUM(`OIL SPOTS`),SUM(`SELVEDGE STRINGS`),SUM(`HOLES`),SUM(`THIN PLACES`),SUM(`FINISH DIRTY HANDLING`)" & vbCrLf &
                                        ",SUM(`NARROW FABRIC`),SUM(`LIGHT OIL`),SUM(`CLIP OUT`),SUM(`BLUE DYE SPOTS/STAINS`),SUM(`GRAY SPOTS/STAINS`)" & vbCrLf &
                                        ",SUM(`SEW DIRTY HANDLING`),SUM(`RED DYE SPOTS/STAINS`),SUM(`SEW SEAMS`),SUM(`SHADED FABRIC`),SUM(`INSIDE TAILS`)" & vbCrLf &
                                        ",SUM(`TEARS`),SUM(`RAW HEMS`),SUM(`PLEATED FABRIC`) FROM jobsummary WHERE FinishTime > '" & fromdate & "' and FinishTime < '" & todate & "' and Machine = 'STT_TEXPA3'"
                    Case 6
                        querystring = "SELECT SUM(`BROKEN PICKS`), SUM(`COLORED FLY`),SUM(`BROWN SPOTS/STAINS`)" & vbCrLf &
                                        ",SUM(`WEAVE SEAMS`),SUM(`BLACK SPOTS/STAINS`),SUM(`TORN SELVAGE`), SUM(`FINISH SEAMS`)" & vbCrLf &
                                        ",SUM(`OIL SPOTS`),SUM(`SELVEDGE STRINGS`),SUM(`HOLES`),SUM(`THIN PLACES`),SUM(`FINISH DIRTY HANDLING`)" & vbCrLf &
                                        ",SUM(`NARROW FABRIC`),SUM(`LIGHT OIL`),SUM(`CLIP OUT`),SUM(`BLUE DYE SPOTS/STAINS`),SUM(`GRAY SPOTS/STAINS`)" & vbCrLf &
                                        ",SUM(`SEW DIRTY HANDLING`),SUM(`RED DYE SPOTS/STAINS`),SUM(`SEW SEAMS`),SUM(`SHADED FABRIC`),SUM(`INSIDE TAILS`)" & vbCrLf &
                                        ",SUM(`TEARS`),SUM(`RAW HEMS`),SUM(`PLEATED FABRIC`) FROM jobsummary WHERE FinishTime > '" & fromdate & "' and FinishTime < '" & todate & "' and (Machine = 'STT_PILLOW1' OR Machine = 'STT_PILLOW2')"
                    Case Else
                        querystring = "String Not Found"

                End Select

                Return querystring

            Else

                Return "sheetnum required"

            End If

        End Function

        Public Function getquerystring_Percent(ByVal sheetnum As Integer, ByVal fromdate As String, ByVal todate As String, ByVal Year As String) As String

            Dim querystring As String

            Dim value As Object = sheetnum
            If Not (value Is Nothing) Then

                Select Case sheetnum
                    Case 4
                        querystring = "select sum(TotalDefects)/sum(TotalSewn) * 100,MONTH(FinishTime), dayofmonth(FinishTime)" & vbCrLf &
                                        ",YEAR(FinishTime), dayofyear(FinishTime) from jobsummary where FinishTime > '" & Year & "-01-01 00:00:00' " & vbCrLf &
                                        "and FinishTime < '" & todate & "' and (Machine = 'STT_TEXPA1' OR Machine = 'STT_TEXPA2') GROUP BY dayofyear(FinishTime)"
                    Case 5
                        querystring = "select sum(TotalDefects)/sum(TotalSewn) * 100,MONTH(FinishTime), dayofmonth(FinishTime)" & vbCrLf &
                                        ",YEAR(FinishTime), dayofyear(FinishTime) from jobsummary where FinishTime > '" & Year & "-01-01 00:00:00' " & vbCrLf &
                                        "and FinishTime < '" & todate & "' and Machine = 'STT_TEXPA3' GROUP BY dayofyear(FinishTime)"
                    Case 6
                        querystring = "select sum(TotalDefects)/sum(TotalSewn) * 100,MONTH(FinishTime), dayofmonth(FinishTime)" & vbCrLf &
                                        ",YEAR(FinishTime), dayofyear(FinishTime) from jobsummary where FinishTime > '" & Year & "-01-01 00:00:00' " & vbCrLf &
                                        "and FinishTime < '" & todate & "' and (Machine = 'STT_PILLOW1' OR Machine = 'STT_PILLOW2') GROUP BY dayofyear(FinishTime)"
                    Case Else
                        querystring = "String Not Found"

                End Select

                Return querystring

            Else

                Return "sheetnum required"

            End If

        End Function

        Public Function InsertChart(ByVal excelpack As ExcelPackage, ByVal sheetnum As Integer) As ExcelPackage

            
            Dim xlsheet As ExcelWorksheet
            Dim chart As ExcelBarChart
            Dim valueaddress1 As New ExcelAddress(5, 3, 29, 3)
            Dim valueaddress2 As New ExcelAddress(5, 4, 29, 4)
            Dim valueaddress3 As New ExcelAddress(5, 5, 29, 5)
            Dim series1 As ExcelChartSerie
            Dim series2 As ExcelChartSerie
            Dim series3 As ExcelChartSerie

            xlsheet = excelpack.Workbook.Worksheets(sheetselect)

            chart = xlsheet.Drawings.AddChart("DefectsChart", eChartType.ColumnClustered)

            chart.Title.Text = getcharttitle(sheetnum) + "(" + Convert.ToString(Yearcurrent) + "-01-01 - " + TodatePublic
            chart.SetPosition(5, 5, 10, 10)
            chart.SetSize(1200, 600)

            series1 = chart.Series.Add(valueaddress1.Address, "B5:B29")
            series1.Header = "2012 YTD AVG"
            series2 = chart.Series.Add(valueaddress2.Address, "B5:B29")
            series2.Header = "2013 YTD AVG"
            series3 = chart.Series.Add(valueaddress3.Address, "B5:B29")
            series3.Header = Convert.ToString(Yearcurrent) + " YTD AVG"

            Return excelpack

        End Function

        Public Function InsertChart2(ByRef xlbook As ExcelPackage, ByVal fromdate As String, ByVal todate As String, ByVal sheetnum As Integer) As ExcelPackage


            Dim excelpack As ExcelPackage = xlbook
            Dim xlsheet As ExcelWorksheet
            Dim chart As ExcelLineChart
            Dim valueaddress As New ExcelAddress(36, 3, 36 + DFPLength, 3)
            Dim valueaddress2 As New ExcelAddress(36, 4, 36 + DFPLength, 4)
            'Dim ser As ExcelChartSeries
            Dim fromsplit() As String = Split(fromdate, " ", -1, CompareMethod.Text)
            Dim tosplit() As String = Split(todate, " ", -1, CompareMethod.Text)
            Dim series1 As ExcelChartSerie
            Dim series2 As ExcelChartSerie

            xlsheet = excelpack.Workbook.Worksheets(sheetselect)

            chart = xlsheet.Drawings.AddChart("PercentChart", eChartType.Line)

            chart.Title.Text = getchart2title(sheetnum, fromsplit(0), todate)
            chart.SetPosition(36, 36, 10, 10)
            chart.SetSize(1200, 600)

            series1 = chart.Series.Add(valueaddress.Address, "B36:B" + Convert.ToString(35 + DFPLength))
            series1.Header = "SPC % 3rds"
            series2 = chart.Series.Add(valueaddress2.Address, "B36:B" + Convert.ToString(35 + DFPLength))
            series2.Header = "3rds % Avg"

            chart.Style = eChartStyle.Style26
            chart.XAxis.Format = "Bold"

            Return excelpack

        End Function

        Public Function getmonthdiff(ByVal fromdate As DateTime, ByVal todate As DateTime) As Decimal

            Dim insec, inmonth As Decimal
            Dim secsinmonth As Decimal = 2678400

            insec = DateDiff(DateInterval.Second, fromdate, todate)

            inmonth = insec / secsinmonth

            Return inmonth

        End Function

        Public Function getcharttitle(ByVal sheetnum As Integer) As String

            Dim title As String

            Select Case sheetnum
                Case 4
                    title = "Thomaston AVG Monthly Frequency of 3rds Texpa 1&2 - Flat Sheet Products"
                Case 5
                    title = "Thomaston AVG Monthly Frequency of 3rds Texpa 3 - Fitted Sheets"
                Case 6
                    title = "Thomaston AVG Monthly Frequency of 3rds Pillow 1 & 2 - Pillowcases"
                Case Else
                    title = "No Title Found"
            End Select

            Return title

        End Function

        Public Function getchart2title(ByVal sheetnum As Integer, ByVal fromdate As String, todate As String) As String

            Dim title As String

            Select Case sheetnum
                Case 4
                    title = "Trend of % 3rds of Flat Sheets at Thomaston Auto Cut & Sew (" + fromdate + " - " + todate + ")"
                Case 5
                    title = "Trend of % 3rds of Fitted Sheets at Thomaston Auto Cut & Sew (" + fromdate + " - " + todate + ")"
                Case 6
                    title = "Trend of % 3rds of Pillowcases at Thomaston Auto Cut & Sew (" + fromdate + " - " + todate + ")"
                Case Else
                    title = "No Title Found"
            End Select

            Return title

        End Function

        Public Function injectdefectPercent(ByRef xlbook As C1XLBook, ByVal sheetnum As Integer, ByVal fromdate As String, ByVal todate As String) As C1XLBook

            Dim record As IDataReader
            Dim sheet1 As XLSheet
            Dim datestyle As XLStyle = New XLStyle(xlbook)
            Dim i As Integer = 4
            Dim j As Integer = 0
            Dim reclength As Integer
            Dim FromDateCur As DateTime = Convert.ToDateTime(fromdate)
            Dim ToDateCur As DateTime = Convert.ToDateTime(todate)
            Dim YearCur As Integer = ToDateCur.Year
            Dim Month As String
            Dim Day As String
            Dim Year As String
            Dim thirdavg As Decimal


            sheet1 = xlbook.Sheets(sheetselect)

            Try

                JobsummString1 = getquerystring_Percent(sheetnum, fromdate, todate, Convert.ToString(YearCur))
                JobsummConnection = New MySqlConnection("server=STT-SERVER1-PC;port=3310;user id=akab1;password=hebron;database=Production")
                JobsummConnection.Open()
                Jobsummcmd = New MySqlCommand(JobsummString1, JobsummConnection)
                JobsummReader = Jobsummcmd.ExecuteReader(CommandBehavior.CloseConnection)

            Catch ex As Exception
                MsgBox(ex.Message)
            End Try

            While JobsummReader.Read
                record = CType(JobsummReader, IDataRecord)
                reclength = record.FieldCount

                sheet1(j + 35, 2).Value = Convert.ToDecimal(record(0))
                Month = Convert.ToString(record(1))
                Day = Convert.ToString(record(2))
                Year = Convert.ToString(record(3))
                sheet1(j + 35, 1).Value = Month + "/" + Day + "/" + Year

                thirdavg = thirdavg + Convert.ToDecimal(record(0))

                j = j + 1

            End While

            JobsummConnection.Close()
            JobsummConnection.Dispose()

            DFPLength = j

            ThirdPAvg = (thirdavg / j)

            If IsDBNull(ThirdPAvg) = False And IsNumeric(ThirdPAvg) = True Then
                For x = 0 To j
                    sheet1(x + 35, 3).Value = ThirdPAvg
                Next
            Else
                ThirdPAvg = 3.2
                For x = 0 To j
                    sheet1(x + 35, 3).Value = ThirdPAvg
                Next
            End If

            For x = 0 To reclength - 1

            Next

            Return xlbook

        End Function


    End Class


End Namespace
