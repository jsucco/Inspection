Imports MySql.Data.MySqlClient
Imports System.Net.Mail
Imports System.Net
Imports System.IO
Imports System.ComponentModel
Imports C1.C1Excel
Imports System.Data
Imports System.Drawing


Namespace core


    Public Class TexpaFlat

        Dim LoomString1 As String
        Dim LoomConnection As MySqlConnection
        Dim LoomReader As MySqlDataReader
        Dim LoomNewRow As DataRow
        Dim LoomcmdBuilder As MySqlCommandBuilder
        Dim Loomcmd As MySqlCommand


        Dim DBname() As String = {"WeavingDefect", "finishingdefect", "sewingdefect"}
        Dim ColArray() As String = {"F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z", "AA", "AB", "AC", "AD", "AE", "AF", "AG", "AH", "AI", "AJ", "AK", "AL", "AM"}
        Dim lastrow As Integer = 0
        'Dim Texpa_FlatDef() As String = {"INSIDE TAILS", "BROKEN PICKS", "THIN PLACES", "SEAMS", "OIL SPOTS", "SELVEDGE STRINGS", "WEAVE SEAMS", "COLORED FLY", "BLACK SPOTS/STAINS", "BLUE DYE SPOTS/STAIN", "BROWN SPOTS/STAINS", "RED DYE SPOTS/STAIN", "SEAMS", "HOLES", "CLIP OUT", "NARROW FABRIC", "GRAY SPOTS/STAINS", "TORN SELVAGE", "FINISH DIRTY HANDLING", "FINISH SEAMS", "SHADED FABRIC", "TEARS", "PLEATED FABRIC", "DIRTY HANDLING", "SEAMS", "RAW HEMS", "LIGHT OIL", "SEW DIRTY HANDLING", "SEW SEAMS"}
        Public Shared sheetselect As String

        Public Function injectJobSum(ByRef xlbook As C1XLBook, ByVal fromdate As String, ByVal todate As String, ByVal sheetnum As Integer) As C1XLBook


            Dim record As IDataReader
            Dim sheet1 As XLSheet
            Dim colstyle As XLStyle = New XLStyle(xlbook)
            Dim datestyle As XLStyle = New XLStyle(xlbook)
            Dim i As Integer = 1
            Dim j As Integer = 0
            Dim reclength As Integer


            sheet1 = xlbook.Sheets(sheetselect)

            colstyle.BackColor = Color.LightGray
            datestyle.Format = "MM/DD/YYYY hh:mm"
            Try

                LoomString1 = getquerystring(sheetnum, fromdate, todate)
                LoomConnection = New MySqlConnection("server=STT-SERVER1-PC;port=3310;user id=akab1;password=hebron;database=Production")
                LoomConnection.Open()
                Loomcmd = New MySqlCommand(LoomString1, LoomConnection)
                LoomReader = Loomcmd.ExecuteReader(CommandBehavior.CloseConnection)

            Catch ex As Exception

            End Try

            While LoomReader.Read
                record = CType(LoomReader, IDataRecord)
                reclength = record.FieldCount

                Try

                    sheet1(i, 0).Value = Convert.ToInt32(record(0))
                    sheet1(i, 1).Value = Convert.ToInt32(record(1))
                    sheet1(i, 2).Value = Convert.ToString(record(2))
                    sheet1(i, 3).Value = Convert.ToString(record(3))
                    sheet1(i, 4).Value = Convert.ToString(record(4))
                    sheet1(i, 5).Value = Convert.ToInt32(record(5))
                    sheet1(i, 6).Value = Convert.ToInt32(record(6))
                    sheet1(i, 7).Value = Convert.ToDouble(record(7))
                    sheet1(i, 8).Value = Convert.ToInt32(record(8))
                    sheet1(i, 9).Value = Convert.ToInt32(record(9))
                    sheet1(i, 10).Value = Convert.ToInt32(record(10))
                    sheet1(i, 11).Value = Convert.ToDouble(record(11))
                    sheet1(i, 12).Value = Convert.ToDouble(record(12))
                    sheet1(i, 12).Style = colstyle
                    sheet1(i, 13).Value = Convert.ToDouble(record(13))
                    sheet1(i, 14).Value = Convert.ToInt32(record(14))
                    sheet1(i, 15).Value = Convert.ToInt32(record(15))
                    sheet1(i, 16).Value = Convert.ToInt32(record(16))
                    sheet1(i, 17).Value = Convert.ToInt32(record(17))
                    sheet1(i, 18).Value = Convert.ToInt32(record(18))
                    sheet1(i, 19).Value = Convert.ToInt32(record(19))
                    sheet1(i, 20).Value = Convert.ToInt32(record(20))
                    sheet1(i, 21).Value = Convert.ToInt32(record(21))
                    sheet1(i, 22).Value = Convert.ToInt32(record(22))
                    sheet1(i, 23).Value = Convert.ToInt32(record(23))
                    sheet1(i, 24).Value = Convert.ToInt32(record(24))
                    sheet1(i, 25).Value = Convert.ToInt32(record(25))
                    sheet1(i, 26).Value = Convert.ToInt32(record(26))
                    sheet1(i, 27).Value = Convert.ToInt32(record(27))
                    sheet1(i, 28).Value = Convert.ToInt32(record(28))
                    sheet1(i, 29).Value = Convert.ToInt32(record(29))
                    sheet1(i, 30).Value = Convert.ToInt32(record(30))
                    sheet1(i, 31).Value = Convert.ToInt32(record(31))
                    sheet1(i, 32).Value = Convert.ToInt32(record(32))
                    sheet1(i, 33).Value = Convert.ToInt32(record(33))
                    sheet1(i, 34).Value = Convert.ToInt32(record(34))
                    sheet1(i, 35).Value = Convert.ToInt32(record(35))
                    sheet1(i, 36).Value = Convert.ToInt32(record(36))
                    sheet1(i, 37).Value = Convert.ToInt32(record(37))
                    sheet1(i, 38).Value = Convert.ToInt32(record(38))
                    sheet1(i, 39).Value = Convert.ToDateTime(record(39))
                    sheet1(i, 39).Style = datestyle
                    sheet1(i, 40).Value = Convert.ToDouble(record(40))
                    sheet1(i, 41).Value = Convert.ToDouble(record(41))
                    sheet1(i, 42).Value = Convert.ToDouble(record(42))
                    sheet1(i, 43).Value = Convert.ToDouble(record(43))
                    sheet1(i, 44).Value = Convert.ToDouble(record(44))
                    sheet1(i, 45).Value = Convert.ToDateTime(record(45))
                    sheet1(i, 45).Style = datestyle
                    sheet1(i, 46).Value = Convert.ToString(record(46))


                Catch ex As Exception
                    MsgBox(ex.Message)
                End Try

                i = i + 1

            End While

            LoomConnection.Close()
            LoomConnection.Dispose()

            lastrow = i

            Return xlbook



        End Function

        Public Function injectsubstats(ByRef xlbook As C1XLBook, ByVal Todate As String) As C1XLBook


            Dim Sheet1 As XLSheet
            Dim subsrow As Integer
            Dim SS1Style As XLStyle = New XLStyle(xlbook)
            Dim SS2Style As XLStyle = New XLStyle(xlbook)
            Dim SS3Style As XLStyle = New XLStyle(xlbook)

            SS3Style.Font = New Font("Calibri", 11, FontStyle.Bold)
            SS3Style.BackColor = Color.Yellow
            SS3Style.SetBorderStyle(XLLineStyleEnum.Thick)
            SS3Style.Format = "MM/DD/YYYY"

            SS1Style.Font = New Font("Calibri", 11, FontStyle.Bold)
            SS1Style.BackColor = Color.LightBlue
            SS1Style.SetBorderStyle(XLLineStyleEnum.Thick)

            SS2Style.Font = New Font("Calibri", 11, FontStyle.Bold)
            SS2Style.BackColor = Color.LightGreen
            SS2Style.SetBorderStyle(XLLineStyleEnum.Thick)
            SS2Style.Format = "#.##%"


            subsrow = lastrow + 2
            Sheet1 = xlbook.Sheets(sheetselect)
            Sheet1(lastrow + 1, 1).Value = Convert.ToDateTime(Todate)
            Sheet1(lastrow + 1, 1).Style = SS3Style
            Sheet1(lastrow + 1, 4).Value = "TOTAL"
            Sheet1(lastrow + 1, 4).Style = SS1Style

            For i = 0 To 33
                Sheet1(lastrow + 1, i + 5).Formula = "SUBTOTAL(9," + ColArray(i) + "2:" + ColArray(i) + Convert.ToString(lastrow) + ")"
                Sheet1(lastrow + 1, i + 5).Style = SS1Style
                Select Case i
                    Case 0, 1, 2, 6, 7, 8
                        GoTo 1010
                    Case Else
                        Sheet1(lastrow + 2, i + 5).Formula = ColArray(i) + Convert.ToString(subsrow) + "/$G" + Convert.ToString(subsrow)
                        Sheet1(lastrow + 2, i + 5).Style = SS2Style
                End Select
