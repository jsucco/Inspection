<%@ WebHandler Language="VB" Class="core.SPC_InspectionInput" %>

Imports System
Imports System.Web
Imports App.Utilities.Web.Handlers
Imports System.Web.Script.Serialization
Imports System.Data.SqlClient
Imports System.Data
Imports System.Globalization
Imports System.Data.Entity
Imports System.Object
Imports System.Data.Objects
Imports System.Threading
Imports System.Threading.Tasks


Namespace core

    Public Class SPC_InspectionInput
        Inherits BaseHandler

        Private Property DA As New InspectionInputDAO
        Private IU As New InspectionUtilityDAO

        Private Property as400 As New AS400DAO
        Private Property DL As New dlayer
        Private Util As New Utilities
        Private bmapps As New BMappers(Of SPCInspection.InspectProductSpec)
        Dim jser As New JavaScriptSerializer()
        Dim dictionary As New Dictionary(Of String, String)
        Dim RollNumber As String = ""
        Dim ItemNumber As String = ""
        Dim LoomNumber As Integer = 0

        Public Function InsertDefect(ByVal id As String, ByVal text As String, ByVal JsonString As String, ByVal ButtonTemplateId As String, ByVal InspectionJobSummaryId As Integer, ByVal InspectionId As Integer, ByVal WeaverShiftIdVal As Integer) As String

            Dim DefectArray As New List(Of SPCInspection.DefectMaster)()

            Dim inputelementarray As New List(Of InputArray)
            Dim returnint As Integer

            Dim RejectionCount As Integer = 0
            Dim InspectionState As Integer = 0
            Dim TargetOrder As String = "WorkOrder"
            Dim InspectionJobSummaryIdret As Int64 = 0
            Dim TargetNumber As String = ""
            Dim TargetState As String = ""
            Dim DefectType As String = "MINOR"
            Dim DHU As Decimal = 0
            Dim listret As New List(Of SPCInspection.DefectReturnArray)
            Dim bmapis As New BMappers(Of SPCInspection.InspectionJobSummary)
            Dim bmapso As New BMappers(Of SingleObject)
            Dim jsobj As New SPCInspection.InspectionJobSummary
            Dim EmployeeNo As String = ""
            Try
                inputelementarray = jser.Deserialize(Of List(Of InputArray))(JsonString)
            Catch ex As Exception
                Throw New Exception("Error filling inputelementarray: " + ex.Message)
                Return "-1"
                Exit Function
            End Try

            GetInputVariblesAsDictionary(inputelementarray)

            If dictionary.Count > 0 Then

                Try

                    If IsNumeric(ButtonTemplateId) = False Then
                        ButtonTemplateId = "0"
                    Else

                        Dim defecttype_obj As New List(Of SingleObject)

                        Try
                            defecttype_obj = GetDefectType(ButtonTemplateId)
                            If defecttype_obj.Count > 0 Then
                                'If defecttype_obj.ToArray()(0).Object1 <> "" Then
                                If defecttype_obj.ToArray()(0).Object1 = "0" Or defecttype_obj.ToArray()(0).Object1 = "False" Then
                                    DefectType = "MINOR"
                                ElseIf defecttype_obj.ToArray()(0).Object1 = "1" Or defecttype_obj.ToArray()(0).Object1 = "True" Then
                                    DefectType = "MAJOR"
                                Else
                                    DefectType = defecttype_obj.ToArray()(0).Object1.ToString.ToUpper()
                                End If
                                'End If
                            End If
                        Catch ex As Exception

                        End Try

                    End If

                    Select Case dictionary.Item("MainContent_InspectionState")
                        Case "WorkOrder"
                            TargetNumber = dictionary.Item("MainContent_WorkOrder")
                            TargetState = "WorkOrder"
                            EmployeeNo = Convert.ToString(dictionary.Item("Auditor_Name"))
                        Case "WORKORDER"
                            TargetNumber = dictionary.Item("MainContent_WorkOrder")
                            TargetState = "WorkOrder"
                            EmployeeNo = Convert.ToString(dictionary.Item("Auditor_Name"))
                        Case "RollNumber"
                            TargetNumber = RollNumber
                            TargetState = "RollNumber"
                            EmployeeNo = Convert.ToString(dictionary.Item("MainContent_Inspector"))
                        Case "ItemNumber"
                            TargetNumber = ItemNumber
                            TargetState = "ItemNumber"
                    End Select

                    InspectionJobSummaryIdret = InspectionJobSummaryId
                    ' End If

                    'If dictionary.Item("MainContent_InspectionId").Length > 0 And dictionary.Item("MainContent_InspectionId").IndexOf(".") > 0 Then
                    '    InspectionId = CType(dictionary.Item("MainContent_InspectionId").Split(".")(1), Integer)
                    'End If

                    DefectArray.Add(New SPCInspection.DefectMaster With {.DefectTime = Convert.ToDateTime(Now), .MergeDate = Convert.ToDateTime(Now), .DefectDesc = text, .POnumber = Convert.ToString(dictionary.Item("MainContent_CPNumber")), .DataNo = Convert.ToString(dictionary.Item("MainContent_DataNumber")), .EmployeeNo = EmployeeNo, .Product = Convert.ToString(dictionary.Item("Product")), .DefectClass = DefectType, .AQL = Convert.ToString(dictionary.Item("AQ_Level")), .ThisPieceNo = Convert.ToString(CType(dictionary.Item("MainContent_Good"), Long) + CType(dictionary.Item("MainContent_Bad_Local"), Long)), .TotalLotPieces = Convert.ToString(dictionary.Item("WOQuantity")), .SampleSize = Convert.ToString(dictionary.Item("MainContent_SampleSize")), .Tablet = "Browser", .WorkOrder = Convert.ToString(dictionary.Item("MainContent_WorkOrder")), .LotNo = Convert.ToString(dictionary.Item("MainContent_CartonNumber")), .Location = Convert.ToString(dictionary.Item("MainContent_Location")), .DataType = Convert.ToString("Defect"), .Dimensions = Convert.ToString(dictionary.Item("Dimensions")), .Comment = Convert.ToString(dictionary.Item("Comment")), .TemplateId = Convert.ToInt64(dictionary.Item("TemplateId")), .InspectionId = InspectionId, .RollNumber = RollNumber, .LoomNumber = LoomNumber, .ButtonTemplateId = Convert.ToInt64(ButtonTemplateId), .Inspector = dictionary.Item("MainContent_Inspector"), .ItemNumber = ItemNumber, .InspectionState = Convert.ToString(dictionary.Item("MainContent_InspectionState")), .WorkRoom = Convert.ToString(dictionary.Item("MainContent_workroom")), .RejectLimiter = Convert.ToInt64(dictionary.Item("MainContent_RE")), .InspectionJobSummaryId = InspectionJobSummaryIdret, .WeaverShiftId = WeaverShiftIdVal})


                Catch ex As Exception
                    Throw New Exception("Error filling DefectArray: " + ex.Message)
                    Return "-1"
                    Exit Function
                End Try
                If Convert.ToInt32(dictionary.Item("TemplateId")) > 0 And InspectionJobSummaryIdret > 0 And DefectArray.Count > 0 Then
                    returnint = DA.InsertDefects(DefectArray)
                Else
                    Throw New Exception("TemplateId, InspectionJobSummaryId or DefectArray count cannot be zero")
                    Return "-1"
                    Exit Function
                End If

                If returnint > 0 Then
                    Try
                        Dim sqlis As String = "UPDATE InspectionJobSummary SET ItemFailCount = @ItemFailCount WHERE (id = " & InspectionJobSummaryIdret.ToString() & ")"
                        Dim retupdate As Boolean = False
                        Dim objis As New SPCInspection.InspectionJobSummary
                        Dim AllTypesDefectCount As Integer = 0
                        Dim listso As New List(Of SingleObject)
                        Dim TotalInspected As Integer
                        objis.ItemFailCount = CType(dictionary.Item("MainContent_Bad_Group"), Integer) + 1

                        retupdate = bmapis.InsertSpcObject(sqlis, objis)

                        'sqlis = "SELECT COUNT(DefectID) AS Object1 FROM DefectMaster  INNER JOIN ButtonTemplate ON DefectMaster.ButtonTemplateId = ButtonTemplate.id WHERE DefectMaster." & TargetState & " = '" & TargetNumber & "' AND ButtonTemplate.DefectType = '1'"
                        'listso = bmapso.GetInspectObject(sqlis)
                        If TargetState = "WorkOrder" Then
                            'AllTypesDefectCount = listso.ToArray()(0).Object1
                            'sqlis = "SELECT DISTINCT InspectionJobSummaryId AS Object1, CAST(SampleSize AS int) AS Object2 FROM DefectMaster WHERE (" & TargetState & " = '" & TargetNumber & "')"
                            'listso.Clear()
                            'listso = bmapso.GetInspectObject(sqlis)
                            'If listso.Count > 0 Then
                            '    Dim listso1 As New List(Of SingleObject)
                            '    Dim TotalItemsInspected1 = Aggregate v In listso Into Sum(v.Object2)
                            TotalInspected = CType(dictionary.Item("MainContent_SampleSize"), Integer)
                            If TotalInspected > 0 Then
                                DHU = DA.CalculateDHU(TargetState, TargetNumber, InspectionJobSummaryIdret, TotalInspected)
                            End If
                            'End If
                        End If


                    Catch ex As Exception
                        Throw New Exception("Error updating InspectionJobSummary Table: " + ex.Message)
                        Return "-1"
                        Exit Function
                    End Try

                End If

                listret.Add(New SPCInspection.DefectReturnArray With {.DefectId = returnint, .InspectionJobSummary = InspectionJobSummaryIdret, .DHU = DHU, .DefectType = DefectType})
                Return jser.Serialize(listret)

            Else
                Throw New Exception("Error converting inputarray to dictionary")
                Return "-1"
                Exit Function
            End If
        End Function

        Private Function GetDefectType(ByVal ButtonTemplateId As Integer) As Object
            Dim bmap_dt As New BMappers(Of SingleObject)
            Dim defecttype_obj As New List(Of SingleObject)
            defecttype_obj = bmap_dt.GetInspectObject("SELECT DefectType AS Object1 FROM  ButtonTemplate WHERE  (id = " & ButtonTemplateId & ")")
            If defecttype_obj.Count > 0 Then
                Return defecttype_obj
            Else
                Return True
            End If

        End Function
        Public Function InsertProductSpec(ByVal InputAr As String, ByVal SessionId As String) As String
            Dim retstring As String = "FAIL"


            If InputAr <> "" And IsNothing(InputAr) = False Then
                Dim InspectList = jser.Deserialize(Of List(Of SPCInspection.InspectProductSpec))(InputAr)
                Dim sqlstring As String = ""
                Dim bmappom As New BMappers(Of SingleObject)
                Dim listpom As New List(Of SingleObject)
                Dim InObj As New SPCInspection.InspectProductSpec
                listpom = bmappom.GetInspectObject("SELECT TOP (1) POM_Row AS Object1 FROM ProductSpecification WHERE (DataNo = '" & InspectList.ToArray()(0).DataNo & "') AND (NOT (POM_Row IS NULL)) ORDER BY POM_Row DESC")

                If listpom.Count = 0 Then
                    InObj.POM_Row = 1
                Else
                    InObj.POM_Row = listpom.ToArray()(0).Object1 + 1
                End If

                InObj.ProductType = InspectList.ToArray()(0).ProductType
                InObj.Spec_Description = InspectList.ToArray()(0).Spec_Description
                InObj.value = InspectList.ToArray()(0).value
                InObj.Upper_Spec_Value = InspectList.ToArray()(0).Upper_Spec_Value
                InObj.Lower_Spec_Value = InspectList.ToArray()(0).Lower_Spec_Value
                InObj.DataNo = InspectList.ToArray()(0).DataNo

                Dim sqlstr As String

                Dim listps As New List(Of SPCInspection.InspectProductSpec)
                Dim ret As Boolean = False

                sqlstr = "INSERT INTO ProductSpecification (TabTemplateId, DataNo, ProductType, Spec_Description, value, Upper_Spec_Value, Lower_Spec_Value, GlobalSpec, ME_SessionID, SpecSource)" & vbCrLf &
                                "VALUES (0,@DataNo,@ProductType,@Spec_Description,@value,@Upper_Spec_Value,@Lower_Spec_Value, 1, '" & SessionId.Trim() & "', 'user')"
                ret = bmapps.InsertSpcObject_RetNum(sqlstr, InObj, True)

                If ret = True Then
                    retstring = "success"
                    Dim listem As New List(Of Emails)
                    Dim bmapem As New BMappers(Of Emails)
                    Dim emaillist As New List(Of Emails)
                    ' Dim body As String = "[DataNo: " & InObj.DataNo & "][SpecId: " & bmapps.RowReturnIdentity.ToString() & "][ ProductType: " & InObj.ProductType & "][SpecDescription: " & InObj.Spec_Description & "]<br /><br />  Spec is currently local to the user who created it.  To make it global go to the below link. <br /><br /> <a href=""http://coreroute_test.standardtextile.com/APP/DataEntry/SPCInspectionUtility.aspx"">coreroute_test.standardtextile.com/APP/DataEntry/SPCInspectionUtility.aspx</a>"
                    Dim body As String = "[DataNo: " & InObj.DataNo & "][SpecId: " & bmapps.RowReturnIdentity.ToString() & "][ ProductType: " & InObj.ProductType & "][SpecDescription: " & InObj.Spec_Description & "]<br /><br />  Spec is currently Global to All users.  To hide it globally go to the below link. <br /><br /> <a href=""http://apr.standardtextile.com/APP/DataEntry/SPCInspectionUtility.aspx"">apr.standardtextile.com/APP/DataEntry/SPCInspectionUtility.aspx</a>"

                    Dim subject As String = "PRODUCT SPEC ENTERED BY USER (DN:" & InObj.DataNo & ")"
                    emaillist = bmapem.GetAprMangObject("SELECT Address, SPEC_ALERT_EMAIL, ADMIN, HomeLocation FROM EmailMaster")
                    'emaillist.Add(New Emails With {.Address = "jsucco@standardtextile.com", .SPEC_ALERT_EMAIL = True})
                    'emaillist.Add(New Emails With {.Address = "kbredwell@standardtextile.com", .SPEC_ALERT_EMAIL = True})
                    listem = (From v In emaillist Where v.SPEC_ALERT_EMAIL = True Select v).ToList()

                    Util.SendMail(subject, body, listem)

                End If

            End If


            Return retstring
        End Function

        Public Function ToggleTimerStatus(ByVal InspectionJobSummaryId As Integer, ByVal ButtonTemplateId As Integer, ByVal StatusValue As String, ByVal DefectID As Integer, ByVal SessionId As String, ByVal ButtonLocationId As Integer, ByVal TimerId As Integer) As String
            Dim retobj As String = "false"

            Try
                If InspectionJobSummaryId > 0 And ButtonTemplateId > 0 Then
                    Using _db As New Inspection_Entities
                        If StatusValue = "START" Then


                            Dim bmap As New BMappers(Of SPCInspection.DefectTimer)
                            Dim newobj As New DefectTimer
                            Dim resultobj As New Object

                            'Dim dtobj As New SPCInspection.DefectTimer
                            newobj.InspectionJobSummaryId = InspectionJobSummaryId
                            newobj.ButtonTemplateId = ButtonTemplateId
                            newobj.StatusValue = StatusValue
                            newobj.DefectID = DefectID
                            newobj.SessionId = SessionId
                            newobj.ButtonLocationId = ButtonLocationId
                            newobj.Timestamp = DateTime.Now


                            _db.DefectTimers.Add(newobj)
                            resultobj = _db.SaveChanges()
                            If resultobj > 0 Then
                                Dim newretobj As Object = New With {.TimerId = newobj.id.ToString(), .StartTime = DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString()}

                                retobj = jser.Serialize(newretobj)
                            End If
                        ElseIf StatusValue = "STOP" Then
                            Dim exobj = (From v In _db.DefectTimers Where v.id = TimerId And v.StopTimestamp = Nothing Select v).First()

                            If IsNothing(exobj) = False Then
                                If exobj.InspectionJobSummaryId > 0 Then
                                    exobj.StopTimestamp = DateTime.Now
                                    Dim resobj = _db.SaveChanges()
                                    If resobj > 0 Then
                                        retobj = "true"
                                    End If
                                End If
                            End If
                        End If
                    End Using

                    'dtobj.Timestamp = DateTime.Now

                    'Dim resultobj = bmap.InsertSpcObject("INSERT INTO DefectTimer (InspectionJobSummaryId, ButtonTemplateId, StatusValue, Timestamp) VALUES (@InspectionJobSummaryId, @ButtonTemplateId, @StatusValue, @Timestamp )", dtobj)


                End If
            Catch ex As Exception
                Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
            End Try


            Return retobj
        End Function

        Public Function GetOpenTimers(ByVal InspectionJobSummaryId As Integer, ByVal SessionId As String) As String
            Dim retobj As String = ""
            Dim otlist As New List(Of DefectTimer)
            Dim retlist As New List(Of DefectTimer)
            Using _db As New Inspection_Entities
                Dim retobjtest = (From v In _db.DefectTimers Where v.InspectionJobSummaryId = InspectionJobSummaryId And v.SessionId = SessionId And v.StopTimestamp = Nothing Select New With {.InspectionJobSummaryId = v.InspectionJobSummaryId, .SessionId = v.SessionId, .ButtonLocationId = v.ButtonLocationId, .TimerId = v.id, .TimerStart = v.Timestamp}).Distinct().ToList()
                Dim retobjtest1 = (From v In retobjtest Select New With {.InspectionJobSummaryId = v.InspectionJobSummaryId, .SessionId = v.SessionId, .ButtonLocationId = v.ButtonLocationId, .TimerId = v.TimerId, .TimerStart = v.TimerStart.ToLongDateString() + " " + v.TimerStart.ToLongTimeString}).ToList()
                'retlist = (From v In _db.DefectTimers Where v.InspectionJobSummaryId = InspectionJobSummaryId And v.SessionId = SessionId And v.StopTimestamp = Nothing Select New SPCInspection.DefectTimers With {.ButtonLocationId = v.ButtonLocationId, .ButtonTemplateId = v.ButtonTemplateId, .DefectID = v.DefectID, .InspectionJobSummaryId = v.InspectionJobSummaryId, .SessionId = v.SessionId}).ToList()


                retobj = jser.Serialize(retobjtest1)
            End Using

            Return retobj

        End Function
        Public Function GetInspectionId() As Integer

            Return DA.GetTemplateId()
        End Function
        Public Function GetCurrentSPCMachineWorkOrder(ByVal ijsid As Integer, ByVal MachineName As String) As String
            Dim bmapso As New BMappers(Of SingleObject)
            Dim listso As List(Of SingleObject)
            Dim jser As New JavaScriptSerializer

            listso = bmapso.GetInspectObject("SELECT InspectionSummaryId AS Object2 FROM LiveProduction WHERE Machine = '" & MachineName & "'")

            If listso.Count > 0 Then
                If listso.ToArray()(0).Object2 <> ijsid And listso.ToArray()(0).Object2 <> 0 Then
                    Dim bmapijs As New BMappers(Of SPCInspection.InspectionJobSummary)
                    Dim listis As New List(Of SPCInspection.InspectionJobSummary)

                    listis = bmapijs.GetInspectObject("SELECT id, JobNumber, DataNo, SampleSize, RejectLimiter, WOQuantity FROM InspectionJobSummary WHERE id = " & listso.ToArray()(0).Object2 & "")

                    Return jser.Serialize(listis)
                End If
            End If

            Return "[0]"

        End Function
        Public Function LoadOpenWorkOrders(ByVal InspectionState As String, ByVal PassCID As String) As String
            Dim bmapis As New BMappers(Of SPCInspection.InspectionJobSummary)
            Dim listis As New List(Of SPCInspection.InspectionJobSummary)
            Dim listret As New List(Of SPCInspection.InspectionJobSummary)
            Dim listspcret As New List(Of SPCInspection.InspectionJobSummary)
            Dim jser As New JavaScriptSerializer()
            Dim DefaultId As Integer = 1
            Dim sql As String = "SELECT ijs.id, ijs.JobNumber, ijs.ItemFailCount, ijs.TemplateId, tn.Name, ijs.AQL_Level, ijs.Standard, ijs.SampleSize, ijs.RejectLimiter, convert(varchar(25), ijs.Inspection_Started, 100) as Inspection_StartedString FROM InspectionJobSummary ijs LEFT OUTER JOIN TemplateName tn ON ijs.TemplateId = tn.TemplateId " & vbCrLf &
                                    "WHERE (Technical_PassFail IS NULL) AND (JobType = '" & InspectionState & "') AND (Inspection_Started >= GETDATE() - 3) AND (LEN(JobNumber) > 3) AND (CID = '" & PassCID & "')"
            listis = bmapis.GetInspectObject(sql)

            If listis.Count > 0 Then
                Dim listar = listis.ToArray()
                DefaultId = listar(listar.Length - 1).id
                listspcret = LoadOpenSPCMachine(PassCID)
                For Each item In listspcret
                    listis.Add(item)
                Next
                'listis.AddRange(LoadClosedInline(PassCID))
                listis.Add(New SPCInspection.InspectionJobSummary With {.id = 1000, .JobNumber = "SELECT OPTION"})
                listret = (From x In listis Select x Order By x.id Descending).ToList()
            Else
                listspcret = LoadOpenSPCMachine(PassCID)
                'listis.AddRange(LoadClosedInline(PassCID))
                For Each item In listspcret
                    listis.Add(item)
                Next
                listis.Add(New SPCInspection.InspectionJobSummary With {.id = 1000, .JobNumber = "SELECT OPTION"})
                listret = (From x In listis Select x Order By x.id Descending).ToList()
            End If

            Return jser.Serialize(listret)

        End Function

        Private Function LoadClosedInline(ByVal CID As String) As List(Of SPCInspection.InspectionJobSummary)
            Dim closedInline As New List(Of SPCInspection.InspectionJobSummary)
            Dim CutOff = DateTime.Now.AddDays(-7)
            Using _db As New Inspection_Entities
                Dim closed = (From x In _db.InspectionJobSummaries Join t In _db.TemplateNames On x.TemplateId Equals t.TemplateId Where x.Inspection_Started >= CutOff And Not x.Technical_PassFail = Nothing And t.LineType.ToUpper() = "IL" And x.CID = CID).ToList()
                closedInline = (From x In closed Select New SPCInspection.InspectionJobSummary With {.id = x.x.id, .JobNumber = x.x.JobNumber, .ItemFailCount = x.x.ItemFailCount, .TemplateId = x.x.TemplateId, .Name = x.t.Name, .AQL_Level = x.x.AQL_Level, .Standard = x.x.Standard, .SampleSize = x.x.SampleSize, .RejectLimiter = x.x.RejectLimiter, .Inspection_StartedString = x.x.Inspection_Started.ToString()}).ToList()
            End Using

            Return closedInline
        End Function
        Private Function LoadOpenSPCMachine(ByVal PassCID As String) As List(Of SPCInspection.InspectionJobSummary)
            Dim bmapis As New BMappers(Of SPCInspection.InspectionJobSummary)
            Dim listis As New List(Of SPCInspection.InspectionJobSummary)
            Dim bmapso As New BMappers(Of SingleObject)
            Dim listso As New List(Of SingleObject)
            Dim ijssql As String = ""
            Dim sql As String
            Dim SQL2 As String = "SELECT InspectionSummaryId AS Object1, Machine AS Object3 FROM LiveProduction WHERE InspectionSummaryId > 0 AND LocationCID = '" & PassCID & "'"
            listso = bmapso.GetInspectObject(SQL2)
            If listso.Count > 0 Then

                Dim cnt As Integer = 0
                For Each item In listso
                    Dim addsql As String = " id = " & item.Object1 & ""
                    If cnt < listso.Count - 1 Then
                        ijssql = ijssql + addsql + " OR "
                    Else
                        ijssql = ijssql + addsql
                    End If
                    cnt += 1
                Next
                sql = "SELECT ijs.id, ijs.JobNumber, ijs.ItemFailCount, ijs.TemplateId, tn.Name, ijs.AQL_Level, ijs.Standard, ijs.SampleSize, ijs.RejectLimiter, convert(varchar(25), ijs.Inspection_Started, 100) as Inspection_StartedString FROM InspectionJobSummary ijs LEFT OUTER JOIN TemplateName tn ON ijs.TemplateId = tn.TemplateId " & vbCrLf &
                                        "WHERE (ijs.JobNumber IS NOT NULL) AND ( " + ijssql + ")"
                listis = bmapis.GetInspectObject(sql)

                For Each itme In listis
                    Dim name As Object = (From v In listso Where v.Object1 = itme.id Select v.Object3).ToArray()
                    If Not IsNothing(name) Then
                        itme.ProdMachineName = name(0)
                        itme.IsSPC = True
                    End If
                Next
            End If
            Return listis

        End Function

        Public Function GetInspectionJobSummaryId2(ByVal TargetNumber As String, ByVal TemplateId As Integer) As String
            Dim TargetOrderCount As Int16 = DA.GetInspectionJobSummaryIdCount(TargetNumber, TemplateId)
            Dim sql As String = ""
            Dim returnid As Object
            If TargetOrderCount > 0 And TargetNumber <> "" And TemplateId > 0 Then
                Dim retobj As List(Of SPCInspection.ExInspectionReturn) = DA.GetOpenInspectionJobSummaryId2(TargetNumber, TemplateId)
                If retobj.Count = 0 Then
                    returnid = "-99"
                Else
                    If retobj.ToArray()(0).LineType = "IL" Then
                        OpenInspectionJob(retobj.ToArray()(0).id)
                    End If
                    returnid = jser.Serialize(retobj)
                End If
            Else
                returnid = "-99"
            End If

            Return returnid
        End Function

        Private Sub OpenInspectionJob(JobSummaryId As Integer)
            Using _db As New Inspection_Entities
                Dim InspectionJob As New InspectionJobSummary
                InspectionJob = (From o In _db.InspectionJobSummaries Where o.id = JobSummaryId Select o)
                If Not InspectionJob.Inspection_Finished Is Nothing Then
                    InspectionJob.TotalInspectedItems = Nothing
                    InspectionJob.Technical_PassFail = Nothing
                    InspectionJob.Technical_PassFail_Timestamp = Nothing
                    InspectionJob.UserConfirm_PassFail = Nothing
                    InspectionJob.UserConfirm_PassFail_Timestamp = Nothing
                    InspectionJob.Inspection_Finished = Nothing
                    _db.SaveChanges()
                End If
            End Using
        End Sub

        Public Function GetRedirect(ByVal InspectionId As Integer, ByVal InspectionStarted As String) As String
            Dim retstring As String = ""
            If InspectionId > 0 Then
                Dim bmapjs As New BMappers(Of SPCInspection.InspectionJobSummary)
                Dim listjs As New List(Of SPCInspection.InspectionJobSummary)
                Dim sql As String = "SELECT top(1) JobType, JobNumber, AQL_Level, Standard, TemplateId, Technical_PassFail FROM InspectionJobSummary WHERE (id = " & InspectionId.ToString() & ") AND (Technical_PassFail IS NULL)"
                listjs = bmapjs.GetInspectObject(sql)
                If listjs.Count > 0 And InspectionStarted = "True" Then
                    Dim querystring As String
                    Dim tid As String = ""
                    If listjs.ToArray()(0).TemplateId > 0 Then
                        tid = "TemplateId=" + listjs.ToArray()(0).TemplateId.ToString()
                    End If
                    If listjs.ToArray()(0).JobType.ToString() = "RollNumber" Then

                        querystring = "?" + tid + "&ijs=" + InspectionId.ToString() + "&400REQ=ROLLNUMBER&OPENRL=True"
                    Else
                        'querystring = "?WorkOrder=" + listjs.ToArray()(0).JobNumber.ToString() + "&AQL=" + listjs.ToArray()(0).AQL_Level.ToString() + "&standard=" + listjs.ToArray()(0).Standard.ToString() + "&" + tid + "&400REQ=OPENWO&OpenWO=True"
                        querystring = "?" + tid + "&ijs=" + InspectionId.ToString() + "&400REQ=OPENWO&OpenWO=True"
                    End If

                    retstring = "/APP/Mob/SPCInspectionInput.aspx" + querystring
                Else
                    retstring = "/APP/Mob/SPCInspectionInput.aspx"
                End If
            Else
                retstring = "/APP/Mob/SPCInspectionInput.aspx"
            End If

            Return retstring
        End Function
        Public Function CreateJobSummaryId(ByVal jobtype As String, ByVal AQLStandard As String, ByVal IsDefect As Boolean, ByVal JobNumber As String, ByVal WOQuantity As String, ByVal AQL As String, ByVal Location As String, ByVal TemplateId As Integer, ByVal DataNo As String, ByVal CID As String, ByVal Auditor As String, ByVal CasePack As String, ByVal WorkRoom As String, ByVal WeaverNamesString As String) As String
            Dim jsobj As New SPCInspection.InspectionJobSummary
            'Dim inputelementarray = jser.Deserialize(Of List(Of InputArray))(JsonString)
            'Dim dictionary As New Dictionary(Of String, String)
            Dim listiv As New List(Of SPCInspection.InspectionVaribles)
            Dim as400 As New AS400DAO
            Dim JobObj As New SPCInspection.StartJobInfo

            jsobj.JobType = jobtype.Trim
            jsobj.DataNo = DataNo
            jsobj.JobNumber = JobNumber
            jsobj.WOQuantity = CType(WOQuantity, Int32)
            jsobj.Standard = AQLStandard
            jsobj.EmployeeNo = Auditor
            jsobj.UnitCost = 0 ' DA.GetLocalUnitCost(DataNo, CID)
            jsobj.UnitDesc = as400.Getas400UnitDesc(DataNo)
            jsobj.TemplateId = TemplateId
            jsobj.PRP_Code = ""
            If Len(jsobj.JobNumber) > 2 And jsobj.WOQuantity > 0 And TemplateId > 0 Then

                If jsobj.JobType = "WorkOrder" Then
                    jsobj.AQL_Level = CType(AQL, Decimal)
                    jsobj.CasePack = CasePack
                    jsobj.WorkRoom = WorkRoom
                    Try
                        If isCompleted(jsobj.JobNumber, CID, jsobj.AQL_Level) = True Then
                            Return -98
                        End If
                        If jsobj.UnitCost = 0 Then
                            Dim workorderarray As Array = as400.GetInspectionWorkOrder(JobNumber).ToArray()
                            If Not workorderarray Is Nothing Then
                                Dim listso As New List(Of SingleObject)
                                Dim listuc As New List(Of SPCInspection.UnitCost)

                                If workorderarray.Length > 0 Then
                                    If workorderarray(0).SHORTITEM.ToString().Length > 0 Then
                                        listuc = DA.Getas400UnitCost(workorderarray(0).SHORTITEM)
                                    End If
                                End If
                                listso = IU.getAS400LocationName(CID)

                                If Not listso Is Nothing And listso.Count > 0 Then
                                    Dim ucobj = From v In listuc Where v.BRANCH = listso.ToArray()(0).Object1
                                    If Not ucobj Is Nothing And ucobj.Count > 0 Then
                                        jsobj.UnitCost = ucobj.ToArray()(0).UNITCOST
                                    ElseIf listuc.Count > 0 Then
                                        jsobj.UnitCost = listuc.ToArray()(0).UNITCOST
                                    End If
                                Else
                                    If Not listuc Is Nothing And listuc.Count > 0 Then
                                        jsobj.UnitCost = listuc.ToArray()(0).UNITCOST
                                    End If
                                End If

                            End If

                        End If
                    Catch ex As Exception
                        jsobj.UnitCost = 0
                    End Try

                    Select Case AQLStandard
                        Case "Reduced"
                            listiv = DA.SetReducedSamplesSize(jsobj.WOQuantity, jsobj.AQL_Level)
                        Case "Tightened"
                            listiv = DA.SetTightSamplesSize(jsobj.WOQuantity, jsobj.AQL_Level)
                        Case "Regular"
                            listiv = DA.SetSampleSize(jsobj.WOQuantity, jsobj.AQL_Level)
                        Case Else
                            listiv = DA.SetSampleSize(jsobj.WOQuantity, jsobj.AQL_Level)
                    End Select
                    If listiv.Count > 0 Then
                        jsobj.RejectLimiter = CType(listiv.ToArray()(0).RE, Integer)
                        jsobj.SampleSize = CType(listiv.ToArray()(0).SampleSize, Integer)
                    Else
                        Return -99
                    End If
                    jsobj.ItemPassCount = jsobj.WOQuantity
                Else
                    jsobj.AQL_Level = 100
                    jsobj.RejectLimiter = 0
                    jsobj.SampleSize = jsobj.WOQuantity
                    jsobj.ItemPassCount = -1
                    jsobj.UnitCost = 0

                End If


                If IsDefect = False Then
                    jsobj.ItemFailCount = 0
                Else
                    jsobj.ItemFailCount = 1
                End If

                jsobj.Inspection_Started = Date.Now
                jsobj.WorkOrderPieces = jsobj.WOQuantity
                jsobj.CID = Location

                JobObj.JobSummaryId = DA.InsertJobSummaryRecord(jsobj)

                If jsobj.JobType <> "WorkOrder" Then
                    JobObj.WeaverShiftId = RecordWeaverProduction(WeaverNamesString, JobObj.JobSummaryId)

                End If

            End If
            Return jser.Serialize(JobObj)

        End Function

        Private Function isCompleted(ByVal JobNumber As String, ByVal CID As String, ByVal AQL_Level As Decimal) As Boolean

            If AQL_Level <> 100 Then
                Return False
            End If

            Dim bmap As New BMappers(Of SingleObject)

            Dim sql As String = "select Count(*) as Object1 from InspectionJobSummary ijs where RTRIM(LTRIM(ijs.JobNumber)) = '" + JobNumber.Trim() + "' and ijs.AQL_Level = 100 and ijs.Inspection_Finished is not null and RTRIM(LTRIM(ijs.CID)) = '" + CID.Trim() + "'"

            Dim singleObj = bmap.GetInspectObject(sql)

            If IsNothing(singleObj) = True Then
                Return False
            End If

            If singleObj.ToArray().Length = 0 Then
                Return False
            End If

            If singleObj.ToArray()(0).Object1 > 0 Then
                Return True
            End If

            Return False
        End Function

        Public Function RecordWeaverProduction(ByVal WeaverString As String, ByVal JobSummaryId As Integer) As Integer
            Dim ShiftId As Integer = 0

            If WeaverString.Length > 0 Then
                Dim weaverObj = jser.Deserialize(Of SPCInspection.Weavers)(WeaverString)
                If IsNothing(weaverObj) = False Then
                    Try
                        Using _db As New Inspection_Entities
                            Dim shiftRec = New WeaverShift()
                            shiftRec.Shift = 1
                            shiftRec.Start = DateTime.Now

                            _db.WeaverShifts.Add(shiftRec)

                            _db.SaveChanges()

                            Dim obj1 As WeaverProduction = New WeaverProduction()
                            Dim obj2 As WeaverProduction = New WeaverProduction()

                            obj1.JobSummaryId = JobSummaryId
                            obj2.JobSummaryId = JobSummaryId
                            obj1.EmployeeNoId = weaverObj.Weaver1ID
                            obj2.EmployeeNoId = weaverObj.Weaver2ID

                            obj1.ShiftId = shiftRec.Id
                            obj2.ShiftId = shiftRec.Id
                            ShiftId = shiftRec.Id
                            If obj1.EmployeeNoId > 0 Then
                                _db.WeaverProductions.Add(obj1)
                            End If
                            If obj2.EmployeeNoId > 0 Then
                                _db.WeaverProductions.Add(obj2)
                            End If

                            _db.SaveChanges()
                        End Using
                    Catch ex As Exception
                        Elmah.ErrorSignal.FromCurrentContext.Raise(ex)
                    End Try
                End If
            End If
            Return ShiftId
        End Function

        Public Function InspectionEnd(ByVal JsonString As String) As Boolean
            Dim dictionary As New Dictionary(Of String, String)
            Dim InspectionEndList As New List(Of SPCInspection.DefectMaster)()
            Dim jser As New JavaScriptSerializer()
            Dim inputelementarray = jser.Deserialize(Of List(Of InputArray))(JsonString)

            For Each element As InputArray In inputelementarray
                Dim valuestring = Convert.ToString(element)
                If Not String.IsNullOrEmpty(valuestring) Then
                    dictionary.Add(element.key, element.value)
                Else
                    dictionary.Add(element.key, Nothing)
                End If

            Next
            If dictionary.Count > 0 Then
                Try
                    InspectionEndList.Add(New SPCInspection.DefectMaster With {.DefectTime = Convert.ToDateTime(Now), .DefectDesc = "NoDefect", .POnumber = Convert.ToString(dictionary.Item("MainContent_CPNumber")), .DataNo = Convert.ToString(dictionary.Item("MainContent_DataNumber")), .EmployeeNo = Convert.ToString(dictionary.Item("Auditor_Name")), .Product = Convert.ToString(dictionary.Item("Product")), .DefectClass = "NoDefect", .AQL = Convert.ToString(dictionary.Item("AQLevel")), .ThisPieceNo = Convert.ToString(CType(dictionary.Item("MainContent_Good"), Integer) + CType(dictionary.Item("MainContent_Bad"), Integer)), .TotalLotPieces = Convert.ToString(dictionary.Item("MainContent_LotSize")), .SampleSize = Convert.ToString(dictionary.Item("MainContent_SampleSize")), .Tablet = "Browser", .WorkOrder = Convert.ToString(dictionary.Item("MainContent_WorkOrder")), .LotNo = Convert.ToString(dictionary.Item("MainContent_CartonNumber")), .Location = Convert.ToString(dictionary.Item("MainContent_Location")), .DataType = "InspectionEnd", .Dimensions = Convert.ToString(dictionary.Item("Dimensions")), .Comment = Convert.ToString(dictionary.Item("Comment")), .TemplateId = Convert.ToInt32(dictionary.Item("TemplateId")), .InspectionId = Convert.ToInt16(dictionary.Item("MainContent_InspectionId"))})
                    DA.InsertDefects(InspectionEndList)
                Catch ex As Exception

                End Try

            End If
            Return True

        End Function

        Public Function PassInspection(ByVal JsonString As String) As String
            Dim DefectArray As New List(Of SPCInspection.DefectMaster)
            Dim dictionary As New Dictionary(Of String, String)
            Dim jser As New JavaScriptSerializer()
            Dim inputelementarray = jser.Deserialize(Of List(Of InputArray))(JsonString)

            For Each element As InputArray In inputelementarray
                Dim valuestring = Convert.ToString(element)
                If Not String.IsNullOrEmpty(valuestring) Then
                    dictionary.Add(element.key, element.value)
                Else
                    dictionary.Add(element.key, Nothing)
                End If

            Next

            Dim GoodCount As Integer = CType(dictionary.Item("MainContent_Good"), Integer)
            Dim BadCount As Integer = CType(dictionary.Item("MainContent_Bad"), Integer)


            If (GoodCount + BadCount) = 0 Then
                Dim InspectionId As Integer = DA.GetTemplateId()
                Try
                    DefectArray.Add(New SPCInspection.DefectMaster With {.DefectTime = Convert.ToDateTime(Now), .DefectDesc = "NoDefect", .POnumber = Convert.ToString(dictionary.Item("MainContent_CPNumber")), .DataNo = Convert.ToString(dictionary.Item("MainContent_DataNumber")), .EmployeeNo = Convert.ToString(dictionary.Item("Auditor_Name")), .Product = Convert.ToString(dictionary.Item("Product")), .DefectClass = "NoDefect", .AQL = Convert.ToString(dictionary.Item("AQLevel")), .ThisPieceNo = Convert.ToString(CType(dictionary.Item("MainContent_Good"), Integer) + CType(dictionary.Item("MainContent_Bad"), Integer)), .TotalLotPieces = Convert.ToString(dictionary.Item("MainContent_LotSize")), .SampleSize = Convert.ToString(dictionary.Item("MainContent_SampleSize")), .Tablet = "Browser", .WorkOrder = Convert.ToString(dictionary.Item("MainContent_WorkOrder")), .LotNo = Convert.ToString(dictionary.Item("MainContent_CartonNumber")), .Location = Convert.ToString(dictionary.Item("MainContent_Location")), .DataType = "InspectionStart", .Dimensions = Convert.ToString(dictionary.Item("Dimensions")), .Comment = Convert.ToString(dictionary.Item("Comment")), .TemplateId = Convert.ToInt32(dictionary.Item("TemplateId")), .InspectionId = InspectionId})
                    Dim returnint As Integer = DA.InsertDefects(DefectArray)
                    Return jser.Serialize(InspectionId)
                Catch ex As Exception
                    Return -1
                End Try

            End If

            Return 0
        End Function

        Public Function SetSampleSize(ByVal _lotsize As String, ByVal _AQLevel As String, ByVal Standard As String) As String
            Dim jser As New JavaScriptSerializer()
            Select Case Standard
                Case "Reduced"
                    Return jser.Serialize(DA.SetReducedSamplesSize(_lotsize, _AQLevel))
                Case "Regular"
                    Return jser.Serialize(DA.SetSampleSize(_lotsize, _AQLevel))
                Case "Tightened"
                    Return jser.Serialize(DA.SetTightSamplesSize(_lotsize, _AQLevel))
                Case Else
                    Return jser.Serialize(DA.SetTightSamplesSize(_lotsize, _AQLevel))
            End Select

        End Function

        'Public Function CheckSPCWorkOrder(ByVal MachineName As String, ByVal CurrentWO As String) As String
        '    Dim bmapso As New BMappers(Of SingleObject)
        '    Dim listso As New List(Of SingleObject)
        '    Dim listret As New List(Of SPCInspection.SPCMachineCheck)
        '    Dim jser As New JavaScriptSerializer()

        '    listso = bmapso.GetInspectObject("SELECT WorkOrder AS Object1 FROM LiveProduction WHERE Machine = '" & MachineName & "'")

        '    If listso.Count > 0 Then
        '        If listso.ToArray()(0).Object1.ToString() <> CurrentWO Then
        '            listret.Add(New SPCInspection.SPCMachineCheck With {.CurrentWO = listso.ToArray()(0).Object1.ToString(), .IsNewWO = True})
        '        Else
        '            listret.Add(New SPCInspection.SPCMachineCheck With {.CurrentWO = CurrentWO, .IsNewWO = False})
        '        End If
        '    Else
        '        listret.Add(New SPCInspection.SPCMachineCheck With {.CurrentWO = "NAN", .IsNewWO = False})
        '    End If

        '    Return jser.Serialize(listret)

        'End Function

        Public Function GetAuditorNames(ByVal LocationId As String) As String
            Dim returnlist As New List(Of String)

            Dim DataSet = New DataSet
            Dim Sql As String
            returnlist.Add("_")
            Sql = "SELECT DISTINCT EmployeeNo" & vbCrLf &
            "FROM DefectMaster" & vbCrLf &
            "WHERE (DefectTime > (getdate() - 30)) AND (Location = '" & LocationId.Substring(3, 3) & "')"

            If Not Util.FillSPCDataSet(DataSet, "DataSet", Sql) Then
                Return Nothing
            End If

            Dim i As Integer = 0
            For Each drow As DataRow In DataSet.Tables(0).Rows
                If IsDBNull(drow(0)) = False And drow(0) <> "SELECT OPTION" Then
                    returnlist.Add(drow(0))
                End If
            Next
            returnlist.Add("SELECT OPTION")
            Dim count As Integer = (From item In returnlist
                                    Where item = "New Name"
                                    Select item).Count()

            If count = 0 Then
                returnlist.Add("New Name")
            End If
            Return jser.Serialize(returnlist.ToArray)

        End Function

        Public Function GetWeaverNames(ByVal LocationId As String) As String
            Dim WeaverNames As String = ""
            Try
                Using _db As New Inspection_Entities
                    _db.Configuration.ProxyCreationEnabled = False
                    Dim empList = (From x In _db.EmployeeNoes Where x.Type.ToUpper() = "WEAVER" Or x.Type = "ALL" Select x)
                    Dim newEmps = (From x In empList Where x.CID = LocationId Or x.CID = "000999").ToList()
                    WeaverNames = jser.Serialize(newEmps)
                End Using
            Catch ex As Exception
                Dim message = ex.Message
            End Try

            Return WeaverNames
        End Function

        Public Function GetRejectionCount(ByVal InspectionState As String, ByVal TargetOrder As String, ByVal InspectionJobSummaryId As Integer) As Integer
            Dim FilterField As String = ""
            Dim bmap_rc As New BMappers(Of core.SingleObject)
            Dim list_rc As New List(Of core.SingleObject)
            Dim RejectionCount As Integer = -1

            If InspectionJobSummaryId > 0 Then

                If InspectionState <> "" Then

                    Dim sql As String = "SELECT COUNT(*) AS Object1 FROM DefectMaster INNER JOIN ButtonTemplate ON DefectMaster.ButtonTemplateId = ButtonTemplate.id WHERE (DefectMaster.InspectionJobSummaryId = " & InspectionJobSummaryId.ToString() & ") AND (DefectMaster." & InspectionState.Trim() & " = '" & TargetOrder & "') AND (ButtonTemplate.DefectType <> '0') AND (ButtonTemplate.DefectType <> 'Upgrade') AND (ButtonTemplate.DefectType <> 'Fix') AND (ButtonTemplate.DefectType <> 'Time')"
                    list_rc = bmap_rc.GetInspectObject(sql)
                    If list_rc.Count > 0 Then
                        RejectionCount = CType(list_rc.ToArray()(0).Object1, Integer)
                    End If
                End If

            End If

            Return RejectionCount
        End Function

        Public Function GetItemInfo(ByVal DataNo As String) As String
            Dim jser As New JavaScriptSerializer()
            Dim listii As New List(Of SPCInspection.InspectionItemInfo)

            listii = DA.GetItemInfo(DataNo)

            Return jser.Serialize(listii)

        End Function

        Private Sub GetInputVariblesAsDictionary(ByVal Inputarray As List(Of InputArray))

            For Each element As InputArray In Inputarray
                Dim valuestring = Convert.ToString(element)
                If Not String.IsNullOrEmpty(valuestring) Then
                    Select Case element.key
                        Case "MainContent_RollNumber"
                            RollNumber = element.value
                        Case "MainContent_LoomNumber"
                            If IsNumeric(element.value) = True Then
                                LoomNumber = element.value
                            End If
                        Case "MainContent_ItemNumber"
                            ItemNumber = element.value
                        Case Else
                            dictionary.Add(element.key, element.value)
                    End Select
                Else
                    dictionary.Add(element.key, Nothing)
                End If
            Next

        End Sub

        Public Function Get_prp_select2(fromdate As String, todate As String) As String
            Dim retobj As New List(Of selectorobject)
            Dim fromd As DateTime = DateTime.Parse(fromdate)
            Dim tod As DateTime = DateTime.Parse(todate)

            Using _db As New Inspection_Entities
                Dim prpvals = (From v In _db.InspectionJobSummaries Where Len(v.PRP_Code) > 0 And v.Inspection_Started >= fromd And v.Inspection_Finished <= tod Select New selectorobject With {.id = v.PRP_Code, .text = v.PRP_Code}).Distinct().ToList()
                If IsNothing(prpvals) = False Then
                    If prpvals.Count > 0 Then
                        retobj = prpvals
                    End If
                End If
            End Using

            If retobj.Count = 0 Then
                retobj.Add(New selectorobject With {.id = "NOVALS", .text = "NOVALS"})
            End If

            Return jser.Serialize(retobj)

        End Function
    End Class

End Namespace
