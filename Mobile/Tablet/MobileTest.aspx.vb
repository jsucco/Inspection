Imports System.Data.SqlClient
Imports System.Data
Imports System.Globalization
Imports System.Web.Script.Serialization


Namespace core


    Partial Class Mobile_Tablet_MobileTest
        Inherits core.APRWebApp

        Private MSD As New MaintSchedDAO
        Private Util As New Utilities
        Private DAO As New DAOFactory

        Public MachineStringArray As String
        Public MaintenanceStringArray As String
        Public CompletedByStringArray As String
        Public MS_ID As String
        Public WorkOrder As String
        Public MS_Maint_Code As String
        Public MM_Id As String
        Public MT_Id As String
        Public MS_Frequency As String
        Public MS_Next_Main_Date As String
        Public MS_Main_Comp_Date As String
        Public EMP_ID As String
        Public MS_Unscheduled_Reason As String
        Public MS_Notes As String
        Public MethodType As String
        Public MFBID As Integer

        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            Dim jser As New JavaScriptSerializer
            MFBID = Convert.ToInt32(Session("MFBID"))


            Dim MachineNameArray As Array = GetCbxValues(0)
            Dim MaintenanceTypeArray As Array = GetCbxValues(1)
            Dim CompletedByArray As Array = GetCbxValues(2)
            Dim MSID As String
            Dim MS_MaintCode As Integer
            MachineStringArray = jser.Serialize(MachineNameArray)
            MaintenanceStringArray = jser.Serialize(MaintenanceTypeArray)
            CompletedByStringArray = jser.Serialize(CompletedByArray)

            MSID = Me.Request.QueryString("MSID")
            MethodType = Me.Request.QueryString("Method")
            MS_MaintCode = Convert.ToInt32(Me.Request.QueryString("SchId"))

            If Not MSID = "" Then
                MS_ID = MSID
                If MethodType = "update" Or MethodType = "delete" Then
                    Dim VariablesSet As DataSet = GetPassThroughVariables(Convert.ToInt32(MSID))
                    If Not VariablesSet Is Nothing Then

                        WorkOrder = Convert.ToString(VariablesSet.Tables(0).Rows(0)("MS_Workorder"))
                        MS_Maint_Code = Convert.ToString(VariablesSet.Tables(0).Rows(0)("MS_Maint_Code"))
                        MM_Id = Convert.ToString(VariablesSet.Tables(0).Rows(0)("MM_Id"))
                        MT_Id = Convert.ToString(VariablesSet.Tables(0).Rows(0)("MT_Id"))
                        MS_Frequency = Convert.ToString(VariablesSet.Tables(0).Rows(0)("MS_Frequency"))
                        MS_Next_Main_Date = Convert.ToString(VariablesSet.Tables(0).Rows(0)("MS_Next_Main_Date"))
                        MS_Main_Comp_Date = Convert.ToString(VariablesSet.Tables(0).Rows(0)("MS_Main_Comp_Date"))
                        EMP_ID = Convert.ToString(VariablesSet.Tables(0).Rows(0)("EMP_ID"))
                        MS_Unscheduled_Reason = Convert.ToString(VariablesSet.Tables(0).Rows(0)("MS_Unscheduled_Reason"))
                        MS_Notes = Convert.ToString(VariablesSet.Tables(0).Rows(0)("MS_Notes"))
                    End If
                ElseIf MethodType = "new" Then
                    Dim NewSet As Array = GetNewSet()
                    If NewSet.Length > 0 Then
                        MS_ID = 0
                        WorkOrder = Convert.ToString(NewSet(0).MS_Workorder)
                        MS_Maint_Code = Convert.ToString(MS_MaintCode)
                        MM_Id = Convert.ToString(NewSet(0).MM_Id)
                        MT_Id = Convert.ToString(NewSet(0).MT_Id)
                        MS_Frequency = Convert.ToString(NewSet(0).MS_Frequency)
                        MS_Next_Main_Date = ""
                        MS_Main_Comp_Date = ""
                        EMP_ID = Convert.ToString(NewSet(0).EMP_ID)
                        MS_Unscheduled_Reason = Convert.ToString(NewSet(0).MS_Unscheduled_Reason)
                    End If

                End If

            End If



        End Sub

        Private Function GetPassThroughVariables(ByVal MS_Id As Integer) As DataSet
            Dim sqlstring As String
            Dim MachNameSet As DataSet = New DataSet
            sqlstring = "SELECT MS_Workorder, MS_Maint_Code, MM_Id, MT_Id, MS_Frequency, MS_Next_Main_Date, MS_Main_Comp_Date, EMP_ID, MS_Unscheduled_Reason, MS_Notes FROM Maintenance_Schedule WHERE (MS_Id = " + Convert.ToString(MS_Id) + ")"

            If Not Util.FillDataSet(MachNameSet, "MachNameSet", sqlstring) Then
                Return Nothing
            End If
            Dim test1 = MachNameSet.Tables(0).Rows(0)("MS_Workorder")

            Return MachNameSet
        End Function

        Function GetNewSet() As Array

            Dim returnlist As New List(Of MaintFlagMS)()
            Dim WorkOrder As String
            Dim EMP_ID As Integer = 56
            Dim workordernum As Object = MSD.AutoGenerateWorkOrder()

            If IsNumeric(workordernum) Then
                WorkOrder = Convert.ToString(workordernum)
            Else
                WorkOrder = Convert.ToString(1001)
            End If


            Dim sqlstring As String
            Dim MachNameSet As DataSet = New DataSet
            sqlstring = "SELECT TOP 1 MM_Id, MM_Name, MM_Number from Machines order by MM_Id"

            If Not Util.FillDataSet(MachNameSet, "MachNameSet", sqlstring) Then
                Return Nothing
            End If
            Dim test1 = MachNameSet.Tables(0).Rows(0)("MM_Id")
            Dim test2 = MachNameSet.Tables(0).Rows(0)("MM_Name")

            If Not Request.Cookies("FBUserID") Is Nothing Then
                Dim cookiestring As String = Server.HtmlEncode(Request.Cookies("FBUserID").Value).ToString()
                If IsNumeric(cookiestring) = True Then
                    EMP_ID = Convert.ToInt32(cookiestring)
                End If
            End If

            returnlist.Add(New MaintFlagMS With {.MM_Id = MachNameSet.Tables(0).Rows(0)("MM_Id"), .MM_Name = MachNameSet.Tables(0).Rows(0)("MM_Name"), .MT_Name = "Install", .MS_Maint_Time_Alotted = 0, .MS_Main_Time_Required = 0, .MS_Frequency = 0, .MS_Last_Main_Date = Convert.ToString(DateTime.Now.ToShortDateString), .MS_Next_Main_Date = Convert.ToString(DateTime.Now), .MS_Maint_Code = 2, .MS_Workorder = WorkOrder, .MS_Main_Comp_Date = Convert.ToString(DateTime.Now), .EMP_ID = EMP_ID, .MS_Total_Machine_Downtime = 0, .MS_Machine_Hours = 0, .MS_Unscheduled_Reason = "", .MS_Notes = "", .RowNumber = 0, .MT_Id = 137, .EMP_First_Name = "New Employee", .MM_Number = MachNameSet.Tables(0).Rows(0)("MM_Number")})


            Return returnlist.ToArray()

        End Function

        Private Function GetCbxValues(ByVal id As Integer) As Array
            Dim sqlstring As String
            Dim result As Boolean
            Dim ComboBox As New List(Of ComboBox)()
            Dim returnstring As String

            Dim MFB As String

            Select Case id
                Case 0
                    If IsNothing(MFBID) = False Then
                        MFB = "FB" + Convert.ToString(MFBID)
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
            result = DAO.GetReader(sqlstring)
            If result = True Then
                ComboBox = DAO.getComboBox()
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


                If DAOFactory.coreConnection.State = ConnectionState.Open Then
                    DAOFactory.coreConnection.Close()
                End If
                Return ComboBox.ToArray()
            Else
                Return Nothing
            End If

        End Function



    End Class



End Namespace
