Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Data
Imports System.Globalization
Imports System.Web.Script.Serialization
Imports C1.C1Excel
Imports System.Drawing
Imports System.Reflection


Namespace core

    Public Class DefectMaseter
        Private _DAOFactory As New DAOFactory
        Private DL As New dlayer
        Private Property Inspect As New InspectionUtilityDAO

        Public Function LoadExportData(ByRef xlbook As C1XLBook, ByVal fromdate As String, ByVal todate As String, ByVal TemplateId As Integer, ByVal Location As String, ByVal Ab As String) As C1XLBook

            Dim from_Date As New DateTime()
            Dim to_Date As New DateTime()
            Dim sheet1 As XLSheet
            Dim exportlist As Array
            Dim SS1Style As XLStyle = New XLStyle(xlbook)

            from_Date = DateTime.Parse(fromdate)
            to_Date = DateTime.Parse(todate)
            If (to_Date.Ticks - from_Date.Ticks) > 0 Then
                exportlist = Inspect.GetDefectMasterData(from_Date, to_Date, Location, Ab).ToArray()
                sheet1 = xlbook.Sheets(0)

                SS1Style.Font = New Font("Calibri", 11, FontStyle.Bold)
                SS1Style.SetBorderStyle(XLLineStyleEnum.Thick)
                SS1Style.Format = "MM/DD/YYYY hh:mm"

                Dim RlColumn As XLColumn = sheet1.Columns(0)
                RlColumn = sheet1.Columns(0)
                RlColumn.Width = 1000

                RlColumn = sheet1.Columns(1)
                RlColumn.Width = 2100
                RlColumn = sheet1.Columns(2)
                RlColumn.Width = 2100
                RlColumn = sheet1.Columns(3)
                RlColumn.Width = 1000
                RlColumn = sheet1.Columns(4)
                RlColumn.Width = 1000
                RlColumn = sheet1.Columns(5)
                RlColumn.Width = 1000
                RlColumn = sheet1.Columns(6)
                RlColumn.Width = 3400
                RlColumn = sheet1.Columns(8)
                RlColumn.Width = 1000
                RlColumn = sheet1.Columns(10)
                RlColumn.Width = 1400
                RlColumn = sheet1.Columns(11)
                RlColumn.Width = 1000
                RlColumn = sheet1.Columns(14)
                RlColumn.Width = 1600
                Dim MasterType As Type = GetType(SPCInspection.DefectMasterDisplay)
                Dim properties As PropertyInfo() = MasterType.GetProperties()
                Dim FieldCount As Integer = 0
                For Each item As PropertyInfo In properties
                    Dim teststring As String = item.Name
                    sheet1(0, FieldCount).Value = item.Name
                    FieldCount += 1
                Next

                Dim rowcount As Integer = 1
                For Each item In exportlist
                    sheet1(rowcount, 0).Value = item.DefectId
                    sheet1(rowcount, 1).Value = item.DefectTime
                    sheet1(rowcount, 1).Style = SS1Style
                    sheet1(rowcount, 2).Value = item.DataNo
                    sheet1(rowcount, 3).Value = item.EmployeeNo
                    sheet1(rowcount, 4).Value = item.POnumber
                    sheet1(rowcount, 5).Value = item.InspectionId
                    sheet1(rowcount, 6).Value = item.DefectDesc
                    sheet1(rowcount, 7).Value = item.Product
                    sheet1(rowcount, 8).Value = item.DefectClass
                    sheet1(rowcount, 9).Value = item.AQL
                    sheet1(rowcount, 10).Value = item.TotalLotPieces
                    sheet1(rowcount, 11).Value = item.WorkOrder
                    sheet1(rowcount, 12).Value = item.RollNo
                    sheet1(rowcount, 13).Value = item.LoomNo
                    sheet1(rowcount, 14).Value = item.DataType

                    rowcount += 1
                Next



            End If



            Return xlbook
        End Function

        Public Function LoadExportDataFromList(ByRef xlbook As C1XLBook, ByVal fromdate As String, ByVal todate As String, ByVal DefectList As List(Of SPCInspection.DefectMasterDisplay)) As C1XLBook

            Dim from_Date As New DateTime()
            Dim to_Date As New DateTime()
            Dim sheet1 As XLSheet
            Dim exportlist As Array
            Dim SS1Style As XLStyle = New XLStyle(xlbook)

            from_Date = DateTime.Parse(fromdate)
            to_Date = DateTime.Parse(todate)
            If (to_Date.Ticks - from_Date.Ticks) > 0 Then
                exportlist = DefectList.ToArray()
                sheet1 = xlbook.Sheets(0)

                SS1Style.Font = New Font("Calibri", 11, FontStyle.Bold)
                SS1Style.SetBorderStyle(XLLineStyleEnum.Thick)
                SS1Style.Format = "MM/DD/YYYY hh:mm"

                Dim RlColumn As XLColumn = sheet1.Columns(0)
                RlColumn = sheet1.Columns(0)
                RlColumn.Width = 1000

                RlColumn = sheet1.Columns(1)
                RlColumn.Width = 2100
                RlColumn = sheet1.Columns(2)
                RlColumn.Width = 2100
                RlColumn = sheet1.Columns(3)
                RlColumn.Width = 1000
                RlColumn = sheet1.Columns(4)
                RlColumn.Width = 1000
                RlColumn = sheet1.Columns(5)
                RlColumn.Width = 1000
                RlColumn = sheet1.Columns(6)
                RlColumn.Width = 3400
                RlColumn = sheet1.Columns(8)
                RlColumn.Width = 1000
                RlColumn = sheet1.Columns(10)
                RlColumn.Width = 1400
                RlColumn = sheet1.Columns(11)
                RlColumn.Width = 1000
                RlColumn = sheet1.Columns(14)
                RlColumn.Width = 1600
                Dim MasterType As Type = GetType(SPCInspection.DefectMasterDisplay)
                Dim properties As PropertyInfo() = MasterType.GetProperties()
                Dim FieldCount As Integer = 0
                For Each item As PropertyInfo In properties
                    Dim teststring As String = item.Name
                    sheet1(0, FieldCount).Value = item.Name
                    FieldCount += 1
                Next

                Dim rowcount As Integer = 1
                For Each item In exportlist
                    sheet1(rowcount, 0).Value = item.DefectId
                    sheet1(rowcount, 1).Value = item.DefectTime
                    sheet1(rowcount, 1).Style = SS1Style
                    sheet1(rowcount, 2).Value = item.DataNo
                    sheet1(rowcount, 3).Value = item.EmployeeNo
                    sheet1(rowcount, 4).Value = item.POnumber
                    sheet1(rowcount, 5).Value = item.InspectionId
                    sheet1(rowcount, 6).Value = item.DefectDesc
                    sheet1(rowcount, 7).Value = item.Product
                    sheet1(rowcount, 8).Value = item.DefectClass
                    sheet1(rowcount, 9).Value = item.AQL
                    sheet1(rowcount, 10).Value = item.TotalLotPieces
                    sheet1(rowcount, 11).Value = item.WorkOrder
                    sheet1(rowcount, 12).Value = item.RollNo
                    sheet1(rowcount, 13).Value = item.LoomNo
                    sheet1(rowcount, 14).Value = item.DataType

                    rowcount += 1
                Next



            End If



            Return xlbook
        End Function

        Public Function LoadSpecDataFromList(ByRef xlbook As C1XLBook, ByVal fromdate As String, ByVal todate As String, ByVal SpecList As List(Of SPCInspection.ProductDisplaySpecCollection)) As C1XLBook

            Dim from_Date As New DateTime()
            Dim to_Date As New DateTime()
            Dim sheet1 As XLSheet
            Dim exportlist As Array
            Dim SS1Style As XLStyle = New XLStyle(xlbook)

            from_Date = DateTime.Parse(fromdate)
            to_Date = DateTime.Parse(todate)
            If (to_Date.Ticks - from_Date.Ticks) > 0 Then
                exportlist = SpecList.ToArray()
                sheet1 = xlbook.Sheets(1)

                SS1Style.Font = New Font("Calibri", 11, FontStyle.Bold)
                SS1Style.SetBorderStyle(XLLineStyleEnum.Thin)
                SS1Style.Format = "MM/DD/YYYY hh:mm"

                Dim RlColumn As XLColumn = sheet1.Columns(0)
                RlColumn = sheet1.Columns(0)
                RlColumn.Width = 1000

                Dim MasterType As Type = GetType(SPCInspection.ProductDisplaySpecCollection)
                Dim properties As PropertyInfo() = MasterType.GetProperties()
                Dim FieldCount As Integer = 0
                Dim itemcnt As Integer = 0
                For Each item As PropertyInfo In properties
                    Dim teststring As String = item.Name
                    If itemcnt <> 6 Then
                        sheet1(0, FieldCount).Value = item.Name
                        RlColumn = sheet1.Columns(FieldCount)
                        RlColumn.Width = 2000
                        FieldCount += 1
                    End If
                    itemcnt += 1
                    
                Next

                Dim rowcount As Integer = 1
                For Each item In exportlist
                    sheet1(rowcount, 0).Value = item.SpecId
                    sheet1(rowcount, 1).Value = item.Spec_Value_Upper
                    sheet1(rowcount, 2).Value = item.Spec_Value_Lower
                    sheet1(rowcount, 3).Value = item.TabName
                    sheet1(rowcount, 4).Value = item.MeasureValue
                    sheet1(rowcount, 5).Value = item.Timestamp
                    sheet1(rowcount, 5).Style = SS1Style
                    sheet1(rowcount, 6).Value = item.inspectionid
                    sheet1(rowcount, 7).Value = item.Spec_Description
                    sheet1(rowcount, 8).Value = item.value
                    sheet1(rowcount, 9).Value = item.SpecDelta

                    rowcount += 1
                Next
            End If

            Return xlbook
        End Function

    End Class

End Namespace

