<%@ WebHandler Language="VB" Class="core.SPC_InspectionUtility" %>

Imports System
Imports System.Web
Imports App.Utilities.Web.Handlers
Imports System.Web.Script.Serialization
Imports System.Data.SqlClient
Imports System.Data
Imports System.Globalization

Namespace core

    Public Class SPC_InspectionUtility
        Inherits BaseHandler

        Private IU As New InspectionUtilityDAO
        Private Util As New Utilities

        Dim Colors As String() = {"rgb(93, 135, 161)", "rgb(176, 181, 121)", "rgb(251, 176, 64)", "rgb(149, 160, 169)", "rgb(211, 18, 69)", "rgb(255, 222, 117)", "rgb(233, 227, 220)", "rgb(95, 110, 119)", "rgb(8, 88, 139)", "rgb(156, 123, 156)", "rgb(93, 152, 89)", "rgb(134, 152, 89)"}
        Public Function GetButtonLibrary() As String
            Dim DSB As New DisplayBoardDAO
            Dim jser As New JavaScriptSerializer
            Dim ButtonValues As New List(Of SPCInspection.buttonlibrary)

            ButtonValues = IU.GetButtonLibrary()

            If ButtonValues.Count > 0 Then
                Return jser.Serialize(ButtonValues.ToArray())
            Else
                Return "nodata"
            End If


        End Function

        Public Function GetButtonLibraryGrid() As String
            Dim DSB As New DisplayBoardDAO
            Dim jser As New JavaScriptSerializer
            Dim returnlist As String
            Dim buttonlib As List(Of SPCInspection.ButtonLibrarygrid) = IU.GetLibraryGrid()
            If buttonlib.Count > 0 Then
                returnlist = jser.Serialize(buttonlib)
            End If

            If buttonlib.Count > 0 Then
                Return jser.Serialize(buttonlib.ToArray())
            Else
                Return "[0]"
            End If

        End Function

        Public Function GetTemplates() As String
            Dim jser As New JavaScriptSerializer
            Dim SelectValues As New List(Of selector2array)

            SelectValues = IU.GetTemplateList()
            If SelectValues.Count > 0 Then
                Return jser.Serialize(SelectValues.ToArray())
            Else
                Return ""
            End If


        End Function

        Public Function GetTemplateTable() As String
            Dim jser As New JavaScriptSerializer
            Dim TemplateTable As New List(Of SPCInspection.TemplateTable)
            Dim TemplateList As New List(Of SPCInspection.InspectionTemplateManagement)
            Dim bmap_tl As New BMappers(Of SPCInspection.InspectionTemplateManagement)

            TemplateList = bmap_tl.GetInspectObject("SELECT * FROM TemplateName WHERE ACTIVE = 'True' ORDER BY Active DESC")

            Dim matches = From c In TemplateList
                          Where c.Name = "New Template"
                          Select c

            'TemplateTable = IU.GetTemplateTable()

            'Dim matches = From c In TemplateTable
            '              Where c.Name = "New Template"
            '              Select c
            Dim object123 As Object = matches(0)
            'Dim rownumber As Integer = TemplateTable.IndexOf(matches(0))
            Dim rownumber As Integer = TemplateList.IndexOf(matches(0))
            If rownumber >= 0 Then
                TemplateList.RemoveAt(rownumber)
                TemplateList.Insert(0, New SPCInspection.InspectionTemplateManagement With {.Name = "New Template", .Owner = "Len", .Loc_CAR = False, .Loc_STT = False, .Loc_STJ = False, .Loc_SPA = False, .Loc_CDC = False, .Loc_LINYI = False, .Loc_FSK = False, .Loc_PCE = False, .Loc_FPC = False, .Loc_FNL = False, .DateCreated = DateTime.Now.ToString(), .Active = "True"})
            End If
            If TemplateList.Count > 0 Then
                Return jser.Serialize(TemplateList.ToArray())
            Else
                Return "false"
            End If

        End Function


        Public Function GetTemplateCollection(ByVal TemplateId As Integer) As String
            Dim jser As New JavaScriptSerializer
            Dim TempCollValues As New List(Of SPCInspection.TemplateCollection)

            TempCollValues = IU.GetInputTemplateCollection(TemplateId)
            Dim asc As New List(Of SPCInspection.TemplateCollection)
            If Not TempCollValues Is Nothing Then
                asc = (From f In TempCollValues Order By f.TabNumber).ToList()
                If Not TempCollValues Is Nothing Then
                    Dim bmapso As New BMappers(Of SingleObject)
                    Dim listso As New List(Of SingleObject)

                    listso = bmapso.GetInspectObject("SELECT it.Name AS Object1 FROM TemplateName tn INNER JOIN InspectionTypes it ON it.id = tn.LineTypeId WHERE TemplateId = " & TemplateId.ToString() & "")

                    If listso.Count > 0 Then
                        For Each item As SPCInspection.TemplateCollection In asc
                            item.LineType = listso.ToArray()(0).Object1
                        Next
                    End If
                    Return jser.Serialize(asc)
                Else
                    Return "false"
                End If
            Else
                Return "empty"
            End If


        End Function

        Public Function GetTemplateCollection_Admin(ByVal TemplateId As Integer) As String
            Dim jser As New JavaScriptSerializer
            Dim TempCollValues As New List(Of SPCInspection.TemplateCollection)

            TempCollValues = IU.GetInputTemplateCollection_Admin(TemplateId)
            Dim asc As New List(Of SPCInspection.TemplateCollection)
            If Not TempCollValues Is Nothing Then
                asc = (From f In TempCollValues Order By f.TabNumber).ToList()
                If Not TempCollValues Is Nothing Then
                    Dim bmapso As New BMappers(Of SingleObject)
                    Dim listso As New List(Of SingleObject)

                    listso = bmapso.GetInspectObject("SELECT it.Name AS Object1 FROM TemplateName tn INNER JOIN InspectionTypes it ON it.id = tn.LineTypeId WHERE TemplateId = " & TemplateId.ToString() & "")

                    If listso.Count > 0 Then
                        For Each item As SPCInspection.TemplateCollection In asc
                            item.LineType = listso.ToArray()(0).Object1
                        Next
                    End If
                    Return jser.Serialize(asc)
                Else
                    Return "false"
                End If
            Else
                Return "empty"
            End If


        End Function

        Public Function InsertTemplate(ByVal Name As String, ByVal Username As String, ByVal InspectionType As String) As Integer

            Dim returnint As Integer = 0
            Dim sqlstring As String

            sqlstring = "select TemplateId from dbo.TemplateName where Name = '" & Name

            Dim TempId = IU.GetTemplateId(Name, Username)
            If TempId > 0 Then
                Return 0
            End If

            Dim listso As List(Of SingleObject)
            Dim bmapso As New BMappers(Of SingleObject)

            listso = bmapso.GetInspectObject("SELECT id as Object1, Abreviation as Object3 FROM InspectionTypes WHERE (Name = N'" & InspectionType.Trim().ToUpper() & "')")

            If IsNothing(listso) = False Then
                If listso.Count > 0 Then
                    returnint = IU.InsertTemplate(Name, Username, listso.ToArray()(0).Object1, listso.ToArray()(0).Object3)
                End If
            End If

            If returnint > 0 Then
                Return returnint
            Else
                Return 0
            End If

        End Function

        Public Function SubmitTemplate(ByVal TemplateId As Integer, ByVal TabArray As String, ByVal ButtonArray As String) As String

            Dim jser As New JavaScriptSerializer
            Dim Tab_Array = jser.Deserialize(Of List(Of SPCInspection.tabarray))(TabArray)
            Dim Button_Array = jser.Deserialize(Of List(Of SPCInspection.buttonarray))(ButtonArray)
            Dim Username As String = HttpContext.Current.Session("Username")
            Dim TempIdSet As DataSet = New DataSet

            Dim sqlstring As String = "SELECT COUNT(TemplateId) AS Count FROM TabTemplate WHERE (TemplateId = " & TemplateId.ToString() & ")"

            If Util.FillSPCDataSet(TempIdSet, "TempIdSet", sqlstring) Then
                If TempIdSet.Tables(0).Rows(0)("Count") = 0 Then
                    Dim result As Boolean = TemplateInsert(TemplateId, Tab_Array, Button_Array)
                    If result = True Then
                        IU.UpdateTemplateCollectionCache(TemplateId)
                        Return "Template Inserted"
                    Else
                        Return "Failed"
                    End If
                Else
                    Dim result As Boolean = TemplateUpdate(TemplateId, Tab_Array, Button_Array)
                    If result = True Then
                        IU.UpdateTemplateCollectionCache(TemplateId)
                        Return "Template Updated"
                    Else
                        Return "Failed"
                    End If
                End If

            Else
                Return "Failed"
            End If

        End Function

        Public Function TemplateUpdate(ByVal TemplateId As Integer, ByVal Tab_Array As System.Collections.Generic.List(Of SPCInspection.tabarray), ByVal Button_Array As System.Collections.Generic.List(Of SPCInspection.buttonarray)) As Boolean

            For i = 0 To Tab_Array.Count - 1
                Dim DeleteResult As Boolean
                Dim TabTemplateId As Integer

                If CheckForTab(TemplateId, Tab_Array(i).title, i) = True Then
                    DeleteResult = IU.DeleteButtonTemplate(Tab_Array(i).TabTemplateId)
                    TabTemplateId = Tab_Array(i).TabTemplateId
                Else
                    TabTemplateId = IU.InsertTab(TemplateId, Tab_Array(i).title, i)
                End If

                If DeleteResult = True Or TabTemplateId > 0 Then
                    For Each Button As SPCInspection.buttonarray In Button_Array
                        If Button.tabindex = i Then
                            If Not IU.InsertButton(TabTemplateId, Button.ButtonId, Button.text, Button.DefectType, Button.Timer) Then
                                Return False
                            End If
                        End If
                    Next
                Else
                    Return False
                End If



            Next

            Return True

        End Function

        Public Function TemplateInsert(ByVal TemplateId As Integer, ByVal Tab_Array As Object, ByVal Button_Array As Object) As Boolean

            For i = 0 To Tab_Array.Count - 1
                If CheckForTab(TemplateId, Tab_Array(i).title, i) = True Then
                    Return False
                    Exit Function
                End If
                Dim returnint As Integer = IU.InsertTab(TemplateId, Tab_Array(i).title, i)
                If returnint = 0 Then
                    Return False
                    Exit Function
                End If
                For Each Button As SPCInspection.buttonarray In Button_Array
                    If Button.tabindex = i Then
                        IU.InsertButton(returnint, Button.ButtonId, Button.text, Button.DefectType, Button.Timer)
                    End If
                Next
            Next
            IU.ActiveTemplate(TemplateId)
            Return True

        End Function

        Private Function CheckForTab(ByVal TemplateId As Integer, ByVal Name As String, ByVal TabNumber As Integer) As Boolean
            Dim sqlstring As String
            Dim TabSet As DataSet = New DataSet
            sqlstring = "select * from TabTemplate where TemplateId = '" & TemplateId.ToString() & "' and Name = '" & Name & "'"


            If Not Util.FillSPCDataSet(TabSet, "TabSet", sqlstring) Then
                Return False
            End If
            Dim id As Integer = TabSet.Tables(0).Rows.Count

            If id > 0 Then
                Return True
            Else
                Return False
            End If

        End Function

        Public Function DeleteTab(ByVal TabNumber As Integer, ByVal TemplateId As Integer, ByVal TabTemplateId As Integer) As Boolean


            If TabTemplateId <> 0 Then

                Dim var As Boolean = IU.DeleteButtonTemplate(TabTemplateId)

                If var = True Then
                    If IU.DeleteTabTemplate(TabTemplateId, TemplateId) Then
                        IU.UpdateTemplateCollectionCache(TemplateId)
                        Return True
                    Else
                        Return False
                    End If
                Else
                    Return False
                End If

            Else
                Return False
            End If

        End Function

        Public Function AddDefectType(ByVal Code As String, ByVal Text As String) As Integer
            Dim results As Integer
            results = IU.AddDefectType(Code, Text)
            Return results
        End Function

        Public Function GetMonthlySumChart(ByVal DataNo As String, ByVal InspectionId As Integer, ByVal fromdatestring As String, ByVal todatestring As String) As String
            Dim CacheList As New List(Of SPCInspection.DefectMasterDisplay)
            Dim jser As New JavaScriptSerializer
            Dim returnobj As Array
            Dim Fromdate As DateTime = Convert.ToDateTime(fromdatestring)
            Dim Todate As DateTime = Convert.ToDateTime(todatestring)
            Dim returnlist As New List(Of SPCInspection.GraphTable)

            CacheList = context.Cache("DefectMaster")

            If DataNo <> "NONE" And DataNo <> "" And InspectionId <> -1 Then
                returnobj = (From v In CacheList Where v.InspectionId = InspectionId And v.DataNo = DataNo Order By v.DefectId Ascending).ToArray()
            ElseIf DataNo <> "NONE" And DataNo <> "" And InspectionId = -1 Then
                returnobj = (From v In CacheList Where v.DataNo = DataNo Order By v.DefectId Ascending).ToArray()
            ElseIf DataNo = "NONE" Or DataNo = "" And InspectionId <> -1 Then
                returnobj = (From v In CacheList Where v.InspectionId = InspectionId Order By v.DefectId Ascending).ToArray()
            Else
                returnobj = (From v In CacheList Where v.DataType = "Defect" Order By v.DefectId Ascending).ToArray()
            End If

            If Not returnobj Is Nothing Then

                Dim startDate As DateTime = New DateTime(Fromdate.Year, Fromdate.Month, 1)
                Dim endate As DateTime = Todate
                Dim M As Integer = Math.Abs((Todate.Year - startDate.Year))
                Dim elapsedTicks As Long = Todate.Ticks - Fromdate.Ticks
                Dim elapsedSpan As New TimeSpan(elapsedTicks)
                Dim Thismonth As DateTime = New DateTime(Fromdate.Year, Fromdate.Month, Todate.Day)
                Dim DefectCount As Integer

                While Thismonth <= endate
                    Dim Listformdate As DateTime = New DateTime(Thismonth.Year, Thismonth.Month, 1)
                    For Each row In returnobj
                        Dim pointdate As DateTime = Convert.ToDateTime(row.DefectTime)
                        If Listformdate.Month = pointdate.Month Then
                            DefectCount += 1
                        End If
                    Next

                    returnlist.Add(New SPCInspection.GraphTable With {.Count = DefectCount, .Listdate = Listformdate.ToString("yyyy-MM-dd")})
                    Thismonth = Thismonth.AddMonths(1)

                End While
            End If

            Return jser.Serialize(returnlist)

        End Function

        Public Function GetDefectPieChart(ByVal DataNo As String, ByVal InspectionId As Integer, ByVal fromdatestring As String, ByVal todatestring As String, ByVal TemplateId As Integer) As String
            Dim CacheList As New List(Of SPCInspection.DefectMasterDisplay)
            Dim jser As New JavaScriptSerializer
            Dim returnobj As Array = Nothing
            Dim Fromdate As DateTime = Convert.ToDateTime(fromdatestring)
            Dim Todate As DateTime = Convert.ToDateTime(todatestring)
            Dim returnlist As New List(Of PieChart)
            Dim cnt As Integer = 0
            Dim dlayer As New dlayer
            Dim CIDList As New List(Of CID)
            CIDList = dlayer.GetCIDInfo()
            If IsNothing(CIDList) = False Then
                Dim CIDArray = CIDList.ToArray()
                CacheList = context.Cache("DefectMaster" + CIDArray(0).CID.ToString() + "_" + TemplateId.ToString())
                If CacheList.Count > 0 Then

                    If DataNo <> "NONE" And DataNo <> "" And InspectionId <> -1 Then
                        returnobj = (From v In CacheList Where v.InspectionId = InspectionId And v.DataNo = DataNo And v.DefectDesc <> "NoDefect" Group v By v.Product Into DefectTypes = Group, Count()).ToArray()
                    ElseIf DataNo <> "NONE" And DataNo <> "" And InspectionId = -1 Then
                        returnobj = (From v In CacheList Where v.DataNo = DataNo And v.DefectDesc <> "NoDefect" Group v By v.Product Into DefectTypes = Group, Count()).ToArray()
                    ElseIf DataNo = "NONE" Or DataNo = "" And InspectionId <> -1 Then
                        returnobj = (From v In CacheList Where v.InspectionId = InspectionId And v.DefectDesc <> "NoDefect" Group v By v.Product Into DefectTypes = Group, Count()).ToArray()
                    Else
                        returnobj = (From v In CacheList Where v.DefectDesc <> "NoDefect" Group v By v.Product Into DefectTypes = Group, Count()).ToArray()
                    End If

                    If IsNothing(returnobj) = False And returnobj.Length > 0 Then
                        For Each value In returnobj
                            returnlist.Add(New PieChart With {.label = value.Product, .value = value.Count, .color = Colors(cnt)})
                            cnt += 1
                            If cnt > 10 Then
                                cnt = 0
                            End If
                        Next
                    End If

                    If returnlist.Count = 0 Then
                        returnlist.Add(New PieChart With {.label = "NoProducts", .value = 1, .color = Colors(0)})
                    End If

                Else
                    returnlist.Add(New PieChart With {.label = "NoProducts_Error", .value = 1, .color = Colors(0)})
                End If
            Else
                returnlist.Add(New PieChart With {.label = "NoProducts_Error", .value = 1, .color = Colors(0)})
            End If

            Return jser.Serialize(returnlist)

        End Function

        Public Function GetDefectDescPieChart(ByVal DataNo As String, ByVal InspectionId As Integer, ByVal fromdatestring As String, ByVal todatestring As String, ByVal TemplateId As Integer) As String
            Dim CacheList As New List(Of SPCInspection.DefectMasterDisplay)
            Dim jser As New JavaScriptSerializer
            Dim returnobj As Array = Nothing
            Dim Fromdate As DateTime = Convert.ToDateTime(fromdatestring)
            Dim Todate As DateTime = Convert.ToDateTime(todatestring)
            Dim returnlist As New List(Of PieChart)
            Dim dlayer As New dlayer
            Dim cnt As Integer = 0
            Dim CIDList As New List(Of CID)
            CIDList = dlayer.GetCIDInfo()
            If IsNothing(CIDList) = False Then
                Dim CIDArray = CIDList.ToArray()
                CacheList = context.Cache("DefectMaster" + CIDArray(0).CID.ToString() + "_" + TemplateId.ToString())
                If CacheList.Count > 0 Then

                    If DataNo <> "NONE" And DataNo <> "" And InspectionId <> -1 Then
                        returnobj = (From v In CacheList Where v.InspectionId = InspectionId And v.DataNo = DataNo And v.DefectDesc <> "NoDefect" Group v By v.DefectDesc Into DefectTypes = Group, Count()).ToArray()
                    ElseIf DataNo <> "NONE" And DataNo <> "" And InspectionId = -1 Then
                        returnobj = (From v In CacheList Where v.DataNo = DataNo And v.DefectDesc <> "NoDefect" Group v By v.DefectDesc Into DefectTypes = Group, Count()).ToArray()
                    ElseIf DataNo = "NONE" Or DataNo = "" And InspectionId <> -1 Then
                        returnobj = (From v In CacheList Where v.InspectionId = InspectionId And v.DefectDesc <> "NoDefect" Group v By v.DefectDesc Into DefectTypes = Group, Count()).ToArray()
                    Else
                        returnobj = (From v In CacheList Where v.DefectDesc <> "NoDefect" Group v By v.DefectDesc Into DefectTypes = Group, Count()).ToArray()
                    End If

                    If IsNothing(returnobj) = False Then
                        For Each value In returnobj
                            returnlist.Add(New PieChart With {.label = value.DefectDesc, .value = value.Count, .color = Colors(cnt)})
                            cnt += 1
                            If cnt > 10 Then
                                cnt = 0
                            End If
                        Next
                    End If

                    If returnlist.Count = 0 Then
                        returnlist.Add(New PieChart With {.label = "NoDefects", .value = 1, .color = Colors(0)})
                    End If
                Else
                    returnlist.Add(New PieChart With {.label = "NoDefects_Error", .value = 1, .color = Colors(0)})
                End If
            Else
                returnlist.Add(New PieChart With {.label = "NoDefects_Error", .value = 1, .color = Colors(0)})
            End If

            Return jser.Serialize(returnlist)
        End Function

        Public Function UpdateSpecTable(ByVal TabTitle As String, ByVal JsonString As String, ByVal TemplateId As Integer) As Boolean
            Dim jser As New JavaScriptSerializer
            Dim counter As Integer
            Dim Spec_Array = jser.Deserialize(Of List(Of SPCInspection.ProductTableSpecs))(JsonString)
            Dim TabTemplateId As Integer

            TabTemplateId = IU.GetTabTemplateID(TabTitle, TemplateId)
            If TemplateId <> 0 Then

                For Each item In Spec_Array
                    If item.Spec_Description <> "" Then
                        Try
                            Dim returnval = IU.AddProductSpec(TabTemplateId, item.Spec_Description, item.value, item.Upper_Spec_Value, item.Lower_Spec_Value)
                            If returnval > 0 Then
                                counter += 1
                            End If
                        Catch ex As Exception

                        End Try


                    End If
                Next
            Else
                Return False
            End If

            If counter > 0 Then
                IU.UpdateProductSpecBit(TabTemplateId)
                Return True
            Else
                Return False
            End If

        End Function



    End Class


End Namespace
