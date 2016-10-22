
Imports System.Net.Mail
Imports System.Net
Imports System.IO
Imports System.ComponentModel
Imports C1.C1Excel
Imports System.Data
Imports System.Data.SqlClient
Imports System.Drawing
Imports System.Reflection
Imports OfficeOpenXml
Imports OfficeOpenXml.Drawing.Chart
Imports OfficeOpenXml.Drawing

Namespace core

    Public Class SPCReports

        Public ErrorMessage As String
        Public pfromdate As String
        Public ptodate As String
        Public Sheetselect As String
        Public DFPLength As Integer = 0
        Public Yearcurrent As Integer = DateTime.Now.Year
        Public TodatePublic As String
        Public LoomNoList As New List(Of SPCInspection.RollLoom)
        Public CollLetters() As String = {"A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z", "AA", "AB", "AC", "AD", "AE", "AF", "AG", "AH", "AI", "AJ", "AK", "AL", "AM", "AN", "AO", "AP"}
        Private DL As New dlayer
        Private util As New Utilities
        Private prod As New SPCProductionDAO
        Private IU As New InspectionUtilityDAO

        Dim ColArray() As String = {"F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z", "AA", "AB", "AC", "AD", "AE", "AF", "AG", "AH", "AI", "AJ", "AK", "AL", "AM", "AN", "AO", "AP"}
        Dim lastrow As Integer = 0
        Dim lastcol As Integer = 0

        Public Function Greige_InjectLoomData(ByRef xlbook As C1XLBook) As C1XLBook
            Dim LoomSet As DataSet = New DataSet
            Dim sql As String
            Dim sheet1 As XLSheet

            sheet1 = xlbook.Sheets(Sheetselect)

            sql = "SELECT DISTINCT LoomNo, RollNo" & vbCrLf &
            "FROM DefectMaster" & vbCrLf &
            "WHERE (NOT (LoomNo = '0')) AND (NOT (LoomNo IS NULL))"

            If util.FillSPCDataSet(LoomSet, "LoomSet", sql) = False Then
                Return xlbook
            End If
            Dim cnt As Integer = 1
            For Each drow As DataRow In LoomSet.Tables(0).Rows
                LoomNoList.Add(New SPCInspection.RollLoom With {.LoomNo = drow(0), .RollNo = drow(1)})

                sheet1(4, cnt).Value = Convert.ToString(drow(0))
                cnt = cnt + 2
            Next

            Return xlbook
        End Function

        Public Function Greige_InjectLoomYds(ByRef xlbook As C1XLBook) As C1XLBook
            Dim sql As String
            Dim cnt As Integer = 1
            Dim sheet1 As XLSheet

            sheet1 = xlbook.Sheets(Sheetselect)

            If LoomNoList.Count > 0 Then
                For Each item As SPCInspection.RollLoom In LoomNoList
                    Dim RollYds As DataSet = New DataSet

                    sql = "SELECT TotalYds" & vbCrLf &
                    "FROM RollProduction" & vbCrLf &
                    "WHERE (RollNo = '" & item.RollNo & "')"

                    If util.FillSPCDataSet(RollYds, "RollYds", sql) = False Then
                        Return xlbook
                    End If
                    Dim ThisRollYds As Integer

                    If RollYds.Tables(0).Rows.Count > 0 Then
                        ThisRollYds = Convert.ToInt64(RollYds.Tables(0).Rows(0)(0))
                        sheet1(6, cnt).Value = ThisRollYds
                        cnt = cnt + 2
                    End If
                Next
            End If

            Return xlbook
        End Function

        Public Function Texpa_InjectJobSummary(ByRef xlbook As ExcelWorkbook, ByVal fromdate As DateTime, ByVal todate As DateTime, ByVal sheetnum As Integer) As ExcelWorkbook
            Dim con As New SqlConnection(DL.InspectConnectionString())
            Dim cmd As SqlCommand = con.CreateCommand()
            Dim todatestring As String = todate.ToString("yyyy-MM-dd H:mm:ss").Split(" ")(0) + " 23:59:59"
            Dim todateform As DateTime = DateTime.Parse(todatestring)
            Dim sheet1 As ExcelWorksheet
            ' Dim colstyle As XLStyle = New XLStyle(xlbook)
            'Dim datestyle As XLStyle = New XLStyle(xlbook)

            Try
                Using con
                    con.Open()
                    Using cmd
                        cmd = New SqlCommand("SP_SPC_GetJobSummary", con)
                        cmd.CommandType = CommandType.StoredProcedure
                        cmd.Parameters.Add("@fromdate", SqlDbType.DateTime).Direction = ParameterDirection.Input
                        cmd.Parameters.Add("@todate", SqlDbType.DateTime).Direction = ParameterDirection.Input
                        cmd.Parameters.Add("@filternumber", SqlDbType.VarChar).Direction = ParameterDirection.Input
                        cmd.Parameters("@fromdate").Value = fromdate
                        cmd.Parameters("@todate").Value = todateform
                        cmd.Parameters("@filternumber").Value = sheetnum
                        cmd.CommandTimeout = 5000

                        Dim jobarray As Array = prod.GetJobSummaryObjects(cmd, con).ToArray()

                        If jobarray.Length > 0 Then
                            sheet1 = xlbook.Worksheets(sheetnum)
                            'sheet1 = xlbook.Sheets(Sheetselect)

                            Dim jsobj As New SPCInspection.JobSummary
                            Dim bustype As Type = jsobj.GetType()
                            Dim jsprop As System.Reflection.PropertyInfo() = bustype.GetProperties()
                            Dim colint As Integer = 1
                            For Each info As PropertyInfo In jsprop
                                sheet1.Cells(1, colint).Value = info.Name.ToUpper()

                                colint += 1
                            Next
                            sheet1.Cells(1, 1, 1, colint).Style.Font.Bold = True

                            Dim Ilast As Integer = 1
                            For I = 2 To jobarray.Length + 1

                                sheet1.Cells(I, 1).Value = Convert.ToInt32(jobarray(I - 2).idJobSummary)
                                sheet1.Cells(I, 2).Value = Convert.ToInt32(jobarray(I - 2).WorkOrder)
                                sheet1.Cells(I, 3).Value = Convert.ToString(jobarray(I - 2).DataNo)
                                sheet1.Cells(I, 4).Value = Convert.ToString(jobarray(I - 2).Description)
                                sheet1.Cells(I, 5).Value = Convert.ToString(jobarray(I - 2).Machine)
                                sheet1.Cells(I, 6).Value = Convert.ToInt32(jobarray(I - 2).TotalSewn)
                                sheet1.Cells(I, 7).Value = Convert.ToInt32(jobarray(I - 2).TotalDefects)
                                sheet1.Cells(I, 8).Value = Convert.ToDouble(jobarray(I - 2).TotalDefectPercentage)
                                sheet1.Cells(I, 9).Formula = "IF(M" + I.ToString() + ">0,G" + I.ToString() + "/(M" + I.ToString() + "/100),0)"
                                sheet1.Cells(I, 9).Style.Numberformat.Format = "#.###"
                                sheet1.Cells(I, 10).Value = Convert.ToInt32(jobarray(I - 2).TotalWeaveDefects)
                                sheet1.Cells(I, 11).Value = Convert.ToInt32(jobarray(I - 2).TotalFinishingDefects)
                                sheet1.Cells(I, 12).Value = Convert.ToInt32(jobarray(I - 2).TotalSewDefects)
                                sheet1.Cells(I, 13).Value = Convert.ToDouble(jobarray(I - 2).TotalYards)

                                'sheet1.Cells(I, 12).Style = colstyle

                                sheet1.Cells(I, 14).Value = Convert.ToInt32(jobarray(I - 2).WEAVE_SEAMS)
                                sheet1.Cells(I, 15).Value = Convert.ToInt32(jobarray(I - 2).SELVEDGE_STRINGS)
                                sheet1.Cells(I, 16).Value = Convert.ToInt32(jobarray(I - 2).INSIDE_TAILS)
                                sheet1.Cells(I, 17).Value = Convert.ToInt32(jobarray(I - 2).BROKEN_PICKS)
                                sheet1.Cells(I, 18).Value = Convert.ToInt32(jobarray(I - 2).THIN_PLACES)
                                sheet1.Cells(I, 19).Value = Convert.ToInt32(jobarray(I - 2).OIL_SPOTS)
                                sheet1.Cells(I, 20).Value = Convert.ToInt32(jobarray(I - 2).RED_DYE_SPOTSSTAINS)
                                sheet1.Cells(I, 21).Value = Convert.ToInt32(jobarray(I - 2).BLUE_DYE_SPOTSSTAINS)
                                sheet1.Cells(I, 22).Value = Convert.ToInt32(jobarray(I - 2).GRAY_SPOTSSTAINS)
                                sheet1.Cells(I, 23).Value = Convert.ToInt32(jobarray(I - 2).BLACK_SPOTSSTAINS)
                                sheet1.Cells(I, 24).Value = Convert.ToInt32(jobarray(I - 2).BROWN_SPOTSSTAINS)
                                sheet1.Cells(I, 25).Value = Convert.ToInt32(jobarray(I - 2).FINISH_DIRTY_HANDLING)
                                sheet1.Cells(I, 26).Value = Convert.ToInt32(jobarray(I - 2).SHADED_FABRIC_HANDLING)
                                sheet1.Cells(I, 27).Value = Convert.ToInt32(jobarray(I - 2).NARROW_FABRIC)
                                sheet1.Cells(I, 28).Value = Convert.ToInt32(jobarray(I - 2).CLIP_OUT)
                                sheet1.Cells(I, 29).Value = Convert.ToInt32(jobarray(I - 2).TORN_SELVAGE)
                                sheet1.Cells(I, 30).Value = Convert.ToInt32(jobarray(I - 2).FINISH_SEAMS)
                                sheet1.Cells(I, 31).Value = Convert.ToInt32(jobarray(I - 2).HOLES)
                                sheet1.Cells(I, 32).Value = Convert.ToInt32(jobarray(I - 2).PLEATED_FABRIC)
                                sheet1.Cells(I, 33).Value = Convert.ToInt32(jobarray(I - 2).RAW_HEMS)
                                sheet1.Cells(I, 34).Value = Convert.ToInt32(jobarray(I - 2).TEARS)
                                sheet1.Cells(I, 35).Value = Convert.ToInt32(jobarray(I - 2).LIGHT_OIL)
                                sheet1.Cells(I, 36).Value = Convert.ToInt32(jobarray(I - 2).SEW_SEAMS)
                                sheet1.Cells(I, 37).Value = Convert.ToInt32(jobarray(I - 2).SEW_DIRTY_HANDLING)
                                sheet1.Cells(I, 38).Value = Convert.ToInt32(jobarray(I - 2).COLORED_FLY)
                                sheet1.Cells(I, 39).Value = Convert.ToDateTime(jobarray(I - 2).FinishTime)
                                sheet1.Cells(I, 39).Style.Numberformat.Format = "yyyy-mm-dd hh:mm:ss"
                                'sheet1.Cells(I, 39).Style = datestyle
                                sheet1.Cells(I, 40).Value = Convert.ToDouble(jobarray(I - 2).RunTime)
                                sheet1.Cells(I, 41).Value = Convert.ToDouble(jobarray(I - 2).DownTime)
                                sheet1.Cells(I, 42).Value = Convert.ToDouble(jobarray(I - 2).CutlengthOverage)
                                sheet1.Cells(I, 43).Value = Convert.ToDouble(jobarray(I - 2).RunTimeEfficiency)
                                sheet1.Cells(I, 44).Value = Convert.ToDouble(jobarray(I - 2).AvgSheetsPerHour)
                                sheet1.Cells(I, 45).Value = Convert.ToDateTime(jobarray(I - 2).Updated)
                                sheet1.Cells(I, 45).Style.Numberformat.Format = "yyyy-mm-dd hh:mm:ss"
                                'sheet1.Cells(I, 45).Style = datestyle
                                sheet1.Cells(I, 46).Value = Convert.ToString(jobarray(I - 2).Roll_OperatorList)

                                Ilast = I
                            Next
                            lastrow = Ilast
                        End If

                    End Using
                End Using
            Catch ex As Exception

            End Try
            Return xlbook
        End Function

        Public Function InjectDefects(ByRef xlbook As ExcelWorkbook, ByVal sheetnum As Integer, ByVal fromdate As String, ByVal todate As String) As ExcelWorkbook

            'Dim sheet1 As XLSheet
            Dim sheet1 As ExcelWorksheet
            Dim sqlstring As String = prod.GetInjectDefectsQueryString(sheetnum, fromdate, todate)
            Dim firstyear As Integer = 2014
            Dim firstcol As Integer = 4
            Dim monthdiv As Decimal
            Dim FromDateCur As DateTime = Convert.ToDateTime(fromdate)
            Dim ToDateCur As DateTime = Convert.ToDateTime(todate)
            Dim ColCur As Integer

            'sheet1 = xlbook.Sheets(Sheetselect)
            sheet1 = xlbook.Worksheets(Sheetselect)
            TodatePublic = todate
            If Not String.IsNullOrEmpty(sqlstring) Then
                Using conn As New SqlConnection(DL.InspectConnectionString)
                    conn.Open()
                    Using cmd As New SqlCommand
                        Dim returnarray As Array = prod.GetDefectsSum(cmd, conn).ToArray()
                        If returnarray.Length = 1 Then
                            Dim YearCur As Integer = ToDateCur.Year
                            monthdiv = getmonthdiff(FromDateCur, ToDateCur)

                            If YearCur >= firstyear Then
                                ColCur = firstcol + (YearCur - firstyear)
                            Else
                                ColCur = 4
                            End If

                            Dim reclength As Integer = returnarray.Length
                            For i = 0 To reclength - 1
                                sheet1.Cells(i + 5, ColCur + 1).Value = Convert.ToInt64(returnarray(i).Count) / monthdiv
                            Next
                        End If
                    End Using
                End Using
            End If

            Return xlbook
        End Function

        Public Function InjectDefectsPercentage(ByRef xlbook As ExcelWorkbook, ByVal sheetnum As Integer, ByVal fromdate As String, ByVal todate As String) As ExcelWorkbook

            'Dim sheet1 As XLSheet
            Dim sheet1 As ExcelWorksheet
            Dim sqlstring As String
            Dim firstyear As Integer = 2014
            Dim firstcol As Integer = 4
            Dim FromDateCur As DateTime = Convert.ToDateTime(fromdate)
            Dim ToDateCur As DateTime = Convert.ToDateTime(todate)
            Dim YearCur As Integer = ToDateCur.Year
            Dim Month As String
            Dim Day As String
            Dim Year As String
            Dim thirdavg As Decimal
            Dim Third_avg As Decimal
            Dim j As Integer = 0
            'sheet1 = xlbook.Sheets(Sheetselect)
            sheet1 = xlbook.Worksheets(Sheetselect)
            sqlstring = prod.GetDefectPercentageQueryString(sheetnum, fromdate, todate, Convert.ToString(YearCur))

            Using conn As New SqlConnection(DL.InspectConnectionString)
                conn.Open()
                Using cmd As New SqlCommand(sqlstring, conn)

                    Dim returnarray As Array = prod.GetDefectsPercentage(cmd, conn).ToArray()

                    If returnarray.Length > 0 Then
                        For Each item In returnarray
                            sheet1.Cells(j + 36, 3).Value = item.pvalue
                            Month = Convert.ToString(item.Month)
                            Day = Convert.ToString(item.Day)
                            Year = Convert.ToString(item.Year)
                            sheet1.Cells(j + 36, 2).Value = Month + "/" + Day + "/" + Year
                            thirdavg = thirdavg + item.pvalue
                        Next




                        Third_avg = thirdavg / returnarray.Length

                        If IsDBNull(Third_avg) = False And IsNumeric(Third_avg) = True Then
                            For x = 0 To returnarray.Length - 1
                                sheet1.Cells(x + 36, 4).Value = Third_avg
                            Next
                        Else
                            Third_avg = 3.2
                            For x = 0 To returnarray.Length - 1
                                sheet1.Cells(x + 36, 4).Value = Third_avg
                            Next
                        End If
                    End If
                End Using
            End Using

            Return xlbook
        End Function

        Private Function getmonthdiff(ByVal fromdate As DateTime, ByVal todate As DateTime) As Decimal

            Dim insec, inmonth As Decimal
            Dim secsinmonth As Decimal = 2678400

            insec = DateDiff(DateInterval.Second, fromdate, todate)

            inmonth = insec / secsinmonth

            Return inmonth

        End Function

        Public Function injectsubstats(ByRef xlbook As ExcelWorkbook, ByVal Todate As String, ByVal sheetnum As Integer) As ExcelWorkbook


            Dim Sheet1 As ExcelWorksheet
            Dim subsrow As Integer
            'Dim SS1Style As XLStyle = New XLStyle(xlbook)
            'Dim SS2Style As XLStyle = New XLStyle(xlbook)
            'Dim SS3Style As XLStyle = New XLStyle(xlbook)
            Sheet1 = xlbook.Worksheets(sheetnum)



            'SS3Style.Font = New Font("Calibri", 11, FontStyle.Bold)
            'SS3Style.BackColor = Color.Yellow
            'SS3Style.SetBorderStyle(XLLineStyleEnum.Thick)
            'SS3Style.Format = "MM/DD/YYYY"

            'SS1Style.Font = New Font("Calibri", 11, FontStyle.Bold)
            'SS1Style.BackColor = Color.LightBlue
            'SS1Style.SetBorderStyle(XLLineStyleEnum.Thick)

            'SS2Style.Font = New Font("Calibri", 11, FontStyle.Bold)
            'SS2Style.BackColor = Color.LightGreen
            'SS2Style.SetBorderStyle(XLLineStyleEnum.Thick)
            'SS2Style.Format = "#.##%"


            subsrow = lastrow + 1
            'Sheet1 = xlbook.Sheets(Sheetselect)
            Sheet1.Cells(lastrow + 1, 1).Value = Convert.ToDateTime(Todate)
            Sheet1.Cells(lastrow + 1, 1).Style.Font.Bold = True
            Sheet1.Cells(lastrow + 1, 1).Style.Fill.PatternType = Style.ExcelFillStyle.Solid
            Sheet1.Cells(lastrow + 1, 1).Style.Fill.BackgroundColor.SetColor(Color.Yellow)
            Sheet1.Cells(lastrow + 1, 1).Style.Numberformat.Format = "yyyy-mm-dd"
            'Sheet1(lastrow + 1, 1).Style = SS3Style
            Sheet1.Cells(lastrow + 1, 5).Value = "TOTAL"
            'Sheet1(lastrow + 1, 4).Style = SS1Style
            Sheet1.Cells(lastrow + 1, 4).Style.Font.Bold = True
            Sheet1.Cells(lastrow + 1, 4).Style.Fill.PatternType = Style.ExcelFillStyle.Solid
            Sheet1.Cells(lastrow + 1, 4).Style.Fill.BackgroundColor.SetColor(Color.LightBlue)

            For i = 0 To 33
                Sheet1.Cells(lastrow + 1, i + 6).Formula = "SUBTOTAL(9," + ColArray(i) + "2:" + ColArray(i) + Convert.ToString(lastrow) + ")"
                Sheet1.Cells(lastrow + 1, i + 6).Style.Font.Bold = True
                Sheet1.Cells(lastrow + 1, i + 6).Style.Fill.PatternType = Style.ExcelFillStyle.Solid
                Sheet1.Cells(lastrow + 1, i + 6).Style.Fill.BackgroundColor.SetColor(Color.LightBlue)
                Select Case i
                    Case 0, 1, 2, 3, 7, 8, 9
                        GoTo 1010
                    Case Else
                        Sheet1.Cells(lastrow + 2, i + 6).Formula = "IF($G" + Convert.ToString(subsrow) + "=0,0," + ColArray(i) + Convert.ToString(subsrow) + "/$G" + Convert.ToString(subsrow) + ")"
                        Sheet1.Cells(lastrow + 2, i + 6).Style.Numberformat.Format = "#.##%"
                        Sheet1.Cells(lastrow + 1, i + 6).Style.Font.Bold = True
                        Sheet1.Cells(lastrow + 1, i + 6).Style.Fill.PatternType = Style.ExcelFillStyle.Solid
                        Sheet1.Cells(lastrow + 1, i + 6).Style.Fill.BackgroundColor.SetColor(Color.LightGreen)
                End Select
