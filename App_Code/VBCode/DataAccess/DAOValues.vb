Imports Microsoft.VisualBasic






Namespace core


    Public Class DAOValues

    End Class

    Public Class BaseMasterPage
        Inherits System.Web.UI.MasterPage

        Public Shared Istablet As Boolean
        Public Shared IsMobile As Boolean

    End Class

    Public Class MaintBoardnames


        Public Property NextDate As String
        Public Property MT_Description As String
        Public Property WordOrder As String
        Public Property Active As String
        Public Property MaintCode As String
        Public Property TimeReq As String
        Public Property MMName As String
        Public Property MMNumber As String

    End Class
    Public Class MaintBoard

        Public Property MM_Id As Integer
        Public Property MT_Id As Integer
        Public Property MS_Next_Main_Date As DateTime
        Public Property MM_Number As Integer
        Public Property MS_Unscheduled_Reason As String
        Public Property MMName As String
        Public Property MT_Description As String
        Public Property WorkOrder As String
        Public Property MS_WOCreate_Timestamp As String
        Public Property MS_WOClosed_Timestamp As String
        Public Property MS_Id As Integer
        Public Property MS_Notes As String

    End Class
    Public Class MaintBoardMobile

        Public Property MT_Description As String
        Public Property MFB_Id As Integer
        Public Property MS_Unscheduled_Reason As String
        Public Property MM_Name As String
        Public Property MS_WorkOrder As String
        Public Property MS_WOCreate_Timestamp As DateTime
        Public Property MS_WOClosed_Timestamp As DateTime
        Public Property MS_Id As Integer

    End Class

    Public Class SPCExporter

        Public Property idJobSummary As String
        Public Property WorkOrder As String
        Public Property DataNo As String
        Public Property Description As String
        Public Property TotalSewn As String
        Public Property TotalDefects As String
        Public Property TotalYds As String
        Public Property FinishTime As String
        Public Property RunTime As String
        Public Property DownTime As String
        Public Property Updated As String

    End Class

    Public Class MaintFlagMS


        Public MM_Id As Integer
        Public MM_Name As String
        Public MT_Name As String
        Public MS_Maint_Time_Alotted As Integer
        Public MS_Main_Time_Required As Integer
        'Public  MG_Name As String
        Public MS_Frequency As Integer
        Public MS_Last_Main_Date As String
        Public MS_Next_Main_Date As String
        Public MS_Maint_Code As Integer
        Public MS_Workorder As String
        Public MS_Main_Comp_Date As String
        Public EMP_ID As Integer
        Public MS_Total_Machine_Downtime As Integer
        Public MS_Machine_Hours As Integer
        Public MS_Unscheduled_Reason As String
        Public MS_Notes As String
        Public RowNumber As Integer
        Public MT_Id As Integer
        Public EMP_First_Name As String
        Public Old_MM_ID As Integer
        Public Old_MT_ID As Integer
        Public Old_MS_Next_Main_Date As DateTime
        Public MM_Number As Integer
        Public MFB_Id As Integer
        Public MS_Id As Integer
        Public MS_WOCreate_Timestamp As String

    End Class
    Public Class MaintFlagMS2


        Public Property MM_Id As Integer
        Public Property MM_Name As String
        Public Property MT_Name As String
        Public Property MS_Maint_Time_Alotted As Integer
        Public Property MS_Main_Time_Required As Integer
        'Public Property  MG_Name As String
        Public Property MS_Frequency As Integer
        Public Property MS_Last_Main_Date As String
        Public Property MS_Next_Main_Date As String
        Public Property MS_Maint_Code As Integer
        Public Property MS_Workorder As String
        Public Property MS_Main_Comp_Date As String
        Public Property EMP_ID As Integer
        Public Property MS_Total_Machine_Downtime As Single
        Public Property MS_Machine_Hours As Single
        Public Property MS_Unscheduled_Reason As String
        Public Property MS_Notes As String
        Public Property RowNumber As Long
        Public Property MT_Id As Integer
        Public Property EMP_First_Name As String
        Public Property Old_MM_ID As Integer
        Public Property Old_MT_ID As Integer
        Public Property Old_MS_Next_Main_Date As DateTime
        Public Property MM_Number As Integer
        Public Property MFB_Id As Integer
        Public Property MS_Id As Integer
        Public Property MS_WOCreate_Timestamp As String

    End Class
    Public Class MaintFlagMSMobile

        Public Property MM_Id As Integer
        Public Property MM_Name As String
        Public Property MT_Name As String
        Public Property MS_Frequency As Integer
        Public Property MS_Next_Main_Date As DateTime
        Public Property MS_Maint_Code As Integer
        Public Property MS_Workorder As String
        Public Property MS_Main_Comp_Date As DateTime
        Public Property EMP_ID As Integer
        Public Property MS_Unscheduled_Reason As String
        Public Property MT_Id As Integer
        Public Property EMP_First_Name As String
        Public Property MM_Number As Integer
        Public Property MS_Id As Integer
        Public Property MS_WOCreate_Timestamp As DateTime
        Public Property MS_Notes As String

    End Class
    Public Class ComboBox
        Public Property label As String
        Public Property value As Integer
    End Class

    Public Class Equipment

        Public Property items As String

    End Class

    Public Class LoomStopStats
        
        Public Property Stops_PerShift As Integer
        Public Property Disturbance_PerShift As Decimal
        Public Property FillStop_PerShift As Decimal
        Public Property WarpStop_PerShift As Decimal
        Public Property PieceLength_PerShift As Decimal
        Public Property Host_PerShift As Decimal



    End Class

    Public Class LoomPickGrid

        Public Property field As String
        Public Property value As Decimal

    End Class

    Public Class LoomPickStats

        Public Property PickCount_Curr As Decimal
        Public Property PickCount_ShiftAvg As Decimal
        Public Property PickCount_Max As Decimal

    End Class

    Public Class LoomPicksCurr

        Public Property LoomNo As Integer
        Public Property Picks As Decimal
        Public Property updated As String
    End Class

    Public Class MaintTypeCodes

        Public Property MT_Id As Integer
        Public Property MT_MFB_Code2 As String

    End Class
    Public Class LoomStopsCurr

        Public Property STOPTYPE As String
        Public Property Timestamp As String

    End Class

    Public Class DashBoardSchedule

        Public Property url As String
        Public Property ClipTop As Integer
        Public Property ClipRight As Integer
        Public Property ClipButton As Integer
        Public Property ClipLeft As Integer
        Public Property MainPlateWidth As Integer
        Public Property MainPlateHeight As Integer
        Public Property MainPlateScale As Decimal
        Public Property InnerDivTop As Integer
        Public Property InnerDivLeft As Integer
        Public Property SlideOrder As Integer
        Public Property Type As String
        Public Property TransTime As Integer
        Public Property TableSourceId As Decimal


    End Class

    Public Class Chartdatavalues

        Public Property Timestamp As String
        Public Property value1 As Decimal
        Public Property value2 As Decimal

    End Class

    Public Class SPCJobProduction

        Public Property Machine As String
        Public Property JobNo As Integer
        Public Property StartTime As String
        Public Property CutLengthSpec As Decimal
        Public Property JobYds As Decimal
        Public Property JobSheets As Integer
        Public Property Efficiency As Decimal
        Public Property AvgSheetsPerHour As Decimal

    End Class

    Public Class buttonlibrary
        Public Property label As String
        Public Property value As String
    End Class

End Namespace