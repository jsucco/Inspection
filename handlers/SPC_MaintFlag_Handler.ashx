<%@ WebHandler Language="VB" Class="core.SPC_MaintFlag_Handler" %>

Imports System
Imports System.Web
Imports App.Utilities.Web.Handlers
Imports System.Web.Script.Serialization
Imports System.Data.SqlClient
Imports System.Data
Imports System.Globalization

Namespace core

    Public Class SPC_MaintFlag_Handler
        Inherits BaseHandler

        Private MSD As New MaintSchedDAO
        Private Util As New Utilities
        Private DAO As New DAOFactory

        Dim ErrorMessage As String
        Dim ErrorFlag As Boolean

        Public Function GetMaintInputData(ByVal id As Integer, ByVal schedhold As String, ByVal MFB_Id As Integer) As String

            Dim sqlstring As String
            Dim returnstring As String
            Dim MaintFlag As New List(Of MaintFlagMS)()
            Dim jser As New JavaScriptSerializer

            sqlstring = "with T1 as (" & vbCrLf &
                                "SELECT s.MM_Id, m.MM_Name,t.MT_Name, s.MS_Maint_Time_Alotted, s.MS_Main_Time_Required, s.MS_Frequency, s.MS_Last_Main_Date, s.MS_Next_Main_Date, s.MS_Maint_Code, s.MS_Workorder, s.MS_Main_Comp_Date, s.EMP_ID, s.MS_Total_Machine_Downtime, s.MS_Machine_Hours, s.MS_Unscheduled_Reason, s.MS_Notes, row_number() over(order by s.MS_Workorder) RowNumber, s.MS_Id,e.EMP_First_Name, m.MM_Number" & vbCrLf &
                                "FROM dbo.Maintenance_Schedule s" & vbCrLf &
                                "INNER JOIN dbo.Maintenance_Types t" & vbCrLf &
                                "ON s.MT_Id= t.MT_Id" & vbCrLf &
                                "INNER JOIN dbo.Machines m" & vbCrLf &
                                "ON s.MM_Id= m.MM_Id" & vbCrLf &
                                "INNER JOIN dbo.Employees e" & vbCrLf &
                                "ON s.EMP_ID= e.EMP_ID" & vbCrLf &
                                "where s.MS_Main_Comp_Date is null and s.MS_Workorder is not null and MS_Maint_Code=" & schedhold & " and s.MFB_Id = " & Convert.ToString(MFB_Id) & ")" & vbCrLf &
                                "select * from T1 where RowNumber = " & Convert.ToString(id) & ";"
            MaintFlag = DAO.getMaintFlag(sqlstring, 578)
            returnstring = jser.Serialize(MaintFlag.ToArray())

            Return returnstring

        End Function

        'Public Function GetMaintInputBox(ByVal MMID As Integer, ByVal MTID As Integer, ByVal NextDate As String, ByVal schedhold As String) As String
        Public Function GetMaintInputBox(ByVal MSID As Integer, ByVal schedhold As String, ByVal MFB_Id As Integer, ByVal PassCID As Integer) As String
            Dim sqlstring As String
            Dim returnstring As String
            Dim MaintFlag As New List(Of MaintFlagMS2)()
            Dim jser As New JavaScriptSerializer
            Dim result As Boolean
            Dim bmapmm As New BMappers(Of MaintFlagMS2)


            'sqlstring = "SELECT s.MM_Id, m.MM_Name,t.MT_Name, s.MS_Maint_Time_Alotted, s.MS_Main_Time_Required, s.MS_Frequency, s.MS_Last_Main_Date, s.MS_Next_Main_Date, s.MS_Maint_Code, s.MS_Workorder, s.MS_Main_Comp_Date, s.EMP_ID, s.MS_Total_Machine_Downtime, s.MS_Machine_Hours, s.MS_Unscheduled_Reason, s.MS_Notes, row_number() over(order by s.MS_Workorder) RowNumber, t.MT_Id,e.EMP_First_Name, m.MM_Number" & vbCrLf &
            '                    "FROM dbo.Maintenance_Schedule s" & vbCrLf &
            '                    "INNER JOIN dbo.Maintenance_Types t" & vbCrLf &
            '                    "ON s.MT_Id= t.MT_Id" & vbCrLf &
            '                    "INNER JOIN dbo.Machines m" & vbCrLf &
            '                    "ON s.MM_Id= m.MM_Id" & vbCrLf &
            '                    "INNER JOIN dbo.Employees e" & vbCrLf &
            '                    "ON s.EMP_ID= e.EMP_ID" & vbCrLf &
            '                    "where s.MS_Main_Comp_Date is null and s.MS_Workorder is not null and MS_Maint_Code=" & schedhold & " and s.MFB_Id = " & Convert.ToString(MSD.MFB_Id) & " and s.MM_Id = " & Convert.ToString(MMID) & " and s.MT_Id = " & Convert.ToString(MTID) & " and s.MS_Next_Main_Date =  '" & NextDate & "'"
            sqlstring = "SELECT s.MM_Id, m.MM_Name,t.MT_Name, s.MS_Maint_Time_Alotted, s.MS_Main_Time_Required, s.MS_Frequency, convert(varchar, s.MS_Last_Main_Date, 1) as MS_Last_Main_Date, convert(varchar, s.MS_Next_Main_Date, 1) as MS_Next_Main_Date , s.MS_Maint_Code, s.MS_Workorder, Isnull(convert(varchar,  s.MS_Main_Comp_Date, 1) , '') as MS_Main_Comp_Date, s.EMP_ID, s.MS_Total_Machine_Downtime, s.MS_Machine_Hours, s.MS_Unscheduled_Reason, s.MS_Notes, row_number() over(order by s.MS_Workorder) RowNumber, t.MT_Id,e.EMP_First_Name, m.MM_Number, s.MFB_Id" & vbCrLf &
                                "FROM dbo.Maintenance_Schedule s" & vbCrLf &
                                "INNER JOIN dbo.Maintenance_Types t" & vbCrLf &
                                "ON s.MT_Id= t.MT_Id" & vbCrLf &
                                "INNER JOIN dbo.Machines m" & vbCrLf &
                                "ON s.MM_Id= m.MM_Id" & vbCrLf &
                                "INNER JOIN dbo.Employees e" & vbCrLf &
                                "ON s.EMP_ID= e.EMP_ID" & vbCrLf &
                                "where s.MS_Workorder is not null and MS_Maint_Code=" & schedhold & " and s.MS_Id = " & Convert.ToString(MSID)

            'MaintFlag = DAO.getMaintFlag(sqlstring, PassCID)
            MaintFlag = bmapmm.GetCtxObject(sqlstring, PassCID)
            returnstring = jser.Serialize(MaintFlag.ToArray())

            Return returnstring

        End Function

        Public Function GetPCGridData(ByVal Schedule As String, ByVal completed As Boolean, ByVal MFB_Id As Integer, ByVal PassCID As String) As String

            Dim sqlstring As String
            Dim returnstring As String
            Dim MaintBoard As New List(Of FlagBoard.MaintBoardPC)()
            Dim jser As New JavaScriptSerializer
            Dim DAO As New DAOFactory
            Dim array As Object
            Dim StartTime As String
            Dim MFBID As Integer = 1
            Dim CIDnum As Integer = CType(PassCID, Integer)
            array = MSD.GetShiftnTime(DateTime.Now).ToArray()

            If IsNumeric(array(0).shiftNum) = True Then
                StartTime = Convert.ToString(array(0).StartHour)
            Else
                StartTime = Convert.ToString(DateTime.Now)
            End If
            If completed = True Then
                If MFB_Id = -1 Then
                    sqlstring = "Select Maintenance_Types.MT_Description,Maintenance_Schedule.MT_Id, Maintenance_Schedule.MS_Unscheduled_Reason,  Machines.MM_Name,Maintenance_Schedule.MS_Workorder, Maintenance_Schedule.MS_Id, Maintenance_Schedule.MFB_Id, Maintenance_Schedule.MS_WOCreate_Timestamp, Maintenance_Schedule.MS_WOClosed_Timestamp, Machines.MM_Number, Maintenance_Schedule.MS_Notes   " & vbCrLf &
                                        "From Maintenance_Schedule" & vbCrLf &
                                        "Inner Join Maintenance_Types" & vbCrLf &
                                        "ON Maintenance_Schedule.MT_Id = Maintenance_Types.MT_Id" & vbCrLf &
                                        "Inner Join Machines" & vbCrLf &
                                        "ON Maintenance_Schedule.MM_Id = Machines.MM_Id" & vbCrLf &
                                        "where (Maintenance_Schedule.MS_Main_Comp_Date is null and Maintenance_Schedule.MS_Workorder is not null and Maintenance_Schedule.EMP_ID is not null and Maintenance_Schedule.MS_Maint_Code = " & Schedule & ") or (Maintenance_Schedule.MS_WOClosed_Timestamp > '" & StartTime & "' and Maintenance_Schedule.MS_Maint_Code = " & Schedule & ")" & vbCrLf &
                                        "order by row_number() over(order by Maintenance_Schedule.MS_Workorder) asc;"
                Else
                    sqlstring = "Select Maintenance_Types.MT_Description,Maintenance_Schedule.MT_Id, Maintenance_Schedule.MS_Unscheduled_Reason,  Machines.MM_Name,Maintenance_Schedule.MS_Workorder, Maintenance_Schedule.MS_Id, Maintenance_Schedule.MFB_Id, Maintenance_Schedule.MS_WOCreate_Timestamp, Maintenance_Schedule.MS_WOClosed_Timestamp, Machines.MM_Number, Maintenance_Schedule.MS_Notes   " & vbCrLf &
                                    "From Maintenance_Schedule" & vbCrLf &
                                    "Inner Join Maintenance_Types" & vbCrLf &
                                    "ON Maintenance_Schedule.MT_Id = Maintenance_Types.MT_Id" & vbCrLf &
                                    "Inner Join Machines" & vbCrLf &
                                    "ON Maintenance_Schedule.MM_Id = Machines.MM_Id" & vbCrLf &
                                    "where (Maintenance_Schedule.MS_Main_Comp_Date is null and Maintenance_Schedule.MS_Workorder is not null and Maintenance_Schedule.EMP_ID is not null and Maintenance_Schedule.MS_Maint_Code = " & Schedule & " and Maintenance_Schedule.MFB_Id = " & Convert.ToString(MFB_Id) & ") or (Maintenance_Schedule.MS_WOClosed_Timestamp > '" & StartTime & "' and Maintenance_Schedule.MS_Maint_Code = " & Schedule & "  AND Maintenance_Schedule.MFB_Id = " & Convert.ToString(MFB_Id) & ")" & vbCrLf &
                                    "order by row_number() over(order by Maintenance_Schedule.MS_Workorder) asc;"
                End If

            Else
                If MFB_Id = -1 Then
                    sqlstring = "Select Maintenance_Types.MT_Description,Maintenance_Schedule.MT_Id, Maintenance_Schedule.MS_Unscheduled_Reason,  Machines.MM_Name,Maintenance_Schedule.MS_Workorder, Maintenance_Schedule.MS_Id, Maintenance_Schedule.MFB_Id, Maintenance_Schedule.MS_WOCreate_Timestamp, Maintenance_Schedule.MS_WOClosed_Timestamp, Machines.MM_Number, Maintenance_Schedule.MS_Notes     " & vbCrLf &
                                    "From Maintenance_Schedule" & vbCrLf &
                                    "Inner Join Maintenance_Types" & vbCrLf &
                                    "ON Maintenance_Schedule.MT_Id = Maintenance_Types.MT_Id" & vbCrLf &
                                    "Inner Join Machines" & vbCrLf &
                                    "ON Maintenance_Schedule.MM_Id = Machines.MM_Id" & vbCrLf &
                                    "where (Maintenance_Schedule.MS_Main_Comp_Date is null and Maintenance_Schedule.MS_Workorder is not null and Maintenance_Schedule.EMP_ID is not null and Maintenance_Schedule.MS_Maint_Code = " & Schedule & ")" & vbCrLf &
                                    "order by row_number() over(order by Maintenance_Schedule.MS_Workorder) asc;"
                Else
                    sqlstring = "Select Maintenance_Types.MT_Description,Maintenance_Schedule.MT_Id, Maintenance_Schedule.MS_Unscheduled_Reason,  Machines.MM_Name,Maintenance_Schedule.MS_Workorder, Maintenance_Schedule.MS_Id, Maintenance_Schedule.MFB_Id, Maintenance_Schedule.MS_WOCreate_Timestamp, Maintenance_Schedule.MS_WOClosed_Timestamp, Machines.MM_Number, Maintenance_Schedule.MS_Notes     " & vbCrLf &
                                    "From Maintenance_Schedule" & vbCrLf &
                                    "Inner Join Maintenance_Types" & vbCrLf &
                                    "ON Maintenance_Schedule.MT_Id = Maintenance_Types.MT_Id" & vbCrLf &
                                    "Inner Join Machines" & vbCrLf &
                                    "ON Maintenance_Schedule.MM_Id = Machines.MM_Id" & vbCrLf &
                                    "where (Maintenance_Schedule.MS_Main_Comp_Date is null and Maintenance_Schedule.MS_Workorder is not null and Maintenance_Schedule.EMP_ID is not null and Maintenance_Schedule.MS_Maint_Code = " & Schedule & " and Maintenance_Schedule.MFB_Id = " & Convert.ToString(MFB_Id) & ")" & vbCrLf &
                                    "order by row_number() over(order by Maintenance_Schedule.MS_Workorder) asc;"
                End If

            End If

            MaintBoard = DAO.getMaintBoardPC(sqlstring, CIDnum)
            returnstring = jser.Serialize(MaintBoard.ToArray())

            Return returnstring

        End Function

        Public Function GetShiftinfo() As String
            Dim returnstring As String
            Dim ShiftArray As New List(Of MaintSchedDAO.ShiftTime)()

            ShiftArray = MSD.GetShiftnTime(DateTime.Now)
            Dim jser As New JavaScriptSerializer
            If ShiftArray.Count > 0 Then
                returnstring = jser.Serialize(ShiftArray.ToArray())
            Else
                returnstring = Nothing
            End If

            Return returnstring

        End Function

        Public Function GetCbxValues(ByVal id As Integer) As String
            Dim sqlstring As String
            Dim result As Boolean
            Dim ComboBox As New List(Of ComboBox)()
            Dim returnstring As String
            Dim jser As New JavaScriptSerializer
            Dim MFB As String

            Select Case id
                Case 0
                    If IsNothing(MaintSchedDAO.MFB_Id) = False Then
                        MFB = "FB" + Convert.ToString(MaintSchedDAO.MFB_Id)
                    Else
                        MFB = "FB1"
                    End If

                    sqlstring = "SELECT MM_Id, MM_Name FROM dbo.Machines where MM_Active = 1 and DATALENGTH(MM_Name) > 1 and MM_MFB_Code1 = '" & MFB & "' order by MM_Name asc;"
                Case 1
                    sqlstring = "SELECT MT_Id, MT_Name, MT_Number FROM dbo.Maintenance_Types where MT_MFB_Code1 = 'FB' ORDER BY MT_Number;"
                Case 2
                    sqlstring = "SELECT EMP_ID, EMP_First_Name FROM dbo.Employees order by EMP_First_Name;"
                Case Else
                    sqlstring = "nothing"
            End Select

            ComboBox = DAO.getComboBox(sqlstring)
            If id = 1 Then
                Dim CBarray As Array = ComboBox.ToArray()
                Dim count As Integer = 0

                For Each combo As ComboBox In CBarray
                    Dim test As String = combo.label
                    Dim splitarray As Object = test.Split(" ")
                    If Not splitarray(0) = ".FB" Then
                        'ComboBox.Remove(New ComboBox() With {.label = combo.label, .Row = combo.Row, .value = combo.value})
                        ComboBox.RemoveAt(count)
                        ComboBox.Add(New ComboBox() With {.label = combo.label, .value = combo.value})
                        GoTo 101

                    End If

                    count = count + 1
