Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Data
Imports System.Globalization
Imports System.Web.Script.Serialization
Imports MySql.Data.MySqlClient

Namespace core


    Public Class DisplayBoardDAO
        Public Property _DAOFactory As New DAOFactory
        Public Property DL As New dlayer

        Private _ErrorMsg As String
        Private Util As New Utilities

        Dim spcString1 As String
        Dim spcConnection As MySqlConnection
        Dim spcReader As MySqlDataReader
        Dim spcNewRow As DataRow
        Dim spccmdBuilder As MySqlCommandBuilder
        Dim spccmd As MySqlCommand

        Public Property ErrorMsg() As String              'bms 4/24/06
            Get                                     'bms 4/24/06
                Return _ErrorMsg                    'bms 4/24/06
            End Get                                 'bms 4/24/06
            Set(ByVal Value As String)                        'bms 4/24/06
                _ErrorMsg = Value                   'bms 4/24/06
            End Set                                 'bms 4/24/06
        End Property

        Public Function GetDSBSlide(ByVal ShowId As Integer, ByVal SlideOrder As Integer) As String

            Dim returnstring As String
            Dim DashBoard As New List(Of DashBoardSchedule)()
            Dim jser As New JavaScriptSerializer
            Dim result As Boolean
            Dim sqlstring As String

            sqlstring = "select url, ClipTop, ClipRight, Clipbutton, ClipLeft, MainPlateWidth, MainPlateHeight, MainPlateScale, InnerDivTop, InnerDivLeft, SlideOrder, Type, TransTime, TableSourceId from dbo.DashboardSchedule where ShowId = " & Convert.ToString(ShowId) & " and SlideOrder = " & Convert.ToString(SlideOrder)

            DashBoard = _DAOFactory.getDashSchedule(sqlstring, 2)
            returnstring = jser.Serialize(DashBoard.ToArray())

            Return returnstring

        End Function

        Public Function GetUtilityChart() As String

            Dim returnstring As String
            Dim DashBoard As New List(Of Chartdatavalues)()
            Dim jser As New JavaScriptSerializer
            Dim result As Boolean
            Dim sqlstring As String

            ' sqlstring = "SELECT TOP (720) Timestamp, Mains_Power, Mains_Cold_Water FROM DI_Main_1A_Cd ORDER BY ID ASC"
            sqlstring = "SELECT Timestamp, Mains_Power, Mains_Cold_Water FROM (SELECT TOP (720) Timestamp, Mains_Power, Mains_Cold_Water FROM DI_Main_1A_Cd ORDER BY ID DESC) AS derivedtbl_1 ORDER BY Timestamp"

            DashBoard = _DAOFactory.getUtilityData(sqlstring)
            returnstring = jser.Serialize(DashBoard.ToArray())

            Return returnstring

        End Function

        Public Function GetMainPlateImage(ByVal cmd As SqlCommand) As DataTable

            Dim dt As New DataTable
            Dim dl As New dlayer

            Dim strConnString As String = dl.APRConnectionString(2)
            Dim con As New SqlConnection(strConnString)
            Dim sda As New SqlDataAdapter
            cmd.CommandType = CommandType.Text
            cmd.Connection = con
            Try
                con.Open()
                sda.SelectCommand = cmd
                sda.Fill(dt)
                Return dt
            Catch ex As Exception
                Return Nothing
            Finally
                con.Close()
                sda.Dispose()
                con.Dispose()
            End Try
        End Function

        Public Function GetSPCJobProduction() As List(Of SPCJobProduction)
            Dim record As IDataReader
            Dim spclist As New List(Of SPCJobProduction)
            Dim jser As New JavaScriptSerializer

            spcString1 = "select Machine, JobNo, StartTime, CutLengthSpec, JobYds, JobSheets, Efficiency, AvgSheetsPerHour from jobproduction order by JobProductionID desc LIMIT 10"
            spcConnection = New MySqlConnection("server=STT-SERVER1-PC;port=3310;user id=akab1;password=hebron;database=Production")
            spcConnection.Open()
            spccmd = New MySqlCommand(spcString1, spcConnection)
            spcReader = spccmd.ExecuteReader(CommandBehavior.CloseConnection)

            While spcReader.Read
                record = CType(spcReader, IDataRecord)

                Try
                    spclist.Add(New SPCJobProduction With {.Machine = Convert.ToString(record(0)), .JobNo = Convert.ToInt32(record(1)), .StartTime = Convert.ToString(record(2)), .CutLengthSpec = Convert.ToDecimal(record(3)), .JobYds = Convert.ToDecimal(record(4)), .JobSheets = Convert.ToString(record(5)), .Efficiency = Convert.ToDecimal(record(6)), .AvgSheetsPerHour = Convert.ToDecimal(record(7))})
                Catch ex As Exception

                End Try


            End While

            spcConnection.Close()
            spcConnection.Dispose()

            Return spclist

        End Function

    End Class


End Namespace