1010:
            Next
            Sheet1.Cells(1, 1, lastrow + 2, 40).AutoFitColumns()

            'AutoSizeColumns(Sheet1, xlbook)

            Return xlbook

        End Function

        Public Function injectsubstats_2(ByRef epack As ExcelWorkbook, ByVal Todate As DateTime, ByVal sheetnum As Integer) As ExcelWorkbook


            Dim Sheet1 As ExcelWorksheet = epack.Worksheets(1)
            Dim subsrow As Integer
            'Dim SS1Style As XLStyle = New XLStyle(xlbook)
            'Dim SS2Style As XLStyle = New XLStyle(xlbook)
            'Dim SS3Style As XLStyle = New XLStyle(xlbook)
            'Sheet1 = epack.Workbook.Worksheets(sheetnum)

            Dim ColArray() As String = {"E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z", "AA", "AB", "AC", "AD", "AE", "AF", "AG", "AH", "AI", "AJ", "AK", "AL", "AM", "AN", "AO", "AP"}

            'SS3Style.Font = New Font("Calibri", 11, FontStyle.Bold)
            'SS3Style.BackColor = Color.Yellow
            'SS3Style.SetBorderStyle(XLLineStyleEnum.Thick)
            'SS3Style.Format = "MM/DD/YYYY"

            'SS1Style.Font = New Font("Calibri", 11, FontStyle.Bold)
            'SS1Style.BackColor = Color.LightBlue
            'SS1Style.SetBorderStyle(XLLineStyleEnum.Thick)

            'SS2Style.Font = New Font("Calibri", 11, FontStyle.Bold)
            'SS2Style.BackColor = Color.LightGreen
            'SS2Style.SetBorderStyle(XLLineStyleEnum.Thick)
            'SS2Style.Format = "#.##%"

            Try
                subsrow = lastrow + 1
                'Sheet1 = xlbook.Sheets(Sheetselect)
                Sheet1.Cells(lastrow, 1).Value = Todate
                Sheet1.Cells(lastrow, 1).Style.Font.Bold = True
                Sheet1.Cells(lastrow, 1).Style.Fill.PatternType = Style.ExcelFillStyle.Solid
                Sheet1.Cells(lastrow, 1).Style.Fill.BackgroundColor.SetColor(Color.Yellow)
                Sheet1.Cells(lastrow, 1).Style.Numberformat.Format = "yyyy-mm-dd"
                'Sheet1(lastrow + 1, 1).Style = SS3Style
                Sheet1.Cells(lastrow, 4).Value = "TOTAL"
                'Sheet1(lastrow + 1, 4).Style = SS1Style
                Sheet1.Cells(lastrow, 4).Style.Font.Bold = True
                Sheet1.Cells(lastrow, 4).Style.Fill.PatternType = Style.ExcelFillStyle.Solid
                Sheet1.Cells(lastrow, 4).Style.Fill.BackgroundColor.SetColor(Color.LightBlue)

                For i = 0 To lastcol - 6

                    Select Case i
                        Case 2, 6
                            Sheet1.Cells(lastrow, i + 5).Formula = "AVERAGE(9," + ColArray(i) + "2:" + ColArray(i) + Convert.ToString(lastrow - 1) + ")"
                            Sheet1.Cells(lastrow, i + 5).Style.Numberformat.Format = "#.##"
                            Sheet1.Cells(lastrow, i + 5).Style.Font.Bold = True
                            Sheet1.Cells(lastrow, i + 5).Style.Fill.PatternType = Style.ExcelFillStyle.Solid
                            Sheet1.Cells(lastrow, i + 5).Style.Fill.BackgroundColor.SetColor(Color.LightGreen)
                        Case Else
                            Sheet1.Cells(lastrow, i + 5).Formula = "SUBTOTAL(9," + ColArray(i) + "2:" + ColArray(i) + Convert.ToString(lastrow - 1) + ")"
                            Sheet1.Cells(lastrow, i + 5).Style.Font.Bold = True
                            Sheet1.Cells(lastrow, i + 5).Style.Fill.PatternType = Style.ExcelFillStyle.Solid
                            Sheet1.Cells(lastrow, i + 5).Style.Fill.BackgroundColor.SetColor(Color.LightBlue)
                    End Select
