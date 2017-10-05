Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Data
Imports System.Globalization
Imports System.Data.Odbc
Imports System.Web.Script.Serialization


Namespace core


    Public Class InspectionUtilityDAO
        Inherits System.Web.UI.Page
        Private Property _DAOFactory As New DAOFactory
        Private Property DL As New dlayer
        Private Property util As New Utilities
        Private Property bmap As New BMappers(Of SPCInspection.TemplateCollection)
        Private Property bmap_1 As New BMappers(Of SPCInspection.ButtonTemplate)
        Private AprManagerDb As String = "AprManager"
        Public Shared ProductSpecsLastCachedDT As DateTime
        Public Shared ProductSpecsLastUpdatedDT As DateTime
        Public LocationNames As List(Of Locationarray)
        Public LocationsSelectors As List(Of selector2array)
        Public Property NEWALLID As Integer
        Dim PickCountstring As String
        Dim PickCountReader As SqlDataReader
        Dim PickCountcmdBuilder As SqlCommandBuilder
        Dim PickCountcmd As SqlCommand
        Dim PickCountConnection As SqlConnection
        Dim result As Boolean
        Dim ExecuteConnection As New SqlConnection(ConfigurationManager.ConnectionStrings("MyDB").ConnectionString)
        Dim Connection As New SqlConnection(ConfigurationManager.ConnectionStrings("MyDB").ConnectionString) 'Connection to MyDB
        Dim Command As New SqlCommand
        Dim DR As SqlDataReader
        Function ExecuteSQL(ByVal SQL As String, ByVal Log As String) As Object
            'This function executes and logs all of our custom SQL statements.

            Dim Outcome As String
            Command.Connection = ExecuteConnection
            ExecuteConnection.Open()
            Dim ErrorString As String = ""
            Try
                'Attempt to execute SQL statement passed into function
                Command.CommandText = SQL
                Command.ExecuteNonQuery()
                Outcome = "Successful"
            Catch ex As Exception
                'If SQL statement fails log error and trap exception
                ErrorString += " " & ex.ToString
                Outcome = "Failed"
                'LogEvent(1, "EXECUTE_SQL", "1 - Failure Executing SQL Statement", SQL, 1, Replace(ex.ToString, "'", "''"))
            End Try
            If Log = 1 Then
                Try
                    'Log our SQL attempt and any errors that may have been thrown
                    Command.CommandText = "insert into MY_sql_execution(MYSQL_Statement, MYSQL_Error, MYSQL_Result, MYSQL_Record, MYSQL_Form, MYSQL_UID, MYSQL_Login, MYSQL_Fullname, MYSQL_IP, MYSQL_SessionID) Select "
                    Command.CommandText += "'" + Replace(SQL, "'", "''") + "' as 'MYSQL_Statement', "
                    Command.CommandText += "'" + Replace(ErrorString, "'", "''") + "' as 'MYSQL_Error',"
                    Command.CommandText += "'" + Outcome + "' as 'MYSQL_Result',"
                    Command.CommandText += "'" + Request.QueryString("JID") + "' as 'MYSQL_Record',"
                    Command.CommandText += "'LandingPage.aspx' as 'Source',"
                    Command.CommandText += "'" + Convert.ToString(0) + "' as 'MYSQL_UID',"
                    Command.CommandText += "'" + "" + "' as 'MYSQL_Login',"
                    Command.CommandText += "'" + "None" + "' as 'MYSQL_Fullname',"
                    Command.CommandText += "'" + Request.UserHostAddress + "' as 'MYSQL_IP', "
                    Command.CommandText += "'" + Session.SessionID + "' as 'MYSQL_SessionID'"
                    Command.ExecuteNonQuery()
                Catch exc As Exception
                End Try
            End If
            ExecuteConnection.Close()
            Return Outcome
        End Function
        Public Sub New()
            Dim PointToTest = System.Web.Configuration.WebConfigurationManager.AppSettings("PointToTest")

            If PointToTest.ToUpper() = "TRUE" Then
                AprManagerDb = System.Web.Configuration.WebConfigurationManager.AppSettings("AprManagerTestDb")
            End If
        End Sub


        Public Function GetButtonLibrary() As List(Of SPCInspection.buttonlibrary)

            Dim ButtonValues As New List(Of SPCInspection.buttonlibrary)()
            Dim sqlstring As String

            sqlstring = "select * from dbo.ButtonLibrary WHERE Hide = 0 ORDER BY UPPER(Name) asc"

            ButtonValues = _DAOFactory.getbuttonlibrary(sqlstring, 3)

            Return ButtonValues

        End Function
        Public Function GetLibraryGrid() As List(Of SPCInspection.ButtonLibrarygrid)
            Dim selectObjects As New List(Of SPCInspection.ButtonLibrarygrid)
            Dim sql As String = "SELECT ButtonId, Name, DefectCode, Hide FROM ButtonLibrary Where Hide = 0 ORDER BY UPPER(Name) asc"
            Dim bmap As New BMappers(Of SPCInspection.ButtonLibrarygrid)
            'selectObjects = BMapper(Of SPCInspection.ButtonLibrarygrid).GetInspectObject(sql)
            selectObjects = bmap.GetInspectObject(sql)
            Return selectObjects
        End Function

        Public Function GetTemplateList() As List(Of selector2array)
            Dim sqlstring As String
            Dim selectValues As New List(Of selector2array)()

            sqlstring = "SELECT TemplateId, Name FROM TemplateName  WHERE Active = 1 ORDER BY TemplateId asc"

            selectValues = _DAOFactory.getSelector2(sqlstring, 3)

            Return selectValues

        End Function

        Public Function GetTemplateListByLocation(ByVal CID As Integer) As List(Of selector2array)

            Dim bmap_locm As New BMappers(Of core.SingleObject)
            Dim sel2_lst As New List(Of selector2array)
            Dim sel2_ret As New List(Of selector2array)
            If CID > 0 Then
                Dim loc_lst As List(Of core.SingleObject)
                Dim sql1 As String = "SELECT Abreviation AS Object1 FROM LocationMaster WHERE CID = 000" & CID.ToString()
                loc_lst = bmap_locm.GetAprMangObject(sql1)
                If loc_lst.Count > 0 Then
                    Dim bmap_sel As New BMappers(Of selector2array)
                    Dim sql2 As String = "SELECT TemplateId AS id, Name as text FROM TemplateName WHERE (Loc_" & loc_lst.ToArray()(0).Object1.ToString().Trim() & " = 1) AND (Active = 1)"
                    sel2_lst = bmap_sel.GetInspectObject(sql2)
                End If
            End If

            If sel2_lst.Count > 0 Then
                Dim listar = sel2_lst.ToArray()
                sel2_lst.Add(New selector2array With {.id = -1, .text = "SELECT OPTION"})
                sel2_ret = (From x In sel2_lst Select x Order By x.id Ascending).ToList()
            Else
                sel2_ret.Add(New selector2array With {.id = -1, .text = "NO TEMPLATES"})
            End If
            Return sel2_ret
        End Function

        Public Function GetTemplateListByLocation_2(ByVal CID As Integer) As List(Of selector2array)

            Dim bmap_locm As New BMappers(Of core.SingleObject)
            Dim sel2_lst As New List(Of selector2array)
            Dim sel2_ret As New List(Of selector2array)
            If CID > 0 Then
                Dim loc_lst As List(Of core.SingleObject)
                Dim sql1 As String = "SELECT id AS Object1, Abreviation AS Object3 FROM LocationMaster WHERE CID = 000" & CID.ToString()
                loc_lst = bmap_locm.GetAprMangObject(sql1)
                If loc_lst.Count > 0 Then
                    Dim bmap_sel As New BMappers(Of selector2array)
                    Dim sql2 As String = "select tn.TemplateId as id, tn.Name as text from TemplateName tn INNER JOIN TemplateActivator ta ON tn.TemplateId = ta.TemplateId where ta.LocationMasterId = " + loc_lst.ToArray()(0).Object1.ToString() + " and ta.Status = 1 and tn.Active = 1"
                    sel2_lst = bmap_sel.GetInspectObject(sql2)
                End If
            End If

            If sel2_lst.Count > 0 Then
                Dim listar = sel2_lst.ToArray()
                sel2_lst.Add(New selector2array With {.id = -1, .text = "SELECT OPTION"})
                sel2_ret = (From x In sel2_lst Select x Order By x.id Ascending).ToList()
            Else
                sel2_ret.Add(New selector2array With {.id = -1, .text = "NO TEMPLATES"})
            End If
            Return sel2_ret
        End Function

        Public Function GetSPCMachineNames(ByVal CID As String) As List(Of selector2array)
            Dim sql As String
            Dim selectvalues As New List(Of selector2array)
            Dim bmap As New BMappers(Of selector2array)

            If CID = "999" Then
                sql = "SELECT Id AS id, Machine as text FROM LiveProduction"
            Else
                sql = "SELECT Id AS id, Machine as text FROM LiveProduction WHERE CID = '" & CID & "'"
            End If

            selectvalues = bmap.GetSpcObject(sql)

            Return selectvalues

        End Function

        Public Function GetLocations(Optional ByVal IsReporter As Boolean = True) As List(Of selector2array)
            Dim sqlstring As String
            Dim selectValues As New List(Of Locationarray)()
            Dim bmap As New BMappers(Of Locationarray)

            sqlstring = "SELECT id as id, Name as text, Abreviation as Abreviation, CAST(CAST(RTRIM(CID) AS INTEGER) AS VARCHAR) as CID, AS400_Abr as ProdAbreviation FROM  LocationMaster WHERE  (InspectionResults = 1) ORDER BY text ASC"
            Try

            
            selectValues = bmap.GetAprMangObject(sqlstring)
            If selectValues.Count > 0 Then

                If IsReporter = True Then

                    Dim object1 As Object = (From x In selectValues Order By x.id Descending Select x.id).ToArray()

                        selectValues.Add(New Locationarray With {.id = object1(0) + 1, .CID = 999, .text = "ALL SITES", .Abreviation = "ALL", .ProdAbreviation = "ALL"})



                        NEWALLID = 999
                End If

                LocationsSelectors = (From x In selectValues Order By x.id Descending Select New core.selector2array With {.id = x.id, .text = Trim(x.text)}).ToList()

            End If
                LocationNames = selectValues

            Catch ex As Exception
                Throw New Exception("Error Retrieving Locations: " + ex.Message)
            End Try

            Return LocationsSelectors

        End Function

        Public Function GetTemplateTable() As List(Of SPCInspection.TemplateTable)
            Dim sqlstring As String
            Dim selectValues As New List(Of SPCInspection.TemplateTable)()
            Dim bmaptt As New BMappers(Of SPCInspection.TemplateTable)

            sqlstring = "SELECT * FROM TemplateName WHERE ACTIVE = 'True' ORDER BY Active DESC"

            'selectValues = _DAOFactory.getTemplateTable(sqlstring, 3)
            selectValues = bmaptt.GetInspectObject(sqlstring)

            For i = 0 To selectValues.Count - 1
                Dim temparray As Array = selectValues.ToArray()
                Dim thisdate As Date = Convert.ToDateTime(temparray(i).DateCreated)
                selectValues(i).DateCreated = thisdate.ToString("MM/dd/yyyy")
            Next

            Return selectValues

        End Function



        Public Function GetTemplateCollection(ByVal TemplateId As Integer) As List(Of SPCInspection.TemplateCollection)
            Dim sqlstring As String
            Dim selectValues As New List(Of SPCInspection.TemplateCollection)()

            sqlstring = "SELECT TabTemplate.TabTemplateId, ButtonTemplate.ButtonId, TabTemplate.Name, TabTemplate.TabNumber, TabTemplate.TemplateId, ButtonLibrary.Name AS ButtonName, TabTemplate.ProductSpecs, ButtonTemplate.DefectType, ButtonTemplate.id, ButtonLibrary.DefectCode, ButtonTemplate.Hide" & vbCrLf &
                            "FROM TabTemplate LEFT OUTER JOIN" & vbCrLf &
                            "ButtonTemplate ON TabTemplate.TabTemplateId = ButtonTemplate.TabTemplateId LEFT OUTER JOIN" & vbCrLf &
                            "ButtonLibrary ON ButtonTemplate.ButtonId = ButtonLibrary.ButtonId" & vbCrLf &
                            "WHERE (TabTemplate.TemplateId = " & TemplateId.ToString() & ") AND (TabTemplate.Active = 1)"


            'selectValues = _DAOFactory.getTemplateCollection(sqlstring)
            selectValues = bmap.GetInspectObject(sqlstring)
            If selectValues.Count = 0 Then
                Return selectValues
            End If
            Return selectValues

        End Function

        Public Function GetInputTemplateCollection(ByVal TemplateId As Integer) As List(Of SPCInspection.TemplateCollection)
            Dim sqlstring As String
            Dim selectValues As New List(Of SPCInspection.TemplateCollection)()

            sqlstring = "SELECT TabTemplate.TabTemplateId, ButtonTemplate.ButtonId, TabTemplate.Name, TabTemplate.TabNumber, TabTemplate.TemplateId, ButtonLibrary.Name AS ButtonName, TabTemplate.ProductSpecs, ButtonTemplate.DefectType, ButtonTemplate.id, ButtonLibrary.DefectCode, ButtonTemplate.Hide, ButtonLibrary.ButtonId AS ButtonLibraryId, ButtonTemplate.Timer" & vbCrLf &
                            "FROM TabTemplate LEFT OUTER JOIN" & vbCrLf &
                            "ButtonTemplate ON TabTemplate.TabTemplateId = ButtonTemplate.TabTemplateId LEFT OUTER JOIN" & vbCrLf &
                            "ButtonLibrary ON ButtonTemplate.ButtonId = ButtonLibrary.ButtonId" & vbCrLf &
                            "WHERE (TabTemplate.TemplateId = " & TemplateId.ToString() & ") AND (TabTemplate.Active = 1) AND (ButtonLibrary.Hide = 0 or Buttonlibrary.Hide IS NULL) and (ButtonTemplate.Hide = 0)" & vbCrLf &
                            "ORDER BY TabNumber ASC"


            'selectValues = _DAOFactory.getTemplateCollection(sqlstring)
            selectValues = bmap.GetInspectObject(sqlstring)
            If selectValues.Count = 0 Then
                Return selectValues
            End If
            Return selectValues

        End Function

        Public Function GetInputTemplateCollection_Admin(ByVal TemplateId As Integer) As List(Of SPCInspection.TemplateCollection)
            Dim sqlstring As String
            Dim selectValues As New List(Of SPCInspection.TemplateCollection)()

            sqlstring = "SELECT TabTemplate.TabTemplateId, ButtonTemplate.ButtonId, TabTemplate.Name, TabTemplate.TabNumber, TabTemplate.TemplateId, ButtonLibrary.Name AS ButtonName, TabTemplate.ProductSpecs, ButtonTemplate.DefectType, ButtonTemplate.id, ButtonLibrary.DefectCode, ButtonTemplate.Hide, ButtonLibrary.ButtonId AS ButtonLibraryId, ButtonTemplate.Timer" & vbCrLf &
                            "FROM TabTemplate LEFT OUTER JOIN" & vbCrLf &
                            "ButtonTemplate ON TabTemplate.TabTemplateId = ButtonTemplate.TabTemplateId LEFT OUTER JOIN" & vbCrLf &
                            "ButtonLibrary ON ButtonTemplate.ButtonId = ButtonLibrary.ButtonId" & vbCrLf &
                            "WHERE (TabTemplate.TemplateId = " & TemplateId.ToString() & ") AND (TabTemplate.Active = 1) AND (ButtonLibrary.Hide = 0 or Buttonlibrary.Hide IS NULL)" & vbCrLf &
                            "ORDER BY TabNumber ASC"


            'selectValues = _DAOFactory.getTemplateCollection(sqlstring)
            selectValues = bmap.GetInspectObject(sqlstring)
            If selectValues.Count = 0 Then
                Return selectValues
            End If
            Return selectValues

        End Function

        Public Function GetSpecCollection(ByVal TemplateId As Integer) As List(Of SPCInspection.ProductSpecCollection)
            Dim sqlstring As String
            Dim selectValues As New List(Of SPCInspection.ProductSpecCollection)()

            sqlstring = "SELECT TabTemplate.TabTemplateId, TabTemplate.TabNumber, ProductSpecification.Spec_Description, ProductSpecification.value, ProductSpecification.Upper_Spec_Value, ProductSpecification.Lower_Spec_Value, ProductSpecification.SpecId, TabTemplate.Name" & vbCrLf &
                         "FROM ProductSpecification INNER JOIN" & vbCrLf &
                         "TabTemplate ON ProductSpecification.TabTemplateId = TabTemplate.TabTemplateId" & vbCrLf &
                         "WHERE (TabTemplate.TemplateId = " & TemplateId.ToString() & ")"


            selectValues = _DAOFactory.getProductSpecCollection(sqlstring)
            If selectValues.Count = 0 Then
                Return selectValues
            End If
            Return selectValues
        End Function

        'Public Function GetDisplaySpecCollection(ByVal CID As String) As List(Of SPCInspection.ProductDisplaySpecCollection)
        '    Dim sqlstring As String
        '    Dim selectValues As New List(Of SPCInspection.ProductDisplaySpecCollection)
        '    Dim bmapps As New BMappers(Of SPCInspection.ProductDisplaySpecCollection)

        '    If CID <> "999" Then
        '        sqlstring = "SELECT         SpecMeasurements.SpecId, SpecMeasurements.id, SpecMeasurements.InspectionJobSummaryId, SpecMeasurements.Timestamp, SpecMeasurements.MeasureValue, InspectionJobSummary.JobNumber, " & vbCrLf &
        '                   "SpecMeasurements.DefectId, SpecMeasurements.InspectionId, InspectionJobSummary.Inspection_Started, ProductSpecification.POM_Row, ProductSpecification.DataNo, SpecMeasurements.ItemNumber, ProductSpecification.ProductType," & vbCrLf &
        '                   "ProductSpecification.Spec_Description, ProductSpecification.value, SpecMeasurements.MeasureValue - ProductSpecification.value AS SpecDelta, ProductSpecification.Upper_Spec_Value," & vbCrLf &
        '                   "ProductSpecification.Lower_Spec_Value" & vbCrLf &
        '"FROM            SpecMeasurements LEFT OUTER JOIN" & vbCrLf &
        '                   "InspectionJobSummary ON SpecMeasurements.InspectionJobSummaryId = InspectionJobSummary.id LEFT OUTER JOIN" & vbCrLf &
        '                   "ProductSpecification ON SpecMeasurements.SpecId = ProductSpecification.SpecId" & vbCrLf &
        '"WHERE        (InspectionJobSummary.CID = N'" & CID & "')" & vbCrLf &
        '"ORDER BY SpecMeasurements.InspectionJobSummaryId"
        '    Else
        '        sqlstring = "SELECT         SpecMeasurements.SpecId, SpecMeasurements.id, SpecMeasurements.InspectionJobSummaryId, SpecMeasurements.Timestamp, SpecMeasurements.MeasureValue, InspectionJobSummary.JobNumber, " & vbCrLf &
        '                       "SpecMeasurements.DefectId, SpecMeasurements.InspectionId, InspectionJobSummary.Inspection_Started, ProductSpecification.POM_Row, ProductSpecification.DataNo, SpecMeasurements.ItemNumber, ProductSpecification.ProductType," & vbCrLf &
        '                       "ProductSpecification.Spec_Description, ProductSpecification.value, SpecMeasurements.MeasureValue - ProductSpecification.value AS SpecDelta, ProductSpecification.Upper_Spec_Value," & vbCrLf &
        '                       "ProductSpecification.Lower_Spec_Value" & vbCrLf &
        '    "FROM            SpecMeasurements LEFT OUTER JOIN" & vbCrLf &
        '                       "InspectionJobSummary ON SpecMeasurements.InspectionJobSummaryId = InspectionJobSummary.id LEFT OUTER JOIN" & vbCrLf &
        '                       "ProductSpecification ON SpecMeasurements.SpecId = ProductSpecification.SpecId" & vbCrLf &
        '    "ORDER BY SpecMeasurements.InspectionJobSummaryId"
        '    End If




        '    selectValues = bmapps.GetInspectObject(sqlstring)

        '    Return selectValues
        'End Function

        Public Function GetDisplaySpecCollection2(ByVal CID As String, ByVal fromdate As DateTime, ByVal todate As DateTime) As List(Of SPCInspection.ProductDisplaySpecCollection)
            Dim sqlstring As String
            Dim selectValues As New List(Of SPCInspection.ProductDisplaySpecCollection)
            Dim bmapps As New BMappers(Of SPCInspection.ProductDisplaySpecCollection)


            sqlstring = "SELECT         SpecMeasurements.SpecId, SpecMeasurements.id, SpecMeasurements.InspectionJobSummaryId, SpecMeasurements.Timestamp, SpecMeasurements.MeasureValue, InspectionJobSummary.JobNumber, " & vbCrLf &
                                   "SpecMeasurements.DefectId, SpecMeasurements.InspectionId, InspectionJobSummary.Inspection_Started, ProductSpecification.POM_Row, ProductSpecification.DataNo, SpecMeasurements.ItemNumber, ProductSpecification.ProductType," & vbCrLf &
                                   "ProductSpecification.Spec_Description, ProductSpecification.value, SpecMeasurements.MeasureValue - ProductSpecification.value AS SpecDelta, ProductSpecification.Upper_Spec_Value," & vbCrLf &
                                   "ProductSpecification.Lower_Spec_Value, InspectionJobSummary.CID AS Location" & vbCrLf &
                "FROM            SpecMeasurements LEFT OUTER JOIN" & vbCrLf &
                                   "InspectionJobSummary ON SpecMeasurements.InspectionJobSummaryId = InspectionJobSummary.id LEFT OUTER JOIN" & vbCrLf &
                                   "ProductSpecification ON SpecMeasurements.SpecId = ProductSpecification.SpecId" & vbCrLf &
                "WHERE        (Timestamp >= CONVERT(DATETIME,'" & fromdate.ToString("yyyy-MM-dd H:mm:ss") & "' )) AND (Timestamp < CONVERT(DATETIME,'" & todate.AddDays(1).ToString("yyyy-MM-dd H:mm:ss") & "' ))" & vbCrLf &
                "ORDER BY SpecMeasurements.InspectionJobSummaryId DESC"

            selectValues = bmapps.GetInspectObject(sqlstring)

            Return selectValues
        End Function

        Public Function GetDisplaySpecCollection3(ByVal fromdate As DateTime, ByVal todate As DateTime) As List(Of SPCInspection.ProductDisplaySpecCollection)
            Dim sqlstring As String
            Dim selectValues As New List(Of SPCInspection.ProductDisplaySpecCollection)
            Dim bmapps As New BMappers(Of SPCInspection.ProductDisplaySpecCollection)


            sqlstring = "SELECT         SpecMeasurements.SpecId, SpecMeasurements.id, SpecMeasurements.InspectionJobSummaryId, SpecMeasurements.Timestamp, SpecMeasurements.MeasureValue, InspectionJobSummary.JobNumber, " & vbCrLf &
                                   "SpecMeasurements.DefectId, SpecMeasurements.InspectionId, InspectionJobSummary.Inspection_Started, ProductSpecification.POM_Row, ProductSpecification.DataNo, SpecMeasurements.ItemNumber, ProductSpecification.ProductType," & vbCrLf &
                                   "ProductSpecification.Spec_Description, ProductSpecification.value, SpecMeasurements.MeasureValue - ProductSpecification.value AS SpecDelta, ProductSpecification.Upper_Spec_Value," & vbCrLf &
                                   "ProductSpecification.Lower_Spec_Value, InspectionJobSummary.CID AS Location" & vbCrLf &
                "FROM            SpecMeasurements LEFT OUTER JOIN" & vbCrLf &
                                   "InspectionJobSummary ON SpecMeasurements.InspectionJobSummaryId = InspectionJobSummary.id LEFT OUTER JOIN" & vbCrLf &
                                   "ProductSpecification ON SpecMeasurements.SpecId = ProductSpecification.SpecId" & vbCrLf &
                "WHERE        (Timestamp >= CONVERT(DATETIME,'" & fromdate.ToString("yyyy-MM-dd H:mm:ss") & "' )) AND (Timestamp < CONVERT(DATETIME,'" & todate.AddDays(1).ToString("yyyy-MM-dd H:mm:ss") & "' ))" & vbCrLf &
                "ORDER BY SpecMeasurements.InspectionJobSummaryId DESC"

            selectValues = bmapps.GetInspectObject(sqlstring)

            Return selectValues
        End Function

        Public Function GetUpdatedDisplaySpecCollection(ByVal SpecFilter As Integer) As List(Of SPCInspection.ProductDisplaySpecCollection)
            Dim sqlstring As String
            Dim selectValues As New List(Of SPCInspection.ProductDisplaySpecCollection)
            Dim bmapps As New BMappers(Of SPCInspection.ProductDisplaySpecCollection)


            sqlstring = "SELECT         SpecMeasurements.SpecId, SpecMeasurements.id, SpecMeasurements.InspectionJobSummaryId, SpecMeasurements.Timestamp, SpecMeasurements.MeasureValue, InspectionJobSummary.JobNumber, " & vbCrLf &
                                   "SpecMeasurements.DefectId, SpecMeasurements.InspectionId, InspectionJobSummary.Inspection_Started, ProductSpecification.POM_Row, ProductSpecification.DataNo, SpecMeasurements.ItemNumber, ProductSpecification.ProductType," & vbCrLf &
                                   "ProductSpecification.Spec_Description, ProductSpecification.value, SpecMeasurements.MeasureValue - ProductSpecification.value AS SpecDelta, ProductSpecification.Upper_Spec_Value," & vbCrLf &
                                   "ProductSpecification.Lower_Spec_Value, InspectionJobSummary.CID AS Location" & vbCrLf &
                "FROM            SpecMeasurements LEFT OUTER JOIN" & vbCrLf &
                                   "InspectionJobSummary ON SpecMeasurements.InspectionJobSummaryId = InspectionJobSummary.id LEFT OUTER JOIN" & vbCrLf &
                                   "ProductSpecification ON SpecMeasurements.SpecId = ProductSpecification.SpecId" & vbCrLf &
                "WHERE        (SpecId > " & SpecFilter.ToString() & ")" & vbCrLf &
                "ORDER BY SpecMeasurements.InspectionJobSummaryId DESC"

            selectValues = bmapps.GetInspectObject(sqlstring)

            Return selectValues
        End Function


        Public Function DeleteButtonTemplate(ByVal TemplateId As Integer) As Boolean
            Dim SQL As String
            Dim sqlcommand As SqlCommand
            Dim returnint As Integer

            'SQL = "DELETE FROM Maintenance_Schedule WHERE MM_ID = " & MM_ID & " And MT_ID = " & MT_ID & " And MS_Next_Main_Date = @Start_Date "
            SQL = "DELETE FROM ButtonTemplate" & vbCrLf &
                    "WHERE (TabTemplateId = " & TemplateId.ToString() & ")"
            Using connection As New SqlConnection(DL.InspectConnectionString())

                sqlcommand = _DAOFactory.GetCommand(SQL.ToString(), connection)
                'Add command parameters             

                Try
                    sqlcommand.Connection.Open()
                    returnint = sqlcommand.ExecuteNonQuery()

                    If returnint < 0 Then
                        Return False
                    End If

                Catch e As Exception
                    Return False
                End Try



            End Using
            Return True

        End Function
        Public Function AddDefectType(ByVal Code As String, ByVal Text As String) As Integer
            Dim SQL As String
            Dim Internalcmd As SqlCommand
            Dim returnint As Integer

            SQL = "INSERT INTO ButtonLibrary (Name, Text, DefectCode)" & vbCrLf &
                    "VALUES (@Name,@Text, @DefectCode)"
            Using connection As New SqlConnection(DL.InspectConnectionString())

                Internalcmd = _DAOFactory.GetCommand(SQL, connection)

                Internalcmd.Parameters.Add(_DAOFactory.Getparameter("@Name", DbType.String))
                Internalcmd.Parameters.Add(_DAOFactory.Getparameter("@Text", DbType.String))
                Internalcmd.Parameters.Add(_DAOFactory.Getparameter("@DefectCode", DbType.String))
                Internalcmd.Parameters("@Name").Value = Text
                Internalcmd.Parameters("@Text").Value = Text
                Internalcmd.Parameters("@DefectCode").Value = Code
                Try
                    Internalcmd.Connection.Open()
                    returnint = Convert.ToInt32(Internalcmd.ExecuteNonQuery())
                    Return returnint
                Catch ex As Exception

                    Return -1
                    Exit Function
                End Try

            End Using

        End Function

        Public Function AddProductSpec(ByVal TabTemplateId As Integer, ByVal Description As String, ByVal value As Decimal, ByVal UpperSpec As Decimal, ByVal LowerSpec As Decimal) As Integer

            Dim SQL As String
            Dim Internalcmd As SqlCommand
            Dim returnint As Integer

            SQL = "INSERT INTO ProductSpecification" & vbCrLf &
                         "(TabTemplateId, Spec_Description, value, Upper_Spec_Value, Lower_Spec_Value)" & vbCrLf &
                         "VALUES (@TabTemplateId,@Spec_Description,@value,@Upper_Spec_Value,@Lower_Spec_Value)"

            Using connection As New SqlConnection(DL.InspectConnectionString())

                Internalcmd = _DAOFactory.GetCommand(SQL, connection)

                Internalcmd.Parameters.Add(_DAOFactory.Getparameter("@TabTemplateId", DbType.Int32))
                Internalcmd.Parameters.Add(_DAOFactory.Getparameter("@Spec_Description", DbType.String))
                Internalcmd.Parameters.Add(_DAOFactory.Getparameter("@value", DbType.Decimal))
                Internalcmd.Parameters.Add(_DAOFactory.Getparameter("@Upper_Spec_Value", DbType.Decimal))
                Internalcmd.Parameters.Add(_DAOFactory.Getparameter("@Lower_Spec_Value", DbType.Decimal))
                Internalcmd.Parameters("@TabTemplateId").Value = TabTemplateId
                Internalcmd.Parameters("@Spec_Description").Value = Description
                Internalcmd.Parameters("@value").Value = value
                Internalcmd.Parameters("@Upper_Spec_Value").Value = UpperSpec
                Internalcmd.Parameters("@Lower_Spec_Value").Value = LowerSpec

                Try
                    Internalcmd.Connection.Open()
                    returnint = Convert.ToInt32(Internalcmd.ExecuteNonQuery())
                    Return returnint
                Catch ex As Exception

                    Throw New System.Exception(ex.Message)
                    Exit Function
                End Try

            End Using


        End Function
        Public Function EditButtonClass(ByVal tabId As Integer, ByVal DefectClass As String) As Integer
            Dim Outcome As String = ""
            Dim SQL As String = "UPDATE dbo.ButtonTemplate SET DefectType='" & DefectClass & "' WHERE id =  " & tabId.ToString()
            Outcome = ExecuteSQL(SQL, 1)
            If Outcome = "Successful" Then
                Return True
            End If
            Return False
        End Function
        Public Function DeleteDefectButtonTemplate(ByVal rowId As Integer) As Integer 'Pretty sure ButtonTemplate is unused...
            Dim Outcome As String = ""
            Dim SQL As String = "UPDATE dbo.ButtonTemplate SET Hide = 1 WHERE ButtonId =  " & rowId.ToString()
            Outcome = ExecuteSQL(SQL, 1)
            If Outcome = "Successful" Then
                Return True
            End If
            Return False
        End Function
        Public Function DeleteRow(ByVal rowId As Integer) As Integer
            Dim Outcome As String = ""
            Dim SQL As String = "UPDATE dbo.ButtonLibrary SET Hide = 1 WHERE ButtonId = " & rowId.ToString()
            If DeleteDefectButtonTemplate(rowId) Then 'takes care of the button template too
                Outcome = ExecuteSQL(SQL, 1)
            End If

            If Outcome = "Successful" Then
                Return True
            End If
            Return False
        End Function
        Public Function AlterDefectRow(ByVal rowId As Integer, ByVal DefectCode As String, ByVal Name As String) As Integer
            Dim Outcome As String = ""
            If rowId = -1 Then
                Dim SQL As String = "INSERT INTO dbo.ButtonLibrary (Name, DefectCode) VALUES ('" & Name & "', '" & DefectCode & "')"
                Outcome = ExecuteSQL(SQL, 1)
                If Outcome = "Successful" Then
                    Return True
                End If
                Return False

            Else

                Dim SQL As String = "UPDATE dbo.ButtonLibrary SET Name = '" & Name & "', DefectCode = '" & DefectCode & "' WHERE ButtonId =  " & rowId.ToString()
                Outcome = ExecuteSQL(SQL, 1)
                If Outcome = "Successful" Then
                    Return True
                End If
                Return False
            End If
        End Function



        Public Function DeleteTabTemplate(ByVal TabTemplatId As Integer, ByVal TemplateId As Integer) As Integer
            Dim Outcome As String = ""
            Dim SQL As String = "UPDATE TabTemplate SET Active = 0 WHERE (TemplateId = " & TemplateId.ToString() & ") AND (TabTemplateId = " & TabTemplatId.ToString() & ")"
            Outcome = ExecuteSQL(SQL, 1)
            If Outcome = "Successful" Then
                Return True
            End If
            Return False
        End Function
        Public Function UpdateButtonLibrary(ByVal obj As SPCInspection.ButtonLibrarygrid) As Boolean
            Dim Outcome As String = ""
            Dim SQL As String = "UPDATE dbo.ButtonLibrary SET Name = '" & obj.Name & "', DefectCode = '" & obj.DefectCode & "' WHERE ButtonId =  " & obj.ButtonId.ToString()
            Outcome = ExecuteSQL(SQL, 1)
            If Outcome = "Successful" Then
                Return True
            End If
            Return False
        End Function
        Public Function ToggleStatusTemplateById(ByVal TemplateId As Integer, ByVal Status As Boolean) As Integer
            Dim Outcome As String = ""
            Dim SQL As String
            Dim StatusVal As String

            If Status = True Then
                StatusVal = "0"
            Else
                StatusVal = "1"
            End If

            SQL = "UPDATE TemplateName SET Active = " & StatusVal & "WHERE (TemplateId = " & TemplateId.ToString() & ")"
            Outcome = ExecuteSQL(SQL, 1)
            If Outcome = "Successful" Then
                Return True
            End If
            Return False

        End Function
        Public Function UpdateProductSpecBit(ByVal TabTemplateId As Integer) As Integer
            Dim SQL As String
            Dim Outcome As String = ""

            SQL = "UPDATE TabTemplate" & vbCrLf &
            "SET ProductSpecs = 1" & vbCrLf &
            "WHERE (TabTemplateId = " & TabTemplateId.ToString() & ")"

            Outcome = ExecuteSQL(SQL, 1)
            If Outcome = "Successful" Then
                Return True
            End If
            Return False

        End Function

        Public Function InsertTemplate(ByVal name As String, Username As String, ByVal LineTypeId As Integer, ByVal LineName As String) As Integer

            Dim SQL As String
            Dim sqlcommand As SqlCommand
            Dim returnint As Integer

            SQL = "INSERT INTO TemplateName (Name,Owner, DateCreated, LineTypeId, LineType, Active) VALUES(@Name, @Username, @Created, @LineTypeId, @LineType, 1)  SELECT SCOPE_IDENTITY()   "


            Using connection As New SqlConnection(DL.InspectConnectionString())


                sqlcommand = _DAOFactory.GetCommand(SQL, connection)
                ''    'Add command parameters                                                                          
                sqlcommand.Parameters.Add(_DAOFactory.Getparameter("@Name", DbType.String))
                sqlcommand.Parameters.Add(_DAOFactory.Getparameter("@Username", DbType.String))
                sqlcommand.Parameters.Add(_DAOFactory.Getparameter("@Created", DbType.DateTime))
                sqlcommand.Parameters.Add(_DAOFactory.Getparameter("@LineTypeId", DbType.Int64))
                sqlcommand.Parameters.Add(_DAOFactory.Getparameter("@LineType", DbType.String))
                ''    'Provide parameter values.                                                                    
                sqlcommand.Parameters("@Name").Value = name
                sqlcommand.Parameters("@Username").Value = Username
                sqlcommand.Parameters("@Created").Value = DateTime.Now
                sqlcommand.Parameters("@LineTypeId").Value = LineTypeId
                sqlcommand.Parameters("@LineType").Value = LineName

                Try

                    sqlcommand.Connection.Open()
                    returnint = sqlcommand.ExecuteScalar()

                Catch e As Exception

                    Return 0
                    Exit Function
                End Try


            End Using
            Return returnint

        End Function

        Public Function InsertTab(ByVal TemplateId As Integer, ByVal Tabname As String, Tabnumber As Integer) As Integer

            Dim SQL As String
            Dim sqlcommand As SqlCommand
            Dim returnint As Integer

            SQL = "INSERT INTO TabTemplate (TemplateId, Name, TabNumber, Updated) VALUES(@TemplateId, @Name, @TabNumber, @Updated) SELECT SCOPE_IDENTITY()  "


            Using connection As New SqlConnection(DL.InspectConnectionString())


                sqlcommand = _DAOFactory.GetCommand(SQL, connection)
                ''    'Add command parameters                                                                          
                sqlcommand.Parameters.Add(_DAOFactory.Getparameter("@TemplateId", DbType.Int32))
                sqlcommand.Parameters.Add(_DAOFactory.Getparameter("@Name", DbType.String))
                sqlcommand.Parameters.Add(_DAOFactory.Getparameter("@TabNumber", DbType.Int32))
                sqlcommand.Parameters.Add(_DAOFactory.Getparameter("@Updated", DbType.DateTime))
                ''    'Provide parameter values.                                                                    
                sqlcommand.Parameters("@TemplateId").Value = TemplateId
                sqlcommand.Parameters("@Name").Value = Tabname
                sqlcommand.Parameters("@TabNumber").Value = Tabnumber
                sqlcommand.Parameters("@Updated").Value = DateTime.Now

                Try

                    sqlcommand.Connection.Open()
                    returnint = Convert.ToInt32(sqlcommand.ExecuteScalar())

                Catch e As Exception

                    Return 0
                    Exit Function
                End Try


            End Using
            Return returnint

        End Function

        Public Function InsertButton(ByVal TabTemplateId As Integer, ByVal ButtonId As Integer, ByVal Name As String, ByVal DefectType As String, ByVal TimerFlag_Val As String) As Boolean

            Dim SQL As String
            Dim sqlcommand As SqlCommand
            Dim returnint As Integer
            Dim util As New Utilities

            SQL = "INSERT INTO ButtonTemplate (TabTemplateId, ButtonId, DefectType, Timer) VALUES(@TabTemplateId, @ButtonId, @DefectType, @Timer) SELECT SCOPE_IDENTITY()   "


            Using connection As New SqlConnection(DL.InspectConnectionString())


                sqlcommand = _DAOFactory.GetCommand(SQL, connection)
                ''    'Add command parameters                                                                          
                sqlcommand.Parameters.Add(_DAOFactory.Getparameter("@TabTemplateId", DbType.Int32))
                sqlcommand.Parameters.Add(_DAOFactory.Getparameter("@ButtonId", DbType.Int32))
                sqlcommand.Parameters.Add(_DAOFactory.Getparameter("@DefectType", DbType.String))
                sqlcommand.Parameters.Add(_DAOFactory.Getparameter("@Timer", DbType.Boolean))

                ''    'Provide parameter values.                                                                    
                sqlcommand.Parameters("@TabTemplateId").Value = TabTemplateId
                sqlcommand.Parameters("@ButtonId").Value = ButtonId
                sqlcommand.Parameters("@DefectType").Value = DefectType
                sqlcommand.Parameters("@Timer").Value = util.ConvertType(TimerFlag_Val, "boolean")



                Try

                    sqlcommand.Connection.Open()
                    returnint = Convert.ToInt32(sqlcommand.ExecuteScalar())

                Catch e As Exception

                    Return 0
                    Exit Function
                End Try


            End Using
            Return returnint

        End Function



        Public Function ActiveTemplate(ByVal TemplateId As Integer) As Boolean

            Dim SQL As String
            Dim sqlcommand As SqlCommand
            Dim returnint As Integer

            SQL = "UPDATE TemplateName" & vbCrLf &
            "SET Active = 1" & vbCrLf &
            "WHERE (TemplateId = @TemplateId)"

            Using connection As New SqlConnection(DL.InspectConnectionString())

                sqlcommand = _DAOFactory.GetCommand(SQL, connection)
                ''    'Add command parameters                                                                          
                sqlcommand.Parameters.Add(_DAOFactory.Getparameter("@TemplateId", DbType.Int32))

                ''    'Provide parameter values.                                                                    
                sqlcommand.Parameters("@TemplateId").Value = TemplateId

                Try
                    sqlcommand.Connection.Open()
                    returnint = Convert.ToInt32(sqlcommand.ExecuteScalar())
                    If returnint = 0 Then
                        Return False
                    End If
                Catch e As Exception

                    Return False
                    Exit Function
                End Try

            End Using
            Return True

        End Function

        Public Function SetLineType(ByVal TemplateId As Integer, ByVal LineType As String, ByVal ColumnCount As Integer) As Boolean

            Dim SQL As String
            Dim sqlcommand As SqlCommand
            Dim returnint As Integer
            Dim listso As List(Of SingleObject)
            Dim bmapso As New BMappers(Of SingleObject)

            listso = bmapso.GetInspectObject("SELECT id as Object1 FROM InspectionTypes WHERE Name = '" & LineType & "'")

            If IsNothing(listso) = False Then
                If listso.Count > 0 Then


                    SQL = "UPDATE TemplateName" & vbCrLf &
                    "SET LineTypeId = " & listso.ToArray()(0).Object1.ToString() & ", ColumnCount = " + ColumnCount.ToString() & vbCrLf &
                    "WHERE (TemplateId = @TemplateId)"

                    Using connection As New SqlConnection(DL.InspectConnectionString())

                        sqlcommand = _DAOFactory.GetCommand(SQL, connection)
                        ''    'Add command parameters                                                                          
                        sqlcommand.Parameters.Add(_DAOFactory.Getparameter("@TemplateId", DbType.Int32))

                        ''    'Provide parameter values.                                                                    
                        sqlcommand.Parameters("@TemplateId").Value = TemplateId

                        Try
                            sqlcommand.Connection.Open()
                            returnint = Convert.ToInt32(sqlcommand.ExecuteScalar())
                            If returnint = 0 Then
                                Return False
                            Else
                                Return True
                            End If
                            Exit Function
                        Catch e As Exception

                            Return False
                            Exit Function
                        End Try

                    End Using
                    Return True

                End If
            End If

            Return False

        End Function

        Public Function GetTemplateId(ByVal name As String, ByVal Username As String) As Integer
            Dim sqlstring As String
            Dim returnint As Integer
            sqlstring = "SELECT TOP (1) TemplateId FROM  TemplateName WHERE (Name = '" & name & "') AND (Owner = '" & Username & "')"

            returnint = _DAOFactory.GetTemplateId(sqlstring, 3)

            Return returnint

        End Function

        Public Function GetDefeaultTemplateID(ByVal TemplateId As String) As String

            Dim TemplateSet As DataSet = New DataSet
            Dim sql As String

            sql = "SELECT TOP (1) Name AS Expr1 FROM TemplateName WHERE(TemplateId = '" & TemplateId & "')"

            If Not util.FillSPCDataSet(TemplateSet, "TemplateSet", sql) Then
                Return Nothing
            End If
            For Each row As DataRow In TemplateSet.Tables(0).Rows
                Dim rowobject As Object = row
                If IsNothing(row.Item(0)) = False Then
                    Return Convert.ToString(row.Item(0))
                End If
            Next
            Return Nothing
        End Function

        Public Function GetTabTemplateID(ByVal TabTitle As String, TemplateId As Integer) As Integer

            Dim TemplateSet As DataSet = New DataSet
            Dim sql As String

            sql = "SELECT TOP (1) TabTemplateId FROM TabTemplate WHERE (TemplateId = '" & TemplateId & "') AND (Name = '" & TabTitle & "')"

            If Not util.FillSPCDataSet(TemplateSet, "TemplateSet", sql) Then
                Return Nothing
            End If
            If TemplateSet.Tables(0).Rows.Count > 0 Then
                Return TemplateSet.Tables(0).Rows(0)("TabTemplateId")
            End If

            Return Nothing

        End Function
        Public Function GetDefectMasterData(ByVal fromdate As DateTime, ByVal todate As DateTime, ByVal Location As String, ByVal Abb As String) As List(Of SPCInspection.DefectMasterDisplay)
            Dim SQL As String
            Dim returnlist As New List(Of SPCInspection.DefectMasterDisplay)
            Dim returnlist2 As New List(Of SPCInspection.DefectMasterDisplay)
            Dim fromdatestring As String = fromdate.ToString("yyyy-MM-dd H:mm:ss").Split(" ")(0) + " 00:00:00"
            Dim todatestring As String = todate.ToString("yyyy-MM-dd H:mm:ss").Split(" ")(0) + " 23:59:59"
            Dim bmap As New BMappers(Of SPCInspection.DefectMasterDisplay)
            Dim locationNameArray = (From x In LocationNames Where x.CID = Location Select x.text).ToArray()
            Dim LocmId = (From x In LocationNames Where x.CID = Location Select x.id).ToArray()

            If Abb = "ALL" Then
                SQL = "SELECT DefectMaster.DefectID, DefectMaster.DefectTime, DefectMaster.DefectDesc, TemplateName.TemplateId, TemplateName.Name, DefectMaster.DataNo, DefectMaster.EmployeeNo, DefectMaster.AQL, DefectMaster.InspectionId, DefectMaster.TotalLotPieces, DefectMaster.Product, DefectMaster.DefectClass, DefectMaster.WorkOrder, DefectMaster.RollNo, DefectMaster.LoomNo, DefectMaster.DataType, DefectMaster.DefectImage_Filename, DefectMaster.Inspector, DefectMaster.InspectionState, DefectMaster.ItemNumber, DefectMaster.WorkRoom" & vbCrLf &
             "FROM DefectMaster INNER JOIN  TemplateName ON DefectMaster.TemplateId = TemplateName.TemplateId" & vbCrLf &
             "WHERE (DefectTime >= CONVERT(DATETIME, '" & fromdatestring & "', 102)) AND (DefectTime <= CONVERT(DATETIME, '" & todatestring & "', 102))" & vbCrLf &
              "ORDER BY DefectMaster.DefectID DESC"
            Else
                SQL = "SELECT DefectMaster.DefectID, DefectMaster.DefectTime, DefectMaster.DefectDesc, TemplateName.TemplateId, TemplateName.Name, DefectMaster.DataNo, DefectMaster.EmployeeNo, DefectMaster.AQL, DefectMaster.InspectionId, DefectMaster.TotalLotPieces, DefectMaster.Product, DefectMaster.DefectClass, DefectMaster.WorkOrder, DefectMaster.RollNo, DefectMaster.LoomNo, DefectMaster.DataType, DefectMaster.DefectImage_Filename, DefectMaster.Inspector, DefectMaster.InspectionState, DefectMaster.ItemNumber, DefectMaster.WorkRoom" & vbCrLf &
             "FROM DefectMaster INNER JOIN  TemplateName ON DefectMaster.TemplateId = TemplateName.TemplateId" & vbCrLf &
             "WHERE (DefectTime >= CONVERT(DATETIME, '" & fromdatestring & "', 102)) AND (DefectTime <= CONVERT(DATETIME, '" & todatestring & "', 102)) AND (Location = '" + Location + "' OR Location = '" + LocmId(0).ToString() + "' OR SUBSTRING(DataType, 0, 4) = '" + Abb + "')" & vbCrLf &
              "ORDER BY DefectMaster.DefectID DESC"
            End If
            
            'SQL = "SELECT DefectID, DefectTime, DefectDesc, POnumber, DataNo, EmployeeNo, AQL, InspectionId, TotalLotPieces, Product, DefectClass, WorkOrder, RollNo, LoomNo, DataType, DefectImage_Filename, Inspector, InspectionState, ItemNumber" & vbCrLf &
            ' "FROM DefectMaster" & vbCrLf &
            ' "WHERE (TemplateId = " & TemplateId.ToString() & ") AND (DefectTime >= CONVERT(DATETIME, '" & fromdate.ToString("yyyy-MM-dd H:mm:ss") & "', 102)) AND (DefectTime <= CONVERT(DATETIME, '" & todatestring & "', 102))" & vbCrLf &
            '  "ORDER BY DefectID DESC"
            Try
                'returnlist2 = BMapper(Of SPCInspection.DefectMasterDisplay).GetInspectObject(SQL)
                returnlist2 = bmap.GetInspectObject(SQL)
            Catch ex As Exception

            End Try

            'returnlist = _DAOFactory.getDefectMasterData(SQL)
            'For i = 0 To returnlist.Count - 1
            '    Dim temparray As Array = returnlist.ToArray()
            '    Dim thisdate As Date = Convert.ToDateTime(temparray(i).DefectTime)
            '    returnlist(i).DefectTime = thisdate.ToString("MM/dd/yyyy H:mm:ss")
            'Next


            'If returnlist.Count = 0 Then
            '    returnlist.Add(New SPCInspection.DefectMasterDisplay With {.DefectID = 0, .DefectTime = "00/00/00", .DefectDesc = "___", .POnumber = "No Records", .DataNo = "No Records", .EmployeeNo = "___", .InspectionId = 0, .TotalLotPieces = "No Records", .Product = "No Records", .WorkOrder = "No Records", .RollNo = "No Records", .LoomNo = "___", .DataType = "___", .DefectImage_Filename = "___"})
            'End If

            Return returnlist2

        End Function

        Public Function GetDefectMasterData2(ByVal fromdate As DateTime, ByVal todate As DateTime, ByVal Location As String) As List(Of SPCInspection.DefectMasterDisplay)
            Dim SQL As String
            Dim returnlist As New List(Of SPCInspection.DefectMasterDisplay)
            Dim returnlist2 As New List(Of SPCInspection.DefectMasterDisplay)
            Dim fromdatestring As String = fromdate.ToString("yyyy-MM-dd H:mm:ss").Split(" ")(0) + " 00:00:00"
            Dim todatestring As String = todate.ToString("yyyy-MM-dd H:mm:ss").Split(" ")(0) + " 23:59:59"
            Dim bmap As New BMappers(Of SPCInspection.DefectMasterDisplay)

            If Location = "999" Then
                SQL = "SELECT DefectMaster.DefectID, DefectMaster.DefectTime, DefectMaster.DefectDesc, TemplateName.TemplateId, TemplateName.Name, DefectMaster.DataNo, DefectMaster.EmployeeNo, DefectMaster.AQL, DefectMaster.InspectionId, DefectMaster.TotalLotPieces, DefectMaster.Product, DefectMaster.DefectClass, DefectMaster.WorkOrder, DefectMaster.RollNo, DefectMaster.LoomNo, DefectMaster.DataType, DefectMaster.DefectImage_Filename, DefectMaster.Inspector, DefectMaster.InspectionState, DefectMaster.ItemNumber, DefectMaster.WorkRoom" & vbCrLf &
             "FROM DefectMaster INNER JOIN  TemplateName ON DefectMaster.TemplateId = TemplateName.TemplateId" & vbCrLf &
             "WHERE (DefectTime >= CONVERT(DATETIME, '" & fromdatestring & "', 102)) AND (DefectTime <= CONVERT(DATETIME, '" & todatestring & "', 102))" & vbCrLf &
              "ORDER BY DefectMaster.DefectID DESC"
            ElseIf Location = "578" Then
                SQL = "SELECT DefectMaster.DefectID, DefectMaster.DefectTime, DefectMaster.DefectDesc, TemplateName.TemplateId, TemplateName.Name, DefectMaster.DataNo, DefectMaster.EmployeeNo, DefectMaster.AQL, DefectMaster.InspectionId, DefectMaster.TotalLotPieces, DefectMaster.Product, DefectMaster.DefectClass, DefectMaster.WorkOrder, DefectMaster.RollNo, DefectMaster.LoomNo, DefectMaster.DataType, DefectMaster.DefectImage_Filename, DefectMaster.Inspector, DefectMaster.InspectionState, DefectMaster.ItemNumber, DefectMaster.WorkRoom" & vbCrLf &
             "FROM DefectMaster INNER JOIN  TemplateName ON DefectMaster.TemplateId = TemplateName.TemplateId" & vbCrLf &
             "WHERE (DefectTime >= CONVERT(DATETIME, '" & fromdatestring & "', 102)) AND (DefectTime <= CONVERT(DATETIME, '" & todatestring & "', 102)) AND (Location = '" + Location + "' OR SUBSTRING(DataType, 0, 4) = 'STT')" & vbCrLf &
              "ORDER BY DefectMaster.DefectID DESC"
            Else
                SQL = "SELECT DefectMaster.DefectID, DefectMaster.DefectTime, DefectMaster.DefectDesc, TemplateName.TemplateId, TemplateName.Name, DefectMaster.DataNo, DefectMaster.EmployeeNo, DefectMaster.AQL, DefectMaster.InspectionId, DefectMaster.TotalLotPieces, DefectMaster.Product, DefectMaster.DefectClass, DefectMaster.WorkOrder, DefectMaster.RollNo, DefectMaster.LoomNo, DefectMaster.DataType, DefectMaster.DefectImage_Filename, DefectMaster.Inspector, DefectMaster.InspectionState, DefectMaster.ItemNumber, DefectMaster.WorkRoom" & vbCrLf &
             "FROM DefectMaster INNER JOIN  TemplateName ON DefectMaster.TemplateId = TemplateName.TemplateId" & vbCrLf &
             "WHERE (DefectTime >= CONVERT(DATETIME, '" & fromdatestring & "', 102)) AND (DefectTime <= CONVERT(DATETIME, '" & todatestring & "', 102)) AND (Location = '" + Location + "')" & vbCrLf &
              "ORDER BY DefectMaster.DefectID DESC"
            End If

            Try

                returnlist2 = bmap.GetInspectObject(SQL)
            Catch ex As Exception

            End Try

            Return returnlist2

        End Function

        Public Function GetDefectMasterData3(ByVal fromdate As DateTime, ByVal todate As DateTime, Optional ByVal filterlist As List(Of ActiveFilterObject) = Nothing) As List(Of SPCInspection.DefectMasterDisplay)
            Dim SQL As String
            Dim returnlist As New List(Of SPCInspection.DefectMasterDisplay)
            Dim returnlist2 As New List(Of SPCInspection.DefectMasterDisplay)
            Dim fromdatestring As String = fromdate.ToString("yyyy-MM-dd H:mm:ss").Split(" ")(0) + " 00:00:00"
            Dim todatestring As String = todate.ToString("yyyy-MM-dd H:mm:ss").Split(" ")(0) + " 23:59:59"
            Dim bmap As New BMappers(Of SPCInspection.DefectMasterDisplay)
            Dim datanosqlstring As String = ""
            Dim audittypesqlstring As String = ""

            If IsNothing(filterlist) = False Then
                If filterlist.Count > 0 Then
                    Select Case filterlist.ToArray()(0).Name
                        Case "pf_AuditType"
                            audittypesqlstring = " AND (it.Name = '" & filterlist.ToArray()(0).value.ToString() & "') "
                        Case "pf_DataNumber"
                            datanosqlstring = " AND (DefectMaster.DataNo = '" & filterlist.ToArray()(0).value.ToString() & "') "
                    End Select
                End If
            End If


            SQL = "SELECT DefectMaster.DefectID, DefectMaster.DefectTime, DefectMaster.DefectDesc, TemplateName.TemplateId, TemplateName.Name, ijs.DataNo, DefectMaster.EmployeeNo, ijs.AQL_Level AS AQL, DefectMaster.InspectionId, DefectMaster.TotalLotPieces, DefectMaster.Product, DefectMaster.DefectClass, DefectMaster.WorkOrder, DefectMaster.RollNo, DefectMaster.LoomNo, DefectMaster.DataType, DefectMaster.DefectImage_Filename, DefectMaster.Inspector, DefectMaster.InspectionState, DefectMaster.ItemNumber, DefectMaster.WorkRoom, DefectMaster.Location" & vbCrLf &
             "FROM DefectMaster INNER JOIN  TemplateName ON DefectMaster.TemplateId = TemplateName.TemplateId LEFT OUTER JOIN InspectionTypes it ON it.id = TemplateName.LineTypeId INNER JOIN InspectionJobSummary ijs ON DefectMaster.InspectionJobSummaryId = ijs.id" & vbCrLf &
             "WHERE (DefectTime >= CONVERT(DATETIME, '" & fromdatestring & "', 102)) AND (DefectTime <= CONVERT(DATETIME, '" & todatestring & "', 102))" & audittypesqlstring & datanosqlstring & vbCrLf &
              "ORDER BY DefectMaster.DefectID DESC"

            Try
                returnlist2 = bmap.GetInspectObject(SQL)
            Catch ex As Exception

            End Try

            Return returnlist2

        End Function

        Public Function GetDefectMasterById(ByVal ijsid As Integer) As List(Of SPCInspection.DefectMasterSubgrid)
            Dim SQL As String
            Dim returnlist As New List(Of SPCInspection.DefectMasterSubgrid)
            Dim bmap As New BMappers(Of SPCInspection.DefectMasterSubgrid)

            SQL = "SELECT DefectID, CONVERT(VARCHAR(19),DefectTime) AS DefectTime , EmployeeNo, InspectionId, DefectDesc, DefectClass, Product, WorkRoom FROM DefectMaster WHERE InspectionJobSummaryId = " & ijsid.ToString()

            Try
                returnlist = bmap.GetInspectObject(SQL)
            Catch ex As Exception

            End Try

            Return returnlist

        End Function

        Public Function GetSpecsById(ByVal ijsid As Integer) As List(Of SPCInspection.SpecsSubgrid)
            Dim SQL As String
            Dim returnlist As New List(Of SPCInspection.SpecsSubgrid)
            Dim bmap As New BMappers(Of SPCInspection.SpecsSubgrid)

            SQL = "select sm.id AS SMid, convert(varchar(30), sm.Timestamp) as Timestamp, ps.ProductType, ps.Spec_Description, ps.value, ps.Upper_Spec_Value, ps.Lower_Spec_Value, sm.MeasureValue, sm.SpecDelta, ps.SpecSource from SpecMeasurements sm INNER JOIN ProductSpecification ps ON sm.SpecId = ps.SpecId WHERE sm.InspectionJobSummaryId = " & ijsid.ToString()

            Try
                returnlist = bmap.GetInspectObject(SQL)
            Catch ex As Exception

            End Try

            Return returnlist

        End Function

        Public Function GetSpecsByLocation(ByVal ijsloc As List(Of ActiveLocations), ByVal fromdate As DateTime, ByVal todate As DateTime) As List(Of SPCInspection.SpecsSubgrid)
            Dim SQL As String
            Dim returnlist As New List(Of SPCInspection.SpecsSubgrid)
            Dim bmap As New BMappers(Of SPCInspection.SpecsSubgrid)
            Dim locstrg As String = ""

            If ijsloc.Count > 0 Then
                locstrg = GetLocationByInspectionSummaryFilter(ijsloc)
            End If

            SQL = "select sm.id AS SMid, ijs.JobNumber, ijs.DataNo, convert(varchar(30), sm.Timestamp) as Timestamp, ps.ProductType, ps.Spec_Description, ps.value, ps.Upper_Spec_Value, ps.Lower_Spec_Value, sm.MeasureValue, sm.SpecDelta, ps.SpecSource from SpecMeasurements sm INNER JOIN ProductSpecification ps ON sm.SpecId = ps.SpecId INNER JOIN InspectionJobSummary ijs ON ijs.id = sm.InspectionJobSummaryId WHERE sm.Timestamp >= '" & fromdate.ToString("MM/dd/yyyy hh:mm") & "' and " & locstrg & " sm.Timestamp <= '" & todate.ToString("MM/dd/yyyy hh:mm") & "'"

            Try
                returnlist = bmap.GetInspectObject(SQL)
            Catch ex As Exception

            End Try

            Return returnlist

        End Function

        Public Function GetMainResultsGraph(ByVal TemplateId As Integer, ByVal fromdate As DateTime, ByVal todate As DateTime, ByVal LocationId As Integer, ByVal CID As String) As List(Of SPCInspection.BarChart)
            Dim con As New SqlConnection(DL.InspectConnectionString())
            Dim cmd As SqlCommand = con.CreateCommand()
            Dim rglist As New List(Of SPCInspection.BarChart)
            Dim retlist As New List(Of SPCInspection.BarChart)
            Try
                Using con
                    con.Open()
                    Using cmd
                        cmd = New SqlCommand("SPC_DefectCountBreakdown_2", con)
                        cmd.CommandType = CommandType.StoredProcedure
                        cmd.Parameters.Add("@fromdate", SqlDbType.DateTime).Direction = ParameterDirection.Input
                        cmd.Parameters.Add("@todate", SqlDbType.DateTime).Direction = ParameterDirection.Input
                        cmd.Parameters.Add("@TEMPLATEID", SqlDbType.Int).Direction = ParameterDirection.Input
                        cmd.Parameters.Add("@LOCATIONID", SqlDbType.Int).Direction = ParameterDirection.Input
                        cmd.Parameters.Add("@CID", SqlDbType.VarChar).Direction = ParameterDirection.Input
                        cmd.Parameters("@fromdate").Value = fromdate
                        cmd.Parameters("@todate").Value = todate
                        cmd.Parameters("@TEMPLATEID").Value = TemplateId
                        cmd.Parameters("@LOCATIONID").Value = LocationId
                        cmd.Parameters("@CID").Value = CID

                        cmd.CommandTimeout = 5000

                        Dim bmap_rg As New BMappers(Of SPCInspection.BarChart)
                        rglist = bmap_rg.GetSpcSP(cmd)
                        retlist = (From x In rglist Order By x.x Ascending).ToList()
                    End Using
                End Using
            Catch ex As Exception
                Return Nothing
            End Try

            Return retlist
        End Function
        Public Function GetStackedDefectLineType(ByVal fromdate As DateTime, ByVal todate As DateTime, ByVal LocationId As Integer, ByVal CID As String) As List(Of SPCInspection.StackedDefectLineType)
            Dim con As New SqlConnection(DL.InspectConnectionString())
            Dim cmd As SqlCommand = con.CreateCommand()
            Dim rglist As New List(Of SPCInspection.StackedDefectLineType)
            Dim retlist As New List(Of SPCInspection.StackedDefectLineType)
            Try
                Using con
                    con.Open()
                    Using cmd
                        cmd = New SqlCommand("GetDefectBreakdownByLineType", con)
                        cmd.CommandType = CommandType.StoredProcedure
                        cmd.Parameters.Add("@fromdate", SqlDbType.DateTime).Direction = ParameterDirection.Input
                        cmd.Parameters.Add("@todate", SqlDbType.DateTime).Direction = ParameterDirection.Input
                        cmd.Parameters.Add("@LOCATIONID", SqlDbType.Int).Direction = ParameterDirection.Input
                        cmd.Parameters.Add("@Location", SqlDbType.VarChar).Direction = ParameterDirection.Input
                        cmd.Parameters("@fromdate").Value = fromdate
                        cmd.Parameters("@todate").Value = todate
                        cmd.Parameters("@LOCATIONID").Value = LocationId
                        cmd.Parameters("@Location").Value = CID

                        cmd.CommandTimeout = 5000

                        Dim bmap_rg As New BMappers(Of SPCInspection.StackedDefectLineType)
                        rglist = bmap_rg.GetSpcSP(cmd)
                    End Using
                End Using
            Catch ex As Exception
                Return Nothing
            End Try

            Return rglist
        End Function

        Public Function GetStackedDefectLineType2(ByVal fromdate As DateTime, ByVal todate As DateTime, ByVal DataNo As String, ByVal AuditType As String, ByVal LocArray As List(Of ActiveLocations)) As List(Of BarChartObject)
            Dim con As New SqlConnection(DL.InspectConnectionString())
            Dim cmd As SqlCommand = con.CreateCommand()
            Dim rglist As New List(Of BarChartObject)
            Dim retlist As New List(Of BarChartObject)
            Try
                Using con
                    con.Open()
                    Using cmd
                        cmd = New SqlCommand("GetInspectionTypesBD", con)
                        cmd.CommandType = CommandType.StoredProcedure
                        cmd.Parameters.Add("@fromdate", SqlDbType.DateTime).Direction = ParameterDirection.Input
                        cmd.Parameters.Add("@todate", SqlDbType.DateTime).Direction = ParameterDirection.Input
                        cmd.Parameters.Add("@LOCFILTER", SqlDbType.VarChar, -1).Direction = ParameterDirection.Input
                        cmd.Parameters.Add("@DataNo", SqlDbType.VarChar, 30).Direction = ParameterDirection.Input
                        cmd.Parameters.Add("@AuditType", SqlDbType.VarChar, 30).Direction = ParameterDirection.Input
                        cmd.Parameters("@fromdate").Value = fromdate
                        cmd.Parameters("@todate").Value = todate
                        cmd.Parameters("@LOCFILTER").Value = GetLocation_InspectionJobSummaryFilter(LocArray)
                        cmd.Parameters("@DataNo").Value = DataNo
                        cmd.Parameters("@AuditType").Value = AuditType
                        cmd.CommandTimeout = 5000

                        Dim bmap_rg As New BMappers(Of BarChartObject)
                        rglist = bmap_rg.GetSpcSP(cmd)
                    End Using
                End Using
            Catch ex As Exception
                Return Nothing
            End Try

            Return rglist
        End Function

        Public Function GetLineChart1(ByVal todate As DateTime, ByVal fromdate As DateTime) As List(Of SPCInspection.LineChart1)
            Dim con As New SqlConnection(DL.InspectConnectionString())
            Dim cmd As SqlCommand = con.CreateCommand()
            Dim rglist As New List(Of SPCInspection.LineChart1)
            Dim retlist As New List(Of SPCInspection.LineChart1)

            Try
                Using con
                    con.Open()
                    Using cmd
                        cmd = New SqlCommand("SP_GetDefectsByTypeRange", con)
                        cmd.CommandType = CommandType.StoredProcedure
                        cmd.Parameters.Add("@TODATE", SqlDbType.DateTime).Direction = ParameterDirection.Input
                        cmd.Parameters.Add("@FROMDATE", SqlDbType.DateTime).Direction = ParameterDirection.Input
                        cmd.Parameters.Add("@CID", SqlDbType.VarChar).Direction = ParameterDirection.Input
                        cmd.Parameters("@TODATE").Value = Date.Now
                        cmd.Parameters("@FROMDATE").Value = fromdate
                        cmd.Parameters("@CID").Value = "999"

                        cmd.CommandTimeout = 5000

                        Dim bmap_rg As New BMappers(Of SPCInspection.LineChart1)
                        rglist = bmap_rg.GetSpcSP(cmd)

                    End Using
                End Using
            Catch ex As Exception
                Return Nothing
            End Try

            Return rglist
        End Function
        

        Public Function GetScatterPlotData(ByVal todate As DateTime, ByVal fromdate As DateTime, ByVal CID As String) As List(Of SPCInspection.InspectionScatterPlot)
            Dim con As New SqlConnection(DL.InspectConnectionString())
            Dim cmd As SqlCommand = con.CreateCommand()
            Dim rglist As New List(Of SPCInspection.InspectionScatterPlot)
            Dim retlist As New List(Of SPCInspection.InspectionScatterPlot)
            Try
                Using con
                    con.Open()
                    Using cmd
                        cmd = New SqlCommand("GetDHUByTemplate", con)
                        cmd.CommandType = CommandType.StoredProcedure
                        cmd.Parameters.Add("@TODATE", SqlDbType.DateTime).Direction = ParameterDirection.Input
                        cmd.Parameters.Add("@FROMDATE", SqlDbType.DateTime).Direction = ParameterDirection.Input
                        cmd.Parameters.Add("@CID", SqlDbType.VarChar).Direction = ParameterDirection.Input
                        If DateDiff(DateInterval.Day, fromdate, todate) < 7 Then
                            fromdate = todate.AddDays(-7)
                        End If
                        cmd.Parameters("@TODATE").Value = todate
                        cmd.Parameters("@FROMDATE").Value = fromdate.AddDays(-1)
                        cmd.Parameters("@CID").Value = CID

                        cmd.CommandTimeout = 5000

                        Dim bmap_rg As New BMappers(Of SPCInspection.InspectionScatterPlot)
                        rglist = bmap_rg.GetSpcSP(cmd)

                    End Using
                End Using
            Catch ex As Exception
                Return Nothing
            End Try

            Return rglist
        End Function
        Public Function GetDHUByLocation(ByVal todate As DateTime, ByVal fromdate As DateTime, ByVal DataNo As String, ByVal AuditType As String, ByVal LocArray As List(Of ActiveLocations)) As List(Of SPCInspection.LocationLineChart)
            Dim con As New SqlConnection(DL.InspectConnectionString())
            Dim cmd As SqlCommand = con.CreateCommand()
            Dim rglist As New List(Of SPCInspection.LocationLineChart)
            Dim retlist As New List(Of SPCInspection.LocationLineChart)
            Dim teststrg As String = GetLocationMasterFilter(LocArray)
            Try
                Using con
                    con.Open()
                    Using cmd
                        cmd = New SqlCommand("GetDHUByLocation3", con)
                        cmd.CommandType = CommandType.StoredProcedure
                        cmd.Parameters.Add("@TODATE", SqlDbType.DateTime).Direction = ParameterDirection.Input
                        cmd.Parameters.Add("@FROMDATE", SqlDbType.DateTime).Direction = ParameterDirection.Input
                        cmd.Parameters.Add("@DATANO", SqlDbType.VarChar, 30).Direction = ParameterDirection.Input
                        cmd.Parameters.Add("@AUDITTYPE", SqlDbType.VarChar, 30).Direction = ParameterDirection.Input
                        cmd.Parameters.Add("@LOCFILTER", SqlDbType.VarChar, 10000).Direction = ParameterDirection.Input
                        If DateDiff(DateInterval.Day, fromdate, todate) < 7 Then
                            fromdate = todate.AddDays(-7)
                        End If
                        cmd.Parameters("@TODATE").Value = todate
                        cmd.Parameters("@FROMDATE").Value = fromdate.AddDays(-1)
                        cmd.Parameters("@DATANO").Value = DataNo
                        cmd.Parameters("@AUDITTYPE").Value = AuditType
                        cmd.Parameters("@LOCFILTER").Value = GetLocationMasterFilter(LocArray)
                        cmd.CommandTimeout = 5000

                        Dim bmap_rg As New BMappers(Of SPCInspection.LocationLineChart)
                        rglist = bmap_rg.GetSpcSP(cmd)

                    End Using
                End Using
            Catch ex As Exception
                Return Nothing
            End Try

            Return rglist
        End Function

        Public Function GetLocationMasterFilter(ByVal LocArray As List(Of ActiveLocations)) As Object

            Dim retstrg As String = ""

            Try
                For Each item In LocArray
                    If item.status = "False" And item.CID <> "999" Then
                        retstrg = retstrg + " SUBSTRING(lm.CID,4,3) <> '" & item.CID & "' AND "
                    End If

                Next

            Catch ex As Exception

            End Try

            Return retstrg
        End Function
        Public Function GetLocation_InspectionJobSummaryFilter(ByVal LocArray As List(Of ActiveLocations)) As Object

            Dim retstrg As String = ""

            Try
                For Each item In LocArray
                    If item.status = "False" And item.CID <> "999" Then
                        retstrg = retstrg + "AND (InspectionJobSummary.CID <> '" & item.CID & "') "
                    End If

                Next

            Catch ex As Exception

            End Try

            Return retstrg
        End Function
        Public Function getAS400LocationName(ByVal CID As Object) As List(Of SingleObject)
            Dim listso As New List(Of SingleObject)
            Dim bmapso As New BMappers(Of SingleObject)
            Dim str As String = ""
            If IsNumeric(CID) = True Then


                Dim CIDnum As Integer = Convert.ToInt64(CID)

                str = "SELECT LinkedServerMaster.DSN_Identifier AS Object1" & vbCrLf &
                            "FROM LinkedServerMaster INNER JOIN" & vbCrLf &
                            "LocationMaster ON LinkedServerMaster.LocationId = LocationMaster.id" & vbCrLf &
                            "WHERE (LinkedServerMaster.DSN_Type = 'AS400') AND (LocationMaster.CID = N'000" & CIDnum.ToString() & "')"
            End If

            listso = bmapso.GetAprMangObject(str)

            Return listso

        End Function
        Public Function GetREJByLocation(ByVal todate As DateTime, ByVal fromdate As DateTime) As List(Of SPCInspection.LocationLineChart)
            Dim con As New SqlConnection(DL.InspectConnectionString())
            Dim cmd As SqlCommand = con.CreateCommand()
            Dim rglist As New List(Of SPCInspection.LocationLineChart)
            Dim retlist As New List(Of SPCInspection.LocationLineChart)
            Try
                Using con
                    con.Open()
                    Using cmd
                        cmd = New SqlCommand("GetREJByLocation", con)
                        cmd.CommandType = CommandType.StoredProcedure
                        cmd.Parameters.Add("@TODATE", SqlDbType.DateTime).Direction = ParameterDirection.Input
                        cmd.Parameters.Add("@FROMDATE", SqlDbType.DateTime).Direction = ParameterDirection.Input

                        If DateDiff(DateInterval.Day, fromdate, todate) < 7 Then
                            fromdate = todate.AddDays(-7)
                        End If
                        cmd.Parameters("@TODATE").Value = todate
                        cmd.Parameters("@FROMDATE").Value = fromdate.AddDays(-1)

                        cmd.CommandTimeout = 5000

                        Dim bmap_rg As New BMappers(Of SPCInspection.LocationLineChart)
                        rglist = bmap_rg.GetSpcSP(cmd)

                    End Using
                End Using
            Catch ex As Exception
                Return Nothing
            End Try

            Return rglist
        End Function

        Private Function GetAS400Abr(ByVal CID As String) As String
            Dim bmapso As New BMappers(Of SingleObject)
            Dim listso As New List(Of SingleObject)
            Dim retobj As String = ""
            If CID <> "999" Then
                listso = bmapso.GetAprMangObject("SELECT lsm.DSN_Identifier Object1 FROM  LocationMaster lm INNER JOIN  LinkedServerMaster lsm ON lm.id = lsm.LocationId where lm.CID = '000" & CID & "'")

                If listso.Count > 0 Then
                    retobj = listso.ToArray()(0).Object1
                End If
            Else
                retobj = "ALL"
            End If

            Return retobj
        End Function

        Public Function GetRejectRate(ByVal fromdate As DateTime, ByVal todate As DateTime, Locationarray As List(Of ActiveLocations), Optional ByVal DataNo As String = "", Optional ByVal AuditType As String = "") As Object

            Dim listso As New List(Of SingleObject)
            Dim bmapso As New BMappers(Of SingleObject)
            Dim retobj As Double = -1
            Dim DNString As String = ""
            Dim ATString As String = ""
            Dim LOCString As String = ""

            If DataNo.ToUpper().ToString() <> "ALL" And DataNo.ToString().Length > 0 Then
                DNString = " ijs.DataNo = '" & DataNo & "' and "
            End If

            If AuditType.ToUpper().ToString() <> "ALL" And AuditType.ToString().Length > 0 Then
                ATString = " tn.LineType = '" + AuditType + "' and "
            End If

            If Locationarray.ToArray().Length > 0 Then
                LOCString = GetLocationByInspectionSummaryFilter(Locationarray)
            End If

            listso = bmapso.GetInspectObject("select AQL_Level as Object1, case when AQL_Level = '100' THEN (sum(ijs.MajorsCount) + sum(ijs.RepairsCount) + sum(ijs.ScrapCount)) * 100 ELSE (sum(ijs.MajorsCount) + sum(ijs.RepairsCount) + sum(ijs.ScrapCount)) * 100 END as Object2, sum(ijs.TotalInspectedItems) as Object3 FROM InspectionJobSummary ijs INNER JOIN TemplateName tn ON ijs.TemplateId = tn.TemplateId where " + DNString + LOCString + ATString + " ijs.Inspection_Started >= '" & fromdate.ToString("MM/dd/yyyy hh:mm") & "' and ijs.Inspection_Started <= '" & todate.ToString("MM/dd/yyyy hh:mm") & "' and ijs.TotalInspectedItems > 0 group by ijs.AQL_Level")

            If listso.Count > 0 Then
                Dim den As Double = 0
                Dim num As Double = 0

                num = (From v In listso Select v.Object2).Sum()
                den = (From v In listso Select CType(v.Object3, Integer)).Sum()

                If den > 0 Then
                    retobj = num / den
                Else
                    retobj = 0
                End If

            Else
                retobj = 0
            End If

            Return retobj

        End Function

        Public Function GetSingleRejectRate(ByVal fromdate As DateTime, ByVal todate As DateTime, Locationid As String, Optional ByVal DataNo As String = "", Optional ByVal AuditType As String = "") As Object

            Dim listso As New List(Of SingleObject)
            Dim bmapso As New BMappers(Of SingleObject)
            Dim retobj As Double = -1
            Dim DNString As String = ""
            Dim ATString As String = ""
            Dim LOCString As String = ""

            If DataNo.ToUpper().ToString() <> "ALL" And DataNo.ToString().Length > 0 Then
                DNString = " ijs.DataNo = '" & DataNo & "' and "
            End If

            If AuditType.ToUpper().ToString() <> "ALL" And AuditType.ToString().Length > 0 Then
                ATString = " it.Name = '" + AuditType + "' and "
            End If

            If Locationid <> "999" Then
                LOCString = " ijs.CID = '" & Locationid.ToString() & "' and "
            End If


            listso = bmapso.GetInspectObject("select AQL_Level as Object1, case when AQL_Level = '100' THEN (sum(ijs.MajorsCount) + sum(ijs.RepairsCount) + sum(ijs.ScrapCount)) * 100 ELSE (sum(ijs.MajorsCount) + sum(ijs.RepairsCount) + sum(ijs.ScrapCount)) * 100 END as Object2, sum(ijs.TotalInspectedItems) as Object3 FROM InspectionJobSummary ijs INNER JOIN TemplateName tn ON ijs.TemplateId = tn.TemplateId INNER JOIN InspectionTypes it ON it.id = tn.LineTypeId where " + DNString + LOCString + ATString + " ijs.Inspection_Started >= '" & fromdate.ToString("MM/dd/yyyy hh:mm") & "' and ijs.Inspection_Started <= '" & todate.ToString("MM/dd/yyyy hh:mm") & "' and ijs.TotalInspectedItems > 0 group by ijs.AQL_Level")

            If listso.Count > 0 Then
                Dim den As Double = 0
                Dim num As Double = 0

                num = (From v In listso Select v.Object2).Sum()
                den = (From v In listso Select CType(v.Object3, Integer)).Sum()

                If den > 0 Then
                    retobj = num / den
                Else
                    retobj = 0
                End If

            Else
                retobj = 0
            End If

            Return retobj

        End Function

        Public Function GetDHURate(ByVal fromdate As DateTime, ByVal todate As DateTime, Locationarray As List(Of ActiveLocations), Optional ByVal DataNo As String = "", Optional ByVal AuditType As String = "") As Object

            Dim listso As New List(Of SingleObject)
            Dim bmapso As New BMappers(Of SingleObject)
            Dim retobj As Double = -1
            Dim DNString As String = ""
            Dim ATString As String = ""
            Dim LOCString As String = ""

            If DataNo.ToUpper().ToString() <> "ALL" And DataNo.ToString().Length > 0 Then
                DNString = " ijs.DataNo = '" & DataNo & "' and "
            End If

            If AuditType.ToUpper().ToString() <> "ALL" And AuditType.ToString().Length > 0 Then
                ATString = " tn.LineType = '" + AuditType + "' and "
            End If
            If Locationarray.ToArray().Length > 0 Then
                LOCString = GetLocationByInspectionSummaryFilter(Locationarray)
            End If

            listso = bmapso.GetInspectObject("select AQL_Level as Object1, case when AQL_Level = '100' THEN (sum(ijs.MajorsCount) + sum(ijs.MinorsCount) + sum(ijs.RepairsCount) + sum(ijs.ScrapCount)) * 100 ELSE (sum(ijs.MajorsCount)  + sum(ijs.MinorsCount) + sum(ijs.RepairsCount) + sum(ijs.ScrapCount)) * 100 END as Object2, sum(ijs.TotalInspectedItems) as Object3 FROM InspectionJobSummary ijs INNER JOIN TemplateName tn ON ijs.TemplateId = tn.TemplateId INNER JOIN InspectionTypes it ON it.id = tn.LineTypeId where " + DNString + LOCString + ATString + " ijs.Inspection_Started >= '" & fromdate.ToString("MM/dd/yyyy hh:mm") & "' and ijs.Inspection_Started <= '" & todate.ToString("MM/dd/yyyy hh:mm") & "' and ijs.TotalInspectedItems > 0 group by ijs.AQL_Level")

            If listso.Count > 0 Then
                Dim den As Double = 0
                Dim num As Double = 0

                num = (From v In listso Select v.Object2).Sum()
                den = (From v In listso Select CType(v.Object3, Integer)).Sum()

                If den > 0 Then
                    retobj = num / den
                Else
                    retobj = 0
                End If

            Else
                retobj = 0
            End If

            Return retobj

        End Function

        Public Function GetSingleDHURate(ByVal fromdate As DateTime, ByVal todate As DateTime, Locationid As String, Optional ByVal DataNo As String = "", Optional ByVal AuditType As String = "") As Object

            Dim listso As New List(Of SingleObject)
            Dim bmapso As New BMappers(Of SingleObject)
            Dim retobj As Double = -1
            Dim DNString As String = ""
            Dim ATString As String = ""
            Dim LOCString As String = ""

            If DataNo.ToUpper().ToString() <> "ALL" And DataNo.ToString().Length > 0 Then
                DNString = " ijs.DataNo = '" & DataNo & "' and "
            End If

            If AuditType.ToUpper().ToString() <> "ALL" And AuditType.ToString().Length > 0 Then
                ATString = " it.Name = '" + AuditType + "' and "
            End If

            If Locationid <> "999" Then
                LOCString = " ijs.CID = '" & Locationid.ToString() & "' and "
            End If

            listso = bmapso.GetInspectObject("select AQL_Level as Object1, case when AQL_Level = '100' THEN (sum(ijs.MajorsCount) + sum(ijs.MinorsCount) + sum(ijs.RepairsCount) + sum(ijs.ScrapCount)) * 100 ELSE (sum(ijs.MajorsCount)  + sum(ijs.MinorsCount) + sum(ijs.RepairsCount) + sum(ijs.ScrapCount)) * 100 END as Object2, sum(ijs.TotalInspectedItems) as Object3 FROM InspectionJobSummary ijs INNER JOIN TemplateName tn ON ijs.TemplateId = tn.TemplateId INNER JOIN InspectionTypes it ON it.id = tn.LineTypeId where " + DNString + LOCString + ATString + " ijs.Inspection_Started >= '" & fromdate.ToString("MM/dd/yyyy hh:mm") & "' and ijs.Inspection_Started <= '" & todate.ToString("MM/dd/yyyy hh:mm") & "' and ijs.TotalInspectedItems > 0 group by ijs.AQL_Level")

            If listso.Count > 0 Then
                Dim den As Double = 0
                Dim num As Double = 0

                num = (From v In listso Select v.Object2).Sum()
                den = (From v In listso Select CType(v.Object3, Integer)).Sum()

                If den > 0 Then
                    retobj = num / den
                Else
                    retobj = 0
                End If

            Else
                retobj = 0
            End If

            Return retobj

        End Function

        Public Function GetSingleComplianceRate(ByVal fromdate As DateTime, ByVal todate As DateTime, Locationid As String, Optional ByVal DataNo As String = "", Optional ByVal AuditType As String = "") As Object

            Dim listso As New List(Of SingleObject)
            Dim listso1 As New List(Of SingleObject)
            Dim bmapso As New BMappers(Of SingleObject)
            Dim retobj As Double = -1
            Dim DNString As String = ""
            Dim LOCString As String = ""

            If DataNo.ToUpper().ToString() <> "ALL" And DataNo.ToString().Length > 0 Then
                DNString = " ijs.DataNo = '" & DataNo & "' and "
            End If

            If Locationid <> "999" Then
                LOCString = " ijs.CID = '" & Locationid.ToString() & "' and "
            End If

            listso = bmapso.GetInspectObject("select count(ijs.id) as Object1 FROM InspectionJobSummary ijs INNER JOIN TemplateName tn ON ijs.TemplateId = tn.TemplateId INNER JOIN InspectionTypes it ON it.id = tn.LineTypeId where " + DNString + LOCString + " ijs.Inspection_Started >= '" & fromdate.ToString("MM/dd/yyyy hh:mm") & "' and it.Name = 'FINAL AUDIT' and ijs.Inspection_Started <= '" & todate.ToString("MM/dd/yyyy hh:mm") & "' and ijs.TotalInspectedItems > 0 ")

            If listso.Count > 0 Then

                listso1 = bmapso.GetInspectObject("select count(ijs.id) as Object1 FROM InspectionJobSummary ijs INNER JOIN TemplateName tn ON ijs.TemplateId = tn.TemplateId INNER JOIN InspectionTypes it ON it.id = tn.LineTypeId where " + DNString + LOCString + " ijs.Inspection_Started >= '" & fromdate.ToString("MM/dd/yyyy hh:mm") & "' and it.Name <> 'ROLL' and ijs.Inspection_Started <= '" & todate.ToString("MM/dd/yyyy hh:mm") & "' and ijs.TotalInspectedItems > 0 ")
                If listso1.Count > 0 Then

                    Dim den As Double = 0
                    Dim num As Double = 0

                    num = listso.ToArray()(0).Object1
                    den = listso1.ToArray()(0).Object1

                    If den > 0 Then
                        retobj = (num / den) * 100
                    Else
                        retobj = 0
                    End If
                End If

            Else
                retobj = 0
            End If

            Return retobj

        End Function

        Public Function GetComplianceRate(ByVal fromdate As DateTime, ByVal todate As DateTime, Locationarray As List(Of ActiveLocations), Optional FilterAction As Boolean = False, Optional ByVal DataNo As String = "", Optional ByVal AuditType As String = "") As Object

            Dim listic As New List(Of SPCInspection.InspectionCompliance_Local)
            Dim listijs As New List(Of SingleObject)
            Dim bmapijs As New BMappers(Of SingleObject)
            Dim retobj As Double = 0
            listijs = bmapijs.GetInspectObject("SELECT TOP(1) id as Object1 FROM InspectionJobSummary order by id desc")


            Dim cachstring As String = "InspectionCompliance_Local_ijsid_" + todate.ToString("yyyy.MM.dd") + "." + fromdate.ToString("yyyy.MM.dd")
            Dim Cachedijsid As Object = HttpRuntime.Cache(cachstring)
            If Not Cachedijsid Is Nothing And listijs.Count > 0 Then
                If Cachedijsid = listijs.ToArray()(0).Object1 Then
                    listic = HttpRuntime.Cache("InspectionCompliance_Local" + todate.ToString("yyyy.MM.dd") + "." + fromdate.ToString("yyyy.MM.dd"))
                End If
            End If
            If listic.Count = 0 Then
                listic = Getas400WOInspectionCompliance(fromdate, todate)
                If FilterAction = False Then
                    Dim cachic As New List(Of SPCInspection.InspectionCompliance_Local)
                    cachic = listic
                    InsertComplianceIntoCache(cachic, fromdate, todate, listijs.ToArray()(0).Object1)
                End If
            End If

            If Not listic Is Nothing Then
                If listic.Count > 0 Then
                    Dim locoff = (From v In Locationarray Where v.status = "False" And v.CID <> "999" Select v).ToList()
                    If Not locoff Is Nothing Then
                        If locoff.Count > 0 Then
                            For Each item In locoff
                                Dim a400abr As String = GetAS400Abr(item.CID)
                                If a400abr.Length > 0 Then
                                    listic = (From v In listic Where v.Location.Trim() <> a400abr.Trim() Select v).ToList()
                                End If
                            Next
                        End If
                    End If
                End If
            End If


            If Not listic Is Nothing Then
                If listic.Count > 0 Then
                    retobj = ReturnCompRate(listic, AuditType, DataNo)
                End If
            End If

            Return retobj


        End Function

        Private Function ReturnCompRate(ByVal listin As List(Of SPCInspection.InspectionCompliance_Local), ByVal AuditType As String, ByVal DataNo As String) As Double
            Dim retobj As Double = 0
            Dim listnew As New List(Of SPCInspection.InspectionCompliance_Local)
            Dim den As Double = 1
            Dim num As Double = 0


            If AuditType = "ALL" And DataNo = "ALL" Then

                den = (From v In listin Select v).Count()
                num = (From v In listin Where v.ijsid.ToString().Length > 0 Select v).Count()

            ElseIf AuditType = "ALL" And DataNo <> "ALL" Then

                den = (From v In listin Select v).Count()
                num = (From v In listin Where v.ijsid.ToString().Length > 0 And v.DataNo.Trim() = DataNo.Trim() Select v).Count()

            ElseIf DataNo = "ALL" And AuditType <> "ALL" Then

                den = (From v In listin Select v).Count()
                num = (From v In listin Where v.ijsid.ToString().Length > 0 And v.LineType = AuditType.Trim() Select v).Count()

            End If

            retobj = (num / den) * 100

            Return retobj
        End Function

        Private Sub InsertComplianceIntoCache(ByVal listic As List(Of SPCInspection.InspectionCompliance_Local), ByVal fromdate As DateTime, ByVal todate As DateTime, lastijsid As Integer)

            If Not listic Is Nothing Then
                If listic.Count > 0 Then
                    HttpRuntime.Cache.Insert("InspectionCompliance_Local" + todate.ToString("yyyy.MM.dd") + "." + fromdate.ToString("yyyy.MM.dd"), listic, Nothing, Date.Now.AddDays(3), System.Web.Caching.Cache.NoSlidingExpiration)
                    If IsNumeric(lastijsid) Then
                        HttpRuntime.Cache.Insert("InspectionCompliance_Local_ijsid_" + todate.ToString("yyyy.MM.dd") + "." + fromdate.ToString("yyyy.MM.dd"), lastijsid, Nothing, Date.Now.AddDays(3), System.Web.Caching.Cache.NoSlidingExpiration)
                    End If
                End If
            End If

        End Sub

        Public Function GetLocationByInspectionSummaryFilter(ByVal LocArray As List(Of ActiveLocations)) As Object

            Dim retstrg As String = ""

            Try
                For Each item In LocArray
                    If item.status = "False" And item.CID <> "999" Then
                        retstrg = retstrg + " ijs.CID <> '" & item.CID & "' AND "
                    End If

                Next

            Catch ex As Exception

            End Try

            Return retstrg
        End Function
        Public Function LocationsFilter_DefectImage(ByVal LocArray As List(Of ActiveLocations)) As String
            Dim retstrg As String = ""

            Try
                For Each item In LocArray
                    If item.status = "False" And item.CID <> "999" And IsNothing(item.ProdAbreviation) = False Then
                        retstrg = retstrg + " AND RTRIM(LTRIM(ijs.CID)) <> '" & item.CID.ToString() & "' "
                    End If

                Next

            Catch ex As Exception

            End Try

            Return retstrg


        End Function
        Public Function GetInspectionComplianceLocationFilter(ByVal LocArray As List(Of ActiveLocations)) As String
            Dim retstrg As String = ""

            Try
                For Each item In LocArray
                    If item.status = "False" And item.CID <> "999" And IsNothing(item.ProdAbreviation) = False Then
                        retstrg = retstrg + " AND RTRIM(LTRIM(ic.Location)) <> '" & item.ProdAbreviation & "' "
                    End If

                Next

            Catch ex As Exception

            End Try

            Return retstrg

        End Function

        Public Function GetInspectionCompliancePrpCodeFilter(ByVal FiltArray As List(Of ActiveFilterObject)) As String
            Dim retstrg As String = "AND ( "
            Dim FieldCnt As Int16 = 0
            Try
                If Not FiltArray Is Nothing Then
                    For Each item In FiltArray
                        If item.Name = "pf_prp" Then
                            If FieldCnt = 0 Then
                                retstrg = retstrg + " RTRIM(LTRIM(ic.Prp_Code)) = '" & item.value & "' "
                            Else
                                retstrg = retstrg + " OR RTRIM(LTRIM(ic.Prp_Code)) = '" & item.value & "' "
                            End If

                            FieldCnt += 1
                        End If

                    Next
                    retstrg = retstrg + " )"
                End If

            Catch ex As Exception
                FieldCnt = 0
            End Try

            If FieldCnt = 0 Then
                retstrg = ""
            End If

            Return retstrg

        End Function

        Public Function GetInspectionCompliancePrpFilter(filterlist As List(Of ActiveFilterObject)) As String
            Dim retstrg As String = ""

            Try
                For Each item In filterlist
                    If item.Name = "pf_prp" Then
                        retstrg = retstrg + " and ic.Prp_Code = '''" & item.value & "''' "
                    End If

                Next

            Catch ex As Exception

            End Try

            Return retstrg

        End Function

        Public Function GetLotAcc(ByVal fromdate As DateTime, ByVal todate As DateTime, Locationarray As List(Of ActiveLocations), Optional ByVal DataNo As String = "", Optional ByVal AuditType As String = "") As Object
            Dim listso As New List(Of SingleObject)
            Dim bmapso As New BMappers(Of SingleObject)
            Dim retobj As Double = -1
            Dim DNString As String = ""
            Dim ATString As String = ""
            Dim LOCString As String = ""

            If DataNo.ToUpper().ToString() <> "ALL" And DataNo.ToString().Length > 0 Then
                DNString = " ijs.DataNo = '" & DataNo & "' and "
            End If

            If AuditType.ToUpper().ToString() <> "ALL" And AuditType.ToString().Length > 0 Then
                ATString = " it.Name = '" + AuditType + "' and "
            End If

            If Locationarray.ToArray().Length > 0 Then
                LOCString = GetLocationByInspectionSummaryFilter(Locationarray)
            End If

            listso = bmapso.GetInspectObject("select 100 * cast((SELECT COUNT(*) FROM InspectionJobSummary ijs INNER JOIN TemplateName tn ON ijs.TemplateId = tn.TemplateId INNER JOIN InspectionTypes it ON it.id = tn.LineTypeId WHERE " + LOCString + DNString + " ijs.Technical_PassFail = 1 AND ijs.Inspection_Started >= '" & fromdate.ToString("MM/dd/yyyy hh:mm") & "' AND ijs.Inspection_Started < '" & todate.ToString("MM/dd/yyyy hh:mm") & "') as float)/cast((SELECT COUNT(*) FROM InspectionJobSummary ijs INNER JOIN TemplateName tn ON ijs.TemplateId = tn.TemplateId INNER JOIN InspectionTypes it ON it.id = tn.LineTypeId WHERE " + LOCString + DNString + " ijs.Inspection_Started >= '" & fromdate.ToString("MM/dd/yyyy hh:mm") & "' AND ijs.Inspection_Started < '" & todate.ToString("MM/dd/yyyy hh:mm") & "') as float) as Object1")

            If listso.Count > 0 Then
                retobj = listso.ToArray()(0).Object1
            ElseIf listso.Count = 0 Then
                retobj = 0
            End If

            Return retobj

        End Function

        Public Function GetSingleLotAcc(ByVal fromdate As DateTime, ByVal todate As DateTime, Locationid As String, Optional ByVal DataNo As String = "", Optional ByVal AuditType As String = "") As Object
            Dim listso As New List(Of SingleObject)
            Dim bmapso As New BMappers(Of SingleObject)
            Dim retobj As Double = -1
            Dim DNString As String = ""
            Dim ATString As String = ""
            Dim LOCString As String = ""

            If DataNo.ToUpper().ToString() <> "ALL" And DataNo.ToString().Length > 0 Then
                DNString = " ijs.DataNo = '" & DataNo & "' and "
            End If

            If AuditType.ToUpper().ToString() <> "ALL" And AuditType.ToString().Length > 0 Then
                ATString = " tn.LineType = '" + AuditType + "' and "
            End If

            If Locationid <> "999" Then
                LOCString = "ijs.CID = '" + Locationid + "' and "
            End If

            listso = bmapso.GetInspectObject("select 100 * cast((SELECT COUNT(*) FROM InspectionJobSummary ijs INNER JOIN TemplateName tn ON ijs.TemplateId = tn.TemplateId WHERE " + LOCString + DNString + " ijs.Technical_PassFail = 1 AND ijs.Inspection_Started >= '" & fromdate.ToString("MM/dd/yyyy hh:mm") & "' AND ijs.Inspection_Started < '" & todate.ToString("MM/dd/yyyy hh:mm") & "') as float)/cast((SELECT COUNT(*) FROM InspectionJobSummary ijs INNER JOIN TemplateName tn ON ijs.TemplateId = tn.TemplateId WHERE " + LOCString + DNString + " ijs.Inspection_Started >= '" & fromdate.ToString("MM/dd/yyyy hh:mm") & "' AND ijs.Inspection_Started < '" & todate.ToString("MM/dd/yyyy hh:mm") & "') as float) as Object1")

            If listso.Count > 0 Then
                retobj = listso.ToArray()(0).Object1
            ElseIf listso.Count = 0 Then
                retobj = 0
            End If

            Return retobj

        End Function

        Public Function GetCompliance_FilterDN(ByVal listwoc As List(Of SPCInspection.WorkOrderCompliance), ByVal DataNumber As String) As Double
            Dim retobj As Double = 0
            Dim num As Object = 0
            Dim den As Object = 0

            If listwoc.Count > 0 Then

                num = (From v In listwoc Where v.Match = True And v.DataNo = DataNumber Select v).Count()
                den = (From v In listwoc Where v.DataNo = DataNumber Select v).Count()

            End If

            If den > 0 Then
                retobj = (num / den) * 100
            End If

            Return retobj
        End Function
        
        Public Function GetMatchPercLocal(ByVal listwoc As List(Of SPCInspection.WorkOrderCompliance), ByVal fromdatein As DateTime, ByVal todatein As DateTime, ByVal CID As String, Optional ByVal ActiveFilterField As Object = Nothing, Optional ByVal ActiveValue As Object = Nothing) As Double

            Dim listwocapr As New List(Of SPCInspection.WorkOrderCompliance)
            Dim retobj As Double = 0
            Dim num As Object = 0
            Dim den As Object = 0
            If listwoc.Count > 0 Then
                If CID <> "999" Then
                    Dim listls As New List(Of SingleObject)
                    Dim bmapso As New BMappers(Of SingleObject)

                    listls = bmapso.GetAprMangObject("SELECT DSN_Identifier as Object1 FROM LinkedServerMaster lsm inner join LocationMaster lm ON lsm.LocationId = lm.id WHERE lm.CID = '000" & CID & "'")

                    If IsNothing(ActiveFilterField) = False And IsNothing(ActiveValue) = False Then
                        If ActiveValue <> "ALL" Then
                            Dim listwoc2 As New List(Of SPCInspection.WorkOrderCompliance)
                            listwoc2 = listwoc
                            listwoc.Clear()
                            Select Case ActiveValue
                                Case "pf_AuditType"
                                    listwoc = (From v In listwoc2 Where v.LineType = ActiveValue Select v).ToList()
                                Case "pf_DataNumber"
                                    listwoc = (From v In listwoc2 Where v.DataNo = ActiveValue Select v).ToList()
                            End Select
                        End If
                    End If
                    If listls.Count > 0 Then
                        Select Case listls.Count
                            Case 1
                                num = (From v In listwoc Where v.Branch.Trim() = listls.ToArray()(0).Object1 And v.Match = True Select v).Count()
                                den = (From v In listwoc Where v.Branch.Trim() = listls.ToArray()(0).Object1 Select v).Count()
                            Case 2
                                num = (From v In listwoc Where v.Branch.Trim() = listls.ToArray()(0).Object1 Or v.Branch.Trim() = listls.ToArray()(1).Object1 And v.Match = True Select v).Count()
                                den = (From v In listwoc Where v.Branch.Trim() = listls.ToArray()(0).Object1 Or v.Branch.Trim() = listls.ToArray()(1).Object1 Select v).Count()
                            Case 3
                                num = (From v In listwoc Where v.Branch.Trim() = listls.ToArray()(0).Object1 Or v.Branch.Trim() = listls.ToArray()(1).Object1 Or v.Branch.Trim() = listls.ToArray()(2).Object1 And v.Match = True Select v).Count()
                                den = (From v In listwoc Where v.Branch.Trim() = listls.ToArray()(0).Object1 Or v.Branch.Trim() = listls.ToArray()(1).Object1 Or v.Branch.Trim() = listls.ToArray()(2).Object1 Select v).Count()
                        End Select
                    End If
                Else
                    num = (From v In listwoc Where v.Match = True Select v).Count()
                    den = (From v In listwoc Select v).Count()
                End If
            End If

            If den > 0 Then
                retobj = (num / den) * 100
            End If

            Return retobj

        End Function
        Public Function Getas400MatchPerc(ByVal fromdate As DateTime, ByVal todate As DateTime, ByVal LocationId As Integer) As Double
            Dim con As New SqlConnection(DL.InspectConnectionString())
            Dim cmd As SqlCommand = con.CreateCommand()
            Dim rglist As Double = -1

            Try
                Using con
                    con.Open()
                    Using cmd
                        cmd = New SqlCommand("SP_AS400_GetMatchPerc", con)
                        cmd.CommandType = CommandType.StoredProcedure
                        cmd.Parameters.Add("@FROMDATE", SqlDbType.DateTime).Direction = ParameterDirection.Input
                        cmd.Parameters.Add("@TODATE", SqlDbType.DateTime).Direction = ParameterDirection.Input
                        cmd.Parameters.Add("@BRANCH", SqlDbType.VarChar).Direction = ParameterDirection.Input
                        cmd.Parameters.Add("@CID", SqlDbType.VarChar).Direction = ParameterDirection.Input
                        cmd.Parameters.Add("@MATCHPERC", SqlDbType.Float).Direction = ParameterDirection.Output
                        cmd.Parameters("@FROMDATE").Value = fromdate
                        cmd.Parameters("@TODATE").Value = todate
                        cmd.Parameters("@BRANCH").Value = GetAS400Abr(LocationId.ToString())
                        cmd.Parameters("@CID").Value = LocationId.ToString()
                        cmd.CommandTimeout = 5000

                        Dim bmap_rg As New BMappers(Of SPCInspection.WorkOrderCompliance)
                        cmd.ExecuteReader(CommandBehavior.CloseConnection)

                        rglist = cmd.Parameters("@MATCHPERC").Value

                    End Using
                End Using
            Catch ex As Exception
                Return Nothing
            End Try

            Return rglist
        End Function


        Public Function Getas400WOByBranch(ByVal fromdate As DateTime, ByVal todate As DateTime, ByVal LocationId As Integer) As List(Of SPCInspection.WorkOrderCompliance)
            Dim con As New SqlConnection(DL.InspectConnectionString())
            Dim cmd As SqlCommand = con.CreateCommand()
            Dim rglist As New List(Of SPCInspection.WorkOrderCompliance)

            Try
                Using con
                    con.Open()
                    Using cmd
                        cmd = New SqlCommand("SP_AS400_GETWOBYBRANCH", con)
                        cmd.CommandType = CommandType.StoredProcedure
                        cmd.Parameters.Add("@FROMDATE", SqlDbType.DateTime).Direction = ParameterDirection.Input
                        cmd.Parameters.Add("@TODATE", SqlDbType.DateTime).Direction = ParameterDirection.Input
                        cmd.Parameters.Add("@BRANCH", SqlDbType.VarChar).Direction = ParameterDirection.Input
                        cmd.Parameters("@FROMDATE").Value = fromdate
                        cmd.Parameters("@TODATE").Value = todate
                        cmd.Parameters("@BRANCH").Value = GetAS400Abr(LocationId.ToString())

                        cmd.CommandTimeout = 5000

                        Dim bmap_rg As New BMappers(Of SPCInspection.WorkOrderCompliance)
                        rglist = bmap_rg.GetSpcSP(cmd)

                    End Using
                End Using
            Catch ex As Exception
                Return Nothing
            End Try

            Return rglist
        End Function

        Public Function Getas400WOInspectionCompliance(ByVal fromdate As DateTime, ByVal todate As DateTime) As List(Of SPCInspection.InspectionCompliance_Local)
            Dim con As New SqlConnection(DL.InspectConnectionString())
            Dim cmd As SqlCommand = con.CreateCommand()
            Dim rglist As New List(Of SPCInspection.InspectionCompliance_Local)

            Try
                Using con
                    con.Open()
                    Using cmd
                        cmd = New SqlCommand("GetInspectionComplianceData", con)
                        cmd.CommandType = CommandType.StoredProcedure
                        cmd.Parameters.Add("@FROMDATE", SqlDbType.DateTime).Direction = ParameterDirection.Input
                        cmd.Parameters.Add("@TODATE", SqlDbType.DateTime).Direction = ParameterDirection.Input
                        cmd.Parameters("@FROMDATE").Value = fromdate
                        cmd.Parameters("@TODATE").Value = todate

                        cmd.CommandTimeout = 5000

                        ' Dim bmap_rg As New BMappers(Of SPCInspection.InspectionCompliance_Local)
                        rglist = _DAOFactory.getInspectionCompliance(cmd)
                        'rglist = bmap_rg.GetSpcSP(cmd)

                    End Using
                End Using
            Catch ex As Exception
                Return Nothing
            End Try

            Return rglist
        End Function

        Public Function GetInspectionCompliancePerc(ByVal fromdate As DateTime, ByVal todate As DateTime, audittype As String, datano As String, prpcode As String, Optional Locations As String = "") As Object
            Dim con As New SqlConnection(DL.InspectConnectionString())
            Dim cmd As SqlCommand = con.CreateCommand()
            Dim rglist As New List(Of SingleObject)

            Try
                Using con
                    con.Open()
                    Using cmd
                        cmd = New SqlCommand("GetInspectionCompliancePerc", con)
                        cmd.CommandType = CommandType.StoredProcedure
                        cmd.Parameters.Add("@FROMDATE", SqlDbType.DateTime).Direction = ParameterDirection.Input
                        cmd.Parameters.Add("@TODATE", SqlDbType.DateTime).Direction = ParameterDirection.Input
                        cmd.Parameters.Add("@AUDITTYPE", SqlDbType.VarChar).Direction = ParameterDirection.Input
                        cmd.Parameters.Add("@DATANO", SqlDbType.VarChar).Direction = ParameterDirection.Input
                        cmd.Parameters.Add("@PRPCODE", SqlDbType.VarChar, Int16.MaxValue).Direction = ParameterDirection.Input
                        cmd.Parameters.Add("@LocationStr", SqlDbType.VarChar, Int16.MaxValue).Direction = ParameterDirection.Input
                        '  cmd.Parameters.Add("@location", SqlDbType.VarChar).Direction = ParameterDirection.Input
                        cmd.Parameters("@FROMDATE").Value = fromdate
                        cmd.Parameters("@TODATE").Value = todate
                        cmd.Parameters("@AUDITTYPE").Value = audittype
                        cmd.Parameters("@DATANO").Value = datano
                        cmd.Parameters("@PRPCODE").Value = prpcode
                        cmd.Parameters("@LocationStr").Value = Locations
                        '  cmd.Parameters("@location").Value = GetAS400Abr(LocationId.ToString())

                        cmd.CommandTimeout = 5000

                        Dim bmap_rg As New BMappers(Of SingleObject)
                        rglist = bmap_rg.GetSpcSP(cmd)

                    End Using
                End Using
            Catch ex As Exception
                Return Nothing
            End Try
            If IsNothing(rglist) = False Then
                If rglist.Count > 0 Then
                    Return rglist.ToArray()(0).Object1
                End If
            End If
            Return -1
        End Function

        Public Function GetInspectionComplianceData(ByVal fromdate As DateTime, ByVal todate As DateTime) As List(Of SPCInspection.InspectionCompliance_Local)
            Dim con As New SqlConnection(DL.InspectConnectionString())
            Dim cmd As SqlCommand = con.CreateCommand()
            Dim rglist As New List(Of SPCInspection.InspectionCompliance_Local)

            Try
                Using con
                    con.Open()
                    Using cmd
                        cmd = New SqlCommand("GetInspectionComplianceData", con)
                        cmd.CommandType = CommandType.StoredProcedure
                        cmd.Parameters.Add("@FROMDATE", SqlDbType.DateTime).Direction = ParameterDirection.Input
                        cmd.Parameters.Add("@TODATE", SqlDbType.DateTime).Direction = ParameterDirection.Input
                        cmd.Parameters("@FROMDATE").Value = fromdate
                        cmd.Parameters("@TODATE").Value = todate

                        cmd.CommandTimeout = 5000

                        Dim bmap_rg As New BMappers(Of SPCInspection.InspectionCompliance_Local)
                        rglist = bmap_rg.GetSpcSP(cmd)

                    End Using
                End Using
            Catch ex As Exception
                Return Nothing
            End Try

            Return rglist


        End Function

        Public Function Getas400WOMatchBranch(ByVal fromdate As DateTime, ByVal todate As DateTime, as400location As String) As List(Of SPCInspection.InspectionCompliance)
            Dim con As New SqlConnection(DL.InspectConnectionString())
            Dim cmd As SqlCommand = con.CreateCommand()
            Dim rglist As New List(Of SPCInspection.InspectionCompliance)
            Try
                Using con
                    con.Open()
                    Using cmd
                        cmd = New SqlCommand("SP_AS400_InspectionCompliance", con)
                        cmd.CommandType = CommandType.StoredProcedure
                        cmd.Parameters.Add("@FROMDATE", SqlDbType.DateTime).Direction = ParameterDirection.Input
                        cmd.Parameters.Add("@TODATE", SqlDbType.DateTime).Direction = ParameterDirection.Input
                        cmd.Parameters.Add("@location", SqlDbType.VarChar, 250).Direction = ParameterDirection.Input
                        cmd.Parameters("@FROMDATE").Value = fromdate
                        cmd.Parameters("@TODATE").Value = todate
                        cmd.Parameters("@location").Value = as400location

                        cmd.CommandTimeout = 5000

                        Dim bmap_rg As New BMappers(Of SPCInspection.InspectionCompliance)
                        rglist = bmap_rg.GetSpcSP(cmd)

                    End Using
                End Using
            Catch ex As Exception
                Return Nothing
            End Try

            Return rglist
        End Function

        Public Function GetDefectMasterMonthlySum(ByVal Location As String, ByVal fromdate As DateTime, ByVal todate As DateTime) As List(Of SPCInspection.GraphTable)
            Dim con As New SqlConnection(DL.InspectConnectionString())
            Dim cmd As SqlCommand = con.CreateCommand()
            Dim todatestring As String = todate.ToString("yyyy-MM-dd H:mm:ss").Split(" ")(0) + " 23:59:59"
            Dim todateform As DateTime = DateTime.Parse(todatestring)
            Try
                Using con
                    con.Open()
                    Using cmd
                        cmd = New SqlCommand("SP_DefectMasterMonthlySum", con)
                        cmd.CommandType = CommandType.StoredProcedure

                        cmd.Parameters.Add("@fromdate", SqlDbType.DateTime).Direction = ParameterDirection.Input
                        cmd.Parameters.Add("@todate", SqlDbType.DateTime).Direction = ParameterDirection.Input
                        cmd.Parameters.Add("@Location", SqlDbType.NChar).Direction = ParameterDirection.Input
                        cmd.Parameters("@fromdate").Value = fromdate
                        cmd.Parameters("@todate").Value = todateform
                        cmd.Parameters("@Location").Value = Location
                        cmd.CommandTimeout = 5000

                        Dim list As New List(Of SPCInspection.GraphTable)
                        Dim wocheckarray As Array = _DAOFactory.GetDefectMasterDailyObjects(cmd, con).ToArray()
                        If wocheckarray.Length > 0 Then
                            If wocheckarray.Length > 0 Then
                                '       list.Add(New SPCInspection.GraphTable With {.Count = wocheckarray(0).Count, .Listdate = New DateTime(wocheckarray(0).Year, wocheckarray(0).Month, 1)})
                                '        Return list
                                '     Else
                                Dim startDate As DateTime = New DateTime(fromdate.Year, wocheckarray(0).Month, 1)
                                Dim endate As DateTime = todate
                                Dim M As Integer = Math.Abs((todate.Year - startDate.Year))

                                Dim elapsedTicks As Long = todate.Ticks - fromdate.Ticks
                                Dim elapsedSpan As New TimeSpan(elapsedTicks)
                                Dim Thismonth As DateTime = New DateTime(fromdate.Year, fromdate.Month, todate.Day)
                                While Thismonth <= endate
                                    Dim returnlistcnt = list.Count
                                    Dim indexflag As Boolean = False
                                    Dim valueflag As Boolean = False
                                    'Dim Listdate As DateTime = fromdate.AddMonths(i)
                                    Dim Listformdate As DateTime = New DateTime(Thismonth.Year, Thismonth.Month, 1)

                                    For Each item In wocheckarray
                                        Dim pointdate As DateTime = New DateTime(item.Year, item.Month, 1)
                                        If Listformdate = pointdate Then
                                            list.Add(New SPCInspection.GraphTable With {.Count = item.Count, .Listdate = pointdate.ToString("yyyy-MM-dd")})
                                            GoTo 101
                                        End If
                                    Next
                                    list.Add(New SPCInspection.GraphTable With {.Count = 0, .Listdate = Listformdate.ToString("yyyy-MM-dd")})