101:
                Next
            End If

            returnstring = jser.Serialize(ComboBox.ToArray())

            Return returnstring

        End Function

        Public Function UpdateMobile(ByVal Json As String, ByVal methodname As String, ByVal PassCID As Integer) As Boolean

            Dim dictionary As New Dictionary(Of String, String)
            Dim MaintFlagArray As New List(Of FlagBoard.MaintFlagMSMobile)()
            Dim jser As New JavaScriptSerializer()
            Dim returnrows As Integer
            Dim inputelementarray = jser.Deserialize(Of List(Of InputArray))(Json)

            For Each element As InputArray In inputelementarray
                Dim valuestring = Convert.ToString(element)
                If Not String.IsNullOrEmpty(valuestring) Then
                    If element.key = "MS_Main_Comp_Date" Then
                        If element.value = "true" Then
                            dictionary.Add(element.key, Date.Now.ToString())
                        Else
                            dictionary.Add(element.key, "")
                        End If
                        GoTo 102
                    End If

                    dictionary.Add(element.key, element.value)
                Else
                    dictionary.Add(element.key, Nothing)
                End If
102:
            Next

            If dictionary.Count > 0 Then
                Try
                    MaintFlagArray.Add(New FlagBoard.MaintFlagMSMobile With {.MM_Id = Convert.ToInt32(dictionary.Item("MM_Name")), .MT_Id = Convert.ToInt32(dictionary.Item("MT_Name")), .MS_Frequency = Convert.ToInt32(dictionary.Item("MS_Frequency")), .MS_Next_Main_Date = Convert.ToString(dictionary.Item("MS_Next_Main_Date")), .MS_Maint_Code = Convert.ToInt32(dictionary.Item("MS_Maint_Code")), .MS_Workorder = Convert.ToString(dictionary.Item("MS_Workorder")), .MS_Main_Comp_Date = Convert.ToString(dictionary.Item("MS_Main_Comp_Date")), .EMP_ID = Convert.ToInt32(dictionary.Item("EMP_First_Name")), .MS_Unscheduled_Reason = Convert.ToString(dictionary.Item("MS_Unscheduled_Reason")), .MS_Id = Convert.ToInt32(dictionary.Item("MS_Id")), .MS_WOCreate_Timestamp = Convert.ToString(DateTime.Now), .MFB_Id = Convert.ToInt32(dictionary.Item("MFB_Id")), .MS_Notes = Convert.ToString(dictionary.Item("MS_Notes"))})
                Catch ex As Exception
                    Return False
                    Exit Function
                End Try

                Select Case methodname
                    Case Is = "update"
                        returnrows = MSD.UpdateMobile(MaintFlagArray, PassCID)

                        If returnrows > 0 And MaintFlagArray(0).MS_Main_Comp_Date <> Nothing And MaintFlagArray(0).MS_Maint_Code = 1 Then
                            If IsDate(MaintFlagArray(0).MS_Main_Comp_Date) = True Then
                                Dim NewNextDate As DateTime
                                NewNextDate = Convert.ToDateTime(MaintFlagArray(0).MS_Main_Comp_Date)
                                If IsNumeric(MaintFlagArray(0).MS_Frequency) = True Then
                                    'Dim MaintFlagArray2 As New List(Of FlagBoard.MaintFlagMSMobile)()
                                    Dim CreateTime As DateTime = Date.Now

                                    Dim CreateTimeadd As DateTime = CreateTime.AddDays(Convert.ToDouble(MaintFlagArray(0).MS_Frequency))
                                    Dim NewNextDateadd As DateTime = NewNextDate.AddDays(Convert.ToDouble(MaintFlagArray(0).MS_Frequency))
                                    MaintFlagArray(0).MS_Next_Main_Date = NewNextDateadd.ToString("g")
                                    MaintFlagArray(0).MS_Main_Comp_Date = ""
                                    MaintFlagArray(0).EMP_ID = MSD.GetBlankEMPID()
                                    MaintFlagArray(0).MS_WOCreate_Timestamp = CreateTimeadd.ToString("g")


                                    returnrows = MSD.InsertMobile(MaintFlagArray, PassCID)
                                Else
                                    Return False
                                    Exit Function
                                End If

                            Else
                                Return False
                                Exit Function
                            End If

                        End If

                    Case Is = "new"
                        returnrows = MSD.InsertMobile(MaintFlagArray, PassCID)
                End Select
                If returnrows > 0 Then
                    Return True
                    Exit Function
                Else
                    Return False
                    Exit Function
                End If
            Else
                Return False
                Exit Function
            End If

        End Function
        Public Function UpdatePC(ByVal Json As String, ByVal methodname As String, ByVal CID As String) As String

            Dim dictionary As New Dictionary(Of String, String)
            Dim MaintFlagArray As New List(Of FlagBoard.MaintFlagMS)()
            Dim jser As New JavaScriptSerializer()
            Dim returnrows As Integer
            Dim CIDnum As Integer = Convert.ToInt32(CID)
            Dim inputelementarray As New List(Of InputArray)

            Try

                inputelementarray = jser.Deserialize(Of List(Of InputArray))(Json)

                For Each element As InputArray In inputelementarray
                    Dim valuestring = Convert.ToString(element)
                    If Not String.IsNullOrEmpty(valuestring) Then
                        If element.key = "MS_Maint_Code" Then
                            Select Case element.value
                                Case Is = "UNSCHEDULED"
                                    dictionary.Add(element.key, 2)
                                Case Is = "SCHEDULED"
                                    dictionary.Add(element.key, 1)
                            End Select
                            GoTo 101
                        End If
                        If element.key = "MS_Main_Comp_Date" Then
                            If element.value = "true" Then
                                dictionary.Add(element.key, Date.Now.Date.ToString())
                            Else
                                dictionary.Add(element.key, "")
                            End If
                            GoTo 101
                        End If
                        dictionary.Add(element.key, element.value)
                    Else
                        dictionary.Add(element.key, Nothing)
                    End If
