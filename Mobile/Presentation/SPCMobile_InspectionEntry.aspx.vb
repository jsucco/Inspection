Imports System.Web.Script.Serialization
Imports System.Data


Namespace core


    Partial Class Mobile_Presentation_SPCMobile_InspectionReporter
        Inherits core.APRWebApp

        Private Property Inspect As New InspectionUtilityDAO
        Private Property DA As New InspectionInputDAO
        Private Property DU As New InspectionUtilityDAO
        Private Property jser As New JavaScriptSerializer
        ' Private Property as400 As New AS400DAO
        Private Property Util As New Utilities
        Public TemplateNames As String

        Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
            Dim corp As New corporate

            If Page.IsPostBack = False Then
                LoadTemplateNames()
                Dim CID As String = Request.QueryString("CID")
                If CID = "" Then
                    Dim CIDList As New List(Of CID)
                    CIDList = Me.Session("CID_Info")
                    Dim CIDarray = CIDList.ToArray()
                    Response.Redirect("~/Mobile/Presentation/SPCMobile_InspectionEntry.aspx?CID=" + CIDarray(0).CID_Print)
                End If
            End If
        End Sub

        Private Sub LoadTemplateNames()
            Dim TemplateArray As Array = Inspect.GetTemplateList().ToArray()
            Dim serlist As New List(Of selector2array)(TemplateArray)
            Dim cnt As Integer = 0
            For Each item In TemplateArray
                If item.text = "New Template" Then
                    item.text = "Over View"
                End If
                Dim TempCount = New DataSet
                Dim sql As String = "SELECT COUNT(DefectID) AS RecCnt" & vbCrLf &
                                         "FROM DefectMaster" & vbCrLf &
                                         "WHERE(TemplateId = " + item.id.ToString() + ")"
                If Not Util.FillSPCDataSet(TempCount, "TempCount", sql) Then
                    GoTo 101
                End If
                If TempCount.Tables(0).Rows.Count > 0 Then
                    Dim RecordCnt As Integer = Convert.ToInt16(TempCount.Tables(0).Rows(0)(0))
                    If RecordCnt = 0 And item.text <> "No Selection" Then
                        serlist.RemoveAt(cnt)
                        cnt -= 1
                    End If
                End If
101:
                cnt += 1
            Next
            TemplateNames = jser.Serialize(serlist.ToArray())

            If TemplateNames Is Nothing Then
                Response.Redirect("~/ErrorPage.aspx")
            End If

        End Sub

    End Class

End Namespace