101:
                                    Thismonth = Thismonth.AddMonths(1)
                                End While
                            End If

                            Return list
                        Else
                            Return Nothing
                        End If

                    End Using
                    con.Close()
                End Using
            Catch ex As Exception
                Return Nothing
            End Try



        End Function


        Public Function GetDefectPieChart(ByVal Location As String, ByVal fromdate As DateTime, ByVal todate As DateTime, ByVal GroupBy As String) As List(Of PieChart)
            Dim con As New SqlConnection(DL.InspectConnectionString())
            Dim cmd As SqlCommand = con.CreateCommand()
            Try
                Using con
                    con.Open()
                    Using cmd
                        cmd = New SqlCommand("SP_DefectPieChart", con)
                        cmd.CommandType = CommandType.StoredProcedure
                        cmd.Parameters.Add("@fromdate", SqlDbType.DateTime).Direction = ParameterDirection.Input
                        cmd.Parameters.Add("@todate", SqlDbType.DateTime).Direction = ParameterDirection.Input
                        cmd.Parameters.Add("@GroupBy", SqlDbType.VarChar).Direction = ParameterDirection.Input
                        cmd.Parameters.Add("@Location", SqlDbType.NChar).Direction = ParameterDirection.Input
                        cmd.Parameters("@fromdate").Value = fromdate
                        cmd.Parameters("@todate").Value = todate
                        cmd.Parameters("@Location").Value = Location
                        cmd.Parameters("@GroupBy").Value = GroupBy
                        cmd.CommandTimeout = 5000

                        Dim list As New List(Of PieChart)
                        Dim Colors As String() = {"rgb(93, 135, 161)", "rgb(176, 181, 121)", "rgb(251, 176, 64)", "rgb(149, 160, 169)", "rgb(211, 18, 69)", "rgb(255, 222, 117)", "rgb(233, 227, 220)"}
                        Dim wocheckarray As Array = _DAOFactory.GetPieChartData(cmd, con).ToArray()
                        Dim othercnt As Integer = 1
                        Dim cnt As Integer = 0
                        If wocheckarray.Length > 0 Then
                            For Each item In wocheckarray
                                If IsNothing(item.desc) = True Or item.desc = "" Then
                                    item.desc = "Other_" + othercnt.ToString()
                                    othercnt += 1
                                End If
                                If Colors.Length < cnt + 1 Then
                                    GoTo 102
                                End If
                                If GroupBy = "DefectDesc" And item.desc = "NoDefect" Then
                                    GoTo 102
                                End If
                                list.Add(New PieChart With {.value = item.value, .label = item.desc, .color = Colors(cnt)})
                                cnt += 1
