Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Data
Imports System.Globalization
Imports System.Web.Script.Serialization

Namespace core


    Public Class MaintSchedDAO
        Public Property _DAOFactory As New DAOFactory
        Public Property DL As New dlayer
        Public Property userDAO As New userDAO


        Public Shared returnint As Integer = 99
        Public Shared MFB_Id As Integer = 1

        Private _ErrorMsg As String
        Private Util As New Utilities

        Public Class ShiftTime

            Public Property shiftNum As Integer
            Public Property StartHour As String
            Public Property EndHour As String

        End Class



        Public Property ErrorMsg() As String              'bms 4/24/06
            Get                                     'bms 4/24/06
                Return _ErrorMsg                    'bms 4/24/06
            End Get                                 'bms 4/24/06
            Set(ByVal Value As String)                        'bms 4/24/06
                _ErrorMsg = Value                   'bms 4/24/06
            End Set                                 'bms 4/24/06
        End Property

        Public Function Update(ByVal MainFlagList As List(Of FlagBoard.MaintFlagMS), ByVal CID As Integer) As Integer

            Dim sqlstring As String
            Dim returnint As Integer

            Dim sqlcommand As SqlCommand

            Dim MainFlagArray() As FlagBoard.MaintFlagMS

            If Not (MainFlagList Is Nothing) Then
                MainFlagArray = MainFlagList.ToArray()
            Else
                Return False
                Exit Function
            End If



            sqlstring = "UPDATE Maintenance_Schedule SET MM_ID = @MM_ID, MT_ID = @MT_ID, MS_Next_Main_Date = @MS_Next_Main_Date, MS_Workorder = @MS_Workorder, MS_Frequency = @MS_Frequency, MS_Last_Main_Date = @MS_Last_Main_Date, MS_Main_Comp_Date = @MS_Main_Comp_Date, " & vbCrLf &
                "EMP_ID = @EMP_ID, MS_Maint_Code = @MS_Maint_Code, MS_Maint_Time_Alotted = @MS_Maint_Time_Alotted, MS_Main_Time_Required = @MS_Main_Time_Required, MS_Machine_Hours = @MS_Machine_Hours, MS_Unscheduled_Reason = @MS_Unscheduled_Reason, MS_Notes = @MS_Notes, " & vbCrLf &
                "MS_Total_Machine_Downtime = @MS_Total_Machine_Downtime, MS_Inventory_Decremented = @MS_Inventory_Decremented, MFB_Id = @MFB_Id, MS_WOClosed_Timestamp = @WOClosed_Timestamp " & vbCrLf &
                "WHERE MS_Id = @MS_Id"


            Using connection As New SqlConnection(DL.SetCtxConnectionString(CID))


                sqlcommand = _DAOFactory.GetCommand(sqlstring, connection)



                sqlcommand.Parameters.Add(_DAOFactory.Getparameter("@MM_ID", DbType.Int32))
                sqlcommand.Parameters.Add(_DAOFactory.Getparameter("@MT_ID", DbType.Int32))
                sqlcommand.Parameters.Add(_DAOFactory.Getparameter("@MS_Next_Main_Date", DbType.DateTime))
                sqlcommand.Parameters.Add(_DAOFactory.Getparameter("@MS_Workorder", DbType.String))
                sqlcommand.Parameters.Add(_DAOFactory.Getparameter("@MS_Frequency", DbType.Int32))
                sqlcommand.Parameters.Add(_DAOFactory.Getparameter("@MS_Last_Main_Date", DbType.DateTime))
                sqlcommand.Parameters.Add(_DAOFactory.Getparameter("@MS_Main_Comp_Date", DbType.DateTime))
                sqlcommand.Parameters.Add(_DAOFactory.Getparameter("@EMP_ID", DbType.Int32))
                sqlcommand.Parameters.Add(_DAOFactory.Getparameter("@MS_Maint_Code", DbType.Int16))
                sqlcommand.Parameters.Add(_DAOFactory.Getparameter("@MS_Maint_Time_Alotted", DbType.Int16))
                sqlcommand.Parameters.Add(_DAOFactory.Getparameter("@MS_Main_Time_Required", DbType.Int16))
                sqlcommand.Parameters.Add(_DAOFactory.Getparameter("@MS_Machine_Hours", DbType.Int32))
                sqlcommand.Parameters.Add(_DAOFactory.Getparameter("@MS_Unscheduled_Reason", DbType.String))
                sqlcommand.Parameters.Add(_DAOFactory.Getparameter("@MS_Notes", DbType.String))
                sqlcommand.Parameters.Add(_DAOFactory.Getparameter("@MS_Total_Machine_Downtime", DbType.Single))
                sqlcommand.Parameters.Add(_DAOFactory.Getparameter("@MS_Inventory_Decremented", DbType.Boolean))
                sqlcommand.Parameters.Add(_DAOFactory.Getparameter("@MS_Id", DbType.Int32))
                'sqlcommand.Parameters.Add(_DAOFactory.Getparameter("@Old_MT_ID", DbType.Int32))
                'sqlcommand.Parameters.Add(_DAOFactory.Getparameter("@Old_MS_Next_Main_Date", DbType.DateTime))
                sqlcommand.Parameters.Add(_DAOFactory.Getparameter("@MFB_Id", DbType.Int16))
                sqlcommand.Parameters.Add(_DAOFactory.Getparameter("@WOClosed_Timestamp", DbType.DateTime))
                ' ''Provide parameter values.
                sqlcommand.Parameters("@MM_ID").Value = MainFlagArray(0).MM_Id
                sqlcommand.Parameters("@MT_ID").Value = MainFlagArray(0).MT_Id
                sqlcommand.Parameters("@MFB_Id").Value = MainFlagArray(0).MFB_Id
                sqlcommand.Parameters("@MS_Next_Main_Date").Value = Convert.ToDateTime(MainFlagArray(0).MS_Next_Main_Date)
                If MainFlagArray(0).MS_Workorder = String.Empty Then
                    sqlcommand.Parameters("@MS_Workorder").Value = System.DBNull.Value
                Else
                    sqlcommand.Parameters("@MS_Workorder").Value = MainFlagArray(0).MS_Workorder
                End If
                sqlcommand.Parameters("@MS_Frequency").Value = MainFlagArray(0).MS_Frequency
                sqlcommand.Parameters("@MS_Last_Main_Date").Value = Convert.ToDateTime(MainFlagArray(0).MS_Last_Main_Date)
                If MainFlagArray(0).MS_Main_Comp_Date <> Nothing Then
                    sqlcommand.Parameters("@MS_Main_Comp_Date").Value = Convert.ToDateTime(MainFlagArray(0).MS_Main_Comp_Date)
                    sqlcommand.Parameters("@WOClosed_Timestamp").Value = Convert.ToDateTime(MainFlagArray(0).MS_WOClosed_Timestamp)

                Else
                    sqlcommand.Parameters("@MS_Main_Comp_Date").Value = System.DBNull.Value
                    sqlcommand.Parameters("@WOClosed_Timestamp").Value = System.DBNull.Value
                End If
                If MainFlagArray(0).EMP_ID <> 0 And MainFlagArray(0).EMP_ID <> Nothing Then
                    sqlcommand.Parameters("@EMP_ID").Value = MainFlagArray(0).EMP_ID
                Else
                    sqlcommand.Parameters("@EMP_ID").Value = System.DBNull.Value
                End If
                sqlcommand.Parameters("@MS_Maint_Code").Value = MainFlagArray(0).MS_Maint_Code
                sqlcommand.Parameters("@MS_Maint_Time_Alotted").Value = MainFlagArray(0).MS_Maint_Time_Alotted
                sqlcommand.Parameters("@MS_Main_Time_Required").Value = MainFlagArray(0).MS_Main_Time_Required
                sqlcommand.Parameters("@MS_Machine_Hours").Value = MainFlagArray(0).MS_Machine_Hours
                If MainFlagArray(0).MS_Unscheduled_Reason = String.Empty Then
                    sqlcommand.Parameters("@MS_Unscheduled_Reason").Value = System.DBNull.Value
                Else
                    sqlcommand.Parameters("@MS_Unscheduled_Reason").Value = MainFlagArray(0).MS_Unscheduled_Reason
                End If
                If MainFlagArray(0).MS_Notes = String.Empty Then
                    sqlcommand.Parameters("@MS_Notes").Value = System.DBNull.Value
                Else
                    sqlcommand.Parameters("@MS_Notes").Value = MainFlagArray(0).MS_Notes
                End If
                sqlcommand.Parameters("@MS_Total_Machine_Downtime").Value = MainFlagArray(0).MS_Total_Machine_Downtime
                sqlcommand.Parameters("@MS_Inventory_Decremented").Value = False
                sqlcommand.Parameters("@MS_Id").Value = MainFlagArray(0).MS_Id
                'sqlcommand.Parameters("@Old_MT_ID").Value = MainFlagArray(0).Old_MT_ID
                'sqlcommand.Parameters("@Old_MS_Next_Main_Date").Value = MainFlagArray(0).Old_MS_Next_Main_Date
                Try

                    sqlcommand.Connection.Open()
                    returnint = sqlcommand.ExecuteNonQuery()
                    userDAO.LogMaintenanceScheduleTrans(MainFlagArray(0).MFB_Id, MainFlagArray(0).EMP_ID, "Update", MainFlagArray(0).MS_Id, "Desktop")
                    Return returnint
                Catch e As Exception
                    ErrorMsg = e.Message
                    Return False
                    Exit Function
                End Try



            End Using

        End Function

        Public Function GetMachineImage(ByVal MMId As Integer, ByVal FileNumber As Integer, ByVal PassCID As Integer) As List(Of FlagBoard.ActiveMachineImage)
            Dim returnlist As New List(Of FlagBoard.ActiveMachineImage)
            Dim ImageReader As SqlDataReader
            Dim record As IDataRecord
            Dim sqlstring As String

            sqlstring = "SELECT Mach_Filename" & FileNumber.ToString() & ", Mach_Image" & FileNumber.ToString() & vbCrLf &
            "FROM Machines" & vbCrLf &
            "WHERE (MM_Id = @MMID)"

            Using con As New SqlConnection(DL.SetCtxConnectionString(PassCID))
                con.Open()
                Dim cmd As New SqlCommand(sqlstring, con)
                cmd.Parameters.Add(_DAOFactory.Getparameter("@MMID", DbType.Int32))
                cmd.Parameters("@MMID").Value = MMId

                ImageReader = cmd.ExecuteReader(CommandBehavior.CloseConnection)

                If ImageReader.FieldCount > 0 Then
                    While ImageReader.Read
                        record = CType(ImageReader, IDataRecord)

                        If IsDBNull(record(1)) = True Then
                            Return Nothing
                        End If
                        Select Case FileNumber
                            Case 1
                                returnlist.Add(New FlagBoard.ActiveMachineImage With {.MachineFileName1 = Convert.ToString(record(0)), .MachineFileBytes1 = record(1)})
                            Case 2
                                returnlist.Add(New FlagBoard.ActiveMachineImage With {.MachineFileName2 = Convert.ToString(record(0)), .MachineFileBytes2 = record(1)})
                            Case 3
                                returnlist.Add(New FlagBoard.ActiveMachineImage With {.MachineFileName3 = Convert.ToString(record(0)), .MachineFileBytes3 = record(1)})
                            Case 4
                                returnlist.Add(New FlagBoard.ActiveMachineImage With {.MachineFileName4 = Convert.ToString(record(0)), .MachineFileBytes4 = record(1)})
                        End Select

                    End While

                    Return returnlist
                Else
                    Return Nothing
                End If

            End Using


        End Function
        Public Function UpdateMobile(ByVal MainFlagList As List(Of FlagBoard.MaintFlagMSMobile), ByVal CID As Integer) As Integer

            Dim sqlstring As String
            Dim returnint As Integer

            Dim sqlcommand As SqlCommand

            Dim MainFlagArray() As FlagBoard.MaintFlagMSMobile

            If Not (MainFlagList Is Nothing) Then
                MainFlagArray = MainFlagList.ToArray()
            Else
                Return False
                Exit Function
            End If



            sqlstring = "UPDATE Maintenance_Schedule SET MM_ID = @MM_ID, MT_ID = @MT_ID, MS_Next_Main_Date = @MS_Next_Main_Date, MS_Frequency = @MS_Frequency, MS_Main_Comp_Date = @MS_Main_Comp_Date, " & vbCrLf &
                "EMP_ID = @EMP_ID, MS_Unscheduled_Reason = @MS_Unscheduled_Reason, MS_WOClosed_Timestamp = @WOClosed_Timestamp, MS_Notes = @MS_Notes, MFB_Id = @MFB_Id " & vbCrLf &
                "WHERE MS_Id = @MS_Id"


            Using connection As New SqlConnection(DL.SetCtxConnectionString(CID))


                sqlcommand = _DAOFactory.GetCommand(sqlstring, connection)

                sqlcommand.Parameters.Add(_DAOFactory.Getparameter("@MM_ID", DbType.Int32))
                sqlcommand.Parameters.Add(_DAOFactory.Getparameter("@MT_ID", DbType.Int32))
                sqlcommand.Parameters.Add(_DAOFactory.Getparameter("@MS_Next_Main_Date", DbType.DateTime))
                sqlcommand.Parameters.Add(_DAOFactory.Getparameter("@MS_Frequency", DbType.Int32))
                sqlcommand.Parameters.Add(_DAOFactory.Getparameter("@MS_Main_Comp_Date", DbType.DateTime))
                sqlcommand.Parameters.Add(_DAOFactory.Getparameter("@EMP_ID", DbType.Int32))
                sqlcommand.Parameters.Add(_DAOFactory.Getparameter("@MS_Unscheduled_Reason", DbType.String))
                sqlcommand.Parameters.Add(_DAOFactory.Getparameter("@MS_Id", DbType.Int32))
                sqlcommand.Parameters.Add(_DAOFactory.Getparameter("@WOClosed_Timestamp", DbType.DateTime))
                sqlcommand.Parameters.Add(_DAOFactory.Getparameter("@MS_Notes", DbType.String))
                sqlcommand.Parameters.Add(_DAOFactory.Getparameter("@MFB_Id", DbType.Int32))
                sqlcommand.Parameters("@MFB_Id").Value = MainFlagArray(0).MFB_Id
                'sqlcommand.Parameters.Add(_DAOFactory.Getparameter("@MFB_Id", DbType.Int16))
                ' ''Provide parameter values.
                sqlcommand.Parameters("@MM_ID").Value = MainFlagArray(0).MM_Id
                sqlcommand.Parameters("@MT_ID").Value = MainFlagArray(0).MT_Id
                'sqlcommand.Parameters("@MFB_Id").Value = MainFlagArray(0).MFB_Id
                sqlcommand.Parameters("@MS_Next_Main_Date").Value = MainFlagArray(0).MS_Next_Main_Date
                sqlcommand.Parameters("@MS_Frequency").Value = MainFlagArray(0).MS_Frequency
                If MainFlagArray(0).MS_Main_Comp_Date <> Nothing Then
                    sqlcommand.Parameters("@MS_Main_Comp_Date").Value = MainFlagArray(0).MS_Main_Comp_Date
                    sqlcommand.Parameters("@WOClosed_Timestamp").Value = DateTime.Now

                Else
                    sqlcommand.Parameters("@MS_Main_Comp_Date").Value = System.DBNull.Value
                    sqlcommand.Parameters("@WOClosed_Timestamp").Value = System.DBNull.Value
                End If
                If MainFlagArray(0).EMP_ID <> 0 And MainFlagArray(0).EMP_ID <> Nothing Then
                    sqlcommand.Parameters("@EMP_ID").Value = MainFlagArray(0).EMP_ID
                Else
                    sqlcommand.Parameters("@EMP_ID").Value = System.DBNull.Value
                End If

                If MainFlagArray(0).MS_Unscheduled_Reason = String.Empty Then
                    sqlcommand.Parameters("@MS_Unscheduled_Reason").Value = System.DBNull.Value
                Else
                    sqlcommand.Parameters("@MS_Unscheduled_Reason").Value = MainFlagArray(0).MS_Unscheduled_Reason
                End If
                If MainFlagArray(0).MS_Notes = String.Empty Then
                    sqlcommand.Parameters("@MS_Notes").Value = System.DBNull.Value
                Else
                    sqlcommand.Parameters("@MS_Notes").Value = MainFlagArray(0).MS_Notes
                End If
                sqlcommand.Parameters("@MS_Id").Value = MainFlagArray(0).MS_Id

                Try

                    sqlcommand.Connection.Open()
                    returnint = sqlcommand.ExecuteNonQuery()
                    userDAO.LogMaintenanceScheduleTrans(MainFlagArray(0).MFB_Id, MainFlagArray(0).EMP_ID, "Update", MainFlagArray(0).MS_Id, "Mobile")
                    Return returnint
                Catch e As Exception
                    ErrorMsg = e.Message
                    Return False
                    Exit Function
                End Try

            End Using

        End Function

        Public Function Delete(ByVal MS_ID As Integer, ByVal MFBID As Integer, ByVal EMPID As Integer, ByVal PassCID As Integer) As Boolean
            Dim SQL As String
            Dim sqlcommand As SqlCommand

            'SQL = "DELETE FROM Maintenance_Schedule WHERE MM_ID = " & MM_ID & " And MT_ID = " & MT_ID & " And MS_Next_Main_Date = @Start_Date "
            SQL = "DELETE FROM Maintenance_Schedule WHERE MS_Id = " & MS_ID



            Using connection As New SqlConnection(DL.SetCtxConnectionString(PassCID))


                sqlcommand = _DAOFactory.GetCommand(SQL, connection)
                sqlcommand = _DAOFactory.GetCommand(SQL.ToString(), connection)
                'Add command parameters             

                Try
                    sqlcommand.Connection.Open()
                    returnint = sqlcommand.ExecuteNonQuery()
                    userDAO.LogMaintenanceScheduleTrans(MFBID, EMPID, "Delete", MS_ID, "Either")
                    If returnint = 0 Then
                        Return False
                    End If

                Catch e As Exception
                    Return False
                End Try



            End Using
            Return True

        End Function


        Public Function Insert(ByVal MainFlagList As List(Of FlagBoard.MaintFlagMS), ByVal CIDnum As Integer) As Integer
            Dim SQL As String
            Dim sqlcommand As SqlCommand

            Dim MainFlagArray() As FlagBoard.MaintFlagMS

            If Not (MainFlagList Is Nothing) Then
                MainFlagArray = MainFlagList.ToArray()
            Else
                Return False
                Exit Function
            End If

            SQL = "INSERT INTO Maintenance_Schedule (MM_ID,MT_ID, MS_Next_Main_Date, MS_Workorder, MS_Frequency, MS_Last_Main_Date, MS_Main_Comp_Date, EMP_ID, MS_Maint_Code, MS_Maint_Time_Alotted, MS_Main_Time_Required, MS_Machine_Hours, MS_Unscheduled_Reason, MS_Notes, MS_Total_Machine_Downtime, MS_Inventory_Decremented, MFB_Id, MS_WOCreate_Timestamp, MS_WOClosed_Timestamp) VALUES(@MM_ID, @MT_ID, @MS_Next_Main_Date, @MS_Workorder, @MS_Frequency, @MS_Last_Main_Date, @MS_Main_Comp_Date, @EMP_ID, @MS_Maint_Code, @MS_Maint_Time_Alotted, @MS_Main_Time_Required, @MS_Machine_Hours, @MS_Unscheduled_Reason, @MS_Notes, @MS_Total_Machine_Downtime, @MS_Inventory_Decremented, @MFB_Id, @WOCreate_Timestamp,@WOClosed_Timestamp) "


            Using connection As New SqlConnection(DL.CtxConnectionString)


                sqlcommand = _DAOFactory.GetCommand(SQL, connection)
                ''    'Add command parameters                                                                          
                sqlcommand.Parameters.Add(_DAOFactory.Getparameter("@MM_ID", DbType.Int32))
                sqlcommand.Parameters.Add(_DAOFactory.Getparameter("@MT_ID", DbType.Int32))
                sqlcommand.Parameters.Add(_DAOFactory.Getparameter("@MS_Next_Main_Date", DbType.DateTime))
                sqlcommand.Parameters.Add(_DAOFactory.Getparameter("@MS_Workorder", DbType.String))
                sqlcommand.Parameters.Add(_DAOFactory.Getparameter("@MS_Frequency", DbType.Int16))
                sqlcommand.Parameters.Add(_DAOFactory.Getparameter("@MS_Last_Main_Date", DbType.DateTime))
                sqlcommand.Parameters.Add(_DAOFactory.Getparameter("@MS_Main_Comp_Date", DbType.DateTime))
                sqlcommand.Parameters.Add(_DAOFactory.Getparameter("@EMP_ID", DbType.Int32))
                sqlcommand.Parameters.Add(_DAOFactory.Getparameter("@MS_Maint_Code", DbType.Int16))
                sqlcommand.Parameters.Add(_DAOFactory.Getparameter("@MS_Maint_Time_Alotted", DbType.Int16))
                sqlcommand.Parameters.Add(_DAOFactory.Getparameter("@MS_Main_Time_Required", DbType.Int16))
                sqlcommand.Parameters.Add(_DAOFactory.Getparameter("@MS_Machine_Hours", DbType.Int32))
                sqlcommand.Parameters.Add(_DAOFactory.Getparameter("@MS_Unscheduled_Reason", DbType.String))
                sqlcommand.Parameters.Add(_DAOFactory.Getparameter("@MS_Notes", DbType.String))
                sqlcommand.Parameters.Add(_DAOFactory.Getparameter("@MS_Total_Machine_Downtime", DbType.Single))
                sqlcommand.Parameters.Add(_DAOFactory.Getparameter("@MS_Inventory_Decremented", DbType.Boolean))
                sqlcommand.Parameters.Add(_DAOFactory.Getparameter("@MFB_Id", DbType.Int16))
                sqlcommand.Parameters.Add(_DAOFactory.Getparameter("@WOCreate_Timestamp", DbType.DateTime))
                sqlcommand.Parameters.Add(_DAOFactory.Getparameter("@WOClosed_Timestamp", DbType.DateTime))

                ''    'Provide parameter values.                                                                    
                sqlcommand.Parameters("@MM_ID").Value = MainFlagArray(0).MM_Id
                sqlcommand.Parameters("@MT_ID").Value = MainFlagArray(0).MT_Id
                sqlcommand.Parameters("@MFB_Id").Value = MainFlagArray(0).MFB_Id
                sqlcommand.Parameters("@MS_Next_Main_Date").Value = MainFlagArray(0).MS_Next_Main_Date
                If MainFlagArray(0).MS_WOCreate_Timestamp <> Nothing Then
                    sqlcommand.Parameters("@WOCreate_Timestamp").Value = MainFlagArray(0).MS_WOCreate_Timestamp
                Else
                    sqlcommand.Parameters("@WOCreate_Timestamp").Value = DateTime.Now
                End If

                If MainFlagArray(0).MS_Main_Comp_Date <> Nothing Then
                    sqlcommand.Parameters("@WOClosed_Timestamp").Value = MainFlagArray(0).MS_WOClosed_Timestamp
                Else
                    sqlcommand.Parameters("@WOClosed_Timestamp").Value = System.DBNull.Value
                End If

                If MainFlagArray(0).MS_Workorder = String.Empty Then
                    sqlcommand.Parameters("@MS_Workorder").Value = System.DBNull.Value
                Else
                    Dim lastworkorder As Object = AutoGenerateWorkOrder(CIDnum)
                    If lastworkorder >= Convert.ToInt32(MainFlagArray(0).MS_Workorder) And (IsNumeric(lastworkorder) = True) Then
                        sqlcommand.Parameters("@MS_Workorder").Value = Convert.ToString(lastworkorder)
                    Else
                        sqlcommand.Parameters("@MS_Workorder").Value = MainFlagArray(0).MS_Workorder
                    End If

                End If
                sqlcommand.Parameters("@MS_Frequency").Value = MainFlagArray(0).MS_Frequency
                If MainFlagArray(0).MS_Last_Main_Date <> Nothing Then
                    sqlcommand.Parameters("@MS_Last_Main_Date").Value = MainFlagArray(0).MS_Last_Main_Date
                Else
                    sqlcommand.Parameters("@MS_Last_Main_Date").Value = DateTime.Now
                End If
                If MainFlagArray(0).MS_Main_Comp_Date <> Nothing Then
                    sqlcommand.Parameters("@MS_Main_Comp_Date").Value = MainFlagArray(0).MS_Main_Comp_Date
                Else
                    sqlcommand.Parameters("@MS_Main_Comp_Date").Value = System.DBNull.Value
                End If
                If MainFlagArray(0).EMP_ID <> 0 And MainFlagArray(0).EMP_ID <> Nothing Then
                    sqlcommand.Parameters("@EMP_ID").Value = MainFlagArray(0).EMP_ID
                Else
                    sqlcommand.Parameters("@EMP_ID").Value = System.DBNull.Value
                End If
                sqlcommand.Parameters("@MS_Maint_Code").Value = MainFlagArray(0).MS_Maint_Code
                sqlcommand.Parameters("@MS_Maint_Time_Alotted").Value = MainFlagArray(0).MS_Maint_Time_Alotted
                sqlcommand.Parameters("@MS_Main_Time_Required").Value = MainFlagArray(0).MS_Main_Time_Required
                sqlcommand.Parameters("@MS_Machine_Hours").Value = MainFlagArray(0).MS_Machine_Hours
                If MainFlagArray(0).MS_Unscheduled_Reason = String.Empty Then
                    sqlcommand.Parameters("@MS_Unscheduled_Reason").Value = System.DBNull.Value
                Else
                    sqlcommand.Parameters("@MS_Unscheduled_Reason").Value = MainFlagArray(0).MS_Unscheduled_Reason
                End If
                If MainFlagArray(0).MS_Notes = String.Empty Then
                    sqlcommand.Parameters("@MS_Notes").Value = System.DBNull.Value
                Else
                    sqlcommand.Parameters("@MS_Notes").Value = MainFlagArray(0).MS_Notes
                End If
                sqlcommand.Parameters("@MS_Total_Machine_Downtime").Value = MainFlagArray(0).MS_Total_Machine_Downtime
                sqlcommand.Parameters("@MS_Inventory_Decremented").Value = 0
                'bms 4/27/06
                Try

                    sqlcommand.Connection.Open()
                    returnint = sqlcommand.ExecuteNonQuery()
                    userDAO.LogMaintenanceScheduleTrans(MainFlagArray(0).MFB_Id, MainFlagArray(0).EMP_ID, "Insert", MainFlagArray(0).MS_Id, "Desktop")
                Catch e As Exception
                    ErrorMsg = e.Message
                    Return 0
                    Exit Function
                End Try


            End Using
            Return returnint


        End Function

        Public Function InsertMobile(ByVal MainFlagList As List(Of FlagBoard.MaintFlagMSMobile), ByVal CID As Integer) As Integer
            Dim SQL As String
            Dim sqlcommand As SqlCommand

            Dim MainFlagArray() As FlagBoard.MaintFlagMSMobile

            If Not (MainFlagList Is Nothing) Then
                MainFlagArray = MainFlagList.ToArray()
            Else
                Return False
                Exit Function
            End If


            SQL = "INSERT INTO Maintenance_Schedule (MM_ID,MT_ID, MS_Next_Main_Date, MS_Workorder, MS_Frequency, MS_Last_Main_Date, MS_Main_Comp_Date, EMP_ID, MS_Maint_Code, MS_Maint_Time_Alotted, MS_Main_Time_Required, MS_Machine_Hours, MS_Unscheduled_Reason, MS_Notes, MS_Total_Machine_Downtime, MS_Inventory_Decremented, MFB_Id, MS_WOCreate_Timestamp,  MS_WOClosed_Timestamp) VALUES(@MM_ID, @MT_ID, @MS_Next_Main_Date, @MS_Workorder, @MS_Frequency, @MS_Last_Main_Date, @MS_Main_Comp_Date, @EMP_ID, @MS_Maint_Code, @MS_Maint_Time_Alotted, @MS_Main_Time_Required, @MS_Machine_Hours, @MS_Unscheduled_Reason, @MS_Notes, @MS_Total_Machine_Downtime, @MS_Inventory_Decremented, @MFB_Id, @WOCreate_Timestamp, @WOClosed_Timestamp) "


            Using connection As New SqlConnection(DL.SetCtxConnectionString(CID))


                sqlcommand = _DAOFactory.GetCommand(SQL, connection)
                ''    'Add command parameters                                                                          
                sqlcommand.Parameters.Add(_DAOFactory.Getparameter("@MM_ID", DbType.Int32))
                sqlcommand.Parameters.Add(_DAOFactory.Getparameter("@MT_ID", DbType.Int32))
                sqlcommand.Parameters.Add(_DAOFactory.Getparameter("@MS_Next_Main_Date", DbType.DateTime))
                sqlcommand.Parameters.Add(_DAOFactory.Getparameter("@MS_Main_Comp_Date", DbType.DateTime))
                sqlcommand.Parameters.Add(_DAOFactory.Getparameter("@MS_Workorder", DbType.String))
                sqlcommand.Parameters.Add(_DAOFactory.Getparameter("@MS_Frequency", DbType.Int16))
                sqlcommand.Parameters.Add(_DAOFactory.Getparameter("@MS_Last_Main_Date", DbType.DateTime))
                sqlcommand.Parameters.Add(_DAOFactory.Getparameter("@EMP_ID", DbType.Int32))
                sqlcommand.Parameters.Add(_DAOFactory.Getparameter("@MS_Maint_Code", DbType.Int16))
                sqlcommand.Parameters.Add(_DAOFactory.Getparameter("@MS_Maint_Time_Alotted", DbType.Int16))
                sqlcommand.Parameters.Add(_DAOFactory.Getparameter("@MS_Main_Time_Required", DbType.Int16))
                sqlcommand.Parameters.Add(_DAOFactory.Getparameter("@MS_Machine_Hours", DbType.Int32))
                sqlcommand.Parameters.Add(_DAOFactory.Getparameter("@MS_Unscheduled_Reason", DbType.String))
                sqlcommand.Parameters.Add(_DAOFactory.Getparameter("@MS_Notes", DbType.String))
                sqlcommand.Parameters.Add(_DAOFactory.Getparameter("@MS_Total_Machine_Downtime", DbType.Single))
                sqlcommand.Parameters.Add(_DAOFactory.Getparameter("@MS_Inventory_Decremented", DbType.Boolean))
                sqlcommand.Parameters.Add(_DAOFactory.Getparameter("@MFB_Id", DbType.Int16))
                sqlcommand.Parameters.Add(_DAOFactory.Getparameter("@WOCreate_Timestamp", DbType.DateTime))
                sqlcommand.Parameters.Add(_DAOFactory.Getparameter("@WOClosed_Timestamp", DbType.DateTime))

                ''    'Provide parameter values.                                                                    
                sqlcommand.Parameters("@MM_ID").Value = MainFlagArray(0).MM_Id
                sqlcommand.Parameters("@MT_ID").Value = MainFlagArray(0).MT_Id
                sqlcommand.Parameters("@MFB_Id").Value = MainFlagArray(0).MFB_Id

                If MainFlagArray(0).MS_Next_Main_Date <> Nothing Then
                    sqlcommand.Parameters("@MS_Next_Main_Date").Value = MainFlagArray(0).MS_Next_Main_Date
                Else
                    sqlcommand.Parameters("@MS_Next_Main_Date").Value = DateTime.Now
                End If

                If MainFlagArray(0).MS_WOCreate_Timestamp <> Nothing Then
                    sqlcommand.Parameters("@WOCreate_Timestamp").Value = MainFlagArray(0).MS_WOCreate_Timestamp
                Else
                    sqlcommand.Parameters("@WOCreate_Timestamp").Value = DateTime.Now
                End If
                If MainFlagArray(0).MS_Main_Comp_Date <> Nothing Then
                    sqlcommand.Parameters("@WOClosed_Timestamp").Value = MainFlagArray(0).MS_Main_Comp_Date
                Else
                    sqlcommand.Parameters("@WOClosed_Timestamp").Value = System.DBNull.Value
                End If
                If MainFlagArray(0).MS_Workorder = String.Empty Then
                    sqlcommand.Parameters("@MS_Workorder").Value = System.DBNull.Value
                Else
                    Dim lastworkorder As Object = AutoGenerateWorkOrder(578)
                    If lastworkorder >= Convert.ToInt32(MainFlagArray(0).MS_Workorder) And (IsNumeric(lastworkorder) = True) Then
                        sqlcommand.Parameters("@MS_Workorder").Value = Convert.ToString(lastworkorder)
                    Else
                        sqlcommand.Parameters("@MS_Workorder").Value = MainFlagArray(0).MS_Workorder
                    End If

                End If
                sqlcommand.Parameters("@MS_Frequency").Value = MainFlagArray(0).MS_Frequency

                sqlcommand.Parameters("@MS_Last_Main_Date").Value = Date.Now

                If MainFlagArray(0).MS_Main_Comp_Date <> Nothing Then
                    sqlcommand.Parameters("@MS_Main_Comp_Date").Value = MainFlagArray(0).MS_Main_Comp_Date
                Else
                    sqlcommand.Parameters("@MS_Main_Comp_Date").Value = System.DBNull.Value
                End If
                If MainFlagArray(0).EMP_ID <> 0 And MainFlagArray(0).EMP_ID <> Nothing Then
                    sqlcommand.Parameters("@EMP_ID").Value = MainFlagArray(0).EMP_ID
                Else
                    sqlcommand.Parameters("@EMP_ID").Value = System.DBNull.Value
                End If
                sqlcommand.Parameters("@MS_Maint_Code").Value = MainFlagArray(0).MS_Maint_Code
                sqlcommand.Parameters("@MS_Maint_Time_Alotted").Value = 0
                sqlcommand.Parameters("@MS_Main_Time_Required").Value = 0
                sqlcommand.Parameters("@MS_Machine_Hours").Value = 0
                If MainFlagArray(0).MS_Unscheduled_Reason = String.Empty Then
                    sqlcommand.Parameters("@MS_Unscheduled_Reason").Value = System.DBNull.Value
                Else
                    sqlcommand.Parameters("@MS_Unscheduled_Reason").Value = MainFlagArray(0).MS_Unscheduled_Reason
                End If
                If MainFlagArray(0).MS_Notes = String.Empty Then
                    sqlcommand.Parameters("@MS_Notes").Value = System.DBNull.Value
                Else
                    sqlcommand.Parameters("@MS_Notes").Value = MainFlagArray(0).MS_Notes
                End If

                sqlcommand.Parameters("@MS_Total_Machine_Downtime").Value = 0
                sqlcommand.Parameters("@MS_Inventory_Decremented").Value = 0
                'bms 4/27/06
                Try

                    sqlcommand.Connection.Open()
                    returnint = sqlcommand.ExecuteNonQuery()
                    userDAO.LogMaintenanceScheduleTrans(MainFlagArray(0).MFB_Id, MainFlagArray(0).EMP_ID, "Insert", MainFlagArray(0).MS_Id, "Mobile")
                Catch e As Exception
                    ErrorMsg = e.Message
                    Return 0
                    Exit Function
                End Try


            End Using
            Return returnint


        End Function

        Public Function AutoGenerateWorkOrder(Optional ByVal CID As Integer = 578) As Object

            Dim Sql As StringBuilder = New StringBuilder
            Dim rsOrderNum = New DataSet
            Sql.Append("SELECT TOP 1 MS_Workorder FROM Maintenance_Schedule ")
            Sql.Append("where isnumeric(ms_workorder) = 1 ")
            ' Sql.Append("Order by MS_Workorder Desc ")                                 
            Sql.Append("ORDER BY CONVERT(int,MS_Workorder) DESC ")

            If Not Util.FillDataSet(rsOrderNum, "rsOrderNum", Sql.ToString(), CID) Then
                Return 10001
            End If

            If rsOrderNum.Tables(0).Rows.Count > 0 Then
                If IsNumeric(rsOrderNum.Tables(0).Rows(0)("MS_Workorder")) Then
                    Return rsOrderNum.Tables(0).Rows(0)("MS_Workorder") + 1
                End If
                Return 10001
            Else
                Return 10001
            End If

        End Function

        Public Function GetShiftnTime(ByVal InputDate As DateTime) As List(Of ShiftTime)

            Dim ShiftList As New List(Of ShiftTime)()
            Dim Today As DateTime = DateTime.Today
            Dim DateShift As String

            DateShift = Today.ToString("d")

            Select Case InputDate

                Case Today.AddHours(8) To Today.AddHours(16)
                    ShiftList.Add(New ShiftTime With {.shiftNum = 1, .StartHour = Convert.ToString(Today.AddHours(8)), .EndHour = Convert.ToString(Today.AddHours(16))})
                Case Today.AddHours(16) To Today.AddHours(24)
                    ShiftList.Add(New ShiftTime With {.shiftNum = 2, .StartHour = Convert.ToString(Today.AddHours(16)), .EndHour = Convert.ToString(Today.AddHours(24))})
                Case Today To Today.AddHours(8)
                    ShiftList.Add(New ShiftTime With {.shiftNum = 2, .StartHour = Convert.ToString(Today), .EndHour = Convert.ToString(Today.AddHours(8))})

            End Select

            Return ShiftList

        End Function

        Public Function GetBlankEMPID() As Integer
            Dim sqlstring As String
            Dim EMPNameSet As DataSet = New DataSet
            sqlstring = "select EMP_ID from dbo.Employees where EMP_Last_Name = '.' and EMP_First_Name = '.'"

            If Not Util.FillDataSet(EMPNameSet, "EMPNameSet", sqlstring) Then
                Return False
            End If

            Return EMPNameSet.Tables(0).Rows(0)("EMP_ID")

        End Function

        Public Function GetMobileInputData(ByVal Schedule As String, ByVal MFB_Id As Integer) As String

            Dim sqlstring As String
            Dim returnstring As String
            Dim MaintBoard As New List(Of MaintBoardMobile)()
            Dim jser As New JavaScriptSerializer
            Dim result As Boolean
            Dim DAO As New DAOFactory
            Dim array As Object
            Dim StartTime As String

            array = GetShiftnTime(DateTime.Now).ToArray()

            If IsNumeric(array(0).shiftNum) = True Then
                StartTime = Convert.ToString(array(0).StartHour)
            Else
                StartTime = Convert.ToString(DateTime.Now)
            End If

            sqlstring = "Select Maintenance_Types.MT_Description, Maintenance_Schedule.MS_Unscheduled_Reason,  Machines.MM_Name,Maintenance_Schedule.MS_Workorder, Maintenance_Schedule.MS_Id, Maintenance_Schedule.MS_WOCreate_Timestamp, Maintenance_Schedule.MS_WOClosed_Timestamp  " & vbCrLf &
                                "From Maintenance_Schedule" & vbCrLf &
                                "Inner Join Maintenance_Types" & vbCrLf &
                                "ON Maintenance_Schedule.MT_Id = Maintenance_Types.MT_Id" & vbCrLf &
                                "Inner Join Machines" & vbCrLf &
                                "ON Maintenance_Schedule.MM_Id = Machines.MM_Id" & vbCrLf &
                                "where (Maintenance_Schedule.MS_Main_Comp_Date is null and Maintenance_Schedule.MS_Workorder is not null and Maintenance_Schedule.EMP_ID is not null and Maintenance_Schedule.MS_Maint_Code = " & Schedule & " and Maintenance_Schedule.MFB_Id = " & Convert.ToString(MFB_Id) & ") or (Maintenance_Schedule.MS_WOClosed_Timestamp > '" & StartTime & "' and Maintenance_Schedule.MS_Maint_Code = " & Schedule & "  AND Maintenance_Schedule.MFB_Id = " & Convert.ToString(MFB_Id) & ")" & vbCrLf &
                                "order by row_number() over(order by Maintenance_Schedule.MS_Workorder) asc;"

            MaintBoard = DAO.getMaintBoardMobile(sqlstring)
            returnstring = jser.Serialize(MaintBoard.ToArray())

            Return returnstring

        End Function

    End Class


End Namespace