101:
                Next
            Catch ex As Exception
                ErrorFlag = True
                ErrorMessage = "Error Process Page variables - " + ex.Message
                GoTo 102
            End Try
            If dictionary.Count > 0 Then
                Try
                    MaintFlagArray.Add(New FlagBoard.MaintFlagMS With {.MM_Id = Convert.ToInt32(dictionary.Item("MM_Name")), .MT_Id = Convert.ToInt32(dictionary.Item("MT_Name")), .MS_Frequency = Convert.ToInt32(dictionary.Item("MS_Frequency")), .MS_Next_Main_Date = Convert.ToString(dictionary.Item("MS_Next_Main_Date")), .MS_Last_Main_Date = Convert.ToString(dictionary.Item("MS_Last_Main_Date")), .MS_Maint_Code = Convert.ToInt32(dictionary.Item("MS_Maint_Code")), .MS_Workorder = Convert.ToString(dictionary.Item("MS_Workorder")), .MS_Main_Comp_Date = Convert.ToString(dictionary.Item("MS_Main_Comp_Date")), .EMP_ID = Convert.ToInt32(dictionary.Item("EMP_First_Name")), .MS_Unscheduled_Reason = Convert.ToString(dictionary.Item("MS_Unscheduled_Reason")), .MS_Id = Convert.ToInt32(dictionary.Item("MS_Id")), .MS_WOCreate_Timestamp = Convert.ToString(dictionary.Item("_Timestamp")), .MS_WOClosed_Timestamp = Convert.ToString(dictionary.Item("_Timestamp")), .MS_Maint_Time_Alotted = Convert.ToInt16(dictionary.Item("MS_Maint_Time_Alotted")), .MS_Main_Time_Required = Convert.ToInt16(dictionary.Item("MS_Main_Time_Required")), .MS_Total_Machine_Downtime = Convert.ToSingle(dictionary.Item("MS_Total_Machine_Downtime")), .MS_Notes = Convert.ToString(dictionary.Item("MS_Notes")), .MS_Machine_Hours = Convert.ToInt32(dictionary.Item("MS_Machine_Hours")), .MFB_Id = Convert.ToInt32(dictionary.Item("MFB_Id"))})
                Catch ex As Exception
                    ErrorFlag = True
                    ErrorMessage = "Error Process List variables - " + ex.Message
                    GoTo 102
                End Try

                Select Case methodname
                    Case Is = "update"
                        Try
                            returnrows = MSD.Update(MaintFlagArray, CIDnum)

                            If returnrows > 0 And MaintFlagArray(0).MS_Main_Comp_Date <> Nothing And MaintFlagArray(0).MS_Maint_Code = 1 Then
                                If IsDate(MaintFlagArray(0).MS_Main_Comp_Date) = True Then
                                    Dim NewNextDate As DateTime
                                    NewNextDate = Convert.ToDateTime(MaintFlagArray(0).MS_Main_Comp_Date)
                                    If IsNumeric(MaintFlagArray(0).MS_Frequency) = True Then
                                        'Dim MaintFlagArray2 As New List(Of FlagBoard.MaintFlagMSMobile)()
                                        Dim CreateTime As DateTime = Date.Now

                                        Dim CreateTimeadd As DateTime = CreateTime.AddDays(Convert.ToDouble(MaintFlagArray(0).MS_Frequency))
                                        Dim NewNextDateadd As DateTime = NewNextDate.AddDays(Convert.ToDouble(MaintFlagArray(0).MS_Frequency))
                                        MaintFlagArray(0).MS_Next_Main_Date = NewNextDateadd.ToString("g")
                                        MaintFlagArray(0).MS_Main_Comp_Date = ""
                                        MaintFlagArray(0).EMP_ID = MSD.GetBlankEMPID()
                                        MaintFlagArray(0).MS_WOCreate_Timestamp = CreateTimeadd.ToString("g")

                                        returnrows = MSD.Insert(MaintFlagArray, CIDnum)
                                    Else
                                        ErrorFlag = True
                                        ErrorMessage = "Error Frequency must be numeric"
                                        GoTo 102
                                    End If

                                Else
                                    ErrorFlag = True
                                    ErrorMessage = "Error Invalid date"
                                End If

                            End If
                        Catch ex As Exception
                            ErrorFlag = True
                            ErrorMessage = "Error inserting into database - " + ex.Message
                            GoTo 102
                        End Try
                    Case Is = "new"
                        Try
                            returnrows = MSD.Insert(MaintFlagArray, CIDnum)
                        Catch ex As Exception
                            ErrorFlag = True
                            ErrorMessage = "Error updating database - " + ex.Message
                            GoTo 102
                        End Try

                End Select
                If returnrows > 0 Then
                    Return "False"
                    Exit Function
                Else
                    ErrorFlag = True
                    ErrorMessage = "Error - no variables found in input array"
                    GoTo 102
                End If
            Else
                ErrorFlag = True
                ErrorMessage = "Error - no variables found in input array"
                GoTo 102
            End If
