
Imports linechart = C1.Web.Wijmo.Controls.C1Chart.C1LineChart
Imports System.IO
Imports OfficeOpenXml
Imports System.Drawing
Imports System.Web.Security
Imports System.Security.Principal
Imports System.Threading

Namespace core

    Partial Class UTILITIES_gaschart
        Inherits System.Web.UI.Page

        Public Shared txtfromdate As DateTime = "02/04/2014 00:00:00"
        Public Shared txttodate As DateTime = "02/09/2014 00:00:00"
        Public Shared sch1array() As String = {"Milnor Hot Water Temp (degF)", "Milnor Cold Water Temp (degF)", "Milnor (Amps)", "Milnor Hot Water (gal)", "Milnor Cold Water (gal)", "Milnor Steam (lbs)", "Milnor Hot Usage (Btus)"}
        Public Shared sch2array() As String = {"Main Cold Water (gal)", "Main Natural Gas (SCFM)", "Main Hot Water (gal)", "Main Power (Amps)", "Main Power (Watts)", "Main Hot Water (btu)"}

        Public Event MenuItemClick As MenuEventHandler
        Dim instance As Menu
        Dim handler As MenuEventHandler

        Public Property testfield As String = "test123"

        Sub Menuevent(ByVal sender As Object, ByVal e As MenuEventArgs) Handles test.MenuItemClick

            Dim selected As String = e.Item.Text
            Dim dtaker As New dtaker


            If selected = "Milnor Hot Water Temp" Then
                dtaker.chartselector = 0
                dtaker.schselector = 1
            ElseIf selected = "Milnor Cold Water Temp" Then
                dtaker.chartselector = 1
                dtaker.schselector = 1
            ElseIf selected = "Milnor Amps" Then
                dtaker.chartselector = 2
                dtaker.schselector = 1
            ElseIf selected = "Milnor Hot Water" Then
                dtaker.chartselector = 3
                dtaker.schselector = 1
            ElseIf selected = "Milnor Cold Water" Then
                dtaker.chartselector = 4
                dtaker.schselector = 1
            ElseIf selected = "Milnor Steam" Then
                dtaker.chartselector = 5
                dtaker.schselector = 1
            ElseIf selected = "Milnor Hot Btu" Then
                dtaker.chartselector = 6
                dtaker.schselector = 1
            ElseIf selected = "Main Cold Water" Then
                dtaker.chartselector = 0
                dtaker.schselector = 2
            ElseIf selected = "Main Natural Gas" Then
                dtaker.chartselector = 1
                dtaker.schselector = 2
            ElseIf selected = "Main Hot Water" Then
                dtaker.chartselector = 2
                dtaker.schselector = 2
            ElseIf selected = "Main Amps" Then
                dtaker.chartselector = 3
                dtaker.schselector = 2
            ElseIf selected = "Main Power" Then
                dtaker.chartselector = 4
                dtaker.schselector = 2
            ElseIf selected = "Main Hot Water btu" Then
                dtaker.chartselector = 5
                dtaker.schselector = 2
            End If

            'redraw()
            Response.Redirect("~/UTILITIES/chartpage.aspx")

        End Sub

        Protected Sub Button1_Click(sender As Object, e As System.EventArgs) Handles Button1.Click
            Dim dtaker As New dtaker

            dtaker.specdrawcnt += 1
            txtfromdate = TxtDateFrom.Date
            txttodate = TxtDateTo.Date
            Response.Redirect("~/UTILITIES/chartpage.aspx")
        End Sub

        Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load

            Dim threadPrincipal As IPrincipal = Thread.CurrentPrincipal
            Dim IsAutho As String = threadPrincipal.Identity.IsAuthenticated
            Dim UserName As String = Context.User.Identity.Name

            Dim dtaker = New dtaker

            Session("namefield") = dtaker.chartselector
            Session("schedule") = dtaker.schselector

            If dtaker.schselector = 1 Then
                Chart_title.Text = sch1array(dtaker.chartselector)
            ElseIf dtaker.schselector = 2 Then
                Chart_title.Text = sch2array(dtaker.chartselector)
            End If

            If dtaker.specdrawcnt > dtaker.specdrawcnt2 Then
                If TxtDateFrom.Date <> txtfromdate Then
                    TxtDateFrom.Date = txtfromdate
                End If
                If TxtDateTo.Date <> txttodate Then
                    TxtDateTo.Date = txttodate
                End If
                dtaker.specdrawcnt2 += 1
            End If
        End Sub

        Private Sub redraw()
            Dim dtaker = New dtaker



            Session("namefield") = dtaker.chartselector
            Session("schedule") = dtaker.schselector

            If dtaker.schselector = 1 Then
                Chart_title.Text = sch1array(dtaker.chartselector)
            ElseIf dtaker.schselector = 2 Then
                Chart_title.Text = sch2array(dtaker.chartselector)
            End If

            If dtaker.specdrawcnt > dtaker.specdrawcnt2 Then
                If TxtDateFrom.Date <> txtfromdate Then
                    TxtDateFrom.Date = txtfromdate
                End If
                If TxtDateTo.Date <> txttodate Then
                    TxtDateTo.Date = txttodate
                End If
                dtaker.specdrawcnt2 += 1
            End If

        End Sub
        Protected Sub Button2_Click(sender As Object, e As System.EventArgs) Handles Button2.Click

            'Dim fromdate As DateTime = Convert.ToDateTime("2014-01-23 21:28:00")
            'Dim todate As DateTime = Convert.ToDateTime("2014-01-29 12:52:00")
            Dim fromdate As DateTime = TxtDateFrom.Date
            Dim todate As DateTime = TxtDateTo.Date
            Dim dtaker As New dtaker
            Dim excelPackage = New ExcelPackage
            Dim excelWorksheet = excelPackage.Workbook.Worksheets.Add("COREData_Exported")
            Dim dataarray As Array
            Dim arraylength As Int16
            Dim value As usage
            Dim timestamp As DateTime
            Dim usage As Decimal
            Dim datatitle As String
            Dim chartselector As Int16 = dtaker.chartselector
            Dim schedule As Int16 = dtaker.schselector

            Label1.Text = "Please Wait Loading Data"
            dataarray = dtaker.getspecData(fromdate, todate, chartselector, schedule).ToArray
            arraylength = dataarray.Length

            If schedule = 1 Then
                datatitle = sch1array(chartselector)
            Else
                datatitle = sch2array(chartselector)
            End If

            excelWorksheet.Cells("A1").Value = "TimeStamp"
            excelWorksheet.Cells("B1").Value = datatitle
            excelWorksheet.Cells("A1:B1").Style.Font.Bold = True
            excelWorksheet.Cells("A2:A:" + Convert.ToString(arraylength + 5)).Style.Numberformat.Format = "YYYY-MM-DD hh:mm:ss"
            excelWorksheet.Column(1).BestFit = True

            For i = 0 To arraylength - 1

                value = dataarray(i)
                timestamp = value.Timestamp
                usage = value.Usage1

                excelWorksheet.SetValue(i + 2, 1, timestamp)
                excelWorksheet.SetValue(i + 2, 2, usage)
                'excelWorksheet.Cells(i + 2, 1).Value = timestamp
                'excelWorksheet.Cells(i + 2, 2).Value = usage

            Next


            excelWorksheet.Cells.AutoFitColumns(0)
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
            Response.AddHeader("content-disposition", "attachment;  filename=COREData.xlsx")

            Dim stream As MemoryStream = New MemoryStream(excelPackage.GetAsByteArray())

            Response.OutputStream.Write(stream.ToArray(), 0, stream.ToArray().Length)

            Response.Flush()

            Response.Close()




            Label1.Text = ""

        End Sub
    End Class


End Namespace
