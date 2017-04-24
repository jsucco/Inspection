<%@ WebHandler Language="VB" Class="core.SPC_Report_Handler" %>

Imports System
Imports System.Web
Imports App.Utilities.Web.Handlers
Imports System.Web.Script.Serialization
Imports System.Data.SqlClient
Imports System.Data
Imports System.Globalization
Imports MySql.Data.MySqlClient




Namespace core

    Public Class SPC_Report_Handler
        Inherits BaseHandler
    
        Public Property dl As New dlayer
        Public Property cd As New ChartData
        Public Property da As New DataAccess
       
        
        Dim JobsummString1 As String
        Dim JobsummConnection As MySqlConnection
        Dim JobsummReader As MySqlDataReader
        Dim JobsummNewRow As DataRow
        Dim JobsummcmdBuilder As MySqlCommandBuilder
        Dim Jobsummcmd As MySqlCommand
        Dim jser As New JavaScriptSerializer
        
        Public Function GetReportData(ByVal date1 As String, ByVal date2 As String, ByVal field As String) As String
            
            Dim returnstring As String = ""
            Dim fieldint As Integer = Convert.ToInt32(field)
            Dim Fromdf As String
            Dim Todf As String 
            Dim record As IDataRecord
            Dim reclength As Integer
            Dim ExportTable As New List(Of SPCExporter)()
            
            Fromdf = parsedate(date1)
            Todf = parsedate(date2)
            
            da.pfromdate = Fromdf
            da.ptodate = Todf
            
            
            Try

                JobsummString1 = getquerystring(fieldint, Fromdf, Todf)
                JobsummConnection = New MySqlConnection("server=STT-SERVER1-PC;port=3310;user id=akab1;password=hebron;database=Production")
                JobsummConnection.Open()
                Jobsummcmd = New MySqlCommand(JobsummString1, JobsummConnection)
                JobsummReader = Jobsummcmd.ExecuteReader(CommandBehavior.CloseConnection)

            Catch ex As Exception
                MsgBox(ex.Message)
            End Try

            While JobsummReader.Read
                record = CType(JobsummReader, IDataRecord)
                reclength = record.FieldCount

                Try
                   
                    ExportTable.Add(New SPCExporter() With {.idJobSummary = Convert.ToString(record(0)), .WorkOrder = Convert.ToString(record(1)), .DataNo = Convert.ToString(record(2)), .Description = Convert.ToString(record(3)), .TotalSewn = Convert.ToString(record(4)), .TotalDefects = Convert.ToString(record(5)), .TotalYds = Convert.ToString(record(6)), .FinishTime = Convert.ToString(record(7)), .RunTime = Convert.ToString(record(8)), .DownTime = Convert.ToString(record(9))})
                    
                Catch ex As Exception
                    MsgBox(ex.Message)
                End Try

            End While

            JobsummConnection.Close()
            JobsummConnection.Dispose()
            
            
            Dim returnarray As SPCExporter() = ExportTable.ToArray()

            returnstring = jser.Serialize(returnarray)
            
            Return returnstring
            
        End Function
        
        Private Function getquerystring(ByVal field As Integer, ByVal fromdate As String, ByVal todate As String) As String
            
            Dim querystring As String
            
            Select Case field
                
                Case 0
                    querystring = "select idJobSummary, WorkOrder,DataNo,Description,Machine, TotalSewn, TotalDefects, TotalYds, FinishTime, RunTime, DownTime from jobsummary where FinishTime >= '" & fromdate & "' and FinishTime <= '" & todate & "' and (Machine = 'STT_TEXPA1' OR Machine = 'STT_TEXPA2')"
                Case 1
                    querystring = "select idJobSummary, WorkOrder,DataNo,Description,Machine, TotalSewn, TotalDefects, TotalYds, FinishTime, RunTime, DownTime from jobsummary where FinishTime >= '" & fromdate & "' and FinishTime <= '" & todate & "' and Machine = 'STT_TEXPA3'"
                Case 2
                    querystring = "select idJobSummary, WorkOrder,DataNo,Description,Machine, TotalSewn, TotalDefects, TotalYds, FinishTime, RunTime, DownTime from jobsummary where FinishTime >= '" & fromdate & "' and FinishTime <= '" & todate & "' and (Machine = 'STT_PILLOW1' OR Machine = 'STT_PILLOW2')"
                Case Else
                    querystring = "No string"
  
            End Select
            
            
            Return querystring
            
        End Function
        
        Private Function parsedate(ByVal datestring As String) As String
            
            Dim returnstring As String
            Dim datesplit() As String
            
            If IsDate(datestring) = True Then
                datesplit = Split(datestring, "/", -1, CompareMethod.Text)
                returnstring = datesplit(2) + "-" + datesplit(0) + "-" + datesplit(1) + " 00:00:00"
            Else
                returnstring = "nostring error"
            End If
            
         
            
            
            
            
            
            Return returnstring
            
        End Function
        

    End Class
    
    
End Namespace
