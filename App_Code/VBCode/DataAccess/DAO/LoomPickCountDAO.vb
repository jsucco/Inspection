Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Data
Imports System.Globalization
Imports System.Web.Script.Serialization
Imports C1.C1Excel
Imports System.Drawing

Namespace core

    Public Class LoomPickCountDAO
        Public Property _DAOFactory As New DAOFactory
        Public Property DL As New dlayer

        Public Shared returnint As Integer = 99

        Private _ErrorMsg As String
        Private Util As New Utilities
        Private DAOF As New DAOFactory

        Public Property ErrorMsg() As String              'bms 4/24/06
            Get                                     'bms 4/24/06
                Return _ErrorMsg                    'bms 4/24/06
            End Get                                 'bms 4/24/06
            Set(ByVal Value As String)                        'bms 4/24/06
                _ErrorMsg = Value                   'bms 4/24/06
            End Set                                 'bms 4/24/06
        End Property

        Dim PickCountstring As String
        Dim PickCountReader As SqlDataReader
        Dim PickCountcmdBuilder As SqlCommandBuilder
        Dim PickCountcmd As SqlCommand
        Dim PickCountConnection As SqlConnection


        Public Shared sheetselect As String

        Public Function GetLSGrid() As String

            Dim returnstring As String
            Dim LoomStops As New List(Of LoomStopStats)()
            Dim jser As New JavaScriptSerializer
            Dim result As Boolean
            Dim sqlstring As String

            sqlstring = "select Stops_PerShift, Disturbance_PerShift, FillStop_PerShift, WarpStop_PerShift, PieceLength_PerShift, Host_PerShift from dbo.Pres_LoomStats order by id asc"

            LoomStops = _DAOFactory.getLoomStats(sqlstring, 1)
            returnstring = jser.Serialize(LoomStops.ToArray())

            Return returnstring

        End Function

        Public Function GetPCGrid(ByVal LoomNo As Integer) As Array

            Dim returnstring As Array
            Dim LoomStops As New List(Of LoomPickStats)()
            Dim jser As New JavaScriptSerializer
            Dim result As Boolean
            Dim sqlstring As String

            '   sqlstring = "select PickCount_Curr, PickCount_ShiftAvg, PickCount_Max from dbo.Pres_LoomStats where LoomNo = " + Convert.ToString(LoomNo) + " order by id asc"
            sqlstring = "select cast(isnull(Stops_PerShift/(nullif(PickCount_Sum,0)/1000000),0) as decimal(10,2)), PickCount_ShiftAvg, PickCount_Max from dbo.Pres_LoomStats where LoomNo = " + Convert.ToString(LoomNo) + " order by id asc"
            
            LoomStops = _DAOFactory.getLoomGridPicks(sqlstring, 1)
            returnstring = LoomStops.ToArray()

            Return returnstring
          
        End Function

        Public Function GetCurrPC() As String
            Dim returnstring As String
            Dim LoomPicks As New List(Of LoomPicksCurr)()
            Dim jser As New JavaScriptSerializer
            Dim result As Boolean
            Dim sqlstring As String

            sqlstring = "select LoomNo, PickCount_Curr, Updated_Timestamp from dbo.Pres_LoomStats"

            LoomPicks = _DAOFactory.getCurrLoomPicks(sqlstring, 1)
            returnstring = jser.Serialize(LoomPicks.ToArray())
             
            Return returnstring

        End Function

        Public Function GetLastStop(ByVal LoomNo As Integer) As String

            Dim returnstring As String
            Dim returnlist As New List(Of LoomStopsCurr)()
            Dim jser As New JavaScriptSerializer

            Dim sqlcommand As New SqlCommand

            Using connection As New SqlConnection(DL.APRConnectionString(1))

                sqlcommand.CommandText = "GetLastLoomStop"
                sqlcommand.CommandType = CommandType.StoredProcedure
                sqlcommand.Connection = connection

                sqlcommand.Parameters.Add(_DAOFactory.Getparameter("@LOOMNO", DbType.Int32))
                sqlcommand.Parameters("@LOOMNO").Value = LoomNo
                sqlcommand.Connection.Open()
                Dim reader As SqlDataReader = sqlcommand.ExecuteReader()
                Dim record As IDataRecord

                While reader.Read()
                    record = CType(reader, IDataRecord)
                    Dim test As Object = record(0)
                    returnlist.Add(New LoomStopsCurr With {.STOPTYPE = Convert.ToString(record(0)), .Timestamp = Convert.ToString(record(1))})
                End While

                If returnlist.Count = 0 And connection.State = ConnectionState.Open Then
                    Dim manualstop As String = "Manual"
                    returnlist.Add(New LoomStopsCurr With {.STOPTYPE = manualstop, .Timestamp = Convert.ToString(DateTime.Now)})
                End If

                returnstring = jser.Serialize(returnlist.ToArray())

            End Using

            If Not returnstring Is Nothing Then
                Return returnstring
            Else
                Return False
            End If


        End Function

        Public Function LoadExportData(ByRef xlbook As C1XLBook, ByVal fromdate As String, ByVal todate As String) As C1XLBook

            Dim sqlstring As String = "select Dornier2Disturbance, Dornier2FillStop, Dornier2Host, Dornier2PieceLength, Dornier2WarpStop, Dornier3Disturbance, Dornier3FillStop, Dornier3Host, Dornier3PieceLength, Dornier3WarpStop, Dornier4Disturbance, Dornier4FillStop, Dornier4Host, Dornier4PieceLength, Dornier4WarpStop, Dornier5Disturbance, Dornier5FillStop, Dornier5Host, Dornier5PieceLength, Dornier5WarpStop, Timestamp from dbo.BR_LoomStopsFrm where Timestamp is not null and Timestamp >= '" & fromdate & "' and Timestamp <= '" & todate & "' order by frm_id desc"
            Dim LoomReader As SqlDataReader
            Dim record As IDataRecord
            Dim reclength As Integer
            Dim sheet1 As XLSheet
            Dim i As Integer
            Dim TPF As New TexpaFlat
            Dim ColArray() As String = {"Dorn2Disturbance", "Dorn2Fill", "Dorn2Host", "Dorn2PieceLength", "Dorn2Warp", "Dorn3Disturbance", "Dorn3Fill", "Dorn3Host", "Dorn3PieceLength", "Dorn3Warp", "Dorn4Disturbance", "Dorn4Fill", "Dorn4Host", "Dorn4PieceLength", "Dorn4Warp", "Dorn5Disturbance", "Dorn5Fill", "Dorn5Host", "Dorn5PieceLength", "Dorn5Warp", "Timestamp"}
            Dim SS1Style As XLStyle = New XLStyle(xlbook)

            SS1Style.Font = New Font("Calibri", 11, FontStyle.Bold)
            SS1Style.BackColor = Color.Yellow
            SS1Style.SetBorderStyle(XLLineStyleEnum.Thick)
            SS1Style.Format = "MM/DD/YYYY hh:mm"


            sheet1 = xlbook.Sheets(sheetselect)

            'If _DAOFactory.GetTRNReader(sqlstring, 1) = True Then

            '    LoomReader = DAOFactory.coreReader
            Using con As New SqlConnection(DL.APRConnectionString(1))

                Dim cmd As New SqlCommand(sqlstring, con)
                LoomReader = cmd.ExecuteReader(CommandBehavior.CloseConnection)

                sheet1(0, 20).Value = ColArray(20)
                While LoomReader.Read
                    record = CType(LoomReader, IDataRecord)
                    reclength = record.FieldCount

                    Try
                        For x = 0 To 19
                            If i = 0 Then
                                sheet1(i, x).Value = ColArray(x)
                            End If
                            sheet1(i + 1, x).Value = Convert.ToInt32(record(x))
                        Next


                        sheet1(i + 1, 20).Value = Convert.ToDateTime(record(20))
                        sheet1(i + 1, 20).Style = SS1Style


                    Catch ex As Exception
                        MsgBox(ex.Message)
                    End Try

                    i = i + 1

                End While


                Return xlbook

            End Using


        End Function

        Public Function LoadExportLoomData(ByRef xlbook As C1XLBook, ByVal fromdate As String, ByVal todate As String, ByVal Interval As Integer) As C1XLBook

            Dim sqlstring As String = getLoomPickSqlstring(Interval, fromdate, todate)
            Dim LoomReader As SqlDataReader
            Dim record As IDataRecord
            Dim reclength As Integer
            Dim sheet1 As XLSheet
            Dim i As Integer
            Dim TPF As New TexpaFlat
            Dim ColArray() As String = {"Dorn2PickCount", "Dorn3PickCount", "Dorn4PickCount", "Dorn5PickCount", "Timestamp"}
            Dim SS1Style As XLStyle = New XLStyle(xlbook)

            SS1Style.Font = New Font("Calibri", 11, FontStyle.Bold)
            SS1Style.BackColor = Color.Yellow
            SS1Style.SetBorderStyle(XLLineStyleEnum.Thick)
            SS1Style.Format = "MM/DD/YYYY hh:mm"


            sheet1 = xlbook.Sheets(sheetselect)

            Using con As New SqlConnection(DL.APRConnectionString(1))

                Dim cmd As New SqlCommand(sqlstring, con)
                LoomReader = cmd.ExecuteReader(CommandBehavior.CloseConnection)

                sheet1(0, 4).Value = ColArray(4)
                While LoomReader.Read
                    record = CType(LoomReader, IDataRecord)
                    reclength = record.FieldCount

                    Try
                        For x = 0 To 3
                            If i = 0 Then
                                sheet1(i, x).Value = ColArray(x)
                            End If
                            sheet1(i + 1, x).Value = Convert.ToDecimal(record(x))
                        Next


                        sheet1(i + 1, 4).Value = Convert.ToDateTime(record(4))
                        sheet1(i + 1, 4).Style = SS1Style


                    Catch ex As Exception
                        MsgBox(ex.Message)
                    End Try

                    i = i + 1

                End While


                Return xlbook
            End Using

        End Function

        Private Function getLoomPickSqlstring(ByVal id As Integer, ByVal fromdate As String, ByVal todate As String) As String
            Dim sqlstring As String

            Select Case id
                Case 1
                    sqlstring = "select Dornier2PickCounter, Dornier3PickCounter, Dornier4PickCounter, Dornier5PickCounter, Timestamp from dbo.BR_LoomPickCountFrm_1 where Timestamp is not null and Timestamp >= '" & fromdate & "' and Timestamp <= '" & todate & "' order by Timestamp asc"
                Case 2
                    sqlstring = "select * from (" & vbCrLf &
                                    "select sum(Dornier2PickCounter) as Dorn2, sum(Dornier3PickCounter) as Dorn3, sum(Dornier4PickCounter) as Dorn4, sum(Dornier5PickCounter) as Dorn5, cast(cast(DATEPART(yy, Timestamp) as varchar) + '-' + cast(DATEPART(mm, Timestamp) as varchar) + '-' + cast(DATEPART(dd, Timestamp) as varchar) + ' ' + cast(DATEPART(hh, Timestamp) as varchar) + ':00:00' as datetime) as newdt from dbo.BR_LoomPickCountFrm_1 where Timestamp >= '" & fromdate & "' and Timestamp <= '" & todate & "' Group by DATEPART(hh, Timestamp), DATEPART(dd, Timestamp), DATEPART(mm, Timestamp), DATEPART(yy, Timestamp) " & vbCrLf &
                                    ") t where newdt is not null order By newdt asc"
                Case 3
                    sqlstring = "select * from (" & vbCrLf &
                                    "select sum(Dornier2PickCounter) as Dorn2, sum(Dornier3PickCounter) as Dorn3, sum(Dornier4PickCounter) as Dorn4, sum(Dornier5PickCounter) as Dorn5, cast(cast(DATEPART(yy, Timestamp) as varchar) + '-' + cast(DATEPART(mm, Timestamp) as varchar) + '-' + cast(DATEPART(dd, Timestamp) as varchar) + ' 00:00:00' as datetime) as newdt from dbo.BR_LoomPickCountFrm_1 where Timestamp >= '" & fromdate & "' and Timestamp <= '" & todate & "' Group by DATEPART(dd, Timestamp), DATEPART(mm, Timestamp), DATEPART(yy, Timestamp) " & vbCrLf &
                                    ") t where newdt is not null order By newdt asc"
                Case 4
                    sqlstring = "select * from (" & vbCrLf &
                                    "select sum(Dornier2PickCounter) as Dorn2, sum(Dornier3PickCounter) as Dorn3, sum(Dornier4PickCounter) as Dorn4, sum(Dornier5PickCounter) as Dorn5, cast(cast(DATEPART(yy, Timestamp) as varchar) + '-' + cast(DATEPART(mm, Timestamp) as varchar) + '-1 00:00:00' as datetime) as newdt from dbo.BR_LoomPickCountFrm_1 where Timestamp >= '" & fromdate & "' and Timestamp <= '" & todate & "' Group by DATEPART(mm, Timestamp), DATEPART(yy, Timestamp) " & vbCrLf &
                                    ") t where newdt is not null order By newdt asc"
                Case Else
                    sqlstring = "select Dornier2PickCounter, Dornier3PickCounter, Dornier4PickCounter, Dornier5PickCounter, Timestamp from dbo.BR_LoomPickCountFrm_1 where Timestamp is not null and Timestamp >= '2014-08-18 00:00:00' and Timestamp <= '2014-08-21 23:59:59' order by Timestamp asc"
            End Select

            Return sqlstring

        End Function



    End Class

End Namespace