102:
            If ErrorFlag = False Then
                Return "False"
            Else
                Return ErrorMessage
            End If

        End Function

        Public Function GetValidImageLinks(ByVal MSID As Integer, ByVal PassCID As Integer) As String
            Dim jser As New JavaScriptSerializer
            Dim sql As String
            Dim ImageCount = New DataSet
            Dim MSIDSet = New DataSet
            Dim MMID As String
            Dim ActiveImageList As New List(Of FlagBoard.ActiveMachineImage)

            If MSID <= 0 Then
                Return Nothing
            End If

            sql = "SELECT MM_Id" & vbCrLf &
            "FROM Maintenance_Schedule" & vbCrLf &
            "WHERE (MS_Id = " & MSID.ToString() & ")"

            If Not Util.FillDataSet(MSIDSet, "TempCount", sql, PassCID) Then
                Return Nothing
            End If

            If MSIDSet.Tables(0).Rows.Count > 0 Then
                MMID = Convert.ToString(MSIDSet.Tables(0).Rows(0)("MM_Id"))
            Else
                Return Nothing
            End If

            sql = "SELECT Mach_Filename1, Mach_Filename2, Mach_Filename3, Mach_Filename4, Mach_Image1, Mach_Image2, Mach_Image3, Mach_Image4, MM_Id" & vbCrLf &
            "FROM Machines" & vbCrLf &
            "WHERE (MM_Id = " & MMID.ToString() & ")"

            If Not Util.FillDataSet(ImageCount, "TempCount", sql, PassCID) Then
                Return Nothing
            End If

            If ImageCount.Tables(0).Rows.Count > 0 Then
                Dim image1status As Boolean = False
                Dim image2status As Boolean = False
                Dim image3status As Boolean = False
                Dim image4status As Boolean = False

                If IsDBNull(ImageCount.Tables(0).Rows(0)("Mach_Filename1")) = False And IsDBNull(ImageCount.Tables(0).Rows(0)("Mach_Image1")) = False Then
                    Dim filename1 As String = ImageCount.Tables(0).Rows(0)("Mach_Filename1")
                    Dim image1bytes As Byte() = ImageCount.Tables(0).Rows(0)("Mach_Image1")

                    If filename1 <> "" And image1bytes.Length > 0 Then
                        image1status = True
                    End If
                End If

                If IsDBNull(ImageCount.Tables(0).Rows(0)("Mach_Filename2")) = False And IsDBNull(ImageCount.Tables(0).Rows(0)("Mach_Image2")) = False Then
                    Dim filename2 As String = ImageCount.Tables(0).Rows(0)("Mach_Filename2")
                    Dim image2bytes As Byte() = ImageCount.Tables(0).Rows(0)("Mach_Image2")

                    If filename2 <> "" And image2bytes.Length > 0 Then
                        image2status = True
                    End If
                End If

                If IsDBNull(ImageCount.Tables(0).Rows(0)("Mach_Filename3")) = False And IsDBNull(ImageCount.Tables(0).Rows(0)("Mach_Image3")) = False Then
                    Dim filename3 As String = ImageCount.Tables(0).Rows(0)("Mach_Filename3")
                    Dim image3bytes As Byte() = ImageCount.Tables(0).Rows(0)("Mach_Image3")
                    If filename3 <> "" And image3bytes.Length > 0 Then
                        image3status = True
                    End If
                End If

                If IsDBNull(ImageCount.Tables(0).Rows(0)("Mach_Filename4")) = False And IsDBNull(ImageCount.Tables(0).Rows(0)("Mach_Image4")) = False Then
                    Dim filename4 As String = ImageCount.Tables(0).Rows(0)("Mach_Filename4")
                    Dim image4bytes As Byte() = ImageCount.Tables(0).Rows(0)("Mach_Image4")
                    If filename4 <> "" And image4bytes.Length > 0 Then
                        image4status = True
                    End If
                End If

                ActiveImageList.Add(New FlagBoard.ActiveMachineImage With {.MMID = Convert.ToUInt32(MMID), .MachineFile1 = image1status, .MachineFile2 = image2status, .MachineFile3 = image3status, .MachineFile4 = image4status})

                Return jser.Serialize(ActiveImageList.ToArray())
            End If

            Return Nothing

        End Function

        Public Function DeleteMaintInputData(ByVal MSID As Integer, ByVal MFBID As Integer, ByVal EMPID As Integer, ByVal PassCID As Integer) As String

            If (IsNumeric(MSID) = True) Then
                If (MSD.Delete(MSID, MFBID, EMPID, PassCID) = True) Then
                    Return "Row Deleted"
                Else
                    Return "DeleteFailed - Multiple IDs"
                End If
            Else
                Return "IDTYPE_Error"
            End If


        End Function

    End Class

End Namespace
