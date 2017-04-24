Imports System.Data.SqlClient
Imports System.Data
Imports System.Globalization
Imports System.Web.Script.Serialization


Namespace core


    Partial Class Mobile_Tablet_MobileMaintSched_Editor
        Inherits core.APRWebApp

        Private MSD As New MaintSchedDAO
        Private Util As New Utilities
        Private DAO As New DAOFactory
        Public FBSelList As String = "[0]"
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
        Public Type As String
        Public MFBNAME As String
        Public CID As String

        Dim listso As New List(Of SingleObject)
        Dim bmapso As New BMappers(Of SingleObject)
        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            Dim jser As New JavaScriptSerializer
            Dim MS_MaintCode As Integer
            Dim MSID As String
            Dim fblist As New List(Of selector2array)
            MFBID = Convert.ToInt16(Request.QueryString("MFBID"))
            Type = Request.QueryString("Type")
            MFBNAME = Request.QueryString("Name")
            MSID = Me.Request.QueryString("MSID")
            MethodType = Me.Request.QueryString("Method")
            MS_MaintCode = Convert.ToInt32(Me.Request.QueryString("SchId"))
            CID = Me.Request.QueryString("CID")
            MFBID = Me.Request.QueryString("MFBID")
            If MFBID > 0 Then
                listso = bmapso.GetCtxObject("SELECT UserID as Object1 FROM Maintenance_FlagBoard WHERE MFB_Id = " & MFBID.ToString() & "", CType(CID, Integer))
                If listso.Count > 0 Then
                    MFBNAME = listso.ToArray()(0).Object1.ToString.ToUpper()
                End If
            End If
            Dim MachineNameArray As Array = GetCbxValues(0)
            Dim MaintenanceTypeArray As Array = GetCbxValues(1)
            Dim CompletedByArray As Array = GetCbxValues(2)

            If MachineNameArray.Length > 0 Then
                MachineStringArray = jser.Serialize(MachineNameArray)
            End If
            If MaintenanceTypeArray.Length > 0 Then
                MaintenanceStringArray = jser.Serialize(MaintenanceTypeArray)
            End If
            If CompletedByArray.Length > 0 Then
                CompletedByStringArray = jser.Serialize(CompletedByArray)
            End If
            If Not MSID = "" Then
                MS_ID = MSID
                If MethodType = "update" Or MethodType = "delete" Then
                    Dim VariablesSet As DataSet = GetPassThroughVariables(Convert.ToInt32(MSID))
                    Dim VariablesList As List(Of MaintFlagMSMobile) = GetPassVariables(Convert.ToInt32(MSID), CID)
                    If Not VariablesList Is Nothing Then
                        If VariablesList.Count > 0 Then
                            'WorkOrder = Convert.ToString(VariablesSet.Tables(0).Rows(0)("MS_Workorder"))
                            'MS_Maint_Code = Convert.ToString(VariablesSet.Tables(0).Rows(0)("MS_Maint_Code"))
                            'MM_Id = Convert.ToString(VariablesSet.Tables(0).Rows(0)("MM_Id"))
                            'MT_Id = Convert.ToString(VariablesSet.Tables(0).Rows(0)("MT_Id"))
                            'MS_Frequency = Convert.ToString(VariablesSet.Tables(0).Rows(0)("MS_Frequency"))
                            'MS_Next_Main_Date = Convert.ToString(VariablesSet.Tables(0).Rows(0)("MS_Next_Main_Date"))
                            'MS_Main_Comp_Date = Convert.ToString(VariablesSet.Tables(0).Rows(0)("MS_Main_Comp_Date"))
                            'EMP_ID = Convert.ToString(VariablesSet.Tables(0).Rows(0)("EMP_ID"))
                            'MS_Unscheduled_Reason = Convert.ToString(VariablesSet.Tables(0).Rows(0)("MS_Unscheduled_Reason"))
                            'MS_Notes = Convert.ToString(VariablesSet.Tables(0).Rows(0)("MS_Notes"))

                            WorkOrder = Convert.ToString(VariablesList.ToArray()(0).MS_Workorder)
                            MS_Maint_Code = Convert.ToString(VariablesList.ToArray()(0).MS_Maint_Code)
                            MM_Id = Convert.ToString(VariablesList.ToArray()(0).MM_Id)
                            MT_Id = Convert.ToString(VariablesList.ToArray()(0).MT_Id)
                            MS_Frequency = Convert.ToString(VariablesList.ToArray()(0).MS_Frequency)
                            MS_Next_Main_Date = Convert.ToString(VariablesList.ToArray()(0).MS_Next_Main_Date)
                            MS_Main_Comp_Date = Convert.ToString(VariablesList.ToArray()(0).MS_Main_Comp_Date)
                            EMP_ID = Convert.ToString(VariablesList.ToArray()(0).EMP_ID)
                            MS_Unscheduled_Reason = Convert.ToString(VariablesList.ToArray()(0).MS_Unscheduled_Reason)
                            MS_Notes = Convert.ToString(VariablesList.ToArray()(0).MS_Notes)
                        End If
                    End If
                ElseIf MethodType = "new" Then
                    Dim NewSet As Array = GetNewSet2(MFBID, Type)
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

            If CID = "" Or CID Is Nothing Or IsNumeric(CID) = False Or MFBID = 0 Then
                Response.Redirect("~/APP/APR_SiteEntry.aspx")
            End If

            Dim bmap_fb As New BMappers(Of selector2array)

            fblist = bmap_fb.GetCtxObject("SELECT MFB_Id AS id, UserID AS text FROM Maintenance_Flagboard", CType(CID, Integer))

            FBSelList = jser.Serialize(fblist)
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

        Private Function GetPassVariables(ByVal MS_Id As Integer, ByVal CID As String) As List(Of MaintFlagMSMobile)
            Dim sqlstring As String = ""
            Dim listmfm As New List(Of MaintFlagMSMobile)
            Dim bmapmfm As New BMappers(Of MaintFlagMSMobile)

            sqlstring = "SELECT MS_Workorder, MS_Maint_Code, MM_Id, MT_Id, MS_Frequency, MS_Next_Main_Date, MS_Main_Comp_Date, EMP_ID, MS_Unscheduled_Reason, MS_Notes FROM Maintenance_Schedule WHERE (MS_Id = " + Convert.ToString(MS_Id) + ")"

            listmfm = bmapmfm.GetCtxObject(sqlstring, CType(CID.ToString().Substring(3, 3), Integer))

            Return listmfm

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

        Function GetNewSet2(ByVal MFBID As Integer, ByVal Type As String) As Array
            Dim jser As New JavaScriptSerializer()
            Dim returnlist As New List(Of MaintFlagMS)()
            Dim returnstring As String
            Dim WorkOrder As String = "1"
            Dim EMP_ID As Integer = 56
            Dim workordernum As Object = MSD.AutoGenerateWorkOrder()
            Dim MM_Number As Integer
            Dim FBNum As String
            If MFBID > 0 Then
                FBNum = Type + MFBID.ToString()
            Else
                FBNum = "FB1"
            End If
            If IsNumeric(workordernum) Then
                WorkOrder = Convert.ToString(workordernum)
            End If

            Dim empset As DataSet = New DataSet
            Dim empstring As String = "SELECT EMP_ID, EMP_First_Name, EMP_Last_Name" & vbCrLf &
                                        "FROM Employees" & vbCrLf &
                                        "WHERE (EMP_First_Name = N'.') AND (EMP_Last_Name = N'.')"
            If Not Util.FillDataSet(empset, "empset", empstring) Then
                Return Nothing
            End If
            If empset.Tables(0).Rows.Count > 0 Then
                EMP_ID = Convert.ToInt32(empset.Tables(0).Rows(0)("EMP_ID"))
            End If

            Dim sqlstring As String
            Dim MachNameSet As DataSet = New DataSet
            sqlstring = "SELECT TOP 1 MM_Id, MM_Name, MM_Number from Machines Where MM_MFB_Code1 = '" & FBNum & "' order by MM_Id"

            If Not Util.FillDataSet(MachNameSet, "MachNameSet", sqlstring) Then
                Return Nothing
            End If
            Dim MMID As Integer
            Dim MMName As String = "NoMachines"
            If MachNameSet.Tables(0).Rows.Count > 0 Then
                MMID = MachNameSet.Tables(0).Rows(0)("MM_Id")
                MMName = MachNameSet.Tables(0).Rows(0)("MM_Name")
                If IsDBNull(MachNameSet.Tables(0).Rows(0)("MM_Number")) = True Then
                    MM_Number = 1
                Else
                    MM_Number = MachNameSet.Tables(0).Rows(0)("MM_Number")
                End If
            End If

            Dim MTstring As String = "SELECT TOP (1) MT_Id FROM Maintenance_Types WHERE (MT_MFB_Code1 = N'" & Type & "')"
            Dim MTset As DataSet = New DataSet
            If Not Util.FillDataSet(MTset, "MTset", MTstring) Then
                Return Nothing
            End If
            Dim MTID As Integer
            If MTset.Tables(0).Rows.Count > 0 Then
                MTID = MTset.Tables(0).Rows(0)("MT_Id")
            End If

            returnlist.Add(New MaintFlagMS With {.MM_Id = MMID, .MM_Name = MMName, .MT_Name = "Install", .MS_Maint_Time_Alotted = 0, .MS_Main_Time_Required = 0, .MS_Frequency = 0, .MS_Last_Main_Date = Convert.ToString(DateTime.Now.ToShortDateString), .MS_Next_Main_Date = Convert.ToString(DateTime.Now), .MS_Maint_Code = 2, .MS_Workorder = WorkOrder, .MS_Main_Comp_Date = "", .EMP_ID = EMP_ID, .MS_Total_Machine_Downtime = 0, .MS_Machine_Hours = 0, .MS_Unscheduled_Reason = "", .MS_Notes = "", .RowNumber = 0, .MT_Id = MTID, .EMP_First_Name = "New Employee", .MM_Number = MM_Number})

            Return returnlist.ToArray()

        End Function

        Private Function GetCbxValues(ByVal id As Integer) As Array
            Dim sqlstring As String
            Dim ComboBox As New List(Of selector2array)()
            Dim bmapcb As New BMappers(Of selector2array)
            Dim MFB As String

            Select Case id
                Case 0
                    If IsNothing(MFBNAME) = False Then
                        MFB = MFBNAME
                    Else
                        MFB = "FB1"
                    End If

                    sqlstring = "SELECT MM_Id AS id, MM_Name as text FROM dbo.Machines where MM_Active = 1 and DATALENGTH(MM_Name) > 1 and MM_MFB_Code1 = '" & MFB & "' order by MM_Name asc;"
                Case 1
                    If Type = "*A" Then
                        sqlstring = "SELECT MT_Id AS id, MT_Name as text FROM dbo.Maintenance_Types  ORDER BY MT_Number;"
                    Else
                        sqlstring = "SELECT MT_Id AS id, MT_Name as text FROM dbo.Maintenance_Types where MT_MFB_Code1 = '" & Type & "' ORDER BY MT_Number;"
                    End If

                Case 2
                    sqlstring = "SELECT EMP_ID as id, EMP_First_Name as text FROM dbo.Employees order by EMP_First_Name;"
                Case Else
                    sqlstring = "nothing"
            End Select
           
            'ComboBox = DAO.getComboBox(sqlstring, CType(CID, Integer))
            ComboBox = bmapcb.GetCtxObject(sqlstring, CType(CID, Integer))
                If id = 1 Then
                    Dim CBarray As Array = ComboBox.ToArray()
                    Dim count As Integer = 0

                For Each combo As selector2array In CBarray
                    Dim test As String = combo.text
                    Dim splitarray As Object = test.Split(" ")
                    If Not splitarray(0) = ".FB" Then
                        'ComboBox.Remove(New ComboBox() With {.label = combo.label, .Row = combo.Row, .value = combo.value})
                        ComboBox.RemoveAt(count)
                        ComboBox.Add(New selector2array With {.text = combo.text, .id = combo.id})
                        GoTo 101

                    End If

                    count = count + 1
101:
                Next
                End If

                Return ComboBox.ToArray()
           
        End Function
    End Class



End Namespace

