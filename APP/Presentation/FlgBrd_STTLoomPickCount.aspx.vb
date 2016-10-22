Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Data
Imports System.Globalization
Imports C1.C1Excel
Imports OfficeOpenXml
Imports System.IO
Imports FiftyOne
Imports FiftyOne.Foundation.Mobile.Detection

Namespace core



    Partial Class APP_Presentation_FlgBrd_STTLoomPickCount
        Inherits core.APRWebApp

        Public Property _DAOFactory As New DAOFactory
        Public Property DL As New dlayer
        Public Property LPC As New LoomPickCountDAO


        Private CSE As New CSExample

        Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load




            'Dim SQL As String
            'Dim sqlcommand As SqlCommand
            'Dim returnint As Integer





            'SQL = "INSERT INTO BR_LoomPickCount (Dornier2PickCounter,Dornier3PickCounter, Dornier4PickCounter, Dornier5PickCounter, Timestamp) VALUES(2, 3, 4, 5, '01-01-2014 00:00:00') "


            'Using connection As New SqlConnection(DL.APRConnectionString)


            '    sqlcommand = _DAOFactory.GetCommand(SQL, connection)
            '    ''    'Add command parameters                                                                          



            '    sqlcommand.Connection.Open()
            '    returnint = sqlcommand.ExecuteNonQuery()




            'End Using





        End Sub

        Protected Sub GoButton_Click(sender As Object, e As System.EventArgs) Handles GoButton.Click
            Dim book As C1XLBook = New C1XLBook()
            Dim sheet1 As XLSheet
            Dim datefrom As String = Request.Form("datefrom")
            Dim dateto As String = Request.Form("dateto")


            Dim interval As Integer = Mins.Checked

            If Not (datefrom Is Nothing) And (dateto Is Nothing) Then
                Exit Sub
            Else
                datefrom = datefrom + " 00:00:00"
                dateto = dateto + " 23:59:59"
            End If


            If Mins.Checked = True Then
                interval = 1
            ElseIf Hrs.Checked = True Then
                interval = 2
            ElseIf Dys.Checked = True Then
                interval = 3
            ElseIf Mnths.Checked = True Then
                interval = 4
            Else
                interval = 2
            End If

            book.Load(Server.MapPath(".") + "\Book1.xlsx")

            sheet1 = book.Sheets("Sheet1")

            LoomPickCountDAO.sheetselect = "Sheet1"
            book = LPC.LoadExportData(book, datefrom, dateto)
            LoomPickCountDAO.sheetselect = "Sheet2"
            book = LPC.LoadExportLoomData(book, datefrom, dateto, interval)


            Dim objStream As System.IO.MemoryStream = New System.IO.MemoryStream

            book.Save(objStream)

            Dim byteArr As Byte() = Array.CreateInstance(GetType(Byte), objStream.Length)
            objStream.Position = 0
            objStream.Read(byteArr, 0, CType(objStream.Length, Integer))
            objStream.Close()
            Response.Clear()
            If Session("ExcelFormat2007") = True Then
                Response.AddHeader("content-disposition", "attachment; filename=STTLooms_Export.xlsx")
            Else
                Response.AddHeader("content-disposition", "attachment; filename=STTLooms_Export.xls")
            End If
            Response.BinaryWrite(byteArr)
            Response.End()


        End Sub
    End Class


End Namespace
