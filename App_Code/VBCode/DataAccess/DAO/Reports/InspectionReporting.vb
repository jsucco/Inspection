Imports Microsoft.VisualBasic
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
Imports OfficeOpenXml.Style

Namespace core

    Public Class InspectionReporting
        Sub New(Optional ByVal SheetSelector As Integer = 0)
            Sheetselect = SheetSelector
        End Sub
        Public ErrorMessage As String
        Public pfromdate As String
        Public ptodate As String
        Public Sheetselect As Integer
        Public Yearcurrent As Integer = DateTime.Now.Year
        Public TodatePublic As String
        Dim lastrow As Integer = 0
        Dim util As New Utilities

        Public Function WorkOrderInspection(ByRef xlbook As ExcelWorkbook, ByVal wilist As List(Of SPCInspection.WorkOrderInspectionSummary), ByVal LocationArray As List(Of core.Locationarray)) As ExcelWorkbook

            Dim sheet1 As ExcelWorksheet

            If wilist.Count > 0 Then
                sheet1 = xlbook.Worksheets.Add("Daily AQL Report")

                Dim newobj As New SPCInspection.WorkOrderInspection
                Dim bustype As Type = newobj.GetType()
                Dim props As PropertyInfo() = bustype.GetProperties()
                Dim i As Integer = 1

                sheet1.Row(1).Height = 27.75
                For Each info As PropertyInfo In props
                    sheet1.Cells(1, i).Value = info.Name.ToUpper()
                    sheet1.Cells(1, i).Style.Font.Bold = True
                    i += 1
                Next
                sheet1.Column(1).Style.Numberformat.Format = "yyyy-mm-dd"
                sheet1.Column(8).Style.Numberformat.Format = "#0.00%"
                sheet1.Column(10).Style.Numberformat.Format = "#0.00%"
                Dim wiarray = wilist.ToArray()
                i = 1
                For Each item In wiarray
                    sheet1.Cells(i + 1, 1).Value = wiarray(i - 1).WorkDate
                    sheet1.Cells(i + 1, 2).Value = wiarray(i - 1).WorkOrder
                    sheet1.Cells(i + 1, 3).Value = wiarray(i - 1).Auditor
                    sheet1.Cells(i + 1, 4).Value = wiarray(i - 1).DataNo
                    sheet1.Cells(i + 1, 5).Value = wiarray(i - 1).WO_Pieces
                    sheet1.Cells(i + 1, 7).Value = wiarray(i - 1).AQL_Pieces
                    sheet1.Cells(i + 1, 8).Value = wiarray(i - 1).DefectRate
                    If wiarray(i - 1).Rejected >= wiarray(i - 1).RejectLimiter Then
                        sheet1.Cells(i + 1, 8).Style.Fill.PatternType = ExcelFillStyle.Solid
                        sheet1.Cells(i + 1, 8).Style.Fill.BackgroundColor.SetColor(Color.Red)
                        sheet1.Cells(i + 1, 8).Style.Font.Color.SetColor(Color.White)
                        sheet1.Cells(i + 1, 8).Style.Font.Bold = True
                    End If
                    sheet1.Cells(i + 1, 9).Value = wiarray(i - 1).Rejected
                    sheet1.Cells(i + 1, 10).Value = wiarray(i - 1).Rejected_Percent

                    i += 1
                Next

                Using range As ExcelRange = sheet1.Cells(1, 1, wiarray.Length + 1, 10)
                    range.Style.Border.Top.Style = ExcelBorderStyle.Thin
                    range.Style.Border.Bottom.Style = ExcelBorderStyle.Thin
                    range.Style.Border.Right.Style = ExcelBorderStyle.Thin
                    range.Style.Border.Left.Style = ExcelBorderStyle.Thin
                    range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center
                End Using
                sheet1.Column(1).Width = 12.5
                sheet1.Cells(2, 2, wiarray.Length + 1, 2).Style.HorizontalAlignment = ExcelHorizontalAlignment.Right
                sheet1.Column(2).Width = 13.29
                sheet1.Column(3).Width = 9.29
                sheet1.Column(4).Width = 10
                sheet1.Column(5).Width = 11
                sheet1.Column(6).Width = 10.57
                sheet1.Column(7).Width = 11.14
                sheet1.Column(8).Width = 13.57
                sheet1.Column(9).Width = 9.71
                sheet1.Column(10).Width = 18.6

            End If

            Return xlbook
        End Function

        Public Function WorkOrderCompiance(ByRef xlbook As ExcelWorkbook, ByVal fromdate As DateTime, ByVal todate As DateTime, ByVal LocationId As Integer) As ExcelWorkbook

            Dim WOCList1 As New List(Of SPCInspection.WorkOrderCompliance)
            Dim WOCList2 As New List(Of SPCInspection.WorkOrderCompliance)
            Dim bmapwoc As New BMappers(Of SPCInspection.WorkOrderCompliance)
            Dim sheet1 As ExcelWorksheet
            Dim sheet2 As ExcelWorksheet
            Dim sheet3 As ExcelWorksheet
            Dim sqlstring As String
            sheet1 = xlbook.Worksheets.Add("WorkOrder Compliance Report")
            sheet2 = xlbook.Worksheets.Add("WorkOrders Inspected")
            sheet3 = xlbook.Worksheets.Add("ALL WorkOrders")
            Try
                'Dim sqlstring As String = "select *, cast(A.ItemFailCount as decimal) / CAST(A.TotalItemsInspected as decimal) * 100 AS DHUAVG FROM(" & vbCrLf &
                '                "SELECT JobNumber as WorkOrder_Inspected, SUM(TotalInspectedItems) AS TotalItemsInspected, SUM(ItemFailCount) AS ItemFailCount FROM InspectionJobSummary" & vbCrLf &
                '                "WHERE (JobType = 'WorkOrder') AND (CID = '" & LocationId.ToString() & "') AND (TotalInspectedItems is not null) AND (TotalInspectedItems > 0) AND (Inspection_Started >= '" & fromdate.ToString("yyyy-MM-dd H:mm:ss") & "') AND (Inspection_Started <= '" & todate.ToString("yyyy-MM-dd H:mm:ss") & "') GROUP BY JobNumber) AS A"
                If LocationId = 999 Then
                    sqlstring = "select *, cast(A.ItemFailCount as decimal) / CAST(A.TotalItemsInspected as decimal) * 100 AS DHUAVG FROM(" & vbCrLf &
                                "SELECT JobNumber as WorkOrder_Inspected, MIN(Inspection_Started) as Inspection_Started, SUM(TotalInspectedItems) AS TotalItemsInspected, SUM(ItemFailCount) AS ItemFailCount FROM InspectionJobSummary" & vbCrLf &
                                "WHERE (JobType = 'WorkOrder') AND (TotalInspectedItems is not null) AND (TotalInspectedItems > 0) AND (Inspection_Started >= DATEADD(m, DATEDIFF(m, 0, '" & fromdate.ToString("yyyy-MM-dd H:mm:ss") & "'), 0)) AND (Inspection_Started <= DATEADD(s,-1,DATEADD(mm, DATEDIFF(m,0,'" & todate.ToString("yyyy-MM-dd H:mm:ss") & "')+1,0))) GROUP BY JobNumber) AS A"
                Else
                    sqlstring = "select *, cast(A.ItemFailCount as decimal) / CAST(A.TotalItemsInspected as decimal) * 100 AS DHUAVG FROM(" & vbCrLf &
                                "SELECT JobNumber as WorkOrder_Inspected, MIN(Inspection_Started) as Inspection_Started, SUM(TotalInspectedItems) AS TotalItemsInspected, SUM(ItemFailCount) AS ItemFailCount FROM InspectionJobSummary" & vbCrLf &
                                "WHERE (JobType = 'WorkOrder') AND (CID = '" & LocationId.ToString() & "') AND (TotalInspectedItems is not null) AND (TotalInspectedItems > 0) AND (Inspection_Started >= DATEADD(m, DATEDIFF(m, 0, '" & fromdate.ToString("yyyy-MM-dd H:mm:ss") & "'), 0)) AND (Inspection_Started <= DATEADD(s,-1,DATEADD(mm, DATEDIFF(m,0,'" & todate.ToString("yyyy-MM-dd H:mm:ss") & "')+1,0))) GROUP BY JobNumber) AS A"
                End If
                
                '=DATEADD(s,-1,DATEADD(mm, DATEDIFF(m,0,@TODATE)+1,0));
                'DATEADD(m, DATEDIFF(m, 0, @FROMDATE), 0)
                WOCList1 = bmapwoc.GetInspectObject(sqlstring)

                If WOCList1.Count > 0 Then
                    Dim IU As New InspectionUtilityDAO
                    Dim i As Integer = 1
                    Try
                        Dim bmapso As New BMappers(Of SingleObject)
                        Dim listso As New List(Of SingleObject)

                        listso = bmapso.GetAprMangObject("SELECT AS400_Connection as Object1 FROM LocationMaster WHERE CID = '000" & LocationId.ToString() & "'")

                        If listso.Count > 0 Or LocationId = 999 Then
                            If util.ConvertType(listso.ToArray()(0).Object1, "Boolean") = True Or LocationId = 999 Then
                                WOCList2 = IU.Getas400WOByBranch(fromdate, todate, LocationId)
                            End If
                        End If


                    Catch ex As Exception

                    End Try


                    If WOCList2.Count > 0 Then
                        Dim newobj As New SPCInspection.WorkOrderCompliance
                        Dim bustype As Type = newobj.GetType()
                        Dim props As PropertyInfo() = bustype.GetProperties()

                        sheet1.Row(1).Height = 27.75
                        For Each info As PropertyInfo In props
                            If info.Name.ToUpper() <> "WORKORDER_INSPECTED" And info.Name.ToUpper() <> "STARTED" Then
                                sheet1.Cells(1, i).Value = info.Name.ToUpper()
                                sheet1.Cells(1, i).Style.Font.Bold = True
                                If info.Name.ToUpper() <> "TOTALITEMSINSPECTED" And info.Name.ToUpper() <> "ITEMFAILCOUNT" And info.Name.ToUpper() <> "DHUAVG" And info.Name.ToUpper() <> "INSPECTION_STARTED" Then
                                    sheet3.Cells(1, i).Value = info.Name.ToUpper()
                                    sheet3.Cells(1, i).Style.Font.Bold = True
                                End If

                                i += 1
                            End If
                            
                        Next
                        sheet1.Cells(1, i).Value = "INSPECTEDFLAG"
                        sheet1.Cells(1, i).Style.Font.Bold = True
                        i = 2
                        sheet1.Column(3).Style.Numberformat.Format = "d/MM/yyyy"
                        sheet1.Column(8).Style.Numberformat.Format = "dd/MM/yyyy hh:mm"
                        sheet2.Column(6).Style.Numberformat.Format = "dd/MM/yyyy hh:mm"
                        sheet1.Column(6).Style.Numberformat.Format = "#0"
                        sheet1.Column(8).Style.Numberformat.Format = "d/MM/yyyy"
                        sheet1.Column(9).Style.Numberformat.Format = "#,##0"
                        sheet1.Column(10).Style.Numberformat.Format = "#,##0"
                        sheet1.Column(11).Style.Numberformat.Format = "#,##0.00%"
                        For Each item In WOCList2
                            sheet1.Cells(i, 1).Value = item.WorkOrder
                            sheet1.Cells(i, 2).Value = item.Description
                            sheet1.Cells(i, 3).Value = item.StartedDate.ToString("d")
                            sheet1.Cells(i, 4).Value = item.DataNo
                            sheet1.Cells(i, 5).Value = item.Branch
                            sheet1.Cells(i, 6).Value = item.Status
                            sheet1.Cells(i, 7).Value = item.Quantity

                            sheet3.Cells(i, 1).Value = item.WorkOrder
                            sheet3.Cells(i, 2).Value = item.Description
                            sheet3.Cells(i, 3).Value = item.StartedDate.ToString("d")
                            sheet3.Cells(i, 4).Value = item.DataNo
                            sheet3.Cells(i, 5).Value = item.Branch
                            sheet3.Cells(i, 6).Value = item.Status
                            sheet3.Cells(i, 7).Value = item.Quantity

                            Dim APRROW = (From X In WOCList1 Where X.WorkOrder_Inspected = item.WorkOrder Select X).ToArray()

                            If APRROW.Length > 0 Then
                                sheet1.Cells(i, 8).Value = APRROW(0).Inspection_Started.ToString("dd/MM/yyyy hh:mm")
                                sheet1.Cells(i, 9).Value = APRROW(0).TotalItemsInspected
                                sheet1.Cells(i, 10).Value = APRROW(0).ItemFailCount
                                sheet1.Cells(i, 11).Value = APRROW(0).DHUAVG / 100
                                sheet1.Cells(i, 12).Value = "TRUE"
                                sheet1.Row(i).Style.Fill.PatternType = ExcelFillStyle.Solid
                                sheet1.Row(i).Style.Fill.BackgroundColor.SetColor(Color.Green)
                                sheet1.Row(i).Style.Font.Color.SetColor(Color.White)
                                sheet1.Row(i).Style.Font.Bold = True
                                sheet1.Cells(i, 6).Style.Numberformat.Format = "#,##0"
                                sheet1.Cells(i, 7).Style.Numberformat.Format = "#,##0"
                                sheet1.Cells(i, 9).Style.Numberformat.Format = "#,##0"
                                sheet1.Cells(i, 10).Style.Numberformat.Format = "#,##0"
                                sheet1.Cells(i, 11).Style.Numberformat.Format = "#,##0.00%"
                            Else
                                sheet1.Cells(i, 8).Value = 0
                                sheet1.Cells(i, 9).Value = 0
                                sheet1.Cells(i, 10).Value = 0
                                sheet1.Cells(i, 11).Value = 0
                                sheet1.Cells(i, 12).Value = "FALSE"
                            End If

                            i += 1
                        Next

                        sheet1.Cells(1, 1, i, 12).AutoFitColumns()
                        sheet3.Cells(1, 1, i, 9).AutoFitColumns()

                    End If
                    sheet2.Cells(1, 1).Value = "WORKORDER"
                    sheet2.Cells(1, 2).Value = "INSPECTIONSTARTED"
                    sheet2.Cells(1, 3).Value = "TOTALITEMSINSPECTED"
                    sheet2.Cells(1, 4).Value = "ITEMFAILCOUNT"
                    sheet2.Cells(1, 5).Value = "DHUAVG"
                    sheet2.Cells(1, 1, 1, 5).Style.Font.Bold = True
                    i = 2
                    For Each item In WOCList1
                        sheet2.Cells(i, 1).Value = item.WorkOrder_Inspected
                        sheet2.Cells(i, 2).Value = item.Inspection_Started.ToString("U")
                        sheet2.Cells(i, 3).Value = item.TotalItemsInspected
                        sheet2.Cells(i, 4).Value = item.ItemFailCount
                        sheet2.Cells(i, 5).Value = item.DHUAVG / 100
                        sheet2.Cells(i, 5).Style.Numberformat.Format = "#,##0.00%"
                        i += 1
                    Next

                    sheet2.Cells(1, 1, i, 5).AutoFitColumns()
                End If

            Catch ex As Exception
                util.newlogobj.date_added = Date.Now
                util.newlogobj.application_name = "APR-WorkOrder Compliance Report"
                util.newlogobj.Target = "USER"
                util.newlogobj.Message = ex.Message
                util.Log()
            End Try

            Return xlbook

        End Function

        Public Function WorkOrderCompiance2(ByRef xlbook As ExcelWorkbook, ByVal WOCList1 As List(Of SPCInspection.WorkOrderCompliance), ByVal WOCList2 As List(Of SPCInspection.WorkOrderCompliance), ByVal LocationId As Integer) As ExcelWorkbook

            Dim bmapwoc As New BMappers(Of SPCInspection.WorkOrderCompliance)
            Dim sheet1 As ExcelWorksheet
            Dim sheet2 As ExcelWorksheet
            Dim sheet3 As ExcelWorksheet
            Dim sqlstring As String
            sheet1 = xlbook.Worksheets.Add("WorkOrder Compliance Report")
            sheet2 = xlbook.Worksheets.Add("WorkOrders Inspected")
            sheet3 = xlbook.Worksheets.Add("ALL WorkOrders")
            Try

                'If WOCList1.Count > 0 Then
                Dim IU As New InspectionUtilityDAO
                Dim i As Integer = 1
                Try
                    'Dim bmapso As New BMappers(Of SingleObject)
                    'Dim listso As New List(Of SingleObject)

                    'listso = bmapso.GetAprMangObject("SELECT AS400_Connection as Object1 FROM LocationMaster WHERE CID = '000" & LocationId.ToString() & "'")

                    'If listso.Count > 0 Or LocationId = 999 Then
                    '    If util.ConvertType(listso.ToArray()(0).Object1, "Boolean") = True Or LocationId = 999 Then
                    '        WOCList2 = IU.Getas400WOByBranch(fromdate, todate, LocationId)
                    '    End If
                    'End If


                Catch ex As Exception

                End Try


                If WOCList2.Count > 0 Then
                    Dim newobj As New SPCInspection.WorkOrderCompliance
                    Dim bustype As Type = newobj.GetType()
                    Dim props As PropertyInfo() = bustype.GetProperties()

                    sheet1.Row(1).Height = 27.75
                    For Each info As PropertyInfo In props
                        If info.Name.ToUpper() <> "WORKORDER_INSPECTED" And info.Name.ToUpper() <> "STARTED" Then
                            sheet1.Cells(1, i).Value = info.Name.ToUpper()
                            sheet1.Cells(1, i).Style.Font.Bold = True
                            If info.Name.ToUpper() <> "TOTALITEMSINSPECTED" And info.Name.ToUpper() <> "ITEMFAILCOUNT" And info.Name.ToUpper() <> "DHUAVG" And info.Name.ToUpper() <> "INSPECTION_STARTED" Then
                                sheet3.Cells(1, i).Value = info.Name.ToUpper()
                                sheet3.Cells(1, i).Style.Font.Bold = True
                            End If

                            i += 1
                        End If

                    Next
                    sheet1.Cells(1, i).Value = "INSPECTEDFLAG"
                    sheet1.Cells(1, i).Style.Font.Bold = True
                    i = 2
                    sheet1.Column(3).Style.Numberformat.Format = "d/MM/yyyy"
                    sheet1.Column(8).Style.Numberformat.Format = "dd/MM/yyyy hh:mm"
                    sheet2.Column(6).Style.Numberformat.Format = "dd/MM/yyyy hh:mm"
                    sheet1.Column(6).Style.Numberformat.Format = "#0"
                    sheet1.Column(8).Style.Numberformat.Format = "d/MM/yyyy"
                    sheet1.Column(9).Style.Numberformat.Format = "#,##0"
                    sheet1.Column(10).Style.Numberformat.Format = "#,##0"
                    sheet1.Column(11).Style.Numberformat.Format = "#,##0.00%"
                    For Each item In WOCList2
                        sheet1.Cells(i, 1).Value = item.WorkOrder
                        sheet1.Cells(i, 2).Value = item.Description
                        sheet1.Cells(i, 3).Value = item.StartedDate.ToString("d")
                        sheet1.Cells(i, 4).Value = item.DataNo
                        sheet1.Cells(i, 5).Value = item.Branch
                        sheet1.Cells(i, 6).Value = item.Status
                        sheet1.Cells(i, 7).Value = item.Quantity

                        sheet3.Cells(i, 1).Value = item.WorkOrder
                        sheet3.Cells(i, 2).Value = item.Description
                        sheet3.Cells(i, 3).Value = item.StartedDate.ToString("d")
                        sheet3.Cells(i, 4).Value = item.DataNo
                        sheet3.Cells(i, 5).Value = item.Branch
                        sheet3.Cells(i, 6).Value = item.Status
                        sheet3.Cells(i, 7).Value = item.Quantity

                        Dim APRROW = (From X In WOCList1 Where X.WorkOrder_Inspected = item.WorkOrder Select X).ToArray()

                        If APRROW.Length > 0 Then
                            sheet1.Cells(i, 8).Value = APRROW(0).Inspection_Started.ToString("dd/MM/yyyy hh:mm")
                            sheet1.Cells(i, 9).Value = APRROW(0).TotalItemsInspected
                            sheet1.Cells(i, 10).Value = APRROW(0).ItemFailCount
                            sheet1.Cells(i, 11).Value = APRROW(0).DHUAVG / 100
                            sheet1.Cells(i, 12).Value = "TRUE"
                            sheet1.Row(i).Style.Fill.PatternType = ExcelFillStyle.Solid
                            sheet1.Row(i).Style.Fill.BackgroundColor.SetColor(Color.Green)
                            sheet1.Row(i).Style.Font.Color.SetColor(Color.White)
                            sheet1.Row(i).Style.Font.Bold = True
                            sheet1.Cells(i, 6).Style.Numberformat.Format = "#,##0"
                            sheet1.Cells(i, 7).Style.Numberformat.Format = "#,##0"
                            sheet1.Cells(i, 9).Style.Numberformat.Format = "#,##0"
                            sheet1.Cells(i, 10).Style.Numberformat.Format = "#,##0"
                            sheet1.Cells(i, 11).Style.Numberformat.Format = "#,##0.00%"
                        Else
                            sheet1.Cells(i, 8).Value = 0
                            sheet1.Cells(i, 9).Value = 0
                            sheet1.Cells(i, 10).Value = 0
                            sheet1.Cells(i, 11).Value = 0
                            sheet1.Cells(i, 12).Value = "FALSE"
                        End If

                        i += 1
                    Next

                    sheet1.Cells(1, 1, i, 12).AutoFitColumns()
                    sheet3.Cells(1, 1, i, 9).AutoFitColumns()

                End If
                sheet2.Cells(1, 1).Value = "WORKORDER"
                sheet2.Cells(1, 2).Value = "INSPECTIONSTARTED"
                sheet2.Cells(1, 3).Value = "TOTALITEMSINSPECTED"
                sheet2.Cells(1, 4).Value = "ITEMFAILCOUNT"
                sheet2.Cells(1, 5).Value = "DHUAVG"
                sheet2.Cells(1, 1, 1, 5).Style.Font.Bold = True
                i = 2
                For Each item In WOCList1
                    sheet2.Cells(i, 1).Value = item.WorkOrder_Inspected
                    sheet2.Cells(i, 2).Value = item.Inspection_Started.ToString("U")
                    sheet2.Cells(i, 3).Value = item.TotalItemsInspected
                    sheet2.Cells(i, 4).Value = item.ItemFailCount
                    sheet2.Cells(i, 5).Value = item.DHUAVG / 100
                    sheet2.Cells(i, 5).Style.Numberformat.Format = "#,##0.00%"
                    i += 1
                Next

                sheet2.Cells(1, 1, i, 5).AutoFitColumns()
                'End If

            Catch ex As Exception
                util.newlogobj.date_added = Date.Now
                util.newlogobj.application_name = "APR-WorkOrder Compliance Report"
                util.newlogobj.Target = "USER"
                util.newlogobj.Message = ex.Message
                util.Log()
            End Try

            Return xlbook

        End Function

        Public Function InsertInspectionJobSummary(ByRef xlbook As ExcelWorkbook, ByVal wilist As List(Of SPCInspection.InspectionJobSumarryReport), ByVal LocationArray As List(Of core.Locationarray)) As ExcelWorkbook

            Dim sheet1 As ExcelWorksheet

            If wilist.Count > 0 Then
                sheet1 = xlbook.Worksheets.Add("AQL Inspection Report")

                Dim newobj As New SPCInspection.InspectionJobSumarryReport
                Dim bustype As Type = newobj.GetType()
                Dim props As PropertyInfo() = bustype.GetProperties()
                Dim i As Integer = 1

                sheet1.Row(1).Height = 27.75
                For Each info As PropertyInfo In props
                    sheet1.Cells(1, i).Value = info.Name.ToUpper()
                    sheet1.Cells(1, i).Style.Font.Bold = True
                    i += 1
                Next
                sheet1.Column(19).Style.Numberformat.Format = "yyyy-mm-dd hh:mm"
                sheet1.Column(20).Style.Numberformat.Format = "yyyy-mm-dd hh:mm"
                sheet1.Column(17).Style.Numberformat.Format = "#0.000"

                Dim wiarray = wilist.ToArray()
                i = 1
                For Each item In wiarray
                    sheet1.Cells(i + 1, 1).Value = wiarray(i - 1).id
                    sheet1.Cells(i + 1, 2).Value = wiarray(i - 1).JobType
                    sheet1.Cells(i + 1, 3).Value = wiarray(i - 1).JobNumber
                    sheet1.Cells(i + 1, 4).Value = wiarray(i - 1).Name
                    sheet1.Cells(i + 1, 5).Value = wiarray(i - 1).LocationName
                    sheet1.Cells(i + 1, 6).Value = wiarray(i - 1).UnitDesc
                    sheet1.Cells(i + 1, 7).Value = wiarray(i - 1).LineType
                    sheet1.Cells(i + 1, 8).Value = wiarray(i - 1).TotalInspectedItems
                    If IsDBNull(wiarray(i - 1).ItemPassCount) = True Then
                        sheet1.Cells(i + 1, 9).Value = 0
                    Else
                        sheet1.Cells(i + 1, 9).Value = wiarray(i - 1).ItemPassCount
                    End If

                    sheet1.Cells(i + 1, 10).Value = wiarray(i - 1).ItemFailCount
                    sheet1.Cells(i + 1, 11).Value = wiarray(i - 1).WOQuantity
                    sheet1.Cells(i + 1, 12).Value = wiarray(i - 1).WorkOrderPieces
                    sheet1.Cells(i + 1, 13).Value = wiarray(i - 1).AQL_Level
                    sheet1.Cells(i + 1, 14).Value = wiarray(i - 1).SampleSize
                    sheet1.Cells(i + 1, 15).Value = wiarray(i - 1).RejectLimiter
                    sheet1.Cells(i + 1, 16).Value = wiarray(i - 1).UnitCost
                    'sheet1.Cells(i + 1, 15).Value = wiarray(i - 1).
                    If wiarray(i - 1).TotalInspectedItems > 0 Then
                        sheet1.Cells(i + 1, 17).Value = (wiarray(i - 1).ItemFailCount * 100) / wiarray(i - 1).TotalInspectedItems
                    Else
                        sheet1.Cells(i + 1, 17).Value = 0
                    End If
                    If IsDBNull(wiarray(i - 1).Technical_PassFail) = True Then
                        sheet1.Cells(i + 1, 18).Value = "OPEN"
                    Else
                        sheet1.Cells(i + 1, 18).Value = wiarray(i - 1).Technical_PassFail
                    End If
                    If wiarray(i - 1).Technical_PassFail = False Then
                        sheet1.Cells(i + 1, 18).Style.Fill.PatternType = ExcelFillStyle.Solid
                        sheet1.Cells(i + 1, 18).Style.Fill.BackgroundColor.SetColor(Color.Red)
                        sheet1.Cells(i + 1, 18).Style.Font.Color.SetColor(Color.White)
                        sheet1.Cells(i + 1, 18).Style.Font.Bold = True
                    ElseIf wiarray(i - 1).Technical_PassFail = True Then
                        sheet1.Cells(i + 1, 18).Style.Fill.PatternType = ExcelFillStyle.Solid
                        sheet1.Cells(i + 1, 18).Style.Fill.BackgroundColor.SetColor(Color.Green)
                        sheet1.Cells(i + 1, 18).Style.Font.Color.SetColor(Color.White)
                        sheet1.Cells(i + 1, 18).Style.Font.Bold = True
                    End If
                    sheet1.Cells(i + 1, 19).Value = wiarray(i - 1).Inspection_Started
                    If IsDBNull(wiarray(i - 1).Inspection_Finished) = True Then
                        sheet1.Cells(i + 1, 20).Value = ""
                    Else
                        sheet1.Cells(i + 1, 20).Value = wiarray(i - 1).Inspection_Finished
                    End If

                    sheet1.Cells(i + 1, 21).Value = wiarray(i - 1).Comments

                    i += 1
                Next

                sheet1.Cells(1, 1, i, 21).AutoFitColumns()

                Using range As ExcelRange = sheet1.Cells(1, 1, wiarray.Length + 1, 21)
                    range.Style.Border.Top.Style = ExcelBorderStyle.Thin
                    range.Style.Border.Bottom.Style = ExcelBorderStyle.Thin
                    range.Style.Border.Right.Style = ExcelBorderStyle.Thin
                    range.Style.Border.Left.Style = ExcelBorderStyle.Thin
                    range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center
                End Using


            End If

            Return xlbook
        End Function

        Public Function InsertInspectionJobSummary2(ByRef xlbook As ExcelWorkbook, ByVal wilist As List(Of SPCInspection.InspectionSummaryDisplay), ByVal LocationArray As List(Of core.Locationarray)) As ExcelWorkbook

            Dim sheet1 As ExcelWorksheet

            If wilist.Count > 0 Then
                sheet1 = xlbook.Worksheets.Add("AQL Inspection Report")

                Dim newobj As New SPCInspection.InspectionSummaryDisplay
                Dim bustype As Type = newobj.GetType()
                Dim props As PropertyInfo() = bustype.GetProperties()
                Dim i As Integer = 1
                Dim j As Integer = 2
                Dim wiarray = wilist.ToArray()
                sheet1.Row(1).Height = 27.75

                If IsNothing(wiarray) = False Then

                    For Each info As PropertyInfo In props
                        sheet1.Cells(1, i).Value = info.Name.ToUpper()
                        sheet1.Cells(1, i).Style.Font.Bold = True
                        j = 2
                        For Each Item In wiarray
                            Try
                                Dim rowval As Object = info.GetValue(Item, Nothing)
                                If info.Name = "STARTED" Or info.Name = "FINISHED" Then
                                    sheet1.Cells(j, i).Value = DateTime.Parse(rowval)
                                Else
                                    sheet1.Cells(j, i).Value = rowval
                                End If

                            Catch ex As Exception

                            End Try
                            j += 1
                        Next
                        i += 1
                    Next
                    'sheet1.Column(19).Style.Numberformat.Format = "yyyy-mm-dd hh:mm"
                    'sheet1.Column(20).Style.Numberformat.Format = "yyyy-mm-dd hh:mm"
                    'sheet1.Column(17).Style.Numberformat.Format = "#0.000"

                End If

                'i = 1
                'For Each item In wiarray
                '    sheet1.Cells(i + 1, 1).Value = wiarray(i - 1).id
                '    sheet1.Cells(i + 1, 2).Value = wiarray(i - 1).JobType
                '    sheet1.Cells(i + 1, 3).Value = wiarray(i - 1).JobNumber
                '    sheet1.Cells(i + 1, 4).Value = wiarray(i - 1).Name
                '    sheet1.Cells(i + 1, 5).Value = wiarray(i - 1).Location
                '    sheet1.Cells(i + 1, 6).Value = wiarray(i - 1).UnitDesc
                '    sheet1.Cells(i + 1, 7).Value = wiarray(i - 1).LineType

                '    'sheet1.Cells(i + 1, 15).Value = wiarray(i - 1).RejectLimiter
                '    sheet1.Cells(i + 1, 16).Value = wiarray(i - 1).UnitCost
                '    'sheet1.Cells(i + 1, 15).Value = wiarray(i - 1).

                '    sheet1.Cells(i + 1, 17).Value = wiarray(i - 1).RejectionRate

                '    If IsDBNull(wiarray(i - 1).Technical_PassFail) = True Then
                '        sheet1.Cells(i + 1, 18).Value = "OPEN"
                '    Else
                '        sheet1.Cells(i + 1, 18).Value = wiarray(i - 1).Technical_PassFail
                '    End If
                '    If wiarray(i - 1).Technical_PassFail = False Then
                '        sheet1.Cells(i + 1, 18).Style.Fill.PatternType = ExcelFillStyle.Solid
                '        sheet1.Cells(i + 1, 18).Style.Fill.BackgroundColor.SetColor(Color.Red)
                '        sheet1.Cells(i + 1, 18).Style.Font.Color.SetColor(Color.White)
                '        sheet1.Cells(i + 1, 18).Style.Font.Bold = True
                '    ElseIf wiarray(i - 1).Technical_PassFail = True Then
                '        sheet1.Cells(i + 1, 18).Style.Fill.PatternType = ExcelFillStyle.Solid
                '        sheet1.Cells(i + 1, 18).Style.Fill.BackgroundColor.SetColor(Color.Green)
                '        sheet1.Cells(i + 1, 18).Style.Font.Color.SetColor(Color.White)
                '        sheet1.Cells(i + 1, 18).Style.Font.Bold = True
                '    End If
                '    sheet1.Cells(i + 1, 19).Value = wiarray(i - 1).STARTED
                '    If IsDBNull(wiarray(i - 1).FINISHED) = True Then
                '        sheet1.Cells(i + 1, 20).Value = ""
                '    Else
                '        sheet1.Cells(i + 1, 20).Value = wiarray(i - 1).FINISHED
                '    End If

                '    i += 1
                'Next

                sheet1.Cells(1, 1, i, 21).AutoFitColumns()

                Using range As ExcelRange = sheet1.Cells(1, 1, wiarray.Length + 1, 21)
                    range.Style.Border.Top.Style = ExcelBorderStyle.Thin
                    range.Style.Border.Bottom.Style = ExcelBorderStyle.Thin
                    range.Style.Border.Right.Style = ExcelBorderStyle.Thin
                    range.Style.Border.Left.Style = ExcelBorderStyle.Thin
                    range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center
                End Using


            End If

            Return xlbook
        End Function

        Public Function InsertCompliance(ByRef xlbook As ExcelWorkbook, ByVal cplist As List(Of SPCInspection.InspectionCompliance_Local)) As ExcelWorkbook
            Dim sheet1 As ExcelWorksheet
            sheet1 = xlbook.Worksheets.Add("Compliance Report")
            If cplist.Count > 0 Then


                Dim newobj As New SPCInspection.InspectionCompliance_Local
                Dim bustype As Type = newobj.GetType()
                Dim props As PropertyInfo() = bustype.GetProperties()
                Dim i As Integer = 1

                'sheet1.Row(1).Height = 27.75
                'For Each info As PropertyInfo In props
                '    If info.Name.ToUpper() <> "DESCRIPTION" And info.Name.ToUpper() <> "UPDATED" And info.Name.ToUpper() <> "STARTED" And info.Name.ToUpper() <> "STARTEDDATE" Then
                '        sheet1.Cells(1, i).Value = info.Name.ToUpper()
                '        sheet1.Cells(1, i).Style.Font.Bold = True
                '        i += 1
                '    End If
                'Next
                sheet1.Cells(1, 1).Value = "id"
                sheet1.Cells(1, 2).Value = "WORKORDER"
                sheet1.Cells(1, 3).Value = "DATANO"
                sheet1.Cells(1, 4).Value = "WADL01"
                sheet1.Cells(1, 5).Value = "LOCATION"
                sheet1.Cells(1, 7).Value = "WADCTO"
                sheet1.Cells(1, 8).Value = "WASRST"
                sheet1.Cells(1, 9).Value = "ijsid"
                sheet1.Cells(1, 10).Value = "LineType"

                Dim cparray = cplist.ToArray()

                i = 1
                For Each item In cparray

                    sheet1.Cells(i + 1, 1).Value = item.Id
                    sheet1.Cells(i + 1, 2).Value = item.WorkOrder
                    sheet1.Cells(i + 1, 3).Value = item.DataNo
                    sheet1.Cells(i + 1, 4).Value = item.WADL01
                    sheet1.Cells(i + 1, 5).Value = item.Location
                    sheet1.Cells(i + 1, 7).Value = item.WADCTO
                    sheet1.Cells(i + 1, 8).Value = item.WASRST
                    sheet1.Cells(i + 1, 9).Value = item.ijsid
                    sheet1.Cells(i + 1, 10).Value = item.LineType

                    i += 1

                Next

                sheet1.Cells(1, 1, i, 10).AutoFitColumns()

            End If

            Return xlbook
        End Function

        Public Function InsertInspectionJobSummaryDisplay(ByRef xlbook As ExcelWorkbook, ByVal wilist As List(Of SPCInspection.InspectionSummaryDisplay), ByVal LocationArray As List(Of core.Locationarray)) As ExcelWorkbook

            Dim sheet1 As ExcelWorksheet

            sheet1 = xlbook.Worksheets.Add("JobSummary Report")

            Dim newobj As New SPCInspection.InspectionSummaryDisplay
            Dim bustype As Type = newobj.GetType()
            Dim props As PropertyInfo() = bustype.GetProperties()
            Dim i As Integer = 1

            sheet1.Row(1).Height = 27.75
            For Each info As PropertyInfo In props
                If info.Name.ToUpper() = "CID" Or info.Name.ToUpper() = "UNITCOST" Or info.Name.ToUpper() = "TEMPLATEID" Or info.Name.ToUpper() = "UPDATEDINSPECTIONSTARTED" Then
                    GoTo 102
                End If

                sheet1.Cells(1, i).Value = info.Name.ToUpper()
                sheet1.Cells(1, i).Style.Font.Bold = True
                i += 1