1010:
            Next


            AutoSizeColumns(Sheet1, xlbook)

            Return xlbook

        End Function


        Private Sub AutoSizeColumns(ByVal sheet As XLSheet, ByRef C1XLBook1 As C1XLBook)

            Using g As Graphics = Graphics.FromHwnd(IntPtr.Zero)

                Dim r As Integer, c As Integer
                For c = 0 To sheet.Columns.Count - 1

                    Dim colWidth As Integer = -1
                    For r = 0 To sheet.Rows.Count - 1

                        Dim value As Object = sheet(r, c).Value
                        If Not (value Is Nothing) Then

                            ' get value (unformatted at this point)
                            Dim text As String = value.ToString()

                            ' format value if cell has a style with format set
                            Dim s As XLStyle = sheet(r, c).Style
                            Try
                                If Not (s Is Nothing) And s.Format.Length > 0 Then
                                    Dim ifmt As IFormattable = TryCast(value, IFormattable)
                                    If Not (ifmt Is Nothing) Then
                                        Dim fmt As String = XLStyle.FormatXLToDotNet(s.Format)
                                        text = ifmt.ToString(fmt, System.Globalization.CultureInfo.CurrentCulture)
                                    End If
                                End If
                            Catch ex As Exception

                            End Try
                            ' get font (default or style)
                            Dim font As Font = C1XLBook1.DefaultFont
                            If Not (s Is Nothing) Then
                                If Not (s.Font Is Nothing) Then
                                    font = s.Font
                                End If
                            End If

                            ' measure string (add a little tolerance)
                            Dim sz As Size = System.Drawing.Size.Ceiling(g.MeasureString(text + "XX", font))

                            ' keep widest so far
                            If sz.Width > colWidth Then
                                colWidth = sz.Width
                            End If
                        End If

                        ' done measuring, set column width
                        If colWidth > -1 Then
                            sheet.Columns(c).Width = C1XLBook.PixelsToTwips(colWidth)
                        End If
                    Next
                Next
            End Using
        End Sub


        Public Function getquerystring(ByVal sheetnum As Integer, ByVal fromdate As String, ByVal todate As String)

            Dim querystring As String

            Dim value As Object = sheetnum
            If Not (value Is Nothing) Then

                Select Case sheetnum
                    Case 1
                        querystring = "select * from jobsummary where FinishTime >= '" & fromdate & "' and FinishTime <= '" & todate & "' and (Machine = 'STT_TEXPA1' OR Machine = 'STT_TEXPA2')"
                    Case 2
                        querystring = "select * from jobsummary where FinishTime >= '" & fromdate & "' and FinishTime <= '" & todate & "' and Machine = 'STT_TEXPA3'"
                    Case 3
                        querystring = "select * from jobsummary where FinishTime >= '" & fromdate & "' and FinishTime <= '" & todate & "' and (Machine = 'STT_PILLOW1' OR Machine = 'STT_PILLOW2')"
                    Case Else
                        querystring = "String Not Found"

                End Select

                Return querystring

            Else

                Return "sheetnum required"

            End If

        End Function

    End Class




End Namespace