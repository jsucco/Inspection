Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Data
Imports System.Globalization
Imports System.Data.Odbc
Imports System.Web.Script.Serialization


Namespace core


    Public Class InspectionUtilityDAO
        Inherits System.Web.UI.Page
        Private Property _DAOFactory As New DAOFactory
        Private Property DL As New dlayer
        Private Property util As New Utilities
        Private Property bmap As New BMappers(Of SPCInspection.TemplateCollection)
        Private Property bmap_1 As New BMappers(Of SPCInspection.ButtonTemplate)
        Private AprManagerDb As String = "AprManager"
        Public Shared ProductSpecsLastCachedDT As DateTime
        Public Shared ProductSpecsLastUpdatedDT As DateTime
        Public LocationNames As List(Of Locationarray)
        Public LocationsSelectors As List(Of selector2array)
        Public Property NEWALLID As Integer
        Dim PickCountstring As String
        Dim PickCountReader As SqlDataReader
        Dim PickCountcmdBuilder As SqlCommandBuilder
        Dim PickCountcmd As SqlCommand
        Dim PickCountConnection As SqlConnection
        Dim result As Boolean
        Dim ExecuteConnection As New SqlConnection(ConfigurationManager.ConnectionStrings("MyDB").ConnectionString)
        Dim Connection As New SqlConnection(ConfigurationManager.ConnectionStrings("MyDB").ConnectionString) 'Connection to MyDB
        'Connection to MyDB
        Dim Command As New SqlCommand

        Dim DR As SqlDataReader

        Function ExecuteSQL(ByVal SQL As String, ByVal Log As String) As Object
            'This function executes and logs all of our custom SQL statements.

            Dim Outcome As String
            Command.Connection = ExecuteConnection
            ExecuteConnection.Open()
            Dim ErrorString As String = ""
            Try
                'Attemp to execute SQL statement passed into function
                Command.CommandText = SQL
                Command.ExecuteNonQuery()
                Outcome = "Successful"
            Catch ex As Exception
                'If SQL statement fails log error and trap exception
                ErrorString += " " & ex.ToString
                Outcome = "Failed"
                'LogEvent(1, "EXECUTE_SQL", "1 - Failure Executing SQL Statement", SQL, 1, Replace(ex.ToString, "'", "''"))
            End Try
            If Log = 1 Then
                Try
                    'Log our SQL attempt and any errors that may have been thrown
                    Command.CommandText = "insert into MY_sql_execution(MYSQL_Statement, MYSQL_Error, MYSQL_Result, MYSQL_Record, MYSQL_Form, MYSQL_UID, MYSQL_Login, MYSQL_Fullname, MYSQL_IP, MYSQL_SessionID) Select "
                    Command.CommandText += "'" + Replace(SQL, "'", "''") + "' as 'MYSQL_Statement', "
                    Command.CommandText += "'" + Replace(ErrorString, "'", "''") + "' as 'MYSQL_Error',"
                    Command.CommandText += "'" + Outcome + "' as 'MYSQL_Result',"
                    Command.CommandText += "'" + Request.QueryString("JID") + "' as 'MYSQL_Record',"
                    Command.CommandText += "'LandingPage.aspx' as 'Source',"
                    Command.CommandText += "'" + Convert.ToString(0) + "' as 'MYSQL_UID',"
                    Command.CommandText += "'" + "" + "' as 'MYSQL_Login',"
                    Command.CommandText += "'" + "None" + "' as 'MYSQL_Fullname',"
                    Command.CommandText += "'" + Request.UserHostAddress + "' as 'MYSQL_IP', "
                    Command.CommandText += "'" + Session.SessionID + "' as 'MYSQL_SessionID'"
                    Command.ExecuteNonQuery()
                Catch exc As Exception
                End Try
            End If
            ExecuteConnection.Close()
            Return Outcome
        End Function

        Public Sub New()
            Dim PointToTest = System.Web.Configuration.WebConfigurationManager.AppSettings("PointToTest")

            If PointToTest.ToUpper() = "TRUE" Then
                AprManagerDb = System.Web.Configuration.WebConfigurationManager.AppSettings("AprManagerTestDb")
            End If
        End Sub
        Public Function setIncrement(ByVal rowId As Integer, ByVal IncrementAmount As String) As Integer
            Dim Outcome As String = ""
            Dim SQL As String = "UPDATE dbo.InspectionJobSummary SET TotalInspectedItems= TotalInspectedItems+" & IncrementAmount & " WHERE id =  " & rowId.ToString()
            Outcome = ExecuteSQL(SQL, 1)
            If Outcome = "Successful" Then
                Return True
            End If
            Return False
        End Function
        Public Function RecordSource(ByVal ID As Integer, ByVal MOP As String, ByVal LOC As String, ByVal DID As Integer) As Integer
            Dim Outcome As String = "" '

            Dim SQL As String = "Insert Into dbo.InspectionJobSummarySupplement Values (" & ID & ",'" & MOP & "','" & LOC & "', CAST('" & DateTime.Now & "' AS DATETIME), " & DID & ")"
            Outcome = ExecuteSQL(SQL, 1)
            If Outcome = "Successful" Then
                Return True
            End If
            Return False
        End Function
        Public Function EditCompletedProperties(ByVal rowId As Integer) As Integer
            Dim Outcome As String = ""
            Dim retval As Integer = -1
            Dim SQL As String = "UPDATE dbo.InspectionJobSummary SET Technical_PassFail = NULL, Technical_PassFail_TimeStamp = NULL, UserConfirm_PassFail = NULL, UserConfirm_PassFail_TimeStamp = NULL, Inspection_Finished = NULL  WHERE id =  " & rowId.ToString()
            Outcome = ExecuteSQL(SQL, 1)
            If Outcome = "Successful" Then
                Return 1
            End If
            Return False
        End Function
        Public Function setISRow(ByVal rowId As Integer, ByVal ItemFailCount As Integer, ByVal TotalInspectedItems As Integer, ByVal TechnicalPassFail As Boolean, ByVal ItemPassCount As Integer, ByVal MajorsCount As Integer, ByVal MinorsCount As Integer, ByVal RepairsCount As Integer, ByVal ScrapCount As Integer, ByVal UserConfirm_PassFail As Boolean, ByVal UserConfirm_PassFail_Timestamp As DateTime, ByVal Inspection_Finished As DateTime, ByVal JobType As String, ByVal Comments As String) As Integer
            Dim Outcome As String = ""
            Dim retval As Integer = -1
            Dim SQL As String = "UPDATE dbo.InspectionJobSummary Set ItemFailCount= " & ItemFailCount & ",TotalInspectedItems = " & TotalInspectedItems & ",Technical_PassFail = '" & TechnicalPassFail & "',ItemPassCount = " & ItemPassCount & ",MajorsCount = " & MajorsCount & ",MinorsCount = " & MinorsCount & ",RepairsCount = " & RepairsCount & ",ScrapCount = " & ScrapCount & ",UserConfirm_PassFail = '" & UserConfirm_PassFail & "',UserConfirm_PassFail_TimeStamp = CAST('" & UserConfirm_PassFail_Timestamp & "' AS DATETIME),Inspection_Finished = CAST('" & Inspection_Finished & "' AS DATETIME),JobType = '" & JobType & "',Comments = '" & Comments & "' WHERE id =  " & rowId.ToString()
            Outcome = ExecuteSQL(SQL, 1)
            If Outcome = "Successful" Then
                Return 1
            End If
            Return False
        End Function
        Public Function getComment(ByVal rowId As Integer) As String
            Dim retval As String = "Error"
            Dim SQL As String = "SELECT Comments as COMM From dbo.InspectionJobSummary WHERE id=" & rowId.ToString()
            Command.CommandType = CommandType.Text 'sets the type of the sql
            Command.Connection = Connection 'sets the connection of our sql command to MyDB
            Command.CommandText = SQL 'sets the statement that executes at the data source to our string
            Connection.Open() 'opens the connction
            DR = Command.ExecuteReader 'sends the command text to the connection and builds tthe SqlDataReader
            If DR.HasRows = True Then 'Check whether the SqlDataReader has 1 or more rows
                DR.Read() 'The default position of the SqlDataReader is before the first record. Therefore, you must call Read to begin accessing any data.  The reader has moved down to the first row.
                'Response.Write(DR("Cust_Name")) 'Writes a string to an HTTP response output stream.  In this case, just Cust_Name

                retval = Convert.ToString(DR("COMM")) '

            Else
                Return retval


            End If

            Connection.Close() 'closes the connection
            DR.Close() 'closes the reader
            Return retval
        End Function
        Public Function setComment(ByVal rowId As Integer, ByVal Comment As String) As Integer
            Dim Outcome As String = ""
            Dim SQL As String = "UPDATE dbo.InspectionJobSummary SET Comments='" & Comment & "' WHERE id =  " & rowId.ToString()
            Outcome = ExecuteSQL(SQL, 1)
            If Outcome = "Successful" Then
                Return True
            End If
            Return False
        End Function
        Public Function setYards(ByVal shiftId As Integer, ByVal Yards As Integer) As Integer
            Dim Outcome As String = ""
            Dim SQL As String = "UPDATE dbo.WeaverProduction SET Yards = " + Yards.ToString() + " WHERE  ShiftId = " & shiftId.ToString()
            Outcome = ExecuteSQL(SQL, 1)
            If Outcome = "Successful" Then
                Return True
            End If
            Return False
        End Function
        Public Function setShiftEnd(ByVal rowId As Integer, ByVal Comment As String) As Boolean
            Dim Outcome As String = ""
            Dim SQL As String = "UPDATE dbo.WeaverShifts SET Finish = getdate(), Comments='" & Comment & "' WHERE  Id = " & rowId.ToString()
            Outcome = ExecuteSQL(SQL, 1)
            If Outcome = "Successful" Then
                Return True
            End If
            Return False
        End Function

        Public Function getLatestDefectId() As Integer
            Dim retval As Integer = -1
            Dim SQL As String = "SELECT MAX(DefectId) AS MAXDI FROM dbo.DefectMaster"
            Command.CommandType = CommandType.Text 'sets the type of the sql
            Command.Connection = Connection 'sets the connection of our sql command to MyDB
            Command.CommandText = SQL 'sets the statement that executes at the data source to our string
            Connection.Open() 'opens the connction
            DR = Command.ExecuteReader 'sends the command text to the connection and builds tthe SqlDataReader
            If DR.HasRows = True Then 'Check whether the SqlDataReader has 1 or more rows
                DR.Read() 'The default position of the SqlDataReader is before the first record. Therefore, you must call Read to begin accessing any data.  The reader has moved down to the first row.
                'Response.Write(DR("Cust_Name")) 'Writes a string to an HTTP response output stream.  In this case, just Cust_Name

                retval = Convert.ToInt32(DR("MAXDI")) '

            Else
                Return retval


            End If

            Connection.Close() 'closes the connection
            DR.Close() 'closes the reader
            Return retval
        End Function
        Public Function getIncrement(ByVal rowId As Integer) As Integer
            Dim retval As Integer = -1
            Dim SQL As String = "SELECT TotalInspectedItems as INSITEMS From dbo.InspectionJobSummary WHERE id=" & rowId.ToString()
            Command.CommandType = CommandType.Text 'sets the type of the sql
            Command.Connection = Connection 'sets the connection of our sql command to MyDB
            Command.CommandText = SQL 'sets the statement that executes at the data source to our string
            Connection.Open() 'opens the connction
            DR = Command.ExecuteReader 'sends the command text to the connection and builds tthe SqlDataReader
            If DR.HasRows = True Then 'Check whether the SqlDataReader has 1 or more rows
                DR.Read() 'The default position of the SqlDataReader is before the first record. Therefore, you must call Read to begin accessing any data.  The reader has moved down to the first row.
                'Response.Write(DR("Cust_Name")) 'Writes a string to an HTTP response output stream.  In this case, just Cust_Name

                retval = Convert.ToInt32(DR("INSITEMS")) '

            Else
                Return retval


            End If

            Connection.Close() 'closes the connection
            DR.Close() 'closes the reader
            Return retval
        End Function
        Public Function GetMonthlyDefectTotal(ByVal CID As Integer, ByVal WS As String) As Integer
            Dim retval As Integer = 0
            Dim DR2 As SqlDataReader
            Dim Command2 As New SqlCommand
            Dim Connection2 As New SqlConnection(ConfigurationManager.ConnectionStrings("MyDB").ConnectionString)
            Dim SQL As String = "Select SUM(MajorsCount+MinorsCount+RepairsCount+ScrapCount) AS TOTAL from dbo.InspectionJobSummaryYearly WHERE CID = " & CID & WS & " AND Inspection_Finished >= DATEADD(month,-1,GETDATE())"
            Command2.CommandType = CommandType.Text 'sets the type of the sql
            Command2.Connection = Connection2 'sets the connection of our sql command to MyDB
            Command2.CommandText = SQL 'sets the statement that executes at the data source to our string
            If (Connection2.State = ConnectionState.Closed) Then
                Connection2.Open()
            End If
            DR2 = Command2.ExecuteReader 'sends the command text to the connection and builds tthe SqlDataReader
            If DR2.HasRows = True Then 'Check whether the SqlDataReader has 1 or more rows
                DR2.Read() 'The default position of the SqlDataReader is before the first record. Therefore, you must call Read to begin accessing any data.  The reader has moved down to the first row.
                'Response.Write(DR("Cust_Name")) 'Writes a string to an HTTP response output stream.  In this case, just Cust_Name
                If Not DR2.IsDBNull(DR2.GetOrdinal("Total")) Then
                    retval = Convert.ToInt32(DR2("Total"))
                End If


            Else
                Return retval


            End If

            Connection2.Close() 'closes the connection
            DR2.Close() 'closes the reader
            Return retval
        End Function
        Public Function GetGlobalMonthlyDefectTotal(ByVal WS As String) As Integer
            Dim retval As Integer = 0
            Dim DR2 As SqlDataReader
            Dim Command2 As New SqlCommand
            Dim Connection2 As New SqlConnection(ConfigurationManager.ConnectionStrings("MyDB").ConnectionString)
            Dim SQL As String = "Select SUM(MajorsCount+MinorsCount+RepairsCount+ScrapCount) AS TOTAL from dbo.InspectionJobSummaryYearly " & WS & " AND Inspection_Finished >= DATEADD(month,-1,GETDATE())"
            Command2.CommandType = CommandType.Text 'sets the type of the sql
            Command2.Connection = Connection2 'sets the connection of our sql command to MyDB
            Command2.CommandText = SQL 'sets the statement that executes at the data source to our string
            If (Connection2.State = ConnectionState.Closed) Then
                Connection2.Open()
            End If
            DR2 = Command2.ExecuteReader 'sends the command text to the connection and builds tthe SqlDataReader
            If DR2.HasRows = True Then 'Check whether the SqlDataReader has 1 or more rows
                DR2.Read() 'The default position of the SqlDataReader is before the first record. Therefore, you must call Read to begin accessing any data.  The reader has moved down to the first row.
                'Response.Write(DR("Cust_Name")) 'Writes a string to an HTTP response output stream.  In this case, just Cust_Name
                If Not DR2.IsDBNull(DR2.GetOrdinal("Total")) Then
                    retval = Convert.ToInt32(DR2("Total"))
                End If


            Else
                Return retval


            End If

            Connection2.Close() 'closes the connection
            DR2.Close() 'closes the reader
            Return retval
        End Function
        Public Function GetYearlyDefectTotal(ByVal CID As Integer, ByVal WS As String) As Integer
            Dim retval As Integer = 0
            Dim DR2 As SqlDataReader
            Dim Command2 As New SqlCommand
            Dim Connection2 As New SqlConnection(ConfigurationManager.ConnectionStrings("MyDB").ConnectionString)
            Dim SQL As String = "Select SUM(MajorsCount+MinorsCount+RepairsCount+ScrapCount) AS TOTAL from dbo.InspectionJobSummaryYearly WHERE CID = " & CID & WS
            Command2.CommandType = CommandType.Text 'sets the type of the sql
            Command2.Connection = Connection2 'sets the connection of our sql command to MyDB
            Command2.CommandText = SQL 'sets the statement that executes at the data source to our string
            If (Connection2.State = ConnectionState.Closed) Then
                Connection2.Open()
            End If
            DR2 = Command2.ExecuteReader 'sends the command text to the connection and builds tthe SqlDataReader
            If DR2.HasRows = True Then 'Check whether the SqlDataReader has 1 or more rows
                DR2.Read() 'The default position of the SqlDataReader is before the first record. Therefore, you must call Read to begin accessing any data.  The reader has moved down to the first row.
                'Response.Write(DR("Cust_Name")) 'Writes a string to an HTTP response output stream.  In this case, just Cust_Name

                If Not DR2.IsDBNull(DR2.GetOrdinal("Total")) Then
                    retval = Convert.ToInt32(DR2("Total"))
                End If

            Else
                Return retval


            End If

            Connection2.Close() 'closes the connection
            DR2.Close() 'closes the reader
            Return retval
        End Function
        Public Function GetGlobalYearlyDefectTotal(ByVal WS As String) As Integer
            Dim retval As Integer = 0
            Dim DR2 As SqlDataReader
            Dim Command2 As New SqlCommand
            Dim Connection2 As New SqlConnection(ConfigurationManager.ConnectionStrings("MyDB").ConnectionString)
            Dim SQL As String = "Select SUM(MajorsCount+MinorsCount+RepairsCount+ScrapCount) AS TOTAL from dbo.InspectionJobSummaryYearly " & WS
            Command2.CommandType = CommandType.Text 'sets the type of the sql
            Command2.Connection = Connection2 'sets the connection of our sql command to MyDB
            Command2.CommandText = SQL 'sets the statement that executes at the data source to our string
            If (Connection2.State = ConnectionState.Closed) Then
                Connection2.Open()
            End If
            DR2 = Command2.ExecuteReader 'sends the command text to the connection and builds tthe SqlDataReader
            If DR2.HasRows = True Then 'Check whether the SqlDataReader has 1 or more rows
                DR2.Read() 'The default position of the SqlDataReader is before the first record. Therefore, you must call Read to begin accessing any data.  The reader has moved down to the first row.
                'Response.Write(DR("Cust_Name")) 'Writes a string to an HTTP response output stream.  In this case, just Cust_Name

                If Not DR2.IsDBNull(DR2.GetOrdinal("Total")) Then
                    retval = Convert.ToInt32(DR2("Total"))
                End If

            Else
                Return retval


            End If

            Connection2.Close() 'closes the connection
            DR2.Close() 'closes the reader
            Return retval
        End Function
        Public Function GetCustomDefectTotal(ByVal CID As Integer, ByVal WS As String) As Integer
            Dim retval As Integer = 0
            Dim DR2 As SqlDataReader
            Dim Command2 As New SqlCommand
            Dim Connection2 As New SqlConnection(ConfigurationManager.ConnectionStrings("MyDB").ConnectionString)
            Dim SQL As String = "Select SUM(MajorsCount+MinorsCount+RepairsCount+ScrapCount) AS TOTAL from dbo.InspectionJobSummaryYearly WHERE CID = " & CID & WS
            Command2.CommandType = CommandType.Text 'sets the type of the sql
            Command2.Connection = Connection2 'sets the connection of our sql command to MyDB
            Command2.CommandText = SQL 'sets the statement that executes at the data source to our string
            If (Connection2.State = ConnectionState.Closed) Then
                Connection2.Open()
            End If
            DR2 = Command2.ExecuteReader 'sends the command text to the connection and builds tthe SqlDataReader
            If DR2.HasRows = True Then 'Check whether the SqlDataReader has 1 or more rows
                DR2.Read() 'The default position of the SqlDataReader is before the first record. Therefore, you must call Read to begin accessing any data.  The reader has moved down to the first row.
                'Response.Write(DR("Cust_Name")) 'Writes a string to an HTTP response output stream.  In this case, just Cust_Name

                If Not DR2.IsDBNull(DR2.GetOrdinal("Total")) Then
                    retval = Convert.ToInt32(DR2("Total"))
                End If

            Else
                Return retval


            End If

            Connection2.Close() 'closes the connection
            DR2.Close() 'closes the reader
            Return retval
        End Function

        Public Function GetGlobalCustomDefectTotal(ByVal WS As String) As Integer
            Dim retval As Integer = 0
            Dim DR2 As SqlDataReader
            Dim Command2 As New SqlCommand
            Dim Connection2 As New SqlConnection(ConfigurationManager.ConnectionStrings("MyDB").ConnectionString)
            Dim SQL As String = "Select SUM(MajorsCount+MinorsCount+RepairsCount+ScrapCount) AS TOTAL from dbo.InspectionJobSummaryYearly " & WS
            Command2.CommandType = CommandType.Text 'sets the type of the sql
            Command2.Connection = Connection2 'sets the connection of our sql command to MyDB
            Command2.CommandText = SQL 'sets the statement that executes at the data source to our string
            If (Connection2.State = ConnectionState.Closed) Then
                Connection2.Open()
            End If
            DR2 = Command2.ExecuteReader 'sends the command text to the connection and builds tthe SqlDataReader
            If DR2.HasRows = True Then 'Check whether the SqlDataReader has 1 or more rows
                DR2.Read() 'The default position of the SqlDataReader is before the first record. Therefore, you must call Read to begin accessing any data.  The reader has moved down to the first row.
                'Response.Write(DR("Cust_Name")) 'Writes a string to an HTTP response output stream.  In this case, just Cust_Name

                If Not DR2.IsDBNull(DR2.GetOrdinal("Total")) Then
                    retval = Convert.ToInt32(DR2("Total"))
                End If

            Else
                Return retval


            End If

            Connection2.Close() 'closes the connection
            DR2.Close() 'closes the reader
            Return retval
        End Function
        Public Function GetMonthlyInspectionTotal(ByVal CID As Integer, ByVal WS As String) As Integer
            Dim retval As Integer = 0
            Dim DR2 As SqlDataReader
            Dim Command2 As New SqlCommand
            Dim Connection2 As New SqlConnection(ConfigurationManager.ConnectionStrings("MyDB").ConnectionString)
            Dim SQL As String = "Select COUNT(id) AS TOTAL from dbo.InspectionJobSummaryYearly WHERE CID = " & CID & WS & " And Inspection_Finished >= DATEADD(month,-1,GETDATE())"
            Command2.CommandType = CommandType.Text 'sets the type of the sql
            Command2.Connection = Connection2 'sets the connection of our sql command to MyDB
            Command2.CommandText = SQL 'sets the statement that executes at the data source to our string
            If (Connection2.State = ConnectionState.Closed) Then
                Connection2.Open()
            End If
            DR2 = Command2.ExecuteReader 'sends the command text to the connection and builds tthe SqlDataReader
            If DR2.HasRows = True Then 'Check whether the SqlDataReader has 1 or more rows
                DR2.Read() 'The default position of the SqlDataReader is before the first record. Therefore, you must call Read to begin accessing any data.  The reader has moved down to the first row.
                'Response.Write(DR("Cust_Name")) 'Writes a string to an HTTP response output stream.  In this case, just Cust_Name
                If Not DR2.IsDBNull(DR2.GetOrdinal("Total")) Then
                    retval = Convert.ToInt32(DR2("Total"))
                End If


            Else
                Return retval


            End If

            Connection2.Close() 'closes the connection
            DR2.Close() 'closes the reader
            Return retval
        End Function

        Public Function GetGlobalMonthlyInspectionTotal(ByVal WS As String) As Integer
            Dim retval As Integer = 0
            Dim DR2 As SqlDataReader
            Dim Command2 As New SqlCommand
            Dim Connection2 As New SqlConnection(ConfigurationManager.ConnectionStrings("MyDB").ConnectionString)
            Dim SQL As String = "Select COUNT(id) AS TOTAL from dbo.InspectionJobSummaryYearly " & WS & " And Inspection_Finished >= DATEADD(month,-1,GETDATE())"
            Command2.CommandType = CommandType.Text 'sets the type of the sql
            Command2.Connection = Connection2 'sets the connection of our sql command to MyDB
            Command2.CommandText = SQL 'sets the statement that executes at the data source to our string
            If (Connection2.State = ConnectionState.Closed) Then
                Connection2.Open()
            End If
            DR2 = Command2.ExecuteReader 'sends the command text to the connection and builds tthe SqlDataReader
            If DR2.HasRows = True Then 'Check whether the SqlDataReader has 1 or more rows
                DR2.Read() 'The default position of the SqlDataReader is before the first record. Therefore, you must call Read to begin accessing any data.  The reader has moved down to the first row.
                'Response.Write(DR("Cust_Name")) 'Writes a string to an HTTP response output stream.  In this case, just Cust_Name
                If Not DR2.IsDBNull(DR2.GetOrdinal("Total")) Then
                    retval = Convert.ToInt32(DR2("Total"))
                End If


            Else
                Return retval


            End If

            Connection2.Close() 'closes the connection
            DR2.Close() 'closes the reader
            Return retval
        End Function
        Public Function GetYearlyInspectionTotal(ByVal CID As Integer, ByVal WS As String) As Integer
            Dim retval As Integer = 0
            Dim DR2 As SqlDataReader
            Dim Command2 As New SqlCommand
            Dim Connection2 As New SqlConnection(ConfigurationManager.ConnectionStrings("MyDB").ConnectionString)
            Dim SQL As String = "Select COUNT(id) AS TOTAL from dbo.InspectionJobSummaryYearly WHERE CID = " & CID & WS
            Command2.CommandType = CommandType.Text 'sets the type of the sql
            Command2.Connection = Connection2 'sets the connection of our sql command to MyDB
            Command2.CommandText = SQL 'sets the statement that executes at the data source to our string
            If (Connection2.State = ConnectionState.Closed) Then
                Connection2.Open()
            End If
            DR2 = Command2.ExecuteReader 'sends the command text to the connection and builds tthe SqlDataReader
            If DR2.HasRows = True Then 'Check whether the SqlDataReader has 1 or more rows
                DR2.Read() 'The default position of the SqlDataReader is before the first record. Therefore, you must call Read to begin accessing any data.  The reader has moved down to the first row.
                'Response.Write(DR("Cust_Name")) 'Writes a string to an HTTP response output stream.  In this case, just Cust_Name

                If Not DR2.IsDBNull(DR2.GetOrdinal("Total")) Then
                    retval = Convert.ToInt32(DR2("Total"))
                End If

            Else
                Return retval


            End If

            Connection2.Close() 'closes the connection
            DR2.Close() 'closes the reader
            Return retval
        End Function

        Public Function GetGlobalYearlyInspectionTotal(ByVal WS As String) As Integer
            Dim retval As Integer = 0
            Dim DR2 As SqlDataReader
            Dim Command2 As New SqlCommand
            Dim Connection2 As New SqlConnection(ConfigurationManager.ConnectionStrings("MyDB").ConnectionString)
            Dim SQL As String = "Select COUNT(id) AS TOTAL from dbo.InspectionJobSummaryYearly " & WS
            Command2.CommandType = CommandType.Text 'sets the type of the sql
            Command2.Connection = Connection2 'sets the connection of our sql command to MyDB
            Command2.CommandText = SQL 'sets the statement that executes at the data source to our string
            If (Connection2.State = ConnectionState.Closed) Then
                Connection2.Open()
            End If
            DR2 = Command2.ExecuteReader 'sends the command text to the connection and builds tthe SqlDataReader
            If DR2.HasRows = True Then 'Check whether the SqlDataReader has 1 or more rows
                DR2.Read() 'The default position of the SqlDataReader is before the first record. Therefore, you must call Read to begin accessing any data.  The reader has moved down to the first row.
                'Response.Write(DR("Cust_Name")) 'Writes a string to an HTTP response output stream.  In this case, just Cust_Name

                If Not DR2.IsDBNull(DR2.GetOrdinal("Total")) Then
                    retval = Convert.ToInt32(DR2("Total"))
                End If

            Else
                Return retval


            End If

            Connection2.Close() 'closes the connection
            DR2.Close() 'closes the reader
            Return retval
        End Function
        Public Function GetCustomInspectionTotal(ByVal CID As Integer, ByVal WS As String) As Integer
            Dim retval As Integer = 0
            Dim DR2 As SqlDataReader
            Dim Command2 As New SqlCommand
            Dim Connection2 As New SqlConnection(ConfigurationManager.ConnectionStrings("MyDB").ConnectionString)
            Dim SQL As String = "Select COUNT(id) AS TOTAL from dbo.InspectionJobSummaryYearly WHERE CID = " & CID & WS
            Command2.CommandType = CommandType.Text 'sets the type of the sql
            Command2.Connection = Connection2 'sets the connection of our sql command to MyDB
            Command2.CommandText = SQL 'sets the statement that executes at the data source to our string
            If (Connection2.State = ConnectionState.Closed) Then
                Connection2.Open()
            End If
            DR2 = Command2.ExecuteReader 'sends the command text to the connection and builds tthe SqlDataReader
            If DR2.HasRows = True Then 'Check whether the SqlDataReader has 1 or more rows
                DR2.Read() 'The default position of the SqlDataReader is before the first record. Therefore, you must call Read to begin accessing any data.  The reader has moved down to the first row.
                'Response.Write(DR("Cust_Name")) 'Writes a string to an HTTP response output stream.  In this case, just Cust_Name

                If Not DR2.IsDBNull(DR2.GetOrdinal("Total")) Then
                    retval = Convert.ToInt32(DR2("Total"))
                End If

            Else
                Return retval


            End If

            Connection2.Close() 'closes the connection
            DR2.Close() 'closes the reader
            Return retval
        End Function
        Public Function GetGlobalCustomInspectionTotal(ByVal WS As String) As Integer
            Dim retval As Integer = 0
            Dim DR2 As SqlDataReader
            Dim Command2 As New SqlCommand
            Dim Connection2 As New SqlConnection(ConfigurationManager.ConnectionStrings("MyDB").ConnectionString)
            Dim SQL As String = "Select COUNT(id) AS TOTAL from dbo.InspectionJobSummaryYearly " & WS
            Command2.CommandType = CommandType.Text 'sets the type of the sql
            Command2.Connection = Connection2 'sets the connection of our sql command to MyDB
            Command2.CommandText = SQL 'sets the statement that executes at the data source to our string
            If (Connection2.State = ConnectionState.Closed) Then
                Connection2.Open()
            End If
            DR2 = Command2.ExecuteReader 'sends the command text to the connection and builds tthe SqlDataReader
            If DR2.HasRows = True Then 'Check whether the SqlDataReader has 1 or more rows
                DR2.Read() 'The default position of the SqlDataReader is before the first record. Therefore, you must call Read to begin accessing any data.  The reader has moved down to the first row.
                'Response.Write(DR("Cust_Name")) 'Writes a string to an HTTP response output stream.  In this case, just Cust_Name

                If Not DR2.IsDBNull(DR2.GetOrdinal("Total")) Then
                    retval = Convert.ToInt32(DR2("Total"))
                End If

            Else
                Return retval


            End If

            Connection2.Close() 'closes the connection
            DR2.Close() 'closes the reader
            Return retval
        End Function
        Public Function GetMonthlyRejectTotal(ByVal CID As Integer, ByVal WS As String) As Integer
            Dim retval As Integer = 0
            Dim DR2 As SqlDataReader
            Dim Command2 As New SqlCommand
            Dim Connection2 As New SqlConnection(ConfigurationManager.ConnectionStrings("MyDB").ConnectionString)
            Dim SQL As String = "Select SUM(MajorsCount+RepairsCount+ScrapCount) AS TOTAL from dbo.InspectionJobSummaryYearly WHERE CID = " & CID & WS & " And Inspection_Finished >= DATEADD(month,-1,GETDATE())"
            Command2.CommandType = CommandType.Text 'sets the type of the sql
            Command2.Connection = Connection2 'sets the connection of our sql command to MyDB
            Command2.CommandText = SQL 'sets the statement that executes at the data source to our string
            If (Connection2.State = ConnectionState.Closed) Then
                Connection2.Open()
            End If
            DR2 = Command2.ExecuteReader 'sends the command text to the connection and builds tthe SqlDataReader
            If DR2.HasRows = True Then 'Check whether the SqlDataReader has 1 or more rows
                DR2.Read() 'The default position of the SqlDataReader is before the first record. Therefore, you must call Read to begin accessing any data.  The reader has moved down to the first row.
                'Response.Write(DR("Cust_Name")) 'Writes a string to an HTTP response output stream.  In this case, just Cust_Name
                If Not DR2.IsDBNull(DR2.GetOrdinal("Total")) Then
                    retval = Convert.ToInt32(DR2("Total"))
                End If


            Else
                Return retval


            End If

            Connection2.Close() 'closes the connection
            DR2.Close() 'closes the reader
            Return retval
        End Function
        Public Function GetGlobalMonthlyRejectTotal(ByVal WS As String) As Integer
            Dim retval As Integer = 0
            Dim DR2 As SqlDataReader
            Dim Command2 As New SqlCommand
            Dim Connection2 As New SqlConnection(ConfigurationManager.ConnectionStrings("MyDB").ConnectionString)
            Dim SQL As String = "Select SUM(MajorsCount+RepairsCount+ScrapCount) AS TOTAL from dbo.InspectionJobSummaryYearly " & WS & " And Inspection_Finished >= DATEADD(month,-1,GETDATE())"
            Command2.CommandType = CommandType.Text 'sets the type of the sql
            Command2.Connection = Connection2 'sets the connection of our sql command to MyDB
            Command2.CommandText = SQL 'sets the statement that executes at the data source to our string
            If (Connection2.State = ConnectionState.Closed) Then
                Connection2.Open()
            End If
            DR2 = Command2.ExecuteReader 'sends the command text to the connection and builds tthe SqlDataReader
            If DR2.HasRows = True Then 'Check whether the SqlDataReader has 1 or more rows
                DR2.Read() 'The default position of the SqlDataReader is before the first record. Therefore, you must call Read to begin accessing any data.  The reader has moved down to the first row.
                'Response.Write(DR("Cust_Name")) 'Writes a string to an HTTP response output stream.  In this case, just Cust_Name
                If Not DR2.IsDBNull(DR2.GetOrdinal("Total")) Then
                    retval = Convert.ToInt32(DR2("Total"))
                End If


            Else
                Return retval


            End If

            Connection2.Close() 'closes the connection
            DR2.Close() 'closes the reader
            Return retval
        End Function
        Public Function GetYearlyRejectTotal(ByVal CID As Integer, ByVal WS As String) As Integer
            Dim retval As Integer = 0
            Dim DR2 As SqlDataReader
            Dim Command2 As New SqlCommand
            Dim Connection2 As New SqlConnection(ConfigurationManager.ConnectionStrings("MyDB").ConnectionString)
            Dim SQL As String = "Select SUM(MajorsCount+RepairsCount+ScrapCount) AS TOTAL from dbo.InspectionJobSummaryYearly WHERE CID = " & CID & WS
            Command2.CommandType = CommandType.Text 'sets the type of the sql
            Command2.Connection = Connection2 'sets the connection of our sql command to MyDB
            Command2.CommandText = SQL 'sets the statement that executes at the data source to our string
            If (Connection2.State = ConnectionState.Closed) Then
                Connection2.Open()
            End If
            DR2 = Command2.ExecuteReader 'sends the command text to the connection and builds tthe SqlDataReader
            If DR2.HasRows = True Then 'Check whether the SqlDataReader has 1 or more rows
                DR2.Read() 'The default position of the SqlDataReader is before the first record. Therefore, you must call Read to begin accessing any data.  The reader has moved down to the first row.
                'Response.Write(DR("Cust_Name")) 'Writes a string to an HTTP response output stream.  In this case, just Cust_Name

                If Not DR2.IsDBNull(DR2.GetOrdinal("Total")) Then
                    retval = Convert.ToInt32(DR2("Total"))
                End If

            Else
                Return retval


            End If

            Connection2.Close() 'closes the connection
            DR2.Close() 'closes the reader
            Return retval
        End Function
        Public Function GetGlobalYearlyRejectTotal(ByVal WS As String) As Integer
            Dim retval As Integer = 0
            Dim DR2 As SqlDataReader
            Dim Command2 As New SqlCommand
            Dim Connection2 As New SqlConnection(ConfigurationManager.ConnectionStrings("MyDB").ConnectionString)
            Dim SQL As String = "Select SUM(MajorsCount+RepairsCount+ScrapCount) AS TOTAL from dbo.InspectionJobSummaryYearly " & WS
            Command2.CommandType = CommandType.Text 'sets the type of the sql
            Command2.Connection = Connection2 'sets the connection of our sql command to MyDB
            Command2.CommandText = SQL 'sets the statement that executes at the data source to our string
            If (Connection2.State = ConnectionState.Closed) Then
                Connection2.Open()
            End If
            DR2 = Command2.ExecuteReader 'sends the command text to the connection and builds tthe SqlDataReader
            If DR2.HasRows = True Then 'Check whether the SqlDataReader has 1 or more rows
                DR2.Read() 'The default position of the SqlDataReader is before the first record. Therefore, you must call Read to begin accessing any data.  The reader has moved down to the first row.
                'Response.Write(DR("Cust_Name")) 'Writes a string to an HTTP response output stream.  In this case, just Cust_Name

                If Not DR2.IsDBNull(DR2.GetOrdinal("Total")) Then
                    retval = Convert.ToInt32(DR2("Total"))
                End If

            Else
                Return retval


            End If

            Connection2.Close() 'closes the connection
            DR2.Close() 'closes the reader
            Return retval
        End Function
        Public Function GetCustomRejectTotal(ByVal CID As Integer, ByVal WS As String) As Integer
            Dim retval As Integer = 0
            Dim DR2 As SqlDataReader
            Dim Command2 As New SqlCommand
            Dim Connection2 As New SqlConnection(ConfigurationManager.ConnectionStrings("MyDB").ConnectionString)
            Dim SQL As String = "Select SUM(MajorsCount+RepairsCount+ScrapCount) AS TOTAL from dbo.InspectionJobSummaryYearly WHERE CID = " & CID & WS
            Command2.CommandType = CommandType.Text 'sets the type of the sql
            Command2.Connection = Connection2 'sets the connection of our sql command to MyDB
            Command2.CommandText = SQL 'sets the statement that executes at the data source to our string
            If (Connection2.State = ConnectionState.Closed) Then
                Connection2.Open()
            End If
            DR2 = Command2.ExecuteReader 'sends the command text to the connection and builds tthe SqlDataReader
            If DR2.HasRows = True Then 'Check whether the SqlDataReader has 1 or more rows
                DR2.Read() 'The default position of the SqlDataReader is before the first record. Therefore, you must call Read to begin accessing any data.  The reader has moved down to the first row.
                'Response.Write(DR("Cust_Name")) 'Writes a string to an HTTP response output stream.  In this case, just Cust_Name

                If Not DR2.IsDBNull(DR2.GetOrdinal("Total")) Then
                    retval = Convert.ToInt32(DR2("Total"))
                End If

            Else
                Return retval


            End If

            Connection2.Close() 'closes the connection
            DR2.Close() 'closes the reader
            Return retval
        End Function

        Public Function GetGlobalCustomRejectTotal(ByVal WS As String) As Integer
            Dim retval As Integer = 0
            Dim DR2 As SqlDataReader
            Dim Command2 As New SqlCommand
            Dim Connection2 As New SqlConnection(ConfigurationManager.ConnectionStrings("MyDB").ConnectionString)
            Dim SQL As String = "Select SUM(MajorsCount+RepairsCount+ScrapCount) AS TOTAL from dbo.InspectionJobSummaryYearly " & WS
            Command2.CommandType = CommandType.Text 'sets the type of the sql
            Command2.Connection = Connection2 'sets the connection of our sql command to MyDB
            Command2.CommandText = SQL 'sets the statement that executes at the data source to our string
            If (Connection2.State = ConnectionState.Closed) Then
                Connection2.Open()
            End If
            DR2 = Command2.ExecuteReader 'sends the command text to the connection and builds tthe SqlDataReader
            If DR2.HasRows = True Then 'Check whether the SqlDataReader has 1 or more rows
                DR2.Read() 'The default position of the SqlDataReader is before the first record. Therefore, you must call Read to begin accessing any data.  The reader has moved down to the first row.
                'Response.Write(DR("Cust_Name")) 'Writes a string to an HTTP response output stream.  In this case, just Cust_Name

                If Not DR2.IsDBNull(DR2.GetOrdinal("Total")) Then
                    retval = Convert.ToInt32(DR2("Total"))
                End If

            Else
                Return retval


            End If

            Connection2.Close() 'closes the connection
            DR2.Close() 'closes the reader
            Return retval
        End Function
        Public Function GetMonthlyRejectLotTotal(ByVal CID As Integer, ByVal WS As String) As Integer
            Dim retval As Integer = 0
            Dim DR2 As SqlDataReader
            Dim Command2 As New SqlCommand
            Dim Connection2 As New SqlConnection(ConfigurationManager.ConnectionStrings("MyDB").ConnectionString)
            Dim SQL As String = "Select COUNT(id) AS TOTAL from dbo.InspectionJobSummaryYearly WHERE CID = " & CID & WS & " And Technical_PassFail = 0 And Inspection_Finished >= DATEADD(month,-1,GETDATE())"
            Command2.CommandType = CommandType.Text 'sets the type of the sql
            Command2.Connection = Connection2 'sets the connection of our sql command to MyDB
            Command2.CommandText = SQL 'sets the statement that executes at the data source to our string
            If (Connection2.State = ConnectionState.Closed) Then
                Connection2.Open()
            End If
            DR2 = Command2.ExecuteReader 'sends the command text to the connection and builds tthe SqlDataReader
            If DR2.HasRows = True Then 'Check whether the SqlDataReader has 1 or more rows
                DR2.Read() 'The default position of the SqlDataReader is before the first record. Therefore, you must call Read to begin accessing any data.  The reader has moved down to the first row.
                'Response.Write(DR("Cust_Name")) 'Writes a string to an HTTP response output stream.  In this case, just Cust_Name
                If Not DR2.IsDBNull(DR2.GetOrdinal("Total")) Then
                    retval = Convert.ToInt32(DR2("Total"))
                End If


            Else
                Return retval


            End If

            Connection2.Close() 'closes the connection
            DR2.Close() 'closes the reader
            Return retval
        End Function
        Public Function GetGlobalMonthlyRejectLotTotal(ByVal WS As String) As Integer
            Dim retval As Integer = 0
            Dim DR2 As SqlDataReader
            Dim Command2 As New SqlCommand
            Dim Connection2 As New SqlConnection(ConfigurationManager.ConnectionStrings("MyDB").ConnectionString)
            Dim SQL As String = "Select COUNT(id) AS TOTAL from dbo.InspectionJobSummaryYearly " & WS & " And Technical_PassFail = 0 And Inspection_Finished >= DATEADD(month,-1,GETDATE())"
            Command2.CommandType = CommandType.Text 'sets the type of the sql
            Command2.Connection = Connection2 'sets the connection of our sql command to MyDB
            Command2.CommandText = SQL 'sets the statement that executes at the data source to our string
            If (Connection2.State = ConnectionState.Closed) Then
                Connection2.Open()
            End If
            DR2 = Command2.ExecuteReader 'sends the command text to the connection and builds tthe SqlDataReader
            If DR2.HasRows = True Then 'Check whether the SqlDataReader has 1 or more rows
                DR2.Read() 'The default position of the SqlDataReader is before the first record. Therefore, you must call Read to begin accessing any data.  The reader has moved down to the first row.
                'Response.Write(DR("Cust_Name")) 'Writes a string to an HTTP response output stream.  In this case, just Cust_Name
                If Not DR2.IsDBNull(DR2.GetOrdinal("Total")) Then
                    retval = Convert.ToInt32(DR2("Total"))
                End If


            Else
                Return retval


            End If

            Connection2.Close() 'closes the connection
            DR2.Close() 'closes the reader
            Return retval
        End Function
        Public Function GetYearlyRejectLotTotal(ByVal CID As Integer, ByVal WS As String) As Integer
            Dim retval As Double = 0
            Dim DR2 As SqlDataReader
            Dim Command2 As New SqlCommand
            Dim Connection2 As New SqlConnection(ConfigurationManager.ConnectionStrings("MyDB").ConnectionString)
            Dim SQL As String = "Select COUNT(id) AS TOTAL from dbo.InspectionJobSummaryYearly WHERE CID = " & CID & WS & " And Technical_PassFail = 0"
            Command2.CommandType = CommandType.Text 'sets the type of the sql
            Command2.Connection = Connection2 'sets the connection of our sql command to MyDB
            Command2.CommandText = SQL 'sets the statement that executes at the data source to our string
            If (Connection2.State = ConnectionState.Closed) Then
                Connection2.Open()
            End If
            DR2 = Command2.ExecuteReader 'sends the command text to the connection and builds tthe SqlDataReader
            If DR2.HasRows = True Then 'Check whether the SqlDataReader has 1 or more rows
                DR2.Read() 'The default position of the SqlDataReader is before the first record. Therefore, you must call Read to begin accessing any data.  The reader has moved down to the first row.
                'Response.Write(DR("Cust_Name")) 'Writes a string to an HTTP response output stream.  In this case, just Cust_Name

                If Not DR2.IsDBNull(DR2.GetOrdinal("Total")) Then
                    retval = Convert.ToInt32(DR2("Total"))
                End If

            Else
                Return retval


            End If

            Connection2.Close() 'closes the connection
            DR2.Close() 'closes the reader
            Return retval
        End Function
        Public Function GetGlobalYearlyRejectLotTotal(ByVal WS As String) As Integer
            Dim retval As Double = 0
            Dim DR2 As SqlDataReader
            Dim Command2 As New SqlCommand
            Dim Connection2 As New SqlConnection(ConfigurationManager.ConnectionStrings("MyDB").ConnectionString)
            Dim SQL As String = "Select COUNT(id) AS TOTAL from dbo.InspectionJobSummaryYearly " & WS & " And Technical_PassFail = 0"
            Command2.CommandType = CommandType.Text 'sets the type of the sql
            Command2.Connection = Connection2 'sets the connection of our sql command to MyDB
            Command2.CommandText = SQL 'sets the statement that executes at the data source to our string
            If (Connection2.State = ConnectionState.Closed) Then
                Connection2.Open()
            End If
            DR2 = Command2.ExecuteReader 'sends the command text to the connection and builds tthe SqlDataReader
            If DR2.HasRows = True Then 'Check whether the SqlDataReader has 1 or more rows
                DR2.Read() 'The default position of the SqlDataReader is before the first record. Therefore, you must call Read to begin accessing any data.  The reader has moved down to the first row.
                'Response.Write(DR("Cust_Name")) 'Writes a string to an HTTP response output stream.  In this case, just Cust_Name

                If Not DR2.IsDBNull(DR2.GetOrdinal("Total")) Then
                    retval = Convert.ToInt32(DR2("Total"))
                End If

            Else
                Return retval


            End If

            Connection2.Close() 'closes the connection
            DR2.Close() 'closes the reader
            Return retval
        End Function
        Public Function GetCustomRejectLotTotal(ByVal CID As Integer, ByVal WS As String) As Integer
            Dim retval As Double = 0
            Dim DR2 As SqlDataReader
            Dim Command2 As New SqlCommand
            Dim Connection2 As New SqlConnection(ConfigurationManager.ConnectionStrings("MyDB").ConnectionString)
            Dim SQL As String = "Select COUNT(id) AS TOTAL from dbo.InspectionJobSummaryYearly WHERE CID = " & CID & " And Technical_PassFail = 0 " & WS
            Command2.CommandType = CommandType.Text 'sets the type of the sql
            Command2.Connection = Connection2 'sets the connection of our sql command to MyDB
            Command2.CommandText = SQL 'sets the statement that executes at the data source to our string
            If (Connection2.State = ConnectionState.Closed) Then
                Connection2.Open()
            End If
            DR2 = Command2.ExecuteReader 'sends the command text to the connection and builds tthe SqlDataReader
            If DR2.HasRows = True Then 'Check whether the SqlDataReader has 1 or more rows
                DR2.Read() 'The default position of the SqlDataReader is before the first record. Therefore, you must call Read to begin accessing any data.  The reader has moved down to the first row.
                'Response.Write(DR("Cust_Name")) 'Writes a string to an HTTP response output stream.  In this case, just Cust_Name

                If Not DR2.IsDBNull(DR2.GetOrdinal("Total")) Then
                    retval = Convert.ToInt32(DR2("Total"))
                End If

            Else
                Return retval


            End If

            Connection2.Close() 'closes the connection
            DR2.Close() 'closes the reader
            Return retval
        End Function
        Public Function GetGlobalCustomRejectLotTotal(ByVal WS As String) As Integer
            Dim retval As Double = 0
            Dim DR2 As SqlDataReader
            Dim Command2 As New SqlCommand
            Dim Connection2 As New SqlConnection(ConfigurationManager.ConnectionStrings("MyDB").ConnectionString)
            Dim SQL As String = "Select COUNT(id) AS TOTAL from dbo.InspectionJobSummaryYearly " & WS & " And Technical_PassFail = 0 "
            Command2.CommandType = CommandType.Text 'sets the type of the sql
            Command2.Connection = Connection2 'sets the connection of our sql command to MyDB
            Command2.CommandText = SQL 'sets the statement that executes at the data source to our string
            If (Connection2.State = ConnectionState.Closed) Then
                Connection2.Open()
            End If
            DR2 = Command2.ExecuteReader 'sends the command text to the connection and builds tthe SqlDataReader
            If DR2.HasRows = True Then 'Check whether the SqlDataReader has 1 or more rows
                DR2.Read() 'The default position of the SqlDataReader is before the first record. Therefore, you must call Read to begin accessing any data.  The reader has moved down to the first row.
                'Response.Write(DR("Cust_Name")) 'Writes a string to an HTTP response output stream.  In this case, just Cust_Name

                If Not DR2.IsDBNull(DR2.GetOrdinal("Total")) Then
                    retval = Convert.ToInt32(DR2("Total"))
                End If

            Else
                Return retval


            End If

            Connection2.Close() 'closes the connection
            DR2.Close() 'closes the reader
            Return retval
        End Function
        Public Function GetMonthlyDHU(ByVal CID As Integer, ByVal WS As String) As Double
            Dim retval As Integer = 0
            Dim DR2 As SqlDataReader
            Dim Command2 As New SqlCommand
            Dim Connection2 As New SqlConnection(ConfigurationManager.ConnectionStrings("MyDB").ConnectionString)
            Dim SQL As String = "Select (CAST(SUM(MajorsCount+MinorsCount+RepairsCount+ScrapCount) AS Decimal(10,2))/CAST(SUM(SampleSize) AS Decimal(10,2)))*100 AS TOTAL from dbo.InspectionJobSummaryYearly WHERE CID = " & CID & WS & " And Inspection_Finished >= DATEADD(month,-1,GETDATE())"
            Command2.CommandType = CommandType.Text 'sets the type of the sql
            Command2.Connection = Connection2 'sets the connection of our sql command to MyDB
            Command2.CommandText = SQL 'sets the statement that executes at the data source to our string
            If (Connection2.State = ConnectionState.Closed) Then
                Connection2.Open()
            End If
            DR2 = Command2.ExecuteReader 'sends the command text to the connection and builds tthe SqlDataReader
            If DR2.HasRows = True Then 'Check whether the SqlDataReader has 1 or more rows
                DR2.Read() 'The default position of the SqlDataReader is before the first record. Therefore, you must call Read to begin accessing any data.  The reader has moved down to the first row.
                'Response.Write(DR("Cust_Name")) 'Writes a string to an HTTP response output stream.  In this case, just Cust_Name
                If Not DR2.IsDBNull(DR2.GetOrdinal("Total")) Then
                    retval = Math.Round(Convert.ToDouble(DR2("Total")), 2)
                End If


            Else
                Return retval


            End If

            Connection2.Close() 'closes the connection
            DR2.Close() 'closes the reader
            Return retval
        End Function

        Public Function GetGlobalMonthlyDHU(ByVal WS As String) As Double
            Dim retval As Integer = 0
            Dim DR2 As SqlDataReader
            Dim Command2 As New SqlCommand
            Dim Connection2 As New SqlConnection(ConfigurationManager.ConnectionStrings("MyDB").ConnectionString)
            Dim SQL As String = "Select (CAST(SUM(MajorsCount+MinorsCount+RepairsCount+ScrapCount) AS Decimal(10,2))/CAST(SUM(SampleSize) AS Decimal(10,2)))*100 AS TOTAL from dbo.InspectionJobSummaryYearly " & WS & " And Inspection_Finished >= DATEADD(month,-1,GETDATE())"
            Command2.CommandType = CommandType.Text 'sets the type of the sql
            Command2.Connection = Connection2 'sets the connection of our sql command to MyDB
            Command2.CommandText = SQL 'sets the statement that executes at the data source to our string
            If (Connection2.State = ConnectionState.Closed) Then
                Connection2.Open()
            End If
            DR2 = Command2.ExecuteReader 'sends the command text to the connection and builds tthe SqlDataReader
            If DR2.HasRows = True Then 'Check whether the SqlDataReader has 1 or more rows
                DR2.Read() 'The default position of the SqlDataReader is before the first record. Therefore, you must call Read to begin accessing any data.  The reader has moved down to the first row.
                'Response.Write(DR("Cust_Name")) 'Writes a string to an HTTP response output stream.  In this case, just Cust_Name
                If Not DR2.IsDBNull(DR2.GetOrdinal("Total")) Then
                    retval = Math.Round(Convert.ToDouble(DR2("Total")), 2)
                End If


            Else
                Return retval


            End If

            Connection2.Close() 'closes the connection
            DR2.Close() 'closes the reader
            Return retval
        End Function
        Public Function GetYearlyDHU(ByVal CID As Integer, ByVal WS As String) As Double
            Dim retval As Double = 0
            Dim DR2 As SqlDataReader
            Dim Command2 As New SqlCommand
            Dim Connection2 As New SqlConnection(ConfigurationManager.ConnectionStrings("MyDB").ConnectionString)
            Dim SQL As String = "Select (CAST(SUM(MajorsCount+MinorsCount+RepairsCount+ScrapCount) AS Decimal(10,2))/CAST(SUM(SampleSize) AS Decimal(10,2)))*100 AS TOTAL from dbo.InspectionJobSummaryYearly WHERE CID = " & CID & WS
            Command2.CommandType = CommandType.Text 'sets the type of the sql
            Command2.Connection = Connection2 'sets the connection of our sql command to MyDB
            Command2.CommandText = SQL 'sets the statement that executes at the data source to our string
            If (Connection2.State = ConnectionState.Closed) Then
                Connection2.Open()
            End If
            DR2 = Command2.ExecuteReader 'sends the command text to the connection and builds tthe SqlDataReader
            If DR2.HasRows = True Then 'Check whether the SqlDataReader has 1 or more rows
                DR2.Read() 'The default position of the SqlDataReader is before the first record. Therefore, you must call Read to begin accessing any data.  The reader has moved down to the first row.
                'Response.Write(DR("Cust_Name")) 'Writes a string to an HTTP response output stream.  In this case, just Cust_Name

                If Not DR2.IsDBNull(DR2.GetOrdinal("Total")) Then
                    retval = Math.Round(Convert.ToDouble(DR2("Total")), 2)
                End If

            Else
                Return retval


            End If

            Connection2.Close() 'closes the connection
            DR2.Close() 'closes the reader
            Return retval
        End Function
        Public Function GetGlobalYearlyDHU(ByVal WS As String) As Double
            Dim retval As Double = 0
            Dim DR2 As SqlDataReader
            Dim Command2 As New SqlCommand
            Dim Connection2 As New SqlConnection(ConfigurationManager.ConnectionStrings("MyDB").ConnectionString)
            Dim SQL As String = "Select (CAST(SUM(MajorsCount+MinorsCount+RepairsCount+ScrapCount) AS Decimal(10,2))/CAST(SUM(SampleSize) AS Decimal(10,2)))*100 AS TOTAL from dbo.InspectionJobSummaryYearly " & WS
            Command2.CommandType = CommandType.Text 'sets the type of the sql
            Command2.Connection = Connection2 'sets the connection of our sql command to MyDB
            Command2.CommandText = SQL 'sets the statement that executes at the data source to our string
            If (Connection2.State = ConnectionState.Closed) Then
                Connection2.Open()
            End If
            DR2 = Command2.ExecuteReader 'sends the command text to the connection and builds tthe SqlDataReader
            If DR2.HasRows = True Then 'Check whether the SqlDataReader has 1 or more rows
                DR2.Read() 'The default position of the SqlDataReader is before the first record. Therefore, you must call Read to begin accessing any data.  The reader has moved down to the first row.
                'Response.Write(DR("Cust_Name")) 'Writes a string to an HTTP response output stream.  In this case, just Cust_Name

                If Not DR2.IsDBNull(DR2.GetOrdinal("Total")) Then
                    retval = Math.Round(Convert.ToDouble(DR2("Total")), 2)
                End If

            Else
                Return retval


            End If

            Connection2.Close() 'closes the connection
            DR2.Close() 'closes the reader
            Return retval
        End Function
        Public Function GetCustomDHU(ByVal CID As Integer, ByVal WS As String) As Double
            Dim retval As Double = 0
            Dim DR2 As SqlDataReader
            Dim Command2 As New SqlCommand
            Dim Connection2 As New SqlConnection(ConfigurationManager.ConnectionStrings("MyDB").ConnectionString)
            Dim SQL As String = "Select (CAST(SUM(MajorsCount+MinorsCount+RepairsCount+ScrapCount) AS Decimal(10,2))/CAST(SUM(SampleSize) AS Decimal(10,2)))*100 AS TOTAL from dbo.InspectionJobSummaryYearly WHERE CID = " & CID & WS
            Command2.CommandType = CommandType.Text 'sets the type of the sql
            Command2.Connection = Connection2 'sets the connection of our sql command to MyDB
            Command2.CommandText = SQL 'sets the statement that executes at the data source to our string
            If (Connection2.State = ConnectionState.Closed) Then
                Connection2.Open()
            End If
            DR2 = Command2.ExecuteReader 'sends the command text to the connection and builds tthe SqlDataReader
            If DR2.HasRows = True Then 'Check whether the SqlDataReader has 1 or more rows
                DR2.Read() 'The default position of the SqlDataReader is before the first record. Therefore, you must call Read to begin accessing any data.  The reader has moved down to the first row.
                'Response.Write(DR("Cust_Name")) 'Writes a string to an HTTP response output stream.  In this case, just Cust_Name

                If Not DR2.IsDBNull(DR2.GetOrdinal("Total")) Then
                    retval = Math.Round(Convert.ToDouble(DR2("Total")), 2)
                End If

            Else
                Return retval


            End If

            Connection2.Close() 'closes the connection
            DR2.Close() 'closes the reader
            Return retval
        End Function
        Public Function GetGlobalCustomDHU(ByVal WS As String) As Double
            Dim retval As Double = 0
            Dim DR2 As SqlDataReader
            Dim Command2 As New SqlCommand
            Dim Connection2 As New SqlConnection(ConfigurationManager.ConnectionStrings("MyDB").ConnectionString)
            Dim SQL As String = "Select (CAST(SUM(MajorsCount+MinorsCount+RepairsCount+ScrapCount) AS Decimal(10,2))/CAST(SUM(SampleSize) AS Decimal(10,2)))*100 AS TOTAL from dbo.InspectionJobSummaryYearly " & WS
            Command2.CommandType = CommandType.Text 'sets the type of the sql
            Command2.Connection = Connection2 'sets the connection of our sql command to MyDB
            Command2.CommandText = SQL 'sets the statement that executes at the data source to our string
            If (Connection2.State = ConnectionState.Closed) Then
                Connection2.Open()
            End If
            DR2 = Command2.ExecuteReader 'sends the command text to the connection and builds tthe SqlDataReader
            If DR2.HasRows = True Then 'Check whether the SqlDataReader has 1 or more rows
                DR2.Read() 'The default position of the SqlDataReader is before the first record. Therefore, you must call Read to begin accessing any data.  The reader has moved down to the first row.
                'Response.Write(DR("Cust_Name")) 'Writes a string to an HTTP response output stream.  In this case, just Cust_Name

                If Not DR2.IsDBNull(DR2.GetOrdinal("Total")) Then
                    retval = Math.Round(Convert.ToDouble(DR2("Total")), 2)
                End If

            Else
                Return retval


            End If

            Connection2.Close() 'closes the connection
            DR2.Close() 'closes the reader
            Return retval
        End Function
        Public Function GetMonthlyRejectionRate(ByVal CID As Integer, ByVal WS As String) As Double
            Dim retval As Integer = 0
            Dim DR2 As SqlDataReader
            Dim Command2 As New SqlCommand
            Dim Connection2 As New SqlConnection(ConfigurationManager.ConnectionStrings("MyDB").ConnectionString)
            Dim SQL As String = "Select (CAST(SUM(MajorsCount+RepairsCount+ScrapCount) AS Decimal(10,2))/CAST(SUM(SampleSize) AS Decimal(10,2)))*100 AS TOTAL from dbo.InspectionJobSummaryYearly WHERE CID = " & CID & WS & " And Inspection_Finished >= DATEADD(month,-1,GETDATE())"
            Command2.CommandType = CommandType.Text 'sets the type of the sql
            Command2.Connection = Connection2 'sets the connection of our sql command to MyDB
            Command2.CommandText = SQL 'sets the statement that executes at the data source to our string
            If (Connection2.State = ConnectionState.Closed) Then
                Connection2.Open()
            End If
            DR2 = Command2.ExecuteReader 'sends the command text to the connection and builds tthe SqlDataReader
            If DR2.HasRows = True Then 'Check whether the SqlDataReader has 1 or more rows
                DR2.Read() 'The default position of the SqlDataReader is before the first record. Therefore, you must call Read to begin accessing any data.  The reader has moved down to the first row.
                'Response.Write(DR("Cust_Name")) 'Writes a string to an HTTP response output stream.  In this case, just Cust_Name
                If Not DR2.IsDBNull(DR2.GetOrdinal("Total")) Then
                    retval = Math.Round(Convert.ToDouble(DR2("Total")), 2)
                End If


            Else
                Return retval


            End If

            Connection2.Close() 'closes the connection
            DR2.Close() 'closes the reader
            Return retval
        End Function
        Public Function GetGlobalMonthlyRejectionRate(ByVal WS As String) As Double
            Dim retval As Integer = 0
            Dim DR2 As SqlDataReader
            Dim Command2 As New SqlCommand
            Dim Connection2 As New SqlConnection(ConfigurationManager.ConnectionStrings("MyDB").ConnectionString)
            Dim SQL As String = "Select (CAST(SUM(MajorsCount+RepairsCount+ScrapCount) AS Decimal(10,2))/CAST(SUM(SampleSize) AS Decimal(10,2)))*100 AS TOTAL from dbo.InspectionJobSummaryYearly " & WS & " And Inspection_Finished >= DATEADD(month,-1,GETDATE())"
            Command2.CommandType = CommandType.Text 'sets the type of the sql
            Command2.Connection = Connection2 'sets the connection of our sql command to MyDB
            Command2.CommandText = SQL 'sets the statement that executes at the data source to our string
            If (Connection2.State = ConnectionState.Closed) Then
                Connection2.Open()
            End If
            DR2 = Command2.ExecuteReader 'sends the command text to the connection and builds tthe SqlDataReader
            If DR2.HasRows = True Then 'Check whether the SqlDataReader has 1 or more rows
                DR2.Read() 'The default position of the SqlDataReader is before the first record. Therefore, you must call Read to begin accessing any data.  The reader has moved down to the first row.
                'Response.Write(DR("Cust_Name")) 'Writes a string to an HTTP response output stream.  In this case, just Cust_Name
                If Not DR2.IsDBNull(DR2.GetOrdinal("Total")) Then
                    retval = Math.Round(Convert.ToDouble(DR2("Total")), 2)
                End If


            Else
                Return retval


            End If

            Connection2.Close() 'closes the connection
            DR2.Close() 'closes the reader
            Return retval
        End Function
        Public Function GetYearlyRejectionRate(ByVal CID As Integer, ByVal WS As String) As Double
            Dim retval As Double = 0
            Dim DR2 As SqlDataReader
            Dim Command2 As New SqlCommand
            Dim Connection2 As New SqlConnection(ConfigurationManager.ConnectionStrings("MyDB").ConnectionString)
            Dim SQL As String = "Select (CAST(SUM(MajorsCount+RepairsCount+ScrapCount) AS Decimal(10,2))/CAST(SUM(SampleSize) AS Decimal(10,2)))*100 AS TOTAL from dbo.InspectionJobSummaryYearly WHERE CID = " & CID & WS
            Command2.CommandType = CommandType.Text 'sets the type of the sql
            Command2.Connection = Connection2 'sets the connection of our sql command to MyDB
            Command2.CommandText = SQL 'sets the statement that executes at the data source to our string
            If (Connection2.State = ConnectionState.Closed) Then
                Connection2.Open()
            End If
            DR2 = Command2.ExecuteReader 'sends the command text to the connection and builds tthe SqlDataReader
            If DR2.HasRows = True Then 'Check whether the SqlDataReader has 1 or more rows
                DR2.Read() 'The default position of the SqlDataReader is before the first record. Therefore, you must call Read to begin accessing any data.  The reader has moved down to the first row.
                'Response.Write(DR("Cust_Name")) 'Writes a string to an HTTP response output stream.  In this case, just Cust_Name

                If Not DR2.IsDBNull(DR2.GetOrdinal("Total")) Then
                    retval = Math.Round(Convert.ToDouble(DR2("Total")), 2)
                End If

            Else
                Return retval


            End If

            Connection2.Close() 'closes the connection
            DR2.Close() 'closes the reader
            Return retval
        End Function
        Public Function GetGlobalYearlyRejectionRate(ByVal WS As String) As Double
            Dim retval As Double = 0
            Dim DR2 As SqlDataReader
            Dim Command2 As New SqlCommand
            Dim Connection2 As New SqlConnection(ConfigurationManager.ConnectionStrings("MyDB").ConnectionString)
            Dim SQL As String = "Select (CAST(SUM(MajorsCount+RepairsCount+ScrapCount) AS Decimal(10,2))/CAST(SUM(SampleSize) AS Decimal(10,2)))*100 AS TOTAL from dbo.InspectionJobSummaryYearly " & WS
            Command2.CommandType = CommandType.Text 'sets the type of the sql
            Command2.Connection = Connection2 'sets the connection of our sql command to MyDB
            Command2.CommandText = SQL 'sets the statement that executes at the data source to our string
            If (Connection2.State = ConnectionState.Closed) Then
                Connection2.Open()
            End If
            DR2 = Command2.ExecuteReader 'sends the command text to the connection and builds tthe SqlDataReader
            If DR2.HasRows = True Then 'Check whether the SqlDataReader has 1 or more rows
                DR2.Read() 'The default position of the SqlDataReader is before the first record. Therefore, you must call Read to begin accessing any data.  The reader has moved down to the first row.
                'Response.Write(DR("Cust_Name")) 'Writes a string to an HTTP response output stream.  In this case, just Cust_Name

                If Not DR2.IsDBNull(DR2.GetOrdinal("Total")) Then
                    retval = Math.Round(Convert.ToDouble(DR2("Total")), 2)
                End If

            Else
                Return retval


            End If

            Connection2.Close() 'closes the connection
            DR2.Close() 'closes the reader
            Return retval
        End Function
        Public Function GetCustomRejectionRate(ByVal CID As Integer, ByVal WS As String) As Double
            Dim retval As Double = 0
            Dim DR2 As SqlDataReader
            Dim Command2 As New SqlCommand
            Dim Connection2 As New SqlConnection(ConfigurationManager.ConnectionStrings("MyDB").ConnectionString)
            Dim SQL As String = "Select (CAST(SUM(MajorsCount+RepairsCount+ScrapCount) AS Decimal(10,2))/CAST(SUM(SampleSize) AS Decimal(10,2)))*100 AS TOTAL from dbo.InspectionJobSummaryYearly WHERE CID = " & CID & WS
            Command2.CommandType = CommandType.Text 'sets the type of the sql
            Command2.Connection = Connection2 'sets the connection of our sql command to MyDB
            Command2.CommandText = SQL 'sets the statement that executes at the data source to our string
            If (Connection2.State = ConnectionState.Closed) Then
                Connection2.Open()
            End If
            DR2 = Command2.ExecuteReader 'sends the command text to the connection and builds tthe SqlDataReader
            If DR2.HasRows = True Then 'Check whether the SqlDataReader has 1 or more rows
                DR2.Read() 'The default position of the SqlDataReader is before the first record. Therefore, you must call Read to begin accessing any data.  The reader has moved down to the first row.
                'Response.Write(DR("Cust_Name")) 'Writes a string to an HTTP response output stream.  In this case, just Cust_Name

                If Not DR2.IsDBNull(DR2.GetOrdinal("Total")) Then
                    retval = Math.Round(Convert.ToDouble(DR2("Total")), 2)
                End If

            Else
                Return retval


            End If

            Connection2.Close() 'closes the connection
            DR2.Close() 'closes the reader
            Return retval
        End Function
        Public Function GetGlobalCustomRejectionRate(ByVal WS As String) As Double
            Dim retval As Double = 0
            Dim DR2 As SqlDataReader
            Dim Command2 As New SqlCommand
            Dim Connection2 As New SqlConnection(ConfigurationManager.ConnectionStrings("MyDB").ConnectionString)
            Dim SQL As String = "Select (CAST(SUM(MajorsCount+RepairsCount+ScrapCount) AS Decimal(10,2))/CAST(SUM(SampleSize) AS Decimal(10,2)))*100 AS TOTAL from dbo.InspectionJobSummaryYearly " & WS
            Command2.CommandType = CommandType.Text 'sets the type of the sql
            Command2.Connection = Connection2 'sets the connection of our sql command to MyDB
            Command2.CommandText = SQL 'sets the statement that executes at the data source to our string
            If (Connection2.State = ConnectionState.Closed) Then
                Connection2.Open()
            End If
            DR2 = Command2.ExecuteReader 'sends the command text to the connection and builds tthe SqlDataReader
            If DR2.HasRows = True Then 'Check whether the SqlDataReader has 1 or more rows
                DR2.Read() 'The default position of the SqlDataReader is before the first record. Therefore, you must call Read to begin accessing any data.  The reader has moved down to the first row.
                'Response.Write(DR("Cust_Name")) 'Writes a string to an HTTP response output stream.  In this case, just Cust_Name

                If Not DR2.IsDBNull(DR2.GetOrdinal("Total")) Then
                    retval = Math.Round(Convert.ToDouble(DR2("Total")), 2)
                End If

            Else
                Return retval


            End If

            Connection2.Close() 'closes the connection
            DR2.Close() 'closes the reader
            Return retval
        End Function
        Public Function GetMonthlyLotAcceptance(ByVal CID As Integer, ByVal WS As String) As String
            Dim retval As String = "'0.00%'"
            Dim DR2 As SqlDataReader
            Dim Command2 As New SqlCommand
            Dim Connection2 As New SqlConnection(ConfigurationManager.ConnectionStrings("MyDB").ConnectionString)
            Dim SQL As String = "Select Top (1) (CAST( (Select COUNT(id) from dbo.InspectionJobSummaryYearly WHERE CID = " & CID & WS & " AND Technical_PassFail=1 AND Inspection_Finished >= DATEADD(month,-1,GETDATE())) AS Decimal(10,2))/CAST((Select COUNT(id) from dbo.InspectionJobSummaryYearly WHERE CID = " & CID & WS & " AND Inspection_Finished >= DATEADD(month,-1,GETDATE())) AS Decimal(10,2)))*100 AS TOTAL from dbo.InspectionJobSummaryYearly"
            Command2.CommandType = CommandType.Text 'sets the type of the sql
            Command2.Connection = Connection2 'sets the connection of our sql command to MyDB
            Command2.CommandText = SQL 'sets the statement that executes at the data source to our string
            If (Connection2.State = ConnectionState.Closed) Then
                Connection2.Open()
            End If
            DR2 = Command2.ExecuteReader 'sends the command text to the connection and builds tthe SqlDataReader
            If DR2.HasRows = True Then 'Check whether the SqlDataReader has 1 or more rows
                DR2.Read() 'The default position of the SqlDataReader is before the first record. Therefore, you must call Read to begin accessing any data.  The reader has moved down to the first row.
                'Response.Write(DR("Cust_Name")) 'Writes a string to an HTTP response output stream.  In this case, just Cust_Name
                If Not DR2.IsDBNull(DR2.GetOrdinal("Total")) Then
                    retval = "'" + Convert.ToString(Math.Round(Convert.ToDouble(DR2("Total")), 2)) + "%'"
                End If


            Else
                Return retval


            End If

            Connection2.Close() 'closes the connection
            DR2.Close() 'closes the reader
            Return retval
        End Function
        Public Function GetGlobalMonthlyLotAcceptance(ByVal WS As String) As String
            Dim retval As String = "'0.00%'"
            Dim DR2 As SqlDataReader
            Dim Command2 As New SqlCommand
            Dim Connection2 As New SqlConnection(ConfigurationManager.ConnectionStrings("MyDB").ConnectionString)
            Dim SQL As String = "Select Top (1) (CAST( (Select COUNT(id) from dbo.InspectionJobSummaryYearly " & WS & " AND Technical_PassFail=1 AND Inspection_Finished >= DATEADD(month,-1,GETDATE())) AS Decimal(10,2))/CAST((Select COUNT(id) from dbo.InspectionJobSummaryYearly " & WS & " AND Inspection_Finished >= DATEADD(month,-1,GETDATE())) AS Decimal(10,2)))*100 AS TOTAL from dbo.InspectionJobSummaryYearly"
            Command2.CommandType = CommandType.Text 'sets the type of the sql
            Command2.Connection = Connection2 'sets the connection of our sql command to MyDB
            Command2.CommandText = SQL 'sets the statement that executes at the data source to our string
            If (Connection2.State = ConnectionState.Closed) Then
                Connection2.Open()
            End If
            DR2 = Command2.ExecuteReader 'sends the command text to the connection and builds tthe SqlDataReader
            If DR2.HasRows = True Then 'Check whether the SqlDataReader has 1 or more rows
                DR2.Read() 'The default position of the SqlDataReader is before the first record. Therefore, you must call Read to begin accessing any data.  The reader has moved down to the first row.
                'Response.Write(DR("Cust_Name")) 'Writes a string to an HTTP response output stream.  In this case, just Cust_Name
                If Not DR2.IsDBNull(DR2.GetOrdinal("Total")) Then
                    retval = "'" + Convert.ToString(Math.Round(Convert.ToDouble(DR2("Total")), 2)) + "%'"
                End If


            Else
                Return retval


            End If

            Connection2.Close() 'closes the connection
            DR2.Close() 'closes the reader
            Return retval
        End Function
        Public Function GetYearlyLotAcceptance(ByVal CID As Integer, ByVal WS As String) As String
            Dim retval As String = "'0.00%'"
            Dim DR2 As SqlDataReader
            Dim Command2 As New SqlCommand
            Dim Connection2 As New SqlConnection(ConfigurationManager.ConnectionStrings("MyDB").ConnectionString)
            Dim SQL As String = "Select Top (1) (CAST( (Select COUNT(id) from dbo.InspectionJobSummaryYearly WHERE CID = " & CID & WS & " AND Technical_PassFail=1 ) AS Decimal(10,2))/CAST((Select COUNT(id) from dbo.InspectionJobSummaryYearly WHERE CID = " & CID & WS & ") AS Decimal(10,2)))*100 AS TOTAL from dbo.InspectionJobSummaryYearly"
            Command2.CommandType = CommandType.Text 'sets the type of the sql
            Command2.Connection = Connection2 'sets the connection of our sql command to MyDB
            Command2.CommandText = SQL 'sets the statement that executes at the data source to our string
            If (Connection2.State = ConnectionState.Closed) Then
                Connection2.Open()
            End If
            DR2 = Command2.ExecuteReader 'sends the command text to the connection and builds tthe SqlDataReader
            If DR2.HasRows = True Then 'Check whether the SqlDataReader has 1 or more rows
                DR2.Read() 'The default position of the SqlDataReader is before the first record. Therefore, you must call Read to begin accessing any data.  The reader has moved down to the first row.
                'Response.Write(DR("Cust_Name")) 'Writes a string to an HTTP response output stream.  In this case, just Cust_Name

                If Not DR2.IsDBNull(DR2.GetOrdinal("Total")) Then
                    retval = "'" + Convert.ToString(Math.Round(Convert.ToDouble(DR2("Total")), 2)) + "%'"
                End If

            Else
                Return retval


            End If

            Connection2.Close() 'closes the connection
            DR2.Close() 'closes the reader
            Return retval
        End Function
        Public Function GetGlobalYearlyLotAcceptance(ByVal WS As String) As String
            Dim retval As String = "'0.00%'"
            Dim DR2 As SqlDataReader
            Dim Command2 As New SqlCommand
            Dim Connection2 As New SqlConnection(ConfigurationManager.ConnectionStrings("MyDB").ConnectionString)
            Dim SQL As String = "Select Top (1) (CAST( (Select COUNT(id) from dbo.InspectionJobSummaryYearly " & WS & " AND Technical_PassFail=1 ) AS Decimal(10,2))/CAST((Select COUNT(id) from dbo.InspectionJobSummaryYearly " & WS & ") AS Decimal(10,2)))*100 AS TOTAL from dbo.InspectionJobSummaryYearly"
            Command2.CommandType = CommandType.Text 'sets the type of the sql
            Command2.Connection = Connection2 'sets the connection of our sql command to MyDB
            Command2.CommandText = SQL 'sets the statement that executes at the data source to our string
            If (Connection2.State = ConnectionState.Closed) Then
                Connection2.Open()
            End If
            DR2 = Command2.ExecuteReader 'sends the command text to the connection and builds tthe SqlDataReader
            If DR2.HasRows = True Then 'Check whether the SqlDataReader has 1 or more rows
                DR2.Read() 'The default position of the SqlDataReader is before the first record. Therefore, you must call Read to begin accessing any data.  The reader has moved down to the first row.
                'Response.Write(DR("Cust_Name")) 'Writes a string to an HTTP response output stream.  In this case, just Cust_Name

                If Not DR2.IsDBNull(DR2.GetOrdinal("Total")) Then
                    retval = "'" + Convert.ToString(Math.Round(Convert.ToDouble(DR2("Total")), 2)) + "%'"
                End If

            Else
                Return retval


            End If

            Connection2.Close() 'closes the connection
            DR2.Close() 'closes the reader
            Return retval
        End Function
        Public Function GetCustomLotAcceptance(ByVal CID As Integer, ByVal WS As String) As String
            Dim retval As String = "'0.00%'"
            Dim DR2 As SqlDataReader
            Dim Command2 As New SqlCommand
            Dim Connection2 As New SqlConnection(ConfigurationManager.ConnectionStrings("MyDB").ConnectionString)
            Dim SQL As String = "Select Top (1) (CAST( (Select COUNT(id) from dbo.InspectionJobSummaryYearly WHERE CID = " & CID & WS & ") AS Decimal(10,2))/CAST((Select COUNT(id) from dbo.InspectionJobSummaryYearly WHERE CID = " & CID & WS & ") AS Decimal(10,2)))*100 AS TOTAL from dbo.InspectionJobSummaryYearly"
            Command2.CommandType = CommandType.Text 'sets the type of the sql
            Command2.Connection = Connection2 'sets the connection of our sql command to MyDB
            Command2.CommandText = SQL 'sets the statement that executes at the data source to our string
            If (Connection2.State = ConnectionState.Closed) Then
                Connection2.Open()
            End If
            DR2 = Command2.ExecuteReader 'sends the command text to the connection and builds tthe SqlDataReader
            If DR2.HasRows = True Then 'Check whether the SqlDataReader has 1 or more rows
                DR2.Read() 'The default position of the SqlDataReader is before the first record. Therefore, you must call Read to begin accessing any data.  The reader has moved down to the first row.
                'Response.Write(DR("Cust_Name")) 'Writes a string to an HTTP response output stream.  In this case, just Cust_Name

                If Not DR2.IsDBNull(DR2.GetOrdinal("Total")) Then
                    retval = "'" + Convert.ToString(Math.Round(Convert.ToDouble(DR2("Total")), 2)) + "%'"
                End If

            Else
                Return retval


            End If

            Connection2.Close() 'closes the connection
            DR2.Close() 'closes the reader
            Return retval
        End Function

        Public Function GetGlobalCustomLotAcceptance(ByVal WS As String) As String
            Dim retval As String = "'0.00%'"
            Dim DR2 As SqlDataReader
            Dim Command2 As New SqlCommand
            Dim Connection2 As New SqlConnection(ConfigurationManager.ConnectionStrings("MyDB").ConnectionString)
            Dim SQL As String = "Select Top (1) (CAST( (Select COUNT(id) from dbo.InspectionJobSummaryYearly " & WS & ") AS Decimal(10,2))/CAST((Select COUNT(id) from dbo.InspectionJobSummaryYearly " & WS & ") AS Decimal(10,2)))*100 AS TOTAL from dbo.InspectionJobSummaryYearly"
            Command2.CommandType = CommandType.Text 'sets the type of the sql
            Command2.Connection = Connection2 'sets the connection of our sql command to MyDB
            Command2.CommandText = SQL 'sets the statement that executes at the data source to our string
            If (Connection2.State = ConnectionState.Closed) Then
                Connection2.Open()
            End If
            DR2 = Command2.ExecuteReader 'sends the command text to the connection and builds tthe SqlDataReader
            If DR2.HasRows = True Then 'Check whether the SqlDataReader has 1 or more rows
                DR2.Read() 'The default position of the SqlDataReader is before the first record. Therefore, you must call Read to begin accessing any data.  The reader has moved down to the first row.
                'Response.Write(DR("Cust_Name")) 'Writes a string to an HTTP response output stream.  In this case, just Cust_Name

                If Not DR2.IsDBNull(DR2.GetOrdinal("Total")) Then
                    retval = "'" + Convert.ToString(Math.Round(Convert.ToDouble(DR2("Total")), 2)) + "%'"
                End If

            Else
                Return retval


            End If

            Connection2.Close() 'closes the connection
            DR2.Close() 'closes the reader
            Return retval
        End Function
        Public Function GetDataArray(ByVal array As List(Of Integer), ByVal fromDate As String, ByVal toDate As String, ByVal DN As String, ByVal WO As String, ByVal AT As String) As List(Of String)
            '{ id: "1", Facility: "Thomaston", Time_Period: "Past 30 Days", No_of_Defects: 100, No_of_Rejects: 1, No_of_Inspections: 10, No_of_Rejected_Lots: 12, DHU: 0.55, Reject_Rate: '25%', Lot_Acceptance: '91.3%', attr: { Facility: { rowspan: "3" } } },
            Dim retval As New List(Of String)()
            Dim startDate As DateTime = Convert.ToDateTime(fromDate)
            Dim oDate As DateTime = Convert.ToDateTime(toDate)
            Dim WhereString2 As String = ""
            If AT = "ALL" Then
                WhereString2 = WhereString2 & " AND InspectionType != 'ROLL'"
            ElseIf AT = "FINAL AUDIT" Then
                WhereString2 = WhereString2 & " AND InspectionType = 'EOL'"
            ElseIf AT = "IN LINE" Then
                WhereString2 = WhereString2 & " AND InspectionType = 'IL'"
            Else
                WhereString2 = WhereString2 & " AND InspectionType = '" & AT & "'"
            End If
            If DN <> "ALL" Then
                WhereString2 = WhereString2 & " AND DataNo = '" & DN & "'"
            End If
            If WO <> "ALL" Then
                WhereString2 = WhereString2 & " AND JobNumber = '" & WO & "'"
            End If
            Dim WhereString As String = " AND Inspection_Finished BETWEEN '" & startDate & "' AND '" & oDate & "'"
            If AT = "ALL" Then
                WhereString = WhereString & " AND InspectionType != 'ROLL'"
            ElseIf AT = "FINAL AUDIT" Then
                WhereString = WhereString & " AND InspectionType = 'EOL'"
            ElseIf AT = "IN LINE" Then
                WhereString = WhereString & " AND InspectionType = 'IL'"
            Else
                WhereString = WhereString & " AND InspectionType = '" & AT & "'"
            End If
            If DN <> "ALL" Then
                WhereString = WhereString & " AND DataNo = '" & DN & "'"
            End If
            If WO <> "ALL" Then
                WhereString = WhereString & " AND JobNumber = '" & WO & "'"
            End If

            For Each CID As Integer In array



                Dim SQL As String = "Select Name from Inspection.dbo.Locations Where NCID=" & CID
                Command.CommandType = CommandType.Text
                Command.Connection = Connection
                Command.CommandText = SQL
                Command.CommandTimeout = 0
                Connection.Open()
                DR = Command.ExecuteReader
                If DR.HasRows = True Then
                    DR.Read()
                    Dim Top As String = String.Format("Facility: '{0}', Time_Period: 'Past 30 Days', No_of_Defects: '{1}', No_of_Rejects: '{3}', No_of_Inspections: '{2}', No_of_Rejected_Lots: '{4}', DHU: '{5}', Reject_Rate: '{6}', Lot_Acceptance: {7}, attr: {{ Facility: {{ rowspan: '3' }} }}", DR.GetString(0), GetMonthlyDefectTotal(CID, WhereString2), GetMonthlyInspectionTotal(CID, WhereString2), GetMonthlyRejectTotal(CID, WhereString2), GetMonthlyRejectLotTotal(CID, WhereString2), GetMonthlyDHU(CID, WhereString2), GetMonthlyRejectionRate(CID, WhereString2), GetMonthlyLotAcceptance(CID, WhereString2))
                    retval.Add("{" + Top + "}")
                    Dim Mid As String = String.Format("Facility: '{0}', Time_Period: 'Past Year', No_of_Defects: '{1}', No_of_Rejects: '{3}', No_of_Inspections: '{2}', No_of_Rejected_Lots: '{4}', DHU: '{5}', Reject_Rate: '{6}', Lot_Acceptance: {7}, attr: {{ Facility: {{ display: 'none' }} }}", DR.GetString(0), GetYearlyDefectTotal(CID, WhereString2), GetYearlyInspectionTotal(CID, WhereString2), GetYearlyRejectTotal(CID, WhereString2), GetYearlyRejectLotTotal(CID, WhereString2), GetYearlyDHU(CID, WhereString2), GetYearlyRejectionRate(CID, WhereString2), GetYearlyLotAcceptance(CID, WhereString2))
                    retval.Add("{" + Mid + "}")
                    Dim Bottom As String = String.Format("Facility: '{0}', Time_Period: 'Custom', No_of_Defects: '{1}', No_of_Rejects: '{3}', No_of_Inspections: '{2}', No_of_Rejected_Lots: '{4}', DHU: '{5}', Reject_Rate: '{6}', Lot_Acceptance: {7}, attr: {{ Facility: {{ display: 'none' }} }}", DR.GetString(0), GetCustomDefectTotal(CID, WhereString), GetCustomInspectionTotal(CID, WhereString), GetCustomRejectTotal(CID, WhereString), GetCustomRejectLotTotal(CID, WhereString), GetCustomDHU(CID, WhereString), GetCustomRejectionRate(CID, WhereString), GetCustomLotAcceptance(CID, WhereString))
                    retval.Add("{" + Bottom + "}")
                End If
                Connection.Close()
                DR.Close()
            Next

            retval = retval.Distinct().ToList
            Return retval
        End Function
        Public Function GetOverallDataArray(ByVal fromDate As String, ByVal toDate As String, ByVal DN As String, ByVal WO As String, ByVal AT As String) As List(Of String)
            '{ id: "1", Facility: "Thomaston", Time_Period: "Past 30 Days", No_of_Defects: 100, No_of_Rejects: 1, No_of_Inspections: 10, No_of_Rejected_Lots: 12, DHU: 0.55, Reject_Rate: '25%', Lot_Acceptance: '91.3%', attr: { Facility: { rowspan: "3" } } },
            Dim retval As New List(Of String)()
            Dim startDate As DateTime = Convert.ToDateTime(fromDate)
            Dim oDate As DateTime = Convert.ToDateTime(toDate)
            Dim WhereString2 As String = "WHERE CID IS NOT NULL"
            If AT = "ALL" Then
                WhereString2 = WhereString2 & " AND InspectionType != 'ROLL'"
            ElseIf AT = "FINAL AUDIT" Then
                WhereString2 = WhereString2 & " AND InspectionType = 'EOL'"
            ElseIf AT = "IN LINE" Then
                WhereString2 = WhereString2 & " AND InspectionType = 'IL'"
            Else
                WhereString2 = WhereString2 & " AND InspectionType = '" & AT & "'"
            End If
            If DN <> "ALL" Then
                WhereString2 = WhereString2 & " AND DataNo = '" & DN & "'"
            End If
            If WO <> "ALL" Then
                WhereString2 = WhereString2 & " AND JobNumber = '" & WO & "'"
            End If
            Dim WhereString As String = "WHERE CID IS NOT NULL AND Inspection_Finished BETWEEN '" & startDate & "' AND '" & oDate & "'"
            If AT = "ALL" Then
                WhereString = WhereString & " AND InspectionType != 'ROLL'"
            ElseIf AT = "FINAL AUDIT" Then
                WhereString = WhereString & " AND InspectionType = 'EOL'"
            ElseIf AT = "IN LINE" Then
                WhereString = WhereString & " AND InspectionType = 'IL'"
            Else
                WhereString = WhereString & " AND InspectionType = '" & AT & "'"
            End If
            If DN <> "ALL" Then
                WhereString = WhereString & " AND DataNo = '" & DN & "'"
            End If
            If WO <> "ALL" Then
                WhereString = WhereString & " AND JobNumber = '" & WO & "'"
            End If
            Dim WhereString3 As String = "WHERE CID IS NOT NULL And Inspection_Finished >= dateadd(day,datediff(day,1,GETDATE()),0) AND Inspection_Finished < dateadd(day,datediff(day,0,GETDATE()),0)"
            If AT = "ALL" Then
                WhereString3 = WhereString3 & " AND InspectionType != 'ROLL'"
            ElseIf AT = "FINAL AUDIT" Then
                WhereString2 = WhereString2 & " AND InspectionType = 'EOL'"
            ElseIf AT = "IN LINE" Then
                WhereString3 = WhereString3 & " AND InspectionType = 'IL'"
            Else
                WhereString3 = WhereString3 & " AND InspectionType = '" & AT & "'"
            End If
            If DN <> "ALL" Then
                WhereString3 = WhereString3 & " AND DataNo = '" & DN & "'"
            End If
            If WO <> "ALL" Then
                WhereString3 = WhereString3 & " AND JobNumber = '" & WO & "'"
            End If





            Dim Top As String = String.Format("Facility: '{0}', Time_Period: 'Past 30 Days', No_of_Defects: '{1}', No_of_Rejects: '{3}', No_of_Inspections: '{2}', No_of_Rejected_Lots: '{4}', DHU: '{5}', Reject_Rate: '{6}', Lot_Acceptance: {7}, attr: {{ Facility: {{ rowspan: '4' }} }}", "Overall", GetGlobalMonthlyDefectTotal(WhereString2), GetGlobalMonthlyInspectionTotal(WhereString2), GetGlobalMonthlyRejectTotal(WhereString2), GetGlobalMonthlyRejectLotTotal(WhereString2), GetGlobalMonthlyDHU(WhereString2), GetGlobalMonthlyRejectionRate(WhereString2), GetGlobalMonthlyLotAcceptance(WhereString2))
            retval.Add("{" + Top + "}")
            Dim Mid As String = String.Format("Facility: '{0}', Time_Period: 'Past Year', No_of_Defects: '{1}', No_of_Rejects: '{3}', No_of_Inspections: '{2}', No_of_Rejected_Lots: '{4}', DHU: '{5}', Reject_Rate: '{6}', Lot_Acceptance: {7}, attr: {{ Facility: {{ display: 'none' }} }}", "Overall", GetGlobalYearlyDefectTotal(WhereString2), GetGlobalYearlyInspectionTotal(WhereString2), GetGlobalYearlyRejectTotal(WhereString2), GetGlobalYearlyRejectLotTotal(WhereString2), GetGlobalYearlyDHU(WhereString2), GetGlobalYearlyRejectionRate(WhereString2), GetGlobalYearlyLotAcceptance(WhereString2))
            retval.Add("{" + Mid + "}")
            Dim Yesterday As String = String.Format("Facility: '{0}', Time_Period: 'Yesterday', No_of_Defects: '{1}', No_of_Rejects: '{3}', No_of_Inspections: '{2}', No_of_Rejected_Lots: '{4}', DHU: '{5}', Reject_Rate: '{6}', Lot_Acceptance: {7}, attr: {{ Facility: {{ display: 'none' }} }}", "Overall", GetGlobalYearlyDefectTotal(WhereString3), GetGlobalYearlyInspectionTotal(WhereString3), GetGlobalYearlyRejectTotal(WhereString3), GetGlobalYearlyRejectLotTotal(WhereString3), GetGlobalYearlyDHU(WhereString3), GetGlobalYearlyRejectionRate(WhereString3), GetGlobalYearlyLotAcceptance(WhereString3))
            retval.Add("{" + Yesterday + "}")
            Dim Bottom As String = String.Format("Facility: '{0}', Time_Period: 'Custom', No_of_Defects: '{1}', No_of_Rejects: '{3}', No_of_Inspections: '{2}', No_of_Rejected_Lots: '{4}', DHU: '{5}', Reject_Rate: '{6}', Lot_Acceptance: {7}, attr: {{ Facility: {{ display: 'none' }} }}", "Overall", GetGlobalCustomDefectTotal(WhereString), GetGlobalCustomInspectionTotal(WhereString), GetGlobalCustomRejectTotal(WhereString), GetGlobalCustomRejectLotTotal(WhereString), GetGlobalCustomDHU(WhereString), GetGlobalCustomRejectionRate(WhereString), GetGlobalCustomLotAcceptance(WhereString))
            retval.Add("{" + Bottom + "}")




            retval = retval.Distinct().ToList
            Return retval
        End Function
        Public Function GetDomesticDataArray(ByVal fromDate As String, ByVal toDate As String, ByVal DN As String, ByVal WO As String, ByVal AT As String) As List(Of String)
            '{ id: "1", Facility: "Thomaston", Time_Period: "Past 30 Days", No_of_Defects: 100, No_of_Rejects: 1, No_of_Inspections: 10, No_of_Rejected_Lots: 12, DHU: 0.55, Reject_Rate: '25%', Lot_Acceptance: '91.3%', attr: { Facility: { rowspan: "3" } } },
            Dim retval As New List(Of String)()
            Dim startDate As DateTime = Convert.ToDateTime(fromDate)
            Dim oDate As DateTime = Convert.ToDateTime(toDate)
            Dim WhereString2 As String = "WHERE (CID = 485 OR CID = 482 OR CID = 578)"
            If AT = "ALL" Then
                WhereString2 = WhereString2 & " AND InspectionType != 'ROLL'"
            ElseIf AT = "FINAL AUDIT" Then
                WhereString2 = WhereString2 & " AND InspectionType = 'EOL'"
            ElseIf AT = "IN LINE" Then
                WhereString2 = WhereString2 & " AND InspectionType = 'IL'"
            Else
                WhereString2 = WhereString2 & " AND InspectionType = '" & AT & "'"
            End If
            If DN <> "ALL" Then
                WhereString2 = WhereString2 & " AND DataNo = '" & DN & "'"
            End If
            If WO <> "ALL" Then
                WhereString2 = WhereString2 & " AND JobNumber = '" & WO & "'"
            End If
            Dim WhereString As String = "WHERE (CID = 485 OR CID = 482 OR CID = 578) AND Inspection_Finished BETWEEN '" & startDate & "' AND '" & oDate & "'"
            If AT = "ALL" Then
                WhereString = WhereString & " AND InspectionType != 'ROLL'"
            ElseIf AT = "FINAL AUDIT" Then
                WhereString = WhereString & " AND InspectionType = 'EOL'"
            ElseIf AT = "IN LINE" Then
                WhereString = WhereString & " AND InspectionType = 'IL'"
            Else
                WhereString = WhereString & " AND InspectionType = '" & AT & "'"
            End If
            If DN <> "ALL" Then
                WhereString = WhereString & " AND DataNo = '" & DN & "'"
            End If
            If WO <> "ALL" Then
                WhereString = WhereString & " AND JobNumber = '" & WO & "'"
            End If
            Dim WhereString3 As String = "WHERE (CID = 485 OR CID = 482 OR CID = 578) And Inspection_Finished >= dateadd(day,datediff(day,1,GETDATE()),0) AND Inspection_Finished < dateadd(day,datediff(day,0,GETDATE()),0)"
            If AT = "ALL" Then
                WhereString3 = WhereString3 & " AND InspectionType != 'ROLL'"
            ElseIf AT = "FINAL AUDIT" Then
                WhereString2 = WhereString2 & " AND InspectionType = 'EOL'"
            ElseIf AT = "IN LINE" Then
                WhereString3 = WhereString3 & " AND InspectionType = 'IL'"
            Else
                WhereString3 = WhereString3 & " AND InspectionType = '" & AT & "'"
            End If
            If DN <> "ALL" Then
                WhereString3 = WhereString3 & " AND DataNo = '" & DN & "'"
            End If
            If WO <> "ALL" Then
                WhereString3 = WhereString3 & " AND JobNumber = '" & WO & "'"
            End If





            Dim Top As String = String.Format("Facility: '{0}', Time_Period: 'Past 30 Days', No_of_Defects: '{1}', No_of_Rejects: '{3}', No_of_Inspections: '{2}', No_of_Rejected_Lots: '{4}', DHU: '{5}', Reject_Rate: '{6}', Lot_Acceptance: {7}, attr: {{ Facility: {{ rowspan: '4' }} }}", "Domestic", GetGlobalMonthlyDefectTotal(WhereString2), GetGlobalMonthlyInspectionTotal(WhereString2), GetGlobalMonthlyRejectTotal(WhereString2), GetGlobalMonthlyRejectLotTotal(WhereString2), GetGlobalMonthlyDHU(WhereString2), GetGlobalMonthlyRejectionRate(WhereString2), GetGlobalMonthlyLotAcceptance(WhereString2))
            retval.Add("{" + Top + "}")
            Dim Mid As String = String.Format("Facility: '{0}', Time_Period: 'Past Year', No_of_Defects: '{1}', No_of_Rejects: '{3}', No_of_Inspections: '{2}', No_of_Rejected_Lots: '{4}', DHU: '{5}', Reject_Rate: '{6}', Lot_Acceptance: {7}, attr: {{ Facility: {{ display: 'none' }} }}", "Domestic", GetGlobalYearlyDefectTotal(WhereString2), GetGlobalYearlyInspectionTotal(WhereString2), GetGlobalYearlyRejectTotal(WhereString2), GetGlobalYearlyRejectLotTotal(WhereString2), GetGlobalYearlyDHU(WhereString2), GetGlobalYearlyRejectionRate(WhereString2), GetGlobalYearlyLotAcceptance(WhereString2))
            retval.Add("{" + Mid + "}")
            Dim Yesterday As String = String.Format("Facility: '{0}', Time_Period: 'Yesterday', No_of_Defects: '{1}', No_of_Rejects: '{3}', No_of_Inspections: '{2}', No_of_Rejected_Lots: '{4}', DHU: '{5}', Reject_Rate: '{6}', Lot_Acceptance: {7}, attr: {{ Facility: {{ display: 'none' }} }}", "Domestic", GetGlobalYearlyDefectTotal(WhereString3), GetGlobalYearlyInspectionTotal(WhereString3), GetGlobalYearlyRejectTotal(WhereString3), GetGlobalYearlyRejectLotTotal(WhereString3), GetGlobalYearlyDHU(WhereString3), GetGlobalYearlyRejectionRate(WhereString3), GetGlobalYearlyLotAcceptance(WhereString3))
            retval.Add("{" + Yesterday + "}")
            Dim Bottom As String = String.Format("Facility: '{0}', Time_Period: 'Custom', No_of_Defects: '{1}', No_of_Rejects: '{3}', No_of_Inspections: '{2}', No_of_Rejected_Lots: '{4}', DHU: '{5}', Reject_Rate: '{6}', Lot_Acceptance: {7}, attr: {{ Facility: {{ display: 'none' }} }}", "Domestic", GetGlobalCustomDefectTotal(WhereString), GetGlobalCustomInspectionTotal(WhereString), GetGlobalCustomRejectTotal(WhereString), GetGlobalCustomRejectLotTotal(WhereString), GetGlobalCustomDHU(WhereString), GetGlobalCustomRejectionRate(WhereString), GetGlobalCustomLotAcceptance(WhereString))
            retval.Add("{" + Bottom + "}")




            retval = retval.Distinct().ToList
            Return retval
        End Function
        Public Function GetInteriorsDataArray(ByVal fromDate As String, ByVal toDate As String, ByVal DN As String, ByVal WO As String, ByVal AT As String) As List(Of String)
            '{ id: "1", Facility: "Thomaston", Time_Period: "Past 30 Days", No_of_Defects: 100, No_of_Rejects: 1, No_of_Inspections: 10, No_of_Rejected_Lots: 12, DHU: 0.55, Reject_Rate: '25%', Lot_Acceptance: '91.3%', attr: { Facility: { rowspan: "3" } } },
            Dim retval As New List(Of String)()
            Dim startDate As DateTime = Convert.ToDateTime(fromDate)
            Dim oDate As DateTime = Convert.ToDateTime(toDate)
            Dim WhereString2 As String = "WHERE (CID = 111 OR CID = 112)"
            If AT = "ALL" Then
                WhereString2 = WhereString2 & " AND InspectionType != 'ROLL'"
            ElseIf AT = "FINAL AUDIT" Then
                WhereString2 = WhereString2 & " AND InspectionType = 'EOL'"
            ElseIf AT = "IN LINE" Then
                WhereString2 = WhereString2 & " AND InspectionType = 'IL'"
            Else
                WhereString2 = WhereString2 & " AND InspectionType = '" & AT & "'"
            End If
            If DN <> "ALL" Then
                WhereString2 = WhereString2 & " AND DataNo = '" & DN & "'"
            End If
            If WO <> "ALL" Then
                WhereString2 = WhereString2 & " AND JobNumber = '" & WO & "'"
            End If
            Dim WhereString As String = "WHERE (CID = 111 OR CID = 112) AND Inspection_Finished BETWEEN '" & startDate & "' AND '" & oDate & "'"
            If AT = "ALL" Then
                WhereString = WhereString & " AND InspectionType != 'ROLL'"
            ElseIf AT = "FINAL AUDIT" Then
                WhereString = WhereString & " AND InspectionType = 'EOL'"
            ElseIf AT = "IN LINE" Then
                WhereString = WhereString & " AND InspectionType = 'IL'"
            Else
                WhereString = WhereString & " AND InspectionType = '" & AT & "'"
            End If
            If DN <> "ALL" Then
                WhereString = WhereString & " AND DataNo = '" & DN & "'"
            End If
            If WO <> "ALL" Then
                WhereString = WhereString & " AND JobNumber = '" & WO & "'"
            End If
            Dim WhereString3 As String = "WHERE (CID = 111 OR CID = 112) And Inspection_Finished >= dateadd(day,datediff(day,1,GETDATE()),0) AND Inspection_Finished < dateadd(day,datediff(day,0,GETDATE()),0)"
            If AT = "ALL" Then
                WhereString3 = WhereString3 & " AND InspectionType != 'ROLL'"
            ElseIf AT = "FINAL AUDIT" Then
                WhereString2 = WhereString2 & " AND InspectionType = 'EOL'"
            ElseIf AT = "IN LINE" Then
                WhereString3 = WhereString3 & " AND InspectionType = 'IL'"
            Else
                WhereString3 = WhereString3 & " AND InspectionType = '" & AT & "'"
            End If
            If DN <> "ALL" Then
                WhereString3 = WhereString3 & " AND DataNo = '" & DN & "'"
            End If
            If WO <> "ALL" Then
                WhereString3 = WhereString3 & " AND JobNumber = '" & WO & "'"
            End If





            Dim Top As String = String.Format("Facility: '{0}', Time_Period: 'Past 30 Days', No_of_Defects: '{1}', No_of_Rejects: '{3}', No_of_Inspections: '{2}', No_of_Rejected_Lots: '{4}', DHU: '{5}', Reject_Rate: '{6}', Lot_Acceptance: {7}, attr: {{ Facility: {{ rowspan: '4' }} }}", "Interiors", GetGlobalMonthlyDefectTotal(WhereString2), GetGlobalMonthlyInspectionTotal(WhereString2), GetGlobalMonthlyRejectTotal(WhereString2), GetGlobalMonthlyRejectLotTotal(WhereString2), GetGlobalMonthlyDHU(WhereString2), GetGlobalMonthlyRejectionRate(WhereString2), GetGlobalMonthlyLotAcceptance(WhereString2))
            retval.Add("{" + Top + "}")
            Dim Mid As String = String.Format("Facility: '{0}', Time_Period: 'Past Year', No_of_Defects: '{1}', No_of_Rejects: '{3}', No_of_Inspections: '{2}', No_of_Rejected_Lots: '{4}', DHU: '{5}', Reject_Rate: '{6}', Lot_Acceptance: {7}, attr: {{ Facility: {{ display: 'none' }} }}", "Interiors", GetGlobalYearlyDefectTotal(WhereString2), GetGlobalYearlyInspectionTotal(WhereString2), GetGlobalYearlyRejectTotal(WhereString2), GetGlobalYearlyRejectLotTotal(WhereString2), GetGlobalYearlyDHU(WhereString2), GetGlobalYearlyRejectionRate(WhereString2), GetGlobalYearlyLotAcceptance(WhereString2))
            retval.Add("{" + Mid + "}")
            Dim Yesterday As String = String.Format("Facility: '{0}', Time_Period: 'Yesterday', No_of_Defects: '{1}', No_of_Rejects: '{3}', No_of_Inspections: '{2}', No_of_Rejected_Lots: '{4}', DHU: '{5}', Reject_Rate: '{6}', Lot_Acceptance: {7}, attr: {{ Facility: {{ display: 'none' }} }}", "Interiors", GetGlobalYearlyDefectTotal(WhereString3), GetGlobalYearlyInspectionTotal(WhereString3), GetGlobalYearlyRejectTotal(WhereString3), GetGlobalYearlyRejectLotTotal(WhereString3), GetGlobalYearlyDHU(WhereString3), GetGlobalYearlyRejectionRate(WhereString3), GetGlobalYearlyLotAcceptance(WhereString3))
            retval.Add("{" + Yesterday + "}")
            Dim Bottom As String = String.Format("Facility: '{0}', Time_Period: 'Custom', No_of_Defects: '{1}', No_of_Rejects: '{3}', No_of_Inspections: '{2}', No_of_Rejected_Lots: '{4}', DHU: '{5}', Reject_Rate: '{6}', Lot_Acceptance: {7}, attr: {{ Facility: {{ display: 'none' }} }}", "Interiors", GetGlobalCustomDefectTotal(WhereString), GetGlobalCustomInspectionTotal(WhereString), GetGlobalCustomRejectTotal(WhereString), GetGlobalCustomRejectLotTotal(WhereString), GetGlobalCustomDHU(WhereString), GetGlobalCustomRejectionRate(WhereString), GetGlobalCustomLotAcceptance(WhereString))
            retval.Add("{" + Bottom + "}")




            retval = retval.Distinct().ToList
            Return retval
        End Function
        Public Function GetGlobalDataArray(ByVal fromDate As String, ByVal toDate As String, ByVal DN As String, ByVal WO As String, ByVal AT As String) As List(Of String)
            '{ id: "1", Facility: "Thomaston", Time_Period: "Past 30 Days", No_of_Defects: 100, No_of_Rejects: 1, No_of_Inspections: 10, No_of_Rejected_Lots: 12, DHU: 0.55, Reject_Rate: '25%', Lot_Acceptance: '91.3%', attr: { Facility: { rowspan: "3" } } },
            Dim retval As New List(Of String)()
            Dim startDate As DateTime = Convert.ToDateTime(fromDate)
            Dim oDate As DateTime = Convert.ToDateTime(toDate)
            Dim WhereString2 As String = "WHERE (CID = 114 OR CID = 115 OR CID = 590 OR CID = 1001 OR CID = 627)"
            If AT = "ALL" Then
                WhereString2 = WhereString2 & " AND InspectionType != 'ROLL'"
            ElseIf AT = "FINAL AUDIT" Then
                WhereString2 = WhereString2 & " AND InspectionType = 'EOL'"
            ElseIf AT = "IN LINE" Then
                WhereString2 = WhereString2 & " AND InspectionType = 'IL'"
            Else
                WhereString2 = WhereString2 & " AND InspectionType = '" & AT & "'"
            End If
            If DN <> "ALL" Then
                WhereString2 = WhereString2 & " AND DataNo = '" & DN & "'"
            End If
            If WO <> "ALL" Then
                WhereString2 = WhereString2 & " AND JobNumber = '" & WO & "'"
            End If
            Dim WhereString As String = "WHERE (CID = 114 OR CID = 115 OR CID = 590 OR CID = 1001 OR CID = 627) AND Inspection_Finished BETWEEN '" & startDate & "' AND '" & oDate & "'"
            If AT = "ALL" Then
                WhereString = WhereString & " AND InspectionType != 'ROLL'"
            ElseIf AT = "FINAL AUDIT" Then
                WhereString = WhereString & " AND InspectionType = 'EOL'"
            ElseIf AT = "IN LINE" Then
                WhereString = WhereString & " AND InspectionType = 'IL'"
            Else
                WhereString = WhereString & " AND InspectionType = '" & AT & "'"
            End If
            If DN <> "ALL" Then
                WhereString = WhereString & " AND DataNo = '" & DN & "'"
            End If
            If WO <> "ALL" Then
                WhereString = WhereString & " AND JobNumber = '" & WO & "'"
            End If
            Dim WhereString3 As String = "WHERE (CID = 114 OR CID = 115 OR CID = 590 OR CID = 1001 OR CID = 627) And Inspection_Finished >= dateadd(day,datediff(day,1,GETDATE()),0) AND Inspection_Finished < dateadd(day,datediff(day,0,GETDATE()),0)"
            If AT = "ALL" Then
                WhereString3 = WhereString3 & " AND InspectionType != 'ROLL'"
            ElseIf AT = "FINAL AUDIT" Then
                WhereString2 = WhereString2 & " AND InspectionType = 'EOL'"
            ElseIf AT = "IN LINE" Then
                WhereString3 = WhereString3 & " AND InspectionType = 'IL'"
            Else
                WhereString3 = WhereString3 & " AND InspectionType = '" & AT & "'"
            End If
            If DN <> "ALL" Then
                WhereString3 = WhereString3 & " AND DataNo = '" & DN & "'"
            End If
            If WO <> "ALL" Then
                WhereString3 = WhereString3 & " AND JobNumber = '" & WO & "'"
            End If





            Dim Top As String = String.Format("Facility: '{0}', Time_Period: 'Past 30 Days', No_of_Defects: '{1}', No_of_Rejects: '{3}', No_of_Inspections: '{2}', No_of_Rejected_Lots: '{4}', DHU: '{5}', Reject_Rate: '{6}', Lot_Acceptance: {7}, attr: {{ Facility: {{ rowspan: '4' }} }}", "Global", GetGlobalMonthlyDefectTotal(WhereString2), GetGlobalMonthlyInspectionTotal(WhereString2), GetGlobalMonthlyRejectTotal(WhereString2), GetGlobalMonthlyRejectLotTotal(WhereString2), GetGlobalMonthlyDHU(WhereString2), GetGlobalMonthlyRejectionRate(WhereString2), GetGlobalMonthlyLotAcceptance(WhereString2))
            retval.Add("{" + Top + "}")
            Dim Mid As String = String.Format("Facility: '{0}', Time_Period: 'Past Year', No_of_Defects: '{1}', No_of_Rejects: '{3}', No_of_Inspections: '{2}', No_of_Rejected_Lots: '{4}', DHU: '{5}', Reject_Rate: '{6}', Lot_Acceptance: {7}, attr: {{ Facility: {{ display: 'none' }} }}", "Global", GetGlobalYearlyDefectTotal(WhereString2), GetGlobalYearlyInspectionTotal(WhereString2), GetGlobalYearlyRejectTotal(WhereString2), GetGlobalYearlyRejectLotTotal(WhereString2), GetGlobalYearlyDHU(WhereString2), GetGlobalYearlyRejectionRate(WhereString2), GetGlobalYearlyLotAcceptance(WhereString2))
            retval.Add("{" + Mid + "}")
            Dim Yesterday As String = String.Format("Facility: '{0}', Time_Period: 'Yesterday', No_of_Defects: '{1}', No_of_Rejects: '{3}', No_of_Inspections: '{2}', No_of_Rejected_Lots: '{4}', DHU: '{5}', Reject_Rate: '{6}', Lot_Acceptance: {7}, attr: {{ Facility: {{ display: 'none' }} }}", "Global", GetGlobalYearlyDefectTotal(WhereString3), GetGlobalYearlyInspectionTotal(WhereString3), GetGlobalYearlyRejectTotal(WhereString3), GetGlobalYearlyRejectLotTotal(WhereString3), GetGlobalYearlyDHU(WhereString3), GetGlobalYearlyRejectionRate(WhereString3), GetGlobalYearlyLotAcceptance(WhereString3))
            retval.Add("{" + Yesterday + "}")
            Dim Bottom As String = String.Format("Facility: '{0}', Time_Period: 'Custom', No_of_Defects: '{1}', No_of_Rejects: '{3}', No_of_Inspections: '{2}', No_of_Rejected_Lots: '{4}', DHU: '{5}', Reject_Rate: '{6}', Lot_Acceptance: {7}, attr: {{ Facility: {{ display: 'none' }} }}", "Global", GetGlobalCustomDefectTotal(WhereString), GetGlobalCustomInspectionTotal(WhereString), GetGlobalCustomRejectTotal(WhereString), GetGlobalCustomRejectLotTotal(WhereString), GetGlobalCustomDHU(WhereString), GetGlobalCustomRejectionRate(WhereString), GetGlobalCustomLotAcceptance(WhereString))
            retval.Add("{" + Bottom + "}")




            retval = retval.Distinct().ToList
            Return retval
        End Function
        Public Function DrawWRChart(ByVal fac As String, ByVal gt As String, ByVal tp As String, ByVal wr As String, ByVal Fromdate As String, ByVal Todate As String, ByVal DataNo As String, ByVal WorkOrder As String, ByVal AuditType As String) As List(Of List(Of String))
            Dim retval As New List(Of List(Of String))()
            Dim WhereString2 As String = ""
            If AuditType = "ALL" Then
                WhereString2 = WhereString2 & " AND InspectionType != 'ROLL'"
            ElseIf AuditType = "FINAL AUDIT" Then
                WhereString2 = WhereString2 & " AND InspectionType = 'EOL'"
            ElseIf AuditType = "IN LINE" Then
                WhereString2 = WhereString2 & " AND InspectionType = 'IL'"
            Else
                WhereString2 = WhereString2 & " AND InspectionType = '" & AuditType & "'"
            End If
            If DataNo <> "ALL" Then
                WhereString2 = WhereString2 & " AND DataNo = '" & DataNo & "'"
            End If
            If WorkOrder <> "ALL" Then
                WhereString2 = WhereString2 & " AND JobNumber = '" & WorkOrder & "'"
            End If
            Dim WhereString As String = " AND Inspection_Finished BETWEEN '" & Fromdate & "' AND '" & Todate & "'"
            If AuditType = "ALL" Then
                WhereString = WhereString & " AND InspectionType != 'ROLL'"
            ElseIf AuditType = "FINAL AUDIT" Then
                WhereString = WhereString & " AND InspectionType = 'EOL'"
            ElseIf AuditType = "IN LINE" Then
                WhereString = WhereString & " AND InspectionType = 'IL'"
            Else
                WhereString = WhereString & " AND InspectionType = '" & AuditType & "'"
            End If
            If DataNo <> "ALL" Then
                WhereString = WhereString & " AND DataNo = '" & DataNo & "'"
            End If
            If WorkOrder <> "ALL" Then
                WhereString = WhereString & " AND JobNumber = '" & WorkOrder & "'"
            End If
            Dim SQL As String = ""
            'AND WorkRoom = '" & wr & "'
            If gt = "No_of_Defects" And tp = "Past Year" Then
                Dim segment As New List(Of String)()
                SQL = "Select ISNULL(SUM(MajorsCount+MinorsCount+RepairsCount+ScrapCount), 0) AS TOTAL, dateadd(DAY,0, datediff(day,0, Inspection_Finished)) AS Comp_Date from Inspection.dbo.Locations inner join dbo.InspectionJobSummaryYearly On dbo.InspectionJobSummaryYearly.CID=NCID WHERE Name ='" & fac & "' AND WorkRoom = '" & wr & "'" & WhereString2 & " Group by dateadd(DAY, 0, DateDiff(Day, 0, Inspection_Finished))  ORDER BY Comp_Date ASC"
                Command.CommandType = CommandType.Text 'sets the type of the sql
                Command.Connection = Connection 'sets the connection of our sql command to MyDB
                Command.CommandText = SQL 'sets the statement that executes at the data source to our string
                Connection.Open() 'opens the connction
                DR = Command.ExecuteReader 'sends the command text to the connection and builds tthe SqlDataReader
                While DR.Read() 'Check whether the SqlDataReader has 1 or more rows

                    segment = New List(Of String)()
                    segment.Add(DR("Comp_Date"))
                    segment.Add(DR("TOTAL"))
                    retval.Add(segment)


                End While
                Connection.Close() 'closes the connection
                DR.Close() 'closes the reader
                Return retval
            ElseIf gt = "No_of_Defects" And tp = "Past 30 Days" Then
                Dim segment As New List(Of String)()
                SQL = "Select ISNULL(SUM(MajorsCount+MinorsCount+RepairsCount+ScrapCount), 0) AS TOTAL, dateadd(DAY,0, datediff(day,0, Inspection_Finished)) AS Comp_Date from Inspection.dbo.Locations inner join dbo.InspectionJobSummaryYearly On dbo.InspectionJobSummaryYearly.CID=NCID WHERE Name ='" & fac & "' AND WorkRoom = '" & wr & "'" & WhereString2 & " And Inspection_Finished >= DATEADD(month,-1,GETDATE()) Group by dateadd(DAY, 0, DateDiff(Day, 0, Inspection_Finished))  ORDER BY Comp_Date ASC"
                Command.CommandType = CommandType.Text 'sets the type of the sql
                Command.Connection = Connection 'sets the connection of our sql command to MyDB
                Command.CommandText = SQL 'sets the statement that executes at the data source to our string
                Connection.Open() 'opens the connction
                DR = Command.ExecuteReader 'sends the command text to the connection and builds tthe SqlDataReader
                While DR.Read() 'Check whether the SqlDataReader has 1 or more rows

                    segment = New List(Of String)()
                    segment.Add(DR("Comp_Date"))
                    segment.Add(DR("TOTAL"))
                    retval.Add(segment)


                End While
                Connection.Close() 'closes the connection
                DR.Close() 'closes the reader
                Return retval
            ElseIf gt = "No_of_Defects" And tp = "Custom" Then
                Dim segment As New List(Of String)()
                SQL = "Select ISNULL(SUM(MajorsCount+MinorsCount+RepairsCount+ScrapCount), 0) AS TOTAL, dateadd(DAY,0, datediff(day,0, Inspection_Finished)) AS Comp_Date from Inspection.dbo.Locations inner join dbo.InspectionJobSummaryYearly On dbo.InspectionJobSummaryYearly.CID=NCID WHERE Name ='" & fac & "'" & WhereString & " AND WorkRoom = '" & wr & "' Group by dateadd(DAY, 0, DateDiff(Day, 0, Inspection_Finished))  ORDER BY Comp_Date ASC"
                Command.CommandType = CommandType.Text 'sets the type of the sql
                Command.Connection = Connection 'sets the connection of our sql command to MyDB
                Command.CommandText = SQL 'sets the statement that executes at the data source to our string
                Connection.Open() 'opens the connction
                DR = Command.ExecuteReader 'sends the command text to the connection and builds tthe SqlDataReader
                While DR.Read() 'Check whether the SqlDataReader has 1 or more rows

                    segment = New List(Of String)()
                    segment.Add(DR("Comp_Date"))
                    segment.Add(DR("TOTAL"))
                    retval.Add(segment)


                End While
                Connection.Close() 'closes the connection
                DR.Close() 'closes the reader
                Return retval
            ElseIf gt = "No_of_Rejects" And tp = "Past Year" Then
                Dim segment As New List(Of String)()
                SQL = "Select ISNULL(SUM(MajorsCount+RepairsCount+ScrapCount), 0) AS TOTAL, dateadd(DAY,0, datediff(day,0, Inspection_Finished)) AS Comp_Date from Inspection.dbo.Locations inner join dbo.InspectionJobSummaryYearly On dbo.InspectionJobSummaryYearly.CID=NCID WHERE Name ='" & fac & "'" & WhereString2 & " AND WorkRoom = '" & wr & "' Group by dateadd(DAY, 0, DateDiff(Day, 0, Inspection_Finished))  ORDER BY Comp_Date ASC"
                Command.CommandType = CommandType.Text 'sets the type of the sql
                Command.Connection = Connection 'sets the connection of our sql command to MyDB
                Command.CommandText = SQL 'sets the statement that executes at the data source to our string
                Connection.Open() 'opens the connction
                DR = Command.ExecuteReader 'sends the command text to the connection and builds tthe SqlDataReader
                While DR.Read() 'Check whether the SqlDataReader has 1 or more rows

                    segment = New List(Of String)()
                    segment.Add(DR("Comp_Date"))
                    segment.Add(DR("TOTAL"))
                    retval.Add(segment)


                End While
                Connection.Close() 'closes the connection
                DR.Close() 'closes the reader
                Return retval
            ElseIf gt = "No_of_Rejects" And tp = "Past 30 Days" Then
                Dim segment As New List(Of String)()
                SQL = "Select ISNULL(SUM(MajorsCount+RepairsCount+ScrapCount), 0) AS TOTAL, dateadd(DAY,0, datediff(day,0, Inspection_Finished)) AS Comp_Date from Inspection.dbo.Locations inner join dbo.InspectionJobSummaryYearly On dbo.InspectionJobSummaryYearly.CID=NCID WHERE Name ='" & fac & "'" & WhereString2 & " AND WorkRoom = '" & wr & "' And Inspection_Finished >= DATEADD(month,-1,GETDATE()) Group by dateadd(DAY, 0, DateDiff(Day, 0, Inspection_Finished))  ORDER BY Comp_Date ASC"
                Command.CommandType = CommandType.Text 'sets the type of the sql
                Command.Connection = Connection 'sets the connection of our sql command to MyDB
                Command.CommandText = SQL 'sets the statement that executes at the data source to our string
                Connection.Open() 'opens the connction
                DR = Command.ExecuteReader 'sends the command text to the connection and builds tthe SqlDataReader
                While DR.Read() 'Check whether the SqlDataReader has 1 or more rows

                    segment = New List(Of String)()
                    segment.Add(DR("Comp_Date"))
                    segment.Add(DR("TOTAL"))
                    retval.Add(segment)


                End While
                Connection.Close() 'closes the connection
                DR.Close() 'closes the reader
                Return retval
            ElseIf gt = "No_of_Rejects" And tp = "Custom" Then
                Dim segment As New List(Of String)()
                SQL = "Select ISNULL(SUM(MajorsCount+RepairsCount+ScrapCount), 0) AS TOTAL, dateadd(DAY,0, datediff(day,0, Inspection_Finished)) AS Comp_Date from Inspection.dbo.Locations inner join dbo.InspectionJobSummaryYearly On dbo.InspectionJobSummaryYearly.CID=NCID WHERE Name ='" & fac & "'" & WhereString & " AND WorkRoom = '" & wr & "' Group by dateadd(DAY, 0, DateDiff(Day, 0, Inspection_Finished))  ORDER BY Comp_Date ASC"
                Command.CommandType = CommandType.Text 'sets the type of the sql
                Command.Connection = Connection 'sets the connection of our sql command to MyDB
                Command.CommandText = SQL 'sets the statement that executes at the data source to our string
                Connection.Open() 'opens the connction
                DR = Command.ExecuteReader 'sends the command text to the connection and builds tthe SqlDataReader
                While DR.Read() 'Check whether the SqlDataReader has 1 or more rows

                    segment = New List(Of String)()
                    segment.Add(DR("Comp_Date"))
                    segment.Add(DR("TOTAL"))
                    retval.Add(segment)


                End While
                Connection.Close() 'closes the connection
                DR.Close() 'closes the reader
                Return retval
            ElseIf gt = "No_of_Inspections" And tp = "Past Year" Then
                Dim segment As New List(Of String)()
                SQL = "Select COUNT(InspectionJobSummaryYearly.id) AS TOTAL, dateadd(DAY,0, datediff(day,0, Inspection_Finished)) AS Comp_Date from Inspection.dbo.Locations inner join dbo.InspectionJobSummaryYearly On dbo.InspectionJobSummaryYearly.CID=NCID WHERE Name ='" & fac & "'" & WhereString2 & " AND WorkRoom = '" & wr & "' Group by dateadd(DAY, 0, DateDiff(Day, 0, Inspection_Finished))  ORDER BY Comp_Date ASC"
                Command.CommandType = CommandType.Text 'sets the type of the sql
                Command.Connection = Connection 'sets the connection of our sql command to MyDB
                Command.CommandText = SQL 'sets the statement that executes at the data source to our string
                Connection.Open() 'opens the connction
                DR = Command.ExecuteReader 'sends the command text to the connection and builds tthe SqlDataReader
                While DR.Read() 'Check whether the SqlDataReader has 1 or more rows

                    segment = New List(Of String)()
                    segment.Add(DR("Comp_Date"))
                    segment.Add(DR("TOTAL"))
                    retval.Add(segment)


                End While
                Connection.Close() 'closes the connection
                DR.Close() 'closes the reader
                Return retval
            ElseIf gt = "No_of_Inspections" And tp = "Past 30 Days" Then
                Dim segment As New List(Of String)()
                SQL = "Select COUNT(InspectionJobSummaryYearly.id) AS TOTAL, dateadd(DAY,0, datediff(day,0, Inspection_Finished)) AS Comp_Date from Inspection.dbo.Locations inner join dbo.InspectionJobSummaryYearly On dbo.InspectionJobSummaryYearly.CID=NCID WHERE Name ='" & fac & "'" & WhereString2 & " AND WorkRoom = '" & wr & "' And Inspection_Finished >= DATEADD(month,-1,GETDATE()) Group by dateadd(DAY, 0, DateDiff(Day, 0, Inspection_Finished))  ORDER BY Comp_Date ASC"
                Command.CommandType = CommandType.Text 'sets the type of the sql
                Command.Connection = Connection 'sets the connection of our sql command to MyDB
                Command.CommandText = SQL 'sets the statement that executes at the data source to our string
                Connection.Open() 'opens the connction
                DR = Command.ExecuteReader 'sends the command text to the connection and builds tthe SqlDataReader
                While DR.Read() 'Check whether the SqlDataReader has 1 or more rows

                    segment = New List(Of String)()
                    segment.Add(DR("Comp_Date"))
                    segment.Add(DR("TOTAL"))
                    retval.Add(segment)


                End While
                Connection.Close() 'closes the connection
                DR.Close() 'closes the reader
                Return retval
            ElseIf gt = "No_of_Inspections" And tp = "Custom" Then
                Dim segment As New List(Of String)()
                SQL = "Select COUNT(InspectionJobSummaryYearly.id) AS TOTAL, dateadd(DAY,0, datediff(day,0, Inspection_Finished)) AS Comp_Date from Inspection.dbo.Locations inner join dbo.InspectionJobSummaryYearly On dbo.InspectionJobSummaryYearly.CID=NCID WHERE Name ='" & fac & "'" & WhereString & " AND WorkRoom = '" & wr & "' Group by dateadd(DAY, 0, DateDiff(Day, 0, Inspection_Finished))  ORDER BY Comp_Date ASC"
                Command.CommandType = CommandType.Text 'sets the type of the sql
                Command.Connection = Connection 'sets the connection of our sql command to MyDB
                Command.CommandText = SQL 'sets the statement that executes at the data source to our string
                Connection.Open() 'opens the connction
                DR = Command.ExecuteReader 'sends the command text to the connection and builds tthe SqlDataReader
                While DR.Read() 'Check whether the SqlDataReader has 1 or more rows

                    segment = New List(Of String)()
                    segment.Add(DR("Comp_Date"))
                    segment.Add(DR("TOTAL"))
                    retval.Add(segment)


                End While
                Connection.Close() 'closes the connection
                DR.Close() 'closes the reader
                Return retval
            ElseIf gt = "No_of_Rejected_Lots" And tp = "Past Year" Then
                Dim segment As New List(Of String)()
                SQL = "Select COUNT(InspectionJobSummaryYearly.id) AS TOTAL, dateadd(DAY,0, datediff(day,0, Inspection_Finished)) AS Comp_Date from Inspection.dbo.Locations inner join dbo.InspectionJobSummaryYearly On dbo.InspectionJobSummaryYearly.CID=NCID WHERE Name ='" & fac & "'" & WhereString2 & " AND WorkRoom = '" & wr & "' AND Technical_PassFail=0 Group by dateadd(DAY, 0, DateDiff(Day, 0, Inspection_Finished))  ORDER BY Comp_Date ASC"
                Command.CommandType = CommandType.Text 'sets the type of the sql
                Command.Connection = Connection 'sets the connection of our sql command to MyDB
                Command.CommandText = SQL 'sets the statement that executes at the data source to our string
                Connection.Open() 'opens the connction
                DR = Command.ExecuteReader 'sends the command text to the connection and builds tthe SqlDataReader
                While DR.Read() 'Check whether the SqlDataReader has 1 or more rows

                    segment = New List(Of String)()
                    segment.Add(DR("Comp_Date"))
                    segment.Add(DR("TOTAL"))
                    retval.Add(segment)


                End While
                Connection.Close() 'closes the connection
                DR.Close() 'closes the reader
                Return retval
            ElseIf gt = "No_of_Rejected_Lots" And tp = "Past 30 Days" Then
                Dim segment As New List(Of String)()
                SQL = "Select COUNT(InspectionJobSummaryYearly.id) AS TOTAL, dateadd(DAY,0, datediff(day,0, Inspection_Finished)) AS Comp_Date from Inspection.dbo.Locations inner join dbo.InspectionJobSummaryYearly On dbo.InspectionJobSummaryYearly.CID=NCID WHERE Name ='" & fac & "'" & WhereString2 & " AND WorkRoom = '" & wr & "' And Inspection_Finished >= DATEADD(month,-1,GETDATE()) AND Technical_PassFail=0 Group by dateadd(DAY, 0, DateDiff(Day, 0, Inspection_Finished))  ORDER BY Comp_Date ASC"
                Command.CommandType = CommandType.Text 'sets the type of the sql
                Command.Connection = Connection 'sets the connection of our sql command to MyDB
                Command.CommandText = SQL 'sets the statement that executes at the data source to our string
                Connection.Open() 'opens the connction
                DR = Command.ExecuteReader 'sends the command text to the connection and builds tthe SqlDataReader
                While DR.Read() 'Check whether the SqlDataReader has 1 or more rows

                    segment = New List(Of String)()
                    segment.Add(DR("Comp_Date"))
                    segment.Add(DR("TOTAL"))
                    retval.Add(segment)


                End While
                Connection.Close() 'closes the connection
                DR.Close() 'closes the reader
                Return retval
            ElseIf gt = "No_of_Rejected_Lots" And tp = "Custom" Then
                Dim segment As New List(Of String)()
                SQL = "Select COUNT(InspectionJobSummaryYearly.id) AS TOTAL, dateadd(DAY,0, datediff(day,0, Inspection_Finished)) AS Comp_Date from Inspection.dbo.Locations inner join dbo.InspectionJobSummaryYearly On dbo.InspectionJobSummaryYearly.CID=NCID WHERE Name ='" & fac & "'" & WhereString & " AND WorkRoom = '" & wr & "' AND Technical_PassFail=0 Group by dateadd(DAY, 0, DateDiff(Day, 0, Inspection_Finished))  ORDER BY Comp_Date ASC"
                Command.CommandType = CommandType.Text 'sets the type of the sql
                Command.Connection = Connection 'sets the connection of our sql command to MyDB
                Command.CommandText = SQL 'sets the statement that executes at the data source to our string
                Connection.Open() 'opens the connction
                DR = Command.ExecuteReader 'sends the command text to the connection and builds tthe SqlDataReader
                While DR.Read() 'Check whether the SqlDataReader has 1 or more rows

                    segment = New List(Of String)()
                    segment.Add(DR("Comp_Date"))
                    segment.Add(DR("TOTAL"))
                    retval.Add(segment)


                End While
                Connection.Close() 'closes the connection
                DR.Close() 'closes the reader
                Return retval
            ElseIf gt = "DHU" And tp = "Past Year" Then
                Dim segment As New List(Of String)()
                SQL = "Select (CAST(SUM(MajorsCount+MinorsCount+RepairsCount+ScrapCount) AS Decimal(10,2))/CAST(SUM(SampleSize) AS Decimal(10,2)))*100 AS TOTAL, dateadd(DAY,0, datediff(day,0, Inspection_Finished)) AS Comp_Date from Inspection.dbo.Locations inner join dbo.InspectionJobSummaryYearly On dbo.InspectionJobSummaryYearly.CID=NCID WHERE Name ='" & fac & "'" & WhereString2 & " AND WorkRoom = '" & wr & "' Group by dateadd(DAY, 0, DateDiff(Day, 0, Inspection_Finished))  ORDER BY Comp_Date ASC"
                Command.CommandType = CommandType.Text 'sets the type of the sql
                Command.Connection = Connection 'sets the connection of our sql command to MyDB
                Command.CommandText = SQL 'sets the statement that executes at the data source to our string
                Connection.Open() 'opens the connction
                DR = Command.ExecuteReader 'sends the command text to the connection and builds tthe SqlDataReader
                While DR.Read() 'Check whether the SqlDataReader has 1 or more rows

                    segment = New List(Of String)()
                    segment.Add(DR("Comp_Date"))
                    segment.Add(DR("TOTAL"))
                    retval.Add(segment)


                End While
                Connection.Close() 'closes the connection
                DR.Close() 'closes the reader
                Return retval

            ElseIf gt = "DHU" And tp = "Past 30 Days" Then
                Dim segment As New List(Of String)()
                SQL = "Select (CAST(SUM(MajorsCount+MinorsCount+RepairsCount+ScrapCount) AS Decimal(10,2))/CAST(SUM(SampleSize) AS Decimal(10,2)))*100 AS TOTAL , dateadd(DAY,0, datediff(day,0, Inspection_Finished)) AS Comp_Date from Inspection.dbo.Locations inner join dbo.InspectionJobSummaryYearly On dbo.InspectionJobSummaryYearly.CID=NCID WHERE Name ='" & fac & "'" & WhereString2 & " AND WorkRoom = '" & wr & "' And Inspection_Finished >= DATEADD(month,-1,GETDATE()) Group by dateadd(DAY, 0, DateDiff(Day, 0, Inspection_Finished))  ORDER BY Comp_Date ASC"
                Command.CommandType = CommandType.Text 'sets the type of the sql
                Command.Connection = Connection 'sets the connection of our sql command to MyDB
                Command.CommandText = SQL 'sets the statement that executes at the data source to our string
                Connection.Open() 'opens the connction
                DR = Command.ExecuteReader 'sends the command text to the connection and builds tthe SqlDataReader
                While DR.Read() 'Check whether the SqlDataReader has 1 or more rows

                    segment = New List(Of String)()
                    segment.Add(DR("Comp_Date"))
                    segment.Add(DR("TOTAL"))
                    retval.Add(segment)


                End While
                Connection.Close() 'closes the connection
                DR.Close() 'closes the reader
                Return retval
            ElseIf gt = "DHU" And tp = "Custom" Then
                Dim segment As New List(Of String)()
                SQL = "Select (CAST(SUM(MajorsCount+MinorsCount+RepairsCount+ScrapCount) AS Decimal(10,2))/CAST(SUM(SampleSize) AS Decimal(10,2)))*100 AS TOTAL, dateadd(DAY,0, datediff(day,0, Inspection_Finished)) AS Comp_Date from Inspection.dbo.Locations inner join dbo.InspectionJobSummaryYearly On dbo.InspectionJobSummaryYearly.CID=NCID WHERE Name ='" & fac & "'" & WhereString & " AND WorkRoom = '" & wr & "' Group by dateadd(DAY, 0, DateDiff(Day, 0, Inspection_Finished))  ORDER BY Comp_Date ASC"
                Command.CommandType = CommandType.Text 'sets the type of the sql
                Command.Connection = Connection 'sets the connection of our sql command to MyDB
                Command.CommandText = SQL 'sets the statement that executes at the data source to our string
                Connection.Open() 'opens the connction
                DR = Command.ExecuteReader 'sends the command text to the connection and builds tthe SqlDataReader
                While DR.Read() 'Check whether the SqlDataReader has 1 or more rows

                    segment = New List(Of String)()
                    segment.Add(DR("Comp_Date"))
                    segment.Add(DR("TOTAL"))
                    retval.Add(segment)


                End While
                Connection.Close() 'closes the connection
                DR.Close() 'closes the reader
                Return retval

            ElseIf gt = "Reject_Rate" And tp = "Past Year" Then
                Dim segment As New List(Of String)()
                SQL = "Select (CAST(SUM(MajorsCount+RepairsCount+ScrapCount) AS Decimal(10,2))/CAST(SUM(SampleSize) AS Decimal(10,2)))*100 AS TOTAL, dateadd(DAY,0, datediff(day,0, Inspection_Finished)) AS Comp_Date from Inspection.dbo.Locations inner join dbo.InspectionJobSummaryYearly On dbo.InspectionJobSummaryYearly.CID=NCID WHERE Name ='" & fac & "'" & WhereString2 & " AND WorkRoom = '" & wr & "' Group by dateadd(DAY, 0, DateDiff(Day, 0, Inspection_Finished))  ORDER BY Comp_Date ASC"
                Command.CommandType = CommandType.Text 'sets the type of the sql
                Command.Connection = Connection 'sets the connection of our sql command to MyDB
                Command.CommandText = SQL 'sets the statement that executes at the data source to our string
                Connection.Open() 'opens the connction
                DR = Command.ExecuteReader 'sends the command text to the connection and builds tthe SqlDataReader
                While DR.Read() 'Check whether the SqlDataReader has 1 or more rows

                    segment = New List(Of String)()
                    segment.Add(DR("Comp_Date"))
                    segment.Add(DR("TOTAL"))
                    retval.Add(segment)


                End While
                Connection.Close() 'closes the connection
                DR.Close() 'closes the reader
                Return retval

            ElseIf gt = "Reject_Rate" And tp = "Past 30 Days" Then
                Dim segment As New List(Of String)()
                SQL = "Select (CAST(SUM(MajorsCount+RepairsCount+ScrapCount) AS Decimal(10,2))/CAST(SUM(SampleSize) AS Decimal(10,2)))*100 AS TOTAL , dateadd(DAY,0, datediff(day,0, Inspection_Finished)) AS Comp_Date from Inspection.dbo.Locations inner join dbo.InspectionJobSummaryYearly On dbo.InspectionJobSummaryYearly.CID=NCID WHERE Name ='" & fac & "'" & WhereString2 & " AND WorkRoom = '" & wr & "' And Inspection_Finished >= DATEADD(month,-1,GETDATE()) Group by dateadd(DAY, 0, DateDiff(Day, 0, Inspection_Finished))  ORDER BY Comp_Date ASC"
                Command.CommandType = CommandType.Text 'sets the type of the sql
                Command.Connection = Connection 'sets the connection of our sql command to MyDB
                Command.CommandText = SQL 'sets the statement that executes at the data source to our string
                Connection.Open() 'opens the connction
                DR = Command.ExecuteReader 'sends the command text to the connection and builds tthe SqlDataReader
                While DR.Read() 'Check whether the SqlDataReader has 1 or more rows

                    segment = New List(Of String)()
                    segment.Add(DR("Comp_Date"))
                    segment.Add(DR("TOTAL"))
                    retval.Add(segment)


                End While
                Connection.Close() 'closes the connection
                DR.Close() 'closes the reader
                Return retval
            ElseIf gt = "Reject_Rate" And tp = "Custom" Then
                Dim segment As New List(Of String)()
                SQL = "Select (CAST(SUM(MajorsCount+RepairsCount+ScrapCount) AS Decimal(10,2))/CAST(SUM(SampleSize) AS Decimal(10,2)))*100 AS TOTAL, dateadd(DAY,0, datediff(day,0, Inspection_Finished)) AS Comp_Date from Inspection.dbo.Locations inner join dbo.InspectionJobSummaryYearly On dbo.InspectionJobSummaryYearly.CID=NCID WHERE Name ='" & fac & "'" & WhereString & " AND WorkRoom = '" & wr & "' Group by dateadd(DAY, 0, DateDiff(Day, 0, Inspection_Finished))  ORDER BY Comp_Date ASC"
                Command.CommandType = CommandType.Text 'sets the type of the sql
                Command.Connection = Connection 'sets the connection of our sql command to MyDB
                Command.CommandText = SQL 'sets the statement that executes at the data source to our string
                Connection.Open() 'opens the connction
                DR = Command.ExecuteReader 'sends the command text to the connection and builds tthe SqlDataReader
                While DR.Read() 'Check whether the SqlDataReader has 1 or more rows

                    segment = New List(Of String)()
                    segment.Add(DR("Comp_Date"))
                    segment.Add(DR("TOTAL"))
                    retval.Add(segment)


                End While
                Connection.Close() 'closes the connection
                DR.Close() 'closes the reader
                Return retval

            ElseIf gt = "Lot_Acceptance" And tp = "Past Year" Then
                Dim segment As New List(Of String)()
                SQL = "Select ISNULL((CAST(s3.TOTAL AS Decimal(10,2)))/(CAST( s3.TOTAL2 AS Decimal(10,2)))*100, 0) AS TOTAL3,dateadd(DAY,0, datediff(day,0, s3.Comp_Date)) AS Comp_Date3  FROM (Select s1.TOTAL, s2.TOTAL2, s2.Comp_Date From ((Select COUNT(InspectionJobSummaryYearly.id) AS TOTAL, dateadd(DAY,0, datediff(day,0, Inspection_Finished)) AS Comp_Date from Inspection.dbo.Locations inner join dbo.InspectionJobSummaryYearly On dbo.InspectionJobSummaryYearly.CID=NCID WHERE Name ='" & fac & "'" & WhereString2 & " AND WorkRoom = '" & wr & "' AND Technical_PassFail=1 Group by dateadd(DAY, 0, DateDiff(Day, 0, Inspection_Finished))) AS s1 RIGHT JOIN (Select COUNT(InspectionJobSummaryYearly.id) AS TOTAL2, dateadd(DAY,0, datediff(day,0, Inspection_Finished)) AS Comp_Date from Inspection.dbo.Locations inner join dbo.InspectionJobSummaryYearly On dbo.InspectionJobSummaryYearly.CID=NCID WHERE Name ='" & fac & "'" & WhereString2 & " AND WorkRoom = '" & wr & "' Group by dateadd(DAY, 0, DateDiff(Day, 0, Inspection_Finished))) AS s2 ON s1.Comp_Date=s2.Comp_Date)) AS s3 ORDER by Comp_Date3 ASC"
                Command.CommandType = CommandType.Text 'sets the type of the sql
                Command.Connection = Connection 'sets the connection of our sql command to MyDB
                Command.CommandText = SQL 'sets the statement that executes at the data source to our string
                Connection.Open() 'opens the connction
                DR = Command.ExecuteReader 'sends the command text to the connection and builds tthe SqlDataReader
                While DR.Read() 'Check whether the SqlDataReader has 1 or more rows

                    segment = New List(Of String)()
                    segment.Add(DR("Comp_Date3"))
                    segment.Add(DR("TOTAL3"))
                    retval.Add(segment)


                End While
                Connection.Close() 'closes the connection
                DR.Close() 'closes the reader
                Return retval

            ElseIf gt = "Lot_Acceptance" And tp = "Past 30 Days" Then
                Dim segment As New List(Of String)()
                SQL = "Select ISNULL((CAST(s3.TOTAL AS Decimal(10,2)))/(CAST( s3.TOTAL2 AS Decimal(10,2)))*100, 0) AS TOTAL3,dateadd(DAY,0, datediff(day,0, s3.Comp_Date)) AS Comp_Date3  FROM (Select s1.TOTAL, s2.TOTAL2, s2.Comp_Date From ((Select COUNT(InspectionJobSummaryYearly.id) AS TOTAL, dateadd(DAY,0, datediff(day,0, Inspection_Finished)) AS Comp_Date from Inspection.dbo.Locations inner join dbo.InspectionJobSummaryYearly On dbo.InspectionJobSummaryYearly.CID=NCID WHERE Name ='" & fac & "'" & WhereString2 & " AND WorkRoom = '" & wr & "' And Inspection_Finished >= DATEADD(month,-1,GETDATE()) AND Technical_PassFail=1 Group by dateadd(DAY, 0, DateDiff(Day, 0, Inspection_Finished))) AS s1 RIGHT JOIN (Select COUNT(InspectionJobSummaryYearly.id) AS TOTAL2, dateadd(DAY,0, datediff(day,0, Inspection_Finished)) AS Comp_Date from Inspection.dbo.Locations inner join dbo.InspectionJobSummaryYearly On dbo.InspectionJobSummaryYearly.CID=NCID WHERE Name ='" & fac & "'" & WhereString2 & " AND WorkRoom = '" & wr & "' And Inspection_Finished >= DATEADD(month,-1,GETDATE()) Group by dateadd(DAY, 0, DateDiff(Day, 0, Inspection_Finished))) AS s2 ON s1.Comp_Date=s2.Comp_Date)) AS s3 ORDER by Comp_Date3 ASC"
                Command.CommandType = CommandType.Text 'sets the type of the sql
                Command.Connection = Connection 'sets the connection of our sql command to MyDB
                Command.CommandText = SQL 'sets the statement that executes at the data source to our string
                Connection.Open() 'opens the connction
                DR = Command.ExecuteReader 'sends the command text to the connection and builds tthe SqlDataReader
                While DR.Read() 'Check whether the SqlDataReader has 1 or more rows

                    segment = New List(Of String)()
                    segment.Add(DR("Comp_Date3"))
                    segment.Add(DR("TOTAL3"))
                    retval.Add(segment)


                End While
                Connection.Close() 'closes the connection
                DR.Close() 'closes the reader
                Return retval
            ElseIf gt = "Lot_Acceptance" And tp = "Custom" Then
                Dim segment As New List(Of String)()
                SQL = "Select ISNULL((CAST(s3.TOTAL AS Decimal(10,2)))/(CAST( s3.TOTAL2 AS Decimal(10,2)))*100, 0) AS TOTAL3,dateadd(DAY,0, datediff(day,0, s3.Comp_Date)) AS Comp_Date3  FROM (Select s1.TOTAL, s2.TOTAL2, s2.Comp_Date From ((Select COUNT(InspectionJobSummaryYearly.id) AS TOTAL, dateadd(DAY,0, datediff(day,0, Inspection_Finished)) AS Comp_Date from Inspection.dbo.Locations inner join dbo.InspectionJobSummaryYearly On dbo.InspectionJobSummaryYearly.CID=NCID WHERE Name ='" & fac & "'" & WhereString & " AND WorkRoom = '" & wr & "' AND Technical_PassFail=1 Group by dateadd(DAY, 0, DateDiff(Day, 0, Inspection_Finished))) AS s1 RIGHT JOIN (Select COUNT(InspectionJobSummaryYearly.id) AS TOTAL2, dateadd(DAY,0, datediff(day,0, Inspection_Finished)) AS Comp_Date from Inspection.dbo.Locations inner join dbo.InspectionJobSummaryYearly On dbo.InspectionJobSummaryYearly.CID=NCID WHERE Name ='" & fac & "'" & WhereString & " AND WorkRoom = '" & wr & "' Group by dateadd(DAY, 0, DateDiff(Day, 0, Inspection_Finished))) AS s2 ON s1.Comp_Date=s2.Comp_Date)) AS s3 ORDER by Comp_Date3 ASC"
                Command.CommandType = CommandType.Text 'sets the type of the sql
                Command.Connection = Connection 'sets the connection of our sql command to MyDB
                Command.CommandText = SQL 'sets the statement that executes at the data source to our string
                Connection.Open() 'opens the connction
                DR = Command.ExecuteReader 'sends the command text to the connection and builds tthe SqlDataReader
                While DR.Read() 'Check whether the SqlDataReader has 1 or more rows

                    segment = New List(Of String)()
                    segment.Add(DR("Comp_Date3"))
                    segment.Add(DR("TOTAL3"))
                    retval.Add(segment)


                End While
                Connection.Close() 'closes the connection
                DR.Close() 'closes the reader
                Return retval

            End If


            Return retval
        End Function
        Public Function DrillDownWR(ByVal dt As String, ByVal fac As String, ByVal gt As String, ByVal tp As String, ByVal wr As String, ByVal Fromdate As String, ByVal Todate As String, ByVal DataNo As String, ByVal WorkOrder As String, ByVal AuditType As String) As List(Of List(Of String))
            Dim retval As New List(Of List(Of String))()
            Dim WhereString2 As String = ""
            If AuditType = "ALL" Then
                WhereString2 = WhereString2 & " AND InspectionType != 'ROLL'"
            ElseIf AuditType = "FINAL AUDIT" Then
                WhereString2 = WhereString2 & " AND InspectionType = 'EOL'"
            ElseIf AuditType = "IN LINE" Then
                WhereString2 = WhereString2 & " AND InspectionType = 'IL'"
            Else
                WhereString2 = WhereString2 & " AND InspectionType = '" & AuditType & "'"
            End If
            If DataNo <> "ALL" Then
                WhereString2 = WhereString2 & " AND DataNo = '" & DataNo & "'"
            End If
            If WorkOrder <> "ALL" Then
                WhereString2 = WhereString2 & " AND JobNumber = '" & WorkOrder & "'"
            End If
            Dim WhereString As String = " AND Inspection_Finished BETWEEN '" & Fromdate & "' AND '" & Todate & "'"
            If AuditType = "ALL" Then
                WhereString = WhereString & " AND InspectionType != 'ROLL'"
            ElseIf AuditType = "FINAL AUDIT" Then
                WhereString = WhereString & " AND InspectionType = 'EOL'"
            ElseIf AuditType = "IN LINE" Then
                WhereString = WhereString & " AND InspectionType = 'IL'"
            Else
                WhereString = WhereString & " AND InspectionType = '" & AuditType & "'"
            End If
            If DataNo <> "ALL" Then
                WhereString = WhereString & " AND DataNo = '" & DataNo & "'"
            End If
            If WorkOrder <> "ALL" Then
                WhereString = WhereString & " AND JobNumber = '" & WorkOrder & "'"
            End If
            Dim SQL As String = ""
            ' AND InspectionJobSummaryYearly.WorkRoom = '" & wr & "' 
            If gt = "No_of_Defects" Then
                Dim segment As New List(Of String)()
                SQL = "select DefectDesc as DefectName, COUNT(DefectDesc) as DefectCount from (Inspection.dbo.Locations inner join dbo.InspectionJobSummaryYearly On dbo.InspectionJobSummaryYearly.CID=NCID) inner join Inspection.dbo.DefectMaster on InspectionJobSummaryYearly.id=Inspection.dbo.DefectMaster.InspectionJobSummaryId WHERE Name ='" & fac & "'" & WhereString2 & " AND Inspection_Finished >= '" & dt & "' and Inspection_Finished < dateadd(day,1,'" & dt & "') AND InspectionJobSummaryYearly.WorkRoom = '" & wr & "'  group by DefectDesc ORDER By DefectCount DESC"
                Command.CommandType = CommandType.Text 'sets the type of the sql
                Command.Connection = Connection 'sets the connection of our sql command to MyDB
                Command.CommandText = SQL 'sets the statement that executes at the data source to our string
                Connection.Open() 'opens the connction
                DR = Command.ExecuteReader 'sends the command text to the connection and builds tthe SqlDataReader
                While DR.Read() 'Check whether the SqlDataReader has 1 or more rows

                    segment = New List(Of String)()
                    segment.Add(DR("DefectName"))
                    segment.Add(DR("DefectCount"))
                    retval.Add(segment)


                End While
                Connection.Close() 'closes the connection
                DR.Close() 'closes the reader
                Return retval
            ElseIf gt = "No_of_Rejects" Then
                Dim segment As New List(Of String)()
                SQL = "select DefectDesc as DefectName, COUNT(DefectDesc) as DefectCount from (Inspection.dbo.Locations inner join dbo.InspectionJobSummaryYearly On dbo.InspectionJobSummaryYearly.CID=NCID) inner join Inspection.dbo.DefectMaster on InspectionJobSummaryYearly.id=Inspection.dbo.DefectMaster.InspectionJobSummaryId WHERE Name ='" & fac & "'" & WhereString2 & " AND Inspection_Finished >= '" & dt & "' and Inspection_Finished < dateadd(day,1,'" & dt & "') AND InspectionJobSummaryYearly.WorkRoom = '" & wr & "'  AND DefectClass != 'MINOR'  group by DefectDesc ORDER By DefectCount DESC"
                Command.CommandType = CommandType.Text 'sets the type of the sql
                Command.Connection = Connection 'sets the connection of our sql command to MyDB
                Command.CommandText = SQL 'sets the statement that executes at the data source to our string
                Connection.Open() 'opens the connction
                DR = Command.ExecuteReader 'sends the command text to the connection and builds tthe SqlDataReader
                While DR.Read() 'Check whether the SqlDataReader has 1 or more rows

                    segment = New List(Of String)()
                    segment.Add(DR("DefectName"))
                    segment.Add(DR("DefectCount"))
                    retval.Add(segment)


                End While
                Connection.Close() 'closes the connection
                DR.Close() 'closes the reader
                Return retval

            ElseIf gt = "No_of_Inspections" Then
                Dim segment As New List(Of String)()
                SQL = "select InspectionJobSummaryYearly.id AS 'Id', JobNumber AS 'JobNumber', DataNo As 'DataNumber' from Inspection.dbo.Locations inner join dbo.InspectionJobSummaryYearly On dbo.InspectionJobSummaryYearly.CID=NCID WHERE Name ='" & fac & "'" & WhereString2 & " AND Inspection_Finished >= '" & dt & "' and Inspection_Finished < dateadd(day,1, '" & dt & "')  AND InspectionJobSummaryYearly.WorkRoom = '" & wr & "' "
                Command.CommandType = CommandType.Text 'sets the type of the sql
                Command.Connection = Connection 'sets the connection of our sql command to MyDB
                Command.CommandText = SQL 'sets the statement that executes at the data source to our string
                Connection.Open() 'opens the connction
                DR = Command.ExecuteReader 'sends the command text to the connection and builds tthe SqlDataReader
                While DR.Read() 'Check whether the SqlDataReader has 1 or more rows

                    segment = New List(Of String)()
                    segment.Add(DR("Id"))
                    segment.Add(DR("JobNumber"))
                    segment.Add(DR("DataNumber"))
                    retval.Add(segment)


                End While
                Connection.Close() 'closes the connection
                DR.Close() 'closes the reader
                Return retval

            ElseIf gt = "No_of_Rejected_Lots" Then
                Dim segment As New List(Of String)()
                SQL = "select InspectionJobSummaryYearly.id AS 'Id', JobNumber AS 'JobNumber', DataNo As 'DataNumber' from Inspection.dbo.Locations inner join dbo.InspectionJobSummaryYearly On dbo.InspectionJobSummaryYearly.CID=NCID WHERE Name ='" & fac & "'" & WhereString2 & " AND Inspection_Finished >= '" & dt & "' and Inspection_Finished < dateadd(day,1, '" & dt & "') AND Technical_PassFail = 0 AND InspectionJobSummaryYearly.WorkRoom = '" & wr & "' "
                Command.CommandType = CommandType.Text 'sets the type of the sql
                Command.Connection = Connection 'sets the connection of our sql command to MyDB
                Command.CommandText = SQL 'sets the statement that executes at the data source to our string
                Connection.Open() 'opens the connction
                DR = Command.ExecuteReader 'sends the command text to the connection and builds tthe SqlDataReader
                While DR.Read() 'Check whether the SqlDataReader has 1 or more rows

                    segment = New List(Of String)()
                    segment.Add(DR("Id"))
                    segment.Add(DR("JobNumber"))
                    segment.Add(DR("DataNumber"))
                    retval.Add(segment)


                End While
                Connection.Close() 'closes the connection
                DR.Close() 'closes the reader
                Return retval


            End If
            Return retval
        End Function
        Public Function DrillDown(ByVal dt As String, ByVal fac As String, ByVal gt As String, ByVal tp As String, ByVal Fromdate As String, ByVal Todate As String, ByVal DataNo As String, ByVal WorkOrder As String, ByVal AuditType As String) As List(Of List(Of String))
            Dim retval As New List(Of List(Of String))()
            If fac <> "Global" And fac <> "Domestic" And fac <> "Interiors" And fac <> "Overall" Then
                Dim WhereString2 As String = ""

                If AuditType = "ALL" Then
                    WhereString2 = WhereString2 & " AND InspectionType != 'ROLL'"
                ElseIf AuditType = "FINAL AUDIT" Then
                    WhereString2 = WhereString2 & " AND InspectionType = 'EOL'"
                ElseIf AuditType = "IN LINE" Then
                    WhereString2 = WhereString2 & " AND InspectionType = 'IL'"
                Else
                    WhereString2 = WhereString2 & " AND InspectionType = '" & AuditType & "'"
                End If
                If DataNo <> "ALL" Then
                    WhereString2 = WhereString2 & " AND DataNo = '" & DataNo & "'"
                End If
                If WorkOrder <> "ALL" Then
                    WhereString2 = WhereString2 & " AND JobNumber = '" & WorkOrder & "'"
                End If
                Dim WhereString As String = " AND Inspection_Finished BETWEEN '" & Fromdate & "' AND '" & Todate & "'"
                If AuditType = "ALL" Then
                    WhereString = WhereString & " AND InspectionType != 'ROLL'"
                ElseIf AuditType = "FINAL AUDIT" Then
                    WhereString = WhereString & " AND InspectionType = 'EOL'"
                ElseIf AuditType = "IN LINE" Then
                    WhereString = WhereString & " AND InspectionType = 'IL'"
                Else
                    WhereString = WhereString & " AND InspectionType = '" & AuditType & "'"
                End If
                If DataNo <> "ALL" Then
                    WhereString = WhereString & " AND DataNo = '" & DataNo & "'"
                End If
                If WorkOrder <> "ALL" Then
                    WhereString = WhereString & " AND JobNumber = '" & WorkOrder & "'"
                End If
                Dim SQL As String = ""

                If gt = "No. of Defects" Then
                    Dim segment As New List(Of String)()
                    SQL = "select DefectDesc as DefectName, COUNT(DefectDesc) as DefectCount from (Inspection.dbo.Locations inner join dbo.InspectionJobSummaryYearly On dbo.InspectionJobSummaryYearly.CID=NCID) inner join Inspection.dbo.DefectMaster on InspectionJobSummaryYearly.id=Inspection.dbo.DefectMaster.InspectionJobSummaryId WHERE Name ='" & fac & "'" & WhereString2 & " AND Inspection_Finished >= '" & dt & "' and Inspection_Finished < dateadd(day,1,'" & dt & "') group by DefectDesc ORDER By DefectCount DESC"
                    Command.CommandType = CommandType.Text 'sets the type of the sql
                    Command.Connection = Connection 'sets the connection of our sql command to MyDB
                    Command.CommandText = SQL 'sets the statement that executes at the data source to our string
                    Connection.Open() 'opens the connction
                    DR = Command.ExecuteReader 'sends the command text to the connection and builds tthe SqlDataReader
                    While DR.Read() 'Check whether the SqlDataReader has 1 or more rows

                        segment = New List(Of String)()
                        segment.Add(DR("DefectName"))
                        segment.Add(DR("DefectCount"))
                        retval.Add(segment)


                    End While
                    Connection.Close() 'closes the connection
                    DR.Close() 'closes the reader
                    Return retval
                ElseIf gt = "No. of Rejects" Then
                    Dim segment As New List(Of String)()
                    SQL = "select DefectDesc as DefectName, COUNT(DefectDesc) as DefectCount from (Inspection.dbo.Locations inner join dbo.InspectionJobSummaryYearly On dbo.InspectionJobSummaryYearly.CID=NCID) inner join Inspection.dbo.DefectMaster on InspectionJobSummaryYearly.id=Inspection.dbo.DefectMaster.InspectionJobSummaryId WHERE Name ='" & fac & "'" & WhereString2 & " AND Inspection_Finished >= '" & dt & "' and Inspection_Finished < dateadd(day,1,'" & dt & "') AND DefectClass != 'MINOR'  group by DefectDesc ORDER By DefectCount DESC"
                    Command.CommandType = CommandType.Text 'sets the type of the sql
                    Command.Connection = Connection 'sets the connection of our sql command to MyDB
                    Command.CommandText = SQL 'sets the statement that executes at the data source to our string
                    Connection.Open() 'opens the connction
                    DR = Command.ExecuteReader 'sends the command text to the connection and builds tthe SqlDataReader
                    While DR.Read() 'Check whether the SqlDataReader has 1 or more rows

                        segment = New List(Of String)()
                        segment.Add(DR("DefectName"))
                        segment.Add(DR("DefectCount"))
                        retval.Add(segment)


                    End While
                    Connection.Close() 'closes the connection
                    DR.Close() 'closes the reader
                    Return retval

                ElseIf gt = "No. of Inspections" Then
                    Dim segment As New List(Of String)()
                    SQL = "select InspectionJobSummaryYearly.id AS 'Id', JobNumber AS 'JobNumber', DataNo As 'DataNumber' from Inspection.dbo.Locations inner join dbo.InspectionJobSummaryYearly On dbo.InspectionJobSummaryYearly.CID=NCID WHERE Name ='" & fac & "'" & WhereString2 & " AND Inspection_Finished >= '" & dt & "' and Inspection_Finished < dateadd(day,1, '" & dt & "')"
                    Command.CommandType = CommandType.Text 'sets the type of the sql
                    Command.Connection = Connection 'sets the connection of our sql command to MyDB
                    Command.CommandText = SQL 'sets the statement that executes at the data source to our string
                    Connection.Open() 'opens the connction
                    DR = Command.ExecuteReader 'sends the command text to the connection and builds tthe SqlDataReader
                    While DR.Read() 'Check whether the SqlDataReader has 1 or more rows

                        segment = New List(Of String)()
                        segment.Add(DR("Id"))
                        segment.Add(DR("JobNumber"))
                        segment.Add(DR("DataNumber"))
                        retval.Add(segment)


                    End While
                    Connection.Close() 'closes the connection
                    DR.Close() 'closes the reader
                    Return retval

                ElseIf gt = "No. of Rejected Lots" Then
                    Dim segment As New List(Of String)()
                    SQL = "select InspectionJobSummaryYearly.id AS 'Id', JobNumber AS 'JobNumber', DataNo As 'DataNumber' from Inspection.dbo.Locations inner join dbo.InspectionJobSummaryYearly On dbo.InspectionJobSummaryYearly.CID=NCID WHERE Name ='" & fac & "'" & WhereString2 & " AND Inspection_Finished >= '" & dt & "' and Inspection_Finished < dateadd(day,1, '" & dt & "') AND Technical_PassFail = 0"
                    Command.CommandType = CommandType.Text 'sets the type of the sql
                    Command.Connection = Connection 'sets the connection of our sql command to MyDB
                    Command.CommandText = SQL 'sets the statement that executes at the data source to our string
                    Connection.Open() 'opens the connction
                    DR = Command.ExecuteReader 'sends the command text to the connection and builds tthe SqlDataReader
                    While DR.Read() 'Check whether the SqlDataReader has 1 or more rows

                        segment = New List(Of String)()
                        segment.Add(DR("Id"))
                        segment.Add(DR("JobNumber"))
                        segment.Add(DR("DataNumber"))
                        retval.Add(segment)


                    End While
                    Connection.Close() 'closes the connection
                    DR.Close() 'closes the reader
                    Return retval


                End If
            ElseIf fac = "Global" Then
                Dim WhereString2 As String = "WHERE (CID = 114 OR CID = 115 OR CID = 590 OR CID = 1001 OR CID = 627)"

                If AuditType = "ALL" Then
                    WhereString2 = WhereString2 & " AND InspectionType != 'ROLL'"
                ElseIf AuditType = "FINAL AUDIT" Then
                    WhereString2 = WhereString2 & " AND InspectionType = 'EOL'"
                ElseIf AuditType = "IN LINE" Then
                    WhereString2 = WhereString2 & " AND InspectionType = 'IL'"
                Else
                    WhereString2 = WhereString2 & " AND InspectionType = '" & AuditType & "'"
                End If
                If DataNo <> "ALL" Then
                    WhereString2 = WhereString2 & " AND DataNo = '" & DataNo & "'"
                End If
                If WorkOrder <> "ALL" Then
                    WhereString2 = WhereString2 & " AND JobNumber = '" & WorkOrder & "'"
                End If
                Dim WhereString As String = "WHERE (CID = 114 OR CID = 115 OR CID = 590 OR CID = 1001 OR CID = 627) AND Inspection_Finished BETWEEN '" & Fromdate & "' AND '" & Todate & "'"
                If AuditType = "ALL" Then
                    WhereString = WhereString & " AND InspectionType != 'ROLL'"
                ElseIf AuditType = "FINAL AUDIT" Then
                    WhereString = WhereString & " AND InspectionType = 'EOL'"
                ElseIf AuditType = "IN LINE" Then
                    WhereString = WhereString & " AND InspectionType = 'IL'"
                Else
                    WhereString = WhereString & " AND InspectionType = '" & AuditType & "'"
                End If
                If DataNo <> "ALL" Then
                    WhereString = WhereString & " AND DataNo = '" & DataNo & "'"
                End If
                If WorkOrder <> "ALL" Then
                    WhereString = WhereString & " AND JobNumber = '" & WorkOrder & "'"
                End If
                Dim SQL As String = ""

                If gt = "No. of Defects" Then
                    Dim segment As New List(Of String)()
                    SQL = "select DefectDesc as DefectName, COUNT(DefectDesc) as DefectCount from dbo.InspectionJobSummaryYearly inner join Inspection.dbo.DefectMaster on InspectionJobSummaryYearly.id=Inspection.dbo.DefectMaster.InspectionJobSummaryId " & WhereString2 & " AND Inspection_Finished >= '" & dt & "' and Inspection_Finished < dateadd(day,1,'" & dt & "') group by DefectDesc ORDER By DefectCount DESC"
                    Command.CommandType = CommandType.Text 'sets the type of the sql
                    Command.Connection = Connection 'sets the connection of our sql command to MyDB
                    Command.CommandText = SQL 'sets the statement that executes at the data source to our string
                    Connection.Open() 'opens the connction
                    DR = Command.ExecuteReader 'sends the command text to the connection and builds tthe SqlDataReader
                    While DR.Read() 'Check whether the SqlDataReader has 1 or more rows

                        segment = New List(Of String)()
                        segment.Add(DR("DefectName"))
                        segment.Add(DR("DefectCount"))
                        retval.Add(segment)


                    End While
                    Connection.Close() 'closes the connection
                    DR.Close() 'closes the reader
                    Return retval
                ElseIf gt = "No. of Rejects" Then
                    Dim segment As New List(Of String)()
                    SQL = "select DefectDesc as DefectName, COUNT(DefectDesc) as DefectCount from dbo.InspectionJobSummaryYearly inner join Inspection.dbo.DefectMaster on InspectionJobSummaryYearly.id=Inspection.dbo.DefectMaster.InspectionJobSummaryId " & WhereString2 & " AND Inspection_Finished >= '" & dt & "' and Inspection_Finished < dateadd(day,1,'" & dt & "') AND DefectClass != 'MINOR'  group by DefectDesc ORDER By DefectCount DESC"
                    Command.CommandType = CommandType.Text 'sets the type of the sql
                    Command.Connection = Connection 'sets the connection of our sql command to MyDB
                    Command.CommandText = SQL 'sets the statement that executes at the data source to our string
                    Connection.Open() 'opens the connction
                    DR = Command.ExecuteReader 'sends the command text to the connection and builds tthe SqlDataReader
                    While DR.Read() 'Check whether the SqlDataReader has 1 or more rows

                        segment = New List(Of String)()
                        segment.Add(DR("DefectName"))
                        segment.Add(DR("DefectCount"))
                        retval.Add(segment)


                    End While
                    Connection.Close() 'closes the connection
                    DR.Close() 'closes the reader
                    Return retval

                ElseIf gt = "No. of Inspections" Then
                    Dim segment As New List(Of String)()
                    SQL = "select InspectionJobSummaryYearly.id AS 'Id', JobNumber AS 'JobNumber', DataNo As 'DataNumber' from dbo.InspectionJobSummaryYearly " & WhereString2 & " AND Inspection_Finished >= '" & dt & "' and Inspection_Finished < dateadd(day,1, '" & dt & "')"
                    Command.CommandType = CommandType.Text 'sets the type of the sql
                    Command.Connection = Connection 'sets the connection of our sql command to MyDB
                    Command.CommandText = SQL 'sets the statement that executes at the data source to our string
                    Connection.Open() 'opens the connction
                    DR = Command.ExecuteReader 'sends the command text to the connection and builds tthe SqlDataReader
                    While DR.Read() 'Check whether the SqlDataReader has 1 or more rows

                        segment = New List(Of String)()
                        segment.Add(DR("Id"))
                        segment.Add(DR("JobNumber"))
                        segment.Add(DR("DataNumber"))
                        retval.Add(segment)


                    End While
                    Connection.Close() 'closes the connection
                    DR.Close() 'closes the reader
                    Return retval

                ElseIf gt = "No. of Rejected Lots" Then
                    Dim segment As New List(Of String)()
                    SQL = "select InspectionJobSummaryYearly.id AS 'Id', JobNumber AS 'JobNumber', DataNo As 'DataNumber' from dbo.InspectionJobSummaryYearly " & WhereString2 & " AND Inspection_Finished >= '" & dt & "' and Inspection_Finished < dateadd(day,1, '" & dt & "') AND Technical_PassFail = 0"
                    Command.CommandType = CommandType.Text 'sets the type of the sql
                    Command.Connection = Connection 'sets the connection of our sql command to MyDB
                    Command.CommandText = SQL 'sets the statement that executes at the data source to our string
                    Connection.Open() 'opens the connction
                    DR = Command.ExecuteReader 'sends the command text to the connection and builds tthe SqlDataReader
                    While DR.Read() 'Check whether the SqlDataReader has 1 or more rows

                        segment = New List(Of String)()
                        segment.Add(DR("Id"))
                        segment.Add(DR("JobNumber"))
                        segment.Add(DR("DataNumber"))
                        retval.Add(segment)


                    End While
                    Connection.Close() 'closes the connection
                    DR.Close() 'closes the reader
                    Return retval



                End If
            ElseIf fac = "Domestic" Then
                Dim WhereString2 As String = "WHERE (CID = 578 OR CID = 485 OR CID = 482)"

                If AuditType = "ALL" Then
                    WhereString2 = WhereString2 & " AND InspectionType != 'ROLL'"
                ElseIf AuditType = "FINAL AUDIT" Then
                    WhereString2 = WhereString2 & " AND InspectionType = 'EOL'"
                ElseIf AuditType = "IN LINE" Then
                    WhereString2 = WhereString2 & " AND InspectionType = 'IL'"
                Else
                    WhereString2 = WhereString2 & " AND InspectionType = '" & AuditType & "'"
                End If
                If DataNo <> "ALL" Then
                    WhereString2 = WhereString2 & " AND DataNo = '" & DataNo & "'"
                End If
                If WorkOrder <> "ALL" Then
                    WhereString2 = WhereString2 & " AND JobNumber = '" & WorkOrder & "'"
                End If
                Dim WhereString As String = "WHERE (CID = 578 OR CID = 485 OR CID = 482) AND Inspection_Finished BETWEEN '" & Fromdate & "' AND '" & Todate & "'"
                If AuditType = "ALL" Then
                    WhereString = WhereString & " AND InspectionType != 'ROLL'"
                ElseIf AuditType = "FINAL AUDIT" Then
                    WhereString = WhereString & " AND InspectionType = 'EOL'"
                ElseIf AuditType = "IN LINE" Then
                    WhereString = WhereString & " AND InspectionType = 'IL'"
                Else
                    WhereString = WhereString & " AND InspectionType = '" & AuditType & "'"
                End If
                If DataNo <> "ALL" Then
                    WhereString = WhereString & " AND DataNo = '" & DataNo & "'"
                End If
                If WorkOrder <> "ALL" Then
                    WhereString = WhereString & " AND JobNumber = '" & WorkOrder & "'"
                End If
                Dim SQL As String = ""

                If gt = "No. of Defects" Then
                    Dim segment As New List(Of String)()
                    SQL = "select DefectDesc as DefectName, COUNT(DefectDesc) as DefectCount from dbo.InspectionJobSummaryYearly inner join Inspection.dbo.DefectMaster on InspectionJobSummaryYearly.id=Inspection.dbo.DefectMaster.InspectionJobSummaryId " & WhereString2 & " AND Inspection_Finished >= '" & dt & "' and Inspection_Finished < dateadd(day,1,'" & dt & "') group by DefectDesc ORDER By DefectCount DESC"
                    Command.CommandType = CommandType.Text 'sets the type of the sql
                    Command.Connection = Connection 'sets the connection of our sql command to MyDB
                    Command.CommandText = SQL 'sets the statement that executes at the data source to our string
                    Connection.Open() 'opens the connction
                    DR = Command.ExecuteReader 'sends the command text to the connection and builds tthe SqlDataReader
                    While DR.Read() 'Check whether the SqlDataReader has 1 or more rows

                        segment = New List(Of String)()
                        segment.Add(DR("DefectName"))
                        segment.Add(DR("DefectCount"))
                        retval.Add(segment)


                    End While
                    Connection.Close() 'closes the connection
                    DR.Close() 'closes the reader
                    Return retval
                ElseIf gt = "No. of Rejects" Then
                    Dim segment As New List(Of String)()
                    SQL = "select DefectDesc as DefectName, COUNT(DefectDesc) as DefectCount from dbo.InspectionJobSummaryYearly inner join Inspection.dbo.DefectMaster on InspectionJobSummaryYearly.id=Inspection.dbo.DefectMaster.InspectionJobSummaryId " & WhereString2 & " AND Inspection_Finished >= '" & dt & "' and Inspection_Finished < dateadd(day,1,'" & dt & "') AND DefectClass != 'MINOR'  group by DefectDesc ORDER By DefectCount DESC"
                    Command.CommandType = CommandType.Text 'sets the type of the sql
                    Command.Connection = Connection 'sets the connection of our sql command to MyDB
                    Command.CommandText = SQL 'sets the statement that executes at the data source to our string
                    Connection.Open() 'opens the connction
                    DR = Command.ExecuteReader 'sends the command text to the connection and builds tthe SqlDataReader
                    While DR.Read() 'Check whether the SqlDataReader has 1 or more rows

                        segment = New List(Of String)()
                        segment.Add(DR("DefectName"))
                        segment.Add(DR("DefectCount"))
                        retval.Add(segment)


                    End While
                    Connection.Close() 'closes the connection
                    DR.Close() 'closes the reader
                    Return retval

                ElseIf gt = "No. of Inspections" Then
                    Dim segment As New List(Of String)()
                    SQL = "select InspectionJobSummaryYearly.id AS 'Id', JobNumber AS 'JobNumber', DataNo As 'DataNumber' from dbo.InspectionJobSummaryYearly " & WhereString2 & " AND Inspection_Finished >= '" & dt & "' and Inspection_Finished < dateadd(day,1, '" & dt & "')"
                    Command.CommandType = CommandType.Text 'sets the type of the sql
                    Command.Connection = Connection 'sets the connection of our sql command to MyDB
                    Command.CommandText = SQL 'sets the statement that executes at the data source to our string
                    Connection.Open() 'opens the connction
                    DR = Command.ExecuteReader 'sends the command text to the connection and builds tthe SqlDataReader
                    While DR.Read() 'Check whether the SqlDataReader has 1 or more rows

                        segment = New List(Of String)()
                        segment.Add(DR("Id"))
                        segment.Add(DR("JobNumber"))
                        segment.Add(DR("DataNumber"))
                        retval.Add(segment)


                    End While
                    Connection.Close() 'closes the connection
                    DR.Close() 'closes the reader
                    Return retval

                ElseIf gt = "No. of Rejected Lots" Then
                    Dim segment As New List(Of String)()
                    SQL = "select InspectionJobSummaryYearly.id AS 'Id', JobNumber AS 'JobNumber', DataNo As 'DataNumber' from dbo.InspectionJobSummaryYearly " & WhereString2 & " AND Inspection_Finished >= '" & dt & "' and Inspection_Finished < dateadd(day,1, '" & dt & "') AND Technical_PassFail = 0"
                    Command.CommandType = CommandType.Text 'sets the type of the sql
                    Command.Connection = Connection 'sets the connection of our sql command to MyDB
                    Command.CommandText = SQL 'sets the statement that executes at the data source to our string
                    Connection.Open() 'opens the connction
                    DR = Command.ExecuteReader 'sends the command text to the connection and builds tthe SqlDataReader
                    While DR.Read() 'Check whether the SqlDataReader has 1 or more rows

                        segment = New List(Of String)()
                        segment.Add(DR("Id"))
                        segment.Add(DR("JobNumber"))
                        segment.Add(DR("DataNumber"))
                        retval.Add(segment)


                    End While
                    Connection.Close() 'closes the connection
                    DR.Close() 'closes the reader
                    Return retval
                End If
            ElseIf fac = "Interiors" Then
                Dim WhereString2 As String = "WHERE (CID = 112 OR CID = 111)"

                If AuditType = "ALL" Then
                    WhereString2 = WhereString2 & " AND InspectionType != 'ROLL'"
                ElseIf AuditType = "FINAL AUDIT" Then
                    WhereString2 = WhereString2 & " AND InspectionType = 'EOL'"
                ElseIf AuditType = "IN LINE" Then
                    WhereString2 = WhereString2 & " AND InspectionType = 'IL'"
                Else
                    WhereString2 = WhereString2 & " AND InspectionType = '" & AuditType & "'"
                End If
                If DataNo <> "ALL" Then
                    WhereString2 = WhereString2 & " AND DataNo = '" & DataNo & "'"
                End If
                If WorkOrder <> "ALL" Then
                    WhereString2 = WhereString2 & " AND JobNumber = '" & WorkOrder & "'"
                End If
                Dim WhereString As String = "WHERE (CID = 112 OR CID = 111) AND Inspection_Finished BETWEEN '" & Fromdate & "' AND '" & Todate & "'"
                If AuditType = "ALL" Then
                    WhereString = WhereString & " AND InspectionType != 'ROLL'"
                ElseIf AuditType = "FINAL AUDIT" Then
                    WhereString = WhereString & " AND InspectionType = 'EOL'"
                ElseIf AuditType = "IN LINE" Then
                    WhereString = WhereString & " AND InspectionType = 'IL'"
                Else
                    WhereString = WhereString & " AND InspectionType = '" & AuditType & "'"
                End If
                If DataNo <> "ALL" Then
                    WhereString = WhereString & " AND DataNo = '" & DataNo & "'"
                End If
                If WorkOrder <> "ALL" Then
                    WhereString = WhereString & " AND JobNumber = '" & WorkOrder & "'"
                End If
                Dim SQL As String = ""

                If gt = "No. of Defects" Then
                    Dim segment As New List(Of String)()
                    SQL = "select DefectDesc as DefectName, COUNT(DefectDesc) as DefectCount from dbo.InspectionJobSummaryYearly inner join Inspection.dbo.DefectMaster on InspectionJobSummaryYearly.id=Inspection.dbo.DefectMaster.InspectionJobSummaryId " & WhereString2 & " AND Inspection_Finished >= '" & dt & "' and Inspection_Finished < dateadd(day,1,'" & dt & "') group by DefectDesc ORDER By DefectCount DESC"
                    Command.CommandType = CommandType.Text 'sets the type of the sql
                    Command.Connection = Connection 'sets the connection of our sql command to MyDB
                    Command.CommandText = SQL 'sets the statement that executes at the data source to our string
                    Connection.Open() 'opens the connction
                    DR = Command.ExecuteReader 'sends the command text to the connection and builds tthe SqlDataReader
                    While DR.Read() 'Check whether the SqlDataReader has 1 or more rows

                        segment = New List(Of String)()
                        segment.Add(DR("DefectName"))
                        segment.Add(DR("DefectCount"))
                        retval.Add(segment)


                    End While
                    Connection.Close() 'closes the connection
                    DR.Close() 'closes the reader
                    Return retval
                ElseIf gt = "No. of Rejects" Then
                    Dim segment As New List(Of String)()
                    SQL = "select DefectDesc as DefectName, COUNT(DefectDesc) as DefectCount from dbo.InspectionJobSummaryYearly inner join Inspection.dbo.DefectMaster on InspectionJobSummaryYearly.id=Inspection.dbo.DefectMaster.InspectionJobSummaryId " & WhereString2 & " AND Inspection_Finished >= '" & dt & "' and Inspection_Finished < dateadd(day,1,'" & dt & "') AND DefectClass != 'MINOR'  group by DefectDesc ORDER By DefectCount DESC"
                    Command.CommandType = CommandType.Text 'sets the type of the sql
                    Command.Connection = Connection 'sets the connection of our sql command to MyDB
                    Command.CommandText = SQL 'sets the statement that executes at the data source to our string
                    Connection.Open() 'opens the connction
                    DR = Command.ExecuteReader 'sends the command text to the connection and builds tthe SqlDataReader
                    While DR.Read() 'Check whether the SqlDataReader has 1 or more rows

                        segment = New List(Of String)()
                        segment.Add(DR("DefectName"))
                        segment.Add(DR("DefectCount"))
                        retval.Add(segment)


                    End While
                    Connection.Close() 'closes the connection
                    DR.Close() 'closes the reader
                    Return retval

                ElseIf gt = "No. of Inspections" Then
                    Dim segment As New List(Of String)()
                    SQL = "select InspectionJobSummaryYearly.id AS 'Id', JobNumber AS 'JobNumber', DataNo As 'DataNumber' from dbo.InspectionJobSummaryYearly " & WhereString2 & " AND Inspection_Finished >= '" & dt & "' and Inspection_Finished < dateadd(day,1, '" & dt & "')"
                    Command.CommandType = CommandType.Text 'sets the type of the sql
                    Command.Connection = Connection 'sets the connection of our sql command to MyDB
                    Command.CommandText = SQL 'sets the statement that executes at the data source to our string
                    Connection.Open() 'opens the connction
                    DR = Command.ExecuteReader 'sends the command text to the connection and builds tthe SqlDataReader
                    While DR.Read() 'Check whether the SqlDataReader has 1 or more rows

                        segment = New List(Of String)()
                        segment.Add(DR("Id"))
                        segment.Add(DR("JobNumber"))
                        segment.Add(DR("DataNumber"))
                        retval.Add(segment)


                    End While
                    Connection.Close() 'closes the connection
                    DR.Close() 'closes the reader
                    Return retval

                ElseIf gt = "No. of Rejected Lots" Then
                    Dim segment As New List(Of String)()
                    SQL = "select InspectionJobSummaryYearly.id AS 'Id', JobNumber AS 'JobNumber', DataNo As 'DataNumber' from dbo.InspectionJobSummaryYearly " & WhereString2 & " AND Inspection_Finished >= '" & dt & "' and Inspection_Finished < dateadd(day,1, '" & dt & "') AND Technical_PassFail = 0"
                    Command.CommandType = CommandType.Text 'sets the type of the sql
                    Command.Connection = Connection 'sets the connection of our sql command to MyDB
                    Command.CommandText = SQL 'sets the statement that executes at the data source to our string
                    Connection.Open() 'opens the connction
                    DR = Command.ExecuteReader 'sends the command text to the connection and builds tthe SqlDataReader
                    While DR.Read() 'Check whether the SqlDataReader has 1 or more rows

                        segment = New List(Of String)()
                        segment.Add(DR("Id"))
                        segment.Add(DR("JobNumber"))
                        segment.Add(DR("DataNumber"))
                        retval.Add(segment)


                    End While
                    Connection.Close() 'closes the connection
                    DR.Close() 'closes the reader
                    Return retval
                End If
            ElseIf fac = "Overall" Then
                Dim WhereString2 As String = "WHERE CID IS NOT NULL"

                If AuditType = "ALL" Then
                    WhereString2 = WhereString2 & " AND InspectionType != 'ROLL'"
                ElseIf AuditType = "FINAL AUDIT" Then
                    WhereString2 = WhereString2 & " AND InspectionType = 'EOL'"
                ElseIf AuditType = "IN LINE" Then
                    WhereString2 = WhereString2 & " AND InspectionType = 'IL'"
                Else
                    WhereString2 = WhereString2 & " AND InspectionType = '" & AuditType & "'"
                End If
                If DataNo <> "ALL" Then
                    WhereString2 = WhereString2 & " AND DataNo = '" & DataNo & "'"
                End If
                If WorkOrder <> "ALL" Then
                    WhereString2 = WhereString2 & " AND JobNumber = '" & WorkOrder & "'"
                End If
                Dim WhereString As String = "WHERE CID IS NOT NULL AND Inspection_Finished BETWEEN '" & Fromdate & "' AND '" & Todate & "'"
                If AuditType = "ALL" Then
                    WhereString = WhereString & " AND InspectionType != 'ROLL'"
                ElseIf AuditType = "FINAL AUDIT" Then
                    WhereString = WhereString & " AND InspectionType = 'EOL'"
                ElseIf AuditType = "IN LINE" Then
                    WhereString = WhereString & " AND InspectionType = 'IL'"
                Else
                    WhereString = WhereString & " AND InspectionType = '" & AuditType & "'"
                End If
                If DataNo <> "ALL" Then
                    WhereString = WhereString & " AND DataNo = '" & DataNo & "'"
                End If
                If WorkOrder <> "ALL" Then
                    WhereString = WhereString & " AND JobNumber = '" & WorkOrder & "'"
                End If
                Dim SQL As String = ""

                If gt = "No. of Defects" Then
                    Dim segment As New List(Of String)()
                    SQL = "select DefectDesc as DefectName, COUNT(DefectDesc) as DefectCount from dbo.InspectionJobSummaryYearly inner join Inspection.dbo.DefectMaster on InspectionJobSummaryYearly.id=Inspection.dbo.DefectMaster.InspectionJobSummaryId " & WhereString2 & " AND Inspection_Finished >= '" & dt & "' and Inspection_Finished < dateadd(day,1,'" & dt & "') group by DefectDesc ORDER By DefectCount DESC"
                    Command.CommandType = CommandType.Text 'sets the type of the sql
                    Command.Connection = Connection 'sets the connection of our sql command to MyDB
                    Command.CommandText = SQL 'sets the statement that executes at the data source to our string
                    Connection.Open() 'opens the connction
                    DR = Command.ExecuteReader 'sends the command text to the connection and builds tthe SqlDataReader
                    While DR.Read() 'Check whether the SqlDataReader has 1 or more rows

                        segment = New List(Of String)()
                        segment.Add(DR("DefectName"))
                        segment.Add(DR("DefectCount"))
                        retval.Add(segment)


                    End While
                    Connection.Close() 'closes the connection
                    DR.Close() 'closes the reader
                    Return retval
                ElseIf gt = "No. of Rejects" Then
                    Dim segment As New List(Of String)()
                    SQL = "select DefectDesc as DefectName, COUNT(DefectDesc) as DefectCount from dbo.InspectionJobSummaryYearly inner join Inspection.dbo.DefectMaster on InspectionJobSummaryYearly.id=Inspection.dbo.DefectMaster.InspectionJobSummaryId " & WhereString2 & " AND Inspection_Finished >= '" & dt & "' and Inspection_Finished < dateadd(day,1,'" & dt & "') AND DefectClass != 'MINOR'  group by DefectDesc ORDER By DefectCount DESC"
                    Command.CommandType = CommandType.Text 'sets the type of the sql
                    Command.Connection = Connection 'sets the connection of our sql command to MyDB
                    Command.CommandText = SQL 'sets the statement that executes at the data source to our string
                    Connection.Open() 'opens the connction
                    DR = Command.ExecuteReader 'sends the command text to the connection and builds tthe SqlDataReader
                    While DR.Read() 'Check whether the SqlDataReader has 1 or more rows

                        segment = New List(Of String)()
                        segment.Add(DR("DefectName"))
                        segment.Add(DR("DefectCount"))
                        retval.Add(segment)


                    End While
                    Connection.Close() 'closes the connection
                    DR.Close() 'closes the reader
                    Return retval

                ElseIf gt = "No. of Inspections" Then
                    Dim segment As New List(Of String)()
                    SQL = "select InspectionJobSummaryYearly.id AS 'Id', JobNumber AS 'JobNumber', DataNo As 'DataNumber' from dbo.InspectionJobSummaryYearly " & WhereString2 & " AND Inspection_Finished >= '" & dt & "' and Inspection_Finished < dateadd(day,1, '" & dt & "')"
                    Command.CommandType = CommandType.Text 'sets the type of the sql
                    Command.Connection = Connection 'sets the connection of our sql command to MyDB
                    Command.CommandText = SQL 'sets the statement that executes at the data source to our string
                    Connection.Open() 'opens the connction
                    DR = Command.ExecuteReader 'sends the command text to the connection and builds tthe SqlDataReader
                    While DR.Read() 'Check whether the SqlDataReader has 1 or more rows

                        segment = New List(Of String)()
                        segment.Add(DR("Id"))
                        segment.Add(DR("JobNumber"))
                        segment.Add(DR("DataNumber"))
                        retval.Add(segment)


                    End While
                    Connection.Close() 'closes the connection
                    DR.Close() 'closes the reader
                    Return retval

                ElseIf gt = "No. of Rejected Lots" Then
                    Dim segment As New List(Of String)()
                    SQL = "select InspectionJobSummaryYearly.id AS 'Id', JobNumber AS 'JobNumber', DataNo As 'DataNumber' from dbo.InspectionJobSummaryYearly " & WhereString2 & " AND Inspection_Finished >= '" & dt & "' and Inspection_Finished < dateadd(day,1, '" & dt & "') AND Technical_PassFail = 0"
                    Command.CommandType = CommandType.Text 'sets the type of the sql
                    Command.Connection = Connection 'sets the connection of our sql command to MyDB
                    Command.CommandText = SQL 'sets the statement that executes at the data source to our string
                    Connection.Open() 'opens the connction
                    DR = Command.ExecuteReader 'sends the command text to the connection and builds tthe SqlDataReader
                    While DR.Read() 'Check whether the SqlDataReader has 1 or more rows

                        segment = New List(Of String)()
                        segment.Add(DR("Id"))
                        segment.Add(DR("JobNumber"))
                        segment.Add(DR("DataNumber"))
                        retval.Add(segment)


                    End While
                    Connection.Close() 'closes the connection
                    DR.Close() 'closes the reader
                    Return retval
                End If
            End If
            Return retval
        End Function
        Public Function DrawChart(ByVal fac As String, ByVal gt As String, ByVal tp As String, ByVal Fromdate As String, ByVal Todate As String, ByVal DataNo As String, ByVal WorkOrder As String, ByVal AuditType As String) As List(Of List(Of String))
            Dim retval As New List(Of List(Of String))()
            If fac <> "Global" And fac <> "Domestic" And fac <> "Interiors" And fac <> "Overall" Then
                Dim WhereString2 As String = ""
                If AuditType = "ALL" Then
                    WhereString2 = WhereString2 & " AND InspectionType != 'ROLL'"
                ElseIf AuditType = "FINAL AUDIT" Then
                    WhereString2 = WhereString2 & " AND InspectionType = 'EOL'"
                ElseIf AuditType = "IN LINE" Then
                    WhereString2 = WhereString2 & " AND InspectionType = 'IL'"
                Else
                    WhereString2 = WhereString2 & " AND InspectionType = '" & AuditType & "'"
                End If
                If DataNo <> "ALL" Then
                    WhereString2 = WhereString2 & " AND DataNo = '" & DataNo & "'"
                End If
                If WorkOrder <> "ALL" Then
                    WhereString2 = WhereString2 & " AND JobNumber = '" & WorkOrder & "'"
                End If
                Dim WhereString As String = " AND Inspection_Finished BETWEEN '" & Fromdate & "' AND '" & Todate & "'"
                If AuditType = "ALL" Then
                    WhereString = WhereString & " AND InspectionType != 'ROLL'"
                ElseIf AuditType = "FINAL AUDIT" Then
                    WhereString = WhereString & " AND InspectionType = 'EOL'"
                ElseIf AuditType = "IN LINE" Then
                    WhereString = WhereString & " AND InspectionType = 'IL'"
                Else
                    WhereString = WhereString & " AND InspectionType = '" & AuditType & "'"
                End If
                If DataNo <> "ALL" Then
                    WhereString = WhereString & " AND DataNo = '" & DataNo & "'"
                End If
                If WorkOrder <> "ALL" Then
                    WhereString = WhereString & " AND JobNumber = '" & WorkOrder & "'"
                End If
                Dim SQL As String = ""

                If gt = "No. of Defects" And tp = "Past Year" Then
                    Dim segment As New List(Of String)()
                    SQL = "Select ISNULL(SUM(MajorsCount+MinorsCount+RepairsCount+ScrapCount), 0) AS TOTAL, dateadd(DAY,0, datediff(day,0, Inspection_Finished)) AS Comp_Date from Inspection.dbo.Locations inner join dbo.InspectionJobSummaryYearly On dbo.InspectionJobSummaryYearly.CID=NCID WHERE Name ='" & fac & "'" & WhereString2 & "Group by dateadd(DAY, 0, DateDiff(Day, 0, Inspection_Finished))  ORDER BY Comp_Date ASC"
                    Command.CommandType = CommandType.Text 'sets the type of the sql
                    Command.Connection = Connection 'sets the connection of our sql command to MyDB
                    Command.CommandText = SQL 'sets the statement that executes at the data source to our string
                    Connection.Open() 'opens the connction
                    DR = Command.ExecuteReader 'sends the command text to the connection and builds tthe SqlDataReader
                    While DR.Read() 'Check whether the SqlDataReader has 1 or more rows

                        segment = New List(Of String)()
                        segment.Add(DR("Comp_Date"))
                        segment.Add(DR("TOTAL"))
                        retval.Add(segment)


                    End While
                    Connection.Close() 'closes the connection
                    DR.Close() 'closes the reader
                    Return retval
                ElseIf gt = "No. of Defects" And tp = "Past 30 Days" Then
                    Dim segment As New List(Of String)()
                    SQL = "Select ISNULL(SUM(MajorsCount+MinorsCount+RepairsCount+ScrapCount), 0) AS TOTAL, dateadd(DAY,0, datediff(day,0, Inspection_Finished)) AS Comp_Date from Inspection.dbo.Locations inner join dbo.InspectionJobSummaryYearly On dbo.InspectionJobSummaryYearly.CID=NCID WHERE Name ='" & fac & "'" & WhereString2 & " And Inspection_Finished >= DATEADD(month,-1,GETDATE()) Group by dateadd(DAY, 0, DateDiff(Day, 0, Inspection_Finished))  ORDER BY Comp_Date ASC"
                    Command.CommandType = CommandType.Text 'sets the type of the sql
                    Command.Connection = Connection 'sets the connection of our sql command to MyDB
                    Command.CommandText = SQL 'sets the statement that executes at the data source to our string
                    Connection.Open() 'opens the connction
                    DR = Command.ExecuteReader 'sends the command text to the connection and builds tthe SqlDataReader
                    While DR.Read() 'Check whether the SqlDataReader has 1 or more rows

                        segment = New List(Of String)()
                        segment.Add(DR("Comp_Date"))
                        segment.Add(DR("TOTAL"))
                        retval.Add(segment)


                    End While
                    Connection.Close() 'closes the connection
                    DR.Close() 'closes the reader
                    Return retval
                ElseIf gt = "No. of Defects" And tp = "Custom" Then
                    Dim segment As New List(Of String)()
                    SQL = "Select ISNULL(SUM(MajorsCount+MinorsCount+RepairsCount+ScrapCount), 0) AS TOTAL, dateadd(DAY,0, datediff(day,0, Inspection_Finished)) AS Comp_Date from Inspection.dbo.Locations inner join dbo.InspectionJobSummaryYearly On dbo.InspectionJobSummaryYearly.CID=NCID WHERE Name ='" & fac & "'" & WhereString & " Group by dateadd(DAY, 0, DateDiff(Day, 0, Inspection_Finished))  ORDER BY Comp_Date ASC"
                    Command.CommandType = CommandType.Text 'sets the type of the sql
                    Command.Connection = Connection 'sets the connection of our sql command to MyDB
                    Command.CommandText = SQL 'sets the statement that executes at the data source to our string
                    Connection.Open() 'opens the connction
                    DR = Command.ExecuteReader 'sends the command text to the connection and builds tthe SqlDataReader
                    While DR.Read() 'Check whether the SqlDataReader has 1 or more rows

                        segment = New List(Of String)()
                        segment.Add(DR("Comp_Date"))
                        segment.Add(DR("TOTAL"))
                        retval.Add(segment)


                    End While
                    Connection.Close() 'closes the connection
                    DR.Close() 'closes the reader
                    Return retval
                ElseIf gt = "No. of Rejects" And tp = "Past Year" Then
                    Dim segment As New List(Of String)()
                    SQL = "Select ISNULL(SUM(MajorsCount+RepairsCount+ScrapCount), 0) AS TOTAL, dateadd(DAY,0, datediff(day,0, Inspection_Finished)) AS Comp_Date from Inspection.dbo.Locations inner join dbo.InspectionJobSummaryYearly On dbo.InspectionJobSummaryYearly.CID=NCID WHERE Name ='" & fac & "'" & WhereString2 & " Group by dateadd(DAY, 0, DateDiff(Day, 0, Inspection_Finished))  ORDER BY Comp_Date ASC"
                    Command.CommandType = CommandType.Text 'sets the type of the sql
                    Command.Connection = Connection 'sets the connection of our sql command to MyDB
                    Command.CommandText = SQL 'sets the statement that executes at the data source to our string
                    Connection.Open() 'opens the connction
                    DR = Command.ExecuteReader 'sends the command text to the connection and builds tthe SqlDataReader
                    While DR.Read() 'Check whether the SqlDataReader has 1 or more rows

                        segment = New List(Of String)()
                        segment.Add(DR("Comp_Date"))
                        segment.Add(DR("TOTAL"))
                        retval.Add(segment)


                    End While
                    Connection.Close() 'closes the connection
                    DR.Close() 'closes the reader
                    Return retval
                ElseIf gt = "No. of Rejects" And tp = "Past 30 Days" Then
                    Dim segment As New List(Of String)()
                    SQL = "Select ISNULL(SUM(MajorsCount+RepairsCount+ScrapCount), 0) AS TOTAL, dateadd(DAY,0, datediff(day,0, Inspection_Finished)) AS Comp_Date from Inspection.dbo.Locations inner join dbo.InspectionJobSummaryYearly On dbo.InspectionJobSummaryYearly.CID=NCID WHERE Name ='" & fac & "'" & WhereString2 & " And Inspection_Finished >= DATEADD(month,-1,GETDATE()) Group by dateadd(DAY, 0, DateDiff(Day, 0, Inspection_Finished))  ORDER BY Comp_Date ASC"
                    Command.CommandType = CommandType.Text 'sets the type of the sql
                    Command.Connection = Connection 'sets the connection of our sql command to MyDB
                    Command.CommandText = SQL 'sets the statement that executes at the data source to our string
                    Connection.Open() 'opens the connction
                    DR = Command.ExecuteReader 'sends the command text to the connection and builds tthe SqlDataReader
                    While DR.Read() 'Check whether the SqlDataReader has 1 or more rows

                        segment = New List(Of String)()
                        segment.Add(DR("Comp_Date"))
                        segment.Add(DR("TOTAL"))
                        retval.Add(segment)


                    End While
                    Connection.Close() 'closes the connection
                    DR.Close() 'closes the reader
                    Return retval
                ElseIf gt = "No. of Rejects" And tp = "Custom" Then
                    Dim segment As New List(Of String)()
                    SQL = "Select ISNULL(SUM(MajorsCount+RepairsCount+ScrapCount), 0) AS TOTAL, dateadd(DAY,0, datediff(day,0, Inspection_Finished)) AS Comp_Date from Inspection.dbo.Locations inner join dbo.InspectionJobSummaryYearly On dbo.InspectionJobSummaryYearly.CID=NCID WHERE Name ='" & fac & "'" & WhereString & "Group by dateadd(DAY, 0, DateDiff(Day, 0, Inspection_Finished))  ORDER BY Comp_Date ASC"
                    Command.CommandType = CommandType.Text 'sets the type of the sql
                    Command.Connection = Connection 'sets the connection of our sql command to MyDB
                    Command.CommandText = SQL 'sets the statement that executes at the data source to our string
                    Connection.Open() 'opens the connction
                    DR = Command.ExecuteReader 'sends the command text to the connection and builds tthe SqlDataReader
                    While DR.Read() 'Check whether the SqlDataReader has 1 or more rows

                        segment = New List(Of String)()
                        segment.Add(DR("Comp_Date"))
                        segment.Add(DR("TOTAL"))
                        retval.Add(segment)


                    End While
                    Connection.Close() 'closes the connection
                    DR.Close() 'closes the reader
                    Return retval
                ElseIf gt = "No. of Inspections" And tp = "Past Year" Then
                    Dim segment As New List(Of String)()
                    SQL = "Select COUNT(InspectionJobSummaryYearly.id) AS TOTAL, dateadd(DAY,0, datediff(day,0, Inspection_Finished)) AS Comp_Date from Inspection.dbo.Locations inner join dbo.InspectionJobSummaryYearly On dbo.InspectionJobSummaryYearly.CID=NCID WHERE Name ='" & fac & "'" & WhereString2 & " Group by dateadd(DAY, 0, DateDiff(Day, 0, Inspection_Finished))  ORDER BY Comp_Date ASC"
                    Command.CommandType = CommandType.Text 'sets the type of the sql
                    Command.Connection = Connection 'sets the connection of our sql command to MyDB
                    Command.CommandText = SQL 'sets the statement that executes at the data source to our string
                    Connection.Open() 'opens the connction
                    DR = Command.ExecuteReader 'sends the command text to the connection and builds tthe SqlDataReader
                    While DR.Read() 'Check whether the SqlDataReader has 1 or more rows

                        segment = New List(Of String)()
                        segment.Add(DR("Comp_Date"))
                        segment.Add(DR("TOTAL"))
                        retval.Add(segment)


                    End While
                    Connection.Close() 'closes the connection
                    DR.Close() 'closes the reader
                    Return retval
                ElseIf gt = "No. of Inspections" And tp = "Past 30 Days" Then
                    Dim segment As New List(Of String)()
                    SQL = "Select COUNT(InspectionJobSummaryYearly.id) AS TOTAL, dateadd(DAY,0, datediff(day,0, Inspection_Finished)) AS Comp_Date from Inspection.dbo.Locations inner join dbo.InspectionJobSummaryYearly On dbo.InspectionJobSummaryYearly.CID=NCID WHERE Name ='" & fac & "'" & WhereString2 & " And Inspection_Finished >= DATEADD(month,-1,GETDATE()) Group by dateadd(DAY, 0, DateDiff(Day, 0, Inspection_Finished))  ORDER BY Comp_Date ASC"
                    Command.CommandType = CommandType.Text 'sets the type of the sql
                    Command.Connection = Connection 'sets the connection of our sql command to MyDB
                    Command.CommandText = SQL 'sets the statement that executes at the data source to our string
                    Connection.Open() 'opens the connction
                    DR = Command.ExecuteReader 'sends the command text to the connection and builds tthe SqlDataReader
                    While DR.Read() 'Check whether the SqlDataReader has 1 or more rows

                        segment = New List(Of String)()
                        segment.Add(DR("Comp_Date"))
                        segment.Add(DR("TOTAL"))
                        retval.Add(segment)


                    End While
                    Connection.Close() 'closes the connection
                    DR.Close() 'closes the reader
                    Return retval
                ElseIf gt = "No. of Inspections" And tp = "Custom" Then
                    Dim segment As New List(Of String)()
                    SQL = "Select COUNT(InspectionJobSummaryYearly.id) AS TOTAL, dateadd(DAY,0, datediff(day,0, Inspection_Finished)) AS Comp_Date from Inspection.dbo.Locations inner join dbo.InspectionJobSummaryYearly On dbo.InspectionJobSummaryYearly.CID=NCID WHERE Name ='" & fac & "'" & WhereString & " Group by dateadd(DAY, 0, DateDiff(Day, 0, Inspection_Finished))  ORDER BY Comp_Date ASC"
                    Command.CommandType = CommandType.Text 'sets the type of the sql
                    Command.Connection = Connection 'sets the connection of our sql command to MyDB
                    Command.CommandText = SQL 'sets the statement that executes at the data source to our string
                    Connection.Open() 'opens the connction
                    DR = Command.ExecuteReader 'sends the command text to the connection and builds tthe SqlDataReader
                    While DR.Read() 'Check whether the SqlDataReader has 1 or more rows

                        segment = New List(Of String)()
                        segment.Add(DR("Comp_Date"))
                        segment.Add(DR("TOTAL"))
                        retval.Add(segment)


                    End While
                    Connection.Close() 'closes the connection
                    DR.Close() 'closes the reader
                    Return retval
                ElseIf gt = "No. of Rejected Lots" And tp = "Past Year" Then
                    Dim segment As New List(Of String)()
                    SQL = "Select COUNT(InspectionJobSummaryYearly.id) AS TOTAL, dateadd(DAY,0, datediff(day,0, Inspection_Finished)) AS Comp_Date from Inspection.dbo.Locations inner join dbo.InspectionJobSummaryYearly On dbo.InspectionJobSummaryYearly.CID=NCID WHERE Name ='" & fac & "'" & WhereString2 & " AND Technical_PassFail=0 Group by dateadd(DAY, 0, DateDiff(Day, 0, Inspection_Finished))  ORDER BY Comp_Date ASC"
                    Command.CommandType = CommandType.Text 'sets the type of the sql
                    Command.Connection = Connection 'sets the connection of our sql command to MyDB
                    Command.CommandText = SQL 'sets the statement that executes at the data source to our string
                    Connection.Open() 'opens the connction
                    DR = Command.ExecuteReader 'sends the command text to the connection and builds tthe SqlDataReader
                    While DR.Read() 'Check whether the SqlDataReader has 1 or more rows

                        segment = New List(Of String)()
                        segment.Add(DR("Comp_Date"))
                        segment.Add(DR("TOTAL"))
                        retval.Add(segment)


                    End While
                    Connection.Close() 'closes the connection
                    DR.Close() 'closes the reader
                    Return retval
                ElseIf gt = "No. of Rejected Lots" And tp = "Past 30 Days" Then
                    Dim segment As New List(Of String)()
                    SQL = "Select COUNT(InspectionJobSummaryYearly.id) AS TOTAL, dateadd(DAY,0, datediff(day,0, Inspection_Finished)) AS Comp_Date from Inspection.dbo.Locations inner join dbo.InspectionJobSummaryYearly On dbo.InspectionJobSummaryYearly.CID=NCID WHERE Name ='" & fac & "'" & WhereString2 & " And Inspection_Finished >= DATEADD(month,-1,GETDATE()) AND Technical_PassFail=0 Group by dateadd(DAY, 0, DateDiff(Day, 0, Inspection_Finished))  ORDER BY Comp_Date ASC"
                    Command.CommandType = CommandType.Text 'sets the type of the sql
                    Command.Connection = Connection 'sets the connection of our sql command to MyDB
                    Command.CommandText = SQL 'sets the statement that executes at the data source to our string
                    Connection.Open() 'opens the connction
                    DR = Command.ExecuteReader 'sends the command text to the connection and builds tthe SqlDataReader
                    While DR.Read() 'Check whether the SqlDataReader has 1 or more rows

                        segment = New List(Of String)()
                        segment.Add(DR("Comp_Date"))
                        segment.Add(DR("TOTAL"))
                        retval.Add(segment)


                    End While
                    Connection.Close() 'closes the connection
                    DR.Close() 'closes the reader
                    Return retval
                ElseIf gt = "No. of Rejected Lots" And tp = "Custom" Then
                    Dim segment As New List(Of String)()
                    SQL = "Select COUNT(InspectionJobSummaryYearly.id) AS TOTAL, dateadd(DAY,0, datediff(day,0, Inspection_Finished)) AS Comp_Date from Inspection.dbo.Locations inner join dbo.InspectionJobSummaryYearly On dbo.InspectionJobSummaryYearly.CID=NCID WHERE Name ='" & fac & "'" & WhereString & " AND Technical_PassFail=0 Group by dateadd(DAY, 0, DateDiff(Day, 0, Inspection_Finished))  ORDER BY Comp_Date ASC"
                    Command.CommandType = CommandType.Text 'sets the type of the sql
                    Command.Connection = Connection 'sets the connection of our sql command to MyDB
                    Command.CommandText = SQL 'sets the statement that executes at the data source to our string
                    Connection.Open() 'opens the connction
                    DR = Command.ExecuteReader 'sends the command text to the connection and builds tthe SqlDataReader
                    While DR.Read() 'Check whether the SqlDataReader has 1 or more rows

                        segment = New List(Of String)()
                        segment.Add(DR("Comp_Date"))
                        segment.Add(DR("TOTAL"))
                        retval.Add(segment)


                    End While
                    Connection.Close() 'closes the connection
                    DR.Close() 'closes the reader
                    Return retval
                ElseIf gt = "DHU" And tp = "Past Year" Then
                    Dim segment As New List(Of String)()
                    SQL = "Select ISNULL((CAST(SUM(MajorsCount+MinorsCount+RepairsCount+ScrapCount) AS Decimal(10,2))/CAST(SUM(SampleSize) AS Decimal(10,2)))*100, 0) AS TOTAL, dateadd(DAY,0, datediff(day,0, Inspection_Finished)) AS Comp_Date from Inspection.dbo.Locations inner join dbo.InspectionJobSummaryYearly On dbo.InspectionJobSummaryYearly.CID=NCID WHERE Name ='" & fac & "'" & WhereString2 & " Group by dateadd(DAY, 0, DateDiff(Day, 0, Inspection_Finished))  ORDER BY Comp_Date ASC"
                    Command.CommandType = CommandType.Text 'sets the type of the sql
                    Command.Connection = Connection 'sets the connection of our sql command to MyDB
                    Command.CommandText = SQL 'sets the statement that executes at the data source to our string
                    Connection.Open() 'opens the connction
                    DR = Command.ExecuteReader 'sends the command text to the connection and builds tthe SqlDataReader
                    While DR.Read() 'Check whether the SqlDataReader has 1 or more rows

                        segment = New List(Of String)()
                        segment.Add(DR("Comp_Date"))
                        segment.Add(DR("TOTAL"))
                        retval.Add(segment)


                    End While
                    Connection.Close() 'closes the connection
                    DR.Close() 'closes the reader
                    Return retval

                ElseIf gt = "DHU" And tp = "Past 30 Days" Then
                    Dim segment As New List(Of String)()
                    SQL = "Select ISNULL((CAST(SUM(MajorsCount+MinorsCount+RepairsCount+ScrapCount) AS Decimal(10,2))/CAST(SUM(SampleSize) AS Decimal(10,2)))*100, 0) AS TOTAL , dateadd(DAY,0, datediff(day,0, Inspection_Finished)) AS Comp_Date from Inspection.dbo.Locations inner join dbo.InspectionJobSummaryYearly On dbo.InspectionJobSummaryYearly.CID=NCID WHERE Name ='" & fac & "'" & WhereString2 & " And Inspection_Finished >= DATEADD(month,-1,GETDATE()) Group by dateadd(DAY, 0, DateDiff(Day, 0, Inspection_Finished))  ORDER BY Comp_Date ASC"
                    Command.CommandType = CommandType.Text 'sets the type of the sql
                    Command.Connection = Connection 'sets the connection of our sql command to MyDB
                    Command.CommandText = SQL 'sets the statement that executes at the data source to our string
                    Connection.Open() 'opens the connction
                    DR = Command.ExecuteReader 'sends the command text to the connection and builds tthe SqlDataReader
                    While DR.Read() 'Check whether the SqlDataReader has 1 or more rows

                        segment = New List(Of String)()
                        segment.Add(DR("Comp_Date"))
                        segment.Add(DR("TOTAL"))
                        retval.Add(segment)


                    End While
                    Connection.Close() 'closes the connection
                    DR.Close() 'closes the reader
                    Return retval
                ElseIf gt = "DHU" And tp = "Custom" Then
                    Dim segment As New List(Of String)()
                    SQL = "Select ISNULL((CAST(SUM(MajorsCount+MinorsCount+RepairsCount+ScrapCount) AS Decimal(10,2))/CAST(SUM(SampleSize) AS Decimal(10,2)))*100, 0) AS TOTAL, dateadd(DAY,0, datediff(day,0, Inspection_Finished)) AS Comp_Date from Inspection.dbo.Locations inner join dbo.InspectionJobSummaryYearly On dbo.InspectionJobSummaryYearly.CID=NCID WHERE Name ='" & fac & "'" & WhereString & " Group by dateadd(DAY, 0, DateDiff(Day, 0, Inspection_Finished))  ORDER BY Comp_Date ASC"
                    Command.CommandType = CommandType.Text 'sets the type of the sql
                    Command.Connection = Connection 'sets the connection of our sql command to MyDB
                    Command.CommandText = SQL 'sets the statement that executes at the data source to our string
                    Connection.Open() 'opens the connction
                    DR = Command.ExecuteReader 'sends the command text to the connection and builds tthe SqlDataReader
                    While DR.Read() 'Check whether the SqlDataReader has 1 or more rows

                        segment = New List(Of String)()
                        segment.Add(DR("Comp_Date"))
                        segment.Add(DR("TOTAL"))
                        retval.Add(segment)


                    End While
                    Connection.Close() 'closes the connection
                    DR.Close() 'closes the reader
                    Return retval

                ElseIf gt = "Reject Rate" And tp = "Past Year" Then
                    Dim segment As New List(Of String)()
                    SQL = "Select ISNULL((CAST(SUM(MajorsCount+RepairsCount+ScrapCount) AS Decimal(10,2))/CAST(SUM(SampleSize) AS Decimal(10,2)))*100, 0) AS TOTAL, dateadd(DAY,0, datediff(day,0, Inspection_Finished)) AS Comp_Date from Inspection.dbo.Locations inner join dbo.InspectionJobSummaryYearly On dbo.InspectionJobSummaryYearly.CID=NCID WHERE Name ='" & fac & "'" & WhereString2 & " Group by dateadd(DAY, 0, DateDiff(Day, 0, Inspection_Finished))  ORDER BY Comp_Date ASC"
                    Command.CommandType = CommandType.Text 'sets the type of the sql
                    Command.Connection = Connection 'sets the connection of our sql command to MyDB
                    Command.CommandText = SQL 'sets the statement that executes at the data source to our string
                    Connection.Open() 'opens the connction
                    DR = Command.ExecuteReader 'sends the command text to the connection and builds tthe SqlDataReader
                    While DR.Read() 'Check whether the SqlDataReader has 1 or more rows

                        segment = New List(Of String)()
                        segment.Add(DR("Comp_Date"))
                        segment.Add(DR("TOTAL"))
                        retval.Add(segment)


                    End While
                    Connection.Close() 'closes the connection
                    DR.Close() 'closes the reader
                    Return retval

                ElseIf gt = "Reject Rate" And tp = "Past 30 Days" Then
                    Dim segment As New List(Of String)()
                    SQL = "Select ISNULL((CAST(SUM(MajorsCount+RepairsCount+ScrapCount) AS Decimal(10,2))/CAST(SUM(SampleSize) AS Decimal(10,2)))*100, 0) AS TOTAL , dateadd(DAY,0, datediff(day,0, Inspection_Finished)) AS Comp_Date from Inspection.dbo.Locations inner join dbo.InspectionJobSummaryYearly On dbo.InspectionJobSummaryYearly.CID=NCID WHERE Name ='" & fac & "'" & WhereString2 & " And Inspection_Finished >= DATEADD(month,-1,GETDATE()) Group by dateadd(DAY, 0, DateDiff(Day, 0, Inspection_Finished))  ORDER BY Comp_Date ASC"
                    Command.CommandType = CommandType.Text 'sets the type of the sql
                    Command.Connection = Connection 'sets the connection of our sql command to MyDB
                    Command.CommandText = SQL 'sets the statement that executes at the data source to our string
                    Connection.Open() 'opens the connction
                    DR = Command.ExecuteReader 'sends the command text to the connection and builds tthe SqlDataReader
                    While DR.Read() 'Check whether the SqlDataReader has 1 or more rows

                        segment = New List(Of String)()
                        segment.Add(DR("Comp_Date"))
                        segment.Add(DR("TOTAL"))
                        retval.Add(segment)


                    End While
                    Connection.Close() 'closes the connection
                    DR.Close() 'closes the reader
                    Return retval
                ElseIf gt = "Reject Rate" And tp = "Custom" Then
                    Dim segment As New List(Of String)()
                    SQL = "Select ISNULL((CAST(SUM(MajorsCount+RepairsCount+ScrapCount) AS Decimal(10,2))/CAST(SUM(SampleSize) AS Decimal(10,2)))*100, 0) AS TOTAL, dateadd(DAY,0, datediff(day,0, Inspection_Finished)) AS Comp_Date from Inspection.dbo.Locations inner join dbo.InspectionJobSummaryYearly On dbo.InspectionJobSummaryYearly.CID=NCID WHERE Name ='" & fac & "'" & WhereString & " Group by dateadd(DAY, 0, DateDiff(Day, 0, Inspection_Finished))  ORDER BY Comp_Date ASC"
                    Command.CommandType = CommandType.Text 'sets the type of the sql
                    Command.Connection = Connection 'sets the connection of our sql command to MyDB
                    Command.CommandText = SQL 'sets the statement that executes at the data source to our string
                    Connection.Open() 'opens the connction
                    DR = Command.ExecuteReader 'sends the command text to the connection and builds tthe SqlDataReader
                    While DR.Read() 'Check whether the SqlDataReader has 1 or more rows

                        segment = New List(Of String)()
                        segment.Add(DR("Comp_Date"))
                        segment.Add(DR("TOTAL"))
                        retval.Add(segment)


                    End While
                    Connection.Close() 'closes the connection
                    DR.Close() 'closes the reader
                    Return retval

                ElseIf gt = "Lot Acceptance" And tp = "Past Year" Then
                    Dim segment As New List(Of String)()
                    SQL = "Select ISNULL((CAST(s3.TOTAL AS Decimal(10,2)))/(CAST( s3.TOTAL2 AS Decimal(10,2)))*100, 0) AS TOTAL3,dateadd(DAY,0, datediff(day,0, s3.Comp_Date)) AS Comp_Date3  FROM (Select s1.TOTAL, s2.TOTAL2, s2.Comp_Date From ((Select COUNT(InspectionJobSummaryYearly.id) AS TOTAL, dateadd(DAY,0, datediff(day,0, Inspection_Finished)) AS Comp_Date from Inspection.dbo.Locations inner join dbo.InspectionJobSummaryYearly On dbo.InspectionJobSummaryYearly.CID=NCID WHERE Name ='" & fac & "'" & WhereString2 & " AND Technical_PassFail=1 Group by dateadd(DAY, 0, DateDiff(Day, 0, Inspection_Finished))) AS s1 RIGHT JOIN (Select COUNT(InspectionJobSummaryYearly.id) AS TOTAL2, dateadd(DAY,0, datediff(day,0, Inspection_Finished)) AS Comp_Date from Inspection.dbo.Locations inner join dbo.InspectionJobSummaryYearly On dbo.InspectionJobSummaryYearly.CID=NCID WHERE Name ='" & fac & "'" & WhereString2 & " Group by dateadd(DAY, 0, DateDiff(Day, 0, Inspection_Finished))) AS s2 ON s1.Comp_Date=s2.Comp_Date)) AS s3 ORDER by Comp_Date3 ASC"
                    Command.CommandType = CommandType.Text 'sets the type of the sql
                    Command.Connection = Connection 'sets the connection of our sql command to MyDB
                    Command.CommandText = SQL 'sets the statement that executes at the data source to our string
                    Connection.Open() 'opens the connction
                    DR = Command.ExecuteReader 'sends the command text to the connection and builds tthe SqlDataReader
                    While DR.Read() 'Check whether the SqlDataReader has 1 or more rows

                        segment = New List(Of String)()
                        segment.Add(DR("Comp_Date3"))
                        segment.Add(DR("TOTAL3"))
                        retval.Add(segment)


                    End While
                    Connection.Close() 'closes the connection
                    DR.Close() 'closes the reader
                    Return retval

                ElseIf gt = "Lot Acceptance" And tp = "Past 30 Days" Then
                    Dim segment As New List(Of String)()
                    SQL = "Select ISNULL((CAST(s3.TOTAL AS Decimal(10,2)))/(CAST( s3.TOTAL2 AS Decimal(10,2)))*100, 0) AS TOTAL3,dateadd(DAY,0, datediff(day,0, s3.Comp_Date)) AS Comp_Date3  FROM (Select s1.TOTAL, s2.TOTAL2, s2.Comp_Date From ((Select COUNT(InspectionJobSummaryYearly.id) AS TOTAL, dateadd(DAY,0, datediff(day,0, Inspection_Finished)) AS Comp_Date from Inspection.dbo.Locations inner join dbo.InspectionJobSummaryYearly On dbo.InspectionJobSummaryYearly.CID=NCID WHERE Name ='" & fac & "'" & WhereString2 & " And Inspection_Finished >= DATEADD(month,-1,GETDATE()) AND Technical_PassFail=1 Group by dateadd(DAY, 0, DateDiff(Day, 0, Inspection_Finished))) AS s1 RIGHT JOIN (Select COUNT(InspectionJobSummaryYearly.id) AS TOTAL2, dateadd(DAY,0, datediff(day,0, Inspection_Finished)) AS Comp_Date from Inspection.dbo.Locations inner join dbo.InspectionJobSummaryYearly On dbo.InspectionJobSummaryYearly.CID=NCID WHERE Name ='" & fac & "'" & WhereString2 & " And Inspection_Finished >= DATEADD(month,-1,GETDATE()) Group by dateadd(DAY, 0, DateDiff(Day, 0, Inspection_Finished))) AS s2 ON s1.Comp_Date=s2.Comp_Date)) AS s3 ORDER by Comp_Date3 ASC"
                    Command.CommandType = CommandType.Text 'sets the type of the sql
                    Command.Connection = Connection 'sets the connection of our sql command to MyDB
                    Command.CommandText = SQL 'sets the statement that executes at the data source to our string
                    Connection.Open() 'opens the connction
                    DR = Command.ExecuteReader 'sends the command text to the connection and builds tthe SqlDataReader
                    While DR.Read() 'Check whether the SqlDataReader has 1 or more rows

                        segment = New List(Of String)()
                        segment.Add(DR("Comp_Date3"))
                        segment.Add(DR("TOTAL3"))
                        retval.Add(segment)


                    End While
                    Connection.Close() 'closes the connection
                    DR.Close() 'closes the reader
                    Return retval
                ElseIf gt = "Lot Acceptance" And tp = "Custom" Then
                    Dim segment As New List(Of String)()
                    SQL = "Select ISNULL((CAST(s3.TOTAL AS Decimal(10,2)))/(CAST( s3.TOTAL2 AS Decimal(10,2)))*100, 0) AS TOTAL3,dateadd(DAY,0, datediff(day,0, s3.Comp_Date)) AS Comp_Date3  FROM (Select s1.TOTAL, s2.TOTAL2, s2.Comp_Date From ((Select COUNT(InspectionJobSummaryYearly.id) AS TOTAL, dateadd(DAY,0, datediff(day,0, Inspection_Finished)) AS Comp_Date from Inspection.dbo.Locations inner join dbo.InspectionJobSummaryYearly On dbo.InspectionJobSummaryYearly.CID=NCID WHERE Name ='" & fac & "'" & WhereString & " AND Technical_PassFail=1 Group by dateadd(DAY, 0, DateDiff(Day, 0, Inspection_Finished))) AS s1 RIGHT JOIN (Select COUNT(InspectionJobSummaryYearly.id) AS TOTAL2, dateadd(DAY,0, datediff(day,0, Inspection_Finished)) AS Comp_Date from Inspection.dbo.Locations inner join dbo.InspectionJobSummaryYearly On dbo.InspectionJobSummaryYearly.CID=NCID WHERE Name ='" & fac & "'" & WhereString & " Group by dateadd(DAY, 0, DateDiff(Day, 0, Inspection_Finished))) AS s2 ON s1.Comp_Date=s2.Comp_Date)) AS s3 ORDER by Comp_Date3 ASC"
                    Command.CommandType = CommandType.Text 'sets the type of the sql
                    Command.Connection = Connection 'sets the connection of our sql command to MyDB
                    Command.CommandText = SQL 'sets the statement that executes at the data source to our string
                    Connection.Open() 'opens the connction
                    DR = Command.ExecuteReader 'sends the command text to the connection and builds tthe SqlDataReader
                    While DR.Read() 'Check whether the SqlDataReader has 1 or more rows

                        segment = New List(Of String)()
                        segment.Add(DR("Comp_Date3"))
                        segment.Add(DR("TOTAL3"))
                        retval.Add(segment)


                    End While
                    Connection.Close() 'closes the connection
                    DR.Close() 'closes the reader
                    Return retval


                End If
            ElseIf fac = "Global" Then
                Dim WhereString2 As String = "WHERE (CID = 114 OR CID = 115 OR CID = 590 OR CID = 1001 OR CID = 627)"
                If AuditType = "ALL" Then
                    WhereString2 = WhereString2 & " AND InspectionType != 'ROLL'"
                ElseIf AuditType = "FINAL AUDIT" Then
                    WhereString2 = WhereString2 & " AND InspectionType = 'EOL'"
                ElseIf AuditType = "IN LINE" Then
                    WhereString2 = WhereString2 & " AND InspectionType = 'IL'"
                Else
                    WhereString2 = WhereString2 & " AND InspectionType = '" & AuditType & "'"
                End If
                If DataNo <> "ALL" Then
                    WhereString2 = WhereString2 & " AND DataNo = '" & DataNo & "'"
                End If
                If WorkOrder <> "ALL" Then
                    WhereString2 = WhereString2 & " AND JobNumber = '" & WorkOrder & "'"
                End If
                Dim WhereString As String = "WHERE (CID = 114 OR CID = 115 OR CID = 590 OR CID = 1001 OR CID = 627) AND Inspection_Finished BETWEEN '" & Fromdate & "' AND '" & Todate & "'"
                If AuditType = "ALL" Then
                    WhereString = WhereString & " AND InspectionType != 'ROLL'"
                ElseIf AuditType = "FINAL AUDIT" Then
                    WhereString = WhereString & " AND InspectionType = 'EOL'"
                ElseIf AuditType = "IN LINE" Then
                    WhereString = WhereString & " AND InspectionType = 'IL'"
                Else
                    WhereString = WhereString & " AND InspectionType = '" & AuditType & "'"
                End If
                If DataNo <> "ALL" Then
                    WhereString = WhereString & " AND DataNo = '" & DataNo & "'"
                End If
                If WorkOrder <> "ALL" Then
                    WhereString = WhereString & " AND JobNumber = '" & WorkOrder & "'"
                End If
                Dim SQL As String = ""

                If gt = "No. of Defects" And tp = "Past Year" Then
                    Dim segment As New List(Of String)()
                    SQL = "Select ISNULL(SUM(MajorsCount+MinorsCount+RepairsCount+ScrapCount), 0) AS TOTAL, dateadd(DAY,0, datediff(day,0, Inspection_Finished)) AS Comp_Date from dbo.InspectionJobSummaryYearly " & WhereString2 & "Group by dateadd(DAY, 0, DateDiff(Day, 0, Inspection_Finished))  ORDER BY Comp_Date ASC"
                    Command.CommandType = CommandType.Text 'sets the type of the sql
                    Command.Connection = Connection 'sets the connection of our sql command to MyDB
                    Command.CommandText = SQL 'sets the statement that executes at the data source to our string
                    Connection.Open() 'opens the connction
                    DR = Command.ExecuteReader 'sends the command text to the connection and builds tthe SqlDataReader
                    While DR.Read() 'Check whether the SqlDataReader has 1 or more rows

                        segment = New List(Of String)()
                        segment.Add(DR("Comp_Date"))
                        segment.Add(DR("TOTAL"))
                        retval.Add(segment)


                    End While
                    Connection.Close() 'closes the connection
                    DR.Close() 'closes the reader
                    Return retval
                ElseIf gt = "No. of Defects" And tp = "Yesterday" Then
                    Dim segment As New List(Of String)()
                    SQL = "Select ISNULL(SUM(MajorsCount+MinorsCount+RepairsCount+ScrapCount), 0) AS TOTAL, dateadd(DAY,0, datediff(day,0, Inspection_Finished)) AS Comp_Date from dbo.InspectionJobSummaryYearly " & WhereString2 & " And Inspection_Finished >= dateadd(day,datediff(day,1,GETDATE()),0)AND Inspection_Finished < dateadd(day,datediff(day,0,GETDATE()),0) Group by dateadd(DAY, 0, DateDiff(Day, 0, Inspection_Finished))  ORDER BY Comp_Date ASC"
                    Command.CommandType = CommandType.Text 'sets the type of the sql
                    Command.Connection = Connection 'sets the connection of our sql command to MyDB
                    Command.CommandText = SQL 'sets the statement that executes at the data source to our string
                    Connection.Open() 'opens the connction
                    DR = Command.ExecuteReader 'sends the command text to the connection and builds tthe SqlDataReader
                    While DR.Read() 'Check whether the SqlDataReader has 1 or more rows

                        segment = New List(Of String)()
                        segment.Add(DR("Comp_Date"))
                        segment.Add(DR("TOTAL"))
                        retval.Add(segment)


                    End While
                    Connection.Close() 'closes the connection
                    DR.Close() 'closes the reader
                    Return retval
                ElseIf gt = "No. of Rejects" And tp = "Yesterday" Then
                    Dim segment As New List(Of String)()
                    SQL = "Select ISNULL(SUM(MajorsCount+RepairsCount+ScrapCount), 0) AS TOTAL, dateadd(DAY,0, datediff(day,0, Inspection_Finished)) AS Comp_Date from dbo.InspectionJobSummaryYearly " & WhereString2 & " And Inspection_Finished >= dateadd(day,datediff(day,1,GETDATE()),0) AND Inspection_Finished < dateadd(day,datediff(day,0,GETDATE()),0) Group by dateadd(DAY, 0, DateDiff(Day, 0, Inspection_Finished))  ORDER BY Comp_Date ASC"
                    Command.CommandType = CommandType.Text 'sets the type of the sql
                    Command.Connection = Connection 'sets the connection of our sql command to MyDB
                    Command.CommandText = SQL 'sets the statement that executes at the data source to our string
                    Connection.Open() 'opens the connction
                    DR = Command.ExecuteReader 'sends the command text to the connection and builds tthe SqlDataReader
                    While DR.Read() 'Check whether the SqlDataReader has 1 or more rows

                        segment = New List(Of String)()
                        segment.Add(DR("Comp_Date"))
                        segment.Add(DR("TOTAL"))
                        retval.Add(segment)


                    End While
                    Connection.Close() 'closes the connection
                    DR.Close() 'closes the reader
                    Return retval
                ElseIf gt = "No. of Inspections" And tp = "Yesterday" Then
                    Dim segment As New List(Of String)()
                    SQL = "Select COUNT(InspectionJobSummaryYearly.id) AS TOTAL, dateadd(DAY,0, datediff(day,0, Inspection_Finished)) AS Comp_Date from dbo.InspectionJobSummaryYearly " & WhereString2 & " And Inspection_Finished >= dateadd(day,datediff(day,1,GETDATE()),0) AND Inspection_Finished < dateadd(day,datediff(day,0,GETDATE()),0) Group by dateadd(DAY, 0, DateDiff(Day, 0, Inspection_Finished))  ORDER BY Comp_Date ASC"
                    Command.CommandType = CommandType.Text 'sets the type of the sql
                    Command.Connection = Connection 'sets the connection of our sql command to MyDB
                    Command.CommandText = SQL 'sets the statement that executes at the data source to our string
                    Connection.Open() 'opens the connction
                    DR = Command.ExecuteReader 'sends the command text to the connection and builds tthe SqlDataReader
                    While DR.Read() 'Check whether the SqlDataReader has 1 or more rows

                        segment = New List(Of String)()
                        segment.Add(DR("Comp_Date"))
                        segment.Add(DR("TOTAL"))
                        retval.Add(segment)


                    End While
                    Connection.Close() 'closes the connection
                    DR.Close() 'closes the reader
                    Return retval
                ElseIf gt = "No. of Rejected Lots" And tp = "Yesterday" Then
                    Dim segment As New List(Of String)()
                    SQL = "Select COUNT(InspectionJobSummaryYearly.id) AS TOTAL, dateadd(DAY,0, datediff(day,0, Inspection_Finished)) AS Comp_Date from dbo.InspectionJobSummaryYearly " & WhereString2 & " AND Technical_PassFail=0 And Inspection_Finished >= dateadd(day,datediff(day,1,GETDATE()),0) AND Inspection_Finished < dateadd(day,datediff(day,0,GETDATE()),0) Group by dateadd(DAY, 0, DateDiff(Day, 0, Inspection_Finished))  ORDER BY Comp_Date ASC"
                    Command.CommandType = CommandType.Text 'sets the type of the sql
                    Command.Connection = Connection 'sets the connection of our sql command to MyDB
                    Command.CommandText = SQL 'sets the statement that executes at the data source to our string
                    Connection.Open() 'opens the connction
                    DR = Command.ExecuteReader 'sends the command text to the connection and builds tthe SqlDataReader
                    While DR.Read() 'Check whether the SqlDataReader has 1 or more rows

                        segment = New List(Of String)()
                        segment.Add(DR("Comp_Date"))
                        segment.Add(DR("TOTAL"))
                        retval.Add(segment)


                    End While
                    Connection.Close() 'closes the connection
                    DR.Close() 'closes the reader
                    Return retval
                ElseIf gt = "DHU" And tp = "Yesterday" Then
                    Dim segment As New List(Of String)()
                    SQL = "Select ISNULL((CAST(SUM(MajorsCount+MinorsCount+RepairsCount+ScrapCount) AS Decimal(10,2))/CAST(SUM(SampleSize) AS Decimal(10,2)))*100, 0) AS TOTAL, dateadd(DAY,0, datediff(day,0, Inspection_Finished)) AS Comp_Date from dbo.InspectionJobSummaryYearly " & WhereString2 & " And Inspection_Finished >= dateadd(day,datediff(day,1,GETDATE()),0) AND Inspection_Finished < dateadd(day,datediff(day,0,GETDATE()),0) Group by dateadd(DAY, 0, DateDiff(Day, 0, Inspection_Finished))  ORDER BY Comp_Date ASC"
                    Command.CommandType = CommandType.Text 'sets the type of the sql
                    Command.Connection = Connection 'sets the connection of our sql command to MyDB
                    Command.CommandText = SQL 'sets the statement that executes at the data source to our string
                    Connection.Open() 'opens the connction
                    DR = Command.ExecuteReader 'sends the command text to the connection and builds tthe SqlDataReader
                    While DR.Read() 'Check whether the SqlDataReader has 1 or more rows

                        segment = New List(Of String)()
                        segment.Add(DR("Comp_Date"))
                        segment.Add(DR("TOTAL"))
                        retval.Add(segment)


                    End While
                    Connection.Close() 'closes the connection
                    DR.Close() 'closes the reader
                    Return retval
                ElseIf gt = "Reject Rate" And tp = "Yesterday" Then
                    Dim segment As New List(Of String)()
                    SQL = "Select ISNULL((CAST(SUM(MajorsCount+RepairsCount+ScrapCount) AS Decimal(10,2))/CAST(SUM(SampleSize) AS Decimal(10,2)))*100, 0) AS TOTAL, dateadd(DAY,0, datediff(day,0, Inspection_Finished)) AS Comp_Date from dbo.InspectionJobSummaryYearly " & WhereString2 & " And Inspection_Finished >= dateadd(day,datediff(day,1,GETDATE()),0) AND Inspection_Finished < dateadd(day,datediff(day,0,GETDATE()),0) Group by dateadd(DAY, 0, DateDiff(Day, 0, Inspection_Finished))  ORDER BY Comp_Date ASC"
                    Command.CommandType = CommandType.Text 'sets the type of the sql
                    Command.Connection = Connection 'sets the connection of our sql command to MyDB
                    Command.CommandText = SQL 'sets the statement that executes at the data source to our string
                    Connection.Open() 'opens the connction
                    DR = Command.ExecuteReader 'sends the command text to the connection and builds tthe SqlDataReader
                    While DR.Read() 'Check whether the SqlDataReader has 1 or more rows

                        segment = New List(Of String)()
                        segment.Add(DR("Comp_Date"))
                        segment.Add(DR("TOTAL"))
                        retval.Add(segment)


                    End While
                    Connection.Close() 'closes the connection
                    DR.Close() 'closes the reader
                    Return retval
                ElseIf gt = "Lot Acceptance" And tp = "Yesterday" Then
                    Dim segment As New List(Of String)()
                    SQL = "Select ISNULL((CAST(s3.TOTAL AS Decimal(10,2)))/(CAST( s3.TOTAL2 AS Decimal(10,2)))*100, 0) AS TOTAL3,dateadd(DAY,0, datediff(day,0, s3.Comp_Date)) AS Comp_Date3  FROM (Select s1.TOTAL, s2.TOTAL2, s2.Comp_Date From ((Select COUNT(InspectionJobSummaryYearly.id) AS TOTAL, dateadd(DAY,0, datediff(day,0, Inspection_Finished)) AS Comp_Date from dbo.InspectionJobSummaryYearly " & WhereString2 & " And Inspection_Finished >= dateadd(day,datediff(day,1,GETDATE()),0) AND Inspection_Finished < dateadd(day,datediff(day,0,GETDATE()),0) AND Technical_PassFail=1 Group by dateadd(DAY, 0, DateDiff(Day, 0, Inspection_Finished))) AS s1 RIGHT JOIN (Select COUNT(InspectionJobSummaryYearly.id) AS TOTAL2, dateadd(DAY,0, datediff(day,0, Inspection_Finished)) AS Comp_Date from dbo.InspectionJobSummaryYearly " & WhereString2 & " And Inspection_Finished >= dateadd(day,datediff(day,1,GETDATE()),0) AND Inspection_Finished < dateadd(day,datediff(day,0,GETDATE()),0) Group by dateadd(DAY, 0, DateDiff(Day, 0, Inspection_Finished))) AS s2 ON s1.Comp_Date=s2.Comp_Date)) AS s3 ORDER by Comp_Date3 ASC"
                    Command.CommandType = CommandType.Text 'sets the type of the sql
                    Command.Connection = Connection 'sets the connection of our sql command to MyDB
                    Command.CommandText = SQL 'sets the statement that executes at the data source to our string
                    Connection.Open() 'opens the connction
                    DR = Command.ExecuteReader 'sends the command text to the connection and builds tthe SqlDataReader
                    While DR.Read() 'Check whether the SqlDataReader has 1 or more rows

                        segment = New List(Of String)()
                        segment.Add(DR("Comp_Date3"))
                        segment.Add(DR("TOTAL3"))
                        retval.Add(segment)


                    End While
                    Connection.Close() 'closes the connection
                    DR.Close() 'closes the reader
                    Return retval

                ElseIf gt = "No. of Defects" And tp = "Past 30 Days" Then
                    Dim segment As New List(Of String)()
                    SQL = "Select ISNULL(SUM(MajorsCount+MinorsCount+RepairsCount+ScrapCount), 0) AS TOTAL, dateadd(DAY,0, datediff(day,0, Inspection_Finished)) AS Comp_Date from dbo.InspectionJobSummaryYearly " & WhereString2 & " And Inspection_Finished >= DATEADD(month,-1,GETDATE()) Group by dateadd(DAY, 0, DateDiff(Day, 0, Inspection_Finished))  ORDER BY Comp_Date ASC"
                    Command.CommandType = CommandType.Text 'sets the type of the sql
                    Command.Connection = Connection 'sets the connection of our sql command to MyDB
                    Command.CommandText = SQL 'sets the statement that executes at the data source to our string
                    Connection.Open() 'opens the connction
                    DR = Command.ExecuteReader 'sends the command text to the connection and builds tthe SqlDataReader
                    While DR.Read() 'Check whether the SqlDataReader has 1 or more rows

                        segment = New List(Of String)()
                        segment.Add(DR("Comp_Date"))
                        segment.Add(DR("TOTAL"))
                        retval.Add(segment)


                    End While
                    Connection.Close() 'closes the connection
                    DR.Close() 'closes the reader
                    Return retval

                ElseIf gt = "No. of Defects" And tp = "Custom" Then
                    Dim segment As New List(Of String)()
                    SQL = "Select ISNULL(SUM(MajorsCount+MinorsCount+RepairsCount+ScrapCount), 0) AS TOTAL, dateadd(DAY,0, datediff(day,0, Inspection_Finished)) AS Comp_Date from dbo.InspectionJobSummaryYearly " & WhereString & " Group by dateadd(DAY, 0, DateDiff(Day, 0, Inspection_Finished))  ORDER BY Comp_Date ASC"
                    Command.CommandType = CommandType.Text 'sets the type of the sql
                    Command.Connection = Connection 'sets the connection of our sql command to MyDB
                    Command.CommandText = SQL 'sets the statement that executes at the data source to our string
                    Connection.Open() 'opens the connction
                    DR = Command.ExecuteReader 'sends the command text to the connection and builds tthe SqlDataReader
                    While DR.Read() 'Check whether the SqlDataReader has 1 or more rows

                        segment = New List(Of String)()
                        segment.Add(DR("Comp_Date"))
                        segment.Add(DR("TOTAL"))
                        retval.Add(segment)


                    End While
                    Connection.Close() 'closes the connection
                    DR.Close() 'closes the reader
                    Return retval
                ElseIf gt = "No. of Rejects" And tp = "Past Year" Then
                    Dim segment As New List(Of String)()
                    SQL = "Select ISNULL(SUM(MajorsCount+RepairsCount+ScrapCount), 0) AS TOTAL, dateadd(DAY,0, datediff(day,0, Inspection_Finished)) AS Comp_Date from dbo.InspectionJobSummaryYearly " & WhereString2 & " Group by dateadd(DAY, 0, DateDiff(Day, 0, Inspection_Finished))  ORDER BY Comp_Date ASC"
                    Command.CommandType = CommandType.Text 'sets the type of the sql
                    Command.Connection = Connection 'sets the connection of our sql command to MyDB
                    Command.CommandText = SQL 'sets the statement that executes at the data source to our string
                    Connection.Open() 'opens the connction
                    DR = Command.ExecuteReader 'sends the command text to the connection and builds tthe SqlDataReader
                    While DR.Read() 'Check whether the SqlDataReader has 1 or more rows

                        segment = New List(Of String)()
                        segment.Add(DR("Comp_Date"))
                        segment.Add(DR("TOTAL"))
                        retval.Add(segment)


                    End While
                    Connection.Close() 'closes the connection
                    DR.Close() 'closes the reader
                    Return retval
                ElseIf gt = "No. of Rejects" And tp = "Past 30 Days" Then
                    Dim segment As New List(Of String)()
                    SQL = "Select ISNULL(SUM(MajorsCount+RepairsCount+ScrapCount), 0) AS TOTAL, dateadd(DAY,0, datediff(day,0, Inspection_Finished)) AS Comp_Date from dbo.InspectionJobSummaryYearly " & WhereString2 & " And Inspection_Finished >= DATEADD(month,-1,GETDATE()) Group by dateadd(DAY, 0, DateDiff(Day, 0, Inspection_Finished))  ORDER BY Comp_Date ASC"
                    Command.CommandType = CommandType.Text 'sets the type of the sql
                    Command.Connection = Connection 'sets the connection of our sql command to MyDB
                    Command.CommandText = SQL 'sets the statement that executes at the data source to our string
                    Connection.Open() 'opens the connction
                    DR = Command.ExecuteReader 'sends the command text to the connection and builds tthe SqlDataReader
                    While DR.Read() 'Check whether the SqlDataReader has 1 or more rows

                        segment = New List(Of String)()
                        segment.Add(DR("Comp_Date"))
                        segment.Add(DR("TOTAL"))
                        retval.Add(segment)


                    End While
                    Connection.Close() 'closes the connection
                    DR.Close() 'closes the reader
                    Return retval
                ElseIf gt = "No. of Rejects" And tp = "Custom" Then
                    Dim segment As New List(Of String)()
                    SQL = "Select ISNULL(SUM(MajorsCount+RepairsCount+ScrapCount), 0) AS TOTAL, dateadd(DAY,0, datediff(day,0, Inspection_Finished)) AS Comp_Date from dbo.InspectionJobSummaryYearly " & WhereString & "Group by dateadd(DAY, 0, DateDiff(Day, 0, Inspection_Finished))  ORDER BY Comp_Date ASC"
                    Command.CommandType = CommandType.Text 'sets the type of the sql
                    Command.Connection = Connection 'sets the connection of our sql command to MyDB
                    Command.CommandText = SQL 'sets the statement that executes at the data source to our string
                    Connection.Open() 'opens the connction
                    DR = Command.ExecuteReader 'sends the command text to the connection and builds tthe SqlDataReader
                    While DR.Read() 'Check whether the SqlDataReader has 1 or more rows

                        segment = New List(Of String)()
                        segment.Add(DR("Comp_Date"))
                        segment.Add(DR("TOTAL"))
                        retval.Add(segment)


                    End While
                    Connection.Close() 'closes the connection
                    DR.Close() 'closes the reader
                    Return retval
                ElseIf gt = "No. of Inspections" And tp = "Past Year" Then
                    Dim segment As New List(Of String)()
                    SQL = "Select COUNT(InspectionJobSummaryYearly.id) AS TOTAL, dateadd(DAY,0, datediff(day,0, Inspection_Finished)) AS Comp_Date from dbo.InspectionJobSummaryYearly " & WhereString2 & " Group by dateadd(DAY, 0, DateDiff(Day, 0, Inspection_Finished))  ORDER BY Comp_Date ASC"
                    Command.CommandType = CommandType.Text 'sets the type of the sql
                    Command.Connection = Connection 'sets the connection of our sql command to MyDB
                    Command.CommandText = SQL 'sets the statement that executes at the data source to our string
                    Connection.Open() 'opens the connction
                    DR = Command.ExecuteReader 'sends the command text to the connection and builds tthe SqlDataReader
                    While DR.Read() 'Check whether the SqlDataReader has 1 or more rows

                        segment = New List(Of String)()
                        segment.Add(DR("Comp_Date"))
                        segment.Add(DR("TOTAL"))
                        retval.Add(segment)


                    End While
                    Connection.Close() 'closes the connection
                    DR.Close() 'closes the reader
                    Return retval
                ElseIf gt = "No. of Inspections" And tp = "Past 30 Days" Then
                    Dim segment As New List(Of String)()
                    SQL = "Select COUNT(InspectionJobSummaryYearly.id) AS TOTAL, dateadd(DAY,0, datediff(day,0, Inspection_Finished)) AS Comp_Date from dbo.InspectionJobSummaryYearly " & WhereString2 & " And Inspection_Finished >= DATEADD(month,-1,GETDATE()) Group by dateadd(DAY, 0, DateDiff(Day, 0, Inspection_Finished))  ORDER BY Comp_Date ASC"
                    Command.CommandType = CommandType.Text 'sets the type of the sql
                    Command.Connection = Connection 'sets the connection of our sql command to MyDB
                    Command.CommandText = SQL 'sets the statement that executes at the data source to our string
                    Connection.Open() 'opens the connction
                    DR = Command.ExecuteReader 'sends the command text to the connection and builds tthe SqlDataReader
                    While DR.Read() 'Check whether the SqlDataReader has 1 or more rows

                        segment = New List(Of String)()
                        segment.Add(DR("Comp_Date"))
                        segment.Add(DR("TOTAL"))
                        retval.Add(segment)


                    End While
                    Connection.Close() 'closes the connection
                    DR.Close() 'closes the reader
                    Return retval
                ElseIf gt = "No. of Inspections" And tp = "Custom" Then
                    Dim segment As New List(Of String)()
                    SQL = "Select COUNT(InspectionJobSummaryYearly.id) AS TOTAL, dateadd(DAY,0, datediff(day,0, Inspection_Finished)) AS Comp_Date from dbo.InspectionJobSummaryYearly " & WhereString & " Group by dateadd(DAY, 0, DateDiff(Day, 0, Inspection_Finished))  ORDER BY Comp_Date ASC"
                    Command.CommandType = CommandType.Text 'sets the type of the sql
                    Command.Connection = Connection 'sets the connection of our sql command to MyDB
                    Command.CommandText = SQL 'sets the statement that executes at the data source to our string
                    Connection.Open() 'opens the connction
                    DR = Command.ExecuteReader 'sends the command text to the connection and builds tthe SqlDataReader
                    While DR.Read() 'Check whether the SqlDataReader has 1 or more rows

                        segment = New List(Of String)()
                        segment.Add(DR("Comp_Date"))
                        segment.Add(DR("TOTAL"))
                        retval.Add(segment)


                    End While
                    Connection.Close() 'closes the connection
                    DR.Close() 'closes the reader
                    Return retval
                ElseIf gt = "No. of Rejected Lots" And tp = "Past Year" Then
                    Dim segment As New List(Of String)()
                    SQL = "Select COUNT(InspectionJobSummaryYearly.id) AS TOTAL, dateadd(DAY,0, datediff(day,0, Inspection_Finished)) AS Comp_Date from dbo.InspectionJobSummaryYearly " & WhereString2 & " AND Technical_PassFail=0 Group by dateadd(DAY, 0, DateDiff(Day, 0, Inspection_Finished))  ORDER BY Comp_Date ASC"
                    Command.CommandType = CommandType.Text 'sets the type of the sql
                    Command.Connection = Connection 'sets the connection of our sql command to MyDB
                    Command.CommandText = SQL 'sets the statement that executes at the data source to our string
                    Connection.Open() 'opens the connction
                    DR = Command.ExecuteReader 'sends the command text to the connection and builds tthe SqlDataReader
                    While DR.Read() 'Check whether the SqlDataReader has 1 or more rows

                        segment = New List(Of String)()
                        segment.Add(DR("Comp_Date"))
                        segment.Add(DR("TOTAL"))
                        retval.Add(segment)


                    End While
                    Connection.Close() 'closes the connection
                    DR.Close() 'closes the reader
                    Return retval
                ElseIf gt = "No. of Rejected Lots" And tp = "Past 30 Days" Then
                    Dim segment As New List(Of String)()
                    SQL = "Select COUNT(InspectionJobSummaryYearly.id) AS TOTAL, dateadd(DAY,0, datediff(day,0, Inspection_Finished)) AS Comp_Date from dbo.InspectionJobSummaryYearly " & WhereString2 & " And Inspection_Finished >= DATEADD(month,-1,GETDATE()) AND Technical_PassFail=0 Group by dateadd(DAY, 0, DateDiff(Day, 0, Inspection_Finished))  ORDER BY Comp_Date ASC"
                    Command.CommandType = CommandType.Text 'sets the type of the sql
                    Command.Connection = Connection 'sets the connection of our sql command to MyDB
                    Command.CommandText = SQL 'sets the statement that executes at the data source to our string
                    Connection.Open() 'opens the connction
                    DR = Command.ExecuteReader 'sends the command text to the connection and builds tthe SqlDataReader
                    While DR.Read() 'Check whether the SqlDataReader has 1 or more rows

                        segment = New List(Of String)()
                        segment.Add(DR("Comp_Date"))
                        segment.Add(DR("TOTAL"))
                        retval.Add(segment)


                    End While
                    Connection.Close() 'closes the connection
                    DR.Close() 'closes the reader
                    Return retval
                ElseIf gt = "No. of Rejected Lots" And tp = "Custom" Then
                    Dim segment As New List(Of String)()
                    SQL = "Select COUNT(InspectionJobSummaryYearly.id) AS TOTAL, dateadd(DAY,0, datediff(day,0, Inspection_Finished)) AS Comp_Date from dbo.InspectionJobSummaryYearly " & WhereString & " AND Technical_PassFail=0 Group by dateadd(DAY, 0, DateDiff(Day, 0, Inspection_Finished))  ORDER BY Comp_Date ASC"
                    Command.CommandType = CommandType.Text 'sets the type of the sql
                    Command.Connection = Connection 'sets the connection of our sql command to MyDB
                    Command.CommandText = SQL 'sets the statement that executes at the data source to our string
                    Connection.Open() 'opens the connction
                    DR = Command.ExecuteReader 'sends the command text to the connection and builds tthe SqlDataReader
                    While DR.Read() 'Check whether the SqlDataReader has 1 or more rows

                        segment = New List(Of String)()
                        segment.Add(DR("Comp_Date"))
                        segment.Add(DR("TOTAL"))
                        retval.Add(segment)


                    End While
                    Connection.Close() 'closes the connection
                    DR.Close() 'closes the reader
                    Return retval
                ElseIf gt = "DHU" And tp = "Past Year" Then
                    Dim segment As New List(Of String)()
                    SQL = "Select ISNULL((CAST(SUM(MajorsCount+MinorsCount+RepairsCount+ScrapCount) AS Decimal(10,2))/CAST(SUM(SampleSize) AS Decimal(10,2)))*100, 0) AS TOTAL, dateadd(DAY,0, datediff(day,0, Inspection_Finished)) AS Comp_Date from dbo.InspectionJobSummaryYearly " & WhereString2 & " Group by dateadd(DAY, 0, DateDiff(Day, 0, Inspection_Finished))  ORDER BY Comp_Date ASC"
                    Command.CommandType = CommandType.Text 'sets the type of the sql
                    Command.Connection = Connection 'sets the connection of our sql command to MyDB
                    Command.CommandText = SQL 'sets the statement that executes at the data source to our string
                    Connection.Open() 'opens the connction
                    DR = Command.ExecuteReader 'sends the command text to the connection and builds tthe SqlDataReader
                    While DR.Read() 'Check whether the SqlDataReader has 1 or more rows

                        segment = New List(Of String)()
                        segment.Add(DR("Comp_Date"))
                        segment.Add(DR("TOTAL"))
                        retval.Add(segment)


                    End While
                    Connection.Close() 'closes the connection
                    DR.Close() 'closes the reader
                    Return retval

                ElseIf gt = "DHU" And tp = "Past 30 Days" Then
                    Dim segment As New List(Of String)()
                    SQL = "Select ISNULL((CAST(SUM(MajorsCount+MinorsCount+RepairsCount+ScrapCount) AS Decimal(10,2))/CAST(SUM(SampleSize) AS Decimal(10,2)))*100, 0) AS TOTAL , dateadd(DAY,0, datediff(day,0, Inspection_Finished)) AS Comp_Date from dbo.InspectionJobSummaryYearly " & WhereString2 & " And Inspection_Finished >= DATEADD(month,-1,GETDATE()) Group by dateadd(DAY, 0, DateDiff(Day, 0, Inspection_Finished))  ORDER BY Comp_Date ASC"
                    Command.CommandType = CommandType.Text 'sets the type of the sql
                    Command.Connection = Connection 'sets the connection of our sql command to MyDB
                    Command.CommandText = SQL 'sets the statement that executes at the data source to our string
                    Connection.Open() 'opens the connction
                    DR = Command.ExecuteReader 'sends the command text to the connection and builds tthe SqlDataReader
                    While DR.Read() 'Check whether the SqlDataReader has 1 or more rows

                        segment = New List(Of String)()
                        segment.Add(DR("Comp_Date"))
                        segment.Add(DR("TOTAL"))
                        retval.Add(segment)


                    End While
                    Connection.Close() 'closes the connection
                    DR.Close() 'closes the reader
                    Return retval
                ElseIf gt = "DHU" And tp = "Custom" Then
                    Dim segment As New List(Of String)()
                    SQL = "Select ISNULL((CAST(SUM(MajorsCount+MinorsCount+RepairsCount+ScrapCount) AS Decimal(10,2))/CAST(SUM(SampleSize) AS Decimal(10,2)))*100, 0) AS TOTAL, dateadd(DAY,0, datediff(day,0, Inspection_Finished)) AS Comp_Date from dbo.InspectionJobSummaryYearly " & WhereString & " Group by dateadd(DAY, 0, DateDiff(Day, 0, Inspection_Finished))  ORDER BY Comp_Date ASC"
                    Command.CommandType = CommandType.Text 'sets the type of the sql
                    Command.Connection = Connection 'sets the connection of our sql command to MyDB
                    Command.CommandText = SQL 'sets the statement that executes at the data source to our string
                    Connection.Open() 'opens the connction
                    DR = Command.ExecuteReader 'sends the command text to the connection and builds tthe SqlDataReader
                    While DR.Read() 'Check whether the SqlDataReader has 1 or more rows

                        segment = New List(Of String)()
                        segment.Add(DR("Comp_Date"))
                        segment.Add(DR("TOTAL"))
                        retval.Add(segment)


                    End While
                    Connection.Close() 'closes the connection
                    DR.Close() 'closes the reader
                    Return retval

                ElseIf gt = "Reject Rate" And tp = "Past Year" Then
                    Dim segment As New List(Of String)()
                    SQL = "Select ISNULL((CAST(SUM(MajorsCount+RepairsCount+ScrapCount) AS Decimal(10,2))/CAST(SUM(SampleSize) AS Decimal(10,2)))*100, 0) AS TOTAL, dateadd(DAY,0, datediff(day,0, Inspection_Finished)) AS Comp_Date from dbo.InspectionJobSummaryYearly " & WhereString2 & " Group by dateadd(DAY, 0, DateDiff(Day, 0, Inspection_Finished))  ORDER BY Comp_Date ASC"
                    Command.CommandType = CommandType.Text 'sets the type of the sql
                    Command.Connection = Connection 'sets the connection of our sql command to MyDB
                    Command.CommandText = SQL 'sets the statement that executes at the data source to our string
                    Connection.Open() 'opens the connction
                    DR = Command.ExecuteReader 'sends the command text to the connection and builds tthe SqlDataReader
                    While DR.Read() 'Check whether the SqlDataReader has 1 or more rows

                        segment = New List(Of String)()
                        segment.Add(DR("Comp_Date"))
                        segment.Add(DR("TOTAL"))
                        retval.Add(segment)


                    End While
                    Connection.Close() 'closes the connection
                    DR.Close() 'closes the reader
                    Return retval

                ElseIf gt = "Reject Rate" And tp = "Past 30 Days" Then
                    Dim segment As New List(Of String)()
                    SQL = "Select ISNULL((CAST(SUM(MajorsCount+RepairsCount+ScrapCount) AS Decimal(10,2))/CAST(SUM(SampleSize) AS Decimal(10,2)))*100, 0) AS TOTAL , dateadd(DAY,0, datediff(day,0, Inspection_Finished)) AS Comp_Date from dbo.InspectionJobSummaryYearly " & WhereString2 & " And Inspection_Finished >= DATEADD(month,-1,GETDATE()) Group by dateadd(DAY, 0, DateDiff(Day, 0, Inspection_Finished))  ORDER BY Comp_Date ASC"
                    Command.CommandType = CommandType.Text 'sets the type of the sql
                    Command.Connection = Connection 'sets the connection of our sql command to MyDB
                    Command.CommandText = SQL 'sets the statement that executes at the data source to our string
                    Connection.Open() 'opens the connction
                    DR = Command.ExecuteReader 'sends the command text to the connection and builds tthe SqlDataReader
                    While DR.Read() 'Check whether the SqlDataReader has 1 or more rows

                        segment = New List(Of String)()
                        segment.Add(DR("Comp_Date"))
                        segment.Add(DR("TOTAL"))
                        retval.Add(segment)


                    End While
                    Connection.Close() 'closes the connection
                    DR.Close() 'closes the reader
                    Return retval
                ElseIf gt = "Reject Rate" And tp = "Custom" Then
                    Dim segment As New List(Of String)()
                    SQL = "Select ISNULL((CAST(SUM(MajorsCount+RepairsCount+ScrapCount) AS Decimal(10,2))/CAST(SUM(SampleSize) AS Decimal(10,2)))*100, 0) AS TOTAL, dateadd(DAY,0, datediff(day,0, Inspection_Finished)) AS Comp_Date from dbo.InspectionJobSummaryYearly " & WhereString & " Group by dateadd(DAY, 0, DateDiff(Day, 0, Inspection_Finished))  ORDER BY Comp_Date ASC"
                    Command.CommandType = CommandType.Text 'sets the type of the sql
                    Command.Connection = Connection 'sets the connection of our sql command to MyDB
                    Command.CommandText = SQL 'sets the statement that executes at the data source to our string
                    Connection.Open() 'opens the connction
                    DR = Command.ExecuteReader 'sends the command text to the connection and builds tthe SqlDataReader
                    While DR.Read() 'Check whether the SqlDataReader has 1 or more rows

                        segment = New List(Of String)()
                        segment.Add(DR("Comp_Date"))
                        segment.Add(DR("TOTAL"))
                        retval.Add(segment)


                    End While
                    Connection.Close() 'closes the connection
                    DR.Close() 'closes the reader
                    Return retval

                ElseIf gt = "Lot Acceptance" And tp = "Past Year" Then
                    Dim segment As New List(Of String)()
                    SQL = "Select ISNULL((CAST(s3.TOTAL AS Decimal(10,2)))/(CAST( s3.TOTAL2 AS Decimal(10,2)))*100, 0) AS TOTAL3,dateadd(DAY,0, datediff(day,0, s3.Comp_Date)) AS Comp_Date3  FROM (Select s1.TOTAL, s2.TOTAL2, s2.Comp_Date From ((Select COUNT(InspectionJobSummaryYearly.id) AS TOTAL, dateadd(DAY,0, datediff(day,0, Inspection_Finished)) AS Comp_Date from dbo.InspectionJobSummaryYearly " & WhereString2 & " AND Technical_PassFail=1 Group by dateadd(DAY, 0, DateDiff(Day, 0, Inspection_Finished))) AS s1 RIGHT JOIN (Select COUNT(InspectionJobSummaryYearly.id) AS TOTAL2, dateadd(DAY,0, datediff(day,0, Inspection_Finished)) AS Comp_Date from dbo.InspectionJobSummaryYearly " & WhereString2 & " Group by dateadd(DAY, 0, DateDiff(Day, 0, Inspection_Finished))) AS s2 ON s1.Comp_Date=s2.Comp_Date)) AS s3 ORDER by Comp_Date3 ASC"
                    Command.CommandType = CommandType.Text 'sets the type of the sql
                    Command.Connection = Connection 'sets the connection of our sql command to MyDB
                    Command.CommandText = SQL 'sets the statement that executes at the data source to our string
                    Connection.Open() 'opens the connction
                    DR = Command.ExecuteReader 'sends the command text to the connection and builds tthe SqlDataReader
                    While DR.Read() 'Check whether the SqlDataReader has 1 or more rows

                        segment = New List(Of String)()
                        segment.Add(DR("Comp_Date3"))
                        segment.Add(DR("TOTAL3"))
                        retval.Add(segment)


                    End While
                    Connection.Close() 'closes the connection
                    DR.Close() 'closes the reader
                    Return retval

                ElseIf gt = "Lot Acceptance" And tp = "Past 30 Days" Then
                    Dim segment As New List(Of String)()
                    SQL = "Select ISNULL((CAST(s3.TOTAL AS Decimal(10,2)))/(CAST( s3.TOTAL2 AS Decimal(10,2)))*100, 0) AS TOTAL3,dateadd(DAY,0, datediff(day,0, s3.Comp_Date)) AS Comp_Date3  FROM (Select s1.TOTAL, s2.TOTAL2, s2.Comp_Date From ((Select COUNT(InspectionJobSummaryYearly.id) AS TOTAL, dateadd(DAY,0, datediff(day,0, Inspection_Finished)) AS Comp_Date from dbo.InspectionJobSummaryYearly " & WhereString2 & " And Inspection_Finished >= DATEADD(month,-1,GETDATE()) AND Technical_PassFail=1 Group by dateadd(DAY, 0, DateDiff(Day, 0, Inspection_Finished))) AS s1 RIGHT JOIN (Select COUNT(InspectionJobSummaryYearly.id) AS TOTAL2, dateadd(DAY,0, datediff(day,0, Inspection_Finished)) AS Comp_Date from dbo.InspectionJobSummaryYearly " & WhereString2 & " And Inspection_Finished >= DATEADD(month,-1,GETDATE()) Group by dateadd(DAY, 0, DateDiff(Day, 0, Inspection_Finished))) AS s2 ON s1.Comp_Date=s2.Comp_Date)) AS s3 ORDER by Comp_Date3 ASC"
                    Command.CommandType = CommandType.Text 'sets the type of the sql
                    Command.Connection = Connection 'sets the connection of our sql command to MyDB
                    Command.CommandText = SQL 'sets the statement that executes at the data source to our string
                    Connection.Open() 'opens the connction
                    DR = Command.ExecuteReader 'sends the command text to the connection and builds tthe SqlDataReader
                    While DR.Read() 'Check whether the SqlDataReader has 1 or more rows

                        segment = New List(Of String)()
                        segment.Add(DR("Comp_Date3"))
                        segment.Add(DR("TOTAL3"))
                        retval.Add(segment)


                    End While
                    Connection.Close() 'closes the connection
                    DR.Close() 'closes the reader
                    Return retval
                ElseIf gt = "Lot Acceptance" And tp = "Custom" Then
                    Dim segment As New List(Of String)()
                    SQL = "Select ISNULL((CAST(s3.TOTAL AS Decimal(10,2)))/(CAST( s3.TOTAL2 AS Decimal(10,2)))*100, 0) AS TOTAL3,dateadd(DAY,0, datediff(day,0, s3.Comp_Date)) AS Comp_Date3  FROM (Select s1.TOTAL, s2.TOTAL2, s2.Comp_Date From ((Select COUNT(InspectionJobSummaryYearly.id) AS TOTAL, dateadd(DAY,0, datediff(day,0, Inspection_Finished)) AS Comp_Date from dbo.InspectionJobSummaryYearly " & WhereString & " AND Technical_PassFail=1 Group by dateadd(DAY, 0, DateDiff(Day, 0, Inspection_Finished))) AS s1 RIGHT JOIN (Select COUNT(InspectionJobSummaryYearly.id) AS TOTAL2, dateadd(DAY,0, datediff(day,0, Inspection_Finished)) AS Comp_Date from dbo.InspectionJobSummaryYearly " & WhereString & " Group by dateadd(DAY, 0, DateDiff(Day, 0, Inspection_Finished))) AS s2 ON s1.Comp_Date=s2.Comp_Date)) AS s3 ORDER by Comp_Date3 ASC"
                    Command.CommandType = CommandType.Text 'sets the type of the sql
                    Command.Connection = Connection 'sets the connection of our sql command to MyDB
                    Command.CommandText = SQL 'sets the statement that executes at the data source to our string
                    Connection.Open() 'opens the connction
                    DR = Command.ExecuteReader 'sends the command text to the connection and builds tthe SqlDataReader
                    While DR.Read() 'Check whether the SqlDataReader has 1 or more rows

                        segment = New List(Of String)()
                        segment.Add(DR("Comp_Date3"))
                        segment.Add(DR("TOTAL3"))
                        retval.Add(segment)


                    End While
                    Connection.Close() 'closes the connection
                    DR.Close() 'closes the reader
                    Return retval


                End If
            ElseIf fac = "Domestic" Then
                Dim WhereString2 As String = "WHERE (CID = 578 OR CID = 485 OR CID = 482)"
                If AuditType = "ALL" Then
                    WhereString2 = WhereString2 & " AND InspectionType != 'ROLL'"
                ElseIf AuditType = "FINAL AUDIT" Then
                    WhereString2 = WhereString2 & " AND InspectionType = 'EOL'"
                ElseIf AuditType = "IN LINE" Then
                    WhereString2 = WhereString2 & " AND InspectionType = 'IL'"
                Else
                    WhereString2 = WhereString2 & " AND InspectionType = '" & AuditType & "'"
                End If
                If DataNo <> "ALL" Then
                    WhereString2 = WhereString2 & " AND DataNo = '" & DataNo & "'"
                End If
                If WorkOrder <> "ALL" Then
                    WhereString2 = WhereString2 & " AND JobNumber = '" & WorkOrder & "'"
                End If
                Dim WhereString As String = "WHERE (CID = 578 OR CID = 485 OR CID = 482) AND Inspection_Finished BETWEEN '" & Fromdate & "' AND '" & Todate & "'"
                If AuditType = "ALL" Then
                    WhereString = WhereString & " AND InspectionType != 'ROLL'"
                ElseIf AuditType = "FINAL AUDIT" Then
                    WhereString = WhereString & " AND InspectionType = 'EOL'"
                ElseIf AuditType = "IN LINE" Then
                    WhereString = WhereString & " AND InspectionType = 'IL'"
                Else
                    WhereString = WhereString & " AND InspectionType = '" & AuditType & "'"
                End If
                If DataNo <> "ALL" Then
                    WhereString = WhereString & " AND DataNo = '" & DataNo & "'"
                End If
                If WorkOrder <> "ALL" Then
                    WhereString = WhereString & " AND JobNumber = '" & WorkOrder & "'"
                End If
                Dim SQL As String = ""

                If gt = "No. of Defects" And tp = "Past Year" Then
                    Dim segment As New List(Of String)()
                    SQL = "Select ISNULL(SUM(MajorsCount+MinorsCount+RepairsCount+ScrapCount), 0) AS TOTAL, dateadd(DAY,0, datediff(day,0, Inspection_Finished)) AS Comp_Date from dbo.InspectionJobSummaryYearly " & WhereString2 & "Group by dateadd(DAY, 0, DateDiff(Day, 0, Inspection_Finished))  ORDER BY Comp_Date ASC"
                    Command.CommandType = CommandType.Text 'sets the type of the sql
                    Command.Connection = Connection 'sets the connection of our sql command to MyDB
                    Command.CommandText = SQL 'sets the statement that executes at the data source to our string
                    Connection.Open() 'opens the connction
                    DR = Command.ExecuteReader 'sends the command text to the connection and builds tthe SqlDataReader
                    While DR.Read() 'Check whether the SqlDataReader has 1 or more rows

                        segment = New List(Of String)()
                        segment.Add(DR("Comp_Date"))
                        segment.Add(DR("TOTAL"))
                        retval.Add(segment)


                    End While
                    Connection.Close() 'closes the connection
                    DR.Close() 'closes the reader
                    Return retval
                ElseIf gt = "No. of Defects" And tp = "Yesterday" Then
                    Dim segment As New List(Of String)()
                    SQL = "Select ISNULL(SUM(MajorsCount+MinorsCount+RepairsCount+ScrapCount), 0) AS TOTAL, dateadd(DAY,0, datediff(day,0, Inspection_Finished)) AS Comp_Date from dbo.InspectionJobSummaryYearly " & WhereString2 & " And Inspection_Finished >= dateadd(day,datediff(day,1,GETDATE()),0)AND Inspection_Finished < dateadd(day,datediff(day,0,GETDATE()),0) Group by dateadd(DAY, 0, DateDiff(Day, 0, Inspection_Finished))  ORDER BY Comp_Date ASC"
                    Command.CommandType = CommandType.Text 'sets the type of the sql
                    Command.Connection = Connection 'sets the connection of our sql command to MyDB
                    Command.CommandText = SQL 'sets the statement that executes at the data source to our string
                    Connection.Open() 'opens the connction
                    DR = Command.ExecuteReader 'sends the command text to the connection and builds tthe SqlDataReader
                    While DR.Read() 'Check whether the SqlDataReader has 1 or more rows

                        segment = New List(Of String)()
                        segment.Add(DR("Comp_Date"))
                        segment.Add(DR("TOTAL"))
                        retval.Add(segment)


                    End While
                    Connection.Close() 'closes the connection
                    DR.Close() 'closes the reader
                    Return retval
                ElseIf gt = "No. of Rejects" And tp = "Yesterday" Then
                    Dim segment As New List(Of String)()
                    SQL = "Select ISNULL(SUM(MajorsCount+RepairsCount+ScrapCount), 0) AS TOTAL, dateadd(DAY,0, datediff(day,0, Inspection_Finished)) AS Comp_Date from dbo.InspectionJobSummaryYearly " & WhereString2 & " And Inspection_Finished >= dateadd(day,datediff(day,1,GETDATE()),0) AND Inspection_Finished < dateadd(day,datediff(day,0,GETDATE()),0) Group by dateadd(DAY, 0, DateDiff(Day, 0, Inspection_Finished))  ORDER BY Comp_Date ASC"
                    Command.CommandType = CommandType.Text 'sets the type of the sql
                    Command.Connection = Connection 'sets the connection of our sql command to MyDB
                    Command.CommandText = SQL 'sets the statement that executes at the data source to our string
                    Connection.Open() 'opens the connction
                    DR = Command.ExecuteReader 'sends the command text to the connection and builds tthe SqlDataReader
                    While DR.Read() 'Check whether the SqlDataReader has 1 or more rows

                        segment = New List(Of String)()
                        segment.Add(DR("Comp_Date"))
                        segment.Add(DR("TOTAL"))
                        retval.Add(segment)


                    End While
                    Connection.Close() 'closes the connection
                    DR.Close() 'closes the reader
                    Return retval
                ElseIf gt = "No. of Inspections" And tp = "Yesterday" Then
                    Dim segment As New List(Of String)()
                    SQL = "Select COUNT(InspectionJobSummaryYearly.id) AS TOTAL, dateadd(DAY,0, datediff(day,0, Inspection_Finished)) AS Comp_Date from dbo.InspectionJobSummaryYearly " & WhereString2 & " And Inspection_Finished >= dateadd(day,datediff(day,1,GETDATE()),0) AND Inspection_Finished < dateadd(day,datediff(day,0,GETDATE()),0) Group by dateadd(DAY, 0, DateDiff(Day, 0, Inspection_Finished))  ORDER BY Comp_Date ASC"
                    Command.CommandType = CommandType.Text 'sets the type of the sql
                    Command.Connection = Connection 'sets the connection of our sql command to MyDB
                    Command.CommandText = SQL 'sets the statement that executes at the data source to our string
                    Connection.Open() 'opens the connction
                    DR = Command.ExecuteReader 'sends the command text to the connection and builds tthe SqlDataReader
                    While DR.Read() 'Check whether the SqlDataReader has 1 or more rows

                        segment = New List(Of String)()
                        segment.Add(DR("Comp_Date"))
                        segment.Add(DR("TOTAL"))
                        retval.Add(segment)


                    End While
                    Connection.Close() 'closes the connection
                    DR.Close() 'closes the reader
                    Return retval
                ElseIf gt = "No. of Rejected Lots" And tp = "Yesterday" Then
                    Dim segment As New List(Of String)()
                    SQL = "Select COUNT(InspectionJobSummaryYearly.id) AS TOTAL, dateadd(DAY,0, datediff(day,0, Inspection_Finished)) AS Comp_Date from dbo.InspectionJobSummaryYearly " & WhereString2 & " AND Technical_PassFail=0 And Inspection_Finished >= dateadd(day,datediff(day,1,GETDATE()),0) AND Inspection_Finished < dateadd(day,datediff(day,0,GETDATE()),0) Group by dateadd(DAY, 0, DateDiff(Day, 0, Inspection_Finished))  ORDER BY Comp_Date ASC"
                    Command.CommandType = CommandType.Text 'sets the type of the sql
                    Command.Connection = Connection 'sets the connection of our sql command to MyDB
                    Command.CommandText = SQL 'sets the statement that executes at the data source to our string
                    Connection.Open() 'opens the connction
                    DR = Command.ExecuteReader 'sends the command text to the connection and builds tthe SqlDataReader
                    While DR.Read() 'Check whether the SqlDataReader has 1 or more rows

                        segment = New List(Of String)()
                        segment.Add(DR("Comp_Date"))
                        segment.Add(DR("TOTAL"))
                        retval.Add(segment)


                    End While
                    Connection.Close() 'closes the connection
                    DR.Close() 'closes the reader
                    Return retval
                ElseIf gt = "DHU" And tp = "Yesterday" Then
                    Dim segment As New List(Of String)()
                    SQL = "Select ISNULL((CAST(SUM(MajorsCount+MinorsCount+RepairsCount+ScrapCount) AS Decimal(10,2))/CAST(SUM(SampleSize) AS Decimal(10,2)))*100, 0) AS TOTAL, dateadd(DAY,0, datediff(day,0, Inspection_Finished)) AS Comp_Date from dbo.InspectionJobSummaryYearly " & WhereString2 & " And Inspection_Finished >= dateadd(day,datediff(day,1,GETDATE()),0) AND Inspection_Finished < dateadd(day,datediff(day,0,GETDATE()),0) Group by dateadd(DAY, 0, DateDiff(Day, 0, Inspection_Finished))  ORDER BY Comp_Date ASC"
                    Command.CommandType = CommandType.Text 'sets the type of the sql
                    Command.Connection = Connection 'sets the connection of our sql command to MyDB
                    Command.CommandText = SQL 'sets the statement that executes at the data source to our string
                    Connection.Open() 'opens the connction
                    DR = Command.ExecuteReader 'sends the command text to the connection and builds tthe SqlDataReader
                    While DR.Read() 'Check whether the SqlDataReader has 1 or more rows

                        segment = New List(Of String)()
                        segment.Add(DR("Comp_Date"))
                        segment.Add(DR("TOTAL"))
                        retval.Add(segment)


                    End While
                    Connection.Close() 'closes the connection
                    DR.Close() 'closes the reader
                    Return retval
                ElseIf gt = "Reject Rate" And tp = "Yesterday" Then
                    Dim segment As New List(Of String)()
                    SQL = "Select ISNULL((CAST(SUM(MajorsCount+RepairsCount+ScrapCount) AS Decimal(10,2))/CAST(SUM(SampleSize) AS Decimal(10,2)))*100, 0) AS TOTAL, dateadd(DAY,0, datediff(day,0, Inspection_Finished)) AS Comp_Date from dbo.InspectionJobSummaryYearly " & WhereString2 & " And Inspection_Finished >= dateadd(day,datediff(day,1,GETDATE()),0) AND Inspection_Finished < dateadd(day,datediff(day,0,GETDATE()),0) Group by dateadd(DAY, 0, DateDiff(Day, 0, Inspection_Finished))  ORDER BY Comp_Date ASC"
                    Command.CommandType = CommandType.Text 'sets the type of the sql
                    Command.Connection = Connection 'sets the connection of our sql command to MyDB
                    Command.CommandText = SQL 'sets the statement that executes at the data source to our string
                    Connection.Open() 'opens the connction
                    DR = Command.ExecuteReader 'sends the command text to the connection and builds tthe SqlDataReader
                    While DR.Read() 'Check whether the SqlDataReader has 1 or more rows

                        segment = New List(Of String)()
                        segment.Add(DR("Comp_Date"))
                        segment.Add(DR("TOTAL"))
                        retval.Add(segment)


                    End While
                    Connection.Close() 'closes the connection
                    DR.Close() 'closes the reader
                    Return retval
                ElseIf gt = "Lot Acceptance" And tp = "Yesterday" Then
                    Dim segment As New List(Of String)()
                    SQL = "Select ISNULL((CAST(s3.TOTAL AS Decimal(10,2)))/(CAST( s3.TOTAL2 AS Decimal(10,2)))*100, 0) AS TOTAL3,dateadd(DAY,0, datediff(day,0, s3.Comp_Date)) AS Comp_Date3  FROM (Select s1.TOTAL, s2.TOTAL2, s2.Comp_Date From ((Select COUNT(InspectionJobSummaryYearly.id) AS TOTAL, dateadd(DAY,0, datediff(day,0, Inspection_Finished)) AS Comp_Date from dbo.InspectionJobSummaryYearly " & WhereString2 & " And Inspection_Finished >= dateadd(day,datediff(day,1,GETDATE()),0) AND Inspection_Finished < dateadd(day,datediff(day,0,GETDATE()),0) AND Technical_PassFail=1 Group by dateadd(DAY, 0, DateDiff(Day, 0, Inspection_Finished))) AS s1 RIGHT JOIN (Select COUNT(InspectionJobSummaryYearly.id) AS TOTAL2, dateadd(DAY,0, datediff(day,0, Inspection_Finished)) AS Comp_Date from dbo.InspectionJobSummaryYearly " & WhereString2 & " And Inspection_Finished >= dateadd(day,datediff(day,1,GETDATE()),0) AND Inspection_Finished < dateadd(day,datediff(day,0,GETDATE()),0) Group by dateadd(DAY, 0, DateDiff(Day, 0, Inspection_Finished))) AS s2 ON s1.Comp_Date=s2.Comp_Date)) AS s3 ORDER by Comp_Date3 ASC"
                    Command.CommandType = CommandType.Text 'sets the type of the sql
                    Command.Connection = Connection 'sets the connection of our sql command to MyDB
                    Command.CommandText = SQL 'sets the statement that executes at the data source to our string
                    Connection.Open() 'opens the connction
                    DR = Command.ExecuteReader 'sends the command text to the connection and builds tthe SqlDataReader
                    While DR.Read() 'Check whether the SqlDataReader has 1 or more rows

                        segment = New List(Of String)()
                        segment.Add(DR("Comp_Date3"))
                        segment.Add(DR("TOTAL3"))
                        retval.Add(segment)


                    End While
                    Connection.Close() 'closes the connection
                    DR.Close() 'closes the reader
                    Return retval

                ElseIf gt = "No. of Defects" And tp = "Past 30 Days" Then
                    Dim segment As New List(Of String)()
                    SQL = "Select ISNULL(SUM(MajorsCount+MinorsCount+RepairsCount+ScrapCount), 0) AS TOTAL, dateadd(DAY,0, datediff(day,0, Inspection_Finished)) AS Comp_Date from dbo.InspectionJobSummaryYearly " & WhereString2 & " And Inspection_Finished >= DATEADD(month,-1,GETDATE()) Group by dateadd(DAY, 0, DateDiff(Day, 0, Inspection_Finished))  ORDER BY Comp_Date ASC"
                    Command.CommandType = CommandType.Text 'sets the type of the sql
                    Command.Connection = Connection 'sets the connection of our sql command to MyDB
                    Command.CommandText = SQL 'sets the statement that executes at the data source to our string
                    Connection.Open() 'opens the connction
                    DR = Command.ExecuteReader 'sends the command text to the connection and builds tthe SqlDataReader
                    While DR.Read() 'Check whether the SqlDataReader has 1 or more rows

                        segment = New List(Of String)()
                        segment.Add(DR("Comp_Date"))
                        segment.Add(DR("TOTAL"))
                        retval.Add(segment)


                    End While
                    Connection.Close() 'closes the connection
                    DR.Close() 'closes the reader
                    Return retval
                ElseIf gt = "No. of Defects" And tp = "Custom" Then
                    Dim segment As New List(Of String)()
                    SQL = "Select ISNULL(SUM(MajorsCount+MinorsCount+RepairsCount+ScrapCount), 0) AS TOTAL, dateadd(DAY,0, datediff(day,0, Inspection_Finished)) AS Comp_Date from dbo.InspectionJobSummaryYearly " & WhereString & " Group by dateadd(DAY, 0, DateDiff(Day, 0, Inspection_Finished))  ORDER BY Comp_Date ASC"
                    Command.CommandType = CommandType.Text 'sets the type of the sql
                    Command.Connection = Connection 'sets the connection of our sql command to MyDB
                    Command.CommandText = SQL 'sets the statement that executes at the data source to our string
                    Connection.Open() 'opens the connction
                    DR = Command.ExecuteReader 'sends the command text to the connection and builds tthe SqlDataReader
                    While DR.Read() 'Check whether the SqlDataReader has 1 or more rows

                        segment = New List(Of String)()
                        segment.Add(DR("Comp_Date"))
                        segment.Add(DR("TOTAL"))
                        retval.Add(segment)


                    End While
                    Connection.Close() 'closes the connection
                    DR.Close() 'closes the reader
                    Return retval
                ElseIf gt = "No. of Rejects" And tp = "Past Year" Then
                    Dim segment As New List(Of String)()
                    SQL = "Select ISNULL(SUM(MajorsCount+RepairsCount+ScrapCount), 0) AS TOTAL, dateadd(DAY,0, datediff(day,0, Inspection_Finished)) AS Comp_Date from dbo.InspectionJobSummaryYearly " & WhereString2 & " Group by dateadd(DAY, 0, DateDiff(Day, 0, Inspection_Finished))  ORDER BY Comp_Date ASC"
                    Command.CommandType = CommandType.Text 'sets the type of the sql
                    Command.Connection = Connection 'sets the connection of our sql command to MyDB
                    Command.CommandText = SQL 'sets the statement that executes at the data source to our string
                    Connection.Open() 'opens the connction
                    DR = Command.ExecuteReader 'sends the command text to the connection and builds tthe SqlDataReader
                    While DR.Read() 'Check whether the SqlDataReader has 1 or more rows

                        segment = New List(Of String)()
                        segment.Add(DR("Comp_Date"))
                        segment.Add(DR("TOTAL"))
                        retval.Add(segment)


                    End While
                    Connection.Close() 'closes the connection
                    DR.Close() 'closes the reader
                    Return retval
                ElseIf gt = "No. of Rejects" And tp = "Past 30 Days" Then
                    Dim segment As New List(Of String)()
                    SQL = "Select ISNULL(SUM(MajorsCount+RepairsCount+ScrapCount), 0) AS TOTAL, dateadd(DAY,0, datediff(day,0, Inspection_Finished)) AS Comp_Date from dbo.InspectionJobSummaryYearly " & WhereString2 & " And Inspection_Finished >= DATEADD(month,-1,GETDATE()) Group by dateadd(DAY, 0, DateDiff(Day, 0, Inspection_Finished))  ORDER BY Comp_Date ASC"
                    Command.CommandType = CommandType.Text 'sets the type of the sql
                    Command.Connection = Connection 'sets the connection of our sql command to MyDB
                    Command.CommandText = SQL 'sets the statement that executes at the data source to our string
                    Connection.Open() 'opens the connction
                    DR = Command.ExecuteReader 'sends the command text to the connection and builds tthe SqlDataReader
                    While DR.Read() 'Check whether the SqlDataReader has 1 or more rows

                        segment = New List(Of String)()
                        segment.Add(DR("Comp_Date"))
                        segment.Add(DR("TOTAL"))
                        retval.Add(segment)


                    End While
                    Connection.Close() 'closes the connection
                    DR.Close() 'closes the reader
                    Return retval
                ElseIf gt = "No. of Rejects" And tp = "Custom" Then
                    Dim segment As New List(Of String)()
                    SQL = "Select ISNULL(SUM(MajorsCount+RepairsCount+ScrapCount), 0) AS TOTAL, dateadd(DAY,0, datediff(day,0, Inspection_Finished)) AS Comp_Date from dbo.InspectionJobSummaryYearly " & WhereString & "Group by dateadd(DAY, 0, DateDiff(Day, 0, Inspection_Finished))  ORDER BY Comp_Date ASC"
                    Command.CommandType = CommandType.Text 'sets the type of the sql
                    Command.Connection = Connection 'sets the connection of our sql command to MyDB
                    Command.CommandText = SQL 'sets the statement that executes at the data source to our string
                    Connection.Open() 'opens the connction
                    DR = Command.ExecuteReader 'sends the command text to the connection and builds tthe SqlDataReader
                    While DR.Read() 'Check whether the SqlDataReader has 1 or more rows

                        segment = New List(Of String)()
                        segment.Add(DR("Comp_Date"))
                        segment.Add(DR("TOTAL"))
                        retval.Add(segment)


                    End While
                    Connection.Close() 'closes the connection
                    DR.Close() 'closes the reader
                    Return retval
                ElseIf gt = "No. of Inspections" And tp = "Past Year" Then
                    Dim segment As New List(Of String)()
                    SQL = "Select COUNT(InspectionJobSummaryYearly.id) AS TOTAL, dateadd(DAY,0, datediff(day,0, Inspection_Finished)) AS Comp_Date from dbo.InspectionJobSummaryYearly " & WhereString2 & " Group by dateadd(DAY, 0, DateDiff(Day, 0, Inspection_Finished))  ORDER BY Comp_Date ASC"
                    Command.CommandType = CommandType.Text 'sets the type of the sql
                    Command.Connection = Connection 'sets the connection of our sql command to MyDB
                    Command.CommandText = SQL 'sets the statement that executes at the data source to our string
                    Connection.Open() 'opens the connction
                    DR = Command.ExecuteReader 'sends the command text to the connection and builds tthe SqlDataReader
                    While DR.Read() 'Check whether the SqlDataReader has 1 or more rows

                        segment = New List(Of String)()
                        segment.Add(DR("Comp_Date"))
                        segment.Add(DR("TOTAL"))
                        retval.Add(segment)


                    End While
                    Connection.Close() 'closes the connection
                    DR.Close() 'closes the reader
                    Return retval
                ElseIf gt = "No. of Inspections" And tp = "Past 30 Days" Then
                    Dim segment As New List(Of String)()
                    SQL = "Select COUNT(InspectionJobSummaryYearly.id) AS TOTAL, dateadd(DAY,0, datediff(day,0, Inspection_Finished)) AS Comp_Date from dbo.InspectionJobSummaryYearly " & WhereString2 & " And Inspection_Finished >= DATEADD(month,-1,GETDATE()) Group by dateadd(DAY, 0, DateDiff(Day, 0, Inspection_Finished))  ORDER BY Comp_Date ASC"
                    Command.CommandType = CommandType.Text 'sets the type of the sql
                    Command.Connection = Connection 'sets the connection of our sql command to MyDB
                    Command.CommandText = SQL 'sets the statement that executes at the data source to our string
                    Connection.Open() 'opens the connction
                    DR = Command.ExecuteReader 'sends the command text to the connection and builds tthe SqlDataReader
                    While DR.Read() 'Check whether the SqlDataReader has 1 or more rows

                        segment = New List(Of String)()
                        segment.Add(DR("Comp_Date"))
                        segment.Add(DR("TOTAL"))
                        retval.Add(segment)


                    End While
                    Connection.Close() 'closes the connection
                    DR.Close() 'closes the reader
                    Return retval
                ElseIf gt = "No. of Inspections" And tp = "Custom" Then
                    Dim segment As New List(Of String)()
                    SQL = "Select COUNT(InspectionJobSummaryYearly.id) AS TOTAL, dateadd(DAY,0, datediff(day,0, Inspection_Finished)) AS Comp_Date from dbo.InspectionJobSummaryYearly " & WhereString & " Group by dateadd(DAY, 0, DateDiff(Day, 0, Inspection_Finished))  ORDER BY Comp_Date ASC"
                    Command.CommandType = CommandType.Text 'sets the type of the sql
                    Command.Connection = Connection 'sets the connection of our sql command to MyDB
                    Command.CommandText = SQL 'sets the statement that executes at the data source to our string
                    Connection.Open() 'opens the connction
                    DR = Command.ExecuteReader 'sends the command text to the connection and builds tthe SqlDataReader
                    While DR.Read() 'Check whether the SqlDataReader has 1 or more rows

                        segment = New List(Of String)()
                        segment.Add(DR("Comp_Date"))
                        segment.Add(DR("TOTAL"))
                        retval.Add(segment)


                    End While
                    Connection.Close() 'closes the connection
                    DR.Close() 'closes the reader
                    Return retval
                ElseIf gt = "No. of Rejected Lots" And tp = "Past Year" Then
                    Dim segment As New List(Of String)()
                    SQL = "Select COUNT(InspectionJobSummaryYearly.id) AS TOTAL, dateadd(DAY,0, datediff(day,0, Inspection_Finished)) AS Comp_Date from dbo.InspectionJobSummaryYearly " & WhereString2 & " AND Technical_PassFail=0 Group by dateadd(DAY, 0, DateDiff(Day, 0, Inspection_Finished))  ORDER BY Comp_Date ASC"
                    Command.CommandType = CommandType.Text 'sets the type of the sql
                    Command.Connection = Connection 'sets the connection of our sql command to MyDB
                    Command.CommandText = SQL 'sets the statement that executes at the data source to our string
                    Connection.Open() 'opens the connction
                    DR = Command.ExecuteReader 'sends the command text to the connection and builds tthe SqlDataReader
                    While DR.Read() 'Check whether the SqlDataReader has 1 or more rows

                        segment = New List(Of String)()
                        segment.Add(DR("Comp_Date"))
                        segment.Add(DR("TOTAL"))
                        retval.Add(segment)


                    End While
                    Connection.Close() 'closes the connection
                    DR.Close() 'closes the reader
                    Return retval
                ElseIf gt = "No. of Rejected Lots" And tp = "Past 30 Days" Then
                    Dim segment As New List(Of String)()
                    SQL = "Select COUNT(InspectionJobSummaryYearly.id) AS TOTAL, dateadd(DAY,0, datediff(day,0, Inspection_Finished)) AS Comp_Date from dbo.InspectionJobSummaryYearly " & WhereString2 & " And Inspection_Finished >= DATEADD(month,-1,GETDATE()) AND Technical_PassFail=0 Group by dateadd(DAY, 0, DateDiff(Day, 0, Inspection_Finished))  ORDER BY Comp_Date ASC"
                    Command.CommandType = CommandType.Text 'sets the type of the sql
                    Command.Connection = Connection 'sets the connection of our sql command to MyDB
                    Command.CommandText = SQL 'sets the statement that executes at the data source to our string
                    Connection.Open() 'opens the connction
                    DR = Command.ExecuteReader 'sends the command text to the connection and builds tthe SqlDataReader
                    While DR.Read() 'Check whether the SqlDataReader has 1 or more rows

                        segment = New List(Of String)()
                        segment.Add(DR("Comp_Date"))
                        segment.Add(DR("TOTAL"))
                        retval.Add(segment)


                    End While
                    Connection.Close() 'closes the connection
                    DR.Close() 'closes the reader
                    Return retval
                ElseIf gt = "No. of Rejected Lots" And tp = "Custom" Then
                    Dim segment As New List(Of String)()
                    SQL = "Select COUNT(InspectionJobSummaryYearly.id) AS TOTAL, dateadd(DAY,0, datediff(day,0, Inspection_Finished)) AS Comp_Date from dbo.InspectionJobSummaryYearly " & WhereString & " AND Technical_PassFail=0 Group by dateadd(DAY, 0, DateDiff(Day, 0, Inspection_Finished))  ORDER BY Comp_Date ASC"
                    Command.CommandType = CommandType.Text 'sets the type of the sql
                    Command.Connection = Connection 'sets the connection of our sql command to MyDB
                    Command.CommandText = SQL 'sets the statement that executes at the data source to our string
                    Connection.Open() 'opens the connction
                    DR = Command.ExecuteReader 'sends the command text to the connection and builds tthe SqlDataReader
                    While DR.Read() 'Check whether the SqlDataReader has 1 or more rows

                        segment = New List(Of String)()
                        segment.Add(DR("Comp_Date"))
                        segment.Add(DR("TOTAL"))
                        retval.Add(segment)


                    End While
                    Connection.Close() 'closes the connection
                    DR.Close() 'closes the reader
                    Return retval
                ElseIf gt = "DHU" And tp = "Past Year" Then
                    Dim segment As New List(Of String)()
                    SQL = "Select ISNULL((CAST(SUM(MajorsCount+MinorsCount+RepairsCount+ScrapCount) AS Decimal(10,2))/CAST(SUM(SampleSize) AS Decimal(10,2)))*100, 0) AS TOTAL, dateadd(DAY,0, datediff(day,0, Inspection_Finished)) AS Comp_Date from dbo.InspectionJobSummaryYearly " & WhereString2 & " Group by dateadd(DAY, 0, DateDiff(Day, 0, Inspection_Finished))  ORDER BY Comp_Date ASC"
                    Command.CommandType = CommandType.Text 'sets the type of the sql
                    Command.Connection = Connection 'sets the connection of our sql command to MyDB
                    Command.CommandText = SQL 'sets the statement that executes at the data source to our string
                    Connection.Open() 'opens the connction
                    DR = Command.ExecuteReader 'sends the command text to the connection and builds tthe SqlDataReader
                    While DR.Read() 'Check whether the SqlDataReader has 1 or more rows

                        segment = New List(Of String)()
                        segment.Add(DR("Comp_Date"))
                        segment.Add(DR("TOTAL"))
                        retval.Add(segment)


                    End While
                    Connection.Close() 'closes the connection
                    DR.Close() 'closes the reader
                    Return retval

                ElseIf gt = "DHU" And tp = "Past 30 Days" Then
                    Dim segment As New List(Of String)()
                    SQL = "Select ISNULL((CAST(SUM(MajorsCount+MinorsCount+RepairsCount+ScrapCount) AS Decimal(10,2))/CAST(SUM(SampleSize) AS Decimal(10,2)))*100, 0) AS TOTAL , dateadd(DAY,0, datediff(day,0, Inspection_Finished)) AS Comp_Date from dbo.InspectionJobSummaryYearly " & WhereString2 & " And Inspection_Finished >= DATEADD(month,-1,GETDATE()) Group by dateadd(DAY, 0, DateDiff(Day, 0, Inspection_Finished))  ORDER BY Comp_Date ASC"
                    Command.CommandType = CommandType.Text 'sets the type of the sql
                    Command.Connection = Connection 'sets the connection of our sql command to MyDB
                    Command.CommandText = SQL 'sets the statement that executes at the data source to our string
                    Connection.Open() 'opens the connction
                    DR = Command.ExecuteReader 'sends the command text to the connection and builds tthe SqlDataReader
                    While DR.Read() 'Check whether the SqlDataReader has 1 or more rows

                        segment = New List(Of String)()
                        segment.Add(DR("Comp_Date"))
                        segment.Add(DR("TOTAL"))
                        retval.Add(segment)


                    End While
                    Connection.Close() 'closes the connection
                    DR.Close() 'closes the reader
                    Return retval
                ElseIf gt = "DHU" And tp = "Custom" Then
                    Dim segment As New List(Of String)()
                    SQL = "Select ISNULL((CAST(SUM(MajorsCount+MinorsCount+RepairsCount+ScrapCount) AS Decimal(10,2))/CAST(SUM(SampleSize) AS Decimal(10,2)))*100, 0) AS TOTAL, dateadd(DAY,0, datediff(day,0, Inspection_Finished)) AS Comp_Date from dbo.InspectionJobSummaryYearly " & WhereString & " Group by dateadd(DAY, 0, DateDiff(Day, 0, Inspection_Finished))  ORDER BY Comp_Date ASC"
                    Command.CommandType = CommandType.Text 'sets the type of the sql
                    Command.Connection = Connection 'sets the connection of our sql command to MyDB
                    Command.CommandText = SQL 'sets the statement that executes at the data source to our string
                    Connection.Open() 'opens the connction
                    DR = Command.ExecuteReader 'sends the command text to the connection and builds tthe SqlDataReader
                    While DR.Read() 'Check whether the SqlDataReader has 1 or more rows

                        segment = New List(Of String)()
                        segment.Add(DR("Comp_Date"))
                        segment.Add(DR("TOTAL"))
                        retval.Add(segment)


                    End While
                    Connection.Close() 'closes the connection
                    DR.Close() 'closes the reader
                    Return retval

                ElseIf gt = "Reject Rate" And tp = "Past Year" Then
                    Dim segment As New List(Of String)()
                    SQL = "Select ISNULL((CAST(SUM(MajorsCount+RepairsCount+ScrapCount) AS Decimal(10,2))/CAST(SUM(SampleSize) AS Decimal(10,2)))*100, 0) AS TOTAL, dateadd(DAY,0, datediff(day,0, Inspection_Finished)) AS Comp_Date from dbo.InspectionJobSummaryYearly " & WhereString2 & " Group by dateadd(DAY, 0, DateDiff(Day, 0, Inspection_Finished))  ORDER BY Comp_Date ASC"
                    Command.CommandType = CommandType.Text 'sets the type of the sql
                    Command.Connection = Connection 'sets the connection of our sql command to MyDB
                    Command.CommandText = SQL 'sets the statement that executes at the data source to our string
                    Connection.Open() 'opens the connction
                    DR = Command.ExecuteReader 'sends the command text to the connection and builds tthe SqlDataReader
                    While DR.Read() 'Check whether the SqlDataReader has 1 or more rows

                        segment = New List(Of String)()
                        segment.Add(DR("Comp_Date"))
                        segment.Add(DR("TOTAL"))
                        retval.Add(segment)


                    End While
                    Connection.Close() 'closes the connection
                    DR.Close() 'closes the reader
                    Return retval

                ElseIf gt = "Reject Rate" And tp = "Past 30 Days" Then
                    Dim segment As New List(Of String)()
                    SQL = "Select ISNULL((CAST(SUM(MajorsCount+RepairsCount+ScrapCount) AS Decimal(10,2))/CAST(SUM(SampleSize) AS Decimal(10,2)))*100, 0) AS TOTAL , dateadd(DAY,0, datediff(day,0, Inspection_Finished)) AS Comp_Date from dbo.InspectionJobSummaryYearly " & WhereString2 & " And Inspection_Finished >= DATEADD(month,-1,GETDATE()) Group by dateadd(DAY, 0, DateDiff(Day, 0, Inspection_Finished))  ORDER BY Comp_Date ASC"
                    Command.CommandType = CommandType.Text 'sets the type of the sql
                    Command.Connection = Connection 'sets the connection of our sql command to MyDB
                    Command.CommandText = SQL 'sets the statement that executes at the data source to our string
                    Connection.Open() 'opens the connction
                    DR = Command.ExecuteReader 'sends the command text to the connection and builds tthe SqlDataReader
                    While DR.Read() 'Check whether the SqlDataReader has 1 or more rows

                        segment = New List(Of String)()
                        segment.Add(DR("Comp_Date"))
                        segment.Add(DR("TOTAL"))
                        retval.Add(segment)


                    End While
                    Connection.Close() 'closes the connection
                    DR.Close() 'closes the reader
                    Return retval
                ElseIf gt = "Reject Rate" And tp = "Custom" Then
                    Dim segment As New List(Of String)()
                    SQL = "Select ISNULL((CAST(SUM(MajorsCount+RepairsCount+ScrapCount) AS Decimal(10,2))/CAST(SUM(SampleSize) AS Decimal(10,2)))*100, 0) AS TOTAL, dateadd(DAY,0, datediff(day,0, Inspection_Finished)) AS Comp_Date from dbo.InspectionJobSummaryYearly " & WhereString & " Group by dateadd(DAY, 0, DateDiff(Day, 0, Inspection_Finished))  ORDER BY Comp_Date ASC"
                    Command.CommandType = CommandType.Text 'sets the type of the sql
                    Command.Connection = Connection 'sets the connection of our sql command to MyDB
                    Command.CommandText = SQL 'sets the statement that executes at the data source to our string
                    Connection.Open() 'opens the connction
                    DR = Command.ExecuteReader 'sends the command text to the connection and builds tthe SqlDataReader
                    While DR.Read() 'Check whether the SqlDataReader has 1 or more rows

                        segment = New List(Of String)()
                        segment.Add(DR("Comp_Date"))
                        segment.Add(DR("TOTAL"))
                        retval.Add(segment)


                    End While
                    Connection.Close() 'closes the connection
                    DR.Close() 'closes the reader
                    Return retval

                ElseIf gt = "Lot Acceptance" And tp = "Past Year" Then
                    Dim segment As New List(Of String)()
                    SQL = "Select ISNULL((CAST(s3.TOTAL AS Decimal(10,2)))/(CAST( s3.TOTAL2 AS Decimal(10,2)))*100, 0) AS TOTAL3,dateadd(DAY,0, datediff(day,0, s3.Comp_Date)) AS Comp_Date3  FROM (Select s1.TOTAL, s2.TOTAL2, s2.Comp_Date From ((Select COUNT(InspectionJobSummaryYearly.id) AS TOTAL, dateadd(DAY,0, datediff(day,0, Inspection_Finished)) AS Comp_Date from dbo.InspectionJobSummaryYearly " & WhereString2 & " AND Technical_PassFail=1 Group by dateadd(DAY, 0, DateDiff(Day, 0, Inspection_Finished))) AS s1 RIGHT JOIN (Select COUNT(InspectionJobSummaryYearly.id) AS TOTAL2, dateadd(DAY,0, datediff(day,0, Inspection_Finished)) AS Comp_Date from dbo.InspectionJobSummaryYearly " & WhereString2 & " Group by dateadd(DAY, 0, DateDiff(Day, 0, Inspection_Finished))) AS s2 ON s1.Comp_Date=s2.Comp_Date)) AS s3 ORDER by Comp_Date3 ASC"
                    Command.CommandType = CommandType.Text 'sets the type of the sql
                    Command.Connection = Connection 'sets the connection of our sql command to MyDB
                    Command.CommandText = SQL 'sets the statement that executes at the data source to our string
                    Connection.Open() 'opens the connction
                    DR = Command.ExecuteReader 'sends the command text to the connection and builds tthe SqlDataReader
                    While DR.Read() 'Check whether the SqlDataReader has 1 or more rows

                        segment = New List(Of String)()
                        segment.Add(DR("Comp_Date3"))
                        segment.Add(DR("TOTAL3"))
                        retval.Add(segment)


                    End While
                    Connection.Close() 'closes the connection
                    DR.Close() 'closes the reader
                    Return retval

                ElseIf gt = "Lot Acceptance" And tp = "Past 30 Days" Then
                    Dim segment As New List(Of String)()
                    SQL = "Select ISNULL((CAST(s3.TOTAL AS Decimal(10,2)))/(CAST( s3.TOTAL2 AS Decimal(10,2)))*100, 0) AS TOTAL3,dateadd(DAY,0, datediff(day,0, s3.Comp_Date)) AS Comp_Date3  FROM (Select s1.TOTAL, s2.TOTAL2, s2.Comp_Date From ((Select COUNT(InspectionJobSummaryYearly.id) AS TOTAL, dateadd(DAY,0, datediff(day,0, Inspection_Finished)) AS Comp_Date from dbo.InspectionJobSummaryYearly " & WhereString2 & " And Inspection_Finished >= DATEADD(month,-1,GETDATE()) AND Technical_PassFail=1 Group by dateadd(DAY, 0, DateDiff(Day, 0, Inspection_Finished))) AS s1 RIGHT JOIN (Select COUNT(InspectionJobSummaryYearly.id) AS TOTAL2, dateadd(DAY,0, datediff(day,0, Inspection_Finished)) AS Comp_Date from dbo.InspectionJobSummaryYearly " & WhereString2 & " And Inspection_Finished >= DATEADD(month,-1,GETDATE()) Group by dateadd(DAY, 0, DateDiff(Day, 0, Inspection_Finished))) AS s2 ON s1.Comp_Date=s2.Comp_Date)) AS s3 ORDER by Comp_Date3 ASC"
                    Command.CommandType = CommandType.Text 'sets the type of the sql
                    Command.Connection = Connection 'sets the connection of our sql command to MyDB
                    Command.CommandText = SQL 'sets the statement that executes at the data source to our string
                    Connection.Open() 'opens the connction
                    DR = Command.ExecuteReader 'sends the command text to the connection and builds tthe SqlDataReader
                    While DR.Read() 'Check whether the SqlDataReader has 1 or more rows

                        segment = New List(Of String)()
                        segment.Add(DR("Comp_Date3"))
                        segment.Add(DR("TOTAL3"))
                        retval.Add(segment)


                    End While
                    Connection.Close() 'closes the connection
                    DR.Close() 'closes the reader
                    Return retval
                ElseIf gt = "Lot Acceptance" And tp = "Custom" Then
                    Dim segment As New List(Of String)()
                    SQL = "Select ISNULL((CAST(s3.TOTAL AS Decimal(10,2)))/(CAST( s3.TOTAL2 AS Decimal(10,2)))*100, 0) AS TOTAL3,dateadd(DAY,0, datediff(day,0, s3.Comp_Date)) AS Comp_Date3  FROM (Select s1.TOTAL, s2.TOTAL2, s2.Comp_Date From ((Select COUNT(InspectionJobSummaryYearly.id) AS TOTAL, dateadd(DAY,0, datediff(day,0, Inspection_Finished)) AS Comp_Date from dbo.InspectionJobSummaryYearly " & WhereString & " AND Technical_PassFail=1 Group by dateadd(DAY, 0, DateDiff(Day, 0, Inspection_Finished))) AS s1 RIGHT JOIN (Select COUNT(InspectionJobSummaryYearly.id) AS TOTAL2, dateadd(DAY,0, datediff(day,0, Inspection_Finished)) AS Comp_Date from dbo.InspectionJobSummaryYearly " & WhereString & " Group by dateadd(DAY, 0, DateDiff(Day, 0, Inspection_Finished))) AS s2 ON s1.Comp_Date=s2.Comp_Date)) AS s3 ORDER by Comp_Date3 ASC"
                    Command.CommandType = CommandType.Text 'sets the type of the sql
                    Command.Connection = Connection 'sets the connection of our sql command to MyDB
                    Command.CommandText = SQL 'sets the statement that executes at the data source to our string
                    Connection.Open() 'opens the connction
                    DR = Command.ExecuteReader 'sends the command text to the connection and builds tthe SqlDataReader
                    While DR.Read() 'Check whether the SqlDataReader has 1 or more rows

                        segment = New List(Of String)()
                        segment.Add(DR("Comp_Date3"))
                        segment.Add(DR("TOTAL3"))
                        retval.Add(segment)


                    End While
                    Connection.Close() 'closes the connection
                    DR.Close() 'closes the reader
                    Return retval


                End If
            ElseIf fac = "Overall" Then
                Dim WhereString2 As String = "WHERE CID IS NOT NULL"
                If AuditType = "ALL" Then
                    WhereString2 = WhereString2 & " AND InspectionType != 'ROLL'"
                ElseIf AuditType = "FINAL AUDIT" Then
                    WhereString2 = WhereString2 & " AND InspectionType = 'EOL'"
                ElseIf AuditType = "IN LINE" Then
                    WhereString2 = WhereString2 & " AND InspectionType = 'IL'"
                Else
                    WhereString2 = WhereString2 & " AND InspectionType = '" & AuditType & "'"
                End If
                If DataNo <> "ALL" Then
                    WhereString2 = WhereString2 & " AND DataNo = '" & DataNo & "'"
                End If
                If WorkOrder <> "ALL" Then
                    WhereString2 = WhereString2 & " AND JobNumber = '" & WorkOrder & "'"
                End If
                Dim WhereString As String = "WHERE CID IS NOT NULL AND Inspection_Finished BETWEEN '" & Fromdate & "' AND '" & Todate & "'"
                If AuditType = "ALL" Then
                    WhereString = WhereString & " AND InspectionType != 'ROLL'"
                ElseIf AuditType = "FINAL AUDIT" Then
                    WhereString = WhereString & " AND InspectionType = 'EOL'"
                ElseIf AuditType = "IN LINE" Then
                    WhereString = WhereString & " AND InspectionType = 'IL'"
                Else
                    WhereString = WhereString & " AND InspectionType = '" & AuditType & "'"
                End If
                If DataNo <> "ALL" Then
                    WhereString = WhereString & " AND DataNo = '" & DataNo & "'"
                End If
                If WorkOrder <> "ALL" Then
                    WhereString = WhereString & " AND JobNumber = '" & WorkOrder & "'"
                End If
                Dim SQL As String = ""

                If gt = "No. of Defects" And tp = "Past Year" Then
                    Dim segment As New List(Of String)()
                    SQL = "Select ISNULL(SUM(MajorsCount+MinorsCount+RepairsCount+ScrapCount), 0) AS TOTAL, dateadd(DAY,0, datediff(day,0, Inspection_Finished)) AS Comp_Date from dbo.InspectionJobSummaryYearly " & WhereString2 & "Group by dateadd(DAY, 0, DateDiff(Day, 0, Inspection_Finished))  ORDER BY Comp_Date ASC"
                    Command.CommandType = CommandType.Text 'sets the type of the sql
                    Command.Connection = Connection 'sets the connection of our sql command to MyDB
                    Command.CommandText = SQL 'sets the statement that executes at the data source to our string
                    Connection.Open() 'opens the connction
                    DR = Command.ExecuteReader 'sends the command text to the connection and builds tthe SqlDataReader
                    While DR.Read() 'Check whether the SqlDataReader has 1 or more rows

                        segment = New List(Of String)()
                        segment.Add(DR("Comp_Date"))
                        segment.Add(DR("TOTAL"))
                        retval.Add(segment)


                    End While
                    Connection.Close() 'closes the connection
                    DR.Close() 'closes the reader
                    Return retval
                ElseIf gt = "No. of Defects" And tp = "Yesterday" Then
                    Dim segment As New List(Of String)()
                    SQL = "Select ISNULL(SUM(MajorsCount+MinorsCount+RepairsCount+ScrapCount), 0) AS TOTAL, dateadd(DAY,0, datediff(day,0, Inspection_Finished)) AS Comp_Date from dbo.InspectionJobSummaryYearly " & WhereString2 & " And Inspection_Finished >= dateadd(day,datediff(day,1,GETDATE()),0) AND Inspection_Finished < dateadd(day,datediff(day,0,GETDATE()),0) Group by dateadd(DAY, 0, DateDiff(Day, 0, Inspection_Finished))  ORDER BY Comp_Date ASC"
                    Command.CommandType = CommandType.Text 'sets the type of the sql
                    Command.Connection = Connection 'sets the connection of our sql command to MyDB
                    Command.CommandText = SQL 'sets the statement that executes at the data source to our string
                    Connection.Open() 'opens the connction
                    DR = Command.ExecuteReader 'sends the command text to the connection and builds tthe SqlDataReader
                    While DR.Read() 'Check whether the SqlDataReader has 1 or more rows

                        segment = New List(Of String)()
                        segment.Add(DR("Comp_Date"))
                        segment.Add(DR("TOTAL"))
                        retval.Add(segment)


                    End While
                    Connection.Close() 'closes the connection
                    DR.Close() 'closes the reader
                    Return retval
                ElseIf gt = "No. of Rejects" And tp = "Yesterday" Then
                    Dim segment As New List(Of String)()
                    SQL = "Select ISNULL(SUM(MajorsCount+RepairsCount+ScrapCount), 0) AS TOTAL, dateadd(DAY,0, datediff(day,0, Inspection_Finished)) AS Comp_Date from dbo.InspectionJobSummaryYearly " & WhereString2 & " And Inspection_Finished >= dateadd(day,datediff(day,1,GETDATE()),0) AND Inspection_Finished < dateadd(day,datediff(day,0,GETDATE()),0) Group by dateadd(DAY, 0, DateDiff(Day, 0, Inspection_Finished))  ORDER BY Comp_Date ASC"
                    Command.CommandType = CommandType.Text 'sets the type of the sql
                    Command.Connection = Connection 'sets the connection of our sql command to MyDB
                    Command.CommandText = SQL 'sets the statement that executes at the data source to our string
                    Connection.Open() 'opens the connction
                    DR = Command.ExecuteReader 'sends the command text to the connection and builds tthe SqlDataReader
                    While DR.Read() 'Check whether the SqlDataReader has 1 or more rows

                        segment = New List(Of String)()
                        segment.Add(DR("Comp_Date"))
                        segment.Add(DR("TOTAL"))
                        retval.Add(segment)


                    End While
                    Connection.Close() 'closes the connection
                    DR.Close() 'closes the reader
                    Return retval
                ElseIf gt = "No. of Inspections" And tp = "Yesterday" Then
                    Dim segment As New List(Of String)()
                    SQL = "Select COUNT(InspectionJobSummaryYearly.id) AS TOTAL, dateadd(DAY,0, datediff(day,0, Inspection_Finished)) AS Comp_Date from dbo.InspectionJobSummaryYearly " & WhereString2 & " And Inspection_Finished >= dateadd(day,datediff(day,1,GETDATE()),0) AND Inspection_Finished < dateadd(day,datediff(day,0,GETDATE()),0) Group by dateadd(DAY, 0, DateDiff(Day, 0, Inspection_Finished))  ORDER BY Comp_Date ASC"
                    Command.CommandType = CommandType.Text 'sets the type of the sql
                    Command.Connection = Connection 'sets the connection of our sql command to MyDB
                    Command.CommandText = SQL 'sets the statement that executes at the data source to our string
                    Connection.Open() 'opens the connction
                    DR = Command.ExecuteReader 'sends the command text to the connection and builds tthe SqlDataReader
                    While DR.Read() 'Check whether the SqlDataReader has 1 or more rows

                        segment = New List(Of String)()
                        segment.Add(DR("Comp_Date"))
                        segment.Add(DR("TOTAL"))
                        retval.Add(segment)


                    End While
                    Connection.Close() 'closes the connection
                    DR.Close() 'closes the reader
                    Return retval
                ElseIf gt = "No. of Rejected Lots" And tp = "Yesterday" Then
                    Dim segment As New List(Of String)()
                    SQL = "Select COUNT(InspectionJobSummaryYearly.id) AS TOTAL, dateadd(DAY,0, datediff(day,0, Inspection_Finished)) AS Comp_Date from dbo.InspectionJobSummaryYearly " & WhereString2 & " AND Technical_PassFail=0 And Inspection_Finished >= dateadd(day,datediff(day,1,GETDATE()),0) AND Inspection_Finished < dateadd(day,datediff(day,0,GETDATE()),0) Group by dateadd(DAY, 0, DateDiff(Day, 0, Inspection_Finished))  ORDER BY Comp_Date ASC"
                    Command.CommandType = CommandType.Text 'sets the type of the sql
                    Command.Connection = Connection 'sets the connection of our sql command to MyDB
                    Command.CommandText = SQL 'sets the statement that executes at the data source to our string
                    Connection.Open() 'opens the connction
                    DR = Command.ExecuteReader 'sends the command text to the connection and builds tthe SqlDataReader
                    While DR.Read() 'Check whether the SqlDataReader has 1 or more rows

                        segment = New List(Of String)()
                        segment.Add(DR("Comp_Date"))
                        segment.Add(DR("TOTAL"))
                        retval.Add(segment)


                    End While
                    Connection.Close() 'closes the connection
                    DR.Close() 'closes the reader
                    Return retval
                ElseIf gt = "DHU" And tp = "Yesterday" Then
                    Dim segment As New List(Of String)()
                    SQL = "Select ISNULL((CAST(SUM(MajorsCount+MinorsCount+RepairsCount+ScrapCount) AS Decimal(10,2))/CAST(SUM(SampleSize) AS Decimal(10,2)))*100, 0) AS TOTAL, dateadd(DAY,0, datediff(day,0, Inspection_Finished)) AS Comp_Date from dbo.InspectionJobSummaryYearly " & WhereString2 & " And Inspection_Finished >= dateadd(day,datediff(day,1,GETDATE()),0) AND Inspection_Finished < dateadd(day,datediff(day,0,GETDATE()),0) Group by dateadd(DAY, 0, DateDiff(Day, 0, Inspection_Finished))  ORDER BY Comp_Date ASC"
                    Command.CommandType = CommandType.Text 'sets the type of the sql
                    Command.Connection = Connection 'sets the connection of our sql command to MyDB
                    Command.CommandText = SQL 'sets the statement that executes at the data source to our string
                    Connection.Open() 'opens the connction
                    DR = Command.ExecuteReader 'sends the command text to the connection and builds tthe SqlDataReader
                    While DR.Read() 'Check whether the SqlDataReader has 1 or more rows

                        segment = New List(Of String)()
                        segment.Add(DR("Comp_Date"))
                        segment.Add(DR("TOTAL"))
                        retval.Add(segment)


                    End While
                    Connection.Close() 'closes the connection
                    DR.Close() 'closes the reader
                    Return retval
                ElseIf gt = "Reject Rate" And tp = "Yesterday" Then
                    Dim segment As New List(Of String)()
                    SQL = "Select ISNULL((CAST(SUM(MajorsCount+RepairsCount+ScrapCount) AS Decimal(10,2))/CAST(SUM(SampleSize) AS Decimal(10,2)))*100, 0) AS TOTAL, dateadd(DAY,0, datediff(day,0, Inspection_Finished)) AS Comp_Date from dbo.InspectionJobSummaryYearly " & WhereString2 & " And Inspection_Finished >= dateadd(day,datediff(day,1,GETDATE()),0) AND Inspection_Finished < dateadd(day,datediff(day,0,GETDATE()),0) Group by dateadd(DAY, 0, DateDiff(Day, 0, Inspection_Finished))  ORDER BY Comp_Date ASC"
                    Command.CommandType = CommandType.Text 'sets the type of the sql
                    Command.Connection = Connection 'sets the connection of our sql command to MyDB
                    Command.CommandText = SQL 'sets the statement that executes at the data source to our string
                    Connection.Open() 'opens the connction
                    DR = Command.ExecuteReader 'sends the command text to the connection and builds tthe SqlDataReader
                    While DR.Read() 'Check whether the SqlDataReader has 1 or more rows

                        segment = New List(Of String)()
                        segment.Add(DR("Comp_Date"))
                        segment.Add(DR("TOTAL"))
                        retval.Add(segment)


                    End While
                    Connection.Close() 'closes the connection
                    DR.Close() 'closes the reader
                    Return retval
                ElseIf gt = "Lot Acceptance" And tp = "Yesterday" Then
                    Dim segment As New List(Of String)()
                    SQL = "Select ISNULL((CAST(s3.TOTAL AS Decimal(10,2)))/(CAST( s3.TOTAL2 AS Decimal(10,2)))*100, 0) AS TOTAL3,dateadd(DAY,0, datediff(day,0, s3.Comp_Date)) AS Comp_Date3  FROM (Select s1.TOTAL, s2.TOTAL2, s2.Comp_Date From ((Select COUNT(InspectionJobSummaryYearly.id) AS TOTAL, dateadd(DAY,0, datediff(day,0, Inspection_Finished)) AS Comp_Date from dbo.InspectionJobSummaryYearly " & WhereString2 & " And Inspection_Finished >= dateadd(day,datediff(day,1,GETDATE()),0) AND Inspection_Finished < dateadd(day,datediff(day,0,GETDATE()),0) AND Technical_PassFail=1 Group by dateadd(DAY, 0, DateDiff(Day, 0, Inspection_Finished))) AS s1 RIGHT JOIN (Select COUNT(InspectionJobSummaryYearly.id) AS TOTAL2, dateadd(DAY,0, datediff(day,0, Inspection_Finished)) AS Comp_Date from dbo.InspectionJobSummaryYearly " & WhereString2 & " And Inspection_Finished >= dateadd(day,datediff(day,1,GETDATE()),0) AND Inspection_Finished < dateadd(day,datediff(day,0,GETDATE()),0) Group by dateadd(DAY, 0, DateDiff(Day, 0, Inspection_Finished))) AS s2 ON s1.Comp_Date=s2.Comp_Date)) AS s3 ORDER by Comp_Date3 ASC"
                    Command.CommandType = CommandType.Text 'sets the type of the sql
                    Command.Connection = Connection 'sets the connection of our sql command to MyDB
                    Command.CommandText = SQL 'sets the statement that executes at the data source to our string
                    Connection.Open() 'opens the connction
                    DR = Command.ExecuteReader 'sends the command text to the connection and builds tthe SqlDataReader
                    While DR.Read() 'Check whether the SqlDataReader has 1 or more rows

                        segment = New List(Of String)()
                        segment.Add(DR("Comp_Date3"))
                        segment.Add(DR("TOTAL3"))
                        retval.Add(segment)


                    End While
                    Connection.Close() 'closes the connection
                    DR.Close() 'closes the reader
                    Return retval

                ElseIf gt = "No. of Defects" And tp = "Past 30 Days" Then
                    Dim segment As New List(Of String)()
                    SQL = "Select ISNULL(SUM(MajorsCount+MinorsCount+RepairsCount+ScrapCount), 0) AS TOTAL, dateadd(DAY,0, datediff(day,0, Inspection_Finished)) AS Comp_Date from dbo.InspectionJobSummaryYearly " & WhereString2 & " And Inspection_Finished >= DATEADD(month,-1,GETDATE()) Group by dateadd(DAY, 0, DateDiff(Day, 0, Inspection_Finished))  ORDER BY Comp_Date ASC"
                    Command.CommandType = CommandType.Text 'sets the type of the sql
                    Command.Connection = Connection 'sets the connection of our sql command to MyDB
                    Command.CommandText = SQL 'sets the statement that executes at the data source to our string
                    Connection.Open() 'opens the connction
                    DR = Command.ExecuteReader 'sends the command text to the connection and builds tthe SqlDataReader
                    While DR.Read() 'Check whether the SqlDataReader has 1 or more rows

                        segment = New List(Of String)()
                        segment.Add(DR("Comp_Date"))
                        segment.Add(DR("TOTAL"))
                        retval.Add(segment)


                    End While
                    Connection.Close() 'closes the connection
                    DR.Close() 'closes the reader
                    Return retval
                ElseIf gt = "No. of Defects" And tp = "Custom" Then
                    Dim segment As New List(Of String)()
                    SQL = "Select ISNULL(SUM(MajorsCount+MinorsCount+RepairsCount+ScrapCount), 0) AS TOTAL, dateadd(DAY,0, datediff(day,0, Inspection_Finished)) AS Comp_Date from dbo.InspectionJobSummaryYearly " & WhereString & " Group by dateadd(DAY, 0, DateDiff(Day, 0, Inspection_Finished))  ORDER BY Comp_Date ASC"
                    Command.CommandType = CommandType.Text 'sets the type of the sql
                    Command.Connection = Connection 'sets the connection of our sql command to MyDB
                    Command.CommandText = SQL 'sets the statement that executes at the data source to our string
                    Connection.Open() 'opens the connction
                    DR = Command.ExecuteReader 'sends the command text to the connection and builds tthe SqlDataReader
                    While DR.Read() 'Check whether the SqlDataReader has 1 or more rows

                        segment = New List(Of String)()
                        segment.Add(DR("Comp_Date"))
                        segment.Add(DR("TOTAL"))
                        retval.Add(segment)


                    End While
                    Connection.Close() 'closes the connection
                    DR.Close() 'closes the reader
                    Return retval
                ElseIf gt = "No. of Rejects" And tp = "Past Year" Then
                    Dim segment As New List(Of String)()
                    SQL = "Select ISNULL(SUM(MajorsCount+RepairsCount+ScrapCount), 0) AS TOTAL, dateadd(DAY,0, datediff(day,0, Inspection_Finished)) AS Comp_Date from dbo.InspectionJobSummaryYearly " & WhereString2 & " Group by dateadd(DAY, 0, DateDiff(Day, 0, Inspection_Finished))  ORDER BY Comp_Date ASC"
                    Command.CommandType = CommandType.Text 'sets the type of the sql
                    Command.Connection = Connection 'sets the connection of our sql command to MyDB
                    Command.CommandText = SQL 'sets the statement that executes at the data source to our string
                    Connection.Open() 'opens the connction
                    DR = Command.ExecuteReader 'sends the command text to the connection and builds tthe SqlDataReader
                    While DR.Read() 'Check whether the SqlDataReader has 1 or more rows

                        segment = New List(Of String)()
                        segment.Add(DR("Comp_Date"))
                        segment.Add(DR("TOTAL"))
                        retval.Add(segment)


                    End While
                    Connection.Close() 'closes the connection
                    DR.Close() 'closes the reader
                    Return retval
                ElseIf gt = "No. of Rejects" And tp = "Past 30 Days" Then
                    Dim segment As New List(Of String)()
                    SQL = "Select ISNULL(SUM(MajorsCount+RepairsCount+ScrapCount), 0) AS TOTAL, dateadd(DAY,0, datediff(day,0, Inspection_Finished)) AS Comp_Date from dbo.InspectionJobSummaryYearly " & WhereString2 & " And Inspection_Finished >= DATEADD(month,-1,GETDATE()) Group by dateadd(DAY, 0, DateDiff(Day, 0, Inspection_Finished))  ORDER BY Comp_Date ASC"
                    Command.CommandType = CommandType.Text 'sets the type of the sql
                    Command.Connection = Connection 'sets the connection of our sql command to MyDB
                    Command.CommandText = SQL 'sets the statement that executes at the data source to our string
                    Connection.Open() 'opens the connction
                    DR = Command.ExecuteReader 'sends the command text to the connection and builds tthe SqlDataReader
                    While DR.Read() 'Check whether the SqlDataReader has 1 or more rows

                        segment = New List(Of String)()
                        segment.Add(DR("Comp_Date"))
                        segment.Add(DR("TOTAL"))
                        retval.Add(segment)


                    End While
                    Connection.Close() 'closes the connection
                    DR.Close() 'closes the reader
                    Return retval
                ElseIf gt = "No. of Rejects" And tp = "Custom" Then
                    Dim segment As New List(Of String)()
                    SQL = "Select ISNULL(SUM(MajorsCount+RepairsCount+ScrapCount), 0) AS TOTAL, dateadd(DAY,0, datediff(day,0, Inspection_Finished)) AS Comp_Date from dbo.InspectionJobSummaryYearly " & WhereString & "Group by dateadd(DAY, 0, DateDiff(Day, 0, Inspection_Finished))  ORDER BY Comp_Date ASC"
                    Command.CommandType = CommandType.Text 'sets the type of the sql
                    Command.Connection = Connection 'sets the connection of our sql command to MyDB
                    Command.CommandText = SQL 'sets the statement that executes at the data source to our string
                    Connection.Open() 'opens the connction
                    DR = Command.ExecuteReader 'sends the command text to the connection and builds tthe SqlDataReader
                    While DR.Read() 'Check whether the SqlDataReader has 1 or more rows

                        segment = New List(Of String)()
                        segment.Add(DR("Comp_Date"))
                        segment.Add(DR("TOTAL"))
                        retval.Add(segment)


                    End While
                    Connection.Close() 'closes the connection
                    DR.Close() 'closes the reader
                    Return retval
                ElseIf gt = "No. of Inspections" And tp = "Past Year" Then
                    Dim segment As New List(Of String)()
                    SQL = "Select COUNT(InspectionJobSummaryYearly.id) AS TOTAL, dateadd(DAY,0, datediff(day,0, Inspection_Finished)) AS Comp_Date from dbo.InspectionJobSummaryYearly " & WhereString2 & " Group by dateadd(DAY, 0, DateDiff(Day, 0, Inspection_Finished))  ORDER BY Comp_Date ASC"
                    Command.CommandType = CommandType.Text 'sets the type of the sql
                    Command.Connection = Connection 'sets the connection of our sql command to MyDB
                    Command.CommandText = SQL 'sets the statement that executes at the data source to our string
                    Connection.Open() 'opens the connction
                    DR = Command.ExecuteReader 'sends the command text to the connection and builds tthe SqlDataReader
                    While DR.Read() 'Check whether the SqlDataReader has 1 or more rows

                        segment = New List(Of String)()
                        segment.Add(DR("Comp_Date"))
                        segment.Add(DR("TOTAL"))
                        retval.Add(segment)


                    End While
                    Connection.Close() 'closes the connection
                    DR.Close() 'closes the reader
                    Return retval
                ElseIf gt = "No. of Inspections" And tp = "Past 30 Days" Then
                    Dim segment As New List(Of String)()
                    SQL = "Select COUNT(InspectionJobSummaryYearly.id) AS TOTAL, dateadd(DAY,0, datediff(day,0, Inspection_Finished)) AS Comp_Date from dbo.InspectionJobSummaryYearly " & WhereString2 & " And Inspection_Finished >= DATEADD(month,-1,GETDATE()) Group by dateadd(DAY, 0, DateDiff(Day, 0, Inspection_Finished))  ORDER BY Comp_Date ASC"
                    Command.CommandType = CommandType.Text 'sets the type of the sql
                    Command.Connection = Connection 'sets the connection of our sql command to MyDB
                    Command.CommandText = SQL 'sets the statement that executes at the data source to our string
                    Connection.Open() 'opens the connction
                    DR = Command.ExecuteReader 'sends the command text to the connection and builds tthe SqlDataReader
                    While DR.Read() 'Check whether the SqlDataReader has 1 or more rows

                        segment = New List(Of String)()
                        segment.Add(DR("Comp_Date"))
                        segment.Add(DR("TOTAL"))
                        retval.Add(segment)


                    End While
                    Connection.Close() 'closes the connection
                    DR.Close() 'closes the reader
                    Return retval
                ElseIf gt = "No. of Inspections" And tp = "Custom" Then
                    Dim segment As New List(Of String)()
                    SQL = "Select COUNT(InspectionJobSummaryYearly.id) AS TOTAL, dateadd(DAY,0, datediff(day,0, Inspection_Finished)) AS Comp_Date from dbo.InspectionJobSummaryYearly " & WhereString & " Group by dateadd(DAY, 0, DateDiff(Day, 0, Inspection_Finished))  ORDER BY Comp_Date ASC"
                    Command.CommandType = CommandType.Text 'sets the type of the sql
                    Command.Connection = Connection 'sets the connection of our sql command to MyDB
                    Command.CommandText = SQL 'sets the statement that executes at the data source to our string
                    Connection.Open() 'opens the connction
                    DR = Command.ExecuteReader 'sends the command text to the connection and builds tthe SqlDataReader
                    While DR.Read() 'Check whether the SqlDataReader has 1 or more rows

                        segment = New List(Of String)()
                        segment.Add(DR("Comp_Date"))
                        segment.Add(DR("TOTAL"))
                        retval.Add(segment)


                    End While
                    Connection.Close() 'closes the connection
                    DR.Close() 'closes the reader
                    Return retval
                ElseIf gt = "No. of Rejected Lots" And tp = "Past Year" Then
                    Dim segment As New List(Of String)()
                    SQL = "Select COUNT(InspectionJobSummaryYearly.id) AS TOTAL, dateadd(DAY,0, datediff(day,0, Inspection_Finished)) AS Comp_Date from dbo.InspectionJobSummaryYearly " & WhereString2 & " AND Technical_PassFail=0 Group by dateadd(DAY, 0, DateDiff(Day, 0, Inspection_Finished))  ORDER BY Comp_Date ASC"
                    Command.CommandType = CommandType.Text 'sets the type of the sql
                    Command.Connection = Connection 'sets the connection of our sql command to MyDB
                    Command.CommandText = SQL 'sets the statement that executes at the data source to our string
                    Connection.Open() 'opens the connction
                    DR = Command.ExecuteReader 'sends the command text to the connection and builds tthe SqlDataReader
                    While DR.Read() 'Check whether the SqlDataReader has 1 or more rows

                        segment = New List(Of String)()
                        segment.Add(DR("Comp_Date"))
                        segment.Add(DR("TOTAL"))
                        retval.Add(segment)


                    End While
                    Connection.Close() 'closes the connection
                    DR.Close() 'closes the reader
                    Return retval
                ElseIf gt = "No. of Rejected Lots" And tp = "Past 30 Days" Then
                    Dim segment As New List(Of String)()
                    SQL = "Select COUNT(InspectionJobSummaryYearly.id) AS TOTAL, dateadd(DAY,0, datediff(day,0, Inspection_Finished)) AS Comp_Date from dbo.InspectionJobSummaryYearly " & WhereString2 & " And Inspection_Finished >= DATEADD(month,-1,GETDATE()) AND Technical_PassFail=0 Group by dateadd(DAY, 0, DateDiff(Day, 0, Inspection_Finished))  ORDER BY Comp_Date ASC"
                    Command.CommandType = CommandType.Text 'sets the type of the sql
                    Command.Connection = Connection 'sets the connection of our sql command to MyDB
                    Command.CommandText = SQL 'sets the statement that executes at the data source to our string
                    Connection.Open() 'opens the connction
                    DR = Command.ExecuteReader 'sends the command text to the connection and builds tthe SqlDataReader
                    While DR.Read() 'Check whether the SqlDataReader has 1 or more rows

                        segment = New List(Of String)()
                        segment.Add(DR("Comp_Date"))
                        segment.Add(DR("TOTAL"))
                        retval.Add(segment)


                    End While
                    Connection.Close() 'closes the connection
                    DR.Close() 'closes the reader
                    Return retval
                ElseIf gt = "No. of Rejected Lots" And tp = "Custom" Then
                    Dim segment As New List(Of String)()
                    SQL = "Select COUNT(InspectionJobSummaryYearly.id) AS TOTAL, dateadd(DAY,0, datediff(day,0, Inspection_Finished)) AS Comp_Date from dbo.InspectionJobSummaryYearly " & WhereString & " AND Technical_PassFail=0 Group by dateadd(DAY, 0, DateDiff(Day, 0, Inspection_Finished))  ORDER BY Comp_Date ASC"
                    Command.CommandType = CommandType.Text 'sets the type of the sql
                    Command.Connection = Connection 'sets the connection of our sql command to MyDB
                    Command.CommandText = SQL 'sets the statement that executes at the data source to our string
                    Connection.Open() 'opens the connction
                    DR = Command.ExecuteReader 'sends the command text to the connection and builds tthe SqlDataReader
                    While DR.Read() 'Check whether the SqlDataReader has 1 or more rows

                        segment = New List(Of String)()
                        segment.Add(DR("Comp_Date"))
                        segment.Add(DR("TOTAL"))
                        retval.Add(segment)


                    End While
                    Connection.Close() 'closes the connection
                    DR.Close() 'closes the reader
                    Return retval
                ElseIf gt = "DHU" And tp = "Past Year" Then
                    Dim segment As New List(Of String)()
                    SQL = "Select ISNULL((CAST(SUM(MajorsCount+MinorsCount+RepairsCount+ScrapCount) AS Decimal(10,2))/CAST(SUM(SampleSize) AS Decimal(10,2)))*100, 0) AS TOTAL, dateadd(DAY,0, datediff(day,0, Inspection_Finished)) AS Comp_Date from dbo.InspectionJobSummaryYearly " & WhereString2 & " Group by dateadd(DAY, 0, DateDiff(Day, 0, Inspection_Finished))  ORDER BY Comp_Date ASC"
                    Command.CommandType = CommandType.Text 'sets the type of the sql
                    Command.Connection = Connection 'sets the connection of our sql command to MyDB
                    Command.CommandText = SQL 'sets the statement that executes at the data source to our string
                    Connection.Open() 'opens the connction
                    DR = Command.ExecuteReader 'sends the command text to the connection and builds tthe SqlDataReader
                    While DR.Read() 'Check whether the SqlDataReader has 1 or more rows

                        segment = New List(Of String)()
                        segment.Add(DR("Comp_Date"))
                        segment.Add(DR("TOTAL"))
                        retval.Add(segment)


                    End While
                    Connection.Close() 'closes the connection
                    DR.Close() 'closes the reader
                    Return retval

                ElseIf gt = "DHU" And tp = "Past 30 Days" Then
                    Dim segment As New List(Of String)()
                    SQL = "Select ISNULL((CAST(SUM(MajorsCount+MinorsCount+RepairsCount+ScrapCount) AS Decimal(10,2))/CAST(SUM(SampleSize) AS Decimal(10,2)))*100, 0) AS TOTAL , dateadd(DAY,0, datediff(day,0, Inspection_Finished)) AS Comp_Date from dbo.InspectionJobSummaryYearly " & WhereString2 & " And Inspection_Finished >= DATEADD(month,-1,GETDATE()) Group by dateadd(DAY, 0, DateDiff(Day, 0, Inspection_Finished))  ORDER BY Comp_Date ASC"
                    Command.CommandType = CommandType.Text 'sets the type of the sql
                    Command.Connection = Connection 'sets the connection of our sql command to MyDB
                    Command.CommandText = SQL 'sets the statement that executes at the data source to our string
                    Connection.Open() 'opens the connction
                    DR = Command.ExecuteReader 'sends the command text to the connection and builds tthe SqlDataReader
                    While DR.Read() 'Check whether the SqlDataReader has 1 or more rows

                        segment = New List(Of String)()
                        segment.Add(DR("Comp_Date"))
                        segment.Add(DR("TOTAL"))
                        retval.Add(segment)


                    End While
                    Connection.Close() 'closes the connection
                    DR.Close() 'closes the reader
                    Return retval
                ElseIf gt = "DHU" And tp = "Custom" Then
                    Dim segment As New List(Of String)()
                    SQL = "Select ISNULL((CAST(SUM(MajorsCount+MinorsCount+RepairsCount+ScrapCount) AS Decimal(10,2))/CAST(SUM(SampleSize) AS Decimal(10,2)))*100, 0) AS TOTAL, dateadd(DAY,0, datediff(day,0, Inspection_Finished)) AS Comp_Date from dbo.InspectionJobSummaryYearly " & WhereString & " Group by dateadd(DAY, 0, DateDiff(Day, 0, Inspection_Finished))  ORDER BY Comp_Date ASC"
                    Command.CommandType = CommandType.Text 'sets the type of the sql
                    Command.Connection = Connection 'sets the connection of our sql command to MyDB
                    Command.CommandText = SQL 'sets the statement that executes at the data source to our string
                    Connection.Open() 'opens the connction
                    DR = Command.ExecuteReader 'sends the command text to the connection and builds tthe SqlDataReader
                    While DR.Read() 'Check whether the SqlDataReader has 1 or more rows

                        segment = New List(Of String)()
                        segment.Add(DR("Comp_Date"))
                        segment.Add(DR("TOTAL"))
                        retval.Add(segment)


                    End While
                    Connection.Close() 'closes the connection
                    DR.Close() 'closes the reader
                    Return retval

                ElseIf gt = "Reject Rate" And tp = "Past Year" Then
                    Dim segment As New List(Of String)()
                    SQL = "Select ISNULL((CAST(SUM(MajorsCount+RepairsCount+ScrapCount) AS Decimal(10,2))/CAST(SUM(SampleSize) AS Decimal(10,2)))*100, 0) AS TOTAL, dateadd(DAY,0, datediff(day,0, Inspection_Finished)) AS Comp_Date from dbo.InspectionJobSummaryYearly " & WhereString2 & " Group by dateadd(DAY, 0, DateDiff(Day, 0, Inspection_Finished))  ORDER BY Comp_Date ASC"
                    Command.CommandType = CommandType.Text 'sets the type of the sql
                    Command.Connection = Connection 'sets the connection of our sql command to MyDB
                    Command.CommandText = SQL 'sets the statement that executes at the data source to our string
                    Connection.Open() 'opens the connction
                    DR = Command.ExecuteReader 'sends the command text to the connection and builds tthe SqlDataReader
                    While DR.Read() 'Check whether the SqlDataReader has 1 or more rows

                        segment = New List(Of String)()
                        segment.Add(DR("Comp_Date"))
                        segment.Add(DR("TOTAL"))
                        retval.Add(segment)


                    End While
                    Connection.Close() 'closes the connection
                    DR.Close() 'closes the reader
                    Return retval

                ElseIf gt = "Reject Rate" And tp = "Past 30 Days" Then
                    Dim segment As New List(Of String)()
                    SQL = "Select ISNULL((CAST(SUM(MajorsCount+RepairsCount+ScrapCount) AS Decimal(10,2))/CAST(SUM(SampleSize) AS Decimal(10,2)))*100, 0) AS TOTAL , dateadd(DAY,0, datediff(day,0, Inspection_Finished)) AS Comp_Date from dbo.InspectionJobSummaryYearly " & WhereString2 & " And Inspection_Finished >= DATEADD(month,-1,GETDATE()) Group by dateadd(DAY, 0, DateDiff(Day, 0, Inspection_Finished))  ORDER BY Comp_Date ASC"
                    Command.CommandType = CommandType.Text 'sets the type of the sql
                    Command.Connection = Connection 'sets the connection of our sql command to MyDB
                    Command.CommandText = SQL 'sets the statement that executes at the data source to our string
                    Connection.Open() 'opens the connction
                    DR = Command.ExecuteReader 'sends the command text to the connection and builds tthe SqlDataReader
                    While DR.Read() 'Check whether the SqlDataReader has 1 or more rows

                        segment = New List(Of String)()
                        segment.Add(DR("Comp_Date"))
                        segment.Add(DR("TOTAL"))
                        retval.Add(segment)


                    End While
                    Connection.Close() 'closes the connection
                    DR.Close() 'closes the reader
                    Return retval
                ElseIf gt = "Reject Rate" And tp = "Custom" Then
                    Dim segment As New List(Of String)()
                    SQL = "Select ISNULL((CAST(SUM(MajorsCount+RepairsCount+ScrapCount) AS Decimal(10,2))/CAST(SUM(SampleSize) AS Decimal(10,2)))*100, 0) AS TOTAL, dateadd(DAY,0, datediff(day,0, Inspection_Finished)) AS Comp_Date from dbo.InspectionJobSummaryYearly " & WhereString & " Group by dateadd(DAY, 0, DateDiff(Day, 0, Inspection_Finished))  ORDER BY Comp_Date ASC"
                    Command.CommandType = CommandType.Text 'sets the type of the sql
                    Command.Connection = Connection 'sets the connection of our sql command to MyDB
                    Command.CommandText = SQL 'sets the statement that executes at the data source to our string
                    Connection.Open() 'opens the connction
                    DR = Command.ExecuteReader 'sends the command text to the connection and builds tthe SqlDataReader
                    While DR.Read() 'Check whether the SqlDataReader has 1 or more rows

                        segment = New List(Of String)()
                        segment.Add(DR("Comp_Date"))
                        segment.Add(DR("TOTAL"))
                        retval.Add(segment)


                    End While
                    Connection.Close() 'closes the connection
                    DR.Close() 'closes the reader
                    Return retval

                ElseIf gt = "Lot Acceptance" And tp = "Past Year" Then
                    Dim segment As New List(Of String)()
                    SQL = "Select ISNULL((CAST(s3.TOTAL AS Decimal(10,2)))/(CAST( s3.TOTAL2 AS Decimal(10,2)))*100, 0) AS TOTAL3,dateadd(DAY,0, datediff(day,0, s3.Comp_Date)) AS Comp_Date3  FROM (Select s1.TOTAL, s2.TOTAL2, s2.Comp_Date From ((Select COUNT(InspectionJobSummaryYearly.id) AS TOTAL, dateadd(DAY,0, datediff(day,0, Inspection_Finished)) AS Comp_Date from dbo.InspectionJobSummaryYearly " & WhereString2 & " AND Technical_PassFail=1 Group by dateadd(DAY, 0, DateDiff(Day, 0, Inspection_Finished))) AS s1 RIGHT JOIN (Select COUNT(InspectionJobSummaryYearly.id) AS TOTAL2, dateadd(DAY,0, datediff(day,0, Inspection_Finished)) AS Comp_Date from dbo.InspectionJobSummaryYearly " & WhereString2 & " Group by dateadd(DAY, 0, DateDiff(Day, 0, Inspection_Finished))) AS s2 ON s1.Comp_Date=s2.Comp_Date)) AS s3 ORDER by Comp_Date3 ASC"
                    Command.CommandType = CommandType.Text 'sets the type of the sql
                    Command.Connection = Connection 'sets the connection of our sql command to MyDB
                    Command.CommandText = SQL 'sets the statement that executes at the data source to our string
                    Connection.Open() 'opens the connction
                    DR = Command.ExecuteReader 'sends the command text to the connection and builds tthe SqlDataReader
                    While DR.Read() 'Check whether the SqlDataReader has 1 or more rows

                        segment = New List(Of String)()
                        segment.Add(DR("Comp_Date3"))
                        segment.Add(DR("TOTAL3"))
                        retval.Add(segment)


                    End While
                    Connection.Close() 'closes the connection
                    DR.Close() 'closes the reader
                    Return retval

                ElseIf gt = "Lot Acceptance" And tp = "Past 30 Days" Then
                    Dim segment As New List(Of String)()
                    SQL = "Select ISNULL((CAST(s3.TOTAL AS Decimal(10,2)))/(CAST( s3.TOTAL2 AS Decimal(10,2)))*100, 0) AS TOTAL3,dateadd(DAY,0, datediff(day,0, s3.Comp_Date)) AS Comp_Date3  FROM (Select s1.TOTAL, s2.TOTAL2, s2.Comp_Date From ((Select COUNT(InspectionJobSummaryYearly.id) AS TOTAL, dateadd(DAY,0, datediff(day,0, Inspection_Finished)) AS Comp_Date from dbo.InspectionJobSummaryYearly " & WhereString2 & " And Inspection_Finished >= DATEADD(month,-1,GETDATE()) AND Technical_PassFail=1 Group by dateadd(DAY, 0, DateDiff(Day, 0, Inspection_Finished))) AS s1 RIGHT JOIN (Select COUNT(InspectionJobSummaryYearly.id) AS TOTAL2, dateadd(DAY,0, datediff(day,0, Inspection_Finished)) AS Comp_Date from dbo.InspectionJobSummaryYearly " & WhereString2 & " And Inspection_Finished >= DATEADD(month,-1,GETDATE()) Group by dateadd(DAY, 0, DateDiff(Day, 0, Inspection_Finished))) AS s2 ON s1.Comp_Date=s2.Comp_Date)) AS s3 ORDER by Comp_Date3 ASC"
                    Command.CommandType = CommandType.Text 'sets the type of the sql
                    Command.Connection = Connection 'sets the connection of our sql command to MyDB
                    Command.CommandText = SQL 'sets the statement that executes at the data source to our string
                    Connection.Open() 'opens the connction
                    DR = Command.ExecuteReader 'sends the command text to the connection and builds tthe SqlDataReader
                    While DR.Read() 'Check whether the SqlDataReader has 1 or more rows

                        segment = New List(Of String)()
                        segment.Add(DR("Comp_Date3"))
                        segment.Add(DR("TOTAL3"))
                        retval.Add(segment)


                    End While
                    Connection.Close() 'closes the connection
                    DR.Close() 'closes the reader
                    Return retval
                ElseIf gt = "Lot Acceptance" And tp = "Custom" Then
                    Dim segment As New List(Of String)()
                    SQL = "Select ISNULL((CAST(s3.TOTAL AS Decimal(10,2)))/(CAST( s3.TOTAL2 AS Decimal(10,2)))*100, 0) AS TOTAL3,dateadd(DAY,0, datediff(day,0, s3.Comp_Date)) AS Comp_Date3  FROM (Select s1.TOTAL, s2.TOTAL2, s2.Comp_Date From ((Select COUNT(InspectionJobSummaryYearly.id) AS TOTAL, dateadd(DAY,0, datediff(day,0, Inspection_Finished)) AS Comp_Date from dbo.InspectionJobSummaryYearly " & WhereString & " AND Technical_PassFail=1 Group by dateadd(DAY, 0, DateDiff(Day, 0, Inspection_Finished))) AS s1 RIGHT JOIN (Select COUNT(InspectionJobSummaryYearly.id) AS TOTAL2, dateadd(DAY,0, datediff(day,0, Inspection_Finished)) AS Comp_Date from dbo.InspectionJobSummaryYearly " & WhereString & " Group by dateadd(DAY, 0, DateDiff(Day, 0, Inspection_Finished))) AS s2 ON s1.Comp_Date=s2.Comp_Date)) AS s3 ORDER by Comp_Date3 ASC"
                    Command.CommandType = CommandType.Text 'sets the type of the sql
                    Command.Connection = Connection 'sets the connection of our sql command to MyDB
                    Command.CommandText = SQL 'sets the statement that executes at the data source to our string
                    Connection.Open() 'opens the connction
                    DR = Command.ExecuteReader 'sends the command text to the connection and builds tthe SqlDataReader
                    While DR.Read() 'Check whether the SqlDataReader has 1 or more rows

                        segment = New List(Of String)()
                        segment.Add(DR("Comp_Date3"))
                        segment.Add(DR("TOTAL3"))
                        retval.Add(segment)


                    End While
                    Connection.Close() 'closes the connection
                    DR.Close() 'closes the reader
                    Return retval


                End If
            ElseIf fac = "Interiors" Then
                Dim WhereString2 As String = "WHERE (CID = 112 OR CID = 111)"
                If AuditType = "ALL" Then
                    WhereString2 = WhereString2 & " AND InspectionType != 'ROLL'"
                ElseIf AuditType = "FINAL AUDIT" Then
                    WhereString2 = WhereString2 & " AND InspectionType = 'EOL'"
                ElseIf AuditType = "IN LINE" Then
                    WhereString2 = WhereString2 & " AND InspectionType = 'IL'"
                Else
                    WhereString2 = WhereString2 & " AND InspectionType = '" & AuditType & "'"
                End If
                If DataNo <> "ALL" Then
                    WhereString2 = WhereString2 & " AND DataNo = '" & DataNo & "'"
                End If
                If WorkOrder <> "ALL" Then
                    WhereString2 = WhereString2 & " AND JobNumber = '" & WorkOrder & "'"
                End If
                Dim WhereString As String = "WHERE (CID = 112 OR CID = 111) AND Inspection_Finished BETWEEN '" & Fromdate & "' AND '" & Todate & "'"
                If AuditType = "ALL" Then
                    WhereString = WhereString & " AND InspectionType != 'ROLL'"
                ElseIf AuditType = "FINAL AUDIT" Then
                    WhereString = WhereString & " AND InspectionType = 'EOL'"
                ElseIf AuditType = "IN LINE" Then
                    WhereString = WhereString & " AND InspectionType = 'IL'"
                Else
                    WhereString = WhereString & " AND InspectionType = '" & AuditType & "'"
                End If
                If DataNo <> "ALL" Then
                    WhereString = WhereString & " AND DataNo = '" & DataNo & "'"
                End If
                If WorkOrder <> "ALL" Then
                    WhereString = WhereString & " AND JobNumber = '" & WorkOrder & "'"
                End If
                Dim SQL As String = ""

                If gt = "No. of Defects" And tp = "Past Year" Then
                    Dim segment As New List(Of String)()
                    SQL = "Select ISNULL(SUM(MajorsCount+MinorsCount+RepairsCount+ScrapCount), 0) AS TOTAL, dateadd(DAY,0, datediff(day,0, Inspection_Finished)) AS Comp_Date from dbo.InspectionJobSummaryYearly " & WhereString2 & "Group by dateadd(DAY, 0, DateDiff(Day, 0, Inspection_Finished))  ORDER BY Comp_Date ASC"
                    Command.CommandType = CommandType.Text 'sets the type of the sql
                    Command.Connection = Connection 'sets the connection of our sql command to MyDB
                    Command.CommandText = SQL 'sets the statement that executes at the data source to our string
                    Connection.Open() 'opens the connction
                    DR = Command.ExecuteReader 'sends the command text to the connection and builds tthe SqlDataReader
                    While DR.Read() 'Check whether the SqlDataReader has 1 or more rows

                        segment = New List(Of String)()
                        segment.Add(DR("Comp_Date"))
                        segment.Add(DR("TOTAL"))
                        retval.Add(segment)


                    End While
                    Connection.Close() 'closes the connection
                    DR.Close() 'closes the reader
                    Return retval
                ElseIf gt = "No. of Defects" And tp = "Yesterday" Then
                    Dim segment As New List(Of String)()
                    SQL = "Select ISNULL(SUM(MajorsCount+MinorsCount+RepairsCount+ScrapCount), 0) AS TOTAL, dateadd(DAY,0, datediff(day,0, Inspection_Finished)) AS Comp_Date from dbo.InspectionJobSummaryYearly " & WhereString2 & " And Inspection_Finished >= dateadd(day,datediff(day,1,GETDATE()),0) AND Inspection_Finished < dateadd(day,datediff(day,0,GETDATE()),0) Group by dateadd(DAY, 0, DateDiff(Day, 0, Inspection_Finished))  ORDER BY Comp_Date ASC"
                    Command.CommandType = CommandType.Text 'sets the type of the sql
                    Command.Connection = Connection 'sets the connection of our sql command to MyDB
                    Command.CommandText = SQL 'sets the statement that executes at the data source to our string
                    Connection.Open() 'opens the connction
                    DR = Command.ExecuteReader 'sends the command text to the connection and builds tthe SqlDataReader
                    While DR.Read() 'Check whether the SqlDataReader has 1 or more rows

                        segment = New List(Of String)()
                        segment.Add(DR("Comp_Date"))
                        segment.Add(DR("TOTAL"))
                        retval.Add(segment)


                    End While
                    Connection.Close() 'closes the connection
                    DR.Close() 'closes the reader
                    Return retval
                ElseIf gt = "No. of Rejects" And tp = "Yesterday" Then
                    Dim segment As New List(Of String)()
                    SQL = "Select ISNULL(SUM(MajorsCount+RepairsCount+ScrapCount), 0) AS TOTAL, dateadd(DAY,0, datediff(day,0, Inspection_Finished)) AS Comp_Date from dbo.InspectionJobSummaryYearly " & WhereString2 & " And Inspection_Finished >= dateadd(day,datediff(day,1,GETDATE()),0) AND Inspection_Finished < dateadd(day,datediff(day,0,GETDATE()),0) Group by dateadd(DAY, 0, DateDiff(Day, 0, Inspection_Finished))  ORDER BY Comp_Date ASC"
                    Command.CommandType = CommandType.Text 'sets the type of the sql
                    Command.Connection = Connection 'sets the connection of our sql command to MyDB
                    Command.CommandText = SQL 'sets the statement that executes at the data source to our string
                    Connection.Open() 'opens the connction
                    DR = Command.ExecuteReader 'sends the command text to the connection and builds tthe SqlDataReader
                    While DR.Read() 'Check whether the SqlDataReader has 1 or more rows

                        segment = New List(Of String)()
                        segment.Add(DR("Comp_Date"))
                        segment.Add(DR("TOTAL"))
                        retval.Add(segment)


                    End While
                    Connection.Close() 'closes the connection
                    DR.Close() 'closes the reader
                    Return retval
                ElseIf gt = "No. of Inspections" And tp = "Yesterday" Then
                    Dim segment As New List(Of String)()
                    SQL = "Select COUNT(InspectionJobSummaryYearly.id) AS TOTAL, dateadd(DAY,0, datediff(day,0, Inspection_Finished)) AS Comp_Date from dbo.InspectionJobSummaryYearly " & WhereString2 & " And Inspection_Finished >= dateadd(day,datediff(day,1,GETDATE()),0) AND Inspection_Finished < dateadd(day,datediff(day,0,GETDATE()),0) Group by dateadd(DAY, 0, DateDiff(Day, 0, Inspection_Finished))  ORDER BY Comp_Date ASC"
                    Command.CommandType = CommandType.Text 'sets the type of the sql
                    Command.Connection = Connection 'sets the connection of our sql command to MyDB
                    Command.CommandText = SQL 'sets the statement that executes at the data source to our string
                    Connection.Open() 'opens the connction
                    DR = Command.ExecuteReader 'sends the command text to the connection and builds tthe SqlDataReader
                    While DR.Read() 'Check whether the SqlDataReader has 1 or more rows

                        segment = New List(Of String)()
                        segment.Add(DR("Comp_Date"))
                        segment.Add(DR("TOTAL"))
                        retval.Add(segment)


                    End While
                    Connection.Close() 'closes the connection
                    DR.Close() 'closes the reader
                    Return retval
                ElseIf gt = "No. of Rejected Lots" And tp = "Yesterday" Then
                    Dim segment As New List(Of String)()
                    SQL = "Select COUNT(InspectionJobSummaryYearly.id) AS TOTAL, dateadd(DAY,0, datediff(day,0, Inspection_Finished)) AS Comp_Date from dbo.InspectionJobSummaryYearly " & WhereString2 & " AND Technical_PassFail=0 And Inspection_Finished >= dateadd(day,datediff(day,1,GETDATE()),0) AND Inspection_Finished < dateadd(day,datediff(day,0,GETDATE()),0) Group by dateadd(DAY, 0, DateDiff(Day, 0, Inspection_Finished))  ORDER BY Comp_Date ASC"
                    Command.CommandType = CommandType.Text 'sets the type of the sql
                    Command.Connection = Connection 'sets the connection of our sql command to MyDB
                    Command.CommandText = SQL 'sets the statement that executes at the data source to our string
                    Connection.Open() 'opens the connction
                    DR = Command.ExecuteReader 'sends the command text to the connection and builds tthe SqlDataReader
                    While DR.Read() 'Check whether the SqlDataReader has 1 or more rows

                        segment = New List(Of String)()
                        segment.Add(DR("Comp_Date"))
                        segment.Add(DR("TOTAL"))
                        retval.Add(segment)


                    End While
                    Connection.Close() 'closes the connection
                    DR.Close() 'closes the reader
                    Return retval
                ElseIf gt = "DHU" And tp = "Yesterday" Then
                    Dim segment As New List(Of String)()
                    SQL = "Select ISNULL((CAST(SUM(MajorsCount+MinorsCount+RepairsCount+ScrapCount) AS Decimal(10,2))/CAST(SUM(SampleSize) AS Decimal(10,2)))*100, 0) AS TOTAL, dateadd(DAY,0, datediff(day,0, Inspection_Finished)) AS Comp_Date from dbo.InspectionJobSummaryYearly " & WhereString2 & " And Inspection_Finished >= dateadd(day,datediff(day,1,GETDATE()),0) AND Inspection_Finished < dateadd(day,datediff(day,0,GETDATE()),0) Group by dateadd(DAY, 0, DateDiff(Day, 0, Inspection_Finished))  ORDER BY Comp_Date ASC"
                    Command.CommandType = CommandType.Text 'sets the type of the sql
                    Command.Connection = Connection 'sets the connection of our sql command to MyDB
                    Command.CommandText = SQL 'sets the statement that executes at the data source to our string
                    Connection.Open() 'opens the connction
                    DR = Command.ExecuteReader 'sends the command text to the connection and builds tthe SqlDataReader
                    While DR.Read() 'Check whether the SqlDataReader has 1 or more rows

                        segment = New List(Of String)()
                        segment.Add(DR("Comp_Date"))
                        segment.Add(DR("TOTAL"))
                        retval.Add(segment)


                    End While
                    Connection.Close() 'closes the connection
                    DR.Close() 'closes the reader
                    Return retval
                ElseIf gt = "Reject Rate" And tp = "Yesterday" Then
                    Dim segment As New List(Of String)()
                    SQL = "Select ISNULL((CAST(SUM(MajorsCount+RepairsCount+ScrapCount) AS Decimal(10,2))/CAST(SUM(SampleSize) AS Decimal(10,2)))*100, 0) AS TOTAL, dateadd(DAY,0, datediff(day,0, Inspection_Finished)) AS Comp_Date from dbo.InspectionJobSummaryYearly " & WhereString2 & " And Inspection_Finished >= dateadd(day,datediff(day,1,GETDATE()),0) AND Inspection_Finished < dateadd(day,datediff(day,0,GETDATE()),0) Group by dateadd(DAY, 0, DateDiff(Day, 0, Inspection_Finished))  ORDER BY Comp_Date ASC"
                    Command.CommandType = CommandType.Text 'sets the type of the sql
                    Command.Connection = Connection 'sets the connection of our sql command to MyDB
                    Command.CommandText = SQL 'sets the statement that executes at the data source to our string
                    Connection.Open() 'opens the connction
                    DR = Command.ExecuteReader 'sends the command text to the connection and builds tthe SqlDataReader
                    While DR.Read() 'Check whether the SqlDataReader has 1 or more rows

                        segment = New List(Of String)()
                        segment.Add(DR("Comp_Date"))
                        segment.Add(DR("TOTAL"))
                        retval.Add(segment)


                    End While
                    Connection.Close() 'closes the connection
                    DR.Close() 'closes the reader
                    Return retval
                ElseIf gt = "Lot Acceptance" And tp = "Yesterday" Then
                    Dim segment As New List(Of String)()
                    SQL = "Select ISNULL((CAST(s3.TOTAL AS Decimal(10,2)))/(CAST( s3.TOTAL2 AS Decimal(10,2)))*100, 0) AS TOTAL3,dateadd(DAY,0, datediff(day,0, s3.Comp_Date)) AS Comp_Date3  FROM (Select s1.TOTAL, s2.TOTAL2, s2.Comp_Date From ((Select COUNT(InspectionJobSummaryYearly.id) AS TOTAL, dateadd(DAY,0, datediff(day,0, Inspection_Finished)) AS Comp_Date from dbo.InspectionJobSummaryYearly " & WhereString2 & " And Inspection_Finished >= dateadd(day,datediff(day,1,GETDATE()),0) AND Inspection_Finished < dateadd(day,datediff(day,0,GETDATE()),0) AND Technical_PassFail=1 Group by dateadd(DAY, 0, DateDiff(Day, 0, Inspection_Finished))) AS s1 RIGHT JOIN (Select COUNT(InspectionJobSummaryYearly.id) AS TOTAL2, dateadd(DAY,0, datediff(day,0, Inspection_Finished)) AS Comp_Date from dbo.InspectionJobSummaryYearly " & WhereString2 & " And Inspection_Finished >= dateadd(day,datediff(day,1,GETDATE()),0) AND Inspection_Finished < dateadd(day,datediff(day,0,GETDATE()),0) Group by dateadd(DAY, 0, DateDiff(Day, 0, Inspection_Finished))) AS s2 ON s1.Comp_Date=s2.Comp_Date)) AS s3 ORDER by Comp_Date3 ASC"
                    Command.CommandType = CommandType.Text 'sets the type of the sql
                    Command.Connection = Connection 'sets the connection of our sql command to MyDB
                    Command.CommandText = SQL 'sets the statement that executes at the data source to our string
                    Connection.Open() 'opens the connction
                    DR = Command.ExecuteReader 'sends the command text to the connection and builds tthe SqlDataReader
                    While DR.Read() 'Check whether the SqlDataReader has 1 or more rows

                        segment = New List(Of String)()
                        segment.Add(DR("Comp_Date3"))
                        segment.Add(DR("TOTAL3"))
                        retval.Add(segment)


                    End While
                    Connection.Close() 'closes the connection
                    DR.Close() 'closes the reader
                    Return retval

                ElseIf gt = "No. of Defects" And tp = "Past 30 Days" Then
                    Dim segment As New List(Of String)()
                    SQL = "Select ISNULL(SUM(MajorsCount+MinorsCount+RepairsCount+ScrapCount), 0) AS TOTAL, dateadd(DAY,0, datediff(day,0, Inspection_Finished)) AS Comp_Date from dbo.InspectionJobSummaryYearly " & WhereString2 & " And Inspection_Finished >= DATEADD(month,-1,GETDATE()) Group by dateadd(DAY, 0, DateDiff(Day, 0, Inspection_Finished))  ORDER BY Comp_Date ASC"
                    Command.CommandType = CommandType.Text 'sets the type of the sql
                    Command.Connection = Connection 'sets the connection of our sql command to MyDB
                    Command.CommandText = SQL 'sets the statement that executes at the data source to our string
                    Connection.Open() 'opens the connction
                    DR = Command.ExecuteReader 'sends the command text to the connection and builds tthe SqlDataReader
                    While DR.Read() 'Check whether the SqlDataReader has 1 or more rows

                        segment = New List(Of String)()
                        segment.Add(DR("Comp_Date"))
                        segment.Add(DR("TOTAL"))
                        retval.Add(segment)


                    End While
                    Connection.Close() 'closes the connection
                    DR.Close() 'closes the reader
                    Return retval
                ElseIf gt = "No. of Defects" And tp = "Custom" Then
                    Dim segment As New List(Of String)()
                    SQL = "Select ISNULL(SUM(MajorsCount+MinorsCount+RepairsCount+ScrapCount), 0) AS TOTAL, dateadd(DAY,0, datediff(day,0, Inspection_Finished)) AS Comp_Date from dbo.InspectionJobSummaryYearly " & WhereString & " Group by dateadd(DAY, 0, DateDiff(Day, 0, Inspection_Finished))  ORDER BY Comp_Date ASC"
                    Command.CommandType = CommandType.Text 'sets the type of the sql
                    Command.Connection = Connection 'sets the connection of our sql command to MyDB
                    Command.CommandText = SQL 'sets the statement that executes at the data source to our string
                    Connection.Open() 'opens the connction
                    DR = Command.ExecuteReader 'sends the command text to the connection and builds tthe SqlDataReader
                    While DR.Read() 'Check whether the SqlDataReader has 1 or more rows

                        segment = New List(Of String)()
                        segment.Add(DR("Comp_Date"))
                        segment.Add(DR("TOTAL"))
                        retval.Add(segment)


                    End While
                    Connection.Close() 'closes the connection
                    DR.Close() 'closes the reader
                    Return retval
                ElseIf gt = "No. of Rejects" And tp = "Past Year" Then
                    Dim segment As New List(Of String)()
                    SQL = "Select ISNULL(SUM(MajorsCount+RepairsCount+ScrapCount), 0) AS TOTAL, dateadd(DAY,0, datediff(day,0, Inspection_Finished)) AS Comp_Date from dbo.InspectionJobSummaryYearly " & WhereString2 & " Group by dateadd(DAY, 0, DateDiff(Day, 0, Inspection_Finished))  ORDER BY Comp_Date ASC"
                    Command.CommandType = CommandType.Text 'sets the type of the sql
                    Command.Connection = Connection 'sets the connection of our sql command to MyDB
                    Command.CommandText = SQL 'sets the statement that executes at the data source to our string
                    Connection.Open() 'opens the connction
                    DR = Command.ExecuteReader 'sends the command text to the connection and builds tthe SqlDataReader
                    While DR.Read() 'Check whether the SqlDataReader has 1 or more rows

                        segment = New List(Of String)()
                        segment.Add(DR("Comp_Date"))
                        segment.Add(DR("TOTAL"))
                        retval.Add(segment)


                    End While
                    Connection.Close() 'closes the connection
                    DR.Close() 'closes the reader
                    Return retval
                ElseIf gt = "No. of Rejects" And tp = "Past 30 Days" Then
                    Dim segment As New List(Of String)()
                    SQL = "Select ISNULL(SUM(MajorsCount+RepairsCount+ScrapCount), 0) AS TOTAL, dateadd(DAY,0, datediff(day,0, Inspection_Finished)) AS Comp_Date from dbo.InspectionJobSummaryYearly " & WhereString2 & " And Inspection_Finished >= DATEADD(month,-1,GETDATE()) Group by dateadd(DAY, 0, DateDiff(Day, 0, Inspection_Finished))  ORDER BY Comp_Date ASC"
                    Command.CommandType = CommandType.Text 'sets the type of the sql
                    Command.Connection = Connection 'sets the connection of our sql command to MyDB
                    Command.CommandText = SQL 'sets the statement that executes at the data source to our string
                    Connection.Open() 'opens the connction
                    DR = Command.ExecuteReader 'sends the command text to the connection and builds tthe SqlDataReader
                    While DR.Read() 'Check whether the SqlDataReader has 1 or more rows

                        segment = New List(Of String)()
                        segment.Add(DR("Comp_Date"))
                        segment.Add(DR("TOTAL"))
                        retval.Add(segment)


                    End While
                    Connection.Close() 'closes the connection
                    DR.Close() 'closes the reader
                    Return retval
                ElseIf gt = "No. of Rejects" And tp = "Custom" Then
                    Dim segment As New List(Of String)()
                    SQL = "Select ISNULL(SUM(MajorsCount+RepairsCount+ScrapCount), 0) AS TOTAL, dateadd(DAY,0, datediff(day,0, Inspection_Finished)) AS Comp_Date from dbo.InspectionJobSummaryYearly " & WhereString & "Group by dateadd(DAY, 0, DateDiff(Day, 0, Inspection_Finished))  ORDER BY Comp_Date ASC"
                    Command.CommandType = CommandType.Text 'sets the type of the sql
                    Command.Connection = Connection 'sets the connection of our sql command to MyDB
                    Command.CommandText = SQL 'sets the statement that executes at the data source to our string
                    Connection.Open() 'opens the connction
                    DR = Command.ExecuteReader 'sends the command text to the connection and builds tthe SqlDataReader
                    While DR.Read() 'Check whether the SqlDataReader has 1 or more rows

                        segment = New List(Of String)()
                        segment.Add(DR("Comp_Date"))
                        segment.Add(DR("TOTAL"))
                        retval.Add(segment)


                    End While
                    Connection.Close() 'closes the connection
                    DR.Close() 'closes the reader
                    Return retval
                ElseIf gt = "No. of Inspections" And tp = "Past Year" Then
                    Dim segment As New List(Of String)()
                    SQL = "Select COUNT(InspectionJobSummaryYearly.id) AS TOTAL, dateadd(DAY,0, datediff(day,0, Inspection_Finished)) AS Comp_Date from dbo.InspectionJobSummaryYearly " & WhereString2 & " Group by dateadd(DAY, 0, DateDiff(Day, 0, Inspection_Finished))  ORDER BY Comp_Date ASC"
                    Command.CommandType = CommandType.Text 'sets the type of the sql
                    Command.Connection = Connection 'sets the connection of our sql command to MyDB
                    Command.CommandText = SQL 'sets the statement that executes at the data source to our string
                    Connection.Open() 'opens the connction
                    DR = Command.ExecuteReader 'sends the command text to the connection and builds tthe SqlDataReader
                    While DR.Read() 'Check whether the SqlDataReader has 1 or more rows

                        segment = New List(Of String)()
                        segment.Add(DR("Comp_Date"))
                        segment.Add(DR("TOTAL"))
                        retval.Add(segment)


                    End While
                    Connection.Close() 'closes the connection
                    DR.Close() 'closes the reader
                    Return retval
                ElseIf gt = "No. of Inspections" And tp = "Past 30 Days" Then
                    Dim segment As New List(Of String)()
                    SQL = "Select COUNT(InspectionJobSummaryYearly.id) AS TOTAL, dateadd(DAY,0, datediff(day,0, Inspection_Finished)) AS Comp_Date from dbo.InspectionJobSummaryYearly " & WhereString2 & " And Inspection_Finished >= DATEADD(month,-1,GETDATE()) Group by dateadd(DAY, 0, DateDiff(Day, 0, Inspection_Finished))  ORDER BY Comp_Date ASC"
                    Command.CommandType = CommandType.Text 'sets the type of the sql
                    Command.Connection = Connection 'sets the connection of our sql command to MyDB
                    Command.CommandText = SQL 'sets the statement that executes at the data source to our string
                    Connection.Open() 'opens the connction
                    DR = Command.ExecuteReader 'sends the command text to the connection and builds tthe SqlDataReader
                    While DR.Read() 'Check whether the SqlDataReader has 1 or more rows

                        segment = New List(Of String)()
                        segment.Add(DR("Comp_Date"))
                        segment.Add(DR("TOTAL"))
                        retval.Add(segment)


                    End While
                    Connection.Close() 'closes the connection
                    DR.Close() 'closes the reader
                    Return retval
                ElseIf gt = "No. of Inspections" And tp = "Custom" Then
                    Dim segment As New List(Of String)()
                    SQL = "Select COUNT(InspectionJobSummaryYearly.id) AS TOTAL, dateadd(DAY,0, datediff(day,0, Inspection_Finished)) AS Comp_Date from dbo.InspectionJobSummaryYearly " & WhereString & " Group by dateadd(DAY, 0, DateDiff(Day, 0, Inspection_Finished))  ORDER BY Comp_Date ASC"
                    Command.CommandType = CommandType.Text 'sets the type of the sql
                    Command.Connection = Connection 'sets the connection of our sql command to MyDB
                    Command.CommandText = SQL 'sets the statement that executes at the data source to our string
                    Connection.Open() 'opens the connction
                    DR = Command.ExecuteReader 'sends the command text to the connection and builds tthe SqlDataReader
                    While DR.Read() 'Check whether the SqlDataReader has 1 or more rows

                        segment = New List(Of String)()
                        segment.Add(DR("Comp_Date"))
                        segment.Add(DR("TOTAL"))
                        retval.Add(segment)


                    End While
                    Connection.Close() 'closes the connection
                    DR.Close() 'closes the reader
                    Return retval
                ElseIf gt = "No. of Rejected Lots" And tp = "Past Year" Then
                    Dim segment As New List(Of String)()
                    SQL = "Select COUNT(InspectionJobSummaryYearly.id) AS TOTAL, dateadd(DAY,0, datediff(day,0, Inspection_Finished)) AS Comp_Date from dbo.InspectionJobSummaryYearly " & WhereString2 & " AND Technical_PassFail=0 Group by dateadd(DAY, 0, DateDiff(Day, 0, Inspection_Finished))  ORDER BY Comp_Date ASC"
                    Command.CommandType = CommandType.Text 'sets the type of the sql
                    Command.Connection = Connection 'sets the connection of our sql command to MyDB
                    Command.CommandText = SQL 'sets the statement that executes at the data source to our string
                    Connection.Open() 'opens the connction
                    DR = Command.ExecuteReader 'sends the command text to the connection and builds tthe SqlDataReader
                    While DR.Read() 'Check whether the SqlDataReader has 1 or more rows

                        segment = New List(Of String)()
                        segment.Add(DR("Comp_Date"))
                        segment.Add(DR("TOTAL"))
                        retval.Add(segment)


                    End While
                    Connection.Close() 'closes the connection
                    DR.Close() 'closes the reader
                    Return retval
                ElseIf gt = "No. of Rejected Lots" And tp = "Past 30 Days" Then
                    Dim segment As New List(Of String)()
                    SQL = "Select COUNT(InspectionJobSummaryYearly.id) AS TOTAL, dateadd(DAY,0, datediff(day,0, Inspection_Finished)) AS Comp_Date from dbo.InspectionJobSummaryYearly " & WhereString2 & " And Inspection_Finished >= DATEADD(month,-1,GETDATE()) AND Technical_PassFail=0 Group by dateadd(DAY, 0, DateDiff(Day, 0, Inspection_Finished))  ORDER BY Comp_Date ASC"
                    Command.CommandType = CommandType.Text 'sets the type of the sql
                    Command.Connection = Connection 'sets the connection of our sql command to MyDB
                    Command.CommandText = SQL 'sets the statement that executes at the data source to our string
                    Connection.Open() 'opens the connction
                    DR = Command.ExecuteReader 'sends the command text to the connection and builds tthe SqlDataReader
                    While DR.Read() 'Check whether the SqlDataReader has 1 or more rows

                        segment = New List(Of String)()
                        segment.Add(DR("Comp_Date"))
                        segment.Add(DR("TOTAL"))
                        retval.Add(segment)


                    End While
                    Connection.Close() 'closes the connection
                    DR.Close() 'closes the reader
                    Return retval
                ElseIf gt = "No. of Rejected Lots" And tp = "Custom" Then
                    Dim segment As New List(Of String)()
                    SQL = "Select COUNT(InspectionJobSummaryYearly.id) AS TOTAL, dateadd(DAY,0, datediff(day,0, Inspection_Finished)) AS Comp_Date from dbo.InspectionJobSummaryYearly " & WhereString & " AND Technical_PassFail=0 Group by dateadd(DAY, 0, DateDiff(Day, 0, Inspection_Finished))  ORDER BY Comp_Date ASC"
                    Command.CommandType = CommandType.Text 'sets the type of the sql
                    Command.Connection = Connection 'sets the connection of our sql command to MyDB
                    Command.CommandText = SQL 'sets the statement that executes at the data source to our string
                    Connection.Open() 'opens the connction
                    DR = Command.ExecuteReader 'sends the command text to the connection and builds tthe SqlDataReader
                    While DR.Read() 'Check whether the SqlDataReader has 1 or more rows

                        segment = New List(Of String)()
                        segment.Add(DR("Comp_Date"))
                        segment.Add(DR("TOTAL"))
                        retval.Add(segment)


                    End While
                    Connection.Close() 'closes the connection
                    DR.Close() 'closes the reader
                    Return retval
                ElseIf gt = "DHU" And tp = "Past Year" Then
                    Dim segment As New List(Of String)()
                    SQL = "Select ISNULL((CAST(SUM(MajorsCount+MinorsCount+RepairsCount+ScrapCount) AS Decimal(10,2))/CAST(SUM(SampleSize) AS Decimal(10,2)))*100, 0) AS TOTAL, dateadd(DAY,0, datediff(day,0, Inspection_Finished)) AS Comp_Date from dbo.InspectionJobSummaryYearly " & WhereString2 & " Group by dateadd(DAY, 0, DateDiff(Day, 0, Inspection_Finished))  ORDER BY Comp_Date ASC"
                    Command.CommandType = CommandType.Text 'sets the type of the sql
                    Command.Connection = Connection 'sets the connection of our sql command to MyDB
                    Command.CommandText = SQL 'sets the statement that executes at the data source to our string
                    Connection.Open() 'opens the connction
                    DR = Command.ExecuteReader 'sends the command text to the connection and builds tthe SqlDataReader
                    While DR.Read() 'Check whether the SqlDataReader has 1 or more rows

                        segment = New List(Of String)()
                        segment.Add(DR("Comp_Date"))
                        segment.Add(DR("TOTAL"))
                        retval.Add(segment)


                    End While
                    Connection.Close() 'closes the connection
                    DR.Close() 'closes the reader
                    Return retval

                ElseIf gt = "DHU" And tp = "Past 30 Days" Then
                    Dim segment As New List(Of String)()
                    SQL = "Select ISNULL((CAST(SUM(MajorsCount+MinorsCount+RepairsCount+ScrapCount) AS Decimal(10,2))/CAST(SUM(SampleSize) AS Decimal(10,2)))*100, 0) AS TOTAL , dateadd(DAY,0, datediff(day,0, Inspection_Finished)) AS Comp_Date from dbo.InspectionJobSummaryYearly " & WhereString2 & " And Inspection_Finished >= DATEADD(month,-1,GETDATE()) Group by dateadd(DAY, 0, DateDiff(Day, 0, Inspection_Finished))  ORDER BY Comp_Date ASC"
                    Command.CommandType = CommandType.Text 'sets the type of the sql
                    Command.Connection = Connection 'sets the connection of our sql command to MyDB
                    Command.CommandText = SQL 'sets the statement that executes at the data source to our string
                    Connection.Open() 'opens the connction
                    DR = Command.ExecuteReader 'sends the command text to the connection and builds tthe SqlDataReader
                    While DR.Read() 'Check whether the SqlDataReader has 1 or more rows

                        segment = New List(Of String)()
                        segment.Add(DR("Comp_Date"))
                        segment.Add(DR("TOTAL"))
                        retval.Add(segment)


                    End While
                    Connection.Close() 'closes the connection
                    DR.Close() 'closes the reader
                    Return retval
                ElseIf gt = "DHU" And tp = "Custom" Then
                    Dim segment As New List(Of String)()
                    SQL = "Select ISNULL((CAST(SUM(MajorsCount+MinorsCount+RepairsCount+ScrapCount) AS Decimal(10,2))/CAST(SUM(SampleSize) AS Decimal(10,2)))*100, 0) AS TOTAL, dateadd(DAY,0, datediff(day,0, Inspection_Finished)) AS Comp_Date from dbo.InspectionJobSummaryYearly " & WhereString & " Group by dateadd(DAY, 0, DateDiff(Day, 0, Inspection_Finished))  ORDER BY Comp_Date ASC"
                    Command.CommandType = CommandType.Text 'sets the type of the sql
                    Command.Connection = Connection 'sets the connection of our sql command to MyDB
                    Command.CommandText = SQL 'sets the statement that executes at the data source to our string
                    Connection.Open() 'opens the connction
                    DR = Command.ExecuteReader 'sends the command text to the connection and builds tthe SqlDataReader
                    While DR.Read() 'Check whether the SqlDataReader has 1 or more rows

                        segment = New List(Of String)()
                        segment.Add(DR("Comp_Date"))
                        segment.Add(DR("TOTAL"))
                        retval.Add(segment)


                    End While
                    Connection.Close() 'closes the connection
                    DR.Close() 'closes the reader
                    Return retval

                ElseIf gt = "Reject Rate" And tp = "Past Year" Then
                    Dim segment As New List(Of String)()
                    SQL = "Select ISNULL((CAST(SUM(MajorsCount+RepairsCount+ScrapCount) AS Decimal(10,2))/CAST(SUM(SampleSize) AS Decimal(10,2)))*100, 0) AS TOTAL, dateadd(DAY,0, datediff(day,0, Inspection_Finished)) AS Comp_Date from dbo.InspectionJobSummaryYearly " & WhereString2 & " Group by dateadd(DAY, 0, DateDiff(Day, 0, Inspection_Finished))  ORDER BY Comp_Date ASC"
                    Command.CommandType = CommandType.Text 'sets the type of the sql
                    Command.Connection = Connection 'sets the connection of our sql command to MyDB
                    Command.CommandText = SQL 'sets the statement that executes at the data source to our string
                    Connection.Open() 'opens the connction
                    DR = Command.ExecuteReader 'sends the command text to the connection and builds tthe SqlDataReader
                    While DR.Read() 'Check whether the SqlDataReader has 1 or more rows

                        segment = New List(Of String)()
                        segment.Add(DR("Comp_Date"))
                        segment.Add(DR("TOTAL"))
                        retval.Add(segment)


                    End While
                    Connection.Close() 'closes the connection
                    DR.Close() 'closes the reader
                    Return retval

                ElseIf gt = "Reject Rate" And tp = "Past 30 Days" Then
                    Dim segment As New List(Of String)()
                    SQL = "Select ISNULL((CAST(SUM(MajorsCount+RepairsCount+ScrapCount) AS Decimal(10,2))/CAST(SUM(SampleSize) AS Decimal(10,2)))*100, 0) AS TOTAL , dateadd(DAY,0, datediff(day,0, Inspection_Finished)) AS Comp_Date from dbo.InspectionJobSummaryYearly " & WhereString2 & " And Inspection_Finished >= DATEADD(month,-1,GETDATE()) Group by dateadd(DAY, 0, DateDiff(Day, 0, Inspection_Finished))  ORDER BY Comp_Date ASC"
                    Command.CommandType = CommandType.Text 'sets the type of the sql
                    Command.Connection = Connection 'sets the connection of our sql command to MyDB
                    Command.CommandText = SQL 'sets the statement that executes at the data source to our string
                    Connection.Open() 'opens the connction
                    DR = Command.ExecuteReader 'sends the command text to the connection and builds tthe SqlDataReader
                    While DR.Read() 'Check whether the SqlDataReader has 1 or more rows

                        segment = New List(Of String)()
                        segment.Add(DR("Comp_Date"))
                        segment.Add(DR("TOTAL"))
                        retval.Add(segment)


                    End While
                    Connection.Close() 'closes the connection
                    DR.Close() 'closes the reader
                    Return retval
                ElseIf gt = "Reject Rate" And tp = "Custom" Then
                    Dim segment As New List(Of String)()
                    SQL = "Select ISNULL((CAST(SUM(MajorsCount+RepairsCount+ScrapCount) AS Decimal(10,2))/CAST(SUM(SampleSize) AS Decimal(10,2)))*100, 0) AS TOTAL, dateadd(DAY,0, datediff(day,0, Inspection_Finished)) AS Comp_Date from dbo.InspectionJobSummaryYearly " & WhereString & " Group by dateadd(DAY, 0, DateDiff(Day, 0, Inspection_Finished))  ORDER BY Comp_Date ASC"
                    Command.CommandType = CommandType.Text 'sets the type of the sql
                    Command.Connection = Connection 'sets the connection of our sql command to MyDB
                    Command.CommandText = SQL 'sets the statement that executes at the data source to our string
                    Connection.Open() 'opens the connction
                    DR = Command.ExecuteReader 'sends the command text to the connection and builds tthe SqlDataReader
                    While DR.Read() 'Check whether the SqlDataReader has 1 or more rows

                        segment = New List(Of String)()
                        segment.Add(DR("Comp_Date"))
                        segment.Add(DR("TOTAL"))
                        retval.Add(segment)


                    End While
                    Connection.Close() 'closes the connection
                    DR.Close() 'closes the reader
                    Return retval

                ElseIf gt = "Lot Acceptance" And tp = "Past Year" Then
                    Dim segment As New List(Of String)()
                    SQL = "Select ISNULL((CAST(s3.TOTAL AS Decimal(10,2)))/(CAST( s3.TOTAL2 AS Decimal(10,2)))*100, 0) AS TOTAL3,dateadd(DAY,0, datediff(day,0, s3.Comp_Date)) AS Comp_Date3  FROM (Select s1.TOTAL, s2.TOTAL2, s2.Comp_Date From ((Select COUNT(InspectionJobSummaryYearly.id) AS TOTAL, dateadd(DAY,0, datediff(day,0, Inspection_Finished)) AS Comp_Date from dbo.InspectionJobSummaryYearly " & WhereString2 & " AND Technical_PassFail=1 Group by dateadd(DAY, 0, DateDiff(Day, 0, Inspection_Finished))) AS s1 RIGHT JOIN (Select COUNT(InspectionJobSummaryYearly.id) AS TOTAL2, dateadd(DAY,0, datediff(day,0, Inspection_Finished)) AS Comp_Date from dbo.InspectionJobSummaryYearly " & WhereString2 & " Group by dateadd(DAY, 0, DateDiff(Day, 0, Inspection_Finished))) AS s2 ON s1.Comp_Date=s2.Comp_Date)) AS s3 ORDER by Comp_Date3 ASC"
                    Command.CommandType = CommandType.Text 'sets the type of the sql
                    Command.Connection = Connection 'sets the connection of our sql command to MyDB
                    Command.CommandText = SQL 'sets the statement that executes at the data source to our string
                    Connection.Open() 'opens the connction
                    DR = Command.ExecuteReader 'sends the command text to the connection and builds tthe SqlDataReader
                    While DR.Read() 'Check whether the SqlDataReader has 1 or more rows

                        segment = New List(Of String)()
                        segment.Add(DR("Comp_Date3"))
                        segment.Add(DR("TOTAL3"))
                        retval.Add(segment)


                    End While
                    Connection.Close() 'closes the connection
                    DR.Close() 'closes the reader
                    Return retval

                ElseIf gt = "Lot Acceptance" And tp = "Past 30 Days" Then
                    Dim segment As New List(Of String)()
                    SQL = "Select ISNULL((CAST(s3.TOTAL AS Decimal(10,2)))/(CAST( s3.TOTAL2 AS Decimal(10,2)))*100, 0) AS TOTAL3,dateadd(DAY,0, datediff(day,0, s3.Comp_Date)) AS Comp_Date3  FROM (Select s1.TOTAL, s2.TOTAL2, s2.Comp_Date From ((Select COUNT(InspectionJobSummaryYearly.id) AS TOTAL, dateadd(DAY,0, datediff(day,0, Inspection_Finished)) AS Comp_Date from dbo.InspectionJobSummaryYearly " & WhereString2 & " And Inspection_Finished >= DATEADD(month,-1,GETDATE()) AND Technical_PassFail=1 Group by dateadd(DAY, 0, DateDiff(Day, 0, Inspection_Finished))) AS s1 RIGHT JOIN (Select COUNT(InspectionJobSummaryYearly.id) AS TOTAL2, dateadd(DAY,0, datediff(day,0, Inspection_Finished)) AS Comp_Date from dbo.InspectionJobSummaryYearly " & WhereString2 & " And Inspection_Finished >= DATEADD(month,-1,GETDATE()) Group by dateadd(DAY, 0, DateDiff(Day, 0, Inspection_Finished))) AS s2 ON s1.Comp_Date=s2.Comp_Date)) AS s3 ORDER by Comp_Date3 ASC"
                    Command.CommandType = CommandType.Text 'sets the type of the sql
                    Command.Connection = Connection 'sets the connection of our sql command to MyDB
                    Command.CommandText = SQL 'sets the statement that executes at the data source to our string
                    Connection.Open() 'opens the connction
                    DR = Command.ExecuteReader 'sends the command text to the connection and builds tthe SqlDataReader
                    While DR.Read() 'Check whether the SqlDataReader has 1 or more rows

                        segment = New List(Of String)()
                        segment.Add(DR("Comp_Date3"))
                        segment.Add(DR("TOTAL3"))
                        retval.Add(segment)


                    End While
                    Connection.Close() 'closes the connection
                    DR.Close() 'closes the reader
                    Return retval
                ElseIf gt = "Lot Acceptance" And tp = "Custom" Then
                    Dim segment As New List(Of String)()
                    SQL = "Select ISNULL((CAST(s3.TOTAL AS Decimal(10,2)))/(CAST( s3.TOTAL2 AS Decimal(10,2)))*100, 0) AS TOTAL3,dateadd(DAY,0, datediff(day,0, s3.Comp_Date)) AS Comp_Date3  FROM (Select s1.TOTAL, s2.TOTAL2, s2.Comp_Date From ((Select COUNT(InspectionJobSummaryYearly.id) AS TOTAL, dateadd(DAY,0, datediff(day,0, Inspection_Finished)) AS Comp_Date from dbo.InspectionJobSummaryYearly " & WhereString & " AND Technical_PassFail=1 Group by dateadd(DAY, 0, DateDiff(Day, 0, Inspection_Finished))) AS s1 RIGHT JOIN (Select COUNT(InspectionJobSummaryYearly.id) AS TOTAL2, dateadd(DAY,0, datediff(day,0, Inspection_Finished)) AS Comp_Date from dbo.InspectionJobSummaryYearly " & WhereString & " Group by dateadd(DAY, 0, DateDiff(Day, 0, Inspection_Finished))) AS s2 ON s1.Comp_Date=s2.Comp_Date)) AS s3 ORDER by Comp_Date3 ASC"
                    Command.CommandType = CommandType.Text 'sets the type of the sql
                    Command.Connection = Connection 'sets the connection of our sql command to MyDB
                    Command.CommandText = SQL 'sets the statement that executes at the data source to our string
                    Connection.Open() 'opens the connction
                    DR = Command.ExecuteReader 'sends the command text to the connection and builds tthe SqlDataReader
                    While DR.Read() 'Check whether the SqlDataReader has 1 or more rows

                        segment = New List(Of String)()
                        segment.Add(DR("Comp_Date3"))
                        segment.Add(DR("TOTAL3"))
                        retval.Add(segment)


                    End While
                    Connection.Close() 'closes the connection
                    DR.Close() 'closes the reader
                    Return retval


                End If
            End If

            Return retval
        End Function
        Public Function GetPrevWOGrid(ByVal WO As String) As List(Of String)
            Dim retval As New List(Of String)()
            Dim SQL As String = "Select InspectionJobSummary.id, InspectionJobSummary.JobNumber,TemplateName.LineType, InspectionJobSummary.SampleSize, InspectionJobSummary.TotalInspectedItems, InspectionJobSummary.AQL_Level, InspectionJobSummary.WorkRoom, InspectionJobSummary.WOQuantity From dbo.InspectionJobSummary INNER JOIN dbo.TemplateName On InspectionJobSummary.TemplateId=TemplateName.TemplateId WHERE JobNumber='" & WO & "' ORDER BY TemplateName.LineType DESC, InspectionJobSummary.id ASC"
            Command.CommandType = CommandType.Text 'sets the type of the sql
            Command.Connection = Connection 'sets the connection of our sql command to MyDB
            Command.CommandText = SQL 'sets the statement that executes at the data source to our string
            Connection.Open() 'opens the connction
            DR = Command.ExecuteReader 'sends the command text to the connection and builds tthe SqlDataReader
            If DR.HasRows = True Then 'Check whether the SqlDataReader has 1 or more rows
                '['ID', 'WO', 'AuditType', 'SampleSize', 'Inspected',
                While DR.Read()
                    Dim Mid As String = String.Format("ID: '{0}', WO: '{1}', LineType: '{2}', SampleSize: '{3}', Inspected: '{4}', AQLLevel: '{5}', WorkRoom: '{6}', WOQuantity: '{7}'", DR.GetInt32(0), DR.GetString(1), DR.GetString(2), DR.GetInt32(3), DR.GetInt32(4), DR.GetDecimal(5), DR.GetString(6), DR.GetInt32(7))
                    retval.Add("{" + Mid + "}")
                End While 'The default position of the SqlDataReader is before the first record. Therefore, you must call Read to begin accessing any data.  The reader has moved down to the first row.
                'Response.Write(DR("Cust_Name")) 'Writes a string to an HTTP response output stream.  In this case, just Cust_Name



            Else
                Return retval


            End If

            Connection.Close() 'closes the connection
            DR.Close() 'closes the reader
            Return retval
        End Function
        Public Function GetMachineLocation(ByVal Location As String) As List(Of String)
            Dim retval As New List(Of String)()
            Dim SQL As String = "SELECT MOP as LOC From dbo.MachineLocations WHERE LOCATION='" & Location & "'"
            Command.CommandType = CommandType.Text 'sets the type of the sql
            Command.Connection = Connection 'sets the connection of our sql command to MyDB
            Command.CommandText = SQL 'sets the statement that executes at the data source to our string
            Connection.Open() 'opens the connction
            DR = Command.ExecuteReader 'sends the command text to the connection and builds tthe SqlDataReader
            If DR.HasRows = True Then 'Check whether the SqlDataReader has 1 or more rows
                While DR.Read()
                    retval.Add(String.Format("{0}", CType(DR, IDataRecord)(0)))
                End While 'The default position of the SqlDataReader is before the first record. Therefore, you must call Read to begin accessing any data.  The reader has moved down to the first row.
                'Response.Write(DR("Cust_Name")) 'Writes a string to an HTTP response output stream.  In this case, just Cust_Name



            Else
                Return retval


            End If

            Connection.Close() 'closes the connection
            DR.Close() 'closes the reader
            Return retval
        End Function
        Public Function GetButtonLibrary() As List(Of SPCInspection.buttonlibrary)

            Dim ButtonValues As New List(Of SPCInspection.buttonlibrary)()
            Dim sqlstring As String

            sqlstring = "select * from dbo.ButtonLibrary WHERE Hide = 0 ORDER BY UPPER(Name) asc"

            ButtonValues = _DAOFactory.getbuttonlibrary(sqlstring, 3)

            Return ButtonValues

        End Function
        Public Function GetLibraryGrid() As List(Of SPCInspection.ButtonLibrarygrid)
            Dim selectObjects As New List(Of SPCInspection.ButtonLibrarygrid)
            Dim sql As String = "SELECT ButtonId, Name, DefectCode, Hide FROM ButtonLibrary Where Hide = 0 ORDER BY UPPER(Name) asc"
            Dim bmap As New BMappers(Of SPCInspection.ButtonLibrarygrid)
            'selectObjects = BMapper(Of SPCInspection.ButtonLibrarygrid).GetInspectObject(sql)
            selectObjects = bmap.GetInspectObject(sql)
            Return selectObjects
        End Function

        Public Function GetTemplateList() As List(Of selector2array)
            Dim sqlstring As String
            Dim selectValues As New List(Of selector2array)()

            sqlstring = "SELECT TemplateId, Name FROM TemplateName  WHERE Active = 1 ORDER BY TemplateId asc"

            selectValues = _DAOFactory.getSelector2(sqlstring, 3)

            Return selectValues

        End Function

        Public Function GetTemplateListByLocation(ByVal CID As Integer) As List(Of selector2array)

            Dim bmap_locm As New BMappers(Of core.SingleObject)
            Dim sel2_lst As New List(Of selector2array)
            Dim sel2_ret As New List(Of selector2array)
            If CID > 0 Then
                Dim loc_lst As List(Of core.SingleObject)
                Dim sql1 As String = "SELECT Abreviation AS Object1 FROM LocationMaster WHERE CID = 000" & CID.ToString()
                loc_lst = bmap_locm.GetAprMangObject(sql1)
                If loc_lst.Count > 0 Then
                    Dim bmap_sel As New BMappers(Of selector2array)
                    Dim sql2 As String = "SELECT TemplateId AS id, Name as text FROM TemplateName WHERE (Loc_" & loc_lst.ToArray()(0).Object1.ToString().Trim() & " = 1) AND (Active = 1)"
                    sel2_lst = bmap_sel.GetInspectObject(sql2)
                End If
            End If

            If sel2_lst.Count > 0 Then
                Dim listar = sel2_lst.ToArray()
                sel2_lst.Add(New selector2array With {.id = -1, .text = "SELECT OPTION"})
                sel2_ret = (From x In sel2_lst Select x Order By x.id Ascending).ToList()
            Else
                sel2_ret.Add(New selector2array With {.id = -1, .text = "NO TEMPLATES"})
            End If
            Return sel2_ret
        End Function

        Public Function GetTemplateListByLocation_2(ByVal CID As Integer) As List(Of selector2array)

            Dim bmap_locm As New BMappers(Of core.SingleObject)
            Dim sel2_lst As New List(Of selector2array)
            Dim sel2_ret As New List(Of selector2array)
            If CID > 0 Then
                Dim loc_lst As List(Of core.SingleObject)
                Dim sql1 As String = "SELECT id AS Object1, Abreviation AS Object3 FROM LocationMaster WHERE CID = 000" & CID.ToString()
                loc_lst = bmap_locm.GetAprMangObject(sql1)
                If loc_lst.Count > 0 Then
                    Dim bmap_sel As New BMappers(Of selector2array)
                    Dim sql2 As String = "select tn.TemplateId as id, tn.Name as text from TemplateName tn INNER JOIN TemplateActivator ta ON tn.TemplateId = ta.TemplateId where ta.LocationMasterId = " + loc_lst.ToArray()(0).Object1.ToString() + " and ta.Status = 1 and tn.Active = 1"
                    sel2_lst = bmap_sel.GetInspectObject(sql2)
                End If
            End If

            If sel2_lst.Count > 0 Then
                Dim listar = sel2_lst.ToArray()
                sel2_lst.Add(New selector2array With {.id = -1, .text = "SELECT OPTION"})
                sel2_ret = (From x In sel2_lst Select x Order By x.id Ascending).ToList()
            Else
                sel2_ret.Add(New selector2array With {.id = -1, .text = "NO TEMPLATES"})
            End If
            Return sel2_ret
        End Function

        Public Function GetSPCMachineNames(ByVal CID As String) As List(Of selector2array)
            Dim sql As String
            Dim selectvalues As New List(Of selector2array)
            Dim bmap As New BMappers(Of selector2array)

            If CID = "999" Then
                sql = "SELECT Id AS id, Machine as text FROM LiveProduction"
            Else
                sql = "SELECT Id AS id, Machine as text FROM LiveProduction WHERE CID = '" & CID & "'"
            End If

            selectvalues = bmap.GetSpcObject(sql)

            Return selectvalues

        End Function

        Public Function GetLocations(Optional ByVal IsReporter As Boolean = True) As List(Of selector2array)
            Dim sqlstring As String
            Dim selectValues As New List(Of Locationarray)()
            Dim bmap As New BMappers(Of Locationarray)

            sqlstring = "SELECT id as id, Name as text, Abreviation as Abreviation, CAST(CAST(CID AS INT) AS VARCHAR) as CID, AS400_Abr as ProdAbreviation FROM  LocationMaster WHERE  (InspectionResults = 1) ORDER BY text ASC"
            Try

            
            selectValues = bmap.GetAprMangObject(sqlstring)
            If selectValues.Count > 0 Then

                If IsReporter = True Then

                    Dim object1 As Object = (From x In selectValues Order By x.id Descending Select x.id).ToArray()

                        selectValues.Add(New Locationarray With {.id = object1(0) + 1, .CID = 999, .text = "ALL SITES", .Abreviation = "ALL", .ProdAbreviation = "ALL"})



                        NEWALLID = 999
                End If

                LocationsSelectors = (From x In selectValues Order By x.id Descending Select New core.selector2array With {.id = x.id, .text = Trim(x.text)}).ToList()

            End If
                LocationNames = selectValues

            Catch ex As Exception
                Throw New Exception("Error Retrieving Locations: " + ex.Message)
            End Try

            Return LocationsSelectors

        End Function

        Public Function GetTemplateTable() As List(Of SPCInspection.TemplateTable)
            Dim sqlstring As String
            Dim selectValues As New List(Of SPCInspection.TemplateTable)()
            Dim bmaptt As New BMappers(Of SPCInspection.TemplateTable)

            sqlstring = "SELECT * FROM TemplateName WHERE ACTIVE = 'True' ORDER BY Active DESC"

            'selectValues = _DAOFactory.getTemplateTable(sqlstring, 3)
            selectValues = bmaptt.GetInspectObject(sqlstring)

            For i = 0 To selectValues.Count - 1
                Dim temparray As Array = selectValues.ToArray()
                Dim thisdate As Date = Convert.ToDateTime(temparray(i).DateCreated)
                selectValues(i).DateCreated = thisdate.ToString("MM/dd/yyyy")
            Next

            Return selectValues

        End Function

        Public Function LoadOpenWorkOrders(ByVal InspectionState As String, ByVal PassCID As String) As String
            Dim bmapis As New BMappers(Of SPCInspection.InspectionJobSummary)
            Dim listis As New List(Of SPCInspection.InspectionJobSummary)
            Dim listret As New List(Of SPCInspection.InspectionJobSummary)
            Dim listspcret As New List(Of SPCInspection.InspectionJobSummary)
            Dim jser As New JavaScriptSerializer()
            Dim DefaultId As Integer = 1
            Dim sql As String = "SELECT ijs.id, ijs.JobNumber, ijs.JobType, ijs.ItemFailCount, ijs.TemplateId, tn.Name, ijs.AQL_Level, ijs.Standard, ijs.SampleSize, ijs.RejectLimiter, convert(varchar(25), ijs.Inspection_Started, 100) as Inspection_StartedString FROM InspectionJobSummary ijs LEFT OUTER JOIN TemplateName tn ON ijs.TemplateId = tn.TemplateId " & vbCrLf &
                                    "WHERE (Technical_PassFail IS NULL) AND (Inspection_Started >= GETDATE() - 3) AND (LEN(JobNumber) > 0) AND (CID = '" & PassCID & "')"
            listis = bmapis.GetInspectObject(sql)

            If listis.Count > 0 Then
                Dim listar = listis.ToArray()
                DefaultId = listar(listar.Length - 1).id
                listspcret = LoadOpenSPCMachine(PassCID)
                For Each item In listspcret
                    listis.Add(item)
                Next

                listis.Add(New SPCInspection.InspectionJobSummary With {.id = 1000, .JobNumber = "SELECT OPTION"})
                listret = (From x In listis Select x Order By x.id Descending).ToList()
            Else
                listspcret = LoadOpenSPCMachine(PassCID)

                For Each item In listspcret
                    listis.Add(item)
                Next
                listis.Add(New SPCInspection.InspectionJobSummary With {.id = 1000, .JobNumber = "SELECT OPTION"})
                listret = (From x In listis Select x Order By x.id Descending).ToList()
            End If

            Return jser.Serialize(listret)

        End Function

        Private Function LoadOpenSPCMachine(ByVal PassCID As String) As List(Of SPCInspection.InspectionJobSummary)
            Dim bmapis As New BMappers(Of SPCInspection.InspectionJobSummary)
            Dim listis As New List(Of SPCInspection.InspectionJobSummary)
            Dim bmapso As New BMappers(Of SingleObject)
            Dim listso As New List(Of SingleObject)
            Dim ijssql As String = ""
            Dim sql As String
            Dim SQL2 As String = "SELECT InspectionSummaryId AS Object1, Machine AS Object3 FROM LiveProduction WHERE InspectionSummaryId > 0 AND LocationCID = '" & PassCID & "'"
            listso = bmapso.GetInspectObject(SQL2)
            If listso.Count > 0 Then

                Dim cnt As Integer = 0
                For Each item In listso
                    Dim addsql As String = " id = " & item.Object1 & ""
                    If cnt < listso.Count - 1 Then
                        ijssql = ijssql + addsql + " OR "
                    Else
                        ijssql = ijssql + addsql
                    End If
                    cnt += 1
                Next
                sql = "SELECT ijs.id, ijs.JobNumber, ijs.ItemFailCount, ijs.TemplateId, tn.Name, ijs.AQL_Level, ijs.Standard, ijs.SampleSize, ijs.RejectLimiter, convert(varchar(25), ijs.Inspection_Started, 100) as Inspection_StartedString FROM InspectionJobSummary ijs LEFT OUTER JOIN TemplateName tn ON ijs.TemplateId = tn.TemplateId " & vbCrLf &
                                        "WHERE (ijs.JobNumber IS NOT NULL) AND ( " + ijssql + ")"
                listis = bmapis.GetInspectObject(sql)

                For Each itme In listis
                    Dim name As Object = (From v In listso Where v.Object1 = itme.id Select v.Object3).ToArray()
                    If Not IsNothing(name) Then
                        itme.ProdMachineName = name(0)
                        itme.IsSPC = True
                    End If
                Next
            End If
            Return listis

        End Function

        Public Function GetTemplateCollection(ByVal TemplateId As Integer) As List(Of SPCInspection.TemplateCollection)
            Dim sqlstring As String
            Dim selectValues As New List(Of SPCInspection.TemplateCollection)()

            sqlstring = "SELECT TabTemplate.TabTemplateId, ButtonTemplate.ButtonId, TabTemplate.Name, TabTemplate.TabNumber, TabTemplate.TemplateId, ButtonLibrary.Name AS ButtonName, TabTemplate.ProductSpecs, ButtonTemplate.DefectType, ButtonTemplate.id, ButtonLibrary.DefectCode, ButtonTemplate.Hide" & vbCrLf &
                            "FROM TabTemplate LEFT OUTER JOIN" & vbCrLf &
                            "ButtonTemplate ON TabTemplate.TabTemplateId = ButtonTemplate.TabTemplateId LEFT OUTER JOIN" & vbCrLf &
                            "ButtonLibrary ON ButtonTemplate.ButtonId = ButtonLibrary.ButtonId" & vbCrLf &
                            "WHERE (TabTemplate.TemplateId = " & TemplateId.ToString() & ") AND (TabTemplate.Active = 1)"


            'selectValues = _DAOFactory.getTemplateCollection(sqlstring)
            selectValues = bmap.GetInspectObject(sqlstring)
            If selectValues.Count = 0 Then
                Return selectValues
            End If
            Return selectValues

        End Function

        Public Function GetInputTemplateCollection(ByVal TemplateId As Integer) As List(Of SPCInspection.TemplateCollection)
            Dim sqlstring As String
            Dim selectValues As New List(Of SPCInspection.TemplateCollection)()

            sqlstring = "SELECT TabTemplate.TabTemplateId, ButtonTemplate.ButtonId, TabTemplate.Name, TabTemplate.TabNumber, TabTemplate.TemplateId, ButtonLibrary.Name AS ButtonName, TabTemplate.ProductSpecs, ButtonTemplate.DefectType, ButtonTemplate.id, ButtonLibrary.DefectCode, ButtonTemplate.Hide, ButtonLibrary.ButtonId AS ButtonLibraryId, ButtonTemplate.Timer" & vbCrLf &
                            "FROM TabTemplate LEFT OUTER JOIN" & vbCrLf &
                            "ButtonTemplate ON TabTemplate.TabTemplateId = ButtonTemplate.TabTemplateId LEFT OUTER JOIN" & vbCrLf &
                            "ButtonLibrary ON ButtonTemplate.ButtonId = ButtonLibrary.ButtonId" & vbCrLf &
                            "WHERE (TabTemplate.TemplateId = " & TemplateId.ToString() & ") AND (TabTemplate.Active = 1) AND (ButtonLibrary.Hide = 0 or Buttonlibrary.Hide IS NULL) and (ButtonTemplate.Hide = 0)" & vbCrLf &
                            "ORDER BY TabNumber ASC"


            'selectValues = _DAOFactory.getTemplateCollection(sqlstring)
            selectValues = bmap.GetInspectObject(sqlstring)
            If selectValues.Count = 0 Then
                Return selectValues
            End If
            Return selectValues

        End Function

        Public Function GetInputTemplateCollection_Admin(ByVal TemplateId As Integer) As List(Of SPCInspection.TemplateCollection)
            Dim sqlstring As String
            Dim selectValues As New List(Of SPCInspection.TemplateCollection)()

            sqlstring = "SELECT TabTemplate.TabTemplateId, ButtonTemplate.ButtonId, TabTemplate.Name, TabTemplate.TabNumber, TabTemplate.TemplateId, ButtonLibrary.Name AS ButtonName, TabTemplate.ProductSpecs, ButtonTemplate.DefectType, ButtonTemplate.id, ButtonLibrary.DefectCode, ButtonTemplate.Hide, ButtonLibrary.ButtonId AS ButtonLibraryId, ButtonTemplate.Timer" & vbCrLf &
                            "FROM TabTemplate LEFT OUTER JOIN" & vbCrLf &
                            "ButtonTemplate ON TabTemplate.TabTemplateId = ButtonTemplate.TabTemplateId LEFT OUTER JOIN" & vbCrLf &
                            "ButtonLibrary ON ButtonTemplate.ButtonId = ButtonLibrary.ButtonId" & vbCrLf &
                            "WHERE (TabTemplate.TemplateId = " & TemplateId.ToString() & ") AND (TabTemplate.Active = 1) AND (ButtonLibrary.Hide = 0 or Buttonlibrary.Hide IS NULL)" & vbCrLf &
                            "ORDER BY TabNumber ASC"


            'selectValues = _DAOFactory.getTemplateCollection(sqlstring)
            selectValues = bmap.GetInspectObject(sqlstring)
            If selectValues.Count = 0 Then
                Return selectValues
            End If
            Return selectValues

        End Function

        Public Function GetSpecCollection(ByVal TemplateId As Integer) As List(Of SPCInspection.ProductSpecCollection)
            Dim sqlstring As String
            Dim selectValues As New List(Of SPCInspection.ProductSpecCollection)()

            sqlstring = "SELECT TabTemplate.TabTemplateId, TabTemplate.TabNumber, ProductSpecification.Spec_Description, ProductSpecification.value, ProductSpecification.Upper_Spec_Value, ProductSpecification.Lower_Spec_Value, ProductSpecification.SpecId, TabTemplate.Name" & vbCrLf &
                         "FROM ProductSpecification INNER JOIN" & vbCrLf &
                         "TabTemplate ON ProductSpecification.TabTemplateId = TabTemplate.TabTemplateId" & vbCrLf &
                         "WHERE (TabTemplate.TemplateId = " & TemplateId.ToString() & ")"


            selectValues = _DAOFactory.getProductSpecCollection(sqlstring)
            If selectValues.Count = 0 Then
                Return selectValues
            End If
            Return selectValues
        End Function

        'Public Function GetDisplaySpecCollection(ByVal CID As String) As List(Of SPCInspection.ProductDisplaySpecCollection)
        '    Dim sqlstring As String
        '    Dim selectValues As New List(Of SPCInspection.ProductDisplaySpecCollection)
        '    Dim bmapps As New BMappers(Of SPCInspection.ProductDisplaySpecCollection)

        '    If CID <> "999" Then
        '        sqlstring = "SELECT         SpecMeasurements.SpecId, SpecMeasurements.id, SpecMeasurements.InspectionJobSummaryId, SpecMeasurements.Timestamp, SpecMeasurements.MeasureValue, InspectionJobSummary.JobNumber, " & vbCrLf &
        '                   "SpecMeasurements.DefectId, SpecMeasurements.InspectionId, InspectionJobSummary.Inspection_Started, ProductSpecification.POM_Row, ProductSpecification.DataNo, SpecMeasurements.ItemNumber, ProductSpecification.ProductType," & vbCrLf &
        '                   "ProductSpecification.Spec_Description, ProductSpecification.value, SpecMeasurements.MeasureValue - ProductSpecification.value AS SpecDelta, ProductSpecification.Upper_Spec_Value," & vbCrLf &
        '                   "ProductSpecification.Lower_Spec_Value" & vbCrLf &
        '"FROM            SpecMeasurements LEFT OUTER JOIN" & vbCrLf &
        '                   "InspectionJobSummary ON SpecMeasurements.InspectionJobSummaryId = InspectionJobSummary.id LEFT OUTER JOIN" & vbCrLf &
        '                   "ProductSpecification ON SpecMeasurements.SpecId = ProductSpecification.SpecId" & vbCrLf &
        '"WHERE        (InspectionJobSummary.CID = N'" & CID & "')" & vbCrLf &
        '"ORDER BY SpecMeasurements.InspectionJobSummaryId"
        '    Else
        '        sqlstring = "SELECT         SpecMeasurements.SpecId, SpecMeasurements.id, SpecMeasurements.InspectionJobSummaryId, SpecMeasurements.Timestamp, SpecMeasurements.MeasureValue, InspectionJobSummary.JobNumber, " & vbCrLf &
        '                       "SpecMeasurements.DefectId, SpecMeasurements.InspectionId, InspectionJobSummary.Inspection_Started, ProductSpecification.POM_Row, ProductSpecification.DataNo, SpecMeasurements.ItemNumber, ProductSpecification.ProductType," & vbCrLf &
        '                       "ProductSpecification.Spec_Description, ProductSpecification.value, SpecMeasurements.MeasureValue - ProductSpecification.value AS SpecDelta, ProductSpecification.Upper_Spec_Value," & vbCrLf &
        '                       "ProductSpecification.Lower_Spec_Value" & vbCrLf &
        '    "FROM            SpecMeasurements LEFT OUTER JOIN" & vbCrLf &
        '                       "InspectionJobSummary ON SpecMeasurements.InspectionJobSummaryId = InspectionJobSummary.id LEFT OUTER JOIN" & vbCrLf &
        '                       "ProductSpecification ON SpecMeasurements.SpecId = ProductSpecification.SpecId" & vbCrLf &
        '    "ORDER BY SpecMeasurements.InspectionJobSummaryId"
        '    End If




        '    selectValues = bmapps.GetInspectObject(sqlstring)

        '    Return selectValues
        'End Function

        Public Function GetDisplaySpecCollection2(ByVal CID As String, ByVal fromdate As DateTime, ByVal todate As DateTime) As List(Of SPCInspection.ProductDisplaySpecCollection)
            Dim sqlstring As String
            Dim selectValues As New List(Of SPCInspection.ProductDisplaySpecCollection)
            Dim bmapps As New BMappers(Of SPCInspection.ProductDisplaySpecCollection)


            sqlstring = "SELECT         SpecMeasurements.SpecId, SpecMeasurements.id, SpecMeasurements.InspectionJobSummaryId, SpecMeasurements.Timestamp, SpecMeasurements.MeasureValue, InspectionJobSummary.JobNumber, " & vbCrLf &
                                   "SpecMeasurements.DefectId, SpecMeasurements.InspectionId, InspectionJobSummary.Inspection_Started, ProductSpecification.POM_Row, ProductSpecification.DataNo, SpecMeasurements.ItemNumber, ProductSpecification.ProductType," & vbCrLf &
                                   "ProductSpecification.Spec_Description, ProductSpecification.value, SpecMeasurements.MeasureValue - ProductSpecification.value AS SpecDelta, ProductSpecification.Upper_Spec_Value," & vbCrLf &
                                   "ProductSpecification.Lower_Spec_Value, InspectionJobSummary.CID AS Location" & vbCrLf &
                "FROM            SpecMeasurements LEFT OUTER JOIN" & vbCrLf &
                                   "InspectionJobSummary ON SpecMeasurements.InspectionJobSummaryId = InspectionJobSummary.id LEFT OUTER JOIN" & vbCrLf &
                                   "ProductSpecification ON SpecMeasurements.SpecId = ProductSpecification.SpecId" & vbCrLf &
                "WHERE        (Timestamp >= CONVERT(DATETIME,'" & fromdate.ToString("yyyy-MM-dd H:mm:ss") & "' )) AND (Timestamp < CONVERT(DATETIME,'" & todate.AddDays(1).ToString("yyyy-MM-dd H:mm:ss") & "' ))" & vbCrLf &
                "ORDER BY SpecMeasurements.InspectionJobSummaryId DESC"

            selectValues = bmapps.GetInspectObject(sqlstring)

            Return selectValues
        End Function

        Public Function GetDisplaySpecCollection3(ByVal fromdate As DateTime, ByVal todate As DateTime) As List(Of SPCInspection.ProductDisplaySpecCollection)
            Dim sqlstring As String
            Dim selectValues As New List(Of SPCInspection.ProductDisplaySpecCollection)
            Dim bmapps As New BMappers(Of SPCInspection.ProductDisplaySpecCollection)


            sqlstring = "SELECT         SpecMeasurements.SpecId, SpecMeasurements.id, SpecMeasurements.InspectionJobSummaryId, SpecMeasurements.Timestamp, SpecMeasurements.MeasureValue, InspectionJobSummary.JobNumber, " & vbCrLf &
                                   "SpecMeasurements.DefectId, SpecMeasurements.InspectionId, InspectionJobSummary.Inspection_Started, ProductSpecification.POM_Row, ProductSpecification.DataNo, SpecMeasurements.ItemNumber, ProductSpecification.ProductType," & vbCrLf &
                                   "ProductSpecification.Spec_Description, ProductSpecification.value, SpecMeasurements.MeasureValue - ProductSpecification.value AS SpecDelta, ProductSpecification.Upper_Spec_Value," & vbCrLf &
                                   "ProductSpecification.Lower_Spec_Value, InspectionJobSummary.CID AS Location" & vbCrLf &
                "FROM            SpecMeasurements LEFT OUTER JOIN" & vbCrLf &
                                   "InspectionJobSummary ON SpecMeasurements.InspectionJobSummaryId = InspectionJobSummary.id LEFT OUTER JOIN" & vbCrLf &
                                   "ProductSpecification ON SpecMeasurements.SpecId = ProductSpecification.SpecId" & vbCrLf &
                "WHERE        (Timestamp >= CONVERT(DATETIME,'" & fromdate.ToString("yyyy-MM-dd H:mm:ss") & "' )) AND (Timestamp < CONVERT(DATETIME,'" & todate.AddDays(1).ToString("yyyy-MM-dd H:mm:ss") & "' ))" & vbCrLf &
                "ORDER BY SpecMeasurements.InspectionJobSummaryId DESC"

            selectValues = bmapps.GetInspectObject(sqlstring)

            Return selectValues
        End Function

        Public Function GetUpdatedDisplaySpecCollection(ByVal SpecFilter As Integer) As List(Of SPCInspection.ProductDisplaySpecCollection)
            Dim sqlstring As String
            Dim selectValues As New List(Of SPCInspection.ProductDisplaySpecCollection)
            Dim bmapps As New BMappers(Of SPCInspection.ProductDisplaySpecCollection)


            sqlstring = "SELECT         SpecMeasurements.SpecId, SpecMeasurements.id, SpecMeasurements.InspectionJobSummaryId, SpecMeasurements.Timestamp, SpecMeasurements.MeasureValue, InspectionJobSummary.JobNumber, " & vbCrLf &
                                   "SpecMeasurements.DefectId, SpecMeasurements.InspectionId, InspectionJobSummary.Inspection_Started, ProductSpecification.POM_Row, ProductSpecification.DataNo, SpecMeasurements.ItemNumber, ProductSpecification.ProductType," & vbCrLf &
                                   "ProductSpecification.Spec_Description, ProductSpecification.value, SpecMeasurements.MeasureValue - ProductSpecification.value AS SpecDelta, ProductSpecification.Upper_Spec_Value," & vbCrLf &
                                   "ProductSpecification.Lower_Spec_Value, InspectionJobSummary.CID AS Location" & vbCrLf &
                "FROM            SpecMeasurements LEFT OUTER JOIN" & vbCrLf &
                                   "InspectionJobSummary ON SpecMeasurements.InspectionJobSummaryId = InspectionJobSummary.id LEFT OUTER JOIN" & vbCrLf &
                                   "ProductSpecification ON SpecMeasurements.SpecId = ProductSpecification.SpecId" & vbCrLf &
                "WHERE        (SpecId > " & SpecFilter.ToString() & ")" & vbCrLf &
                "ORDER BY SpecMeasurements.InspectionJobSummaryId DESC"

            selectValues = bmapps.GetInspectObject(sqlstring)

            Return selectValues
        End Function


        Public Function DeleteButtonTemplate(ByVal TemplateId As Integer) As Boolean
            Dim SQL As String
            Dim sqlcommand As SqlCommand
            Dim returnint As Integer

            'SQL = "DELETE FROM Maintenance_Schedule WHERE MM_ID = " & MM_ID & " And MT_ID = " & MT_ID & " And MS_Next_Main_Date = @Start_Date "
            SQL = "DELETE FROM ButtonTemplate" & vbCrLf &
                    "WHERE (TabTemplateId = " & TemplateId.ToString() & ")"
            Using connection As New SqlConnection(DL.InspectConnectionString())

                sqlcommand = _DAOFactory.GetCommand(SQL.ToString(), connection)
                'Add command parameters             

                Try
                    sqlcommand.Connection.Open()
                    returnint = sqlcommand.ExecuteNonQuery()

                    If returnint < 0 Then
                        Return False
                    End If

                Catch e As Exception
                    Return False
                End Try



            End Using
            Return True

        End Function
        Public Function AddDefectType(ByVal Code As String, ByVal Text As String) As Integer
            Dim SQL As String
            Dim Internalcmd As SqlCommand
            Dim returnint As Integer

            SQL = "INSERT INTO ButtonLibrary (Name, Text, DefectCode)" & vbCrLf &
                    "VALUES (@Name,@Text, @DefectCode)"
            Using connection As New SqlConnection(DL.InspectConnectionString())

                Internalcmd = _DAOFactory.GetCommand(SQL, connection)

                Internalcmd.Parameters.Add(_DAOFactory.Getparameter("@Name", DbType.String))
                Internalcmd.Parameters.Add(_DAOFactory.Getparameter("@Text", DbType.String))
                Internalcmd.Parameters.Add(_DAOFactory.Getparameter("@DefectCode", DbType.String))
                Internalcmd.Parameters("@Name").Value = Text
                Internalcmd.Parameters("@Text").Value = Text
                Internalcmd.Parameters("@DefectCode").Value = Code
                Try
                    Internalcmd.Connection.Open()
                    returnint = Convert.ToInt32(Internalcmd.ExecuteNonQuery())
                    Return returnint
                Catch ex As Exception

                    Return -1
                    Exit Function
                End Try

            End Using

        End Function

        Public Function AddProductSpec(ByVal TabTemplateId As Integer, ByVal Description As String, ByVal value As Decimal, ByVal UpperSpec As Decimal, ByVal LowerSpec As Decimal) As Integer

            Dim SQL As String
            Dim Internalcmd As SqlCommand
            Dim returnint As Integer

            SQL = "INSERT INTO ProductSpecification" & vbCrLf &
                         "(TabTemplateId, Spec_Description, value, Upper_Spec_Value, Lower_Spec_Value)" & vbCrLf &
                         "VALUES (@TabTemplateId,@Spec_Description,@value,@Upper_Spec_Value,@Lower_Spec_Value)"

            Using connection As New SqlConnection(DL.InspectConnectionString())

                Internalcmd = _DAOFactory.GetCommand(SQL, connection)

                Internalcmd.Parameters.Add(_DAOFactory.Getparameter("@TabTemplateId", DbType.Int32))
                Internalcmd.Parameters.Add(_DAOFactory.Getparameter("@Spec_Description", DbType.String))
                Internalcmd.Parameters.Add(_DAOFactory.Getparameter("@value", DbType.Decimal))
                Internalcmd.Parameters.Add(_DAOFactory.Getparameter("@Upper_Spec_Value", DbType.Decimal))
                Internalcmd.Parameters.Add(_DAOFactory.Getparameter("@Lower_Spec_Value", DbType.Decimal))
                Internalcmd.Parameters("@TabTemplateId").Value = TabTemplateId
                Internalcmd.Parameters("@Spec_Description").Value = Description
                Internalcmd.Parameters("@value").Value = value
                Internalcmd.Parameters("@Upper_Spec_Value").Value = UpperSpec
                Internalcmd.Parameters("@Lower_Spec_Value").Value = LowerSpec

                Try
                    Internalcmd.Connection.Open()
                    returnint = Convert.ToInt32(Internalcmd.ExecuteNonQuery())
                    Return returnint
                Catch ex As Exception

                    Throw New System.Exception(ex.Message)
                    Exit Function
                End Try

            End Using


        End Function
        Public Function delSpecM(ByVal rowId As Integer) As Integer
            Dim Outcome As String = ""
            Dim SQL As String = "DELETE FROM dbo.SpecMeasurements Where SpecId = " & rowId.ToString()

            Outcome = ExecuteSQL(SQL, 1)


            If Outcome = "Successful" Then
                Return True
            End If
            Return False
        End Function
        Public Function delSpec(ByVal rowId As Integer) As Integer
            Dim Outcome As String = ""
            Dim SQL As String = "DELETE FROM dbo.ProductSpecification Where SpecId = " & rowId.ToString()
            If delSpecM(rowId) Then
                Outcome = ExecuteSQL(SQL, 1)
            End If

            If Outcome = "Successful" Then
                Return True
            End If
            Return False
        End Function
        Public Function EditButtonClass(ByVal tabId As Integer, ByVal DefectClass As String) As Integer
            Dim Outcome As String = ""
            Dim SQL As String = "UPDATE dbo.ButtonTemplate SET DefectType='" & DefectClass & "' WHERE id =  " & tabId.ToString()
            Outcome = ExecuteSQL(SQL, 1)
            If Outcome = "Successful" Then
                Return True
            End If
            Return False
        End Function
        Public Function DeleteDefectButtonTemplate(ByVal rowId As Integer) As Integer 'helper method to get AROUND FOREIGN KEY CONSTRAINTS
            Dim Outcome As String = ""
            Dim SQL As String = "UPDATE dbo.ButtonTemplate SET Hide = 1 WHERE ButtonId =  " & rowId.ToString()
            Outcome = ExecuteSQL(SQL, 1)
            If Outcome = "Successful" Then
                Return True
            End If
            Return False
        End Function
        Public Function DeleteRow(ByVal rowId As Integer) As Integer
            Dim Outcome As String = ""
            Dim SQL As String = "UPDATE dbo.ButtonLibrary SET Hide = 1 WHERE ButtonId = " & rowId.ToString()
            If DeleteDefectButtonTemplate(rowId) Then
                Outcome = ExecuteSQL(SQL, 1)
            End If
            If Outcome = "Successful" Then
                Return True
            End If
            Return False
        End Function
        Public Function AlterDefectRow(ByVal rowId As Integer, ByVal DefectCode As String, ByVal Name As String) As Integer
            Dim Outcome As String = ""
            If rowId = -1 Then
                Dim SQL As String = "INSERT INTO dbo.ButtonLibrary (Name, DefectCode) VALUES ('" & Name & "', '" & DefectCode & "')"
                Outcome = ExecuteSQL(SQL, 1)
                If Outcome = "Successful" Then
                    Return True
                End If
                Return False

            Else

                Dim SQL As String = "UPDATE dbo.ButtonLibrary SET Name = '" & Name & "', DefectCode = '" & DefectCode & "' WHERE ButtonId =  " & rowId.ToString()
                Outcome = ExecuteSQL(SQL, 1)
                If Outcome = "Successful" Then
                    Return True
                End If
                Return False
            End If

        End Function
        Public Function DeleteTabTemplate(ByVal TabTemplatId As Integer, ByVal TemplateId As Integer) As Integer
            Dim Outcome As String = ""
            Dim SQL As String = "UPDATE TabTemplate SET Active = 0 WHERE (TemplateId = " & TemplateId.ToString() & ") AND (TabTemplateId = " & TabTemplatId.ToString() & ")"
            Outcome = ExecuteSQL(SQL, 1)
            If Outcome = "Successful" Then
                Return True
            End If
            Return False

        End Function
        Public Function UpdateButtonLibrary(ByVal obj As SPCInspection.ButtonLibrarygrid) As Boolean
            Dim Outcome As String = ""
            Dim SQL As String = "UPDATE dbo.ButtonLibrary SET Name = '" & obj.Name & "', DefectCode = '" & obj.DefectCode & "' WHERE ButtonId =  " & obj.ButtonId.ToString()
            Outcome = ExecuteSQL(SQL, 1)
            If Outcome = "Successful" Then
                Return True
            End If
            Return False
        End Function
        Public Function ToggleStatusTemplateById(ByVal TemplateId As Integer, ByVal Status As Boolean) As Integer
            Dim Outcome As String = ""
            Dim SQL As String
            Dim StatusVal As String

            If Status = True Then
                StatusVal = "0"
            Else
                StatusVal = "1"
            End If
            SQL = "UPDATE TemplateName SET Active = " & StatusVal & "WHERE (TemplateId = " & TemplateId.ToString() & ")"
            Outcome = ExecuteSQL(SQL, 1)
            If Outcome = "Successful" Then
                Return True
            End If
            Return False


        End Function
        Public Function UpdateProductSpecBit(ByVal TabTemplateId As Integer) As Integer
            Dim SQL As String
            Dim Outcome As String = ""
            SQL = "UPDATE TabTemplate" & vbCrLf &
            "SET ProductSpecs = 1" & vbCrLf &
            "WHERE (TabTemplateId = " & TabTemplateId.ToString() & ")"

            Outcome = ExecuteSQL(SQL, 1)
            If Outcome = "Successful" Then
                Return True
            End If
            Return False

        End Function

        Public Function InsertTemplate(ByVal name As String, Username As String, ByVal LineTypeId As Integer, ByVal LineName As String) As Integer

            Dim SQL As String
            Dim sqlcommand As SqlCommand
            Dim returnint As Integer

            SQL = "INSERT INTO TemplateName (Name,Owner, DateCreated, LineTypeId, LineType, Active) VALUES(@Name, @Username, @Created, @LineTypeId, @LineType, 1)  SELECT SCOPE_IDENTITY()   "


            Using connection As New SqlConnection(DL.InspectConnectionString())


                sqlcommand = _DAOFactory.GetCommand(SQL, connection)
                ''    'Add command parameters                                                                          
                sqlcommand.Parameters.Add(_DAOFactory.Getparameter("@Name", DbType.String))
                sqlcommand.Parameters.Add(_DAOFactory.Getparameter("@Username", DbType.String))
                sqlcommand.Parameters.Add(_DAOFactory.Getparameter("@Created", DbType.DateTime))
                sqlcommand.Parameters.Add(_DAOFactory.Getparameter("@LineTypeId", DbType.Int64))
                sqlcommand.Parameters.Add(_DAOFactory.Getparameter("@LineType", DbType.String))
                ''    'Provide parameter values.                                                                    
                sqlcommand.Parameters("@Name").Value = name
                sqlcommand.Parameters("@Username").Value = Username
                sqlcommand.Parameters("@Created").Value = DateTime.Now
                sqlcommand.Parameters("@LineTypeId").Value = LineTypeId
                sqlcommand.Parameters("@LineType").Value = LineName

                Try

                    sqlcommand.Connection.Open()
                    returnint = sqlcommand.ExecuteScalar()

                Catch e As Exception

                    Return 0
                    Exit Function
                End Try


            End Using
            Return returnint

        End Function

        Public Function InsertTab(ByVal TemplateId As Integer, ByVal Tabname As String, Tabnumber As Integer) As Integer

            Dim SQL As String
            Dim sqlcommand As SqlCommand
            Dim returnint As Integer

            SQL = "INSERT INTO TabTemplate (TemplateId, Name, TabNumber, Updated) VALUES(@TemplateId, @Name, @TabNumber, @Updated) SELECT SCOPE_IDENTITY()  "


            Using connection As New SqlConnection(DL.InspectConnectionString())


                sqlcommand = _DAOFactory.GetCommand(SQL, connection)
                ''    'Add command parameters                                                                          
                sqlcommand.Parameters.Add(_DAOFactory.Getparameter("@TemplateId", DbType.Int32))
                sqlcommand.Parameters.Add(_DAOFactory.Getparameter("@Name", DbType.String))
                sqlcommand.Parameters.Add(_DAOFactory.Getparameter("@TabNumber", DbType.Int32))
                sqlcommand.Parameters.Add(_DAOFactory.Getparameter("@Updated", DbType.DateTime))
                ''    'Provide parameter values.                                                                    
                sqlcommand.Parameters("@TemplateId").Value = TemplateId
                sqlcommand.Parameters("@Name").Value = Tabname
                sqlcommand.Parameters("@TabNumber").Value = Tabnumber
                sqlcommand.Parameters("@Updated").Value = DateTime.Now

                Try

                    sqlcommand.Connection.Open()
                    returnint = Convert.ToInt32(sqlcommand.ExecuteScalar())

                Catch e As Exception

                    Return 0
                    Exit Function
                End Try


            End Using
            Return returnint

        End Function

        Public Function InsertButton(ByVal TabTemplateId As Integer, ByVal ButtonId As Integer, ByVal Name As String, ByVal DefectType As String, ByVal TimerFlag_Val As String) As Boolean

            Dim SQL As String
            Dim sqlcommand As SqlCommand
            Dim returnint As Integer
            Dim util As New Utilities

            SQL = "INSERT INTO ButtonTemplate (TabTemplateId, ButtonId, DefectType, Timer) VALUES(@TabTemplateId, @ButtonId, @DefectType, @Timer) SELECT SCOPE_IDENTITY()   "


            Using connection As New SqlConnection(DL.InspectConnectionString())


                sqlcommand = _DAOFactory.GetCommand(SQL, connection)
                ''    'Add command parameters                                                                          
                sqlcommand.Parameters.Add(_DAOFactory.Getparameter("@TabTemplateId", DbType.Int32))
                sqlcommand.Parameters.Add(_DAOFactory.Getparameter("@ButtonId", DbType.Int32))
                sqlcommand.Parameters.Add(_DAOFactory.Getparameter("@DefectType", DbType.String))
                sqlcommand.Parameters.Add(_DAOFactory.Getparameter("@Timer", DbType.Boolean))

                ''    'Provide parameter values.                                                                    
                sqlcommand.Parameters("@TabTemplateId").Value = TabTemplateId
                sqlcommand.Parameters("@ButtonId").Value = ButtonId
                sqlcommand.Parameters("@DefectType").Value = DefectType
                sqlcommand.Parameters("@Timer").Value = util.ConvertType(TimerFlag_Val, "boolean")



                Try

                    sqlcommand.Connection.Open()
                    returnint = Convert.ToInt32(sqlcommand.ExecuteScalar())

                Catch e As Exception

                    Return 0
                    Exit Function
                End Try


            End Using
            Return returnint

        End Function



        Public Function ActiveTemplate(ByVal TemplateId As Integer) As Boolean

            Dim SQL As String
            Dim sqlcommand As SqlCommand
            Dim returnint As Integer

            SQL = "UPDATE TemplateName" & vbCrLf &
            "SET Active = 1" & vbCrLf &
            "WHERE (TemplateId = @TemplateId)"

            Using connection As New SqlConnection(DL.InspectConnectionString())

                sqlcommand = _DAOFactory.GetCommand(SQL, connection)
                ''    'Add command parameters                                                                          
                sqlcommand.Parameters.Add(_DAOFactory.Getparameter("@TemplateId", DbType.Int32))

                ''    'Provide parameter values.                                                                    
                sqlcommand.Parameters("@TemplateId").Value = TemplateId

                Try
                    sqlcommand.Connection.Open()
                    returnint = Convert.ToInt32(sqlcommand.ExecuteScalar())
                    If returnint = 0 Then
                        Return False
                    End If
                Catch e As Exception

                    Return False
                    Exit Function
                End Try

            End Using
            Return True

        End Function

        Public Function SetLineType(ByVal TemplateId As Integer, ByVal LineType As String, ByVal ColumnCount As Integer) As Boolean

            Dim SQL As String
            Dim sqlcommand As SqlCommand
            Dim returnint As Integer
            Dim listso As List(Of SingleObject)
            Dim bmapso As New BMappers(Of SingleObject)

            listso = bmapso.GetInspectObject("SELECT id as Object1 FROM InspectionTypes WHERE Name = '" & LineType & "'")

            If IsNothing(listso) = False Then
                If listso.Count > 0 Then


                    SQL = "UPDATE TemplateName" & vbCrLf &
                    "SET LineTypeId = " & listso.ToArray()(0).Object1.ToString() & ", ColumnCount = " + ColumnCount.ToString() & vbCrLf &
                    "WHERE (TemplateId = @TemplateId)"

                    Using connection As New SqlConnection(DL.InspectConnectionString())

                        sqlcommand = _DAOFactory.GetCommand(SQL, connection)
                        ''    'Add command parameters                                                                          
                        sqlcommand.Parameters.Add(_DAOFactory.Getparameter("@TemplateId", DbType.Int32))

                        ''    'Provide parameter values.                                                                    
                        sqlcommand.Parameters("@TemplateId").Value = TemplateId

                        Try
                            sqlcommand.Connection.Open()
                            returnint = Convert.ToInt32(sqlcommand.ExecuteScalar())
                            If returnint = 0 Then
                                Return False
                            Else
                                Return True
                            End If
                            Exit Function
                        Catch e As Exception

                            Return False
                            Exit Function
                        End Try

                    End Using
                    Return True

                End If
            End If

            Return False

        End Function

        Public Function GetTemplateId(ByVal name As String, ByVal Username As String) As Integer
            Dim sqlstring As String
            Dim returnint As Integer
            sqlstring = "SELECT TOP (1) TemplateId FROM  TemplateName WHERE (Name = '" & name & "') AND (Owner = '" & Username & "')"

            returnint = _DAOFactory.GetTemplateId(sqlstring, 3)

            Return returnint

        End Function

        Public Function GetDefeaultTemplateID(ByVal TemplateId As String) As String

            Dim TemplateSet As DataSet = New DataSet
            Dim sql As String

            sql = "SELECT TOP (1) Name AS Expr1 FROM TemplateName WHERE(TemplateId = '" & TemplateId & "')"

            If Not util.FillSPCDataSet(TemplateSet, "TemplateSet", sql) Then
                Return Nothing
            End If
            For Each row As DataRow In TemplateSet.Tables(0).Rows
                Dim rowobject As Object = row
                If IsNothing(row.Item(0)) = False Then
                    Return Convert.ToString(row.Item(0))
                End If
            Next
            Return Nothing
        End Function

        Public Function GetTabTemplateID(ByVal TabTitle As String, TemplateId As Integer) As Integer

            Dim TemplateSet As DataSet = New DataSet
            Dim sql As String

            sql = "SELECT TOP (1) TabTemplateId FROM TabTemplate WHERE (TemplateId = '" & TemplateId & "') AND (Name = '" & TabTitle & "')"

            If Not util.FillSPCDataSet(TemplateSet, "TemplateSet", sql) Then
                Return Nothing
            End If
            If TemplateSet.Tables(0).Rows.Count > 0 Then
                Return TemplateSet.Tables(0).Rows(0)("TabTemplateId")
            End If

            Return Nothing

        End Function
        Public Function GetDefectMasterData(ByVal fromdate As DateTime, ByVal todate As DateTime, ByVal Location As String, ByVal Abb As String) As List(Of SPCInspection.DefectMasterDisplay)
            Dim SQL As String
            Dim returnlist As New List(Of SPCInspection.DefectMasterDisplay)
            Dim returnlist2 As New List(Of SPCInspection.DefectMasterDisplay)
            Dim fromdatestring As String = fromdate.ToString("yyyy-MM-dd H:mm:ss").Split(" ")(0) + " 00:00:00"
            Dim todatestring As String = todate.ToString("yyyy-MM-dd H:mm:ss").Split(" ")(0) + " 23:59:59"
            Dim bmap As New BMappers(Of SPCInspection.DefectMasterDisplay)
            Dim locationNameArray = (From x In LocationNames Where x.CID = Location Select x.text).ToArray()
            Dim LocmId = (From x In LocationNames Where x.CID = Location Select x.id).ToArray()

            If Abb = "ALL" Then
                SQL = "SELECT DefectMaster.DefectID, DefectMaster.DefectTime, DefectMaster.DefectDesc, TemplateName.TemplateId, TemplateName.Name, DefectMaster.DataNo, DefectMaster.EmployeeNo, DefectMaster.AQL, DefectMaster.InspectionId, DefectMaster.TotalLotPieces, DefectMaster.Product, DefectMaster.DefectClass, DefectMaster.WorkOrder, DefectMaster.RollNo, DefectMaster.LoomNo, DefectMaster.DataType, DefectMaster.DefectImage_Filename, DefectMaster.Inspector, DefectMaster.InspectionState, DefectMaster.ItemNumber, DefectMaster.WorkRoom" & vbCrLf &
             "FROM DefectMaster INNER JOIN  TemplateName ON DefectMaster.TemplateId = TemplateName.TemplateId" & vbCrLf &
             "WHERE (DefectTime >= CONVERT(DATETIME, '" & fromdatestring & "', 102)) AND (DefectTime <= CONVERT(DATETIME, '" & todatestring & "', 102))" & vbCrLf &
              "ORDER BY DefectMaster.DefectID DESC"
            Else
                SQL = "SELECT DefectMaster.DefectID, DefectMaster.DefectTime, DefectMaster.DefectDesc, TemplateName.TemplateId, TemplateName.Name, DefectMaster.DataNo, DefectMaster.EmployeeNo, DefectMaster.AQL, DefectMaster.InspectionId, DefectMaster.TotalLotPieces, DefectMaster.Product, DefectMaster.DefectClass, DefectMaster.WorkOrder, DefectMaster.RollNo, DefectMaster.LoomNo, DefectMaster.DataType, DefectMaster.DefectImage_Filename, DefectMaster.Inspector, DefectMaster.InspectionState, DefectMaster.ItemNumber, DefectMaster.WorkRoom" & vbCrLf &
             "FROM DefectMaster INNER JOIN  TemplateName ON DefectMaster.TemplateId = TemplateName.TemplateId" & vbCrLf &
             "WHERE (DefectTime >= CONVERT(DATETIME, '" & fromdatestring & "', 102)) AND (DefectTime <= CONVERT(DATETIME, '" & todatestring & "', 102)) AND (Location = '" + Location + "' OR Location = '" + LocmId(0).ToString() + "' OR SUBSTRING(DataType, 0, 4) = '" + Abb + "')" & vbCrLf &
              "ORDER BY DefectMaster.DefectID DESC"
            End If
            
            'SQL = "SELECT DefectID, DefectTime, DefectDesc, POnumber, DataNo, EmployeeNo, AQL, InspectionId, TotalLotPieces, Product, DefectClass, WorkOrder, RollNo, LoomNo, DataType, DefectImage_Filename, Inspector, InspectionState, ItemNumber" & vbCrLf &
            ' "FROM DefectMaster" & vbCrLf &
            ' "WHERE (TemplateId = " & TemplateId.ToString() & ") AND (DefectTime >= CONVERT(DATETIME, '" & fromdate.ToString("yyyy-MM-dd H:mm:ss") & "', 102)) AND (DefectTime <= CONVERT(DATETIME, '" & todatestring & "', 102))" & vbCrLf &
            '  "ORDER BY DefectID DESC"
            Try
                'returnlist2 = BMapper(Of SPCInspection.DefectMasterDisplay).GetInspectObject(SQL)
                returnlist2 = bmap.GetInspectObject(SQL)
            Catch ex As Exception

            End Try

            'returnlist = _DAOFactory.getDefectMasterData(SQL)
            'For i = 0 To returnlist.Count - 1
            '    Dim temparray As Array = returnlist.ToArray()
            '    Dim thisdate As Date = Convert.ToDateTime(temparray(i).DefectTime)
            '    returnlist(i).DefectTime = thisdate.ToString("MM/dd/yyyy H:mm:ss")
            'Next


            'If returnlist.Count = 0 Then
            '    returnlist.Add(New SPCInspection.DefectMasterDisplay With {.DefectID = 0, .DefectTime = "00/00/00", .DefectDesc = "___", .POnumber = "No Records", .DataNo = "No Records", .EmployeeNo = "___", .InspectionId = 0, .TotalLotPieces = "No Records", .Product = "No Records", .WorkOrder = "No Records", .RollNo = "No Records", .LoomNo = "___", .DataType = "___", .DefectImage_Filename = "___"})
            'End If

            Return returnlist2

        End Function

        Public Function GetDefectMasterData2(ByVal fromdate As DateTime, ByVal todate As DateTime, ByVal Location As String) As List(Of SPCInspection.DefectMasterDisplay)
            Dim SQL As String
            Dim returnlist As New List(Of SPCInspection.DefectMasterDisplay)
            Dim returnlist2 As New List(Of SPCInspection.DefectMasterDisplay)
            Dim fromdatestring As String = fromdate.ToString("yyyy-MM-dd H:mm:ss").Split(" ")(0) + " 00:00:00"
            Dim todatestring As String = todate.ToString("yyyy-MM-dd H:mm:ss").Split(" ")(0) + " 23:59:59"
            Dim bmap As New BMappers(Of SPCInspection.DefectMasterDisplay)

            If Location = "999" Then
                SQL = "SELECT DefectMaster.DefectID, DefectMaster.DefectTime, DefectMaster.DefectDesc, TemplateName.TemplateId, TemplateName.Name, DefectMaster.DataNo, DefectMaster.EmployeeNo, DefectMaster.AQL, DefectMaster.InspectionId, DefectMaster.TotalLotPieces, DefectMaster.Product, DefectMaster.DefectClass, DefectMaster.WorkOrder, DefectMaster.RollNo, DefectMaster.LoomNo, DefectMaster.DataType, DefectMaster.DefectImage_Filename, DefectMaster.Inspector, DefectMaster.InspectionState, DefectMaster.ItemNumber, DefectMaster.WorkRoom" & vbCrLf &
             "FROM DefectMaster INNER JOIN  TemplateName ON DefectMaster.TemplateId = TemplateName.TemplateId" & vbCrLf &
             "WHERE (DefectTime >= CONVERT(DATETIME, '" & fromdatestring & "', 102)) AND (DefectTime <= CONVERT(DATETIME, '" & todatestring & "', 102))" & vbCrLf &
              "ORDER BY DefectMaster.DefectID DESC"
            ElseIf Location = "578" Then
                SQL = "SELECT DefectMaster.DefectID, DefectMaster.DefectTime, DefectMaster.DefectDesc, TemplateName.TemplateId, TemplateName.Name, DefectMaster.DataNo, DefectMaster.EmployeeNo, DefectMaster.AQL, DefectMaster.InspectionId, DefectMaster.TotalLotPieces, DefectMaster.Product, DefectMaster.DefectClass, DefectMaster.WorkOrder, DefectMaster.RollNo, DefectMaster.LoomNo, DefectMaster.DataType, DefectMaster.DefectImage_Filename, DefectMaster.Inspector, DefectMaster.InspectionState, DefectMaster.ItemNumber, DefectMaster.WorkRoom" & vbCrLf &
             "FROM DefectMaster INNER JOIN  TemplateName ON DefectMaster.TemplateId = TemplateName.TemplateId" & vbCrLf &
             "WHERE (DefectTime >= CONVERT(DATETIME, '" & fromdatestring & "', 102)) AND (DefectTime <= CONVERT(DATETIME, '" & todatestring & "', 102)) AND (Location = '" + Location + "' OR SUBSTRING(DataType, 0, 4) = 'STT')" & vbCrLf &
              "ORDER BY DefectMaster.DefectID DESC"
            Else
                SQL = "SELECT DefectMaster.DefectID, DefectMaster.DefectTime, DefectMaster.DefectDesc, TemplateName.TemplateId, TemplateName.Name, DefectMaster.DataNo, DefectMaster.EmployeeNo, DefectMaster.AQL, DefectMaster.InspectionId, DefectMaster.TotalLotPieces, DefectMaster.Product, DefectMaster.DefectClass, DefectMaster.WorkOrder, DefectMaster.RollNo, DefectMaster.LoomNo, DefectMaster.DataType, DefectMaster.DefectImage_Filename, DefectMaster.Inspector, DefectMaster.InspectionState, DefectMaster.ItemNumber, DefectMaster.WorkRoom" & vbCrLf &
             "FROM DefectMaster INNER JOIN  TemplateName ON DefectMaster.TemplateId = TemplateName.TemplateId" & vbCrLf &
             "WHERE (DefectTime >= CONVERT(DATETIME, '" & fromdatestring & "', 102)) AND (DefectTime <= CONVERT(DATETIME, '" & todatestring & "', 102)) AND (Location = '" + Location + "')" & vbCrLf &
              "ORDER BY DefectMaster.DefectID DESC"
            End If

            Try

                returnlist2 = bmap.GetInspectObject(SQL)
            Catch ex As Exception

            End Try

            Return returnlist2

        End Function

        Public Function GetDefectMasterData3(ByVal fromdate As DateTime, ByVal todate As DateTime, Optional ByVal filterlist As List(Of ActiveFilterObject) = Nothing) As List(Of SPCInspection.DefectMasterDisplay)
            Dim SQL As String
            Dim returnlist As New List(Of SPCInspection.DefectMasterDisplay)
            Dim returnlist2 As New List(Of SPCInspection.DefectMasterDisplay)
            Dim fromdatestring As String = fromdate.ToString("yyyy-MM-dd H:mm:ss").Split(" ")(0) + " 00:00:00"
            Dim todatestring As String = todate.ToString("yyyy-MM-dd H:mm:ss").Split(" ")(0) + " 23:59:59"
            Dim bmap As New BMappers(Of SPCInspection.DefectMasterDisplay)
            Dim datanosqlstring As String = ""
            Dim audittypesqlstring As String = ""

            If IsNothing(filterlist) = False Then
                If filterlist.Count > 0 Then
                    Select Case filterlist.ToArray()(0).Name
                        Case "pf_AuditType"
                            audittypesqlstring = " AND (it.Name = '" & filterlist.ToArray()(0).value.ToString() & "') "
                        Case "pf_DataNumber"
                            datanosqlstring = " AND (DefectMaster.DataNo = '" & filterlist.ToArray()(0).value.ToString() & "') "
                    End Select
                End If
            End If


            SQL = "SELECT DefectMaster.DefectID, DefectMaster.DefectTime, DefectMaster.DefectDesc, TemplateName.TemplateId, TemplateName.Name, ijs.DataNo, DefectMaster.EmployeeNo, ijs.AQL_Level AS AQL, DefectMaster.InspectionId, DefectMaster.TotalLotPieces, DefectMaster.Product, DefectMaster.DefectClass, DefectMaster.WorkOrder, DefectMaster.RollNo, DefectMaster.LoomNo, DefectMaster.DataType, DefectMaster.DefectImage_Filename, DefectMaster.Inspector, DefectMaster.InspectionState, DefectMaster.ItemNumber, DefectMaster.WorkRoom, DefectMaster.Location" & vbCrLf &
             "FROM DefectMaster INNER JOIN  TemplateName ON DefectMaster.TemplateId = TemplateName.TemplateId LEFT OUTER JOIN InspectionTypes it ON it.id = TemplateName.LineTypeId INNER JOIN InspectionJobSummary ijs ON DefectMaster.InspectionJobSummaryId = ijs.id" & vbCrLf &
             "WHERE (DefectTime >= CONVERT(DATETIME, '" & fromdatestring & "', 102)) AND (DefectTime <= CONVERT(DATETIME, '" & todatestring & "', 102))" & audittypesqlstring & datanosqlstring & vbCrLf &
              "ORDER BY DefectMaster.DefectID DESC"

            Try
                returnlist2 = bmap.GetInspectObject(SQL)
            Catch ex As Exception

            End Try

            Return returnlist2

        End Function
        Public Function WRGetMonthlyDefectTotal(ByVal Facility As String, ByVal WR As String, ByVal WS As String) As Integer
            Dim retval As Integer = 0
            Dim DR2 As SqlDataReader
            Dim Command2 As New SqlCommand
            Dim Connection2 As New SqlConnection(ConfigurationManager.ConnectionStrings("MyDB").ConnectionString)
            Dim SQL As String = "Select SUM(MajorsCount+MinorsCount+RepairsCount+ScrapCount) AS TOTAL from Inspection.dbo.Locations inner join dbo.InspectionJobSummaryYearly on dbo.InspectionJobSummaryYearly.CID=NCID Where Name='" & Facility & "'" & WS & " AND WorkRoom='" & WR & "' And Inspection_Finished >= DATEADD(month,-1,GETDATE())"
            Command2.CommandType = CommandType.Text 'sets the type of the sql
            Command2.Connection = Connection2 'sets the connection of our sql command to MyDB
            Command2.CommandText = SQL 'sets the statement that executes at the data source to our string
            If (Connection2.State = ConnectionState.Closed) Then
                Connection2.Open()
            End If
            DR2 = Command2.ExecuteReader 'sends the command text to the connection and builds tthe SqlDataReader
            If DR2.HasRows = True Then 'Check whether the SqlDataReader has 1 or more rows
                DR2.Read() 'The default position of the SqlDataReader is before the first record. Therefore, you must call Read to begin accessing any data.  The reader has moved down to the first row.
                'Response.Write(DR("Cust_Name")) 'Writes a string to an HTTP response output stream.  In this case, just Cust_Name
                If Not DR2.IsDBNull(DR2.GetOrdinal("Total")) Then
                    retval = Convert.ToInt32(DR2("Total"))
                End If


            Else
                Return retval


            End If

            Connection2.Close() 'closes the connection
            DR2.Close() 'closes the reader
            Return retval
        End Function
        Public Function WRGetYearlyDefectTotal(ByVal Facility As String, ByVal WR As String, ByVal WS As String) As Integer
            Dim retval As Integer = 0
            Dim DR2 As SqlDataReader
            Dim Command2 As New SqlCommand
            Dim Connection2 As New SqlConnection(ConfigurationManager.ConnectionStrings("MyDB").ConnectionString)
            Dim SQL As String = "Select SUM(MajorsCount+MinorsCount+RepairsCount+ScrapCount) AS TOTAL from Inspection.dbo.Locations inner join dbo.InspectionJobSummaryYearly on dbo.InspectionJobSummaryYearly.CID=NCID Where Name='" & Facility & "'" & WS & " AND WorkRoom='" & WR & "'"
            Command2.CommandType = CommandType.Text 'sets the type of the sql
            Command2.Connection = Connection2 'sets the connection of our sql command to MyDB
            Command2.CommandText = SQL 'sets the statement that executes at the data source to our string
            If (Connection2.State = ConnectionState.Closed) Then
                Connection2.Open()
            End If
            DR2 = Command2.ExecuteReader 'sends the command text to the connection and builds tthe SqlDataReader
            If DR2.HasRows = True Then 'Check whether the SqlDataReader has 1 or more rows
                DR2.Read() 'The default position of the SqlDataReader is before the first record. Therefore, you must call Read to begin accessing any data.  The reader has moved down to the first row.
                'Response.Write(DR("Cust_Name")) 'Writes a string to an HTTP response output stream.  In this case, just Cust_Name

                If Not DR2.IsDBNull(DR2.GetOrdinal("Total")) Then
                    retval = Convert.ToInt32(DR2("Total"))
                End If

            Else
                Return retval


            End If

            Connection2.Close() 'closes the connection
            DR2.Close() 'closes the reader
            Return retval
        End Function
        Public Function WRGetCustomDefectTotal(ByVal Facility As String, ByVal WR As String, ByVal WS As String) As Integer
            Dim retval As Integer = 0
            Dim DR2 As SqlDataReader
            Dim Command2 As New SqlCommand
            Dim Connection2 As New SqlConnection(ConfigurationManager.ConnectionStrings("MyDB").ConnectionString)
            Dim SQL As String = "Select SUM(MajorsCount+MinorsCount+RepairsCount+ScrapCount) AS TOTAL from Inspection.dbo.Locations inner join dbo.InspectionJobSummaryYearly on dbo.InspectionJobSummaryYearly.CID=NCID Where Name='" & Facility & "' AND WorkRoom='" & WR & "'" & WS
            Command2.CommandType = CommandType.Text 'sets the type of the sql
            Command2.Connection = Connection2 'sets the connection of our sql command to MyDB
            Command2.CommandText = SQL 'sets the statement that executes at the data source to our string
            If (Connection2.State = ConnectionState.Closed) Then
                Connection2.Open()
            End If
            DR2 = Command2.ExecuteReader 'sends the command text to the connection and builds tthe SqlDataReader
            If DR2.HasRows = True Then 'Check whether the SqlDataReader has 1 or more rows
                DR2.Read() 'The default position of the SqlDataReader is before the first record. Therefore, you must call Read to begin accessing any data.  The reader has moved down to the first row.
                'Response.Write(DR("Cust_Name")) 'Writes a string to an HTTP response output stream.  In this case, just Cust_Name

                If Not DR2.IsDBNull(DR2.GetOrdinal("Total")) Then
                    retval = Convert.ToInt32(DR2("Total"))
                End If

            Else
                Return retval


            End If

            Connection2.Close() 'closes the connection
            DR2.Close() 'closes the reader
            Return retval
        End Function
        Public Function WRGetMonthlyInspectionTotal(ByVal Facility As String, ByVal WR As String, ByVal WS As String) As Integer
            Dim retval As Integer = 0
            Dim DR2 As SqlDataReader
            Dim Command2 As New SqlCommand
            Dim Connection2 As New SqlConnection(ConfigurationManager.ConnectionStrings("MyDB").ConnectionString)
            Dim SQL As String = "Select COUNT(InspectionJobSummaryYearly.id) AS TOTAL from Inspection.dbo.Locations inner join dbo.InspectionJobSummaryYearly on dbo.InspectionJobSummaryYearly.CID=NCID Where Name='" & Facility & "'" & WS & " AND WorkRoom='" & WR & "' And Inspection_Finished >= DATEADD(month,-1,GETDATE())"
            Command2.CommandType = CommandType.Text 'sets the type of the sql
            Command2.Connection = Connection2 'sets the connection of our sql command to MyDB
            Command2.CommandText = SQL 'sets the statement that executes at the data source to our string
            If (Connection2.State = ConnectionState.Closed) Then
                Connection2.Open()
            End If
            DR2 = Command2.ExecuteReader 'sends the command text to the connection and builds tthe SqlDataReader
            If DR2.HasRows = True Then 'Check whether the SqlDataReader has 1 or more rows
                DR2.Read() 'The default position of the SqlDataReader is before the first record. Therefore, you must call Read to begin accessing any data.  The reader has moved down to the first row.
                'Response.Write(DR("Cust_Name")) 'Writes a string to an HTTP response output stream.  In this case, just Cust_Name
                If Not DR2.IsDBNull(DR2.GetOrdinal("Total")) Then
                    retval = Convert.ToInt32(DR2("Total"))
                End If


            Else
                Return retval


            End If

            Connection2.Close() 'closes the connection
            DR2.Close() 'closes the reader
            Return retval
        End Function
        Public Function WRGetYearlyInspectionTotal(ByVal Facility As String, ByVal WR As String, ByVal WS As String) As Integer
            Dim retval As Integer = 0
            Dim DR2 As SqlDataReader
            Dim Command2 As New SqlCommand
            Dim Connection2 As New SqlConnection(ConfigurationManager.ConnectionStrings("MyDB").ConnectionString)
            Dim SQL As String = "Select COUNT(InspectionJobSummaryYearly.id) AS TOTAL from Inspection.dbo.Locations inner join dbo.InspectionJobSummaryYearly on dbo.InspectionJobSummaryYearly.CID=NCID Where Name='" & Facility & "'" & WS & " AND WorkRoom='" & WR & "'"
            Command2.CommandType = CommandType.Text 'sets the type of the sql
            Command2.Connection = Connection2 'sets the connection of our sql command to MyDB
            Command2.CommandText = SQL 'sets the statement that executes at the data source to our string
            If (Connection2.State = ConnectionState.Closed) Then
                Connection2.Open()
            End If
            DR2 = Command2.ExecuteReader 'sends the command text to the connection and builds tthe SqlDataReader
            If DR2.HasRows = True Then 'Check whether the SqlDataReader has 1 or more rows
                DR2.Read() 'The default position of the SqlDataReader is before the first record. Therefore, you must call Read to begin accessing any data.  The reader has moved down to the first row.
                'Response.Write(DR("Cust_Name")) 'Writes a string to an HTTP response output stream.  In this case, just Cust_Name

                If Not DR2.IsDBNull(DR2.GetOrdinal("Total")) Then
                    retval = Convert.ToInt32(DR2("Total"))
                End If

            Else
                Return retval


            End If

            Connection2.Close() 'closes the connection
            DR2.Close() 'closes the reader
            Return retval
        End Function

        Public Function WRGetCustomInspectionTotal(ByVal Facility As String, ByVal WR As String, ByVal WS As String) As Integer
            Dim retval As Integer = 0
            Dim DR2 As SqlDataReader
            Dim Command2 As New SqlCommand
            Dim Connection2 As New SqlConnection(ConfigurationManager.ConnectionStrings("MyDB").ConnectionString)
            Dim SQL As String = "Select COUNT(InspectionJobSummaryYearly.id) AS TOTAL from Inspection.dbo.Locations inner join dbo.InspectionJobSummaryYearly on dbo.InspectionJobSummaryYearly.CID=NCID Where Name='" & Facility & "' AND WorkRoom='" & WR & "'" & WS
            Command2.CommandType = CommandType.Text 'sets the type of the sql
            Command2.Connection = Connection2 'sets the connection of our sql command to MyDB
            Command2.CommandText = SQL 'sets the statement that executes at the data source to our string
            If (Connection2.State = ConnectionState.Closed) Then
                Connection2.Open()
            End If
            DR2 = Command2.ExecuteReader 'sends the command text to the connection and builds tthe SqlDataReader
            If DR2.HasRows = True Then 'Check whether the SqlDataReader has 1 or more rows
                DR2.Read() 'The default position of the SqlDataReader is before the first record. Therefore, you must call Read to begin accessing any data.  The reader has moved down to the first row.
                'Response.Write(DR("Cust_Name")) 'Writes a string to an HTTP response output stream.  In this case, just Cust_Name

                If Not DR2.IsDBNull(DR2.GetOrdinal("Total")) Then
                    retval = Convert.ToInt32(DR2("Total"))
                End If

            Else
                Return retval


            End If

            Connection2.Close() 'closes the connection
            DR2.Close() 'closes the reader
            Return retval
        End Function
        Public Function WRGetMonthlyRejectTotal(ByVal Facility As String, ByVal WR As String, ByVal WS As String) As Integer
            Dim retval As Integer = 0
            Dim DR2 As SqlDataReader
            Dim Command2 As New SqlCommand
            Dim Connection2 As New SqlConnection(ConfigurationManager.ConnectionStrings("MyDB").ConnectionString)
            Dim SQL As String = "Select SUM(MajorsCount+RepairsCount+ScrapCount) AS TOTAL from Inspection.dbo.Locations inner join dbo.InspectionJobSummaryYearly on dbo.InspectionJobSummaryYearly.CID=NCID Where Name='" & Facility & "'" & WS & " AND WorkRoom='" & WR & "' And Inspection_Finished >= DATEADD(month,-1,GETDATE())"
            Command2.CommandType = CommandType.Text 'sets the type of the sql
            Command2.Connection = Connection2 'sets the connection of our sql command to MyDB
            Command2.CommandText = SQL 'sets the statement that executes at the data source to our string
            If (Connection2.State = ConnectionState.Closed) Then
                Connection2.Open()
            End If
            DR2 = Command2.ExecuteReader 'sends the command text to the connection and builds tthe SqlDataReader
            If DR2.HasRows = True Then 'Check whether the SqlDataReader has 1 or more rows
                DR2.Read() 'The default position of the SqlDataReader is before the first record. Therefore, you must call Read to begin accessing any data.  The reader has moved down to the first row.
                'Response.Write(DR("Cust_Name")) 'Writes a string to an HTTP response output stream.  In this case, just Cust_Name
                If Not DR2.IsDBNull(DR2.GetOrdinal("Total")) Then
                    retval = Convert.ToInt32(DR2("Total"))
                End If


            Else
                Return retval


            End If

            Connection2.Close() 'closes the connection
            DR2.Close() 'closes the reader
            Return retval
        End Function
        Public Function WRGetYearlyRejectTotal(ByVal Facility As String, ByVal WR As String, ByVal WS As String) As Integer
            Dim retval As Integer = 0
            Dim DR2 As SqlDataReader
            Dim Command2 As New SqlCommand
            Dim Connection2 As New SqlConnection(ConfigurationManager.ConnectionStrings("MyDB").ConnectionString)
            Dim SQL As String = "Select SUM(MajorsCount+RepairsCount+ScrapCount) AS TOTAL from Inspection.dbo.Locations inner join dbo.InspectionJobSummaryYearly on dbo.InspectionJobSummaryYearly.CID=NCID Where Name='" & Facility & "'" & WS & " AND WorkRoom='" & WR & "'"
            Command2.CommandType = CommandType.Text 'sets the type of the sql
            Command2.Connection = Connection2 'sets the connection of our sql command to MyDB
            Command2.CommandText = SQL 'sets the statement that executes at the data source to our string
            If (Connection2.State = ConnectionState.Closed) Then
                Connection2.Open()
            End If
            DR2 = Command2.ExecuteReader 'sends the command text to the connection and builds tthe SqlDataReader
            If DR2.HasRows = True Then 'Check whether the SqlDataReader has 1 or more rows
                DR2.Read() 'The default position of the SqlDataReader is before the first record. Therefore, you must call Read to begin accessing any data.  The reader has moved down to the first row.
                'Response.Write(DR("Cust_Name")) 'Writes a string to an HTTP response output stream.  In this case, just Cust_Name

                If Not DR2.IsDBNull(DR2.GetOrdinal("Total")) Then
                    retval = Convert.ToInt32(DR2("Total"))
                End If

            Else
                Return retval


            End If

            Connection2.Close() 'closes the connection
            DR2.Close() 'closes the reader
            Return retval
        End Function

        Public Function WRGetCustomRejectTotal(ByVal Facility As String, ByVal WR As String, ByVal WS As String) As Integer
            Dim retval As Integer = 0
            Dim DR2 As SqlDataReader
            Dim Command2 As New SqlCommand
            Dim Connection2 As New SqlConnection(ConfigurationManager.ConnectionStrings("MyDB").ConnectionString)
            Dim SQL As String = "Select SUM(MajorsCount+RepairsCount+ScrapCount) AS TOTAL from Inspection.dbo.Locations inner join dbo.InspectionJobSummaryYearly on dbo.InspectionJobSummaryYearly.CID=NCID Where Name='" & Facility & "' AND WorkRoom='" & WR & "'" & WS
            Command2.CommandType = CommandType.Text 'sets the type of the sql
            Command2.Connection = Connection2 'sets the connection of our sql command to MyDB
            Command2.CommandText = SQL 'sets the statement that executes at the data source to our string
            If (Connection2.State = ConnectionState.Closed) Then
                Connection2.Open()
            End If
            DR2 = Command2.ExecuteReader 'sends the command text to the connection and builds tthe SqlDataReader
            If DR2.HasRows = True Then 'Check whether the SqlDataReader has 1 or more rows
                DR2.Read() 'The default position of the SqlDataReader is before the first record. Therefore, you must call Read to begin accessing any data.  The reader has moved down to the first row.
                'Response.Write(DR("Cust_Name")) 'Writes a string to an HTTP response output stream.  In this case, just Cust_Name

                If Not DR2.IsDBNull(DR2.GetOrdinal("Total")) Then
                    retval = Convert.ToInt32(DR2("Total"))
                End If

            Else
                Return retval


            End If

            Connection2.Close() 'closes the connection
            DR2.Close() 'closes the reader
            Return retval
        End Function
        Public Function WRGetMonthlyRejectLotTotal(ByVal Facility As String, ByVal WR As String, ByVal WS As String) As Integer
            Dim retval As Integer = 0
            Dim DR2 As SqlDataReader
            Dim Command2 As New SqlCommand
            Dim Connection2 As New SqlConnection(ConfigurationManager.ConnectionStrings("MyDB").ConnectionString)
            Dim SQL As String = "Select COUNT(InspectionJobSummaryYearly.id) AS TOTAL from Inspection.dbo.Locations inner join dbo.InspectionJobSummaryYearly on dbo.InspectionJobSummaryYearly.CID=NCID Where Name='" & Facility & "'" & WS & " AND WorkRoom='" & WR & "' And Technical_PassFail = 0 And Inspection_Finished >= DATEADD(month,-1,GETDATE())"
            Command2.CommandType = CommandType.Text 'sets the type of the sql
            Command2.Connection = Connection2 'sets the connection of our sql command to MyDB
            Command2.CommandText = SQL 'sets the statement that executes at the data source to our string
            If (Connection2.State = ConnectionState.Closed) Then
                Connection2.Open()
            End If
            DR2 = Command2.ExecuteReader 'sends the command text to the connection and builds tthe SqlDataReader
            If DR2.HasRows = True Then 'Check whether the SqlDataReader has 1 or more rows
                DR2.Read() 'The default position of the SqlDataReader is before the first record. Therefore, you must call Read to begin accessing any data.  The reader has moved down to the first row.
                'Response.Write(DR("Cust_Name")) 'Writes a string to an HTTP response output stream.  In this case, just Cust_Name
                If Not DR2.IsDBNull(DR2.GetOrdinal("Total")) Then
                    retval = Convert.ToInt32(DR2("Total"))
                End If


            Else
                Return retval


            End If

            Connection2.Close() 'closes the connection
            DR2.Close() 'closes the reader
            Return retval
        End Function
        Public Function WRGetYearlyRejectLotTotal(ByVal Facility As String, ByVal WR As String, ByVal WS As String) As Integer
            Dim retval As Double = 0
            Dim DR2 As SqlDataReader
            Dim Command2 As New SqlCommand
            Dim Connection2 As New SqlConnection(ConfigurationManager.ConnectionStrings("MyDB").ConnectionString)
            Dim SQL As String = "Select COUNT(InspectionJobSummaryYearly.id) AS TOTAL from Inspection.dbo.Locations inner join dbo.InspectionJobSummaryYearly on dbo.InspectionJobSummaryYearly.CID=NCID Where Name='" & Facility & "'" & WS & " AND WorkRoom='" & WR & "' And Technical_PassFail = 0"
            Command2.CommandType = CommandType.Text 'sets the type of the sql
            Command2.Connection = Connection2 'sets the connection of our sql command to MyDB
            Command2.CommandText = SQL 'sets the statement that executes at the data source to our string
            If (Connection2.State = ConnectionState.Closed) Then
                Connection2.Open()
            End If
            DR2 = Command2.ExecuteReader 'sends the command text to the connection and builds tthe SqlDataReader
            If DR2.HasRows = True Then 'Check whether the SqlDataReader has 1 or more rows
                DR2.Read() 'The default position of the SqlDataReader is before the first record. Therefore, you must call Read to begin accessing any data.  The reader has moved down to the first row.
                'Response.Write(DR("Cust_Name")) 'Writes a string to an HTTP response output stream.  In this case, just Cust_Name

                If Not DR2.IsDBNull(DR2.GetOrdinal("Total")) Then
                    retval = Convert.ToInt32(DR2("Total"))
                End If

            Else
                Return retval


            End If

            Connection2.Close() 'closes the connection
            DR2.Close() 'closes the reader
            Return retval
        End Function
        Public Function WRGetCustomRejectLotTotal(ByVal Facility As String, ByVal WR As String, ByVal WS As String) As Integer
            Dim retval As Double = 0
            Dim DR2 As SqlDataReader
            Dim Command2 As New SqlCommand
            Dim Connection2 As New SqlConnection(ConfigurationManager.ConnectionStrings("MyDB").ConnectionString)
            Dim SQL As String = "Select COUNT(InspectionJobSummaryYearly.id) AS TOTAL from Inspection.dbo.Locations inner join dbo.InspectionJobSummaryYearly on dbo.InspectionJobSummaryYearly.CID=NCID Where Name='" & Facility & "' AND WorkRoom='" & WR & "' And Technical_PassFail = 0" & WS
            Command2.CommandType = CommandType.Text 'sets the type of the sql
            Command2.Connection = Connection2 'sets the connection of our sql command to MyDB
            Command2.CommandText = SQL 'sets the statement that executes at the data source to our string
            If (Connection2.State = ConnectionState.Closed) Then
                Connection2.Open()
            End If
            DR2 = Command2.ExecuteReader 'sends the command text to the connection and builds tthe SqlDataReader
            If DR2.HasRows = True Then 'Check whether the SqlDataReader has 1 or more rows
                DR2.Read() 'The default position of the SqlDataReader is before the first record. Therefore, you must call Read to begin accessing any data.  The reader has moved down to the first row.
                'Response.Write(DR("Cust_Name")) 'Writes a string to an HTTP response output stream.  In this case, just Cust_Name

                If Not DR2.IsDBNull(DR2.GetOrdinal("Total")) Then
                    retval = Convert.ToInt32(DR2("Total"))
                End If

            Else
                Return retval


            End If

            Connection2.Close() 'closes the connection
            DR2.Close() 'closes the reader
            Return retval
        End Function
        Public Function WRGetMonthlyDHU(ByVal Facility As String, ByVal WR As String, ByVal WS As String) As Double
            Dim retval As Integer = 0
            Dim DR2 As SqlDataReader
            Dim Command2 As New SqlCommand
            Dim Connection2 As New SqlConnection(ConfigurationManager.ConnectionStrings("MyDB").ConnectionString)
            Dim SQL As String = "Select (CAST(SUM(MajorsCount+MinorsCount+RepairsCount+ScrapCount) AS Decimal(10,2))/CAST(SUM(SampleSize) AS Decimal(10,2)))*100 AS TOTAL from Inspection.dbo.Locations inner join dbo.InspectionJobSummaryYearly on dbo.InspectionJobSummaryYearly.CID=NCID Where Name='" & Facility & "'" & WS & " AND WorkRoom='" & WR & "' And Inspection_Finished >= DATEADD(month,-1,GETDATE())"
            Command2.CommandType = CommandType.Text 'sets the type of the sql
            Command2.Connection = Connection2 'sets the connection of our sql command to MyDB
            Command2.CommandText = SQL 'sets the statement that executes at the data source to our string
            If (Connection2.State = ConnectionState.Closed) Then
                Connection2.Open()
            End If
            DR2 = Command2.ExecuteReader 'sends the command text to the connection and builds tthe SqlDataReader
            If DR2.HasRows = True Then 'Check whether the SqlDataReader has 1 or more rows
                DR2.Read() 'The default position of the SqlDataReader is before the first record. Therefore, you must call Read to begin accessing any data.  The reader has moved down to the first row.
                'Response.Write(DR("Cust_Name")) 'Writes a string to an HTTP response output stream.  In this case, just Cust_Name
                If Not DR2.IsDBNull(DR2.GetOrdinal("Total")) Then
                    retval = Math.Round(Convert.ToDouble(DR2("Total")), 2)
                End If


            Else
                Return retval


            End If

            Connection2.Close() 'closes the connection
            DR2.Close() 'closes the reader
            Return retval
        End Function
        Public Function WRGetYearlyDHU(ByVal Facility As String, ByVal WR As String, ByVal WS As String) As Double
            Dim retval As Double = 0
            Dim DR2 As SqlDataReader
            Dim Command2 As New SqlCommand
            Dim Connection2 As New SqlConnection(ConfigurationManager.ConnectionStrings("MyDB").ConnectionString)
            Dim SQL As String = "Select (CAST(SUM(MajorsCount+MinorsCount+RepairsCount+ScrapCount) AS Decimal(10,2))/CAST(SUM(SampleSize) AS Decimal(10,2)))*100 AS TOTAL from Inspection.dbo.Locations inner join dbo.InspectionJobSummaryYearly on dbo.InspectionJobSummaryYearly.CID=NCID Where Name='" & Facility & "'" & WS & " AND WorkRoom='" & WR & "'"
            Command2.CommandType = CommandType.Text 'sets the type of the sql
            Command2.Connection = Connection2 'sets the connection of our sql command to MyDB
            Command2.CommandText = SQL 'sets the statement that executes at the data source to our string
            If (Connection2.State = ConnectionState.Closed) Then
                Connection2.Open()
            End If
            DR2 = Command2.ExecuteReader 'sends the command text to the connection and builds tthe SqlDataReader
            If DR2.HasRows = True Then 'Check whether the SqlDataReader has 1 or more rows
                DR2.Read() 'The default position of the SqlDataReader is before the first record. Therefore, you must call Read to begin accessing any data.  The reader has moved down to the first row.
                'Response.Write(DR("Cust_Name")) 'Writes a string to an HTTP response output stream.  In this case, just Cust_Name

                If Not DR2.IsDBNull(DR2.GetOrdinal("Total")) Then
                    retval = Math.Round(Convert.ToDouble(DR2("Total")), 2)
                End If

            Else
                Return retval


            End If

            Connection2.Close() 'closes the connection
            DR2.Close() 'closes the reader
            Return retval
        End Function
        Public Function WRGetCustomDHU(ByVal Facility As String, ByVal WR As String, ByVal WS As String) As Double
            Dim retval As Double = 0
            Dim DR2 As SqlDataReader
            Dim Command2 As New SqlCommand
            Dim Connection2 As New SqlConnection(ConfigurationManager.ConnectionStrings("MyDB").ConnectionString)
            Dim SQL As String = "Select (CAST(SUM(MajorsCount+MinorsCount+RepairsCount+ScrapCount) AS Decimal(10,2))/CAST(SUM(SampleSize) AS Decimal(10,2)))*100 AS TOTAL from Inspection.dbo.Locations inner join dbo.InspectionJobSummaryYearly on dbo.InspectionJobSummaryYearly.CID=NCID Where Name='" & Facility & "' AND WorkRoom='" & WR & "'" & WS
            Command2.CommandType = CommandType.Text 'sets the type of the sql
            Command2.Connection = Connection2 'sets the connection of our sql command to MyDB
            Command2.CommandText = SQL 'sets the statement that executes at the data source to our string
            If (Connection2.State = ConnectionState.Closed) Then
                Connection2.Open()
            End If
            DR2 = Command2.ExecuteReader 'sends the command text to the connection and builds tthe SqlDataReader
            If DR2.HasRows = True Then 'Check whether the SqlDataReader has 1 or more rows
                DR2.Read() 'The default position of the SqlDataReader is before the first record. Therefore, you must call Read to begin accessing any data.  The reader has moved down to the first row.
                'Response.Write(DR("Cust_Name")) 'Writes a string to an HTTP response output stream.  In this case, just Cust_Name

                If Not DR2.IsDBNull(DR2.GetOrdinal("Total")) Then
                    retval = Math.Round(Convert.ToDouble(DR2("Total")), 2)
                End If

            Else
                Return retval


            End If

            Connection2.Close() 'closes the connection
            DR2.Close() 'closes the reader
            Return retval
        End Function
        Public Function WRGetMonthlyRejectionRate(ByVal Facility As String, ByVal WR As String, ByVal WS As String) As Double
            Dim retval As Integer = 0
            Dim DR2 As SqlDataReader
            Dim Command2 As New SqlCommand
            Dim Connection2 As New SqlConnection(ConfigurationManager.ConnectionStrings("MyDB").ConnectionString)
            Dim SQL As String = "Select (CAST(SUM(MajorsCount+RepairsCount+ScrapCount) AS Decimal(10,2))/CAST(SUM(SampleSize) AS Decimal(10,2)))*100 AS TOTAL from Inspection.dbo.Locations inner join dbo.InspectionJobSummaryYearly on dbo.InspectionJobSummaryYearly.CID=NCID Where Name='" & Facility & "'" & WS & " AND WorkRoom='" & WR & "' And Inspection_Finished >= DATEADD(month,-1,GETDATE())"
            Command2.CommandType = CommandType.Text 'sets the type of the sql
            Command2.Connection = Connection2 'sets the connection of our sql command to MyDB
            Command2.CommandText = SQL 'sets the statement that executes at the data source to our string
            If (Connection2.State = ConnectionState.Closed) Then
                Connection2.Open()
            End If
            DR2 = Command2.ExecuteReader 'sends the command text to the connection and builds tthe SqlDataReader
            If DR2.HasRows = True Then 'Check whether the SqlDataReader has 1 or more rows
                DR2.Read() 'The default position of the SqlDataReader is before the first record. Therefore, you must call Read to begin accessing any data.  The reader has moved down to the first row.
                'Response.Write(DR("Cust_Name")) 'Writes a string to an HTTP response output stream.  In this case, just Cust_Name
                If Not DR2.IsDBNull(DR2.GetOrdinal("Total")) Then
                    retval = Math.Round(Convert.ToDouble(DR2("Total")), 2)
                End If


            Else
                Return retval


            End If

            Connection2.Close() 'closes the connection
            DR2.Close() 'closes the reader
            Return retval
        End Function
        Public Function WRGetYearlyRejectionRate(ByVal Facility As String, ByVal WR As String, ByVal WS As String) As Double
            Dim retval As Double = 0
            Dim DR2 As SqlDataReader
            Dim Command2 As New SqlCommand
            Dim Connection2 As New SqlConnection(ConfigurationManager.ConnectionStrings("MyDB").ConnectionString)
            Dim SQL As String = "Select (CAST(SUM(MajorsCount+RepairsCount+ScrapCount) AS Decimal(10,2))/CAST(SUM(SampleSize) AS Decimal(10,2)))*100 AS TOTAL from Inspection.dbo.Locations inner join dbo.InspectionJobSummaryYearly on dbo.InspectionJobSummaryYearly.CID=NCID Where Name='" & Facility & "'" & WS & " AND WorkRoom='" & WR & "'"
            Command2.CommandType = CommandType.Text 'sets the type of the sql
            Command2.Connection = Connection2 'sets the connection of our sql command to MyDB
            Command2.CommandText = SQL 'sets the statement that executes at the data source to our string
            If (Connection2.State = ConnectionState.Closed) Then
                Connection2.Open()
            End If
            DR2 = Command2.ExecuteReader 'sends the command text to the connection and builds tthe SqlDataReader
            If DR2.HasRows = True Then 'Check whether the SqlDataReader has 1 or more rows
                DR2.Read() 'The default position of the SqlDataReader is before the first record. Therefore, you must call Read to begin accessing any data.  The reader has moved down to the first row.
                'Response.Write(DR("Cust_Name")) 'Writes a string to an HTTP response output stream.  In this case, just Cust_Name

                If Not DR2.IsDBNull(DR2.GetOrdinal("Total")) Then
                    retval = Math.Round(Convert.ToDouble(DR2("Total")), 2)
                End If

            Else
                Return retval


            End If

            Connection2.Close() 'closes the connection
            DR2.Close() 'closes the reader
            Return retval
        End Function
        Public Function WRGetCustomRejectionRate(ByVal Facility As String, ByVal WR As String, ByVal WS As String) As Double
            Dim retval As Double = 0
            Dim DR2 As SqlDataReader
            Dim Command2 As New SqlCommand
            Dim Connection2 As New SqlConnection(ConfigurationManager.ConnectionStrings("MyDB").ConnectionString)
            Dim SQL As String = "Select (CAST(SUM(MajorsCount+RepairsCount+ScrapCount) AS Decimal(10,2))/CAST(SUM(SampleSize) AS Decimal(10,2)))*100 AS TOTAL from Inspection.dbo.Locations inner join dbo.InspectionJobSummaryYearly on dbo.InspectionJobSummaryYearly.CID=NCID Where Name='" & Facility & "' AND WorkRoom='" & WR & "'" & WS
            Command2.CommandType = CommandType.Text 'sets the type of the sql
            Command2.Connection = Connection2 'sets the connection of our sql command to MyDB
            Command2.CommandText = SQL 'sets the statement that executes at the data source to our string
            If (Connection2.State = ConnectionState.Closed) Then
                Connection2.Open()
            End If
            DR2 = Command2.ExecuteReader 'sends the command text to the connection and builds tthe SqlDataReader
            If DR2.HasRows = True Then 'Check whether the SqlDataReader has 1 or more rows
                DR2.Read() 'The default position of the SqlDataReader is before the first record. Therefore, you must call Read to begin accessing any data.  The reader has moved down to the first row.
                'Response.Write(DR("Cust_Name")) 'Writes a string to an HTTP response output stream.  In this case, just Cust_Name

                If Not DR2.IsDBNull(DR2.GetOrdinal("Total")) Then
                    retval = Math.Round(Convert.ToDouble(DR2("Total")), 2)
                End If

            Else
                Return retval


            End If

            Connection2.Close() 'closes the connection
            DR2.Close() 'closes the reader
            Return retval
        End Function
        Public Function WRGetMonthlyLotAcceptance(ByVal Facility As String, ByVal WR As String, ByVal WS As String) As String
            Dim retval As String = "'0.00%'"
            Dim DR2 As SqlDataReader
            Dim Command2 As New SqlCommand
            Dim Connection2 As New SqlConnection(ConfigurationManager.ConnectionStrings("MyDB").ConnectionString)
            Dim SQL As String = "Select Top (1) (CAST( (Select COUNT(InspectionJobSummaryYearly.id) from Inspection.dbo.Locations inner join dbo.InspectionJobSummaryYearly on dbo.InspectionJobSummaryYearly.CID=NCID Where Name='" & Facility & "'" & WS & " AND WorkRoom='" & WR & "' AND Technical_PassFail=1 AND Inspection_Finished >= DATEADD(month,-1,GETDATE())) AS Decimal(10,2))/CAST((Select COUNT(InspectionJobSummaryYearly.id) from Inspection.dbo.Locations inner join dbo.InspectionJobSummaryYearly on dbo.InspectionJobSummaryYearly.CID=NCID Where Name='" & Facility & "'" & WS & " AND WorkRoom='" & WR & "' AND Inspection_Finished >= DATEADD(month,-1,GETDATE())) AS Decimal(10,2)))*100 AS TOTAL from dbo.InspectionJobSummaryYearly"
            Command2.CommandType = CommandType.Text 'sets the type of the sql
            Command2.Connection = Connection2 'sets the connection of our sql command to MyDB
            Command2.CommandText = SQL 'sets the statement that executes at the data source to our string
            If (Connection2.State = ConnectionState.Closed) Then
                Connection2.Open()
            End If
            DR2 = Command2.ExecuteReader 'sends the command text to the connection and builds tthe SqlDataReader
            If DR2.HasRows = True Then 'Check whether the SqlDataReader has 1 or more rows
                DR2.Read() 'The default position of the SqlDataReader is before the first record. Therefore, you must call Read to begin accessing any data.  The reader has moved down to the first row.
                'Response.Write(DR("Cust_Name")) 'Writes a string to an HTTP response output stream.  In this case, just Cust_Name
                If Not DR2.IsDBNull(DR2.GetOrdinal("Total")) Then
                    retval = "'" + Convert.ToString(Math.Round(Convert.ToDouble(DR2("Total")), 2)) + "%'"
                End If


            Else
                Return retval


            End If

            Connection2.Close() 'closes the connection
            DR2.Close() 'closes the reader
            Return retval
        End Function
        Public Function WRGetYearlyLotAcceptance(ByVal Facility As String, ByVal WR As String, ByVal WS As String) As String
            Dim retval As String = "'0.00%'"
            Dim DR2 As SqlDataReader
            Dim Command2 As New SqlCommand
            Dim Connection2 As New SqlConnection(ConfigurationManager.ConnectionStrings("MyDB").ConnectionString)
            Dim SQL As String = "Select Top (1) (CAST( (Select COUNT(InspectionJobSummaryYearly.id) from Inspection.dbo.Locations inner join dbo.InspectionJobSummaryYearly on dbo.InspectionJobSummaryYearly.CID=NCID Where Name='" & Facility & "'" & WS & " AND WorkRoom='" & WR & "' AND Technical_PassFail=1 ) AS Decimal(10,2))/CAST((Select COUNT(InspectionJobSummaryYearly.id) from Inspection.dbo.Locations inner join dbo.InspectionJobSummaryYearly on dbo.InspectionJobSummaryYearly.CID=NCID Where Name='" & Facility & "'" & WS & " AND WorkRoom='" & WR & "') AS Decimal(10,2)))*100 AS TOTAL from dbo.InspectionJobSummaryYearly"
            Command2.CommandType = CommandType.Text 'sets the type of the sql
            Command2.Connection = Connection2 'sets the connection of our sql command to MyDB
            Command2.CommandText = SQL 'sets the statement that executes at the data source to our string
            If (Connection2.State = ConnectionState.Closed) Then
                Connection2.Open()
            End If
            DR2 = Command2.ExecuteReader 'sends the command text to the connection and builds tthe SqlDataReader
            If DR2.HasRows = True Then 'Check whether the SqlDataReader has 1 or more rows
                DR2.Read() 'The default position of the SqlDataReader is before the first record. Therefore, you must call Read to begin accessing any data.  The reader has moved down to the first row.
                'Response.Write(DR("Cust_Name")) 'Writes a string to an HTTP response output stream.  In this case, just Cust_Name

                If Not DR2.IsDBNull(DR2.GetOrdinal("Total")) Then
                    retval = "'" + Convert.ToString(Math.Round(Convert.ToDouble(DR2("Total")), 2)) + "%'"
                End If

            Else
                Return retval


            End If

            Connection2.Close() 'closes the connection
            DR2.Close() 'closes the reader
            Return retval
        End Function
        Public Function WRGetCustomLotAcceptance(ByVal Facility As String, ByVal WR As String, ByVal WS As String) As String
            Dim retval As String = "'0.00%'"
            Dim DR2 As SqlDataReader
            Dim Command2 As New SqlCommand
            Dim Connection2 As New SqlConnection(ConfigurationManager.ConnectionStrings("MyDB").ConnectionString)

            Dim SQL As String = "Select Top (1) (CAST( (Select COUNT(InspectionJobSummaryYearly.id) from Inspection.dbo.Locations inner join dbo.InspectionJobSummaryYearly on dbo.InspectionJobSummaryYearly.CID=NCID Where Name='" & Facility & "' AND WorkRoom='" & WR & "' AND Technical_PassFail=1" & WS & ") AS Decimal(10,2))/CAST((Select COUNT(InspectionJobSummaryYearly.id) from Inspection.dbo.Locations inner join dbo.InspectionJobSummaryYearly on dbo.InspectionJobSummaryYearly.CID=NCID Where Name='" & Facility & "' AND WorkRoom='" & WR & "'" & WS & ") AS Decimal(10,2)))*100 AS TOTAL from dbo.InspectionJobSummaryYearly"
            Command2.CommandType = CommandType.Text 'sets the type of the sql
            Command2.Connection = Connection2 'sets the connection of our sql command to MyDB
            Command2.CommandText = SQL 'sets the statement that executes at the data source to our string
            If (Connection2.State = ConnectionState.Closed) Then
                Connection2.Open()
            End If
            DR2 = Command2.ExecuteReader 'sends the command text to the connection and builds tthe SqlDataReader
            If DR2.HasRows = True Then 'Check whether the SqlDataReader has 1 or more rows
                DR2.Read() 'The default position of the SqlDataReader is before the first record. Therefore, you must call Read to begin accessing any data.  The reader has moved down to the first row.
                'Response.Write(DR("Cust_Name")) 'Writes a string to an HTTP response output stream.  In this case, just Cust_Name

                If Not DR2.IsDBNull(DR2.GetOrdinal("Total")) Then
                    retval = "'" + Convert.ToString(Math.Round(Convert.ToDouble(DR2("Total")), 2)) + "%'"
                End If

            Else
                Return retval


            End If

            Connection2.Close() 'closes the connection
            DR2.Close() 'closes the reader
            Return retval
        End Function
        Public Function GetMainGridSubGrid(ByVal Facility As String, ByVal Time_Period As String, ByVal Fromdate As String, ByVal Todate As String, ByVal DataNo As String, ByVal WorkOrder As String, ByVal AuditType As String) As List(Of SPCInspection.MainGridSubgrid)

            Dim retList As New List(Of SPCInspection.MainGridSubgrid)()
            Dim WRList As New List(Of String)()
            Dim WhereString2 As String = ""
            If AuditType = "ALL" Then
                WhereString2 = WhereString2 & " AND InspectionType != 'ROLL'"
            ElseIf AuditType = "FINAL AUDIT" Then
                WhereString2 = WhereString2 & " AND InspectionType = 'EOL'"
            ElseIf AuditType = "IN LINE" Then
                WhereString2 = WhereString2 & " AND InspectionType = 'IL'"
            Else
                WhereString2 = WhereString2 & " AND InspectionType = '" & AuditType & "'"
            End If
            If DataNo <> "ALL" Then
                WhereString2 = WhereString2 & " AND DataNo = '" & DataNo & "'"
            End If
            If WorkOrder <> "ALL" Then
                WhereString2 = WhereString2 & " AND JobNumber = '" & WorkOrder & "'"
            End If
            Dim WhereString As String = " AND Inspection_Finished BETWEEN '" & Fromdate & "' AND '" & Todate & "'"
            If AuditType = "ALL" Then
                WhereString = WhereString & " AND InspectionType != 'ROLL'"
            ElseIf AuditType = "FINAL AUDIT" Then
                WhereString = WhereString & " AND InspectionType = 'EOL'"
            ElseIf AuditType = "IN LINE" Then
                WhereString = WhereString & " AND InspectionType = 'IL'"
            Else
                WhereString = WhereString & " AND InspectionType = '" & AuditType & "'"
            End If
            If DataNo <> "ALL" Then
                WhereString = WhereString & " AND DataNo = '" & DataNo & "'"
            End If
            If WorkOrder <> "ALL" Then
                WhereString = WhereString & " AND JobNumber = '" & WorkOrder & "'"
            End If


            Dim SQL As String = "Select Distinct ISNULL(WorkRoom, 'Other') from Inspection.dbo.Locations inner join dbo.InspectionJobSummaryYearly on dbo.InspectionJobSummaryYearly.CID=NCID Where Name='" & Facility & "'"
            Command.CommandType = CommandType.Text
            Command.Connection = Connection
            Command.CommandText = SQL
            Command.CommandTimeout = 0
            Connection.Open()
            DR = Command.ExecuteReader
            While DR.Read()
                'WRList.Add(DR.GetString(0))
                Dim Subrow As New SPCInspection.MainGridSubgrid()
                If Time_Period = "Past Year" And DR.GetString(0) <> Nothing Then
                    Subrow.FacilityWorkroom = DR.GetString(0)
                    Subrow.DHU = WRGetYearlyDHU(Facility, DR.GetString(0), WhereString2)
                    Subrow.Lot_Acceptance = WRGetYearlyLotAcceptance(Facility, DR.GetString(0), WhereString2)
                    Subrow.No_of_Defects = WRGetYearlyDefectTotal(Facility, DR.GetString(0), WhereString2)
                    Subrow.No_of_Rejected_Lots = WRGetYearlyRejectLotTotal(Facility, DR.GetString(0), WhereString2)
                    Subrow.No_of_Rejects = WRGetYearlyRejectTotal(Facility, DR.GetString(0), WhereString2)
                    Subrow.Reject_Rate = WRGetYearlyRejectionRate(Facility, DR.GetString(0), WhereString2)
                    Subrow.No_of_Inspections = WRGetYearlyInspectionTotal(Facility, DR.GetString(0), WhereString2)
                    retList.Add(Subrow)
                End If
                If Time_Period = "Past 30 Days" And DR.GetString(0) <> Nothing Then
                    Subrow.FacilityWorkroom = DR.GetString(0)
                    Subrow.DHU = WRGetMonthlyDHU(Facility, DR.GetString(0), WhereString2)
                    Subrow.Lot_Acceptance = WRGetMonthlyLotAcceptance(Facility, DR.GetString(0), WhereString2)
                    Subrow.No_of_Defects = WRGetMonthlyDefectTotal(Facility, DR.GetString(0), WhereString2)
                    Subrow.No_of_Rejected_Lots = WRGetMonthlyRejectLotTotal(Facility, DR.GetString(0), WhereString2)
                    Subrow.No_of_Rejects = WRGetMonthlyRejectTotal(Facility, DR.GetString(0), WhereString2)
                    Subrow.Reject_Rate = WRGetMonthlyRejectionRate(Facility, DR.GetString(0), WhereString2)
                    Subrow.No_of_Inspections = WRGetMonthlyInspectionTotal(Facility, DR.GetString(0), WhereString2)
                    retList.Add(Subrow)
                End If
                If Time_Period = "Custom" And DR.GetString(0) <> Nothing Then
                    Subrow.FacilityWorkroom = DR.GetString(0)
                    Subrow.DHU = WRGetCustomDHU(Facility, DR.GetString(0), WhereString)
                    Subrow.Lot_Acceptance = WRGetCustomLotAcceptance(Facility, DR.GetString(0), WhereString)
                    Subrow.No_of_Defects = WRGetCustomDefectTotal(Facility, DR.GetString(0), WhereString)
                    Subrow.No_of_Rejected_Lots = WRGetCustomRejectLotTotal(Facility, DR.GetString(0), WhereString)
                    Subrow.No_of_Rejects = WRGetCustomRejectTotal(Facility, DR.GetString(0), WhereString)
                    Subrow.Reject_Rate = WRGetCustomRejectionRate(Facility, DR.GetString(0), WhereString)
                    Subrow.No_of_Inspections = WRGetCustomInspectionTotal(Facility, DR.GetString(0), WhereString)
                    retList.Add(Subrow)
                End If

            End While
            Return retList
        End Function
        Public Function GetDefectMasterById(ByVal ijsid As Integer) As List(Of SPCInspection.DefectMasterSubgrid)
            Dim SQL As String
            Dim returnlist As New List(Of SPCInspection.DefectMasterSubgrid)
            Dim bmap As New BMappers(Of SPCInspection.DefectMasterSubgrid)

            SQL = "SELECT DefectID, CONVERT(VARCHAR(19),DefectTime) AS DefectTime , EmployeeNo, InspectionId, DefectDesc, DefectClass, Product, WorkRoom FROM DefectMaster WHERE InspectionJobSummaryId = " & ijsid.ToString()

            Try
                returnlist = bmap.GetInspectObject(SQL)
            Catch ex As Exception

            End Try

            Return returnlist

        End Function

        Public Function GetSpecsById(ByVal ijsid As Integer) As List(Of SPCInspection.SpecsSubgrid)
            Dim SQL As String
            Dim returnlist As New List(Of SPCInspection.SpecsSubgrid)
            Dim bmap As New BMappers(Of SPCInspection.SpecsSubgrid)

            SQL = "select sm.id AS SMid, convert(varchar(30), sm.Timestamp) as Timestamp, ps.ProductType, ps.Spec_Description, ps.value, ps.Upper_Spec_Value, ps.Lower_Spec_Value, sm.MeasureValue, sm.SpecDelta, ps.SpecSource from SpecMeasurements sm INNER JOIN ProductSpecification ps ON sm.SpecId = ps.SpecId WHERE sm.InspectionJobSummaryId = " & ijsid.ToString()

            Try
                returnlist = bmap.GetInspectObject(SQL)
            Catch ex As Exception

            End Try

            Return returnlist

        End Function

        Public Function GetSpecsByLocation(ByVal ijsloc As List(Of ActiveLocations), ByVal fromdate As DateTime, ByVal todate As DateTime) As List(Of SPCInspection.SpecsSubgrid)
            Dim SQL As String
            Dim returnlist As New List(Of SPCInspection.SpecsSubgrid)
            Dim bmap As New BMappers(Of SPCInspection.SpecsSubgrid)
            Dim locstrg As String = ""

            If ijsloc.Count > 0 Then
                locstrg = GetLocationByInspectionSummaryFilter(ijsloc)
            End If

            SQL = "select sm.id AS SMid, ijs.JobNumber, ijs.DataNo, convert(varchar(30), sm.Timestamp) as Timestamp, ps.ProductType, ps.Spec_Description, ps.value, ps.Upper_Spec_Value, ps.Lower_Spec_Value, sm.MeasureValue, sm.SpecDelta, ps.SpecSource from SpecMeasurements sm INNER JOIN ProductSpecification ps ON sm.SpecId = ps.SpecId INNER JOIN InspectionJobSummary ijs ON ijs.id = sm.InspectionJobSummaryId WHERE sm.Timestamp >= '" & fromdate.ToString("MM/dd/yyyy hh:mm") & "' and " & locstrg & " sm.Timestamp <= '" & todate.ToString("MM/dd/yyyy hh:mm") & "'"

            Try
                returnlist = bmap.GetInspectObject(SQL)
            Catch ex As Exception

            End Try

            Return returnlist

        End Function

        Public Function GetMainResultsGraph(ByVal TemplateId As Integer, ByVal fromdate As DateTime, ByVal todate As DateTime, ByVal LocationId As Integer, ByVal CID As String) As List(Of SPCInspection.BarChart)
            Dim con As New SqlConnection(DL.InspectConnectionString())
            Dim cmd As SqlCommand = con.CreateCommand()
            Dim rglist As New List(Of SPCInspection.BarChart)
            Dim retlist As New List(Of SPCInspection.BarChart)
            Try
                Using con
                    con.Open()
                    Using cmd
                        cmd = New SqlCommand("SPC_DefectCountBreakdown_2", con)
                        cmd.CommandType = CommandType.StoredProcedure
                        cmd.Parameters.Add("@fromdate", SqlDbType.DateTime).Direction = ParameterDirection.Input
                        cmd.Parameters.Add("@todate", SqlDbType.DateTime).Direction = ParameterDirection.Input
                        cmd.Parameters.Add("@TEMPLATEID", SqlDbType.Int).Direction = ParameterDirection.Input
                        cmd.Parameters.Add("@LOCATIONID", SqlDbType.Int).Direction = ParameterDirection.Input
                        cmd.Parameters.Add("@CID", SqlDbType.VarChar).Direction = ParameterDirection.Input
                        cmd.Parameters("@fromdate").Value = fromdate
                        cmd.Parameters("@todate").Value = todate
                        cmd.Parameters("@TEMPLATEID").Value = TemplateId
                        cmd.Parameters("@LOCATIONID").Value = LocationId
                        cmd.Parameters("@CID").Value = CID

                        cmd.CommandTimeout = 5000

                        Dim bmap_rg As New BMappers(Of SPCInspection.BarChart)
                        rglist = bmap_rg.GetSpcSP(cmd)
                        retlist = (From x In rglist Order By x.x Ascending).ToList()
                    End Using
                End Using
            Catch ex As Exception
                Return Nothing
            End Try

            Return retlist
        End Function
        Public Function GetStackedDefectLineType(ByVal fromdate As DateTime, ByVal todate As DateTime, ByVal LocationId As Integer, ByVal CID As String) As List(Of SPCInspection.StackedDefectLineType)
            Dim con As New SqlConnection(DL.InspectConnectionString())
            Dim cmd As SqlCommand = con.CreateCommand()
            Dim rglist As New List(Of SPCInspection.StackedDefectLineType)
            Dim retlist As New List(Of SPCInspection.StackedDefectLineType)
            Try
                Using con
                    con.Open()
                    Using cmd
                        cmd = New SqlCommand("GetDefectBreakdownByLineType", con)
                        cmd.CommandType = CommandType.StoredProcedure
                        cmd.Parameters.Add("@fromdate", SqlDbType.DateTime).Direction = ParameterDirection.Input
                        cmd.Parameters.Add("@todate", SqlDbType.DateTime).Direction = ParameterDirection.Input
                        cmd.Parameters.Add("@LOCATIONID", SqlDbType.Int).Direction = ParameterDirection.Input
                        cmd.Parameters.Add("@Location", SqlDbType.VarChar).Direction = ParameterDirection.Input
                        cmd.Parameters("@fromdate").Value = fromdate
                        cmd.Parameters("@todate").Value = todate
                        cmd.Parameters("@LOCATIONID").Value = LocationId
                        cmd.Parameters("@Location").Value = CID

                        cmd.CommandTimeout = 5000

                        Dim bmap_rg As New BMappers(Of SPCInspection.StackedDefectLineType)
                        rglist = bmap_rg.GetSpcSP(cmd)
                    End Using
                End Using
            Catch ex As Exception
                Return Nothing
            End Try

            Return rglist
        End Function

        Public Function GetStackedDefectLineType2(ByVal fromdate As DateTime, ByVal todate As DateTime, ByVal DataNo As String, ByVal AuditType As String, ByVal LocArray As List(Of ActiveLocations)) As List(Of BarChartObject)
            Dim con As New SqlConnection(DL.InspectConnectionString())
            Dim cmd As SqlCommand = con.CreateCommand()
            Dim rglist As New List(Of BarChartObject)
            Dim retlist As New List(Of BarChartObject)
            Try
                Using con
                    con.Open()
                    Using cmd
                        cmd = New SqlCommand("GetInspectionTypesBD", con)
                        cmd.CommandType = CommandType.StoredProcedure
                        cmd.Parameters.Add("@fromdate", SqlDbType.DateTime).Direction = ParameterDirection.Input
                        cmd.Parameters.Add("@todate", SqlDbType.DateTime).Direction = ParameterDirection.Input
                        cmd.Parameters.Add("@LOCFILTER", SqlDbType.VarChar, -1).Direction = ParameterDirection.Input
                        cmd.Parameters.Add("@DataNo", SqlDbType.VarChar, 30).Direction = ParameterDirection.Input
                        cmd.Parameters.Add("@AuditType", SqlDbType.VarChar, 30).Direction = ParameterDirection.Input
                        cmd.Parameters("@fromdate").Value = fromdate
                        cmd.Parameters("@todate").Value = todate
                        cmd.Parameters("@LOCFILTER").Value = GetLocation_InspectionJobSummaryFilter(LocArray)
                        cmd.Parameters("@DataNo").Value = DataNo
                        cmd.Parameters("@AuditType").Value = AuditType
                        cmd.CommandTimeout = 5000

                        Dim bmap_rg As New BMappers(Of BarChartObject)
                        rglist = bmap_rg.GetSpcSP(cmd)
                    End Using
                End Using
            Catch ex As Exception
                Return Nothing
            End Try

            Return rglist
        End Function

        Public Function GetLineChart1(ByVal todate As DateTime, ByVal fromdate As DateTime) As List(Of SPCInspection.LineChart1)
            Dim con As New SqlConnection(DL.InspectConnectionString())
            Dim cmd As SqlCommand = con.CreateCommand()
            Dim rglist As New List(Of SPCInspection.LineChart1)
            Dim retlist As New List(Of SPCInspection.LineChart1)

            Try
                Using con
                    con.Open()
                    Using cmd
                        cmd = New SqlCommand("SP_GetDefectsByTypeRange", con)
                        cmd.CommandType = CommandType.StoredProcedure
                        cmd.Parameters.Add("@TODATE", SqlDbType.DateTime).Direction = ParameterDirection.Input
                        cmd.Parameters.Add("@FROMDATE", SqlDbType.DateTime).Direction = ParameterDirection.Input
                        cmd.Parameters.Add("@CID", SqlDbType.VarChar).Direction = ParameterDirection.Input
                        cmd.Parameters("@TODATE").Value = Date.Now
                        cmd.Parameters("@FROMDATE").Value = fromdate
                        cmd.Parameters("@CID").Value = "999"

                        cmd.CommandTimeout = 5000

                        Dim bmap_rg As New BMappers(Of SPCInspection.LineChart1)
                        rglist = bmap_rg.GetSpcSP(cmd)

                    End Using
                End Using
            Catch ex As Exception
                Return Nothing
            End Try

            Return rglist
        End Function
        

        Public Function GetScatterPlotData(ByVal todate As DateTime, ByVal fromdate As DateTime, ByVal CID As String) As List(Of SPCInspection.InspectionScatterPlot)
            Dim con As New SqlConnection(DL.InspectConnectionString())
            Dim cmd As SqlCommand = con.CreateCommand()
            Dim rglist As New List(Of SPCInspection.InspectionScatterPlot)
            Dim retlist As New List(Of SPCInspection.InspectionScatterPlot)
            Try
                Using con
                    con.Open()
                    Using cmd
                        cmd = New SqlCommand("GetDHUByTemplate", con)
                        cmd.CommandType = CommandType.StoredProcedure
                        cmd.Parameters.Add("@TODATE", SqlDbType.DateTime).Direction = ParameterDirection.Input
                        cmd.Parameters.Add("@FROMDATE", SqlDbType.DateTime).Direction = ParameterDirection.Input
                        cmd.Parameters.Add("@CID", SqlDbType.VarChar).Direction = ParameterDirection.Input
                        If DateDiff(DateInterval.Day, fromdate, todate) < 7 Then
                            fromdate = todate.AddDays(-7)
                        End If
                        cmd.Parameters("@TODATE").Value = todate
                        cmd.Parameters("@FROMDATE").Value = fromdate.AddDays(-1)
                        cmd.Parameters("@CID").Value = CID

                        cmd.CommandTimeout = 5000

                        Dim bmap_rg As New BMappers(Of SPCInspection.InspectionScatterPlot)
                        rglist = bmap_rg.GetSpcSP(cmd)

                    End Using
                End Using
            Catch ex As Exception
                Return Nothing
            End Try

            Return rglist
        End Function
        Public Function GetDHUByLocation(ByVal todate As DateTime, ByVal fromdate As DateTime, ByVal DataNo As String, ByVal AuditType As String, ByVal LocArray As List(Of ActiveLocations)) As List(Of SPCInspection.LocationLineChart)
            Dim con As New SqlConnection(DL.InspectConnectionString())
            Dim cmd As SqlCommand = con.CreateCommand()
            Dim rglist As New List(Of SPCInspection.LocationLineChart)
            Dim retlist As New List(Of SPCInspection.LocationLineChart)
            'Dim teststrg As String = GetLocationMasterFilter(LocArray)
            Try
                Using con
                    con.Open()
                    Using cmd
                        cmd = New SqlCommand("GetDHUByLocation3", con)
                        cmd.CommandType = CommandType.StoredProcedure
                        cmd.Parameters.Add("@TODATE", SqlDbType.DateTime).Direction = ParameterDirection.Input
                        cmd.Parameters.Add("@FROMDATE", SqlDbType.DateTime).Direction = ParameterDirection.Input
                        cmd.Parameters.Add("@DATANO", SqlDbType.VarChar, 30).Direction = ParameterDirection.Input
                        cmd.Parameters.Add("@AUDITTYPE", SqlDbType.VarChar, 30).Direction = ParameterDirection.Input
                        cmd.Parameters.Add("@LOCFILTER", SqlDbType.VarChar, 10000).Direction = ParameterDirection.Input
                        If DateDiff(DateInterval.Day, fromdate, todate) < 7 Then
                            fromdate = todate.AddDays(-7)
                        End If
                        cmd.Parameters("@TODATE").Value = todate
                        cmd.Parameters("@FROMDATE").Value = fromdate.AddDays(-1)
                        cmd.Parameters("@DATANO").Value = DataNo
                        cmd.Parameters("@AUDITTYPE").Value = AuditType
                        cmd.Parameters("@LOCFILTER").Value = GetLocationMasterFilter(LocArray)
                        cmd.CommandTimeout = 5000

                        Dim bmap_rg As New BMappers(Of SPCInspection.LocationLineChart)
                        rglist = bmap_rg.GetSpcSP(cmd)

                    End Using
                End Using
            Catch ex As Exception
                Return Nothing
            End Try

            Return rglist
        End Function

        Public Function GetLocationMasterFilter(ByVal LocArray As List(Of ActiveLocations)) As Object

            Dim retstrg As String = ""

            Try
                For Each item In LocArray
                    If item.status = "False" And item.CID <> "999" Then
                        retstrg = retstrg + " CAST(CAST(lm.CID AS INT) AS VARCHAR) <> '" & item.CID & "' AND "
                    End If

                Next

            Catch ex As Exception

            End Try

            Return retstrg
        End Function
        Public Function GetLocation_InspectionJobSummaryFilter(ByVal LocArray As List(Of ActiveLocations)) As Object

            Dim retstrg As String = ""

            Try
                For Each item In LocArray
                    If item.status = "False" And item.CID <> "999" Then
                        retstrg = retstrg + "AND (InspectionJobSummary.CID <> '" & item.CID & "') "
                    End If

                Next

            Catch ex As Exception

            End Try

            Return retstrg
        End Function
        Public Function getAS400LocationName(ByVal CID As Object) As List(Of SingleObject)
            Dim listso As New List(Of SingleObject)
            Dim bmapso As New BMappers(Of SingleObject)
            Dim str As String = ""
            If IsNumeric(CID) = True Then


                Dim CIDnum As Integer = Convert.ToInt64(CID)

                str = "SELECT LinkedServerMaster.DSN_Identifier AS Object1" & vbCrLf &
                            "FROM LinkedServerMaster INNER JOIN" & vbCrLf &
                            "LocationMaster ON LinkedServerMaster.LocationId = LocationMaster.id" & vbCrLf &
                            "WHERE (LinkedServerMaster.DSN_Type = 'AS400') AND (LocationMaster.CID = N'000" & CIDnum.ToString() & "')"
            End If

            listso = bmapso.GetAprMangObject(str)

            Return listso

        End Function
        Public Function GetREJByLocation(ByVal todate As DateTime, ByVal fromdate As DateTime) As List(Of SPCInspection.LocationLineChart)
            Dim con As New SqlConnection(DL.InspectConnectionString())
            Dim cmd As SqlCommand = con.CreateCommand()
            Dim rglist As New List(Of SPCInspection.LocationLineChart)
            Dim retlist As New List(Of SPCInspection.LocationLineChart)
            Try
                Using con
                    con.Open()
                    Using cmd
                        cmd = New SqlCommand("GetREJByLocation", con)
                        cmd.CommandType = CommandType.StoredProcedure
                        cmd.Parameters.Add("@TODATE", SqlDbType.DateTime).Direction = ParameterDirection.Input
                        cmd.Parameters.Add("@FROMDATE", SqlDbType.DateTime).Direction = ParameterDirection.Input

                        If DateDiff(DateInterval.Day, fromdate, todate) < 7 Then
                            fromdate = todate.AddDays(-7)
                        End If
                        cmd.Parameters("@TODATE").Value = todate
                        cmd.Parameters("@FROMDATE").Value = fromdate.AddDays(-1)

                        cmd.CommandTimeout = 5000

                        Dim bmap_rg As New BMappers(Of SPCInspection.LocationLineChart)
                        rglist = bmap_rg.GetSpcSP(cmd)

                    End Using
                End Using
            Catch ex As Exception
                Return Nothing
            End Try

            Return rglist
        End Function

        Private Function GetAS400Abr(ByVal CID As String) As String
            Dim bmapso As New BMappers(Of SingleObject)
            Dim listso As New List(Of SingleObject)
            Dim retobj As String = ""
            If CID <> "999" Then
                listso = bmapso.GetAprMangObject("SELECT lsm.DSN_Identifier Object1 FROM  LocationMaster lm INNER JOIN  LinkedServerMaster lsm ON lm.id = lsm.LocationId where lm.CID = '000" & CID & "'")

                If listso.Count > 0 Then
                    retobj = listso.ToArray()(0).Object1
                End If
            Else
                retobj = "ALL"
            End If

            Return retobj
        End Function

        Public Function GetRejectRate(ByVal fromdate As DateTime, ByVal todate As DateTime, Locationarray As List(Of ActiveLocations), Optional ByVal DataNo As String = "", Optional ByVal AuditType As String = "") As Object

            Dim listso As New List(Of SingleObject)
            Dim bmapso As New BMappers(Of SingleObject)
            Dim retobj As Double = -1
            Dim DNString As String = ""
            Dim ATString As String = ""
            Dim LOCString As String = ""

            If DataNo.ToUpper().ToString() <> "ALL" And DataNo.ToString().Length > 0 Then
                DNString = " ijs.DataNo = '" & DataNo & "' and "
            End If

            If AuditType.ToUpper().ToString() <> "ALL" And AuditType.ToString().Length > 0 Then
                ATString = " tn.LineType = '" + AuditType + "' and "
            End If

            If Locationarray.ToArray().Length > 0 Then
                LOCString = GetLocationByInspectionSummaryFilter(Locationarray)
            End If

            listso = bmapso.GetInspectObject("select AQL_Level as Object1, case when AQL_Level = '100' THEN (sum(ijs.MajorsCount) + sum(ijs.RepairsCount) + sum(ijs.ScrapCount)) * 100 ELSE (sum(ijs.MajorsCount) + sum(ijs.RepairsCount) + sum(ijs.ScrapCount)) * 100 END as Object2, sum(ijs.TotalInspectedItems) as Object3 FROM InspectionJobSummary ijs INNER JOIN TemplateName tn ON ijs.TemplateId = tn.TemplateId where " + DNString + LOCString + ATString + " ijs.Inspection_Started >= '" & fromdate.ToString("MM/dd/yyyy hh:mm") & "' and ijs.Inspection_Started <= '" & todate.ToString("MM/dd/yyyy hh:mm") & "' and ijs.TotalInspectedItems > 0 group by ijs.AQL_Level")

            If listso.Count > 0 Then
                Dim den As Double = 0
                Dim num As Double = 0

                num = (From v In listso Select v.Object2).Sum()
                den = (From v In listso Select CType(v.Object3, Integer)).Sum()

                If den > 0 Then
                    retobj = num / den
                Else
                    retobj = 0
                End If

            Else
                retobj = 0
            End If

            Return retobj

        End Function

        Public Function GetSingleRejectRate(ByVal fromdate As DateTime, ByVal todate As DateTime, Locationid As String, Optional ByVal DataNo As String = "", Optional ByVal AuditType As String = "") As Object

            Dim listso As New List(Of SingleObject)
            Dim bmapso As New BMappers(Of SingleObject)
            Dim retobj As Double = -1
            Dim DNString As String = ""
            Dim ATString As String = ""
            Dim LOCString As String = ""

            If DataNo.ToUpper().ToString() <> "ALL" And DataNo.ToString().Length > 0 Then
                DNString = " ijs.DataNo = '" & DataNo & "' and "
            End If

            If AuditType.ToUpper().ToString() <> "ALL" And AuditType.ToString().Length > 0 Then
                ATString = " it.Name = '" + AuditType + "' and "
            End If

            If Locationid <> "999" Then
                LOCString = " ijs.CID = '" & Locationid.ToString() & "' and "
            End If


            listso = bmapso.GetInspectObject("select AQL_Level as Object1, case when AQL_Level = '100' THEN (sum(ijs.MajorsCount) + sum(ijs.RepairsCount) + sum(ijs.ScrapCount)) * 100 ELSE (sum(ijs.MajorsCount) + sum(ijs.RepairsCount) + sum(ijs.ScrapCount)) * 100 END as Object2, sum(ijs.TotalInspectedItems) as Object3 FROM InspectionJobSummary ijs INNER JOIN TemplateName tn ON ijs.TemplateId = tn.TemplateId INNER JOIN InspectionTypes it ON it.id = tn.LineTypeId where " + DNString + LOCString + ATString + " ijs.Inspection_Started >= '" & fromdate.ToString("MM/dd/yyyy hh:mm") & "' and ijs.Inspection_Started <= '" & todate.ToString("MM/dd/yyyy hh:mm") & "' and ijs.TotalInspectedItems > 0 group by ijs.AQL_Level")

            If listso.Count > 0 Then
                Dim den As Double = 0
                Dim num As Double = 0

                num = (From v In listso Select v.Object2).Sum()
                den = (From v In listso Select CType(v.Object3, Integer)).Sum()

                If den > 0 Then
                    retobj = num / den
                Else
                    retobj = 0
                End If

            Else
                retobj = 0
            End If

            Return retobj

        End Function

        Public Function GetDHURate(ByVal fromdate As DateTime, ByVal todate As DateTime, Locationarray As List(Of ActiveLocations), Optional ByVal DataNo As String = "", Optional ByVal AuditType As String = "") As Object

            Dim listso As New List(Of SingleObject)
            Dim bmapso As New BMappers(Of SingleObject)
            Dim retobj As Double = -1
            Dim DNString As String = ""
            Dim ATString As String = ""
            Dim LOCString As String = ""

            If DataNo.ToUpper().ToString() <> "ALL" And DataNo.ToString().Length > 0 Then
                DNString = " ijs.DataNo = '" & DataNo & "' and "
            End If

            If AuditType.ToUpper().ToString() <> "ALL" And AuditType.ToString().Length > 0 Then
                ATString = " tn.LineType = '" + AuditType + "' and "
            End If
            If Locationarray.ToArray().Length > 0 Then
                LOCString = GetLocationByInspectionSummaryFilter(Locationarray)
            End If

            listso = bmapso.GetInspectObject("select AQL_Level as Object1, case when AQL_Level = '100' THEN (sum(ijs.MajorsCount) + sum(ijs.MinorsCount) + sum(ijs.RepairsCount) + sum(ijs.ScrapCount)) * 100 ELSE (sum(ijs.MajorsCount)  + sum(ijs.MinorsCount) + sum(ijs.RepairsCount) + sum(ijs.ScrapCount)) * 100 END as Object2, sum(ijs.TotalInspectedItems) as Object3 FROM InspectionJobSummary ijs INNER JOIN TemplateName tn ON ijs.TemplateId = tn.TemplateId INNER JOIN InspectionTypes it ON it.id = tn.LineTypeId where " + DNString + LOCString + ATString + " ijs.Inspection_Started >= '" & fromdate.ToString("MM/dd/yyyy hh:mm") & "' and ijs.Inspection_Started <= '" & todate.ToString("MM/dd/yyyy hh:mm") & "' and ijs.TotalInspectedItems > 0 group by ijs.AQL_Level")

            If listso.Count > 0 Then
                Dim den As Double = 0
                Dim num As Double = 0

                num = (From v In listso Select v.Object2).Sum()
                den = (From v In listso Select CType(v.Object3, Integer)).Sum()

                If den > 0 Then
                    retobj = num / den
                Else
                    retobj = 0
                End If

            Else
                retobj = 0
            End If

            Return retobj

        End Function

        Public Function GetSingleDHURate(ByVal fromdate As DateTime, ByVal todate As DateTime, Locationid As String, Optional ByVal DataNo As String = "", Optional ByVal AuditType As String = "") As Object

            Dim listso As New List(Of SingleObject)
            Dim bmapso As New BMappers(Of SingleObject)
            Dim retobj As Double = -1
            Dim DNString As String = ""
            Dim ATString As String = ""
            Dim LOCString As String = ""

            If DataNo.ToUpper().ToString() <> "ALL" And DataNo.ToString().Length > 0 Then
                DNString = " ijs.DataNo = '" & DataNo & "' and "
            End If

            If AuditType.ToUpper().ToString() <> "ALL" And AuditType.ToString().Length > 0 Then
                ATString = " it.Name = '" + AuditType + "' and "
            End If

            If Locationid <> "999" Then
                LOCString = " ijs.CID = '" & Locationid.ToString() & "' and "
            End If

            listso = bmapso.GetInspectObject("select AQL_Level as Object1, case when AQL_Level = '100' THEN (sum(ijs.MajorsCount) + sum(ijs.MinorsCount) + sum(ijs.RepairsCount) + sum(ijs.ScrapCount)) * 100 ELSE (sum(ijs.MajorsCount)  + sum(ijs.MinorsCount) + sum(ijs.RepairsCount) + sum(ijs.ScrapCount)) * 100 END as Object2, sum(ijs.TotalInspectedItems) as Object3 FROM InspectionJobSummary ijs INNER JOIN TemplateName tn ON ijs.TemplateId = tn.TemplateId INNER JOIN InspectionTypes it ON it.id = tn.LineTypeId where " + DNString + LOCString + ATString + " ijs.Inspection_Started >= '" & fromdate.ToString("MM/dd/yyyy hh:mm") & "' and ijs.Inspection_Started <= '" & todate.ToString("MM/dd/yyyy hh:mm") & "' and ijs.TotalInspectedItems > 0 group by ijs.AQL_Level")

            If listso.Count > 0 Then
                Dim den As Double = 0
                Dim num As Double = 0

                num = (From v In listso Select v.Object2).Sum()
                den = (From v In listso Select CType(v.Object3, Integer)).Sum()

                If den > 0 Then
                    retobj = num / den
                Else
                    retobj = 0
                End If

            Else
                retobj = 0
            End If

            Return retobj

        End Function

        Public Function GetSingleComplianceRate(ByVal fromdate As DateTime, ByVal todate As DateTime, Locationid As String, Optional ByVal DataNo As String = "", Optional ByVal AuditType As String = "") As Object

            Dim listso As New List(Of SingleObject)
            Dim listso1 As New List(Of SingleObject)
            Dim bmapso As New BMappers(Of SingleObject)
            Dim retobj As Double = -1
            Dim DNString As String = ""
            Dim LOCString As String = ""

            If DataNo.ToUpper().ToString() <> "ALL" And DataNo.ToString().Length > 0 Then
                DNString = " ijs.DataNo = '" & DataNo & "' and "
            End If

            If Locationid <> "999" Then
                LOCString = " ijs.CID = '" & Locationid.ToString() & "' and "
            End If

            listso = bmapso.GetInspectObject("select count(ijs.id) as Object1 FROM InspectionJobSummary ijs INNER JOIN TemplateName tn ON ijs.TemplateId = tn.TemplateId INNER JOIN InspectionTypes it ON it.id = tn.LineTypeId where " + DNString + LOCString + " ijs.Inspection_Started >= '" & fromdate.ToString("MM/dd/yyyy hh:mm") & "' and it.Name = 'FINAL AUDIT' and ijs.Inspection_Started <= '" & todate.ToString("MM/dd/yyyy hh:mm") & "' and ijs.TotalInspectedItems > 0 ")

            If listso.Count > 0 Then

                listso1 = bmapso.GetInspectObject("select count(ijs.id) as Object1 FROM InspectionJobSummary ijs INNER JOIN TemplateName tn ON ijs.TemplateId = tn.TemplateId INNER JOIN InspectionTypes it ON it.id = tn.LineTypeId where " + DNString + LOCString + " ijs.Inspection_Started >= '" & fromdate.ToString("MM/dd/yyyy hh:mm") & "' and it.Name <> 'ROLL' and ijs.Inspection_Started <= '" & todate.ToString("MM/dd/yyyy hh:mm") & "' and ijs.TotalInspectedItems > 0 ")
                If listso1.Count > 0 Then

                    Dim den As Double = 0
                    Dim num As Double = 0

                    num = listso.ToArray()(0).Object1
                    den = listso1.ToArray()(0).Object1

                    If den > 0 Then
                        retobj = (num / den) * 100
                    Else
                        retobj = 0
                    End If
                End If

            Else
                retobj = 0
            End If

            Return retobj

        End Function

        Public Function GetComplianceRate(ByVal fromdate As DateTime, ByVal todate As DateTime, Locationarray As List(Of ActiveLocations), Optional FilterAction As Boolean = False, Optional ByVal DataNo As String = "", Optional ByVal AuditType As String = "") As Object

            Dim listic As New List(Of SPCInspection.InspectionCompliance_Local)
            Dim listijs As New List(Of SingleObject)
            Dim bmapijs As New BMappers(Of SingleObject)
            Dim retobj As Double = 0
            listijs = bmapijs.GetInspectObject("SELECT TOP(1) id as Object1 FROM InspectionJobSummary order by id desc")


            Dim cachstring As String = "InspectionCompliance_Local_ijsid_" + todate.ToString("yyyy.MM.dd") + "." + fromdate.ToString("yyyy.MM.dd")
            Dim Cachedijsid As Object = HttpRuntime.Cache(cachstring)
            If Not Cachedijsid Is Nothing And listijs.Count > 0 Then
                If Cachedijsid = listijs.ToArray()(0).Object1 Then
                    listic = HttpRuntime.Cache("InspectionCompliance_Local" + todate.ToString("yyyy.MM.dd") + "." + fromdate.ToString("yyyy.MM.dd"))
                End If
            End If
            If listic.Count = 0 Then
                listic = Getas400WOInspectionCompliance(fromdate, todate)
                If FilterAction = False Then
                    Dim cachic As New List(Of SPCInspection.InspectionCompliance_Local)
                    cachic = listic
                    InsertComplianceIntoCache(cachic, fromdate, todate, listijs.ToArray()(0).Object1)
                End If
            End If

            If Not listic Is Nothing Then
                If listic.Count > 0 Then
                    Dim locoff = (From v In Locationarray Where v.status = "False" And v.CID <> "999" Select v).ToList()
                    If Not locoff Is Nothing Then
                        If locoff.Count > 0 Then
                            For Each item In locoff
                                Dim a400abr As String = GetAS400Abr(item.CID)
                                If a400abr.Length > 0 Then
                                    listic = (From v In listic Where v.Location.Trim() <> a400abr.Trim() Select v).ToList()
                                End If
                            Next
                        End If
                    End If
                End If
            End If


            If Not listic Is Nothing Then
                If listic.Count > 0 Then
                    retobj = ReturnCompRate(listic, AuditType, DataNo)
                End If
            End If

            Return retobj


        End Function

        Private Function ReturnCompRate(ByVal listin As List(Of SPCInspection.InspectionCompliance_Local), ByVal AuditType As String, ByVal DataNo As String) As Double
            Dim retobj As Double = 0
            Dim listnew As New List(Of SPCInspection.InspectionCompliance_Local)
            Dim den As Double = 1
            Dim num As Double = 0


            If AuditType = "ALL" And DataNo = "ALL" Then

                den = (From v In listin Select v).Count()
                num = (From v In listin Where v.ijsid.ToString().Length > 0 Select v).Count()

            ElseIf AuditType = "ALL" And DataNo <> "ALL" Then

                den = (From v In listin Select v).Count()
                num = (From v In listin Where v.ijsid.ToString().Length > 0 And v.DataNo.Trim() = DataNo.Trim() Select v).Count()

            ElseIf DataNo = "ALL" And AuditType <> "ALL" Then

                den = (From v In listin Select v).Count()
                num = (From v In listin Where v.ijsid.ToString().Length > 0 And v.LineType = AuditType.Trim() Select v).Count()

            End If

            retobj = (num / den) * 100

            Return retobj
        End Function

        Private Sub InsertComplianceIntoCache(ByVal listic As List(Of SPCInspection.InspectionCompliance_Local), ByVal fromdate As DateTime, ByVal todate As DateTime, lastijsid As Integer)

            If Not listic Is Nothing Then
                If listic.Count > 0 Then
                    HttpRuntime.Cache.Insert("InspectionCompliance_Local" + todate.ToString("yyyy.MM.dd") + "." + fromdate.ToString("yyyy.MM.dd"), listic, Nothing, Date.Now.AddDays(3), System.Web.Caching.Cache.NoSlidingExpiration)
                    If IsNumeric(lastijsid) Then
                        HttpRuntime.Cache.Insert("InspectionCompliance_Local_ijsid_" + todate.ToString("yyyy.MM.dd") + "." + fromdate.ToString("yyyy.MM.dd"), lastijsid, Nothing, Date.Now.AddDays(3), System.Web.Caching.Cache.NoSlidingExpiration)
                    End If
                End If
            End If

        End Sub

        Public Function GetLocationByInspectionSummaryFilter(ByVal LocArray As List(Of ActiveLocations)) As Object

            Dim retstrg As String = ""

            Try
                For Each item In LocArray
                    If item.status = "False" And item.CID <> "999" Then
                        retstrg = retstrg + " ijs.CID <> '" & item.CID & "' AND "
                    End If

                Next

            Catch ex As Exception

            End Try

            Return retstrg
        End Function
        Public Function LocationsFilter_DefectImage(ByVal LocArray As List(Of ActiveLocations)) As String
            Dim retstrg As String = ""

            Try
                For Each item In LocArray
                    If item.status = "False" And item.CID <> "999" And IsNothing(item.ProdAbreviation) = False Then
                        retstrg = retstrg + " AND RTRIM(LTRIM(ijs.CID)) <> '" & item.CID.ToString() & "' "
                    End If

                Next

            Catch ex As Exception

            End Try

            Return retstrg


        End Function
        Public Function GetInspectionComplianceLocationFilter(ByVal LocArray As List(Of ActiveLocations)) As String
            Dim retstrg As String = ""

            Try
                For Each item In LocArray
                    If item.status = "False" And item.CID <> "999" And IsNothing(item.ProdAbreviation) = False Then
                        retstrg = retstrg + " AND RTRIM(LTRIM(ic.Location)) <> '" & item.ProdAbreviation & "' "
                    End If

                Next

            Catch ex As Exception

            End Try

            Return retstrg

        End Function

        Public Function GetInspectionCompliancePrpCodeFilter(ByVal FiltArray As List(Of ActiveFilterObject)) As String
            Dim retstrg As String = "AND ( "
            Dim FieldCnt As Int16 = 0
            Try
                If Not FiltArray Is Nothing Then
                    For Each item In FiltArray
                        If item.Name = "pf_prp" Then
                            If FieldCnt = 0 Then
                                retstrg = retstrg + " RTRIM(LTRIM(ic.Prp_Code)) = '" & item.value & "' "
                            Else
                                retstrg = retstrg + " OR RTRIM(LTRIM(ic.Prp_Code)) = '" & item.value & "' "
                            End If

                            FieldCnt += 1
                        End If

                    Next
                    retstrg = retstrg + " )"
                End If

            Catch ex As Exception
                FieldCnt = 0
            End Try

            If FieldCnt = 0 Then
                retstrg = ""
            End If

            Return retstrg

        End Function

        Public Function GetInspectionCompliancePrpFilter(filterlist As List(Of ActiveFilterObject)) As String
            Dim retstrg As String = ""

            Try
                For Each item In filterlist
                    If item.Name = "pf_prp" Then
                        retstrg = retstrg + " and ic.Prp_Code = '''" & item.value & "''' "
                    End If

                Next

            Catch ex As Exception

            End Try

            Return retstrg

        End Function

        Public Function GetLotAcc(ByVal fromdate As DateTime, ByVal todate As DateTime, Locationarray As List(Of ActiveLocations), Optional ByVal DataNo As String = "", Optional ByVal AuditType As String = "") As Object
            Dim listso As New List(Of SingleObject)
            Dim bmapso As New BMappers(Of SingleObject)
            Dim retobj As Double = -1
            Dim DNString As String = ""
            Dim ATString As String = ""
            Dim LOCString As String = ""

            If DataNo.ToUpper().ToString() <> "ALL" And DataNo.ToString().Length > 0 Then
                DNString = " ijs.DataNo = '" & DataNo & "' and "
            End If

            If AuditType.ToUpper().ToString() <> "ALL" And AuditType.ToString().Length > 0 Then
                ATString = " it.Name = '" + AuditType + "' and "
            End If

            If Locationarray.ToArray().Length > 0 Then
                LOCString = GetLocationByInspectionSummaryFilter(Locationarray)
            End If

            listso = bmapso.GetInspectObject("select 100 * cast((SELECT COUNT(*) FROM InspectionJobSummary ijs INNER JOIN TemplateName tn ON ijs.TemplateId = tn.TemplateId INNER JOIN InspectionTypes it ON it.id = tn.LineTypeId WHERE " + LOCString + DNString + " ijs.Technical_PassFail = 1 AND ijs.Inspection_Started >= '" & fromdate.ToString("MM/dd/yyyy hh:mm") & "' AND ijs.Inspection_Started < '" & todate.ToString("MM/dd/yyyy hh:mm") & "') as float)/cast((SELECT COUNT(*) FROM InspectionJobSummary ijs INNER JOIN TemplateName tn ON ijs.TemplateId = tn.TemplateId INNER JOIN InspectionTypes it ON it.id = tn.LineTypeId WHERE " + LOCString + DNString + " ijs.Inspection_Started >= '" & fromdate.ToString("MM/dd/yyyy hh:mm") & "' AND ijs.Inspection_Started < '" & todate.ToString("MM/dd/yyyy hh:mm") & "') as float) as Object1")

            If listso.Count > 0 Then
                retobj = listso.ToArray()(0).Object1
            ElseIf listso.Count = 0 Then
                retobj = 0
            End If

            Return retobj

        End Function

        Public Function GetSingleLotAcc(ByVal fromdate As DateTime, ByVal todate As DateTime, Locationid As String, Optional ByVal DataNo As String = "", Optional ByVal AuditType As String = "") As Object
            Dim listso As New List(Of SingleObject)
            Dim bmapso As New BMappers(Of SingleObject)
            Dim retobj As Double = -1
            Dim DNString As String = ""
            Dim ATString As String = ""
            Dim LOCString As String = ""

            If DataNo.ToUpper().ToString() <> "ALL" And DataNo.ToString().Length > 0 Then
                DNString = " ijs.DataNo = '" & DataNo & "' and "
            End If

            If AuditType.ToUpper().ToString() <> "ALL" And AuditType.ToString().Length > 0 Then
                ATString = " tn.LineType = '" + AuditType + "' and "
            End If

            If Locationid <> "999" Then
                LOCString = "ijs.CID = '" + Locationid + "' and "
            End If

            listso = bmapso.GetInspectObject("select 100 * cast((SELECT COUNT(*) FROM InspectionJobSummary ijs INNER JOIN TemplateName tn ON ijs.TemplateId = tn.TemplateId WHERE " + LOCString + DNString + " ijs.Technical_PassFail = 1 AND ijs.Inspection_Started >= '" & fromdate.ToString("MM/dd/yyyy hh:mm") & "' AND ijs.Inspection_Started < '" & todate.ToString("MM/dd/yyyy hh:mm") & "') as float)/cast((SELECT COUNT(*) FROM InspectionJobSummary ijs INNER JOIN TemplateName tn ON ijs.TemplateId = tn.TemplateId WHERE " + LOCString + DNString + " ijs.Inspection_Started >= '" & fromdate.ToString("MM/dd/yyyy hh:mm") & "' AND ijs.Inspection_Started < '" & todate.ToString("MM/dd/yyyy hh:mm") & "') as float) as Object1")

            If listso.Count > 0 Then
                retobj = listso.ToArray()(0).Object1
            ElseIf listso.Count = 0 Then
                retobj = 0
            End If

            Return retobj

        End Function

        Public Function GetCompliance_FilterDN(ByVal listwoc As List(Of SPCInspection.WorkOrderCompliance), ByVal DataNumber As String) As Double
            Dim retobj As Double = 0
            Dim num As Object = 0
            Dim den As Object = 0

            If listwoc.Count > 0 Then

                num = (From v In listwoc Where v.Match = True And v.DataNo = DataNumber Select v).Count()
                den = (From v In listwoc Where v.DataNo = DataNumber Select v).Count()

            End If

            If den > 0 Then
                retobj = (num / den) * 100
            End If

            Return retobj
        End Function
        
        Public Function GetMatchPercLocal(ByVal listwoc As List(Of SPCInspection.WorkOrderCompliance), ByVal fromdatein As DateTime, ByVal todatein As DateTime, ByVal CID As String, Optional ByVal ActiveFilterField As Object = Nothing, Optional ByVal ActiveValue As Object = Nothing) As Double

            Dim listwocapr As New List(Of SPCInspection.WorkOrderCompliance)
            Dim retobj As Double = 0
            Dim num As Object = 0
            Dim den As Object = 0
            If listwoc.Count > 0 Then
                If CID <> "999" Then
                    Dim listls As New List(Of SingleObject)
                    Dim bmapso As New BMappers(Of SingleObject)

                    listls = bmapso.GetAprMangObject("SELECT DSN_Identifier as Object1 FROM LinkedServerMaster lsm inner join LocationMaster lm ON lsm.LocationId = lm.id WHERE lm.CID = '000" & CID & "'")

                    If IsNothing(ActiveFilterField) = False And IsNothing(ActiveValue) = False Then
                        If ActiveValue <> "ALL" Then
                            Dim listwoc2 As New List(Of SPCInspection.WorkOrderCompliance)
                            listwoc2 = listwoc
                            listwoc.Clear()
                            Select Case ActiveValue
                                Case "pf_AuditType"
                                    listwoc = (From v In listwoc2 Where v.LineType = ActiveValue Select v).ToList()
                                Case "pf_DataNumber"
                                    listwoc = (From v In listwoc2 Where v.DataNo = ActiveValue Select v).ToList()
                            End Select
                        End If
                    End If
                    If listls.Count > 0 Then
                        Select Case listls.Count
                            Case 1
                                num = (From v In listwoc Where v.Branch.Trim() = listls.ToArray()(0).Object1 And v.Match = True Select v).Count()
                                den = (From v In listwoc Where v.Branch.Trim() = listls.ToArray()(0).Object1 Select v).Count()
                            Case 2
                                num = (From v In listwoc Where v.Branch.Trim() = listls.ToArray()(0).Object1 Or v.Branch.Trim() = listls.ToArray()(1).Object1 And v.Match = True Select v).Count()
                                den = (From v In listwoc Where v.Branch.Trim() = listls.ToArray()(0).Object1 Or v.Branch.Trim() = listls.ToArray()(1).Object1 Select v).Count()
                            Case 3
                                num = (From v In listwoc Where v.Branch.Trim() = listls.ToArray()(0).Object1 Or v.Branch.Trim() = listls.ToArray()(1).Object1 Or v.Branch.Trim() = listls.ToArray()(2).Object1 And v.Match = True Select v).Count()
                                den = (From v In listwoc Where v.Branch.Trim() = listls.ToArray()(0).Object1 Or v.Branch.Trim() = listls.ToArray()(1).Object1 Or v.Branch.Trim() = listls.ToArray()(2).Object1 Select v).Count()
                        End Select
                    End If
                Else
                    num = (From v In listwoc Where v.Match = True Select v).Count()
                    den = (From v In listwoc Select v).Count()
                End If
            End If

            If den > 0 Then
                retobj = (num / den) * 100
            End If

            Return retobj

        End Function
        Public Function Getas400MatchPerc(ByVal fromdate As DateTime, ByVal todate As DateTime, ByVal LocationId As Integer) As Double
            Dim con As New SqlConnection(DL.InspectConnectionString())
            Dim cmd As SqlCommand = con.CreateCommand()
            Dim rglist As Double = -1

            Try
                Using con
                    con.Open()
                    Using cmd
                        cmd = New SqlCommand("SP_AS400_GetMatchPerc", con)
                        cmd.CommandType = CommandType.StoredProcedure
                        cmd.Parameters.Add("@FROMDATE", SqlDbType.DateTime).Direction = ParameterDirection.Input
                        cmd.Parameters.Add("@TODATE", SqlDbType.DateTime).Direction = ParameterDirection.Input
                        cmd.Parameters.Add("@BRANCH", SqlDbType.VarChar).Direction = ParameterDirection.Input
                        cmd.Parameters.Add("@CID", SqlDbType.VarChar).Direction = ParameterDirection.Input
                        cmd.Parameters.Add("@MATCHPERC", SqlDbType.Float).Direction = ParameterDirection.Output
                        cmd.Parameters("@FROMDATE").Value = fromdate
                        cmd.Parameters("@TODATE").Value = todate
                        cmd.Parameters("@BRANCH").Value = GetAS400Abr(LocationId.ToString())
                        cmd.Parameters("@CID").Value = LocationId.ToString()
                        cmd.CommandTimeout = 5000

                        Dim bmap_rg As New BMappers(Of SPCInspection.WorkOrderCompliance)
                        cmd.ExecuteReader(CommandBehavior.CloseConnection)

                        rglist = cmd.Parameters("@MATCHPERC").Value

                    End Using
                End Using
            Catch ex As Exception
                Return Nothing
            End Try

            Return rglist
        End Function


        Public Function Getas400WOByBranch(ByVal fromdate As DateTime, ByVal todate As DateTime, ByVal LocationId As Integer) As List(Of SPCInspection.WorkOrderCompliance)
            Dim con As New SqlConnection(DL.InspectConnectionString())
            Dim cmd As SqlCommand = con.CreateCommand()
            Dim rglist As New List(Of SPCInspection.WorkOrderCompliance)

            Try
                Using con
                    con.Open()
                    Using cmd
                        cmd = New SqlCommand("SP_AS400_GETWOBYBRANCH", con)
                        cmd.CommandType = CommandType.StoredProcedure
                        cmd.Parameters.Add("@FROMDATE", SqlDbType.DateTime).Direction = ParameterDirection.Input
                        cmd.Parameters.Add("@TODATE", SqlDbType.DateTime).Direction = ParameterDirection.Input
                        cmd.Parameters.Add("@BRANCH", SqlDbType.VarChar).Direction = ParameterDirection.Input
                        cmd.Parameters("@FROMDATE").Value = fromdate
                        cmd.Parameters("@TODATE").Value = todate
                        cmd.Parameters("@BRANCH").Value = GetAS400Abr(LocationId.ToString())

                        cmd.CommandTimeout = 5000

                        Dim bmap_rg As New BMappers(Of SPCInspection.WorkOrderCompliance)
                        rglist = bmap_rg.GetSpcSP(cmd)

                    End Using
                End Using
            Catch ex As Exception
                Return Nothing
            End Try

            Return rglist
        End Function

        Public Function Getas400WOInspectionCompliance(ByVal fromdate As DateTime, ByVal todate As DateTime) As List(Of SPCInspection.InspectionCompliance_Local)
            Dim con As New SqlConnection(DL.InspectConnectionString())
            Dim cmd As SqlCommand = con.CreateCommand()
            Dim rglist As New List(Of SPCInspection.InspectionCompliance_Local)

            Try
                Using con
                    con.Open()
                    Using cmd
                        cmd = New SqlCommand("GetInspectionComplianceData", con)
                        cmd.CommandType = CommandType.StoredProcedure
                        cmd.Parameters.Add("@FROMDATE", SqlDbType.DateTime).Direction = ParameterDirection.Input
                        cmd.Parameters.Add("@TODATE", SqlDbType.DateTime).Direction = ParameterDirection.Input
                        cmd.Parameters("@FROMDATE").Value = fromdate
                        cmd.Parameters("@TODATE").Value = todate

                        cmd.CommandTimeout = 5000

                        ' Dim bmap_rg As New BMappers(Of SPCInspection.InspectionCompliance_Local)
                        rglist = _DAOFactory.getInspectionCompliance(cmd)
                        'rglist = bmap_rg.GetSpcSP(cmd)

                    End Using
                End Using
            Catch ex As Exception
                Return Nothing
            End Try

            Return rglist
        End Function

        Public Function GetInspectionCompliancePerc(ByVal fromdate As DateTime, ByVal todate As DateTime, audittype As String, datano As String, prpcode As String, Optional Locations As String = "") As Object
            Dim con As New SqlConnection(DL.InspectConnectionString())
            Dim cmd As SqlCommand = con.CreateCommand()
            Dim rglist As New List(Of SingleObject)

            Try
                Using con
                    con.Open()
                    Using cmd
                        cmd = New SqlCommand("GetInspectionCompliancePerc", con)
                        cmd.CommandType = CommandType.StoredProcedure
                        cmd.Parameters.Add("@FROMDATE", SqlDbType.DateTime).Direction = ParameterDirection.Input
                        cmd.Parameters.Add("@TODATE", SqlDbType.DateTime).Direction = ParameterDirection.Input
                        cmd.Parameters.Add("@AUDITTYPE", SqlDbType.VarChar).Direction = ParameterDirection.Input
                        cmd.Parameters.Add("@DATANO", SqlDbType.VarChar).Direction = ParameterDirection.Input
                        cmd.Parameters.Add("@PRPCODE", SqlDbType.VarChar, Int16.MaxValue).Direction = ParameterDirection.Input
                        cmd.Parameters.Add("@LocationStr", SqlDbType.VarChar, Int16.MaxValue).Direction = ParameterDirection.Input
                        '  cmd.Parameters.Add("@location", SqlDbType.VarChar).Direction = ParameterDirection.Input
                        cmd.Parameters("@FROMDATE").Value = fromdate
                        cmd.Parameters("@TODATE").Value = todate
                        cmd.Parameters("@AUDITTYPE").Value = audittype
                        cmd.Parameters("@DATANO").Value = datano
                        cmd.Parameters("@PRPCODE").Value = prpcode
                        cmd.Parameters("@LocationStr").Value = Locations
                        '  cmd.Parameters("@location").Value = GetAS400Abr(LocationId.ToString())

                        cmd.CommandTimeout = 5000

                        Dim bmap_rg As New BMappers(Of SingleObject)
                        rglist = bmap_rg.GetSpcSP(cmd)

                    End Using
                End Using
            Catch ex As Exception
                Return Nothing
            End Try
            If IsNothing(rglist) = False Then
                If rglist.Count > 0 Then
                    Return rglist.ToArray()(0).Object1
                End If
            End If
            Return -1
        End Function

        Public Function GetInspectionComplianceData(ByVal fromdate As DateTime, ByVal todate As DateTime) As List(Of SPCInspection.InspectionCompliance_Local)
            Dim con As New SqlConnection(DL.InspectConnectionString())
            Dim cmd As SqlCommand = con.CreateCommand()
            Dim rglist As New List(Of SPCInspection.InspectionCompliance_Local)

            Try
                Using con
                    con.Open()
                    Using cmd
                        cmd = New SqlCommand("GetInspectionComplianceData", con)
                        cmd.CommandType = CommandType.StoredProcedure
                        cmd.Parameters.Add("@FROMDATE", SqlDbType.DateTime).Direction = ParameterDirection.Input
                        cmd.Parameters.Add("@TODATE", SqlDbType.DateTime).Direction = ParameterDirection.Input
                        cmd.Parameters("@FROMDATE").Value = fromdate
                        cmd.Parameters("@TODATE").Value = todate

                        cmd.CommandTimeout = 5000

                        Dim bmap_rg As New BMappers(Of SPCInspection.InspectionCompliance_Local)
                        rglist = bmap_rg.GetSpcSP(cmd)

                    End Using
                End Using
            Catch ex As Exception
                Return Nothing
            End Try

            Return rglist


        End Function

        Public Function Getas400WOMatchBranch(ByVal fromdate As DateTime, ByVal todate As DateTime, as400location As String) As List(Of SPCInspection.InspectionCompliance)
            Dim con As New SqlConnection(DL.InspectConnectionString())
            Dim cmd As SqlCommand = con.CreateCommand()
            Dim rglist As New List(Of SPCInspection.InspectionCompliance)
            Try
                Using con
                    con.Open()
                    Using cmd
                        cmd = New SqlCommand("SP_AS400_InspectionCompliance", con)
                        cmd.CommandType = CommandType.StoredProcedure
                        cmd.Parameters.Add("@FROMDATE", SqlDbType.DateTime).Direction = ParameterDirection.Input
                        cmd.Parameters.Add("@TODATE", SqlDbType.DateTime).Direction = ParameterDirection.Input
                        cmd.Parameters.Add("@location", SqlDbType.VarChar, 250).Direction = ParameterDirection.Input
                        cmd.Parameters("@FROMDATE").Value = fromdate
                        cmd.Parameters("@TODATE").Value = todate
                        cmd.Parameters("@location").Value = as400location

                        cmd.CommandTimeout = 5000

                        Dim bmap_rg As New BMappers(Of SPCInspection.InspectionCompliance)
                        rglist = bmap_rg.GetSpcSP(cmd)

                    End Using
                End Using
            Catch ex As Exception
                Return Nothing
            End Try

            Return rglist
        End Function

        Public Function GetDefectMasterMonthlySum(ByVal Location As String, ByVal fromdate As DateTime, ByVal todate As DateTime) As List(Of SPCInspection.GraphTable)
            Dim con As New SqlConnection(DL.InspectConnectionString())
            Dim cmd As SqlCommand = con.CreateCommand()
            Dim todatestring As String = todate.ToString("yyyy-MM-dd H:mm:ss").Split(" ")(0) + " 23:59:59"
            Dim todateform As DateTime = DateTime.Parse(todatestring)
            Try
                Using con
                    con.Open()
                    Using cmd
                        cmd = New SqlCommand("SP_DefectMasterMonthlySum", con)
                        cmd.CommandType = CommandType.StoredProcedure

                        cmd.Parameters.Add("@fromdate", SqlDbType.DateTime).Direction = ParameterDirection.Input
                        cmd.Parameters.Add("@todate", SqlDbType.DateTime).Direction = ParameterDirection.Input
                        cmd.Parameters.Add("@Location", SqlDbType.NChar).Direction = ParameterDirection.Input
                        cmd.Parameters("@fromdate").Value = fromdate
                        cmd.Parameters("@todate").Value = todateform
                        cmd.Parameters("@Location").Value = Location
                        cmd.CommandTimeout = 5000

                        Dim list As New List(Of SPCInspection.GraphTable)
                        Dim wocheckarray As Array = _DAOFactory.GetDefectMasterDailyObjects(cmd, con).ToArray()
                        If wocheckarray.Length > 0 Then
                            If wocheckarray.Length > 0 Then
                                '       list.Add(New SPCInspection.GraphTable With {.Count = wocheckarray(0).Count, .Listdate = New DateTime(wocheckarray(0).Year, wocheckarray(0).Month, 1)})
                                '        Return list
                                '     Else
                                Dim startDate As DateTime = New DateTime(fromdate.Year, wocheckarray(0).Month, 1)
                                Dim endate As DateTime = todate
                                Dim M As Integer = Math.Abs((todate.Year - startDate.Year))

                                Dim elapsedTicks As Long = todate.Ticks - fromdate.Ticks
                                Dim elapsedSpan As New TimeSpan(elapsedTicks)
                                Dim Thismonth As DateTime = New DateTime(fromdate.Year, fromdate.Month, todate.Day)
                                While Thismonth <= endate
                                    Dim returnlistcnt = list.Count
                                    Dim indexflag As Boolean = False
                                    Dim valueflag As Boolean = False
                                    'Dim Listdate As DateTime = fromdate.AddMonths(i)
                                    Dim Listformdate As DateTime = New DateTime(Thismonth.Year, Thismonth.Month, 1)

                                    For Each item In wocheckarray
                                        Dim pointdate As DateTime = New DateTime(item.Year, item.Month, 1)
                                        If Listformdate = pointdate Then
                                            list.Add(New SPCInspection.GraphTable With {.Count = item.Count, .Listdate = pointdate.ToString("yyyy-MM-dd")})
                                            GoTo 101
                                        End If
                                    Next
                                    list.Add(New SPCInspection.GraphTable With {.Count = 0, .Listdate = Listformdate.ToString("yyyy-MM-dd")})

101:
                                    Thismonth = Thismonth.AddMonths(1)
                                End While
                            End If

                            Return list
                        Else
                            Return Nothing
                        End If

                    End Using
                    con.Close()
                End Using
            Catch ex As Exception
                Return Nothing
            End Try



        End Function


        Public Function GetDefectPieChart(ByVal Location As String, ByVal fromdate As DateTime, ByVal todate As DateTime, ByVal GroupBy As String) As List(Of PieChart)
            Dim con As New SqlConnection(DL.InspectConnectionString())
            Dim cmd As SqlCommand = con.CreateCommand()
            Try
                Using con
                    con.Open()
                    Using cmd
                        cmd = New SqlCommand("SP_DefectPieChart", con)
                        cmd.CommandType = CommandType.StoredProcedure
                        cmd.Parameters.Add("@fromdate", SqlDbType.DateTime).Direction = ParameterDirection.Input
                        cmd.Parameters.Add("@todate", SqlDbType.DateTime).Direction = ParameterDirection.Input
                        cmd.Parameters.Add("@GroupBy", SqlDbType.VarChar).Direction = ParameterDirection.Input
                        cmd.Parameters.Add("@Location", SqlDbType.NChar).Direction = ParameterDirection.Input
                        cmd.Parameters("@fromdate").Value = fromdate
                        cmd.Parameters("@todate").Value = todate
                        cmd.Parameters("@Location").Value = Location
                        cmd.Parameters("@GroupBy").Value = GroupBy
                        cmd.CommandTimeout = 5000

                        Dim list As New List(Of PieChart)
                        Dim Colors As String() = {"rgb(93, 135, 161)", "rgb(176, 181, 121)", "rgb(251, 176, 64)", "rgb(149, 160, 169)", "rgb(211, 18, 69)", "rgb(255, 222, 117)", "rgb(233, 227, 220)"}
                        Dim wocheckarray As Array = _DAOFactory.GetPieChartData(cmd, con).ToArray()
                        Dim othercnt As Integer = 1
                        Dim cnt As Integer = 0
                        If wocheckarray.Length > 0 Then
                            For Each item In wocheckarray
                                If IsNothing(item.desc) = True Or item.desc = "" Then
                                    item.desc = "Other_" + othercnt.ToString()
                                    othercnt += 1
                                End If
                                If Colors.Length < cnt + 1 Then
                                    GoTo 102
                                End If
                                If GroupBy = "DefectDesc" And item.desc = "NoDefect" Then
                                    GoTo 102
                                End If
                                list.Add(New PieChart With {.value = item.value, .label = item.desc, .color = Colors(cnt)})
                                cnt += 1
102:
                            Next

                            list.Sort(Function(x As PieChart, y As PieChart)
                                          If x.value = 0 AndAlso y.value = 0 Then
                                              Return 0
                                          ElseIf x.value = 0 Then
                                              Return -1
                                          ElseIf y.value = 0 Then
                                              Return 1
                                          Else
                                              Return x.value.CompareTo(y.value)
                                          End If
                                      End Function)
                            If Colors.Length < list.Count Then
                                Dim count = list.Count - Colors.Length
                                For i = 0 To count - 1
                                    list.RemoveAt(i)
                                Next
                            End If
                        End If

                        Return list
                    End Using
                    con.Close()
                End Using

            Catch ex As Exception
                Return Nothing
            End Try

        End Function

        Public Function GetDefectMasterDataTypeCount(ByVal Location As Integer, ByVal fromdate As DateTime, ByVal todate As DateTime, ByVal DataType As String) As Integer
            Dim con As New SqlConnection(DL.InspectConnectionString)
            Dim cmd As SqlCommand = con.CreateCommand()
            Try
                Using con
                    con.Open()
                    Using cmd
                        cmd = New SqlCommand("SP_CountDefectMasterDataType", con)
                        cmd.CommandType = CommandType.StoredProcedure

                        cmd.Parameters.Add("@fromdate", SqlDbType.DateTime).Direction = ParameterDirection.Input
                        cmd.Parameters.Add("@todate", SqlDbType.DateTime).Direction = ParameterDirection.Input
                        cmd.Parameters.Add("@DataType", SqlDbType.VarChar).Direction = ParameterDirection.Input
                        cmd.Parameters.Add("@LOCATIONID", SqlDbType.VarChar).Direction = ParameterDirection.Input
                        cmd.Parameters("@fromdate").Value = fromdate
                        cmd.Parameters("@todate").Value = todate
                        cmd.Parameters("@DataType").Value = DataType
                        cmd.Parameters("@LOCATIONID").Value = Location
                        cmd.CommandTimeout = 5000

                        Dim list As New List(Of SPCInspection.GraphTable)
                        Dim wocheckarray As Array = _DAOFactory.GetDefectMasterDataTypeCount(cmd, con).ToArray()
                        If IsNumeric(wocheckarray(0)) = True Then
                            Return wocheckarray(0)
                        Else
                            Return 0
                        End If

                    End Using
                    con.Close()
                End Using
            Catch ex As Exception
                Return Nothing
            End Try

        End Function

        Public Function GetDefectMasterHistogram(ByVal Location As String, ByVal fromdate As DateTime, ByVal todate As DateTime) As List(Of SPCInspection.BarChart)
            Dim con As New SqlConnection(DL.InspectConnectionString)
            Dim cmd As SqlCommand = con.CreateCommand()
            Dim returnlist As New List(Of SPCInspection.BarChart)
            Try
                Using con
                    con.Open()
                    Using cmd
                        cmd = New SqlCommand("SP_DefectMasterHistogram", con)
                        cmd.CommandType = CommandType.StoredProcedure
                        cmd.Parameters.Add("@fromdate", SqlDbType.DateTime).Direction = ParameterDirection.Input
                        cmd.Parameters.Add("@todate", SqlDbType.DateTime).Direction = ParameterDirection.Input
                        cmd.Parameters.Add("@Location", SqlDbType.NChar).Direction = ParameterDirection.Input
                        cmd.Parameters("@fromdate").Value = fromdate
                        cmd.Parameters("@todate").Value = todate
                        cmd.Parameters("@Location").Value = Location
                        cmd.CommandTimeout = 5000

                        Dim list As New List(Of SPCInspection.GraphTable)
                        returnlist = _DAOFactory.GetDefectMasterHistogram(cmd, con)

                    End Using
                    con.Close()
                End Using
            Catch ex As Exception
                Return Nothing
            End Try

            Return returnlist
        End Function

        Public Function GetJobSummary_1(ByVal Location As String, ByVal daysback As Integer, ByVal todate As DateTime, ByVal TemplateId As Integer) As List(Of SPCInspection.JobSummary)
            Dim con As New SqlConnection(DL.InspectConnectionString)
            Dim cmd As SqlCommand = con.CreateCommand()
            Dim list As New List(Of SPCInspection.JobSummary)
            Try
                Using con
                    con.Open()
                    Using cmd
                        cmd = New SqlCommand("SP_SPC_GetJobSummary_1", con)
                        cmd.CommandType = CommandType.StoredProcedure

                        cmd.Parameters.Add("@TEMPLATEID", SqlDbType.Int).Direction = ParameterDirection.Input
                        cmd.Parameters.Add("@DAYSBACK", SqlDbType.Int).Direction = ParameterDirection.Input
                        cmd.Parameters.Add("@TODATE", SqlDbType.DateTime).Direction = ParameterDirection.Input
                        cmd.Parameters.Add("@LOCATION", SqlDbType.VarChar).Direction = ParameterDirection.Input
                        cmd.Parameters("@TEMPLATEID").Value = TemplateId
                        cmd.Parameters("@DAYSBACK").Value = daysback
                        cmd.Parameters("@TODATE").Value = todate
                        cmd.Parameters("@LOCATION").Value = Location
                        cmd.CommandTimeout = 5000


                        Dim bmapjs As New BMappers(Of SPCInspection.JobSummary)

                        list = bmapjs.GetSpcSP(cmd)

                        If list.Count > 0 Then
                            Return list
                        Else
                            Return list
                        End If

                    End Using
                    con.Close()
                End Using
            Catch ex As Exception
                Return list
            End Try

        End Function
        Public Function GetJobSummary_2(ByVal Location As String, ByVal daysback As Integer, ByVal todate As DateTime, ByVal TemplateId As Integer) As List(Of Production.JobSummary_DBreakdown)
            Dim con As New SqlConnection(DL.InspectConnectionString)
            Dim cmd As SqlCommand = con.CreateCommand()
            Dim list As New List(Of Production.JobSummary_DBreakdown)
            Try
                Using con
                    con.Open()
                    Using cmd
                        cmd = New SqlCommand("SP_SPC_GetJobSummary_2", con)
                        cmd.CommandType = CommandType.StoredProcedure

                        cmd.Parameters.Add("@TEMPLATEID", SqlDbType.Int).Direction = ParameterDirection.Input
                        cmd.Parameters.Add("@DAYSBACK", SqlDbType.Int).Direction = ParameterDirection.Input
                        cmd.Parameters.Add("@TODATE", SqlDbType.DateTime).Direction = ParameterDirection.Input
                        cmd.Parameters.Add("@LOCATION", SqlDbType.VarChar).Direction = ParameterDirection.Input
                        cmd.Parameters("@TEMPLATEID").Value = TemplateId
                        cmd.Parameters("@DAYSBACK").Value = daysback
                        cmd.Parameters("@TODATE").Value = todate
                        cmd.Parameters("@LOCATION").Value = Location
                        cmd.CommandTimeout = 5000


                        Dim bmapjs As New BMappers(Of Production.JobSummary_DBreakdown)

                        list = bmapjs.GetSpcSP(cmd)

                        

                    End Using
                    con.Close()
                End Using
            Catch ex As Exception
                Return Nothing
            End Try

            Return list

        End Function

        Public Function GetDefectMasterWorkOrderCount(ByVal Location As Integer, ByVal fromdate As DateTime, ByVal todate As DateTime) As Integer
            Dim con As New SqlConnection(DL.InspectConnectionString)
            Dim cmd As SqlCommand = con.CreateCommand()
            Try
                Using con
                    con.Open()
                    Using cmd
                        cmd = New SqlCommand("SP_DefectMasterWorkOrderCount", con)
                        cmd.CommandType = CommandType.StoredProcedure

                        cmd.Parameters.Add("@fromdate", SqlDbType.DateTime).Direction = ParameterDirection.Input
                        cmd.Parameters.Add("@todate", SqlDbType.DateTime).Direction = ParameterDirection.Input
                        cmd.Parameters.Add("@LOCATIONID", SqlDbType.VarChar).Direction = ParameterDirection.Input
                        cmd.Parameters("@fromdate").Value = fromdate
                        cmd.Parameters("@todate").Value = todate
                        cmd.Parameters("@LOCATIONID").Value = Location
                        cmd.CommandTimeout = 5000

                        Dim list As New List(Of SPCInspection.GraphTable)
                        Dim wocheckarray As Array = _DAOFactory.GetDefectMasterDataTypeCount(cmd, con).ToArray()
                        If IsNumeric(wocheckarray(0)) = True Then
                            Return wocheckarray(0)
                        Else
                            Return 0
                        End If

                    End Using
                    con.Close()
                End Using
            Catch ex As Exception
                Return Nothing
            End Try



        End Function

        Public Function GetDefectImage(ByVal DefectId As Integer) As List(Of SPCInspection.DefectMaster)
            Dim returnlist As New List(Of SPCInspection.DefectMaster)
            Dim ImageReader As SqlDataReader
            Dim record As IDataRecord
            Dim sqlstring As String

            sqlstring = "SELECT DefectImage, DefectImage_Filename" & vbCrLf &
            "FROM DefectMaster" & vbCrLf &
            "WHERE (DefectID = @DefectId)"

            Using con As New SqlConnection(DL.InspectConnectionString())
                con.Open()
                Dim cmd As New SqlCommand(sqlstring, con)
                cmd.Parameters.Add(_DAOFactory.Getparameter("@DefectId", DbType.Int32))
                cmd.Parameters("@DefectId").Value = DefectId

                Try
                    ImageReader = cmd.ExecuteReader(CommandBehavior.CloseConnection)

                    If ImageReader.FieldCount > 0 Then
                        While ImageReader.Read
                            record = CType(ImageReader, IDataRecord)

                            If IsDBNull(record(0)) = True Then
                                Return Nothing
                            End If
                            Try
                                returnlist.Add(New SPCInspection.DefectMaster With {.DefectImage = record(0), .DefectImage_Filename = Convert.ToString(record(1))})
                            Catch ex As Exception

                            End Try

                        End While

                        Return returnlist
                    Else
                        Return Nothing
                    End If

                Catch ex As Exception
                    Return Nothing
                End Try



            End Using


        End Function

        Public Function TemplateInsert(ByVal TemplateId As Integer, ByVal Tab_Array As Object, ByVal Button_Array As Object) As Boolean

            For i = 0 To Tab_Array.Count - 1
                If CheckForTab(TemplateId, Tab_Array(i).title, i) = True Then
                    Return False
                    Exit Function
                End If
                Dim returnint As Integer = InsertTab(TemplateId, Tab_Array(i).title, i)
                If returnint = 0 Then
                    Return False
                    Exit Function
                End If
                For Each Button As SPCInspection.buttonarray In Button_Array
                    If Button.tabindex = i Then
                        InsertButton(returnint, Button.ButtonId, Button.text, Button.DefectType, Button.Timer)
                    End If
                Next
            Next
            ActiveTemplate(TemplateId)
            Return True

        End Function

        Public Function TemplateUpdate(ByVal TemplateId As Integer, ByVal Tab_Array As Object, ByVal Button_Array As Object) As Boolean

            For i = 0 To Tab_Array.Count - 1
                Dim TabTemplateId As Integer

                If CheckForTab(TemplateId, Tab_Array(i).title, i) = True And Tab_Array(i).TabTemplateId <> 0 Then
                    'DeleteResult = DeleteButtonTemplate(Tab_Array(i).TabTemplateId)
                    TabTemplateId = Tab_Array(i).TabTemplateId

                    If TabTemplateId > 0 Then
                        For Each Button As SPCInspection.buttonarray In Button_Array
                            If Button.tabindex = i Then
                                'If Not InsertButton(TabTemplateId, Button.ButtonId, Button.text, Button.DefectType) Then
                                '    Return False
                                'End If
                                'If Button.DefectType <> "False" And Button.DefectType <> "True" Then
                                '    Button.DefectType = "True"
                                'End If
                                If Button.id <> -1 Then
                                    Dim sql As String = "UPDATE       ButtonTemplate" & vbCrLf &
                                                        "SET ButtonId = @ButtonId, DefectType = @DefectType, Hide = @Hide, Timer = @Timer " & vbCrLf &
                                                        "WHERE (id = @id)"
                                    Dim btobj As New SPCInspection.ButtonTemplate
                                    btobj.ButtonId = Button.ButtonId
                                    btobj.DefectType = Button.DefectType
                                    btobj.id = Button.id
                                    btobj.Hide = Button.Hide
                                    btobj.Timer = Button.Timer
                                    If Not bmap_1.InsertSpcObject(sql, btobj) Then
                                        Return False
                                    End If
                                Else
                                    Dim sql As String = "INSERT INTO ButtonTemplate" & vbCrLf &
                                                         "(ButtonId, DefectType, TabTemplateId, Timer)" & vbCrLf &
                                                         "VALUES (@ButtonId,@DefectType,@TabTemplateId, @Timer )"
                                    Dim btobj As New SPCInspection.ButtonTemplate
                                    btobj.ButtonId = Button.ButtonId
                                    btobj.DefectType = Button.DefectType
                                    btobj.TabTemplateId = TabTemplateId
                                    btobj.Timer = Button.Timer
                                    If Not bmap_1.InsertSpcObject(sql, btobj) Then
                                        Return False
                                    End If
                                End If

                            End If
                        Next
                    Else
                        Return False
                    End If
                Else
                    TabTemplateId = InsertTab(TemplateId, Tab_Array(i).title, i)

                    If TabTemplateId > 0 Then
                        For Each Button As SPCInspection.buttonarray In Button_Array
                            If Button.tabindex = i Then
                                'If Not InsertButton(TabTemplateId, Button.ButtonId, Button.text, Button.DefectType) Then
                                '    Return False
                                'End If
                                'If Button.DefectType <> "False" And Button.DefectType <> "True" Then
                                '    Button.DefectType = "True"
                                'End If
                                Dim sql As String = "INSERT INTO ButtonTemplate" & vbCrLf &
                                                         "(ButtonId, DefectType, TabTemplateId)" & vbCrLf &
                                                         "VALUES (@ButtonId,@DefectType,@TabTemplateId)"
                                Dim btobj As New SPCInspection.ButtonTemplate
                                btobj.ButtonId = Button.ButtonId
                                btobj.DefectType = Button.DefectType
                                btobj.TabTemplateId = TabTemplateId
                                If Not bmap_1.InsertSpcObject(sql, btobj) Then
                                    Return False
                                End If
                            End If
                        Next
                    End If

                End If


            Next

            Return True

        End Function

        Private Function CheckForTab(ByVal TemplateId As Integer, ByVal Name As String, ByVal TabNumber As Integer) As Boolean
            Dim sqlstring As String
            Dim TabSet As DataSet = New DataSet
            sqlstring = "select * from TabTemplate where TemplateId = '" & TemplateId.ToString() & "' and Name = '" & Name & "'"


            If Not util.FillSPCDataSet(TabSet, "TabSet", sqlstring) Then
                Return False
            End If
            Dim id As Integer = TabSet.Tables(0).Rows.Count

            If id > 0 Then
                Return True
            Else
                Return False
            End If

        End Function

        Public Function CheckSpecTemplate(ByVal TemplateId As Integer) As Boolean
            Dim sqlstring As String
            Dim TabSet As DataSet = New DataSet
            sqlstring = "SELECT        COUNT(SpecMeasurements.SpecId) AS CNT" & vbCrLf &
                        "FROM            SpecMeasurements INNER JOIN" & vbCrLf &
                        "TabTemplate ON SpecMeasurements.TabTemplateId = TabTemplate.TabTemplateId" & vbCrLf &
                        "GROUP BY TabTemplate.TemplateId" & vbCrLf &
                        "HAVING (TabTemplate.TemplateId = " & TemplateId.ToString() & ")"


            If Not util.FillSPCDataSet(TabSet, "TabSet", sqlstring) Then
                Return False
            End If
            Dim id As Integer = TabSet.Tables(0).Rows.Count

            If id > 0 Then
                Dim valuecnt = Convert.ToInt32(TabSet.Tables(0).Rows(0)("CNT"))
                If valuecnt > 0 Then
                    Return True
                Else
                    Return False
                End If
            Else
                Return False
            End If

        End Function


        Public Function InsertSpecMeasurement(ByVal SpecId As Integer, ByVal DefectId As Integer, ByVal InspectionId As Integer, ByVal InspectionJobSummaryId As Integer, ByVal MeasureValue As Decimal, ByVal SpecItemCount As Integer, ByVal SpecDelta As Decimal) As Boolean
            Dim con As New SqlConnection(DL.InspectConnectionString)
            Dim cmd As SqlCommand = con.CreateCommand()
            Dim returnint As Integer
            Dim Sql As String

            Try
                Using con
                    con.Open()
                    Using cmd
                        Sql = "INSERT INTO SpecMeasurements" & vbCrLf &
                         "(SpecId, DefectId, InspectionId, InspectionJobSummaryId, Timestamp, MeasureValue, ItemNumber, SpecDelta)" & vbCrLf &
                         "VALUES (@SpecId, @DefectId, @InspectionId, @InspectionJobSummaryId, GETDATE(),@MeasureValue, @ItemNumber, @SpecDelta)" & vbCrLf &
                         "SELECT SCOPE_IDENTITY();"
                        cmd = _DAOFactory.GetCommand(Sql, con)
                        ''    'Add command parameters                                                                          
                        cmd.Parameters.Add(_DAOFactory.Getparameter("@SpecId", DbType.Int32))
                        cmd.Parameters.Add(_DAOFactory.Getparameter("@DefectId", DbType.Int32))
                        cmd.Parameters.Add(_DAOFactory.Getparameter("@InspectionId", DbType.Int32))
                        cmd.Parameters.Add(_DAOFactory.Getparameter("@InspectionJobSummaryId", DbType.Int32))
                        cmd.Parameters.Add(_DAOFactory.Getparameter("@MeasureValue", DbType.Decimal))
                        cmd.Parameters.Add(_DAOFactory.Getparameter("@ItemNumber", DbType.Int32))
                        cmd.Parameters.Add(_DAOFactory.Getparameter("@SpecDelta", DbType.Decimal))
                        If DefectId = 0 Then
                            cmd.Parameters("@DefectId").Value = 0
                        Else
                            cmd.Parameters("@DefectId").Value = DefectId
                        End If

                        If InspectionId = 0 Then
                            cmd.Parameters("@InspectionId").Value = DBNull.Value
                        Else
                            cmd.Parameters("@InspectionId").Value = InspectionId
                        End If
                        cmd.Parameters("@InspectionJobSummaryId").Value = InspectionJobSummaryId
                        Dim listso As New List(Of SingleObject)
                        If InspectionJobSummaryId > 0 Then
                            Dim bmapso As New BMappers(Of SingleObject)

                            listso = bmapso.GetInspectObject("SELECT CID as Object1 FROM InspectionJobSummary WHERE id = " & InspectionJobSummaryId.ToString())

                        End If
                        cmd.Parameters("@SpecId").Value = SpecId
                        cmd.Parameters("@MeasureValue").Value = MeasureValue
                        cmd.Parameters("@ItemNumber").Value = SpecItemCount
                        cmd.Parameters("@SpecDelta").Value = SpecDelta
                        Try

                            returnint = Convert.ToInt32(cmd.ExecuteScalar())
                            If returnint = 0 Then
                                Return False
                            ElseIf returnint > 0 And listso.Count > 0 Then
                                InspectionInputDAO.RegisterSpecCache(returnint, listso.ToArray()(0).Object1.ToString())
                            End If
                        Catch e As Exception
                            Throw New System.Exception(e.Message)
                            Return False
                            Exit Function
                        End Try
                        con.Close()
                    End Using
                End Using
            Catch ex As Exception
                Return False
            End Try

            Return True
        End Function

        Public Sub UpdateTemplateCollectionCache(ByVal TemplateId As Integer)

            Dim selectValues As New List(Of SPCInspection.TemplateCollection)()
            selectValues = GetInputTemplateCollection(TemplateId)
            If selectValues.Count > 0 Then
                Dim dlayer As New dlayer
                Dim jser As New JavaScriptSerializer
                'Dim CIDArray = dlayer.GetCIDInfo().ToArray()
                If selectValues.Count > 0 Then
                    Dim TemplateCollectionCache As String
                    TemplateCollectionCache = jser.Serialize(selectValues)
                    Dim Cachestring As String = TemplateId.ToString() + "TemplateCollection"
                    dlayer.InsertCacheObject(Cachestring, TemplateCollectionCache, 5)
                End If
            End If
        End Sub

        Public Function GetRollInspectionSummaryHeaders(ByVal datefrom As DateTime, ByVal dateto As DateTime) As List(Of SPCInspection.RollInspectionSummaryHeaders)
            Dim con As New SqlConnection(DL.InspectConnectionString)
            Dim cmd As SqlCommand = con.CreateCommand()
            Dim readerlist As New List(Of SPCInspection.RollInspectionSummaryHeaders)
            Dim Sql As String
            Dim datefromstring As String
            Dim datetostring As String

            datefromstring = datefrom.ToString("MM/dd/yyyy HH:mm:ss", CultureInfo.InvariantCulture)
            datetostring = dateto.ToString("MM/dd/yyyy  HH:mm:ss", CultureInfo.InvariantCulture)

            Sql = "SELECT RollStartTimestamp, LoomNo, RollNumber, Style, Yards_Inspected, DefectiveYards, DHY" & vbCrLf &
                    "FROM RollInspectionSummary" & vbCrLf &
                    "WHERE (RollStartTimestamp >= CONVERT(DATETIME, '" & datefromstring & "', 101)) AND (RollStartTimestamp <= CONVERT(DATETIME, '" & datetostring & "', 101))"

            readerlist = _DAOFactory.getRollInspectionSummaryHeaders(Sql)

            Return readerlist
        End Function

        Public Function GetInspectionSummary(ByVal fromdate As DateTime, ByVal todate As DateTime) As List(Of SPCInspection.InspectionSummaryDisplay)

            Dim listis As New List(Of SPCInspection.InspectionSummaryDisplay)
            Dim bmapis As New BMappers(Of SPCInspection.InspectionSummaryDisplay)

            'Dim sql As String = "SELECT ijs.id, ijs.JobType, ijs.JobNumber, ijs.UnitDesc, SUBSTRING(lm.CID, 4, 3) as CID, lm.Abreviation AS Location, CASE WHEN ijs.Technical_PassFail = 0 THEN  'FAIL' WHEN ijs.Technical_PassFail = 1 THEN  'PASS' END AS Technical_PassFail, CONVERT(VARCHAR(20),  ISNULL(ijs.Inspection_Started, ''), 1) AS STARTED, ISNULL(convert(varchar(20), ijs.Inspection_Finished), '') AS FINISHED, CASE WHEN ijs.SampleSize > 0 THEN (SELECT cast(count(*) as float) FROM DefectMaster WHERE InspectionJobSummaryId = ijs.id) * 100/ijs.SampleSize ELSE 0 END AS DHU, CASE WHEN ijs.SampleSize > 0 THEN (SELECT cast(count(*) as float) FROM DefectMaster WHERE InspectionJobSummaryId = ijs.id AND DefectClass = 'MAJOR') * 100/ijs.SampleSize ELSE 0 END AS RejectionRate, ijs.UnitCost FROM InspectionJobSummary ijs LEFT OUTER JOIN AprManager.dbo.LocationMaster lm on ijs.CID = SUBSTRING(lm.CID, 4, 3) WHERE (ijs.Inspection_Started >= CONVERT(DATETIME, '" & fromdate.ToString("yyyy-MM-dd") & "', 101)) AND (ijs.Inspection_Started <= CONVERT(DATETIME, '" & todate.AddDays(1).ToString("yyyy-MM-dd") & "', 101)) ORDER BY ijs.id DESC"
            'Dim sql As String = "SELECT ijs.id, ijs.JobType, ijs.JobNumber, ijs.UnitDesc, SUBSTRING(lm.CID, 4, 3) as CID, lm.Abreviation AS Location, CASE WHEN ijs.Technical_PassFail = 0 THEN  'FAIL' WHEN ijs.Technical_PassFail = 1 THEN  'PASS' END AS Technical_PassFail, CONVERT(VARCHAR(20),  ISNULL(ijs.Inspection_Started, ''), 1) AS STARTED, ISNULL(convert(varchar(20), ijs.Inspection_Finished), '') AS FINISHED, CASE WHEN ijs.SampleSize > 0 THEN cast((MajorsCount + MinorsCount + RepairsCount + ScrapCount) as float) * 100/ijs.SampleSize ELSE 0 END AS DHU, CASE WHEN ijs.SampleSize > 0 THEN cast((MajorsCount) as float) * 100/ijs.SampleSize ELSE 0 END AS RejectionRate, ijs.UnitCost FROM InspectionJobSummary ijs LEFT OUTER JOIN AprManager.dbo.LocationMaster lm on ijs.CID = SUBSTRING(lm.CID, 4, 3) WHERE (ijs.Inspection_Started >= CONVERT(DATETIME, '" & fromdate.ToString("yyyy-MM-dd") & "', 101)) AND (ijs.Inspection_Started <= CONVERT(DATETIME, '" & todate.AddDays(1).ToString("yyyy-MM-dd") & "', 101)) ORDER BY ijs.id DESC"
            Dim sql As String = "SELECT ijs.id, ijs.JobType, ijs.JobNumber, ijs.DataNo, ijs.UnitDesc, SUBSTRING(lm.CID, 4, 3) as CID, lm.Abreviation AS Location, tn.TemplateId, tn.Name, it.Name as LineType, ijs.TotalInspectedItems, ijs.ItemPassCount, ijs.ItemFailCount, ijs.WOQuantity, ijs.WorkOrderPieces, ijs.AQL_Level, ijs.SampleSize, ijs.RejectLimiter,  CASE WHEN ijs.Technical_PassFail = 0 THEN  'FAIL' WHEN ijs.Technical_PassFail = 1 THEN  'PASS' END AS Technical_PassFail, Inspection_Started AS STARTED, ijs.Inspection_Finished  AS FINISHED, ijs.PRP_Code, CASE WHEN ijs.SampleSize > 0 THEN cast((MajorsCount + MinorsCount + RepairsCount + ScrapCount) as float) * 100/ijs.SampleSize ELSE 0 END AS DHU, CASE WHEN ijs.SampleSize > 0 THEN cast((MajorsCount) as float) * 100/ijs.SampleSize ELSE 0 END AS RejectionRate, ijs.UnitCost, ijs.Comments FROM InspectionJobSummary ijs LEFT OUTER JOIN " + AprManagerDb + ".dbo.LocationMaster lm on ijs.CID = SUBSTRING(lm.CID, 4, 3) LEFT OUTER JOIN TemplateName tn ON ijs.TemplateId = tn.TemplateId LEFT OUTER JOIN InspectionTypes it on it.id = tn.LineTypeId WHERE (ijs.Inspection_Started >= CONVERT(DATETIME, '" & fromdate.ToString("yyyy-MM-dd") & "', 101)) AND (ijs.Inspection_Started <= CONVERT(DATETIME, '" & todate.AddDays(1).ToString("yyyy-MM-dd") & "', 101)) ORDER BY ijs.id DESC"
            listis = bmapis.GetInspectObject(sql)

            Return listis
        End Function

        Public Function GetSpecSummary(ByVal fromdate As DateTime, ByVal todate As DateTime) As List(Of SPCInspection.SpecSummaryDisplay)

            Dim listis As New List(Of SPCInspection.SpecSummaryDisplay)
            Dim bmapis As New BMappers(Of SPCInspection.SpecSummaryDisplay)
            Dim sql As String = "select sq.* from (" & vbCrLf &
                            "SELECT ijs.id, ijs.JobNumber, ijs.UnitDesc, ijs.DataNo, lm.Abreviation AS Location, CAST(CAST(lm.CID AS INT) AS VARCHAR(7)) as CID , tn.LineType as LineTypeVariable, it.Name AS LineType, CONVERT(VARCHAR(20),  ISNULL(ijs.Inspection_Started, '')) AS Inspection_Started, ISNULL(convert(varchar(20), ijs.Inspection_Finished), '') AS Inspection_Finished, (SELECT COUNT(*) FROM SpecMeasurements sm WHERE sm.InspectionJobSummaryId = ijs.id) AS totcount, (SELECT COUNT(*) FROM SpecMeasurements sm INNER JOIN ProductSpecification ps ON ps.SpecId = sm.SpecId WHERE (sm.InspectionJobSummaryId = ijs.id) and (sm.SpecDelta <= ps.Upper_Spec_Value) and (sm.SpecDelta >= ps.Lower_Spec_Value)) as SpecsMet, (SELECT COUNT(*) FROM SpecMeasurements sm INNER JOIN ProductSpecification ps ON ps.SpecId = sm.SpecId WHERE (sm.InspectionJobSummaryId = ijs.id) and (sm.SpecDelta > ps.Upper_Spec_Value or sm.SpecDelta < ps.Lower_Spec_Value)) as SpecsFailed FROM InspectionJobSummary ijs LEFT OUTER JOIN " + AprManagerDb + ".dbo.LocationMaster lm on CAST(lm.CID AS INT) = CAST(ijs.CID AS INT) LEFT OUTER JOIN TemplateName tn ON tn.TemplateId = ijs.TemplateId  LEFT OUTER JOIN InspectionTypes it on it.id = tn.LineTypeId WHERE (ijs.Inspection_Started <= CONVERT(DATETIME, '" & todate.AddDays(1).ToString("yyyy-MM-dd") & "', 101)) AND (ijs.Inspection_Started >= CONVERT(DATETIME, '" & fromdate.ToString("yyyy-MM-dd") & "', 101))" & vbCrLf &
                            ") sq where sq.totcount > 0 order by sq.Inspection_Started desc"

            listis = bmapis.GetInspectObject(sql)

            Return listis
        End Function

        Public Function getDumpData(ByVal fromdate As DateTime, ByVal todate As DateTime) As List(Of SPCInspection.Dump)
            Dim listis As New List(Of SPCInspection.Dump)
            Dim bmapis As New BMappers(Of SPCInspection.Dump)
            Dim sql As String = "select * from dbo.INS_Summary_VW WHERE (Inspection_Started >= CONVERT(DATETIME, '" & fromdate.ToString("yyyy-MM-dd") & "', 101)) AND (Inspection_Started <= CONVERT(DATETIME, '" & todate.AddDays(1).ToString("yyyy-MM-dd") & "', 101))  order by id desc"

            listis = bmapis.GetInspectObject(sql)

            Return listis

        End Function

        Public Function getDfectTimerReport(ByVal fromdate As DateTime, ByVal todate As DateTime) As List(Of SPCInspection.TimerReport)
            Dim listdtr As New List(Of SPCInspection.TimerReport)
            Dim bmapdtr As New BMappers(Of SPCInspection.TimerReport)
            Dim sql As String = "select ijs.JobType, ijs.JobNumber, lm.Name AS Location, lm.CID, ijs.DataNo, ijs.UnitDesc, bl.Name as DefectName, bt.DefectType, dm.EmployeeNo, dt.Timestamp, dt.StopTimestamp, ISNULL(DATEDIFF(MINUTE, dt.Timestamp, StopTimestamp), 0) as Timespan_min from DefectTimer dt INNER JOIN InspectionJobSummary ijs on dt.InspectionJobSummaryId = ijs.id INNER JOIN ButtonTemplate bt ON dt.ButtonTemplateId = bt.id inner join ButtonLibrary bl ON bt.ButtonId = bl.ButtonId inner join " + AprManagerDb + ".dbo.LocationMaster lm ON ijs.CID = substring(lm.CID, 4,3) inner join DefectMaster dm on dt.DefectID = dm.DefectID where dt.Timestamp <= CONVERT(DATETIME, '" + todate.AddDays(1).ToString("yyyy-MM-dd") + "') AND dt.Timestamp >= CONVERT(DATETIME, '" + fromdate.ToString("yyyy-MM-dd") + "')"

            listdtr = bmapdtr.GetInspectObject(sql)

            Return listdtr
        End Function

        Public Function GetSpecSummaryUnFinished(ByVal fromdate As DateTime, ByVal todate As DateTime) As List(Of SPCInspection.SpecSummaryDisplay)

            Dim listis As New List(Of SPCInspection.SpecSummaryDisplay)
            Dim bmapis As New BMappers(Of SPCInspection.SpecSummaryDisplay)
            Dim sql As String = "select sq.* from (" & vbCrLf &
                            "SELECT ijs.id, ijs.JobNumber, ijs.UnitDesc, ijs.DataNo, lm.Abreviation AS Location, SUBSTRING(lm.CID, 4, 3) as CID , CONVERT(VARCHAR(20),  ISNULL(ijs.Inspection_Started, '')) AS Inspection_Started, ISNULL(convert(varchar(20), ijs.Inspection_Finished), '') AS Inspection_Finished, (SELECT COUNT(*) FROM SpecMeasurements sm WHERE sm.InspectionJobSummaryId = ijs.id) AS totcount, (SELECT COUNT(*) FROM SpecMeasurements sm INNER JOIN ProductSpecification ps ON ps.SpecId = sm.SpecId WHERE (sm.InspectionJobSummaryId = ijs.id) and (sm.SpecDelta <= ps.Upper_Spec_Value) and (sm.SpecDelta >= ps.Lower_Spec_Value)) as SpecsMet, (SELECT COUNT(*) FROM SpecMeasurements sm INNER JOIN ProductSpecification ps ON ps.SpecId = sm.SpecId WHERE (sm.InspectionJobSummaryId = ijs.id) and (sm.SpecDelta > ps.Upper_Spec_Value or sm.SpecDelta < ps.Lower_Spec_Value)) as SpecsFailed FROM InspectionJobSummary ijs LEFT OUTER JOIN " + AprManagerDb + ".dbo.LocationMaster lm on ijs.CID = SUBSTRING(lm.CID, 4, 3) WHERE (ijs.Inspection_Started <= CONVERT(DATETIME, '" & todate.AddDays(1).ToString("yyyy-MM-dd") & "', 101)) AND (ijs.Inspection_Started >= CONVERT(DATETIME, '" & fromdate.ToString("yyyy-MM-dd") & "', 101)) AND (ijs.Inspection_Finished is null)" & vbCrLf &
                            ") sq where sq.totcount > 0 order by sq.Inspection_Started desc"

            listis = bmapis.GetInspectObject(sql)

            Return listis
        End Function

        Public Function GetInspectionSummaryDay(ByVal fromdate As DateTime, ByVal todate As DateTime) As List(Of SPCInspection.InspectionSummaryDisplay)

            Dim listis As New List(Of SPCInspection.InspectionSummaryDisplay)
            Dim bmapis As New BMappers(Of SPCInspection.InspectionSummaryDisplay)
            'Dim sql As String = "SELECT ijs.id, ijs.JobType, ijs.JobNumber, ijs.UnitDesc, SUBSTRING(lm.CID, 4, 3) as CID, lm.Abreviation AS Location, CASE WHEN ijs.Technical_PassFail = 0 THEN  'FAIL' WHEN ijs.Technical_PassFail = 1 THEN  'PASS' END AS Technical_PassFail, CONVERT(VARCHAR(20),  ISNULL(ijs.Inspection_Started, ''), 1) AS STARTED, ISNULL(convert(varchar(20), ijs.Inspection_Finished), '') AS FINISHED, CASE WHEN ijs.SampleSize > 0 THEN (SELECT cast(count(*) as float) FROM DefectMaster WHERE InspectionJobSummaryId = ijs.id) * 100/ijs.SampleSize ELSE 0 END AS DHU, CASE WHEN ijs.SampleSize > 0 THEN (SELECT cast(count(*) as float) FROM DefectMaster WHERE InspectionJobSummaryId = ijs.id AND DefectClass = 'MAJOR') * 100/ijs.SampleSize ELSE 0 END AS RejectionRate, ijs.UnitCost FROM InspectionJobSummary ijs LEFT OUTER JOIN AprManager.dbo.LocationMaster lm on ijs.CID = SUBSTRING(lm.CID, 4, 3) WHERE (ijs.Inspection_Started >= CONVERT(DATETIME, '" & fromdate.ToString("yyyy-MM-dd") & "', 101)) AND (ijs.Inspection_Started < CONVERT(DATETIME, '" & todate.AddDays(1).ToString("yyyy-MM-dd") & "', 101)) ORDER BY ijs.id DESC"
            'Dim sql As String = "SELECT ijs.id, ijs.JobType, ijs.JobNumber, ijs.UnitDesc, SUBSTRING(lm.CID, 4, 3) as CID, lm.Abreviation AS Location, CASE WHEN ijs.Technical_PassFail = 0 THEN  'FAIL' WHEN ijs.Technical_PassFail = 1 THEN  'PASS' END AS Technical_PassFail, CONVERT(VARCHAR(20),  ISNULL(ijs.Inspection_Started, ''), 1) AS STARTED, ISNULL(convert(varchar(20), ijs.Inspection_Finished), '') AS FINISHED, CASE WHEN ijs.SampleSize > 0 THEN cast((MajorsCount + MinorsCount + RepairsCount + ScrapCount) as float) * 100/ijs.SampleSize ELSE 0 END AS DHU, CASE WHEN ijs.SampleSize > 0 THEN cast((MajorsCount) as float) * 100/ijs.SampleSize ELSE 0 END AS RejectionRate, ijs.UnitCost FROM InspectionJobSummary ijs LEFT OUTER JOIN AprManager.dbo.LocationMaster lm on ijs.CID = SUBSTRING(lm.CID, 4, 3) WHERE (ijs.Inspection_Started >= CONVERT(DATETIME, '" & fromdate.ToString("yyyy-MM-dd") & "', 101)) AND (ijs.Inspection_Started <= CONVERT(DATETIME, '" & todate.AddDays(1).ToString("yyyy-MM-dd") & "', 101)) ORDER BY ijs.id DESC"
            Dim sql As String = "SELECT ijs.id AS ijsid, ijs.JobType, ijs.JobNumber, ijs.DataNo, ijs.UnitDesc, CAST(CAST(lm.CID AS INT) AS VARCHAR) as CID, lm.Abreviation AS Location, tn.TemplateId, tn.Name, tn.LineType AS LineTypeVariable, it.Name as LineType, ijs.TotalInspectedItems, ijs.ItemPassCount, ijs.ItemFailCount, ijs.WOQuantity, ijs.WorkOrderPieces, ijs.AQL_Level, ijs.SampleSize, ijs.RejectLimiter,  CASE WHEN ijs.Technical_PassFail = 0 THEN  'FAIL' WHEN ijs.Technical_PassFail = 1 THEN  'PASS' END AS Technical_PassFail, Inspection_Started AS STARTED, ijs.Inspection_Finished  AS FINISHED, ijs.PRP_Code, ijs.MajorsCount, ijs.MinorsCount, ijs.ScrapCount, ijs.RepairsCount, CASE WHEN ijs.SampleSize > 0 THEN cast((MajorsCount + MinorsCount + RepairsCount + ScrapCount) as float) * 100/ijs.SampleSize ELSE 0 END AS DHU, CASE WHEN ijs.SampleSize > 0 THEN cast((MajorsCount) as float) * 100/ijs.SampleSize ELSE 0 END AS RejectionRate, ijs.UnitCost, ijs.Comments FROM InspectionJobSummary ijs LEFT OUTER JOIN " + AprManagerDb + ".dbo.LocationMaster lm on CAST(ijs.CID AS INT) = CAST(lm.CID AS INT) LEFT OUTER JOIN TemplateName tn ON ijs.TemplateId = tn.TemplateId LEFT OUTER JOIN InspectionTypes it ON tn.LineTypeId = it.id WHERE (ijs.Inspection_Started >= CONVERT(DATETIME, '" & fromdate.ToString("yyyy-MM-dd") & "', 101)) AND (ijs.Inspection_Started <= CONVERT(DATETIME, '" & todate.AddDays(1).ToString("yyyy-MM-dd") & "', 101)) ORDER BY ijs.id DESC"

            listis = bmapis.GetInspectObject(sql)

            Return listis
        End Function

        Public Function GetInspectionSummaryDayUnFinished(ByVal fromdate As DateTime, ByVal todate As DateTime) As List(Of SPCInspection.InspectionSummaryDisplay)

            Dim listis As New List(Of SPCInspection.InspectionSummaryDisplay)
            Dim bmapis As New BMappers(Of SPCInspection.InspectionSummaryDisplay)
            Dim sql As String = "SELECT ijs.id AS ijsid, ijs.JobType, ijs.JobNumber, ijs.DataNo, ijs.UnitDesc, SUBSTRING(lm.CID, 4, 3) as CID, lm.Abreviation AS Location, tn.TemplateId, tn.Name, tn.LineType AS LineTypeVariable, it.Name AS LineType, ijs.TotalInspectedItems, ijs.ItemPassCount, ijs.ItemFailCount, ijs.WOQuantity, ijs.WorkOrderPieces, ijs.AQL_Level, ijs.SampleSize, ijs.RejectLimiter,  CASE WHEN ijs.Technical_PassFail = 0 THEN  'FAIL' WHEN ijs.Technical_PassFail = 1 THEN  'PASS' END AS Technical_PassFail, Inspection_Started AS STARTED, ijs.Inspection_Finished  AS FINISHED, ijs.PRP_Code, CASE WHEN ijs.SampleSize > 0 THEN cast((MajorsCount + MinorsCount + RepairsCount + ScrapCount) as float) * 100/ijs.SampleSize ELSE 0 END AS DHU, CASE WHEN ijs.SampleSize > 0 THEN cast((MajorsCount) as float) * 100/ijs.SampleSize ELSE 0 END AS RejectionRate, ijs.UnitCost, ijs.Comments FROM InspectionJobSummary ijs LEFT OUTER JOIN " + AprManagerDb + ".dbo.LocationMaster lm on ijs.CID = SUBSTRING(lm.CID, 4, 3) LEFT OUTER JOIN TemplateName tn ON ijs.TemplateId = tn.TemplateId LEFT OUTER JOIN InspectionTypes it on it.id = tn.LineTypeId WHERE (ijs.Inspection_Started >= CONVERT(DATETIME, '" & fromdate.ToString("yyyy-MM-dd") & "', 101)) AND (ijs.Inspection_Started < CONVERT(DATETIME, '" & todate.AddDays(1).ToString("yyyy-MM-dd") & "', 101)) AND (ijs.Inspection_Finished is null) ORDER BY ijs.id DESC"
            Try
                listis = bmapis.GetInspectObject(sql)
            Catch ex As Exception

            End Try

            Return listis
        End Function


        Public Function GetRollInspectionDetailTable(ByVal datefrom As DateTime, ByVal dateto As DateTime) As List(Of SPCInspection.RollInspectionDetailTable)
            Dim con As New SqlConnection(DL.InspectConnectionString)
            Dim cmd As SqlCommand = con.CreateCommand()
            Dim corereader As SqlDataReader
            Dim record As IDataRecord
            Dim readerlist As New List(Of SPCInspection.RollInspectionDetailTable)

            Try
                Using con
                    con.Open()
                    Using cmd
                        cmd = New SqlCommand("SP_SPC_GetGreigeReportDetail", con)
                        cmd.CommandType = CommandType.StoredProcedure
                        ''    'Add command parameters                                                                          
                        cmd.Parameters.Add("@DATESTART", SqlDbType.DateTime).Direction = ParameterDirection.Input
                        cmd.Parameters.Add("@DATEEND", SqlDbType.DateTime).Direction = ParameterDirection.Input


                        cmd.Parameters("@DATESTART").Value = datefrom
                        cmd.Parameters("@DATEEND").Value = dateto

                        Try

                            corereader = cmd.ExecuteReader(CommandBehavior.CloseConnection)

                            If corereader.FieldCount > 0 And corereader.HasRows = True Then
                                While corereader.Read
                                    record = CType(corereader, IDataRecord)
                                    readerlist.Add(New SPCInspection.RollInspectionDetailTable With {.ButtonId = Convert.ToInt32(record(0)), .Text = Convert.ToString(record(1)), .ShiftNumber = Convert.ToInt32(record(2)), .DHY = Convert.ToDecimal(record(3)), .DefectCount = Convert.ToInt32(record(4)), .RSID = Convert.ToInt32(record(5))})
                                End While

                                Return readerlist
                            Else
                                Return readerlist
                            End If
                        Catch e As Exception
                            Throw New System.Exception(e.Message)
                            Return Nothing
                            Exit Function
                        End Try
                        con.Close()
                    End Using
                End Using
            Catch ex As Exception
                Return Nothing
            End Try

            Return readerlist

        End Function

        Public Function GetDefectImageDisplay(ByVal fromdate As DateTime, ByVal todate As DateTime, Optional ByVal DefectId As Integer = Nothing) As List(Of SPCInspection.DefectImageDisplay_)
            Dim sqlstring As String
            Dim bmap As New BMappers(Of SPCInspection.DefectImageDisplay_)
            Dim retlist As New List(Of SPCInspection.DefectImageDisplay_)



            sqlstring = "select dm.DefectID as DefectID_, dm.DefectTime as DefectTime_, dm.Location as Location_, dm.DefectDesc as DefectDesc_, ijs.Inspection_Started as Inspection_Started_, ijs.JobNumber as JobNumber_, ijs.DataNo as DataNo_, ijs.PRP_Code as Prp_Code, dm.DefectImage as Image, UnitDesc as UnitDesc_, it.Name as AuditType from DefectMaster dm" & vbCrLf &
                            "INNER JOIN InspectionJobSummary ijs ON dm.InspectionJobSummaryId = ijs.id" & vbCrLf &
                            "INNER JOIN TemplateName tn ON ijs.TemplateId = tn.TemplateId" & vbCrLf &
                            "INNER JOIN InspectionTypes it ON rtrim(tn.LineType) = it.Abreviation" & vbCrLf &
                            "WHERE ijs.Inspection_Started >= cast('" + fromdate.ToShortDateString() + " " + fromdate.ToShortTimeString() + "' as datetime) and ijs.Inspection_Started <= cast('" + todate.ToShortDateString() + " " + todate.ToShortTimeString() + "' as datetime) and dm.DefectImage is not null "

            If DefectId > 0 Then
                sqlstring = sqlstring + " and dm.DefectID > " + DefectId.ToString()
            End If
            retlist = bmap.GetInspectObject(sqlstring)

            Return retlist

        End Function

        Public Function GetDefectImageDescList(ByVal fromdate As DateTime, ByVal todate As DateTime) As List(Of SPCInspection.DefectImageDisplay_)
            Dim sqlstring As String
            Dim bmap As New BMappers(Of SPCInspection.DefectImageDisplay_)
            Dim retlist As New List(Of SPCInspection.DefectImageDisplay_)



            sqlstring = " select dm.Location as Location_, dm.DefectDesc as DefectDesc_, ijs.JobNumber as WorkOrder_, ijs.DataNo as DataNo_, ijs.PRP_Code as Prp_Code, UnitDesc as UnitDesc_, it.Name as AuditType from DefectMaster dm" & vbCrLf &
                            "INNER JOIN InspectionJobSummary ijs ON dm.InspectionJobSummaryId = ijs.id" & vbCrLf &
                            "INNER JOIN TemplateName tn ON ijs.TemplateId = tn.TemplateId" & vbCrLf &
                            "INNER JOIN InspectionTypes it ON rtrim(tn.LineType) = it.Abreviation" & vbCrLf &
                            "WHERE ijs.Inspection_Started >= cast('" + fromdate.ToShortDateString() + " " + fromdate.ToShortTimeString() + "' as datetime) and ijs.Inspection_Started <= cast('" + todate.ToShortDateString() + " " + todate.ToShortTimeString() + "' as datetime) and dm.DefectImage is not null "

            retlist = bmap.GetInspectObject(sqlstring)

            Return retlist

        End Function

    End Class





End Namespace