102:
            Next
            sheet1.Column(19).Style.Numberformat.Format = "yyyy-mm-dd hh:mm"
            sheet1.Column(20).Style.Numberformat.Format = "yyyy-mm-dd hh:mm"

            Dim wiarray = wilist.ToArray()
            i = 1
            For Each item In wiarray
                sheet1.Cells(i + 1, 1).Value = wiarray(i - 1).ijsid
                sheet1.Cells(i + 1, 2).Value = wiarray(i - 1).JobType
                sheet1.Cells(i + 1, 3).Value = wiarray(i - 1).DataNo
                sheet1.Cells(i + 1, 4).Value = wiarray(i - 1).JobNumber
                sheet1.Cells(i + 1, 5).Value = wiarray(i - 1).PRP_Code
                sheet1.Cells(i + 1, 6).Value = wiarray(i - 1).UnitDesc
                sheet1.Cells(i + 1, 7).Value = wiarray(i - 1).Location
                sheet1.Cells(i + 1, 8).Value = wiarray(i - 1).Name
                sheet1.Cells(i + 1, 9).Value = wiarray(i - 1).LineType
                sheet1.Cells(i + 1, 10).Value = wiarray(i - 1).TotalInspectedItems
                sheet1.Cells(i + 1, 11).Value = wiarray(i - 1).ItemPassCount
                sheet1.Cells(i + 1, 12).Value = wiarray(i - 1).ItemFailCount
                sheet1.Cells(i + 1, 13).Value = wiarray(i - 1).WOQuantity
                sheet1.Cells(i + 1, 14).Value = wiarray(i - 1).WorkOrderPieces
                sheet1.Cells(i + 1, 15).Value = wiarray(i - 1).AQL_Level
                sheet1.Cells(i + 1, 16).Value = wiarray(i - 1).SampleSize
                sheet1.Cells(i + 1, 17).Value = wiarray(i - 1).RejectLimiter

                sheet1.Cells(i + 1, 19).Value = wiarray(i - 1).STARTED
                If wiarray(i - 1).Technical_PassFail.ToString().Length < 2 Then
                    sheet1.Cells(i + 1, 20).Value = ""
                Else
                    sheet1.Cells(i + 1, 20).Value = wiarray(i - 1).FINISHED
                End If
                sheet1.Cells(i + 1, 21).Value = wiarray(i - 1).MajorsCount
                sheet1.Cells(i + 1, 22).Value = wiarray(i - 1).MinorsCount
                sheet1.Cells(i + 1, 23).Value = wiarray(i - 1).RepairsCount
                sheet1.Cells(i + 1, 24).Value = wiarray(i - 1).ScrapCount
                sheet1.Cells(i + 1, 25).Value = wiarray(i - 1).DHU
                sheet1.Cells(i + 1, 26).Value = wiarray(i - 1).RejectionRate
                'sheet1.Cells(i + 1, 22).Value = wiarray(i - 1).UnitCost
                sheet1.Cells(i + 1, 27).Value = wiarray(i - 1).Comments
                'sheet1.Cells(i + 1, 15).Value = wiarray(i - 1).

                If wiarray(i - 1).Technical_PassFail = "FAIL" Then
                    sheet1.Cells(i + 1, 18).Style.Fill.PatternType = ExcelFillStyle.Solid
                    sheet1.Cells(i + 1, 18).Style.Fill.BackgroundColor.SetColor(Color.Red)
                    sheet1.Cells(i + 1, 18).Style.Font.Color.SetColor(Color.White)
                    sheet1.Cells(i + 1, 18).Style.Font.Bold = True
                    sheet1.Cells(i + 1, 18).Value = 0
                ElseIf wiarray(i - 1).Technical_PassFail = "PASS" Then
                    sheet1.Cells(i + 1, 18).Style.Fill.PatternType = ExcelFillStyle.Solid
                    sheet1.Cells(i + 1, 18).Style.Fill.BackgroundColor.SetColor(Color.Green)
                    sheet1.Cells(i + 1, 18).Style.Font.Color.SetColor(Color.White)
                    sheet1.Cells(i + 1, 18).Style.Font.Bold = True
                    sheet1.Cells(i + 1, 18).Value = 1
                End If


                i += 1
            Next

            sheet1.Cells(1, 1, i, 27).AutoFitColumns()

            Using range As ExcelRange = sheet1.Cells(1, 1, wiarray.Length + 1, 27)
                range.Style.Border.Top.Style = ExcelBorderStyle.Thin
                range.Style.Border.Bottom.Style = ExcelBorderStyle.Thin
                range.Style.Border.Right.Style = ExcelBorderStyle.Thin
                range.Style.Border.Left.Style = ExcelBorderStyle.Thin
                range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center
            End Using


            Return xlbook
        End Function

        Public Function GetSpecsSubgrid(ByRef workbook As ExcelWorkbook, ByVal listssg As List(Of SPCInspection.SpecsSubgrid)) As ExcelWorkbook
            workbook.Worksheets.Add("Spec Details")

            If listssg.Count > 0 Then

                Try
                    Dim sheet1 As ExcelWorksheet = workbook.Worksheets("Spec Details")
                    Dim newobj As New SPCInspection.SpecsSubgrid
                    Dim objtype As Type = newobj.GetType()
                    Dim objprop As System.Reflection.PropertyInfo() = objtype.GetProperties()
                    Dim colint As Integer = 1
                    Dim rowint As Integer = 2
                    Dim objarray = listssg.ToArray()

                    For Each info As PropertyInfo In objprop

                        If info.Name.ToUpper() <> "" Then
                            sheet1.Cells(1, colint).Value = info.Name.ToUpper()
                            colint += 1
                        End If

                    Next
                    sheet1.Column(4).Style.Numberformat.Format = "yyyy-mm-dd hh:mm"
                    For Each item In objarray
                        sheet1.Cells(rowint, 1).Value = item.SMid
                        sheet1.Cells(rowint, 2).Value = item.JobNumber
                        sheet1.Cells(rowint, 3).Value = item.DataNo
                        sheet1.Cells(rowint, 4).Value = item.Timestamp
                        sheet1.Cells(rowint, 5).Value = item.ProductType
                        sheet1.Cells(rowint, 6).Value = item.Spec_Description
                        sheet1.Cells(rowint, 7).Value = item.value
                        sheet1.Cells(rowint, 8).Value = item.Upper_Spec_Value
                        sheet1.Cells(rowint, 9).Value = item.Lower_Spec_Value
                        sheet1.Cells(rowint, 10).Value = item.MeasureValue
                        sheet1.Cells(rowint, 11).Value = item.SpecDelta
                        sheet1.Cells(rowint, 12).Value = item.SpecSource
                        rowint += 1
                    Next

                    sheet1.Cells(1, 1, rowint, colint).AutoFitColumns()
                Catch ex As Exception

                End Try



            End If

            Return workbook

        End Function

        Public Function GetProductSpecDisplay(ByRef workbook As ExcelWorkbook, ByVal ProductSpecs As List(Of SPCInspection.ProductDisplaySpecCollection)) As ExcelWorkbook
            workbook.Worksheets.Add("Specs & Measurements")

            If ProductSpecs.Count > 0 Then

                Try
                    Dim sheet1 As ExcelWorksheet = workbook.Worksheets("Specs & Measurements")
                    Dim newobj As New SPCInspection.ProductDisplaySpecCollection
                    Dim objtype As Type = newobj.GetType()
                    Dim objprop As System.Reflection.PropertyInfo() = objtype.GetProperties()
                    Dim colint As Integer = 1
                    Dim rowint As Integer = 2
                    Dim objarray = ProductSpecs.ToArray()

                    For Each info As PropertyInfo In objprop

                        If info.Name.ToUpper() <> "POM_ROW" Then
                            sheet1.Cells(1, colint).Value = info.Name.ToUpper()
                            colint += 1
                        End If

                    Next
                    sheet1.Cells(1, colint).Value = "OUTOFSPECFLAG"
                    sheet1.Cells(1, 1, 1, colint).Style.Font.Bold = True
                    sheet1.Column(5).Style.Numberformat.Format = "yyyy-mm-dd hh:mm"
                    sheet1.Column(10).Style.Numberformat.Format = "yyyy-mm-dd hh:mm"
                    sheet1.Column(17).Style.Numberformat.Format = "#,##0.00"
                    For Each item In objarray
                        sheet1.Cells(rowint, 1).Value = item.id
                        sheet1.Cells(rowint, 2).Value = item.SpecId
                        sheet1.Cells(rowint, 3).Value = item.DefectId
                        sheet1.Cells(rowint, 4).Value = item.InspectionJobSummaryId
                        sheet1.Cells(rowint, 5).Value = item.Timestamp
                        sheet1.Cells(rowint, 6).Value = item.JobNumber
                        sheet1.Cells(rowint, 7).Value = item.ProductType
                        sheet1.Cells(rowint, 8).Value = item.DataNo
                        sheet1.Cells(rowint, 9).Value = item.ItemNumber
                        sheet1.Cells(rowint, 10).Value = item.Inspection_Started
                        sheet1.Cells(rowint, 11).Value = item.InspectionId
                        sheet1.Cells(rowint, 12).Value = item.MeasureValue
                        sheet1.Cells(rowint, 13).Value = item.Spec_Description
                        sheet1.Cells(rowint, 14).Value = item.value
                        sheet1.Cells(rowint, 15).Value = item.Upper_Spec_Value
                        sheet1.Cells(rowint, 16).Value = item.Lower_Spec_Value
                        sheet1.Cells(rowint, 17).Value = item.SpecDelta
                        sheet1.Cells(rowint, 18).Value = "FALSE"
                        If item.SpecDelta < item.Lower_Spec_Value Or item.SpecDelta > item.Upper_Spec_Value Then
                            sheet1.Row(rowint).Style.Fill.PatternType = ExcelFillStyle.Solid
                            sheet1.Row(rowint).Style.Fill.BackgroundColor.SetColor(Color.Red)
                            sheet1.Row(rowint).Style.Font.Color.SetColor(Color.White)
                            sheet1.Row(rowint).Style.Font.Bold = True
                            sheet1.Cells(rowint, 1).Style.Numberformat.Format = "#,##0"
                            sheet1.Cells(rowint, 2).Style.Numberformat.Format = "#,##0"
                            sheet1.Cells(rowint, 3).Style.Numberformat.Format = "#,##0"
                            sheet1.Cells(rowint, 4).Style.Numberformat.Format = "#,##0"
                            sheet1.Cells(rowint, 9).Style.Numberformat.Format = "#,##0"
                            sheet1.Cells(rowint, 11).Style.Numberformat.Format = "#,##0"
                            sheet1.Cells(rowint, 12).Style.Numberformat.Format = "#,##0.00"
                            sheet1.Cells(rowint, 14).Style.Numberformat.Format = "#,##0.00"
                            sheet1.Cells(rowint, 15).Style.Numberformat.Format = "#,##0.00"
                            sheet1.Cells(rowint, 16).Style.Numberformat.Format = "#,##0.00"
                            sheet1.Cells(rowint, 17).Style.Numberformat.Format = "#,##0.00"
                            sheet1.Cells(rowint, 18).Value = "TRUE"
                        End If

                        rowint += 1
                    Next



                    sheet1.Cells(1, 1, rowint, colint).AutoFitColumns()



                Catch ex As Exception

                End Try

            End If
            Return workbook
        End Function

        Public Function GetSpecSummaryDisplay(ByRef workbook As ExcelWorkbook, ByVal ProductSpecs As List(Of SPCInspection.SpecSummaryDisplay)) As ExcelWorkbook
            workbook.Worksheets.Add("Specs & Measurements")

            If ProductSpecs.Count > 0 Then

                Try
                    Dim sheet1 As ExcelWorksheet = workbook.Worksheets("Specs & Measurements")
                    Dim newobj As New SPCInspection.SpecSummaryDisplay
                    Dim objtype As Type = newobj.GetType()
                    Dim objprop As System.Reflection.PropertyInfo() = objtype.GetProperties()
                    Dim colint As Integer = 1
                    Dim rowint As Integer = 2
                    Dim objarray = ProductSpecs.ToArray()

                    For Each info As PropertyInfo In objprop

                        If info.Name.ToUpper() <> "" And info.Name.ToUpper() <> "LINETYPE" And info.Name.ToUpper() <> "ID" And info.Name.ToUpper() <> "CID" Then
                            sheet1.Cells(1, colint).Value = info.Name.ToUpper()
                            colint += 1
                        End If

                    Next
                    sheet1.Cells(1, 1, 1, colint).Style.Font.Bold = True
                    sheet1.Column(7).Style.Numberformat.Format = "yyyy-mm-dd hh:mm"
                    sheet1.Column(8).Style.Numberformat.Format = "yyyy-mm-dd hh:mm"

                    For Each item In objarray

                        sheet1.Cells(rowint, 1).Value = item.Location
                        sheet1.Cells(rowint, 2).Value = item.id
                        sheet1.Cells(rowint, 3).Value = item.JobNumber
                        sheet1.Cells(rowint, 4).Value = item.DataNo
                        sheet1.Cells(rowint, 5).Value = item.ProductType
                        sheet1.Cells(rowint, 6).Value = item.UnitDesc
                        sheet1.Cells(rowint, 7).Value = item.Inspection_Started
                        sheet1.Cells(rowint, 8).Value = item.Inspection_Finished
                        sheet1.Cells(rowint, 9).Value = item.totcount
                        sheet1.Cells(rowint, 10).Value = item.SpecsMet
                        sheet1.Cells(rowint, 11).Value = item.SpecsFailed
                        
                        If item.SpecsFailed > 0 Then
                            sheet1.Cells(rowint, 11).Style.Fill.PatternType = ExcelFillStyle.Solid
                            sheet1.Cells(rowint, 11).Style.Fill.BackgroundColor.SetColor(Color.Red)
                            sheet1.Cells(rowint, 11).Style.Font.Color.SetColor(Color.White)
                            sheet1.Cells(rowint, 11).Style.Font.Bold = True
                        End If

                        rowint += 1
                    Next

                    sheet1.Cells(1, 1, rowint, colint).AutoFitColumns()

                Catch ex As Exception

                End Try

            End If
            Return workbook
        End Function



        Public Function GetWorkOrderInspection_Table(ByVal fromdate As DateTime, ByVal todate As DateTime, ByVal LocationId As Integer, ByVal LocationArray As List(Of core.Locationarray)) As List(Of SPCInspection.WorkOrderInspection)
            Dim todatestring As String = todate.ToString("yyyy-MM-dd H:mm:ss").Split(" ")(0) + " 23:59:59"
            Dim todateform As DateTime = DateTime.Parse(todatestring)
            Dim bmap_wi As New BMappers(Of SPCInspection.WorkOrderInspection)
            Dim wilist As New List(Of SPCInspection.WorkOrderInspection)
            Dim sql As String

            Dim abresults = (From x In LocationArray Where x.CID = LocationId.ToString() Select x.Abreviation).ToArray()
            Dim loresults = (From x In LocationArray Where x.CID = LocationId.ToString() Select x.text).ToArray()
            If abresults(0) = "ALL" Then
                sql = "select CONVERT(VARCHAR(10), DefectTime, 101) as WorkDate, WorkOrder, max(EmployeeNo) as Auditor, ISNULL(max(woi.DataNo),'DATAN') AS DataNo, ISNULL(max(woi.WorkOrderPieces),0) AS WO_Pieces, (SELECT COUNT(DISTINCT InspectionId) FROM DefectMaster WHERE (WorkOrder = dm.WorkOrder) AND (CONVERT(VARCHAR(10), DefectTime, 101) = CONVERT(VARCHAR(10), dm.DefectTime, 101))) as AQL_Boxes, (SELECT COUNT(DISTINCT InspectionId) FROM DefectMaster WHERE (WorkOrder = dm.WorkOrder) AND (CONVERT(VARCHAR(10), DefectTime, 101) = CONVERT(VARCHAR(10), dm.DefectTime, 101))) * CAST(CasePackConv AS float) as AQL_Pieces, case when max(woi.WorkOrderPieces) <> 0 then CAST((SELECT COUNT(DISTINCT InspectionId) FROM DefectMaster WHERE (WorkOrder = dm.WorkOrder) AND (CONVERT(VARCHAR(10), DefectTime, 101) = CONVERT(VARCHAR(10), dm.DefectTime, 101))) * CAST(CasePackConv AS float) AS float) / CAST(max(woi.WorkOrderPieces) AS float) else 0 end AS AQL_Percent, (SELECT COUNT(*) FROM DefectMaster WHERE (WorkOrder = dm.WorkOrder) AND (CONVERT(VARCHAR(10), DefectTime, 101) = CONVERT(VARCHAR(10), dm.DefectTime, 101)) AND (DefectClass = 'MAJOR')) AS Rejected,  CAST((SELECT COUNT(*) FROM DefectMaster WHERE (WorkOrder = dm.WorkOrder) AND (CONVERT(VARCHAR(10), DefectTime, 101) = CONVERT(VARCHAR(10), dm.DefectTime, 101)) AND (DefectClass = 'MAJOR')) as float) / CAST((SELECT COUNT(DISTINCT InspectionId) FROM DefectMaster WHERE (WorkOrder = dm.WorkOrder) AND (CONVERT(VARCHAR(10), DefectTime, 101) = CONVERT(VARCHAR(10), dm.DefectTime, 101))) * CAST(CasePackConv AS float) as float) AS Rejected_Percent" & vbCrLf &
        "FROM dbo.DefectMaster AS dm INNER JOIN WorkOrderInspection as woi ON dm.DefectID = woi.DefectId" & vbCrLf &
        "WHERE (WorkOrder is not null) AND (CasePackConv is not null) AND (WorkOrder <> '')  AND (DefectTime > CONVERT(DATETIME,'" & fromdate.ToString("yyyy-MM-dd H:mm:ss") & "')) AND (DefectTime < CONVERT(DATETIME,'" & todateform.ToString("yyyy-MM-dd H:mm:ss") & "'))" & vbCrLf &
        "GROUP BY dm.WorkOrder, CONVERT(VARCHAR(10), DefectTime, 101), dm.CasePackConv" & vbCrLf &
        "ORDER BY WorkDate"
            Else
                sql = "select CONVERT(VARCHAR(10), DefectTime, 101) as WorkDate, WorkOrder, max(EmployeeNo) as Auditor, ISNULL(max(woi.DataNo),'DATAN') AS DataNo, ISNULL(max(woi.WorkOrderPieces),0) AS WO_Pieces, (SELECT COUNT(DISTINCT InspectionId) FROM DefectMaster WHERE (WorkOrder = dm.WorkOrder) AND (CONVERT(VARCHAR(10), DefectTime, 101) = CONVERT(VARCHAR(10), dm.DefectTime, 101))) as AQL_Boxes, (SELECT COUNT(DISTINCT InspectionId) FROM DefectMaster WHERE (WorkOrder = dm.WorkOrder) AND (CONVERT(VARCHAR(10), DefectTime, 101) = CONVERT(VARCHAR(10), dm.DefectTime, 101))) * CAST(CasePackConv AS float) as AQL_Pieces, case when max(woi.WorkOrderPieces) <> 0 then CAST((SELECT COUNT(DISTINCT InspectionId) FROM DefectMaster WHERE (WorkOrder = dm.WorkOrder) AND (CONVERT(VARCHAR(10), DefectTime, 101) = CONVERT(VARCHAR(10), dm.DefectTime, 101))) * CAST(CasePackConv AS float) AS float) / CAST(max(woi.WorkOrderPieces) AS float) else 0 end AS AQL_Percent, (SELECT COUNT(*) FROM DefectMaster WHERE (WorkOrder = dm.WorkOrder) AND (CONVERT(VARCHAR(10), DefectTime, 101) = CONVERT(VARCHAR(10), dm.DefectTime, 101)) AND (DefectClass = 'MAJOR')) AS Rejected,  CAST((SELECT COUNT(*) FROM DefectMaster WHERE (WorkOrder = dm.WorkOrder) AND (CONVERT(VARCHAR(10), DefectTime, 101) = CONVERT(VARCHAR(10), dm.DefectTime, 101)) AND (DefectClass = 'MAJOR')) as float) / CAST((SELECT COUNT(DISTINCT InspectionId) FROM DefectMaster WHERE (WorkOrder = dm.WorkOrder) AND (CONVERT(VARCHAR(10), DefectTime, 101) = CONVERT(VARCHAR(10), dm.DefectTime, 101))) * CAST(CasePackConv AS float) as float) AS Rejected_Percent" & vbCrLf &
        "FROM dbo.DefectMaster AS dm INNER JOIN WorkOrderInspection as woi ON dm.DefectID = woi.DefectId" & vbCrLf &
        "WHERE (WorkOrder is not null) AND (CasePackConv is not null) AND (WorkOrder <> '')  AND (DefectTime > CONVERT(DATETIME,'" & fromdate.ToString("yyyy-MM-dd H:mm:ss") & "')) AND (DefectTime < CONVERT(DATETIME,'" & todateform.ToString("yyyy-MM-dd H:mm:ss") & "'))  AND (dm.Location = '" & loresults(0).Trim() & "' or dm.Location = '" & LocationId.ToString() & "' or SUBSTRING(dm.DataType, 0, 4) = '" & abresults(0) & "')" & vbCrLf &
        "GROUP BY dm.WorkOrder, CONVERT(VARCHAR(10), DefectTime, 101), dm.CasePackConv" & vbCrLf &
        "ORDER BY WorkDate"
            End If

            wilist = bmap_wi.GetInspectObject(sql)

            Return wilist
        End Function
        Public Function GetInspectionJobSummary(ByVal fromdate As DateTime, ByVal todate As DateTime, ByVal LocationId As Integer, ByVal LocationArray As List(Of core.Locationarray)) As List(Of SPCInspection.InspectionJobSumarryReport)
            Dim todatestring As String = todate.ToString("yyyy-MM-dd H:mm:ss").Split(" ")(0) + " 23:59:59"
            Dim todateform As DateTime = DateTime.Parse(todatestring)
            Dim bmap_wi As New BMappers(Of SPCInspection.InspectionJobSumarryReport)
            Dim wilist As New List(Of SPCInspection.InspectionJobSumarryReport)
            Dim sql As String
            Dim abresults = (From x In LocationArray Where x.CID = LocationId.ToString() Select x.Abreviation).ToArray()

            If abresults(0) = "ALL" Then
                sql = "SELECT ijs.id, ijs.JobType, tn.Name, ijs.UnitDesc, tn.LineType, ISNULL(ijs.TotalInspectedItems, 0) AS TotalInspectedItems, ijs.JobNumber,lm.Name AS LocationName, ijs.ItemPassCount, ijs.ItemFailCount, ijs.WOQuantity, ijs.WorkOrderPieces, ijs.AQL_Level, ijs.SampleSize, ijs.RejectLimiter, ijs.UnitCost, ISNULL(ijs.Technical_PassFail,CASE WHEN ijs.ItemFailCount >= ijs.RejectLimiter THEN 0 ELSE 1 END) AS Technical_PassFail, ijs.Inspection_Started, ijs.Inspection_Finished AS Inspection_Finished, ISNULL(ijs.Comments, '') AS Comments" & vbCrLf &
                     "FROM InspectionJobSummary  ijs INNER JOIN TemplateName tn on ijs.TemplateId = tn.TemplateId LEFT OUTER JOIN AprManager.dbo.LocationMaster lm on ijs.CID = SUBSTRING(lm.CID, 4, 3)   WHERE (LEN(JobNumber) > 3) AND (JobType = 'WorkOrder') AND (ijs.Inspection_Started >= CONVERT(DATETIME,'" & fromdate.ToString("yyyy-MM-dd H:mm:ss") & "')) AND (ijs.Inspection_Started <= CONVERT(DATETIME,'" & todateform.ToString("yyyy-MM-dd H:mm:ss") & "'))"
            Else
                'sql = "SELECT ijs.id, ijs.JobType, tn.Name, tn.LineType, ijs.TotalInspectedItems, ijs.JobNumber, ijs.ItemPassCount, ijs.ItemFailCount, ijs.WOQuantity, ijs.WorkOrderPieces, ijs.AQL_Level, ijs.SampleSize, ijs.RejectLimiter, ijs.Technical_PassFail, ijs.Inspection_Started, ijs.Inspection_Finished, ijs.Comments" & vbCrLf &
                '    "FROM InspectionJobSummary  ijs INNER JOIN TemplateName tn on ijs.TemplateId = tn.TemplateId WHERE (CID = '" & LocationId.ToString() & "') AND (LEN(JobNumber) > 3) AND (JobType = 'WorkOrder')"
                sql = "SELECT ijs.id, ijs.JobType, tn.Name, ijs.UnitDesc, tn.LineType, ISNULL(ijs.TotalInspectedItems, 0) AS TotalInspectedItems, ijs.JobNumber,lm.Name AS LocationName, ijs.ItemPassCount, ijs.ItemFailCount, ijs.WOQuantity, ijs.WorkOrderPieces, ijs.AQL_Level, ijs.SampleSize, ijs.RejectLimiter, ijs.UnitCost, ISNULL(ijs.Technical_PassFail,CASE WHEN ijs.ItemFailCount >= ijs.RejectLimiter THEN 0 ELSE 1 END) AS Technical_PassFail, ijs.Inspection_Started, ijs.Inspection_Finished AS Inspection_Finished, ISNULL(ijs.Comments, '') AS Comments" & vbCrLf &
                        "FROM InspectionJobSummary  ijs INNER JOIN TemplateName tn on ijs.TemplateId = tn.TemplateId LEFT OUTER JOIN AprManager.dbo.LocationMaster lm on ijs.CID = SUBSTRING(lm.CID, 4, 3)   WHERE (ijs.CID = '" & LocationId.ToString() & "') AND (LEN(JobNumber) > 3) AND (JobType = 'WorkOrder')  AND (ijs.Inspection_Started >= CONVERT(DATETIME,'" & fromdate.ToString("yyyy-MM-dd H:mm:ss") & "')) AND (ijs.Inspection_Started <= CONVERT(DATETIME,'" & todateform.ToString("yyyy-MM-dd H:mm:ss") & "'))"
            End If

            wilist = bmap_wi.GetInspectObject(sql)

            Return wilist

        End Function

        Public Function GetWorkOrderInspectionSummary(ByVal fromdate As DateTime, ByVal todate As DateTime, ByVal LocationId As Integer, ByVal LocationArray As List(Of core.Locationarray)) As List(Of SPCInspection.WorkOrderInspectionSummary)
            Dim todatestring As String = todate.ToString("yyyy-MM-dd H:mm:ss").Split(" ")(0) + " 23:59:59"
            Dim todateform As DateTime = DateTime.Parse(todatestring)
            Dim bmap_wi As New BMappers(Of SPCInspection.WorkOrderInspectionSummary)
            Dim wilist As New List(Of SPCInspection.WorkOrderInspectionSummary)
            Dim sql As String

            Dim abresults = (From x In LocationArray Where x.CID = LocationId.ToString() Select x.Abreviation).ToArray()
            Dim loresults = (From x In LocationArray Where x.CID = LocationId.ToString() Select x.text).ToArray()
            If abresults(0) = "ALL" Then
                sql = "select CONVERT(VARCHAR(10), DefectTime, 101) as WorkDate, WorkOrder, max(EmployeeNo) as Auditor, ISNULL(max(woi.DataNo),'DATAN') AS DataNo, ISNULL(max(woi.WorkOrderPieces),0) AS WO_Pieces, CAST(ISNULL(max(dm.SampleSize),0) AS FLOAT) AS AQL_Pieces, case when max(dm.SampleSize) <> 0 then CAST((SELECT COUNT(DISTINCT DefectID) FROM DefectMaster WHERE (WorkOrder = dm.WorkOrder)) AS float) / CAST(max(dm.SampleSize) AS float) else 0 end AS DefectRate, (SELECT COUNT(*) FROM DefectMaster WHERE (WorkOrder = dm.WorkOrder) AND (DefectClass = 'MAJOR')) AS Rejected,  case when max(dm.SampleSize) <> 0 then CAST((SELECT COUNT(DISTINCT DefectID) FROM DefectMaster WHERE (WorkOrder = dm.WorkOrder) AND (DefectClass = 'MAJOR')) AS float) / CAST(max(dm.SampleSize) AS float) else 0 end AS Rejected_Percent, ISNULL(MAX(dm.RejectLimiter),0) AS RejectLimiter" & vbCrLf &
        "FROM dbo.DefectMaster AS dm INNER JOIN WorkOrderInspection as woi ON dm.DefectID = woi.DefectId" & vbCrLf &
        "WHERE (WorkOrder is not null) AND (WorkOrder <> '')  AND (DefectTime > CONVERT(DATETIME,'" & fromdate.ToString("yyyy-MM-dd H:mm:ss") & "')) AND (DefectTime < CONVERT(DATETIME,'" & todateform.ToString("yyyy-MM-dd H:mm:ss") & "'))" & vbCrLf &
        "GROUP BY dm.WorkOrder, CONVERT(VARCHAR(10), DefectTime, 101)" & vbCrLf &
        "ORDER BY WorkDate"
            Else
                sql = "select CONVERT(VARCHAR(10), DefectTime, 101) as WorkDate, WorkOrder, max(EmployeeNo) as Auditor, ISNULL(max(woi.DataNo),'DATAN') AS DataNo, ISNULL(max(woi.WorkOrderPieces),0) AS WO_Pieces, CAST(ISNULL(max(dm.SampleSize),0) AS FLOAT) AS AQL_Pieces, case when max(dm.SampleSize) <> 0 then CAST((SELECT COUNT(DISTINCT DefectID) FROM DefectMaster WHERE (WorkOrder = dm.WorkOrder)) AS float) / CAST(max(dm.SampleSize) AS float) else 0 end AS DefectRate, (SELECT COUNT(*) FROM DefectMaster WHERE (WorkOrder = dm.WorkOrder) AND (DefectClass = 'MAJOR')) AS Rejected,  case when max(dm.SampleSize) <> 0 then CAST((SELECT COUNT(DISTINCT DefectID) FROM DefectMaster WHERE (WorkOrder = dm.WorkOrder) AND (DefectClass = 'MAJOR')) AS float) / CAST(max(dm.SampleSize) AS float) else 0 end AS Rejected_Percent, ISNULL(MAX(dm.RejectLimiter),0) AS RejectLimiter" & vbCrLf &
        "FROM dbo.DefectMaster AS dm INNER JOIN WorkOrderInspection as woi ON dm.DefectID = woi.DefectId" & vbCrLf &
        "WHERE (WorkOrder is not null) AND (WorkOrder <> '')  AND (DefectTime > CONVERT(DATETIME,'" & fromdate.ToString("yyyy-MM-dd H:mm:ss") & "')) AND (DefectTime < CONVERT(DATETIME,'" & todateform.ToString("yyyy-MM-dd H:mm:ss") & "'))  AND (dm.Location = '" & loresults(0).Trim() & "' or dm.Location = '" & LocationId.ToString() & "' or SUBSTRING(dm.DataType, 0, 4) = '" & abresults(0) & "')" & vbCrLf &
        "GROUP BY dm.WorkOrder, CONVERT(VARCHAR(10), DefectTime, 101), dm.Location" & vbCrLf &
        "ORDER BY WorkDate"
            End If

            wilist = bmap_wi.GetInspectObject(sql)

            Return wilist
        End Function

    End Class


End Namespace