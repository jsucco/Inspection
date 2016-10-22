Imports MySql.Data.MySqlClient
Imports System.Net.Mail
Imports System.Net
Imports System.IO
Imports System.ComponentModel
Imports C1.C1Excel
Imports System.Data


Namespace core


    Public Class DataAccess

        'Dim RemoteMySqlConnString As String = "server=STT-SERVER1-PC;port=3310;user id=akab1;password=hebron;database=Production"
        Dim RemoteMySqlConnString As String = "server=localhost;port=3310;user id=root;password=hebron;database=Production"

        Dim LoomString1 As String
        Dim LoomConnection As MySqlConnection
        Dim LoomReader As MySqlDataReader
        Dim LoomNewRow As DataRow
        Dim LoomcmdBuilder As MySqlCommandBuilder
        Dim Loomcmd As MySqlCommand
        Public Shared pfromdate As String
        Public Shared ptodate As String

        Public Shared LoomNo As New List(Of String)
        Public Shared Sheetselect As String
        Dim weavedefects() As String = {"INSIDE TAILS", "BROKEN PICKS", "DROPPED PICK", "BENT REED", "THIN PLACES", "SLACK PICK", "END OUT", "WOVEN IN WASTE", "WAVY", "TEMPLE MARK", "STOP MARK", "SMASH", "SLUBS", "SELVEDGE STRINGS", "OTHER", "SOILED FILLING", "OVERSHOTS", "OIL SPOTS", "GREASE", "BLACK OIL"}


        Public Function injectLoomData(ByRef xlbook As C1XLBook) As C1XLBook

            Dim record As IDataReader
            Dim sheet1 As XLSheet
            Dim i As Integer = 1

            sheet1 = xlbook.Sheets(Sheetselect)


            LoomString1 = "SELECT DISTINCT LoomNo, StyleNo FROM WeavingDefect WHERE Machine = 'STT_GREIGE1' and DefectTime > '" & pfromdate & "' and DefectTime < '" & ptodate & "'"
            LoomConnection = New MySqlConnection("server=STT-SERVER1-PC;port=3310;user id=akab1;password=hebron;database=Production")
            LoomConnection.Open()
            Loomcmd = New MySqlCommand(LoomString1, LoomConnection)
            LoomReader = Loomcmd.ExecuteReader(CommandBehavior.CloseConnection)

            While LoomReader.Read
                record = CType(LoomReader, IDataRecord)

                LoomNo.Add(Convert.ToString(record(0)))
                sheet1(4, i).Value = Convert.ToInt32(record(0))
                sheet1(5, i).Value = Convert.ToString(record(1))
                'sheet1(5, 9).Value = obj
                i = i + 2

            End While

            LoomConnection.Close()
            LoomConnection.Dispose()

            Return xlbook


        End Function
        
        Public Function injectLoomYds(ByRef xlbook As C1XLBook) As C1XLBook

            Dim record As IDataReader
            Dim LoomArray() As String
            Dim sheet1 As XLSheet

            Dim j As Integer = 0
            Dim i As Integer = 1

            sheet1 = xlbook.Sheets(Sheetselect)
            LoomArray = LoomNo.ToArray()

            For j = 0 To LoomArray.Length - 1

                Dim stringtest As String = LoomArray(j)

                LoomString1 = "SELECT sum(distinct(RollYds)) FROM WeavingDefect WHERE Machine = 'STT_GREIGE1' AND LoomNo = '" & stringtest & "' and DefectTime > '" & pfromdate & "' and DefectTime < '" & ptodate & "'"
                LoomConnection = New MySqlConnection("server=STT-SERVER1-PC;port=3310;user id=akab1;password=hebron;database=Production")
                LoomConnection.Open()
                Loomcmd = New MySqlCommand(LoomString1, LoomConnection)
                LoomReader = Loomcmd.ExecuteReader(CommandBehavior.CloseConnection)

                While LoomReader.Read
                    record = CType(LoomReader, IDataRecord)

                    sheet1(6, i).Value = Convert.ToInt32(record(0))

                    'sheet1(5, 9).Value = obj
                    i = i + 2

                End While

                LoomConnection.Close()
                LoomConnection.Dispose()

            Next


            Return xlbook


        End Function

        Public Function injectLoomshiftYds(ByRef xlbook As C1XLBook, ByVal shift As Integer, ByVal startrow As Integer, ByVal startcol As Integer, ByVal LoomNo As String) As C1XLBook

            Dim record As IDataReader
            Dim sheet1 As XLSheet

            Dim j As Integer = 0

            Dim hourarray() As Integer
            Dim starthour As Integer
            Dim endhour As Integer

            hourarray = getdefecthours(shift)

            'set start and end hour from hour array
            If hourarray.Length = 2 Then
                starthour = hourarray(0)
                endhour = hourarray(1)
            Else
                starthour = 0
                endhour = 8
            End If

            sheet1 = xlbook.Sheets(Sheetselect)


            LoomString1 = "SELECT sum(RollYds) FROM WeavingDefect WHERE Machine = 'STT_GREIGE1' AND LoomNo = '" & LoomNo & "' and DefectTime > '" & pfromdate & "' and DefectTime < '" & ptodate & "' and HOUR(DefectTime) > " & starthour & " and HOUR(DefectTime) < " & endhour & ""
            LoomConnection = New MySqlConnection("server=STT-SERVER1-PC;port=3310;user id=akab1;password=hebron;database=Production")
            LoomConnection.Open()
            Loomcmd = New MySqlCommand(LoomString1, LoomConnection)
            LoomReader = Loomcmd.ExecuteReader(CommandBehavior.CloseConnection)

            While LoomReader.Read
                record = CType(LoomReader, IDataRecord)

                If IsDBNull(record(0)) = True Then
                    sheet1(startrow, startcol).Value = 0
                Else
                    sheet1(startrow, startcol).Value = Convert.ToInt32(record(0))
                End If

            End While

            LoomConnection.Close()
            LoomConnection.Dispose()



            Return xlbook


        End Function


        Public Function injectLoomDefects(ByRef xlbook As C1XLBook, ByVal fromdate As String, ByVal todate As String, ByVal startrow As Integer, ByVal startcol As Integer, ByVal LoomNo As String, ByVal shift As Integer) As C1XLBook

            Dim record As IDataReader
            'Dim LoomArray() As String
            Dim sheet1 As XLSheet
            Dim hourarray() As Integer

            Dim j As Integer = 0
            Dim i As Integer = 1
            Dim starthour As Integer
            Dim endhour As Integer


            sheet1 = xlbook.Sheets(Sheetselect)
            hourarray = getdefecthours(shift)

            'set start and end hour from hour array
            If hourarray.Length = 2 Then
                starthour = hourarray(0)
                endhour = hourarray(1)
            Else
                starthour = 0
                endhour = 8
            End If
            Using LoomConnection As MySqlConnection = New MySqlConnection("server=STT-SERVER1-PC;port=3310;user id=akab1;password=hebron;database=Production")


                For j = 0 To weavedefects.Length - 1



                    'Dim stringtest As String = LoomArray(j)

                    Try

                        LoomString1 = "SELECT COUNT(Defect) from weavingdefect where Machine = 'STT_GREIGE1' and DefectTime > '" & pfromdate & "' and DefectTime < '" & ptodate & "' and Defect = '" & weavedefects(j) & "' and LoomNo = '" & LoomNo & "' and HOUR(DefectTime) > " & starthour & " and HOUR(DefectTime) < " & endhour & ""
                        'LoomConnection = New MySqlConnection("server=STT-SERVER1-PC;port=3310;user id=akab1;password=hebron;database=Production")
                        LoomConnection.Open()
                        Loomcmd = New MySqlCommand(LoomString1, LoomConnection)
                        LoomReader = Loomcmd.ExecuteReader(CommandBehavior.CloseConnection)



                    Catch ex As Exception
                        MsgBox(ex.Message)
                    End Try
                    While LoomReader.Read
                        record = CType(LoomReader, IDataRecord)

                        sheet1(j + startrow, startcol).Value = Convert.ToInt32(record(0))

                        'sheet1(5, 9).Value = obj
                        ' i = i + 2

                    End While

                    LoomConnection.Close()
                    'LoomConnection.Dispose()



                Next

            End Using
            Return xlbook


        End Function

        Public Function getdefecthours(ByVal shift As Integer) As Array

            Dim shifthours As New List(Of Integer)

            Select Case shift

                Case 1
                    shifthours.Add(0)
                    shifthours.Add(8)
                Case 2
                    shifthours.Add(8)
                    shifthours.Add(16)
                Case 3
                    shifthours.Add(16)
                    shifthours.Add(24)

            End Select

            Return shifthours.ToArray()

        End Function


    End Class


End Namespace