1010:
                Next
                Sheet1.Cells(1, 1, lastrow + 2, 40).AutoFitColumns()
            Catch ex As Exception

            End Try
            'AutoSizeColumns(Sheet1, xlbook)

            Return epack

        End Function

        Public Function InsertChart(ByRef excelpack As ExcelWorkbook, ByVal sheetnum As Integer) As ExcelWorkbook


            Dim xlsheet As ExcelWorksheet
            Dim chart As ExcelBarChart
            Dim valueaddress1 As New ExcelAddress(5, 3, 29, 3)
            Dim valueaddress2 As New ExcelAddress(5, 4, 29, 4)
            Dim valueaddress3 As New ExcelAddress(5, 5, 29, 5)
            Dim series1 As ExcelChartSerie
            Dim series2 As ExcelChartSerie
            Dim series3 As ExcelChartSerie

            xlsheet = excelpack.Worksheets(Sheetselect)

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

        Public Function InsertChart2(ByRef excelpack As ExcelWorkbook, ByVal fromdate As String, ByVal todate As String, ByVal sheetnum As Integer) As ExcelWorkbook



            Dim xlsheet As ExcelWorksheet
            Dim chart As ExcelLineChart
            Dim valueaddress As New ExcelAddress(36, 3, 36 + DFPLength, 3)
            Dim valueaddress2 As New ExcelAddress(36, 4, 36 + DFPLength, 4)
            'Dim ser As ExcelChartSeries
            Dim fromsplit() As String = Split(fromdate, " ", -1, CompareMethod.Text)
            Dim tosplit() As String = Split(todate, " ", -1, CompareMethod.Text)
            Dim series1 As ExcelChartSerie
            Dim series2 As ExcelChartSerie

            xlsheet = excelpack.Worksheets(Sheetselect)

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
        Public Function getGriegeInspectionReport_1(ByRef xlbook As ExcelWorkbook, ByVal headerlist As List(Of SPCInspection.RollInspectionSummaryHeaders), ByVal detailist As List(Of SPCInspection.RollInspectionDetailTable), ByVal fromdate As DateTime, ByVal todate As DateTime) As ExcelWorkbook
            Dim headerarray As Array = headerlist.ToArray()
            Dim detailarray As Array = detailist.ToArray()
            If Not xlbook Is Nothing Then
                Dim sheet1 As ExcelWorksheet = xlbook.Worksheets.Add("Griege Report")
                If headerarray.Length > 0 And detailarray.Length > 0 Then
                    Dim cnt As Int32
                    Try

                        sheet1.Cells(1, 1, 1, cnt + 3).Value = "Standard TexTile"
                        sheet1.Cells(2, 1, 2, cnt + 3).Value = "Greiege Inspections"
                        sheet1.Cells(3, 1, 3, cnt + 3).Value = "FROM: " & fromdate.ToString("d") & " TO: " & todate.ToString("d")
                        sheet1.Cells(1, 1, 3, cnt + 3).Style.Font.Bold = True
                        For Each header As SPCInspection.RollInspectionSummaryHeaders In headerarray

                            Dim col As Integer
                            If cnt = 0 Then
                                col = 1
                                cnt = 1
                            Else
                                col = cnt * ((cnt + (cnt - 1)) / cnt)
                            End If

                            sheet1.Cells(5, col + 1, 5, col + 2).Merge = True
                            sheet1.Cells(6, col + 1, 6, col + 2).Merge = True
                            sheet1.Cells(7, col + 1, 7, col + 2).Merge = True
                            sheet1.Cells(8, col + 1, 8, col + 2).Merge = True
                            sheet1.Cells(9, col + 1, 9, col + 2).Merge = True
                            sheet1.Cells(10, col + 1, 10, col + 2).Merge = True

                            sheet1.Cells(5, col + 1).Value = header.LoomNo
                            sheet1.Cells(6, col + 1).Value = header.RollNumber
                            sheet1.Cells(7, col + 1).Value = header.Style
                            sheet1.Cells(8, col + 1).Value = header.Yards_Inspected
                            sheet1.Cells(9, col + 1).Value = header.DefectYardsf
                            sheet1.Cells(10, col + 1).Value = header.DHY
                            If col = 1 Then
                                sheet1.Cells(1, 1, 1, headerarray.Length * 2 + 1).Merge = True
                                sheet1.Cells(2, 1, 2, headerarray.Length * 2 + 1).Merge = True
                                sheet1.Cells(3, 1, 3, headerarray.Length * 2 + 1).Merge = True
                                sheet1.Cells(5, 1).Value = "Loom #"
                                sheet1.Cells(6, 1).Value = "Roll #"
                                sheet1.Cells(7, 1).Value = "Style"
                                sheet1.Cells(8, 1).Value = "Yards Inspected"
                                sheet1.Cells(9, 1).Value = "Defective Yards"
                                sheet1.Cells(10, 1).Value = "Defects Per 100 Yds"
                                sheet1.Cells(5, 1, 5, col + 2).Style.Border.Top.Style = Style.ExcelBorderStyle.Thick
                                sheet1.Cells(10, 1, 10, col + 2).Style.Border.Bottom.Style = Style.ExcelBorderStyle.Thick
                                For i = 0 To 2
                                    Dim locarray As Integer() = {12, 30, 48}
                                    sheet1.Cells(locarray(i), 1).Value = "1st Shift"
                                    sheet1.Cells(locarray(i), 1).Style.Fill.PatternType = Style.ExcelFillStyle.Solid
                                    sheet1.Cells(locarray(i), 1).Style.Fill.BackgroundColor.SetColor(Color.Gray)
                                    sheet1.Cells(locarray(i), 1).Style.Font.Bold = True
                                    sheet1.Cells(locarray(i) + 1, 1).Value = "Yards Inspected"
                                    sheet1.Cells(locarray(i) + 1, 1).Style.Fill.PatternType = Style.ExcelFillStyle.Solid
                                    sheet1.Cells(locarray(i) + 1, 1).Style.Fill.BackgroundColor.SetColor(Color.Gray)
                                    sheet1.Cells(locarray(i) + 1, 1).Style.Font.Bold = True

                                    sheet1.Cells(locarray(i) + 2, 1, locarray(i) + 2, col + 3).Style.Border.Top.Style = Style.ExcelBorderStyle.Thick

                                    sheet1.Cells(locarray(i) + 16, 1).Value = "Total"


                                Next
                                sheet1.Cells("A5:A13").Style.HorizontalAlignment = Style.ExcelHorizontalAlignment.Left
                            End If

                            sheet1.Cells(5, col + 1).Style.Border.Bottom.Style = Style.ExcelBorderStyle.Thin
                            sheet1.Cells(5, col + 2).Style.Border.Bottom.Style = Style.ExcelBorderStyle.Thin
                            sheet1.Cells(6, col + 1).Style.Border.Bottom.Style = Style.ExcelBorderStyle.Thin
                            sheet1.Cells(6, col + 2).Style.Border.Bottom.Style = Style.ExcelBorderStyle.Thin
                            sheet1.Cells(7, col + 1).Style.Border.Bottom.Style = Style.ExcelBorderStyle.Thin
                            sheet1.Cells(7, col + 2).Style.Border.Bottom.Style = Style.ExcelBorderStyle.Thin
                            sheet1.Cells(8, col + 1).Style.Border.Bottom.Style = Style.ExcelBorderStyle.Thin
                            sheet1.Cells(8, col + 2).Style.Border.Bottom.Style = Style.ExcelBorderStyle.Thin
                            sheet1.Cells(9, col + 1).Style.Border.Bottom.Style = Style.ExcelBorderStyle.Thin
                            sheet1.Cells(9, col + 2).Style.Border.Bottom.Style = Style.ExcelBorderStyle.Thin

                            sheet1.Cells(5, col + 1).Style.Border.Top.Style = Style.ExcelBorderStyle.Thick
                            sheet1.Cells(5, col + 2).Style.Border.Top.Style = Style.ExcelBorderStyle.Thick
                            Using range = sheet1.Cells(5, col + 1, 10, col + 2)
                                range.Style.Border.Right.Style = Style.ExcelBorderStyle.Thick
                                range.Style.Border.Left.Style = Style.ExcelBorderStyle.Thick
                                range.Style.HorizontalAlignment = Style.ExcelHorizontalAlignment.Center
                                range.Style.Font.Bold = True
                            End Using

                            sheet1.Cells(10, col + 1).Style.Border.Bottom.Style = Style.ExcelBorderStyle.Thick
                            sheet1.Cells(10, col + 2).Style.Border.Bottom.Style = Style.ExcelBorderStyle.Thick


                            sheet1.Cells(14, col + 1, 27, col + 1).Style.Border.Left.Style = Style.ExcelBorderStyle.Thick
                            sheet1.Cells(14, col + 2, 27, col + 2).Style.Border.Right.Style = Style.ExcelBorderStyle.Thick
                            sheet1.Cells(27, col + 1, 27, col + 2).Style.Border.Bottom.Style = Style.ExcelBorderStyle.Thick
                            sheet1.Cells(14, col + 1, 14, col + 2).Style.Border.Top.Style = Style.ExcelBorderStyle.Thick

                            sheet1.Cells(32, col + 1, 45, col + 1).Style.Border.Left.Style = Style.ExcelBorderStyle.Thick
                            sheet1.Cells(32, col + 2, 45, col + 2).Style.Border.Right.Style = Style.ExcelBorderStyle.Thick
                            sheet1.Cells(45, col + 1, 45, col + 2).Style.Border.Bottom.Style = Style.ExcelBorderStyle.Thick
                            sheet1.Cells(32, col + 1, 32, col + 2).Style.Border.Top.Style = Style.ExcelBorderStyle.Thick

                            sheet1.Cells(50, col + 1, 63, col + 1).Style.Border.Left.Style = Style.ExcelBorderStyle.Thick
                            sheet1.Cells(50, col + 2, 63, col + 2).Style.Border.Right.Style = Style.ExcelBorderStyle.Thick
                            sheet1.Cells(63, col + 1, 63, col + 2).Style.Border.Bottom.Style = Style.ExcelBorderStyle.Thick
                            sheet1.Cells(50, col + 1, 50, col + 2).Style.Border.Top.Style = Style.ExcelBorderStyle.Thick


                            cnt += 1
                        Next
                    Catch ex As Exception

                    End Try

                    Try
                        Dim loomcount As Integer = 0
                        Dim RSIDPoint As Integer = 0
                        Dim shiftnumber As Integer = 0
                        cnt = 0
                        For Each detail As SPCInspection.RollInspectionDetailTable In detailarray
                            If detail.ShiftNumber = 1 Or detail.ShiftNumber = 2 Or detail.ShiftNumber = 3 Then

                                If detail.RSID <> RSIDPoint Then
                                    RSIDPoint = detail.RSID
                                    loomcount += 1
                                End If
                                If detail.ShiftNumber <> shiftnumber Then
                                    shiftnumber = detail.ShiftNumber
                                    cnt = 0
                                End If
                                Dim col As Integer = loomcount * ((loomcount + (loomcount - 1)) / loomcount)
                                Select Case detail.ShiftNumber
                                    Case 1
                                        sheet1.Cells(14 + cnt, col + 1).Value = detail.DefectCount
                                        sheet1.Cells(14 + cnt, col + 2).Value = detail.DHY
                                        sheet1.Cells(14 + cnt, col + 1).Style.Border.Bottom.Style = Style.ExcelBorderStyle.Thin
                                        sheet1.Cells(14 + cnt, col + 2).Style.Border.Bottom.Style = Style.ExcelBorderStyle.Thin
                                        sheet1.Cells(14 + cnt, col + 1).Style.Border.Right.Style = Style.ExcelBorderStyle.Thin
                                        If loomcount = 1 Then
                                            sheet1.Cells(14 + cnt, 1).Value = detail.Text

                                        End If
                                    Case 2
                                        sheet1.Cells(32 + cnt, col + 1).Value = detail.DefectCount
                                        sheet1.Cells(32 + cnt, col + 2).Value = detail.DHY
                                        sheet1.Cells(32 + cnt, col + 1).Style.Border.Bottom.Style = Style.ExcelBorderStyle.Thin
                                        sheet1.Cells(32 + cnt, col + 2).Style.Border.Bottom.Style = Style.ExcelBorderStyle.Thin
                                        sheet1.Cells(32 + cnt, col + 1).Style.Border.Right.Style = Style.ExcelBorderStyle.Thin
                                        If loomcount = 1 Then
                                            sheet1.Cells(32 + cnt, 1).Value = detail.Text

                                        End If
                                    Case 3
                                        sheet1.Cells(50 + cnt, col + 1).Value = detail.DefectCount
                                        sheet1.Cells(50 + cnt, col + 2).Value = detail.DHY
                                        sheet1.Cells(50 + cnt, col + 1).Style.Border.Bottom.Style = Style.ExcelBorderStyle.Thin
                                        sheet1.Cells(50 + cnt, col + 2).Style.Border.Bottom.Style = Style.ExcelBorderStyle.Thin
                                        sheet1.Cells(50 + cnt, col + 1).Style.Border.Right.Style = Style.ExcelBorderStyle.Thin
                                        If loomcount = 1 Then
                                            sheet1.Cells(50 + cnt, 1).Value = detail.Text

                                        End If
                                End Select

                                cnt += 1
                            End If
                        Next

                        sheet1.Column(1).AutoFit()
                        cnt = 0
                        For Each header As SPCInspection.RollInspectionSummaryHeaders In headerarray
                            Dim col As Integer
                            If cnt = 0 Then
                                col = 1
                                cnt = 1
                            Else
                                col = cnt * ((cnt + (cnt - 1)) / cnt)
                            End If
                            For i = 0 To 2
                                Dim locarray As Integer() = {12, 30, 48}
                                sheet1.Cells(locarray(i) + 16, col + 1).Formula = "=SUM(" & CollLetters(col) & (locarray(i) + 2).ToString() & ":" & CollLetters(col) & (locarray(i) + 15).ToString() & ")"
                                sheet1.Cells(locarray(i) + 16, col + 2).Formula = "=SUM(" & CollLetters(col + 1) & (locarray(i) + 2).ToString() & ":" & CollLetters(col + 1) & (locarray(i) + 15).ToString() & ")"
                                sheet1.Cells(locarray(i) + 16, col + 1, locarray(i) + 16, col + 2).Style.Font.Bold = True
                                sheet1.Cells(locarray(i), col + 1).Value = "# OF DEF."
                                sheet1.Cells(locarray(i), col + 2).Value = "DHY"
                                sheet1.Cells(locarray(i), col + 1, locarray(i) + 1, col + 2).Style.Fill.PatternType = Style.ExcelFillStyle.Solid
                                sheet1.Cells(locarray(i), col + 1, locarray(i) + 1, col + 2).Style.Fill.BackgroundColor.SetColor(Color.Gray)
                                sheet1.Cells(locarray(i) + 16, 1, locarray(i) + 16, col + 2).Style.Border.Bottom.Style = Style.ExcelBorderStyle.Thick
                                sheet1.Cells(locarray(i) + 15, 1, locarray(i) + 15, col + 2).Style.Border.Bottom.Style = Style.ExcelBorderStyle.Thick
                            Next
                            cnt += 1
                        Next

                    Catch ex As Exception

                    End Try

                End If

                Return xlbook
            Else
                Throw New System.Exception("Excel book cannot be null")
            End If

        End Function
        Public Function getGriegeInspectionReport(ByVal datefrom As String, ByVal dateto As String, ByRef epack As ExcelPackage) As ExcelWorkbook
            Dim headerlist As Array
            Dim detaillist As Array
            If IsDate(datefrom) = True And IsDate(dateto) = True Then

                Dim sheet1 As ExcelWorksheet = epack.Workbook.Worksheets.Add("Griege Report")


                Dim fromdate As DateTime = CType(datefrom, DateTime)
                Dim todate As DateTime = CType(dateto, DateTime)

                headerlist = IU.GetRollInspectionSummaryHeaders(fromdate, todate).ToArray()
                detaillist = IU.GetRollInspectionDetailTable(fromdate, todate).ToArray()

                If headerlist.Length > 0 And detaillist.Length > 0 Then
                    Dim cnt As Int32
                    Try

                        sheet1.Cells(1, 1, 1, cnt + 3).Value = "Standard TexTile"
                        sheet1.Cells(2, 1, 2, cnt + 3).Value = "Greiege Inspections"
                        sheet1.Cells(3, 1, 3, cnt + 3).Value = "FROM: " & fromdate.ToString("d") & " TO: " & todate.ToString("d")
                        sheet1.Cells(1, 1, 3, cnt + 3).Style.Font.Bold = True
                        For Each header As SPCInspection.RollInspectionSummaryHeaders In headerlist

                            Dim col As Integer
                            If cnt = 0 Then
                                col = 1
                                cnt = 1
                            Else
                                col = cnt * ((cnt + (cnt - 1)) / cnt)
                            End If

                            sheet1.Cells(5, col + 1, 5, col + 2).Merge = True
                            sheet1.Cells(6, col + 1, 6, col + 2).Merge = True
                            sheet1.Cells(7, col + 1, 7, col + 2).Merge = True
                            sheet1.Cells(8, col + 1, 8, col + 2).Merge = True
                            sheet1.Cells(9, col + 1, 9, col + 2).Merge = True
                            sheet1.Cells(10, col + 1, 10, col + 2).Merge = True

                            sheet1.Cells(5, col + 1).Value = header.LoomNo
                            sheet1.Cells(6, col + 1).Value = header.RollNumber
                            sheet1.Cells(7, col + 1).Value = header.Style
                            sheet1.Cells(8, col + 1).Value = header.Yards_Inspected
                            sheet1.Cells(9, col + 1).Value = header.DefectYardsf
                            sheet1.Cells(10, col + 1).Value = header.DHY
                            If col = 1 Then
                                sheet1.Cells(1, 1, 1, headerlist.Length * 2 + 1).Merge = True
                                sheet1.Cells(2, 1, 2, headerlist.Length * 2 + 1).Merge = True
                                sheet1.Cells(3, 1, 3, headerlist.Length * 2 + 1).Merge = True
                                sheet1.Cells(5, 1).Value = "Loom #"
                                sheet1.Cells(6, 1).Value = "Roll #"
                                sheet1.Cells(7, 1).Value = "Style"
                                sheet1.Cells(8, 1).Value = "Yards Inspected"
                                sheet1.Cells(9, 1).Value = "Defective Yards"
                                sheet1.Cells(10, 1).Value = "Defects Per 100 Yds"
                                sheet1.Cells(5, 1, 5, col + 2).Style.Border.Top.Style = Style.ExcelBorderStyle.Thick
                                sheet1.Cells(10, 1, 10, col + 2).Style.Border.Bottom.Style = Style.ExcelBorderStyle.Thick
                                For i = 0 To 2
                                    Dim locarray As Integer() = {12, 30, 48}
                                    sheet1.Cells(locarray(i), 1).Value = "1st Shift"
                                    sheet1.Cells(locarray(i), 1).Style.Fill.PatternType = Style.ExcelFillStyle.Solid
                                    sheet1.Cells(locarray(i), 1).Style.Fill.BackgroundColor.SetColor(Color.Gray)
                                    sheet1.Cells(locarray(i), 1).Style.Font.Bold = True
                                    sheet1.Cells(locarray(i) + 1, 1).Value = "Yards Inspected"
                                    sheet1.Cells(locarray(i) + 1, 1).Style.Fill.PatternType = Style.ExcelFillStyle.Solid
                                    sheet1.Cells(locarray(i) + 1, 1).Style.Fill.BackgroundColor.SetColor(Color.Gray)
                                    sheet1.Cells(locarray(i) + 1, 1).Style.Font.Bold = True

                                    sheet1.Cells(locarray(i) + 2, 1, locarray(i) + 2, col + 3).Style.Border.Top.Style = Style.ExcelBorderStyle.Thick
                                   
                                    sheet1.Cells(locarray(i) + 16, 1).Value = "Total"


                                Next
                                sheet1.Cells("A5:A13").Style.HorizontalAlignment = Style.ExcelHorizontalAlignment.Left
                            End If

                            sheet1.Cells(5, col + 1).Style.Border.Bottom.Style = Style.ExcelBorderStyle.Thin
                            sheet1.Cells(5, col + 2).Style.Border.Bottom.Style = Style.ExcelBorderStyle.Thin
                            sheet1.Cells(6, col + 1).Style.Border.Bottom.Style = Style.ExcelBorderStyle.Thin
                            sheet1.Cells(6, col + 2).Style.Border.Bottom.Style = Style.ExcelBorderStyle.Thin
                            sheet1.Cells(7, col + 1).Style.Border.Bottom.Style = Style.ExcelBorderStyle.Thin
                            sheet1.Cells(7, col + 2).Style.Border.Bottom.Style = Style.ExcelBorderStyle.Thin
                            sheet1.Cells(8, col + 1).Style.Border.Bottom.Style = Style.ExcelBorderStyle.Thin
                            sheet1.Cells(8, col + 2).Style.Border.Bottom.Style = Style.ExcelBorderStyle.Thin
                            sheet1.Cells(9, col + 1).Style.Border.Bottom.Style = Style.ExcelBorderStyle.Thin
                            sheet1.Cells(9, col + 2).Style.Border.Bottom.Style = Style.ExcelBorderStyle.Thin

                            sheet1.Cells(5, col + 1).Style.Border.Top.Style = Style.ExcelBorderStyle.Thick
                            sheet1.Cells(5, col + 2).Style.Border.Top.Style = Style.ExcelBorderStyle.Thick
                            Using range = sheet1.Cells(5, col + 1, 10, col + 2)
                                range.Style.Border.Right.Style = Style.ExcelBorderStyle.Thick
                                range.Style.Border.Left.Style = Style.ExcelBorderStyle.Thick
                                range.Style.HorizontalAlignment = Style.ExcelHorizontalAlignment.Center
                                range.Style.Font.Bold = True
                            End Using

                            sheet1.Cells(10, col + 1).Style.Border.Bottom.Style = Style.ExcelBorderStyle.Thick
                            sheet1.Cells(10, col + 2).Style.Border.Bottom.Style = Style.ExcelBorderStyle.Thick


                            sheet1.Cells(14, col + 1, 27, col + 1).Style.Border.Left.Style = Style.ExcelBorderStyle.Thick
                            sheet1.Cells(14, col + 2, 27, col + 2).Style.Border.Right.Style = Style.ExcelBorderStyle.Thick
                            sheet1.Cells(27, col + 1, 27, col + 2).Style.Border.Bottom.Style = Style.ExcelBorderStyle.Thick
                            sheet1.Cells(14, col + 1, 14, col + 2).Style.Border.Top.Style = Style.ExcelBorderStyle.Thick

                            sheet1.Cells(32, col + 1, 45, col + 1).Style.Border.Left.Style = Style.ExcelBorderStyle.Thick
                            sheet1.Cells(32, col + 2, 45, col + 2).Style.Border.Right.Style = Style.ExcelBorderStyle.Thick
                            sheet1.Cells(45, col + 1, 45, col + 2).Style.Border.Bottom.Style = Style.ExcelBorderStyle.Thick
                            sheet1.Cells(32, col + 1, 32, col + 2).Style.Border.Top.Style = Style.ExcelBorderStyle.Thick

                            sheet1.Cells(50, col + 1, 63, col + 1).Style.Border.Left.Style = Style.ExcelBorderStyle.Thick
                            sheet1.Cells(50, col + 2, 63, col + 2).Style.Border.Right.Style = Style.ExcelBorderStyle.Thick
                            sheet1.Cells(63, col + 1, 63, col + 2).Style.Border.Bottom.Style = Style.ExcelBorderStyle.Thick
                            sheet1.Cells(50, col + 1, 50, col + 2).Style.Border.Top.Style = Style.ExcelBorderStyle.Thick


                            cnt += 1
                        Next
                    Catch ex As Exception

                    End Try

                    Try
                        Dim loomcount As Integer = 0
                        Dim RSIDPoint As Integer = 0
                        Dim shiftnumber As Integer = 0
                        cnt = 0
                        For Each detail As SPCInspection.RollInspectionDetailTable In detaillist
                            If detail.ShiftNumber = 1 Or detail.ShiftNumber = 2 Or detail.ShiftNumber = 3 Then

                                If detail.RSID <> RSIDPoint Then
                                    RSIDPoint = detail.RSID
                                    loomcount += 1
                                End If
                                If detail.ShiftNumber <> shiftnumber Then
                                    shiftnumber = detail.ShiftNumber
                                    cnt = 0
                                End If
                                Dim col As Integer = loomcount * ((loomcount + (loomcount - 1)) / loomcount)
                                Select Case detail.ShiftNumber
                                    Case 1
                                        sheet1.Cells(14 + cnt, col + 1).Value = detail.DefectCount
                                        sheet1.Cells(14 + cnt, col + 2).Value = detail.DHY
                                        sheet1.Cells(14 + cnt, col + 1).Style.Border.Bottom.Style = Style.ExcelBorderStyle.Thin
                                        sheet1.Cells(14 + cnt, col + 2).Style.Border.Bottom.Style = Style.ExcelBorderStyle.Thin
                                        sheet1.Cells(14 + cnt, col + 1).Style.Border.Right.Style = Style.ExcelBorderStyle.Thin
                                        If loomcount = 1 Then
                                            sheet1.Cells(14 + cnt, 1).Value = detail.Text

                                        End If
                                    Case 2
                                        sheet1.Cells(32 + cnt, col + 1).Value = detail.DefectCount
                                        sheet1.Cells(32 + cnt, col + 2).Value = detail.DHY
                                        sheet1.Cells(32 + cnt, col + 1).Style.Border.Bottom.Style = Style.ExcelBorderStyle.Thin
                                        sheet1.Cells(32 + cnt, col + 2).Style.Border.Bottom.Style = Style.ExcelBorderStyle.Thin
                                        sheet1.Cells(32 + cnt, col + 1).Style.Border.Right.Style = Style.ExcelBorderStyle.Thin
                                        If loomcount = 1 Then
                                            sheet1.Cells(32 + cnt, 1).Value = detail.Text

                                        End If
                                    Case 3
                                        sheet1.Cells(50 + cnt, col + 1).Value = detail.DefectCount
                                        sheet1.Cells(50 + cnt, col + 2).Value = detail.DHY
                                        sheet1.Cells(50 + cnt, col + 1).Style.Border.Bottom.Style = Style.ExcelBorderStyle.Thin
                                        sheet1.Cells(50 + cnt, col + 2).Style.Border.Bottom.Style = Style.ExcelBorderStyle.Thin
                                        sheet1.Cells(50 + cnt, col + 1).Style.Border.Right.Style = Style.ExcelBorderStyle.Thin
                                        If loomcount = 1 Then
                                            sheet1.Cells(50 + cnt, 1).Value = detail.Text

                                        End If
                                End Select

                                cnt += 1
                            End If
                        Next

                        sheet1.Column(1).AutoFit()
                        cnt = 0
                        For Each header As SPCInspection.RollInspectionSummaryHeaders In headerlist
                            Dim col As Integer
                            If cnt = 0 Then
                                col = 1
                                cnt = 1
                            Else
                                col = cnt * ((cnt + (cnt - 1)) / cnt)
                            End If
                            For i = 0 To 2
                                Dim locarray As Integer() = {12, 30, 48}
                                sheet1.Cells(locarray(i) + 16, col + 1).Formula = "=SUM(" & CollLetters(col) & (locarray(i) + 2).ToString() & ":" & CollLetters(col) & (locarray(i) + 15).ToString() & ")"
                                sheet1.Cells(locarray(i) + 16, col + 2).Formula = "=SUM(" & CollLetters(col + 1) & (locarray(i) + 2).ToString() & ":" & CollLetters(col + 1) & (locarray(i) + 15).ToString() & ")"
                                sheet1.Cells(locarray(i) + 16, col + 1, locarray(i) + 16, col + 2).Style.Font.Bold = True
                                sheet1.Cells(locarray(i), col + 1).Value = "# OF DEF."
                                sheet1.Cells(locarray(i), col + 2).Value = "DHY"
                                sheet1.Cells(locarray(i), col + 1, locarray(i) + 1, col + 2).Style.Fill.PatternType = Style.ExcelFillStyle.Solid
                                sheet1.Cells(locarray(i), col + 1, locarray(i) + 1, col + 2).Style.Fill.BackgroundColor.SetColor(Color.Gray)
                                sheet1.Cells(locarray(i) + 16, 1, locarray(i) + 16, col + 2).Style.Border.Bottom.Style = Style.ExcelBorderStyle.Thick
                                sheet1.Cells(locarray(i) + 15, 1, locarray(i) + 15, col + 2).Style.Border.Bottom.Style = Style.ExcelBorderStyle.Thick
                            Next
                            cnt += 1
                        Next

                    Catch ex As Exception

                    End Try
                End If

            End If

            Return epack.Workbook
        End Function

        Public Function GetSpcReport(ByRef epack As ExcelPackage, ByVal todate As DateTime, ByVal fromdate As DateTime, ByVal Location As String) As ExcelWorkbook

            Dim weekday As DayOfWeek = todate.DayOfWeek
            Dim daysback As Integer = DateDiff(DateInterval.Day, todate, fromdate)
            Dim todatestring As String = todate.ToString("yyyy-MM-dd HH:mm")

            ' Using epack
            Dim retworkbook As ExcelWorkbook

            retworkbook = InjectJobSummary(epack, Location, daysback, todate, 1)
            retworkbook = injectsubstats_2(retworkbook, todatestring, 1)
            If Location = "CAR" Then
                Dim xlsheet As ExcelWorksheet
                If retworkbook.Worksheets.Count > 0 Then
                    xlsheet = retworkbook.Worksheets(1)
                    xlsheet.Column(4).Hidden = True
                    xlsheet.Column(5).Hidden = True
                    xlsheet.Column(6).Hidden = True
                End If
            End If

            Return retworkbook
            '  End Using

        End Function

        Public Function GetDefectTimerReport(ByVal workbook As ExcelWorkbook, listdtr As List(Of SPCInspection.TimerReport), ByVal Tabname As String) As ExcelWorkbook
            workbook.Worksheets.Add(Tabname)
            Try
                If listdtr.Count > 0 Then
                    Dim sheet1 As ExcelWorksheet = workbook.Worksheets(Tabname)
                    Dim newobj As New SPCInspection.TimerReport
                    Dim objtype As Type = newobj.GetType()
                    Dim objprop As System.Reflection.PropertyInfo() = objtype.GetProperties()
                    Dim colint As Integer = 1

                    For Each info As PropertyInfo In objprop
                        If info.Name.ToUpper() <> "CID" Then
                            sheet1.Cells(1, colint).Value = info.Name.ToUpper()
                            colint += 1
                        End If
                    Next

                    Dim rowint As Integer = 2

                    sheet1.Column(9).Style.Numberformat.Format = "yyyy-mm-dd hh:mm"
                    sheet1.Column(10).Style.Numberformat.Format = "yyyy-mm-dd hh:mm"

                    For Each item In listdtr
                        sheet1.Cells(rowint, 1).Value = item.JobType
                        sheet1.Cells(rowint, 2).Value = item.JobNumber
                        sheet1.Cells(rowint, 3).Value = item.Location
                        sheet1.Cells(rowint, 4).Value = item.DataNo
                        sheet1.Cells(rowint, 5).Value = item.UnitDesc
                        sheet1.Cells(rowint, 6).Value = item.DefectName
                        sheet1.Cells(rowint, 7).Value = item.DefectType
                        sheet1.Cells(rowint, 8).Value = item.EmployeeNo
                        sheet1.Cells(rowint, 9).Value = item.Timestamp
                        If item.StopTimestamp.Year = 1 Then
                            sheet1.Cells(rowint, 10).Value = ""
                        Else
                            sheet1.Cells(rowint, 10).Value = item.StopTimestamp
                        End If

                        sheet1.Cells(rowint, 11).Value = item.Timespan_min

                        rowint += 1
                    Next

                    sheet1.Cells(1, 1, rowint, colint).AutoFitColumns()
                End If
            Catch ex As Exception
                ErrorMessage = ex.Message
                Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
            End Try

            Return workbook
        End Function
        Public Function GetDataDump(ByRef workbook As ExcelWorkbook, ByVal listdd As List(Of SPCInspection.Dump), ByVal Tabname As String) As ExcelWorkbook
            workbook.Worksheets.Add(Tabname)
            Try
                If listdd.Count > 0 Then
                    Dim sheet1 As ExcelWorksheet = workbook.Worksheets(Tabname)
                    Dim newobj As New SPCInspection.Dump
                    Dim objtype As Type = newobj.GetType()
                    Dim objprop As System.Reflection.PropertyInfo() = objtype.GetProperties()
                    Dim colint As Integer = 1
                    Dim rowint As Integer = 2
                    Dim objarray = listdd.ToArray()

                    For Each info As PropertyInfo In objprop
                        If info.Name.ToUpper() = "USERCONFIRM_PASSFAIL" Or info.Name.ToUpper() = "USERCONFIRM_PASSFAIL_TIMESTAMP" Or info.Name.ToUpper() = "AQL" Or info.Name.ToUpper() = "DIMENSIONS" Then
                            GoTo 101
                        End If

                        sheet1.Cells(1, colint).Value = info.Name.ToUpper()
                        colint += 1
