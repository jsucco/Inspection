Imports System.Data

Namespace core


    Partial Class Mobile_DataEntry_OrderEntry
        Inherits core.APRWebApp

        Private Property InspectInput As New InspectionInputDAO
        Public DefectID As Integer
        Public InspectionID As Integer
        Public HasDefectID As String = "false"
        Public HasInspectionID As String = "false"
        Public WorkOrderSelection As String = "[0]"
        Public DefectSelection As String = "[0]"
        Public CID As String
        Public InspectionStarted As Boolean = False
        Private Property util As New Utilities
        Dim DefectIDQS As String = ""
        Dim InspectionIDQS As String = ""
        Dim JobNumberIDQS As String = ""
        Dim InspectionStartedQS As String = ""
        Dim II As New InspectionInputDAO

        Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load

            DefectIDQS = Request.QueryString("DefectID")
            InspectionIDQS = Request.QueryString("InspectionID")
            JobNumberIDQS = Request.QueryString("JobNumber")
            InspectionStartedQS = Request.QueryString("InspectionStarted")
            CID = Request.QueryString("CID")

            If IsNothing(CID) = False And CID <> "" Then
                If IsNothing(DefectIDQS) = False And IsNothing(InspectionIDQS) = False Then
                    If IsNumeric(DefectIDQS) = True Then
                        DefectID = CType(DefectIDQS, Integer)
                        If DefectID > 0 Then
                            HasDefectID = "true"
                            LoadImageInfo()
                            
                        End If
                    End If
                    If IsNothing(JobNumberIDQS) = False Then
                        lblWorkOrderNumber.Text = JobNumberIDQS.ToString()
                    End If
                    If IsNumeric(InspectionIDQS) = True Then
                        InspectionID = CType(InspectionIDQS, Integer)
                        If InspectionID > 0 Then
                            HasInspectionID = "true"
                        End If
                    End If
                End If
                WorkOrderSelection = II.LoadWorkOrderSelection(CID)
                If IsNothing(InspectionStartedQS) = False Then
                    If InspectionStartedQS = "true" Or InspectionStartedQS = "True" Then
                        InspectionStarted = True
                    End If
                End If
            Else
                Response.Redirect("~/APP/Mob/SPCInspectionInput.aspx")
            End If
            If HasDefectID = "false" Then
                If HasInspectionID = "false" Then
                    'WorkOrderSelection = II.LoadWorkOrderSelection(CID)
                Else
                    Dim JobNumberString As String = Request.QueryString("JobNumber")
                    If IsNothing(JobNumberString) = False Then
                        lblWorkOrderNumber.Text = "JobNumber: " + JobNumberString
                        JobNumberHidden.Value = JobNumberString
                        InspectionIDHidden.Value = InspectionID.ToString()
                    Else
                        Response.Redirect("~/APP/Mob/SPCInspectionInput.aspx")
                    End If
                    DefectSelection = II.LoadDefectSelection(InspectionID.ToString())
                End If
            End If
        End Sub

        Private Sub LoadImageInfo()
            If HasDefectID = "true" Then
                Dim DefectMasterArray As Array = GetDefectMasterDataByID(DefectID).ToArray()
                If DefectMasterArray.Length > 0 Then
                    lblWorkOrderNumber.Text = "JobNumber: " + DefectMasterArray(0).WorkOrder
                    DefectDesc.Value = DefectMasterArray(0).DefectDesc
                    Location.Value = DefectMasterArray(0).Location + " - " + DefectMasterArray(0).DefectTime
                End If
            End If

        End Sub

        Protected Function GetDefectMasterDataByID(ByVal DefectID As Integer) As List(Of SPCInspection.DefectMaster)
            Dim sql As String
            Dim DefectMaster = New DataSet
            Dim returnlist As New List(Of SPCInspection.DefectMaster)

            sql = "SELECT DefectDesc, DefectTime, WorkOrder, Location, DefectID" & vbCrLf &
            "FROM DefectMaster" & vbCrLf &
            "WHERE (DefectID = " & DefectID.ToString() & ")"

            If Not Util.FillSPCDataSet(DefectMaster, "DefectMaster", sql) Then
                Return Nothing
            End If

            If DefectMaster.Tables(0).Rows.Count > 0 Then
                returnlist.Add(New SPCInspection.DefectMaster With {.DefectDesc = DefectMaster.Tables(0).Rows(0)("DefectDesc"), .DefectTime = DefectMaster.Tables(0).Rows(0)("DefectTime"), .Location = DefectMaster.Tables(0).Rows(0)("Location"), .WorkOrder = DefectMaster.Tables(0).Rows(0)("WorkOrder")})
                Return returnlist
            End If

            Return Nothing

        End Function

    End Class

End Namespace

