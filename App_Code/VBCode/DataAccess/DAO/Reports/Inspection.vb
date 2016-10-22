
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

    Public Class InpectionReports
        Sub New(ByVal SheetSelector As Integer)
            Sheetselect = SheetSelector
        End Sub
        Public ErrorMessage As String
        Public pfromdate As String
        Public ptodate As String
        Public Sheetselect As Integer
        Public Yearcurrent As Integer = DateTime.Now.Year
        Public TodatePublic As String
        Dim lastrow As Integer = 0

        Public Function WorkOrderInspection(ByRef xlbook As ExcelWorkbook, ByVal fromdate As DateTime, ByVal todate As DateTime, ByVal TemplateId As Integer) As ExcelWorkbook
            Dim todatestring As String = todate.ToString("yyyy-MM-dd H:mm:ss").Split(" ")(0) + " 23:59:59"
            Dim todateform As DateTime = DateTime.Parse(todatestring)
            Dim sheet1 As ExcelWorksheet
            Dim bmap_wi As New BMappers(Of SPCInspection.WorkOrderInspection)
            Dim wilist As New List(Of SPCInspection.WorkOrderInspection)
            Dim sql As String

            sql = "select CONVERT(VARCHAR(10), DefectTime, 101) as WorkDate, WorkOrder, max(EmployeeNo) as Auditor, ISNULL(max(woi.DataNo),'DATAN') AS DataNo, ISNULL(max(woi.WorkOrderPieces),0) AS WO_Pieces, COUNT(dm.InspectionId) as AQL_Boxes, COUNT(dm.InspectionId) * 36 as AQL_Pieces, case when max(woi.WorkOrderPieces) <> 0 then 100 * CAST(COUNT(dm.InspectionId) * 36 AS float) / CAST(max(woi.WorkOrderPieces) AS float) else 0 end AS AQL_Percent, (SELECT COUNT(*) FROM DefectMaster WHERE (WorkOrder = dm.WorkOrder) AND (DefectClass = 'MAJOR') OR (DefectClass = 'DEFAULT')) AS Rejected, 100 * CAST((SELECT COUNT(*) FROM DefectMaster WHERE (WorkOrder = dm.WorkOrder) AND (DefectClass = 'MAJOR') OR (DefectClass = 'DEFAULT')) as float) / CAST(COUNT(dm.InspectionId) * 36 as float) AS Rejected_Percent" & vbCrLf &
                    "FROM dbo.DefectMaster AS dm INNER JOIN WorkOrderInspection as woi ON dm.DefectID = woi.DefectId" & vbCrLf &
                    "WHERE (WorkOrder is not null) AND (WorkOrder <> '') AND (ISNUMERIC(WorkOrder) = 1) AND (TemplateId = " & TemplateId.ToString() & ")  AND (DefectTime < CONVERT(DATETIME,'" & fromdate.ToString("yyyy-MM-dd H:mm:ss") & "')) AND (DefectTime > CONVERT(DATETIME,'" & todateform.ToString("yyyy-MM-dd H:mm:ss") & "'))" & vbCrLf &
                    "GROUP BY dm.WorkOrder, dm.InspectionId, CONVERT(VARCHAR(10), DefectTime, 101)" & vbCrLf &
                    "ORDER BY WorkDate"

            wilist = bmap_wi.GetInspectObject(sql)

            If wilist.Count > 0 Then
                sheet1 = xlbook.Worksheets.Add("Daily AQL Report")

                Dim newobj As New SPCInspection.WorkOrderInspection
                Dim bustype As Type = newobj.GetType()
                Dim props As PropertyInfo() = bustype.GetProperties()
                Dim i As Integer = 1

                For Each info As PropertyInfo In props
                    sheet1.Cells(0, i).Value = info.Name.ToUpper()
                    i += 1
                Next
                sheet1.Column(1).Style.Numberformat.Format = "yyyy-mm-dd"
                sheet1.Column(8).Style.Numberformat.Format = "#0.00%"
                Dim wiarray = wilist.ToArray()
                i = 0
                For Each item In wiarray
                    sheet1.Cells(i, 1).Value = wiarray(i).WorkDate
                    sheet1.Cells(i, 2).Value = wiarray(i).WorkOrder
                    sheet1.Cells(i, 3).Value = wiarray(i).Auditor
                    sheet1.Cells(i, 4).Value = wiarray(i).DataNo
                    sheet1.Cells(i, 5).Value = wiarray(i).WO_Pieces
                    sheet1.Cells(i, 6).Value = wiarray(i).AQL_Boxes
                    sheet1.Cells(i, 7).Value = wiarray(i).AQL_Pieces
                    sheet1.Cells(i, 8).Value = wiarray(i).AQL_Percent
                    If wiarray(i).AQL_Percent > 0.05 Then
                        sheet1.Cells(i, 8).Style.Fill.BackgroundColor.SetColor(Color.Green)
                    End If
                    sheet1.Cells(i, 9).Value = wiarray(i).Rejected
                    sheet1.Cells(i, 10).Value = wiarray(i).Rejected_Percent

                    i += 1
                Next

                Using range As ExcelRange = sheet1.Cells(1, 1, wiarray.Length, 10)
                    range.Style.Border.Top.Style = ExcelBorderStyle.Thin
                    range.Style.Border.Bottom.Style = ExcelBorderStyle.Thin
                    range.Style.Border.Right.Style = ExcelBorderStyle.Thin
                    range.Style.Border.Left.Style = ExcelBorderStyle.Thin
                    range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center
                End Using
                sheet1.Column(1).Width = 12.5
                sheet1.Column(2).Width = 8.29
                sheet1.Column(3).Width = 8.29
                sheet1.Column(4).Width = 10
                sheet1.Column(5).Width = 8.57
                sheet1.Column(6).Width = 8.29
                sheet1.Column(7).Width = 8.29
                sheet1.Column(8).Width = 8.29
                sheet1.Column(9).Width = 8.29
                sheet1.Column(10).Width = 8.29

            End If

            Return xlbook
        End Function

    End Class


End Namespace