101:
                    Next
                    sheet1.Cells(1, 1, 1, colint).Style.Font.Bold = True
                    sheet1.Column(20).Style.Numberformat.Format = "yyyy-mm-dd hh:mm"
                    sheet1.Column(21).Style.Numberformat.Format = "yyyy-mm-dd hh:mm"
                    sheet1.Column(31).Style.Numberformat.Format = "yyyy-mm-dd hh:mm"
                    sheet1.Column(42).Style.Numberformat.Format = "yyyy-mm-dd hh:mm"
                    sheet1.Column(66).Style.Numberformat.Format = "yyyy-mm-dd hh:mm"
                    For Each item In objarray
                        Try
                            sheet1.Cells(rowint, 1).Value = item.id
                            sheet1.Cells(rowint, 2).Value = item.JobType
                            sheet1.Cells(rowint, 3).Value = item.JobNumber
                            sheet1.Cells(rowint, 4).Value = item.INSDataNum
                            sheet1.Cells(rowint, 5).Value = item.LOCID
                            sheet1.Cells(rowint, 6).Value = item.LOCName
                            sheet1.Cells(rowint, 7).Value = item.INStemplateID
                            sheet1.Cells(rowint, 8).Value = item.TMPName
                            sheet1.Cells(rowint, 9).Value = item.ItemPassCount
                            sheet1.Cells(rowint, 10).Value = item.ItemFailCount
                            sheet1.Cells(rowint, 11).Value = item.WOQuantity
                            sheet1.Cells(rowint, 12).Value = item.WorkOrderPieces
                            sheet1.Cells(rowint, 13).Value = item.AQL_Level
                            sheet1.Cells(rowint, 14).Value = item.Standard
                            sheet1.Cells(rowint, 15).Value = item.INSSampleSize
                            sheet1.Cells(rowint, 16).Value = item.TotalInspectedItems
                            sheet1.Cells(rowint, 17).Value = item.INSRejectLimiter
                            sheet1.Cells(rowint, 18).Value = item.Technical_PassFail
                            sheet1.Cells(rowint, 19).Value = item.Technical_PassFail_Timestamp
                            sheet1.Cells(rowint, 20).Value = item.Inspection_Started
                            sheet1.Cells(rowint, 21).Value = item.Inspection_Finished
                            sheet1.Cells(rowint, 22).Value = item.UnitCost
                            sheet1.Cells(rowint, 23).Value = item.UnitDesc
                            sheet1.Cells(rowint, 24).Value = item.Comments
                            sheet1.Cells(rowint, 25).Value = item.ProdMacineName
                            sheet1.Cells(rowint, 26).Value = item.MajorsCount
                            sheet1.Cells(rowint, 27).Value = item.MinorsCount
                            sheet1.Cells(rowint, 28).Value = item.RepairsCount
                            sheet1.Cells(rowint, 29).Value = item.ScrapCount
                            sheet1.Cells(rowint, 30).Value = item.DefectID
                            sheet1.Cells(rowint, 31).Value = item.DefectTime
                            sheet1.Cells(rowint, 32).Value = item.DefectDesc
                            sheet1.Cells(rowint, 33).Value = item.POnumber
                            sheet1.Cells(rowint, 34).Value = item.DataNo
                            sheet1.Cells(rowint, 35).Value = item.EmployeeNo
                            sheet1.Cells(rowint, 36).Value = item.ThisPieceNo
                            sheet1.Cells(rowint, 37).Value = item.SampleSize
                            sheet1.Cells(rowint, 38).Value = item.RejectLimiter
                            sheet1.Cells(rowint, 39).Value = item.TotalLotPieces
                            sheet1.Cells(rowint, 40).Value = item.Product
                            sheet1.Cells(rowint, 41).Value = item.DefectClass
                            sheet1.Cells(rowint, 42).Value = item.MergeDate
                            sheet1.Cells(rowint, 43).Value = item.Tablet
                            sheet1.Cells(rowint, 44).Value = item.WorkOrder
                            sheet1.Cells(rowint, 45).Value = item.LotNo
                            sheet1.Cells(rowint, 46).Value = item.Location
                            sheet1.Cells(rowint, 47).Value = item.Datatype
                            sheet1.Cells(rowint, 48).Value = item.Comment
                            sheet1.Cells(rowint, 49).Value = item.LoomNo
                            sheet1.Cells(rowint, 50).Value = item.DefectPoints
                            sheet1.Cells(rowint, 51).Value = item.GriegeNo
                            sheet1.Cells(rowint, 52).Value = item.RollNo
                            sheet1.Cells(rowint, 53).Value = item.Operation
                            sheet1.Cells(rowint, 54).Value = item.TemplateId
                            sheet1.Cells(rowint, 55).Value = item.InspectionId
                            sheet1.Cells(rowint, 56).Value = item.ButtonTemplateId
                            sheet1.Cells(rowint, 57).Value = item.Inspector
                            sheet1.Cells(rowint, 58).Value = item.ItemNumber
                            sheet1.Cells(rowint, 59).Value = item.InspectionState
                            sheet1.Cells(rowint, 60).Value = item.CasePackConv
                            sheet1.Cells(rowint, 61).Value = item.WorkRoom
                            sheet1.Cells(rowint, 62).Value = item.InspectionJobSummaryId
                            sheet1.Cells(rowint, 63).Value = item.TMPtempalteID
                            sheet1.Cells(rowint, 64).Value = item.Name
                            sheet1.Cells(rowint, 65).Value = item.Owner
                            sheet1.Cells(rowint, 66).Value = item.DateCreated
                            sheet1.Cells(rowint, 67).Value = item.Active
                            sheet1.Cells(rowint, 68).Value = item.LineType
                            sheet1.Cells(rowint, 69).Value = item.Ins_GriegeBatch
                            sheet1.Cells(rowint, 70).Value = item.Ins_WorkOrderInspection
                            sheet1.Cells(rowint, 71).Value = item.Loc_STT
                            sheet1.Cells(rowint, 72).Value = item.LOC_CAR
                            sheet1.Cells(rowint, 73).Value = item.LOC_STJ
                            sheet1.Cells(rowint, 74).Value = item.LOC_SPA
                            sheet1.Cells(rowint, 75).Value = item.LOC_CDC
                            sheet1.Cells(rowint, 76).Value = item.LOC_LINYI
                            sheet1.Cells(rowint, 77).Value = item.Loc_PCE
                            sheet1.Cells(rowint, 78).Value = item.Loc_FSK
                            sheet1.Cells(rowint, 79).Value = item.Loc_FNL
                            sheet1.Cells(rowint, 80).Value = item.Loc_FPC
                            sheet1.Cells(rowint, 81).Value = item.LOCID
                            sheet1.Cells(rowint, 82).Value = item.Abreviation
                            sheet1.Cells(rowint, 83).Value = item.DBname
                            sheet1.Cells(rowint, 84).Value = item.CID
                            sheet1.Cells(rowint, 85).Value = item.ConnectionString
                            sheet1.Cells(rowint, 86).Value = item.InspectionResults
                            sheet1.Cells(rowint, 87).Value = item.ProductionResults
                            sheet1.Cells(rowint, 88).Value = item.AS400_Connection
                            sheet1.Cells(rowint, 89).Value = item.AS400_Abr
                        Catch ex As Exception
                            ErrorMessage = ex.Message
                        End Try
                        
                        rowint += 1
                    Next

                    sheet1.Cells(1, 1, rowint, colint).AutoFitColumns()

                End If
            Catch ex As Exception
                ErrorMessage = ex.Message
            End Try

            Return workbook

        End Function
        Public Function GetDefectMaster(ByRef workbook As ExcelWorkbook, ByVal DefectMaster As List(Of SPCInspection.DefectMasterDisplay), ByVal Tabname As String) As ExcelWorkbook
            workbook.Worksheets.Add(Tabname)
            Try
                If DefectMaster.Count > 0 Then
                    Dim sheet1 As ExcelWorksheet = workbook.Worksheets(Tabname)
                    Dim newobj As New SPCInspection.DefectMasterDisplay
                    Dim objtype As Type = newobj.GetType()
                    Dim objprop As System.Reflection.PropertyInfo() = objtype.GetProperties()
                    Dim colint As Integer = 1
                    Dim rowint As Integer = 2
                    Dim objarray = DefectMaster.ToArray()

                    For Each info As PropertyInfo In objprop
                        If colint < 17 Then
                            sheet1.Cells(1, colint).Value = info.Name.ToUpper()
                            colint += 1
                        End If
                    Next
                    sheet1.Cells(1, 1, 1, colint).Style.Font.Bold = True

                    For Each item In objarray
                        sheet1.Cells(rowint, 1).Value = item.DefectID
                        sheet1.Cells(rowint, 2).Value = item.DefectTime
                        sheet1.Cells(rowint, 2).Style.Numberformat.Format = "yyyy-mm-dd hh:mm:ss"
                        sheet1.Cells(rowint, 3).Value = item.WorkOrder
                        sheet1.Cells(rowint, 4).Value = item.ItemNumber
                        sheet1.Cells(rowint, 5).Value = item.RollNo
                        sheet1.Cells(rowint, 6).Value = item.DataNo
                        sheet1.Cells(rowint, 7).Value = item.EmployeeNo
                        sheet1.Cells(rowint, 8).Value = item.AQL
                        sheet1.Cells(rowint, 9).Value = item.Inspector
                        sheet1.Cells(rowint, 10).Value = item.Name
                        sheet1.Cells(rowint, 11).Value = item.InspectionId
                        sheet1.Cells(rowint, 12).Value = item.DefectDesc
                        sheet1.Cells(rowint, 13).Value = item.TotalLotPieces
                        sheet1.Cells(rowint, 14).Value = item.Product
                        sheet1.Cells(rowint, 15).Value = item.LoomNo
                        sheet1.Cells(rowint, 16).Value = item.DataType

                        rowint += 1
                    Next

                    sheet1.Cells(1, 1, rowint, colint).AutoFitColumns()
                End If


            Catch ex As Exception

            End Try

            Return workbook

        End Function

        Public Function InjectJobSummary(ByRef epack As ExcelPackage, ByVal Location As String, ByVal daysback As Integer, ByVal todate As DateTime, ByVal sheetnum As Integer) As ExcelWorkbook
            Dim con As New SqlConnection(DL.InspectConnectionString)
            Dim cmd As SqlCommand = con.CreateCommand()
            Dim sheet1 As ExcelWorksheet
            Dim ID As New InspectionUtilityDAO
            Dim jslist As New List(Of SPCInspection.JobSummary)
            Dim jblist As New List(Of Production.JobSummary_DBreakdown)
            sheet1 = epack.Workbook.Worksheets(sheetnum)
            Try
                jslist = ID.GetJobSummary_1(Location, daysback, todate, 19)

            Catch ex As Exception

            End Try
            Try
                jblist = ID.GetJobSummary_2(Location, daysback, todate, 19)

            Catch ex As Exception

            End Try

            If jslist.Count > 0 And jblist.Count > 0 Then

                Dim jsarray = jslist.ToArray()
                Dim jbarray = jblist.ToArray()
                Dim jsobj As New SPCInspection.JobSummary
                Dim bustype As Type = jsobj.GetType()
                Dim jsprop As System.Reflection.PropertyInfo() = bustype.GetProperties()
                Dim colint As Integer = 1
                For Each info As PropertyInfo In jsprop
                    If colint <> 1 Then
                        Dim headername As String = info.Name.ToUpper()
                        If headername = "HOURLYYDS" Then
                            sheet1.Cells(1, colint - 1).Value = "WORKORDER YDS"
                        Else
                            sheet1.Cells(1, colint - 1).Value = info.Name.ToUpper()
                        End If

                    End If
                    colint += 1
                Next
                sheet1.Cells(1, 1, 1, colint).Style.Font.Bold = True
                Dim Irow As Integer = 2

                For Each js In jsarray

                    Dim jbfilt = (From x In jblist Where x.WorkOrderID = js.WorkOrderID And x.Machine = js.Machine).ToArray()
                    sheet1.Cells(Irow, 1).Value = js.Machine
                    sheet1.Cells(Irow, 2).Value = js.WorkOrderID
                    sheet1.Cells(Irow, 3).Value = js.HourBegin
                    sheet1.Cells(Irow, 3).Style.Numberformat.Format = "yyyy-mm-dd hh:mm:ss"
                    If jbfilt.Length > 0 Then
                        Dim Icol As Integer = 12

                        For Each jb In jbfilt
                            If Irow = 2 Then
                                sheet1.Cells(Irow - 1, Icol).Value = jb.DTYPE
                                sheet1.Cells(Irow - 1, Icol).Style.Font.Bold = True
                            End If
                            sheet1.Cells(Irow, Icol).Value = jb.DEFSUM

                            Icol += 1
                            lastcol = Icol
                        Next
                    End If
                    sheet1.Cells(Irow, 4).Value = js.ProductCount
                    sheet1.Cells(Irow, 5).Value = js.OverLengthInches
                    sheet1.Cells(Irow, 6).Value = js.CutLengthSpec
                    sheet1.Cells(Irow, 7).Value = js.AvgYdsPmin
                    sheet1.Cells(Irow, 8).Value = js.HourlyYds
                    sheet1.Cells(Irow, 9).Value = js.RunTime
                    sheet1.Cells(Irow, 10).Value = js.DownTime
                    sheet1.Cells(Irow, 11).Value = (js.DefPerProd / 100)
                    sheet1.Cells(Irow, 11).Style.Numberformat.Format = "#.###%"
                    Irow += 1
                    lastrow = Irow
                Next

            End If

            Return epack.Workbook
        End Function
    End Class


End Namespace