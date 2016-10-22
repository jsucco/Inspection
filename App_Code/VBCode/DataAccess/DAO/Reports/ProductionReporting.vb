Imports Microsoft.VisualBasic
Imports System.Net
Imports OfficeOpenXml
Imports OfficeOpenXml.Drawing.Chart
Imports OfficeOpenXml.Drawing
Imports OfficeOpenXml.Style
Imports System.Reflection

Namespace core
    Public Class ProductionReporting

        Public Function GetWorkOrderProduction(ByRef xlbook As ExcelWorkbook, ByVal wplist As List(Of Production.WorkOrderProductionSTT)) As ExcelWorkbook

            If Not xlbook Is Nothing Then

                Dim sheet1 As ExcelWorksheet = xlbook.Worksheets.Add("Work Order Production")

                If wplist.Count > 0 Then
                    Dim wparray = wplist.ToArray()
                    Dim newobj As New Production.WorkOrderProductionSTT
                    Dim bustype As Type = newobj.GetType()
                    Dim props As PropertyInfo() = bustype.GetProperties()
                    Dim i As Integer = 1
                    Dim col As Integer = 1

                    sheet1.Row(1).Height = 27.75
                    For Each info As PropertyInfo In props
                        sheet1.Cells(1, i).Value = info.Name.ToUpper()
                        sheet1.Cells(1, i).Style.Font.Bold = True
                        i += 1
                    Next
                    Dim row As Int16 = 2
                    For Each item In wparray
                        sheet1.Cells(row, 1).Value = item.ID
                        sheet1.Cells(row, 2).Value = item.Machine
                        sheet1.Cells(row, 3).Value = item.WorkOrder
                        sheet1.Cells(row, 4).Value = item.OperatorNo
                        sheet1.Cells(row, 5).Value = item.StartTime
                        sheet1.Cells(row, 6).Value = item.FinishTime
                        sheet1.Cells(row, 7).Value = item.DataNo
                        sheet1.Cells(row, 8).Value = item.GreigeNo
                        sheet1.Cells(row, 9).Value = item.CutLengthSpec
                        sheet1.Cells(row, 10).Value = item.JobYds
                        sheet1.Cells(row, 11).Value = item.JobSheets
                        sheet1.Cells(row, 12).Value = item.JobOverLengthInches
                        sheet1.Cells(row, 13).Value = item.ScheduledTime
                        sheet1.Cells(row, 14).Value = item.DownTime
                        sheet1.Cells(row, 15).Value = item.RunTime
                        sheet1.Cells(row, 16).Value = item.AvgSheetsPerHour
                        sheet1.Cells(row, 17).Value = item.JDECOMP
                        sheet1.Cells(row, 18).Value = item.JDESCRAP
                        sheet1.Cells(row, 19).Value = item.JDETOTREC
                        sheet1.Cells(row, 20).Value = item.DIFF_PERC
                        row += 1
                    Next
                    sheet1.Cells(2, 5, row, 6).Style.Numberformat.Format = "yyyy-mm-dd"
                    sheet1.Cells(2, 20, row, 20).Style.Numberformat.Format = "#0.00%"
                    sheet1.Cells(1, 1, row, 20).AutoFitColumns()
                End If
                Return xlbook
            Else
                Throw New System.Exception("xlbook cannot be null")
            End If

        End Function

        Public Function GetRollProduction(ByRef xlbook As ExcelWorkbook, ByVal wplist As List(Of Production.RollProductionSTT)) As ExcelWorkbook

            If Not xlbook Is Nothing Then

                Dim sheet1 As ExcelWorksheet = xlbook.Worksheets.Add("Roll Production")

                If wplist.Count > 0 Then
                    Dim wparray = wplist.ToArray()
                    Dim newobj As New Production.RollProductionSTT
                    Dim bustype As Type = newobj.GetType()
                    Dim props As PropertyInfo() = bustype.GetProperties()
                    Dim i As Integer = 1
                    Dim col As Integer = 1

                    sheet1.Row(1).Height = 27.75
                    For Each info As PropertyInfo In props
                        sheet1.Cells(1, i).Value = info.Name.ToUpper()
                        sheet1.Cells(1, i).Style.Font.Bold = True
                        i += 1
                    Next
                    Dim row As Int16 = 2
                    For Each item In wparray
                        sheet1.Cells(row, 1).Value = item.RollProductionID
                        sheet1.Cells(row, 2).Value = item.Machine
                        sheet1.Cells(row, 4).Value = item.OperatorNo
                        sheet1.Cells(row, 5).Value = item.StartTime
                        sheet1.Cells(row, 6).Value = item.EndTime
                        sheet1.Cells(row, 7).Value = item.TotalYds
                        sheet1.Cells(row, 8).Value = item.TotalSheets
                        sheet1.Cells(row, 9).Value = item.TicketYds
                        sheet1.Cells(row, 10).Value = item.TicketOverYds
                        sheet1.Cells(row, 11).Value = item.RollNo
                        sheet1.Cells(row, 12).Value = item.JobNo
                        sheet1.Cells(row, 13).Value = item.DataNo
                        sheet1.Cells(row, 14).Value = item.GreigeNo
                        sheet1.Cells(row, 15).Value = item.TimeStamp_Trans
                        row += 1
                    Next
                    sheet1.Cells(2, 5, row, 6).Style.Numberformat.Format = "yyyy-mm-dd"
                    sheet1.Cells(1, 1, row, 15).AutoFitColumns()
                End If
                Return xlbook
            Else
                Throw New System.Exception("xlbook cannot be null")
            End If

        End Function

        Public Function GetOperatorProduction(ByRef xlbook As ExcelWorkbook, ByVal wplist As List(Of Production.OperatorProduction)) As ExcelWorkbook

            If Not xlbook Is Nothing Then

                Dim sheet1 As ExcelWorksheet = xlbook.Worksheets.Add("Operator Production")

                If wplist.Count > 0 Then
                    Dim wparray = wplist.ToArray()
                    Dim newobj As New Production.OperatorProduction
                    Dim bustype As Type = newobj.GetType()
                    Dim props As PropertyInfo() = bustype.GetProperties()
                    Dim i As Integer = 1
                    Dim col As Integer = 1

                    sheet1.Row(1).Height = 27.75
                    For Each info As PropertyInfo In props
                        sheet1.Cells(1, i).Value = info.Name.ToUpper()
                        sheet1.Cells(1, i).Style.Font.Bold = True
                        i += 1
                    Next
                    Dim row As Int16 = 2
                    For Each item In wparray
                        sheet1.Cells(row, 1).Value = item.OperatorID
                        sheet1.Cells(row, 2).Value = item.Machine
                        sheet1.Cells(row, 3).Value = item.OperatorNo
                        sheet1.Cells(row, 4).Value = item.StartTime
                        sheet1.Cells(row, 5).Value = item.EndTime
                        sheet1.Cells(row, 6).Value = item.ScheduledTime
                        sheet1.Cells(row, 7).Value = item.RunTime
                        sheet1.Cells(row, 8).Value = item.DownTime
                        sheet1.Cells(row, 9).Value = item.TotalYds
                        sheet1.Cells(row, 10).Value = item.TotalSheets
                        sheet1.Cells(row, 11).Value = item.Efficiency
                        sheet1.Cells(row, 12).Value = item.AvgSheetsPerMin
                        sheet1.Cells(row, 13).Value = item.AvgYdsPerMin
                        sheet1.Cells(row, 14).Value = item.OverLengthInches
                        row += 1
                    Next
                    sheet1.Cells(2, 5, row, 6).Style.Numberformat.Format = "yyyy-mm-dd"
                    sheet1.Cells(1, 1, row, 12).AutoFitColumns()
                End If
                Return xlbook
            Else
                Throw New System.Exception("xlbook cannot be null")
            End If

        End Function
    End Class
End Namespace