<%@ WebHandler Language="VB" Class="core.ut_flagboard_handler" %>

Imports System
Imports System.Web
Imports App.Utilities.Web.Handlers
Imports System.Web.Script.Serialization
Imports System.Data.SqlClient
Imports System.Data
Imports System.Globalization




Namespace core
    
    

    Public Class ut_flagboard_handler
        Inherits BaseHandler
    
        Public Property dl As New dlayer
        Public Property cd As New ChartData
        Public Property MSD As New MaintSchedDAO
        
        Dim MainSchedstring As String
        Dim MainSchedReader As SqlDataReader
        Dim MainSchedcmdBuilder As SqlCommandBuilder
        Dim MainSchedcmd As SqlCommand
        Dim MainSchedConnection As SqlConnection

        Public Function MainSched_Call(ByVal Schedule As String) As String


            Dim returnstring As String
            Dim MaintBoard As New List(Of MaintBoardnames)()
            Dim record As IDataRecord
            Dim jser As New JavaScriptSerializer


            MainSchedstring = "Select Machines.MM_Name, Maintenance_Types.MT_Description,Maintenance_Schedule.MS_Workorder, Maintenance_Schedule.MS_Main_Time_Required" & vbCrLf &
                                "From Maintenance_Schedule" & vbCrLf &
                                "Inner Join Maintenance_Types" & vbCrLf &
                                "ON Maintenance_Schedule.MT_Id = Maintenance_Types.MT_Id" & vbCrLf &
                                "Inner Join Machines" & vbCrLf &
                                "ON Maintenance_Schedule.MM_Id = Machines.MM_Id" & vbCrLf &
                                "where Maintenance_Schedule.MS_Workorder is not null and Maintenance_Schedule.MS_Maint_Code = " + Schedule

            Using con As New SqlConnection(dl.CtxConnectionString)

                Dim cmd As New SqlCommand(MainSchedstring, con)
                MainSchedReader = MainSchedcmd.ExecuteReader(CommandBehavior.CloseConnection)

                While MainSchedReader.Read
                    record = CType(MainSchedReader, IDataRecord)


                    MaintBoard.Add(New MaintBoardnames() With {.MMName = Convert.ToString(record(0)), .MT_Description = Convert.ToString(record(1)), .WordOrder = Convert.ToString(record(2)), .TimeReq = Convert.ToString(record(3))})
                End While

                Dim returnarray As MaintBoardnames() = MaintBoard.ToArray()

                returnstring = jser.Serialize(returnarray)


                Return returnstring

            End Using
        End Function
        
        Public Function GetMaintInputData(ByVal Schedule As String, ByVal MFBID As Integer) As String
            
            Dim sqlstring As String
            Dim returnstring As String
            Dim MaintBoard As New List(Of MaintBoard)()
            Dim jser As New JavaScriptSerializer
            Dim result As Boolean
            Dim DAO As New DAOFactory
            Dim array As Object
            Dim StartTime As String
            
            
            array = MSD.GetShiftnTime(DateTime.Now).ToArray()
            
            If IsNumeric(array(0).shiftNum) = True Then
                StartTime = Convert.ToString(array(0).StartHour)
            Else
                StartTime = Convert.ToString(DateTime.Now)
            End If
            
            If MFBID = 0 Then
                MFBID = 1
            End If
            
            sqlstring = "Select Maintenance_Schedule.MM_Id,  Maintenance_Schedule.MT_Id,  Maintenance_Schedule.MS_Next_Main_Date, Machines.MM_Number, Maintenance_Schedule.MS_Unscheduled_Reason,  Machines.MM_Name, Maintenance_Types.MT_Description,Maintenance_Schedule.MS_Workorder, Maintenance_Schedule.MS_Id, Maintenance_Schedule.MS_WOCreate_Timestamp, Maintenance_Schedule.MS_WOClosed_Timestamp, Maintenance_Schedule.MS_Notes  " & vbCrLf &
                                "From Maintenance_Schedule" & vbCrLf &
                                "Inner Join Maintenance_Types" & vbCrLf &
                                "ON Maintenance_Schedule.MT_Id = Maintenance_Types.MT_Id" & vbCrLf &
                                "Inner Join Machines" & vbCrLf &
                                "ON Maintenance_Schedule.MM_Id = Machines.MM_Id" & vbCrLf &
                                "where (Maintenance_Schedule.MS_Main_Comp_Date is null and Maintenance_Schedule.MS_Workorder is not null and Maintenance_Schedule.EMP_ID is not null and Maintenance_Schedule.MS_Maint_Code = " & Schedule & " and Maintenance_Schedule.MFB_Id = " & Convert.ToString(MFBID) & ") or (Maintenance_Schedule.MS_WOClosed_Timestamp > '" & StartTime & "' and Maintenance_Schedule.MS_Maint_Code = " & Schedule & ")" & vbCrLf &
                                "order by row_number() over(order by Maintenance_Schedule.MS_Workorder) asc;"

            MaintBoard = DAO.getMaintBoard(sqlstring)
            returnstring = jser.Serialize(MaintBoard.ToArray())
               
            Return returnstring
           

        End Function
        
        Public Function GetMobileInputData(ByVal Schedule As String, ByVal MFB_Id As Integer, ByVal sCID As String) As String
            
            Dim sqlstring As String
            Dim returnstring As String
            Dim MaintBoard As New List(Of MaintBoardMobile)()
            Dim bmapmb As New BMappers(Of MaintBoardMobile)
            Dim jser As New JavaScriptSerializer
            Dim result As Boolean
            Dim DAO As New DAOFactory
            Dim array As Object
            Dim StartTime As String

            array = MSD.GetShiftnTime(DateTime.Now).ToArray()
            
            If IsNumeric(array(0).shiftNum) = True Then
                StartTime = Convert.ToString(array(0).StartHour)
            Else
                StartTime = Convert.ToString(DateTime.Now)
            End If
            
            If MFB_Id = -1 Then
                sqlstring = "Select Maintenance_Types.MT_Description,Maintenance_Schedule.MFB_Id, Maintenance_Schedule.MS_Unscheduled_Reason,  Machines.MM_Name,Maintenance_Schedule.MS_Workorder, Maintenance_Schedule.MS_Id, Maintenance_Schedule.MS_WOCreate_Timestamp, Maintenance_Schedule.MS_WOClosed_Timestamp  " & vbCrLf &
                                "From Maintenance_Schedule" & vbCrLf &
                                "Inner Join Maintenance_Types" & vbCrLf &
                                "ON Maintenance_Schedule.MT_Id = Maintenance_Types.MT_Id" & vbCrLf &
                                "Inner Join Machines" & vbCrLf &
                                "ON Maintenance_Schedule.MM_Id = Machines.MM_Id" & vbCrLf &
                                "where (Maintenance_Schedule.MS_Main_Comp_Date is null and Maintenance_Schedule.MS_Workorder is not null and Maintenance_Schedule.EMP_ID is not null and Maintenance_Schedule.MS_Maint_Code = " & Schedule & ") or (Maintenance_Schedule.MS_WOClosed_Timestamp > '" & StartTime & "' and Maintenance_Schedule.MS_Maint_Code = " & Schedule & " )" & vbCrLf &
                                "order by row_number() over(order by Maintenance_Schedule.MS_Workorder) asc;"
            Else
                sqlstring = "Select Maintenance_Types.MT_Description, Maintenance_Schedule.MFB_Id, Maintenance_Schedule.MS_Unscheduled_Reason,  Machines.MM_Name,Maintenance_Schedule.MS_Workorder, Maintenance_Schedule.MS_Id, Maintenance_Schedule.MS_WOCreate_Timestamp, Maintenance_Schedule.MS_WOClosed_Timestamp  " & vbCrLf &
                                "From Maintenance_Schedule" & vbCrLf &
                                "Inner Join Maintenance_Types" & vbCrLf &
                                "ON Maintenance_Schedule.MT_Id = Maintenance_Types.MT_Id" & vbCrLf &
                                "Inner Join Machines" & vbCrLf &
                                "ON Maintenance_Schedule.MM_Id = Machines.MM_Id" & vbCrLf &
                                "where (Maintenance_Schedule.MS_Main_Comp_Date is null and Maintenance_Schedule.MS_Workorder is not null and Maintenance_Schedule.EMP_ID is not null and Maintenance_Schedule.MS_Maint_Code = " & Schedule & " and Maintenance_Schedule.MFB_Id = " & Convert.ToString(MFB_Id) & ") or (Maintenance_Schedule.MS_WOClosed_Timestamp > '" & StartTime & "' and Maintenance_Schedule.MS_Maint_Code = " & Schedule & "  AND Maintenance_Schedule.MFB_Id = " & Convert.ToString(MFB_Id) & ")" & vbCrLf &
                                "order by row_number() over(order by Maintenance_Schedule.MS_Workorder) asc;"
            End If
            
          
            'MaintBoard = DAO.getMaintBoardMobile(sqlstring)
            MaintBoard = bmapmb.GetCtxObject(sqlstring, sCID.ToString.Substring(3, 3))
            returnstring = jser.Serialize(MaintBoard.ToArray())
               
            Return returnstring
         
        End Function
    
        Public Function Hello() As String
            
            Dim returnstring As String = "Hello Test success"
            
            
            Return returnstring
            
            
        End Function
    
    
    End Class

End Namespace