102:
                            Next

                            list.Sort(Function(x As PieChart, y As PieChart)
                                          If x.value = 0 AndAlso y.value = 0 Then
                                              Return 0
                                          ElseIf x.value = 0 Then
                                              Return -1
                                          ElseIf y.value = 0 Then
                                              Return 1
                                          Else
                                              Return x.value.CompareTo(y.value)
                                          End If
                                      End Function)
                            If Colors.Length < list.Count Then
                                Dim count = list.Count - Colors.Length
                                For i = 0 To count - 1
                                    list.RemoveAt(i)
                                Next
                            End If
                        End If

                        Return list
                    End Using
                    con.Close()
                End Using

            Catch ex As Exception
                Return Nothing
            End Try

        End Function

        Public Function GetDefectMasterDataTypeCount(ByVal Location As Integer, ByVal fromdate As DateTime, ByVal todate As DateTime, ByVal DataType As String) As Integer
            Dim con As New SqlConnection(DL.InspectConnectionString)
            Dim cmd As SqlCommand = con.CreateCommand()
            Try
                Using con
                    con.Open()
                    Using cmd
                        cmd = New SqlCommand("SP_CountDefectMasterDataType", con)
                        cmd.CommandType = CommandType.StoredProcedure

                        cmd.Parameters.Add("@fromdate", SqlDbType.DateTime).Direction = ParameterDirection.Input
                        cmd.Parameters.Add("@todate", SqlDbType.DateTime).Direction = ParameterDirection.Input
                        cmd.Parameters.Add("@DataType", SqlDbType.VarChar).Direction = ParameterDirection.Input
                        cmd.Parameters.Add("@LOCATIONID", SqlDbType.VarChar).Direction = ParameterDirection.Input
                        cmd.Parameters("@fromdate").Value = fromdate
                        cmd.Parameters("@todate").Value = todate
                        cmd.Parameters("@DataType").Value = DataType
                        cmd.Parameters("@LOCATIONID").Value = Location
                        cmd.CommandTimeout = 5000

                        Dim list As New List(Of SPCInspection.GraphTable)
                        Dim wocheckarray As Array = _DAOFactory.GetDefectMasterDataTypeCount(cmd, con).ToArray()
                        If IsNumeric(wocheckarray(0)) = True Then
                            Return wocheckarray(0)
                        Else
                            Return 0
                        End If

                    End Using
                    con.Close()
                End Using
            Catch ex As Exception
                Return Nothing
            End Try

        End Function

        Public Function GetDefectMasterHistogram(ByVal Location As String, ByVal fromdate As DateTime, ByVal todate As DateTime) As List(Of SPCInspection.BarChart)
            Dim con As New SqlConnection(DL.InspectConnectionString)
            Dim cmd As SqlCommand = con.CreateCommand()
            Dim returnlist As New List(Of SPCInspection.BarChart)
            Try
                Using con
                    con.Open()
                    Using cmd
                        cmd = New SqlCommand("SP_DefectMasterHistogram", con)
                        cmd.CommandType = CommandType.StoredProcedure
                        cmd.Parameters.Add("@fromdate", SqlDbType.DateTime).Direction = ParameterDirection.Input
                        cmd.Parameters.Add("@todate", SqlDbType.DateTime).Direction = ParameterDirection.Input
                        cmd.Parameters.Add("@Location", SqlDbType.NChar).Direction = ParameterDirection.Input
                        cmd.Parameters("@fromdate").Value = fromdate
                        cmd.Parameters("@todate").Value = todate
                        cmd.Parameters("@Location").Value = Location
                        cmd.CommandTimeout = 5000

                        Dim list As New List(Of SPCInspection.GraphTable)
                        returnlist = _DAOFactory.GetDefectMasterHistogram(cmd, con)

                    End Using
                    con.Close()
                End Using
            Catch ex As Exception
                Return Nothing
            End Try

            Return returnlist
        End Function

        Public Function GetJobSummary_1(ByVal Location As String, ByVal daysback As Integer, ByVal todate As DateTime, ByVal TemplateId As Integer) As List(Of SPCInspection.JobSummary)
            Dim con As New SqlConnection(DL.InspectConnectionString)
            Dim cmd As SqlCommand = con.CreateCommand()
            Dim list As New List(Of SPCInspection.JobSummary)
            Try
                Using con
                    con.Open()
                    Using cmd
                        cmd = New SqlCommand("SP_SPC_GetJobSummary_1", con)
                        cmd.CommandType = CommandType.StoredProcedure

                        cmd.Parameters.Add("@TEMPLATEID", SqlDbType.Int).Direction = ParameterDirection.Input
                        cmd.Parameters.Add("@DAYSBACK", SqlDbType.Int).Direction = ParameterDirection.Input
                        cmd.Parameters.Add("@TODATE", SqlDbType.DateTime).Direction = ParameterDirection.Input
                        cmd.Parameters.Add("@LOCATION", SqlDbType.VarChar).Direction = ParameterDirection.Input
                        cmd.Parameters("@TEMPLATEID").Value = TemplateId
                        cmd.Parameters("@DAYSBACK").Value = daysback
                        cmd.Parameters("@TODATE").Value = todate
                        cmd.Parameters("@LOCATION").Value = Location
                        cmd.CommandTimeout = 5000


                        Dim bmapjs As New BMappers(Of SPCInspection.JobSummary)

                        list = bmapjs.GetSpcSP(cmd)

                        If list.Count > 0 Then
                            Return list
                        Else
                            Return list
                        End If

                    End Using
                    con.Close()
                End Using
            Catch ex As Exception
                Return list
            End Try

        End Function
        Public Function GetJobSummary_2(ByVal Location As String, ByVal daysback As Integer, ByVal todate As DateTime, ByVal TemplateId As Integer) As List(Of Production.JobSummary_DBreakdown)
            Dim con As New SqlConnection(DL.InspectConnectionString)
            Dim cmd As SqlCommand = con.CreateCommand()
            Dim list As New List(Of Production.JobSummary_DBreakdown)
            Try
                Using con
                    con.Open()
                    Using cmd
                        cmd = New SqlCommand("SP_SPC_GetJobSummary_2", con)
                        cmd.CommandType = CommandType.StoredProcedure

                        cmd.Parameters.Add("@TEMPLATEID", SqlDbType.Int).Direction = ParameterDirection.Input
                        cmd.Parameters.Add("@DAYSBACK", SqlDbType.Int).Direction = ParameterDirection.Input
                        cmd.Parameters.Add("@TODATE", SqlDbType.DateTime).Direction = ParameterDirection.Input
                        cmd.Parameters.Add("@LOCATION", SqlDbType.VarChar).Direction = ParameterDirection.Input
                        cmd.Parameters("@TEMPLATEID").Value = TemplateId
                        cmd.Parameters("@DAYSBACK").Value = daysback
                        cmd.Parameters("@TODATE").Value = todate
                        cmd.Parameters("@LOCATION").Value = Location
                        cmd.CommandTimeout = 5000


                        Dim bmapjs As New BMappers(Of Production.JobSummary_DBreakdown)

                        list = bmapjs.GetSpcSP(cmd)

                        

                    End Using
                    con.Close()
                End Using
            Catch ex As Exception
                Return Nothing
            End Try

            Return list

        End Function

        Public Function GetDefectMasterWorkOrderCount(ByVal Location As Integer, ByVal fromdate As DateTime, ByVal todate As DateTime) As Integer
            Dim con As New SqlConnection(DL.InspectConnectionString)
            Dim cmd As SqlCommand = con.CreateCommand()
            Try
                Using con
                    con.Open()
                    Using cmd
                        cmd = New SqlCommand("SP_DefectMasterWorkOrderCount", con)
                        cmd.CommandType = CommandType.StoredProcedure

                        cmd.Parameters.Add("@fromdate", SqlDbType.DateTime).Direction = ParameterDirection.Input
                        cmd.Parameters.Add("@todate", SqlDbType.DateTime).Direction = ParameterDirection.Input
                        cmd.Parameters.Add("@LOCATIONID", SqlDbType.VarChar).Direction = ParameterDirection.Input
                        cmd.Parameters("@fromdate").Value = fromdate
                        cmd.Parameters("@todate").Value = todate
                        cmd.Parameters("@LOCATIONID").Value = Location
                        cmd.CommandTimeout = 5000

                        Dim list As New List(Of SPCInspection.GraphTable)
                        Dim wocheckarray As Array = _DAOFactory.GetDefectMasterDataTypeCount(cmd, con).ToArray()
                        If IsNumeric(wocheckarray(0)) = True Then
                            Return wocheckarray(0)
                        Else
                            Return 0
                        End If

                    End Using
                    con.Close()
                End Using
            Catch ex As Exception
                Return Nothing
            End Try



        End Function

        Public Function GetDefectImage(ByVal DefectId As Integer) As List(Of SPCInspection.DefectMaster)
            Dim returnlist As New List(Of SPCInspection.DefectMaster)
            Dim ImageReader As SqlDataReader
            Dim record As IDataRecord
            Dim sqlstring As String

            sqlstring = "SELECT DefectImage, DefectImage_Filename" & vbCrLf &
            "FROM DefectMaster" & vbCrLf &
            "WHERE (DefectID = @DefectId)"

            Using con As New SqlConnection(DL.InspectConnectionString())
                con.Open()
                Dim cmd As New SqlCommand(sqlstring, con)
                cmd.Parameters.Add(_DAOFactory.Getparameter("@DefectId", DbType.Int32))
                cmd.Parameters("@DefectId").Value = DefectId

                Try
                    ImageReader = cmd.ExecuteReader(CommandBehavior.CloseConnection)

                    If ImageReader.FieldCount > 0 Then
                        While ImageReader.Read
                            record = CType(ImageReader, IDataRecord)

                            If IsDBNull(record(0)) = True Then
                                Return Nothing
                            End If
                            Try
                                returnlist.Add(New SPCInspection.DefectMaster With {.DefectImage = record(0), .DefectImage_Filename = Convert.ToString(record(1))})
                            Catch ex As Exception

                            End Try

                        End While

                        Return returnlist
                    Else
                        Return Nothing
                    End If

                Catch ex As Exception
                    Return Nothing
                End Try



            End Using


        End Function

        Public Function TemplateInsert(ByVal TemplateId As Integer, ByVal Tab_Array As Object, ByVal Button_Array As Object) As Boolean

            For i = 0 To Tab_Array.Count - 1
                If CheckForTab(TemplateId, Tab_Array(i).title, i) = True Then
                    Return False
                    Exit Function
                End If
                Dim returnint As Integer = InsertTab(TemplateId, Tab_Array(i).title, i)
                If returnint = 0 Then
                    Return False
                    Exit Function
                End If
                For Each Button As SPCInspection.buttonarray In Button_Array
                    If Button.tabindex = i Then
                        InsertButton(returnint, Button.ButtonId, Button.text, Button.DefectType, Button.Timer)
                    End If
                Next
            Next
            ActiveTemplate(TemplateId)
            Return True

        End Function

        Public Function TemplateUpdate(ByVal TemplateId As Integer, ByVal Tab_Array As Object, ByVal Button_Array As Object) As Boolean

            For i = 0 To Tab_Array.Count - 1
                Dim TabTemplateId As Integer

                If CheckForTab(TemplateId, Tab_Array(i).title, i) = True And Tab_Array(i).TabTemplateId <> 0 Then
                    'DeleteResult = DeleteButtonTemplate(Tab_Array(i).TabTemplateId)
                    TabTemplateId = Tab_Array(i).TabTemplateId

                    If TabTemplateId > 0 Then
                        For Each Button As SPCInspection.buttonarray In Button_Array
                            If Button.tabindex = i Then
                                'If Not InsertButton(TabTemplateId, Button.ButtonId, Button.text, Button.DefectType) Then
                                '    Return False
                                'End If
                                'If Button.DefectType <> "False" And Button.DefectType <> "True" Then
                                '    Button.DefectType = "True"
                                'End If
                                If Button.id <> -1 Then
                                    Dim sql As String = "UPDATE       ButtonTemplate" & vbCrLf &
                                                        "SET ButtonId = @ButtonId, DefectType = @DefectType, Hide = @Hide, Timer = @Timer " & vbCrLf &
                                                        "WHERE (id = @id)"
                                    Dim btobj As New SPCInspection.ButtonTemplate
                                    btobj.ButtonId = Button.ButtonId
                                    btobj.DefectType = Button.DefectType
                                    btobj.id = Button.id
                                    btobj.Hide = Button.Hide
                                    btobj.Timer = Button.Timer
                                    If Not bmap_1.InsertSpcObject(sql, btobj) Then
                                        Return False
                                    End If
                                Else
                                    Dim sql As String = "INSERT INTO ButtonTemplate" & vbCrLf &
                                                         "(ButtonId, DefectType, TabTemplateId, Timer)" & vbCrLf &
                                                         "VALUES (@ButtonId,@DefectType,@TabTemplateId, @Timer )"
                                    Dim btobj As New SPCInspection.ButtonTemplate
                                    btobj.ButtonId = Button.ButtonId
                                    btobj.DefectType = Button.DefectType
                                    btobj.TabTemplateId = TabTemplateId
                                    btobj.Timer = Button.Timer
                                    If Not bmap_1.InsertSpcObject(sql, btobj) Then
                                        Return False
                                    End If
                                End If

                            End If
                        Next
                    Else
                        Return False
                    End If
                Else
                    TabTemplateId = InsertTab(TemplateId, Tab_Array(i).title, i)

                    If TabTemplateId > 0 Then
                        For Each Button As SPCInspection.buttonarray In Button_Array
                            If Button.tabindex = i Then
                                'If Not InsertButton(TabTemplateId, Button.ButtonId, Button.text, Button.DefectType) Then
                                '    Return False
                                'End If
                                'If Button.DefectType <> "False" And Button.DefectType <> "True" Then
                                '    Button.DefectType = "True"
                                'End If
                                Dim sql As String = "INSERT INTO ButtonTemplate" & vbCrLf &
                                                         "(ButtonId, DefectType, TabTemplateId)" & vbCrLf &
                                                         "VALUES (@ButtonId,@DefectType,@TabTemplateId)"
                                Dim btobj As New SPCInspection.ButtonTemplate
                                btobj.ButtonId = Button.ButtonId
                                btobj.DefectType = Button.DefectType
                                btobj.TabTemplateId = TabTemplateId
                                If Not bmap_1.InsertSpcObject(sql, btobj) Then
                                    Return False
                                End If
                            End If
                        Next
                    End If

                End If


            Next

            Return True

        End Function

        Private Function CheckForTab(ByVal TemplateId As Integer, ByVal Name As String, ByVal TabNumber As Integer) As Boolean
            Dim sqlstring As String
            Dim TabSet As DataSet = New DataSet
            sqlstring = "select * from TabTemplate where TemplateId = '" & TemplateId.ToString() & "' and Name = '" & Name & "'"


            If Not util.FillSPCDataSet(TabSet, "TabSet", sqlstring) Then
                Return False
            End If
            Dim id As Integer = TabSet.Tables(0).Rows.Count

            If id > 0 Then
                Return True
            Else
                Return False
            End If

        End Function

        Public Function CheckSpecTemplate(ByVal TemplateId As Integer) As Boolean
            Dim sqlstring As String
            Dim TabSet As DataSet = New DataSet
            sqlstring = "SELECT        COUNT(SpecMeasurements.SpecId) AS CNT" & vbCrLf &
                        "FROM            SpecMeasurements INNER JOIN" & vbCrLf &
                        "TabTemplate ON SpecMeasurements.TabTemplateId = TabTemplate.TabTemplateId" & vbCrLf &
                        "GROUP BY TabTemplate.TemplateId" & vbCrLf &
                        "HAVING (TabTemplate.TemplateId = " & TemplateId.ToString() & ")"


            If Not util.FillSPCDataSet(TabSet, "TabSet", sqlstring) Then
                Return False
            End If
            Dim id As Integer = TabSet.Tables(0).Rows.Count

            If id > 0 Then
                Dim valuecnt = Convert.ToInt32(TabSet.Tables(0).Rows(0)("CNT"))
                If valuecnt > 0 Then
                    Return True
                Else
                    Return False
                End If
            Else
                Return False
            End If

        End Function


        Public Function InsertSpecMeasurement(ByVal SpecId As Integer, ByVal DefectId As Integer, ByVal InspectionId As Integer, ByVal InspectionJobSummaryId As Integer, ByVal MeasureValue As Decimal, ByVal SpecItemCount As Integer, ByVal SpecDelta As Decimal) As Boolean
            Dim con As New SqlConnection(DL.InspectConnectionString)
            Dim cmd As SqlCommand = con.CreateCommand()
            Dim returnint As Integer
            Dim Sql As String

            Try
                Using con
                    con.Open()
                    Using cmd
                        Sql = "INSERT INTO SpecMeasurements" & vbCrLf &
                         "(SpecId, DefectId, InspectionId, InspectionJobSummaryId, Timestamp, MeasureValue, ItemNumber, SpecDelta)" & vbCrLf &
                         "VALUES (@SpecId, @DefectId, @InspectionId, @InspectionJobSummaryId, GETDATE(),@MeasureValue, @ItemNumber, @SpecDelta)" & vbCrLf &
                         "SELECT SCOPE_IDENTITY();"
                        cmd = _DAOFactory.GetCommand(Sql, con)
                        ''    'Add command parameters                                                                          
                        cmd.Parameters.Add(_DAOFactory.Getparameter("@SpecId", DbType.Int32))
                        cmd.Parameters.Add(_DAOFactory.Getparameter("@DefectId", DbType.Int32))
                        cmd.Parameters.Add(_DAOFactory.Getparameter("@InspectionId", DbType.Int32))
                        cmd.Parameters.Add(_DAOFactory.Getparameter("@InspectionJobSummaryId", DbType.Int32))
                        cmd.Parameters.Add(_DAOFactory.Getparameter("@MeasureValue", DbType.Decimal))
                        cmd.Parameters.Add(_DAOFactory.Getparameter("@ItemNumber", DbType.Int32))
                        cmd.Parameters.Add(_DAOFactory.Getparameter("@SpecDelta", DbType.Decimal))
                        If DefectId = 0 Then
                            cmd.Parameters("@DefectId").Value = 0
                        Else
                            cmd.Parameters("@DefectId").Value = DefectId
                        End If

                        If InspectionId = 0 Then
                            cmd.Parameters("@InspectionId").Value = DBNull.Value
                        Else
                            cmd.Parameters("@InspectionId").Value = InspectionId
                        End If
                        cmd.Parameters("@InspectionJobSummaryId").Value = InspectionJobSummaryId
                        Dim listso As New List(Of SingleObject)
                        If InspectionJobSummaryId > 0 Then
                            Dim bmapso As New BMappers(Of SingleObject)

                            listso = bmapso.GetInspectObject("SELECT CID as Object1 FROM InspectionJobSummary WHERE id = " & InspectionJobSummaryId.ToString())

                        End If
                        cmd.Parameters("@SpecId").Value = SpecId
                        cmd.Parameters("@MeasureValue").Value = MeasureValue
                        cmd.Parameters("@ItemNumber").Value = SpecItemCount
                        cmd.Parameters("@SpecDelta").Value = SpecDelta
                        Try

                            returnint = Convert.ToInt32(cmd.ExecuteScalar())
                            If returnint = 0 Then
                                Return False
                            ElseIf returnint > 0 And listso.Count > 0 Then
                                InspectionInputDAO.RegisterSpecCache(returnint, listso.ToArray()(0).Object1.ToString())
                            End If
                        Catch e As Exception
                            Throw New System.Exception(e.Message)
                            Return False
                            Exit Function
                        End Try
                        con.Close()
                    End Using
                End Using
            Catch ex As Exception
                Return False
            End Try

            Return True
        End Function

        Public Sub UpdateTemplateCollectionCache(ByVal TemplateId As Integer)

            Dim selectValues As New List(Of SPCInspection.TemplateCollection)()
            selectValues = GetInputTemplateCollection(TemplateId)
            If selectValues.Count > 0 Then
                Dim dlayer As New dlayer
                Dim jser As New JavaScriptSerializer
                'Dim CIDArray = dlayer.GetCIDInfo().ToArray()
                If selectValues.Count > 0 Then
                    Dim TemplateCollectionCache As String
                    TemplateCollectionCache = jser.Serialize(selectValues)
                    Dim Cachestring As String = TemplateId.ToString() + "TemplateCollection"
                    dlayer.InsertCacheObject(Cachestring, TemplateCollectionCache, 5)
                End If
            End If
        End Sub

        Public Function GetRollInspectionSummaryHeaders(ByVal datefrom As DateTime, ByVal dateto As DateTime) As List(Of SPCInspection.RollInspectionSummaryHeaders)
            Dim con As New SqlConnection(DL.InspectConnectionString)
            Dim cmd As SqlCommand = con.CreateCommand()


            Dim readerlist As New List(Of SPCInspection.RollInspectionSummaryHeaders)
            Dim Sql As String
            Dim datefromstring As String
            Dim datetostring As String

            datefromstring = datefrom.ToString("MM/dd/yyyy HH:mm:ss", CultureInfo.InvariantCulture)
            datetostring = dateto.ToString("MM/dd/yyyy  HH:mm:ss", CultureInfo.InvariantCulture)

            Sql = "SELECT RollStartTimestamp, LoomNo, RollNumber, Style, Yards_Inspected, DefectiveYards, DHY" & vbCrLf &
                    "FROM RollInspectionSummary" & vbCrLf &
                    "WHERE (RollStartTimestamp >= CONVERT(DATETIME, '" & datefromstring & "', 101)) AND (RollStartTimestamp <= CONVERT(DATETIME, '" & datetostring & "', 101))"

            readerlist = _DAOFactory.getRollInspectionSummaryHeaders(Sql)

            Return readerlist
        End Function

        Public Function GetInspectionSummary(ByVal fromdate As DateTime, ByVal todate As DateTime) As List(Of SPCInspection.InspectionSummaryDisplay)

            Dim listis As New List(Of SPCInspection.InspectionSummaryDisplay)
            Dim bmapis As New BMappers(Of SPCInspection.InspectionSummaryDisplay)

            'Dim sql As String = "SELECT ijs.id, ijs.JobType, ijs.JobNumber, ijs.UnitDesc, SUBSTRING(lm.CID, 4, 3) as CID, lm.Abreviation AS Location, CASE WHEN ijs.Technical_PassFail = 0 THEN  'FAIL' WHEN ijs.Technical_PassFail = 1 THEN  'PASS' END AS Technical_PassFail, CONVERT(VARCHAR(20),  ISNULL(ijs.Inspection_Started, ''), 1) AS STARTED, ISNULL(convert(varchar(20), ijs.Inspection_Finished), '') AS FINISHED, CASE WHEN ijs.SampleSize > 0 THEN (SELECT cast(count(*) as float) FROM DefectMaster WHERE InspectionJobSummaryId = ijs.id) * 100/ijs.SampleSize ELSE 0 END AS DHU, CASE WHEN ijs.SampleSize > 0 THEN (SELECT cast(count(*) as float) FROM DefectMaster WHERE InspectionJobSummaryId = ijs.id AND DefectClass = 'MAJOR') * 100/ijs.SampleSize ELSE 0 END AS RejectionRate, ijs.UnitCost FROM InspectionJobSummary ijs LEFT OUTER JOIN AprManager.dbo.LocationMaster lm on ijs.CID = SUBSTRING(lm.CID, 4, 3) WHERE (ijs.Inspection_Started >= CONVERT(DATETIME, '" & fromdate.ToString("yyyy-MM-dd") & "', 101)) AND (ijs.Inspection_Started <= CONVERT(DATETIME, '" & todate.AddDays(1).ToString("yyyy-MM-dd") & "', 101)) ORDER BY ijs.id DESC"
            'Dim sql As String = "SELECT ijs.id, ijs.JobType, ijs.JobNumber, ijs.UnitDesc, SUBSTRING(lm.CID, 4, 3) as CID, lm.Abreviation AS Location, CASE WHEN ijs.Technical_PassFail = 0 THEN  'FAIL' WHEN ijs.Technical_PassFail = 1 THEN  'PASS' END AS Technical_PassFail, CONVERT(VARCHAR(20),  ISNULL(ijs.Inspection_Started, ''), 1) AS STARTED, ISNULL(convert(varchar(20), ijs.Inspection_Finished), '') AS FINISHED, CASE WHEN ijs.SampleSize > 0 THEN cast((MajorsCount + MinorsCount + RepairsCount + ScrapCount) as float) * 100/ijs.SampleSize ELSE 0 END AS DHU, CASE WHEN ijs.SampleSize > 0 THEN cast((MajorsCount) as float) * 100/ijs.SampleSize ELSE 0 END AS RejectionRate, ijs.UnitCost FROM InspectionJobSummary ijs LEFT OUTER JOIN AprManager.dbo.LocationMaster lm on ijs.CID = SUBSTRING(lm.CID, 4, 3) WHERE (ijs.Inspection_Started >= CONVERT(DATETIME, '" & fromdate.ToString("yyyy-MM-dd") & "', 101)) AND (ijs.Inspection_Started <= CONVERT(DATETIME, '" & todate.AddDays(1).ToString("yyyy-MM-dd") & "', 101)) ORDER BY ijs.id DESC"
            Dim sql As String = "SELECT ijs.id, ijs.JobType, ijs.JobNumber, ijs.DataNo, ijs.UnitDesc, SUBSTRING(lm.CID, 4, 3) as CID, lm.Abreviation AS Location, tn.TemplateId, tn.Name, it.Name as LineType, ijs.TotalInspectedItems, ijs.ItemPassCount, ijs.ItemFailCount, ijs.WOQuantity, ijs.WorkOrderPieces, ijs.AQL_Level, ijs.SampleSize, ijs.RejectLimiter,  CASE WHEN ijs.Technical_PassFail = 0 THEN  'FAIL' WHEN ijs.Technical_PassFail = 1 THEN  'PASS' END AS Technical_PassFail, Inspection_Started AS STARTED, ijs.Inspection_Finished  AS FINISHED, ijs.PRP_Code, CASE WHEN ijs.SampleSize > 0 THEN cast((MajorsCount + MinorsCount + RepairsCount + ScrapCount) as float) * 100/ijs.SampleSize ELSE 0 END AS DHU, CASE WHEN ijs.SampleSize > 0 THEN cast((MajorsCount) as float) * 100/ijs.SampleSize ELSE 0 END AS RejectionRate, ijs.UnitCost, ijs.Comments FROM InspectionJobSummary ijs LEFT OUTER JOIN " + AprManagerDb + ".dbo.LocationMaster lm on ijs.CID = SUBSTRING(lm.CID, 4, 3) LEFT OUTER JOIN TemplateName tn ON ijs.TemplateId = tn.TemplateId LEFT OUTER JOIN InspectionTypes it on it.id = tn.LineTypeId WHERE (ijs.Inspection_Started >= CONVERT(DATETIME, '" & fromdate.ToString("yyyy-MM-dd") & "', 101)) AND (ijs.Inspection_Started <= CONVERT(DATETIME, '" & todate.AddDays(1).ToString("yyyy-MM-dd") & "', 101)) ORDER BY ijs.id DESC"
            listis = bmapis.GetInspectObject(sql)

            Return listis
        End Function

        Public Function GetSpecSummary(ByVal fromdate As DateTime, ByVal todate As DateTime) As List(Of SPCInspection.SpecSummaryDisplay)

            Dim listis As New List(Of SPCInspection.SpecSummaryDisplay)
            Dim bmapis As New BMappers(Of SPCInspection.SpecSummaryDisplay)
            Dim sql As String = "select sq.* from (" & vbCrLf &
                            "SELECT ijs.id, ijs.JobNumber, ijs.UnitDesc, ijs.DataNo, lm.Abreviation AS Location, SUBSTRING(lm.CID, 4, 3) as CID , tn.LineType as LineTypeVariable, it.Name AS LineType, CONVERT(VARCHAR(20),  ISNULL(ijs.Inspection_Started, '')) AS Inspection_Started, ISNULL(convert(varchar(20), ijs.Inspection_Finished), '') AS Inspection_Finished, (SELECT COUNT(*) FROM SpecMeasurements sm WHERE sm.InspectionJobSummaryId = ijs.id) AS totcount, (SELECT COUNT(*) FROM SpecMeasurements sm INNER JOIN ProductSpecification ps ON ps.SpecId = sm.SpecId WHERE (sm.InspectionJobSummaryId = ijs.id) and (sm.SpecDelta <= ps.Upper_Spec_Value) and (sm.SpecDelta >= ps.Lower_Spec_Value)) as SpecsMet, (SELECT COUNT(*) FROM SpecMeasurements sm INNER JOIN ProductSpecification ps ON ps.SpecId = sm.SpecId WHERE (sm.InspectionJobSummaryId = ijs.id) and (sm.SpecDelta > ps.Upper_Spec_Value or sm.SpecDelta < ps.Lower_Spec_Value)) as SpecsFailed FROM InspectionJobSummary ijs LEFT OUTER JOIN " + AprManagerDb + ".dbo.LocationMaster lm on ijs.CID = SUBSTRING(lm.CID, 4, 3) LEFT OUTER JOIN TemplateName tn ON tn.TemplateId = ijs.TemplateId  LEFT OUTER JOIN InspectionTypes it on it.id = tn.LineTypeId WHERE (ijs.Inspection_Started <= CONVERT(DATETIME, '" & todate.AddDays(1).ToString("yyyy-MM-dd") & "', 101)) AND (ijs.Inspection_Started >= CONVERT(DATETIME, '" & fromdate.ToString("yyyy-MM-dd") & "', 101))" & vbCrLf &
                            ") sq where sq.totcount > 0 order by sq.Inspection_Started desc"

            listis = bmapis.GetInspectObject(sql)

            Return listis
        End Function

        Public Function getDumpData(ByVal fromdate As DateTime, ByVal todate As DateTime) As List(Of SPCInspection.Dump)
            Dim listis As New List(Of SPCInspection.Dump)
            Dim bmapis As New BMappers(Of SPCInspection.Dump)
            Dim sql As String = "select * from dbo.INS_Summary_VW WHERE (Inspection_Started >= CONVERT(DATETIME, '" & fromdate.ToString("yyyy-MM-dd") & "', 101)) AND (Inspection_Started <= CONVERT(DATETIME, '" & todate.AddDays(1).ToString("yyyy-MM-dd") & "', 101))  order by id desc"

            listis = bmapis.GetInspectObject(sql)

            Return listis

        End Function

        Public Function getDfectTimerReport(ByVal fromdate As DateTime, ByVal todate As DateTime) As List(Of SPCInspection.TimerReport)
            Dim listdtr As New List(Of SPCInspection.TimerReport)
            Dim bmapdtr As New BMappers(Of SPCInspection.TimerReport)
            Dim sql As String = "select ijs.JobType, ijs.JobNumber, lm.Name AS Location, lm.CID, ijs.DataNo, ijs.UnitDesc, bl.Name as DefectName, bt.DefectType, dm.EmployeeNo, dt.Timestamp, dt.StopTimestamp, ISNULL(DATEDIFF(MINUTE, dt.Timestamp, StopTimestamp), 0) as Timespan_min from DefectTimer dt INNER JOIN InspectionJobSummary ijs on dt.InspectionJobSummaryId = ijs.id INNER JOIN ButtonTemplate bt ON dt.ButtonTemplateId = bt.id inner join ButtonLibrary bl ON bt.ButtonId = bl.ButtonId inner join " + AprManagerDb + ".dbo.LocationMaster lm ON ijs.CID = substring(lm.CID, 4,3) inner join DefectMaster dm on dt.DefectID = dm.DefectID where dt.Timestamp <= CONVERT(DATETIME, '" + todate.AddDays(1).ToString("yyyy-MM-dd") + "') AND dt.Timestamp >= CONVERT(DATETIME, '" + fromdate.ToString("yyyy-MM-dd") + "')"

            listdtr = bmapdtr.GetInspectObject(sql)

            Return listdtr
        End Function

        Public Function GetSpecSummaryUnFinished(ByVal fromdate As DateTime, ByVal todate As DateTime) As List(Of SPCInspection.SpecSummaryDisplay)

            Dim listis As New List(Of SPCInspection.SpecSummaryDisplay)
            Dim bmapis As New BMappers(Of SPCInspection.SpecSummaryDisplay)
            Dim sql As String = "select sq.* from (" & vbCrLf &
                            "SELECT ijs.id, ijs.JobNumber, ijs.UnitDesc, ijs.DataNo, lm.Abreviation AS Location, SUBSTRING(lm.CID, 4, 3) as CID , CONVERT(VARCHAR(20),  ISNULL(ijs.Inspection_Started, '')) AS Inspection_Started, ISNULL(convert(varchar(20), ijs.Inspection_Finished), '') AS Inspection_Finished, (SELECT COUNT(*) FROM SpecMeasurements sm WHERE sm.InspectionJobSummaryId = ijs.id) AS totcount, (SELECT COUNT(*) FROM SpecMeasurements sm INNER JOIN ProductSpecification ps ON ps.SpecId = sm.SpecId WHERE (sm.InspectionJobSummaryId = ijs.id) and (sm.SpecDelta <= ps.Upper_Spec_Value) and (sm.SpecDelta >= ps.Lower_Spec_Value)) as SpecsMet, (SELECT COUNT(*) FROM SpecMeasurements sm INNER JOIN ProductSpecification ps ON ps.SpecId = sm.SpecId WHERE (sm.InspectionJobSummaryId = ijs.id) and (sm.SpecDelta > ps.Upper_Spec_Value or sm.SpecDelta < ps.Lower_Spec_Value)) as SpecsFailed FROM InspectionJobSummary ijs LEFT OUTER JOIN " + AprManagerDb + ".dbo.LocationMaster lm on ijs.CID = SUBSTRING(lm.CID, 4, 3) WHERE (ijs.Inspection_Started <= CONVERT(DATETIME, '" & todate.AddDays(1).ToString("yyyy-MM-dd") & "', 101)) AND (ijs.Inspection_Started >= CONVERT(DATETIME, '" & fromdate.ToString("yyyy-MM-dd") & "', 101)) AND (ijs.Inspection_Finished is null)" & vbCrLf &
                            ") sq where sq.totcount > 0 order by sq.Inspection_Started desc"

            listis = bmapis.GetInspectObject(sql)

            Return listis
        End Function

        Public Function GetInspectionSummaryDay(ByVal fromdate As DateTime, ByVal todate As DateTime) As List(Of SPCInspection.InspectionSummaryDisplay)

            Dim listis As New List(Of SPCInspection.InspectionSummaryDisplay)
            Dim bmapis As New BMappers(Of SPCInspection.InspectionSummaryDisplay)
            'Dim sql As String = "SELECT ijs.id, ijs.JobType, ijs.JobNumber, ijs.UnitDesc, SUBSTRING(lm.CID, 4, 3) as CID, lm.Abreviation AS Location, CASE WHEN ijs.Technical_PassFail = 0 THEN  'FAIL' WHEN ijs.Technical_PassFail = 1 THEN  'PASS' END AS Technical_PassFail, CONVERT(VARCHAR(20),  ISNULL(ijs.Inspection_Started, ''), 1) AS STARTED, ISNULL(convert(varchar(20), ijs.Inspection_Finished), '') AS FINISHED, CASE WHEN ijs.SampleSize > 0 THEN (SELECT cast(count(*) as float) FROM DefectMaster WHERE InspectionJobSummaryId = ijs.id) * 100/ijs.SampleSize ELSE 0 END AS DHU, CASE WHEN ijs.SampleSize > 0 THEN (SELECT cast(count(*) as float) FROM DefectMaster WHERE InspectionJobSummaryId = ijs.id AND DefectClass = 'MAJOR') * 100/ijs.SampleSize ELSE 0 END AS RejectionRate, ijs.UnitCost FROM InspectionJobSummary ijs LEFT OUTER JOIN AprManager.dbo.LocationMaster lm on ijs.CID = SUBSTRING(lm.CID, 4, 3) WHERE (ijs.Inspection_Started >= CONVERT(DATETIME, '" & fromdate.ToString("yyyy-MM-dd") & "', 101)) AND (ijs.Inspection_Started < CONVERT(DATETIME, '" & todate.AddDays(1).ToString("yyyy-MM-dd") & "', 101)) ORDER BY ijs.id DESC"
            'Dim sql As String = "SELECT ijs.id, ijs.JobType, ijs.JobNumber, ijs.UnitDesc, SUBSTRING(lm.CID, 4, 3) as CID, lm.Abreviation AS Location, CASE WHEN ijs.Technical_PassFail = 0 THEN  'FAIL' WHEN ijs.Technical_PassFail = 1 THEN  'PASS' END AS Technical_PassFail, CONVERT(VARCHAR(20),  ISNULL(ijs.Inspection_Started, ''), 1) AS STARTED, ISNULL(convert(varchar(20), ijs.Inspection_Finished), '') AS FINISHED, CASE WHEN ijs.SampleSize > 0 THEN cast((MajorsCount + MinorsCount + RepairsCount + ScrapCount) as float) * 100/ijs.SampleSize ELSE 0 END AS DHU, CASE WHEN ijs.SampleSize > 0 THEN cast((MajorsCount) as float) * 100/ijs.SampleSize ELSE 0 END AS RejectionRate, ijs.UnitCost FROM InspectionJobSummary ijs LEFT OUTER JOIN AprManager.dbo.LocationMaster lm on ijs.CID = SUBSTRING(lm.CID, 4, 3) WHERE (ijs.Inspection_Started >= CONVERT(DATETIME, '" & fromdate.ToString("yyyy-MM-dd") & "', 101)) AND (ijs.Inspection_Started <= CONVERT(DATETIME, '" & todate.AddDays(1).ToString("yyyy-MM-dd") & "', 101)) ORDER BY ijs.id DESC"
            Dim sql As String = "SELECT ijs.id AS ijsid, ijs.JobType, ijs.JobNumber, ijs.DataNo, ijs.UnitDesc, SUBSTRING(lm.CID, 4, 3) as CID, lm.Abreviation AS Location, tn.TemplateId, tn.Name, tn.LineType AS LineTypeVariable, it.Name as LineType, ijs.TotalInspectedItems, ijs.ItemPassCount, ijs.ItemFailCount, ijs.WOQuantity, ijs.WorkOrderPieces, ijs.AQL_Level, ijs.SampleSize, ijs.RejectLimiter,  CASE WHEN ijs.Technical_PassFail = 0 THEN  'FAIL' WHEN ijs.Technical_PassFail = 1 THEN  'PASS' END AS Technical_PassFail, Inspection_Started AS STARTED, ijs.Inspection_Finished  AS FINISHED, ijs.PRP_Code, ijs.MajorsCount, ijs.MinorsCount, ijs.ScrapCount, ijs.RepairsCount, CASE WHEN ijs.SampleSize > 0 THEN cast((MajorsCount + MinorsCount + RepairsCount + ScrapCount) as float) * 100/ijs.SampleSize ELSE 0 END AS DHU, CASE WHEN ijs.SampleSize > 0 THEN cast((MajorsCount) as float) * 100/ijs.SampleSize ELSE 0 END AS RejectionRate, ijs.UnitCost, ijs.Comments FROM InspectionJobSummary ijs LEFT OUTER JOIN " + AprManagerDb + ".dbo.LocationMaster lm on ijs.CID = SUBSTRING(lm.CID, 4, 3) LEFT OUTER JOIN TemplateName tn ON ijs.TemplateId = tn.TemplateId LEFT OUTER JOIN InspectionTypes it ON tn.LineTypeId = it.id WHERE (ijs.Inspection_Started >= CONVERT(DATETIME, '" & fromdate.ToString("yyyy-MM-dd") & "', 101)) AND (ijs.Inspection_Started <= CONVERT(DATETIME, '" & todate.AddDays(1).ToString("yyyy-MM-dd") & "', 101)) ORDER BY ijs.id DESC"

            listis = bmapis.GetInspectObject(sql)

            Return listis
        End Function

        Public Function GetInspectionSummaryDayUnFinished(ByVal fromdate As DateTime, ByVal todate As DateTime) As List(Of SPCInspection.InspectionSummaryDisplay)

            Dim listis As New List(Of SPCInspection.InspectionSummaryDisplay)
            Dim bmapis As New BMappers(Of SPCInspection.InspectionSummaryDisplay)
            Dim sql As String = "SELECT ijs.id AS ijsid, ijs.JobType, ijs.JobNumber, ijs.DataNo, ijs.UnitDesc, SUBSTRING(lm.CID, 4, 3) as CID, lm.Abreviation AS Location, tn.TemplateId, tn.Name, tn.LineType AS LineTypeVariable, it.Name AS LineType, ijs.TotalInspectedItems, ijs.ItemPassCount, ijs.ItemFailCount, ijs.WOQuantity, ijs.WorkOrderPieces, ijs.AQL_Level, ijs.SampleSize, ijs.RejectLimiter,  CASE WHEN ijs.Technical_PassFail = 0 THEN  'FAIL' WHEN ijs.Technical_PassFail = 1 THEN  'PASS' END AS Technical_PassFail, Inspection_Started AS STARTED, ijs.Inspection_Finished  AS FINISHED, ijs.PRP_Code, CASE WHEN ijs.SampleSize > 0 THEN cast((MajorsCount + MinorsCount + RepairsCount + ScrapCount) as float) * 100/ijs.SampleSize ELSE 0 END AS DHU, CASE WHEN ijs.SampleSize > 0 THEN cast((MajorsCount) as float) * 100/ijs.SampleSize ELSE 0 END AS RejectionRate, ijs.UnitCost, ijs.Comments FROM InspectionJobSummary ijs LEFT OUTER JOIN " + AprManagerDb + ".dbo.LocationMaster lm on ijs.CID = SUBSTRING(lm.CID, 4, 3) LEFT OUTER JOIN TemplateName tn ON ijs.TemplateId = tn.TemplateId LEFT OUTER JOIN InspectionTypes it on it.id = tn.LineTypeId WHERE (ijs.Inspection_Started >= CONVERT(DATETIME, '" & fromdate.ToString("yyyy-MM-dd") & "', 101)) AND (ijs.Inspection_Started < CONVERT(DATETIME, '" & todate.AddDays(1).ToString("yyyy-MM-dd") & "', 101)) AND (ijs.Inspection_Finished is null) ORDER BY ijs.id DESC"
            Try
                listis = bmapis.GetInspectObject(sql)
            Catch ex As Exception

            End Try

            Return listis
        End Function


        Public Function GetRollInspectionDetailTable(ByVal datefrom As DateTime, ByVal dateto As DateTime) As List(Of SPCInspection.RollInspectionDetailTable)
            Dim con As New SqlConnection(DL.InspectConnectionString)
            Dim cmd As SqlCommand = con.CreateCommand()
            Dim corereader As SqlDataReader
            Dim record As IDataRecord
            Dim readerlist As New List(Of SPCInspection.RollInspectionDetailTable)

            Try
                Using con
                    con.Open()
                    Using cmd
                        cmd = New SqlCommand("SP_SPC_GetGreigeReportDetail", con)
                        cmd.CommandType = CommandType.StoredProcedure
                        ''    'Add command parameters                                                                          
                        cmd.Parameters.Add("@DATESTART", SqlDbType.DateTime).Direction = ParameterDirection.Input
                        cmd.Parameters.Add("@DATEEND", SqlDbType.DateTime).Direction = ParameterDirection.Input


                        cmd.Parameters("@DATESTART").Value = datefrom
                        cmd.Parameters("@DATEEND").Value = dateto

                        Try

                            corereader = cmd.ExecuteReader(CommandBehavior.CloseConnection)

                            If corereader.FieldCount > 0 And corereader.HasRows = True Then
                                While corereader.Read
                                    record = CType(corereader, IDataRecord)
                                    readerlist.Add(New SPCInspection.RollInspectionDetailTable With {.ButtonId = Convert.ToInt32(record(0)), .Text = Convert.ToString(record(1)), .ShiftNumber = Convert.ToInt32(record(2)), .DHY = Convert.ToDecimal(record(3)), .DefectCount = Convert.ToInt32(record(4)), .RSID = Convert.ToInt32(record(5))})
                                End While

                                Return readerlist
                            Else
                                Return readerlist
                            End If
                        Catch e As Exception
                            Throw New System.Exception(e.Message)
                            Return Nothing
                            Exit Function
                        End Try
                        con.Close()
                    End Using
                End Using
            Catch ex As Exception
                Return Nothing
            End Try

            Return readerlist

        End Function

        Public Function GetDefectImageDisplay(ByVal fromdate As DateTime, ByVal todate As DateTime, Optional ByVal DefectId As Integer = Nothing) As List(Of SPCInspection.DefectImageDisplay_)
            Dim sqlstring As String
            Dim bmap As New BMappers(Of SPCInspection.DefectImageDisplay_)
            Dim retlist As New List(Of SPCInspection.DefectImageDisplay_)



            sqlstring = "select dm.DefectID as DefectID_, dm.DefectTime as DefectTime_, dm.Location as Location_, dm.DefectDesc as DefectDesc_, ijs.Inspection_Started as Inspection_Started_, ijs.JobNumber as JobNumber_, ijs.DataNo as DataNo_, ijs.PRP_Code as Prp_Code, dm.DefectImage as Image, UnitDesc as UnitDesc_, it.Name as AuditType from DefectMaster dm" & vbCrLf &
                            "INNER JOIN InspectionJobSummary ijs ON dm.InspectionJobSummaryId = ijs.id" & vbCrLf &
                            "INNER JOIN TemplateName tn ON ijs.TemplateId = tn.TemplateId" & vbCrLf &
                            "INNER JOIN InspectionTypes it ON rtrim(tn.LineType) = it.Abreviation" & vbCrLf &
                            "WHERE ijs.Inspection_Started >= cast('" + fromdate.ToShortDateString() + " " + fromdate.ToShortTimeString() + "' as datetime) and ijs.Inspection_Started <= cast('" + todate.ToShortDateString() + " " + todate.ToShortTimeString() + "' as datetime) and dm.DefectImage is not null "

            If DefectId > 0 Then
                sqlstring = sqlstring + " and dm.DefectID > " + DefectId.ToString()
            End If
            retlist = bmap.GetInspectObject(sqlstring)

            Return retlist

        End Function

        Public Function GetDefectImageDescList(ByVal fromdate As DateTime, ByVal todate As DateTime) As List(Of SPCInspection.DefectImageDisplay_)
            Dim sqlstring As String
            Dim bmap As New BMappers(Of SPCInspection.DefectImageDisplay_)
            Dim retlist As New List(Of SPCInspection.DefectImageDisplay_)



            sqlstring = " select dm.Location as Location_, dm.DefectDesc as DefectDesc_, ijs.DataNo as DataNo_, ijs.PRP_Code as Prp_Code, UnitDesc as UnitDesc_, it.Name as AuditType from DefectMaster dm" & vbCrLf &
                            "INNER JOIN InspectionJobSummary ijs ON dm.InspectionJobSummaryId = ijs.id" & vbCrLf &
                            "INNER JOIN TemplateName tn ON ijs.TemplateId = tn.TemplateId" & vbCrLf &
                            "INNER JOIN InspectionTypes it ON rtrim(tn.LineType) = it.Abreviation" & vbCrLf &
                            "WHERE ijs.Inspection_Started >= cast('" + fromdate.ToShortDateString() + " " + fromdate.ToShortTimeString() + "' as datetime) and ijs.Inspection_Started <= cast('" + todate.ToShortDateString() + " " + todate.ToShortTimeString() + "' as datetime) and dm.DefectImage is not null "

            retlist = bmap.GetInspectObject(sqlstring)

            Return retlist

        End Function
        'Public Function GetWorkOrderdata() As Object
        '    Dim conn As New iDB2Connection("Datasource=Prod;userid=SUCCO_T;password=SUCCO_T;")
        '    Dim cmd As New iDB2Command("select * from devdatas.f4801 where  WADOCO = 1447255", conn)
        '    Dim record As IDataRecord
        '    Dim dr As iDB2DataReader
        '    Dim itemlist As New List(Of Object)





        'Using connection As New iDB2Connection("Datasource=Prod;userid=PCSUSER;password=PCSUSER;")

        '    Using Command As New iDB2Command("select * from stcdata.f4801 where  WADOCO = 1447255", connection)
        '        Dim dr As iDB2DataReader
        '        Dim record As IDataRecord
        '        dr = Command.ExecuteReader()
        '        While dr.Read
        '            record = CType(dr, IDataRecord)
        '            Dim test As Object = record


        '        End While


        '    End Using

        'End Using


        'End Function

    End Class





End Namespace
