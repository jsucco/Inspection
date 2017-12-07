Imports Microsoft.VisualBasic
Imports System.Reflection


Namespace core

    Public Class ClassFieldInfo
        Public Name As String
        Public Type As String
    End Class

    Namespace FlagBoard

        Public Class MaintFlagMSMobile

            Public Property MM_Id As Integer
            Public Property MT_Id As Integer
            Public Property MS_Frequency As Integer
            Public Property MS_Next_Main_Date As String
            Public Property MS_Maint_Code As Integer
            Public Property MS_Workorder As String
            Public Property MS_Main_Comp_Date As String
            Public Property EMP_ID As Integer
            Public Property MS_Unscheduled_Reason As String
            Public Property MS_Id As Integer
            Public Property MS_WOCreate_Timestamp As String
            Public Property MFB_Id As Integer
            Public Property MS_Notes As String

        End Class

        Public Class MaintFlagMS

            Public Property MM_Id As Integer
            Public Property MS_Maint_Time_Alotted As Integer
            Public Property MS_Main_Time_Required As Integer
            Public Property MS_Frequency As Decimal
            Public Property MS_Last_Main_Date As String
            Public Property MS_Next_Main_Date As String
            Public Property MS_Maint_Code As Integer
            Public Property MS_Workorder As String
            Public Property MS_Main_Comp_Date As String
            Public Property EMP_ID As Integer
            Public Property MS_Total_Machine_Downtime As Decimal
            Public Property MS_Machine_Hours As Integer
            Public Property MS_Unscheduled_Reason As String
            Public Property MS_Notes As String
            Public Property MT_Id As Integer
            Public Property MM_Number As Integer
            Public Property MFB_Id As Integer
            Public Property MS_Id As Integer
            Public Property MS_WOCreate_Timestamp As String
            Public Property MS_WOClosed_Timestamp As String
        End Class

        Public Class MaintBoardPC

            Public MM_Number As Integer
            Public MT_Description As String
            Public MT_Id As Integer
            Public MS_Unscheduled_Reason As String
            Public MMName As String
            Public WorkOrder As String
            Public MFB_Id As String
            Public MS_WOCreate_Timestamp As String
            Public MS_WOClosed_Timestamp As String
            Public MS_Id As Int64
            Public MS_Notes As String


        End Class
        Public Class MaintBoardPCtest

            Public Property MM_Number As Integer
            Public Property MT_Description As String
            Public Property MT_Id As Integer
            Public Property MS_Unscheduled_Reason As String
            Public Property MMName As String
            Public Property WorkOrder As String
            Public Property MS_WOCreate_Timestamp As DateTime
            Public Property MS_WOClosed_Timestamp As DateTime
            Public Property MS_Id As Integer
            Public Property MS_Notes As String


        End Class
        Public Class ActiveMachineImage
            Public Property MMID As Integer
            Public Property MachineFile1 As Boolean
            Public Property MachineFileName1 As String
            Public Property MachineFileBytes1 As Byte()
            Public Property MachineFile2 As Boolean
            Public Property MachineFileName2 As String
            Public Property MachineFileBytes2 As Byte()
            Public Property MachineFile3 As Boolean
            Public Property MachineFileName3 As String
            Public Property MachineFileBytes3 As Byte()
            Public Property MachineFile4 As Boolean
            Public Property MachineFileName4 As String
            Public Property MachineFileBytes4 As Byte()
        End Class

        Public Class Employees
            Public Property EMP_ID As Integer
            Public Property EMP_Last_Name As String
            Public Property EMP_First_Name As String
            Public Property EMP_Active As Boolean
            Public Property EMP_Type As Integer
            Public Property EMP_Pay_Rate As Decimal
            Public Property EMP_Hours_Per_Period As Integer
            Public Property EMP_Hospital As String
            Public Property EMP_Area As Integer
            Public Property EMP_LoginLink As String
            Public Property EMP_Email As String
        End Class

        Public Class FlagBoardID
            Public Property MFB_Id As Integer
            Public Property UserID As Integer
        End Class
    End Namespace

    Namespace SPCInspection

        Public Class tabarray
            Public Property title As String
            Public Property TabNumber As Integer
            Public Property TabTemplateId As Integer
        End Class
        Public Class buttonarray
            Public Property text As String
            Public Property tabindex As Integer
            Public Property id As Integer
            Public Property ButtonId As Integer
            Public Property DefectType As String
            Public Property Hide As Boolean
            Public Property Timer As Boolean
        End Class

        Public Class buttonlibrary
            Public Property id As Integer
            Public Property label As String
            Public Property value As Object
        End Class
        Public Class ButtonLibrarygrid
            Public Property ButtonId As Integer
            Public Property Name As String
            Public Property DefectCode As String
            Public Property Hide As Boolean
        End Class
        Public Class ButtonTemplate
            Public Property id As Integer
            Public Property TabTemplateId As Integer
            Public Property ButtonId As Integer
            Public Property DefectType As String
            Public Property Hide As Boolean
            Public Property Timer As Boolean
        End Class

        Public Class TemplateCollection
            Public Property TabTemplateId As Integer
            Public Property ButtonId As Integer
            Public Property Name As String
            Public Property LineType As String
            Public Property TabNumber As Integer
            Public Property TemplateId As Integer
            Public Property ButtonName As String
            Public Property ProductSpecs As Boolean
            Public Property DefectType As String
            Public Property id As Integer
            Public Property DefectCode As Object
            Public Property Hide As Boolean
            Public Property ButtonLibraryId As Integer
            Public Property Timer As Boolean
            Public Property ColumnCount As Integer
        End Class

        Public Class ProductSpecCollection
            Public Property TabTemplateId As Integer
            Public Property TabName As String
            Public Property TabNumber As Integer
            Public Property SpecId As Integer
            Public Property Spec_Description As String
            Public Property value As Decimal
            Public Property Spec_Value_Upper As Decimal
            Public Property Spec_Value_Lower As Decimal
            Public Property Measured_Value As Decimal
        End Class

        Public Class ProductDisplaySpecCollection
            Public Property id As Integer
            Public Property SpecId As Integer
            Public Property DefectId As Integer
            Public Property InspectionJobSummaryId As Integer
            Public Property Timestamp As DateTime
            Public Property JobNumber As String
            Public Property ProductType As String
            Public Property DataNo As String
            Public Property ItemNumber As Integer
            Public Property Inspection_Started As DateTime
            Public Property InspectionId As Integer
            Public Property MeasureValue As Decimal
            Public Property POM_Row As Integer
            Public Property Spec_Description As String
            Public Property value As Decimal
            Public Property Upper_Spec_Value As Decimal
            Public Property Lower_Spec_Value As Decimal
            Public Property SpecDelta As Decimal
            Public Property Location As String
        End Class
        Public Class DefectSelection
            Public Property DefectId As Integer
            Public Property DefectTime As String
            Public Property DefectDesc As String
            Public Property InspectionJobSummaryId As Integer
        End Class
        Public Class DefectMaster
            Public Property DefectId As Long
            Public Property DefectTime As DateTime
            Public Property POnumber As String
            Public Property DefectDesc As String
            Public Property DataNo As String
            Public Property EmployeeNo As String
            Public Property Product As String
            Public Property DefectClass As String
            Public Property AQL As String
            Public Property RejectLimiter As Integer
            Public Property TotalLotPieces As String
            Public Property Tablet As String
            Public Property WorkOrder As String
            Public Property RollNumber As String
            Public Property LoomNumber As Integer
            Public Property GriegeNo As String
            Public Property LotNo As String
            Public Property Location As String
            Public Property DataType As String
            Public Property Comment As String
            Public Property Dimensions As String
            Public Property SampleSize As String
            Public Property LotSize As String
            Public Property ThisPieceNo As String
            Public Property MergeDate As DateTime
            Public Property TemplateId As Long
            Public Property ButtonTemplateId As Long
            Public Property InspectionId As Long
            Public Property DefectImage As Byte()
            Public Property DefectImage_Filename As String
            Public Property Inspector As String
            Public Property ItemNumber As String
            Public Property InspectionState As String
            Public Property WorkRoom As String
            Public Property InspectionJobSummaryId As Long
            Public Property WeaverShiftId As Integer
        End Class

        Public Class DefectTimers
            Public Property id As Integer
            Public Property StatusValue As String
            Public Property InspectionJobSummaryId As Integer
            Public Property ButtonTemplateId As Integer
            Public Property DefectID As Integer
            Public Property SessionId As String
            Public Property Timestamp As DateTime
            Public Property ButtonLocationId As Integer
            Public Property StopTimestamp As DateTime
        End Class

        Public Class InspectionJobSummary
            Public Property id As Integer
            Public Property JobType As String
            Public Property DataNo As String
            Public Property JobNumber As String
            Public Property CID As String
            Public Property TemplateId As Integer
            Public Property Name As String
            Public Property ItemPassCount As Integer
            Public Property ItemFailCount As Integer
            Public Property WOQuantity As Integer
            Public Property WorkOrderPieces As Integer
            Public Property AQL_Level As Decimal
            Public Property SampleSize As Integer
            Public Property Standard As String
            Public Property TotalInspectedItems As Integer
            Public Property RejectLimiter As Integer
            Public Property Technical_PassFail As Boolean
            Public Property Technical_PassFail_Timestamp As DateTime
            Public Property UserConfirm_PassFail As Boolean
            Public Property UserConfirm_PassFail_Timestamp As DateTime
            Public Property Inspection_Started As DateTime
            Public Property Inspection_StartedString As String
            Public Property Inspection_Finished As DateTime
            Public Property PRP_Code As String
            Public Property Comments As String
            Public Property ProdMachineName As String
            Public Property UnitCost As Double
            Public Property UnitDesc As String
            Public Property IsSPC As Boolean
            Public Property MajorsCount As Integer
            Public Property MinorsCount As Integer
            Public Property RepairsCount As Integer
            Public Property ScrapCount As Integer
            Public Property EmployeeNo As String
            Public Property CasePack As String
            Public Property WorkRoom As String
        End Class

        Public Class Workroom
            Public Property Id As Integer
            Public Property Name As String
            Public Property Abbreviation As String
            Public Property Status As Boolean
        End Class

        Public Class AlertEmails
            Public Property Id As Integer
            Public Property Code As String
            Public Property CID As String
            Public Property Loc As String
            Public Property Email As String
            Public Property ACTIVE As Boolean
            Public Property AUTOCOMP As Boolean
            Public Property INSCOMP As Boolean
            Public Property INSCOMPEX As Boolean
        End Class

        Public Class OpenRollInfo
            Public Property JobNumber As String
            Public Property DataNo As String
            Public Property WOQuantity As Integer
            Public Property AQL_Level As Decimal
            Public Property Standard As String
            Public Property EmployeeNoId As Integer
            Public Property Initials As String
            Public Property EmployeeNo As String
            Public Property ShiftId As Integer
            Public Property CasePack As String
        End Class
        Public Class StartJobInfo
            Public Property JobSummaryId As Integer = -99
            Public Property WeaverShiftId As Integer
        End Class
        Public Class DefectTimer
            Public Property id As Integer
            Public Property InspectionJobSummaryId As Integer
            Public Property ButtonTemplateId As Integer
            Public Property StatusValue As String
            Public Property DefectID As Integer
            Public Property SessionId As String
            Public Property Timestamp As DateTime
            Public Property StopTimestamp As DateTime
        End Class
        Public Class InspectionJobSumarryReport
            Public Property id As Integer
            Public Property JobType As String
            Public Property JobNumber As String
            Public Property Name As String
            Public Property LocationName As String
            Public Property UnitDesc As String
            Public Property LineType As String
            Public Property TotalInspectedItems As Integer
            Public Property ItemPassCount As Integer
            Public Property ItemFailCount As Integer
            Public Property WOQuantity As Integer
            Public Property WorkOrderPieces As Integer
            Public Property AQL_Level As Decimal
            Public Property SampleSize As Integer
            Public Property RejectLimiter As Integer
            Public Property UnitCost As Double
            Public Property DHU As Decimal
            Public Property Technical_PassFail As Boolean
            Public Property Inspection_Started As DateTime
            Public Property Inspection_Finished As DateTime
            Public Property Comments As String
        End Class
        Public Class DefectMasterDisplay
            Public Shared FuncPropList As New Dictionary(Of String, String)
            Public Property DefectID As Integer
            Public Property DefectTime As DateTime
            Public Property WorkOrder As String
            Public Property ItemNumber As String
            Public Property RollNo As String
            Public Property DataNo As String
            Public Property AQL As Decimal
            Public Property EmployeeNo As String
            Public Property Inspector As String
            Public Property Name As String
            Public Property InspectionId As Integer
            Public Property DefectDesc As String
            Public Property TotalLotPieces As String
            Public Property Product As String
            Public Property LoomNo As String
            Public Property DataType As String
            Public Property WorkRoom As String
            'Public Property POnumber As String
            Public Property TemplateId As Integer
            Public Property DefectClass As String
            Public Property Location As String
            Public Property DefectImage_Filename As String
            Public Property InspectionState As String
        End Class
        Public Class DefectImageDisplay_
            Public Property DefectID_ As Integer
            Public Property DefectTime_ As DateTime
            Public Property Location_ As String
            Public Property DefectDesc_ As String
            Public Property Inspection_Started_ As DateTime
            Public Property JobNumber_ As String
            Public Property UnitDesc_ As String
            Public Property Technical_PassFail_ As Boolean
            Public Property ijsid_ As Integer
            Public Property DataNo_ As String
            Public Property TemplateName_ As String
            Public Property AuditType As String
            Public Property Prp_Code As String
            Public Property Image As Byte()
        End Class
        Public Class DefectMasterSubgrid
            Public Property DefectID As Integer
            Public Property DefectTime As String
            Public Property EmployeeNo As String
            Public Property InspectionId As Integer
            Public Property DefectDesc As String
            Public Property Product As String
            Public Property DefectClass As String
            Public Property WorkRoom As String
        End Class
        Public Class SpecsSubgrid
            Public Property SMid As Integer
            Public Property JobNumber As String
            Public Property DataNo As String
            Public Property Timestamp As String
            Public Property ProductType As String
            Public Property Spec_Description As String
            Public Property value As Decimal
            Public Property Upper_Spec_Value As Decimal
            Public Property Lower_Spec_Value As Decimal
            Public Property MeasureValue As Decimal
            Public Property SpecDelta As Decimal
            Public Property SpecSource As String
        End Class
        Public Class InspectionCacheVar
            Public Property LastDefectID As Integer
            Public Property LastDefectIDTimeStamp As DateTime
            Public Property CID As String
        End Class

        Public Class DefectMasterCount
            Public Property Count As Integer
            Public Property TemplateId As Integer
            Public Property Year As Integer
            Public Property Month As Integer
            Public Property Listdate As DateTime
        End Class
        Public Class GraphTable
            Public Property Listdate As String
            Public Property Count As Integer
        End Class
        Public Class PieTable
            Public Property Desc As String
            Public Property Count As Decimal
        End Class
        Public Class JobSummary
            Public Property RowID As Integer
            Public Property Machine As String
            Public Property WorkOrderID As String
            Public Property HourBegin As DateTime
            Public Property ProductCount As Integer
            Public Property OverLengthInches As Decimal
            Public Property CutLengthSpec As Decimal
            Public Property AvgYdsPmin As Decimal
            Public Property HourlyYds As Decimal
            Public Property RunTime As Decimal
            Public Property DownTime As Decimal
            Public Property DefPerProd As Double
        End Class
        Public Class JobSummary_DBreakdown
            Public Property RowID As Integer
            Public Property Machine As String
            Public Property WorkOrderID As String
            Public Property DTYPE As String
            Public Property BSPCId As Integer
            Public Property DEFSUM As Integer
        End Class
        Public Class InspectionCarton
            Public Property WONUM As Integer
            Public Property DATAN As String
            Public Property CASEPACK As Integer
            Public Property WOQUANTITY As Integer
            Public Property LOCATION As String
        End Class
        Public Class InspectionWorkOrder
            Public Property DATAN As String
            Public Property WOQUANTITY As Integer
            Public Property LOCATION As String
            Public Property SHORTITEM As Integer
            Public Property CASEPACK As Integer
            Public Property WORKROOM As String
            Public Property WOPIECES As Integer
        End Class
        Public Class as400WorkOrder
            Public Property WADOCO As String
            Public Property WALITM As String
            Public Property WO_QTY As Decimal
            Public Property WAMCU As String
            Public Property WAITM As Object
            Public Property QTY As Decimal
            Public Property WAMMCU As String
        End Class
        Public Class Roll_Ledge
            Public Property GREIGENUMBER As String
            Public Property TICKETYARDS As Decimal
        End Class
        Public Class BarChart
            Public Property x As String
            Public Property y As Integer
        End Class
        Public Class StackedDefectLineType
            Public Property DefectDesc As String
            Public Property IL As Integer
            Public Property EOL As Integer
            Public Property ROLL As Integer
        End Class

        Public Class TemplateTable
            Public Property TemplateId As Integer
            Public Property Name As String
            Public Property Owner As String
            Public Property Loc_STT As Boolean
            Public Property Loc_CAR As Boolean
            Public Property Loc_STJ As Boolean
            Public Property Loc_SPA As Boolean
            Public Property Loc_CDC As Boolean
            Public Property Loc_LINYI As Boolean
            Public Property Loc_FSK As Boolean
            Public Property Loc_FNL As Boolean
            Public Property Loc_FPC As Boolean
            Public Property Loc_PCE As Boolean
            Public Property DateCreated As DateTime
            Public Property Status As String
            Public Property Active As Boolean
        End Class
        Public Class InspectionTemplateManagement
            Public Property TemplateId As Integer
            Public Property Name As String
            Public Property Owner As String
            Public Property Loc_STT As Boolean
            Public Property Loc_CAR As Boolean
            Public Property Loc_STJ As Boolean
            Public Property Loc_SPA As Boolean
            Public Property Loc_CDC As Boolean
            Public Property Loc_LINYI As Boolean
            Public Property Loc_FSK As Boolean
            Public Property Loc_FNL As Boolean
            Public Property Loc_FPC As Boolean
            Public Property Loc_PCE As Boolean
            Public Property DateCreated As DateTime
            Public Property Active As Boolean
            Public Property ColumnCount As Integer
        End Class
        Public Class TemplateDirectory
            Public Property TemplateId As Integer
            Public Property Name As String
            Public Property Owner As String
            Public Property DateCreated As DateTime
            Public Property Active As Boolean
            Public Property Ins_GriegeBatch As Boolean
            Public Property Ins_WorkOrderInspection As Boolean
        End Class

        Public Class ProductSpecs
            Public Property SpecId As Integer
            Public Property POM_Row As Integer
            Public Property TabTemplateId As Integer
            Public Property DataNo As String
            Public Property ProductType As String
            Public Property Spec_Description As String
            Public Property HowTo As String
            Public Property value As Decimal
            Public Property Upper_Spec_Value As Decimal
            Public Property Lower_Spec_Value As Decimal
            Public Property GlobalSpec As Boolean
            Public Property SpecSource As String
        End Class
        Public Class PDMProductSpecs
            Public Property FolderId As Integer
            Public Property Style As String
            Public Property PageId As Integer
            Public Property CompanyId As Integer
            Public Property ProductType As String
            Public Property Position As Integer
            Public Property Name As String
            Public Property POM_Row As Integer
            Public Property Grade As String
            Public Property RefCode As String
            Public Property Description As String
            Public Property TolPlus As String
            Public Property TolMinus As String
            Public Property HowTo As String
        End Class
        Public Class InspectProductSpec
            Public Property SpecId As Integer
            Public Property DataNo As String
            Public Property POM_Row As Integer
            Public Property RefCode As String
            Public Property ProductType As String
            Public Property value As Decimal
            Public Property Spec_Description As String
            Public Property Upper_Spec_Value As Decimal
            Public Property Lower_Spec_Value As Decimal
            Public Property Measured_Value As Decimal
            Public Property GlobalSpec As Boolean
        End Class
        Public Class ProductTableSpecs

            Public Property Spec_Description As String
            Public Property DataNo As String
            Public Property value As Decimal
            Public Property Upper_Spec_Value As Decimal
            Public Property Lower_Spec_Value As Decimal
            Public Property id As String
        End Class
        Public Class InspectionVaribles
            Public Property SampleSize As String
            Public Property Acceptance As Integer
            Public Property AC As String
            Public Property RE As String
            Public Property LotSize As String
        End Class
        Public Class Weavers
            Public Property Weaver1ID As Integer
            Public Property Weaver1Initials As String
            Public Property Weaver2ID As Integer
            Public Property Weaver2Initials As String
        End Class
        Public Class RollLoom
            Public Property LoomNo As String
            Public Property RollNo As String
        End Class

        Public Class FilterColumnValues
            Public Property col As Object
            Public Property val As Object
        End Class

        Public Class JobSummary_old
            Public Property idJobSummary As Integer
            Public Property WorkOrder As String
            Public Property DataNo As String
            Public Property Description As String
            Public Property Machine As String
            Public Property TotalSewn As Integer
            Public Property TotalDefects As Integer
            Public Property TotalDefectPercentage As Decimal
            Public Property DefectsPerHundredYards As Decimal
            Public Property TotalWeaveDefects As Integer
            Public Property TotalFinishingDefects As Integer
            Public Property TotalSewDefects As Integer
            Public Property TotalYards As Decimal
            Public Property WEAVE_SEAMS As Integer
            Public Property SELVEDGE_STRINGS As Integer
            Public Property INSIDE_TAILS As Integer
            Public Property BROKEN_PICKS As Integer
            Public Property THIN_PLACES As Integer
            Public Property OIL_SPOTS As Integer
            Public Property RED_DYE_SPOTSSTAINS As Integer
            Public Property BLUE_DYE_SPOTSSTAINS As Integer
            Public Property GRAY_SPOTSSTAINS As Integer
            Public Property BLACK_SPOTSSTAINS As Integer
            Public Property BROWN_SPOTSSTAINS As Integer
            Public Property FINISH_DIRTY_HANDLING As Integer
            Public Property SHADED_FABRIC_HANDLING As Integer
            Public Property NARROW_FABRIC As Integer
            Public Property CLIP_OUT As Integer
            Public Property TORN_SELVAGE As Integer
            Public Property FINISH_SEAMS As Integer
            Public Property HOLES As Integer
            Public Property PLEATED_FABRIC As Integer
            Public Property RAW_HEMS As Integer
            Public Property TEARS As Integer
            Public Property LIGHT_OIL As Integer
            Public Property SEW_SEAMS As Integer
            Public Property SEW_DIRTY_HANDLING As Integer
            Public Property COLORED_FLY As Integer
            Public Property FinishTime As DateTime
            Public Property RunTime As Decimal
            Public Property DownTime As Decimal
            Public Property CutlengthOverage As Decimal
            Public Property RunTimeEfficiency As Decimal
            Public Property AvgSheetsPerHour As Decimal
            Public Property Updated As DateTime
            Public Property Roll_OperatorList As String

        End Class

        Public Class DefectGroup
            Public DefectType As String
            Public Count As Integer
        End Class

        Public Class DefectsPercentage
            Public Month As Integer
            Public Day As Integer
            Public year As Integer
            Public pvalue As Decimal
        End Class

        Public Class RollInspectionSummaryHeaders
            Public Property LoomNo As Integer
            Public Property RollNumber As String
            Public Property Style As String
            Public Property Yards_Inspected As Decimal
            Public Property DefectYardsf As Integer
            Public Property DHY As Decimal
            Public Property RollStartTimestamp As DateTime
        End Class
        Public Class InspectionSummaryDisplay
            Public Property ijsid As Integer
            Public Property JobType As String
            Public Property JobNumber As String
            Public Property DataNo As String
            Public Property PRP_Code As String
            Public Property UnitDesc As String
            Public Property CID As String
            Public Property Location As String
            Public Property TemplateId As Integer
            Public Property Name As String
            Public Property LineType As String
            Public Property LineTypeVariable As String
            Public Property TotalInspectedItems As Integer
            Public Property ItemPassCount As Integer
            Public Property ItemFailCount As Integer
            Public Property WOQuantity As Integer
            Public Property WorkOrderPieces As Integer
            Public Property AQL_Level As Decimal
            Public Property SampleSize As Integer
            Public Property RejectLimiter As Integer
            Public Property Technical_PassFail As String
            Public Property STARTED As DateTime
            Public Property FINISHED As DateTime?
            Public Property MajorsCount As Double
            Public Property MinorsCount As Double
            Public Property RepairsCount As Double
            Public Property ScrapCount As Double
            Public Property DHU As Double
            Public Property RejectionRate As Double
            Public Property UnitCost As Double
            Public Property Comments As String
            Public Property UpdatedInspectionStarted As Boolean
        End Class
        Public Class SpecSummaryDisplay
            Public Property id As Integer
            Public Property CID As String
            Public Property Location As String
            Public Property InspectionJobSummaryId As Integer
            Public Property JobNumber As String
            Public Property DataNo As String
            Public Property ProductType As String
            Public Property LineType As String
            Public Property LineTypeVariable As String
            Public Property UnitDesc As String
            Public Property Inspection_Started As String
            Public Property Inspection_Finished As String
            Public Property totcount As Integer
            Public Property SpecsMet As Integer
            Public Property SpecsFailed As Integer
        End Class
        Public Class Dump
            Public Property id As Integer
            Public Property JobType As String
            Public Property JobNumber As String
            Public Property INSDataNum As String
            Public Property LOCCID As String
            Public Property LOCName As String
            Public Property INStemplateID As Integer
            Public Property TMPName As String
            Public Property ItemPassCount As Integer
            Public Property ItemFailCount As Integer
            Public Property WOQuantity As Integer
            Public Property WorkOrderPieces As Integer
            Public Property AQL_Level As Decimal
            Public Property Standard As String
            Public Property INSSampleSize As Integer
            Public Property TotalInspectedItems As Integer
            Public Property INSRejectLimiter As Integer
            Public Property Technical_PassFail As Boolean
            Public Property Technical_PassFail_Timestamp As DateTime
            Public Property UserConfirm_PassFail As Boolean
            Public Property UserConfirm_PassFail_Timestamp As DateTime
            Public Property Inspection_Started As DateTime
            Public Property Inspection_Finished As DateTime
            Public Property UnitCost As Double
            Public Property UnitDesc As String
            Public Property Comments As String
            Public Property ProdMacineName As String
            Public Property MajorsCount As Integer
            Public Property MinorsCount As Integer
            Public Property RepairsCount As Integer
            Public Property ScrapCount As Integer
            Public Property DefectID As Integer
            Public Property DefectTime As DateTime
            Public Property DefectDesc As String
            Public Property POnumber As String
            Public Property DataNo As String
            Public Property EmployeeNo As String
            Public Property AQL As String
            Public Property ThisPieceNo As String
            Public Property SampleSize As String
            Public Property RejectLimiter As Integer
            Public Property TotalLotPieces As String
            Public Property Product As String
            Public Property DefectClass As String
            Public Property MergeDate As DateTime
            Public Property Tablet As Object
            Public Property WorkOrder As String
            Public Property LotNo As String
            Public Property Location As String
            Public Property Datatype As String
            Public Property Dimensions As String
            Public Property Comment As String
            Public Property LoomNo As Object
            Public Property DefectPoints As Object
            Public Property GriegeNo As String
            Public Property RollNo As String
            Public Property Operation As String
            Public Property TemplateId As Integer
            Public Property InspectionId As Integer
            Public Property ButtonTemplateId As Integer
            Public Property Inspector As String
            Public Property ItemNumber As String
            Public Property InspectionState As String
            Public Property CasePackConv As Object
            Public Property WorkRoom As String
            Public Property InspectionJobSummaryId As Integer
            Public Property TMPtempalteID As Integer
            Public Property Name As String
            Public Property Owner As String
            Public Property DateCreated As DateTime
            Public Property Active As Boolean
            Public Property LineType As String
            Public Property Ins_GriegeBatch As Boolean
            Public Property Ins_WorkOrderInspection As Boolean
            Public Property Loc_STT As Boolean
            Public Property LOC_CAR As Boolean
            Public Property LOC_STJ As Boolean
            Public Property LOC_SPA As Boolean
            Public Property LOC_CDC As Boolean
            Public Property LOC_LINYI As Boolean
            Public Property Loc_PCE As Boolean
            Public Property Loc_FSK As Boolean
            Public Property Loc_FNL As Boolean
            Public Property Loc_FPC As Boolean
            Public Property LOCID As Integer
            Public Property Abreviation As String
            Public Property DBname As String
            Public Property CID As String
            Public Property ConnectionString As String
            Public Property InspectionResults As Boolean
            Public Property ProductionResults As Boolean
            Public Property AS400_Connection As Boolean
            Public Property AS400_Abr As String
        End Class

        Public Class TimerReport
            Public Property JobType As String
            Public Property JobNumber As String
            Public Property Location As String
            Public Property CID As String
            Public Property DataNo As String
            Public Property UnitDesc As String
            Public Property DefectName As String
            Public Property DefectType As String
            Public Property EmployeeNo As String
            Public Property Timestamp As DateTime
            Public Property StopTimestamp As DateTime
            Public Property Timespan_min As Integer
        End Class

        Public Class RollInspectionDetailTable
            Public Property ButtonId As Integer
            Public Property Text As String
            Public Property ShiftNumber As Integer
            Public Property DHY As Decimal
            Public Property DefectCount As Integer
            Public Property RSID As Integer
        End Class

        Public Class WorkOrderInspection
            Public Property WorkDate As String
            Public Property WorkOrder As String
            Public Property Auditor As String
            Public Property DataNo As String
            Public Property WO_Pieces As Integer
            Public Property AQL_Boxes As Integer
            Public Property AQL_Pieces As Double
            Public Property AQL_Percent As Double
            Public Property Rejected As Integer
            Public Property Rejected_Percent As Double
        End Class
        Public Class WorkOrderInspectionSummary
            Public Property WorkDate As String
            Public Property WorkOrder As String
            Public Property Auditor As String
            Public Property DataNo As String
            Public Property WO_Pieces As Integer
            Public Property AQL_Pieces As Double
            Public Property DefectRate As Double
            Public Property Rejected As Integer
            Public Property Rejected_Percent As Double
            Public Property RejectLimiter As Integer
        End Class
        Public Class MainResultsGraph
            Public Property DText As String
            Public Property ThisRange As Integer
        End Class
        Public Class DefectImageDisplay
            Public Property DefectID As Integer
            Public Property InspectionJobSummaryId As Integer
            Public Property DefectTime As DateTime
            Public Property DefectDesc As String
            Public Property DataNo As String
            Public Property Linetype As String
            Public Property CID As String
            Public Property WorkOrder As String
            Public Property Location As String
            Public Property DefectImage_Filename As String
        End Class
        Public Class DefectReturnArray
            Public Property DefectId As Integer
            Public Property InspectionJobSummary As Integer
            Public Property DHU As Decimal
            Public Property DefectType As String
        End Class
        Public Class SPCMachineCheck
            Public Property CurrentWO As String
            Public Property IsNewWO As Boolean
        End Class

        Public Class WorkOrderCompliance
            Public Property WorkOrder As String
            Public Property Description As String
            Public Property Started As Integer
            Public Property StartedDate As DateTime
            Public Property DataNo As String
            Public Property LineType As String
            Public Property Branch As String
            Public Property Status As Integer
            Public Property Quantity As Decimal
            Public Property WorkOrder_Inspected As String
            Public Property InspectedCID As String
            Public Property Inspection_Started As DateTime
            Public Property TotalItemsInspected As Integer
            Public Property ItemFailCount As Integer
            Public Property DHUAVG As Decimal
            Public Property Updated As Boolean
            Public Property Match As Boolean
            Public Property Inline As Boolean
            Public Property Final As Boolean
        End Class
        Public Class InspectionCompliance
            Public Property id As Integer
            Public Property WADOCO As String
            Public Property WALITM As String
            Public Property WADL01 As String
            Public Property WAMCU As String
            Public Property WAUPMJ As String
            Public Property WADCTO As String
            Public Property WASRST As Object
            Public Property APR_MATCH As Boolean
            Public Property LineType As String
        End Class
        Public Class InspectionCompliance_Local
            Public Property Id As Integer
            Public Property WorkOrder As String
            Public Property DataNo As String
            Public Property WADL01 As String
            Public Property Location As String
            Public Property WADCTO As String
            Public Property WASRST As String
            Public Property ijsid As Object
            Public Property LineType As String
        End Class
        Public Class InspectionItemInfo
            Public Property Description As String
            Public Property IMPRP1 As String
            Public Property Type As String
        End Class
        Public Class InteriorSpecs
            Public Property Description As String
            Public Property WBDOCO As String
        End Class
        Public Class InspectionScatterPlot
            Public Property DATEVAL As String
            Public Property DHU_1 As Double
            Public Property DHU_2 As Double
            Public Property DHU_3 As Double
            Public Property DHU_4 As Double
        End Class
        Public Class LocationLineChart
            Public Property DATEVAL As String
            Public Property LOC_1 As Double
            Public Property LOC_2 As Double
            Public Property LOC_3 As Double
            Public Property LOC_4 As Double
            Public Property LOC_5 As Double
            Public Property LOC_6 As Double
            Public Property LOC_7 As Double
            Public Property LOC_8 As Double
            Public Property LOC_9 As Double
            Public Property LOC_10 As Double
            Public Property LOC_11 As Double
            Public Property LOC_12 As Double
            Public Property LOC_13 As Double
            Public Property LOC_14 As Double
            Public Property LOC_15 As Double
            Public Property LOC_16 As Double
            Public Property LOC_17 As Double
            Public Property LOC_18 As Double
            Public Property LOC_19 As Double
            Public Property LOC_20 As Double
            Public Property LOC_21 As Double
            Public Property LOC_22 As Double
            Public Property LOC_23 As Double
            Public Property LOC_24 As Double
            Public Property LOC_25 As Double
            Public Property LOC_26 As Double
            Public Property LOC_27 As Double
            Public Property LOC_28 As Double
            Public Property LOC_29 As Double
            Public Property LOC_30 As Double
            Public Property LOC_31 As Double
            Public Property LOC_32 As Double
            Public Property LOC_33 As Double
            Public Property LOC_34 As Double
            Public Property LOC_35 As Double
            Public Property LOC_36 As Double
            Public Property LOC_37 As Double
            Public Property LOC_38 As Double
            Public Property LOC_39 As Double
            Public Property LOC_40 As Double
            Public Property LOC_41 As Double
            Public Property LOC_42 As Double
            Public Property LOC_43 As Double
            Public Property LOC_44 As Double
            Public Property LOC_45 As Double
            Public Property LOC_46 As Double
            Public Property LOC_47 As Double
            Public Property LOC_48 As Double
            Public Property LOC_49 As Double
            Public Property LOC_50 As Double
        End Class

        Public Class OverTable
            Public Property Type As String
            Public Property ytd As Double
            Public Property mtd As Double
            Public Property F_ytd As Double
            Public Property F_mtd As Double
        End Class

        Public Class ExInspectionReturn
            Public Property id As Integer
            Public Property AQL_Level As String
            Public Property RejectLimiter As Integer
            Public Property WOQuantity As Integer
            Public Property SampleSize As Integer
            Public Property ItemFailCount As Integer
            Public Property Standard As String
            Public Property LineType As String
        End Class

        Public Class LinkedServerAlias
            Public Property LocationId As Integer
            Public Property Abreviation As String
            Public Property CID As String
            Public Property DSN_Identifier As String
        End Class

        Public Class InspectionTypes
            Public Property id As Integer
            Public Property Name As String
            Public Property Abreviation As String
            Public Property Description As String
        End Class

        Public Class LineChart1
            Public Property DATEBEGIN As String
            Public Property Major As Integer
            Public Property Minor As Integer
            Public Property Repairs As Integer
            Public Property Scraps As Integer
        End Class

        Public Class UnitCost
            Public Property UNITCOST As Decimal
            Public Property BRANCH As String
        End Class

        Public Class TemplateManger_LocSubgrid
            Public Property LocationMaster_id As Integer
            Public Property Abr As String
            Public Property Name As String
            Public Property LiveStatus As Boolean
        End Class

        Public Class UserInputs
            Public Property MainContent_WorkOrder As String
            Public Property MainContent_workroom As String
            Public Property MainContent_CPNumber As String
            Public Property MainContent_DataNumber As String
            Public Property MainContent_AuditorName As String
            Public Property MainContent_Location As String
            Public Property WOQuantity As String
            Public Property MainContent_RollNumber As String
            Public Property MainContent_LoomNumber As String
            Public Property MainContent_Inspector As String
        End Class
    End Namespace

    Namespace Production

        Public Class HourlyProductionSTT
            Public Shared FuncPropList As New Dictionary(Of String, String)

            Public Property HourID As Integer
            Public Property Machine As String
            Public Property HourBegin As DateTime
            Public Property ProductCount As Integer
            Public Property OverLengthInches As Decimal
            Public Property HourlyYds As Decimal
            Public Property CutLengthSpec As Decimal
            Public Property RunTime As Decimal
            Public Property DownTime As Decimal
            Public Property WorkOrderID As String
        End Class

        Public Class OperatorProduction
            Public Shared FuncPropList As New Dictionary(Of String, String)

            Public Property OperatorID As Integer
            Public Property Machine As String
            Public Property OperatorNo As String
            Public Property StartTime As DateTime
            Public Property EndTime As DateTime
            Public Property ScheduledTime As Decimal
            Public Property RunTime As Decimal
            Public Property DownTime As Decimal
            Public Property TotalYds As Decimal
            Public Property TotalSheets As Integer
            Public Property Efficiency As Decimal
            Public Property AvgSheetsPerMin As Decimal
            Public Property AvgYdsPerMin As Decimal
            Public Property OverLengthInches As Decimal

        End Class

        Public Class HP_ChartRangeSTT
            Public Property HOURBEGIN As DateTime
            Public Property STT_TEXPA2 As Decimal
            Public Property STT_TEXPA1 As Decimal
            Public Property STT_TEXPA3 As Decimal
            Public Property STT_PILLOW1 As Decimal
            Public Property STT_PILLOW2 As Decimal
            Public Property STT_AKAB2 As Decimal
            Public Property STT_AKAB1 As Decimal
        End Class
        Public Class HP_ChartRangeCAR
            Public Property HOURBEGIN As DateTime
            Public Property CAR_TENTER1 As Decimal
            Public Property CAR_TENTER2 As Decimal
            Public Property CAR_PREPEXIT As Decimal
            Public Property CAR_PREPENTRY As Decimal
        End Class
        Public Class HP_ChartRangeALL
            Public Property HOURBEGIN As DateTime
            Public Property CAR_TENTER1 As Decimal
            Public Property CAR_TENTER2 As Decimal
            Public Property CAR_PREPEXIT As Decimal
            Public Property CAR_PREPENTRY As Decimal
            Public Property STT_TEXPA2 As Decimal
            Public Property STT_TEXPA1 As Decimal
            Public Property STT_TEXPA3 As Decimal
            Public Property STT_PILLOW1 As Decimal
            Public Property STT_PILLOW2 As Decimal
            Public Property STT_AKAB2 As Decimal
            Public Property STT_AKAB1 As Decimal
        End Class
        Public Class HP_ChartRangeCAR_ST
            Public Property HOURBEGIN As String
            Public Property CAR_TENTER1 As Decimal
            Public Property CAR_TENTER2 As Decimal
            Public Property CAR_PREPEXIT As Decimal
            Public Property CAR_PREPENTRY As Decimal
        End Class

        Public Class HP_ChartRangeSTT_ST
            Public Property HOURBEGIN As String
            Public Property STT_TEXPA1 As Decimal
            Public Property STT_TEXPA3 As Decimal
            Public Property STT_TEXPA2 As Decimal
            Public Property STT_PILLOW1 As Decimal
            Public Property STT_PILLOW2 As Decimal
            Public Property STT_AKAB2 As Decimal
            Public Property STT_AKAB1 As Decimal
        End Class
        Public Class HP_ChartRangeALL_ST
            Public Property HOURBEGIN As String
            Public Property STT_TEXPA1 As Decimal
            Public Property STT_TEXPA3 As Decimal
            Public Property STT_TEXPA2 As Decimal
            Public Property STT_PILLOW1 As Decimal
            Public Property STT_PILLOW2 As Decimal
            Public Property STT_AKAB2 As Decimal
            Public Property STT_AKAB1 As Decimal
            Public Property CAR_TENTER1 As Decimal
            Public Property CAR_TENTER2 As Decimal
            Public Property CAR_PREPEXIT As Decimal
            Public Property CAR_PREPENTRY As Decimal
        End Class

        Public Class WorkOrderProductionSTT
            Public Shared FuncPropList As New Dictionary(Of String, String)

            Public Property ID As Integer
            Public Property Machine As String
            Public Property WorkOrder As String
            Public Property OperatorNo As String
            Public Property StartTime As DateTime
            Public Property FinishTime As DateTime
            Public Property DataNo As String
            Public Property GreigeNo As String
            Public Property CutLengthSpec As String
            Public Property JobYds As Decimal
            Public Property JobSheets As Integer
            Public Property JobOverLengthInches As Decimal
            Public Property ScheduledTime As Decimal
            Public Property DownTime As Decimal
            Public Property RunTime As Decimal
            Public Property AvgSheetsPerHour As Decimal
            Public Property JDECOMP As Integer
            Public Property JDESCRAP As Integer
            Public Property JDETOTREC As Integer
            Public Property DIFF_PERC As Decimal


        End Class

        Public Class RollProductionSTT
            Public Shared FuncPropList As New Dictionary(Of String, String)

            Public Property RollProductionID As Integer
            Public Property Machine As String
            Public Property OperatorNo As String
            Public Property StartTime As DateTime
            Public Property EndTime As DateTime
            Public Property TotalYds As Decimal
            Public Property TotalSheets As Integer
            Public Property TicketYds As Decimal
            Public Property TicketOverYds As Decimal
            Public Property RollNo As String
            Public Property JobNo As String
            Public Property DataNo As String
            Public Property GreigeNo As String
            Public Property TimeStamp_Trans As DateTime
        End Class
        Public Class JobSummaryMAIN
            Public Property RowID As Integer
            Public Property Machine As String
            Public Property WorkOrderID As String
            Public Property HourBegin As DateTime
            Public Property ProductCount As Integer
            Public Property OverLengthInches As Decimal
            Public Property CutLengthSpec As Decimal
            Public Property AvgYdsPmin As Decimal
            Public Property HourlyYds As Decimal
            Public Property RunTime As Decimal
            Public Property DownTime As Decimal
            Public Property DefPerProd As Double
        End Class

        Public Class JobSummary_DBreakdown
            Public Property RowID As Integer
            Public Property Machine As String
            Public Property WorkOrderID As String
            Public Property DTYPE As String
            Public Property BSPCId As Integer
            Public Property DEFSUM As Integer
        End Class

        
    End Namespace
    Public Class BarChartObject
        Public Property RowID As Integer
        Public Property RowDesc As String
        Public Property Col1 As Object
        Public Property Col2 As Object
        Public Property Col3 As Object
        Public Property Col4 As Object
        Public Property Col5 As Object
        Public Property Col6 As Object
        Public Property Col7 As Object
        Public Property Col8 As Object
        Public Property Col9 As Object
        Public Property Col10 As Object
        Public Property Col11 As Object
        Public Property Col12 As Object
        Public Property Col13 As Object
        Public Property Col14 As Object
        Public Property Col15 As Object
        Public Property Col16 As Object
        Public Property Col17 As Object
        Public Property Col18 As Object
        Public Property Col19 As Object
        Public Property Col20 As Object
    End Class
    Public Class InfoSchema
        Public Property COLUMN_NAME As String
    End Class
    Public Class SingleObject
        Public Property Object1 As Object
        Public Property Object2 As Integer
        Public Property Object3 As Object
    End Class
    Public Class jqgridFilterList
        Public Property Col1Name As String
        Public Property Col1 As Array
        Public Property selectedVal1 As Object
        Public Property Col2Name As String
        Public Property Col2 As Array
        Public Property selectedVal2 As Object
        Public Property Col3Name As String
        Public Property Col3 As Array
        Public Property selectedVal3 As Object
        Public Property Col4Name As String
        Public Property Col4 As Array
        Public Property selectedVal4 As Object
        Public Property Col5Name As String
        Public Property Col5 As Array
        Public Property selectedVal5 As Object
        Public Property Col6Name As String
        Public Property Col6 As Array
        Public Property selectedVal6 As Object
        Public Property Col7Name As String
        Public Property Col7 As Array
        Public Property selectedVal7 As Object
    End Class
    Public Class Locationarray
        Public Property id As Integer
        Public Property text As String
        Public Property ProdAbreviation As String
        Public Property Abreviation As String
        Public Property CID As String
    End Class
    Public Class ActiveLocations
        Public Property value As String
        Public Property status As String
        Public Property ProdAbreviation As String
        Public Property CID As String
    End Class
    Public Class ActiveFilterObject
        Public Property id As Integer
        Public Property Name As String
        Public Property value As Object
    End Class
    Public Class selector2array
        Public Property id As Integer
        Public Property text As String
    End Class
    Public Class selectorobject
        Public Property id As String
        Public Property text As String
    End Class
    Public Class PieChart
        Public Property label As String
        Public Property value As Decimal
        Public Property color As String
    End Class
    Public Class PieChartdata
        Public Property value As Decimal
        Public Property id As Integer
        Public Property desc As String
    End Class

    Public Class InputArray
        Public Property value As Object
        Public Property key As String
    End Class

    Public Class DefectValues
        Public Property DefectTime As DateTime
        Public Property DefectDesc As String
        Public Property POnumber As String
        Public Property DataNo As String
        Public Property EmployeeNo As String
        Public Property AQL As String
        Public Property ThisPieceNo As String
        Public Property SampleSize As String
        Public Property TotalLotPieces As String
        Public Property Product As String
        Public Property DefectClass As String
        Public Property MergeDate As String
        Public Property Tablet As String
        Public Property WorkOrder As String
        Public Property LotNo As String
        Public Property Location As String
        Public Property DataType As String
        Public Property Dimensions As String
        Public Property Comment As String
        Public Property LoomNo As String
        Public Property DefectPoints As String
        Public Property GreigeNo As String
        Public Property RollNo As String
        Public Property Operation As String

    End Class

    Public Class NavigationPermissions
        Public Property APRPM_Enabled As Boolean
        Public Property APRUtility_Enabled As Boolean
        Public Property APRLoom_Enabled As Boolean
        Public Property APRInspection_Enabled As Boolean
        Public Property APRSPC_Enabled As Boolean
    End Class

    Public Class CarouselImage
        Public Property imageUrl As String
        Public Property linkUrl As String
        Public Property content As String
        Public Property caption As String
        Public Property DataNo As String
        Public Property AuditType As String
        Public Property DefectDesc As String
        Public Property LocationCID As String
        Public Property prpcode As String
    End Class
    Public Class Emails
        Public Property Address As String
        Public Property SPC_CAR_RPT As Boolean
        Public Property MCS_CAR_RPT As Boolean
        Public Property SPC_STT_RPT As Boolean
        Public Property INS_ALERT_EMAIL As Boolean
        Public Property SPEC_ALERT_EMAIL As Boolean
        Public Property ADMIN As Boolean
        Public Property HomeLocation As String
    End Class

    Public Class ApplicationLog
        Public Property date_added As DateTime
        Public Property type As String
        Public Property Target As String
        Public Property Message As String
        Public Property application_name As String
    End Class
    Public Class DayCache
        Public Property UnitDate As DateTime
        Public Property ListObj As Object
    End Class
    Public Class jsonData
        Public Property total As Object
        Public Property page As Object
        Public Property records As Object
        Public Property userdata As Object
        Public Property rows As Object
    End Class
    Public Class SubgridjsonData
        Public Property cell As Object
        Public Property repeatitems As Object
        Public Property rows As Object
        Public Property id As Object
    End Class
End Namespace
